#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: UserUtil.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2010-2014
 *
 *  Description:
 *  Interface define for admin.
 *
 *  Notes:
 * $Id: UserUtil.cs 181867 2010-10-06 08:06:18Z ACHIEVO\daly.zeng $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using System;
using System.Web;
using System.Web.Security;

using Accela.ACA.BLL.Account;
using Accela.ACA.Common;
using Accela.ACA.Common.Util;
using Accela.ACA.Web.Common;
using Accela.ACA.Web.Util;
using Accela.ACA.WSProxy;

namespace Accela.ACA.Web.UserInfo
{
    /// <summary>
    /// user utility
    /// </summary>
    public static class UserUtil
    {
        /// <summary>
        /// get the user's initial or the user's name. if the biz domain is true, return the user's initial,
        /// otherwise return the user's initial or the user's name by the option the user selected.
        /// </summary>
        /// <param name="user">SysUserModel object</param>
        /// <returns>string user name</returns>
        public static string GetUserName(SysUserModel user)
        {
            if (user == null)
            {
                return string.Empty;
            }

            // if enable to display user initial name, which means all users will display the initial name.
            if (StandardChoiceUtil.IsDisplayUserInitial())
            {
                return I18nStringUtil.GetString(user.resInitial, user.initial, user.resFullName, user.fullName);
            }
            else
            {
                if (user.displayInitial)
                {
                    return I18nStringUtil.GetString(user.resInitial, user.initial, user.resFullName, user.fullName);
                }
                else
                {
                    return I18nStringUtil.GetString(user.resFullName, user.fullName);
                }
            }
        }

        /// <summary>
        /// format to the full name
        /// </summary>
        /// <param name="firstName">The first name.</param>
        /// <param name="middleName">Name of the middle.</param>
        /// <param name="lastName">The last name.</param>
        /// <returns>the full name</returns>
        public static string FormatToFullName(string firstName, string middleName, string lastName)
        {
            string result = string.Empty;

            switch (I18nCultureUtil.UserPreferredCulture.ToLowerInvariant())
            {
                case "zh-cn":
                case "zh-hk":
                case "zh-mo":
                case "zh-sg":
                case "zh-tw":
                    result = DataUtil.ConcatStringWithSplitChar(new string[] { lastName, middleName, firstName }, ACAConstant.BLANK);
                    break;
                default:
                    result = DataUtil.ConcatStringWithSplitChar(new string[] { firstName, middleName, lastName }, ACAConstant.BLANK);
                    break;
            }

            return result;
        }

        /// <summary>
        /// Users the login.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <param name="isLdapEnabled">if set to true then is LDAP enabled.</param>
        /// <param name="isFromNewUi">is from new ui</param>
        public static void UserLogin(User user, bool isLdapEnabled, bool isFromNewUi = false)
        {
            bool needChangePassword = user != null &&
                                      (ValidationUtil.IsYes(user.UserModel4WS.needChangePassword) ||
                                       AccountUtil.IsPasswordExpiration(user.UserModel4WS));
            string fromNewUi = isFromNewUi ? ACAConstant.COMMON_Y : ACAConstant.COMMON_N;

            /*
             * Use the internal authentication and needs to change password.
             * Will navigate user to change password process.
             */
            if (!isLdapEnabled && !AuthenticationUtil.IsNeedRegistration && needChangePassword)
            {
                // Clear the authentication ticket
                FormsAuthentication.SignOut();

                // Clear the contents of their session
                HttpContext.Current.Session.Clear();

                string isPasswordExpires = ACAConstant.COMMON_Y.Equals(user.UserModel4WS.needChangePassword) ? ACAConstant.COMMON_N : ACAConstant.COMMON_Y;
                string changePasswordUrl = string.Format("~/Account/ChangePassword.aspx?IsPasswordExpires={0}&userID={1}&isFromNewUi={2}", isPasswordExpires, HttpUtility.UrlEncode(user.UserModel4WS.userID), fromNewUi);

                UrlHelper.KeepReturnUrlAndRedirect(changePasswordUrl);
            }
            else
            {
                if (user != null)
                {
                    PublicUserModel4WS publicUserModel = AppSession.User.UserModel4WS;

                    if (!isLdapEnabled
                        && !AuthenticationUtil.IsNeedRegistration
                        && SecurityQuestionUtil.IsNeedUpdateUserQuestions(publicUserModel.questions))
                    {
                        //Clear the authentication ticket and clear the current session.
                        FormsAuthentication.SignOut();
                        HttpContext.Current.Session.Clear();

                        HttpContext.Current.Session[SessionConstant.SESSION_USER_FOR_LOGIN_SECURITY] = publicUserModel;
                        string url = string.Format("~/Account/SecurityQuestionUpdate.aspx?isFromNewUi={0}", fromNewUi);
                        UrlHelper.KeepReturnUrlAndRedirect(url);
                    }
                    else
                    {
                        //Create authentication ticket and redirect user to requested location.
                        AccountUtil.CreateAuthTicketAndRedirect(user.UserID, !isFromNewUi);
                    }
                }
            }
        }

