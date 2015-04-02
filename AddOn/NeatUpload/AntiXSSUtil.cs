using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Security.Application;

namespace Brettle.Web.NeatUpload
{
    public class AntiXSSUtil
    {
        /// <summary>
        /// The Microsoft Anti-Cross Site Scripting Library can be used to provide additional protection to ASP.NET Web-based applications against Cross-Site Scripting (XSS) attacks.
        /// </summary>
        /// <param name="content">javascript need encode</param>
        /// <returns>encoded javascript</returns>
        public static string AntiXssJavaScriptEncode(string content)
        {
            string result = String.Empty;
            result = AntiXss.JavaScriptEncode(content, false);

            return result;
        }

        /// <summary>
        /// The Microsoft Anti-Cross Site Scripting Library can be used to provide additional protection to ASP.NET Web-based applications against Cross-Site Scripting (XSS) attacks.
        /// </summary>
        /// <param name="content">html need encode</param>
        /// <returns>encoded html</returns>
        public static string AntiXssHtmlEncode(string content)
        {
            string result = String.Empty;
            result = AntiXss.HtmlEncode(content);

            return result;
        }

        /// <summary>
        /// The Microsoft Anti-Cross Site Scripting Library can be used to provide additional protection to ASP.NET Web-based applications against Cross-Site Scripting (XSS) attacks.
        /// </summary>
        /// <param name="content">url need encode</param>
        /// <returns>encoded url</returns>
        public static string AntiXssUrlEncode(string context)
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
                        url += AntiXss.UrlEncode(tmp.Substring(tmp.IndexOf('=') + 1));
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

            return FilterJSChar(context);
        }

        /// <summary>
        /// Filter the special javascript charater
        /// </summary>
        /// <param name="content">the string to be filtered</param>
        /// <returns>the string has been filtered.</returns>
        public static string FilterJSChar(string content)
        {
            string result = content.Replace("\\", "\\\\").Replace("'", "\\'").Replace("\"", "\\\"");
            result = result.Replace("\r\n", "\\n").Replace("\r", String.Empty).Replace("\n", "\\n");
            return result;
        }

    }
}
