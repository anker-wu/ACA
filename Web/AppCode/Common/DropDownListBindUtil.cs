#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: DropDownListBindUtil.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2007-2014
 *
 *  Description:
 *  Provide interface to bind list data to dropdown list control.
 *
 *  Notes:
 *      $Id: DropDownListBindUtil.cs 278415 2014-09-03 08:45:04Z ACHIEVO\james.shi $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI.WebControls;
using Accela.ACA.BLL.AddressBuilder;
using Accela.ACA.BLL.APO;
using Accela.ACA.BLL.Asset;
using Accela.ACA.BLL.Attachment;
using Accela.ACA.BLL.Cap;
using Accela.ACA.BLL.Common;
using Accela.ACA.BLL.LicenseCertification;
using Accela.ACA.BLL.People;
using Accela.ACA.Common;
using Accela.ACA.Common.Util;
using Accela.ACA.WSProxy;
using Accela.Web.Controls;

namespace Accela.ACA.Web.Common
{
    /// <summary>
    /// Provide interface to bind list data to dropdown list control. 
    /// </summary>
    public static class DropDownListBindUtil
    {
        #region Fields

        /// <summary>
        /// MAX LENGTH NO LIMITED
        /// </summary>
        private const int MAX_LENGTH_NO_LIMITED = 0;

        /// <summary>
        /// none applicable.
        /// </summary>
        private const string NONE_APPLICABLE = "NONE_APPLICABLE";

        /// <summary>
        /// The asset class type point
        /// </summary>
        private const string ASSET_CLASS_TYPE_POINT = "Point";

        /// <summary>
        /// The asset class type linear
        /// </summary>
        private const string ASSET_CLASS_TYPE_LINEAR = "Linear";

        /// <summary>
        /// The asset class type node link linear
        /// </summary>
        private const string ASSET_CLASS_TYPE_NODE_LINK_LINEAR = "Node-Link Linear";

        /// <summary>
        /// The asset class type polygon
        /// </summary>
        private const string ASSET_CLASS_TYPE_POLYGON = "Polygon";

        /// <summary>
        /// The asset class type component
        /// </summary>
        private const string ASSET_CLASS_TYPE_COMPONENT = "Component";

        #endregion Fields

        #region Properties

        /// <summary>
        /// Gets the default ListItem for dropdownlist, which often is placed on the first position.
        /// </summary>
        public static ListItem DefaultListItem
        {
            get
            {
                ListItem _defaultItem = new ListItem();
                _defaultItem.Text = WebConstant.DropDownDefaultText;
                _defaultItem.Value = string.Empty;

                return _defaultItem;
            }
        }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Binds Board Types with the specified DropdownList instance
        /// </summary>
        /// <param name="ddlReason">The DDL reason.</param>
        public static void BindReprintReason(AccelaDropDownList ddlReason)
        {
            IBizDomainBll standChoiceBll = ObjectFactory.GetObject(typeof(IBizDomainBll)) as IBizDomainBll;
            IList<BizDomainModel4WS> stdItems = standChoiceBll.GetBizDomainValue(
                ConfigManager.AgencyCode,
                BizDomainConstant.STD_REPRINT_REASONS,
                new QueryFormat4WS(),
                false,
                I18nCultureUtil.UserPreferredCulture);

            ddlReason.Items.Clear();
            ddlReason.Items.Add(DefaultListItem);

            if (stdItems != null && stdItems.Count > 0)
            {
                foreach (var item in stdItems)
                {
                    ddlReason.Items.Add(new ListItem(I18nStringUtil.GetString(item.resBizdomainValue, item.bizdomainValue), item.dispositionID.ToString()));
                }
            }
        }

        /// <summary>
        /// Binds Board Types with the specified DropdownList instance
        /// </summary>
        /// <param name="ddlBoardTypes">The DropdownList to bind</param>
        /// <param name="moduleName">The module which the user is accessing</param>
        /// <param name="filterName">The CAP type filter name</param>
        /// <param name="vchType">The VCHType</param>
        /// <param name="userID">The user ID</param>
        /// <param name="containAsChildOnly">whether need as child only cap type</param>
        public static void BindBoardTypes(AccelaDropDownList ddlBoardTypes, string moduleName, string filterName, string vchType, string userID, bool containAsChildOnly)
        {
            ddlBoardTypes.Items.Clear();

            List<ListItem> boardTypeListItems = new List<ListItem>();
            if (ddlBoardTypes != null)
            {
                ICapTypeBll capTypeBll = ObjectFactory.GetObject(typeof(ICapTypeBll)) as ICapTypeBll;
                List<CapTypeModel> boardTypes = capTypeBll.GetBoardTypes(moduleName, filterName, vchType, userID, containAsChildOnly);

                foreach (CapTypeModel boardType in boardTypes)
                {
                    ListItem item = new ListItem(boardType.dispType, boardType.type);
                    boardTypeListItems.Add(item);
                }
            }

            BindDDL(boardTypeListItems, ddlBoardTypes, false);
            ListItem defaultItem = new ListItem(LabelUtil.GetGlobalTextByKey("aca_cap_type_board_type_default_select"), string.Empty);
            ddlBoardTypes.Items.Insert(0, defaultItem);

            // if has only one item, make it selected.
            if (boardTypeListItems.Count == 1)
            {
                ddlBoardTypes.SelectedValue = boardTypeListItems[0].Value;
            }
        }

        /// <summary>
        /// Bind the specific standard choice items to construct type AccelaDropDownList control with value-text format by standard choice category name.
        /// </summary>
        /// <param name="ddlControl">AccelaDropDownList control.</param>
        public static void BindConstructType(AccelaDropDownList ddlControl)
        {
            IBizDomainBll standChoiceBll = ObjectFactory.GetObject(typeof(IBizDomainBll)) as IBizDomainBll;
            IList<ItemValue> stdItems = standChoiceBll.GetBizDomainList(ConfigManager.AgencyCode, BizDomainConstant.STD_CAT_CONSTUCTION_TYPE, false, 2);

            ddlControl.Items.Clear();
            ddlControl.Items.Add(DefaultListItem);

            if (stdItems != null &&
                stdItems.Count > 0)
            {
                foreach (ItemValue item in stdItems)
                {
                    ddlControl.Items.Add(new ListItem(item.Value.ToString(), item.Key));
                }
            }

            ddlControl.SourceType = DropDownListDataSourceType.StandardChoice;
            ddlControl.StdCategory = BizDomainConstant.STD_CAT_CONSTUCTION_TYPE;
            ddlControl.ShowType = DropDownListShowType.ShowBoth;
            ddlControl.MaxValueLength = StandardChoiceMaxLength.MAX_LENGTH_CONSTRUCTION_TYPE;
        }

        /// <summary>
        /// Bind the Preferred Channel to AccelaDropDownList control.
        /// </summary>
        /// <param name="ddlPreferredChannel">AccelaDropDownList control to be bind.</param>
        /// <param name="moduleName">Module name to get the labels.</param>
        public static void BindPreferredChannel(AccelaDropDownList ddlPreferredChannel, string moduleName)
        {
            IBizDomainBll standChoiceBll = ObjectFactory.GetObject(typeof(IBizDomainBll)) as IBizDomainBll;
            IList<ItemValue> stdItems = standChoiceBll.GetBizDomainList(ConfigManager.AgencyCode, BizDomainConstant.STD_CONTACT_PREFERRED_CHANNEL, false);
            
            List<ListItem> listItems = new List<ListItem>();

            if (stdItems != null && stdItems.Count > 0)
            {
                foreach (ItemValue item in stdItems)
                {
                    listItems.Add(new ListItem(item.Value.ToString(), item.Key));
                }
            }

            BindDDL(listItems, ddlPreferredChannel);
        }

        /// <summary>
        /// Get ListItems by peopleModel array.
        /// </summary>
        /// <param name="peopleModelList">peopleModel List</param>
        /// <returns>ListItem array</returns>
        public static IList<ListItem> ConvertPeopleToListItem(PeopleModel4WS[] peopleModelList)
        {
            IList<ListItem> items = new List<ListItem>();
            if (peopleModelList != null)
            {
                foreach (var people in peopleModelList)
                {
                    string contactName = string.Empty;

                    if (ContactType4License.Organization.ToString().Equals(people.contactTypeFlag, StringComparison.InvariantCultureIgnoreCase))
                    {
                        contactName = people.businessName;
                    }
                    else
                    {
                        if (string.IsNullOrEmpty(people.firstName) && string.IsNullOrEmpty(people.lastName))
                        {
                            contactName = people.fullName;
                        }
                        else
                        {
                            contactName = people.firstName + "  " + people.lastName;
                        }
                    }

                    if (string.IsNullOrEmpty(contactName))
                    {
                        continue;
                    }

                    ListItem item = new ListItem();
                    item.Text = contactName;
                    item.Value = string.Format("Contact|{0}|{1}", people.contactSeqNumber, people.contactType);
                    items.Add(item);
                }
            }

            return items;
        }

        /// <summary>
        /// Bind the contact type item to AccelaDropDownList control.
        /// </summary>
        /// <param name="ddlContactType">AccelaDropDownList control to be bind.</param>
        /// <param name="moduleName">module Name.</param>
        /// <param name="isMultipleContact">is multiple contact or not.</param>
        /// <param name="componentName">The component name.</param>
        public static void BindContactTypeWithPageFlow(AccelaDropDownList ddlContactType, string moduleName, bool isMultipleContact, string componentName)
        {
            if (ddlContactType == null || string.IsNullOrEmpty(moduleName))
            {
                return;
            }

            if (AppSession.IsAdmin)
            {
                BindContactType(ddlContactType, moduleName, ContactTypeSource.Transaction);
            }
            else if (isMultipleContact)
            {
                PageFlowGroupModel pageFlow = AppSession.GetPageflowGroupFromSession();
                string agencyCode = pageFlow != null ? pageFlow.serviceProviderCode : ConfigManager.AgencyCode;
                string pageFlowGroupName = pageFlow != null ? pageFlow.pageFlowGrpCode : null;

                XEntityPermissionModel xentity = new XEntityPermissionModel();
                xentity.servProvCode = agencyCode;
                xentity.entityType = XEntityPermissionConstant.CONTACT_DATA_VALIDATION;
                xentity.entityId = moduleName;
                xentity.entityId2 = pageFlowGroupName;
                xentity.componentName = componentName;

                List<ContactTypeUIModel> contactTypeUIList = GetContactTypesByXEntity(xentity, true);
                List<ListItem> contactlist = new List<ListItem>();

                if (contactTypeUIList != null && contactTypeUIList.Count() > 0)
                {
                    foreach (ContactTypeUIModel contactTypeUI in contactTypeUIList)
                    {
                        contactlist.Add(new ListItem(contactTypeUI.Text, contactTypeUI.Key));
                    }
                }

                BindDDL(contactlist, ddlContactType);
            }
            else
            {
                //In single contact section, bind all contact type to ContactType dropdownlist.
                BindContactTypesInSTD(ddlContactType);
            }
        }

        /// <summary>
        /// Verify drop down list is only one item.
        /// </summary>
        /// <param name="dropDownList">accela drop down list control</param>
        /// <returns>is only one item</returns>
        public static bool IsExistOnlyOneItem(AccelaDropDownList dropDownList)
        {
            if (dropDownList == null || (dropDownList.Items.Count != 1 && dropDownList.Items.Count != 2))
            {
                return false;
            }

            bool existOnlyOneItem = false;
            bool existDefaultItem = false;

            foreach (ListItem item in dropDownList.Items)
            {
                if (DefaultListItem.Value.Equals(item.Value, StringComparison.InvariantCultureIgnoreCase))
                {
                    existDefaultItem = true;
                    break;
                }
            }

            if ((!existDefaultItem && dropDownList.Items.Count == 1)
                || (existDefaultItem && dropDownList.Items.Count == 2))
            {
                existOnlyOneItem = true;
            }

            return existOnlyOneItem;
        }

