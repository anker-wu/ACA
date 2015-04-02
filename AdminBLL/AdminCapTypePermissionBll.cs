#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: AdminCapTypePermissionBll.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2009-2014
 *
 *  Description:
 *
 *
 *  Notes:
 * $Id: AdminCapTypePermissionBll.cs 277080 2014-08-11 06:28:07Z ACHIEVO\james.shi $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using System;
using System.Text;

using Accela.ACA.Common;
using Accela.ACA.Common.Common;
using Accela.ACA.Common.Log;
using Accela.ACA.WSProxy;

using log4net;

namespace Accela.ACA.AdminBLL
{
    /// <summary>
    /// This class is operation cap type Permission in ACA admin. 
    /// </summary>
    public class AdminCapTypePermissionBll : BaseBll, IAdminCapTypePermissionBll
    {
        #region Fields

        /// <summary>
        /// Module level string display in select cap types.
        /// </summary>
        private const string ModuleLevelCapTypeName = "All Record Types in the module";

        /// <summary>
        /// Logger object.
        /// </summary>
        private static readonly ILog Logger = LogFactory.Instance.GetLogger(typeof(AdminCapTypeFilterBll));

        #endregion Fields

        #region Properties

        /// <summary>
        /// Gets an instance of CapTypePermissionService.
        /// </summary>
        private CapTypePermissionWebServiceService CapTypePermissionService
        {
            get
            {
                return WSFactory.Instance.GetWebService<CapTypePermissionWebServiceService>();
            }
        }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Get Cap Type Permission List.
        /// </summary>
        /// <param name="capTypePermission">the capType Permission Model</param>
        /// <returns>the CapTypePermission Model list.</returns>
        /// <exception cref="ACAException">Exception from web service.</exception>
        public CapTypePermissionModel[] GetCapTypePermissions(CapTypePermissionModel capTypePermission)
        {
            if (Logger.IsDebugEnabled)
            {
                Logger.Debug("Begin GetCapTypePermissions");
            }

            try
            {
                CapTypePermissionModel[] capTypePermissionModels = CapTypePermissionService.getCapTypePermissions(capTypePermission);

                if (Logger.IsDebugEnabled)
                {
                    Logger.Debug("End GetCapTypePermissions");
                }

                return capTypePermissionModels;
            }
            catch (Exception e)
            {
                throw new ACAException(e);
            }
        }

        /// <summary>
        /// Get Cap Type Permission List.
        /// </summary>
        /// <param name="moduleName">the module Name</param>
        /// <param name="policy">the policy of User Role Privilege.</param>
        /// <returns>Cap Type jason string</returns>
        /// <exception cref="ACAException">Exception from web service.</exception>
        public string GetCapTypeRoles(string moduleName, XpolicyUserRolePrivilegeModel policy)
        {
            if (Logger.IsDebugEnabled)
            {
                Logger.Debug("Begin GetCapTypeRoles");
            }

            try
            {
                string searchLevel = policy == null || !ACAConstant.COMMON_ONE.Equals(policy.data4) ? "var isModuleLevel = true;" : "var isModuleLevel = false;";
                string userRoleList = ConvertToUserRoleString(GetDefaultUserRolePrivilegeModel(policy)).PadRight(10, '0');
                string moduleLevelRole = FormatUserRole(userRoleList);

                StringBuilder roleDatas = new StringBuilder();

                roleDatas.Append(searchLevel);
                roleDatas.Append("var ModuleLevelCapTypeRoleList=[");
                roleDatas.Append("[\"-1\",");
                roleDatas.Append("\"" + ModuleLevelCapTypeName + "\",");
                roleDatas.Append(moduleLevelRole).Append("]];");
                roleDatas.Append(BuilderCapTypeLevelRoles(moduleName));

                if (Logger.IsDebugEnabled)
                {
                    Logger.Debug("End GetCapTypeRoles");
                }

                return roleDatas.ToString();
            }
            catch (Exception e)
            {
                throw new ACAException(e);
            }
        }

