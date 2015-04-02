#region Header

/**
* <pre>
*  Accela Citizen Access
*  File: ContactSearchList.ascx.cs
*
*  Accela, Inc.
*  Copyright (C): 2013-2014
*
*  Description: An edit form for generic template fields.
*
*  Notes:
* $Id: ContactSearchList.ascx.cs 258693 2013-10-21 09:43:59Z ACHIEVO\daniel.shi $.
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
using System.Web.UI.WebControls;

using Accela.ACA.Common;
using Accela.ACA.Common.Util;
using Accela.ACA.Web.Common;
using Accela.ACA.Web.Common.Control;
using Accela.ACA.WSProxy;
using Accela.Web.Controls;

namespace Accela.ACA.Web.Component
{
    /// <summary>
    /// contact search list control
    /// </summary>
    public partial class ContactSearchList : BaseUserControl
    {
        #region Fields

        /// <summary>
        /// indicate the default index value  when no contact is selected in the contact list
        /// </summary>
        public const int NO_CONTACT_SELECTED = -1;

        /// <summary>
        /// The search type.
        /// </summary>
        private ContactProcessType _contactSearchType = ContactProcessType.Lookup;

        /// <summary>
        /// The contact section position
        /// </summary>
        private ACAConstant.ContactSectionPosition _contactSectionPosition = ACAConstant.ContactSectionPosition.None;
        
		#endregion

        /// <summary>
        /// grid view page index changing event.
        /// </summary>
        public event GridViewPageEventHandler PageIndexChanging;

        /// <summary>
        /// Gets the page size of GridView 
        /// </summary>
        public int PageSize
        {
            get
            {
                return gdvSearchContactList.PageSize;
            }
        }

        /// <summary>
        /// Sets the selected row index
        /// </summary>
        public int? SelectedRowIndex
        {
            set
            {
                // set the radio button as checked.
                if (value.HasValue && value >= 0)
                {
                    GViewContactList.SelectRow(GViewContactList.Rows[value.Value]);
                }
            }
        }

        /// <summary>
        /// Gets data comes from
        /// </summary>
        public string ValidateFlag
        {
            get 
            { 
                return Request.QueryString[UrlConstant.ValidateFlag] ?? string.Empty; 
            }
        }

        /// <summary>
        /// Gets the selected item
        /// </summary>
        public PeopleModel SelectedItem
        {
            get
            {
                List<int> indexs = GViewContactList.GetSelectedRowIndexes();

                if (indexs != null && indexs.Count > 0)
                {
                    int dataIndex = indexs[0];
                    DataTable dt = GetDataSource();

                    if (dt != null && dt.Rows.Count > dataIndex)
                    {
                        int searchRowIndex = Convert.ToInt32(dt.Rows[dataIndex]["SearchRowIndex"]);
                        return RefDataSource[searchRowIndex];
                    }

                    return null;
                }

                return null;
            }
        }

        /// <summary>
        /// Gets the contact list.
        /// </summary>
        public AccelaGridView GViewContactList
        {
            get
            {
                return gdvSearchContactList;
            }
        }

        /// <summary>
        /// Gets the view id for reference contact list
        /// </summary>
        public string GViewID
        {
            get
            {
                return gdvSearchContactList.GridViewNumber;
            }
        }

        /// <summary>
        /// Gets or sets cap contact data table.
        /// </summary>
        public IList<PeopleModel> RefDataSource
        {
            get
            {
                if (ViewState["RefDataSource"] == null)
                {
                    ViewState["RefDataSource"] = new List<PeopleModel>();
                }

                return (IList<PeopleModel>)ViewState["RefDataSource"];
            }

            set
            {
                ViewState["RefDataSource"] = value;
            }
        }

        /// <summary>
        /// Gets or sets the search type.
        /// </summary>
        public ContactProcessType ContactSearchType
        {
            get
            {
                return _contactSearchType;
            }

            set
            {
                _contactSearchType = value;

                if (ContactSearchType == ContactProcessType.Lookup)
                {
                    gdvSearchContactList.GridViewNumber = GviewID.ReferenceContactList;
                    GridViewBuildHelper.InitializeGridWithTemplate(gdvSearchContactList, ModuleName, BizDomainConstant.STD_CAT_CONTACT_TYPE);
                }
                else
                {
                    gdvSearchContactList.GridViewNumber = ContactSearchType == ContactProcessType.SelectContactFromOtherAgencies
                                                            ? GviewID.OtherAgenciesReferenceContactList
                                                            : (ContactSearchType == ContactProcessType.SelectContactFromAccount && ContactSectionPosition == ACAConstant.ContactSectionPosition.RegisterAccount ? GviewID.ContactSelectFromProfessionals : GviewID.ContactSelectFromAccountList);
                    GridViewBuildHelper.SetSimpleViewElements(gdvSearchContactList, ModuleName, AppSession.IsAdmin);
                }
            }
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
            }
        }

        /// <summary>
        /// bind data list.
        /// </summary>
        public void BindRefContactsList()
        {
            ResetDataSource();
            gdvSearchContactList.DataBind();
        }

        /// <summary>
        /// Clear condition
        /// </summary>
        public void HideCondition()
        {
            ucConditon.HideCondition();
        }

        /// <summary>
        /// Validate whether the condition is locked for the selected contact.
        /// </summary>
        /// <returns>true or false</returns>
        public bool ValidateCondition()
        {
            return ShowCondition(SelectedItem);
        }

        /// <summary>
        /// Raises the page load event
        /// </summary>
        /// <param name="sender">An object that contains the event sender.</param>
        /// <param name="e">A System.EventArgs object containing the event data.</param>
        protected void Page_Load(object sender, EventArgs e)
        {      
        }

        /// <summary>
        /// Response permit grid view page index changing event to record the latest page index.
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">A System.EventArgs object containing the event data.</param>
        protected void RefContactList_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            /*
             * Reset data source since we use Datatable as the DataSource,
             *  but in AccelaGridView DataSource will not be keep if DataSource is a Datatable.
             *  See AccelaGridView_PageIndexChanging method in AccelaGridView.
             */
            ResetDataSource();

            if (PageIndexChanging != null)
            {
                PageIndexChanging(sender, e);
            }
        }

        /// <summary>
        /// Response permit grid view RowCommand event to record the latest sort expression.
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">A System.EventArgs object containing the event data.</param>
        protected void SearchContactList_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Header")
            {
                /*
                 * Reset data source since we use Datatable as the DataSource,
                 *  but in AccelaGridView DataSource will not be keep if DataSource is a Datatable.
                 *  See AccelaGridView_Sorting method in AccelaGridView.
                 */
                BindRefContactsList();
            }
        }

        /// <summary>
        /// grid view LicenseList row data bound event.
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">GridViewCommandEventArgs e</param>
        protected void SearchContactList_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                DataRowView rowView = (DataRowView)e.Row.DataItem;
                AccelaLabel lblFullName = e.Row.FindControl("lblFullName") as AccelaLabel;
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

                if (string.IsNullOrEmpty(rowView["fullName"].ToString()))
                {
                    string[] fullName = { rowView["firstName"].ToString(), rowView["middleName"].ToString(), rowView["lastName"].ToString() };
                    string displayName = DataUtil.ConcatStringWithSplitChar(fullName, ACAConstant.BLANK);
                    lblFullName.Text = displayName;
                }
            }
        }

        /// <summary>
        /// The vent method validating the conditions.
        /// </summary>
        /// <param name="sender">An object that contains the event sender.</param>
        /// <param name="e">A System.EventArgs object containing the event data.</param>
        protected void ConditionsCheckButton_Click(object sender, EventArgs e)
        {
            ShowCondition(SelectedItem);
        }

        /// <summary>
        /// if conditionModel is not null, show locked, hold or notice message.
        /// </summary>
        /// <param name="people">A <see cref="PeopleModel"/></param>
        /// <returns>true or false.</returns>
        private bool ShowCondition(PeopleModel people)
        {
            bool valid = ConditionsUtil.ShowCondition(ucConditon, people);
            return valid;
        }

        /// <summary>
        /// Reset data source for Contact search list.
        /// </summary>
        private void ResetDataSource()
        {
            gdvSearchContactList.EmptyDataText = AppSession.IsAdmin ? string.Empty : GetTextByKey("aca_contactlist_msg_norecordfound");
            gdvSearchContactList.DataSource = GetDataSource();
        }

        /// <summary>
        /// module list to data table and sort
        /// </summary>
        /// <returns>Data Table</returns>
        private DataTable GetDataSource()
        {
            DataTable dt;

            if (ContactSearchType == ContactProcessType.Lookup)
            {
                dt = GridViewBuildHelper.MergeTemplateAttributes2DataTable(gdvSearchContactList, RefDataSource, "attributes");
            }
            else
            {
                dt = ObjectConvertUtil.ConvertModels2DataTable(RefDataSource.ToArray(), false, false);
            }

            string gdvSortExpression = string.Empty;

            if (!string.IsNullOrWhiteSpace(gdvSearchContactList.GridViewSortExpression))
            {
                gdvSortExpression = string.Format(
                                    "{0} {1}",
                                    gdvSearchContactList.GridViewSortExpression,
                                    gdvSearchContactList.GridViewSortDirection);
            }

            //Extract the data table for Child Model.
            dt = ObjectConvertUtil.ExtractChildModel(dt, "compactAddress");

            if (dt == null || dt.Rows.Count == 0)
            {
                return dt;
            }

            //If the contact section data source in "Select from Account" is Reference then should remove the external contact
            if (ComponentDataSource.Reference.Equals(ValidateFlag)
                && ContactSearchType == ContactProcessType.SelectContactFromAccount)
            {
                DataRow[] rows = dt.Select(string.Format("serviceProviderCode <>'{0}'", ConfigManager.SuperAgencyCode));

                foreach (var dataRow in rows)
                {
                    dt.Rows.Remove(dataRow);
                }

                // Contact in RefDataSource also should be removed.
                for (int i = RefDataSource.Count - 1; i >= 0; i--)
                {
                    PeopleModel peopleModel = RefDataSource[i];

                    if (!peopleModel.serviceProviderCode.Equals(ConfigManager.SuperAgencyCode))
                    {
                        RefDataSource.RemoveAt(i);   
                    }
                }
            }
  
            dt.DefaultView.Sort = gdvSortExpression;
            return dt.DefaultView.ToTable();
        }
    }
}