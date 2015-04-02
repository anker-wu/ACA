﻿/**
 * <pre>
 * 
 *  Accela Citizen Access
 *  File: CapModel4WS.cs
 * 
 *  Accela, Inc.
 *  Copyright (C): 2007-2014
 * 
 *  Description:
 * 
 *  Notes:
 * $Id: Reference.cs 269276 2014-04-09 09:38:35Z ACHIEVO\james.shi $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.235
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

// 
// This source code was auto-generated by Microsoft.VSDesigner, Version 4.0.30319.235.
// 
#pragma warning disable 1591

namespace Accela.ACA.WSProxy{
    using System;
    using System.Web.Services;
    using System.Diagnostics;
    using System.Web.Services.Protocols;
    using System.ComponentModel;
    using System.Xml.Serialization;


    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.18408")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Web.Services.WebServiceBindingAttribute(Name = "TemplateWebServiceServiceSoapBinding", Namespace = "http://service.webservice.accela.com/")]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(GenericTemplateFieldPK))]
    public partial class TemplateWebServiceService : System.Web.Services.Protocols.SoapHttpClientProtocol
    {

        private System.Threading.SendOrPostCallback getGenericTemplateStructureByEntityPKModelOperationCompleted;

        private System.Threading.SendOrPostCallback getEditAttributesOperationCompleted;

        private System.Threading.SendOrPostCallback getTemplateAssociateASIGroupOperationCompleted;

        private System.Threading.SendOrPostCallback getDocTemplateOperationCompleted;

        private System.Threading.SendOrPostCallback getLPAttributes4SupportEMSEOperationCompleted;

        private System.Threading.SendOrPostCallback getDailyGenericTemplateOperationCompleted;

        private System.Threading.SendOrPostCallback getContactTemplateOperationCompleted;

        private System.Threading.SendOrPostCallback getAttributesOperationCompleted;

        private bool useDefaultCredentialsSetExplicitly;

        /// <remarks/>
        public TemplateWebServiceService()
        {
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

        public new string Url
        {
            get
            {
                return base.Url;
            }
            set
            {
                if ((((this.IsLocalFileSystemWebService(base.Url) == true)
                            && (this.useDefaultCredentialsSetExplicitly == false))
                            && (this.IsLocalFileSystemWebService(value) == false)))
                {
                    base.UseDefaultCredentials = false;
                }
                base.Url = value;
            }
        }

        public new bool UseDefaultCredentials
        {
            get
            {
                return base.UseDefaultCredentials;
            }
            set
            {
                base.UseDefaultCredentials = value;
                this.useDefaultCredentialsSetExplicitly = true;
            }
        }

        /// <remarks/>
        public event getGenericTemplateStructureByEntityPKModelCompletedEventHandler getGenericTemplateStructureByEntityPKModelCompleted;

        /// <remarks/>
        public event getEditAttributesCompletedEventHandler getEditAttributesCompleted;

        /// <remarks/>
        public event getTemplateAssociateASIGroupCompletedEventHandler getTemplateAssociateASIGroupCompleted;

        /// <remarks/>
        public event getDocTemplateCompletedEventHandler getDocTemplateCompleted;

        /// <remarks/>
        public event getLPAttributes4SupportEMSECompletedEventHandler getLPAttributes4SupportEMSECompleted;

        /// <remarks/>
        public event getDailyGenericTemplateCompletedEventHandler getDailyGenericTemplateCompleted;

        /// <remarks/>
        public event getContactTemplateCompletedEventHandler getContactTemplateCompleted;

        /// <remarks/>
        public event getAttributesCompletedEventHandler getAttributesCompleted;

        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("", RequestNamespace = "http://service.webservice.accela.com/", ResponseNamespace = "http://service.webservice.accela.com/", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute("return", Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public TemplateModel getGenericTemplateStructureByEntityPKModel([System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)] EntityPKModel arg0, [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)] bool arg1, [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)] string arg2)
        {
            object[] results = this.Invoke("getGenericTemplateStructureByEntityPKModel", new object[] {
                        arg0,
                        arg1,
                        arg2});
            return ((TemplateModel)(results[0]));
        }

        /// <remarks/>
        public void getGenericTemplateStructureByEntityPKModelAsync(EntityPKModel arg0, bool arg1, string arg2)
        {
            this.getGenericTemplateStructureByEntityPKModelAsync(arg0, arg1, arg2, null);
        }

        /// <remarks/>
        public void getGenericTemplateStructureByEntityPKModelAsync(EntityPKModel arg0, bool arg1, string arg2, object userState)
        {
            if ((this.getGenericTemplateStructureByEntityPKModelOperationCompleted == null))
            {
                this.getGenericTemplateStructureByEntityPKModelOperationCompleted = new System.Threading.SendOrPostCallback(this.OngetGenericTemplateStructureByEntityPKModelOperationCompleted);
            }
            this.InvokeAsync("getGenericTemplateStructureByEntityPKModel", new object[] {
                        arg0,
                        arg1,
                        arg2}, this.getGenericTemplateStructureByEntityPKModelOperationCompleted, userState);
        }

        private void OngetGenericTemplateStructureByEntityPKModelOperationCompleted(object arg)
        {
            if ((this.getGenericTemplateStructureByEntityPKModelCompleted != null))
            {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.getGenericTemplateStructureByEntityPKModelCompleted(this, new getGenericTemplateStructureByEntityPKModelCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }

        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("", RequestNamespace = "http://service.webservice.accela.com/", ResponseNamespace = "http://service.webservice.accela.com/", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute("return", Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public TemplateAttributeModel[] getEditAttributes([System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)] string arg0, [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)] string arg1, [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)] string arg2, [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)] CapIDModel4WS arg3, [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)] string arg4, [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)] string arg5)
        {
            object[] results = this.Invoke("getEditAttributes", new object[] {
                        arg0,
                        arg1,
                        arg2,
                        arg3,
                        arg4,
                        arg5});
            return ((TemplateAttributeModel[])(results[0]));
        }

        /// <remarks/>
        public void getEditAttributesAsync(string arg0, string arg1, string arg2, CapIDModel4WS arg3, string arg4, string arg5)
        {
            this.getEditAttributesAsync(arg0, arg1, arg2, arg3, arg4, arg5, null);
        }

        /// <remarks/>
        public void getEditAttributesAsync(string arg0, string arg1, string arg2, CapIDModel4WS arg3, string arg4, string arg5, object userState)
        {
            if ((this.getEditAttributesOperationCompleted == null))
            {
                this.getEditAttributesOperationCompleted = new System.Threading.SendOrPostCallback(this.OngetEditAttributesOperationCompleted);
            }
            this.InvokeAsync("getEditAttributes", new object[] {
                        arg0,
                        arg1,
                        arg2,
                        arg3,
                        arg4,
                        arg5}, this.getEditAttributesOperationCompleted, userState);
        }

        private void OngetEditAttributesOperationCompleted(object arg)
        {
            if ((this.getEditAttributesCompleted != null))
            {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.getEditAttributesCompleted(this, new getEditAttributesCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }

        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("", RequestNamespace = "http://service.webservice.accela.com/", ResponseNamespace = "http://service.webservice.accela.com/", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute("return", Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string getTemplateAssociateASIGroup([System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)] string arg0, [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)] int arg1, [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)] [System.Xml.Serialization.XmlIgnoreAttribute()] bool arg1Specified, [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)] string arg2)
        {
            object[] results = this.Invoke("getTemplateAssociateASIGroup", new object[] {
                        arg0,
                        arg1,
                        arg1Specified,
                        arg2});
            return ((string)(results[0]));
        }

        /// <remarks/>
        public void getTemplateAssociateASIGroupAsync(string arg0, int arg1, bool arg1Specified, string arg2)
        {
            this.getTemplateAssociateASIGroupAsync(arg0, arg1, arg1Specified, arg2, null);
        }

        /// <remarks/>
        public void getTemplateAssociateASIGroupAsync(string arg0, int arg1, bool arg1Specified, string arg2, object userState)
        {
            if ((this.getTemplateAssociateASIGroupOperationCompleted == null))
            {
                this.getTemplateAssociateASIGroupOperationCompleted = new System.Threading.SendOrPostCallback(this.OngetTemplateAssociateASIGroupOperationCompleted);
            }
            this.InvokeAsync("getTemplateAssociateASIGroup", new object[] {
                        arg0,
                        arg1,
                        arg1Specified,
                        arg2}, this.getTemplateAssociateASIGroupOperationCompleted, userState);
        }

        private void OngetTemplateAssociateASIGroupOperationCompleted(object arg)
        {
            if ((this.getTemplateAssociateASIGroupCompleted != null))
            {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.getTemplateAssociateASIGroupCompleted(this, new getTemplateAssociateASIGroupCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }

        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("", RequestNamespace = "http://service.webservice.accela.com/", ResponseNamespace = "http://service.webservice.accela.com/", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute("return", Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public TemplateModel getDocTemplate([System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)] string arg0, [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)] string arg1, [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)] string arg2)
        {
            object[] results = this.Invoke("getDocTemplate", new object[] {
                        arg0,
                        arg1,
                        arg2});
            return ((TemplateModel)(results[0]));
        }

        /// <remarks/>
        public void getDocTemplateAsync(string arg0, string arg1, string arg2)
        {
            this.getDocTemplateAsync(arg0, arg1, arg2, null);
        }

        /// <remarks/>
        public void getDocTemplateAsync(string arg0, string arg1, string arg2, object userState)
        {
            if ((this.getDocTemplateOperationCompleted == null))
            {
                this.getDocTemplateOperationCompleted = new System.Threading.SendOrPostCallback(this.OngetDocTemplateOperationCompleted);
            }
            this.InvokeAsync("getDocTemplate", new object[] {
                        arg0,
                        arg1,
                        arg2}, this.getDocTemplateOperationCompleted, userState);
        }

        private void OngetDocTemplateOperationCompleted(object arg)
        {
            if ((this.getDocTemplateCompleted != null))
            {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.getDocTemplateCompleted(this, new getDocTemplateCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }

        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("", RequestNamespace = "http://service.webservice.accela.com/", ResponseNamespace = "http://service.webservice.accela.com/", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute("return", Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public TemplateAttributeModel[] getLPAttributes4SupportEMSE([System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)] string arg0, [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)] string arg1, [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)] string arg2, [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)] string arg3, [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)] string arg4)
        {
            object[] results = this.Invoke("getLPAttributes4SupportEMSE", new object[] {
                        arg0,
                        arg1,
                        arg2,
                        arg3,
                        arg4});
            return ((TemplateAttributeModel[])(results[0]));
        }

        /// <remarks/>
        public void getLPAttributes4SupportEMSEAsync(string arg0, string arg1, string arg2, string arg3, string arg4)
        {
            this.getLPAttributes4SupportEMSEAsync(arg0, arg1, arg2, arg3, arg4, null);
        }

        /// <remarks/>
        public void getLPAttributes4SupportEMSEAsync(string arg0, string arg1, string arg2, string arg3, string arg4, object userState)
        {
            if ((this.getLPAttributes4SupportEMSEOperationCompleted == null))
            {
                this.getLPAttributes4SupportEMSEOperationCompleted = new System.Threading.SendOrPostCallback(this.OngetLPAttributes4SupportEMSEOperationCompleted);
            }
            this.InvokeAsync("getLPAttributes4SupportEMSE", new object[] {
                        arg0,
                        arg1,
                        arg2,
                        arg3,
                        arg4}, this.getLPAttributes4SupportEMSEOperationCompleted, userState);
        }

        private void OngetLPAttributes4SupportEMSEOperationCompleted(object arg)
        {
            if ((this.getLPAttributes4SupportEMSECompleted != null))
            {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.getLPAttributes4SupportEMSECompleted(this, new getLPAttributes4SupportEMSECompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }

        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("", RequestNamespace = "http://service.webservice.accela.com/", ResponseNamespace = "http://service.webservice.accela.com/", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute("return", Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public TemplateModel getDailyGenericTemplate([System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)] EntityPKModel arg0)
        {
            object[] results = this.Invoke("getDailyGenericTemplate", new object[] {
                        arg0});
            return ((TemplateModel)(results[0]));
        }

        /// <remarks/>
        public void getDailyGenericTemplateAsync(EntityPKModel arg0)
        {
            this.getDailyGenericTemplateAsync(arg0, null);
        }

        /// <remarks/>
        public void getDailyGenericTemplateAsync(EntityPKModel arg0, object userState)
        {
            if ((this.getDailyGenericTemplateOperationCompleted == null))
            {
                this.getDailyGenericTemplateOperationCompleted = new System.Threading.SendOrPostCallback(this.OngetDailyGenericTemplateOperationCompleted);
            }
            this.InvokeAsync("getDailyGenericTemplate", new object[] {
                        arg0}, this.getDailyGenericTemplateOperationCompleted, userState);
        }

        private void OngetDailyGenericTemplateOperationCompleted(object arg)
        {
            if ((this.getDailyGenericTemplateCompleted != null))
            {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.getDailyGenericTemplateCompleted(this, new getDailyGenericTemplateCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }

        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("", RequestNamespace = "http://service.webservice.accela.com/", ResponseNamespace = "http://service.webservice.accela.com/", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute("return", Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public TemplateModel getContactTemplate([System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)] string arg0, [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)] string arg1, [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)] bool arg2, [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)] string arg3)
        {
            object[] results = this.Invoke("getContactTemplate", new object[] {
                        arg0,
                        arg1,
                        arg2,
                        arg3});
            return ((TemplateModel)(results[0]));
        }

        /// <remarks/>
        public void getContactTemplateAsync(string arg0, string arg1, bool arg2, string arg3)
        {
            this.getContactTemplateAsync(arg0, arg1, arg2, arg3, null);
        }

        /// <remarks/>
        public void getContactTemplateAsync(string arg0, string arg1, bool arg2, string arg3, object userState)
        {
            if ((this.getContactTemplateOperationCompleted == null))
            {
                this.getContactTemplateOperationCompleted = new System.Threading.SendOrPostCallback(this.OngetContactTemplateOperationCompleted);
            }
            this.InvokeAsync("getContactTemplate", new object[] {
                        arg0,
                        arg1,
                        arg2,
                        arg3}, this.getContactTemplateOperationCompleted, userState);
        }

        private void OngetContactTemplateOperationCompleted(object arg)
        {
            if ((this.getContactTemplateCompleted != null))
            {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.getContactTemplateCompleted(this, new getContactTemplateCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }

        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("", RequestNamespace = "http://service.webservice.accela.com/", ResponseNamespace = "http://service.webservice.accela.com/", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute("return", Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public TemplateAttributeModel[] getAttributes([System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)] string arg0, [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)] string arg1, [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)] string arg2, [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)] string arg3, [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)] string arg4)
        {
            object[] results = this.Invoke("getAttributes", new object[] {
                        arg0,
                        arg1,
                        arg2,
                        arg3,
                        arg4});
            return ((TemplateAttributeModel[])(results[0]));
        }

        /// <remarks/>
        public void getAttributesAsync(string arg0, string arg1, string arg2, string arg3, string arg4)
        {
            this.getAttributesAsync(arg0, arg1, arg2, arg3, arg4, null);
        }

        /// <remarks/>
        public void getAttributesAsync(string arg0, string arg1, string arg2, string arg3, string arg4, object userState)
        {
            if ((this.getAttributesOperationCompleted == null))
            {
                this.getAttributesOperationCompleted = new System.Threading.SendOrPostCallback(this.OngetAttributesOperationCompleted);
            }
            this.InvokeAsync("getAttributes", new object[] {
                        arg0,
                        arg1,
                        arg2,
                        arg3,
                        arg4}, this.getAttributesOperationCompleted, userState);
        }

        private void OngetAttributesOperationCompleted(object arg)
        {
            if ((this.getAttributesCompleted != null))
            {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.getAttributesCompleted(this, new getAttributesCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }

        /// <remarks/>
        public new void CancelAsync(object userState)
        {
            base.CancelAsync(userState);
        }

        private bool IsLocalFileSystemWebService(string url)
        {
            if (((url == null)
                        || (url == string.Empty)))
            {
                return false;
            }
            System.Uri wsUri = new System.Uri(url);
            if (((wsUri.Port >= 1024)
                        && (string.Compare(wsUri.Host, "localHost", System.StringComparison.OrdinalIgnoreCase) == 0)))
            {
                return true;
            }
            return false;
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.1")]
    public delegate void getEditAttributesCompletedEventHandler(object sender, getEditAttributesCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.1")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class getEditAttributesCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal getEditAttributesCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public TemplateAttributeModel[] Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((TemplateAttributeModel[])(this.results[0]));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.1")]
    public delegate void getDocTemplateCompletedEventHandler(object sender, getDocTemplateCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.1")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class getDocTemplateCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal getDocTemplateCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public TemplateModel Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((TemplateModel)(this.results[0]));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.1")]
    public delegate void getLPAttributes4SupportEMSECompletedEventHandler(object sender, getLPAttributes4SupportEMSECompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.1")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class getLPAttributes4SupportEMSECompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal getLPAttributes4SupportEMSECompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public TemplateAttributeModel[] Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((TemplateAttributeModel[])(this.results[0]));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.1")]
    public delegate void getDailyGenericTemplateCompletedEventHandler(object sender, getDailyGenericTemplateCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.1")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class getDailyGenericTemplateCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal getDailyGenericTemplateCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public TemplateModel Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((TemplateModel)(this.results[0]));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.1")]
    public delegate void getContactTemplateCompletedEventHandler(object sender, getContactTemplateCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.1")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class getContactTemplateCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal getContactTemplateCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public TemplateModel Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((TemplateModel)(this.results[0]));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.1")]
    public delegate void getAttributesCompletedEventHandler(object sender, getAttributesCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.1")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class getAttributesCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal getAttributesCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public TemplateAttributeModel[] Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((TemplateAttributeModel[])(this.results[0]));
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.17929")]
    public delegate void getGenericTemplateStructureByEntityPKModelCompletedEventHandler(object sender, getGenericTemplateStructureByEntityPKModelCompletedEventArgs e);

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.17929")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class getGenericTemplateStructureByEntityPKModelCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs
    {

        private object[] results;

        internal getGenericTemplateStructureByEntityPKModelCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) :
            base(exception, cancelled, userState)
        {
            this.results = results;
        }

        /// <remarks/>
        public TemplateModel Result
        {
            get
            {
                this.RaiseExceptionIfNecessary();
                return ((TemplateModel)(this.results[0]));
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.18408")]
    public delegate void getTemplateAssociateASIGroupCompletedEventHandler(object sender, getTemplateAssociateASIGroupCompletedEventArgs e);

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.18408")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class getTemplateAssociateASIGroupCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs
    {

        private object[] results;

        internal getTemplateAssociateASIGroupCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) :
            base(exception, cancelled, userState)
        {
            this.results = results;
        }

        /// <remarks/>
        public string Result
        {
            get
            {
                this.RaiseExceptionIfNecessary();
                return ((string)(this.results[0]));
            }
        }
    }
}

#pragma warning restore 1591