﻿// Copyright(c) Microsoft Corporation
// All rights reserved.
//
// Licensed under the Apache License, Version 2.0 (the License); you may not use
// this file except in compliance with the License. You may obtain a copy of the
// License at http://www.apache.org/licenses/LICENSE-2.0
//
// THIS CODE IS PROVIDED ON AN  *AS IS* BASIS, WITHOUT WARRANTIES OR CONDITIONS
// OF ANY KIND, EITHER EXPRESS OR IMPLIED, INCLUDING WITHOUT LIMITATION ANY
// IMPLIED WARRANTIES OR CONDITIONS OF TITLE, FITNESS FOR A PARTICULAR PURPOSE,
// MERCHANTABILITY OR NON-INFRINGEMENT.
//
// See the Apache Version 2.0 License for specific language governing
// permissions and limitations under the License.

using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Microsoft.Python.Analysis.Modules;
using Microsoft.Python.Analysis.Values;
using Microsoft.Python.Core;
using Microsoft.Python.Core.Collections;
using Microsoft.Python.Core.Diagnostics;
using Microsoft.Python.Parsing.Ast;

namespace Microsoft.Python.Analysis.Types {
    [DebuggerDisplay("Function {Name} ({TypeId})")]
    internal sealed class PythonFunctionType : PythonType, IPythonFunctionType {
        private static readonly IReadOnlyList<string> DefaultClassMethods = new[] { "__new__", "__init_subclass__", "__class_getitem__" };
        private ImmutableArray<IPythonFunctionOverload> _overloads = ImmutableArray<IPythonFunctionOverload>.Empty;
        private bool _isAbstract;
        private bool _isSpecialized;

        /// <summary>
        /// Creates function for specializations
        /// </summary>
        public static PythonFunctionType Specialize(string name, IPythonModule declaringModule, string documentation)
            => new PythonFunctionType(name, new Location(declaringModule), documentation, true);

        private PythonFunctionType(string name, Location location, string documentation, bool isSpecialized = false) :
            base(name, location, documentation ?? string.Empty, BuiltinTypeId.Function) {
            _isSpecialized = isSpecialized;
        }

        /// <summary>
        /// Creates function type to use in special cases when function is dynamically
        /// created, such as in specializations and custom iterators, without the actual
        /// function definition in the AST.
        /// </summary>
        public PythonFunctionType(
            string name,
            Location location,
            IPythonType declaringType,
            string documentation
        ) : base(name, location, documentation, declaringType is IPythonClassType ? BuiltinTypeId.Method : BuiltinTypeId.Function) {
            DeclaringType = declaringType;
        }

        public PythonFunctionType(
            FunctionDefinition fd,
            IPythonType declaringType,
            Location location
        ) : base(fd.Name, location,
            fd.Name == "__init__" ? (declaringType?.Documentation ?? fd.GetDocumentation()) : fd.GetDocumentation(), 
            declaringType is IPythonClassType ? BuiltinTypeId.Method : BuiltinTypeId.Function) {
            DeclaringType = declaringType;
            
            // IsStub must be set permanently so when location of the stub is reassigned
            // to the primary module for navigation purposes, function still remembers
            // that it case from a stub.
            IsStub = location.Module.ModuleType == ModuleType.Stub;

            location.Module.AddAstNode(this, fd);
            ProcessDecorators(fd);
            DecideClassMethod();
        }

        #region IPythonType
        public override PythonMemberType MemberType
            => TypeId == BuiltinTypeId.Function ? PythonMemberType.Function : PythonMemberType.Method;

        public override string QualifiedName => this.GetQualifiedName();

        public override IMember Call(IPythonInstance instance, string memberName, IArgumentSet args) {
            // Now we can go and find overload with matching arguments.
            var overload = Overloads[args.OverloadIndex];
            return overload?.Call(args, instance?.GetPythonType() ?? DeclaringType);
        }

