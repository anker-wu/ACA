#region Header
/**
 * <pre>
 * 
 *  Accela Citizen Access
 *  File: GGuideSheetItemModel.cs
 * 
 *  Accela, Inc.
 *  Copyright (C): 2010
 * 
 *  Description:
 * 
 *  Notes:
 * $Id: GGuideSheetItemModel.cs 181867 2010-10-06 08:06:18Z ACHIEVO\daly.zeng $.
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
    public partial class GGuideSheetItemModel : LanguageModel
    {

        private ActivityModel activityModelField;

        private System.Nullable<System.DateTime> auditDateField;

        private string auditIDField;

        private string auditStatusField;

        private string guideItemASIGroupNameField;

        private string guideItemASIGroupVisibleField;

        private string guideItemASITableGroupNameField;

        private string guideItemCarryCheckField;

        private string guideItemCommentField;

        private string guideItemCommentVisibleField;

        private System.Nullable<int> guideItemDisplayOrderField;

        private System.Nullable<int> guideItemScoreField;

        private string guideItemScoreVisibleField;

        private System.Nullable<long> guideItemSeqNbrField;

        private string guideItemStatusField;

        private string guideItemStatusGroupNameField;

        private string guideItemStatusVisibleField;

        private string guideItemTextField;

        private string guideItemTextVisibleField;

        private string guideSheetIdField;

        private string guideTypeField;

        private System.Nullable<long> guidesheetSeqNbrField;

        private GGSItemASISubGroupModel[] itemASISubgroupListField;

        private GGSItemASITableSubGroupModel[] itemASITableSubgroupListField;

        private string lhsTypeField;

        private string majorViolationField;

        private string majorViolationVisibleField;

        private System.Nullable<double> maxPointsField;

        private string maxPointsVisibleField;

        private string resGuideItemCommentField;

        private string resGuideItemTextField;

        private string serviceProviderCodeField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public ActivityModel activityModel
        {
            get
            {
                return this.activityModelField;
            }
            set
            {
                this.activityModelField = value;
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
        public string guideItemASIGroupName
        {
            get
            {
                return this.guideItemASIGroupNameField;
            }
            set
            {
                this.guideItemASIGroupNameField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string guideItemASIGroupVisible
        {
            get
            {
                return this.guideItemASIGroupVisibleField;
            }
            set
            {
                this.guideItemASIGroupVisibleField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string guideItemASITableGroupName
        {
            get
            {
                return this.guideItemASITableGroupNameField;
            }
            set
            {
                this.guideItemASITableGroupNameField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string guideItemCarryCheck
        {
            get
            {
                return this.guideItemCarryCheckField;
            }
            set
            {
                this.guideItemCarryCheckField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string guideItemComment
        {
            get
            {
                return this.guideItemCommentField;
            }
            set
            {
                this.guideItemCommentField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string guideItemCommentVisible
        {
            get
            {
                return this.guideItemCommentVisibleField;
            }
            set
            {
                this.guideItemCommentVisibleField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = true)]
        public System.Nullable<int> guideItemDisplayOrder
        {
            get
            {
                return this.guideItemDisplayOrderField;
            }
            set
            {
                this.guideItemDisplayOrderField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = true)]
        public System.Nullable<int> guideItemScore
        {
            get
            {
                return this.guideItemScoreField;
            }
            set
            {
                this.guideItemScoreField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string guideItemScoreVisible
        {
            get
            {
                return this.guideItemScoreVisibleField;
            }
            set
            {
                this.guideItemScoreVisibleField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = true)]
        public System.Nullable<long> guideItemSeqNbr
        {
            get
            {
                return this.guideItemSeqNbrField;
            }
            set
            {
                this.guideItemSeqNbrField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string guideItemStatus
        {
            get
            {
                return this.guideItemStatusField;
            }
            set
            {
                this.guideItemStatusField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string guideItemStatusGroupName
        {
            get
            {
                return this.guideItemStatusGroupNameField;
            }
            set
            {
                this.guideItemStatusGroupNameField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string guideItemStatusVisible
        {
            get
            {
                return this.guideItemStatusVisibleField;
            }
            set
            {
                this.guideItemStatusVisibleField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string guideItemText
        {
            get
            {
                return this.guideItemTextField;
            }
            set
            {
                this.guideItemTextField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string guideItemTextVisible
        {
            get
            {
                return this.guideItemTextVisibleField;
            }
            set
            {
                this.guideItemTextVisibleField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string guideSheetId
        {
            get
            {
                return this.guideSheetIdField;
            }
            set
            {
                this.guideSheetIdField = value;
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
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = true)]
        public System.Nullable<long> guidesheetSeqNbr
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
        [System.Xml.Serialization.XmlElementAttribute("itemASISubgroupList", Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = true)]
        public GGSItemASISubGroupModel[] itemASISubgroupList
        {
            get
            {
                return this.itemASISubgroupListField;
            }
            set
            {
                this.itemASISubgroupListField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("itemASITableSubgroupList", Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = true)]
        public GGSItemASITableSubGroupModel[] itemASITableSubgroupList
        {
            get
            {
                return this.itemASITableSubgroupListField;
            }
            set
            {
                this.itemASITableSubgroupListField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string lhsType
        {
            get
            {
                return this.lhsTypeField;
            }
            set
            {
                this.lhsTypeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string majorViolation
        {
            get
            {
                return this.majorViolationField;
            }
            set
            {
                this.majorViolationField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string majorViolationVisible
        {
            get
            {
                return this.majorViolationVisibleField;
            }
            set
            {
                this.majorViolationVisibleField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = true)]
        public System.Nullable<double> maxPoints
        {
            get
            {
                return this.maxPointsField;
            }
            set
            {
                this.maxPointsField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string maxPointsVisible
        {
            get
            {
                return this.maxPointsVisibleField;
            }
            set
            {
                this.maxPointsVisibleField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string resGuideItemComment
        {
            get
            {
                return this.resGuideItemCommentField;
            }
            set
            {
                this.resGuideItemCommentField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string resGuideItemText
        {
            get
            {
                return this.resGuideItemTextField;
            }
            set
            {
                this.resGuideItemTextField = value;
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