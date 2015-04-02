/**
 * <pre>
 * 
 *  Accela Citizen Access
 *  File: SpellCheckerResultModel.cs
 * 
 *  Accela, Inc.
 *  Copyright (C): 2009
 * 
 *  Description:
 * 
 *  Notes:
 * $Id: SpellCheckerResultModel.cs 130107 2009-05-11 12:23:56Z ACHIEVO\weiky chen $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

namespace Accela.ACA.WSProxy
{
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.3053")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://spellchecker.aa.accela.com/")]
    public partial class SpellCheckerResultModel
    {

        private string returnCodeField;

        private string returnMessageField;

        private SpellCheckerWordSuggestionsModel[] suggestionsField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string returnCode
        {
            get
            {
                return this.returnCodeField;
            }
            set
            {
                this.returnCodeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string returnMessage
        {
            get
            {
                return this.returnMessageField;
            }
            set
            {
                this.returnMessageField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("suggestions", Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = true)]
        public SpellCheckerWordSuggestionsModel[] suggestions
        {
            get
            {
                return this.suggestionsField;
            }
            set
            {
                this.suggestionsField = value;
            }
        }
    }
}
