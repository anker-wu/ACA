#region Header

/**
 *  Accela Citizen Access
 *  File: ScriptFilter.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2007-2014
 *
 *  Description:
 *   Provide Key-Value pair object.
 *
 *  Notes:
 * $Id: ScriptFilter.cs 279179 2014-10-13 08:59:15Z ACHIEVO\james.shi $.
 *  Revision History
 *  Date,                  Who,                 What
 */

#endregion Header

using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Microsoft.Security.Application;

namespace Accela.ACA.Common.Common
{
    /// <summary>
    /// supply filtering malicious code server
    /// </summary>
    public static class ScriptFilter
    {
        #region Fields

        /// <summary>
        /// the posterior script express.
        /// </summary>
        private const string POSTERIOR_SCRIPT_EXPRESS = @"(?i)</script([^>])*?>";

        /// <summary>
        /// the prior script express.
        /// </summary>
        private const string PRIOR_SCRIPT_EXPRESS = @"(?i)<script([^>])*?>";

        #endregion Fields

        #region Methods

        /// <summary>
        /// Encode Html mark 
        /// </summary>
        /// <param name="context">string context</param>
        /// <returns>html encode</returns>
        public static string EncodeHtml(string context)
        {
            if (string.IsNullOrEmpty(context))
            {
                return string.Empty;
            }

            return System.Web.HttpUtility.HtmlEncode(context);
        }

        /// <summary>
        /// Encode Html text and filter some specific characters
        /// </summary>
        /// <param name="context">string context</param>
        /// <returns>html encode by filter JS Char</returns>
        public static string EncodeHtmlEx(string context)
        {
            if (string.IsNullOrEmpty(context))
            {
                return string.Empty;
            }

            return FilterJSChar(System.Web.HttpUtility.HtmlEncode(context));
        }

