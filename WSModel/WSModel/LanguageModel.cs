/**
 * <pre>
 * 
 *  Accela Citizen Access
 *  File: LanguageModel.cs
 * 
 *  Accela, Inc.
 *  Copyright (C): 2010-2014
 * 
 *  Description:
 * 
 *  Notes:
 * $Id: LanguageModel.cs 171698 2010-04-29 09:39:39Z ACHIEVO\daly.zeng $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

using Accela.ACA.WSProxy.WSModel;

namespace Accela.ACA.WSProxy
{
    /// <remarks/>
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(InspectionTypeModel))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(RefAppSpecInfoDropDownModel))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(GGuideSheetItemModel))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(GGuideSheetModel))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(TimeGroupModel))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(TimeTypeModel))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(TimeLogModel))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(CommentModel))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(PartTransactionModel))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(StandardCommentModel))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(TaskItemModel))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(StrucEstaMasterModel))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(StructureEstablishmentModel))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(CostingModel))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(BStructureModel))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(ParcelModel))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(OwnerModel))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(ConditionModel))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(NoticeConditionModel))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(AssetOrderModel))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(AssetMasterModel))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(TemplateAttributeModel))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(CapPage4ACAModel))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(CapModel))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(CalendarInspectionTypeModel))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(PersonModel))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(PeopleModel))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(AssetContactModel))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(SysUserModel))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(AuditModel))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(CapTypeIconModel))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(CapTypeModel))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(ActivityModel))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(InspectionModel))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(AppSpecificInfoModel))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(XDataFilterModel))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(XDataFilterElementModel))]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.3053")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://model.webservice.accela.com")]
    public partial class LanguageModel
    {
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = true)]
        public System.Nullable<long> resId
        {
            get;
            set;
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string resLangId
        {
            get;
            set;
        }
    }
}
