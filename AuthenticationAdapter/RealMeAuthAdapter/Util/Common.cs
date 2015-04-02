#region Header

/**
* <pre>
*  Accela Citizen Access
*  File: Common.cs
*
*  Accela, Inc.
*  Copyright (C): 2014
*
*  Description: The common functions for RealMe authentication adapter.
*
* </pre>
*/

#endregion

using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using Accela.ACA.SSOInterface;

namespace Accela.ACA.RealMeAccessGate
{
    /// <summary>
    /// The common functions for RealMe authentication adapter.
    /// </summary>
    public static class Common
    {
        /// <summary>
        /// Get values from parameter
        /// </summary>
        /// <param name="parameter">The parameters</param>
        /// <returns>Name value collection</returns>
        public static NameValueCollection CollecteValues(string parameter)
        {
            var parameters = parameter.Split('&');
            var parameterCollection = new NameValueCollection();

            foreach (string param in parameters)
            {
                var keyValue = GetParamKeyAndValue(param);
                parameterCollection.Add(keyValue.Key, keyValue.Value);
            }

            return parameterCollection;
        }

        /// <summary>
        ///  Get key and value from parameter.
        /// </summary>
        /// <param name="parameter">The parameter for extract.</param>
        /// <returns>Key and value</returns>
        public static KeyValuePair<string, string> GetParamKeyAndValue(string parameter)
        {
            if (string.IsNullOrEmpty(parameter))
            {
                return new KeyValuePair<string, string>(string.Empty, string.Empty);
            }

            Regex keyRegex = new Regex(@"[?&]?[^=]*");
            Match keyMatch = keyRegex.Match(parameter);
            string key = keyMatch.Value.TrimStart('&', '?');
            Regex valueRegex = new Regex(@"={1}[\S]*");
            Match valueMatch = valueRegex.Match(parameter, keyMatch.Value.Length);
            string value = valueMatch.Value.TrimStart('=');
            return new KeyValuePair<string, string>(key, value);
        }

        /// <summary>
        /// Save data to cookie
        /// </summary>
        /// <param name="data">Data for save</param>
        /// <param name="keyName">Cookie name</param>
        /// <param name="expireMinutes">Expire minutes</param>
        public static void SaveDataToCookie(string data, string keyName, int expireMinutes = 1440)
        {
            HttpRequest request = HttpContext.Current.Request;
            var cookie = request.Cookies[keyName];
            bool isSecureConnection = ACAContext.Instance.Protocol.Equals("https", StringComparison.OrdinalIgnoreCase);

            if (cookie == null)
            {
                cookie = new HttpCookie(keyName, data);
                cookie.Secure = isSecureConnection;
                cookie.Expires = DateTime.Now.AddMinutes(expireMinutes);
                cookie.HttpOnly = true;
            }
            else
            {
                cookie.Expires = !string.IsNullOrEmpty(data) ? DateTime.Now.AddMinutes(expireMinutes) : DateTime.Now.AddDays(-1);
                cookie.Secure = isSecureConnection;
                cookie.HttpOnly = true;
                cookie.Value = data;
            }

            HttpContext.Current.Response.AppendCookie(cookie);
        }

        /// <summary>
        /// Get data from cookie by key name
        /// </summary>
        /// <param name="keyName">Cookie name</param>
        /// <returns>Return cookie data</returns>
        public static string GetDataFromCookie(string keyName)
        {
            HttpRequest request = HttpContext.Current.Request;
            var cookie = request.Cookies[keyName];

            if (cookie == null)
            {
                return string.Empty;
            }

            return cookie.Value;
        }

        /// <summary>
        /// Judge the data whether contains another data
        /// </summary>
        /// <param name="data">The data for judge</param>
        /// <param name="anotherData">Another data</param>
        /// <returns>If is contains return true, else return false</returns>
        public static bool IsContainsArrary(IEnumerable<string> data, IEnumerable<string> anotherData)
        {
            if (data == null)
            {
                return anotherData == null;
            }

            int comreDataCount = anotherData.Count();
            if (data.Count() < comreDataCount)
            {
                return false;
            }

            return data.Intersect(anotherData).Count() == comreDataCount;
        }
        
        /// <summary>
        /// Indicates the text whether is Y (ignore case sensitive).
        /// True-if the value is 'y'; otherwise false.
        /// </summary>
        /// <param name="text">the value to be judged.</param>
        /// <returns>true-the value is regarded Yes, false-indicates the values is not Yes.</returns>
        public static bool IsYes(string text)
        {
            if (string.IsNullOrEmpty(text))
            {
                return false;
            }

            if (text.Equals(Constant.COMMON_Y, StringComparison.InvariantCultureIgnoreCase))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}