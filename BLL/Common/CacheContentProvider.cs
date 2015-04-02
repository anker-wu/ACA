#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: CacheContentProvider.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2008-2014
 *
 *  Description:
 *  Cache content provider, it assists the CahceManager to work.
 *
 *  Notes:
 * $Id: CacheContentProvider.cs 277237 2014-08-13 01:17:52Z ACHIEVO\james.shi $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using Accela.ACA.Common;
using Accela.ACA.Common.Util;
using Accela.ACA.Web.Common;
using Accela.ACA.WSProxy;
using Accela.ACA.WSProxy.WSModel;

namespace Accela.ACA.BLL.Common
{
    /// <summary>
    /// Cache content provider, it assists the CacheManager to work.
    /// </summary>
    internal static class CacheContentProvider
    {
        #region Fields

        /// <summary>
        /// the public user id.
        /// </summary>
        private const string CALLER_ID = "ACA System";

        /// <summary>
        /// label category name.
        /// </summary>
        private const string LABEL_CATEGORY_NAME = "ACA Admin";

        /// <summary>
        /// template genus APO.
        /// </summary>
        private const string TEMPLATE_GENUS_APO = "APO";

        /// <summary>
        /// template genus people.
        /// </summary>
        private const string TEMPLATE_GENUS_PEOPLE = "People";

        /// <summary>
        /// the admin suffix.
        /// </summary>
        private const string ADMIN_SUFFIX = "_Admin";

        /// <summary>
        /// the daily suffix.
        /// </summary>
        private const string DAILY_SUFFIX = "_Daily";

        /// <summary>
        /// categories for  master lang description.
        /// </summary>
        private static Hashtable _categories4MasterLangDesc;

        /// <summary>
        /// categories for multi lang all.
        /// </summary>
        private static Hashtable _categories4MultiLangAll;

        /// <summary>
        /// categories for multi lang all with Master Lang Key.
        /// </summary>
        private static Hashtable _categories4MultiLangAllWithMasterLangKey;

        /// <summary>
        /// categories for multi lang description..
        /// </summary>
        private static Hashtable _categories4MultiLangDesc;

        /// <summary>
        /// categories for multi lang value.
        /// </summary>
        private static Hashtable _categories4MultiLangValue;

        #endregion Fields

        #region Properties

        /// <summary>
        /// Gets an instance of AcaAdminTreeService.
        /// </summary>
        private static AcaAdminTreeWebServiceService AcaAdminTreeService
        {
            get
            {
                return WSFactory.Instance.GetWebService<AcaAdminTreeWebServiceService>();
            }
        }

        /// <summary>
        /// Gets an instance of ServiceProviderService.
        /// </summary>
        private static ServiceProviderWebServiceService ServiceProviderService
        {
            get
            {
                return WSFactory.Instance.GetWebService<ServiceProviderWebServiceService>();
            }
        }

        /// <summary>
        /// Gets an instance of CapTypePermissionService.
        /// </summary>
        private static CapTypePermissionWebServiceService CapTypePermissionService
        {
            get
            {
                return WSFactory.Instance.GetWebService<CapTypePermissionWebServiceService>();
            }
        }

        /// <summary>
        /// Gets an instance of BizDomainService.
        /// </summary>
        private static BizDomainWebServiceService BizDomainService
        {
            get
            {
                return WSFactory.Instance.GetWebService<BizDomainWebServiceService>();
            }
        }

        /// <summary>
        /// Gets an instance of I18nSettingsService.
        /// </summary>
        private static I18nSettingsWebServiceService I18nSettingsService
        {
            get
            {
                return WSFactory.Instance.GetWebService<I18nSettingsWebServiceService>();
            }
        }

        /// <summary>
        /// Gets an instance of CapTypeFilterService.
        /// </summary>
        private static CapTypeFilterManagerWebServiceService CapTypeFilterService
        {
            get
            {
                return WSFactory.Instance.GetWebService<CapTypeFilterManagerWebServiceService>();
            }
        }

        /// <summary>
        /// Gets an instance of AppStatusGroupWebServiceService.
        /// </summary>
        private static AppStatusGroupWebServiceService AppStatusGroupWebservice
        {
            get
            {
                return WSFactory.Instance.GetWebService<AppStatusGroupWebServiceService>();
            }
        }

        /// <summary>
        /// Gets an instance of InspectionTypePermissionService.
        /// </summary>
        private static InspectionTypePermissionWebServiceService InspectionTypePermissionService
        {
            get
            {
                return WSFactory.Instance.GetWebService<InspectionTypePermissionWebServiceService>();
            }
        }

        /// <summary>
        /// Gets an instance of RegionalWebServiceService.
        /// </summary>
        private static RegionalWebServiceService RegionalService
        {
            get
            {
                return WSFactory.Instance.GetWebService<RegionalWebServiceService>();
            }
        }

        /// <summary>
        /// Gets standard choice category table for master language Description
        /// </summary>
        private static Hashtable Categories4MasterLangDesc
        {
            get
            {
                if (_categories4MasterLangDesc == null)
                {
                    _categories4MasterLangDesc = new Hashtable(StringComparer.InvariantCultureIgnoreCase);
                    _categories4MasterLangDesc.Add(BizDomainConstant.STD_CAT_ACA_CONFIGS, null);
                    _categories4MasterLangDesc.Add(BizDomainConstant.STD_CAT_INSPECTION_CONFIGS, null);
                    _categories4MasterLangDesc.Add(BizDomainConstant.STD_CAT_PHONE_NUMBER_IDD_ENABLE, null);
                    _categories4MasterLangDesc.Add(BizDomainConstant.STD_CAT_EPAYMENT_CONFIG, null);
                    _categories4MasterLangDesc.Add(BizDomainConstant.STD_CAT_TEMPLATE_EMSE_DROPDOWN, null);
                    _categories4MasterLangDesc.Add(BizDomainConstant.STD_CONTINUING_EDUCATION_REQUIRED_HOURS, null);
                    _categories4MasterLangDesc.Add(BizDomainConstant.STD_LICENSE_AUTO_APPROVED, null);
                    _categories4MasterLangDesc.Add(BizDomainConstant.STD_CAT_PASSWORD_POLICY_SETTINGS, null);
                    _categories4MasterLangDesc.Add(BizDomainConstant.STD_AUTO_SYNC_PEOPLE, null);
                }

                return _categories4MasterLangDesc;
            }
        }

        /// <summary>
        /// Gets standard choice category table for multi-language description
        /// </summary>
        private static Hashtable Categories4MultiLangAll
        {
            get
            {
                if (_categories4MultiLangAll == null)
                {
                    _categories4MultiLangAll = new Hashtable(StringComparer.InvariantCultureIgnoreCase);
                    _categories4MultiLangAll.Add(BizDomainConstant.STD_CAT_CAP_SEARCH_TYPE, null);
                    _categories4MultiLangAll.Add(BizDomainConstant.STD_CAT_APO_LOOKUP_TYPE, null);
                    _categories4MultiLangAll.Add(BizDomainConstant.STD_CAT_CAP_PAYMENT_TYPE, null);
                    _categories4MultiLangAll.Add(BizDomainConstant.STD_FEE_QUANTITY_ACCURACY, null);
                }

                return _categories4MultiLangAll;
            }
        }

        /// <summary>
        /// Gets standard choice category table for multi-language description
        /// </summary>
        private static Hashtable Categories4MultiLangAllWithMasterLangKey
        {
            get
            {
                if (_categories4MultiLangAllWithMasterLangKey == null)
                {
                    _categories4MultiLangAllWithMasterLangKey = new Hashtable(StringComparer.InvariantCultureIgnoreCase);
                    _categories4MultiLangAllWithMasterLangKey.Add(BizDomainConstant.STD_CAT_CONSTUCTION_TYPE, null);
                }

                return _categories4MultiLangAllWithMasterLangKey;
            }
        }

