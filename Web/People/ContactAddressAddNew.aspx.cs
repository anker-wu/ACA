#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: ContactAddressAddNew.aspx.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2013-2014
 *
 *  Description:
 *
 *  Notes:
 * $Id: ContactAddressAddNew.aspx.cs 151830 2013-10-09 13:39:43Z ACHIEVO\blues.gao $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web.UI;
using Accela.ACA.BLL.APO;
using Accela.ACA.Common;
using Accela.ACA.Common.Util;
using Accela.ACA.Web.Common;
using Accela.ACA.Web.ExpressionBuild;
using Accela.ACA.WSProxy;

namespace Accela.ACA.Web.People
{
    /// <summary>
    /// the class of Contact address edit
    /// </summary>
    public partial class ContactAddressAddNew : PeoplePopupBasePage
    {
        #region Fields

        /// <summary>
        /// Indicating whether the contact address is enabled.
        /// </summary>
        private bool _isContactAddressEnabled;

        #endregion Fields

        #region Properties

        /// <summary>
        /// Gets the reference contact sequence number
        /// </summary>
        public string RefContactSeqNbr
        {
            get
            {
                CapContactModel4WS capContactModel = ContactSessionParameterModel.Data.DataObject as CapContactModel4WS;

                if (ContactSessionParameterModel.ContactSectionPosition == ACAConstant.ContactSectionPosition.SpearForm
                    && ContactSessionParameterModel.Process.ContactProcessType != ContactProcessType.Add
                    && capContactModel != null
                    && capContactModel.people != null)
                {
                    PeopleModel4WS people = capContactModel.people;

                    if (ContactSessionParameterModel.Process.ContactProcessType == ContactProcessType.Lookup
                        || ContactSessionParameterModel.Process.ContactProcessType == ContactProcessType.SelectContactFromCloseMatch
                        || ContactSessionParameterModel.Process.ContactProcessType == ContactProcessType.SelectContactFromAccount)
                    {
                        capContactModel.refContactNumber = people.contactSeqNumber;
                        return people.contactSeqNumber;
                    }

                    if (ContactSessionParameterModel.Process.ContactProcessType == ContactProcessType.Edit
                        || ContactSessionParameterModel.Process.ContactProcessType == ContactProcessType.None)
                    {
                        return capContactModel.refContactNumber;
                    }
                }

                return string.Empty;
            }
        }

        /// <summary>
        /// Gets the callback function name.
        /// </summary>
        protected string CallbackFunctionName
        {
            get
            {
                bool isAdd = ContactSessionParameterModel.Process.ContactAddressProcessType == ContactAddressProcessType.Add;
                
                return string.Format(
                                    "{0}(true,{1},\"{{0}}\");",
                                    ContactSessionParameterModel.Process.CACallbackFunctionName,
                                    isAdd.ToString().ToLower());
            }
        }

