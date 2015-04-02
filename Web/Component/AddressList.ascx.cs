#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: AddressList.ascx.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2008-2014
 *
 *  Description:
 *
 *  Notes:
 *      $Id: AddressList.ascx.cs 277585 2014-08-18 11:09:47Z ACHIEVO\james.shi $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;

using Accela.ACA.BLL.APO;
using Accela.ACA.BLL.Common;
using Accela.ACA.Common;
using Accela.ACA.Common.Common;
using Accela.ACA.Common.Util;
using Accela.ACA.Web.Common;
using Accela.ACA.Web.Common.Control;
using Accela.ACA.WSProxy;
using Accela.Web.Controls;

namespace Accela.ACA.Web.Component
{
    /// <summary>
    /// the class for AddressList.
    /// </summary>
    public partial class AddressList : BaseUserControl
    {
        #region Fields

        /// <summary>
        /// address list.
        /// </summary>
        private const string PARCELINFO_LIST = "PARCELINFO_LIST";

        #endregion Fields

        #region Events

        /// <summary>
        /// grid view sorted event.
        /// </summary>
        public event GridViewSortedEventHandler GridViewSort;

        /// <summary>
        /// grid view page index changing event
        /// </summary>
        public event GridViewPageEventHandler PageIndexChanging;

        #endregion Events

        #region Properties

        /// <summary>
        /// Gets or sets data source
        /// </summary>
        public DataTable GridViewDataSource
        {
            get
            {
                return (DataTable)ViewState["AddressModels"];
            }

            set
            {
                ViewState["AddressModels"] = value;
            }
        }

        /// <summary>
        /// Gets or sets parcel info list.
        /// </summary>
        public ParcelInfoModel[] ParcelInfoList
        {
            get
            {
                if (ViewState[PARCELINFO_LIST] == null)
                {
                    return null;
                }

                return (ParcelInfoModel[])ViewState[PARCELINFO_LIST];
            }

            set
            {
                ViewState[PARCELINFO_LIST] = value;
            }       
        }

        /// <summary>
        /// Gets the page size.
        /// </summary>
        public int PageSize
        {
            get
            {
                return gvAddress.PageSize;
            }
        }

        /// <summary>
        /// Gets or sets zip code.
        /// </summary>
        public string ZipCode
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets city.
        /// </summary>
        public string City
        {
            get;
            set;
        }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Bind address list when user click search button.
        /// </summary>
        /// <param name="dtAddress">Address data table</param>
        public void BindAddressList(DataTable dtAddress)
        {
            // gvAddress.Items.Clear();
            divAddressResult.Visible = true;

            if (dtAddress.Rows.Count >= 1)
            {
                //if more than one result is returned display them in the list
                divAddressPan.Visible = true;
                gvAddress.Visible = true;

                GridViewDataSource = dtAddress;
                gvAddress.DataSource = GridViewDataSource;
                gvAddress.PageIndex = 0;
                gvAddress.DataBind();
                string text = GetTextByKey("superAgency_workLocation_label_selectAddress");
                string appendPrefix = GetTextByKey("superAgency_workLocation_label_addressFound");
                lblSelectAddress.Text = string.Format(text + "(" + appendPrefix + "):", gvAddress.CountSummary);

                if (dtAddress.Rows.Count == 1)
                {
                    serviceControl.BindServiceListByParcel(ParcelInfoList[0]);
                }
            }
            else
            {
                divAddressPan.Visible = false;
                this.ShowNoAddressNotice(true);
            }
        }

                /// <summary>
        /// Raises the <c>System.Web.UI.Control.Init</c> event.
        /// </summary>
        /// <param name="e">An System.EventArgs object that contains the event data.</param>
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            //if standard choice display owner section is "N" and is in daily side, set owner column is hidden.
            if (!AppSession.IsAdmin && !StandardChoiceUtil.IsDisplayOwnerSection())
            {
                GridViewBuildHelper.SetHiddenColumn(gvAddress, new[] { "Owner" });
            }

            GridViewBuildHelper.SetSimpleViewElements(gvAddress, ModuleName, AppSession.IsAdmin);
        }

        /// <summary>
        /// Raises the page load event
        /// </summary>
        /// <param name="sender">An object that contains the event sender.</param>
        /// <param name="e">A System.EventArgs object containing the event data.</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            serviceControl.ZipCode = ZipCode;
            serviceControl.City = City;
            serviceControl.SingleServiceOnly = false;

            if (!IsPostBack)
            {
                if (AppSession.IsAdmin)
                {
                    divAddressResult.Visible = true;
                    divForAdminShowAddress.Visible = true;

                    BindAddressForAdmin();
                }
            }
        }

        /// <summary>
        /// fire page index changing event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void Address_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            if (PageIndexChanging != null)
            {
                PageIndexChanging(sender, e);
            }
        }

        /// <summary>
        /// fire sorted event.
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void Address_GridViewSort(object sender, AccelaGridViewSortEventArgs e)
        {
            if (GridViewSort != null)
            {
                GridViewSort(sender, e);
            }
        }

        /// <summary>
        /// Raises ref-address list data bound event
        /// </summary>
        /// <param name="sender">An object that contains the event sender.</param>
        /// <param name="e">An GridViewCommandEventArgs object that contains the event data.</param>
        protected void Address_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType != DataControlRowType.DataRow)
            {
                return;
            }

            DataRowView dv = (DataRowView)e.Row.DataItem;
            DataRow dr = dv.Row;

            AccelaLinkButton lnkGetService = (AccelaLinkButton)e.Row.FindControl("lnkGetService");
            lnkGetService.CommandArgument = dr["RowIndex"].ToString();
        }

        /// <summary>
        /// Raises ref-address list row command event - select address
        /// </summary>
        /// <param name="sender">An object that contains the event sender.</param>
        /// <param name="e">An GridViewCommandEventArgs object that contains the event data.</param>
        protected void Address_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "GetService")
            {
                var parcelInfo = from p in this.ParcelInfoList where p.RowIndex == int.Parse(e.CommandArgument.ToString()) select p;
                ParcelInfoModel[] parcelInfos = parcelInfo.ToArray();
                serviceControl.BindServiceListByParcel(parcelInfos[0]);
            }
            else
            {
                gvAddress.DataSource = GridViewDataSource;
                gvAddress.DataBind();
                
                if (AppSession.SelectedParcelInfo != null)
                {
                    serviceControl.BindServiceListByParcel(AppSession.SelectedParcelInfo);
                }
            }
        }

        /// <summary>
        /// Bind Address list in admin.
        /// </summary>
        private void BindAddressForAdmin()
        {
            if (AppSession.IsAdmin)
            {
                gvAddress.DataSource = APOUtil.BuildAPODataTable(null);
                gvAddress.DataBind();
                gvAddress.Visible = true;
            }
        }

        /// <summary>
        /// Show notice message for none address.
        /// </summary>
        /// <param name="show">true or false</param>
        private void ShowNoAddressNotice(bool show)
        {
            divAddressResult.Visible = true;

            //there is no result, show the official web site link url
            serviceControl.ShowOfficialSite(show);
        }

        #endregion Methods
    }
}
