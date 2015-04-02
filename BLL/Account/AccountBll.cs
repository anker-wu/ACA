#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: AccountBLL.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2008-2014
 *
 *  Description:
 *
 *  Notes:
 * $Id: AccountBll.cs 278210 2014-08-29 05:45:24Z ACHIEVO\james.shi $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using System;
using System.Reflection;

using Accela.ACA.BLL.Common;
using Accela.ACA.Common;
using Accela.ACA.Common.Log;
using Accela.ACA.Common.Util;
using Accela.ACA.WSProxy;

using log4net;

namespace Accela.ACA.BLL.Account
{
    /// <summary>
    /// This class provide the ability to operation account.
    /// </summary>
    public class AccountBll : BaseBll, IAccountBll
    {
        #region Fields

        /// <summary>
        /// logger object.
        /// </summary>
        private static readonly ILog Logger = LogFactory.Instance.GetLogger(typeof(AccountBll));

        #endregion Fields

        #region Properties

        /// <summary>
        /// Gets an instance of PublicUserService.
        /// </summary>
        private PublicUserWebServiceService PublicUserService
        {
            get
            {
                return WSFactory.Instance.GetWebService<PublicUserWebServiceService>();
            }
        }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Method to activate user by hyperlink.
        /// </summary>
        /// <param name="agencyCode">agency code</param>
        /// <param name="uuId">uUID number</param>
        /// <returns>activate user name</returns>
        public string ActivateUser(string agencyCode, string uuId)
        {
            if (Logger.IsDebugEnabled)
            {
                Logger.Debug("Begin AccountBLL.ActivateUser()");
            }

            if (string.IsNullOrEmpty(uuId))
            {
                throw new DataValidateException(new string[] { "UUID" });
            }

            try
            {
                string result = PublicUserService.activateUser(agencyCode, uuId);

                if (Logger.IsDebugEnabled)
                {
                    Logger.Debug("End AccountBLL.ActivateUser()");
                }

                return result;
            }
            catch (Exception e)
            {
                throw new ACAException(e);
            }
        }

        /// <summary>
        /// Method to create a public user model
        /// </summary>
        /// <param name="userModel">new public user model</param>
        /// <param name="licenseModel4WSArray">licenseModel4WSArray collection</param>
        /// <returns>user sequence number</returns>
        public string CreatePublicUser(PublicUserModel4WS userModel, LicenseModel4WS[] licenseModel4WSArray)
        {
            return CreatePublicUser(userModel, licenseModel4WSArray, null);
        }

        /// <summary>
        /// Method to create a public user model and the relationship model between public user and SSO.
        /// </summary>
        /// <param name="userModel">new public user model</param>
        /// <param name="licenseModel4WSArray">licenseModel4WSArray collection</param>
        /// <param name="userSSOModel">The relationship model between public user and SSO</param>
        /// <returns>user sequence number</returns>
        /// <exception cref="ACAException">Exception from web service.</exception>
        public string CreatePublicUser(PublicUserModel4WS userModel, LicenseModel4WS[] licenseModel4WSArray, XPublicUserSSOModel userSSOModel)
        {
            /*
             * 1. Create user _accountWS.
             * 2. Add lience for the citizen.
            */
            try
            {
                //Only SSO create public user people model is null.
                if (userModel.peopleModel == null || userModel.peopleModel.Length == 0)
                {
                    CreatePeople4ExternalUser(ref userModel);
                }

                //set people template attribute associated contact sequence number.
                SetTemplateAttributeContactSeqNum(userModel);

                string userSeqNum = PublicUserService.createPublicUser(userModel, licenseModel4WSArray, userSSOModel).ToString();

                return userSeqNum;

                //TODO: invoke licenseWS to add license for the citizen.
            }
            catch (Exception e)
            {
                throw new ACAException(e);
            }
        }

        /// <summary>
        /// Method to edit a existing publish user model
        /// </summary>
        /// <param name="userModel">public user model</param>
        /// <exception cref="DataValidateException">userModel is null</exception>
        /// <exception cref="ACAException">Exception from web service.</exception>
        public void EditPublicUser(PublicUserModel4WS userModel)
        {
            EditPublicUser(userModel, null);
        }

