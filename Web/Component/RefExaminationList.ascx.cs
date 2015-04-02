#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: RefExaminationList.aspx.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2009-2014
 *
 *  Description:
 *
 *  Notes:
 *      $Id: RefExaminationList.ascx.cs 140040 2009-07-21 06:06:55Z ACHIEVO\jackie.yu $.
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
    /// UC for RefExaminationList
    /// </summary>
    public partial class RefExaminationList : BaseUserControl
    {
        #region Fields

        /// <summary>
        /// command for selected row in examinations list.
        /// </summary>
        private const string COMMAND_SELECTED_EXAMINATIONS = "selectedExaminations";

        /// <summary>
        /// grid view page index changing event.
        /// </summary>
        public event GridViewPageEventHandler PageIndexChanging;

        /// <summary>
        /// grid view sorted event.
        /// </summary>
        public event GridViewSortedEventHandler GridViewSort;

        #endregion Fields

        #region Properties

        /// <summary>
        /// Gets or sets examinations list data source.
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
        /// Convert refExamination list to data table.
        /// </summary>
        /// <param name="refExaminationModels">refExaminationModel array</param>
        /// <returns>dataTable for examination list</returns>
        public DataTable ConvertListToDataTable(RefExaminationModel4WS[] refExaminationModels)
        {
            DataTable dataTable = ConstructDataTable();

            if (refExaminationModels != null && refExaminationModels.Length > 0)
            {
                for (int i = 0; i < refExaminationModels.Length; i++)
                {
                    if (refExaminationModels[i] == null)
                    {
                        continue;
                    }

                    DataRow dr = dataTable.NewRow();
                    dr[ColumnConstant.RefExaminations.Name.ToString()] = refExaminationModels[i].examName;
                    
                    string gradingStyle = refExaminationModels[i].gradingStyle;
                    string score = refExaminationModels[i].passingScore;

                    dr[ColumnConstant.RefExaminations.GradingStyle.ToString()] = EducationUtil.FormatGradingStyle(gradingStyle);
                    dr[ColumnConstant.RefExaminations.PassingScore.ToString()] = EducationUtil.FormatScore(gradingStyle, score, true);
                    dr[ColumnConstant.RefExaminations.refExamNbr.ToString()] = refExaminationModels[i].refExamNbr;
                    dataTable.Rows.Add(dr);
                }
            }

            return dataTable;
        }

        /// <summary>
        /// Bind examinations list.
        /// </summary>
        /// <param name="pageIndex">page index.</param>
        /// <param name="sort">string for sort.</param>
        public void BindExaminationList(int pageIndex, string sort)
        {
            if (pageIndex >= 0)
            {
                gdvRefExaminationList.PageIndex = pageIndex;
            }

            if (!string.IsNullOrEmpty(sort))
            {
                string[] array = sort.Trim().Split(' ');

                if (array.Length == 2)
                {
                    gdvRefExaminationList.GridViewSortExpression = array[0];
                    gdvRefExaminationList.GridViewSortDirection = array[1];
                    DataView dataView = new DataView(DataSource);
                    dataView.Sort = sort;
                    DataSource = dataView.ToTable();
                }
            }

            Bind();
        }

        /// <summary>
        /// Fire sorted event.
        /// </summary>
        /// <param name="sender">Object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void RefExaminationList_GridViewSort(object sender, AccelaGridViewSortEventArgs e)
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
        protected void RefExaminationList_PageIndexChanging(object sender, GridViewPageEventArgs e)
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
            GridViewBuildHelper.SetSimpleViewElements(gdvRefExaminationList, ModuleName, AppSession.IsAdmin);
            gdvRefExaminationList.GridViewDownload += RefExaminationList_GridViewDownload;

            if (!IsPostBack)
            {
                if (!AppSession.IsAdmin && StandardChoiceUtil.IsEnableExport2CSV())
                {
                    gdvRefExaminationList.ShowExportLink = true;
                    gdvRefExaminationList.ExportFileName = "Examinations";
                }
                else
                {
                    gdvRefExaminationList.ShowExportLink = false;
                }
            }

            base.OnInit(e);
        }

        /// <summary>
        /// Handles the grid view download event.
        /// </summary>
        /// <param name="sender">Object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void RefExaminationList_GridViewDownload(object sender, EventArgs e)
        {
            Bind();
        }

        /// <summary>
        /// GridView EducationList row command event method.
        /// </summary>
        /// <param name="sender">An object that contains the event sender.</param>
        /// <param name="e">A System.EventArgs object containing the event data.</param>
        protected void RefExaminationList_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == COMMAND_SELECTED_EXAMINATIONS)
            {
                //1. Get command argument.
                string refExamNbr = e.CommandArgument.ToString();

                //2.Go to provider detail page.
                Response.Redirect(string.Format("ExaminationDetail.aspx?refExamNbr={0}", refExamNbr));
            }
            else
            {
                Bind();
            }
        }

        /// <summary>
        /// Construct a new DataTable for Examinations.
        /// </summary>
        /// <returns>
        /// Construct Examinations dataTable
        /// </returns>
        private static DataTable ConstructDataTable()
        {
            DataTable dtExamintions = new DataTable();
            dtExamintions.Columns.Add(ColumnConstant.RefExaminations.Name.ToString());
            dtExamintions.Columns.Add(ColumnConstant.RefExaminations.GradingStyle.ToString());
            dtExamintions.Columns.Add(ColumnConstant.RefExaminations.PassingScore.ToString());
            dtExamintions.Columns.Add(ColumnConstant.RefExaminations.refExamNbr.ToString());

            return dtExamintions;
        }

        /// <summary>
        /// Bind education list.
        /// </summary>
        private void Bind()
        {
            gdvRefExaminationList.DataSource = DataSource;
            gdvRefExaminationList.EmptyDataText = AppSession.IsAdmin ? string.Empty : GetTextByKey("per_permitList_messagel_noRecord");
            gdvRefExaminationList.DataBind();
        }

        #endregion Methods
    }
}