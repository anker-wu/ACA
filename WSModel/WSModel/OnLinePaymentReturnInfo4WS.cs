/**
 * <pre>
 * 
 *  Accela Citizen Access
 *  File: OnLinePaymentReturnInfo4WS.cs
 * 
 *  Accela, Inc.
 *  Copyright (C): 2007-2010
 * 
 *  Description:
 * 
 *  Notes:
 * $Id: OnLinePaymentReturnInfo4WS.cs 182693 2010-10-19 08:24:27Z ACHIEVO\daly.zeng $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

namespace Accela.ACA.WSProxy
{
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.1433")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://model.webservice.accela.com")]
    public partial class OnLinePaymentReturnInfo4WS
    {

        private string accelaTransCodeField;

        private string agencyTransCodeField;

        private string batchNbrField;

        private string errCodeField;

        private PaymentModel paymentField;

        private PayTrail[] paymentTrailsField;

        private string rtnMsgField;

        private TransactionModel4WS[] transactionLogsField;

        private bool updLicenseStatusField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string accelaTransCode
        {
            get
            {
                return this.accelaTransCodeField;
            }
            set
            {
                this.accelaTransCodeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string agencyTransCode
        {
            get
            {
                return this.agencyTransCodeField;
            }
            set
            {
                this.agencyTransCodeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
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
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string errCode
        {
            get
            {
                return this.errCodeField;
            }
            set
            {
                this.errCodeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public PaymentModel payment
        {
            get
            {
                return this.paymentField;
            }
            set
            {
                this.paymentField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("paymentTrails", Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = true)]
        public PayTrail[] paymentTrails
        {
            get
            {
                return this.paymentTrailsField;
            }
            set
            {
                this.paymentTrailsField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string rtnMsg
        {
            get
            {
                return this.rtnMsgField;
            }
            set
            {
                this.rtnMsgField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("transactionLogs", Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = true)]
        public TransactionModel4WS[] transactionLogs
        {
            get
            {
                return this.transactionLogsField;
            }
            set
            {
                this.transactionLogsField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public bool updLicenseStatus
        {
            get
            {
                return this.updLicenseStatusField;
            }
            set
            {
                this.updLicenseStatusField = value;
            }
        }
    }
}
