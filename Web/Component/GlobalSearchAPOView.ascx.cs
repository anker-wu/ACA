#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: GlobalSearchAPOView.ascx.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2009-2014
 *
 *  Description: Display global search APO list
 *
 *  Notes:
 *      $Id: GlobalSearchAPOView.ascx.cs 277971 2014-08-25 06:10:06Z ACHIEVO\james.shi $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Web.UI.WebControls;
using Accela.ACA.Common;
using Accela.ACA.Web.Common;
using Accela.ACA.Web.Common.Control;
using Accela.ACA.Web.Common.GlobalSearch;
using Accela.Web.Controls;

namespace Accela.ACA.Web.Component
{
    /// <summary>
    /// global search APO view
    /// </summary>
    public partial class GlobalSearchAPOView : BaseUserControl
    {
        #region Events

        /// <summary>
        /// grid view page index changing event
        /// </summary>
        public event GridViewPageEventHandler PageIndexChanging;

        /// <summary>
        /// Occurs when [list loading].
        /// </summary>
        public event EventHandler ListLoading;

        /// <summary>
        /// Occurs when [list loaded].
        /// </summary>
        public event EventHandler ListLoaded;

        #endregion Events

        #region Properties

        /// <summary>
        /// Gets or sets data source
        /// </summary>
        public DataTable GridViewDataSource
        {
            get
            {
                if (ViewState[ModuleName] != null)
                {
                    return (DataTable)ViewState[ModuleName];
                }

                return null;
            }

            set
            {
                ViewState[ModuleName] = value;
            }
        }

        /// <summary>
        /// Gets the total count.
        /// </summary>
        /// <value>The total count.</value>
        public int TotalCount
        {
            get
            {
                return GetActiveGridView().CustomizedTotalCount;
            }
        }

        /// <summary>
        /// Gets the page size of GridView 
        /// </summary>
        public int PageSize
        {
            get
            {
                return GetActiveGridView().PageSize;
            }
        }

        /// <summary>
        /// Gets the display total count in GridView.
        /// </summary>
        public string CountSummary
        {
            get
            {
                return GetActiveGridView().CountSummary;
            }
        }

