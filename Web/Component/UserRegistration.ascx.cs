#region Header
/**
 * <pre>
 * 
 *  Accela Citizen Access
 *  File: UserRegistration.cs
 * 
 *  Accela, Inc.
 *  Copyright (C): 2011-2014
 * 
 *  Description:
 *  Define UserAccountEdit
 *  Notes:
 *      $Id: UserRegistration.cs 133464 2009-06-05 05:06:34Z ACHIEVO\jackie.yu $.
 *  Revision History
 *  &lt;Date&gt;  &lt;Who&gt;  &lt;What&gt;
 * </pre>
 */
#endregion Header

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using Accela.ACA.Common;
using Accela.ACA.Common.Util;
using Accela.ACA.Web.Common;
using Accela.ACA.Web.Common.Control;
using Accela.ACA.Web.FormDesigner;
using Accela.ACA.Web.UserInfo;
using Accela.ACA.Web.Util;
using Accela.ACA.WSProxy;

namespace Accela.ACA.Web.Component
{
    /// <summary>
    /// UserRegistration Form
    /// </summary>
    public partial class UserRegistration : FormDesignerBaseControl
    {
        /// <summary>
        /// Initializes a new instance of the UserRegistration class.
        /// </summary>
        public UserRegistration()
            : base(GviewID.UserRegistration)
        {
        }

        #region Properties

        /// <summary>
        /// Gets or sets a value indicating whether this instance is for clerk edit.
        /// </summary>
        /// <value><c>true</c> if this instance is for clerk edit; otherwise, <c>false</c>.</value>
        public bool IsForClerkEdit { get; set; }

