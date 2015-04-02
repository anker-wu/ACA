/**
 *  Accela Citizen Access
 *  File: PaymentUtil.cs
 * 
 *  Accela, Inc.
 *  Copyright (C): 2009-2013
 * 
 *  Description:
 *  
 * 
 *  Notes:
 * $Id: PaymentUtil.cs 130107 2009-07-21 12:23:56Z ACHIEVO\cary.cao $.
 *  Revision History
 *  Date,                  Who,                 What
 */

using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.IO;
using System.Web;
using System.Xml;
using System.Xml.Serialization;
using Accela.ACA.Payment.Xml;
using System.Text;

namespace Accela.ACA.PaymentAdapter
{
    /// <summary>
    ///  payment utility
    /// </summary>
    public sealed class PaymentUtil
    {
        /// <summary>
        /// Gets the adapter parameter mapping file path
        /// </summary>
        private static string MappingFilePath
        {
            get
            {
                string adapterName = GetConfig("AdapterName");
                string filePath = Path.Combine(HttpContext.Current.Server.MapPath("~"), adapterName + "\\ParameterMapping.xml");
                return filePath;
            }
        }

        /// <summary>
        /// get post back data from third party payment provider.
        /// </summary>
        /// <returns></returns>
        public static string GetPostbackDataString()
        {
            StringBuilder sb = new StringBuilder();

            sb.Append("\r\nRequest.Form:\r\n");
            foreach (string key in HttpContext.Current.Request.Form.AllKeys)
            {
                sb.Append(string.Format("{0}={1}\n", key, HttpContext.Current.Request.Form[key]));
            }

            sb.Append("\r\nRequest.QueryString:\r\n");
            foreach (string key in HttpContext.Current.Request.QueryString.AllKeys)
            {
                sb.Append(string.Format("{0}={1}\n", key, HttpContext.Current.Request.QueryString[key]));
            }

            return sb.ToString();
        }

        /// <summary>
        /// Convert input data to string, if the object is null, return String.Empty
        /// </summary>
        /// <param name="data">the source data</param>
        /// <returns>the string value</returns>
        public static string ParseObjectToString(object data)
        {
            if (data == null)
            {
                return String.Empty;
            }

            return data.ToString();
        }

        /// <summary>
        /// Get custom session configuration by key
        /// </summary>
        /// <param name="key">the session key</param>
        /// <returns>the session value</returns>
        public static string GetConfig(string key)
        {
            NameValueCollection parameters = ConfigurationManager.GetSection(@"paymentAdapter") as NameValueCollection;
            return parameters[key];
        }

        /// <summary>
        /// Get parameters list
        /// </summary>
        /// <param name="actionType">the action name</param>
        /// <returns>the parameter nodes</returns>
        internal static IList<ParameterNode> GetParameterMappingByName(ActionType actionType)
        {
            ParamMapping mapping = LoadParameterMapping(MappingFilePath);

            IList<ParameterNode> parameters = new List<ParameterNode>();

            if (mapping == null || mapping.Adapter == null || mapping.Adapter.Parameters.Count == 0)
            {
                return parameters;
            }
            else
            {
                return mapping.Adapter.Parameters;
            }
        }

        /// <summary>
        /// Get parameter list from mappings depends on the ActionType
        /// </summary>
        /// <param name="actionType">the action type</param>
        /// <returns>parameters in the appointed action</returns>
        internal static IList<ParameterNode> ListParameterMapping(ActionType actionType)
        {
            IList<ParameterNode> parameters = GetParameterMappingByName(actionType);

            // matches the parameters
            foreach (ParameterNode param in parameters)
            {
                string key = String.Empty;

                if (actionType.Equals(ActionType.FromACA))
                {
                    key = param.ACA;
                }
                else
                {
                    key = param.ThirdParty;
                }

                param.ParamValue = ParameterHelper.GetParameterByKey(key);
            }

            return parameters;
        }

        /// <summary>
        /// Get parameter mapping
        /// </summary>
        /// <param name="fullPath">the mapping file path</param>
        /// <returns>the instance of ParamMapping</returns>
        private static ParamMapping LoadParameterMapping(string fullPath)
        {
            if (!File.Exists(fullPath))
            {
                throw new Exception("Can not find mapping file." + fullPath);
            }

            if (PaymentHelper.GetDataFromCache<ParamMapping>("ParamMapping") == null)
            {
                using (XmlTextReader reader = new XmlTextReader(new StreamReader(fullPath)))
                {
                    XmlSerializer xml = new XmlSerializer(typeof(ParamMapping));
                    ParamMapping parameter = (ParamMapping)xml.Deserialize(reader);

                    PaymentHelper.SetDataToCache<ParamMapping>("ParamMapping", parameter, 300);
                }
            }

            return PaymentHelper.GetDataFromCache<ParamMapping>("ParamMapping");
        }

        /// <summary>
        /// get domain url
        /// </summary>
        /// <param name="pagePathName">the target page</param>
        /// <returns>the whole url</returns>
        public static string GetDomainUrl(string pagePathName)
        {
            string host = HttpContext.Current.Request.Url.Host;
            string port = HttpContext.Current.Request.Url.Port.ToString();
            string http = HttpContext.Current.Request.Url.Scheme;
            string appPath = HttpContext.Current.Request.ApplicationPath.TrimEnd('/');
            pagePathName = pagePathName.TrimStart('/');

            string url = String.Empty;

            // if port is set to default value, needn't show this port
            if (port == "80")
            {
                url = String.Format("{0}://{1}{2}/{3}", http, host, appPath, pagePathName);
            }
            else
            {
                url = String.Format("{0}://{1}:{2}{3}/{4}", http, host, port, appPath, pagePathName);
            }

            return url;
        }
    }
}
