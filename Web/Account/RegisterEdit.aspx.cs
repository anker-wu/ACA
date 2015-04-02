#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: RegisterEdit.aspx.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2008-2014
 *
 *  Description:
 *
 *  Notes:
 *     $Id: RegisterEdit.aspx.cs 278620 2014-09-09 08:23:59Z ACHIEVO\alan.hu $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 */

#endregion Header

using System;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.Services;
using Accela.ACA.BLL;
using Accela.ACA.BLL.Account;
using Accela.ACA.BLL.Common;
using Accela.ACA.Common;
using Accela.ACA.Common.Util;
using Accela.ACA.ExpressionBuild;
using Accela.ACA.SSOInterface;
using Accela.ACA.SSOInterface.Model;
using Accela.ACA.Web.Common;
using Accela.ACA.Web.ExpressionBuild;
using Accela.ACA.Web.NewUI;
using Accela.ACA.Web.People;
using Accela.ACA.Web.UserInfo;
using Accela.ACA.Web.Util;
using Accela.ACA.WSProxy;
using Accela.ACA.WSProxy.WSModel;

namespace Accela.ACA.Web.Account
{
    /// <summary>
    /// Page for editing register user
    /// </summary>
    public partial class RegisterEdit : BasePage
    {
        /// <summary>
        /// Keep the e-mail address for Agent Clerk editing to prevent the e-mail validation if the e-mail does not been changed.
        /// </summary>
        private static string _clerkEmail = string.Empty;

        /// <summary>
        /// Indicating if current request if for Clerk register or edit.
        /// </summary>
        private bool _isForClerk = false;

        /// <summary>
        /// Agent Clerk sequence number
        /// </summary>
        private string _clerkSeqNbr = string.Empty;

        /// <summary>
        /// Indicating the Clerk edit action between Clerk Add and Clerk edit.
        /// </summary>
        private bool _isClerkEdit = false;

        #region Properties

