#region Header

/**
* <pre>
*  Accela Citizen Access
*  File: RefContactList.ascx.cs
*
*  Accela, Inc.
*  Copyright (C): 2012-2014
*
*  Description: An edit form for generic template fields.
*
*  Notes:
* $Id: RefContactList.ascx.cs 278148 2014-08-28 07:30:09Z ACHIEVO\james.shi $.
*  Revision History
*  Date,            Who,        What
*  Sep 20, 2011      Alan Hu     Initial.
* </pre>
*/

#endregion

using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using Accela.ACA.BLL.Account;
using Accela.ACA.BLL.People;
using Accela.ACA.Common;
using Accela.ACA.Common.Util;
using Accela.ACA.UI.Model;
using Accela.ACA.Web.Common;
using Accela.ACA.Web.Common.Control;
using Accela.ACA.Web.Util;
using Accela.ACA.WSProxy;
using Accela.Web.Controls;

namespace Accela.ACA.Web.Component
{
    /// <summary>
    /// RefContactList Control.
    /// </summary>
    public partial class RefContactList : BaseUserControl
    {
        #region Fields

        /// <summary>
        /// Command name .
        /// </summary>
        private const string COMMAND_SELECT_CONTACT = "SelectContact";

        #endregion

        #region Events

        /// <summary>
        /// grid view row command event.
        /// </summary>
        public event CommonEventHandler ContactSelected;

        /// <summary>
        /// grid view page index changing event.
        /// </summary>
        public event GridViewPageEventHandler PageIndexChanging;

        /// <summary>
        /// grid view sorted event.
        /// </summary>
        public event GridViewSortedEventHandler GridViewSort;

        #endregion Events

        #region Properties

        /// <summary>
        /// Gets or sets the view id for reference contact list
        /// </summary>
        public string GViewID
        {
            get
            {
                return gdvRefContactList.GridViewNumber;
            }

            set
            {
                gdvRefContactList.GridViewNumber = value;
            }
        }

        /// <summary>
        /// Gets or sets data source
        /// </summary>
        public IList<PeopleModel> RefContactsDataSource
        {
            get
            {
                if (ViewState["DataSource"] == null)
                {
                    ViewState["DataSource"] = new List<PeopleModel>();
                }

                return (IList<PeopleModel>)ViewState["DataSource"];
            }

            set
            {
                ViewState["DataSource"] = value;
            }
        }

        /// <summary>
        /// Gets the contact list.
        /// </summary>
        public AccelaGridView GViewContactList
        {
            get
            {
                return gdvRefContactList;
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
        /// Gets or sets the contact section position.
        /// </summary>
        public ACAConstant.ContactSectionPosition ContactSectionPosition
        {
            get;
            set;
        }

        #endregion

        #region Methods

        /// <summary>
        /// bind data list.
        /// </summary>
        public void BindRefContactsList()
        {
            DataTable dt = GridViewBuildHelper.MergeTemplateAttributes2DataTable(gdvRefContactList, RefContactsDataSource, "attributes");

            //Extract the data table for Child Model.
            dt = ObjectConvertUtil.ExtractChildModel(dt, "compactAddress");
            gdvRefContactList.EmptyDataText = AppSession.IsAdmin ? string.Empty : GetTextByKey("aca_contactlist_msg_norecordfound");
            gdvRefContactList.DataSource = dt;
            gdvRefContactList.DataBind();
        }

        /// <summary>
        /// <c>OnInit</c> event method.
        /// </summary>
        /// <param name="e">EventArgs e</param>
        protected override void OnInit(EventArgs e)
        {
            if (!IsPostBack)
            {
                if (!AppSession.IsAdmin && StandardChoiceUtil.IsEnableExport2CSV() 
                    && (GViewID == GviewID.AccountReferenceContactList || GViewID == GviewID.AuthAgentSearchList))
                {
                    //Only show export function in account management page.
                    gdvRefContactList.ShowExportLink = true;
                    gdvRefContactList.ExportFileName = "ReferenceContactList";
                }
                else
                {
                    gdvRefContactList.ShowExportLink = false;
                }
            }

            GridViewBuildHelper.InitializeGridWithTemplate(gdvRefContactList, ModuleName, BizDomainConstant.STD_CAT_CONTACT_TYPE);
            base.OnInit(e);
        }

        /// <summary>
        /// Raises the page load event
        /// </summary>
        /// <param name="sender">An object that contains the event sender.</param>
        /// <param name="e">A System.EventArgs object containing the event data.</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            //when export CSV form request, it need Re-bind GV.
            if (IsPostBack && Request.Form[Page.postEventSourceID] != null && Request.Form[Page.postEventSourceID].IndexOf("btnExport") > -1)
            {
                BindRefContactsList();
            }
        }

        /// <summary>
        /// OnPreRender event method.
        /// </summary>
        /// <param name="e">EventArgs e</param>
        protected override void OnPreRender(EventArgs e)
        {
            ChangeLabelKey();

            base.OnPreRender(e);
        }

        /// <summary>
        /// Select a reference record to fill contact section.
        /// </summary>
        /// <param name="sender">Object sender</param>
        /// <param name="e">GridView Command Event argument</param>
        protected void RefContactList_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName.EndsWith(COMMAND_SELECT_CONTACT))
            {
                string contactSeqNumber = e.CommandArgument.ToString();
                SelectContact(sender, contactSeqNumber);
            }
            else
            {
                BindRefContactsList();
            }
        }

