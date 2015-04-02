#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: ForgotPassword.aspx.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2008-2014
 *
 *  Description:
 *
 *  Notes:
 *      $Id: ForgotPassword.aspx.cs 278854 2014-09-16 09:49:40Z ACHIEVO\james.shi $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using System;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI.WebControls;
using Accela.ACA.BLL.Account;
using Accela.ACA.BLL.Admin;
using Accela.ACA.Common;
using Accela.ACA.Common.Common;
using Accela.ACA.Common.Util;
using Accela.ACA.Web.Common;
using Accela.ACA.Web.UserInfo;
using Accela.ACA.Web.Util;
using Accela.ACA.WSProxy;

namespace Accela.ACA.Web.Account
{
    /// <summary>
    /// forgot password page
    /// </summary>
    public partial class ForgotPassword : BasePage
    {
        /* ===================================================
         * This page in only used for internal authentication.
         * ===================================================*/

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

        /// <summary>
        /// Gets or sets a value indicating a security question value which generated randomly.
        /// </summary>
        private static string SecurityQuestionValue
        {
            get
            {
                return Convert.ToString(HttpContext.Current.Session["SecurityQuestionValue"]);
            }

            set
            {
                HttpContext.Current.Session["SecurityQuestionValue"] = value;
            }
        }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Check unique Email address from Javascript of AccountEdit
        /// </summary>
        /// <param name="emailAddress">Email address</param>
        /// <returns>Validation Information</returns>
        /// <exception cref="System.Exception">throw the exception for not found email</exception>
        [WebMethod(Description = "Get the Question by email", EnableSession = true)]
        public static string GetQuestionByEmail(string emailAddress)
        {
            string result = string.Empty;

            IAccountBll accountBll = ObjectFactory.GetObject<IAccountBll>();

            if (I18nEmailUtil.CheckEmailFormat(emailAddress))
            {
                PublicUserModel publicUserModel = accountBll.GetPublicUserByEmailOrUserID(emailAddress);

                if (publicUserModel != null)
                {
                    if (publicUserModel.questions == null)
                    {
                        return string.Empty;
                    }

                    PublicUserQuestionModel questionModel = SecurityQuestionUtil.GenerateSecurityQuestionRandomly(publicUserModel.questions);
                    SecurityQuestionValue = questionModel == null ? string.Empty : questionModel.questionValue;
                    result = ScriptFilter.EncodeHtml(SecurityQuestionValue);
                }
                else
                {
                    string errorMsg = LabelUtil.GetTextByKey("aca_findpassword_msg_nonexistentemail", string.Empty);
                    throw new Exception(errorMsg);
                }
            }

            return result;
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

            if (!Page.IsPostBack)
            {
                if (AppSession.IsAdmin)
                {
                    InitUI();
                }

                if (StandardChoiceUtil.IsEnableResetPasswordOnCombine())
                {
                    if (Request.QueryString[UrlConstant.FORGOTPASSWORD_PAGE_TYPE] != null
                        && UrlConstant.FORGOTPASSWORD_PAGE_CONFIRMPASSWORD.Equals(Request.QueryString[UrlConstant.FORGOTPASSWORD_PAGE_TYPE], StringComparison.InvariantCultureIgnoreCase))
                    {
                        pnlCombine.Visible = false;
                    }
                    else
                    {
                        pnlCombine.Visible = true;
                    }

                    pnlEmail.Visible = false;
                    pnlSecurityAnswer.Visible = false;

                    if (!AppSession.IsAdmin)
                    {
                        Page.ClientScript.RegisterStartupScript(GetType(), "disableCombineSecurityAnswer", "disableCombineSecurityAnswer(true);", true);
                    }
                }

                txtCombineEmail.Attributes.Add("onkeyup", "checkCombineEmailAddress_onkeyup('" + txtCombineEmail.ClientID + "',event)");

                AccessibilityUtil.FocusElement(txtEmail.ClientID);
                RegisterSubmitJavaScript(txtEmail, btnEmailHidden.ClientID);
                RegisterSubmitJavaScript(txtSecurityAnswer, btnSendEmailHidden.ClientID);
            }
        }

