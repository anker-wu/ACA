#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: I18nCulture.aspx.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2008-2014
 *
 *  Description:
 *
 *  Notes:
 *      $Id: I18nCulture.aspx.cs 277316 2014-08-14 02:02:02Z ACHIEVO\james.shi $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using System;
using System.Collections.Generic;
using System.Text;

using Accela.ACA.Common.Util;
using Accela.ACA.Web.Common;
using Accela.ACA.Web.Security;

/// <summary>
/// Page to handler I18N culture
/// </summary>
[SuppressCsrfCheck]
public partial class Admin_I18nCulture : System.Web.UI.Page
{
    #region Fields

    /// <summary>
    /// Action flag to change language
    /// </summary>
    private const string CHANGE_LANGUAGE = "ChangeLanguage";

    /// <summary>
    /// Action flag to get multi language is supported or not
    /// </summary>
    private const string ENABLE_MULTI_LANGUAGE = "MultiLanguageSupportEnable";

    /// <summary>
    /// Action flag to get default language
    /// </summary>
    private const string GET_DEFAULT_LANGUAGE = "GetDefaultLanguage";

    /// <summary>
    /// Action flag to get language list
    /// </summary>
    private const string GET_LANGUAGELIST = "GetLanguageList";

    /// <summary>
    /// Action flag to get preferred culture
    /// </summary>
    private const string GET_PREFERRED_CULTURE = "GetPreferredCulture";

    #endregion Fields

    #region Methods

    /// <summary>
    /// Raises the page load event
    /// </summary>
    /// <param name="sender">An object that contains the event sender.</param>
    /// <param name="e">A System.EventArgs object containing the event data.</param>
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!AppSession.IsAdmin)
        {
            Response.Redirect("../login.aspx");
        }

        switch (Request.Params["action"])
        {
            case GET_LANGUAGELIST:
                Dictionary<string, string> languageList = I18nCultureUtil.GetSupportedLanguageList();

                Response.Write(ConvertDictionaryToJson(languageList));
                break;
            case CHANGE_LANGUAGE:
                string language = Request.Params["language"];
                HandleLanguageSwitch(language);
                break;
            case GET_PREFERRED_CULTURE:
                string preferredCulture = GetPreferredCulture();

                Response.Write(preferredCulture);
                break;
            case GET_DEFAULT_LANGUAGE:
                string defaultLanguage = GetDefaultLanguage();

                Response.Write(defaultLanguage);
                break;

            case ENABLE_MULTI_LANGUAGE:
                Response.Write(MultiLanguageSupportEnable() ? 'Y' : 'N');
                break;
        }
    }

    /// <summary>
    /// Convert dictionary to json format
    /// </summary>
    /// <param name="dictionary">Dictionary object</param>
    /// <returns>Json format string</returns>
    private string ConvertDictionaryToJson(Dictionary<string, string> dictionary)
    {
        if (dictionary == null)
        {
            return "[['','']]";
        }

        StringBuilder jasonStr = new StringBuilder();
        jasonStr.Append("[");

        foreach (KeyValuePair<string, string> entry in dictionary)
        {
            jasonStr.Append("['" + entry.Key + "','" + entry.Value + "'],");
        }

        jasonStr.Remove(jasonStr.Length - 1, 1);
        jasonStr.Append("]");

        return jasonStr.ToString();
    }

    /// <summary>
    /// Get default language
    /// </summary>
    /// <returns>default language,like "en-US"</returns>
    private string GetDefaultLanguage()
    {
        string defaultLanguage = I18nCultureUtil.DefaultCulture;
        
        return defaultLanguage;
    }

    /// <summary>
    /// Get user preferred culture
    /// </summary>
    /// <returns>user preferred culture</returns>
    private string GetPreferredCulture()
    {
        return I18nCultureUtil.UserPreferredCulture;
    }

    /// <summary>
    /// handle language switch, if <c>I18nCultureUtil.UserPreferredCulture</c> changed, return true, otherwise, return false
    /// </summary>
    /// <param name="language">current user selected language</param>
    /// <returns>true if <c>I18nCultureUtil.UserPreferredCulture</c> changed, otherwise, false.</returns>
    private bool HandleLanguageSwitch(string language)
    {
        string culture = language;

        if (!string.IsNullOrEmpty(culture))
        {
            Dictionary<string, string> supportedCultureList = I18nCultureUtil.GetSupportedLanguageList();
            foreach (string key in supportedCultureList.Keys)
            {
                if (key.Equals(culture, StringComparison.InvariantCultureIgnoreCase))
                {
                    I18nCultureUtil.UserPreferredCulture = culture;
                    return true;
                }
            }
        }

        return false;
    }

    /// <summary>
    /// Check if multiple language is supported or not
    /// </summary>
    /// <returns>true if multiple language is supported, otherwise, false.</returns>
    private bool MultiLanguageSupportEnable()
    {
        return I18nCultureUtil.IsMultiLanguageEnabled;
    }

    #endregion Methods
}