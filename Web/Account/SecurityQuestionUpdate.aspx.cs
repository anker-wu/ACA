#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: SecurityQuestionUpdate.ascx.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2014
 *
 *  Description:
 *
 *  Notes:
 *      $Id: SecurityQuestionUpdate.ascx.cs 169604 2014-03-14 03:01:38Z ACHIEVO\daly.zeng $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Security;
using Accela.ACA.BLL.Account;
using Accela.ACA.Common;
using Accela.ACA.Common.Util;
using Accela.ACA.Web.Common;
using Accela.ACA.Web.UserInfo;
using Accela.ACA.WSProxy;

namespace Accela.ACA.Web.Account
{
    /// <summary>
    /// Security question edit page.
    /// </summary>
    public partial class SecurityQuestionUpdate : BasePage
    {
        #region Properties

        /// <summary>
        /// Gets or sets a value indicating public user model.
        /// </summary>
        private PublicUserModel4WS PublicUser
        {
            get
            {
                return Session[SessionConstant.SESSION_USER_FOR_LOGIN_SECURITY] as PublicUserModel4WS;
            }

            set
            {
                Session[SessionConstant.SESSION_USER_FOR_LOGIN_SECURITY] = value;
            }
        }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Raises the System.Web.UI.Control.Initial event.
        /// </summary>
        /// <param name="e">An System.EventArgs object that contains the event data.</param>
        protected override void OnInit(EventArgs e)
        {
            if (!Page.IsPostBack && !AppSession.IsAdmin)
            {
                int controlCount = SecurityQuestionUtil.GetMultipleQuestionControlCount();
                ddlQuestionForDaily.ChildControlCount = controlCount;
                txbAnswerForDaily.ChildControlCount = controlCount;
                ddlQuestionForDaily.NextFocusControlID = txbAnswerForDaily.ClientID;
            }

            base.OnInit(e);
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
                if (AppSession.IsAdmin)
                {
                    txtQuestionForAdmin.Visible = true;
                    txbAnswerForAdmin.Visible = true;
                }
                else
                {
                    ddlQuestionForDaily.Visible = true;
                    txbAnswerForDaily.Visible = true;
                }

                if (PublicUser != null)
                {
                    txbUserID.Text = PublicUser.userID;

                    if (PublicUser.questions != null && PublicUser.questions.Length != 0)
                    {
                        var orderQuestions = PublicUser.questions.OrderBy(f => f.sortOrder);
                        string questionValue = string.Join(ACAConstant.SPLIT_CHAR.ToString(), orderQuestions.Select(f => f.questionValue.Trim()));
                        string answerValue = string.Join(ACAConstant.SPLIT_CHAR.ToString(), orderQuestions.Select(f => f.answerValue.Trim()));
                        ddlQuestionForDaily.SetValue(questionValue);
                        txbAnswerForDaily.SetValue(answerValue);
                    }
                }
            }
        }

        /// <summary>
        /// Raises the submit button click event
        /// </summary>
        /// <param name="sender">An object that contains the event sender.</param>
        /// <param name="e">A System.EventArgs object containing the event data.</param>
        protected void SubmitButton_Click(object sender, EventArgs e)
        {
            try
            {
                string[] arrQuestion = ddlQuestionForDaily.GetValue().Split(ACAConstant.SPLIT_CHAR);
                string[] arrAnswer = txbAnswerForDaily.GetValue().Split(ACAConstant.SPLIT_CHAR);
                IList<PublicUserQuestionModel> questionList = new List<PublicUserQuestionModel>();

                for (int i = 0; i < arrQuestion.Length; i++)
                {
                    questionList.Add(new PublicUserQuestionModel()
                    {
                        questionValue = arrQuestion[i],
                        answerValue = arrAnswer[i].Trim(),
                        sortOrder = i.ToString(),
                        recFulName = ACAConstant.ADMIN
                    });
                }

                PublicUser.questions = questionList.ToArray();
                IAccountBll accountBll = ObjectFactory.GetObject<IAccountBll>();
                accountBll.UpdateSecurityQuestions(PublicUser);
                AccountUtil.CreateUserContext(PublicUser);
                PublicUser = null;

                //set value of public user ID for soap header.
                I18nSoapHeaderExtension.CurrentUser = AppSession.User == null ? string.Empty : AppSession.User.PublicUserId;

                bool isFromNewUi = ValidationUtil.IsYes(Request[UrlConstant.IS_FROM_NEWUI]) && StandardChoiceUtil.IsEnableNewTemplate();

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
            catch (ACAException ex)
            {
                string err = ex.Message;
                MessageUtil.ShowMessage(Page, MessageType.Error, err);
            }
        }

        #endregion Methods
    }
}