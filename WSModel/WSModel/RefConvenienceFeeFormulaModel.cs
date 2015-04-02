#region Header
/**
 * <pre>
 * 
 *  Accela Citizen Access
 *  File: RefConvenienceFeeFormulaModel.cs
 * 
 *  Accela, Inc.
 *  Copyright (C): 2013
 * 
 *  Description:
 * 
 *  Notes:
 * $Id: RefConvenienceFeeFormulaModel.cs 181867 2013-03-27 08:06:18Z ACHIEVO\daly.zeng $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */
#endregion Header

namespace Accela.ACA.WSProxy
{
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.1")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://model.webservice.accela.com")]
    public partial class RefConvenienceFeeFormulaModel
    {
        private string productField;

        private SimpleAuditModel auditModelField;

        private string cardTypeField;

        private RefConvenienceFeeFormulaPKModel convenienceFeeFormulaPKModelField;

        private string formulaField;

        private long merchantAccSeqField;

        private bool merchantAccSeqFieldSpecified;

        private string paymentMethodField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string product
        {
            get
            {
                return this.productField;
            }
            set
            {
                this.productField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public SimpleAuditModel auditModel
        {
            get
            {
                return this.auditModelField;
            }
            set
            {
                this.auditModelField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string cardType
        {
            get
            {
                return this.cardTypeField;
            }
            set
            {
                this.cardTypeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public RefConvenienceFeeFormulaPKModel convenienceFeeFormulaPKModel
        {
            get
            {
                return this.convenienceFeeFormulaPKModelField;
            }
            set
            {
                this.convenienceFeeFormulaPKModelField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string formula
        {
            get
            {
                return this.formulaField;
            }
            set
            {
                this.formulaField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public long merchantAccSeq
        {
            get
            {
                return this.merchantAccSeqField;
            }
            set
            {
                this.merchantAccSeqField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool merchantAccSeqSpecified
        {
            get
            {
                return this.merchantAccSeqFieldSpecified;
            }
            set
            {
                this.merchantAccSeqFieldSpecified = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string paymentMethod
        {
            get
            {
                return this.paymentMethodField;
            }
            set
            {
                this.paymentMethodField = value;
            }
        }
    }
}
