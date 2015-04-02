#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: BizDomainBll.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2008-2014
 *
 *  Description:
 *
 *  Notes:
 * $Id: BizDomainBll.cs 278428 2014-09-03 10:30:50Z ACHIEVO\james.shi $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Web;

using Accela.ACA.Common;
using Accela.ACA.Common.Log;
using Accela.ACA.Common.Util;

using Accela.ACA.WSProxy;

using log4net;

namespace Accela.ACA.BLL.Common
{
    /// <summary>
    /// This class provide the ability to get standard choice value.
    /// </summary>
    public class BizDomainBll : BaseBll, IBizDomainBll
    {
        #region Fields

        /// <summary>
        /// force login
        /// </summary>
        private const string FORCELOGIN = "forcelogin";

        /// <summary>
        /// The single service only
        /// </summary>
        private const string SINGLESERVICEONLY = "singleserviceonly";

        /// <summary>
        /// the url prefix.
        /// </summary>
        private const string URL_PREFIX = "url=";

        /// <summary>
        /// logger object.
        /// </summary>
        private static readonly ILog Logger = LogFactory.Instance.GetLogger(typeof(BizDomainBll));

        #endregion Fields

        #region Properties

        /// <summary>
        /// Gets an instance of BizDomainWebService.
        /// </summary>
        private BizDomainWebServiceService BizDomainService
        {
            get
            {
                return WSFactory.Instance.GetWebService<BizDomainWebServiceService>();
            }
        }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Define Standard Choice,Create and Edit Biz Domain Model Array.
        /// </summary>
        /// <param name="bizDomainModel4WSArray">BizDomainMode for web service</param>
        /// <returns>true successful, false failure.</returns>
        /// <exception cref="DataValidateException">{ <c>bizDomainModel4WSArray</c> } is null</exception>
        /// <exception cref="ACAException">Exception from web service.</exception>
        public bool CreateAndEditBizDomain(BizDomainModel4WS[] bizDomainModel4WSArray)
        {
            if (bizDomainModel4WSArray == null)
            {
                throw new DataValidateException(new string[] { "bizDomainModel4WSArray" });
            }

            try
            {
                BizDomainService.createAndEditBizDomain(bizDomainModel4WSArray, false);
                return true;
            }
            catch (Exception e)
            {
                throw new ACAException(e);
            }
        }

        /// <summary>
        /// Define Standard Choice and Create Biz Domain to save.
        /// </summary>
        /// <param name="bizDomainModel4WS">BizDomainMode for web service</param>
        /// <returns>true successful, false failure.</returns>
        /// <exception cref="DataValidateException">{ <c>bizDomainModel4WS</c> } is null</exception>
        /// <exception cref="ACAException">Exception from web service.</exception>
        public bool CreateBizDomain(BizDomainModel4WS bizDomainModel4WS)
        {
            if (bizDomainModel4WS == null)
            {
                throw new DataValidateException(new string[] { "bizDomainModel4WS" });
            }

            try
            {
                return BizDomainService.createBizDomain(bizDomainModel4WS, false);
            }
            catch (Exception e)
            {
                throw new ACAException(e);
            }
        }

        /// <summary>
        /// Define Standard Choice and Update Biz Domain.
        /// </summary>
        /// <param name="bizDomainModel4WS">BizDomainMode for web service</param>
        /// <returns>true successful, false failure.</returns>
        /// <exception cref="DataValidateException">{ <c>bizDomainModel4WS</c> } is null</exception>
        /// <exception cref="ACAException">Exception from web service.</exception>
        public bool EditBizDomain(BizDomainModel4WS bizDomainModel4WS)
        {
            if (bizDomainModel4WS == null)
            {
                throw new DataValidateException(new string[] { "bizDomainModel4WS" });
            }

            try
            {
                return BizDomainService.editBizDomain(bizDomainModel4WS, false);
            }
            catch (Exception e)
            {
                throw new ACAException(e);
            }
        }