        /// <summary>
        /// Gets standard choice category table for multi-language description
        /// </summary>
        private static Hashtable Categories4MultiLangDesc
        {
            get
            {
                if (_categories4MultiLangDesc == null)
                {
                    _categories4MultiLangDesc = new Hashtable(StringComparer.InvariantCultureIgnoreCase);

                    _categories4MultiLangDesc.Add(BizDomainConstant.STD_CAT_ACA_CONFIGS_TABS, null);
                    _categories4MultiLangDesc.Add(BizDomainConstant.STD_CAT_ACA_CONFIGS_LINKS, null);
                    _categories4MultiLangDesc.Add(BizDomainConstant.STD_CAT_PARCEL_MASK, null);
                    _categories4MultiLangDesc.Add(BizDomainConstant.STD_CAT_ACA_FILTER_CAP_BY_LICENSE, null);
                    _categories4MultiLangDesc.Add(BizDomainConstant.STD_CAT_FIND_APP_DATE_RANGE, null);
                    _categories4MultiLangDesc.Add(BizDomainConstant.STD_CAT_COUNTRY, null);
                    _categories4MultiLangDesc.Add(BizDomainConstant.STD_CAT_COUNTRY_IDD, null);
                    _categories4MultiLangDesc.Add(BizDomainConstant.STD_CAT_GENDER, null);
                    _categories4MultiLangDesc.Add(BizDomainConstant.STD_CAT_SALUTATION, null);
                    _categories4MultiLangDesc.Add(BizDomainConstant.STD_CAT_CONTACT_METHODS, null);
                    _categories4MultiLangDesc.Add(BizDomainConstant.STD_EXAM_CSV_FORMAT, null);
                    _categories4MultiLangDesc.Add(BizDomainConstant.STD_CAT_CONDITION_STATUS, null);
                    _categories4MultiLangDesc.Add(BizDomainConstant.STD_CAT_CONDITIONS_OF_APPROVAL_STATUS, null);
                }

                return _categories4MultiLangDesc;
            }
        }

        /// <summary>
        /// Gets standard choice category table for multi-language value
        /// </summary>
        private static Hashtable Categories4MultiLangValue
        {
            get
            {
                if (_categories4MultiLangValue == null)
                {
                    _categories4MultiLangValue = new Hashtable(StringComparer.InvariantCultureIgnoreCase);
                    _categories4MultiLangValue.Add(BizDomainConstant.STD_CAT_STREET_DIRECTIONS, null);
                    _categories4MultiLangValue.Add(BizDomainConstant.STD_CAT_UNIT_TYPES, null);
                    _categories4MultiLangValue.Add(BizDomainConstant.STD_CAT_STATE_TYPE, null);
                    _categories4MultiLangValue.Add(BizDomainConstant.STD_CAT_LICENSE_TYPE, null);
                    _categories4MultiLangValue.Add(BizDomainConstant.STD_CAT_PAYMENT_CREDITCARD_TYPE, null);
                    _categories4MultiLangValue.Add(BizDomainConstant.STD_CAT_PAYMENT_CHECK_TYPE, null);
                    _categories4MultiLangValue.Add(BizDomainConstant.STD_CAT_APO_SUBDIVISIONS, null);
                    _categories4MultiLangValue.Add(BizDomainConstant.STD_CAT_STREET_SUFFIXES, null);
                    _categories4MultiLangValue.Add(BizDomainConstant.STD_CAT_REF_ADDRESS_TYPE, null);
                    _categories4MultiLangValue.Add(BizDomainConstant.STD_CAT_CONTACT_TYPE, null);
                    _categories4MultiLangValue.Add(BizDomainConstant.STD_CAT_DEFAULT_JOB_VALUE, null);
                    _categories4MultiLangValue.Add(BizDomainConstant.STD_CAT_SELF_PLAN_RULESET, null);
                    _categories4MultiLangValue.Add(BizDomainConstant.STD_CAT_EDUCATION_DEGREE_TYPE, null);
                    _categories4MultiLangValue.Add(BizDomainConstant.STD_PEOPLE_ATTRIBUTE_UNIT, null);
                    _categories4MultiLangValue.Add(BizDomainConstant.STD_CAT_PAYMENT_CHECK_ACCOUNT_TYPE, null);
                    _categories4MultiLangValue.Add(BizDomainConstant.STD_CAT_LICENSING_BOARD, null);
                    _categories4MultiLangValue.Add(BizDomainConstant.STD_TRANSACTION_TYPE, null);
                    _categories4MultiLangValue.Add(BizDomainConstant.STD_PAYMENT_PROCESSING_METHOD, null);
                    _categories4MultiLangValue.Add(BizDomainConstant.STD_CAT_CONTACT_ADDRESS_TYPE, null);
                    _categories4MultiLangValue.Add(BizDomainConstant.STD_CAT_RACE, null);
                    _categories4MultiLangValue.Add(BizDomainConstant.STD_CAT_AUTO_INVOICE_MODULE, null);
                    _categories4MultiLangValue.Add(BizDomainConstant.STD_CAT_ASSET_GROUP, null);
                    _categories4MultiLangValue.Add(BizDomainConstant.STD_CAT_CONDITIONS_GROUP, null);
                }

                return _categories4MultiLangValue;
            }
        }

        /// <summary>
        /// Gets an instance of EMSEService.
        /// </summary>
        private static EMSEWebServiceService EMSEService
        {
            get
            {
                return WSFactory.Instance.GetWebService<EMSEWebServiceService>();
            }
        }

        /// <summary>
        /// Gets an instance of SmartChoiceGroupService.
        /// </summary>
        private static SmartChoiceGroupWebServiceService SmartChoiceGroupService
        {
            get
            {
                return WSFactory.Instance.GetWebService<SmartChoiceGroupWebServiceService>();
            }
        }

        /// <summary>
        /// Gets an instance of GenericViewService.
        /// </summary>
        private static GenericViewWebServiceService GenericViewService
        {
            get
            {
                return WSFactory.Instance.GetWebService<GenericViewWebServiceService>();
            }
        }

        /// <summary>
        /// Gets an instance of AgencyProfService.
        /// </summary>
        private static AgencyProfWebServiceService AgencyProfService
        {
            get
            {
                return WSFactory.Instance.GetWebService<AgencyProfWebServiceService>();
            }
        }

        /// <summary>
        /// Gets an instance of PageFlowConfigService.
        /// </summary>
        private static PageFlowConfigWebServiceService PageFlowConfigService
        {
            get
            {
                return WSFactory.Instance.GetWebService<PageFlowConfigWebServiceService>();
            }
        }

        /// <summary>
        /// Gets an instance of PolicyService.
        /// </summary>
        private static PolicyWebServiceService PolicyService
        {
            get
            {
                return WSFactory.Instance.GetWebService<PolicyWebServiceService>();
            }
        }

        /// <summary>
        /// Gets an instance of ServerConstantService.
        /// </summary>
        private static ServerConstantWebServiceService ServerConstantService
        {
            get
            {
                return WSFactory.Instance.GetWebService<ServerConstantWebServiceService>();
            }
        }

        /// <summary>
        /// Gets an instance of TemplateService.
        /// </summary>
        private static TemplateWebServiceService TemplateService
        {
            get
            {
                return WSFactory.Instance.GetWebService<TemplateWebServiceService>();
            }
        }

        /// <summary>
        /// Gets an instance of XEntityPermissionService.
        /// </summary>
        private static EntityPermissionWebServiceService XEntityPermissionService
        {
            get
            {
                return WSFactory.Instance.GetWebService<EntityPermissionWebServiceService>();
            }
        }

        /// <summary>
        /// Gets an instance of EDMSDocumentWebServiceService
        /// </summary>
        private static EDMSDocumentWebServiceService EDMSDocumentService
        {
            get
            {
                return WSFactory.Instance.GetWebService<EDMSDocumentWebServiceService>();
            }
        }

