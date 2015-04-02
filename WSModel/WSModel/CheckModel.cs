/**
 * <pre>
 * 
 *  Accela Citizen Access
 *  File: CheckModel.cs
 * 
 *  Accela, Inc.
 *  Copyright (C): 2007-2009
 * 
 *  Description:
 * 
 *  Notes:
 * $Id: CheckModel.cs 130107 2009-05-11 12:23:56Z ACHIEVO\jackie.yu $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

namespace Accela.ACA.WSProxy
{ /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.42")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://adaptor.onlinePayment.cashier.finance.aa.accela.com")]
    public partial class CheckModel
    {

        private string accountNbrField;

        private string checkNbrField;

        private string checkProTypeField;

        private string checkTypeField;

        private string cityField;

        private string emailField;

        private string licenseNbrField;

        private string nameField;

        private string phoneNbrField;

        private string postalCodeField;

        private string routingNbrField;

        private string ssNbrField;

        private string stateField;

        private string streetAddressField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public string accountNbr
        {
            get
            {
                return this.accountNbrField;
            }
            set
            {
                this.accountNbrField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public string checkNbr
        {
            get
            {
                return this.checkNbrField;
            }
            set
            {
                this.checkNbrField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public string checkProType
        {
            get
            {
                return this.checkProTypeField;
            }
            set
            {
                this.checkProTypeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public string checkType
        {
            get
            {
                return this.checkTypeField;
            }
            set
            {
                this.checkTypeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public string city
        {
            get
            {
                return this.cityField;
            }
            set
            {
                this.cityField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public string email
        {
            get
            {
                return this.emailField;
            }
            set
            {
                this.emailField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public string licenseNbr
        {
            get
            {
                return this.licenseNbrField;
            }
            set
            {
                this.licenseNbrField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public string name
        {
            get
            {
                return this.nameField;
            }
            set
            {
                this.nameField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public string phoneNbr
        {
            get
            {
                return this.phoneNbrField;
            }
            set
            {
                this.phoneNbrField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public string postalCode
        {
            get
            {
                return this.postalCodeField;
            }
            set
            {
                this.postalCodeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public string routingNbr
        {
            get
            {
                return this.routingNbrField;
            }
            set
            {
                this.routingNbrField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public string ssNbr
        {
            get
            {
                return this.ssNbrField;
            }
            set
            {
                this.ssNbrField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public string state
        {
            get
            {
                return this.stateField;
            }
            set
            {
                this.stateField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public string streetAddress
        {
            get
            {
                return this.streetAddressField;
            }
            set
            {
                this.streetAddressField = value;
            }
        }
    }
}
