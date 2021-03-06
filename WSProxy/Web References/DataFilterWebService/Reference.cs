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

using Accela.ACA.WSProxy;

#pragma warning disable 1591

namespace ACAWebService.DataFilterWebService
{
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
    [System.Web.Services.WebServiceBindingAttribute(Name = "DataFilterWebServiceServiceSoapBinding", Namespace = "http://service.webservice.accela.com/")]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(LanguageModel))]
    public partial class DataFilterWebServiceService : System.Web.Services.Protocols.SoapHttpClientProtocol
    {

        private System.Threading.SendOrPostCallback getXDataFilterByViewIdOperationCompleted;

        private System.Threading.SendOrPostCallback getXDataFilterElementByDataFilterIdOperationCompleted;

        private bool useDefaultCredentialsSetExplicitly;

        /// <remarks/>
        public DataFilterWebServiceService()
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
        public event getXDataFilterByViewIdCompletedEventHandler getXDataFilterByViewIdCompleted;

        /// <remarks/>
        public event getXDataFilterElementByDataFilterIdCompletedEventHandler getXDataFilterElementByDataFilterIdCompleted;

        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("", RequestNamespace = "http://service.webservice.accela.com/", ResponseNamespace = "http://service.webservice.accela.com/", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute("return", Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public XDataFilterModel[] getXDataFilterByViewId([System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)] string arg0, [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)] long arg1, [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)] string arg2, [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)] string arg3, [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)] string arg4)
        {
            object[] results = this.Invoke("getXDataFilterByViewId", new object[] {
                        arg0,
                        arg1,
                        arg2,
                        arg3,
                        arg4});
            return ((XDataFilterModel[])(results[0]));
        }

        /// <remarks/>
        public void getXDataFilterByViewIdAsync(string arg0, long arg1, string arg2, string arg3, string arg4)
        {
            this.getXDataFilterByViewIdAsync(arg0, arg1, arg2, arg3, arg4, null);
        }

        /// <remarks/>
        public void getXDataFilterByViewIdAsync(string arg0, long arg1, string arg2, string arg3, string arg4, object userState)
        {
            if ((this.getXDataFilterByViewIdOperationCompleted == null))
            {
                this.getXDataFilterByViewIdOperationCompleted = new System.Threading.SendOrPostCallback(this.OngetXDataFilterByViewIdOperationCompleted);
            }
            this.InvokeAsync("getXDataFilterByViewId", new object[] {
                        arg0,
                        arg1,
                        arg2,
                        arg3,
                        arg4}, this.getXDataFilterByViewIdOperationCompleted, userState);
        }

        private void OngetXDataFilterByViewIdOperationCompleted(object arg)
        {
            if ((this.getXDataFilterByViewIdCompleted != null))
            {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.getXDataFilterByViewIdCompleted(this, new getXDataFilterByViewIdCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }

        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("", RequestNamespace = "http://service.webservice.accela.com/", ResponseNamespace = "http://service.webservice.accela.com/", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute("return", Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public XDataFilterElementModel[] getXDataFilterElementByDataFilterId([System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)] string arg0, [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)] long arg1, [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)] long arg2, [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)] string arg3)
        {
            object[] results = this.Invoke("getXDataFilterElementByDataFilterId", new object[] {
                        arg0,
                        arg1,
                        arg2,
                        arg3});
            return ((XDataFilterElementModel[])(results[0]));
        }

        /// <remarks/>
        public void getXDataFilterElementByDataFilterIdAsync(string arg0, long arg1, long arg2, string arg3)
        {
            this.getXDataFilterElementByDataFilterIdAsync(arg0, arg1, arg2, arg3, null);
        }

        /// <remarks/>
        public void getXDataFilterElementByDataFilterIdAsync(string arg0, long arg1, long arg2, string arg3, object userState)
        {
            if ((this.getXDataFilterElementByDataFilterIdOperationCompleted == null))
            {
                this.getXDataFilterElementByDataFilterIdOperationCompleted = new System.Threading.SendOrPostCallback(this.OngetXDataFilterElementByDataFilterIdOperationCompleted);
            }
            this.InvokeAsync("getXDataFilterElementByDataFilterId", new object[] {
                        arg0,
                        arg1,
                        arg2,
                        arg3}, this.getXDataFilterElementByDataFilterIdOperationCompleted, userState);
        }

        private void OngetXDataFilterElementByDataFilterIdOperationCompleted(object arg)
        {
            if ((this.getXDataFilterElementByDataFilterIdCompleted != null))
            {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.getXDataFilterElementByDataFilterIdCompleted(this, new getXDataFilterElementByDataFilterIdCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
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
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.18408")]
    public delegate void getXDataFilterByViewIdCompletedEventHandler(object sender, getXDataFilterByViewIdCompletedEventArgs e);

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.18408")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class getXDataFilterByViewIdCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs
    {

        private object[] results;

        internal getXDataFilterByViewIdCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) :
            base(exception, cancelled, userState)
        {
            this.results = results;
        }

        /// <remarks/>
        public XDataFilterModel[] Result
        {
            get
            {
                this.RaiseExceptionIfNecessary();
                return ((XDataFilterModel[])(this.results[0]));
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.18408")]
    public delegate void getXDataFilterElementByDataFilterIdCompletedEventHandler(object sender, getXDataFilterElementByDataFilterIdCompletedEventArgs e);

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.18408")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class getXDataFilterElementByDataFilterIdCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs
    {

        private object[] results;

        internal getXDataFilterElementByDataFilterIdCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) :
            base(exception, cancelled, userState)
        {
            this.results = results;
        }

        /// <remarks/>
        public XDataFilterElementModel[] Result
        {
            get
            {
                this.RaiseExceptionIfNecessary();
                return ((XDataFilterElementModel[])(this.results[0]));
            }
        }
    }
}

#pragma warning restore 1591