        /// <summary>
        /// Gets an instance of XPolicyService.
        /// </summary>
        private static PolicyWebServiceService XPolicyService
        {
            get
            {
                return WSFactory.Instance.GetWebService<PolicyWebServiceService>();
            }
        }

        /// <summary>
        /// Gets an instance of GFilterViewService.
        /// </summary>
        private static GFilterViewWebServiceService GFilterViewService
        {
            get
            {
                return WSFactory.Instance.GetWebService<GFilterViewWebServiceService>();
            }
        }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Gets the type of all contact type.
        /// </summary>
        /// <param name="agencyCode">The agency code.</param>
        /// <returns>contact type list.</returns>
        internal static BizDomainModel4WS[] GetAllContactType(string agencyCode)
        {
            BizDomainModel4WS searchModel = new BizDomainModel4WS();
            searchModel.bizdomain = BizDomainConstant.STD_CAT_CONTACT_TYPE;
            searchModel.serviceProviderCode = agencyCode;

            return BizDomainService.getBizDomainListByModel(searchModel, "Admin User", false);
        }

        /// <summary>
        /// Build label key
        /// </summary>
        /// <param name="labels">Hashtable of label keys</param>
        /// <param name="textModelArray">XUITextModel array</param>
        internal static void BuildLabelsWithKey(Hashtable labels, XUITextModel[] textModelArray)
        {
            //put all labels and keys into hashtable
            if (textModelArray != null && textModelArray.Length > 0)
            {
                foreach (XUITextModel textItem in textModelArray)
                {
                    string labelKey = BuildLabelKey(textItem);

                    labels[labelKey] = textItem.stringValue;

                    //get xui text content value
                    if (!string.IsNullOrEmpty(textItem.textContent))
                    {
                        labels[labelKey + ACAConstant.LABEL_CONTENT_SUFFIX] = textItem.textContent;
                    }
                }
            }
        }

        /// <summary>
        /// Get all ACA web pages
        /// </summary>
        /// <param name="agencyCode">agency code</param>
        /// <returns>Hashtable that stores page id and url</returns>
        internal static Hashtable GetAllACAPages(string agencyCode)
        {
            AcaAdminTreeNodeModel4WS[] treeNodes = AcaAdminTreeService.getACAPageList(agencyCode, ACAConstant.ADMIN_CALLER_ID);
            Hashtable htPages = new Hashtable();

            if (treeNodes != null && treeNodes.Length > 0)
            {
                foreach (AcaAdminTreeNodeModel4WS node in treeNodes)
                {
                    // node.actionURL == "#": not web page, just for adimn use.
                    if (node != null && node.actionURL != ACAConstant.ADMIN_ROOT_NODE && !htPages.ContainsKey(node.elementID))
                    {
                        HandlePageUrl(node);

                        // elementID: pageID, actionUrl: page
                        htPages.Add(node.elementID, node.actionURL);
                    }
                }
            }

            return htPages;
        }
        
        /// <summary>
        /// Get all inspection action permission
        /// </summary>
        /// <param name="agencyCode">agency code.</param>
        /// <returns>hash table for inspection action permission</returns>
        internal static Hashtable GetAllInsActionPermission(string agencyCode)
        {
            //1. Get Inspection Action Permission from DB.
            InspectionActionPermissionModel[] insActionPermissionArray = InspectionTypePermissionService.getActionPermissionSettings(agencyCode);
            Hashtable htInsActionPermission = new Hashtable();

            //2. group by appStatusGroudCode, appStatus and InspectionGroup. 
            if (insActionPermissionArray != null && insActionPermissionArray.Length > 0)
            {
                foreach (InspectionActionPermissionModel insActionPermission in insActionPermissionArray)
                {
                    if (insActionPermission == null)
                    {
                        continue;
                    }

                    string appStatus = insActionPermission.appStatusGroupModel != null ? insActionPermission.appStatusGroupModel.status : string.Empty;
                    string appStatusGroup = insActionPermission.appStatusGroupModel != null ? insActionPermission.appStatusGroupModel.appStatusGroupCode : string.Empty;
                    string insGroupCode = insActionPermission.inspectionTypeModel != null ? insActionPermission.inspectionTypeModel.groupCode : string.Empty;
                    string key = string.Format("{0}{1}{2}{3}{4}", appStatusGroup, ACAConstant.SPLIT_CHAR5, appStatus, ACAConstant.SPLIT_CHAR5, insGroupCode);
                    IList<InspectionActionPermissionModel> iListinsActionPermission = null;

                    //if hashtable is not exist key. it need create a new item.
                    if (!htInsActionPermission.Contains(key))
                    {
                        iListinsActionPermission = new List<InspectionActionPermissionModel>();
                        iListinsActionPermission.Add(insActionPermission);
                        htInsActionPermission.Add(key, iListinsActionPermission);
                    }
                    else
                    {
                        iListinsActionPermission = htInsActionPermission[key] as IList<InspectionActionPermissionModel>;

                        if (iListinsActionPermission != null)
                        {
                            iListinsActionPermission.Add(insActionPermission);
                            htInsActionPermission[key] = iListinsActionPermission;
                        }
                    }
                }
            }

            return htInsActionPermission;
        }

        /// <summary>
        /// Get Service Provider model.
        /// </summary>
        /// <param name="key">the cache key.</param>
        /// <param name="agencyCode">agency code</param>
        /// <returns>Hashtable that stores service provider model.</returns>
        internal static Hashtable GetServiceProvider(string key, string agencyCode)
        {
            ServiceProviderModel serviceProviderModel = ServiceProviderService.getServiceProviderByPK(agencyCode, CALLER_ID);
            Hashtable htServiceProvider = new Hashtable();

            if (serviceProviderModel != null)
            {
                htServiceProvider.Add(key, serviceProviderModel);
            }

            return htServiceProvider;
        }

        /// <summary>
        /// Get Cap Type Entities model.
        /// </summary>
        /// <param name="agencyCode">agency code</param>
        /// <returns>Hashtable that stores service provider model.</returns>
        internal static Hashtable GetCapTypeEntities(string agencyCode)
        {
            CapTypePermissionModel capTypePermission = new CapTypePermissionModel();
            capTypePermission.serviceProviderCode = agencyCode;

            Hashtable htCapTypeEntities = new Hashtable();

            foreach (string type in Enum.GetNames(typeof(ControllerType)))
            {
                capTypePermission.controllerType = type;
                CapTypePermissionModel[] capTypePermissions = CapTypePermissionService.getAllCapTypesWithPermission(capTypePermission);

                if (capTypePermissions != null && capTypePermissions.Length > 0)
                {
                    foreach (CapTypePermissionModel model in capTypePermissions)
                    {
                        model.controllerType = type;
                    }

                    htCapTypeEntities.Add(type, capTypePermissions);
                }
            }

            return htCapTypeEntities;
        }

        /// <summary>
        /// Get Cap Type Entities model.
        /// </summary>
        /// <param name="agencyCode">agency code</param>
        /// <returns>Hashtable that stores service provider model.</returns>
        internal static Hashtable GetXEntityPermission(string agencyCode)
        {
            //Only to cache ACA items (REC_FUL_NAM = ACA Admin).
            XEntityPermissionModel xEntity = new XEntityPermissionModel();
            xEntity.servProvCode = agencyCode;
            xEntity.recFulNam = ACAConstant.ADMIN_CALLER_ID;

            var htXEntities = new Hashtable();
            var xEntities = XEntityPermissionService.getXEntityPermissions(xEntity);

            if (xEntities != null && xEntities.Length > 0)
            {
                htXEntities.Add(string.Empty, xEntities);
            }

            return htXEntities;
        }

