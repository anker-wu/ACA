#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: I18nCultureUtil.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2008-2014
 *
 *  Description:
 *  I18nCultureUtil for getting I18n culture information.
 *
 *  Notes:
 * $Id: I18nCultureUtil.cs 276545 2014-08-04 02:15:30Z ACHIEVO\james.shi $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using Accela.ACA.Common.Log;
using Accela.ACA.WSProxy;
using log4net;

namespace Accela.ACA.Common.Util
{
    /// <summary>
    /// provides I18n culture to serve the framework. 
    /// </summary>
    public static class I18nCultureUtil
    {
        #region Fields

        /// <summary>
        /// User preferred culture
        /// </summary>
        private const string KEY_UserPreferredCulture = "UserPreferredCulture";

        /// <summary>
        /// User preferred culture info
        /// </summary>
        private const string KEY_UserPreferredCultureInfo = "UserPreferredCultureInfo";

        /// <summary>
        /// Ajax tool kit's number expression
        /// </summary>
        private const string NumberExp = "9";

        /// <summary>
        /// Ajax tool kit's letter expression
        /// </summary>
        private const string LetterExp = "L";

        /// <summary>
        /// Ajax tool kit's number and letter expression
        /// </summary>
        private const string AllExp = "N";

        /// <summary>
        /// number expression
        /// </summary>
        private const string NumberExpression = "[0-9]{1}";

        /// <summary>
        /// Letter expression
        /// </summary>
        private const string LetterExpression = "[A-Za-z]{1}";

        /// <summary>
        /// The optional letter expression
        /// </summary>
        private const string OptionalExpression = "[\\s]{1}";

        /// <summary>
        /// logger information.
        /// </summary>
        private static readonly ILog Logger = LogFactory.Instance.GetLogger(typeof(I18nCultureUtil));

        /// <summary>
        /// Used to comparing.
        /// </summary>
        private static List<string> _allCultureList = new List<string>();

        /// <summary>
        /// Service provider culture Info
        /// </summary>
        private static CultureInfo _currencyCultureInfo;

        /// <summary>
        /// Web service culture Info.
        /// </summary>
        private static CultureInfo _webServiceCultureInfo;

        /// <summary>
        /// US english culture info.
        /// </summary>
        private static CultureInfo _enUSCultureInfo;

