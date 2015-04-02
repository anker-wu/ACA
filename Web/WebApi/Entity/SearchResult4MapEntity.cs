#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: SearchResult4MapEntity.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2014
 *
 *  Description:
 *  Provider a tool to access web.config.
 *
 *  Notes:
 *      $Id: SearchResult4MapEntity.cs 183096 2014-8-8 03:00:43Z ACHIEVO\reid.wang $.
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
    /// Search Result 4 Map Entity
    /// </summary>
    public class SearchResult4MapEntity
    {
        /// <summary>
        /// parcel list
        /// </summary>
        private IList<ParcelEntity> _parcelList;

        /// <summary>
        /// record entity list
        /// </summary>
        private IList<RecordEntity> _recordList;

        /// <summary>
        /// Gets or sets parcel list
        /// </summary>
        [JsonProperty("parcelList")]
        public IList<ParcelEntity> ParcelList
        {
            get { return _parcelList; }
            set { _parcelList = value; }
        }

        /// <summary>
        /// Gets or sets record list
        /// </summary>
        [JsonProperty("recordList")]
        public IList<RecordEntity> RecordList
        {
            get { return _recordList; }
            set { _recordList = value; }
        }
    }
}