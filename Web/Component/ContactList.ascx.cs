#region Header
/**
 * <pre>
 * 
 *  Accela Citizen Access
 *  File: ContactList.ascx.cs
 * 
 *  Accela, Inc.
 *  Copyright (C): 2010-2014
 * 
 *  Description:
 * 
 *  Notes:
 *      $Id: ContactList.ascx.cs 277741 2014-08-20 10:31:02Z ACHIEVO\james.shi $.
 *  Revision History
 *  &lt;Date&gt;  &lt;Who&gt;  &lt;What&gt;
 * </pre>
 */
#endregion Header

using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;

using Accela.ACA.BLL.Common;
using Accela.ACA.BLL.People;
using Accela.ACA.Common;
using Accela.ACA.Common.Util;
using Accela.ACA.ExpressionBuild;
using Accela.ACA.Web.Common;
using Accela.ACA.Web.Common.Control;
using Accela.ACA.WSProxy;
using Accela.Web.Controls;

namespace Accela.ACA.Web.Component
{
    /// <summary>
    /// Indicate whether the control is search result or just a contact list in spear form
    /// </summary>
    public enum ContactListLocation
    {
        /// <summary>
        /// Search Result
        /// </summary>
        SearchResult,

        /// <summary>
        /// In Spear Form Page. 
        /// </summary>
        SpearForm
    }

    /// <summary>
    /// UC for display contact list.
    /// </summary>
    public partial class ContactList : BaseUserControl
    {
        #region Fields

        /// <summary>
        /// indicate the default index value  when no contact is selected in the contact list
        /// </summary>
        public const int NO_CONTACT_SELECTED = -1;

        /// <summary>
        /// Command name .
        /// </summary>
        private const string COMMAND_SELECT_CONTACT = "SelectContact";

        /// <summary>
        /// Command name for deleting a contact
        /// </summary>
        private const string COMMAND_DELETE_CONTACT = "DeleteContact";

        /// <summary>
        /// the validate flag.
        /// </summary>
        private bool _validate = true;

        /// <summary>
        /// the validate flag.
        /// </summary>
        private string _validateFlag = string.Empty;

        /// <summary>
        /// indicate Multiple contact form editable.
        /// </summary>
        private bool _isEditable;

        /// <summary>
        /// Indicates whether enable this control's delete action or not.
        /// </summary>
        private bool _enableDeleteAction = true;

        /// <summary>
        /// indicates whether the control is used in search list or spear form
        /// </summary>
        private ContactListLocation _location = ContactListLocation.SearchResult;

        /// <summary>
        /// grid view row command event.
        /// </summary>
        public event CommonEventHandler ContactSelected;

        /// <summary>
        /// grid view row command event
        /// </summary>
        public event CommonEventHandler ContactsDeleted;

        /// <summary>
        /// grid view page index changing event.
        /// </summary>
        public event GridViewPageEventHandler PageIndexChanging;

        /// <summary>
        /// grid view sorted event.
        /// </summary>
        public event GridViewSortedEventHandler GridViewSort;

        /// <summary>
        /// grid view download event.
        /// </summary>
        public event GridViewDownloadEventHandler GridViewDownloadAll;

        #endregion Fields

        #region Properties

