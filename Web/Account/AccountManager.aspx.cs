#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: AccountManager.aspx.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2008-2014
 *
 *  Description:
 *
 *  Notes:
 *      $Id: AccountManager.aspx.cs 278450 2014-09-04 06:44:06Z ACHIEVO\james.shi $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using System;
using System.Collections.Generic;
using System.Web.Services;
using Accela.ACA.BLL.Account;
using Accela.ACA.Common;
using Accela.ACA.Common.Util;
using Accela.ACA.ExpressionBuild;
using Accela.ACA.Web.Common;
using Accela.ACA.Web.Common.Control;
using Accela.ACA.Web.ExpressionBuild;
using Accela.ACA.Web.People;
using Accela.ACA.Web.UserInfo;
using Accela.ACA.Web.Util;
using Accela.ACA.WSProxy;
using Accela.Web.Controls;

namespace Accela.ACA.Web.Account
{
    /// <summary>
    /// Page for manage user account
    /// </summary>
    public partial class AccountManager : BasePage
    {
        #region Fields

        /// <summary>
        /// long view id
        /// </summary>
        private const long VIEW_ID = 5041;

        #endregion Fields

        #region Properties

        /// <summary>
        /// Gets or sets a value indicating whether edit login or not
        /// </summary>
        protected bool IsEditLogin
        {
            get
            {
                if (ViewState["IsEditLogin"] == null)
                {
                    return false;
                }

                return (bool)ViewState["IsEditLogin"];
            }

            set
            {
                ViewState["IsEditLogin"] = value;
            }
        }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Creates the contact parameters session.
        /// </summary>
        /// <param name="contactExpressionType">Type of the contact.</param>
        /// <param name="postbackControlId">The post back control id.</param>
        /// <param name="contactSectionPosition">The contact section position.</param>
        /// <param name="processType">Type of the process.</param>
        /// <returns>The contact parameters session.</returns>
        [WebMethod(Description = "Creates the contact parameters session", EnableSession = true)]
        public static bool CreateContactParametersSession(string contactExpressionType, string postbackControlId, string contactSectionPosition, string processType)
        {
            ExpressionType contactExpressionTypeEnum = EnumUtil<ExpressionType>.Parse(contactExpressionType);
            ACAConstant.ContactSectionPosition contactSectionPositionEnum = EnumUtil<ACAConstant.ContactSectionPosition>.Parse(contactSectionPosition);
            ContactProcessType processTypeEnum = EnumUtil<ContactProcessType>.Parse(processType);

            ContactSessionParameter sessionParameter = new ContactSessionParameter();
            sessionParameter.Process.ContactProcessType = processTypeEnum;
            sessionParameter.Process.CallbackFunctionName = postbackControlId;
            sessionParameter.ContactSectionPosition = contactSectionPositionEnum;
            sessionParameter.ContactExpressionType = contactExpressionTypeEnum;
            sessionParameter.Data.DataObject = new PeopleModel4WS();

            AppSession.SetContactSessionParameter(sessionParameter);
            return true;
        }

        /// <summary>
        /// Check unique Email address from Javascript of AccountEdit
        /// </summary>
        /// <param name="emailAddress">Email address</param>
        /// <returns>Validation Information</returns>
        [WebMethod(Description = "is existed email", EnableSession = true)]
        public static string IsExistEmail(string emailAddress)
        {
            PublicUserModel4WS model = AppSession.User.UserModel4WS;

            if (model.email.Equals(emailAddress, StringComparison.InvariantCultureIgnoreCase))
            {
                return string.Empty;
            }

            return UserUtil.IsExistEmail(emailAddress);
        }

        /// <summary>
        /// Uncheck unique user name from Javascript of AccountEdit
        /// </summary>
        /// <param name="userID">string user id</param>
        /// <returns>Validation Information</returns>
        [WebMethod(Description = "is existed user name", EnableSession = true)]
        public static string IsExistUserName(string userID)
        {
            //The user name of control is readonly and it is valid so needn't validator
            return string.Empty;
        }

        /// <summary>
        /// Check password security
        /// </summary>
        /// <param name="password">the password</param>
        /// <param name="userName">user name for login</param>
        /// <returns>Check result</returns>
        [WebMethod(Description = "Check password security", EnableSession = true)]
        public static string CheckPasswordSecurity(string password, string userName)
        {
            return AccountUtil.CheckPasswordSecurity(password, userName, false);
        }

