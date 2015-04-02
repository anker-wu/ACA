/**
 * <pre>
 * 
 *  Accela Citizen Access
 *  File: TrustAccountTransactionModel.cs
 * 
 *  Accela, Inc.
 *  Copyright (C): 2010
 * 
 *  Description:
 * 
 *  Notes:
 * $Id: TrustAccountTransactionModel.cs 182693 2010-10-19 08:24:27Z ACHIEVO\daly.zeng $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Accela.ACA.WSProxy
{

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.3053")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://model.webservice.accela.com")]
    public partial class TrustAccountTransactionModel
    {
        private string acctIDField;

        private System.Nullable<long> acctSeqField;

        private string altIDField;

        private System.Nullable<double> balanceField;

        private System.Nullable<long> batchSeqNbrField;

        private CapIDModel capIDField;

        private string cashierIDField;

        private string ccAuthCodeField;

        private System.Nullable<System.DateTime> ccExpDateField;

        private string ccHolderNameField;

        private System.Nullable<long> clientReceiptNbrField;

        private System.Nullable<long> clientTransNbrField;

        private string commentField;

        private string depositMethodField;

        private System.Nullable<System.DateTime> endRecDateField;

        private string officeCodeField;

        private string paymentRefNbrField;

        private System.Nullable<long> paymentSeqNbrField;

        private string payorField;

        private string procTransIDField;

        private System.Nullable<double> processingFeeField;

        private System.Nullable<System.DateTime> recDateField;

        private string recFulNamField;

        private string recStatusField;

        private string receiptCustomizedNbrField;

        private string receivedTypeField;

        private string registerNbrField;

        private string servProvCodeField;

        private long sessionNbrField;

        private string targetAcctIDField;

        private string terminalIDField;

        private System.Nullable<double> transAmountField;

        private System.Nullable<long> transSeqField;

        private string transTypeField;

        private string workstationIDField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string acctID
        {
            get
            {
                return this.acctIDField;
            }
            set
            {
                this.acctIDField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = true)]
        public System.Nullable<long> acctSeq
        {
            get
            {
                return this.acctSeqField;
            }
            set
            {
                this.acctSeqField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string altID
        {
            get
            {
                return this.altIDField;
            }
            set
            {
                this.altIDField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = true)]
        public System.Nullable<double> balance
        {
            get
            {
                return this.balanceField;
            }
            set
            {
                this.balanceField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = true)]
        public System.Nullable<long> batchSeqNbr
        {
            get
            {
                return this.batchSeqNbrField;
            }
            set
            {
                this.batchSeqNbrField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public CapIDModel capID
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
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string cashierID
        {
            get
            {
                return this.cashierIDField;
            }
            set
            {
                this.cashierIDField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string ccAuthCode
        {
            get
            {
                return this.ccAuthCodeField;
            }
            set
            {
                this.ccAuthCodeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = true)]
        public System.Nullable<System.DateTime> ccExpDate
        {
            get
            {
                return this.ccExpDateField;
            }
            set
            {
                this.ccExpDateField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string ccHolderName
        {
            get
            {
                return this.ccHolderNameField;
            }
            set
            {
                this.ccHolderNameField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = true)]
        public System.Nullable<long> clientReceiptNbr
        {
            get
            {
                return this.clientReceiptNbrField;
            }
            set
            {
                this.clientReceiptNbrField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = true)]
        public System.Nullable<long> clientTransNbr
        {
            get
            {
                return this.clientTransNbrField;
            }
            set
            {
                this.clientTransNbrField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string comment
        {
            get
            {
                return this.commentField;
            }
            set
            {
                this.commentField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string depositMethod
        {
            get
            {
                return this.depositMethodField;
            }
            set
            {
                this.depositMethodField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = true)]
        public System.Nullable<System.DateTime> endRecDate
        {
            get
            {
                return this.endRecDateField;
            }
            set
            {
                this.endRecDateField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string officeCode
        {
            get
            {
                return this.officeCodeField;
            }
            set
            {
                this.officeCodeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string paymentRefNbr
        {
            get
            {
                return this.paymentRefNbrField;
            }
            set
            {
                this.paymentRefNbrField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = true)]
        public System.Nullable<long> paymentSeqNbr
        {
            get
            {
                return this.paymentSeqNbrField;
            }
            set
            {
                this.paymentSeqNbrField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string payor
        {
            get
            {
                return this.payorField;
            }
            set
            {
                this.payorField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string procTransID
        {
            get
            {
                return this.procTransIDField;
            }
            set
            {
                this.procTransIDField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = true)]
        public System.Nullable<double> processingFee
        {
            get
            {
                return this.processingFeeField;
            }
            set
            {
                this.processingFeeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = true)]
        public System.Nullable<System.DateTime> recDate
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
        public string receiptCustomizedNbr
        {
            get
            {
                return this.receiptCustomizedNbrField;
            }
            set
            {
                this.receiptCustomizedNbrField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string receivedType
        {
            get
            {
                return this.receivedTypeField;
            }
            set
            {
                this.receivedTypeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string registerNbr
        {
            get
            {
                return this.registerNbrField;
            }
            set
            {
                this.registerNbrField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string servProvCode
        {
            get
            {
                return this.servProvCodeField;
            }
            set
            {
                this.servProvCodeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public long sessionNbr
        {
            get
            {
                return this.sessionNbrField;
            }
            set
            {
                this.sessionNbrField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string targetAcctID
        {
            get
            {
                return this.targetAcctIDField;
            }
            set
            {
                this.targetAcctIDField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string terminalID
        {
            get
            {
                return this.terminalIDField;
            }
            set
            {
                this.terminalIDField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = true)]
        public System.Nullable<double> transAmount
        {
            get
            {
                return this.transAmountField;
            }
            set
            {
                this.transAmountField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = true)]
        public System.Nullable<long> transSeq
        {
            get
            {
                return this.transSeqField;
            }
            set
            {
                this.transSeqField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string transType
        {
            get
            {
                return this.transTypeField;
            }
            set
            {
                this.transTypeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string workstationID
        {
            get
            {
                return this.workstationIDField;
            }
            set
            {
                this.workstationIDField = value;
            }
        }
    }
    
}
