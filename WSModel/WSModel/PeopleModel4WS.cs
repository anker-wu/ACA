/**
 * <pre>
 * 
 *  Accela Citizen Access
 *  File: PeopleModel4WS.cs
 * 
 *  Accela, Inc.
 *  Copyright (C): 2007-2014
 * 
 *  Description:
 * 
 *  Notes:
 * $Id: PeopleModel4WS.cs 274085 2014-06-25 07:27:24Z ACHIEVO\james.shi $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

namespace Accela.ACA.WSProxy
{
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.225")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://model.webservice.accela.com")]
    public partial class PeopleModel4WS
    {
        private string agencyAliasNameField;

        private TemplateAttributeModel[] attributesField;

        private string auditDateField;

        private string auditIDField;

        private string auditStatusField;

        private string birthCityField;

        private string birthDateField;

        private string birthRegionField;

        private string birthStateField;

        private string businessNameField;

        private string businessName2Field;

        private string commentField;

        private CompactAddressModel4WS compactAddressField;

        private ContactAddressModel[] contactAddressListField;

        private string contactSeqNumberField;

        private string contactTypeField;

        private string contactTypeFlagField;

        private string countryField;

        private string countryCodeField;

        private string deceasedDateField;

        private string driverLicenseNbrField;

        private string driverLicenseStateField;

        private string emailField;

        private string endBirthDateField;

        private string endDateField;

        private string endDeceasedDateField;

        private string faxField;

        private string faxCountryCodeField;

        private string feinField;

        private string firstNameField;

        private string flagField;

        private string fullNameField;

        private string genderField;

        private NoticeConditionModel hightestConditionField;

        private string holdCodeField;

        private string holdDescriptionField;

        private string idField;

        private string ivrPinNumberField;

        private string ivrUserNumberField;

        private string lastNameField;

        private string maskedSsnField;

        private string middleNameField;

        private string namesuffixField;

        private NoticeConditionModel[] noticeConditionsField;

        private string passportNumberField;

        private PeopleAKAModel[] peopleAKAListField;

        private string phone1Field;

        private string phone1CountryCodeField;

        private string phone2Field;

        private string phone2CountryCodeField;

        private string phone3Field;

        private string phone3CountryCodeField;

        private string postOfficeBoxField;

        private string preferredChannelField;

        private string preferredChannelStringField;

        private string contractorPeopleStatusField;

        private string raceField;

        private string relationField;

        private string resContactTypeField;

        private string salutationField;

        private string serviceProviderCodeField;

        private string socialSecurityNumberField;

        private string startDateField;

        private string stateIDNbrField;

        private TemplateModel templateField;

        private string titleField;

        private string tradeNameField;

        private string accountOwnerField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string agencyAliasName
        {
            get
            {
                return this.agencyAliasNameField;
            }
            set
            {
                this.agencyAliasNameField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("attributes", Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = true)]
        public TemplateAttributeModel[] attributes
        {
            get
            {
                return this.attributesField;
            }
            set
            {
                this.attributesField = value;
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
        public string birthCity
        {
            get
            {
                return this.birthCityField;
            }
            set
            {
                this.birthCityField = value;
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
        public string birthRegion
        {
            get
            {
                return this.birthRegionField;
            }
            set
            {
                this.birthRegionField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string birthState
        {
            get
            {
                return this.birthStateField;
            }
            set
            {
                this.birthStateField = value;
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
        public string businessName2
        {
            get
            {
                return this.businessName2Field;
            }
            set
            {
                this.businessName2Field = value;
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
        public CompactAddressModel4WS compactAddress
        {
            get
            {
                return this.compactAddressField;
            }
            set
            {
                this.compactAddressField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("contactAddressList", Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = true)]
        public ContactAddressModel[] contactAddressList
        {
            get
            {
                return this.contactAddressListField;
            }
            set
            {
                this.contactAddressListField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string contactSeqNumber
        {
            get
            {
                return this.contactSeqNumberField;
            }
            set
            {
                this.contactSeqNumberField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string contactType
        {
            get
            {
                return this.contactTypeField;
            }
            set
            {
                this.contactTypeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string contactTypeFlag
        {
            get
            {
                return this.contactTypeFlagField;
            }
            set
            {
                this.contactTypeFlagField = value;
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
        public string deceasedDate
        {
            get
            {
                return this.deceasedDateField;
            }
            set
            {
                this.deceasedDateField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string driverLicenseNbr
        {
            get
            {
                return this.driverLicenseNbrField;
            }
            set
            {
                this.driverLicenseNbrField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string driverLicenseState
        {
            get
            {
                return this.driverLicenseStateField;
            }
            set
            {
                this.driverLicenseStateField = value;
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
        public string endBirthDate
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
        public string endDate
        {
            get
            {
                return this.endDateField;
            }
            set
            {
                this.endDateField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string endDeceasedDate
        {
            get
            {
                return this.endDeceasedDateField;
            }
            set
            {
                this.endDeceasedDateField = value;
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
        public string flag
        {
            get
            {
                return this.flagField;
            }
            set
            {
                this.flagField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string fullName
        {
            get
            {
                return this.fullNameField;
            }
            set
            {
                this.fullNameField = value;
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
        public NoticeConditionModel hightestCondition
        {
            get
            {
                return this.hightestConditionField;
            }
            set
            {
                this.hightestConditionField = value;
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
        public string holdDescription
        {
            get
            {
                return this.holdDescriptionField;
            }
            set
            {
                this.holdDescriptionField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string id
        {
            get
            {
                return this.idField;
            }
            set
            {
                this.idField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string ivrPinNumber
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
        public string ivrUserNumber
        {
            get
            {
                return this.ivrUserNumberField;
            }
            set
            {
                this.ivrUserNumberField = value;
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
        public string namesuffix
        {
            get
            {
                return this.namesuffixField;
            }
            set
            {
                this.namesuffixField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("noticeConditions", Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = true)]
        public NoticeConditionModel[] noticeConditions
        {
            get
            {
                return this.noticeConditionsField;
            }
            set
            {
                this.noticeConditionsField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string passportNumber
        {
            get
            {
                return this.passportNumberField;
            }
            set
            {
                this.passportNumberField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("peopleAKAList", Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = true)]
        public PeopleAKAModel[] peopleAKAList
        {
            get
            {
                return this.peopleAKAListField;
            }
            set
            {
                this.peopleAKAListField = value;
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
        public string preferredChannel
        {
            get
            {
                return this.preferredChannelField;
            }
            set
            {
                this.preferredChannelField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string preferredChannelString
        {
            get
            {
                return this.preferredChannelStringField;
            }
            set
            {
                this.preferredChannelStringField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string contractorPeopleStatus
        {
            get
            {
                return this.contractorPeopleStatusField;
            }
            set
            {
                this.contractorPeopleStatusField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string race
        {
            get
            {
                return this.raceField;
            }
            set
            {
                this.raceField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string relation
        {
            get
            {
                return this.relationField;
            }
            set
            {
                this.relationField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string resContactType
        {
            get
            {
                return this.resContactTypeField;
            }
            set
            {
                this.resContactTypeField = value;
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
        public string startDate
        {
            get
            {
                return this.startDateField;
            }
            set
            {
                this.startDateField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string stateIDNbr
        {
            get
            {
                return this.stateIDNbrField;
            }
            set
            {
                this.stateIDNbrField = value;
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
        public string tradeName
        {
            get
            {
                return this.tradeNameField;
            }
            set
            {
                this.tradeNameField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string accountOwner
        {
            get
            {
                return this.accountOwnerField;
            }
            set
            {
                this.accountOwnerField = value;
            }
        }
    }
}