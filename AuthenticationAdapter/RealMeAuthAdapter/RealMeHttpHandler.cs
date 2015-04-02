#region Header

/**
* <pre>
*  Accela Citizen Access
*  File: RealMeHttpHandler.cs
*
*  Accela, Inc.
*  Copyright (C): 2014
*
*  Description: The common functions for RealMe handler.
*
* </pre>
*/

#endregion

using System;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.SessionState;
using Accela.ACA.SSOInterface;
using Accela.ACA.SSOInterface.Constant;
using Accela.ACA.SSOInterface.Model;
using log4net;

namespace Accela.ACA.RealMeAccessGate
{
    /// <summary>
    /// Real me http handler class
    /// </summary>
    public sealed class RealMeHttpHandler : IHttpHandler, IRequiresSessionState
    {
        #region Fields

        /// <summary>
        /// Logger object
        /// </summary>
        private static readonly ILog Log = LogFactory.Instance.GetLogger(typeof(RealMeHttpHandler));

        /// <summary>
        /// The RealMe authorize adapter.
        /// </summary>
        private static readonly RealMeAuthAdapter RealMeAuthAdapter = new RealMeAuthAdapter();

        #endregion

        #region Property

        /// <summary>
        /// Gets a value indicating whether another request can use this instance.
        /// </summary>
        public bool IsReusable
        {
            get
            {
                return false;
            }
        }

        #endregion

        #region Methods

        /// <summary>
        ///  Handle the http request.
        /// </summary>
        /// <param name="context">Current http context.</param>
        public void ProcessRequest(HttpContext context)
        {
            var request = context.Request;
            var queryKeys = request.QueryString.AllKeys;
            var isForAssociation = false;
            bool isFromRealMe = Common.IsYes(request.QueryString[Constant.IS_FROM_REAL_ME]);

            try
            {
                if (context.Session[Constant.TEMPORARY_USER_SESSION] != null)
                {
                    isForAssociation = true;

                    //To resolved the issue that it would not show expired message when click SSO Link more than once.
                    if (!isFromRealMe)
                    {
                        ACAContext.Instance.GetUserBySSOLinkToken(request.QueryString[Constant.EMAIL_QUERY_TOKEN]);
                    }
                }
                else if (queryKeys.Contains(Constant.EMAIL_QUERY_TOKEN))
                {
                    //Stores the public user data to session, if the request came from email link.
                    isForAssociation = true;
                    StorePublicUser(context);

                    if (context.Session[Constant.TEMPORARY_USER_SESSION] == null)
                    {
                        context.Response.StatusCode = Constant.HTTP_STATUS_CODE_404_RESOURCE_NOT_FOUND;
                        return;
                    }
                }
                else if (!isFromRealMe)
                {
                    //It is if neither came from eamil, nor came RealMe that it will shows error code 404.
                    context.Response.StatusCode = Constant.HTTP_STATUS_CODE_404_RESOURCE_NOT_FOUND;
                    return;
                }

                var realMeName = string.Empty;
                var isLogined = GetLoginStatus(ref realMeName);

                //If the current user had logined and need associates with a public user, it gonna to the register page.
                if (isLogined)
                {
                    if (isForAssociation)
                    {
                        CheckForRegister(realMeName, context);
                    }
                    else
                    {
                        Log.Warn("RealMeHttpHandler-> ProcessRequest -> Current user had logined, but not for associating with any public user.");
                        ShowWelcomePageWithMsg(string.Empty);
                    }
                }
                else
                {
                    RealMeAuthAdapter.RedirectToLoginPage();
                }
            }
            catch (Exception exp)
            {
                if (!(exp is ThreadAbortException))
                {
                    Log.Error(exp.Message, exp);
                    ShowWelcomePageWithMsg(exp.Message);
                }
            }
        }