        /// <summary>
        /// Get Contact type by agency and XEntity.
        /// </summary>
        /// <param name="xentity">the XEntity model</param>
        /// <param name="isSelected">true or false</param>
        /// <returns>contact type UI model list.</returns>
        public static List<ContactTypeUIModel> GetContactTypesByXEntity(XEntityPermissionModel xentity, bool isSelected)
        {
            var xEntityPermissionBll = ObjectFactory.GetObject<IXEntityPermissionBll>();
            IXPolicyBll xpolicyBll = ObjectFactory.GetObject<IXPolicyBll>();

            var xEntities = xEntityPermissionBll.GetXEntityPermissions(xentity);
            List<XPolicyModel> xpolicyList = null;

            //When standard choice "ENABLE_CONTACT_TYPE_FILTERING_BY_MODULE" is "NO", PageFlow level contact types should ignore module level contact type settings.
            if (StandardChoiceUtil.IsEnableContactTypeFilteringByModule())
            {
                xpolicyList = xpolicyBll.GetPolicyListByPolicyName(XPolicyConstant.CONTACT_TYPE_RESTRICTION_BY_MODULE, xentity.servProvCode);
            }

            if (xpolicyList != null && xpolicyList.Count != 0)
            {
                xpolicyList = xpolicyList.Where(
                       p => ACAConstant.LEVEL_TYPE_MODULE.Equals(p.level, StringComparison.OrdinalIgnoreCase)
                    && p.levelData == xentity.entityId 
                    && p.data2.Equals(ACAConstant.RECORD_CONTACT_TYPE)).ToList();
            }

            IBizDomainBll standChoiceBll = ObjectFactory.GetObject<IBizDomainBll>();
            IList<ItemValue> stdItems = standChoiceBll.GetContactTypeList(xentity.servProvCode, false, ContactTypeSource.Transaction);

            if (stdItems == null || stdItems.Count == 0)
            {
                return new List<ContactTypeUIModel>();
            }

            List<ContactTypeUIModel> contactTypeList = new List<ContactTypeUIModel>();

            // get the entities that has permission
            foreach (ItemValue itemValue in stdItems.Where(itemValue => itemValue != null))
            {
                if (xpolicyList != null && xpolicyList.Count > 0)
                {
                    // module level uncheck the contact type.
                    IEnumerable<XPolicyModel> moduleLevelContactTypeEnu = xpolicyList.Where(x => x.data1 == itemValue.Key && ValidationUtil.IsNo(x.rightGranted));

                    if (moduleLevelContactTypeEnu.Any())
                    {
                        continue;
                    }
                }

                contactTypeList.Add(GetPageFlowLevelContactTypeUIModel(xEntities, itemValue));
            }

            return isSelected ? contactTypeList.Where(c => c.Checked).ToList() : contactTypeList;
        }

        /// <summary>
        /// Binds the contact type for lookup.
        /// </summary>
        /// <param name="ddlContactType">Type of the DDL contact.</param>
        public static void BindContactTypeForLookup(AccelaDropDownList ddlContactType)
        {
            XEntityPermissionModel xEntity = new XEntityPermissionModel();
            xEntity.servProvCode = ConfigManager.AgencyCode;
            xEntity.entityType = XEntityPermissionConstant.REFERENCE_CONTACT_SEARCH;

            var xEntityPermissionBll = ObjectFactory.GetObject<IXEntityPermissionBll>();
            IEnumerable<XEntityPermissionModel> xEntities = xEntityPermissionBll.GetXEntityPermissions(xEntity);

            IBizDomainBll standChoiceBll = ObjectFactory.GetObject<IBizDomainBll>();
            IList<ItemValue> stdItems = standChoiceBll.GetContactTypeList(ConfigManager.AgencyCode, false, ContactTypeSource.Reference);
            List<ListItem> contactTypes = new List<ListItem>();

            foreach (var itemValue in stdItems)
            {
                if (itemValue == null)
                {
                    continue;
                }

                if (xEntities != null && !xEntities.Any(o => o.entityId3.Equals(itemValue.Key) && ValidationUtil.IsNo(o.permissionValue)))
                {
                    contactTypes.Add(new ListItem(itemValue.Value.ToString(), itemValue.Key));
                }
            }

            BindDDL(contactTypes, ddlContactType);
        }

        /// <summary>
        /// Bind the contact type item to AccelaDropDownList control.
        /// </summary>
        /// <param name="ddlContactType">AccelaDropDownList control to be bind.</param>
        /// <param name="moduleName">module Name.</param>
        /// <param name="contactTypeSource">the contact type source.</param>
        public static void BindContactType(AccelaDropDownList ddlContactType, string moduleName, string contactTypeSource)
        {
            if (ddlContactType == null || moduleName == null)
            {
                return;
            }

            //1. Get contact type.
            List<ListItem> contactlist = GetContactTypeItems(moduleName, contactTypeSource);

            // 2. bind the contact types to dropdown list.
            ddlContactType.SourceType = ddlContactType.SourceType;
            ddlContactType.StdCategory = BizDomainConstant.STD_CAT_CONTACT_TYPE;
            ddlContactType.MaxValueLength = StandardChoiceMaxLength.MAX_LENGTH_CONTACT_TYPE;

            BindDDL(contactlist, ddlContactType);
        }

        /// <summary>
        /// Binds the type of the reference contact.
        /// </summary>
        /// <param name="ddlContactType">Type of the DDL contact.</param>
        public static void BindReferenceContactTypeFilterBySearchSetting(AccelaDropDownList ddlContactType)
        {
            IBizDomainBll standChoiceBll = ObjectFactory.GetObject<IBizDomainBll>();
            IXEntityPermissionBll xPolicyBll = ObjectFactory.GetObject<IXEntityPermissionBll>();

            List<ListItem> contactTypes = new List<ListItem>();
            XEntityPermissionModel xEntity = new XEntityPermissionModel();
            xEntity.servProvCode = ConfigManager.AgencyCode;
            xEntity.entityType = XEntityPermissionConstant.REFERENCE_CONTACT_SEARCH;
            IEnumerable<XEntityPermissionModel> xEntities = xPolicyBll.GetXEntityPermissions(xEntity);
            IList<ItemValue> stdItems = standChoiceBll.GetContactTypeList(ConfigManager.AgencyCode, false, ContactTypeSource.Reference);

            foreach (var itemValue in stdItems)
            {
                if (itemValue == null)
                {
                    continue;
                }

                if (xEntities != null && xEntities.Any(o => o.entityId3.Equals(itemValue.Key) && ValidationUtil.IsNo(o.permissionValue)))
                {
                    continue;
                }

                if (xEntities == null || !xEntities.Any(o => o.entityId3.Equals(itemValue.Key)))
                {
                    continue;
                }

                ListItem listItem = new ListItem(itemValue.Value.ToString(), itemValue.Key);
                contactTypes.Add(listItem);
            }

            BindDDL(contactTypes, ddlContactType);
        }

        /// <summary>
        /// Binds the contact type in registration.
        /// </summary>
        /// <param name="ddlContactType">Type of the DDL contact.</param>
        /// <param name="entityPremissionString">The entity permission string.</param>
        public static void BindContactTypeInRegistration(AccelaDropDownList ddlContactType, string entityPremissionString)
        {
            ddlContactType.SourceType = DropDownListDataSourceType.STDandXPolicy;
            ddlContactType.StdCategory = entityPremissionString;

            List<ListItem> contactlist = GetContactTypeItemsInRegistration(entityPremissionString);

            if (!contactlist.Any())
            {
                return;
            }

            BindDDL(contactlist, ddlContactType);

            if (contactlist.Count == 1 && !AppSession.IsAdmin)
            {
                SetSelectedValue(ddlContactType, contactlist[0].Value);
            }
        }

        /// <summary>
        /// Get the contact type items for registration.
        /// </summary>
        /// <param name="entityPremissionString">The entity permission string.</param>
        /// <returns>ListItem collection</returns>
        public static List<ListItem> GetContactTypeItemsInRegistration(string entityPremissionString)
        {
            List<ListItem> contactlist = new List<ListItem>();
            IBizDomainBll standChoiceBll = ObjectFactory.GetObject<IBizDomainBll>();
            IXPolicyBll xpolicyBll = ObjectFactory.GetObject<IXPolicyBll>();

            IList<ItemValue> stdItems = standChoiceBll.GetContactTypeList(ConfigManager.AgencyCode, false, ContactTypeSource.Reference);
            IEnumerable<XPolicyModel> xpolicyList = xpolicyBll.GetPolicyListByCategory(ConfigManager.AgencyCode, entityPremissionString, ACAConstant.LEVEL_TYPE_AGENCY, ConfigManager.AgencyCode);
            bool hasRight = true;

            if (stdItems == null || !stdItems.Any())
            {
                return contactlist;
            }

            foreach (ItemValue contactTypeItem in stdItems)
            {
                if (contactTypeItem == null)
                {
                    continue;
                }

                hasRight = true;

                if (xpolicyList != null && xpolicyList.Any())
                {
                    if (xpolicyList.Any(x => x.data4 == contactTypeItem.Key && ValidationUtil.IsNo(x.data2)))
                    {
                        hasRight = false;
                    }

                    if (xpolicyList.All(x => x.data4 != contactTypeItem.Key))
                    {
                        hasRight = false;
                    }
                }

                if (hasRight || AppSession.IsAdmin)
                {
                    ListItem dropdownItem = new ListItem();
                    dropdownItem.Text = contactTypeItem.Value.ToString();
                    dropdownItem.Value = AppSession.IsAdmin && !hasRight ? ACAConstant.SPLIT_CHAR4 + ACAConstant.SPLIT_DOUBLE_VERTICAL + contactTypeItem.Key : contactTypeItem.Key;
                    contactlist.Add(dropdownItem);
                }
            }

            return contactlist;
        }

        /// <summary>
        /// Bind Contact Type by agency.
        /// </summary>
        /// <param name="ddlContactType">contact type dropdownlist</param>
        /// <param name="contactTypeSource">ContactType source</param>
        public static void BindContactType(AccelaDropDownList ddlContactType, string contactTypeSource)
        {
            List<ListItem> contactTypes = new List<ListItem>();
            IBizDomainBll standChoiceBll = ObjectFactory.GetObject(typeof(IBizDomainBll)) as IBizDomainBll;
            IList<ItemValue> stdItems = standChoiceBll.GetContactTypeList(ConfigManager.AgencyCode, false, contactTypeSource);

            foreach (ItemValue itemValue in stdItems)
            {
                //if xpolicy models is null. it will get item from stander choice.
                if (itemValue == null)
                {
                    continue;
                }

                ListItem listItem = new ListItem(itemValue.Value.ToString(), itemValue.Key);
                contactTypes.Add(listItem);
            }

            BindDDL(contactTypes, ddlContactType, true);
        }

        /// <summary>
        /// Bind Contact Type.
        /// </summary>
        /// <param name="ddlContactType">AccelaDropDownList control.</param>
        public static void BindContactType4License(AccelaDropDownList ddlContactType)
        {
            List<ListItem> listItems = new List<ListItem>();

            listItems.Add(new ListItem(BasePage.GetStaticTextByKey("license_contacttype_individual"), ContactType4License.Individual.ToString().ToLower()));
            listItems.Add(new ListItem(BasePage.GetStaticTextByKey("license_contacttype_organization"), ContactType4License.Organization.ToString().ToLower()));

            BindDDL(listItems, ddlContactType, true, false);
        }

        /// <summary>
        /// bind document type
        /// </summary>
        /// <param name="ddlDocType">AccelaDropDownList control.</param>
        /// <param name="ignoreDocTypePermission">ignore Doc Type Permission.</param>
        public static void BindAllDocumentTypes(AccelaDropDownList ddlDocType, bool ignoreDocTypePermission)
        {
            IEDMSDocumentBll edmsBll = (IEDMSDocumentBll)ObjectFactory.GetObject(typeof(IEDMSDocumentBll));
            RefDocumentModel[] items = edmsBll.GetAllDocumentTypes(ConfigManager.AgencyCode);
            List<ListItem> listItems = new List<ListItem>();

            if (items != null && items.Length > 0)
            {
                foreach (RefDocumentModel item in items)
                {
                    if (!ignoreDocTypePermission && ValidationUtil.IsNo(item.isRestrictDocType4ACA))
                    {
                        continue;
                    }

                    string text = I18nStringUtil.GetString(item.resDocumentType, item.documentType);
                    ListItem listItem = new ListItem()
                    {
                        Text = text,
                        Value = item.documentCode + ACAConstant.SPLIT_DOUBLE_COLON + item.documentType
                    };

                    listItems.Add(listItem);
                }
            }

            BindDDL(listItems, ddlDocType, true, false);
        }

