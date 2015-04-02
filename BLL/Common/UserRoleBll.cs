#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: UerRoleBll.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2010-2014
 *
 *  Description:
 *
 *  Notes:
 * $Id: UserRoleBll.cs 278106 2014-08-27 09:49:49Z ACHIEVO\james.shi $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Accela.ACA.Common;
using Accela.ACA.Common.Util;
using Accela.ACA.WSProxy;

namespace Accela.ACA.BLL.Common
{
    /// <summary>
    /// This class provide the ability to get user role related value
    /// </summary>
    public class UserRoleBll : BaseBll, IUserRoleBll
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
        /// Convert a user role string to UserRolePrivilegeModel model
        /// </summary>
        /// <param name="userRole">a 5-bit string</param>
        /// <returns>User role privilege object</returns>
        public UserRolePrivilegeModel ConvertToUserRolePrivilegeModel(string userRole)
        {
            char[] userTypes = userRole.ToCharArray();
            UserRolePrivilegeModel userPrivilegeModel = new UserRolePrivilegeModel();

            userPrivilegeModel.allAcaUserAllowed = userTypes[0] == ACCESSED_CHAR ? true : false;
            userPrivilegeModel.capCreatorAllowed = userTypes[1] == ACCESSED_CHAR ? true : false;
            userPrivilegeModel.licensendProfessionalAllowed = userTypes[2] == ACCESSED_CHAR ? true : false;
            userPrivilegeModel.contactAllowed = userTypes[3] == ACCESSED_CHAR ? true : false;
            userPrivilegeModel.ownerAllowed = userTypes[4] == ACCESSED_CHAR ? true : false;

            if (userTypes.Length > 5)
            {
                userPrivilegeModel.registeredUserAllowed = userTypes[5] == ACCESSED_CHAR ? true : false;
            }

            if (userTypes.Length > 6)
            {
                userPrivilegeModel.AgentAllowed = userTypes[6] == ACCESSED_CHAR;
            }

            if (userTypes.Length > 7)
            {
                userPrivilegeModel.AgentClerkAllowed = userTypes[7] == ACCESSED_CHAR;
            }

            return userPrivilegeModel;
        }

        /// <summary>
        /// Convert UserRolePrivilegeModel model to a string
        /// </summary>
        /// <param name="userPrivilegeModel">UserRolePrivilegeModel object</param>
        /// <returns>string for user role privilege</returns>
        public string ConvertToUserRoleString(UserRolePrivilegeModel userPrivilegeModel)
        {
            StringBuilder userTypes = new StringBuilder();

            userTypes.Append(userPrivilegeModel.allAcaUserAllowed ? ACCESSED : DENIED);
            userTypes.Append(userPrivilegeModel.capCreatorAllowed ? ACCESSED : DENIED);
            userTypes.Append(userPrivilegeModel.licensendProfessionalAllowed ? ACCESSED : DENIED);
            userTypes.Append(userPrivilegeModel.contactAllowed ? ACCESSED : DENIED);
            userTypes.Append(userPrivilegeModel.ownerAllowed ? ACCESSED : DENIED);
            userTypes.Append(userPrivilegeModel.registeredUserAllowed ? ACCESSED : DENIED);

            return userTypes.ToString();
        }

        /// <summary>
        /// Determine whether current public user has right to process CAP Type activities
        /// </summary>
        /// <param name="userRole">a UserRolePrivilegeModel model which contains information about what user role has right</param>
        /// <param name="isModuleLevel">Is the cap type use module level role settings.</param>
        /// <returns>a boolean value indicates whether current public user has right</returns>
        public bool IsCapTypeHasRight(UserRolePrivilegeModel userRole, bool isModuleLevel)
        {
            bool hasRight = false;

            if (userRole == null)
            {
                return false;
            }

            if (userRole.allAcaUserAllowed)
            {
                hasRight = true;
            }

            if (!hasRight && userRole.registeredUserAllowed)
            {
                if (IsRegisteredUser())
                {
                    hasRight = true;
                }
            }

            if (!hasRight && userRole.capCreatorAllowed)
            {
                if (IsRegisteredUser())
                {
                    hasRight = true;
                }
            }

            if (!hasRight && userRole.contactAllowed)
            {
                if (IsRegisteredUser())
                {
                    hasRight = true;
                }
            }

            if (!hasRight && userRole.ownerAllowed)
            {
                if (IsRegisteredUser())
                {
                    hasRight = true;
                }
            }

            if (!hasRight && userRole.licensendProfessionalAllowed)
            {
                if (IsRegisteredUser())
                {
                    if (isModuleLevel)
                    {
                        hasRight = true;
                    }
                    else
                    {
                        hasRight = IsMatchLicenseTypes(userRole);
                    }
                }
            }

            return hasRight;
        }

