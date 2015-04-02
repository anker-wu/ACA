#region Header
/**
 * <pre>
 * 
 *  Accela Citizen Access
 *  File: GGuideSheetModel.cs
 * 
 *  Accela, Inc.
 *  Copyright (C): 2010
 * 
 *  Description:
 * 
 *  Notes:
 * $Id: GGuideSheetModel.cs 181867 2010-10-06 08:06:18Z ACHIEVO\daly.zeng $.
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
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://model.webservice.accela.com")]
    public partial class GGuideSheetModel : LanguageModel
    {

        private string activityNumberField;

        private System.DateTime auditDateField;

        private bool auditDateFieldSpecified;

        private string auditIDField;

        private string auditStatusField;

        private CapIDModel capIDField;

        private string guideDescField;

        private string guideStatusField;

        private string guideTypeField;

        private long guidesheetSeqNbrField;

        private bool guidesheetSeqNbrFieldSpecified;

        private string identifierField;

        private GGuideSheetItemModel[] gGuideSheetItemModelsField;

        private string resGuideTypeField;

        private string serviceProviderCodeField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string activityNumber
        {
            get
            {
                return this.activityNumberField;
            }
            set
            {
                this.activityNumberField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public System.DateTime auditDate
        {
            get
            {
                return this.auditDateField;
            }
            set
            {
                this.auditDateField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool auditDateSpecified
        {
            get
            {
                return this.auditDateFieldSpecified;
            }
            set
            {
                this.auditDateFieldSpecified = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string auditID
        {
            get
            {
                return this.auditIDField;
            }
            set
            {
                this.auditIDField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string auditStatus
        {
            get
            {
                return this.auditStatusField;
            }
            set
            {
                this.auditStatusField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public CapIDModel capID
        {
            get
            {
                return this.capIDField;
            }
            set
            {
                this.capIDField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string guideDesc
        {
            get
            {
                return this.guideDescField;
            }
            set
            {
                this.guideDescField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string guideStatus
        {
            get
            {
                return this.guideStatusField;
            }
            set
            {
                this.guideStatusField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string guideType
        {
            get
            {
                return this.guideTypeField;
            }
            set
            {
                this.guideTypeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public long guidesheetSeqNbr
        {
            get
            {
                return this.guidesheetSeqNbrField;
            }
            set
            {
                this.guidesheetSeqNbrField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool guidesheetSeqNbrSpecified
        {
            get
            {
                return this.guidesheetSeqNbrFieldSpecified;
            }
            set
            {
                this.guidesheetSeqNbrFieldSpecified = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string identifier
        {
            get
            {
                return this.identifierField;
            }
            set
            {
                this.identifierField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        [System.Xml.Serialization.XmlArrayItemAttribute("gGuideSheetItemModel", Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = false)]
        public GGuideSheetItemModel[] gGuideSheetItemModels
        {
            get
            {
                return this.gGuideSheetItemModelsField;
            }
            set
            {
                this.gGuideSheetItemModelsField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string resGuideType
        {
            get
            {
                return this.resGuideTypeField;
            }
            set
            {
                this.resGuideTypeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string serviceProviderCode
        {
            get
            {
                return this.serviceProviderCodeField;
            }
            set
            {
                this.serviceProviderCodeField = value;
            }
        }
    }
}