/**
 * <pre>
 * 
 *  Accela Citizen Access
 *  File: ParcelModel.cs
 * 
 *  Accela, Inc.
 *  Copyright (C): 2010-2011
 * 
 *  Description:
 * 
 *  Notes:
 * $Id: ParcelModel.cs 158744 2009-11-27 03:34:37Z ACHIEVO\daly.zeng $.
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
    public partial class ParcelModel : LanguageModel
    {

        private System.Nullable<System.DateTime> auditDateField;

        private string auditIDField;

        private string auditStatusField;

        private string blockField;

        private string bookField;

        private CapIDModel capIDField;

        private string censusTractField;

        private string councilDistrictField;

        private DuplicatedAPOKeyModel[] duplicatedAPOKeysField;

        private System.Nullable<long> eventIDField;

        private System.Nullable<double> exemptValueField;

        private GISObjectModel[] gisObjectListField;

        private System.Nullable<long> gisSeqNoField;

        private NoticeConditionModel hightestConditionField;

        private System.Nullable<double> improvedValueField;

        private string inspectionDistrictField;

        private System.Nullable<double> landValueField;

        private string legalDescField;

        private string lotField;

        private string mapNoField;

        private string mapRefField;

        private NoticeConditionModel[] noticeConditionsField;

        private string pageField;

        private string parcelField;

        private System.Nullable<double> parcelAreaField;

        private string parcelNumberField;

        private string parcelStatusField;

        private string planAreaField;

        private string primaryParcelFlagField;

        private string rangeField;

        private string[] refAddressTypesField;

        private string resSubdivisionField;

        private System.Nullable<int> sectionField;

        private System.Nullable<long> sourceSeqNumberField;

        private string subdivisionField;

        private string supervisorDistrictField;

        private TemplateAttributeModel[] templatesField;

        private string townshipField;

        private string tractField;

        private string uIDField;

        private string unmaskedParcelNumberField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = true)]
        public System.Nullable<System.DateTime> auditDate
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
        public string block
        {
            get
            {
                return this.blockField;
            }
            set
            {
                this.blockField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string book
        {
            get
            {
                return this.bookField;
            }
            set
            {
                this.bookField = value;
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
        public string censusTract
        {
            get
            {
                return this.censusTractField;
            }
            set
            {
                this.censusTractField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string councilDistrict
        {
            get
            {
                return this.councilDistrictField;
            }
            set
            {
                this.councilDistrictField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        [System.Xml.Serialization.XmlArrayItemAttribute("duplicatedAPOKey", Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = false)]
        public DuplicatedAPOKeyModel[] duplicatedAPOKeys
        {
            get
            {
                return this.duplicatedAPOKeysField;
            }
            set
            {
                this.duplicatedAPOKeysField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = true)]
        public System.Nullable<long> eventID
        {
            get
            {
                return this.eventIDField;
            }
            set
            {
                this.eventIDField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = true)]
        public System.Nullable<double> exemptValue
        {
            get
            {
                return this.exemptValueField;
            }
            set
            {
                this.exemptValueField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("gisObjectList", Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = true)]
        public GISObjectModel[] gisObjectList
        {
            get
            {
                return this.gisObjectListField;
            }
            set
            {
                this.gisObjectListField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = true)]
        public System.Nullable<long> gisSeqNo
        {
            get
            {
                return this.gisSeqNoField;
            }
            set
            {
                this.gisSeqNoField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public NoticeConditionModel hightestCondition
        {
            get
            {
                return this.hightestConditionField;
            }
            set
            {
                this.hightestConditionField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = true)]
        public System.Nullable<double> improvedValue
        {
            get
            {
                return this.improvedValueField;
            }
            set
            {
                this.improvedValueField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string inspectionDistrict
        {
            get
            {
                return this.inspectionDistrictField;
            }
            set
            {
                this.inspectionDistrictField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = true)]
        public System.Nullable<double> landValue
        {
            get
            {
                return this.landValueField;
            }
            set
            {
                this.landValueField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string legalDesc
        {
            get
            {
                return this.legalDescField;
            }
            set
            {
                this.legalDescField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string lot
        {
            get
            {
                return this.lotField;
            }
            set
            {
                this.lotField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string mapNo
        {
            get
            {
                return this.mapNoField;
            }
            set
            {
                this.mapNoField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string mapRef
        {
            get
            {
                return this.mapRefField;
            }
            set
            {
                this.mapRefField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("noticeConditions", Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = true)]
        public NoticeConditionModel[] noticeConditions
        {
            get
            {
                return this.noticeConditionsField;
            }
            set
            {
                this.noticeConditionsField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string page
        {
            get
            {
                return this.pageField;
            }
            set
            {
                this.pageField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string parcel
        {
            get
            {
                return this.parcelField;
            }
            set
            {
                this.parcelField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = true)]
        public System.Nullable<double> parcelArea
        {
            get
            {
                return this.parcelAreaField;
            }
            set
            {
                this.parcelAreaField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string parcelNumber
        {
            get
            {
                return this.parcelNumberField;
            }
            set
            {
                this.parcelNumberField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string parcelStatus
        {
            get
            {
                return this.parcelStatusField;
            }
            set
            {
                this.parcelStatusField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string planArea
        {
            get
            {
                return this.planAreaField;
            }
            set
            {
                this.planAreaField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string primaryParcelFlag
        {
            get
            {
                return this.primaryParcelFlagField;
            }
            set
            {
                this.primaryParcelFlagField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string range
        {
            get
            {
                return this.rangeField;
            }
            set
            {
                this.rangeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("refAddressTypes", Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = true)]
        public string[] refAddressTypes
        {
            get
            {
                return this.refAddressTypesField;
            }
            set
            {
                this.refAddressTypesField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string resSubdivision
        {
            get
            {
                return this.resSubdivisionField;
            }
            set
            {
                this.resSubdivisionField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = true)]
        public System.Nullable<int> section
        {
            get
            {
                return this.sectionField;
            }
            set
            {
                this.sectionField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = true)]
        public System.Nullable<long> sourceSeqNumber
        {
            get
            {
                return this.sourceSeqNumberField;
            }
            set
            {
                this.sourceSeqNumberField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string subdivision
        {
            get
            {
                return this.subdivisionField;
            }
            set
            {
                this.subdivisionField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string supervisorDistrict
        {
            get
            {
                return this.supervisorDistrictField;
            }
            set
            {
                this.supervisorDistrictField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        [System.Xml.Serialization.XmlArrayItemAttribute("template", Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = false)]
        public TemplateAttributeModel[] templates
        {
            get
            {
                return this.templatesField;
            }
            set
            {
                this.templatesField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string township
        {
            get
            {
                return this.townshipField;
            }
            set
            {
                this.townshipField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string tract
        {
            get
            {
                return this.tractField;
            }
            set
            {
                this.tractField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string UID
        {
            get
            {
                return this.uIDField;
            }
            set
            {
                this.uIDField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string unmaskedParcelNumber
        {
            get
            {
                return this.unmaskedParcelNumberField;
            }
            set
            {
                this.unmaskedParcelNumberField = value;
            }
        }
    }
}
