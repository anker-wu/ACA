#region Header

/*
 * <pre>
 *
 *  Accela Citizen Access
 *  File: XPolicyWrapper.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2007-2014
 *
 *  Description:
 *
 *  Notes:
 *      $Id: XPolicyWrapper.cs 277080 2014-08-11 06:28:07Z ACHIEVO\james.shi $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using System.Collections;
using System.Collections.Generic;

using Accela.ACA.BLL.Common;
using Accela.ACA.Common;
using Accela.ACA.WSProxy;

namespace Accela.ACA.AdminBLL
{
    /// <summary>
    /// This class is operation XPolicy wrapper.
    /// </summary>
    public class XPolicyWrapper : IXPolicyWrapper
    {
        #region Fields

        /// <summary>
        /// Hashtable for policy.
        /// </summary>
        private Hashtable _htXPolicys;

        #endregion Fields

        #region Methods

        /// <summary>
        /// Method to create or update a set of policies
        /// </summary>
        /// <param name="agencyCode">agency code</param>
        /// <param name="policys">The are XPolicyUserRolePrivilegeModel4WS models</param>
        /// <param name="callerId">public user id</param>
        public void CreateOrUpdatePolicy(string agencyCode, XpolicyUserRolePrivilegeModel[] policys, string callerId)
        {
            new XPolicyAdminBll().CreateOrUpdatePolicy(agencyCode, policys, callerId);
        }

        /// <summary>
        /// Method to get a single policy. The value is a XPolicyUserRolePrivilegeModel
        /// </summary>
        /// <param name="agencyCode">current agency code</param>
        /// <param name="policyName">the name of policy such as ACA_CAP_SEARCH_USER_ROLES</param>
        /// <param name="levelType">indicates whether the policy is in a module level or agency level or global level</param>
        /// <param name="levelData">If module level, the module name. if agency level the agency code</param>
        /// <returns>return a XPolicyUserRolePrivilegeModel4WS instance</returns>
        public XpolicyUserRolePrivilegeModel GetPolicy(string agencyCode, string policyName, string levelType, string levelData)
        {
            InitXPolicy(agencyCode);

            string key = agencyCode + policyName;

            if (_htXPolicys.ContainsKey(key) && _htXPolicys[key] != null)
            {
                ArrayList policys = _htXPolicys[key] as ArrayList;

                foreach (XpolicyUserRolePrivilegeModel policy in policys)
                {
                    if (policy.level.Equals(levelType) && policy.levelData.Equals(levelData))
                    {
                        return policy;
                    }
                }
            }

            return null;
        }

        /// <summary>
        /// Get matched roles for contact type. The condition is mark up of
        /// Agency + PolicyName + levelType + levelData+data1.
        /// </summary>
        /// <param name="agencyCode">current agency code</param>
        /// <param name="moduleName">module name. It will be saved in level Data field</param>
        /// <param name="contactTypes">contact type list</param>
        /// <returns>Return data format is IDictionary data collection.The key of dictionary is contact type name</returns>
        public IDictionary<string, UserRolePrivilegeModel> GetSelectedContactRoles(string agencyCode, string moduleName, IList<ItemValue> contactTypes)
        {
            IDictionary<string, UserRolePrivilegeModel> contactTypeRoles = new Dictionary<string, UserRolePrivilegeModel>();
            InitXPolicy(agencyCode);
            string key = agencyCode + ACAConstant.ACA_CONTACT_TYPE_USER_ROLES;

            if (_htXPolicys.ContainsKey(key) && _htXPolicys[key] != null)
            {
                ArrayList policys = _htXPolicys[key] as ArrayList;

                foreach (XpolicyUserRolePrivilegeModel policy in policys)
                {
                    if (policy.level.Equals(ACAConstant.LEVEL_TYPE_MODULE) && policy.levelData.Equals(moduleName))
                    {
                        // if there exists duplicate key, use the latest one.
                        contactTypeRoles[policy.data1] = policy.userRolePrivilegeModel;
                    }
                }
            }

            //if some contact types is new record then set all aca user to default value .
            foreach (ItemValue contactType in contactTypes)
            {
                if (!contactTypeRoles.ContainsKey(contactType.Key))
                {
                    UserRolePrivilegeModel tempRole = new UserRolePrivilegeModel();
                    tempRole.allAcaUserAllowed = true;
                    contactTypeRoles.Add(contactType.Key, tempRole);
                }
            }

            return contactTypeRoles;
        }

        /// <summary>
        /// Initial Policy.
        /// </summary>
        /// <param name="agencyCode">The agency code.</param>
        private void InitXPolicy(string agencyCode)
        {
            if (_htXPolicys == null)
            {
                _htXPolicys = new XPolicyAdminBll().GetAllACAPolicys(agencyCode);
            }
        }

        #endregion Methods
    }
}