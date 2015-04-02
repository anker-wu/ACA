#region Header
/**
 * <pre>
 * 
 *  Accela Citizen Access
 *  File: RefEducationModel.cs
 * 
 *  Accela, Inc.
 *  Copyright (C): 2011
 * 
 *  Description:
 * 
 *  Notes:
 * $Id: RefEducationModel.cs 181867 2011-09-27 08:06:18Z ACHIEVO\daly.zeng $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */
#endregion Header

namespace Accela.ACA.WSProxy
{
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.1")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://service.webservice.accela.com/")]
    public partial class RefEducationModel : RefEducationPKModel
    {
        private SimpleAuditModel auditModelField;

        private string commentsField;

        private string degreeField;

        private ProviderModel[] providerModelsField;

        private XRefEducationAppTypeModel[] refEduAppTypeModelsField;

        private XRefEducationProviderModel[] refEduProviderModelsField;

        private string refEducationNameField;

        private System.Nullable<long> resIdField;

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
        public string comments
        {
            get
            {
                return this.commentsField;
            }
            set
            {
                this.commentsField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string degree
        {
            get
            {
                return this.degreeField;
            }
            set
            {
                this.degreeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        [System.Xml.Serialization.XmlArrayItemAttribute("providerModel", Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = false)]
        public ProviderModel[] providerModels
        {
            get
            {
                return this.providerModelsField;
            }
            set
            {
                this.providerModelsField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        [System.Xml.Serialization.XmlArrayItemAttribute("refEduAppTypeModel", Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = false)]
        public XRefEducationAppTypeModel[] refEduAppTypeModels
        {
            get
            {
                return this.refEduAppTypeModelsField;
            }
            set
            {
                this.refEduAppTypeModelsField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        [System.Xml.Serialization.XmlArrayItemAttribute("refEduProviderModel", Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = false)]
        public XRefEducationProviderModel[] refEduProviderModels
        {
            get
            {
                return this.refEduProviderModelsField;
            }
            set
            {
                this.refEduProviderModelsField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string refEducationName
        {
            get
            {
                return this.refEducationNameField;
            }
            set
            {
                this.refEducationNameField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = true)]
        public System.Nullable<long> resId
        {
            get
            {
                return this.resIdField;
            }
            set
            {
                this.resIdField = value;
            }
        }
    }
}
