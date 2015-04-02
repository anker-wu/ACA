#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: ProxyUserRoleBll.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2010-2014
 *
 *  Description:
 *  Interface define for admin.
 *
 *  Notes:
 * $Id: ProxyUserRoleBll.cs 278415 2014-09-03 08:45:04Z ACHIEVO\james.shi $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using System.Collections.Generic;
using System.Linq;
using System.Text;

using Accela.ACA.BLL.Cap;
using Accela.ACA.Common;
using Accela.ACA.Common.Util;
using Accela.ACA.WSProxy;

namespace Accela.ACA.BLL.Common
{
    /// <summary>
    /// The Prosy user role business.
    /// </summary>
    public class ProxyUserRoleBll : BaseBll, IProxyUserRoleBll
    {
        #region Fields

        /// <summary>
        /// Flag to indicate has access right
        /// </summary>
        private const string ACCESSED = "1";

        /// <summary>
        /// Char flag to indicate has access right
        /// </summary>
        private const char ACCESSED_CHAR = '1';

        /// <summary>
        /// Flag to indicate has no access right
        /// </summary>
        private const string DENIED = "0";

        #endregion Fields

        #region Methods

        /// <summary>
        /// Convert a user role string to ProxyUserRolePrivilegeModel4WS model
        /// </summary>
        /// <param name="userRole">a 5-bit string</param>
        /// <returns>ProxyUserRolePrivilegeModel4WS object</returns>
        public ProxyUserRolePrivilegeModel4WS ConvertToUserRolePrivilegeModel(string userRole)
        {
            char[] userTypes = userRole.ToCharArray();
            ProxyUserRolePrivilegeModel4WS proxyUserRolePrivilege = new ProxyUserRolePrivilegeModel4WS();

            proxyUserRolePrivilege.viewRecordAllowed = userTypes[0] == ACCESSED_CHAR;
            proxyUserRolePrivilege.createApplicationAllowed = userTypes[1] == ACCESSED_CHAR;
            proxyUserRolePrivilege.renewRecordAllowed = userTypes[2] == ACCESSED_CHAR;
            proxyUserRolePrivilege.manageInspectionsAllowed = userTypes[3] == ACCESSED_CHAR;
            proxyUserRolePrivilege.manageDocumentsAllowed = userTypes[4] == ACCESSED_CHAR;
            proxyUserRolePrivilege.makePaymentsAllowed = userTypes[5] == ACCESSED_CHAR;
            proxyUserRolePrivilege.amendmentAllowed = userTypes[6] == ACCESSED_CHAR;

            return proxyUserRolePrivilege;
        }

        /// <summary>
        /// Convert ProxyUserRolePrivilegeModel4WS model to a string
        /// </summary>
        /// <param name="proxyUserRolePrivilege">ProxyUserRolePrivilegeModel4WS object</param>
        /// <returns>string for user role privilege</returns>
        public string ConvertToUserRoleString(ProxyUserRolePrivilegeModel4WS proxyUserRolePrivilege)
        {
            StringBuilder userTypes = new StringBuilder();

            userTypes.Append(proxyUserRolePrivilege.viewRecordAllowed ? ACCESSED : DENIED);
            userTypes.Append(proxyUserRolePrivilege.createApplicationAllowed ? ACCESSED : DENIED);
            userTypes.Append(proxyUserRolePrivilege.renewRecordAllowed ? ACCESSED : DENIED);
            userTypes.Append(proxyUserRolePrivilege.manageInspectionsAllowed ? ACCESSED : DENIED);
            userTypes.Append(proxyUserRolePrivilege.manageDocumentsAllowed ? ACCESSED : DENIED);
            userTypes.Append(proxyUserRolePrivilege.makePaymentsAllowed ? ACCESSED : DENIED);
            userTypes.Append(proxyUserRolePrivilege.amendmentAllowed ? ACCESSED : DENIED);

            return userTypes.ToString();
        }

