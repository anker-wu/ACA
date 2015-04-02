/**
 * <pre>
 * 
 *  Accela Citizen Access
 *  File: RefContinuingEducationModel4WS.cs
 * 
 *  Accela, Inc.
 *  Copyright (C): 2009-2013
 * 
 *  Description:
 * 
 *  Notes:
 * $Id: RefContinuingEducationModel4WS.cs 130107 2009-05-11 12:23:56Z ACHIEVO\jackie.yu $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

namespace Accela.ACA.WSProxy
{
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.18060")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://model.webservice.accela.com")]
    public partial class RefContinuingEducationModel4WS : RefContinuingEducationPKModel4WS
    {

        private AuditModel4WS auditModelField;

        private string commentsField;

        private string contEduNameField;

        private string gradingStyleField;

        private string passingPercentageField;

        private string passingScoreField;

        private ProviderModel4WS[] providerModelsField;

        private XRefContinuingEducationAppTypeModel4WS[] refContEduAppTypeModelsField;

        private XRefContinuingEducationProviderModel4WS[] refContEduProviderModelsField;

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
        [System.Xml.Serialization.XmlElementAttribute("refContEduAppTypeModels", Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = true)]
        public XRefContinuingEducationAppTypeModel4WS[] refContEduAppTypeModels
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
        [System.Xml.Serialization.XmlElementAttribute("refContEduProviderModels", Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = true)]
        public XRefContinuingEducationProviderModel4WS[] refContEduProviderModels
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
