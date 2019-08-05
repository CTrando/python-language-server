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
using System.ComponentModel.Design;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Python.Analysis.Documents;
using Microsoft.Python.Core.Text;
using Microsoft.Python.LanguageServer.CodeActions;
using Microsoft.Python.LanguageServer.Documents;
using Microsoft.Python.LanguageServer.Protocol;
using Microsoft.Python.LanguageServer.Sources;

namespace Microsoft.Python.LanguageServer.Implementation {
    public sealed partial class Server {
        /// <summary>
        /// Returns an array of ApplyWorkspaceEditParams to send to client after handling command
        /// </summary>
        /// <returns></returns>
        // add async to method definition if need to do any async work
        public async Task<IReadOnlyList<ApplyWorkspaceEditParams>> ExecuteCommandAsync(ExecuteCommandParams @params, IRunningDocumentTable rdt, CancellationToken cancellationToken) {
            var edits = new List<ApplyWorkspaceEditParams>();
            switch (@params.command) {
                case Actions.InsertImport:
                    var codeActionID = @params.arguments[0] as string;
                    _codeActionTable.TryGetValue(codeActionID, out var args);

                    var documentVersion = args[0] as string;
                    var uri = args[1] as Uri;
                    var insertText = args[2] as string;

                    // will have the string import statement that we need, just need to find where to put it inside the document
                    var start = new SourceLocation(1, 1);

                    var tmp = new Dictionary<Uri, TextEdit[]>();
                    var text = new TextEdit {
                        range = new Range { start = start, end = start },
                        newText = insertText
                    };

                    tmp.Add(uri, new[] { text });
                    var we = new WorkspaceEdit { changes = tmp };

                    edits.Add(new ApplyWorkspaceEditParams { label = "refactor", edit = we });
                    break;
                default:
                    break;
            }
            return edits;
        }

        public async Task<Command[]> GetAvailableCommandsAsync(CodeActionParams @params, CancellationToken cancellationToken) {
            var ret = new List<Command>();
            var uri = @params.textDocument.uri;
            var analysis = await Document.GetAnalysisAsync(uri, Services, 200, cancellationToken);
            var importSuggestions = new UnresolvedImportSource().GetImportSuggestions(analysis, _codeActionTable, @params, cancellationToken);
            ret.AddRange(importSuggestions);
            return ret.ToArray();
        }
    }
}