        /// <summary>
        /// Raises continue button click event
        /// </summary>
        /// <param name="sender">An object that contains the event sender.</param>
        /// <param name="e">A System.EventArgs object containing the event data.</param>
        protected void ContinueButton_Click(object sender, EventArgs e)
        {
            IAccountBll accountBll = ObjectFactory.GetObject<IAccountBll>();
            try
            {
                string email = this.txtEmail.Text.Trim();
                PublicUserModel publicUserModel = accountBll.GetPublicUserByEmailOrUserID(email);

                if (publicUserModel != null)
                {
                    if (publicUserModel.questions == null)
                    {
                        ResetPassword(publicUserModel.userID);
                        return;
                    }

                    pnlEmail.Visible = false;
                    pnlSecurityAnswer.Visible = true;
                    AccessibilityUtil.FocusElement(txtSecurityAnswer.ClientID);
                    PublicUserQuestionModel questionModel = SecurityQuestionUtil.GenerateSecurityQuestionRandomly(publicUserModel.questions);
                    SecurityQuestionValue = questionModel == null ? string.Empty : questionModel.questionValue;
                    lblSecurityQuestion.Text = SecurityQuestionValue;
                    pnlSuccess.Visible = false;

                    // set the url match aca_admin_tree for showing security question
                    _adminUrl = ACAConstant.PageForgotPasswordSecurityQuestionUrl;
                }
                else
                {
                    AccessibilityUtil.FocusElement(txtEmail.ClientID);
                    string msg = GetTextByKey("aca_findpassword_msg_nonexistentemail");
                    MessageUtil.ShowMessage(Page, MessageType.Error, msg);
                }
            }
            catch (ACAException ex)
            {
                MessageUtil.ShowMessage(Page, MessageType.Error, ex.Message);
            }
        }

        /// <summary>
        /// Raises the send email button click event
        /// </summary>
        /// <param name="sender">An object that contains the event sender.</param>
        /// <param name="e">A System.EventArgs object containing the event data.</param>
        protected void SendEmailButton_Click(object sender, EventArgs e)
        {
            string securityAnswer = string.Empty;
            IAccountBll accountBll = ObjectFactory.GetObject<IAccountBll>();
            string email = string.Empty;

            if (StandardChoiceUtil.IsEnableResetPasswordOnCombine())
            {
                securityAnswer = txtCombineSecurityAnswer.Text.Trim();
                email = txtCombineEmail.Text.Trim();
            }
            else
            {
                securityAnswer = txtSecurityAnswer.Text.Trim();
                email = txtEmail.Text.Trim();
            }

            try
            {
                PublicUserModel publicUserModel = accountBll.GetPublicUserByEmailOrUserID(email);

                if (publicUserModel == null)
                {
                    AccessibilityUtil.FocusElement(txtEmail.ClientID);
                    string msg = GetTextByKey("aca_findpassword_msg_nonexistentemail");
                    MessageUtil.ShowMessage(Page, MessageType.Error, msg);
                }
                else if (publicUserModel.questions == null || publicUserModel.questions.Length == 0)
                {
                    ResetPassword(publicUserModel.userID);
                }
                else
                {
                    CheckSecurityAnswer(publicUserModel, securityAnswer);
                }
            }
            catch (ACAException ex)
            {
                MessageUtil.ShowMessage(Page, MessageType.Error, ex.Message);
            }
        }

        /// <summary>
        /// user press enter key, the page will be submitted. Add a Javascript to the textbox attributes.
        /// </summary>
        /// <param name="txt">TextBox control</param>
        /// <param name="clientID">Client id of the control</param>
        private static void RegisterSubmitJavaScript(TextBox txt, string clientID)
        {
            string javascript = "javascript:var keynum;";
            javascript += " keynum = event.keyCode || event.which;";
            javascript += " if(keynum==13) {";
            javascript += " document.getElementById('" + clientID + "').click();return false;}  ";
            txt.Attributes.Add("onkeydown", javascript);
        }