        /// <summary>
        /// Is the permission type has permission.
        /// </summary>
        /// <param name="permissionType">the permission type</param>
        /// <param name="role">the role string.</param>
        /// <returns>true or false.</returns>
        public bool IsPermissionTypeHasPermission(ProxyPermissionType permissionType, string role)
        {
            ProxyUserRolePrivilegeModel4WS userRoleModel = ConvertToUserRolePrivilegeModel(role);

            bool hasPermission = (permissionType == ProxyPermissionType.VIEW_RECORD && userRoleModel.viewRecordAllowed)
                || (permissionType == ProxyPermissionType.CREATE_APPLICATION && userRoleModel.createApplicationAllowed)
                || (permissionType == ProxyPermissionType.RENEW_RECORD && userRoleModel.renewRecordAllowed)
                || (permissionType == ProxyPermissionType.MANAGE_INSPECTIONS && userRoleModel.manageInspectionsAllowed)
                || (permissionType == ProxyPermissionType.MANAGE_DOCUMENTS && userRoleModel.manageDocumentsAllowed)
                || (permissionType == ProxyPermissionType.MAKE_PAYMENTS && userRoleModel.makePaymentsAllowed)
                || (permissionType == ProxyPermissionType.AMENDMENT && userRoleModel.amendmentAllowed);

            return hasPermission;
        }

        /// <summary>
        /// Judge cap contact model is set schedule inspection and read only permission or not.
        /// </summary>
        /// <param name="recordModel">the record model.</param>
        /// <param name="userRole">the user role.</param>
        /// <param name="permissionType">the permission Type</param>
        /// <param name="ignoreContactAccessLevel">ignore contact access level right that set at contact section of spear form</param>
        /// <returns>has schedule inspection and read only</returns>
        public bool HasPermission(CapModel4WS recordModel, UserRolePrivilegeModel userRole, ProxyPermissionType permissionType, bool ignoreContactAccessLevel = false)
        {
            bool hasPermission = false;
            var userRoleBll = ObjectFactory.GetObject<IUserRoleBll>();
            string contactPermissionType = GetContactPermissionType(permissionType);

            //the proxy user has role himself. 
            if (userRoleBll.HasPermission(recordModel, userRole, User.UserModel4WS, contactPermissionType, ignoreContactAccessLevel) || IsAdmin)
            {
                return true;
            }

            if (!IsEnableProxyUser())
            {
                return false;
            }

            List<PublicUserModel4WS> initUsers = new List<PublicUserModel4WS>();
            PublicUserModel4WS user = User.UserModel4WS;

            if (user.initialUsers != null && user.initialUsers.Length > 0)
            {
                initUsers = user.initialUsers.ToList();
            } 

            bool assignedPermission = false;

            if (initUsers.Count > 0)
            {
                foreach (PublicUserModel4WS initUser in initUsers)
                {
                    if (initUser.proxyUserModel == null || initUser.proxyUserModel.XProxyUserPermissionModels == null || initUser.proxyUserModel.XProxyUserPermissionModels.Length < 0)
                    {
                        return false;
                    }

                    string moduleName = recordModel != null ? recordModel.moduleName : string.Empty;
                    XProxyUserPermissionModel[] proxyPermission = initUser.proxyUserModel.XProxyUserPermissionModels.Where(p => p.levelType == ACAConstant.LEVEL_TYPE_MODULE && p.levelData == moduleName).ToArray();
                    
                    if (proxyPermission == null || proxyPermission.Length < 1)
                    {
                        continue;
                    }

                    ProxyUserRolePrivilegeModel4WS rolePrivilege = ConvertToUserRolePrivilegeModel(proxyPermission[0].permission);

                    switch (permissionType)
                    {
                        case ProxyPermissionType.VIEW_RECORD:
                            assignedPermission = rolePrivilege.viewRecordAllowed;
                            break;
                        case ProxyPermissionType.RENEW_RECORD:
                            assignedPermission = rolePrivilege.renewRecordAllowed;
                            break;
                        case ProxyPermissionType.MANAGE_INSPECTIONS:
                            assignedPermission = rolePrivilege.manageInspectionsAllowed;
                            break;
                        case ProxyPermissionType.MANAGE_DOCUMENTS:
                            assignedPermission = rolePrivilege.manageDocumentsAllowed;
                            break;
                        case ProxyPermissionType.MAKE_PAYMENTS:
                            assignedPermission = rolePrivilege.makePaymentsAllowed;
                            break;
                        case ProxyPermissionType.AMENDMENT:
                            assignedPermission = rolePrivilege.amendmentAllowed;
                            break;
                        default:
                            break;
                    }

                    if (assignedPermission && userRoleBll.HasPermission(recordModel, userRole, initUser, contactPermissionType))
                    {
                        hasPermission = true;
                        break;
                    }
                }
            }

            return hasPermission;
        }

