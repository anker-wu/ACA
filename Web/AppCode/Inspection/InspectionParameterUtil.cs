#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: InspectionParameterManager.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2010-2014
 *
 *  Description:
 *
 *  Notes:
 *      $Id: InspectionParameterUtil.cs 277174 2014-08-12 07:38:31Z ACHIEVO\james.shi $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web;

using Accela.ACA.BLL.Common;
using Accela.ACA.BLL.Inspection;
using Accela.ACA.Common;
using Accela.ACA.Common.Util;
using Accela.ACA.Inspection;
using Accela.ACA.Web.Common;
using Accela.ACA.WSProxy;

namespace Accela.ACA.Web.Inspection
{
    /// <summary>
    /// Inspection Parameter Utility
    /// </summary>
    public static class InspectionParameterUtil
    {
        /// <summary>
        /// Updates the URL and save parameters.
        /// </summary>
        /// <param name="url">The URL string.</param>
        /// <param name="inspectionParameter">The inspection parameter.</param>
        /// <returns>the updated URL</returns>
        public static string UpdateURLAndSaveParameters(string url, InspectionParameter inspectionParameter)
        {
            var urlParameters = BuildModelFromURL();
            var parameters = BuildQueryString(inspectionParameter);
            var parameterStoreKey = !string.IsNullOrEmpty(urlParameters.ParameterStoreKey) ? urlParameters.ParameterStoreKey : CommonUtil.GetRandomUniqueID();
            AddOrUpdateParameters(parameterStoreKey, parameters);
            string newUrl = AttachParameterDataToURL(url, parameterStoreKey);

            return newUrl;
        }

        /// <summary>
        /// Gets the parameters.
        /// </summary>
        /// <param name="key">The store key.</param>
        /// <returns>the parameters.</returns>
        public static InspectionParameter GetParameters(string key)
        {
            var parametersStore = AppSession.InspectionParameters;
            var queryString = parametersStore == null || !parametersStore.ContainsKey(key) ? string.Empty : parametersStore[key] as string;
            var result = queryString == null ? null : BuildModelFromQueryString(queryString);
            return result;
        }

        /// <summary>
        /// Updates the parameters.
        /// </summary>
        /// <param name="parameter">The parameter.</param>
        /// <param name="inspectionViewModel">The inspection view model.</param>
        public static void UpdateParameters(InspectionParameter parameter, InspectionViewModel inspectionViewModel)
        {
            if (inspectionViewModel.ID > 0)
            {
                parameter.Group = inspectionViewModel.InspectionDataModel.Group;
                parameter.InAdvance = inspectionViewModel.InspectionDataModel.InAdvance;
                parameter.Required = inspectionViewModel.InspectionDataModel.Required;
                parameter.ReadyTimeEnabled = inspectionViewModel.InspectionDataModel.ReadyTimeEnabled;
                parameter.ResultGroup = inspectionViewModel.InspectionDataModel.ResultGroup;
                parameter.ScheduledDateTime = inspectionViewModel.InspectionDataModel.ScheduledDateTime;
                parameter.ScheduleType = inspectionViewModel.InspectionDataModel.ScheduleType;
                parameter.Type = inspectionViewModel.InspectionDataModel.Type;
                parameter.TypeID = inspectionViewModel.TypeID.ToString();
                parameter.TypeText = inspectionViewModel.TypeText;
                parameter.Units = inspectionViewModel.InspectionDataModel.Units;
                parameter.RescheduleRestrictionSettings = inspectionViewModel.InspectionDataModel.RestrictionSettings4Reschedule;
                parameter.CancelRestrictionSettings = inspectionViewModel.InspectionDataModel.RestrictionSettings4Cancel;
                parameter.ContactFirstName = inspectionViewModel.ContactFirstName;
                parameter.ContactMiddleName = inspectionViewModel.ContactMiddleName;
                parameter.ContactLastName = inspectionViewModel.ContactLastName;
                parameter.ContactPhoneNumber = inspectionViewModel.ContactPhoneNumber;
                parameter.ContactPhoneIDD = inspectionViewModel.ContactPhoneIDD;
                parameter.ContactFullPhoneNumber = inspectionViewModel.ContactFullPhoneNumber;
                parameter.Status = inspectionViewModel.InspectionDataModel.Status;
                parameter.TimeOption = inspectionViewModel.TimeOption;
            }
        }

