#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: LoginBox.aspx.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2007-2014
 *
 *  Description:
 *
 *  Notes:
 *      $Id: LoginBox.ascx.cs 278445 2014-09-04 05:44:51Z ACHIEVO\james.shi $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using System;
using System.Configuration;
using System.Security.Authentication;
using System.Web;

using Accela.ACA.BLL.Account;
using Accela.ACA.BLL.Common;
using Accela.ACA.Common;
using Accela.ACA.Common.Log;
using Accela.ACA.Common.Util;
using Accela.ACA.Web.Common;
using Accela.ACA.Web.UserInfo;
using Accela.ACA.Web.Util;
using Accela.ACA.WSProxy;
using Accela.ACA.WSProxy.WSModel;
using log4net;

namespace Accela.ACA.Web.Component
{
    /// <summary>
    /// This class provide the ability to operation LoginBox. 
    /// </summary>
    public partial class LoginBox : BaseUserControl
    {
        /* ===================================================
         * This page in only used for internal authentication.
         * ===================================================*/

        #region Fields

        /// <summary>
        /// Add a mechanism to prevent resubmit credential(userId and password) info for security reason -- see below scenario:
        /// 1. A user account had been disabled.
        /// 2. The user enter the userId and password and try to login, system prompts the error and this user does not close the login window.
        /// 3. Another user to use ACA, and just now administrator enabled the previous user account.
        /// 4. Because the browser stored the form data, so the second user just need to refresh the page will user previous user account to logged into ACA.
        /// </summary>
        private const string SESSIONKEY_PREVENT_RESUBMISSION = "PREVENT_RESUBMIT_CREDENTIAL_FOR_LOGIN";

        /// <summary>
        /// Loggers object.
        /// </summary>
        private static readonly ILog Logger = LogFactory.Instance.GetLogger(typeof(LoginBox));

        #endregion Fields

        #region Properties

        /// <summary>
        /// Gets or sets user name text box.
        /// </summary>
        public string TextUserName
        {
            get
            {
                return txtUserId.Text;
            }

            set
            {
                txtUserId.Text = value;
            }
        }

        /// <summary>
        /// Gets or sets the validate resubmit session to prevent resubmit credential info for security reason.
        /// </summary>
        /// <value>
        /// The validate resubmit session.
        /// </value>
        private string ValidateResubmitSession
        {
            get
            {
                return Session[SESSIONKEY_PREVENT_RESUBMISSION] == null
                           ? string.Empty
                           : Session[SESSIONKEY_PREVENT_RESUBMISSION].ToString();
            }

            set
            {
                Session[SESSIONKEY_PREVENT_RESUBMISSION] = value;
                hdnValidateResubmit.Value = value;
            }
        }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Handle the <c>Init</c> event.
        /// </summary>
        /// <param name="e">Event argument</param>
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            /*
             * For Security reason, the session ID should be changed after user's success login.
             */
            var cacheManager = ObjectFactory.GetObject<ICacheManager>();
            string cacheKey = Session.SessionID;
            User user = cacheManager.GetSingleCachedItem(cacheKey) as User;

            if (user != null)
            {
                AppSession.User = user;
                AccountUtil.ApplyUserInfoToContext(user, Context);
                cacheManager.Remove(cacheKey);
                ProcessUserLogin();
            }
        }

        /// <summary>
        /// Raises the page load event
        /// </summary>
        /// <param name="sender">An object that contains the event sender.</param>
        /// <param name="e">A System.EventArgs object containing the event data.</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //if registration links is disabled, hidden the registration relevant information.
                if (!StandardChoiceUtil.IsRegistrationEnabled())
                {
                    divRegisterAccount.Visible = false;
                }

                //Hide the Forgot password link if LDAP authentication is enabled.
                if (StandardChoiceUtil.IsEnableLdapAuthentication())
                {
                    divForgotPassword.Visible = false;
                }

                if (!StandardChoiceUtil.IsEnableCaptchaForLogin())
                {
                    divRecaptcha.Visible = false;
                    txtPassword.Attributes.Add("onkeydown", "triggerLogin(event);");
                }

                GetRememberedUser();
                
