#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: CacheManager.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2007-2014
 *
 *  Description:
 *
 *  Notes:
 * $Id: CacheManager.cs 277237 2014-08-13 01:17:52Z ACHIEVO\james.shi $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 *   10/12/2007      Daly zeng
 *   11/27/2008      Jackie Yu                  Removed configurable cache expiration interval
 * </pre>
 */

#endregion Header

using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Timers;
using System.Web;
using System.Web.Caching;

using Accela.ACA.BLL.APO;
using Accela.ACA.Common;
using Accela.ACA.Common.Log;
using Accela.ACA.Common.Util;
using Accela.ACA.Web.Common;
using Accela.ACA.WSProxy;
using log4net;

namespace Accela.ACA.BLL.Common
{
    /// <summary>
    /// Cache Manager, Provided cache ability for all need to be required in ACA.
    /// </summary>
    public class CacheManager : BaseBll, ICacheManager
    {
        #region Fields

        /// <summary>
        /// the interval for time elapsed(2 minutes)
        /// </summary>
        private const int TIME_INTERVAL = 2 * 60 * 1000;

        /// <summary>
        /// default cache expiration.
        /// </summary>
        private const string DEFAULT_CACHE_EXPIRATION = "DefaultCacheExpiration"; // the node name in web.config for cache.

        /// <summary>
        /// the default expiration time if there is no any setting in web.config (minutes,1440minutes = 24 hours)
        /// </summary>
        private const int DEFAULT_EXPIRATION_TIME = 1440;

        /// <summary>
        /// minutes. don't allow the expiration interval less than 30 minutes to ensure ACA performance
        /// </summary>
        private const int MIN_EXPIRATION_TIME_BY_MINUTES = 30;

        /// <summary>
        /// logger object.
        /// </summary>
        private static readonly ILog Logger = LogFactory.Instance.GetLogger(typeof(CacheManager));

        /// <summary>
        /// The local cache status
        /// </summary>
        private static readonly IList<ItemValue> LocalCacheStatus = new List<ItemValue>();
        
        /// <summary>
        /// the timer for execute upload file event.
        /// </summary>
        private static readonly Timer UploadiTimer = new Timer();

        #endregion Fields

        /// <summary>
        /// Initializes static members of the <see cref="CacheManager"/> class.
        /// </summary>
        static CacheManager()
        {
            StartTimer();
        }

        #region Properties
        #endregion Properties

        #region Methods

        #region Public Methods

        /// <summary>
        /// Clear cache according to cacheKeys.
        /// </summary>
        /// <param name="cacheKeys">the cache key needs to be clear.</param>
        public void ClearCache(string[] cacheKeys)
        {
            if (cacheKeys == null || cacheKeys.Length == 0)
            {
                return;
            }

            if (Logger.IsDebugEnabled)
            {
                StringBuilder sbChangedCache = new StringBuilder("Refresh Cache: ");

                foreach (string cacheKey in cacheKeys)
                {
                    sbChangedCache.Append(cacheKey).Append("||");
                }

                Logger.Debug(sbChangedCache.ToString());
            }

            IDictionaryEnumerator cacheEnum = HttpRuntime.Cache.GetEnumerator();

            while (cacheEnum.MoveNext())
            {
                // because all cache key appends the language flag, so only need to mach prefix
                if (cacheKeys.Any(o => cacheEnum.Key.ToString().Contains(o) ||
                    cacheEnum.Key.ToString().Contains(AgencyCode + ACAConstant.SPLIT_CHAR + I18nCultureUtil.AppendUserPreferredCultureFlag(o))))
                {
                    HttpRuntime.Cache.Remove(cacheEnum.Key.ToString());
                    Logger.DebugFormat("Cache [{0}] is clear by Timer event.", cacheEnum.Key);
                }
            }

            /*
             * Auto clear Contact Type cache if Standard Choice be changed in AA Admin.
             */
            string contactTypeKey = AgencyCode + ACAConstant.SPLIT_CHAR
                                    + I18nCultureUtil.AppendUserPreferredCultureFlag(CacheConstant.CACHE_CONTACT_TYPE);

            if (cacheKeys.Contains(CacheConstant.CACHE_KEY_STANDARDCHOICE)
                && !cacheKeys.Contains(CacheConstant.CACHE_CONTACT_TYPE)
                && HttpRuntime.Cache.Get(contactTypeKey) != null)
            {
                HttpRuntime.Cache.Remove(contactTypeKey);
            }

            //update field after clear cache, this behavior means add cache hit ratio.
            I18nCultureUtil.I18nPrimarySetting = ObjectFactory.GetObject<II18nSettingsProvider>().GetI18nPrimarySettings();
        }

        /// <summary>
        /// Cache others
        /// </summary>
        /// <param name="key">string key</param>
        /// <param name="value">object value</param>
        /// <param name="expireTime">the expiration time(minutes)</param>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void AddSingleItemToCache(string key, object value, int expireTime)
        {
            if (HttpRuntime.Cache.Get(key) != null)
            {
                return;
            }

            CacheSettings setting = new CacheSettings();
            setting.Key = key;
            setting.ExpireTime = expireTime;

            AddSingleCacheItemToCache(setting, value, CacheItemPriority.NotRemovable);

            if (Logger.IsDebugEnabled)
            {
                Logger.Debug("Add single item to cache");
            }
        }

        /// <summary>
        /// Load Cache Item.(1.GUI/XUI_TEXT,2.standard choice,3.XPolicy)
        /// </summary>
        public void LoadGlobalCache()
        {
            string cacheKey = string.Empty;
            List<string> cultureList = I18nCultureUtil.GetSupportedCultureList();

            foreach (string culture in cultureList)
            {
                cacheKey = AgencyCode + ACAConstant.SPLIT_CHAR + I18nCultureUtil.AppendCultureFlag(CacheConstant.CACHE_KEY_LABEL, culture);
                AddLabelsToCache(AgencyCode, cacheKey, culture);

                cacheKey = AgencyCode + ACAConstant.SPLIT_CHAR + I18nCultureUtil.AppendCultureFlag(CacheConstant.CACHE_KEY_STANDARDCHOICE, culture);
                AddStandardChoicesToCache(AgencyCode, cacheKey, culture);

                cacheKey = AgencyCode + ACAConstant.SPLIT_CHAR + I18nCultureUtil.AppendCultureFlag(CacheConstant.CACHE_KEY_ALL_POLICY, culture) + "_Daily";
                AddPoliciesToCache(AgencyCode, cacheKey, culture);

                cacheKey = AgencyCode + ACAConstant.SPLIT_CHAR + I18nCultureUtil.AppendCultureFlag(CacheConstant.CACHE_KEY_POLICY, culture);
                AddPolicyToCache(AgencyCode, cacheKey, culture);
            }
        }

        /// <summary>
        /// Get required for document types.
        /// </summary>
        /// <param name="searchModel">search model</param>
        /// <param name="cacheKey">the cache key</param>
        /// <returns>Hashtable that stores required for document types model.</returns>
        public Hashtable GetRequiredDocumentTypes(RefRequiredDocumentModel searchModel, string cacheKey)
        {
            string key = AgencyCode +
                         ACAConstant.SPLIT_CHAR +
                         I18nCultureUtil.AppendUserPreferredCultureFlag(CacheConstant.CACHE_KEY_REQUIRED_DOCUMENT_TYPE);

            Hashtable resultHashTable = HttpRuntime.Cache.Get(key) as Hashtable;

            if (resultHashTable != null && resultHashTable.ContainsKey(cacheKey))
            {
                return resultHashTable;
            }

            RefRequiredDocumentModel[] innerEntry = CacheContentProvider.GetRequiredDocumentTypes(searchModel);

            if (resultHashTable != null)
            {
                if (innerEntry != null)
                {
                    resultHashTable.Add(cacheKey, innerEntry);
                }
            }
            else
            {
                resultHashTable = new Hashtable { { cacheKey, innerEntry } };
                AddCacheItemToCache(key, resultHashTable);
            }

            return resultHashTable;
        }