        internal override void SetDocumentation(string documentation) {
            foreach (var o in Overloads) {
                (o as PythonFunctionOverload)?.SetDocumentation(documentation);
            }
            base.SetDocumentation(documentation);
        }

        #endregion

        #region IPythonFunction
        public FunctionDefinition FunctionDefinition => DeclaringModule.GetAstNode<FunctionDefinition>(this);
        public IPythonType DeclaringType { get; }
        public override string Documentation => (_overloads.Count > 0 ? _overloads[0].Documentation : default) ?? base.Documentation;
        public bool IsClassMethod { get; private set; }
        public bool IsStatic { get; private set; }
        public override bool IsAbstract => _isAbstract;
        public override bool IsSpecialized => _isSpecialized;

        public bool IsOverload { get; private set; }
        public bool IsStub { get; }
        public bool IsUnbound => DeclaringType == null;

        public IReadOnlyList<IPythonFunctionOverload> Overloads => _overloads;
        #endregion

        internal void Specialize(string[] dependencies) {
            _isSpecialized = true;
            Dependencies = dependencies != null
                ? ImmutableArray<string>.Create(dependencies)
                : ImmutableArray<string>.Empty;
        }

        internal ImmutableArray<string> Dependencies { get; private set; } = ImmutableArray<string>.Empty;

        internal void AddOverload(IPythonFunctionOverload overload)
            => _overloads = _overloads.Count > 0 ? _overloads.Add(overload) : ImmutableArray<IPythonFunctionOverload>.Create(overload);

        internal IPythonFunctionType ToUnbound() {
            Debug.Assert(DeclaringType != null, "Attempt to unbound standalone function.");
            return new PythonUnboundMethod(this);
        }

        private void DecideClassMethod() {
            if (IsClassMethod) {
                return;
            }

            IsClassMethod = DefaultClassMethods.Contains(Name);
        }

        private void ProcessDecorators(FunctionDefinition fd) {
            // TODO warn about incompatible combinations, e.g @staticmethod + @classmethod
            foreach (var dec in (fd.Decorators?.Decorators).MaybeEnumerate().OfType<NameExpression>()) {
                switch (dec.Name) {
                    case @"staticmethod":
                        IsStatic = true;
                        break;
                    case @"classmethod":
                        IsClassMethod = true;
                        break;
                    case @"abstractmethod":
                        _isAbstract = true;
                        break;
                    case @"abstractstaticmethod":
                        IsStatic = true;
                        _isAbstract = true;
                        break;
                    case @"abstractclassmethod":
                        IsClassMethod = true;
                        _isAbstract = true;
                        break;
                    case @"overload":
                        IsOverload = true;
                        break;
                    case @"property":
                    case @"abstractproperty":
                        // Ignore property decorators if the declaring type is unknown.
                        // TODO: Restore this to a hard failure once property can handle not having a declaring type.
                        Debug.Assert(DeclaringType.IsUnknown(), "Found property attribute while processing function. Properties should be handled in the respective class.");
                        break;
                }
            }
        }

        /// <summary>
        /// Represents unbound method, such in C.f where C is class rather than the instance.
        /// </summary>
        internal sealed class PythonUnboundMethod : PythonTypeWrapper, IPythonFunctionType {
            public PythonUnboundMethod(IPythonFunctionType function) : base(function, function.DeclaringModule) {
                Function = function;
            }

            public IPythonFunctionType Function { get; }
            public FunctionDefinition FunctionDefinition => Function.FunctionDefinition;
            public IPythonType DeclaringType => Function.DeclaringType;
            public bool IsStatic => Function.IsStatic;
            public bool IsClassMethod => Function.IsClassMethod;
            public bool IsOverload => Function.IsOverload;
            public bool IsStub => Function.IsStub;
            public bool IsUnbound => true;

            public IReadOnlyList<IPythonFunctionOverload> Overloads => Function.Overloads;
            public override BuiltinTypeId TypeId => BuiltinTypeId.Function;
            public override PythonMemberType MemberType => PythonMemberType.Function;
        }
    }
}
