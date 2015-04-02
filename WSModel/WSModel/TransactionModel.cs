/**
 * <pre>
 * 
 *  Accela Citizen Access
 *  File: TransactionModel4WS4TrustAcc.cs
 * 
 *  Accela, Inc.
 *  Copyright (C): 2010-2013
 * 
 *  Description:
 * 
 *  Notes:
 * $Id: TransactionModel4WS4TrustAcc.cs 130107 2009-05-11 12:23:56Z ACHIEVO\jackie.yu $.
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
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://model.webservice.accela.com")]
    public partial class TransactionModel
    {
        private System.Nullable<long> accountNumberField;

        private string actionSourceField;

        private System.Nullable<System.DateTime> auditDateField;

        private string auditIDField;

        private string auditStatusField;

        private string authCodeField;

        private System.Nullable<long> batchTransCodeField;

        private string cCHolderNameField;

        private string cCNumberField;

        private string cCTypeField;

        private System.Nullable<long> clientNumberField;

        private string convFeeTransCodeField;

        private double convenienceFeeField;

        private string customBatchTransCodeField;

        private System.Nullable<System.DateTime> endDateField;

        private string entityIDField;

        private string entityTypeField;

        private System.Nullable<System.DateTime> exportDateField;

        private string exportFileNameField;

        private string feeTypeField;

        private string gateWayTransactionIDField;

        private string gatewayTransTtatusField;

        private string isExportedField;

        private string procRespMsgField;

        private string procResultField;

        private string procTransIDField;

        private string procTransTypeField;

        private string procZipRespField;

        private string providerField;

        private long receiptNbrField;

        private string serviceProviderCodeField;

        private System.Nullable<System.DateTime> startDateField;

        private string statusField;

        private string terminalIDField;

        private System.Nullable<double> totalFeeField;

        private string transTypeField;

        private System.Nullable<long> transactionNumberField;

        private string workstationIDField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = true)]
        public System.Nullable<long> accountNumber
        {
            get
            {
                return this.accountNumberField;
            }
            set
            {
                this.accountNumberField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
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
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = true)]
        public System.Nullable<System.DateTime> auditDate
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
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = true)]
        public System.Nullable<long> batchTransCode
        {
            get
            {
                return this.batchTransCodeField;
            }
            set
            {
                this.batchTransCodeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string CCHolderName
        {
            get
            {
                return this.cCHolderNameField;
            }
            set
            {
                this.cCHolderNameField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string CCNumber
        {
            get
            {
                return this.cCNumberField;
            }
            set
            {
                this.cCNumberField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string CCType
        {
            get
            {
                return this.cCTypeField;
            }
            set
            {
                this.cCTypeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = true)]
        public System.Nullable<long> clientNumber
        {
            get
            {
                return this.clientNumberField;
            }
            set
            {
                this.clientNumberField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string convFeeTransCode
        {
            get
            {
                return this.convFeeTransCodeField;
            }
            set
            {
                this.convFeeTransCodeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public double convenienceFee
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
        public string customBatchTransCode
        {
            get
            {
                return this.customBatchTransCodeField;
            }
            set
            {
                this.customBatchTransCodeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = true)]
        public System.Nullable<System.DateTime> endDate
        {
            get
            {
                return this.endDateField;
            }
            set
            {
                this.endDateField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string entityID
        {
            get
            {
                return this.entityIDField;
            }
            set
            {
                this.entityIDField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string entityType
        {
            get
            {
                return this.entityTypeField;
            }
            set
            {
                this.entityTypeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = true)]
        public System.Nullable<System.DateTime> exportDate
        {
            get
            {
                return this.exportDateField;
            }
            set
            {
                this.exportDateField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string exportFileName
        {
            get
            {
                return this.exportFileNameField;
            }
            set
            {
                this.exportFileNameField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string feeType
        {
            get
            {
                return this.feeTypeField;
            }
            set
            {
                this.feeTypeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string gateWayTransactionID
        {
            get
            {
                return this.gateWayTransactionIDField;
            }
            set
            {
                this.gateWayTransactionIDField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string gatewayTransTtatus
        {
            get
            {
                return this.gatewayTransTtatusField;
            }
            set
            {
                this.gatewayTransTtatusField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string isExported
        {
            get
            {
                return this.isExportedField;
            }
            set
            {
                this.isExportedField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string procRespMsg
        {
            get
            {
                return this.procRespMsgField;
            }
            set
            {
                this.procRespMsgField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string procResult
        {
            get
            {
                return this.procResultField;
            }
            set
            {
                this.procResultField = value;
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
        public string procZipResp
        {
            get
            {
                return this.procZipRespField;
            }
            set
            {
                this.procZipRespField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string provider
        {
            get
            {
                return this.providerField;
            }
            set
            {
                this.providerField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public long receiptNbr
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
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = true)]
        public System.Nullable<System.DateTime> startDate
        {
            get
            {
                return this.startDateField;
            }
            set
            {
                this.startDateField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string status
        {
            get
            {
                return this.statusField;
            }
            set
            {
                this.statusField = value;
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
        public System.Nullable<double> totalFee
        {
            get
            {
                return this.totalFeeField;
            }
            set
            {
                this.totalFeeField = value;
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
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = true)]
        public System.Nullable<long> transactionNumber
        {
            get
            {
                return this.transactionNumberField;
            }
            set
            {
                this.transactionNumberField = value;
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
