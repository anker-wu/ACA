#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: RestAPIConfigItem.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2014
 *
 *  Description:
 *
 *  Notes:
 * $Id: RestAPIConfigItem.cs 130988 2009-05-15 10:48:01Z ACHIEVO\jackie.yu $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 *  07/24/2014           Jone.Lu              Initial.
 * </pre>
 */

#endregion Header

using System.Text;

namespace Accela.ACA.Common.RestAPI
{
    /// <summary>
    /// Rest API config item object.
    /// </summary>
    public class RestAPIConfigItem
    {
        #region Properties

        /// <summary>
        /// Gets or sets the API config name.
        /// </summary>
        public string Name
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the default timeout value.
        /// </summary>
        public int Timeout
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the root url for API in this site.
        /// </summary>
        public string Url
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the API version in this site.
        /// </summary>
        public string Version
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the API default encoding format.
        /// </summary>
        public Encoding DefaultEncoding
        {
            get;
            set;
        }

        #endregion Properties
    }
}
