#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: IXPolicyBll.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2008-2014
 *
 *  Description:
 *
 *  Notes:
 *      $Id: IXPolicyBll.cs 277004 2014-08-09 07:44:41Z ACHIEVO\james.shi $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using System.Collections;
using System.Collections.Generic;

using Accela.ACA.WSProxy;

namespace Accela.ACA.BLL.Common
{
    /// <summary>
    /// interface of policy 
    /// </summary>
    public interface IXPolicyBll
    {
        #region Methods

        /// <summary>
        ///  Method to create or update a policy
        /// </summary>
        /// <param name="agencyCode">agency code</param>
        /// <param name="policy">the XPolicyUserRolePrivilegeModel4WS model</param>
        /// <param name="callerId">public user id</param>
        void CreateOrUpdatePolicy(string agencyCode, XpolicyUserRolePrivilegeModel[] policy, string callerId);

        /// <summary>
        /// Create or update policy value for ACA admin
        /// </summary>
        /// <param name="xpolicyArray">XPolicy array</param>
        /// <param name="ignoreLanguage">is ignore language</param>
        void CreateOrUpdatePolicy(XPolicyModel[] xpolicyArray, bool ignoreLanguage);

        /// <summary>
        /// Create or update policy
        /// </summary>
        /// <param name="policiesWithLan">the models which need support multiple language</param>
        /// <param name="policiesWithoutLan">the models which ignore multiple language</param>
        void CreateOrUpdatePolicy(XPolicyModel[] policiesWithLan, XPolicyModel[] policiesWithoutLan);

        /// <summary>
        /// Create or update policy as data3 is key 
        /// </summary>
        /// <param name="policiesWithLan">XPolicyModel array</param>
        /// <param name="policiesWithoutLan">XPolicyModel array without language</param>
        void CreateOrUpdateXPolicyForData3AsKey(XPolicyModel[] policiesWithLan, XPolicyModel[] policiesWithoutLan);

        /// <summary>
        /// this method get all ACA policy for an agency
        /// </summary>
        /// <param name="agencyCode">agency code</param>
        /// <returns>return an array of XPolicyUserRolePrivilegeModel4WS instances</returns>
        Hashtable GetAllACAPolicys(string agencyCode);

        /// <summary>
        /// Get EPayment policy model
        /// </summary>
        /// <returns>a XPolicyModel</returns>
        XPolicyModel GetEPaymentPolicyModel();

        /// <summary> 
        ///  Method to get a policy from cache. Agency + PolicyName + levelType + levelData is the key,The value is a XPolicyUserRolePrivilegeModel
        /// </summary>
        /// <param name="agencyCode">current agency code</param>
        /// <param name="policyName">the name of policy such as ACA_CAP_SEARCH_USER_ROLES</param>
        /// <param name="levelType">indicates whether the policy is in a module level or agency level or global level</param>
        /// <param name="levelData">If module level, the module name. if agency level the agency code</param>
        /// <returns>return an array of XPolicyUserRolePrivilegeModel4WS instances</returns>
        XpolicyUserRolePrivilegeModel GetPolicy(string agencyCode, string policyName, string levelType, string levelData);

        /// <summary> 
        ///  Method to get a policy from cache. Agency + PolicyName + levelType + levelData + data1 is the key,The value is a UserRolePrivilegeModel
        /// </summary>
        /// <param name="moduleName">Module Name</param>
        /// <param name="contactType">Contact Type, it be saved in policy data1 field</param>
        /// <returns>return a UserRolePrivilegeModel instance</returns>
        UserRolePrivilegeModel GetPolicyByContactType(string moduleName, string contactType);

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
        XPolicyModel[] GetPolicyListByCategory(string category);

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
        XPolicyModel[] GetPolicyListByCategory(string category, string moduleName);

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
        XPolicyModel[] GetPolicyListByCategory(string category, string levelType, string levelData);

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
        XPolicyModel[] GetPolicyListByCategory(string agencyCode, string category, string levelType, string levelData);

        /// <summary>
        /// Get policy value by category, this method will get policies by given level type and level data and data4 value.
        /// </summary>
        /// <param name="category">category name</param>
        /// <param name="levelType">level type, includes: STANDARDDATA, AGENCY, MODULE</param>
        /// <param name="levelData">level data</param>
        /// <param name="data4">data 4.</param>
        /// <returns>a policy model array
        /// </returns>
        string GetPolicyValueForData4AsKey(string category, string levelType, string levelData, string data4);

        /// <summary>
        /// Get and merge search type items from Global and Agency level.
        /// </summary>
        /// <returns>
        /// a policy model array
        /// </returns>
        List<XPolicyModel> GetSearchTypeItems();

        /// <summary>
        /// Get Policy List by Policy Name
        /// </summary>
        /// <param name="policyName">a policy name</param>
        /// <param name="agencyCode">The agency code.</param>
        /// <returns>a policy model array</returns>
        List<XPolicyModel> GetPolicyListByPolicyName(string policyName, string agencyCode);

        /// <summary>
        /// Get policies by name
        /// </summary>
        /// <param name="policyName">string policy name</param>
        /// <returns>an ArrayList</returns>
        ArrayList GetPolicyModelsByName(string policyName);

        /// <summary>
        /// Get value by key, this method will get policy by route: module -> agency -> global
        /// </summary>
        /// <param name="key">string key</param>
        /// <param name="moduleName">Module Name</param>
        /// <returns>a policy model</returns>
        string GetValueByKey(string key, string moduleName);

        /// <summary>
        /// Get value by key, this method will get policy by route: agency -> global
        /// </summary>
        /// <param name="key">string key</param>
        /// <returns>a policy model</returns>
        string GetValueByKey(string key);

        /// <summary>
        /// Get value by key, this method will get policy by route: agency -> global
        /// </summary>
        /// <param name="key">string key</param>
        /// <returns>a policy model</returns>
        string GetSuperAgencyValueByKey(string key);

        /// <summary>
        /// get value by given key, level type and level data. this method will not go through parent level
        /// </summary>
        /// <param name="key">string key</param>
        /// <param name="levelType">level type, includes: STANDARDDATA, AGENCY, MODULE</param>
        /// <param name="levelData">level data</param>
        /// <returns>a policy model</returns>
        string GetValueByKey(string key, string levelType, string levelData);

        /// <summary>
        /// Get policy array by policy level.
        /// </summary>
        /// <param name="policyName">policy name</param>
        /// <param name="level">level type</param>
        /// <param name="levelData">level data</param>
        /// <param name="callerID">string callerID</param>
        /// <returns>Policy array</returns>
        XPolicyModel[] GetXPolicyByLevel(string policyName, string level, string levelData, string callerID);

        /// <summary>
        /// Get enabled search cross modules from policy.
        /// </summary>
        /// <param name="currentModuleName">current module name</param>
        /// <returns>name of modules</returns>
        List<string> GetCrossModulesFromXPolicy(string currentModuleName);

        /// <summary>
        /// V360 can setting security for contact type. Remove contact type should remove contact type security.
        /// </summary>
        /// <param name="policyModels">policy models need be removed.</param>
        void RemoveXPoliciesForContactTypeSecurity(XPolicyModel[] policyModels);    

        #endregion Methods
    }
}