/**
 * <pre>
 * 
 *  Accela Citizen Access
 *  File: TemplateGroup.cs
 * 
 *  Accela, Inc.
 *  Copyright (C): 2011-2012
 * 
 *  Description:
 * 
 *  Notes:
 * $Id: TemplateGroup.cs 130107 2009-05-11 12:23:56Z ACHIEVO\jackie.yu $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

namespace Accela.ACA.WSProxy
{
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.225")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://model.webservice.accela.com")]
    public partial class TemplateGroup
    {

        private ACATemplateConfigModel acaTemplateConfigModelField;

        private string groupNameField;

        private System.Nullable<bool> readOnlyField;

        private TemplateSubgroup[] subgroupsField;

        private templateType templateTypeField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public ACATemplateConfigModel acaTemplateConfigModel
        {
            get
            {
                return this.acaTemplateConfigModelField;
            }
            set
            {
                this.acaTemplateConfigModelField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string groupName
        {
            get
            {
                return this.groupNameField;
            }
            set
            {
                this.groupNameField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = true)]
        public System.Nullable<bool> readOnly
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
        [System.Xml.Serialization.XmlElementAttribute("subgroups", Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = true)]
        public TemplateSubgroup[] subgroups
        {
            get
            {
                return this.subgroupsField;
            }
            set
            {
                this.subgroupsField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public templateType templateType
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
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.225")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://model.webservice.accela.com/")]
    public enum templateType
    {

        /// <remarks/>
        DefaultType,

        /// <remarks/>
        Form,

        /// <remarks/>
        Table,
    }
}