        /// <summary>
        /// Method to edit a existing publish user model
        /// </summary>
        /// <param name="userModel">public user model</param>
        /// <param name="userSSOModel">The relationship model between public user and SSO</param>
        /// <exception cref="ACAException">Exception from web service.</exception>
        public void EditPublicUser(PublicUserModel4WS userModel, XPublicUserSSOModel userSSOModel)
        {
            try
            {
                PublicUserService.editPublicUser(userModel, userSSOModel);
            }
            catch (Exception e)
            {
                throw new ACAException(e);
            }
        }

        /// <summary>
        /// Gets a publicUser model by email address.
        /// </summary>
        /// <param name="serviceProviderCode">service provider code</param>
        /// <param name="emailOrUserID">email address or public user Id</param>
        /// <returns>Public User Model</returns>
        /// <exception cref="DataValidateException">email Or User ID is null}</exception>
        /// <exception cref="ACAException">Exception from web service.</exception>
        public PublicUserModel4WS GetPublicUserByEmailOrUserId(string serviceProviderCode, string emailOrUserID)
        {
            if (string.IsNullOrEmpty(emailOrUserID))
            {
                throw new DataValidateException(new string[] { "emailOrUserID" });
            }

            try
            {
                PublicUserModel4WS response = PublicUserService.getPublicUserByEmailOrUserId(serviceProviderCode, emailOrUserID);

                return response;
            }
            catch (Exception e)
            {
                throw new ACAException(e);
            }
        }

        /// <summary>
        /// Method to assert if existing Email address.
        /// </summary>
        /// <param name="agenceCode">agency code</param>
        /// <param name="emailID">Email address</param>
        /// <returns>Account Type</returns>
        /// <exception cref="DataValidateException">emailID is null</exception>
        /// <exception cref="ACAException">Exception from web service.</exception>
        public string IsExistingEmailID(string agenceCode, string emailID)
        {
            if (Logger.IsDebugEnabled)
            {
                Logger.Debug("Begin AccountBLL.IsExistingEmailID()");
            }

            if (string.IsNullOrEmpty(emailID))
            {
                throw new DataValidateException(new string[] { "emailID" });
            }

            try
            {
                string result = PublicUserService.isExistingEmailID(agenceCode, emailID);

                if (Logger.IsDebugEnabled)
                {
                    Logger.Debug("End AccountBLL.IsExistingEmailID()");
                }

                return result;
            }
            catch (Exception e)
            {
                throw new ACAException(e);
            }
        }

        /// <summary>
        /// Method to assert if existing user.
        /// </summary>
        /// <param name="agenceCode">agency code</param>
        /// <param name="userID">public user id</param>
        /// <returns>Account Type</returns>
        /// <exception cref="DataValidateException">{ userID } is null</exception>
        /// <exception cref="ACAException">Exception from web service.</exception>
        public string IsExistingUserID(string agenceCode, string userID)
        {
            if (Logger.IsDebugEnabled)
            {
                Logger.Debug("Begin AccountBLL.IsExistingUserID()");
            }

            if (string.IsNullOrEmpty(userID))
            {
                throw new DataValidateException(new string[] { "userID" });
            }

            try
            {
                string result = PublicUserService.isExistingUserID(agenceCode, userID);

                if (Logger.IsDebugEnabled)
                {
                    Logger.Debug("End AccountBLL.IsExistingUserID()");
                }

                return result;
            }
            catch (Exception e)
            {
                throw new ACAException(e);
            }
        }

        /// <summary>
        /// Resets password for a public User.
        /// </summary>
        /// <param name="serviceProviderCode">service provider code</param>
        /// <param name="userID">public user ID</param>
        /// <exception cref="DataValidateException">{ userID } is null</exception>
        /// <exception cref="ACAException">Exception from web service.</exception>
        public void ResetPassword(string serviceProviderCode, string userID)
        {
            if (Logger.IsDebugEnabled)
            {
                Logger.Debug("Begin AccountBLL.resetPassword()");
            }

            if (string.IsNullOrEmpty(userID))
            {
                throw new DataValidateException(new string[] { "userID" });
            }

            try
            {
                PublicUserService.resetPassword(serviceProviderCode, userID);
            }
            catch (Exception e)
            {
                throw new ACAException(e);
            }

            if (Logger.IsDebugEnabled)
            {
                Logger.Debug("End AccountBLL.resetPassword()");
            }
        }

