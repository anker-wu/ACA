#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: XPolicyBll.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2007-2014
 *
 *  Description:
 *
 *  Notes:
 * $Id: XPolicyBll.cs 277355 2014-08-14 07:28:03Z ACHIEVO\james.shi $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 *   10/12/2007      Daly zeng
 *   11/27/2008      Jackie Yu                  Removed configurable cache expiration interval
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
    /// Provided methods for XPolicy.
    /// </summary>
    public class XPolicyBll : BaseBll, IXPolicyBll
    {
        #region Fields

        /// <summary>
        /// logger object.
        /// </summary>
        private static readonly ILog Logger = LogFactory.Instance.GetLogger(typeof(XPolicyBll));

        #endregion Fields

        #region Properties

        /// <summary>
        /// Gets an instance of XPolicyService.
        /// </summary>
        private PolicyWebServiceService XPolicyService
        {
            get
            {
                return WSFactory.Instance.GetWebService<PolicyWebServiceService>();
            }
        }

        #endregion Properties

        #region Methods

        /// <summary>
        ///  Method to create or update a policy
        /// </summary>
        /// <param name="agencyCode">agency code</param>
        /// <param name="policy">the XPolicyUserRolePrivilegeModel4WS model</param>
        /// <param name="callerId">public user id</param>
        public void CreateOrUpdatePolicy(string agencyCode, XpolicyUserRolePrivilegeModel[] policy, string callerId)
        {
            return;
        }

        /// <summary>
        /// Create or update policy
        /// </summary>
        /// <param name="policiesWithLan">the models which need support multiple language</param>
        /// <param name="policiesWithoutLan">the models which ignore multiple language</param>
        /// <exception cref="System.NotImplementedException">The method not implement</exception>
        public void CreateOrUpdatePolicy(XPolicyModel[] policiesWithLan, XPolicyModel[] policiesWithoutLan)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Create or update policy as data3 is key 
        /// </summary>
        /// <param name="policiesWithLan">XPolicyModel array</param>
        /// <param name="policiesWithoutLan">XPolicyModel array without language</param>
        public void CreateOrUpdateXPolicyForData3AsKey(XPolicyModel[] policiesWithLan, XPolicyModel[] policiesWithoutLan)
        {
            return;
        }

        /// <summary>
        /// this method get all ACA policy for an agency
        /// </summary>
        /// <param name="agencyCode">agency code</param>
        /// <returns>return an array of XPolicyUserRolePrivilegeModel4WS instances</returns>
        public Hashtable GetAllACAPolicys(string agencyCode)
        {
            return null;
        }

        /// <summary>
        /// Get EPaymentPolicyModel
        /// </summary>
        /// <returns>a XPolicyModel</returns>
        public XPolicyModel GetEPaymentPolicyModel()
        {
            ArrayList models = GetPolicyModelsByName("PaymentAdapters");

            if (models == null || models.Count == 0)
            {
                return null;
            }

            IBizDomainBll bizBll = ObjectFactory.GetObject<IBizDomainBll>();
            string adapterType = bizBll.GetValueForStandardChoice(AgencyCode, BizDomainConstant.STD_CAT_EPAYMENT_CONFIG, "ACAAdapterType");

            foreach (XPolicyModel model in models)
            {
                if (model.levelData == adapterType)
                {
                    return model;
                }
            }

            return null;
        }

        /// <summary>
        /// Method to get a policy from cache. Agency + PolicyName + levelType + levelData is the key,The value is a XPolicyUserRolePrivilegeModel
        /// </summary>
        /// <param name="agencyCode">current agency code</param>
        /// <param name="policyName">the name of policy such as ACA_CAP_SEARCH_USER_ROLES</param>
        /// <param name="levelType">indicates whether the policy is in a module level or agency level or global level</param>
        /// <param name="levelData">If module level, the module name. if agency level the agency code</param>
        /// <returns>return an array of XPolicyUserRolePrivilegeModel4WS instances</returns>
        /// <exception cref="ACAException">Exception from web service.</exception>
        public XpolicyUserRolePrivilegeModel GetPolicy(string agencyCode, string policyName, string levelType, string levelData)
        {
            try
            {
                ICacheManager cacheManager = ObjectFactory.GetObject<ICacheManager>();
                string cacheKey = agencyCode + ACAConstant.SPLIT_CHAR + I18nCultureUtil.AppendUserPreferredCultureFlag(CacheConstant.CACHE_KEY_POLICY);
                Hashtable htXPolicys = cacheManager.GetCachedItem(agencyCode, cacheKey);

                string key = agencyCode + policyName;

                if (htXPolicys.ContainsKey(key) && htXPolicys[key] != null)
                {
                    ArrayList policys = htXPolicys[key] as ArrayList;

                    foreach (XpolicyUserRolePrivilegeModel policy in policys)
                    {
                        if (string.Equals(policy.level, levelType) && string.Equals(policy.levelData, levelData))
                        {
                            return policy;
                        }
                    }
                }

                return null;
            }
            catch (Exception e)
            {
                throw new ACAException(e);
            }
        }

        /// <summary>
        /// Get ACA policy models by category, this method will get policies by route: agency -> global
        /// </summary>
        /// <param name="category">category name</param>
        /// <returns>a policy model array
        /// data1:category name
        /// data2:item value
        /// data3:not used
        /// data4:item key
        /// </returns>
        public XPolicyModel[] GetPolicyListByCategory(string category)
        {
            List<XPolicyModel> policies = GetPolicyListByPolicyName(BizDomainConstant.STD_CAT_ACA_CONFIGS, AgencyCode);
            if (policies == null || policies.Count == 0)
            {
                return null;
            }

            //agency level
            IEnumerable<XPolicyModel> ps = SelectPoliciesByCategory(policies, category, ACAConstant.LEVEL_TYPE_AGENCY, AgencyCode);

            //global level
            IEnumerable<XPolicyModel> gps = SelectPoliciesByCategory(policies, category, ACAConstant.GLOBAL, ACAConstant.STANDARDDATA);

            return GetPolicyList(ps, gps);
        }

        /// <summary>
        /// Get ACA policy models by category, this method will get policies by route: module -> agency -> global
        /// </summary>
        /// <param name="category">category name</param>
        /// <param name="moduleName">Module Name</param>
        /// <returns>a policy model array
        /// data1:category name
        /// data2:item value
        /// data3:not used
        /// data4:item key
        /// </returns>
        public XPolicyModel[] GetPolicyListByCategory(string category, string moduleName)
        {
            List<XPolicyModel> policies = GetPolicyListByPolicyName(BizDomainConstant.STD_CAT_ACA_CONFIGS, AgencyCode);
            if (policies == null || policies.Count == 0)
            {
                return null;
            }

            //module level
            IEnumerable<XPolicyModel> ps = SelectPoliciesByCategory(policies, category, ACAConstant.LEVEL_TYPE_MODULE, moduleName);

            if (ps == null || !ps.Any())
            {
                //agency level
                ps = SelectPoliciesByCategory(policies, category, ACAConstant.LEVEL_TYPE_AGENCY, AgencyCode);
            }

            //global level
            IEnumerable<XPolicyModel> gps = SelectPoliciesByCategory(policies, category, ACAConstant.GLOBAL, ACAConstant.STANDARDDATA);

            return GetPolicyList(ps, gps);
        }

        /// <summary>
        /// Get ACA policy models by category, this method will get policies by given level type and level data
        /// and will not go through parent level
        /// </summary>
        /// <param name="category">category name</param>
        /// <param name="levelType">level type, includes: STANDARDDATA, AGENCY, MODULE</param>
        /// <param name="levelData">level data</param>
        /// <returns>a policy model array
        /// data1:category name
        /// data2:item value
        /// data3:not used
        /// data4:item key
        /// </returns>
        public XPolicyModel[] GetPolicyListByCategory(string category, string levelType, string levelData)
        {
            return GetPolicyListByCategory(AgencyCode, category, levelType, levelData);
        }

        /// <summary>
        /// Get ACA policy models by category, this method will get policies by given level type and level data
        /// and will not go through parent level
        /// </summary>
        /// <param name="agencyCode">agency code</param>
        /// <param name="category">category name</param>
        /// <param name="levelType">level type, includes: STANDARDDATA, AGENCY, MODULE</param>
        /// <param name="levelData">level data</param>
        /// <returns>a policy model array
        /// data1:category name
        /// data2:item value
        /// data3:not used
        /// data4:item key
        /// </returns>
        public XPolicyModel[] GetPolicyListByCategory(string agencyCode, string category, string levelType, string levelData)
        {
            List<XPolicyModel> policies = GetPolicyListByPolicyName(BizDomainConstant.STD_CAT_ACA_CONFIGS, agencyCode);

            if (policies == null || policies.Count == 0)
            {
                return null;
            }

            //module level
            IEnumerable<XPolicyModel> ps = SelectPoliciesByCategory(policies, category, levelType, levelData);

            if (ps == null || !ps.Any())
            {
                return null;
            }

            return ps.ToArray();
        }

        /// <summary>
        /// Get Auto Fill policy models by category, this method will get policies by given level type and level data
        /// </summary>
        /// <param name="category">category name</param>
        /// <param name="levelType">level type, includes: STANDARDDATA, AGENCY, MODULE</param>
        /// <param name="levelData">level data</param>
        /// <param name="data4">position ID or label key.</param>
        /// <returns>a policy model array</returns>
        public string GetPolicyValueForData4AsKey(string category, string levelType, string levelData, string data4)
        {
            string autoFillValue = string.Empty;

            List<XPolicyModel> policies = GetPolicyListByPolicyName(BizDomainConstant.STD_CAT_ACA_CONFIGS, AgencyCode);

            if (policies == null || policies.Count == 0)
            {
                return null;
            }

            IEnumerable<XPolicyModel> selectPolicies = SelectPoliciesForData4AsKey(policies, category, levelType, levelData, data4);

            if (selectPolicies != null && selectPolicies.Any())
            {
                autoFillValue = GetValue(selectPolicies.ToArray());
            }

            return autoFillValue;
        }

        /// <summary>
        /// Get and merge search type items from Global and Agency level.
        /// </summary>
        /// <returns>
        /// a policy model array
        /// </returns>
        public List<XPolicyModel> GetSearchTypeItems()
        {
            List<XPolicyModel> policies = GetPolicyListByPolicyName(BizDomainConstant.STD_CAT_ACA_CONFIGS, AgencyCode);

            if (policies == null || policies.Count == 0)
            {
                return null;
            }

            //global level
            IEnumerable<XPolicyModel> gps = SelectPoliciesByCategory(policies, ACAConstant.EDUCATION_LOOKUP, ACAConstant.GLOBAL, ACAConstant.STANDARDDATA);

            //agency level
            IEnumerable<XPolicyModel> ps = SelectPoliciesByCategory(policies, ACAConstant.EDUCATION_LOOKUP, ACAConstant.LEVEL_TYPE_AGENCY, AgencyCode);

            return UnionSearchTypeItems(gps, ps);
        }

        /// <summary>
        /// Get Policy List by Policy Name
        /// </summary>
        /// <param name="policyName">a policy name</param>
        /// <param name="agencyCode">The agency code.</param>
        /// <returns>a policy model array</returns>
        public List<XPolicyModel> GetPolicyListByPolicyName(string policyName, string agencyCode)
        {
            ICacheManager cacheManager = ObjectFactory.GetObject<ICacheManager>();
            string cacheKey = agencyCode + ACAConstant.SPLIT_CHAR + I18nCultureUtil.AppendUserPreferredCultureFlag(CacheConstant.CACHE_KEY_ALL_POLICY) + "_Daily";
            Hashtable htXpolicies = cacheManager.GetCachedItem(agencyCode, cacheKey);

            if (htXpolicies.ContainsKey(policyName))
            {
                return htXpolicies[policyName] as List<XPolicyModel>;
            }

            return null;
        }

        /// <summary>
        /// Get policy models by policy name
        /// </summary>
        /// <param name="policyName">string policy name</param>
        /// <returns>array list.</returns>
        public ArrayList GetPolicyModelsByName(string policyName)
        {
            if (Logger.IsDebugEnabled)
            {
                Logger.Debug("Begin Get XPolicy model from cache");
            }

            try
            {
                ICacheManager cacheManager = ObjectFactory.GetObject<ICacheManager>();
                Hashtable htXPolicys = cacheManager.GetCachedItem(AgencyCode, I18nCultureUtil.AppendUserPreferredCultureFlag(CacheConstant.CACHE_KEY_XPOLICY));

                ArrayList policys = null;
                string key = AgencyCode + policyName;

                // get agency level plocy
                if (htXPolicys.ContainsKey(key) && htXPolicys[key] != null)
                {
                    policys = htXPolicys[key] as ArrayList;
                }
                else
                {
                    // if it doesn't exist agency level policy, return golobal level policy model.
                    key = ACAConstant.STANDARDDATA + policyName;

                    if (htXPolicys.ContainsKey(key) && htXPolicys[key] != null)
                    {
                        policys = htXPolicys[key] as ArrayList;
                    }
                }

                if (Logger.IsDebugEnabled)
                {
                    Logger.Debug("End Get GetPolicyModelsByName model from cache");
                }

                return policys;
            }
            catch (Exception e)
            {
                throw new ACAException(e);
            }
        }

        /// <summary>
        /// Get value by key, this method will get policy by route: module -> agency -> global
        /// </summary>
        /// <param name="key">string policy key</param>
        /// <param name="moduleName">Module Name</param>
        /// <returns>a policy model</returns>
        public string GetValueByKey(string key, string moduleName)
        {
            List<XPolicyModel> policies = GetPolicyListByPolicyName(BizDomainConstant.STD_CAT_ACA_CONFIGS, AgencyCode);
            if (policies == null || policies.Count == 0)
            {
                return null;
            }

            //module level
            IEnumerable<XPolicyModel> ps = SelectPoliciesByKey(policies, key, ACAConstant.LEVEL_TYPE_MODULE, moduleName);

            if (ps == null || !ps.Any())
            {
                //agency level
                ps = SelectPoliciesByKey(policies, key, ACAConstant.LEVEL_TYPE_AGENCY, AgencyCode);

                if (ps == null || !ps.Any())
                {
                    //global level
                    ps = SelectPoliciesByCategory(policies, key, ACAConstant.GLOBAL, ACAConstant.STANDARDDATA);
                }
            }

            return GetValue(ps);
        }

        /// <summary>
        /// Get value by key, this method will get policy by route: agency -&gt; global
        /// </summary>
        /// <param name="key">string key</param>
        /// <returns>a policy model</returns>
        public string GetSuperAgencyValueByKey(string key)
        {
            List<XPolicyModel> policies = GetPolicyListByPolicyName(BizDomainConstant.STD_CAT_ACA_CONFIGS, SuperAgencyCode);

            if (policies == null || policies.Count == 0)
            {
                return null;
            }

            //agency level
            IEnumerable<XPolicyModel> ps = SelectPoliciesByKey(policies, key, ACAConstant.LEVEL_TYPE_AGENCY, SuperAgencyCode);

            if (ps == null || ps.Count() == 0)
            {
                //global level
                ps = SelectPoliciesByCategory(policies, key, ACAConstant.GLOBAL, ACAConstant.STANDARDDATA);
            }

            return GetValue(ps);
        }

        /// <summary>
        /// Get value by key, this method will get policy by route: agency -> global
        /// </summary>
        /// <param name="key">string key.</param>
        /// <returns>a policy model</returns>
        public string GetValueByKey(string key)
        {
            List<XPolicyModel> policies = GetPolicyListByPolicyName(BizDomainConstant.STD_CAT_ACA_CONFIGS, AgencyCode);
            if (policies == null || policies.Count == 0)
            {
                return null;
            }

            //agency level
            IEnumerable<XPolicyModel> ps = SelectPoliciesByKey(policies, key, ACAConstant.LEVEL_TYPE_AGENCY, AgencyCode);

            if (ps == null || !ps.Any())
            {
                //global level
                ps = SelectPoliciesByCategory(policies, key, ACAConstant.GLOBAL, ACAConstant.STANDARDDATA);
            }

            return GetValue(ps);
        }

        /// <summary>
        /// get value by given key, level type and level data. this method will not go through parent level
        /// </summary>
        /// <param name="key">policy key</param>
        /// <param name="levelType">level type, includes: STANDARDDATA, AGENCY, MODULE</param>
        /// <param name="levelData">level data</param>
        /// <returns>a policy model</returns>
        public string GetValueByKey(string key, string levelType, string levelData)
        {
            List<XPolicyModel> policies = GetPolicyListByPolicyName(BizDomainConstant.STD_CAT_ACA_CONFIGS, AgencyCode);
            if (policies == null || policies.Count == 0)
            {
                return null;
            }

            return GetValue(SelectPoliciesByKey(policies, key, levelType, levelData));
        }

        /// <summary>
        /// Creates the original update policy.
        /// </summary>
        /// <param name="xpolicy4WSArray">The XPolicy array.</param>
        /// <param name="ignoreLanguage">if set to <c>true</c> [ignore language].</param>
        /// <exception cref="System.NotImplementedException">The method not implement</exception>
        void IXPolicyBll.CreateOrUpdatePolicy(XPolicyModel[] xpolicy4WSArray, bool ignoreLanguage)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Method to get a policy from cache. Agency + PolicyName + levelType + levelData + data1 is the key,The value is a UserRolePrivilegeModel
        /// </summary>
        /// <param name="moduleName">Module Name</param>
        /// <param name="contactType">Contact Type, it be saved in policy data1 field</param>
        /// <returns>return a UserRolePrivilegeModel instance</returns>
        /// <exception cref="ACAException">Exception from web service.</exception>
        UserRolePrivilegeModel IXPolicyBll.GetPolicyByContactType(string moduleName, string contactType)
        {
            //if don't configur role on current contact type then ALL ACA user is default value of current contact
            UserRolePrivilegeModel defaultUserRole = new UserRolePrivilegeModel();
            defaultUserRole.allAcaUserAllowed = true;

            if (string.IsNullOrEmpty(contactType))
            {
                return defaultUserRole;
            }

            if (Logger.IsDebugEnabled)
            {
                Logger.Debug("Begin Get XPolicy model from cache");
            }

            try
            {
                ICacheManager cacheManager = ObjectFactory.GetObject<ICacheManager>();
                Hashtable htXPolicys = cacheManager.GetCachedItem(AgencyCode, I18nCultureUtil.AppendUserPreferredCultureFlag(CacheConstant.CACHE_KEY_POLICY));

                string key = AgencyCode + ACAConstant.ACA_CONTACT_TYPE_USER_ROLES;

                if (htXPolicys.ContainsKey(key) && htXPolicys[key] != null)
                {
                    ArrayList policys = htXPolicys[key] as ArrayList;

                    if (policys == null)
                    {
                        return null;
                    }

                    foreach (XpolicyUserRolePrivilegeModel policy in policys)
                    {
                        if (policy == null || policy.level == null || policy.levelData == null || policy.data1 == null)
                        {
                            continue;
                        }

                        if (policy.level.Equals(ACAConstant.LEVEL_TYPE_MODULE) && policy.levelData.Equals(moduleName) && policy.data1.Equals(contactType))
                        {
                            defaultUserRole = policy.userRolePrivilegeModel;
                            break;
                        }
                    }
                }

                if (Logger.IsDebugEnabled)
                {
                    Logger.Debug("End Get XPolicy model from cache");
                }

                return defaultUserRole;
            }
            catch (Exception e)
            {
                throw new ACAException(e);
            }
        }

        /// <summary>
        /// Get policy array by policy level.
        /// </summary>
        /// <param name="policyName">policy name</param>
        /// <param name="level">level type</param>
        /// <param name="levelData">level data</param>
        /// <param name="callerID">string callerID</param>
        /// <returns>Policy array</returns>
        /// <exception cref="ACAException">Exception from web service.</exception>
        XPolicyModel[] IXPolicyBll.GetXPolicyByLevel(string policyName, string level, string levelData, string callerID)
        {
            if (Logger.IsDebugEnabled)
            {
                Logger.Debug("Begin IXPolicyBll.GetXPolicyByLevel()");
            }

            try
            {
                XPolicyModel[] policyModels = XPolicyService.getXPolicyByLevel(AgencyCode, policyName, level, levelData, callerID);

                if (Logger.IsDebugEnabled)
                {
                    Logger.Debug("End IXPolicyBll.GetXPolicyByLevel()");
                }

                return policyModels;
            }
            catch (Exception e)
            {
                throw new ACAException(e);
            }
        }

        /// <summary>
        /// Get enabled search cross modules from policy.
        /// </summary>
        /// <param name="currentModuleName">current module name</param>
        /// <returns>name of modules</returns>
        List<string> IXPolicyBll.GetCrossModulesFromXPolicy(string currentModuleName)
        {
            ACAUserType userType = User == null || User.IsAnonymous ? ACAUserType.Anonymous : ACAUserType.Registered;
            List<string> enabledModules = new List<string>();
            XPolicyModel[] xpCrossModules = XPolicyService.getXPolicyByLevel(AgencyCode, BizDomainConstant.STD_CAT_ACA_CONFIGS, ACAConstant.LEVEL_TYPE_MODULE, currentModuleName, ACAConstant.ADMIN_CALLER_ID);
            IBizDomainBll bizBll = (IBizDomainBll)ObjectFactory.GetObject(typeof(IBizDomainBll));
            IList<TabItem> tabList = bizBll.GetTabsList(AgencyCode, userType, false);

            if (tabList != null)
            {
                if (xpCrossModules != null && xpCrossModules.Length > 0)
                {
                    var modules =
                        from item in xpCrossModules
                        where item != null && ACAConstant.ACA_ENABLE_SEARCH_CROSS_MODULE.Equals(item.data1) &&
                                   currentModuleName.Equals(item.levelData) && ACAConstant.COMMON_Y.Equals(item.data2) && ACAConstant.VALID_STATUS.Equals(item.status)
                        select item.data4;
                    enabledModules = modules.ToList<string>();

                    foreach (TabItem item in tabList)
                    {
                        //Filter disabled tab and "Home" tab from tabList.
                        if (item != null 
                            && ((enabledModules.Contains(item.Key) 
                                && (!item.TabVisible || string.IsNullOrEmpty(item.Label) || "aca_sys_default_home".Equals(item.Label))) 
                                || "aca_sys_apo_search".Equals(item.Label)))
                        {
                            enabledModules.Remove(item.Key);
                        }
                    }
                }
            }

            return enabledModules;
        }

        /// <summary>
        /// Removes the executable policies for contact type security.
        /// </summary>
        /// <param name="policyModels">The executable policy models.</param>
        void IXPolicyBll.RemoveXPoliciesForContactTypeSecurity(XPolicyModel[] policyModels)
        {
            XPolicyService.removeXPoliciesForContactTypeSecurity(policyModels);
        }

        /// <summary>
        /// Get Policy List.
        /// </summary>
        /// <param name="ps">source for IEnumerable</param>
        /// <param name="gps">target for IEnumerable</param>
        /// <returns>XPolicy model.</returns>
        private XPolicyModel[] GetPolicyList(IEnumerable<XPolicyModel> ps, IEnumerable<XPolicyModel> gps)
        {
            if (ps == null || !ps.Any())
            {
                if (gps == null || !gps.Any())
                {
                    return null;
                }
                else
                {
                    return gps.ToArray();
                }
            }

            if (gps == null || gps.Count() == 0)
            {
                return ps.ToArray();
            }

            //merge module/agency level and global level data
            return ps.Union(gps, new PolicyComparer()).ToArray();
        }

        /// <summary>
        /// Union search type items.
        /// </summary>
        /// <param name="globalData">the global level policy</param>
        /// <param name="agencyData">the agency level policy</param>
        /// <returns>the search type items.</returns>
        private List<XPolicyModel> UnionSearchTypeItems(IEnumerable<XPolicyModel> globalData, IEnumerable<XPolicyModel> agencyData)
        {
            List<XPolicyModel> genInfo = new List<XPolicyModel>();

            if (globalData == null || !globalData.Any())
            {
                return null;
            }

            if (agencyData == null || !agencyData.Any())
            {
                return globalData.ToList();
            }

            List<string> agencyLevelKeys = agencyData.Select(p => p.data4).ToList();
            
            //Some items existing in global level but not in agency level.
            IEnumerable<XPolicyModel> addedType = globalData.Where(g => !agencyLevelKeys.Contains(g.data4));
            genInfo.Union(addedType);

            bool needI18nValue = I18nCultureUtil.IsMultiLanguageEnabled && !I18nCultureUtil.IsInMasterLanguage;

            foreach (XPolicyModel agencyLevel in agencyData)
            {
                //Get I18n value.
                if (needI18nValue && string.IsNullOrEmpty(agencyLevel.resData2))
                {
                    agencyLevel.dispData2 = globalData
                        .Where(g => agencyLevel.data4.Equals(g.data4, StringComparison.OrdinalIgnoreCase))
                        .Select(g => g.dispData2).ToList()[0];
                }

                genInfo.Add(agencyLevel);
            }

            return genInfo == null ? null : genInfo.OrderBy(g => g.dispData2).ToList();
        }

        /// <summary>
        /// Get value.
        /// </summary>
        /// <param name="ps">XPolicy Model</param>
        /// <returns>resData2 for value.</returns>
        private string GetValue(IEnumerable<XPolicyModel> ps)
        {
            if (ps != null && ps.Count() > 0)
            {
                XPolicyModel model = ps.ElementAt(0);

                return string.IsNullOrEmpty(model.dispData2) ? model.data2 : model.dispData2;
            }

            return null;
        }

        /// <summary>
        /// Select Policies by Category
        /// </summary>
        /// <param name="policies">policy model</param>
        /// <param name="category">category name.</param>
        /// <param name="levelType">level type.</param>
        /// <param name="levelData">level data.</param>
        /// <returns>XPolicy model</returns>
        private IEnumerable<XPolicyModel> SelectPoliciesByCategory(List<XPolicyModel> policies, string category, string levelType, string levelData)
        {
            return from p in policies where p.data1 == category && p.level == levelType && p.levelData == levelData select p;
        }

        /// <summary>
        /// Select Policies by Category
        /// </summary>
        /// <param name="policies">policy model</param>
        /// <param name="key">policy key code.</param>
        /// <param name="levelType">level type.</param>
        /// <param name="levelData">level data.</param>
        /// <returns>XPolicy model</returns>
        private IEnumerable<XPolicyModel> SelectPoliciesByKey(List<XPolicyModel> policies, string key, string levelType, string levelData)
        {
            return from p in policies where p.data1 == key && p.data4 == key && p.level == levelType && p.levelData == levelData select p;
        }

        /// <summary>
        /// Select Policies by Category
        /// </summary>
        /// <param name="policies">policy model</param>
        /// <param name="key">policy key code.</param>
        /// <param name="levelType">level type.</param>
        /// <param name="levelData">level data.</param>
        /// <param name="data4">position ID or label key.</param>
        /// <returns>XPolicy model</returns>
        private IEnumerable<XPolicyModel> SelectPoliciesForData4AsKey(List<XPolicyModel> policies, string key, string levelType, string levelData, string data4)
        {
            return from p in policies where p.data1 == key && p.data4 == data4 && p.level == levelType && p.levelData == levelData select p;
        }

        #endregion Methods

        #region Nested Types

        /// <summary>
        /// the class for policy comparer.
        /// </summary>
        private class PolicyComparer : IEqualityComparer<XPolicyModel>
        {
            #region Methods

            /// <summary>
            /// Equals data.
            /// </summary>
            /// <param name="x">x: XPolicyModel</param>
            /// <param name="y">y: XPolicyModel</param>
            /// <returns>true or false.</returns>
            public bool Equals(XPolicyModel x, XPolicyModel y)
            {
                return x.data4 == y.data4;
            }

            /// <summary>
            /// Get Hash Code.
            /// </summary>
            /// <param name="obj">XPolicy Model</param>
            /// <returns>hash code for XPolicy.</returns>
            public int GetHashCode(XPolicyModel obj)
            {
                return obj.GetHashCode();
            }

            #endregion Methods
        }

        #endregion Nested Types
    }
}
