#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: ContactAddressList.aspx.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2011-2014
 *
 *  Description:
 *
 *  Notes:
 *      $Id: ContactAddressList.ascx.cs 277634 2014-08-19 08:16:23Z ACHIEVO\james.shi $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using Accela.ACA.BLL.Common;
using Accela.ACA.Common;
using Accela.ACA.Common.Common;
using Accela.ACA.Common.Util;
using Accela.ACA.UI.Model;
using Accela.ACA.Web.Common;
using Accela.ACA.Web.Common.Control;
using Accela.ACA.Web.People;
using Accela.ACA.WSProxy;
using Accela.Web.Controls;

namespace Accela.ACA.Web.Component
{
    /// <summary>
    /// List view of the contact addresses.
    /// </summary>
    public partial class ContactAddressList : BaseUserControl
    {
        #region Fields

        /// <summary>
        /// Command name of deleting a contact address.
        /// </summary>
        protected const string COMMAND_DELETE_CONTACT_ADDRESS = "DeleteContactAddress";

        /// <summary>
        /// Command name of selecting a contact address.
        /// </summary>
        protected const string COMMAND_SELECT_CONTACT_ADDRESS = "SelectContactAddress";

        /// <summary>
        /// Command name of deactivate a contact address.
        /// </summary>
        protected const string COMMAND_DEACTIVATE_PRIMARY_CONTACT_ADDRESS = "DeactivatePrimaryContactAddress";

        /// <summary>
        /// Command name of set primary a contact address.
        /// </summary>
        protected const string COMMAND_SET_PRIMARY_CONTACT_ADDRESS = "SetPrimaryContactAddress";

        /// <summary>
        /// A value to indicating have not any records been selected.
        /// </summary>
        private const int NO_SELECTED = -1;

        /// <summary>
        /// Indicating whether the contact address form is need to validate reference data.
        /// </summary>
        private bool _isValidate;

        /// <summary>
        /// The contact section position.
        /// </summary>
        private ACAConstant.ContactSectionPosition _contactSectionPosition;

        /// <summary>
        /// Indicating whether the contact address is enabled.
        /// </summary>
        private bool _isEnable;

        #endregion

        #region Events

        /// <summary>
        /// Select contact address event.
        /// </summary>
        public event CommonEventHandler ContactAddressSelected;

        /// <summary>
        /// Deactivated contact address event.
        /// </summary>
        public event CommonEventHandler ContactAddressDeactivate;

        /// <summary>
        /// Data source changed event.
        /// </summary>
        public event CommonEventHandler DataSourceChanged;

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the current contact type.
        /// </summary>
        public string ContactType
        { 
            get
            {
                return ViewState["ContactType"] == null ? string.Empty : ViewState["ContactType"].ToString();
            }

            set
            {
                ViewState["ContactType"] = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether of contact address form editable property.
        /// </summary>
        public bool IsEditable
        {
            get
            {
                return bool.Parse(ViewState["IsEditable"].ToString());
            }

            set
            {
                ViewState["IsEditable"] = value;
                divButtons.Visible = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether of contact address form editable property.
        /// </summary>
        public bool IsForView
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets a value indicating whether the contact address form validate.
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
            }
        }

        /// <summary>
        /// Gets or sets the contact section position.
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
                gdvContactAddressList.GridViewNumber = PeopleUtil.GetContactAddressListGviewID(_contactSectionPosition);
                GridViewBuildHelper.SetSimpleViewElements(gdvContactAddressList, ModuleName, AppSession.IsAdmin);
            }
        }

        /// <summary>
        /// Gets or sets the selected index of contact address list.
        /// </summary>
        public int SelectedIndex
        {
            get
            {
                return ViewState["SelectedIndex"] == null ? NO_SELECTED : Convert.ToInt32(ViewState["SelectedIndex"]);
            }

            set
            {
                ViewState["SelectedIndex"] = value;
            }
        }

