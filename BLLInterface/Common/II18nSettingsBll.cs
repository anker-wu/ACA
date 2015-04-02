#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: II18nSettingsBll.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2009-2014
 *
 *  Description:
 *
 *  Notes:
 * $Id: II18nSettingsBll.cs 277008 2014-08-09 08:37:50Z ACHIEVO\james.shi $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using Accela.ACA.WSProxy;

namespace Accela.ACA.BLL.Common
{
    /// <summary>
    /// An interface that defines all approaches to get I18n setting value.
    /// </summary>
    public interface II18nSettingsBll
    {
        #region Methods

        /// <summary>
        /// Gets I18n primary settings.
        /// e.g. currency locale,language options hidden setting,default language,multiple language support setting.
        /// </summary>
        /// <param name="agencyCode">The agency code.</param>
        /// <returns>I18n primary setting</returns>
        I18nPrimarySettingsModel GetI18nPrimarySettings(string agencyCode);

        /// <summary>
        /// Gets I18n locale relevant settings.
        /// e.g. short date format, long date format, customized date time format, currency symbol
        /// </summary>
        /// <param name="agencyCode">The agency code.</param>
        /// <returns>I18n locale relevant setting</returns>
        I18nLocaleRelevantSettingsModel GetI18nLocaleRelevantSettings(string agencyCode);

        #endregion Methods
    }
}
