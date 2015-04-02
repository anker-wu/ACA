#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: AccelaMemberShipProvider.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2008-2014
 *
 *  Description:
 *
 *  Notes:
 *
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using System;
using System.Web.Security;

using Accela.ACA.BLL.Account;
using Accela.ACA.Common;
using Accela.ACA.Web.Util;
using Accela.ACA.WSProxy;

namespace Accela.ACA.Web.Common
{
    /// <summary>
    /// AccelaMemberShipProvider -- Custom Membership Provider for ACA
    /// </summary>
    public class AccelaMemberShipProvider : MembershipProvider
    {
        #region Properties

        /// <summary>
        /// Gets or sets application name
        /// </summary>
        public override string ApplicationName
        {
            get
            {
                return ACAConstant.PRODUCT_NAME;
            }

            set
            {
            }
        }

        /// <summary>
        /// Gets a value indicating whether password reset is enabled
        /// </summary>
        public override bool EnablePasswordReset
        {
            get
            {
                return true;
            }
        }

        /// <summary>
        /// Gets a value indicating whether password retrieval is enabled
        /// </summary>
        public override bool EnablePasswordRetrieval
        {
            get
            {
                return true;
            }
        }

        /// <summary>
        /// Gets max invalid password attempts
        /// </summary>
        public override int MaxInvalidPasswordAttempts
        {
            get
            {
                return 10;
            }
        }

        /// <summary>
        /// Gets min required non alpha number chars
        /// </summary>
        public override int MinRequiredNonAlphanumericCharacters
        {
            get
            {
                return 0;
            }
        }

        /// <summary>
        /// Gets min required password length
        /// </summary>
        public override int MinRequiredPasswordLength
        {
            get
            {
                return 1;
            }
        }

        /// <summary>
        /// Gets password attempt window
        /// </summary>
        public override int PasswordAttemptWindow
        {
            get
            {
                return 10;
            }
        }

        /// <summary>
        /// Gets password format
        /// </summary>
        public override MembershipPasswordFormat PasswordFormat
        {
            get
            {
                return MembershipPasswordFormat.Clear;
            }
        }