        /// <summary>
        /// Raises the System.Web.UI.Page.SaveStateComplete event after the page state
        /// has been saved to the persistence medium.
        /// </summary>
        /// <param name="e">A System.EventArgs object containing the event data.</param>
        protected override void OnSaveStateComplete(EventArgs e)
        {
            if (!AppSession.IsAdmin && AppSession.User.IsAuthorizedAgent)
            {
                lblAccountType.LabelKey = "aca_authagent_accountmanager_label_agenttype";
            }
            else if (IsExistLicenses())
            {
                lblAccountType.LabelKey = "acc_manage_text_AccountType";
            }
            else
            {
                lblAccountType.LabelKey = "acc_manage_text_AccountType_citizenType";
            }
        }

        /// <summary>
        /// Initialize Event Handler
        /// </summary>
        /// <param name="e">A System.EventArgs object containing the event data.</param>
        protected override void OnInit(EventArgs e)
        {
            if (!IsPostBack)
            {
                //Clear UI data before initialize sections.
                UIModelUtil.ClearUIData();
            }

            base.OnInit(e);

            AccelaButton btnAddClerk = new AccelaButton();
            btnAddClerk.LabelKey = "aca_authagent_accountmanager_label_addclerk";
            btnAddClerk.DivDisableCss = "ACA_SmButtonDisable ACA_SmButtonDisable_FontSize";
            btnAddClerk.DivEnableCss = "ACA_SmButton ACA_SmButton_FontSize";
            btnAddClerk.Click += AddClerkButton_Click;
            sectionTitleBar.AddToolBarControls(btnAddClerk);
            sectionTitleBar.PermissionValueId = btnAddClerk.ClientID;
        }

        /// <summary>
        /// Raises the page load event
        /// </summary>
        /// <param name="sender">An object that contains the event sender.</param>
        /// <param name="e">A System.EventArgs object containing the event data.</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            // display view pattern when it is view state, otherwise displays edit pattern control
            this.InitControlPresentation();
            DialogUtil.RegisterScriptForDialog(this.Page);

            lblAttachmentTitle.SectionID = string.Format(
                                                    "{1}{0}{2}{0}{3}",
                                                    ACAConstant.SPLIT_CHAR,
                                                    ModuleName,
                                                    GviewID.PeopleAttachment,
                                                    attachmentEdit.ClientID + "_dlDocumentEdit_ctl00_documentEdit_");

            if (!AppSession.IsAdmin && AppSession.User.IsAnonymous)
            {
                Response.Redirect(ACAConstant.URL_DEFAULT);
            }

            if (!Page.IsPostBack)
            {
                AppSession.SetContactSessionParameter(null);
                AppSession.SetRegisterContactSessionParameter(null);

                //If disable account manage link, it should directly to default page.
                if (!AppSession.IsAdmin && !StandardChoiceUtil.IsAccountManagementEnabled())
                {
                    Response.Redirect(ACAConstant.URL_DEFAULT);
                }

                if (!AppSession.IsAdmin && !StandardChoiceUtil.IsEnableProxyUser())
                {
                    divMyDelegateUserView.Visible = false;
                }

                // display current user information to Contact and Account view controlp
                DisplayRefContactList();
                PublicUserModel4WS model = AppSession.User.UserModel4WS;
                this.accountView.Display(model);
                ControlBuildHelper.HideStandardFields(GviewID.RegistrationContactForm, ModuleName, Controls, AppSession.IsAdmin);
                userLicenseList.DisplayLicenseList(AppSession.User.UserSeqNum);
  
                //display trust account list.
                IList<TrustAccountModel> trustAcct = GetTrustAccountList();
                trustAcctList.BindList(trustAcct);

                //The add licnese  link enable is configured in ACA admin.
                btnAddLicense.Visible = !StandardChoiceUtil.DisabledAddLicense();

                //display add license successful info.
                DisplayMsg();

                if (AppSession.IsAdmin || AppSession.User.IsAuthorizedAgent)
                {
                    divAgentClerk.Visible = true;
                    agentClerkList.BindClerkList();
                }

                if (!AppSession.IsAdmin && AppSession.User.IsAuthorizedAgent)
                {
                    divLicense.Visible = false;
                    divAttachment.Visible = false;
                    divTrustAccount.Visible = false;
                    divMyDelegateUserView.Visible = false;
                }

                ControlBuildHelper.SetInstructionValue(this.acc_manage_label_accountInfo_sub_label, GetTextByKey("acc_manage_label_accountInfo|sub"));
                ControlBuildHelper.SetInstructionValue(this.acc_manage_label_contactInfo_sub_label, GetTextByKey("acc_manage_label_contactInfo|sub"));
                ControlBuildHelper.SetInstructionValue(this.acc_manage_label_licenseInfo_sub_label, GetTextByKey("acc_manage_label_licenseInfo|sub"));
                ControlBuildHelper.SetInstructionValue(this.lblMyProxyUsers_sub_label, GetTextByKey("aca_delegate_section_title|sub"));
            }

