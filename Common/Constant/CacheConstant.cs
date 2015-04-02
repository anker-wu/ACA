#region Header
/**
 * <pre>
 * 
 *  Accela Citizen Access
 *  File: ColumnConstant.cs
 * 
 *  Accela, Inc.
 *  Copyright (C): 2009-2014
 * 
 *  Description:
 *  cache contant define
 *  Notes:
 *      $Id: CacheConstant.cs 133464 2009-06-05 05:06:34Z ACHIEVO\jackie.yu $.
 *  Revision History
 *  &lt;Date&gt;  &lt;Who&gt;  &lt;What&gt;
 * </pre>
 */
#endregion Header

using System;
using Accela.ACA.Common.Log;
using log4net;

namespace Accela.ACA.Common
{
    /// <summary>
    /// The constant define for BizDomain
    /// </summary>
    public static class CacheConstant
    {
        #region Cache Group
        /// <summary>
        /// Clear all caches which is defined in CacheKeys.
        /// </summary>
        public const string CACHE_CAT_ALL = "ALL";

        /// <summary>
        /// Clear all caches but not clear label cache.
        /// </summary>
        public const string CACHE_CAT_ALL_BUT_EXCLUDE_LABLE = "ALL_BUT_EXCLUDE_LABLE";

        #endregion

        #region ACA Cache Key

        /// <summary>
        /// cache all pages that has config in aca admin tree
        /// </summary>
        public const string CACHE_KEY_ACAPAGES = "ACA_PAGES";

        /// <summary>
        /// cache Service Provider.
        /// </summary>
        public const string CACHE_KEY_SERVICEPROVIDER = "SERVICE_PROVIDER";

        /// <summary>
        /// cache config all policy
        /// </summary>
        public const string CACHE_KEY_ALL_POLICY = "ALL_POLICY";

        /// <summary>
        /// cache config cap type filter
        /// </summary>
        public const string CACHE_KEY_CAPTYPEFILTER = "CAP_TYPE_FILTER";

        /// <summary>
        /// cache config EMSE event 
        /// </summary>
        public const string CACHE_KEY_EMSEEVENT = "EMSE_EVENT";

        /// <summary>
        /// cache config global I18N settings 
        /// </summary>
        public const string CACHE_KEY_GLOBALI18NSETTINGS = "GLOBAL_I18N_SETTINGS";

        /// <summary>
        /// cache config ASI security
        /// </summary>
        public const string CACHE_KEY_ASISECURITIES = "ASI_SECURITY";

        /// <summary>
        /// cache config hardcode 
        /// </summary>
        public const string CACHE_KEY_HARDCODE = "HARD_CODE";

        /// <summary>
        /// cache config label 
        /// </summary>
        public const string CACHE_KEY_LABEL = "ACA_LABELS";

        /// <summary>
        /// cache config logo
        /// </summary>
        public const string CACHE_KEY_LOGO = "LOGO";

        /// <summary>
        /// cache config common logo
        /// </summary>
        public const string CACHE_KEY_COMMON_LOGO = "COMMON_LOGO";

        /// <summary>
        /// cache config page flow group
        /// </summary>
        public const string CACHE_KEY_PAGEFLOWGROUP = "PAGE_FLOW_GROUP";

        /// <summary>
        /// cache config policy 
        /// </summary>
        public const string CACHE_KEY_POLICY = "POLICY";

        /// <summary>
        /// cache config server constant 
        /// </summary>
        public const string CACHE_KEY_SERVERCONSTANT = "SERVER_CONSTANT";

        /// <summary>
        /// cache config public user groups.
        /// </summary>
        public const string CACHE_KEY_PUBLICUSERGROUP = "PUBLIC_USER_GROUP";

        /// <summary>
        /// cache config standard choice 
        /// </summary>
        public const string CACHE_KEY_STANDARDCHOICE = "STANDARD_CHOICE";

        /// <summary>
        /// cache config condition group 
        /// </summary>
        public const string CACHE_KEY_CONDITION_GROUP = "CONDITION_GROUP";

        /// <summary>
        /// cache config cap status 
        /// </summary>
        public const string CACHE_KEY_APPSTATUS = "APP_STATUS";
        
