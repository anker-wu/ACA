#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: Reference.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2009-2011
 *
 *  Description:
 *  
 *
 *  Notes:
 *  $Id: Reference.cs 130988 2009-9-8  9:25:01Z ACHIEVO\jackie.yu $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header
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
    [System.Web.Services.WebServiceBindingAttribute(Name="GlobalSearchWebServiceServiceSoapBinding", Namespace="http://service.webservice.accela.com/")]
    public partial class GlobalSearchWebServiceService : System.Web.Services.Protocols.SoapHttpClientProtocol {
        
        private System.Threading.SendOrPostCallback executeQueryOperationCompleted;
        
        private System.Threading.SendOrPostCallback getTotalCountOperationCompleted;
        
        private bool useDefaultCredentialsSetExplicitly;
        
        /// <remarks/>
        public GlobalSearchWebServiceService() {
            
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
        public event executeQueryCompletedEventHandler executeQueryCompleted;
        
        /// <remarks/>
        public event getTotalCountCompletedEventHandler getTotalCountCompleted;
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("", RequestNamespace="http://service.webservice.accela.com/", ResponseNamespace="http://service.webservice.accela.com/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute("return", Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public GlobalSearchResult4WS executeQuery([System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)] GlobalSearchParam4WS arg0, [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)] string arg1, [System.Xml.Serialization.XmlElementAttribute("arg2", Form=System.Xml.Schema.XmlSchemaForm.Unqualified)] string[] arg2) {
            object[] results = this.Invoke("executeQuery", new object[] {
                        arg0,
                        arg1,
                        arg2});
            return ((GlobalSearchResult4WS)(results[0]));
        }
        
        /// <remarks/>
        public void executeQueryAsync(GlobalSearchParam4WS arg0, string arg1, string[] arg2) {
            this.executeQueryAsync(arg0, arg1, arg2, null);
        }
        
        /// <remarks/>
        public void executeQueryAsync(GlobalSearchParam4WS arg0, string arg1, string[] arg2, object userState) {
            if ((this.executeQueryOperationCompleted == null)) {
                this.executeQueryOperationCompleted = new System.Threading.SendOrPostCallback(this.OnexecuteQueryOperationCompleted);
            }
            this.InvokeAsync("executeQuery", new object[] {
                        arg0,
                        arg1,
                        arg2}, this.executeQueryOperationCompleted, userState);
        }
        
        private void OnexecuteQueryOperationCompleted(object arg) {
            if ((this.executeQueryCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.executeQueryCompleted(this, new executeQueryCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("", RequestNamespace="http://service.webservice.accela.com/", ResponseNamespace="http://service.webservice.accela.com/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute("return", Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public int getTotalCount([System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)] GlobalSearchParam4WS arg0, [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)] string arg1) {
            object[] results = this.Invoke("getTotalCount", new object[] {
                        arg0,
                        arg1});
            return ((int)(results[0]));
        }
        
        /// <remarks/>
        public void getTotalCountAsync(GlobalSearchParam4WS arg0, string arg1) {
            this.getTotalCountAsync(arg0, arg1, null);
        }
        
        /// <remarks/>
        public void getTotalCountAsync(GlobalSearchParam4WS arg0, string arg1, object userState) {
            if ((this.getTotalCountOperationCompleted == null)) {
                this.getTotalCountOperationCompleted = new System.Threading.SendOrPostCallback(this.OngetTotalCountOperationCompleted);
            }
            this.InvokeAsync("getTotalCount", new object[] {
                        arg0,
                        arg1}, this.getTotalCountOperationCompleted, userState);
        }
        
        private void OngetTotalCountOperationCompleted(object arg) {
            if ((this.getTotalCountCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.getTotalCountCompleted(this, new getTotalCountCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
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
    public delegate void executeQueryCompletedEventHandler(object sender, executeQueryCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.1")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class executeQueryCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal executeQueryCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public GlobalSearchResult4WS Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((GlobalSearchResult4WS)(this.results[0]));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.1")]
    public delegate void getTotalCountCompletedEventHandler(object sender, getTotalCountCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.1")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class getTotalCountCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal getTotalCountCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public int Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((int)(this.results[0]));
            }
        }
    }
}

#pragma warning restore 1591