        /// <summary>
        /// Bind ItemValue list to AccelaDropDownList control.
        /// default sorting behavior is to sort text of ListItem.
        /// </summary>
        /// <param name="items">the list of ListItem to be bound.</param>
        /// <param name="ddlControl">AccelaDropDownList control.</param>
        public static void BindDDL(IList<ListItem> items, AccelaDropDownList ddlControl)
        {
            BindDDL(items, ddlControl, true, true);
        }

        /// <summary>
        /// Bind ItemValue list to DropDownList control.
        /// default sorting behavior is to sort text of ListItem.
        /// </summary>
        /// <param name="items">list item value</param>
        /// <param name="ddlControl">DropDownList control</param>
        /// <param name="needDefaultItem">need Default Item or not</param>
        public static void BindDDL(IList<ListItem> items, DropDownList ddlControl, bool needDefaultItem)
        {
            BindDDL(items, ddlControl, needDefaultItem, true);
        }

        /// <summary>
        /// Bind ItemValue list to AccelaDropDownList control.
        /// </summary>
        /// <param name="items">the list of ListItem to be bound.</param>
        /// <param name="ddlControl">DropDownList control.</param>
        /// <param name="needDefaultItem">need Default Item or not</param>
        /// <param name="needSorting">need Sorting or not</param>
        public static void BindDDL(IList<ListItem> items, DropDownList ddlControl, bool needDefaultItem, bool needSorting)
        {
            ddlControl.Items.Clear();

            if (needDefaultItem)
            {
                ddlControl.Items.Add(DefaultListItem);
            }

            if (items != null && items.Count > 0)
            {
                if (needSorting)
                {
                    ((List<ListItem>)items).Sort(ListItemComparer.Instance); //defaul sorting behavior
                }

                foreach (ListItem item in items)
                {
                    ddlControl.Items.Add(item);
                }
            }
        }

        /// <summary>
        /// Bind the degree type list to AccelaDropDownList control.
        /// </summary>
        /// <param name="ddlDegreeType">degree type to bind AccelaDropDownList control.</param>
        public static void BindDegreeType(AccelaDropDownList ddlDegreeType)
        {
            BindStandardChoise(ddlDegreeType, BizDomainConstant.STD_CAT_EDUCATION_DEGREE_TYPE, StandardChoiceMaxLength.MAX_LENGTH_EDUCATION_DEGREE);
        }

        /// <summary>
        /// bind education to AccelaDropDownList control
        /// </summary>
        /// <param name="ddlSearchType">AccelaDropDownList control.</param>
        public static void BindEducationSearchType(AccelaDropDownList ddlSearchType)
        {
            IList<ListItem> items = GetEducationTypeItems();
            BindDDL(items, ddlSearchType, false, true);
        }

        /// <summary>
        /// Bind Enabled and Disabled to AccelaDropDownList control.
        /// </summary>
        /// <param name="ddlist">AccelaDropDownList control.</param>
        public static void BindEnabledDisableDropDown(AccelaDropDownList ddlist)
        {
            IList<ListItem> list = new List<ListItem>();
            list.Add(new ListItem(BasePage.GetStaticTextByKey("ACA_Common_Enabled"), ACAConstant.VALID_STATUS));
            list.Add(new ListItem(BasePage.GetStaticTextByKey("ACA_Common_Disabled"), ACAConstant.INVALID_STATUS));

            BindDDL(list, ddlist, true, false);
        }

        /// <summary>
        /// Bind examination status to AccelaDropDownList control.
        /// </summary>
        /// <param name="ddlist">AccelaDropDownList control.</param>
        public static void BindExaminationStatus(AccelaDropDownList ddlist)
        {
            IList<ListItem> list = new List<ListItem>();
            list.Add(new ListItem(BasePage.GetStaticTextByKey("aca_common_examination_status_completed"), ACAConstant.EXAMINATION_STATUS_COMPLETED_PENDING));
            list.Add(new ListItem(BasePage.GetStaticTextByKey("aca_common_examination_status_pending"), ACAConstant.EXAMINATION_STATUS_PENDING));            
            BindDDL(list, ddlist, false, false);
        }

        /// <summary>
        /// Bind the gender item to AccelaRadioButtonList control.
        /// </summary>
        /// <param name="rdlGender">AccelaDropDownList control to be bind.</param>
        public static void BindGender(AccelaRadioButtonList rdlGender)
        {
            List<ListItem> listItems = GetGenderItems();

            rdlGender.Items.Clear();

            foreach (ListItem item in listItems)
            {
                rdlGender.Items.Add(item);
            }
        }

        /// <summary>
        /// Bind the gender item to AccelaDropDownList control.
        /// </summary>
        /// <param name="ddlGender">AccelaDropDownList control to be bind.</param>
        public static void BindGender(AccelaDropDownList ddlGender)
        {
            List<ListItem> listItems = GetGenderItems();

            BindDDL(listItems, ddlGender, true, false);
        }

        /// <summary>
        /// Bind the license type list to AccelaDropDownList control.
        /// </summary>
        /// <param name="ddlLicenseType">AccelaDropDownList control to be bind.</param>
        public static void BindLicenseType(AccelaDropDownList ddlLicenseType)
        {
            BindStandardChoise(ddlLicenseType, BizDomainConstant.STD_CAT_LICENSE_TYPE, StandardChoiceMaxLength.MAX_LENGTH_LP_TYPE);
        }

        /// <summary>
        /// Bind the license type list to AccelaDropDownList control.
        /// </summary>
        /// <param name="ddlLicenseType">AccelaDropDownList control to be bind.</param>
        /// <param name="moduleName">The module name.</param>
        /// <param name="licensingBoard">The licensing board.</param>
        public static void BindLicenseType(AccelaDropDownList ddlLicenseType, string moduleName, string licensingBoard)
        {
            BindStandardChoise(ddlLicenseType, BizDomainConstant.STD_CAT_LICENSE_TYPE, StandardChoiceMaxLength.MAX_LENGTH_LP_TYPE);

            if (string.IsNullOrEmpty(licensingBoard))
            {
                return;
            }

            // get the license type list that belong to license board.
            XEntityPermissionModel xentity = new XEntityPermissionModel();
            xentity.servProvCode = ConfigManager.AgencyCode;
            xentity.entityType = XEntityPermissionConstant.LICENSING_BOARD;
            xentity.entityId = I18nStringUtil.GetString(moduleName, ConfigManager.AgencyCode);
            xentity.entityId2 = licensingBoard;

            IXEntityPermissionBll xEntityPermissionBll = ObjectFactory.GetObject<IXEntityPermissionBll>();
            IEnumerable<XEntityPermissionModel> licenseTypeEntities = xEntityPermissionBll.GetXEntityPermissions(xentity);

            if (licenseTypeEntities == null || !licenseTypeEntities.Any())
            {
                return;
            }

            List<ListItem> licenseListType = new List<ListItem>();

            foreach (ListItem item in ddlLicenseType.Items)
            {
                string licenseType = item.Value;

                if (string.IsNullOrEmpty(licenseType))
                {
                    licenseListType.Add(item);
                    continue;
                }

                XEntityPermissionModel xEntityLicenseType = licenseTypeEntities.FirstOrDefault(f => string.Equals(licenseType, f.entityId3, StringComparison.InvariantCultureIgnoreCase));

                if (xEntityLicenseType != null && !ValidationUtil.IsNo(xEntityLicenseType.permissionValue))
                {
                    licenseListType.Add(item);
                }
            }

            ddlLicenseType.Items.Clear();
            ddlLicenseType.Items.AddRange(licenseListType.ToArray());
        }

        /// <summary>
        /// Bind the license type list for food facility to AccelaDropDownList control.
        /// </summary>
        /// <param name="ddlLicenseType">AccelaDropDownList control to be bind.</param>
        public static void BindLincenseType4FoodFacility(AccelaDropDownList ddlLicenseType)
        {
            BindLicenseType(ddlLicenseType);

            string foodFacilityType = StandardChoiceUtil.GetStandardChoiceValueByKey(BizDomainConstant.STD_FOOD_FACILITY_INSPECTION, "RELATED_LICENSE_TYPES");

            if (!string.IsNullOrEmpty(foodFacilityType))
            {
                string[] foodFacilityTypes = foodFacilityType.Split(ACAConstant.COMMA.ToCharArray());
                List<string> foodFacilityTypeList = new List<string>(foodFacilityTypes);

                int ddlLicenseTypeCount = ddlLicenseType.Items.Count;

                for (int i = ddlLicenseTypeCount - 1; i >= 0; i--)
                {
                    ListItem item = ddlLicenseType.Items[i];

                    if (!item.Equals(DropDownListBindUtil.DefaultListItem) && !foodFacilityTypeList.Contains(item.Value))
                    {
                        ddlLicenseType.Items.Remove(item);
                    }
                }
            }
        }

        /// <summary>
        /// Bind the license type list to AccelaDropDownList control.
        /// </summary>
        /// <param name="ddlLicenseType">AccelaDropDownList control to be bind.</param>
        public static void BindLicensingBoard(AccelaDropDownList ddlLicenseType)
        {
            BindStandardChoise(ddlLicenseType, BizDomainConstant.STD_CAT_LICENSING_BOARD, StandardChoiceMaxLength.MAX_LENGTH_LICENSING_BOARD);
        }

        /// <summary>
        /// bind modules to AccelaDropDownList control
        /// </summary>
        /// <param name="ddlModule">AccelaDropDownList control.</param>
        /// <param name="isGlobalSearch">indicates whether it is for GlobalSearch.(global search need to replace '--select--' with 'all records')</param>
        public static void BindModules(AccelaDropDownList ddlModule, bool isGlobalSearch)
        {
            IList<ListItem> items = GetModules();

            if (isGlobalSearch)
            {
                BindDDL(items, ddlModule, false, true);

                string allModulesText = LabelUtil.GetTextByKey("per_globalsearch_label_allrecords", null);
                string allModuleKeys = GetAllModuleKeys(items);
                ddlModule.Items.Insert(0, new ListItem(allModulesText, allModuleKeys));
            }
            else
            {
                BindDDL(items, ddlModule, true, false);
            }
        }

        /// <summary>
        /// bind the data for month.
        /// </summary>
        /// <param name="ddlExpMonth">AccelaDropDownList control.</param>
        public static void BindMonth(AccelaDropDownList ddlExpMonth)
        {
            for (int i = 1; i <= 12; i++)
            {
                ListItem item = new ListItem();

                if (i < 10)
                {
                    item.Text = "0" + i;
                    item.Value = "0" + i;
                    ddlExpMonth.Items.Add(item);
                }
                else
                {
                    item.Text = i.ToString();
                    item.Value = i.ToString();
                    ddlExpMonth.Items.Add(item);
                }
            }
        }

        /// <summary>
        /// Bind Provider Type, filter user sequence number.
        /// </summary>
        /// <param name="ddlProvider">AccelaDropDownList Control</param>
        public static void BindProviderByUserSeqNbr(AccelaDropDownList ddlProvider)
        {
            IProviderBll providerBll = (IProviderBll)ObjectFactory.GetObject(typeof(IProviderBll));
            ProviderModel4WS[] providermodels = providerBll.GetProviderListByUserSeqNbr(ConfigManager.AgencyCode, AppSession.User.UserSeqNum);

            if (providermodels != null && providermodels.Length > 0)
            {
                IList<ListItem> providerList = new List<ListItem>();

                foreach (ProviderModel4WS provider in providermodels)
                {
                    providerList.Add(new ListItem(provider.providerName + ACAConstant.SPLIT_CHAR + provider.providerNo, provider.providerNbr.ToString()));
                }

                ((List<ListItem>)providerList).Sort(ListItemComparer.Instance);

                BindDDL(providerList, ddlProvider, true, false);
            }
            else
            {
                BindDDL(null, ddlProvider, true, false);
            }
        }

