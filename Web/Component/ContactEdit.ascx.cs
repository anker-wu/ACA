#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: ContactEdit.ascx.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2008-2014
 *
 *  Description:
 *
 *  Notes:
 *      $Id: ContactEdit.ascx.cs 278342 2014-09-02 06:29:40Z ACHIEVO\james.shi $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using System;
using System.Linq;
using System.Web.Script.Serialization;
using System.Web.UI;

using Accela.ACA.Common;
using Accela.ACA.Common.Util;
using Accela.ACA.ExpressionBuild;
using Accela.ACA.Web.Common;
using Accela.ACA.Web.People;
using Accela.ACA.WSProxy;

namespace Accela.ACA.Web.Component
{
    /// <summary>
    /// the class for ContactEdit.
    /// </summary>
    public partial class ContactEdit : BaseUserControl
    {
        #region Fields

        /// <summary>
        /// Indicating whether the contact address is enabled.
        /// </summary>
        private bool _isContactAddressEnabled = StandardChoiceUtil.IsEnableContactAddress() || AppSession.IsAdmin;

        /// <summary>
        /// indicate whether to show contact type field
        /// </summary>
        private bool _isShowContactType;

        /// <summary>
        ///  The contact section position.
        /// </summary>
        private ACAConstant.ContactSectionPosition _contactSectionPosition;

        /// <summary>
        /// is Required
        /// </summary>
        private bool _isRequired = false;

        /// <summary>
        /// Contact Order
        /// </summary>
        private string _contactOrder = string.Empty;

        /// <summary>
        /// R-Reference,need reference data
        /// T-Transactional, user input data.
        /// N-No Limitation,both reference and daily
        /// </summary>
        private string _validateFlag = string.Empty;

        /// <summary>
        /// indicate the contact form is validate or not.
        /// </summary>
        private bool _isValidate;

        /// <summary>
        /// indicate the contact form is used in multiple contact edit or not.
        /// </summary>
        private bool _isMultipleContact;

        /// <summary>
        /// The session parameter string
        /// </summary>
        private string _sessionParameterString = string.Empty;

        /// <summary>
        /// Add contact event by Add from saved¡¢Add new or Look up button
        /// </summary>
        public event CommonEventHandler ContactChanged;

        #endregion Fields

        #region Events

        /// <summary>
        /// An event before the click event of Add New button.
        /// </summary>
        public event CommonEventHandler PreAddNewClick;

        #endregion Events

        #region Properties

        /// <summary>
        /// Gets or sets the type of the component.
        /// </summary>
        /// <value>
        /// The type of the component.
        /// </value>
        public int ComponentID
        {
            get;
            set;
        }

        /// <summary>
        /// Sets a value indicating whether the 'Add New' button is visible.
        /// </summary>
        public bool EnableAddNew
        {
            set
            {
                btnAddNew.Visible = value;
            }
        }

        /// <summary>
        /// Sets a value indicating whether the 'Select Contact from Account' button is visible.
        /// </summary>
        public bool EnableSelectFromAccount
        {
            set
            {
                btnAddFromSaved.Visible = value;
            }
        }

        /// <summary>
        /// Sets a value indicating whether the 'Look Up' button is visible.
        /// </summary>
        public bool EnableLookUp
        {
            set
            {
                btnLookUp.Visible = value;
            }
        }

        /// <summary>
        /// Gets or sets the current contact expression type.
        /// </summary>
        public ExpressionType ContactExpressionType
        {
            get; 
            set;
        }

        /// <summary>
        /// Gets or sets the contact type name.
        /// </summary>
        public string ContactType
        {
            get
            {
                return hfContatType.Value;
            }

            set
            {
                hfContatType.Value = value;
                ucContactAddressList.ContactType = ContactType;
            }
        }

