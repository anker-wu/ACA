#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: ContactLookUp.aspx.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2013-2014
 *
 *  Description:
 *
 *  Notes:
 * $Id: ContactLookUp.aspx.cs 151830 2013-10-09 13:39:43Z ACHIEVO\blues.gao $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web.Script.Serialization;
using System.Web.UI;
using System.Web.UI.WebControls;
using Accela.ACA.BLL.Account;
using Accela.ACA.BLL.APO;
using Accela.ACA.BLL.Common;
using Accela.ACA.BLL.People;
using Accela.ACA.Common;
using Accela.ACA.Common.Util;
using Accela.ACA.Web.Common;
using Accela.ACA.WSProxy;
using Accela.Web.Controls;

namespace Accela.ACA.Web.People
{
    /// <summary>
    /// the class of Contact look up
    /// </summary>
    public partial class ContactLookUp : PeoplePopupBasePage
    {
        #region Fields

        /// <summary>
        /// The label key of page title for Look Up
        /// </summary>
        private const string LABEL_KEY_CONTACTLOOKUP_TITLE = "aca_contactlookup_label_title";

        /// <summary>
        /// The label key of page title for Select From Account in the page of the module level.
        /// </summary>
        private const string LABEL_KEY_SELECTFROMACCOUNT_TITLE = "aca_contactlookup_label_selectfromaccount_title";

        /// <summary>
        /// The label key of page title for Select From Account in registration account page.
        /// </summary>
        private const string LABEL_KEY_REGISTRATION_SELECTFROMACCOUNT_TITLE = "aca_registration_contactlookup_label_selectfromaccount_title";
        
        /// <summary>
        /// Select your contact information
        /// </summary>
        private const string LABEL_KEY_REGISTRATION_SELECTCONTACT_TITLE = "aca_existing_account_registeration_label_select_title";

        /// <summary>
        /// OK button
        /// </summary>
        private const string LABEL_KEY_SELECT_CONTACT_INFO_LABEL_OK = "aca_select_contactinfo_label_btnok";

        /// <summary>
        /// Cancel button
        /// </summary>
        private const string LABEL_KEY_SELECT_CONTACT_INFO_LABEL_CANCEL = "aca_select_contactinfo_label_btncancel";

        #endregion Fields

        #region Properties

        /// <summary>
        /// Gets or sets the selected people
        /// </summary>
        private PeopleModel SelectedPeople
        {
            get
            {
                if (ViewState["SelectedPeople"] != null)
                {
                    return ViewState["SelectedPeople"] as PeopleModel;
                }

                return null;
            }

            set
            {
                ViewState["SelectedPeople"] = value;
            }
        }

        /// <summary>
        /// Gets the all steps of the search page
        /// (Step1:search form;step2:contact list form;3:contact address list form;4:contact edit form)
        /// </summary>
        private List<ContactLookUpStep> AllSteps
        {
            get
            {
                List<ContactLookUpStep> allSteps = new List<ContactLookUpStep>();

                if (ContactSearchType == ContactProcessType.Lookup)
                {
                    allSteps.Add(ContactLookUpStep.SearchForm);
                }

                allSteps.Add(ContactLookUpStep.ContactList);
                allSteps.Add(ContactLookUpStep.CotactAddressLookUp);

                return allSteps;
            }
        }

