#region

/**
 * <pre>
 * 
 *  Accela Citizen Access
 *  File: LabelKeyController.cs
 * 
 *  Accela, Inc.
 *  Copyright (C): 2014
 * 
 *  Description:
 *  
 *  Notes:
 *      $Id:LabelKeyController.cs 77905 2014-08-27 12:49:28Z ACHIEVO\Reid.wang.fu $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion

using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Web.Http;
using Accela.ACA.BLL.Common;
using Accela.ACA.Common;
using Accela.ACA.Common.Common;
using Accela.ACA.Common.Util;
using Accela.ACA.Web.Common;
using Accela.Web.Controls;

namespace Accela.ACA.Web.WebApi
{
    /// <summary>
    /// Label key controller
    /// </summary>
    public class LabelKeyController : ApiController
    {
        /// <summary>
        /// Get the agency logo
        /// </summary>
        /// <param name="labelKey">a specific label key</param>
        /// <returns>Y or N</returns>
        [ActionName("Label-Key")]
        [HttpPost]
        public HttpResponseMessage LabelKeyByType([FromBody]LabelKey labelKey)
        {
            StringBuilder result = new StringBuilder();
            result.Append("{");
            string[] keys = labelKey.Keys;
            string route = labelKey.Route;

            if (keys.Length > 0)
            {
                Hashtable dataHs = new Hashtable();
                dataHs.Add("routeType", route);
                result.Append("\"routeType\":\"" + route + "\",");
                result.Append("\"agencyName\":\"" + ConfigManager.AgencyCode + "\",");

                for (int i = 0; i < keys.Length; i++)
                {
                    string key = keys[i];
                    string value  = LabelUtil.GetGlobalTextByKey(key);
                        
                    if (value.Contains("\""))
                    {
                        value = value.Replace("\"", "\\\"");
                    }

                    value = SafeHtmlValue(value);
                    result.Append("\"" + key + "\":\"" + value + "\",");
                }
            }

            result = ApiUtil.GetRealString(result);
            result.Append("}");

            return new HttpResponseMessage
            {
                Content = new StringContent(result.ToString())
            };
        }

        /// <summary>
        /// Get default label text
        /// </summary>
        /// <param name="labelKey">label key</param>
        /// <returns>the origin text of current language</returns>
        [ActionName("defaultLabel")]
        public string GetDefaultLabel(string labelKey)
        {
            ILabelConvertor lc = ObjectFactory.GetObject<ILabelConvertor>();

            var value = lc.GetGUITextByKey(labelKey);

            if (!string.IsNullOrWhiteSpace(value))
            {
                value = ScriptFilter.FilterScript(value, false);
            }

            return value;
        }

        /// <summary>
        /// Get default language text
        /// </summary>
        /// <param name="labelKey">label key</param>
        /// <returns>the text of default language</returns>
        [ActionName("defaultLanguageText")]
        public string GetDefaultLanguageText(string labelKey)
        {
            ILabelConvertor lc = ObjectFactory.GetObject<ILabelConvertor>();

            var value = lc.GetDefaultLanguageTextByKey(labelKey, string.Empty);

            if (!string.IsNullOrWhiteSpace(value))
            {
                value = ScriptFilter.FilterScript(value, false);
            }

            return value;
        }

        /// <summary>
        /// Get the GUI text by label key.
        /// </summary>
        /// <param name="key">label key which is unique.</param>
        /// <param name="cultureName">language culture type.</param>
        /// <returns>The GUI text</returns>
        [NonAction]
        public string GetGuiTextByKey(string key, string cultureName)
        {
            string result = string.Empty;
            Hashtable labels = GetLabelKeys(ConfigManager.AgencyCode, cultureName);

            if (labels.Contains(key))
            {
                result = labels[key].ToString();
            }

            if (result == key && key.EndsWith("|sub", StringComparison.OrdinalIgnoreCase))
            {
                result = string.Empty;
            }

            return result;
        }

        /// <summary>
        /// Get all label keys collection.
        /// </summary>
        /// <param name="agencyCode">agency code</param>
        /// <param name="cultureName">language culture type</param>
        /// <returns>label keys collection</returns>
         [NonAction]
        public Hashtable GetLabelKeys(string agencyCode, string cultureName)
        {
            ICacheManager cacheManager = ObjectFactory.GetObject<ICacheManager>();
            return cacheManager.GetCachedItem(agencyCode, agencyCode + ACAConstant.SPLIT_CHAR + I18nCultureUtil.AppendCultureFlag(CacheConstant.CACHE_KEY_LABEL, cultureName));
        }

         /// <summary>
         /// Converts a string to the safety  HTML
         /// </summary>
         /// <param name="str">source string</param>
         /// <returns>safe html</returns>
         [NonAction]
         private static string SafeHtmlValue(string str)
         {
             return string.IsNullOrEmpty(str) ? str : str.Replace("\r", string.Empty).Replace("\n", string.Empty);
         }

        /// <summary>
         /// LabelKey model
        /// </summary>
        public class LabelKey
        {
            /// <summary>
            /// Gets or sets route
            /// </summary>
            public string Route { get; set; }

            /// <summary>
            /// Gets or sets keys
            /// </summary>
            public string[] Keys { get; set; }
            
            /// <summary>
            /// Gets or sets culture name
            /// </summary>
            public string CultureName { get; set; }
        }
    }
}