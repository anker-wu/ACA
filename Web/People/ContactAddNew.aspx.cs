#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: ContactAddNew.aspx.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2013-2014
 *
 *  Description:
 *
 *  Notes:
 * $Id: ContactAddNew.aspx.cs 151830 2013-10-09 13:39:43Z ACHIEVO\blues.gao $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using System;
using System.Linq;
using System.Text;
using System.Web.Services;
using System.Web.UI;

using Accela.ACA.BLL.Account;
using Accela.ACA.BLL.Common;
using Accela.ACA.BLL.People;
using Accela.ACA.Common;
using Accela.ACA.Common.Common;
using Accela.ACA.Common.Util;
using Accela.ACA.Web.Common;
using Accela.ACA.Web.ExpressionBuild;
using Accela.ACA.Web.Util;
using Accela.ACA.WSProxy;

namespace Accela.ACA.Web.People
{
    /// <summary>
    /// the class of Contact edit 
    /// </summary>
    public partial class ContactAddNew : PeoplePopupBasePage
    {
        #region Fields

        /// <summary>
        /// A value indicating whether this page is back from other page.
        /// </summary>
        private bool _isBack;

        /// <summary>
        /// A value indicating whether this contact is close match.
        /// </summary>
        private bool _isCloseMatch;

        #endregion Fields

        #region Properties

        /// <summary>
        /// Gets or sets a value indicating whether the data is new.
        /// </summary>
        protected bool IsForNew
        {
            get
            {
                object temp = ViewState["IsForNew"];
                bool result = temp == null || bool.Parse(temp.ToString());
                return result;
            }

            set
            {
                ViewState["IsForNew"] = value;
            }
        }

        /// <summary>
        /// Gets or sets the reference contact sequence number.
        /// </summary>
        protected string RefContactSeqNbr
        {
            get
            {
                return Convert.ToString(ViewState["RefContactSeqNbr"]);
            }

            set
            {
                ViewState["RefContactSeqNbr"] = value;
            }
        }