        /// <summary>
        /// Gets contact address list.
        /// </summary>
        public IList<ContactAddressModel> DataSource
        {
            get
            {
                if (gdvContactAddressList.DataSource is IList<ContactAddressModel>)
                {
                    return gdvContactAddressList.DataSource as IList<ContactAddressModel>;
                }
                else if (gdvContactAddressList.DataSource is IList)
                {
                    IList list = gdvContactAddressList.DataSource as IList;
                    IList<ContactAddressModel> dataList = new List<ContactAddressModel>();

                    foreach (object item in list)
                    {
                        if (item is ContactAddressModel)
                        {
                            dataList.Add(item as ContactAddressModel);
                        }
                    }
                    
                    return dataList;
                }
                else
                {
                    return new List<ContactAddressModel>();
                }
            }

            private set
            {
                //Generate row index.
                IList<ContactAddressModel> contactAddressList = value;
                int index = 0;

                if (contactAddressList != null && contactAddressList.Count > 0)
                {
                    foreach (ContactAddressModel contactAddress in contactAddressList)
                    {
                        contactAddress.RowIndex = index;
                        index++;
                    }
                }
                else
                {
                    contactAddressList = new List<ContactAddressModel>();
                }

                gdvContactAddressList.DataSource = contactAddressList;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether current contact is reference or not.
        /// </summary>
        public bool IsRefContact
        {
            get
            {
                if (ViewState["IsRefContact"] == null)
                {
                    return false;
                }

                return (bool)ViewState["IsRefContact"];
            }

            set
            {
                ViewState["IsRefContact"] = value;
            }
        }

        /// <summary>
        /// Gets or sets the view id for reference contact list
        /// </summary>
        public string GViewID
        {
            get
            {
                return gdvContactAddressList.GridViewNumber;
            }

            set
            {
                gdvContactAddressList.GridViewNumber = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the contact address list is initialized.
        /// </summary>
        public bool IsListInitialized
        {
            get
            {
                if (ViewState["IsListInitialized"] != null)
                {
                    return (bool)ViewState["IsListInitialized"];
                }

                return false;
            }

            set
            {
                ViewState["IsListInitialized"] = value;
            }
        }

        /// <summary>
        /// Gets a value indicating whether contact address list is null or not.
        /// </summary>
        public bool IsListEmpty
        {
            get
            {
                return DataSource.Count < 1;
            }
        }

        /// <summary>
        /// Gets or sets the clientID of the parent control.
        /// </summary>
        public string ParentClientID
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets a value indicating whether [need create session].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [need create session]; otherwise, <c>false</c>.
        /// </value>
        public bool NeedCreateSession
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the session parameter string.
        /// </summary>
        /// <value>
        /// The session parameter string.
        /// </value>
        public string SessionParameterString
        {
            get;
            set;
        }

        /// <summary>
        /// Gets callback function name when success to add/edit people callback to refresh base form.
        /// </summary>
        /// <value>
        /// The parent post id.
        /// </value>
        public string CallbackFunctionName
        {
            get
            {
                return ClientID + "_Refresh";
            }
        }

        /// <summary>
        /// Gets the name of the session parameter function.
        /// </summary>
        /// <value>
        /// The name of the session parameter function.
        /// </value>
        protected string SessionParameterFunctionName
        {
            get
            {
                return ClientID + "_OperationSession";
            }
        }

        /// <summary>
        /// Gets the name of the session parameter function call back function.
        /// </summary>
        protected string CallBack4CreateSessionFunction
        {
            get
            {
                return ClientID + "_AddNewContactAddress";
            }
        }

        /// <summary>
        /// Gets the section id for contact address form.
        /// </summary>
        private string SectionId4ContactAddressForm
        {
            get
            {
                return PeopleUtil.GetContactAddressEditGviewID(ContactSectionPosition);
            }
        }

        /// <summary>
        /// Gets the contact session parameter.
        /// </summary>
        /// <value>
        /// The contact session parameter.
        /// </value>
        private ContactSessionParameter ContactSessionParameter
        {
            get
            {
                return AppSession.GetContactSessionParameter();
            }
        }

        /// <summary>
        /// Gets a value indicating whether the contact data is close match or not.
        /// </summary>
        private bool IsCloseMatch
        {
            get
            {
                return ContactSessionParameter != null 
                    && ContactSessionParameter.Data != null 
                    && ContactSessionParameter.Data.IsCloseMatch;
            }
        }

        /// <summary>
        /// Gets a value indicating whether the contact type editable settings or not.
        /// </summary>
        private bool ContactTypePermission
        {
            get
            {
                if (!AppSession.IsAdmin && ACAConstant.ContactSectionPosition.ModifyReferenceContact.Equals(ContactSectionPosition))
                {
                    return ContactUtil.GetContactTypeEditablePermissionByContactSeqNbr(Request.QueryString[UrlConstant.CONTACT_SEQ_NUMBER]);
                }

                return true;
            }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Collect contact addresses from contact address list.
        /// </summary>
        /// <returns>Array of contact address.</returns>
        public ContactAddressModel[] GetContactAddresses()
        {
            return DataSource.ToArray();
        }

        /// <summary>
        /// Disable contact address edit form.
        /// </summary>
        public void DisableEditForm()
        {
            if (!AppSession.IsAdmin)
            {
                btnAddContactAddress.Enabled = false;
                btnAddContactAddress.OnClientClick = string.Empty;
            }
        }

        /// <summary>
        /// Load contact address list.
        /// </summary>
        /// <param name="contactAddresses">Contact address list.</param>
        public void Display(ContactAddressModel[] contactAddresses)
        {
            this.Display(contactAddresses == null ? new List<ContactAddressModel>() : contactAddresses.ToList());
        }

        /// <summary>
        /// Load contact address list.
        /// </summary>
        /// <param name="contactAddresses">Contact address list.</param>
        /// <param name="needValidateAddressType">Indicates whether need to validate the required settings for contact address type.</param>
        public void Display(ContactAddressModel[] contactAddresses, bool needValidateAddressType)
        {
            this.Display(contactAddresses == null ? new List<ContactAddressModel>() : contactAddresses.ToList(), needValidateAddressType);
        }

        /// <summary>
        /// Load contact address list.
        /// </summary>
        /// <param name="contactAddresses">Contact address list.</param>
        public void Display(IList<ContactAddressModel> contactAddresses)
        {
            DataSource = contactAddresses;
            BindData();
            SelectedIndex = NO_SELECTED;
        }

        /// <summary>
        /// Load contact address list.
        /// </summary>
        /// <param name="contactAddresses">Contact address list.</param>
        /// <param name="needValidateAddressType">Indicates whether need to validate the required settings for contact address type.</param>
        public void Display(IList<ContactAddressModel> contactAddresses, bool needValidateAddressType)
        {
            this.Display(contactAddresses);
            Validate(DataSource, needValidateAddressType, true);
            IsListInitialized = true;
        }

        /// <summary>
        /// Show action notice message.
        /// </summary>
        /// <param name="actionType">The action type.</param>
        public void ShowActionNoticeMessage(ActionType actionType)
        {
            InitMessage();
            string labelKey = string.Empty;
            divImgSuccess.Visible = true;

            switch (actionType)
            {
                case ActionType.AddSuccess:
                    lblActionNoticeAddSuccess.Visible = true;
                    labelKey = lblActionNoticeAddSuccess.LabelKey;
                    break;
                case ActionType.UpdateSuccess:
                    lblActionNoticeUpdateSuccess.Visible = true;
                    labelKey = lblActionNoticeUpdateSuccess.LabelKey;
                    break;
                case ActionType.DeleteSuccess:
                    lblActionNoticeDeleteSuccess.Visible = true;
                    labelKey = lblActionNoticeDeleteSuccess.LabelKey;
                    break;
                case ActionType.Deactivated:
                    lblActionDeactivatedSuccess.Visible = true;
                    labelKey = lblActionDeactivatedSuccess.LabelKey;
                    break;
                case ActionType.Duplicate:
                    lblDuplicateContactAddressMessage.Visible = true;
                    labelKey = lblDuplicateContactAddressMessage.LabelKey;
                    break;
                case ActionType.DuplicateToSelf:
                    lblDuplicateToSelf.Visible = true;
                    labelKey = lblDuplicateToSelf.LabelKey;
                    break;
                case ActionType.SetAsPrimary:
                    lblActionSetPrimarySuccess.Visible = true;
                    labelKey = lblActionSetPrimarySuccess.LabelKey;
                    break;
            }

            ScriptManager.RegisterClientScriptBlock(
                Page,
                Page.GetType(),
                "GotoMessageBar",
                string.Format("Sys.WebForms.PageRequestManager.getInstance().add_pageLoaded(function(){{{0}_scrollIntoView('{1}');}});", ClientID, divContactAddressList.ClientID),
                true);

            if (AccessibilityUtil.AccessibilityEnabled)
            {
                MessageUtil.ShowAlertMessage(this, GetTextByKey(labelKey));
            }
        }

        /// <summary>
        /// Validate required fields and fields format.
        /// </summary>
        /// <param name="contactAddresses">Array of contact address model.</param>
        /// <param name="needValidate">Indicates whether need to validate the required settings for contact address type.</param>
        /// <param name="isLocate">a value indicating whether message need be located or not.</param>
        /// <returns>Error message if any.</returns>
        public string Validate(IList<ContactAddressModel> contactAddresses, bool needValidate, bool isLocate)
        {
            if (!StandardChoiceUtil.IsEnableContactAddress()
                || (!StandardChoiceUtil.IsEnableContactAddressMaintenance()
                    && (ACAConstant.ContactSectionPosition.RegisterAccount.Equals(ContactSectionPosition)
                        || ACAConstant.ContactSectionPosition.RegisterClerk.Equals(ContactSectionPosition)
                        || ACAConstant.ContactSectionPosition.RegisterAccountConfirm.Equals(ContactSectionPosition)
                        || ACAConstant.ContactSectionPosition.SpearFormCloseMatchConfirm.Equals(ContactSectionPosition)
                        || ACAConstant.ContactSectionPosition.RegisterClerkConfirm.Equals(ContactSectionPosition)
                        || ACAConstant.ContactSectionPosition.AddReferenceContact.Equals(ContactSectionPosition)
                        || ACAConstant.ContactSectionPosition.EditClerk.Equals(ContactSectionPosition)
                        || ACAConstant.ContactSectionPosition.ModifyReferenceContact.Equals(ContactSectionPosition)))
                || (ContactSessionParameter != null
                   && ContactSessionParameter.Data != null
                   && ContactSessionParameter.Data.IsCloseMatch))
            {
                return string.Empty;
            }

            divIncompleteMark.Visible = false;
            divDuplicateMark.Visible = false;
            List<string> message = new List<string>();

            if (needValidate)
            {
                string primaryAddressMessage = ContactUtil.GetPrimaryContactAddressMessage(ModuleName, ContactType, IsEditable, ContactSectionPosition, contactAddresses);

                if (!string.IsNullOrEmpty(primaryAddressMessage))
                {
                    divIncompleteMark.Visible = true;
                    message.Add(primaryAddressMessage);
                }
            }

            // Data from daily need to do validate. 
            if (!(IsValidate && IsRefContact) && IsEditable)
            {
                // Auto-fill will not validate.needValidateAddressType is used for this.
                string requiredAddressType = needValidate ? RequiredValidationUtil.ValidateContactAddressType(ContactType, contactAddresses) : string.Empty;

                if (!RequiredValidationUtil.ValidateFields4ContactAddressList(ModuleName, contactAddresses, SectionId4ContactAddressForm, ContactSectionPosition) ||
                    !FormatValidationUtil.ValidateFormat4ContactAddressList(ModuleName, contactAddresses, SectionId4ContactAddressForm, ContactSectionPosition))
                {
                    divIncompleteMark.Visible = true;
                    message.Add(GetTextByKey(lblIncomplete.LabelKey));
                }

                if (!string.IsNullOrEmpty(requiredAddressType))
                {
                    divIncompleteMark.Visible = true;
                    message.Add(requiredAddressType);
                }

                string duplicateContactAddressMessage = ContactUtil.GetErrorMessage4DuplicateContactAddress(ModuleName, ConfigManager.AgencyCode, contactAddresses);

                if (!string.IsNullOrWhiteSpace(duplicateContactAddressMessage) && !divIncompleteMark.Visible)
                {
                    divDuplicateMark.Visible = true;
                    message.Add(duplicateContactAddressMessage);
                }
            }

            lblIncomplete.Text = DataUtil.ConcatStringWithSplitChar(message, ACAConstant.HTML_BR);

            if (!string.IsNullOrEmpty(lblIncomplete.Text) && isLocate)
            {
                var script = string.Format("{0}_ToggleEditForm(true);setTimeout(\"{0}_scrollIntoView('{1}');\",0);", ClientID, divIncompleteMark.ClientID);
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "ContactAddress" + CommonUtil.GetRandomUniqueID(), script, true);
            }

            if (AccessibilityUtil.AccessibilityEnabled)
            {
                ltScriptForIncomplete.Text = MessageUtil.GetAlertScript(lblIncomplete.Text);
            }

            string generalErrorMsg = !string.IsNullOrEmpty(lblIncomplete.Text) ? GetTextByKey("aca_common_msg_validateerror") : string.Empty;

            if (!string.IsNullOrWhiteSpace(generalErrorMsg))
            {
                pnlContactAdressList.Update();
            }

            return generalErrorMsg;
        }

        /// <summary>
        /// Shows the validate error message.
        /// </summary>
        /// <param name="message">The message.</param>
        public void ShowValidateErrorMessage(string message)
        {
            if (!string.IsNullOrEmpty(message))
            {
                divIncompleteMark.Visible = true;
                lblIncomplete.Text = message;
            }
        }

        /// <summary>
        /// Hide the validate error message.
        /// </summary>
        public void HideValidateErrorMessage()
        {
            if (divIncompleteMark.Visible)
            {
                divIncompleteMark.Visible = false;
                lblIncomplete.Text = string.Empty;
            }
        }

        #endregion Public Methods

        #region Protected Methods

        /// <summary>
        /// <c>OnInit</c> event method
        /// </summary>
        /// <param name="e">EventArgs e</param>
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            IsEditable = false;
        }

        /// <summary>
        /// Handle the Page_Load event.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">Event argument.</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            // Data source is Reference
            if ((IsValidate && IsRefContact) || !IsEditable)
            {
                btnAddContactAddress.Enabled = false;
                btnAddContactAddress.OnClientClick = string.Empty;
            }
            else
            {
                btnAddContactAddress.Enabled = true;
                btnAddContactAddress.OnClientClick =
                string.Format(
                        "{0}('{1}', '{2}', '{3}', {4}); return false;",
                        SessionParameterFunctionName,
                        string.Empty,
                        ContactAddressProcessType.Add.ToString("D"),
                        btnAddContactAddress.ClientID,
                        CallBack4CreateSessionFunction);
            }

            if (AppSession.IsAdmin
                && ContactSectionPosition != ACAConstant.ContactSectionPosition.RegisterAccountComplete
                && ContactSectionPosition != ACAConstant.ContactSectionPosition.RegisterClerkComplete)
            {
                divButtons.Visible = true;
                lblActionNoticeAddSuccess.Visible = true;
                lblActionNoticeDeleteSuccess.Visible = true;
                lblActionNoticeUpdateSuccess.Visible = true;
                lblActionDeactivatedSuccess.Visible = true;
                lblDuplicateContactAddressMessage.Visible = true;
            }

            if (StandardChoiceUtil.IsEnableContactAddress())
            {
                string contactType = string.Empty;
                ContactSessionParameter contactSessionParameter = AppSession.GetContactSessionParameter();

                if (!string.IsNullOrEmpty(ContactType))
                {
                    contactType = ContactType;
                }
                else if (contactSessionParameter != null)
                {
                    contactType = contactSessionParameter.ContactType;
                    ContactType = contactSessionParameter.ContactType;
                }

                ShowRequiredInstruction(contactType);
            }

            if (!AppSession.IsAdmin
                && !ACAConstant.ContactSectionPosition.SpearForm.Equals(ContactSectionPosition)
                && !ACAConstant.ContactSectionPosition.AuthAgentCustomerDetail.Equals(ContactSectionPosition))
            {
                bool isEnableContactAddressMaintenance = StandardChoiceUtil.IsEnableContactAddressMaintenance();
                divButtons.Visible = isEnableContactAddressMaintenance;
                btnAddContactAddress.Visible = isEnableContactAddressMaintenance;
                lblContactAddressListInstruction.Visible = isEnableContactAddressMaintenance;
            }

            if (ACAConstant.ContactSectionPosition.RegisterAccountComplete.Equals(ContactSectionPosition)
                || ACAConstant.ContactSectionPosition.ValidatedContactAddress.Equals(ContactSectionPosition)
                || ACAConstant.ContactSectionPosition.RegisterClerkComplete.Equals(ContactSectionPosition)
                || ACAConstant.ContactSectionPosition.SpearFormCloseMatchConfirm.Equals(ContactSectionPosition)
                || ((ACAConstant.ContactSectionPosition.RegisterAccount.Equals(ContactSectionPosition)
                        || ACAConstant.ContactSectionPosition.RegisterAccountConfirm.Equals(ContactSectionPosition)
                        || ACAConstant.ContactSectionPosition.RegisterClerk.Equals(ContactSectionPosition)
                        || ACAConstant.ContactSectionPosition.RegisterClerkConfirm.Equals(ContactSectionPosition))
                    && IsCloseMatch))
            {
                divButtons.Visible = false;
                btnAddContactAddress.Visible = false;
                lblContactAddressListInstruction.Visible = false;
            }

            if (AppSession.IsAdmin && ACAConstant.ContactSectionPosition.ModifyReferenceContact.Equals(ContactSectionPosition))
            {
                btnAddContactAddress.Enabled = true;
                btnAddContactAddress.OnClientClick = string.Empty;
            }

            DialogUtil.RegisterScriptForDialog(Page);
        }

        /// <summary>
        /// Override OnPreRender event to change the label key.
        /// </summary>
        /// <param name="e">Event argument.</param>
        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);

            // Hide the column of rdoSelector.
            if (!GviewID.ExternalAddressList.Equals(GViewID))
            {
                foreach (DataControlField field in gdvContactAddressList.Columns)
                {
                    AccelaTemplateField accelaTemplate = field as AccelaTemplateField;

                    if (accelaTemplate != null && accelaTemplate.ColumnId == "colSelector")
                    {
                        accelaTemplate.Visible = false;
                        break;
                    }
                }
            }

            //Switch the label key for Authorized Agent Customer Detail page.
            ChangeLabelKey();

            if (!AppSession.IsAdmin
                && ContactSessionParameter != null
                && ContactSessionParameter.Data != null
                && ContactSessionParameter.Data.IsCloseMatch)
            {
                divButtons.Visible = false;
            }
        }

