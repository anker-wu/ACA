/**
 * <pre>
 * 
 *  Accela Citizen Access
 *  File: F4FeeModel4WS.cs
 * 
 *  Accela, Inc.
 *  Copyright (C): 2007-2009
 * 
 *  Description:
 * 
 *  Notes:
 * $Id: F4FeeModel4WS.cs 130107 2009-05-11 12:23:56Z ACHIEVO\jackie.yu $.
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
    public partial class F4FeeModel4WS
    {

        private F4FeeItemModel4WS f4FeeItemModelField;

        private long receiptNbrField;

        private X4FeeItemInvoiceModel4WS x4FeeItemInvoiceModelField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public F4FeeItemModel4WS f4FeeItemModel
        {
            get
            {
                return this.f4FeeItemModelField;
            }
            set
            {
                this.f4FeeItemModelField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public long receiptNbr
        {
            get
            {
                return this.receiptNbrField;
            }
            set
            {
                this.receiptNbrField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public X4FeeItemInvoiceModel4WS x4FeeItemInvoiceModel
        {
            get
            {
                return this.x4FeeItemInvoiceModelField;
            }
            set
            {
                this.x4FeeItemInvoiceModelField = value;
            }
        }
    }
}