        /// <summary>
        /// Encode json string
        /// </summary>
        /// <param name="json">json data.</param>
        /// <returns>Encode Json</returns>
        public static string EncodeJson(string json)
        {
            string encodeStr = string.Empty;

            if (!string.IsNullOrEmpty(json))
            {
                encodeStr = json.Replace("[", "&#91;").Replace("]", "&#93;").Replace("{", "&#123;").Replace("}", "&#125;").Replace("\"", "&quot;").Replace("´", "&acute;").Replace("'", "&#39;").Replace(",", "&sbquo;").Replace("<", "&lt;").Replace(">", "&gt;").Replace("/", "&#47;").Replace(@"\", "&#92;").Replace("^", "&#94;").Replace("*", "&#42;").Replace("\r\n", "&#92;r&#92;n").Replace("\n", "&#92;n");
            }

            return encodeStr;
        }

        /// <summary>
        /// Decode Json string
        /// </summary>
        /// <param name="json">Json format string</param>
        /// <returns>Content that has been decoded</returns>
        public static string DecodeJson(string json)
        {
            string decodeStr = string.Empty;

            if (!string.IsNullOrEmpty(json))
            {
                decodeStr = json.Replace("&#91;", "[").Replace("&#93;", "]").Replace("&#123;", "{").Replace("&#125;", "}").Replace("&quot;", "\"").Replace("&acute;", "´").Replace("&#39;", "'").Replace("&sbquo;", ",").Replace("&lt;", "<").Replace("&gt;", ">").Replace("&#47;", "/").Replace("&#92;", @"\").Replace("&#94;", "^").Replace("&#42;", "*");
            }

            return decodeStr;
        }

        /// <summary>
        /// Encode URL parameters and filter some specific characters 
        /// </summary>
        /// <param name="context">the string to be filtered</param>
        /// <returns>the string has been encoded.</returns>
        public static string EncodeUrlEx(string context)
        {
            if (string.IsNullOrEmpty(context))
            {
                return string.Empty;
            }

            if (context.IndexOf('?') > 0)
            {
                string url = context.Substring(0, context.IndexOf('?') + 1);
                string quesryString = context.Substring(context.IndexOf('?') + 1);
                string[] parameters = quesryString.Split('&');

                foreach (string tmp in parameters)
                {
                    if (tmp.IndexOf('=') > 0)
                    {
                        url += tmp.Substring(0, tmp.IndexOf('=') + 1);
                        url += AntiXssUrlEncode(tmp.Substring(tmp.IndexOf('=') + 1));
                        url += "&";
                    }
                    else
                    {
                        url += url + tmp;
                        url += "&";
                    }
                }

                url = url.Substring(0, url.Length - 1);
                context = url;
            }

            return context;
        }

        /// <summary>
        /// Filter the special javascript character
        /// </summary>
        /// <param name="content">the string to be filtered</param>
        /// <returns>the string has been filtered.</returns>
        public static string FilterJSChar(string content)
        {
            if (string.IsNullOrEmpty(content))
            {
                return string.Empty;
            }

            string result = content.Replace("\\", "\\\\").Replace("'", "\\'").Replace("\"", "\\\"");
            result = result.Replace("\r\n", "\\n").Replace("\r", string.Empty).Replace("\n", "\\n");
            return result;
        }

        /// <summary>
        /// filter script
        /// </summary>
        /// <param name="content">to be filtered string</param>
        /// <param name="isNeedEncode">If need,encode html mark</param>
        /// <returns>string after filter script</returns>
        public static string FilterScript(string content, bool isNeedEncode)
        {
            if (string.IsNullOrEmpty(content))
            {
                return string.Empty;
            }

            if (isNeedEncode)
            {
                return EncodeHtml(content);
            }
            else
            {
                content = Regex.Replace(content, PRIOR_SCRIPT_EXPRESS, new MatchEvaluator(CorrectString), RegexOptions.IgnoreCase | RegexOptions.CultureInvariant);
                return Regex.Replace(content, POSTERIOR_SCRIPT_EXPRESS, new MatchEvaluator(CorrectString), RegexOptions.IgnoreCase | RegexOptions.CultureInvariant);
            }
        }

        /// <summary>
        /// the function applies to asp.label,because asp.label has no label key,so it's isNeedEncode set to true
        /// </summary>
        /// <param name="content">string content</param>
        /// <returns>string after filter script</returns>
        public static string FilterScript(string content)
        {
            return FilterScript(content, true);
        }

        /// <summary>
        /// the function applies to asp.label,because asp.label has no label key,so it's isNeedEncode set to true
        /// </summary>
        /// <param name="content">string content</param>
        /// <returns>string after filter script ex</returns>
        public static string FilterScriptEx(object content)
        {
            string tmpContent = content == null ? string.Empty : content.ToString();
            return FilterScript(tmpContent, true);
        }

        /// <summary>
        /// Encodes input strings for use in HTML attributes.
        /// </summary>
        /// <param name="content">Un-trusted input is used as an HTML attribute.</param>
        /// <returns>encoded content.</returns>
        public static string AntiXssHtmlAttributeEncode(string content)
        {
            string result = AntiXss.HtmlAttributeEncode(content);

            return result;
        }

        /// <summary>
        /// The Microsoft Anti-Cross Site Scripting Library can be used to provide additional protection to ASP.NET Web-based applications against Cross-Site Scripting (XSS) attacks.
        /// </summary>
        /// <param name="content">javascript need encode</param>
        /// <returns>encoded javascript</returns>
        public static string AntiXssJavaScriptEncode(string content)
        {
            string result = string.Empty;
            result = AntiXss.JavaScriptEncode(content, false);

            return result;
        }

        /// <summary>
        /// The Microsoft Anti-Cross Site Scripting Library can be used to provide additional protection to ASP.NET Web-based applications against Cross-Site Scripting (XSS) attacks.
        /// </summary>
        /// <param name="content">url need encode</param>
        /// <returns>encoded url</returns>
        public static string AntiXssUrlEncode(string content)
        {
            string result = string.Empty;
            result = AntiXss.UrlEncode(content);

            return result;
        }

        /// <summary>
        /// The Microsoft Anti-Cross Site Scripting Library can be used to provide additional protection to ASP.NET Web-based applications against Cross-Site Scripting (XSS) attacks.
        /// </summary>
        /// <param name="content">html need encode</param>
        /// <returns>encoded html</returns>
        public static string AntiXssHtmlEncode(string content)
        {
            string result = string.Empty;
            result = AntiXss.HtmlEncode(content);

            return result;
        }

        /// <summary>
        /// Remove html tag
        /// </summary>
        /// <param name="input">input string</param>
        /// <returns>content that has removed HTML tags</returns>
        public static string RemoveHTMLTag(string input)
        {
            if (string.IsNullOrEmpty(input))
            {
                return input;
            }

            input = input.Replace("\r", string.Empty);
            input = input.Replace("\n", string.Empty);

            Regex reg = new Regex(@"<[^>]+>|</[^>]+>", RegexOptions.Singleline);

            return reg.Replace(input, string.Empty);
        }

        /// <summary>
        /// Check if unsafe data is contained or not
        /// </summary>
        /// <param name="contentArray">The content list that needs validation</param>
        /// <returns>True - Some unsafe data like XSS script exists in one content.</returns>
        public static bool IsUnSafeData(IList<string> contentArray)
        {
            bool flag = false;

            // Step-1: Get some soecial patterns of unsafe data
            IList<string> patterns = GetForbiddenScriptList();

            for (int i = 0; !flag && i < patterns.Count; i++)
            {
                foreach (string content in contentArray)
                {
                    // If any item is unsafe, return true
                    if (Regex.IsMatch(content, patterns[i], RegexOptions.IgnorePatternWhitespace))
                    {
                        flag = true;
                        break;
                    }
                }
            }

            return flag;
        }

        /// <summary>
        /// Formats the content of the CSV file.
        /// </summary>
        /// <param name="content">The file's content.</param>
        /// <param name="needRemoveHtmlTag">Indicates whether the HTML tag need removed.</param>
        /// <returns>Return the formatted content of the CSV file.</returns>
        public static string FormatCSVContent(string content, bool needRemoveHtmlTag)
        {
            string result = content;

            if (needRemoveHtmlTag)
            {
                result = RemoveHTMLTag(content);
            }

            if (!string.IsNullOrEmpty(result))
            {
                result = result.Replace(ACAConstant.HTML_NBSP, " ");
                result = result.Replace("\r\n", string.Empty).Trim();
                result = System.Web.HttpUtility.HtmlDecode(result);
                result = "\"" + result.Replace("\"", "\"\"") + "\"";
                result = System.Web.HttpUtility.HtmlEncode(result);
            }

            return result;
        }

        /// <summary>
        /// collect script need to forbidden
        /// </summary>
        /// <returns>the forbidden scripts</returns>
        private static IList<string> GetForbiddenScriptList()
        {
            IList<string> patterns = new List<string>();

            //forbiddened common script
            patterns.Add(@"/\*[^(\*/)]*\*/");

            //forbiddened html target
            patterns.Add(@"<[^>]+(\/)*>");
            patterns.Add(@"(?i)<script([^>])*?>");
            patterns.Add(@"(?i)</script([^>])*?>");
            patterns.Add(@"(?i)<img([^>])*?");
            patterns.Add(@"(?i)<xml([^>])*?");
            patterns.Add(@"(?i)<style([^>])*?");

            //forbiddened html attribute
            patterns.Add(@"javascript:");
            patterns.Add(@"vbscript:");
            patterns.Add(@"expression\([^)]*\)");

            return patterns;
        }

        /// <summary>
        /// replace all matching strings
        /// </summary>
        /// <param name="match">match express.</param>
        /// <returns>Correct string</returns>
        private static string CorrectString(Match match)
        {
            string matchValue = match.Value;
            return EncodeHtml(matchValue);
        }

        #endregion Methods
    }
}