        /// <summary>
        /// Gets the Generic view ID.
        /// </summary>
        private string ViewID
        {
            get
            {
                return PeopleUtil.GetContactAddressEditGviewID(ContactSectionPosition);
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the need external contact address.
        /// </summary>
        private bool IsNeedValidateContactAddress
        {
            get; 
            set;
        }

        #endregion Properties

        #region Methods

        /// <summary>
        /// The On Initial event.
        /// </summary>
        /// <param name="e">the event handle.</param>
        protected override void OnInit(EventArgs e)
        {
            _isContactAddressEnabled = StandardChoiceUtil.IsEnableContactAddress() || AppSession.IsAdmin;
            ucContactAddressEdit.ContactSectionPosition = ContactSectionPosition;
            ucContactAddressEdit.ContactExpressionType = ContactExpressionType;

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
            DialogUtil.RegisterScriptForDialog(Page);

            // Initialize the page's title
            InitTitle();

            ucContactAddressEdit.IsEnabled = _isContactAddressEnabled;
            ucContactAddressEdit.IsEditable = ContactSessionParameterModel.PageFlowComponent.IsEditable && _isContactAddressEnabled;

            if (!IsPostBack && !AppSession.IsAdmin && _isContactAddressEnabled)
            {
                ucContactAddressEdit.BindAddressType(ContactType);

                ContactAddressModel addressModel = null;

                if (ContactSessionParameterModel != null && ContactSessionParameterModel.Data.ContactAddressRowIndex != null)
                {
                    liClearBtn.Visible = false;
                    PeopleModel4WS people;

                    if (ContactSessionParameterModel.ContactSectionPosition == ACAConstant.ContactSectionPosition.RegisterAccountConfirm
                        && ContactSessionParameterModel.Data.IsCloseMatch)
                    {
                        PublicUserModel4WS model = PeopleUtil.GetPublicUserFromSession();
                        people = model.peopleModel[0];
                    }
                    else
                    {
                        people = ContactUtil.GetPeopleModelFromContactSessionParameter();
                    }

                    if (people != null)
                    {
                        addressModel = people.contactAddressList.FirstOrDefault(f => f.RowIndex == ContactSessionParameterModel.Data.ContactAddressRowIndex.Value);
                    }
                }

                // Load the contact address information
                bool isDisable = ucContactAddressEdit.LoadSelectedContactAddressInfo(addressModel);

                if ((ContactSessionParameterModel != null 
                        && ComponentDataSource.Reference.Equals(ContactSessionParameterModel.PageFlowComponent.ComponentDataSource)
                        && !string.IsNullOrEmpty(RefContactSeqNbr))
                    || isDisable 
                    || ContactSessionParameterModel.Data.IsCloseMatch
                    || ContactSessionParameterModel.ContactSectionPosition == ACAConstant.ContactSectionPosition.SpearFormCloseMatchConfirm
                    || !_isContactAddressEnabled)
                {
                    ucContactAddressEdit.DisableEditForm();
                    btnSave.Enabled = false;
                    btnSaveAndAddAnother.Enabled = false;
                    btnClear.Enabled = false;
                }

                DeactivateContactAddress(addressModel);
            }
        }

        /// <summary>
        /// On Pre-Render event method.
        /// </summary>
        /// <param name="e">The event args</param>
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
        /// Raises the <see cref="E:System.Web.UI.Page.LoadComplete" /> event at the end of the page load stage.
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data.</param>
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
        /// Validate contact address by XAPO or USPS.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">Event argument.</param>
        protected void Select_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(hfSelectedAddress.Value))
            {
                int selectedIndex = Convert.ToInt32(hfSelectedAddress.Value);
                ContactAddressModel contactAddress = validatedContactAddressList.DataSource.SingleOrDefault(p => p.RowIndex == selectedIndex);
                SaveValidatedContactAddress(contactAddress);
                hfSelectedAddress.Value = string.Empty;
            }
        }

        /// <summary>
        /// Validate contact address by XAPO or USPS.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">Event argument.</param>
        protected void Skip_Click(object sender, EventArgs e)
        {
            ContactAddressModel contactAddress = Session[SessionConstant.SESSION_VALIDATE_CONTACT_ADDRESS] as ContactAddressModel;
            contactAddress.validateFlag = ACAConstant.COMMON_N;
            SaveValidatedContactAddress(contactAddress);
            hfSelectedAddress.Value = string.Empty;
        }

        /// <summary>
        /// Handles the Click event of the SaveAndClose button control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void SaveAndCloseButton_Click(object sender, EventArgs e)
        {
            BtnSaveClick(false);
        }

        /// <summary>
        /// Handles the Click event of the SaveAndAddAnother button control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void SaveAndAddAnotherButton_Click(object sender, EventArgs e)
        {
            BtnSaveClick(true);
            Page.FocusElement(btnSaveAndAddAnother.ClientID);
        }

        /// <summary>
        /// Clear button click event.
        /// </summary>
        /// <param name="sender">An object that contains the event sender.</param>
        /// <param name="e">A System.EventArgs object containing the event data.</param>
        protected void ClearButton_Click(object sender, EventArgs e)
        {
            ucContactAddressEdit.ClearExpressionValue(true);
            ucContactAddressEdit.ClearEditForm(true);
            Page.FocusElement(btnClear.ClientID);
        }

        /// <summary>
        /// register "java script" code into page to running expression for "on submit".
        /// </summary>
        private void RegisterExpressionOnLoad()
        {
            var callJsFunction = ExpressionUtil.GetExpressionScript(false, ucContactAddressEdit);

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
            var callJsFunction = ExpressionUtil.GetExpressionScript(true, ucContactAddressEdit);
            var strSubmitFuction = ExpressionUtil.GetExpressionScriptOnSubmit(callJsFunction, null, "ButtonClientClick();");

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
                case ACAConstant.ContactSectionPosition.ModifyReferenceContact:
                    labelKey = "aca_accountmanagement_contactaddressaddnew_label_edit_title";
                    break;
                case ACAConstant.ContactSectionPosition.RegisterAccount:
                    labelKey = "aca_registration_contactaddressaddnew_label_edit_title";
                    break;
                case ACAConstant.ContactSectionPosition.RegisterClerk:
                case ACAConstant.ContactSectionPosition.EditClerk:
                    labelKey = "aca_accountmanagement_clerk_contactaddressaddnew_label_edit_title";
                    break;
                case ACAConstant.ContactSectionPosition.AuthAgentCustomerDetail:
                    labelKey = "aca_authagentcustomerdetail_contactaddressaddnew_label_edit_title";
                    break;
                case ACAConstant.ContactSectionPosition.SpearForm:
                default:
                    // This label key is for module level
                    labelKey = "aca_contactaddressaddnew_label_edit_title";
                    break;
            }