        /// <summary>
        /// cache config cap status 
        /// </summary>
        public const string CACHE_KEY_CUSTOMIZATION = "CUSTOMIZATION";

        /// <summary>
        /// cache config template
        /// </summary>
        public const string CACHE_KEY_TEMPLATE = "TEMPLATE";

        /// <summary>
        /// cache config XPOLICY
        /// </summary>
        public const string CACHE_KEY_XPOLICY = "XPOLICY";

        /// <summary>
        /// cache config I18n primary settings
        /// </summary>
        public const string CACHE_KEY_I18N_PRIMARY_SETTINGS = "I18N_PRIMARY_SETTINGS";

        /// <summary>
        /// cache config I18n locale relevant settings
        /// </summary>
        public const string CACHE_KEY_I18N_LOCALE_RELEVANT_SETTINGS = "I18N_LOCALE_RELEVANT_SETTINGS";

        /// <summary>
        /// cache config All Cap Type Permission.
        /// </summary>
        public const string CACHE_KEY_ALL_CAPTYPE_ENTITIES = "ALL_CAPTYPE_ENTITIES";

        /// <summary>
        /// cache config inspection action permission.
        /// </summary>
        public const string CACHE_KEY_INSPECTION_ACTION_PERMISSION = "INSPECTION_ACTION_PERMISSION";

        /// <summary>
        /// cache GVIEW element.
        /// </summary>
        public const string CACHE_KEY_GVIEW_ELEMENT = "GVIEW_ELEMENT";

        /// <summary>
        /// cache GVIEW
        /// </summary>
        public const string CACHE_KEY_GVIEW = "GVIEW";

        /// <summary>
        /// cache all XENTITY permission.
        /// </summary>
        public const string CACHE_KEY_ALL_XENTITY_PEMISSION = "ALL_XENTITY_PEMISSION";

        /// <summary>
        /// cache required contact address type.
        /// </summary>
        public const string CACHE_KEY_REQUIRED_CONTACT_ADDRESS_TYPE = "REQUIRED_CONTACT_ADDRESS_TYPE";

        /// <summary>
        /// cache required document type.
        /// </summary>
        public const string CACHE_KEY_REQUIRED_DOCUMENT_TYPE = "REQUIRED_DOCUMENT_TYPE";

        /// <summary>
        /// cache key for regional
        /// </summary>
        public const string CACHE_KEY_REGIONAL_CONFIG = "REGIONAL_CONFIG";

        /// <summary>
        /// cache key for regional state
        /// </summary>
        public const string CACHE_KEY_REGIONAL_CONFIG_STATE = "REGIONAL_CONFIG_STATE";

        /// <summary>
        /// Cache key for EDMS policy.
        /// </summary>
        public const string CACHE_EDMS_POLICY = "EDMS_POLICY";

        /// <summary>
        /// The cache of contact type model
        /// </summary>
        public const string CACHE_CONTACT_TYPE = "CONTACT_TYPE";

        #endregion

        #region AA Cache Name
        
        /// <summary>
        /// the AA cache name: BIZ domain
        /// </summary>
        private const string AA_CACHE_NAME_BIZ_DOMAIN = "BIZ_DOMAIN";

        /// <summary>
        /// the AA cache name: server constants 
        /// </summary>
        private const string AA_CACHE_NAME_SERVER_CONSTANTS = "SERVER_CONSTANTS";

        /// <summary>
        /// the AA cache name: ASI security 
        /// </summary>
        private const string AA_CACHE_NAME_ASI_SECURITY = "ASI_SECURITY";

        /// <summary>
        /// the AA cache name: cap type permission in ACA
        /// </summary>
        private const string AA_CACHE_NAME_ACA_CAP_TYPE_PERMISSION = "ACA_CAP_TYPE_PERMISSION";

        /// <summary>
        /// the AA cache name: agencies 
        /// </summary>
        private const string AA_CACHE_NAME_AGENCIES = "AGENCIES";

        /// <summary>
        /// the AA cache name: XUI and GUI's text 
        /// </summary>
        private const string AA_CACHE_NAME_ACA_GUI_TEXT = "ACA_XUI_GUI_TEXTS";

        #endregion

        #region Logger

        /// <summary>
        /// The logger object
        /// </summary>
        private static readonly ILog Logger = LogFactory.Instance.GetLogger(typeof(CacheConstant));

