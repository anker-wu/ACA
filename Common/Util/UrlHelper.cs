#region Header

/**
* <pre>
*  Accela Citizen Access
*  File: UrlHelper.cs
*
*  Accela, Inc.
*  Copyright (C): 2014
*
*  Description: Url helper.
*
*  Notes:
* $Id$.
*  Revision History
*  Date,            Who,        What
*  Sep 23, 2011      Alan Hu     Initial.
* </pre>
*/

#endregion

using System.IO;
using System.Linq;
using System.Text;
using System.Web;

namespace Accela.ACA.Common.Util
{
    /// <summary>
    /// Url helper.
    /// </summary>
    public class UrlHelper
    {
        /// <summary>
        /// Combine queryString from append queryString into source url.
        /// </summary>
        /// <param name="sourceUrl">source url</param>
        /// <param name="appendQueryString">append queryString</param>
        /// <returns>Return a new url.The url combine append queryString.</returns>
        public static string CombineQueryString(string sourceUrl, string appendQueryString)
        {
            if (string.IsNullOrWhiteSpace(sourceUrl))
            {
                return string.Empty;
            }

            // 1. not exist append query string
            if (string.IsNullOrWhiteSpace(appendQueryString))
            {
                return sourceUrl;
            }

            string[] tempUrl = sourceUrl.Split('?');
            string sourceQueryString = tempUrl.Length > 1 ? tempUrl[1] : string.Empty;

            // 2. only exist append query string.
            if (string.IsNullOrWhiteSpace(sourceQueryString) && !string.IsNullOrWhiteSpace(appendQueryString))
            {
                return tempUrl[0] + "?" + appendQueryString;
            }

            // 3. both source and append query string exist, so combine it.
            var objSourceQueryString = HttpUtility.ParseQueryString(sourceQueryString);
            var objRoutingQueryString = HttpUtility.ParseQueryString(appendQueryString);

            StringBuilder appendValues = new StringBuilder();

            foreach (string key in objRoutingQueryString.AllKeys)
            {
                if (!string.IsNullOrEmpty(key) && !objSourceQueryString.AllKeys.Contains(key))
                {
                    appendValues.AppendFormat("&{0}={1}", key, objRoutingQueryString[key]);
                }
            }

            return string.Format("{0}{1}", sourceUrl, appendValues);
        }

        /// <summary>
        /// Redirect to specified page
        /// </summary>
        /// <param name="redirectUrl">the redirect url.</param>
        public static void RedirectForDeepLink(string redirectUrl)
        {
            bool redirectWithContainer = true;
            
            if (ValidationUtil.IsYes(HttpContext.Current.Request[UrlConstant.FROM_ACA]))
            {
                redirectWithContainer = false;
            }
            else
            {
                // From login page, the bridgeview page is loaded.
                if (HttpContext.Current.Request.UrlReferrer != null
                    && HttpContext.Current.Request.UrlReferrer.PathAndQuery != null)
                {
                    // From ACA
                    if (HttpContext.Current.Request.UrlReferrer.PathAndQuery.StartsWith(HttpContext.Current.Request.ApplicationPath))
                    {
                        redirectWithContainer = false;
                    }
                }

                if (HttpContext.Current.Session == null)
                {
                    redirectWithContainer = false;
                }
            } 
            
            if (redirectWithContainer)
            {
                if (redirectUrl.StartsWith(HttpContext.Current.Request.ApplicationPath))
                {
                    redirectUrl = redirectUrl.Substring(HttpContext.Current.Request.ApplicationPath.Length);
                }

                if (redirectUrl.StartsWith("/"))
                {
                    redirectUrl = redirectUrl.Substring(1);
                }

                HttpContext.Current.Session[ACAConstant.CURRENT_URL] = redirectUrl;
                HttpContext.Current.Response.Redirect(ACAConstant.URL_DEFAULT_PAGE);               
            }
            else
            {
                HttpContext.Current.Response.Redirect(redirectUrl);
            }
        }