        /// <summary>
        /// judge cap contact model is set schedule inspection and read only permission or not.
        /// </summary>
        /// <param name="recordModel">The record model.</param>
        /// <param name="userRole">the user Role</param>
        /// <returns>has schedule inspection and read only</returns>
        public bool HasReadOnlyPermission(CapModel4WS recordModel, UserRolePrivilegeModel userRole)
        {
            bool hasPermission = false;

            var userRoleBll = ObjectFactory.GetObject<IUserRoleBll>();

            //the proxy user has role himself.
            if (userRoleBll.HasReadOnlyPermission(recordModel, userRole, User.UserModel4WS) || IsAdmin)
            {
                return true;
            }

            if (!IsEnableProxyUser())
            {
                return false;
            }

            List<PublicUserModel4WS> initUsers = new List<PublicUserModel4WS>();
            ICapBll capBll = (ICapBll)ObjectFactory.GetObject(typeof(Accela.ACA.BLL.Cap.ICapBll));

            if (recordModel != null && capBll.IsPartialCap(recordModel.capClass))
            {
                initUsers.Add(GetCurrentUser(recordModel));
            }
            else
            {
                PublicUserModel4WS user = User.UserModel4WS;

                if (user.initialUsers != null && user.initialUsers.Length > 0)
                {
                    initUsers = user.initialUsers.ToList();
                }
            }

            bool assignedPermission = false;

            if (initUsers.Count > 0)
            {
                foreach (PublicUserModel4WS initUser in initUsers)
                {
                    if (initUser.proxyUserModel != null && initUser.proxyUserModel.XProxyUserPermissionModels != null)
                    {
                        XProxyUserPermissionModel[] proxyPermissions = initUser.proxyUserModel.XProxyUserPermissionModels.Where(p => p.levelType == ACAConstant.LEVEL_TYPE_MODULE && p.levelData == recordModel.moduleName).ToArray();

                        if (proxyPermissions == null || proxyPermissions.Length < 1)
                        {
                            continue;
                        }

                        ProxyUserRolePrivilegeModel4WS rolePrivilege = ConvertToUserRolePrivilegeModel(proxyPermissions[0].permission);
                        assignedPermission = rolePrivilege.viewRecordAllowed;

                        if (assignedPermission && userRoleBll.HasReadOnlyPermission(recordModel, userRole, initUser))
                        {
                            hasPermission = true;
                            break;
                        }
                    }
                }
            }

            return hasPermission;
        }

        /// <summary>
        /// judge cap contact model is set schedule inspection and read only permission or not.
        /// </summary>
        /// <param name="recordModel">The record model.</param>
        /// <param name="userRole">The user role.</param>
        /// <returns><c>true</c> if [has schedule permission] [the specified record model]; otherwise, <c>false</c>.</returns>
        public bool HasSchedulePermission(CapModel4WS recordModel, UserRolePrivilegeModel userRole)
        {
            bool hasPermission = false;

            var userRoleBll = ObjectFactory.GetObject<IUserRoleBll>();

            //the proxy user has role himself.
            if (userRoleBll.HasPermission(recordModel, userRole, User.UserModel4WS, ContactPermission.ScheduleInspectionOnly) || IsAdmin)
            {
                return true;
            }

            if (!IsEnableProxyUser())
            {
                return false;
            }

            List<PublicUserModel4WS> initUsers = new List<PublicUserModel4WS>();
            PublicUserModel4WS user = User.UserModel4WS;

            if (user.initialUsers != null && user.initialUsers.Length > 0)
            {
                initUsers = user.initialUsers.ToList();
            }

            bool assignedPermission = false;

            if (initUsers.Count > 0)
            {
                foreach (PublicUserModel4WS initUser in initUsers)
                {
                    if (initUser.proxyUserModel != null && initUser.proxyUserModel.XProxyUserPermissionModels != null)
                    {
                        XProxyUserPermissionModel[] proxyPermissions = initUser.proxyUserModel.XProxyUserPermissionModels.Where(p => p.levelType == ACAConstant.LEVEL_TYPE_MODULE && p.levelData == recordModel.moduleName).ToArray();

                        if (proxyPermissions == null || proxyPermissions.Length < 1)
                        {
                            continue;
                        }

                        ProxyUserRolePrivilegeModel4WS rolePrivilege = ConvertToUserRolePrivilegeModel(proxyPermissions[0].permission);
                        assignedPermission = rolePrivilege.manageInspectionsAllowed;

                        if (assignedPermission && userRoleBll.HasPermission(recordModel, userRole, initUser, ContactPermission.ScheduleInspectionOnly))
                        {
                            hasPermission = true;
                            break;
                        }
                    }
                }
            }

            return hasPermission;
        }

