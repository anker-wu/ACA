/**
 * <pre>
 * 
 *  Accela Citizen Access
 *  File: ProviderModel4WS.cs
 * 
 *  Accela, Inc.
 *  Copyright (C): 2009-2011
 * 
 *  Description:
 *  ProviderModel4WS model..
 *  
 *  Notes:
 * $Id: ProviderModel4WS.cs 206999 2011-11-09 01:21:42Z ACHIEVO\alan.hu $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

namespace Accela.ACA.WSProxy
{
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.1433")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://model.webservice.accela.com")]
    public partial class ProviderModel4WS : ProviderPKModel4WS
    {

        private string externalExamURLField;

        private string offerContinuingField;

        private string offerEducationField;

        private string offerExaminationField;

        private string providerNameField;

        private string providerNoField;

        private RefContinuingEducationModel4WS[] refContEducationsField;

        private RefEducationModel4WS[] refEduModelField;

        private RefExaminationModel4WS[] refExaminationsField;

        private RefLicenseProfessionalModel4WS refLicenseProfessionalModelField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string externalExamURL
        {
            get
            {
                return this.externalExamURLField;
            }
            set
            {
                this.externalExamURLField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string offerContinuing
        {
            get
            {
                return this.offerContinuingField;
            }
            set
            {
                this.offerContinuingField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string offerEducation
        {
            get
            {
                return this.offerEducationField;
            }
            set
            {
                this.offerEducationField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string offerExamination
        {
            get
            {
                return this.offerExaminationField;
            }
            set
            {
                this.offerExaminationField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string providerName
        {
            get
            {
                return this.providerNameField;
            }
            set
            {
                this.providerNameField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string providerNo
        {
            get
            {
                return this.providerNoField;
            }
            set
            {
                this.providerNoField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("refContEducations", Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = true)]
        public RefContinuingEducationModel4WS[] refContEducations
        {
            get
            {
                return this.refContEducationsField;
            }
            set
            {
                this.refContEducationsField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("refEduModel", Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = true)]
        public RefEducationModel4WS[] refEduModel
        {
            get
            {
                return this.refEduModelField;
            }
            set
            {
                this.refEduModelField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("refExaminations", Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = true)]
        public RefExaminationModel4WS[] refExaminations
        {
            get
            {
                return this.refExaminationsField;
            }
            set
            {
                this.refExaminationsField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public RefLicenseProfessionalModel4WS refLicenseProfessionalModel
        {
            get
            {
                return this.refLicenseProfessionalModelField;
            }
            set
            {
                this.refLicenseProfessionalModelField = value;
            }
        }
    }
}
