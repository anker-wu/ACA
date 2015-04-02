#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: AdminBizDomainBll.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2008-2014
 *
 *  Description:
 *
 *  Notes:
 * $Id: AdminBizDomainBll.cs 277080 2014-08-11 06:28:07Z ACHIEVO\james.shi $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using Accela.ACA.BLL.Common;
using Accela.ACA.Common;
using Accela.ACA.Common.Log;
using Accela.ACA.Common.Util;
using Accela.ACA.WSProxy;

using log4net;

namespace Accela.ACA.AdminBLL
{
    /// <summary>
    /// This class provide the ability to get standard choice value.
    /// </summary>
    public class AdminBizDomainBll : IBizDomainBll
    {
        #region Fields

        /// <summary>
        /// need record type filter.
        /// </summary>
        private const string NEEDFILTER = "needfilter";

        /// <summary>
        /// Logger object.
        /// </summary>
        private static readonly ILog Logger = LogFactory.Instance.GetLogger(typeof(AdminBizDomainBll));

        #endregion Fields

        #region Properties

        /// <summary>
        /// Gets collection that biz model description field as item value.
        /// </summary>
        /// <value>The description asynchronous value.</value>
        private static IList<string> DescriptionAsValue
        {
            get
            {
                IList<string> bizDomains = new List<string>()
                {
                    BizDomainConstant.STD_CAT_GENDER,
                    BizDomainConstant.STD_CAT_COUNTRY,
                    BizDomainConstant.STD_CAT_ACA_PAGE_PICKER,
                    BizDomainConstant.STD_CAT_ACA_CONFIGS_TABS,
                    BizDomainConstant.STD_CAT_ACA_CONFIGS_LINKS,
                    BizDomainConstant.STD_CONTACT_PREFERRED_CHANNEL
                };

                return bizDomains;
            }
        }

        /// <summary>
        /// Gets an instance of Standard Choices service.
        /// </summary>
        /// <value>The biz domain service.</value>
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
        /// <param name="bizDomains">BizDomainMode for web service</param>
        /// <returns>true successful, false failure.</returns>
        public bool CreateAndEditBizDomain(BizDomainModel4WS[] bizDomains)
        {
            return CreateAndEditBizDomain(bizDomains, false);
        }

