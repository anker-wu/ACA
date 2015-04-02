#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: RestAPIUtil.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2014
 *
 *  Description:
 *
 *  Notes:
 * $Id: APIUtil.cs 130988 2009-05-15 10:48:01Z ACHIEVO\jackie.yu $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 *  07/24/2014           Jone.Lu              Initial.
 * </pre>
 */

#endregion Header

using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Web;
using System.Xml;
using Newtonsoft.Json;

namespace Accela.ACA.Common.RestAPI
{
    /// <summary>
    /// Rest API utility.
    /// </summary>
    public class RestAPIUtil
    {
        #region Fields

        /// <summary>
        /// Local instance of API configuration analyzer.
        /// </summary>
        private static RestAPIConfig _apiConfig;

        #endregion Fields

        #region Properties

        /// <summary>
        /// Gets the API config.
        /// </summary>
        private static RestAPIConfig APIConfig
        {
            get
            {
                if (_apiConfig != null)
                {
                    return _apiConfig;
                }

                RestAPIConfig configurations = new RestAPIConfig();
                var xmlDoc = new XmlDocument();
                xmlDoc.Load(HttpContext.Current.Server.MapPath("~/Config/RestAPI.config"));
                var node = xmlDoc.SelectSingleNode("/configuration/APIConfigs");

                if (node != null)
                {
                    if (node.Attributes != null && node.Attributes["defaultAPI"] != null)
                    {
                        configurations.DefaultAPIName = node.Attributes["defaultAPI"].Value;
                    }

                    if (node.Attributes != null && node.Attributes["defaultTimeout"] != null)
                    {
                        configurations.DefaultTimeout = int.Parse(node.Attributes["defaultTimeout"].Value);
                    }

                    if (node.Attributes != null && node.Attributes["defaultEncoding"] != null)
                    {
                        configurations.DefaultEncoding = Encoding.GetEncoding(node.Attributes["defaultEncoding"].Value);
                    }

                    foreach (XmlNode n in node.ChildNodes)
                    {
                        if (n.Attributes == null)
                        {
                            continue;
                        }

                        string name = string.Empty;

                        if (n.Attributes["name"] != null)
                        {
                            name = n.Attributes["name"].Value;
                        }

                        if (string.IsNullOrEmpty(name)
                            || (configurations.APIConfigItems != null && configurations.APIConfigItems.ContainsKey(name)))
                        {
                            continue;
                        }

                        var config = new RestAPIConfigItem();
                        config.Name = name;

                        if (n.Attributes["url"] != null)
                        {
                            config.Url = n.Attributes["url"].Value.TrimEnd('/');
                        }

                        if (n.Attributes["version"] != null)
                        {
                            config.Version = n.Attributes["version"].Value;
                        }

                        config.Timeout = n.Attributes["timeout"] != null ? int.Parse(n.Attributes["timeout"].Value) : configurations.DefaultTimeout;

                        config.DefaultEncoding = n.Attributes["encoding"] != null ? Encoding.GetEncoding(n.Attributes["encoding"].Value)
                                                                                    : configurations.DefaultEncoding;

                        if (configurations.APIConfigItems == null)
                        {
                            configurations.APIConfigItems = new Dictionary<string, RestAPIConfigItem>();
                        }

                        configurations.APIConfigItems.Add(name, config);
                    }
                }

                _apiConfig = configurations;
                return _apiConfig;
            }
        }

        #endregion Properties

        /// <summary>
        /// Post to Rest API.
        /// </summary>
        /// <param name="restAPIUrl">Rest API url(don't attach offset and limit url parameter).</param>
        /// <param name="postData">post data, which format with "{key:value}"</param>
        /// <param name="apiConfigName">API config name</param>
        /// <returns>the post API result.</returns>
        internal static string HttpPost(string restAPIUrl, string postData, string apiConfigName)
        {
            RestAPIConfigItem apiConfig = GetAPIConfiguration(apiConfigName);
            string apiUrl = apiConfig.Url + string.Format(restAPIUrl, apiConfig.Version);

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(apiUrl);
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            request.Timeout = apiConfig.Timeout * 1000;

            byte[] data = apiConfig.DefaultEncoding.GetBytes(postData);
            request.ContentLength = data.Length;

            using (Stream stream = request.GetRequestStream())
            {
                stream.Write(data, 0, data.Length);
            }

            try
            {
                return GetResponseContent(request.GetResponse());
            }
            catch (WebException e)
            {
                if (e.Response == null)
                {
                    throw new ACAException(e);
                }

                return GetResponseContent(e.Response);
            }
        }

        /// <summary>
        /// Get from rest API.
        /// </summary>
        /// <param name="restAPIUrl">Rest API url(don't attach offset and limit url parameter).</param>
        /// <param name="apiConfigName">API config name.</param>
        /// <returns>Get Data</returns>
        internal static string HttpGet(string restAPIUrl, string apiConfigName)
        {
            try
            {
                RestAPIConfigItem apiConfigItem = GetAPIConfiguration(apiConfigName);
                string apiUrl = apiConfigItem.Url + string.Format(restAPIUrl, apiConfigItem.Version);

                Uri uri = new Uri(apiUrl);
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(uri);
                request.Method = "GET";
                request.ContentType = "application/x-www-form-urlencoded";
                request.AllowAutoRedirect = false;
                request.Timeout = apiConfigItem.Timeout * 1000;

                return GetResponseContent(request.GetResponse());
            }
            catch (WebException e)
            {
                if (e.Response == null)
                {
                    throw new ACAException(e);
                }

                return GetResponseContent(e.Response);
            }
        }

        /// <summary>
        /// Convert to RestAPIResponse Model.
        /// </summary>
        /// <param name="strRestAPIResponse">Rest API Response</param>
        /// <returns>RestAPIResponse Model.</returns>
        internal static ResponseModel ConvertToRestAPIResponseModel(string strRestAPIResponse)
        {
            if (string.IsNullOrEmpty(strRestAPIResponse))
            {
                return null;
            }

            ResponseModel result = (ResponseModel)JsonConvert.DeserializeObject(strRestAPIResponse, typeof(ResponseModel));

            return result;
        }

        /// <summary>
        /// Get API config according the API config name, if no exist, return the default API config.
        /// </summary>
        /// <param name="apiConfigName">API config name</param>
        /// <returns>the API config</returns>
        private static RestAPIConfigItem GetAPIConfiguration(string apiConfigName)
        {
            RestAPIConfigItem apiConfiguration = null;

            if (APIConfig.APIConfigItems != null)
            {
                if (APIConfig.APIConfigItems.ContainsKey(apiConfigName))
                {
                    apiConfiguration = APIConfig.APIConfigItems[apiConfigName];
                }
                else if (APIConfig.APIConfigItems.ContainsKey(APIConfig.DefaultAPIName))
                {
                    apiConfiguration = APIConfig.APIConfigItems[APIConfig.DefaultAPIName];
                }
            }

            return apiConfiguration;
        }

        /// <summary>
        /// Get response content.
        /// </summary>
        /// <param name="webResponse">web response.</param>
        /// <returns>Response content.</returns>
        private static string GetResponseContent(WebResponse webResponse)
        {
            string result = string.Empty;

            using (HttpWebResponse response = (HttpWebResponse)webResponse)
            {
                using (Stream dataStream = response.GetResponseStream())
                using (StreamReader reader = new StreamReader(dataStream))
                {
                    result = reader.ReadToEnd();
                }
            }

            return result;
        }
    }
}
