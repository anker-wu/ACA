#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: IAccountBll.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2007-2014
 *
 *  Description:
 *
 *  Notes:
 * $Id: IAccountBll.cs 278210 2014-08-29 05:45:24Z ACHIEVO\james.shi $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 *  03/20/2008    Steven.lee    Initial version.
 * </pre>
 */

#endregion Header

using Accela.ACA.WSProxy;

namespace Accela.ACA.BLL.Account
{
    /// <summary>
    /// interface of Account business
    /// </summary>
    public interface IAccountBll
    {
        #region Methods
        /// <summary>
        /// Method to activate user by hyperlink.
        /// </summary>
        /// <param name="agencyCode">agency code</param>
        /// <param name="uUID">uUID number</param>
        /// <returns>activate user name</returns>
        string ActivateUser(string agencyCode, string uUID);

        /// <summary>
        ///  Method to create a public user model
        /// </summary>
        /// <param name="userModel">new public user model</param>
        /// <param name="licenseModel4WSArray">licenseModel4WSArray collection</param>
        /// <returns>user sequence number</returns>
        string CreatePublicUser(PublicUserModel4WS userModel, LicenseModel4WS[] licenseModel4WSArray);

        /// <summary>
        ///  Method to create a public user model and the relationship model between public user and SSO.
        /// </summary>
        /// <param name="userModel">new public user model</param>
        /// <param name="licenseModel4WSArray">licenseModel4WSArray collection</param>
        /// <param name="userSSOModel">The relationship model between public user and SSO</param>
        /// <returns>user sequence number</returns>
        string CreatePublicUser(PublicUserModel4WS userModel, LicenseModel4WS[] licenseModel4WSArray, XPublicUserSSOModel userSSOModel);

        /// <summary>
        /// Method to edit a existing publish user model
        /// </summary>
        /// <param name="userModel">public user model</param>
        void EditPublicUser(PublicUserModel4WS userModel);

        /// <summary>
        /// Method to edit a existing publish user model
        /// </summary>
        /// <param name="userModel">public user model</param>
        /// <param name="userSSOModel">The relationship model between public user and SSO</param>
        void EditPublicUser(PublicUserModel4WS userModel, XPublicUserSSOModel userSSOModel);

        /// <summary>
        /// Gets a publicUser model by email address.
        /// </summary>
        /// <param name="serviceProviderCode">service provider code</param>
        /// <param name="emailOrUserID">email address or public user Id</param>
        /// <returns>Public User Model</returns>
        PublicUserModel4WS GetPublicUserByEmailOrUserId(string serviceProviderCode, string emailOrUserID);

        /// <summary>
        /// Method to assert if existing Email address.
        /// </summary>
        /// <param name="agenceCode">agency code</param>
        /// <param name="emailID">Email address</param>
        /// <returns>Account Type</returns>
        string IsExistingEmailID(string agenceCode, string emailID);

        /// <summary>
        /// Method to assert if existing user.
        /// </summary>
        /// <param name="agenceCode">agency code</param>
        /// <param name="userID">public user id</param>
        /// <returns>Account Type</returns>
        string IsExistingUserID(string agenceCode, string userID);

        /// <summary>
        /// Resets password for a public User.
        /// </summary>
        /// <param name="serviceProviderCode">service provider code</param>
        /// <param name="userID">public user ID</param>
        void ResetPassword(string serviceProviderCode, string userID);

        /// <summary>
        /// Method to sign on and retrieve public user information
        /// </summary>
        /// <param name="agenceCode">agency code</param>
        /// <param name="userId">publish user id</param>
        /// <param name="password">user password</param>
        /// <returns>error message or public user information</returns>
        PublicUserModel4WS Signon(string agenceCode, string userId, string password);
        
        /// <summary>
        /// Retrieve user information and initial user context for external user.
        /// </summary>
        /// <param name="agenceCode">Agency code.</param>
        /// <param name="userId">public user id.</param>
        /// <param name="accountType">account type.</param>
        /// <returns>Public user model.</returns>
        PublicUserModel4WS Signon4External(string agenceCode, string userId, string accountType);

        /// <summary>
        /// Update password.
        /// </summary>
        /// <param name="serviceProviderCode">service provider code</param>
        /// <param name="userSeqNum">public user sequence number</param>
        /// <param name="flag">flag indicate the password whether have been changed</param>
        /// <param name="auditID">public user id</param>
        /// <returns>Indicate whether need change password</returns>
        bool UpdateNeedChangePassword(string serviceProviderCode, string userSeqNum, string flag, string auditID);

        /// <summary>
        /// Validate password security.
        /// </summary>
        /// <param name="passwordConditionModel">password condition model</param>
        /// <returns>password validate result</returns>
        PasswordResultModel ValidatePasswordSecurity(PasswordConditionModel passwordConditionModel);

        /// <summary>
        /// Get password requirement.
        /// </summary>
        /// <param name="agencyCode">service provider code</param>
        /// <returns>password requirement</returns>
        string GetPasswordRequirement(string agencyCode);
               
        /// <summary>
        /// public user model.
        /// </summary>
        /// <param name="userSeqNum">the user sequence number.</param>
        /// <returns>the public user model.</returns>
        PublicUserModel4WS GetPublicUser(string userSeqNum);

