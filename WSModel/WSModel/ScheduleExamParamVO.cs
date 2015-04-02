#region Header
/**
 * <pre>
 * 
 *  Accela Citizen Access
 *  File: ScheduleExamParamVO.cs
 * 
 *  Accela, Inc.
 *  Copyright (C): 2011-2013
 * 
 *  Description:
 * 
 *  Notes:
 * $Id: ScheduleExamParamVO.cs 181867 2011-09-27 08:06:18Z ACHIEVO\daly.zeng $.
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
    public partial class ScheduleExamParamVO
    {

        private System.Nullable<long> calendarIDField;

        private string callerIDField;

        private CapIDModel capIDField;

        private System.Nullable<System.DateTime> endTimeField;

        private System.Nullable<System.DateTime> examDateField;

        private string examNameField;

        private System.Nullable<long> examSeqNbrField;

        private bool existingACAUserField;

        private bool fromACAField;

        private string inputEmailField;

        private bool isBeyondAllowanceDateField;

        private bool isFromReferenceSideField;

        private System.Nullable<long> locationIDField;

        private System.Nullable<long> proctorIDField;

        private long providerNbrField;

        private System.Nullable<long> refExamNbrField;

        private string reschedCancelReasonField;

        private System.Nullable<long> rosterIDField;

        private bool sameProviderForRescheduleField;

        private System.Nullable<System.DateTime> scheduleDateField;

        private System.Nullable<long> scheduleIDField;

        private string scheduleTypeField;

        private string serviceProviderCodeField;

        private System.Nullable<System.DateTime> startTimeField;

        private StudentInfoVO studentInfoField;

        private TemplateModel templateField;

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
        public string callerID
        {
            get
            {
                return this.callerIDField;
            }
            set
            {
                this.callerIDField = value;
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
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = true)]
        public System.Nullable<long> examSeqNbr
        {
            get
            {
                return this.examSeqNbrField;
            }
            set
            {
                this.examSeqNbrField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public bool existingACAUser
        {
            get
            {
                return this.existingACAUserField;
            }
            set
            {
                this.existingACAUserField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public bool fromACA
        {
            get
            {
                return this.fromACAField;
            }
            set
            {
                this.fromACAField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string inputEmail
        {
            get
            {
                return this.inputEmailField;
            }
            set
            {
                this.inputEmailField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public bool isBeyondAllowanceDate
        {
            get
            {
                return this.isBeyondAllowanceDateField;
            }
            set
            {
                this.isBeyondAllowanceDateField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public bool isFromReferenceSide
        {
            get
            {
                return this.isFromReferenceSideField;
            }
            set
            {
                this.isFromReferenceSideField = value;
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
        public System.Nullable<long> proctorID
        {
            get
            {
                return this.proctorIDField;
            }
            set
            {
                this.proctorIDField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public long providerNbr
        {
            get
            {
                return this.providerNbrField;
            }
            set
            {
                this.providerNbrField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = true)]
        public System.Nullable<long> refExamNbr
        {
            get
            {
                return this.refExamNbrField;
            }
            set
            {
                this.refExamNbrField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string reschedCancelReason
        {
            get
            {
                return this.reschedCancelReasonField;
            }
            set
            {
                this.reschedCancelReasonField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = true)]
        public System.Nullable<long> rosterID
        {
            get
            {
                return this.rosterIDField;
            }
            set
            {
                this.rosterIDField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public bool sameProviderForReschedule
        {
            get
            {
                return this.sameProviderForRescheduleField;
            }
            set
            {
                this.sameProviderForRescheduleField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = true)]
        public System.Nullable<System.DateTime> scheduleDate
        {
            get
            {
                return this.scheduleDateField;
            }
            set
            {
                this.scheduleDateField = value;
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
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string scheduleType
        {
            get
            {
                return this.scheduleTypeField;
            }
            set
            {
                this.scheduleTypeField = value;
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
        public StudentInfoVO studentInfo
        {
            get
            {
                return this.studentInfoField;
            }
            set
            {
                this.studentInfoField = value;
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
    }
}
