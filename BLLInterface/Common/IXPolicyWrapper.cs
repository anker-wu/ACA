#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: IXPolicyWrapper.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2008-2014
 *
 *  Description:
 *
 *  Notes:
 *      $Id: IXPolicyWrapper.cs 277004 2014-08-09 07:44:41Z ACHIEVO\james.shi $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using System.Collections.Generic;

using Accela.ACA.Common;
using Accela.ACA.WSProxy;

namespace Accela.ACA.BLL.Common
{
    /// <summary>
    /// This interface defined method are main actions of policy table.
    /// </summary>
    public interface IXPolicyWrapper
    {
        #region Methods

        /// <summary>
        /// Method to create or update a set of policies
        /// </summary>
        /// <param name="agencyCode">agency code </param>
        /// <param name="policys">The are XPolicyUserRolePrivilegeModel4WS models</param>
        /// <param name="callerId">public user id</param>
        void CreateOrUpdatePolicy(string agencyCode, XpolicyUserRolePrivilegeModel[] policys, string callerId);

        /// <summary> 
        ///  Method to get a single policy. The value is a XPolicyUserRolePrivilegeModel
        /// </summary>
        /// <param name="agencyCode">current agency code</param>
        /// <param name="policyName">the name of policy such as ACA_CAP_SEARCH_USER_ROLES</param>
        /// <param name="levelType">indicates whether the policy is in a module level or agency level or global level</param>
        /// <param name="levelData">If module level, the module name. if agency level the agency code</param>
        /// <returns>return a XPolicyUserRolePrivilegeModel4WS instance</returns>
        XpolicyUserRolePrivilegeModel GetPolicy(string agencyCode, string policyName, string levelType, string levelData);

        /// <summary>
        /// Get matched roles for contact type. The condition is mark up of 
        ///  Agency + PolicyName + levelType + levelData+data1.
        /// </summary>
        ///  <param name="agencyCode">current agency code</param>
        /// <param name="moduleName">module name. It will be saved in level Data field</param>
        /// <param name="contactTypes">contact type list</param>
        /// <returns>Return data format is IDictionary data collection.The key of dictionary is contact type name</returns>
        IDictionary<string, UserRolePrivilegeModel> GetSelectedContactRoles(string agencyCode, string moduleName, IList<ItemValue> contactTypes);

        #endregion Methods
    }
}