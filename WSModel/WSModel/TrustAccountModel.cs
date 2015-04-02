/**
 * <pre>
 * 
 *  Accela Citizen Access
 *  File: TrustAccountModel.cs
 * 
 *  Accela, Inc.
 *  Copyright (C): 2010-2011
 * 
 *  Description:
 * 
 *  Notes:
 * $Id: TrustAccountModel.cs 130107 2009-05-11 12:23:56Z ACHIEVO\jackie.yu $.
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
    public partial class TrustAccountModel : TrustAccountBaseModel
    {

        private System.Nullable<double> acctBalanceField;

        private System.Nullable<long> acctSeqField;

        private CapIDModel associatedCapIDField;

        private string associationsField;

        private System.Nullable<double> overdraftLimitField;

        private string primaryField;

        private double thresholdAmountField;

        private bool thresholdAmountFieldSpecified;

        private TrustAccountTransactionModel[] trustAccountTransactionModelsField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = true)]
        public System.Nullable<double> acctBalance
        {
            get
            {
                return this.acctBalanceField;
            }
            set
            {
                this.acctBalanceField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = true)]
        public System.Nullable<long> acctSeq
        {
            get
            {
                return this.acctSeqField;
            }
            set
            {
                this.acctSeqField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public CapIDModel associatedCapID
        {
            get
            {
                return this.associatedCapIDField;
            }
            set
            {
                this.associatedCapIDField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string associations
        {
            get
            {
                return this.associationsField;
            }
            set
            {
                this.associationsField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = true)]
        public System.Nullable<double> overdraftLimit
        {
            get
            {
                return this.overdraftLimitField;
            }
            set
            {
                this.overdraftLimitField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string primary
        {
            get
            {
                return this.primaryField;
            }
            set
            {
                this.primaryField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public double thresholdAmount
        {
            get
            {
                return this.thresholdAmountField;
            }
            set
            {
                this.thresholdAmountField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool thresholdAmountSpecified
        {
            get
            {
                return this.thresholdAmountFieldSpecified;
            }
            set
            {
                this.thresholdAmountFieldSpecified = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        [System.Xml.Serialization.XmlArrayItemAttribute("trustAccountTransactionModel", Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = false)]
        public TrustAccountTransactionModel[] trustAccountTransactionModels
        {
            get
            {
                return this.trustAccountTransactionModelsField;
            }
            set
            {
                this.trustAccountTransactionModelsField = value;
            }
        }
    }
}
