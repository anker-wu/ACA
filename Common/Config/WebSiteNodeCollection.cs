#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: WebSiteNodeCollection.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2008-2014
 *
 *  Description:
 *
 *  Notes:
 * $Id: WebSiteNodeCollection.cs 276545 2014-08-04 02:15:30Z ACHIEVO\james.shi $.
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
    /// Define a collection of WebSiteNode According to 'webServiceConfiguration\webSites' in web.config.
    /// </summary>
    public sealed class WebSiteNodeCollection : ConfigurationElementCollection
    {
        #region Fields

        /// <summary>
        /// Web service default site.
        /// </summary>
        private const string WS_DEAULT_SITE = "defaultSite";

        /// <summary>
        /// Web service select web site.
        /// </summary>
        private const string WS_SECTION_WEB_SITE = "webSite";

        #endregion Fields

        #region Properties

        /// <summary>
        /// Gets the type of the ConfigurationElementCollection. 
        /// </summary>
        public override ConfigurationElementCollectionType CollectionType
        {
            get
            {
                return ConfigurationElementCollectionType.BasicMap;
            }
        }

        /// <summary>
        /// Gets the default site name.
        /// </summary>
        [ConfigurationProperty(WS_DEAULT_SITE, IsRequired = true)]
        public string DefaultSite
        {
            get
            {
                return this[WS_DEAULT_SITE] as string;
            }
        }

        /// <summary>
        ///  Gets the name used to identify this collection of elements in the configuration file.
        /// </summary>
        protected override string ElementName
        {
            get
            {
                return WS_SECTION_WEB_SITE;
            }
        }

        #endregion Properties

        #region Indexers

        /// <summary>
        /// Gets and sets the WebSiteNode element at the specified index location.
        /// </summary>
        /// <param name="index">The index location of the ConfigurationElement to return.</param>
        /// <returns>The WebSiteNode at the specified index. </returns>
        public WebSiteNode this[int index]
        {
            get
            {
                return BaseGet(index) as WebSiteNode;
            }

            set
            {
                if (BaseGet(index) != null)
                {
                    BaseRemoveAt(index);
                }

                BaseAdd(index, value);
            }
        }

        #endregion Indexers

        #region Methods

        /// <summary>
        /// creates a new ConfigurationElement.
        /// </summary>
        /// <returns>A new WebSiteNode as ConfigurationElement.</returns>
        protected override ConfigurationElement CreateNewElement()
        {
            return new WebSiteNode();
        }

        /// <summary>
        /// Gets the element key for a specified configuration element.
        /// </summary>
        /// <param name="element">The ConfigurationElement to return the key for. </param>
        /// <returns>An Object that acts as the key for the specified ConfigurationElement. </returns>
        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((WebSiteNode)element).Name;
        }

        #endregion Methods
    }
}