        /// <summary>
        /// Gets a value indicating whether the agent clerk action.
        /// </summary>
        protected bool IsForClerk
        {
            get
            {
                return ValidationUtil.IsYes(Request.QueryString[UrlConstant.IS_FOR_CLERK]);
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the clearing contact is reference contact or not.
        /// </summary>
        protected bool IsClearRefContact
        {
            get
            {
                return Convert.ToBoolean(ViewState["IsClearRefContact"]);
            }

            set
            {
                ViewState["IsClearRefContact"] = value;
            }
        }

        /// <summary>
        /// Gets a value indicating whether the current contact is approved.
        /// </summary>
        /// <returns>true or false.</returns>
        private bool IsContactApproved
        {
            get
            {
                bool isContactApproved = false;
                string contactSeqNbr = Request.QueryString[UrlConstant.CONTACT_SEQ_NUMBER];

                if (!string.IsNullOrEmpty(contactSeqNbr))
                {
                    PeopleModel4WS[] approvedContacts = AppSession.User.ApprovedContacts;

                    if (approvedContacts != null && approvedContacts.Count(p => contactSeqNbr.Equals(p.contactSeqNumber)) > 0)
                    {
                        isContactApproved = true;
                    }
                }

                return isContactApproved;
            }
        }

        /// <summary>
        /// Gets the url of Register Account Confirm page
        /// </summary>
        private string RegisterAccountConfirmUrl
        {
            get
            {
                return Page.ResolveUrl("~/Account/RegisterAccountConfirm.aspx") + Request.Url.Query;
            }
        }

        #endregion Properties

        #region Methods

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
            AppSession.SetContactSessionParameter(sessionParameter);
        }

        /// <summary>
        /// get section fields info by section id
        /// </summary>
        /// <param name="agencyCode">The agency code</param>
        /// <param name="moduleName">module name</param>
        /// <param name="permissionValue">The permission value</param>
        /// <param name="controlPrefix">control prefix.</param>
        /// <returns>a json include the fields info</returns>
        [System.Web.Services.WebMethod(Description = "GetSectionFields", EnableSession = true)]
        public static string GetStandardFieldsRequriedInfo(string agencyCode, string moduleName, string permissionValue, string controlPrefix)
        {
            string json = string.Empty;
            IGviewBll gviewBll = ObjectFactory.GetObject<IGviewBll>();
            json = gviewBll.GetSimpleViewElementByJsonFormat(agencyCode, moduleName, GviewID.ContactEdit, controlPrefix, GViewConstant.PERMISSION_PEOPLE, ScriptFilter.DecodeJson(permissionValue), AppSession.User.UserID);

            return json;
        }

        /// <summary>
        /// On Initial event
        /// </summary>
        /// <param name="e">event arguments</param>
        protected override void OnInit(EventArgs e)
        {
            ContactSessionParameter parametersModel = AppSession.GetContactSessionParameter();
            ContactSessionParameter parametersModelRegister = AppSession.GetRegisterContactSessionParameter();
            _isBack = ValidationUtil.IsYes(Request.QueryString[UrlConstant.IS_BACK]);

            if (!IsPostBack
                && !_isBack
                && parametersModel != null
                && (parametersModel.ContactSectionPosition == ACAConstant.ContactSectionPosition.RegisterAccount
                    || parametersModel.ContactSectionPosition == ACAConstant.ContactSectionPosition.RegisterClerk
                    || parametersModel.ContactSectionPosition == ACAConstant.ContactSectionPosition.EditClerk
                    || parametersModel.ContactSectionPosition == ACAConstant.ContactSectionPosition.RegisterExistingAccount)
                && parametersModelRegister != null)
            {
                parametersModel.Data = ObjectCloneUtil.DeepCopy(parametersModelRegister.Data);
                AppSession.SetContactSessionParameter(parametersModel);
            }

            if (!_isBack && ContactSessionParameterModel.Data.IsCloseMatch)
            {
                Response.Redirect(RegisterAccountConfirmUrl + string.Format("&{0}={1}", UrlConstant.NEED_IDENTIFY_CHECK, ACAConstant.COMMON_NO));
            }

            if (_isBack && ContactSessionParameterModel.Data.IsCloseMatch)
            {
                // Only Register Clerk/Account page can be back to this page, so ContactSectionPosition either ContactSectionPosition.RegisterClerk or ContactSectionPosition.RegisterAccount
                ContactSessionParameterModel.ContactSectionPosition = IsForClerk ? ACAConstant.ContactSectionPosition.RegisterClerk : ACAConstant.ContactSectionPosition.RegisterAccount;
                ContactSessionParameterModel.Data.IsCloseMatch = false;
            }

            ucContactInfo.ParentID = ParentID;
            ucContactInfo.ContactSectionPosition = ContactSectionPosition;
            ucContactInfo.ContactExpressionType = ContactExpressionType;
            ucContactInfo.ValidateFlag = ContactSessionParameterModel.PageFlowComponent.ComponentDataSource;
            ucContactInfo.IsMultipleContact = IsMultipleContact;

            if (!AppSession.IsAdmin
                && parametersModel != null
                && parametersModel.ContactSectionPosition == ACAConstant.ContactSectionPosition.EditClerk)
            {
                btnClear.Visible = false;
            }

            base.OnInit(e);
        }

        /// <summary>
        /// Handles the Load event of the Page control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            SetDialogMaxHeight("550");
            SetDialogFixedWidth("800");

            if (!IsPostBack)
            {
                IsForNew = ContactSessionParameterModel.Process.ContactProcessType == ContactProcessType.Add
                    || UrlConstant.NEW_FLAG.Equals(Request.QueryString[UrlConstant.NEW]);

                if (ContactSectionPosition.Equals(ACAConstant.ContactSectionPosition.EditClerk))
                {
                    liClearBtn.Visible = false;
                }

                if (ContactSessionParameterModel.Process.ContactProcessType == ContactProcessType.Add)
                {
                    liClearBtn.Visible = true;
                }

                if (AppSession.IsAdmin && ACAConstant.ContactSectionPosition.SpearForm.Equals(ContactSectionPosition))
                {
                    ucContactInfo.SetSpearFormCloseMatch();
                }
            }

            // Initialize the page's title
            InitTitle();

            ucContactInfo.SetSectionRequired("0");
            ucContactInfo.IsOpenedFromContactAddNew = true;
            ucContactInfo.ContactTypeFlagChangedEvent += new CommonEventHandler(ContactInfo_ContactTypeFlagChangedEvent);

            if (AppSession.IsAdmin)
            {
                //The AddClerk and EditClerk have different treet note in ACA admin, so the Clear button should not be visible in EditClerk page.
                if (!ContactSectionPosition.Equals(ACAConstant.ContactSectionPosition.EditClerk))
                {
                    liClearBtn.Visible = true;
                }

                ucContactInfo.IsEditable = true;

                if (!IsPostBack)
                {
                    ucContactInfo.ContactType = ContactType;
                }
            }
            else if (!IsPostBack)
            {
                ucContactInfo.IsEditable = IsForNew || IsContactApproved;
                ucContactInfo.IsForNew = IsForNew;
                ucContactInfo.ContactType = ContactType;
                
                CapModel4WS capModel = AppSession.GetCapModelFromSession(ModuleName);
                PeopleModel4WS people = LoadContactInfo(ContactSessionParameterModel);
                SwitchContinueBtnDisplay(capModel, people.contactTypeFlag);

                ucContactInfo.IsRefContact = !string.IsNullOrEmpty(RefContactSeqNbr);

                if ((ComponentDataSource.Reference.Equals(ContactSessionParameterModel.PageFlowComponent.ComponentDataSource) && !string.IsNullOrEmpty(RefContactSeqNbr))
                    || !ContactSessionParameterModel.PageFlowComponent.IsEditable)
                {
                    btnClear.Enabled = false;
                }

                if (!ContactSessionParameterModel.PageFlowComponent.IsEditable)
                {
                    if (!TemplateUtil.IsExistsAlwaysEditableRequiredTemplateField(people.attributes))
                    {
                        btnSave.Enabled = false;
                        btnSave.OnClientClick = string.Empty;
                    }
                }

                CapContactModel4WS capContactModel = ContactSessionParameterModel.Data.DataObject as CapContactModel4WS;

                if (ContactUtil.IsPrimaryContact(capContactModel))
                {
                    btnClear.OnClientClick = "return ClearContact();";
                }

                ucContactInfo.ContactTypeFlag = people.contactTypeFlag;
            }
        }