        #endregion

        #region static properties

        /// <summary>
        /// Gets all cache keys.New cache key is added, also need to update here.
        /// </summary>
        public static string[] CacheKeys
        {
            get
            {
                return new string[]
                {
                    CACHE_KEY_STANDARDCHOICE, CACHE_KEY_GLOBALI18NSETTINGS, CACHE_KEY_HARDCODE,
                    CACHE_KEY_LABEL, CACHE_KEY_SERVERCONSTANT, CACHE_KEY_TEMPLATE,
                    CACHE_KEY_PAGEFLOWGROUP, CACHE_KEY_CAPTYPEFILTER,
                    CACHE_KEY_POLICY, CACHE_KEY_XPOLICY, CACHE_KEY_EMSEEVENT, CACHE_KEY_LOGO,
                    CACHE_KEY_ACAPAGES, CACHE_KEY_ALL_POLICY, CACHE_KEY_SERVICEPROVIDER,
                    CACHE_KEY_I18N_PRIMARY_SETTINGS, CACHE_KEY_I18N_LOCALE_RELEVANT_SETTINGS, CACHE_KEY_COMMON_LOGO,
                    CACHE_KEY_ALL_CAPTYPE_ENTITIES, CACHE_KEY_ASISECURITIES, CACHE_KEY_GVIEW_ELEMENT, CACHE_KEY_INSPECTION_ACTION_PERMISSION,
                    CACHE_KEY_ALL_XENTITY_PEMISSION, CACHE_KEY_GVIEW, CACHE_KEY_REQUIRED_CONTACT_ADDRESS_TYPE,
                    CACHE_KEY_REGIONAL_CONFIG, CACHE_KEY_REGIONAL_CONFIG_STATE, CACHE_EDMS_POLICY, CACHE_KEY_APPSTATUS, CACHE_CONTACT_TYPE, CACHE_KEY_REQUIRED_DOCUMENT_TYPE, CACHE_KEY_CUSTOMIZATION, CACHE_KEY_CONDITION_GROUP
                };
            }
        }

        #endregion

        #region static method

        /// <summary>
        /// indicates whether the cache name is valid
        /// </summary>
        /// <param name="cacheName">ACA cache name.</param>
        /// <returns>true - if the cache name is valid,false-invalid.</returns>
        public static bool IsValidCacheName(string cacheName)
        {
            bool valid = false;

            foreach (string name in CacheKeys)
            {
                if (name.Equals(cacheName, StringComparison.InvariantCultureIgnoreCase))
                {
                    valid = true;
                    break;
                }
            }

            if (!valid)
            {
                Logger.DebugFormat("Invalid cacheName : {0}.", cacheName);
            }

            return valid;
        }

        /// <summary>
        /// Converts AA cache name to ACA cache name.
        /// </summary>
        /// <param name="cacheName">cache name.</param>
        /// <returns>ACA cache name.</returns>
        public static string ConvertAACacheNameToACA(string cacheName)
        {
            if (cacheName == null)
            {
                return string.Empty;
            }

            string cacheNameUpper = cacheName.ToUpper();
            string cacheKey = null;

            switch (cacheNameUpper)
            {
                case AA_CACHE_NAME_BIZ_DOMAIN:
                    cacheKey = CACHE_KEY_STANDARDCHOICE;
                    break;
                case AA_CACHE_NAME_SERVER_CONSTANTS:
                    cacheKey = CACHE_KEY_SERVERCONSTANT;
                    break;
                case AA_CACHE_NAME_ASI_SECURITY:
                    cacheKey = CACHE_KEY_ASISECURITIES;
                    break;
                case AA_CACHE_NAME_ACA_CAP_TYPE_PERMISSION:
                    cacheKey = CACHE_KEY_ALL_CAPTYPE_ENTITIES;
                    break;
                case AA_CACHE_NAME_AGENCIES:
                    cacheKey = CACHE_KEY_SERVICEPROVIDER;
                    break;
                case AA_CACHE_NAME_ACA_GUI_TEXT:
                    cacheKey = CACHE_KEY_LABEL;
                    break;
                default:
                    cacheKey = cacheNameUpper;
                    break;
            }

            return cacheKey;
        }

        #endregion
    }
}
