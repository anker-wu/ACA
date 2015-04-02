#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: SecurityQuestionVerification.aspx.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2014
 *
 *  Description:
 *
 *  Notes:
 *      $Id: SecurityQuestionVerification.aspx.cs 265950 2014-02-19 09:27:54Z ACHIEVO\foxus.lin $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using System;
using System.Linq;
using System.Web;

using Accela.ACA.BLL.Account;
using Accela.ACA.Common;
using Accela.ACA.Common.Util;
using Accela.ACA.Web.Common;
using Accela.ACA.Web.UserInfo;
using Accela.ACA.WSProxy;

namespace Accela.ACA.Web.Account
{
    /// <summary>
    /// Security question verification page
    /// </summary>
    public partial class SecurityQuestionVerification : BasePage
    {
        #region Properties

        /// <summary>
        /// Gets or sets a value indicating a security question value which generated randomly.
        /// </summary>
        private string SecurityQuestionValue
        {
            get
            {
                return Convert.ToString(ViewState["SecurityQuestionValue"]);
            }

            set
            {
                ViewState["SecurityQuestionValue"] = value;
            }
        }

        /// <summary>
        /// Gets a value indicating the public user model.
        /// </summary>
        private PublicUserModel4WS PublicUser
        {
            get
            {
                return Session[SessionConstant.SESSION_USER_FOR_LOGIN_SECURITY] as PublicUserModel4WS;
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
            if (!IsPostBack && !AppSession.IsAdmin)
            {
                txtSecurityAnswer.Attributes.Add("onkeydown", "triggerLogin(event);");

                if (PublicUser != null)
                {
                    PublicUserQuestionModel questionModel = SecurityQuestionUtil.GenerateSecurityQuestionRandomly(PublicUser.questions);

                    if (questionModel != null)
                    {
                        SecurityQuestionValue = questionModel.questionValue;
                        lblSecurityQuestion.Text = SecurityQuestionValue;
                    }

                    UserUtil.CheckRememberMe();
                }
            }
        }

        /// <summary>
        /// Raises the login button click event
        /// </summary>
        /// <param name="sender">An object that contains the event sender.</param>
        /// <param name="e">A System.EventArgs object containing the event data.</param>
        protected void LoginButton_Click(object sender, EventArgs e)
        {
            string answer = txtSecurityAnswer.Text.Trim();

            if (PublicUser != null)
            {
                try
                {
                    PublicUserQuestionModel[] questionModelList = PublicUser.questions;
                    IAccountBll accountBll = ObjectFactory.GetObject<IAccountBll>();
                    bool isAnswerCorrect = questionModelList != null && questionModelList.Any(f => f.questionValue == SecurityQuestionValue && f.answerValue.Trim() == answer);
                    accountBll.IsLockedUserBySecurityQuestionFail(ConfigManager.AgencyCode, PublicUser.userID, PublicUser.userSeqNum, isAnswerCorrect);

                    if (isAnswerCorrect)
                    {
                        AccountUtil.CreateUserContext(PublicUser);
                        bool isFromNewUi = ValidationUtil.IsYes(Request[UrlConstant.IS_FROM_NEWUI]) && StandardChoiceUtil.IsEnableNewTemplate();

                        if (isFromNewUi)
                        {
                            UserUtil.UserLogin(AppSession.User, StandardChoiceUtil.IsEnableLdapAuthentication(), true);
                            string firstName = string.IsNullOrEmpty(AppSession.User.FirstName) ? string.Empty : AppSession.User.FirstName;
                            string lastName = string.IsNullOrEmpty(AppSession.User.LastName) ? string.Empty : AppSession.User.LastName;
                            ClientScript.RegisterClientScriptBlock(ClientScript.GetType(), "redirectToLaunchpad", "<script>window.parent.redirectToLaunchpad('" + firstName + "','" + lastName + "');</script>");
                        }
                        else
                        {
                            //For Security reason, change session ID after user's success login.
                            string url = "~/ChangeSessionID.aspx?" + Request.QueryString;
                            Server.Transfer(url, false);
                        }
                    }
                    else
                    {
                        Page.FocusElement(txtSecurityAnswer.ClientID);
                        MessageUtil.ShowMessage(Page, MessageType.Error, GetTextByKey("aca_securityquestion_verification_msg_invalid"));
                    }
                }
                catch (ACAException ex)
                {
                    string url = string.Format("{0}?ReturnMessage={1}&{2}={3}", ACAConstant.URL_DEFAULT, HttpUtility.HtmlEncode(ex.Message), UrlConstant.MESSAGE_TYPE, MessageType.Error);
                    UrlHelper.KeepReturnUrlAndRedirect(url);
                }
            }
        }

        /// <summary>
        /// Raises the return to login button click event
        /// </summary>
        /// <param name="sender">An object that contains the event sender.</param>
        /// <param name="e">A System.EventArgs object containing the event data.</param>
        protected void ReturnToLoginButton_Click(object sender, EventArgs e)
        {
            bool isFromNewUi = ValidationUtil.IsYes(Request[UrlConstant.IS_FROM_NEWUI]) && StandardChoiceUtil.IsEnableNewTemplate();

            if (isFromNewUi)
            {
                ClientScript.RegisterStartupScript(ClientScript.GetType(), "redirectToLogin", "<script>window.parent.redirectToLogin();</script>");
            }
            else
            {
                UrlHelper.KeepReturnUrlAndRedirect(ACAConstant.URL_DEFAULT);
            }
        }

        #endregion Methods
    }
}
