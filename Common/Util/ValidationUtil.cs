#region Header

/**
 *  Accela Citizen Access
 *  File: ValidationUtil.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2008-2014
 *
 *  Description:
 *   It provides the validation related utility to serve the framework.
 *
 *  Notes:
 * $Id: ValidationUtil.cs 276545 2014-08-04 02:15:30Z ACHIEVO\james.shi $.
 *  Revision History
 *  Date,                  Who,                 What
 */

#endregion Header

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Security;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using System.Text.RegularExpressions;
using Accela.ACA.Common.Log;
using Accela.ACA.WSProxy;

namespace Accela.ACA.Common.Util
{
    /// <summary>
    ///  provides the validation related utility to serve the framework. 
    /// </summary>
    public static class ValidationUtil
    {
        #region Fields

        /// <summary>
        /// Final score id for GView.
        /// </summary>
        private const string FINAL_SCORE_ID = "txtFinalScore_txtFinalScore";

        /// <summary>
        /// logger object.
        /// </summary>
        private static readonly log4net.ILog Logger = LogFactory.Instance.GetLogger(typeof(ValidationUtil));

        /// <summary>
        /// the regex for integer.
        /// </summary>
        private static Regex _regexInt = null;

        /// <summary>
        /// the regex for number.
        /// </summary>
        private static Regex _regexNumber = null;

        #endregion Fields

        #region Methods

        /// <summary>
        /// Check the string whether is an integer. 
        /// the "+/-" is allowed.
        /// </summary>
        /// <param name="text">the string to be checked.</param>
        /// <returns>return true if it is integer, return false if not.</returns>
        public static bool IsInt(string text)
        {
            if (string.IsNullOrEmpty(text))
            {
                return false;
            }

            if (_regexInt == null)
            {
                _regexInt = new Regex(@"^((\+|-)\d)?\d*$", RegexOptions.CultureInvariant | RegexOptions.Compiled);
            }

            return _regexInt.IsMatch(text);
        }

