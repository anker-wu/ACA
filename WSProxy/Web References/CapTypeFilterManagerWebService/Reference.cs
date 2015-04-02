﻿/**
 * <pre>
 * 
 *  Accela Citizen Access
 *  File: CapModel4WS.cs
 * 
 *  Accela, Inc.
 *  Copyright (C): 2007-2012
 * 
 *  Description:
 * 
 *  Notes:
 * $Id: Reference.cs 234112 2012-09-29 04:20:24Z ACHIEVO\daly.zeng $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.269
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

// 
// This source code was auto-generated by Microsoft.VSDesigner, Version 4.0.30319.269.
// 
#pragma warning disable 1591

namespace Accela.ACA.WSProxy {
    using System;
    using System.Web.Services;
    using System.Diagnostics;
    using System.Web.Services.Protocols;
    using System.ComponentModel;
    using System.Xml.Serialization;
    using Accela.ACA.WSProxy.WSModel;
    
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.1")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Web.Services.WebServiceBindingAttribute(Name="CapTypeFilterManagerWebServiceServiceSoapBinding", Namespace="http://service.webservice.accela.com/")]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(LanguageModel))]
    public partial class CapTypeFilterManagerWebServiceService : System.Web.Services.Protocols.SoapHttpClientProtocol {
        
        private System.Threading.SendOrPostCallback createOrEditFilter4ButtonOperationCompleted;
        
        private System.Threading.SendOrPostCallback getFilter4ButtonOperationCompleted;
        
        private System.Threading.SendOrPostCallback getCapTypeFilterListOperationCompleted;
        
        private System.Threading.SendOrPostCallback editFilter4ButtonAgencyLevelOperationCompleted;
        
        private System.Threading.SendOrPostCallback getCapTypeFilterModelOperationCompleted;
        
        private System.Threading.SendOrPostCallback createCapTypeFilterOperationCompleted;
        
        private System.Threading.SendOrPostCallback getFilter4ButtonListByFilterNameOperationCompleted;
        
        private System.Threading.SendOrPostCallback deleteCapTypeFilterOperationCompleted;
        
        private System.Threading.SendOrPostCallback getCapTypeFilterListByModuleOperationCompleted;
        
        private System.Threading.SendOrPostCallback editCapTypeFilterOperationCompleted;
        
        private System.Threading.SendOrPostCallback getAllRelationOnButton2FilterOperationCompleted;
        
        private bool useDefaultCredentialsSetExplicitly;
        
        /// <remarks/>
        public CapTypeFilterManagerWebServiceService() {
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
        public event createOrEditFilter4ButtonCompletedEventHandler createOrEditFilter4ButtonCompleted;
        
        /// <remarks/>
        public event getFilter4ButtonCompletedEventHandler getFilter4ButtonCompleted;
        
        /// <remarks/>
        public event getCapTypeFilterListCompletedEventHandler getCapTypeFilterListCompleted;
        
        /// <remarks/>
        public event editFilter4ButtonAgencyLevelCompletedEventHandler editFilter4ButtonAgencyLevelCompleted;
        
        /// <remarks/>
        public event getCapTypeFilterModelCompletedEventHandler getCapTypeFilterModelCompleted;
        
        /// <remarks/>
        public event createCapTypeFilterCompletedEventHandler createCapTypeFilterCompleted;
        
        /// <remarks/>
        public event getFilter4ButtonListByFilterNameCompletedEventHandler getFilter4ButtonListByFilterNameCompleted;
        
        /// <remarks/>
        public event deleteCapTypeFilterCompletedEventHandler deleteCapTypeFilterCompleted;
        
        /// <remarks/>
        public event getCapTypeFilterListByModuleCompletedEventHandler getCapTypeFilterListByModuleCompleted;
        
        /// <remarks/>
        public event editCapTypeFilterCompletedEventHandler editCapTypeFilterCompleted;
        
        /// <remarks/>
        public event getAllRelationOnButton2FilterCompletedEventHandler getAllRelationOnButton2FilterCompleted;
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("", RequestNamespace="http://service.webservice.accela.com/", ResponseNamespace="http://service.webservice.accela.com/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public void createOrEditFilter4Button([System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)] XButtonFilterModel4WS arg0, [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)] string arg1) {
            this.Invoke("createOrEditFilter4Button", new object[] {
                        arg0,
                        arg1});
        }
        
        /// <remarks/>
        public void createOrEditFilter4ButtonAsync(XButtonFilterModel4WS arg0, string arg1) {
            this.createOrEditFilter4ButtonAsync(arg0, arg1, null);
        }
        
        /// <remarks/>
        public void createOrEditFilter4ButtonAsync(XButtonFilterModel4WS arg0, string arg1, object userState) {
            if ((this.createOrEditFilter4ButtonOperationCompleted == null)) {
                this.createOrEditFilter4ButtonOperationCompleted = new System.Threading.SendOrPostCallback(this.OncreateOrEditFilter4ButtonOperationCompleted);
            }
            this.InvokeAsync("createOrEditFilter4Button", new object[] {
                        arg0,
                        arg1}, this.createOrEditFilter4ButtonOperationCompleted, userState);
        }
        
        private void OncreateOrEditFilter4ButtonOperationCompleted(object arg) {
            if ((this.createOrEditFilter4ButtonCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.createOrEditFilter4ButtonCompleted(this, new System.ComponentModel.AsyncCompletedEventArgs(invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("", RequestNamespace="http://service.webservice.accela.com/", ResponseNamespace="http://service.webservice.accela.com/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute("return", Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public XButtonFilterModel4WS getFilter4Button([System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)] string arg0, [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)] string arg1, [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)] string arg2, [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)] string arg3) {
            object[] results = this.Invoke("getFilter4Button", new object[] {
                        arg0,
                        arg1,
                        arg2,
                        arg3});
            return ((XButtonFilterModel4WS)(results[0]));
        }
        
        /// <remarks/>
        public void getFilter4ButtonAsync(string arg0, string arg1, string arg2, string arg3) {
            this.getFilter4ButtonAsync(arg0, arg1, arg2, arg3, null);
        }
        
        /// <remarks/>
        public void getFilter4ButtonAsync(string arg0, string arg1, string arg2, string arg3, object userState) {
            if ((this.getFilter4ButtonOperationCompleted == null)) {
                this.getFilter4ButtonOperationCompleted = new System.Threading.SendOrPostCallback(this.OngetFilter4ButtonOperationCompleted);
            }
            this.InvokeAsync("getFilter4Button", new object[] {
                        arg0,
                        arg1,
                        arg2,
                        arg3}, this.getFilter4ButtonOperationCompleted, userState);
        }
        
        private void OngetFilter4ButtonOperationCompleted(object arg) {
            if ((this.getFilter4ButtonCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.getFilter4ButtonCompleted(this, new getFilter4ButtonCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("", RequestNamespace="http://service.webservice.accela.com/", ResponseNamespace="http://service.webservice.accela.com/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute("return", Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public CapTypeFilterModel4WS[] getCapTypeFilterList([System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)] string arg0, [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)] string arg1) {
            object[] results = this.Invoke("getCapTypeFilterList", new object[] {
                        arg0,
                        arg1});
            return ((CapTypeFilterModel4WS[])(results[0]));
        }
        
        /// <remarks/>
        public void getCapTypeFilterListAsync(string arg0, string arg1) {
            this.getCapTypeFilterListAsync(arg0, arg1, null);
        }
        
        /// <remarks/>
        public void getCapTypeFilterListAsync(string arg0, string arg1, object userState) {
            if ((this.getCapTypeFilterListOperationCompleted == null)) {
                this.getCapTypeFilterListOperationCompleted = new System.Threading.SendOrPostCallback(this.OngetCapTypeFilterListOperationCompleted);
            }
            this.InvokeAsync("getCapTypeFilterList", new object[] {
                        arg0,
                        arg1}, this.getCapTypeFilterListOperationCompleted, userState);
        }
        
        private void OngetCapTypeFilterListOperationCompleted(object arg) {
            if ((this.getCapTypeFilterListCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.getCapTypeFilterListCompleted(this, new getCapTypeFilterListCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("", RequestNamespace="http://service.webservice.accela.com/", ResponseNamespace="http://service.webservice.accela.com/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public void editFilter4ButtonAgencyLevel([System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)] XButtonFilterModel4WS arg0, [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)] string arg1) {
            this.Invoke("editFilter4ButtonAgencyLevel", new object[] {
                        arg0,
                        arg1});
        }
        
        /// <remarks/>
        public void editFilter4ButtonAgencyLevelAsync(XButtonFilterModel4WS arg0, string arg1) {
            this.editFilter4ButtonAgencyLevelAsync(arg0, arg1, null);
        }
        
        /// <remarks/>
        public void editFilter4ButtonAgencyLevelAsync(XButtonFilterModel4WS arg0, string arg1, object userState) {
            if ((this.editFilter4ButtonAgencyLevelOperationCompleted == null)) {
                this.editFilter4ButtonAgencyLevelOperationCompleted = new System.Threading.SendOrPostCallback(this.OneditFilter4ButtonAgencyLevelOperationCompleted);
            }
            this.InvokeAsync("editFilter4ButtonAgencyLevel", new object[] {
                        arg0,
                        arg1}, this.editFilter4ButtonAgencyLevelOperationCompleted, userState);
        }
        
        private void OneditFilter4ButtonAgencyLevelOperationCompleted(object arg) {
            if ((this.editFilter4ButtonAgencyLevelCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.editFilter4ButtonAgencyLevelCompleted(this, new System.ComponentModel.AsyncCompletedEventArgs(invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("", RequestNamespace="http://service.webservice.accela.com/", ResponseNamespace="http://service.webservice.accela.com/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute("return", Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public CapTypeFilterModel4WS getCapTypeFilterModel([System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)] string arg0, [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)] string arg1, [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)] string arg2, [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)] string arg3) {
            object[] results = this.Invoke("getCapTypeFilterModel", new object[] {
                        arg0,
                        arg1,
                        arg2,
                        arg3});
            return ((CapTypeFilterModel4WS)(results[0]));
        }
        
        /// <remarks/>
        public void getCapTypeFilterModelAsync(string arg0, string arg1, string arg2, string arg3) {
            this.getCapTypeFilterModelAsync(arg0, arg1, arg2, arg3, null);
        }
        
        /// <remarks/>
        public void getCapTypeFilterModelAsync(string arg0, string arg1, string arg2, string arg3, object userState) {
            if ((this.getCapTypeFilterModelOperationCompleted == null)) {
                this.getCapTypeFilterModelOperationCompleted = new System.Threading.SendOrPostCallback(this.OngetCapTypeFilterModelOperationCompleted);
            }
            this.InvokeAsync("getCapTypeFilterModel", new object[] {
                        arg0,
                        arg1,
                        arg2,
                        arg3}, this.getCapTypeFilterModelOperationCompleted, userState);
        }
        
        private void OngetCapTypeFilterModelOperationCompleted(object arg) {
            if ((this.getCapTypeFilterModelCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.getCapTypeFilterModelCompleted(this, new getCapTypeFilterModelCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("", RequestNamespace="http://service.webservice.accela.com/", ResponseNamespace="http://service.webservice.accela.com/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public void createCapTypeFilter([System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)] CapTypeFilterModel4WS arg0, [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)] string arg1) {
            this.Invoke("createCapTypeFilter", new object[] {
                        arg0,
                        arg1});
        }
        
        /// <remarks/>
        public void createCapTypeFilterAsync(CapTypeFilterModel4WS arg0, string arg1) {
            this.createCapTypeFilterAsync(arg0, arg1, null);
        }
        
        /// <remarks/>
        public void createCapTypeFilterAsync(CapTypeFilterModel4WS arg0, string arg1, object userState) {
            if ((this.createCapTypeFilterOperationCompleted == null)) {
                this.createCapTypeFilterOperationCompleted = new System.Threading.SendOrPostCallback(this.OncreateCapTypeFilterOperationCompleted);
            }
            this.InvokeAsync("createCapTypeFilter", new object[] {
                        arg0,
                        arg1}, this.createCapTypeFilterOperationCompleted, userState);
        }
        
        private void OncreateCapTypeFilterOperationCompleted(object arg) {
            if ((this.createCapTypeFilterCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.createCapTypeFilterCompleted(this, new System.ComponentModel.AsyncCompletedEventArgs(invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("", RequestNamespace="http://service.webservice.accela.com/", ResponseNamespace="http://service.webservice.accela.com/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute("return", Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public XButtonFilterModel4WS[] getFilter4ButtonListByFilterName([System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)] string arg0, [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)] string arg1, [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)] string arg2, [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)] string arg3) {
            object[] results = this.Invoke("getFilter4ButtonListByFilterName", new object[] {
                        arg0,
                        arg1,
                        arg2,
                        arg3});
            return ((XButtonFilterModel4WS[])(results[0]));
        }
        
        /// <remarks/>
        public void getFilter4ButtonListByFilterNameAsync(string arg0, string arg1, string arg2, string arg3) {
            this.getFilter4ButtonListByFilterNameAsync(arg0, arg1, arg2, arg3, null);
        }
        
        /// <remarks/>
        public void getFilter4ButtonListByFilterNameAsync(string arg0, string arg1, string arg2, string arg3, object userState) {
            if ((this.getFilter4ButtonListByFilterNameOperationCompleted == null)) {
                this.getFilter4ButtonListByFilterNameOperationCompleted = new System.Threading.SendOrPostCallback(this.OngetFilter4ButtonListByFilterNameOperationCompleted);
            }
            this.InvokeAsync("getFilter4ButtonListByFilterName", new object[] {
                        arg0,
                        arg1,
                        arg2,
                        arg3}, this.getFilter4ButtonListByFilterNameOperationCompleted, userState);
        }
        
        private void OngetFilter4ButtonListByFilterNameOperationCompleted(object arg) {
            if ((this.getFilter4ButtonListByFilterNameCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.getFilter4ButtonListByFilterNameCompleted(this, new getFilter4ButtonListByFilterNameCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("", RequestNamespace="http://service.webservice.accela.com/", ResponseNamespace="http://service.webservice.accela.com/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public void deleteCapTypeFilter([System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)] string arg0, [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)] string arg1, [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)] string arg2, [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)] string arg3) {
            this.Invoke("deleteCapTypeFilter", new object[] {
                        arg0,
                        arg1,
                        arg2,
                        arg3});
        }
        
        /// <remarks/>
        public void deleteCapTypeFilterAsync(string arg0, string arg1, string arg2, string arg3) {
            this.deleteCapTypeFilterAsync(arg0, arg1, arg2, arg3, null);
        }
        
        /// <remarks/>
        public void deleteCapTypeFilterAsync(string arg0, string arg1, string arg2, string arg3, object userState) {
            if ((this.deleteCapTypeFilterOperationCompleted == null)) {
                this.deleteCapTypeFilterOperationCompleted = new System.Threading.SendOrPostCallback(this.OndeleteCapTypeFilterOperationCompleted);
            }
            this.InvokeAsync("deleteCapTypeFilter", new object[] {
                        arg0,
                        arg1,
                        arg2,
                        arg3}, this.deleteCapTypeFilterOperationCompleted, userState);
        }
        
        private void OndeleteCapTypeFilterOperationCompleted(object arg) {
            if ((this.deleteCapTypeFilterCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.deleteCapTypeFilterCompleted(this, new System.ComponentModel.AsyncCompletedEventArgs(invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("", RequestNamespace="http://service.webservice.accela.com/", ResponseNamespace="http://service.webservice.accela.com/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute("return", Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string[] getCapTypeFilterListByModule([System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)] string arg0, [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)] string arg1, [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)] string arg2) {
            object[] results = this.Invoke("getCapTypeFilterListByModule", new object[] {
                        arg0,
                        arg1,
                        arg2});
            return ((string[])(results[0]));
        }
        
        /// <remarks/>
        public void getCapTypeFilterListByModuleAsync(string arg0, string arg1, string arg2) {
            this.getCapTypeFilterListByModuleAsync(arg0, arg1, arg2, null);
        }
        
        /// <remarks/>
        public void getCapTypeFilterListByModuleAsync(string arg0, string arg1, string arg2, object userState) {
            if ((this.getCapTypeFilterListByModuleOperationCompleted == null)) {
                this.getCapTypeFilterListByModuleOperationCompleted = new System.Threading.SendOrPostCallback(this.OngetCapTypeFilterListByModuleOperationCompleted);
            }
            this.InvokeAsync("getCapTypeFilterListByModule", new object[] {
                        arg0,
                        arg1,
                        arg2}, this.getCapTypeFilterListByModuleOperationCompleted, userState);
        }
        
        private void OngetCapTypeFilterListByModuleOperationCompleted(object arg) {
            if ((this.getCapTypeFilterListByModuleCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.getCapTypeFilterListByModuleCompleted(this, new getCapTypeFilterListByModuleCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("", RequestNamespace="http://service.webservice.accela.com/", ResponseNamespace="http://service.webservice.accela.com/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public void editCapTypeFilter([System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)] CapTypeFilterModel4WS arg0, [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)] string arg1) {
            this.Invoke("editCapTypeFilter", new object[] {
                        arg0,
                        arg1});
        }
        
        /// <remarks/>
        public void editCapTypeFilterAsync(CapTypeFilterModel4WS arg0, string arg1) {
            this.editCapTypeFilterAsync(arg0, arg1, null);
        }
        
        /// <remarks/>
        public void editCapTypeFilterAsync(CapTypeFilterModel4WS arg0, string arg1, object userState) {
            if ((this.editCapTypeFilterOperationCompleted == null)) {
                this.editCapTypeFilterOperationCompleted = new System.Threading.SendOrPostCallback(this.OneditCapTypeFilterOperationCompleted);
            }
            this.InvokeAsync("editCapTypeFilter", new object[] {
                        arg0,
                        arg1}, this.editCapTypeFilterOperationCompleted, userState);
        }
        
        private void OneditCapTypeFilterOperationCompleted(object arg) {
            if ((this.editCapTypeFilterCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.editCapTypeFilterCompleted(this, new System.ComponentModel.AsyncCompletedEventArgs(invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("", RequestNamespace="http://service.webservice.accela.com/", ResponseNamespace="http://service.webservice.accela.com/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute("return", Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public XButtonFilterModel4WS[] getAllRelationOnButton2Filter([System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)] string arg0, [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)] string arg1) {
            object[] results = this.Invoke("getAllRelationOnButton2Filter", new object[] {
                        arg0,
                        arg1});
            return ((XButtonFilterModel4WS[])(results[0]));
        }
        
        /// <remarks/>
        public void getAllRelationOnButton2FilterAsync(string arg0, string arg1) {
            this.getAllRelationOnButton2FilterAsync(arg0, arg1, null);
        }
        
        /// <remarks/>
        public void getAllRelationOnButton2FilterAsync(string arg0, string arg1, object userState) {
            if ((this.getAllRelationOnButton2FilterOperationCompleted == null)) {
                this.getAllRelationOnButton2FilterOperationCompleted = new System.Threading.SendOrPostCallback(this.OngetAllRelationOnButton2FilterOperationCompleted);
            }
            this.InvokeAsync("getAllRelationOnButton2Filter", new object[] {
                        arg0,
                        arg1}, this.getAllRelationOnButton2FilterOperationCompleted, userState);
        }
        
        private void OngetAllRelationOnButton2FilterOperationCompleted(object arg) {
            if ((this.getAllRelationOnButton2FilterCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.getAllRelationOnButton2FilterCompleted(this, new getAllRelationOnButton2FilterCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
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
    public delegate void createOrEditFilter4ButtonCompletedEventHandler(object sender, System.ComponentModel.AsyncCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.1")]
    public delegate void getFilter4ButtonCompletedEventHandler(object sender, getFilter4ButtonCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.1")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class getFilter4ButtonCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal getFilter4ButtonCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public XButtonFilterModel4WS Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((XButtonFilterModel4WS)(this.results[0]));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.1")]
    public delegate void getCapTypeFilterListCompletedEventHandler(object sender, getCapTypeFilterListCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.1")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class getCapTypeFilterListCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal getCapTypeFilterListCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public CapTypeFilterModel4WS[] Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((CapTypeFilterModel4WS[])(this.results[0]));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.1")]
    public delegate void editFilter4ButtonAgencyLevelCompletedEventHandler(object sender, System.ComponentModel.AsyncCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.1")]
    public delegate void getCapTypeFilterModelCompletedEventHandler(object sender, getCapTypeFilterModelCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.1")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class getCapTypeFilterModelCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal getCapTypeFilterModelCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public CapTypeFilterModel4WS Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((CapTypeFilterModel4WS)(this.results[0]));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.1")]
    public delegate void createCapTypeFilterCompletedEventHandler(object sender, System.ComponentModel.AsyncCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.1")]
    public delegate void getFilter4ButtonListByFilterNameCompletedEventHandler(object sender, getFilter4ButtonListByFilterNameCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.1")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class getFilter4ButtonListByFilterNameCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal getFilter4ButtonListByFilterNameCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public XButtonFilterModel4WS[] Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((XButtonFilterModel4WS[])(this.results[0]));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.1")]
    public delegate void deleteCapTypeFilterCompletedEventHandler(object sender, System.ComponentModel.AsyncCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.1")]
    public delegate void getCapTypeFilterListByModuleCompletedEventHandler(object sender, getCapTypeFilterListByModuleCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.1")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class getCapTypeFilterListByModuleCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal getCapTypeFilterListByModuleCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public string[] Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((string[])(this.results[0]));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.1")]
    public delegate void editCapTypeFilterCompletedEventHandler(object sender, System.ComponentModel.AsyncCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.1")]
    public delegate void getAllRelationOnButton2FilterCompletedEventHandler(object sender, getAllRelationOnButton2FilterCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.1")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class getAllRelationOnButton2FilterCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal getAllRelationOnButton2FilterCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public XButtonFilterModel4WS[] Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((XButtonFilterModel4WS[])(this.results[0]));
            }
        }
    }
}

#pragma warning restore 1591