        /// <summary>
        /// Gets or sets the moduleName of base user control.
        /// </summary>
        protected override string ModuleName
        {
            get
            {
                if (ViewState["SectionModuleName"] != null)
                {
                    return ViewState["SectionModuleName"].ToString();
                }
                else
                {
                    return Request.QueryString[ACAConstant.MODULE_NAME];
                }
            }

            set
            {
                ViewState["SectionModuleName"] = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is loading history.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is loading history; otherwise, <c>false</c>.
        /// </value>
        private bool IsLoadingHistory
        {
            get;
            set;
        }

        #endregion Properties

        #region Public Methods

        /// <summary>
        /// Displays the specified list.
        /// </summary>
        /// <param name="apoList">The apo list.</param>
        /// <param name="isLoadingHistory">if set to <c>true</c> [is loading history].</param>
        /// <param name="globalSearchType">Type of the global search.</param>
        public void Display(List<APOView4UI> apoList, bool isLoadingHistory, GlobalSearchType globalSearchType)
        {
            IsLoadingHistory = isLoadingHistory;

            if (isLoadingHistory)
            {
                SetupViewByHistory();
            }
            else
            {
                UpdateAPOSearchType(globalSearchType);
            }

            GlobalSearchType currentSearchType = GetSelectedSearchType();
            GlobalSearchParameter historyParameter = GlobalSearchManager.GetGlobalSearchParameter(currentSearchType);

            if (historyParameter != null)
            {
                GetActiveGridView().CustomizedTotalCount = historyParameter.TotalRecordsFromWS;
            }

            if (AppSession.IsAdmin)
            {
                gdvAPOListByParcel.DataSource = null;
                gdvAPOListByParcel.DataBind();
                gdvAPOListByAddress.DataSource = null;
                gdvAPOListByAddress.DataBind();
            }
            else
            {
                GetActiveGridView().DataSource = apoList;
                GetActiveGridView().DataBind();
            }
        }

        /// <summary>
        /// Sets the grid view sort info.
        /// </summary>
        /// <param name="sortColumn">The sort column.</param>
        /// <param name="sortDirection">The sort direction.</param>
        public void SetGridViewSortInfo(string sortColumn, string sortDirection)
        {
            GetActiveGridView().GridViewSortExpression = sortColumn;
            GetActiveGridView().GridViewSortDirection = sortDirection;
        }

        /// <summary>
        /// Navigate To APO detail page.
        /// </summary>
        /// <param name="apoViewList">APO view UI list model</param>
        /// <returns>IsNavigate Flag</returns>
        public bool RedirectToApoDetail(List<APOView4UI> apoViewList)
        {
            bool isNavigate = false;

            if (apoViewList == null || apoViewList.Count != 1)
            {
                return isNavigate;
            }

            string redirectUrl = string.Empty;
            APOView4UI apoView = apoViewList[0];
            bool isOnlyShowParcel = !string.IsNullOrEmpty(apoView.ParcelNumber)
                                    && string.IsNullOrEmpty(apoView.OwnerName)
                                    && string.IsNullOrEmpty(apoView.AddressDescription);

            bool isOnlyShowAddress = !string.IsNullOrEmpty(apoView.AddressDescription)
                                     && string.IsNullOrEmpty(apoView.OwnerName)
                                     && string.IsNullOrEmpty(apoView.ParcelNumber);

            //Owner can't exist alone, it must link the other Module,such as Parcel or Address.So there's no need to judge the Owner
            if (isOnlyShowParcel)
            {
                redirectUrl = string.Format(
                    "APO/ParcelDetail.aspx?{0}={1}&{2}={3}&{4}={5}&{6}={7}&{8}={9}",
                    ACAConstant.REQUEST_PARMETER_PARCEL_NUMBER,
                    UrlEncode(apoView.ParcelNumber),
                    ACAConstant.REQUEST_PARMETER_PARCEL_SEQUENCE,
                    UrlEncode(apoView.ParcelSeqNbr),
                    ACAConstant.REQUEST_PARMETER_REFADDRESS_ID,
                    UrlEncode(apoView.AddressSeqNumber),
                    ACAConstant.REQUEST_PARMETER_ADDRESS_SEQUENCE,
                    UrlEncode(apoView.AddressSourceNumber),
                    ACAConstant.REQUEST_PARMETER_PARCEL_UID,
                    string.Empty);
            }
            else if (isOnlyShowAddress)
            {
                redirectUrl = string.Format(
                    "APO/AddressDetail.aspx?{0}={1}&{2}={3}&{4}={5}&{6}={7}",
                    ACAConstant.REQUEST_PARMETER_REFADDRESS_ID,
                    UrlEncode(apoView.AddressSeqNumber),
                    ACAConstant.REQUEST_PARMETER_ADDRESS_SEQUENCE,
                    UrlEncode(apoView.AddressSourceNumber),
                    ACAConstant.REQUEST_PARMETER_PARCEL_NUMBER,
                    UrlEncode(apoView.ParcelNumber),
                    ACAConstant.REQUEST_PARMETER_REFADDRESS_UID,
                    string.Empty);
            }

            if (!string.IsNullOrEmpty(redirectUrl))
            {
                isNavigate = true;
                Response.Redirect(FileUtil.AppendApplicationRoot(redirectUrl));
            }

            return isNavigate;
        }

        #endregion Public Methods

        #region Protected Methods

        /// <summary>
        /// Handles the Load event of the Page control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ControlBuildHelper.SetInstructionValue(lblAPOViewTitle_sub_label, GetTextByKey("per_globalsearch_label_apo|sub"));
            }
        }

        /// <summary>
        /// Handles the IndexChanged event of the APOSearchType Dropdown List control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected void APOSearchType_IndexChanged(object sender, EventArgs e)
        {
            if (ListLoading != null)
            {
                ListLoading(sender, e);
            }

            SetupGridViewVisibility();
            GlobalSearchType globalSearchType = GetSelectedSearchType();
            List<APOView4UI> resultList = null;

            if (!AppSession.IsAdmin)
            {
                GlobalSearchParameter historyParameter = GlobalSearchManager.GetGlobalSearchParameter(globalSearchType);
                string sortColumn = GlobalSearchType.ADDRESS == globalSearchType ? "fullAddress" : "parcelNumber";

                if (historyParameter != null)
                {
                    resultList = GlobalSearchManager.ExecuteQuery<APOView4UI>(globalSearchType, historyParameter.QueryText, historyParameter.ModuleArray, sortColumn, historyParameter.SortDirection, PageSize, null);
                }
                else
                {
                    string queryText = Request.QueryString.Get(ACAConstant.GLOBAL_SEARCH_QUERY_TEXT);
                    resultList = GlobalSearchManager.ExecuteQuery<APOView4UI>(globalSearchType, queryText, new[] { string.Empty }, sortColumn, ACAConstant.ORDER_BY_ASC, PageSize, null);
                }
            }

            GetActiveGridView().PageIndex = 0;
            Display(resultList);

            if (ListLoaded != null)
            {
                ListLoaded(sender, e);
            }
        }