        /// <summary>
        /// Builds the query string.
        /// </summary>
        /// <param name="inspectionParameter">The inspection parameter.</param>
        /// <returns>the query string.</returns>
        public static string BuildQueryString(InspectionParameter inspectionParameter)
        {
            string result = string.Empty;
            NameValueCollection parameterCollection = HttpUtility.ParseQueryString(string.Empty);

            Type type = inspectionParameter.GetType();
            PropertyInfo[] propertyInfos = type.GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly);

            if (propertyInfos != null)
            {
                foreach (PropertyInfo propertyInfo in propertyInfos)
                {
                    Attribute attribute = Attribute.GetCustomAttribute(propertyInfo, typeof(URLParameterAttribute), false);
                    URLParameterAttribute urlParameter = attribute as URLParameterAttribute;
                    object tempValue = urlParameter != null ? propertyInfo.GetValue(inspectionParameter, null) : null;

                    if (tempValue != null && tempValue is DateTime)
                    {
                        var tempDateTimeString = I18nDateTimeUtil.FormatToDateTimeStringForWebService((DateTime)tempValue);
                        parameterCollection.Add(urlParameter.Key, tempDateTimeString);
                    }
                    else if (tempValue != null && !string.IsNullOrEmpty(tempValue.ToString()))
                    {
                        parameterCollection.Add(urlParameter.Key, tempValue.ToString());
                    }
                }
            }

            result = parameterCollection.ToString();

            return result;
        }

        /// <summary>
        /// Builds the parameter model from URL.
        /// </summary>
        /// <returns>the parameter model from URL.</returns>
        public static InspectionParameter BuildModelFromURL()
        {
            InspectionParameter result = new InspectionParameter();
            SetValuesFromURL(HttpContext.Current.Request.QueryString, result);

            return result;
        }

        /// <summary>
        /// Builds the parameter model from URL.
        /// </summary>
        /// <param name="nameValueCollection">The name value collection.</param>
        /// <returns>the parameter model from query string NameValueCollection.</returns>
        public static InspectionParameter BuildModelFromQueryString(NameValueCollection nameValueCollection)
        {
            InspectionParameter result = new InspectionParameter();
            SetValuesFromURL(nameValueCollection, result);

            return result;
        }

        /// <summary>
        /// Builds the model from query string.
        /// </summary>
        /// <param name="queryString">The query string.</param>
        /// <returns>the parameter model from query string.</returns>
        public static InspectionParameter BuildModelFromQueryString(string queryString)
        {
            var result = new InspectionParameter();
            var nameValueCollection = HttpUtility.ParseQueryString(queryString);
            SetValuesFromURL(nameValueCollection, result);

            return result;
        }

        /// <summary>
        /// Attaches the parameter data to URL.
        /// </summary>
        /// <param name="url">The URL without parameter data.</param>
        /// <param name="parameterStoreKey">The parameter store key.</param>
        /// <returns>
        /// The URL with parameter data
        /// </returns>
        private static string AttachParameterDataToURL(string url, string parameterStoreKey)
        {
            int quoteIndex = string.IsNullOrEmpty(url) ? -1 : url.IndexOf("?");
            var urlPath = quoteIndex == -1 ? url : url.Substring(0, quoteIndex);
            var queryString = quoteIndex == -1 ? string.Empty : url.Substring(quoteIndex + 1);
            var parameters = BuildModelFromQueryString(queryString);
            parameters.ParameterStoreKey = parameterStoreKey;
            var newQueryString = BuildQueryString(parameters);
            var newUrl = string.Format("{0}?{1}", urlPath, newQueryString);
            return newUrl;
        }

        /// <summary>
        /// Adds the or update parameters.
        /// </summary>
        /// <param name="key">The parameter key.</param>
        /// <param name="value">The parameter value.</param>
        private static void AddOrUpdateParameters(string key, string value)
        {
            var parametersStore = AppSession.InspectionParameters;

            if (parametersStore == null)
            {
                parametersStore = new Hashtable();
            }
            else
            {
                parametersStore.Clear();
            }

            parametersStore[key] = value;
            AppSession.InspectionParameters = parametersStore;
        }

