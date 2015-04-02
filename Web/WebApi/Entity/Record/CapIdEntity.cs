#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: CapIdEntity.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2014
 *
 *  Description:
 *  Provider a tool to access web.config.
 *
 *  Notes:
 *      $Id: CapIdEntity.cs 183096 2014-8-8 03:00:43Z ACHIEVO\reid.wang $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using Newtonsoft.Json;

namespace Accela.ACA.Web.WebApi.Entity
{
    /// <summary>
    /// Cap Id Entity
    /// </summary>
    public class CapIdEntity
    {
        /// <summary>
        /// string id 1
        /// </summary>
        private string _id1;

        /// <summary>
        /// string id 2
        /// </summary>
        private string _id2;

        /// <summary>
        /// string id 3
        /// </summary>
        private string _id3;

        /// <summary>
        /// Gets or sets id 1
        /// </summary>
        [JsonProperty("id1")]
        public string Id1
        {
            get { return _id1; }
            set { _id1 = value; }
        }

        /// <summary>
        /// Gets or sets id 2
        /// </summary>
        [JsonProperty("id2")]
        public string Id2
        {
            get { return _id2; }
            set { _id2 = value; }
        }

        /// <summary>
        /// Gets or sets id 3
        /// </summary>
        [JsonProperty("id3")]
        public string Id3
        {
            get { return _id3; }
            set { _id3 = value; }
        }
    }
}