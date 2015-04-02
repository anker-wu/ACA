﻿/**
 * <pre>
 * 
 *  Accela Citizen Access
 *  File: Reference.cs
 * 
 *  Accela, Inc.
 *  Copyright (C): 2014
 * 
 *  Description:
 * 
 *  Notes:
 * $Id: Reference.cs 198626 2011-07-04 01:28:35Z ACHIEVO\daly.zeng $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.18444
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

// 
// This source code was auto-generated by Microsoft.VSDesigner, Version 4.0.30319.18444.
// 
#pragma warning disable 1591

namespace Accela.ACA.WSProxy {
    using System;
    using System.Web.Services;
    using System.Diagnostics;
    using System.Web.Services.Protocols;
    using System.Xml.Serialization;
    using System.ComponentModel;
    
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.18408")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Web.Services.WebServiceBindingAttribute(Name="APOWebServiceServiceSoapBinding", Namespace="http://service.webservice.accela.com/")]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(AAException))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(objectFactory))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(getRefAddressListByParcelPK))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(getOwnerListByParcelPKs))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(getRefAddressListByParcel))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(getRefOwnerList))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(getRefParcelList))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(getRefAddressList))]
    public partial class APOWebServiceService : System.Web.Services.Protocols.SoapHttpClientProtocol {
        
        private System.Threading.SendOrPostCallback getRefAddressListOperationCompleted;
        
        private System.Threading.SendOrPostCallback getRefParcelListOperationCompleted;

        private System.Threading.SendOrPostCallback getRefAddressListByParcelOperationCompleted;
        
        private System.Threading.SendOrPostCallback getRefOwnerListOperationCompleted;

        private System.Threading.SendOrPostCallback getOwnerListByParcelPKsOperationCompleted;
        
        private System.Threading.SendOrPostCallback getRefAddressListByParcelPKOperationCompleted;
        
        private bool useDefaultCredentialsSetExplicitly;
        
        /// <remarks/>
        public APOWebServiceService() {
            if ((this.IsLocalFileSystemWebService(this.Url) == true))
            {
                this.UseDefaultCredentials = true;
                this.useDefaultCredentialsSetExplicitly = false;
            }
            else
            {
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
        public event getRefAddressListCompletedEventHandler getRefAddressListCompleted;
        
        /// <remarks/>
        public event getRefParcelListCompletedEventHandler getRefParcelListCompleted;

        /// <remarks/>
        public event getRefAddressListByParcelCompletedEventHandler getRefAddressListByParcelCompleted;
        
        /// <remarks/>
        public event getRefOwnerListCompletedEventHandler getRefOwnerListCompleted;

        /// <remarks/>
        public event getOwnerListByParcelPKsCompletedEventHandler getOwnerListByParcelPKsCompleted;
        
        /// <remarks/>
        public event getRefAddressListByParcelPKCompletedEventHandler getRefAddressListByParcelPKCompleted;
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("", RequestNamespace="http://service.webservice.accela.com/", ResponseNamespace="http://service.webservice.accela.com/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute("return", Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public SearchResultModel getRefAddressList([System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)] string arg0, [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)] RefAddressModel arg1, [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)] QueryFormat arg2)
        {
            object[] results = this.Invoke("getRefAddressList", new object[] {
                        arg0,
                        arg1,
                        arg2});
            return ((SearchResultModel)(results[0]));
        }
        
        /// <remarks/>
        public void getRefAddressListAsync(string arg0, RefAddressModel arg1, QueryFormat arg2)
        {
            this.getRefAddressListAsync(arg0, arg1, arg2, null);
        }
        
        /// <remarks/>
        public void getRefAddressListAsync(string arg0, RefAddressModel arg1, QueryFormat arg2, object userState)
        {
            if ((this.getRefAddressListOperationCompleted == null)) {
                this.getRefAddressListOperationCompleted = new System.Threading.SendOrPostCallback(this.OngetRefAddressListOperationCompleted);
            }
            this.InvokeAsync("getRefAddressList", new object[] {
                        arg0,
                        arg1,
                        arg2}, this.getRefAddressListOperationCompleted, userState);
        }
        
        private void OngetRefAddressListOperationCompleted(object arg) {
            if ((this.getRefAddressListCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.getRefAddressListCompleted(this, new getRefAddressListCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("", RequestNamespace="http://service.webservice.accela.com/", ResponseNamespace="http://service.webservice.accela.com/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute("return", Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public SearchResultModel getRefParcelList([System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)] string arg0, [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)] ParcelModel arg1, [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)] QueryFormat arg2)
        {
            object[] results = this.Invoke("getRefParcelList", new object[] {
                        arg0,
                        arg1,
                        arg2});
            return ((SearchResultModel)(results[0]));
        }
        
        /// <remarks/>
        public void getRefParcelListAsync(string arg0, ParcelModel arg1, QueryFormat arg2)
        {
            this.getRefParcelListAsync(arg0, arg1, arg2, null);
        }
        
        /// <remarks/>
        public void getRefParcelListAsync(string arg0, ParcelModel arg1, QueryFormat arg2, object userState)
        {
            if ((this.getRefParcelListOperationCompleted == null)) {
                this.getRefParcelListOperationCompleted = new System.Threading.SendOrPostCallback(this.OngetRefParcelListOperationCompleted);
            }
            this.InvokeAsync("getRefParcelList", new object[] {
                        arg0,
                        arg1,
                        arg2}, this.getRefParcelListOperationCompleted, userState);
        }
        
        private void OngetRefParcelListOperationCompleted(object arg) {
            if ((this.getRefParcelListCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.getRefParcelListCompleted(this, new getRefParcelListCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }

        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("", RequestNamespace = "http://service.webservice.accela.com/", ResponseNamespace = "http://service.webservice.accela.com/", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute("return", Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public SearchResultModel getRefAddressListByParcel([System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)] string arg0, [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)] ParcelModel arg1, [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)] QueryFormat arg2)
        {
            object[] results = this.Invoke("getRefAddressListByParcel", new object[] {
                        arg0,
                        arg1,
                        arg2});
            return ((SearchResultModel)(results[0]));
        }

        /// <remarks/>
        public void getRefAddressListByParcelAsync(string arg0, ParcelModel arg1, QueryFormat arg2)
        {
            this.getRefAddressListByParcelAsync(arg0, arg1, arg2, null);
        }

        /// <remarks/>
        public void getRefAddressListByParcelAsync(string arg0, ParcelModel arg1, QueryFormat arg2, object userState)
        {
            if ((this.getRefAddressListByParcelOperationCompleted == null))
            {
                this.getRefAddressListByParcelOperationCompleted = new System.Threading.SendOrPostCallback(this.OngetRefAddressListByParcelOperationCompleted);
            }
            this.InvokeAsync("getRefAddressListByParcel", new object[] {
                        arg0,
                        arg1,
                        arg2}, this.getRefAddressListByParcelOperationCompleted, userState);
        }

        private void OngetRefAddressListByParcelOperationCompleted(object arg)
        {
            if ((this.getRefAddressListByParcelCompleted != null))
            {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.getRefAddressListByParcelCompleted(this, new getRefAddressListByParcelCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("", RequestNamespace="http://service.webservice.accela.com/", ResponseNamespace="http://service.webservice.accela.com/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute("return", Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public SearchResultModel getRefOwnerList([System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)] string arg0, [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)] OwnerCompModel arg1, [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)] QueryFormat arg2)
        {
            object[] results = this.Invoke("getRefOwnerList", new object[] {
                        arg0,
                        arg1,
                        arg2});
            return ((SearchResultModel)(results[0]));
        }
        
        /// <remarks/>
        public void getRefOwnerListAsync(string arg0, OwnerCompModel arg1, QueryFormat arg2)
        {
            this.getRefOwnerListAsync(arg0, arg1, arg2, null);
        }
        
        /// <remarks/>
        public void getRefOwnerListAsync(string arg0, OwnerCompModel arg1, QueryFormat arg2, object userState)
        {
            if ((this.getRefOwnerListOperationCompleted == null)) {
                this.getRefOwnerListOperationCompleted = new System.Threading.SendOrPostCallback(this.OngetRefOwnerListOperationCompleted);
            }
            this.InvokeAsync("getRefOwnerList", new object[] {
                        arg0,
                        arg1,
                        arg2}, this.getRefOwnerListOperationCompleted, userState);
        }
        
        private void OngetRefOwnerListOperationCompleted(object arg) {
            if ((this.getRefOwnerListCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.getRefOwnerListCompleted(this, new getRefOwnerListCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }

        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("", RequestNamespace="http://service.webservice.accela.com/", ResponseNamespace="http://service.webservice.accela.com/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute("return", Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public SearchResultModel getOwnerListByParcelPKs([System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)] string arg0, [System.Xml.Serialization.XmlElementAttribute("arg1", Form = System.Xml.Schema.XmlSchemaForm.Unqualified)] ParcelModel[] arg1, [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)] bool arg2, [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)] QueryFormat arg3)
        {
            object[] results = this.Invoke("getOwnerListByParcelPKs", new object[] {
                        arg0,
                        arg1,
                        arg2,
                        arg3});
            return ((SearchResultModel)(results[0]));
        }

        /// <remarks/>
        public void getOwnerListByParcelPKsAsync(string arg0, ParcelModel[] arg1, bool arg2, QueryFormat arg3)
        {
            this.getOwnerListByParcelPKsAsync(arg0, arg1, arg2, arg3, null);
        }

        /// <remarks/>
        public void getOwnerListByParcelPKsAsync(string arg0, ParcelModel[] arg1, bool arg2, QueryFormat arg3, object userState)
        {
            if ((this.getOwnerListByParcelPKsOperationCompleted == null))
            {
                this.getOwnerListByParcelPKsOperationCompleted = new System.Threading.SendOrPostCallback(this.OngetOwnerListByParcelPKsOperationCompleted);
            }
            this.InvokeAsync("getOwnerListByParcelPKs", new object[] {
                        arg0,
                        arg1,
                        arg2,
                        arg3}, this.getOwnerListByParcelPKsOperationCompleted, userState);
        }

        private void OngetOwnerListByParcelPKsOperationCompleted(object arg)
        {
            if ((this.getOwnerListByParcelPKsCompleted != null))
            {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.getOwnerListByParcelPKsCompleted(this, new getOwnerListByParcelPKsCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("", RequestNamespace="http://service.webservice.accela.com/", ResponseNamespace="http://service.webservice.accela.com/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute("return", Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public RefAddressModel[] getRefAddressListByParcelPK([System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)] string arg0, [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)] ParcelModel arg1, [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)] bool arg2)
        {
            object[] results = this.Invoke("getRefAddressListByParcelPK", new object[] {
                        arg0,
                        arg1,
                        arg2});
            return ((RefAddressModel[])(results[0]));
        }
        
        /// <remarks/>
        public void getRefAddressListByParcelPKAsync(string arg0, ParcelModel arg1, bool arg2)
        {
            this.getRefAddressListByParcelPKAsync(arg0, arg1, arg2, null);
        }
        
        /// <remarks/>
        public void getRefAddressListByParcelPKAsync(string arg0, ParcelModel arg1, bool arg2, object userState)
        {
            if ((this.getRefAddressListByParcelPKOperationCompleted == null))
            {
                this.getRefAddressListByParcelPKOperationCompleted = new System.Threading.SendOrPostCallback(this.OngetRefAddressListByParcelPKOperationCompleted);
            }
            this.InvokeAsync("getRefAddressListByParcelPK", new object[] {
                        arg0,
                        arg1,
                        arg2}, this.getRefAddressListByParcelPKOperationCompleted, userState);
        }
        
        private void OngetRefAddressListByParcelPKOperationCompleted(object arg)
        {
            if ((this.getRefAddressListByParcelPKCompleted != null))
            {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.getRefAddressListByParcelPKCompleted(this, new getRefAddressListByParcelPKCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
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
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.18408")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://service.webservice.accela.com/")]
    public partial class AAException {
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.18408")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://service.webservice.accela.com/")]
    public partial class getRefAddressListByParcelPK
    {
        
        private string arg0Field;
        
        private ParcelModel arg1Field;
		
		private bool arg2Field;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string arg0
        {
            get
            {
                return this.arg0Field;
            }
            set
            {
                this.arg0Field = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public ParcelModel arg1
        {
            get
            {
                return this.arg1Field;
            }
            set
            {
                this.arg1Field = value;
            }
        }
		
		/// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public bool arg2
        {
            get
            {
                return this.arg2Field;
            }
            set
            {
                this.arg2Field = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.18408")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://service.webservice.accela.com/")]
    public partial class getRefOwnerList {
        
        private string arg0Field;
        
        private OwnerCompModel arg1Field;

        private QueryFormat arg2Field;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string arg0 {
            get {
                return this.arg0Field;
            }
            set {
                this.arg0Field = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public OwnerCompModel arg1 {
            get {
                return this.arg1Field;
            }
            set {
                this.arg1Field = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public QueryFormat arg2
        {
            get {
                return this.arg2Field;
            }
            set {
                this.arg2Field = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.18408")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://service.webservice.accela.com/")]
    public partial class getOwnerListByParcelPKs
    {

        private string arg0Field;

        private ParcelModel[] arg1Field;

        private bool arg2Field;

        private QueryFormat arg3Field;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string arg0
        {
            get
            {
                return this.arg0Field;
            }
            set
            {
                this.arg0Field = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("arg1", Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public ParcelModel[] arg1
        {
            get
            {
                return this.arg1Field;
            }
            set
            {
                this.arg1Field = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public bool arg2
        {
            get
            {
                return this.arg2Field;
            }
            set
            {
                this.arg2Field = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public QueryFormat arg3
        {
            get
            {
                return this.arg3Field;
            }
            set
            {
                this.arg3Field = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.18408")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://service.webservice.accela.com/")]
    public partial class getRefParcelList {
        
        private string arg0Field;
        
        private ParcelModel arg1Field;

        private QueryFormat arg2Field;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string arg0 {
            get {
                return this.arg0Field;
            }
            set {
                this.arg0Field = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public ParcelModel arg1 {
            get {
                return this.arg1Field;
            }
            set {
                this.arg1Field = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public QueryFormat arg2
        {
            get {
                return this.arg2Field;
            }
            set {
                this.arg2Field = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.18408")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://service.webservice.accela.com/")]
    public partial class getRefAddressListByParcel
    {

        private string arg0Field;

        private ParcelModel arg1Field;

        private QueryFormat arg2Field;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string arg0
        {
            get
            {
                return this.arg0Field;
            }
            set
            {
                this.arg0Field = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public ParcelModel arg1
        {
            get
            {
                return this.arg1Field;
            }
            set
            {
                this.arg1Field = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public QueryFormat arg2
        {
            get
            {
                return this.arg2Field;
            }
            set
            {
                this.arg2Field = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.18408")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://service.webservice.accela.com/")]
    public partial class getRefAddressList {
        
        private string arg0Field;
        
        private RefAddressModel arg1Field;
        
        private QueryFormat arg2Field;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string arg0 {
            get {
                return this.arg0Field;
            }
            set {
                this.arg0Field = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public RefAddressModel arg1 {
            get {
                return this.arg1Field;
            }
            set {
                this.arg1Field = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public QueryFormat arg2
        {
            get {
                return this.arg2Field;
            }
            set {
                this.arg2Field = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.18408")]
    public delegate void getRefAddressListCompletedEventHandler(object sender, getRefAddressListCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.18408")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class getRefAddressListCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal getRefAddressListCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public RefAddressModel[] Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((RefAddressModel[])(this.results[0]));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.18408")]
    public delegate void getRefParcelListCompletedEventHandler(object sender, getRefParcelListCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.18408")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class getRefParcelListCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal getRefParcelListCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public ParcelModel[] Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((ParcelModel[])(this.results[0]));
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.18408")]
    public delegate void getRefAddressListByParcelCompletedEventHandler(object sender, getRefAddressListByParcelCompletedEventArgs e);

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.18408")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class getRefAddressListByParcelCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs
    {

        private object[] results;

        internal getRefAddressListByParcelCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) :
            base(exception, cancelled, userState)
        {
            this.results = results;
        }

        /// <remarks/>
        public SearchResultModel Result
        {
            get
            {
                this.RaiseExceptionIfNecessary();
                return ((SearchResultModel)(this.results[0]));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.18408")]
    public delegate void getRefOwnerListCompletedEventHandler(object sender, getRefOwnerListCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.18408")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class getRefOwnerListCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal getRefOwnerListCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public OwnerModel[] Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((OwnerModel[])(this.results[0]));
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.18408")]
    public delegate void getOwnerListByParcelPKsCompletedEventHandler(object sender, getOwnerListByParcelPKsCompletedEventArgs e);

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.18408")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class getOwnerListByParcelPKsCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs
    {

        private object[] results;

        internal getOwnerListByParcelPKsCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) :
            base(exception, cancelled, userState)
        {
            this.results = results;
        }

        /// <remarks/>
        public SearchResultModel Result
        {
            get
            {
                this.RaiseExceptionIfNecessary();
                return ((SearchResultModel)(this.results[0]));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.18408")]
    public delegate void getRefAddressListByParcelPKCompletedEventHandler(object sender, getRefAddressListByParcelPKCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.18408")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class getRefAddressListByParcelPKCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs
    {
        
        private object[] results;
        
        internal getRefAddressListByParcelPKCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public RefAddressModel[] Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((RefAddressModel[])(this.results[0]));
            }
        }
    }
}

#pragma warning restore 1591