        /// <summary>
        /// Indicates whether enable proxy user.
        /// </summary>
        /// <returns>true means proxy user is enabled, else not</returns>
        public bool IsEnableProxyUser()
        {
            IXPolicyBll policyBll = (IXPolicyBll)ObjectFactory.GetObject(typeof(IXPolicyBll));
            string isEnableProxyUser = policyBll.GetValueByKey(XPolicyConstant.ACA_ENABLE_PROXYUSER);

            // if the value is 'Yes' or 'Y',the proxy user function is on, otherwise is off.
            return ValidationUtil.IsYes(isEnableProxyUser);
        }

        /// <summary>
        /// Get auto fill data.
        /// </summary>
        /// <param name="recordModel">the record model.</param>
        /// <returns>public user model.</returns>
        public PublicUserModel4WS GetCurrentUser(CapModel4WS recordModel)
        {
            PublicUserModel4WS currentUser = null;
            PublicUserModel4WS user = User.UserModel4WS;

            if (recordModel == null || recordModel.createdBy == User.PublicUserId || IsAdmin)
            {
                return user;
            }

            List<PublicUserModel4WS> initUsers = new List<PublicUserModel4WS>();
            var userRoleBll = ObjectFactory.GetObject<IUserRoleBll>();

            if (user.initialUsers != null && user.initialUsers.Length > 0)
            {
                initUsers = user.initialUsers.ToList();
            }

            if (initUsers.Count > 0)
            {
                IXPolicyBll xPolicyBll = (IXPolicyBll)ObjectFactory.GetObject(typeof(IXPolicyBll));
                XpolicyUserRolePrivilegeModel policy = xPolicyBll.GetPolicy(AgencyCode, ACAConstant.USER_ROLE_POLICY_FOR_CAP_SEARCH, ACAConstant.LEVEL_TYPE_MODULE, recordModel.moduleName);

                foreach (PublicUserModel4WS initUser in initUsers)
                {
                    if (initUser.proxyUserModel != null && initUser.proxyUserModel.XProxyUserPermissionModels != null)
                    {
                        XProxyUserPermissionModel[] proxyPermissions = initUser.proxyUserModel.XProxyUserPermissionModels.Where(p => p.levelType == ACAConstant.LEVEL_TYPE_MODULE && p.levelData == recordModel.moduleName).ToArray();

                        if (proxyPermissions == null || proxyPermissions.Length < 1)
                        {
                            continue;
                        }

                        ProxyUserRolePrivilegeModel4WS rolePrivilege = ConvertToUserRolePrivilegeModel(proxyPermissions[0].permission);
                        bool assignedPermission = rolePrivilege.createApplicationAllowed;
                        UserRolePrivilegeModel userRole = null;

                        if (policy != null)
                        {
                            userRole = policy.userRolePrivilegeModel;
                        }
                        else
                        {
                            userRole = new UserRolePrivilegeModel();
                            userRole.allAcaUserAllowed = true;
                        }
                        
                        if (userRole.allAcaUserAllowed || userRole.registeredUserAllowed)
                        {
                            userRole = userRoleBll.GetDefaultRole();
                        }

                        if (assignedPermission && userRoleBll.HasFullAccess(recordModel, userRole, initUser))
                        {
                            currentUser = initUser;
                            break;
                        }
                        else
                        {
                            continue;
                        }
                    }
                }
            }

            if (currentUser == null)
            {
                currentUser = user;
            }

            return currentUser;
        }

        /// <summary>
        /// Get Contact Permission Type.
        /// </summary>
        /// <param name="permissionType">the proxy user permission Type</param>
        /// <returns>contact type permission string</returns>
        private string GetContactPermissionType(ProxyPermissionType permissionType)
        {
            string contactPermissionType = string.Empty;

            switch (permissionType)
            {
                case ProxyPermissionType.AMENDMENT:
                    contactPermissionType = ContactPermission.RenewAndAmend;
                    break;
                case ProxyPermissionType.RENEW_RECORD:
                    contactPermissionType = ContactPermission.RenewAndAmend;
                    break;
                case ProxyPermissionType.MAKE_PAYMENTS:
                    contactPermissionType = ContactPermission.MakePayments;
                    break;
                case ProxyPermissionType.MANAGE_DOCUMENTS:
                    contactPermissionType = ContactPermission.ManageDocuments;
                    break;
                case ProxyPermissionType.MANAGE_INSPECTIONS:
                    contactPermissionType = ContactPermission.ScheduleInspectionOnly;
                    break;
                case ProxyPermissionType.VIEW_RECORD:
                    contactPermissionType = ContactPermission.ReadOnly;
                    break;
            }

            return contactPermissionType;
        }

        #endregion Methods
    }
}
