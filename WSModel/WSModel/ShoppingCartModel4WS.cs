/**
 * <pre>
 * 
 *  Accela Citizen Access
 *  File: ShoppingCartModel4WS.cs
 * 
 *  Accela, Inc.
 *  Copyright (C): 2009
 * 
 *  Description:
 * 
 *  Notes:
 * $Id: ShoppingCartModel4WS.cs 130107 2009-05-11 12:23:56Z ACHIEVO\jackie.yu $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

namespace Accela.ACA.WSProxy
{



    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.1433")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://model.webservice.accela.com")]
    public partial class ShoppingCartModel4WS
    {

        private long cartSeqNumberField;

        private string recDateField;

        private string recFullNameField;

        private string recStatusField;

        private ShoppingCartItemModel4WS[] shoppingCartItemsField;

        private long userSeqNumberField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public long cartSeqNumber
        {
            get
            {
                return this.cartSeqNumberField;
            }
            set
            {
                this.cartSeqNumberField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string recDate
        {
            get
            {
                return this.recDateField;
            }
            set
            {
                this.recDateField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string recFullName
        {
            get
            {
                return this.recFullNameField;
            }
            set
            {
                this.recFullNameField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string recStatus
        {
            get
            {
                return this.recStatusField;
            }
            set
            {
                this.recStatusField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("shoppingCartItems", Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = true)]
        public ShoppingCartItemModel4WS[] shoppingCartItems
        {
            get
            {
                return this.shoppingCartItemsField;
            }
            set
            {
                this.shoppingCartItemsField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public long userSeqNumber
        {
            get
            {
                return this.userSeqNumberField;
            }
            set
            {
                this.userSeqNumberField = value;
            }
        }
    }
}