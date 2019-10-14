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
using System.Linq;
using Microsoft.Python.Analysis.Types;
using Microsoft.Python.Analysis.Types.Collections;
using Microsoft.Python.Analysis.Values;
using Microsoft.Python.Analysis.Values.Collections;
using Microsoft.Python.Core.Disposables;
using Microsoft.Python.Parsing;
using Microsoft.Python.Parsing.Ast;

namespace Microsoft.Python.Analysis.Analyzer.Evaluation {
    internal sealed partial class ExpressionEval {
        private const int MaxCollectionSize = 1000;

        public IMember GetValueFromIndex(IndexExpression expr, LookupOptions lookupOptions = LookupOptions.Normal) {
            if (expr?.Target == null) {
                return null;
            }

            var target = GetValueFromExpression(expr.Target, lookupOptions);
            // Try generics first since this may be an expression like Dict[int, str]
            var result = GetValueFromGeneric(target, expr, lookupOptions);
            if (result != null) {
                return result;
            }

            if (expr.Index is SliceExpression || expr.Index is TupleExpression) {
                // When slicing, assume result is the same type
                return target;
            }

            var type = target.GetPythonType();
            if (type != null) {
                if (!(target is IPythonInstance instance)) {
                    instance = type.CreateInstance(ArgumentSet.Empty(expr, this));
                }
                var index = GetValueFromExpression(expr.Index, lookupOptions);
                if (index != null) {
                    return type.Index(instance, new ArgumentSet(new[] { index }, expr, this));
                }
            }

            return UnknownType;
        }

        public IMember GetValueFromList(ListExpression expression, LookupOptions lookupOptions = LookupOptions.Normal) {
            var contents = new List<IMember>();
            foreach (var item in expression.Items.Take(MaxCollectionSize)) {
                var value = GetValueFromExpression(item, lookupOptions) ?? UnknownType;
                contents.Add(value);
            }
            return PythonCollectionType.CreateList(Module, contents, exact: expression.Items.Count <= MaxCollectionSize);
        }

        public IMember GetValueFromDictionary(DictionaryExpression expression, LookupOptions lookupOptions = LookupOptions.Normal) {
            var contents = new Dictionary<IMember, IMember>();
            foreach (var item in expression.Items.Take(MaxCollectionSize)) {
                var key = GetValueFromExpression(item.SliceStart, lookupOptions) ?? UnknownType;
                var value = GetValueFromExpression(item.SliceStop, lookupOptions) ?? UnknownType;
                contents[key] = value;
            }
            return new PythonDictionary(Module, contents, exact: expression.Items.Count <= MaxCollectionSize);
        }

        private IMember GetValueFromTuple(TupleExpression expression, LookupOptions lookupOptions = LookupOptions.Normal) {
            var contents = new List<IMember>();
            foreach (var item in expression.Items.Take(MaxCollectionSize)) {
                var value = GetValueFromExpression(item, lookupOptions) ?? UnknownType;
                contents.Add(value);
            }
            return PythonCollectionType.CreateTuple(Module, contents, exact: expression.Items.Count <= MaxCollectionSize);
        }

        public IMember GetValueFromSet(SetExpression expression, LookupOptions lookupOptions = LookupOptions.Normal) {
            var contents = new List<IMember>();
            foreach (var item in expression.Items.Take(MaxCollectionSize)) {
                var value = GetValueFromExpression(item, lookupOptions) ?? UnknownType;
                contents.Add(value);
            }
            return PythonCollectionType.CreateSet(Module, contents, exact: expression.Items.Count <= MaxCollectionSize);
        }

        public IMember GetValueFromGenerator(GeneratorExpression expression, LookupOptions lookupOptions = LookupOptions.Normal) {
            var iter = expression.Iterators.OfType<ComprehensionFor>().FirstOrDefault();
            if (iter != null) {
                return GetValueFromExpression(iter.List, lookupOptions) ?? UnknownType;
            }
            return UnknownType;
        }

        public IMember GetValueFromComprehension(Comprehension node) {
            using (OpenScope(Module, node)) {
                ProcessComprehension(node);
                // TODO: Evaluate comprehensions to produce exact contents, if possible.
                switch (node) {
                    case ListComprehension lc:
                        var v1 = GetValueFromExpression(lc.Item, lookupOptions) ?? UnknownType;
                        return PythonCollectionType.CreateList(Module, new[] { v1 });
                    case SetComprehension sc:
                        var v2 = GetValueFromExpression(sc.Item, lookupOptions) ?? UnknownType;
                        return PythonCollectionType.CreateSet(Module, new[] { v2 });
                    case DictionaryComprehension dc:
                        var k = GetValueFromExpression(dc.Key, lookupOptions) ?? UnknownType;
                        var v = GetValueFromExpression(dc.Value, lookupOptions) ?? UnknownType;
                        return new PythonDictionary(new PythonDictionaryType(Interpreter.ModuleResolution.BuiltinsModule), new Dictionary<IMember, IMember> { { k, v } });
                }
                return UnknownType;
            }
        }
        
        internal void ProcessComprehension(Comprehension node, LookupOptions lookupOptions = LookupOptions.Normal) {
            using (OpenScope(Module, node)) {
                foreach (var cfor in node.Iterators.OfType<ComprehensionFor>().Where(c => c.Left != null)) {
                    var value = GetValueFromExpression(cfor.List, lookupOptions);
                    if (value != null) {
                        switch (cfor.Left) {
                            case NameExpression nex when value is IPythonCollection c1:
                                DeclareVariable(nex.Name, c1.GetIterator().Next, VariableSource.Declaration, nex);
                                break;
                            case NameExpression nex:
                                DeclareVariable(nex.Name, UnknownType, VariableSource.Declaration, nex);
                                break;
                            case TupleExpression tex when value is IPythonDictionary dict && tex.Items.Count > 0:
                                if (tex.Items[0] is NameExpression nx0 && !string.IsNullOrEmpty(nx0.Name)) {
                                    DeclareVariable(nx0.Name, dict.Keys.FirstOrDefault() ?? UnknownType, VariableSource.Declaration, nx0);
                                }
                                if (tex.Items.Count > 1 && tex.Items[1] is NameExpression nx1 && !string.IsNullOrEmpty(nx1.Name)) {
                                    DeclareVariable(nx1.Name, dict.Values.FirstOrDefault() ?? UnknownType, VariableSource.Declaration, nx1);
                                }
                                foreach (var item in tex.Items.Skip(2).OfType<NameExpression>().Where(x => !string.IsNullOrEmpty(x.Name))) {
                                    DeclareVariable(item.Name, UnknownType, VariableSource.Declaration, item);
                                }
                                break;
                            case TupleExpression tex when value is IPythonCollection c2 && tex.Items.Count > 0:
                                var iter = c2.GetIterator();
                                foreach (var item in tex.Items.OfType<NameExpression>().Where(x => !string.IsNullOrEmpty(x.Name))) {
                                    DeclareVariable(item.Name, iter.Next, VariableSource.Declaration, item);
                                }
                                break;
                        }
                    }
                }
            }
        }
    }
}
