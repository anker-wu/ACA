#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: SpatialReferenceEntity.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2014
 *
 *  Description:
 *  Provider a tool to access web.config.
 *
 *  Notes:
 *      $Id: SpatialReferenceEntity.cs 183096 2014-8-8 03:00:43Z ACHIEVO\reid.wang $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using Newtonsoft.Json;

namespace Accela.ACA.Web.WebApi.Entity
{
    /// <summary>
    /// Spatial Reference Entity
    /// </summary>
    public class SpatialReferenceEntity
    {
        /// <summary>
        /// double WKID
        /// </summary>
        private double? _wkid;

        /// <summary>
        /// Gets or sets WKID
        /// </summary>
        [JsonProperty("wkid")]
        public double? Wkid
        {
            get { return _wkid; }
            set { _wkid = value; }
        }
    }
}