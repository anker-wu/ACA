#region Header
/**
 * <pre>
 * 
 *  Accela Citizen Access
 *  File: RefExaminationModel.cs
 * 
 *  Accela, Inc.
 *  Copyright (C): 2011
 * 
 *  Description:
 * 
 *  Notes:
 * $Id: RefExaminationModel.cs 181867 2011-09-27 08:06:18Z ACHIEVO\daly.zeng $.
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
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://model.webservice.accela.com")]
    public partial class RefExaminationModel : RefExaminationPKModel
    {

        private SimpleAuditModel auditModelField;

        private string commentsField;

        private string examNameField;

        private string gradingStyleField;

        private string passingScoreField;

        private ProviderModel[] providerModelsField;

        private XRefExaminationAppTypeModel[] refExamAppTypeModelsField;

        private XRefExaminationProviderModel[] refExamProviderModelsField;

        private System.Nullable<long> resIdField;

        private string workflowstatusField;

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
        public string examName
        {
            get
            {
                return this.examNameField;
            }
            set
            {
                this.examNameField = value;
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
        [System.Xml.Serialization.XmlArrayItemAttribute("refExamAppTypeModel", Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = false)]
        public XRefExaminationAppTypeModel[] refExamAppTypeModels
        {
            get
            {
                return this.refExamAppTypeModelsField;
            }
            set
            {
                this.refExamAppTypeModelsField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        [System.Xml.Serialization.XmlArrayItemAttribute("refExamProviderModel", Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = false)]
        public XRefExaminationProviderModel[] refExamProviderModels
        {
            get
            {
                return this.refExamProviderModelsField;
            }
            set
            {
                this.refExamProviderModelsField = value;
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

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string workflowstatus
        {
            get
            {
                return this.workflowstatusField;
            }
            set
            {
                this.workflowstatusField = value;
            }
        }
    }
}
