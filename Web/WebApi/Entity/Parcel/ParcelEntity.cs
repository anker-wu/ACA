#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: ParcelEntity.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2014
 *
 *  Description:
 *  Provider a tool to access web.config.
 *
 *  Notes:
 *      $Id: ParcelEntity.cs 183096 2014-8-8 03:00:43Z ACHIEVO\reid.wang $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using Newtonsoft.Json;

namespace Accela.ACA.Web.WebApi.Entity
{
    /// <summary>
    /// Parcel Entity
    /// </summary>
    public class ParcelEntity
    {
        /// <summary>
        /// string address
        /// </summary>
        private string _address;

        /// <summary>
        /// string parcel UID
        /// </summary>
        private string _parcelUid;

        /// <summary>
        /// parcel number
        /// </summary>
        private string _parcelNumber;

        /// <summary>
        /// source sequence number
        /// </summary>
        private string _sourceSeqNumber;

        /// <summary>
        /// string owner
        /// </summary>
        private string _owner;

        /// <summary>
        /// unmasked Parcel Number
        /// </summary>
        private string _unmaskedParcelNumber;

        /// <summary>
        /// Gets or sets address
        /// </summary>
        [JsonProperty("address")]
        public string Address
        {
            get { return _address; }
            set { _address = value; }
        }

        /// <summary>
        /// Gets or sets parcel UID
        /// </summary>
        [JsonProperty("parcelUid")]
        public string ParcelUid
        {
            get { return _parcelUid; }
            set { _parcelUid = value; }
        }

        /// <summary>
        /// Gets or sets parcel Number
        /// </summary>
        [JsonProperty("parcelNumber")]
        public string ParcelNumber
        {
            get { return _parcelNumber; }
            set { _parcelNumber = value; }
        }

        /// <summary>
        /// Gets or sets source sequence number
        /// </summary>
        [JsonProperty("sourceSeqNumber")]
        public string SourceSeqNumber
        {
            get { return _sourceSeqNumber; }
            set { _sourceSeqNumber = value; }
        }

        /// <summary>
        /// Gets or sets owner
        /// </summary>
        [JsonProperty("owner")]
        public string Owner
        {
            get { return _owner; }
            set { _owner = value; }
        }

        /// <summary>
        /// Gets or sets unmasked parcel number
        /// </summary>
        [JsonProperty("unmaskedParcelNumber")]
        public string UnmaskedParcelNumber
        {
            get { return _unmaskedParcelNumber; }
            set { _unmaskedParcelNumber = value; }
        }
    }
}