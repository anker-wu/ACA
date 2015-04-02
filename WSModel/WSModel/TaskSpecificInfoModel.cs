#region Header
/**
 * <pre>
 * 
 *  Accela Citizen Access
 *  File: TaskSpecificInfoModel.cs
 * 
 *  Accela, Inc.
 *  Copyright (C): 2010
 * 
 *  Description:
 * 
 *  Notes:
 * $Id: TaskSpecificInfoModel.cs 181867 2010-10-06 08:06:18Z ACHIEVO\daly.zeng $.
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
    public partial class TaskSpecificInfoModel
    {
        private string actStatusField;

        private string attributeUnitTypeField;

        private string attributeValueField;

        private string attributeValueReqFlagField;

        private System.DateTime auditDateField;

        private bool auditDateFieldSpecified;

        private string auditStatusField;

        private string auditidField;

        private int bGroupDspOrderField;

        private bool bGroupDspOrderFieldSpecified;

        private string checkboxDescField;

        private string checkboxIndField;

        private string checkboxTypeField;

        private string checklistCommentField;

        private int displayLengthField;

        private bool displayLengthFieldSpecified;

        private long displayOrderField;

        private System.DateTime endDateField;

        private bool endDateFieldSpecified;

        private string feeIndicatorField;

        private string groupCodeField;

        private bool isCalculateField;

        private bool isCalculateFieldSpecified;

        private int maxLengthField;

        private bool maxLengthFieldSpecified;

        private string permitID1Field;

        private string permitID2Field;

        private string permitID3Field;

        private long processIDField;

        private string requiredFeeCalcField;

        private string serviceProviderCodeField;

        private System.DateTime startDateField;

        private bool startDateFieldSpecified;

        private int stepNumberField;

        private bool supervisorEditOnlyField;

        private bool supervisorEditOnlyFieldSpecified;

        private string taskStatusReqFlagField;

        private string validationScriptNameField;

        private object[] valueListField;

        private string vchDispFlagField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string actStatus
        {
            get
            {
                return this.actStatusField;
            }
            set
            {
                this.actStatusField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string attributeUnitType
        {
            get
            {
                return this.attributeUnitTypeField;
            }
            set
            {
                this.attributeUnitTypeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string attributeValue
        {
            get
            {
                return this.attributeValueField;
            }
            set
            {
                this.attributeValueField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string attributeValueReqFlag
        {
            get
            {
                return this.attributeValueReqFlagField;
            }
            set
            {
                this.attributeValueReqFlagField = value;
            }
        }

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
        public string auditid
        {
            get
            {
                return this.auditidField;
            }
            set
            {
                this.auditidField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public int BGroupDspOrder
        {
            get
            {
                return this.bGroupDspOrderField;
            }
            set
            {
                this.bGroupDspOrderField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool BGroupDspOrderSpecified
        {
            get
            {
                return this.bGroupDspOrderFieldSpecified;
            }
            set
            {
                this.bGroupDspOrderFieldSpecified = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string checkboxDesc
        {
            get
            {
                return this.checkboxDescField;
            }
            set
            {
                this.checkboxDescField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string checkboxInd
        {
            get
            {
                return this.checkboxIndField;
            }
            set
            {
                this.checkboxIndField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string checkboxType
        {
            get
            {
                return this.checkboxTypeField;
            }
            set
            {
                this.checkboxTypeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string checklistComment
        {
            get
            {
                return this.checklistCommentField;
            }
            set
            {
                this.checklistCommentField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public int displayLength
        {
            get
            {
                return this.displayLengthField;
            }
            set
            {
                this.displayLengthField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool displayLengthSpecified
        {
            get
            {
                return this.displayLengthFieldSpecified;
            }
            set
            {
                this.displayLengthFieldSpecified = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public long displayOrder
        {
            get
            {
                return this.displayOrderField;
            }
            set
            {
                this.displayOrderField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public System.DateTime endDate
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
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool endDateSpecified
        {
            get
            {
                return this.endDateFieldSpecified;
            }
            set
            {
                this.endDateFieldSpecified = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string feeIndicator
        {
            get
            {
                return this.feeIndicatorField;
            }
            set
            {
                this.feeIndicatorField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string groupCode
        {
            get
            {
                return this.groupCodeField;
            }
            set
            {
                this.groupCodeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public bool isCalculate
        {
            get
            {
                return this.isCalculateField;
            }
            set
            {
                this.isCalculateField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool isCalculateSpecified
        {
            get
            {
                return this.isCalculateFieldSpecified;
            }
            set
            {
                this.isCalculateFieldSpecified = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public int maxLength
        {
            get
            {
                return this.maxLengthField;
            }
            set
            {
                this.maxLengthField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool maxLengthSpecified
        {
            get
            {
                return this.maxLengthFieldSpecified;
            }
            set
            {
                this.maxLengthFieldSpecified = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string permitID1
        {
            get
            {
                return this.permitID1Field;
            }
            set
            {
                this.permitID1Field = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string permitID2
        {
            get
            {
                return this.permitID2Field;
            }
            set
            {
                this.permitID2Field = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string permitID3
        {
            get
            {
                return this.permitID3Field;
            }
            set
            {
                this.permitID3Field = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public long processID
        {
            get
            {
                return this.processIDField;
            }
            set
            {
                this.processIDField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string requiredFeeCalc
        {
            get
            {
                return this.requiredFeeCalcField;
            }
            set
            {
                this.requiredFeeCalcField = value;
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
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public System.DateTime startDate
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
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool startDateSpecified
        {
            get
            {
                return this.startDateFieldSpecified;
            }
            set
            {
                this.startDateFieldSpecified = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public int stepNumber
        {
            get
            {
                return this.stepNumberField;
            }
            set
            {
                this.stepNumberField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public bool supervisorEditOnly
        {
            get
            {
                return this.supervisorEditOnlyField;
            }
            set
            {
                this.supervisorEditOnlyField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool supervisorEditOnlySpecified
        {
            get
            {
                return this.supervisorEditOnlyFieldSpecified;
            }
            set
            {
                this.supervisorEditOnlyFieldSpecified = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string taskStatusReqFlag
        {
            get
            {
                return this.taskStatusReqFlagField;
            }
            set
            {
                this.taskStatusReqFlagField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string validationScriptName
        {
            get
            {
                return this.validationScriptNameField;
            }
            set
            {
                this.validationScriptNameField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("valueList", Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = true)]
        public object[] valueList
        {
            get
            {
                return this.valueListField;
            }
            set
            {
                this.valueListField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string vchDispFlag
        {
            get
            {
                return this.vchDispFlagField;
            }
            set
            {
                this.vchDispFlagField = value;
            }
        }
    }
}
