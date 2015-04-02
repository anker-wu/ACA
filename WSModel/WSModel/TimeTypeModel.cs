#region Header
/**
 * <pre>
 * 
 *  Accela Citizen Access
 *  File: TimeTypeModel.cs
 * 
 *  Accela, Inc.
 *  Copyright (C): 2010
 * 
 *  Description:
 * 
 *  Notes:
 * $Id: TimeTypeModel.cs 181867 2010-10-06 08:06:18Z ACHIEVO\daly.zeng $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */
#endregion Header

namespace Accela.ACA.WSProxy
{
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.3053")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://service.webservice.accela.com/")]
    public partial class TimeTypeModel : LanguageModel
    {
        private string billableFlagField;

        private CapTypeModel capTypeModelField;

        private double defaultPctAdjField;

        private bool defaultPctAdjFieldSpecified;

        private double defaultRateField;

        private bool defaultRateFieldSpecified;

        private System.DateTime recDateField;

        private bool recDateFieldSpecified;

        private string recFulNameField;

        private string recStatusField;

        private string recordTypeField;

        private string resTimeTypeNameField;

        private string servProvCodeField;

        private TimeGroupModel[] timeGroupsField;

        private string timeTypeDescField;

        private string timeTypeNameField;

        private long timeTypeSeqField;

        private bool timeTypeSeqFieldSpecified;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string billableFlag
        {
            get
            {
                return this.billableFlagField;
            }
            set
            {
                this.billableFlagField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public CapTypeModel capTypeModel
        {
            get
            {
                return this.capTypeModelField;
            }
            set
            {
                this.capTypeModelField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public double defaultPctAdj
        {
            get
            {
                return this.defaultPctAdjField;
            }
            set
            {
                this.defaultPctAdjField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool defaultPctAdjSpecified
        {
            get
            {
                return this.defaultPctAdjFieldSpecified;
            }
            set
            {
                this.defaultPctAdjFieldSpecified = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public double defaultRate
        {
            get
            {
                return this.defaultRateField;
            }
            set
            {
                this.defaultRateField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool defaultRateSpecified
        {
            get
            {
                return this.defaultRateFieldSpecified;
            }
            set
            {
                this.defaultRateFieldSpecified = value;
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
        public string recFulName
        {
            get
            {
                return this.recFulNameField;
            }
            set
            {
                this.recFulNameField = value;
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
        public string recordType
        {
            get
            {
                return this.recordTypeField;
            }
            set
            {
                this.recordTypeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string resTimeTypeName
        {
            get
            {
                return this.resTimeTypeNameField;
            }
            set
            {
                this.resTimeTypeNameField = value;
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
        [System.Xml.Serialization.XmlElementAttribute("timeGroups", Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = true)]
        public TimeGroupModel[] timeGroups
        {
            get
            {
                return this.timeGroupsField;
            }
            set
            {
                this.timeGroupsField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string timeTypeDesc
        {
            get
            {
                return this.timeTypeDescField;
            }
            set
            {
                this.timeTypeDescField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string timeTypeName
        {
            get
            {
                return this.timeTypeNameField;
            }
            set
            {
                this.timeTypeNameField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public long timeTypeSeq
        {
            get
            {
                return this.timeTypeSeqField;
            }
            set
            {
                this.timeTypeSeqField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool timeTypeSeqSpecified
        {
            get
            {
                return this.timeTypeSeqFieldSpecified;
            }
            set
            {
                this.timeTypeSeqFieldSpecified = value;
            }
        }
    }
}
