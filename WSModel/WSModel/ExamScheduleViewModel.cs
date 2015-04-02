#region Header
/**
 * <pre>
 * 
 *  Accela Citizen Access
 *  File: ExamScheduleViewModel.cs
 * 
 *  Accela, Inc.
 *  Copyright (C): 2011-2012
 * 
 *  Description:
 * 
 *  Notes:
 * $Id: ExamScheduleViewModel.cs 181867 2011-09-27 08:06:18Z ACHIEVO\daly.zeng $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */
#endregion Header

namespace Accela.ACA.WSProxy
{
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.1")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://model.webservice.accela.com")]
    public partial class ExamScheduleViewModel
    {

        private int availableSeatsField;

        private string batchJobNameField;

        private System.Nullable<long> calendarIDField;

        private System.Nullable<System.DateTime> dateField;

        private string dispTimeField;

        private System.Nullable<System.DateTime> endTimeField;

        private RefFeeDsecVO[] refFeeDsecVOsField;

        private string examNameField;

        private bool externalField;

        private string instructionsField;

        private System.Nullable<long> locationIDField;

        private RProviderLocationModel locationModelField;

        private string providerNameField;

        private long providerNbrField;

        private System.Nullable<long> refExamNbrField;

        private System.Nullable<long> scheduleIDField;

        private string serviceProviderCodeField;

        private System.Nullable<System.DateTime> startTimeField;

        private string supportLangField;

        private string totalFeeAmountField;

        private string weekdayField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public int availableSeats
        {
            get
            {
                return this.availableSeatsField;
            }
            set
            {
                this.availableSeatsField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string batchJobName
        {
            get
            {
                return this.batchJobNameField;
            }
            set
            {
                this.batchJobNameField = value;
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
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = true)]
        public System.Nullable<System.DateTime> date
        {
            get
            {
                return this.dateField;
            }
            set
            {
                this.dateField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string dispTime
        {
            get
            {
                return this.dispTimeField;
            }
            set
            {
                this.dispTimeField = value;
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
        [System.Xml.Serialization.XmlArrayAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        [System.Xml.Serialization.XmlArrayItemAttribute("refFeeDsecVO", Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = false)]
        public RefFeeDsecVO[] refFeeDsecVOs
        {
            get
            {
                return this.refFeeDsecVOsField;
            }
            set
            {
                this.refFeeDsecVOsField = value;
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
        public bool external
        {
            get
            {
                return this.externalField;
            }
            set
            {
                this.externalField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string instructions
        {
            get
            {
                return this.instructionsField;
            }
            set
            {
                this.instructionsField = value;
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
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public RProviderLocationModel locationModel
        {
            get
            {
                return this.locationModelField;
            }
            set
            {
                this.locationModelField = value;
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
        public string supportLang
        {
            get
            {
                return this.supportLangField;
            }
            set
            {
                this.supportLangField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string totalFeeAmount
        {
            get
            {
                return this.totalFeeAmountField;
            }
            set
            {
                this.totalFeeAmountField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string weekday
        {
            get
            {
                return this.weekdayField;
            }
            set
            {
                this.weekdayField = value;
            }
        }
    }
}