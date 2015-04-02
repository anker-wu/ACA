#region Header
/**
 * <pre>
 * 
 *  Accela Citizen Access
 *  File: StructureEstablishmentModel.cs
 * 
 *  Accela, Inc.
 *  Copyright (C): 2010
 * 
 *  Description:
 * 
 *  Notes:
 * $Id: StructureEstablishmentModel.cs 181867 2010-10-06 08:06:18Z ACHIEVO\daly.zeng $.
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
    public partial class StructureEstablishmentModel : StrucEstaMasterModel
    {
        private object[] dataAttributesField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("dataAttributes", Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = true)]
        public object[] dataAttributes
        {
            get
            {
                return this.dataAttributesField;
            }
            set
            {
                this.dataAttributesField = value;
            }
        }
    }
}