        /// <summary>
        /// Method to sign on and retrieve public user information
        /// </summary>
        /// <param name="agenceCode">agency code</param>
        /// <param name="userId">publish user id</param>
        /// <param name="password">user password</param>
        /// <returns>error message or public user information</returns>
        /// <exception cref="DataValidateException">{ userId } is null</exception>
        /// <exception cref="ACAException">Exception from web service.</exception>
        public PublicUserModel4WS Signon(string agenceCode, string userId, string password)
        {
            if (Logger.IsDebugEnabled)
            {
                Logger.Debug("Begin AccountBLL.signon()");
            }

            if (string.IsNullOrEmpty(userId))
            {
                throw new DataValidateException(new string[] { "userId" });
            }

            ResultModel signonModel = null;
            PublicUserModel4WS model = null;

            try
            {
                signonModel = PublicUserService.signon(agenceCode, userId, password);
            }
            catch (Exception e)
            {
                throw new ACAException(e);
            }

            if (string.IsNullOrEmpty(signonModel.errorCode))
            {
                string userSeqNum = Convert.ToString(signonModel.entityValue);

                if (!string.IsNullOrEmpty(userSeqNum) && !userSeqNum.Equals("-1"))
                {
                    model = PublicUserService.getPublicUser(agenceCode, Convert.ToInt64(userSeqNum));
                }
            }
            else
            {
                throw new ACAException(new Exception(signonModel.errorCode));
            }

            if (Logger.IsDebugEnabled)
            {
                Logger.Debug("End AccountBLL.signon()");
            }

            return model;
        }

        /// <summary>
        /// Retrieve user information and initial user context for external user.
        /// </summary>
        /// <param name="agenceCode">Agency code.</param>
        /// <param name="userId">public user id.</param>
        /// <param name="accountType">account type.</param>
        /// <returns>Public user model.</returns>
        /// <exception cref="ACAException">Exception from web service.</exception>
        public PublicUserModel4WS Signon4External(string agenceCode, string userId, string accountType)
        {
            try
            {
                return PublicUserService.signon4External(agenceCode, userId, accountType);
            }
            catch (Exception exception)
            {
                throw new ACAException(exception);
            }
        }

        /// <summary>
        /// public user model.
        /// </summary>
        /// <param name="userSeqNum">the user sequence number.</param>
        /// <returns>the public user model.</returns>
        /// <exception cref="ACAException">Exception from web service.</exception>
        public PublicUserModel4WS GetPublicUser(string userSeqNum)
        {
            try
            {
                return PublicUserService.getPublicUser(AgencyCode, Convert.ToInt64(userSeqNum));
            }
            catch (Exception e)
            {
                throw new ACAException(e);
            }
        }

        /// <summary>
        /// Merge source user's properties which value are not null to destination user model.
        /// </summary>
        /// <param name="source">Source public user model.</param>
        /// <param name="destination">Destination public user model.</param>
        public void MergePublicUserInfo(PublicUserModel4WS source, ref PublicUserModel4WS destination)
        {
            if (source == null || destination == null)
            {
                return;
            }

            Type publicUserType = destination.GetType();
            PropertyInfo[] propertyInfos = publicUserType.GetProperties();

            if (propertyInfos != null && propertyInfos.Length > 0)
            {
                foreach (PropertyInfo destProperty in propertyInfos)
                {
                    if (destProperty == null || !destProperty.CanWrite)
                    {
                        continue;
                    }

                    PropertyInfo sourceProperty = publicUserType.GetProperty(destProperty.Name);

                    if (sourceProperty == null && !sourceProperty.CanRead)
                    {
                        continue;
                    }

                    object sourceValue = sourceProperty.GetValue(source, null);

                    if (sourceValue != null)
                    {
                        destProperty.SetValue(destination, sourceValue, null);
                    }
                }
            }
        }