        /// <summary>
        /// Gets password strength regular expression
        /// </summary>
        public override string PasswordStrengthRegularExpression
        {
            get
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// Gets a value indicating whether question and answer is required
        /// </summary>
        public override bool RequiresQuestionAndAnswer
        {
            get
            {
                return true;
            }
        }

        /// <summary>
        /// Gets a value indicating whether unique email is required
        /// </summary>
        public override bool RequiresUniqueEmail
        {
            get
            {
                return true;
            }
        }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Change password
        /// </summary>
        /// <param name="username">user name.</param>
        /// <param name="oldPassword">old password</param>
        /// <param name="newPassword">new password</param>
        /// <returns>"The method or operation is not implemented."</returns>
        public override bool ChangePassword(string username, string oldPassword, string newPassword)
        {
            return true;
        }

        /// <summary>
        /// Change password question and answer
        /// </summary>
        /// <param name="username">user name.</param>
        /// <param name="password">old password</param>
        /// <param name="newPasswordQuestion">The new password question.</param>
        /// <param name="newPasswordAnswer">The new password answer.</param>
        /// <returns>"The method or operation is not implemented."</returns>
        public override bool ChangePasswordQuestionAndAnswer(string username, string password, string newPasswordQuestion, string newPasswordAnswer)
        {
            return true;
        }

        /// <summary>
        /// Adds a new membership user to the data source.
        /// </summary>
        /// <param name="username"> The user name for the new user.</param>
        /// <param name="password">The password for the new user.</param>
        /// <param name="email">The e-mail address for the new user.</param>
        /// <param name="passwordQuestion">The password question for the new user.</param>
        /// <param name="passwordAnswer">The password answer for the new user</param>
        /// <param name="isApproved">Whether or not the new user is approved to be validated.</param>
        /// <param name="providerUserKey">The unique identifier from the membership data source for the user.</param>
        /// <param name="status">A System.Web.Security.MembershipCreateStatus enumeration value indicating
        /// whether the user was created successfully.</param>
        /// <returns>"The method or operation is not implemented."</returns>
        public override MembershipUser CreateUser(string username, string password, string email, string passwordQuestion, string passwordAnswer, bool isApproved, object providerUserKey, out MembershipCreateStatus status)
        {
            throw new ACAException("The method or operation is not implemented.");
        }

        /// <summary>
        /// Removes a user from the membership data source.
        /// </summary>
        /// <param name="username">The name of the user to delete.</param>
        /// <param name="deleteAllRelatedData">true to delete data related to the user from the database; false to leave
        /// data related to the user in the database.</param>
        /// <returns>"The method or operation is not implemented."</returns>
        public override bool DeleteUser(string username, bool deleteAllRelatedData)
        {
            return true;
        }

        /// <summary>
        /// Gets a collection of membership users where the e-mail address contains the
        /// specified e-mail address to match.
        /// </summary>
        /// <param name="emailToMatch">The e-mail address to search for.</param>
        /// <param name="pageIndex"> The index of the page of results to return. pageIndex is zero-based.</param>
        /// <param name="pageSize">The size of the page of results to return.</param>
        /// <param name="totalRecords">The total number of matched users.</param>
        /// <returns>"The method or operation is not implemented."</returns>
        public override MembershipUserCollection FindUsersByEmail(string emailToMatch, int pageIndex, int pageSize, out int totalRecords)
        {
            throw new ACAException("The method or operation is not implemented.");
        }

        /// <summary>
        /// Gets a collection of membership users where the user name contains the specified user name to match.
        /// </summary>
        /// <param name="usernameToMatch">The user name to search for.</param>
        /// <param name="pageIndex">The index of the page of results to return. pageIndex is zero-based.</param>
        /// <param name="pageSize">The size of the page of results to return.</param>
        /// <param name="totalRecords">The total number of matched users.</param>
        /// <returns>A MembershipUserCollection collection that contains a page of pageSize MembershipUser objects beginning at the page specified by pageIndex.</returns>
        public override MembershipUserCollection FindUsersByName(string usernameToMatch, int pageIndex, int pageSize, out int totalRecords)
        {
            throw new ACAException("The method or operation is not implemented.");
        }

        /// <summary>
        /// Gets a collection of all the users in the data source in pages of data.
        /// </summary>
        /// <param name="pageIndex">The index of the page of results to return. pageIndex is zero-based.</param>
        /// <param name="pageSize">The size of the page of results to return.</param>
        /// <param name="totalRecords">The total number of matched users.</param>
        /// <returns>A MembershipUserCollection collection that contains a page of pageSize MembershipUser objects beginning at the page specified by pageIndex.</returns>
        public override MembershipUserCollection GetAllUsers(int pageIndex, int pageSize, out int totalRecords)
        {
            throw new ACAException("The method or operation is not implemented.");
        }

        /// <summary>
        /// Gets the number of users currently accessing the application.
        /// </summary>
        /// <returns>The number of users currently accessing the application.</returns>
        public override int GetNumberOfUsersOnline()
        {
            return 100;
        }

        /// <summary>
        /// Gets the password for the specified user name from the data source.
        /// </summary>
        /// <param name="username">The user to retrieve the password for. </param>
        /// <param name="answer">The password answer for the user.</param>
        /// <returns>The password for the specified user name.</returns>
        public override string GetPassword(string username, string answer)
        {
            throw new ACAException("The method or operation is not implemented.");
        }

        /// <summary>
        /// Overloaded. Gets information from the data source for a membership user.
        /// </summary>
        /// <param name="username">The name of the user to get information for. </param>
        /// <param name="userIsOnline">true to update the last-activity date/time stamp for the user; false to return user information without updating the last-activity date/time stamp for the user. </param>
        /// <returns>A MembershipUser object populated with the specified user's information from the data source.</returns>
        public override MembershipUser GetUser(string username, bool userIsOnline)
        {
            throw new ACAException("The method or operation is not implemented.");
        }

        /// <summary>
        /// Gets user information from the data source based on the unique identifier for the membership user. Provides an option to update the last-activity date/time stamp for the user.
        /// </summary>
        /// <param name="providerUserKey">The unique identifier for the membership user to get information for.</param>
        /// <param name="userIsOnline">true to update the last-activity date/time stamp for the user; false to return user information without updating the last-activity date/time stamp for the user.</param>
        /// <returns>A MembershipUser object populated with the specified user's information from the data source.</returns>
        public override MembershipUser GetUser(object providerUserKey, bool userIsOnline)
        {
            throw new ACAException("The method or operation is not implemented.");
        }

        /// <summary>
        /// Gets the user name associated with the specified e-mail address.
        /// </summary>
        /// <param name="email">The e-mail address to search for. </param>
        /// <returns>The user name associated with the specified e-mail address. If no match is found, return null reference.</returns>
        public override string GetUserNameByEmail(string email)
        {
            throw new ACAException("The method or operation is not implemented.");
        }

        /// <summary>
        /// Resets a user's password to a new, automatically generated password.
        /// </summary>
        /// <param name="username">The user to reset the password for.</param>
        /// <param name="answer">The password answer for the specified user.</param>
        /// <returns>The new password for the specified user.</returns>
        public override string ResetPassword(string username, string answer)
        {
            throw new ACAException("The method or operation is not implemented.");
        }

        /// <summary>
        /// Clears a lock so that the membership user can be validated.
        /// </summary>
        /// <param name="userName">The membership user whose lock status you want to clear.</param>
        /// <returns>true if the membership user was successfully unlocked; otherwise, false.</returns>
        public override bool UnlockUser(string userName)
        {
            throw new ACAException("The method or operation is not implemented.");
        }

        /// <summary>
        /// Updates information about a user in the data source.
        /// </summary>
        /// <param name="user">A MembershipUser object that represents the user to update and the updated information for the user.</param>
        public override void UpdateUser(MembershipUser user)
        {
            throw new ACAException("The method or operation is not implemented.");
        }

        /// <summary>
        /// Verifies that the specified user name and password exist in the data source.
        /// </summary>
        /// <param name="username">The name of the user to validate.</param>
        /// <param name="password">The password for the specified user.</param>
        /// <returns>true if the specified username and password are valid; otherwise, false.</returns>
        public override bool ValidateUser(string username, string password)
        {
            PublicUserModel4WS publicUser = null;

            try
            {
                publicUser = AuthenticationUtil.ValidateUser(username, password);
            }
            catch (ACAException exp)
            {
                string errorMessage = AccountUtil.GetErrorMessageByErrorCode(exp.Message);
                throw new ACAException(new Exception(errorMessage));
            }
           
            //Create public user info to AppSession after validate passed.
            AccountUtil.CreateUserContext(publicUser);

            return true;
        }

        #endregion Methods
    }
}