/**
 * <pre>
 * 
 *  Accela Citizen Access
 *  File: SupportedLanguageModel.cs
 * 
 *  Accela, Inc.
 *  Copyright (C): 2010
 * 
 *  Description:
 * 
 *  Notes:
 * $Id: SupportedLanguageModel.cs 185374 2010-11-26 09:47:10Z ACHIEVO\xinter.peng $.
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
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://model.webservice.accela.com")]
    public partial class SupportedLanguageModel
    {

        private string localeField;

        private string localeLabelField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string locale
        {
            get
            {
                return this.localeField;
            }
            set
            {
                this.localeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string localeLabel
        {
            get
            {
                return this.localeLabelField;
            }
            set
            {
                this.localeLabelField = value;
            }
        }
    }
}