        /// <summary>
        /// Update password.
        /// </summary>
        /// <param name="serviceProviderCode">service provider code</param>
        /// <param name="userSeqNum">public user sequence number</param>
        /// <param name="flag">flag indicate the password whether have been changed</param>
        /// <param name="auditID">public user id</param>
        /// <returns>Indicate whether need change password</returns>
        /// <exception cref="ACAException">Exception from web service.</exception>
        public bool UpdateNeedChangePassword(string serviceProviderCode, string userSeqNum, string flag, string auditID)
        {
            if (Logger.IsDebugEnabled)
            {
                Logger.Debug("Begin AccountBLL.UpdateNeedChangePassword()");
            }

            try
            {
                bool isNeedChangePassword = PublicUserService.updateNeedChangePassword(serviceProviderCode, userSeqNum, flag, auditID);

                if (Logger.IsDebugEnabled)
                {
                    Logger.Debug("End AccountBLL.UpdateNeedChangePassword()");
                }

                return isNeedChangePassword;
            }
            catch (Exception e)
            {
                throw new ACAException(e);
            }
        }

        /// <summary>
        /// Validate user when login in ACA
        /// </summary>
        /// <param name="userName">validation's user name</param>
        /// <param name="password">validation's user password</param>
        /// <returns>Result model</returns>
        public PublicUserModel4WS ValidateUser(string userName, string password)
        {
            IBizDomainBll bizBll = (IBizDomainBll)ObjectFactory.GetObject(typeof(IBizDomainBll));
            string strEnableLdap = bizBll.GetValueForACAConfig(SuperAgencyCode, BizDomainConstant.STD_ITEM_ENABLE_LDAP_AUTHENTICATION);
            PublicUserModel4WS accountInfo = null;

            //If the LDAP authentication is enabled, the internal authentication will be disabled.
            if (ValidationUtil.IsYes(strEnableLdap))
            {
                PublicUserModel4WS ldapUserInfo = LdapAuthentication.ValidateUser(userName, password);
                PublicUserModel4WS existingUser = Signon4External(AgencyCode, userName, string.Empty);

                if (existingUser != null && !string.IsNullOrEmpty(existingUser.userSeqNum))
                {
                    //Use the LDAP user's information which value are not null to update existing public user.
                    MergePublicUserInfo(ldapUserInfo, ref existingUser);
                    existingUser.password = null;
                    existingUser.servProvCode = AgencyCode;

                    //Save user information to DB.
                    EditPublicUser(existingUser);
                    accountInfo = existingUser;
                }
                else
                {
                    //The public user information not existing in Accela system.
                    ldapUserInfo.userSeqNum = "-1";
                    accountInfo = ldapUserInfo;

                    //Create people data for the new account.
                    CreatePeople4ExternalUser(ref accountInfo);
                }
            }
            else
            {
                accountInfo = Signon(AgencyCode, userName, password);
            }

            if (accountInfo != null && accountInfo.middleName == null)
            {
                accountInfo.middleName = string.Empty;
            }

            return accountInfo;
        }

        /// <summary>
        /// Validate password security.
        /// </summary>
        /// <param name="passwordConditionModel">password condition model</param>
        /// <returns>password validate result</returns>
        /// <exception cref="ACAException">Exception from web service.</exception>
        public PasswordResultModel ValidatePasswordSecurity(PasswordConditionModel passwordConditionModel)
        {             
            if (Logger.IsDebugEnabled)
            {
                Logger.Debug("Begin AccountBLL.ValidatePasswordSecurity()");
            }
            
            PasswordResultModel passwordResultModel = null;

            try
            {
                passwordResultModel = PublicUserService.validatePasswordSecurity(passwordConditionModel); 
            }
            catch (Exception e)
            {
                throw new ACAException(e);
            }

            if (Logger.IsDebugEnabled)
            {
                Logger.Debug("End AccountBLL.ValidatePasswordSecurity()");
            }

            return passwordResultModel;
        }