        /// <summary>
        /// Convert UserRolePrivilegeModel model to a string
        /// </summary>
        /// <param name="userPrivilegeModel">UserRolePrivilegeModel object</param>
        /// <returns>string for user role privilege</returns>
        public string ConvertToUserRoleString(UserRolePrivilegeModel userPrivilegeModel)
        {
            StringBuilder userTypes = new StringBuilder();

            userTypes.Append(userPrivilegeModel.allAcaUserAllowed ? "1" : "0");
            userTypes.Append(userPrivilegeModel.capCreatorAllowed ? "1" : "0");
            userTypes.Append(userPrivilegeModel.licensendProfessionalAllowed ? "1" : "0");
            userTypes.Append(userPrivilegeModel.contactAllowed ? "1" : "0");
            userTypes.Append(userPrivilegeModel.ownerAllowed ? "1" : "0");
            userTypes.Append(userPrivilegeModel.registeredUserAllowed ? "1" : "0");

            return userTypes.ToString();
        }

        /// <summary>
        /// Saves the cap type permissions.
        /// </summary>
        /// <param name="capTypePermissionModels">The cap type permission models.</param>
        /// <exception cref="ACAException">Exception from web service.</exception>
        public void SaveCapTypePermissions(CapTypePermissionModel[] capTypePermissionModels)
        {
            if (Logger.IsDebugEnabled)
            {
                Logger.Debug("Begin SaveCapTypePermissions");
            }

            try
            {
                CapTypePermissionService.saveCapTypePermissions(capTypePermissionModels);

                if (Logger.IsDebugEnabled)
                {
                    Logger.Debug("End SaveCapTypePermissions");
                } 
            }
            catch (Exception e)
            {
                throw new ACAException(e);
            }
        }

        /// <summary>
        /// Delete Cap type permissions.
        /// </summary>
        /// <param name="capTypePermission">the cap Type Permission.</param>
        /// <exception cref="ACAException">Exception from web service.</exception>
        public void DeleteCapTypePermissions(CapTypePermissionModel capTypePermission)
        {
            if (Logger.IsDebugEnabled)
            {
                Logger.Debug("Begin DeleteCapTypePermissions");
            }

            try
            {
                CapTypePermissionService.deleteCapTypePermissions(capTypePermission);

                if (Logger.IsDebugEnabled)
                {
                    Logger.Debug("End DeleteCapTypePermissions");
                }
            }
            catch (Exception e)
            {
                throw new ACAException(e);
            }
        }

        /// <summary>
        /// Get CAP type permission for amendment button setting.
        /// </summary>
        /// <param name="buttonSettingModel">button Setting Model.</param>
        /// <param name="callerID">caller ID number.</param>
        /// <returns>amendment button settings by cap types.</returns>
        public ButtonSettingModel4WS GetButtonSetting4CapType(ButtonSettingModel4WS buttonSettingModel, string callerID)
        {
            ButtonSettingModel4WS response = CapTypePermissionService.getButtonSetting4CapType(buttonSettingModel, callerID);

            return response;
        }

        /// <summary>
        /// Update CAP type permissions for button setting.
        /// </summary>
        /// <param name="buttonSetting">amendment button settings object</param>
        /// <param name="capTypePermissions">array of cap type permission</param>
        public void UpdateButtonSetting4CapType(ButtonSettingModel4WS buttonSetting, CapTypePermissionModel[] capTypePermissions)
        {
            CapTypePermissionService.updateButtonSetting4CapType(AgencyCode, buttonSetting, capTypePermissions, ACAConstant.ADMIN_CALLER_ID);
        }

        /// <summary>
        /// Get Cap type permission list.
        /// </summary>
        /// <param name="buttonName">button name</param>
        /// <param name="capTypes">CapTypeModel array</param>
        /// <returns>CapTypePermissionModel array</returns>
        public CapTypePermissionModel[] GetCapTypePermission4ButtonSetting(string buttonName, CapTypeModel[] capTypes)
        {
            CapTypePermissionModel[] capTypePermissions = CapTypePermissionService.getCapTypePermission4ButtonSetting(AgencyCode, buttonName, capTypes);

            return capTypePermissions;
        }

