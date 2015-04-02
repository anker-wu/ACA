﻿/**
 * <pre>
 * 
 *  Accela Citizen Access
 *  File: AppStatusGroupWebService.cs
 * 
 *  Accela, Inc.
 *  Copyright (C): 2007-2011
 * 
 *  Description:
 * 
 *  Notes:
 * $Id: Reference.cs 188126 2011-01-05 06:13:35Z ACHIEVO\daly.zeng $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:2.0.50727.3053
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

// 
// This source code was auto-generated by Microsoft.VSDesigner, Version 2.0.50727.3053.
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
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.1")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Web.Services.WebServiceBindingAttribute(Name="AppStatusGroupWebServiceServiceSoapBinding", Namespace="http://service.webservice.accela.com/")]
    public partial class AppStatusGroupWebServiceService : System.Web.Services.Protocols.SoapHttpClientProtocol {
        
        private System.Threading.SendOrPostCallback getAppStatusGroupBySPCOperationCompleted;
        
        private System.Threading.SendOrPostCallback editAppStatusGroupOperationCompleted;
        
        private System.Threading.SendOrPostCallback getAppStatusByCapTypeOperationCompleted;
        
        private bool useDefaultCredentialsSetExplicitly;
        
        /// <remarks/>
        public AppStatusGroupWebServiceService() {
            
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
        public event getAppStatusGroupBySPCCompletedEventHandler getAppStatusGroupBySPCCompleted;
        
        /// <remarks/>
        public event editAppStatusGroupCompletedEventHandler editAppStatusGroupCompleted;
        
        /// <remarks/>
        public event getAppStatusByCapTypeCompletedEventHandler getAppStatusByCapTypeCompleted;
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("", RequestNamespace="http://service.webservice.accela.com/", ResponseNamespace="http://service.webservice.accela.com/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute("return", Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public AppStatusGroupModel4WS[] getAppStatusGroupBySPC([System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)] string arg0, [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)] string arg1) {
            object[] results = this.Invoke("getAppStatusGroupBySPC", new object[] {
                        arg0,
                        arg1});
            return ((AppStatusGroupModel4WS[])(results[0]));
        }
        
        /// <remarks/>
        public void getAppStatusGroupBySPCAsync(string arg0, string arg1) {
            this.getAppStatusGroupBySPCAsync(arg0, arg1, null);
        }
        
        /// <remarks/>
        public void getAppStatusGroupBySPCAsync(string arg0, string arg1, object userState) {
            if ((this.getAppStatusGroupBySPCOperationCompleted == null)) {
                this.getAppStatusGroupBySPCOperationCompleted = new System.Threading.SendOrPostCallback(this.OngetAppStatusGroupBySPCOperationCompleted);
            }
            this.InvokeAsync("getAppStatusGroupBySPC", new object[] {
                        arg0,
                        arg1}, this.getAppStatusGroupBySPCOperationCompleted, userState);
        }
        
        private void OngetAppStatusGroupBySPCOperationCompleted(object arg) {
            if ((this.getAppStatusGroupBySPCCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.getAppStatusGroupBySPCCompleted(this, new getAppStatusGroupBySPCCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("", RequestNamespace="http://service.webservice.accela.com/", ResponseNamespace="http://service.webservice.accela.com/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public void editAppStatusGroup([System.Xml.Serialization.XmlElementAttribute("arg0", Form=System.Xml.Schema.XmlSchemaForm.Unqualified)] AppStatusGroupModel4WS[] arg0) {
            this.Invoke("editAppStatusGroup", new object[] {
                        arg0});
        }
        
        /// <remarks/>
        public void editAppStatusGroupAsync(AppStatusGroupModel4WS[] arg0) {
            this.editAppStatusGroupAsync(arg0, null);
        }
        
        /// <remarks/>
        public void editAppStatusGroupAsync(AppStatusGroupModel4WS[] arg0, object userState) {
            if ((this.editAppStatusGroupOperationCompleted == null)) {
                this.editAppStatusGroupOperationCompleted = new System.Threading.SendOrPostCallback(this.OneditAppStatusGroupOperationCompleted);
            }
            this.InvokeAsync("editAppStatusGroup", new object[] {
                        arg0}, this.editAppStatusGroupOperationCompleted, userState);
        }
        
        private void OneditAppStatusGroupOperationCompleted(object arg) {
            if ((this.editAppStatusGroupCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.editAppStatusGroupCompleted(this, new System.ComponentModel.AsyncCompletedEventArgs(invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("", RequestNamespace="http://service.webservice.accela.com/", ResponseNamespace="http://service.webservice.accela.com/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute("return", Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public AppStatusGroupModel4WS[] getAppStatusByCapType([System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)] string arg0, [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)] CapTypeModel arg1, [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)] string arg2) {
            object[] results = this.Invoke("getAppStatusByCapType", new object[] {
                        arg0,
                        arg1,
                        arg2});
            return ((AppStatusGroupModel4WS[])(results[0]));
        }
        
        /// <remarks/>
        public void getAppStatusByCapTypeAsync(string arg0, CapTypeModel arg1, string arg2) {
            this.getAppStatusByCapTypeAsync(arg0, arg1, arg2, null);
        }
        
        /// <remarks/>
        public void getAppStatusByCapTypeAsync(string arg0, CapTypeModel arg1, string arg2, object userState) {
            if ((this.getAppStatusByCapTypeOperationCompleted == null)) {
                this.getAppStatusByCapTypeOperationCompleted = new System.Threading.SendOrPostCallback(this.OngetAppStatusByCapTypeOperationCompleted);
            }
            this.InvokeAsync("getAppStatusByCapType", new object[] {
                        arg0,
                        arg1,
                        arg2}, this.getAppStatusByCapTypeOperationCompleted, userState);
        }
        
        private void OngetAppStatusByCapTypeOperationCompleted(object arg) {
            if ((this.getAppStatusByCapTypeCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.getAppStatusByCapTypeCompleted(this, new getAppStatusByCapTypeCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
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
    public delegate void getAppStatusGroupBySPCCompletedEventHandler(object sender, getAppStatusGroupBySPCCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.1")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class getAppStatusGroupBySPCCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal getAppStatusGroupBySPCCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public AppStatusGroupModel4WS[] Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((AppStatusGroupModel4WS[])(this.results[0]));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.1")]
    public delegate void editAppStatusGroupCompletedEventHandler(object sender, System.ComponentModel.AsyncCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.1")]
    public delegate void getAppStatusByCapTypeCompletedEventHandler(object sender, getAppStatusByCapTypeCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.1")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class getAppStatusByCapTypeCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal getAppStatusByCapTypeCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public AppStatusGroupModel4WS[] Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((AppStatusGroupModel4WS[])(this.results[0]));
            }
        }
    }
}

#pragma warning restore 1591