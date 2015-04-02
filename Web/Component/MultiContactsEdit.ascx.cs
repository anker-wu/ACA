/**
 * <pre>
 * 
 *  Accela Citizen Access
 *  File: MultiContactsEdit.ascx.cs
 * 
 *  Accela, Inc.
 *  Copyright (C): 2009-2014
 * 
 *  Description:
 * 
 *  Notes:
 *      $Id: MultiContactsEdit.ascx.cs 278740 2014-09-12 07:25:54Z ACHIEVO\james.shi $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.UI.WebControls;

using Accela.ACA.BLL.Common;
using Accela.ACA.BLL.People;
using Accela.ACA.Common;
using Accela.ACA.Common.Log;
using Accela.ACA.Common.Util;
using Accela.ACA.Web.Common;
using Accela.ACA.Web.People;
using Accela.ACA.WSProxy;
using Accela.Web.Controls;
using log4net;

namespace Accela.ACA.Web.Component
{
    /// <summary>
    /// Multiple contacts edit form contains Contacts edit form and Contacts list form.
    /// </summary>
    public partial class MultiContactsEdit : BaseUserControl
    {
        #region member variable

        /// <summary>
        /// Loggers object.
        /// </summary>
        private static readonly ILog Logger = LogFactory.Instance.GetLogger(typeof(MultiContactsEdit));

        /// <summary>
        /// indicate the contact form is editable or not.
        /// </summary>
        private bool _isEditable;

        /// <summary>
        /// R-Reference,need reference data
        /// T-Transactional, user input data.
        /// N-No Limitation,both reference and daily
        /// </summary>
        private string _validateFlag = string.Empty;

        /// <summary>
        /// the validate flag.
        /// </summary>
        private bool _validate;

        /// <summary>
        /// indicate the contact form is in confirm page or not.
        /// </summary>
        private bool _isInConfirmPage;

        /// <summary>
        /// The _component name
        /// </summary>
        private string _componentName = string.Empty;

        /// <summary>
        /// The component ID
        /// </summary>
        private int _componentID = 0;

        /// <summary>
        /// The current contact type
        /// </summary>
        private string _contactType = string.Empty;

        #endregion

        #region Events

        /// <summary>
        /// Add contact event by clicking Save button
        /// </summary>
        public event CommonEventHandler ContactsChanged;

        #endregion

        #region property

        /// <summary>
        /// Gets or sets the type of the component.
        /// </summary>
        /// <value>
        /// The type of the component.
        /// </value>
        public int ComponentID
        {
            get
            {
                return _componentID;
            }

            set
            {
                _componentID = value;
                ucContactList.ComponentID = value;
                contactEdit.ComponentID = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is validate.
        /// </summary>
        /// <value><c>true</c> if this instance is validate; otherwise, <c>false</c>.</value>
        public bool IsValidate
        {
            get
            {
                return _validate;
            }

            set
            {
                _validate = value;
                ucContactList.IsValidate = _validate;
                contactEdit.IsValidate = _validate;
            }
        }

        /// <summary>
        /// Gets or sets the data comes from.
        /// </summary>
        /// <value>The validate flag.</value>
        public string ValidateFlag
        {
            get
            {
                return _validateFlag;
            }

            set
            {
                _validateFlag = value;
                contactEdit.ValidateFlag = _validateFlag;
                ucContactList.ValidateFlag = _validateFlag;

                // it need set IsValidate or not, NOT only set true when data source is reference.
                IsValidate = ComponentDataSource.Reference.Equals(value);
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether edit the form.
        /// </summary>
        public bool IsEditable
        {
            get
            {
                return _isEditable;
            }

            set
            {
                _isEditable = value;
                contactEdit.IsEditable = _isEditable;
                ucContactList.IsEditable = _isEditable;
            }
        }

        /// <summary>
        /// Gets Explore Contact Edit Control
        /// </summary>
        public ContactEdit ContactEdit
        {
            get
            {
                return this.contactEdit;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this control is displayed in the confirm page.
        /// </summary>
        public bool IsInConfirmPage
        {
            get
            {
                return _isInConfirmPage;
            }

            set
            {
                _isInConfirmPage = value;

                if (_isInConfirmPage)
                {
                    ucContactList.EnableDeleteAction = false;
                }

                ucContactList.IsInCapConfirm = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether no need to scroll to list control.
        /// </summary>
        public bool IsNeedScrollToList
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets component name.
        /// </summary>
        public string ComponentName
        {
            get
            {
                return _componentName;
            }

            set
            {
                _componentName = value;
                contactEdit.ComponentName = value;
                ucContactList.ComponentName = value;
                contactTypeIndicator.ComponentName = value;
            }
        }

        /// <summary>
        /// Gets or sets the current contact type.
        /// </summary>
        public string ContactType
        {
            get
            {
                return _contactType;
            }

            set
            {
                _contactType = value;
            }
        }

        /// <summary>
        /// Gets the data source of the license list.
        /// </summary>
        public DataTable DataSource
        {
            get
            {
                return ucContactList.DataSource;
            }
        }

        /// <summary>
        /// Gets the function create contact session to open the edit form.
        /// </summary>
        public string CreateContactSessionFunction
        {
            get
            {
                return ucContactList.CreateContactSessionFunction;
            }
        }

        /// <summary>
        /// Gets edit contact function name
        /// </summary>
        public string EditContactFunction
        {
            get
            {
                return ucContactList.EditContactFunction;
            }
        }

        #endregion

        #region public method

        /// <summary>
        /// set the section required property
        /// </summary>
        /// <param name="isRequired">indicate if the section is required</param>
        public void SetSectionRequired(bool isRequired)
        {
            ucContactList.SetGridViewRequired(isRequired);
        }

        /// <summary>
        /// Display all contacts in the current cap
        /// </summary>
        /// <param name="contacts">CapContactModel4WS object</param>
        public void DisplayContacts(CapContactModel4WS[] contacts)
        {
            InitContactList(contacts);
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

        #endregion

        #region protected method

        /// <summary>
        /// Display add from saved notice.
        /// </summary>
        /// <param name="isSuccess">True when adding record successfully; false otherwise.</param>
        /// <param name="msg">The message.</param>
        public void DisplayAddSavedNotice(bool isSuccess, string msg)
        {
            if (isSuccess)
            {
                this.lblActionNoticeAddSuccess.Text = msg;
                this.lblActionNoticeAddSuccess.Visible = true;

                if (AccessibilityUtil.AccessibilityEnabled)
                {
                    MessageUtil.ShowAlertMessage(lblActionNoticeAddSuccess, msg);
                }

                divImgSuccess.Visible = true;
            }
            else
            {
                this.lblActionNoticeAddFailed.Text = msg;
                this.lblActionNoticeAddFailed.Visible = true;

                if (AccessibilityUtil.AccessibilityEnabled)
                {
                    MessageUtil.ShowAlertMessage(lblActionNoticeAddFailed, msg);
                }

                divImgFailed.Visible = true;
            }

            divActionNotice.Visible = true;
            MultiContactPanel.Update();
        }

        /// <summary>
        /// Refresh the contact list.
        /// </summary>
        /// <param name="isFromContactEdit">Is from contact edit page.</param>
        public void RefreshContactList(bool isFromContactEdit = true)
        {
            ContactSessionParameter contactSessionParameter = AppSession.GetContactSessionParameter();
            CapContactModel4WS contact = null;

            if (contactSessionParameter != null)
            {
                contact = ObjectCloneUtil.DeepCopy(contactSessionParameter.Data.DataObject as CapContactModel4WS);
            }

            if (contact == null)
            {
                return;
            }

            if (!isFromContactEdit)
            {
                hdnIsClearRefContact.Value = ACAConstant.COMMON_N;
            }

            CapContactModel4WS tempContact = new CapContactModel4WS();
            tempContact.people = new PeopleModel4WS();
            tempContact.people.contactType = contact.people.contactType;
            contactSessionParameter.Data.DataObject = tempContact;
            AppSession.SetContactSessionParameter(contactSessionParameter);
            
            //Check the added contact record in Contact List whether it is exceed the Max Num setting in Contact Type Option or not.
            if (!ValidateContactTypeExceededMaxNum(contact, ucContactList.DataSource))
            {
                MessageUtil.ShowMessageByControl(
                    Page,
                    MessageType.Error,
                    LabelUtil.GetTextByKey("aca_contactlist_message_exceededmaxnum", ModuleName));

                contactEdit.IsNeedFocusElement = false;
                MultiContactPanel.Update();
                return;
            }

            bool isUpdateAction = ucContactList.Save(contact);
            ucContactList.BindContactList();
            ucContactList.RowIndex = ContactList.NO_CONTACT_SELECTED;

            try
            {
                RaiseContactChangeEvent(btnRefreshContactList, new CommonEventArgs(contact));

                contactEdit.ShowActionNoticeMessage(isUpdateAction ? ActionType.UpdateSuccess : ActionType.AddSuccess);
            }
            catch (ACAException ex)
            {
                Logger.Error(ex);
                contactEdit.ShowActionNoticeMessage(isUpdateAction ? ActionType.UpdateFailed : ActionType.AddFailed);
            }

            contactEdit.Update();
            MultiContactPanel.Update();

            if (isFromContactEdit)
            {
                Page.FocusElement(Request.Form["__LASTFOCUS_ID"]);
            }
        }

        /// <summary>
        /// Raises the initial event
        /// </summary>
        /// <param name="e">A System.EventArgs object containing the event data.</param>
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            ucContactList.CreateContactSessionFunction = contactEdit.CreateContactSessionFunction;
            ucContactList.EditContactFunction = contactEdit.EditContactFunction;
        }

        /// <summary>
        /// Raises the page load event
        /// </summary>
        /// <param name="sender">An object that contains the event sender.</param>
        /// <param name="e">A System.EventArgs object containing the event data.</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            ucContactList.ContactsDeleted += new CommonEventHandler(ContactList_ContactsDeleted);
            ucContactList.GridViewSort += new GridViewSortedEventHandler(ContactList_GridViewSort);
            ucContactList.PageIndexChanging += new GridViewPageEventHandler(ContactList_PageIndexChanging);
            ucContactList.ParentID = contactEdit.ClientID;
            contactEdit.IsMultipleContact = true;
            IsNeedScrollToList = true;

            if (!AppSession.IsAdmin)
            {
                HideActionNoticeMessage();

                if (IsInConfirmPage)
                {
                    contactEdit.Visible = false;
                }
            }
            else
            {
                divActionNotice.Visible = true;
                lblActionNoticeAddFailed.Visible = true;
                lblActionNoticeAddSuccess.Visible = true;
                lblActionNoticeDeleteFailed.Visible = true;
                lblActionNoticeDeleteSuccess.Visible = true;
                lblActionNoticeEditFailed.Visible = true;
                lblActionNoticeEditSuccess.Visible = true;
            }
        }

        /// <summary>
        /// Refresh the contact list.
        /// </summary>
        /// <param name="sender">An object that contains the event sender.</param>
        /// <param name="e">An System.EventArgs object that contains the event data.</param>
        protected void RefreshContactListButton_Click(object sender, EventArgs e)
        {
            RefreshContactList();
        }

        /// <summary>
        /// Raises the PreRender event.
        /// </summary>
        /// <param name="e">An EventArgs object that contains the event data.</param>
        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);

            if (!AppSession.IsAdmin)
            {
                CapContactModel4WS[] contacts = GetContactsFromContactList();
                string errorMsg = string.Empty;

                if (IsValidate && contacts != null)
                {
                    errorMsg = ContactUtil.IsValidContact(contacts, ModuleName);
                }

                if (!string.IsNullOrEmpty(errorMsg))
                {
                    ShowValidateErrorMessage(errorMsg);

                    //if the Contact validate failed then should return to prevent the required validate message overwrite.
                    return;
                }

                ShowRequiredMessage(contacts);
            }
        }

        #endregion

        #region private method

        /// <summary>
        /// in page index event, keep control display status
        /// </summary>
        /// <param name="sender">An object that contains the event sender.</param>
        /// <param name="e">EventArgs e</param>
        private void ContactList_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
        }

        /// <summary>
        /// Handle ContactDelete event;delete a contact
        /// </summary>
        /// <param name="sender">An object that contains the event sender.</param>
        /// <param name="arg">A CommonEventArgs object containing the contact model.</param>
        private void ContactList_ContactsDeleted(object sender, CommonEventArgs arg)
        {
            if (ContactsChanged != null)
            {
                try
                {
                    RaiseContactChangeEvent(sender, arg);
                    contactEdit.ShowActionNoticeMessage(ActionType.DeleteSuccess);
                }
                catch (ACAException ex)
                {
                    Logger.Error(ex);
                    contactEdit.ShowActionNoticeMessage(ActionType.DeleteFailed);
                }
            }

            ucContactList.RowIndex = ContactList.NO_CONTACT_SELECTED;
            contactEdit.Update();
            MultiContactPanel.Update();
        }

        /// <summary>
        /// Handles the GridViewSort event of the contactList control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">A CommonEventArgs object.</param>
        private void ContactList_GridViewSort(object sender, EventArgs e)
        {
            MultiContactPanel.Update();
        }

        /// <summary>
        /// Validate Contact type.
        /// </summary>
        /// <param name="contact">A <see cref="CapContactModel4WS"/> object.</param>
        /// <param name="contactsGroup">Contact list.</param>
        /// <returns>true or false</returns>
        private bool ValidateContactTypeExceededMaxNum(CapContactModel4WS contact, DataTable contactsGroup)
        {
            string contactKey = contact.people.contactType;
            bool checkingRuleIsValidated = true;

            if (contactsGroup != null && contactsGroup.Rows.Count > 0)
            {
                PageFlowGroupModel pageFlow = AppSession.GetPageflowGroupFromSession();
                string agencyCode = pageFlow != null ? pageFlow.serviceProviderCode : ConfigManager.AgencyCode;
                string pageFlowGroupName = pageFlow != null ? pageFlow.pageFlowGrpCode : null;

                XEntityPermissionModel xentity = new XEntityPermissionModel();
                xentity.servProvCode = agencyCode;
                xentity.entityType = XEntityPermissionConstant.CONTACT_DATA_VALIDATION;
                xentity.entityId = ModuleName;
                xentity.componentName = ComponentName;
                xentity.entityId2 = pageFlowGroupName;

                List<ContactTypeUIModel> contactTypeUIModels = DropDownListBindUtil.GetContactTypesByXEntity(xentity, true);
                List<string> contactTypeList = new List<string>();

                IEnumerable<ContactTypeUIModel> contactTypeUIEnum = contactTypeUIModels.Where(c => c.Key == contactKey);

                if (contactTypeUIEnum != null && contactTypeUIEnum.Count() > 0
                    && ValidationUtil.IsNumber(contactTypeUIEnum.Single().MaxNum))
                {
                    int maxNumSettingAtAdmin = int.Parse(contactTypeUIEnum.Single().MaxNum);

                    foreach (DataRow drContact in contactsGroup.Rows)
                    {
                        contactTypeList.Add(drContact[ColumnConstant.Contact.ContactType.ToString()].ToString());
                    }

                    IEnumerable<string> capContactEnu = contactTypeList.Where(c => c == contactKey);
                    int addedContactTypesNum = 0;

                    if (capContactEnu != null && capContactEnu.Count() > 0)
                    {
                        addedContactTypesNum = capContactEnu.Count();
                    }

                    // rowIndex = NO_CONTACT_SELECTED is add contact, rowIndex != NO_CONTACT_SELECTED is update contact.
                    int rowIndex = ucContactList.RowIndex;
                    if (rowIndex != ContactList.NO_CONTACT_SELECTED)
                    {
                        //1.update contact's contact type.
                        //2.update contact's other information.
                        int pos = ListUtil.FindPos(rowIndex, ucContactList.DataSource, ColumnConstant.Contact.RowIndex.ToString());
                        bool isUpdateToSelf = contactsGroup.Rows[pos][ColumnConstant.Contact.ContactType.ToString()].ToString() == contactKey;

                        if (isUpdateToSelf && addedContactTypesNum > maxNumSettingAtAdmin)
                        {
                            checkingRuleIsValidated = false;
                        }
                        else if (!isUpdateToSelf && addedContactTypesNum == maxNumSettingAtAdmin)
                        {
                            checkingRuleIsValidated = false;
                        }
                    }
                    else if (addedContactTypesNum >= maxNumSettingAtAdmin)
                    {
                        checkingRuleIsValidated = false;
                    }
                }
            }

            return checkingRuleIsValidated;
        }

        /// <summary>
        /// Handle ContactChanged event
        /// </summary>
        /// <param name="sender">An object that contains the event sender.</param>
        /// <param name="arg">A CommonEventArgs object containing the contact model.</param>
        private void RaiseContactChangeEvent(object sender, CommonEventArgs arg)
        {
            if (ContactsChanged != null && ucContactList.DataSource != null)
            {
                CapContactModel4WS[] contacts = GetContactsFromContactList();
                CapContactModel4WS capContact = null;

                if (arg != null)
                {
                    capContact = arg.ArgObject as CapContactModel4WS;
                }

                if (contacts != null && contacts.Length > 0)
                {
                    ContactsChanged(sender, new CommonEventArgs(new object[] { contacts, capContact, hdnIsClearRefContact.Value }));
                }
                else
                {
                    // all data has removed from ContactList
                    ContactsChanged(sender, new CommonEventArgs(new object[] { ComponentName, capContact, hdnIsClearRefContact.Value }));
                }

                ShowRequiredMessage(contacts);
            }
        }

        /// <summary>
        /// Get an array of CapContactModel4WS from the contactList control
        /// </summary>
        /// <returns>an array of CapContactModel4WS</returns>
        private CapContactModel4WS[] GetContactsFromContactList()
        {
            if (ucContactList.DataSource == null || (ucContactList.DataSource as DataTable).Rows.Count == 0)
            {
                return null;
            }

            DataTable dtContact = ucContactList.DataSource as DataTable;
            ArrayList contacts = new ArrayList();

            foreach (DataRow drContact in dtContact.Rows)
            {
                if (drContact[ColumnConstant.Contact.CapContactModel.ToString()] != DBNull.Value)
                {
                    CapContactModel4WS contact = drContact[ColumnConstant.Contact.CapContactModel.ToString()] as CapContactModel4WS;
                    contacts.Add(contact);
                }
            }

            return (CapContactModel4WS[])contacts.ToArray(typeof(CapContactModel4WS));
        }

        /// <summary>
        /// Show required message when multiple contact list exist no value of required field. 
        /// </summary>
        /// <param name="contacts">CapContactModel4WS models</param>
        private void ShowRequiredMessage(CapContactModel4WS[] contacts)
        {
            string errorMsg = string.Empty;
            string fieldRequiredAndFormatMsg = GetTextByKey("per_contactlist_required_validate_msg"); 

            if (!IsEditable) 
            {
                if (!CapUtil.ValidateTemplateFields(IsValidate, IsEditable, contacts))
                {
                     errorMsg = fieldRequiredAndFormatMsg;
                }
            }
            else
            {
                bool isContactDataSourceNoLimitation = ComponentDataSource.NoLimitation.Equals(ValidateFlag, StringComparison.OrdinalIgnoreCase);
                if (contacts != null)
                {
                    if ((string.IsNullOrEmpty(errorMsg)
                             && CapUtil.IsNeedValidateContacts(IsValidate, IsEditable, contacts)
                             && (!RequiredValidationUtil.ValidateFields4ContactList(ModuleName, GviewID.ContactEdit, contacts, contactEdit.ContactSectionPosition)
                                || !FormatValidationUtil.ValidateFormat4ContactList(ModuleName, GviewID.ContactEdit, contacts, isContactDataSourceNoLimitation, contactEdit.ContactSectionPosition)))
                         || (StandardChoiceUtil.IsEnableContactAddress() 
                             && (!RequiredValidationUtil.ValidateCAPrimary(IsEditable, contactEdit.ContactSectionPosition, contacts)
                                 || !RequiredValidationUtil.ValidateContactListAddressType(IsEditable, contactEdit.ContactSectionPosition, contacts))))
                    {
                        errorMsg = fieldRequiredAndFormatMsg;
                    }
                }
            }

            ShowValidateErrorMessage(errorMsg);

            if (AccessibilityUtil.AccessibilityEnabled)
            {
                MessageUtil.ShowAlertMessage(errorMessageLabel, errorMsg);
            }
        }

        /// <summary>
        /// Initializes contact list GridView.
        /// </summary>
        /// <param name="contacts">Cap Contact model</param>
        private void InitContactList(CapContactModel4WS[] contacts)
        {
            IPeopleBll peopleBLL = ObjectFactory.GetObject<IPeopleBll>();
            DataTable dtContact = null;

            if (!AppSession.IsAdmin)
            {
                dtContact = peopleBLL.ConvertContactListToDataTable(contacts);
            }

            ucContactList.DataSource = dtContact;
            ucContactList.BindContactList();
        }

        /// <summary>
        /// Hide action notice message.
        /// </summary>
        private void HideActionNoticeMessage()
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
        }

        #endregion
    }
}