        /// <summary>
        /// fire page index changing event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void APOList_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            if (ListLoading != null)
            {
                ListLoading(sender, e);
            }

            AccelaGridView currentGridView = sender as AccelaGridView;
            currentGridView.PageIndex = e.NewPageIndex;
            GlobalSearchType globalSearchType = GetSelectedSearchType();
            GlobalSearchParameter globalSearchParameter = GlobalSearchManager.GetGlobalSearchParameter(globalSearchType);

            if (globalSearchParameter != null)
            {
                globalSearchParameter.PageIndex = e.NewPageIndex;
            }

            if (globalSearchParameter != null && globalSearchParameter.NeedRequestNewRecords())
            {
                List<APOView4UI> apoList = GlobalSearchManager.ExecuteQuery<APOView4UI>(globalSearchType, e.NewPageIndex, null);
                Display(apoList);
            }
            else
            {
                currentGridView.DataBind();
            }

            if (PageIndexChanging != null)
            {
                PageIndexChanging(sender, e);
            }

            if (ListLoaded != null)
            {
                ListLoaded(sender, e);
            }
        }

        /// <summary>
        /// GridView APOList RowCommand
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void APOList_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if ("Header".Equals(e.CommandName, StringComparison.OrdinalIgnoreCase) && e.CommandSource is GridViewHeaderLabel)
            {
                if (ListLoading != null)
                {
                    ListLoading(sender, e);
                }

                GlobalSearchType globalSearchType = GetSelectedSearchType();
                AccelaGridView currentGridView = sender as AccelaGridView;
                string newSortExpression = ((GridViewHeaderLabel)e.CommandSource).SortExpression;
                string newSortDirection = ACAConstant.ORDER_BY_ASC.Equals(currentGridView.GridViewSortDirection, StringComparison.OrdinalIgnoreCase) ? ACAConstant.ORDER_BY_DESC : ACAConstant.ORDER_BY_ASC;
                currentGridView.PageIndex = 0;

                List<APOView4UI> apoList = GlobalSearchManager.ExecuteQuery<APOView4UI>(globalSearchType, newSortExpression, newSortDirection, null);
                Display(apoList);

                if (ListLoaded != null)
                {
                    ListLoaded(sender, e);
                }
            }
        }

        /// <summary>
        /// GridView APOList RowDataBound
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void APOList_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            SetupLinks(sender, e);
        }

        /// <summary>
        /// Raises the <c>System.Web.UI.Control.Init</c> event.
        /// </summary>
        /// <param name="e">An System.EventArgs object that contains the event data.</param>
        protected override void OnInit(EventArgs e)
        {
            //if standard choice display owner section is "N" and is in daily side, set owner column is hidden.
            if (!AppSession.IsAdmin && !StandardChoiceUtil.IsDisplayOwnerSection())
            {
                GridViewBuildHelper.SetHiddenColumn(gdvAPOListByParcel, new[] { "Owner" });
                GridViewBuildHelper.SetHiddenColumn(gdvAPOListByAddress, new[] { "Owner" });
            }

            GridViewBuildHelper.SetSimpleViewElements(gdvAPOListByParcel, ModuleName, AppSession.IsAdmin);
            GridViewBuildHelper.SetSimpleViewElements(gdvAPOListByAddress, ModuleName, AppSession.IsAdmin);

            if (!Page.IsPostBack)
            {
                SetupAPOSearchTypeList();
                SetupGridViewVisibility();
            }

            InitalExport();
            base.OnInit(e);
        }

        #endregion Protected Methods

        #region Private Methods

        /// <summary>
        /// Displays the specified apo list.
        /// </summary>
        /// <param name="apoList">The apo list.</param>
        private void Display(List<APOView4UI> apoList)
        {
            GlobalSearchType selectedSearchType = GetSelectedSearchType();
            Display(apoList, false, selectedSearchType);
        }

        /// <summary>
        /// Setups the view by history.
        /// </summary>
        private void SetupViewByHistory()
        {
            GlobalSearchParameter historyParameter = GlobalSearchManager.GetGlobalSearchParameter(GlobalSearchType.ADDRESS);

            if (historyParameter != null)
            {
                UpdateAPOSearchType(historyParameter.GlobalSearchType);
                SetupGridViewVisibility();
                GetActiveGridView().PageIndex = historyParameter.PageIndex;
                GetActiveGridView().GridViewSortExpression = historyParameter.SortColulmn;
                GetActiveGridView().GridViewSortDirection = historyParameter.SortDirection;
            }
        }

        /// <summary>
        /// Setups the links.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="System.Web.UI.WebControls.GridViewRowEventArgs"/> instance containing the event data.</param>
        private void SetupLinks(object sender, GridViewRowEventArgs e)
        {
            if (e.Row != null && e.Row.RowType == DataControlRowType.DataRow && e.Row.DataItem != null && e.Row.DataItem is APOView4UI)
            {
                if (ListLoading != null)
                {
                    ListLoading(sender, e);
                }

                APOView4UI currentItem = e.Row.DataItem as APOView4UI;
                HyperLink hlParcelNumber = (HyperLink)e.Row.FindControl("hlParcelNumber");
                HyperLink hlOwnerName = (HyperLink)e.Row.FindControl("hlOwnerName");
                HyperLink hlAddressDescription = (HyperLink)e.Row.FindControl("hlAddressDescription");

                hlAddressDescription.NavigateUrl = string.Format(
                            CultureInfo.InvariantCulture,
                            "../APO/AddressDetail.aspx?{0}={1}&{2}={3}&{4}={5}&{6}={7}",
                            ACAConstant.REQUEST_PARMETER_REFADDRESS_ID,
                            UrlEncode(currentItem.AddressSeqNumber),
                            ACAConstant.REQUEST_PARMETER_ADDRESS_SEQUENCE,
                            UrlEncode(currentItem.AddressSourceNumber),
                            ACAConstant.REQUEST_PARMETER_PARCEL_NUMBER,
                            UrlEncode(currentItem.ParcelNumber),
                            ACAConstant.REQUEST_PARMETER_REFADDRESS_UID,
                            string.Empty);

                hlParcelNumber.NavigateUrl = string.Format(
                            CultureInfo.InvariantCulture,
                            "../APO/ParcelDetail.aspx?{0}={1}&{2}={3}&{4}={5}&{6}={7}&{8}={9}",
                            ACAConstant.REQUEST_PARMETER_PARCEL_NUMBER,
                            UrlEncode(currentItem.ParcelNumber),
                            ACAConstant.REQUEST_PARMETER_PARCEL_SEQUENCE,
                            UrlEncode(currentItem.ParcelSeqNbr),
                            ACAConstant.REQUEST_PARMETER_REFADDRESS_ID,
                            UrlEncode(currentItem.AddressSeqNumber),
                            ACAConstant.REQUEST_PARMETER_ADDRESS_SEQUENCE,
                            UrlEncode(currentItem.AddressSourceNumber),
                            ACAConstant.REQUEST_PARMETER_PARCEL_UID,
                            string.Empty);

                hlOwnerName.NavigateUrl = string.Format(
                            CultureInfo.InvariantCulture,
                            "../APO/OwnerDetail.aspx?{0}={1}&{2}={3}&{4}={5}&{6}={7}&{8}={9}&{10}={11}&{12}={13}&{14}={15}&{16}={17}",
                            ACAConstant.REQUEST_PARMETER_OWNER_NUMBER,
                            UrlEncode(currentItem.OwnerSeqNumber),
                            ACAConstant.REQUEST_PARMETER_OWNER_SEQUENCE,
                            UrlEncode(currentItem.OwnerSourceNumber),
                            ACAConstant.REQUEST_PARMETER_OWNER_UID,
                            string.Empty,
                            ACAConstant.REQUEST_PARMETER_REFADDRESS_ID,
                            UrlEncode(currentItem.AddressSeqNumber),
                            ACAConstant.REQUEST_PARMETER_REFADDRESS_UID,
                            string.Empty,
                            ACAConstant.REQUEST_PARMETER_ADDRESS_SEQUENCE,
                            UrlEncode(currentItem.AddressSourceNumber),
                            ACAConstant.REQUEST_PARMETER_PARCEL_NUMBER,
                            UrlEncode(currentItem.ParcelNumber),
                            ACAConstant.REQUEST_PARMETER_PARCEL_UID,
                            string.Empty,
                            ACAConstant.REQUEST_PARMETER_PARCEL_SEQUENCE,
                            UrlEncode(currentItem.ParcelSeqNbr));
            }
        }

        /// <summary>
        /// Initializes the GridView's export link visible. 
        /// </summary>
        private void InitalExport()
        {
            if (!AppSession.IsAdmin && StandardChoiceUtil.IsEnableExport2CSV())
            {
                gdvAPOListByParcel.ShowExportLink = true;
                gdvAPOListByParcel.ExportFileName = "PropertyInformation";
                gdvAPOListByAddress.ShowExportLink = true;
                gdvAPOListByAddress.ExportFileName = "PropertyInformation";
            }
            else
            {
                gdvAPOListByParcel.ShowExportLink = false;
                gdvAPOListByAddress.ShowExportLink = false;
            }
        }

        /// <summary>
        /// Gets the active grid view.
        /// </summary>
        /// <returns>the active grid view</returns>
        private AccelaGridView GetActiveGridView()
        {
            AccelaGridView result = gdvAPOListByParcel;

            if (gdvAPOListByAddress.Visible)
            {
                result = gdvAPOListByAddress;
            }

            return result;
        }

        /// <summary>
        /// Setups the APO search types.
        /// </summary>
        private void SetupAPOSearchTypeList()
        {
            ddlAPOSearchType.Visible = true;
            ddlAPOSearchType.Items.Clear();
            ddlAPOSearchType.Items.Add(new ListItem(GetTextByKey("per_globalsearch_label_searchbyaddress"), GlobalSearchType.ADDRESS.ToString()));
            ddlAPOSearchType.Items.Add(new ListItem(GetTextByKey("per_globalsearch_label_searchbyparcel"), GlobalSearchType.PARCEL.ToString()));

            if (AppSession.IsAdmin)
            {
                ddlAPOSearchType.Attributes.Add("onchange", "return switchGridView(false);");
                ddlAPOSearchType.AutoPostBack = false;
            }
            else
            {
                ddlAPOSearchType.Attributes.Add("onchange", string.Format(CultureInfo.InvariantCulture, "showLoadingPanel('{0}');", ddlAPOSearchType.ClientID));
            }
        }

        /// <summary>
        /// Gets the type of the selected search.
        /// </summary>
        /// <returns>the type of the selected search</returns>
        private GlobalSearchType GetSelectedSearchType()
        {
            GlobalSearchType result = GlobalSearchType.ADDRESS;

            if (ddlAPOSearchType.Items.Count > 0)
            {
                result = (GlobalSearchType)Enum.Parse(typeof(GlobalSearchType), ddlAPOSearchType.SelectedValue, true);
            }

            return result;
        }

        /// <summary>
        /// Updates the type of the APO search.
        /// </summary>
        /// <param name="globalSearchType">Type of the global search.</param>
        private void UpdateAPOSearchType(GlobalSearchType globalSearchType)
        {
            ListItem selectedItem = ddlAPOSearchType.Items.FindByValue(globalSearchType.ToString());

            if (selectedItem != null)
            {
                ddlAPOSearchType.SelectedIndex = -1;
                selectedItem.Selected = true;
            }

            SetupGridViewVisibility();
        }

        /// <summary>
        /// Gets the type of the history search.
        /// </summary>
        /// <returns>the type of the history search</returns>
        private GlobalSearchType GetHistorySearchType()
        {
            GlobalSearchType result = GlobalSearchType.ADDRESS;

            GlobalSearchParameter historyParameter = GlobalSearchManager.GetGlobalSearchParameter(result);

            if (historyParameter != null)
            {
                result = historyParameter.GlobalSearchType;
            }

            return result;
        }

        /// <summary>
        /// Setups the grid view visibility.
        /// </summary>
        private void SetupGridViewVisibility()
        {
            if (AppSession.IsAdmin)
            {
                gdvAPOListByParcel.Visible = true;
                gdvAPOListByAddress.Visible = true;
                Page.ClientScript.RegisterStartupScript(GetType(), "switchGridView", "switchGridView(true);", true);
            }
            else
            {
                GlobalSearchType globalSearchType = GetSelectedSearchType();
                bool isSearchByParcel = GlobalSearchType.PARCEL == globalSearchType;
                gdvAPOListByParcel.Visible = isSearchByParcel;
                gdvAPOListByAddress.Visible = !isSearchByParcel;

                //string instructionKey = isSearchByParcel ? "per_globalsearch_instruction_searchbyparcel" : "per_globalsearch_instruction_searchbyaddress";
                //lblSearchInstruction.Text = GetTextByKey(instructionKey);
            }
        }

        #endregion Private Methods
    }
}