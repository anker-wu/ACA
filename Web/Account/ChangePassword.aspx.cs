#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: ChangePassword.ascx.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2010-2014
 *
 *  Description:
 *
 *  Notes:
 *      $Id: ChangePassword.ascx.cs 169604 2010-03-30 09:59:38Z ACHIEVO\daly.zeng $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using System;
using System.Web.Security;
using System.Web.Services;

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
    /// display change password page.
    /// </summary>
    public partial class ChangePassword : BasePage
    {
        /* ===================================================
         * This page in only used for internal authentication.
         * ===================================================*/

        /// <summary>
        /// Check password security
        /// </summary>
        /// <param name="password">The password of the user</param>
        /// <param name="userName">user name for login</param>
        /// <returns>Check result</returns>
        [WebMethod(Description = "Check password security", EnableSession = true)]
        public static string CheckPasswordSecurity(string password, string userName)
        {
            return AccountUtil.CheckPasswordSecurity(password, userName, false);
        }

        /// <summary>
        /// Raises the page load event
        /// </summary>
        /// <param name="sender">An object that contains the event sender.</param>
        /// <param name="e">A System.EventArgs object containing the event data.</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            /*
             * Below scenarios disallow user enter the reset password page:
             * 1. Is external authentication adapter.
             * 2. Is internal authentication adapter but the LDAP authentication is enabled.
             */
            if (!AppSession.IsAdmin && (!AuthenticationUtil.IsInternalAuthAdapter || StandardChoiceUtil.IsEnableLdapAuthentication()))
            {
                Response.Redirect(ACAConstant.URL_DEFAULT);
            }

            bool isCheckPassword = AccountUtil.IsEnablePasswordSecurity();

            if (!Page.IsPostBack)
            {
                if (!AppSession.IsAdmin)
                {
                    if (ACAConstant.COMMON_Y.Equals(Request.QueryString["IsPasswordExpires"]))
                    {
                        string errorMessage = GetTextByKey("aca_change_password_label_expiremessage");
                        MessageUtil.ShowMessage(Page, MessageType.Error, errorMessage);
                    }
                    else
                    {
                        string errorMessage = GetTextByKey("password_update_alert_message");
                        MessageUtil.ShowMessage(Page, MessageType.Error, errorMessage);
                    }

                    txbUserID.Text = Request.QueryString["userID"];
                }

                if (AppSession.IsAdmin || !isCheckPassword)
                {
                    ucPasswordSecurityBar1.Visible = false;
                }
            }

            if (isCheckPassword && !AppSession.IsAdmin)
            {
                txbNewPassword1.CustomValidationFunction = "CheckPasswordSecurity_onblur";
                txbNewPassword1.Validate = "required;customvalidation";
                txbNewPassword1.Attributes.Add("onblur", "password_onblur();");
            }
            else
            {
                txbNewPassword1.CustomValidationFunction = string.Empty;
                txbNewPassword1.Validate = "required;minlength;maxlength";
            }
        }

        /// <summary>
        /// Submit button Click
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void SubmitButton_Click(object sender, EventArgs e)
        {
            string userID = txbUserID.Text.Trim();
            string oldPassword = txbOldPassword.Text.Trim();
            string newPassword = txbNewPassword1.Text.Trim();
            string cofirmPassword = txbNewPassword2.Text.Trim();

            // if new password is not null, new password must equal confirm password.
            if (!newPassword.Equals(cofirmPassword, StringComparison.InvariantCulture))
            {
                string err = GetTextByKey("acc_manage_error_newPassEntry");
                MessageUtil.ShowMessage(Page, MessageType.Error, err);
                return;
            }

            try
            {
                IAccountBll accountBll = (IAccountBll)ObjectFactory.GetObject(typeof(IAccountBll));

                // Validate oldPassword.
                PublicUserModel4WS publicUserModel = accountBll.Signon(ConfigManager.AgencyCode, userID, oldPassword);

                // Update password
                publicUserModel.servProvCode = ConfigManager.AgencyCode;
                publicUserModel.password = newPassword;
                accountBll.EditPublicUser(publicUserModel);

                if (!string.IsNullOrEmpty(publicUserModel.password))
                {
                    accountBll.UpdateNeedChangePassword(ConfigManager.AgencyCode, publicUserModel.userSeqNum, ACAConstant.COMMON_N, ACAConstant.PUBLIC_USER_NAME + publicUserModel.userSeqNum);

                    // Add this line to refresh daily end cache
                    publicUserModel.needChangePassword = ACAConstant.COMMON_N;
                }

                // Relogin and redirct to welcome page
                if (Membership.ValidateUser(userID, newPassword))
                {
                    bool isFromNewUi = ValidationUtil.IsYes(Request[UrlConstant.IS_FROM_NEWUI]) && StandardChoiceUtil.IsEnableNewTemplate();

                    if (SecurityQuestionUtil.IsNeedUpdateUserQuestions(AppSession.User.UserModel4WS.questions))
                    {
                        //Clear the authentication ticket and clear the current session.
                        FormsAuthentication.SignOut();
                        Session.Clear();

                        Session[SessionConstant.SESSION_USER_FOR_LOGIN_SECURITY] = publicUserModel;
                        string fromNewUi = isFromNewUi ? ACAConstant.COMMON_Y : ACAConstant.COMMON_N;
                        string url = string.Format("~/Account/SecurityQuestionUpdate.aspx?isFromNewUi={0}", fromNewUi);
                        UrlHelper.KeepReturnUrlAndRedirect(url);
                    }
                    else
                    {
                        //set value of public user ID for soap header.
                        I18nSoapHeaderExtension.CurrentUser = AppSession.User == null ? string.Empty : AppSession.User.PublicUserId;

                        if (isFromNewUi)
                        {
                            AccountUtil.CreateAuthTicketAndRedirect(AppSession.User.UserID, false);
                            string firstName = string.IsNullOrEmpty(AppSession.User.FirstName) ? string.Empty : AppSession.User.FirstName;
                            string lastName = string.IsNullOrEmpty(AppSession.User.LastName) ? string.Empty : AppSession.User.LastName;
                            ClientScript.RegisterClientScriptBlock(ClientScript.GetType(), "redirectToLaunchpad", "<script>window.parent.redirectToLaunchpad('" + firstName + "','" + lastName + "');</script>");
                        }
                        else
                        {
                            FormsAuthentication.RedirectFromLoginPage(txbUserID.Text, false);
                        }
                    }
                }
            }
            catch (ACAException ex)
            {
                string err = ex.Message;

                if (!string.IsNullOrEmpty(err))
                {
                    err = GetTextByKey("acc_manage_error_oldPassIncorret");
                }

                MessageUtil.ShowMessage(Page, MessageType.Error, err);
            }
        }
    }
}
