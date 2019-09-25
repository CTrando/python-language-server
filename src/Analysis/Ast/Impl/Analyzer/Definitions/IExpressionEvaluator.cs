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

using System;
using System.Collections.Generic;
using Microsoft.Python.Analysis.Diagnostics;
using Microsoft.Python.Analysis.Types;
using Microsoft.Python.Analysis.Values;
using Microsoft.Python.Core;
using Microsoft.Python.Parsing.Ast;
using Microsoft.Python.Parsing.Definition;

namespace Microsoft.Python.Analysis.Analyzer {
    public interface IExpressionEvaluator {
        /// <summary>
        /// Opens existing scope for a node. The scope is pushed
        /// on the stack and will be removed when the returned
        /// disposable is disposed.
        /// </summary>
        IDisposable OpenScope(IScope scope);

        /// <summary>
        /// Opens existing scope for a node. The scope is pushed
        /// on the stack and will be removed when the returned
        /// disposable is disposed.
        /// </summary>
        IDisposable OpenScope(IPythonModule module, IScopeNode scope);

        /// <summary>
        /// Currently opened (deep-most) scope.
        /// </summary>
        IScope CurrentScope { get; }

        /// <summary>
        /// Module global scope.
        /// </summary>
        IGlobalScope GlobalScope { get; }

        /// <summary>
        /// Determines node location in the module source code.
        /// </summary>
        LocationInfo GetLocation(Node node);

        /// <summary>
        /// Evaluates expression in the currently open scope.
        /// </summary>
        IMember GetValueFromExpression(Expression expr, LookupOptions options = LookupOptions.Normal);

        IMember LookupNameInScopes(string name, out IScope scope, out IVariable v, LookupOptions options = LookupOptions.Normal);

        IPythonType GetTypeFromString(string typeString);

        /// <summary>
        /// Module AST.
        /// </summary>
        PythonAst Ast { get; }

        /// <summary>
        /// Associated module.
        /// </summary>
        IPythonModule Module { get; }

        /// <summary>
        /// Interpreter used in the module analysis.
        /// </summary>
        IPythonInterpreter Interpreter { get; }

        /// <summary>
        /// Application service container.
        /// </summary>
        IServiceContainer Services { get; }

        void ReportDiagnostics(Uri documentUri, DiagnosticsEntry entry);

        IEnumerable<DiagnosticsEntry> Diagnostics { get; }

        /// <summary>
        /// Represents built-in 'unknown' type. <see cref="BuiltinTypeId.Unknown"/>.
        /// </summary>
        IPythonType UnknownType { get; }
    }
}