        /// <summary>
        /// return right for default role
        /// </summary>
        /// <returns>is has right for default role</returns>
        public UserRolePrivilegeModel GetDefaultRole()
        {
            UserRolePrivilegeModel userRole = new UserRolePrivilegeModel();
            userRole.allAcaUserAllowed = false;
            userRole.capCreatorAllowed = true;
            userRole.contactAllowed = true;
            userRole.licensendProfessionalAllowed = true;
            userRole.ownerAllowed = true;

            return userRole;
        }

        /// <summary>
        /// Judge cap contact model is set full access permission or not.
        /// </summary>
        /// <param name="recordModel">The record model.</param>
        /// <param name="userRole">The user role.</param>
        /// <param name="user">The user model.</param>
        /// <returns>has full access or not</returns>
        public bool HasFullAccess(CapModel4WS recordModel, UserRolePrivilegeModel userRole, PublicUserModel4WS user)
        {
            bool hasRight = HasRight(recordModel, userRole, user);
            bool hasFullAccess = hasRight;

            if (!hasRight)
            {
                if (recordModel != null && recordModel.contactsGroup != null && recordModel.contactsGroup.Length > 0)
                {
                    foreach (CapContactModel4WS capContact in recordModel.contactsGroup)
                    {
                        if (string.IsNullOrEmpty(capContact.accessLevel))
                        {
                            if (userRole.contactAllowed && HasMatchedContact(capContact, user))
                            {
                                return true;
                            }
                            else
                            {
                                continue;
                            }
                        }

                        if ((capContact.accessLevel.IndexOf(ContactPermission.FullAccess) > -1)
                            && HasMatchedContact(capContact, user))
                        {
                            return true;
                        }
                    }
                }

                if (recordModel != null && recordModel.applicantModel != null)
                {
                    if (string.IsNullOrEmpty(recordModel.applicantModel.accessLevel))
                    {
                        if (userRole.contactAllowed && HasMatchedContact(recordModel.applicantModel, user))
                        {
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }

                    if ((recordModel.applicantModel.accessLevel.IndexOf(ContactPermission.FullAccess) > -1)
                        && HasMatchedContact(recordModel.applicantModel, user))
                    {
                        return true;
                    }
                }
            }

            return hasFullAccess;
        }

        /// <summary>
        /// Judge cap contact model is set schedule inspection and read only permission or not.
        /// </summary>
        /// <param name="recordModel">The record model.</param>
        /// <param name="userRolePrivilegeModel">The user role privilege model.</param>
        /// <param name="user">The public user</param>
        /// <param name="contactPermissionType">specific contact permission type</param>
        /// <param name="ignoreContactAccessLevel">ignore contact access level right that set at contact section of spear form</param>
        /// <returns>has schedule inspection and read only</returns>
        public bool HasPermission(CapModel4WS recordModel, UserRolePrivilegeModel userRolePrivilegeModel, PublicUserModel4WS user, string contactPermissionType, bool ignoreContactAccessLevel = false)
        {
            userRolePrivilegeModel = userRolePrivilegeModel ?? new UserRolePrivilegeModel();

            bool hasRight = HasRight(recordModel, userRolePrivilegeModel, user); //indicates role permission except contact
            bool hasPermission = hasRight;

            if (!hasRight)
            {
                if (recordModel != null && recordModel.contactsGroup != null && recordModel.contactsGroup.Length > 0)
                {
                    foreach (CapContactModel4WS capContact in recordModel.contactsGroup)
                    {
                        if (string.IsNullOrEmpty(capContact.accessLevel) || ignoreContactAccessLevel)
                        {
                            if (userRolePrivilegeModel.contactAllowed && HasMatchedContact(capContact, user))
                            {
                                return true;
                            }

                            continue;
                        }

                        if ((capContact.accessLevel.IndexOf(ContactPermission.FullAccess) > -1 || capContact.accessLevel.IndexOf(contactPermissionType) > -1)
                            && HasMatchedContact(capContact, user))
                        {
                            //If the accessLevel is Full then should judge the user role setting again.
                            return IsHasRightForModuleSetting(userRolePrivilegeModel);
                        }
                    }
                }

                if (recordModel != null && recordModel.applicantModel != null)
                {
                    if (string.IsNullOrEmpty(recordModel.applicantModel.accessLevel) || ignoreContactAccessLevel)
                    {
                        if (userRolePrivilegeModel.contactAllowed && HasMatchedContact(recordModel.applicantModel, user))
                        {
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }

                    if ((recordModel.applicantModel.accessLevel.IndexOf(ContactPermission.FullAccess) > -1 || recordModel.applicantModel.accessLevel.IndexOf(contactPermissionType) > -1)
                        && HasMatchedContact(recordModel.applicantModel, user))
                    {
                        //If the accessLevel is Full then should judge the user role setting again.
                        return IsHasRightForModuleSetting(userRolePrivilegeModel);
                    }
                }
            }

            return hasPermission;
        }

        /// <summary>
        /// Judge cap contact model is set read only permission or not.
        /// </summary>
        /// <param name="recordModel">The record model.</param>
        /// <param name="userRole">The user role.</param>
        /// <param name="user">The user model.</param>
        /// <returns>has read only or not</returns>
        public bool HasReadOnlyPermission(CapModel4WS recordModel, UserRolePrivilegeModel userRole, PublicUserModel4WS user)
        {
            bool hasRight = HasRight(recordModel, userRole, user);
            bool hasReadOnly = hasRight;

            if (!hasRight)
            {
                List<CapContactModel4WS> contactList = new List<CapContactModel4WS>();

                if (recordModel != null && recordModel.contactsGroup != null && recordModel.contactsGroup.Length > 0)
                {
                    contactList.AddRange(recordModel.contactsGroup);
                }

                if (recordModel != null && recordModel.applicantModel != null)
                {
                    contactList.Add(recordModel.applicantModel);
                }

                if (contactList.Any())
                {
                    return HasContactPermission(userRole, user, contactList.ToArray());
                }
            }

            return hasReadOnly;
        }

        /// <summary>
        /// determine whether a CAP is created by current public user
        /// </summary>
        /// <param name="recordModel">The record model.</param>
        /// <param name="user">The user model.</param>
        /// <returns>true if the CAP is created by the current user; otherwise,false.</returns>
        public bool IsCAPCreator(CapModel4WS recordModel, PublicUserModel4WS user)
        {
            bool isCAPCreator = false;

            string publicUserID = ACAConstant.PUBLIC_USER_NAME + user.userSeqNum;

            if (recordModel != null && recordModel.createdBy == publicUserID)
            {
                isCAPCreator = true;
            }

            return isCAPCreator;
        }

        /// <summary>
        /// determine whether the license of a CAP is associated with current public user
        /// </summary>
        /// <param name="recordModel">The record model.</param>
        /// <param name="userRole">a UserRolePrivilegeModel model</param>
        /// <param name="user">The user model.</param>
        /// <returns>
        /// true if license of a CAP is associated with current public user; otherwise,false.
        /// </returns>
        public bool IsLicensedProfessional(CapModel4WS recordModel, UserRolePrivilegeModel userRole, PublicUserModel4WS user)
        {
            LicenseModel4WS[] licenses = user.licenseModel;
            bool hasRight = false;

            if (recordModel == null ||
                licenses == null ||
                userRole == null ||
                (recordModel.licenseProfessionalList == null && recordModel.licenseProfessionalModel == null))
            {
                return hasRight;
            }

            LicenseProfessionalModel[] licenseProfessionalList = TempModelConvert.ConvertToLicenseProfessionalModelList(recordModel.licenseProfessionalList);

            licenseProfessionalList = licenseProfessionalList ?? new LicenseProfessionalModel[1] { TempModelConvert.ConvertToLicenseProfessionalModel(recordModel.licenseProfessionalModel) };

            foreach (LicenseModel4WS license in licenses)
            {
                foreach (LicenseProfessionalModel licenseProfessional in licenseProfessionalList)
                {
                    if (licenseProfessional == null)
                    {
                        continue; 
                    }

                    if (!string.IsNullOrEmpty(licenseProfessional.licenseType) && !string.IsNullOrEmpty(licenseProfessional.licenseNbr)
                        && licenseProfessional.licenseType.Equals(license.licenseType) && licenseProfessional.licenseNbr.Equals(license.stateLicense))
                    {
                        // don't need to compare the LP type permission
                        if (userRole.licenseTypeRuleArray == null || userRole.licenseTypeRuleArray.Length == 0)
                        {
                            hasRight = true;
                            break;
                        }

                        // need to limit the permission to LP type level.
                        if (HasLPTypeRight(license.licenseType, userRole.licenseTypeRuleArray))
                        {
                            hasRight = true;
                            break;
                        }
                    }
                }

                if (hasRight)
                {
                    break;
                }
            }

            return hasRight;
        }

        /// <summary>
        /// Get Record Search Role
        /// </summary>
        /// <param name="agencyCode">current agency code</param>
        /// <param name="moduleName">the module name</param>
        /// <param name="capType">Type of the cap.</param>
        /// <returns>the user role privilege model.</returns>
        public UserRolePrivilegeModel GetRecordSearchRole(string agencyCode, string moduleName, CapTypeModel capType)
        {
            string entityPermission = ACAConstant.DEFAULT_PERMISSION;
            IXPolicyBll policyBll = (IXPolicyBll)ObjectFactory.GetObject(typeof(IXPolicyBll));
            XpolicyUserRolePrivilegeModel policy = policyBll.GetPolicy(agencyCode, ACAConstant.USER_ROLE_POLICY_FOR_CAP_SEARCH, ACAConstant.LEVEL_TYPE_MODULE, moduleName);
            bool isCapTypeLevel = policy != null && ACAConstant.COMMON_ONE.Equals(policy.data4);

            if (isCapTypeLevel)
            {
                CapTypePermissionModel capTypePermission = new CapTypePermissionModel();

                capTypePermission.serviceProviderCode = agencyCode;
                capTypePermission.moduleName = moduleName;
                capTypePermission.group = capType.group;
                capTypePermission.type = capType.type;
                capTypePermission.subType = capType.subType;
                capTypePermission.category = capType.category;
                capTypePermission.controllerType = ControllerType.CAPSEARCHFILTER.ToString();
                capTypePermission.entityType = EntityType.GENERAL.ToString();

                ICapTypePermissionBll capTypePermissionBll = (ICapTypePermissionBll)ObjectFactory.GetObject(typeof(ICapTypePermissionBll));
                CapTypePermissionModel[] permission = capTypePermissionBll.GetCapTypePermissions(agencyCode, capTypePermission);

                if (permission != null && permission.Length > 0)
                {
                    entityPermission = permission[0].entityPermission;
                }
            }
            else
            {
                if (policy != null)
                {
                    entityPermission = policy.data3;
                }
            }

            var userRoleBll = ObjectFactory.GetObject<IUserRoleBll>();
            UserRolePrivilegeModel userRole = userRoleBll.ConvertToUserRolePrivilegeModel(entityPermission);

            return userRole;
        }

        /// <summary>
        /// determine whether the owner of a CAP is associated with current public user
        /// </summary>
        /// <param name="recordModel">The record model.</param>
        /// <param name="user">The user model.</param>
        /// <returns>
        /// true if the owner of a CAP is associated with current public user; otherwise,false.
        /// </returns>
        public bool IsOwner(CapModel4WS recordModel, PublicUserModel4WS user)
        {
            OwnerModel[] owners = user.ownerModel;

            if (recordModel == null || owners == null
                || (recordModel.ownerModel == null && recordModel.capOwnerList == null))
            {
                return false;
            }

            bool isOwner = false;
            List<RefOwnerModel> ownerList = new List<RefOwnerModel>();
            if (recordModel.capOwnerList != null && recordModel.capOwnerList.Length > 0)
            {
                ownerList.AddRange(recordModel.capOwnerList);
            }

            if (recordModel.ownerModel != null)
            {
                ownerList.Add(recordModel.ownerModel);
            }

            if (ownerList.Count > 0)
            {
                foreach (OwnerModel owner in owners)
                {
                    if (owner == null)
                    {
                        continue;
                    }

                    if (HasMatchedOwner(ownerList.ToArray<RefOwnerModel>(), owner))
                    {
                        isOwner = true;
                        break;
                    }
                }
            }

            return isOwner;
        }

        /// <summary>
        /// Gets whether has matched contact for the current contact group of cap model
        /// </summary>
        /// <param name="capContact">The cap contact.</param>
        /// <param name="user">The user model.</param>
        /// <returns>
        /// true if has matched contact in the contact group; otherwise,false.
        /// </returns>
        private bool HasMatchedContact(CapContactModel4WS capContact, PublicUserModel4WS user)
        {
            PeopleModel4WS[] contacts = user.peopleModel;

            if (contacts != null && contacts.Length > 0)
            {
                foreach (PeopleModel4WS contact in contacts)
                {
                    if (contact == null)
                    {
                        continue;
                    }

                    if (contact.contactSeqNumber.Equals(capContact.refContactNumber))
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        /// <summary>
        /// determine whether the license of a CAP is associated with current public user
        /// </summary>
        /// <param name="userRole">a UserRolePrivilegeModel model</param>
        /// <returns>true if license of a CAP is associated with current public user; otherwise,false.</returns>
        private bool IsMatchLicenseTypes(UserRolePrivilegeModel userRole)
        {
            LicenseModel4WS[] licenses = User == null ? null : User.Licenses;
            bool hasRight = false;

            if (licenses == null ||
                userRole == null)
            {
                return hasRight;
            }

            foreach (LicenseModel4WS license in licenses)
            {
                // don't need to compare the LP type permission
                if (userRole.licenseTypeRuleArray == null || userRole.licenseTypeRuleArray.Length == 0)
                {
                    hasRight = true;
                    break;
                }

                // need to limit the permission to LP type level.
                if (HasLPTypeRight(license.licenseType, userRole.licenseTypeRuleArray))
                {
                    hasRight = true;
                    break;
                }
            }

            return hasRight;
        }

        /// <summary>
        /// check whether the user has the LP type right.
        /// </summary>
        /// <param name="licenseType">user's license type.</param>
        /// <param name="lpTypes">the LP type list.</param>
        /// <returns>if the user contains the specific LP, return true; otherwise,return false.</returns>
        private bool HasLPTypeRight(string licenseType, string[] lpTypes)
        {
            bool hasRight = false;

            if (string.IsNullOrEmpty(licenseType) || lpTypes == null || lpTypes.Length == 0)
            {
                return hasRight;
            }

            foreach (string lpType in lpTypes)
            {
                if (lpType.Equals(licenseType, StringComparison.InvariantCultureIgnoreCase))
                {
                    hasRight = true;
                    break;
                }
            }

            return hasRight;
        }

        /// <summary>
        /// Gets whether has matched owner for the current owner list of cap model
        /// </summary>
        /// <param name="capOwners">OwnerModel list object</param>
        /// <param name="owner">OwnerModel object need to match</param>
        /// <returns>true if has matched owner in the owner list; otherwise,false.</returns>
        private bool HasMatchedOwner(RefOwnerModel[] capOwners, OwnerModel owner)
        {
            bool isOwner = false;
            string ownerUID = owner.UID;

            foreach (RefOwnerModel capOwner in capOwners)
            {
                bool isNullNumber = capOwner.l1OwnerNumber == null ? true : false;

                // no owner for the CAP.
                if (string.IsNullOrEmpty(capOwner.UID) && isNullNumber)
                {
                    continue;
                }

                if (owner.ownerNumber == null || isNullNumber)
                {
                    // external APO
                    if (ownerUID == capOwner.UID)
                    {
                        isOwner = true;
                        break;
                    }
                }
                else
                {
                    // internal APO
                    if (owner.ownerNumber == capOwner.l1OwnerNumber)
                    {
                        isOwner = true;
                        break;
                    }
                }
            }

            return isOwner;
        }

        /// <summary>
        /// Gets whether the current user is registered user or not
        /// </summary>
        /// <returns>true means the current user is registered;otherwise,false.</returns>
        private bool IsRegisteredUser()
        {
            bool isRegisteredUser = true;

            if (User == null || User.IsAnonymous)
            {
                isRegisteredUser = false;
            }

            return isRegisteredUser;
        }

        /// <summary>
        /// Determine whether current public user has right to process CAP activities
        /// remove contact privilege from this function according the contact permission set in spear form.
        /// </summary>
        /// <param name="recordModel">The record model.</param>
        /// <param name="userRole">a UserRolePrivilegeModel model which contains information about what user role has right</param>
        /// <param name="user">The user model.</param>
        /// <returns>
        /// a boolean value indicates whether current public user has right
        /// </returns>
        private bool HasRight(CapModel4WS recordModel, UserRolePrivilegeModel userRole, PublicUserModel4WS user)
        {
            bool hasRight = false;

            if (userRole == null)
            {
                return false;
            }

            if (userRole.allAcaUserAllowed)
            {
                hasRight = true;
            }

            if (!hasRight && userRole.registeredUserAllowed)
            {
                if (IsRegisteredUser())
                {
                    // please refer to the property summary for more info.
                    if (!userRole.AgentOrClerkNotInRegisteredUsers
                        || (userRole.AgentOrClerkNotInRegisteredUsers && !CommonUtil.IsAuthorizedAgent(user) && !CommonUtil.IsAgentClerk(user)))
                    {
                        hasRight = true;
                    }
                }
            }

            if (!hasRight && userRole.capCreatorAllowed)
            {
                if (IsCAPCreator(recordModel, user))
                {
                    hasRight = true;
                }
            }

            if (!hasRight && userRole.licensendProfessionalAllowed)
            {
                if (IsLicensedProfessional(recordModel, userRole, user))
                {
                    hasRight = true;
                }
            }

            if (!hasRight && userRole.ownerAllowed)
            {
                if (IsOwner(recordModel, user))
                {
                    hasRight = true;
                }
            }

            if (!hasRight && userRole.AgentAllowed)
            {
                if (CommonUtil.IsAuthorizedAgent(user))
                {
                    hasRight = true;
                }
            }

            if (!hasRight && userRole.AgentClerkAllowed)
            {
                if (CommonUtil.IsAgentClerk(user))
                {
                    hasRight = true;
                }
            }

            return hasRight;
        }

        /// <summary>
        /// Determine whether current public user has right by contact permission
        /// </summary>
        /// <param name="userRole">a UserRolePrivilegeModel model which contains information about what user role has right</param>
        /// <returns> a boolean value indicates whether current public user has right</returns>
        private bool IsHasRightForModuleSetting(UserRolePrivilegeModel userRole)
        {
            bool hasRight = false;

            if (userRole == null)
            {
                return false;
            }

            if (userRole.capCreatorAllowed || userRole.contactAllowed)
            {
                hasRight = true;
            }

            return hasRight;
        }

        /// <summary>
        /// Check the contact permission
        /// </summary>
        /// <param name="userRole">user role privilege</param>
        /// <param name="user">public user</param>
        /// <param name="capContacts">cap contacts</param>
        /// <returns><c>true</c> if [has contact permission] [the specified user role]; otherwise, <c>false</c>.</returns>
        private bool HasContactPermission(UserRolePrivilegeModel userRole, PublicUserModel4WS user, CapContactModel4WS[] capContacts)
        {
            if (capContacts == null || capContacts.Length == 0)
            {
                return false;
            }

            foreach (CapContactModel4WS capContact in capContacts)
            {
                if (string.IsNullOrEmpty(capContact.accessLevel))
                {
                    if (userRole.contactAllowed && HasMatchedContact(capContact, user))
                    {
                        return true;
                    }
                    else
                    {
                        continue;
                    }
                }

                if ((capContact.accessLevel.IndexOf(ContactPermission.FullAccess) > -1
                     || capContact.accessLevel.IndexOf(ContactPermission.ScheduleInspectionOnly) > -1
                     || capContact.accessLevel.IndexOf(ContactPermission.MakePayments) > -1
                     || capContact.accessLevel.IndexOf(ContactPermission.ManageDocuments) > -1
                     || capContact.accessLevel.IndexOf(ContactPermission.RenewAndAmend) > -1
                     || capContact.accessLevel.IndexOf(ContactPermission.ReadOnly) > -1)
                    && HasMatchedContact(capContact, user)
                    && IsHasRightForModuleSetting(userRole))
                {
                    return true;
                }
            }

            return false;
        }

        #endregion Methods
    }
}
