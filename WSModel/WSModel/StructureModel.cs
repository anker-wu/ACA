#region Header
/**
 * <pre>
 * 
 *  Accela Citizen Access
 *  File: StructureModel.cs
 * 
 *  Accela, Inc.
 *  Copyright (C): 2010
 * 
 *  Description:
 * 
 *  Notes:
 * $Id: StructureModel.cs 181867 2010-10-06 08:06:18Z ACHIEVO\daly.zeng $.
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
    public partial class StructureModel
    {
        private string auditStatusField;

        private long sourceSeqNumberField;

        private string structureNameField;

        private long structureNumberField;

        private string structureStatusField;

        private System.DateTime structureStatusDateField;

        private bool structureStatusDateFieldSpecified;

        private System.DateTime structureStatusEndDateField;

        private bool structureStatusEndDateFieldSpecified;

        private string structureTypeField;

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
        public long sourceSeqNumber
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
        public string structureName
        {
            get
            {
                return this.structureNameField;
            }
            set
            {
                this.structureNameField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public long structureNumber
        {
            get
            {
                return this.structureNumberField;
            }
            set
            {
                this.structureNumberField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string structureStatus
        {
            get
            {
                return this.structureStatusField;
            }
            set
            {
                this.structureStatusField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public System.DateTime structureStatusDate
        {
            get
            {
                return this.structureStatusDateField;
            }
            set
            {
                this.structureStatusDateField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool structureStatusDateSpecified
        {
            get
            {
                return this.structureStatusDateFieldSpecified;
            }
            set
            {
                this.structureStatusDateFieldSpecified = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public System.DateTime structureStatusEndDate
        {
            get
            {
                return this.structureStatusEndDateField;
            }
            set
            {
                this.structureStatusEndDateField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool structureStatusEndDateSpecified
        {
            get
            {
                return this.structureStatusEndDateFieldSpecified;
            }
            set
            {
                this.structureStatusEndDateFieldSpecified = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string structureType
        {
            get
            {
                return this.structureTypeField;
            }
            set
            {
                this.structureTypeField = value;
            }
        }
    }
}
