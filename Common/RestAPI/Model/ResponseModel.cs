#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: ResponseModel.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2014
 *
 *  Description:
 *
 *  Notes:
 * $Id: ResponseModel.cs 130988 2009-05-15 10:48:01Z ACHIEVO\jackie.yu $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 *  07/24/2014           Jone.Lu              Initial.
 * </pre>
 */

#endregion Header

using System.Net;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Accela.ACA.Common.RestAPI
{
    /// <summary>
    /// Rest API Response model.
    /// </summary>
    public class ResponseModel
    {
        /// <summary>
        /// Gets or sets the Accela REST API returns standard HTTP status,as defined in HTTP/1.1 Status Code Definitions.  
        /// </summary>
        [JsonProperty("status")]
        public HttpStatusCode Status
        {
            get;
            set;
        }

        #region ErrorInfo

        /// <summary>
        /// Gets or sets the error code, such as: “fid_unauthorized” etc
        /// </summary>
        [JsonProperty("code")]
        public string ErrorCode
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the error message is user friendly message for human
        /// </summary>
        [JsonProperty("message")]
        public string ErrorMessage
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the more error extra information
        /// </summary>
        [JsonProperty("more")]
        public string ErrorExtraInfo
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the error traceId that is required for Accela to trace the necessary log and other information to facilitate the trouble shooting
        /// </summary>
        [JsonProperty("traceId")]
        public string ErrorTraceId
        {
            get;
            set;
        }

        #endregion ErrorInfo

        /// <summary>
        /// Gets or sets the result which rest API return.
        /// </summary>
        [JsonProperty("result")]
        public object Result
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the page model for list.
        /// </summary>
        [JsonProperty("page")]
        public PageModel PageInfo
        {
            get;
            set;
        }
    }
}
