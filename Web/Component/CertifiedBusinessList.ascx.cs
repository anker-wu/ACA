#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: CertifiedBusinessList.ascx.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2011-2014
 *
 *  Description:
 *
 *  Notes:
 *      $Id: CertifiedBusinessList.ascx.cs 190488 2011-02-17 10:00:36Z ACHIEVO\daly.zeng $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using System;
using System.Collections.Generic;
using System.Data;
using System.Web.UI.WebControls;

using Accela.ACA.Common;
using Accela.ACA.Common.Util;
using Accela.ACA.Web.Common;
using Accela.ACA.Web.Common.Control;
using Accela.Web.Controls;

namespace Accela.ACA.Web.Component
{
    /// <summary>
    /// Controller for certified business list.
    /// </summary>
    public partial class CertifiedBusinessList : BaseUserControl
    {
        #region Fields

        /// <summary>
        /// Export file name for certified business list.
        /// </summary>
        private const string CERTIFIED_BUSINESS_FILENAME = "CertifiedBusinessList";

        /// <summary>
        /// grid view sorted event.
        /// </summary>
        public event GridViewSortedEventHandler GridViewSort;

        /// <summary>
        /// grid view download event.
        /// </summary>
        public event GridViewDownloadEventHandler GridViewDownloadAll;

        /// <summary>
        /// grid view page index changing event.
        /// </summary>
        public event GridViewPageEventHandler PageIndexChanging;

        #endregion Fields

        #region Properties

        /// <summary>
        /// Gets or sets license list data source.
        /// </summary>
        public DataTable DataSource
        {
            get
            {
                if (ViewState["DataSource"] == null)
                {
                    ViewState["DataSource"] = LicenseUtil.CreateDataTable4CertBusiness();
                }

                return (DataTable)ViewState["DataSource"];
            }

            set
            {
                ViewState["DataSource"] = value;
            }
        }

        /// <summary>
        /// Gets page size.
        /// </summary>
        public int PageSize
        {
            get
            {
                return gdvCertBusinessList.PageSize;
            }
        }

        /// <summary>
        /// Gets count for display in search result label.
        /// </summary>
        public string CountSummary
        {
            get
            {
                return gdvCertBusinessList.CountSummary;
            }
        }

        #endregion Properties

        #region Method

        /// <summary>
        /// Bind certified business list.
        /// </summary>
        /// <param name="pageIndex">page index.</param>
        /// <param name="sort">string for sort.</param>
        public void BindLicenseeList(int pageIndex, string sort)
        {
            gdvCertBusinessList.DataSource = DataSource;

            if (pageIndex >= 0)
            {
                gdvCertBusinessList.PageIndex = pageIndex;
            }

            if (!string.IsNullOrEmpty(sort))
            {
                string[] s = sort.Trim().Split(' ');

                if (s.Length == 2)
                {
                    gdvCertBusinessList.GridViewSortExpression = s[0];
                    gdvCertBusinessList.GridViewSortDirection = s[1];
                }
            }

            gdvCertBusinessList.DataBind();
        }

        /// <summary>
        /// Initialize the short list action.
        /// </summary>
        public void InitialShortListAction()
        {
            gdvCertBusinessList.InitialShortListAction();
        }

        /// <summary>
        /// <c>OnInit</c> event method.
        /// </summary>
        /// <param name="e">A System.EventArgs object containing the event data.</param>
        protected override void OnInit(EventArgs e)
        {
            string exportFileName = CERTIFIED_BUSINESS_FILENAME;

            // set the simple view element before the GridViewNumber is setted.
            GridViewBuildHelper.SetSimpleViewElements(gdvCertBusinessList, ModuleName, AppSession.IsAdmin);

            SetExperiencesColumnProperties();

            if (!AppSession.IsAdmin && StandardChoiceUtil.IsEnableExport2CSV())
            {
                gdvCertBusinessList.ShowExportLink = true;
                gdvCertBusinessList.ExportFileName = exportFileName;
            }
            else
            {
                gdvCertBusinessList.ShowExportLink = false;
            }          

            base.OnInit(e);
        }

