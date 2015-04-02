#region Header

/*
 * <pre>
 *
 *  Accela Citizen Access
 *  File: XPolicyAdminBll.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2007-2014
 *
 *  Description:
 *
 *  Notes:
 *      $Id: XPolicyAdminBll.cs 278415 2014-09-03 08:45:04Z ACHIEVO\james.shi $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using Accela.ACA.BLL.Common;
using Accela.ACA.Common;
using Accela.ACA.Common.Log;
using Accela.ACA.Common.Util;
using Accela.ACA.WSProxy;

using log4net;

namespace Accela.ACA.AdminBLL
{
    /// <summary>
    /// Policy business class
    /// </summary>
    public class XPolicyAdminBll : BaseBll, IXPolicyBll
    {
        #region Fields

        /// <summary>
        /// an instance of ILog
        /// </summary>
        private static readonly ILog Logger = LogFactory.Instance.GetLogger(typeof(XPolicyAdminBll));

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
        /// Create or update policy
        /// </summary>
        /// <param name="policiesWithLan">the models which need support multiple language</param>
        /// <param name="policiesWithoutLan">the models which ignore multiple language</param>
        /// <exception cref="ACAException">Exception from web service.</exception>
        public void CreateOrUpdatePolicy(XPolicyModel[] policiesWithLan, XPolicyModel[] policiesWithoutLan)
        {
            if (Logger.IsDebugEnabled)
            {
                Logger.Debug("Begin IAdminXPolicyBll.CreateOrUpdatePolicy()");
            }

            try
            {
                SetPolicyName(policiesWithLan);
                SetPolicyName(policiesWithoutLan);

                XPolicyService.createOrUpdateXPolicy(policiesWithLan, policiesWithoutLan);

                ClearXpolicyCache();

                if (Logger.IsDebugEnabled)
                {
                    Logger.Debug("End IAdminXPolicyBll.CreateOrUpdatePolicy()");
                }
            }
            catch (Exception e)
            {
                throw new ACAException(e);
            }
        }

        /// <summary>
        /// Create or update policy as data3 is key
        /// </summary>
        /// <param name="policiesWithLan">XPolicyModel array</param>
        /// <param name="policiesWithoutLan">XPolicyModel array without language</param>
        /// <exception cref="ACAException">Exception from web service.</exception>
        public void CreateOrUpdateXPolicyForData3AsKey(XPolicyModel[] policiesWithLan, XPolicyModel[] policiesWithoutLan)
        {
            if (Logger.IsDebugEnabled)
            {
                Logger.Debug("Begin IAdminXPolicyBll.createOrUpdateXPolicyForData3AsKey()");
            }

            try
            {
                SetPolicyName(policiesWithLan);
                SetPolicyName(policiesWithoutLan);

                XPolicyService.createOrUpdateXPolicyForData3AsKey(policiesWithLan, policiesWithoutLan);

                ClearXpolicyCache();

                if (Logger.IsDebugEnabled)
                {
                    Logger.Debug("End IAdminXPolicyBll.createOrUpdateXPolicyForData3AsKey()");
                }
            }
            catch (Exception e)
            {
                throw new ACAException(e);
            }
        }

        /// <summary>
        /// this method get all ACA policy for an agency
        /// </summary>
        /// <param name="agencyCode">agency code</param>
        /// <returns>return an array of XPolicyUserRolePrivilegeModel4WS instances</returns>
        public Hashtable GetAllACAPolicys(string agencyCode)
        {
            // the "System" is only used for logging. The web service should be refactored to get all cap type filters without caller id.
            // the "ACA" means this method get policys for ACA.
            string culture = I18nCultureUtil.GetUserPreferredCulture();

            if (!string.IsNullOrEmpty(culture))
            {
                culture = I18nCultureUtil.ChangeCulture4WS(culture);
            }

            XpolicyUserRolePrivilegeModel[] policys = XPolicyService.getXpolicyUserRoleList(agencyCode, "ACA", "System", false, culture);

            Hashtable htPolicys = new Hashtable();

            if (policys == null || policys.Length == 0)
            {
                return htPolicys;
            }

            ArrayList groupedXPolicy;

            foreach (XpolicyUserRolePrivilegeModel policy in policys)
            {
                string key = policy.serviceProviderCode + policy.policyName;

                if (!htPolicys.ContainsKey(key))
                {
                    groupedXPolicy = new ArrayList();
                    groupedXPolicy.Add(policy);

                    htPolicys.Add(key, groupedXPolicy);
                }
                else
                {
                    (htPolicys[key] as ArrayList).Add(policy);
                }
            }

            return htPolicys;
        }

        /// <summary>
        /// Get EPayment policy model
        /// </summary>
        /// <returns>a XPolicyModel</returns>
        public XPolicyModel GetEPaymentPolicyModel()
        {
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
        public XpolicyUserRolePrivilegeModel GetPolicy(string agencyCode, string policyName, string levelType, string levelData)
        {
            return null;
        }

        /// <summary>
        /// Method to get a policy from cache. Agency + PolicyName + levelType + levelData + data1 is the key,The value is a UserRolePrivilegeModel
        /// </summary>
        /// <param name="moduleName">Module Name</param>
        /// <param name="contactType">Contact Type, it be saved in policy data1 field</param>
        /// <returns>return a UserRolePrivilegeModel instance</returns>
        public UserRolePrivilegeModel GetPolicyByContactType(string moduleName, string contactType)
        {
            return null;
        }

        /// <summary>
        /// Get ACA policy models by category, this method will get policies by route: agency -&gt; global
        /// </summary>
        /// <param name="category">category name</param>
        /// <returns>a policy model array
        /// data1:category name
        /// data2:item value
        /// data3:not used
        /// data4:item key</returns>
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
        /// Get policy value by category, this method will get policies by given level type and level data and data4 value.
        /// </summary>
        /// <param name="category">category name</param>
        /// <param name="levelType">level type, includes: STANDARDDATA, AGENCY, MODULE</param>
        /// <param name="levelData">level data</param>
        /// <param name="data4">data 4.</param>
        /// <returns>a policy model array</returns>
        public string GetPolicyValueForData4AsKey(string category, string levelType, string levelData, string data4)
        {
            string xpolicyValue = string.Empty;

            List<XPolicyModel> policies = GetPolicyListByPolicyName(BizDomainConstant.STD_CAT_ACA_CONFIGS, AgencyCode);

            if (policies == null || policies.Count == 0)
            {
                return null;
            }

            IEnumerable<XPolicyModel> selectPolicies = SelectPoliciesForData4AsKey(policies, category, levelType, levelData, data4);

            if (selectPolicies != null && selectPolicies.Any())
            {
                xpolicyValue = GetValue(selectPolicies.ToArray());
            }

            return xpolicyValue;
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
        /// <param name="agencyCode">The agency code.</param>
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
        /// Get Policy List by Policy Name
        /// </summary>
        /// <param name="policyName">a policy name</param>
        /// <param name="agencyCode">The agency code.</param>
        /// <returns>a policy model array</returns>
        public List<XPolicyModel> GetPolicyListByPolicyName(string policyName, string agencyCode)
        {
            ICacheManager cacheManager = ObjectFactory.GetObject<ICacheManager>();
            Hashtable htXpolicies = cacheManager.GetCachedItem(AgencyCode, AgencyCode + ACAConstant.SPLIT_CHAR + I18nCultureUtil.AppendUserPreferredCultureFlag(CacheConstant.CACHE_KEY_ALL_POLICY) + "_Admin");

            if (htXpolicies.ContainsKey(policyName))
            {
                return htXpolicies[policyName] as List<XPolicyModel>;
            }

            return null;
        }

        /// <summary>
        /// Get policy models by name
        /// </summary>
        /// <param name="policyName">policy name</param>
        /// <returns>null array list</returns>
        public ArrayList GetPolicyModelsByName(string policyName)
        {
            return null;
        }

        /// <summary>
        /// Get value by key, this method will get policy by route: module -> agency -> global
        /// </summary>
        /// <param name="key">policy key</param>
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
        /// Get value by key, this method will get policy by route: agency -> global
        /// </summary>
        /// <param name="key">string key</param>
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
        /// <param name="key">string key</param>
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
        /// Get value by key, this method will get policy by route: agency -&gt; global
        /// </summary>
        /// <param name="key">string key</param>
        /// <returns>a policy model</returns>
        public string GetSuperAgencyValueByKey(string key)
        {
            return GetValueByKey(key);
        }

        /// <summary>
        /// Method to create or update a policy
        /// </summary>
        /// <param name="agencyCode">agency code</param>
        /// <param name="policy">the XPolicyUserRolePrivilegeModel4WS model</param>
        /// <param name="callerId">public user id</param>
        public void CreateOrUpdatePolicy(string agencyCode, XpolicyUserRolePrivilegeModel[] policy, string callerId)
        {
            XPolicyService.createOrEditRolePolicy(AgencyCode, policy, callerId, false);
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
             ACAUserType userType = this.User == null ? ACAUserType.Anonymous : ACAUserType.Registered;

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
        /// Create or update policy value for ACA admin
        /// </summary>
        /// <param name="xpolicyArray">XPolicy array</param>
        /// <param name="ignoreLanguage">is ignore language</param>
        void IXPolicyBll.CreateOrUpdatePolicy(XPolicyModel[] xpolicyArray, bool ignoreLanguage)
        {
            XPolicyService.createOrEditPolicy(AgencyCode, xpolicyArray, "ACA Admin", ignoreLanguage);
        }

        /// <summary>
        /// V360 can setting security for contact type. Remove contact type should remove contact type security.
        /// </summary>
        /// <param name="policyModels">policy models need be removed.</param>
        void IXPolicyBll.RemoveXPoliciesForContactTypeSecurity(XPolicyModel[] policyModels)
        {
            XPolicyService.removeXPoliciesForContactTypeSecurity(policyModels);
        }

        /// <summary>
        /// Clears the Policy cache.
        /// </summary>
        private void ClearXpolicyCache()
        {
            //.* means match all language.
            string key = AgencyCode + ACAConstant.SPLIT_CHAR + I18nCultureUtil.AppendCultureFlag(CacheConstant.CACHE_KEY_ALL_POLICY, ".*") + "_Admin";
            System.Text.RegularExpressions.Regex reg = new System.Text.RegularExpressions.Regex(key);
            ArrayList keys = new ArrayList();

            foreach (DictionaryEntry de in HttpRuntime.Cache)
            {
                if (reg.IsMatch(de.Key.ToString()))
                {
                    keys.Add(de.Key.ToString());
                }
            }

            foreach (string k in keys)
            {
                HttpRuntime.Cache.Remove(k);
            }
        }

        /// <summary>
        /// get union policy list.
        /// </summary>
        /// <param name="ps">source IEnumerable of XPolicyModel</param>
        /// <param name="gps">target IEnumerable of XPolicyModel</param>
        /// <returns>an array of XPolicyModel</returns>
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

            if (gps == null || !gps.Any())
            {
                return ps.ToArray();
            }

            //merge module/agency level and global level data
            return ps.Union(gps, new PolicyComparer()).ToArray();
        }

        /// <summary>
        /// Union search type items.
        /// </summary>
        /// <param name="globalData">the global level Policy</param>
        /// <param name="agencyData">the agency level Policy</param>
        /// <returns>union search type list</returns>
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
        /// Get policy value
        /// </summary>
        /// <param name="ps">IEnumerable of XPolicyModel</param>
        /// <returns>string of policy value</returns>
        private string GetValue(IEnumerable<XPolicyModel> ps)
        {
            if (ps != null && ps.Any())
            {
                XPolicyModel model = ps.ElementAt(0);
                return string.IsNullOrEmpty(model.dispData2) ? model.data2 : model.dispData2;
            }

            return null;
        }

        /// <summary>
        /// Select policies by category
        /// </summary>
        /// <param name="policies">list of XPolicyModel</param>
        /// <param name="category">category name</param>
        /// <param name="levelType">level type</param>
        /// <param name="levelData">level data</param>
        /// <returns>IEnumerable of XPolicyModel</returns>
        private IEnumerable<XPolicyModel> SelectPoliciesByCategory(List<XPolicyModel> policies, string category, string levelType, string levelData)
        {
            return from p in policies where p.data1 == category && p.level == levelType && p.levelData == levelData select p;
        }

        /// <summary>
        /// Select policies by key
        /// </summary>
        /// <param name="policies">list of XPolicyModel</param>
        /// <param name="key">string key</param>
        /// <param name="levelType">level type</param>
        /// <param name="levelData">level data</param>
        /// <returns>IEnumerable of XPolicyModel</returns>
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
        /// <param name="postionID">position ID.</param>
        /// <returns>XPolicy model</returns>
        private IEnumerable<XPolicyModel> SelectPoliciesForData4AsKey(List<XPolicyModel> policies, string key, string levelType, string levelData, string postionID)
        {
            return from p in policies where p.data1 == key && p.data4 == postionID && p.level == levelType && p.levelData == levelData select p;
        }

        /// <summary>
        /// Set policy names
        /// </summary>
        /// <param name="policies">XPolicyModel array</param>
        private void SetPolicyName(XPolicyModel[] policies)
        {
            if (policies != null && policies.Length > 0)
            {
                foreach (XPolicyModel policy in policies)
                {
                    //Module level contact type settings not in category "ACA_CONFIGS" .
                    if (XPolicyConstant.CONTACT_TYPE_RESTRICTION_BY_MODULE.Equals(policy.policyName))
                    {
                        continue;
                    }

                    policy.policyName = BizDomainConstant.STD_CAT_ACA_CONFIGS;
                }
            }
        }

        #endregion Methods

        #region Nested Types

        /// <summary>
        /// class of policy comparer
        /// </summary>
        private class PolicyComparer : IEqualityComparer<XPolicyModel>
        {
            #region Methods

            /// <summary>
            /// Compare two XPolicyModel
            /// </summary>
            /// <param name="x">a XPolicyModel</param>
            /// <param name="y">other XPolicyModel</param>
            /// <returns>true means the one model equal the other, false means not</returns>
            public bool Equals(XPolicyModel x, XPolicyModel y)
            {
                return x.data4 == y.data4;
            }

            /// <summary>
            /// Get hash code
            /// </summary>
            /// <param name="obj">a XPolicyModel</param>
            /// <returns>hash code value.</returns>
            public int GetHashCode(XPolicyModel obj)
            {
                return obj.GetHashCode();
            }

            #endregion Methods
        }

        #endregion Nested Types
    }
}