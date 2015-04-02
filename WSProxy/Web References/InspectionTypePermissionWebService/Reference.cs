#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: InspectionTypePermissionWebServiceService.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2010
 *
 *  Description:
 *
 *  Notes:
 * $Id: InspectionTypePermissionWebServiceService.cs 178385 2010-08-06 07:47:06Z ACHIEVO\jackie.yu $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 *  05/09/2007    troy.yang    Initial version.
 * </pre>
 */

#endregion Header
#pragma warning disable 1591

namespace Accela.ACA.WSProxy {
    using System.Diagnostics;
    using System.Web.Services;
    using System.ComponentModel;
    using System.Web.Services.Protocols;
    using System;
    using System.Xml.Serialization;
    
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "2.0.50727.3053")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Web.Services.WebServiceBindingAttribute(Name="InspectionTypePermissionWebServiceServiceSoapBinding", Namespace="http://service.webservice.accela.com/")]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(LanguageModel))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(AppStatusGroupPKModel))]
    public partial class InspectionTypePermissionWebServiceService : System.Web.Services.Protocols.SoapHttpClientProtocol {
        
        private System.Threading.SendOrPostCallback checkInspectionTypeActionableOperationCompleted;
        
        private System.Threading.SendOrPostCallback getActionPermissionSettingsOperationCompleted;
        
        private System.Threading.SendOrPostCallback hasActionableInspectionTypeOperationCompleted;
        
        private System.Threading.SendOrPostCallback saveActionPermissionSettingsOperationCompleted;
        
        private bool useDefaultCredentialsSetExplicitly;
        
        /// <remarks/>
        public InspectionTypePermissionWebServiceService() {
            if ((this.IsLocalFileSystemWebService(this.Url) == true)) {
                this.UseDefaultCredentials = true;
                this.useDefaultCredentialsSetExplicitly = false;
            }
            else {
                this.useDefaultCredentialsSetExplicitly = true;
            }
        }
        
        public new string Url {
            get {
                return base.Url;
            }
            set {
                if ((((this.IsLocalFileSystemWebService(base.Url) == true) 
                            && (this.useDefaultCredentialsSetExplicitly == false)) 
                            && (this.IsLocalFileSystemWebService(value) == false))) {
                    base.UseDefaultCredentials = false;
                }
                base.Url = value;
            }
        }
        
        public new bool UseDefaultCredentials {
            get {
                return base.UseDefaultCredentials;
            }
            set {
                base.UseDefaultCredentials = value;
                this.useDefaultCredentialsSetExplicitly = true;
            }
        }
        
        /// <remarks/>
        public event checkInspectionTypeActionableCompletedEventHandler checkInspectionTypeActionableCompleted;
        
        /// <remarks/>
        public event getActionPermissionSettingsCompletedEventHandler getActionPermissionSettingsCompleted;
        
        /// <remarks/>
        public event hasActionableInspectionTypeCompletedEventHandler hasActionableInspectionTypeCompleted;
        
        /// <remarks/>
        public event saveActionPermissionSettingsCompletedEventHandler saveActionPermissionSettingsCompleted;
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("", RequestNamespace="http://service.webservice.accela.com/", ResponseNamespace="http://service.webservice.accela.com/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute("return", Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public bool checkInspectionTypeActionable([System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)] string arg0, [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)] CapIDModel arg1, [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)] string arg2, [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)] string arg3) {
            object[] results = this.Invoke("checkInspectionTypeActionable", new object[] {
                        arg0,
                        arg1,
                        arg2,
                        arg3});
            return ((bool)(results[0]));
        }
        
        /// <remarks/>
        public void checkInspectionTypeActionableAsync(string arg0, CapIDModel arg1, string arg2, string arg3) {
            this.checkInspectionTypeActionableAsync(arg0, arg1, arg2, arg3, null);
        }
        
        /// <remarks/>
        public void checkInspectionTypeActionableAsync(string arg0, CapIDModel arg1, string arg2, string arg3, object userState) {
            if ((this.checkInspectionTypeActionableOperationCompleted == null)) {
                this.checkInspectionTypeActionableOperationCompleted = new System.Threading.SendOrPostCallback(this.OncheckInspectionTypeActionableOperationCompleted);
            }
            this.InvokeAsync("checkInspectionTypeActionable", new object[] {
                        arg0,
                        arg1,
                        arg2,
                        arg3}, this.checkInspectionTypeActionableOperationCompleted, userState);
        }
        
        private void OncheckInspectionTypeActionableOperationCompleted(object arg) {
            if ((this.checkInspectionTypeActionableCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.checkInspectionTypeActionableCompleted(this, new checkInspectionTypeActionableCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("", RequestNamespace="http://service.webservice.accela.com/", ResponseNamespace="http://service.webservice.accela.com/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute("return", Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public InspectionActionPermissionModel[] getActionPermissionSettings([System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)] string arg0) {
            object[] results = this.Invoke("getActionPermissionSettings", new object[] {
                        arg0});
            return ((InspectionActionPermissionModel[])(results[0]));
        }
        
        /// <remarks/>
        public void getActionPermissionSettingsAsync(string arg0) {
            this.getActionPermissionSettingsAsync(arg0, null);
        }
        
        /// <remarks/>
        public void getActionPermissionSettingsAsync(string arg0, object userState) {
            if ((this.getActionPermissionSettingsOperationCompleted == null)) {
                this.getActionPermissionSettingsOperationCompleted = new System.Threading.SendOrPostCallback(this.OngetActionPermissionSettingsOperationCompleted);
            }
            this.InvokeAsync("getActionPermissionSettings", new object[] {
                        arg0}, this.getActionPermissionSettingsOperationCompleted, userState);
        }
        
        private void OngetActionPermissionSettingsOperationCompleted(object arg) {
            if ((this.getActionPermissionSettingsCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.getActionPermissionSettingsCompleted(this, new getActionPermissionSettingsCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("", RequestNamespace="http://service.webservice.accela.com/", ResponseNamespace="http://service.webservice.accela.com/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute("return", Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public bool hasActionableInspectionType([System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)] string arg0, [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)] CapIDModel arg1, [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)] string arg2) {
            object[] results = this.Invoke("hasActionableInspectionType", new object[] {
                        arg0,
                        arg1,
                        arg2});
            return ((bool)(results[0]));
        }
        
        /// <remarks/>
        public void hasActionableInspectionTypeAsync(string arg0, CapIDModel arg1, string arg2) {
            this.hasActionableInspectionTypeAsync(arg0, arg1, arg2, null);
        }
        
        /// <remarks/>
        public void hasActionableInspectionTypeAsync(string arg0, CapIDModel arg1, string arg2, object userState) {
            if ((this.hasActionableInspectionTypeOperationCompleted == null)) {
                this.hasActionableInspectionTypeOperationCompleted = new System.Threading.SendOrPostCallback(this.OnhasActionableInspectionTypeOperationCompleted);
            }
            this.InvokeAsync("hasActionableInspectionType", new object[] {
                        arg0,
                        arg1,
                        arg2}, this.hasActionableInspectionTypeOperationCompleted, userState);
        }
        
        private void OnhasActionableInspectionTypeOperationCompleted(object arg) {
            if ((this.hasActionableInspectionTypeCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.hasActionableInspectionTypeCompleted(this, new hasActionableInspectionTypeCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("", RequestNamespace="http://service.webservice.accela.com/", ResponseNamespace="http://service.webservice.accela.com/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute("return", Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public bool saveActionPermissionSettings([System.Xml.Serialization.XmlElementAttribute("arg0", Form=System.Xml.Schema.XmlSchemaForm.Unqualified)] InspectionActionPermissionModel[] arg0) {
            object[] results = this.Invoke("saveActionPermissionSettings", new object[] {
                        arg0});
            return ((bool)(results[0]));
        }
        
        /// <remarks/>
        public void saveActionPermissionSettingsAsync(InspectionActionPermissionModel[] arg0) {
            this.saveActionPermissionSettingsAsync(arg0, null);
        }
        
        /// <remarks/>
        public void saveActionPermissionSettingsAsync(InspectionActionPermissionModel[] arg0, object userState) {
            if ((this.saveActionPermissionSettingsOperationCompleted == null)) {
                this.saveActionPermissionSettingsOperationCompleted = new System.Threading.SendOrPostCallback(this.OnsaveActionPermissionSettingsOperationCompleted);
            }
            this.InvokeAsync("saveActionPermissionSettings", new object[] {
                        arg0}, this.saveActionPermissionSettingsOperationCompleted, userState);
        }
        
        private void OnsaveActionPermissionSettingsOperationCompleted(object arg) {
            if ((this.saveActionPermissionSettingsCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.saveActionPermissionSettingsCompleted(this, new saveActionPermissionSettingsCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        public new void CancelAsync(object userState) {
            base.CancelAsync(userState);
        }
        
        private bool IsLocalFileSystemWebService(string url) {
            if (((url == null) 
                        || (url == string.Empty))) {
                return false;
            }
            System.Uri wsUri = new System.Uri(url);
            if (((wsUri.Port >= 1024) 
                        && (string.Compare(wsUri.Host, "localHost", System.StringComparison.OrdinalIgnoreCase) == 0))) {
                return true;
            }
            return false;
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "2.0.50727.3053")]
    public delegate void checkInspectionTypeActionableCompletedEventHandler(object sender, checkInspectionTypeActionableCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "2.0.50727.3053")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class checkInspectionTypeActionableCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal checkInspectionTypeActionableCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public bool Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((bool)(this.results[0]));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "2.0.50727.3053")]
    public delegate void getActionPermissionSettingsCompletedEventHandler(object sender, getActionPermissionSettingsCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "2.0.50727.3053")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class getActionPermissionSettingsCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal getActionPermissionSettingsCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public InspectionActionPermissionModel[] Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((InspectionActionPermissionModel[])(this.results[0]));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "2.0.50727.3053")]
    public delegate void hasActionableInspectionTypeCompletedEventHandler(object sender, hasActionableInspectionTypeCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "2.0.50727.3053")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class hasActionableInspectionTypeCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal hasActionableInspectionTypeCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public bool Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((bool)(this.results[0]));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "2.0.50727.3053")]
    public delegate void saveActionPermissionSettingsCompletedEventHandler(object sender, saveActionPermissionSettingsCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "2.0.50727.3053")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class saveActionPermissionSettingsCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal saveActionPermissionSettingsCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public bool Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((bool)(this.results[0]));
            }
        }
    }
}

#pragma warning restore 1591