        /// <summary>
        /// Stores the public user data to session.
        /// </summary>
        /// <param name="context">Current http context.</param>
        private static void StorePublicUser(HttpContext context)
        {
            var request = context.Request;
            var token   = request.QueryString[Constant.EMAIL_QUERY_TOKEN];

            if (!string.IsNullOrWhiteSpace(token))
            {
                UserModel userModel = ACAContext.Instance.GetUserBySSOLinkToken(token, true);

                if (userModel == null)
                {
                    if (Log.IsDebugEnabled)
                    {
                        Log.DebugFormat("RealMeHttpHandler-> StorePublicUser: Can not get public user by token({0}).", token);
                    }

                    return;
                }

                context.Session[Constant.TEMPORARY_USER_SESSION] = userModel;
            }
            else
            {
                if (Log.IsDebugEnabled)
                {
                    Log.Debug("RealMeHttpHandler-> StorePublicUser: SSO link token is empty.");
                }

                ShowWelcomePageWithMsg(string.Empty);
            }
        }

        /// <summary>
        /// Gets login status of the current user.
        /// </summary>
        /// <param name="realMeName">The RealMe user account</param>
        /// <returns>Return whether or not login.</returns>
        private static bool GetLoginStatus(ref string realMeName)
        {
            var isLogined           = false;
            var token               = RealMeAuthAdapter.GetSecurityToken();
            var validateLoginResult = RealMeAuthAdapter.ValidateSecurityToken(token);

            if (validateLoginResult != null)
            {
                realMeName = validateLoginResult.UserName;
                isLogined  = EnumUtil<Constant.RealMeLoginStatus>.Parse(validateLoginResult.StatusCode) == Constant.RealMeLoginStatus.SUCCESS;
            }
            else if (Log.IsDebugEnabled)
            {
                JavaScriptSerializer javaScriptSerializer = new JavaScriptSerializer();
                Log.DebugFormat("RealMeHttpHandler-> GetLoginStatus: Validate security token failed.({0})", javaScriptSerializer.Serialize(token));
            }

            return isLogined;
        }

        /// <summary>
        /// It is the RealMe user that had associated with a public user.
        /// </summary>
        /// <param name="name">RealMe user account</param>
        /// <param name="context">Current http context</param>
        private static void CheckForRegister(string name, HttpContext context)
        {
            bool isAssociated = false;

            if (!string.IsNullOrEmpty(name))
            {
                isAssociated = ACAContext.Instance.IsExistSSORelationship(name, Constant.SSO_ACCOUNT_TYPE_REALME);
            }

            if (isAssociated)
            {
                if (Log.IsDebugEnabled)
                {
                    Log.Debug("RealMeHttpHandler-> CheckForRegister: Realme account had associated with a public user.");
                }

                ShowWelcomePageWithMsg(ACAContext.Instance.GetGUITextByKey("aca_sso_associate_publicuser_msg_duplicate"));
            }

            var userModel = context.Session[Constant.TEMPORARY_USER_SESSION] as UserModel;

            if (userModel == null)
            {
                if (Log.IsDebugEnabled)
                {
                    Log.Debug("RealMeHttpHandler-> CheckForRegister: Can not get public user from session.");
                }

                ShowWelcomePageWithMsg(ACAContext.Instance.GetGUITextByKey("aca_sessiontimeout_msg_timeout"));
            }

            userModel.SSOType = Constant.SSO_ACCOUNT_TYPE_REALME;
            userModel.SSOUserName = name;

            context.Session[Constant.TEMPORARY_USER_SESSION] = null;
            ACAContext.Instance.RedirectToRegistration(userModel, string.Empty, true);
        }

        /// <summary>
        /// It going to redirects to welcome page, if passed message is not empty.
        /// </summary>
        /// <param name="message">The message for shows in welcome page.</param>
        private static void ShowWelcomePageWithMsg(string message)
        {
            HttpContext.Current.Session[Constant.TEMPORARY_USER_SESSION] = null;

            if (!string.IsNullOrWhiteSpace(message))
            {
                ACAContext.Instance.ShowMessage(message, true);
            }
            else
            {
                ACAContext.Instance.RedirectWithInIframe(FileUtil.AppendAbsolutePath(SSOConstant.URL_WELCOME_PAGE), true);
            }
        }

        #endregion
    }
}
