#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: GlobalSearchUtil.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2009-2014
 *
 *  Description:
 *  
 *
 *  Notes:
 *  $Id: GlobalSearchUtil.cs 130988 2009-9-1  9:43:01Z ACHIEVO\jackie.yu $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using System;
using System.Collections.Generic;
using System.Reflection;

using Accela.ACA.BLL.Common;
using Accela.ACA.Common;
using Accela.ACA.Common.Util;
using Accela.ACA.Web.Common.GlobalSearch;

namespace Accela.ACA.Web.Common
{
    /// <summary>
    /// Global search utility
    /// </summary>
    public static class GlobalSearchUtil
    {
        /// <summary>
        /// Gets all module keys.
        /// </summary>
        /// <returns>all module keys</returns>
        public static string[] GetAllModuleKeys()
        {
            string[] results = null;
            Dictionary<string, string> allModules = TabUtil.GetAllEnableModules(false);

            if (allModules != null)
            {
                results = new string[allModules.Count];
                allModules.Keys.CopyTo(results, 0);
            }

            return results;
        }

        /// <summary>
        /// Enable global search 
        /// </summary>
        /// <returns>true: if global search is enabled</returns>
        public static bool IsGlobalSearchEnabled()
        {
            IXPolicyBll xPolicyBll = (IXPolicyBll)ObjectFactory.GetObject(typeof(IXPolicyBll));
            string xPolicyValue = xPolicyBll.GetSuperAgencyValueByKey(ACAConstant.ENABLE_GLOBAL_SEARCH);

            bool isGlobalSearchEnabled = ValidationUtil.IsYes(xPolicyValue);

            return isGlobalSearchEnabled;
        }

        /// <summary>
        /// Enable records(CAP) list
        /// </summary>
        /// <returns>true: if record is enabled</returns>
        public static bool IsRecordEnabled()
        {
            IXPolicyBll xPolicyBll = (IXPolicyBll)ObjectFactory.GetObject(typeof(IXPolicyBll));
            string xPolicyValue = xPolicyBll.GetValueByKey(ACAConstant.ENABLE_GLOBAL_SEARCH_FOR_RECORDS);

            bool isRecordEnabled = !ValidationUtil.IsNo(xPolicyValue);

            return isRecordEnabled;
        }

        /// <summary>
        /// Enable LP(licensed professional) list
        /// </summary>
        /// <returns>true: if License profession is enabled</returns>
        public static bool IsLPEnabled()
        {
            IXPolicyBll xPolicyBll = (IXPolicyBll)ObjectFactory.GetObject(typeof(IXPolicyBll));
            string xPolicyValue = xPolicyBll.GetValueByKey(ACAConstant.ENABLE_GLOBAL_SEARCH_FOR_LP);

            bool isLPEnabled = !ValidationUtil.IsNo(xPolicyValue);

            return isLPEnabled;
        }

        /// <summary>
        /// Enable property information(APO) list
        /// </summary>
        /// <returns>true: if APO is enabled</returns>
        public static bool IsAPOEnabled()
        {
            IXPolicyBll xPolicyBll = (IXPolicyBll)ObjectFactory.GetObject(typeof(IXPolicyBll));
            string xPolicyValue = xPolicyBll.GetValueByKey(ACAConstant.ENABLE_GLOBAL_SEARCH_FOR_APO);

            bool isAPOEnabled = !ValidationUtil.IsNo(xPolicyValue);

            return isAPOEnabled;
        }

        /// <summary>
        /// Get Java property name
        /// </summary>
        /// <param name="searchType">search type</param>
        /// <param name="name">column name</param>
        /// <returns>the Java property name</returns>
        public static string GetJavaPropertyName(GlobalSearchType searchType, string name)
        {
            string javaPropertyName = string.Empty;
            Type type = GetViewType(searchType);

            if (type != null)
            {
                PropertyInfo[] properties = type.GetProperties();

                foreach (PropertyInfo p in properties)
                {
                    object[] keys = p.GetCustomAttributes(typeof(FieldMappingAttribute), true);

                    if (keys.Length == 1)
                    {
                        FieldMappingAttribute attr = (FieldMappingAttribute)keys[0];

                        if (p.Name == name)
                        {
                            javaPropertyName = attr.JavaPropertyName;
                            break;
                        }
                    }
                }
            }
            
            return javaPropertyName;
        }

        /// <summary>
        /// Get view type
        /// </summary>
        /// <param name="searchType">search type</param>
        /// <returns>View type for UI</returns>
        private static Type GetViewType(GlobalSearchType searchType)
        {
            Type type = null;

            switch (searchType)
            {
                case GlobalSearchType.CAP:
                    type = typeof(CAPView4UI);
                    break;
                case GlobalSearchType.LP:
                    type = typeof(LPView4UI);
                    break;
                case GlobalSearchType.PARCEL:
                case GlobalSearchType.ADDRESS:
                    type = typeof(APOView4UI);
                    break;
            }

            return type;
        }
    }
}
