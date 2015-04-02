/**
 * <pre>
 * 
 *  Accela Citizen Access
 *  File: TemplateModel.cs
 * 
 *  Accela, Inc.
 *  Copyright (C): 2011
 * 
 *  Description:
 * 
 *  Notes:
 * $Id: TemplateModel.cs 130107 2009-05-11 12:23:56Z ACHIEVO\jackie.yu $.
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
    public partial class TemplateModel
    {

        private SimpleAuditModel auditModelField;

        private EntityPKModel entityPKModelField;

        private System.Nullable<bool> readOnlyField;

        private TemplateGroup[] templateFormsField;

        private TemplateGroup[] templateTablesField;

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
        [System.Xml.Serialization.XmlElementAttribute("templateForms", Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = true)]
        public TemplateGroup[] templateForms
        {
            get
            {
                return this.templateFormsField;
            }
            set
            {
                this.templateFormsField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("templateTables", Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = true)]
        public TemplateGroup[] templateTables
        {
            get
            {
                return this.templateTablesField;
            }
            set
            {
                this.templateTablesField = value;
            }
        }
    }
}
