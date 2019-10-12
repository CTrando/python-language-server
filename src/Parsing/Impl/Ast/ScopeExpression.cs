// Copyright(c) Microsoft Corporation
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

namespace Microsoft.Python.Parsing.Ast {
    public abstract class ScopeExpression : Expression, IBindableNode {
        protected void Clear() => ScopeDelegate.Clear();
        
        #region IScopeNode
        public IScopeNode ParentScopeNode { get; set; }

        public virtual Statement Body { get; }

        public virtual bool HasLateBoundVariableSets { get; set; }
        
        public bool IsClosure => ScopeDelegate.IsClosure;

        IReadOnlyList<PythonVariable> IScopeNode.ScopeVariables => ScopeDelegate.ScopeVariables;

        public bool ContainsNestedFreeVariables { get; set; }

        public bool NeedsLocalsDictionary { get; set; }

        public virtual string Name => "<unknown>";

        public bool TryGetVariable(string name, out PythonVariable variable) => ScopeDelegate.TryGetVariable(name, out variable);

        public bool IsGlobal => ScopeDelegate.IsGlobal;

        public PythonAst GlobalParent => ScopeDelegate.Ast;
        bool IScopeNode.ContainsNestedFreeVariables { get; set; }

        public IReadOnlyList<PythonVariable> FreeVariables => ScopeDelegate.FreeVariables;
        
        #endregion
        
        internal abstract ScopeDelegate ScopeDelegate { get; }

        #region IBindableNode
        void IBindableNode.Bind(PythonNameBinder binder) => ScopeDelegate.Bind(binder);

        void IBindableNode.FinishBind(PythonNameBinder binder) => ScopeDelegate.FinishBind(binder);

        bool IBindableNode.TryBindOuter(IBindableNode from, string name, bool allowGlobals, out PythonVariable variable)
            => ScopeDelegate.TryBindOuter(from, name, allowGlobals, out variable);
        
        void IBindableNode.AddVariable(PythonVariable variable) => ScopeDelegate.AddVariable(variable);

        void IBindableNode.AddFreeVariable(PythonVariable variable, bool accessedInScope) => ScopeDelegate.AddFreeVariable(variable, accessedInScope);

        string IBindableNode.AddReferencedGlobal(string name) => ScopeDelegate.AddReferencedGlobal(name);

        void IBindableNode.AddNonLocalVariable(NameExpression name) => ScopeDelegate.AddNonLocalVariable(name);

        void IBindableNode.AddCellVariable(PythonVariable variable) => ScopeDelegate.AddCellVariable(variable);

        bool IBindableNode.ExposesLocalVariable(PythonVariable name) => ScopeDelegate.ExposesLocalVariable(name);

        PythonVariable IBindableNode.BindReference(PythonNameBinder binder, string name) => ScopeDelegate.BindReference(binder, name);

        PythonReference IBindableNode.Reference(string name) => ScopeDelegate.Reference(name);

        bool IBindableNode.IsReferenced(string name) => ScopeDelegate.IsReferenced(name);

        PythonVariable IBindableNode.CreateVariable(string name, VariableKind kind) => ScopeDelegate.CreateVariable(name, kind);

        PythonVariable IBindableNode.EnsureVariable(string name) => ScopeDelegate.EnsureVariable(name);
        
        PythonVariable IBindableNode.EnsureGlobalVariable(string name) => ScopeDelegate.EnsureGlobalVariable(name);

        PythonVariable IBindableNode.DefineParameter(string name) => ScopeDelegate.DefineParameter(name);

        bool IBindableNode.ContainsImportStar { get; set; }
        bool IBindableNode.ContainsExceptionHandling { get; set; }
        bool IBindableNode.ContainsUnqualifiedExec { get; set; }
        #endregion
    }
}
