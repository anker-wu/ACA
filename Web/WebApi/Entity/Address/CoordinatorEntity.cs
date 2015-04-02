#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: CoordinatorEntity.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2014
 *
 *  Description:
 *  Provider a tool to access web.config.
 *
 *  Notes:
 *      $Id: CoordinatorEntity.cs 183096 2014-8-8 03:00:43Z ACHIEVO\reid.wang $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using Newtonsoft.Json;

namespace Accela.ACA.Web.WebApi.Entity
{
    /// <summary>
    /// Coordinator Entity
    /// </summary>
    public class CoordinatorEntity
    {
        /// <summary>
        /// double x
        /// </summary>
        private double? _x;

        /// <summary>
        /// double y
        /// </summary>
        private double? _y;

        /// <summary>
        /// Spatial Reference Entity
        /// </summary>
        private SpatialReferenceEntity _spatialReference;

        /// <summary>
        /// Gets or sets Spatial Reference Entity
        /// </summary>
        [JsonProperty("spatialReference")]
        public SpatialReferenceEntity SpatialReference
        {
            get { return _spatialReference; }
            set { _spatialReference = value; }
        }

        /// <summary>
        /// Gets or sets y
        /// </summary>
        [JsonProperty("y")]
        public double? YCoordinator
        {
            get { return _y; }
            set { _y = value; }
        }

        /// <summary>
        /// Gets or sets x
        /// </summary>
        [JsonProperty("x")]
        public double? XCoordinator
        {
            get { return _x; }
            set { _x = value; }
        }
    }
}