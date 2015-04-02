#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: Reference.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2009-2014
 *
 *  Description:
 *  
 *
 *  Notes:
 *  $Id: Reference.cs 266805 2014-03-03 05:28:29Z ACHIEVO\james.shi $.
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
    [System.Web.Services.WebServiceBindingAttribute(Name="ConditionWebServiceServiceSoapBinding", Namespace="http://service.webservice.accela.com/")]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(GenericTemplateFieldPK))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(LanguageModel))]
    public partial class ConditionWebServiceService : System.Web.Services.Protocols.SoapHttpClientProtocol {
        
        private System.Threading.SendOrPostCallback getContactConditionsOperationCompleted;

        private System.Threading.SendOrPostCallback getCapConditionApprovalByNbrOperationCompleted;
        
        private System.Threading.SendOrPostCallback getAddressConditionsOperationCompleted;
        
        private System.Threading.SendOrPostCallback isParcelLockedOperationCompleted;
        
        private System.Threading.SendOrPostCallback isAddressLockedOperationCompleted;
        
        private System.Threading.SendOrPostCallback isOwnerLockedOperationCompleted;
        
        private System.Threading.SendOrPostCallback isLicenseLockedOperationCompleted;
        
        private System.Threading.SendOrPostCallback isCapLockedOperationCompleted;
        
        private System.Threading.SendOrPostCallback isContactLockedOperationCompleted;
        
        private System.Threading.SendOrPostCallback getAllCondition4ACAFeeEstimateOperationCompleted;

        private System.Threading.SendOrPostCallback getCapConditionsOperationCompleted;
        
        private System.Threading.SendOrPostCallback getParcelConditionsOperationCompleted;
        
        private bool useDefaultCredentialsSetExplicitly;
        
        /// <remarks/>
        public ConditionWebServiceService() {
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
        public event getContactConditionsCompletedEventHandler getContactConditionsCompleted;

        /// <remarks/>
        public event getCapConditionApprovalByNbrCompletedEventHandler getCapConditionApprovalByNbrCompleted;
        
        /// <remarks/>
        public event getAddressConditionsCompletedEventHandler getAddressConditionsCompleted;
        
        /// <remarks/>
        public event isParcelLockedCompletedEventHandler isParcelLockedCompleted;
        
        /// <remarks/>
        public event isAddressLockedCompletedEventHandler isAddressLockedCompleted;
        
        /// <remarks/>
        public event isOwnerLockedCompletedEventHandler isOwnerLockedCompleted;
        
        /// <remarks/>
        public event isLicenseLockedCompletedEventHandler isLicenseLockedCompleted;
        
        /// <remarks/>
        public event isCapLockedCompletedEventHandler isCapLockedCompleted;
        
        /// <remarks/>
        public event isContactLockedCompletedEventHandler isContactLockedCompleted;
        
        /// <remarks/>
        public event getAllCondition4ACAFeeEstimateCompletedEventHandler getAllCondition4ACAFeeEstimateCompleted;

        /// <remarks/>
        public event getCapConditionsCompletedEventHandler getCapConditionsCompleted;
        
        /// <remarks/>
        public event getParcelConditionsCompletedEventHandler getParcelConditionsCompleted;
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("", RequestNamespace="http://service.webservice.accela.com/", ResponseNamespace="http://service.webservice.accela.com/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute("return", Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public NoticeConditionModel[] getContactConditions([System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)] string arg0, [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)] string arg1) {
            object[] results = this.Invoke("getContactConditions", new object[] {
                        arg0,
                        arg1});
            return ((NoticeConditionModel[])(results[0]));
        }
        
        /// <remarks/>
        public void getContactConditionsAsync(string arg0, string arg1) {
            this.getContactConditionsAsync(arg0, arg1, null);
        }
        
        /// <remarks/>
        public void getContactConditionsAsync(string arg0, string arg1, object userState) {
            if ((this.getContactConditionsOperationCompleted == null)) {
                this.getContactConditionsOperationCompleted = new System.Threading.SendOrPostCallback(this.OngetContactConditionsOperationCompleted);
            }
            this.InvokeAsync("getContactConditions", new object[] {
                        arg0,
                        arg1}, this.getContactConditionsOperationCompleted, userState);
        }
        
        private void OngetContactConditionsOperationCompleted(object arg) {
            if ((this.getContactConditionsCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.getContactConditionsCompleted(this, new getContactConditionsCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }

        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("", RequestNamespace = "http://service.webservice.accela.com/", ResponseNamespace = "http://service.webservice.accela.com/", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute("return", Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public NoticeConditionModel getCapConditionApprovalByNbr([System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)] CapIDModel arg0, [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)] string arg1)
        {
            object[] results = this.Invoke("getCapConditionApprovalByNbr", new object[] {
                        arg0,
                        arg1});
            return ((NoticeConditionModel)(results[0]));
        }

        /// <remarks/>
        public void getCapConditionApprovalByNbrAsync(CapIDModel arg0, string arg1)
        {
            this.getCapConditionApprovalByNbrAsync(arg0, arg1, null);
        }

        /// <remarks/>
        public void getCapConditionApprovalByNbrAsync(CapIDModel arg0, string arg1, object userState)
        {
            if ((this.getCapConditionApprovalByNbrOperationCompleted == null))
            {
                this.getCapConditionApprovalByNbrOperationCompleted = new System.Threading.SendOrPostCallback(this.OngetCapConditionApprovalByNbrOperationCompleted);
            }
            this.InvokeAsync("getCapConditionApprovalByNbr", new object[] {
                        arg0,
                        arg1}, this.getCapConditionApprovalByNbrOperationCompleted, userState);
        }

        private void OngetCapConditionApprovalByNbrOperationCompleted(object arg)
        {
            if ((this.getCapConditionApprovalByNbrCompleted != null))
            {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.getCapConditionApprovalByNbrCompleted(this, new getCapConditionApprovalByNbrCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("", RequestNamespace="http://service.webservice.accela.com/", ResponseNamespace="http://service.webservice.accela.com/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute("return", Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public NoticeConditionModel[] getAddressConditions([System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)] string arg0, [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)] RefAddressModel arg1, [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)] string arg2) {
            object[] results = this.Invoke("getAddressConditions", new object[] {
                        arg0,
                        arg1,
                        arg2});
            return ((NoticeConditionModel[])(results[0]));
        }
        
        /// <remarks/>
        public void getAddressConditionsAsync(string arg0, RefAddressModel arg1, string arg2) {
            this.getAddressConditionsAsync(arg0, arg1, arg2, null);
        }
        
        /// <remarks/>
        public void getAddressConditionsAsync(string arg0, RefAddressModel arg1, string arg2, object userState) {
            if ((this.getAddressConditionsOperationCompleted == null)) {
                this.getAddressConditionsOperationCompleted = new System.Threading.SendOrPostCallback(this.OngetAddressConditionsOperationCompleted);
            }
            this.InvokeAsync("getAddressConditions", new object[] {
                        arg0,
                        arg1,
                        arg2}, this.getAddressConditionsOperationCompleted, userState);
        }
        
        private void OngetAddressConditionsOperationCompleted(object arg) {
            if ((this.getAddressConditionsCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.getAddressConditionsCompleted(this, new getAddressConditionsCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("", RequestNamespace="http://service.webservice.accela.com/", ResponseNamespace="http://service.webservice.accela.com/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute("return", Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public bool isParcelLocked([System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)] string arg0, [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)] ParcelModel arg1, [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)] string arg2) {
            object[] results = this.Invoke("isParcelLocked", new object[] {
                        arg0,
                        arg1,
                        arg2});
            return ((bool)(results[0]));
        }
        
        /// <remarks/>
        public void isParcelLockedAsync(string arg0, ParcelModel arg1, string arg2) {
            this.isParcelLockedAsync(arg0, arg1, arg2, null);
        }
        
        /// <remarks/>
        public void isParcelLockedAsync(string arg0, ParcelModel arg1, string arg2, object userState) {
            if ((this.isParcelLockedOperationCompleted == null)) {
                this.isParcelLockedOperationCompleted = new System.Threading.SendOrPostCallback(this.OnisParcelLockedOperationCompleted);
            }
            this.InvokeAsync("isParcelLocked", new object[] {
                        arg0,
                        arg1,
                        arg2}, this.isParcelLockedOperationCompleted, userState);
        }
        
        private void OnisParcelLockedOperationCompleted(object arg) {
            if ((this.isParcelLockedCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.isParcelLockedCompleted(this, new isParcelLockedCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("", RequestNamespace="http://service.webservice.accela.com/", ResponseNamespace="http://service.webservice.accela.com/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute("return", Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public bool isAddressLocked([System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)] string arg0, [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)] RefAddressModel arg1, [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)] string arg2) {
            object[] results = this.Invoke("isAddressLocked", new object[] {
                        arg0,
                        arg1,
                        arg2});
            return ((bool)(results[0]));
        }
        
        /// <remarks/>
        public void isAddressLockedAsync(string arg0, RefAddressModel arg1, string arg2) {
            this.isAddressLockedAsync(arg0, arg1, arg2, null);
        }
        
        /// <remarks/>
        public void isAddressLockedAsync(string arg0, RefAddressModel arg1, string arg2, object userState) {
            if ((this.isAddressLockedOperationCompleted == null)) {
                this.isAddressLockedOperationCompleted = new System.Threading.SendOrPostCallback(this.OnisAddressLockedOperationCompleted);
            }
            this.InvokeAsync("isAddressLocked", new object[] {
                        arg0,
                        arg1,
                        arg2}, this.isAddressLockedOperationCompleted, userState);
        }
        
        private void OnisAddressLockedOperationCompleted(object arg) {
            if ((this.isAddressLockedCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.isAddressLockedCompleted(this, new isAddressLockedCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("", RequestNamespace="http://service.webservice.accela.com/", ResponseNamespace="http://service.webservice.accela.com/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute("return", Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public bool isOwnerLocked([System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)] string arg0, [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)] OwnerModel arg1, [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)] string arg2) {
            object[] results = this.Invoke("isOwnerLocked", new object[] {
                        arg0,
                        arg1,
                        arg2});
            return ((bool)(results[0]));
        }
        
        /// <remarks/>
        public void isOwnerLockedAsync(string arg0, OwnerModel arg1, string arg2) {
            this.isOwnerLockedAsync(arg0, arg1, arg2, null);
        }
        
        /// <remarks/>
        public void isOwnerLockedAsync(string arg0, OwnerModel arg1, string arg2, object userState) {
            if ((this.isOwnerLockedOperationCompleted == null)) {
                this.isOwnerLockedOperationCompleted = new System.Threading.SendOrPostCallback(this.OnisOwnerLockedOperationCompleted);
            }
            this.InvokeAsync("isOwnerLocked", new object[] {
                        arg0,
                        arg1,
                        arg2}, this.isOwnerLockedOperationCompleted, userState);
        }
        
        private void OnisOwnerLockedOperationCompleted(object arg) {
            if ((this.isOwnerLockedCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.isOwnerLockedCompleted(this, new isOwnerLockedCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("", RequestNamespace="http://service.webservice.accela.com/", ResponseNamespace="http://service.webservice.accela.com/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute("return", Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public bool isLicenseLocked([System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)] string arg0, [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)] long arg1, [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)] string arg2) {
            object[] results = this.Invoke("isLicenseLocked", new object[] {
                        arg0,
                        arg1,
                        arg2});
            return ((bool)(results[0]));
        }
        
        /// <remarks/>
        public void isLicenseLockedAsync(string arg0, long arg1, string arg2) {
            this.isLicenseLockedAsync(arg0, arg1, arg2, null);
        }
        
        /// <remarks/>
        public void isLicenseLockedAsync(string arg0, long arg1, string arg2, object userState) {
            if ((this.isLicenseLockedOperationCompleted == null)) {
                this.isLicenseLockedOperationCompleted = new System.Threading.SendOrPostCallback(this.OnisLicenseLockedOperationCompleted);
            }
            this.InvokeAsync("isLicenseLocked", new object[] {
                        arg0,
                        arg1,
                        arg2}, this.isLicenseLockedOperationCompleted, userState);
        }
        
        private void OnisLicenseLockedOperationCompleted(object arg) {
            if ((this.isLicenseLockedCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.isLicenseLockedCompleted(this, new isLicenseLockedCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("", RequestNamespace="http://service.webservice.accela.com/", ResponseNamespace="http://service.webservice.accela.com/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute("return", Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public bool isCapLocked([System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)] CapIDModel arg0) {
            object[] results = this.Invoke("isCapLocked", new object[] {
                        arg0});
            return ((bool)(results[0]));
        }
        
        /// <remarks/>
        public void isCapLockedAsync(CapIDModel arg0) {
            this.isCapLockedAsync(arg0, null);
        }
        
        /// <remarks/>
        public void isCapLockedAsync(CapIDModel arg0, object userState) {
            if ((this.isCapLockedOperationCompleted == null)) {
                this.isCapLockedOperationCompleted = new System.Threading.SendOrPostCallback(this.OnisCapLockedOperationCompleted);
            }
            this.InvokeAsync("isCapLocked", new object[] {
                        arg0}, this.isCapLockedOperationCompleted, userState);
        }
        
        private void OnisCapLockedOperationCompleted(object arg) {
            if ((this.isCapLockedCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.isCapLockedCompleted(this, new isCapLockedCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("", RequestNamespace="http://service.webservice.accela.com/", ResponseNamespace="http://service.webservice.accela.com/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute("return", Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public bool isContactLocked([System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)] string arg0, [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)] long arg1, [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)] string arg2) {
            object[] results = this.Invoke("isContactLocked", new object[] {
                        arg0,
                        arg1,
                        arg2});
            return ((bool)(results[0]));
        }
        
        /// <remarks/>
        public void isContactLockedAsync(string arg0, long arg1, string arg2) {
            this.isContactLockedAsync(arg0, arg1, arg2, null);
        }
        
        /// <remarks/>
        public void isContactLockedAsync(string arg0, long arg1, string arg2, object userState) {
            if ((this.isContactLockedOperationCompleted == null)) {
                this.isContactLockedOperationCompleted = new System.Threading.SendOrPostCallback(this.OnisContactLockedOperationCompleted);
            }
            this.InvokeAsync("isContactLocked", new object[] {
                        arg0,
                        arg1,
                        arg2}, this.isContactLockedOperationCompleted, userState);
        }
        
        private void OnisContactLockedOperationCompleted(object arg) {
            if ((this.isContactLockedCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.isContactLockedCompleted(this, new isContactLockedCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("", RequestNamespace = "http://service.webservice.accela.com/", ResponseNamespace = "http://service.webservice.accela.com/", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute("return", Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public NoticeConditionModel[] getAllCondition4ACAFeeEstimate([System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)] CapIDModel4WS arg0, [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)] string arg1, [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)] string arg2) {
            object[] results = this.Invoke("getAllCondition4ACAFeeEstimate", new object[] {
                        arg0,
                        arg1,
                        arg2});
            return ((NoticeConditionModel[])(results[0]));
        }

        /// <remarks/>
        public void getAllCondition4ACAFeeEstimateAsync(CapIDModel4WS arg0, string arg1, string arg2) {
            this.getAllCondition4ACAFeeEstimateAsync(arg0, arg1, arg2, null);
        }

        /// <remarks/>
        public void getAllCondition4ACAFeeEstimateAsync(CapIDModel4WS arg0, string arg1, string arg2, object userState) {
            if ((this.getAllCondition4ACAFeeEstimateOperationCompleted == null)) {
                this.getAllCondition4ACAFeeEstimateOperationCompleted = new System.Threading.SendOrPostCallback(this.OngetAllCondition4ACAFeeEstimateOperationCompleted);
            }
            this.InvokeAsync("getAllCondition4ACAFeeEstimate", new object[] {
                        arg0,
                        arg1,
                        arg2}, this.getAllCondition4ACAFeeEstimateOperationCompleted, userState);
        }

        private void OngetAllCondition4ACAFeeEstimateOperationCompleted(object arg){
            if ((this.getAllCondition4ACAFeeEstimateCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.getAllCondition4ACAFeeEstimateCompleted(this, new getAllCondition4ACAFeeEstimateCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }

        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("", RequestNamespace = "http://service.webservice.accela.com/", ResponseNamespace = "http://service.webservice.accela.com/", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute("return", Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public CapConditionModel4WS[] getCapConditions([System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)] CapIDModel4WS arg0, [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)] string arg1)
        {
            object[] results = this.Invoke("getCapConditions", new object[] {
                        arg0,
                        arg1});
            return ((CapConditionModel4WS[])(results[0]));
        }

        /// <remarks/>
        public void getCapConditionsAsync(CapIDModel4WS arg0, string arg1)
        {
            this.getCapConditionsAsync(arg0, arg1, null);
        }

        /// <remarks/>
        public void getCapConditionsAsync(CapIDModel4WS arg0, string arg1, object userState)
        {
            if ((this.getCapConditionsOperationCompleted == null))
            {
                this.getCapConditionsOperationCompleted = new System.Threading.SendOrPostCallback(this.OngetCapConditionsOperationCompleted);
            }
            this.InvokeAsync("getCapConditions", new object[] {
                        arg0,
                        arg1}, this.getCapConditionsOperationCompleted, userState);
        }

        private void OngetCapConditionsOperationCompleted(object arg)
        {
            if ((this.getCapConditionsCompleted != null))
            {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.getCapConditionsCompleted(this, new getCapConditionsCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }

        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("", RequestNamespace="http://service.webservice.accela.com/", ResponseNamespace="http://service.webservice.accela.com/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute("return", Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public NoticeConditionModel[] getParcelConditions([System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)] string arg0, [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)] ParcelModel arg1, [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)] string arg2) {
            object[] results = this.Invoke("getParcelConditions", new object[] {
                        arg0,
                        arg1,
                        arg2});
            return ((NoticeConditionModel[])(results[0]));
        }
        
        /// <remarks/>
        public void getParcelConditionsAsync(string arg0, ParcelModel arg1, string arg2) {
            this.getParcelConditionsAsync(arg0, arg1, arg2, null);
        }
        
        /// <remarks/>
        public void getParcelConditionsAsync(string arg0, ParcelModel arg1, string arg2, object userState) {
            if ((this.getParcelConditionsOperationCompleted == null)) {
                this.getParcelConditionsOperationCompleted = new System.Threading.SendOrPostCallback(this.OngetParcelConditionsOperationCompleted);
            }
            this.InvokeAsync("getParcelConditions", new object[] {
                        arg0,
                        arg1,
                        arg2}, this.getParcelConditionsOperationCompleted, userState);
        }
        
        private void OngetParcelConditionsOperationCompleted(object arg) {
            if ((this.getParcelConditionsCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.getParcelConditionsCompleted(this, new getParcelConditionsCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
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
    public delegate void getContactConditionsCompletedEventHandler(object sender, getContactConditionsCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.1")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class getContactConditionsCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal getContactConditionsCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public NoticeConditionModel[] Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((NoticeConditionModel[])(this.results[0]));
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.1")]
    public delegate void getCapConditionApprovalByNbrCompletedEventHandler(object sender, getCapConditionApprovalByNbrCompletedEventArgs e);

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.1")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class getCapConditionApprovalByNbrCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs
    {

        private object[] results;

        internal getCapConditionApprovalByNbrCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) :
            base(exception, cancelled, userState)
        {
            this.results = results;
        }

        /// <remarks/>
        public NoticeConditionModel Result
        {
            get
            {
                this.RaiseExceptionIfNecessary();
                return ((NoticeConditionModel)(this.results[0]));
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.1")]
    public delegate void getAddressConditionsCompletedEventHandler(object sender, getAddressConditionsCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.1")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class getAddressConditionsCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal getAddressConditionsCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public NoticeConditionModel[] Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((NoticeConditionModel[])(this.results[0]));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.1")]
    public delegate void isParcelLockedCompletedEventHandler(object sender, isParcelLockedCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.1")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class isParcelLockedCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal isParcelLockedCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
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
    public delegate void isAddressLockedCompletedEventHandler(object sender, isAddressLockedCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.1")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class isAddressLockedCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal isAddressLockedCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
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
    public delegate void isOwnerLockedCompletedEventHandler(object sender, isOwnerLockedCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.1")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class isOwnerLockedCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal isOwnerLockedCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
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
    public delegate void isLicenseLockedCompletedEventHandler(object sender, isLicenseLockedCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.1")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class isLicenseLockedCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal isLicenseLockedCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
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
    public delegate void isCapLockedCompletedEventHandler(object sender, isCapLockedCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.1")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class isCapLockedCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal isCapLockedCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
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
    public delegate void isContactLockedCompletedEventHandler(object sender, isContactLockedCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.1")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class isContactLockedCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal isContactLockedCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
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
    public delegate void getAllCondition4ACAFeeEstimateCompletedEventHandler(object sender, getAllCondition4ACAFeeEstimateCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.1")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class getAllCondition4ACAFeeEstimateCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal getAllCondition4ACAFeeEstimateCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public NoticeConditionModel[] Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((NoticeConditionModel[])(this.results[0]));
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.1")]
    public delegate void getCapConditionsCompletedEventHandler(object sender, getCapConditionsCompletedEventArgs e);

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.1")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class getCapConditionsCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs
    {

        private object[] results;

        internal getCapConditionsCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) :
            base(exception, cancelled, userState)
        {
            this.results = results;
        }

        /// <remarks/>
        public CapConditionModel4WS[] Result
        {
            get
            {
                this.RaiseExceptionIfNecessary();
                return ((CapConditionModel4WS[])(this.results[0]));
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.1")]
    public delegate void getParcelConditionsCompletedEventHandler(object sender, getParcelConditionsCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.1")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class getParcelConditionsCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal getParcelConditionsCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public NoticeConditionModel[] Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((NoticeConditionModel[])(this.results[0]));
            }
        }
    }
}

#pragma warning restore 1591