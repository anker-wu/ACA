/**
 * <pre>
 * 
 *  Accela Citizen Access
 *  File: TaskItemModel4WS.cs
 * 
 *  Accela, Inc.
 *  Copyright (C): 2007-2011
 * 
 *  Description:
 * 
 *  Notes:
 * $Id: TaskItemModel4WS.cs 209458 2011-12-12 06:03:07Z ACHIEVO\daly.zeng $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

using Accela.ACA.WSProxy.WSModel;

namespace Accela.ACA.WSProxy
{
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.1433")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://model.webservice.accela.com")]
    public partial class TaskItemModel4WS
    {

        private string actionDepartmentNameField;

        private string activeFlagField;

        private string adHocNameField;

        private string approvalField;

        private string asgnEmailDispField;

        private string assignDepartmentNameField;

        private SysUserModel4WS assignedUserField;

        private string assignmentDateField;

        private string assignmentDateStringField;

        private string auditDateField;

        private string auditDateStringField;

        private string auditIDField;

        private string auditStatusField;

        private string billableField;

        private string calendarIDField;

        private string calendarIdStringField;

        private CapIDModel4WS capIDField;

        private string checkboxCodeField;

        private string checkboxGroupField;

        private string completeFlagField;

        private string currentTaskIDField;

        private string customProcedureField;

        private int daysDueField;

        private string daysDueStringField;

        private string dispositionField;

        private string dispositionCommentField;

        private string dispositionDateField;

        private string dispositionDateStringField;

        private string dispositionEndDateField;

        private string dispositionNoteField;

        private string dueDateField;

        private string dueDateStringField;

        private string endAssignmentDateField;

        private string endDispositionDateField;

        private string endDueDateField;

        private string endTimeField;

        private string estimatedDueDateField;

        private string generalFlagField;

        private TaskItemModel4WS[] historyTaskItemsField;

        private string hoursSpentField;

        private string hoursSpentRequiredField;

        private string inPossessionTimeField;

        private string isRestrictView4ACAField;

        private string loopCountField;

        private string mouClockActionField;

        private string nextTaskIDField;

        private string overTimeField;

        private string parallelIndField;

        private string parentTaskNameField;

        private string processCodeField;

        private long processIDField;

        private string resDispositionField;

        private string resDispositionCommentField;

        private string resTaskDescriptionField;

        private string restrictRoleField;

        private string serviceProviderCodeField;

        private StandardCommentModel4WS[] standardCommentsField;

        private string startTimeField;

        private string statusDateField;

        private string statusDateStringField;

        private string statusEndDateField;

        private int stepNumberField;

        private SysUserModel4WS sysUserField;

        private string taskDescriptionField;

        private TaskSpecificInfoModel4WS[] taskSpecItemsField;

        private string timerStartTimeField;

        private string trackStartDateField;

        private UserRolePrivilegeModel userRolePrivilegeModelField;

        private string workflowIDField;

        private string refActStatusFlagField;

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
        public SysUserModel4WS assignedUser
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
        public string assignmentDate
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
        public string auditDate
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
        public string calendarID
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
        public CapIDModel4WS capID
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
        public string dispositionDate
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
        public string dispositionEndDate
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
        public string dueDate
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
        public string endAssignmentDate
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
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string endDispositionDate
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
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string endDueDate
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
        public string estimatedDueDate
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
        [System.Xml.Serialization.XmlElementAttribute("historyTaskItems", Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = true)]
        public TaskItemModel4WS[] historyTaskItems
        {
            get
            {
                return this.historyTaskItemsField;
            }
            set
            {
                this.historyTaskItemsField = value;
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
        public string inPossessionTime
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
        public string resTaskDescription
        {
            get
            {
                return this.resTaskDescriptionField;
            }
            set
            {
                this.resTaskDescriptionField = value;
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
        [System.Xml.Serialization.XmlElementAttribute("standardComments", Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = true)]
        public StandardCommentModel4WS[] standardComments
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
        public string statusDate
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
        public string statusEndDate
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
        public SysUserModel4WS sysUser
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
        [System.Xml.Serialization.XmlElementAttribute("taskSpecItems", Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = true)]
        public TaskSpecificInfoModel4WS[] taskSpecItems
        {
            get
            {
                return this.taskSpecItemsField;
            }
            set
            {
                this.taskSpecItemsField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string timerStartTime
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
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string trackStartDate
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
    }
}
