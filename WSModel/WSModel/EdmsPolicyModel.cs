/**
 * <pre>
 * 
 *  Accela Citizen Access
 *  File: EdmsPolicyModel.cs
 * 
 *  Accela, Inc.
 *  Copyright (C): 2007-2009
 * 
 *  Description:
 * 
 *  Notes:
 * $Id: EdmsPolicyModel.cs 130107 2009-05-11 12:23:56Z ACHIEVO\jackie.yu $.
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
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://service.webservice.accela.com/")]
    public partial class EdmsPolicyModel
    {

        private string configurationField;

        private string data2Field;

        private string data5Field;

        private bool defaultRightField;

        private bool defaultRightFieldSpecified;

        private bool deleteRightField;

        private bool deleteRightFieldSpecified;

        private bool downloadRightField;

        private bool downloadRightFieldSpecified;

        private long policySeqField;

        private string sourceNameField;

        private bool uploadRightField;

        private bool uploadRightFieldSpecified;

        private bool viewRightField;

        private bool viewRightFieldSpecified;

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
        public bool defaultRight
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
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool defaultRightSpecified
        {
            get
            {
                return this.defaultRightFieldSpecified;
            }
            set
            {
                this.defaultRightFieldSpecified = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public bool deleteRight
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
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool deleteRightSpecified
        {
            get
            {
                return this.deleteRightFieldSpecified;
            }
            set
            {
                this.deleteRightFieldSpecified = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public bool downloadRight
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
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool downloadRightSpecified
        {
            get
            {
                return this.downloadRightFieldSpecified;
            }
            set
            {
                this.downloadRightFieldSpecified = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
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
        public bool uploadRight
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
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool uploadRightSpecified
        {
            get
            {
                return this.uploadRightFieldSpecified;
            }
            set
            {
                this.uploadRightFieldSpecified = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public bool viewRight
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

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool viewRightSpecified
        {
            get
            {
                return this.viewRightFieldSpecified;
            }
            set
            {
                this.viewRightFieldSpecified = value;
            }
        }
    }
    
}
