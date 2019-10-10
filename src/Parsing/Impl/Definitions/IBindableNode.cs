using Microsoft.Python.Parsing.Ast;

namespace Microsoft.Python.Parsing {
    internal interface IBindableNode : IScopeNode {
        #region Binding
        void Bind(PythonNameBinder binder);

        void FinishBind(PythonNameBinder binder);

        bool TryBindOuter(IBindableNode from, string name, bool allowGlobals, out PythonVariable variable);
        #endregion

        #region Add variables
        void AddVariable(PythonVariable variable);
        void AddFreeVariable(PythonVariable variable, bool accessedInScope);

        string AddReferencedGlobal(string name);

        void AddNonLocalVariable(NameExpression name);

        void AddCellVariable(PythonVariable variable);
        #endregion

        #region References
        PythonVariable BindReference(PythonNameBinder binder, string name);

        PythonReference Reference(string name);

        bool IsReferenced(string name);
        #endregion

        #region Create Variables
        PythonVariable CreateVariable(string name, VariableKind kind);

        PythonVariable EnsureVariable(string name);

        PythonVariable EnsureGlobalVariable(string name);

        PythonVariable DefineParameter(string name);
        #endregion

        #region Contains
        bool ContainsImportStar { get; set; }
        bool ContainsExceptionHandling { get; set; }

        bool ContainsUnqualifiedExec { get; set; }

        /// <summary>
        /// True if variables can be set in a late bound fashion that we don't
        /// know about at code gen time - for example via from fob import *.
        /// 
        /// This is tracked independently of the ContainsUnqualifiedExec/NeedsLocalsDictionary
        /// </summary>
        bool HasLateBoundVariableSets { get; set; }

        bool ExposesLocalVariable(PythonVariable name);
        # endregion
    }
}
