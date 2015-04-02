#region Header
/**
 * <pre>
 * 
 *  Accela Citizen Access
 *  File: AssetContactModel.cs
 * 
 *  Accela, Inc.
 *  Copyright (C): 2010
 * 
 *  Description:
 * 
 *  Notes:
 * $Id: AssetContactModel.cs 181867 2010-10-06 08:06:18Z ACHIEVO\daly.zeng $.
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
    public partial class AssetContactModel : PeopleModel
    {
        private long assetSeqNbrField;

        private bool assetSeqNbrFieldSpecified;

        private string primaryFlagField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public long assetSeqNbr
        {
            get
            {
                return this.assetSeqNbrField;
            }
            set
            {
                this.assetSeqNbrField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool assetSeqNbrSpecified
        {
            get
            {
                return this.assetSeqNbrFieldSpecified;
            }
            set
            {
                this.assetSeqNbrFieldSpecified = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string primaryFlag
        {
            get
            {
                return this.primaryFlagField;
            }
            set
            {
                this.primaryFlagField = value;
            }
        }
    }
    
}
