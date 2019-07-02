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

using Microsoft.Python.Analysis.Modules;
using Microsoft.Python.Analysis.Types;
using Microsoft.Python.Core.Text;
using Microsoft.Python.Parsing;

namespace Microsoft.Python.Analysis.Diagnostics {
    public sealed class DiagnosticsEntry {
        public DiagnosticsEntry(string message, SourceSpan span, string errorCode, Severity severity, DiagnosticSource source) {
            Message = message;
            SourceSpan = span;
            ErrorCode = errorCode;
            Severity = severity;
            Source = source;
        }

        /// <summary>
        /// Human-readable, localizable message.
        /// </summary>
        public string Message { get; }

        /// <summary>
        /// Location of the diagnostics.
        /// </summary>
        public SourceSpan SourceSpan { get; }

        /// <summary>
        /// Error code: non-localizable, unique identifier for the problem.
        /// </summary>
        public string ErrorCode { get; }

        /// <summary>
        /// Issue severity.
        /// </summary>
        public Severity Severity { get; }

        /// <summary>
        /// Subsystem that produced the diagnostics.
        /// </summary>
        public DiagnosticSource Source { get; }

        public bool ShouldReport(IPythonModule module) {
            // module should always be a user written python module
            if (module.ModuleType != ModuleType.User || !(module is PythonModule pythonModule)) {
                return false;
            }

            string line = pythonModule.GetLine(SourceSpan.Start.Line);

            // find if #noqa is in the string
            int commentPos = line.IndexOf("#");
            if (commentPos >= 0) {
                var comment = line.Substring(commentPos + 1);
                if (comment.Contains("noqa")) {
                    return false;
                }
            }

            return true;
        }

        public override bool Equals(object obj) {
            if (!(obj is DiagnosticsEntry e)) {
                return false;
            }
            return ErrorCode == e.ErrorCode && SourceSpan == e.SourceSpan;
        }
        public override int GetHashCode() => 0;
    }
}
