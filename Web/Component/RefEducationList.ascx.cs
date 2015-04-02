#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: RefEducationList.aspx.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2007-2014
 *
 *  Description:
 *
 *  Notes:
 *      $Id: RefEducationList.ascx.cs 277971 2014-08-25 06:10:06Z ACHIEVO\james.shi $.
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

namespace Accela.ACA.Web.Education
{
    /// <summary>
    /// UC for RefEducationList
    /// </summary>
    public partial class RefEducationList : BaseUserControl
    {
        #region Fields

        /// <summary>
        /// Command for selected row in education list.
        /// </summary>
        private const string COMMAND_SELECTED_EDUCATION = "selectedEducation";

        #endregion Fields

        #region Events

        /// <summary>
        /// Grid view page index changing event.
        /// </summary>
        public event GridViewPageEventHandler PageIndexChanging;

        /// <summary>
        /// Grid view sorted event.
        /// </summary>
        public event GridViewSortedEventHandler GridViewSort;

        #endregion Events

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
                    ViewState["DataSource"] = new DataTable();
                }

                return ViewState["DataSource"] as DataTable;
            }

            set
            {
                ViewState["DataSource"] = value;
            }
        }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Bind education list.
        /// </summary>
        /// <param name="pageIndex">page index.</param>
        /// <param name="sort">string for sort.</param>
        public void BindEducationList(int pageIndex, string sort)
        {
            if (pageIndex >= 0)
            {
                gdvRefEducationList.PageIndex = pageIndex;
            }

            if (!string.IsNullOrEmpty(sort))
            {
                string[] s = sort.Trim().Split(' ');

                if (s.Length == 2)
                {
                    gdvRefEducationList.GridViewSortExpression = s[0];
                    gdvRefEducationList.GridViewSortDirection = s[1];
                    DataView dataView = new DataView(DataSource);
                    dataView.Sort = sort;
                    DataSource = dataView.ToTable();
                }
            }

            Bind();
        }

        /// <summary>
        /// Convert refEducation list to data table.
        /// </summary>
        /// <param name="refEducationModels">refEducationModels array</param>
        /// <returns>dataTable for education list</returns>
        public DataTable ConvertEducationListToDataTable(RefEducationModel4WS[] refEducationModels)
        {
            DataTable dtProvider = ConstructEducationDataTable();

            if (refEducationModels != null && refEducationModels.Length > 0)
            {
                foreach (RefEducationModel4WS refEducationModel in refEducationModels)
                {
                    if (refEducationModel != null)
                    {
                        DataRow dr = dtProvider.NewRow();
                        dr[ColumnConstant.RefEducation.Name.ToString()] = refEducationModel.refEducationName;
                        dr[ColumnConstant.RefEducation.Degree.ToString()] = refEducationModel.degree;
                        dr[ColumnConstant.RefEducation.Number.ToString()] = refEducationModel.refEducationNbr;
                        dtProvider.Rows.Add(dr);
                    }
                }
            }

            return dtProvider;
        }

        /// <summary>
        /// Fire sorted event.
        /// </summary>
        /// <param name="sender">Object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void RefEducationList_GridViewSort(object sender, AccelaGridViewSortEventArgs e)
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
        protected void RefEducationList_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            if (PageIndexChanging != null)
            {
                PageIndexChanging(sender, e);
            }
        }

        /// <summary>
        /// <c>OnInit</c> event method.
        /// </summary>
        /// <param name="e">A System.EventArgs object containing the event data.</param>
        protected override void OnInit(EventArgs e)
        {
            GridViewBuildHelper.SetSimpleViewElements(gdvRefEducationList, ModuleName, AppSession.IsAdmin);
            gdvRefEducationList.GridViewDownload += RefEducationList_GridViewDownload;

            if (!IsPostBack)
            {
                if (!AppSession.IsAdmin && StandardChoiceUtil.IsEnableExport2CSV())
                {
                    gdvRefEducationList.ShowExportLink = true;
                    gdvRefEducationList.ExportFileName = "Education";
                }
                else
                {
                    gdvRefEducationList.ShowExportLink = false;
                }
            }

            base.OnInit(e);
        }

        /// <summary>
        /// Handles the grid view download event.
        /// </summary>
        /// <param name="sender">Object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void RefEducationList_GridViewDownload(object sender, EventArgs e)
        {
            Bind();
        }

        /// <summary>
        /// GridView EducationList row command event method.
        /// </summary>
        /// <param name="sender">An object that contains the event sender.</param>
        /// <param name="e">A System.EventArgs object containing the event data.</param>
        protected void EducationList_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == COMMAND_SELECTED_EDUCATION)
            {
                //1. Get command argument.
                string educationNumber = e.CommandArgument.ToString();

                //2.Go to provider detail page.
                Response.Redirect(string.Format("EducationDetail.aspx?refEducationNbr={0}", educationNumber));
            }
            else
            {
                Bind();
            }
        }

        /// <summary>
        /// Construct a new DataTable for Education.
        /// </summary>
        /// <returns>
        /// Construct education dataTable
        /// </returns>
        private static DataTable ConstructEducationDataTable()
        {
            DataTable educationDataTable = new DataTable();
            educationDataTable.Columns.Add(ColumnConstant.RefEducation.Name.ToString());
            educationDataTable.Columns.Add(ColumnConstant.RefEducation.Degree.ToString());
            educationDataTable.Columns.Add(ColumnConstant.RefEducation.Number.ToString());

            return educationDataTable;
        }

        /// <summary>
        /// Bind education list.
        /// </summary>
        private void Bind()
        {
            gdvRefEducationList.DataSource = DataSource;
            gdvRefEducationList.EmptyDataText = AppSession.IsAdmin ? string.Empty : GetTextByKey("per_permitList_messagel_noRecord");
            gdvRefEducationList.DataBind();
        }

        #endregion Methods
    }
}