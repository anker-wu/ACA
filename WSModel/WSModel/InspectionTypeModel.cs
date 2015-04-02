#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: InspectionTypeModel.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2010-2011
 *
 *  Description:
 *
 *  Notes:
 * $Id: InspectionTypeModel.cs 178385 2010-08-06 07:47:06Z ACHIEVO\jackie.yu $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 *  05/09/2007    troy.yang    Initial version.
 * </pre>
 */

#endregion Header

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
    public partial class InspectionTypeModel : LanguageModel
    {

        private bool isActiveField;

        private string allowFailGuideSheetItemsField;

        private string allowMultiInspField;

        private System.Nullable<System.DateTime> auditDateField;

        private string auditStatusField;

        private string autoAssignField;

        private CalendarInspectionTypeModel calendarInspectionTypeField;

        private System.Nullable<int> cancelDaysInACAField;

        private System.Nullable<int> cancelHoursInACAField;

        private string cancelOptionInACAField;

        private string cancelTimeInACAField;

        private string carryoverFlagField;

        private bool isCompletedField;

        private bool isConfiguredInInspFlowField;

        private string displayInACAField;

        private string flowEnabledFlagField;

        private string gradeField;

        private string groupCodeField;

        private string groupCodeNameField;

        private string hasFlowFlagField;

        private bool isInAdvanceField;

        private string inspEditableField;

        private System.Nullable<double> inspUnitsField;

        private string ivrNumberField;

        private System.Nullable<double> maxPointsField;

        private string priorityField;

        private System.Nullable<int> reScheduleDaysInACAField;

        private System.Nullable<int> reScheduleHoursInACAField;

        private string reScheduleOptionInACAField;

        private string reScheduleTimeInACAField;

        private string requiredInspectionField;

        private string resGroupCodeField;

        private string resGroupCodeNameField;

        private string resTypeField;

        private InspectionResultGroupModel resultGroupField;

        private long sequenceNumberField;

        private string serviceProviderCodeField;

        private int totalScoreField;

        private bool totalScoreFieldSpecified;

        private string totalScoreOptionField;

        private string typeField;

        private string unitNBRField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public bool isActive
        {
            get
            {
                return this.isActiveField;
            }
            set
            {
                this.isActiveField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string allowFailGuideSheetItems
        {
            get
            {
                return this.allowFailGuideSheetItemsField;
            }
            set
            {
                this.allowFailGuideSheetItemsField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string allowMultiInsp
        {
            get
            {
                return this.allowMultiInspField;
            }
            set
            {
                this.allowMultiInspField = value;
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
        public string autoAssign
        {
            get
            {
                return this.autoAssignField;
            }
            set
            {
                this.autoAssignField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public CalendarInspectionTypeModel calendarInspectionType
        {
            get
            {
                return this.calendarInspectionTypeField;
            }
            set
            {
                this.calendarInspectionTypeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = true)]
        public System.Nullable<int> cancelDaysInACA
        {
            get
            {
                return this.cancelDaysInACAField;
            }
            set
            {
                this.cancelDaysInACAField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = true)]
        public System.Nullable<int> cancelHoursInACA
        {
            get
            {
                return this.cancelHoursInACAField;
            }
            set
            {
                this.cancelHoursInACAField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string cancelOptionInACA
        {
            get
            {
                return this.cancelOptionInACAField;
            }
            set
            {
                this.cancelOptionInACAField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string cancelTimeInACA
        {
            get
            {
                return this.cancelTimeInACAField;
            }
            set
            {
                this.cancelTimeInACAField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string carryoverFlag
        {
            get
            {
                return this.carryoverFlagField;
            }
            set
            {
                this.carryoverFlagField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public bool isCompleted
        {
            get
            {
                return this.isCompletedField;
            }
            set
            {
                this.isCompletedField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public bool isConfiguredInInspFlow
        {
            get
            {
                return this.isConfiguredInInspFlowField;
            }
            set
            {
                this.isConfiguredInInspFlowField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string displayInACA
        {
            get
            {
                return this.displayInACAField;
            }
            set
            {
                this.displayInACAField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string flowEnabledFlag
        {
            get
            {
                return this.flowEnabledFlagField;
            }
            set
            {
                this.flowEnabledFlagField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string grade
        {
            get
            {
                return this.gradeField;
            }
            set
            {
                this.gradeField = value;
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
        public string groupCodeName
        {
            get
            {
                return this.groupCodeNameField;
            }
            set
            {
                this.groupCodeNameField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string hasFlowFlag
        {
            get
            {
                return this.hasFlowFlagField;
            }
            set
            {
                this.hasFlowFlagField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public bool isInAdvance
        {
            get
            {
                return this.isInAdvanceField;
            }
            set
            {
                this.isInAdvanceField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string inspEditable
        {
            get
            {
                return this.inspEditableField;
            }
            set
            {
                this.inspEditableField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = true)]
        public System.Nullable<double> inspUnits
        {
            get
            {
                return this.inspUnitsField;
            }
            set
            {
                this.inspUnitsField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string ivrNumber
        {
            get
            {
                return this.ivrNumberField;
            }
            set
            {
                this.ivrNumberField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = true)]
        public System.Nullable<double> maxPoints
        {
            get
            {
                return this.maxPointsField;
            }
            set
            {
                this.maxPointsField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string priority
        {
            get
            {
                return this.priorityField;
            }
            set
            {
                this.priorityField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = true)]
        public System.Nullable<int> reScheduleDaysInACA
        {
            get
            {
                return this.reScheduleDaysInACAField;
            }
            set
            {
                this.reScheduleDaysInACAField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = true)]
        public System.Nullable<int> reScheduleHoursInACA
        {
            get
            {
                return this.reScheduleHoursInACAField;
            }
            set
            {
                this.reScheduleHoursInACAField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string reScheduleOptionInACA
        {
            get
            {
                return this.reScheduleOptionInACAField;
            }
            set
            {
                this.reScheduleOptionInACAField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string reScheduleTimeInACA
        {
            get
            {
                return this.reScheduleTimeInACAField;
            }
            set
            {
                this.reScheduleTimeInACAField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string requiredInspection
        {
            get
            {
                return this.requiredInspectionField;
            }
            set
            {
                this.requiredInspectionField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string resGroupCode
        {
            get
            {
                return this.resGroupCodeField;
            }
            set
            {
                this.resGroupCodeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string resGroupCodeName
        {
            get
            {
                return this.resGroupCodeNameField;
            }
            set
            {
                this.resGroupCodeNameField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string resType
        {
            get
            {
                return this.resTypeField;
            }
            set
            {
                this.resTypeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public InspectionResultGroupModel resultGroup
        {
            get
            {
                return this.resultGroupField;
            }
            set
            {
                this.resultGroupField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public long sequenceNumber
        {
            get
            {
                return this.sequenceNumberField;
            }
            set
            {
                this.sequenceNumberField = value;
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
        public int totalScore
        {
            get
            {
                return this.totalScoreField;
            }
            set
            {
                this.totalScoreField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool totalScoreSpecified
        {
            get
            {
                return this.totalScoreFieldSpecified;
            }
            set
            {
                this.totalScoreFieldSpecified = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string totalScoreOption
        {
            get
            {
                return this.totalScoreOptionField;
            }
            set
            {
                this.totalScoreOptionField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string type
        {
            get
            {
                return this.typeField;
            }
            set
            {
                this.typeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string unitNBR
        {
            get
            {
                return this.unitNBRField;
            }
            set
            {
                this.unitNBRField = value;
            }
        }
    }
}