        /// <summary>
        /// Get required for document types model.
        /// </summary>
        /// <param name="searchModel">the search model</param>
        /// <returns>required document type models.</returns>
        internal static RefRequiredDocumentModel[] GetRequiredDocumentTypes(RefRequiredDocumentModel searchModel)
        {
           return EDMSDocumentService.getRequiredDocumentList(searchModel);
        }

        /// <summary>
        /// Get all available EMSE event config for ACA.
        /// </summary>
        /// <param name="agencyCode">The agency code</param>
        /// <returns>all available EMSE event config for ACA</returns>
        internal static Hashtable GetAllEventScript(string agencyCode)
        {
            EventModel4WS[] events = EMSEService.getAllEventScript(agencyCode);
            Hashtable htEMSEEventConfig = new Hashtable();

            if (events != null && events.Length > 0)
            {
                foreach (EventModel4WS eventModel in events)
                {
                    if (eventModel != null && !htEMSEEventConfig.ContainsKey(eventModel.eventName))
                    {
                        htEMSEEventConfig.Add(eventModel.eventName, eventModel.scriptCode);
                    }
                }
            }

            return htEMSEEventConfig;
        }

        /// <summary>
        /// Get bizDomain Key value pairs
        /// </summary>
        /// <param name="agencyCode">agency code</param>
        /// <param name="category">category name</param>
        /// <param name="culture">The culture.</param>
        /// <returns>hashtable containing standard choices key and value pair</returns>
        internal static Hashtable GetBizDomainKeyValuePairs(string agencyCode, string category, string culture)
        {
            BizDomainModel4WS[] bizDominModes = BizDomainService.getBizDomainValue(agencyCode, category, BllUtil.EmptyQueryFormat, false, GetCulture(culture));

            if (bizDominModes == null || bizDominModes.Length == 0)
            {
                return null;
            }

            //Add ignore case flag to standard choice item hashtable.
            Hashtable stdItems = new Hashtable(StringComparer.InvariantCultureIgnoreCase);
            int unknownCounter = 0;

            foreach (BizDomainModel4WS bizModel in bizDominModes)
            {
                if (bizModel.auditStatus == ACAConstant.VALID_STATUS && bizModel.bizdomainValue != null && !stdItems.Contains(bizModel.bizdomainValue))
                {
                    string text = string.Empty;
                    string value = bizModel.bizdomainValue;

                    if (IsInCategories4MasterLangDesc(category, value))
                    {
                        text = bizModel.description;
                    }
                    else if (IsInCategories4MultiLangValue(category, value))
                    {
                        text = I18nStringUtil.GetString(bizModel.resBizdomainValue, bizModel.bizdomainValue);
                    }
                    else if (IsInCategories4MultiLangDesc(category, value))
                    {
                        text = I18nStringUtil.GetString(bizModel.resDescription, bizModel.description);
                    }
                    else if (IsInCategories4MultiLangAll(category, value))
                    {
                        value = I18nStringUtil.GetString(bizModel.resBizdomainValue, bizModel.bizdomainValue);
                        text = I18nStringUtil.GetString(bizModel.resDescription, bizModel.description);
                    }
                    else if (IsInCategories4MultiLangAllWithMasterLangKey(category, value))
                    {
                        text = I18nStringUtil.GetString(bizModel.resBizdomainValue, bizModel.bizdomainValue) + "-" + I18nStringUtil.GetString(bizModel.resDescription, bizModel.description);
                    }
                    else
                    {
                        text = I18nStringUtil.GetString(bizModel.resDescription, bizModel.description, bizModel.resBizdomainValue, bizModel.bizdomainValue);
                    }

                    if (string.IsNullOrEmpty(text))
                    {
                        ++unknownCounter;
                        text = string.Empty;
                    }

                    stdItems.Add(value, text);
                }
            }

            return stdItems;
        }

        /// <summary>
        /// Get all cap type filters for ACA.
        /// </summary>
        /// <param name="agencyCode">The agency code</param>
        /// <returns>all page flow group for ACA</returns>
        internal static Hashtable GetCapTypeFilters(string agencyCode)
        {
            // the "System" is only used for logging. The web service should be refactored to get all cap type filters without caller id.
            XButtonFilterModel4WS[] capTypeFilters = CapTypeFilterService.getAllRelationOnButton2Filter(agencyCode, "System");

            Hashtable htCapTypeFilters = new Hashtable();

            if (capTypeFilters == null || capTypeFilters.Length == 0)
            {
                return htCapTypeFilters;
            }

            foreach (XButtonFilterModel4WS filter in capTypeFilters)
            {
                string key = filter.moduleName + filter.controlLabelKey;

                if (!htCapTypeFilters.ContainsKey(key))
                {
                    htCapTypeFilters.Add(key, filter.filterName);
                }
            }

            return htCapTypeFilters;
        }

        /// <summary>
        /// Gets all level Labels for cache.
        /// </summary>
        /// <param name="agencyCode">agency code.</param>
        /// <param name="cultureName">culture name</param>
        /// <returns>All of labels(key-value)</returns>
        internal static Hashtable GetGuiTexts(string agencyCode, string cultureName)
        {
            GUITextModel4WS textModel4WS = new GUITextModel4WS();
            textModel4WS.categoryName = LABEL_CATEGORY_NAME;

            string languageCode = string.Empty;
            string regionalCode = string.Empty;
            I18nCultureUtil.SplitCulture(cultureName, ref languageCode, ref regionalCode);
            textModel4WS.langCode = languageCode;
            textModel4WS.language = languageCode;
            textModel4WS.country = regionalCode;
            textModel4WS.countryCode = regionalCode;

            XUITextModel[] textModelArray = GenericViewService.getXUIGUITextList(agencyCode, CALLER_ID, textModel4WS);
            Hashtable labels = new Hashtable();

            BuildLabelsWithKey(labels, textModelArray);

            return labels;
        }

        /// <summary>
        /// Gets all of standard choice which are module level from web service.
        /// </summary>
        /// <param name="agencyCode">agency code.</param>
        /// <returns>All of standard choice which are module level that is used by ACA.</returns>
        internal static Hashtable GetHardCodeDropDownLists(string agencyCode)
        {
            // Get All of standard choice category which are module level is used by ACA.
            string[] categorys = BizDomainConstant.GetACAHardCodeDropDownListCategoryNames();

            if (categorys == null || categorys.Length == 0)
            {
                return new Hashtable();
            }

            Hashtable stdCategories = new Hashtable();

            // Loop category to get all standard choice items that are module level and will be cached.
            foreach (string category in categorys)
            {
                XPolicyModel[] policyModels = PolicyService.getPolicyList(agencyCode, category, null, ACAConstant.GRANTED_RIGHT, false, false);

                // if there is no policy items for the category,move to next category
                if (policyModels == null || policyModels.Length == 0)
                {
                    continue;
                }

                Hashtable stdItems = GetHardCodePolicyModels(policyModels, agencyCode);

                if (!stdCategories.Contains(category))
                {
                    stdCategories.Add(category, stdItems);
                }
            }

            return stdCategories;
        }

        /// <summary>
        /// Gets all hard code dropdownlist items from policy models.
        /// </summary>
        /// <param name="policyModels">XPolicyModel array.</param>
        /// <param name="agencyCode">agency code</param>
        /// <returns>Hashtable contains all hard code drop down list items.</returns>
        internal static Hashtable GetHardCodePolicyModels(XPolicyModel[] policyModels, string agencyCode)
        {
            Hashtable stdItems = new Hashtable();

            foreach (XPolicyModel policyModel in policyModels)
            {
                if (policyModel.data2 != null)
                {
                    // global
                    string value = I18nStringUtil.GetString(policyModel.dispData2, policyModel.data2);
                    
                    //read module level datas, add a flag to remark it.
                    if (policyModel.level == ACAConstant.LEVEL_TYPE_MODULE)
                    {
                        value = ACAConstant.LEVEL_TYPE_MODULE + ":" + policyModel.levelData + "_" + value;
                    }
                    else if (policyModel.serviceProviderCode == agencyCode) 
                    {
                        // read agecy level datas
                        value = policyModel.serviceProviderCode + "_" + value;
                    }

                    stdItems[value] = policyModel;
                }
            }

            return stdItems;
        }

