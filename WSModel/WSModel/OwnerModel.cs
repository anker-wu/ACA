/**
 * <pre>
 * 
 *  Accela Citizen Access
 *  File: OwnerModel.cs
 * 
 *  Accela, Inc.
 *  Copyright (C): 2010-2014
 * 
 *  Description:
 * 
 *  Notes:
 * $Id: OwnerModel.cs 130107 2009-05-11 12:23:56Z ACHIEVO\jackie.yu $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

namespace Accela.ACA.WSProxy
{

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.1433")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://model.webservice.accela.com")]
    public partial class OwnerModel : LanguageModel
    {

        private string addressField;

        private string address1Field;

        private string address2Field;

        private string address3Field;

        private System.Nullable<System.DateTime> auditDateField;

        private string auditIDField;

        private string auditStatusField;

        private string cityField;

        private string countryField;

        private DuplicatedAPOKeyModel[] duplicatedAPOKeysField;

        private string emailField;

        private System.Nullable<long> eventIDField;

        private string faxField;

        private string faxCountryCodeField;

        private NoticeConditionModel hightestConditionField;

        private string isPrimaryField;

        private string ivrPinNumberField;

        private string ivrUserNumberField;

        private string mailAddress1Field;

        private string mailAddress2Field;

        private string mailAddress3Field;

        private string mailCityField;

        private string mailCountryField;

        private string mailStateField;

        private string mailZipField;

        private string mappingDailyOwnerNbrField;

        private NoticeConditionModel[] noticeConditionsField;

        private string ownerFirstNameField;

        private string ownerFullNameField;

        private string ownerLastNameField;

        private string ownerMiddleNameField;

        private System.Nullable<long> ownerNumberField;

        private string ownerStatusField;

        private string ownerTitleField;

        private ParcelInfoModel[] parcelListsField;

        private string phoneField;

        private string phoneCountryCodeField;

        private string primaryField;

        private string resCountryField;

        private System.Nullable<long> sourceSeqNumberField;

        private string stateField;

        private string taxIDField;

        private TemplateAttributeModel[] templatesField;

        private string uIDField;

        private string zipField;

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
        [System.Xml.Serialization.XmlArrayAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        [System.Xml.Serialization.XmlArrayItemAttribute("duplicatedAPOKey", Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = false)]
        public DuplicatedAPOKeyModel[] duplicatedAPOKeys
        {
            get
            {
                return this.duplicatedAPOKeysField;
            }
            set
            {
                this.duplicatedAPOKeysField = value;
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
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = true)]
        public System.Nullable<long> eventID
        {
            get
            {
                return this.eventIDField;
            }
            set
            {
                this.eventIDField = value;
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
        public string isPrimary
        {
            get
            {
                return this.isPrimaryField;
            }
            set
            {
                this.isPrimaryField = value;
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
        public string mailAddress1
        {
            get
            {
                return this.mailAddress1Field;
            }
            set
            {
                this.mailAddress1Field = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string mailAddress2
        {
            get
            {
                return this.mailAddress2Field;
            }
            set
            {
                this.mailAddress2Field = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string mailAddress3
        {
            get
            {
                return this.mailAddress3Field;
            }
            set
            {
                this.mailAddress3Field = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string mailCity
        {
            get
            {
                return this.mailCityField;
            }
            set
            {
                this.mailCityField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string mailCountry
        {
            get
            {
                return this.mailCountryField;
            }
            set
            {
                this.mailCountryField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string mailState
        {
            get
            {
                return this.mailStateField;
            }
            set
            {
                this.mailStateField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string mailZip
        {
            get
            {
                return this.mailZipField;
            }
            set
            {
                this.mailZipField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string mappingDailyOwnerNbr
        {
            get
            {
                return this.mappingDailyOwnerNbrField;
            }
            set
            {
                this.mappingDailyOwnerNbrField = value;
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
        public string ownerFirstName
        {
            get
            {
                return this.ownerFirstNameField;
            }
            set
            {
                this.ownerFirstNameField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string ownerFullName
        {
            get
            {
                return this.ownerFullNameField;
            }
            set
            {
                this.ownerFullNameField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string ownerLastName
        {
            get
            {
                return this.ownerLastNameField;
            }
            set
            {
                this.ownerLastNameField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string ownerMiddleName
        {
            get
            {
                return this.ownerMiddleNameField;
            }
            set
            {
                this.ownerMiddleNameField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = true)]
        public System.Nullable<long> ownerNumber
        {
            get
            {
                return this.ownerNumberField;
            }
            set
            {
                this.ownerNumberField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string ownerStatus
        {
            get
            {
                return this.ownerStatusField;
            }
            set
            {
                this.ownerStatusField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string ownerTitle
        {
            get
            {
                return this.ownerTitleField;
            }
            set
            {
                this.ownerTitleField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        [System.Xml.Serialization.XmlArrayItemAttribute("parcelList", Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = false)]
        public ParcelInfoModel[] parcelLists
        {
            get
            {
                return this.parcelListsField;
            }
            set
            {
                this.parcelListsField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string phone
        {
            get
            {
                return this.phoneField;
            }
            set
            {
                this.phoneField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string phoneCountryCode
        {
            get
            {
                return this.phoneCountryCodeField;
            }
            set
            {
                this.phoneCountryCodeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string primary
        {
            get
            {
                return this.primaryField;
            }
            set
            {
                this.primaryField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string resCountry
        {
            get
            {
                return this.resCountryField;
            }
            set
            {
                this.resCountryField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = true)]
        public System.Nullable<long> sourceSeqNumber
        {
            get
            {
                return this.sourceSeqNumberField;
            }
            set
            {
                this.sourceSeqNumberField = value;
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
        public string taxID
        {
            get
            {
                return this.taxIDField;
            }
            set
            {
                this.taxIDField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        [System.Xml.Serialization.XmlArrayItemAttribute("template", Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = false)]
        public TemplateAttributeModel[] templates
        {
            get
            {
                return this.templatesField;
            }
            set
            {
                this.templatesField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string UID
        {
            get
            {
                return this.uIDField;
            }
            set
            {
                this.uIDField = value;
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
    }
}