        /// <summary>
        /// return a cached item by the given key. The key list as below:
        /// STD_CACHE_KEY_LABEL,
        /// STD_CACHE_KEY_STANDARDCHOICE,
        /// STD_CACHE_KEY_TEMPLATE,
        /// STD_CACHE_KEY_SERVERCONSTANT
        /// </summary>
        /// <param name="agencyCode">Agency Code</param>
        /// <param name="key">string cache key</param>
        /// <returns>The retrieved cache item, or null if the key is not found.</returns>
        public Hashtable GetCachedItem(string agencyCode, string key)
        {
            agencyCode = GetAgencyCode(agencyCode, key);

            if (HttpRuntime.Cache.Get(key) != null)
            {
                return HttpRuntime.Cache.Get(key) as Hashtable;
            }

            string originalKey = string.Empty;
            string culture = string.Empty;
            I18nCultureUtil.SplitKey(key, agencyCode, ref originalKey, ref culture);

            //if cache is expirated, load data from web service to cache.
            // the cache data isn't allowed to be cached as null. if there is no data,
            // the cached data should be an empty hashtable but not null,otherwise it may bring on dead loop.
            if (originalKey.Equals(CacheConstant.CACHE_KEY_LABEL, StringComparison.InvariantCultureIgnoreCase))
            {
                AddLabelsToCache(agencyCode, key, culture);
            }
            else if (originalKey.Equals(CacheConstant.CACHE_KEY_I18N_PRIMARY_SETTINGS, StringComparison.InvariantCultureIgnoreCase))
            {
                AddI18nPrimarySettingsToCache(agencyCode, key);
            }
            else if (originalKey.Equals(CacheConstant.CACHE_KEY_I18N_LOCALE_RELEVANT_SETTINGS, StringComparison.InvariantCultureIgnoreCase))
            {
                culture = I18nCultureUtil.ChangeCulture4WS(culture);
                AddI18nLocaleRelevantSettingsToCache(agencyCode, key, culture);
            }
            else if (originalKey.Equals(CacheConstant.CACHE_KEY_GLOBALI18NSETTINGS, StringComparison.InvariantCultureIgnoreCase))
            {
                AddGlobalI18nSettingsToCache(agencyCode, key, culture);
            }
            else if (originalKey.Equals(CacheConstant.CACHE_KEY_STANDARDCHOICE, StringComparison.InvariantCultureIgnoreCase))
            {
                AddStandardChoicesToCache(agencyCode, key, culture);
            }
            else if (originalKey.Equals(CacheConstant.CACHE_KEY_HARDCODE, StringComparison.InvariantCultureIgnoreCase))
            {
                AddHardCodeDropDownListsToCache(agencyCode, key);
            }
            else if (originalKey.Equals(CacheConstant.CACHE_KEY_TEMPLATE, StringComparison.InvariantCultureIgnoreCase))
            {
                AddTemplatesToCache(agencyCode, key);
            }
            else if (originalKey.Equals(CacheConstant.CACHE_KEY_SERVERCONSTANT, StringComparison.InvariantCultureIgnoreCase))
            {
                AddServerConstantsToCache(agencyCode, key);
            }
            else if (CacheConstant.CACHE_KEY_PUBLICUSERGROUP.Equals(originalKey, StringComparison.InvariantCultureIgnoreCase))
            {
                AddPublicUserGroupsToCache(agencyCode, key);
            }
            else if (originalKey.Equals(CacheConstant.CACHE_KEY_PAGEFLOWGROUP, StringComparison.InvariantCultureIgnoreCase))
            {
                AddPageflowGroupToCache(agencyCode, key);
            }
            else if (originalKey.Equals(CacheConstant.CACHE_KEY_CAPTYPEFILTER, StringComparison.InvariantCultureIgnoreCase))
            {
                AddCapTypeFilterToCache(agencyCode, key);
            }
            else if (originalKey.Equals(CacheConstant.CACHE_KEY_EMSEEVENT, StringComparison.InvariantCultureIgnoreCase))
            {
                AddEMSEEventCofigToCache(agencyCode, key);
            }
            else if (originalKey.Equals(CacheConstant.CACHE_KEY_POLICY, StringComparison.InvariantCultureIgnoreCase))
            {
                AddPolicyToCache(agencyCode, key, culture);
            }
            else if (originalKey.Equals(CacheConstant.CACHE_KEY_XPOLICY, StringComparison.InvariantCultureIgnoreCase))
            {
                AddXPolicyToCache(agencyCode, key);
            }
            else if (originalKey.Equals(CacheConstant.CACHE_KEY_ALL_POLICY, StringComparison.InvariantCultureIgnoreCase))
            {
                AddPoliciesToCache(agencyCode, key, culture);
            }
            else if (originalKey.Equals(CacheConstant.CACHE_KEY_LOGO, StringComparison.InvariantCultureIgnoreCase))
            {
                AddLogosToCache(agencyCode, key);
            }
            else if (originalKey.Equals(CacheConstant.CACHE_KEY_COMMON_LOGO, StringComparison.InvariantCultureIgnoreCase))
            {
                AddCommonLogosToCache(agencyCode, key);
            }
            else if (originalKey.Equals(CacheConstant.CACHE_KEY_ACAPAGES, StringComparison.InvariantCultureIgnoreCase))
            {
                AddACAPagesToCache(agencyCode, key);
            }
            else if (originalKey.Equals(CacheConstant.CACHE_KEY_SERVICEPROVIDER, StringComparison.InvariantCultureIgnoreCase))
            {
                AddServiceProviderToCache(agencyCode, key);
            }
            else if (originalKey.Equals(CacheConstant.CACHE_KEY_ALL_CAPTYPE_ENTITIES, StringComparison.InvariantCultureIgnoreCase))
            {
                AddCapTypeEntitiesToCache(agencyCode, key);
            }
            else if (originalKey.Equals(CacheConstant.CACHE_KEY_ALL_XENTITY_PEMISSION, StringComparison.InvariantCultureIgnoreCase))
            {
                AddXEntityPermissionToCache(agencyCode, key);
            }
            else if (originalKey.Equals(CacheConstant.CACHE_KEY_INSPECTION_ACTION_PERMISSION, StringComparison.InvariantCultureIgnoreCase))
            {
                AddInsActionPermissionToCache(agencyCode, key);
            }
            else if (originalKey.Equals(CacheConstant.CACHE_KEY_CONDITION_GROUP, StringComparison.InvariantCultureIgnoreCase))
            {
                AddConditionGroupToCache(agencyCode, key);
            }
            else
            {
                Logger.ErrorFormat("Cache Key:{0} isn't found, please make sure the key is valid.");
            }

            object cacheEntry = HttpRuntime.Cache.Get(key);
            return cacheEntry == null ? new Hashtable() : (Hashtable)cacheEntry;
        }

