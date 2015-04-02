#region Header

/**
 *  Accela Citizen Access
 *  File: AttributeModel.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2013
 *
 *  Description:
 *  
 *
 *  Notes:
 * $Id: AttributeModel.cs 134054 2013-10-16 18:03:54Z ACHIEVO\eric.he $.
 *  Revision History
 *  Date,                  Who,                 What
 */

#endregion Header

namespace Accela.ACA.WSProxy
{
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.1015")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://model.webservice.accela.com")]
    public partial class AttributeModel : LanguageModel
    {

        private string displayStatusField;

        private string errorTipField;

        private string r1AttributeDescField;

        private string r1AttributeGroupField;

        private string r1AttributeLabelField;

        private System.Nullable<long> r1AttributeLengthField;

        private string r1AttributeNameField;

        private string r1AttributeUnitTypeField;

        private string r1AttributeValueField;

        private string r1AttributeValueReqFlagField;

        private string r1AttributeValueTypeField;

        private System.Nullable<long> r1DisplayOrderField;

        private string r1IsDropdownListField;

        private string r1SearchableFlagField;

        private string rangeSearchFlagField;

        private System.Nullable<System.DateTime> recDateField;

        private string recFulNamField;

        private string recStatusField;

        private string servProvCodeField;

        private string sharedDDListNameField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string displayStatus
        {
            get
            {
                return this.displayStatusField;
            }
            set
            {
                this.displayStatusField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string errorTip
        {
            get
            {
                return this.errorTipField;
            }
            set
            {
                this.errorTipField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string r1AttributeDesc
        {
            get
            {
                return this.r1AttributeDescField;
            }
            set
            {
                this.r1AttributeDescField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string r1AttributeGroup
        {
            get
            {
                return this.r1AttributeGroupField;
            }
            set
            {
                this.r1AttributeGroupField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string r1AttributeLabel
        {
            get
            {
                return this.r1AttributeLabelField;
            }
            set
            {
                this.r1AttributeLabelField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = true)]
        public System.Nullable<long> r1AttributeLength
        {
            get
            {
                return this.r1AttributeLengthField;
            }
            set
            {
                this.r1AttributeLengthField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string r1AttributeName
        {
            get
            {
                return this.r1AttributeNameField;
            }
            set
            {
                this.r1AttributeNameField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string r1AttributeUnitType
        {
            get
            {
                return this.r1AttributeUnitTypeField;
            }
            set
            {
                this.r1AttributeUnitTypeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string r1AttributeValue
        {
            get
            {
                return this.r1AttributeValueField;
            }
            set
            {
                this.r1AttributeValueField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string r1AttributeValueReqFlag
        {
            get
            {
                return this.r1AttributeValueReqFlagField;
            }
            set
            {
                this.r1AttributeValueReqFlagField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string r1AttributeValueType
        {
            get
            {
                return this.r1AttributeValueTypeField;
            }
            set
            {
                this.r1AttributeValueTypeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = true)]
        public System.Nullable<long> r1DisplayOrder
        {
            get
            {
                return this.r1DisplayOrderField;
            }
            set
            {
                this.r1DisplayOrderField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string r1IsDropdownList
        {
            get
            {
                return this.r1IsDropdownListField;
            }
            set
            {
                this.r1IsDropdownListField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string r1SearchableFlag
        {
            get
            {
                return this.r1SearchableFlagField;
            }
            set
            {
                this.r1SearchableFlagField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string rangeSearchFlag
        {
            get
            {
                return this.rangeSearchFlagField;
            }
            set
            {
                this.rangeSearchFlagField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = true)]
        public System.Nullable<System.DateTime> recDate
        {
            get
            {
                return this.recDateField;
            }
            set
            {
                this.recDateField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string recFulNam
        {
            get
            {
                return this.recFulNamField;
            }
            set
            {
                this.recFulNamField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string recStatus
        {
            get
            {
                return this.recStatusField;
            }
            set
            {
                this.recStatusField = value;
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
        public string sharedDDListName
        {
            get
            {
                return this.sharedDDListNameField;
            }
            set
            {
                this.sharedDDListNameField = value;
            }
        }
    }
}