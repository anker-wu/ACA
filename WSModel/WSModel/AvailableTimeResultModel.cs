/**
 * <pre>
 * 
 *  Accela Citizen Access
 *  File: AvailableTimeResultModel.cs
 * 
 *  Accela, Inc.
 *  Copyright (C): 2012
 * 
 *  Description:
 * 
 *  Notes:
 * $Id: AvailableTimeResultModel.cs 171698 2012-05-18 18:20:39Z ACHIEVO\jone.lu $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

namespace Accela.ACA.WSProxy
{
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.1")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://model.webservice.accela.com")]
    public partial class AvailableTimeResultModel
    {

        private string actualMessageField;

        private double actualUnitField;

        private long calendarIDField;

        private string endAMPMField;

        private string endTimeField;

        private string flagField;

        private long idNumberField;

        private InspectionScheduleModel inspSchdlModelField;

        private string inspTypeField;

        private SysUserModel inspectorField;

        private string messageField;

        private object[] messageParamsField;

        private int resultTypeField;

        private System.Nullable<System.DateTime> scheduleDateField;

        private AvailableTimeResultModel secondValidationResultField;

        private string startAMPMField;

        private string startTimeField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string actualMessage
        {
            get
            {
                return this.actualMessageField;
            }
            set
            {
                this.actualMessageField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public double actualUnit
        {
            get
            {
                return this.actualUnitField;
            }
            set
            {
                this.actualUnitField = value;
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
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string endAMPM
        {
            get
            {
                return this.endAMPMField;
            }
            set
            {
                this.endAMPMField = value;
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
        public string flag
        {
            get
            {
                return this.flagField;
            }
            set
            {
                this.flagField = value;
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
        public InspectionScheduleModel inspSchdlModel
        {
            get
            {
                return this.inspSchdlModelField;
            }
            set
            {
                this.inspSchdlModelField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string inspType
        {
            get
            {
                return this.inspTypeField;
            }
            set
            {
                this.inspTypeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public SysUserModel inspector
        {
            get
            {
                return this.inspectorField;
            }
            set
            {
                this.inspectorField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string message
        {
            get
            {
                return this.messageField;
            }
            set
            {
                this.messageField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("messageParams", Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = true)]
        public object[] messageParams
        {
            get
            {
                return this.messageParamsField;
            }
            set
            {
                this.messageParamsField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public int resultType
        {
            get
            {
                return this.resultTypeField;
            }
            set
            {
                this.resultTypeField = value;
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
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public AvailableTimeResultModel secondValidationResult
        {
            get
            {
                return this.secondValidationResultField;
            }
            set
            {
                this.secondValidationResultField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string startAMPM
        {
            get
            {
                return this.startAMPMField;
            }
            set
            {
                this.startAMPMField = value;
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
    }
}