        /// <summary>
        /// return a cached item by the given key.
        /// </summary>
        /// <param name="key">string cache key</param>
        /// <returns>The retrieved cache item, or null if the key is not found.</returns>
        public object GetSingleCachedItem(string key)
        {
            if (!string.IsNullOrEmpty(key))
            {
                object cacheEntry = HttpRuntime.Cache.Get(key);
                return cacheEntry;
            }

            return null;
        }

        /// <summary>
        /// Gets the default expiration time, if the value is configured in web.config,
        /// get it from web.config, if not, return 1440 minutes as default value.
        /// </summary>
        /// <returns>default expiration time</returns>
        public int GetDefaultExpirationTime()
        {
            string expiraTime = ConfigurationManager.AppSettings[DEFAULT_CACHE_EXPIRATION];

            int interval = DEFAULT_EXPIRATION_TIME;

            decimal parsedResult = 0;

            if (decimal.TryParse(expiraTime, NumberStyles.Number, CultureInfo.InvariantCulture, out parsedResult))
            {
                interval = (int)decimal.Truncate(parsedResult);
            }

            return interval;
        }

        /// <summary>
        /// Get Cap Status From Cache
        /// </summary>
        /// <param name="agencyCode">Agency Code</param>
        /// <param name="moduleName">Module Name</param>
        /// <param name="group">App Status Group Code</param>
        /// <param name="status">App Status</param>
        /// <returns>App Status Group Model</returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public AppStatusGroupModel4WS GetAppStatusGroupModel(string agencyCode, string moduleName, string group, string status)
        {
            // The same app status is configured under different modules.
            string key = agencyCode + ACAConstant.SPLIT_CHAR + moduleName + ACAConstant.SPLIT_CHAR + group + ACAConstant.SPLIT_CHAR + status;
            string cacheKey = agencyCode + ACAConstant.SPLIT_CHAR + CacheConstant.CACHE_KEY_APPSTATUS;

            Hashtable htCapStatus = HttpRuntime.Cache.Get(cacheKey) as Hashtable;

            if (htCapStatus != null && htCapStatus.ContainsKey(key))
            {
                return htCapStatus[key] as AppStatusGroupModel4WS;
            }

            if (htCapStatus == null)
            {
                htCapStatus = new Hashtable();
            }

            AppStatusGroupModel4WS[] appStatuses = CacheContentProvider.GetAppStatusByModule(agencyCode, moduleName);

            if (appStatuses != null && appStatuses.Length > 0)
            {
                foreach (AppStatusGroupModel4WS appStatusItem in appStatuses)
                {
                    if (appStatusItem == null)
                    {
                        continue;
                    }

                    key = appStatusItem.servProvCode + ACAConstant.SPLIT_CHAR
                        + appStatusItem.moduleName + ACAConstant.SPLIT_CHAR
                        + appStatusItem.appStatusGroupCode + ACAConstant.SPLIT_CHAR
                        + appStatusItem.status;

                    if (htCapStatus.ContainsKey(key))
                    {
                        htCapStatus[key] = appStatusItem;
                    }
                    else
                    {
                        htCapStatus.Add(key, appStatusItem);
                    }
                }
            }

            AddCacheItemToCache(cacheKey, htCapStatus);

            return htCapStatus[key] as AppStatusGroupModel4WS;
        }

        /// <summary>
        /// refresh labels cache after saved by ACA Admin
        /// </summary>
        /// <param name="agencyCode">agency code</param>
        /// <param name="lables">Hashtable of labels</param>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void RefreshLabels(string agencyCode, Hashtable lables)
        {
            string cacheKey = agencyCode + ACAConstant.SPLIT_CHAR + I18nCultureUtil.AppendUserPreferredCultureFlag(CacheConstant.CACHE_KEY_LABEL);
            AddLabelsToCache(lables, cacheKey);
        }

        /// <summary>
        /// remove an item from the cache by the key
        /// </summary>
        /// <param name="key">the key of a cache entry</param>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void Remove(string key)
        {
            if (HttpRuntime.Cache.Get(key) != null)
            {
                HttpRuntime.Cache.Remove(key);
            }
        }

        /// <summary>
        /// Cache others
        /// </summary>
        /// <param name="key">string key</param>
        /// <param name="value">object value</param>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void UpdateSingleCacheItem(string key, object value)
        {
            if (HttpRuntime.Cache.Get(key) == null)
            {
                return;
            }

            HttpRuntime.Cache[key] = value;

            if (Logger.IsDebugEnabled)
            {
                Logger.Debug("update single item to cache");
            }
        }

        /// <summary>
        /// Get cached generic view element.
        /// </summary>
        /// <param name="moduleName">module name.</param>
        /// <param name="viewId">string view id.</param>
        /// <returns>generic view array.</returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public SimpleViewElementModel4WS[] GetGviewElements(string moduleName, string viewId)
        {
            string cacheKey = AgencyCode + ACAConstant.SPLIT_CHAR + CacheConstant.CACHE_KEY_GVIEW_ELEMENT;
            string module = string.IsNullOrEmpty(moduleName) ? AgencyCode : moduleName;
            string viewKey = module + ACAConstant.SPLIT_CHAR5 + viewId;
            Hashtable htGviews = null;

            if (HttpRuntime.Cache.Get(cacheKey) != null)
            {
                htGviews = HttpRuntime.Cache.Get(cacheKey) as Hashtable;

                if (htGviews.ContainsKey(viewKey))
                {
                    return htGviews[viewKey] as SimpleViewElementModel4WS[];
                }
            }

            if (htGviews == null)
            {
                htGviews = new Hashtable();
                AddCacheItemToCache(cacheKey, htGviews);
            }

            SimpleViewElementModel4WS[] simpleViewElements = CacheContentProvider.GetGViewElements(AgencyCode, moduleName, viewId);
            htGviews.Add(viewKey, simpleViewElements);

            return simpleViewElements;
        }

        /// <summary>
        /// Get cached generic view element.
        /// </summary>
        /// <param name="moduleName">module name.</param>
        /// <param name="permission">The permission.</param>
        /// <param name="viewId">string view id.</param>
        /// <param name="callerId">The caller ID.</param>
        /// <returns>generic view array.</returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public SimpleViewElementModel4WS[] GetGviewElements(string moduleName, GFilterScreenPermissionModel4WS permission, string viewId, string callerId)
        {
            string cacheKey = AgencyCode + ACAConstant.SPLIT_CHAR + CacheConstant.CACHE_KEY_GVIEW_ELEMENT;
            string module = string.IsNullOrEmpty(moduleName) ? AgencyCode : moduleName;
            StringBuilder sbKey = new StringBuilder();
            sbKey.Append(module);
            sbKey.Append(ACAConstant.SPLIT_CHAR5);
            sbKey.Append(viewId);

            if (permission != null)
            {
                if (!string.IsNullOrEmpty(permission.permissionLevel))
                {
                    sbKey.Append(permission.permissionLevel);
                }

                if (!string.IsNullOrEmpty(permission.permissionValue))
                {
                    sbKey.Append(permission.permissionValue);
                }
            }

            string viewKey = sbKey.ToString();
            Hashtable htGviews = null;

            if (HttpRuntime.Cache.Get(cacheKey) != null)
            {
                htGviews = HttpRuntime.Cache.Get(cacheKey) as Hashtable;

                if (htGviews.ContainsKey(viewKey))
                {
                    return htGviews[viewKey] as SimpleViewElementModel4WS[];
                }
            }

            if (htGviews == null)
            {
                htGviews = new Hashtable();
                AddCacheItemToCache(cacheKey, htGviews);
            }

            SimpleViewElementModel4WS[] simpleViewElements = CacheContentProvider.GetGViewElements(AgencyCode, moduleName, permission, viewId, callerId);
            htGviews.Add(viewKey, simpleViewElements);

            return simpleViewElements;
        }