        /// <summary>
        /// Get all agency logos for ACA.
        /// </summary>
        /// <param name="agencyCode">The agency code</param>
        /// <returns>a hashtable which contains all logos of all agencies</returns>
        internal static Hashtable GetLogos(string agencyCode)
        {
            // the "System" is only used for logging. The web service should be refactored to get all cap type filters without caller id.
            // the "ACA" means this method get policys for ACA.
            LogoModel[] logos = AgencyProfService.getAllAgencyLogos(agencyCode);

            Hashtable htLogos = new Hashtable();

            if (logos == null || logos.Length == 0)
            {
                return htLogos;
            }

            foreach (LogoModel logo in logos)
            {
                if (!htLogos.ContainsKey(logo.serviceProviderCode))
                {
                    htLogos.Add(logo.serviceProviderCode, logo);
                }
            }

            return htLogos;
        }

        /// <summary>
        /// Gets the common logos.
        /// notes: only return empty Hashtable for lazy updates (get one cache one).
        /// </summary>
        /// <param name="agencyCode">The agency code.</param>
        /// <returns>the common logos.</returns>
        internal static Hashtable GetCommonLogos(string agencyCode)
        {
            Hashtable htLogos = new Hashtable();

            return htLogos;
        }

        /// <summary>
        /// Get all page flow group for ACA.
        /// </summary>
        /// <param name="agencyCode">The agency code</param>
        /// <returns>all page flow group for ACA</returns>
        internal static Hashtable GetPageflowGroups(string agencyCode)
        {
            // the "System" is only used for logging. The web service should be refactored to get all page flow groups without caller id.
            PageFlowGroupModel[] pageflowGroups = PageFlowConfigService.getAllPageFlowGroup(agencyCode, "System");

            Hashtable pfGroups = new Hashtable();

            if (pageflowGroups == null || pageflowGroups.Length == 0)
            {
                return pfGroups;
            }

            foreach (PageFlowGroupModel pageflow in pageflowGroups)
            {
                if (!pfGroups.ContainsKey(pageflow.pageFlowGrpCode))
                {
                    pfGroups.Add(pageflow.pageFlowGrpCode, pageflow);
                }
            }

            return pfGroups;
        }

        /// <summary>
        /// Gets ASI security for ACA
        /// </summary>
        /// <param name="asiSecurityQueryParam">query parameters</param>
        /// <returns>ASI security hash table</returns>
        internal static Hashtable GetASISecurities(ASISecurityQueryParam4WS asiSecurityQueryParam)
        {
            MapEntry4WS[] asiSecurityModels = null;

            if (asiSecurityQueryParam != null && asiSecurityQueryParam.asiSecurityModel != null && asiSecurityQueryParam.asiSecurityModel.XPolicy != null
                && !string.IsNullOrEmpty(asiSecurityQueryParam.agency) && !string.IsNullOrEmpty(asiSecurityQueryParam.userType)
                && !string.IsNullOrEmpty(asiSecurityQueryParam.asiSecurityModel.XPolicy.data1) && !string.IsNullOrEmpty(asiSecurityQueryParam.asiSecurityModel.XPolicy.data5))
            {
                asiSecurityModels = XPolicyService.getASISecurityMap(asiSecurityQueryParam);
            }
            
            Hashtable asiSecurities = new Hashtable();

            if (asiSecurityModels == null || asiSecurityModels.Length == 0)
            {
                return asiSecurities;
            }

            foreach (MapEntry4WS asiSecurity in asiSecurityModels)
            {
                string fieldSecurityKey = HttpUtility.UrlDecode(asiSecurity.key);

                if (!asiSecurities.ContainsKey(fieldSecurityKey))
                {
                    asiSecurities.Add(fieldSecurityKey, asiSecurity.value.ToString());
                }
                else
                {
                    asiSecurities[fieldSecurityKey] = asiSecurity.value.ToString();
                }
            }

            return asiSecurities;
        }

        /// <summary>
        /// Get all policy for ACA.
        /// </summary>
        /// <param name="agencyCode">The agency code</param>
        /// <param name="culture">The culture.</param>
        /// <returns>all page flow group for ACA</returns>
        internal static Hashtable GetPolicys(string agencyCode, string culture)
        {
            // the "System" is only used for logging. The web service should be refactored to get all cap type filters without caller id.
            // the "ACA" means this method get policys for ACA.
            XpolicyUserRolePrivilegeModel[] policys = XPolicyService.getXpolicyUserRoleList(agencyCode, "ACA", "System", false, GetCulture(culture));

            Hashtable htPolicys = new Hashtable();

            if (policys == null || policys.Length == 0)
            {
                return htPolicys;
            }

            ArrayList groupedXPolicy;

            foreach (XpolicyUserRolePrivilegeModel policy in policys)
            {
                string key = policy.serviceProviderCode + policy.policyName;

                if (!htPolicys.ContainsKey(key))
                {
                    groupedXPolicy = new ArrayList();
                    groupedXPolicy.Add(policy);
                    htPolicys.Add(key, groupedXPolicy);
                }
                else
                {
                    (htPolicys[key] as ArrayList).Add(policy);
                }
            }

            return htPolicys;
        }

        /// <summary>
        /// Get ServerConstant value by constant name
        /// </summary>
        /// <param name="agencyCode">agency code.</param>
        /// <param name="constantName">constant name.</param>
        /// <returns>constant value.</returns>
        internal static string GetServerConstantValue(string agencyCode, string constantName)
        {
            ServerConstantModel4WS[] constants = ServerConstantService.getServerConstant(agencyCode, constantName, string.Empty);

            if (constants == null || constants.Length == 0)
            {
                return string.Empty;
            }

            string constantVlaue = string.Empty;

            // loop to find the matched constant name
            foreach (ServerConstantModel4WS constant in constants)
            {
                if (constant.serverConstant.Equals(constantName, StringComparison.InvariantCultureIgnoreCase))
                {
                    constantVlaue = constant.serverConstantValue;
                    break;
                }
            }

            return constantVlaue;
        }

        /// <summary>
        /// Gets public users' groups for ACA.
        /// </summary>
        /// <param name="agencyCode">agency code.</param>
        /// <returns>public users' groups hash table.</returns>
        internal static Hashtable GetPublicUserGroups(string agencyCode)
        {
            UserGroupModel4WS[] userGroups = ServerConstantService.getPublicUserGroups(agencyCode, string.Empty);

            Hashtable htUserGroups = new Hashtable();

            if (userGroups == null || userGroups.Length == 0)
            {
                return htUserGroups;
            }

            foreach (UserGroupModel4WS userGroup in userGroups)
            {
                if (!htUserGroups.ContainsKey(userGroup.moduleName))
                {
                    htUserGroups.Add(userGroup.moduleName, userGroup.groupSeqNumber);
                }
                else
                {
                    htUserGroups[userGroup.moduleName] = userGroup.groupSeqNumber;
                }
            }

            return htUserGroups;
        }