        /// <summary>
        /// Handle the RowCommand event for contact address list.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">Event argument.</param>
        protected void ContactAddressList_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            switch (e.CommandName)
            {
                case "Header":

                    BindData();
                    break;
            }
        }

        /// <summary>
        /// Handle the RowCommand event for contact address list.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">Event argument.</param>
        protected void ContactAddressList_ActionCommand(object sender, EventArgs e)
        {
            int rowIndex = 0;
            AccelaButton actionButton = (AccelaButton)sender;

            switch (actionButton.CommandName)
            {
                case COMMAND_DELETE_CONTACT_ADDRESS:
                    //delete a contact
                    rowIndex = Convert.ToInt32(actionButton.CommandArgument);
                    DeleteContactAddress(sender, rowIndex);
                    break;

                case COMMAND_DEACTIVATE_PRIMARY_CONTACT_ADDRESS:
                    rowIndex = Convert.ToInt32(actionButton.CommandArgument);
                    DeactivateContactAddress(sender, rowIndex, true);
                    break;

                case COMMAND_SET_PRIMARY_CONTACT_ADDRESS:
                    //set primary a contact
                    rowIndex = Convert.ToInt32(actionButton.CommandArgument);
                    SetPrimaryContactAddress(sender, rowIndex);
                    break;
            }
        }

