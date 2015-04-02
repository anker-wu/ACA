/**
 * <pre>
 * 
 *  Accela Citizen Access
 *  File: OnlinePaymentTransactionModel.cs.cs
 * 
 *  Accela, Inc.
 *  Copyright (C): 2013
 * 
 *  Description:
 * 
 * </pre>
 */
namespace Accela.ACA.WSProxy
{
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.1")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://model.webservice.accela.com")]
    public partial class OnlinePaymentTransactionModel
    {

        private string aaTransIdField;

        private string aaTransStatusField;

        private string applicationIdField;

        private string authCodeField;

        private string ccNumberField;

        private string ccTypeField;

        private string convenienceFeeField;

        private string data1Field;

        private string data2Field;

        private string data3Field;

        private string data4Field;

        private string notificationURLField;

        private string paymentAmountField;

        private string payorNameField;

        private string procTransIdField;

        private string procTransMessageField;

        private string procTransResultCodeField;

        private string procTransStatusField;

        private string procTransTypeField;

        private System.DateTime recDateField;

        private bool recDateFieldSpecified;

        private string recFulNamField;

        private string recStatusField;

        private string redirectToACAURLField;

        private string transSeqNumField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string aaTransId
        {
            get
            {
                return this.aaTransIdField;
            }
            set
            {
                this.aaTransIdField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string aaTransStatus
        {
            get
            {
                return this.aaTransStatusField;
            }
            set
            {
                this.aaTransStatusField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string applicationId
        {
            get
            {
                return this.applicationIdField;
            }
            set
            {
                this.applicationIdField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string authCode
        {
            get
            {
                return this.authCodeField;
            }
            set
            {
                this.authCodeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string ccNumber
        {
            get
            {
                return this.ccNumberField;
            }
            set
            {
                this.ccNumberField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string ccType
        {
            get
            {
                return this.ccTypeField;
            }
            set
            {
                this.ccTypeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string convenienceFee
        {
            get
            {
                return this.convenienceFeeField;
            }
            set
            {
                this.convenienceFeeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string data1
        {
            get
            {
                return this.data1Field;
            }
            set
            {
                this.data1Field = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string data2
        {
            get
            {
                return this.data2Field;
            }
            set
            {
                this.data2Field = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string data3
        {
            get
            {
                return this.data3Field;
            }
            set
            {
                this.data3Field = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string data4
        {
            get
            {
                return this.data4Field;
            }
            set
            {
                this.data4Field = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string notificationURL
        {
            get
            {
                return this.notificationURLField;
            }
            set
            {
                this.notificationURLField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string paymentAmount
        {
            get
            {
                return this.paymentAmountField;
            }
            set
            {
                this.paymentAmountField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string payorName
        {
            get
            {
                return this.payorNameField;
            }
            set
            {
                this.payorNameField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string procTransId
        {
            get
            {
                return this.procTransIdField;
            }
            set
            {
                this.procTransIdField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string procTransMessage
        {
            get
            {
                return this.procTransMessageField;
            }
            set
            {
                this.procTransMessageField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string procTransResultCode
        {
            get
            {
                return this.procTransResultCodeField;
            }
            set
            {
                this.procTransResultCodeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string procTransStatus
        {
            get
            {
                return this.procTransStatusField;
            }
            set
            {
                this.procTransStatusField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string procTransType
        {
            get
            {
                return this.procTransTypeField;
            }
            set
            {
                this.procTransTypeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public System.DateTime recDate
        {
            get
            {
                return this.recDateField;
            }
            set
            {
                this.recDateField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool recDateSpecified
        {
            get
            {
                return this.recDateFieldSpecified;
            }
            set
            {
                this.recDateFieldSpecified = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string recFulNam
        {
            get
            {
                return this.recFulNamField;
            }
            set
            {
                this.recFulNamField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string recStatus
        {
            get
            {
                return this.recStatusField;
            }
            set
            {
                this.recStatusField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string redirectToACAURL
        {
            get
            {
                return this.redirectToACAURLField;
            }
            set
            {
                this.redirectToACAURLField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string transSeqNum
        {
            get
            {
                return this.transSeqNumField;
            }
            set
            {
                this.transSeqNumField = value;
            }
        }
    }
}