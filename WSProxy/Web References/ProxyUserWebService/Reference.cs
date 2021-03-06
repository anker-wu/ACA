﻿/**
 * <pre>
 * 
 *  Accela Citizen Access
 *  File: Reference.cs
 * 
 *  Accela, Inc.
 *  Copyright (C): 2010
 * 
 *  Description:
 * 
 *  Notes:
 * $Id: Reference.cs 185056 2010-11-23 03:37:54Z ACHIEVO\daly.zeng $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:2.0.50727.1433
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

// 
// This source code was auto-generated by Microsoft.VSDesigner, Version 2.0.50727.1433.
// 
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
    [System.Web.Services.WebServiceBindingAttribute(Name="ProxyUserWebServiceServiceSoapBinding", Namespace="http://service.webservice.accela.com/")]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(LanguageModel))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(XProxyUserPermissionPKModel))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(XProxyUserPKModel))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(XProxyUserPermissionModel[]))]
    public partial class ProxyUserWebServiceService : System.Web.Services.Protocols.SoapHttpClientProtocol {
        
        private System.Threading.SendOrPostCallback deleteProxyUserOperationCompleted;
        
        private System.Threading.SendOrPostCallback updateProxyStatusOperationCompleted;
        
        private System.Threading.SendOrPostCallback getProxyUsersOperationCompleted;
        
        private System.Threading.SendOrPostCallback validateProxyUserPaymentPermissionOperationCompleted;
        
        private System.Threading.SendOrPostCallback validateEmailOperationCompleted;
        
        private System.Threading.SendOrPostCallback createProxyUserOperationCompleted;
        
        private System.Threading.SendOrPostCallback updatePermissionsOperationCompleted;
        
        private bool useDefaultCredentialsSetExplicitly;
        
        /// <remarks/>
        public ProxyUserWebServiceService() {
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
        public event deleteProxyUserCompletedEventHandler deleteProxyUserCompleted;
        
        /// <remarks/>
        public event updateProxyStatusCompletedEventHandler updateProxyStatusCompleted;
        
        /// <remarks/>
        public event getProxyUsersCompletedEventHandler getProxyUsersCompleted;
        
        /// <remarks/>
        public event validateProxyUserPaymentPermissionCompletedEventHandler validateProxyUserPaymentPermissionCompleted;
        
        /// <remarks/>
        public event validateEmailCompletedEventHandler validateEmailCompleted;
        
        /// <remarks/>
        public event createProxyUserCompletedEventHandler createProxyUserCompleted;
        
        /// <remarks/>
        public event updatePermissionsCompletedEventHandler updatePermissionsCompleted;
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("", RequestNamespace="http://service.webservice.accela.com/", ResponseNamespace="http://service.webservice.accela.com/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public void deleteProxyUser([System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)] XProxyUserModel arg0, [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)] string arg1) {
            this.Invoke("deleteProxyUser", new object[] {
                        arg0,
                        arg1});
        }
        
        /// <remarks/>
        public void deleteProxyUserAsync(XProxyUserModel arg0, string arg1) {
            this.deleteProxyUserAsync(arg0, arg1, null);
        }
        
        /// <remarks/>
        public void deleteProxyUserAsync(XProxyUserModel arg0, string arg1, object userState) {
            if ((this.deleteProxyUserOperationCompleted == null)) {
                this.deleteProxyUserOperationCompleted = new System.Threading.SendOrPostCallback(this.OndeleteProxyUserOperationCompleted);
            }
            this.InvokeAsync("deleteProxyUser", new object[] {
                        arg0,
                        arg1}, this.deleteProxyUserOperationCompleted, userState);
        }
        
        private void OndeleteProxyUserOperationCompleted(object arg) {
            if ((this.deleteProxyUserCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.deleteProxyUserCompleted(this, new System.ComponentModel.AsyncCompletedEventArgs(invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("", RequestNamespace="http://service.webservice.accela.com/", ResponseNamespace="http://service.webservice.accela.com/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public void updateProxyStatus([System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)] XProxyUserModel arg0, [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)] string arg1) {
            this.Invoke("updateProxyStatus", new object[] {
                        arg0,
                        arg1});
        }
        
        /// <remarks/>
        public void updateProxyStatusAsync(XProxyUserModel arg0, string arg1) {
            this.updateProxyStatusAsync(arg0, arg1, null);
        }
        
        /// <remarks/>
        public void updateProxyStatusAsync(XProxyUserModel arg0, string arg1, object userState) {
            if ((this.updateProxyStatusOperationCompleted == null)) {
                this.updateProxyStatusOperationCompleted = new System.Threading.SendOrPostCallback(this.OnupdateProxyStatusOperationCompleted);
            }
            this.InvokeAsync("updateProxyStatus", new object[] {
                        arg0,
                        arg1}, this.updateProxyStatusOperationCompleted, userState);
        }
        
        private void OnupdateProxyStatusOperationCompleted(object arg) {
            if ((this.updateProxyStatusCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.updateProxyStatusCompleted(this, new System.ComponentModel.AsyncCompletedEventArgs(invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("", RequestNamespace="http://service.webservice.accela.com/", ResponseNamespace="http://service.webservice.accela.com/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute("return", Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public PublicUserModel4WS getProxyUsers([System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)] string arg0, [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)] long arg1) {
            object[] results = this.Invoke("getProxyUsers", new object[] {
                        arg0,
                        arg1});
            return ((PublicUserModel4WS)(results[0]));
        }
        
        /// <remarks/>
        public void getProxyUsersAsync(string arg0, long arg1) {
            this.getProxyUsersAsync(arg0, arg1, null);
        }
        
        /// <remarks/>
        public void getProxyUsersAsync(string arg0, long arg1, object userState) {
            if ((this.getProxyUsersOperationCompleted == null)) {
                this.getProxyUsersOperationCompleted = new System.Threading.SendOrPostCallback(this.OngetProxyUsersOperationCompleted);
            }
            this.InvokeAsync("getProxyUsers", new object[] {
                        arg0,
                        arg1}, this.getProxyUsersOperationCompleted, userState);
        }
        
        private void OngetProxyUsersOperationCompleted(object arg) {
            if ((this.getProxyUsersCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.getProxyUsersCompleted(this, new getProxyUsersCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("", RequestNamespace="http://service.webservice.accela.com/", ResponseNamespace="http://service.webservice.accela.com/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute("return", Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public CapIDModel[] validateProxyUserPaymentPermission([System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)] string arg0, [System.Xml.Serialization.XmlElementAttribute("arg1", Form=System.Xml.Schema.XmlSchemaForm.Unqualified)] CapIDModel[] arg1, [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)] string arg2) {
            object[] results = this.Invoke("validateProxyUserPaymentPermission", new object[] {
                        arg0,
                        arg1,
                        arg2});
            return ((CapIDModel[])(results[0]));
        }
        
        /// <remarks/>
        public void validateProxyUserPaymentPermissionAsync(string arg0, CapIDModel[] arg1, string arg2) {
            this.validateProxyUserPaymentPermissionAsync(arg0, arg1, arg2, null);
        }
        
        /// <remarks/>
        public void validateProxyUserPaymentPermissionAsync(string arg0, CapIDModel[] arg1, string arg2, object userState) {
            if ((this.validateProxyUserPaymentPermissionOperationCompleted == null)) {
                this.validateProxyUserPaymentPermissionOperationCompleted = new System.Threading.SendOrPostCallback(this.OnvalidateProxyUserPaymentPermissionOperationCompleted);
            }
            this.InvokeAsync("validateProxyUserPaymentPermission", new object[] {
                        arg0,
                        arg1,
                        arg2}, this.validateProxyUserPaymentPermissionOperationCompleted, userState);
        }
        
        private void OnvalidateProxyUserPaymentPermissionOperationCompleted(object arg) {
            if ((this.validateProxyUserPaymentPermissionCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.validateProxyUserPaymentPermissionCompleted(this, new validateProxyUserPaymentPermissionCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("", RequestNamespace="http://service.webservice.accela.com/", ResponseNamespace="http://service.webservice.accela.com/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public void validateEmail([System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)] string arg0, [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)] string arg1, [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)] string arg2) {
            this.Invoke("validateEmail", new object[] {
                        arg0,
                        arg1,
                        arg2});
        }
        
        /// <remarks/>
        public void validateEmailAsync(string arg0, string arg1, string arg2) {
            this.validateEmailAsync(arg0, arg1, arg2, null);
        }
        
        /// <remarks/>
        public void validateEmailAsync(string arg0, string arg1, string arg2, object userState) {
            if ((this.validateEmailOperationCompleted == null)) {
                this.validateEmailOperationCompleted = new System.Threading.SendOrPostCallback(this.OnvalidateEmailOperationCompleted);
            }
            this.InvokeAsync("validateEmail", new object[] {
                        arg0,
                        arg1,
                        arg2}, this.validateEmailOperationCompleted, userState);
        }
        
        private void OnvalidateEmailOperationCompleted(object arg) {
            if ((this.validateEmailCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.validateEmailCompleted(this, new System.ComponentModel.AsyncCompletedEventArgs(invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("", RequestNamespace="http://service.webservice.accela.com/", ResponseNamespace="http://service.webservice.accela.com/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public void createProxyUser([System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)] XProxyUserModel arg0, [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)] string arg1, [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)] string arg2) {
            this.Invoke("createProxyUser", new object[] {
                        arg0,
                        arg1,
                        arg2});
        }
        
        /// <remarks/>
        public void createProxyUserAsync(XProxyUserModel arg0, string arg1, string arg2) {
            this.createProxyUserAsync(arg0, arg1, arg2, null);
        }
        
        /// <remarks/>
        public void createProxyUserAsync(XProxyUserModel arg0, string arg1, string arg2, object userState) {
            if ((this.createProxyUserOperationCompleted == null)) {
                this.createProxyUserOperationCompleted = new System.Threading.SendOrPostCallback(this.OncreateProxyUserOperationCompleted);
            }
            this.InvokeAsync("createProxyUser", new object[] {
                        arg0,
                        arg1,
                        arg2}, this.createProxyUserOperationCompleted, userState);
        }
        
        private void OncreateProxyUserOperationCompleted(object arg) {
            if ((this.createProxyUserCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.createProxyUserCompleted(this, new System.ComponentModel.AsyncCompletedEventArgs(invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("", RequestNamespace="http://service.webservice.accela.com/", ResponseNamespace="http://service.webservice.accela.com/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public void updatePermissions([System.Xml.Serialization.XmlElementAttribute("arg0", Form=System.Xml.Schema.XmlSchemaForm.Unqualified)] XProxyUserPermissionModel[] arg0, [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)] string arg1) {
            this.Invoke("updatePermissions", new object[] {
                        arg0,
                        arg1});
        }
        
        /// <remarks/>
        public void updatePermissionsAsync(XProxyUserPermissionModel[] arg0, string arg1) {
            this.updatePermissionsAsync(arg0, arg1, null);
        }
        
        /// <remarks/>
        public void updatePermissionsAsync(XProxyUserPermissionModel[] arg0, string arg1, object userState) {
            if ((this.updatePermissionsOperationCompleted == null)) {
                this.updatePermissionsOperationCompleted = new System.Threading.SendOrPostCallback(this.OnupdatePermissionsOperationCompleted);
            }
            this.InvokeAsync("updatePermissions", new object[] {
                        arg0,
                        arg1}, this.updatePermissionsOperationCompleted, userState);
        }
        
        private void OnupdatePermissionsOperationCompleted(object arg) {
            if ((this.updatePermissionsCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.updatePermissionsCompleted(this, new System.ComponentModel.AsyncCompletedEventArgs(invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
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
    public delegate void deleteProxyUserCompletedEventHandler(object sender, System.ComponentModel.AsyncCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "2.0.50727.3053")]
    public delegate void updateProxyStatusCompletedEventHandler(object sender, System.ComponentModel.AsyncCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "2.0.50727.3053")]
    public delegate void getProxyUsersCompletedEventHandler(object sender, getProxyUsersCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "2.0.50727.3053")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class getProxyUsersCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal getProxyUsersCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public PublicUserModel4WS Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((PublicUserModel4WS)(this.results[0]));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "2.0.50727.3053")]
    public delegate void validateProxyUserPaymentPermissionCompletedEventHandler(object sender, validateProxyUserPaymentPermissionCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "2.0.50727.3053")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class validateProxyUserPaymentPermissionCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal validateProxyUserPaymentPermissionCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public CapIDModel[] Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((CapIDModel[])(this.results[0]));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "2.0.50727.3053")]
    public delegate void validateEmailCompletedEventHandler(object sender, System.ComponentModel.AsyncCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "2.0.50727.3053")]
    public delegate void createProxyUserCompletedEventHandler(object sender, System.ComponentModel.AsyncCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "2.0.50727.3053")]
    public delegate void updatePermissionsCompletedEventHandler(object sender, System.ComponentModel.AsyncCompletedEventArgs e);
}

#pragma warning restore 1591