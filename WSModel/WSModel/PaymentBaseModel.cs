/**
 * <pre>
 * 
 *  Accela Citizen Access
 *  File: PaymenttBaseModel.cs
 * 
 *  Accela, Inc.
 *  Copyright (C): 2010
 * 
 *  Description:
 * 
 *  Notes:
 * $Id: PaymentBaseModel.cs 182693 2010-10-19 08:24:27Z ACHIEVO\daly.zeng $.
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
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(PaymentModel))]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.3053")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://model.webservice.accela.com")]
    public partial class PaymentBaseModel
    {

        private string acctIDField;

        private double amountNotAllocatedField;

        private System.Nullable<System.DateTime> auditDateField;

        private string auditIDField;

        private string auditStatusField;

        private string cCAuthCodeField;

        private CapIDModel capIDField;

        private string cardHolderNameField;

        private string cashierIDField;

        private System.Nullable<System.DateTime> ccExpDateField;

        private string ccTypeField;

        private double changeDueField;

        private string officeCodeField;

        private string payeeField;

        private string payeeAddressField;

        private string payeePhoneField;

        private string payeePhoneCountryCodeField;

        private double paymentAmountField;

        private double paymentChangeField;

        private string paymentCommentField;

        private System.Nullable<System.DateTime> paymentDateField;

        private string paymentMethodField;

        private string paymentRefNbrField;

        private long paymentSeqNbrField;

        private string paymentStatusField;

        private string receiptCustomizedNBRField;

        private long receiptNbrField;

        private string receivedTypeField;

        private string registerNbrField;

        private long sessionNbrField;

        private string terminalIDField;

        private double totalInvoiceAmountField;

        private double totalPaidAmountField;

        private string tranCodeField;

        private string tranNbrField;

        private CapIDModel transferCapIDField;

        private string udf1Field;

        private string udf2Field;

        private string udf3Field;

        private string udf4Field;

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
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public double amountNotAllocated
        {
            get
            {
                return this.amountNotAllocatedField;
            }
            set
            {
                this.amountNotAllocatedField = value;
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
        public string CCAuthCode
        {
            get
            {
                return this.cCAuthCodeField;
            }
            set
            {
                this.cCAuthCodeField = value;
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
        public string cardHolderName
        {
            get
            {
                return this.cardHolderNameField;
            }
            set
            {
                this.cardHolderNameField = value;
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
        public double changeDue
        {
            get
            {
                return this.changeDueField;
            }
            set
            {
                this.changeDueField = value;
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
        public string payee
        {
            get
            {
                return this.payeeField;
            }
            set
            {
                this.payeeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string payeeAddress
        {
            get
            {
                return this.payeeAddressField;
            }
            set
            {
                this.payeeAddressField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string payeePhone
        {
            get
            {
                return this.payeePhoneField;
            }
            set
            {
                this.payeePhoneField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string payeePhoneCountryCode
        {
            get
            {
                return this.payeePhoneCountryCodeField;
            }
            set
            {
                this.payeePhoneCountryCodeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public double paymentAmount
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
        public double paymentChange
        {
            get
            {
                return this.paymentChangeField;
            }
            set
            {
                this.paymentChangeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string paymentComment
        {
            get
            {
                return this.paymentCommentField;
            }
            set
            {
                this.paymentCommentField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = true)]
        public System.Nullable<System.DateTime> paymentDate
        {
            get
            {
                return this.paymentDateField;
            }
            set
            {
                this.paymentDateField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string paymentMethod
        {
            get
            {
                return this.paymentMethodField;
            }
            set
            {
                this.paymentMethodField = value;
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
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public long paymentSeqNbr
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
        public string paymentStatus
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
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string receiptCustomizedNBR
        {
            get
            {
                return this.receiptCustomizedNBRField;
            }
            set
            {
                this.receiptCustomizedNBRField = value;
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
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public double totalInvoiceAmount
        {
            get
            {
                return this.totalInvoiceAmountField;
            }
            set
            {
                this.totalInvoiceAmountField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public double totalPaidAmount
        {
            get
            {
                return this.totalPaidAmountField;
            }
            set
            {
                this.totalPaidAmountField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string tranCode
        {
            get
            {
                return this.tranCodeField;
            }
            set
            {
                this.tranCodeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string tranNbr
        {
            get
            {
                return this.tranNbrField;
            }
            set
            {
                this.tranNbrField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public CapIDModel transferCapID
        {
            get
            {
                return this.transferCapIDField;
            }
            set
            {
                this.transferCapIDField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string udf1
        {
            get
            {
                return this.udf1Field;
            }
            set
            {
                this.udf1Field = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string udf2
        {
            get
            {
                return this.udf2Field;
            }
            set
            {
                this.udf2Field = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string udf3
        {
            get
            {
                return this.udf3Field;
            }
            set
            {
                this.udf3Field = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string udf4
        {
            get
            {
                return this.udf4Field;
            }
            set
            {
                this.udf4Field = value;
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
