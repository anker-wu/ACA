#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: CapParcelItemAddressEntity.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2014
 *
 *  Description:
 *  Provider a tool to access web.config.
 *
 *  Notes:
 *      $Id: CapParcelItemAddressEntity.cs 183096 2014-8-8 03:00:43Z ACHIEVO\reid.wang $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using Newtonsoft.Json;

namespace Accela.ACA.Web.WebApi.Entity
{
    /// <summary>
    /// Cap Parcel Item Address Entity
    /// </summary>
    public class CapParcelItemAddressEntity
    {
        /// <summary>
        /// address entity
        /// </summary>
        private AddressEntity _address;

        /// <summary>
        /// GIS ID
        /// </summary>
        private string _gisId;

        /// <summary>
        /// string identifier
        /// </summary>
        private string _identifierDisplay;

        /// <summary>
        /// context type
        /// </summary>
        private string _contextType;

        /// <summary>
        /// Gets or sets address
        /// </summary>
        [JsonProperty("address")]
        public AddressEntity Address
        {
            get { return _address; }
            set { _address = value; }
        }

        /// <summary>
        /// Gets or sets GIS ID
        /// </summary>
        [JsonProperty("gisId")]
        public string GisId
        {
            get { return _gisId; }
            set { _gisId = value; }
        }

        /// <summary>
        /// Gets or sets identifier display
        /// </summary>
        [JsonProperty("identifierDisplay")]
        public string IdentifierDisplay
        {
            get { return _identifierDisplay; }
            set { _identifierDisplay = value; }
        }

        /// <summary>
        /// Gets or sets context type
        /// </summary>
        [JsonProperty("contextType")]
        public string ContextType
        {
            get { return _contextType; }
            set { _contextType = value; }
        }
    }
}