        /// <summary>
        /// Get all Smart Choice Group for ACA.
        /// </summary>
        /// <param name="agencyCode">agency code.</param>
        /// <returns>all smart choice group for ACA</returns>
        internal static Hashtable GetSmartChoiceGroup(string agencyCode)
        {
            //SmartChoiceGroupWebServiceService smartChoiceWS = WSFactory.Instance.GetSmartChoiceGroupService();

            // group name and callerid is null to get all scg data for ACA.
            SmartChoiceGroupModel4WS[] smartChoiceGroups = SmartChoiceGroupService.getSmartChoiceGroup(agencyCode, null, null);

            Hashtable htGroups = new Hashtable();

            if (smartChoiceGroups == null || smartChoiceGroups.Length == 0)
            {
                return htGroups;
            }

            foreach (SmartChoiceGroupModel4WS scg in smartChoiceGroups)
            {
                if (!htGroups.ContainsKey(scg.groupCode))
                {
                    ArrayList groupItems = new ArrayList();

                    groupItems.Add(scg);
                    htGroups.Add(scg.groupCode, groupItems);
                }
                else
                {
                    (htGroups[scg.groupCode] as ArrayList).Add(scg);
                }
            }

            Hashtable result = new Hashtable();

            if (htGroups.Count > 0)
            {
                IDictionaryEnumerator enumerator = htGroups.GetEnumerator();

                while (enumerator.MoveNext())
                {
                    ArrayList sectionList = enumerator.Value as ArrayList;

                    if (sectionList.Count > 0)
                    {
                        SmartChoiceGroupModel4WS[] sections = new SmartChoiceGroupModel4WS[sectionList.Count];
                        sectionList.CopyTo(sections);
                        result.Add(enumerator.Key, sections);
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// Gets all of standard choice from web service.
        /// </summary>
        /// <param name="agencyCode">agency code.</param>
        /// <param name="culture">The culture.</param>
        /// <returns>All of standard choice that is used by ACA.</returns>
        internal static Hashtable GetStandardChoices(string agencyCode, string culture)
        {
            // Get All of standard choice category is used by ACA.
            string[] categorys = BizDomainConstant.GetACAStandardChoiceCategoryNames();

            if (categorys == null || categorys.Length == 0)
            {
                return new Hashtable();
            }

            //BizDomainWebServiceService _bizDomainWS = WSFactory.Instance.GetBizDomainService();
            Hashtable stdCategories = new Hashtable();

            // Loop category to get all standard choice items.
            foreach (string category in categorys)
            {
                Hashtable stdItems = GetBizDomainKeyValuePairs(agencyCode, category, culture);

                if (BizDomainConstant.STD_CAT_COUNTRY_IDD.Equals(category))
                {
                    List<RegionalModel> regionalModels = GetRegionalModels(agencyCode + ACAConstant.SPLIT_CHAR + CacheConstant.CACHE_KEY_REGIONAL_CONFIG);
                    Dictionary<string, string> keyList = new Dictionary<string, string>();

                    foreach (DictionaryEntry dictionaryEntry in stdItems)
                    {
                        RegionalModel regionalModel = regionalModels.FirstOrDefault(o => string.Equals(o.countryCode, dictionaryEntry.Key.ToString(), StringComparison.InvariantCultureIgnoreCase));

                        if (regionalModel != null)
                        {
                            keyList.Add(dictionaryEntry.Key.ToString(), regionalModel.phoneNumCode);
                        }
                    }

                    foreach (KeyValuePair<string, string> keyValuePair in keyList)
                    {
                        if (!string.IsNullOrEmpty(keyValuePair.Value))
                        {
                            stdItems[keyValuePair.Key] = keyValuePair.Value;
                        }
                    }
                }

                if (!stdCategories.Contains(category) && stdItems != null)
                {
                    stdCategories.Add(category, stdItems);
                }
            }

            return stdCategories;
        }

        /// <summary>
        /// Regionals the state cache.
        /// </summary>
        /// <param name="cacheRegionalStateKey">The cache regional state key.</param>
        /// <param name="regionalModels">The regional models.</param>
        /// <returns>regional setting with state.</returns>
        internal static Dictionary<string, Dictionary<string, string>> GetRegionalState(string cacheRegionalStateKey, List<RegionalModel> regionalModels)
        {
            Dictionary<string, string> stateSTD = new Dictionary<string, string>();
            Dictionary<string, Dictionary<string, string>> regionalStateCache = new Dictionary<string, Dictionary<string, string>>();

            if (HttpRuntime.Cache.Get(cacheRegionalStateKey) != null)
            {
                regionalStateCache = HttpRuntime.Cache.Get(cacheRegionalStateKey) as Dictionary<string, Dictionary<string, string>>;
            }
            else
            {
                //no regional setting in config, return empty collection
                if (regionalModels == null || regionalModels.Count == 0)
                {
                    return regionalStateCache;
                }

                foreach (var model in regionalModels)
                {
                    if (!string.IsNullOrEmpty(model.mappedState))
                    {
                        stateSTD.Add(model.countryCode, model.mappedState);
                    }
                }

                BizDomainModel[] bizDomains = null;

                if (stateSTD.Count > 0)
                {
                    bizDomains = RegionalService.getStatesConfig(ConfigManager.AgencyCode, stateSTD.Values.Distinct().ToArray());
                }

                if (bizDomains == null || bizDomains.Length == 0)
                {
                    return regionalStateCache;
                }

                foreach (KeyValuePair<string, string> keyValuePair in stateSTD)
                {
                    string key = keyValuePair.Key;
                    string value = keyValuePair.Value;
                    var bizDomain4Country = bizDomains.Where(o => o.bizdomain.Equals(value, StringComparison.InvariantCultureIgnoreCase)).ToList();

                    //if the collection is null then continue
                    if (!bizDomain4Country.Any())
                    {
                        continue;
                    }

                    Dictionary<string, string> regionalStates = new Dictionary<string, string>();

                    foreach (BizDomainModel bizDomainModel in bizDomain4Country)
                    {
                        //if STD has same state code then continue
                        if (!regionalStates.Any(o => string.Equals(o.Key, bizDomainModel.bizdomainValue, StringComparison.InvariantCultureIgnoreCase)))
                        {
                            string text = I18nStringUtil.GetString(bizDomainModel.resBizdomainValue, bizDomainModel.bizdomainValue);
                            regionalStates.Add(bizDomainModel.bizdomainValue, text);
                        }
                    }

                    regionalStateCache.Add(key, regionalStates);
                }
            }

            return regionalStateCache;
        }

        /// <summary>
        /// Regionals the models.
        /// </summary>
        /// <param name="cacheRegionalKey">The cache regional key.</param>
        /// <returns>regional setting</returns>
        internal static List<RegionalModel> GetRegionalModels(string cacheRegionalKey)
        {
            List<RegionalModel> regionalModels = new List<RegionalModel>();

            if (HttpRuntime.Cache.Get(cacheRegionalKey) != null)
            {
                regionalModels = HttpRuntime.Cache.Get(cacheRegionalKey) as List<RegionalModel>;
            }
            else
            {
                RegionalModel[] regionals = RegionalService.getAllRegional(ConfigManager.AgencyCode);

                if (regionals != null && regionals.Length != 0)
                {
                    regionalModels = regionals.ToList();
                }
            }

            return regionalModels;
        }

        /// <summary>
        /// Get all APO and People templates for ACA.
        /// </summary>
        /// <param name="agencyCode">agency code.</param>
        /// <returns>all template for ACA</returns>
        internal static Hashtable GetTemplates(string agencyCode)
        {
            Hashtable htTempates = new Hashtable();

            //TemplateWebServiceService templateWS = WSFactory.Instance.GetTemplateService();

            // 1.Get all APO templates. Needn't template type,templateRefNum,callerId when getting all template.
            TemplateAttributeModel[] attributeModels = TemplateService.getAttributes(agencyCode, TEMPLATE_GENUS_APO, null, null, null);

            // add all of APO template to hashtable
            AddAttributesToHashtable(attributeModels, htTempates, TEMPLATE_GENUS_APO);

            // 2.Get all People templates. Needn't template type,templateRefNum,callerId when getting all template.
            attributeModels = TemplateService.getAttributes(agencyCode, TEMPLATE_GENUS_PEOPLE, null, null, null);

            // add all of people template to template hashtable
            AddAttributesToHashtable(attributeModels, htTempates, TEMPLATE_GENUS_PEOPLE);

            // Convert ArrayList to Array
            Hashtable result = new Hashtable();

            if (htTempates.Count > 0)
            {
                IDictionaryEnumerator enumerator = htTempates.GetEnumerator();

                while (enumerator.MoveNext())
                {
                    ArrayList attributeList = enumerator.Value as ArrayList;

                    if (attributeList.Count > 0)
                    {
                        TemplateAttributeModel[] attributes = new TemplateAttributeModel[attributeList.Count];
                        attributeList.CopyTo(attributes, 0);
                        result.Add(enumerator.Key, attributes);
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// Get all XPolicy for ACA.
        /// </summary>
        /// <param name="agencyCode">The agency code</param>
        /// <param name="culture">The culture.</param>
        /// <returns>all XPolicy for ACA</returns>
        internal static Hashtable GetXPolicyList(string agencyCode, string culture)
        {
            Hashtable htPolicys = new Hashtable();
            string[] categorys = BizDomainConstant.GetXPolicyCategoryNames();

            if (categorys == null || categorys.Length == 0)
            {
                return htPolicys;
            }

            XPolicyModel[] policies = XPolicyService.getXPolicyList(agencyCode, categorys, string.Empty, GetCulture(culture));

            if (policies == null || policies.Length == 0)
            {
                return htPolicys;
            }

            List<XPolicyModel> groupedXPolicy;

            foreach (XPolicyModel policy in policies)
            {
                if (!htPolicys.ContainsKey(policy.policyName))
                {
                    groupedXPolicy = new List<XPolicyModel>();
                    groupedXPolicy.Add(policy);
                    htPolicys.Add(policy.policyName, groupedXPolicy);
                }
                else
                {
                    (htPolicys[policy.policyName] as List<XPolicyModel>).Add(policy);
                }
            }

            return htPolicys;
        }

        /// <summary>
        /// Get all XPolicy for ACA.
        /// </summary>
        /// <param name="agencyCode">The agency code</param>
        /// <returns>all page flow group for ACA</returns>
        internal static Hashtable GetXPolicys(string agencyCode)
        {
            XPolicyModel[] policys = PolicyService.getPolicyByNameAndLevelType(agencyCode, XPolicyConstant.EPAYMENT_ADAPTER, "Adapter");

            Hashtable htPolicys = new Hashtable();

            if (policys == null || policys.Length == 0)
            {
                return htPolicys;
            }

            ArrayList groupedXPolicy;

            foreach (XPolicyModel policy in policys)
            {
                string key = policy.serviceProviderCode + policy.policyName;

                if (!htPolicys.ContainsKey(key))
                {
                    groupedXPolicy = new ArrayList();
                    groupedXPolicy.Add(policy);
                    htPolicys.Add(key, groupedXPolicy);
                }
                else
                {
                    (htPolicys[key] as ArrayList).Add(policy);
                }
            }

            return htPolicys;
        }

        /// <summary>
        /// Gets I18n primary settings.
        /// </summary>
        /// <param name="agencyCode">The agency code.</param>
        /// <returns>I18n settings</returns>
        internal static Hashtable GetI18nPrimarySettings(string agencyCode)
        {
            Hashtable results = new Hashtable(StringComparer.InvariantCultureIgnoreCase);
            I18nPrimarySettingsModel i18nPrimarySettings = I18nSettingsService.getI18nPrimarySettings(agencyCode);
            results.Add(CacheConstant.CACHE_KEY_I18N_PRIMARY_SETTINGS, i18nPrimarySettings);

            return results;
        }

        /// <summary>
        /// Gets I18n locale relevant settings.
        /// </summary>
        /// <param name="agencyCode">The agency code.</param>
        /// <param name="locale">The locale.</param>
        /// <returns>I18n settings</returns>
        internal static Hashtable GetI18nLocaleRelevantSettings(string agencyCode, string locale)
        {
            Hashtable results = new Hashtable(StringComparer.InvariantCultureIgnoreCase);
            I18nLocaleRelevantSettingsModel i18nLocaleRelevantSettings = I18nSettingsService.getI18nLocaleRelevantSettings(agencyCode, locale);
            results.Add(CacheConstant.CACHE_KEY_I18N_LOCALE_RELEVANT_SETTINGS, i18nLocaleRelevantSettings);

            return results;
        }

        /// <summary>
        /// Get generic view elements.
        /// </summary>
        /// <param name="agencyCode">agency code.</param>
        /// <param name="moduleName">module name.</param>
        /// <param name="viewID">string view id.</param>
        /// <returns>generic view array</returns>
        internal static SimpleViewElementModel4WS[] GetGViewElements(string agencyCode, string moduleName, string viewID)
        {
            string levelType = ACAConstant.LEVEL_TYPE_MODULE;

            if (string.IsNullOrEmpty(moduleName) || moduleName == agencyCode)
            {
                moduleName = agencyCode;
                levelType = ACAConstant.LEVEL_TYPE_AGENCY;
            }

            SimpleViewElementModel4WS[] simpleViewElements = GFilterViewService.getSimpleViewElementModel(agencyCode, levelType, moduleName, viewID);

            return simpleViewElements;
        }

        /// <summary>
        /// Get generic view elements.
        /// </summary>
        /// <param name="agencyCode">agency code.</param>
        /// <param name="moduleName">module name.</param>
        /// <param name="permission">The permission.</param>
        /// <param name="viewID">generic view id.</param>
        /// <param name="callerId">The caller ID.</param>
        /// <returns>generic view array</returns>
        internal static SimpleViewElementModel4WS[] GetGViewElements(string agencyCode, string moduleName, GFilterScreenPermissionModel4WS permission, string viewID, string callerId)
        {
            string levelType = ACAConstant.LEVEL_TYPE_MODULE;

            if (string.IsNullOrEmpty(moduleName) || moduleName == agencyCode)
            {
                moduleName = agencyCode;
                levelType = ACAConstant.LEVEL_TYPE_AGENCY;
            }

            SimpleViewElementModel4WS[] simpleViewElements = null;
            SimpleViewModel4WS simpleView = GFilterViewService.getFilterScreenView(agencyCode, levelType, moduleName, viewID, permission, callerId);

            if (simpleView != null)
            {
                simpleViewElements = simpleView.simpleViewElements;
            }

            return simpleViewElements;
        }

        /// <summary>
        /// Gets the generic view.
        /// </summary>
        /// <param name="agencyCode">The agency code.</param>
        /// <param name="moduleName">Name of the module.</param>
        /// <param name="permission">The permission.</param>
        /// <param name="viewID">The view ID.</param>
        /// <param name="callerId">The caller id.</param>
        /// <returns>generic view</returns>
        internal static SimpleViewModel4WS GetGView(string agencyCode, string moduleName, GFilterScreenPermissionModel4WS permission, string viewID, string callerId)
        {
            string levelType = ACAConstant.LEVEL_TYPE_MODULE;

            if (string.IsNullOrEmpty(moduleName) || moduleName == agencyCode)
            {
                moduleName = agencyCode;
                levelType = ACAConstant.LEVEL_TYPE_AGENCY;
            }

            SimpleViewModel4WS simpleView = GFilterViewService.getFilterScreenView(agencyCode, levelType, moduleName, viewID, permission, callerId);

            if (simpleView != null)
            {
                simpleView.simpleViewElements = null;
            }

            return simpleView;
        }

        /// <summary>
        /// Get App Status List By Agency Code and Module Name
        /// </summary>
        /// <param name="agencyCode">Agency Code</param>
        /// <param name="moduleName">Module Name</param>
        /// <returns>App Status List</returns>
        internal static AppStatusGroupModel4WS[] GetAppStatusByModule(string agencyCode, string moduleName)
        {
            return AppStatusGroupWebservice.getAppStatusGroupBySPC(agencyCode, moduleName);
        }

        /// <summary>
        /// Add attributes to specify template hashtable, the stored values is ArrayList for each type template.
        /// </summary>
        /// <param name="attributes">TemplateAttributeModel array</param>
        /// <param name="htTempates">the template hashtable that attribute will be added to.</param>
        /// <param name="templateGenus">template genus such as APO and people.</param>
        private static void AddAttributesToHashtable(TemplateAttributeModel[] attributes, Hashtable htTempates, string templateGenus)
        {
            if (attributes == null || attributes.Length == 0)
            {
                return;
            }

            foreach (TemplateAttributeModel attribute in attributes)
            {
                if (attribute == null || string.IsNullOrEmpty(attribute.attributeName))
                {
                    continue;
                }

                string templateTypeKey;

                if (templateGenus == TEMPLATE_GENUS_APO)
                {
                    // build the templte key with template type with big template (APO),CAP_ is required according to ACAConstant.TemplateType enumerate
                    templateTypeKey = templateGenus + "|cap_" + attribute.templateType.ToLower();
                }
                else
                {
                    templateTypeKey = templateGenus + "|" + attribute.templateType.ToLower();
                }

                if (!htTempates.ContainsKey(templateTypeKey))
                {
                    ArrayList attributesList = new ArrayList();

                    attributesList.Add(attribute);
                    htTempates.Add(templateTypeKey, attributesList);
                }
                else
                {
                    (htTempates[templateTypeKey] as ArrayList).Add(attribute);
                }
            }
        }

        /// <summary>
        /// Build unique label key by XUIText model
        /// </summary>
        /// <param name="textModel">XUITextModel object.</param>
        /// <returns>label key with different level.</returns>
        private static string BuildLabelKey(XUITextModel textModel)
        {
            string cacheKey = string.Empty;

            if (ACAConstant.GLOBAL.Equals(textModel.textLevelType, StringComparison.InvariantCultureIgnoreCase))
            {
                cacheKey = textModel.stringKey;
            }
            else if (ACAConstant.LEVEL_TYPE_AGENCY.Equals(textModel.textLevelType, StringComparison.InvariantCultureIgnoreCase))
            {
                cacheKey = textModel.servProvCode + "_" + textModel.stringKey;
            }
            else if (ACAConstant.LEVEL_TYPE_PAGEFLOW.Equals(textModel.textLevelType, StringComparison.InvariantCultureIgnoreCase))
            {
                cacheKey = textModel.servProvCode + "_" + ACAConstant.LEVEL_TYPE_PAGEFLOW + "_" + textModel.textLevelName + "_" + textModel.stringKey;
            }
            else
            {
                cacheKey = textModel.servProvCode + "_" + textModel.textLevelName + "_" + textModel.stringKey;
            }

            return cacheKey;
        }

        /// <summary>
        /// Handle page url - Remove the unnecessary parameters.
        /// </summary>
        /// <param name="node">a AcaAdminTreeNodeModel4WS</param>
        private static void HandlePageUrl(AcaAdminTreeNodeModel4WS node)
        {
            if (node == null)
            {
                return;
            }

            // These pages are used more than once, so the parameter is necessary.
            string[] repeatedUsePags = ACAConstant.GetRepeatedUsePages();

            bool isNeedParam = false;

            foreach (string page in repeatedUsePags)
            {
                if (node.actionURL.Contains(page))
                {
                    // remove the unnecessary parameters
                    node.actionURL = page;

                    isNeedParam = true;
                    break;
                }
            }

            // Remove url parameter
            if (!isNeedParam && node.actionURL.IndexOf('?') > -1)
            {
                node.actionURL = node.actionURL.Substring(0, node.actionURL.IndexOf('?'));
            }
        }

        /// <summary>
        /// Is In Categories for Master Lang Description.
        /// </summary>
        /// <param name="category">category name.</param>
        /// <param name="bizdomainValue">standard choices value.</param>
        /// <returns>Is In Categories for Master Language Description</returns>
        private static bool IsInCategories4MasterLangDesc(string category, string bizdomainValue)
        {
            if (IsInI18nSettings4MasterLangDesc(category, bizdomainValue))
            {
                return true;
            }
            else
            {
                return Categories4MasterLangDesc.ContainsKey(category);
            }
        }

        /// <summary>
        /// Is In Categories for Multi Lang All
        /// </summary>
        /// <param name="category">category name</param>
        /// <param name="bizdomainValue">standard choices value</param>
        /// <returns>true or false.</returns>
        private static bool IsInCategories4MultiLangAll(string category, string bizdomainValue)
        {
            return Categories4MultiLangAll.ContainsKey(category);
        }

        /// <summary>
        /// Is In Categories for Multi Lang All
        /// </summary>
        /// <param name="category">category name</param>
        /// <param name="bizdomainValue">standard choices value</param>
        /// <returns>true or false.</returns>
        private static bool IsInCategories4MultiLangAllWithMasterLangKey(string category, string bizdomainValue)
        {
            return Categories4MultiLangAllWithMasterLangKey.ContainsKey(category);
        }

        /// <summary>
        /// Is In Categories for Multi Lang Description.
        /// </summary>
        /// <param name="category">category name</param>
        /// <param name="bizdomainValue">standard choices value.</param>
        /// <returns>true or false.</returns>
        private static bool IsInCategories4MultiLangDesc(string category, string bizdomainValue)
        {
            if (category.Equals(BizDomainConstant.STD_CAT_I18N_SETTINGS, StringComparison.InvariantCulture) && !IsInI18nSettings4MasterLangDesc(category, bizdomainValue))
            {
                return true;
            }
            else
            {
                return Categories4MultiLangDesc.ContainsKey(category);
            }
        }

        /// <summary>
        /// Is In Categories for Multi Lang Value
        /// </summary>
        /// <param name="category">category name</param>
        /// <param name="bizdomainValue">standard choices Value</param>
        /// <returns>true or false.</returns>
        private static bool IsInCategories4MultiLangValue(string category, string bizdomainValue)
        {
            return Categories4MultiLangValue.ContainsKey(category);
        }
        
        /// <summary>
        /// remove "_admin" from culture.
        /// </summary>
        /// <param name="culture">the culture.</param>
        /// <returns>culture string.</returns>
        private static string GetCulture(string culture)
        {
            string newCulture = string.IsNullOrEmpty(culture) ? string.Empty : culture.Trim();

            if (newCulture.EndsWith(ADMIN_SUFFIX, StringComparison.OrdinalIgnoreCase))
            {
                newCulture = newCulture.Replace(ADMIN_SUFFIX, string.Empty);
            }
            else if (newCulture.EndsWith(DAILY_SUFFIX, StringComparison.OrdinalIgnoreCase))
            {
                newCulture = newCulture.Replace(DAILY_SUFFIX, string.Empty);
            }

            newCulture = I18nCultureUtil.ChangeCulture4WS(newCulture);

            return newCulture;
        }

        /// <summary>
        /// Check is in I18nSettings for master language description
        /// </summary>
        /// <param name="category">string category</param>
        /// <param name="bizdomainValue">string standard choices value</param>
        /// <returns>true of false</returns>
        private static bool IsInI18nSettings4MasterLangDesc(string category, string bizdomainValue)
        {
            if (category.Equals(BizDomainConstant.STD_CAT_I18N_SETTINGS, StringComparison.InvariantCulture))
            {
                if (bizdomainValue.Equals(ACAConstant.I18N_SETTINGS_CURRENCY_LOCALE, StringComparison.InvariantCultureIgnoreCase) || bizdomainValue.Equals(ACAConstant.I18N_SETTINGS_DEFAULT_LANGUAGE, StringComparison.InvariantCultureIgnoreCase) || bizdomainValue.Equals(ACAConstant.I18N_SETTINGS_MULTI_LANGUAGE_SUPPORT_ENABLE, StringComparison.InvariantCultureIgnoreCase))
                {
                    return true;
                }
            }

            return false;
        }

        #endregion Methods
    }
}
