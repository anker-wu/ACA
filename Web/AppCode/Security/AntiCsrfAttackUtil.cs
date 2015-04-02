#region Header

/*
 * <pre>
 *  Accela Citizen Access
 *  File: AntiCsrfAttackUtil.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2014
 *
 *  Description: A utility class to prevent the Cross-Site Request Forgery attacks.
 *
 * </pre>
 */

#endregion

using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using Accela.ACA.Common;
using Accela.ACA.Common.Log;
using Accela.ACA.Web.Common;
using log4net;

namespace Accela.ACA.Web.Security
{
    /// <summary>
    /// A utility class to prevent the Cross-Site Request Forgery attacks.
    /// </summary>
    public class AntiCsrfAttackUtil
    {
        /// <summary>
        /// Log instance.
        /// </summary>
        private static readonly ILog Log = LogFactory.Instance.GetLogger(typeof(AntiCsrfAttackUtil));

        /// <summary>
        /// A list to cache the suppressed items.
        /// </summary>
        private static readonly List<string> SuppressedItems = new List<string>();

        /// <summary>
        /// Trusted External Site List
        /// </summary>
        private static readonly List<string> TrustedExternalSites = GetTrustedExternalSites();

        /// <summary>
        /// Trusted Local Site List
        /// </summary>
        private static readonly List<string> TrustedLocalSites = GetTrustedLocalSites();

        /// <summary>
        /// The flag to indicates whether Disable url referrer check
        /// Note: In order to improve the performance, store it into memory all the time.
        /// </summary>
        private static bool _isUrlReferrerCheckEnabled = StandardChoiceUtil.IsEnableUrlRefererCheck();

        /// <summary>
        /// Gets the flag to indicates whether the resource file needs Checking Url Referrer
        /// </summary>
        /// <param name="context">The current http context</param>
        /// <returns>true or false</returns>
        public static bool IsNeedCheckUrlReferrer(HttpContext context)
        {
            // Disable URL Referer Check as default
            if (!_isUrlReferrerCheckEnabled)
            {
                return false;
            }

            // Only accept the POST request from the internal page
            if (!"POST".Equals(context.Request.RequestType, StringComparison.OrdinalIgnoreCase)
                || !"POST".Equals(context.Request.HttpMethod, StringComparison.OrdinalIgnoreCase))
            {
                return false;
            }

            // Exclude some special handler(.aspx or .ashx)
            bool isSuppressed = false;
            if (context.Handler != null)
            {
                isSuppressed = IsSuppressed(context.Handler);
            }
            else
            {
                Log.ErrorFormat("Not found the handler for the reuqest url {0}", context.Request.Url.AbsoluteUri);
            }

            return !isSuppressed;
        }

        /// <summary>
        /// Check the http header url referrer to determine if the POST request is from the trusted site
        /// Here is some cases:
        /// 1, Access ACA site in IIS manager directly. For example:<c>127.0.0.1 or localhost</c>
        /// 2, Access ACA site with domain or IP
        /// 3, Access ACA site with https or http protocol
        /// 4, Access ACA site under load balancer environment. To resolve this problem, it requires administrator to configure ACA sites in the web.config file
        /// 5, Access ACA site from the 3rd trusted external web site. To resolve this problem, it requires administrator to configure the 3rd trusted external site in the standard choice ACA_SECURITY_SETTING
        /// </summary>
        /// <param name="context">The current http context</param>
        /// <returns>true or false</returns>
        public static bool IsCsrfAttackByUrlReferrer(HttpContext context)
        {
            if (Log.IsDebugEnabled)
            {
                Log.DebugFormat(
                    "Checking Referer Header for url[{0}][{1}]: {2}",
                    context.Request.RequestType,
                    context.Request.HttpMethod,
                    context.Request.Url.AbsoluteUri);
            }

            // 1, Get URLReferrer value from Http Request Header
            string urlReferrer = string.Empty;
            if (context.Request.UrlReferrer != null)
            {
                urlReferrer = context.Request.UrlReferrer.ToString();
            }

            // 2, The referer is required
            if (string.IsNullOrEmpty(urlReferrer))
            {
                Log.DebugFormat("Empty URL Referrer for request url: {0}", context.Request.Url.AbsoluteUri);
                return true;
            }

            // 3, Check whether the POST request comes from the trusted local or external site
            if (IsTrustedLocalUrl(urlReferrer, context) || IsTrustedExternalUrl(urlReferrer))
            {
                return false;
            }

            Log.DebugFormat("Invalid URL Referrer {0} for request url: {1}", urlReferrer, context.Request.Url.AbsoluteUri);
            return true;
        }

