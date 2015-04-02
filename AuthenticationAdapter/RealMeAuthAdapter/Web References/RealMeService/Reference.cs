/**
 * <pre>
 * 
 *  Accela Citizen Access
 *  File: RealMe.cs
 * 
 *  Accela, Inc.
 *  Copyright (C): 2014
 * 
 *  Description:
 * 
 *  Notes:
 * $Id: RealMe.cs 179034 2014-09-03 09:17:55Z ACHIEVO\foxus.lin $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

namespace Accela.ACA.RealMeAccessGate.RealMeService {
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
    [System.Web.Services.WebServiceBindingAttribute(Name="RealMeSoap", Namespace="http://tempuri.org/")]
    public partial class RealMe : System.Web.Services.Protocols.SoapHttpClientProtocol {
        
        private System.Threading.SendOrPostCallback getLoginURLOperationCompleted;
        
        private System.Threading.SendOrPostCallback validateLoginOperationCompleted;
        
        private bool useDefaultCredentialsSetExplicitly;
        
        /// <remarks/>
        public RealMe() {
            this.Url = global::Accela.ACA.RealMeAccessGate.Properties.Settings.Default.Accela_ACA_RealMeAccessGate_RealMeService_RealMe;
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
        public event getLoginURLCompletedEventHandler getLoginURLCompleted;
        
        /// <remarks/>
        public event validateLoginCompletedEventHandler validateLoginCompleted;
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://tempuri.org/getLoginURL", RequestNamespace="http://tempuri.org/", ResponseNamespace="http://tempuri.org/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public string getLoginURL(string state, string returnURL) {
            object[] results = this.Invoke("getLoginURL", new object[] {
                        state,
                        returnURL});
            return ((string)(results[0]));
        }
        
        /// <remarks/>
        public void getLoginURLAsync(string state, string returnURL) {
            this.getLoginURLAsync(state, returnURL, null);
        }
        
        /// <remarks/>
        public void getLoginURLAsync(string state, string returnURL, object userState) {
            if ((this.getLoginURLOperationCompleted == null)) {
                this.getLoginURLOperationCompleted = new System.Threading.SendOrPostCallback(this.OngetLoginURLOperationCompleted);
            }
            this.InvokeAsync("getLoginURL", new object[] {
                        state,
                        returnURL}, this.getLoginURLOperationCompleted, userState);
        }
        
        private void OngetLoginURLOperationCompleted(object arg) {
            if ((this.getLoginURLCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.getLoginURLCompleted(this, new getLoginURLCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://tempuri.org/validateLogin", RequestNamespace="http://tempuri.org/", ResponseNamespace="http://tempuri.org/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public string validateLogin(string ipAddress, string userAgent, string SAMLart, string RelayState, string SigAlg, string Signature, out string statusCode, out string username, out string resultMessage, out string securityToken, out string isAssociatedWithUser) {
            object[] results = this.Invoke("validateLogin", new object[] {
                        ipAddress,
                        userAgent,
                        SAMLart,
                        RelayState,
                        SigAlg,
                        Signature});
            statusCode = ((string)(results[1]));
            username = ((string)(results[2]));
            resultMessage = ((string)(results[3]));
            securityToken = ((string)(results[4]));
            isAssociatedWithUser = ((string)(results[5]));
            return ((string)(results[0]));
        }
        
        /// <remarks/>
        public void validateLoginAsync(string ipAddress, string userAgent, string SAMLart, string RelayState, string SigAlg, string Signature) {
            this.validateLoginAsync(ipAddress, userAgent, SAMLart, RelayState, SigAlg, Signature, null);
        }
        
        /// <remarks/>
        public void validateLoginAsync(string ipAddress, string userAgent, string SAMLart, string RelayState, string SigAlg, string Signature, object userState) {
            if ((this.validateLoginOperationCompleted == null)) {
                this.validateLoginOperationCompleted = new System.Threading.SendOrPostCallback(this.OnvalidateLoginOperationCompleted);
            }
            this.InvokeAsync("validateLogin", new object[] {
                        ipAddress,
                        userAgent,
                        SAMLart,
                        RelayState,
                        SigAlg,
                        Signature}, this.validateLoginOperationCompleted, userState);
        }
        
        private void OnvalidateLoginOperationCompleted(object arg) {
            if ((this.validateLoginCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.validateLoginCompleted(this, new validateLoginCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
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
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.18408")]
    public delegate void getLoginURLCompletedEventHandler(object sender, getLoginURLCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.18408")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class getLoginURLCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal getLoginURLCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public string Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((string)(this.results[0]));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.18408")]
    public delegate void validateLoginCompletedEventHandler(object sender, validateLoginCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.18408")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class validateLoginCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal validateLoginCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public string Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((string)(this.results[0]));
            }
        }
        
        /// <remarks/>
        public string statusCode {
            get {
                this.RaiseExceptionIfNecessary();
                return ((string)(this.results[1]));
            }
        }
        
        /// <remarks/>
        public string username {
            get {
                this.RaiseExceptionIfNecessary();
                return ((string)(this.results[2]));
            }
        }
        
        /// <remarks/>
        public string resultMessage {
            get {
                this.RaiseExceptionIfNecessary();
                return ((string)(this.results[3]));
            }
        }
        
        /// <remarks/>
        public string securityToken {
            get {
                this.RaiseExceptionIfNecessary();
                return ((string)(this.results[4]));
            }
        }
        
        /// <remarks/>
        public string isAssociatedWithUser {
            get {
                this.RaiseExceptionIfNecessary();
                return ((string)(this.results[5]));
            }
        }
    }
}

#pragma warning restore 1591