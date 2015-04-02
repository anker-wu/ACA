/**
 * <pre>
 * 
 *  Accela Citizen Access
 *  File: CompletePaymentResultModel.cs
 * 
 *  Accela, Inc.
 *  Copyright (C): 2009-2012
 * 
 *  Description:
 * 
 *  Notes:
 * $Id: CapPaymentResultWithAddressModel.cs 219041 2012-05-10 05:40:41Z ACHIEVO\daly.zeng $.
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
    [System.Xml.Serialization.SoapTypeAttribute(Namespace = "http://model.webservice.accela.com")]
    public partial class CapPaymentResultWithAddressModel
    {

        private CapIDModel4WS capIDField; 

        private bool paymentStatusField;

        private string moduleNameField;

        private string receiptNbrField;

        private string batchNbrField;

        private string paramStringField;

        private string capTypeField;

        private bool hasFeeField;

        private string actionSourceField;

        /// <remarks/>
        [System.Xml.Serialization.SoapElementAttribute(IsNullable = true)]
        public string actionSource
        {
            get
            {
                return this.actionSourceField;
            }
            set
            {
                this.actionSourceField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.SoapElementAttribute(IsNullable = true)]
        public CapIDModel4WS capID
        {
            get
            {
                return this.capIDField;
            }
            set
            {
                this.capIDField = value;
            }
        } 

        /// <remarks/>
        [System.Xml.Serialization.SoapElementAttribute(IsNullable = true)]
        public bool paymentStatus
        {
            get
            {
                return this.paymentStatusField;
            }
            set
            {
                this.paymentStatusField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.SoapElementAttribute(IsNullable = true)]
        public string moduleName
        {
            get
            {
                return this.moduleNameField;
            }
            set
            {
                this.moduleNameField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.SoapElementAttribute(IsNullable = true)]
        public string receiptNbr
        {
            get
            {
                return this.receiptNbrField;
            }
            set
            {
                this.receiptNbrField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.SoapElementAttribute(IsNullable = true)]
        public string batchNbr
        {
            get
            {
                return this.batchNbrField;
            }
            set
            {
                this.batchNbrField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.SoapElementAttribute(IsNullable = true)]
        public string paramString
        {
            get
            {
                return this.paramStringField;
            }
            set
            {
                this.paramStringField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.SoapElementAttribute(IsNullable = true)]
        public AddressModel address
        {
            get;
            set;
        }
        

        /// <remarks/>
        [System.Xml.Serialization.SoapElementAttribute(IsNullable = true)]
        public string capType
        {
            get
            {
                return this.capTypeField;
            }
            set
            {
                this.capTypeField = value;
            }
        }       
            
        /// <remarks/>
        [System.Xml.Serialization.SoapElementAttribute(IsNullable = true)]
        public bool hasFee
        {
            get
            {
                return this.hasFeeField;
            }
            set
            {
                this.hasFeeField = value;
            }
        }
        
    }
}
