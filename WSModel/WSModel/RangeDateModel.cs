#region Header
/**
 * <pre>
 * 
 *  Accela Citizen Access
 *  File: RangeDateModel.cs
 * 
 *  Accela, Inc.
 *  Copyright (C): 2010
 * 
 *  Description:
 * 
 *  Notes:
 * $Id: RangeDateModel.cs 187076 2010-12-21 08:20:14Z ACHIEVO\xinter.peng $.
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
    public partial class RangeDateModel
    {

        private System.DateTime fromDateField;

        private bool fromDateFieldSpecified;

        private System.DateTime toDateField;

        private bool toDateFieldSpecified;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public System.DateTime fromDate
        {
            get
            {
                return this.fromDateField;
            }
            set
            {
                this.fromDateField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool fromDateSpecified
        {
            get
            {
                return this.fromDateFieldSpecified;
            }
            set
            {
                this.fromDateFieldSpecified = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public System.DateTime toDate
        {
            get
            {
                return this.toDateField;
            }
            set
            {
                this.toDateField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool toDateSpecified
        {
            get
            {
                return this.toDateFieldSpecified;
            }
            set
            {
                this.toDateFieldSpecified = value;
            }
        }
    }
}