        /// <summary>
        /// Response permit GridView page index changing event to record the latest page index.
        /// </summary>
        /// <param name="sender">Object sender</param>
        /// <param name="e">A System.EventArgs object containing the event data.</param>
        protected void RefContactList_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            if (PageIndexChanging != null)
            {
                PageIndexChanging(sender, e);
            }
        }

        /// <summary>
        /// Response permit GridView sorted event to record the latest sort expression.
        /// </summary>
        /// <param name="sender">Object sender</param>
        /// <param name="e">A System.EventArgs object containing the event data.</param>
        protected void RefContactList_GridViewSort(object sender, AccelaGridViewSortEventArgs e)
        {
            if (GridViewSort != null)
            {
                GridViewSort(sender, e);
            }
        }

        /// <summary>
        /// Removes the selected license.
        /// </summary>
        /// <param name="sender">Object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void RemoveContactItemLink_OnClick(object sender, EventArgs e)
        {
            LinkButton lbndel = (LinkButton)sender;
            string contactSeqNbr = lbndel.Attributes["SeqNbr"];
            IPeopleBll peopleBll = (IPeopleBll)ObjectFactory.GetObject(typeof(IPeopleBll));
            peopleBll.RemoveContact4publicUser(long.Parse(AppSession.User.UserSeqNum), long.Parse(contactSeqNbr));
            AppSession.ReloadPublicUserSession();

            string script = string.Format(
                @"{0}_RefreshContactList();
                  if (typeof (RefreshAttachmentList) != 'undefined'){{RefreshAttachmentList();}}",
                ClientID);

            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "RefreshContactListAndAttachmentList", script, true);
        }

        /// <summary>
        /// set primary contact for public user
        /// </summary>
        /// <param name="sender">Object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void SetAccountOwnerLink_OnClick(object sender, EventArgs e)
        {
            LinkButton btnSetPrimary = (LinkButton)sender;
            string contactSeqNbr = btnSetPrimary.Attributes["SeqNbr"];
            IPeopleBll peopleBll = ObjectFactory.GetObject<IPeopleBll>();
            peopleBll.SetAccountOwner4PublicUser(long.Parse(AppSession.User.UserSeqNum), long.Parse(contactSeqNbr));

            AppSession.ReloadPublicUserSession();

            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "RefreshContactList", ClientID + "_RefreshContactList();", true);
        }

        /// <summary>
        /// Sync contact data when use SSO/LDAP login.
        /// </summary>
        /// <param name="sender">Object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void SyncContactDataButton_OnClick(object sender, EventArgs e)
        {
            string contactSeqNbr = Request.Form[Page.postEventArgumentID];
            PeopleModel4WS people = AppSession.User.UserModel4WS.peopleModel.Where(c => contactSeqNbr.Equals(c.contactSeqNumber, StringComparison.OrdinalIgnoreCase)).SingleOrDefault();

            if (people != null)
            {
                IAccountBll accountBll = (IAccountBll)ObjectFactory.GetObject(typeof(IAccountBll));
                IPeopleBll peopleBll = (IPeopleBll)ObjectFactory.GetObject(typeof(IPeopleBll));

                accountBll.SetAccountInfoToPeopleModel(AppSession.User.UserModel4WS, ref people);
                PeopleUtil.SetPeopleTemplateContactSeqNum(people);

                /*
                 * RefContactEditBefore and RefContactEditAfter event will be triggered 
                 *  in PeopleService.addContact4PublicUser and PeopleService.editRefContact.
                 */
                try
                {
                    peopleBll.EditRefContact(TempModelConvert.ConvertToPeopleModel(people));
                }
                catch (Exception exception)
                {
                    MessageUtil.ShowMessage(this, MessageType.Error, exception.Message);
                }

                AppSession.ReloadPublicUserSession();

                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "RefreshContactList", ClientID + "_RefreshContactList();", true);
            }
        }

        /// <summary>
        /// GridView LicenseList row data bound event.
        /// </summary>
        /// <param name="sender">Object sender</param>
        /// <param name="e">GridViewCommandEventArgs e</param>
        protected void RefContactList_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                DataRowView rowView = (DataRowView)e.Row.DataItem;
                BuildActionMenu(e);
                LinkButton btnFullName = e.Row.FindControl("btnFullName") as LinkButton;
                AccelaLabel lblFullName = e.Row.FindControl("lblFullName") as AccelaLabel;

                if (rowView["fullName"] == null || string.IsNullOrEmpty(rowView["fullName"].ToString()))
                {
                    string firstName = rowView["firstName"] == null ? string.Empty : rowView["firstName"].ToString();
                    string middleName = rowView["middleName"] == null ? string.Empty : rowView["middleName"].ToString();
                    string lastName = rowView["lastName"] == null ? string.Empty : rowView["lastName"].ToString();
                    string fullName = DataUtil.ConcatStringWithSplitChar(new string[] { firstName, middleName, lastName }, ACAConstant.BLANK);
                    btnFullName.Text = fullName;
                    lblFullName.Text = fullName;
                }
                else
                {
                    btnFullName.Text = rowView["fullName"].ToString();
                    lblFullName.Text = rowView["fullName"].ToString();
                }

                if (IsForView)
                {
                    LinkButton btnFirstName = e.Row.FindControl("btnFirstName") as LinkButton;
                    AccelaLabel lblFirstName = e.Row.FindControl("lblFirstName") as AccelaLabel;
                    btnFirstName.Visible = false;
                    lblFirstName.Visible = true;

                    LinkButton btnMiddleName = e.Row.FindControl("btnMiddleName") as LinkButton;
                    AccelaLabel lblMiddleName = e.Row.FindControl("lblMiddleName") as AccelaLabel;
                    btnMiddleName.Visible = false;
                    lblMiddleName.Visible = true;

                    LinkButton btnLastName = e.Row.FindControl("btnLastName") as LinkButton;
                    AccelaLabel lblLastName = e.Row.FindControl("lblLastName") as AccelaLabel;
                    btnLastName.Visible = false;
                    lblLastName.Visible = true;

                    btnFullName.Visible = false;
                    lblFullName.Visible = true;

                    LinkButton btnBusiness = e.Row.FindControl("btnBusiness") as LinkButton;
                    AccelaLabel lblBusiness = e.Row.FindControl("lblBusiness") as AccelaLabel;
                    btnBusiness.Visible = false;
                    lblBusiness.Visible = true;

                    LinkButton btnContactType = e.Row.FindControl("btnContactType") as LinkButton;
                    AccelaLabel lblContactType = e.Row.FindControl("lblContactType") as AccelaLabel;
                    btnContactType.Visible = false;
                    lblContactType.Visible = true;
                }

                AccelaLabel lblStatus = e.Row.FindControl("lblStatus") as AccelaLabel;

                string relateStatus = rowView["contractorPeopleStatus"].ToString();
                string labelKey = "aca_label_contractor_people_status_approved";

                switch (relateStatus)
                {
                    case ContractorPeopleStatus.Pending:
                        labelKey = "aca_label_contractor_people_status_pending";
                        break;
                    case ContractorPeopleStatus.Rejected:
                        labelKey = "aca_label_contractor_people_status_rejected";
                        break;
                }

                lblStatus.Text = GetTextByKey(labelKey);

                AccelaLabel lblPreferredChannel = e.Row.FindControl("lblPreferredChannel") as AccelaLabel;

                if (rowView["preferredChannel"] != null)
                {
                    lblPreferredChannel.Text = StandardChoiceUtil.GetStandardChoiceValueByKey(BizDomainConstant.STD_CONTACT_PREFERRED_CHANNEL, rowView["preferredChannel"].ToString());
                }

                AccelaLabel lblAccountOwner = e.Row.FindControl("lblAccountOwner") as AccelaLabel;
                lblAccountOwner.Text = rowView["accountOwner"] != null && ValidationUtil.IsYes(rowView["accountOwner"].ToString())
                                           ? GetTextByKey("ACA_Common_Yes")
                                           : GetTextByKey("ACA_Common_No");
            }
        }

        /// <summary>
        /// Select a contact and raise contactSelected event
        /// </summary>
        /// <param name="sender">The sender object.</param>
        /// <param name="contactSeqNumber">Contact Sequence Number</param>
        private void SelectContact(object sender, string contactSeqNumber)
        {
            //Raise ContactSelected event
            if (ContactSelected != null)
            {
                if (RefContactsDataSource == null || RefContactsDataSource.Count == 0)
                {
                    return;
                }

                PeopleModel people = RefContactsDataSource.Where(c => string.Equals(c.contactSeqNumber, contactSeqNumber, StringComparison.Ordinal)).SingleOrDefault();
                ContactSelected(sender, new CommonEventArgs(people));
            }
        }

        /// <summary>
        /// Bind action menu.
        /// </summary>
        /// <param name="eventArgs">GridView Row Event Arguments</param>
        private void BuildActionMenu(GridViewRowEventArgs eventArgs)
        {
            DataRowView rowView = (DataRowView)eventArgs.Row.DataItem;
            PopupActions actionMenu = eventArgs.Row.FindControl("actionMenu") as PopupActions;
            LinkButton btnRemoveContact = eventArgs.Row.FindControl("btnRemoveContactItem") as LinkButton;
            LinkButton btnSetAccountOwner = eventArgs.Row.FindControl("btnSetAccountOwner") as LinkButton;

            ActionViewModel actionView;
            string contactType = rowView["contactType"].ToString();
            string contactSeqNbr = rowView["contactSeqNumber"].ToString();
            string agency = rowView["serviceProviderCode"] != null ? rowView["serviceProviderCode"].ToString() : string.Empty;
            var actionList = new List<ActionViewModel>();

            if (!string.IsNullOrEmpty(contactSeqNbr) && !string.IsNullOrEmpty(contactType))
            {
                //View details action
                actionView = new ActionViewModel();
                actionView.ActionLabel = GetTextByKey("aca_refcontactlist_label_action_view");
                actionView.IcoUrl = ImageUtil.GetImageURL("popaction_view.png");
                actionView.ClientEvent = string.Format("return ShowAccountContactEdit('{0}','{1}');", "edit", contactSeqNbr);
                actionList.Add(actionView);

                bool isShowRemoveButton = false;

                if (btnRemoveContact != null && AppSession.User.ApprovedContacts != null)
                {
                    string relateStatus = rowView["contractorPeopleStatus"].ToString();

                    //the current agency contact should not be delete while is only one.
                    int appovedContactNum = AppSession.User.ApprovedContacts.Count(o => o.serviceProviderCode.Equals(ConfigManager.AgencyCode));

                    /*
                     * A valid registered user must associated at least one approved reference contact.
                     * If current contact isn't in Approved status, or user have more than one approved contacts, the associated contact can be remove.
                     */
                    if (ContractorPeopleStatus.Pending.Equals(relateStatus, StringComparison.OrdinalIgnoreCase)
                        || ContractorPeopleStatus.Rejected.Equals(relateStatus, StringComparison.OrdinalIgnoreCase)
                        || ((string.IsNullOrEmpty(relateStatus) || ContractorPeopleStatus.Approved.Equals(relateStatus, StringComparison.OrdinalIgnoreCase))
                                && appovedContactNum > 1))
                    {
                        isShowRemoveButton = true;
                    }
                }

                //Delete action
                if (isShowRemoveButton && agency.Equals(ConfigManager.AgencyCode, StringComparison.OrdinalIgnoreCase))
                {
                    btnRemoveContact.Attributes.Add("SeqNbr", contactSeqNbr);
                    actionView = new ActionViewModel();
                    actionView.ActionLabel = GetTextByKey("aca_refcontactlist_label_action_remove");
                    actionView.IcoUrl = ImageUtil.GetImageURL("popaction_delete.png");
                    actionView.ClientEvent = string.Format("return RemoveContact('{0}');", btnRemoveContact.UniqueID);
                    actionList.Add(actionView);
                }

                if (((AuthenticationUtil.IsAuthAdapter && !AuthenticationUtil.IsInternalAuthAdapter) || StandardChoiceUtil.IsEnableLdapAuthentication())
                    && agency.Equals(ConfigManager.AgencyCode, StringComparison.OrdinalIgnoreCase))
                {
                    actionView = new ActionViewModel();
                    actionView.ActionLabel = GetTextByKey("aca_refcontactlist_label_action_syncdata");
                    actionView.IcoUrl = ImageUtil.GetImageURL("popaction_update.png");
                    actionView.ClientEvent = string.Format("return SyncContactData('{0}');", contactSeqNbr);
                    actionList.Add(actionView);
                }

                // set as account owner
                string isAccountOwner = rowView["accountOwner"] as string;

                if (btnSetAccountOwner != null
                    && AppSession.User.ApprovedContacts != null
                    && agency.Equals(ConfigManager.AgencyCode, StringComparison.OrdinalIgnoreCase)
                    && ContractorPeopleStatus.Approved.Equals(rowView["contractorPeopleStatus"] as string, StringComparison.OrdinalIgnoreCase)
                    && !ValidationUtil.IsYes(isAccountOwner))
                {
                    btnSetAccountOwner.Attributes.Add("SeqNbr", contactSeqNbr);
                    actionView = new ActionViewModel();
                    actionView.ActionLabel = GetTextByKey("aca_refcontactlist_label_setaccountowner");
                    actionView.IcoUrl = ImageUtil.GetImageURL("popaction_set_as_primary.png");
                    actionView.ClientEvent = string.Format("return SetAccountOwner('{0}');", btnSetAccountOwner.UniqueID);
                    actionList.Add(actionView);
                }
            }

            if (actionList.Count > 0)
            {
                actionMenu.Visible = true;
                actionMenu.ActionLableKey = "aca_refcontactlist_label_actionlink";
                actionMenu.AvailableActions = actionList.ToArray();
                actionMenu.BindListAction();
            }
        }

        /// <summary>
        /// change the label key by the view id
        /// </summary>
        private void ChangeLabelKey()
        {
            if (GViewID == GviewID.AuthAgentSearchList)
            {
                GridViewRow headerRow = GridViewBuildHelper.GetHeaderRow(gdvRefContactList);

                if (headerRow == null)
                {
                    return;
                }

                ((IAccelaNonInputControl)headerRow.FindControl("lnkContactTypeFlag")).LabelKey = "aca_authagent_customersearchlist_label_contacttypeflag";
                ((IAccelaNonInputControl)headerRow.FindControl("lnkSalutation")).LabelKey = "aca_authagent_customersearchlist_label_salutation";
                ((IAccelaNonInputControl)headerRow.FindControl("lnkTitle")).LabelKey = "aca_authagent_customersearchlist_label_title";
                ((IAccelaNonInputControl)headerRow.FindControl("lnkFirstName")).LabelKey = "aca_authagent_customersearchlist_label_firstname";
                ((IAccelaNonInputControl)headerRow.FindControl("lnkMiddleName")).LabelKey = "aca_authagent_customersearchlist_label_middlename";
                ((IAccelaNonInputControl)headerRow.FindControl("lnkLastName")).LabelKey = "aca_authagent_customersearchlist_label_lastname";
                ((IAccelaNonInputControl)headerRow.FindControl("lnkFullName")).LabelKey = "aca_authagent_customersearchlist_label_fullname";
                ((IAccelaNonInputControl)headerRow.FindControl("lnkBirthDate")).LabelKey = "aca_authagent_customersearchlist_label_birthdate";
                ((IAccelaNonInputControl)headerRow.FindControl("lnkGender")).LabelKey = "aca_authagent_customersearchlist_label_gender";
                ((IAccelaNonInputControl)headerRow.FindControl("lnkBusiness")).LabelKey = "aca_authagent_customersearchlist_label_business";
                ((IAccelaNonInputControl)headerRow.FindControl("lnkTradeName")).LabelKey = "aca_authagent_customersearchlist_label_tradename";
                ((IAccelaNonInputControl)headerRow.FindControl("lnkSSN")).LabelKey = "aca_authagent_customersearchlist_label_ssn";
                ((IAccelaNonInputControl)headerRow.FindControl("lnkFein")).LabelKey = "aca_authagent_customersearchlist_label_fein";
                ((IAccelaNonInputControl)headerRow.FindControl("lnkContactType")).LabelKey = "aca_authagent_customersearchlist_label_contacttype";
                ((IAccelaNonInputControl)headerRow.FindControl("lnkAddressLine1")).LabelKey = "aca_authagent_customersearchlist_label_address1";
                ((IAccelaNonInputControl)headerRow.FindControl("lnkAddressLine2")).LabelKey = "aca_authagent_customersearchlist_label_address2";
                ((IAccelaNonInputControl)headerRow.FindControl("lnkAddressLine3")).LabelKey = "aca_authagent_customersearchlist_label_address3";
                ((IAccelaNonInputControl)headerRow.FindControl("lnkCity")).LabelKey = "aca_authagent_customersearchlist_label_city";
                ((IAccelaNonInputControl)headerRow.FindControl("lnkState")).LabelKey = "aca_authagent_customersearchlist_label_state";
                ((IAccelaNonInputControl)headerRow.FindControl("lnkZip")).LabelKey = "aca_authagent_customersearchlist_label_zip";
                ((IAccelaNonInputControl)headerRow.FindControl("lnkPOBox")).LabelKey = "aca_authagent_customersearchlist_label_pobox";
                ((IAccelaNonInputControl)headerRow.FindControl("lnkCountry")).LabelKey = "aca_authagent_customersearchlist_label_country";
                ((IAccelaNonInputControl)headerRow.FindControl("lnkHomePhone")).LabelKey = "aca_authagent_customersearchlist_label_homephone";
                ((IAccelaNonInputControl)headerRow.FindControl("lnkWorkPhone")).LabelKey = "aca_authagent_customersearchlist_label_workphone";
                ((IAccelaNonInputControl)headerRow.FindControl("lnkMobilePhone")).LabelKey = "aca_authagent_customersearchlist_label_mobilephone";
                ((IAccelaNonInputControl)headerRow.FindControl("lnkFax")).LabelKey = "aca_authagent_customersearchlist_label_fax";
                ((IAccelaNonInputControl)headerRow.FindControl("lnkEmail")).LabelKey = "aca_authagent_customersearchlist_label_email";
                ((IAccelaNonInputControl)headerRow.FindControl("lnkBusinessName2")).LabelKey = "aca_authagent_customersearchlist_label_businessname2";
                ((IAccelaNonInputControl)headerRow.FindControl("lnkBirthplaceCity")).LabelKey = "aca_authagent_customersearchlist_label_birthplacecity";
                ((IAccelaNonInputControl)headerRow.FindControl("lnkBirthplaceState")).LabelKey = "aca_authagent_customersearchlist_label_birthplacestate";
                ((IAccelaNonInputControl)headerRow.FindControl("lnkBirthCountry")).LabelKey = "aca_authagent_customersearchlist_label_birthcountry";
                ((IAccelaNonInputControl)headerRow.FindControl("lnkRace")).LabelKey = "aca_authagent_customersearchlist_label_race";
                ((IAccelaNonInputControl)headerRow.FindControl("lnkDeceasedDate")).LabelKey = "aca_authagent_customersearchlist_label_deceaseddate";
                ((IAccelaNonInputControl)headerRow.FindControl("lnkPassportNumber")).LabelKey = "aca_authagent_customersearchlist_label_passportnumber";
                ((IAccelaNonInputControl)headerRow.FindControl("lnkDriverLicenseNumber")).LabelKey = "aca_authagent_customersearchlist_label_driverlicensenumber";
                ((IAccelaNonInputControl)headerRow.FindControl("lnkDriverLicenseState")).LabelKey = "aca_authagent_customersearchlist_label_driverlicensestate";
                ((IAccelaNonInputControl)headerRow.FindControl("lnkStateIdNumber")).LabelKey = "aca_authagent_customersearchlist_label_stateidnumber";
                ((IAccelaNonInputControl)headerRow.FindControl("lnkComment")).LabelKey = "aca_authagent_customersearchlist_label_comment";
                ((IAccelaNonInputControl)headerRow.FindControl("lnkPreferredChannel")).LabelKey = "aca_authagent_customersearchlist_label_preferredchannel";
            }
        }

        #endregion
    }
}