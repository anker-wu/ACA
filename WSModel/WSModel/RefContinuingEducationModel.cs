#region Header
/**
 * <pre>
 * 
 *  Accela Citizen Access
 *  File: RefContinuingEducationModel.cs
 * 
 *  Accela, Inc.
 *  Copyright (C): 2011
 * 
 *  Description:
 * 
 *  Notes:
 * $Id: RefContinuingEducationModel.cs 181867 2011-09-27 08:06:18Z ACHIEVO\daly.zeng $.
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
    public partial class RefContinuingEducationModel : RefContinuingEducationPKModel
    {
        private SimpleAuditModel auditModelField;

        private string commentsField;

        private string contEduNameField;

        private string gradingStyleField;

        private string passingScoreField;

        private ProviderModel[] providerModelsField;

        private XRefContinuingEducationAppTypeModel[] refContEduAppTypeModelsField;

        private XRefContinuingEducationProviderModel[] refContEduProviderModelsField;

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
        public string contEduName
        {
            get
            {
                return this.contEduNameField;
            }
            set
            {
                this.contEduNameField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string gradingStyle
        {
            get
            {
                return this.gradingStyleField;
            }
            set
            {
                this.gradingStyleField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string passingScore
        {
            get
            {
                return this.passingScoreField;
            }
            set
            {
                this.passingScoreField = value;
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
        [System.Xml.Serialization.XmlArrayItemAttribute("refContEduAppTypeModel", Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = false)]
        public XRefContinuingEducationAppTypeModel[] refContEduAppTypeModels
        {
            get
            {
                return this.refContEduAppTypeModelsField;
            }
            set
            {
                this.refContEduAppTypeModelsField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        [System.Xml.Serialization.XmlArrayItemAttribute("refContEduProviderModel", Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = false)]
        public XRefContinuingEducationProviderModel[] refContEduProviderModels
        {
            get
            {
                return this.refContEduProviderModelsField;
            }
            set
            {
                this.refContEduProviderModelsField = value;
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