        /// <summary>
        /// Keep current return URL and redirects a request to a new URL.
        /// The return URL is stored in "ReturnURL" query string parameters.
        /// </summary>
        /// <param name="url">The target location.</param>
        public static void KeepReturnUrlAndRedirect(string url)
        {
            HttpContext current = HttpContext.Current;
            string returnUrl = current.Request.QueryString[UrlConstant.RETURN_URL];

            if (!string.IsNullOrEmpty(returnUrl))
            {
                if (returnUrl.Contains("/") || returnUrl.Contains("&"))
                {
                    returnUrl = HttpUtility.UrlEncode(returnUrl);
                }

                if (url.IndexOf('?') < 0)
                {
                    url += "?" + UrlConstant.RETURN_URL + "=" + returnUrl;
                }
                else
                {
                    url += "&" + UrlConstant.RETURN_URL + "=" + returnUrl;
                }
            }

            current.Response.Redirect(url);
        }

        #region CustomizationUrlHelper

        /// <summary>
        /// whether exist customization page
        /// </summary>
        /// <param name="curRequest">Current Request</param>
        /// <param name="agencyCode">Agency Code</param>
        /// <param name="customRootDirectory">Custom Root Directory</param>
        /// <returns>exist customization page or not.</returns>
        public static bool ExistCustomizationPage(HttpRequest curRequest, string agencyCode, string customRootDirectory)
        {
            string customizePhysicalFileUrl = Convert2CustomizationPhysicalFileUrl(curRequest, agencyCode, customRootDirectory);
            bool bExist = File.Exists(customizePhysicalFileUrl);

            return bExist;
        }

        /// <summary>
        /// get Customization Relative Url
        /// </summary>
        /// <param name="curRequest">Current Request</param>
        /// <param name="agencyCode">Agency Code</param>
        /// <param name="customRootDirectory">Custom Root Directory</param>
        /// <returns>customization relative url.</returns>
        public static string GetCustomizationRelativeUrl(HttpRequest curRequest, string agencyCode, string customRootDirectory)
        {
            string customizePhysicalFileUrl = Convert2CustomizationPhysicalFileUrl(curRequest, agencyCode, customRootDirectory);
            string customizeRelativeUrl = Convert2RelativeUrlFromPhysicalPath(customizePhysicalFileUrl);

            return customizeRelativeUrl;
        }

        /// <summary>
        /// Convert to customization Physical File Url
        /// </summary>
        /// <param name="curRequest">Current Request</param>
        /// <param name="agencyCode">Agency Code</param>
        /// <param name="customRootDirectory">Custom Root Directory</param>
        /// <returns>customize physical file url.</returns>
        private static string Convert2CustomizationPhysicalFileUrl(HttpRequest curRequest, string agencyCode, string customRootDirectory)
        {
            string rootPhysicalFileUrl = curRequest.PhysicalApplicationPath;
            string relativePhysicalFileUrl = curRequest.PhysicalPath.Replace(rootPhysicalFileUrl, string.Empty);

            string customizePhysicalFileUrl = string.Format(
                                            "{0}{1}\\{2}{3}",
                                            rootPhysicalFileUrl,
                                            customRootDirectory,
                                            !string.IsNullOrEmpty(agencyCode) ? agencyCode + "\\" : string.Empty,
                                            relativePhysicalFileUrl);

            return customizePhysicalFileUrl;
        }

        /// <summary>
        /// Convert to customization Relative File Url
        /// </summary>
        /// <param name="physicalFileUrl">physical File Url</param>
        /// <returns>relative url.</returns>
        private static string Convert2RelativeUrlFromPhysicalPath(string physicalFileUrl)
        {
            string relativeUrl = physicalFileUrl.Replace(HttpContext.Current.Request.ServerVariables["APPL_PHYSICAL_PATH"], "/").Replace(@"\", "/");
            relativeUrl = relativeUrl.Substring(1);
            return relativeUrl;
        }

        #endregion
    }
}