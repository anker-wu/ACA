#region Header
/**
 * <pre>
 * 
 *  Accela Citizen Access
 *  File: TimeGroupModel.cs
 * 
 *  Accela, Inc.
 *  Copyright (C): 2010
 * 
 *  Description:
 * 
 *  Notes:
 * $Id: TimeGroupModel.cs 181867 2010-10-06 08:06:18Z ACHIEVO\daly.zeng $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */
#endregion Header

namespace Accela.ACA.WSProxy
{
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.3053")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://service.webservice.accela.com/")]
    public partial class TimeGroupModel : LanguageModel
    {
        private long curGroupIdField;

        private bool curGroupIdFieldSpecified;

        private string curUserIdField;

        private System.DateTime recDateField;

        private bool recDateFieldSpecified;

        private string recFulNamField;

        private string recStatusField;

        private string servProvCodeField;

        private string timeGroupDescField;

        private string timeGroupNameField;

        private long timeGroupSeqField;

        private bool timeGroupSeqFieldSpecified;

        private TimeTypeModel[] timeTypeListField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public long curGroupId
        {
            get
            {
                return this.curGroupIdField;
            }
            set
            {
                this.curGroupIdField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool curGroupIdSpecified
        {
            get
            {
                return this.curGroupIdFieldSpecified;
            }
            set
            {
                this.curGroupIdFieldSpecified = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string curUserId
        {
            get
            {
                return this.curUserIdField;
            }
            set
            {
                this.curUserIdField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public System.DateTime recDate
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
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool recDateSpecified
        {
            get
            {
                return this.recDateFieldSpecified;
            }
            set
            {
                this.recDateFieldSpecified = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string recFulNam
        {
            get
            {
                return this.recFulNamField;
            }
            set
            {
                this.recFulNamField = value;
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
        public string timeGroupDesc
        {
            get
            {
                return this.timeGroupDescField;
            }
            set
            {
                this.timeGroupDescField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string timeGroupName
        {
            get
            {
                return this.timeGroupNameField;
            }
            set
            {
                this.timeGroupNameField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public long timeGroupSeq
        {
            get
            {
                return this.timeGroupSeqField;
            }
            set
            {
                this.timeGroupSeqField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool timeGroupSeqSpecified
        {
            get
            {
                return this.timeGroupSeqFieldSpecified;
            }
            set
            {
                this.timeGroupSeqFieldSpecified = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("timeTypeList", Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = true)]
        public TimeTypeModel[] timeTypeList
        {
            get
            {
                return this.timeTypeListField;
            }
            set
            {
                this.timeTypeListField = value;
            }
        }
    }
}
