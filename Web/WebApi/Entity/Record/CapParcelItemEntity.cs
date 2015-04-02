#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: CapParcelItemEntity.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2014
 *
 *  Description:
 *  Provider a tool to access web.config.
 *
 *  Notes:
 *      $Id: CapParcelItemEntity.cs 183096 2014-8-8 03:00:43Z ACHIEVO\reid.wang $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using Newtonsoft.Json;

namespace Accela.ACA.Web.WebApi.Entity
{
    /// <summary>
    /// Cap Parcel Item Entity
    /// </summary>
    public class CapParcelItemEntity
    {
        /// <summary>
        /// string layer
        /// </summary>
        private string _layer;

        /// <summary>
        /// string GIS ID
        /// </summary>
        private string _gisId;

        /// <summary>
        /// context type
        /// </summary>
        private string _contextType;

        /// <summary>
        /// parcel key
        /// </summary>
        private string _pacerlKey;

        /// <summary>
        /// identifier display
        /// </summary>
        private string _identifierDisplay;

        /// <summary>
        /// Gets or sets layer
        /// </summary>
        [JsonProperty("layer")]
        public string Layer
        {
            get { return _layer; }
            set { _layer = value; }
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
        /// Gets or sets context type
        /// </summary>
        [JsonProperty("contextType")]
        public string ContextType
        {
            get { return _contextType; }
            set { _contextType = value; }
        }

        /// <summary>
        /// Gets or sets parcel key
        /// </summary>
        [JsonProperty("pacerlKey")]
        public string PacerlKey
        {
            get { return _pacerlKey; }
            set { _pacerlKey = value; }
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
    }
}