        /// <summary>
        /// Gets or sets the ref contact number.
        /// </summary>
        public string RefContactNumber
        {
            get
            {
                return hdnRefContactSeqNumber.Value;
            }

            set
            {
                hdnRefContactSeqNumber.Value = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether to show the Contact Detail form after Add New.
        /// </summary>
        public bool IsShowDetail
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets a value indicating whether to show the contact type field in the contact view.
        /// </summary>
        public bool IsShowContactType
        {
            get
            {
                return _isShowContactType;
            }

            set
            {
                _isShowContactType = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether need select contact type before Add New
        /// </summary>
        public bool NeedSelectType
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets a value indicating whether to Focus Add Contact button.
        /// </summary>
        public bool IsNeedFocusElement
        {
            get;
            set;
        }

        /// <summary>
        ///  Gets or sets the contact section position.
        /// </summary>
        public ACAConstant.ContactSectionPosition ContactSectionPosition
        {
            get
            {
                return _contactSectionPosition;
            }

            set
            {
                _contactSectionPosition = value;

                if (_isContactAddressEnabled || AppSession.IsAdmin)
                {
                    ucContactAddressList.ContactSectionPosition = value;
                }

                // if register user and in cap edit page, the "Select from Account" should be showed.
                if (!AppSession.IsAdmin)
                {
                    switch (ContactSectionPosition)
                    {
                        case ACAConstant.ContactSectionPosition.SpearForm:
                            // if anonymous, hide it.
                            EnableSelectFromAccount = !AppSession.User.IsAnonymous;
                            break;
                        case ACAConstant.ContactSectionPosition.RegisterAccount:
                            EnableSelectFromAccount = StandardChoiceUtil.IsRequiredLicense();
                            break;
                        case ACAConstant.ContactSectionPosition.RegisterExistingAccount:
                            EnableSelectFromAccount = true;
                            break;
                        default:
                            EnableSelectFromAccount = false;
                            break;
                    }
                }

                if (ContactSectionPosition == ACAConstant.ContactSectionPosition.RegisterAccount
                    || ContactSectionPosition == ACAConstant.ContactSectionPosition.RegisterExistingAccount)
                {
                    btnAddFromSaved.LabelKey = "aca_contactedit_label_selectfrom";
                }

                btnRemove.Visible = ContactSectionPosition != ACAConstant.ContactSectionPosition.EditClerk;
            }
        }

        /// <summary>
        /// Gets or sets focus click id.
        /// </summary>
        public string SkippingToParentClickID
        {
            get
            {
                return ViewState["SkippingToParentClickID"] as string;
            }

            set
            {
                ViewState["SkippingToParentClickID"] = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the smart choice validate flag for license
        /// </summary>
        public bool IsValidate
        {
            get
            {
                return _isValidate;
            }

            set
            {
                _isValidate = value;

                if (_isContactAddressEnabled)
                {
                    ucContactAddressList.IsValidate = value;
                }
            }
        }

        /// <summary>
        /// Gets or sets data comes from
        /// </summary>
        public string ValidateFlag
        {
            get
            {
                return _validateFlag;
            }

            set
            {
                _validateFlag = value;

                if (ComponentDataSource.Reference.Equals(value))
                {
                    IsValidate = true;
                }
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether edit the form.
        /// </summary>
        public bool IsEditable
        {
            get
            {
                object result = ViewState["IsEditable"];
                return result == null || bool.Parse(result.ToString());
            }

            set
            {
                ViewState["IsEditable"] = value;

                if (_isContactAddressEnabled)
                {
                    ucContactAddressList.IsEditable = value;
                }
            }
        }

        /// <summary>
        /// Gets or sets the name of the component.
        /// </summary>
        /// <value>
        /// The name of the component.
        /// </value>
        public string ComponentName
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets a value indicating whether the Multiple contact be used.
        /// </summary>
        public bool IsMultipleContact
        {
            get
            {
                return _isMultipleContact;
            }

            set
            {
                _isMultipleContact = value;
                NeedSelectType = value;
            }
        }

        /// <summary>
        /// Gets the function create contact session to open the edit form.
        /// </summary>
        public string CreateContactSessionFunction
        {
            get
            {
                return ClientID + "_CreateContactSession";
            }
        }

        /// <summary>
        /// Gets edit contact function name
        /// </summary>
        public string EditContactFunction
        {
            get
            {
                return ClientID + "_EditContact";
            }
        }

        /// <summary>
        /// Gets the client script name of Add New click event.
        /// </summary>
        public string AddNewClientFunction
        {
            get
            {
                return ClientID + "_AddNewContact";
            }
        }

        /// <summary>
        /// Gets the select from account button client id.
        /// </summary>
        public string SelectFromAccountClientID
        {
            get
            {
                return btnAddFromSaved.ClientID;
            }
        }

        /// <summary>
        /// Gets the look up button client id.
        /// </summary>
        public string LookUpClientID
        {
            get
            {
                return btnLookUp.ClientID;
            }
        }

        /// <summary>
        /// Gets the edit button client id.
        /// </summary>
        public string EditClientID
        {
            get
            {
                return btnEdit.ClientID;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether current user is from another agency or not.
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
        /// Gets or sets a value indicating whether this is an registration action for an existing account or not.
        /// </summary>
        public bool IsNewUserRegisterExistingAccount
        {
            get
            {
                return Convert.ToBoolean(ViewState["IsRegisterExistingAccount"]);
            }

            set
            {
                ViewState["IsRegisterExistingAccount"] = value;
            }
        }

        /// <summary>
        /// Gets or sets UserId in another agency
        /// </summary>
        public string ExistingAccountRegisterationUserID
        {
            get
            {
                if (!string.IsNullOrEmpty(Request.QueryString[UrlConstant.USER_ID_OR_EMAIL]))
                {
                    ViewState["ExistingAccountRegisterationUserID"] = Request.QueryString[UrlConstant.USER_ID_OR_EMAIL];
                    return Request.QueryString[UrlConstant.USER_ID_OR_EMAIL];
                }

                if (ViewState["ExistingAccountRegisterationUserID"] != null)
                {
                    return ViewState["ExistingAccountRegisterationUserID"].ToString();
                }

                return string.Empty;
            }

            set
            {
                ViewState["ExistingAccountRegisterationUserID"] = value;
            }
        }

        /// <summary>
        /// Gets the "Select from..." button client id.
        /// </summary>
        public string AddFromSavedClientID
        {
            get
            {
                return btnAddFromSaved.ClientID;
            }
        }

        /// <summary>
        /// Gets the session parameter string.
        /// </summary>
        /// <value>
        /// The session parameter string.
        /// </value>
        protected string SessionParameterString
        {
            get
            {
                if (string.IsNullOrEmpty(_sessionParameterString))
                {
                    ContactSessionParameter parameter = new ContactSessionParameter();
                    JavaScriptSerializer javaScriptSerializer = new JavaScriptSerializer();

                    parameter.PageFlowComponent.ComponentName = ComponentName;
                    parameter.PageFlowComponent.ComponentID = EnumUtil<PageFlowComponent>.Parse(ComponentID.ToString());
                    parameter.ContactExpressionType = ContactExpressionType;
                    parameter.Process.CallbackFunctionName = ClientID;                    
                    parameter.PageFlowComponent.ComponentDataSource = ValidateFlag;
                    parameter.ShowDetail = IsShowDetail;
                    parameter.ContactType = ContactType;
                    parameter.ContactSectionPosition = ContactSectionPosition;
                    parameter.PageFlowComponent.IsEditable = IsEditable;

                    _sessionParameterString = javaScriptSerializer.Serialize(parameter);
                }
                
                return _sessionParameterString;
            }
        }

        /// <summary>
        /// Gets current SubAgencyCap type
        /// </summary>
        protected string IsSubAgencyCap
        {
            get { return Request.QueryString[ACAConstant.IS_SUBAGENCY_CAP]; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether country initialize or not.
        /// </summary>
        protected bool IsInitCountry
        {
            get;
            set;
        }

        #endregion Properties

        #region Public Methods

        /// <summary>
        /// True must fill all required field, False skip validate required field
        /// </summary>
        /// <param name="contactOrder">contact order</param>
        /// <param name="required">Is required</param>
        public void SetSectionRequired(string contactOrder, bool required)
        {
            _contactOrder = contactOrder;
            _isRequired = required;
        }

        /// <summary>
        /// Update all the controls in this control's UpdatePanel
        /// </summary>
        public void Update()
        {
            ContactEditPanel.Update();
        }

        /// <summary>
        /// Displays people/contact information to UI.
        /// </summary>
        /// <param name="capContact">CapContactModel4WS object</param>
        /// <param name="contactType">Contact type, one value of standard choice items in "Contact Type" 
        /// and which is configured in smart choice group as default value of section.</param>
        /// <param name="isInConfirmPage">is In confirm page</param>
        public void DisplayView(CapContactModel4WS capContact, string contactType, bool isInConfirmPage)
        {
            if (capContact == null)
            {
                RefContactNumber = string.Empty;
                DisplayView(null, string.Empty);
            }
            else
            {
                RefContactNumber = capContact.refContactNumber;
                DisplayView(capContact.people, capContact.accessLevel);
            }
        }

        /// <summary>
        /// Display the contact in the view control
        /// </summary>
        /// <param name="people">A cap contact model.</param>
        public void DisplayView(PeopleModel4WS people)
        {
            DisplayView(people, string.Empty);
        }

        /// <summary>
        /// Set ContactAddress List Is Editable
        /// </summary>
        /// <param name="isEditable">is editable</param>
        public void SetContactAddressListIsEditable(bool isEditable)
        {
            if (_isContactAddressEnabled)
            {
                ucContactAddressList.IsEditable = isEditable;
            }
        }

        /// <summary>
        /// Change the confirm message for removing contact.
        /// </summary>
        public void ChangeMsg4RemoveContact()
        {
            string removeContactMsg = GetTextByKey("aca_capedit_contact_msg_removeprimarycontact").Replace("'", "\\'");
            btnRemove.OnClientClick = string.Format("return {0}_RemoveContact('{1}');", ClientID, removeContactMsg);
        }

        /// <summary>
        /// Refresh the contact section.
        /// </summary>
        /// <param name="isFromContactEdit">Is from contact edit page.</param>
        public void RefreshContact(bool isFromContactEdit = true)
        {
            if (!isFromContactEdit)
            {
                hdnIsClearRefContact.Value = ACAConstant.COMMON_N;
            }

            RefreshContactChange(null);
        }

        /// <summary>
        /// Validates the contact address.
        /// </summary>
        /// <param name="people">The people.</param>
        /// <returns>Contact address list validation result</returns>
        public string ValidateContactAddress(PeopleModel4WS people)
        {
            if (people != null)
            {
                return ucContactAddressList.Validate(people.contactAddressList, true, true);
            }
            
            return string.Empty;
        }

        /// <summary>
        /// Show action notice message.
        /// </summary>
        /// <param name="actionType">
        /// The action type.
        /// </param>
        public void ShowActionNoticeMessage(ActionType actionType)
        {
            if (!IsMultipleContact && hfIsEditContactAddress.Value == "1")
            {
                // Show message for contact address
                ucContactAddressList.ShowActionNoticeMessage(actionType);
                Page.FocusElement(string.Format("{0}_lnkAddContactAddress", ClientID));
            }
            else
            {
                // Show message for contact
                string labelKey = null;
                divActionNotice.Visible = true;

                switch (actionType)
                {
                    case ActionType.AddSuccess:
                        divImgSuccess.Visible = true;
                        lblActionNoticeAddSuccess.Visible = true;
                        labelKey = lblActionNoticeAddSuccess.LabelKey;
                        break;
                    case ActionType.UpdateSuccess:
                        divImgSuccess.Visible = true;
                        lblActionNoticeEditSuccess.Visible = true;
                        labelKey = lblActionNoticeEditSuccess.LabelKey;
                        break;
                    case ActionType.DeleteSuccess:
                        divImgSuccess.Visible = true;
                        lblActionNoticeDeleteSuccess.Visible = true;
                        labelKey = lblActionNoticeDeleteSuccess.LabelKey;
                        break;
                    case ActionType.AddFailed:
                        divImgFailed.Visible = true;
                        lblActionNoticeAddFailed.Visible = true;
                        labelKey = lblActionNoticeAddFailed.LabelKey;
                        break;
                    case ActionType.UpdateFailed:
                        divImgFailed.Visible = true;
                        lblActionNoticeEditFailed.Visible = true;
                        labelKey = lblActionNoticeEditFailed.LabelKey;
                        break;
                    case ActionType.DeleteFailed:
                        divImgFailed.Visible = true;
                        lblActionNoticeDeleteFailed.Visible = true;
                        labelKey = lblActionNoticeDeleteFailed.LabelKey;
                        break;
                }

                if (AccessibilityUtil.AccessibilityEnabled)
                {
                    MessageUtil.ShowAlertMessage(this, GetTextByKey(labelKey));
                }
            }
        }

        /// <summary>
        /// Hide action notice message.
        /// </summary>
        public void HideActionNoticeMessage()
        {
            if (IsPostBackOnCurrentControl())
            {
                divActionNotice.Visible = false;
                divImgFailed.Visible = false;
                divImgSuccess.Visible = false;
                lblActionNoticeAddSuccess.Visible = false;
                lblActionNoticeEditSuccess.Visible = false;
                lblActionNoticeDeleteSuccess.Visible = false;
                lblActionNoticeAddFailed.Visible = false;
                lblActionNoticeEditFailed.Visible = false;
                lblActionNoticeDeleteFailed.Visible = false;
                ContactEditPanel.Update();
            }
        }

        /// <summary>
        /// Shows the validate error message.
        /// </summary>
        /// <param name="message">The message.</param>
        public void ShowValidateErrorMessage(string message)
        {
            if (!string.IsNullOrEmpty(message))
            {
                MessageUtil.ShowMessageByControl(errorMessageLabel, MessageType.Error, message);
            }
        }

        /// <summary>
        /// Shows the contact address validate error message.
        /// </summary>
        /// <param name="message">The message.</param>
        public void ShowContactAddressValidateErrorMessage(string message)
        {
            if (!string.IsNullOrEmpty(message))
            {
                ucContactAddressList.ShowValidateErrorMessage(message);
            }
        }

        #endregion Public Methods

        #region Protected Methods

        /// <summary>
        /// Raises the initial event
        /// </summary>
        /// <param name="e">A System.EventArgs object containing the event data.</param>
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            ucContactAddressList.ParentClientID = ClientID;
            btnRemove.OnClientClick = string.Format("return {0}_RemoveContact();", ClientID);
        }

        /// <summary>
        /// Raises the page load event
        /// </summary>
        /// <param name="sender">An object that contains the event sender.</param>
        /// <param name="e">A System.EventArgs object containing the event data.</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!AppSession.IsAdmin)
            {
                hlEnd.NextControlClientID = SkippingToParentClickID;
                HideActionNoticeMessage();
            }

            if (!IsPostBack)
            {
                /*
                 * Hide Add New button: 
                 * 1. This form isn't editabled.
                 * 2. When set component property data source 'Reference'.
                 * 3. On Clerk edit page.
                 */
                if (!IsEditable
                    || ComponentDataSource.Reference.Equals(ValidateFlag)
                    || ContactSectionPosition == ACAConstant.ContactSectionPosition.EditClerk)
                {
                    EnableAddNew = false;
                }

                /*
                 * Hide Look Up button: 
                 * 1. This form isn't editabled.
                 * 2. Not enabled search reference contact.
                 * 3. When set component property data source 'Transactional'.
                 */
                if (!IsEditable
                    || !StandardChoiceUtil.IsEnableRefContactSearch()
                    || ComponentDataSource.Transactional.Equals(ValidateFlag))
                {
                    EnableLookUp = false;
                }

                if (!IsEditable)
                {
                    EnableSelectFromAccount = false;
                    btnRemove.Visible = false;
                }

                if (AppSession.IsAdmin)
                {
                    DisplayView(null);
                }
            }
            
            ucContactAddressList.NeedCreateSession = true;
            ucContactAddressList.SessionParameterString = SessionParameterString;
            ucContactAddressList.IsRefContact = !string.IsNullOrEmpty(RefContactNumber);

            if (PreAddNewClick != null)
            {
                btnAddNew.CausesValidation = true;
                btnAddNew.OnClientClick = string.Empty;
                btnAddFromSaved.CausesValidation = true;
                btnAddFromSaved.OnClientClick = string.Empty;
            }
            else
            {
                btnAddNew.OnClientClick = string.Format("{0}_CreateContactSession({1}, '','',{2}); return false;", ClientID, ContactProcessType.Add.ToString("D"), AddNewClientFunction);

                if (IsLoginUseExistingAccount || IsNewUserRegisterExistingAccount)
                {
                    btnAddFromSaved.OnClientClick = string.Format("{0}_CreateContactSession({1}, '', '', {0}_AddFromOtherAgencies); return false;", ClientID, ContactProcessType.SelectContactFromOtherAgencies.ToString("D"));
                }
                else
                {
                    btnAddFromSaved.OnClientClick = string.Format("{0}_CreateContactSession({1}, '','',{0}_AddFromSavedContact); return false;", ClientID, ContactProcessType.SelectContactFromAccount.ToString("D"));
                }

                btnLookUp.OnClientClick =
                    string.Format(
                        "{0}_CreateContactSession({1}, '','',{0}_LookUpContact); return false;",
                        ClientID,
                        ContactProcessType.Lookup.ToString("D"));

                btnEdit.OnClientClick =
                    string.Format(
                        "SetLastFocus('{3}');{0}({1}, '','',{2}); return false;",
                        CreateContactSessionFunction,
                        ContactProcessType.Edit.ToString("D"),
                        EditContactFunction,
                        btnEdit.ClientID);
            }
        }

        /// <summary>
        /// override PreRender event.
        /// </summary>
        /// <param name="e">Event argument object</param>
        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);

            // todo, is need pass Register's contact
            if (!IsMultipleContact)
            {
                string errorMessage = IsReferenceDataValidation();

                if (!string.IsNullOrEmpty(errorMessage))
                {
                    ShowValidateErrorMessage(errorMessage);

                    //if the Contact validate failed then should return to prevent the required validate message overwrite.
                    return;
                }

                SetFieldRequiredValidation();
                SetSectionRequiredValidation();

                CapModel4WS capModel = AppSession.GetCapModelFromSession(ModuleName);
                CapContactModel4WS capContactModel = capModel != null
                    ? ContactUtil.FindContactWithComponentName(capModel.contactsGroup, ComponentName)
                    : null;

                if (capContactModel != null)
                {
                    ValidateContactAddress(capContactModel.people);
                }
            }
        }

        /// <summary>
        /// Click Select Contact from Account button event handler.
        /// </summary>
        /// <param name="sender">An object that contains the event sender.</param>
        /// <param name="e">An System.EventArgs object that contains the event data.</param>
        protected void AddFromSavedButton_Click(object sender, EventArgs e)
        {
            if (PreAddNewClick != null)
            {
                string script = string.Format("{0}_CreateContactSession({1}, '','',{0}_AddFromSavedContact);", ClientID, ContactProcessType.SelectContactFromAccount.ToString("D"));

                RaisePreAddNewClick(script);
            }
        }

        /// <summary>
        /// Click Add New button event handler.
        /// </summary>
        /// <param name="sender">An object that contains the event sender.</param>
        /// <param name="e">An System.EventArgs object that contains the event data.</param>
        protected void AddNewButton_Click(object sender, EventArgs e)
        {
            if (PreAddNewClick != null)
            {
                string script = string.Format("{0}_CreateContactSession({1}, '','',{2});", ClientID, ContactProcessType.Add.ToString("D"), AddNewClientFunction);

                RaisePreAddNewClick(script);
            }
        }

        /// <summary>
        /// Remove the contact.
        /// </summary>
        /// <param name="sender">An object that contains the event sender.</param>
        /// <param name="e">An System.EventArgs object that contains the event data.</param>
        protected void RemoveButton_Click(object sender, EventArgs e)
        {
            if (ContactSectionPosition == ACAConstant.ContactSectionPosition.SpearForm)
            {
                CapModel4WS capModel = AppSession.GetCapModelFromSession(ModuleName);
                CapContactModel4WS removingCapContact = ObjectCloneUtil.DeepCopy(capModel.contactsGroup.First(f => f.componentName == ComponentName));
                RaiseContactChangeEvent(sender, removingCapContact, true);
            }

            AppSession.SetContactSessionParameter(null);
            AppSession.SetRegisterContactSessionParameter(null);

            divSelectContactSection.Visible = true;
            sepForSelectContact.Visible = true;

            // Remove the ViewState["ContactType"] value for contactEdit_ucContactAddressList.
            ContactType = null; 

            ucContactView.Visible = false;
            sepLineForContactView.Visible = false;
            divContactList.Attributes["class"] = "ACA_Hide";
            ucConditon.HideCondition();
            divConditon.Visible = false;

            ShowActionNoticeMessage(ActionType.DeleteSuccess);
        }

        /// <summary>
        /// Rebind the contact address list.
        /// </summary>
        /// <param name="sender">An object that contains the event sender.</param>
        /// <param name="e">An System.EventArgs object that contains the event data.</param>
        protected void RefreshContactButton_Click(object sender, EventArgs e)
        {
            txtValidateSectionRequired.ValidationByHiddenTextBox = false;
            RefreshContactChange(sender);

            Page.FocusElement(Request.Form["__LASTFOCUS_ID"]);
        }

        /// <summary>
        /// Handle the ContactAddressSelected event, populate contact information into contact address edit form.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="arg">Event argument.</param>
        protected void ContactAddressList_ContactAddressSelected(object sender, CommonEventArgs arg)
        {
            //PeopleUtil.SaveTempContactAddresses(contactAddressList.DataSource == null ? null : contactAddressList.DataSource.ToArray());
        }

        /// <summary>
        /// Handle the ContactAddressSelected event, populate contact information into contact address edit form.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="arg">Event argument.</param>
        protected void ContactAddressList_ContactAddressDeactivate(object sender, CommonEventArgs arg)
        {
            //PeopleUtil.SaveTempContactAddresses(contactAddressList.DataSource == null ? null : contactAddressList.DataSource.ToArray());
        }

        /// <summary>
        /// Handle the DataSourceChanged event, to control the contact edit form.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="arg">Event argument.</param>
        protected void ContactAddressList_DataSourceChanged(object sender, CommonEventArgs arg)
        {
            ContactAddressModel[] addressList = ucContactAddressList.DataSource == null
                                                ? null
                                                : ucContactAddressList.DataSource.ToArray();
            ContactUtil.SetContactAddressListToContactSessionParameter(addressList);

            if (ACAConstant.ContactSectionPosition.RegisterAccount.Equals(ContactSectionPosition)
                || ACAConstant.ContactSectionPosition.RegisterExistingAccount.Equals(ContactSectionPosition)
                || ACAConstant.ContactSectionPosition.RegisterClerk.Equals(ContactSectionPosition)
                || ACAConstant.ContactSectionPosition.RegisterAccountConfirm.Equals(ContactSectionPosition)
                || ACAConstant.ContactSectionPosition.RegisterClerkConfirm.Equals(ContactSectionPosition)
                || ACAConstant.ContactSectionPosition.EditClerk.Equals(ContactSectionPosition))
            {
                ContactSessionParameter contactSessionParameterRegister = AppSession.GetRegisterContactSessionParameter();
                PeopleModel4WS peopleRegister = ContactUtil.GetPeopleModelFromContactSessionParameter(contactSessionParameterRegister);
                peopleRegister.contactAddressList = ObjectCloneUtil.DeepCopy(addressList);
            }

            CapModel4WS capModel = AppSession.GetCapModelFromSession(ModuleName);

            if (capModel == null)
            {
                return;
            }

            CapContactModel4WS capContactModel = ContactUtil.FindContactWithComponentName(capModel.contactsGroup, ComponentName);

            if (capContactModel != null)
            {
                capContactModel.people.contactAddressList = addressList;
            }

            RaiseContactChangeEvent(sender, capContactModel);
        }

        #endregion Protected Methods

        #region Private Methods

        /// <summary>
        /// Display the contact address list.
        /// </summary>
        /// <param name="addressList">Contact address list</param>
        private void DisplayContactAddressList(ContactAddressModel[] addressList)
        {
            if (!ucContactAddressList.Visible)
            {
                ucContactAddressList.Visible = true;
            }

            ucContactAddressList.Display(addressList);
        }

        /// <summary>
        /// if conditionModel is not null, show locked, hold or notice message.
        /// </summary>
        /// <param name="people">A <see cref="PeopleModel4WS"/></param>
        /// <returns>true or false.</returns>
        private bool ShowCondition(PeopleModel4WS people)
        {
            bool valid = ConditionsUtil.ShowCondition(ucConditon, people);

            divConditon.Visible = ucConditon.ConditionResult != ConditionResult.None;

            return valid;
        }

        /// <summary>
        /// Raise ContactChanged event when add or remove contact.
        /// </summary>
        /// <param name="sender">An object that contains the event sender.</param>
        /// <param name="capContact">The cap contact.</param>
        /// <param name="isDeleteAction">Whether is delete action.</param>
        private void RaiseContactChangeEvent(object sender, CapContactModel4WS capContact, bool isDeleteAction = false)
        {
            if (ContactChanged != null)
            {
                ContactChanged(sender, new CommonEventArgs(new object[] { isDeleteAction, capContact, hdnIsClearRefContact.Value }));
            }
        }

        /// <summary>
        /// Refreshes the contact change.
        /// </summary>
        /// <param name="sender">The sender.</param>
        private void RefreshContactChange(object sender)
        {
            CapContactModel4WS capContact = null;
            ContactSessionParameter parametersModel = AppSession.GetContactSessionParameter();
            PeopleModel4WS people = ContactUtil.GetPeopleModelFromContactSessionParameter(parametersModel);

            if (parametersModel.Data.DataObject is CapContactModel4WS)
            {
                capContact = parametersModel.Data.DataObject as CapContactModel4WS;
            }

            if (parametersModel.Data.IsCloseMatch)
            {
                ucContactAddressList.IsEditable = false;
            }

            // Load contact view
            switch (ContactSectionPosition)
            {
                case ACAConstant.ContactSectionPosition.SpearForm:
                    DisplayView(people, capContact == null ? string.Empty : capContact.accessLevel);
                    break;
                case ACAConstant.ContactSectionPosition.EditClerk:
                case ACAConstant.ContactSectionPosition.RegisterClerk:
                case ACAConstant.ContactSectionPosition.RegisterAccount:
                case ACAConstant.ContactSectionPosition.RegisterExistingAccount:
                    DisplayView(people);
                    break;
            }

            RaiseContactChangeEvent(sender, capContact);

            // Show message
            ActionType actionType = (parametersModel.Process.ContactProcessType != ContactProcessType.Edit) ? ActionType.AddSuccess : ActionType.UpdateSuccess;
            ShowActionNoticeMessage(actionType);
        }

        /// <summary>
        /// Validate section required when section is set required and no data input
        /// </summary>
        private void SetSectionRequiredValidation()
        {
            if (IsEditable && _isRequired && !ucContactView.Visible)
            {
                txtValidateSectionRequired.ValidationByHiddenTextBox = true;
                txtValidateSectionRequired.CheckControlValueValidateFunction = ClientID + "_CheckRequired4Contact";   
            }
        }

        /// <summary>
        /// Raise PreAddNewClick event.
        /// </summary>
        /// <param name="script">The script.</param>
        private void RaisePreAddNewClick(string script)
        {
            CommonEventArgs args = new CommonEventArgs(false);

            // the args maybe changed in this event.
            PreAddNewClick(this, args);

            // No cancel the click
            if (!bool.Parse(args.ArgObject.ToString()))
            {
                ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "AddNewContact", script, true);
            }
        }

        /// <summary>
        /// Set section fields required validation.
        /// Has some required fields not input and some field format not correct.
        /// </summary>
        private void SetFieldRequiredValidation()
        {
            CapModel4WS capModel = AppSession.GetCapModelFromSession(ModuleName);

            if (capModel != null && capModel.contactsGroup != null)
            {
                CapContactModel4WS contact = ContactUtil.FindContactWithComponentName(capModel.contactsGroup, ComponentName);

                if (contact == null)
                {
                    return;
                }

                bool isRefContact = IsValidate && !string.IsNullOrEmpty(contact.refContactNumber);

                if (!ContactUtil.ValidateRequiredField4SingleContact(ModuleName, contact, IsEditable, isRefContact, ValidateFlag, ContactSectionPosition))
                {
                    MessageUtil.ShowMessageByControl(errorMessageLabel, MessageType.Error, GetTextByKey("per_contactlist_required_validate_msg"));
                    txtValidateSectionRequired.ValidationByHiddenTextBox = true;
                    txtValidateSectionRequired.CheckControlValueValidateFunction = ClientID + "_ValidateFieldRequired4Contact";
                }
            }
        } 

        /// <summary>
        /// Determines whether is reference data validation.
        /// </summary>
        /// <returns>True is validate else not validate</returns>
        private string IsReferenceDataValidation()
        {
            CapModel4WS capModel = AppSession.GetCapModelFromSession(ModuleName);

            if (!IsValidate || capModel == null)
            {
                return string.Empty;
            }

            CapContactModel4WS contact = ContactUtil.FindContactWithComponentName(capModel.contactsGroup, ComponentName);

            if (contact == null)
            {
                return string.Empty;
            }

            return ContactUtil.IsValidContact(new[] { contact }, ModuleName);
        }

        /// <summary>
        /// Display the contact in the view control
        /// </summary>
        /// <param name="people">A cap contact model.</param>
        /// <param name="accessLevel">the contact access level</param>
        private void DisplayView(PeopleModel4WS people, string accessLevel)
        {
            if (AppSession.IsAdmin)
            {
                if (ACAConstant.ContactSectionPosition.SpearForm.Equals(ContactSectionPosition)
                    && IsMultipleContact)
                {
                    divConditon.Visible = true;
                    ucConditon.Visible = true;
                    ucConditon.HideCondition();
                }
            }
            else
            {
                ucConditon.HideCondition();

                if (people == null || (ACAConstant.ContactSectionPosition.SpearForm.Equals(ContactSectionPosition) && !ShowCondition(people)))
                {
                    return;
                }
            }

            if (people != null)
            {
                hdnContactSeqNumber.Value = people.contactSeqNumber;
                RefContactNumber = people.contactSeqNumber;
                ucContactAddressList.IsRefContact = !string.IsNullOrEmpty(RefContactNumber);
            }

            // Operate button
            if (divSelectContactSection.Visible && !AppSession.IsAdmin)
            {
                divSelectContactSection.Visible = false;
                sepForSelectContact.Visible = false;
            }

            // Operate result: contact list/contact view/contact address list
            divContactList.Attributes["class"] = "ACA_Show";

            // Load contact view
            ucContactView.Visible = true;
            ucContactView.ContactSectionPosition = ContactSectionPosition;
            ucContactView.Display(people, accessLevel);

            ucContactAddressList.Visible = _isContactAddressEnabled;

            CapModel4WS capModel = AppSession.GetCapModelFromSession(ModuleName);

            if (people != null
                && !string.IsNullOrEmpty(RefContactNumber)
                && ValidationUtil.IsYes(people.flag)
                && ContactUtil.IsPrimaryContactHasLCData(capModel, RefContactNumber))
            {
                ChangeMsg4RemoveContact();
            }
            else
            {
                // If the contact information is not contains LC data then should change the remove button OnClientClick event to default.
                btnRemove.OnClientClick = string.Format("return {0}_RemoveContact();", ClientID);
            }

            // Load contact address list
            if (_isContactAddressEnabled)
            {
                DisplayContactAddressList(people == null ? null : people.contactAddressList);
            }
        }

        #endregion Private Methods
    }
}