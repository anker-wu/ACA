/**
 * <pre>
 * 
 *  Accela Citizen Access
 *  File: QueryFormat.cs
 * 
 *  Accela, Inc.
 *  Copyright (C): 2010-2014
 * 
 *  Description:
 * 
 *  Notes:
 * $Id: QueryFormat.cs 130107 2009-05-11 12:23:56Z ACHIEVO\jackie.yu $.
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
    public partial class QueryFormat
    {

        private int endRowField;

        private string groupingField;

        private int maxRowsField;

        private string orderField;

        private bool publicUserFlagField;

        private int startRowField;

        private XDataFilterElementModel[] dataFilterField;

        private XDataFilterElementModel[] quickQueryField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public int endRow
        {
            get
            {
                return this.endRowField;
            }
            set
            {
                this.endRowField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string grouping
        {
            get
            {
                return this.groupingField;
            }
            set
            {
                this.groupingField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public int maxRows
        {
            get
            {
                return this.maxRowsField;
            }
            set
            {
                this.maxRowsField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string order
        {
            get
            {
                return this.orderField;
            }
            set
            {
                this.orderField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public bool publicUserFlag
        {
            get
            {
                return this.publicUserFlagField;
            }
            set
            {
                this.publicUserFlagField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public int startRow
        {
            get
            {
                return this.startRowField;
            }
            set
            {
                this.startRowField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("dataFilter", Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = true)]
        public XDataFilterElementModel[] dataFilter
        {
            get
            {
                return this.dataFilterField;
            }
            set
            {
                this.dataFilterField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("quickQuery", Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = true)]
        public XDataFilterElementModel[] quickQuery
        {
            get
            {
                return this.quickQueryField;
            }
            set
            {
                this.quickQueryField = value;
            }
        }

    }
}