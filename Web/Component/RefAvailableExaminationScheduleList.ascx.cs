#region Header
/**
 * <pre>
 * 
 *  Accela Citizen Access
 *  File: RefAvailableExaminationScheduleList.ascx.cs
 * 
 *  Accela, Inc.
 *  Copyright (C): 2011-2014
 * 
 *  Description:
 * 
 *  Notes:
 * $Id: ExamScheduleViewModel.cs 181867 2011-09-27 08:06:18Z ACHIEVO\daly.zeng $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */
#endregion Header

using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;

using Accela.ACA.Common;
using Accela.ACA.Common.Util;
using Accela.ACA.Web.Common;
using Accela.ACA.Web.Common.Control;
using Accela.ACA.Web.Examination;
using Accela.Web.Controls;

namespace Accela.ACA.Web.Component
{
    /// <summary>
    /// Available Examination ScheduleList
    /// </summary>
    public partial class RefAvailableExaminationScheduleList : BaseUserControl
    {
        #region Fields

        /// <summary>
        /// Examination url of view details page
        /// </summary>
        private const string EXAMINATION_URL_PROVIDERVIEWDETAILS = "Examination/ProviderExaminationDetail.aspx";

        /// <summary>
        /// Indicating the component used for search result or not
        /// </summary>
        private static bool _isForSearch;

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
        /// Gets or sets cap contact data table.
        /// </summary>
        /// <value>The data source.</value>
        public DataTable DataSource
        {
            get
            {
                if (ViewState["AvailableExaminationScheduleListDataSource"] == null)
                {
                    ViewState["AvailableExaminationScheduleListDataSource"] = new DataTable();
                }

                return (DataTable)ViewState["AvailableExaminationScheduleListDataSource"];
            }

            set
            {
                ViewState["AvailableExaminationScheduleListDataSource"] = value;
            }
        }

        /// <summary>
        /// Gets or sets examination Number.
        /// </summary>
        public string ExaminationNbr
        {
            get
            {
                return (string)ViewState["ExaminationNbr"];
            }

            set
            {
                ViewState["ExaminationNbr"] = value;
            }
        }

        /// <summary>
        /// Gets or sets examination name.
        /// </summary>
        public string ExaminationName
        {
            get
            {
                return (string)ViewState["ExaminationName"];
            }

            set
            {
                ViewState["ExaminationName"] = value;
            }
        }

        /// <summary>
        /// Sets a value indicating whether the component used for search result or not.
        /// </summary>
        public bool IsForSeach
        {
            set
            {
                _isForSearch = value;
            }
        }