        /// <summary>
        /// Binds the provider info by exam.
        /// </summary>
        /// <param name="servProvCode">The service provide code.</param>
        /// <param name="examNbr">The exam NBR.</param>
        /// <param name="ddlProvider">The DDL provider.</param>
        /// <param name="ddlProviderCity">The DDL provider city.</param>
        /// <param name="ddlProviderState">State of the DDL provider.</param>
        public static void BindProviderInfoByExam(string servProvCode, string examNbr, AccelaDropDownList ddlProvider, AccelaDropDownList ddlProviderCity, AccelaDropDownList ddlProviderState)
        {
            IProviderBll providerBll = (IProviderBll)ObjectFactory.GetObject(typeof(IProviderBll));
            ProviderModel[] providermodels;

            if (string.IsNullOrEmpty(examNbr))
            {
                providermodels = null;
            }
            else
            {
                providermodels = providerBll.GetProvidersByExamNbr(servProvCode, examNbr);
            }

            if (providermodels != null && providermodels.Length > 0)
            {
                IList<ListItem> providerList = new List<ListItem>();
                IList<ListItem> providerCityList = new List<ListItem>();
                IList<ListItem> providerStateList = new List<ListItem>();

                foreach (ProviderModel provider in providermodels)
                {
                    providerList.Add(new ListItem(provider.providerName + ACAConstant.SPLITLINE + provider.providerNo, provider.providerNbr.ToString()));

                    if (provider.rProviderLocations != null && provider.rProviderLocations.Length > 0)
                    {
                        foreach (var providerLocation in provider.rProviderLocations)
                        {
                            if (!string.IsNullOrEmpty(providerLocation.city) && providerCityList.Where(o => o.Value == providerLocation.city).Count() == 0)
                            {
                                providerCityList.Add(new ListItem(providerLocation.city, providerLocation.city));
                            }

                            if (!string.IsNullOrEmpty(providerLocation.state) && providerStateList.Where(o => o.Value == providerLocation.state).Count() == 0)
                            {
                                providerStateList.Add(new ListItem(providerLocation.state, providerLocation.state));
                            }
                        }
                    }
                }

                ((List<ListItem>)providerList).Sort(ListItemComparer.Instance);

                BindDDL(providerList, ddlProvider, true, false);

                BindDDL(providerStateList, ddlProviderState, true, false);

                BindDDL(providerCityList, ddlProviderCity, true, false);
            }
            else
            {
                BindDDL(null, ddlProvider, true, false);

                BindDDL(null, ddlProviderState, true, false);

                BindDDL(null, ddlProviderCity, true, false);
            }
        }

        /// <summary>
        /// Sets the and get select value, if input DDl text will return the select value.
        /// </summary>
        /// <param name="ddlProvider">The DDL provider.</param>
        /// <param name="textOrValue">The text or value.</param>
        /// <returns>the select value.</returns>
        public static string SetAndGetSelectValue(AccelaDropDownList ddlProvider, string textOrValue)
        {
            string result = string.Empty;

            if (ddlProvider.Items.Count == 0)
            {
                return result;
            }

            bool isFound = false;

            if (textOrValue == null)
            {
                textOrValue = string.Empty;
            }

            foreach (ListItem item in ddlProvider.Items)
            {
                if (item.Value.Equals(textOrValue, StringComparison.InvariantCultureIgnoreCase) ||
                    item.Text.Equals(textOrValue, StringComparison.InvariantCultureIgnoreCase))
                {
                    result = item.Value;
                    ddlProvider.SelectedValue = result;
                    isFound = true;
                    break;
                }
            }

            if (!isFound)
            {
                ddlProvider.SelectedIndex = -1;
            }

            return result;
        }

        /// <summary>
        /// Bind Provider Type.
        /// </summary>
        /// <param name="ddlProviderType">AccelaDropDownList control.</param>
        public static void BindProviderType(AccelaDropDownList ddlProviderType)
        {
            List<ListItem> listItems = new List<ListItem>();

            listItems.Add(new ListItem(BasePage.GetStaticTextByKey("educationrelationsearchform_offereducation"), ProviderType.OfferEducation.ToString()));
            listItems.Add(new ListItem(BasePage.GetStaticTextByKey("educationrelationsearchform_offercontinuing"), ProviderType.OfferContinuing.ToString()));
            listItems.Add(new ListItem(BasePage.GetStaticTextByKey("educationrelationsearchform_offerexamination"), ProviderType.OfferExamination.ToString()));

            BindDDL(listItems, ddlProviderType, true, false);
        }

        /// <summary>
        /// Bind the salutation item to AccelaDropDownList control.
        /// </summary>
        /// <param name="ddlSalutation">AccelaDropDownList control to be bind.</param>
        public static void BindSalutation(AccelaDropDownList ddlSalutation)
        {
            BindStandardChoise(ddlSalutation, BizDomainConstant.STD_CAT_SALUTATION, StandardChoiceMaxLength.MAX_LENGTH_SALUTATION);
        }

        /// <summary>
        /// Bind the specific standard choice items to AccelaDropDownList control by standard choice category name.
        /// </summary>
        /// <param name="ddlControl">AccelaDropDownList control.</param>
        /// <param name="stdCatName">standard choice category name which can be got from ACAConstant class.</param>
        public static void BindStandardChoise(AccelaDropDownList ddlControl, string stdCatName)
        {
            BindStandardChoise(ddlControl, stdCatName, MAX_LENGTH_NO_LIMITED);
        }

        /// <summary>
        /// Get contact address type by contact type and bind to a DropDownList control.
        /// </summary>
        /// <param name="ddlControl">DropDownList control.</param>
        /// <param name="contactType">Contact type.</param>
        public static void BindContactAddressType(AccelaDropDownList ddlControl, string contactType)
        {
            if (!string.IsNullOrEmpty(contactType))
            {
                IRefAddressBll refAddressBll = (IRefAddressBll)ObjectFactory.GetObject(typeof(IRefAddressBll));
                XRefContactAddressTypeModel[] contactAddressTypes = refAddressBll.GetContactAddressTypeByContactType(ConfigManager.AgencyCode, contactType);

                if (contactAddressTypes != null)
                {
                    List<ListItem> items = new List<ListItem>();
                    List<string> requiredAddressTypes = new List<string>();
                    Hashtable stdItems = new Hashtable(StringComparer.InvariantCultureIgnoreCase);

                    foreach (XRefContactAddressTypeModel addressType in contactAddressTypes)
                    {
                        ListItem item = new ListItem();
                        string addressTypeValue = StandardChoiceUtil.GetStandardChoiceValueByKey(BizDomainConstant.STD_CAT_CONTACT_ADDRESS_TYPE, addressType.addressType);

                        if (!string.IsNullOrEmpty(addressTypeValue))
                        {
                            item.Text = addressTypeValue;
                            item.Value = addressType.addressType;
                            items.Add(item);
                        }

                        if (ValidationUtil.IsYes(addressType.required))
                        {
                            requiredAddressTypes.Add(addressType.addressType);
                        }
                    }

                    BindDDL(items, ddlControl);
                    stdItems.Add(contactType, requiredAddressTypes);
                    string cacheKey = CacheConstant.CACHE_KEY_REQUIRED_CONTACT_ADDRESS_TYPE;
                    ICacheManager cacheManager = (ICacheManager)ObjectFactory.GetObject(typeof(ICacheManager));
                    cacheManager.AddSingleItemToCache(cacheKey, stdItems, cacheManager.GetDefaultExpirationTime());
                }
                else
                {
                    BindStandardChoise(ddlControl, BizDomainConstant.STD_CAT_CONTACT_ADDRESS_TYPE);
                }
            }
            else
            {
                BindStandardChoise(ddlControl, BizDomainConstant.STD_CAT_CONTACT_ADDRESS_TYPE);
            }
        }

        /// <summary>
        /// Bind the states to AccelaDropDownList control.
        /// </summary>
        /// <param name="ddlState">AccelaDropDownList control to be bind.</param>
        public static void BindState(AccelaDropDownList ddlState)
        {
            BindStandardChoise(ddlState, BizDomainConstant.STD_CAT_STATE_TYPE, StandardChoiceMaxLength.MAX_LENGTH_PlANPAY_STATES);
        }

        /// <summary>
        /// Bind the street direction to AccelaDropDownList control.
        /// </summary>
        /// <param name="ddlStreetDirection">AccelaDropDownList control to be bind.</param>
        public static void BindStreetDirection(AccelaDropDownList ddlStreetDirection)
        {
            BindStandardChoise(ddlStreetDirection, BizDomainConstant.STD_CAT_STREET_DIRECTIONS, StandardChoiceMaxLength.MAX_LENGTH_STREET_DIRECTION);
        }

        /// <summary>
        /// Bind the cap status to AccelaDropDownList control.
        /// </summary>
        /// <param name="ddlCapStatus">AccelaDropDownList control to be bind.</param>
        /// <param name="moduleName">The module name</param>
        /// <param name="appStatusGroupModels">The app Status Group Model list.</param>
        /// <param name="stdCategory">The standard choice's category in policy name "ACA_CONFIGS".</param>
        public static void BindCapStatus(AccelaDropDownList ddlCapStatus, string moduleName, AppStatusGroupModel4WS[] appStatusGroupModels, string stdCategory = null)
        {
            List<ListItem> listItems = new List<ListItem>();

            if (ddlCapStatus.Visible && appStatusGroupModels != null && appStatusGroupModels.Length > 0)
            {
                foreach (AppStatusGroupModel4WS appStatusGroupModel in appStatusGroupModels)
                {
                    string appStatus = I18nStringUtil.GetString(appStatusGroupModel.resStatus, appStatusGroupModel.status);
                    ListItem li = new ListItem(appStatus, appStatusGroupModel.status);

                    if (!listItems.Contains(li))
                    {
                        listItems.Add(li);
                    }
                }
            }

            if (!string.IsNullOrWhiteSpace(stdCategory))
            {
                ddlCapStatus.StdCategory = stdCategory;
                ddlCapStatus.SourceType = DropDownListDataSourceType.STDandXPolicy;

                IXPolicyBll xpolicyBll = ObjectFactory.GetObject<IXPolicyBll>();
                IEnumerable<XPolicyModel> xpolicyList = xpolicyBll.GetPolicyListByCategory(ConfigManager.AgencyCode, stdCategory, ACAConstant.LEVEL_TYPE_MODULE, moduleName);
                List<ListItem> capStatuslist = new List<ListItem>();

                if (!listItems.Any())
                {
                    BindDDL(null, ddlCapStatus, true, true);
                    return;
                }

                foreach (ListItem capStatusItem in listItems)
                {
                    if (capStatusItem == null)
                    {
                        continue;
                    }

                    bool hasRight = true;

                    if (xpolicyList != null && xpolicyList.Any())
                    {
                        if (xpolicyList.Any(x => x.data4 == capStatusItem.Value && ValidationUtil.IsNo(x.data2)))
                        {
                            hasRight = false;
                        }
                        else if (xpolicyList.All(x => x.data4 != capStatusItem.Value))
                        {
                            hasRight = false;
                        }
                    }

                    if (hasRight || AppSession.IsAdmin)
                    {
                        ListItem dropdownItem = new ListItem();
                        dropdownItem.Text = capStatusItem.Text;
                        dropdownItem.Value = AppSession.IsAdmin && !hasRight
                            ? ACAConstant.SPLIT_CHAR4 + ACAConstant.SPLIT_DOUBLE_VERTICAL + capStatusItem.Value
                            : capStatusItem.Value;
                        capStatuslist.Add(dropdownItem);
                    }
                }

                BindDDL(capStatuslist, ddlCapStatus, true, true);
            }
            else
            {
                BindDDL(listItems, ddlCapStatus, true, true);
            }
        }

