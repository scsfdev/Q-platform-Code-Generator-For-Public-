﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace QP_Code_Generator.Properties {
    
    
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.VisualStudio.Editors.SettingsDesigner.SettingsSingleFileGenerator", "15.9.0.0")]
    internal sealed partial class Settings : global::System.Configuration.ApplicationSettingsBase {
        
        private static Settings defaultInstance = ((Settings)(global::System.Configuration.ApplicationSettingsBase.Synchronized(new Settings())));
        
        public static Settings Default {
            get {
                return defaultInstance;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("")]
        public string SaveTo {
            get {
                return ((string)(this["SaveTo"]));
            }
            set {
                this["SaveTo"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("https://HostUrl/api/FrameQR")]
        public string FQR {
            get {
                return ((string)(this["FQR"]));
            }
            set {
                this["FQR"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("https://HostUrl/api/sqrc")]
        public string SQRC {
            get {
                return ((string)(this["SQRC"]));
            }
            set {
                this["SQRC"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("https://HostUrl/api/SQRCdecode")]
        public string DSQRC {
            get {
                return ((string)(this["DSQRC"]));
            }
            set {
                this["DSQRC"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("https://HostUrl/api/FQRdecode")]
        public string DFQR {
            get {
                return ((string)(this["DFQR"]));
            }
            set {
                this["DFQR"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("https://HostUrl/api/SQRCBulk")]
        public string BFQR {
            get {
                return ((string)(this["BFQR"]));
            }
            set {
                this["BFQR"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("https://HostUrl/api/FrameQRBulk")]
        public string BSQRC {
            get {
                return ((string)(this["BSQRC"]));
            }
            set {
                this["BSQRC"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("https://AuthUrl/cert/api/token")]
        public string APIURL {
            get {
                return ((string)(this["APIURL"]));
            }
            set {
                this["APIURL"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("ApiKeyHere")]
        public string APIKEY {
            get {
                return ((string)(this["APIKEY"]));
            }
            set {
                this["APIKEY"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("HostUrl")]
        public string HOSTURL {
            get {
                return ((string)(this["HOSTURL"]));
            }
            set {
                this["HOSTURL"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("0123456789ABCDEF")]
        public string SQRCKEY {
            get {
                return ((string)(this["SQRCKEY"]));
            }
            set {
                this["SQRCKEY"] = value;
            }
        }
    }
}
