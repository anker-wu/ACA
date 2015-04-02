/**
 * <pre>
 * 
 *  Accela Citizen Access
 *  File: RefAddressModel.cs
 * 
 *  Accela, Inc.
 *  Copyright (C): 2010-2014
 * 
 *  Description:
 * 
 *  Notes:
 * $Id: RefAddressModel.cs 130107 2009-05-11 12:23:56Z ACHIEVO\jackie.yu $.
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
    public partial class RefAddressModel
    {

        private string addressDescriptionField;

        private string addressLine1Field;

        private string addressLine2Field;

        private string addressStatusField;

        private string addressTypeField;

        private string addressTypeFlagField;

        private System.Nullable<System.DateTime> auditDateField;

        private string auditIDField;

        private string auditStatusField;

        private string cityField;

        private string countryField;

        private string countryCodeField;

        private string countyField;

        private System.Nullable<double> distanceField;

        private DuplicatedAPOKeyModel[] duplicatedAPOKeysField;

        private string eventIDField;

        private string fullAddressField;

        private NoticeConditionModel hightestConditionField;

        private string houseFractionEndField;

        private string houseFractionStartField;

        private System.Nullable<int> houseNumberEndField;

        private System.Nullable<int> houseNumberRangeEndField;

        private System.Nullable<int> houseNumberRangeStartField;

        private System.Nullable<int> houseNumberStartField;

        private string inspectionDistrictField;

        private string inspectionDistrictPrefixField;
        
        private string levelPrefixField;
		
        private string levelNumberStartField;        
		
		private string levelNumberEndField;

        private string houseNumberAlphaStartField;
		
		private string houseNumberAlphaEndField;

        private string lotField;

        private string mappingDailyAddressNbrField;

        private string neighborhoodPrefixField;

        private string neighborhoodField;

        private NoticeConditionModel[] noticeConditionsField;

        private ParcelInfoModel[] parcelListsField;

        private string parcelNumberField;

        private string primaryFlagField;

        private System.Nullable<long> refAddressIdField;

        private string[] refAddressTypesField;

        private string resCountryCodeField;

        private string resStateField;

        private string resStreetDirectionField;

        private string resStreetSuffixField;

        private string resStreetSuffixdirectionField;

        private string resUnitTypeField;

        private string secondaryRoadField;

        private System.Nullable<int> secondaryRoadNumberField;

        private string sourceFlagField;

        private System.Nullable<int> sourceNumberField;

        private string stateField;

        private string streetDirectionField;

        private string streetNameField;

        private string streetPrefixField;

        private string streetSuffixField;

        private string streetSuffixdirectionField;

        private string subdivisionField;

        private TemplateAttributeModel[] templatesField;

        private string uIDField;

        private string unitEndField;

        private string unitRangeEndField;

        private string unitRangeStartField;

        private string unitStartField;

        private string unitTypeField;

        private System.Nullable<double> xCoordinatorField;

        private System.Nullable<double> yCoordinatorField;

        private string zipField;

        private System.Nullable<int> houseNumberStartFromField;

        private System.Nullable<int> houseNumberStartToField;

        private System.Nullable<int> houseNumberEndFromField;

        private System.Nullable<int> houseNumberEndToField;

            /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string addressDescription
        {
            get
            {
                return this.addressDescriptionField;
            }
            set
            {
                this.addressDescriptionField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string addressLine1
        {
            get
            {
                return this.addressLine1Field;
            }
            set
            {
                this.addressLine1Field = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string addressLine2
        {
            get
            {
                return this.addressLine2Field;
            }
            set
            {
                this.addressLine2Field = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string addressStatus
        {
            get
            {
                return this.addressStatusField;
            }
            set
            {
                this.addressStatusField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string addressType
        {
            get
            {
                return this.addressTypeField;
            }
            set
            {
                this.addressTypeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string addressTypeFlag
        {
            get
            {
                return this.addressTypeFlagField;
            }
            set
            {
                this.addressTypeFlagField = value;
            }
        }

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
        public string city
        {
            get
            {
                return this.cityField;
            }
            set
            {
                this.cityField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string country
        {
            get
            {
                return this.countryField;
            }
            set
            {
                this.countryField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string countryCode
        {
            get
            {
                return this.countryCodeField;
            }
            set
            {
                this.countryCodeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string county
        {
            get
            {
                return this.countyField;
            }
            set
            {
                this.countyField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = true)]
        public System.Nullable<double> distance
        {
            get
            {
                return this.distanceField;
            }
            set
            {
                this.distanceField = value;
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
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string eventID
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
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string fullAddress
        {
            get
            {
                return this.fullAddressField;
            }
            set
            {
                this.fullAddressField = value;
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
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string houseFractionEnd
        {
            get
            {
                return this.houseFractionEndField;
            }
            set
            {
                this.houseFractionEndField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string houseFractionStart
        {
            get
            {
                return this.houseFractionStartField;
            }
            set
            {
                this.houseFractionStartField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = true)]
        public System.Nullable<int> houseNumberEnd
        {
            get
            {
                return this.houseNumberEndField;
            }
            set
            {
                this.houseNumberEndField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = true)]
        public System.Nullable<int> houseNumberRangeEnd
        {
            get
            {
                return this.houseNumberRangeEndField;
            }
            set
            {
                this.houseNumberRangeEndField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = true)]
        public System.Nullable<int> houseNumberRangeStart
        {
            get
            {
                return this.houseNumberRangeStartField;
            }
            set
            {
                this.houseNumberRangeStartField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = true)]
        public System.Nullable<int> houseNumberStart
        {
            get
            {
                return this.houseNumberStartField;
            }
            set
            {
                this.houseNumberStartField = value;
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
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string inspectionDistrictPrefix
        {
            get
            {
                return this.inspectionDistrictPrefixField;
            }
            set
            {
                this.inspectionDistrictPrefixField = value;
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
        public string mappingDailyAddressNbr
        {
            get
            {
                return this.mappingDailyAddressNbrField;
            }
            set
            {
                this.mappingDailyAddressNbrField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string neighborhoodPrefix
        {
            get
            {
                return this.neighborhoodPrefixField;
            }
            set
            {
                this.neighborhoodPrefixField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string neighborhood
        {
            get
            {
                return this.neighborhoodField;
            }
            set
            {
                this.neighborhoodField = value;
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
        [System.Xml.Serialization.XmlArrayAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        [System.Xml.Serialization.XmlArrayItemAttribute("parcelList", Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = false)]
        public ParcelInfoModel[] parcelLists
        {
            get
            {
                return this.parcelListsField;
            }
            set
            {
                this.parcelListsField = value;
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
        public string primaryFlag
        {
            get
            {
                return this.primaryFlagField;
            }
            set
            {
                this.primaryFlagField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = true)]
        public System.Nullable<long> refAddressId
        {
            get
            {
                return this.refAddressIdField;
            }
            set
            {
                this.refAddressIdField = value;
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
        public string resCountryCode
        {
            get
            {
                return this.resCountryCodeField;
            }
            set
            {
                this.resCountryCodeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string resState
        {
            get
            {
                return this.resStateField;
            }
            set
            {
                this.resStateField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string resStreetDirection
        {
            get
            {
                return this.resStreetDirectionField;
            }
            set
            {
                this.resStreetDirectionField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string resStreetSuffix
        {
            get
            {
                return this.resStreetSuffixField;
            }
            set
            {
                this.resStreetSuffixField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string resStreetSuffixdirection
        {
            get
            {
                return this.resStreetSuffixdirectionField;
            }
            set
            {
                this.resStreetSuffixdirectionField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string resUnitType
        {
            get
            {
                return this.resUnitTypeField;
            }
            set
            {
                this.resUnitTypeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string secondaryRoad
        {
            get
            {
                return this.secondaryRoadField;
            }
            set
            {
                this.secondaryRoadField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = true)]
        public System.Nullable<int> secondaryRoadNumber
        {
            get
            {
                return this.secondaryRoadNumberField;
            }
            set
            {
                this.secondaryRoadNumberField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string sourceFlag
        {
            get
            {
                return this.sourceFlagField;
            }
            set
            {
                this.sourceFlagField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = true)]
        public System.Nullable<int> sourceNumber
        {
            get
            {
                return this.sourceNumberField;
            }
            set
            {
                this.sourceNumberField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string state
        {
            get
            {
                return this.stateField;
            }
            set
            {
                this.stateField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string streetDirection
        {
            get
            {
                return this.streetDirectionField;
            }
            set
            {
                this.streetDirectionField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string streetName
        {
            get
            {
                return this.streetNameField;
            }
            set
            {
                this.streetNameField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string streetPrefix
        {
            get
            {
                return this.streetPrefixField;
            }
            set
            {
                this.streetPrefixField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string streetSuffix
        {
            get
            {
                return this.streetSuffixField;
            }
            set
            {
                this.streetSuffixField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string streetSuffixdirection
        {
            get
            {
                return this.streetSuffixdirectionField;
            }
            set
            {
                this.streetSuffixdirectionField = value;
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
        public string unitEnd
        {
            get
            {
                return this.unitEndField;
            }
            set
            {
                this.unitEndField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string unitRangeEnd
        {
            get
            {
                return this.unitRangeEndField;
            }
            set
            {
                this.unitRangeEndField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string unitRangeStart
        {
            get
            {
                return this.unitRangeStartField;
            }
            set
            {
                this.unitRangeStartField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string unitStart
        {
            get
            {
                return this.unitStartField;
            }
            set
            {
                this.unitStartField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string unitType
        {
            get
            {
                return this.unitTypeField;
            }
            set
            {
                this.unitTypeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = true)]
        public System.Nullable<double> XCoordinator
        {
            get
            {
                return this.xCoordinatorField;
            }
            set
            {
                this.xCoordinatorField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = true)]
        public System.Nullable<double> YCoordinator
        {
            get
            {
                return this.yCoordinatorField;
            }
            set
            {
                this.yCoordinatorField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string zip
        {
            get
            {
                return this.zipField;
            }
            set
            {
                this.zipField = value;
            }
        }
		
		/// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string levelPrefix
        {
            get
            {
                return this.levelPrefixField;
            }
            set
            {
                this.levelPrefixField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string levelNumberStart
        {
            get
            {
                return this.levelNumberStartField;
            }
            set
            {
                this.levelNumberStartField = value;
            }
        }        
		
		/// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string levelNumberEnd
        {
            get
            {
                return this.levelNumberEndField;
            }
            set
            {
                this.levelNumberEndField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string houseNumberAlphaStart
        {
            get
            {
                return this.houseNumberAlphaStartField;
            }
            set
            {
                this.houseNumberAlphaStartField = value;
            }
        }
		
		/// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string houseNumberAlphaEnd
        {
            get
            {
                return this.houseNumberAlphaEndField;
            }
            set
            {
                this.houseNumberAlphaEndField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public System.Nullable<int> houseNumberStartFrom
        {
            get { return this.houseNumberStartFromField; }
            set { this.houseNumberStartFromField = value; }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public System.Nullable<int>  houseNumberStartTo
        {
            get { return this.houseNumberStartToField; }
            set { this.houseNumberStartToField = value; }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public System.Nullable<int>  houseNumberEndFrom
        {
            get { return this.houseNumberEndFromField; }
            set { this.houseNumberEndFromField = value; }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public System.Nullable<int> houseNumberEndTo
        {
            get { return this.houseNumberEndToField; }
            set { this.houseNumberEndToField = value; }
        }
    }    
}