        /// <summary>
        /// Gets or sets the parent id.
        /// </summary>
        public string ParentID { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to show the address's count of a contact.
        /// </summary>
        public bool IsShowAddressCount { get; set; }

        /// <summary>
        /// Gets or sets cap contact data table.
        /// </summary>
        public DataTable DataSource
        {
            get
            {
                if (ViewState["ContactListDataSource"] == null)
                {
                    ViewState["ContactListDataSource"] = new DataTable();
                }

                return (DataTable)ViewState["ContactListDataSource"];
            }

            set
            {
                ViewState["ContactListDataSource"] = value;
            }
        }

        /// <summary>
        /// Gets or sets the ContactListLocation which indicates whether the list in the search results or the contact list in spear form
        /// </summary>
        /// <value>The location.</value>
        public ContactListLocation Location
        {
            get
            {
                return _location;
            }

            set
            {
                _location = value;
            }
        }

        /// <summary>
        /// Gets or sets contact list page index
        /// </summary>
        public int PageIndex
        {
            get
            {
                return gdvContactList.PageIndex;
            }

            set
            {
                gdvContactList.PageIndex = value;
            }
        }

        /// <summary>
        /// Gets or sets the Row index.
        /// </summary>
        public int RowIndex
        {
            get
            {
                return ViewState["RowIndex"] == null ? -1 : Convert.ToInt32(ViewState["RowIndex"]);
            }

            set
            {
                ViewState["RowIndex"] = value;
            }
        }

        /// <summary>
        /// Gets or sets the view id for contact list
        /// </summary>
        public string GViewID
        {
            get
            {
                return gdvContactList.GridViewNumber;
            }

            set
            {
                gdvContactList.GridViewNumber = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether of multiple contact form editable property.
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
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the smart choice validate flag for license
        /// </summary>
        public bool IsValidate
        {
            get
            {
                return _validate;
            }

            set
            {
                _validate = value;
            }
        }

        /// <summary>
        /// Gets or sets the data comes from.
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
            }
        }

        /// <summary>
        /// Gets or sets the focus element id.
        /// </summary>
        public string FocusElementId
        {
            get
            {
                if (ViewState["FocusElementId"] != null)
                {
                    return ViewState["FocusElementId"].ToString();
                }

                return string.Empty;
            }

            set
            {
                ViewState["FocusElementId"] = value;
            }
        }

        /// <summary>
        /// Gets the page size of GridView 
        /// </summary>
        public int PageSize
        {
            get
            {
                return gdvContactList.PageSize;
            }
        }

        /// <summary>
        /// Gets total count of current list. 
        /// </summary>
        public string CountSummary
        {
            get
            {
                return gdvContactList.CountSummary;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether enable delete action or not.
        /// </summary>
        public bool EnableDeleteAction
        {
            get
            {
                return _enableDeleteAction;
            }

            set
            {
                _enableDeleteAction = value;
            }
        }

        /// <summary>
        /// Gets a value indicating whether this instance is contact list exist template column.
        /// </summary>
        /// <value><c>true</c> if this instance is contact list exist template column; otherwise, <c>false</c>.</value>
        public bool IsExistTemplateColumn
        {
            get
            {
                return GridViewBuildHelper.IsExistTemplateColumn(gdvContactList);
            }
        }

        /// <summary>
        /// Gets all template attribute names.
        /// </summary>
        public string[] TemplateAttributeNames
        {
            get
            {
                return GridViewBuildHelper.GetTemplateAttributeNames(gdvContactList);
            }
        }

        /// <summary>
        /// Gets or sets ParentContainer
        /// </summary>
        public string ParentContainer
        {
            get; 
            set; 
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
        /// Gets or sets the current contact type.
        /// </summary>
        public string ContactType
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets a value indicating whether to show the Contact Detail form after Add New.
        /// </summary>
        public bool IsShowDetail { get; set; }

        /// <summary>
        /// Gets the ContactSectionPosition.
        /// </summary>
        public ACAConstant.ContactSectionPosition ContactSectionPosition
        {
            get
            {
                return Location == ContactListLocation.SpearForm
                           ? ACAConstant.ContactSectionPosition.SpearForm
                           : ACAConstant.ContactSectionPosition.None;
            }
        }

        /// <summary>
        /// Gets or sets the edit contact function name.
        /// </summary>
        public string EditContactFunction
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the function create contact session to open the edit form.
        /// </summary>
        public string CreateContactSessionFunction
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets a value indicating whether judge LP list position or not.
        /// </summary>
        public bool IsInCapConfirm
        {
            get;
            set;
        }

        /// <summary>
        /// Gets current SubAgencyCap type
        /// </summary>
        protected string IsSubAgencyCap
        {
            get { return Request.QueryString[ACAConstant.IS_SUBAGENCY_CAP]; }
        }
        #endregion Properties

        #region Methods

        /// <summary>
        /// Save the contact to the list
        /// </summary>
        /// <param name="contact">A CapContactModel4WS</param>
        /// <returns>A value indicating whether the action is update.</returns>
        public bool Save(CapContactModel4WS contact)
        {
            bool isUpdateAction = contact.people.RowIndex != null;

            // if new added row, the row index is the row count. if modified row, the row index is selected row index.
            int dataRowIndex = 0;

            if (!isUpdateAction)
            {
                if (DataSource != null && DataSource.Rows.Count > 0)
                {
                    foreach (DataRow dataRow in DataSource.Rows)
                    {
                        int rowIndex = Convert.ToInt32(dataRow[ColumnConstant.Contact.RowIndex.ToString()]);
                        dataRowIndex = dataRowIndex >= rowIndex ? dataRowIndex : rowIndex;
                    }

                    dataRowIndex++;
                }

                contact.people.RowIndex = dataRowIndex;
            }

            IPeopleBll peopleBll = ObjectFactory.GetObject<IPeopleBll>();
            DataRow drContact = peopleBll.ConvertContactModelToDataRow(contact);
            DataRow drNewContact = DataSource.NewRow();
            drNewContact.ItemArray = drContact.ItemArray;

            if (isUpdateAction)
            {
                int pos = ListUtil.FindPos(contact.people.RowIndex.Value, DataSource, ColumnConstant.Contact.RowIndex.ToString());
                DataSource.Rows.RemoveAt(pos);
                DataSource.Rows.InsertAt(drNewContact, pos);
            }
            else
            {
                DataSource.Rows.Add(drNewContact);
            }

            return isUpdateAction;
        }

        /// <summary>
        /// bind contact list by contact list data source.
        /// </summary>
        /// <param name="sortExpression">The sort expression.</param>
        public void BindContactList(string sortExpression = null)
        {
            DataTable dtMerged = GridViewBuildHelper.MergeTemplateAttributes2DataTable(gdvContactList, DataSource);

            if (!string.IsNullOrEmpty(sortExpression))
            {
                dtMerged.DefaultView.Sort = sortExpression;
                dtMerged = dtMerged.DefaultView.ToTable();
            }

            gdvContactList.DataSource = dtMerged;
            gdvContactList.EmptyDataText = AppSession.IsAdmin ? string.Empty : GetTextByKey("per_permitList_messagel_noRecord");
            gdvContactList.DataBind();
            ContactListPanel.Update();
            hfIsContactTypeNull.Value = string.Empty;

            if (DataSource != null && DataSource.Rows.Count > 0)
            {
                foreach (DataRow contactRow in DataSource.Rows)
                {
                    CapContactModel4WS contact = contactRow.Field<CapContactModel4WS>(ColumnConstant.Contact.CapContactModel.ToString());

                    // if contact type in contact list is null then will not do save and resume.
                    // Because contact type is db required,after doing save and resume with no contact type 
                    // when back form cap home the contact list is lost value with no contact type value. 
                    //The call function at capEdit.aspx Js function:saveAndResume.
                    if (contact != null && contact.people != null && string.IsNullOrEmpty(contact.people.contactType))
                    {
                        hfIsContactTypeNull.Value = ACAConstant.COMMON_Y;
                        break;
                    }
                }
            }
        }

        /// <summary>
        /// set the GridView required property
        /// </summary>
        /// <param name="isRequired">indicate if the GridView is required</param>
        public void SetGridViewRequired(bool isRequired)
        {
            gdvContactList.IsRequired = isRequired;
        }

        /// <summary>
        /// Merges the template field's data.
        /// </summary>
        /// <param name="dataSource">The original data source.</param>
        /// <returns>The data source that merged template data.</returns>
        public DataTable MergeTemplateData(DataTable dataSource)
        {
            return GridViewBuildHelper.MergeTemplateAttributes2DataTable(gdvContactList, dataSource);
        }

        /// <summary>
        /// GridView ContactList row command event method.
        /// </summary>
        /// <param name="sender">An object that contains the event sender.</param>
        /// <param name="e">A System.EventArgs object containing the event data.</param>
        protected void ContactList_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName.EndsWith(COMMAND_SELECT_CONTACT))
            {
                //select a contact to edit
                SelectContact(sender, e);

                if (string.IsNullOrEmpty(FocusElementId))
                {
                    e.FocusRowCellByName(e.CommandName.Replace(COMMAND_SELECT_CONTACT, string.Empty));
                }
                else
                {
                    ContactListPanel.FocusElement(FocusElementId);
                }
            }
            else if (e.CommandName == COMMAND_DELETE_CONTACT)
            {
                //delete a contact
                int dataItemIndex = Convert.ToInt32(e.CommandArgument);
                DeleteContact(sender, dataItemIndex);
            }
            else
            {
                BindContactList();
            }
        }

        /// <summary>
        /// <c>OnInit</c> event method.
        /// </summary>
        /// <param name="e">EventArgs e</param>
        protected override void OnInit(EventArgs e)
        {
            if (!AppSession.IsAdmin && StandardChoiceUtil.IsEnableExport2CSV() && Location == ContactListLocation.SearchResult)
            {
                gdvContactList.ShowExportLink = true;
                gdvContactList.ExportFileName = "Contact";
            }
            else
            {
                gdvContactList.ShowExportLink = false;
            }

            if (Location == ContactListLocation.SpearForm)
            {
                gdvContactList.IsInSPEARForm = true;
            }
            else
            {
                gdvContactList.IsInSPEARForm = false;

                // [Required Image Column] / [Delete Button Column], only shown in Spear Form
                foreach (DataControlField gridColumn in gdvContactList.Columns)
                {
                    AccelaTemplateField field = gridColumn as AccelaTemplateField;

                    if (field != null && (field.AttributeName == "RequiredImageColumn" || field.AttributeName == "lnkActionHeader"
                        || (!IsShowAddressCount && field.ColumnId == "AdditionalAddresses")))
                    {
                        field.Visible = false;
                    }
                }
            }

            GridViewBuildHelper.InitializeGridWithTemplate(gdvContactList, ModuleName, BizDomainConstant.STD_CAT_CONTACT_TYPE);

            base.OnInit(e);
        }

        /// <summary>
        /// Handles the grid view download event.
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void ContactList_GridViewDownload(object sender, GridViewDownloadEventArgs e)
        {
            BindContactList();

            // execute the download event that setting the exported content.
            if (GridViewDownloadAll != null)
            {
                GridViewDownloadAll(sender, e);
            }
        }

        /// <summary>
        /// fire sorted event.
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void ContactList_GridViewSort(object sender, AccelaGridViewSortEventArgs e)
        {
            if (GridViewSort != null)
            {
                DataSource = GridViewBuildHelper.MergeTemplateAttributes2DataTable(gdvContactList, DataSource);
                DataSource.DefaultView.Sort = e.GridViewSortExpression;
                DataSource = DataSource.DefaultView.ToTable();
                GridViewSort(sender, e);
            }
        }

        /// <summary>
        /// page index changing event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void ContactList_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            if (PageIndexChanging != null)
            {
                PageIndexChanging(sender, e);
            }
        }

        /// <summary>
        /// GridView ContactList row data bound event.
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">GridViewCommandEventArgs e</param>
        protected void ContactList_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                //if multiple contact form is read only or in confirm page,the delete link in gdvLicenseList is hiden.
                AccelaLinkButton lnkDelete = e.Row.FindControl("btnDelete") as AccelaLinkButton;

                // needn't add validation logic in search result list.
                DataRowView dv = (DataRowView)e.Row.DataItem;
                CapContactModel4WS capContactModel = dv[ColumnConstant.Contact.CapContactModel.ToString()] as CapContactModel4WS;

                if (capContactModel == null)
                {
                    return;
                }

                if ((!IsEditable || !EnableDeleteAction) && lnkDelete != null)
                {
                    lnkDelete.CssClass = "ACA_Hide";
                }

                //alert a confirm message when delete a record.
                if (lnkDelete != null)
                {
                    string deleteMessage = GetTextByKey("aca_common_delete_alert_message").Replace("'", "\\'");
                    CapModel4WS capModel = AppSession.GetCapModelFromSession(ModuleName);

                    if (ValidationUtil.IsYes(capContactModel.people.flag)
                        && ContactUtil.IsPrimaryContactHasLCData(capModel, capContactModel.refContactNumber))
                    {
                        deleteMessage = GetTextByKey("aca_capedit_contact_msg_removeprimarycontact").Replace("'", "\\'");
                    }

                    lnkDelete.Attributes.Add("onclick", string.Format("javascript:return confirm('{0}')", deleteMessage));
                }

                AccelaLabel lblCountry = (AccelaLabel)e.Row.FindControl("lblCountry");
                AccelaDiv divImg = (AccelaDiv)e.Row.FindControl("divImg");

                if (lblCountry != null)
                {
                    lblCountry.Text = StandardChoiceUtil.GetCountryByKey(lblCountry.Text);
                }

                if (gdvContactList.IsInSPEARForm)
                {
                    bool isContactDataSourceNoLimitation = ComponentDataSource.NoLimitation.Equals(ValidateFlag, StringComparison.OrdinalIgnoreCase);
                    CapContactModel4WS[] contactModels = { capContactModel };
                    
                    if (!IsEditable)
                    {
                        divImg.Visible = !CapUtil.ValidateTemplateFields(IsValidate, IsEditable, contactModels);
                    }
                    else
                    {
                        //Validate all of required fields have value for each row in multiple contact list.
                        if ((CapUtil.IsNeedValidateContacts(IsValidate, IsEditable, contactModels)
                                && (!RequiredValidationUtil.ValidateFields4ContactList(ModuleName, GviewID.ContactEdit, contactModels, ACAConstant.ContactSectionPosition.SpearForm)
                                    || (!FormatValidationUtil.ValidateFormat4ContactList(ModuleName, GviewID.ContactEdit, contactModels, isContactDataSourceNoLimitation, ACAConstant.ContactSectionPosition.SpearForm))
                                    || (StandardChoiceUtil.IsEnableContactAddress() 
                                        && !RequiredValidationUtil.ValidateContactAddressType(contactModels))))
                            || (IsValidate && string.IsNullOrEmpty(capContactModel.refContactNumber))
                            || (StandardChoiceUtil.IsEnableContactAddress()
                                && (RequiredValidationUtil.IsNeedValidateCAPrimary(IsEditable, ACAConstant.ContactSectionPosition.SpearForm, capContactModel.people.contactType, capContactModel.people.contactAddressList)
                                    || (string.IsNullOrEmpty(capContactModel.refContactNumber) && !string.IsNullOrWhiteSpace(RequiredValidationUtil.ValidateContactAddressType(capContactModel.people.contactType, capContactModel.people.contactAddressList))))))
                        {
                            divImg.Visible = true;
                        }
                        else
                        {
                            divImg.Visible = false;
                        }
                    }
                }

                string countryCode = capContactModel != null && capContactModel.people != null
                                     && capContactModel.people.compactAddress != null
                                         ? capContactModel.people.compactAddress.countryCode
                                         : string.Empty;
                FormatPhoneShow((HiddenField)e.Row.FindControl("hdnHomePhoneCode"), (AccelaLabel)e.Row.FindControl("lblHomePhone"), countryCode);
                FormatPhoneShow((HiddenField)e.Row.FindControl("hdnWorkPhoneCode"), (AccelaLabel)e.Row.FindControl("lblWorkPhone"), countryCode);
                FormatPhoneShow((HiddenField)e.Row.FindControl("hdnMobilePhoneCode"), (AccelaLabel)e.Row.FindControl("lblMobilePhone"), countryCode);
                FormatPhoneShow((HiddenField)e.Row.FindControl("hdnFaxCode"), (AccelaLabel)e.Row.FindControl("lblFax"), countryCode);

                //If hiden the contact permission in contact edit must be set the "" value.
                GFilterScreenPermissionModel4WS formDesign =
                    ControlBuildHelper.GetPermissionWithGenericTemplate(GviewID.ContactEdit, GViewConstant.PERMISSION_PEOPLE, capContactModel.people.contactType, capContactModel.people.template);
                SimpleViewElementModel4WS[] models = ModelUIFormat.GetSimpleViewElementModelBySectionID(ModuleName, formDesign, GviewID.ContactEdit);
                IGviewBll gviewBll = ObjectFactory.GetObject<IGviewBll>();

                if (!gviewBll.IsFieldVisible(models, "radioListContactPermission"))
                {
                    AccelaLabel lblPermission = (AccelaLabel)e.Row.FindControl("lblContactPermission");

                    if (lblPermission != null)
                    {
                        lblPermission.Text = string.Empty;
                    }
                }

                AccelaLabel lblPreferredChannel = e.Row.FindControl("lblPreferredChannel") as AccelaLabel;

                if (dv[ColumnConstant.Contact.PreferredChannel.ToString()] != null)
                {
                    lblPreferredChannel.Text = StandardChoiceUtil.GetStandardChoiceValueByKey(BizDomainConstant.STD_CONTACT_PREFERRED_CHANNEL, dv[ColumnConstant.Contact.PreferredChannel.ToString()].ToString());
                }
            }
        }