        /// <summary>
        /// Gets the 1st password control's client Id
        /// </summary>
        public string PasswordClientId
        {
            get
            {
                return txbPassword1.ClientID;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether current user comes from another agency.
        /// </summary>
        public bool IsLoginUseExistingAccount
        {
            get
            {
                return Convert.ToBoolean(ViewState["IsLoginUseExistingAccount"]);
            }

            set
            {
                ViewState["IsLoginUseExistingAccount"] = value;
            }
        }

        /// <summary>
        /// Gets a value indicating whether current registering account is existing in another agency.
        /// </summary>
        public bool IsAccountRecoverAction
        {
            get
            {
                return IsLoginUseExistingAccount || !string.IsNullOrWhiteSpace(Request.QueryString[UrlConstant.RECOVER_SOURCE]);
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is for clerk.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is for clerk; otherwise, <c>false</c>.
        /// </value>
        protected bool IsForClerk { get; set; }

        /// <summary>
        /// Gets the recover message link text.
        /// </summary>
        protected string RecoverMsgLinkText
        {
            get
            {
                return GetTextByKey("aca_existing_account_registeration_label_linktext");
            }
        }

        /// <summary>
        /// Gets or sets permission
        /// </summary>
        protected override GFilterScreenPermissionModel4WS Permission
        {
            get
            {
                base.Permission = new GFilterScreenPermissionModel4WS();
                base.Permission.permissionLevel = GViewConstant.PERMISSION_USERREGISTRATION;
                return base.Permission;
            }

            set
            {
                base.Permission = value;
            }
        }

        #endregion

        /// <summary>
        /// Get user model
        /// </summary>
        /// <param name="model">Public User Model</param>
        /// <returns>a PublicUserModel4WS</returns>
        public PublicUserModel4WS GetPublicUserModel4WS(PublicUserModel4WS model)
        {
            // Return null if control is empty
            if (string.IsNullOrEmpty(txbUserName.Text)
                && string.IsNullOrEmpty(txbEmail.Text)
                && string.IsNullOrEmpty(txbPassword1.Text)
                && string.IsNullOrEmpty(txbPassword2.Text)
                && string.IsNullOrEmpty(ddlQuestionForDaily.GetValue())
                && string.IsNullOrEmpty(txbAnswerForDaily.GetValue()))
            {
                return model;
            }

            model.userID = txbUserName.Text.Trim();
            model.email = txbEmail.Text.Trim();

            if (StandardChoiceUtil.IsEnableLdapAuthentication() || AuthenticationUtil.IsNeedRegistration)
            {
                /*
                 * For security reason:
                 * If the LDAP/RealMe authentication is enabled, will generate a random password for internal account;
                 * To avoid the internal account use the blank password to log no system if administrator closed the LDAP/RealMe authentication.
                 */
                model.password = AccountUtil.GetRandomPassword();
            }
            else
            {
                model.password = txbPassword1.Text.Trim();
                string[] arrQuestion = ddlQuestionForDaily.GetValue().Split(ACAConstant.SPLIT_CHAR);
                string[] arrAnswer = txbAnswerForDaily.GetValue().Split(ACAConstant.SPLIT_CHAR);
                IList<PublicUserQuestionModel> questionList = new List<PublicUserQuestionModel>();

                for (int i = 0; i < arrQuestion.Length; i++)
                {
                    questionList.Add(new PublicUserQuestionModel()
                    {
                        questionValue = arrQuestion[i],
                        answerValue = arrAnswer[i].Trim(),
                        recFulName = ACAConstant.ADMIN
                    });
                }

                model.questions = questionList.ToArray();
            }

            model.cellPhone = txbMobilePhone.GetPhone(string.Empty);
            model.cellPhoneCountryCode = txbMobilePhone.CountryCodeText.Trim();
            model.receiveSMS = cbReceiveSMS.Checked ? ACAConstant.COMMON_Y : ACAConstant.COMMON_N;

            if (ValidationUtil.IsYes(Request.QueryString[UrlConstant.IS_FOR_CLERK]) && AppSession.User.IsAuthorizedAgent)
            {
                model.authServiceProviderCode = ConfigManager.SuperAgencyCode;
                model.authAgentID = AppSession.User.UserID;
                model.accountType = ACAConstant.PUBLICUSER_TYPE_AUTH_AGENT_CLERK;
                model.needChangePassword = ACAConstant.COMMON_Y;
            }

            return model;
        }

        /// <summary>
        /// Display existing account information
        /// </summary>
        /// <param name="model">Existing user account model</param>
        public void DisplayExistingAccountRegistrationInfo(PublicUserModel model)
        {
            if (IsLoginUseExistingAccount)
            {
                if (model != null)
                {
                    txbUserName.Text = model.userID;
                    txbEmail.Text = model.email;
                    txbUserName.DisableEdit();
                    txbEmail.DisableEdit();
                }
            }
        }

        /// <summary>
        /// Get an user's identifier: user id or email
        /// </summary>
        /// <returns>User identifier</returns>
        public string GetUserIdentifier()
        {
            return Request.QueryString[UrlConstant.USER_ID_OR_EMAIL];
        }

        /// <summary>
        /// Get password from this user control.
        /// </summary>
        /// <returns>Password value.</returns>
        public string GetPassword()
        {
            return txbPassword1.Text;
        }

        /// <summary>
        /// Display user id/email according to the eventSource.
        /// </summary>
        /// <param name="eventSource">Indicate which link was clicked.</param>
        /// <param name="userIdOrEmail">The user unique identifier original email.</param>
        public void DisplayNewUserRegisterExistingAccountUI(string eventSource, string userIdOrEmail)
        {
            // Change password field label
            txbPassword1.LabelKey = "aca_userinfoform_label_enter_passoword";
            
            // Hide unnecessary fields
            txbPassword2.IsHidden = true;
            ddlQuestionForDaily.IsHidden = true;
            txbAnswerForDaily.IsHidden = true;
            cbReceiveSMS.IsHidden = true;
            txbMobilePhone.IsHidden = true;

            if (eventSource.Equals("email"))
            {
                txbUserName.IsHidden = true;
                txbEmail.Text = userIdOrEmail;
                txbEmail.DisableEdit();
            }
            else if (eventSource.Equals("userId"))
            {
                txbEmail.IsHidden = true;
                txbUserName.Text = userIdOrEmail;
                txbUserName.DisableEdit();
            }
        }

        /// <summary>
        /// Raises the System.Web.UI.Control.Initial event.
        /// </summary>
        /// <param name="e">An System.EventArgs object that contains the event data.</param>
        protected override void OnInit(EventArgs e)
        {
            if (!Page.IsPostBack && !AppSession.IsAdmin && !IsLoginUseExistingAccount)
            {
                int controlCount = SecurityQuestionUtil.GetMultipleQuestionControlCount();
                ddlQuestionForDaily.ChildControlCount = controlCount;
                txbAnswerForDaily.ChildControlCount = controlCount;
                ddlQuestionForDaily.NextFocusControlID = txbAnswerForDaily.ClientID;
            }
        }

        /// <summary>
        /// Raises the page load event
        /// </summary>
        /// <param name="sender">An object that contains the event sender.</param>
        /// <param name="e">A System.EventArgs object containing the event data.</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            bool isCheckPassword = AccountUtil.IsEnablePasswordSecurity() && !IsAccountRecoverAction;
            IsForClerk = ValidationUtil.IsYes(Request.QueryString[UrlConstant.IS_FOR_CLERK]);

            if (IsLoginUseExistingAccount)
            {
                ViewId = GviewID.ExistingAccountRegisteration;
            }
            else
            {
                ViewId = IsForClerk ? GviewID.AuthAgentNewClerkAccountForm : GviewID.UserRegistration;
            }
           
            if (!IsForClerk)
            {
                txbUserName.CustomValidationMessageKey = "aca_existing_account_registeration_msg_existinguser";
                txbEmail.CustomValidationMessageKey = "aca_existing_account_registeration_msg_existingemail";
            }

            if (!Page.IsPostBack)
            {
                ucPasswordSecurityBar1.Visible = isCheckPassword && !IsLoginUseExistingAccount;

                if (AppSession.IsAdmin)
                {
                    ucPasswordSecurityBar1.Visible = false;
                }
                else
                {
                    if (StandardChoiceUtil.IsEnableLdapAuthentication() || AuthenticationUtil.IsNeedRegistration)
                    {
                        DisplayUserInfo();
                    }
                }
            }

            if (!IsAccountRecoverAction)
            {
                if (isCheckPassword && !AppSession.IsAdmin)
                {
                    txbPassword1.CustomValidationFunction = "CheckPasswordSecurity_onblur";
                    txbPassword1.Validate = "customvalidation";
                    txbPassword1.Attributes.Add("onblur", "password_onblur();");
                }
                else
                {
                    txbPassword1.CustomValidationFunction = string.Empty;
                    txbPassword1.Validate = "minlength;maxlength";
                }
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

                if (StandardChoiceUtil.IsEnableLdapAuthentication() || !AuthenticationUtil.IsInternalAuthAdapter)
                {
                    string script = string.Format(
                        "$('#{0}_parentGrid, #{1}_parentGrid, #{2}_parentGrid, #{3}_parentGrid').hide();",
                        txbPassword1.ClientID,
                        txbPassword2.ClientID,
                        ddlQuestionForDaily.ClientID,
                        txbAnswerForDaily.ClientID);
                    ScriptManager.RegisterStartupScript(Page, GetType(), "hideFields4ExternalUser", script, true);
                }
            }

            // If postback by contact section need restore password
            if (!IsLoginUseExistingAccount && IsPostBack && Request.Form[Page.postEventSourceID].IndexOf("contactEdit", StringComparison.InvariantCultureIgnoreCase) > -1)
            {
                // If wrong password, the password value can not restore.
                bool needRestore = true;

                // For bug #49516
                // Restore the password field when the updatepanel update.
                if (!string.IsNullOrEmpty(txbPassword1.Text) && needRestore)
                {
                    ScriptManager.RegisterStartupScript(
                        this.Page,
                        this.Page.GetType(),
                        "restorepwd",
                        "$('#" + this.txbPassword1.ClientID + "').val('" + this.txbPassword1.Text + "')",
                        true);
                }

                if (!string.IsNullOrEmpty(txbPassword2.Text))
                {
                    ScriptManager.RegisterStartupScript(
                        this.Page,
                        this.Page.GetType(),
                        "restorepwdag",
                        "$('#" + this.txbPassword2.ClientID + "').val('" + this.txbPassword2.Text + "')",
                        true);
                }
            }
        }

        /// <summary>
        /// Display the user information: 
        ///     1.for LDAP; 
        ///     2.for back from register account confirm page; 
        ///     3.for the adapter which implement IExternalAuthorizedAdapter interface.
        /// </summary>
        private void DisplayUserInfo()
        {
            PublicUserModel4WS registerUser = PeopleUtil.GetPublicUserFromSession();

            // Diaplay the user information and disable the user name field.
            if (registerUser != null)
            {
                txbUserName.Text = registerUser.userID;
                txbEmail.Text = registerUser.email;

                if (AuthenticationUtil.IsNeedRegistration)
                {
                    bool isFromSSOLink = !string.IsNullOrWhiteSpace(registerUser.userSeqNum) && registerUser.userSeqNum != ACAConstant.ANONYMOUS_FLAG;

                    if (isFromSSOLink)
                    {
                        txbUserName.DisableEdit();
                        txbEmail.DisableEdit();
                    }
                    else
                    {
                        txbUserName.Text = registerUser.SSOUserName;
                    }
                }

                if (!IsLoginUseExistingAccount)
                {
                    txbMobilePhone.CountryCodeText = registerUser.cellPhoneCountryCode;
                    txbMobilePhone.Text = ModelUIFormat.FormatPhone4EditPage(registerUser.cellPhone, StandardChoiceUtil.GetDefaultCountry());
                    cbReceiveSMS.Checked = ACAConstant.COMMON_Y.Equals(registerUser.receiveSMS, StringComparison.OrdinalIgnoreCase);
                }

                if (StandardChoiceUtil.IsEnableLdapAuthentication())
                {
                    txbUserName.DisableEdit();
                }
            }
        }
    }
}