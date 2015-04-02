#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: WebServiceNodeCollection.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2008-2014
 *
 *  Description:
 *
 *  Notes:
 * $Id: WebServiceNodeCollection.cs 276545 2014-08-04 02:15:30Z ACHIEVO\james.shi $.
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
    /// Define a collection of WebServiceNode According to 'webServiceConfiguration\webSites\webSite\webServices' in web.config.
    /// </summary>
    public sealed class WebServiceNodeCollection : ConfigurationElementCollection
    {
        #region Fields

        /// <summary>
        /// Web Service.
        /// </summary>
        private const string WS_SECTION_WEB_SERVICE = "webService";

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
        ///  Gets the name used to identify this collection of elements in the configuration file.
        /// </summary>
        protected override string ElementName
        {
            get
            {
                return WS_SECTION_WEB_SERVICE;
            }
        }

        #endregion Properties

        #region Indexers

        /// <summary>
        /// Gets the WebServiceNode element at the specified index location.
        /// Sets the WebServiceNode element at the specified index location.
        /// </summary>
        /// <param name="index">The index location of the ConfigurationElement to return.</param>
        /// <returns>The WebServiceNode at the specified index. </returns>
        public WebServiceNode this[int index]
        {
            get
            {
                return BaseGet(index) as WebServiceNode;
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
        /// <returns>A new WebServiceNode as ConfigurationElement.</returns>
        protected override ConfigurationElement CreateNewElement()
        {
            return new WebServiceNode();
        }

        /// <summary>
        /// Gets the element key for a specified configuration element.
        /// </summary>
        /// <param name="element">The ConfigurationElement to return the key for. </param>
        /// <returns>An Object that acts as the key for the specified ConfigurationElement. </returns>
        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((WebServiceNode)element).ID;
        }

        #endregion Methods
    }
}