                //set the url that forget password and register a new account.
                if (!AppSession.IsAdmin)
                {
                    hrefForgotPWD.HRef = "~/Account/ForgotPassword.aspx";
                    hrefRegisterAccount.HRef = "~/Account/RegisterDisclaimer.aspx";
                    
                    // Auto-fill user name field when from registering an existing current agency
                    string userIdentifier = Request.QueryString[UrlConstant.USER_ID_OR_EMAIL];

                    if (!string.IsNullOrEmpty(userIdentifier))
                    {
                        txtUserId.Text = userIdentifier;
                    }
                }

                //To prevent resubmit credential info for security reason.
                ValidateResubmitSession = CommonUtil.GetRandomUniqueID();
            }

            chkRemember.Attributes.Add("title", LabelUtil.RemoveHtmlFormat(GetTextByKey("acc_sign_label_rememberMe")));
        }

        /// <summary>
        /// Login Link Click
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void LoginButton_Click(object sender, EventArgs e)
        {
            if (StandardChoiceUtil.IsEnableCaptchaForLogin() && !reCaptcha.Validate())
            {
                return;
            }

            /*
             * 1. To prevent resubmit credential info for security reason.
             * 2. When ValidateResubmitSession is empty, it is logout and page jump to login.aspx, there do clear session. So not need do validate resubmit validation
            */
            if (!string.IsNullOrEmpty(ValidateResubmitSession) && !string.Equals(ValidateResubmitSession, hdnValidateResubmit.Value))
            {
                Response.Redirect(Request.Url.AbsoluteUri);
                return;
            }

            ValidateResubmitSession = CommonUtil.GetRandomUniqueID();

            if (Page.IsValid)
            {
                try
                {
                    // Handled with on login EMSE event
                    EMSEOnLoginResultModel4WS resultModel4ws = RunBeforeLoginEMSEEvent();
                    string returnMessage = resultModel4ws != null ? resultModel4ws.returnMessage : string.Empty;
                    string returnCode = resultModel4ws != null ? resultModel4ws.returnCode : string.Empty;

                    if (string.IsNullOrEmpty(returnCode) || !EmseUtil.RETURN_CODE_FOR_DISPLAY_MESSAGE.Equals(returnCode))
                    {
                        AuthBySecurityQuestionModel authBySecurityQuestion = SecurityQuestionUtil.GetAuthBySecurityQuestionSetting();
                        string urlParam4RemenberUser = string.Empty;

                        if (chkRemember.Checked)
                        {
                            string encryptedUserName = SecurityUtil.MachineKeyEncode(txtUserId.Text);
                            urlParam4RemenberUser = "?" + UrlConstant.Remembered_User_Name + "=" + encryptedUserName;
                        }

                        PublicUserModel4WS publicUser = AuthenticationUtil.ValidateUser(txtUserId.Text, txtPassword.Text);

                        //The security question verification function do not support LDAP and SSO.
                        if (authBySecurityQuestion.Enable
                            && AuthenticationUtil.IsInternalAuthAdapter 
                            && !StandardChoiceUtil.IsEnableLdapAuthentication()
                            && SecurityQuestionUtil.IsExistActiveQuestion(publicUser.questions))
                        {
                            Session[SessionConstant.SESSION_USER_FOR_LOGIN_SECURITY] = publicUser;
                            string url = "~/Account/SecurityQuestionVerification.aspx" + urlParam4RemenberUser;
                            UrlHelper.KeepReturnUrlAndRedirect(url);
                        }
                        else
                        {
                            AccountUtil.CreateUserContext(publicUser);

                            //For Security reason, change session ID after user's success login.
                            string url = "~/ChangeSessionID.aspx" + urlParam4RemenberUser;
                            Server.Transfer(url, false);
                        }
                    }
                    else
                    {
                        // handle with the message is empty but it should display in the page.
                        if (string.IsNullOrEmpty(returnMessage))
                        {
                            returnMessage = " ";
                        }

                        MessageUtil.ShowMessage(Page, MessageType.Notice, returnMessage);
                        Page.FocusElement(txtUserId.ClientID);
                    }
                }
                catch (ConfigurationErrorsException configErr)
                {
                    Logger.Error(configErr);

                    //Show configuration error message.
                    MessageUtil.ShowMessage(Page, MessageType.Error, GetTextByKey("aca_system_configuration_error"));
                    Page.FocusElement(txtUserId.ClientID);
                }
                catch (AuthenticationException authErr)
                {
                    Logger.Error(authErr);

                    //Show authentication error message.
                    MessageUtil.ShowMessage(Page, MessageType.Error, GetTextByKey("aca.error.publicuser.login.fail"));
                    Page.FocusElement(txtUserId.ClientID);
                }
                catch (ACAException ex)
                {
                    LoginStatusCode statusCode = EnumUtil<LoginStatusCode>.Parse(ex.Message);

                    if (statusCode == LoginStatusCode.NOTREGISTERED)
                    {
                        string url = string.Format(
                                                   "~/Account/ExistingAccountRegisteration.aspx?{0}={1}&{2}={3}",
                                                   UrlConstant.IS_LOGIN_USE_EXISTING_ACCOUNT,
                                                   ACAConstant.COMMON_Y,
                                                   UrlConstant.USER_ID_OR_EMAIL,
                                                   HttpUtility.UrlEncode(txtUserId.Text));
                        Response.Redirect(url);

                        return;
                    }

                    Logger.Error(ex);
                    string errorMessage = AccountUtil.GetErrorMessageByErrorCode(ex.Message);
                    MessageUtil.ShowMessage(Page, MessageType.Error, errorMessage);
                    Page.FocusElement(txtUserId.ClientID);
                }
            }
        }

        /// <summary>
        /// Handle the login process after the user credentials validate passed.
        /// </summary>
        private void ProcessUserLogin()
        {
            bool isLdapEnabled = StandardChoiceUtil.IsEnableLdapAuthentication();
            User user = AppSession.User;

            /*
             * Used the LDAP authentication, and the user is not existing in Accela system.
             * Save user information to Session and navigate user to registeration process to fill out the required information
             *  and to complete the user creation.
             */
            if (isLdapEnabled && user != null && AccountUtil.IsNewLdapUser(user.UserModel4WS))
            {
                HttpContext.Current.Session.Clear();
                AppSession.IsEditFromLoginFlag = true;
                PeopleUtil.SavePublicUserToSession(user.UserModel4WS);
                UrlHelper.KeepReturnUrlAndRedirect("~/Account/RegisterDisclaimer.aspx");
            }

            //Sets cookie based on Remembered user name.
            UserUtil.CheckRememberMe();
            UserUtil.UserLogin(user, isLdapEnabled);
        }

        /// <summary>
        /// when user check remember me and click login button the user id will be recorded in cookie.
        /// </summary>
        private void GetRememberedUser()
        {
            HttpCookie cookie = Context.Request.Cookies.Get(CookieConstant.REMEMBERED_USER_NAME);

            if (cookie != null && !string.IsNullOrEmpty(cookie.Value))
            {
                try
                {
                    string decryptedUserName = SecurityUtil.MachineKeyDecode(cookie.Value);
                    txtUserId.Text = decryptedUserName;
                    Page.FocusElement(txtPassword.ClientID);
                    chkRemember.Checked = true;
                }
                catch
                {
                    txtUserId.Text = string.Empty;

                    //let the current cookie expire if its user name is failed to decoded
                    cookie.HttpOnly = true;
                    cookie.Expires = DateTime.Now.AddDays(-1);
                    Context.Response.Cookies.Add(cookie);

                    Page.FocusElement(txtUserId.ClientID);
                }
            }
            else
            {
                Page.FocusElement(txtUserId.ClientID);
            }
        }

        /// <summary>
        /// handled with the onLogin EMSE event
        /// </summary>
        /// <returns>a EMSEOnLoginResultModel4WS</returns>
        private EMSEOnLoginResultModel4WS RunBeforeLoginEMSEEvent()
        {
            OnLoginParamsModel4WS paramsModel = new OnLoginParamsModel4WS();
            paramsModel.username = txtUserId.Text.Trim();

            // the AppSession.User.UserID is empty before the validation username and password.
            return EmseUtil.RunEMSEScriptOnLogin(ACAConstant.EMSE_BEFORE_LOGON, null, paramsModel);
        }

        #endregion Methods
    }
}
