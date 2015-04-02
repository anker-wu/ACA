#region Header
/**
 * <pre>
 * 
 *  Accela Citizen Access
 *  File: XDataFilterModel.cs
 * 
 *  Accela, Inc.
 *  Copyright (C): 2014
 * 
 *  Description:
 * 
 *  Notes:
 * $Id: XDataFilterModel.cs 181867 2014-02-17 08:06:18Z ACHIEVO\eric.he $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
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
    public partial class XDataFilterModel
    {

        private string datafilterDescField;

        private long datafilterIdField;

        private bool datafilterIdFieldSpecified;

        private string datafilterNameField;

        private string datafilterTypeField;

        private string levelIdField;

        private string levelTypeField;

        private string primaryField;

        private object[] resDataFilterField;

        private string servProvCodeField;

        private string userIdField;

        private long viewIdField;

        private bool viewIdFieldSpecified;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string datafilterDesc
        {
            get
            {
                return this.datafilterDescField;
            }
            set
            {
                this.datafilterDescField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public long datafilterId
        {
            get
            {
                return this.datafilterIdField;
            }
            set
            {
                this.datafilterIdField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool datafilterIdSpecified
        {
            get
            {
                return this.datafilterIdFieldSpecified;
            }
            set
            {
                this.datafilterIdFieldSpecified = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string datafilterName
        {
            get
            {
                return this.datafilterNameField;
            }
            set
            {
                this.datafilterNameField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string datafilterType
        {
            get
            {
                return this.datafilterTypeField;
            }
            set
            {
                this.datafilterTypeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string levelId
        {
            get
            {
                return this.levelIdField;
            }
            set
            {
                this.levelIdField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string levelType
        {
            get
            {
                return this.levelTypeField;
            }
            set
            {
                this.levelTypeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string primary
        {
            get
            {
                return this.primaryField;
            }
            set
            {
                this.primaryField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("resDataFilter", Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = true)]
        public object[] resDataFilter
        {
            get
            {
                return this.resDataFilterField;
            }
            set
            {
                this.resDataFilterField = value;
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
        public string userId
        {
            get
            {
                return this.userIdField;
            }
            set
            {
                this.userIdField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public long viewId
        {
            get
            {
                return this.viewIdField;
            }
            set
            {
                this.viewIdField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool viewIdSpecified
        {
            get
            {
                return this.viewIdFieldSpecified;
            }
            set
            {
                this.viewIdFieldSpecified = value;
            }
        }
    }
}
