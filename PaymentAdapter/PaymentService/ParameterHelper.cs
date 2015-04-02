/**
 *  Accela Citizen Access
 *  File: ParameterHelper.cs
 * 
 *  Accela, Inc.
 *  Copyright (C): 2009-2013
 * 
 *  Description:
 *  
 * 
 *  Notes:
 * $Id: ParameterHelper.cs 130107 2009-07-21 12:23:56Z ACHIEVO\cary.cao $.
 *  Revision History
 *  Date,                  Who,                 What
 */

using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Web;
using Accela.ACA.Payment.Xml;

namespace Accela.ACA.PaymentAdapter
{
    /// <summary>
    /// parameter helper class
    /// </summary>
    public sealed class ParameterHelper
    {
        /// <summary>
        /// get parameter mapping
        /// </summary>
        /// <param name="actionType">the ActionType</param>
        /// <returns>parameter mapping collection</returns>
        public static Hashtable GetParameterMapping(ActionType actionType)
        {
            IList<ParameterNode> parameters = PaymentUtil.ListParameterMapping(actionType);

            Hashtable paramHT = new Hashtable();

            foreach (ParameterNode param in parameters)
            {
                if (!paramHT.ContainsKey(param.MappingName) && !String.IsNullOrEmpty(param.ParamValue))
                {
                    paramHT.Add(param.MappingName, param.ParamValue);
                }
            }

            return paramHT;
        }

        /// <summary>
        /// get request parameters
        /// </summary>
        /// <returns>request string</returns>
        public static string GetReqeustParameters()
        {
            HttpRequest request = HttpContext.Current.Request;
            string[] parameters = { };
            StringBuilder sb = new StringBuilder();

            if (request.Form.AllKeys.Length != 0)
            {
                parameters = request.Form.AllKeys;
            }
            else if (request.QueryString.AllKeys.Length != 0)
            {
                parameters = request.QueryString.AllKeys;
            }

            foreach (string key in parameters)
            {
                if (sb.Length > 0)
                {
                    sb.Append("&");
                }

                string value = String.Empty;

                if (request.Form[key] != null)
                {
                    value = HttpContext.Current.Server.UrlEncode(request.Form[key]);
                }
                else if (request.QueryString[key] != null)
                {
                    value = HttpContext.Current.Server.UrlEncode(request.QueryString[key]);
                }

                sb.Append(string.Format("{0}={1}", key, value));
            }

            return sb.ToString();
        }

        /// <summary>
        /// get parameter by key
        /// </summary>
        /// <param name="key">parameter key</param>
        /// <returns>parameter value</returns>
        public static string GetParameterByKey(string key)
        {
            HttpRequest request = HttpContext.Current.Request;
            string param = String.Empty;

            if (request.QueryString[key] != null)
            {
                // The redirect mode
                param = request.QueryString[key];
            }
            else if (request.Form[key] != null)
            {
                // The post back mode
                param = request.Form[key];
            }

            return param;
        }

        /// <summary>
        /// get parameter by key
        /// </summary>
        /// <param name="parameters">the parameters in hashtable</param>
        /// <param name="key">the parameter key</param>
        /// <returns>the parameter value</returns>
        public static string GetParameterByKey(Hashtable parameters, string key)
        {
            string value = String.Empty;

            foreach (DictionaryEntry item in parameters)
            {
                if (key == item.Key.ToString() && item.Value != null)
                {
                    value = item.Value.ToString();
                    break;
                }
            }

            return value;
        }

        /// <summary>
        /// split response text and save it to hashtable
        /// </summary>
        /// <param name="actionType">ActionType instance</param> 
        /// <param name="responseText">the response text</param>
        /// <param name="splitChar">the split char</param>
        /// <returns>key value pairs</returns>
        public static Hashtable SplitResponseText(ActionType actionType, string responseText, char splitChar)
        {
            Hashtable htKeyValues = new Hashtable();

            if (String.IsNullOrEmpty(responseText))
            {
                return htKeyValues;
            }

            string[] pairs = responseText.Split(splitChar);

            foreach (string param in pairs)
            {
                string[] keyValues = param.Split('=');

                if (keyValues.Length == 2)
                {
                    string key = GetMappingName(actionType, keyValues[0]);
                    string value = keyValues[1];

                    if (!String.IsNullOrEmpty(key) && !htKeyValues.ContainsKey(key))
                    {
                        htKeyValues.Add(key, value);
                    }
                }
            }

            return htKeyValues;
        }

        /// <summary>
        /// Combine response text. 
        /// It is an specific logic for ProcTransactionType that 1 = Credit Card, 2 = Personal Check, 3 = Business Check
        /// </summary>
        /// <param name="parameters">parameters key value pairs</param>
        /// <param name="split">the split code</param>
        /// <returns>the response text</returns>
        public static string CombineResponseText(Hashtable parameters, string split)
        {
            StringBuilder responseText = new StringBuilder();

            foreach (DictionaryEntry item in parameters)
            {
                // convert payment_type
                //1 = credit card; 2 = personal check; 3 = business check
                string val = HttpUtility.UrlEncode(item.Value.ToString());
                if (item.Key.ToString() == "ProcTransactionType")
                {
                    if (val == "1")
                    {
                        val = PaymentConstant.PAY_METHOD_CREDIT_CARD;
                    }
                    else
                    {
                        val = PaymentConstant.PAY_METHOD_CHECK;
                    }
                }

                responseText.AppendFormat("{0}={1}{2}", item.Key, val, split);
            }

            return responseText.ToString();
        }

        /// <summary>
        /// get mapping name by key
        /// </summary>
        /// <param name="actionType">ActionType instance</param>
        /// <param name="key">the parameter key from ACA or thridparty</param>
        /// <returns>the matched mapping name</returns>
        private static string GetMappingName(ActionType actionType, string key)
        {
            IList<ParameterNode> parameters = PaymentUtil.GetParameterMappingByName(actionType);

            string mappingName = String.Empty;

            foreach (ParameterNode param in parameters)
            {
                if (actionType.Equals(ActionType.FromACA) && param.ACA == key)
                {
                    mappingName = param.MappingName;
                    break;
                }
                else if (actionType.Equals(ActionType.FromThirdParty) && param.ThirdParty == key)
                {
                    mappingName = param.MappingName;
                    break;
                }
            }

            return mappingName;
        }
    }
}
