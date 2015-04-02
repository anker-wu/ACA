#region Header
/**
 * <pre>
 * 
 *  Accela Citizen Access
 *  File: ProviderModel.cs
 * 
 *  Accela, Inc.
 *  Copyright (C): 2011
 * 
 *  Description:
 * 
 *  Notes:
 * $Id: ProviderModel.cs 181867 2011-09-27 08:06:18Z ACHIEVO\daly.zeng $.
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
    public partial class ProviderModel : ProviderPKModel
    {

        private SimpleAuditModel auditModelField;

        private string externalExamURLField;

        private RProviderLocationModel[] rProviderLocationsField;

        private string offerContinuingField;

        private string offerEducationField;

        private string offerExaminationField;

        private string providerNameField;

        private string providerNoField;

        private RefContinuingEducationModel[] refContinuingEducationsField;

        private RefEducationModel[] refEducationsField;

        private XRefExaminationProviderModel[] xRefExaminationProvidersField;

        private RefExaminationModel[] refExaminationsField;

        private RefLicenseProfessionalModel refLicenseProfessionalModelField;

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
        [System.Xml.Serialization.XmlArrayAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        [System.Xml.Serialization.XmlArrayItemAttribute("rProviderLocation", Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = false)]
        public RProviderLocationModel[] rProviderLocations
        {
            get
            {
                return this.rProviderLocationsField;
            }
            set
            {
                this.rProviderLocationsField = value;
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
        [System.Xml.Serialization.XmlArrayAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        [System.Xml.Serialization.XmlArrayItemAttribute("refContinuingEducation", Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = false)]
        public RefContinuingEducationModel[] refContinuingEducations
        {
            get
            {
                return this.refContinuingEducationsField;
            }
            set
            {
                this.refContinuingEducationsField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        [System.Xml.Serialization.XmlArrayItemAttribute("refEducation", Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = false)]
        public RefEducationModel[] refEducations
        {
            get
            {
                return this.refEducationsField;
            }
            set
            {
                this.refEducationsField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        [System.Xml.Serialization.XmlArrayItemAttribute("xRefExaminationProvider", Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = false)]
        public XRefExaminationProviderModel[] xRefExaminationProviders
        {
            get
            {
                return this.xRefExaminationProvidersField;
            }
            set
            {
                this.xRefExaminationProvidersField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        [System.Xml.Serialization.XmlArrayItemAttribute("refExamination", Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = false)]
        public RefExaminationModel[] refExaminations
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
        public RefLicenseProfessionalModel refLicenseProfessionalModel
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
