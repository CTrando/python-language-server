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

using System;
using System.Collections.Generic;
using Microsoft.Python.Parsing.Definition;

namespace Microsoft.Python.Parsing.Ast {
    public abstract class ScopeStatement : Statement, IScopeStatement {
        public virtual string ScopeName => "<unknown>";
        public IScopeNode Parent { get; set; }
        public abstract Statement Body { get; }

        public bool IsClosure => ScopeInfo.IsClosure;

        public bool ContainsNestedFreeVariables => ScopeInfo.ContainsNestedFreeVariables;

        public bool NeedsLocalsDictionary => ScopeInfo.NeedsLocalsDictionary;

        public virtual string Name => ScopeName;

        public ICollection<PythonVariable> ScopeVariables => ScopeInfo.ScopeVariables;

        public bool IsGlobal => ScopeInfo.IsGlobal;

        public PythonAst GlobalParent => ScopeInfo.GlobalParent;

        public IReadOnlyList<PythonVariable> FreeVariables => ScopeInfo.FreeVariables;

        public bool TryGetVariable(string name, out PythonVariable variable) => ScopeInfo.TryGetVariable(name, out variable);

        public abstract ScopeInfo ScopeInfo { get; }

        public void Bind(PythonNameBinder binder) => ScopeInfo.Bind(binder);

        public void FinishBind(PythonNameBinder binder) => ScopeInfo.FinishBind(binder);
    }
}
