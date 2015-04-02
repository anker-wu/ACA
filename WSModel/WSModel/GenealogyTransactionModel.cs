/**
 * <pre>
 * 
 *  Accela Citizen Access
 *  File: ACAModel4WS.cs
 * 
 *  Accela, Inc.
 *  Copyright (C): 2010
 * 
 *  Description:
 * 
 *  Notes:
 * $Id: GenealogyTransactionModel.cs 185933 2010-12-07 03:41:40Z ACHIEVO\daly.zeng $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

namespace Accela.ACA.WSProxy
{
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.3053")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://model.webservice.accela.com")]
    public partial class GenealogyTransactionModel
    {

        private GenealogyModel[] childGenealogyModelsField;

        private string genTranActionField;

        private System.Nullable<System.DateTime> genTranDateField;

        private string genTranDescField;

        private long genTranIDField;

        private ParcelModel[] parcelModelsField;

        private GenealogyModel[] parentGenealogyModelsField;

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        [System.Xml.Serialization.XmlArrayItemAttribute("childGenealogyModel", Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = false)]
        public GenealogyModel[] childGenealogyModels
        {
            get
            {
                return this.childGenealogyModelsField;
            }
            set
            {
                this.childGenealogyModelsField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string genTranAction
        {
            get
            {
                return this.genTranActionField;
            }
            set
            {
                this.genTranActionField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = true)]
        public System.Nullable<System.DateTime> genTranDate
        {
            get
            {
                return this.genTranDateField;
            }
            set
            {
                this.genTranDateField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string genTranDesc
        {
            get
            {
                return this.genTranDescField;
            }
            set
            {
                this.genTranDescField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public long genTranID
        {
            get
            {
                return this.genTranIDField;
            }
            set
            {
                this.genTranIDField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        [System.Xml.Serialization.XmlArrayItemAttribute("parcelModel", Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = false)]
        public ParcelModel[] parcelModels
        {
            get
            {
                return this.parcelModelsField;
            }
            set
            {
                this.parcelModelsField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        [System.Xml.Serialization.XmlArrayItemAttribute("parentGenealogyModel", Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = false)]
        public GenealogyModel[] parentGenealogyModels
        {
            get
            {
                return this.parentGenealogyModelsField;
            }
            set
            {
                this.parentGenealogyModelsField = value;
            }
        }
    }
}
