#region Header
/**
 * <pre>
 * 
 *  Accela Citizen Access
 *  File: AssetOrderModel.cs
 * 
 *  Accela, Inc.
 *  Copyright (C): 2010
 * 
 *  Description:
 * 
 *  Notes:
 * $Id: AssetOrderModel.cs 181867 2010-10-06 08:06:18Z ACHIEVO\daly.zeng $.
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
    public partial class AssetOrderModel : LanguageModel
    {
        private string assetCompleteField;

        private System.DateTime assetCompletionDateField;

        private bool assetCompletionDateFieldSpecified;

        private int assetOrderField;

        private bool assetOrderFieldSpecified;

        private string assetShortNotesField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string assetComplete
        {
            get
            {
                return this.assetCompleteField;
            }
            set
            {
                this.assetCompleteField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public System.DateTime assetCompletionDate
        {
            get
            {
                return this.assetCompletionDateField;
            }
            set
            {
                this.assetCompletionDateField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool assetCompletionDateSpecified
        {
            get
            {
                return this.assetCompletionDateFieldSpecified;
            }
            set
            {
                this.assetCompletionDateFieldSpecified = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public int assetOrder
        {
            get
            {
                return this.assetOrderField;
            }
            set
            {
                this.assetOrderField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool assetOrderSpecified
        {
            get
            {
                return this.assetOrderFieldSpecified;
            }
            set
            {
                this.assetOrderFieldSpecified = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string assetShortNotes
        {
            get
            {
                return this.assetShortNotesField;
            }
            set
            {
                this.assetShortNotesField = value;
            }
        }
    }    
}
