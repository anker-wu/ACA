/**
 * <pre>
 * 
 *  Accela Citizen Access
 *  File: ComponentModel.cs
 * 
 *  Accela, Inc.
 *  Copyright (C): 2010-2013
 * 
 *  Description:
 * 
 *  Notes:
 * $Id: ComponentModel.cs 131140 2010-08-12 03:22:37Z ACHIEVO\daly.zeng $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

namespace Accela.ACA.WSProxy
{
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.1")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://model.webservice.accela.com")]
    public partial class ComponentModel : LanguageModel
    {
        private System.Nullable<System.DateTime> auditDateField;

        private string auditIDField;

        private string auditStatusField;

        private long componentIDField;

        private string componentNameField;

        private long componentSeqNbrField;

        private string customHeadingField;

        private string displayFlagField;

        private int displayOrderField;

        private string editableFlagField;

        private string instructionField;

        private string pageFlowGrpCodeField;

        private long pageIDField;

        private string portletRange1Field;

        private string portletRange2Field;

        private string requiredFlagField;

        private string resCustomHeadingField;

        private string resInstructionField;

        private string serviceProviderCodeField;

        private string validateFlagField;

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
        public long componentID
        {
            get
            {
                return this.componentIDField;
            }
            set
            {
                this.componentIDField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string componentName
        {
            get
            {
                return this.componentNameField;
            }
            set
            {
                this.componentNameField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public long componentSeqNbr
        {
            get
            {
                return this.componentSeqNbrField;
            }
            set
            {
                this.componentSeqNbrField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string customHeading
        {
            get
            {
                return this.customHeadingField;
            }
            set
            {
                this.customHeadingField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string displayFlag
        {
            get
            {
                return this.displayFlagField;
            }
            set
            {
                this.displayFlagField = value;
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
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string editableFlag
        {
            get
            {
                return this.editableFlagField;
            }
            set
            {
                this.editableFlagField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string instruction
        {
            get
            {
                return this.instructionField;
            }
            set
            {
                this.instructionField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string pageFlowGrpCode
        {
            get
            {
                return this.pageFlowGrpCodeField;
            }
            set
            {
                this.pageFlowGrpCodeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public long pageID
        {
            get
            {
                return this.pageIDField;
            }
            set
            {
                this.pageIDField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string portletRange1
        {
            get
            {
                return this.portletRange1Field;
            }
            set
            {
                this.portletRange1Field = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string portletRange2
        {
            get
            {
                return this.portletRange2Field;
            }
            set
            {
                this.portletRange2Field = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string requiredFlag
        {
            get
            {
                return this.requiredFlagField;
            }
            set
            {
                this.requiredFlagField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string resCustomHeading
        {
            get
            {
                return this.resCustomHeadingField;
            }
            set
            {
                this.resCustomHeadingField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string resInstruction
        {
            get
            {
                return this.resInstructionField;
            }
            set
            {
                this.resInstructionField = value;
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
        public string validateFlag
        {
            get
            {
                return this.validateFlagField;
            }
            set
            {
                this.validateFlagField = value;
            }
        }
    }
}
