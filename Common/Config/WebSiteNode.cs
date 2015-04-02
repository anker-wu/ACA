#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: WebSiteNode.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2008-2014
 *
 *  Description:
 *
 *  Notes:
 * $Id: WebSiteNode.cs 276545 2014-08-04 02:15:30Z ACHIEVO\james.shi $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 *  08/15/2007           Jackie.Yu              Initial.
 * </pre>
 */

#endregion Header

using System.Configuration;

namespace Accela.ACA.Common.Config
{
    /// <summary>
    /// Define Data Object for WebSiteNode Element According to 'webServiceConfiguration\webSites\webSite' in web.config.
    /// For getting web site node value from config file. 
    /// </summary>
    public sealed class WebSiteNode : ConfigurationElement
    {
        #region Fields

        /// <summary>
        /// Web services.
        /// </summary>
        private const string WS_SECTION_WEB_SERVICES = "webServices";

        #endregion Fields

        #region Properties

        /// <summary>
        /// Gets the site name.
        /// </summary>
        [ConfigurationProperty("name", IsRequired = true)]
        public string Name
        {
            get
            {
                return this["name"] as string;
            }
        }

        /// <summary>
        /// Gets the default timeout value for invoking web service.
        /// </summary>
        [ConfigurationProperty("timeout", IsRequired = true)]
        public int Timeout
        {
            get
            {
                return (int)this["timeout"];
            }
        }

        /// <summary>
        /// Gets the root url for invoking web service in this site.
        /// </summary>
        [ConfigurationProperty("url", IsRequired = true)]
        public string Url
        {
            get
            {
                return this["url"] as string;
            }
        }

        /// <summary>
        /// Gets the custom web service node collection in this site.
        /// </summary>
        [ConfigurationProperty(WS_SECTION_WEB_SERVICES, IsRequired = false)]
        public WebServiceNodeCollection WebServices
        {
            get
            {
                return this[WS_SECTION_WEB_SERVICES] as WebServiceNodeCollection;
            }
        }

        #endregion Properties
    }
}