        /// <summary>
        /// Method to get contact type in biz domain(standard choice) object list.
        /// </summary>
        /// <param name="agencyCode">agency code</param>
        /// <param name="bizName">biz domain name</param>
        /// <param name="includeInactivity">if include inactivity items</param>
        /// <returns>ItemValue list.</returns>
        /// <exception cref="DataValidateException">{ <c>agencyCode, bizName</c> } is null</exception>
        /// <exception cref="ACAException">Exception from web service.</exception>
        public IList<ItemValue> GetBizDomainList(string agencyCode, string bizName, bool includeInactivity)
        {
            if (string.IsNullOrEmpty(agencyCode) || string.IsNullOrEmpty(bizName))
            {
                throw new DataValidateException(new string[] { "agencyCode", bizName });
            }

            IList<ItemValue> items = new List<ItemValue>();

            try
            {
                ICacheManager cacheManager = ObjectFactory.GetObject<ICacheManager>();
                string cacheKey = string.Format("{0}{1}{2}", agencyCode, ACAConstant.SPLIT_CHAR, I18nCultureUtil.AppendUserPreferredCultureFlag(CacheConstant.CACHE_KEY_STANDARDCHOICE));
                Hashtable htStdCates = cacheManager.GetCachedItem(agencyCode, cacheKey);

                if (htStdCates == null)
                {
                    return items;
                }

                Hashtable htStdItems = htStdCates[bizName] as Hashtable;

                if (htStdItems != null && htStdItems.Count > 0)
                {
                    foreach (string key in htStdItems.Keys)
                    {
                        ItemValue item = new ItemValue();
                        item.Key = key;
                        item.Value = htStdItems[key];
                        items.Add(item);
                    }

                    //should not use "items.ToList().Sort(ItemValueComparer.Instance);", because it will new a instance when tolist().
                    ((List<ItemValue>)items).Sort(ItemValueComparer.Instance);
                }

                return items;
            }
            catch (Exception e)
            {
                throw new ACAException(e);
            }
        }

        /// <summary>
        /// Method to get biz domain(standard choice) object list.
        /// </summary>
        /// <param name="agencyCode">agency code</param>
        /// <param name="bizName">biz domain name</param>
        /// <param name="includeInactivity">whether need to show inactivity record</param>
        /// <param name="showType">indicates what value is used for standard choice, value,value description or both</param>
        /// <returns>Standard choice item list.</returns>
        public IList<ItemValue> GetBizDomainList(string agencyCode, string bizName, bool includeInactivity, int showType)
        {
            return GetBizDomainList(agencyCode, bizName, includeInactivity);
        }

        /// <summary>
        /// Method to get contact type in biz domain(standard choice) object list.
        /// </summary>
        /// <param name="agencyCode">agency code</param>
        /// <param name="includeInactivity">if include inactivity items</param>
        /// <param name="contactTypeSource">BIZ domain description</param>
        /// <returns>Contact type list.</returns>
        /// <exception cref="DataValidateException">{ <c>agencyCode</c> } is null</exception>
        /// <exception cref="ACAException">Exception from web service.</exception>
        public IList<ItemValue> GetContactTypeList(string agencyCode, bool includeInactivity, string contactTypeSource)
        {
            if (string.IsNullOrEmpty(agencyCode))
            {
                throw new DataValidateException(new string[] { "agencyCode" });
            }

            IList<ItemValue> items = new List<ItemValue>();
            try
            {
                ICacheManager cacheManager = ObjectFactory.GetObject<ICacheManager>();
                BizDomainModel4WS[] models = cacheManager.GetAllContactType(agencyCode, includeInactivity);

                if (models != null && models.Length > 0)
                {
                    if (!ContactTypeSource.All.Equals(contactTypeSource, StringComparison.InvariantCultureIgnoreCase))
                    {
                        models = models.Where(o =>
                            string.Equals(o.description, ContactTypeSource.Both, StringComparison.InvariantCultureIgnoreCase) ||
                            string.Equals(o.description, contactTypeSource, StringComparison.InvariantCultureIgnoreCase)).ToArray();
                    }

                    foreach (BizDomainModel4WS bdm in models)
                    {
                        ItemValue item = new ItemValue();

                        item.Value = I18nStringUtil.GetString(bdm.resBizdomainValue, bdm.bizdomainValue);
                        item.Key = bdm.bizdomainValue;

                        items.Add(item);
                    }
                }

                return items;
            }
            catch (Exception e)
            {
                throw new ACAException(e);
            }
        }

        /// <summary>
        /// Method to get biz domain(standard choice) object model.
        /// </summary>
        /// <param name="bizDomainModel4WS">BizDomainModel for web service</param>
        /// <param name="callerID">callerID number.</param>
        /// <returns>Biz Domain Model</returns>
        /// <exception cref="DataValidateException">{ <c>bizDomainModel4WS, callerID</c> } is null</exception>
        /// <exception cref="ACAException">Exception from web service.</exception>
        public BizDomainModel4WS GetBizDomainListByModel(BizDomainModel4WS bizDomainModel4WS, string callerID)
        {
            if (bizDomainModel4WS == null)
            {
                throw new DataValidateException(new string[] { "bizDomainModel4WS" });
            }

            if (string.IsNullOrEmpty(callerID))
            {
                throw new DataValidateException(new string[] { "callerID" });
            }

            try
            {
                BizDomainModel4WS[] stdItems = BizDomainService.getBizDomainListByModel(bizDomainModel4WS, callerID, false);

                if (stdItems == null)
                {
                    return null;
                }
                else
                {
                    return stdItems[0];
                }
            }
            catch (Exception e)
            {
                throw new ACAException(e);
            }
        }

