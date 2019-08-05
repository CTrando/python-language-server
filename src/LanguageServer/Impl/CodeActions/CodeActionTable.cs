using System;
using System.Collections.Generic;

namespace Microsoft.Python.LanguageServer.CodeActions {
    class CodeActionTable {
        private Dictionary<string, int> _prefixCount;
        private Dictionary<string, object[]> _objects;

        public CodeActionTable() {
            _prefixCount = new Dictionary<string, int>();
            _objects = new Dictionary<string, object[]>();
        }

        public string Put(string p, object[] o) {
            _prefixCount.TryGetValue(p, out var count);
            _prefixCount[p] = ++count;

            var id = $"{p}-{count}-{o.GetHashCode()}";
            _objects[id] = o;
            return id;
        }

        public bool TryGetValue(string id, out object[] ret) {
            return _objects.TryGetValue(id, out ret);
        }
    }
}
