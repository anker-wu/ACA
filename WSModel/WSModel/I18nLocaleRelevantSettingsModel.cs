/**
 * <pre>
 * 
 *  Accela Citizen Access
 *  File: I18nLocaleRelevantSettingsModel.cs
 * 
 *  Accela, Inc.
 *  Copyright (C): 2010-2011
 * 
 *  Description:
 * 
 *  Notes:
 * $Id: I18nLocaleRelevantSettingsModel.cs 209168 2011-12-07 06:31:49Z ACHIEVO\alan.hu $.
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
    public partial class I18nLocaleRelevantSettingsModel
    {

        private string currencySymbolField;

        private string dateFormatField;

        private string dateTimeFormatField;

        private string inspectionDateTimeFormatField;

        private string inspectionTimeFormatField;

        private string localeField;

        private string longDateFormatField;

        private string timeFormatField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string currencySymbol
        {
            get
            {
                return this.currencySymbolField;
            }
            set
            {
                this.currencySymbolField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string dateFormat
        {
            get
            {
                return this.dateFormatField;
            }
            set
            {
                this.dateFormatField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string dateTimeFormat
        {
            get
            {
                return this.dateTimeFormatField;
            }
            set
            {
                this.dateTimeFormatField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string inspectionDateTimeFormat
        {
            get
            {
                return this.inspectionDateTimeFormatField;
            }
            set
            {
                this.inspectionDateTimeFormatField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string inspectionTimeFormat
        {
            get
            {
                return this.inspectionTimeFormatField;
            }
            set
            {
                this.inspectionTimeFormatField = value;
            }
        }

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
        public string longDateFormat
        {
            get
            {
                return this.longDateFormatField;
            }
            set
            {
                this.longDateFormatField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string timeFormat
        {
            get
            {
                return this.timeFormatField;
            }
            set
            {
                this.timeFormatField = value;
            }
        }
    }
}