        /// <summary>
        /// Method to get biz domain(standard choice) object model.
        /// </summary>
        /// <param name="bizDomainModel4WS">BizDomainModel for web service</param>
        /// <param name="key">key values.</param>
        /// <param name="callerID">callerID  number.</param>
        /// <returns>Biz Domain Model</returns>
        /// <exception cref="DataValidateException">{ <c>bizDomainModel4WS, key, callerID</c> } is null</exception>
        /// <exception cref="ACAException">Exception from web service.</exception>
        public BizDomainModel4WS GetBizDomainListByModel(BizDomainModel4WS bizDomainModel4WS, string key, string callerID)
        {
            if (bizDomainModel4WS == null)
            {
                throw new DataValidateException(new string[] { "bizDomainModel4WS" });
            }

            if (string.IsNullOrEmpty(key) || string.IsNullOrEmpty(callerID))
            {
                throw new DataValidateException(new string[] { "key, callerID" });
            }

            try
            {
                BizDomainModel4WS bizWS = new BizDomainModel4WS();
                BizDomainModel4WS[] stdItems = BizDomainService.getBizDomainListByModel(bizDomainModel4WS, callerID, false);

                if (stdItems == null)
                {
                    return null;
                }

                foreach (BizDomainModel4WS ws in stdItems)
                {
                    if (ws.bizdomainValue.Equals(key, StringComparison.InvariantCultureIgnoreCase))
                    {
                        bizWS = ws;
                    }
                }

                return bizWS;
            }
            catch (Exception e)
            {
                throw new ACAException(e);
            }
        }

        /// <summary>
        /// Method to get biz domain(standard choice) values.
        /// </summary>
        /// <param name="agencyCode">agency code</param>
        /// <param name="bizName">biz domain name</param>
        /// <returns>Array of String</returns>
        /// <exception cref="DataValidateException">{ <c>agencyCode, bizName</c> } is null</exception>
        /// <exception cref="ACAException">Exception from web service.</exception>
        public string[] GetBizDomainValueList(string agencyCode, string bizName)
        {
            if (string.IsNullOrEmpty(agencyCode) || string.IsNullOrEmpty(bizName))
            {
                throw new DataValidateException(new string[] { "agencyCode", bizName });
            }

            try
            {
                ICacheManager cacheManager = ObjectFactory.GetObject<ICacheManager>();
                string cacheKey = string.Format("{0}{1}{2}", agencyCode, ACAConstant.SPLIT_CHAR, I18nCultureUtil.AppendUserPreferredCultureFlag(CacheConstant.CACHE_KEY_STANDARDCHOICE));
                Hashtable htStdCates = cacheManager.GetCachedItem(agencyCode, cacheKey);

                if (htStdCates == null)
                {
                    return null;
                }

                Hashtable htStdItems = htStdCates[bizName] as Hashtable;

                if (htStdItems == null || htStdItems.Count == 0)
                {
                    return null;
                }

                string[] result = new string[htStdItems.Count];

                int index = 0;

                foreach (DictionaryEntry de in htStdItems)
                {
                    result[index++] = (string)de.Key;
                }

                return result;
            }
            catch (Exception e)
            {
                throw new ACAException(e);
            }
        }

        /// <summary>
        /// This Configured Url of link button.
        /// </summary>
        /// <returns>string for configured url</returns>
        public string GetConfiguredUrlFromXPolicy()
        {
            IList<ItemValue> buttonUrlbizDomainList = GetBizDomainList(AgencyCode, BizDomainConstant.STD_CAT_ACA_PAGE_PICKER, false);

            IXPolicyBll xPolicy = ObjectFactory.GetObject<IXPolicyBll>();

            string shoppingCartPolicyValue = xPolicy.GetValueByKey(ACAConstant.ACA_SHOPPING_CART_REDIRECT_PAGE, ACAConstant.LEVEL_TYPE_AGENCY, AgencyCode);

            if (string.IsNullOrEmpty(shoppingCartPolicyValue))
            {
                return string.Empty;
            }

            string selectedUrl = string.Empty;

            foreach (ItemValue buttonUrlbizDomain in buttonUrlbizDomainList)
            {
                if (buttonUrlbizDomain.Key == shoppingCartPolicyValue)
                {
                    string buttonUrlbizDomainValue = buttonUrlbizDomain.Value.ToString();
                    string[] nameAndUrl = buttonUrlbizDomainValue.Split(',');

                    selectedUrl = nameAndUrl[1].Replace(URL_PREFIX, string.Empty);

                    break;
                }
            }

            return selectedUrl;
        }

