#region Header

/**
 * <pre>
 *
 *  Accela
 *  File: II18nSettingsProvider.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2009-2014
 *
 *  Description:
 *
 *  Notes:
 * $Id: II18nSettingsProvider.cs 276545 2014-08-04 02:15:30Z ACHIEVO\james.shi $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using System.Collections;

using Accela.ACA.WSProxy;

namespace Accela.ACA.Common
{
    /// <summary>
    /// provides interface for I18n settings provider. 
    /// </summary>
    public interface II18nSettingsProvider
    {
        #region Methods

        /// <summary>
        /// Gets I18n primary settings.
        /// e.g. currency locale,language options hidden setting,default language,multiple language support setting.
        /// </summary>
        /// <returns>I18n primary settings</returns>
        I18nPrimarySettingsModel GetI18nPrimarySettings();

        /// <summary>
        /// Gets I18n locale relevant settings.
        /// e.g. short date format, long date format, customized date time format, currency symbol
        /// </summary>
        /// <returns>I18n locale relevant settings</returns>
        I18nLocaleRelevantSettingsModel GetI18nLocaleRelevantSettings();

        /// <summary>
        /// Gets supported languages.
        /// </summary>
        /// <returns>Supported languages.</returns>
        Hashtable GetSupportedLanguages();

        #endregion Methods
    }
}
