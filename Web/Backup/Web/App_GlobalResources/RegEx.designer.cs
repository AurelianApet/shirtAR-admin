//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.34014
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Resources {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option or rebuild the Visual Studio project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.VisualStudio.Web.Application.StronglyTypedResourceProxyBuilder", "10.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class RegEx {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal RegEx() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("Resources.RegEx", global::System.Reflection.Assembly.Load("App_GlobalResources"));
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
        ///   Looks up a localized string similar to ^[0-9]{1,50}.
        /// </summary>
        internal static string REGEX_BANKNUM {
            get {
                return ResourceManager.GetString("REGEX_BANKNUM", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to ^[\w\.-]+@([\w-]+\.)+[\w-]+$.
        /// </summary>
        internal static string REGEX_EMAIL {
            get {
                return ResourceManager.GetString("REGEX_EMAIL", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to ^[a-zA-Z0-9]{4,10}.
        /// </summary>
        internal static string REGEX_LOGINID {
            get {
                return ResourceManager.GetString("REGEX_LOGINID", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to ^.{4,100}.
        /// </summary>
        internal static string REGEX_LOGINPWD {
            get {
                return ResourceManager.GetString("REGEX_LOGINPWD", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to ^[가-힣]{2,5}.
        /// </summary>
        internal static string REGEX_NAME {
            get {
                return ResourceManager.GetString("REGEX_NAME", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to [가-힣a-zA-Z0-9]{3,10}.
        /// </summary>
        internal static string REGEX_NICKNAME {
            get {
                return ResourceManager.GetString("REGEX_NICKNAME", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to ^[가-힣]{2,5}.
        /// </summary>
        internal static string REGEX_OWNERNAME {
            get {
                return ResourceManager.GetString("REGEX_OWNERNAME", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to ^[0-9]{3}[-]*[0-9]{3,4}[-]*[0-9]{4}.
        /// </summary>
        internal static string REGEX_TELNO {
            get {
                return ResourceManager.GetString("REGEX_TELNO", resourceCulture);
            }
        }
    }
}