        /// <summary>
        /// This method gets the Tabs from the DB and sorts them in the right order.
        /// </summary>
        /// <param name="agencyCode">the agency code</param>
        /// <param name="userType">ACA user type</param>
        /// <param name="hasProviderLicense">if set to <c>true</c> [has provider license].</param>
        /// <returns>Sorted List of tabs</returns>
        /// <exception cref="ACAException">
        /// 1. 'ACA_CONFIGS_TABS' node hasn't been configured in Standard Choice. Please configured it correctly.
        /// 2. Exception from web service.
        /// </exception>
        public IList<TabItem> GetTabsList(string agencyCode, ACAUserType userType, bool hasProviderLicense)
        {
            IList<ItemValue> tabs = GetBizDomainList(agencyCode, BizDomainConstant.STD_CAT_ACA_CONFIGS_TABS, false, 1);
            IList<ItemValue> links = GetBizDomainList(agencyCode, BizDomainConstant.STD_CAT_ACA_CONFIGS_LINKS, false, 1);

            if (tabs == null || tabs.Count == 0)
            {
                throw new ACAException("'ACA_CONFIGS_TABS' node hasn't been configured in Standard Choice. Please configure it correctly.");
            }

            IComparer<LinkItem> linkItemComparer = new LinkItemComparer();
            List<TabItem> tabList = new List<TabItem>();

            foreach (ItemValue tab in tabs)
            {
                TabItem tabItem = BuildTabItem(tab);

                // if it is anonymous and the link is set as not anonymous to use. Needn't to add it to list.
                if (ACAUserType.Anonymous.Equals(userType) && !tabItem.IsAnonymousInRoles)
                {
                    continue;
                }

                // if it is register and the link is set as not register to use. Needn't to add it to list.
                if (ACAUserType.Registered.Equals(userType) && !tabItem.IsRegisterInRoles)
                {
                    continue;
                }

                // build sub link item of model
                foreach (ItemValue link in links)
                {
                    // only load the matched sub link, the seperate char '_' is required to distinguish the prefix
                    string moduleName = link.Key.LastIndexOf("_") > -1 ? link.Key.Substring(0, link.Key.LastIndexOf("_")) : link.Key;

                    if (!moduleName.Equals(tab.Key, StringComparison.InvariantCultureIgnoreCase))
                    {
                        continue;
                    }

                    LinkItem subItem = BuildLinkItem(link);

                    // if it is anonymous and the link is set as not anonymous to use. Needn't to add to list.
                    if (ACAUserType.Anonymous.Equals(userType) && !subItem.IsAnonymousInRoles)
                    {
                        continue;
                    }

                    // if it is register and the link is set as not register to use. Needn't to add it to list.
                    if (ACAUserType.Registered.Equals(userType) && !subItem.IsRegisterInRoles)
                    {
                        continue;
                    }

                    // Filter CSV upload link for not provider user
                    if ("APO_UPLOADCSV".Equals(subItem.Key, StringComparison.OrdinalIgnoreCase) && !hasProviderLicense)
                    {
                        continue;
                    }

                    // judge the permission to upload inspection result
                    if (BizDomainConstant.STD_CAT_ACA_CONFIGS_LINKS_UPLOAD_INSPECTION.Equals(subItem.Key, StringComparison.InvariantCultureIgnoreCase) && !User.IsInspector)
                    {
                        continue;
                    }

                    tabItem.Children.Add(subItem);
                }

                // Sorts the sub link of the List using the comparer.
                tabItem.Children.Sort(linkItemComparer);

                tabList.Add(tabItem);
            }

            // Sorts the module of the List using the tabs sort comparer.
            IComparer<TabItem> tabComparer = new TabItemComparer();
            tabList.Sort(tabComparer);

            return tabList;
        }

        /// <summary>
        /// Get Condition Group Name for I18N
        /// </summary>
        /// <param name="agencyCode">Agency Code</param>
        /// <param name="conditionGroup">Condition Group</param>
        /// <returns>Condition Group for I18N</returns>
        public string GetConditionGroupFor18N(string agencyCode, string conditionGroup)
        {
            ICacheManager cacheManager = ObjectFactory.GetObject<ICacheManager>();

            string cacheKey = string.Format(
                "{0}{1}{2}",
                agencyCode,
                ACAConstant.SPLIT_CHAR,
                I18nCultureUtil.AppendUserPreferredCultureFlag(CacheConstant.CACHE_KEY_CONDITION_GROUP));

            Hashtable htStdCates = cacheManager.GetCachedItem(agencyCode, cacheKey);

            if (htStdCates == null)
            {
                return string.Empty;
            }

            string resConditionGroup = htStdCates[conditionGroup] as string;

            return string.IsNullOrEmpty(resConditionGroup) ? conditionGroup : resConditionGroup;
        }

        /// <summary>
        /// Gets value by standard choice of ACA_CONFIG category.
        /// if the key doesn't exist, it returns the defaultValue.
        /// </summary>
        /// <param name="agencyCode">agency code.</param>
        /// <param name="key">standard choice key in ACA_CONFIG category.</param>
        /// <param name="defaultValue">if the key doesn't exist or empty, it returns defaultValue passed by caller.</param>
        /// <returns>standard choice value, return default value if the key can't be found from admin configuration(AA).</returns>
        public string GetValueForACAConfig(string agencyCode, string key, string defaultValue)
        {
            string result = GetValueForACAConfig(agencyCode, key);

            if (!string.IsNullOrEmpty(result))
            {
                return result;
            }
            else
            {
                return defaultValue;
            }
        }

