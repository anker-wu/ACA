#region Header
/**
 * <pre>
 * 
 *  Accela Citizen Access
 *  File: CostingModel.cs
 * 
 *  Accela, Inc.
 *  Copyright (C): 2010
 * 
 *  Description:
 * 
 *  Notes:
 * $Id: CostingModel.cs 181867 2010-10-06 08:06:18Z ACHIEVO\daly.zeng $.
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
    public partial class CostingModel : LanguageModel
    {
        private System.DateTime auditDateField;

        private bool auditDateFieldSpecified;

        private string auditIDField;

        private string auditStatusField;

        private CapIDModel capIDField;

        private string costCategoryField;

        private string costStatusField;

        private double costTotalField;

        private bool costTotalFieldSpecified;

        private string costingCommentsField;

        private string costingCostAccountField;

        private System.DateTime costingCostDateField;

        private bool costingCostDateFieldSpecified;

        private System.DateTime costingCostDateToField;

        private bool costingCostDateToFieldSpecified;

        private double costingCostFactorField;

        private bool costingCostFactorFieldSpecified;

        private double costingCostFixField;

        private bool costingCostFixFieldSpecified;

        private long costingCostIDField;

        private bool costingCostIDFieldSpecified;

        private string costingCostItemField;

        private string costingCostTypeField;

        private string costingCostTypeItemField;

        private CostingPK costingPKField;

        private double costingQuantityField;

        private bool costingQuantityFieldSpecified;

        private double costingTotalCostField;

        private bool costingTotalCostFieldSpecified;

        private double costingUnitCostField;

        private bool costingUnitCostFieldSpecified;

        private string costingUnitOfMeasureField;

        private string distributeFlagField;

        private string endTimeField;

        private long relatedAsgnNbrField;

        private bool relatedAsgnNbrFieldSpecified;

        private string startTimeField;

        private string workOrderTaskCodeField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public System.DateTime auditDate
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
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool auditDateSpecified
        {
            get
            {
                return this.auditDateFieldSpecified;
            }
            set
            {
                this.auditDateFieldSpecified = value;
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
        public string costCategory
        {
            get
            {
                return this.costCategoryField;
            }
            set
            {
                this.costCategoryField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string costStatus
        {
            get
            {
                return this.costStatusField;
            }
            set
            {
                this.costStatusField = value;
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
        public string costingComments
        {
            get
            {
                return this.costingCommentsField;
            }
            set
            {
                this.costingCommentsField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string costingCostAccount
        {
            get
            {
                return this.costingCostAccountField;
            }
            set
            {
                this.costingCostAccountField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public System.DateTime costingCostDate
        {
            get
            {
                return this.costingCostDateField;
            }
            set
            {
                this.costingCostDateField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool costingCostDateSpecified
        {
            get
            {
                return this.costingCostDateFieldSpecified;
            }
            set
            {
                this.costingCostDateFieldSpecified = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public System.DateTime costingCostDateTo
        {
            get
            {
                return this.costingCostDateToField;
            }
            set
            {
                this.costingCostDateToField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool costingCostDateToSpecified
        {
            get
            {
                return this.costingCostDateToFieldSpecified;
            }
            set
            {
                this.costingCostDateToFieldSpecified = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public double costingCostFactor
        {
            get
            {
                return this.costingCostFactorField;
            }
            set
            {
                this.costingCostFactorField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool costingCostFactorSpecified
        {
            get
            {
                return this.costingCostFactorFieldSpecified;
            }
            set
            {
                this.costingCostFactorFieldSpecified = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public double costingCostFix
        {
            get
            {
                return this.costingCostFixField;
            }
            set
            {
                this.costingCostFixField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool costingCostFixSpecified
        {
            get
            {
                return this.costingCostFixFieldSpecified;
            }
            set
            {
                this.costingCostFixFieldSpecified = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public long costingCostID
        {
            get
            {
                return this.costingCostIDField;
            }
            set
            {
                this.costingCostIDField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool costingCostIDSpecified
        {
            get
            {
                return this.costingCostIDFieldSpecified;
            }
            set
            {
                this.costingCostIDFieldSpecified = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string costingCostItem
        {
            get
            {
                return this.costingCostItemField;
            }
            set
            {
                this.costingCostItemField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string costingCostType
        {
            get
            {
                return this.costingCostTypeField;
            }
            set
            {
                this.costingCostTypeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string costingCostTypeItem
        {
            get
            {
                return this.costingCostTypeItemField;
            }
            set
            {
                this.costingCostTypeItemField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public CostingPK costingPK
        {
            get
            {
                return this.costingPKField;
            }
            set
            {
                this.costingPKField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public double costingQuantity
        {
            get
            {
                return this.costingQuantityField;
            }
            set
            {
                this.costingQuantityField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool costingQuantitySpecified
        {
            get
            {
                return this.costingQuantityFieldSpecified;
            }
            set
            {
                this.costingQuantityFieldSpecified = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public double costingTotalCost
        {
            get
            {
                return this.costingTotalCostField;
            }
            set
            {
                this.costingTotalCostField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool costingTotalCostSpecified
        {
            get
            {
                return this.costingTotalCostFieldSpecified;
            }
            set
            {
                this.costingTotalCostFieldSpecified = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public double costingUnitCost
        {
            get
            {
                return this.costingUnitCostField;
            }
            set
            {
                this.costingUnitCostField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool costingUnitCostSpecified
        {
            get
            {
                return this.costingUnitCostFieldSpecified;
            }
            set
            {
                this.costingUnitCostFieldSpecified = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string costingUnitOfMeasure
        {
            get
            {
                return this.costingUnitOfMeasureField;
            }
            set
            {
                this.costingUnitOfMeasureField = value;
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
        public string endTime
        {
            get
            {
                return this.endTimeField;
            }
            set
            {
                this.endTimeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public long relatedAsgnNbr
        {
            get
            {
                return this.relatedAsgnNbrField;
            }
            set
            {
                this.relatedAsgnNbrField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool relatedAsgnNbrSpecified
        {
            get
            {
                return this.relatedAsgnNbrFieldSpecified;
            }
            set
            {
                this.relatedAsgnNbrFieldSpecified = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string startTime
        {
            get
            {
                return this.startTimeField;
            }
            set
            {
                this.startTimeField = value;
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
