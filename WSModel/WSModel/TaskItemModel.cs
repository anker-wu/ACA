#region Header
/**
 * <pre>
 * 
 *  Accela Citizen Access
 *  File: TaskItemModel.cs
 * 
 *  Accela, Inc.
 *  Copyright (C): 2010
 * 
 *  Description:
 * 
 *  Notes:
 * $Id: TaskItemModel.cs 181867 2010-10-06 08:06:18Z ACHIEVO\daly.zeng $.
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
    public partial class TaskItemModel : LanguageModel
    {
        private string actionDepartmentNameField;

        private string activeFlagField;

        private string adHocNameField;

        private string appStatusField;

        private string appStatusGroupCodeField;

        private string approvalField;

        private string asgnDeptField;

        private string asgnEmailDispField;

        private string asgnStaffField;

        private string assignDepartmentNameField;

        private string assignedStatusField;

        private SysUserModel assignedUserField;

        private System.DateTime assignmentDateField;

        private bool assignmentDateFieldSpecified;

        private string assignmentDateStringField;

        private System.DateTime auditDateField;

        private bool auditDateFieldSpecified;

        private string auditDateStringField;

        private string auditIDField;

        private string auditStatusField;

        private string billableField;

        private long calendarIDField;

        private bool calendarIDFieldSpecified;

        private string calendarIdStringField;

        private CapIDModel capIDField;

        private string checkboxCodeField;

        private string checkboxGroupField;

        private string completeFlagField;

        private string currentTaskIDField;

        private string customProcedureField;

        private int daysDueField;

        private string daysDueStringField;

        private string deleteFlagField;

        private string displayInACAField;

        private TimeLogModel[] displayTimeModelsField;

        private string dispositionField;

        private string dispositionCommentField;

        private System.DateTime dispositionDateField;

        private bool dispositionDateFieldSpecified;

        private string dispositionDateStringField;

        private System.DateTime dispositionEndDateField;

        private bool dispositionEndDateFieldSpecified;

        private string dispositionNoteField;

        private System.DateTime dueDateField;

        private bool dueDateFieldSpecified;

        private string dueDateStringField;

        private System.DateTime endAssignmentDateField;

        private bool endAssignmentDateFieldSpecified;

        private System.DateTime endDispositionDateField;

        private bool endDispositionDateFieldSpecified;

        private System.DateTime endDueDateField;

        private bool endDueDateFieldSpecified;

        private System.DateTime endTimeField;

        private bool endTimeFieldSpecified;

        private System.DateTime estimatedDueDateField;

        private bool estimatedDueDateFieldSpecified;

        private TimeLogModel[] existTimeModelsField;

        private string generalFlagField;

        private string hoursSpentField;

        private string hoursSpentRequiredField;

        private double inPossessionTimeField;

        private bool inPossessionTimeFieldSpecified;

        private string isRestrictView4ACAField;

        private string langIDField;

        private string loopCountField;

        private string mouClockActionField;

        private string nextTaskIDField;

        private string overTimeField;

        private string parallelIndField;

        private string parentTaskNameField;

        private string processCodeField;

        private long processHistorySeqField;

        private long processIDField;

        private string refActStatusFlagField;

        private string resDispositionField;

        private string resDispositionCommentField;

        private string restrictRoleField;

        private string serviceProviderCodeField;

        private string shortNotesField;

        private StandardCommentModel[] standardCommentsField;

        private System.DateTime startTimeField;

        private bool startTimeFieldSpecified;

        private System.DateTime statusDateField;

        private bool statusDateFieldSpecified;

        private string statusDateStringField;

        private System.DateTime statusEndDateField;

        private bool statusEndDateFieldSpecified;

        private int stepNumberField;

        private SysUserModel sysUserField;

        private string taskDescriptionField;

        private System.DateTime timerStartTimeField;

        private bool timerStartTimeFieldSpecified;

        private System.DateTime trackStartDateField;

        private bool trackStartDateFieldSpecified;

        private UserRolePrivilegeModel userRolePrivilegeModelField;

        private string workflowIDField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string actionDepartmentName
        {
            get
            {
                return this.actionDepartmentNameField;
            }
            set
            {
                this.actionDepartmentNameField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string activeFlag
        {
            get
            {
                return this.activeFlagField;
            }
            set
            {
                this.activeFlagField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string adHocName
        {
            get
            {
                return this.adHocNameField;
            }
            set
            {
                this.adHocNameField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string appStatus
        {
            get
            {
                return this.appStatusField;
            }
            set
            {
                this.appStatusField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string appStatusGroupCode
        {
            get
            {
                return this.appStatusGroupCodeField;
            }
            set
            {
                this.appStatusGroupCodeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string approval
        {
            get
            {
                return this.approvalField;
            }
            set
            {
                this.approvalField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string asgnDept
        {
            get
            {
                return this.asgnDeptField;
            }
            set
            {
                this.asgnDeptField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string asgnEmailDisp
        {
            get
            {
                return this.asgnEmailDispField;
            }
            set
            {
                this.asgnEmailDispField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string asgnStaff
        {
            get
            {
                return this.asgnStaffField;
            }
            set
            {
                this.asgnStaffField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string assignDepartmentName
        {
            get
            {
                return this.assignDepartmentNameField;
            }
            set
            {
                this.assignDepartmentNameField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string assignedStatus
        {
            get
            {
                return this.assignedStatusField;
            }
            set
            {
                this.assignedStatusField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public SysUserModel assignedUser
        {
            get
            {
                return this.assignedUserField;
            }
            set
            {
                this.assignedUserField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public System.DateTime assignmentDate
        {
            get
            {
                return this.assignmentDateField;
            }
            set
            {
                this.assignmentDateField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool assignmentDateSpecified
        {
            get
            {
                return this.assignmentDateFieldSpecified;
            }
            set
            {
                this.assignmentDateFieldSpecified = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string assignmentDateString
        {
            get
            {
                return this.assignmentDateStringField;
            }
            set
            {
                this.assignmentDateStringField = value;
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
        public string auditDateString
        {
            get
            {
                return this.auditDateStringField;
            }
            set
            {
                this.auditDateStringField = value;
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
        public long calendarID
        {
            get
            {
                return this.calendarIDField;
            }
            set
            {
                this.calendarIDField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool calendarIDSpecified
        {
            get
            {
                return this.calendarIDFieldSpecified;
            }
            set
            {
                this.calendarIDFieldSpecified = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string calendarIdString
        {
            get
            {
                return this.calendarIdStringField;
            }
            set
            {
                this.calendarIdStringField = value;
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
        public string checkboxCode
        {
            get
            {
                return this.checkboxCodeField;
            }
            set
            {
                this.checkboxCodeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string checkboxGroup
        {
            get
            {
                return this.checkboxGroupField;
            }
            set
            {
                this.checkboxGroupField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string completeFlag
        {
            get
            {
                return this.completeFlagField;
            }
            set
            {
                this.completeFlagField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string currentTaskID
        {
            get
            {
                return this.currentTaskIDField;
            }
            set
            {
                this.currentTaskIDField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string customProcedure
        {
            get
            {
                return this.customProcedureField;
            }
            set
            {
                this.customProcedureField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public int daysDue
        {
            get
            {
                return this.daysDueField;
            }
            set
            {
                this.daysDueField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string daysDueString
        {
            get
            {
                return this.daysDueStringField;
            }
            set
            {
                this.daysDueStringField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string deleteFlag
        {
            get
            {
                return this.deleteFlagField;
            }
            set
            {
                this.deleteFlagField = value;
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
        [System.Xml.Serialization.XmlElementAttribute("displayTimeModels", Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = true)]
        public TimeLogModel[] displayTimeModels
        {
            get
            {
                return this.displayTimeModelsField;
            }
            set
            {
                this.displayTimeModelsField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string disposition
        {
            get
            {
                return this.dispositionField;
            }
            set
            {
                this.dispositionField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string dispositionComment
        {
            get
            {
                return this.dispositionCommentField;
            }
            set
            {
                this.dispositionCommentField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public System.DateTime dispositionDate
        {
            get
            {
                return this.dispositionDateField;
            }
            set
            {
                this.dispositionDateField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool dispositionDateSpecified
        {
            get
            {
                return this.dispositionDateFieldSpecified;
            }
            set
            {
                this.dispositionDateFieldSpecified = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string dispositionDateString
        {
            get
            {
                return this.dispositionDateStringField;
            }
            set
            {
                this.dispositionDateStringField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public System.DateTime dispositionEndDate
        {
            get
            {
                return this.dispositionEndDateField;
            }
            set
            {
                this.dispositionEndDateField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool dispositionEndDateSpecified
        {
            get
            {
                return this.dispositionEndDateFieldSpecified;
            }
            set
            {
                this.dispositionEndDateFieldSpecified = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string dispositionNote
        {
            get
            {
                return this.dispositionNoteField;
            }
            set
            {
                this.dispositionNoteField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public System.DateTime dueDate
        {
            get
            {
                return this.dueDateField;
            }
            set
            {
                this.dueDateField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool dueDateSpecified
        {
            get
            {
                return this.dueDateFieldSpecified;
            }
            set
            {
                this.dueDateFieldSpecified = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string dueDateString
        {
            get
            {
                return this.dueDateStringField;
            }
            set
            {
                this.dueDateStringField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public System.DateTime endAssignmentDate
        {
            get
            {
                return this.endAssignmentDateField;
            }
            set
            {
                this.endAssignmentDateField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool endAssignmentDateSpecified
        {
            get
            {
                return this.endAssignmentDateFieldSpecified;
            }
            set
            {
                this.endAssignmentDateFieldSpecified = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public System.DateTime endDispositionDate
        {
            get
            {
                return this.endDispositionDateField;
            }
            set
            {
                this.endDispositionDateField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool endDispositionDateSpecified
        {
            get
            {
                return this.endDispositionDateFieldSpecified;
            }
            set
            {
                this.endDispositionDateFieldSpecified = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public System.DateTime endDueDate
        {
            get
            {
                return this.endDueDateField;
            }
            set
            {
                this.endDueDateField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool endDueDateSpecified
        {
            get
            {
                return this.endDueDateFieldSpecified;
            }
            set
            {
                this.endDueDateFieldSpecified = value;
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
        public System.DateTime estimatedDueDate
        {
            get
            {
                return this.estimatedDueDateField;
            }
            set
            {
                this.estimatedDueDateField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool estimatedDueDateSpecified
        {
            get
            {
                return this.estimatedDueDateFieldSpecified;
            }
            set
            {
                this.estimatedDueDateFieldSpecified = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("existTimeModels", Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = true)]
        public TimeLogModel[] existTimeModels
        {
            get
            {
                return this.existTimeModelsField;
            }
            set
            {
                this.existTimeModelsField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string generalFlag
        {
            get
            {
                return this.generalFlagField;
            }
            set
            {
                this.generalFlagField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string hoursSpent
        {
            get
            {
                return this.hoursSpentField;
            }
            set
            {
                this.hoursSpentField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string hoursSpentRequired
        {
            get
            {
                return this.hoursSpentRequiredField;
            }
            set
            {
                this.hoursSpentRequiredField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public double inPossessionTime
        {
            get
            {
                return this.inPossessionTimeField;
            }
            set
            {
                this.inPossessionTimeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool inPossessionTimeSpecified
        {
            get
            {
                return this.inPossessionTimeFieldSpecified;
            }
            set
            {
                this.inPossessionTimeFieldSpecified = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string isRestrictView4ACA
        {
            get
            {
                return this.isRestrictView4ACAField;
            }
            set
            {
                this.isRestrictView4ACAField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string langID
        {
            get
            {
                return this.langIDField;
            }
            set
            {
                this.langIDField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string loopCount
        {
            get
            {
                return this.loopCountField;
            }
            set
            {
                this.loopCountField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string mouClockAction
        {
            get
            {
                return this.mouClockActionField;
            }
            set
            {
                this.mouClockActionField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string nextTaskID
        {
            get
            {
                return this.nextTaskIDField;
            }
            set
            {
                this.nextTaskIDField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string overTime
        {
            get
            {
                return this.overTimeField;
            }
            set
            {
                this.overTimeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string parallelInd
        {
            get
            {
                return this.parallelIndField;
            }
            set
            {
                this.parallelIndField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string parentTaskName
        {
            get
            {
                return this.parentTaskNameField;
            }
            set
            {
                this.parentTaskNameField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string processCode
        {
            get
            {
                return this.processCodeField;
            }
            set
            {
                this.processCodeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public long processHistorySeq
        {
            get
            {
                return this.processHistorySeqField;
            }
            set
            {
                this.processHistorySeqField = value;
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
        public string refActStatusFlag
        {
            get
            {
                return this.refActStatusFlagField;
            }
            set
            {
                this.refActStatusFlagField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string resDisposition
        {
            get
            {
                return this.resDispositionField;
            }
            set
            {
                this.resDispositionField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string resDispositionComment
        {
            get
            {
                return this.resDispositionCommentField;
            }
            set
            {
                this.resDispositionCommentField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string restrictRole
        {
            get
            {
                return this.restrictRoleField;
            }
            set
            {
                this.restrictRoleField = value;
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
        public string shortNotes
        {
            get
            {
                return this.shortNotesField;
            }
            set
            {
                this.shortNotesField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("standardComments", Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = true)]
        public StandardCommentModel[] standardComments
        {
            get
            {
                return this.standardCommentsField;
            }
            set
            {
                this.standardCommentsField = value;
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
        public System.DateTime statusDate
        {
            get
            {
                return this.statusDateField;
            }
            set
            {
                this.statusDateField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool statusDateSpecified
        {
            get
            {
                return this.statusDateFieldSpecified;
            }
            set
            {
                this.statusDateFieldSpecified = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string statusDateString
        {
            get
            {
                return this.statusDateStringField;
            }
            set
            {
                this.statusDateStringField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public System.DateTime statusEndDate
        {
            get
            {
                return this.statusEndDateField;
            }
            set
            {
                this.statusEndDateField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool statusEndDateSpecified
        {
            get
            {
                return this.statusEndDateFieldSpecified;
            }
            set
            {
                this.statusEndDateFieldSpecified = value;
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
        public SysUserModel sysUser
        {
            get
            {
                return this.sysUserField;
            }
            set
            {
                this.sysUserField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string taskDescription
        {
            get
            {
                return this.taskDescriptionField;
            }
            set
            {
                this.taskDescriptionField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public System.DateTime timerStartTime
        {
            get
            {
                return this.timerStartTimeField;
            }
            set
            {
                this.timerStartTimeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool timerStartTimeSpecified
        {
            get
            {
                return this.timerStartTimeFieldSpecified;
            }
            set
            {
                this.timerStartTimeFieldSpecified = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public System.DateTime trackStartDate
        {
            get
            {
                return this.trackStartDateField;
            }
            set
            {
                this.trackStartDateField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool trackStartDateSpecified
        {
            get
            {
                return this.trackStartDateFieldSpecified;
            }
            set
            {
                this.trackStartDateFieldSpecified = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public UserRolePrivilegeModel userRolePrivilegeModel
        {
            get
            {
                return this.userRolePrivilegeModelField;
            }
            set
            {
                this.userRolePrivilegeModelField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string workflowID
        {
            get
            {
                return this.workflowIDField;
            }
            set
            {
                this.workflowIDField = value;
            }
        }
    }
}
