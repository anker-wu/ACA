#region Header
/**
 * <pre>
 * 
 *  Accela Citizen Access
 *  File: AssetRatingModel.cs
 * 
 *  Accela, Inc.
 *  Copyright (C): 2010
 * 
 *  Description:
 * 
 *  Notes:
 * $Id: AssetRatingModel.cs 181867 2010-10-06 08:06:18Z ACHIEVO\daly.zeng $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */
#endregion Header

namespace Accela.ACA.WSProxy
{
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.3053")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://service.webservice.accela.com/")]
    public partial class AssetRatingModel : AssetRatingPK
    {
        private long assetCAIDField;

        private bool assetCAIDFieldSpecified;

        private long assetSeqField;

        private bool assetSeqFieldSpecified;

        private double endRatingValueField;

        private bool endRatingValueFieldSpecified;

        private string inspectedByDeptField;

        private SysUserModel inspectorField;

        private string preCalcFlagField;

        private double previousValueField;

        private bool previousValueFieldSpecified;

        private string ratingCalcTypeField;

        private System.DateTime ratingDateField;

        private bool ratingDateFieldSpecified;

        private string ratingTimeField;

        private string ratingTypeField;

        private long ratingTypeIdField;

        private bool ratingTypeIdFieldSpecified;

        private double ratingValueField;

        private bool ratingValueFieldSpecified;

        private System.DateTime recDateField;

        private bool recDateFieldSpecified;

        private string recFulNamField;

        private string recStatusField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public long assetCAID
        {
            get
            {
                return this.assetCAIDField;
            }
            set
            {
                this.assetCAIDField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool assetCAIDSpecified
        {
            get
            {
                return this.assetCAIDFieldSpecified;
            }
            set
            {
                this.assetCAIDFieldSpecified = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public long assetSeq
        {
            get
            {
                return this.assetSeqField;
            }
            set
            {
                this.assetSeqField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool assetSeqSpecified
        {
            get
            {
                return this.assetSeqFieldSpecified;
            }
            set
            {
                this.assetSeqFieldSpecified = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public double endRatingValue
        {
            get
            {
                return this.endRatingValueField;
            }
            set
            {
                this.endRatingValueField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool endRatingValueSpecified
        {
            get
            {
                return this.endRatingValueFieldSpecified;
            }
            set
            {
                this.endRatingValueFieldSpecified = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string inspectedByDept
        {
            get
            {
                return this.inspectedByDeptField;
            }
            set
            {
                this.inspectedByDeptField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public SysUserModel inspector
        {
            get
            {
                return this.inspectorField;
            }
            set
            {
                this.inspectorField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string preCalcFlag
        {
            get
            {
                return this.preCalcFlagField;
            }
            set
            {
                this.preCalcFlagField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public double previousValue
        {
            get
            {
                return this.previousValueField;
            }
            set
            {
                this.previousValueField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool previousValueSpecified
        {
            get
            {
                return this.previousValueFieldSpecified;
            }
            set
            {
                this.previousValueFieldSpecified = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string ratingCalcType
        {
            get
            {
                return this.ratingCalcTypeField;
            }
            set
            {
                this.ratingCalcTypeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public System.DateTime ratingDate
        {
            get
            {
                return this.ratingDateField;
            }
            set
            {
                this.ratingDateField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool ratingDateSpecified
        {
            get
            {
                return this.ratingDateFieldSpecified;
            }
            set
            {
                this.ratingDateFieldSpecified = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string ratingTime
        {
            get
            {
                return this.ratingTimeField;
            }
            set
            {
                this.ratingTimeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string ratingType
        {
            get
            {
                return this.ratingTypeField;
            }
            set
            {
                this.ratingTypeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public long ratingTypeId
        {
            get
            {
                return this.ratingTypeIdField;
            }
            set
            {
                this.ratingTypeIdField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool ratingTypeIdSpecified
        {
            get
            {
                return this.ratingTypeIdFieldSpecified;
            }
            set
            {
                this.ratingTypeIdFieldSpecified = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public double ratingValue
        {
            get
            {
                return this.ratingValueField;
            }
            set
            {
                this.ratingValueField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool ratingValueSpecified
        {
            get
            {
                return this.ratingValueFieldSpecified;
            }
            set
            {
                this.ratingValueFieldSpecified = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public System.DateTime recDate
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
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool recDateSpecified
        {
            get
            {
                return this.recDateFieldSpecified;
            }
            set
            {
                this.recDateFieldSpecified = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string recFulNam
        {
            get
            {
                return this.recFulNamField;
            }
            set
            {
                this.recFulNamField = value;
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
    }    
}
