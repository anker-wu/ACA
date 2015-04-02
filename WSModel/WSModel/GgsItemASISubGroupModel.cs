#region Header
/**
 * <pre>
 * 
 *  Accela Citizen Access
 *  File: GGSItemASISubGroupModel.cs
 * 
 *  Accela, Inc.
 *  Copyright (C): 2010
 * 
 *  Description:
 * 
 *  Notes:
 * $Id: GGSItemASISubGroupModel.cs 181867 2010-10-06 08:06:18Z ACHIEVO\daly.zeng $.
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
    public partial class GGSItemASISubGroupModel
    {

        private GGSItemASIModel[] asiListField;

        private int b1GroupDspOrderField;

        private bool b1GroupDspOrderFieldSpecified;

        private string groupCodeField;

        private long guideItemSeqNbrField;

        private bool guideItemSeqNbrFieldSpecified;

        private long guidesheetSeqNbrField;

        private bool guidesheetSeqNbrFieldSpecified;

        private string serviceProviderCodeField;

        private string subgroupCodeField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("asiList", Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = true)]
        public GGSItemASIModel[] asiList
        {
            get
            {
                return this.asiListField;
            }
            set
            {
                this.asiListField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public int b1GroupDspOrder
        {
            get
            {
                return this.b1GroupDspOrderField;
            }
            set
            {
                this.b1GroupDspOrderField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool b1GroupDspOrderSpecified
        {
            get
            {
                return this.b1GroupDspOrderFieldSpecified;
            }
            set
            {
                this.b1GroupDspOrderFieldSpecified = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string groupCode
        {
            get
            {
                return this.groupCodeField;
            }
            set
            {
                this.groupCodeField = value;
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
        public string subgroupCode
        {
            get
            {
                return this.subgroupCodeField;
            }
            set
            {
                this.subgroupCodeField = value;
            }
        }
    }
}