        /// <summary>
        /// Bind the street suffixes to AccelaDropDownList control.
        /// </summary>
        /// <param name="ddlStreetSuffix">AccelaDropDownList control to be bind.</param>
        public static void BindStreetSuffix(AccelaDropDownList ddlStreetSuffix)
        {
            BindStandardChoise(ddlStreetSuffix, BizDomainConstant.STD_CAT_STREET_SUFFIXES, StandardChoiceMaxLength.MAX_LENGTH_STREET_SUFFIXES);
        }

        /// <summary>
        /// Bind agency list to AccelaDropDownList control.
        /// </summary>
        /// <param name="ddlSubAgency">AccelaDropDownList control</param>
        public static void BindSubAgencies(AccelaDropDownList ddlSubAgency)
        {
            IList<ListItem> list = GetAgencyList();

            //For super agency , it need "--select--"; For normal agency, it needn't "--select--".
            bool isNeedDefaultItem = StandardChoiceUtil.IsSuperAgency();
            BindDDL(list, ddlSubAgency, isNeedDefaultItem, true);
        }

        /// <summary>
        /// Bind the subdivisions to AccelaDropDownList control.
        /// </summary>
        /// <param name="ddlSubdivision">AccelaDropDownList control to be bind.</param>
        public static void BindSubdivision(AccelaDropDownList ddlSubdivision)
        {
            BindStandardChoise(ddlSubdivision, BizDomainConstant.STD_CAT_APO_SUBDIVISIONS, StandardChoiceMaxLength.MAX_LENGTH_SUBDIVISIONS);
        }

        /// <summary>
        /// Bind the unit types to AccelaDropDownList control.
        /// </summary>
        /// <param name="ddlUnitType">AccelaDropDownList control to be bind.</param>
        public static void BindUnitType(AccelaDropDownList ddlUnitType)
        {
            BindStandardChoise(ddlUnitType, BizDomainConstant.STD_CAT_UNIT_TYPES, StandardChoiceMaxLength.MAX_LENGTH_UNIT_TYPES);
        }

        /// <summary>
        /// bind valid licenses to AccelaDropDownList control
        /// </summary>
        /// <param name="licenseArray">The licenseModel4WS array, if it's null, it will be queried within this routine.</param>
        /// <param name="ddlLicenseList">AccelaDropDownList control to be bind</param>
        /// <param name="applicable">the option to determine whether the 'NONE_APPLICABLE' is needed</param>
        public static void BindValidLicense(LicenseModel4WS[] licenseArray, AccelaDropDownList ddlLicenseList, bool applicable)
        {
            // true - get only valid licenses. false - get all licenses including invalid license.
            if (licenseArray == null)
            {
                licenseArray = AppSession.User.Licenses;
            }

            if (licenseArray != null && licenseArray.Length > 0)
            {
                IList<ListItem> licenseList = new List<ListItem>();
                string[] licStateInfo = new string[3];
                string licenseText = string.Empty;

                foreach (LicenseModel4WS license in licenseArray)
                {
                    licStateInfo[0] = StandardChoiceUtil.IsDisplayLicenseState() ? I18nUtil.DisplayStateForI18N(license.licState, license.countryCode) : string.Empty;
                    licStateInfo[1] = I18nStringUtil.GetString(license.resLicenseType, license.licenseType);
                    licStateInfo[2] = license.stateLicense;
                    licenseText = DataUtil.ConcatStringWithSplitChar(licStateInfo, ACAConstant.BLANK);

                    licenseList.Add(new ListItem(licenseText, license.licSeqNbr));
                }

                ((List<ListItem>)licenseList).Sort(ListItemComparer.Instance);

                if (applicable)
                {
                    string noneApplicableText = LabelUtil.GetTextByKey("per_selectLicProfessional_label_option", null);
                    licenseList.Add(new ListItem(noneApplicableText, NONE_APPLICABLE));
                }

                BindDDL(licenseList, ddlLicenseList, true, false);
            }
        }

        /// <summary>
        /// bind valid licenses/addresses/owners/parcels to AccelaDropDownList control
        /// </summary>
        /// <param name="associatedType">payment associated type.</param>
        /// <param name="ddlAssociatedType">AccelaDropDownList control to be bind</param>
        /// <param name="applicable">the option to determine whether the 'NONE_APPLICABLE' is needed</param>
        public static void BindTrustAccountValidAssociatedTypes(ACAConstant.PaymentAssociatedType associatedType, AccelaDropDownList ddlAssociatedType, bool applicable)
        {
            ddlAssociatedType.Items.Clear();

            if (AppSession.User == null || AppSession.User.UserModel4WS == null)
            {
                return;
            }

            IList<ListItem> associatedTypesList = GetTrustAccountValidAssociatedTypes(associatedType);

            if (associatedTypesList != null && associatedTypesList.Count > 0)
            {
                ((List<ListItem>)associatedTypesList).Sort(ListItemComparer.Instance);

                if (applicable)
                {
                    string noneApplicableText = LabelUtil.GetTextByKey("per_selectLicProfessional_label_option", null);
                    associatedTypesList.Add(new ListItem(noneApplicableText, NONE_APPLICABLE));
                }

                BindDDL(associatedTypesList, ddlAssociatedType, true, false);

                if (associatedTypesList.Count == 1)
                {
                    ddlAssociatedType.SelectedValue = associatedTypesList[0].Value;
                }
            }
        }

        /// <summary>
        /// get data source for <c>ddlAssociatedType</c> control.
        /// </summary>
        /// <param name="associatedType">payment associated type.</param>
        /// <returns>The validate types that trust account associated.</returns>
        public static IList<ListItem> GetTrustAccountValidAssociatedTypes(ACAConstant.PaymentAssociatedType associatedType)
        {
            IList<ListItem> associatedTypeList = new List<ListItem>();

            switch (associatedType)
            {
                case ACAConstant.PaymentAssociatedType.Licenses:
                    LicenseModel4WS[] licenses = AppSession.User.UserModel4WS.licenseModel;
                    string[] licenseInfo = new string[3];
                    string licenseText = string.Empty;

                    if (licenses != null && licenses.Length > 0)
                    {
                        foreach (LicenseModel4WS license in licenses)
                        {
                            licenseInfo[0] = StandardChoiceUtil.IsDisplayLicenseState() ? I18nUtil.DisplayStateForI18N(license.licState, license.countryCode) : string.Empty;
                            licenseInfo[1] = I18nStringUtil.GetString(license.resLicenseType, license.licenseType);
                            licenseInfo[2] = license.stateLicense;
                            licenseText = DataUtil.ConcatStringWithSplitChar(licenseInfo, ACAConstant.BLANK);

                            associatedTypeList.Add(new ListItem(licenseText, license.licSeqNbr));
                        }
                    }

                    break;
                case ACAConstant.PaymentAssociatedType.Contacts:
                    PeopleModel4WS[] contacts = AppSession.User.UserModel4WS.peopleModel;

                    if (contacts != null && contacts.Length > 0)
                    {
                        foreach (PeopleModel4WS contact in contacts)
                        {
                            if (ConfigManager.AgencyCode.Equals(contact.serviceProviderCode, StringComparison.InvariantCultureIgnoreCase))
                            {
                                string contactName = string.IsNullOrEmpty(contact.firstName) && string.IsNullOrEmpty(contact.lastName)
                                  ? contact.fullName : contact.firstName + " " + contact.lastName;

                                associatedTypeList.Add(new ListItem(contactName, contact.contactSeqNumber));
                            }
                        }
                    }

                    break;
                case ACAConstant.PaymentAssociatedType.Addresses:
                    RefAddressModel[] addresses = AppSession.User.UserModel4WS.addressList;

                    IAddressBuilderBll addressBuilderBll = ObjectFactory.GetObject<IAddressBuilderBll>();
                    string addressString = string.Empty;

                    if (addresses != null && addresses.Length > 0)
                    {
                        foreach (RefAddressModel address in addresses)
                        {
                            addressString = addressBuilderBll.BuildAddressByFormatType(address, null, AddressFormatType.SHORT_ADDRESS_NO_FORMAT);
                            associatedTypeList.Add(new ListItem(addressString, address.refAddressId.ToString()));
                        }
                    }

                    break;
                case ACAConstant.PaymentAssociatedType.Parcels:
                    ParcelModel[] parcels = AppSession.User.UserModel4WS.parcelList;

                    if (parcels != null && parcels.Length > 0)
                    {
                        foreach (ParcelModel parcel in parcels)
                        {
                            associatedTypeList.Add(new ListItem(parcel.parcelNumber, parcel.parcelNumber.ToString()));
                        }
                    }

                    break;
                case ACAConstant.PaymentAssociatedType.Record:
                    associatedTypeList = TrustAccountUtil.GetAssociatedRecordList();
                    break;
            }

            return associatedTypeList;
        }

        /// <summary>
        /// bind the data for year.
        /// </summary>
        /// <param name="ddlExpYear">AccelaDropDownList control.</param>
        public static void BindYear(AccelaDropDownList ddlExpYear)
        {
            int year = DateTime.Now.Year;

            for (int i = year; i < year + 10; i++)
            {
                ListItem item = new ListItem();
                item.Text = i.ToString();
                item.Value = i.ToString();
                ddlExpYear.Items.Add(item);
            }
        }

        /// <summary>
        /// Bind Yes and No to AccelaDropDownList control.
        /// </summary>
        /// <param name="ddlist">AccelaDropDownList control.</param>
        public static void BindYesNoDropDown(AccelaDropDownList ddlist)
        {
            IList<ListItem> list = new List<ListItem>();
            list.Add(new ListItem(BasePage.GetStaticTextByKey("ACA_Common_Yes"), ACAConstant.COMMON_Y));
            list.Add(new ListItem(BasePage.GetStaticTextByKey("ACA_Common_No"), ACAConstant.COMMON_N));

            BindDDL(list, ddlist, true, false);
        }

        /// <summary>
        /// Binds the pass fail drop down.
        /// </summary>
        /// <param name="ddlist">The dropdownlist.</param>
        /// <param name="needDefaultItem">true:need default item, false:no need default item.</param>
        public static void BindPassFailDropDown(AccelaDropDownList ddlist, bool needDefaultItem)
        {
            IList<ListItem> list = new List<ListItem>();
            list.Add(new ListItem(BasePage.GetStaticTextByKey("aca_common_pass"), ACAConstant.COMMON_ONE));
            list.Add(new ListItem(BasePage.GetStaticTextByKey("aca_common_fail"), ACAConstant.COMMON_ZERO));

            BindDDL(list, ddlist, needDefaultItem, false);
        }

        /// <summary>
        /// Bind Yes and No to DropDownList control.
        /// </summary>
        /// <param name="ddlist">DropDownList control.</param>
        public static void BindAssetClassType(AccelaDropDownList ddlist)
        {
            IList<ListItem> list = new List<ListItem>();
            list.Add(new ListItem(ASSET_CLASS_TYPE_POINT, ASSET_CLASS_TYPE_POINT));
            list.Add(new ListItem(ASSET_CLASS_TYPE_LINEAR, ASSET_CLASS_TYPE_LINEAR));
            list.Add(new ListItem(ASSET_CLASS_TYPE_NODE_LINK_LINEAR, ASSET_CLASS_TYPE_NODE_LINK_LINEAR));
            list.Add(new ListItem(ASSET_CLASS_TYPE_POLYGON, ASSET_CLASS_TYPE_POLYGON));
            list.Add(new ListItem(ASSET_CLASS_TYPE_COMPONENT, ASSET_CLASS_TYPE_COMPONENT));

            BindDDL(list, ddlist, true, false);
        }