        /// <summary>
        /// Gets the generic view.
        /// </summary>
        /// <param name="agencyCode">The agency code.</param>
        /// <param name="moduleName">Name of the module.</param>
        /// <param name="permission">The permission.</param>
        /// <param name="viewId">The view id.</param>
        /// <param name="callerId">The caller id.</param>
        /// <returns>generic view object</returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public SimpleViewModel4WS GetGview(string agencyCode, string moduleName, GFilterScreenPermissionModel4WS permission, string viewId, string callerId)
        {
            string cacheKey = agencyCode + ACAConstant.SPLIT_CHAR + CacheConstant.CACHE_KEY_GVIEW;
            string module = string.IsNullOrEmpty(moduleName) ? agencyCode : moduleName;
            StringBuilder sbKey = new StringBuilder();
            sbKey.Append(module);
            sbKey.Append(ACAConstant.SPLIT_CHAR5);
            sbKey.Append(viewId);

            if (permission != null)
            {
                if (!string.IsNullOrEmpty(permission.permissionLevel))
                {
                    sbKey.Append(permission.permissionLevel);
                }

                if (!string.IsNullOrEmpty(permission.permissionValue))
                {
                    sbKey.Append(permission.permissionValue);
                }
            }

            string viewKey = sbKey.ToString();

            Hashtable htGviews = null;

            if (HttpRuntime.Cache.Get(cacheKey) != null)
            {
                htGviews = HttpRuntime.Cache.Get(cacheKey) as Hashtable;

                if (htGviews.ContainsKey(viewKey))
                {
                    return htGviews[viewKey] as SimpleViewModel4WS;
                }
            }

            if (htGviews == null)
            {
                htGviews = new Hashtable();
                AddCacheItemToCache(cacheKey, htGviews);
            }

            SimpleViewModel4WS simpleView = CacheContentProvider.GetGView(agencyCode, moduleName, permission, viewId, callerId);

            htGviews.Add(viewKey, simpleView);

            return simpleView;
        }

        /// <summary>
        /// Get bizDomain Key value pairs from biz server directly instead of cache.
        /// </summary>
        /// <param name="agencyCode">agency code</param>
        /// <param name="category">category name</param>
        /// <param name="culture">The culture.</param>
        /// <returns>hashtable containing BizDomain key and value pair</returns>
        public Hashtable GetBizDomainKeyValuePairs(string agencyCode, string category, string culture)
        {
            Hashtable stdItems = CacheContentProvider.GetBizDomainKeyValuePairs(agencyCode, category, culture);

            return stdItems;
        }

        /// <summary>
        /// Get required contact address type from cache.
        /// </summary>
        /// <param name="contactType">contact type</param>
        /// <returns>string list</returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public IList<string> GetRequiredContactAddressType(string contactType)
        {
            string cacheKey = AgencyCode + ACAConstant.SPLIT_CHAR + CacheConstant.CACHE_KEY_REQUIRED_CONTACT_ADDRESS_TYPE;
            Hashtable cacheItem = HttpRuntime.Cache.Get(cacheKey) as Hashtable;
            List<string> requiredAddressTypes = null;

            if (cacheItem != null)
            {
                requiredAddressTypes = cacheItem[contactType] as List<string>;
            }

            // if cache is null, will re-search associated required address type and add it to cache.  
            if (requiredAddressTypes == null)
            {
                IRefAddressBll refAddressBll = (IRefAddressBll)ObjectFactory.GetObject(typeof(IRefAddressBll));
                XRefContactAddressTypeModel[] contactAddressTypes = refAddressBll.GetContactAddressTypeByContactType(ConfigManager.AgencyCode, contactType);

                if (contactAddressTypes != null)
                {
                    requiredAddressTypes = new List<string>();
                    IBizDomainBll bizBll = ObjectFactory.GetObject<IBizDomainBll>();
                    string addressTypeValue = string.Empty;

                    foreach (XRefContactAddressTypeModel addressType in contactAddressTypes)
                    {
                        addressTypeValue = bizBll.GetValueForStandardChoice(ConfigManager.AgencyCode, BizDomainConstant.STD_CAT_CONTACT_ADDRESS_TYPE, addressType.addressType);

                        if (!string.IsNullOrEmpty(addressTypeValue) && ValidationUtil.IsYes(addressType.required))
                        {
                            requiredAddressTypes.Add(addressType.addressType);
                        }
                    }

                    if (cacheItem == null)
                    {
                        cacheItem = new Hashtable(StringComparer.InvariantCultureIgnoreCase);
                    }

                    cacheItem.Add(contactType, requiredAddressTypes);
                    AddCacheItemToCache(cacheKey, cacheItem);
                }
            }

            return requiredAddressTypes;
        }

        /// <summary>
        /// Gets the regional model by country.
        /// </summary>
        /// <param name="countryCode">The country code.</param>
        /// <param name="states">The states.</param>
        /// <returns>regional setting object.</returns>
        public RegionalModel GetRegionalModelByCountry(string countryCode, out Dictionary<string, string> states)
        {
            RegionalModel regionalModel = null;

            //regional setting model cache key.
            string cacheRegionalKey = AgencyCode + ACAConstant.SPLIT_CHAR + CacheConstant.CACHE_KEY_REGIONAL_CONFIG;

            //regional setting country's state cache key
            string cacheRegionalStateKey = AgencyCode + ACAConstant.SPLIT_CHAR + I18nCultureUtil.AppendUserPreferredCultureFlag(CacheConstant.CACHE_KEY_REGIONAL_CONFIG_STATE);

            states = new Dictionary<string, string>();

            //get regional model from cache. 
            List<RegionalModel> regionalModels = CacheContentProvider.GetRegionalModels(cacheRegionalKey);

            if (regionalModels != null && regionalModels.Count > 0)
            {
                if (HttpRuntime.Cache.Get(cacheRegionalKey) == null)
                {
                    //add the regional setting to cache
                    this.AddSingleCacheItemToCache(this.GetDefaultCacheSettings(cacheRegionalKey), regionalModels, CacheItemPriority.Default);
                }

                //if country code empty, get default setting.
                regionalModel = string.IsNullOrEmpty(countryCode)
                                    ? regionalModels.FirstOrDefault(o => o.defaultCountryRegion)
                                    : regionalModels.FirstOrDefault(o => o.countryCode == countryCode);
            }

            Dictionary<string, Dictionary<string, string>> regionalStateCache = CacheContentProvider.GetRegionalState(cacheRegionalStateKey, regionalModels);

            if (regionalStateCache != null && regionalStateCache.Count > 0)
            {
                if (HttpRuntime.Cache.Get(cacheRegionalStateKey) == null)
                {
                    this.AddSingleCacheItemToCache(this.GetDefaultCacheSettings(cacheRegionalStateKey), regionalStateCache, CacheItemPriority.Default);
                }

                if (regionalModel != null)
                {
                    regionalStateCache.TryGetValue(regionalModel.countryCode, out states);
                }
            }

            return regionalModel;
        }

