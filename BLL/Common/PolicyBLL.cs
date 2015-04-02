#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: IBizDomainBll.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2008-2014
 *
 *  Description:
 *
 *  Notes:
 * $Id: PolicyBLL.cs 277237 2014-08-13 01:17:52Z ACHIEVO\james.shi $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Accela.ACA.Common;
using Accela.ACA.Common.Log;
using Accela.ACA.Common.Util;
using Accela.ACA.WSProxy;

using log4net;

namespace Accela.ACA.BLL.Common
{
    /// <summary>
    /// This class provide the ability to get policy value.
    /// </summary>
    public class PolicyBLL : BaseBll, IPolicyBLL
    {
        #region Methods

        /// <summary>
        /// Get policy list by policy model.
        /// </summary>
        /// <param name="policyName">policy name</param>
        /// <param name="moduleName">module name</param>
        /// <returns>an list of policy object</returns>
        /// <exception cref="ACAException">Exception from web service.</exception>
        public IList<ItemValue> GetPolicyListForPayment(string policyName, string moduleName)
        {
            IList<ItemValue> items = new List<ItemValue>();

            try
            {
                ICacheManager cacheManager = (ICacheManager)ObjectFactory.GetObject(typeof(ICacheManager));
                Hashtable htStdCates = cacheManager.GetCachedItem(AgencyCode, I18nCultureUtil.AppendUserPreferredCultureFlag(CacheConstant.CACHE_KEY_HARDCODE));

                if (htStdCates == null)
                {
                    return items;
                }

                Hashtable htStdItems = htStdCates[policyName] as Hashtable;

                if (htStdItems != null && htStdItems.Count > 0)
                {
                    if (!string.IsNullOrEmpty(moduleName))
                    {
                        items = GetModuleOrAgencyDatas(htStdItems, moduleName, ACAConstant.VALID_STATUS);
                    }
                    
                    if (items == null || items.Count == 0)
                    {
                        items = GetPaymentItems(htStdItems);
                    }

                    ((List<ItemValue>)items).Sort(ItemValueComparer.Instance);
                }

                return items;
            }
            catch (Exception e)
            {
                throw new ACAException(e);
            }
        }
        
        /// <summary>
        /// Get policy list by policy name.
        /// </summary>
        /// <param name="policyName">policy name</param>
        /// <param name="moduleName">module name</param>
        /// <returns>an list of XPolicyModel</returns>
        public IList<ItemValue> GetPolicyList(string policyName, string moduleName)
        {
            IList<ItemValue> items = new List<ItemValue>();

            try
            {
                ICacheManager cacheManager = ObjectFactory.GetObject<ICacheManager>();
                Hashtable htStdCates = cacheManager.GetCachedItem(AgencyCode, I18nCultureUtil.AppendUserPreferredCultureFlag(CacheConstant.CACHE_KEY_HARDCODE));

                if (htStdCates == null)
                {
                    return items;
                }

                string bizName = policyName;
                Hashtable htStdItems = htStdCates[bizName] as Hashtable;

                if (htStdItems != null && htStdItems.Count > 0)
                {
                    items = GetPolicyItems(htStdItems, moduleName);
                    ((List<ItemValue>)items).Sort(ItemValueComparer.Instance);
                }

                return items;
            }
            catch (Exception e)
            {
                throw new ACAException(e);
            }
        }

        /// <summary>
        /// Get Policy items. If it is module level , should merge agency level settings.
        /// </summary>
        /// <param name="htStdItems">all policy items.</param>
        /// <param name="moduleName">the module name.</param>
        /// <returns>policy items.</returns>
        private IList<ItemValue> GetPolicyItems(Hashtable htStdItems, string moduleName)
        {
            IList<ItemValue> validItems = new List<ItemValue>();
            IList<ItemValue> inValidItems = new List<ItemValue>();
            IList<ItemValue> globalItems = new List<ItemValue>();
            validItems = GetModuleOrAgencyDatas(htStdItems, moduleName, ACAConstant.VALID_STATUS);
            inValidItems = GetModuleOrAgencyDatas(htStdItems, moduleName, ACAConstant.INVALID_STATUS);
            globalItems = GetGlobalDatas(htStdItems);

            //if no select item in aca admin should display all item in aca daily
            if (validItems == null || validItems.Count == 0)
            {
                return globalItems;
            }

            IList<ItemValue> items = new List<ItemValue>();
            ArrayList keyList = new ArrayList();

            //record valid item and key
            foreach (ItemValue validItem in validItems)
            {
                if (!keyList.Contains(validItem.Value))
                {
                    items.Add(validItem);
                    keyList.Add(validItem.Value);
                }
            }

            //record invalid key
            foreach (ItemValue inValidItem in inValidItems)
            {
                if (!keyList.Contains(inValidItem.Value))
                {
                    keyList.Add(inValidItem.Value);
                }
            }

            //record valid item and not record invalid item.
            foreach (ItemValue globalItem in globalItems)
            {
                if (!keyList.Contains(globalItem.Value))
                {
                    items.Add(globalItem);
                }
            }

            return items;
        }

