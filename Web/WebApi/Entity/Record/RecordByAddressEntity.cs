#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: RecordByAddressEntity.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2014
 *
 *  Description:
 *  Provider a tool to access web.config.
 *
 *  Notes:
 *      $Id: RecordByAddressEntity.cs 183096 2014-8-8 03:00:43Z ACHIEVO\reid.wang $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using Newtonsoft.Json;

namespace Accela.ACA.Web.WebApi.Entity
{
    /// <summary>
    /// Record By Address Entity
    /// </summary>
    public class RecordByAddressEntity
    {
        /// <summary>
        /// Address Entity
        /// </summary>
        private AddressEntity _address;

        /// <summary>
        /// Record Entity
        /// </summary>
        private RecordEntity _record;

        /// <summary>
        /// context Type
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
        /// Gets or sets record
        /// </summary>
        [JsonProperty("record")]
        public RecordEntity Record
        {
            get { return _record; }
            set { _record = value; }
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