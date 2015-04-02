#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: IUerRoleBll.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2010-2014
 *
 *  Description:
 *
 *  Notes:
 * $Id: IUserRoleBll.cs 277004 2014-08-09 07:44:41Z ACHIEVO\james.shi $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using Accela.ACA.WSProxy;

namespace Accela.ACA.BLL.Common
{
    /// <summary>
    /// An interface that defines all approach to get user role related value.
    /// </summary>
    public interface IUserRoleBll
    {
        /// <summary>
        /// Convert a user role string to UserRolePrivilegeModel model
        /// </summary>
        /// <param name="userRole">a 5-bit string</param>
        /// <returns>User role privilege object</returns>
        UserRolePrivilegeModel ConvertToUserRolePrivilegeModel(string userRole);

        /// <summary>
        /// Convert UserRolePrivilegeModel model to a string
        /// </summary>
        /// <param name="userPrivilegeModel">UserRolePrivilegeModel object</param>
        /// <returns>string for user role privilege</returns>
        string ConvertToUserRoleString(UserRolePrivilegeModel userPrivilegeModel);

        /// <summary>
        /// Determine whether current public user has right to process CAP Type activities
        /// </summary>
        /// <param name="userRole">a UserRolePrivilegeModel model which contains information about what user role has right</param>
        /// <param name="isModuleLevel">Is the cap type use module level role settings.</param>
        /// <returns>a boolean value indicates whether current public user has right</returns>
        bool IsCapTypeHasRight(UserRolePrivilegeModel userRole, bool isModuleLevel);

        /// <summary>
        /// return right for default role
        /// </summary>
        /// <returns>is has right for default role</returns>
        UserRolePrivilegeModel GetDefaultRole();

        /// <summary>
        /// Judge cap contact model is set full access permission or not.
        /// </summary>
        /// <param name="recordModel">The record model.</param>
        /// <param name="userRole">The user role.</param>
        /// <param name="user">The user model.</param>
        /// <returns>has full access or not</returns>
        bool HasFullAccess(CapModel4WS recordModel, UserRolePrivilegeModel userRole, PublicUserModel4WS user);

        ///// <summary>
        ///// Judege capcontact model is set schedule inspection and read only permission or not.
        ///// </summary>
        ///// <param name="recordModel">The record model.</param>
        ///// <param name="userRolePrivilegeModel">The user role privilege model.</param>
        ///// <returns>has schedule inspection and read only</returns>
        //bool HasSchedulePermission(CapModel4WS recordModel, UserRolePrivilegeModel userRolePrivilegeModel, PublicUserModel4WS user);

        /// <summary>
        /// Judge cap contact model is set read only permission or not.
        /// </summary>
        /// <param name="recordModel">The record model.</param>
        /// <param name="userRole">The user role.</param>
        /// <param name="user">The user model.</param>
        /// <returns>has read only or not</returns>
        bool HasReadOnlyPermission(CapModel4WS recordModel, UserRolePrivilegeModel userRole, PublicUserModel4WS user);

        /// <summary>
        /// determine whether the license of a CAP is associated with current public user
        /// </summary>
        /// <param name="recordModel">The record model.</param>
        /// <param name="userRole">a UserRolePrivilegeModel model</param>
        /// <param name="user">The user model.</param>
        /// <returns>
        /// true if license of a CAP is associated with current public user; otherwise,false.
        /// </returns>
        bool IsLicensedProfessional(CapModel4WS recordModel, UserRolePrivilegeModel userRole, PublicUserModel4WS user);

        /// <summary>
        /// determine whether a CAP is created by current public user
        /// </summary>
        /// <param name="recordModel">The record model.</param>
        /// <param name="user">The user model.</param>
        /// <returns>
        /// true if the CAP is created by the current user; otherwise,false.
        /// </returns>
        bool IsCAPCreator(CapModel4WS recordModel, PublicUserModel4WS user);

         /// <summary>
        /// determine whether the owner of a CAP is associated with current public user
        /// </summary>
        /// <param name="recordModel">The record model.</param>
        /// <param name="user">The user model.</param>
        /// <returns>
        /// true if the owner of a CAP is associated with current public user; otherwise,false.
        /// </returns>
        bool IsOwner(CapModel4WS recordModel, PublicUserModel4WS user);

        /// <summary>
        /// Judge cap contact model is set schedule inspection and read only permission or not.
        /// </summary>
        /// <param name="recordModel">The record model.</param>
        /// <param name="userRolePrivilegeModel">The user role privilege model.</param>
        /// <param name="user">The public user</param>
        /// <param name="contactPermissionType">specific contact permission type</param>
        /// <param name="ignoreContactAccessLevel">ignore contact access level right that set at contact section of spear form</param>
        /// <returns>has schedule inspection and read only</returns>
        bool HasPermission(CapModel4WS recordModel, UserRolePrivilegeModel userRolePrivilegeModel, PublicUserModel4WS user, string contactPermissionType, bool ignoreContactAccessLevel = false);

        /// <summary>
        /// Get Record Search Role
        /// </summary>
        /// <param name="agencyCode">current agency code</param>
        /// <param name="moduleName">the module name</param>
        /// <param name="capType">Type of the cap.</param>
        /// <returns>the user role privilege model.</returns>
        UserRolePrivilegeModel GetRecordSearchRole(string agencyCode, string moduleName, CapTypeModel capType);
    }
}
