/**
 * <pre>
 * 
 *  Accela Citizen Access
 *  File: PaymentModel4WS.cs
 * 
 *  Accela, Inc.
 *  Copyright (C): 2007-2009
 * 
 *  Description:
 * 
 *  Notes:
 * $Id: PaymentModel4WS.cs 130107 2009-05-11 12:23:56Z ACHIEVO\jackie.yu $.
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
	public partial class PaymentModel4WS {
        
        private string acctIDField;
        
        private double amountNotAllocatedField;
        
        private string auditDateField;
        
        private string auditIDField;
        
        private string auditStatusField;
        
        private string batchTransCodeField;
        
        private CapIDModel4WS capIDField;
        
        private string cashierIDField;
        
        private string ccAuthCodeField;
        
        private string ccExpDateField;
        
        private string ccHolderNameField;
        
        private string ccTypeField;
        
        private long changeDueField;
        
        private string ftPerID1Field;
        
        private string ftPerID2Field;
        
        private string ftPerID3Field;
        
        private string officeCodeField;
        
        private string payeeField;
        
        private string payeeAddressField;
        
        private string payeePhoneField;
        
        private double paymentAmountField;
        
        private double paymentChangeField;
        
        private string paymentCommentField;
        
        private string paymentDateField;
        
        private string paymentMethodField;
        
        private string paymentRefNbrField;
        
        private long paymentSeqNbrField;
        
        private string paymentStatusField;
        
        private string posTransSeqField;
        
        private double processingFeeField;
        
        private string receiptCustomizedNBRField;
        
        private long receiptNbrField;
        
        private string registerNbrField;
        
        private long sessionNbrField;
        
        private string terminalIDField;
        
        private double totalInvoiceAmountField;
        
        private double totalPaidAmountField;
        
        private string tranCodeField;
        
        private string tranNbrField;
        
        private CapIDModel4WS transferCapIDField;
        
        private string udf1Field;
        
        private string udf2Field;
        
        private string udf3Field;
        
        private string udf4Field;
        
        private string workstationIDField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string acctID {
            get {
                return this.acctIDField;
            }
            set {
                this.acctIDField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public double amountNotAllocated {
            get {
                return this.amountNotAllocatedField;
            }
            set {
                this.amountNotAllocatedField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string auditDate {
            get {
                return this.auditDateField;
            }
            set {
                this.auditDateField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string auditID {
            get {
                return this.auditIDField;
            }
            set {
                this.auditIDField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string auditStatus {
            get {
                return this.auditStatusField;
            }
            set {
                this.auditStatusField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string batchTransCode {
            get {
                return this.batchTransCodeField;
            }
            set {
                this.batchTransCodeField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public CapIDModel4WS capID {
            get {
                return this.capIDField;
            }
            set {
                this.capIDField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string cashierID {
            get {
                return this.cashierIDField;
            }
            set {
                this.cashierIDField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string ccAuthCode {
            get {
                return this.ccAuthCodeField;
            }
            set {
                this.ccAuthCodeField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string ccExpDate {
            get {
                return this.ccExpDateField;
            }
            set {
                this.ccExpDateField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string ccHolderName {
            get {
                return this.ccHolderNameField;
            }
            set {
                this.ccHolderNameField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string ccType {
            get {
                return this.ccTypeField;
            }
            set {
                this.ccTypeField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public long changeDue {
            get {
                return this.changeDueField;
            }
            set {
                this.changeDueField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string ftPerID1 {
            get {
                return this.ftPerID1Field;
            }
            set {
                this.ftPerID1Field = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string ftPerID2 {
            get {
                return this.ftPerID2Field;
            }
            set {
                this.ftPerID2Field = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string ftPerID3 {
            get {
                return this.ftPerID3Field;
            }
            set {
                this.ftPerID3Field = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string officeCode {
            get {
                return this.officeCodeField;
            }
            set {
                this.officeCodeField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string payee {
            get {
                return this.payeeField;
            }
            set {
                this.payeeField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string payeeAddress {
            get {
                return this.payeeAddressField;
            }
            set {
                this.payeeAddressField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string payeePhone {
            get {
                return this.payeePhoneField;
            }
            set {
                this.payeePhoneField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public double paymentAmount {
            get {
                return this.paymentAmountField;
            }
            set {
                this.paymentAmountField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public double paymentChange {
            get {
                return this.paymentChangeField;
            }
            set {
                this.paymentChangeField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string paymentComment {
            get {
                return this.paymentCommentField;
            }
            set {
                this.paymentCommentField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string paymentDate {
            get {
                return this.paymentDateField;
            }
            set {
                this.paymentDateField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string paymentMethod {
            get {
                return this.paymentMethodField;
            }
            set {
                this.paymentMethodField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string paymentRefNbr {
            get {
                return this.paymentRefNbrField;
            }
            set {
                this.paymentRefNbrField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public long paymentSeqNbr {
            get {
                return this.paymentSeqNbrField;
            }
            set {
                this.paymentSeqNbrField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string paymentStatus {
            get {
                return this.paymentStatusField;
            }
            set {
                this.paymentStatusField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string posTransSeq {
            get {
                return this.posTransSeqField;
            }
            set {
                this.posTransSeqField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public double processingFee {
            get {
                return this.processingFeeField;
            }
            set {
                this.processingFeeField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string receiptCustomizedNBR {
            get {
                return this.receiptCustomizedNBRField;
            }
            set {
                this.receiptCustomizedNBRField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public long receiptNbr {
            get {
                return this.receiptNbrField;
            }
            set {
                this.receiptNbrField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string registerNbr {
            get {
                return this.registerNbrField;
            }
            set {
                this.registerNbrField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public long sessionNbr {
            get {
                return this.sessionNbrField;
            }
            set {
                this.sessionNbrField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string terminalID {
            get {
                return this.terminalIDField;
            }
            set {
                this.terminalIDField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public double totalInvoiceAmount {
            get {
                return this.totalInvoiceAmountField;
            }
            set {
                this.totalInvoiceAmountField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public double totalPaidAmount {
            get {
                return this.totalPaidAmountField;
            }
            set {
                this.totalPaidAmountField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string tranCode {
            get {
                return this.tranCodeField;
            }
            set {
                this.tranCodeField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string tranNbr {
            get {
                return this.tranNbrField;
            }
            set {
                this.tranNbrField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public CapIDModel4WS transferCapID {
            get {
                return this.transferCapIDField;
            }
            set {
                this.transferCapIDField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string udf1 {
            get {
                return this.udf1Field;
            }
            set {
                this.udf1Field = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string udf2 {
            get {
                return this.udf2Field;
            }
            set {
                this.udf2Field = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string udf3 {
            get {
                return this.udf3Field;
            }
            set {
                this.udf3Field = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string udf4 {
            get {
                return this.udf4Field;
            }
            set {
                this.udf4Field = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string workstationID {
            get {
                return this.workstationIDField;
            }
            set {
                this.workstationIDField = value;
            }
        }
    }
}
