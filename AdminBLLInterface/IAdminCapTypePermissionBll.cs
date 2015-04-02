#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: IAdminCapTypePermissionBll.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2009-2010
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

using System.Collections;

using Accela.ACA.WSProxy;
using Accela.ACA.WSProxy.WSModel;

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
        CapTypePermissionModel4WS[] GetCapTypePermissions(CapTypePermissionModel4WS capTypePermission);
               
        /// <summary>
        /// Save Cap Type Permission List.
        /// </summary>
        /// <param name="capTypePermissions">the Cap Type Permission List.</param>
        void SaveCapTypePermissions(CapTypePermissionModel4WS[] capTypePermissions);

        /// <summary>
        /// Get Cap Type Permission List.
        /// </summary>
        /// <param name="moduleName">the module Name</param>
        /// <param name="policy">the XpolicyUserRolePrivilegeModel4WS model.</param>
        /// <returns>Cap Type jason string</returns>
        string GetCapTypeRoles(string moduleName, XpolicyUserRolePrivilegeModel4WS policy);
 
        /// <summary>
        /// Delte Cap type permissions.
        /// </summary>
        /// <param name="capTypePermission">the capTypePermission model.</param>
        void DeleteCapTypePermissions(CapTypePermissionModel4WS capTypePermission);

        /// <summary>
        /// Get CAP type permission for button setting.
        /// </summary>
        /// <param name="buttonSettingModel">button Setting Model.</param>
        /// <param name="callerID">caller ID number.</param> 
        /// <returns>Button Setting Model4WS</returns>
        ButtonSettingModel4WS GetButtonSetting4CapType(ButtonSettingModel4WS buttonSettingModel, string callerID);

        /// <summary>
        /// Update CAP type permissions for button setting.
        /// </summary>
        /// <param name="buttonSetting">ButtonSettingModel4WS obj</param>
        /// <param name="capTypePermissions">array of CapTypePermissionModel4WS</param>
        void UpdateButtonSetting4CapType(ButtonSettingModel4WS buttonSetting, CapTypePermissionModel4WS[] capTypePermissions);

        /// <summary>
        /// Get Cap type permission list.
        /// </summary>
        /// <param name="buttonName">button name</param>
        /// <param name="capTypes">CapTypeModel array</param>
        /// <returns>CapTypePermissionModel4WS array</returns>
        CapTypePermissionModel4WS[] GetCapTypePermission4ButtonSetting(string buttonName, CapTypeModel[] capTypes);

        #endregion Methods
    }
}