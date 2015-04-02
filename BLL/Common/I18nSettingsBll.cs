#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: I18nSettingsBll.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2009-2014
 *
 *  Description:
 *
 *  Notes:
 * $Id: I18nSettingsBll.cs 277237 2014-08-13 01:17:52Z ACHIEVO\james.shi $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using System;
using System.Collections;
using System.Collections.Generic;
using System.Web;

using Accela.ACA.Common;
using Accela.ACA.Common.Log;
using Accela.ACA.Common.Util;
using Accela.ACA.WSProxy;

using log4net;

namespace Accela.ACA.BLL.Common
{
    /// <summary>
    /// This class provides the ability to get I18n setting value.
    /// </summary>
    public sealed class I18nSettingsBll : II18nSettingsBll
    {
        /// <summary>
        /// Gets I18n primary settings.
        /// e.g. currency locale,language options hidden setting,default language,multiple language support setting.
        /// </summary>
        /// <param name="agencyCode">The agency code.</param>
        /// <returns>I18n primary settings</returns>
        I18nPrimarySettingsModel II18nSettingsBll.GetI18nPrimarySettings(string agencyCode)
        {
            I18nPrimarySettingsModel result = null;

            ICacheManager cacheManager = ObjectFactory.GetObject<ICacheManager>();
            Hashtable htStdItems = cacheManager.GetCachedItem(agencyCode, I18nCultureUtil.AppendCultureFlag(CacheConstant.CACHE_KEY_I18N_PRIMARY_SETTINGS, string.Empty));

            if (htStdItems != null && htStdItems.Contains(CacheConstant.CACHE_KEY_I18N_PRIMARY_SETTINGS))
            {
                result = htStdItems[CacheConstant.CACHE_KEY_I18N_PRIMARY_SETTINGS] as I18nPrimarySettingsModel;
            }

            return result;
        }

        /// <summary>
        /// Gets I18n locale relevant settings.
        /// e.g. short date format, long date format, customized date time format, currency symbol
        /// </summary>
        /// <param name="agencyCode">The agency code.</param>
        /// <returns>I18n locale relevant setting</returns>
        I18nLocaleRelevantSettingsModel II18nSettingsBll.GetI18nLocaleRelevantSettings(string agencyCode)
        {
            I18nLocaleRelevantSettingsModel result = null;

            ICacheManager cacheManager = ObjectFactory.GetObject<ICacheManager>();
            Hashtable htStdItems = cacheManager.GetCachedItem(agencyCode, I18nCultureUtil.AppendUserPreferredCultureFlag(CacheConstant.CACHE_KEY_I18N_LOCALE_RELEVANT_SETTINGS));

            if (htStdItems != null && htStdItems.Contains(CacheConstant.CACHE_KEY_I18N_LOCALE_RELEVANT_SETTINGS))
            {
                result = htStdItems[CacheConstant.CACHE_KEY_I18N_LOCALE_RELEVANT_SETTINGS] as I18nLocaleRelevantSettingsModel;
            }

            return result;
        }
    }
}
