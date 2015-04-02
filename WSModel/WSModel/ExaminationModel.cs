#region Header
/**
 * <pre>
 * 
 *  Accela Citizen Access
 *  File: ExaminationModel.cs
 * 
 *  Accela, Inc.
 *  Copyright (C): 2010-2013
 * 
 *  Description:
 * 
 *  Notes:
 * $Id: ExaminationModel.cs 181867 2010-10-06 08:06:18Z ACHIEVO\daly.zeng $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */
#endregion Header

namespace Accela.ACA.WSProxy
{
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.17929")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://model.webservice.accela.com")]
    public partial class ExaminationModel
    {
        private string approvedFlagField;

        private int associatedExamCountField;

        private SimpleAuditModel auditModelField;

        private string b1PerId1Field;

        private string b1PerId2Field;

        private string b1PerId3Field;

        private System.Nullable<long> calendarIDField;

        private string commentsField;

        private System.Nullable<long> contactSeqNumberField;

        private System.Nullable<System.DateTime> endTimeField;

        private System.Nullable<long> entityIDField;

        private string entityTypeField;

        private long examAttemptField;

        private bool examAttemptFieldSpecified;

        private System.Nullable<System.DateTime> examDateField;

        private string examNameField;

        private ExamProviderDetailModel examProviderDetailModelField;

        private string examStatusField;

        private ExaminationPKModel examinationPKModelField;

        private string externalUserIDField;

        private System.Nullable<double> finalScoreField;

        private string gradingStyleField;

        private System.Nullable<long> locationIDField;

        private System.Nullable<double> passingScoreField;

        private string providerNameField;

        private string providerNoField;

        private System.Nullable<long> refExamSeqField;

        private string requiredFlagField;

        private System.Nullable<long> scheduleIDField;

        private System.Nullable<System.DateTime> startTimeField;

        private string syncFlagField;

        private TemplateModel templateField;

        private int uIUIDField;

        private bool uIUIDFieldSpecified;

        private string userExamIDField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string approvedFlag
        {
            get
            {
                return this.approvedFlagField;
            }
            set
            {
                this.approvedFlagField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public int associatedExamCount
        {
            get
            {
                return this.associatedExamCountField;
            }
            set
            {
                this.associatedExamCountField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public SimpleAuditModel auditModel
        {
            get
            {
                return this.auditModelField;
            }
            set
            {
                this.auditModelField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string b1PerId1
        {
            get
            {
                return this.b1PerId1Field;
            }
            set
            {
                this.b1PerId1Field = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string b1PerId2
        {
            get
            {
                return this.b1PerId2Field;
            }
            set
            {
                this.b1PerId2Field = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string b1PerId3
        {
            get
            {
                return this.b1PerId3Field;
            }
            set
            {
                this.b1PerId3Field = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = true)]
        public System.Nullable<long> calendarID
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
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = true)]
        public System.Nullable<long> contactSeqNumber
        {
            get
            {
                return this.contactSeqNumberField;
            }
            set
            {
                this.contactSeqNumberField = value;
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
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = true)]
        public System.Nullable<long> entityID
        {
            get
            {
                return this.entityIDField;
            }
            set
            {
                this.entityIDField = value;
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
        public long examAttempt
        {
            get
            {
                return this.examAttemptField;
            }
            set
            {
                this.examAttemptField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool examAttemptSpecified
        {
            get
            {
                return this.examAttemptFieldSpecified;
            }
            set
            {
                this.examAttemptFieldSpecified = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = true)]
        public System.Nullable<System.DateTime> examDate
        {
            get
            {
                return this.examDateField;
            }
            set
            {
                this.examDateField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string examName
        {
            get
            {
                return this.examNameField;
            }
            set
            {
                this.examNameField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public ExamProviderDetailModel examProviderDetailModel
        {
            get
            {
                return this.examProviderDetailModelField;
            }
            set
            {
                this.examProviderDetailModelField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string examStatus
        {
            get
            {
                return this.examStatusField;
            }
            set
            {
                this.examStatusField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public ExaminationPKModel examinationPKModel
        {
            get
            {
                return this.examinationPKModelField;
            }
            set
            {
                this.examinationPKModelField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string externalUserID
        {
            get
            {
                return this.externalUserIDField;
            }
            set
            {
                this.externalUserIDField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = true)]
        public System.Nullable<double> finalScore
        {
            get
            {
                return this.finalScoreField;
            }
            set
            {
                this.finalScoreField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string gradingStyle
        {
            get
            {
                return this.gradingStyleField;
            }
            set
            {
                this.gradingStyleField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = true)]
        public System.Nullable<long> locationID
        {
            get
            {
                return this.locationIDField;
            }
            set
            {
                this.locationIDField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = true)]
        public System.Nullable<double> passingScore
        {
            get
            {
                return this.passingScoreField;
            }
            set
            {
                this.passingScoreField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string providerName
        {
            get
            {
                return this.providerNameField;
            }
            set
            {
                this.providerNameField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string providerNo
        {
            get
            {
                return this.providerNoField;
            }
            set
            {
                this.providerNoField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = true)]
        public System.Nullable<long> refExamSeq
        {
            get
            {
                return this.refExamSeqField;
            }
            set
            {
                this.refExamSeqField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string requiredFlag
        {
            get
            {
                return this.requiredFlagField;
            }
            set
            {
                this.requiredFlagField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = true)]
        public System.Nullable<long> scheduleID
        {
            get
            {
                return this.scheduleIDField;
            }
            set
            {
                this.scheduleIDField = value;
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
        public string syncFlag
        {
            get
            {
                return this.syncFlagField;
            }
            set
            {
                this.syncFlagField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public TemplateModel template
        {
            get
            {
                return this.templateField;
            }
            set
            {
                this.templateField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public int UIUID
        {
            get
            {
                return this.uIUIDField;
            }
            set
            {
                this.uIUIDField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool UIUIDSpecified
        {
            get
            {
                return this.uIUIDFieldSpecified;
            }
            set
            {
                this.uIUIDFieldSpecified = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string userExamID
        {
            get
            {
                return this.userExamIDField;
            }
            set
            {
                this.userExamIDField = value;
            }
        }
    }
}
