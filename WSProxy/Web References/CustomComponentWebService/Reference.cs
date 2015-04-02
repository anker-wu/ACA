#region Header

/**
 *  Accela Citizen Access
 *  File: Reference.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2011
 *
 *  Description:
 *
 *  Notes:
 * $Id: Reference.cs 192687 2011-03-14 05:38:13Z ACHIEVO\daly.zeng $.
 *  Revision History
 *  Date,                  Who,                 What
 */

#endregion Header

//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.1
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

// 
// This source code was auto-generated by Microsoft.VSDesigner, Version 4.0.30319.1.
// 
#pragma warning disable 1591

namespace Accela.ACA.WSProxy {
    using System;
    using System.Web.Services;
    using System.Diagnostics;
    using System.Web.Services.Protocols;
    using System.ComponentModel;
    using System.Xml.Serialization;
    
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.1")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Web.Services.WebServiceBindingAttribute(Name="CustomComponentWebServiceServiceSoapBinding", Namespace="http://service.webservice.accela.com/")]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(CustomComponentPKModel))]
    public partial class CustomComponentWebServiceService : System.Web.Services.Protocols.SoapHttpClientProtocol {
        
        private System.Threading.SendOrPostCallback getComponentConfigOperationCompleted;
        
        private System.Threading.SendOrPostCallback saveComponentConfigOperationCompleted;
        
        private bool useDefaultCredentialsSetExplicitly;
        
        /// <remarks/>
        public CustomComponentWebServiceService() {
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
        public event getComponentConfigCompletedEventHandler getComponentConfigCompleted;
        
        /// <remarks/>
        public event saveComponentConfigCompletedEventHandler saveComponentConfigCompleted;
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("", RequestNamespace="http://service.webservice.accela.com/", ResponseNamespace="http://service.webservice.accela.com/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute("return", Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public CustomComponentModel[] getComponentConfig([System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)] CustomComponentModel arg0) {
            object[] results = this.Invoke("getComponentConfig", new object[] {
                        arg0});
            return ((CustomComponentModel[])(results[0]));
        }
        
        /// <remarks/>
        public void getComponentConfigAsync(CustomComponentModel arg0) {
            this.getComponentConfigAsync(arg0, null);
        }
        
        /// <remarks/>
        public void getComponentConfigAsync(CustomComponentModel arg0, object userState) {
            if ((this.getComponentConfigOperationCompleted == null)) {
                this.getComponentConfigOperationCompleted = new System.Threading.SendOrPostCallback(this.OngetComponentConfigOperationCompleted);
            }
            this.InvokeAsync("getComponentConfig", new object[] {
                        arg0}, this.getComponentConfigOperationCompleted, userState);
        }
        
        private void OngetComponentConfigOperationCompleted(object arg) {
            if ((this.getComponentConfigCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.getComponentConfigCompleted(this, new getComponentConfigCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("", RequestNamespace="http://service.webservice.accela.com/", ResponseNamespace="http://service.webservice.accela.com/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public void saveComponentConfig([System.Xml.Serialization.XmlElementAttribute("arg0", Form=System.Xml.Schema.XmlSchemaForm.Unqualified)] CustomComponentModel[] arg0) {
            this.Invoke("saveComponentConfig", new object[] {
                        arg0});
        }
        
        /// <remarks/>
        public void saveComponentConfigAsync(CustomComponentModel[] arg0) {
            this.saveComponentConfigAsync(arg0, null);
        }
        
        /// <remarks/>
        public void saveComponentConfigAsync(CustomComponentModel[] arg0, object userState) {
            if ((this.saveComponentConfigOperationCompleted == null)) {
                this.saveComponentConfigOperationCompleted = new System.Threading.SendOrPostCallback(this.OnsaveComponentConfigOperationCompleted);
            }
            this.InvokeAsync("saveComponentConfig", new object[] {
                        arg0}, this.saveComponentConfigOperationCompleted, userState);
        }
        
        private void OnsaveComponentConfigOperationCompleted(object arg) {
            if ((this.saveComponentConfigCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.saveComponentConfigCompleted(this, new System.ComponentModel.AsyncCompletedEventArgs(invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
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
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.1")]
    public delegate void getComponentConfigCompletedEventHandler(object sender, getComponentConfigCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.1")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class getComponentConfigCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal getComponentConfigCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public CustomComponentModel[] Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((CustomComponentModel[])(this.results[0]));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.1")]
    public delegate void saveComponentConfigCompletedEventHandler(object sender, System.ComponentModel.AsyncCompletedEventArgs e);
}

#pragma warning restore 1591