        /// <summary>
        /// Gets value by standard choice of ACA_CONFIG category.
        /// </summary>
        /// <param name="agencyCode">agency code.</param>
        /// <param name="key">standard choice key in ACA_CONFIG category.</param>
        /// <returns>standard choice value, return string.Empty</returns>
        public string GetValueForACAConfig(string agencyCode, string key)
        {
            return GetValueForStandardChoice(agencyCode, BizDomainConstant.STD_CAT_ACA_CONFIGS, key);
        }

        /// <summary>
        /// Gets value by standard choice by given category and key.
        /// </summary>
        /// <param name="agencyCode">agency code.</param>
        /// <param name="category">standard choice category</param>
        /// <param name="key">standard choice key in category.</param>
        /// <returns>standard choice value, return string.Empty</returns>
        public string GetValueForStandardChoice(string agencyCode, string category, string key)
        {
            if (string.IsNullOrEmpty(category) || string.IsNullOrEmpty(key))
            {
                return string.Empty;
            }

            ICacheManager cacheManager = ObjectFactory.GetObject<ICacheManager>();
            string cacheKey = string.Format("{0}{1}{2}", agencyCode, ACAConstant.SPLIT_CHAR, I18nCultureUtil.AppendUserPreferredCultureFlag(CacheConstant.CACHE_KEY_STANDARDCHOICE));
            Hashtable htStdCates = cacheManager.GetCachedItem(agencyCode, cacheKey);

            if (htStdCates == null)
            {
                return string.Empty;
            }

            Hashtable htStdItems = htStdCates[category] as Hashtable;

            if (htStdItems != null && htStdItems.Contains(key))
            {
                if (htStdItems[key] != null)
                {
                    return htStdItems[key].ToString();
                }
                else
                {
                    return string.Empty;
                }
            }
            else
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// Judge whether the biz domain is existed or not.
        /// </summary>
        /// <param name="servProvCode">service provider code</param>
        /// <param name="bizDomain">biz domain name</param>
        /// <param name="bizDomainValues">biz domain value</param>
        /// <returns>message for exist biz domain value</returns>
        public string IsExistBizDomainValue(string servProvCode, string bizDomain, string[] bizDomainValues)
        {
            if (string.IsNullOrEmpty(bizDomain) || bizDomainValues == null || bizDomainValues.Length <= 0)
            {
                return string.Empty;
            }

            try
            {
                return BizDomainService.isExistBizDomainValue(servProvCode, bizDomain, bizDomainValues);
            }
            catch (Exception e)
            {
                throw new ACAException(e);
            }
        }

        /// <summary>
        /// Gets the biz domain value.
        /// </summary>
        /// <param name="agencyCode">The agency code.</param>
        /// <param name="category">The category.</param>
        /// <param name="queryFormat">The query format.</param>
        /// <param name="ignoreLanguage">if set to <c>true</c> [ignore language].</param>
        /// <param name="culture">The culture.</param>
        /// <returns>standard choice list</returns>
        /// <exception cref="ACAException">Exception from web service.</exception>
        public BizDomainModel4WS[] GetBizDomainValue(string agencyCode, string category, QueryFormat4WS queryFormat, bool ignoreLanguage, string culture)
        {
            try
            {
                return BizDomainService.getBizDomainValue(agencyCode, category, queryFormat, ignoreLanguage, I18nCultureUtil.ChangeCulture4WS(culture));
            }
            catch (Exception e)
            {
                throw new ACAException(e);
            } 
        }

        /// <summary>
        /// Define Standard Choice,Create and Edit Biz Domain Model Array.
        /// </summary>
        /// <param name="bizDomainModel4WSArray">BizDomainMode for web service</param>
        /// <param name="ignoreLanguage">ignoring language flag</param>
        /// <returns>true successful, false failure.</returns>
        /// <exception cref="ACAException">Daily side doesn't support to create BizDomain</exception>
        bool IBizDomainBll.CreateAndEditBizDomain(BizDomainModel4WS[] bizDomainModel4WSArray, bool ignoreLanguage)
        {
            throw new ACAException("Daily side doesn't support to create bizdomain.");
        }

        /// <summary>
        /// Method to get biz domain(standard choice) object model
        /// </summary>
        /// <param name="bizDomainModel4WS">BizDomainModel for web service</param>
        /// <param name="callerID">the public user id.</param>
        /// <param name="ignoreLanguage">ignoring language flag</param>
        /// <returns>Biz Domain Model</returns>
        BizDomainModel4WS IBizDomainBll.GetBizDomainListByModel(BizDomainModel4WS bizDomainModel4WS, string callerID, bool ignoreLanguage)
        {
            if (bizDomainModel4WS == null)
            {
                throw new DataValidateException(new string[] { "bizDomainModel4WS" });
            }

            if (string.IsNullOrEmpty(callerID))
            {
                throw new DataValidateException(new string[] { callerID });
            }

            try
            {
                BizDomainModel4WS[] stdItems = BizDomainService.getBizDomainListByModel(bizDomainModel4WS, callerID, ignoreLanguage);

                if (stdItems == null || stdItems.Length == 0)
                {
                    return null;
                }
                else
                {
                    return stdItems[0];
                }
            }
            catch (Exception e)
            {
                throw new ACAException(e);
            }
        }

        /// <summary>
        /// Indicates whether only allow selection one service.
        /// </summary>
        /// <param name="module">module name</param>
        /// <returns>true - single service selection only,false - allow select multiple services.</returns>
        public bool IsSingleServiceOnly(string module)
        {
            List<LinkItem> linkItems = this.GetLinkList(SuperAgencyCode);
            LinkItem item = linkItems.FirstOrDefault(o => o.Url.IndexOf("createRecordByService", StringComparison.InvariantCultureIgnoreCase) > -1
                                                          && string.Equals(o.Module, module, StringComparison.InvariantCultureIgnoreCase));

            if (item != null)
            {
                return item.SingleServiceOnly;
            }

            return true;
        }

        /// <summary>
        /// Indicates whether the url need to force login.
        /// </summary>
        /// <param name="module">module name</param>
        /// <param name="url">url,only file name such as Cap Home page</param>
        /// <param name="urlParameter">url parameter that identify the key with url. Most, it is null.</param>
        /// <returns>true-need force login,false-needn't force login</returns>
        public bool IsForceLogin(string module, string url, string urlParameter)
        {
            string urlFileName = "-" + GetFileNameFromURL(url);

            if (!string.IsNullOrEmpty(module))
            {
                urlFileName = module.ToLower() + urlFileName;
            }

            //Get all tablinks from ACA_CONFIGS_LINKS
            List<LinkItem> links = GetLinkList(SuperAgencyCode);

            //CapDetail follow the force login logic as Cap Search 
            if (url.IndexOf("/capdetail", StringComparison.InvariantCultureIgnoreCase) != -1)
            {
                LinkItem item = links.FirstOrDefault(o => o.Url.IndexOf("caphome.aspx", StringComparison.InvariantCultureIgnoreCase) > -1
                                                          && o.Url.IndexOf("IsToShowInspection", StringComparison.InvariantCultureIgnoreCase) == -1
                                                          && string.Equals(o.Module, module, StringComparison.InvariantCultureIgnoreCase));

                if (item != null)
                {
                    return item.ForceLogin;
                }
            }
            else
            {
                //judge whether the url is contained in the tablinks based module
                foreach (LinkItem item in links)
                {
                    string urlKey = "-" + GetFileNameFromURL(item.Url);

                    if (!string.IsNullOrEmpty(item.Module))
                    {
                        urlKey = item.Module.ToLower() + urlKey;
                    }

                    bool fileNameMatch = urlKey.Equals(urlFileName, StringComparison.InvariantCultureIgnoreCase);
                    bool noUrlPara = string.IsNullOrEmpty(urlParameter);
                    bool urlParaMatch = !noUrlPara && item.Url.Contains(urlParameter);

                    if (fileNameMatch && (noUrlPara || urlParaMatch))
                    {
                        return item.ForceLogin;
                    }
                }
            }

            return false;
        }

        /// <summary>
        /// The standard choice item value of the biz domain.
        /// </summary>
        /// <param name="agencyCode">The agency code</param>
        /// <param name="bizDomain">biz domain name</param>
        /// <param name="bizDomainItemKey">The standard choice item key</param>
        /// <returns>The standard choice item description</returns>
        public string GetBizDomainItemDesc(string agencyCode, string bizDomain, string bizDomainItemKey)
        {
            string result = string.Empty;

            BizDomainModel4WS[] bizList = GetBizDomainValue(agencyCode, bizDomain, new QueryFormat4WS(), false, I18nCultureUtil.UserPreferredCulture);

            if (bizList != null && bizList.Length > 0)
            {
                BizDomainModel4WS bizDomainItem = bizList.FirstOrDefault(o => o.bizdomainValue.Equals(bizDomainItemKey));

                if (bizDomainItem != null)
                {
                    result = !string.IsNullOrEmpty(bizDomainItem.resDescription)
                                 ? bizDomainItem.resDescription
                                 : bizDomainItem.description;
                }
            }

            return result;
        }

        /// <summary>
        /// Build TabLinkItem by standard choice item.
        /// </summary>
        /// <param name="linkItem">LinkItem, it is required not null.</param>
        /// <param name="stdItem">standard choice item.</param>
        private static void BuildItem(ILinkItem linkItem, ItemValue stdItem)
        {
            // each link string accroding below format is configurated in standard choice:
            // label=Permits,order=1,role=0|1,url=/Cap/CapHome.aspx,module=building
            // need to parse the string to TabLinkItem.
            linkItem.Key = stdItem.Key;

            string[] pairs = stdItem.Value.ToString().Split(',');
            Hashtable htPairs = new Hashtable();

            foreach (string pair in pairs)
            {
                //label=Permits,required to be configurated as a pair with "="
                string[] keyValues = pair.Split('=');

                if (keyValues.Length >= 2)
                {
                    int index = pair.IndexOf("=", StringComparison.InvariantCulture) + 1;

                    if (pair.Length > index)
                    {
                        //htPairs.Add(keyValues[0].ToLower().Trim(), pair.Substring(index));
                        htPairs.Add(keyValues[0].ToLowerInvariant().Trim(), pair.Substring(index));
                    }
                }
            }

            if (htPairs.Count > 0)
            {
                // label
                if (htPairs.ContainsKey("label"))
                {
                    linkItem.Label = htPairs["label"].ToString().Trim();
                }

                // order
                if (htPairs.ContainsKey("order"))
                {
                    string order = htPairs["order"].ToString().Trim();
                    if (ValidationUtil.IsNumber(order))
                    {
                        linkItem.Order = int.Parse(order);
                    }
                    else
                    {
                        linkItem.Order = 0;
                    }
                }

                // role
                if (htPairs.ContainsKey("role"))
                {
                    linkItem.Roles = htPairs["role"].ToString().Trim();
                }

                bool hasUrl = htPairs.ContainsKey("url");

                // module
                if (htPairs.ContainsKey("module"))
                {
                    linkItem.Module = htPairs["module"].ToString().Trim();
                }
                else if (hasUrl)
                { 
                    //try to get the module name from url
                    string[] ps = htPairs["url"].ToString().Split(new char[] { '?', '&' });

                    if (ps.Length > 0)
                    {
                        foreach (string p in ps)
                        {
                            string[] m = p.Split('=');

                            if (m.Length == 2 && m[0] == "module")
                            {
                                linkItem.Module = m[1].Trim();
                                break;
                            }
                        }
                    }
                }

                // url
                if (hasUrl)
                {
                    linkItem.Url = RebuildUrl(htPairs["url"].ToString().Trim(), linkItem.Module);
                }

                // forcelogin
                if (htPairs.ContainsKey(FORCELOGIN))
                {
                    string fl = htPairs[FORCELOGIN].ToString().Trim().ToUpperInvariant();

                    if (ValidationUtil.IsYes(fl))
                    {
                        linkItem.ForceLogin = true;
                    }
                    else
                    {
                        linkItem.ForceLogin = false;
                    }
                }

                //single service selection only
                if (htPairs.ContainsKey(SINGLESERVICEONLY))
                {
                    string fl = htPairs[SINGLESERVICEONLY].ToString().Trim().ToUpperInvariant();

                    if (ValidationUtil.IsYes(fl))
                    {
                        linkItem.SingleServiceOnly = true;
                    }
                    else
                    {
                        linkItem.SingleServiceOnly = false;
                    }
                }

                if (linkItem is TabItem)
                {
                    TabItem ti = linkItem as TabItem;

                    // Judge tab whether need to show.
                    if (htPairs.ContainsKey("tabvisible"))
                    {
                        string ts = htPairs["tabvisible"].ToString().Trim().ToUpperInvariant();

                        if (ValidationUtil.IsNo(ts))
                        {
                            ti.TabVisible = false;
                        }
                        else
                        {
                            ti.TabVisible = true;
                        }
                    }

                    // Judge block lin in home page whether need to show.
                    if (htPairs.ContainsKey("blockvisible"))
                    {
                        string bs = htPairs["blockvisible"].ToString().Trim().ToUpperInvariant();
                        
                        if (ValidationUtil.IsNo(bs))
                        {
                            ti.BlockVisible = false;
                        }
                        else
                        {
                            ti.BlockVisible = true;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Build linkItem by standard choice item.
        /// </summary>
        /// <param name="stdItem">standard choice item.</param>
        /// <returns>linkItem object.</returns>
        private static LinkItem BuildLinkItem(ItemValue stdItem)
        {
            LinkItem linkItem = new LinkItem();

            BuildItem(linkItem, stdItem);

            return linkItem;
        }

        /// <summary>
        /// Build Tab item by standard choice item.
        /// </summary>
        /// <param name="stdItem">standard choice item.</param>
        /// <returns>Tab Item object.</returns>
        private static TabItem BuildTabItem(ItemValue stdItem)
        {
            TabItem tabItem = new TabItem();

            BuildItem(tabItem, stdItem);

            return tabItem;
        }

        /// <summary>
        /// Appends the module parameter to url.
        /// </summary>
        /// <param name="mainUrl">the main url</param>
        /// <param name="module">module name.</param>
        /// <returns>a url with module name parameter.</returns>
        private static string RebuildUrl(string mainUrl, string module)
        {
            if (string.IsNullOrEmpty(mainUrl))
            {
                return string.Empty;
            }

            string url = mainUrl;

            if (!url.StartsWith("/", StringComparison.OrdinalIgnoreCase))
            {
                url = "/" + url;
            }

            if (string.IsNullOrEmpty(module))
            {
                return url;
            }

            //judge whether is not exist module in url.
            if (mainUrl.ToUpperInvariant().IndexOf("MODULE=", StringComparison.InvariantCulture) != -1)
            {
                return url;
            }

            // if there is no parameter in url, append "?", otherwise append "&"
            if (url.IndexOf("?", StringComparison.OrdinalIgnoreCase) == -1)
            {
                url += "?";
            }
            else
            {
                url += "&";
            }

            url += "module=" + module;

            return url;
        }

        /// <summary>
        /// This method gets the Tabs from the DB and sorts them in the right order.
        /// </summary>
        /// <param name="agencyCode">The agency code</param>
        /// <returns>Return the list of linkItems</returns>
        private List<LinkItem> GetLinkList(string agencyCode)
        {
            IList<ItemValue> tabs = null;
            IList<ItemValue> links = null;

            try
            {
                tabs = GetBizDomainList(agencyCode, BizDomainConstant.STD_CAT_ACA_CONFIGS_TABS, false, 1);
                links = GetBizDomainList(agencyCode, BizDomainConstant.STD_CAT_ACA_CONFIGS_LINKS, false, 1);
            }
            catch (Exception ex)
            {
                Logger.Fatal(ex);
            }

            if (tabs == null || tabs.Count == 0)
            {
                throw new ACAException("'ACA_CONFIGS_TABS' node hasn't been configurated in Standard Choice. Please configurate it correctly.");
            }

            List<LinkItem> resultList = new List<LinkItem>();

            foreach (ItemValue tab in tabs)
            {
                // Build model level link item
                foreach (ItemValue link in links)
                {
                    if (!link.Key.StartsWith(tab.Key + "_", StringComparison.InvariantCultureIgnoreCase))
                    {
                        continue;
                    }

                    LinkItem item = BuildLinkItem(link);

                    if (item != null && IsURLMatched(item.Url))
                    {
                        resultList.Add(item);
                    }
                }
            }

            return resultList;
        }

        /// <summary>
        /// Determines whether [is URL matched] [the specified current URL].
        /// </summary>
        /// <param name="settingURL">The setting URL.</param>
        /// <returns>
        /// <c>true</c> if [is URL matched] [the specified current URL]; otherwise, <c>false</c>.
        /// </returns>
        private bool IsURLMatched(string settingURL)
        {
            bool result = true;

            if (!string.IsNullOrEmpty(settingURL))
            {
                HttpRequest currentRequest = HttpContext.Current.Request;

                int settingIndexOfSep = settingURL.IndexOf(ACAConstant.QUESTION_MARK);
                string settingFilePath = settingIndexOfSep != -1 ? settingURL.Substring(0, settingIndexOfSep) : string.Empty;
                string settingQueryString = (settingIndexOfSep < settingURL.Length - 1) ? settingURL.Substring(settingIndexOfSep + 1) : string.Empty;
                NameValueCollection settingQueryCollection = HttpUtility.ParseQueryString(settingQueryString);

                if (settingQueryCollection == null)
                {
                    settingQueryCollection = new NameValueCollection();
                }

                if (currentRequest.FilePath.IndexOf("CapHome.aspx") != -1 && settingFilePath.IndexOf("CapHome.aspx") != -1)
                {
                    result = false;

                    if (string.IsNullOrEmpty(currentRequest.QueryString["IsToShowInspection"]) && string.IsNullOrEmpty(settingQueryCollection["IsToShowInspection"]))
                    {
                        result = true;
                    }
                    else if (!string.IsNullOrEmpty(currentRequest.QueryString["IsToShowInspection"]) && !string.IsNullOrEmpty(settingQueryCollection["IsToShowInspection"]))
                    {
                        result = true;
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// Get File Name From URL.
        /// </summary>
        /// <param name="url">the url string.</param>
        /// <returns>the file name.</returns>
        private string GetFileNameFromURL(string url)
        {
            if (string.IsNullOrEmpty(url) || url.IndexOf("/") < 0)
            {
                return string.Empty;
            }

            string subStringURL = string.Empty;
            
            //get sub url before Character "?", if there have none , get all url.
            if (url.IndexOf("?") < 0) 
            {
                subStringURL = url;
            }
            else
            {
                subStringURL = url.Substring(0, url.IndexOf("?"));
            }

            int lengthStart = subStringURL.LastIndexOf("/") + 1; //get sub url after character "/".
            subStringURL = subStringURL.Substring(lengthStart);

            return subStringURL;
        }

        #endregion Methods
    }
}