        /// <summary>
        /// Handle the RowDataBound event for contact address list.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">Event argument.</param>
        protected void ContactAddressList_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                ContactAddressModel contactAddress = e.Row.DataItem as ContactAddressModel;

                if (contactAddress == null)
                {
                    return;
                }

                AccelaLabel lblStatus = (AccelaLabel)e.Row.FindControl("lblStatus");
                AccelaLabel lblPrimary = (AccelaLabel)e.Row.FindControl("lblPrimary");
                AccelaLabel lblAddress = (AccelaLabel)e.Row.FindControl("lblAddress");
                AccelaLabel lblValidated = (AccelaLabel)e.Row.FindControl("lblValidated");
                AccelaDiv divRequiredMark = (AccelaDiv)e.Row.FindControl("divRequiredMark");
                AccelaDiv divDuplicatedMark = (AccelaDiv)e.Row.FindControl("divDuplicatedMark");
                AccelaLinkButton lnkAddress = e.Row.FindControl("lnkAddress") as AccelaLinkButton;
                AccelaRadioButton rdoSelector = (AccelaRadioButton)e.Row.FindControl("rdoSelector");

                string contactSequence = Request.QueryString[UrlConstant.CONTACT_SEQ_NUMBER];

                if (ContactSectionPosition == ACAConstant.ContactSectionPosition.SpearForm
                    || string.IsNullOrEmpty(contactSequence)
                    || ContactUtil.IsContactTypeEnable4AcountContactEdit(contactSequence, ContactType))
                {
                    BuildActionMenu(e);
                }

