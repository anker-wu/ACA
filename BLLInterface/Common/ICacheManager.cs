#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: ICacheManager.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2008-2014
 *
 *  Description:
 *
 *  Notes:
 * $Id: ICacheManager.cs 277008 2014-08-09 08:37:50Z ACHIEVO\james.shi $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using System.Collections;
using System.Collections.Generic;

using Accela.ACA.WSProxy;

namespace Accela.ACA.BLL.Common
{
    /// <summary>
    /// Cache Manager, Provided cache ability for all need to be required in ACA.
    /// </summary>
    public interface ICacheManager
    {
        #region Methods

        /// <summary>
        /// Clear cache according to cacheKeys.
        /// </summary>
        /// <param name="cacheKeys">the cache key needs to be clear.</param>
        void ClearCache(string[] cacheKeys);

        /// <summary>
        /// Cache others
        /// </summary>
        /// <param name="key">key values.</param>
        /// <param name="value">value object.</param>
        /// <param name="expireTime">the expiration time(seconds)</param>
        void AddSingleItemToCache(string key, object value, int expireTime);

        /// <summary>
        /// return a cached item by the given key. The key list as below:
        /// STD_CACHE_KEY_LABEL,
        /// STD_CACHE_KEY_STANDARDCHOICE,
        /// STD_CACHE_KEY_SMARTCHOICEGROUP,
        /// STD_CACHE_KEY_TEMPLATE,
        /// STD_CACHE_KEY_SERVERCONSTANT
        /// </summary>
        /// <param name="agencyCode">Agency Code</param>
        /// <param name="key">cache key value.</param>
        /// <returns>The retrieved cache item, or null if the key is not found.</returns>
        Hashtable GetCachedItem(string agencyCode, string key);

        /// <summary>
        /// load cached item by the given key. The key list as below:
        /// STD_CACHE_KEY_LABEL,
        /// STD_CACHE_KEY_STANDARDCHOICE
        /// </summary>
        void LoadGlobalCache();

        /// <summary>
        /// return a cached item by the given key.
        /// </summary>
        /// <param name="key">cache key value.</param>
        /// <returns>The retrieved cache item, or null if the key is not found.</returns>
        object GetSingleCachedItem(string key);

        /// <summary>
        /// Gets the default expiration time, if the value is configured in web.config,
        /// get it from web.config, if not, return 1440 minutes as default value.
        /// </summary>
        /// <returns>default expiration time</returns>
        int GetDefaultExpirationTime();

        /// <summary>
        /// refresh labels cache after saved by ACA Admin
        /// </summary>
        /// <param name="agencyCode">agency code</param>
        /// <param name="lables">hashtable of labels.</param>
        void RefreshLabels(string agencyCode, Hashtable lables);

        /// <summary>
        /// remove an item from the cache by the key
        /// </summary>
        /// <param name="key">the key of a cache entry</param>
        void Remove(string key);

        /// <summary>
        /// Cache others
        /// </summary>
        /// <param name="key">key values.</param>
        /// <param name="value">value object.</param>
        void UpdateSingleCacheItem(string key, object value);

        /// <summary>
        /// Get cached generic view element.
        /// </summary>
        /// <param name="moduleName">module name.</param>
        /// <param name="viewId">string view id.</param>
        /// <returns>Simple View Element array.</returns>
        SimpleViewElementModel4WS[] GetGviewElements(string moduleName, string viewId);

        /// <summary>
        /// Get required document types model.
        /// </summary>
        /// <param name="searchModel">search model</param>
        /// <param name="cacheKey">the cache key</param>
        /// <returns>Hashtable that stores required document types model.</returns>
        Hashtable GetRequiredDocumentTypes(RefRequiredDocumentModel searchModel, string cacheKey);

        /// <summary>
        /// Get cached generic view element.
        /// </summary>
        /// <param name="moduleName">Name of the module.</param>
        /// <param name="permission">The permission.</param>
        /// <param name="viewId">The view unique identifier.</param>
        /// <param name="callerId">The caller unique identifier.</param>
        /// <returns>generic view object array.</returns>
        SimpleViewElementModel4WS[] GetGviewElements(string moduleName, GFilterScreenPermissionModel4WS permission, string viewId, string callerId);
        
        /// <summary>
        /// Get ASI security from cache
        /// </summary>
        /// <param name="asiSecurityQueryParam">ASI security search parameter model</param>
        /// <param name="key">key for ASI security cache</param>
        /// <returns>a hashtable include ASI security</returns>
        Hashtable GetASISecuritiesFromCache(ASISecurityQueryParam4WS asiSecurityQueryParam, string key);

        /// <summary>
        /// Gets the generic view.
        /// </summary>
        /// <param name="agencyCode">The agency code.</param>
        /// <param name="moduleName">Name of the module.</param>
        /// <param name="permission">The permission.</param>
        /// <param name="viewId">The view id.</param>
        /// <param name="callerId">The caller id.</param>
        /// <returns>generic view object</returns>
        SimpleViewModel4WS GetGview(string agencyCode, string moduleName, GFilterScreenPermissionModel4WS permission, string viewId, string callerId);

        /// <summary>
        /// Get bizDomain Key value pairs from biz server directly instead of cache.
        /// </summary>
        /// <param name="agencyCode">agency code</param>
        /// <param name="category">category name</param>
        /// <param name="culture">The culture.</param>
        /// <returns>hashtable containing standard choice key and value pair</returns>
        Hashtable GetBizDomainKeyValuePairs(string agencyCode, string category, string culture);

        /// <summary>
        /// Get required contact address type from cache.
        /// </summary>
        /// <param name="contactType">contact type</param>
        /// <returns>string list</returns>
        IList<string> GetRequiredContactAddressType(string contactType);

        /// <summary>
        /// Gets the regional model by country.
        /// </summary>
        /// <param name="countryCode">The country code.</param>
        /// <param name="states">The states.</param>
        /// <returns>regional setting object.</returns>
        RegionalModel GetRegionalModelByCountry(string countryCode, out Dictionary<string, string> states);

        /// <summary>
        /// Get Cap Status From Cache
        /// </summary>
        /// <param name="agencyCode">Agency Code</param>
        /// <param name="moduleName">Module Name</param>
        /// <param name="group">App Status Group Code</param>
        /// <param name="status">App Status</param>
        /// <returns>App Status Group Model</returns>
        AppStatusGroupModel4WS GetAppStatusGroupModel(string agencyCode, string moduleName, string group, string status);

        /// <summary>
        /// Gets the type of all contact.
        /// </summary>
        /// <param name="agencyCode">Agency Code</param>
        /// <param name="includeInactivity">Indicates whether return inactive contact types.</param>
        /// <returns>Contact type list</returns>
        BizDomainModel4WS[] GetAllContactType(string agencyCode, bool includeInactivity);

        /// <summary>
        /// Get Customization File Map
        /// </summary>
        /// <param name="agency">Agency Code</param>
        /// <param name="culture">Culture information</param>
        /// <returns>Customization File Map</returns>
        Hashtable GetCustomizationFileMap(string agency, string culture);

        #endregion Methods
    }
}