        /// <summary>
        /// Get policy data which are global level.
        /// </summary>
        /// <param name="htStdItems">Hashtable:that contains all cached hard code drop down list items</param>
        /// <returns>a list of item</returns>
        private IList<ItemValue> GetGlobalDatas(Hashtable htStdItems)
        {
            IList<ItemValue> globalItems = new List<ItemValue>();

            foreach (string key in htStdItems.Keys)
            {
                XPolicyModel xPolicy = htStdItems[key] as XPolicyModel;
                ItemValue item = new ItemValue();
                string modulePrefix = ACAConstant.LEVEL_TYPE_MODULE + ":";
                string agencyPrefix = AgencyCode + ACAConstant.SPLIT_CHAR5;

                // it's not module and agency level
                if (!key.Contains(modulePrefix)
                    && !key.Contains(agencyPrefix)
                    && ACAConstant.VALID_STATUS.Equals(xPolicy.recStatus))
                {
                    // gloable level
                    item.Key = key;
                    item.Value = xPolicy.data1;
                    globalItems.Add(item);
                }
            }

            return globalItems;
        }

        /// <summary>
        /// Get policy data which are global level.
        /// </summary>
        /// <param name="htStdItems">Hashtable:that contains all cached hard code drop down list items</param>
        /// <returns>a list of item</returns>
        private IList<ItemValue> GetPaymentItems(Hashtable htStdItems)
        {
            IList<ItemValue> agencyLevelItems = new List<ItemValue>();
            IList<ItemValue> globalLevelItems = new List<ItemValue>();
            IList<ItemValue> items = new List<ItemValue>();

            foreach (string key in htStdItems.Keys)
            {
                XPolicyModel xPolicy = htStdItems[key] as XPolicyModel;
                ItemValue item = new ItemValue();
                string agencyPrefix = AgencyCode + "_";
                string modulePrefix = ACAConstant.LEVEL_TYPE_MODULE + ":";

                // it's agency level.
                if (key.Contains(agencyPrefix))
                {
                    if (xPolicy.recStatus == ACAConstant.VALID_STATUS)
                    {
                        item.Key = key.Substring(agencyPrefix.Length);
                        item.Value = xPolicy.data1;
                        agencyLevelItems.Add(item);
                    }
                }
                else if (!key.Contains(modulePrefix))
                {
                    item.Key = key;
                    item.Value = xPolicy.data1;
                    globalLevelItems.Add(item);
                }
            }

            if (agencyLevelItems.Count > 0)
            {
                items = agencyLevelItems;
            }
            else
            {
                items = globalLevelItems;
            }

            return items;
        }

        /// <summary>
        /// Get policy data which are module or agency level.
        /// </summary>
        /// <param name="htStdItems">Hashtable:that contains all cached hard code drop down list items</param>
        /// <param name="moduleName">Module name</param>
        /// <param name="status">status for XPolicy</param>
        /// <returns>a list of item</returns>
        private IList<ItemValue> GetModuleOrAgencyDatas(Hashtable htStdItems, string moduleName, string status)
        {
            IList<ItemValue> items = new List<ItemValue>();

            foreach (string key in htStdItems.Keys)
            {
                XPolicyModel xPolicy = htStdItems[key] as XPolicyModel;
                ItemValue item = new ItemValue();
                string modulePrefix = ACAConstant.LEVEL_TYPE_MODULE + ":";
                string agencyPrefix = AgencyCode + ACAConstant.SPLIT_CHAR5;

                if (!string.IsNullOrEmpty(moduleName))
                {
                    //module level
                    if (key.Contains(modulePrefix) && status.Equals(xPolicy.recStatus))
                    {
                        string tmp = key.Substring(modulePrefix.Length);
                        int length = moduleName.Length;

                        if (tmp.StartsWith(moduleName, StringComparison.InvariantCultureIgnoreCase))
                        {
                            item.Key = tmp.Substring(length + 1);
                            item.Value = xPolicy.data1;
                            items.Add(item);
                        }
                    }
                }
                else
                {
                    // it's not module level
                    if (!key.Contains(modulePrefix)
                        && key.Contains(agencyPrefix)
                        && status.Equals(xPolicy.recStatus))
                    {
                        // agency level
                        item.Key = key.Substring(agencyPrefix.Length);
                        item.Value = xPolicy.data1;
                        items.Add(item);
                    }
                }
            }

            return items;
        }

        #endregion Methods
        }
}