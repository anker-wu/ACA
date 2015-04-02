/**
 * <pre>
 * 
 *  Accela Citizen Access
 *  File: PublicUserModel4WS.cs
 * 
 *  Accela, Inc.
 *  Copyright (C): 2007-2014
 * 
 *  Description:
 * 
 *  Notes:
 * $Id: PublicUserModel4WS.cs 269106 2014-04-04 07:54:20Z ACHIEVO\james.shi $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

namespace Accela.ACA.WSProxy
{
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.18060")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://model.webservice.accela.com")]
    public partial class PublicUserModel4WS
    {

        private string accountTypeField;

        private string addressField;

        private string address2Field;

        private RefAddressModel[] addressListField;

        private string auditDateField;

        private string auditIDField;

        private string auditStatusField;

        private string authAgentIDField;

        private string authServiceProviderCodeField;

        private string birthDateField;

        private string businessNameField;

        private string cellPhoneField;

        private string cellPhoneCountryCodeField;

        private string cityField;

        private string cookieField;

        private string countryField;

        private string emailField;

        private string enablePrintField;

        private string faxField;

        private string faxCountryCodeField;

        private string feinField;

        private string firstNameField;

        private string genderField;

        private string homePhoneField;

        private string homePhoneCountryCodeField;

        private PublicUserModel4WS[] initialUsersField;

        private string lastNameField;

        private LicenseModel4WS[] licenseModelField;

        private string maskedSsnField;

        private string middleNameField;

        private string needChangePasswordField;

        private OwnerModel[] ownerModelField;

        private string pagerField;

        private ParcelModel[] parcelListField;

        private string passwordField;

        private string passwordChangeDateField;

        private string passwordHintField;

        private string passwordRequestAnswerField;

        private string passwordRequestQuestionField;

        private PublicUserQuestionModel[] questionsField;

        private PeopleModel4WS[] peopleModelField;

        private string poboxField;

        private string prefContactChannelField;

        private string prefPhoneField;

        private XProxyUserModel proxyUserModelField;

        private PublicUserModel4WS[] proxyUsersField;

        private string receiveSMSField;

        private string roleTypeField;

        private string salutationField;

        private string servProvCodeField;

        private string ssnField;

        private string stateField;

        private string statusOfV360UserField;

        private TemplateAttributeModel[] templateAttributesField;

        private string userIDField;

        private string userSeqNumField;

        private string userTitleField;

        private string viadorUrlField;

        private string workPhoneField;

        private string workPhoneCountryCodeField;

        private string zipField;

        private XSocialMediaUserModel[] xSocialMediaField;

        private string statusField;

        private string uUIDField;

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
        [System.Xml.Serialization.XmlElementAttribute("addressList", Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = true)]
        public RefAddressModel[] addressList
        {
            get
            {
                return this.addressListField;
            }
            set
            {
                this.addressListField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string auditDate
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
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string birthDate
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
        [System.Xml.Serialization.XmlElementAttribute("initialUsers", Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = true)]
        public PublicUserModel4WS[] initialUsers
        {
            get
            {
                return this.initialUsersField;
            }
            set
            {
                this.initialUsersField = value;
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
        [System.Xml.Serialization.XmlElementAttribute("licenseModel", Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = true)]
        public LicenseModel4WS[] licenseModel
        {
            get
            {
                return this.licenseModelField;
            }
            set
            {
                this.licenseModelField = value;
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
        [System.Xml.Serialization.XmlElementAttribute("ownerModel", Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = true)]
        public OwnerModel[] ownerModel
        {
            get
            {
                return this.ownerModelField;
            }
            set
            {
                this.ownerModelField = value;
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
        [System.Xml.Serialization.XmlElementAttribute("parcelList", Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = true)]
        public ParcelModel[] parcelList
        {
            get
            {
                return this.parcelListField;
            }
            set
            {
                this.parcelListField = value;
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
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string passwordChangeDate
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
        [System.Xml.Serialization.XmlElementAttribute("peopleModel", Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = true)]
        public PeopleModel4WS[] peopleModel
        {
            get
            {
                return this.peopleModelField;
            }
            set
            {
                this.peopleModelField = value;
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
        [System.Xml.Serialization.XmlElementAttribute("proxyUsers", Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = true)]
        public PublicUserModel4WS[] proxyUsers
        {
            get
            {
                return this.proxyUsersField;
            }
            set
            {
                this.proxyUsersField = value;
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
        [System.Xml.Serialization.XmlElementAttribute("templateAttributes", Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = true)]
        public TemplateAttributeModel[] templateAttributes
        {
            get
            {
                return this.templateAttributesField;
            }
            set
            {
                this.templateAttributesField = value;
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
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string userSeqNum
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
    }
}