        /// <summary>
        /// Gets or sets the grid view id.
        /// </summary>
        /// <value>The grid view id.</value>
        public string GridViewId
        {
            get
            {
                return gdvExaminationScheduleList.GridViewNumber;
            }

            set
            {
                gdvExaminationScheduleList.GridViewNumber = value;
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Bind contact list by contact list data source.
        /// </summary>
        /// <param name="clearHidden">If set to <c>true</c> [clear hidden].</param>
        /// <param name="defaultSort">If set to <c>true</c> [default sort].</param>
        /// <param name="resetPageIndex">If set to <c>true</c> [reset page index].</param>
        public void BindAvailableExaminationScheduleList(bool clearHidden, bool defaultSort, bool resetPageIndex)
        {
            if (defaultSort)
            {
                gdvExaminationScheduleList.GridViewSortExpression =
                    ColumnConstant.RefExaminationScheduleDetail.Date.ToString();
                gdvExaminationScheduleList.GridViewSortDirection = ACAConstant.ORDER_BY_ASC;
                DataSource.DefaultView.Sort = ColumnConstant.RefExaminationScheduleDetail.Date.ToString() + " ASC";
            }

            if (resetPageIndex)
            {
                gdvExaminationScheduleList.PageIndex = 0;
            }

            gdvExaminationScheduleList.DataSource = DataSource;
            gdvExaminationScheduleList.EmptyDataText = AppSession.IsAdmin ? string.Empty : GetTextByKey("per_permitList_messagel_noRecord");
            gdvExaminationScheduleList.DataBind();
            ExaminationScheduleListPanel.Update();

            if (clearHidden)
            {
                hfSelectedID.Value = string.Empty;
            }
        }

        /// <summary>
        /// Clears the radio button status.
        /// </summary>
        public void ClearRadioButtonStatus()
        {
            if (DataSource != null)
            {
                foreach (GridViewRow row in gdvExaminationScheduleList.Rows)
                {
                    AccelaRadioButton radioButton = row.FindControl("rdAvailableExamination") as AccelaRadioButton;

                    if (radioButton != null && radioButton.Checked)
                    {
                        radioButton.Checked = false;
                    }
                }
            }
        }

        /// <summary>
        /// Gets the selected examination.
        /// </summary>
        /// <param name="examinationWizardParameter">The examination wizard parameter.</param>
        public void GetSelectedExamination(ExaminationParameter examinationWizardParameter)
        {
            if (DataSource != null && DataSource.Rows.Count > 0)
            {
                foreach (DataRow dataRow in DataSource.Rows)
                {
                    if (dataRow[ColumnConstant.RefExaminationScheduleDetail.ExaminationScheduleID.ToString()].ToString() == hfSelectedID.Value)
                    {
                        ExaminationScheduleUtil.ConvertDataRowToModel(dataRow, ref examinationWizardParameter);
                    }
                }
            }
        }

        /// <summary>
        /// Handles the Load event of the Page control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected void Page_Load(object sender, EventArgs e)
        {            
            DialogUtil.RegisterScriptForDialog(this.Page);             
        }

        /// <summary>
        /// GridView ContactList row command event method.
        /// </summary>
        /// <param name="sender">An object that contains the event sender.</param>
        /// <param name="e">A System.EventArgs object containing the event data.</param>
        protected void ExaminationScheduleList_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (!string.IsNullOrEmpty(e.CommandName))
            {
                BindAvailableExaminationScheduleList(false, false, false);
            }
        }

         /// <summary>
        /// Fire sorted event.
        /// </summary>
        /// <param name="sender">Object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void ExaminationScheduleList_GridViewSort(object sender, AccelaGridViewSortEventArgs e)
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
        protected void ExaminationScheduleList_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            int selectedRowIndex = e.NewPageIndex * gdvExaminationScheduleList.PageSize;
            int pageIndex = 0;
            if (selectedRowIndex > 0)
            {
                pageIndex = selectedRowIndex / gdvExaminationScheduleList.PageSize;
            }

            gdvExaminationScheduleList.DataSource = DataSource;
            gdvExaminationScheduleList.PageIndex = pageIndex;
            gdvExaminationScheduleList.DataBind();

            if (PageIndexChanging != null)
            {
                PageIndexChanging(sender, e);
            }
        }

        /// <summary>
        /// OnPreRender event method.
        /// </summary>
        /// <param name="e">A System.EventArgs object containing the event data.</param>
        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);

            //it is for aca admin to switch label keys, if grid view no data, couldn't trigger the row create & databind event.
            Control headerRow = null;

            if (gdvExaminationScheduleList.HasControls() && gdvExaminationScheduleList.Controls[0].HasControls())
            {
                headerRow = gdvExaminationScheduleList.Controls[0].Controls[0];
            }

