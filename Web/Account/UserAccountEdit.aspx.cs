#region Header
/**
 * <pre>
 * 
 *  Accela Citizen Access
 *  File: UserAccountEdit.cs
 * 
 *  Accela, Inc.
 *  Copyright (C): 2011-2014
 * 
 *  Description:
 *  Define UserAccountEdit
 *  Notes:
 *      $Id: UserAccountEdit.cs 133464 2009-06-05 05:06:34Z ACHIEVO\jackie.yu $.
 *  Revision History
 *  &lt;Date&gt;  &lt;Who&gt;  &lt;What&gt;
 * </pre>
 */
#endregion Header

using System;
using System.Web.Services;
using Accela.ACA.BLL.Account;
using Accela.ACA.Common;
using Accela.ACA.Web.Common;
using Accela.ACA.Web.UserInfo;
using Accela.ACA.WSProxy;

namespace Accela.ACA.Web.Account
{
    /// <summary>
    /// UserAccountEdit class
    /// </summary>
    public partial class UserAccountEdit : PopupDialogBasePage
    {
        /// <summary>
        /// Check unique Email address from Javascript of AccountEdit
        /// </summary>
        /// <param name="emailAddress">Email address</param>
        /// <returns>Validation Information</returns>
        [WebMethod(Description = "is existed email", EnableSession = true)]
        public static string IsExistEmail(string emailAddress)
        {
            // To prevent the e-mail validation if the e-mail does not been changed.
            if (emailAddress.Trim().Equals(AppSession.User.UserModel4WS.email, StringComparison.InvariantCultureIgnoreCase))
            {
                return string.Empty;
            }

            return UserUtil.IsExistEmail(emailAddress);
        }

        /// <summary>
        /// Check password security
        /// </summary>
        /// <param name="password">The password of user.</param>
        /// <param name="userName">The user name for login</param>
        /// <param name="isForNewClerk">if set to <c>true</c> [is for new clerk].</param>
        /// <returns>
        /// Check result
        /// </returns>
        [WebMethod(Description = "Check password security", EnableSession = true)]
        public static string CheckPasswordSecurity(string password, string userName, bool isForNewClerk)
        {
            return AccountUtil.CheckPasswordSecurity(password, userName, isForNewClerk);
        }

        /// <summary>
        /// Check unique user name from Javascript of AccountEdit
        /// </summary>
        /// <param name="userID">string user name</param>
        /// <returns>Validation Information</returns>
        [WebMethod(Description = "is existed user name", EnableSession = true)]
        public static string IsExistUserName(string userID)
        {
            return UserUtil.IsExistUserName(userID);
        }

        /// <summary>
        /// page load event handler.
        /// </summary>
        /// <param name="sender">page object</param>
        /// <param name="e">event arguments</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            SetPageTitleKey("acc_manage_label_accountInfo");
            SetDialogMaxHeight("550");

            if (!IsPostBack)
            {
                PublicUserModel4WS model = AppSession.User.UserModel4WS;
                userAccountEdit.Display(model);
            }

            if (AppSession.IsAdmin)
            {
                SetPageTitleVisible(false);
                lblLoginInfo.Visible = true;
                lblLoginInfo.SectionID = ACAConstant.SPLIT_CHAR + GviewID.UserAccount + ACAConstant.SPLIT_CHAR + userAccountEdit.ClientID + "_";
            }
        }

        /// <summary>
        /// Click Save button event handler
        /// </summary>
        /// <param name="sender">Button Object</param>
        /// <param name="e">Event Arguments </param>
        protected void SaveButton_Click(object sender, EventArgs e)
        {
            PublicUserModel4WS model = AppSession.User.UserModel4WS;
            IAccountBll accountBll = (IAccountBll)ObjectFactory.GetObject(typeof(IAccountBll));

            string oldPassword = userAccountEdit.GetOldPassword();
            string newPassword = userAccountEdit.GetNewPassword();
            string cofirmPassword = userAccountEdit.GetConfirmPassword();
            string err = string.Empty;

            if (!string.IsNullOrEmpty(oldPassword))
            {
                try
                {
                    accountBll.Signon(ConfigManager.AgencyCode, model.userID, oldPassword);
                }
                catch (ACAException exp)
                {
                    //if error message is "Invalid user name or password", we need change message to friendly message.
                    err = exp.Message;

                    if (!string.IsNullOrEmpty(err))
                    {
                        err = GetTextByKey("acc_manage_error_oldPassIncorret");
                    }

                    ShowError(err);
                    return;
                }

                if (string.IsNullOrEmpty(newPassword))
                {
                    model.password = null;
                }
                else
                {
                    // if new password is not null, new password must equal confirm password.
                    if (!newPassword.Equals(cofirmPassword, StringComparison.InvariantCulture))
                    {
                        err = GetTextByKey("acc_manage_error_newPassEntry");
                        ShowError(err);
                        return;
                    }

                    model.password = newPassword;
                }
            }
            else if ((!string.IsNullOrEmpty(newPassword)) || (!string.IsNullOrEmpty(cofirmPassword)))
            {
                // if oldpassword is null, newpassword is not null
                err = GetTextByKey("acc_manage_error_oldPassEntry");
                ShowError(err);
                return;
            }
            else
            {
                model.password = null; //password and newpassword and confirmpassword are null;
            }

            //change user model value in control directly
            string initialEmailAddress = model.email;
            userAccountEdit.GetPublicUserModel4WS(model);

            // update the model including password and security answer.
            try
            {
                model.servProvCode = ConfigManager.AgencyCode;

                accountBll.EditPublicUser(model);

                if (!string.IsNullOrEmpty(model.password))
                {
                    accountBll.UpdateNeedChangePassword(ConfigManager.AgencyCode, model.userSeqNum, ACAConstant.COMMON_N, AppSession.User.PublicUserId);
                    
                    // Add this line to refresh daily end cache
                    model.needChangePassword = ACAConstant.COMMON_N;
                }

                this.ClientScript.RegisterStartupScript(this.GetType(), "CloseUserAccountPage", "PopupClose();", true);
            }
            catch (ACAException ex)
            {
                err = ex.Message;

                ShowError(err);
                AppSession.User.UserModel4WS.email = initialEmailAddress;
                return;
            }
        }

        /// <summary>
        /// if password is wrong or new password is null, show error message.
        /// </summary>
        /// <param name="err">error message</param>
        private void ShowError(string err)
        {
            MessageUtil.ShowMessageInPopup(Page, MessageType.Error, err);
        }
    }
}