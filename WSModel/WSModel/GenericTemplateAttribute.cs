/**
 * <pre>
 * 
 *  Accela Citizen Access
 *  File: GenericTemplateAttribute.cs
 * 
 *  Accela, Inc.
 *  Copyright (C): 2011-2014
 * 
 *  Description:
 * 
 *  Notes:
 * $Id: GenericTemplateAttribute.cs 171698 2010-04-29 09:39:39Z ACHIEVO\daly.zeng $.
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
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.18060")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://model.webservice.accela.com")]
    public partial class GenericTemplateAttribute : GenericTemplateFieldPK
    {

        private ACATemplateConfigModel acaTemplateConfigField;

        private string alignField;

        private string asiGroupCodeField;

        private SimpleAuditModel auditModelField;

        private string defaultValueField;

        private string displayFieldNameField;

        private int displayLenField;

        private System.Nullable<int> displayOrderField;

        private EntityPKModel entityPKModelField;

        private System.Nullable<int> entityTypeField;

        private int fieldTypeField;

        private int maxLenField;

        private GenericTemplateAttributeEntry[] optionsField;

        private bool readOnlyField;

        private string requireFlagField;

        private System.Nullable<long> seqNumField;

        private string serviceProviderCodeField;

        private System.Nullable<int> subgroupDispOrderField;

        private System.Nullable<int> templateTypeField;

        private string unitTypeField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public ACATemplateConfigModel acaTemplateConfig
        {
            get
            {
                return this.acaTemplateConfigField;
            }
            set
            {
                this.acaTemplateConfigField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string align
        {
            get
            {
                return this.alignField;
            }
            set
            {
                this.alignField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string asiGroupCode
        {
            get
            {
                return this.asiGroupCodeField;
            }
            set
            {
                this.asiGroupCodeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public SimpleAuditModel auditModel
        {
            get
            {
                return this.auditModelField;
            }
            set
            {
                this.auditModelField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string defaultValue
        {
            get
            {
                return this.defaultValueField;
            }
            set
            {
                this.defaultValueField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string displayFieldName
        {
            get
            {
                return this.displayFieldNameField;
            }
            set
            {
                this.displayFieldNameField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public int displayLen
        {
            get
            {
                return this.displayLenField;
            }
            set
            {
                this.displayLenField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = true)]
        public System.Nullable<int> displayOrder
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
        public EntityPKModel entityPKModel
        {
            get
            {
                return this.entityPKModelField;
            }
            set
            {
                this.entityPKModelField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = true)]
        public System.Nullable<int> entityType
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
        public int maxLen
        {
            get
            {
                return this.maxLenField;
            }
            set
            {
                this.maxLenField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        [System.Xml.Serialization.XmlArrayItemAttribute("entry", Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = false)]
        public GenericTemplateAttributeEntry[] options
        {
            get
            {
                return this.optionsField;
            }
            set
            {
                this.optionsField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public bool readOnly
        {
            get
            {
                return this.readOnlyField;
            }
            set
            {
                this.readOnlyField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string requireFlag
        {
            get
            {
                return this.requireFlagField;
            }
            set
            {
                this.requireFlagField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = true)]
        public System.Nullable<long> seqNum
        {
            get
            {
                return this.seqNumField;
            }
            set
            {
                this.seqNumField = value;
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
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = true)]
        public System.Nullable<int> subgroupDispOrder
        {
            get
            {
                return this.subgroupDispOrderField;
            }
            set
            {
                this.subgroupDispOrderField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = true)]
        public System.Nullable<int> templateType
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
        public string unitType
        {
            get
            {
                return this.unitTypeField;
            }
            set
            {
                this.unitTypeField = value;
            }
        }
    }
}
