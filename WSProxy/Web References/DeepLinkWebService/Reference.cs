﻿#region Header

/**
 *  Accela Citizen Access
 *  File: Reference.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2013
 *
 *  Description:
 *
 *  Notes:
 * $Id: Reference.cs 192687 2013-10-11 05:38:13Z ACHIEVO\peter.peng $.
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
    [System.Web.Services.WebServiceBindingAttribute(Name="DeepLinkWebServiceServiceSoapBinding", Namespace="http://service.webservice.accela.com/")]
    public partial class DeepLinkWebServiceService : System.Web.Services.Protocols.SoapHttpClientProtocol {
        
        private System.Threading.SendOrPostCallback updateDeepLinkAuditTrailOperationCompleted;
        
        private System.Threading.SendOrPostCallback removeDeepLinkAuditTrailOperationCompleted;
        
        private System.Threading.SendOrPostCallback getDeepLinkAuditTrailOperationCompleted;
        
        private System.Threading.SendOrPostCallback createDeepLinkAuditTrailOperationCompleted;
        
        private bool useDefaultCredentialsSetExplicitly;
        
        /// <remarks/>
        public DeepLinkWebServiceService() {
            
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
        public event updateDeepLinkAuditTrailCompletedEventHandler updateDeepLinkAuditTrailCompleted;
        
        /// <remarks/>
        public event removeDeepLinkAuditTrailCompletedEventHandler removeDeepLinkAuditTrailCompleted;
        
        /// <remarks/>
        public event getDeepLinkAuditTrailCompletedEventHandler getDeepLinkAuditTrailCompleted;
        
        /// <remarks/>
        public event createDeepLinkAuditTrailCompletedEventHandler createDeepLinkAuditTrailCompleted;
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("", RequestNamespace="http://service.webservice.accela.com/", ResponseNamespace="http://service.webservice.accela.com/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute("return", Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public bool updateDeepLinkAuditTrail([System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)] string arg0, [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)] DeepLinkAuditTrailModel arg1, [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)] string arg2) {
            object[] results = this.Invoke("updateDeepLinkAuditTrail", new object[] {
                        arg0,
                        arg1,
                        arg2});
            return ((bool)(results[0]));
        }
        
        /// <remarks/>
        public void updateDeepLinkAuditTrailAsync(string arg0, DeepLinkAuditTrailModel arg1, string arg2) {
            this.updateDeepLinkAuditTrailAsync(arg0, arg1, arg2, null);
        }
        
        /// <remarks/>
        public void updateDeepLinkAuditTrailAsync(string arg0, DeepLinkAuditTrailModel arg1, string arg2, object userState) {
            if ((this.updateDeepLinkAuditTrailOperationCompleted == null)) {
                this.updateDeepLinkAuditTrailOperationCompleted = new System.Threading.SendOrPostCallback(this.OnupdateDeepLinkAuditTrailOperationCompleted);
            }
            this.InvokeAsync("updateDeepLinkAuditTrail", new object[] {
                        arg0,
                        arg1,
                        arg2}, this.updateDeepLinkAuditTrailOperationCompleted, userState);
        }
        
        private void OnupdateDeepLinkAuditTrailOperationCompleted(object arg) {
            if ((this.updateDeepLinkAuditTrailCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.updateDeepLinkAuditTrailCompleted(this, new updateDeepLinkAuditTrailCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("", RequestNamespace="http://service.webservice.accela.com/", ResponseNamespace="http://service.webservice.accela.com/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute("return", Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public bool removeDeepLinkAuditTrail([System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)] string arg0, [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)] DeepLinkAuditTrailModel arg1, [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)] string arg2) {
            object[] results = this.Invoke("removeDeepLinkAuditTrail", new object[] {
                        arg0,
                        arg1,
                        arg2});
            return ((bool)(results[0]));
        }
        
        /// <remarks/>
        public void removeDeepLinkAuditTrailAsync(string arg0, DeepLinkAuditTrailModel arg1, string arg2) {
            this.removeDeepLinkAuditTrailAsync(arg0, arg1, arg2, null);
        }
        
        /// <remarks/>
        public void removeDeepLinkAuditTrailAsync(string arg0, DeepLinkAuditTrailModel arg1, string arg2, object userState) {
            if ((this.removeDeepLinkAuditTrailOperationCompleted == null)) {
                this.removeDeepLinkAuditTrailOperationCompleted = new System.Threading.SendOrPostCallback(this.OnremoveDeepLinkAuditTrailOperationCompleted);
            }
            this.InvokeAsync("removeDeepLinkAuditTrail", new object[] {
                        arg0,
                        arg1,
                        arg2}, this.removeDeepLinkAuditTrailOperationCompleted, userState);
        }
        
        private void OnremoveDeepLinkAuditTrailOperationCompleted(object arg) {
            if ((this.removeDeepLinkAuditTrailCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.removeDeepLinkAuditTrailCompleted(this, new removeDeepLinkAuditTrailCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("", RequestNamespace="http://service.webservice.accela.com/", ResponseNamespace="http://service.webservice.accela.com/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute("return", Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public DeepLinkAuditTrailModel getDeepLinkAuditTrail([System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)] string arg0, [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)] string arg1, [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)] string arg2) {
            object[] results = this.Invoke("getDeepLinkAuditTrail", new object[] {
                        arg0,
                        arg1,
                        arg2});
            return ((DeepLinkAuditTrailModel)(results[0]));
        }
        
        /// <remarks/>
        public void getDeepLinkAuditTrailAsync(string arg0, string arg1, string arg2) {
            this.getDeepLinkAuditTrailAsync(arg0, arg1, arg2, null);
        }
        
        /// <remarks/>
        public void getDeepLinkAuditTrailAsync(string arg0, string arg1, string arg2, object userState) {
            if ((this.getDeepLinkAuditTrailOperationCompleted == null)) {
                this.getDeepLinkAuditTrailOperationCompleted = new System.Threading.SendOrPostCallback(this.OngetDeepLinkAuditTrailOperationCompleted);
            }
            this.InvokeAsync("getDeepLinkAuditTrail", new object[] {
                        arg0,
                        arg1,
                        arg2}, this.getDeepLinkAuditTrailOperationCompleted, userState);
        }
        
        private void OngetDeepLinkAuditTrailOperationCompleted(object arg) {
            if ((this.getDeepLinkAuditTrailCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.getDeepLinkAuditTrailCompleted(this, new getDeepLinkAuditTrailCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("", RequestNamespace="http://service.webservice.accela.com/", ResponseNamespace="http://service.webservice.accela.com/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute("return", Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public DeepLinkAuditTrailModel createDeepLinkAuditTrail([System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)] string arg0, [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)] DeepLinkAuditTrailModel arg1, [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)] string arg2) {
            object[] results = this.Invoke("createDeepLinkAuditTrail", new object[] {
                        arg0,
                        arg1,
                        arg2});
            return ((DeepLinkAuditTrailModel)(results[0]));
        }
        
        /// <remarks/>
        public void createDeepLinkAuditTrailAsync(string arg0, DeepLinkAuditTrailModel arg1, string arg2) {
            this.createDeepLinkAuditTrailAsync(arg0, arg1, arg2, null);
        }
        
        /// <remarks/>
        public void createDeepLinkAuditTrailAsync(string arg0, DeepLinkAuditTrailModel arg1, string arg2, object userState) {
            if ((this.createDeepLinkAuditTrailOperationCompleted == null)) {
                this.createDeepLinkAuditTrailOperationCompleted = new System.Threading.SendOrPostCallback(this.OncreateDeepLinkAuditTrailOperationCompleted);
            }
            this.InvokeAsync("createDeepLinkAuditTrail", new object[] {
                        arg0,
                        arg1,
                        arg2}, this.createDeepLinkAuditTrailOperationCompleted, userState);
        }
        
        private void OncreateDeepLinkAuditTrailOperationCompleted(object arg) {
            if ((this.createDeepLinkAuditTrailCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.createDeepLinkAuditTrailCompleted(this, new createDeepLinkAuditTrailCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
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
    public delegate void updateDeepLinkAuditTrailCompletedEventHandler(object sender, updateDeepLinkAuditTrailCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.1")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class updateDeepLinkAuditTrailCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal updateDeepLinkAuditTrailCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
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
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.1")]
    public delegate void removeDeepLinkAuditTrailCompletedEventHandler(object sender, removeDeepLinkAuditTrailCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.1")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class removeDeepLinkAuditTrailCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal removeDeepLinkAuditTrailCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
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
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.1")]
    public delegate void getDeepLinkAuditTrailCompletedEventHandler(object sender, getDeepLinkAuditTrailCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.1")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class getDeepLinkAuditTrailCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal getDeepLinkAuditTrailCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public DeepLinkAuditTrailModel Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((DeepLinkAuditTrailModel)(this.results[0]));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.1")]
    public delegate void createDeepLinkAuditTrailCompletedEventHandler(object sender, createDeepLinkAuditTrailCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.1")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class createDeepLinkAuditTrailCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal createDeepLinkAuditTrailCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public DeepLinkAuditTrailModel Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((DeepLinkAuditTrailModel)(this.results[0]));
            }
        }
    }
}

#pragma warning restore 1591