         /// <summary>
        /// Raises the page load event
        /// </summary>
        /// <param name="sender">An object that contains the event sender.</param>
        /// <param name="e">A System.EventArgs object containing the event data.</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            // set the gridview's DataSource which can save as ViewState for short list.
            if (!AppSession.IsAdmin)
            {
                gdvCertBusinessList.DataSource = DataSource;
            }
        }

        /// <summary>
        /// GridView CertBusinessList row command event method.
        /// </summary>
        /// <param name="sender">An object that contains the event sender.</param>
        /// <param name="e">A System.EventArgs object containing the event data.</param>
        protected void CertBusinessList_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e == null || e.CommandArgument == null)
            {
                return;
            }

            if (e.CommandName == "Action")
            {
                string[] arrArgs = e.CommandArgument.ToString().Split(',');

                if (arrArgs.Length == 4)
                {
                    Response.Redirect(string.Format(
                        "CertifiedBusinessDetail.aspx?licenseSeqNbr={0}&stateLicense={1}&licenseType={2}&{3}={4}", 
                        arrArgs[0], 
                        arrArgs[1], 
                        arrArgs[2],
                        UrlConstant.AgencyCode,
                        arrArgs[3]));
                }
            }
        }

        /// <summary>
        /// Fire sorted event.
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void CertBusinessList_GridViewSort(object sender, AccelaGridViewSortEventArgs e)
        {
            if (GridViewSort != null)
            {
                GridViewSort(sender, e);
            }           
        }

        /// <summary>
        /// Page index changing event
        /// </summary>
        /// <param name="sender">Object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void CertBusinessList_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            if (PageIndexChanging != null)
            {
                PageIndexChanging(sender, e);
            }           
        }

        /// <summary>
        /// After grid data bind event.
        /// </summary>
        /// <param name="sender">Object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void CertBusinessList_DataBound(object sender, EventArgs e)
        {
            // set the DataSource which can save as ViewState for short list.
            if (!AppSession.IsAdmin)
            {
                DataSource = gdvCertBusinessList.DataSource as DataTable;
            }
        }

        /// <summary>
        /// GridView CertBusinessList download event method.
        /// </summary>
        /// <param name="sender">An object that contains the event sender.</param>
        /// <param name="e">A GridViewDownloadEventArgs object containing the event data.</param>
        protected void CertBusinessList_GridViewDownload(object sender, GridViewDownloadEventArgs e)
        {
            // execute the download event that setting the exported content.
            if (GridViewDownloadAll != null)
            {
                GridViewDownloadAll(sender, e);
            }
        }
        
        /// <summary>
        /// Set the largest contract experiences column properties
        /// </summary>
        private void SetExperiencesColumnProperties()
        {
            List<ColumnProperty> columnProperties = new List<ColumnProperty>();
            foreach (ColumnProperty cp in gdvCertBusinessList.ColumnProperties)
            {
                columnProperties.Add(cp);
            }

            List<string> lstExperiencesColumn = new List<string>();
            for (int i = 1; i <= LicenseUtil.EXPERIENCES_TOP_NUMBER; i++)
            {
                lstExperiencesColumn.Add(string.Format("lnkClientName{0}Header", i));
                lstExperiencesColumn.Add(string.Format("lnkJobValue{0}Header", i));
                lstExperiencesColumn.Add(string.Format("lnkWorkDate{0}Header", i));
                lstExperiencesColumn.Add(string.Format("lnkDescription{0}Header", i));
            }

            foreach (string colName in lstExperiencesColumn)
            {
                ColumnProperty cp = new ColumnProperty();
                cp.ElementName = colName;
                cp.Visible = ACAConstant.VALID_STATUS;
                columnProperties.Add(cp);
            }

            gdvCertBusinessList.ColumnProperties = columnProperties.ToArray();
        }

        #endregion Method
    }
}
