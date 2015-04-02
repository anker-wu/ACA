#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: AttributeValueModel.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2013
 *
 *  Description:
 *
 *  Notes:
 *      $Id: AttributeValueModel.cs 144714 2009-08-25 12:44:16Z ACHIEVO\jackie.yu $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

namespace Accela.ACA.WSProxy
{
    /// <remarks/>
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(AssetAttrTableValueModel))]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.18060")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://service.webservice.accela.com/")]
    public partial class AttributeValueModel : LanguageModel
    {

        private string dispR1AttributeValueField;

        private string r1AttributeNameField;

        private string r1AttributeValueField;

        private string r1AttributeValueTypeField;

        private System.DateTime recDateField;

        private bool recDateFieldSpecified;

        private string recFulNamField;

        private string recStatusField;

        private string resR1AttributeValueField;

        private string servProvCodeField;

        private double weightingFactorField;

        private bool weightingFactorFieldSpecified;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string dispR1AttributeValue
        {
            get
            {
                return this.dispR1AttributeValueField;
            }
            set
            {
                this.dispR1AttributeValueField = value;
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
        public string resR1AttributeValue
        {
            get
            {
                return this.resR1AttributeValueField;
            }
            set
            {
                this.resR1AttributeValueField = value;
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
        public double weightingFactor
        {
            get
            {
                return this.weightingFactorField;
            }
            set
            {
                this.weightingFactorField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool weightingFactorSpecified
        {
            get
            {
                return this.weightingFactorFieldSpecified;
            }
            set
            {
                this.weightingFactorFieldSpecified = value;
            }
        }
    }
}