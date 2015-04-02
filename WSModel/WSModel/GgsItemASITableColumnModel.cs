#region Header
/**
 * <pre>
 * 
 *  Accela Citizen Access
 *  File: GGSItemASITableColumnModel.cs
 * 
 *  Accela, Inc.
 *  Copyright (C): 2010
 * 
 *  Description:
 * 
 *  Notes:
 * $Id: GGSItemASITableColumnModel.cs 181867 2010-10-06 08:06:18Z ACHIEVO\daly.zeng $.
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
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://model.webservice.accela.com")]
    public partial class GGSItemASITableColumnModel
    {

        private string attributeValueField;

        private string attributeValueReqFlagField;

        private System.DateTime auditDateField;

        private bool auditDateFieldSpecified;

        private string auditIDField;

        private string auditStatusField;

        private string columnNameField;

        private int columnTypeField;

        private bool columnTypeFieldSpecified;

        private int displayLengthField;

        private bool displayLengthFieldSpecified;

        private int displayOrderField;

        private bool displayOrderFieldSpecified;

        private RefAppSpecInfoDropDownModel[] dropDownValuesField;

        private int maxLengthField;

        private bool maxLengthFieldSpecified;

        private bool supervisorEditOnlyField;

        private bool supervisorEditOnlyFieldSpecified;

        private GGSItemASITableColumnModelEntry[] valueMapField;

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
        public string columnName
        {
            get
            {
                return this.columnNameField;
            }
            set
            {
                this.columnNameField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public int columnType
        {
            get
            {
                return this.columnTypeField;
            }
            set
            {
                this.columnTypeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool columnTypeSpecified
        {
            get
            {
                return this.columnTypeFieldSpecified;
            }
            set
            {
                this.columnTypeFieldSpecified = value;
            }
        }

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
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool displayLengthSpecified
        {
            get
            {
                return this.displayLengthFieldSpecified;
            }
            set
            {
                this.displayLengthFieldSpecified = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public int displayOrder
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
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool displayOrderSpecified
        {
            get
            {
                return this.displayOrderFieldSpecified;
            }
            set
            {
                this.displayOrderFieldSpecified = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("dropDownValues", Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = true)]
        public RefAppSpecInfoDropDownModel[] dropDownValues
        {
            get
            {
                return this.dropDownValuesField;
            }
            set
            {
                this.dropDownValuesField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public int maxLength
        {
            get
            {
                return this.maxLengthField;
            }
            set
            {
                this.maxLengthField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool maxLengthSpecified
        {
            get
            {
                return this.maxLengthFieldSpecified;
            }
            set
            {
                this.maxLengthFieldSpecified = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public bool supervisorEditOnly
        {
            get
            {
                return this.supervisorEditOnlyField;
            }
            set
            {
                this.supervisorEditOnlyField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool supervisorEditOnlySpecified
        {
            get
            {
                return this.supervisorEditOnlyFieldSpecified;
            }
            set
            {
                this.supervisorEditOnlyFieldSpecified = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        [System.Xml.Serialization.XmlArrayItemAttribute("entry", Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = false)]
        public GGSItemASITableColumnModelEntry[] valueMap
        {
            get
            {
                return this.valueMapField;
            }
            set
            {
                this.valueMapField = value;
            }
        }
    }
}
