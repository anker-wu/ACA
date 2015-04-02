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
 * $Id: GenealogyModel.cs 185933 2010-12-07 03:41:40Z ACHIEVO\daly.zeng $.
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
    public partial class GenealogyModel
    {

        private long genSeqNbrField;

        private string genSourceField;

        private long genStageNbrField;

        private long genTranIDField;

        private string gisIDField;

        private string objectNbrField;

        private string objectTypeField;

        private System.Nullable<System.DateTime> recDateField;

        private string recFullNameField;

        private string recStatusField;

        private string unmaskedObjectNbrField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public long genSeqNbr
        {
            get
            {
                return this.genSeqNbrField;
            }
            set
            {
                this.genSeqNbrField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string genSource
        {
            get
            {
                return this.genSourceField;
            }
            set
            {
                this.genSourceField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public long genStageNbr
        {
            get
            {
                return this.genStageNbrField;
            }
            set
            {
                this.genStageNbrField = value;
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
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string gisID
        {
            get
            {
                return this.gisIDField;
            }
            set
            {
                this.gisIDField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string objectNbr
        {
            get
            {
                return this.objectNbrField;
            }
            set
            {
                this.objectNbrField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string objectType
        {
            get
            {
                return this.objectTypeField;
            }
            set
            {
                this.objectTypeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = true)]
        public System.Nullable<System.DateTime> recDate
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
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string unmaskedObjectNbr
        {
            get
            {
                return this.unmaskedObjectNbrField;
            }
            set
            {
                this.unmaskedObjectNbrField = value;
            }
        }
    }
}
