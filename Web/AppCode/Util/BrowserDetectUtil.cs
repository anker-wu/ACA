#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: BrowserDetectUtil.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2013-2014
 *
 *  Description:
 *
 * </pre>
 */
#endregion

using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;

using Accela.ACA.Common;
using Accela.ACA.Web.Common;

namespace Accela.ACA.Web.Util
{
    /// <summary>
    /// Browser detect utility
    /// </summary>
    public static class BrowserDetectUtil
    {
        /// <summary>
        /// Support browser
        /// </summary>
        private static SupportedBrowser[] _supportedBrowsers;

        /// <summary>
        /// Initializes static members of the <see cref="BrowserDetectUtil" /> class.
        /// </summary>
        static BrowserDetectUtil()
        {
            /*
             * Init the ACA supported browsers.
             * It need to add the new item if there is new browser will be supported.
             */
            _supportedBrowsers = new SupportedBrowser[]
            {
                new SupportedBrowser
                {
                    DisplayName = "Internet Explorer 10 & 11",
                    IndicateExpression = new[] { @".*(msie) (\d+).*", @"(trident/7).*rv:(\d+).*" },
                    VariableName = "$$InternetExplorer$$",
                    SupportedVersions = new string[] { "10", "11" }
                },
                new SupportedBrowser
                {
                    DisplayName = "Mozilla Firefox 32",
                    VariableName = "$$Firefox$$",
                    IndicateExpression = new[] { @"/.*(firefox)\/(\w.).*" },
                    SupportedVersions = new string[] { "32" }
                },
                new SupportedBrowser
                {
                    DisplayName = "Chrome 37",
                    VariableName = "$$Chrome$$",
                    IndicateExpression = new[] { @"/.*(chrome)\/(\w.).*/" },
                    SupportedVersions = new string[] { "37" }
                },
                new SupportedBrowser
                {
                    DisplayName = "Safari 6",
                    VariableName = "$$Safari$$",
                    SupportedVersions = new string[] { "6" },
                    IndicateExpression = new[] { @"/.*(apple).*version\/([\w.]).*safari.*/" }
                },
                new SupportedBrowser
                {
                    DisplayName = "Opera 24",
                    IndicateExpression = new[] { @".*(opr)\/(\d+).*" },
                    VariableName = "$$Opera$$",
                    SupportedVersions = new string[] { "24" }
                }
            };
        }

        /// <summary>
        /// Gets or sets a value indicating whether the browser detect logic is did.
        /// </summary>
        /// <returns>bool true or false</returns>
        public static bool IsDetected
        {
            get
            {
                bool isDetected = false;
                HttpCookie cookie = HttpContext.Current.Request.Cookies[CookieConstant.BROWSER_DETECT_FLAG];

                if (cookie != null && cookie.Value != null)
                {
                    isDetected = Convert.ToBoolean(cookie.Value);
                }

                return isDetected;
            }

            set
            {
                if (value)
                {
                    HttpCookie cookie = new HttpCookie(CookieConstant.BROWSER_DETECT_FLAG, Convert.ToString(value));
                    cookie.HttpOnly = true;
                    HttpContext.Current.Response.Cookies.Add(cookie);

                    //Delete the cookie after user close the Browser.
                    HttpContext.Current.Response.Cookies[CookieConstant.BROWSER_DETECT_FLAG].Expires = DateTime.MinValue;
                }
            }
        }

        /// <summary>
        /// Detect current browser and show the notice message if current browser does not be supported.
        /// </summary>
        /// <param name="moduleName">module name</param>
        /// <param name="session">session object</param>
        /// <param name="page">current page</param>
        public static void Detect(string moduleName, HttpSessionState session, Page page)
        {
            if (IsDetected)
            {
                return;
            }
            
            SupportedBrowser currentBrowser = null;
            string currentBrowserVersion = string.Empty;
            string userAgent = HttpContext.Current.Request.UserAgent.ToLower();
            string message = LabelUtil.GetTextByKey("aca_global_msg_browserdetect", moduleName);

            foreach (SupportedBrowser browser in _supportedBrowsers)
            {
                message = message.Replace(browser.VariableName, browser.DisplayName);

                if (currentBrowser == null)
                {
                    Match matchBrowser = null;

                    foreach (string exp in browser.IndicateExpression)
                    {
                        matchBrowser = Regex.Match(userAgent, exp);

                        if (matchBrowser.Success)
                        {
                            currentBrowser = browser;
                            currentBrowserVersion = matchBrowser.Groups[2].Value;
                            break;
                        }
                    }
                }
            }

            if (currentBrowser == null || !currentBrowser.SupportedVersions.Contains(currentBrowserVersion))
            {
                MessageUtil.ShowMessage(page, MessageType.Notice, message, false, -1);
            }

            IsDetected = true;
        }

        /// <summary>
        /// Support browser struct
        /// </summary>
        public class SupportedBrowser
        {
            /// <summary>
            /// Gets or sets the browser's display name in the notice message.
            /// </summary>
            public string DisplayName
            {
                get;
                set;
            }

            /// <summary>
            /// Gets or sets a regular expression array used to indicate the browser.
            /// </summary>
            public string[] IndicateExpression
            {
                get;
                set;
            }

            /// <summary>
            /// Gets or sets the variable name in the notice message used replace to the display name.
            /// </summary>
            public string VariableName
            {
                get;
                set;
            }

            /// <summary>
            /// Gets or sets supported browser versions.
            /// </summary>
            public string[] SupportedVersions
            {
                get;
                set;
            }
        }
    }
}