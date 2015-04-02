#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: GlobalSearchParam4WS.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2009
 *
 *  Description:
 *  
 *
 *  Notes:
 *  $Id: GlobalSearchParam4WS.cs 130988 2009-8-21  10:22:01Z ACHIEVO\jackie.yu $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

namespace Accela.ACA.WSProxy
{
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.1433")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://model.webservice.accela.com")]
    public partial class GlobalSearchParam4WS
    {

        private string[] moduleArrayField;

        private string queryStringField;

        private int recordCountField;

        private int recordStartNumberField;

        private string searchTypeField;

        private string servProvCodeField;

        private string sortColumnField;

        private string sortDirectionField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("moduleArray", Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = true)]
        public string[] moduleArray
        {
            get
            {
                return this.moduleArrayField;
            }
            set
            {
                this.moduleArrayField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string queryString
        {
            get
            {
                return this.queryStringField;
            }
            set
            {
                this.queryStringField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public int recordCount
        {
            get
            {
                return this.recordCountField;
            }
            set
            {
                this.recordCountField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public int recordStartNumber
        {
            get
            {
                return this.recordStartNumberField;
            }
            set
            {
                this.recordStartNumberField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string searchType
        {
            get
            {
                return this.searchTypeField;
            }
            set
            {
                this.searchTypeField = value;
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
        public string sortColumn
        {
            get
            {
                return this.sortColumnField;
            }
            set
            {
                this.sortColumnField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string sortDirection
        {
            get
            {
                return this.sortDirectionField;
            }
            set
            {
                this.sortDirectionField = value;
            }
        }
    }
}
