#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: ContactAddressSearchList.ascx.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2013-2014
 *
 *  Description:
 *
 *  Notes:
 *      $Id: ContactAddressSearchList.ascx.cs 258478 2013-10-17 02:28:19Z ACHIEVO\daniel.shi $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion

using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Web.UI.WebControls;

using Accela.ACA.BLL.Common;
using Accela.ACA.Common;
using Accela.ACA.Common.Util;
using Accela.ACA.Web.Common;
using Accela.ACA.Web.Common.Control;
using Accela.ACA.WSProxy;
using Accela.Web.Controls;

namespace Accela.ACA.Web.Component
{
    /// <summary>
    /// Contact address search list
    /// </summary>
    public partial class ContactAddressSearchList : BaseUserControl
    {
        #region Fields

        /// <summary>
        /// The contact section position.
        /// </summary>
        private ACAConstant.ContactSectionPosition _contactSectionPosition;

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the current contact type.
        /// </summary>
        public string ContactType { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether of contact address form editable property.
        /// </summary>
        public bool IsForView
        {
            get;
            set;
        }

        /// <summary>
        /// Gets the selected row Index
        /// </summary>
        public List<int> SelectedRowIndexs
        {
            get
            {
                return gdvContactAddressList.GetSelectedRowIndexesInCurrentPage();
            }
        }

        /// <summary>
        /// Gets the selected item
        /// </summary>
        public IList<ContactAddressModel> SelectedItems
        {
            get
            {
                if (SelectedRowIndexs == null || SelectedRowIndexs.Count == 0 || DataSource == null)
                {
                    return null;
                }

                IList<ContactAddressModel> selectedContactAddresses = new List<ContactAddressModel>();
                DataTable dataSource = gdvContactAddressList.GetSelectedData(DataSource);

                if (dataSource != null && dataSource.Rows.Count > 0)
                {
                    foreach (DataRow row in dataSource.Rows)
                    {
                        ContactAddressModel model = (ContactAddressModel)row["ContactAddressModel"];

                        if (model != null)
                        {
                            selectedContactAddresses.Add(model);
                        }
                    }
                }

                return selectedContactAddresses;
            }
        }

        /// <summary>
        /// Gets contact address list.
        /// </summary>
        public DataTable DataSource
        {
            get
            {
                return (DataTable)ViewState["RefContactAddress"];
            }

            private set
            {
                ViewState["RefContactAddress"] = value;
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
                gdvContactAddressList.GridViewNumber = PeopleUtil.GetContactAddressListGviewID(_contactSectionPosition, true);
                GridViewBuildHelper.SetSimpleViewElements(gdvContactAddressList, ModuleName, AppSession.IsAdmin);
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is need remove selected item.
        /// </summary>
        /// <value><c>true</c> if this instance is need remove selected item; otherwise, <c>false</c>.</value>
        public bool IsNeedRemoveSelectedItem
        {
            get;

            set;
        }

        /// <summary>
        /// Gets or sets the required contact address type.
        /// </summary>
        private IList<string> RequiredContactAddressTypes
        {
            get;
 
            set;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Load contact address list.
        /// </summary>
        /// <param name="contactAddresses">Contact address list.</param>
        public void LoadAddressList(List<ContactAddressModel> contactAddresses)
        {
            if (contactAddresses != null && contactAddresses.Count > 0)
            {
                ICacheManager cacheManager = ObjectFactory.GetObject<ICacheManager>();
                RequiredContactAddressTypes = ObjectCloneUtil.DeepCopy(cacheManager.GetRequiredContactAddressType(ContactType));

                contactAddresses.RemoveAll(f => f.auditModel == null || !ContractorPeopleStatus.Approved.Equals(f.auditModel.auditStatus, StringComparison.OrdinalIgnoreCase));

                DataTable dt = ObjectConvertUtil.ConvertModels2DataTable(contactAddresses.ToArray(), true, false);

                //Extract the data table for Child Model.
                dt = ObjectConvertUtil.ExtractChildModel(dt, "auditModel");
                dt.Columns.Add("ContactAddressModel", typeof(ContactAddressModel));
                int index = 0;

                foreach (ContactAddressModel address in contactAddresses)
                {
                    address.RowIndex = index;
                    dt.Rows[index]["ContactAddressModel"] = address;
                    index++;
                }

                DataSource = dt;
            }

            BindData();
        }

        #region Event Handlers

        /// <summary>
        /// Handle the Page_Load event.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">Event argument.</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            DialogUtil.RegisterScriptForDialog(this.Page);
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
                DataRowView rowView = (DataRowView)e.Row.DataItem;
                AccelaLabel lblStatus = (AccelaLabel)e.Row.FindControl("lblStatus");
                AccelaLabel lblPrimary = (AccelaLabel)e.Row.FindControl("lblPrimary");
                AccelaLabel lblAddress = (AccelaLabel)e.Row.FindControl("lblAddress");
                AccelaLabel lblValidated = (AccelaLabel)e.Row.FindControl("lblValidated");
                ContactAddressModel contactAddress = (ContactAddressModel)rowView["ContactAddressModel"];

                if (contactAddress != null && RequiredContactAddressTypes != null && RequiredContactAddressTypes.Contains(contactAddress.addressType))
                {
                    gdvContactAddressList.SelectRow(e.Row);

                    // Because it maybe exists the same AddressType, so remove the all AddressType that same with the selected. To avoid selecting the duplicate AddressType.
                    RequiredContactAddressTypes.Remove(contactAddress.addressType);
                }
                else if (IsNeedRemoveSelectedItem)
                {
                    gdvContactAddressList.UnselectRow(e.Row);
                }

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

                lblAddress.Text = ContactUtil.BuildAddress(contactAddress);
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

                if (IsNeedRemoveSelectedItem)
                {
                    gdvContactAddressList.UnselectRow(e.Row);
                }
            }
        }
        
        /// <summary>
        /// Handles the RowCommand event of the ContactAddressList control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="GridViewCommandEventArgs"/> instance containing the event data.</param>
        protected void ContactAddressList_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            BindData();
        }

        /// <summary>
        /// response permit grid view sorted event to record the latest sort expression.
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">A System.EventArgs object containing the event data.</param>
        protected void ContactAddressList_GridViewSort(object sender, AccelaGridViewSortEventArgs e)
        {
            DataSource.DefaultView.Sort = e.GridViewSortExpression;
            DataSource = DataSource.DefaultView.ToTable();
        }

        #endregion

        /// <summary>
        /// Re-bind data to contact address list.
        /// </summary>
        private void BindData()
        {
            gdvContactAddressList.DataSource = DataSource;
            gdvContactAddressList.EmptyDataText = AppSession.IsAdmin ? string.Empty : GetTextByKey("per_permitList_messagel_noRecord");
            gdvContactAddressList.DataBind();
        }

        #endregion
    }
}