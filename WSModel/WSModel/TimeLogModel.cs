#region Header
/**
 * <pre>
 * 
 *  Accela Citizen Access
 *  File: TimeLogModel.cs
 * 
 *  Accela, Inc.
 *  Copyright (C): 2010
 * 
 *  Description:
 * 
 *  Notes:
 * $Id: TimeLogModel.cs 181867 2010-10-06 08:06:18Z ACHIEVO\daly.zeng $.
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
    public partial class TimeLogModel : LanguageModel
    {
        private string accessModelField;

        private string billableField;

        private CapIDModel capIDModelField;

        private string createdByField;

        private System.DateTime createdDateField;

        private bool createdDateFieldSpecified;

        private System.DateTime dateLoggedField;

        private bool dateLoggedFieldSpecified;

        private System.DateTime endTimeField;

        private bool endTimeFieldSpecified;

        private string entityIdField;

        private string entityTypeField;

        private double entryCostField;

        private bool entryCostFieldSpecified;

        private double entryPctField;

        private bool entryPctFieldSpecified;

        private double entryRateField;

        private bool entryRateFieldSpecified;

        private System.DateTime lastChangeDateField;

        private bool lastChangeDateFieldSpecified;

        private string lastChangeUserField;

        private string materialsField;

        private double materialsCostField;

        private bool materialsCostFieldSpecified;

        private double milageTotalField;

        private bool milageTotalFieldSpecified;

        private double mileageEndField;

        private bool mileageEndFieldSpecified;

        private double mileageStartField;

        private bool mileageStartFieldSpecified;

        private string notationField;

        private string referenceField;

        private string servProvCodeField;

        private System.DateTime startTimeField;

        private bool startTimeFieldSpecified;

        private System.DateTime timeElapsedField;

        private bool timeElapsedFieldSpecified;

        private long timeGroupSeqField;

        private bool timeGroupSeqFieldSpecified;

        private long timeLogSeqField;

        private bool timeLogSeqFieldSpecified;

        private string timeLogStatusField;

        private TimeTypeModel timeTypeModelField;

        private long timeTypeSeqField;

        private bool timeTypeSeqFieldSpecified;

        private int totalMinutesField;

        private bool totalMinutesFieldSpecified;

        private long userGroupSeqNbrField;

        private bool userGroupSeqNbrFieldSpecified;

        private string vehicleIdField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string accessModel
        {
            get
            {
                return this.accessModelField;
            }
            set
            {
                this.accessModelField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string billable
        {
            get
            {
                return this.billableField;
            }
            set
            {
                this.billableField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public CapIDModel capIDModel
        {
            get
            {
                return this.capIDModelField;
            }
            set
            {
                this.capIDModelField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string createdBy
        {
            get
            {
                return this.createdByField;
            }
            set
            {
                this.createdByField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public System.DateTime createdDate
        {
            get
            {
                return this.createdDateField;
            }
            set
            {
                this.createdDateField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool createdDateSpecified
        {
            get
            {
                return this.createdDateFieldSpecified;
            }
            set
            {
                this.createdDateFieldSpecified = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public System.DateTime dateLogged
        {
            get
            {
                return this.dateLoggedField;
            }
            set
            {
                this.dateLoggedField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool dateLoggedSpecified
        {
            get
            {
                return this.dateLoggedFieldSpecified;
            }
            set
            {
                this.dateLoggedFieldSpecified = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public System.DateTime endTime
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
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool endTimeSpecified
        {
            get
            {
                return this.endTimeFieldSpecified;
            }
            set
            {
                this.endTimeFieldSpecified = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string entityId
        {
            get
            {
                return this.entityIdField;
            }
            set
            {
                this.entityIdField = value;
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
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public double entryCost
        {
            get
            {
                return this.entryCostField;
            }
            set
            {
                this.entryCostField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool entryCostSpecified
        {
            get
            {
                return this.entryCostFieldSpecified;
            }
            set
            {
                this.entryCostFieldSpecified = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public double entryPct
        {
            get
            {
                return this.entryPctField;
            }
            set
            {
                this.entryPctField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool entryPctSpecified
        {
            get
            {
                return this.entryPctFieldSpecified;
            }
            set
            {
                this.entryPctFieldSpecified = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public double entryRate
        {
            get
            {
                return this.entryRateField;
            }
            set
            {
                this.entryRateField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool entryRateSpecified
        {
            get
            {
                return this.entryRateFieldSpecified;
            }
            set
            {
                this.entryRateFieldSpecified = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public System.DateTime lastChangeDate
        {
            get
            {
                return this.lastChangeDateField;
            }
            set
            {
                this.lastChangeDateField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool lastChangeDateSpecified
        {
            get
            {
                return this.lastChangeDateFieldSpecified;
            }
            set
            {
                this.lastChangeDateFieldSpecified = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string lastChangeUser
        {
            get
            {
                return this.lastChangeUserField;
            }
            set
            {
                this.lastChangeUserField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string materials
        {
            get
            {
                return this.materialsField;
            }
            set
            {
                this.materialsField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public double materialsCost
        {
            get
            {
                return this.materialsCostField;
            }
            set
            {
                this.materialsCostField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool materialsCostSpecified
        {
            get
            {
                return this.materialsCostFieldSpecified;
            }
            set
            {
                this.materialsCostFieldSpecified = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public double milageTotal
        {
            get
            {
                return this.milageTotalField;
            }
            set
            {
                this.milageTotalField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool milageTotalSpecified
        {
            get
            {
                return this.milageTotalFieldSpecified;
            }
            set
            {
                this.milageTotalFieldSpecified = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public double mileageEnd
        {
            get
            {
                return this.mileageEndField;
            }
            set
            {
                this.mileageEndField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool mileageEndSpecified
        {
            get
            {
                return this.mileageEndFieldSpecified;
            }
            set
            {
                this.mileageEndFieldSpecified = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public double mileageStart
        {
            get
            {
                return this.mileageStartField;
            }
            set
            {
                this.mileageStartField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool mileageStartSpecified
        {
            get
            {
                return this.mileageStartFieldSpecified;
            }
            set
            {
                this.mileageStartFieldSpecified = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string notation
        {
            get
            {
                return this.notationField;
            }
            set
            {
                this.notationField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string reference
        {
            get
            {
                return this.referenceField;
            }
            set
            {
                this.referenceField = value;
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
        public System.DateTime startTime
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
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool startTimeSpecified
        {
            get
            {
                return this.startTimeFieldSpecified;
            }
            set
            {
                this.startTimeFieldSpecified = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public System.DateTime timeElapsed
        {
            get
            {
                return this.timeElapsedField;
            }
            set
            {
                this.timeElapsedField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool timeElapsedSpecified
        {
            get
            {
                return this.timeElapsedFieldSpecified;
            }
            set
            {
                this.timeElapsedFieldSpecified = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public long timeGroupSeq
        {
            get
            {
                return this.timeGroupSeqField;
            }
            set
            {
                this.timeGroupSeqField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool timeGroupSeqSpecified
        {
            get
            {
                return this.timeGroupSeqFieldSpecified;
            }
            set
            {
                this.timeGroupSeqFieldSpecified = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public long timeLogSeq
        {
            get
            {
                return this.timeLogSeqField;
            }
            set
            {
                this.timeLogSeqField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool timeLogSeqSpecified
        {
            get
            {
                return this.timeLogSeqFieldSpecified;
            }
            set
            {
                this.timeLogSeqFieldSpecified = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string timeLogStatus
        {
            get
            {
                return this.timeLogStatusField;
            }
            set
            {
                this.timeLogStatusField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public TimeTypeModel timeTypeModel
        {
            get
            {
                return this.timeTypeModelField;
            }
            set
            {
                this.timeTypeModelField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public long timeTypeSeq
        {
            get
            {
                return this.timeTypeSeqField;
            }
            set
            {
                this.timeTypeSeqField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool timeTypeSeqSpecified
        {
            get
            {
                return this.timeTypeSeqFieldSpecified;
            }
            set
            {
                this.timeTypeSeqFieldSpecified = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public int totalMinutes
        {
            get
            {
                return this.totalMinutesField;
            }
            set
            {
                this.totalMinutesField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool totalMinutesSpecified
        {
            get
            {
                return this.totalMinutesFieldSpecified;
            }
            set
            {
                this.totalMinutesFieldSpecified = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public long userGroupSeqNbr
        {
            get
            {
                return this.userGroupSeqNbrField;
            }
            set
            {
                this.userGroupSeqNbrField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool userGroupSeqNbrSpecified
        {
            get
            {
                return this.userGroupSeqNbrFieldSpecified;
            }
            set
            {
                this.userGroupSeqNbrFieldSpecified = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string vehicleId
        {
            get
            {
                return this.vehicleIdField;
            }
            set
            {
                this.vehicleIdField = value;
            }
        }
    }
}