        /// <summary>
        /// Define Standard Choice,Create and Edit Biz Domain Model Array.
        /// </summary>
        /// <param name="bizDomains">BizDomainMode for web service</param>
        /// <param name="ignoreLanguage">ignoring language flag</param>
        /// <returns>true successful, false failure.</returns>
        /// <exception cref="DataValidateException">new string[] { bizDomains }</exception>
        /// <exception cref="ACAException"></exception>
        public bool CreateAndEditBizDomain(BizDomainModel4WS[] bizDomains, bool ignoreLanguage)
        {
            if (Logger.IsDebugEnabled)
            {
                Logger.Debug("Begin BizDomainBll.CreateAndEditBizDomain()");
            }

            if (bizDomains == null)
            {
                throw new DataValidateException(new string[] { "bizDomains" });
            }

            try
            {
                BizDomainService.createAndEditBizDomain(bizDomains, ignoreLanguage);

                if (Logger.IsDebugEnabled)
                {
                    Logger.Debug("End BizDomainBll.CreateAndEditBizDomain()");
                }

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
        /// <param name="bizDomainModel4WS">Standard Choice object</param>
        /// <returns>true successful, false failure.</returns>
        /// <exception cref="DataValidateException">new string[] { bizDomainModel4WS }</exception>
        /// <exception cref="ACAException"></exception>
        public bool CreateBizDomain(BizDomainModel4WS bizDomainModel4WS)
        {
            if (Logger.IsDebugEnabled)
            {
                Logger.Debug("Begin BizDomainBll.CreateBizDomain()");
            }

            if (bizDomainModel4WS == null)
            {
                throw new DataValidateException(new string[] { "bizDomainModel4WS" });
            }

            try
            {
                bool response = BizDomainService.createBizDomain(bizDomainModel4WS, false);

                if (Logger.IsDebugEnabled)
                {
                    Logger.Debug("End BizDomainBll.CreateBizDomain()");
                }

                return response;
            }
            catch (Exception e)
            {
                throw new ACAException(e);
            }
        }

        /// <summary>
        /// Define Standard Choice and Update.
        /// </summary>
        /// <param name="bizDomainModel4WS">Standard Choice object</param>
        /// <returns>true successful, false failure.</returns>
        /// <exception cref="DataValidateException">new string[] { bizDomainModel4WS }</exception>
        /// <exception cref="ACAException"></exception>
        public bool EditBizDomain(BizDomainModel4WS bizDomainModel4WS)
        {
            if (Logger.IsDebugEnabled)
            {
                Logger.Debug("Begin BizDomainBll.EditBizDomain()");
            }

            if (bizDomainModel4WS == null)
            {
                throw new DataValidateException(new string[] { "bizDomainModel4WS" });
            }

            try
            {
                bool response = BizDomainService.editBizDomain(bizDomainModel4WS, false);

                if (Logger.IsDebugEnabled)
                {
                    Logger.Debug("End BizDomainBll.EditBizDomain()");
                }

                return response;
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
        /// <param name="includeInactivity">if include inactivity items</param>
        /// <returns>ItemValue list.</returns>
        public IList<ItemValue> GetBizDomainList(string agencyCode, string bizName, bool includeInactivity)
        {
            //biz model description field as item value 
            if (DescriptionAsValue.Contains(bizName))
            {
                //show value /res Decription
                return GetBizDomainList(agencyCode, bizName, includeInactivity, 3);
            }
            else
            {
                //show value / res value
                return GetBizDomainList(agencyCode, bizName, includeInactivity, 0);
            }
        }

        /// <summary>
        /// Method to get biz domain(standard choice) object list.
        /// </summary>
        /// <param name="agencyCode">agency code</param>
        /// <param name="includeInactivity">if include inactivity items</param>
        /// <param name="contactTypeSource">The contact type source.</param>
        /// <returns>ItemValue list.</returns>
        /// <exception cref="DataValidateException">new string[] { agencyCode }</exception>
        /// <exception cref="ACAException"></exception>
        public IList<ItemValue> GetContactTypeList(string agencyCode, bool includeInactivity, string contactTypeSource)
        {
            if (string.IsNullOrEmpty(agencyCode))
            {
                throw new DataValidateException(new string[] { "agencyCode" });
            }

            IList<ItemValue> items = new List<ItemValue>();
            try
            {
                BizDomainModel4WS model = new BizDomainModel4WS();
                model.bizdomain = BizDomainConstant.STD_CAT_CONTACT_TYPE;
                model.serviceProviderCode = agencyCode;

                if (!includeInactivity)
                {
                    model.auditStatus = ACAConstant.VALID_STATUS;
                }

                BizDomainModel4WS[] models = BizDomainService.getBizDomainListByModel(model, "Admin User", false);

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
        /// <param name="bizName">Standard Choice name</param>
        /// <param name="includeInactivity">if include inactivity items</param>
        /// <param name="showType">indicates what value is used for Standard Choice, value,value description or both</param>
        /// <returns>Item Value list.</returns>
        /// <exception cref="DataValidateException">new string[] { agencyCode, bizName }</exception>
        /// <exception cref="ACAException"></exception>
        public IList<ItemValue> GetBizDomainList(string agencyCode, string bizName, bool includeInactivity, int showType)
        {
            if (Logger.IsDebugEnabled)
            {
                Logger.Debug("Begin AdminBizDomainBll.GetBizDomainList()");
            }

            if (string.IsNullOrEmpty(agencyCode) || string.IsNullOrEmpty(bizName))
            {
                throw new DataValidateException(new string[] { "agencyCode", bizName });
            }

            IList<ItemValue> items = new List<ItemValue>();
            try
            {
                BizDomainModel4WS model = new BizDomainModel4WS();
                model.bizdomain = bizName;
                model.serviceProviderCode = agencyCode;

                if (!includeInactivity)
                {
                    model.auditStatus = ACAConstant.VALID_STATUS;
                }

                BizDomainModel4WS[] models = BizDomainService.getBizDomainListByModel(model, "Admin User", false);

                if (models != null && models.Length > 0)
                {
                    foreach (BizDomainModel4WS bdm in models)
                    {
                        ItemValue item = new ItemValue();

                        switch (showType)
                        {
                            case 1: // show description (include country,salutation)
                                item.Value = I18nStringUtil.GetString(bdm.resDescription, bdm.description);

                                item.Key = bdm.bizdomainValue + "||" + I18nStringUtil.GetString(bdm.resBizdomainValue, bdm.bizdomainValue) + "||" + bdm.description + "||"
                                           + I18nStringUtil.GetString(bdm.resDescription, bdm.description);
                                break;
                            case 2: // show value - description (include constuction type)
                                item.Value = string.Format("{0} - {1}", I18nStringUtil.GetString(bdm.resBizdomainValue, bdm.bizdomainValue), I18nStringUtil.GetString(bdm.resDescription, bdm.description));
                                item.Key = bdm.bizdomainValue + "||" + I18nStringUtil.GetString(bdm.resBizdomainValue, bdm.bizdomainValue) + "||" + bdm.description + "||"
                                           + I18nStringUtil.GetString(bdm.resDescription, bdm.description);
                                break;
                            case 3: // key = STD.value, value = STD.description
                                item.Value = I18nStringUtil.GetString(bdm.resDescription, bdm.description);
                                item.Key = bdm.bizdomainValue;
                                break;
                            default: //show bizdomianValue
                                item.Value = I18nStringUtil.GetString(bdm.resBizdomainValue, bdm.bizdomainValue);
                                item.Key = bdm.bizdomainValue;
                                break;
                        }

                        items.Add(item);
                    }

                    ((List<ItemValue>)items).Sort(ItemValueComparer.Instance);
                }

                if (Logger.IsDebugEnabled)
                {
                    Logger.Debug("End AdminBizDomainBll.GetBizDomainList()");
                }

                return items;
            }
            catch (Exception e)
            {
                throw new ACAException(e);
            }
        }

        /// <summary>
        /// Gets biz domain(standard choice) object model.
        /// </summary>
        /// <param name="bizDomainModel4WS">Standard Choice object</param>
        /// <param name="callerID">caller id.</param>
        /// <returns>Standard Choice information</returns>
        /// <exception cref="DataValidateException">
        /// new string[] { bizDomainModel4WS }
        /// or
        /// </exception>
        /// <exception cref="ACAException"></exception>
        public BizDomainModel4WS GetBizDomainListByModel(BizDomainModel4WS bizDomainModel4WS, string callerID)
        {
            if (Logger.IsDebugEnabled)
            {
                Logger.Debug("Begin BizDomainBll.GetBizDomainListByModel()");
            }

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
        /// Gets biz domain(standard choice) object model.
        /// </summary>
        /// <param name="bizDomain">BizDomainModel for web service</param>
        /// <param name="key">key value.</param>
        /// <param name="callerID">caller id.</param>
        /// <returns>Biz Domain Model</returns>
        /// <exception cref="DataValidateException">
        /// new string[] { bizDomainModel4WS }
        /// or
        /// </exception>
        /// <exception cref="ACAException"></exception>
        public BizDomainModel4WS GetBizDomainListByModel(BizDomainModel4WS bizDomain, string key, string callerID)
        {
            if (Logger.IsDebugEnabled)
            {
                Logger.Debug("Begin BizDomainBll.GetBizDomainListByModel()");
            }

            if (bizDomain == null)
            {
                throw new DataValidateException(new string[] { "bizDomainModel4WS" });
            }

            if (string.IsNullOrEmpty(key) || string.IsNullOrEmpty(callerID))
            {
                throw new DataValidateException(new string[] { key, callerID });
            }

            try
            {
                BizDomainModel4WS bizWS = new BizDomainModel4WS();
                BizDomainModel4WS[] stdItems = BizDomainService.getBizDomainListByModel(bizDomain, callerID, false);

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

                if (Logger.IsDebugEnabled)
                {
                    Logger.Debug("End BizDomainBll.GetBizDomainListByModel()");
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
        /// <exception cref="DataValidateException">new string[] { agencyCode, bizName }</exception>
        /// <exception cref="ACAException"></exception>
        public string[] GetBizDomainValueList(string agencyCode, string bizName)
        {
            if (Logger.IsDebugEnabled)
            {
                Logger.Debug("Begin BizDomainBll.getBizDomainValueList()");
            }

            if (string.IsNullOrEmpty(agencyCode) || string.IsNullOrEmpty(bizName))
            {
                throw new DataValidateException(new string[] { "agencyCode", bizName });
            }

            try
            {
                ICacheManager cacheManager = ObjectFactory.GetObject<ICacheManager>();
                string cacheKey = agencyCode + ACAConstant.SPLIT_CHAR + I18nCultureUtil.AppendUserPreferredCultureFlag(CacheConstant.CACHE_KEY_STANDARDCHOICE);
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

                if (Logger.IsDebugEnabled)
                {
                    Logger.Debug("End BizDomainBll.getBizDomainValueList()");
                }

                return result;
            }
            catch (Exception e)
            {
                throw new ACAException(e);
            }
        }

        /// <summary>
        /// Judge whether the biz domain is existed or not.
        /// </summary>
        /// <param name="servProvCode">service provider code</param>
        /// <param name="bizDomain">biz domain name</param>
        /// <param name="bizDomainValues">biz domain value</param>
        /// <returns>message for exist biz domain value</returns>
        /// <exception cref="ACAException">all web service exception form java</exception>
        public string IsExistBizDomainValue(string servProvCode, string bizDomain, string[] bizDomainValues)
        {
            if (string.IsNullOrEmpty(bizDomain) || bizDomainValues == null || bizDomainValues.Length <= 0)
            {
                return string.Empty;
            }

            try
            {
                string isExistMessage = BizDomainService.isExistBizDomainValue(servProvCode, bizDomain, bizDomainValues);

                return isExistMessage;
            }
            catch (Exception e)
            {
                throw new ACAException(e);
            }
        }

        /// <summary>
        /// This Configured Url of link button.
        /// </summary>
        /// <returns>string for configured url, String.Empty for admin implementation.</returns>
        public string GetConfiguredUrlFromXPolicy()
        {
            return string.Empty;
        }

        /// <summary>
        /// This method gets the Tabs from the DB and sorts them in the right order.
        /// </summary>
        /// <param name="agencyCode">the agency code</param>
        /// <param name="userType">ACA user type</param>
        /// <param name="hasProviderLicense">if set to <c>true</c> [has provider license].</param>
        /// <returns>Sorted List of tabs</returns>
        /// <exception cref="ACAException">'ACA_CONFIGS_TABS' node hasn't been configured in Standard Choice. Please configure it correctly.</exception>
        public IList<TabItem> GetTabsList(string agencyCode, ACAUserType userType, bool hasProviderLicense)
        {
            IList<ItemValue> tabs = GetBizDomainList(agencyCode, BizDomainConstant.STD_CAT_ACA_CONFIGS_TABS, false);
            IList<ItemValue> links = GetBizDomainList(agencyCode, BizDomainConstant.STD_CAT_ACA_CONFIGS_LINKS, false);

            if (tabs == null || tabs.Count == 0)
            {
                throw new ACAException("'ACA_CONFIGS_TABS' node hasn't been configurated in Standard Choice. Please configurate it correctly.");
            }

            IComparer<LinkItem> linkItemComparer = new LinkItemComparer();
            List<TabItem> tabList = new List<TabItem>();

            // each tab string accroding below format:
            // label=Permits,order=1,role=0|1,url=/Cap/CapHome.aspx,module=building
            // need to parse the string to TabLinkItem.
            foreach (ItemValue tab in tabs)
            {
                // Build model level link item
                TabItem tabItem = BuildTabItem(tab);

                // if it is anonymous and the link is set as not anonymous to use. Needn't to add to list.
                if (ACAUserType.Anonymous.Equals(userType) && !tabItem.IsAnonymousInRoles)
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

                    tabItem.Children.Add(subItem);
                }

                // Sorts the sub link of the List using the comparer.
                tabItem.Children.Sort(linkItemComparer);

                tabList.Add(tabItem);
            }

            // Sorts the module of the List using the tabs sort comparer.
            IComparer<TabItem> tabComparer = new TabItemComparer();
            tabList.Sort(tabComparer);

            ITransactionLogBll transLogBll = ObjectFactory.GetObject<ITransactionLogBll>();
            transLogBll.AddTransactionLog("BizDomainBll.cs", "GetTabsList", "Successfully");

            return tabList;
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
        /// Get Condition Group Name for I18N
        /// </summary>
        /// <param name="agencyCode">Agency Code</param>
        /// <param name="conditionGroup">Condition Group</param>
        /// <returns>Condition Group for I18N</returns>
        public string GetConditionGroupFor18N(string agencyCode, string conditionGroup)
        {
            return string.Empty;
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
            string cacheKey = agencyCode + ACAConstant.SPLIT_CHAR + I18nCultureUtil.AppendUserPreferredCultureFlag(CacheConstant.CACHE_KEY_STANDARDCHOICE);
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
        /// Gets the biz domain value.
        /// </summary>
        /// <param name="agencyCode">The agency code.</param>
        /// <param name="category">The category.</param>
        /// <param name="queryFormat4Ws">The query format.</param>
        /// <param name="ignoreLanguage">if set to <c>true</c> [ignore language].</param>
        /// <param name="culture">The culture.</param>
        /// <returns>Standard Choices information</returns>
        /// <exception cref="ACAException">exception from web service</exception>
        public BizDomainModel4WS[] GetBizDomainValue(string agencyCode, string category, QueryFormat4WS queryFormat4Ws, bool ignoreLanguage, string culture)
        {
            try
            {
                return BizDomainService.getBizDomainValue(agencyCode, category, queryFormat4Ws, ignoreLanguage, I18nCultureUtil.ChangeCulture4WS(culture));
            }
            catch (Exception e)
            {
                throw new ACAException(e);
            }
        }

        /// <summary>
        /// Method to get biz domain(standard choice) object model
        /// </summary>
        /// <param name="bizDomainModel4WS">BizDomainModel for web service</param>
        /// <param name="callerID">caller id.</param>
        /// <param name="ignoreLanguage">ignoring language flag</param>
        /// <returns>Biz Domain Model</returns>
        /// <exception cref="DataValidateException">
        /// new string[] { bizDomainModel4WS }
        /// or
        /// </exception>
        /// <exception cref="ACAException"></exception>
        BizDomainModel4WS IBizDomainBll.GetBizDomainListByModel(BizDomainModel4WS bizDomainModel4WS, string callerID, bool ignoreLanguage)
        {
            if (Logger.IsDebugEnabled)
            {
                Logger.Debug("Begin BizDomainBll.GetBizDomainListByModel()");
            }

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
        /// Indicates whether single service selection only.
        /// </summary>
        /// <param name="module">module name</param>
        /// <returns>true - single service selection only,false - allow select multiple services.</returns>
        public bool IsSingleServiceOnly(string module)
        {
            // this metord is no use, it is only use to implement the IBizDomainBll interface.
            return false;
        }

        /// <summary>
        /// Indicates whether the url need to force login.
        /// </summary>
        /// <param name="module">module name</param>
        /// <param name="url">url,only file name such as CapHome page</param>
        /// <param name="urlParameter">url parameter that identify the key with url. Most, it is null.</param>
        /// <returns>true-need force login,false-needn't force login</returns>
        public bool IsForceLogin(string module, string url, string urlParameter)
        {
            // this metord is no use, it is only use to implement the IBizDomainBll interface.
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
        /// Builds the item.
        /// </summary>
        /// <param name="linkItem">The link item.</param>
        /// <param name="stdItem">The standard item.</param>
        private static void BuildItem(ILinkItem linkItem, ItemValue stdItem)
        {
            // each link string accroding below format is configurated in standard choice:
            // label=Permits,order=1,role=0|1,url=/Cap/CapHome.aspx,module=building
            // need to parse the string to TabLinkItem.
            linkItem.Key = stdItem.Key;

            string[] pairs = stdItem.Value.ToString().Split(',');

            if (pairs == null || pairs.Length == 0)
            {
                return;
            }

            Hashtable htPairs = new Hashtable();

            foreach (string pair in pairs)
            {
                //label=Permits,required to be configurated as a pair with "="
                string[] keyValues = pair.Split('=');

                if (keyValues != null && keyValues.Length >= 2)
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

                // needFilter
                if (htPairs.ContainsKey(NEEDFILTER))
                {
                    string nf = htPairs[NEEDFILTER].ToString().Trim().ToUpperInvariant();

                    if (ValidationUtil.IsYes(nf))
                    {
                        linkItem.NeedFilter = true;
                    }
                    else
                    {
                        linkItem.NeedFilter = false;
                    }
                }

                if (linkItem is TabItem)
                {
                    TabItem ti = linkItem as TabItem;

                    // Judge tab whether need to show.
                    if (htPairs.ContainsKey("tabvisible"))
                    {
                        string ts = htPairs["tabvisible"].ToString().Trim();

                        if (string.Compare(ts, ACAConstant.COMMON_NO, StringComparison.InvariantCultureIgnoreCase) == 0 || string.Compare(ts, ACAConstant.COMMON_N, StringComparison.InvariantCultureIgnoreCase) == 0)
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
                        string bs = htPairs["blockvisible"].ToString().Trim();

                        if (string.Compare(bs, ACAConstant.COMMON_NO, StringComparison.InvariantCultureIgnoreCase) == 0 || string.Compare(bs, ACAConstant.COMMON_N, StringComparison.InvariantCultureIgnoreCase) == 0)
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
        /// <param name="mainUrl">main url.</param>
        /// <param name="module">module name.</param>
        /// <returns>a url with module name parameter.</returns>
        private static string RebuildUrl(string mainUrl, string module)
        {
            if (string.IsNullOrEmpty(mainUrl))
            {
                return string.Empty;
            }

            string url = mainUrl;

            if (!url.StartsWith("/", StringComparison.InvariantCulture))
            {
                url = "/" + url;
            }

            if (string.IsNullOrEmpty(module))
            {
                return url;
            }

            // if there is no parameter in url, append "?", otherwise append "&"
            if (url.IndexOf("?", StringComparison.InvariantCulture) == -1)
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

        #endregion Methods
    }
}
