#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: RestAPIBase.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2014
 *
 *  Description:
 *
 *  Notes:
 * $Id: RestAPIBase.cs 130988 2009-05-15 10:48:01Z ACHIEVO\jackie.yu $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 *  07/24/2014           Jone.Lu              Initial.
 * </pre>
 */

#endregion Header

using System;
using System.Net;
using Accela.ACA.Common.Log;
using Accela.ACA.Common.Util;
using Accela.ACA.WSProxy;
using log4net;
using Newtonsoft.Json.Linq;

namespace Accela.ACA.Common.RestAPI
{
    /// <summary>
    /// Base Rest API
    /// </summary>
    public class RestAPIBase
    {
        #region Fields

        /// <summary>
        /// Rest API query format offset.
        /// </summary>
        private const string QUERYFORMAT_OFFSET = "offset";

        /// <summary>
        /// Rest API query format limit.
        /// </summary>
        private const string QUERYFORMAT_LIMIT = "limit";

        /// <summary>
        /// Max limit each rest API.
        /// </summary>
        private const int MAX_LIMIT_EACH_RESTAPI = 1000;

        /// <summary>
        /// Logger object.
        /// </summary>
        private static readonly ILog Logger = LogFactory.Instance.GetLogger(typeof(RestAPIBase));

        /// <summary>
        /// rest API config name
        /// </summary>
        private string _apiConfigName = string.Empty;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="RestAPIBase" /> class.
        /// </summary>
        protected RestAPIBase()
        {
            _apiConfigName = GetType().Name;
        }

        #endregion Constructors

        /// <summary>
        /// Call REST API use HTTP Post method.
        /// </summary>
        /// <param name="restAPIUrl">Rest API url(don't attach offset and limit url parameter).</param>
        /// <param name="postData">post data, which format: { "key1":"value1", "key2":"value2" }</param>
        /// <param name="queryFormat">query format.</param>
        /// <returns>the post rest API response.</returns>
        protected ResponseModel HttpPost4RestAPI(string restAPIUrl, string postData, QueryFormat queryFormat = null)
        {
            ResponseModel apiResult = PostOrGetRestAPI(restAPIUrl, postData, queryFormat, HttpMethod.POST);
            return apiResult;
        }

        /// <summary>
        /// Call REST API use HTTP Get method.
        /// </summary>
        /// <param name="restAPIUrl">Rest API url(don't attach offset and limit url parameter).</param>
        /// <param name="queryFormat">query format.</param>
        /// <returns>Get Data</returns>
        protected ResponseModel HttpGet4RestAPI(string restAPIUrl, QueryFormat queryFormat = null)
        {
            ResponseModel apiResult = PostOrGetRestAPI(restAPIUrl, string.Empty, queryFormat, HttpMethod.GET); 
            return apiResult;
        }

        /// <summary>
        /// Rest API response.
        /// </summary>
        /// <param name="restAPIUrl">Rest API url(don't attach offset and limit url parameter).</param>
        /// <param name="postData">post data, which format with "{key:value}"</param>
        /// <param name="queryFormat">the rest API query format.</param>
        /// <param name="httpMethod">Rest API http method.</param>
        /// <returns>Post or get of the Rest API response.</returns>
        private ResponseModel PostOrGetRestAPI(string restAPIUrl, string postData, QueryFormat queryFormat, HttpMethod httpMethod)
        {
            ResponseModel result = null;
            int queriedCount = 0;

            while (true)
            {
                var offsetLimit = GetOffsetLimit(queriedCount, queryFormat);

                // If offsetLimit is null or result pageinfo hasMore is false, it means that query completed.                 
                if (offsetLimit == null || (result != null && result.PageInfo != null && !result.PageInfo.HasMore))
                {
                    break;
                }

                ResponseModel pieceResult = PostOrGetRestAPI(restAPIUrl, postData, offsetLimit, httpMethod);

                if (pieceResult == null)
                {
                    break;
                }

                /* 
                 * If status is unSuccess, then should return the unSuccess result.
                 * If PageInfo is null, it means that the rest api method always not produce page info.
                 */
                if (pieceResult.Status != HttpStatusCode.OK || pieceResult.PageInfo == null || !(pieceResult.Result is JArray))
                {
                    return pieceResult;
                }

                // Run here, it means that the Rest API method can return page info.
                JArray pieceArray = pieceResult.Result as JArray;
                queriedCount += pieceArray.Count;

                // If result is null, means that it first set the result.
                if (result == null)
                {
                    result = pieceResult;
                }
                else if (result.PageInfo != null && result.Result is JArray)
                {
                    JArray apiResultArray = result.Result as JArray;
                    apiResultArray.Add(pieceArray.Children());

                    // set the hasmore and maxnum.
                    result.PageInfo.HasMore = pieceResult.PageInfo.HasMore;
                    result.PageInfo.MaxNum = queriedCount;
                }
            }

            return result;
        } 

        /// <summary>
        /// Rest API response.
        /// </summary>
        /// <param name="restAPIUrl">Rest API url(don't attach offset and limit url parameter).</param>
        /// <param name="postData">post data, which format with "{key:value}"</param>
        /// <param name="offsetLimit">offset and limit info for rest API</param>
        /// <param name="httpMethod">Rest API http method.</param>
        /// <returns>Post or get of the Rest API response.</returns>
        private ResponseModel PostOrGetRestAPI(string restAPIUrl, string postData, Tuple<int, int> offsetLimit, HttpMethod httpMethod)
        {
            int offset = offsetLimit.Item1;
            int limit = offsetLimit.Item2;

            string urlParams = string.Format("{0}={1}&{2}={3}", QUERYFORMAT_OFFSET, offset, QUERYFORMAT_LIMIT, limit);
            string urlWithQueryFormatInfo = UrlHelper.CombineQueryString(restAPIUrl, urlParams);

            ResponseModel apiResult = null;
            string strAPIResult;

            if (httpMethod == HttpMethod.POST)
            {
                strAPIResult = RestAPIUtil.HttpPost(urlWithQueryFormatInfo, postData, _apiConfigName);
            }
            else
            {
                strAPIResult = RestAPIUtil.HttpGet(urlWithQueryFormatInfo, _apiConfigName);
            }

            if (!string.IsNullOrEmpty(strAPIResult))
            {
                ResponseModel tempModel = RestAPIUtil.ConvertToRestAPIResponseModel(strAPIResult);

                if (tempModel == null)
                {
                    return null;
                }

                if (tempModel.Status != HttpStatusCode.OK)
                {
                    Logger.Error(strAPIResult);
                }

                apiResult = tempModel;
            }

            return apiResult;
        }

        /// <summary>
        /// Get offset and limit for rest API.
        /// </summary>
        /// <param name="queriedCount">the queried count.</param>
        /// <param name="queryFormat">query format.</param>
        /// <returns>offset and limit info.</returns>
        private Tuple<int, int> GetOffsetLimit(int queriedCount, QueryFormat queryFormat)
        {
            Tuple<int, int> result = null;

            if (queryFormat == null)
            {
                result = new Tuple<int, int>(queriedCount, MAX_LIMIT_EACH_RESTAPI);
            }
            else
            {
                int needQueryCount = queryFormat.endRow - queryFormat.startRow + 1;

                if (queriedCount >= needQueryCount)
                {
                    return null;
                }

                int offset = (queryFormat.startRow - 1) + queriedCount;
                int limit = needQueryCount - queriedCount;

                result = new Tuple<int, int>(offset, limit);
            }

            return result;
        }
    }
}