        /// <summary>
        /// OnPreRender event
        /// </summary>
        /// <param name="e">event arguments</param>
        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);

            // Don't run expression in administration model.
            if (!AppSession.IsAdmin)
            {
                ExpressionUtil.RegisterScriptLibToCurrentPage(this);

                if (!Page.IsPostBack)
                {
                    RegisterExpressionOnLoad();
                }

                RegisterExpressionOnSubmit();
                ExpressionUtil.ResetJsExpression(this);
            }
        }

        /// <summary>
        /// Handle the LoadComplete event.
        /// </summary>
        /// <param name="e">Event argument</param>
        protected override void OnLoadComplete(EventArgs e)
        {
            base.OnLoadComplete(e);

            if (ScriptManager.GetCurrent(Page).IsInAsyncPostBack)
            {
                // Handle ASIT expression behaviors.
                ExpressionUtil.HandleASITPostbackBehavior(Page);
            }
        }

        /// <summary>
        /// Click Save and Close button event handler. 
        /// Save the contact information and close this page.
        /// </summary>
        /// <param name="sender">An object that contains the event sender.</param>
        /// <param name="e">A System.EventArgs object containing the event data.</param>
        protected void SaveAndCloseButton_Click(object sender, EventArgs e)
        {
            if (!SaveContact())
            {
                return;
            }

            StringBuilder script = new StringBuilder();

            if (_isCloseMatch)
            {
                script.Append("if(typeof (parent.RefreshAttachmentList) != 'undefined'){parent.RefreshAttachmentList();} ");
            }
            
            string isClearRefContact = IsClearRefContact ? ACAConstant.COMMON_Y : ACAConstant.COMMON_N;
            script.AppendFormat("PopupClose('{0}');", isClearRefContact);
            ScriptManager.RegisterStartupScript(Page, GetType(), "SaveContactOrRefreshAttachmentList", script.ToString(), true);
        }

        /// <summary>
        /// Click Continue button event handler. 
        /// Save the contact information and go to detail page.
        /// </summary>
        /// <param name="sender">An object that contains the event sender.</param>
        /// <param name="e">A System.EventArgs object containing the event data.</param>
        protected void ContinueButton_Click(object sender, EventArgs e)
        {
            if (ContactSectionPosition == ACAConstant.ContactSectionPosition.SpearForm)
            {
                if (!SaveContact())
                {
                    return;
                }

                string url = string.Format(
                    "~/LicenseCertification/RefContactEducationExamLookUp.aspx?contactSeqNbr={0}&{1}={2}&isPopup=Y",
                    RefContactSeqNbr,
                    ACAConstant.MODULE_NAME,
                    ModuleName);

                Response.Redirect(url);
                Response.End();
            }

            // Get user info from control
            PeopleModel4WS publicUserPeople = ucContactInfo.GetPeopleModel();
            ContactSessionParameter parametersModel = AppSession.GetContactSessionParameter();
            PeopleModel4WS people = ContactUtil.GetPeopleModelFromContactSessionParameter(parametersModel);
            publicUserPeople.contactAddressList = people.contactAddressList;

            if (!string.IsNullOrEmpty(publicUserPeople.contactSeqNumber)
                && int.Parse(publicUserPeople.contactSeqNumber) < 0)
            {
                publicUserPeople.contactSeqNumber = string.Empty;
            }

            PublicUserModel4WS model = PeopleUtil.GetPublicUserFromSession();
            model.peopleModel = new PeopleModel4WS[1] { publicUserPeople };

            try
            {
                IPeopleBll peopleBll = ObjectFactory.GetObject<IPeopleBll>();
                PeopleModel peopleModel4Search = TempModelConvert.ConvertToPeopleModel(publicUserPeople);

                // when country hidden, clear the country code in search model
                if (ucContactInfo.IsCountryHidden)
                {
                    peopleModel4Search.countryCode = string.Empty;
                    peopleModel4Search.compactAddress.countryCode = string.Empty;
                }

                PeopleModel peopleModel = peopleBll.GetPeopleByClosematch(peopleModel4Search);

                if (peopleModel != null)
                {
                    //Store the user input data to session to support the Back function from Account Confirm page.
                    parametersModel.Data.DataObject = publicUserPeople;
                    AppSession.SetContactSessionParameter(parametersModel);

                    //use reference contact.
                    PeopleModel4WS referencePeople = TempModelConvert.ConvertToPeopleModel4WS(peopleModel);

                    if (IsForClerk)
                    {
                        referencePeople.contactType = model.peopleModel[0].contactType;
                        GenericTemplateUtil.MergeGenericTemplate(peopleModel.template, model.peopleModel[0].template, string.Empty);
                        referencePeople.attributes = model.peopleModel[0].attributes;
                        SetPeopleTemlateSequenceNumber(referencePeople.contactSeqNumber, referencePeople.attributes);
                        referencePeople.template = model.peopleModel[0].template;
                    }

                    model.peopleModel = new[] { referencePeople };

                    PeopleUtil.SavePublicUserToSession(model);
                    Response.Redirect(RegisterAccountConfirmUrl);
                }
                else
                {
                    string identityKeyMessage = PeopleUtil.GetIdentityKeyMessage(model.peopleModel[0], PeopleUtil.IdentityFieldLabels, "aca_message_duplicate_contact_identity_reg");

                    if (!string.IsNullOrEmpty(identityKeyMessage))
                    {
                        PeopleUtil.SavePublicUserToSession(model);
                        MessageUtil.ShowMessageInPopupScrollTop(Page, MessageType.Error, identityKeyMessage);
                        return;
                    }

                    IAccountBll accountBll = ObjectFactory.GetObject<IAccountBll>();

                    if (!string.IsNullOrEmpty(model.userSeqNum))
                    {
                        // Update password
                        if (!string.IsNullOrEmpty(model.password))
                        {
                            accountBll.UpdateNeedChangePassword(ConfigManager.AgencyCode, model.userSeqNum, ACAConstant.COMMON_Y, AppSession.User.PublicUserId);

                            // Add this line to refresh daily end cache
                            model.needChangePassword = ACAConstant.COMMON_Y;
                        }

                        // Update public user
                        accountBll.EditAuthAgentClerk(model, AppSession.User.PublicUserId);

                        // Refresh to Account Manager page
                        ScriptManager.RegisterStartupScript(Page, GetType(), "RefreshToAccountManager", "RefreshToAccountManager();", true);
                    }
                    else
                    {
                        // Get license array
                        LicenseModel4WS[] arrayLicense = PeopleUtil.GetArrayLicense();

                        // Create public user
                        string userSeqNum = accountBll.CreatePublicUser(model, arrayLicense);
                        model.userSeqNum = userSeqNum;
                        PeopleUtil.SavePublicUserToSession(model);

                        // Refresh to register confirm page
                        ScriptManager.RegisterStartupScript(Page, GetType(), "SaveContact", "PopupClose();", true);
                    }
                }
            }
            catch (ACAException ex)
            {
                MessageUtil.ShowMessageInPopupScrollTop(Page, MessageType.Error, ex.Message);
            }
        }

        /// <summary>
        /// Clear button click event.
        /// </summary>
        /// <param name="sender">An object that contains the event sender.</param>
        /// <param name="e">A System.EventArgs object containing the event data.</param>
        protected void ClearButton_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(RefContactSeqNbr))
            {
                IsClearRefContact = true;
                liSaveBtn.Visible = true;
                liContinueBtn.Visible = false;
            }

            ucContactInfo.ResetContactForm(true, false);
            ucContactInfo.ClearExpressionValue(true);
            ucContactInfo.ClearAddressList();
            RefContactSeqNbr = string.Empty;
            btnClear.OnClientClick = string.Empty;
            ucContactInfo.PrimaryContactFlag = string.Empty;
            Page.FocusElement(btnClear.ClientID);
            ContactUtil.SetContactAddressListToContactSessionParameter(null);
        }

        /// <summary>
        /// Save the contact to the session or DB.
        /// </summary>
        /// <returns>
        /// save successful
        /// </returns>
        private bool SaveContact()
        {
            ContactSessionParameter parametersModel = AppSession.GetContactSessionParameter();
            bool result = SaveContactToSession(parametersModel);

            if (!result)
            {
                return false;
            }

            switch (parametersModel.ContactSectionPosition)
            {
                case ACAConstant.ContactSectionPosition.AddReferenceContact:
                case ACAConstant.ContactSectionPosition.ModifyReferenceContact:
                    ContactSessionParameter parametersModelClone = ObjectCloneUtil.DeepCopy(parametersModel);
                    result = SaveContactToDataBase(parametersModelClone);

                    if (result)
                    {
                        PeopleModel4WS peopleModel = ContactUtil.GetPeopleModelFromContactSessionParameter(parametersModelClone);
                        peopleModel.contactAddressList = null;
                        parametersModel.Data.DataObject = peopleModel;
                    }

                    break;
            }

            return result;
        }

        /// <summary>
        /// Save the contact to the session
        /// </summary>
        /// <param name="parametersModel">The parameters model.</param>
        /// <returns>
        /// save to session successful
        /// </returns>
        private bool SaveContactToSession(ContactSessionParameter parametersModel)
        {
            bool result = false;

            try
            {
                ContactAddressModel[] contactAddressList = ucContactInfo.GetContactAddressList();

                if (!ucContactInfo.IsDataValid())
                {
                    string errorMsg = GetTextByKey("aca_contactedit_msg_referencedatarequired");
                    MessageUtil.ShowMessageInPopupScrollTop(Page, MessageType.Error, errorMsg);
                    return false;
                }

                if (!ucContactInfo.IsPassContactAddressValidation(contactAddressList))
                {
                    return false;
                }

                // Spear Form
                if (parametersModel.Data.DataObject is CapContactModel4WS)
                {
                    CapContactModel4WS paramContact = parametersModel.Data.DataObject as CapContactModel4WS;

                    CapContactModel4WS capContact = ucContactInfo.GetContactModel();
                    capContact.people.RowIndex = parametersModel.Process.ContactProcessType != ContactProcessType.Edit ? null : paramContact.people.RowIndex;
                    capContact.people.contactAddressList = contactAddressList;
                    capContact.people.contactSeqNumber = paramContact.people.contactSeqNumber;
                    capContact.componentName = parametersModel.PageFlowComponent.ComponentName;

                    parametersModel.Data.DataObject = capContact;
                }
                else if (parametersModel.Data.DataObject is PeopleModel4WS)
                {
                    //Acount
                    PeopleModel4WS paramPeople = parametersModel.Data.DataObject as PeopleModel4WS;

                    PeopleModel4WS people = ucContactInfo.GetPeopleModel();
                    people.RowIndex = paramPeople.RowIndex;
                    people.contactAddressList = contactAddressList;
                    people.contactSeqNumber = paramPeople.contactSeqNumber;

                    if (ContactSectionPosition == ACAConstant.ContactSectionPosition.EditClerk)
                    {
                        string identityKeyMessage = PeopleUtil.GetIdentityKeyMessage(people, PeopleUtil.IdentityFieldLabels, "aca_accountcontactedit_msg_duplicate_contact_forupdate");

                        if (!string.IsNullOrEmpty(identityKeyMessage))
                        {
                            MessageUtil.ShowMessageInPopupScrollTop(Page, MessageType.Error, identityKeyMessage);
                            return false;
                        }
                    }

                    parametersModel.ContactType = people.contactType;
                    parametersModel.Data.DataObject = people;
                }

                PeopleUtil.SaveTempIsCountryHidden(ucContactInfo.IsCountryHidden);

                PublicUserModel4WS publicUserModel4SSO = PeopleUtil.GetPublicUserFromSession();
                bool isFromSSOLink = AuthenticationUtil.IsNeedRegistration 
                                     && !string.IsNullOrWhiteSpace(publicUserModel4SSO.userSeqNum)
                                     && publicUserModel4SSO.userSeqNum != ACAConstant.ANONYMOUS_FLAG;

                // Validate Close Match
                if (parametersModel.Process.ContactProcessType == ContactProcessType.Add
                    && parametersModel.ContactSectionPosition == ACAConstant.ContactSectionPosition.SpearForm
                    && ComponentDataSource.NoLimitation.Equals(parametersModel.PageFlowComponent.ComponentDataSource, StringComparison.OrdinalIgnoreCase)
                    && CloseMatch(parametersModel))
                {
                    // if auto sync people switch turn off, should close match and use reference contactSeqNumber only.
                    if (!StandardChoiceUtil.AutoSyncPeople(ModuleName, PeopleType.Contact))
                    {
                        PeopleModel4WS closeMatchPeople = ContactUtil.GetPeopleModelFromContactSessionParameter(parametersModel);
                        PeopleModel4WS userInputPeople = PeopleUtil.GetInputPeopleForCloseMatchFromSession();

                        PeopleUtil.SetContactAddressEntityID(userInputPeople.contactAddressList, closeMatchPeople.contactSeqNumber);

                        if (parametersModel.Data.DataObject is CapContactModel4WS)
                        {
                            CapContactModel4WS capContact = parametersModel.Data.DataObject as CapContactModel4WS;
                            capContact.people = userInputPeople;
                            capContact.refContactNumber = closeMatchPeople.contactSeqNumber;
                        }

                        PeopleUtil.RemoveInputPeopleForCloseMatchFromSession();
                        AppSession.SetContactSessionParameter(parametersModel);
                        ScriptManager.RegisterStartupScript(Page, GetType(), "SaveContact", "PopupClose(" + closeMatchPeople.contactSeqNumber + ");", true);
                    }
                    else
                    {
                        string url = Page.ResolveUrl("~/People/ContactCloseMatchConfirm.aspx") + Request.Url.Query;
                        Response.Redirect(url);
                    }
                }
                else if (((parametersModel.ContactSectionPosition == ACAConstant.ContactSectionPosition.RegisterAccount && !isFromSSOLink)
                            || parametersModel.ContactSectionPosition == ACAConstant.ContactSectionPosition.RegisterClerk
                            || parametersModel.ContactSectionPosition == ACAConstant.ContactSectionPosition.RegisterExistingAccount)
                        && CloseMatch(parametersModel))
                {
                    Response.Redirect(RegisterAccountConfirmUrl);
                }
                else
                {
                    AppSession.SetContactSessionParameter(parametersModel);

                    if (parametersModel.ContactSectionPosition == ACAConstant.ContactSectionPosition.RegisterAccount
                        || parametersModel.ContactSectionPosition == ACAConstant.ContactSectionPosition.RegisterExistingAccount
                        || parametersModel.ContactSectionPosition == ACAConstant.ContactSectionPosition.RegisterClerk
                        || parametersModel.ContactSectionPosition == ACAConstant.ContactSectionPosition.EditClerk)
                    {
                        ContactSessionParameter parametersModelClone = ObjectCloneUtil.DeepCopy(parametersModel);
                        AppSession.SetRegisterContactSessionParameter(parametersModelClone);
                    }
                }

                result = true;
            }
            catch (Exception exception)
            {
                MessageUtil.ShowMessageInPopupScrollTop(this, MessageType.Error, exception.Message);
            }

            return result;
        }

        /// <summary>
        /// Save the contact to DB.
        /// </summary>
        /// <param name="parametersModel">The parameters model.</param>
        /// <returns>
        /// save to DB successful
        /// </returns>
        private bool SaveContactToDataBase(ContactSessionParameter parametersModel)
        {
            bool result = false;

            try
            {
                PeopleModel4WS peopleModel4Ws = ContactUtil.GetPeopleModelFromContactSessionParameter(parametersModel);
                string caErrorMessage = string.Empty;

                if (StandardChoiceUtil.IsEnableContactAddress() && peopleModel4Ws != null)
                {
                    caErrorMessage = ucContactInfo.ValidateContactAddress(peopleModel4Ws.contactAddressList);
                }

                if (string.IsNullOrEmpty(caErrorMessage))
                {
                    IPeopleBll peopleBll = ObjectFactory.GetObject<IPeopleBll>();

                    if (IsForNew)
                    {
                        peopleModel4Ws.contactSeqNumber = string.Empty;
                        peopleModel4Ws.auditStatus = ACAConstant.VALID_STATUS;
                        PeopleModel peopleModel4Search = TempModelConvert.ConvertToPeopleModel(peopleModel4Ws);

                        // when country hidden, clear the country code in search model
                        if (ucContactInfo.IsCountryHidden)
                        {
                            peopleModel4Search.countryCode = string.Empty;
                            peopleModel4Search.compactAddress.countryCode = string.Empty;
                        }

                        PeopleModel matchedPeople = peopleBll.GetPeopleByClosematch(peopleModel4Search);

                        if (matchedPeople != null)
                        {
                            if (AppSession.User.UserModel4WS.peopleModel != null
                                && AppSession.User.UserModel4WS.peopleModel.Count(p => p.contactSeqNumber == matchedPeople.contactSeqNumber) > 0)
                            {
                                MessageUtil.ShowMessageInPopupScrollTop(Page, MessageType.Error, GetTextByKey("acc_message_contact_already_associated"));
                                return false;
                            }

                            _isCloseMatch = true;
                            peopleModel4Ws = TempModelConvert.ConvertToPeopleModel4WS(matchedPeople);

                            if (peopleModel4Ws.attributes != null)
                            {
                                foreach (TemplateAttributeModel attribute in peopleModel4Ws.attributes)
                                {
                                    attribute.templateObjectNum = peopleModel4Ws.contactSeqNumber;
                                } 
                            }
                        }
                        else
                        {
                            _isCloseMatch = false;
                            string identityKeyMessage = PeopleUtil.GetIdentityKeyMessage(peopleModel4Ws, PeopleUtil.IdentityFieldLabels, "aca_message_duplicate_contact_identity_add");

                            if (!string.IsNullOrEmpty(identityKeyMessage))
                            {
                                MessageUtil.ShowMessageInPopupScrollTop(Page, MessageType.Error, identityKeyMessage);
                                return false;
                            }
                        }

                        PublicUserModel4WS publicUser = new PublicUserModel4WS();
                        publicUser.servProvCode = ConfigManager.AgencyCode;
                        publicUser.userSeqNum = AppSession.User.UserSeqNum;
                        publicUser.peopleModel = new PeopleModel4WS[1] { peopleModel4Ws };
                        peopleBll.AddContact4PublicUser(publicUser);
                    }
                    else
                    {
                        peopleBll.EditRefContact(TempModelConvert.ConvertToPeopleModel(peopleModel4Ws));
                    }

                    // Clear the contact in the session
                    AppSession.ReloadPublicUserSession();
                    result = true;
                }
            }
            catch (Exception exception)
            {
                /*
                 * RefContactEditBefore and RefContactEditAfter event will be triggered 
                 *  in PeopleService.addContact4PublicUser and PeopleService.editRefContact.
                 */
                MessageUtil.ShowMessageInPopupScrollTop(this, MessageType.Error, exception.Message);
            }

            return result;
        }

        /// <summary>
        /// Load contact information.
        /// </summary>
        /// <param name="parametersModel">The parameters model.</param>
        /// <returns>The people model.</returns>
        private PeopleModel4WS LoadContactInfo(ContactSessionParameter parametersModel)
        {
            string contactSeqNbr = string.Empty;
            PeopleModel4WS people = ContactUtil.GetPeopleModelFromContactSessionParameter(parametersModel);

            CapContactModel4WS capContactModel = parametersModel.Data.DataObject as CapContactModel4WS;

            if (parametersModel.ContactSectionPosition == ACAConstant.ContactSectionPosition.SpearForm
                && parametersModel.Process.ContactProcessType != ContactProcessType.Add)
            {
                if (capContactModel != null)
                {
                    people = capContactModel.people;

                    if (people != null)
                    {
                        if (ContactSessionParameterModel.Process.ContactProcessType == ContactProcessType.Lookup
                            || ContactSessionParameterModel.Process.ContactProcessType == ContactProcessType.SelectContactFromCloseMatch
                            || ContactSessionParameterModel.Process.ContactProcessType == ContactProcessType.SelectContactFromAccount)
                        {
                            capContactModel.refContactNumber = people.contactSeqNumber;
                            RefContactSeqNbr = people.contactSeqNumber;
                            people.flag = string.Empty;
                        }
                        else if (ContactSessionParameterModel.Process.ContactProcessType == ContactProcessType.Edit)
                        {
                            RefContactSeqNbr = capContactModel.refContactNumber;
                            ucContactInfo.PrimaryContactFlag = people.flag;
                            parametersModel.ContactType = people.contactType;
                        }
                    }
                }
            }

            if (!IsForNew && people != null)
            {
                ucContactInfo.IsEditable = parametersModel.PageFlowComponent.IsEditable;
            }

            if (IsForNew && !_isBack)
            {
                ucContactInfo.LoadContactProperties();
                ucContactInfo.ClearAddressList();

                /* If people.contactAddressList is not null that means have added address data, 
                 * however if user refresh current page then should clear address list.
                 */
                if (people != null)
                {
                    ContactUtil.SetContactAddressListToContactSessionParameter(null, parametersModel);
                }
            }
            else if (people != null)
            {
                contactSeqNbr = people.contactSeqNumber;
                ucContactInfo.InitCountry();
                ucContactInfo.DisplayPeople(people, people.contactType, RefContactSeqNbr, capContactModel == null ? string.Empty : capContactModel.accessLevel);

                // if contact is pending or reject, cannot modify contact information.
                if (ContractorPeopleStatus.Pending.Equals(people.contractorPeopleStatus, StringComparison.OrdinalIgnoreCase) || ContractorPeopleStatus.Rejected.Equals(people.contractorPeopleStatus, StringComparison.OrdinalIgnoreCase))
                {
                    btnSave.Enabled = false;
                    string[] filterIds = TemplateUtil.GetAlwaysEditableControlIDs(people.attributes, ACAConstant.REFERENCE_CONTACT_TEMPLATE_FIELD_PREFIX);
                    ucContactInfo.DisableContactForm(false, filterIds);
                }
            }

            ucContactInfo.ContactSeqNumber = contactSeqNbr;
            return people;
        }

        /// <summary>
        /// register "java script" code into page to running expression for "on submit".
        /// </summary>
        private void RegisterExpressionOnLoad()
        {
            var callJsFunction = ExpressionUtil.GetExpressionScript(false, ucContactInfo);

            if (!Page.ClientScript.IsStartupScriptRegistered("OnLoadExpression"))
            {
                ScriptManager.RegisterStartupScript(this, typeof(Page), "OnLoadExpression", callJsFunction, true);
            }
        }

        /// <summary>
        /// register JS function.
        /// register "javascript" code into page to running expression for "on submit".
        /// </summary>
        private void RegisterExpressionOnSubmit()
        {
            var callJsFunction = ExpressionUtil.GetExpressionScript(true, ucContactInfo);
            var strSubmitFuction = ExpressionUtil.GetExpressionScriptOnSubmit(callJsFunction);

            if (!Page.ClientScript.IsStartupScriptRegistered("OnSubmitExpression"))
            {
                ScriptManager.RegisterStartupScript(this, typeof(Page), "OnSubmitExpression", strSubmitFuction, true);
            }
        }

        /// <summary>
        /// Initialize the page's title.
        /// </summary>
        private void InitTitle()
        {
            string labelKey = string.Empty;

            switch (ContactSectionPosition)
            {
                case ACAConstant.ContactSectionPosition.AddReferenceContact:
                    labelKey = "aca_manage_label_newcontact_title";
                    break;
                case ACAConstant.ContactSectionPosition.ModifyReferenceContact:
                    labelKey = "aca_manage_label_contactinfo_title";
                    break;
                case ACAConstant.ContactSectionPosition.RegisterAccount:
                    labelKey = "aca_registration_contactaddnew_label_edit_title";
                    break;
                case ACAConstant.ContactSectionPosition.RegisterClerk:
                    labelKey = "aca_accountmanagement_clerk_contactaddnew_label_new_title";
                    break;
                case ACAConstant.ContactSectionPosition.EditClerk:
                    labelKey = "aca_accountmanagement_clerk_contactaddnew_label_edit_title";
                    break;
                case ACAConstant.ContactSectionPosition.AuthAgentCustomerDetail:
                    labelKey = string.Empty;
                    break;
                case ACAConstant.ContactSectionPosition.SpearForm:
                default:
                    // This label key is for module level
                    labelKey = "aca_contactaddnew_label_edit_title";
                    break;
            }

            if (!AppSession.IsAdmin)
            {
                SetPageTitleKey(labelKey);
            }
            else
            {
                ucContactInfo.InitTitleBar(labelKey);
            }
        }

        /// <summary>
        /// Validate the current contact match with the contact in database.
        /// </summary>
        /// <param name="parametersModel">The parameters model.</param>
        /// <returns>A value indicating whether the current contact match with the contact in database.</returns>
        private bool CloseMatch(ContactSessionParameter parametersModel)
        {
            PeopleModel4WS inputPeople = ContactUtil.GetPeopleModelFromContactSessionParameter(parametersModel);
            PeopleModel searchPeople = TempModelConvert.ConvertToPeopleModel(inputPeople);

            if (parametersModel.ContactSectionPosition != ACAConstant.ContactSectionPosition.SpearForm)
            {
                parametersModel.Data.IsCloseMatch = false;

                // when country hidden, clear the country code in search model
                if (ucContactInfo.IsCountryHidden)
                {
                    searchPeople.countryCode = string.Empty;
                    searchPeople.compactAddress.countryCode = string.Empty;
                }

                PublicUserModel4WS publicUser = PeopleUtil.GetPublicUserFromSession();
                publicUser.peopleModel = new PeopleModel4WS[1] { inputPeople };
            }

            IPeopleBll peopleBll = ObjectFactory.GetObject<IPeopleBll>();
            PeopleModel matchPeople = peopleBll.GetPeopleByClosematch(searchPeople);

            if (matchPeople == null)
            {
                return false;
            }

            if (parametersModel.ContactSectionPosition != ACAConstant.ContactSectionPosition.SpearForm)
            {
                // Store the user input data to session to support the Back function from Account Confirm page.
                parametersModel.Data.DataObject = inputPeople;
                parametersModel.Data.IsCloseMatch = true;

                if (IsForClerk)
                {
                    matchPeople.contactType = inputPeople.contactType;
                    GenericTemplateUtil.MergeGenericTemplate(matchPeople.template, inputPeople.template, string.Empty);
                    matchPeople.attributes = inputPeople.attributes;
                    SetPeopleTemlateSequenceNumber(matchPeople.contactSeqNumber, inputPeople.attributes);
                    matchPeople.template = inputPeople.template;
                }

                // use reference contact.
                PublicUserModel4WS publicUser = PeopleUtil.GetPublicUserFromSession();
                publicUser.peopleModel = new[] { TempModelConvert.ConvertToPeopleModel4WS(matchPeople) };
                PeopleUtil.SavePublicUserToSession(publicUser);
            }
            else
            {
                PeopleModel4WS matchPeople4WS = TempModelConvert.ConvertToPeopleModel4WS(matchPeople);
                matchPeople4WS.contactType = inputPeople.contactType;
                matchPeople4WS.serviceProviderCode = ConfigManager.AgencyCode;

                if (matchPeople4WS.contactAddressList != null)
                {
                    var activeContactAddresses = matchPeople4WS.contactAddressList.Where(p => ACAConstant.VALID_STATUS.Equals(p.auditModel.auditStatus, StringComparison.InvariantCultureIgnoreCase));
                    matchPeople4WS.contactAddressList = activeContactAddresses.ToArray();
                }

                if (matchPeople4WS.contactAddressList != null && matchPeople4WS.contactAddressList.Count() == 1 && StandardChoiceUtil.IsPrimaryContactAddressRequired())
                {
                    matchPeople4WS.contactAddressList[0].primary = ACAConstant.COMMON_Y;
                }

                ContactUtil.MergeContactTemplateModel(matchPeople4WS, inputPeople.contactType, ModuleName);

                if (parametersModel.Data.DataObject is CapContactModel4WS)
                {
                    CapContactModel4WS capContact = parametersModel.Data.DataObject as CapContactModel4WS;
                    capContact.people = matchPeople4WS;
                    capContact.refContactNumber = matchPeople4WS.contactSeqNumber;
                    PeopleUtil.SaveInputPeopleForCloseMatchToSession(inputPeople);
                }
            }

            AppSession.SetContactSessionParameter(parametersModel);
            return true;
        }

        /// <summary>
        /// Sets the people template sequence number.
        /// </summary>
        /// <param name="contactSeqNumber">The contact sequence number.</param>
        /// <param name="attributes">The attributes.</param>
        private void SetPeopleTemlateSequenceNumber(string contactSeqNumber, TemplateAttributeModel[] attributes)
        {
            if (attributes == null || attributes.Length == 0)
            {
                return;
            }

            foreach (var templateAttributeModel in attributes)
            {
                if (templateAttributeModel == null)
                {
                    continue;
                }

                templateAttributeModel.templateObjectNum = contactSeqNumber;
            }
        }

        /// <summary>
        /// Handle the contact type flag change event.
        /// </summary>
        /// <param name="sender">The sender object</param>
        /// <param name="arg">The common event arguments</param>
        private void ContactInfo_ContactTypeFlagChangedEvent(object sender, CommonEventArgs arg)
        {
            CapModel4WS capModel = AppSession.GetCapModelFromSession(ModuleName);
            SwitchContinueBtnDisplay(capModel, Convert.ToString(arg.ArgObject));
            upActionPanel.Update();
        }

        /// <summary>
        /// Display or hide the Continue button.
        /// </summary>
        /// <param name="capModel">The cap model.</param>
        /// <param name="contactTypeFlag">The contact type flag.</param>
        private void SwitchContinueBtnDisplay(CapModel4WS capModel, string contactTypeFlag)
        {
            if (ContactUtil.IsNeedToSelectLCData(capModel, ContactSessionParameterModel, RefContactSeqNbr, contactTypeFlag))
            {
                liSaveBtn.Visible = false;
                liContinueBtn.Visible = true;
                ucContactInfo.IsNeedSelectedLicenseCertification = true;
            }
            else
            {
                liSaveBtn.Visible = true;
                liContinueBtn.Visible = false;
                ucContactInfo.IsNeedSelectedLicenseCertification = false;
            }
        }

        #endregion Methods
    }
}