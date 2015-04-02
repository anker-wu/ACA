/**
 * <pre>
 * 
 *  Accela Citizen Access
 *  File: EdmsPolicyModel4WS.cs
 * 
 *  Accela, Inc.
 *  Copyright (C): 2007-2009
 * 
 *  Description:
 * 
 *  Notes:
 * $Id: EdmsPolicyModel4WS.cs 130107 2009-05-11 12:23:56Z ACHIEVO\jackie.yu $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

namespace Accela.ACA.WSProxy
{
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.42")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://model.webservice.accela.com")]
    public partial class EdmsPolicyModel4WS
    {

        private string agenctNameField;

        private string agencyNameField;

        private string configurationField;

        private string data2Field;

        private string data5Field;

        private string defaultRightField;

        private string deleteRightField;

        private string downloadRightField;

        private long policySeqField;

        private string sourceNameField;

        private string uploadRightField;

        private string viewRightField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string agenctName
        {
            get
            {
                return this.agenctNameField;
            }
            set
            {
                this.agenctNameField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string agencyName
        {
            get
            {
                return this.agencyNameField;
            }
            set
            {
                this.agencyNameField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string configuration
        {
            get
            {
                return this.configurationField;
            }
            set
            {
                this.configurationField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string data2
        {
            get
            {
                return this.data2Field;
            }
            set
            {
                this.data2Field = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string data5
        {
            get
            {
                return this.data5Field;
            }
            set
            {
                this.data5Field = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string defaultRight
        {
            get
            {
                return this.defaultRightField;
            }
            set
            {
                this.defaultRightField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string deleteRight
        {
            get
            {
                return this.deleteRightField;
            }
            set
            {
                this.deleteRightField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string downloadRight
        {
            get
            {
                return this.downloadRightField;
            }
            set
            {
                this.downloadRightField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public long policySeq
        {
            get
            {
                return this.policySeqField;
            }
            set
            {
                this.policySeqField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string sourceName
        {
            get
            {
                return this.sourceNameField;
            }
            set
            {
                this.sourceNameField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string uploadRight
        {
            get
            {
                return this.uploadRightField;
            }
            set
            {
                this.uploadRightField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string viewRight
        {
            get
            {
                return this.viewRightField;
            }
            set
            {
                this.viewRightField = value;
            }
        }
    }
}
