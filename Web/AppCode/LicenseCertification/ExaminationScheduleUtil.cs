#region Header
/**
 * <pre>
 * 
 *  Accela Citizen Access
 *  File: ExaminationScheduleUtil.cs
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
using System.Web;

using Accela.ACA.BLL.AddressBuilder;
using Accela.ACA.BLL.Common;
using Accela.ACA.Common;
using Accela.ACA.Common.Util;
using Accela.ACA.Web.Common;
using Accela.ACA.WSProxy;

namespace Accela.ACA.Web.Examination
{
    /// <summary>
    /// Examination Schedule Wizard Page Name
    /// </summary>
    public enum ExaminationScheduleWizardPageName
    {
        /// <summary>
        /// unknown page
        /// </summary>
        Unknown,

        /// <summary>
        /// page name for <c>AvailableExaminationList.aspx</c>
        /// </summary>
        ExamList,

        /// <summary>
        /// page name for <c>ExaminationRescheduleReason.aspx</c>
        /// </summary>
        ExamRescheduleReason,

        /// <summary>
        /// page name for <c>AvailableExaminationScheduleList.aspx</c>
        /// </summary>
        ExamScheduleList,

        /// <summary>
        /// page name for <c>ExaminationScheduleConfirm.aspx</c>
        /// </summary>
        ExamScheduleConfirm,
    }

    /// <summary>
    /// Examination Schedule Utility
    /// </summary>
    public class ExaminationScheduleUtil
    {
        #region Fields

        /// <summary>
        /// page name for exam name list
        /// </summary>
        public const string EXAM_SCHEDULE_PAGE_EXAMLIST = "Examination/AvailableExaminationList.aspx";

        /// <summary>
        /// page name for exam schedule list
        /// </summary>
        public const string EXAM_SCHEDULE_PAGE_EXAMSCHEDULELIST = "Examination/AvailableExaminationScheduleList.aspx";

        /// <summary>
        /// page name for exam schedule confirm
        /// </summary>
        public const string EXAM_SCHEDULE_PAGE_EXAMSCHEDULECONFIRM = "Examination/ExaminationScheduleConfirm.aspx";

        /// <summary>
        /// Examination url of reschedule Reason page
        /// </summary>
        private const string EXAMINATION_URL_RESCHEDULE_REASON = "Examination/ExaminationRescheduleReason.aspx";

        #endregion

        /// <summary>
        /// Get the totals the fee,.
        /// </summary>
        /// <param name="feeItems">The fee items.</param>
        /// <returns>
        /// There two possible type return.
        /// 1. sum the fee amount
        /// 2. if some fee item need be calculate, return <c>"to be calc"</c>
        /// </returns>
        public static string TotalFee(RefFeeDsecVO[] feeItems)
        {
            double totalFee = 0;
            bool isTobeCalc = false;
            string toBeCalc = string.Empty;

            if (feeItems != null)
            {
                foreach (var feeItem in feeItems)
                {
                    double fee = 0;

                    if (I18nNumberUtil.TryParseMoneyFromWebService(feeItem.feeAmount, out fee))
                    {
                        //if try to parse successful
                        totalFee += fee;
                    }
                    else
                    {
                        //if failed to parse, it is "to be calc" value on this fee item.
                        //return this value to display.
                        toBeCalc = feeItem.feeAmount;
                        isTobeCalc = true;
                        break;
                    }
                }
            }

            return isTobeCalc ? toBeCalc : I18nNumberUtil.FormatNumberForWebService(totalFee);
        }

        /// <summary>
        /// Converts the ref examination model to data table.
        /// </summary>
        /// <param name="refExamModels">The ref exam models.</param>
        /// <param name="selectExamination">The select examination.</param>
        /// <returns>examination name table</returns>
        public static DataTable ConvertRefExaminationModelToDataTable(RefExaminationModel4WS[] refExamModels, string selectExamination)
        {
            var dataTable = CreatDataTable();

            if (refExamModels != null)
            {
                foreach (var refExaminationModel in refExamModels)
                {
                    DataRow dataRow = dataTable.NewRow();

                    dataRow["ExaminationName"] = refExaminationModel.examName;
                    dataRow["ExaminationID"] = refExaminationModel.refExamNbr;
                    
                    dataRow["Enabled"] = true;

                    if (!string.IsNullOrEmpty(selectExamination) && long.Parse(selectExamination) == refExaminationModel.refExamNbr)
                    {
                        dataRow["Selected"] = true;
                    }
                    else
                    {
                        dataRow["Selected"] = false;
                    }

                    dataTable.Rows.Add(dataRow);
                }
            }

            return dataTable;
        }

        /// <summary>
        /// Converts the reason list to data table.
        /// </summary>
        /// <param name="reasons">The reasons.</param>
        /// <param name="selectReason">The select reason.</param>
        /// <returns>The data table that convert from reason list.</returns>
        public static DataTable ConvertReasonListToDataTable(BizDomainModel4WS[] reasons, string selectReason)
        {
            var dataTable = CreatReasonDataTable();

            if (reasons != null && reasons.Length > 0)
            {
                foreach (var reason in reasons)
                {
                    DataRow dataRow = dataTable.NewRow();

                    dataRow["ReasonString"] = reason.bizdomainValue;
                    dataRow["ID"] = reason.dispositionID;

                    dataRow["Enabled"] = true;

                    if (!string.IsNullOrEmpty(selectReason) && int.Parse(selectReason) == reason.dispositionID)
                    {
                        dataRow["Selected"] = true;
                    }
                    else
                    {
                        dataRow["Selected"] = false;
                    }

                    dataTable.Rows.Add(dataRow);
                }
            }

            return dataTable;
        }

        /// <summary>
        /// Converts the ref examination model to data table.
        /// </summary>
        /// <param name="examScheduleViewModels">The exam schedule view models.</param>
        /// <returns>examination schedule list table</returns>
        public static DataTable ConvertExaminationSchduleModelToDataTable(ExamScheduleViewModel[] examScheduleViewModels)
        {
            var dataTable = CreatScheduleDataTable();

            if (examScheduleViewModels != null)
            {
                int i = 0;

                foreach (var examScheduleViewModel in examScheduleViewModels)
                {
                    DataRow dataRow = dataTable.NewRow();

                    dataRow[ColumnConstant.RefExaminationScheduleDetail.ExaminationScheduleID.ToString()] = i.ToString();
                    dataRow[ColumnConstant.RefExaminationScheduleDetail.Provider.ToString()] = examScheduleViewModel.providerName;
                    dataRow[ColumnConstant.RefExaminationScheduleDetail.Fee.ToString()] = examScheduleViewModel.totalFeeAmount;
                    dataRow[ColumnConstant.RefExaminationScheduleDetail.Date.ToString()] = examScheduleViewModel.date == null ? DateTime.MinValue : examScheduleViewModel.date.Value;
                    dataRow[ColumnConstant.RefExaminationScheduleDetail.WeekyDay.ToString()] =
                        examScheduleViewModel.date == null ? string.Empty : I18nCultureUtil.UserPreferredCultureInfo.DateTimeFormat.DayNames[(int)examScheduleViewModel.date.Value.DayOfWeek];
                    dataRow[ColumnConstant.RefExaminationScheduleDetail.StartTime.ToString()] = examScheduleViewModel.startTime == null ? DateTime.MinValue : examScheduleViewModel.startTime.Value;
                    dataRow[ColumnConstant.RefExaminationScheduleDetail.EndTime.ToString()] = examScheduleViewModel.endTime == null ? DateTime.MinValue : examScheduleViewModel.endTime.Value;
                    dataRow[ColumnConstant.RefExaminationScheduleDetail.AvailableSeats.ToString()] = examScheduleViewModel.availableSeats;
                    dataRow[ColumnConstant.RefExaminationScheduleDetail.CalendarId.ToString()] = examScheduleViewModel.calendarID == null ? string.Empty : examScheduleViewModel.calendarID.Value.ToString();
                    dataRow[ColumnConstant.RefExaminationScheduleDetail.ScheduleId.ToString()] = examScheduleViewModel.scheduleID == null ? string.Empty : examScheduleViewModel.scheduleID.Value.ToString();
                    dataRow[ColumnConstant.RefExaminationScheduleDetail.LocationId.ToString()] = examScheduleViewModel.locationID == null ? string.Empty : examScheduleViewModel.locationID.Value.ToString();
                    dataRow[ColumnConstant.RefExaminationScheduleDetail.ProviderNbr.ToString()] = examScheduleViewModel.providerNbr;
                    dataRow[ColumnConstant.RefExaminationScheduleDetail.IsExternal.ToString()] = examScheduleViewModel.external.ToString();
                    dataRow[ColumnConstant.RefExaminationScheduleDetail.ExamInstructions.ToString()] = examScheduleViewModel.instructions;
                    RProviderLocationModel lc = examScheduleViewModel.locationModel;

                    if (lc != null)
                    {
                        IAddressBuilderBll addressBuilderBll = ObjectFactory.GetObject(typeof(IAddressBuilderBll)) as IAddressBuilderBll;
                        string site = addressBuilderBll.BuildAddress4ProviderLocation(lc);
                        dataRow[ColumnConstant.RefExaminationScheduleDetail.ExaminationSite.ToString()] = site;
                        dataRow[ColumnConstant.RefExaminationScheduleDetail.HandicapAccessible.ToString()] = lc.isHandicapAccessible;
                        dataRow[ColumnConstant.RefExaminationScheduleDetail.AccessiblityDesc.ToString()] = lc.handicapAccessible;
                        dataRow[ColumnConstant.RefExaminationScheduleDetail.DrivingDesc.ToString()] = lc.drivingDirections;
                    }

                    dataRow[ColumnConstant.RefExaminationScheduleDetail.IsOnline.ToString()] = "true";
                    dataRow["Selected"] = false;
                    dataRow["Enabled"] = true;
                    
                    dataTable.Rows.Add(dataRow);
                    i++;
                }
            }

            return dataTable;
        }

        /// <summary>
        /// Gets the wizard previous URL.
        /// </summary>
        /// <param name="urlParameters">The URL parameters.</param>
        /// <returns>
        /// the wizard previous URL.
        /// </returns>
        public static string GetWizardPreviousURL(ExaminationParameter urlParameters)
        {
            string result = string.Empty;
            var beginPage = ExaminationScheduleWizardPageName.ExamList;
            var currentPage = GetCurrentPageName(HttpContext.Current.Request.Path);
            var previousPage = ExaminationScheduleWizardPageName.Unknown;

            if (currentPage == ExaminationScheduleWizardPageName.ExamList)
            {
                previousPage = ExaminationScheduleWizardPageName.Unknown;
            }
            else if (currentPage == ExaminationScheduleWizardPageName.ExamRescheduleReason)
            {
                previousPage = ExaminationScheduleWizardPageName.Unknown;
            }
            else if (currentPage == ExaminationScheduleWizardPageName.ExamScheduleList)
            {
                if (bool.Parse(urlParameters.IsReschedule))
                {
                    previousPage = ExaminationScheduleWizardPageName.ExamRescheduleReason;
                }
                else
                {
                    previousPage = ExaminationScheduleWizardPageName.ExamList;
                }
            }
            else if (currentPage == ExaminationScheduleWizardPageName.ExamScheduleConfirm)
            {
                previousPage = ExaminationScheduleWizardPageName.ExamScheduleList;
            }

            if (previousPage < beginPage || previousPage >= currentPage)
            {
                previousPage = ExaminationScheduleWizardPageName.Unknown;
            }

            if (previousPage != ExaminationScheduleWizardPageName.Unknown)
            {
                string relativeURL = GetRelativeURL(previousPage);
                string absolateURL = FileUtil.AppendApplicationRoot(relativeURL);
                result = string.Concat(absolateURL, "?", HttpContext.Current.Request.QueryString);
            }

            return result;
        }

        /// <summary>
        /// Converts the data row to model.
        /// </summary>
        /// <param name="dataRow">The data row.</param>
        /// <param name="examScheduleViewModel">The exam schedule view model.</param>
        public static void ConvertDataRowToModel(DataRow dataRow, ref ExaminationParameter examScheduleViewModel)
        {
            examScheduleViewModel.ScheduleID = dataRow[ColumnConstant.RefExaminationScheduleDetail.ExaminationScheduleID.ToString()].ToString();
            examScheduleViewModel.ExamScheduleProviderName = dataRow[ColumnConstant.RefExaminationScheduleDetail.Provider.ToString()].ToString();
            examScheduleViewModel.ExamScheduleTotalFee = dataRow[ColumnConstant.RefExaminationScheduleDetail.Fee.ToString()].ToString();
            examScheduleViewModel.ExamScheduleDate =
                (DateTime)dataRow[ColumnConstant.RefExaminationScheduleDetail.Date.ToString()] == DateTime.MinValue
                    ? string.Empty
                    : I18nDateTimeUtil.FormatToDateStringForUI(
                        (DateTime)dataRow[ColumnConstant.RefExaminationScheduleDetail.Date.ToString()]);
            examScheduleViewModel.ExamScheduleWeekDay = dataRow[ColumnConstant.RefExaminationScheduleDetail.WeekyDay.ToString()].ToString();
            examScheduleViewModel.ExamScheduleStartTime =
                 (DateTime)dataRow[ColumnConstant.RefExaminationScheduleDetail.StartTime.ToString()] == DateTime.MinValue
                    ? string.Empty
                    : I18nDateTimeUtil.FormatToTimeStringForUI(
                        (DateTime)dataRow[ColumnConstant.RefExaminationScheduleDetail.StartTime.ToString()], false);
            examScheduleViewModel.ExamScheduleEndTime =
                 (DateTime)dataRow[ColumnConstant.RefExaminationScheduleDetail.EndTime.ToString()] == DateTime.MinValue
                    ? string.Empty
                    : I18nDateTimeUtil.FormatToTimeStringForUI(
                        (DateTime)dataRow[ColumnConstant.RefExaminationScheduleDetail.EndTime.ToString()], false);
            examScheduleViewModel.ExamScheduleLocation = dataRow[ColumnConstant.RefExaminationScheduleDetail.ExaminationSite.ToString()].ToString();
            examScheduleViewModel.ExamScheduleHandlecap = dataRow[ColumnConstant.RefExaminationScheduleDetail.HandicapAccessible.ToString()].ToString();
            examScheduleViewModel.ExamScheduleSeats = dataRow[ColumnConstant.RefExaminationScheduleDetail.AvailableSeats.ToString()].ToString();
            examScheduleViewModel.ExamScheduleCalendarId = dataRow[ColumnConstant.RefExaminationScheduleDetail.CalendarId.ToString()].ToString();
            examScheduleViewModel.ExamScheduleScheduleId = dataRow[ColumnConstant.RefExaminationScheduleDetail.ScheduleId.ToString()].ToString();
            examScheduleViewModel.ExamScheduleLocationId = dataRow[ColumnConstant.RefExaminationScheduleDetail.LocationId.ToString()].ToString();
            examScheduleViewModel.ExamScheduleProviderNbr = dataRow[ColumnConstant.RefExaminationScheduleDetail.ProviderNbr.ToString()].ToString();
            examScheduleViewModel.IsExternal = dataRow[ColumnConstant.RefExaminationScheduleDetail.IsExternal.ToString()].ToString();
            examScheduleViewModel.IsOnline = dataRow[ColumnConstant.RefExaminationScheduleDetail.IsOnline.ToString()].ToString();
        }

        /// <summary>
        /// Gets the primary cap contact model.
        /// </summary>
        /// <param name="recordModel">The record model.</param>
        /// <returns>The primary cap contact model.</returns>
        public static CapContactModel4WS GetCapPrimaryContact(CapModel4WS recordModel)
        {
            CapContactModel4WS capContactModel = null;

            if (recordModel.applicantModel != null
                && recordModel.applicantModel.people != null
                && ValidationUtil.IsYes(recordModel.applicantModel.people.flag))
            {
                capContactModel = recordModel.applicantModel;
            }

            if (capContactModel == null 
                && recordModel.contactsGroup != null 
                && recordModel.contactsGroup.Length > 0)
            {
                foreach (var contactModel in recordModel.contactsGroup)
                {
                    if (contactModel.people != null && ValidationUtil.IsYes(contactModel.people.flag))
                    {
                        capContactModel = contactModel;
                        break;
                    }
                }
            }

            return capContactModel;
        }

        /// <summary>
        /// Clears the condition.
        /// </summary>
        /// <param name="urlParameters">The URL parameters.</param>
        public static void ClearSearchParameter(ExaminationParameter urlParameters)
        {
            urlParameters.ConditionCity = string.Empty;
            urlParameters.ConditionState = string.Empty;
            urlParameters.ConditionProviderNbr = string.Empty;
            urlParameters.ConditionStartTime = string.Empty;
            urlParameters.ConditionEndTime = string.Empty;
        }

        /// <summary>
        /// Validates the schedule date.
        /// </summary>
        /// <param name="startDate">The start date.</param>
        /// <param name="endDate">The end date.</param>
        /// <param name="module">The module.</param>
        /// <returns>The error message when validate schedule date failed.</returns>
        public static string ValidateScheduleDate(string startDate, string endDate, string module)
        {
            ITimeZoneBll providerBll = (ITimeZoneBll)ObjectFactory.GetObject(typeof(ITimeZoneBll));
            DateTime agencyTime = providerBll.GetAgencyCurrentDate(ConfigManager.AgencyCode);

            if (!string.IsNullOrEmpty(startDate) && !string.IsNullOrEmpty(endDate))
            {
                DateTime dtBegin = I18nDateTimeUtil.ParseFromUI(startDate);
                DateTime dtEnd = I18nDateTimeUtil.ParseFromUI(endDate).AddDays(1).AddSeconds(-1);

                if (dtBegin > dtEnd)
                {
                    return LabelUtil.GetTextByKey("aca_exam_schedule_msg_date_start_end", module);
                }
            }

            if (!string.IsNullOrEmpty(startDate))
            {
                DateTime dtBegin = I18nDateTimeUtil.ParseFromUI(startDate);

                if (dtBegin < agencyTime.Date)
                {
                    return LabelUtil.GetTextByKey("aca_exam_schedule_msg_date_start", module);
                }
            }
            else
            {
                return LabelUtil.GetTextByKey("aca_exam_schedule_msg_date_start", module);
            }

            return string.Empty;
        }

        /// <summary>
        /// Creates the data table.
        /// </summary>
        /// <returns>table of examination name</returns>
        private static DataTable CreatDataTable()
        {
            var dataTable = new DataTable();

            dataTable.Columns.Add(new DataColumn("ExaminationName", typeof(string)));
            dataTable.Columns.Add(new DataColumn("ExaminationID", typeof(string)));
            dataTable.Columns.Add(new DataColumn("Selected", typeof(bool)));
            dataTable.Columns.Add(new DataColumn("Enabled", typeof(bool)));

            return dataTable;
        }

        /// <summary>
        /// Creates the schedule data table.
        /// </summary>
        /// <returns>schedule list table</returns>
        private static DataTable CreatScheduleDataTable()
        {
            var dataTable = new DataTable();

            dataTable.Columns.Add(new DataColumn(ColumnConstant.RefExaminationScheduleDetail.ExaminationScheduleID.ToString(), typeof(string)));
            dataTable.Columns.Add(new DataColumn(ColumnConstant.RefExaminationScheduleDetail.Provider.ToString(), typeof(string)));
            dataTable.Columns.Add(new DataColumn(ColumnConstant.RefExaminationScheduleDetail.Fee.ToString(), typeof(string)));
            dataTable.Columns.Add(new DataColumn(ColumnConstant.RefExaminationScheduleDetail.Date.ToString(), typeof(DateTime)));
            dataTable.Columns.Add(new DataColumn(ColumnConstant.RefExaminationScheduleDetail.WeekyDay.ToString(), typeof(string)));
            dataTable.Columns.Add(new DataColumn(ColumnConstant.RefExaminationScheduleDetail.StartTime.ToString(), typeof(DateTime)));
            dataTable.Columns.Add(new DataColumn(ColumnConstant.RefExaminationScheduleDetail.EndTime.ToString(), typeof(DateTime)));
            dataTable.Columns.Add(new DataColumn(ColumnConstant.RefExaminationScheduleDetail.ExaminationSite.ToString(), typeof(string)));
            dataTable.Columns.Add(new DataColumn(ColumnConstant.RefExaminationScheduleDetail.HandicapAccessible.ToString(), typeof(string)));
            dataTable.Columns.Add(new DataColumn(ColumnConstant.RefExaminationScheduleDetail.AvailableSeats.ToString(), typeof(int)));
            dataTable.Columns.Add(new DataColumn(ColumnConstant.RefExaminationScheduleDetail.LocationId.ToString(), typeof(string)));
            dataTable.Columns.Add(new DataColumn(ColumnConstant.RefExaminationScheduleDetail.ScheduleId.ToString(), typeof(string)));
            dataTable.Columns.Add(new DataColumn(ColumnConstant.RefExaminationScheduleDetail.CalendarId.ToString(), typeof(string)));
            dataTable.Columns.Add(new DataColumn(ColumnConstant.RefExaminationScheduleDetail.ProviderNbr.ToString(), typeof(string)));
            dataTable.Columns.Add(new DataColumn(ColumnConstant.RefExaminationScheduleDetail.IsExternal.ToString(), typeof(string)));
            dataTable.Columns.Add(new DataColumn(ColumnConstant.RefExaminationScheduleDetail.AccessiblityDesc.ToString(), typeof(string)));
            dataTable.Columns.Add(new DataColumn(ColumnConstant.RefExaminationScheduleDetail.DrivingDesc.ToString(), typeof(string)));
            dataTable.Columns.Add(new DataColumn(ColumnConstant.RefExaminationScheduleDetail.ExamInstructions.ToString(), typeof(string)));
            dataTable.Columns.Add(new DataColumn(ColumnConstant.RefExaminationScheduleDetail.IsOnline.ToString(), typeof(string)));
            dataTable.Columns.Add(new DataColumn("Selected", typeof(bool)));
            dataTable.Columns.Add(new DataColumn("Enabled", typeof(bool)));

            return dataTable;
        }

        /// <summary>
        /// Creates the reason data table.
        /// </summary>
        /// <returns>The reason data table construction.</returns>
        private static DataTable CreatReasonDataTable()
        {
            var dataTable = new DataTable();

            dataTable.Columns.Add(new DataColumn("ReasonString", typeof(string)));
            dataTable.Columns.Add(new DataColumn("ID", typeof(string)));
            dataTable.Columns.Add(new DataColumn("Selected", typeof(bool)));
            dataTable.Columns.Add(new DataColumn("Enabled", typeof(bool)));

            return dataTable;
        }

        /// <summary>
        /// Gets the name of the current page.
        /// </summary>
        /// <param name="currentRequstPath">The current request path.</param>
        /// <returns> Examination Schedule Wizard Page Name </returns>
        private static ExaminationScheduleWizardPageName GetCurrentPageName(string currentRequstPath)
        {
            var result = ExaminationScheduleWizardPageName.Unknown;

            if (currentRequstPath.EndsWith(EXAM_SCHEDULE_PAGE_EXAMLIST, StringComparison.OrdinalIgnoreCase))
            {
                result = ExaminationScheduleWizardPageName.ExamList;
            }
            else if (currentRequstPath.EndsWith(EXAM_SCHEDULE_PAGE_EXAMSCHEDULELIST, StringComparison.OrdinalIgnoreCase))
            {
                result = ExaminationScheduleWizardPageName.ExamScheduleList;
            }
            else if (currentRequstPath.EndsWith(EXAM_SCHEDULE_PAGE_EXAMSCHEDULECONFIRM, StringComparison.OrdinalIgnoreCase))
            {
                result = ExaminationScheduleWizardPageName.ExamScheduleConfirm;
            }
            else if (currentRequstPath.EndsWith(EXAMINATION_URL_RESCHEDULE_REASON, StringComparison.OrdinalIgnoreCase))
            {
                result = ExaminationScheduleWizardPageName.ExamRescheduleReason;
            }

            return result;
        }

        /// <summary>
        /// Gets the relative URL.
        /// </summary>
        /// <param name="previousPage">The previous page.</param>
        /// <returns>schedule url string</returns>
        private static string GetRelativeURL(ExaminationScheduleWizardPageName previousPage)
        {
            string result = string.Empty;

            switch (previousPage)
            {
                case ExaminationScheduleWizardPageName.ExamList:
                    result = EXAM_SCHEDULE_PAGE_EXAMLIST;
                    break;
                case ExaminationScheduleWizardPageName.ExamScheduleList:
                    result = EXAM_SCHEDULE_PAGE_EXAMSCHEDULELIST;
                    break;
                case ExaminationScheduleWizardPageName.ExamScheduleConfirm:
                    result = EXAM_SCHEDULE_PAGE_EXAMSCHEDULECONFIRM;
                    break;
                case ExaminationScheduleWizardPageName.ExamRescheduleReason:
                    result = EXAMINATION_URL_RESCHEDULE_REASON;
                    break;
                default:
                    result = string.Empty;
                    break;
            }

            return result;
        }
    }
}