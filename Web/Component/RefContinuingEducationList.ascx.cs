#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: RefContinuingEducationList.ascx.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2009-2014
 *
 *  Description:
 *
 *  Notes:
 *      $Id: RefContinuingEducationList.ascx.cs 140040 2009-07-21 06:06:55Z ACHIEVO\jackie.yu $.
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
    /// UC for RefContinuingEducationList
    /// </summary>
    public partial class RefContinuingEducationList : BaseUserControl
    {
        #region Fields

        /// <summary>
        /// Command for selected row in education list.
        /// </summary>
        private const string COMMAND_SELECTED_CONTINUING_EDUCATION = "selectedContinuingEducation";

        /// <summary>
        /// Grid view page index changing event.
        /// </summary>
        public event GridViewPageEventHandler PageIndexChanging;

        /// <summary>
        /// Grid view sorted event.
        /// </summary>
        public event GridViewSortedEventHandler GridViewSort;

        #endregion Fields

        #region Properties

        /// <summary>
        /// Gets or sets continuing education list data source.
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
        /// Convert refContinuingEducation list to data table.
        /// </summary>
        /// <param name="refContinuingEducationModels">refContinuingEducationModel array</param>
        /// <returns>dataTable for education list</returns>
        public DataTable ConvertListToDataTable(RefContinuingEducationModel4WS[] refContinuingEducationModels)
        {
            DataTable dataTable = ConstructDataTable();

            if (refContinuingEducationModels != null && refContinuingEducationModels.Length > 0)
            {
                for (int i = 0; i < refContinuingEducationModels.Length; i++)
                {
                    if (refContinuingEducationModels[i] == null)
                    {
                        continue;
                    }

                    DataRow dr = dataTable.NewRow();
                    dr[ColumnConstant.RefContinuingEducation.CourseName.ToString()] = refContinuingEducationModels[i].contEduName;

                    string gradingStyle = refContinuingEducationModels[i].gradingStyle;
                    string score = refContinuingEducationModels[i].passingScore;

                    dr[ColumnConstant.RefContinuingEducation.GradingStyle.ToString()] = EducationUtil.FormatGradingStyle(gradingStyle);
                    dr[ColumnConstant.RefContinuingEducation.PassingScore.ToString()] = EducationUtil.FormatScore(gradingStyle, score, true);
                    dr[ColumnConstant.RefContinuingEducation.ContEduNbr.ToString()] = refContinuingEducationModels[i].refContEduNbr;
                    dataTable.Rows.Add(dr);
                }
            }

            return dataTable;
        }

        /// <summary>
        /// Bind provider list.
        /// </summary>
        /// <param name="pageIndex">page index.</param>
        /// <param name="sort">string for sort.</param>
        public void BindContEduList(int pageIndex, string sort)
        {
            if (pageIndex >= 0)
            {
                gdvRefContinuingEducationList.PageIndex = pageIndex;
            }

            if (!string.IsNullOrEmpty(sort))
            {
                string[] array = sort.Trim().Split(' ');

                if (array.Length == 2)
                {
                    gdvRefContinuingEducationList.GridViewSortExpression = array[0];
                    gdvRefContinuingEducationList.GridViewSortDirection = array[1];

                    DataView dataView = new DataView(DataSource);
                    dataView.Sort = sort;
                    DataSource = dataView.ToTable();
                }
            }

            Bind();
        }

        /// <summary>
        /// <c>OnInit</c> event method.
        /// </summary>
        /// <param name="e">A System.EventArgs object containing the event data.</param>
        protected override void OnInit(EventArgs e)
        {
            GridViewBuildHelper.SetSimpleViewElements(gdvRefContinuingEducationList, ModuleName, AppSession.IsAdmin);
            gdvRefContinuingEducationList.GridViewDownload += RefContinuingEducationList_GridViewDownload;

            if (!IsPostBack)
            {
                if (!AppSession.IsAdmin && StandardChoiceUtil.IsEnableExport2CSV())
                {
                    gdvRefContinuingEducationList.ShowExportLink = true;
                    gdvRefContinuingEducationList.ExportFileName = "ContinuingEducation";
                }
                else
                {
                    gdvRefContinuingEducationList.ShowExportLink = false;
                }
            }

            base.OnInit(e);
        }

        /// <summary>
        /// Handles the grid view download event.
        /// </summary>
        /// <param name="sender">Object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void RefContinuingEducationList_GridViewDownload(object sender, EventArgs e)
        {
            Bind();
        }

        /// <summary>
        /// Fire sorted event.
        /// </summary>
        /// <param name="sender">Object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void RefContinuingEducationList_GridViewSort(object sender, AccelaGridViewSortEventArgs e)
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
        protected void RefContinuingEducationList_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            if (PageIndexChanging != null)
            {
                PageIndexChanging(sender, e);
            }
        }

        /// <summary>
        /// GridView EducationList row command event method.
        /// </summary>
        /// <param name="sender">An object that contains the event sender.</param>
        /// <param name="e">A System.EventArgs object containing the event data.</param>
        protected void RefContinuingEducationList_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == COMMAND_SELECTED_CONTINUING_EDUCATION)
            {
                //1. Get command argument.
                string contEduNbr = e.CommandArgument.ToString();

                //2.Go to provider detail page.
                Response.Redirect(string.Format("ContinuingEducationDetail.aspx?refContEduNbr={0}", contEduNbr));
            }
            else
            {
                Bind();
            }
        }

        /// <summary>
        /// Construct a new dataTable for continuing education.
        /// </summary>
        /// <returns>Construct continuing education dataTable</returns>
        private static DataTable ConstructDataTable()
        {
            DataTable dataTable = new DataTable();
            dataTable.Columns.Add(ColumnConstant.RefContinuingEducation.CourseName.ToString());
            dataTable.Columns.Add(ColumnConstant.RefContinuingEducation.GradingStyle.ToString());
            dataTable.Columns.Add(ColumnConstant.RefContinuingEducation.PassingScore.ToString());
            dataTable.Columns.Add(ColumnConstant.RefContinuingEducation.ContEduNbr.ToString());

            return dataTable;
        }

        /// <summary>
        /// Bind continuing education list.
        /// </summary>
        private void Bind()
        {
            gdvRefContinuingEducationList.DataSource = DataSource;
            gdvRefContinuingEducationList.EmptyDataText = AppSession.IsAdmin ? string.Empty : GetTextByKey("per_permitList_messagel_noRecord");
            gdvRefContinuingEducationList.DataBind();
        }

        #endregion Methods
    }
}
