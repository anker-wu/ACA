#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: RefProviderList.ascx.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2009-2014
 *
 *  Description:
 *
 *  Notes:
 *      $Id: RefProviderList.ascx.cs 277971 2014-08-25 06:10:06Z ACHIEVO\james.shi $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using System;
using System.Data;
using System.Web.UI.WebControls;

using Accela.ACA.Common;
using Accela.ACA.Web.Common;
using Accela.ACA.Web.Common.Control;
using Accela.ACA.WSProxy;
using Accela.Web.Controls;

namespace Accela.ACA.Web.Component
{
    /// <summary>
    /// UC for display RefProvider list.
    /// </summary>
    public partial class RefProviderList : BaseUserControl
    {
        #region Fields

        /// <summary>
        /// Command constant "selected provider".
        /// </summary>
        private const string COMMAND_SELECTED_PROVIDER = "selectedProvider";
        
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
        /// Gets or sets provider list data source.
        /// </summary>
        public DataTable DataSource
        {
            get
            {
                if (ViewState["DataSource"] == null)
                {
                    ViewState["DataSource"] = ConstructProviderDataTable();
                }

                return ViewState["DataSource"] as DataTable;
            }

            set
            {
                ViewState["DataSource"] = value;
            }
        }

        /// <summary>
        /// Gets the current GridView page size.
        /// </summary>
        public int PageSize
        {
            get
            {
                return gdvRefProviderList.PageSize;
            }
        }

        /// <summary>
        /// Gets the display label in search result label.
        /// </summary>
        public string CountSummary
        {
            get
            {
                return gdvRefProviderList.CountSummary;
            }
        }

        #endregion Properties

        #region Method

        /// <summary>
        /// Bind provider list.
        /// </summary>
        /// <param name="pageIndex">Page index.</param>
        /// <param name="sort">String for sort.</param>
        public void BindProviderList(int pageIndex, string sort)
        {
            if (pageIndex >= 0)
            {
                gdvRefProviderList.PageIndex = pageIndex;
            }

            if (!string.IsNullOrEmpty(sort))
            {
                string[] s = sort.Trim().Split(' ');

                if (s.Length == 2)
                {
                    gdvRefProviderList.GridViewSortExpression = s[0];
                    gdvRefProviderList.GridViewSortDirection = s[1];

                    //sort dataTable.
                    DataView dataView = new DataView(DataSource);
                    dataView.Sort = sort;
                    DataSource = dataView.ToTable();
                }
            }

            Bind();
        }

        /// <summary>
        /// Convert provider list to data table.
        /// </summary>
        /// <param name="providerModelist">ProviderModel array</param>
        /// <returns>DataTable for capContact list</returns>
        public DataTable ConvertProviderListToDataTable(ProviderModel4WS[] providerModelist)
        {
            DataTable dtProvider = ConstructProviderDataTable();

            if (providerModelist == null || providerModelist.Length == 0)
            {
                return dtProvider;
            }

            foreach (ProviderModel4WS providerModel in providerModelist)
            {
                if (providerModel == null)
                {
                    continue;
                }

                DataRow dr = dtProvider.NewRow();
                dtProvider.Rows.Add(dr);

                dr[ColumnConstant.RefProvider.Name.ToString()] = providerModel.providerName;
                dr[ColumnConstant.RefProvider.Number.ToString()] = providerModel.providerNo;

                if (providerModel.refLicenseProfessionalModel != null)
                {
                    dr[ColumnConstant.RefProvider.Address.ToString()] = providerModel.refLicenseProfessionalModel.address1;
                    dr[ColumnConstant.RefProvider.PhoneNumber.ToString()] = ModelUIFormat.FormatPhoneShow(providerModel.refLicenseProfessionalModel.phone1CountryCode, providerModel.refLicenseProfessionalModel.phone1, providerModel.refLicenseProfessionalModel.countyCode);
                }

                dr[ColumnConstant.RefProvider.ProviderPKNbr.ToString()] = providerModel.providerNbr;
            }

            return dtProvider;
        }

        /// <summary>
        /// fire sorted event.
        /// </summary>
        /// <param name="sender">Object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void RefProvidertList_GridViewSort(object sender, AccelaGridViewSortEventArgs e)
        {
            DataSource.DefaultView.Sort = e.GridViewSortExpression;
            DataSource = DataSource.DefaultView.ToTable();

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
        protected void RefProvidertList_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            if (PageIndexChanging != null)
            {
                PageIndexChanging(sender, e);
            }
        }

        /// <summary>
        /// GridView ProviderList row command event method.
        /// </summary>
        /// <param name="sender">An object that contains the event sender.</param>
        /// <param name="e">A System.EventArgs object containing the event data.</param>
        protected void RefProviderList_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == COMMAND_SELECTED_PROVIDER)
            {
                string providerPKNbr = e.CommandArgument.ToString();

                Response.Redirect(string.Format("ProviderDetail.aspx?providerPKNbr={0}", providerPKNbr));
            }
            else
            {
                Bind();
            }
        }

        /// <summary>
        /// GridView RefProviderList download event method.
        /// </summary>
        /// <param name="sender">An object that contains the event sender.</param>
        /// <param name="e">A GridViewDownloadEventArgs object containing the event data.</param>
        protected void RefProviderList_GridViewDownload(object sender, GridViewDownloadEventArgs e)
        {
            // execute the download event that setting the exported content.
            Bind();
            if (GridViewDownloadAll != null)
            {
                GridViewDownloadAll(sender, e);
            }
        }

        /// <summary>
        /// <c>OnInit</c> event method.
        /// </summary>
        /// <param name="e">A System.EventArgs object containing the event data.</param>
        protected override void OnInit(EventArgs e)
        {
            GridViewBuildHelper.SetSimpleViewElements(gdvRefProviderList, ModuleName, AppSession.IsAdmin);

            if (!IsPostBack)
            {
                if (!AppSession.IsAdmin && StandardChoiceUtil.IsEnableExport2CSV())
                {
                    gdvRefProviderList.ShowExportLink = true;
                    gdvRefProviderList.ExportFileName = "Provider";
                }
                else
                {
                    gdvRefProviderList.ShowExportLink = false;
                }
            }

            base.OnInit(e);
        }

        /// <summary>
        /// Construct a new DataTable for Provider.
        /// </summary>
        /// <returns>
        /// Construct provider dataTable
        /// </returns>
        private static DataTable ConstructProviderDataTable()
        {
            DataTable providerDataTable = new DataTable();
            providerDataTable.Columns.Add(ColumnConstant.RefProvider.Name.ToString());
            providerDataTable.Columns.Add(ColumnConstant.RefProvider.Number.ToString());
            providerDataTable.Columns.Add(ColumnConstant.RefProvider.Address.ToString());
            providerDataTable.Columns.Add(ColumnConstant.RefProvider.PhoneNumber.ToString());
            providerDataTable.Columns.Add(ColumnConstant.RefProvider.ProviderPKNbr.ToString());

            return providerDataTable;
        }

        /// <summary>
        /// Bind data to provider list GridView.
        /// </summary>
        private void Bind()
        {
            gdvRefProviderList.DataSource = DataSource;
            gdvRefProviderList.EmptyDataText = AppSession.IsAdmin ? string.Empty : GetTextByKey("per_permitList_messagel_noRecord");
            gdvRefProviderList.DataBind();
        }

        #endregion Method
    }
}