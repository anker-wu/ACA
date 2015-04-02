/**
 * <pre>
 * 
 *  Accela Citizen Access
 *  File: ReportButtonPropertyModel4WS.cs
 * 
 *  Accela, Inc.
 *  Copyright (C): 2007-2009
 * 
 *  Description:
 * 
 *  Notes:
 * $Id: ReportButtonPropertyModel4WS.cs 130439 2009-05-13 10:02:20Z ACHIEVO\jackie.yu $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

namespace Accela.ACA.WSProxy
{
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.1433")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://model.webservice.accela.com")]
    public partial class ReportButtonPropertyModel4WS
    {

        private string buttonNameField;

        private string errorInfoField;

        private bool isDisplayedField;

        private string reportIdField;

        private string reportNameField;

        private string resReportNameField;
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string buttonName
        {
            get
            {
                return this.buttonNameField;
            }
            set
            {
                this.buttonNameField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string errorInfo
        {
            get
            {
                return this.errorInfoField;
            }
            set
            {
                this.errorInfoField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public bool isDisplayed
        {
            get
            {
                return this.isDisplayedField;
            }
            set
            {
                this.isDisplayedField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string reportId
        {
            get
            {
                return this.reportIdField;
            }
            set
            {
                this.reportIdField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string reportName
        {
            get
            {
                return this.reportNameField;
            }
            set
            {
                this.reportNameField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string resReportName
        {
            get
            {
                return this.resReportNameField;
            }
            set
            {
                this.resReportNameField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string description
        {
            get;
            set;
        }


        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public bool isMultipleWindow
        {
            get;
            set;
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public bool isSave2EDMS
        {
            get;
            set;
        }
    }
}
