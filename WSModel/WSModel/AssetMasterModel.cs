#region Header
/**
 * <pre>
 * 
 *  Accela Citizen Access
 *  File: AssetMasterModel.cs
 * 
 *  Accela, Inc.
 *  Copyright (C): 2010-2013
 * 
 *  Description:
 * 
 *  Notes:
 * $Id: AssetMasterModel.cs 181867 2010-10-06 08:06:18Z ACHIEVO\daly.zeng $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
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
    public partial class AssetMasterModel : LanguageModel
    {

        private AssetOrderModel assetOrderModelField;

        private AssetRatingModel[] assetRatingListField;

        private System.Nullable<double> assetSizeField;

        private string capTypeStringField;

        private System.Nullable<decimal> costField;

        private System.Nullable<decimal> costLTDField;

        private System.Nullable<double> currentValueField;

        private System.Nullable<System.DateTime> dateOfServiceField;

        private string dependentFlagField;

        private System.Nullable<double> depreciationAmountField;

        private System.Nullable<double> depreciationValueField;

        private string endAssetIDField;

        private System.Nullable<double> endAssetSizeField;

        private System.Nullable<double> endCurrentValueField;

        private System.Nullable<System.DateTime> endDateField;

        private System.Nullable<System.DateTime> endDateOfServiceField;

        private System.Nullable<System.DateTime> endEndDateField;

        private System.Nullable<System.DateTime> endStartDateField;

        private System.Nullable<System.DateTime> endStatusDateField;

        private string g1AssetCommentsField;

        private string g1AssetGroupField;

        private string g1AssetIDField;

        private string g1AssetNameField;

        private System.Nullable<long> g1AssetSequenceNumberField;

        private string g1AssetStatusField;

        private System.Nullable<System.DateTime> g1AssetStatusDateField;

        private string g1AssetTypeField;

        private string g1ClassTypeField;

        private string g1DescriptionField;

        private string parentAssetField;

        private System.Nullable<System.DateTime> recDateField;

        private string recFulNamField;

        private string recStatusField;

        private RefAddressModel[] refAddressListField;

        private RefAddressModel refAddressModelField;

        private AssetContactModel[] refContactsListField;

        private string resG1AssetCommentsField;

        private string resG1AssetNameField;

        private string resG1DescriptionField;

        private System.Nullable<double> salvageValueField;

        private string serviceProviderCodeField;

        private string sizeUnitField;

        private string startAssetIDField;

        private System.Nullable<System.DateTime> startDateField;

        private System.Nullable<double> startValueField;

        private System.Nullable<long> totalNumField;

        private System.Nullable<double> useFulLifeField;

        private WorkOrderAssetModel workOrderAssetModelField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public AssetOrderModel assetOrderModel
        {
            get
            {
                return this.assetOrderModelField;
            }
            set
            {
                this.assetOrderModelField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("assetRatingList", Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = true)]
        public AssetRatingModel[] assetRatingList
        {
            get
            {
                return this.assetRatingListField;
            }
            set
            {
                this.assetRatingListField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = true)]
        public System.Nullable<double> assetSize
        {
            get
            {
                return this.assetSizeField;
            }
            set
            {
                this.assetSizeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string capTypeString
        {
            get
            {
                return this.capTypeStringField;
            }
            set
            {
                this.capTypeStringField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = true)]
        public System.Nullable<decimal> cost
        {
            get
            {
                return this.costField;
            }
            set
            {
                this.costField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = true)]
        public System.Nullable<decimal> costLTD
        {
            get
            {
                return this.costLTDField;
            }
            set
            {
                this.costLTDField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = true)]
        public System.Nullable<double> currentValue
        {
            get
            {
                return this.currentValueField;
            }
            set
            {
                this.currentValueField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = true)]
        public System.Nullable<System.DateTime> dateOfService
        {
            get
            {
                return this.dateOfServiceField;
            }
            set
            {
                this.dateOfServiceField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string dependentFlag
        {
            get
            {
                return this.dependentFlagField;
            }
            set
            {
                this.dependentFlagField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = true)]
        public System.Nullable<double> depreciationAmount
        {
            get
            {
                return this.depreciationAmountField;
            }
            set
            {
                this.depreciationAmountField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = true)]
        public System.Nullable<double> depreciationValue
        {
            get
            {
                return this.depreciationValueField;
            }
            set
            {
                this.depreciationValueField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string endAssetID
        {
            get
            {
                return this.endAssetIDField;
            }
            set
            {
                this.endAssetIDField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = true)]
        public System.Nullable<double> endAssetSize
        {
            get
            {
                return this.endAssetSizeField;
            }
            set
            {
                this.endAssetSizeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = true)]
        public System.Nullable<double> endCurrentValue
        {
            get
            {
                return this.endCurrentValueField;
            }
            set
            {
                this.endCurrentValueField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = true)]
        public System.Nullable<System.DateTime> endDate
        {
            get
            {
                return this.endDateField;
            }
            set
            {
                this.endDateField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = true)]
        public System.Nullable<System.DateTime> endDateOfService
        {
            get
            {
                return this.endDateOfServiceField;
            }
            set
            {
                this.endDateOfServiceField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = true)]
        public System.Nullable<System.DateTime> endEndDate
        {
            get
            {
                return this.endEndDateField;
            }
            set
            {
                this.endEndDateField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = true)]
        public System.Nullable<System.DateTime> endStartDate
        {
            get
            {
                return this.endStartDateField;
            }
            set
            {
                this.endStartDateField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = true)]
        public System.Nullable<System.DateTime> endStatusDate
        {
            get
            {
                return this.endStatusDateField;
            }
            set
            {
                this.endStatusDateField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string g1AssetComments
        {
            get
            {
                return this.g1AssetCommentsField;
            }
            set
            {
                this.g1AssetCommentsField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string g1AssetGroup
        {
            get
            {
                return this.g1AssetGroupField;
            }
            set
            {
                this.g1AssetGroupField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string g1AssetID
        {
            get
            {
                return this.g1AssetIDField;
            }
            set
            {
                this.g1AssetIDField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string g1AssetName
        {
            get
            {
                return this.g1AssetNameField;
            }
            set
            {
                this.g1AssetNameField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = true)]
        public System.Nullable<long> g1AssetSequenceNumber
        {
            get
            {
                return this.g1AssetSequenceNumberField;
            }
            set
            {
                this.g1AssetSequenceNumberField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string g1AssetStatus
        {
            get
            {
                return this.g1AssetStatusField;
            }
            set
            {
                this.g1AssetStatusField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = true)]
        public System.Nullable<System.DateTime> g1AssetStatusDate
        {
            get
            {
                return this.g1AssetStatusDateField;
            }
            set
            {
                this.g1AssetStatusDateField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string g1AssetType
        {
            get
            {
                return this.g1AssetTypeField;
            }
            set
            {
                this.g1AssetTypeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string g1ClassType
        {
            get
            {
                return this.g1ClassTypeField;
            }
            set
            {
                this.g1ClassTypeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string g1Description
        {
            get
            {
                return this.g1DescriptionField;
            }
            set
            {
                this.g1DescriptionField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string parentAsset
        {
            get
            {
                return this.parentAssetField;
            }
            set
            {
                this.parentAssetField = value;
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

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("refAddressList", Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = true)]
        public RefAddressModel[] refAddressList
        {
            get
            {
                return this.refAddressListField;
            }
            set
            {
                this.refAddressListField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public RefAddressModel refAddressModel
        {
            get
            {
                return this.refAddressModelField;
            }
            set
            {
                this.refAddressModelField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("refContactsList", Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = true)]
        public AssetContactModel[] refContactsList
        {
            get
            {
                return this.refContactsListField;
            }
            set
            {
                this.refContactsListField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string resG1AssetComments
        {
            get
            {
                return this.resG1AssetCommentsField;
            }
            set
            {
                this.resG1AssetCommentsField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string resG1AssetName
        {
            get
            {
                return this.resG1AssetNameField;
            }
            set
            {
                this.resG1AssetNameField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string resG1Description
        {
            get
            {
                return this.resG1DescriptionField;
            }
            set
            {
                this.resG1DescriptionField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = true)]
        public System.Nullable<double> salvageValue
        {
            get
            {
                return this.salvageValueField;
            }
            set
            {
                this.salvageValueField = value;
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
        public string sizeUnit
        {
            get
            {
                return this.sizeUnitField;
            }
            set
            {
                this.sizeUnitField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string startAssetID
        {
            get
            {
                return this.startAssetIDField;
            }
            set
            {
                this.startAssetIDField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = true)]
        public System.Nullable<System.DateTime> startDate
        {
            get
            {
                return this.startDateField;
            }
            set
            {
                this.startDateField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = true)]
        public System.Nullable<double> startValue
        {
            get
            {
                return this.startValueField;
            }
            set
            {
                this.startValueField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = true)]
        public System.Nullable<long> totalNum
        {
            get
            {
                return this.totalNumField;
            }
            set
            {
                this.totalNumField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = true)]
        public System.Nullable<double> useFulLife
        {
            get
            {
                return this.useFulLifeField;
            }
            set
            {
                this.useFulLifeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public WorkOrderAssetModel workOrderAssetModel
        {
            get
            {
                return this.workOrderAssetModelField;
            }
            set
            {
                this.workOrderAssetModelField = value;
            }
        }
    }
}
