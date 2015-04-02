#region Header
/**
 * <pre>
 * 
 *  Accela Citizen Access
 *  File: RefMerchantAccountModel.cs
 * 
 *  Accela, Inc.
 *  Copyright (C): 2013
 * 
 *  Description:
 * 
 *  Notes:
 * $Id: RefMerchantAccountModel.cs 181867 2013-03-27 08:06:18Z ACHIEVO\daly.zeng $.
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
    public partial class RefMerchantAccountModel
    {

        private string accountNameField;

        private SimpleAuditModel auditModelField;

        private string categoryField;

        private string convenienceAccNumberField;

        private string convenienceAccPasswordField;

        private string convenienceAccSettleNumberField;

        private string descriptionField;

        private RefConvenienceFeeFormulaModel[] formulasField;

        private string groupField;

        private MerchantAccountPKModel merchantAccountPKModelField;

        private string principalAccNumberField;

        private string principalAccPasswordField;

        private string principalAccSettleNumberField;

        private string subtypeField;

        private string typeField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string accountName
        {
            get
            {
                return this.accountNameField;
            }
            set
            {
                this.accountNameField = value;
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
        public string category
        {
            get
            {
                return this.categoryField;
            }
            set
            {
                this.categoryField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string convenienceAccNumber
        {
            get
            {
                return this.convenienceAccNumberField;
            }
            set
            {
                this.convenienceAccNumberField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string convenienceAccPassword
        {
            get
            {
                return this.convenienceAccPasswordField;
            }
            set
            {
                this.convenienceAccPasswordField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string convenienceAccSettleNumber
        {
            get
            {
                return this.convenienceAccSettleNumberField;
            }
            set
            {
                this.convenienceAccSettleNumberField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string description
        {
            get
            {
                return this.descriptionField;
            }
            set
            {
                this.descriptionField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("formulas", Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = true)]
        public RefConvenienceFeeFormulaModel[] formulas
        {
            get
            {
                return this.formulasField;
            }
            set
            {
                this.formulasField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string group
        {
            get
            {
                return this.groupField;
            }
            set
            {
                this.groupField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public MerchantAccountPKModel merchantAccountPKModel
        {
            get
            {
                return this.merchantAccountPKModelField;
            }
            set
            {
                this.merchantAccountPKModelField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string principalAccNumber
        {
            get
            {
                return this.principalAccNumberField;
            }
            set
            {
                this.principalAccNumberField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string principalAccPassword
        {
            get
            {
                return this.principalAccPasswordField;
            }
            set
            {
                this.principalAccPasswordField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string principalAccSettleNumber
        {
            get
            {
                return this.principalAccSettleNumberField;
            }
            set
            {
                this.principalAccSettleNumberField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string subtype
        {
            get
            {
                return this.subtypeField;
            }
            set
            {
                this.subtypeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string type
        {
            get
            {
                return this.typeField;
            }
            set
            {
                this.typeField = value;
            }
        }
    }
}
