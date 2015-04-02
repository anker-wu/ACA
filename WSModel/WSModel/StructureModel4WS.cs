/**
 * <pre>
 * 
 *  Accela Citizen Access
 *  File: StructureModel4WS.cs
 * 
 *  Accela, Inc.
 *  Copyright (C): 2007-2009
 * 
 *  Description:
 * 
 *  Notes:
 * $Id: StructureModel4WS.cs 130107 2009-05-11 12:23:56Z ACHIEVO\jackie.yu $.
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
    public partial class StructureModel4WS
    {

        private string auditDateField;

        private string auditDateStringField;

        private string auditIDField;

        private string auditStatusField;

        private long sourceSeqNumberField;

        private string structureNameField;

        private long structureNumberField;

        private string structureStatusField;

        private string structureStatusDateField;

        private string structureStatusDateStringField;

        private string structureStatusEndDateField;

        private string structureTypeField;

        private StructureTypeModel4WS structureTypeModelField;

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
        public string auditDateString
        {
            get
            {
                return this.auditDateStringField;
            }
            set
            {
                this.auditDateStringField = value;
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
        public string structureStatusDate
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
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string structureStatusDateString
        {
            get
            {
                return this.structureStatusDateStringField;
            }
            set
            {
                this.structureStatusDateStringField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string structureStatusEndDate
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

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public StructureTypeModel4WS structureTypeModel
        {
            get
            {
                return this.structureTypeModelField;
            }
            set
            {
                this.structureTypeModelField = value;
            }
        }
    }
    
}