        /// <summary>
        /// initial UI in ACA admin
        /// </summary>
        private void InitUI()
        {
            string pageType = Request.QueryString[UrlConstant.FORGOTPASSWORD_PAGE_TYPE];

            switch (pageType)
            {
                case UrlConstant.FORGOTPASSWORD_PAGE_SECURITYQUESTION:
                    pnlEmail.Visible = false;
                    pnlSecurityAnswer.Visible = true;
                    break;

                case UrlConstant.FORGOTPASSWORD_PAGE_CONFIRMPASSWORD:
                    pnlEmail.Visible = false;
                    pnlCombine.Visible = false;
                    pnlSuccess.Visible = true;
                    findPasswordSuccessMessage.Show(MessageType.Success, "acc_findPassword_label_SuccessTitle", MessageSeperationType.Both);
                    break;
            }
        }

        /// <summary>
        /// Check securityAnswer whether wright
        /// </summary>
        /// <param name="publicUserModel">current public user model</param>
        /// <param name="securityAnswer">security question's answer</param>
        private void CheckSecurityAnswer(PublicUserModel publicUserModel, string securityAnswer)
        {
            bool isAnswerCorrect = publicUserModel != null
                                   && publicUserModel.questions != null
                                   && publicUserModel.questions.Any(f => f.questionValue == SecurityQuestionValue && f.answerValue.Trim() == securityAnswer.Trim());

            if (isAnswerCorrect)
            {
                ResetPassword(publicUserModel.userID);

                // set the url match aca_admin_tree for password confirmation
                _adminUrl = ACAConstant.PageForgotPasswordConfirmUrl;
            }
            else
            {
                if (!StandardChoiceUtil.IsEnableResetPasswordOnCombine())
                {
                    AccessibilityUtil.FocusElement(txtSecurityAnswer.ClientID);
                }
                else
                {
                    //reset question to page
                    Page.ClientScript.RegisterStartupScript(this.GetType(), "setQuestion" + CommonUtil.GetRandomUniqueID(), "document.getElementById('" + lblSecurityQuestionCombine.ClientID + "').innerHTML='" + ScriptFilter.EncodeHtml(SecurityQuestionValue) + "';", true);
                    AccessibilityUtil.FocusElement(txtCombineSecurityAnswer.ClientID);
                }

                string msg = GetTextByKey("acc_forgotPassword_error_securityAnswerlInvalidate");
                MessageUtil.ShowMessage(Page, MessageType.Error, msg);
            }
        }

        /// <summary>
        /// Resets the password.
        /// </summary>
        /// <param name="userid">The user id.</param>
        private void ResetPassword(string userid)
        {
            IAccountBll accountBll = ObjectFactory.GetObject<IAccountBll>();
            accountBll.ResetPassword(ConfigManager.AgencyCode, userid);

            bool isFromNewUi = ValidationUtil.IsYes(Request[UrlConstant.IS_FROM_NEWUI]) && StandardChoiceUtil.IsEnableNewTemplate();

            if (isFromNewUi)
            {
                var successMsg = LabelUtil.GetTextByKey("acc_findPassword_label_SuccessTitle", this.ModuleName);
                ClientScript.RegisterStartupScript(ClientScript.GetType(), "redirectToLogin", string.Format("<script>window.parent.redirectToLogin('{0}');</script>", successMsg));
                return;
            }

            pnlEmail.Visible = false;
            pnlSecurityAnswer.Visible = false;
            pnlCombine.Visible = false;
            pnlSuccess.Visible = true;

            findPasswordSuccessMessage.Show(
                MessageType.Success,
                "acc_findPassword_label_SuccessTitle",
                MessageSeperationType.Both);
        }

        #endregion Methods
    }
}