        /// <summary>
        /// Bind the asset type to DropDownList control.
        /// </summary>
        /// <param name="ddlList">DropDownList control to be bind.</param>
        /// <param name="assetGroup">asset group</param>
        public static void BindAssetTypeByAssetGroup(AccelaDropDownList ddlList, string assetGroup)
        {
            IAssetBll assetBll = ObjectFactory.GetObject<IAssetBll>();
            IList<AssetTypeModel> assetTypeItems = assetBll.GetAssetTypeListByAssetGroup(ConfigManager.AgencyCode, assetGroup);
            List<ListItem> listItems = new List<ListItem>();

            if (assetTypeItems != null && assetTypeItems.Count > 0)
            {
                foreach (AssetTypeModel item in assetTypeItems)
                {
                    listItems.Add(new ListItem(item.assetType, item.assetType));
                }
            }

            BindDDL(listItems, ddlList);
        }

        /// <summary>
        /// Bind the asset type to DropDownList control.
        /// </summary>
        /// <param name="ddlList">DropDownList control to be bind.</param>
        /// <param name="assetGroup">asset group</param>
        /// <param name="assetTypeCapTypeModels">asset Type Cap Type Models</param>
        public static void BindAssetTypeByAssetGroup(AccelaDropDownList ddlList, string assetGroup, List<XAssetTypeCapTypeModel> assetTypeCapTypeModels)
        {
            if (assetTypeCapTypeModels == null)
            {
                return;
            }

            List<XAssetTypeCapTypeModel> assetTypeItems = assetTypeCapTypeModels.Where(o => o.assetGroup.Equals(assetGroup, StringComparison.InvariantCultureIgnoreCase)).ToList();
            List<ListItem> listItems = new List<ListItem>();

            if (assetTypeItems != null && assetTypeItems.Count > 0)
            {
                foreach (XAssetTypeCapTypeModel item in assetTypeItems)
                {
                    listItems.Add(new ListItem(item.assetType, item.assetType));
                }
            }

            BindDDL(listItems, ddlList);
        }

        /// <summary>
        /// get items for hardcode AccelaDropDownList control
        /// </summary>
        /// <param name="ddl">AccelaDropDownList control</param>
        /// <param name="stdCatName">standard choice category name which can be got from ACAConstant class.</param>
        /// <param name="moduleName">module Name</param>
        /// <returns>IList item value</returns>
        public static IList<ItemValue> GetHardcodeItems4DDL(AccelaDropDownList ddl, string stdCatName, string moduleName)
        {
            ddl.StdCategory = stdCatName;
            ddl.SourceType = DropDownListDataSourceType.HardCode;

            //IBizDomainBll standChoiceBll = ObjectFactory.GetObject(typeof(IBizDomainBll)) as IBizDomainBll;
            //IList<ItemValue> items = standChoiceBll.GetBizDomainList(ConfigManager.AgencyCode, stdCatName, true);
            IPolicyBLL policyBll = ObjectFactory.GetObject(typeof(IPolicyBLL)) as IPolicyBLL;
            IList<ItemValue> items = policyBll.GetPolicyList(stdCatName, moduleName);

            //SortIList(items);
            ((List<ItemValue>)items).Sort(ItemValueComparer.Instance);

            return items;
        }

        /// <summary>
        /// get contact type text by contact type value of License.
        /// </summary>
        /// <param name="selectedValue">dropdown select value</param>
        /// <returns>text value</returns>
        public static string GetTypeFlagTextByValue(string selectedValue)
        {
            string selectedText = string.Empty;

            if (ContactType4License.Individual.ToString().Equals(selectedValue, StringComparison.InvariantCultureIgnoreCase))
            {
                selectedText = BasePage.GetStaticTextByKey("license_contacttype_individual");
            }

            if (ContactType4License.Organization.ToString().Equals(selectedValue, StringComparison.InvariantCultureIgnoreCase))
            {
                selectedText = BasePage.GetStaticTextByKey("license_contacttype_organization");
            }

            return selectedText;
        }

        /// <summary>
        /// Set the selected item to first item.
        /// </summary>
        /// <param name="ddl"> dropdown list control</param>
        public static void SetSelectedToFirstItem(AccelaDropDownList ddl)
        {
            if (ddl.Items.Count > 0)
            {
                ddl.SelectedIndex = 0;
            }
        }

        /// <summary>
        /// Set a value to a AccelaDropDownList ignoring the case
        /// </summary>
        /// <param name="ddl"> dropdown list control</param>
        /// <param name="selectedValue">the value set to AccelaDropDownList control as selected.</param>
        public static void SetSelectedValue(AccelaDropDownList ddl, string selectedValue)
        {
            if (ddl.Items.Count == 0)
            {
                return;
            }

            bool isFound = false;

            if (selectedValue == null)
            {
                selectedValue = string.Empty;
            }

            foreach (ListItem item in ddl.Items)
            {
                if (item.Value.Equals(selectedValue, StringComparison.InvariantCultureIgnoreCase) ||
                    item.Text.Equals(selectedValue, StringComparison.InvariantCultureIgnoreCase))
                {
                    ddl.SelectedValue = item.Value;
                    isFound = true;
                    break;
                }
            }

            if (!isFound)
            {
                ddl.SelectedIndex = -1;
            }
        }

        /// <summary>
        /// Sets the country selected value.
        /// </summary>
        /// <param name="ddl">the country dropdownlist control.</param>
        /// <param name="countryCode">State of the CTL.</param>
        /// <param name="getDefault">get Default.</param>
        /// <param name="applySetting">if set to <c>true</c> [apply setting].</param>
        /// <param name="isIgnoreValidate">if set to <c>true</c> [is ignore validate].</param>
        public static void SetCountrySelectedValue(
            AccelaCountryDropDownList ddl, 
            string countryCode, 
            bool getDefault, 
            bool applySetting, 
            bool isIgnoreValidate)
        {
            string oldCountry = ControlUtil.GetControlValue(ddl);
            SetSelectedValue(ddl, countryCode);

            if (applySetting || (ddl.SelectedItem != null && !oldCountry.Equals(countryCode, StringComparison.InvariantCultureIgnoreCase)))
            {
                ControlUtil.ApplyRegionalSettingByCountry(false, isIgnoreValidate, getDefault, ddl, null, true);
            }
        }

        /// <summary>
        /// Set a value to a AccelaRadioButtonList ignoring the case
        /// </summary>
        /// <param name="rbl"> Radio Button list control</param>
        /// <param name="selectedValue">the value set to AccelaRadioButtonList control as selected.</param>
        public static void SetSelectedValue(AccelaRadioButtonList rbl, string selectedValue)
        {
            if (rbl.Items.Count == 0)
            {
                return;
            }

            bool isFound = false;

            if (selectedValue == null)
            {
                selectedValue = string.Empty;
            }

            foreach (ListItem item in rbl.Items)
            {
                //if (item.Value.ToUpper() == selectedValue.ToUpper())
                if (item.Value.Equals(selectedValue, StringComparison.InvariantCultureIgnoreCase))
                {
                    rbl.SelectedValue = item.Value;
                    isFound = true;
                    break;
                }
            }

            if (!isFound)
            {
                rbl.SelectedIndex = -1;
            }
        }

        /// <summary>
        /// Set a value to a AccelaRadioButtonList ignoring the case
        /// </summary>
        /// <param name="radio"> dropdown list control</param>
        /// <param name="selectedValue">the value set to AccelaDropDownList control as selected.</param>
        public static void SetSelectedValueForRadioList(AccelaRadioButtonList radio, string selectedValue)
        {
            if (radio.Items.Count == 0)
            {
                return;
            }

            bool isFound = false;

            if (selectedValue == null)
            {
                selectedValue = string.Empty;
            }

            foreach (ListItem item in radio.Items)
            {
                if (item.Value.ToUpper() ==
                    selectedValue.ToUpper())
                {
                    radio.SelectedValue = item.Value;
                    isFound = true;
                    break;
                }
            }

            if (!isFound)
            {
                radio.SelectedIndex = -1;
            }
        }
        
        /// <summary>
        /// get contact permission text by radio button value.
        /// </summary>
        /// <param name="selectedValue">radio button select value</param>
        /// <param name="moduleName">module Name</param>
        /// <returns>text value</returns>
        public static string GetContactPermissionTextByValue(string selectedValue, string moduleName)
        {
            Dictionary<string, string> permissionLabels = new Dictionary<string, string>();
            permissionLabels.Add(ContactPermission.FullAccess, LabelUtil.GetTextByKey("contact_permission_fullaccess", moduleName));
            permissionLabels.Add(ContactPermission.ReadOnly, LabelUtil.GetTextByKey("contact_permission_readonly", moduleName));
            permissionLabels.Add(ContactPermission.NoAccess, LabelUtil.GetTextByKey("contact_permission_noaccess", moduleName));
            permissionLabels.Add(ContactPermission.RenewAndAmend, LabelUtil.GetTextByKey("aca_contactpermission_label_renewandamend", moduleName));
            permissionLabels.Add(ContactPermission.ScheduleInspectionOnly, LabelUtil.GetTextByKey("aca_contactpermission_label_manageinspections", moduleName));
            permissionLabels.Add(ContactPermission.MakePayments, LabelUtil.GetTextByKey("aca_contactpermission_label_makepayments", moduleName));
            permissionLabels.Add(ContactPermission.ManageDocuments, LabelUtil.GetTextByKey("aca_contactpermission_label_managedocuments", moduleName));
            string selectedText = string.Empty;

            foreach (KeyValuePair<string, string> kvp in permissionLabels)
            {
                if (selectedValue.IndexOf(kvp.Key) > -1)
                {
                    selectedText += kvp.Value + "<br/>";
                }
            }

            if (selectedText.EndsWith("<br/>"))
            {
                selectedText = selectedText.TrimEnd("<br/>".ToCharArray());
            }

            return selectedText;        
        }

        /// <summary>
        /// Bind the largest contract Experience to AccelaDropDownList control.
        /// </summary>
        /// <param name="ddlNigpType">AccelaDropDownList control to be bind.</param>
        public static void BindNigpType(AccelaDropDownList ddlNigpType)
        {
            DropDownListDataSourceType sourceType = ddlNigpType.SourceType;
            BindStandardChoise(ddlNigpType, BizDomainConstant.STD_CERT_BUSINESS_NIGP_TYPE);

            // Restore the source type, because the NIGP type MUST not add choice in ACA admin.
            // If add choice in admin, the description value will be set as 'drop list item' cause dispaly wrong in daily side.
            ddlNigpType.SourceType = sourceType;
        }

        /// <summary>
        /// Bind the largest contract Experience to AccelaDropDownList control.
        /// </summary>
        /// <param name="ddlLargestContractExperience">AccelaDropDownList control to be bind.</param>
        public static void BindLargestContractExperience(AccelaDropDownList ddlLargestContractExperience)
        {
            IBizDomainBll standChoiceBll = ObjectFactory.GetObject(typeof(IBizDomainBll)) as IBizDomainBll;
            IList<ItemValue> stdItems = standChoiceBll.GetBizDomainList(ConfigManager.AgencyCode, BizDomainConstant.STD_CERT_BUSINESS_LARGEST_CONTRACT_EXPERIENCE, false);

            // only value of standard choice  is need to be bind to AccelaDropDownList in ACA
            List<ListItem> listItems = new List<ListItem>();
            if (stdItems != null && stdItems.Count > 0)
            {
                foreach (ItemValue item in stdItems)
                {
                    listItems.Add(new ListItem(item.Key, item.Value.ToString()));
                }
            }

            ddlLargestContractExperience.StdCategory = BizDomainConstant.STD_CERT_BUSINESS_LARGEST_CONTRACT_EXPERIENCE;
            ddlLargestContractExperience.MaxValueLength = MAX_LENGTH_NO_LIMITED;

            BindDDL(listItems, ddlLargestContractExperience);
        }

        /// <summary>
        /// Bind the largest contract value to AccelaDropDownList control.
        /// </summary>
        /// <param name="ddlLargestContractValue">AccelaDropDownList control to be bind.</param>
        public static void BindLargestContractValue(AccelaDropDownList ddlLargestContractValue)
        {
            IList<ListItem> listItem = GetLargestContactAmountItems();
            BindDDL(listItem, ddlLargestContractValue, true, false);
        }