        /// <summary>
        /// Gets the type of all contact.
        /// </summary>
        /// <param name="agencyCode">Agency Code</param>
        /// <param name="includeInactivity">Indicates whether return inactive contact types.</param>
        /// <returns>Contact type list</returns>
        public BizDomainModel4WS[] GetAllContactType(string agencyCode, bool includeInactivity)
        {
            string cacheContactTypeKey = agencyCode + ACAConstant.SPLIT_CHAR + I18nCultureUtil.AppendUserPreferredCultureFlag(CacheConstant.CACHE_CONTACT_TYPE);
            BizDomainModel4WS[] contactTypeResults = null;

            if (HttpRuntime.Cache.Get(cacheContactTypeKey) == null)
            {
                contactTypeResults = CacheContentProvider.GetAllContactType(agencyCode);
                AddSingleCacheItemToCache(GetDefaultCacheSettings(cacheContactTypeKey), contactTypeResults, CacheItemPriority.Default);
            }
            else
            {
                contactTypeResults = HttpRuntime.Cache.Get(cacheContactTypeKey) as BizDomainModel4WS[];
            }

            if (includeInactivity || contactTypeResults == null)
            {
                return contactTypeResults;
            }

            return contactTypeResults.Where(o => ACAConstant.VALID_STATUS.Equals(o.auditStatus)).ToArray();
        }

        /// <summary>
        /// Gets ASI security from cache.
        /// </summary>
        /// <param name="asiSecuriyQueryParam">query parameters</param>
        /// <param name="key">cache key of ASI security</param>
        /// <returns>ASI security hash table</returns>
        public Hashtable GetASISecuritiesFromCache(ASISecurityQueryParam4WS asiSecuriyQueryParam, string key)
        {
            if (HttpRuntime.Cache.Get(key) == null)
            {
                AddCacheItemToCache(key, null);
            }

            object cacheEntry = HttpRuntime.Cache.Get(key);
            Hashtable asiSecurity = cacheEntry == null ? new Hashtable() : (Hashtable)cacheEntry;

            string asiSecurityKey = BuildASISecurityKey(asiSecuriyQueryParam);

            Hashtable htASISecurities = null;

            if (!asiSecurity.ContainsKey(asiSecurityKey))
            {
                htASISecurities = CacheContentProvider.GetASISecurities(asiSecuriyQueryParam);
                asiSecurity.Add(asiSecurityKey, htASISecurities);
                AddCacheItemToCache(key, asiSecurity);
            }
            else
            {
                htASISecurities = (Hashtable)asiSecurity[asiSecurityKey];
            }

            return htASISecurities;
        }

        /// <summary>
        /// Get Customization File Map
        /// </summary>
        /// <param name="agency">Agency Code</param>
        /// <param name="culture">Culture information</param>
        /// <returns>Customization File Map</returns>
        public Hashtable GetCustomizationFileMap(string agency, string culture)
        {
            string cacheKey = agency + ACAConstant.SPLIT_CHAR + I18nCultureUtil.AppendCultureFlag(CacheConstant.CACHE_KEY_CUSTOMIZATION, culture);

            Hashtable htCustomization = HttpRuntime.Cache.Get(cacheKey) as Hashtable;

            // Initialize one dictionary instance
            if (htCustomization == null)
            {
                htCustomization = new Hashtable();

                AddCacheItemToCache(cacheKey, htCustomization);
            }

            return htCustomization;
        }

        #endregion

        #region Timer

        /// <summary>
        /// Start the timer for check cache status.
        /// </summary>
        private static void StartTimer()
        {
            UploadiTimer.Interval = TIME_INTERVAL;
            UploadiTimer.Elapsed += new ElapsedEventHandler(CheckCacheStatus);

            UploadiTimer.Start();
        }

        /// <summary>
        /// Check cache status.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="ElapsedEventArgs"/> instance containing the event data.</param>
        private static void CheckCacheStatus(object sender, ElapsedEventArgs e)
        {
            try
            {
                CheckAndRefreshCache();
            }
            catch (Exception ex)
            {
                Logger.ErrorFormat("Timer for cache status check. {0}", ex.ToString());
            }
        }

