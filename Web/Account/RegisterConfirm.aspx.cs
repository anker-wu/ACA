#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: RegisterConfirm.aspx.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2008-2014
 *
 *  Description:
 *
 *  Notes:
 *      $Id: RegisterConfirm.aspx.cs 278555 2014-09-05 08:36:26Z ACHIEVO\james.shi $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using System;
using Accela.ACA.BLL.Account;
using Accela.ACA.Common;
using Accela.ACA.Common.Util;
using Accela.ACA.Web.Common;
using Accela.ACA.Web.UserInfo;
using Accela.ACA.Web.Util;
using Accela.ACA.WSProxy;

namespace Accela.ACA.Web.Account
{
    /// <summary>
    /// Page for register confirmation
    /// </summary>
    public partial class RegisterConfirm : BasePage
    {
        #region Properties

        /// <summary>
        /// Gets or sets user register model
        /// </summary>
        private PublicUserModel4WS UserModel
        {
            get
            {
                return ViewState["RegisterConfirmModel"] as PublicUserModel4WS;
            }

            set
            {
                ViewState["RegisterConfirmModel"] = value;
            }
        }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Raises the page load event
        /// </summary>
        /// <param name="sender">An object that contains the event sender.</param>
        /// <param name="e">A System.EventArgs object containing the event data.</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                IAccountBll accountBll = (IAccountBll)ObjectFactory.GetObject(typeof(IAccountBll));
                string labelKeySuccess = "acc_registerSuccessInfo_label_note";

                if (ValidationUtil.IsYes(Request.QueryString[UrlConstant.IS_FOR_CLERK]))
                {
                    btnLoginNow.Visible = false;

                    if (!AppSession.IsAdmin && AppSession.User.IsAnonymous)
                    {
                        GotoLoginPage();
                    }

                    labelKeySuccess = "aca_authagent_registerclerkcomplete_label_successinfo";
                    lblRegisterSuccessCongratulaterInfo.LabelKey = "aca_authagent_registerclerkcomplete_label_congratulate";
                    licenseInfoTitle.Visible = false;
                    userLicenseList.Visible = false;
                    divBack.Visible = true;
                }

                registerSuccessInfo.Show(MessageType.Success, labelKeySuccess, MessageSeperationType.Bottom);

                UserModel = PeopleUtil.GetPublicUserFromSession();
                string userSeqNum = string.Empty;
                bool hasAssociatedLicense = false;

                if (!AppSession.IsAdmin)
                {
                    /* If the Enabled LoginOn Registration STD is turn on and the current user isAnonymous And is not ldap User then can show the LoginNow button. 
                     * If the Agent User Add Clerk then do not show the login now button.
                     */
                    btnLoginNow.Visible = StandardChoiceUtil.IsEnabledLoginOnRegistration()
                                            && !StandardChoiceUtil.IsEnableLdapAuthentication()
                                            && !ValidationUtil.IsYes(Request.QueryString[UrlConstant.IS_FOR_CLERK]);

                    if (UserModel == null)
                    {
                        userSeqNum = Request[UrlConstant.USER_SEQ_NUM];
                        UserModel = accountBll.GetPublicUser(userSeqNum);
                    }
                    else
                    {
                        userSeqNum = UserModel.userSeqNum;
                    }

                    if (Session[SessionConstant.SESSION_REGISTER_LICENSES] != null)
                    {
                        hasAssociatedLicense = true;
                    }
                    else
                    {
                        ContractorLicenseModel4WS[] contractorLicenses = LicenseUtil.GetContractorLicenseByUserSeqNumber(ConfigManager.AgencyCode, userSeqNum);

                        if (contractorLicenses != null && contractorLicenses.Length > 0)
                        {
                            hasAssociatedLicense = true;
                        }
                    }
                }

                if ((hasAssociatedLicense || AppSession.IsAdmin) && !ValidationUtil.IsYes(Request.QueryString[UrlConstant.IS_FOR_CLERK]))
                {
                    licenseInfoTitle.Visible = true;
                    userLicenseList.DisplayLicenseList(userSeqNum);
                }

                ContactInfo.Display(UserModel);

