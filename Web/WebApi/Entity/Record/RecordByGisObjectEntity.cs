#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: RecordByGisObjectEntity.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2014
 *
 *  Description:
 *  Provider a tool to access web.config.
 *
 *  Notes:
 *      $Id: RecordByGisObjectEntity.cs 183096 2014-8-8 03:00:43Z ACHIEVO\reid.wang $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using Newtonsoft.Json;

namespace Accela.ACA.Web.WebApi.Entity
{
    /// <summary>
    /// Record By GIS Object Entity
    /// </summary>
    public class RecordByGisObjectEntity
    {
        /// <summary>
        /// string layer
        /// </summary>
        private string _layer;

        /// <summary>
        /// GIS ID
        /// </summary>
        private string _gisId;

        /// <summary>
        /// context type
        /// </summary>
        private string _contextType;

        /// <summary>
        /// record entity
        /// </summary>
        private RecordEntity _reocrd;

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
        /// Gets or sets record
        /// </summary>
        [JsonProperty("reocrd")]
        public RecordEntity Reocrd
        {
            get { return _reocrd; }
            set { _reocrd = value; }
        }
    }
}