        /// <summary>
        /// Force login redirect for deep link.
        /// </summary>
        public static void ForceLoginForDeepLink()
        {
            string loginUrl = AuthenticationUtil.LoginUrl;

            if (loginUrl.IndexOf('?') >= 0)
            {
                loginUrl += "&";
            }
            else
            {
                loginUrl += "?";
            }

            loginUrl += UrlConstant.RETURN_URL + "=" + HttpUtility.UrlEncode(HttpContext.Current.Request.Url.PathAndQuery);

            UrlHelper.RedirectForDeepLink(loginUrl);
        }

        /// <summary>
        /// Remember/forget user name.
        /// </summary>
        public static void CheckRememberMe()
        {
            string rememberedUserName = HttpContext.Current.Request.QueryString[UrlConstant.Remembered_User_Name];

            if (!string.IsNullOrEmpty(rememberedUserName))
            {
                HttpCookie cookie = new HttpCookie(CookieConstant.REMEMBERED_USER_NAME, rememberedUserName);
                cookie.HttpOnly = true;
                TimeSpan cookiesExistTime = new TimeSpan(2, 0, 0, 0);
                cookie.Expires = DateTime.Now + cookiesExistTime;
                HttpContext.Current.Response.Cookies.Add(cookie);
            }
            else
            {
                // let the cookie expires right now.
                HttpCookie cookie = HttpContext.Current.Response.Cookies[CookieConstant.REMEMBERED_USER_NAME];
                cookie.HttpOnly = true;
                cookie.Expires = DateTime.Now.AddDays(-1);
            }
        }

        /// <summary>
        /// Check unique user name from Javascript of AccountEdit
        /// </summary>
        /// <param name="userID">string user name</param>
        /// <returns>Validation Information</returns>
        public static string IsExistUserName(string userID)
        {
            string agencyCode = ConfigManager.AgencyCode;
            string results = string.Empty;

            IAccountBll accountBll = ObjectFactory.GetObject<IAccountBll>();
            string accountTypeByUserId = accountBll.IsExistingUserID(agencyCode, userID);
            string accountTypeByEmail = accountBll.IsExistingEmailID(agencyCode, userID);

            //The input data can be user name or email address in the textbox of register user.
            if (!string.IsNullOrEmpty(accountTypeByUserId))
            {
                results = string.Format("userId{0}{1}", ACAConstant.SPLIT_CHAR, accountTypeByUserId);
            }

            if (!string.IsNullOrEmpty(accountTypeByEmail))
            {
                results = string.Format("email{0}{1}", ACAConstant.SPLIT_CHAR, accountTypeByEmail);
            }

            return results;
        }

        /// <summary>
        /// Check unique Email address from Javascript of AccountEdit
        /// </summary>
        /// <param name="emailAddress">Email address</param>
        /// <returns>Validation Information</returns>
        public static string IsExistEmail(string emailAddress)
        {
            string agencyCode = ConfigManager.AgencyCode;
            string results = string.Empty;

            IAccountBll accountBll = ObjectFactory.GetObject<IAccountBll>();
            string accountTypeByEmail = accountBll.IsExistingEmailID(agencyCode, emailAddress);
            string accountTypeByUserId = accountBll.IsExistingUserID(agencyCode, emailAddress);

            if (!string.IsNullOrEmpty(accountTypeByEmail))
            {
                results = string.Format("email{0}{1}", ACAConstant.SPLIT_CHAR, accountTypeByEmail);
            }

            if (!string.IsNullOrEmpty(accountTypeByUserId))
            {
                results = string.Format("userId{0}{1}", ACAConstant.SPLIT_CHAR, accountTypeByUserId);
            }

            return results;
        }
    }
}
