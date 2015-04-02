#region Header
/**
 * <pre>
 * 
 *  Accela Citizen Access
 *  File: PaymentModel.cs
 * 
 *  Accela, Inc.
 *  Copyright (C): 2010
 * 
 *  Description:
 * 
 *  Notes:
 * $Id: PaymentModel.cs 182693 2010-10-19 08:24:27Z ACHIEVO\daly.zeng $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */
#endregion Header


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Accela.ACA.WSProxy
{
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.3053")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://model.webservice.accela.com")]
    public partial class PaymentModel : PaymentBaseModel
    {

        private System.Nullable<long> batchTransCodeField;

        private string ccAuthCodeField;

        private string ccHolderNameField;

        private string moduleField;

        private System.Nullable<long> paymentMethodGroupNbrField;

        private System.Nullable<long> posTransSeqField;

        private double processingFeeField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = true)]
        public System.Nullable<long> batchTransCode
        {
            get
            {
                return this.batchTransCodeField;
            }
            set
            {
                this.batchTransCodeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string ccAuthCode
        {
            get
            {
                return this.ccAuthCodeField;
            }
            set
            {
                this.ccAuthCodeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string ccHolderName
        {
            get
            {
                return this.ccHolderNameField;
            }
            set
            {
                this.ccHolderNameField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string module
        {
            get
            {
                return this.moduleField;
            }
            set
            {
                this.moduleField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = true)]
        public System.Nullable<long> paymentMethodGroupNbr
        {
            get
            {
                return this.paymentMethodGroupNbrField;
            }
            set
            {
                this.paymentMethodGroupNbrField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = true)]
        public System.Nullable<long> posTransSeq
        {
            get
            {
                return this.posTransSeqField;
            }
            set
            {
                this.posTransSeqField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public double processingFee
        {
            get
            {
                return this.processingFeeField;
            }
            set
            {
                this.processingFeeField = value;
            }
        }
    }
}
