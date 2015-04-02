/**
 * <pre>
 * 
 *  Accela Citizen Access
 *  File: LicenseModel.cs
 * 
 *  Accela, Inc.
 *  Copyright (C): 2011
 * 
 *  Description:
 * 
 *  Notes:
 * $Id: LicenseModel.cs 181867 2010-09-30 08:06:18Z ACHIEVO\daly.zeng $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

using System;

namespace Accela.ACA.WSProxy
{
    /// <remarks/>
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(SearchLicenseModel))]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.1")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://model.webservice.accela.com")]
    public partial class LicenseModel
    {
        private string acaPermissionField;

        private string address1Field;

        private string address2Field;

        private string address3Field;

        private string agencyCodeField;

        private System.Nullable<System.DateTime> auditDateField;

        private string auditIDField;

        private string auditStatusField;

        private System.Nullable<System.DateTime> birthDateField;

        private bool bizLicExpiredField;

        private string busName2Field;

        private System.Nullable<System.DateTime> businessLicExpDateField;

        private string businessLicenseField;

        private string businessNameField;

        private string cityField;

        private string cityCodeField;

        private string commentField;

        private string contLicBusNameField;

        private string contactFirstNameField;

        private string contactLastNameField;

        private string contactMiddleNameField;

        private System.Nullable<long> contrLicNoField;

        private string contryCodeField;

        private string countryField;

        private string countryCodeField;

        private string degreeField;

        private string disciplineField;

        private string eMailAddressField;

        private string einSsField;

        private System.Nullable<System.DateTime> endbirthDateField;

        private string faxField;

        private string faxCountryCodeField;

        private string feinField;

        private string genderField;

        private string holdCodeField;

        private string holdDescField;

        private bool insExpiredField;

        private System.Nullable<double> insuranceAmountField;

        private string insuranceCoField;

        private System.Nullable<System.DateTime> insuranceExpDateField;

        private bool isTNExpiredField;

        private System.Nullable<long> ivrPinNumberField;

        private bool licExpiredField;

        private System.Nullable<System.DateTime> licOrigIssDateField;

        private System.Nullable<long> licSeqNbrField;

        private string licStateField;

        private RecordTypeLicTypePermissionModel licTypePermissionField;

        private string licenseBoardField;

        private System.Nullable<System.DateTime> licenseExpirationDateField;

        private System.Nullable<System.DateTime> licenseIssueDateField;

        private System.Nullable<System.DateTime> licenseLastRenewalDateField;

        private string licenseStatusField;

        private string licenseTypeField;

        private string maskedSsnField;

        private string permStatusCodeField;

        private string phone1Field;

        private string phone1CountryCodeField;

        private string phone2Field;

        private string phone2CountryCodeField;

        private string phone3Field;

        private string phone3CountryCodeField;

        private string policyField;

        private string postOfficeBoxField;

        private string providerNameField;

        private string providerNoField;

        private string pseudoBusName2Field;

        private string pseudoBusinessNameField;

        private string recLocdField;

        private string recSecurityField;

        private string resLicenseStatusField;

        private string salutationField;

        private string selfInsField;

        private string serviceProviderCodeField;

        private string socialSecurityNumberField;

        private string stateField;

        private string stateLicenseField;

        private string statusGroupCodeField;

        private string suffixNameField;

        private TemplateModel templateField;

        private string titleField;

        private string typeFlagField;

        private System.Nullable<System.DateTime> wcCancDateField;

        private System.Nullable<System.DateTime> wcEffDateField;

        private string wcExemptField;

        private System.Nullable<System.DateTime> wcExpDateField;

        private string wcInsCoCodeField;

        private System.Nullable<System.DateTime> wcIntentToSuspNtcSentDateField;

        private string wcPolicyNoField;

        private System.Nullable<System.DateTime> wcSuspendDateField;

        private string zipField;

        private string resLicenseTypeField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string acaPermission
        {
            get
            {
                return this.acaPermissionField;
            }
            set
            {
                this.acaPermissionField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string address1
        {
            get
            {
                return this.address1Field;
            }
            set
            {
                this.address1Field = value;
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
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string address3
        {
            get
            {
                return this.address3Field;
            }
            set
            {
                this.address3Field = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string agencyCode
        {
            get
            {
                return this.agencyCodeField;
            }
            set
            {
                this.agencyCodeField = value;
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
        public bool bizLicExpired
        {
            get
            {
                return this.bizLicExpiredField;
            }
            set
            {
                this.bizLicExpiredField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string busName2
        {
            get
            {
                return this.busName2Field;
            }
            set
            {
                this.busName2Field = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = true)]
        public System.Nullable<System.DateTime> businessLicExpDate
        {
            get
            {
                return this.businessLicExpDateField;
            }
            set
            {
                this.businessLicExpDateField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string businessLicense
        {
            get
            {
                return this.businessLicenseField;
            }
            set
            {
                this.businessLicenseField = value;
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
        public string cityCode
        {
            get
            {
                return this.cityCodeField;
            }
            set
            {
                this.cityCodeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string comment
        {
            get
            {
                return this.commentField;
            }
            set
            {
                this.commentField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string contLicBusName
        {
            get
            {
                return this.contLicBusNameField;
            }
            set
            {
                this.contLicBusNameField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string contactFirstName
        {
            get
            {
                return this.contactFirstNameField;
            }
            set
            {
                this.contactFirstNameField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string contactLastName
        {
            get
            {
                return this.contactLastNameField;
            }
            set
            {
                this.contactLastNameField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string contactMiddleName
        {
            get
            {
                return this.contactMiddleNameField;
            }
            set
            {
                this.contactMiddleNameField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = true)]
        public System.Nullable<long> contrLicNo
        {
            get
            {
                return this.contrLicNoField;
            }
            set
            {
                this.contrLicNoField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string contryCode
        {
            get
            {
                return this.contryCodeField;
            }
            set
            {
                this.contryCodeField = value;
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
        public string countryCode
        {
            get
            {
                return this.countryCodeField;
            }
            set
            {
                this.countryCodeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string degree
        {
            get
            {
                return this.degreeField;
            }
            set
            {
                this.degreeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string discipline
        {
            get
            {
                return this.disciplineField;
            }
            set
            {
                this.disciplineField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string EMailAddress
        {
            get
            {
                return this.eMailAddressField;
            }
            set
            {
                this.eMailAddressField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string einSs
        {
            get
            {
                return this.einSsField;
            }
            set
            {
                this.einSsField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = true)]
        public System.Nullable<System.DateTime> endbirthDate
        {
            get
            {
                return this.endbirthDateField;
            }
            set
            {
                this.endbirthDateField = value;
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
        public string holdCode
        {
            get
            {
                return this.holdCodeField;
            }
            set
            {
                this.holdCodeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string holdDesc
        {
            get
            {
                return this.holdDescField;
            }
            set
            {
                this.holdDescField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public bool insExpired
        {
            get
            {
                return this.insExpiredField;
            }
            set
            {
                this.insExpiredField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = true)]
        public System.Nullable<double> insuranceAmount
        {
            get
            {
                return this.insuranceAmountField;
            }
            set
            {
                this.insuranceAmountField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string insuranceCo
        {
            get
            {
                return this.insuranceCoField;
            }
            set
            {
                this.insuranceCoField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = true)]
        public System.Nullable<System.DateTime> insuranceExpDate
        {
            get
            {
                return this.insuranceExpDateField;
            }
            set
            {
                this.insuranceExpDateField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public bool isTNExpired
        {
            get
            {
                return this.isTNExpiredField;
            }
            set
            {
                this.isTNExpiredField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = true)]
        public System.Nullable<long> ivrPinNumber
        {
            get
            {
                return this.ivrPinNumberField;
            }
            set
            {
                this.ivrPinNumberField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public bool licExpired
        {
            get
            {
                return this.licExpiredField;
            }
            set
            {
                this.licExpiredField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = true)]
        public System.Nullable<System.DateTime> licOrigIssDate
        {
            get
            {
                return this.licOrigIssDateField;
            }
            set
            {
                this.licOrigIssDateField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = true)]
        public System.Nullable<long> licSeqNbr
        {
            get
            {
                return this.licSeqNbrField;
            }
            set
            {
                this.licSeqNbrField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string licState
        {
            get
            {
                return this.licStateField;
            }
            set
            {
                this.licStateField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public RecordTypeLicTypePermissionModel licTypePermission
        {
            get
            {
                return this.licTypePermissionField;
            }
            set
            {
                this.licTypePermissionField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string licenseBoard
        {
            get
            {
                return this.licenseBoardField;
            }
            set
            {
                this.licenseBoardField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = true)]
        public System.Nullable<System.DateTime> licenseExpirationDate
        {
            get
            {
                return this.licenseExpirationDateField;
            }
            set
            {
                this.licenseExpirationDateField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = true)]
        public System.Nullable<System.DateTime> licenseIssueDate
        {
            get
            {
                return this.licenseIssueDateField;
            }
            set
            {
                this.licenseIssueDateField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = true)]
        public System.Nullable<System.DateTime> licenseLastRenewalDate
        {
            get
            {
                return this.licenseLastRenewalDateField;
            }
            set
            {
                this.licenseLastRenewalDateField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string licenseStatus
        {
            get
            {
                return this.licenseStatusField;
            }
            set
            {
                this.licenseStatusField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string licenseType
        {
            get
            {
                return this.licenseTypeField;
            }
            set
            {
                this.licenseTypeField = value;
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
        public string permStatusCode
        {
            get
            {
                return this.permStatusCodeField;
            }
            set
            {
                this.permStatusCodeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string phone1
        {
            get
            {
                return this.phone1Field;
            }
            set
            {
                this.phone1Field = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string phone1CountryCode
        {
            get
            {
                return this.phone1CountryCodeField;
            }
            set
            {
                this.phone1CountryCodeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string phone2
        {
            get
            {
                return this.phone2Field;
            }
            set
            {
                this.phone2Field = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string phone2CountryCode
        {
            get
            {
                return this.phone2CountryCodeField;
            }
            set
            {
                this.phone2CountryCodeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string phone3
        {
            get
            {
                return this.phone3Field;
            }
            set
            {
                this.phone3Field = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string phone3CountryCode
        {
            get
            {
                return this.phone3CountryCodeField;
            }
            set
            {
                this.phone3CountryCodeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string policy
        {
            get
            {
                return this.policyField;
            }
            set
            {
                this.policyField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string postOfficeBox
        {
            get
            {
                return this.postOfficeBoxField;
            }
            set
            {
                this.postOfficeBoxField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string providerName
        {
            get
            {
                return this.providerNameField;
            }
            set
            {
                this.providerNameField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string providerNo
        {
            get
            {
                return this.providerNoField;
            }
            set
            {
                this.providerNoField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string pseudoBusName2
        {
            get
            {
                return this.pseudoBusName2Field;
            }
            set
            {
                this.pseudoBusName2Field = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string pseudoBusinessName
        {
            get
            {
                return this.pseudoBusinessNameField;
            }
            set
            {
                this.pseudoBusinessNameField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string recLocd
        {
            get
            {
                return this.recLocdField;
            }
            set
            {
                this.recLocdField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string recSecurity
        {
            get
            {
                return this.recSecurityField;
            }
            set
            {
                this.recSecurityField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string resLicenseStatus
        {
            get
            {
                return this.resLicenseStatusField;
            }
            set
            {
                this.resLicenseStatusField = value;
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
        public string selfIns
        {
            get
            {
                return this.selfInsField;
            }
            set
            {
                this.selfInsField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string serviceProviderCode
        {
            get
            {
                return this.serviceProviderCodeField;
            }
            set
            {
                this.serviceProviderCodeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string socialSecurityNumber
        {
            get
            {
                return this.socialSecurityNumberField;
            }
            set
            {
                this.socialSecurityNumberField = value;
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
        public string stateLicense
        {
            get
            {
                return this.stateLicenseField;
            }
            set
            {
                this.stateLicenseField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string statusGroupCode
        {
            get
            {
                return this.statusGroupCodeField;
            }
            set
            {
                this.statusGroupCodeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string suffixName
        {
            get
            {
                return this.suffixNameField;
            }
            set
            {
                this.suffixNameField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public TemplateModel template
        {
            get
            {
                return this.templateField;
            }
            set
            {
                this.templateField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string title
        {
            get
            {
                return this.titleField;
            }
            set
            {
                this.titleField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string typeFlag
        {
            get
            {
                return this.typeFlagField;
            }
            set
            {
                this.typeFlagField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = true)]
        public System.Nullable<System.DateTime> wcCancDate
        {
            get
            {
                return this.wcCancDateField;
            }
            set
            {
                this.wcCancDateField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = true)]
        public System.Nullable<System.DateTime> wcEffDate
        {
            get
            {
                return this.wcEffDateField;
            }
            set
            {
                this.wcEffDateField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string wcExempt
        {
            get
            {
                return this.wcExemptField;
            }
            set
            {
                this.wcExemptField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = true)]
        public System.Nullable<System.DateTime> wcExpDate
        {
            get
            {
                return this.wcExpDateField;
            }
            set
            {
                this.wcExpDateField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string wcInsCoCode
        {
            get
            {
                return this.wcInsCoCodeField;
            }
            set
            {
                this.wcInsCoCodeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = true)]
        public System.Nullable<System.DateTime> wcIntentToSuspNtcSentDate
        {
            get
            {
                return this.wcIntentToSuspNtcSentDateField;
            }
            set
            {
                this.wcIntentToSuspNtcSentDateField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string wcPolicyNo
        {
            get
            {
                return this.wcPolicyNoField;
            }
            set
            {
                this.wcPolicyNoField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = true)]
        public System.Nullable<System.DateTime> wcSuspendDate
        {
            get
            {
                return this.wcSuspendDateField;
            }
            set
            {
                this.wcSuspendDateField = value;
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
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string resLicenseType
        {
            get
            {
                return this.resLicenseTypeField;
            }
            set
            {
                this.resLicenseTypeField = value;
            }
        }        
    }
}
