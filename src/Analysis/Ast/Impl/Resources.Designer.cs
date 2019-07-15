﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Microsoft.Python.Analysis {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "16.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class Resources {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Resources() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("Microsoft.Python.Analysis.Resources", typeof(Resources).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Overrides the current thread's CurrentUICulture property for all
        ///   resource lookups using this strongly typed resource class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Parameter {0} already specified..
        /// </summary>
        internal static string Analysis_ParameterAlreadySpecified {
            get {
                return ResourceManager.GetString("Analysis_ParameterAlreadySpecified", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Parameter {0} is missing..
        /// </summary>
        internal static string Analysis_ParameterMissing {
            get {
                return ResourceManager.GetString("Analysis_ParameterMissing", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Positional arguments are not allowed after keyword argument..
        /// </summary>
        internal static string Analysis_PositionalArgumentAfterKeyword {
            get {
                return ResourceManager.GetString("Analysis_PositionalArgumentAfterKeyword", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Too many function arguments..
        /// </summary>
        internal static string Analysis_TooManyFunctionArguments {
            get {
                return ResourceManager.GetString("Analysis_TooManyFunctionArguments", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Too many positional arguments before &apos;*&apos; argument..
        /// </summary>
        internal static string Analysis_TooManyPositionalArgumentBeforeStar {
            get {
                return ResourceManager.GetString("Analysis_TooManyPositionalArgumentBeforeStar", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Unknown parameter name..
        /// </summary>
        internal static string Analysis_UnknownParameterName {
            get {
                return ResourceManager.GetString("Analysis_UnknownParameterName", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Analysis cache path: {0}.
        /// </summary>
        internal static string AnalysisCachePath {
            get {
                return ResourceManager.GetString("AnalysisCachePath", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Analyzing in background, {0} items left....
        /// </summary>
        internal static string AnalysisProgress {
            get {
                return ResourceManager.GetString("AnalysisProgress", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Duplicate argument name &apos;{0}&apos; in function definition..
        /// </summary>
        internal static string DuplicateArgumentName {
            get {
                return ResourceManager.GetString("DuplicateArgumentName", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Environment variable &apos;{0}&apos; is not set, using the default cache location instead..
        /// </summary>
        internal static string EnvVariableNotSet {
            get {
                return ResourceManager.GetString("EnvVariableNotSet", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Path &apos;{0}&apos; is not rooted, using the default cache location instead..
        /// </summary>
        internal static string EnvVariablePathNotRooted {
            get {
                return ResourceManager.GetString("EnvVariablePathNotRooted", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to &apos;{0}&apos; may not be callable.
        /// </summary>
        internal static string ErrorNotCallable {
            get {
                return ResourceManager.GetString("ErrorNotCallable", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to object may not be callable.
        /// </summary>
        internal static string ErrorNotCallableEmpty {
            get {
                return ResourceManager.GetString("ErrorNotCallableEmpty", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Relative import &apos;{0}&apos; beyond top-level package.
        /// </summary>
        internal static string ErrorRelativeImportBeyondTopLevel {
            get {
                return ResourceManager.GetString("ErrorRelativeImportBeyondTopLevel", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to unresolved import &apos;{0}&apos;.
        /// </summary>
        internal static string ErrorUnresolvedImport {
            get {
                return ResourceManager.GetString("ErrorUnresolvedImport", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to &apos;{0}&apos; used before definition.
        /// </summary>
        internal static string ErrorUseBeforeDef {
            get {
                return ResourceManager.GetString("ErrorUseBeforeDef", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to &apos;{0}&apos; is not defined in the global scope.
        /// </summary>
        internal static string ErrorVariableNotDefinedGlobally {
            get {
                return ResourceManager.GetString("ErrorVariableNotDefinedGlobally", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to &apos;{0}&apos; is not defined in non-local scopes.
        /// </summary>
        internal static string ErrorVariableNotDefinedNonLocal {
            get {
                return ResourceManager.GetString("ErrorVariableNotDefinedNonLocal", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to An exception occured while discovering search paths; analysis will not be available..
        /// </summary>
        internal static string ExceptionGettingSearchPaths {
            get {
                return ResourceManager.GetString("ExceptionGettingSearchPaths", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Arguments to Generic must all be type parameters..
        /// </summary>
        internal static string GenericNotAllTypeParameters {
            get {
                return ResourceManager.GetString("GenericNotAllTypeParameters", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Arguments to Generic must all be unique..
        /// </summary>
        internal static string GenericNotAllUnique {
            get {
                return ResourceManager.GetString("GenericNotAllUnique", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Interpreter does not exist; analysis will not be available..
        /// </summary>
        internal static string InterpreterNotFound {
            get {
                return ResourceManager.GetString("InterpreterNotFound", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The first argument to NewType must be a string, but it is of type &apos;{0}&apos;..
        /// </summary>
        internal static string NewTypeFirstArgNotString {
            get {
                return ResourceManager.GetString("NewTypeFirstArgNotString", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to property of type {0}.
        /// </summary>
        internal static string PropertyOfType {
            get {
                return ResourceManager.GetString("PropertyOfType", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to property of unknown type.
        /// </summary>
        internal static string PropertyOfUnknownType {
            get {
                return ResourceManager.GetString("PropertyOfUnknownType", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Explicit return in __init__ .
        /// </summary>
        internal static string ReturnInInit {
            get {
                return ResourceManager.GetString("ReturnInInit", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Unable to determine analysis cache path. Using default &apos;{0}&apos;..
        /// </summary>
        internal static string UnableToDetermineCachePath {
            get {
                return ResourceManager.GetString("UnableToDetermineCachePath", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Unable to determine analysis cache path. Exception: {0}. Using default &apos;{1}&apos;..
        /// </summary>
        internal static string UnableToDetermineCachePathException {
            get {
                return ResourceManager.GetString("UnableToDetermineCachePathException", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Undefined variable: &apos;{0}&apos;.
        /// </summary>
        internal static string UndefinedVariable {
            get {
                return ResourceManager.GetString("UndefinedVariable", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Unsupported operand types for &apos;{0}&apos;: &apos;{1}&apos; and &apos;{2}&apos;.
        /// </summary>
        internal static string UnsupporedOperandType {
            get {
                return ResourceManager.GetString("UnsupporedOperandType", resourceCulture);
            }
        }
    }
}
