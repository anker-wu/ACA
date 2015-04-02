#region Header
/**
 * <pre>
 * 
 *  Accela Citizen Access
 *  File: PartTransactionModel.cs
 * 
 *  Accela, Inc.
 *  Copyright (C): 2010
 * 
 *  Description:
 * 
 *  Notes:
 * $Id: PartTransactionModel.cs 181867 2010-10-06 08:06:18Z ACHIEVO\daly.zeng $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */
#endregion Header

namespace Accela.ACA.WSProxy
{
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.3053")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://service.webservice.accela.com/")]
    public partial class PartTransactionModel : LanguageModel
    {
        private string altIDField;

        private string capIDField;

        private string commentsField;

        private double costTotalField;

        private bool costTotalFieldSpecified;

        private string distributeFlagField;

        private string hardReservationField;

        private string iD1Field;

        private string iD2Field;

        private string iD3Field;

        private object[] locSupplyColField;

        private long locationSeqField;

        private bool locationSeqFieldSpecified;

        private string partBinField;

        private string partBrandField;

        private string partDescriptionField;

        private string partLocationField;

        private string partNumberField;

        private long partSeqField;

        private bool partSeqFieldSpecified;

        private string partTypeField;

        private double quantityField;

        private bool quantityFieldSpecified;

        private System.DateTime recDateField;

        private bool recDateFieldSpecified;

        private string recFulNamField;

        private string recStatusField;

        private string resPartBrandField;

        private string resPartDescriptionField;

        private string resPartLocationField;

        private string resToPartLocationField;

        private string resWorkOrderTaskCodeField;

        private long reservationNumField;

        private bool reservationNumFieldSpecified;

        private string reservationStatusField;

        private string servProvCodeField;

        private string taxableField;

        private long toLocationSeqField;

        private bool toLocationSeqFieldSpecified;

        private string toPartLocationField;

        private double transactionCostField;

        private bool transactionCostFieldSpecified;

        private System.DateTime transactionDateField;

        private bool transactionDateFieldSpecified;

        private System.DateTime transactionDateFromField;

        private bool transactionDateFromFieldSpecified;

        private System.DateTime transactionDateToField;

        private bool transactionDateToFieldSpecified;

        private long transactionSeqField;

        private bool transactionSeqFieldSpecified;

        private string transactionTypeField;

        private double unitCostField;

        private bool unitCostFieldSpecified;

        private string unitOfMeasurementField;

        private string workOrderTaskCodeField;

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
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string capID
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
        public string comments
        {
            get
            {
                return this.commentsField;
            }
            set
            {
                this.commentsField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public double costTotal
        {
            get
            {
                return this.costTotalField;
            }
            set
            {
                this.costTotalField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool costTotalSpecified
        {
            get
            {
                return this.costTotalFieldSpecified;
            }
            set
            {
                this.costTotalFieldSpecified = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string distributeFlag
        {
            get
            {
                return this.distributeFlagField;
            }
            set
            {
                this.distributeFlagField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string hardReservation
        {
            get
            {
                return this.hardReservationField;
            }
            set
            {
                this.hardReservationField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string ID1
        {
            get
            {
                return this.iD1Field;
            }
            set
            {
                this.iD1Field = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string ID2
        {
            get
            {
                return this.iD2Field;
            }
            set
            {
                this.iD2Field = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string ID3
        {
            get
            {
                return this.iD3Field;
            }
            set
            {
                this.iD3Field = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("locSupplyCol", Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = true)]
        public object[] locSupplyCol
        {
            get
            {
                return this.locSupplyColField;
            }
            set
            {
                this.locSupplyColField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public long locationSeq
        {
            get
            {
                return this.locationSeqField;
            }
            set
            {
                this.locationSeqField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool locationSeqSpecified
        {
            get
            {
                return this.locationSeqFieldSpecified;
            }
            set
            {
                this.locationSeqFieldSpecified = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string partBin
        {
            get
            {
                return this.partBinField;
            }
            set
            {
                this.partBinField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string partBrand
        {
            get
            {
                return this.partBrandField;
            }
            set
            {
                this.partBrandField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string partDescription
        {
            get
            {
                return this.partDescriptionField;
            }
            set
            {
                this.partDescriptionField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string partLocation
        {
            get
            {
                return this.partLocationField;
            }
            set
            {
                this.partLocationField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string partNumber
        {
            get
            {
                return this.partNumberField;
            }
            set
            {
                this.partNumberField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public long partSeq
        {
            get
            {
                return this.partSeqField;
            }
            set
            {
                this.partSeqField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool partSeqSpecified
        {
            get
            {
                return this.partSeqFieldSpecified;
            }
            set
            {
                this.partSeqFieldSpecified = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string partType
        {
            get
            {
                return this.partTypeField;
            }
            set
            {
                this.partTypeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public double quantity
        {
            get
            {
                return this.quantityField;
            }
            set
            {
                this.quantityField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool quantitySpecified
        {
            get
            {
                return this.quantityFieldSpecified;
            }
            set
            {
                this.quantityFieldSpecified = value;
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
        public string resPartBrand
        {
            get
            {
                return this.resPartBrandField;
            }
            set
            {
                this.resPartBrandField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string resPartDescription
        {
            get
            {
                return this.resPartDescriptionField;
            }
            set
            {
                this.resPartDescriptionField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string resPartLocation
        {
            get
            {
                return this.resPartLocationField;
            }
            set
            {
                this.resPartLocationField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string resToPartLocation
        {
            get
            {
                return this.resToPartLocationField;
            }
            set
            {
                this.resToPartLocationField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string resWorkOrderTaskCode
        {
            get
            {
                return this.resWorkOrderTaskCodeField;
            }
            set
            {
                this.resWorkOrderTaskCodeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public long reservationNum
        {
            get
            {
                return this.reservationNumField;
            }
            set
            {
                this.reservationNumField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool reservationNumSpecified
        {
            get
            {
                return this.reservationNumFieldSpecified;
            }
            set
            {
                this.reservationNumFieldSpecified = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string reservationStatus
        {
            get
            {
                return this.reservationStatusField;
            }
            set
            {
                this.reservationStatusField = value;
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
        public string taxable
        {
            get
            {
                return this.taxableField;
            }
            set
            {
                this.taxableField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public long toLocationSeq
        {
            get
            {
                return this.toLocationSeqField;
            }
            set
            {
                this.toLocationSeqField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool toLocationSeqSpecified
        {
            get
            {
                return this.toLocationSeqFieldSpecified;
            }
            set
            {
                this.toLocationSeqFieldSpecified = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string toPartLocation
        {
            get
            {
                return this.toPartLocationField;
            }
            set
            {
                this.toPartLocationField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public double transactionCost
        {
            get
            {
                return this.transactionCostField;
            }
            set
            {
                this.transactionCostField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool transactionCostSpecified
        {
            get
            {
                return this.transactionCostFieldSpecified;
            }
            set
            {
                this.transactionCostFieldSpecified = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public System.DateTime transactionDate
        {
            get
            {
                return this.transactionDateField;
            }
            set
            {
                this.transactionDateField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool transactionDateSpecified
        {
            get
            {
                return this.transactionDateFieldSpecified;
            }
            set
            {
                this.transactionDateFieldSpecified = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public System.DateTime transactionDateFrom
        {
            get
            {
                return this.transactionDateFromField;
            }
            set
            {
                this.transactionDateFromField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool transactionDateFromSpecified
        {
            get
            {
                return this.transactionDateFromFieldSpecified;
            }
            set
            {
                this.transactionDateFromFieldSpecified = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public System.DateTime transactionDateTo
        {
            get
            {
                return this.transactionDateToField;
            }
            set
            {
                this.transactionDateToField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool transactionDateToSpecified
        {
            get
            {
                return this.transactionDateToFieldSpecified;
            }
            set
            {
                this.transactionDateToFieldSpecified = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public long transactionSeq
        {
            get
            {
                return this.transactionSeqField;
            }
            set
            {
                this.transactionSeqField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool transactionSeqSpecified
        {
            get
            {
                return this.transactionSeqFieldSpecified;
            }
            set
            {
                this.transactionSeqFieldSpecified = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string transactionType
        {
            get
            {
                return this.transactionTypeField;
            }
            set
            {
                this.transactionTypeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public double unitCost
        {
            get
            {
                return this.unitCostField;
            }
            set
            {
                this.unitCostField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool unitCostSpecified
        {
            get
            {
                return this.unitCostFieldSpecified;
            }
            set
            {
                this.unitCostFieldSpecified = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string unitOfMeasurement
        {
            get
            {
                return this.unitOfMeasurementField;
            }
            set
            {
                this.unitOfMeasurementField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string workOrderTaskCode
        {
            get
            {
                return this.workOrderTaskCodeField;
            }
            set
            {
                this.workOrderTaskCodeField = value;
            }
        }
    }
}