        /// <summary>
        /// Check whether need to refresh cache from biz-server, if yes then clear ACA cache.
        /// Only refresh changed cache by cache name.
        /// </summary>
        private static void CheckAndRefreshCache()
        {
            // 1. get cache status from biz server
            string[] cacheStatusFromBizServer = WSFactory.Instance.GetWebService<CacheManageWebServiceService>().checkCacheStatus();
            List<ItemValue> cacheStatusListFromBizServer = new List<ItemValue>();
            StringBuilder sbLog = new StringBuilder("Cache status refresh from biz server: ");

            // convert string key=value to ItemValue key-value object
            if (cacheStatusFromBizServer != null)
            {
                foreach (string item in cacheStatusFromBizServer)
                {
                    if (!string.IsNullOrEmpty(item))
                    {
                        if (Logger.IsDebugEnabled)
                        {
                            sbLog.Append(item + "||");
                        }

                        string[] keyValue = item.Split('=');
                        string key = keyValue[0].Trim();
                        key = CacheConstant.ConvertAACacheNameToACA(key);
                        string value = keyValue.Length > 1 ? keyValue[1] : string.Empty;

                        ItemValue iv = new ItemValue(key, value);
                        cacheStatusListFromBizServer.Add(iv);
                    }
                }
            }

            if (Logger.IsDebugEnabled)
            {
                Logger.Debug(sbLog.ToString());
            }

            // 2. compare the last update time to see whether what needs to reload.
            List<string> needToReloadCaches = new List<string>();
            bool existedInLocal = false;
            bool isChanged = true;

            foreach (ItemValue server in cacheStatusListFromBizServer)
            {
                existedInLocal = false;
                isChanged = true;

                foreach (ItemValue local in LocalCacheStatus)
                {
                    if (server.Key == local.Key)
                    {
                        existedInLocal = true;

                        // whether the last update time is changed
                        if (Convert.ToString(server.Value) == Convert.ToString(local.Value))
                        {
                            isChanged = false;
                        }
                        else
                        {
                            isChanged = true;
                            local.Value = server.Value;
                        }
                    }
                }

                if (!existedInLocal)
                {
                    LocalCacheStatus.Add(new ItemValue(server.Key, server.Value));
                }

                if (isChanged)
                {
                    needToReloadCaches.Add(server.Key);
                }
            }

            // 3. Clear changed cache, system will auto refresh cache when next access the cache content
            if (needToReloadCaches.Count > 0)
            {
                string[] cacheKeys = new string[needToReloadCaches.Count];

                //convert AA cache name to ACA cache name
                for (int i = 0; i < cacheKeys.Length; i++)
                {
                    cacheKeys[i] = CacheConstant.ConvertAACacheNameToACA(needToReloadCaches[i]);
                }

                // revmove invalid cache name
                List<string> cacheKeyList = new List<string>();
                for (int i = 0; i < cacheKeys.Length; i++)
                {
                    if (CacheConstant.IsValidCacheName(cacheKeys[i]) && !cacheKeyList.Contains(cacheKeys[i]))
                    {
                        cacheKeyList.Add(cacheKeys[i]);
                    }
                }

                ObjectFactory.GetObject<ICacheManager>().ClearCache(cacheKeyList.ToArray());
            }
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// get key length is 3, get cache by pass in agency code.
        /// </summary>
        /// <param name="agencyCode">the agency code</param>
        /// <param name="key">the key</param>
        /// <returns>agency code.</returns>
        private string GetAgencyCode(string agencyCode, string key)
        {
            string newAgencyCode = AgencyCode;

            if (!string.IsNullOrEmpty(key))
            {
                string[] splitArray = key.Split(new char[] { ACAConstant.SPLIT_CHAR }, StringSplitOptions.None);

                if (splitArray.Length == 3)
                {
                    newAgencyCode = agencyCode;
                }
            }

            return newAgencyCode;
        }

        /// <summary>
        ///  Cache inspection action permission array to Cache.
        /// </summary>
        /// <param name="agencyCode">agency code</param>
        /// <param name="cacheKey">string cache key</param>
        private void AddInsActionPermissionToCache(string agencyCode, string cacheKey)
        {
            if (HttpRuntime.Cache.Get(cacheKey) != null)
            {
                return;
            }

            AddCacheItemToCache(cacheKey, CacheContentProvider.GetAllInsActionPermission(agencyCode));
        }

        /// <summary>
        ///  Cache condition group array to Cache.
        /// </summary>
        /// <param name="agencyCode">agency code</param>
        /// <param name="cacheKey">string cache key</param>
        private void AddConditionGroupToCache(string agencyCode, string cacheKey)
        {
            if (HttpRuntime.Cache.Get(cacheKey) != null)
            {
                return;
            }

            Hashtable htStdCates = GetBizDomainKeyValuePairs(agencyCode, BizDomainConstant.STD_CAT_CONDITIONS_GROUP, I18nCultureUtil.UserPreferredCulture);

            AddCacheItemToCache(cacheKey, htStdCates);
        }

        /// <summary>
        ///  Cache all ACA page list which is predefined in aca admin tree.
        /// </summary>
        /// <param name="agencyCode">agency code</param>
        /// <param name="cacheKey">string cache key</param>
        private void AddACAPagesToCache(string agencyCode, string cacheKey)
        {
            if (HttpRuntime.Cache.Get(cacheKey) != null)
            {
                return;
            }

            Hashtable htPages = CacheContentProvider.GetAllACAPages(agencyCode);

            AddCacheItemToCache(cacheKey, htPages);

            if (Logger.IsDebugEnabled)
            {
                Logger.Debug("Add ACA pages to cache.");
            }
        }

        /// <summary>
        ///  Cache Service Provider model to cache.
        /// </summary>
        /// <param name="agencyCode">agency code</param>
        /// <param name="cacheKey">string cache key</param>
        private void AddServiceProviderToCache(string agencyCode, string cacheKey)
        {
            if (HttpRuntime.Cache.Get(cacheKey) != null)
            {
                return;
            }

            Hashtable htServiceProvider = CacheContentProvider.GetServiceProvider(CacheConstant.CACHE_KEY_SERVICEPROVIDER, agencyCode);

            AddCacheItemToCache(cacheKey, htServiceProvider);

            if (Logger.IsDebugEnabled)
            {
                Logger.Debug("Add Service Provider model to cache.");
            }
        }

        /// <summary>
        ///  Cache Cap Type Entities model to cache.
        /// </summary>
        /// <param name="agencyCode">agency code</param>
        /// <param name="cacheKey">string cache key</param>
        private void AddCapTypeEntitiesToCache(string agencyCode, string cacheKey)
        {
            if (HttpRuntime.Cache.Get(cacheKey) != null)
            {
                return;
            }

            Hashtable htCapTypeEntities = CacheContentProvider.GetCapTypeEntities(agencyCode);

            AddCacheItemToCache(cacheKey, htCapTypeEntities);

            if (Logger.IsDebugEnabled)
            {
                Logger.Debug("Add Service Provider model to cache.");
            }
        }

        /// <summary>
        ///  Cache XEntities model to cache.
        /// </summary>
        /// <param name="agencyCode">agency code</param>
        /// <param name="cacheKey">string cache key</param>
        private void AddXEntityPermissionToCache(string agencyCode, string cacheKey)
        {
            if (HttpRuntime.Cache.Get(cacheKey) != null)
            {
                return;
            }

            var htXEntities = CacheContentProvider.GetXEntityPermission(agencyCode);
            AddCacheItemToCache(cacheKey, htXEntities);
        }

        /// <summary>
        /// Add cache item content to cache
        /// </summary>
        /// <param name="cacheKey">Cache Key.</param>
        /// <param name="item">the content to be cached.</param>
        private void AddCacheItemToCache(string cacheKey, Hashtable item)
        {
            if (item == null)
            {
                // avoid to circle loop
                item = new Hashtable();
            }

            AddSingleCacheItemToCache(GetDefaultCacheSettings(cacheKey), item, CacheItemPriority.Default);
        }

        /// <summary>
        /// Cache All of cap type filters.
        /// </summary>
        /// <param name="agencyCode">agency code.</param>
        /// <param name="cacheKey">string cache key</param>
        [MethodImpl(MethodImplOptions.Synchronized)]
        private void AddCapTypeFilterToCache(string agencyCode, string cacheKey)
        {
            if (HttpRuntime.Cache.Get(cacheKey) != null)
            {
                return;
            }

            Hashtable htCapTypeFilters = CacheContentProvider.GetCapTypeFilters(agencyCode);

            AddCacheItemToCache(cacheKey, htCapTypeFilters);

            if (Logger.IsDebugEnabled)
            {
                Logger.Debug("Add cap type filters to cache.");
            }
        }

        /// <summary>
        /// Cache All of the available EMSE event.
        /// </summary>
        /// <param name="agencyCode">agency code.</param>
        /// <param name="cacheKey">string cache key</param>
        [MethodImpl(MethodImplOptions.Synchronized)]
        private void AddEMSEEventCofigToCache(string agencyCode, string cacheKey)
        {
            if (HttpRuntime.Cache.Get(cacheKey) != null)
            {
                return;
            }

            Hashtable htEMSEEventConfig = CacheContentProvider.GetAllEventScript(agencyCode);

            AddCacheItemToCache(cacheKey, htEMSEEventConfig);

            if (Logger.IsDebugEnabled)
            {
                Logger.Debug("Add available EMSE event to cache.");
            }
        }

        /// <summary>
        /// Cache global I18n settings(only have master language value)
        /// </summary>
        /// <param name="agencyCode">agency code.</param>
        /// <param name="cacheKey">string cache key.</param>
        /// <param name="culture">The culture.</param>
        [MethodImpl(MethodImplOptions.Synchronized)]
        private void AddGlobalI18nSettingsToCache(string agencyCode, string cacheKey, string culture)
        {
            if (HttpRuntime.Cache.Get(cacheKey) != null)
            {
                return;
            }

            Hashtable i18nSettings = CacheContentProvider.GetBizDomainKeyValuePairs(agencyCode, BizDomainConstant.STD_CAT_I18N_SETTINGS, culture);

            // make sure the cached data is not null to avoid the dead loop
            if (i18nSettings == null)
            {
                i18nSettings = new Hashtable();
            }

            AddCacheItemToCache(cacheKey, i18nSettings);

            if (Logger.IsDebugEnabled)
            {
                Logger.Debug("Refresh global I18n settings from web service.");
            }
        }

        /// <summary>
        /// Adds the fixed dropdown list to cache. e.g:lookup list 
        /// </summary>
        /// <param name="agencyCode">agency code.</param>
        /// <param name="cacheKey">string cache key</param>
        [MethodImpl(MethodImplOptions.Synchronized)]
        private void AddHardCodeDropDownListsToCache(string agencyCode, string cacheKey)
        {
            if (HttpRuntime.Cache.Get(cacheKey) != null)
            {
                return;
            }

            Hashtable hardCodes = CacheContentProvider.GetHardCodeDropDownLists(agencyCode);

            // make sure the cached data is not null to avoid the dead loop
            if (hardCodes == null)
            {
                hardCodes = new Hashtable();
            }

            AddCacheItemToCache(cacheKey, hardCodes);

            if (Logger.IsDebugEnabled)
            {
                Logger.Debug("Refresh Look up dropdown list from web service.");
            }
        }

        /// <summary>
        /// Cache all of Labels.
        /// </summary>
        /// <param name="agencyCode">agency code.</param>
        /// <param name="cacheKey">string cache key.</param>
        /// <param name="cultureName">culture name.</param>
        [MethodImpl(MethodImplOptions.Synchronized)]
        private void AddLabelsToCache(string agencyCode, string cacheKey, string cultureName)
        {
            if (HttpRuntime.Cache.Get(cacheKey) != null)
            {
                return;
            }

            //Cache labels
            Hashtable labels = CacheContentProvider.GetGuiTexts(agencyCode, cultureName);

            // make sure the cached data is not null to avoid the dead loop
            if (labels == null)
            {
                labels = new Hashtable();
            }

            AddLabelsToCache(labels, cacheKey);
        }

        /// <summary>
        /// Cache all of Labels.
        /// </summary>
        /// <param name="labels">hash table of labels</param>
        /// <param name="cacheKey">string cache key</param>
        [MethodImpl(MethodImplOptions.Synchronized)]
        private void AddLabelsToCache(Hashtable labels, string cacheKey)
        {
            AddCacheItemToCache(cacheKey, labels);

            if (Logger.IsDebugEnabled)
            {
                Logger.Debug("Refresh Labels from web service.");
            }
        }

        /// <summary>
        /// Cache All of agency logos.
        /// </summary>
        /// <param name="agencyCode">agency code.</param>
        /// <param name="cacheKey">string cache key</param>
        [MethodImpl(MethodImplOptions.Synchronized)]
        private void AddLogosToCache(string agencyCode, string cacheKey)
        {
            IServiceProviderBll serviceProviderBll = (IServiceProviderBll)ObjectFactory.GetObject(typeof(IServiceProviderBll));

            IBizDomainBll bizBll = (IBizDomainBll)ObjectFactory.GetObject(typeof(IBizDomainBll));
            string superAgency = bizBll.GetValueForStandardChoice(agencyCode, BizDomainConstant.STD_CAT_MULTI_SERVICE_SETTINGS, BizDomainConstant.STD_ITEM_IS_SUPER_AGENCY);

            // if it is configurated with 'Yes' or 'Y',it's super agency.
            bool isSuperAgency = ValidationUtil.IsYes(superAgency);

            if (HttpRuntime.Cache.Get(cacheKey) != null || !isSuperAgency)
            {
                return;
            }

            Hashtable htLogos = CacheContentProvider.GetLogos(agencyCode);

            AddCacheItemToCache(cacheKey, htLogos);

            if (Logger.IsDebugEnabled)
            {
                Logger.Debug("Add logos to cache.");
            }
        }

        /// <summary>
        /// Cache All common logos.
        /// </summary>
        /// <param name="agencyCode">agency code.</param>
        /// <param name="cacheKey">string cache key</param>
        [MethodImpl(MethodImplOptions.Synchronized)]
        private void AddCommonLogosToCache(string agencyCode, string cacheKey)
        {
            if (HttpRuntime.Cache.Get(cacheKey) == null)
            {
                Hashtable htLogos = CacheContentProvider.GetCommonLogos(agencyCode);

                AddCacheItemToCache(cacheKey, htLogos);
            }

            if (Logger.IsDebugEnabled)
            {
                Logger.Debug("Add common logos to cache.");
            }
        }

        /// <summary>
        /// Cache All of page flow groups.
        /// </summary>
        /// <param name="agencyCode">agency code.</param>
        /// <param name="cacheKey">string cache key</param>
        [MethodImpl(MethodImplOptions.Synchronized)]
        private void AddPageflowGroupToCache(string agencyCode, string cacheKey)
        {
            if (HttpRuntime.Cache.Get(cacheKey) != null)
            {
                return;
            }

            Hashtable htPageflowGroups = CacheContentProvider.GetPageflowGroups(agencyCode);

            AddCacheItemToCache(cacheKey, htPageflowGroups);

            if (Logger.IsDebugEnabled)
            {
                Logger.Debug("Refresh page flow group from web service.");
            }
        }

        /// <summary>
        /// Creates ASI security Key
        /// </summary>
        /// <param name="asiSecurityQueryParam">query parameters</param>
        /// <returns>ASi security key string</returns>
        private string BuildASISecurityKey(ASISecurityQueryParam4WS asiSecurityQueryParam)
        {
            if (asiSecurityQueryParam == null || asiSecurityQueryParam.asiSecurityModel == null
                || asiSecurityQueryParam.asiSecurityModel.XPolicy == null)
            {
                return string.Empty;
            }

            StringBuilder keyBuilder = new StringBuilder();
            keyBuilder.Append(asiSecurityQueryParam.agency).Append(ACAConstant.SPLIT_DOUBLE_COLON);
            keyBuilder.Append(asiSecurityQueryParam.asiSecurityModel.XPolicy.data1).Append(ACAConstant.SPLIT_DOUBLE_COLON);
            keyBuilder.Append(asiSecurityQueryParam.asiSecurityModel.XPolicy.data5).Append(ACAConstant.SPLIT_DOUBLE_COLON);
            keyBuilder.Append(asiSecurityQueryParam.module).Append(ACAConstant.SPLIT_DOUBLE_COLON);
            keyBuilder.Append(asiSecurityQueryParam.userGroup).Append(ACAConstant.SPLIT_DOUBLE_COLON);
            keyBuilder.Append(asiSecurityQueryParam.userType);

            return keyBuilder.ToString();
        }

        /// <summary>
        /// Cache All of ACA policy.
        /// </summary>
        /// <param name="agencyCode">agency code.</param>
        /// <param name="cacheKey">string cache key.</param>
        /// <param name="culture">The culture.</param>
        [MethodImpl(MethodImplOptions.Synchronized)]
        private void AddPoliciesToCache(string agencyCode, string cacheKey, string culture)
        {
            if (HttpRuntime.Cache.Get(cacheKey) != null)
            {
                return;
            }

            Hashtable htPolicys = CacheContentProvider.GetXPolicyList(agencyCode, culture);

            AddCacheItemToCache(cacheKey, htPolicys);

            if (Logger.IsDebugEnabled)
            {
                Logger.Debug("Add all policys to cache.");
            }
        }

        /// <summary>
        /// Cache All of ACA policy.
        /// </summary>
        /// <param name="agencyCode">agency code.</param>
        /// <param name="cacheKey">string cache key</param>
        /// <param name="culture">The culture.</param>
        [MethodImpl(MethodImplOptions.Synchronized)]
        private void AddPolicyToCache(string agencyCode, string cacheKey, string culture)
        {
            if (HttpRuntime.Cache.Get(cacheKey) != null)
            {
                return;
            }

            Hashtable htPolicys = CacheContentProvider.GetPolicys(agencyCode, culture);

            AddCacheItemToCache(cacheKey, htPolicys);

            if (Logger.IsDebugEnabled)
            {
                Logger.Debug("Add policys to cache.");
            }
        }

        /// <summary>
        /// Cache all of Server constant.
        /// </summary>
        /// <param name="agencyCode">agency code.</param>
        /// <param name="cacheKey">string cache key</param>
        [MethodImpl(MethodImplOptions.Synchronized)]
        private void AddServerConstantsToCache(string agencyCode, string cacheKey)
        {
            if (HttpRuntime.Cache.Get(cacheKey) != null)
            {
                return;
            }

            //Cache server constants
            string[] constantNames = ACAConstant.GetACAServerConstants();
            Hashtable serverConstants = new Hashtable();

            foreach (string constantName in constantNames)
            {
                string constantValue = CacheContentProvider.GetServerConstantValue(agencyCode, constantName);
                serverConstants.Add(constantName, constantValue);
            }

            AddCacheItemToCache(cacheKey, serverConstants);

            if (Logger.IsDebugEnabled)
            {
                Logger.Debug("Refresh sever constants from web service.");
            }
        }

        /// <summary>
        /// Cache all of public user group.
        /// </summary>
        /// <param name="agencyCode">agency code.</param>
        /// <param name="cacheKey">string cache key</param>
        [MethodImpl(MethodImplOptions.Synchronized)]
        private void AddPublicUserGroupsToCache(string agencyCode, string cacheKey)
        {
            if (HttpRuntime.Cache.Get(cacheKey) != null)
            {
                return;
            }

            Hashtable htUserGroups = CacheContentProvider.GetPublicUserGroups(agencyCode);

            // make sure the cached data is not null to avoid the dead loop
            if (htUserGroups == null)
            {
                htUserGroups = new Hashtable();
            }

            AddCacheItemToCache(cacheKey, htUserGroups);

            if (Logger.IsDebugEnabled)
            {
                Logger.Debug("Refresh public user groups from web service.");
            }
        }

        /// <summary>
        /// Add cache item content to cache
        /// </summary>
        /// <param name="settings">cache setting .</param>
        /// <param name="item">the content to be cached.</param>
        /// <param name="priority">the priority of the item</param>
        private void AddSingleCacheItemToCache(CacheSettings settings, object item, CacheItemPriority priority)
        {
            if (HttpRuntime.Cache[settings.Key] != null)
            {
                HttpRuntime.Cache[settings.Key] = item;
            }
            else
            {
                int expirationTime = MIN_EXPIRATION_TIME_BY_MINUTES;

                if (settings.ExpireTime > MIN_EXPIRATION_TIME_BY_MINUTES)
                {
                    expirationTime = settings.ExpireTime;
                }

                if (item != null)
                {
                    HttpRuntime.Cache.Insert(settings.Key, item, null, DateTime.Now.AddMinutes(expirationTime), TimeSpan.Zero, priority, null);
                }
            }
        }

        /// <summary>
        /// Cache standard choices
        /// </summary>
        /// <param name="agencyCode">agency code.</param>
        /// <param name="cacheKey">string cache key.</param>
        /// <param name="culture">The culture.</param>
        [MethodImpl(MethodImplOptions.Synchronized)]
        private void AddStandardChoicesToCache(string agencyCode, string cacheKey, string culture)
        {
            if (HttpRuntime.Cache.Get(cacheKey) != null)
            {
                return;
            }

            Hashtable standardChoices = CacheContentProvider.GetStandardChoices(agencyCode, culture);

            // make sure the cached data is not null to avoid the dead loop
            if (standardChoices == null)
            {
                standardChoices = new Hashtable();
            }

            AddCacheItemToCache(cacheKey, standardChoices);

            if (Logger.IsDebugEnabled)
            {
                Logger.Debug("Refresh standard choice from web service.");
            }
        }

        /// <summary>
        /// Cache All of template includes APO and People template.
        /// </summary>
        /// <param name="agencyCode">agency code.</param>
        /// <param name="cacheKey">string cache key</param>
        [MethodImpl(MethodImplOptions.Synchronized)]
        private void AddTemplatesToCache(string agencyCode, string cacheKey)
        {
            if (HttpRuntime.Cache.Get(cacheKey) != null)
            {
                return;
            }

            Hashtable htTemplates = CacheContentProvider.GetTemplates(agencyCode);

            // make sure the cached data is not null to avoid the dead loop
            if (htTemplates == null)
            {
                htTemplates = new Hashtable();
            }

            AddCacheItemToCache(cacheKey, htTemplates);

            if (Logger.IsDebugEnabled)
            {
                Logger.Debug("Refresh template from web service.");
            }
        }

        /// <summary>
        /// Cache All of ACA policy.
        /// </summary>
        /// <param name="agencyCode">agency code.</param>
        /// <param name="cacheKey">string cache key.</param>
        [MethodImpl(MethodImplOptions.Synchronized)]
        private void AddXPolicyToCache(string agencyCode, string cacheKey)
        {
            if (HttpRuntime.Cache.Get(cacheKey) != null)
            {
                return;
            }

            Hashtable htPolicys = CacheContentProvider.GetXPolicys(agencyCode);

            AddCacheItemToCache(cacheKey, htPolicys);

            if (Logger.IsDebugEnabled)
            {
                Logger.Debug("Add policys to cache.");
            }
        }

        /// <summary>
        /// Cache I18n primary settings
        /// </summary>
        /// <param name="agencyCode">agency code.</param>
        /// <param name="cacheKey">string cache key.</param>
        [MethodImpl(MethodImplOptions.Synchronized)]
        private void AddI18nPrimarySettingsToCache(string agencyCode, string cacheKey)
        {
            if (HttpRuntime.Cache.Get(cacheKey) != null)
            {
                return;
            }

            Hashtable i18nSettings = null;

            try
            {
                i18nSettings = CacheContentProvider.GetI18nPrimarySettings(agencyCode);
            }
            catch (Exception ex)
            {
                Logger.Fatal(ex);
                throw new ACAException(ex);
            }

            // make sure the cached data is not null to avoid the dead loop
            if (i18nSettings == null)
            {
                i18nSettings = new Hashtable();
            }

            AddCacheItemToCache(cacheKey, i18nSettings);

            if (Logger.IsDebugEnabled)
            {
                Logger.Debug("Refresh agency level I18n settings from web service.");
            }
        }

        /// <summary>
        /// Cache I18n locale relevant settings
        /// </summary>
        /// <param name="agencyCode">agency code.</param>
        /// <param name="cacheKey">string cache key.</param>
        /// <param name="locale">Name of the locale.</param>
        [MethodImpl(MethodImplOptions.Synchronized)]
        private void AddI18nLocaleRelevantSettingsToCache(string agencyCode, string cacheKey, string locale)
        {
            if (HttpRuntime.Cache.Get(cacheKey) != null)
            {
                return;
            }

            Hashtable i18nSettings = CacheContentProvider.GetI18nLocaleRelevantSettings(agencyCode, locale);

            // make sure the cached data is not null to avoid the dead loop
            if (i18nSettings == null)
            {
                i18nSettings = new Hashtable();
            }

            AddCacheItemToCache(cacheKey, i18nSettings);

            if (Logger.IsDebugEnabled)
            {
                Logger.Debug("Refresh locale level I18n settings from web service.");
            }
        }

        /// <summary>
        /// Gets the default CacheSettings object.
        /// </summary>
        /// <param name="cacheKey">cache key to identify the cache item.</param>
        /// <returns>default CacheSettings object.</returns>
        private CacheSettings GetDefaultCacheSettings(string cacheKey)
        {
            CacheSettings setting = new CacheSettings();
            setting.Key = cacheKey;
            setting.ExpireTime = GetDefaultExpirationTime();

            return setting;
        }

        #endregion

        #endregion Methods
    }
}