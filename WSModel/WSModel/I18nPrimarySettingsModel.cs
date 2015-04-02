/**
 * <pre>
 * 
 *  Accela Citizen Access
 *  File: I18nPrimarySettingsModel.cs
 * 
 *  Accela, Inc.
 *  Copyright (C): 2010
 * 
 *  Description:
 * 
 *  Notes:
 * $Id: I18nPrimarySettingsModel.cs 185374 2010-11-26 09:47:10Z ACHIEVO\xinter.peng $.
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
    public partial class I18nPrimarySettingsModel
    {

        private string addressLocaleField;

        private string currencyLocaleField;

        private bool hideLanguageOptionsField;

        private bool multipleLanguageSupportField;

        private string primaryLanguageField;

        private SupportedLanguageModel[] supportLanguagesField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string addressLocale
        {
            get
            {
                return this.addressLocaleField;
            }
            set
            {
                this.addressLocaleField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string currencyLocale
        {
            get
            {
                return this.currencyLocaleField;
            }
            set
            {
                this.currencyLocaleField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public bool hideLanguageOptions
        {
            get
            {
                return this.hideLanguageOptionsField;
            }
            set
            {
                this.hideLanguageOptionsField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public bool multipleLanguageSupport
        {
            get
            {
                return this.multipleLanguageSupportField;
            }
            set
            {
                this.multipleLanguageSupportField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string primaryLanguage
        {
            get
            {
                return this.primaryLanguageField;
            }
            set
            {
                this.primaryLanguageField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("supportLanguages", Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = true)]
        public SupportedLanguageModel[] supportLanguages
        {
            get
            {
                return this.supportLanguagesField;
            }
            set
            {
                this.supportLanguagesField = value;
            }
        }
    }
}
