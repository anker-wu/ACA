#region Header
/**
 * <pre>
 * 
 *  Accela Citizen Access
 *  File: ActivityModel.cs
 * 
 *  Accela, Inc.
 *  Copyright (C): 2010-2011
 * 
 *  Description:
 * 
 *  Notes:
 * $Id: ActivityModel.cs 181867 2010-10-06 08:06:18Z ACHIEVO\daly.zeng $.
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
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://model.webservice.accela.com")]
    public partial class ActivityModel : LanguageModel
    {

        private string actEndTime1Field;

        private string actEndTime2Field;

        private System.Nullable<System.DateTime> activityDateField;

        private string activityDescriptionField;

        private string activityGroupField;

        private string activityJvalField;

        private string activityTypeField;

        private System.Nullable<System.DateTime> auditDateField;

        private string auditIDField;

        private string auditStatusField;

        private string autoAssignField;

        private System.Nullable<long> calendarIdField;

        private CapIDModel capIDField;

        private CapIDModel capIDModelField;

        private string capStatusField;

        private CapTypeModel capTypeField;

        private string carryoverFlagField;

        private string completeTime1Field;

        private string completeTime2Field;

        private System.Nullable<System.DateTime> completionDateField;

        private string contactFnameField;

        private string contactLnameField;

        private string contactMnameField;

        private string contactNbrField;

        private string contactPhoneNumField;

        private string contactPhoneNumIDDField;

        private string createdByACAField;

        private string displayInACAField;

        private string docCategoryField;

        private string docGroupField;

        private string documentDescriptionField;

        private string documentIDField;

        private DocumentModel documentModelField;

        private string documentNameField;

        private System.Nullable<System.DateTime> endActivityDateField;

        private System.Nullable<System.DateTime> endCompletionDateField;

        private System.Nullable<float> endMilageField;

        private System.Nullable<System.DateTime> endRecordDateField;

        private System.Nullable<System.DateTime> endStatusDateField;

        private System.Nullable<System.DateTime> endTimeField;

        private string estimatedEndTimeField;

        private string estimatedStartTimeField;

        private System.Nullable<System.DateTime> desiredDateField;

        private string desiredTime2Field;

        private string desiredTimeField;

        private string fileKeyField;

        private string fileNameField;

        private string gisAreaNameField;

        private string gradeField;

        private long idNumberField;

        private string inAdvanceFlagField;

        private string inspBillableField;

        private string inspResultTypeField;

        private System.Nullable<long> inspSequenceNumberField;

        private System.Nullable<double> inspUnitsField;

        private string inspectionGroupField;

        private string isBySupervisorField;

        private string isRestrictView4ACAField;

        private System.Nullable<float> latitudeField;

        private System.Nullable<float> longitudeField;

        private int majorViolationField;

        private System.Nullable<double> maxPointsField;

        private System.Nullable<float> milageField;

        private string oldStatusField;

        private string overtimeField;

        private System.Nullable<System.DateTime> recordDateField;

        private string recordDescriptionField;

        private string recordTime1Field;

        private string recordTime2Field;

        private string recordTypeField;

        private string reqPhoneNumField;

        private string reqPhoneNumIDDField;

        private string requestorFnameField;

        private string requestorLnameField;

        private string requestorMnameField;

        private string requestorUserIDField;

        private string requiredInspectionField;

        private string resActivityTypeField;

        private string resStatusField;

        private string restrictRoleField;

        private bool scheduledField;

        private string serviceProviderCodeField;

        private bool signOffWorkflowTaskField;

        private string sourceField;

        private System.Nullable<float> startMilageField;

        private System.Nullable<System.DateTime> startTimeField;

        private string statusField;

        private System.Nullable<System.DateTime> statusDateField;

        private string statusTime1Field;

        private string statusTime2Field;

        private SysUserModel sysUserField;

        private string time1Field;

        private string time2Field;

        private System.Nullable<double> timeTotalField;

        private long totalScoreField;

        private string unitNBRField;

        private UserRolePrivilegeModel userRolePrivilegeModelField;

        private string vehicleIDField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string actEndTime1
        {
            get
            {
                return this.actEndTime1Field;
            }
            set
            {
                this.actEndTime1Field = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string actEndTime2
        {
            get
            {
                return this.actEndTime2Field;
            }
            set
            {
                this.actEndTime2Field = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = true)]
        public System.Nullable<System.DateTime> activityDate
        {
            get
            {
                return this.activityDateField;
            }
            set
            {
                this.activityDateField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string activityDescription
        {
            get
            {
                return this.activityDescriptionField;
            }
            set
            {
                this.activityDescriptionField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string activityGroup
        {
            get
            {
                return this.activityGroupField;
            }
            set
            {
                this.activityGroupField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string activityJval
        {
            get
            {
                return this.activityJvalField;
            }
            set
            {
                this.activityJvalField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string activityType
        {
            get
            {
                return this.activityTypeField;
            }
            set
            {
                this.activityTypeField = value;
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
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = true)]
        public System.Nullable<long> calendarId
        {
            get
            {
                return this.calendarIdField;
            }
            set
            {
                this.calendarIdField = value;
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
        public string capStatus
        {
            get
            {
                return this.capStatusField;
            }
            set
            {
                this.capStatusField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public CapTypeModel capType
        {
            get
            {
                return this.capTypeField;
            }
            set
            {
                this.capTypeField = value;
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
        public string completeTime1
        {
            get
            {
                return this.completeTime1Field;
            }
            set
            {
                this.completeTime1Field = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string completeTime2
        {
            get
            {
                return this.completeTime2Field;
            }
            set
            {
                this.completeTime2Field = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = true)]
        public System.Nullable<System.DateTime> completionDate
        {
            get
            {
                return this.completionDateField;
            }
            set
            {
                this.completionDateField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string contactFname
        {
            get
            {
                return this.contactFnameField;
            }
            set
            {
                this.contactFnameField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string contactLname
        {
            get
            {
                return this.contactLnameField;
            }
            set
            {
                this.contactLnameField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string contactMname
        {
            get
            {
                return this.contactMnameField;
            }
            set
            {
                this.contactMnameField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string contactNbr
        {
            get
            {
                return this.contactNbrField;
            }
            set
            {
                this.contactNbrField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string contactPhoneNum
        {
            get
            {
                return this.contactPhoneNumField;
            }
            set
            {
                this.contactPhoneNumField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string contactPhoneNumIDD
        {
            get
            {
                return this.contactPhoneNumIDDField;
            }
            set
            {
                this.contactPhoneNumIDDField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string createdByACA
        {
            get
            {
                return this.createdByACAField;
            }
            set
            {
                this.createdByACAField = value;
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
        public string docCategory
        {
            get
            {
                return this.docCategoryField;
            }
            set
            {
                this.docCategoryField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string docGroup
        {
            get
            {
                return this.docGroupField;
            }
            set
            {
                this.docGroupField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string documentDescription
        {
            get
            {
                return this.documentDescriptionField;
            }
            set
            {
                this.documentDescriptionField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string documentID
        {
            get
            {
                return this.documentIDField;
            }
            set
            {
                this.documentIDField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public DocumentModel documentModel
        {
            get
            {
                return this.documentModelField;
            }
            set
            {
                this.documentModelField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string documentName
        {
            get
            {
                return this.documentNameField;
            }
            set
            {
                this.documentNameField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = true)]
        public System.Nullable<System.DateTime> endActivityDate
        {
            get
            {
                return this.endActivityDateField;
            }
            set
            {
                this.endActivityDateField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = true)]
        public System.Nullable<System.DateTime> endCompletionDate
        {
            get
            {
                return this.endCompletionDateField;
            }
            set
            {
                this.endCompletionDateField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = true)]
        public System.Nullable<float> endMilage
        {
            get
            {
                return this.endMilageField;
            }
            set
            {
                this.endMilageField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = true)]
        public System.Nullable<System.DateTime> endRecordDate
        {
            get
            {
                return this.endRecordDateField;
            }
            set
            {
                this.endRecordDateField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = true)]
        public System.Nullable<System.DateTime> endStatusDate
        {
            get
            {
                return this.endStatusDateField;
            }
            set
            {
                this.endStatusDateField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = true)]
        public System.Nullable<System.DateTime> endTime
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
        public string estimatedEndTime
        {
            get
            {
                return this.estimatedEndTimeField;
            }
            set
            {
                this.estimatedEndTimeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string estimatedStartTime
        {
            get
            {
                return this.estimatedStartTimeField;
            }
            set
            {
                this.estimatedStartTimeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string fileKey
        {
            get
            {
                return this.fileKeyField;
            }
            set
            {
                this.fileKeyField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string fileName
        {
            get
            {
                return this.fileNameField;
            }
            set
            {
                this.fileNameField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string gisAreaName
        {
            get
            {
                return this.gisAreaNameField;
            }
            set
            {
                this.gisAreaNameField = value;
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
        public long idNumber
        {
            get
            {
                return this.idNumberField;
            }
            set
            {
                this.idNumberField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string inAdvanceFlag
        {
            get
            {
                return this.inAdvanceFlagField;
            }
            set
            {
                this.inAdvanceFlagField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string inspBillable
        {
            get
            {
                return this.inspBillableField;
            }
            set
            {
                this.inspBillableField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string inspResultType
        {
            get
            {
                return this.inspResultTypeField;
            }
            set
            {
                this.inspResultTypeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = true)]
        public System.Nullable<long> inspSequenceNumber
        {
            get
            {
                return this.inspSequenceNumberField;
            }
            set
            {
                this.inspSequenceNumberField = value;
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
        public string inspectionGroup
        {
            get
            {
                return this.inspectionGroupField;
            }
            set
            {
                this.inspectionGroupField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string isBySupervisor
        {
            get
            {
                return this.isBySupervisorField;
            }
            set
            {
                this.isBySupervisorField = value;
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
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = true)]
        public System.Nullable<float> latitude
        {
            get
            {
                return this.latitudeField;
            }
            set
            {
                this.latitudeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = true)]
        public System.Nullable<float> longitude
        {
            get
            {
                return this.longitudeField;
            }
            set
            {
                this.longitudeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public int majorViolation
        {
            get
            {
                return this.majorViolationField;
            }
            set
            {
                this.majorViolationField = value;
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
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = true)]
        public System.Nullable<float> milage
        {
            get
            {
                return this.milageField;
            }
            set
            {
                this.milageField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string oldStatus
        {
            get
            {
                return this.oldStatusField;
            }
            set
            {
                this.oldStatusField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string overtime
        {
            get
            {
                return this.overtimeField;
            }
            set
            {
                this.overtimeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = true)]
        public System.Nullable<System.DateTime> recordDate
        {
            get
            {
                return this.recordDateField;
            }
            set
            {
                this.recordDateField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string recordDescription
        {
            get
            {
                return this.recordDescriptionField;
            }
            set
            {
                this.recordDescriptionField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string recordTime1
        {
            get
            {
                return this.recordTime1Field;
            }
            set
            {
                this.recordTime1Field = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string recordTime2
        {
            get
            {
                return this.recordTime2Field;
            }
            set
            {
                this.recordTime2Field = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string recordType
        {
            get
            {
                return this.recordTypeField;
            }
            set
            {
                this.recordTypeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string reqPhoneNum
        {
            get
            {
                return this.reqPhoneNumField;
            }
            set
            {
                this.reqPhoneNumField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string reqPhoneNumIDD
        {
            get
            {
                return this.reqPhoneNumIDDField;
            }
            set
            {
                this.reqPhoneNumIDDField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string requestorFname
        {
            get
            {
                return this.requestorFnameField;
            }
            set
            {
                this.requestorFnameField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string requestorLname
        {
            get
            {
                return this.requestorLnameField;
            }
            set
            {
                this.requestorLnameField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string requestorMname
        {
            get
            {
                return this.requestorMnameField;
            }
            set
            {
                this.requestorMnameField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string requestorUserID
        {
            get
            {
                return this.requestorUserIDField;
            }
            set
            {
                this.requestorUserIDField = value;
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
        public string resActivityType
        {
            get
            {
                return this.resActivityTypeField;
            }
            set
            {
                this.resActivityTypeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string resStatus
        {
            get
            {
                return this.resStatusField;
            }
            set
            {
                this.resStatusField = value;
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
        public bool scheduled
        {
            get
            {
                return this.scheduledField;
            }
            set
            {
                this.scheduledField = value;
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
        public bool signOffWorkflowTask
        {
            get
            {
                return this.signOffWorkflowTaskField;
            }
            set
            {
                this.signOffWorkflowTaskField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string source
        {
            get
            {
                return this.sourceField;
            }
            set
            {
                this.sourceField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = true)]
        public System.Nullable<float> startMilage
        {
            get
            {
                return this.startMilageField;
            }
            set
            {
                this.startMilageField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = true)]
        public System.Nullable<System.DateTime> startTime
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
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = true)]
        public System.Nullable<System.DateTime> statusDate
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
        public string statusTime1
        {
            get
            {
                return this.statusTime1Field;
            }
            set
            {
                this.statusTime1Field = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string statusTime2
        {
            get
            {
                return this.statusTime2Field;
            }
            set
            {
                this.statusTime2Field = value;
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
        public string time1
        {
            get
            {
                return this.time1Field;
            }
            set
            {
                this.time1Field = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string time2
        {
            get
            {
                return this.time2Field;
            }
            set
            {
                this.time2Field = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = true)]
        public System.Nullable<double> timeTotal
        {
            get
            {
                return this.timeTotalField;
            }
            set
            {
                this.timeTotalField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public long totalScore
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
        public string vehicleID
        {
            get
            {
                return this.vehicleIDField;
            }
            set
            {
                this.vehicleIDField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public System.Nullable<System.DateTime> desiredDate
        {
            get
            {
                return this.desiredDateField;
            }
            set
            {
                this.desiredDateField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string desiredTime2
        {
            get
            {
                return this.desiredTime2Field;
            }
            set
            {
                this.desiredTime2Field = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string desiredTime
        {
            get
            {
                return this.desiredTimeField;
            }
            set
            {
                this.desiredTimeField = value;
            }
        }
    }
}