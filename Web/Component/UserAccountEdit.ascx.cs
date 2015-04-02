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
 *      $Id: UserAccountEdit.ascx.cs 277971 2014-08-25 06:10:06Z ACHIEVO\james.shi $.
 *  Revision History
 *  &lt;Date&gt;  &lt;Who&gt;  &lt;What&gt;
 * </pre>
 */
#endregion Header

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web.UI;
using Accela.ACA.BLL.Account;
using Accela.ACA.Common;
using Accela.ACA.Common.Util;
using Accela.ACA.Web.Common;
using Accela.ACA.Web.Common.Control;
using Accela.ACA.Web.FormDesigner;
using Accela.ACA.Web.SocialMedia;
using Accela.ACA.Web.UserInfo;
using Accela.ACA.WSProxy;

namespace Accela.ACA.Web.Component
{
    /// <summary>
    /// User account edit form.
    /// </summary>
    public partial class UserAccountEdit : FormDesignerBaseControl
    {
        /// <summary>
        /// Initializes a new instance of the UserAccountEdit class.
        /// </summary>
        public UserAccountEdit()
            : base(GviewID.UserAccount)
        {
        }

        /// <summary>
        /// Gets or sets Permission.
        /// </summary>
        protected override WSProxy.GFilterScreenPermissionModel4WS Permission
        {
            get
            {
                base.Permission = new GFilterScreenPermissionModel4WS();
                base.Permission.permissionLevel = GViewConstant.PERMISSION_USERACCOUNT;
                return base.Permission;
            }

            set
            {
                base.Permission = value;
            }
        }

        /// <summary>
        /// Get confirm password.
        /// </summary>
        /// <returns>Confirm Password</returns>
        public string GetConfirmPassword()
        {
            return txbNewPassword2.Text.Trim();
        }

        /// <summary>
        /// Get new password.
        /// </summary>
        /// <returns>New password</returns>
        public string GetNewPassword()
        {
            return txbNewPassword1.Text.Trim();
        }

        /// <summary>
        /// Get old password.
        /// </summary>
        /// <returns>String for old password.</returns>
        public string GetOldPassword()
        {
            return txbOldPassword.Text.Trim();
        }

        /// <summary>
        /// Display info from model(PublicUserModel4WS)
        /// </summary>
        /// <param name="model">A PublicUserModel4WS</param>
        public void Display(PublicUserModel4WS model)
        {
            txbUserID.Text = model.userID;
            txbEmailID.Text = model.email;

            if (SocialMediaUtil.IsAutoCreateAccount)
            {
                txbOldPassword.Visible = false;
                txbNewPassword1.Visible = false;
                txbNewPassword2.Visible = false;
            }

            txbMobilePhone.CountryCodeText = model.cellPhoneCountryCode;
            txbMobilePhone.Text = ModelUIFormat.FormatPhone4EditPage(model.cellPhone, StandardChoiceUtil.GetDefaultCountry());
            cbReceiveSMS.Checked = ACAConstant.COMMON_Y.Equals(model.receiveSMS, StringComparison.OrdinalIgnoreCase);

            if (model.questions != null && model.questions.Length != 0)
            {
                var orderQuestions = model.questions.OrderBy(f => f.sortOrder);
                string questionValue = string.Join(ACAConstant.SPLIT_CHAR.ToString(), orderQuestions.Select(f => f.questionValue.Trim()));
                string answerValue = string.Join(ACAConstant.SPLIT_CHAR.ToString(), orderQuestions.Select(f => f.answerValue.Trim()));
                ddlQuestionForDaily.SetValue(questionValue);
                txbAnswerForDaily.SetValue(answerValue);
            }
        }

        /// <summary>
        /// Get user model
        /// </summary>
        /// <param name="model">Public User Model</param>
        /// <returns>A PublicUserModel4WS</returns>
        public PublicUserModel4WS GetPublicUserModel4WS(PublicUserModel4WS model)
        {
            string email = txbEmailID.Text.Trim();

            if (!model.email.Equals(email, StringComparison.InvariantCultureIgnoreCase))
            {
                model.email = email;
            }

            string[] arrQuestion = ddlQuestionForDaily.GetValue().Split(ACAConstant.SPLIT_CHAR);
            string[] arrAnswer = txbAnswerForDaily.GetValue().Split(ACAConstant.SPLIT_CHAR);
            IList<PublicUserQuestionModel> questionList = new List<PublicUserQuestionModel>();

            for (int i = 0; i < arrQuestion.Length; i++)
            {
                questionList.Add(new PublicUserQuestionModel()
                {
                    questionValue = arrQuestion[i],
                    answerValue = arrAnswer[i].Trim(),
                    recFulName = ACAConstant.ADMIN,
                    sortOrder = i.ToString()
                });
            }

            model.questions = questionList.ToArray();

            if (txbMobilePhone.Visible)
            {
                model.cellPhone = txbMobilePhone.GetPhone(StandardChoiceUtil.GetDefaultCountry());
                model.cellPhoneCountryCode = txbMobilePhone.CountryCodeText.Trim();
            }

            if (cbReceiveSMS.Visible)
            {
                model.receiveSMS = cbReceiveSMS.Checked ? ACAConstant.COMMON_Y : ACAConstant.COMMON_N;
            }

            return model;
        }

