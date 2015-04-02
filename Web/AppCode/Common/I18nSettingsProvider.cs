#region Header

/**
 * <pre>
 *
 *  Accela
 *  File: I18nSettingsProvider.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2009-2014
 *
 *  Description:
 *
 *  Notes:
 * $Id: I18nSettingsProvider.cs 277071 2014-08-11 03:38:26Z ACHIEVO\james.shi $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using System;
using System.Collections;
using System.Collections.Generic;

using Accela.ACA.BLL.Common;
using Accela.ACA.Common;
using Accela.ACA.Common.Util;
using Accela.ACA.WSProxy;

namespace Accela.ACA.Web.Common
{
    /// <summary>
    /// Provider simple I18n settings provider
    /// </summary>
    public class I18nSettingsProvider : II18nSettingsProvider
    {
        #region Methods

        /// <summary>
        /// Gets I18n primary settings.
        /// e.g. currency locale,language options hidden setting,default language,multiple language support setting.
        /// </summary>
        /// <returns>I18n primary settings</returns>
        I18nPrimarySettingsModel II18nSettingsProvider.GetI18nPrimarySettings()
        {
            II18nSettingsBll bll = ObjectFactory.GetObject(typeof(II18nSettingsBll)) as II18nSettingsBll;

            //return value must not null, otherwise, it's ok to throw null exception by system.
            I18nPrimarySettingsModel result = bll.GetI18nPrimarySettings(ConfigManager.AgencyCode);

            return result;
        }

        /// <summary>
        /// Gets I18n locale relevant settings.
        /// e.g. short date format, long date format, customized date time format, currency symbol
        /// </summary>
        /// <returns>I18n locale relevant settings</returns>
        I18nLocaleRelevantSettingsModel II18nSettingsProvider.GetI18nLocaleRelevantSettings()
        {
            II18nSettingsBll bll = ObjectFactory.GetObject(typeof(II18nSettingsBll)) as II18nSettingsBll;

            //return value must not null, otherwise, it's ok to throw null exception by system.
            I18nLocaleRelevantSettingsModel result = bll.GetI18nLocaleRelevantSettings(ConfigManager.AgencyCode);

            return result;
        }

        /// <summary>
        /// Gets supported languages.
        /// </summary>
        /// <returns>Supported languages.</returns>
        Hashtable II18nSettingsProvider.GetSupportedLanguages()
        {
            II18nSettingsBll bll = ObjectFactory.GetObject(typeof(II18nSettingsBll)) as II18nSettingsBll;

            //return value must be not null, otherwise, it's ok to throw null exception by system.
            I18nPrimarySettingsModel i18nPrimarySetting = bll.GetI18nPrimarySettings(ConfigManager.AgencyCode);
            Hashtable result = new Hashtable(StringComparer.InvariantCultureIgnoreCase);

            //supportLanguages must be not null, otherwise, it's ok to throw null exception by system.
            foreach (SupportedLanguageModel supportedLanguageModel in i18nPrimarySetting.supportLanguages)
            {
                string key = I18nCultureUtil.ChangeCulture4DotNet(supportedLanguageModel.locale);
                result.Add(key, supportedLanguageModel.localeLabel);
            }

            return result;
        }

        #endregion Methods
    }
}