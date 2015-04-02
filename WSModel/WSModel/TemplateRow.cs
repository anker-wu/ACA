#region Header
/**
 * <pre>
 * 
 *  Accela Citizen Access
 *  File: TemplateRow.cs
 * 
 *  Accela, Inc.
 *  Copyright (C): 2011
 * 
 *  Description:
 * 
 *  Notes:
 * $Id: TemplateRow.cs 181867 2010-10-06 08:06:18Z ACHIEVO\daly.zeng $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */
#endregion Header

namespace Accela.ACA.WSProxy
{
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.225")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://model.webservice.accela.com")]
    public partial class TemplateRow
    {

        private System.Nullable<long> rowIndexField;

        private GenericTemplateTableValue[] valuesField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = true)]
        public System.Nullable<long> rowIndex
        {
            get
            {
                return this.rowIndexField;
            }
            set
            {
                this.rowIndexField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("values", Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = true)]
        public GenericTemplateTableValue[] values
        {
            get
            {
                return this.valuesField;
            }
            set
            {
                this.valuesField = value;
            }
        }
    }
}
