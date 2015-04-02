#region Header
/**
 * <pre>
 * 
 *  Accela Citizen Access
 *  File: AssetAttrTableValueModel.cs
 * 
 *  Accela, Inc.
 *  Copyright (C): 2013
 * 
 *  Description:
 *
 *  Notes:
 *      $Id: AssetAttrTableValueModel.cs 133464 2009-06-05 05:06:34Z ACHIEVO\jackie.yu $.
 *  Revision History
 *  &lt;Date&gt;  &lt;Who&gt;  &lt;What&gt;
 * </pre>
 */
#endregion Header


namespace Accela.ACA.WSProxy
{
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.1015")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://model.webservice.accela.com")]
    public partial class AssetAttrTableValueModel : AttributeValueModel
    {

        private System.Nullable<long> assetSeqNbrField;

        private string attributeValueReqFlagField;

        private System.Nullable<int> colNumberField;

        private string drowDownListFlagField;

        private string errorTipsField;

        private System.Nullable<int> rowNumberField;

        private System.Nullable<long> tableIDField;

        private string tableNameField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = true)]
        public System.Nullable<long> assetSeqNbr
        {
            get
            {
                return this.assetSeqNbrField;
            }
            set
            {
                this.assetSeqNbrField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string attributeValueReqFlag
        {
            get
            {
                return this.attributeValueReqFlagField;
            }
            set
            {
                this.attributeValueReqFlagField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = true)]
        public System.Nullable<int> colNumber
        {
            get
            {
                return this.colNumberField;
            }
            set
            {
                this.colNumberField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string drowDownListFlag
        {
            get
            {
                return this.drowDownListFlagField;
            }
            set
            {
                this.drowDownListFlagField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string errorTips
        {
            get
            {
                return this.errorTipsField;
            }
            set
            {
                this.errorTipsField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = true)]
        public System.Nullable<int> rowNumber
        {
            get
            {
                return this.rowNumberField;
            }
            set
            {
                this.rowNumberField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = true)]
        public System.Nullable<long> tableID
        {
            get
            {
                return this.tableIDField;
            }
            set
            {
                this.tableIDField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string tableName
        {
            get
            {
                return this.tableNameField;
            }
            set
            {
                this.tableNameField = value;
            }
        }
    }
}