/**
 * <pre>
 * 
 *  Accela Citizen Access
 *  File: ButtonSettingModel4WS.cs
 * 
 *  Accela, Inc.
 *  Copyright (C): 2007-2010
 * 
 *  Description:
 * 
 *  Notes:
 * $Id: ButtonSettingModel4WS.cs 178037 2010-07-30 06:25:12Z ACHIEVO\daly.zeng $.
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
    public partial class ButtonSettingModel4WS
    {

        private CapTypeModel[] availableCapTypeListField;

        private string buttonNameField;

        private string moduleNameField;

        private CapTypeModel[] selectedCapTypeListField;

        private string serviceProviderCodeField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("availableCapTypeList", Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = true)]
        public CapTypeModel[] availableCapTypeList
        {
            get
            {
                return this.availableCapTypeListField;
            }
            set
            {
                this.availableCapTypeListField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string buttonName
        {
            get
            {
                return this.buttonNameField;
            }
            set
            {
                this.buttonNameField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string moduleName
        {
            get
            {
                return this.moduleNameField;
            }
            set
            {
                this.moduleNameField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("selectedCapTypeList", Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = true)]
        public CapTypeModel[] selectedCapTypeList
        {
            get
            {
                return this.selectedCapTypeListField;
            }
            set
            {
                this.selectedCapTypeListField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string serviceProviderCode
        {
            get
            {
                return this.serviceProviderCodeField;
            }
            set
            {
                this.serviceProviderCodeField = value;
            }
        }
    }
}