        /// <summary>
        /// Raises the System.Web.UI.Control.Initial event.
        /// </summary>
        /// <param name="e">An System.EventArgs object that contains the event data.</param>
        protected override void OnInit(EventArgs e)
        {
            bool isForClerk = ValidationUtil.IsYes(Request.QueryString[UrlConstant.IS_FOR_CLERK]);
            ViewId = isForClerk ? GviewID.AuthAgentEditClerkAccountForm : GviewID.UserAccount;

            if (!Page.IsPostBack && !AppSession.IsAdmin)
            {
                IAccountBll accountBll = ObjectFactory.GetObject<IAccountBll>();
                PublicUserModel4WS publicUserModel = isForClerk
                    ? accountBll.GetPublicUser(Request.QueryString[UrlConstant.CLERK_SEQ_NBR])
                    : AppSession.User.UserModel4WS;
                int userQuestionCount = publicUserModel.questions == null ? 0 : publicUserModel.questions.Length;
                int controlCount = SecurityQuestionUtil.GetMultipleQuestionControlCount(userQuestionCount);

                ddlQuestionForDaily.ChildControlCount = controlCount;
                txbAnswerForDaily.ChildControlCount = controlCount;
                ddlQuestionForDaily.NextFocusControlID = txbAnswerForDaily.ClientID;
            }
        }

        /// <summary>
        /// Page load event handler.
        /// </summary>
        /// <param name="sender">Page object</param>
        /// <param name="e">Event arguments</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            bool isCheckPassword = AccountUtil.IsEnablePasswordSecurity();

            if (!IsPostBack)
            {
                ucPasswordSecurityBar2.Visible = isCheckPassword;
            }

            if (AppSession.IsAdmin)
            {
                txbUserID.BackColor = Color.Empty;
                ucPasswordSecurityBar2.Visible = false;
            }

            if (isCheckPassword && !AppSession.IsAdmin)
            {
                txbNewPassword1.CustomValidationFunction = "CheckPasswordSecurity_onblur";
                txbNewPassword1.Validate = "customvalidation";
                txbNewPassword1.Attributes.Add("onblur", "password_onblur();");
            }
            else
            {
                txbNewPassword1.CustomValidationFunction = string.Empty;
                txbNewPassword1.Validate = "minlength;maxlength";
            }

            ControlUtil.SetMaskForPhoneAndZip(!IsPostBack, true, null, null, false, txbMobilePhone);
            ControlBuildHelper.AddValidationForStandardFields(ViewId, ModuleName, Permission, phContent.Controls);
            InitFormDesignerPlaceHolder(phContent);

            // Switch the control for admin setting.
            if (AppSession.IsAdmin)
            {
                ddlQuestion.Visible = true;
                txbAnswer.Visible = true;
            }
            else
            {
                foreach (SimpleViewElementModel4WS item in phContent.SimpleViewModel.simpleViewElements)
                {
                    if (item.viewElementName == ddlQuestion.ID)
                    {
                        item.viewElementName = ddlQuestionForDaily.ID;
                    }

                    if (item.viewElementName == txbAnswer.ID)
                    {
                        item.viewElementName = txbAnswerForDaily.ID;
                    }
                }
            }

            // If postback by contact section need restore password
            if (IsPostBack && Request.Form[Page.postEventSourceID].IndexOf("contactEdit", StringComparison.InvariantCultureIgnoreCase) > -1)
            {
                //for bug #49516
                //restore the password field when the updatepanel update.
                if (!string.IsNullOrEmpty(txbNewPassword1.Text))
                {
                    ScriptManager.RegisterStartupScript(
                        Page,
                        Page.GetType(),
                        "restorepwd",
                        "$('#" + txbNewPassword1.ClientID + "').val('" + txbNewPassword1.Text + "')",
                        true);
                }

                if (!string.IsNullOrEmpty(txbNewPassword2.Text))
                {
                    ScriptManager.RegisterStartupScript(
                        Page,
                        Page.GetType(),
                        "restorepwdag",
                        "$('#" + txbNewPassword2.ClientID + "').val('" + txbNewPassword2.Text + "')",
                        true);
                }
            }
        }
    }
}