                /*
                 * Clear all useless sessions.
                 * LDAP registration may be redirect to another page, so needs to clear the sessions before page redirection.
                 */
                AppSession.User.AllContractorLicenses = null;
                Session.Remove(SessionConstant.SESSION_REGISTER_LICENSES);
                PeopleUtil.RemovePublicUserFromSession();
                AppSession.SetContactSessionParameter(null);
                AppSession.SetRegisterContactSessionParameter(null);

                if (UserModel != null && StandardChoiceUtil.IsEnableLdapAuthentication() && !AppSession.IsAdmin)
                {
                    /*
                     * 1. The account created by createPublicUser interface may be is inactive (Depends on Agency's e-mail verification setting).
                     * 2. Call the "Signon4External" method to validate the user status and get the user information. such as the related LP information.
                     * 2. For LDAP authentication, the new registered users should be activated automatically.
                     *    If administrator forgot to open the automatic activation logic, validate the user status to avoid automatic sign on for current user.
                     */

                    try
                    {
                        UserModel = accountBll.Signon4External(ConfigManager.AgencyCode, UserModel.userID, string.Empty);
                        /*
                         * Automatic login for LDAP authentication.
                         * 1. Create public user info to AppSession.
                         * 2. Create authentication ticket.
                         * 3. Redirect user to the requested url if user requested other url, otherwise, stay on current page.
                         */
                        AccountUtil.CreateUserContext(UserModel);
                        bool autoRedirect = Request.QueryString[UrlConstant.RETURN_URL] != null;

                        AccountUtil.CreateAuthTicketAndRedirect(UserModel.userID, autoRedirect);
                    }
                    catch (Exception exp)
                    {
                        registerSuccessInfo.ShowWithText(MessageType.Error, exp.Message, MessageSeperationType.Bottom);
                    }
                }
            }
        }

        /// <summary>
        /// Auto login use register information
        /// </summary>
        /// <param name="sender">The sender object.</param>
        /// <param name="e">The event argument.</param>
        protected void LoginNowButton_Click(object sender, EventArgs e)
        {
            bool isFromNewUi = ValidationUtil.IsYes(Request[UrlConstant.IS_FROM_NEWUI]) && StandardChoiceUtil.IsEnableNewTemplate();
            var expireTime = Session[SessionConstant.SESSION_REGISTRE_USER_MODEL_EXPIRETIME];

            if (isFromNewUi)
            {
                if (expireTime != null && (DateTime)expireTime > DateTime.Now)
                {
                    Session.Remove(SessionConstant.SESSION_REGISTRE_USER_MODEL_EXPIRETIME);
                    string userName = string.Empty;
                    string userPassword = string.Empty;

                    if (UserModel != null)
                    {
                        userName = UserModel.userID;
                        userPassword = UserModel.password;
                    }

                    ClientScript.RegisterStartupScript(ClientScript.GetType(), "Login", "<script>window.parent.Login('" + userName + "','" + userPassword + "');</script>");
                    return; 
                }

                ClientScript.RegisterStartupScript(ClientScript.GetType(), "LoginPage", "<script>window.parent.redirectToLogin();</script>");
                return; 
            }

            try
            {
                if (expireTime != null && (DateTime)expireTime > DateTime.Now)
                {
                    Session.Remove(SessionConstant.SESSION_REGISTRE_USER_MODEL_EXPIRETIME);

                    if (UserModel != null
                        && !string.IsNullOrEmpty(UserModel.userID)
                        && !string.IsNullOrEmpty(UserModel.password))
                    {
                        PublicUserModel4WS publicUser = AuthenticationUtil.ValidateUser(UserModel.userID, UserModel.password);

                        bool isLdapEnabled = StandardChoiceUtil.IsEnableLdapAuthentication();
                        AccountUtil.CreateUserContext(publicUser);
                        UserUtil.UserLogin(AppSession.User, isLdapEnabled);
                    }
                }

                Response.Redirect("~/Login.aspx");
            }
            catch (ACAException ex)
            {
                string errorMessage = AccountUtil.GetErrorMessageByErrorCode(ex.Message);
                registerSuccessInfo.ShowWithText(MessageType.Error, errorMessage, MessageSeperationType.Bottom);
            }
        }

        #endregion Methods
    }
}