        /// <summary>
        /// get Cap Type name.
        /// </summary>
        /// <param name="capTypePermissionModel">the capTypePermission Model</param>
        /// <returns>The Cap Type Name.</returns>
        private string GetCapTypeName(CapTypePermissionModel capTypePermissionModel)
        {
            string capTypeName = string.Empty;

            if (capTypePermissionModel != null)
            {
                capTypeName = string.Format("{0}{1}{2}{3}{4}{5}{6}", capTypePermissionModel.group, ACAConstant.SPLIT_CHAR, capTypePermissionModel.type, ACAConstant.SPLIT_CHAR, capTypePermissionModel.subType, ACAConstant.SPLIT_CHAR, capTypePermissionModel.category);
            }

            return capTypeName;
        }

        /// <summary>
        /// Get UserRolePrivilege Model.
        /// </summary>
        /// <param name="policy">The policy.</param>
        /// <returns>the UserRolePrivilege Model</returns>
        private UserRolePrivilegeModel GetDefaultUserRolePrivilegeModel(XpolicyUserRolePrivilegeModel policy)
        {
            UserRolePrivilegeModel userRoleModel = new UserRolePrivilegeModel();

            if (policy == null)
            {
                userRoleModel.allAcaUserAllowed = true;
            }
            else
            {
                userRoleModel = policy.userRolePrivilegeModel;
            }

            return userRoleModel;
        }

        /// <summary>
        /// Get The Format User Role .
        /// </summary>
        /// <param name="originalRole">the Original Role.</param>
        /// <returns>Format User Role.</returns>
        private string FormatUserRole(string originalRole)
        {
            StringBuilder destinctRole = new StringBuilder();

            if (string.IsNullOrEmpty(originalRole))
            {
                destinctRole.Append("1,1,1,1,1,1,0,0,0,0");
            }
            else
            {
                char[] originalRoleArray = originalRole.ToCharArray();
                foreach (char userRole in originalRoleArray)
                {
                    destinctRole.Append(userRole.ToString()).Append(",");
                }

                destinctRole.Remove(destinctRole.Length - 1, 1);
            }

            return destinctRole.ToString();
        }

        /// <summary>
        /// Builder Cap Type Level Role Jason String.
        /// </summary>
        /// <param name="moduleName">the module name</param>
        /// <returns>The Cap Type Level Role Jason String.</returns>
        private string BuilderCapTypeLevelRoles(string moduleName)
        {
            StringBuilder capTypeLevelRole = new StringBuilder();
            CapTypePermissionModel capTypePermissionModel = new CapTypePermissionModel();
            capTypePermissionModel.moduleName = moduleName;
            capTypePermissionModel.serviceProviderCode = AgencyCode;
            capTypePermissionModel.controllerType = ControllerType.CAPSEARCHFILTER.ToString();
            capTypePermissionModel.entityType = EntityType.GENERAL.ToString();
            CapTypePermissionModel[] capTypePermissionModels = CapTypePermissionService.getAllCapTypesWithPermission(capTypePermissionModel);

            capTypeLevelRole.Append("var CapTypeLevelRoleList=[");

            if (capTypePermissionModels != null && capTypePermissionModels.Length > 0)
            {
                foreach (CapTypePermissionModel model in capTypePermissionModels)
                {
                    string capTypeName = GetCapTypeName(model);
                    string capTypeAliasName = string.IsNullOrEmpty(model.alias) ? capTypeName.Replace(ACAConstant.SPLIT_CHAR.ToString(), ACAConstant.SLASH) : model.alias;
                    capTypeLevelRole.Append("[\"");
                    capTypeLevelRole.Append(ScriptFilter.EncodeJson(capTypeName));
                    capTypeLevelRole.Append("\",\"");
                    capTypeLevelRole.Append(ScriptFilter.EncodeJson(capTypeAliasName));
                    capTypeLevelRole.Append("\",");
                    capTypeLevelRole.Append(FormatUserRole(model.entityPermission));
                    capTypeLevelRole.Append("],");
                }

                capTypeLevelRole.Remove(capTypeLevelRole.Length - 1, 1);
            }

            capTypeLevelRole.Append("];");

            return capTypeLevelRole.ToString();
        }

        #endregion Methods
    }
}