        /// <summary>
        /// Merge source user's properties which value are not null to destination user model.
        /// </summary>
        /// <param name="source">Source public user model.</param>
        /// <param name="destination">Destination public user model.</param>
        void MergePublicUserInfo(PublicUserModel4WS source, ref PublicUserModel4WS destination);

        /// <summary>
        /// Validate user when login in ACA
        /// </summary>
        /// <param name="userName">validation's user name</param>
        /// <param name="password">validation's user password</param>
        /// <returns>Result model</returns>
        PublicUserModel4WS ValidateUser(string userName, string password);

        /// <summary>
        /// Validate public user account status
        /// </summary>
        /// <param name="servProvCode">Service Provider Code</param>
        /// <param name="emailOrUserID">Email or User ID</param>
        /// <returns>
        /// return null if the account doesn't exist in DB. 
        /// return public user sequence number if the account exists in DB.
        /// </returns>
        string ValidatePublicUserAccount(string servProvCode, string emailOrUserID);

        /// <summary>
        /// set public user account information to people model.
        /// </summary>
        /// <param name="publicUser">the public user model.</param>
        /// <param name="people">the people model.</param>
        void SetAccountInfoToPeopleModel(PublicUserModel4WS publicUser, ref PeopleModel4WS people);

        /// <summary>
        /// Edit authorized agent clerk.
        /// </summary>
        /// <param name="userModel">public user model</param>
        /// <param name="callerID">public user id</param>
        void EditAuthAgentClerk(PublicUserModel4WS userModel, string callerID);

        /// <summary>
        /// Deactivate clerk.
        /// </summary>
        /// <param name="servProvCode">agency code</param>
        /// <param name="uuId">uUID number</param>
        /// <returns>deactivate user name</returns>
        string DeactivateUserForClerk(string servProvCode, string uuId);

        /// <summary>
        /// Get the clerk list by agent user.
        /// </summary>
        /// <param name="publicUser">public user model</param>
        /// <returns>clerk user model array</returns>
        PublicUserModel4WS[] GetClerkList(PublicUserModel4WS publicUser);

        /// <summary>
        /// Check the lock-account setting when user input answer wrong.
        /// </summary>
        /// <param name="servProvCode">agency code</param>
        /// <param name="userIdOrEmailId">The user id or email id.</param>
        /// <param name="userSeqNbr">The user sequence NBR.</param>
        /// <param name="isAnswerCorrect">Is answer correct.</param>
        /// <returns>The public user status.</returns>
        string IsLockedUserBySecurityQuestionFail(string servProvCode, string userIdOrEmailId, string userSeqNbr, bool isAnswerCorrect);

        /// <summary>
        /// Update user security questions.
        /// </summary>
        /// <param name="userModel">public user model</param>
        void UpdateSecurityQuestions(PublicUserModel4WS userModel);

        /// <summary>
        /// Get associated licenses by agency code and user id/ email.
        /// </summary>
        /// <param name="servProvCode">Agency code</param>
        /// <param name="userIDOrEmail">User ID or Email</param>
        /// <returns>public user associated license list</returns>
        LicenseModel4WS[] GetPublicUserAssociatedLicenses(string servProvCode, string userIDOrEmail);

        /// <summary>
        /// Get public user model by user email or user id
        /// </summary>
        /// <param name="emailOrUserID">User id or email id</param>
        /// <returns>Public user model</returns>
        PublicUserModel GetPublicUserByEmailOrUserID(string emailOrUserID);

        /// <summary>
        /// Get associated contacts by user email or user id
        /// </summary>
        /// <param name="agencyCode">Agency code</param>
        /// <param name="emailOrUserID">User id or email id</param>
        /// <param name="isIgnoreAgency">Is ignore agency</param>
        /// <returns>associated contacts</returns>
        PeopleModel4WS[] GetContactsByPublicUser(string agencyCode, string emailOrUserID, bool isIgnoreAgency);

        /// <summary>
        /// Activate an existing user account
        /// </summary>
        /// <param name="model">public user information.</param>
        /// <returns>User sequence number</returns>
        string ActiveExistingAccount(PublicUserModel4WS model);

        /// <summary>
        /// Get User token form rest API.
        /// </summary>
        /// <param name="servProvCode">the agency code.</param>
        /// <param name="userID">the user id</param>
        /// <returns>the user token.</returns>
        string GetUserToken(string servProvCode, string userID);

        /// <summary>
        /// Edit the relationship between public user and external user.
        /// </summary>
        /// <param name="userSSOModel">The relationship model between public user and SSO.</param>
        /// <param name="actionType">The action type.</param>
        void EditXPublicUserSSO(XPublicUserSSOModel userSSOModel, string actionType);

        /// <summary>
        /// Get the public user model by the SSO relationship model.
        /// </summary>
        /// <param name="userSSOModel">The relationship model between public user and SSO.</param>
        /// <returns>public user information.</returns>
        PublicUserModel4WS GetPublicUserBySSO(XPublicUserSSOModel userSSOModel);

        #endregion Methods
    }
}