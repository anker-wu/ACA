#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: SubModuleEntity.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2014
 *
 *  Description:
 *  Provider a tool to access web.config.
 *
 *  Notes:
 *      $Id: SubModuleEntity.cs 183096 2014-8-8 03:00:43Z ACHIEVO\reid.wang $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using Newtonsoft.Json;

namespace Accela.ACA.Web.WebApi.Entity
{
    /// <summary>
    /// SUB Module Entity
    /// </summary>
    public class SubModuleEntity
    {
        /// <summary>
        /// SUB Module Name
        /// </summary>
        private string _subModuleName;

        /// <summary>
        /// string key
        /// </summary>
        private string _key;

        /// <summary>
        /// string URL
        /// </summary>
        private string _url;

        /// <summary>
        /// string URL
        /// </summary>
        private string _filterName;

        /// <summary>
        /// Gets or sets SUB module name
        /// </summary>
        [JsonProperty("subModuleName")]
        public string SubModuleName
        {
            get { return _subModuleName; }
            set { _subModuleName = value; }
        }

        /// <summary>
        /// Gets or sets key
        /// </summary>
         [JsonProperty("key")]
        public string Key
        {
            get { return _key; }
            set { _key = value; }
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

         /// <summary>
         /// Gets or sets URL
         /// </summary>
         [JsonProperty("filterName")]
         public string FilterName
         {
             get { return _filterName; }
             set { _filterName = value; }
         }
    }
}