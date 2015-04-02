#region Header
/**
 * <pre>
 * 
 *  Accela Citizen Access
 *  File: TemplateSubgroup.cs
 * 
 *  Accela, Inc.
 *  Copyright (C): 2011-2012
 * 
 *  Description:
 * 
 *  Notes:
 * $Id: TemplateSubgroup.cs 181867 2010-10-06 08:06:18Z ACHIEVO\daly.zeng $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */
#endregion Header

namespace Accela.ACA.WSProxy
{
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.225")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://model.webservice.accela.com")]
    public partial class TemplateSubgroup
    {

        private ACATemplateConfigModel acaTemplateConfigModelField;

        private string displayNameField;

        private System.Nullable<int> displayOrderField;

        private GenericTemplateAttribute[] fieldsField;

        private System.Nullable<bool> readOnlyField;

        private TemplateRow[] rowsField;

        private string subgroupNameField;

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
        public string displayName
        {
            get
            {
                return this.displayNameField;
            }
            set
            {
                this.displayNameField = value;
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
        [System.Xml.Serialization.XmlElementAttribute("fields", Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = true)]
        public GenericTemplateAttribute[] fields
        {
            get
            {
                return this.fieldsField;
            }
            set
            {
                this.fieldsField = value;
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
        [System.Xml.Serialization.XmlElementAttribute("rows", Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = true)]
        public TemplateRow[] rows
        {
            get
            {
                return this.rowsField;
            }
            set
            {
                this.rowsField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string subgroupName
        {
            get
            {
                return this.subgroupNameField;
            }
            set
            {
                this.subgroupNameField = value;
            }
        }
    }
}
