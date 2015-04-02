#region Header

/**
 *  Accela Citizen Access
 *  File: AssetTypeModel.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2013
 *
 *  Description:
 *  
 *
 *  Notes:
 * $Id: AssetTypeModel.cs 134054 2013-10-16 18:03:54Z ACHIEVO\eric.he $.
 *  Revision History
 *  Date,                  Who,                 What
 */

#endregion Header

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Accela.ACA.WSProxy
{

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.1015")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://model.webservice.accela.com")]
    public partial class AssetTypeModel : LanguageModel
    {

        private string assetCommentsField;

        private string assetDescField;

        private string assetGroupField;

        private string assetIDMaskNameField;

        private string assetIconSrcTypeField;

        private string assetIdMaskField;

        private string assetStandardIconField;

        private string assetTypeField;

        private string assetTypeCodeField;

        private byte[] assetTypeIconField;

        private string attrTemplateIdField;

        private string classTypeField;

        private GisRelatedTableModel gISRelatedTableModelField;

        private string gisIdForAssetIdField;

        private string gisLayerField;

        private string gisRelatedTableField;

        private System.Nullable<long> gisRelatedTableIDField;

        private string gisServiceField;

        private string mapToGISField;

        private string r1ResourceFlagField;

        private System.Nullable<System.DateTime> recDateField;

        private string recFullNameField;

        private string recStatusField;

        private string relationshipField;

        private byte[] resAssetIconField;

        private string resAssetTypeField;

        private System.Nullable<long> seqNbrField;

        private string seqResetField;

        private string serviceProviderCodeField;

        private string sizeRequiredField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string assetComments
        {
            get
            {
                return this.assetCommentsField;
            }
            set
            {
                this.assetCommentsField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string assetDesc
        {
            get
            {
                return this.assetDescField;
            }
            set
            {
                this.assetDescField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string assetGroup
        {
            get
            {
                return this.assetGroupField;
            }
            set
            {
                this.assetGroupField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string assetIDMaskName
        {
            get
            {
                return this.assetIDMaskNameField;
            }
            set
            {
                this.assetIDMaskNameField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string assetIconSrcType
        {
            get
            {
                return this.assetIconSrcTypeField;
            }
            set
            {
                this.assetIconSrcTypeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string assetIdMask
        {
            get
            {
                return this.assetIdMaskField;
            }
            set
            {
                this.assetIdMaskField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string assetStandardIcon
        {
            get
            {
                return this.assetStandardIconField;
            }
            set
            {
                this.assetStandardIconField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string assetType
        {
            get
            {
                return this.assetTypeField;
            }
            set
            {
                this.assetTypeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string assetTypeCode
        {
            get
            {
                return this.assetTypeCodeField;
            }
            set
            {
                this.assetTypeCodeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, DataType = "base64Binary")]
        public byte[] assetTypeIcon
        {
            get
            {
                return this.assetTypeIconField;
            }
            set
            {
                this.assetTypeIconField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string attrTemplateId
        {
            get
            {
                return this.attrTemplateIdField;
            }
            set
            {
                this.attrTemplateIdField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string classType
        {
            get
            {
                return this.classTypeField;
            }
            set
            {
                this.classTypeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public GisRelatedTableModel GISRelatedTableModel
        {
            get
            {
                return this.gISRelatedTableModelField;
            }
            set
            {
                this.gISRelatedTableModelField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string gisIdForAssetId
        {
            get
            {
                return this.gisIdForAssetIdField;
            }
            set
            {
                this.gisIdForAssetIdField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string gisLayer
        {
            get
            {
                return this.gisLayerField;
            }
            set
            {
                this.gisLayerField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string gisRelatedTable
        {
            get
            {
                return this.gisRelatedTableField;
            }
            set
            {
                this.gisRelatedTableField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = true)]
        public System.Nullable<long> gisRelatedTableID
        {
            get
            {
                return this.gisRelatedTableIDField;
            }
            set
            {
                this.gisRelatedTableIDField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string gisService
        {
            get
            {
                return this.gisServiceField;
            }
            set
            {
                this.gisServiceField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string mapToGIS
        {
            get
            {
                return this.mapToGISField;
            }
            set
            {
                this.mapToGISField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string r1ResourceFlag
        {
            get
            {
                return this.r1ResourceFlagField;
            }
            set
            {
                this.r1ResourceFlagField = value;
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
        public string relationship
        {
            get
            {
                return this.relationshipField;
            }
            set
            {
                this.relationshipField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, DataType = "base64Binary")]
        public byte[] resAssetIcon
        {
            get
            {
                return this.resAssetIconField;
            }
            set
            {
                this.resAssetIconField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string resAssetType
        {
            get
            {
                return this.resAssetTypeField;
            }
            set
            {
                this.resAssetTypeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = true)]
        public System.Nullable<long> seqNbr
        {
            get
            {
                return this.seqNbrField;
            }
            set
            {
                this.seqNbrField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string seqReset
        {
            get
            {
                return this.seqResetField;
            }
            set
            {
                this.seqResetField = value;
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

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string sizeRequired
        {
            get
            {
                return this.sizeRequiredField;
            }
            set
            {
                this.sizeRequiredField = value;
            }
        }
    }
}
