#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: AddressInfoEntity.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2014
 *
 *  Description:
 *  Provider a tool to access web.config.
 *
 *  Notes:
 *      $Id: AddressInfoEntity.cs 183096 2014-8-8 03:00:43Z ACHIEVO\reid.wang $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using Newtonsoft.Json;

namespace Accela.ACA.Web.WebApi.Entity
{
    /// <summary>
    /// Address Info Entity
    /// </summary>
    public class AddressInfoEntity
    {
        /// <summary>
        /// Address Entity
        /// </summary>
        private AddressEntity _attributes;

        /// <summary>
        /// Coordinator Entity
        /// </summary>
        private CoordinatorEntity _location;

        /// <summary>
        /// Gets or sets attributes
        /// </summary>
        [JsonProperty("attributes")]
        public AddressEntity Attributes
        {
            get { return _attributes; }
            set { _attributes = value; }
        }

        /// <summary>
        /// Gets or sets location
        /// </summary>
        [JsonProperty("location")]
        public CoordinatorEntity Location
        {
            get { return _location; }
            set { _location = value; }
        }
    }
}