#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: WSFactory.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2008-2014
 *
 *  Description:
 *
 *  Notes:
 * $Id: WebServiceConfig.cs 276545 2014-08-04 02:15:30Z ACHIEVO\james.shi $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 *  08/15/2007           Jackie.Yu              Initial.
 * </pre>
 */

#endregion Header

using System;

namespace Accela.ACA.Common.Config
{
    /// <summary>
    /// Manage the web service config¡£
    /// Read the web service config information from Config\WebService.config file.
    /// </summary>
    public static class WebServiceConfig
    {
        #region Fields

        /// <summary>
        /// Max time out second.
        /// </summary>
        private const int MAX_TIMEOUT_SECOND = 10000;

        /// <summary>
        /// Default site.
        /// </summary>
        private static WebSiteNode _defaultSite = null;

        #endregion Fields

        #region Constructors

        #endregion Constructors

        #region Properties

        /// <summary>
        /// Gets the WebServiceConfiguration section object.
        /// </summary>
        private static WSConfiguration Configuration
        {
            get
            {
                return WSConfiguration.GetConfig();
            }
        }

        /// <summary>
        /// Gets the default web site node in web.config.
        /// </summary>
        private static WebSiteNode DefaultSite
        {
            get
            {
                if (_defaultSite == null)
                {
                    string defaultSiteName = Configuration.WebSites.DefaultSite;
                    bool isFoundDefaultSite = false;

                    foreach (WebSiteNode siteNode in Configuration.WebSites)
                    {
                        if (siteNode.Name == defaultSiteName)
                        {
                            _defaultSite = siteNode;
                            isFoundDefaultSite = true;
                            break;
                        }
                    }

                    // default site is required.
                    if (!isFoundDefaultSite)
                    {
                        string configError = string.Format("The default site:{0} can't be found in webServiceConfiguration section in web.config.", defaultSiteName);
                        throw new ACAException(configError);
                    }
                }

                return _defaultSite;
            }
        }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Get the default WebServiceParameter object.
        /// </summary>
        /// <returns>the default WebServiceParameter object.</returns>
        public static WebServiceParameter GetDefaultConfigParameter()
        {
            WebServiceParameter defaultConfig = new WebServiceParameter();
            defaultConfig.Url = DefaultSite.Url;
            defaultConfig.Timeout = ConvertSecondToMilliSecond(DefaultSite.Timeout);

            return defaultConfig;
        }

        /// <summary>
        /// Get WebServiceParameter for invoking web service.
        /// if not find the matched config node,which will return the default parameter.
        /// </summary>
        /// <param name="type">WebService class type.</param>
        /// <returns>The parameter for invoking web service.</returns>
        public static WebServiceParameter GetWebServiceParameter(Type type)
        {
            if (type == null)
            {
                throw new ACAException("parameter 'type' isn't allowed to be null.");
            }

            string wsFullName = type.FullName;

            // look for the custom web service config node by web service id(full name)
            WebServiceNode customNode = FindWebServiceNode(wsFullName);

            WebServiceParameter result;

            // if found the custom node, return the custom value.
            if (customNode != null)
            {
                result = ConvertConfigNodeToParameter(customNode);

                if (result.Url != null && !result.Url.ToLower().StartsWith("http://"))
                {
                    WebServiceParameter defaultConfig = GetDefaultConfigParameter().Clone();
                    result.Url = defaultConfig.Url.Replace("//", "#").Split('/')[0].Replace("#", "//") + (result.Url.StartsWith("/") ? string.Empty : "/") + result.Url;
                }
            }
            else
            {
                // if can't be found, return the default value with the default rule include class name.
                result = GetDefaultConfigParameter().Clone(); 
                result.Url = RebuildUrl(result.Url, wsFullName);
            }

            return result;
        }

        /// <summary>
        /// Convert the section node to WebServiceParameter object.
        /// </summary>
        /// <param name="customNode">WebServiceNode to be converted.</param>
        /// <returns>WebServiceParameter that has been converted from WebServiceNode.</returns>
        private static WebServiceParameter ConvertConfigNodeToParameter(WebServiceNode customNode)
        {
            WebServiceParameter parameter = new WebServiceParameter();
            parameter.ID = customNode.ID;
            parameter.Url = customNode.Url;
            parameter.Timeout = ConvertSecondToMilliSecond(customNode.Timeout);

            return parameter;
        }

        /// <summary>
        /// Convert second to millisecond.
        /// if the second value is too large, replace second parameter with MAX_TIMEOUT_SECOND.
        /// </summary>
        /// <param name="second">the second count</param>
        /// <returns>the millisecond.</returns>
        private static int ConvertSecondToMilliSecond(int second)
        {
            return (second > MAX_TIMEOUT_SECOND) ? MAX_TIMEOUT_SECOND * 1000 : second * 1000;
        }

        /// <summary>
        /// Look for the web service node in WebServices section.
        /// </summary>
        /// <param name="webServiceId">web service id in webService section.</param>
        /// <returns>the WebService node that be found, if not find,return null.</returns>
        private static WebServiceNode FindWebServiceNode(string webServiceId)
        {
            WebServiceNode result = null;

            foreach (WebSiteNode siteNode in Configuration.WebSites)
            {
                foreach (WebServiceNode wsNode in siteNode.WebServices)
                {
                    if (wsNode.ID == webServiceId)
                    {
                        result = wsNode;
                        break;
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// Build the full web service url according to the default rule.
        /// </summary>
        /// <param name="urlRoot">default site root.</param>
        /// <param name="wsFullName">proxy web service class full name.</param>
        /// <returns>the real web service url according the proxy web service.</returns>
        private static string RebuildUrl(string urlRoot, string wsFullName)
        {
            string result = urlRoot;

            int startIndex = wsFullName.LastIndexOf(".", StringComparison.InvariantCulture) + 1;
            string serviceName = wsFullName.Substring(startIndex);

            // remove the end of "Service"(length is 7),is generated by tools.
            // for example: PublicUserWebServiceService should be PublicUserWebService
            serviceName = serviceName.Remove(serviceName.Length - 7);

            if (!result.EndsWith("/", StringComparison.OrdinalIgnoreCase))
            {
                result += "/";
            }

            result += serviceName;

            return result;
        }

        #endregion Methods
    }
}