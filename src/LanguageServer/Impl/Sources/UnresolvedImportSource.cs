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
using System.Linq;
using System.Threading;
using Microsoft.Python.Analysis;
using Microsoft.Python.Analysis.Analyzer.Expressions;
using Microsoft.Python.Analysis.Diagnostics;
using Microsoft.Python.Core.Text;
using Microsoft.Python.LanguageServer.CodeActions;
using Microsoft.Python.LanguageServer.Protocol;
using Microsoft.Python.Parsing.Ast;


namespace Microsoft.Python.LanguageServer.Sources {
    internal sealed class UnresolvedImportSource {
        /// <summary>
        /// Returns a list of possible import statements corresponding to the given location
        /// </summary>
        /// <param name="uri"></param>
        /// <param name="location"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public IEnumerable<Command> GetImportSuggestions(IDocumentAnalysis analysis, CodeActionTable caTable, CodeActionParams @params, CancellationToken cancellationToken = default) {
            var ret = new List<Command>();
            var mres = analysis.Document.Interpreter.ModuleResolution;
            var modules = mres.CurrentPathResolver.GetAllModuleNames().Where(n => !string.IsNullOrEmpty(n)).Distinct();
            var undefinedDiagnostics = @params.context.diagnostics.Where(d => d.code == ErrorCodes.UndefinedVariable).ToList();
            var uri = @params.textDocument.uri;

            foreach (var diagnostic in undefinedDiagnostics) {
                ret.AddRange(GetImportSuggestions(analysis, caTable, uri, modules, diagnostic.range));
                //ret.AddRange(GetFromImportSuggestions(analysis, @params, modules, diagnostic.range));
            }

            return ret;
        }

        public IEnumerable<Command> GetImportSuggestions(IDocumentAnalysis analysis, CodeActionTable caTable, Uri uri, IEnumerable<string> moduleNames, Range range) {
            var ret = new List<Command>();
            var ast = analysis.Ast;
            var expr = ast.FindExpression(range, new FindExpressionOptions { Names = true });
            switch (expr) {
                case NameExpression n:
                    var name = n.Name;
                    // Make sure we can import the name
                    if(!moduleNames.Contains(name)) {
                        break;
                    }

                    var insertText = $"import {n.Name} \n";
                    var args = new object[] { analysis.Version, uri, insertText};
                    var id = caTable.Put("auto-import", args);

                    var tmp = new Command();
                    tmp.title = insertText;
                    tmp.command = Actions.InsertImport;
                    tmp.arguments = new object[] { id };

                    ret.Add(tmp);
                    break;
                default:
                    break;
            }
            return ret;
        }

        public IEnumerable<Command> GetFromImportSuggestions(IDocumentAnalysis analysis, CodeActionParams @params, IEnumerable<string> moduleNames, Range range) {
            return Array.Empty<Command>();
        }












    }
}
