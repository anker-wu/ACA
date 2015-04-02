#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: GlobalSearchLPView.ascx.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2009-2014
 *
 *  Description: Display global search APO list
 *
 *  Notes:
 *      $Id: GlobalSearchLPView.ascx.cs 277741 2014-08-20 10:31:02Z ACHIEVO\james.shi $.
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
    /// global search LP view
    /// </summary>
    public partial class GlobalSearchLPView : BaseUserControl
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
                else
                {
                    return null;
                }
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
                return this.gdvLicenseList.CustomizedTotalCount;
            }
        }

        /// <summary>
        /// Gets the page size of GridView 
        /// </summary>
        public int PageSize
        {
            get
            {
                return gdvLicenseList.PageSize;
            }
        }

        /// <summary>
        /// Gets display total count in GridView.
        /// </summary>
        public string CountSummary
        {
            get
            {
                return gdvLicenseList.CountSummary;
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

        #endregion Properties

        /// <summary>
        /// Displays the specified list.
        /// </summary>
        /// <param name="lpList">The LP list.</param>
        /// <param name="isLoadingHistory">if set to <c>true</c> [is loading history].</param>
        public void Display(List<LPView4UI> lpList, bool isLoadingHistory)
        {
            if (isLoadingHistory)
            {
                SetupViewByHistory();
            }

            GlobalSearchParameter historyParameter = GlobalSearchManager.GetGlobalSearchParameter(GlobalSearchType.LP);

            if (historyParameter != null)
            {
                gdvLicenseList.CustomizedTotalCount = historyParameter.TotalRecordsFromWS;
            }

            int listcount = lpList == null ? 0 : lpList.Count;
            gdvLicenseList.DataSource = lpList;
            gdvLicenseList.DataBind();
        }

        /// <summary>
        /// Sets the grid view sort info.
        /// </summary>
        /// <param name="sortColumn">The sort column.</param>
        /// <param name="sortDirection">The sort direction.</param>
        public void SetGridViewSortInfo(string sortColumn, string sortDirection)
        {
            gdvLicenseList.GridViewSortExpression = sortColumn;
            gdvLicenseList.GridViewSortDirection = sortDirection;
        }

        /// <summary>
        /// Navigate To LP detail page.
        /// </summary>
        /// <param name="lpViewList">LP view UI list model</param>
        /// <returns>IsNavigate Flag</returns>
        public bool RedirectToLpDetail(List<LPView4UI> lpViewList)
        {
            bool isNavigate = false;

            if (lpViewList != null && lpViewList.Count == 1)
            {
                isNavigate = true;
                LPView4UI lPView = lpViewList[0];
                
                string url = string.Format(
                        "GeneralProperty/LicenseeDetail.aspx?LicenseeNumber={0}&LicenseeType={1}",
                        UrlEncode(lPView.LicenseNumber),
                        UrlEncode(lPView.LicenseType));
               
                Response.Redirect(FileUtil.AppendApplicationRoot(url));
            }

            return isNavigate;
        }

        /// <summary>
        /// Raises the <c>System.Web.UI.Control.Init</c> event.
        /// </summary>
        /// <param name="e">An System.EventArgs object that contains the event data.</param>
        protected override void OnInit(EventArgs e)
        {
            GridViewBuildHelper.SetSimpleViewElements(gdvLicenseList, ModuleName, AppSession.IsAdmin);
            InitalExport();
            base.OnInit(e);
        }

        /// <summary>
        /// fire page index changing event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void LicenseList_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            if (ListLoading != null)
            {
                ListLoading(sender, e);
            }

            gdvLicenseList.PageIndex = e.NewPageIndex;

            GlobalSearchParameter globalSearchParameter = GlobalSearchManager.GetGlobalSearchParameter(GlobalSearchType.LP);

            if (globalSearchParameter != null)
            {
                globalSearchParameter.PageIndex = e.NewPageIndex;
            }

            if (globalSearchParameter != null && globalSearchParameter.NeedRequestNewRecords())
            {
                List<LPView4UI> lpList = GlobalSearchManager.ExecuteQuery<LPView4UI>(GlobalSearchType.LP, e.NewPageIndex, null);
                Display(lpList);
            }
            else
            {
                gdvLicenseList.DataBind();
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
        /// GridView LicenseList RowCommand
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void LicenseList_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if ("Header".Equals(e.CommandName, StringComparison.OrdinalIgnoreCase) && e.CommandSource is GridViewHeaderLabel)
            {
                if (ListLoading != null)
                {
                    ListLoading(sender, e);
                }

                AccelaGridView currentGridView = sender as AccelaGridView;
                string newSortExpression = ((GridViewHeaderLabel)e.CommandSource).SortExpression;
                string newSortDirection = ACAConstant.ORDER_BY_ASC.Equals(currentGridView.GridViewSortDirection, StringComparison.OrdinalIgnoreCase) ? ACAConstant.ORDER_BY_DESC : ACAConstant.ORDER_BY_ASC;
                currentGridView.PageIndex = 0;

                List<LPView4UI> lpList = GlobalSearchManager.ExecuteQuery<LPView4UI>(GlobalSearchType.LP, newSortExpression, newSortDirection, null);
                Display(lpList);

                if (ListLoaded != null)
                {
                    ListLoaded(sender, e);
                }
            }
        }

        /// <summary>
        /// GridView LicenseList RowDataBound
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void LicenseList_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            SetupLicenseNumberLink(sender, e);
        }

        /// <summary>
        /// Displays the specified list.
        /// </summary>
        /// <param name="list">The LPView4UI list.</param>
        private void Display(List<LPView4UI> list)
        {
            this.Display(list, false);
        }

        /// <summary>
        /// Setups the view by history.
        /// </summary>
        private void SetupViewByHistory()
        {
            GlobalSearchParameter historyParameter = GlobalSearchManager.GetGlobalSearchParameter(GlobalSearchType.LP);

            if (historyParameter != null)
            {
                gdvLicenseList.PageIndex = historyParameter.PageIndex;
                gdvLicenseList.GridViewSortExpression = historyParameter.SortColulmn;
                gdvLicenseList.GridViewSortDirection = historyParameter.SortDirection;
            }
        }

        /// <summary>
        /// Initializes the GridView's export link visible. 
        /// </summary>
        private void InitalExport()
        {
            if (!AppSession.IsAdmin && StandardChoiceUtil.IsEnableExport2CSV())
            {
                gdvLicenseList.ShowExportLink = true;
                gdvLicenseList.ExportFileName = "LicensedProfessionals";
            }
            else
            {
                gdvLicenseList.ShowExportLink = false;
            }
        }

        /// <summary>
        /// Setups the license number link.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="System.Web.UI.WebControls.GridViewRowEventArgs"/> instance containing the event data.</param>
        private void SetupLicenseNumberLink(object sender, GridViewRowEventArgs e)
        {
            if (e.Row != null && e.Row.RowType == DataControlRowType.DataRow && e.Row.DataItem != null && e.Row.DataItem is LPView4UI)
            {
                LPView4UI currentItem = e.Row.DataItem as LPView4UI;
                HyperLink hlLicenseNumber = (HyperLink)e.Row.FindControl("hlLicenseNumber");

                if (!string.IsNullOrEmpty(currentItem.LicenseNumber))
                {
                    hlLicenseNumber.NavigateUrl = string.Format(
                        CultureInfo.InvariantCulture,
                        "../GeneralProperty/LicenseeDetail.aspx?LicenseeNumber={0}&LicenseeType={1}",
                        UrlEncode(currentItem.LicenseNumber),
                        UrlEncode(currentItem.LicenseType));
                }
            }
        }
    }
}