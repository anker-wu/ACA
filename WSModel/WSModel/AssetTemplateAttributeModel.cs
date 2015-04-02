#region Header

/**
 *  Accela Citizen Access
 *  File: templateAttributeModel.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2013
 *
 *  Description:
 *  
 *
 *  Notes:
 * $Id: templateAttributeModel.cs 134054 2013-10-16 18:03:54Z ACHIEVO\eric.he $.
 *  Revision History
 *  Date,                  Who,                 What
 */

#endregion Header

namespace Accela.ACA.WSProxy
{
    /// <remarks/>
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(AttrTableAttributeModel))]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.18060")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://service.webservice.accela.com/")]
    public partial class templateAttributeModel : LanguageModel
    {

        private int displayLengthField;

        private string displayStatusField;

        private string fieldDisplayLabelField;

        private int fieldTypeField;

        private string r1AssetTemplateIdField;

        private string r1AttributeDescField;

        private string r1AttributeGroupField;

        private string r1AttributeLabelField;

        private long r1AttributeLengthField;

        private bool r1AttributeLengthFieldSpecified;

        private string r1AttributeNameField;

        private string r1AttributeUnitTypeField;

        private string r1AttributeValueField;

        private string r1AttributeValueReqFlagField;

        private string r1AttributeValueTypeField;

        private long r1DisplayOrderField;

        private bool r1DisplayOrderFieldSpecified;

        private string r1IsDropdownListField;

        private string r1SearchableFlagField;

        private string rangeSearchFlagField;

        private System.DateTime recDateField;

        private bool recDateFieldSpecified;

        private string recFulNamField;

        private string recStatusField;

        private string screenColorField;

        private int screenDisplayHeightField;

        private int screenDisplayWidthField;

        private string screenFontField;

        private string screenJustificationField;

        private string servProvCodeField;

        private string sharedDDListNameField;

        private string watermarkLabelField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public int displayLength
        {
            get
            {
                return this.displayLengthField;
            }
            set
            {
                this.displayLengthField = value;
            }
        }

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
        public string fieldDisplayLabel
        {
            get
            {
                return this.fieldDisplayLabelField;
            }
            set
            {
                this.fieldDisplayLabelField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public int fieldType
        {
            get
            {
                return this.fieldTypeField;
            }
            set
            {
                this.fieldTypeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string r1AssetTemplateId
        {
            get
            {
                return this.r1AssetTemplateIdField;
            }
            set
            {
                this.r1AssetTemplateIdField = value;
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
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public long r1AttributeLength
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
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool r1AttributeLengthSpecified
        {
            get
            {
                return this.r1AttributeLengthFieldSpecified;
            }
            set
            {
                this.r1AttributeLengthFieldSpecified = value;
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
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public long r1DisplayOrder
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
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool r1DisplayOrderSpecified
        {
            get
            {
                return this.r1DisplayOrderFieldSpecified;
            }
            set
            {
                this.r1DisplayOrderFieldSpecified = value;
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
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public System.DateTime recDate
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
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool recDateSpecified
        {
            get
            {
                return this.recDateFieldSpecified;
            }
            set
            {
                this.recDateFieldSpecified = value;
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
        public string screenColor
        {
            get
            {
                return this.screenColorField;
            }
            set
            {
                this.screenColorField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public int screenDisplayHeight
        {
            get
            {
                return this.screenDisplayHeightField;
            }
            set
            {
                this.screenDisplayHeightField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public int screenDisplayWidth
        {
            get
            {
                return this.screenDisplayWidthField;
            }
            set
            {
                this.screenDisplayWidthField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string screenFont
        {
            get
            {
                return this.screenFontField;
            }
            set
            {
                this.screenFontField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string screenJustification
        {
            get
            {
                return this.screenJustificationField;
            }
            set
            {
                this.screenJustificationField = value;
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

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string watermarkLabel
        {
            get
            {
                return this.watermarkLabelField;
            }
            set
            {
                this.watermarkLabelField = value;
            }
        }
    }
}