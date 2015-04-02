#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: I18nUtil.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2007-2014
 *
 *  Description:
 *  I18n Utilities.
 *
 *  Notes:
 *      $Id: I18nUtil.cs 277071 2014-08-11 03:38:26Z ACHIEVO\james.shi $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using System;
using System.Collections.Generic;
using Accela.ACA.BLL.AddressBuilder;
using Accela.ACA.BLL.Common;
using Accela.ACA.Common;
using Accela.ACA.Common.Util;

namespace Accela.ACA.Web.Common
{
    /// <summary>
    /// I18n utility for language switch panel.
    /// </summary>
    public sealed class I18nUtil
    {
        #region Methods

        /// <summary>
        /// Gets the language switch panel.
        /// </summary>
        /// <returns>language switch panel HTML</returns>
        public static string GetLanguageSwitchPanel()
        {
            return GetLanguageSwitchPanel(3);
        }

        /// <summary>
        /// Gets the language switch panel.
        /// </summary>
        /// <param name="languageCountInOneRow">The language count in one row.</param>
        /// <returns>language switch panel HTML</returns>
        public static string GetLanguageSwitchPanel(int languageCountInOneRow)
        {
            string outputString = string.Empty;
            Dictionary<string, string> supportedCultureList = I18nCultureUtil.GetSupportedLanguageList();

            bool isMultiLanguageEnabled = I18nCultureUtil.IsMultiLanguageEnabled;
            bool isLanguageOptionsHid = I18nCultureUtil.IsLanguageOptionsHid;
            bool isOnlyOneLanguageAvailable = supportedCultureList.Count <= 1;
            bool isToShowLanguageSwitchPanel = isMultiLanguageEnabled && !isLanguageOptionsHid && !isOnlyOneLanguageAvailable;

            string languageSwitchPanelJS = GetLanguageSwitchPanelJS(isToShowLanguageSwitchPanel);

            if (isToShowLanguageSwitchPanel)
            {
                string urlPattern = @"<a href='{0}?culture={1}'>{2}</a>";
                string currentCulture = I18nCultureUtil.UserPreferredCulture;
                List<string> languageOptionList = new List<string>();
                
                foreach (string key in supportedCultureList.Keys)
                {
                    string cultureLanguageText = supportedCultureList[key];

                    if ((languageOptionList.Count + 1) % languageCountInOneRow != 0)
                    {
                        cultureLanguageText = string.Format("{0}&nbsp;", cultureLanguageText);
                    }

                    if (currentCulture == key)
                    {
                        cultureLanguageText = string.Format("<span class='ACA_CurrentLanguageHeighLevel ACA_LanguageText'>{0}</span>", cultureLanguageText);
                    }
                    else
                    {
                        cultureLanguageText = string.Format("<span class='ACA_LanguageText'>{0}</span>", cultureLanguageText);
                    }

                    languageOptionList.Add(string.Format(urlPattern, FileUtil.ApplicationRoot, key, cultureLanguageText));
                }

                string panelString = I18nStringUtil.FormatToTableRow(languageCountInOneRow, languageOptionList.ToArray());
                panelString = string.Format(@"<span id=""spanLanguageOptions"" style=""display:none"">{0}</span>", panelString);
                outputString = GetTextDirectionStyleString() + languageSwitchPanelJS + panelString;
            }
            else
            {
                outputString = languageSwitchPanelJS;
            }

            return outputString;
        }

        /// <summary>
        /// Display License's ResState
        /// </summary>
        /// <param name="state">State of license</param>
        /// <param name="countryCode">The country code.</param>
        /// <returns>ResState of license.</returns>
        public static string DisplayStateForI18N(string state, string countryCode)
        {
            IAddressBuilderBll addressBuilder = ObjectFactory.GetObject<IAddressBuilderBll>();

            return addressBuilder.GetStateI18n(state, countryCode);
        }

        /// <summary>
        /// Get current module name base main language value of module name.
        /// </summary>
        /// <param name="moduleName">main language module name</param>
        /// <returns>current module name.</returns>
        public static string GetResModuleName(string moduleName)
        {
            string resultModuleName = string.Empty;

            IList<TabItem> tabItems = TabUtil.GetTabList();

            if (tabItems != null && tabItems.Count > 0)
            {
                foreach (TabItem item in tabItems)
                {
                    if (moduleName == item.Module && item.TabVisible && !string.IsNullOrEmpty(item.Label))
                    {
                        resultModuleName = LabelUtil.RemoveHtmlFormat(LabelUtil.GetTextByKey(item.Label, item.Module));

                        if (resultModuleName == LabelUtil.GetGlobalTextByKey("aca_sys_default_module_name"))
                        {
                            resultModuleName = DataUtil.AddBlankToString(item.Module);
                        }

                        break;
                    }
                }
            }

            return resultModuleName;
        }

        /// <summary>
        /// Gets the language switch panel JS.
        /// </summary>
        /// <param name="isToShowLanguageSwitchPanel">if set to <c>true</c> [is to show language switch panel].</param>
        /// <returns>language switch panel Javascript string</returns>
        private static string GetLanguageSwitchPanelJS(bool isToShowLanguageSwitchPanel)
        {
            string jsString = string.Empty;

            if (isToShowLanguageSwitchPanel)
            {
                jsString = @"
                    <script type=""text/JavaScript"" language=""JavaScript"">
                    var LanguageOptionsVisible=false;
                    function SetLanguageOptionsVisible(visible){
                        LanguageOptionsVisible=visible;
                        var spanLanguageOptions = document.getElementById(""spanLanguageOptions"");
                        spanLanguageOptions.style.display=visible?"""":""none"";
                    }

                    function DoIframeOnload(){
                        SetLanguageOptionsVisible(window.LanguageOptionsVisible)
                    }
                    </script>
                    ";
            }
            else
            {
                jsString = @"
                    <script type=""text/JavaScript"" language=""JavaScript"">
                    function DoIframeOnload(){
                    }
                    </script>
                    ";
            }

            return jsString;
        }

        /// <summary>
        /// Gets the text direction style string.
        /// </summary>
        /// <returns>the text direction style string</returns>
        private static string GetTextDirectionStyleString()
        {
            string style = string.Format(
                        @"<style type=""text/css"">
                        html
                        {{
                            direction:{0};
                        }}
                        </style>
                        ",
                         Accela.ACA.Common.Util.I18nCultureUtil.UserPreferredCultureInfo.TextInfo.IsRightToLeft ? "RTL" : "LTR");
            return style;
        }

        #endregion Methods
    }
}
