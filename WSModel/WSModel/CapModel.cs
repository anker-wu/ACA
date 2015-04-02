#region Header
/**
 * <pre>
 * 
 *  Accela Citizen Access
 *  File: CapModel.cs
 * 
 *  Accela, Inc.
 *  Copyright (C): 2010-2011
 * 
 *  Description:
 * 
 *  Notes:
 * $Id: CapModel.cs 181867 2010-10-06 08:06:18Z ACHIEVO\daly.zeng $.
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
    public partial class CapModel : CapPage4ACAModel
    {
        private string activeTasksField;

        private string activeTasksStatusField;

        private AddressModel addressModelField;

        private object[] addtionalItemsField;

        private string altIDField;

        private object[] appAddressModelField;

        private object[] appRefDocumentModelField;

        private object[] appSpecificInfoGroupsField;

        private string appTypeAliasField;

        private CapContactModel applicantModelField;

        private TemplateAttributeModel assetArributeModelField;

        private object[] assetListField;

        private AssetMasterModel assetMasterModelField;

        private System.DateTime auditDateField;

        private bool auditDateFieldSpecified;

        private string auditIDField;

        private string auditStatusField;

        private B1ExpirationModel b1ExpirationModelField;

        private BCalcValuatnModel[] bCalcValuationListField;

        private BStructureModel bStructureModelField;

        private BValuatnModel bValuatnModelField;

        private string basedOnField;

        private string capClassField;

        private CapContactModel capContactModelField;

        private CapDetailModel capDetailModelField;

        private CapIDModel capIDField;

        private RefOwnerModel[] capOwnerListField;

        private OwnerModel capOwnerModelField;

        private ParcelModel capParcelModelField;

        private string capStatusField;

        private System.DateTime capStatusDateField;

        private bool capStatusDateFieldSpecified;

        private CapTypeModel capTypeField;

        private CapWorkDesModel capWorkDesModelField;

        private string checkBoxCommentField;

        private ContinuingEducationModel[] contEducationListField;

        private object[] contactsGroupField;

        private object[] costingListField;

        private CostingModel costingModelField;

        private EducationModel[] educationListField;

        private System.DateTime endFileDateField;

        private bool endFileDateFieldSpecified;

        private System.DateTime endReportedDateField;

        private bool endReportedDateFieldSpecified;

        private StructureEstablishmentModel[] establishmentListField;

        private string eventCodeField;

        private ExaminationModel[] examinationListField;

        private System.DateTime fileDateField;

        private bool fileDateFieldSpecified;

        private TaskItemModel historyTaskItemModelField;

        private string initiatedProductField;

        private string isManualAltIDField;

        private bool isSuperAgencyField;

        private bool isSuperAgencyFieldSpecified;

        private string licSeqNbrField;

        private LicenseProfessionalModel[] licenseProfessionalListField;

        private LicenseProfessionalModel licenseProfessionalModelField;

        private string moduleNameField;

        private OwnerModel[] ownerListField;

        private RefOwnerModel ownerModelField;

        private ParcelModel[] parcelListField;

        private CapParcelModel parcelModelField;

        private CapIDModel parentCapIDField;

        private PartTransactionModel partModelField;

        private string processCodeField;

        private ProjectModel projectModelField;

        private long projectNumberField;

        private string qUD1Field;

        private string qUD2Field;

        private string qUD3Field;

        private string qUD4Field;

        private string receiptNumberField;

        private RefAddressModel[] refAddressListField;

        private RefAddressModel refAddressModelField;

        private string refIDField;

        private string refID1Field;

        private string refID2Field;

        private string refID3Field;

        private System.DateTime reportedDateField;

        private bool reportedDateFieldSpecified;

        private string reportedTimeField;

        private SectionTownShipRangeModel sectionTownShipRangeModelField;

        private string setIDField;

        private string sourceField;

        private object[] specialInfoField;

        private string specialTextField;

        private string standardPCClassField;

        private double standardPCTimeField;

        private string statusGroupCodeField;

        private StructureEstablishmentModel[] structureListField;

        private string superServProvCodeField;

        private SysUserModel sysUserField;

        private TaskItemModel taskItemModelField;

        private TaskSpecificInfoModel[] taskSpecInfosField;

        private long trackingNbrField;

        private bool useManualAltIDField;

        private bool useManualAltIDFieldSpecified;

        private object[] userDefineParcelAttributesField;

        private XAssetTypeCapTypeModel[] xAssetTypeCapTypeListField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string activeTasks
        {
            get
            {
                return this.activeTasksField;
            }
            set
            {
                this.activeTasksField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string activeTasksStatus
        {
            get
            {
                return this.activeTasksStatusField;
            }
            set
            {
                this.activeTasksStatusField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public AddressModel addressModel
        {
            get
            {
                return this.addressModelField;
            }
            set
            {
                this.addressModelField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("addtionalItems", Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = true)]
        public object[] addtionalItems
        {
            get
            {
                return this.addtionalItemsField;
            }
            set
            {
                this.addtionalItemsField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string altID
        {
            get
            {
                return this.altIDField;
            }
            set
            {
                this.altIDField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("appAddressModel", Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = true)]
        public object[] appAddressModel
        {
            get
            {
                return this.appAddressModelField;
            }
            set
            {
                this.appAddressModelField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("appRefDocumentModel", Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = true)]
        public object[] appRefDocumentModel
        {
            get
            {
                return this.appRefDocumentModelField;
            }
            set
            {
                this.appRefDocumentModelField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("appSpecificInfoGroups", Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = true)]
        public object[] appSpecificInfoGroups
        {
            get
            {
                return this.appSpecificInfoGroupsField;
            }
            set
            {
                this.appSpecificInfoGroupsField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string appTypeAlias
        {
            get
            {
                return this.appTypeAliasField;
            }
            set
            {
                this.appTypeAliasField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public CapContactModel applicantModel
        {
            get
            {
                return this.applicantModelField;
            }
            set
            {
                this.applicantModelField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public TemplateAttributeModel assetArributeModel
        {
            get
            {
                return this.assetArributeModelField;
            }
            set
            {
                this.assetArributeModelField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("assetList", Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = true)]
        public object[] assetList
        {
            get
            {
                return this.assetListField;
            }
            set
            {
                this.assetListField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public AssetMasterModel assetMasterModel
        {
            get
            {
                return this.assetMasterModelField;
            }
            set
            {
                this.assetMasterModelField = value;
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
        public B1ExpirationModel b1ExpirationModel
        {
            get
            {
                return this.b1ExpirationModelField;
            }
            set
            {
                this.b1ExpirationModelField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("BCalcValuationList", Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = true)]
        public BCalcValuatnModel[] BCalcValuationList
        {
            get
            {
                return this.bCalcValuationListField;
            }
            set
            {
                this.bCalcValuationListField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public BStructureModel BStructureModel
        {
            get
            {
                return this.bStructureModelField;
            }
            set
            {
                this.bStructureModelField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public BValuatnModel BValuatnModel
        {
            get
            {
                return this.bValuatnModelField;
            }
            set
            {
                this.bValuatnModelField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string basedOn
        {
            get
            {
                return this.basedOnField;
            }
            set
            {
                this.basedOnField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string capClass
        {
            get
            {
                return this.capClassField;
            }
            set
            {
                this.capClassField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public CapContactModel capContactModel
        {
            get
            {
                return this.capContactModelField;
            }
            set
            {
                this.capContactModelField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public CapDetailModel capDetailModel
        {
            get
            {
                return this.capDetailModelField;
            }
            set
            {
                this.capDetailModelField = value;
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
        [System.Xml.Serialization.XmlElementAttribute("capOwnerList", Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = true)]
        public RefOwnerModel[] capOwnerList
        {
            get
            {
                return this.capOwnerListField;
            }
            set
            {
                this.capOwnerListField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public OwnerModel capOwnerModel
        {
            get
            {
                return this.capOwnerModelField;
            }
            set
            {
                this.capOwnerModelField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public ParcelModel capParcelModel
        {
            get
            {
                return this.capParcelModelField;
            }
            set
            {
                this.capParcelModelField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string capStatus
        {
            get
            {
                return this.capStatusField;
            }
            set
            {
                this.capStatusField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public System.DateTime capStatusDate
        {
            get
            {
                return this.capStatusDateField;
            }
            set
            {
                this.capStatusDateField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool capStatusDateSpecified
        {
            get
            {
                return this.capStatusDateFieldSpecified;
            }
            set
            {
                this.capStatusDateFieldSpecified = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public CapTypeModel capType
        {
            get
            {
                return this.capTypeField;
            }
            set
            {
                this.capTypeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public CapWorkDesModel capWorkDesModel
        {
            get
            {
                return this.capWorkDesModelField;
            }
            set
            {
                this.capWorkDesModelField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string checkBoxComment
        {
            get
            {
                return this.checkBoxCommentField;
            }
            set
            {
                this.checkBoxCommentField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("contEducationList", Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = true)]
        public ContinuingEducationModel[] contEducationList
        {
            get
            {
                return this.contEducationListField;
            }
            set
            {
                this.contEducationListField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("contactsGroup", Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = true)]
        public object[] contactsGroup
        {
            get
            {
                return this.contactsGroupField;
            }
            set
            {
                this.contactsGroupField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("costingList", Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = true)]
        public object[] costingList
        {
            get
            {
                return this.costingListField;
            }
            set
            {
                this.costingListField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public CostingModel costingModel
        {
            get
            {
                return this.costingModelField;
            }
            set
            {
                this.costingModelField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("educationList", Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = true)]
        public EducationModel[] educationList
        {
            get
            {
                return this.educationListField;
            }
            set
            {
                this.educationListField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public System.DateTime endFileDate
        {
            get
            {
                return this.endFileDateField;
            }
            set
            {
                this.endFileDateField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool endFileDateSpecified
        {
            get
            {
                return this.endFileDateFieldSpecified;
            }
            set
            {
                this.endFileDateFieldSpecified = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public System.DateTime endReportedDate
        {
            get
            {
                return this.endReportedDateField;
            }
            set
            {
                this.endReportedDateField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool endReportedDateSpecified
        {
            get
            {
                return this.endReportedDateFieldSpecified;
            }
            set
            {
                this.endReportedDateFieldSpecified = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("establishmentList", Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = true)]
        public StructureEstablishmentModel[] establishmentList
        {
            get
            {
                return this.establishmentListField;
            }
            set
            {
                this.establishmentListField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string eventCode
        {
            get
            {
                return this.eventCodeField;
            }
            set
            {
                this.eventCodeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("examinationList", Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = true)]
        public ExaminationModel[] examinationList
        {
            get
            {
                return this.examinationListField;
            }
            set
            {
                this.examinationListField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public System.DateTime fileDate
        {
            get
            {
                return this.fileDateField;
            }
            set
            {
                this.fileDateField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool fileDateSpecified
        {
            get
            {
                return this.fileDateFieldSpecified;
            }
            set
            {
                this.fileDateFieldSpecified = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public TaskItemModel historyTaskItemModel
        {
            get
            {
                return this.historyTaskItemModelField;
            }
            set
            {
                this.historyTaskItemModelField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string initiatedProduct
        {
            get
            {
                return this.initiatedProductField;
            }
            set
            {
                this.initiatedProductField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string isManualAltID
        {
            get
            {
                return this.isManualAltIDField;
            }
            set
            {
                this.isManualAltIDField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public bool isSuperAgency
        {
            get
            {
                return this.isSuperAgencyField;
            }
            set
            {
                this.isSuperAgencyField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool isSuperAgencySpecified
        {
            get
            {
                return this.isSuperAgencyFieldSpecified;
            }
            set
            {
                this.isSuperAgencyFieldSpecified = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string licSeqNbr
        {
            get
            {
                return this.licSeqNbrField;
            }
            set
            {
                this.licSeqNbrField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("licenseProfessionalList", Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = true)]
        public LicenseProfessionalModel[] licenseProfessionalList
        {
            get
            {
                return this.licenseProfessionalListField;
            }
            set
            {
                this.licenseProfessionalListField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public LicenseProfessionalModel licenseProfessionalModel
        {
            get
            {
                return this.licenseProfessionalModelField;
            }
            set
            {
                this.licenseProfessionalModelField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string moduleName
        {
            get
            {
                return this.moduleNameField;
            }
            set
            {
                this.moduleNameField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("ownerList", Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = true)]
        public OwnerModel[] ownerList
        {
            get
            {
                return this.ownerListField;
            }
            set
            {
                this.ownerListField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public RefOwnerModel ownerModel
        {
            get
            {
                return this.ownerModelField;
            }
            set
            {
                this.ownerModelField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("parcelList", Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = true)]
        public ParcelModel[] parcelList
        {
            get
            {
                return this.parcelListField;
            }
            set
            {
                this.parcelListField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public CapParcelModel parcelModel
        {
            get
            {
                return this.parcelModelField;
            }
            set
            {
                this.parcelModelField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public CapIDModel parentCapID
        {
            get
            {
                return this.parentCapIDField;
            }
            set
            {
                this.parentCapIDField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public PartTransactionModel partModel
        {
            get
            {
                return this.partModelField;
            }
            set
            {
                this.partModelField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string processCode
        {
            get
            {
                return this.processCodeField;
            }
            set
            {
                this.processCodeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public ProjectModel projectModel
        {
            get
            {
                return this.projectModelField;
            }
            set
            {
                this.projectModelField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public long projectNumber
        {
            get
            {
                return this.projectNumberField;
            }
            set
            {
                this.projectNumberField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string QUD1
        {
            get
            {
                return this.qUD1Field;
            }
            set
            {
                this.qUD1Field = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string QUD2
        {
            get
            {
                return this.qUD2Field;
            }
            set
            {
                this.qUD2Field = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string QUD3
        {
            get
            {
                return this.qUD3Field;
            }
            set
            {
                this.qUD3Field = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string QUD4
        {
            get
            {
                return this.qUD4Field;
            }
            set
            {
                this.qUD4Field = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string receiptNumber
        {
            get
            {
                return this.receiptNumberField;
            }
            set
            {
                this.receiptNumberField = value;
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
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string refID
        {
            get
            {
                return this.refIDField;
            }
            set
            {
                this.refIDField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string refID1
        {
            get
            {
                return this.refID1Field;
            }
            set
            {
                this.refID1Field = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string refID2
        {
            get
            {
                return this.refID2Field;
            }
            set
            {
                this.refID2Field = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string refID3
        {
            get
            {
                return this.refID3Field;
            }
            set
            {
                this.refID3Field = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public System.DateTime reportedDate
        {
            get
            {
                return this.reportedDateField;
            }
            set
            {
                this.reportedDateField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool reportedDateSpecified
        {
            get
            {
                return this.reportedDateFieldSpecified;
            }
            set
            {
                this.reportedDateFieldSpecified = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string reportedTime
        {
            get
            {
                return this.reportedTimeField;
            }
            set
            {
                this.reportedTimeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public SectionTownShipRangeModel sectionTownShipRangeModel
        {
            get
            {
                return this.sectionTownShipRangeModelField;
            }
            set
            {
                this.sectionTownShipRangeModelField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string setID
        {
            get
            {
                return this.setIDField;
            }
            set
            {
                this.setIDField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string source
        {
            get
            {
                return this.sourceField;
            }
            set
            {
                this.sourceField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("specialInfo", Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = true)]
        public object[] specialInfo
        {
            get
            {
                return this.specialInfoField;
            }
            set
            {
                this.specialInfoField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string specialText
        {
            get
            {
                return this.specialTextField;
            }
            set
            {
                this.specialTextField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string standardPCClass
        {
            get
            {
                return this.standardPCClassField;
            }
            set
            {
                this.standardPCClassField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public double standardPCTime
        {
            get
            {
                return this.standardPCTimeField;
            }
            set
            {
                this.standardPCTimeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string statusGroupCode
        {
            get
            {
                return this.statusGroupCodeField;
            }
            set
            {
                this.statusGroupCodeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("structureList", Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = true)]
        public StructureEstablishmentModel[] structureList
        {
            get
            {
                return this.structureListField;
            }
            set
            {
                this.structureListField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string superServProvCode
        {
            get
            {
                return this.superServProvCodeField;
            }
            set
            {
                this.superServProvCodeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public SysUserModel sysUser
        {
            get
            {
                return this.sysUserField;
            }
            set
            {
                this.sysUserField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public TaskItemModel taskItemModel
        {
            get
            {
                return this.taskItemModelField;
            }
            set
            {
                this.taskItemModelField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("taskSpecInfos", Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = true)]
        public TaskSpecificInfoModel[] taskSpecInfos
        {
            get
            {
                return this.taskSpecInfosField;
            }
            set
            {
                this.taskSpecInfosField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public long trackingNbr
        {
            get
            {
                return this.trackingNbrField;
            }
            set
            {
                this.trackingNbrField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public bool useManualAltID
        {
            get
            {
                return this.useManualAltIDField;
            }
            set
            {
                this.useManualAltIDField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool useManualAltIDSpecified
        {
            get
            {
                return this.useManualAltIDFieldSpecified;
            }
            set
            {
                this.useManualAltIDFieldSpecified = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("userDefineParcelAttributes", Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = true)]
        public object[] userDefineParcelAttributes
        {
            get
            {
                return this.userDefineParcelAttributesField;
            }
            set
            {
                this.userDefineParcelAttributesField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("XAssetTypeCapTypeList", Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = true)]
        public XAssetTypeCapTypeModel[] XAssetTypeCapTypeList
        {
            get
            {
                return this.xAssetTypeCapTypeListField;
            }
            set
            {
                this.xAssetTypeCapTypeListField = value;
            }
        }
    }
}