            if (!AppSession.IsAdmin)
            {
                SetPageTitleKey(labelKey);
            }
            else
            {
                lblEditContactTitle.Visible = true;
                lblEditContactTitle.LabelKey = labelKey;
                lblEditContactTitle.SectionID = ModuleName + ACAConstant.SPLIT_CHAR + ViewID + ACAConstant.SPLIT_CHAR + ucContactAddressEdit.ClientID + "_";
            }
        }

        /// <summary>
        /// Save contact address to session
        /// </summary>
        /// <returns>save to session successful</returns>
        private string SaveToSession()
        {
            string errorMsg = string.Empty;

            try
            {
                int selectIndex = -1;
                ContactAddressModel contactAddressInfo = null;
                List<ContactAddressModel> addresses = new List<ContactAddressModel>();
                PeopleModel4WS people = ContactUtil.GetPeopleModelFromContactSessionParameter();

                if (people != null && people.contactAddressList != null)
                {
                    addresses = people.contactAddressList.ToList();
                }

                if (ContactSessionParameterModel.Data.ContactAddressRowIndex != null)
                {
                    selectIndex = ContactSessionParameterModel.Data.ContactAddressRowIndex.Value;
                    contactAddressInfo = addresses.FirstOrDefault(f => f.RowIndex == selectIndex);
                }

                contactAddressInfo = ucContactAddressEdit.CollectContactAddressInfo(contactAddressInfo);

                if (!contactAddressInfo.entityID.HasValue && !string.IsNullOrEmpty(people.contactSeqNumber))
                {
                    /*
                     * Should fill the entity info for new added contact adress.
                     * Include entityID and entityType, entityType filled in business according to the Contact type(Reference contact/Daily contact),
                     *  so just fill the entityID.
                     */
                    contactAddressInfo.entityID = long.Parse(people.contactSeqNumber, CultureInfo.InvariantCulture);
                }

                if (ucContactAddressEdit.NeedValidateContactAddress(contactAddressInfo))
                {
                    //If the contact address need to validate, the default value of validate flag is N.
                    IsNeedValidateContactAddress = true;
                    contactAddressInfo.validateFlag = string.IsNullOrEmpty(contactAddressInfo.validateFlag) ? ACAConstant.COMMON_N : contactAddressInfo.validateFlag;
                    Session[SessionConstant.SESSION_VALIDATE_CONTACT_ADDRESS] = contactAddressInfo;
                    errorMsg = DisplayValidatedContactAddress();
                }
                else
                {
                    errorMsg = SaveToContactSessionParameter(people, contactAddressInfo, addresses, selectIndex);
                }
            }
            catch (Exception exception)
            {
                /*
                 * RefContactEditBefore and RefContactEditAfter event will be triggered 
                 *  in PeopleService.addContact4PublicUser and PeopleService.editRefContact.
                 */
                return exception.Message;
            }

            return errorMsg;
        }

        /// <summary>
        /// Save contact address to session parameter.
        /// </summary>
        /// <param name="people">people model</param>
        /// <param name="contactAddressInfo">contact address info</param>
        /// <param name="dataSource">data source</param>
        /// <param name="selectIndex">select index</param>
        /// <returns>save message.</returns>
        private string SaveToContactSessionParameter(PeopleModel4WS people, ContactAddressModel contactAddressInfo, List<ContactAddressModel> dataSource, int selectIndex)
        {
            string errorMsg = ucContactAddressEdit.SaveContactAddress(contactAddressInfo, dataSource, selectIndex);

            if (string.IsNullOrEmpty(errorMsg))
            {
                people.contactAddressList = dataSource.ToArray();
                ContactSessionParameterModel.Data.ContactAddressRowIndex = null;
                AppSession.SetContactSessionParameter(ContactSessionParameterModel);
            }

            return errorMsg;
        }