        /// <summary>
        /// Gets or sets the current step
        /// </summary>
        private int CurrentStepIndex
        {
            get
            {
                if (ViewState["CurrentStepIndex"] != null)
                {
                    return (int)ViewState["CurrentStepIndex"];
                }

                return 0;
            }

            set
            {
                ViewState["CurrentStepIndex"] = value;
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

        #endregion Properties

        #region Methods

        /// <summary>
        /// On Initial event
        /// </summary>
        /// <param name="e">event arguments</param>
        protected override void OnInit(EventArgs e)
        {
            ucContactInfo.ParentID = ParentID;
            ucContactInfo.ContactSectionPosition = ContactSectionPosition;

            // reset the view id as it set in ucContactInfo.ContactSectionPosition incorrect.
            ucContactInfo.SetViewId(PeopleUtil.GetContactSearchGviewID());
            ucContactInfo.ContactExpressionType = ContactExpressionType;
            ucContactInfo.IsMultipleContact = IsMultipleContact;

            contactSearchList.ContactSectionPosition = ContactSectionPosition;
            contactSearchList.ContactSearchType = ContactSearchType;
            IsLoginUseExistingAccount = ValidationUtil.IsYes(Request.QueryString[UrlConstant.IS_LOGIN_USE_EXISTING_ACCOUNT]);

            /* Use a separate label in Registeration page since there only support "Select from Professional".*/
            if (ACAConstant.ContactSectionPosition.RegisterAccount.Equals(ContactSectionPosition))
            {
                lblContactListHints.LabelKey = "aca_contactlookup_label_contactlisthintsprofessional";
            }
            else if (ACAConstant.ContactSectionPosition.RegisterExistingAccount.Equals(ContactSectionPosition))
            {
                lblContactListHints.LabelKey = "aca_existing_account_registeration_label_select_info";
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

            // Initialize the page's title
            InitTitle();
           
            contactAddressSearchList.ContactSectionPosition = ContactSessionParameterModel.ContactSectionPosition;

            if (AppSession.IsAdmin)
            {
                contactAddressSearchList.LoadAddressList(null);
            }

            if (!IsPostBack)
            {
                ContinueToNextStep(0);
            }

            if (ContactSectionPosition == ACAConstant.ContactSectionPosition.RegisterAccount || ContactSectionPosition == ACAConstant.ContactSectionPosition.RegisterExistingAccount)
            {
                ddlContactType.AutoPostBack = false;
            }
        }

        /// <summary>
        /// Search Click
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void SearchButton_Click(object sender, EventArgs e)
        {
            if (!ucContactInfo.ValidateSearchCondition())
            {
                string message = GetTextByKey("per_permitList_SearchCriteria_Required");
                MessageUtil.ShowMessageInPopup(Page, MessageType.Error, message);
                ScriptManager.RegisterClientScriptBlock(Page, GetType(), "GotoMessageBar", "scrollIntoView('messageSpan');", true);
                return;
            }

            contactSearchList.RefDataSource = null;
            contactSearchList.HideCondition();
            GetLookUpDataSource(0, null);
            SelectedPeople = null;
            ContinueToNextStep(1);
        }

        /// <summary>
        /// Clear button click event.
        /// </summary>
        /// <param name="sender">An object that contains the event sender.</param>
        /// <param name="e">A System.EventArgs object containing the event data.</param>
        protected void ClearButton_Click(object sender, EventArgs e)
        {
            ucContactInfo.ResetContactForm(true);
            Page.FocusElement(btnClear.ClientID);
        }

        /// <summary>
        /// Click Continue button event handler. 
        /// Save the contact information to session and go to the contact address information add page.
        /// </summary>
        /// <param name="sender">An object that contains the event sender.</param>
        /// <param name="e">A System.EventArgs object containing the event data.</param>
        protected void ContinueContactButton_Click(object sender, EventArgs e)
        {
            SelectedPeople = contactSearchList.SelectedItem;

            if (SelectedPeople == null)
            {
                string message = GetTextByKey("per_permitList_SearchCriteria_Required");
                MessageUtil.ShowMessageInPopup(Page, MessageType.Error, message);
                return;
            }

            SaveContact(SelectedPeople);
        }

        /// <summary>
        /// Click Continue button event handler. 
        /// Save the contact information to session and go to the contact address information add page.
        /// </summary>
        /// <param name="sender">An object that contains the event sender.</param>
        /// <param name="e">A System.EventArgs object containing the event data.</param>
        protected void ContinueContactAddressButton_Click(object sender, EventArgs e)
        {
            if (SelectedPeople == null)
            {
                return;
            }

            BuildDispalyPeopleData();
        }

        /// <summary>
        /// Click Continue button event handler. 
        /// Save the contact information to session and go to the contact address information add page.
        /// </summary>
        /// <param name="sender">An object that contains the event sender.</param>
        /// <param name="e">A System.EventArgs object containing the event data.</param>
        protected void BackButton_Click(object sender, EventArgs e)
        {
            ContinueToNextStep(-1);
        }

        /// <summary>
        /// Handle the contact type dropdownlist selected changed event to load the contact address
        /// </summary>
        /// <param name="sender">the sender object</param>
        /// <param name="e">the event arguments</param>
        protected void DdlContactType_SelectedIndexChanged(object sender, EventArgs e)
        {
            contactAddressSearchList.IsNeedRemoveSelectedItem = true;
            InitContactAddressInfo();
            Page.FocusElement(ddlContactType.ClientID);
        }

        /// <summary>
        /// in page index event, keep control display status
        /// </summary>
        /// <param name="sender">An object that contains the event sender.</param>
        /// <param name="e">EventArgs e</param>
        protected void ContactList_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            contactSearchList.HideCondition();
            PaginationModel pageInfo = PaginationUtil.GetPageInfoByID(contactSearchList.ClientID);

            if (e.NewPageIndex > pageInfo.EndPage)
            {
                if (ContactSearchType == ContactProcessType.Lookup)
                {
                    GetLookUpDataSource(e.NewPageIndex, pageInfo.SortExpression);
                }
                else if (ContactSearchType == ContactProcessType.SelectContactFromAccount && StandardChoiceUtil.IsExternalParcelSource())
                {
                    BindAccountAssociatedDataSource(e.NewPageIndex, pageInfo.SortExpression, true);
                }

                BindContactList();
            }
        }

        /// <summary>
        /// Format search contact name
        /// </summary>
        /// <param name="license">License Model</param>
        /// <returns>display name</returns>
        private static string FormatSearchLicenseName(LicenseModel4WS license)
        {
            if (license == null)
            {
                return string.Empty;
            }

            string resLicenseType = I18nStringUtil.GetString(license.resLicenseType, license.licenseType);

            return
                DataUtil.ConcatStringWithSplitChar(
                    new string[]
                    {
                        license.businessName,
                        license.contactFirstName,
                        license.contactMiddleName,
                        license.contactLastName,
                        resLicenseType,
                        license.stateLicense
                    },
                    ACAConstant.BLANK);
        }

        /// <summary>
        /// Build fill contact edit form people data.
        /// </summary>
        private void BuildDispalyPeopleData()
        {
            string contactType = string.Empty;

            if (IsMultipleContact || ContactSectionPosition == ACAConstant.ContactSectionPosition.RegisterAccount
                || ContactSectionPosition == ACAConstant.ContactSectionPosition.RegisterExistingAccount)
            {
                contactType = string.IsNullOrEmpty(ddlContactType.GetValue()) ? SelectedPeople.contactType : ddlContactType.GetValue();
                ContactSessionParameterModel.ContactType = contactType;
            }
            else
            {
                contactType = ContactType;
            }

            if (!IsLoginUseExistingAccount)
            {
                SelectedPeople.contactAddressLists = contactAddressSearchList.SelectedItems == null ? null : contactAddressSearchList.SelectedItems.ToArray();
            }
            else if (SelectedPeople.contactAddressLists != null && SelectedPeople.contactAddressLists.Length > 0)
            {
                List<ContactAddressModel> validAddresses = new List<ContactAddressModel>();

                foreach (var contactAddress in SelectedPeople.contactAddressLists)
                {
                    if (ACAConstant.VALID_STATUS.Equals(contactAddress.auditModel.auditStatus, StringComparison.OrdinalIgnoreCase))
                    {
                        validAddresses.Add(contactAddress);
                    }
                }

                if (validAddresses.Count > 0)
                {
                    SelectedPeople.contactAddressLists = validAddresses.ToArray();
                }
                else
                {
                    SelectedPeople.contactAddressLists = null;
                }
            }

            SaveContactAddress(SelectedPeople, contactType);
        }

        /// <summary>
        /// Initialize the page's title.
        /// </summary>
        private void InitTitle()
        {
            string gviewId = PeopleUtil.GetContactSearchGviewID();
            ContactProcessType processType;

            if (AppSession.IsAdmin)
            {
                lblContactTitle.Visible = true;
                processType = ContactSearchType;

                if (processType == ContactProcessType.SelectContactFromAccount || processType == ContactProcessType.SelectContactFromOtherAgencies)
                {
                    lblContactTitle.LabelType = LabelType.SectionTitle;
                }
            }
            else
            {
                ContactSessionParameter sessionParameter = AppSession.GetContactSessionParameter();
                processType = sessionParameter.Process.ContactProcessType;
            }

            ChangeLabelKeys(processType);
            lblContactTitle.SectionID = ModuleName + ACAConstant.SPLIT_CHAR + gviewId + ACAConstant.SPLIT_CHAR + ucContactInfo.ClientID + "_";

            if (!AppSession.IsAdmin)
            {
                SetPageTitleKey(lblContactTitle.LabelKey);
            }
        }

        /// <summary>
        /// Sets the template attributes value by contact type.
        /// </summary>
        /// <param name="people">The people.</param>
        /// <param name="contactType">Type of the contact.</param>
        private void SetTemplateAttributesValueByContactType(PeopleModel people, string contactType)
        {
            if (people == null || string.IsNullOrEmpty(contactType) || string.Equals(people.contactType, contactType, StringComparison.InvariantCultureIgnoreCase)
                || people.attributes == null || people.attributes.Length == 0)
            {
                return;
            }

            // convert PeopleModel with value to dictionary
            Dictionary<string, string> dict = new Dictionary<string, string>();

            foreach (object attribute in people.attributes)
            {
                TemplateAttributeModel model = attribute as TemplateAttributeModel;

                if (model != null)
                {
                    dict.Add(string.Format("{0}_{1}", model.attributeName, model.attributeValueDataType), model.attributeValue);
                }
            }

            // assign the value to the people template attribute associate with the contact type
            ITemplateBll templateBll = (ITemplateBll)ObjectFactory.GetObject(typeof(ITemplateBll));
            TemplateAttributeModel[] attributes = templateBll.GetPeopleTemplateAttributes(contactType, ConfigManager.AgencyCode, AppSession.User.PublicUserId);

            if (attributes != null && attributes.Length > 0)
            {
                foreach (TemplateAttributeModel model in attributes)
                {
                    string key = string.Format("{0}_{1}", model.attributeName, model.attributeValueDataType);

                    if (dict.ContainsKey(key))
                    {
                        model.attributeValue = dict[key];
                    }
                }
            }

            // assign to people attributes with the value that assigned.
            people.attributes = attributes;
        }

        /// <summary>
        /// Continue to next step
        /// </summary>
        /// <param name="steps">is continue to next step.</param>
        private void ContinueToNextStep(int steps)
        {
            MessageUtil.HideMessageByControl(Page);
            SetDialogFixedWidth("800");

            divContactLookUpCriteria.Visible = false;
            divContactList.Visible = false;
            divContactAddressList.Visible = false;
            List<ContactLookUpStep> curSteps = new List<ContactLookUpStep>();

            if (AppSession.IsAdmin)
            {
                curSteps.AddRange(AllSteps);
            }
            else
            {
                CurrentStepIndex = CurrentStepIndex + steps;
                curSteps.Add(AllSteps[CurrentStepIndex]);
            }

            foreach (ContactLookUpStep step in curSteps)
            {
                switch (step)
                {
                    case ContactLookUpStep.SearchForm:
                        divContactLookUpCriteria.Visible = true;
                        InitContactForm();
                        break;

                    case ContactLookUpStep.ContactList:
                        divContactList.Visible = true;

                        if (ContactSearchType != ContactProcessType.Lookup || ContactSearchType == ContactProcessType.SelectContactFromOtherAgencies)
                        {
                            if (!AppSession.IsAdmin)
                            {
                                contactSearchList.RefDataSource = null;
                                BindAutoFillDataSource();
                            }

                            btnBack.Visible = false;
                        }

                        BindContactList();
                        break;

                    case ContactLookUpStep.CotactAddressLookUp:
                        divContactAddressList.Visible = true;
                        divAddressList.Visible = AppSession.IsAdmin || StandardChoiceUtil.IsEnableContactAddress();

                        /*
                         * Change contact address related UI elements in contact selection page when:
                         * current page is in registration procedure
                         */
                        if (ContactSectionPosition == ACAConstant.ContactSectionPosition.RegisterAccount
                            || ContactSectionPosition == ACAConstant.ContactSectionPosition.RegisterExistingAccount)
                        {
                            divAddressList.Visible = false;

                            SetDialogFixedWidth("400");
                            divContactType.Attributes["class"] = "contact_type";
                        }

                        if (AppSession.IsAdmin)
                        {
                            /*
                             * Hide contact address related UI elements in contact selection page when:
                             * current page is in registration procedure
                             */
                            if (ContactSectionPosition == ACAConstant.ContactSectionPosition.RegisterAccount 
                                || ContactSectionPosition == ACAConstant.ContactSectionPosition.RegisterExistingAccount)
                            {
                                divContactAddressListButton.Visible = false;
                            }

                            break;
                        }

                        // If Look Up, need to get the full contact name
                        if (ContactSearchType == ContactProcessType.Lookup && SelectedPeople != null)
                        {
                            IPeopleBll peopleBll = ObjectFactory.GetObject<IPeopleBll>();
                            SelectedPeople.SearchContactName = peopleBll.GetContactUserName(TempModelConvert.ConvertToPeopleModel4WS(SelectedPeople), true);
                        }

                        lblContactFullName.Text = SelectedPeople == null ? string.Empty : SelectedPeople.SearchContactName;

                        if (IsMultipleContact || ContactSectionPosition == ACAConstant.ContactSectionPosition.RegisterAccount || ContactSectionPosition == ACAConstant.ContactSectionPosition.RegisterExistingAccount)
                        {
                            if (ContactSectionPosition == ACAConstant.ContactSectionPosition.SpearForm)
                            {
                                DropDownListBindUtil.BindContactTypeWithPageFlow(
                                    ddlContactType,
                                    ModuleName,
                                    true,
                                    ContactSessionParameterModel.PageFlowComponent.ComponentName);
                            }
                            else if (ContactSectionPosition == ACAConstant.ContactSectionPosition.RegisterAccount || ContactSectionPosition == ACAConstant.ContactSectionPosition.RegisterExistingAccount || ContactSectionPosition == ACAConstant.ContactSectionPosition.AddReferenceContact)
                            {
                                DropDownListBindUtil.BindContactTypeInRegistration(ddlContactType, XPolicyConstant.CONTACT_TYPE_REGISTERATION);
                            }
                            else
                            {
                                DropDownListBindUtil.BindContactType(ddlContactType, ContactTypeSource.Reference);
                            }

                            ddlContactType.Visible = true;

                            /*
                             * If only one contact type available, auto-select the contact type.
                             * In contact address selection form, will display message to notice user the required contact address based on the contact type,
                             *      and auto-select the required contact address.
                             */
                            ListItem defaultItem = new ListItem(WebConstant.DropDownDefaultText, string.Empty);

                            if (ddlContactType.Items.Count == 2 && ddlContactType.Items.IndexOf(defaultItem) > -1)
                            {
                                ddlContactType.SelectedValue = ddlContactType.Items[1].Value;
                            }
                        }
                        else
                        {
                            lblContactType.Visible = true;
                            lblContactType.Text = ContactType;
                        }

                        InitContactAddressInfo();

                        break;
                }
            }

            if (DropDownListBindUtil.IsExistOnlyOneItem(ddlContactType)
                && (ACAConstant.ContactSectionPosition.RegisterAccount == ContactSectionPosition 
                    || ACAConstant.ContactSectionPosition.RegisterExistingAccount == ContactSectionPosition
                    || !StandardChoiceUtil.IsEnableContactAddress()))
            {
                BuildDispalyPeopleData();
            }
        }

        /// <summary>
        /// Initial contact address information.
        /// bind contact address and show required contact address.
        /// </summary>
        private void InitContactAddressInfo()
        {
            string contactType = IsMultipleContact || ContactSectionPosition == ACAConstant.ContactSectionPosition.RegisterAccount || ContactSectionPosition == ACAConstant.ContactSectionPosition.RegisterExistingAccount ? ddlContactType.SelectedValue : ContactType;
            ShowRequiredContactAddressInstruction(contactType);
            contactAddressSearchList.ContactType = contactType;
            contactAddressSearchList.LoadAddressList(ObjectConvertUtil.ConvertArrayToList(SelectedPeople.contactAddressLists));
        }

        /// <summary>
        /// Initial search by contact section.
        /// </summary>
        private void InitContactForm()
        {
            ucContactInfo.SetSectionRequired("0");
            ucContactInfo.IsShowContactType = true;
            ucContactInfo.IsEditable = true;

            if (!AppSession.IsAdmin)
            {
                SetPageTitleKey(LABEL_KEY_CONTACTLOOKUP_TITLE);
            }
        }

        /// <summary>
        /// Search reference contact data.
        /// </summary>
        /// <param name="currentPageIndex">current page index</param>
        /// <param name="sortExpression">sort expression</param>
        private void GetLookUpDataSource(int currentPageIndex, string sortExpression)
        {
            IPeopleBll peopleBll = (IPeopleBll)ObjectFactory.GetObject(typeof(IPeopleBll));
            PeopleModel people = ucContactInfo.GetSearchCondition();

            PaginationModel pageInfo = PaginationUtil.GetPageInfoByID(contactSearchList.ClientID);
            pageInfo.SortExpression = sortExpression;
            pageInfo.CurrentPageIndex = currentPageIndex;
            pageInfo.CustomPageSize = contactSearchList.GViewContactList.PageSize;
            QueryFormat queryFormat = PaginationUtil.GetQueryFormatModel(pageInfo);

            //Search contact in spear form, should aways get template, even all template are not display in list.
            PeopleModel[] peopleArray = peopleBll.GetRefContactByPeopleModel(ConfigManager.AgencyCode, people, true, true, queryFormat);

            IList<PeopleModel> peopleList = new List<PeopleModel>();

            if (peopleArray != null && peopleArray.Length > 0)
            {
                int searchRowIndex = contactSearchList.RefDataSource.Count;

                foreach (PeopleModel peopleModel in peopleArray)
                {
                    PeopleModel item = ConvertToSearchModel(peopleModel);
                    item.SearchRowIndex = searchRowIndex;
                    item.SearchContactName = string.Empty;
                    item.SearchContactType = string.Empty;
                    peopleList.Add(item);

                    searchRowIndex++;
                }
            }

            peopleList = PaginationUtil.MergeDataSource<IList<PeopleModel>>(contactSearchList.RefDataSource, peopleList, pageInfo);
            contactSearchList.RefDataSource = peopleList;
        }

        /// <summary>
        /// Bind Contact
        /// </summary>
        private void BindContactList()
        {
            contactSearchList.BindRefContactsList();

            if (contactSearchList.RefDataSource.Count == 1)
            {
                // if only one contact, directly select it to go to select it's addresses.
                contactSearchList.SelectedRowIndex = 0;
                SelectedPeople = contactSearchList.SelectedItem;

                if (SelectedPeople != null)
                {
                    JavaScriptSerializer javaScriptSerializer = new JavaScriptSerializer();
                    AutoFillParameter parameter = javaScriptSerializer.Deserialize<AutoFillParameter>(SelectedPeople.ConditionParameters);

                    if (!ACAConstant.AutoFillType4SpearForm.License.Equals(parameter.AutoFillType))
                    {
                        IPeopleBll peopleBll = ObjectFactory.GetObject<IPeopleBll>();
                        SelectedPeople.SearchContactName = peopleBll.GetContactUserName(TempModelConvert.ConvertToPeopleModel4WS(SelectedPeople), true);
                    }

                    SaveContact(SelectedPeople);
                }
            }
        }

        /// <summary>
        /// Bind auto fill data source
        /// </summary>
        private void BindAutoFillDataSource()
        {
            List<PeopleModel> items = new List<PeopleModel>();

            //if register user and in cap edit page, the auto-fill should be showed.
            if (ACAConstant.ContactSectionPosition.SpearForm.Equals(ContactSectionPosition)
                && (AppSession.User.IsAuthorizedAgent || AppSession.User.IsAgentClerk))
            {
                BindCustomerContactDataSource();
            }
            else if (ACAConstant.ContactSectionPosition.SpearForm.Equals(ContactSectionPosition) && !AppSession.User.IsAnonymous)
            {
                BindAccountAssociatedDataSource(0, null);
            }
            else if (ACAConstant.ContactSectionPosition.RegisterAccount.Equals(ContactSectionPosition) && StandardChoiceUtil.IsRequiredLicense())
            {
                BindLicenseList4RegisterDataSource();
            }
            else if (ACAConstant.ContactSectionPosition.RegisterExistingAccount.Equals(ContactSectionPosition))
            {
                BuildContactList4ExistingAccountRegisterationDataSource();
            }
        }

        /// <summary>
        /// Get agency display text.
        /// </summary>
        /// <param name="licModel">License model for WS.</param>
        /// <returns>return agency display text</returns>
        private string GetAgencyDisplayText(LicenseModel4WS licModel)
        {
            if (string.IsNullOrEmpty(licModel.agencyAliasName) && string.IsNullOrEmpty(licModel.serviceProviderCode))
            {
                return string.Empty;
            }

            return string.IsNullOrEmpty(licModel.agencyAliasName)
                    ? licModel.serviceProviderCode
                    : licModel.agencyAliasName;
        }

        /// <summary>
        /// Add Contact List Item
        /// </summary>
        /// <returns>return account associated contact</returns>
        private List<PeopleModel> GetAcountAssociatedContact()
        {
            var proxyUserRoleBll = ObjectFactory.GetObject<IProxyUserRoleBll>();
            PublicUserModel4WS user = proxyUserRoleBll.GetCurrentUser(AppSession.GetCapModelFromSession(ModuleName));
            List<PeopleModel> items = new List<PeopleModel>();

            if (user != null && user.peopleModel != null)
            {
                PeopleModel4WS[] peopleModelList = user.peopleModel.Where(p =>
                    ContractorPeopleStatus.Approved.Equals(p.contractorPeopleStatus, StringComparison.OrdinalIgnoreCase)
                    || string.IsNullOrEmpty(p.contractorPeopleStatus)).ToArray();

                if (peopleModelList.Length > 0)
                {
                    IList<PeopleModel> peopleList = TempModelConvert.ConvertToPeopleModel(peopleModelList, true);
                    AddContactListItem(peopleList.ToArray(), items);
                }

                return items;
            }

            return null;
        }

        /// <summary>
        /// Add Contact List Item.
        /// </summary>
        /// <param name="peopleModelList">peopleModel List</param>
        /// <param name="items">contact item list.</param>
        private void AddContactListItem(IEnumerable<PeopleModel> peopleModelList, IList<PeopleModel> items)
        {
            if (peopleModelList == null)
            {
                return;
            }

            int searchRowIndex = items.Count;
            string searchCategoryText = GetTextByKey("ACA_ContactEdit_AutoFillItems_AssociatedContactSeparator");
            IPeopleBll peopleBll = ObjectFactory.GetObject<IPeopleBll>();

            foreach (PeopleModel peopleModel in peopleModelList)
            {
                string contactName = peopleBll.GetContactUserName(TempModelConvert.ConvertToPeopleModel4WS(peopleModel), true);

                PeopleModel item = ConvertToSearchModel(peopleModel);
                item.SearchRowIndex = searchRowIndex;
                item.SearchContactName = contactName;
                item.SearchCategoryText = searchCategoryText;
                item.SearchContactType = peopleModel.contactType;
                item.SearchAgencyText = string.IsNullOrEmpty(peopleModel.agencyAliasName)
                    ? peopleModel.serviceProviderCode
                    : peopleModel.agencyAliasName;

                items.Add(item);

                searchRowIndex++;
            }
        }

        /// <summary>
        /// build associated LP option for auto-fill data source. 
        /// </summary>
        /// <param name="associatedDataSourceCount">The associated data source's count.</param>
        /// <returns>return account associated LP</returns>
        private List<PeopleModel> GetAccountAssociatedLP(int associatedDataSourceCount)
        {
            List<PeopleModel> items = new List<PeopleModel>();

            var proxyUserRoleBll = ObjectFactory.GetObject<IProxyUserRoleBll>();
            PublicUserModel4WS user = proxyUserRoleBll.GetCurrentUser(AppSession.GetCapModelFromSession(ModuleName));

            if (user == null)
            {
                return null;
            }

            LicenseModel4WS[] licenseItems = user.licenseModel;

            if (licenseItems == null || licenseItems.Length == 0)
            {
                return null;
            }

            int searchRowIndex = associatedDataSourceCount;
            string searchCategoryText = GetTextByKey("aca_contactedit_autofillitems_associatedlicenseseparator");

            foreach (LicenseModel4WS licenseItem in licenseItems)
            {
                if (licenseItem == null)
                {
                    continue;
                }

                ILicenseBLL licenseBll = (ILicenseBLL)ObjectFactory.GetObject(typeof(ILicenseBLL));
                LicenseModel4WS licenseModel = licenseBll.GetLicenseCondition(ConfigManager.AgencyCode, long.Parse(licenseItem.licSeqNbr), AppSession.User.PublicUserId);

                if (ConditionsUtil.IsLicenseLocked(licenseModel))
                {
                    continue;
                }

                PeopleModel item = ConvertToSearchModel(licenseItem);

                item.SearchRowIndex = searchRowIndex;
                item.SearchContactName = FormatSearchLicenseName(licenseItem);
                item.SearchCategoryText = searchCategoryText;
                item.SearchContactType = licenseItem.licenseType;
                items.Add(item);

                searchRowIndex++;
            }

            return items;
        }

        /// <summary>
        /// build associated owner option for auto-fill data source.
        /// </summary>
        /// <param name="currentPageIndex">Current page index</param>
        /// <param name="sortExpression">Sort Expression</param>
        /// <param name="count">The count of associated Contact and LP</param>
        /// <param name="isPageIndexChanging">Is pageIndexChanging flag</param>
        /// <returns>People List</returns>
        private List<PeopleModel> GetAutoFillOwner(int currentPageIndex, string sortExpression, int count, bool isPageIndexChanging)
        {
            if (!PageFlowUtil.IsComponentExist(PageFlowComponent.OWNER))
            {
                return null;
            }

            int endRow = 0;

            if (!isPageIndexChanging)
            {
                //Calculate the end row for associated owner list.
                int pageSizeIndex = count / (contactSearchList.PageSize * ACAConstant.DEFAULT_PAGECOUNT);
                endRow = (contactSearchList.PageSize * ACAConstant.DEFAULT_PAGECOUNT * (pageSizeIndex + 1)) - count + 1;
            }

            OwnerModel[] owners = GetOwnerAutoFillByParcelPK(currentPageIndex, sortExpression, endRow);

            if (owners == null || owners.Length == 0)
            {
                return null;
            }

            List<PeopleModel> peopleList = GetPeopleListByAutoFillOwner(owners, count);

            return peopleList;
        }

        /// <summary>
        /// get added license array from session.
        /// </summary>
        /// <returns>LicenseModel4WS array</returns>
        private LicenseModel4WS[] GetArrayLicense4Register()
        {
            ArrayList selectedLicenses = Session[SessionConstant.SESSION_REGISTER_LICENSES] as ArrayList;

            if (selectedLicenses == null || selectedLicenses.Count == 0)
            {
                return null;
            }

            LicenseModel4WS[] arrayLicense = (LicenseModel4WS[])selectedLicenses.ToArray(typeof(LicenseModel4WS));
            return arrayLicense;
        }

        /// <summary>
        /// Convert the LicenseModel4WS to SearchContactModel 
        /// </summary>
        /// <param name="licenseItem">the LicenseModel4WS</param>
        /// <returns>the people model</returns>
        private PeopleModel ConvertToSearchModel(LicenseModel4WS licenseItem)
        {
            PeopleModel people = new PeopleModel();
            people.salutation = licenseItem.salutation;
            people.title = licenseItem.title;
            people.firstName = licenseItem.contactFirstName;
            people.middleName = licenseItem.contactMiddleName;
            people.lastName = licenseItem.contactLastName;
            people.fullName = string.Empty;
            people.birthDate = string.IsNullOrEmpty(licenseItem.birthDate) ? (DateTime?)null : I18nDateTimeUtil.ParseFromWebService(licenseItem.birthDate);
            people.gender = licenseItem.gender;
            people.businessName = licenseItem.businessName;
            people.country = licenseItem.country;
            people.countryCode = licenseItem.countryCode;
            people.phone1CountryCode = licenseItem.phone1CountryCode;
            people.phone1 = licenseItem.phone1;
            people.postOfficeBox = licenseItem.postOfficeBox;
            people.phone2CountryCode = licenseItem.phone2CountryCode;
            people.phone2 = licenseItem.phone2;
            people.phone3CountryCode = licenseItem.phone3CountryCode;
            people.phone3 = licenseItem.phone3;
            people.faxCountryCode = licenseItem.faxCountryCode;
            people.fax = licenseItem.fax;
            people.email = licenseItem.emailAddress;
            people.namesuffix = licenseItem.suffixName;
            people.socialSecurityNumber = licenseItem.socialSecurityNumber;
            people.fein = licenseItem.fein;
            people.contactTypeFlag = licenseItem.typeFlag;

            CompactAddressModel compactAddress = new CompactAddressModel();
            compactAddress.addressLine1 = licenseItem.address1;
            compactAddress.addressLine2 = licenseItem.address2;
            compactAddress.addressLine3 = licenseItem.address3;
            compactAddress.city = licenseItem.city;
            compactAddress.country = licenseItem.country;
            compactAddress.countryCode = licenseItem.countryCode;
            compactAddress.state = licenseItem.state;
            compactAddress.streetName = string.Empty;
            compactAddress.zip = licenseItem.zip;
            people.compactAddress = compactAddress;

            people.tradeName = string.Empty;
            people.businessName2 = string.Empty; 
            people.birthCity = string.Empty;
            people.birthState = string.Empty;
            people.birthRegion = string.Empty;
            people.race = string.Empty;
            people.deceasedDate = null;
            people.passportNumber = string.Empty;
            people.driverLicenseNbr = string.Empty;
            people.driverLicenseState = string.Empty;
            people.stateIDNbr = string.Empty;
            people.preferredChannel = null;
            people.comment = string.Empty;
            people.peopleAKAList = null;
            people.template = licenseItem.template;
            people.attributes = licenseItem.templateAttributes;
            people.serviceProviderCode = licenseItem.serviceProviderCode;
            people.postOfficeBox = licenseItem.postOfficeBox;

            AutoFillParameter parameter = new AutoFillParameter()
            {
                AutoFillType = ACAConstant.AutoFillType4SpearForm.License,
                SectionId = this.ID,
                EntityId = licenseItem.stateLicense,
                EntityType = licenseItem.licenseType,
                EntityRefId = licenseItem.licSeqNbr
            };

            JavaScriptSerializer javaScriptSerializer = new JavaScriptSerializer();
            people.ConditionParameters = javaScriptSerializer.Serialize(parameter);

            return people;
        }

        /// <summary>
        /// Convert the OwnerModel to SearchModel 
        /// </summary>
        /// <param name="owner">the OwnerModel</param>
        /// <returns>the people model</returns>
        private PeopleModel ConvertToSearchModel(OwnerModel owner)
        {
            PeopleModel people = new PeopleModel();

            people.salutation = string.Empty;
            people.title = owner.ownerTitle;
            people.firstName = owner.ownerFirstName;
            people.middleName = owner.ownerMiddleName;
            people.lastName = owner.ownerLastName;
            people.fullName = owner.ownerFullName;
            people.birthDate = null;
            people.gender = string.Empty;
            people.businessName = owner.ownerFullName;
            people.country = owner.mailCountry;
            people.phone1CountryCode = owner.phoneCountryCode;
            people.phone1 = owner.phone;
            people.postOfficeBox = string.Empty;
            people.phone2CountryCode = string.Empty;
            people.phone2 = string.Empty;
            people.phone3CountryCode = string.Empty;
            people.phone3 = string.Empty;
            people.faxCountryCode = owner.faxCountryCode;
            people.fax = owner.fax;
            people.email = owner.email;
            people.socialSecurityNumber = string.Empty;
            people.fein = string.Empty;

            CompactAddressModel compactAddress = new CompactAddressModel();
            compactAddress.addressLine1 = owner.mailAddress1;
            compactAddress.addressLine2 = owner.mailAddress2;
            compactAddress.addressLine3 = owner.mailAddress3;
            compactAddress.city = owner.mailCity;
            compactAddress.country = owner.mailCountry;
            compactAddress.countryCode = owner.mailCountry;
            compactAddress.state = owner.mailState;
            compactAddress.streetName = string.Empty;
            compactAddress.zip = owner.mailZip;
            people.compactAddress = compactAddress;
            people.serviceProviderCode = ConfigManager.AgencyCode;

            people.tradeName = string.Empty;
            people.businessName2 = string.Empty;
            people.birthCity = string.Empty;
            people.birthState = string.Empty;

            people.birthRegion = string.Empty;
            people.race = string.Empty;
            people.deceasedDate = null;
            people.passportNumber = string.Empty;
            people.driverLicenseNbr = string.Empty;
            people.driverLicenseState = string.Empty;
            people.stateIDNbr = string.Empty;
            people.preferredChannel = null;
            people.comment = string.Empty;
            people.peopleAKAList = null;

            TemplateAttributeModel[] attributes = null;

            if (!ConfigManager.SuperAgencyCode.Equals(ConfigManager.AgencyCode, StringComparison.OrdinalIgnoreCase))
            {
                ITemplateBll templateBll = (ITemplateBll)ObjectFactory.GetObject(typeof(ITemplateBll));
                attributes = templateBll.GetAPOTemplateAttributes(TemplateType.CAP_OWNER, ConfigManager.AgencyCode, AppSession.User.PublicUserId);
            }
            else
            {
                attributes = owner.templates;
            }

            people.attributes = attributes;

            AutoFillParameter parameter = new AutoFillParameter()
            {
                AutoFillType = ACAConstant.AutoFillType4SpearForm.ContactOwner,
                SectionId = this.ID,
                EntityId = owner.ownerNumber.HasValue ? owner.ownerNumber.ToString() : string.Empty,
                EntityType = owner.sourceSeqNumber.HasValue ? owner.sourceSeqNumber.ToString() : string.Empty,
                EntityRefId = owner.UID
            };

            JavaScriptSerializer javaScriptSerializer = new JavaScriptSerializer();
            people.ConditionParameters = javaScriptSerializer.Serialize(parameter);

            return people;
        }

        /// <summary>
        /// Convert the PeopleModel to SearchModel 
        /// </summary>
        /// <param name="people">the OwnerModel</param>
        /// <returns>the people model</returns>
        private PeopleModel ConvertToSearchModel(PeopleModel people)
        {
            PeopleModel searchModel = new PeopleModel();

            searchModel.auditStatus = people.auditStatus;
            searchModel.firstName = people.firstName;
            searchModel.middleName = people.middleName;
            searchModel.lastName = people.lastName;
            searchModel.fullName = people.fullName;
            searchModel.namesuffix = people.namesuffix;
            searchModel.rate1 = people.rate1;
            searchModel.searchFullName = people.searchFullName;
            searchModel.title = people.title;
            searchModel.attributes = people.attributes;
            searchModel.auditDate = people.auditDate;
            searchModel.auditID = people.auditID;
            searchModel.birthDate = people.birthDate;
            searchModel.birthCity = people.birthCity;
            searchModel.birthRegion = people.birthRegion;
            searchModel.birthState = people.birthState;
            searchModel.busName2 = people.busName2;
            searchModel.businessName = people.businessName;
            searchModel.businessName2 = people.businessName2;
            searchModel.comment = people.comment;
            searchModel.compactAddress = people.compactAddress;
            searchModel.contactAddress = people.contactAddress;
            searchModel.contactAddressLists = people.contactAddressLists;
            searchModel.contactSeqNumber = people.contactSeqNumber;
            searchModel.contactType = people.contactType;
            searchModel.contactTypeFlag = people.contactTypeFlag;
            searchModel.country = people.country;
            searchModel.countryCode = people.countryCode;
            searchModel.deceasedDate = people.deceasedDate;
            searchModel.driverLicenseNbr = people.driverLicenseNbr;
            searchModel.driverLicenseState = people.driverLicenseState;
            searchModel.email = people.email;
            searchModel.endBirthDate = people.endBirthDate;
            searchModel.endDate = people.endDate;
            searchModel.endDeceasedDate = people.endDeceasedDate;
            searchModel.fax = people.fax;
            searchModel.faxCountryCode = people.faxCountryCode;
            searchModel.fein = people.fein;
            searchModel.flag = people.flag;
            searchModel.gender = people.gender;
            searchModel.holdCode = people.holdCode;
            searchModel.holdDescription = people.holdDescription;
            searchModel.id = people.id;
            searchModel.ivrPinNumber = people.ivrPinNumber;
            searchModel.ivrUserNumber = people.ivrUserNumber;
            searchModel.maskedSsn = people.maskedSsn;
            searchModel.passportNumber = people.passportNumber;
            searchModel.peopleAKAList = people.peopleAKAList;
            searchModel.phone1 = people.phone1;
            searchModel.phone1CountryCode = people.phone1CountryCode;
            searchModel.phone2 = people.phone2;
            searchModel.phone2CountryCode = people.phone2CountryCode;
            searchModel.phone3 = people.phone3;
            searchModel.phone3CountryCode = people.phone3CountryCode;
            searchModel.postOfficeBox = people.postOfficeBox;
            searchModel.preferredChannel = people.preferredChannel;
            searchModel.preferredChannelString = people.preferredChannelString;
            searchModel.race = people.race;
            searchModel.relation = people.relation;
            searchModel.salutation = people.salutation;
            searchModel.serviceProviderCode = people.serviceProviderCode;
            searchModel.socialSecurityNumber = people.socialSecurityNumber;
            searchModel.startDate = people.startDate;
            searchModel.template = people.template;
            searchModel.stateIDNbr = people.stateIDNbr;
            searchModel.tradeName = people.tradeName;
            searchModel.contractorPeopleStatus = people.contractorPeopleStatus;

            AutoFillParameter parameter = new AutoFillParameter()
            {
                AutoFillType = ACAConstant.AutoFillType4SpearForm.Contact,
                SectionId = ID,
                EntityId = string.Empty,
                EntityType = people.contactType,
                EntityRefId = people.contactSeqNumber
            };

            JavaScriptSerializer javaScriptSerializer = new JavaScriptSerializer();
            searchModel.ConditionParameters = javaScriptSerializer.Serialize(parameter);

            return searchModel;
        }

        /// <summary>
        /// Save the contact.
        /// </summary>
        /// <param name="people">The selected contact</param>
        private void SaveContact(PeopleModel people)
        {
            if (!IsLoginUseExistingAccount && !contactSearchList.ValidateCondition())
            {
                return;
            }

            // If disable the contact address, set contactAddressLists as null.
            if (!StandardChoiceUtil.IsEnableContactAddress())
            {
                people.contactAddressLists = null;
            }

            // if no address in the single contact in SPEAR form, directly run select address event.
            if (ContactSectionPosition == ACAConstant.ContactSectionPosition.SpearForm
                && !IsMultipleContact
                && (people.contactAddressLists == null || people.contactAddressLists.Length == 0))
            {
                SaveContactAddress(people, ContactType);
            }
            else
            {
                ContinueToNextStep(1);
            }
        }

        /// <summary>
        /// Save the contact address and redirect to add page or close the popup.
        /// </summary>
        /// <param name="people">Selected contact</param>
        /// <param name="contactType">Contact type</param>
        private void SaveContactAddress(PeopleModel people, string contactType)
        {
            if (people == null)
            {
                return;
            }

            // Clear the contact sequency number if the contact from other agency.
            if (!people.serviceProviderCode.Equals(ConfigManager.AgencyCode, StringComparison.OrdinalIgnoreCase))
            {
                people.contactSeqNumber = string.Empty;
            }

            if (ValidationUtil.IsYes(people.flag))
            {
                people.flag = string.Empty;
            }

            SetTemplateAttributesValueByContactType(people, contactType);
            people.template = PeopleUtil.MergeGenericTemplate(people.template, contactType, ModuleName);

            PeopleModel4WS tmpPeople = TempModelConvert.ConvertToPeopleModel4WS(people);

            if (!string.Equals(tmpPeople.contactType, contactType, StringComparison.InvariantCulture))
            {
                ContactUtil.MergeContactTemplateModel(tmpPeople, contactType, ModuleName);
            }

            tmpPeople.contactType = contactType;
            tmpPeople.serviceProviderCode = ConfigManager.AgencyCode;

            if (tmpPeople.contactAddressList != null && tmpPeople.contactAddressList.Count() == 1 && StandardChoiceUtil.IsPrimaryContactAddressRequired())
            {
                tmpPeople.contactAddressList[0].primary = ACAConstant.COMMON_Y;
            }
            else
            {
                string errorMsg = ContactUtil.GetErrorMessage4DuplicateContactAddress(ModuleName, ConfigManager.AgencyCode, tmpPeople.contactAddressList);

                if (!string.IsNullOrEmpty(errorMsg) && !IsLoginUseExistingAccount)
                {
                    MessageUtil.ShowMessageInPopup(Page, MessageType.Error, errorMsg);
                    return;
                }
            }

            if (ContactSessionParameterModel.Data.DataObject is CapContactModel4WS)
            {
                CapContactModel4WS capContact = ContactSessionParameterModel.Data.DataObject as CapContactModel4WS;
                capContact.people = tmpPeople;
                capContact.refContactNumber = tmpPeople.contactSeqNumber;

                if (string.IsNullOrWhiteSpace(capContact.accessLevel))
                {
                    capContact.accessLevel = ContactUtil.GetDefaultContactPermisssion(contactType, GviewID.ContactEdit, ModuleName, "radioListContactPermission");
                }

                ContactSessionParameterModel.Data.DataObject = capContact;
            }
            else if (ContactSessionParameterModel.Data.DataObject is PeopleModel4WS)
            {
                ContactSessionParameterModel.Data.DataObject = tmpPeople;
            }

            AppSession.SetContactSessionParameter(ContactSessionParameterModel);

            if (ContactUtil.IsPassContactValidateInSpearForm(ContactSessionParameterModel, ModuleName))
            {
                CapModel4WS capModel = AppSession.GetCapModelFromSession(ModuleName);

                if (ContactUtil.IsNeedToSelectLCData(capModel, ContactSessionParameterModel, people.contactSeqNumber, people.contactTypeFlag))
                {
                    string url = string.Format(
                        "~/LicenseCertification/RefContactEducationExamLookUp.aspx?contactSeqNbr={0}&{1}={2}&isPopup=Y",
                        people.contactSeqNumber,
                        ACAConstant.MODULE_NAME,
                        ModuleName);

                    Response.Redirect(url);
                    Response.End();
                }
                else
                {
                    ScriptManager.RegisterStartupScript(Page, GetType(), "SaveContact", "PopupClose(" + people.contactSeqNumber + ");", true);
                }
            }
            else
            {
                Response.Redirect(Page.ResolveUrl("~/People/ContactAddNew.aspx") + Request.Url.Query);
            }
        }

        /// <summary>
        /// Show required contact address type instruction message.
        /// </summary>
        /// <param name="contactType">current contact type</param>
        private void ShowRequiredContactAddressInstruction(string contactType)
        {
            List<string> instruction = new List<string>();
            instruction.Add(GetTextByKey("aca_contactlookup_label_contactaddresslisthints"));
            instruction.Add(PeopleUtil.GetRequiredAddressInstruction(ModuleName, contactType));
            lblContactAddressListHints.Text = DataUtil.ConcatStringWithSplitChar(instruction, ACAConstant.HTML_BR);
        }

        /// <summary>
        /// Change label keys for contact title, button OK and Cancel.
        /// </summary>
        /// <param name="contactProcessType">Contact process type.</param>
        private void ChangeLabelKeys(ContactProcessType contactProcessType)
        {
            string labelKey;

            if (contactProcessType == ContactProcessType.SelectContactFromAccount || contactProcessType == ContactProcessType.SelectContactFromOtherAgencies)
            {
                if (ContactSectionPosition == ACAConstant.ContactSectionPosition.RegisterAccount)
                {
                    labelKey = LABEL_KEY_REGISTRATION_SELECTFROMACCOUNT_TITLE;
                }
                else if (ContactSectionPosition == ACAConstant.ContactSectionPosition.RegisterExistingAccount)
                {
                    labelKey = LABEL_KEY_REGISTRATION_SELECTCONTACT_TITLE;
                    btnContinueContact.LabelKey = LABEL_KEY_SELECT_CONTACT_INFO_LABEL_OK;
                    btnCancelContactList.LabelKey = LABEL_KEY_SELECT_CONTACT_INFO_LABEL_CANCEL;
                }
                else
                {
                    labelKey = LABEL_KEY_SELECTFROMACCOUNT_TITLE;
                }
            }
            else
            {
                labelKey = LABEL_KEY_CONTACTLOOKUP_TITLE;
            }

            lblContactTitle.LabelKey = labelKey;
        }

        /// <summary>
        /// Get the owner auto fill list for contact.
        /// </summary>
        /// <param name="currentPageIndex">Current PageIndex</param>
        /// <param name="sortExpression">Sort Expression</param>
        /// <param name="endRow">The end row for list</param>
        /// <returns>Owner model array</returns>
        private OwnerModel[] GetOwnerAutoFillByParcelPK(int currentPageIndex, string sortExpression, int endRow = 0)
        {
            ParcelModel parcelPK = (ParcelModel)Session[SessionConstant.APO_SESSION_PARCELMODEL];

            if (parcelPK == null)
            {
                return null;
            }

            PaginationModel pageInfo = PaginationUtil.GetPageInfoByID(contactSearchList.ClientID);
            pageInfo.SortExpression = sortExpression;
            pageInfo.CurrentPageIndex = currentPageIndex;
            pageInfo.CustomPageSize = contactSearchList.PageSize;
            QueryFormat queryFormat = PaginationUtil.GetQueryFormatModel(pageInfo);

            if (endRow != 0)
            {
                queryFormat.endRow = endRow;
            }

            IAPOBll apoBll = ObjectFactory.GetObject<IAPOBll>();
            SearchResultModel searchResult = apoBll.GetOwnerListByParcelPKs(ConfigManager.AgencyCode, new[] { parcelPK }, true, queryFormat);

            pageInfo.StartDBRow = searchResult.startRow;
            OwnerModel[] owners = ObjectConvertUtil.ConvertObjectArray2EntityArray<OwnerModel>(searchResult.resultList);

            return owners;
        }

        /// <summary>
        /// Update Auto Fill Owner To Contact Data Source
        /// </summary>
        /// <param name="owners">Owner List</param>
        /// <param name="count">The count of contact data source</param>
        /// <returns>People List</returns>
        private List<PeopleModel> GetPeopleListByAutoFillOwner(IEnumerable<OwnerModel> owners, int count = 0)
        {
            int searchRowIndex = 0;

            if (count != 0)
            {
                searchRowIndex = count;
            }
            else
            {
                searchRowIndex = contactSearchList.RefDataSource.Count; 
            }

            string searchCategoryText = GetTextByKey("aca_contactedit_autofillitems_associatedownerseparator");
            List<PeopleModel> peopleList = new List<PeopleModel>();

            foreach (OwnerModel owner in owners)
            {
                if (owner == null || string.IsNullOrEmpty(owner.ownerFullName))
                {
                    continue;
                }

                string ownerNumber = owner.ownerNumber.HasValue ? owner.ownerNumber.ToString() : string.Empty;
                string sourceSeqNumber = owner.sourceSeqNumber.HasValue ? owner.sourceSeqNumber.ToString() : string.Empty;
                string ownerUID = owner.UID;

                IOwnerBll ownerBll = (IOwnerBll)ObjectFactory.GetObject(typeof(IOwnerBll));
                OwnerModel refOwnerModel = ownerBll.GetOwnerCondition(ConfigManager.AgencyCode, sourceSeqNumber, ownerNumber, ownerUID);

                if (ConditionsUtil.IsOwnerLocked(refOwnerModel))
                {
                    continue;
                }

                PeopleModel item = ConvertToSearchModel(owner);
                item.SearchRowIndex = searchRowIndex;
                item.SearchContactName = owner.ownerFullName;
                item.SearchCategoryText = searchCategoryText;
                item.SearchContactType = string.Empty;
                peopleList.Add(item);

                searchRowIndex++;
            }

            return peopleList;
        }

        /// <summary>
        /// Bind customer detail contact to contact data source.
        /// </summary>
        private void BindCustomerContactDataSource()
        {
            // For authorized agent or clerk, the auto-fill item only include the contact info that inputed or selected in the customer detail page
            string contactSeqNbr = Request[UrlConstant.CONTACT_SEQ_NUMBER];
            List<PeopleModel> items = new List<PeopleModel>();

            if (!string.IsNullOrEmpty(contactSeqNbr))
            {
                PeopleModel peopleModel = AppSession.GetPeopleModelFromSession(contactSeqNbr);
                AddContactListItem(new[] { peopleModel }, items);
            }

            contactSearchList.RefDataSource = items;
        }

        /// <summary>
        /// Bind account associated data(Including contact, LP, owner) auto fill list to contact data source
        /// </summary>
        /// <param name="currentPageIndex">Current PageIndex</param>
        /// <param name="sortExpression">Sort Expression</param>
        /// <param name="isPageIndexChanging">isPageIndexChanging flag</param>
        private void BindAccountAssociatedDataSource(int currentPageIndex, string sortExpression, bool isPageIndexChanging = false)
        {
            List<PeopleModel> items = new List<PeopleModel>();
            List<PeopleModel> temp = null;
            int count = 0;

            if (!isPageIndexChanging)
            {
                temp = GetAcountAssociatedContact();

                if (temp != null && temp.Count > 0)
                {
                    items.AddRange(temp);
                }     
            }

            if (ContactSessionParameterModel.PageFlowComponent.ComponentDataSource != ComponentDataSource.Reference)
            {
                if (!isPageIndexChanging)
                {
                    temp = GetAccountAssociatedLP(items.Count);

                    if (temp != null && temp.Count > 0)
                    {
                        items.AddRange(temp);
                    }
                }

                if (!isPageIndexChanging)
                {
                    count = items.Count;
                }

                temp = GetAutoFillOwner(currentPageIndex, sortExpression, count, isPageIndexChanging);

                if (temp != null && temp.Count > 0)
                {
                    items.AddRange(temp);
                }
            }

            IList<PeopleModel> peopleList = items;

            PaginationModel pageInfo = PaginationUtil.GetPageInfoByID(contactSearchList.ClientID);
            pageInfo.SortExpression = sortExpression;
            pageInfo.CurrentPageIndex = currentPageIndex;
            pageInfo.CustomPageSize = contactSearchList.PageSize;

            peopleList = PaginationUtil.MergeDataSource<IList<PeopleModel>>(contactSearchList.RefDataSource, peopleList, pageInfo);
            contactSearchList.RefDataSource = peopleList;
        }

        /// <summary>
        /// Bind license item into auto fill data source.
        /// </summary>
        private void BindLicenseList4RegisterDataSource()
        {
            List<PeopleModel> items = new List<PeopleModel>();
            LicenseModel4WS[] licenseList = GetArrayLicense4Register();

            if (licenseList == null || licenseList.Length == 0)
            {
                return;
            }

            int searchRowIndex = items.Count;
            string searchCategoryText = GetTextByKey("aca_contactedit_autofillitems_associatedlicenseseparator");

            foreach (LicenseModel4WS license in licenseList)
            {
                if (license == null)
                {
                    continue;
                }

                ILicenseBLL licenseBll = (ILicenseBLL)ObjectFactory.GetObject(typeof(ILicenseBLL));
                LicenseModel4WS licenseModel = licenseBll.GetLicenseCondition(ConfigManager.AgencyCode, long.Parse(license.licSeqNbr), AppSession.User.PublicUserId);

                if (ConditionsUtil.IsLicenseLocked(licenseModel))
                {
                    continue;
                }

                PeopleModel item = ConvertToSearchModel(license);
                item.SearchRowIndex = searchRowIndex;

                //display name rule [Business Name]+[F+M+L] Name +LP type+ LP Number
                item.SearchContactName = FormatSearchLicenseName(license);
                item.SearchCategoryText = searchCategoryText;
                item.SearchContactType = license.licenseType;
                items.Add(item);

                searchRowIndex++;
            }

            contactSearchList.RefDataSource = items;
        }

        /// <summary>
        /// Construct contact list
        /// </summary>
        private void BuildContactList4ExistingAccountRegisterationDataSource()
        {
            List<PeopleModel> items = new List<PeopleModel>();

            if (IsLoginUseExistingAccount)
            {
                int searchRowIndex = items.Count;
                string searchCategoryText = GetTextByKey("aca_contactedit_autofillitems_associatedlicenseseparator");
                ArrayList selectedLicenseList = Session[SessionConstant.SESSION_REGISTER_LICENSES] as ArrayList;

                if (selectedLicenseList != null && selectedLicenseList.Count > 0)
                {
                    foreach (LicenseModel4WS licModel in selectedLicenseList)
                    {
                        PeopleModel contact = ConvertToSearchModel(licModel);
                        contact.SearchRowIndex = searchRowIndex;
                        contact.SearchContactName = FormatSearchLicenseName(licModel);
                        contact.SearchCategoryText = searchCategoryText;
                        contact.SearchContactType = licModel.licenseType;
                        contact.SearchAgencyText = GetAgencyDisplayText(licModel);
                        items.Add(contact);
                        searchRowIndex++;
                    }
                }

                IAccountBll accountBll = ObjectFactory.GetObject<IAccountBll>();
                string existringUserIdInOtherAgency = Request.QueryString[UrlConstant.USER_ID_OR_EMAIL];
                PeopleModel4WS[] contactsModels = accountBll.GetContactsByPublicUser(ConfigManager.AgencyCode, existringUserIdInOtherAgency, true);
                IList<PeopleModel> peopleList = TempModelConvert.ConvertToPeopleModel(contactsModels, true);

                if (peopleList != null && peopleList.Count > 0)
                {
                    foreach (PeopleModel peopleModel in peopleList)
                    {
                        if (peopleModel.contactAddressLists == null || !peopleModel.contactAddressLists.Any())
                        {
                            continue;
                        }

                        foreach (var contactAddress in peopleModel.contactAddressLists)
                        {
                            contactAddress.contactAddressPK.serviceProviderCode = ConfigManager.AgencyCode;
                        }
                    }

                    AddContactListItem(peopleList.ToArray(), items);
                }

                contactSearchList.RefDataSource = items;
            }
        }

        #endregion Methods
    }
}