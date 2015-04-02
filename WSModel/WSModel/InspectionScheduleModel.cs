/**
 * <pre>
 * 
 *  Accela Citizen Access
 *  File: InspectionScheduleModel.cs
 * 
 *  Accela, Inc.
 *  Copyright (C): 2012
 * 
 *  Description:
 * 
 *  Notes:
 * $Id: InspectionScheduleModel.cs 171698 2012-05-18 18:20:39Z ACHIEVO\jone.lu $.
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
    public partial class InspectionScheduleModel
    {

        private double actualUnitsField;

        private CalendarModel calendarField;

        private System.DateTime endTimeField;

        private InspectionModel inspectionField;

        private SysUserModel inspectorField;

        private int scheduleOrderField;

        private System.DateTime startTimeField;


        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public double actualUnits
        {
            get
            {
                return this.actualUnitsField;
            }
            set
            {
                this.actualUnitsField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public CalendarModel calendar
        {
            get
            {
                return this.calendarField;
            }
            set
            {
                this.calendarField = value;
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
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public InspectionModel inspection
        {
            get
            {
                return this.inspectionField;
            }
            set
            {
                this.inspectionField = value;
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
        public int scheduleOrder
        {
            get
            {
                return this.scheduleOrderField;
            }
            set
            {
                this.scheduleOrderField = value;
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
    }
}
