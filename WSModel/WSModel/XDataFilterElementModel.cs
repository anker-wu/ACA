#region Header
/**
 * <pre>
 * 
 *  Accela Citizen Access
 *  File: XDataFilterElementModel.cs
 * 
 *  Accela, Inc.
 *  Copyright (C): 2014
 * 
 *  Description:
 * 
 *  Notes:
 * $Id: XDataFilterElementModel.cs 181867 2014-02-17 08:06:18Z ACHIEVO\eric.he $.
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
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://service.webservice.accela.com/")]
    public partial class XDataFilterElementModel : AuditModel
    {

        private bool addUpperField;

        private string booleanOptrField;

        private bool caseSensitiveField;

        private long datafilterElementIdField;

        private bool datafilterElementIdFieldSpecified;

        private long datafilterIdField;

        private bool datafilterIdFieldSpecified;

        private string operatorField;

        private string prefixField;

        private string refColumnNameField;

        private string refColumnTypeField;

        private string searchByField;

        private string servProvCodeField;

        private string suffixField;

        private string tableAliasField;

        private string valueField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public bool addUpper
        {
            get
            {
                return this.addUpperField;
            }
            set
            {
                this.addUpperField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string booleanOptr
        {
            get
            {
                return this.booleanOptrField;
            }
            set
            {
                this.booleanOptrField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public bool caseSensitive
        {
            get
            {
                return this.caseSensitiveField;
            }
            set
            {
                this.caseSensitiveField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public long datafilterElementId
        {
            get
            {
                return this.datafilterElementIdField;
            }
            set
            {
                this.datafilterElementIdField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool datafilterElementIdSpecified
        {
            get
            {
                return this.datafilterElementIdFieldSpecified;
            }
            set
            {
                this.datafilterElementIdFieldSpecified = value;
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
        public string @operator
        {
            get
            {
                return this.operatorField;
            }
            set
            {
                this.operatorField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string prefix
        {
            get
            {
                return this.prefixField;
            }
            set
            {
                this.prefixField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string refColumnName
        {
            get
            {
                return this.refColumnNameField;
            }
            set
            {
                this.refColumnNameField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string refColumnType
        {
            get
            {
                return this.refColumnTypeField;
            }
            set
            {
                this.refColumnTypeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string searchBy
        {
            get
            {
                return this.searchByField;
            }
            set
            {
                this.searchByField = value;
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
        public string suffix
        {
            get
            {
                return this.suffixField;
            }
            set
            {
                this.suffixField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string tableAlias
        {
            get
            {
                return this.tableAliasField;
            }
            set
            {
                this.tableAliasField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string value
        {
            get
            {
                return this.valueField;
            }
            set
            {
                this.valueField = value;
            }
        }
    }
}