                rdoSelector.InputAttributes.Add("onclick", "ClickValidatedAddress(this)");
                rdoSelector.InputAttributes.Add("rowindex", contactAddress.RowIndex.ToString());
                rdoSelector.InputAttributes.Add("title", GetTextByKey("aca_selectonerecord_checkbox"));
                lblPrimary.Text = ValidationUtil.IsYes(contactAddress.primary) ? GetTextByKey("ACA_Common_Yes") : GetTextByKey("ACA_Common_No");
                string auditStatus = contactAddress.auditModel != null ? contactAddress.auditModel.auditStatus : string.Empty;
                lblStatus.Text = ACAConstant.VALID_STATUS.Equals(auditStatus, StringComparison.InvariantCultureIgnoreCase)
                    ? LabelUtil.GetTextByKey("aca_common_active", ModuleName) : LabelUtil.GetTextByKey("aca_common_inactive", ModuleName);

                if (ValidationUtil.IsYes(contactAddress.validateFlag))
                {
                    lblValidated.Text = GetTextByKey("ACA_Common_Yes");
                }
                else if (ValidationUtil.IsNo(contactAddress.validateFlag))
                {
                    lblValidated.Text = GetTextByKey("ACA_Common_No");
                }

                if (lnkAddress != null)
                {
                    string address = ScriptFilter.EncodeHtml(ContactUtil.BuildAddress(contactAddress));

                    if (!IsForView)
                    {
                        lnkAddress.Text = address;
                    }
                    else
                    {
                        lblAddress.Visible = true;
                        lnkAddress.Visible = false;
                        lblAddress.Text = address;
                    }

                    //address link
                    string clientScript = string.Format(
                                                "{0}('{1}', '{2}', '{3}', {4}); return false;",
                                                SessionParameterFunctionName,
                                                contactAddress.RowIndex,
                                                ContactAddressProcessType.Edit.ToString("D"),
                                                lnkAddress.ClientID,
                                                CallBack4CreateSessionFunction);
                    lnkAddress.OnClientClick = clientScript;
                }

