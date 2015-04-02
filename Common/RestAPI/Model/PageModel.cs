#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: PageModel.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2014
 *
 *  Description:
 *
 *  Notes:
 * $Id: PageModel.cs 130988 2009-05-15 10:48:01Z ACHIEVO\jackie.yu $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 *  07/24/2014           Jone.Lu              Initial.
 * </pre>
 */

#endregion Header

using Newtonsoft.Json;

namespace Accela.ACA.Common.RestAPI
{
    /// <summary>
    /// Rest API page model.
    /// </summary>
    public class PageModel
    {
        /// <summary>
        /// Gets or sets the offset.
        /// </summary>
        [JsonProperty("offset")]
        public int StartRow
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the limit that set max number of rows returned in one API call for performance.
        /// </summary>
        [JsonProperty("limit")]
        public int MaxNum
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the resource items total. 
        /// </summary>
        [JsonProperty("total")]
        public int TotalCount
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets a value indicating whether has more data or not.
        /// </summary>
        [JsonProperty("hasmore")]
        public bool HasMore
        {
            get;
            set;
        }
    }
}
