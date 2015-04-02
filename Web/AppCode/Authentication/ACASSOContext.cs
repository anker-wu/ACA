#region Header

/**
 *  Accela Citizen Access
 *  File: AuthenticationAPI.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2014
 *
 *  Description:
 *
 *  Notes:
 * $Id: AuthenticationAPI.cs 131314 2009-08-31 06:07:41Z ACHIEVO\jackie.yu $.
 *  Revision History
 *  Date,                  Who,                 What
 */

#endregion Header

using System;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;

using Accela.ACA.BLL.Account;
using Accela.ACA.Common;
using Accela.ACA.SSOInterface;
using Accela.ACA.SSOInterface.Constant;
using Accela.ACA.SSOInterface.Model;
using Accela.ACA.Web.Common;
using Accela.ACA.WSProxy;

namespace Accela.ACA.Web.Authentication
{
    /// <summary>
    /// Class AuthenticationAPI
    /// </summary>
    public class ACASSOContext : IACASSOContext
    {
        #region Properties

        /// <summary>
        /// Gets or sets the pre public user account creation event handler.
        /// </summary>
        public UserCreation PrePublicUserCreation
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the post public user account creation event handler.
        /// </summary>
        public UserCreation PostPublicUserCreation
        {
            get;
            set;
        }

        /// <summary>
        /// Gets a value indicating whether current user is anonymous user.
        /// </summary>
        public bool IsAnonymousUser
        {
            get
            {
                return AppSession.User.IsAnonymous;
            }
        }

