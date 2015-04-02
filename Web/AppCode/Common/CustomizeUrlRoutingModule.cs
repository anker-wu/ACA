#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: CustomizeUrlRoutingModule.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2014
 *
 *  Description:
 *
 *  Notes:
 *      $Id: CustomizeUrlRoutingModule.cs 266082 2014-02-18 03:28:27Z ACHIEVO\james.shi $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion

using System;
using System.Web;

using Accela.ACA.Common;
using Accela.ACA.Common.Log;
using Accela.ACA.Common.Util;
using log4net;

namespace Accela.ACA.Web.Common
{
    /// <summary>
    /// Customized page url routing HttpModule
    /// </summary>
    public class CustomizeUrlRoutingModule : IHttpModule
    {
        /// <summary>
        /// Logger instance.
        /// </summary>
        private static readonly ILog Logger = LogFactory.Instance.GetLogger(typeof(CustomizeUrlRoutingModule));

        /// <summary>
        /// Gets the full path of the current url
        /// </summary>
        public string FullyQualifiedApplicationPath
        {
            get
            {
                var appPath = string.Empty;
                var context = HttpContext.Current;

                if (context != null)
                {
                    // Format the fully qualified website url/name
                    appPath = string.Format(
                        "{0}://{1}{2}{3}",
                        ConfigManager.Protocol,
                        context.Request.Url.Host,
                        context.Request.Url.Port == 80 ? string.Empty : ":" + context.Request.Url.Port,
                        context.Request.ApplicationPath);
                }

                if (!appPath.EndsWith("/"))
                {
                    appPath += "/";
                }

                return appPath;
            }
        }

        /// <summary>
        /// the Initialize event of the HttpModule
        /// </summary>
        /// <param name="context">the HttpApplication instance</param>
        public void Init(HttpApplication context)
        {
            context.BeginRequest += new EventHandler(ContextBeginRequest);
        }

        /// <summary>
        /// Implement the Dispose method
        /// </summary>
        public void Dispose()
        {
        }

        /// <summary>
        /// the Begin request event of the httpModule
        /// </summary>
        /// <param name="sender">the sender of the event</param>
        /// <param name="e">the args of the event</param>
        private void ContextBeginRequest(object sender, EventArgs e)
        {
            HttpRequest request = HttpContext.Current.Request;
            string originalUrl = request.Path.Substring(request.ApplicationPath.Length);

            // original url is not ASPX page or it has converted to customized page url then return.
            if (!originalUrl.ToLower().Contains(".aspx")
                || originalUrl.ToLower().Contains(ACAConstant.DEFAULT_CUSTOMIZATION_DIRECTORY.ToLower()))
            {
                return;
            }

            CustomizationType customType = FileUtil.GetCustomizationType(originalUrl, string.Empty);
            
            if (!customType.Equals(CustomizationType.CustomPage))
            {
                return;
            }

            Logger.DebugFormat("BeginRequest Event ---{0}-----{1}", originalUrl, customType.ToString());
            Logger.DebugFormat("The File Path is ---{0}", HttpContext.Current.Server.MapPath("~/" + ConfigManager.CustomizationDirectory + "/" + originalUrl));
            
            bool firstRequest = !CustomizePageUrlMap.GetInstance().UrlMap.ContainsKey(originalUrl);
            string customizeRelativeUrl = string.Empty;

            // get the customized page url
            if (!firstRequest)
            {
                customizeRelativeUrl = CustomizePageUrlMap.GetInstance().UrlMap[originalUrl].ToString();
            }
            else
            {
                bool existCustomPage = UrlHelper.ExistCustomizationPage(request, ConfigManager.AgencyCode, ACAConstant.DEFAULT_CUSTOMIZATION_DIRECTORY);

                if (existCustomPage)
                {
                    customizeRelativeUrl = UrlHelper.GetCustomizationRelativeUrl(request, ConfigManager.AgencyCode, ACAConstant.DEFAULT_CUSTOMIZATION_DIRECTORY);
                    CustomizePageUrlMap.GetInstance().UrlMap[originalUrl] = customizeRelativeUrl;
                }
            }

            // redirect to the customized page
            if (!string.IsNullOrEmpty(customizeRelativeUrl))
            {
                string paraPart = string.Empty;

                if (request.RawUrl.IndexOf('?') != -1)
                {
                    paraPart = request.RawUrl.Substring(request.RawUrl.IndexOf('?'));
                }

                HttpContext.Current.Response.Redirect(FullyQualifiedApplicationPath + customizeRelativeUrl + paraPart);
            }
        }
    }
}