        /// <summary>
        /// Bind the certified business certified category to AccelaCheckBoxList control.
        /// </summary>
        /// <param name="cbListCertifiedAs">AccelaCheckBoxList control to be bind.</param>
        public static void BindCertifiedCategory(AccelaCheckBoxList cbListCertifiedAs)
        {
            string displayFormat = "{0}";

            BindStandardChoiceForListControl(cbListCertifiedAs, BizDomainConstant.STD_CERT_BUSINESS_CERTIFICATION_CATEGORY, displayFormat, null);
        }

        /// <summary>
        /// Bind the certified business ethnicity to AccelaCheckBoxList control.
        /// </summary>
        /// <param name="cbListEthnicity">AccelaCheckBoxList control to be bind.</param>
        public static void BindCertifiedBusinessEthnicity(AccelaCheckBoxList cbListEthnicity)
        {
            BindStandardChoiceForListControl(cbListEthnicity, BizDomainConstant.STD_CERT_BUSINESS_ETHNICITY);
        }

        /// <summary>
        /// Bind the record type by filter name.
        /// </summary>
        /// <param name="ddlRecordType">The record type.</param>
        /// <param name="moduleName">The moduleName</param>
        /// <param name="filterName">The cap type filter name.</param>
        public static void BindRecordTypeByFilterName(AccelaDropDownList ddlRecordType, string moduleName, string filterName)
        {
            if (string.IsNullOrEmpty(moduleName) || string.IsNullOrEmpty(filterName))
            {
                return;
            }

            ICapTypeBll capTypeBll = ObjectFactory.GetObject<ICapTypeBll>();
            CapTypeModel[] capTypes = capTypeBll.GetGeneralCapTypeList(moduleName, filterName, ACAConstant.VCH_TYPE_VHAPP, AppSession.User.PublicUserId);

            if (capTypes == null || capTypes.Length == 0)
            {
                return;
            }

            IList<ListItem> listItems = new List<ListItem>();

            foreach (CapTypeModel model in capTypes)
            {
                ListItem item = new ListItem();
                item.Text = CAPHelper.GetAliasOrCapTypeLabel(model);
                item.Value = CAPHelper.GetCapTypeValue(model);

                listItems.Add(item);
            }

            BindDDL(listItems, ddlRecordType);
        }

        /// <summary>
        /// Bind Asset Group.
        /// </summary>
        /// <param name="ddlAssetGroup">asset group dropdownlist</param>
        public static void BindAssetGroup(AccelaDropDownList ddlAssetGroup)
        {
            BindStandardChoise(ddlAssetGroup, BizDomainConstant.STD_CAT_ASSET_GROUP, StandardChoiceMaxLength.MAX_LENGTH_ASSET_GROUP);
        }

        /// <summary>
        /// Bind Asset Group by record type relationship asset type
        /// </summary>
        /// <param name="ddlControl">the drop down list</param>
        /// <param name="assetTypeCapTypeModels">list of asset type cap type</param>
        public static void BindAssetGroup(AccelaDropDownList ddlControl, List<XAssetTypeCapTypeModel> assetTypeCapTypeModels)
        {
            List<ListItem> listItems = new List<ListItem>();

            if (assetTypeCapTypeModels != null && assetTypeCapTypeModels.Count > 0)
            {
                foreach (XAssetTypeCapTypeModel assetTypeCapTypeModel in assetTypeCapTypeModels)
                {
                    listItems.Add(new ListItem(assetTypeCapTypeModel.assetGroup, assetTypeCapTypeModel.assetGroup));
                }
            }

            ddlControl.SourceType = DropDownListDataSourceType.StandardChoice;
            ddlControl.StdCategory = BizDomainConstant.STD_CAT_ASSET_GROUP;
            ddlControl.MaxValueLength = StandardChoiceMaxLength.MAX_LENGTH_ASSET_GROUP;

            BindDDL(listItems.Distinct().ToList(), ddlControl);
        }

        /// <summary>
        /// Bind the contact list by cap model
        /// </summary>
        /// <param name="ddlContactList">The contact list control.</param>
        /// <param name="capModel">The cap model.</param>
        public static void BindContactListByCapModel(AccelaDropDownList ddlContactList, CapModel4WS capModel)
        {
            if (capModel == null)
            {
                return;
            }

            IPeopleBll peopleBll = ObjectFactory.GetObject<IPeopleBll>();
            List<ListItem> listItems = new List<ListItem>();

            if (capModel.contactsGroup != null)
            {
                foreach (CapContactModel4WS item in capModel.contactsGroup)
                {
                    if (item.people != null 
                        && !string.IsNullOrEmpty(item.refContactNumber) 
                        && listItems.All(f => f.Value != item.refContactNumber)
                        && EnumUtil<ContactType4License>.Parse(item.people.contactTypeFlag, ContactType4License.Individual) == ContactType4License.Individual)
                    {
                        listItems.Add(new ListItem(peopleBll.GetContactUserName(item.people), item.refContactNumber));
                    }
                }
            }

            BindDDL(listItems, ddlContactList, false, false);
        }

        /// <summary>
        /// Bind the data filter list to AccelaDropDownList control.
        /// </summary>
        /// <param name="ddlDataFilter">The DataFilter dropdownlist</param>
        /// <param name="dataFilterModels">Data filter models</param>
        /// <param name="defaultDataFilterId">Default data filter id</param>
        public static void BindDataFilter(AccelaDropDownList ddlDataFilter, XDataFilterModel[] dataFilterModels, long defaultDataFilterId)
        {
            ddlDataFilter.Items.Clear();
            ddlDataFilter.Visible = false;

            if (dataFilterModels != null && dataFilterModels.Length > 0)
            {
                IList<ListItem> datafilterList = new List<ListItem>();

                foreach (XDataFilterModel dataFilter in dataFilterModels)
                {
                    datafilterList.Add(new ListItem(dataFilter.datafilterName, dataFilter.datafilterId.ToString()));

                    // If there is no default data filter, ACA will show the primary data filter
                    if (defaultDataFilterId == 0 && ValidationUtil.IsYes(dataFilter.primary))
                    {
                        defaultDataFilterId = dataFilter.datafilterId;
                    }
                }

                ((List<ListItem>)datafilterList).Sort(ListItemComparer.Instance);

                BindDDL(datafilterList, ddlDataFilter);
                ddlDataFilter.Visible = true;
                ddlDataFilter.SelectedValue = defaultDataFilterId.ToString();
            }
        }

        /// <summary>
        /// Bind the specific standard choice items to AccelaDropDownList control by standard choice category name.
        /// </summary>
        /// <param name="ddlControl">AccelaDropDownList control.</param>
        /// <param name="stdCatName">standard choice category name which can be got from ACAConstant class.</param>
        /// <param name="maxLength">limit max length, don't control if value less than 0</param>
        private static void BindStandardChoise(AccelaDropDownList ddlControl, string stdCatName, int maxLength)
        {
            IBizDomainBll standChoiceBll = ObjectFactory.GetObject(typeof(IBizDomainBll)) as IBizDomainBll;
            IList<ItemValue> stdItems = standChoiceBll.GetBizDomainList(ConfigManager.AgencyCode, stdCatName, false, (int)ddlControl.ShowType);

            // only value of standard choice  is need to be bind to AccelaDropDownList in ACA
            List<ListItem> listItems = new List<ListItem>();
            if (stdItems != null &&
                stdItems.Count > 0)
            {
                foreach (ItemValue item in stdItems)
                {
                    string tempValue;

                    //if used by admin, the value of standard choice can't be intercepted.
                    if (!AppSession.IsAdmin &&
                        (maxLength > 0 && !string.IsNullOrEmpty(item.Key) && item.Key.Length > maxLength))
                    {
                        tempValue = item.Key.Substring(0, maxLength);
                    }
                    else
                    {
                        tempValue = item.Key;
                    }

                    listItems.Add(new ListItem(item.Value.ToString(), tempValue));
                }
            }

            ddlControl.SourceType = DropDownListDataSourceType.StandardChoice;
            ddlControl.StdCategory = stdCatName;
            ddlControl.MaxValueLength = maxLength;

            BindDDL(listItems, ddlControl);
        }

        /// <summary>
        /// Bind All standard choice contact type items to AccelaDropDownList control.
        /// </summary>
        /// <param name="ddlContactType">the contact type dropdownlist.</param>
        private static void BindContactTypesInSTD(AccelaDropDownList ddlContactType)
        {
            if (ddlContactType == null)
            {
                return;
            }

            List<ListItem> contactTypes = new List<ListItem>();

            // 1.Get all contact type from standard choice
            IBizDomainBll standChoiceBll = ObjectFactory.GetObject(typeof(IBizDomainBll)) as IBizDomainBll;
            IList<ItemValue> stdItems = standChoiceBll.GetContactTypeList(ConfigManager.AgencyCode, false, ContactTypeSource.Transaction);

            // 3. get the contact type list that need to be filtered from standard choice
            foreach (ItemValue itemValue in stdItems)
            {
                //if xpolicy models is null. it will get item from stander choice.
                if (itemValue == null)
                {
                    continue;
                }

                ListItem listItem = new ListItem(itemValue.Value.ToString(), itemValue.Key);
                contactTypes.Add(listItem);
            }

            BindDDL(contactTypes, ddlContactType);
        }

        /// <summary>
        /// Bind the specific standard choice items to ListControl control by standard choice category name.
        /// </summary>
        /// <param name="listControl">list control and its subclass, such as: AccelaRadioButtonList, AccelaCheckBoxList, AccelaListBox.</param>
        /// <param name="stdCatName">standard choice category name which can be got from BizDomainConstant class.</param>
        private static void BindStandardChoiceForListControl(ListControl listControl, string stdCatName)
        {
            BindStandardChoiceForListControl(listControl, stdCatName, null, null);
        }

        /// <summary>
        /// Bind the specific standard choice items to ListControl control by standard choice category name.
        /// </summary>
        /// <param name="listControl">list control and its subclass, such as: AccelaRadioButtonList, AccelaCheckBoxList, AccelaListBox.</param>
        /// <param name="stdCatName">standard choice category name which can be got from BizDomainConstant class.</param>
        /// <param name="displayFieldFormat">The display field format, for example: "{0} - {1}".</param>
        private static void BindStandardChoiceForListControl(ListControl listControl, string stdCatName, string displayFieldFormat)
        {
            BindStandardChoiceForListControl(listControl, stdCatName, displayFieldFormat, null);
        }

        /// <summary>
        /// Bind the specific standard choice items to ListControl control by standard choice category name.
        /// </summary>
        /// <param name="listControl">list control and its subclass, such as: AccelaRadioButtonList, AccelaCheckBoxList, AccelaListBox.</param>
        /// <param name="stdCatName">standard choice category name which can be got from BizDomainConstant class.</param>
        /// <param name="displayFieldFormat">The display field format, for example: "{0} - {1}".</param>
        /// <param name="comparisonForSort">The comparison delegate for sort.</param>
        private static void BindStandardChoiceForListControl(ListControl listControl, string stdCatName, string displayFieldFormat, Comparison<ItemValue> comparisonForSort)
        {
            IBizDomainBll standChoiceBll = ObjectFactory.GetObject(typeof(IBizDomainBll)) as IBizDomainBll;
            IList<ItemValue> stdItems = standChoiceBll.GetBizDomainList(ConfigManager.AgencyCode, stdCatName, false);

            listControl.Items.Clear();

            if (stdItems != null && stdItems.Count > 0)
            {
                if (comparisonForSort != null)
                {
                    ((List<ItemValue>)stdItems).Sort(comparisonForSort);
                }

                foreach (ItemValue item in stdItems)
                {
                    ListItem li = new ListItem();
                    li.Value = item.Key;

                    if (!string.IsNullOrEmpty(displayFieldFormat))
                    {
                        li.Text = string.Format(displayFieldFormat, item.Key, item.Value);
                    }
                    else
                    {
                        li.Text = item.Value.ToString();
                    }

                    listControl.Items.Add(li);
                }
            }
        }

