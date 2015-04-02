#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: DataAttributeModel.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2013
 *
 *  Description:
 *
 *  Notes:
 *      $Id: DataAttributeModel.cs 144714 2009-08-25 12:44:16Z ACHIEVO\jackie.yu $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
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
    public partial class DataAttributeModel : LanguageModel
    {

        private System.Nullable<long> g1AssetSequenceNumberField;

        private string g1AttributeNameField;

        private string g1AttributeValueField;

        private string g1AttributeValueDateTypeField;

        private string g1EndAttributeValueField;

        private string r1AttributeValueTypeField;

        private string r1IsDropdownListField;

        private System.Nullable<System.DateTime> recDateField;

        private string recFulNamField;

        private string recStatusField;

        private string resG1AttributeValueField;

        private string searchByRangeField;

        private string serviceProviderCodeField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = true)]
        public System.Nullable<long> g1AssetSequenceNumber
        {
            get
            {
                return this.g1AssetSequenceNumberField;
            }
            set
            {
                this.g1AssetSequenceNumberField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string g1AttributeName
        {
            get
            {
                return this.g1AttributeNameField;
            }
            set
            {
                this.g1AttributeNameField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string g1AttributeValue
        {
            get
            {
                return this.g1AttributeValueField;
            }
            set
            {
                this.g1AttributeValueField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string g1AttributeValueDateType
        {
            get
            {
                return this.g1AttributeValueDateTypeField;
            }
            set
            {
                this.g1AttributeValueDateTypeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string g1EndAttributeValue
        {
            get
            {
                return this.g1EndAttributeValueField;
            }
            set
            {
                this.g1EndAttributeValueField = value;
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
        public string resG1AttributeValue
        {
            get
            {
                return this.resG1AttributeValueField;
            }
            set
            {
                this.resG1AttributeValueField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string searchByRange
        {
            get
            {
                return this.searchByRangeField;
            }
            set
            {
                this.searchByRangeField = value;
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
    }
}