        /// <summary>
        /// Sets the values from URL.
        /// </summary>
        /// <param name="nameValueCollection">The name value collection.</param>
        /// <param name="result">The values from URL.</param>
        private static void SetValuesFromURL(NameValueCollection nameValueCollection, InspectionParameter result)
        {
            Type type = result.GetType();
            PropertyInfo[] propertyInfos = type.GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly);

            if (propertyInfos != null)
            {
                foreach (PropertyInfo propertyInfo in propertyInfos)
                {
                    Attribute attribute = Attribute.GetCustomAttribute(propertyInfo, typeof(URLParameterAttribute), false);
                    URLParameterAttribute urlParameter = attribute as URLParameterAttribute;

                    if (urlParameter != null)
                    {
                        string value = nameValueCollection[urlParameter.Key];
                        SetValue(result, propertyInfo, value);
                    }
                }
            }
        }

        /// <summary>
        /// Sets the value.
        /// </summary>
        /// <param name="result">The result.</param>
        /// <param name="propertyInfo">The property info.</param>
        /// <param name="value">The value.</param>
        private static void SetValue(InspectionParameter result, PropertyInfo propertyInfo, string value)
        {
            if (propertyInfo != null && !string.IsNullOrEmpty(value))
            {
                if (propertyInfo.PropertyType.BaseType == typeof(Enum))
                {
                    var parseValue = Enum.Parse(propertyInfo.PropertyType, value, true);
                    propertyInfo.SetValue(result, parseValue, null);
                }
                else if (propertyInfo.PropertyType == typeof(string))
                {
                    propertyInfo.SetValue(result, value, null);
                }
                else if (propertyInfo.PropertyType.IsGenericType && propertyInfo.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>))
                {
                    Type underlingType = propertyInfo.PropertyType.GetGenericArguments()[0];

                    if (underlingType == typeof(DateTime))
                    {
                        SetDateTimeValue(result, propertyInfo, value);
                    }
                    else if (underlingType == typeof(bool))
                    {
                        SetBoolValue(result, propertyInfo, value);
                    }
                    else
                    {
                        propertyInfo.SetValue(result, Convert.ChangeType(value, underlingType), null);
                    }
                }
                else if (propertyInfo.PropertyType == typeof(bool))
                {
                    SetBoolValue(result, propertyInfo, value);
                }
                else if (propertyInfo.PropertyType == typeof(DateTime))
                {
                    SetDateTimeValue(result, propertyInfo, value);
                }
                else if (propertyInfo.PropertyType.IsValueType)
                {
                    propertyInfo.SetValue(result, Convert.ChangeType(value, propertyInfo.PropertyType), null);
                }
            }
        }

        /// <summary>
        /// Sets the date time value.
        /// </summary>
        /// <param name="result">The result.</param>
        /// <param name="propertyInfo">The property info.</param>
        /// <param name="value">The datetime value string.</param>
        private static void SetDateTimeValue(InspectionParameter result, PropertyInfo propertyInfo, string value)
        {
            DateTime tempDateTimeValue = DateTime.Now;
            bool tempParsedOK = I18nDateTimeUtil.TryParseFromWebService(value, out tempDateTimeValue);

            if (tempParsedOK)
            {
                propertyInfo.SetValue(result, tempDateTimeValue, null);
            }
        }

        /// <summary>
        /// Sets the boolean value.
        /// </summary>
        /// <param name="result">The result.</param>
        /// <param name="propertyInfo">The property info.</param>
        /// <param name="value">The boolean value string.</param>
        private static void SetBoolValue(InspectionParameter result, PropertyInfo propertyInfo, string value)
        {
            if (ValidationUtil.IsYes(value))
            {
                propertyInfo.SetValue(result, true, null);
            }
            else if (ValidationUtil.IsNo(value))
            {
                propertyInfo.SetValue(result, false, null);
            }
            else if (ACAConstant.COMMON_TRUE.Equals(value, StringComparison.OrdinalIgnoreCase) || ACAConstant.COMMON_FALSE.Equals(value, StringComparison.OrdinalIgnoreCase))
            {
                propertyInfo.SetValue(result, Convert.ChangeType(value, typeof(bool)), null);
            }
        }
    }
}
