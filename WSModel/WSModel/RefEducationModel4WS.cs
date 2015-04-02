/**
 * <pre>
 * 
 *  Accela Citizen Access
 *  File: RefEducationModel4WS.cs
 * 
 *  Accela, Inc.
 *  Copyright (C): 2009-2013
 * 
 *  Description:
 *  RefEducationModel4WS model..
 * 
 *  Notes:
 * $Id: RefEducationModel4WS.cs 242253 2013-01-13 08:47:05Z ACHIEVO\alan.hu $.
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
    public partial class RefEducationModel4WS : RefEducationPKModel4WS
    {

        private AuditModel4WS auditModelField;

        private string commentsField;

        private string degreeField;

        private ProviderModel4WS[] providerModelsField;

        private XRefEducationAppTypeModel4WS[] refEduAppTypeModelsField;

        private XRefEducationProviderModel4WS[] refEduProviderModelsField;

        private string refEducationNameField;

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
        [System.Xml.Serialization.XmlElementAttribute("refEduAppTypeModels", Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = true)]
        public XRefEducationAppTypeModel4WS[] refEduAppTypeModels
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
        [System.Xml.Serialization.XmlElementAttribute("refEduProviderModels", Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = true)]
        public XRefEducationProviderModel4WS[] refEduProviderModels
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