        /// <summary>
        /// the I18N primary setting
        /// </summary>
        private static I18nPrimarySettingsModel _i18nPrimarySetting = I18nSettingsProvider.GetI18nPrimarySettings();

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initializes static members of the <see cref="I18nCultureUtil"/> class.
        /// </summary>
        static I18nCultureUtil()
        {
            BuildAllCultureList();
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// Gets or sets the I18N primary setting, for add cache hit ratio.
        /// </summary>
        public static I18nPrimarySettingsModel I18nPrimarySetting
        {
            get
            {
                return _i18nPrimarySetting;
            }

            set
            {
                _i18nPrimarySetting = value;
            }
        }

        /// <summary>
        /// Gets the default culture.
        /// </summary>
        /// <value>The default culture.</value>
        public static string DefaultCulture
        {
            get
            {
                string result = I18nPrimarySetting.primaryLanguage;

                if (!string.IsNullOrEmpty(result))
                {
                    result = ChangeCulture4DotNet(result);
                }

                return result;
            }
        }

        /// <summary>
        /// Gets a value indicating whether multi-language is enabled.
        /// </summary>
        /// <value><c>true</c> if multiple language is enabled; otherwise, <c>false</c>.</value>
        public static bool IsMultiLanguageEnabled
        {
            get
            {
                bool result = I18nPrimarySetting.multipleLanguageSupport;

                return result;
            }
        }

        /// <summary>
        /// Gets a value indicating whether language options is hid.
        /// </summary>
        /// <value><c>true</c> if language options is hid; otherwise, <c>false</c>.</value>
        public static bool IsLanguageOptionsHid
        {
            get
            {
                bool result = I18nPrimarySetting.hideLanguageOptions;

                if (!IsMultiLanguageEnabled)
                {
                    result = false;
                }

                return result;
            }
        }

        /// <summary>
        /// Gets a value indicating whether current environment in master language
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is in master language; otherwise, <c>false</c>.
        /// </value>
        public static bool IsInMasterLanguage
        {
            get
            {
                return UserPreferredCulture.Equals(DefaultCulture, StringComparison.InvariantCultureIgnoreCase);
            }
        }

        /// <summary>
        /// Gets a value indicating whether Chinese culture is enabled.
        /// </summary>
        /// <value>
        /// <c>true</c> if Chinese culture is enabled; otherwise, <c>false</c>.
        /// </value>
        public static bool IsChineseCultureEnabled
        {
            get
            {
                return I18nCultureUtil.UserPreferredCulture.StartsWith("zh-", StringComparison.InvariantCultureIgnoreCase);
            }
        }

        /// <summary>
        /// Gets address culture
        /// </summary>
        /// <value>The address culture.</value>
        public static string AddressCulture
        {
            get
            {
                string result = I18nPrimarySetting.addressLocale;

                if (!string.IsNullOrEmpty(result))
                {
                    result = ChangeCulture4DotNet(result);
                }

                if (!IsValidCulture(result))
                {
                    result = DefaultCulture;
                }

                return result;
            }
        }

        /// <summary>
        /// Gets or sets User preferred culture
        /// </summary>
        /// <value>The user preferred culture.</value>
        public static string UserPreferredCulture
        {
            get
            {
                // need to separate admin culture and daily culture when getting or setting.
                string cultureString = string.Empty;

                if (HttpContext.Current != null && HttpContext.Current.Session != null)
                {
                    cultureString = HttpContext.Current.Session[KEY_UserPreferredCulture] as string;
                }

                if (string.IsNullOrEmpty(cultureString))
                {
                    cultureString = GetUserPreferredCulture();

                    if (HttpContext.Current != null && HttpContext.Current.Session != null && !string.IsNullOrEmpty(cultureString))
                    {
                        UserPreferredCulture = cultureString;
                    }
                }

                return cultureString;
            }

            set
            {
                if (IsValidCulture(value))
                {
                    HttpContext.Current.Session[KEY_UserPreferredCulture] = value;
                    HttpContext.Current.Session[KEY_UserPreferredCultureInfo] = new CultureInfo(value);
                    SetCultureToCookie(value);
                }
            }
        }

        /// <summary>
        /// Gets User preferred culture information
        /// </summary>
        /// <value>The user preferred culture info.</value>
        public static CultureInfo UserPreferredCultureInfo
        {
            get
            {
                if (HttpContext.Current == null || HttpContext.Current.Session == null)
                {
                    return WebServiceCultureInfo;
                }

                if (null == HttpContext.Current.Session[KEY_UserPreferredCultureInfo] || (UserPreferredCulture != (HttpContext.Current.Session[KEY_UserPreferredCultureInfo] as CultureInfo).TwoLetterISOLanguageName))
                {
                    HttpContext.Current.Session[KEY_UserPreferredCultureInfo] = new CultureInfo(UserPreferredCulture);
                }

                return HttpContext.Current.Session[KEY_UserPreferredCultureInfo] as CultureInfo;
            }
        }

        /// <summary>
        /// Gets Property of web service culture info
        /// </summary>
        /// <value>The web service culture info.</value>
        public static CultureInfo WebServiceCultureInfo
        {
            get
            {
                if (_webServiceCultureInfo == null)
                {
                    _webServiceCultureInfo = new CultureInfo(ACAConstant.CULTURE_FOR_WEB_SERVICE);
                }

                return _webServiceCultureInfo;
            }
        }

        /// <summary>
        /// Gets the I18n settings provider.
        /// </summary>
        /// <value>The I18n settings provider.</value>
        private static II18nSettingsProvider I18nSettingsProvider
        {
            get
            {
                II18nSettingsProvider result = ObjectFactory.GetObject(typeof(II18nSettingsProvider)) as II18nSettingsProvider;

                return result;
            }
        }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Masks to expression.
        /// </summary>
        /// <param name="mask">The mask.</param>
        /// <returns>the expression.</returns>
        public static string ZipMaskToExpression(string mask)
        {
            StringBuilder expression = new StringBuilder();

            if (!string.IsNullOrEmpty(mask))
            {
                Regex regex = new Regex(MaskChars.OptionalMatchExpression);
                string optionMask = regex.Match(mask).Value;

                if (!string.IsNullOrEmpty(optionMask))
                {
                    mask = mask.Replace(optionMask, string.Empty);
                }

                expression.Append("^");
                expression.Append(mask.Replace(MaskChars.NumericChar.ToString(), NumberExpression)
                    .Replace(MaskChars.LetterChar.ToString(), LetterExpression));

                if (!string.IsNullOrEmpty(optionMask))
                {
                    optionMask = optionMask.Replace(MaskChars.LeftBracket.ToString(), string.Empty).Replace(
                            MaskChars.RightBracket.ToString(), string.Empty);

                    expression.Append("(");
                    expression.Append(
                        optionMask.Replace(MaskChars.NumericChar.ToString(), NumberExpression).Replace(
                            MaskChars.LetterChar.ToString(), LetterExpression));

                    expression.Append("|");

                    expression.Append(
                        optionMask.Replace(MaskChars.NumericChar.ToString(), OptionalExpression).Replace(
                            MaskChars.LetterChar.ToString(), OptionalExpression));

                    expression.Append(")?$");
                }
            }

            return expression.ToString();
        }

        /// <summary>
        /// Phones the mask to expression.
        /// </summary>
        /// <param name="mask">The mask.</param>
        /// <returns>The expression.</returns>
        public static string PhoneMaskToExpression(string mask)
        {
            string expression = string.Empty;

            if (!string.IsNullOrEmpty(mask))
            {
                expression = mask.Replace(MaskChars.NumericChar.ToString(), NumberExpression)
                    .Replace(MaskChars.LeftBracket.ToString(), "\\" + MaskChars.LeftBracket.ToString())
                    .Replace(MaskChars.RightBracket.ToString(), "\\" + MaskChars.RightBracket.ToString());
            }

            return expression;
        }

        /// <summary>
        /// AAs the mask to ajax mask.
        /// </summary>
        /// <param name="mask">The mask.</param>
        /// <returns>The AJAX mask.</returns>
        public static string AAZipMaskToAjaxMask(string mask)
        {
            string expression = string.Empty;

            if (!string.IsNullOrEmpty(mask))
            {
                expression = mask.Replace(MaskChars.NumericChar.ToString(), NumberExp)
                                .Replace(MaskChars.LetterChar.ToString(), LetterExp)
                                .Replace(MaskChars.All.ToString(), AllExp)
                                .Replace(MaskChars.LeftBracket.ToString(), string.Empty)
                                .Replace(MaskChars.RightBracket.ToString(), string.Empty);
            }

            return expression;
        }

        /// <summary>
        /// AAs the phone mask to ajax mask.
        /// </summary>
        /// <param name="mask">The mask.</param>
        /// <returns>The AJAX mask.</returns>
        public static string AAPhoneMaskToAjaxMask(string mask)
        {
            string expression = string.Empty;

            if (!string.IsNullOrEmpty(mask))
            {
                expression = mask.Replace(MaskChars.NumericChar.ToString(), NumberExp);
            }

            return expression;
        }

        /// <summary>
        /// Append culture flag to current string, it usually uses for cache
        /// </summary>
        /// <param name="s">Culture string</param>
        /// <param name="userPreferredCulture">user preferred culture</param>
        /// <returns>format string by userPreferredCulture</returns>
        public static string AppendCultureFlag(string s, string userPreferredCulture)
        {
            return string.Format("{0}{1}{2}", s, ACAConstant.SPLIT_CHAR, userPreferredCulture);
        }

        /// <summary>
        /// Append user preferred culture flag to current string, it usually uses for cache
        /// </summary>
        /// <param name="s">Culture string</param>
        /// <returns>format string by userPreferredCulture</returns>
        public static string AppendUserPreferredCultureFlag(string s)
        {
            return AppendCultureFlag(s, UserPreferredCulture);
        }

        /// <summary>
        /// combine culture for web service, the result looks like en_US 
        /// </summary>
        /// <param name="languageCode">language code.</param>
        /// <param name="regionalCode">regional code.</param>
        /// <returns>user preferred culture temp</returns>
        public static string CombineCulture4WS(string languageCode, string regionalCode)
        {
            string languageCodeTemp = string.IsNullOrEmpty(languageCode) ? string.Empty : languageCode;
            string regionalCodeTemp = string.IsNullOrEmpty(regionalCode) ? string.Empty : regionalCode;
            string userPreferredCultureTemp = string.Format("{0}{1}{2}", languageCodeTemp, regionalCodeTemp, ACAConstant.CULTURE_SEPARATOR_4_JAVA);

            return userPreferredCultureTemp;
        }

        /// <summary>
        /// Changes the culture for web service, the result looks like en_US 
        /// </summary>
        /// <param name="originalCulture">The original culture.</param>
        /// <returns>the culture changed for web service.</returns>
        public static string ChangeCulture4WS(string originalCulture)
        {
            string result = string.Empty;

            if (!string.IsNullOrEmpty(originalCulture))
            {
                result = originalCulture.Replace(ACAConstant.CULTURE_SEPARATOR_4_DOTNET, ACAConstant.CULTURE_SEPARATOR_4_JAVA);
            }

            return result;
        }

        /// <summary>
        /// Changes the culture for .Net 
        /// </summary>
        /// <param name="originalCulture">The original culture.</param>
        /// <returns>the culture changed for .Net.</returns>
        public static string ChangeCulture4DotNet(string originalCulture)
        {
            string result = string.Empty;

            if (!string.IsNullOrEmpty(originalCulture))
            {
                result = originalCulture.Replace(ACAConstant.CULTURE_SEPARATOR_4_JAVA, ACAConstant.CULTURE_SEPARATOR_4_DOTNET);
            }

            return result;
        }

        /// <summary>
        /// Get language code, for example "en".
        /// </summary>
        /// <param name="cultureName">culture name</param>
        /// <returns>Language Code</returns>
        public static string GetLanguageCode(string cultureName)
        {
            return GetLanguageCode(new CultureInfo(cultureName));
        }

        /// <summary>
        /// Get language code, for example "en".
        /// </summary>
        /// <param name="cultureInfo">culture information.</param>
        /// <returns>Language Code</returns>
        public static string GetLanguageCode(CultureInfo cultureInfo)
        {
            if (cultureInfo.IsNeutralCulture)
            {
                return cultureInfo.Name;
            }

            return cultureInfo.TwoLetterISOLanguageName;
        }

        /// <summary>
        /// Get language code for soap handler
        /// </summary>
        /// <returns>Language Code</returns>
        public static string GetLanguageCodeForSoapHandler()
        {
            return GetLanguageCode(UserPreferredCultureInfo);
        }

        /// <summary>
        /// Get regional code, for example "US"
        /// </summary>
        /// <param name="cultureName">culture name</param>
        /// <returns>Regional Code</returns>
        public static string GetRegionalCode(string cultureName)
        {
            return GetRegionalCode(new CultureInfo(cultureName));
        }

        /// <summary>
        /// Get regional code, for example "US"
        /// </summary>
        /// <param name="cultureInfo">culture Info</param>
        /// <returns>Regional Code</returns>
        public static string GetRegionalCode(CultureInfo cultureInfo)
        {
            if (cultureInfo.IsNeutralCulture)
            {
                return string.Empty;
            }

            RegionInfo ri = new RegionInfo(cultureInfo.LCID);

            return ri.Name;
        }

        /// <summary>
        /// Get regional code for soap handler
        /// </summary>
        /// <returns>Regional Code</returns>
        public static string GetRegionalCodeForSoapHandler()
        {
            return GetRegionalCode(UserPreferredCultureInfo);
        }

        /// <summary>
        /// Get supported culture list in the format "CultureName", for example: "en-US".
        /// </summary>
        /// <returns>Supported culture list</returns>
        public static List<string> GetSupportedCultureList()
        {
            Dictionary<string, string> supportedLanguageList = GetSupportedLanguageList();
            List<string> result = new List<string>();

            if (supportedLanguageList != null)
            {
                foreach (string key in supportedLanguageList.Keys)
                {
                    result.Add(key);
                }
            }

            return result;
        }

        /// <summary>
        /// Get supported language list in the format: "cultureName,LanguageName", for example: "en-US,English"
        /// </summary>
        /// <returns>Supported language list</returns>
        public static Dictionary<string, string> GetSupportedLanguageList()
        {
            Dictionary<string, string> result = new Dictionary<string, string>();

            Hashtable i18nLanguages = I18nSettingsProvider.GetSupportedLanguages();

            if (!IsMultiLanguageEnabled)
            {
                string defaultCulture4Java = I18nSettingsProvider.GetI18nPrimarySettings().primaryLanguage;
                string theKey = ChangeCulture4DotNet(defaultCulture4Java);
                string defaultCultureText = i18nLanguages.ContainsKey(theKey) ? i18nLanguages[theKey] as string : theKey;
                result.Add(theKey, defaultCultureText);
            }
            else if (i18nLanguages != null)
            {
                foreach (string key in i18nLanguages.Keys)
                {
                    string languageText = i18nLanguages[key] as string;
                    string theKey = ChangeCulture4DotNet(key);
                    result.Add(theKey, languageText);
                }
            }

            return result;
        }

        /// <summary>
        /// Get User Preferred Culture
        /// </summary>
        /// <returns>User preferred culture</returns>
        public static string GetUserPreferredCulture()
        {
            //get supported curtures
            List<string> supportedCultureList = GetSupportedCultureList();

            string defaultSpecificCultureFromWS = GetSupportedSpecificCulture(supportedCultureList, DefaultCulture);

            if (!IsMultiLanguageEnabled)
            {
                return DefaultCulture;
            }

            //from cookie setting
            string culture = GetCultureFromCookie();

            if (!string.IsNullOrEmpty(culture) && ListContains(supportedCultureList, culture))
            {
                return GetFormattedCulture(supportedCultureList, culture);
            }

            //from web service defautl language
            culture = defaultSpecificCultureFromWS;

            if (!string.IsNullOrEmpty(culture) && ListContains(supportedCultureList, culture))
            {
                return GetFormattedCulture(supportedCultureList, culture);
            }

            //from browser languages setting
            List<string> cultureListFromBrowser = GetCultureListFromBrowserSetting();

            foreach (string tempCulture in cultureListFromBrowser)
            {
                if (ListContains(supportedCultureList, tempCulture))
                {
                    return GetFormattedCulture(supportedCultureList, tempCulture);
                }
            }

            //return first culture in supported culture list.
            return supportedCultureList[0];
        }

        /// <summary>
        /// Remove culture flag from current string, it usually uses for cache
        /// </summary>
        /// <param name="s">the string content</param>
        /// <param name="userPreferredCulture">user preferred culture</param>
        /// <returns>format string after remove culture flag.</returns>
        public static string RemoveCultureFlag(string s, string userPreferredCulture)
        {
            return s.Replace(ACAConstant.SPLIT_CHAR + userPreferredCulture, string.Empty);
        }

        /// <summary>
        /// split culture
        /// </summary>
        /// <param name="cultureName">culture Name</param>
        /// <param name="languageCode">language code</param>
        /// <param name="regionalCode">regional code</param>
        public static void SplitCulture(string cultureName, ref string languageCode, ref string regionalCode)
        {
            if (!string.IsNullOrEmpty(cultureName))
            {
                CultureInfo ci = new CultureInfo(cultureName);
                languageCode = GetLanguageCode(cultureName);
                regionalCode = GetRegionalCode(cultureName);
            }
        }

        /// <summary>
        /// cache key separator
        /// private constant string CacheKeySeparator = @"^__^";
        ///  abstract culture information from the combined string.
        /// </summary>
        /// <param name="combinedString">combined string</param>
        /// <param name="originalKey">original Key.</param>
        /// <param name="culture">culture information.</param>
        public static void SplitKey(string combinedString, ref string originalKey, ref string culture)
        {
            if (!string.IsNullOrEmpty(combinedString))
            {
                string[] splitArray = combinedString.Split(new char[] { ACAConstant.SPLIT_CHAR }, StringSplitOptions.None);

                if (null != splitArray && splitArray.Length == 2)
                {
                    originalKey = splitArray[0];
                    culture = splitArray[1];
                }
            }
        }

        /// <summary>
        /// Separator for cache key of included agency code
        /// </summary>
        /// <param name="combinedString">combined string</param>
        /// <param name="agencyCode">agency code</param>
        /// <param name="originalKey">original Key</param>
        /// <param name="culture">The culture</param>
        public static void SplitKey(string combinedString, string agencyCode, ref string originalKey, ref string culture)
        {
            if (!string.IsNullOrEmpty(combinedString))
            {
                string[] splitArray = combinedString.Split(new char[] { ACAConstant.SPLIT_CHAR }, StringSplitOptions.None);

                if (null != splitArray && splitArray.Length == 2)
                {
                    SplitKey(combinedString, ref originalKey, ref culture);
                }
                else if (null != splitArray && splitArray.Length == 3 && splitArray[0].Equals(agencyCode, StringComparison.InvariantCultureIgnoreCase))
                {
                    originalKey = splitArray[1];
                    culture = splitArray[2];
                }
            }
        }

        /// <summary>
        /// Validate the culture.
        /// </summary>
        /// <param name="culture">culture parameter.</param>
        /// <returns>true - valid, false-invalid.</returns>
        public static bool ValidateCulture(string culture)
        {
            // if null, system get the default value by frameworks
            if (string.IsNullOrWhiteSpace(culture))
            {
                return true;
            }

            try
            {
                // if not throw exception than return true
                new CultureInfo(culture);
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Build all cultures list for comparing.
        /// all keyes are lowercase.
        /// </summary>
        private static void BuildAllCultureList()
        {
            CultureInfo[] cultureInfoArray = CultureInfo.GetCultures(CultureTypes.SpecificCultures);

            foreach (CultureInfo ci in cultureInfoArray)
            {
                if (!ci.IsNeutralCulture && !string.IsNullOrEmpty(ci.Name))
                {
                    _allCultureList.Add(ci.Name);
                }
            }
        }

        /// <summary>
        /// get culture from cookie
        /// </summary>
        /// <returns>Culture from cookie</returns>
        private static string GetCultureFromCookie()
        {
            HttpCookie theCookie = HttpContext.Current == null
                                       ? null
                                       : HttpContext.Current.Request.Cookies[CookieConstant.USER_PREFERRED_CULTURE];

            if (theCookie != null)
            {
                theCookie.HttpOnly = true;

                if (ListContains(_allCultureList, theCookie.Value))
                {
                    return theCookie.Value;
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
        /// get cultures list from browser setting
        /// </summary>
        /// <returns>Culture list from browser setting</returns>
        private static List<string> GetCultureListFromBrowserSetting()
        {
            List<string> cultureList = new List<string>();
            string language = null;

            if (HttpContext.Current != null && HttpContext.Current.Request.UserLanguages != null)
            {
                foreach (string tempLanguage in HttpContext.Current.Request.UserLanguages)
                {
                    if (tempLanguage.IndexOf(";", StringComparison.InvariantCulture) != -1)
                    {
                        language = tempLanguage.Substring(0, tempLanguage.IndexOf(";", StringComparison.InvariantCulture));
                    }
                    else
                    {
                        language = tempLanguage;
                    }

                    if (ListContains(_allCultureList, language))
                    {
                        cultureList.Add(language);
                    }
                }
            }

            return cultureList;
        }

        /// <summary>
        /// Get formatted culture.
        /// </summary>
        /// <param name="list">the list information.</param>
        /// <param name="culture">the culture information.</param>
        /// <returns>Formatted Culture</returns>
        private static string GetFormattedCulture(List<string> list, string culture)
        {
            if (list == null || culture == null)
            {
                return culture;
            }

            string lowercaseCulture = culture.ToLowerInvariant();

            foreach (string tempString in list)
            {
                if (tempString.ToLowerInvariant() == lowercaseCulture)
                {
                    return tempString;
                }
            }

            return culture;
        }

        /// <summary>
        /// get supported specific culture by language code
        /// </summary>
        /// <param name="supportedCultureList">supported culture list.</param>
        /// <param name="languageCode">language code</param>
        /// <returns>Supported Specific Culture</returns>
        private static string GetSupportedSpecificCulture(List<string> supportedCultureList, string languageCode)
        {
            if (string.IsNullOrEmpty(languageCode))
            {
                return string.Empty;
            }

            if (null != supportedCultureList)
            {
                foreach (string tempCulture in supportedCultureList)
                {
                    if (tempCulture.Equals(languageCode, StringComparison.InvariantCultureIgnoreCase) || tempCulture.StartsWith(languageCode + "-", StringComparison.InvariantCultureIgnoreCase))
                    {
                        return tempCulture;
                    }
                }
            }

            return string.Empty;
        }

        /// <summary>
        /// Check whether the culture is in supported culture list.
        /// </summary>
        /// <param name="culture">configured culture in I18NSettings.</param>
        /// <returns>true-valid culture, false-invalid culture.</returns>
        private static bool IsValidCulture(string culture)
        {
            bool result = false;

            List<string> supportedCultureList = GetSupportedCultureList();

            if (supportedCultureList != null)
            {
                foreach (string key in supportedCultureList)
                {
                    if (key.Equals(culture, StringComparison.InvariantCultureIgnoreCase))
                    {
                        result = true;
                        break;
                    }
                }
            }

            if (!result)
            {
                Logger.FatalFormat("The culture {0} is not supported,please check the I18NSETTINGS standard choice.", culture);
            }

            return result;
        }

        /// <summary>
        /// Check if the list contains the culture.
        /// </summary>
        /// <param name="list">the list information.</param>
        /// <param name="culture">the culture information.</param>
        /// <returns>true-list contain, false-list isn't contain.</returns>
        private static bool ListContains(List<string> list, string culture)
        {
            if (list == null || culture == null)
            {
                return false;
            }

            string lowercaseCulture = culture.ToLowerInvariant();

            foreach (string tempString in list)
            {
                if (tempString.ToLowerInvariant() == lowercaseCulture)
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// set culture to cookie
        /// </summary>
        /// <param name="culture">the culture information.</param>
        private static void SetCultureToCookie(string culture)
        {
            HttpCookie theCookie = null;

            /*
             * The Secure attribute will be lost if the Cookie already exists and still to re-create it.
             * So add logic to avoid the duplicate Cookie creation.
             */
            if (HttpContext.Current.Response.Cookies.AllKeys.Contains(CookieConstant.USER_PREFERRED_CULTURE))
            {
                theCookie = HttpContext.Current.Response.Cookies[CookieConstant.USER_PREFERRED_CULTURE];

                if (theCookie != null && !theCookie.Value.Equals(culture, StringComparison.OrdinalIgnoreCase))
                {
                    theCookie.Value = culture;
                }
            }
            else
            {
                theCookie = new HttpCookie(CookieConstant.USER_PREFERRED_CULTURE);
                theCookie.HttpOnly = true;
                theCookie.Value = culture;
                theCookie.Expires = DateTime.Now.AddDays(90);

                HttpContext.Current.Response.Cookies.Add(theCookie);
            }
        }

        #endregion Methods
    }
}