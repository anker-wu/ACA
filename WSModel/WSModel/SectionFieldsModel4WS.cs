/**
 * <pre>
 * 
 *  Accela Citizen Access
 *  File: AcaAdminTreeNodeModel4WS.cs
 * 
 *  Accela, Inc.
 *  Copyright (C): 2007-2009
 * 
 *  Description:
 * 
 *  Notes:
 * $Id: SectionFieldsModel4WS.cs 130107 2009-05-11 12:23:56Z ACHIEVO\jackie.yu $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

namespace Accela.ACA.WSProxy
{
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.42")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://model.webservice.accela.com")]
    public partial class SectionFieldsModel4WS
    {

        private string sectionIDField;

        private SimpleViewElementModel4WS[] simpleViewElementModel4WSField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string sectionID
        {
            get
            {
                return this.sectionIDField;
            }
            set
            {
                this.sectionIDField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("simpleViewElementModel4WS", Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = true)]
        public SimpleViewElementModel4WS[] simpleViewElementModel4WS
        {
            get
            {
                return this.simpleViewElementModel4WSField;
            }
            set
            {
                this.simpleViewElementModel4WSField = value;
            }
        }
    }
}
