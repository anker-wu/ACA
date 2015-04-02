#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: UploadInspectionList.ascx.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2013-2014
 *
 *  Description: The upload inspection list.
 *
 *  Notes:
 *      $Id: UploadInspectionList.ascx.cs 252704 2013-06-25 10:05:17Z ACHIEVO\alan.hu $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using System;
using System.Collections.Generic;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;

using Accela.ACA.BLL.AddressBuilder;
using Accela.ACA.BLL.Inspection;
using Accela.ACA.Common;
using Accela.ACA.Common.Common;
using Accela.ACA.Common.Util;
using Accela.ACA.Inspection;
using Accela.ACA.Web.AppCode.Common;
using Accela.ACA.Web.Common;
using Accela.ACA.Web.Common.Control;
using Accela.ACA.WSProxy;
using Accela.Web.Controls;

namespace Accela.ACA.Web.Component
{
    /// <summary>
    /// The upload inspection list component.
    /// </summary>
    public partial class UploadInspectionList : BaseUserControl
    {
        #region Fields

        /// <summary>
        /// The export file name for new inspection list.
        /// </summary>
        private const string EXPORT_FILENAME_NEW_INSPECTION = "NewInspectionList";

        /// <summary>
        /// The export file name for resulted inspection list.
        /// </summary>
        private const string EXPORT_FILENAME_RESULTED_INSPECTION = "ResultedInspectionList";

        /// <summary>
        /// The inspection list status for new inspection.
        /// </summary>
        private const string INSPECTION_LIST_STATUS_NEW = "NEW";

        /// <summary>
        /// The inspection list status for result inspection.
        /// </summary>
        private const string INSPECTION_LIST_STATUS_RESULT = "RESULT";

        #endregion Fields

        #region Properties

        /// <summary>
        /// Gets or sets the grid view number.
        /// </summary>
        public string GridViewNumber { get; set; }

        /// <summary>
        /// Gets or sets the data source.
        /// </summary>
        public DataTable GridViewDataSource
        {
            get
            {
                if (AppSession.IsAdmin)
                {
                    return null;
                }

                return ViewState["GridViewDataSource"] as DataTable;
            }

            set
            {
                ViewState["GridViewDataSource"] = value;
            }
        }

        /// <summary>
        /// Gets the page size.
        /// </summary>
        public int PageSize
        {
            get
            {
                return gdvInspectionList.PageSize;
            }
        }

        #endregion Properties

        #region Protected Methods

        /// <summary>
        /// <c>OnInit</c> event method.
        /// </summary>
        /// <param name="e">A System.EventArgs object containing the event data.</param>
        protected override void OnInit(EventArgs e)
        {
            gdvInspectionList.GridViewNumber = GridViewNumber;

            GridViewBuildHelper.SetSimpleViewElements(gdvInspectionList, string.Empty, AppSession.IsAdmin);

            // hide the attachment column in New Inspection List
            if (GridViewNumber == GviewID.NewInspectionList)
            {
                foreach (var column in gdvInspectionList.Columns)
                {
                    AccelaTemplateField templateField = column as AccelaTemplateField;

                    if (templateField != null && (templateField.ColumnId == "AttachmentExpandLogo" || templateField.ColumnId == "AttachmentExpandContent"))
                    {
                        templateField.Visible = false;
                    }
                }
            }

            if (!AppSession.IsAdmin && StandardChoiceUtil.IsEnableExport2CSV())
            {
                gdvInspectionList.ShowExportLink = true;
                gdvInspectionList.ExportFileName = GridViewNumber == GviewID.NewInspectionList ? EXPORT_FILENAME_NEW_INSPECTION : EXPORT_FILENAME_RESULTED_INSPECTION;
            }
            else
            {
                gdvInspectionList.ShowExportLink = false;
            }

            base.OnInit(e);
        }

        /// <summary>
        /// Page load event method.
        /// </summary>
        /// <param name="sender">The sender object.</param>
        /// <param name="e">A System.EventArgs object containing the event data.</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Page.IsPostBack)
            {
                gdvInspectionList.DataSource = GridViewDataSource;
                gdvInspectionList.DataBind();
            }

            if (!IsPostBack && !AppSession.IsAdmin)
            {
                BindInspectionList(0, null);
            }
            else if (AppSession.IsAdmin)
            {
                Display(null, null);
            }
        }

