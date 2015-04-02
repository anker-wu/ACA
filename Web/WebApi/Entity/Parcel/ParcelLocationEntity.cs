#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: ParcelLocationEntity.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2014
 *
 *  Description:
 *  Provider a tool to access web.config.
 *
 *  Notes:
 *      $Id: ParcelLocationEntity.cs 183096 2014-8-8 03:00:43Z ACHIEVO\reid.wang $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using System.Collections.Generic;
using Newtonsoft.Json;

namespace Accela.ACA.Web.WebApi.Entity
{
    /// <summary>
    /// Parcel Location Entity
    /// </summary>
    public class ParcelLocationEntity
    {
        /// <summary>
        /// service provider code
        /// </summary>
        private string _serviceProviderCode;

        /// <summary>
        /// string language
        /// </summary>
        private string _language;

        /// <summary>
        /// string context
        /// </summary>
        private string _context;

        /// <summary>
        /// parcel list
        /// </summary>
        private List<ParcelEntity> _parcels;

        /// <summary>
        /// Gets or sets service provider code
        /// </summary>
        [JsonProperty("serviceProviderCode")]
        public string ServiceProviderCode
        {
            get { return _serviceProviderCode; }
            set { _serviceProviderCode = value; }
        }

        /// <summary>
        /// Gets or sets language
        /// </summary>
        [JsonProperty("language")]
        public string Language
        {
            get { return _language; }
            set { _language = value; }
        }

        /// <summary>
        /// Gets or sets context
        /// </summary>
        [JsonProperty("context")]
        public string Context
        {
            get { return _context; }
            set { _context = value; }
        }

        /// <summary>
        /// Gets or sets parcel list
        /// </summary>
        [JsonProperty("parcels")]
        public List<ParcelEntity> Parcels
        {
            get { return _parcels; }
            set { _parcels = value; }
        }
    }
}