        /// <summary>
        /// Indicates the text whether is No or N (ignore case sensitive).
        /// True-if the value is any one of 'No','N','no','n'.
        /// </summary>
        /// <param name="text">the value to be judged.</param>
        /// <returns>true-the value is regarded No, false-indicates the values is not No.</returns>
        public static bool IsNo(string text)
        {
            if (string.IsNullOrEmpty(text))
            {
                return false;
            }

            if (text.Equals(ACAConstant.COMMON_NO, StringComparison.InvariantCultureIgnoreCase) || text.Equals(ACAConstant.COMMON_N, StringComparison.InvariantCultureIgnoreCase))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Check whether the string is invariant-culture number.
        /// digits is allowed to 4.
        /// </summary>
        /// <param name="text">the string to be checked.</param>
        /// <returns>return true if it is number, return false if not.</returns>
        public static bool IsNumber(string text)
        {
            if (string.IsNullOrEmpty(text))
            {
                return false;
            }

            if (_regexNumber == null)
            {
                string pattern = "^(-?[0-9]*[.]*[0-9]{0,4})$";
                _regexNumber = new Regex(pattern, RegexOptions.CultureInvariant | RegexOptions.Compiled);
            }

            return _regexNumber.IsMatch(text);
        }

        /// <summary>
        /// Check whether URL is valid.
        /// </summary>
        /// <param name="url">URL string</param>
        /// <returns>return true if valid.</returns>
        public static bool IsValidUrl(string url)
        {
            System.Uri u = null;
            try
            {
                u = new Uri(url);
            }
            catch
            {
                return false;
            }

            bool isExist = false;
            System.Net.HttpWebRequest r = System.Net.HttpWebRequest.Create(u) as System.Net.HttpWebRequest;
            r.Method = "HEAD";
            try
            {
                if (url.StartsWith("https", StringComparison.InvariantCultureIgnoreCase))
                {
                    ServicePointManager.ServerCertificateValidationCallback = new System.Net.Security.RemoteCertificateValidationCallback(CheckValidationResult);
                }

                System.Net.HttpWebResponse s = r.GetResponse() as System.Net.HttpWebResponse;

                if (s.StatusCode == System.Net.HttpStatusCode.OK && string.Equals(s.ResponseUri.ToString(), url, StringComparison.InvariantCultureIgnoreCase))
                {
                    isExist = true;
                }
            }
            catch (System.Net.WebException x)
            {
                Logger.ErrorFormat("Error occurred, error message:{0}", x);
            }

            return isExist;
        }

        /// <summary>
        /// Indicates the text whether is Yes or Y (ignore case sensitive).
        /// True-if the value is any one of 'Yes','Y','yes','y'; otherwise false, such as null or string.Empty
        /// </summary>
        /// <param name="text">the value to be judged.</param>
        /// <returns>true-the value is regarded Yes, false-indicates the values is not Yes.</returns>
        public static bool IsYes(string text)
        {
            if (string.IsNullOrEmpty(text))
            {
                return false;
            }

            if (text.Equals(ACAConstant.COMMON_YES, StringComparison.InvariantCultureIgnoreCase) || text.Equals(ACAConstant.COMMON_Y, StringComparison.InvariantCultureIgnoreCase))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Indicates the text whether is H
        /// </summary>
        /// <param name="text">the value to be judged.</param>
        /// <returns>true if the value is H, default is false</returns>
        public static bool IsHidden(string text)
        {
            if (string.IsNullOrEmpty(text))
            {
                return false;
            }

            return text.Equals(ACAConstant.COMMON_H, StringComparison.InvariantCultureIgnoreCase);
        }

        /// <summary>
        /// Indicates the text whether is True.
        /// </summary>
        /// <param name="text">the value to be judged.</param>
        /// <returns>true if the value is True/true..., default is false</returns>
        public static bool IsTrue(string text)
        {
            return ACAConstant.COMMON_TRUE.Equals(text, StringComparison.InvariantCultureIgnoreCase);
        }

        /// <summary>
        /// Indicates the text whether is False.
        /// </summary>
        /// <param name="text">the value to be judged.</param>
        /// <returns>true if the value is False/false..., default is false</returns>
        public static bool IsFalse(string text)
        {
            return ACAConstant.COMMON_FALSE.Equals(text, StringComparison.InvariantCultureIgnoreCase);
        }

        /// <summary>
        /// Validate whether all required field has had the value. Only validate the string type so far(need to enhance it later).
        /// </summary>
        /// <param name="model">the object with all required field value.</param>
        /// <param name="simpleViewElementModels">simple view element models.</param>
        /// <param name="useZip">if set to <c>true</c> current regional setting use the Zip field,
        /// <c>false</c> means not use the Zip field and will not execute the require validation for Zip field.</param>
        /// <returns>
        /// true - all required field have the values.
        /// false - at least existing a required field have the empty/null value.
        /// </returns>
        public static bool ValidateRequiredValue(object model, SimpleViewElementModel4WS[] simpleViewElementModels, bool useZip)
        {
            string exceptRequiredField = string.Empty;     
            
            object gradingStyleValue = GetPropertyValue(model, "gradingStyle");

            if (gradingStyleValue != null && gradingStyleValue.ToString() == "none")
            {
                exceptRequiredField = FINAL_SCORE_ID;
            }

            string[] propertyNames = GetPropertyNames(simpleViewElementModels, exceptRequiredField, useZip);
            
            // if there is no specified required property,means don't need to validate 
            if (propertyNames == null || propertyNames.Length == 0)
            {
                return true;
            }

            // if there is specified required property, but the model is null. it means validation is failed.
            if (model == null)
            {
                return false;
            }

            bool validated = true;

            // store the object name and corresponding field names.
            Hashtable htModels = new Hashtable();

            ArrayList modelNames = GetAllModelNames(propertyNames, htModels);

            // store the concrete object instance
            Hashtable htObjects = new Hashtable();

            foreach (string modelName in modelNames)
            {
                int lastSplitChar = modelName.LastIndexOf(ACAConstant.DELIMITER_STAR);
                string parentModelName = lastSplitChar == -1 ? modelName : modelName.Substring(0, lastSplitChar);

                if (!htObjects.Contains(parentModelName))
                {
                    htObjects.Add(modelName, model);
                }
                else
                {
                    object parentModel = htObjects[parentModelName];
                    htObjects.Add(modelName, GetPropertyValue(parentModel, modelName.Substring(lastSplitChar + 1)));
                }
            }

            IDictionaryEnumerator enumerator = htModels.GetEnumerator();

            // loop each object to validate each field whether there is empty field.
            while (enumerator.MoveNext())
            {
                string key = enumerator.Key.ToString();

                string requiredFields = Convert.ToString(htModels[key]);

                if (string.IsNullOrEmpty(requiredFields))
                {
                    continue;
                }

                bool hasEmptyField = ExistEmpty(htObjects[key], requiredFields.Split(','));

                if (hasEmptyField)
                {
                    validated = false;
                    break;
                }
            }

            return validated;
        }
        
        /// <summary>
        /// Is all standard fields empty.
        /// Use the <c>SimpleViewElementModel4WS.viewElementDesc</c> property to reflect the all properties of the specific model to determine if all properties are empty.
        /// </summary>
        /// <param name="model">the object with all value.</param>
        /// <param name="simpleViewElementModels">simple view element models.</param>
        /// <param name="exceptRequiredFields">The excepted required fields.</param>
        /// <returns>
        /// true - all standard fields is empty.
        /// false - at least existing a required field have the empty/null value.
        /// </returns>
        public static bool IsAllStandardFieldsEmpty(object model, SimpleViewElementModel4WS[] simpleViewElementModels, string[] exceptRequiredFields)
        {
            bool isAllStandardFieldsEmpty = true;

            if (model == null || simpleViewElementModels == null || simpleViewElementModels.Length == 0)
            {
                return false;
            }

            string[] propertyNames = simpleViewElementModels.Where(s => ACAConstant.VALID_STATUS.Equals(s.recStatus, StringComparison.InvariantCultureIgnoreCase) &&
                    ACAConstant.COMMON_Y.Equals(s.standard, StringComparison.InvariantCultureIgnoreCase) &&
                    !ControlType.Line.ToString().Equals(s.elementType, StringComparison.InvariantCultureIgnoreCase) &&
                    !exceptRequiredFields.Contains(s.viewElementDesc)).Select(n => n.viewElementDesc).ToArray();

            if (propertyNames == null || propertyNames.Length == 0)
            {
                return false;
            }

            // store the object name and corresponding field names.
            Hashtable htModels = new Hashtable();

            ArrayList modelNames = GetAllModelNames(propertyNames, htModels);

            // store the concrete object instance
            Hashtable htObjects = new Hashtable();

            foreach (string modelName in modelNames)
            {
                int lastSplitChar = modelName.LastIndexOf(ACAConstant.DELIMITER_STAR);
                string parentModelName = lastSplitChar == -1 ? modelName : modelName.Substring(0, lastSplitChar);

                if (!htObjects.Contains(parentModelName))
                {
                    htObjects.Add(modelName, model);
                }
                else
                {
                    object parentModel = htObjects[parentModelName];
                    htObjects.Add(modelName, GetPropertyValue(parentModel, modelName.Substring(lastSplitChar + 1)));
                }
            }

            IDictionaryEnumerator enumerator = htModels.GetEnumerator();

            // loop each object to validate each field whether there is empty field.
            while (enumerator.MoveNext())
            {
                string key = enumerator.Key.ToString();

                string fields = Convert.ToString(htModels[key]);

                if (string.IsNullOrEmpty(fields))
                {
                    continue;
                }

                bool isAllEmpty = IsAllEmpty(htObjects[key], fields.Split(','));

                if (!isAllEmpty)
                {
                    isAllStandardFieldsEmpty = false;
                    break;
                }
            }

            return isAllStandardFieldsEmpty;
        }

        /// <summary>
        /// Indicates the text whether is r or R (ignore case sensitive).
        /// True-if the value is any one of 'R','r'.
        /// </summary>
        /// <param name="text">the value to be judged.</param>
        /// <returns>true-the value is regarded 'r' or 'R', false-indicates the values is not 'R' or 'r'.</returns>
        public static bool IsReadOnly(string text)
        {
            bool isReadOnly = false;

            if (!string.IsNullOrEmpty(text))
            {
                if (text.Equals(ACAConstant.COMMON_READONLY, StringComparison.InvariantCultureIgnoreCase))
                {
                    isReadOnly = true;
                }
            }

            return isReadOnly;
        }

        /// <summary>
        /// Gets the index of the duplicate unit
        /// </summary>
        /// <param name="objList">object List</param>
        /// <param name="ignoreCase">ignore Case</param>
        /// <returns>the index of the duplicate unit</returns>
        public static int DuplicateIndexOf(IEnumerable<string> objList, bool ignoreCase = false)
        {
            if (objList == null || objList.Count() < 2)
            {
                return -1;
            }

            StringComparison comparison = ignoreCase ? StringComparison.InvariantCultureIgnoreCase : StringComparison.InvariantCulture;

            for (int i = 0; i < objList.Count(); i++)
            {
                for (int j = i + 1; j < objList.Count(); j++)
                {
                    if (string.Equals(objList.ElementAt(i), objList.ElementAt(j), comparison))
                    {
                        return j;
                    }
                }
            }

            return -1;
        }

        /// <summary>
        /// Check whether all fields is empty.
        /// </summary>
        /// <param name="model">the object with specified properties.</param>
        /// <param name="propertyNames">the property names for checking whether the corresponding property values is null/empty. </param>
        /// <returns>true or false.</returns>
        public static bool IsAllEmpty(object model, string[] propertyNames)
        {
            bool isAllEmpty = true;

            // there is no field need to be checked. regard it as non-existing empty value  
            if (propertyNames == null || propertyNames.Length == 0 || model == null)
            {
                return isAllEmpty;
            }

            PropertyInfo[] properties = model.GetType().GetProperties();

            // loop all property value to see whether there exists the empty value
            foreach (string property in propertyNames)
            {
                // get the property value by specified property name
                object propertyValue = GetPropertyValue(model, property, properties);

                if (propertyValue == null)
                {
                    continue;
                }

                if (propertyValue is string)
                {
                    if (string.IsNullOrWhiteSpace(propertyValue as string))
                    {
                        continue;
                    }
                    else
                    {
                        isAllEmpty = false;
                        break;
                    }
                }
                else
                {
                    Type type = propertyValue.GetType();
                    object defaultValue = null;

                    if (type.IsValueType)
                    {
                        defaultValue = Activator.CreateInstance(type);
                    }

                    if (propertyValue != defaultValue)
                    {
                        isAllEmpty = false;
                        break;
                    }
                }
            }

            return isAllEmpty;
        }

        /// <summary>
        /// Get all model name(not-leaf nodes) according to specified property names.
        /// e.g A*B*C,A*B*D*N, the model name will be A,A*B,A*B*D
        /// </summary>
        /// <param name="propertyNames">the specified property names.</param>
        /// <param name="htModels">Model name with corresponding property names.</param>
        /// <returns>model names.</returns>
        private static ArrayList GetAllModelNames(string[] propertyNames, Hashtable htModels)
        {
            // remove blank space for the property names.
            string[] fieldNames = new string[propertyNames.Length];

            for (int i = 0; i < propertyNames.Length; i++)
            {
                fieldNames[i] = propertyNames[i].Replace(" ", string.Empty);
            }

            // store the object name for sorting
            ArrayList modelNames = new ArrayList();

            // loop all field name to get all object name (all not-leaf nodes)
            // e.g A*B*C,A*B*D*N, the object name will be A,A*B,A*B*D
            foreach (string filedName in fieldNames)
            {
                string[] nodes = filedName.Split(ACAConstant.DELIMITER_STAR.ToCharArray());

                string tempNode = null;

                // get all not-leaf nodes, then store them to modelNames
                // e.g A*B*C,A*B*D*N, the object name will be A,A*B,A*B*D
                for (int i = 0; i < nodes.Length - 1; i++)
                {
                    if (!string.IsNullOrEmpty(tempNode))
                    {
                        tempNode += ACAConstant.DELIMITER_STAR;
                    }

                    tempNode += nodes[i];

                    if (!modelNames.Contains(tempNode))
                    {
                        modelNames.Add(tempNode);
                        htModels.Add(tempNode, string.Empty);
                    }
                }

                int lastIndexOfStar = filedName.LastIndexOf(ACAConstant.DELIMITER_STAR, StringComparison.InvariantCultureIgnoreCase);
                string modelName = string.Empty;
                string propertyName = string.Empty;

                if (lastIndexOfStar > -1)
                {
                    modelName = filedName.Substring(0, lastIndexOfStar);
                    propertyName = filedName.Substring(lastIndexOfStar + 1);
                }

                // get all property names of current model that need to be regarded as required field
                string names = htModels[modelName] as string;

                // the delimiter among property name
                if (!string.IsNullOrEmpty(names))
                {
                    names += ",";
                }

                htModels[modelName] = names + propertyName;
            }

            modelNames.Sort();
            return modelNames;
        }

        /// <summary>
        /// Check whether there is any empty/null value in specified property.
        /// </summary>
        /// <param name="model">the object with specified properties.</param>
        /// <param name="propertyNames">the property names for checking whether the corresponding property values is null/empty. </param>
        /// <returns>
        /// true - there is empty/null value in specified property.
        /// false- All specified property doesn't exist empty/null value.
        /// </returns>
        private static bool ExistEmpty(object model, string[] propertyNames)
        {
            // there is no field need to be checked. regard it as non-existing empty value  
            if (propertyNames == null || propertyNames.Length == 0)
            {
                return false;
            }

            // if object is null and they have fields need to be checked
            if (model == null)
            {
                return true;
            }

            bool isEmpty = false;
            PropertyInfo[] properties = model.GetType().GetProperties();

            // loop all property value to see whether there exists the empty value
            foreach (string property in propertyNames)
            {
                if (model is PeopleModel4WS)
                {
                    PeopleModel4WS peopleModel4WS = (PeopleModel4WS)model;
                    if (IgnoreContactReqFieldValidation(peopleModel4WS.contactTypeFlag, property))
                    {
                        continue;
                    }
                }

                // get the property value by specified property name
                object properyValue = GetPropertyValue(model, property, properties);

                if (string.IsNullOrEmpty(Convert.ToString(properyValue).Trim()))
                {
                    isEmpty = true;
                    break;
                }
            }

            return isEmpty;
        }

        /// <summary>
        /// Check if ignore required fields validation
        /// </summary>
        /// <param name="contactTypeFlag">contact Type Flag</param>
        /// <param name="propertyValue">property Value</param>
        /// <returns>true or false</returns>
        private static bool IgnoreContactReqFieldValidation(string contactTypeFlag, string propertyValue)
        {
            if (ContactType4License.Organization.ToString().Equals(contactTypeFlag, StringComparison.InvariantCultureIgnoreCase))
            {
                if (propertyValue.Equals("firstName", StringComparison.InvariantCultureIgnoreCase)
                    || propertyValue.Equals("lastName", StringComparison.InvariantCultureIgnoreCase)
                    || propertyValue.Equals("middleName", StringComparison.InvariantCultureIgnoreCase)
                    || propertyValue.Equals("fullName", StringComparison.InvariantCultureIgnoreCase)
                    || propertyValue.Equals("maskedSsn", StringComparison.InvariantCultureIgnoreCase)
                    || propertyValue.Equals("title", StringComparison.InvariantCultureIgnoreCase)
                    || propertyValue.Equals("socialSecurityNumber", StringComparison.InvariantCultureIgnoreCase)
                    || propertyValue.Equals("salutation", StringComparison.InvariantCultureIgnoreCase)
                    || propertyValue.Equals("gender", StringComparison.InvariantCultureIgnoreCase)
                    || propertyValue.Equals("birthDate", StringComparison.InvariantCultureIgnoreCase)
                    || propertyValue.Equals("birthCity", StringComparison.InvariantCultureIgnoreCase)
                    || propertyValue.Equals("birthState", StringComparison.InvariantCultureIgnoreCase)
                    || propertyValue.Equals("birthRegion", StringComparison.InvariantCultureIgnoreCase)
                    || propertyValue.Equals("race", StringComparison.InvariantCultureIgnoreCase)
                    || propertyValue.Equals("deceasedDate", StringComparison.InvariantCultureIgnoreCase)
                    || propertyValue.Equals("passportNumber", StringComparison.InvariantCultureIgnoreCase)
                    || propertyValue.Equals("driverLicenseNbr", StringComparison.InvariantCultureIgnoreCase)
                    || propertyValue.Equals("driverLicenseState", StringComparison.InvariantCultureIgnoreCase)
                    || propertyValue.Equals("stateIDNbr", StringComparison.InvariantCultureIgnoreCase))
                {
                    return true;
                }
            }

            if (ContactType4License.Individual.ToString().Equals(contactTypeFlag, StringComparison.InvariantCultureIgnoreCase))
            {
                if (propertyValue.Equals("businessName", StringComparison.InvariantCultureIgnoreCase)
                    || propertyValue.Equals("tradeName", StringComparison.InvariantCultureIgnoreCase)               
                    || propertyValue.Equals("fein", StringComparison.InvariantCultureIgnoreCase))
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Get the corresponding property value according to property name.
        /// </summary>
        /// <param name="model">the object of have the property value.</param>
        /// <param name="propertyName">property name.</param>
        /// <returns>the property value.</returns>
        private static object GetPropertyValue(object model, string propertyName)
        {
            // get all properties of model.
            if (model == null)
            {
                return null;
            }

            Type objectType = model.GetType();
            PropertyInfo[] properties = objectType.GetProperties();

            return GetPropertyValue(model, propertyName, properties);
        }

        /// <summary>
        /// Get the corresponding property value according to property name.
        /// </summary>
        /// <param name="model">the object of have the property value.</param>
        /// <param name="propertyName">property name.</param>
        /// <param name="properties">PropertyInfo list.</param>
        /// <returns>the property value.</returns>
        private static object GetPropertyValue(object model, string propertyName, PropertyInfo[] properties)
        {
            if (model == null || string.IsNullOrEmpty(propertyName))
            {
                return null;
            }

            Type objectType = model.GetType();

            PropertyInfo pi = GetPropertyInfo(objectType, propertyName, properties);

            if (pi != null)
            {
                return pi.GetValue(model, null);
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Get PropertyInfo object
        /// </summary>
        /// <param name="objectType">model type</param>
        /// <param name="propertyName">property name</param>
        /// <param name="properties">model properties</param>
        /// <returns>PropertyInfo object</returns>
        private static PropertyInfo GetPropertyInfo(Type objectType, string propertyName, PropertyInfo[] properties)
        {
            PropertyInfo pi = null;

            foreach (PropertyInfo property in properties)
            {
                if (property.Name.Equals(propertyName, StringComparison.InvariantCultureIgnoreCase))
                {
                    pi = objectType.GetProperty(property.Name);
                    break;
                }
            }

            return pi;
        }

        /// <summary>
        /// Get all required fields from GUI Element table.
        /// </summary>
        /// <param name="simpleViewElementModels">simple view element models</param>
        /// <param name="exceptRequiredField">the required fields that except for validation, etc: don't validate finalScore when gradingStyle is none.</param>
        /// <param name="useZip">if set to <c>true</c> current regional setting use the Zip field,
        /// <c>false</c> means not use the Zip field and will not execute the require validation for Zip field.</param>
        /// <returns>property name list</returns>
        private static string[] GetPropertyNames(SimpleViewElementModel4WS[] simpleViewElementModels, string exceptRequiredField, bool useZip)
        {
            if (simpleViewElementModels == null || simpleViewElementModels.Length == 0)
            {
                return null;
            }

            ArrayList requiredFields = new ArrayList();

            //Add of requried fields into list.
            foreach (SimpleViewElementModel4WS simpleViewElement in simpleViewElementModels)
            {
                if (IsYes(simpleViewElement.required) &&
                    ACAConstant.VALID_STATUS.Equals(simpleViewElement.recStatus, StringComparison.InvariantCultureIgnoreCase) &&
                    ACAConstant.COMMON_Y.Equals(simpleViewElement.standard, StringComparison.InvariantCultureIgnoreCase) &&
                    simpleViewElement.viewElementName != exceptRequiredField)
                {
                    if (IsZipField(simpleViewElement) && !useZip)
                    {
                        continue;
                    }
                    else
                    {
                        requiredFields.Add(simpleViewElement.viewElementDesc);
                    }
                }
            }

            string[] propertyNames = null;

            if (requiredFields.Count > 0)
            {
                propertyNames = (string[])requiredFields.ToArray(typeof(string));
            }

            return propertyNames;
        }

        /// <summary>
        /// Determines whether [is zip field] [the specified simple view element].
        /// </summary>
        /// <param name="simpleViewElement">The simple view element.</param>
        /// <returns>true if it is a zip field.</returns>
        private static bool IsZipField(SimpleViewElementModel4WS simpleViewElement)
        {
            bool isZipFiled = false;

            string viewElementDesc = simpleViewElement.viewElementDesc;

            isZipFiled = viewElementDesc.EndsWith("*zip", StringComparison.OrdinalIgnoreCase);

            return isZipFiled;
        }

        /// <summary>
        /// RemoteCertificateValidation Callback
        /// </summary>
        /// <param name="sender">Object sender</param>
        /// <param name="certificate">X509 Certificate</param>
        /// <param name="chain">X509 Chain</param>
        /// <param name="errors">SSLPolicy Errors</param>
        /// <returns>true if it is the validated result.</returns>
        private static bool CheckValidationResult(object sender, System.Security.Cryptography.X509Certificates.X509Certificate certificate, X509Chain chain, SslPolicyErrors errors)
        {
            return true;
        }

        #endregion Methods
    }
}
