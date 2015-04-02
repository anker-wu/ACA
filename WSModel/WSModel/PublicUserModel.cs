/**
 * <pre>
 * 
 *  Accela Citizen Access
 *  File: PublicUserModel.cs
 * 
 *  Accela, Inc.
 *  Copyright (C): 2014
 * 
 *  Description:
 * 
 *  Notes:
 * $Id: PublicUserModel.cs 170671 2014-06-03 05:52:20Z ACHIEVO\joy.duan $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

namespace Accela.ACA.WSProxy
{
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.18408")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://model.webservice.accela.com")]
    public partial class PublicUserModel
    {
        private string accountTypeField;

        private string addressField;

        private string address2Field;

        private System.Nullable<System.DateTime> auditDateField;

        private string auditIDField;

        private string auditStatusField;

        private string authAgentIDField;

        private string authServiceProviderCodeField;

        private System.Nullable<System.DateTime> birthDateField;

        private string businessNameField;

        private string cellPhoneField;

        private string cellPhoneCountryCodeField;

        private string cityField;

        private ContactAddressModel[] contactAddressField;

        private ContactAttributeModel[] contactAttributesField;

        private string cookieField;

        private string countryField;

        private string emailField;

        private string enablePrintField;

        private System.Nullable<System.DateTime> endBirthDateField;

        private string faxField;

        private string faxCountryCodeField;

        private string feinField;

        private string firstNameField;

        private string genderField;

        private string homePhoneField;

        private string homePhoneCountryCodeField;

        private IdentifierModel i18NCountryField;

        private IdentifierModel i18NPrefContactChannelField;

        private IdentifierModel i18NSalutationField;

        private IdentifierModel i18NStateField;

        private string lastNameField;

        private string maskedSsnField;

        private string middleNameField;

        private string needChangePasswordField;

        private string oldPasswordField;

        private string pagerField;

        private string passwordField;

        private System.Nullable<System.DateTime> passwordChangeDateField;

        private string passwordHintField;

        private string passwordRequestAnswerField;

        private string passwordRequestQuestionField;

        private PeopleModel[] peoplesField;

        private string poboxField;

        private string prefContactChannelField;

        private string prefPhoneField;

        private XProxyUserModel proxyUserModelField;

        private PublicUserQuestionModel[] questionsField;

        private string receiveSMSField;

        private System.Nullable<System.DateTime> registerDateField;

        private string roleTypeField;

        private string salutationField;

        private string servProvCodeField;

        private PublicUserModelEntry[] specificInfoField;

        private string ssnField;

        private string stateField;

        private string statusField;

        private string statusOfV360UserField;

        private string uUIDField;

        private string userIDField;

        private System.Nullable<long> userSeqNumField;

        private string userTitleField;

        private string viadorUrlField;

        private string workPhoneField;

        private string workPhoneCountryCodeField;

        private string zipField;

        private XSocialMediaUserModel[] xSocialMediaField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string accountType
        {
            get
            {
                return this.accountTypeField;
            }
            set
            {
                this.accountTypeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string address
        {
            get
            {
                return this.addressField;
            }
            set
            {
                this.addressField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string address2
        {
            get
            {
                return this.address2Field;
            }
            set
            {
                this.address2Field = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = true)]
        public System.Nullable<System.DateTime> auditDate
        {
            get
            {
                return this.auditDateField;
            }
            set
            {
                this.auditDateField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string auditID
        {
            get
            {
                return this.auditIDField;
            }
            set
            {
                this.auditIDField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string auditStatus
        {
            get
            {
                return this.auditStatusField;
            }
            set
            {
                this.auditStatusField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string authAgentID
        {
            get
            {
                return this.authAgentIDField;
            }
            set
            {
                this.authAgentIDField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string authServiceProviderCode
        {
            get
            {
                return this.authServiceProviderCodeField;
            }
            set
            {
                this.authServiceProviderCodeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = true)]
        public System.Nullable<System.DateTime> birthDate
        {
            get
            {
                return this.birthDateField;
            }
            set
            {
                this.birthDateField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string businessName
        {
            get
            {
                return this.businessNameField;
            }
            set
            {
                this.businessNameField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string cellPhone
        {
            get
            {
                return this.cellPhoneField;
            }
            set
            {
                this.cellPhoneField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string cellPhoneCountryCode
        {
            get
            {
                return this.cellPhoneCountryCodeField;
            }
            set
            {
                this.cellPhoneCountryCodeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string city
        {
            get
            {
                return this.cityField;
            }
            set
            {
                this.cityField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("contactAddress", Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = true)]
        public ContactAddressModel[] contactAddress
        {
            get
            {
                return this.contactAddressField;
            }
            set
            {
                this.contactAddressField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("contactAttributes", Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = true)]
        public ContactAttributeModel[] contactAttributes
        {
            get
            {
                return this.contactAttributesField;
            }
            set
            {
                this.contactAttributesField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string cookie
        {
            get
            {
                return this.cookieField;
            }
            set
            {
                this.cookieField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string country
        {
            get
            {
                return this.countryField;
            }
            set
            {
                this.countryField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string email
        {
            get
            {
                return this.emailField;
            }
            set
            {
                this.emailField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string enablePrint
        {
            get
            {
                return this.enablePrintField;
            }
            set
            {
                this.enablePrintField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = true)]
        public System.Nullable<System.DateTime> endBirthDate
        {
            get
            {
                return this.endBirthDateField;
            }
            set
            {
                this.endBirthDateField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string fax
        {
            get
            {
                return this.faxField;
            }
            set
            {
                this.faxField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string faxCountryCode
        {
            get
            {
                return this.faxCountryCodeField;
            }
            set
            {
                this.faxCountryCodeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string fein
        {
            get
            {
                return this.feinField;
            }
            set
            {
                this.feinField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string firstName
        {
            get
            {
                return this.firstNameField;
            }
            set
            {
                this.firstNameField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string gender
        {
            get
            {
                return this.genderField;
            }
            set
            {
                this.genderField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string homePhone
        {
            get
            {
                return this.homePhoneField;
            }
            set
            {
                this.homePhoneField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string homePhoneCountryCode
        {
            get
            {
                return this.homePhoneCountryCodeField;
            }
            set
            {
                this.homePhoneCountryCodeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public IdentifierModel i18NCountry
        {
            get
            {
                return this.i18NCountryField;
            }
            set
            {
                this.i18NCountryField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public IdentifierModel i18NPrefContactChannel
        {
            get
            {
                return this.i18NPrefContactChannelField;
            }
            set
            {
                this.i18NPrefContactChannelField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public IdentifierModel i18NSalutation
        {
            get
            {
                return this.i18NSalutationField;
            }
            set
            {
                this.i18NSalutationField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public IdentifierModel i18NState
        {
            get
            {
                return this.i18NStateField;
            }
            set
            {
                this.i18NStateField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string lastName
        {
            get
            {
                return this.lastNameField;
            }
            set
            {
                this.lastNameField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string maskedSsn
        {
            get
            {
                return this.maskedSsnField;
            }
            set
            {
                this.maskedSsnField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string middleName
        {
            get
            {
                return this.middleNameField;
            }
            set
            {
                this.middleNameField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string needChangePassword
        {
            get
            {
                return this.needChangePasswordField;
            }
            set
            {
                this.needChangePasswordField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string oldPassword
        {
            get
            {
                return this.oldPasswordField;
            }
            set
            {
                this.oldPasswordField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string pager
        {
            get
            {
                return this.pagerField;
            }
            set
            {
                this.pagerField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string password
        {
            get
            {
                return this.passwordField;
            }
            set
            {
                this.passwordField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = true)]
        public System.Nullable<System.DateTime> passwordChangeDate
        {
            get
            {
                return this.passwordChangeDateField;
            }
            set
            {
                this.passwordChangeDateField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string passwordHint
        {
            get
            {
                return this.passwordHintField;
            }
            set
            {
                this.passwordHintField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string passwordRequestAnswer
        {
            get
            {
                return this.passwordRequestAnswerField;
            }
            set
            {
                this.passwordRequestAnswerField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string passwordRequestQuestion
        {
            get
            {
                return this.passwordRequestQuestionField;
            }
            set
            {
                this.passwordRequestQuestionField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("peoples", Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = true)]
        public PeopleModel[] peoples
        {
            get
            {
                return this.peoplesField;
            }
            set
            {
                this.peoplesField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string pobox
        {
            get
            {
                return this.poboxField;
            }
            set
            {
                this.poboxField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string prefContactChannel
        {
            get
            {
                return this.prefContactChannelField;
            }
            set
            {
                this.prefContactChannelField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string prefPhone
        {
            get
            {
                return this.prefPhoneField;
            }
            set
            {
                this.prefPhoneField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public XProxyUserModel proxyUserModel
        {
            get
            {
                return this.proxyUserModelField;
            }
            set
            {
                this.proxyUserModelField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("questions", Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = true)]
        public PublicUserQuestionModel[] questions
        {
            get
            {
                return this.questionsField;
            }
            set
            {
                this.questionsField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string receiveSMS
        {
            get
            {
                return this.receiveSMSField;
            }
            set
            {
                this.receiveSMSField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = true)]
        public System.Nullable<System.DateTime> registerDate
        {
            get
            {
                return this.registerDateField;
            }
            set
            {
                this.registerDateField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string roleType
        {
            get
            {
                return this.roleTypeField;
            }
            set
            {
                this.roleTypeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string salutation
        {
            get
            {
                return this.salutationField;
            }
            set
            {
                this.salutationField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string servProvCode
        {
            get
            {
                return this.servProvCodeField;
            }
            set
            {
                this.servProvCodeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        [System.Xml.Serialization.XmlArrayItemAttribute("entry", Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = false)]
        public PublicUserModelEntry[] specificInfo
        {
            get
            {
                return this.specificInfoField;
            }
            set
            {
                this.specificInfoField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string ssn
        {
            get
            {
                return this.ssnField;
            }
            set
            {
                this.ssnField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string state
        {
            get
            {
                return this.stateField;
            }
            set
            {
                this.stateField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string status
        {
            get
            {
                return this.statusField;
            }
            set
            {
                this.statusField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string statusOfV360User
        {
            get
            {
                return this.statusOfV360UserField;
            }
            set
            {
                this.statusOfV360UserField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string UUID
        {
            get
            {
                return this.uUIDField;
            }
            set
            {
                this.uUIDField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string userID
        {
            get
            {
                return this.userIDField;
            }
            set
            {
                this.userIDField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = true)]
        public System.Nullable<long> userSeqNum
        {
            get
            {
                return this.userSeqNumField;
            }
            set
            {
                this.userSeqNumField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string userTitle
        {
            get
            {
                return this.userTitleField;
            }
            set
            {
                this.userTitleField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string viadorUrl
        {
            get
            {
                return this.viadorUrlField;
            }
            set
            {
                this.viadorUrlField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string workPhone
        {
            get
            {
                return this.workPhoneField;
            }
            set
            {
                this.workPhoneField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string workPhoneCountryCode
        {
            get
            {
                return this.workPhoneCountryCodeField;
            }
            set
            {
                this.workPhoneCountryCodeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string zip
        {
            get
            {
                return this.zipField;
            }
            set
            {
                this.zipField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("xSocialMedia", Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = true)]
        public XSocialMediaUserModel[] xSocialMedia
        {
            get
            {
                return this.xSocialMediaField;
            }
            set
            {
                this.xSocialMediaField = value;
            }
        }
    }
}