        /// <summary>
        /// Get password requirement.
        /// </summary>
        /// <param name="agencyCode">service provider code</param>
        /// <returns>password requirement</returns>
        /// <exception cref="ACAException">Exception from web service.</exception>
        public string GetPasswordRequirement(string agencyCode)
        {
            string requirement = null;

            try
            {
                requirement = PublicUserService.getPasswordRequirement(agencyCode);
            }
            catch (Exception e)
            {
                throw new ACAException(e);
            }

            // Base on WCAG criteria, replace h4 with div
            if (!string.IsNullOrEmpty(requirement))
            {
                requirement = requirement.Replace("h4.password_title ", "div.password_title ")
                    .Replace("<h4 class=\"password_title\"", "<div class=\"Header_h4 password_title\"")
                    .Replace("</h4>", "</div>");
            }

            return requirement;
        }

        /// <summary>
        /// Edit authorized agent clerk.
        /// </summary>
        /// <param name="userModel">public user model</param>
        /// <param name="callerID">public user id</param>
        /// <exception cref="DataValidateException">{ userModel } or { callerID } is null</exception>
        /// <exception cref="ACAException">Exception from web service.</exception>
        public void EditAuthAgentClerk(PublicUserModel4WS userModel, string callerID)
        {
            if (userModel == null)
            {
                throw new DataValidateException(new string[] { "userModel" });
            }

            if (string.IsNullOrEmpty(callerID))
            {
                throw new DataValidateException(new string[] { "callerID" });
            }

            try
            {
                /* If the templateObjectNum is null, the biz interface PublicUserService.editAuthAgentClerk will throw exception.
                 * So avoid the userModel.peopleModel.attributes[].templateObjectNum is null, set its value as contactSeqNumber.
                 */
                SetTemplateAttributeContactSeqNum(userModel);

                PublicUserService.editAuthAgentClerk(userModel, callerID);
            }
            catch (Exception e)
            {
                throw new ACAException(e);
            }
        }

        /// <summary>
        /// Method to deactivate clerk.
        /// </summary>
        /// <param name="agencyCode">agency code</param>
        /// <param name="uuId">uUID number</param>
        /// <returns>deactivate user name</returns>
        /// <exception cref="DataValidateException">{ UUID } is null</exception>
        /// <exception cref="ACAException">Exception from web service.</exception>
        public string DeactivateUserForClerk(string agencyCode, string uuId)
        {
            if (string.IsNullOrEmpty(uuId))
            {
                throw new DataValidateException(new string[] { "UUID" });
            }

            try
            {
                string result = PublicUserService.deactivateUserForClerk(agencyCode, uuId);

                return result;
            }
            catch (Exception e)
            {
                throw new ACAException(e);
            }
        }

        /// <summary>
        /// Get the clerk list by agent user.
        /// </summary>
        /// <param name="publicUser">public user model</param>
        /// <returns>clerk user model array</returns>
        /// <exception cref="DataValidateException">{ publicUser } is null</exception>
        /// <exception cref="ACAException">Exception from web service.</exception>
        public PublicUserModel4WS[] GetClerkList(PublicUserModel4WS publicUser)
        {
            if (publicUser == null)
            {
                throw new DataValidateException(new string[] { "publicUser" });
            }

            try
            {
                PublicUserModel4WS[] result = PublicUserService.getClerkList(publicUser);

                return result;
            }
            catch (Exception e)
            {
                throw new ACAException(e);
            }
        }

        /// <summary>
        /// Check the lock-account setting when user input answer wrong.
        /// </summary>
        /// <param name="servProvCode">agency code</param>
        /// <param name="userIdOrEmailId">The user id or email id.</param>
        /// <param name="userSeqNbr">The user sequence NBR.</param>
        /// <param name="isAnswerCorrect">Is answer correct.</param>
        /// <returns>The public user status.</returns>
        /// <exception cref="DataValidateException">{ userIdOrEmailId } is null</exception>
        /// <exception cref="ACAException">Exception from web service.</exception>
        public string IsLockedUserBySecurityQuestionFail(string servProvCode, string userIdOrEmailId, string userSeqNbr, bool isAnswerCorrect)
        {
            if (string.IsNullOrEmpty(userIdOrEmailId))
            {
                throw new DataValidateException(new string[] { "userIdOrEmailId" });
            }

            try
            {
                return PublicUserService.isLockedUserBySecurityQuestionFail(servProvCode, userIdOrEmailId, userSeqNbr, isAnswerCorrect);
            }
            catch (Exception e)
            {
                throw new ACAException(e);
            }
        }