        /// <summary>
        /// Gets the correct protocol to resolve one conflict protocol issue related to load balance env.
        /// </summary>
        public string Protocol
        {
            get
            {
                return ConfigManager.Protocol;
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Check whether existing relationship between SSO and public user.
        /// </summary>
        /// <param name="externalUserName">external user name.</param>
        /// <param name="accountType">account type.</param>
        /// <returns>If existing relationship between SSO and public user then return true; else return false.</returns>
        public bool IsExistSSORelationship(string externalUserName, string accountType)
        {
            IAccountBll accountBll = ObjectFactory.GetObject<IAccountBll>();
            PublicUserModel4WS publicUserModel = accountBll.Signon4External(
                ConfigManager.AgencyCode,
                externalUserName,
                accountType);
            return publicUserModel != null;
        }

        /// <summary>
        /// Check whether the SSO user has associated with public user, if exist, create user context.
        /// </summary>
        /// <param name="externalUserName">external user name.</param>
        /// <param name="accountType">account type.</param>
        /// <returns>User model</returns>
        public UserModel CreateUserContext(string externalUserName, string accountType)
        {
            IAccountBll accountBll = ObjectFactory.GetObject<IAccountBll>();
            PublicUserModel4WS publicUserModel = accountBll.Signon4External(
                ConfigManager.AgencyCode,
                externalUserName,
                accountType);

            if (publicUserModel == null)
            {
                return null;
            }

            AccountUtil.CreateUserContext(publicUserModel);
            UserModel userModel = ModelConverter.ConvertToUserModel4SSO(publicUserModel);
            userModel.SSOType = accountType;
            userModel.SSOUserName = externalUserName;

            //Create authentication cookie for specified user.
            FormsAuthentication.SetAuthCookie(publicUserModel.userID, false);

            return userModel;
        }

        /// <summary>
        /// Redirect to registration page according current request url.
        /// </summary>
        /// <param name="userModel">User model</param>
        /// <param name="returnUrl">url to be redirect after login success</param>
        /// <param name="isWrapFrame">is wrap frame</param>
        public void RedirectToRegistration(
            UserModel userModel,
            string returnUrl,
            bool isWrapFrame = false)
        {
            IAccountBll accountBll = ObjectFactory.GetObject<IAccountBll>();
            HttpResponse response = HttpContext.Current.Response;
            string amcaUrl = response.ApplyAppPathModifier(SSOConstant.AMCA_DEFAULT_PAGE);
            PublicUserModel4WS publicUserModel = null;

            if (userModel != null)
            {
                //from SSO link.
                if (!string.IsNullOrWhiteSpace(userModel.UserSeqNum)
                    && userModel.UserSeqNum != ACAConstant.ANONYMOUS_FLAG)
                {
                    publicUserModel = accountBll.GetPublicUser(userModel.UserSeqNum);
                    publicUserModel.SSOType = userModel.SSOType;
                    publicUserModel.SSOUserName = userModel.SSOUserName;
                }
                else
                {
                    publicUserModel = ModelConverter.ConvertToPublicUserModel4WS(userModel);
                }

                AppSession.IsEditFromLoginFlag = true;
                PeopleUtil.SavePublicUserToSession(publicUserModel);
            }

            if (IsAmcaRequest())
            {
                amcaUrl += string.Format(
                    "?{0}={1}",
                    UrlConstant.IS_EXTERNAL_ACCOUNT_ASSOCIATED_PUBLICUSER,
                    ACAConstant.COMMON_N);
                response.Redirect(amcaUrl);
            }
            else
            {
                string acaRegisterUrl = FileUtil.AppendApplicationRoot(SSOConstant.ACA_REGISTER_URL);

                if (!string.IsNullOrEmpty(returnUrl))
                {
                    acaRegisterUrl += string.Format(
                        "?{0}={1}",
                        UrlConstant.RETURN_URL,
                        HttpUtility.UrlEncode(returnUrl));
                }

                RedirectWithInIframe(acaRegisterUrl, isWrapFrame);
            }
        }

        /// <summary>
        /// Get the user model by SSO link token.
        /// </summary>
        /// <param name="token">SSO link token</param>
        /// <param name="isExpireSSOLink">is expire SSO link</param>
        /// <returns>User model.</returns>
        public UserModel GetUserBySSOLinkToken(string token, bool isExpireSSOLink = false)
        {
            IAccountBll accountBll = ObjectFactory.GetObject<IAccountBll>();
            PublicUserModel4WS publicUserModel =
                accountBll.GetPublicUserBySSO(
                    new XPublicUserSSOModel { publicUserUUID = token, serviceProviderCode = ConfigManager.AgencyCode, });

            if (isExpireSSOLink)
            {
                SimpleAuditModel auditModel = new SimpleAuditModel();
                auditModel.auditID = publicUserModel.auditID;

                var xpublicUserSsoModel = new XPublicUserSSOModel();
                xpublicUserSsoModel.publicUserUUID = token;
                xpublicUserSsoModel.serviceProviderCode = ConfigManager.AgencyCode;
                xpublicUserSsoModel.auditModel = auditModel;

                accountBll.EditXPublicUserSSO(xpublicUserSsoModel, SSOActionType.Expire);
            }

            return ModelConverter.ConvertToUserModel4SSO(publicUserModel);
        }

        /// <summary>
        /// Get the GUI text by label key.
        /// </summary>
        /// <param name="labelKey">label key</param>
        /// <returns>label key value</returns>
        public string GetGUITextByKey(string labelKey)
        {
            return LabelUtil.GetTextByKey(labelKey, string.Empty);
        }

        /// <summary>
        /// Redirect to current url within frame.
        /// </summary>
        /// <param name="currentUrl">current url.</param>
        /// <param name="isWrapFrame">is wrap frame</param>
        public void RedirectWithInIframe(string currentUrl, bool isWrapFrame = false)
        {
            HttpResponse response = HttpContext.Current.Response;
            HttpSessionState session = HttpContext.Current.Session;

            if (isWrapFrame)
            {
                session[ACAConstant.CURRENT_URL] = currentUrl;
                response.Redirect(ACAConstant.URL_DEFAULT_PAGE);
            }
            else
            {
                response.Redirect(currentUrl);
            }
        }

        /// <summary>
        /// Show message to ACA default page.
        /// </summary>
        /// <param name="msg">Error message.</param>
        /// <param name="isForceRedirectDefault">Whether redirect to default page</param>
        public void ShowMessage(string msg, bool isForceRedirectDefault = false)
        {
            if (!string.IsNullOrWhiteSpace(msg))
            {
                string currentUrl = string.Empty;

                if (IsAmcaRequest())
                {
                    isForceRedirectDefault = false;
                    currentUrl = string.Format("{0}?login=failed&Message={1}", SSOConstant.AMCA_DEFAULT_PAGE, HttpUtility.HtmlEncode(msg));
                }
                else
                {
                    currentUrl = string.Format(
                       "{0}?{1}={2}&{3}={4}",
                       FileUtil.AppendApplicationRoot(ACAConstant.URL_WELCOME_PAGE),
                       UrlConstant.RETURN_MESSAGE,
                       HttpUtility.HtmlEncode(msg),
                       UrlConstant.MESSAGE_TYPE,
                       MessageType.Error);
                }

                RedirectWithInIframe(currentUrl, isForceRedirectDefault);
            }
        }

        /// <summary>
        /// Get a value which indicates whether the specified HTTP request comes from Accela Mobile Citizen Access
        /// (AMCA).AMCA is a web application which is embedded in Accela Citizen Access. AMCA does not support anonymous user while Accela Citizen Access does.
        /// </summary>
        /// <returns>True for AMCA request, otherwise false.</returns>
        public bool IsAmcaRequest()
        {
            return IsAmcaRequest(HttpContext.Current.Request.Url);
        }

        /// <summary>
        /// Get a value which indicates whether the specified HTTP request comes from Accela Mobile Citizen Access
        /// (AMCA).AMCA is a web application which is embedded in Accela Citizen Access. AMCA does not support anonymous user while Accela Citizen Access does.
        /// </summary>
        /// <param name="uri">Service URI.</param>
        /// <returns>True for AMCA request, otherwise false.</returns>
        public bool IsAmcaRequest(Uri uri)
        {
            if (uri == null)
            {
                return false;
            }

            HttpRequest request = HttpContext.Current.Request;
            string amcaPath = FileUtil.CombineWebPath(request.ApplicationPath, "amca");
            return uri.AbsolutePath.StartsWith(amcaPath, StringComparison.OrdinalIgnoreCase);
        }

        /// <summary>
        /// Check current request url whether is raw request, rather than application error handle page request.
        /// </summary>
        /// <returns>True: raw request, False: application error handle page request.</returns>
        public bool IsRawRequest()
        {
            string requestPath = HttpContext.Current.Request.Url.AbsolutePath;
            string[] queryStrings = HttpContext.Current.Request.QueryString.AllKeys;

            return !requestPath.EndsWith("Error.aspx", StringComparison.InvariantCultureIgnoreCase)
                   && !queryStrings.Contains(UrlConstant.RETURN_MESSAGE)
                   && !queryStrings.Contains(UrlConstant.RETURN_MESSAGE_KEY);
        }

        #endregion Methods
    }
}