                if (!IsForView)
                {
                    //validate contact address model fields required and format.
                    ContactAddressModel[] contactAddresses = { contactAddress };

                    if (!IsValidate
                        && IsEditable
                        && (!RequiredValidationUtil.ValidateFields4ContactAddressList(ModuleName, contactAddresses, SectionId4ContactAddressForm, ContactSectionPosition)
                               || !FormatValidationUtil.ValidateFormat4ContactAddressList(ModuleName, contactAddresses, SectionId4ContactAddressForm, ContactSectionPosition)))
                    {
                        divRequiredMark.Visible = true;
                    }

                    ContactAddressModel[] duplicateContactAddressList = ContactUtil.GetDuplicateContactAddressList(DataSource, contactAddress);

                    if (!IsValidate
                        && IsEditable
                        && duplicateContactAddressList != null
                        && duplicateContactAddressList.Length > 0
                        && !divRequiredMark.Visible)
                    {
                        divDuplicatedMark.Visible = true;
                    }
                    else
                    {
                        divDuplicatedMark.Visible = false;
                    }
                }
            }
            else if (e.Row.RowType == DataControlRowType.Header)
            {
                AccelaLabel lblRecipientTitle = (AccelaLabel)e.Row.FindControl("lblRecipientTitle") as AccelaLabel;
                AccelaLabel lblAddressTypeTitle = (AccelaLabel)e.Row.FindControl("lblAddressTypeTitle") as AccelaLabel;
                GridViewHeaderLabel lnkRecipient = (GridViewHeaderLabel)e.Row.FindControl("lnkRecipient") as GridViewHeaderLabel;
                GridViewHeaderLabel lnkAddressType = (GridViewHeaderLabel)e.Row.FindControl("lnkAddressType") as GridViewHeaderLabel;

                if (IsForView)
                {
                    lnkRecipient.Visible = lnkAddressType.Visible = false;
                    lblRecipientTitle.Visible = lblAddressTypeTitle.Visible = true;
                }
            }
        }

        /// <summary>
        /// Deactivate the normal contact address.
        /// </summary>
        /// <param name="sender">Trigger object.</param>
        /// <param name="e">Event argument.</param>
        protected void Deactivate_OnClick(object sender, EventArgs e)
        {
            int rowIndex = Convert.ToInt32(hfRowIndex.Value);
            DeactivateContactAddress(sender, rowIndex, false);
        }

        /// <summary>
        /// Refresh the contact address list.
        /// </summary>
        /// <param name="sender">An object that contains the event sender.</param>
        /// <param name="e">An System.EventArgs object that contains the event data.</param>
        protected void RefreshButton_Click(object sender, EventArgs e)
        {
            ContactSessionParameter contactSessionParameter = AppSession.GetContactSessionParameter();
            PeopleModel4WS people = ContactUtil.GetPeopleModelFromContactSessionParameter(contactSessionParameter);

            // Load contact address list
            if (people != null)
            {
                Display(people.contactAddressList, false);

                if (DataSourceChanged != null)
                {
                    var parameter = new KeyValuePair<ContactAddressProcessType, List<ContactAddressModel>>(contactSessionParameter.Process.ContactAddressProcessType, people.contactAddressList.ToList());
                    DataSourceChanged(sender, new CommonEventArgs(parameter));
                }
            }

            // Show message
            ActionType actionType = hfIsForNew.Value == "1" ? ActionType.AddSuccess : ActionType.UpdateSuccess;
            ShowActionNoticeMessage(actionType);

            Page.FocusElement(Request.Form["__LASTFOCUS_ID"]);
        }

        #endregion Protected Methods

        #region Private Methods

        /// <summary>
        /// Show required address type instruction message.
        /// </summary>
        /// <param name="contactType">current contact type</param>
        private void ShowRequiredInstruction(string contactType)
        {
            List<string> instruction = new List<string>();
            instruction.Add(GetTextByKey("aca_contactaddress_label_instruction"));

            if (!AppSession.IsAdmin)
            {
                if (StandardChoiceUtil.IsPrimaryContactAddressRequired()
                    && ACAConstant.ContactSectionPosition.SpearForm.Equals(ContactSectionPosition))
                {
                    instruction.Add(GetTextByKey("aca_contactaddress_message_needprimary"));
                }

                instruction.Add(PeopleUtil.GetRequiredAddressInstruction(ModuleName, contactType));
            }

            lblContactAddressListInstruction.Text = DataUtil.ConcatStringWithSplitChar(instruction, ACAConstant.HTML_BR);
        }

        /// <summary>
        /// Re-bind data to contact address list.
        /// </summary>
        private void BindData()
        {
            gdvContactAddressList.DataSource = DataSource;
            gdvContactAddressList.EmptyDataText = AppSession.IsAdmin ? string.Empty : GetTextByKey("per_permitList_messagel_noRecord");
            gdvContactAddressList.DataBind();

            ShowIncompleteMark();
            ShowDuplicateMark();
        }

        /// <summary>
        /// Check if contact addresses are duplicated.
        /// </summary>
        private void ShowDuplicateMark()
        {
            string errorMessage = ContactUtil.GetErrorMessage4DuplicateContactAddress(ModuleName, ConfigManager.AgencyCode, DataSource);

            if (!IsValidate
                && IsEditable
                && !string.IsNullOrWhiteSpace(errorMessage))
            {
                divDuplicateMark.Visible = true;
            }
            else
            {
                divDuplicateMark.Visible = false;
            }
        }

        /// <summary>
        /// Validate contact addresses and to show incomplete mark.
        /// </summary>
        private void ShowIncompleteMark()
        {
            if (!IsValidate
                && IsEditable
                && (!RequiredValidationUtil.ValidateFields4ContactAddressList(ModuleName, DataSource, SectionId4ContactAddressForm, ContactSectionPosition)
                       || !FormatValidationUtil.ValidateFormat4ContactAddressList(ModuleName, DataSource, SectionId4ContactAddressForm, ContactSectionPosition)))
            {
                divIncompleteMark.Visible = true;
            }
            else
            {
                divIncompleteMark.Visible = false;
            }
        }

        /// <summary>
        /// Initial Message.
        /// </summary>
        private void InitMessage()
        {
            lblActionNoticeAddSuccess.Visible = false;
            lblActionNoticeDeleteSuccess.Visible = false;
            lblActionNoticeUpdateSuccess.Visible = false;
            lblActionDeactivatedSuccess.Visible = false;
            lblDuplicateContactAddressMessage.Visible = false;
            lblDuplicateToSelf.Visible = false;
            divImgSuccess.Visible = false;
        }

        /// <summary>
        /// Delete a contact from contact list.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="rowIndex">Selected item row index.</param>
        private void DeleteContactAddress(object sender, int rowIndex)
        {
            ContactAddressModel contactAddress = DataSource.FirstOrDefault(p => p.RowIndex == rowIndex);
            DataSource.Remove(contactAddress);
            Display(DataSource.ToArray(), true);

            if (DataSourceChanged != null)
            {
                var parameter = new KeyValuePair<ContactAddressProcessType, List<ContactAddressModel>>(ContactAddressProcessType.Delete, DataSource.ToList());
                DataSourceChanged(sender, new CommonEventArgs(parameter));
            }

            // If no duplicate contact address found, hide the error message.
            string errorMessage = ContactUtil.GetErrorMessage4DuplicateContactAddress(ModuleName, ConfigManager.AgencyCode, DataSource);

            if (string.IsNullOrWhiteSpace(errorMessage))
            {
                divDuplicateMark.Visible = false;
            }

            ShowActionNoticeMessage(ActionType.DeleteSuccess);
            Page.FocusElement(string.Format("{0}_lnkAddContactAddress", ClientID));
        }

        /// <summary>
        /// Deactivate contact address and raise ContactAddressSelected event.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="rowIndex">Selected item row index.</param>
        /// <param name="isContactAddressUsedInDailyAsPrimary">Is Contact Address Used In Daily As Primary</param>
        private void DeactivateContactAddress(object sender, int rowIndex, bool isContactAddressUsedInDailyAsPrimary)
        {
            if (!isContactAddressUsedInDailyAsPrimary)
            {
                ContactAddressModel contactAddress = DataSource.FirstOrDefault(p => p.RowIndex == rowIndex);
                string endDate = catEndDate.Text.Trim();
                contactAddress.auditModel.auditStatus = ACAConstant.INVALID_STATUS;

                if (!string.IsNullOrEmpty(endDate))
                {
                    contactAddress.expirationDate = I18nDateTimeUtil.ParseFromUI(endDate);
                }
                else
                {
                    contactAddress.expirationDate = null;
                }

                BindData();

                if (DataSourceChanged != null)
                {
                    var parameter = new KeyValuePair<ContactAddressProcessType, List<ContactAddressModel>>(ContactAddressProcessType.Edit, DataSource.ToList());
                    DataSourceChanged(sender, new CommonEventArgs(parameter));
                    ShowActionNoticeMessage(ActionType.Deactivated);
                    Page.FocusElement(string.Format("{0}_lnkAddContactAddress", ClientID));
                }
            }
        }

        /// <summary>
        /// Set as primary contact address and raise ContactAddressSelected event.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="rowIndex">Selected item row index.</param>
        private void SetPrimaryContactAddress(object sender, int rowIndex)
        {
            if (DataSource != null)
            {
                foreach (ContactAddressModel address in DataSource)
                {
                    address.primary = ACAConstant.COMMON_N;
                }
            }

            SelectedIndex = rowIndex;
            ContactAddressModel contactAddress = DataSource.Where(p => p.RowIndex == rowIndex).SingleOrDefault();
            contactAddress.primary = ACAConstant.COMMON_Y;
            BindData();

            if (DataSourceChanged != null)
            {
                var parameter = new KeyValuePair<ContactAddressProcessType, List<ContactAddressModel>>(ContactAddressProcessType.Edit, DataSource.ToList());
                DataSourceChanged(sender, new CommonEventArgs(parameter));
                ShowActionNoticeMessage(ActionType.SetAsPrimary);
            }

            Page.FocusElement(string.Format("{0}_lnkAddContactAddress", ClientID));
        }

        /// <summary>
        /// Bind action menu.
        /// </summary>
        /// <param name="eventArgs">GridView Row Event Arguments</param>
        private void BuildActionMenu(GridViewRowEventArgs eventArgs)
        {
            ContactAddressModel contactAddress = eventArgs.Row.DataItem as ContactAddressModel;

            ContactSessionParameter contactSessionParameter = AppSession.GetContactSessionParameter();
            PeopleModel4WS people = ContactUtil.GetPeopleModelFromContactSessionParameter(contactSessionParameter);

            if ((people != null
                    && (string.Equals(people.contractorPeopleStatus, ContractorPeopleStatus.Pending)
                        || string.Equals(people.contractorPeopleStatus, ContractorPeopleStatus.Rejected)))
                || (!ACAConstant.ContactSectionPosition.SpearForm.Equals(ContactSectionPosition) 
                    && !ACAConstant.ContactSectionPosition.AuthAgentCustomerDetail.Equals(ContactSectionPosition) 
                    && !StandardChoiceUtil.IsEnableContactAddressMaintenance())
                || (contactAddress.auditModel != null && ACAConstant.INVALID_STATUS.Equals(contactAddress.auditModel.auditStatus, StringComparison.InvariantCultureIgnoreCase))
                || ACAConstant.ContactSectionPosition.RegisterAccountConfirm.Equals(ContactSectionPosition)
                || ACAConstant.ContactSectionPosition.SpearFormCloseMatchConfirm.Equals(ContactSectionPosition)
                || ACAConstant.ContactSectionPosition.RegisterAccountComplete.Equals(ContactSectionPosition)
                || ACAConstant.ContactSectionPosition.RegisterClerkConfirm.Equals(ContactSectionPosition)
                || ACAConstant.ContactSectionPosition.RegisterClerkComplete.Equals(ContactSectionPosition)
                || (!IsEditable && ACAConstant.ContactSectionPosition.AuthAgentCustomerDetail.Equals(ContactSectionPosition) && !StandardChoiceUtil.IsCustomerDetailEditable())
                || !ContactTypePermission)
            {
                return;
            }

            ActionViewModel actionView;
            var actionList = new List<ActionViewModel>();
            AccelaButton lnkEdit = eventArgs.Row.FindControl("lnkEdit") as AccelaButton;
            AccelaButton lnkDelete = eventArgs.Row.FindControl("lnkDelete") as AccelaButton;
            AccelaButton lnkPrimary = eventArgs.Row.FindControl("lnkPrimary") as AccelaButton;
            AccelaButton lnkDeactivate = eventArgs.Row.FindControl("lnkDeactivate") as AccelaButton;
            PopupActions actionMenu = eventArgs.Row.FindControl("actionMenu") as PopupActions;
            string actionStyle = StandardChoiceUtil.GetPopActionItemStyle();
            
            //Edit link
            if (lnkEdit != null && !IsHideEditLink(contactAddress))
            {
                actionView = new ActionViewModel();
                actionView.ActionLabel = GetTextByKey("aca_contactaddresslist_label_edit");
                actionView.IcoUrl = ImageUtil.GetImageURL("popaction_edit.png");
                actionView.ActionId = actionMenu.ClientID + "_Edit";

                string clientScript = string.Format(
                                            "{0}('{1}', '{2}', '{3}', {4}); return false;",
                                            SessionParameterFunctionName,
                                            contactAddress.RowIndex,
                                            ContactAddressProcessType.Edit.ToString("D"),
                                            actionStyle.Equals(ACAConstant.POP_ACTION_ICO, StringComparison.CurrentCultureIgnoreCase) ? actionView.ActionId : actionMenu.ActionsLinkClientID,
                                            CallBack4CreateSessionFunction);

                actionView.ClientEvent = clientScript;
                actionList.Add(actionView);
            }

            //Delete link
            if (lnkDelete != null && !IsHideRemoveLink(contactAddress))
            {
                actionView = new ActionViewModel();
                actionView.ActionLabel = GetTextByKey("aca_contactaddresslist_label_delete");
                actionView.IcoUrl = ImageUtil.GetImageURL("popaction_delete.png");
                actionView.ActionId = actionMenu.ClientID + "_Remove";
                actionView.ClientEvent = string.Format("return RemoveContactAddress('{0}');", lnkDelete.UniqueID);
                actionList.Add(actionView);
            }

            /*
             * Deactivate link(addressID > -1 means the address is reference address)
             * And disallow to deactive a contact address which already used to replace other contact address.
             */
            if (!IsValidate
                && (IsEditable || (ContactSessionParameter != null && !ContactSessionParameter.Data.IsCloseMatch))
                && contactAddress.contactAddressPK != null
                && contactAddress.contactAddressPK.addressID > -1
                && contactAddress.replaceAddressID == null
                && !ValidationUtil.IsYes(contactAddress.primary)
                && ContactSectionPosition != ACAConstant.ContactSectionPosition.RegisterExistingAccount
                && StandardChoiceUtil.IsEnableContactAddressDeactivate())
            {
                actionView = new ActionViewModel();
                actionView.ActionLabel = GetTextByKey("aca_contactaddresslist_label_deactivate");
                actionView.IcoUrl = ImageUtil.GetImageURL("popaction_deactivate.png");
                actionView.ActionId = actionMenu.ClientID + "_Deactivate";

                actionView.ClientEvent = string.Format(
                                                       "return {0}_DeactivateContactAddress('{1}','{2}', '{3}');",
                                                       ClientID,
                                                       contactAddress.RowIndex.ToString(),
                                                       contactAddress.contactAddressPK.addressID.ToString(),
                                                       actionStyle.Equals(ACAConstant.POP_ACTION_ICO, StringComparison.CurrentCultureIgnoreCase) ? actionView.ActionId : actionMenu.ActionsLinkClientID);

                actionList.Add(actionView);
            }

            //Primary link
            if (lnkPrimary != null
                && !ValidationUtil.IsYes(contactAddress.primary)
                && ACAConstant.ContactSectionPosition.SpearForm.Equals(ContactSectionPosition))
            {
                actionView = new ActionViewModel();
                actionView.ActionLabel = GetTextByKey("aca_contactaddresslist_label_setasprimary");
                actionView.IcoUrl = ImageUtil.GetImageURL("popaction_set_as_primary.png");
                actionView.ActionId = actionMenu.ClientID + "_SetAsPrimary";
                actionView.ClientEvent = string.Format("return CallPostBackFunction('{0}');", lnkPrimary.UniqueID);
                actionList.Add(actionView);
            }

            if (actionList.Count > 0)
            {
                actionMenu.Visible = true;
                actionMenu.ActionLableKey = "aca_contactaddresslist_label_actionlink";
                actionMenu.AvailableActions = actionList.ToArray();
                actionMenu.BindListAction();
            }
        }

        /// <summary>
        /// Gets a value to indicating whether needs hide the delete link.
        /// </summary>
        /// <param name="contactAddress">contactAddress model</param>
        /// <returns>boolean value</returns>
        private bool IsHideRemoveLink(ContactAddressModel contactAddress)
        {
            bool isHidden = false;

            //Hide the delete link if contact address form is read only. ContactAddressPKModel.addressID > -1 means the address is reference address.
            if (IsForView
                || ValidationUtil.IsYes(contactAddress.primary)
                || (!IsEditable && ContactSessionParameter != null && ContactSessionParameter.Data.IsCloseMatch)
                || contactAddress.replaceAddressID != null
                || (((!ACAConstant.ContactSectionPosition.SpearForm.Equals(ContactSectionPosition) && !ACAConstant.ContactSectionPosition.RegisterExistingAccount.Equals(ContactSectionPosition)) || IsValidate)
                     && contactAddress.contactAddressPK != null
                     && contactAddress.contactAddressPK.addressID > -1))
            {
                isHidden = true;
            }

            return isHidden;
        }

        /// <summary>
        /// Gets a value to indicating whether needs to hide the edit link.
        /// </summary>
        /// <param name="contactAddress">Contact Address Model</param>
        /// <returns>boolean value</returns>
        private bool IsHideEditLink(ContactAddressModel contactAddress)
        {
            bool isHidden = false;

            //"addressID > -1" means the contact address is reference address.
            if ((IsValidate && contactAddress.contactAddressPK != null && contactAddress.contactAddressPK.addressID > -1)
                || IsForView
                || (!IsEditable && ContactSessionParameter != null && ContactSessionParameter.Data.IsCloseMatch)
                || !StandardChoiceUtil.IsEnableContactAddressEdit())
            {
                isHidden = true;
            }

            return isHidden;
        }

        /// <summary>
        /// Change the label key
        /// </summary>
        private void ChangeLabelKey()
        {
            if (gdvContactAddressList.GridViewNumber == GviewID.AuthAgentCustomerDetailCAList)
            {
                lblIncomplete.LabelKey = "per_authagent_contactlist_required_validate_msg";
                lblActionNoticeAddSuccess.LabelKey = "aca_authagent_contactaddress_msg_addsuccessfully";
                lblActionNoticeDeleteSuccess.LabelKey = "aca_authagent_contactaddress_msg_removesuccessfully";
                lblActionNoticeUpdateSuccess.LabelKey = "aca_authagent_contactaddress_msg_updatesuccessfully";
                lblActionDeactivatedSuccess.LabelKey = "aca_authagent_contactaddress_msg_deactivatedsuccessfully";
                lblDuplicateContactAddressMessage.LabelKey = "aca_authagent_contactaddress_msg_duplicate";
                lblDuplicateToSelf.LabelKey = "aca_authagent_contactaddress_msg_duplicatetoself";
                gdvContactAddressList.SummaryKey = "aca_authagent_contactaddresslist_summary";

                GridViewRow headerRow = GridViewBuildHelper.GetHeaderRow(gdvContactAddressList);

                ((IAccelaNonInputControl)headerRow.FindControl("lnkAddressType")).LabelKey = "aca_authagent_contactaddresslist_label_addresstype";
                ((IAccelaNonInputControl)headerRow.FindControl("lnkRecipient")).LabelKey = "aca_authagent_contactaddresslist_label_recipient";
                ((IAccelaNonInputControl)headerRow.FindControl("lnkAddress")).LabelKey = "aca_authagent_contactaddresslist_label_address";
                ((IAccelaNonInputControl)headerRow.FindControl("lnkStartDate")).LabelKey = "aca_authagent_contactaddresslist_label_startdate";
                ((IAccelaNonInputControl)headerRow.FindControl("lnkEndDate")).LabelKey = "aca_authagent_contactaddresslist_label_enddate";
                ((IAccelaNonInputControl)headerRow.FindControl("lnkPhone")).LabelKey = "aca_authagent_contactaddresslist_label_phone";
                ((IAccelaNonInputControl)headerRow.FindControl("lnkFax")).LabelKey = "aca_authagent_contactaddresslist_label_fax";
                ((IAccelaNonInputControl)headerRow.FindControl("lnkActionHeader")).LabelKey = "aca_authagent_contactaddresslist_label_action";
                ((IAccelaNonInputControl)headerRow.FindControl("lnkStatusHeader")).LabelKey = "aca_authagent_contactaddresslist_label_status";
            }
        }

        #endregion Private Methods
    }
}