        /// <summary>
        /// Validate contact address by third data source.USPS, XAPO
        /// </summary>
        /// <returns>validate message.</returns>
        private string DisplayValidatedContactAddress()
        {
            SetPageTitleKey("aca_validatedcontactaddresslist_label_matchingresult");
            string errorMsg = string.Empty;
            divContactAddressInput.Visible = false;
            divValidatedContactAddressContent.Visible = true;
            ContactAddressModel contactAddress = (ContactAddressModel)Session[SessionConstant.SESSION_VALIDATE_CONTACT_ADDRESS];
            List<ContactAddressModel> validatedList = new List<ContactAddressModel>();
            IRefAddressBll refAddressBll = (IRefAddressBll)ObjectFactory.GetObject(typeof(IRefAddressBll));
            validatedList.AddRange(refAddressBll.GetContactAddressListFromExternal(CapUtil.GetAgencyCode(ModuleName), contactAddress));

            //Bind contact address list
            validatedContactAddressList.Display(validatedList);

            if (validatedList.Count < 1)
            {
                lblMsgBar.Visible = true;
                errorMsg = GetTextByKey("aca_validatedcontactaddresslist_msg_nomatching");
            }

            return errorMsg;
        }

        /// <summary>
        /// Save validated contact address in external.
        /// </summary>
        /// <param name="contactAddress">saved contact address</param>
        private void SaveValidatedContactAddress(ContactAddressModel contactAddress)
        {
            string errorMsg = string.Empty;
            List<ContactAddressModel> addresses = new List<ContactAddressModel>();
            PeopleModel4WS people = ContactUtil.GetPeopleModelFromContactSessionParameter();
            int selectIndex = ContactSessionParameterModel.Data.ContactAddressRowIndex != null ? ContactSessionParameterModel.Data.ContactAddressRowIndex.Value : -1;

            if (people != null && people.contactAddressList != null)
            {
                addresses = people.contactAddressList.ToList();
            }

            errorMsg = SaveToContactSessionParameter(people, contactAddress, addresses, selectIndex);

            if (string.IsNullOrEmpty(errorMsg))
            {
                if (ValidationUtil.IsTrue(hfIsSaveAndAdd.Value))
                {
                    DisplaySaveAndAddForm();
                }
                else
                {
                    ClientScript.RegisterStartupScript(GetType(), "SaveContactAddress", "PopupClose();", true);
                }
            }
            else
            {
                DisplayValidatedContactAddress();
                MessageUtil.ShowMessageInPopup(Page, MessageType.Error, errorMsg);
            }
        }

        /// <summary>
        /// Display save and add another form.
        /// </summary>
        private void DisplaySaveAndAddForm()
        {
            ucContactAddressEdit.ClearExpressionValue(true);
            ClientScript.RegisterStartupScript(GetType(), "RefreshContact", "RefreshContact();", true);
            ContactSessionParameterModel.Data.ContactAddressRowIndex = null;
            liClearBtn.Visible = true;

            /* 
             * divContactAddressInput.Visible must be set before ResetContactAddressForm.
             * Otherwise, some default values such as State maybe no way to set by ResetContactAddressForm.
             */
            divContactAddressInput.Visible = true;
            InitTitle();
            ucContactAddressEdit.ResetContactAddressForm(false);

            divValidatedContactAddressContent.Visible = false;
        }

        /// <summary>
        /// Deactivate contact address
        /// </summary>
        /// <param name="contactAddress">contact address model</param>
        private void DeactivateContactAddress(ContactAddressModel contactAddress)
        {
            if (ValidationUtil.IsYes(Request.QueryString["isDeactivateAction"]))
            {
                btnSave.Enabled = true;
                btnClear.Enabled = true;
                btnSaveAndAddAnother.Enabled = false;
                ucContactAddressEdit.DeactivateContactAddress(contactAddress);
            }
        }

        /// <summary>
        /// 'Save And Close/Save And Add Another' button event.
        /// </summary>
        /// <param name="isSaveAndAdd">Is save and add another</param>
        private void BtnSaveClick(bool isSaveAndAdd)
        {
            hfIsSaveAndAdd.Value = isSaveAndAdd ? ACAConstant.COMMON_TRUE : ACAConstant.COMMON_FALSE;
            string errorMsg = SaveToSession();

            if (string.IsNullOrEmpty(errorMsg))
            {
                hfSelectedAddress.Value = string.Empty;

                if (IsNeedValidateContactAddress)
                {
                    return;
                }

                if (isSaveAndAdd)
                {
                    DisplaySaveAndAddForm();
                }
                else
                {
                    ClientScript.RegisterStartupScript(GetType(), "SaveContactAddress", "PopupClose();", true);
                }
            }
            else
            {
                MessageUtil.ShowMessageInPopup(Page, MessageType.Error, errorMsg);
            }
        }

        #endregion Methods
    }
}