/**
 * <pre>
 * 
 *  Accela Citizen Access
 *  File: MyCollectionWebService.cs
 * 
 *  Accela, Inc.
 *  Copyright (C): 2009-2011
 * 
 *  Description:
 * 
 *  Notes:
 * $Id: Reference.cs 193354 2011-03-21 10:17:54Z ACHIEVO\daly.zeng $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */
#pragma warning disable 1591

namespace Accela.ACA.WSProxy {
    using System.Diagnostics;
    using System.Web.Services;
    using System.ComponentModel;
    using System.Web.Services.Protocols;
    using System;
    using System.Xml.Serialization;
    
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "2.0.50727.1433")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Web.Services.WebServiceBindingAttribute(Name="MyCollectionWebServiceServiceSoapBinding", Namespace="http://service.webservice.accela.com/")]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(deleteCollectionResponse))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(deleteCollection))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(createCollectionResponse))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(createCollection))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(moveCaps2CollectionResponse))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(moveCaps2Collection))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(getCollections))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(updateCollectionResponse))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(updateCollection))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(addCaps2CollectionResponse))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(addCaps2Collection))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(copyCaps2CollectionResponse))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(copyCaps2Collection))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(getDetailInfoByCollectionIdResponse))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(getDetailInfoByCollectionId))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(removeCapsFromCollectionResponse))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(removeCapsFromCollection))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(getCollections4Management))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(AddressBaseModel))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(LanguageModel))]
    public partial class MyCollectionWebServiceService : System.Web.Services.Protocols.SoapHttpClientProtocol {
        
        private System.Threading.SendOrPostCallback getCollections4ManagementOperationCompleted;
        
        private System.Threading.SendOrPostCallback removeCapsFromCollectionOperationCompleted;
        
        private System.Threading.SendOrPostCallback getDetailInfoByCollectionIdOperationCompleted;
        
        private System.Threading.SendOrPostCallback copyCaps2CollectionOperationCompleted;
        
        private System.Threading.SendOrPostCallback addCaps2CollectionOperationCompleted;
        
        private System.Threading.SendOrPostCallback updateCollectionOperationCompleted;
        
        private System.Threading.SendOrPostCallback getCollectionsOperationCompleted;
        
        private System.Threading.SendOrPostCallback moveCaps2CollectionOperationCompleted;
        
        private System.Threading.SendOrPostCallback createCollectionOperationCompleted;
        
        private System.Threading.SendOrPostCallback deleteCollectionOperationCompleted;
        
        private bool useDefaultCredentialsSetExplicitly;
        
        /// <remarks/>
        public MyCollectionWebServiceService() {
            
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
        public event getCollections4ManagementCompletedEventHandler getCollections4ManagementCompleted;
        
        /// <remarks/>
        public event removeCapsFromCollectionCompletedEventHandler removeCapsFromCollectionCompleted;
        
        /// <remarks/>
        public event getDetailInfoByCollectionIdCompletedEventHandler getDetailInfoByCollectionIdCompleted;
        
        /// <remarks/>
        public event copyCaps2CollectionCompletedEventHandler copyCaps2CollectionCompleted;
        
        /// <remarks/>
        public event addCaps2CollectionCompletedEventHandler addCaps2CollectionCompleted;
        
        /// <remarks/>
        public event updateCollectionCompletedEventHandler updateCollectionCompleted;
        
        /// <remarks/>
        public event getCollectionsCompletedEventHandler getCollectionsCompleted;
        
        /// <remarks/>
        public event moveCaps2CollectionCompletedEventHandler moveCaps2CollectionCompleted;
        
        /// <remarks/>
        public event createCollectionCompletedEventHandler createCollectionCompleted;
        
        /// <remarks/>
        public event deleteCollectionCompletedEventHandler deleteCollectionCompleted;
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("", RequestNamespace="http://service.webservice.accela.com/", ResponseNamespace="http://service.webservice.accela.com/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute("return", Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public MyCollectionModel[] getCollections4Management([System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)] string arg0, [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)] string arg1) {
            object[] results = this.Invoke("getCollections4Management", new object[] {
                        arg0,
                        arg1});
            return ((MyCollectionModel[])(results[0]));
        }
        
        /// <remarks/>
        public void getCollections4ManagementAsync(string arg0, string arg1) {
            this.getCollections4ManagementAsync(arg0, arg1, null);
        }
        
        /// <remarks/>
        public void getCollections4ManagementAsync(string arg0, string arg1, object userState) {
            if ((this.getCollections4ManagementOperationCompleted == null)) {
                this.getCollections4ManagementOperationCompleted = new System.Threading.SendOrPostCallback(this.OngetCollections4ManagementOperationCompleted);
            }
            this.InvokeAsync("getCollections4Management", new object[] {
                        arg0,
                        arg1}, this.getCollections4ManagementOperationCompleted, userState);
        }
        
        private void OngetCollections4ManagementOperationCompleted(object arg) {
            if ((this.getCollections4ManagementCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.getCollections4ManagementCompleted(this, new getCollections4ManagementCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("", RequestNamespace="http://service.webservice.accela.com/", ResponseNamespace="http://service.webservice.accela.com/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public void removeCapsFromCollection([System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)] MyCollectionModel arg0) {
            this.Invoke("removeCapsFromCollection", new object[] {
                        arg0});
        }
        
        /// <remarks/>
        public void removeCapsFromCollectionAsync(MyCollectionModel arg0) {
            this.removeCapsFromCollectionAsync(arg0, null);
        }
        
        /// <remarks/>
        public void removeCapsFromCollectionAsync(MyCollectionModel arg0, object userState) {
            if ((this.removeCapsFromCollectionOperationCompleted == null)) {
                this.removeCapsFromCollectionOperationCompleted = new System.Threading.SendOrPostCallback(this.OnremoveCapsFromCollectionOperationCompleted);
            }
            this.InvokeAsync("removeCapsFromCollection", new object[] {
                        arg0}, this.removeCapsFromCollectionOperationCompleted, userState);
        }
        
        private void OnremoveCapsFromCollectionOperationCompleted(object arg) {
            if ((this.removeCapsFromCollectionCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.removeCapsFromCollectionCompleted(this, new System.ComponentModel.AsyncCompletedEventArgs(invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }

        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("", RequestNamespace = "http://service.webservice.accela.com/", ResponseNamespace = "http://service.webservice.accela.com/", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute("return", Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public MyCollectionModel getDetailInfoByCollectionId([System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)] string arg0, [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)] string arg1, [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)] string arg2, [System.Xml.Serialization.XmlElementAttribute("arg3", Form = System.Xml.Schema.XmlSchemaForm.Unqualified)] string[] arg3) {
            object[] results = this.Invoke("getDetailInfoByCollectionId", new object[] {
                        arg0,
                        arg1,
                        arg2,
                        arg3});
            return ((MyCollectionModel)(results[0]));
        }

        /// <remarks/>
        public void getDetailInfoByCollectionIdAsync(string arg0, string arg1, string arg2, string[] arg3) {
            this.getDetailInfoByCollectionIdAsync(arg0, arg1, arg2, arg3, null);
        }

        /// <remarks/>
        public void getDetailInfoByCollectionIdAsync(string arg0, string arg1, string arg2, string[] arg3, object userState) {
            if ((this.getDetailInfoByCollectionIdOperationCompleted == null)) {
                this.getDetailInfoByCollectionIdOperationCompleted = new System.Threading.SendOrPostCallback(this.OngetDetailInfoByCollectionIdOperationCompleted);
            }
            this.InvokeAsync("getDetailInfoByCollectionId", new object[] {
                        arg0,
                        arg1,
                        arg2,
                        arg3}, this.getDetailInfoByCollectionIdOperationCompleted, userState);
        }

        private void OngetDetailInfoByCollectionIdOperationCompleted(object arg) {
            if ((this.getDetailInfoByCollectionIdCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.getDetailInfoByCollectionIdCompleted(this, new getDetailInfoByCollectionIdCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }

        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("", RequestNamespace="http://service.webservice.accela.com/", ResponseNamespace="http://service.webservice.accela.com/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public void copyCaps2Collection([System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)] MyCollectionModel arg0, [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)] MyCollectionModel arg1) {
            this.Invoke("copyCaps2Collection", new object[] {
                        arg0,
                        arg1});
        }
        
        /// <remarks/>
        public void copyCaps2CollectionAsync(MyCollectionModel arg0, MyCollectionModel arg1) {
            this.copyCaps2CollectionAsync(arg0, arg1, null);
        }
        
        /// <remarks/>
        public void copyCaps2CollectionAsync(MyCollectionModel arg0, MyCollectionModel arg1, object userState) {
            if ((this.copyCaps2CollectionOperationCompleted == null)) {
                this.copyCaps2CollectionOperationCompleted = new System.Threading.SendOrPostCallback(this.OncopyCaps2CollectionOperationCompleted);
            }
            this.InvokeAsync("copyCaps2Collection", new object[] {
                        arg0,
                        arg1}, this.copyCaps2CollectionOperationCompleted, userState);
        }
        
        private void OncopyCaps2CollectionOperationCompleted(object arg) {
            if ((this.copyCaps2CollectionCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.copyCaps2CollectionCompleted(this, new System.ComponentModel.AsyncCompletedEventArgs(invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("", RequestNamespace="http://service.webservice.accela.com/", ResponseNamespace="http://service.webservice.accela.com/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public void addCaps2Collection([System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)] MyCollectionModel arg0) {
            this.Invoke("addCaps2Collection", new object[] {
                        arg0});
        }
        
        /// <remarks/>
        public void addCaps2CollectionAsync(MyCollectionModel arg0) {
            this.addCaps2CollectionAsync(arg0, null);
        }
        
        /// <remarks/>
        public void addCaps2CollectionAsync(MyCollectionModel arg0, object userState) {
            if ((this.addCaps2CollectionOperationCompleted == null)) {
                this.addCaps2CollectionOperationCompleted = new System.Threading.SendOrPostCallback(this.OnaddCaps2CollectionOperationCompleted);
            }
            this.InvokeAsync("addCaps2Collection", new object[] {
                        arg0}, this.addCaps2CollectionOperationCompleted, userState);
        }
        
        private void OnaddCaps2CollectionOperationCompleted(object arg) {
            if ((this.addCaps2CollectionCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.addCaps2CollectionCompleted(this, new System.ComponentModel.AsyncCompletedEventArgs(invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("", RequestNamespace="http://service.webservice.accela.com/", ResponseNamespace="http://service.webservice.accela.com/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public void updateCollection([System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)] MyCollectionModel arg0) {
            this.Invoke("updateCollection", new object[] {
                        arg0});
        }
        
        /// <remarks/>
        public void updateCollectionAsync(MyCollectionModel arg0) {
            this.updateCollectionAsync(arg0, null);
        }
        
        /// <remarks/>
        public void updateCollectionAsync(MyCollectionModel arg0, object userState) {
            if ((this.updateCollectionOperationCompleted == null)) {
                this.updateCollectionOperationCompleted = new System.Threading.SendOrPostCallback(this.OnupdateCollectionOperationCompleted);
            }
            this.InvokeAsync("updateCollection", new object[] {
                        arg0}, this.updateCollectionOperationCompleted, userState);
        }
        
        private void OnupdateCollectionOperationCompleted(object arg) {
            if ((this.updateCollectionCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.updateCollectionCompleted(this, new System.ComponentModel.AsyncCompletedEventArgs(invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("", RequestNamespace="http://service.webservice.accela.com/", ResponseNamespace="http://service.webservice.accela.com/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute("return", Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public MyCollectionModel[] getCollections([System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)] string arg0, [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)] string arg1) {
            object[] results = this.Invoke("getCollections", new object[] {
                        arg0,
                        arg1});
            return ((MyCollectionModel[])(results[0]));
        }
        
        /// <remarks/>
        public void getCollectionsAsync(string arg0, string arg1) {
            this.getCollectionsAsync(arg0, arg1, null);
        }
        
        /// <remarks/>
        public void getCollectionsAsync(string arg0, string arg1, object userState) {
            if ((this.getCollectionsOperationCompleted == null)) {
                this.getCollectionsOperationCompleted = new System.Threading.SendOrPostCallback(this.OngetCollectionsOperationCompleted);
            }
            this.InvokeAsync("getCollections", new object[] {
                        arg0,
                        arg1}, this.getCollectionsOperationCompleted, userState);
        }
        
        private void OngetCollectionsOperationCompleted(object arg) {
            if ((this.getCollectionsCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.getCollectionsCompleted(this, new getCollectionsCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("", RequestNamespace="http://service.webservice.accela.com/", ResponseNamespace="http://service.webservice.accela.com/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public void moveCaps2Collection([System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)] MyCollectionModel arg0, [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)] MyCollectionModel arg1) {
            this.Invoke("moveCaps2Collection", new object[] {
                        arg0,
                        arg1});
        }
        
        /// <remarks/>
        public void moveCaps2CollectionAsync(MyCollectionModel arg0, MyCollectionModel arg1) {
            this.moveCaps2CollectionAsync(arg0, arg1, null);
        }
        
        /// <remarks/>
        public void moveCaps2CollectionAsync(MyCollectionModel arg0, MyCollectionModel arg1, object userState) {
            if ((this.moveCaps2CollectionOperationCompleted == null)) {
                this.moveCaps2CollectionOperationCompleted = new System.Threading.SendOrPostCallback(this.OnmoveCaps2CollectionOperationCompleted);
            }
            this.InvokeAsync("moveCaps2Collection", new object[] {
                        arg0,
                        arg1}, this.moveCaps2CollectionOperationCompleted, userState);
        }
        
        private void OnmoveCaps2CollectionOperationCompleted(object arg) {
            if ((this.moveCaps2CollectionCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.moveCaps2CollectionCompleted(this, new System.ComponentModel.AsyncCompletedEventArgs(invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("", RequestNamespace="http://service.webservice.accela.com/", ResponseNamespace="http://service.webservice.accela.com/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public void createCollection([System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)] MyCollectionModel arg0) {
            this.Invoke("createCollection", new object[] {
                        arg0});
        }
        
        /// <remarks/>
        public void createCollectionAsync(MyCollectionModel arg0) {
            this.createCollectionAsync(arg0, null);
        }
        
        /// <remarks/>
        public void createCollectionAsync(MyCollectionModel arg0, object userState) {
            if ((this.createCollectionOperationCompleted == null)) {
                this.createCollectionOperationCompleted = new System.Threading.SendOrPostCallback(this.OncreateCollectionOperationCompleted);
            }
            this.InvokeAsync("createCollection", new object[] {
                        arg0}, this.createCollectionOperationCompleted, userState);
        }
        
        private void OncreateCollectionOperationCompleted(object arg) {
            if ((this.createCollectionCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.createCollectionCompleted(this, new System.ComponentModel.AsyncCompletedEventArgs(invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("", RequestNamespace="http://service.webservice.accela.com/", ResponseNamespace="http://service.webservice.accela.com/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public void deleteCollection([System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)] string arg0, [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)] string arg1, [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)] string arg2) {
            this.Invoke("deleteCollection", new object[] {
                        arg0,
                        arg1,
                        arg2});
        }
        
        /// <remarks/>
        public void deleteCollectionAsync(string arg0, string arg1, string arg2) {
            this.deleteCollectionAsync(arg0, arg1, arg2, null);
        }
        
        /// <remarks/>
        public void deleteCollectionAsync(string arg0, string arg1, string arg2, object userState) {
            if ((this.deleteCollectionOperationCompleted == null)) {
                this.deleteCollectionOperationCompleted = new System.Threading.SendOrPostCallback(this.OndeleteCollectionOperationCompleted);
            }
            this.InvokeAsync("deleteCollection", new object[] {
                        arg0,
                        arg1,
                        arg2}, this.deleteCollectionOperationCompleted, userState);
        }
        
        private void OndeleteCollectionOperationCompleted(object arg) {
            if ((this.deleteCollectionCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.deleteCollectionCompleted(this, new System.ComponentModel.AsyncCompletedEventArgs(invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
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
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.1433")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://service.webservice.accela.com/")]
    public partial class deleteCollectionResponse {
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.1433")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://service.webservice.accela.com/")]
    public partial class deleteCollection {
        
        private string arg0Field;
        
        private string arg1Field;
        
        private string arg2Field;
        
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
        public string arg1 {
            get {
                return this.arg1Field;
            }
            set {
                this.arg1Field = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string arg2 {
            get {
                return this.arg2Field;
            }
            set {
                this.arg2Field = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.1433")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://service.webservice.accela.com/")]
    public partial class createCollectionResponse {
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.1433")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://service.webservice.accela.com/")]
    public partial class createCollection {
        
        private MyCollectionModel arg0Field;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public MyCollectionModel arg0 {
            get {
                return this.arg0Field;
            }
            set {
                this.arg0Field = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.1433")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://service.webservice.accela.com/")]
    public partial class moveCaps2CollectionResponse {
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.1433")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://service.webservice.accela.com/")]
    public partial class moveCaps2Collection {
        
        private MyCollectionModel arg0Field;
        
        private MyCollectionModel arg1Field;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public MyCollectionModel arg0 {
            get {
                return this.arg0Field;
            }
            set {
                this.arg0Field = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public MyCollectionModel arg1 {
            get {
                return this.arg1Field;
            }
            set {
                this.arg1Field = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.1433")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://service.webservice.accela.com/")]
    public partial class getCollections {
        
        private string arg0Field;
        
        private string arg1Field;
        
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
        public string arg1 {
            get {
                return this.arg1Field;
            }
            set {
                this.arg1Field = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.1433")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://service.webservice.accela.com/")]
    public partial class updateCollectionResponse {
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.1433")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://service.webservice.accela.com/")]
    public partial class updateCollection {
        
        private MyCollectionModel arg0Field;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public MyCollectionModel arg0 {
            get {
                return this.arg0Field;
            }
            set {
                this.arg0Field = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.1433")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://service.webservice.accela.com/")]
    public partial class addCaps2CollectionResponse {
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.1433")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://service.webservice.accela.com/")]
    public partial class addCaps2Collection {
        
        private MyCollectionModel arg0Field;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public MyCollectionModel arg0 {
            get {
                return this.arg0Field;
            }
            set {
                this.arg0Field = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.1433")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://service.webservice.accela.com/")]
    public partial class copyCaps2CollectionResponse {
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.1433")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://service.webservice.accela.com/")]
    public partial class copyCaps2Collection {
        
        private MyCollectionModel arg0Field;
        
        private MyCollectionModel arg1Field;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public MyCollectionModel arg0 {
            get {
                return this.arg0Field;
            }
            set {
                this.arg0Field = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public MyCollectionModel arg1 {
            get {
                return this.arg1Field;
            }
            set {
                this.arg1Field = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.1433")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://service.webservice.accela.com/")]
    public partial class getDetailInfoByCollectionIdResponse {
        
        private MyCollectionModel returnField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public MyCollectionModel @return {
            get {
                return this.returnField;
            }
            set {
                this.returnField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.1433")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://service.webservice.accela.com/")]
    public partial class getDetailInfoByCollectionId {
        
        private string arg0Field;
        
        private string arg1Field;
        
        private string arg2Field;
        
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
        public string arg1 {
            get {
                return this.arg1Field;
            }
            set {
                this.arg1Field = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string arg2 {
            get {
                return this.arg2Field;
            }
            set {
                this.arg2Field = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.1433")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://service.webservice.accela.com/")]
    public partial class removeCapsFromCollectionResponse {
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.1433")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://service.webservice.accela.com/")]
    public partial class removeCapsFromCollection {
        
        private MyCollectionModel arg0Field;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public MyCollectionModel arg0 {
            get {
                return this.arg0Field;
            }
            set {
                this.arg0Field = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.1433")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://service.webservice.accela.com/")]
    public partial class getCollections4Management {
        
        private string arg0Field;
        
        private string arg1Field;
        
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
        public string arg1 {
            get {
                return this.arg1Field;
            }
            set {
                this.arg1Field = value;
            }
        }
    }
    
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "2.0.50727.1433")]
    public delegate void getCollections4ManagementCompletedEventHandler(object sender, getCollections4ManagementCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "2.0.50727.1433")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class getCollections4ManagementCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal getCollections4ManagementCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public MyCollectionModel[] Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((MyCollectionModel[])(this.results[0]));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "2.0.50727.1433")]
    public delegate void removeCapsFromCollectionCompletedEventHandler(object sender, System.ComponentModel.AsyncCompletedEventArgs e);

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.1")]
    public delegate void getDetailInfoByCollectionIdCompletedEventHandler(object sender, getDetailInfoByCollectionIdCompletedEventArgs e);

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.1")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class getDetailInfoByCollectionIdCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {

        private object[] results;

        internal getDetailInfoByCollectionIdCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) :
            base(exception, cancelled, userState) {
            this.results = results;
        }

        /// <remarks/>
        public MyCollectionModel Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((MyCollectionModel)(this.results[0]));
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "2.0.50727.1433")]
    public delegate void copyCaps2CollectionCompletedEventHandler(object sender, System.ComponentModel.AsyncCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "2.0.50727.1433")]
    public delegate void addCaps2CollectionCompletedEventHandler(object sender, System.ComponentModel.AsyncCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "2.0.50727.1433")]
    public delegate void updateCollectionCompletedEventHandler(object sender, System.ComponentModel.AsyncCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "2.0.50727.1433")]
    public delegate void getCollectionsCompletedEventHandler(object sender, getCollectionsCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "2.0.50727.1433")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class getCollectionsCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal getCollectionsCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public MyCollectionModel[] Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((MyCollectionModel[])(this.results[0]));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "2.0.50727.1433")]
    public delegate void moveCaps2CollectionCompletedEventHandler(object sender, System.ComponentModel.AsyncCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "2.0.50727.1433")]
    public delegate void createCollectionCompletedEventHandler(object sender, System.ComponentModel.AsyncCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "2.0.50727.1433")]
    public delegate void deleteCollectionCompletedEventHandler(object sender, System.ComponentModel.AsyncCompletedEventArgs e);
}

#pragma warning restore 1591