            if (headerRow != null)
            {
                var lnkProviderHeader = headerRow.FindControl("lnkProvider") as GridViewHeaderLabel;
                var lnkFeeHeader = headerRow.FindControl("lnkFee") as GridViewHeaderLabel;
                var lnkDateHeader = headerRow.FindControl("lnkDate") as GridViewHeaderLabel;
                var lnkWeekyDayHeader = headerRow.FindControl("lnkWeekyDay") as GridViewHeaderLabel;
                var lnkStartTimeHeader = headerRow.FindControl("lnkStartTime") as GridViewHeaderLabel;
                var lnkEndTimeHeader = headerRow.FindControl("lnkEndTime") as GridViewHeaderLabel;
                var lnkExaminationSiteHeader = headerRow.FindControl("lnkExaminationSite") as GridViewHeaderLabel;
                var lnkAvailableSeatsHeader = headerRow.FindControl("lnkAvailableSeats") as GridViewHeaderLabel;
                var lnkHandicapAccessibleHeader = headerRow.FindControl("lnkHandicapAccessible") as GridViewHeaderLabel;

                if (GridViewId == GviewID.ExaminationSchedule)
                {
                    SetGridViewHeaderLabelKey(lnkProviderHeader, "aca_exam_schedule_availablelist_header_provider");
                    SetGridViewHeaderLabelKey(lnkFeeHeader, "aca_exam_schedule_availablelist_header_fee");
                    SetGridViewHeaderLabelKey(lnkDateHeader, "aca_exam_schedule_availablelist_header_date");
                    SetGridViewHeaderLabelKey(lnkWeekyDayHeader, "aca_exam_schedule_availablelist_header_weekyday");
                    SetGridViewHeaderLabelKey(lnkStartTimeHeader, "aca_exam_schedule_availablelist_header_starttime");
                    SetGridViewHeaderLabelKey(lnkEndTimeHeader, "aca_exam_schedule_availablelist_header_endtime");
                    SetGridViewHeaderLabelKey(lnkExaminationSiteHeader, "aca_exam_schedule_availablelist_header_examsite");
                    SetGridViewHeaderLabelKey(lnkAvailableSeatsHeader, "aca_exam_schedule_availablelist_header_seats");
                    SetGridViewHeaderLabelKey(lnkHandicapAccessibleHeader, "aca_exam_schedule_availablelist_header_handicap");
                }
                else if (GridViewId == GviewID.CheckexaminationSchedule)
                {
                    SetGridViewHeaderLabelKey(lnkProviderHeader, "aca_exam_detail_header_provider");
                    SetGridViewHeaderLabelKey(lnkFeeHeader, "aca_exam_detail_header_fee");
                    SetGridViewHeaderLabelKey(lnkDateHeader, "aca_exam_detail_header_date");
                    SetGridViewHeaderLabelKey(lnkWeekyDayHeader, "aca_exam_detail_header_weekday");
                    SetGridViewHeaderLabelKey(lnkStartTimeHeader, "aca_exam_detail_header_starttime");
                    SetGridViewHeaderLabelKey(lnkEndTimeHeader, "aca_exam_detail_header_endtime");
                    SetGridViewHeaderLabelKey(lnkExaminationSiteHeader, "aca_exam_detail_header_examsite");
                    SetGridViewHeaderLabelKey(lnkAvailableSeatsHeader, "aca_exam_detail_header_seats");
                    SetGridViewHeaderLabelKey(lnkHandicapAccessibleHeader, "aca_exam_detail_header_handicap");
                }
            }
        }

        /// <summary>
        /// Page Initial Handler
        /// </summary>
        /// <param name="e">EventArgs object</param>
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            HideColumnsForSpecialCase();
            GridViewBuildHelper.SetSimpleViewElements(gdvExaminationScheduleList, ModuleName, AppSession.IsAdmin);
        }

        /// <summary>
        /// GridView ContactList row data bound event.
        /// </summary>
        /// <param name="sender">Object sender</param>
        /// <param name="e">GridViewCommandEventArgs e</param>
        protected void ExaminationScheduleList_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                ExaminationParameter parameter = new ExaminationParameter();
                DataRowView rowView = (DataRowView)e.Row.DataItem;

                parameter.ExamScheduleScheduleId = rowView[ColumnConstant.RefExaminationScheduleDetail.ScheduleId.ToString()].ToString();
                string examinationScheduleID = rowView[ColumnConstant.RefExaminationScheduleDetail.ExaminationScheduleID.ToString()].ToString();
                parameter.ExamScheduleLocationId = rowView[ColumnConstant.RefExaminationScheduleDetail.LocationId.ToString()].ToString();
                parameter.ExamScheduleCalendarId = rowView[ColumnConstant.RefExaminationScheduleDetail.CalendarId.ToString()].ToString();
                parameter.ExamScheduleProviderNbr = rowView[ColumnConstant.RefExaminationScheduleDetail.ProviderNbr.ToString()].ToString();
                parameter.ExamScheduleWeekDay = rowView[ColumnConstant.RefExaminationScheduleDetail.WeekyDay.ToString()].ToString();
                parameter.ExamScheduleSeats = rowView[ColumnConstant.RefExaminationScheduleDetail.AvailableSeats.ToString()].ToString();

                DateTime dateTime =
                    (DateTime)rowView[ColumnConstant.RefExaminationScheduleDetail.Date.ToString()];

                if (dateTime != DateTime.MinValue)
                {
                    parameter.ExamScheduleDate = I18nDateTimeUtil.FormatToDateStringForUI(dateTime);
                }

                DateTime startTime =
                    (DateTime)rowView[ColumnConstant.RefExaminationScheduleDetail.StartTime.ToString()];

                if (startTime != DateTime.MinValue)
                {
                    parameter.ExamScheduleStartTime = I18nDateTimeUtil.FormatToTimeStringForUI(startTime, false);
                }

                DateTime endTime =
                    (DateTime)rowView[ColumnConstant.RefExaminationScheduleDetail.EndTime.ToString()];

                if (endTime != DateTime.MinValue)
                {
                    parameter.ExamScheduleEndTime = I18nDateTimeUtil.FormatToTimeStringForUI(endTime, false);
                }

                parameter.ExaminationName = ExaminationName;
                parameter.ExaminationNbr = ExaminationNbr;
                                
                AccelaRadioButton accelaRadioButton = e.Row.Cells[0].FindControl("rdAvailableExamination") as AccelaRadioButton;
                AccelaLinkButton lnkProviderViewDetail = e.Row.FindControl("lnkProviderViewDetail") as AccelaLinkButton;

                string selectedexaminationid = hfSelectedID.Value;

                if (accelaRadioButton != null)
                {
                    if (selectedexaminationid == examinationScheduleID)
                    {
                        accelaRadioButton.Checked = true;
                    }
                    else
                    {
                        accelaRadioButton.Checked = false;
                    }

                    accelaRadioButton.Attributes.Add(
                        "onclick",
                        "SelectExaminationRadio(this,'" + gdvExaminationScheduleList.ClientID + "','" + hfSelectedID.ClientID + "');SetButtonStatus();SetSelectProviderNbr('" + hfSelectedProviderNbr.ClientID + "','" + parameter.ExamScheduleProviderNbr + "');");
                    accelaRadioButton.InputAttributes.Add("title", GetTextByKey("aca_selectonerecord_checkbox"));
                }

                if (_isForSearch)
                {
                    accelaRadioButton.Visible = false;
                    lnkProviderViewDetail.Visible = true;

                    string url = FileUtil.AppendApplicationRoot(EXAMINATION_URL_PROVIDERVIEWDETAILS);
                    url = string.Format("{0}?{1}", url, ExaminationParameterUtil.BuildQueryString(parameter));

                    lnkProviderViewDetail.Attributes.Add("onclick", "ShowExaminationPopupDialog('" + url + "','" + lnkProviderViewDetail.ClientID + "')");
                }
                else
                {                    
                    lnkProviderViewDetail.Visible = false;
                    accelaRadioButton.Visible = true;
                }
            }
        }

        /// <summary>
        /// Set the GridViewHeaderLabel's label key
        /// </summary>
        /// <param name="gridViewHeaderLabel">The GridView header label</param>
        /// <param name="labelKey">The label key</param>
        private void SetGridViewHeaderLabelKey(GridViewHeaderLabel gridViewHeaderLabel, string labelKey)
        {
            if (gridViewHeaderLabel != null)
            {
                gridViewHeaderLabel.LabelKey = labelKey;
            }
        }

        /// <summary>
        /// Hide the Columns or Links for the special case.
        /// </summary>
        private void HideColumnsForSpecialCase()
        {
            if (GviewID.ExaminationSchedule.Equals(GridViewId))
            {
                gdvExaminationScheduleList.Columns[10].Visible = false;
            }
        }

        #endregion
    }
}