        /// <summary>
        /// Handles the grid view download event.
        /// </summary>
        /// <param name="sender">Object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void InspectionList_GridViewDownload(object sender, GridViewDownloadEventArgs e)
        {
            // execute the download event that setting the exported content.
            GridViewBuildHelper.DownloadAll(
                                            sender,
                                            e, 
                                            (QueryFormat queryFormat) => 
                                            {         
                                                PaginationModel pageInfo = new PaginationModel();
                                                DownloadResultModel resultModel = GetInspectionListByQueryFormat(queryFormat, ref pageInfo);
                                                return resultModel;
                                            });
        }

        /// <summary>
        /// Fire page index changing event
        /// </summary>
        /// <param name="sender">Object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void InspectionList_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            PaginationModel pageInfo = PaginationUtil.GetPageInfoByID(ClientID);

            if (e.NewPageIndex > pageInfo.EndPage)
            {
                BindInspectionList(e.NewPageIndex, pageInfo.SortExpression);
            }
        }

        /// <summary>
        /// Fire sorted event.
        /// </summary>
        /// <param name="sender">Object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void InspectionList_GridViewSort(object sender, AccelaGridViewSortEventArgs e)
        {
            PaginationModel pageInfo = PaginationUtil.GetPageInfoByID(ClientID);
            pageInfo.SortExpression = e.GridViewSortExpression;

            GridViewDataSource.DefaultView.Sort = e.GridViewSortExpression;
            GridViewDataSource = GridViewDataSource.DefaultView.ToTable();
        }

        /// <summary>
        /// Row data bound event.
        /// </summary>
        /// <param name="sender">The sender object.</param>
        /// <param name="e">The GridViewRowEventArgs.</param>
        protected void InspectionList_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                DataRowView row = (DataRowView)e.Row.DataItem;

                HyperLink hlRecordId = (HyperLink)e.Row.FindControl("hlRecordId");
                HyperLink hlInspectionType = (HyperLink)e.Row.FindControl("hlInspectionType");
                AccelaLabel lblRecordIdLabel = (AccelaLabel)e.Row.FindControl("lblRecordIdLabel");
                AccelaLabel lblInspectionTypeLabel = (AccelaLabel)e.Row.FindControl("lblInspectionTypeLabel");

                ActivityModel activity = row["Activity"] as ActivityModel;
                bool hasRecordPermission = false;

                CapWithConditionModel4WS capWithConditionModel = null;

                // indicates that if have permission to view cap detail/inspection detail.
                if (activity != null && activity.capID != null)
                {
                    CapIDModel4WS capIdModel = new CapIDModel4WS
                                                   {
                                                       id1 = activity.capID.ID1,
                                                       id2 = activity.capID.ID2,
                                                       id3 = activity.capID.ID3,
                                                       serviceProviderCode = activity.capID.serviceProviderCode
                                                   };

                    capWithConditionModel = CapUtil.GetCapWithConditionModel4WS(capIdModel, AppSession.User.UserSeqNum, true);

                    if (capWithConditionModel != null && capWithConditionModel.capModel != null)
                    {
                        hasRecordPermission = true;
                    }
                }

                // Set record id field
                if (hasRecordPermission)
                {
                    SetRecordDetailLink(hlRecordId, activity);
                }
                else
                {
                    hlRecordId.Visible = false;
                    lblRecordIdLabel.Visible = true;
                }

                // Set inspection type field
                bool hasInspectionPermission = false;
                
                if (hasRecordPermission && capWithConditionModel.capModel.capID != null)
                {
                    Dictionary<string, UserRolePrivilegeModel> sectionPermissions = CapUtil.GetSectionPermissions(capWithConditionModel.capModel.capID.serviceProviderCode, capWithConditionModel.capModel.moduleName);
                    hasInspectionPermission = CapUtil.GetSectionVisibility(CapDetailSectionType.INSPECTIONS.ToString(), sectionPermissions, capWithConditionModel.capModel);
                }

                if (hasInspectionPermission)
                {
                    SetInspectionDetailLink(hlInspectionType, activity);
                }
                else
                {
                    hlInspectionType.Visible = false;
                    lblInspectionTypeLabel.Visible = true;
                }
            }
            else if (e.Row.RowType == DataControlRowType.Header)
            {
                SetLabelKey();
            }
        }

        /// <summary>
        /// Handles the OnPreRender event of the control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected void InspectionList_PreRender(object sender, EventArgs e)
        {
            /*             
             * 1. If GridViewDataSource is not null, then set the grid view header in the InspectionList_RowDataBound event,
             *    because the export method execute faster then the InspectionList_PreRender event when click download link.
             * 2. If GridViewDataSource is null, then execute this event to set the grid view header while the InspectionList_RowDataBound event not execute.
             */
            if (GridViewDataSource != null && GridViewDataSource.Rows.Count > 0)
            {
                return;
            }

            SetLabelKey();
        }

        /// <summary>
        /// Download inspection attachment event.
        /// </summary>
        /// <param name="sender">The sender object.</param>
        /// <param name="e">The EventArgs.</param>
        protected void DownloadInspectionAttchmentLink_Click(object sender, EventArgs e)
        {
            try
            {
                string senderArgs = Request.Form["__EVENTARGUMENT"];
                string[] documentValue = senderArgs.Split(ACAConstant.SPLIT_CHAR);

                if (documentValue != null && documentValue.Length >= 5)
                {
                    string agencyCode = documentValue[0];
                    string documentNo = documentValue[1];
                    string fileKey = documentValue[2];
                    string entityId = documentValue[3];
                    string entityType = documentValue[4];
                    string moduleName = documentValue[5];

                    string downloadUrl = string.Format(
                            "Handlers/FileHandler.ashx?action=Download&agency={0}&{1}={2}&entityId={3}&entityType={4}&{5}={6}&fileKey={7}", agencyCode, ACAConstant.MODULE_NAME, moduleName, entityId, entityType, UrlConstant.DOCUMENT_NO, documentNo, fileKey);

                    Response.Redirect(FileUtil.AppendApplicationRoot(downloadUrl));
                }
            }
            catch (ACAException)
            {
                MessageUtil.ShowMessage(Page, MessageType.Error, GetTextByKey("aca_common_technical_difficulty"), false, -1);
            }
        }

        #endregion Protected Methods

        #region Private Methods

        /// <summary>
        /// Sets the link for record detail.
        /// </summary>
        /// <param name="hyperLink">The HyperLink control.</param>
        /// <param name="activity">The activity model.</param>
        private void SetRecordDetailLink(HyperLink hyperLink, ActivityModel activity)
        {
            if (activity == null || activity.capID == null)
            {
                return;
            }

            CapIDModel capIDModel = activity.capID;
            string moduleName = activity.capType != null ? activity.capType.moduleName : string.Empty;

            string url = string.Format(
                                        "~/Cap/CapDetail.aspx?Module={0}&TabName={0}&capID1={1}&capID2={2}&capID3={3}&{4}={5}",
                                        ScriptFilter.AntiXssUrlEncode(moduleName),
                                        capIDModel.ID1,
                                        capIDModel.ID2,
                                        capIDModel.ID3,
                                        UrlConstant.AgencyCode,
                                        Server.UrlEncode(capIDModel.serviceProviderCode));

            hyperLink.NavigateUrl = ResolveUrl(url);
        }

        /// <summary>
        /// Sets the links for inspection detail.
        /// </summary>
        /// <param name="hyperLink">The hyper link</param>
        /// <param name="activity">The activity model</param>
        private void SetInspectionDetailLink(HyperLink hyperLink, ActivityModel activity)
        {
            if (activity == null || activity.capID == null)
            {
                return;
            }

            string url = string.Format(
                "/Inspection/InspectionDetails.aspx?{0}={1}&Module={2}&ID={3}&RecordID1={4}&RecordID2={5}&RecordID3={6}&{7}={8}&{9}={10}",
                UrlConstant.AgencyCode,
                activity.capID.serviceProviderCode,
                activity.capType != null ? activity.capType.group : string.Empty,
                activity.idNumber,
                activity.capID.ID1,
                activity.capID.ID2,
                activity.capID.ID3,
                UrlConstant.IS_POPUP_PAGE,
                ACAConstant.COMMON_Y,
                UrlConstant.HIDE_ACTION_BUTTON,
                ACAConstant.COMMON_Y);

            hyperLink.NavigateUrl = string.Format("javascript:ShowInspectionDetail('{0}')", FileUtil.AppendApplicationRoot(url));
        }

        /// <summary>
        /// Get the only time part.
        /// </summary>
        /// <param name="dateString">The date string.</param>
        /// <param name="amOrPmValue">The AM or PM</param>
        /// <param name="specificTimeValue">The time value.</param>
        /// <returns>The datetime object.</returns>
        private string GetOnlyTimePart(string dateString, string amOrPmValue, string specificTimeValue)
        {
            string result = string.Empty;

            if (!string.IsNullOrEmpty(specificTimeValue))
            {
                result = specificTimeValue;

                if ("AM".Equals(amOrPmValue, StringComparison.InvariantCultureIgnoreCase)
                    || "PM".Equals(amOrPmValue, StringComparison.InvariantCultureIgnoreCase))
                {
                    string parsingString = string.Format("{0} {1} {2}", dateString, specificTimeValue, amOrPmValue);
                    DateTime parsingResult;

                    if (I18nDateTimeUtil.TryParseFromWebService(parsingString, out parsingResult))
                    {
                        result = parsingResult.ToString(I18nDateTimeUtil.ShortTimePattern, I18nDateTimeUtil.CustomizedDateTimeFormatInfo);
                    }
                }
            }
            else
            {
                switch (amOrPmValue)
                {
                    case "AM":
                        result = LabelUtil.GetTextByKey("aca_calendar_morning", ModuleName);
                        break;
                    case "PM":
                        result = LabelUtil.GetTextByKey("aca_calendar_afternoon", ModuleName);
                        break;
                }
            }

            return result;
        }

        /// <summary>
        /// Set the label key by the view id
        /// </summary>
        private void SetLabelKey()
        {
            GridViewRow headerRow = GridViewBuildHelper.GetHeaderRow(gdvInspectionList);

            if (headerRow == null)
            {
                return;
            }

            if (GridViewNumber == GviewID.NewInspectionList)
            {
                ((IAccelaNonInputControl)headerRow.FindControl("lnkRecordIdHeader")).LabelKey = "aca_newinspectionlist_label_recordid";
                ((IAccelaNonInputControl)headerRow.FindControl("lnkInspectionTypeHeader")).LabelKey = "aca_newinspectionlist_label_inspectiontype";
                ((IAccelaNonInputControl)headerRow.FindControl("lnkAddressHeader")).LabelKey = "aca_newinspectionlist_label_address";
                ((IAccelaNonInputControl)headerRow.FindControl("lnkScheduledDateHeader")).LabelKey = "aca_newinspectionlist_label_scheduleddate";
                ((IAccelaNonInputControl)headerRow.FindControl("lnkScheduledStartTimeHeader")).LabelKey = "aca_newinspectionlist_label_scheduledstarttime";
                ((IAccelaNonInputControl)headerRow.FindControl("lnkScheduledEndTimeHeader")).LabelKey = "aca_newinspectionlist_label_scheduledendtime";
                ((IAccelaNonInputControl)headerRow.FindControl("lnkStatusHeader")).LabelKey = "aca_newinspectionlist_label_status";
            }
            else
            {
                ((IAccelaNonInputControl)headerRow.FindControl("lnkRecordIdHeader")).LabelKey = "aca_resultedinspectionlist_label_recordid";
                ((IAccelaNonInputControl)headerRow.FindControl("lnkInspectionTypeHeader")).LabelKey = "aca_resultedinspectionlist_label_inspectiontype";
                ((IAccelaNonInputControl)headerRow.FindControl("lnkAddressHeader")).LabelKey = "aca_resultedinspectionlist_label_address";
                ((IAccelaNonInputControl)headerRow.FindControl("lnkInspectionDateHeader")).LabelKey = "aca_resultedinspectionlist_label_inspectiondate";
                ((IAccelaNonInputControl)headerRow.FindControl("lnkStatusHeader")).LabelKey = "aca_resultedinspectionlist_label_status";
                ((IAccelaNonInputControl)headerRow.FindControl("lnkScoreHeader")).LabelKey = "aca_resultedinspectionlist_label_score";
                ((IAccelaNonInputControl)headerRow.FindControl("lnkGradeHeader")).LabelKey = "aca_resultedinspectionlist_label_grade";
            }
        }

        /// <summary>
        /// Bind the new inspection list.
        /// </summary>
        /// <param name="currentPageIndex">The current page index</param>
        /// <param name="sortExpression">The sort expression</param>
        private void BindInspectionList(int currentPageIndex, string sortExpression)
        {
            PaginationModel pageInfo = PaginationUtil.GetPageInfoByID(ClientID);
            pageInfo.SortExpression = sortExpression;
            pageInfo.CurrentPageIndex = currentPageIndex;
            pageInfo.CustomPageSize = PageSize;
            QueryFormat queryFormat = PaginationUtil.GetQueryFormatModel(pageInfo);

            DataTable dtInspectionList = GetInspectionListByQueryFormat(queryFormat, ref pageInfo).DataSource;
            dtInspectionList = PaginationUtil.MergeDataSource(GridViewDataSource, dtInspectionList, pageInfo);

            Display(dtInspectionList, pageInfo.SortExpression);
        }

        /// <summary>
        /// Get inspection list by query criteria
        /// </summary>
        /// <param name="queryFormat">Query format</param>
        /// <param name="pageInfo">Pagination model</param>
        /// <returns>Download result model that resulted inspection list</returns>
        private DownloadResultModel GetInspectionListByQueryFormat(QueryFormat queryFormat, ref PaginationModel pageInfo)
        {
            string status = GridViewNumber == GviewID.NewInspectionList ? INSPECTION_LIST_STATUS_NEW : INSPECTION_LIST_STATUS_RESULT;
            IInspectionBll inspectionBll = ObjectFactory.GetObject<IInspectionBll>();
            SearchResultModel resultModel = inspectionBll.GetMyInspections(ConfigManager.AgencyCode, status, queryFormat);

            DataTable dtInspectionList = null;

            if (resultModel != null)
            {
                pageInfo.StartDBRow = resultModel.startRow;
                InspectionModel[] inspectionList = ObjectConvertUtil.ConvertObjectArray2EntityArray<InspectionModel>(resultModel.resultList);
                dtInspectionList = CreateDataSource(inspectionList);
            }

            DownloadResultModel model = new DownloadResultModel();
            model.StartDBRow = pageInfo.StartDBRow;
            model.DataSource = dtInspectionList;

            return model;
        }

        /// <summary>
        /// Create data source use inspection list.
        /// </summary>
        /// <param name="inspectionList">The inspection list.</param>
        /// <returns>The data source.</returns>
        private DataTable CreateDataSource(InspectionModel[] inspectionList)
        {
            if (inspectionList == null || inspectionList.Length == 0)
            {
                return null;
            }

            DataTable dt = new DataTable();
            dt.Columns.Add("RecordId", typeof(string));
            dt.Columns.Add("InspectionSeqNbr", typeof(long));
            dt.Columns.Add("InspectionType", typeof(string));
            dt.Columns.Add("Address", typeof(string));
            dt.Columns.Add("ScheduledDate", typeof(DateTime));
            dt.Columns.Add("ScheduledStartTime", typeof(string));
            dt.Columns.Add("ScheduledEndTime", typeof(string));
            dt.Columns.Add("InspectionDate", typeof(DateTime));
            dt.Columns.Add("Status", typeof(string));
            dt.Columns.Add("Score", typeof(long));
            dt.Columns.Add("Grade", typeof(string));
            dt.Columns.Add("Activity", typeof(ActivityModel));
            dt.Columns.Add("InspectionEntity", typeof(string));

            string minDateString = I18nDateTimeUtil.FormatToDateStringForWebService(DateTime.MinValue);

            foreach (InspectionModel model in inspectionList)
            {
                if (model == null)
                {
                    continue;
                }

                DataRow dr = dt.NewRow();
                ActivityModel activity = model.activity;

                if (activity != null)
                {
                    dr["RecordId"] = activity.capID != null ? activity.capID.customID : string.Empty;
                    dr["InspectionSeqNbr"] = activity.idNumber;

                    if (activity.activityDate != null)
                    {
                        dr["ScheduledDate"] = activity.activityDate.Value;
                    }
                    else
                    {
                        dr["ScheduledDate"] = DBNull.Value;
                    }

                    string scheduledStartTime = GetOnlyTimePart(minDateString, activity.time1, activity.time2);

                    if (!string.IsNullOrEmpty(scheduledStartTime))
                    {
                        dr["ScheduledStartTime"] = scheduledStartTime;
                    }
                    else
                    {
                        dr["ScheduledStartTime"] = DBNull.Value;
                    }

                    string scheduledEndTime = GetOnlyTimePart(minDateString, activity.actEndTime1, activity.actEndTime2);

                    if (!string.IsNullOrEmpty(scheduledEndTime))
                    {
                        dr["ScheduledEndTime"] = scheduledEndTime;
                    }
                    else
                    {
                        dr["ScheduledEndTime"] = DBNull.Value;
                    }

                    if (model.activity.completionDate != null)
                    {
                        dr["InspectionDate"] = model.activity.completionDate.Value;
                    }
                    else
                    {
                        dr["InspectionDate"] = DBNull.Value;
                    }

                    dr["Status"] = activity.status;
                    dr["Score"] = activity.totalScore;
                    dr["Grade"] = activity.grade;

                    // in resulted inspection list, need set the InspectionEntity column to get/upload inspection attachment
                    // in resulted inspection list, need set the InspectionEntity column to get/upload inspection attachment
                    if (GridViewNumber == GviewID.ResultedInspectionList && activity.capID != null)
                    {
                        // set the inspection document upload permission
                        CapIDModel4WS capIdModel = new CapIDModel4WS
                        {
                            id1 = activity.capID.ID1,
                            id2 = activity.capID.ID2,
                            id3 = activity.capID.ID3,
                            serviceProviderCode = activity.capID.serviceProviderCode
                        };

                        string moduleName = activity.capType != null ? activity.capType.group : string.Empty;

                        EdmsPolicyModel4WS edmsPolicyModel = CapUtil.GetEdmsPolicyModel4WS(ConfigManager.AgencyCode, moduleName, AppSession.User.PublicUserId, capIdModel);
                        bool hasEDMSUploadRight = false;
                        
                        if (edmsPolicyModel != null)
                        {
                            hasEDMSUploadRight = ValidationUtil.IsTrue(edmsPolicyModel.uploadRight);
                        }

                        dr["InspectionEntity"] = string.Format(
                            "{1}{0}{2}{0}{3}{0}{4}{0}{5}{0}{6}{0}{7}",
                            ACAConstant.SPLIT_CHAR,
                            activity.capID.serviceProviderCode,
                            activity.capID.ID1,
                            activity.capID.ID2,
                            activity.capID.ID3,
                            activity.idNumber,
                            moduleName,
                            hasEDMSUploadRight ? ACAConstant.COMMON_Y : ACAConstant.COMMON_N);
                    }
                }

                dr["InspectionType"] = model.inspectionType;
                dr["Activity"] = model.activity;

                if (model.primaryAddress != null)
                {
                    IAddressBuilderBll addressBuilderBll = ObjectFactory.GetObject<IAddressBuilderBll>();
                    dr["Address"] = addressBuilderBll.BuildAddressByFormatType(model.primaryAddress, null, AddressFormatType.SHORT_ADDRESS_NO_FORMAT);
                }

                dt.Rows.Add(dr);
            }

            return dt;
        }

        /// <summary>
        /// Display the inspection list.
        /// </summary>
        /// <param name="dataSource">The inspection list data source.</param>
        /// <param name="sort">String for sort</param>
        private void Display(DataTable dataSource, string sort)
        {
            if (sort == null)
            {
                if (GridViewNumber == GviewID.NewInspectionList)
                {
                    sort = "ScheduledDate DESC";
                }
                else if (GridViewNumber == GviewID.ResultedInspectionList)
                {
                    sort = "InspectionDate DESC";
                }
            }

            string[] s = sort.Trim().Split(' ');

            if (s.Length == 2 && dataSource != null)
            {
                gdvInspectionList.GridViewSortExpression = s[0];
                gdvInspectionList.GridViewSortDirection = s[1];
                DataView dataView = new DataView(dataSource);
                dataView.Sort = sort;
                dataSource = dataView.ToTable();
            }

            GridViewDataSource = dataSource;
            gdvInspectionList.DataSource = GridViewDataSource;
            gdvInspectionList.DataBind();
        }

        #endregion Private Methods
    }
}