/**
 * <pre>
 * 
 *  Accela Citizen Access
 *  File: PeopleAttributeModel.cs
 * 
 *  Accela, Inc.
 *  Copyright (C): 2014
 * 
 *  Description:
 * 
 *  Notes:
 * $Id: PeopleAttributeModel.cs 170671 2014-06-03 05:52:20Z ACHIEVO\joy.duan $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

namespace Accela.ACA.WSProxy
{
    /// <remarks/>
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(ContactAttributeModel))]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.18408")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://service.webservice.accela.com/")]
    public partial class PeopleAttributeModel : LanguageModel
    {
        private string attributeLabelField;

        private string attributeNameField;

        private string attributeTemplateNameField;

        private string attributeUnitTypeField;

        private string attributeValueField;

        private string attributeValueDataTypeField;

        private string attributeValueReqFlagField;

        private System.DateTime auditDateField;

        private bool auditDateFieldSpecified;

        private string auditIDField;

        private string auditStatusField;

        private string contactNoField;

        private string contactTypeField;

        private string dispInTableFlagField;

        private long displayOrderField;

        private string serviceProviderCodeField;

        private string validationScriptField;

        private string vchFlagField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string attributeLabel
        {
            get
            {
                return this.attributeLabelField;
            }
            set
            {
                this.attributeLabelField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string attributeName
        {
            get
            {
                return this.attributeNameField;
            }
            set
            {
                this.attributeNameField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string attributeTemplateName
        {
            get
            {
                return this.attributeTemplateNameField;
            }
            set
            {
                this.attributeTemplateNameField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string attributeUnitType
        {
            get
            {
                return this.attributeUnitTypeField;
            }
            set
            {
                this.attributeUnitTypeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string attributeValue
        {
            get
            {
                return this.attributeValueField;
            }
            set
            {
                this.attributeValueField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string attributeValueDataType
        {
            get
            {
                return this.attributeValueDataTypeField;
            }
            set
            {
                this.attributeValueDataTypeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string attributeValueReqFlag
        {
            get
            {
                return this.attributeValueReqFlagField;
            }
            set
            {
                this.attributeValueReqFlagField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public System.DateTime auditDate
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
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool auditDateSpecified
        {
            get
            {
                return this.auditDateFieldSpecified;
            }
            set
            {
                this.auditDateFieldSpecified = value;
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
        public string contactNo
        {
            get
            {
                return this.contactNoField;
            }
            set
            {
                this.contactNoField = value;
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
        public string dispInTableFlag
        {
            get
            {
                return this.dispInTableFlagField;
            }
            set
            {
                this.dispInTableFlagField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public long displayOrder
        {
            get
            {
                return this.displayOrderField;
            }
            set
            {
                this.displayOrderField = value;
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
        public string validationScript
        {
            get
            {
                return this.validationScriptField;
            }
            set
            {
                this.validationScriptField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string vchFlag
        {
            get
            {
                return this.vchFlagField;
            }
            set
            {
                this.vchFlagField = value;
            }
        }
    }
}