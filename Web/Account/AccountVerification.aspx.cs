#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: AccountVerification.aspx.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2007-2014
 *
 *  Description:
 *  Account verify page.
 *
 *  Notes:
 *      $Id: AccountVerification.aspx.cs 278210 2014-08-29 05:45:24Z ACHIEVO\james.shi $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using System;
using System.Web;
using System.Web.Security;

using Accela.ACA.BLL.Account;
using Accela.ACA.BLL.Admin;
using Accela.ACA.Common;
using Accela.ACA.Web.Common;
using Accela.ACA.Web.Util;

namespace Accela.ACA.Web.Account
{
    /// <summary>
    /// ActiveAccount page for ACA
    /// </summary>
    public partial class AccountVerification : BasePage
    {
        #region Fields

        /// <summary>
        /// the url that match the aca admin tree
        /// </summary>
        private string _adminUrl = string.Empty;

        #endregion Fields

        #region Properties

        /// <summary>
        /// Gets current page's page id
        /// </summary>
        public override string PageID
        {
            get
            {
                string pageID = base.PageID;

                if (!string.IsNullOrEmpty(_adminUrl))
                {
                    IAdminBll adminBll = ObjectFactory.GetObject(typeof(IAdminBll)) as IAdminBll;
                    pageID = adminBll.GetPageIDbyUrl(_adminUrl);
                }

                return pageID;
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
            // if registration links is disabled, Don't allow to access login page
            if (!StandardChoiceUtil.IsLoginEnabled())
            {
                // avoid user enter url directly to login page.
                Response.Redirect(ACAConstant.URL_DEFAULT);
            }

            //In Daily side, login box only shown for internal authentication adapter.
            if (!AppSession.IsAdmin)
            {
                LoginBox.Visible = AuthenticationUtil.IsInternalAuthAdapter;
            }

            if (!IsPostBack)
            {
                //Showing different AgencyCode in the text.
                IbISuccessInfo.Text = string.Format(GetTextByKey("acc_verification_account_success_Tip"), ConfigManager.AgencyCode);

                if (!AppSession.IsAdmin)
                {
                    if (AuthenticationUtil.IsInternalAuthAdapter)
                    {
                        // Clear the authentication ticket
                        FormsAuthentication.SignOut();

                        // Clear the contents of their session
                        HttpContext.Current.Session.Clear();

                        // Tell the system to drop the session reference so that it does
                        // not need to be carried around with the user
                        HttpContext.Current.Session.Abandon();
                    }

                    //Verification account step:
                    //1.Get the userID
                    string userID = ActivateAccount();

                    //2.If the userID is null the user account is not activated.
                    DisplayInfo(userID);

                    //If the current authentication adapter is external, directly redirect to third-party login page.
                    if (!AuthenticationUtil.IsInternalAuthAdapter && AuthenticationUtil.IsNeedRegistration)
                    {
                        AuthenticationUtil.Signout(HttpContext.Current);
                        AuthenticationUtil.RedirectToLoginPage(FileUtil.AppendAbsolutePath(ACAConstant.URL_WELCOME_PAGE), string.Empty);
                    }
                }
                else
                {
                    ShowExpiredInfo();
                }
            }
        }

        /// <summary>
        /// Get the register's userID
        /// </summary>
        /// <returns>registered user id</returns>
        private string ActivateAccount()
        {
            string userID = string.Empty;
            string msg = string.Empty;
            string uuid = string.Empty;
            try
            {
                IAccountBll accountBll = ObjectFactory.GetObject<IAccountBll>();

                //build the forward URL request object.
                HttpRequest firstparameter = new HttpRequest("uuid", Request.UrlReferrer.ToString(), Request.UrlReferrer.Query.Remove(0, 1));
                uuid = firstparameter.QueryString["uuid"];

                if (string.IsNullOrEmpty(uuid))
                {
                    msg = GetTextByKey("acc_accountVerification_error_invalid");
                    MessageUtil.ShowMessage(Page, MessageType.Error, msg);

                    return string.Empty;
                }

                userID = accountBll.ActivateUser(ConfigManager.AgencyCode, uuid);

                return userID;
            }
            catch (ACAException ex)
            {
                msg = ex.Message;
                MessageUtil.ShowMessage(Page, MessageType.Error, msg);

                return string.Empty;
            }
        }

        /// <summary>
        /// according to the response value to show different information.
        /// </summary>
        /// <param name="userID">string user id</param>
        private void DisplayInfo(string userID)
        {
            // if the account is not actived
            if (string.IsNullOrEmpty(userID))
            {
                divFail.Visible = true;
                divSuccess.Visible = false;

                // set the url match aca_admin_tree
                this._adminUrl = ACAConstant.PAGE_ACCOUNT_VERIFICATION_EXPIRED;
            }
            else
            {
                if (LoginBox.Visible)
                {
                    LoginBox.TextUserName = userID;
                }
            }
        }

        /// <summary>
        /// click Account Verification Expire in ACA Admin show expired information.
        /// </summary>
        private void ShowExpiredInfo()
        {
            string isExpiredPage = string.Empty;
            isExpiredPage = Request.QueryString["isExpiredPage"];
            if (AppSession.IsAdmin && !string.IsNullOrEmpty(isExpiredPage) && isExpiredPage.Equals(ACAConstant.COMMON_Y, StringComparison.InvariantCultureIgnoreCase))
            {
                divFail.Visible = true;
                divSuccess.Visible = false;
            }
        }

        #endregion Methods
    }
}
