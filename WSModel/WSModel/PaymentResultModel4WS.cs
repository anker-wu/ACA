/**
 * <pre>
 * 
 *  Accela Citizen Access
 *  File: PaymentResultModel4WS.cs
 * 
 *  Accela, Inc.
 *  Copyright (C): 2007-2010
 * 
 *  Description:
 * 
 *  Notes:
 * $Id: PaymentResultModel4WS.cs 185374 2010-11-26 09:47:10Z ACHIEVO\xinter.peng $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

namespace Accela.ACA.WSProxy
{
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.832")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://model.webservice.accela.com")]
    public partial class PaymentResultModel4WS
    {

        private CapIDModel4WS capIDField;

        private bool parentCapField;

        private string[] serviceNamesField;

        private string moduleNameField;

        private bool succeedField;

        private bool hasFeeField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public CapIDModel4WS capID
        {
            get
            {
                return this.capIDField;
            }
            set
            {
                this.capIDField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public bool parentCap
        {
            get
            {
                return this.parentCapField;
            }
            set
            {
                this.parentCapField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("serviceNames", Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = true)]
        public string[] serviceNames
        {
            get
            {
                return this.serviceNamesField;
            }
            set
            {
                this.serviceNamesField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public bool succeed
        {
            get
            {
                return this.succeedField;
            }
            set
            {
                this.succeedField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public bool hasFee
        {
            get
            {
                return this.hasFeeField;
            }
            set
            {
                this.hasFeeField = value;
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
    }
}
