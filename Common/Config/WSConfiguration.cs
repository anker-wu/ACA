#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: WSConfiguration.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2008-2009
 *
 *  Description:
 *
 *  Notes:
 * $Id: WSConfiguration.cs 131314 2009-05-19 06:07:41Z ACHIEVO\jackie.yu $.
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
    /// Web Service Section object to load and handle the webServiceConfiguration section in web.config.
    /// </summary>
    public sealed class WSConfiguration : ConfigurationSection
    {
        #region Fields

        /// <summary>
        /// Web service configuration.
        /// </summary>
        private const string WS_SECTION_WEB_SERVICE_CONFIGURATION = "webServiceConfiguration";

        /// <summary>
        /// Web service select web site.
        /// </summary>
        private const string WS_SECTION_WEB_SITES = "webSites";

        #endregion Fields

        #region Properties

        /// <summary>
        /// Gets WebSite node collection.
        /// </summary>
        [ConfigurationProperty(WS_SECTION_WEB_SITES, IsRequired = true)]
        public WebSiteNodeCollection WebSites
        {
            get
            {
                return this[WS_SECTION_WEB_SITES] as WebSiteNodeCollection;
            }
        }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Gets the instance of WSConfiguration class.
        /// </summary>
        /// <returns>the WSConfiguration object.</returns>
        public static WSConfiguration GetConfig()
        {
            return ConfigurationManager.GetSection(WS_SECTION_WEB_SERVICE_CONFIGURATION) as WSConfiguration;
        }

        #endregion Methods
    }
}