            /*
             * Below scenarios disallow user to edit the user information:
             * 1. Is external authentication adapter.
             * 2. Is internal authentication adapter but the LDAP authentication is enabled.
             */
            if (!AuthenticationUtil.IsInternalAuthAdapter || StandardChoiceUtil.IsEnableLdapAuthentication())
            {
                btnEditAccount.Visible = false;
                btnAddContact.Visible = false;
            }

            btnAddContact.Visible = StandardChoiceUtil.IsEnabelManualContactAssociation();
            
            if (!AppSession.IsAdmin && AppSession.User.IsAuthorizedAgent)
            {
                btnAddContact.Visible = false;
            }
        }

        /// <summary>
        /// OnPreRender event.
        /// </summary>
        /// <param name="e">event handle.</param>
        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);
            InitPeopleAttachment();

            if (!Page.IsPostBack)
            {
                // Clear some temporary data stored in seestion for expression 
                ExpressionUtil.ClearExpressionVariables();
            }
        }

        /// <summary>
        /// Raise add license click event
        /// </summary>
        /// <param name="sender">An object that contains the event sender.</param>
        /// <param name="e">An System.EventArgs object that contains the event data.</param>
        protected void AddLicenseButton_Click(object sender, EventArgs e)
        {
            string url = string.Format("SearchLicense.aspx?{0}={1}", UrlConstant.IS_FROM_ACCOUNT_MANAGER, ACAConstant.COMMON_Y);
            Response.Redirect(url);
        }

        /// <summary>
        /// Rebind account contact list.
        /// </summary>
        /// <param name="sender">An object that contains the event sender.</param>
        /// <param name="e">An System.EventArgs object that contains the event data.</param>
        protected void AccountContactRefreshButton_Click(object sender, EventArgs e)
        {
            DisplayRefContactList();
        }

        /// <summary>
        /// New clerk event handler.
        /// </summary>
        /// <param name="sender">An object that contains the event sender.</param>
        /// <param name="e">An System.EventArgs object that contains the event data.</param>
        protected void AddClerkButton_Click(object sender, EventArgs e)
        {
            Response.Redirect(string.Format("RegisterEdit.aspx?{0}={1}", UrlConstant.IS_FOR_CLERK, ACAConstant.COMMON_Y));
        }

        /// <summary>
        /// display clew info for active license to the user 
        /// </summary>
        private void DisplayMsg()
        {
            string returnCode = Request.QueryString["returnCode"];
            string licenseNbr = Request.QueryString["licenseNbr"];
            string msg = string.Empty;
            MessageType msgType = MessageType.Error;
            string successMsg = string.Empty;

            if (string.IsNullOrEmpty(licenseNbr) || string.IsNullOrEmpty(returnCode))
            {
                return;
            }

            IssueLicenseReturnCode returnCodeEnum = (IssueLicenseReturnCode)int.Parse(returnCode);

            switch (returnCodeEnum)
            {
                case IssueLicenseReturnCode.ISSUE_SUCCESS_AUTO_APPROVED: //Issue license with auto approved successfully.
                    msg = GetTextByKey("acc_message_success_addapprovedlicense");
                    successMsg = DataUtil.StringFormat(msg, licenseNbr);

                    if (Convert.ToBoolean(Request["isLicenseExpired"]))
                    {
                        // append the license expiration message.
                        successMsg += "<br/>" + GetTextByKey("acc_message_expiredlicense");
                    }

                    msg = successMsg;
                    msgType = MessageType.Success;
                    break;
                case IssueLicenseReturnCode.ISSUE_SUCCESS: //Issue license successfully, need waiting for approval.
                    msg = GetTextByKey("acc_message_succe_addedLicense");
                    successMsg = DataUtil.StringFormat(msg, licenseNbr);

                    if (Convert.ToBoolean(Request["isLicenseExpired"]))
                    {
                        // append the license expiration message.
                        successMsg += "<br/>" + GetTextByKey("acc_message_expiredlicense");
                    }

                    msg = successMsg;
                    msgType = MessageType.Success;
                    break;
                case IssueLicenseReturnCode.LICENSE_CONNECTED_THE_ACCOUNT: //Already has the same type license
                    msg = GetTextByKey("acc_message_error_existLicense");
                    break;
                case IssueLicenseReturnCode.LIC_INEXISTENCE: //license is not exist
                    msg = GetTextByKey("acc_message_error_inexistenceLicense");
                    break;
                case IssueLicenseReturnCode.LIC_INVALID: //license is invalid
                    msg = GetTextByKey("acc_message_error_invalidLicense");
                    break;
            }

            if (!string.IsNullOrEmpty(msg))
            {
                MessageUtil.ShowMessage(Page, msgType, msg);
            }
        }

        /// <summary>
        /// Initial control presentation.
        /// </summary>
        private void InitControlPresentation()
        {
            if (!IsEditLogin)
            {
                this.accountView.Visible = true;
            }
            else
            {
                this.accountView.Visible = false;
            }
        }

        /// <summary>
        /// Indicates there license information.
        /// </summary>
        /// <returns>true if exist license; otherwise, false.</returns>
        private bool IsExistLicenses()
        {
            bool isExistLicenses = false;

            // if there is no license professional, hide the license info title.
            ContractorLicenseModel4WS[] contractorLicenses = LicenseUtil.GetContractorLicenseByUserSeqNumber(ConfigManager.AgencyCode, AppSession.User.UserSeqNum);
            isExistLicenses = contractorLicenses != null && contractorLicenses.Length > 0;

            return isExistLicenses;
        }

        /// <summary>
        /// display trust account list.
        /// </summary>
        /// <returns>trust account model array.</returns>
        private IList<TrustAccountModel> GetTrustAccountList()
        {
            IList<TrustAccountModel> trustAcct = null;

            if (!AppSession.IsAdmin)
            {
                ITrustAccountBll trustAccountBll = (ITrustAccountBll)ObjectFactory.GetObject(typeof(ITrustAccountBll));
                trustAcct = trustAccountBll.GetTrustAccountListByPublicUserID(AppSession.User.UserSeqNum);
            }

            return trustAcct;
        }

        /// <summary>
        /// Initialize people attachment function.
        /// </summary>
        private void InitPeopleAttachment()
        {
            if (!AppSession.IsAdmin)
            {
                /*
                 * 1- If there is not exists approved license or approved contact, hide the attachment section;
                 * 2- Enable account attachment;
                 * 3- Does not display attachment list for Agent.
                 */
                int approvedLicenseNum = AttachmentUtil.GetAvaliableLicense4PeopleDocument(ConfigManager.AgencyCode).Count + AttachmentUtil.GetAvaliableContact4PeopleDocument().Count;
                bool isActive = approvedLicenseNum > 0 && StandardChoiceUtil.IsEnableAccountAttachment() && !AppSession.User.IsAuthorizedAgent;

                divAttachment.Visible = isActive;
            }
        }

        /// <summary>
        /// Display contact that associated with user.
        /// </summary>
        private void DisplayRefContactList()
        {
            PeopleModel4WS[] refPeoples = null;

            if (!AppSession.IsAdmin)
            {
                refPeoples = AppSession.User.UserModel4WS.peopleModel;
            }

            refContactList.RefContactsDataSource = TempModelConvert.ConvertToPeopleModel(refPeoples, true);
            refContactList.BindRefContactsList();
        }

        #endregion Methods
    }
}
