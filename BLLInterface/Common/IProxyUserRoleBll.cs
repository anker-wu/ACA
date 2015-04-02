#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: IProxyUserRoleBll.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2010-2014
 *
 *  Description:
 *
 *  Notes:
 * $Id: IProxyUserRoleBll.cs 277008 2014-08-09 08:37:50Z ACHIEVO\james.shi $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using Accela.ACA.Common;
using Accela.ACA.WSProxy;

namespace Accela.ACA.BLL.Common
{
    /// <summary>
    /// An interface that defines all approach to get user role related value.
    /// </summary>
    public interface IProxyUserRoleBll
    {
        /// <summary>
        /// Convert a user role string to proxy user role privilege model
        /// </summary>
        /// <param name="userRole">a 5-bit string</param>
        /// <returns>ProxyUserRolePrivilegeModel4WS object</returns>
        ProxyUserRolePrivilegeModel4WS ConvertToUserRolePrivilegeModel(string userRole);

        /// <summary>
        /// Convert user role privilege object to a string
        /// </summary>
        /// <param name="proxyUserRolePrivilege">proxyUserRolePrivilege object</param>
        /// <returns>string for user role privilege</returns>
        string ConvertToUserRoleString(ProxyUserRolePrivilegeModel4WS proxyUserRolePrivilege);

        /// <summary>
        /// return right for default role
        /// </summary>
        /// <param name="permissionType">the permission type object</param>
        /// <param name="role">The role.</param>
        /// <returns>is has right for default role</returns>
        bool IsPermissionTypeHasPermission(ProxyPermissionType permissionType, string role);

        /// <summary>
        /// judge cap contact model is set schedule inspection and read only permission or not.
        /// </summary>
        /// <param name="recordModel">The record model.</param>
        /// <param name="userRole">The user role.</param>
        /// <param name="permissionType">The permission type.</param>
        /// <param name="ignoreContactAccessLevel">ignore contact access level right that set at contact section of spear form</param>
        /// <returns>has schedule inspection and read only</returns>
        bool HasPermission(CapModel4WS recordModel, UserRolePrivilegeModel userRole, ProxyPermissionType permissionType, bool ignoreContactAccessLevel = false);

        /// <summary>
        /// judge cap contact model is set schedule inspection and read only permission or not.
        /// </summary>
        /// <param name="recordModel">The record model.</param>
        /// <param name="userRole">the user Role</param>
        /// <returns>has schedule inspection and read only</returns>
        bool HasReadOnlyPermission(CapModel4WS recordModel, UserRolePrivilegeModel userRole);

        /// <summary>
        /// Indicates whether enable proxy user.
        /// </summary>
        /// <returns>true means proxy user is enabled, else not</returns>
        bool IsEnableProxyUser();

        /// <summary>
        /// Get auto fill data.
        /// </summary>
        /// <param name="recordModel">the record Model.</param>
        /// <returns>public user model.</returns>
        PublicUserModel4WS GetCurrentUser(CapModel4WS recordModel);

        /// <summary>
        /// judge cap contact model is set schedule inspection and read only permission or not.
        /// </summary>
        /// <param name="recordModel">The record model.</param>
        /// <param name="userRole">The user role.</param>
        /// <returns><c>true</c> if [has schedule permission] [the specified record model]; otherwise, <c>false</c>.</returns>
        bool HasSchedulePermission(CapModel4WS recordModel, UserRolePrivilegeModel userRole);
    }
}