        /// <summary>
        /// Format phone show
        /// </summary>
        /// <param name="hfnPhoneCode">control for phone code.</param>
        /// <param name="lblPhone">control for phone.</param>
        /// <param name="countryCode">The LBL country code.</param>
        private void FormatPhoneShow(HiddenField hfnPhoneCode, AccelaLabel lblPhone, string countryCode)
        {
            string homePhoneCode = hfnPhoneCode != null ? hfnPhoneCode.Value : string.Empty;
            string homePhone = lblPhone != null ? lblPhone.Text : string.Empty;

            if (lblPhone != null)
            {
                lblPhone.Text = ModelUIFormat.FormatPhoneShow(homePhoneCode, homePhone, countryCode);
            }
        }

        /// <summary>
        /// Select a contact and raise contactSelected event
        /// </summary>
        /// <param name="sender">the sender object.</param>
        /// <param name="e">A System.EventArgs object containing the event data.</param>
        private void SelectContact(object sender, GridViewCommandEventArgs e)
        {
            //Raise ContactSelected event
            int dataItemIndex = Convert.ToInt32(e.CommandArgument);
            DataRow drContact = DataSource.Rows[dataItemIndex];

            if (drContact[ColumnConstant.Contact.RowIndex.ToString()] != DBNull.Value)
            {
                RowIndex = Convert.ToInt32(drContact[ColumnConstant.Contact.RowIndex.ToString()]);
            }

            CapContactModel4WS capContactModel = (CapContactModel4WS)drContact[ColumnConstant.Contact.CapContactModel.ToString()];

            PeopleUtil.SaveTempContact(capContactModel.people);

            if (IsInCapConfirm)
            {
                ComponentModel searchComponentModel = new ComponentModel();
                searchComponentModel.componentID = (long)PageFlowComponent.CONTACT_LIST;
                searchComponentModel.componentSeqNbr = Convert.ToInt64(capContactModel.componentName.Split('_')[1]);

                string sectionName = capContactModel.componentName;
                PageFlowGroupModel pageflowGroup = AppSession.GetPageflowGroupFromSession();
                CapUtil.BackToPageContainSection(searchComponentModel, sectionName, pageflowGroup, RowIndex.ToString());
            }
            else
            {
                if (!ACAConstant.CAP_HOME_PAGE.Equals(ParentContainer, StringComparison.InvariantCultureIgnoreCase))
                {
                    AccelaLinkButton lnkEdit = e.CommandSource as AccelaLinkButton;
                    string clientScript = string.Format("SetLastFocus('{4}');{0}({1}, '{2}','',{3});", CreateContactSessionFunction, ContactProcessType.Edit.ToString("D"), capContactModel.people.RowIndex, EditContactFunction, lnkEdit.ClientID);
                    ScriptManager.RegisterStartupScript(Page, GetType(), ComponentName, clientScript, true);
                }

                if (ContactSelected != null)
                {
                    object[] args = new object[] { RowIndex, capContactModel };
                    ContactSelected(sender, new CommonEventArgs(args));
                }
            }
        }

        /// <summary>
        /// Remove a contact from contact list and raise the contacts changed event
        /// </summary>
        /// <param name="sender">the sender object.</param>
        /// <param name="dataItemIndex">the index a contact record in contact list</param>
        private void DeleteContact(object sender, int dataItemIndex)
        {
            if (DataSource != null)
            {
                DataRow drContact = DataSource.Rows[dataItemIndex];
                CapContactModel4WS capContactModel = (CapContactModel4WS)drContact[ColumnConstant.Contact.CapContactModel.ToString()];

                DataTable dtContacts = DataSource as DataTable;
                dtContacts.Rows.RemoveAt(dataItemIndex);
                ListUtil.UpdateRowIndex(dtContacts, ColumnConstant.Contact.RowIndex.ToString());
                dtContacts.AcceptChanges();
                DataSource = dtContacts;
                BindContactList();

                if (ContactsDeleted != null)
                {
                    ContactsDeleted(sender, new CommonEventArgs(capContactModel));
                }

                Page.SetFocus("lnkAddContact");
            }
        }

        #endregion Methods
    }
}