        /// <summary>
        /// Gets or sets the user id which exists in another agency.
        /// </summary>
        protected string ExistingAccountRegisterationUserID
        {
            get
            {
                return ViewState["ExistingAccountRegisterationUserID"] as string;
            }

            set
            {
                ViewState["ExistingAccountRegisterationUserID"] = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether current user is an existing account.
        /// </summary>
        private bool IsNewUserRegisterExistingAccount
        {
            get
            {
                return Convert.ToBoolean(ViewState["IsNewUserRegisterExistingAccount"]);
            }

            set
            {
                ViewState["IsNewUserRegisterExistingAccount"] = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether current user comes from another agency.
        /// </summary>
        private bool IsLoginUseExistingAccount
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
        /// Gets or sets the Agent Clerk contact sequence number.
        /// </summary>
        private string ContactSeqNbr
        {
            get
            {
                return ViewState["ContactSeqNbr"] as string;
            }

            set
            {
                ViewState["ContactSeqNbr"] = value;
            }
        }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Creates the contact parameters session.
        /// </summary>
        /// <param name="moduleName">Name of the module.</param>
        /// <param name="contactIndex">Index of the contact.</param>
        /// <param name="contactAddressIndex">Index of the contact address.</param>
        /// <param name="processType">Type of the process.</param>
        /// <param name="parameterString">The parameter string.</param>
        /// <returns>The contact parameters session.</returns>
        [WebMethod(Description = "Creates the contact parameters session", EnableSession = true)]
        public static bool CreateContactParametersSession(string moduleName, string contactIndex, string contactAddressIndex, string processType, string parameterString)
        {
            ContactSessionParameter sessionParameter = new JavaScriptSerializer().Deserialize<ContactSessionParameter>(parameterString);

            if (!string.IsNullOrEmpty(processType))
            {
                sessionParameter.Process.ContactProcessType = EnumUtil<ContactProcessType>.Parse(processType);
            }

            if (sessionParameter.Process.ContactProcessType == ContactProcessType.Edit)
            {
                //todo: need better solutions
                ContactSessionParameter tempParameter = AppSession.GetContactSessionParameter();
                sessionParameter.Data.DataObject = tempParameter.Data.DataObject;
                sessionParameter.Data.IsCloseMatch = tempParameter.Data.IsCloseMatch;
                sessionParameter.ContactType = tempParameter.ContactType;
            }
            else
            {
                PeopleModel4WS people = new PeopleModel4WS();
                people.contactType = sessionParameter.ContactType;
                sessionParameter.Data.DataObject = people;
            }

            AppSession.SetContactSessionParameter(sessionParameter);
            return true;
        }

        /// <summary>
        /// Creates the contact parameters session.
        /// </summary>
        /// <param name="moduleName">Name of the module.</param>
        /// <param name="contactAddressIndex">Index of the contact address.</param>
        /// <param name="processType">Type of the process.</param>
        /// <param name="callbackName">Name of the callback.</param>
        /// <param name="parameterString">The parameter string.</param>
        [WebMethod(Description = "Operation Contact Address Session", EnableSession = true)]
        public static void OperationContactAddressSession(string moduleName, string contactAddressIndex, string processType, string callbackName, string parameterString)
        {
            ContactSessionParameter sessionParameter = AppSession.GetContactSessionParameter();

            if (!string.IsNullOrEmpty(parameterString))
            {
                ContactSessionParameter newSessionParameter = new JavaScriptSerializer().Deserialize<ContactSessionParameter>(parameterString);
                sessionParameter.ContactSectionPosition = newSessionParameter.ContactSectionPosition;
            }

            if (!string.IsNullOrEmpty(processType))
            {
                sessionParameter.Process.ContactAddressProcessType = EnumUtil<ContactAddressProcessType>.Parse(processType);
            }

            if (!string.IsNullOrEmpty(contactAddressIndex))
            {
                sessionParameter.Data.ContactAddressRowIndex = int.Parse(contactAddressIndex);
            }
            else
            {
                sessionParameter.Data.ContactAddressRowIndex = null;
            }

            sessionParameter.Process.CACallbackFunctionName = callbackName;

            ContactSessionParameter parametersModel = AppSession.GetRegisterContactSessionParameter();

            if (parametersModel != null)
            {
                sessionParameter.Data.DataObject = ObjectCloneUtil.DeepCopy(parametersModel.Data.DataObject);
            }

            AppSession.SetContactSessionParameter(sessionParameter);
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
        /// Check unique Email address from Javascript of AccountEdit
        /// </summary>
        /// <param name="emailAddress">Email address</param>
        /// <returns>Validation Information</returns>
        [WebMethod(Description = "is existed email", EnableSession = true)]
        public static string IsExistEmail(string emailAddress)
        {
            //For Agent clerk -- to prevent the e-mail validation if the e-mail does not been changed.
            if (!string.IsNullOrEmpty(_clerkEmail) && emailAddress.Trim().Equals(_clerkEmail, StringComparison.InvariantCultureIgnoreCase))
            {
                return string.Empty;
            }

            return UserUtil.IsExistEmail(emailAddress);
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
        /// Clear contact had added
        /// </summary>
        [WebMethod(Description = "Clear contact", EnableSession = true)]
        public static void ClearContact()
        {
            AppSession.SetContactSessionParameter(null);
            AppSession.SetRegisterContactSessionParameter(null);
        }

        /// <summary>
        /// Check if the user enter correct password while he/she tries to register an existing account
        /// </summary>
        /// <param name="userIdentifier">User id or email.</param>
        /// <param name="password">The Password.</param>
        /// <returns>True if the password is correct for the not-registered user, else false.</returns>
        [WebMethod(Description = "Validate if the password is correct.", EnableSession = true)]
        public static string IsPasswordCorrect(string userIdentifier, string password)
        {
            ResultModel result = AccountUtil.IsPasswordCorrect(userIdentifier, password);

            return string.Format("{{passValidation:\"{0}\",errorMessage:\"{1}\"}}", result.entityValue, result.errorMsg);
        }

        /// <summary>
        /// On Initial event method
        /// </summary>
        /// <param name="e">EventArgs e</param>
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            _clerkSeqNbr = Request.QueryString[UrlConstant.CLERK_SEQ_NBR];
            _isForClerk = ValidationUtil.IsYes(Request.QueryString[UrlConstant.IS_FOR_CLERK]);
            IsLoginUseExistingAccount = ValidationUtil.IsYes(Request.QueryString[UrlConstant.IS_LOGIN_USE_EXISTING_ACCOUNT]);
            ExistingAccountRegisterationUserID = Request.QueryString[UrlConstant.USER_ID_OR_EMAIL];

            if (IsLoginUseExistingAccount)
            {
                contactEdit.IsLoginUseExistingAccount = true;
                UserRegistration.IsLoginUseExistingAccount = true;
            }
            
            if (_isForClerk && ((AppSession.IsAdmin && Request.QueryString.AllKeys.Contains(UrlConstant.CLERK_SEQ_NBR)) || !string.IsNullOrEmpty(_clerkSeqNbr)))
            {
                _isClerkEdit = true;
                UserRegistration.IsForClerkEdit = true;
                contactEdit.IsShowDetail = true;
            }

            if (!string.IsNullOrWhiteSpace(Request.QueryString[UrlConstant.RECOVER_SOURCE]))
            {
                IsNewUserRegisterExistingAccount = true;
            }

            string loginInfoViewId = string.Empty;
            string loginInfoClientID = UserRegistration.ClientID;

            if (_isForClerk)
            {
                contactEdit.EnableSelectFromAccount = false;

                if (_isClerkEdit)
                {
                    loginInfoClientID = userAccountEdit.ClientID;
                    lblClerkTitle.LabelKey = "aca_authagent_editclerk_label_title";
                    loginInfoViewId = GviewID.AuthAgentEditClerkAccountForm;
                    contactEdit.ContactSectionPosition = ACAConstant.ContactSectionPosition.EditClerk;
                }
                else
                {
                    lblClerkTitle.LabelKey = "aca_authagent_addclerk_label_title";
                    StartNextButton2.LabelKey = "aca_authagent_addclerk_label_continue";
                    loginInfoViewId = GviewID.AuthAgentNewClerkAccountForm;
                    contactEdit.ContactSectionPosition = ACAConstant.ContactSectionPosition.RegisterClerk;
                }
            }
            else
            {
                loginInfoViewId = GviewID.UserRegistration;

                if (IsLoginUseExistingAccount || IsNewUserRegisterExistingAccount)
                {
                    contactEdit.ContactSectionPosition = ACAConstant.ContactSectionPosition.RegisterExistingAccount;
                }
                else
                {
                    contactEdit.ContactSectionPosition = ACAConstant.ContactSectionPosition.RegisterAccount;
                }
            }

            lblContactTitle.SectionID = ACAConstant.SPLIT_CHAR + string.Empty + ACAConstant.SPLIT_CHAR + "ctl00_PlaceHolderMain_contactEdit_";
            lblLoginInfo.SectionID = ACAConstant.SPLIT_CHAR + loginInfoViewId + ACAConstant.SPLIT_CHAR + loginInfoClientID + "_";
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
                UIModelUtil.ClearUIData();
            }
            else
            {
                // Register hot key to cancel button
                string javascript = string.Format("OverrideTabKey(event, false, '{0}');", hlDuplicateKeyBegin.ClientID);
                hlDuplicateKeyEnd.OnKeyDown = javascript;
            }

            if (IsNewUserRegisterExistingAccount)
            {
                HandleNewUserRegisterExistingAccount();
            }

            //Check user whether can enter the registration process.
            AccountUtil.CheckRegistrationPermission();

            //Set the contact section as required, which need to validate.
            contactEdit.SetSectionRequired("0", true);
            contactEdit.ContactExpressionType = ExpressionType.ReferenceContact;
            ContactSessionParameter parametersModel = AppSession.GetRegisterContactSessionParameter();

            if (parametersModel != null && parametersModel.Data != null && parametersModel.Data.IsCloseMatch)
            {
                contactEdit.SetContactAddressListIsEditable(false);
            }
            else
            {
                contactEdit.IsEditable = true;
            }

            if (!IsPostBack)
            {
                InitAccountInfo();

                if (!StandardChoiceUtil.IsEnableCaptchaForRegistration())
                {
                    divRecaptcha.Visible = false;
                }
            }
        }

        /// <summary>
        /// OnPreRender event method.
        /// </summary>
        /// <param name="e">EventArgs e</param>
        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);

            if (!AppSession.IsAdmin && !IsPostBack)
            {
                // Clear some temporary data stored in seestion for expression
                ExpressionUtil.ClearExpressionVariables();
            }

            ContactSessionParameter parametersModel = AppSession.GetRegisterContactSessionParameter();

            if (parametersModel != null)
            {
                contactEdit.ValidateContactAddress(ContactUtil.GetPeopleModelFromContactSessionParameter(parametersModel));
            }
        }

        /// <summary>
        /// Initialize account info
        /// </summary>
        protected void InitAccountInfo()
        {
            if (_isForClerk)
            {
                //Clear the static value.
                _clerkEmail = string.Empty;

                if (!AppSession.IsAdmin && AppSession.User.IsAnonymous)
                {
                    GotoLoginPage();
                }

                divRegisterTitle.Visible = false;
                lnkCancel.Visible = true;

                //Edit clerk
                if (_isClerkEdit)
                {
                    userAccountEdit.Visible = true;
                    UserRegistration.Visible = false;
                    StartNextButton2.Visible = false;
                    btnSave.Visible = true;

                    if (AppSession.IsAdmin)
                    {
                        divAmmendment.Visible = true;
                    }
                    else
                    {
                        InitClerkInfo(_clerkSeqNbr);
                    }
                }
                else
                {
                    userAccountEdit.Visible = false;
                    UserRegistration.Visible = true;

                    if (!AppSession.IsAdmin)
                    {
                        ContactSessionParameter parametersModel = AppSession.GetRegisterContactSessionParameter();

                        if (parametersModel != null)
                        {
                            DisplayContact(ContactUtil.GetPeopleModelFromContactSessionParameter(parametersModel));
                        }
                    }
                }
            }
            else
            {
                lblClerkTitle.Visible = false;
                h1ClerkTitle.Visible = lblClerkTitle.Visible;

                if (!AppSession.IsAdmin)
                {
                    if (AppSession.IsEditFromLoginFlag)
                    {
                        InitAccountInfoFromLogin();
                        AppSession.IsEditFromLoginFlag = false;
                    }
                    else
                    {
                        ContactSessionParameter parametersModel = AppSession.GetRegisterContactSessionParameter();

                        if (parametersModel != null)
                        {
                            DisplayContact(ContactUtil.GetPeopleModelFromContactSessionParameter(parametersModel));
                        }
                    }

                    if (IsLoginUseExistingAccount || IsNewUserRegisterExistingAccount)
                    {
                        userAccountEdit.Visible = false;
                        UserRegistration.Visible = true;
                        contactEdit.EnableSelectFromAccount = true;
                        IAccountBll accountBll = ObjectFactory.GetObject<IAccountBll>();
                        PublicUserModel model = null;

                        string userIdentifier = string.Empty;

                        if (IsLoginUseExistingAccount)
                        {
                            userIdentifier = ExistingAccountRegisterationUserID;
                        }
                        else if (IsNewUserRegisterExistingAccount)
                        {
                            userIdentifier = UserRegistration.GetUserIdentifier();
                        }

                        model = accountBll.GetPublicUserByEmailOrUserID(userIdentifier);
                        UserRegistration.DisplayExistingAccountRegistrationInfo(model);
                    }
                }
            }
        }

        /// <summary>
        /// Click on button "Continue Registration"
        /// </summary>
        /// <param name="sender">An object that contains the event sender.</param>
        /// <param name="e">An System.EventArgs object that contains the event data.</param>
        protected void NextCreateAccountButton_OnClick(object sender, EventArgs e)
        {
            if (StandardChoiceUtil.IsEnableCaptchaForRegistration() && !reCaptcha.Validate())
            {
                return;
            }

            // get user info(Account,Contact)
            ContactSessionParameter parametersModel = AppSession.GetRegisterContactSessionParameter();
            PublicUserModel4WS publicUserModel = new PublicUserModel4WS();
            PublicUserModel4WS publicUserModel4SSO = PeopleUtil.GetPublicUserFromSession();

            //From SSO link.
            if (AuthenticationUtil.IsNeedRegistration 
                && !string.IsNullOrWhiteSpace(publicUserModel4SSO.userSeqNum)
                && publicUserModel4SSO.userSeqNum != ACAConstant.ANONYMOUS_FLAG)
            {
                publicUserModel = publicUserModel4SSO;
            }
            else
            {
                publicUserModel = UserRegistration.GetPublicUserModel4WS(publicUserModel);
                publicUserModel.servProvCode = ConfigManager.AgencyCode;
                publicUserModel.auditID = publicUserModel.userID;
                publicUserModel.auditStatus = ACAConstant.VALID_STATUS;
                publicUserModel.roleType = ACAConstant.ROLE_TYPE_CITIZEN;
            }

            publicUserModel.peopleModel = new PeopleModel4WS[] { parametersModel.Data.DataObject as PeopleModel4WS };

            PeopleModel4WS peopleModel = publicUserModel.peopleModel[0];

            //If the "ENABLE_CONTACT_ADDRESS_MAINTENANCE" Or "ENABLE_CONTACT_ADDRESS" STD is turn on then need to validate
            if (peopleModel != null
                && StandardChoiceUtil.IsEnableContactAddress()
                && StandardChoiceUtil.IsEnableContactAddressMaintenance()
                && parametersModel.Data != null
                && !parametersModel.Data.IsCloseMatch)
            {
                string errorMessage = RequiredValidationUtil.ValidateContactAddressType(peopleModel.contactType, peopleModel.contactAddressList);

                if (!string.IsNullOrEmpty(errorMessage))
                {
                    contactEdit.ShowContactAddressValidateErrorMessage(errorMessage);
                    return;
                }
            }

            try
            {
                if (parametersModel.Data != null && parametersModel.Data.IsCloseMatch)
                {
                    CreatePublicUserAndRedirect(publicUserModel);
                }
                else
                {
                    string identityKeyMessage = PeopleUtil.GetIdentityKeyMessage(publicUserModel.peopleModel[0], PeopleUtil.IdentityFieldLabels, "aca_message_duplicate_contact_identity_reg");

                    if (!string.IsNullOrEmpty(identityKeyMessage))
                    {
                        PeopleUtil.SavePublicUserToSession(publicUserModel);
                        lblDuplicateIndentityMessage.Text = identityKeyMessage;
                        Page.ClientScript.RegisterStartupScript(this.GetType(), "showDuplicateMessage", "showDuplicateMessage();", true);
                        return;
                    }

                    CreatePublicUserAndRedirect(publicUserModel);
                }
            }
            catch (ACAException ex)
            {
                MessageUtil.ShowMessage(Page, MessageType.Error, ex.Message);
            }
        }

        /// <summary>
        /// Click on button 'Add New'.
        /// </summary>
        /// <param name="sender">An object that contains the event sender.</param>
        /// <param name="arg">A CommonEventArgs object that contains the event data.</param>
        protected void ContactEdit_OnPreAddNewClick(object sender, CommonEventArgs arg)
        {
            try
            {
                if (StandardChoiceUtil.IsEnableCaptchaForRegistration() && !reCaptcha.Validate())
                {
                    arg.ArgObject = true;
                    return;
                }

                PublicUserModel4WS model = null;

                if (!_isClerkEdit)
                {
                    model = new PublicUserModel4WS();
                    model = UserRegistration.GetPublicUserModel4WS(model);
                    model.servProvCode = ConfigManager.AgencyCode;
                    model.auditID = model.userID;
                    model.auditStatus = ACAConstant.VALID_STATUS;
                    model.roleType = ACAConstant.ROLE_TYPE_CITIZEN;
                }
                else
                {
                    IAccountBll accountBll = ObjectFactory.GetObject<IAccountBll>();
                    model = accountBll.GetPublicUser(_clerkSeqNbr);
                    string newPassword = userAccountEdit.GetNewPassword();

                    if (!string.IsNullOrEmpty(newPassword))
                    {
                        model.password = newPassword;
                    }

                    userAccountEdit.GetPublicUserModel4WS(model);
                    model.servProvCode = ConfigManager.SuperAgencyCode;
                }

                PeopleUtil.SavePublicUserToSession(model);
            }
            catch (ACAException ex)
            {
                MessageUtil.ShowMessage(Page, MessageType.Error, ex.Message);
                arg.ArgObject = true;
            }
        }

        /// <summary>
        /// Save the clerk modify.
        /// </summary>
        /// <param name="sender">An object that contains the event sender.</param>
        /// <param name="e">An System.EventArgs object that contains the event data.</param>
        protected void SaveButton_Click(object sender, EventArgs e)
        {
            try
            {
                if (StandardChoiceUtil.IsEnableCaptchaForRegistration() && !reCaptcha.Validate())
                {
                    return;
                }

                IAccountBll accountBll = ObjectFactory.GetObject<IAccountBll>();
                PublicUserModel4WS userModel = accountBll.GetPublicUser(_clerkSeqNbr);
                ContactSessionParameter parametersModel = AppSession.GetRegisterContactSessionParameter();
                PeopleModel4WS peopleModel = ContactUtil.GetPeopleModelFromContactSessionParameter(parametersModel);

                string errorMsg = contactEdit.ValidateContactAddress(peopleModel);

                if (!string.IsNullOrEmpty(errorMsg))
                {
                    return;
                }

                string newPassword = userAccountEdit.GetNewPassword();

                if (!string.IsNullOrEmpty(newPassword))
                {
                    userModel.password = newPassword;

                    // Clerk need to change password if Agent changed Clerk's password.
                    accountBll.UpdateNeedChangePassword(
                        ConfigManager.AgencyCode,
                        userModel.userSeqNum,
                        ACAConstant.COMMON_Y,
                        AppSession.User.PublicUserId);

                    // Add this line to refresh daily end cache
                    userModel.needChangePassword = ACAConstant.COMMON_Y;
                }
                else
                {
                    //if new password empty means keep the old password.
                    userModel.password = string.Empty;
                }

                userModel.peopleModel = new PeopleModel4WS[] { peopleModel };
                userAccountEdit.GetPublicUserModel4WS(userModel);
                userModel.servProvCode = ConfigManager.SuperAgencyCode;
                accountBll.EditAuthAgentClerk(userModel, AppSession.User.PublicUserId);

                AppSession.SetContactSessionParameter(null);
                AppSession.SetRegisterContactSessionParameter(null);

                Response.Redirect("AccountManager.aspx");
            }
            catch (Exception ex)
            {
                MessageUtil.ShowMessage(Page, MessageType.Error, ex.Message);
            }
        }

        /// <summary>
        /// Cancel the clerk modify.
        /// </summary>
        /// <param name="sender">An object that contains the event sender.</param>
        /// <param name="e">An System.EventArgs object that contains the event data.</param>
        protected void CancelButton_Click(object sender, EventArgs e)
        {
            AppSession.SetContactSessionParameter(null);
            AppSession.SetRegisterContactSessionParameter(null);
            Response.Redirect("AccountManager.aspx");
        }

        /// <summary>
        /// Amendment button event handler.
        /// </summary>
        /// <param name="sender">the sender object.</param>
        /// <param name="e">the event hander.</param>
        protected void CreateAmendmentButton_Click(object sender, EventArgs e)
        {
            ICapTypeFilterBll captypeFilterBll = (ICapTypeFilterBll)ObjectFactory.GetObject(typeof(ICapTypeFilterBll));
            XButtonFilterModel4WS xbFilter = captypeFilterBll.GetFilter4ButtonModel(ConfigManager.AgencyCode, ModuleName, "aca_account_management_contact_amendment", AppSession.User.PublicUserId);

            if (xbFilter != null && !string.IsNullOrEmpty(xbFilter.moduleName) && !string.IsNullOrEmpty(xbFilter.filterName))
            {
                //Use the "isSubAgencyCap" to bypass worklocation.aspx page in super agency when doing Contact Amendment at account manager page.
                string isSubAgencyCap = StandardChoiceUtil.IsSuperAgency() ? ACAConstant.IS_SUBAGENCY_CAP + "=" + ACAConstant.COMMON_Y : string.Empty;
                string url = string.Format(
                    "~/Cap/CapType.aspx?Module={0}&{1}={2}&{3}={4}{5}&isAmendment=Y",
                    xbFilter.moduleName,
                    UrlConstant.FILTER_NAME,
                    HttpUtility.UrlEncode(xbFilter.filterName),
                    UrlConstant.CONTACT_SEQ_NUMBER,
                    ContactSeqNbr,
                    isSubAgencyCap);
                Response.Redirect(url);
            }
        }

        /// <summary>
        /// Duplicate key submit handler.
        /// </summary>
        /// <param name="sender">the sender object.</param>
        /// <param name="e">The event handle</param>
        protected void ContinueButton_OnClick(object sender, EventArgs e)
        {
            PublicUserModel4WS publicUser = PeopleUtil.GetPublicUserFromSession();
            PeopleModel4WS people = publicUser.peopleModel[0];
            people.auditStatus = ACAConstant.VALID_STATUS;

            var duplicateFieldList = PeopleUtil.DuplicateIndentityFields;

            if (duplicateFieldList != null)
            {
                foreach (long fieldKey in duplicateFieldList)
                {
                    switch (fieldKey)
                    {
                        case (long)IdentityFields.SSN:
                            people.socialSecurityNumber = null;
                            break;
                        case (long)IdentityFields.FEIN:
                            people.fein = null;
                            break;
                        case (long)IdentityFields.PassportNumber:
                            people.passportNumber = null;
                            break;
                        case (long)IdentityFields.StateIDNumber:
                            people.stateIDNbr = null;
                            break;
                        case (long)IdentityFields.DriverLicenseNumber:
                            people.driverLicenseNbr = null;
                            break;
                        case (long)IdentityFields.DriverLicenseState:
                            people.driverLicenseState = null;
                            break;
                        case (long)IdentityFields.Email:
                            people.email = null;
                            break;
                    }
                }
            }

            CreatePublicUserAndRedirect(publicUser);
        }

        /// <summary>
        /// Create public user and redirect to confirm page.
        /// </summary>
        /// <param name="model">A PublicUserModel4WS</param>
        private void CreatePublicUserAndRedirect(PublicUserModel4WS model)
        {
            if (AuthenticationUtil.IsNeedRegistration)
            {
                AssociatePublicUserWithExternalInfo(model);
                return;
            }
            
            IAccountBll accountBll = ObjectFactory.GetObject<IAccountBll>();
            string userSeqNum = string.Empty;

            if (IsLoginUseExistingAccount)
            {
                userSeqNum = ActivateExistingUserAccount(model);
            }
            else if (IsNewUserRegisterExistingAccount)
            {
                try
                {
                    accountBll.Signon(ConfigManager.AgencyCode, UserRegistration.GetUserIdentifier(), model.password);
                }
                catch (ACAException ex)
                {
                    LoginStatusCode loginStatusCode = EnumUtil<LoginStatusCode>.Parse(ex.Message);
                    string errorMessage = string.Empty;

                    if (loginStatusCode == LoginStatusCode.NOTREGISTERED)
                    {
                        userSeqNum = ActivateExistingUserAccount(model);
                    }
                    else
                    {
                        if (loginStatusCode == LoginStatusCode.FAIL)
                        {
                            errorMessage = GetTextByKey("aca_existing_account_registeration_msg_incorrectpwd");
                        }
                        else
                        {
                            errorMessage = AccountUtil.GetErrorMessageByErrorCode(ex.Message);
                        }

                        MessageUtil.ShowMessage(Page, MessageType.Error, errorMessage);
                        Page.SetFocus(UserRegistration.PasswordClientId);
                        return;
                    }
                }
            }
            else
            {
                userSeqNum = accountBll.CreatePublicUser(model, PeopleUtil.GetArrayLicense());
            }

            model.userSeqNum = userSeqNum;

            if (IsLoginUseExistingAccount || IsNewUserRegisterExistingAccount)
            {
                string orginalPwd = model.password;

                /* The password would become encode string after got the model, 
                 * to support validation for 'Login Now' after registration complement, so need to restore to orginal password.
                 */
                model = accountBll.GetPublicUser(userSeqNum);
                model.password = orginalPwd;
            }

            PeopleUtil.SavePublicUserToSession(model);
            
            string url = string.Format(
                "RegisterConfirm.aspx?{0}={1}{2}",
                UrlConstant.USER_SEQ_NUM,
                userSeqNum,
                _isForClerk ? string.Format("&{0}={1}", UrlConstant.IS_FOR_CLERK, ACAConstant.COMMON_Y) : string.Empty);

            if (IsLoginUseExistingAccount || IsNewUserRegisterExistingAccount)
            {
                url += string.Format("&{0}={1}", UrlConstant.IS_LOGIN_USE_EXISTING_ACCOUNT, ACAConstant.COMMON_Y);
            }

            bool isFromNewUi = ValidationUtil.IsYes(Request[UrlConstant.IS_FROM_NEWUI]) && StandardChoiceUtil.IsEnableNewTemplate();

            if (isFromNewUi)
            {
                url = NewUiUtil.GetUrlByResource(url);
            }

            //set Registration Login information expire time before redirect to confirm page
            if (AppSession.User.IsAnonymous)
            {
                SetRegistrationInfoExpireTime();
            }

            // goto register confirm page
            UrlHelper.KeepReturnUrlAndRedirect(url);
        }

        /// <summary>
        /// Initialize the clerk information
        /// </summary>
        /// <param name="clerkSeqNbr">The clerk sequence number.</param>
        private void InitClerkInfo(string clerkSeqNbr)
        {
            IAccountBll accountBll = ObjectFactory.GetObject<IAccountBll>();
            PublicUserModel4WS clerkModel = accountBll.GetPublicUser(clerkSeqNbr);

            if (clerkModel == null)
            {
                return;
            }

            //Keep the e-mail address for Agent Clerk editing to prevent the e-mail validation if the e-mail does not been changed.
            _clerkEmail = clerkModel.email;

            userAccountEdit.Display(clerkModel);

            contactEdit.SetSectionRequired("0", true);

            ICapTypeFilterBll captypeFilterBll = (ICapTypeFilterBll)ObjectFactory.GetObject(typeof(ICapTypeFilterBll));
            XButtonFilterModel4WS xbFilter = captypeFilterBll.GetFilter4ButtonModel(ConfigManager.AgencyCode, this.ModuleName, "aca_account_management_contact_amendment", AppSession.User.PublicUserId);

            if (xbFilter != null && !string.IsNullOrEmpty(xbFilter.moduleName) && !string.IsNullOrEmpty(xbFilter.filterName))
            {
                divAmmendment.Visible = true;
            }

            PeopleModel4WS people = null;

            if (AppSession.IsEditFromClerkFlag)
            {
                if (clerkModel.peopleModel != null && clerkModel.peopleModel.Length > 0)
                {
                    people = clerkModel.peopleModel[0];

                    //this step will change people value, should change before save to session, so that contactSession data should be consistent with RegisterContactSession
                    DisplayContact(people);

                    SaveToContactSessionParameter(people);

                    // When the agent amendment the contact information, the contact section in spear form need to load the peope model from session.
                    AppSession.SetPeopleModelToSession(people.contactSeqNumber, TempModelConvert.ConvertToPeopleModel(people));
                }

                AppSession.IsEditFromClerkFlag = false;
            }
            else
            {
                ContactSessionParameter parametersModel = AppSession.GetRegisterContactSessionParameter();
                people = ContactUtil.GetPeopleModelFromContactSessionParameter(parametersModel);

                DisplayContact(people);
            }
        }
        
        /// <summary>
        /// Initialize the account information from Login page.
        /// </summary>
        private void InitAccountInfoFromLogin()
        {
            if (StandardChoiceUtil.IsEnableLdapAuthentication() || AuthenticationUtil.IsNeedRegistration)
            {
                /*
                * Display the user account information if LDAP/RealMe authentication is enabled.
                * The user information in session is created in Login control.
                */
                PublicUserModel4WS publicUser = PeopleUtil.GetPublicUserFromSession();

                if (publicUser != null && publicUser.peopleModel != null && publicUser.peopleModel.Length > 0)
                {
                    PeopleModel4WS peopleModel = AuthenticationUtil.IsNeedRegistration
                                                 ? ContactUtil.GetPeopleModelForEdit(publicUser.peopleModel)
                                                 : publicUser.peopleModel[0];
                    SaveToContactSessionParameter(peopleModel);
                    DisplayContact(peopleModel);
                }
            }
        }

        /// <summary>
        /// Display contact.
        /// </summary>
        /// <param name="people">A <see cref="PeopleModel4WS"/> object.</param>
        private void DisplayContact(PeopleModel4WS people)
        {
            if (people != null)
            {
                ContactSeqNbr = people.contactSeqNumber;
                contactEdit.DisplayView(people);

                //if contact is pending or reject, cannot modify contact information.
                if (ContractorPeopleStatus.Pending.Equals(people.contractorPeopleStatus, StringComparison.OrdinalIgnoreCase) || ContractorPeopleStatus.Rejected.Equals(people.contractorPeopleStatus, StringComparison.OrdinalIgnoreCase))
                {
                    lnkCreateAmendment.Visible = false;
                    btnSave.Enabled = false;
                }
            }
        }

        /// <summary>
        /// Save the contact info to the ContactSessionParameter object.
        /// </summary>
        /// <param name="people">A <see cref="PeopleModel4WS"/> object.</param>
        private void SaveToContactSessionParameter(PeopleModel4WS people)
        {
            //todo: need better solution
            if (people != null)
            {
                ContactSessionParameter sessionParameter = new ContactSessionParameter();
                sessionParameter.ContactType = people.contactType;
                sessionParameter.Data.DataObject = people;
                AppSession.SetContactSessionParameter(sessionParameter);
                ContactSessionParameter sessionParameterClone = ObjectCloneUtil.DeepCopy(sessionParameter);
                AppSession.SetRegisterContactSessionParameter(sessionParameterClone);
            }
        }

        /// <summary>
        /// set RegistrationInfo Expire Time
        /// </summary>
        private void SetRegistrationInfoExpireTime()
        {
            XPolicyModel[] xPolicyList = StandardChoiceUtil.GetRegistrationInfoExpireTime();

            if (xPolicyList != null && xPolicyList.Length > 0)
            {
                XPolicyModel xpolicy = xPolicyList[0];
                int expireTime = !string.IsNullOrEmpty(xpolicy.data3) ? Convert.ToInt32(xpolicy.data3) : 300;
               
                Session[SessionConstant.SESSION_REGISTRE_USER_MODEL_EXPIRETIME] = DateTime.Now.AddSeconds(expireTime);
            }
        }

        /// <summary>
        /// Get existing user model.
        /// </summary>
        /// <returns>Public user model.</returns>
        private PublicUserModel GetExistingUserModel()
        {
            IAccountBll accountBll = ObjectFactory.GetObject<IAccountBll>();
            string userIdentifier = string.Empty;

            if (IsLoginUseExistingAccount)
            {
                userIdentifier = ExistingAccountRegisterationUserID;
            }
            else if (IsNewUserRegisterExistingAccount)
            {
                userIdentifier = UserRegistration.GetUserIdentifier();
            }

            return accountBll.GetPublicUserByEmailOrUserID(userIdentifier);
        }

        /// <summary>
        /// Handle new user registering existing account
        /// </summary>
        private void HandleNewUserRegisterExistingAccount()
        {
            string userIdentifier = UserRegistration.GetUserIdentifier();
            IAccountBll accountBll = ObjectFactory.GetObject<IAccountBll>();
            PeopleModel4WS[] contacts = accountBll.GetContactsByPublicUser(ConfigManager.AgencyCode, userIdentifier, false);

            if (contacts != null && contacts.Length > 0)
            {
                string url = string.Format("~/Login.aspx?{0}={1}", UrlConstant.USER_ID_OR_EMAIL, HttpUtility.UrlEncode(userIdentifier));
                Response.Redirect(url);
            }
            else
            {
                ExistingAccountRegisterationUserID = userIdentifier;
                string eventSource = Request.QueryString[UrlConstant.RECOVER_SOURCE];
                UserRegistration.DisplayNewUserRegisterExistingAccountUI(eventSource, ExistingAccountRegisterationUserID);
                contactEdit.IsNewUserRegisterExistingAccount = IsNewUserRegisterExistingAccount;
                contactEdit.ExistingAccountRegisterationUserID = ExistingAccountRegisterationUserID;
                contactEdit.EnableSelectFromAccount = true;
            }
        }

        /// <summary>
        /// Activate existing user account and return user sequence number.
        /// </summary>
        /// <param name="model">Public user model.</param>
        /// <returns>User sequence number.</returns>
        private string ActivateExistingUserAccount(PublicUserModel4WS model)
        {
            PublicUserModel existingUserModel = GetExistingUserModel();

            if (existingUserModel.userSeqNum.HasValue)
            {
                model.userSeqNum = existingUserModel.userSeqNum.ToString();
                model.userID = existingUserModel.userID;
                model.email = existingUserModel.email;
            }

            foreach (var people in model.peopleModel)
            {
                people.contactSeqNumber = null;
            }

            IAccountBll accountBll = ObjectFactory.GetObject<IAccountBll>();
            model.licenseModel = PeopleUtil.GetArrayLicense();
            string userSeqNum = accountBll.ActiveExistingAccount(model);

            return userSeqNum;
        }

        /// <summary>
        /// Associate public user with external user and sign on directly.
        /// </summary>
        /// <param name="model">Public user model.</param>
        private void AssociatePublicUserWithExternalInfo(PublicUserModel4WS model)
        {
            IAccountBll accountBll = ObjectFactory.GetObject<IAccountBll>();
            PublicUserModel4WS publicUserModel4SSO = PeopleUtil.GetPublicUserFromSession();
            PublicUserModel4WS publicUser = null;

            try
            {
                LicenseModel4WS[] newLicenseList = PeopleUtil.GetArrayLicense();

                XPublicUserSSOModel publicUserSSOModel = new XPublicUserSSOModel();
                publicUserSSOModel.serviceProviderCode = ConfigManager.AgencyCode;
                publicUserSSOModel.accountType = publicUserModel4SSO.SSOType;
                publicUserSSOModel.accountID = publicUserModel4SSO.SSOUserName;

                UserCreationEventArgs publicUserCreationArgs = new UserCreationEventArgs();
                PeopleModel4WS peopleModel = model.peopleModel[0];
                publicUserCreationArgs.FirstName = peopleModel.firstName;
                publicUserCreationArgs.LastName = peopleModel.lastName;

                string errorMsg = string.Empty;

                if (ACAContext.Events.PreUserCreation != null)
                {
                    ReturnResultModel resultModelBefore = ACAContext.Events.PreUserCreation(publicUserCreationArgs);

                    if (string.IsNullOrEmpty(publicUserModel4SSO.SSOUserName))
                    {
                        if (resultModelBefore != null)
                        {
                            publicUserSSOModel.accountID = Convert.ToString(resultModelBefore.ReturnEntity);
                            errorMsg = resultModelBefore.Message;
                        }

                        if (string.IsNullOrEmpty(publicUserSSOModel.accountID))
                        {
                            MessageUtil.ShowMessage(Page, MessageType.Error, errorMsg);
                            return;
                        }
                    }
                }

                //From SSO link
                if (!string.IsNullOrWhiteSpace(publicUserModel4SSO.userSeqNum) && publicUserModel4SSO.userSeqNum != ACAConstant.ANONYMOUS_FLAG)
                {
                    if (!ValidationUtil.IsYes(model.peopleModel[0].accountOwner))
                    {
                        model.peopleModel[0].accountOwner = ACAConstant.COMMON_Y;
                    }

                    LicenseUtil.MergeLicense(model, newLicenseList);

                    SimpleAuditModel auditModel = new SimpleAuditModel();
                    auditModel.auditID = model.auditID;
                    publicUserSSOModel.auditModel = auditModel;
                    publicUserSSOModel.userSeqNbr = long.Parse(model.userSeqNum);

                    accountBll.EditPublicUser(model, publicUserSSOModel);
                }
                else
                {
                    model.userSeqNum = accountBll.CreatePublicUser(model, newLicenseList, publicUserSSOModel);
                }

                if (ACAContext.Events.PostUserCreation != null)
                {
                    ACAContext.Events.PreUserCreation(publicUserCreationArgs);
                }

                publicUser = accountBll.Signon4External(ConfigManager.AgencyCode, publicUserModel4SSO.SSOUserName, publicUserModel4SSO.SSOType);
            }
            catch (ACAException exp)
            {
                string url = string.Format(
                    "{0}?{1}={2}&{3}={4}", 
                    ACAConstant.URL_DEFAULT, 
                    UrlConstant.RETURN_MESSAGE,
                    HttpUtility.HtmlEncode(exp.Message), 
                    UrlConstant.MESSAGE_TYPE, 
                    MessageType.Error);
                Response.Redirect(url);
                return;
            }

            AccountUtil.CreateUserContext(publicUser);
            PeopleUtil.SavePublicUserToSession(null);
            UserUtil.UserLogin(AppSession.User, false);
        }

        #endregion Methods
    }
}
