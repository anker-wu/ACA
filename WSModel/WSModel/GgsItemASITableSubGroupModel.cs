#region Header
/**
 * <pre>
 * 
 *  Accela Citizen Access
 *  File: GGSItemASITableSubGroupModel.cs
 * 
 *  Accela, Inc.
 *  Copyright (C): 2010
 * 
 *  Description:
 * 
 *  Notes:
 * $Id: GGSItemASITableSubGroupModel.cs 181867 2010-10-06 08:06:18Z ACHIEVO\daly.zeng $.
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
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://model.webservice.accela.com")]
    public partial class GGSItemASITableSubGroupModel
    {

        private int b1TableDspOrderField;

        private bool b1TableDspOrderFieldSpecified;

        private GGSItemASITableColumnModel[] columnListField;

        private string disableTableSortField;

        private string groupNameField;

        private long guideItemSeqNbrField;

        private bool guideItemSeqNbrFieldSpecified;

        private long guidesheetSeqNbrField;

        private bool guidesheetSeqNbrFieldSpecified;

        private string serviceProviderCodeField;

        private string tableNameField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public int b1TableDspOrder
        {
            get
            {
                return this.b1TableDspOrderField;
            }
            set
            {
                this.b1TableDspOrderField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool b1TableDspOrderSpecified
        {
            get
            {
                return this.b1TableDspOrderFieldSpecified;
            }
            set
            {
                this.b1TableDspOrderFieldSpecified = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("columnList", Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = true)]
        public GGSItemASITableColumnModel[] columnList
        {
            get
            {
                return this.columnListField;
            }
            set
            {
                this.columnListField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string disableTableSort
        {
            get
            {
                return this.disableTableSortField;
            }
            set
            {
                this.disableTableSortField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string groupName
        {
            get
            {
                return this.groupNameField;
            }
            set
            {
                this.groupNameField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public long guideItemSeqNbr
        {
            get
            {
                return this.guideItemSeqNbrField;
            }
            set
            {
                this.guideItemSeqNbrField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool guideItemSeqNbrSpecified
        {
            get
            {
                return this.guideItemSeqNbrFieldSpecified;
            }
            set
            {
                this.guideItemSeqNbrFieldSpecified = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public long guidesheetSeqNbr
        {
            get
            {
                return this.guidesheetSeqNbrField;
            }
            set
            {
                this.guidesheetSeqNbrField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool guidesheetSeqNbrSpecified
        {
            get
            {
                return this.guidesheetSeqNbrFieldSpecified;
            }
            set
            {
                this.guidesheetSeqNbrFieldSpecified = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string serviceProviderCode
        {
            get
            {
                return this.serviceProviderCodeField;
            }
            set
            {
                this.serviceProviderCodeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string tableName
        {
            get
            {
                return this.tableNameField;
            }
            set
            {
                this.tableNameField = value;
            }
        }
    }
}