        /// <summary>
        /// get agency list.
        /// In super agency, get all sub agencies and super agency.
        /// In normal agency, get normal agency.
        /// </summary>
        /// <returns>agency list</returns>
        private static IList<ListItem> GetAgencyList()
        {
            string agencyCode = ConfigManager.AgencyCode;
            IList<ListItem> list = new List<ListItem>();

            //for super agency, get all sub agencies, super agency and default item.
            if (AppSession.IsAdmin ||
                StandardChoiceUtil.IsSuperAgency())
            {
                IServiceProviderBll serviceProviderBll = (IServiceProviderBll)ObjectFactory.GetObject(typeof(IServiceProviderBll));

                //return subagencies include agency code from web configurer.
                string[] subAgencies = serviceProviderBll.GetSubAgencies(AppSession.User.PublicUserId);

                if (subAgencies != null &&
                    subAgencies.Length > 0)
                {
                    foreach (string subAgency in subAgencies)
                    {
                        if (string.IsNullOrEmpty(subAgency))
                        {
                            continue;
                        }

                        list.Add(new ListItem(subAgency, subAgency));
                    }
                }
            }
            else
            {
                //for normal agency, only get normal agency and default item.
                list.Add(new ListItem(agencyCode, agencyCode));
            }

            return list;
        }

        /// <summary>
        /// Gets all module keys.
        /// </summary>
        /// <param name="modules">The modules.</param>
        /// <returns>all module keys</returns>
        private static string GetAllModuleKeys(IList<ListItem> modules)
        {
            StringBuilder builder = new StringBuilder();
            if (modules != null)
            {
                foreach (ListItem item in modules)
                {
                    if (builder.Length > 0)
                    {
                        builder.Append(",");
                    }

                    builder.Append(item.Value);
                }
            }

            return builder.ToString();
        }

        /// <summary>
        /// Get contactType items
        /// </summary>
        /// <param name="moduleName">module name</param>
        /// <param name="contactTypeSource">the contact type source.</param>
        /// <returns>contact type items.</returns>
        private static List<ListItem> GetContactTypeItems(string moduleName, string contactTypeSource)
        {
            List<ListItem> contactTypes = new List<ListItem>();

            // 1.Get all contact type from standard choice
            IBizDomainBll standChoiceBll = ObjectFactory.GetObject(typeof(IBizDomainBll)) as IBizDomainBll;
            IList<ItemValue> stdItems = standChoiceBll.GetContactTypeList(ConfigManager.AgencyCode, false, contactTypeSource);

            // 2. Get Xpolicy items for contact type permission.
            IXPolicyBll xpolicyBll = ObjectFactory.GetObject(typeof(IXPolicyBll)) as IXPolicyBll;
            List<XPolicyModel> xPolicyModels = null;

            //When standchoice "ENABLE_CONTACT_TYPE_FILTERING_BY_MODULE" is "NO", PageFlow level contact types should ignore module level contact type settings.
            if (AppSession.IsAdmin || StandardChoiceUtil.IsEnableContactTypeFilteringByModule())
            {
                xPolicyModels = xpolicyBll.GetPolicyListByPolicyName(XPolicyConstant.CONTACT_TYPE_RESTRICTION_BY_MODULE, ConfigManager.AgencyCode);
            }

            if (xPolicyModels != null && xPolicyModels.Count != 0)
            {
                xPolicyModels = xPolicyModels.Where(p => p.level.Equals(ACAConstant.LEVEL_TYPE_MODULE, StringComparison.OrdinalIgnoreCase)
                    && p.levelData == moduleName && p.data2.Equals(ACAConstant.RECORD_CONTACT_TYPE, StringComparison.OrdinalIgnoreCase)).ToList();
            }

            // 3. get the contact type list that need to be filtered from standard choice
            foreach (ItemValue itemValue in stdItems)
            {
                //if xpolicy models is null. it will get item from stander choice.
                if (itemValue == null)
                {
                    continue;
                }

                ListItem listItem = new ListItem(itemValue.Value.ToString(), itemValue.Key);
                contactTypes.Add(listItem);

                //Get permission value from xpolicy.
                if (xPolicyModels == null)
                {
                    continue;
                }

                foreach (XPolicyModel xPolicyModel in xPolicyModels)
                {
                    //xPolicyModel.data4: contact type item key.
                    if (xPolicyModel == null || xPolicyModel.data1 == null || xPolicyModel.data1 != itemValue.Key)
                    {
                        continue;
                    }

                    //it can find Xpolicy's key by standChoice value;  xpolicy.data2: Y/N;   Y:Enable, N:Disable.
                    if (xPolicyModel.rightGranted.Equals(ACAConstant.COMMON_N, StringComparison.OrdinalIgnoreCase))
                    {
                        if (AppSession.IsAdmin)
                        {
                            // key "-", not checked check box.
                            listItem.Value = ACAConstant.SPLIT_CHAR4 + ACAConstant.SPLIT_DOUBLE_VERTICAL + listItem.Value;
                        }
                        else
                        {
                            contactTypes.Remove(listItem);
                        }
                    }

                    //it has find the xpolicy key and do filtered.
                    break;
                }
            }

            return contactTypes;
        }

        /// <summary>
        /// Get Page flow level contact type UI model.
        /// </summary>
        /// <param name="xEntities">the XEntity models</param>
        /// <param name="stdItemValue">the standard choice item value</param>
        /// <returns>Contact Type UI Model</returns>
        private static ContactTypeUIModel GetPageFlowLevelContactTypeUIModel(IEnumerable<XEntityPermissionModel> xEntities, ItemValue stdItemValue)
        {
            string key = stdItemValue.Key;
            string text = stdItemValue.Value.ToString();
            bool isChecked = true;
            string minNum = string.Empty;
            string maxNum = string.Empty;

            if (xEntities != null)
            {
                IEnumerable<XEntityPermissionModel> pageFlowEntitys = xEntities.Where(x => x.entityId3 == stdItemValue.Key);

                if (pageFlowEntitys.Any())
                {
                    XEntityPermissionModel pageFlowLevelContactType = pageFlowEntitys.FirstOrDefault();
                    isChecked = ValidationUtil.IsYes(pageFlowLevelContactType.permissionValue);
                    minNum = string.IsNullOrEmpty(pageFlowLevelContactType.data1) ? string.Empty : pageFlowLevelContactType.data1;
                    maxNum = string.IsNullOrEmpty(pageFlowLevelContactType.data2) ? string.Empty : pageFlowLevelContactType.data2;
                }
            }

            return ConstructContactTypeUIModel(key, text, isChecked, minNum, maxNum);
        }

        /// <summary>
        /// Get education type items
        /// </summary>
        /// <returns>IList item value</returns>
        private static List<ListItem> GetEducationTypeItems()
        {
            // 1. Get Xpolicy items for education type.
            IXPolicyBll xpolicyBll = ObjectFactory.GetObject(typeof(IXPolicyBll)) as IXPolicyBll;
            List<XPolicyModel> xPolicyModels = xpolicyBll.GetSearchTypeItems();
            List<ListItem> educationTypeItems = new List<ListItem>();
            bool isAllDisable = true;

            if (xPolicyModels == null || xPolicyModels.Count == 0)
            {
                return educationTypeItems;
            }

            //find all xpolicys whether is disable?
            foreach (XPolicyModel xPolicyModel in xPolicyModels)
            {
                //if xpolicy.status is "A".
                if (xPolicyModel != null && xPolicyModel.status == ACAConstant.VALID_STATUS)
                {
                    isAllDisable = false;
                    break;
                }
            }

            //Get items form xpolicys.
            foreach (XPolicyModel xPolicyModel in xPolicyModels)
            {
                if (xPolicyModel == null)
                {
                    continue;
                }

                ListItem listItem = new ListItem(I18nStringUtil.GetString(xPolicyModel.dispData2, xPolicyModel.data2), xPolicyModel.data4);

                if (AppSession.IsAdmin)
                {
                    //build item format " -1SearchforProvider || SearchforProvider".
                    string activeFlag = xPolicyModel.status == ACAConstant.INVALID_STATUS ? "-" : string.Empty;
                    listItem.Value = activeFlag + xPolicyModel.data4 + "||" + xPolicyModel.data2;
                    educationTypeItems.Add(listItem);
                }
                else
                {
                    //if current xpolicy is disable and all xpolicys isn't disable, it needn't add this.xpolicy.
                    if (isAllDisable || xPolicyModel.status == ACAConstant.VALID_STATUS)
                    {
                        educationTypeItems.Add(listItem);
                    }
                }
            }

            return educationTypeItems;
        }

        /// <summary>
        /// Get gender items.
        /// </summary>
        /// <returns>gender items</returns>
        private static List<ListItem> GetGenderItems()
        {
            IBizDomainBll standChoiceBll = ObjectFactory.GetObject(typeof(IBizDomainBll)) as IBizDomainBll;
            IList<ItemValue> stdItems = standChoiceBll.GetBizDomainList(ConfigManager.AgencyCode, BizDomainConstant.STD_CAT_GENDER, false);

            List<ListItem> listItems = new List<ListItem>();

            if (stdItems != null && stdItems.Count > 0)
            {
                foreach (ItemValue item in stdItems)
                {
                    if (item.Key.Trim().Length > 1)
                    {
                        continue;
                    }

                    listItems.Add(new ListItem(item.Value.ToString(), item.Key.Trim()));
                }
            }

            return listItems;
        }

        /// <summary>
        /// Construct contact type UI model.
        /// </summary>
        /// <param name="key">the key word.</param>
        /// <param name="text">the text value.</param>
        /// <param name="isChecked">is checked or not.</param>
        /// <param name="minNum">the min number</param>
        /// <param name="maxNum">the max number</param>
        /// <returns>the construct contact type UI model</returns>
        private static ContactTypeUIModel ConstructContactTypeUIModel(string key, string text, bool isChecked, string minNum, string maxNum)
        {
            ContactTypeUIModel contactTypeUIModel = new ContactTypeUIModel();
            contactTypeUIModel.Key = key;
            contactTypeUIModel.Text = text;
            contactTypeUIModel.Checked = isChecked;
            contactTypeUIModel.MinNum = minNum;
            contactTypeUIModel.MaxNum = maxNum;

            return contactTypeUIModel;
        }

        /// <summary>
        /// Get the largest contact amount items.
        /// </summary>
        /// <returns>the largest contact amount items.</returns>
        private static List<ListItem> GetLargestContactAmountItems()
        {
            IBizDomainBll standChoiceBll = ObjectFactory.GetObject(typeof(IBizDomainBll)) as IBizDomainBll;
            IList<ItemValue> stdItems = standChoiceBll.GetBizDomainList(ConfigManager.AgencyCode, BizDomainConstant.STD_CERT_BUSINESS_LARGEST_CONTRACT_AMOUNT, false);

            // set the sort rule
            Comparison<ItemValue> comparisonForSort = new Comparison<ItemValue>(
                delegate(ItemValue item1, ItemValue item2)
                {
                    long value1 = 0;
                    long value2 = 0;

                    long.TryParse(item1.Key.Replace(",", string.Empty), out value1);
                    long.TryParse(item2.Key.Replace(",", string.Empty), out value2);

                    return (int)(value1 - value2);
                });

            ((List<ItemValue>)stdItems).Sort(comparisonForSort);

            // add list item
            List<ListItem> listItems = new List<ListItem>();

            if (stdItems != null && stdItems.Count > 0)
            {
                foreach (ItemValue item in stdItems)
                {
                    listItems.Add(new ListItem(item.Key.Trim(), item.Key.Replace(",", string.Empty).Trim()));
                }
            }

            return listItems;
        }

        /// <summary>
        /// Gets the modules.
        /// </summary>
        /// <returns>module list</returns>
        private static IList<ListItem> GetModules()
        {
            IList<ListItem> results = new List<ListItem>();
            Dictionary<string, string> allModules = TabUtil.GetAllEnableModules(false);

            if (allModules != null)
            {
                foreach (KeyValuePair<string, string> kvp in allModules)
                {
                    results.Add(new ListItem(HttpUtility.HtmlDecode(kvp.Value), kvp.Key));
                }
            }

            return results;
        }

        #endregion Methods
    }
}