        /// <summary>
        /// Update user security questions.
        /// </summary>
        /// <param name="userModel">public user model</param>
        /// <exception cref="DataValidateException">{ userModel } is null</exception>
        /// <exception cref="ACAException">Exception from web service.</exception>
        public void UpdateSecurityQuestions(PublicUserModel4WS userModel)
        {
            if (userModel == null)
            {
                throw new DataValidateException(new string[] { "userModel" });
            }

            try
            {
                PublicUserService.updateSecurityQuestions(userModel);
            }
            catch (Exception e)
            {
                throw new ACAException(e);
            }
        }

        /// <summary>
        /// Validate public user account status
        /// </summary>
        /// <param name="servProvCode">Service Provider Code</param>
        /// <param name="emailOrUserID">Email or User ID</param>
        /// <returns>
        /// return null if the account doesn't exist in DB. 
        /// return public user sequence number if the account exists in DB.
        /// </returns>
        public string ValidatePublicUserAccount(string servProvCode, string emailOrUserID)
        {
            return PublicUserService.validatePublicUserAccount(servProvCode, emailOrUserID);
        }

        /// <summary>
        /// set public user account information to people model.
        /// </summary>
        /// <param name="publicUser">the public user model.</param>
        /// <param name="people">the people model.</param>
        public void SetAccountInfoToPeopleModel(PublicUserModel4WS publicUser, ref PeopleModel4WS people)
        {
            if (publicUser == null || people == null)
            {
                return;
            }

            people.email = publicUser.email;
            people.firstName = publicUser.firstName;
            people.middleName = publicUser.middleName;
            people.lastName = publicUser.lastName;
            people.gender = publicUser.gender;
            people.salutation = publicUser.salutation;
            people.birthDate = publicUser.birthDate;

            CompactAddressModel4WS compactAddress = new CompactAddressModel4WS();
            compactAddress.addressLine1 = publicUser.address;
            compactAddress.addressLine2 = publicUser.address2;
            compactAddress.city = publicUser.city;
            compactAddress.state = publicUser.state;
            compactAddress.country = publicUser.country;
            compactAddress.zip = publicUser.zip;

            people.compactAddress = compactAddress;

            people.phone1 = publicUser.homePhone;
            people.phone2 = publicUser.cellPhone;
            people.phone3 = publicUser.workPhone;
            people.fax = publicUser.fax;
            people.fein = publicUser.fein;
            people.socialSecurityNumber = publicUser.ssn;
            people.businessName = publicUser.businessName;
            people.title = publicUser.userTitle;
            people.postOfficeBox = publicUser.pobox;
        }

        /// <summary>
        /// Get associated licenses by agency code and user id/ email.
        /// </summary>
        /// <param name="servProvCode">Agency code</param>
        /// <param name="userIDOrEmail">User ID or Email</param>
        /// <returns>public user associated license list</returns>
        /// <exception cref="ACAException">Exception from web service.</exception>
        public LicenseModel4WS[] GetPublicUserAssociatedLicenses(string servProvCode, string userIDOrEmail)
        {
            try
            {
                return PublicUserService.getPublicUserAssociatedLicenses(servProvCode, userIDOrEmail);
            }
            catch (Exception e)
            {
                throw new ACAException(e);
            }
        }

        /// <summary>
        /// Get associated contacts by user email or user id
        /// </summary>
        /// <param name="agencyCode">Agency Code</param>
        /// <param name="emailOrUserID">User id or email id</param>
        /// <param name="isIgnoreAgency">Is ignore agency</param>
        /// <returns>associated contacts</returns>
        /// <exception cref="ACAException">Exception from web service.</exception>
        public PeopleModel4WS[] GetContactsByPublicUser(string agencyCode, string emailOrUserID, bool isIgnoreAgency)
        {
            try
            {
                return PublicUserService.getContactsByPublicUser(agencyCode, emailOrUserID, isIgnoreAgency);
            }
            catch (Exception e)
            {
                throw new ACAException(e);
            }
        }

