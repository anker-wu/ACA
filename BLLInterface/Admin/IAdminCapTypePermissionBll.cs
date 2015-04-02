#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: IAdminCapTypePermissionBll.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2010-2014
 *
 *  Description:
 *  Interface define for ICapTypePermissionBll.
 *
 *  Notes:
 * $Id: IAdminCapTypePermissionBll.cs 178037 2010-07-30 06:25:12Z ACHIEVO\daly.zeng $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using Accela.ACA.WSProxy;

namespace Accela.ACA.AdminBLL
{
    /// <summary>
    /// Defines many methods for Cap type permission.
    /// </summary>
    public interface IAdminCapTypePermissionBll
    {
        #region Methods

        /// <summary>
        /// Get Cap Type Permission List.
        /// </summary>
        /// <param name="capTypePermission">the capType Permission Model</param>
        /// <returns>the CapTypePermission Model list.</returns>
        CapTypePermissionModel[] GetCapTypePermissions(CapTypePermissionModel capTypePermission);
               
        /// <summary>
        /// Save Cap Type Permission List.
        /// </summary>
        /// <param name="capTypePermissions">the Cap Type Permission List.</param>
        void SaveCapTypePermissions(CapTypePermissionModel[] capTypePermissions);

        /// <summary>
        /// Get Cap Type Permission List.
        /// </summary>
        /// <param name="moduleName">the module Name</param>
        /// <param name="policy">the policy of User Role Privilege.</param>
        /// <returns>Cap Type jason string</returns>
        string GetCapTypeRoles(string moduleName, XpolicyUserRolePrivilegeModel policy);
 
        /// <summary>
        /// Delete Cap type permissions.
        /// </summary>
        /// <param name="capTypePermission">the cap Type Permission.</param>
        void DeleteCapTypePermissions(CapTypePermissionModel capTypePermission);

        /// <summary>
        /// Get CAP type permission for amendment button setting.
        /// </summary>
        /// <param name="buttonSettingModel">button Setting Model.</param>
        /// <param name="callerID">caller ID number.</param> 
        /// <returns>amendment button settings by cap types. </returns>
        ButtonSettingModel4WS GetButtonSetting4CapType(ButtonSettingModel4WS buttonSettingModel, string callerID);

        /// <summary>
        /// Update CAP type permissions for button setting.
        /// </summary>
        /// <param name="buttonSetting">amendment button settings object</param>
        /// <param name="capTypePermissions">array of cap type permission</param>
        void UpdateButtonSetting4CapType(ButtonSettingModel4WS buttonSetting, CapTypePermissionModel[] capTypePermissions);

        /// <summary>
        /// Get Cap type permission list.
        /// </summary>
        /// <param name="buttonName">button name</param>
        /// <param name="capTypes">CapTypeModel array</param>
        /// <returns>CapTypePermissionModel array</returns>
        CapTypePermissionModel[] GetCapTypePermission4ButtonSetting(string buttonName, CapTypeModel[] capTypes);

        #endregion Methods
    }
}