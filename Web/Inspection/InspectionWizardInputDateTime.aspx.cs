#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: InspectionWizardInputDateTime.aspx.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2010-2014
 *
 *  Description: Select the schedule or request datetime.
 *
 *  Notes:
 *      $Id: InspectionWizardInputDateTime.aspx.cs 170366 2010-09-08 05:34:25Z ACHIEVO\daly.zeng $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using System;
using System.Web.UI;

using Accela.ACA.BLL.Inspection;
using Accela.ACA.Common;
using Accela.ACA.Common.Common;
using Accela.ACA.Common.Log;
using Accela.ACA.Common.Util;
using Accela.ACA.Inspection;
using Accela.ACA.Web.Common;
using Accela.ACA.WSProxy;

namespace Accela.ACA.Web.Inspection
{
    /// <summary>
    /// This class provide the ability to select the schedule or request datetime.
    /// </summary>
    public partial class InspectionWizardInputDateTime : InspectionWizardBasePage
    {
        #region Fields

        /// <summary>
        /// The aca block unit for Day
        /// </summary>
        private const string ACA_BLOCK_UNIT_DAY = "Day";

        /// <summary>
        /// log4net Logger
        /// </summary>
        private static readonly log4net.ILog Logger = LogFactory.Instance.GetLogger(typeof(InspectionWizardInputDateTime));

        #endregion Fields

        #region Protected Methods

        /// <summary>
        /// Raises the page load event.
        /// </summary>
        /// <param name="sender">An object that contains the event sender.</param>
        /// <param name="e">A System.EventArgs object containing the event data.</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                SetPageTitleKey("aca_inspection_title_scheduleorrequestinspection");
                SetDialogMaxHeight("600");

