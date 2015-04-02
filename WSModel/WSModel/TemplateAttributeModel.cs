/**
 * <pre>
 * 
 *  Accela Citizen Access
 *  File: TemplateAttributeModel.cs
 * 
 *  Accela, Inc.
 *  Copyright (C): 2010
 * 
 *  Description:
 * 
 *  Notes:
 * $Id: TemplateAttributeModel.cs 171698 2010-04-29 09:39:39Z ACHIEVO\daly.zeng $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Accela.ACA.WSProxy
{
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.3053")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://model.webservice.accela.com")]
    public partial class TemplateAttributeModel
    {

        private string attributeLabelField;

        private string attributeNameField;

        private string attributeScriptCodeField;

        private string attributeUnitTypeField;

        private string attributeValueField;

        private string attributeValueDataTypeField;

        private string attributeValueReqFlagField;

        private System.Nullable<System.DateTime> auditDateField;

        private string auditIDField;

        private string auditStatusField;

        private CapIDModel capIDField;

        private string defaultAttributeLabelField;

        private System.Nullable<long> displayOrderField;

        private TemplateEntityType entityTypeField;

        private bool entityTypeFieldSpecified;

        private string instructionField;

        private string resAttributeUnitTypeField;

        private string resInstructionField;

        private string resWaterMarkField;

        private TemplateAttrValueModel[] selectOptionsField;

        private string serviceProviderCodeField;

        private string templateObjectNumField;

        private string templateTypeField;

        private string vchFlagField;

        private string waterMarkField;

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
        public string attributeScriptCode
        {
            get
            {
                return this.attributeScriptCodeField;
            }
            set
            {
                this.attributeScriptCodeField = value;
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
        public CapIDModel capID
        {
            get
            {
                return this.capIDField;
            }
            set
            {
                this.capIDField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string defaultAttributeLabel
        {
            get
            {
                return this.defaultAttributeLabelField;
            }
            set
            {
                this.defaultAttributeLabelField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = true)]
        public System.Nullable<long> displayOrder
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
        public TemplateEntityType entityType
        {
            get
            {
                return this.entityTypeField;
            }
            set
            {
                this.entityTypeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool entityTypeSpecified
        {
            get
            {
                return this.entityTypeFieldSpecified;
            }
            set
            {
                this.entityTypeFieldSpecified = value;
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
        public string resAttributeUnitType
        {
            get
            {
                return this.resAttributeUnitTypeField;
            }
            set
            {
                this.resAttributeUnitTypeField = value;
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
        public string resWaterMark
        {
            get
            {
                return this.resWaterMarkField;
            }
            set
            {
                this.resWaterMarkField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        [System.Xml.Serialization.XmlArrayItemAttribute("selectOption", Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = false)]
        public TemplateAttrValueModel[] selectOptions
        {
            get
            {
                return this.selectOptionsField;
            }
            set
            {
                this.selectOptionsField = value;
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
        public string templateObjectNum
        {
            get
            {
                return this.templateObjectNumField;
            }
            set
            {
                this.templateObjectNumField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string templateType
        {
            get
            {
                return this.templateTypeField;
            }
            set
            {
                this.templateTypeField = value;
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

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string waterMark
        {
            get
            {
                return this.waterMarkField;
            }
            set
            {
                this.waterMarkField = value;
            }
        }
    }
}
