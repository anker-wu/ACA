#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: ModuleEntity.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2014
 *
 *  Description:
 *  Provider a tool to access web.config.
 *
 *  Notes:
 *      $Id: ModuleEntity.cs 183096 2014-8-8 03:00:43Z ACHIEVO\reid.wang $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using Newtonsoft.Json;

namespace Accela.ACA.Web.WebApi.Entity
{
    /// <summary>
    /// Module Entity
    /// </summary>
    public class ModuleEntity
    {
        /// <summary>
        /// module title
        /// </summary>
        private string _moduleTitle;

        /// <summary>
        /// string module
        /// </summary>
        private string _module;

        /// <summary>
        /// string URL
        /// </summary>
        private string _url;

        /// <summary>
        /// Gets or sets module title
        /// </summary>
        [JsonProperty("moduleTitle")]
        public string ModuleTitle
        {
            get { return _moduleTitle; }
            set { _moduleTitle = value; }
        }

        /// <summary>
        /// Gets or sets module
        /// </summary>
        [JsonProperty("module")]
        public string Module
        {
            get { return _module; }
            set { _module = value; }
        }

        /// <summary>
        /// Gets or sets URL
        /// </summary>
        [JsonProperty("url")]
        public string Url
        {
            get { return _url; }
            set { _url = value; }
        }
    }
}