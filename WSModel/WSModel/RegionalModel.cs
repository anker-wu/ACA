using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Accela.ACA.WSProxy
{
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.17929")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://service.webservice.accela.com/")]
    public partial class RegionalModel : RegionalPKModel
    {

        private SimpleAuditModel auditModelField;

        private string countryField;

        private string countryCodeField;

        private bool defaultCountryRegionField;

        private string mappedStateField;

        private string phoneNumMaskField;

        private string phoneNumCodeField;

        private string useZipCodeField;

        private string zipCodeMaskField;

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
        public string country
        {
            get
            {
                return this.countryField;
            }
            set
            {
                this.countryField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string countryCode
        {
            get
            {
                return this.countryCodeField;
            }
            set
            {
                this.countryCodeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public bool defaultCountryRegion
        {
            get
            {
                return this.defaultCountryRegionField;
            }
            set
            {
                this.defaultCountryRegionField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string mappedState
        {
            get
            {
                return this.mappedStateField;
            }
            set
            {
                this.mappedStateField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string phoneNumMask
        {
            get
            {
                return this.phoneNumMaskField;
            }
            set
            {
                this.phoneNumMaskField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string phoneNumCode
        {
            get
            {
                return this.phoneNumCodeField;
            }
            set
            {
                this.phoneNumCodeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string useZipCode
        {
            get
            {
                return this.useZipCodeField;
            }
            set
            {
                this.useZipCodeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string zipCodeMask
        {
            get
            {
                return this.zipCodeMaskField;
            }
            set
            {
                this.zipCodeMaskField = value;
            }
        }
    }
}
