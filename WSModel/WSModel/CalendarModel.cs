#region Header
/**
 * <pre>
 * 
 *  Accela Citizen Access
 *  File: CalendarModel.cs
 * 
 *  Accela, Inc.
 *  Copyright (C): 2010-2014
 * 
 *  Description:
 * 
 *  Notes:
 *
 * </pre>
 */
#endregion Header

namespace Accela.ACA.WSProxy
{
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.18408")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://model.webservice.accela.com")]
    public partial class CalendarModel : LanguageModel
    {

        private System.Nullable<int> acaScheduleBlockField;

        private string acaScheduleBlockUnitField;

        private System.Nullable<long> calendarAttemptsField;

        private System.Nullable<int> calendarBlockSizeField;

        private string calendarBlockUnitField;

        private string calendarCommentField;

        private string calendarCutOffTimeField;

        private string calendarEnableForACAField;

        private System.Nullable<long> calendarIDField;

        private string calendarNameField;

        private System.Nullable<int> calendarPriorityField;

        private string calendarReasonField;

        private string calendarTypeField;

        private System.Nullable<double> calendarUnitField;

        private string calendarUserField;

        private string daysInAdvanceField;

        private string defaultLocationField;

        private string hideInspectionTimesInACAField;

        private System.Nullable<System.DateTime> recDateField;

        private string recFullNameField;

        private string recStatusField;

        private string resCalendarCommentField;

        private string resCalendarNameField;

        private string resCalendarReasonField;

        private System.Nullable<long> scheduleNumberOfDaysField;

        private string serviceProviderCodeField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = true)]
        public System.Nullable<int> acaScheduleBlock
        {
            get
            {
                return this.acaScheduleBlockField;
            }
            set
            {
                this.acaScheduleBlockField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string acaScheduleBlockUnit
        {
            get
            {
                return this.acaScheduleBlockUnitField;
            }
            set
            {
                this.acaScheduleBlockUnitField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = true)]
        public System.Nullable<long> calendarAttempts
        {
            get
            {
                return this.calendarAttemptsField;
            }
            set
            {
                this.calendarAttemptsField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = true)]
        public System.Nullable<int> calendarBlockSize
        {
            get
            {
                return this.calendarBlockSizeField;
            }
            set
            {
                this.calendarBlockSizeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string calendarBlockUnit
        {
            get
            {
                return this.calendarBlockUnitField;
            }
            set
            {
                this.calendarBlockUnitField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string calendarComment
        {
            get
            {
                return this.calendarCommentField;
            }
            set
            {
                this.calendarCommentField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string calendarCutOffTime
        {
            get
            {
                return this.calendarCutOffTimeField;
            }
            set
            {
                this.calendarCutOffTimeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string calendarEnableForACA
        {
            get
            {
                return this.calendarEnableForACAField;
            }
            set
            {
                this.calendarEnableForACAField = value;
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
        public string calendarName
        {
            get
            {
                return this.calendarNameField;
            }
            set
            {
                this.calendarNameField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = true)]
        public System.Nullable<int> calendarPriority
        {
            get
            {
                return this.calendarPriorityField;
            }
            set
            {
                this.calendarPriorityField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string calendarReason
        {
            get
            {
                return this.calendarReasonField;
            }
            set
            {
                this.calendarReasonField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string calendarType
        {
            get
            {
                return this.calendarTypeField;
            }
            set
            {
                this.calendarTypeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = true)]
        public System.Nullable<double> calendarUnit
        {
            get
            {
                return this.calendarUnitField;
            }
            set
            {
                this.calendarUnitField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string calendarUser
        {
            get
            {
                return this.calendarUserField;
            }
            set
            {
                this.calendarUserField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string daysInAdvance
        {
            get
            {
                return this.daysInAdvanceField;
            }
            set
            {
                this.daysInAdvanceField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string defaultLocation
        {
            get
            {
                return this.defaultLocationField;
            }
            set
            {
                this.defaultLocationField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string hideInspectionTimesInACA
        {
            get
            {
                return this.hideInspectionTimesInACAField;
            }
            set
            {
                this.hideInspectionTimesInACAField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = true)]
        public System.Nullable<System.DateTime> recDate
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
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string recFullName
        {
            get
            {
                return this.recFullNameField;
            }
            set
            {
                this.recFullNameField = value;
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
        public string resCalendarComment
        {
            get
            {
                return this.resCalendarCommentField;
            }
            set
            {
                this.resCalendarCommentField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string resCalendarName
        {
            get
            {
                return this.resCalendarNameField;
            }
            set
            {
                this.resCalendarNameField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string resCalendarReason
        {
            get
            {
                return this.resCalendarReasonField;
            }
            set
            {
                this.resCalendarReasonField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = true)]
        public System.Nullable<long> scheduleNumberOfDays
        {
            get
            {
                return this.scheduleNumberOfDaysField;
            }
            set
            {
                this.scheduleNumberOfDaysField = value;
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
    }
}