        /// <summary>
        /// Get public user model by user email or user id
        /// </summary>
        /// <param name="emailOrUserID">User id or email id</param>
        /// <returns>Public user model</returns>
        /// <exception cref="ACAException">Exception from web service.</exception>
        public PublicUserModel GetPublicUserByEmailOrUserID(string emailOrUserID)
        {
            try
            {
                return PublicUserService.getPublicUserByEmailOrUserID(emailOrUserID);
            }
            catch (Exception e)
            {
                throw new ACAException(e);
            }
        }

        /// <summary>
        /// Activate an existing user account
        /// </summary>
        /// <param name="model">public user information.</param>
        /// <returns>User sequence number</returns>
        /// <exception cref="DataValidateException">{ model } is null</exception>
        /// <exception cref="ACAException">Exception from web service.</exception>
        public string ActiveExistingAccount(PublicUserModel4WS model)
        {
            try
            {
                if (model == null)
                {
                    throw new DataValidateException(new string[] { "model" });
                }

                return PublicUserService.activeExistingAccount(model).ToString();
            }
            catch (Exception e)
            {
                throw new ACAException(e);
            }
        }

        /// <summary>
        /// Get User token form rest API.
        /// </summary>
        /// <param name="servProvCode">the agency code.</param>
        /// <param name="userID">the user id</param>
        /// <returns>the user token.</returns>
        /// <exception cref="DataValidateException">{ agency code } or { userID } is null</exception>
        /// <exception cref="ACAException">Exception from web service.</exception>
        public string GetUserToken(string servProvCode, string userID)
        {
            try
            {
                if (string.IsNullOrEmpty(servProvCode))
                {
                    throw new DataValidateException(new string[] { "agencycode" });
                }

                if (string.IsNullOrEmpty(userID))
                {
                    throw new DataValidateException(new string[] { "userID" });
                }

                return PublicUserService.getUserToken(servProvCode, userID);
            }
            catch (Exception e)
            {
                return null;
            }
        }

        /// <summary>
        /// Edit the relationship between public user and external user.
        /// </summary>
        /// <param name="userSSOModel">The relationship model between public user and SSO.</param>
        /// <param name="actionType">The action type.</param>
        /// <exception cref="ACAException">Exception from web service.</exception>
        public void EditXPublicUserSSO(XPublicUserSSOModel userSSOModel, string actionType)
        {
            try
            {
                PublicUserService.editXPublicUserSSO(userSSOModel, actionType);
            }
            catch (Exception e)
            {
                throw new ACAException(e);
            }
        }

        /// <summary>
        /// Get the public user model by the SSO relationship model.
        /// </summary>
        /// <param name="userSSOModel">The relationship model between public user and SSO.</param>
        /// <returns>public user information.</returns>
        /// <exception cref="ACAException">Exception from web service.</exception>
        public PublicUserModel4WS GetPublicUserBySSO(XPublicUserSSOModel userSSOModel)
        {
            try
            {
                return PublicUserService.getPublicUserBySSO(userSSOModel);
            }
            catch (Exception e)
            {
                throw new ACAException(e);
            }
        }

        /// <summary>
        /// Set template attribute contact sequence number.
        /// </summary>
        /// <param name="userModel">public user model</param>
        private static void SetTemplateAttributeContactSeqNum(PublicUserModel4WS userModel)
        {
            if (userModel.peopleModel != null && userModel.peopleModel.Length > 0)
            {
                foreach (PeopleModel4WS people in userModel.peopleModel)
                {
                    if (people.attributes == null)
                    {
                        continue;
                    }

                    foreach (TemplateAttributeModel attribute in people.attributes)
                    {
                        attribute.templateObjectNum = people.contactSeqNumber;
                    }
                }
            }
        }

        /// <summary>
        /// Create people data for external user(SSO/LDAP).
        /// </summary>
        /// <param name="publicUser">the public user model.</param>
        private void CreatePeople4ExternalUser(ref PublicUserModel4WS publicUser)
        {
            PeopleModel4WS people = new PeopleModel4WS();
            people.serviceProviderCode = AgencyCode;
            people.auditStatus = ACAConstant.VALID_STATUS;
            people.contactType = ContactType4License.Individual.ToString();

            SetAccountInfoToPeopleModel(publicUser, ref people);
            publicUser.peopleModel = new PeopleModel4WS[] { people };
        }

        #endregion Methods
    }
}