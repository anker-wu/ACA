/**
 * <pre>
 * 
 *  Accela Citizen Access
 *  File: PaymentResultModel.cs
 * 
 *  Accela, Inc.
 *  Copyright (C): 2007-2009
 * 
 *  Description:
 * 
 *  Notes:
 * $Id: PaymentResultModel.cs 130107 2009-05-11 12:23:56Z ACHIEVO\jackie.yu $.
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
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://adaptor.onlinePayment.cashier.finance.aa.accela.com")]
    public partial class PaymentResultModel
    {

        private string authCodeField;

        private string resultCodeField;

        private string resultMessageField;

        private string transCodeField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public string authCode
        {
            get
            {
                return this.authCodeField;
            }
            set
            {
                this.authCodeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public string resultCode
        {
            get
            {
                return this.resultCodeField;
            }
            set
            {
                this.resultCodeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public string resultMessage
        {
            get
            {
                return this.resultMessageField;
            }
            set
            {
                this.resultMessageField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public string transCode
        {
            get
            {
                return this.transCodeField;
            }
            set
            {
                this.transCodeField = value;
            }
        }
    }
}
