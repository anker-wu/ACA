/**
 * <pre>
 * 
 *  Accela Citizen Access
 *  File: BizDomainModel4WS.cs
 * 
 *  Accela, Inc.
 *  Copyright (C): 2007-2009
 * 
 *  Description:
 * 
 *  Notes:
 * $Id: BizDomainModel4WS.cs 130107 2009-05-11 12:23:56Z ACHIEVO\jackie.yu $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

namespace Accela.ACA.WSProxy
{

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.42")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://model.webservice.accela.com")]
    public partial class BizDomainModel4WS
    {

        private string auditDateField;

        private string auditIDField;

        private string auditStatusField;//required

        private string bizdomainField;//required

        private string bizdomainValueField;//required

        private string descriptionField;

        private long dispositionIDField;//required when edit,but others needn't

        private string serviceProviderCodeField;//required

        private string sortOrderField;//if need be sorted ,required

        private string resBizdomainValueField;
        private string resDescriptionField;


        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string auditDate
        {
            get
            {
                return this.auditDateField;
            }
            set
            {
                this.auditDateField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string auditID
        {
            get
            {
                return this.auditIDField;
            }
            set
            {
                this.auditIDField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string auditStatus
        {
            get
            {
                return this.auditStatusField;
            }
            set
            {
                this.auditStatusField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string bizdomain
        {
            get
            {
                return this.bizdomainField;
            }
            set
            {
                this.bizdomainField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string bizdomainValue
        {
            get
            {
                return this.bizdomainValueField;
            }
            set
            {
                this.bizdomainValueField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string description
        {
            get
            {
                return this.descriptionField;
            }
            set
            {
                this.descriptionField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public long dispositionID
        {
            get
            {
                return this.dispositionIDField;
            }
            set
            {
                this.dispositionIDField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string serviceProviderCode
        {
            get
            {
                return this.serviceProviderCodeField;
            }
            set
            {
                this.serviceProviderCodeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string sortOrder
        {
            get
            {
                return this.sortOrderField;
            }
            set
            {
                this.sortOrderField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string resBizdomainValue 
        {
            get
            {
                return this.resBizdomainValueField;
            }
            set
            {
                this.resBizdomainValueField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string resDescription 
        {
            get
            {
                return this.resDescriptionField;
            }
            set
            {
                this.resDescriptionField = value;
            }
        }
    }
    
}
