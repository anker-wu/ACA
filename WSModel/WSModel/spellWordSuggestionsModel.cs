/**
 * <pre>
 * 
 *  Accela Citizen Access
 *  File: SpellCheckerWordSuggestionsModel.cs
 * 
 *  Accela, Inc.
 *  Copyright (C): 2009
 * 
 *  Description:
 * 
 *  Notes:
 * $Id: SpellCheckerWordSuggestionsModel.cs 130107 2009-05-11 12:23:56Z ACHIEVO\weiky chen $.
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
    public partial class SpellCheckerWordSuggestionsModel
    {

        private string[] suggestionsField;

        private string wordField;

        private int wordContextPositionField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("suggestions", Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = true)]
        public string[] suggestions
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

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string word
        {
            get
            {
                return this.wordField;
            }
            set
            {
                this.wordField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public int wordContextPosition
        {
            get
            {
                return this.wordContextPositionField;
            }
            set
            {
                this.wordContextPositionField = value;
            }
        }
    }
}
