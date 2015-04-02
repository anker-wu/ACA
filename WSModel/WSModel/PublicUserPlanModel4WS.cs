/**
 * <pre>
 * 
 *  Accela Citizen Access
 *  File: PublicUserPlanModel4WS.cs
 * 
 *  Accela, Inc.
 *  Copyright (C): 2007-2009
 * 
 *  Description:
 * 
 *  Notes:
 * $Id: PublicUserPlanModel4WS.cs 130107 2009-05-11 12:23:56Z ACHIEVO\jackie.yu $.
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
    public partial class PublicUserPlanModel4WS
    {

        private string checkDateField;

        private string fileDisplayNameField;

        private string filePathField;

        private string planNameField;

        private long planSeqNbrField;

        private string planStatusField;

        private string publicUserIDField;

        private string recDateField;

        private string recFulNameField;

        private string recStatusField;

        private byte[] reportDocumentField;

        private int retryTimesField;

        private string[] ruleSetField;

        private string servProvCodeField;

        private string submitDateField;

        private string transactionIDField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string checkDate
        {
            get
            {
                return this.checkDateField;
            }
            set
            {
                this.checkDateField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string fileDisplayName
        {
            get
            {
                return this.fileDisplayNameField;
            }
            set
            {
                this.fileDisplayNameField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string filePath
        {
            get
            {
                return this.filePathField;
            }
            set
            {
                this.filePathField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string planName
        {
            get
            {
                return this.planNameField;
            }
            set
            {
                this.planNameField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public long planSeqNbr
        {
            get
            {
                return this.planSeqNbrField;
            }
            set
            {
                this.planSeqNbrField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string planStatus
        {
            get
            {
                return this.planStatusField;
            }
            set
            {
                this.planStatusField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string publicUserID
        {
            get
            {
                return this.publicUserIDField;
            }
            set
            {
                this.publicUserIDField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string recDate
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
        public string recFulName
        {
            get
            {
                return this.recFulNameField;
            }
            set
            {
                this.recFulNameField = value;
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
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, DataType = "base64Binary")]
        public byte[] reportDocument
        {
            get
            {
                return this.reportDocumentField;
            }
            set
            {
                this.reportDocumentField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public int retryTimes
        {
            get
            {
                return this.retryTimesField;
            }
            set
            {
                this.retryTimesField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("ruleSet", Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = true)]
        public string[] ruleSet
        {
            get
            {
                return this.ruleSetField;
            }
            set
            {
                this.ruleSetField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string servProvCode
        {
            get
            {
                return this.servProvCodeField;
            }
            set
            {
                this.servProvCodeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string submitDate
        {
            get
            {
                return this.submitDateField;
            }
            set
            {
                this.submitDateField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string transactionID
        {
            get
            {
                return this.transactionIDField;
            }
            set
            {
                this.transactionIDField = value;
            }
        }
    }
}
