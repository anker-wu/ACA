/**
 * <pre>
 * 
 *  Accela Citizen Access
 *  File: ReportResultModel4WS.cs
 * 
 *  Accela, Inc.
 *  Copyright (C): 2007-2009
 * 
 *  Description:
 * 
 *  Notes:
 * $Id: ReportResultModel4WS.cs 130107 2009-05-11 12:23:56Z ACHIEVO\jackie.yu $.
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
    public partial class ReportResultModel4WS
    {

        private byte[] contentField;

        private string formatField;

        private string nameField;

        private bool printOnlyField;

        private ReportActionMessageModel4WS[] reportMessagesField;

        private string[] responseHeadersField;

        private string[] responseHeadersValuesField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, DataType = "base64Binary")]
        public byte[] content
        {
            get
            {
                return this.contentField;
            }
            set
            {
                this.contentField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string format
        {
            get
            {
                return this.formatField;
            }
            set
            {
                this.formatField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string name
        {
            get
            {
                return this.nameField;
            }
            set
            {
                this.nameField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public bool printOnly
        {
            get
            {
                return this.printOnlyField;
            }
            set
            {
                this.printOnlyField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("reportMessages", Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = true)]
        public ReportActionMessageModel4WS[] reportMessages
        {
            get
            {
                return this.reportMessagesField;
            }
            set
            {
                this.reportMessagesField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("responseHeaders", Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = true)]
        public string[] responseHeaders
        {
            get
            {
                return this.responseHeadersField;
            }
            set
            {
                this.responseHeadersField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("responseHeadersValues", Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = true)]
        public string[] responseHeadersValues
        {
            get
            {
                return this.responseHeadersValuesField;
            }
            set
            {
                this.responseHeadersValuesField = value;
            }
        }
    }
}
