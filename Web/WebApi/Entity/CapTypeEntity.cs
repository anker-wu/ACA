#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: CapTypeEntity.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2014
 *
 *  Description:
 *  Provider a tool to access web.config.
 *
 *  Notes:
 *      $Id: CapTypeEntity.cs 183096 2014-8-8 03:00:43Z ACHIEVO\reid.wang $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using Newtonsoft.Json;

namespace Accela.ACA.Web.WebApi.Entity
{
    /// <summary>
    /// Cap Type Entity
    /// </summary>
    public class CapTypeEntity
    {
        /// <summary>
        /// cap type value
        /// </summary>
        private string _capTypeValue;

        /// <summary>
        /// cap type text
        /// </summary>
        private string _capTypeText;

        /// <summary>
        /// Gets or sets cap type value
        /// </summary>
        [JsonProperty("capTypeValue")]
        public string CapTypeValue
        {
            get { return _capTypeValue; }
            set { _capTypeValue = value; }
        }

        /// <summary>
        /// Gets or sets cap type text
        /// </summary>
        [JsonProperty("capTypeText")]
        public string CapTypeText
        {
            get { return _capTypeText; }
            set { _capTypeText = value; }
        }
    }
}