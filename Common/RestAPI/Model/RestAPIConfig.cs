#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: RestAPIConfig.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2014
 *
 *  Description:
 *
 *  Notes:
 * $Id: RestAPIConfig.cs 130988 2009-05-15 10:48:01Z ACHIEVO\jackie.yu $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 *  07/24/2014           Jone.Lu              Initial.
 * </pre>
 */

#endregion Header

using System.Collections.Generic;
using System.Text;

namespace Accela.ACA.Common.RestAPI
{
    /// <summary>
    /// Rest API config object.
    /// </summary>
    public class RestAPIConfig
    {
        #region Fields

        /// <summary>
        /// the default time out(in seconds)
        /// </summary>
        private int _defaultTimeout = 300;

        /// <summary>
        /// the default encoding.
        /// </summary>
        private Encoding _defaultEncoding = Encoding.UTF8;

        #endregion Fields

        /// <summary>
        /// Gets or sets the default Rest API Name.
        /// </summary>
        public string DefaultAPIName
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the default API timeout value.
        /// </summary>
        public int DefaultTimeout
        {
            get
            {
                return _defaultTimeout;
            }

            set
            {
                _defaultTimeout = value;
            }
        }

        /// <summary>
        /// Gets or sets the API default encoding format.
        /// </summary>
        public Encoding DefaultEncoding
        {
            get
            {
                return _defaultEncoding;
            }

            set
            {
                _defaultEncoding = value;
            }
        }

        /// <summary>
        /// Gets or sets the rest API configuration Dictionary<!--<key:restAPIConfigName;value:RestAPIConfigItem>-->.
        /// </summary>
        public Dictionary<string, RestAPIConfigItem> APIConfigItems
        {
            get;
            set;
        }
    }
}