                if (!Page.IsPostBack)
                {
                    if (!AppSession.IsAdmin)
                    {
                        MarkCurrentPageTrace(InspectionWizardPage.DateTime, false);
                    }

                    InitUI();

                    if (!AppSession.IsAdmin)
                    {
                        MarkCurrentPageTrace(InspectionWizardPage.DateTime, true);

                        // to make the back button show/hide.
                        bool isShowBack = IsShowBack(InspectionWizardPage.DateTime);

                        tdBack.Visible = isShowBack;
                        tdBackSpace.Visible = isShowBack;
                    }
                }
            }
            catch (ACAException ex)
            {
                Logger.Error(ex.Message, ex);
                MessageUtil.ShowMessage(Page, MessageType.Error, ex.Message);
            }
        }

        /// <summary>
        /// Raises the Continue button click event.
        /// </summary>
        /// <param name="sender">An object that contains the event sender.</param>
        /// <param name="e">A System.EventArgs object containing the event data.</param>
        protected void ContinueButton_Click(object sender, EventArgs e)
        {
            try
            {
                var timeOption = InspectionTimeOption.Unknow;
                Range<DateTime?> activityDateRange = GetActivityDate(out timeOption);
                DateTime? activityDate = activityDateRange.LowerBound;
                InspectionParameter inspectionParameter = InspectionWizardParameter;

                if (activityDate != null || inspectionParameter.ScheduleType == InspectionScheduleType.RequestOnlyPending || inspectionParameter.ScheduleType == InspectionScheduleType.RequestSameDayNextDay)
                {
                    inspectionParameter.ScheduledDateTime = activityDate;
                    inspectionParameter.EndScheduledDateTime = activityDateRange.UpperBound;
                    inspectionParameter.TimeOption = timeOption;

                    bool isBlockedWhenNoInspectorFound = StandardChoiceUtil.IsBlockedWhenNoInspectorFound(ConfigManager.AgencyCode);
                    AvailableTimeResultModel availableTimeResultModel = GetAvailableTimeResult(activityDate, timeOption, isBlockedWhenNoInspectorFound);

                    if (availableTimeResultModel != null && activityDate != null)
                    {
                        //show message that schedule date is no available in calendar.
                        if ((inspectionParameter.ScheduleType == InspectionScheduleType.ScheduleUsingCalendar || inspectionParameter.ScheduleType == InspectionScheduleType.Unknown) &&
                            availableTimeResultModel.scheduleDate != null && 
                            !calendar.IsAvailableDate(availableTimeResultModel.scheduleDate.Value))
                        {
                            MessageUtil.ShowMessageInPopup(Page, MessageType.Error, GetTextByKey("aca_inspection_msg_novalidate_datetime"));
                            return;
                        }

                        CalendarModel acaCalendarModel = GetACACalendarModel();
                        double calendarAttempts = Convert.ToDouble(acaCalendarModel.calendarAttempts);

                        // Show message that no available time found when exceed the count of trying find the available time.
                        if (availableTimeResultModel.flag != ACAConstant.INSPECTION_FLAG_SUCCESS)
                        {
                            if (calendarAttempts < 1)
                            {
                                MessageUtil.ShowMessageInPopup(Page, MessageType.Error, GetTextByKey("aca_inspection_msg_novalidate_datetime"));
                                return;
                            }

                            DateTime noAvailableTimeStart = activityDate.Value;
                            DateTime noAvailableTimeEnd = activityDate.Value.AddDays(calendarAttempts - 1);

                            string noAvailableTimeStartString = I18nDateTimeUtil.FormatToInspectionDateTimeStringForUI(noAvailableTimeStart);
                            string noAvailableTimeEndString = I18nDateTimeUtil.FormatToInspectionDateTimeStringForUI(noAvailableTimeEnd.Date.AddHours(23).AddMinutes(59));

                            MessageUtil.ShowMessageInPopup(Page, MessageType.Error, string.Format(GetTextByKey("aca_inspection_message_no_validate_datetime"), noAvailableTimeStartString, noAvailableTimeEndString));

                            return;
                        }

                        // show error message if the selected date cannot found enough inspection unit. 
                        if (!string.IsNullOrEmpty(availableTimeResultModel.startTime)
                            && !string.IsNullOrEmpty(availableTimeResultModel.startAMPM))
                        {
                            string startAvailableTimeString = string.Format(
                                                                        "{0} {1} {2}",
                                                                        I18nDateTimeUtil.FormatToDateStringForWebService(availableTimeResultModel.scheduleDate),
                                                                        availableTimeResultModel.startTime,
                                                                        availableTimeResultModel.startAMPM);

                            DateTime startAvailableTime = DateTime.MinValue;
                            I18nDateTimeUtil.TryParseFromWebService(startAvailableTimeString, out startAvailableTime);

                            // if ScheduleType is RequestSameDayNextDay ,then judge the date only, otherwise,should judge the startAvailableTime.
                            if ((inspectionParameter.ScheduleType == InspectionScheduleType.RequestSameDayNextDay && DateTime.Compare(activityDate.Value.Date, availableTimeResultModel.scheduleDate.Value.Date) < 0)
                                || (inspectionParameter.ScheduleType != InspectionScheduleType.RequestSameDayNextDay && !IsGivenDateTimeAvailable(activityDate, startAvailableTime, availableTimeResultModel.startAMPM, timeOption)))
                            {
                                if (calendarAttempts < 1)
                                {
                                    MessageUtil.ShowMessageInPopup(Page, MessageType.Error, GetTextByKey("aca_inspection_msg_novalidate_datetime"));
                                    return;
                                }

                                string stratAvailableTimeInfo = I18nDateTimeUtil.FormatToInspectionDateTimeStringForUI(startAvailableTime);

                                MessageUtil.ShowMessageInPopup(Page, MessageType.Error, string.Format(GetTextByKey("aca_inspection_message_validate_datetime"), stratAvailableTimeInfo));

                                return;
                            }
                        }
                    }

                    if (inspectionParameter.ScheduleType == InspectionScheduleType.RequestSameDayNextDay)
                    {
                        inspectionParameter.RequestDayOption = sameDayNextDay.GetRequestDayOption();
                    }

                    string url = string.Format("{0}?{1}", "InspectionWizardInputLocation.aspx", Request.QueryString);
                    url = InspectionParameterUtil.UpdateURLAndSaveParameters(url, inspectionParameter);

                    Response.Redirect(url);
                }
                else
                {
                    MessageUtil.ShowMessageInPopup(Page, MessageType.Error, "Have not selected the date time");
                }
            }
            catch (ACAException ex)
            {
                Logger.Error(ex.Message, ex);
                MessageUtil.ShowMessage(Page, MessageType.Error, ex.Message);
            }
        }

        /// <summary>
        /// Raises the Back button click event.
        /// </summary>
        /// <param name="sender">An object that contains the event sender.</param>
        /// <param name="e">A System.EventArgs object containing the event data.</param>
        protected void BackButton_Click(object sender, EventArgs e)
        {
            try
            {
                string previousURL = InspectionActionViewUtil.GetWizardPreviousURL(InspectionWizardParameter);

                if (!string.IsNullOrEmpty(previousURL))
                {
                    Response.Redirect(previousURL);
                }
            }
            catch (ACAException ex)
            {
                Logger.Error(ex.Message, ex);
                MessageUtil.ShowMessage(Page, MessageType.Error, ex.Message);
            }
        }

        /// <summary>
        /// Initializes the <see cref="T:System.Web.UI.HtmlTextWriter"/> object and calls on the child controls of the <see cref="T:System.Web.UI.Page"/> to render.
        /// </summary>
        /// <param name="writer">The <see cref="T:System.Web.UI.HtmlTextWriter"/> that receives the page content.</param>
        protected override void Render(HtmlTextWriter writer)
        {
            var timeOption = InspectionTimeOption.Unknow;
            DateTime? activityDate = GetActivityDate(out timeOption).LowerBound;
            InspectionParameter inspectionParameter = InspectionWizardParameter;

            if (!AppSession.IsAdmin && activityDate == null && inspectionParameter.ScheduleType != InspectionScheduleType.RequestOnlyPending && inspectionParameter.ScheduleType != InspectionScheduleType.RequestSameDayNextDay)
            {
                SetWizardButtonDisable(lnkContinue.ClientID, true);
            }

            base.Render(writer);
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Initialize the UI.
        /// </summary>
        private void InitUI()
        {
            if (!AppSession.IsAdmin)
            {
                lblInpectionType.Text = string.Format(GetTextByKey("aca_inspection_type_label"), InspectionWizardParameter.TypeText);

                inspectionReadyTime.Visible = false;
                sameDayNextDay.Visible = false;
                calendar.Visible = false;

                switch (InspectionWizardParameter.ScheduleType)
                {
                    case InspectionScheduleType.None:
                    case InspectionScheduleType.RequestOnlyPending:
                        if (IsReadyTimeEnabled())
                        {
                            inspectionReadyTime.Visible = true;
                            InitReadyTimeControl();
                        }

                        break;
                    case InspectionScheduleType.RequestSameDayNextDay:
                        sameDayNextDay.Visible = true;
                        InitSameDayNextDayControl();
                        break;
                    case InspectionScheduleType.ScheduleUsingCalendar:
                    case InspectionScheduleType.Unknown:
                    default:
                        calendar.Visible = true;
                        InitSchedulingCalendarControl();
                        break;
                }

                if (InspectionWizardParameter.ScheduleType == InspectionScheduleType.ScheduleUsingCalendar || InspectionWizardParameter.ScheduleType == InspectionScheduleType.Unknown)
                {
                    bool disabled = InspectionWizardParameter.ScheduledDateTime == null;
                    SetWizardButtonDisable(lnkContinue.ClientID, disabled);
                }

                // hide the back button if the previous is empty
                string previousURL = InspectionActionViewUtil.GetWizardPreviousURL(InspectionWizardParameter);

                if (string.IsNullOrEmpty(previousURL))
                {
                    tdBack.Visible = false;
                    tdBackSpace.Visible = false;
                }
            }
            else
            {
                inspectionReadyTime.Visible = true;
                sameDayNextDay.Visible = true;
                calendar.Visible = true;
            }
        }

        /// <summary>
        /// Initialize the ready time control.
        /// </summary>
        private void InitReadyTimeControl()
        {
            inspectionReadyTime.InspectionTypeID = InspectionWizardParameter.TypeID;
            inspectionReadyTime.LastSelectedReadyTime = InspectionWizardParameter.ScheduledDateTime;
        }

        /// <summary>
        /// Initialize the same day next day control.
        /// </summary>
        private void InitSameDayNextDayControl()
        {
            var recordModel = GetCapModel();
            var recordIDModel = recordModel == null ? null : TempModelConvert.Trim4WSOfCapIDModel(recordModel.capID);
            sameDayNextDay.LastRequestDayOption = InspectionWizardParameter.RequestDayOption;
            sameDayNextDay.RecordIDModel = recordIDModel;
            sameDayNextDay.Action = InspectionWizardParameter.Action;
            sameDayNextDay.InAdvance = InspectionWizardParameter.InAdvance;
            sameDayNextDay.InspectionID = InspectionWizardParameter.ID;
            sameDayNextDay.Required = InspectionWizardParameter.Required;
            sameDayNextDay.ReadyTimeEnabled = InspectionWizardParameter.ReadyTimeEnabled;
            sameDayNextDay.ScheduleType = InspectionWizardParameter.ScheduleType;
            sameDayNextDay.InspectionType = InspectionWizardParameter.Type;
            sameDayNextDay.InspectionTypeID = InspectionWizardParameter.TypeID;
            sameDayNextDay.InspectionGroup = InspectionWizardParameter.Group;
            sameDayNextDay.InspectionTypeUnits = InspectionWizardParameter.Units != null ? InspectionWizardParameter.Units.Value : 0;
        }

        /// <summary>
        /// Initialize the scheduling calendar control.
        /// </summary>
        private void InitSchedulingCalendarControl()
        {
            var recordModel = GetCapModel();
            var recordIDModel = recordModel == null ? null : TempModelConvert.Trim4WSOfCapIDModel(recordModel.capID);
            calendar.RecordIDModel = recordIDModel;
            calendar.Action = InspectionWizardParameter.Action;
            calendar.InAdvance = InspectionWizardParameter.InAdvance;
            calendar.InspectionID = InspectionWizardParameter.ID;
            calendar.Required = InspectionWizardParameter.Required;
            calendar.InspectionType = InspectionWizardParameter.Type;
            calendar.InspectionTypeID = InspectionWizardParameter.TypeID;
            calendar.InspectionGroup = InspectionWizardParameter.Group;
            calendar.LastSelectedDateTime = InspectionWizardParameter.ScheduledDateTime;
            calendar.LastSelectedTimeOption = InspectionWizardParameter.TimeOption;
            calendar.ReadyTimeEnabled = InspectionWizardParameter.ReadyTimeEnabled;
            calendar.ScheduleType = InspectionWizardParameter.ScheduleType;
            calendar.CalendarDateChanged = "calendarDateChanged";
            calendar.CalendarTimeSelected = "calendarTimeSelected";
        }

        /// <summary>
        /// Determines whether [is ready time enabled].
        /// </summary>
        /// <returns>
        /// <c>true</c> if [is ready time enabled]; otherwise, <c>false</c>.
        /// </returns>
        private bool IsReadyTimeEnabled()
        {
            bool result = false;
            var inspectionParameter = InspectionWizardParameter;
            InspectionScheduleType scheduleType = inspectionParameter.ScheduleType;
            bool isRequestOnlyPending = scheduleType == InspectionScheduleType.RequestOnlyPending;
            bool isReadyTimeEnabled = inspectionParameter.ReadyTimeEnabled != null ? inspectionParameter.ReadyTimeEnabled.Value : false;
            result = isReadyTimeEnabled && isRequestOnlyPending;

            return result;
        }

        /// <summary>
        /// Get the activity date.
        /// </summary>
        /// <param name="timeOption">The time option.</param>
        /// <returns>Return the activity date.</returns>
        private Range<DateTime?> GetActivityDate(out InspectionTimeOption timeOption)
        {
            Range<DateTime?> result = new Range<DateTime?>();
            timeOption = InspectionTimeOption.Unknow;

            IInspectionBll inspectionBll = (IInspectionBll)ObjectFactory.GetObject(typeof(IInspectionBll));

            if (inspectionBll != null)
            {
                switch (InspectionWizardParameter.ScheduleType)
                {
                    case InspectionScheduleType.RequestOnlyPending:
                        timeOption = InspectionTimeOption.SpecificTime;
                        result.LowerBound = inspectionReadyTime.Visible ? inspectionReadyTime.GetReadyDateTime() : null;
                        break;
                    case InspectionScheduleType.RequestSameDayNextDay:
                        timeOption = InspectionTimeOption.AllDay;
                        result.LowerBound = sameDayNextDay.GetSelectedDate();
                        break;
                    case InspectionScheduleType.ScheduleUsingCalendar:
                    case InspectionScheduleType.Unknown:
                    default:
                        timeOption = calendar.GetSelectedTimeOption();
                        result = calendar.GetSelectedDateTime();
                        break;
                }
            }

            return result;
        }

        /// <summary>
        /// Validate inspection date time and get the available time result
        /// </summary>
        /// <param name="selectedDate">the selected date</param>
        /// <param name="timeOption">the time option</param>
        /// <param name="isBlockedWhenNoInspectorFound">Whether block or continue when no inspector was found.</param>
        /// <returns>the available result</returns>
        private AvailableTimeResultModel GetAvailableTimeResult(DateTime? selectedDate, InspectionTimeOption timeOption, bool isBlockedWhenNoInspectorFound = true)
        {
            AvailableTimeResultModel availableTimeResultModel = null;

            if (selectedDate != null)
            {
                switch (InspectionWizardParameter.ScheduleType)
                {
                    case InspectionScheduleType.None:
                    case InspectionScheduleType.RequestOnlyPending:
                        break;
                    case InspectionScheduleType.RequestSameDayNextDay:
                        availableTimeResultModel = sameDayNextDay.GetAvailableTimeResult(selectedDate.Value, isBlockedWhenNoInspectorFound);
                        break;
                    case InspectionScheduleType.ScheduleUsingCalendar:
                    case InspectionScheduleType.Unknown:
                    default:
                            availableTimeResultModel = calendar.GetAvailableTimeResult(selectedDate.Value, timeOption, isBlockedWhenNoInspectorFound);
                        break;
                }
            }

            return availableTimeResultModel;
        }

        /// <summary>
        /// Gets the ACA calendar model.
        /// </summary>
        /// <returns>the ACA calendar model.</returns>
        private CalendarModel GetACACalendarModel()
        {
            CalendarModel result = null;

            if (!string.IsNullOrEmpty(InspectionWizardParameter.TypeID))
            {
                ICalendarBll calendarBll = ObjectFactory.GetObject<ICalendarBll>();
                result = calendarBll.GetACACalendarByInspType(InspectionWizardParameter.AgencyCode, InspectionWizardParameter.TypeID);
            }

            return result;
        }

        /// <summary>
        /// 1. the start time is not available if return available time is before start time.
        /// 2. the start time is not available if start time is AM or PM and return available time is not same as start time.
        /// </summary>
        /// <param name="activityDate">The activity date.</param>
        /// <param name="startAvailableTime4Compare">The start available time for compare.</param>
        /// <param name="availableStartAMPM">The available start AM/PM.</param>
        /// <param name="timeOption">The inspection time option.</param>
        /// <returns>Whether the given date time available.</returns>
        private bool IsGivenDateTimeAvailable(DateTime? activityDate, DateTime startAvailableTime4Compare, string availableStartAMPM, InspectionTimeOption timeOption)
        {
            if (timeOption == InspectionTimeOption.AllDay)
            {
                if (DateTime.Compare(activityDate.Value.Date, startAvailableTime4Compare.Date) < 0)
                {
                    return false;
                }
                
                return true;
            }

            if (timeOption != InspectionTimeOption.AM && timeOption != InspectionTimeOption.PM && DateTime.Compare(activityDate.Value, startAvailableTime4Compare) < 0)
            {
                return false;
            }

            if (!string.IsNullOrEmpty(availableStartAMPM) && (timeOption == InspectionTimeOption.AM || timeOption == InspectionTimeOption.PM) 
                && !availableStartAMPM.Equals(timeOption.ToString(), StringComparison.OrdinalIgnoreCase))
            {
                return false;
            }

            return true;
        }

        #endregion Private Methods
    }
}