        /// <summary>
        /// Is the trusted local web site url?
        /// Below is the core concept:
        /// 1, Initialize one static array of the different ACA url - TrustedLocalSites - to void the redundant arithmetic.
        /// 2, Put the frequently-used items at the front to improve the performance.
        ///    For example: 
        ///    A, <c>The Domain Format URL > The IP Format URL > The localhost URL > The 127.0.0.1 URL</c>
        ///    B, Https > Http (Note: The most ACA site supports https)
        /// 3, Just the check the http header UrlReferrer to determine the current request is trusted or not.
        /// </summary>
        /// <param name="siteUrl">the site url.</param>
        /// <param name="context">the current http context, it is just used to initialize the trusted local site url list.</param>
        /// <returns>true or false</returns>
        public static bool IsTrustedLocalUrl(string siteUrl, HttpContext context)
        {
            //1, Check if the url referrer exists in the existing trusted local site list
            if (TrustedLocalSites.Any(site => siteUrl.StartsWith(site, StringComparison.OrdinalIgnoreCase)))
            {
                return true;
            }

            //2, Check if the url referrer is same as the request url. 
            string applicationRoot = FileUtil.GetApplicationRoot(context);
            Uri requestUrl = context.Request.Url;
            string authority = requestUrl.Authority;
            string localSite = FileUtil.CombineWebPath(requestUrl.Scheme + "://" + authority, applicationRoot);

            // If not same, it is definitly not from the local site.
            if (!siteUrl.StartsWith(localSite, StringComparison.OrdinalIgnoreCase))
            {
                return false;
            }

            // 4, Insert the trusted local site url into the static array.
            lock (TrustedLocalSites)
            {
                if (!TrustedLocalSites.Contains(localSite))
                {
                    if (authority.StartsWith("localhost", StringComparison.OrdinalIgnoreCase)
                        || authority.StartsWith("127.0.0.1", StringComparison.OrdinalIgnoreCase))
                    {
                        TrustedLocalSites.Add(localSite);
                    }
                    else
                    {
                        TrustedLocalSites.Insert(0, localSite);
                    }

                    // Output the tusted sites for troubleshoot
                    foreach (var trustedSite in TrustedLocalSites)
                    {
                        Log.DebugFormat("Trusted Local Site: {0}", trustedSite);
                    }
                }
            }

            return true;
        }

        /// <summary>
        /// Is the trusted external web site url?
        /// </summary>
        /// <param name="siteUrl">the site url.</param>
        /// <returns>true or false</returns>
        public static bool IsTrustedExternalUrl(string siteUrl)
        {
            if (TrustedExternalSites != null)
            {
                if (TrustedExternalSites.Any(site => siteUrl.StartsWith(site, StringComparison.OrdinalIgnoreCase)))
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Load ACA Security Setting after make any changes
        /// </summary>
        public static void LoadSecuritySetting()
        {
            _isUrlReferrerCheckEnabled = StandardChoiceUtil.IsEnableUrlRefererCheck();

            TrustedExternalSites.Clear();
            TrustedExternalSites.AddRange(GetTrustedExternalSites());
        }

        /// <summary>
        /// Gets the flag to indicates the handler is marked as Suppressed.
        /// </summary>
        /// <param name="handler">Http Handler</param>
        /// <returns>true or false</returns>
        public static bool IsSuppressed(IHttpHandler handler)
        {
            bool isSuppressed = false;
            if (handler != null)
            {
                string assemblyName = handler.GetType().AssemblyQualifiedName;
                isSuppressed = SuppressedItems.Contains(assemblyName);

                if (!isSuppressed)
                {
                    if (handler.GetType().GetCustomAttributes(typeof(SuppressCsrfCheckAttribute), true).Any())
                    {
                        SuppressedItems.Add(assemblyName);
                        isSuppressed = true;
                    }
                }
            }

            return isSuppressed;
        }

        /// <summary>
        /// Get Trusted Local Sites
        /// Note: To resolve one different protocol issue - https VS http on load balancer environment, it 
        ///       requires administrator to configures the ACA Site Url in web.config
        /// </summary>
        /// <returns>A array of trusted local site url.</returns>
        private static List<string> GetTrustedLocalSites()
        {
            List<string> trustedLocalSites = new List<string>();

            // Add the trusted local sites configured in web.config
            string localSites = ConfigurationManager.AppSettings["TrustedSites"];
            if (!string.IsNullOrWhiteSpace(localSites))
            {
                string[] sites = localSites.Split(ACAConstant.COMMA_CHAR);
                trustedLocalSites = GetSiteList(sites);

                // 2, Output the tusted sites for troubleshoot
                foreach (var trustedSite in trustedLocalSites)
                {
                    Log.DebugFormat("Trusted Local Site: {0}", trustedSite);
                }
            }

            return trustedLocalSites;
        }

        /// <summary>
        /// Get Trusted External Sites
        /// </summary>
        /// <returns>A array of the trusted external sites.</returns>
        private static List<string> GetTrustedExternalSites()
        {
            List<string> trustedExternalSites = new List<string>();

            string[] tustedSites = StandardChoiceUtil.GetTrustedSiteUrls();

            // 1, Load the trusted external site configured in standard choice
            trustedExternalSites = GetSiteList(tustedSites);

            // 2, Output the tusted sites for troubleshoot
            foreach (var trustedSite in trustedExternalSites)
            {
                Log.DebugFormat("Trusted External Site: {0}", trustedSite);
            }

            return trustedExternalSites;
        }

        /// <summary>
        /// Get the trusted site list
        /// </summary>
        /// <param name="sites">Array of site list.</param>
        /// <returns>trusted site list</returns>
        private static List<string> GetSiteList(string[] sites)
        {
            var resultSiteList = new List<string>();

            // Return one empty list
            if (sites == null || sites.Length == 0)
            {
                return resultSiteList;
            }

            foreach (var site in sites)
            {
                if (string.IsNullOrWhiteSpace(site))
                {
                    continue;
                }

                // The site url must starts with http:// or https://
                if (!site.StartsWith("http", StringComparison.OrdinalIgnoreCase)
                    && !site.StartsWith("https", StringComparison.OrdinalIgnoreCase))
                {
                    Log.ErrorFormat("The incorrect site url:{0}, please starts with http:// or https://.", site);
                    continue;
                }

                if (!resultSiteList.Contains(site))
                {
                    resultSiteList.Add(site);
                }
            }

            return resultSiteList;
        }
    }
}