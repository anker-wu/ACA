/**
 * <pre>
 * 
 *  Accela Citizen Access
 *  File: RefExaminationModel4WS.cs
 * 
 *  Accela, Inc.
 *  Copyright (C): 2009-2013
 * 
 *  Description:
 *  RefExaminationModel4WS model..
 * 
 *  Notes:
 * $Id: RefExaminationModel4WS.cs 137738 2009-07-06 06:52:41Z ACHIEVO\jackie.yu $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

namespace Accela.ACA.WSProxy
{
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.17929")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://model.webservice.accela.com")]
    public partial class RefExaminationModel4WS : RefExaminationPKModel4WS
    {

        private AuditModel4WS auditModelField;

        private string commentsField;

        private string examNameField;

        private string gradingStyleField;

        private string passingPercentageField;

        private string passingScoreField;

        private ProviderModel4WS[] providerModelsField;

        private XRefExaminationAppTypeModel4WS[] refExamAppTypeModelsField;

        private XRefExaminationProviderModel4WS[] refExamProviderModelsField;

        private string templateField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public AuditModel4WS auditModel
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
        public string passingPercentage
        {
            get
            {
                return this.passingPercentageField;
            }
            set
            {
                this.passingPercentageField = value;
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
        [System.Xml.Serialization.XmlElementAttribute("providerModels", Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = true)]
        public ProviderModel4WS[] providerModels
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
        [System.Xml.Serialization.XmlElementAttribute("refExamAppTypeModels", Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = true)]
        public XRefExaminationAppTypeModel4WS[] refExamAppTypeModels
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
        [System.Xml.Serialization.XmlElementAttribute("refExamProviderModels", Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = true)]
        public XRefExaminationProviderModel4WS[] refExamProviderModels
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
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string template
        {
            get
            {
                return this.templateField;
            }
            set
            {
                this.templateField = value;
            }
        }
    }
}
