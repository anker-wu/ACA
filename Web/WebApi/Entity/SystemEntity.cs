#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: SystemEntity.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2014
 *
 *  Description:
 *  Provider a tool to access web.config.
 *
 *  Notes:
 *      $Id: SystemEntity.cs 183096 2014-8-8 03:00:43Z ACHIEVO\reid.wang $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header
using Newtonsoft.Json;

namespace Accela.ACA.Web.WebApi.Entity
{
    /// <summary>
    /// System Entity
    /// </summary>
    public class SystemEntity
    {
        /// <summary>
        /// service provider code
        /// </summary>
        private string _serviceProviderCode;

        /// <summary>
        /// map service id
        /// </summary>
        private string _mapServiceId;

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
        /// Gets or sets map service id
        /// </summary>
        [JsonProperty("mapServiceId")]
        public string MapServiceId
        {
            get { return _mapServiceId; }
            set { _mapServiceId = value; }
        }
    }
}