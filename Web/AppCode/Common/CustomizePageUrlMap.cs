#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: CustomizePageUrlMap.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2014
 *
 *  Description:
 *
 *  Notes:
 *      $Id: CustomizePageUrlMap.cs 266082 2014-02-18 03:28:27Z ACHIEVO\james.shi $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion

using System.Collections;

namespace Accela.ACA.Web.Common
{
    /// <summary>
    /// the singleton of the custom page url
    /// </summary>
    public class CustomizePageUrlMap
    {
        /// <summary>
        /// the object of the lock
        /// </summary>
        private static readonly object Object4Lock = new object();

        /// <summary>
        /// the instance of singleton
        /// </summary>
        private static CustomizePageUrlMap instance = null;

        /// <summary>
        /// Prevents a default instance of the CustomizePageUrlMap class from being created.
        /// </summary>
        private CustomizePageUrlMap()
        {
        }

        /// <summary>
        /// Gets the url map of the singleton.
        /// </summary>
        public Hashtable UrlMap
        {
            get
            {
                return Hashtable.Synchronized(new Hashtable());
            }
        }

        /// <summary>
        /// get the singleton of the CustomizePageUrlMap
        /// </summary>
        /// <returns>the instance of the CustomizePageUrlMap</returns>
        public static CustomizePageUrlMap GetInstance()
        {
            if (instance == null)
            {
                lock (Object4Lock)
                {
                    if (instance == null)
                    {
                        instance = new CustomizePageUrlMap();
                    }
                }
            }

            return instance;
        }
    }
}
