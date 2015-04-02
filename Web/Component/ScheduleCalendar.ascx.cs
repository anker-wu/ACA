#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: ScheduleCalendar.ascx.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2007-2014
 *
 *  Description: Calendar control
 *
 *  Notes:
 *      $Id: ScheduleCalendar.ascx.cs 277971 2014-08-25 06:10:06Z ACHIEVO\james.shi $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;

using Accela.ACA.BLL.Common;
using Accela.ACA.BLL.Inspection;
using Accela.ACA.Common;
using Accela.ACA.Common.Common;
using Accela.ACA.Common.Log;
using Accela.ACA.Common.Util;
using Accela.ACA.Inspection;
using Accela.ACA.Web.Common;
using Accela.ACA.Web.Inspection;
using Accela.ACA.WSProxy;
using Accela.Web.Controls;

namespace Accela.ACA.Web.Component
{
    /// <summary>
    /// This class provide the ability to operation ScheduleCalendar.
    /// </summary>
    public partial class ScheduleCalendar : BaseUserControl
    {
        #region Fields

        /// <summary>
        /// AM constant string
        /// </summary>
        private const string AM = "am";

        /// <summary>
        /// Range type: All Day
        /// </summary>
        private const string TYPE_ALL_DAY = "ALL_DAY";

        /// <summary>
        /// Range type: AM/PM
        /// </summary>
        private const string TYPE_AM_PM = "AM_OR_PM";

        /// <summary>
        /// Range type: specific time range
        /// </summary>
        private const string TYPE_START_END_TIME = "START_END_TIME";

        /// <summary>
        /// Log4net Logger
        /// </summary>
        private static readonly log4net.ILog Logger = LogFactory.Instance.GetLogger(typeof(ScheduleCalendar));

        #endregion Fields

        #region Properties

        /// <summary>
        /// Gets or sets the record ID model.
        /// </summary>
        /// <value>The record ID model.</value>
        public CapIDModel RecordIDModel
        {
            get
            {
                return ViewState["RecordIDModel"] as CapIDModel;
            }

            set
            {
                ViewState["RecordIDModel"] = value;
            }
        }

        /// <summary>
        /// Gets or sets the action
        /// </summary>
        public InspectionAction Action
        {
            get
            {
                return (InspectionAction)ViewState["Action"];
            }

            set
            {
                ViewState["Action"] = value;
            }
        }

        /// <summary>
        /// Gets or sets the inAdvance
        /// </summary>
        public bool? InAdvance
        {
            get
            {
                return (bool?)ViewState["InAdvance"];
            }

            set
            {
                ViewState["InAdvance"] = value;
            }
        }

        /// <summary>
        /// Gets or sets the inspection id
        /// </summary>
        public string InspectionID
        {
            get
            {
                return (string)ViewState["InspectionID"];
            }

            set
            {
                ViewState["InspectionID"] = value;
            }
        }

        /// <summary>
        /// Gets or sets the required
        /// </summary>
        public bool? Required
        {
            get
            {
                return (bool?)ViewState["Required"];
            }

            set
            {
                ViewState["Required"] = value;
            }
        }

        /// <summary>
        /// Gets or sets the ReadyTimeEnabled
        /// </summary>
        public bool? ReadyTimeEnabled
        {
            get
            {
                return (bool?)ViewState["ReadyTimeEnabled"];
            }

            set
            {
                ViewState["ReadyTimeEnabled"] = value;
            }
        }

        /// <summary>
        /// Gets or sets the ScheduleType
        /// </summary>
        public InspectionScheduleType ScheduleType
        {
            get
            {
                return (InspectionScheduleType)ViewState["ScheduleType"];
            }

            set
            {
                ViewState["ScheduleType"] = value;
            }
        }

        /// <summary>
        /// Gets or sets the type of the inspection.
        /// </summary>
        /// <value>The type of the inspection.</value>
        public string InspectionType
        {
            get
            {
                return (string)ViewState["InspectionType"];
            }

            set
            {
                ViewState["InspectionType"] = value;
            }
        }

        /// <summary>
        /// Gets or sets the inspection type ID.
        /// </summary>
        /// <value>The inspection type ID.</value>
        public string InspectionTypeID
        {
            get
            {
                return (string)ViewState["InspectionTypeID"];
            }

            set
            {
                ViewState["InspectionTypeID"] = value;
            }
        }

        /// <summary>
        /// Gets or sets the inspection group.
        /// </summary>
        /// <value>The inspection group.</value>
        public string InspectionGroup
        {
            get
            {
                return (string)ViewState["InspectionGroup"];
            }

            set
            {
                ViewState["InspectionGroup"] = value;
            }
        }

        /// <summary>
        /// Gets or sets the last selected date time.
        /// no need to save in viewState as it is used to initialization.
        /// </summary>
        /// <value>The last selected date time.</value>
        public DateTime? LastSelectedDateTime
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the last selected time option.
        /// </summary>
        /// <value>The last selected time option.</value>
        public InspectionTimeOption LastSelectedTimeOption
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the client script function name for calendar date changed
        /// </summary>
        public string CalendarDateChanged
        {
            get
            {
                return ViewState["CalendarDateChanged"] == null ? string.Empty : ViewState["CalendarDateChanged"].ToString();
            }

            set
            {
                ViewState["CalendarDateChanged"] = value;
            }
        }

        /// <summary>
        /// Gets or sets the client script function name for calendar time period selection
        /// </summary>
        public string CalendarTimeSelected
        {
            get
            {
                return ViewState["CalendarTimeSelected"] == null ? string.Empty : ViewState["CalendarTimeSelected"].ToString();
            }

            set
            {
                ViewState["CalendarTimeSelected"] = value;
            }
        }

        /// <summary>
        /// Gets or sets the working days and times.
        /// </summary>
        /// <value>The working days and times.</value>
        private DateTimeRangePageModel[] WorkingDaysAndTimes
        {
            get
            {
                return ViewState["WorkingDaysAndTimes"] as DateTimeRangePageModel[];
            }

            set
            {
                ViewState["WorkingDaysAndTimes"] = value;
            }
        }

        /// <summary>
        /// Gets or sets the selected working days and time.
        /// </summary>
        /// <value>The selected working days and time.</value>
        private DateTimeRangePageModel SelectedWorkingDaysAndTime
        {
            get
            {
                return ViewState["SelectedWorkingDaysAndTime"] as DateTimeRangePageModel;
            }

            set
            {
                ViewState["SelectedWorkingDaysAndTime"] = value;
            }
        }

        /// <summary>
        /// Gets the current agency.
        /// </summary>
        /// <value>The current agency.</value>
        private string CurrentAgency
        {
            get
            {
                if (ViewState["CurrentAgency"] == null)
                {
                    ViewState["CurrentAgency"] = CapUtil.GetAgencyCode(ModuleName);
                }

                return ViewState["CurrentAgency"] as string;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this instance has enough units for the selected date.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance has enough units; otherwise, <c>false</c>.
        /// </value>
        private bool HasEnoughUnits
        {
            get
            {
                return ViewState["HasEnoughUnits"] == null ? false : Convert.ToBoolean(ViewState["HasEnoughUnits"]);
            }

            set
            {
                ViewState["HasEnoughUnits"] = value;
            }
        }

        /// <summary>
        /// Gets or sets current Selected date by user
        /// </summary>
        private DateTime? CurrentSelectedDate
        {
            get
            {
                return ViewState["SelectDate"] == null ? null : (DateTime?)ViewState["SelectDate"];
            }

            set
            {
                ViewState["SelectDate"] = value;
            }
        }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Gets the selected date time.
        /// </summary>
        /// <returns>The selected date time.</returns>
        public Range<DateTime?> GetSelectedDateTime()
        {
            Range<DateTime?> result = new Range<DateTime?>();

            AccelaRadioButton rdoItem = GetSelectedEventItemControl();
            HiddenField hfStartTime = rdoItem != null && rdoItem.Enabled ? (HiddenField)rdoItem.FindControl(rdoItem.ID + "_DateValue") : null;
            HiddenField hfEndTime = rdoItem != null && rdoItem.Enabled ? (HiddenField)rdoItem.FindControl(rdoItem.ID + "_EndDateValue") : null;

            result.LowerBound = hfStartTime != null ? I18nDateTimeUtil.ParseFromWebService(hfStartTime.Value) : (DateTime?)null;
            result.UpperBound = hfEndTime != null ? I18nDateTimeUtil.ParseFromWebService(hfEndTime.Value) : (DateTime?)null;

            return result;
        }

        /// <summary>
        /// Validate inspection date time and get the available time result
        /// </summary>
        /// <param name="selectedDate">The selected date</param>
        /// <param name="timeOption">The time option</param>
        /// <param name="isBlockedWhenNoInspectorFound">IsBlockedWhenNoInspectorFound Flag</param>
        /// <returns>get the available result.</returns>
        public AvailableTimeResultModel GetAvailableTimeResult(DateTime selectedDate, InspectionTimeOption timeOption, bool isBlockedWhenNoInspectorFound = true)
        {
            InspectionParameter inspectionParameter = new InspectionParameter();
            inspectionParameter.AgencyCode = CurrentAgency;
            inspectionParameter.Action = Action;
            inspectionParameter.Type = InspectionType;
            inspectionParameter.InAdvance = InAdvance;
            inspectionParameter.ID = InspectionID;
            inspectionParameter.TypeID = InspectionTypeID;
            inspectionParameter.Required = Required;
            inspectionParameter.ScheduleType = ScheduleType;
            inspectionParameter.ReadyTimeEnabled = ReadyTimeEnabled;
            inspectionParameter.TimeOption = timeOption;

            return InspectionViewUtil.GetAvailableTimeResult(selectedDate, RecordIDModel, inspectionParameter, isBlockedWhenNoInspectorFound);
        }

        /// <summary>
        /// Gets the selected time string.
        /// </summary>
        /// <returns>the selected time string.</returns>
        public InspectionTimeOption GetSelectedTimeOption()
        {
            var result = InspectionTimeOption.Unknow;
            AccelaRadioButton rdoItem = GetSelectedEventItemControl();
            HiddenField hiddenField = rdoItem != null && rdoItem.Enabled ? (HiddenField)rdoItem.FindControl(rdoItem.ID + "_TimeOption") : null;
            string value = hiddenField != null && !string.IsNullOrEmpty(hiddenField.Value) ? hiddenField.Value : string.Empty;
            result = EnumUtil<InspectionTimeOption>.Parse(value, InspectionTimeOption.Unknow);
            return result;
        }

        /// <summary>
        /// Get schedule time whether for working days and times
        /// </summary>
        /// <param name="date">The schedule date.</param>
        /// <returns>Whether is available date</returns>
        public bool IsAvailableDate(DateTime date)
        {
            if (WorkingDaysAndTimes != null)
            {
                foreach (DateTimeRangePageModel model in WorkingDaysAndTimes)
                {
                    if (model.date != null && model.date.Value.Date.CompareTo(date.Date) == 0)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        /// <summary>
        /// Raises the page load event
        /// </summary>
        /// <param name="sender">An object that contains the event sender.</param>
        /// <param name="e">A System.EventArgs object containing the event data.</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (this.Visible)
            {
                try
                {
                    if (!Page.IsPostBack)
                    {
                        var firstVisibleDate = GetInitialFirstVisibleDate();
                        RefreshCalendar(firstVisibleDate);
                        RestoreSelection();
                    }
                    else
                    {
                        // need re-show time periods for postback action.
                        if (CurrentSelectedDate != null)
                        {
                            ShowTimePeriods(CurrentSelectedDate.Value);
                        }
                    }

                    FirstDayOfWeek firstDayOfWeek = (FirstDayOfWeek)Enum.Parse(typeof(FirstDayOfWeek), I18nCultureUtil.UserPreferredCultureInfo.DateTimeFormat.FirstDayOfWeek.ToString());
                    calendar1.FirstDayOfWeek = firstDayOfWeek;
                    calendar2.FirstDayOfWeek = firstDayOfWeek;
                    calendar3.FirstDayOfWeek = firstDayOfWeek;
                }
                catch (ACAException ex)
                {
                    Logger.Error(ex.Message, ex);
                    MessageUtil.ShowMessage(Page, MessageType.Error, ex.Message);
                }
            }
        }

        /// <summary>
        /// Event handler for calendar selection changed
        /// </summary>
        /// <param name="sender">Object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void Calendar_SelectionChanged(object sender, EventArgs e)
        {
            try
            {
                Calendar control = (Calendar)sender;

                //clear other calendar's select date
                calendar1.SelectedDate = control != calendar1 ? DateTime.MinValue : calendar1.SelectedDate;
                calendar2.SelectedDate = control != calendar2 ? DateTime.MinValue : calendar2.SelectedDate;
                calendar3.SelectedDate = control != calendar3 ? DateTime.MinValue : calendar3.SelectedDate;

                if (CurrentSelectedDate == null || control.SelectedDate.CompareTo(CurrentSelectedDate.Value) != 0)
                {
                    CurrentSelectedDate = control.SelectedDate;
                    HasEnoughUnits = true;
                    SelectedWorkingDaysAndTime = WorkingDaysAndTimes == null ? null : Array.Find<DateTimeRangePageModel>(WorkingDaysAndTimes, p => p.date == control.SelectedDate);
                    ShowTimePeriods(control.SelectedDate);

                    string jsScript = string.Format("$('#{0} img').attr('alt', '{1}');", hlUseToFocus.ClientID, lblAvaliableTimes.Text);
                    ScriptManager.RegisterStartupScript(upScheduleCalendar, GetType(), "ChangeLnkAlt", jsScript, true);

                    //Update title of the selected date. 
                    string selectedDateAlt = string.Format(GetTextByKey("aca_calendar_label_selecteddate"), I18nDateTimeUtil.FormatToLongDateStringForUI(control.SelectedDate));
                    string updateSelectedDateTitle = string.Format("$('#{0} [datestatus=\"selected\"] a').attr('title', '{1}');", control.ClientID, selectedDateAlt);
                    ScriptManager.RegisterStartupScript(control, GetType(), "UpdateSelectDateTitle", updateSelectedDateTitle, true);

                    Page.FocusElement(hlUseToFocus.ClientID);
                }
            }
            catch (ACAException ex)
            {
                Logger.Error(ex.Message, ex);
                MessageUtil.ShowMessage(Page, MessageType.Error, ex.Message);
            }
        }

        /// <summary>
        /// Calendar OnDayRender
        /// </summary>
        /// <param name="sender">Object sender</param>
        /// <param name="e">DayRenderEventArgs e</param>
        protected void Calendar_OnDayRender(object sender, DayRenderEventArgs e)
        {
            try
            {
                if (e.Day.IsOtherMonth)
                {
                    e.Cell.Text = " ";
                    e.Cell.BackColor = Color.White;
                }
                else
                {
                    var workingDayAndTimes = WorkingDaysAndTimes == null ? null : Array.Find<DateTimeRangePageModel>(WorkingDaysAndTimes, p => p.date == e.Day.Date);

                    if (!HasAnyActiveTime(workingDayAndTimes))
                    {
                        e.Cell.CssClass = "CalendarDayInactive ACA_LinkButton";
                        e.Day.IsSelectable = false;
                        e.Cell.ToolTip = GetTextByKey("aca_calendar_inactive_day");
                    }
                    else if (CurrentSelectedDate != null && e.Day.Date == CurrentSelectedDate.Value)
                    {
                        e.Cell.ForeColor = Color.DarkBlue;
                        e.Cell.BackColor = Color.Yellow;
                        e.Cell.Controls.AddAt(0, new LiteralControl("<b>"));
                        e.Cell.Attributes.Add("datestatus", "selected");
                    }
                    else
                    {
                        e.Cell.ForeColor = Color.DarkBlue;
                        e.Cell.BackColor = Color.White;

                        if (!string.IsNullOrEmpty(CalendarDateChanged))
                        {
                            e.Cell.Attributes.Add("onclick", string.Format("{0}(this);", CalendarDateChanged));
                        }
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
        /// Click the next month.
        /// </summary>
        /// <param name="sender">Object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void LnkNextMonth_Click(object sender, EventArgs e)
        {
            try
            {
                var firstVisibleDate = calendar1.VisibleDate.AddMonths(1);
                RefreshCalendar(firstVisibleDate);
            }
            catch (ACAException ex)
            {
                Logger.Error(ex.Message, ex);
                MessageUtil.ShowMessage(Page, MessageType.Error, ex.Message);
            }
        }

        /// <summary>
        /// Click the previous month
        /// </summary>
        /// <param name="sender">Object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void LnkPreMonth_Click(object sender, EventArgs e)
        {
            try
            {
                var firstVisibleDate = calendar1.VisibleDate.AddMonths(-1);
                RefreshCalendar(firstVisibleDate);
            }
            catch (ACAException ex)
            {
                Logger.Error(ex.Message, ex);
                MessageUtil.ShowMessage(Page, MessageType.Error, ex.Message);
            }
        }

        /// <summary>
        /// Gets the initial first visible date.
        /// </summary>
        /// <returns>the initial first visible date.</returns>
        private DateTime GetInitialFirstVisibleDate()
        {
            var timeBll = ObjectFactory.GetObject<ITimeZoneBll>();
            var firstVisibleDate = timeBll.GetAgencyCurrentDate(CurrentAgency);

            // set visible date back to last selection if selection is invisible.
            if (LastSelectedDateTime != null && (LastSelectedDateTime.Value < firstVisibleDate || SubtractDateMonth(LastSelectedDateTime.Value, firstVisibleDate) >= 3))
            {
                firstVisibleDate = LastSelectedDateTime.Value.Date;
            }

            return firstVisibleDate;
        }

        /// <summary>
        /// Refresh the UI of calendar group
        /// </summary>
        /// <param name="visibleDate">the current date</param>
        private void RefreshCalendar(DateTime visibleDate)
        {
            calendar1.ToolTip = GetTextByKey("aca_calendar_default_toolTip");
            calendar2.ToolTip = GetTextByKey("aca_calendar_default_toolTip");
            calendar3.ToolTip = GetTextByKey("aca_calendar_default_toolTip");

            string summary = GetTextByKey("aca_summary_control_calendar");
            calendar1.Attributes.Add("summary", summary);
            calendar2.Attributes.Add("summary", summary);
            calendar3.Attributes.Add("summary", summary);

            calendar1.VisibleDate = visibleDate;
            calendar2.VisibleDate = visibleDate.AddMonths(1);
            calendar3.VisibleDate = visibleDate.AddMonths(2);

            calendar1.Caption = "<span class='font12px'>" + calendar1.VisibleDate.ToString(ACAConstant.ABBREVIATED_MONTH_YEAR_FORMAT) + "</span>";
            calendar2.Caption = "<span class='font12px'>" + calendar2.VisibleDate.ToString(ACAConstant.ABBREVIATED_MONTH_YEAR_FORMAT) + "</span>";
            calendar3.Caption = "<span class='font12px'>" + calendar3.VisibleDate.ToString(ACAConstant.ABBREVIATED_MONTH_YEAR_FORMAT) + "</span>";

            SetWorkingDaysAndTimes(visibleDate);
        }

        /// <summary>
        /// Sets the working days and times.
        /// </summary>
        /// <param name="visibleDate">The visible date.</param>
        private void SetWorkingDaysAndTimes(DateTime visibleDate)
        {
            if (!string.IsNullOrEmpty(InspectionTypeID))
            {
                //the first day of first calendar
                DateTime startDate = new DateTime(visibleDate.Year, visibleDate.Month, 1);

                //the last day of last calendar 
                DateTime endDate = startDate.AddMonths(3).AddDays(-1);

                var inspectionTypeModel = new InspectionTypeModel();
                inspectionTypeModel.sequenceNumber = long.Parse(InspectionTypeID);
                inspectionTypeModel.groupCode = InspectionGroup;
                inspectionTypeModel.type = InspectionType;

                var calendarBll = ObjectFactory.GetObject<ICalendarBll>();

                // Gets current available working days
                DateTimeRangePageModel[] currentDateTimes = calendarBll.GetNextWorkingDaysAndTimes(CurrentAgency, startDate, endDate, RecordIDModel, inspectionTypeModel);
                List<DateTimeRangePageModel> newWorkingDayList = new List<DateTimeRangePageModel>();

                // Save the existing available working days
                if (WorkingDaysAndTimes != null && WorkingDaysAndTimes.Length > 0)
                {
                    newWorkingDayList.AddRange(WorkingDaysAndTimes);
                }

                // Add current new available working days
                if (currentDateTimes != null && currentDateTimes.Length > 0)
                {
                    newWorkingDayList.AddRange(currentDateTimes);
                }

                WorkingDaysAndTimes = newWorkingDayList.ToArray();
            }
        }

        /// <summary>
        /// Restores the selection for calendar and time period.
        /// </summary>
        private void RestoreSelection()
        {
            if (LastSelectedDateTime != null)
            {
                var calendars = new Calendar[] { calendar1, calendar2, calendar3 };
                var calendar = calendars.FirstOrDefault(p => SubtractDateMonth(p.VisibleDate, LastSelectedDateTime.Value) == 0);

                if (calendar != null)
                {
                    calendar.SelectedDate = LastSelectedDateTime.Value.Date;
                    Calendar_SelectionChanged(calendar, null);
                }
            }
        }

        /// <summary>
        /// Show time periods
        /// </summary>
        /// <param name="selectedDate">The schedule date</param>
        private void ShowTimePeriods(DateTime selectedDate)
        {
            bool isBlockedWhenNoInspectorFound = StandardChoiceUtil.IsBlockedWhenNoInspectorFound(ConfigManager.AgencyCode);
            AvailableTimeResultModel availableTimeResultModel = GetAvailableTimeResult(selectedDate, InspectionTimeOption.Unknow, isBlockedWhenNoInspectorFound);

            if (availableTimeResultModel != null && availableTimeResultModel.flag == ACAConstant.INSPECTION_FLAG_SUCCESS && selectedDate == availableTimeResultModel.scheduleDate)
            {
                HasEnoughUnits = true;
            }
            else
            {
                HasEnoughUnits = false;
            }

            divDayEventItems.Visible = false;
            divMorningEventItems.Visible = false;
            divAfternoonEventItems.Visible = false;
            divBottomGray.Visible = false;

            lblAvaliableTimes.Text = string.Format(GetTextByKey("aca_calendar_avaliable_times"), I18nDateTimeUtil.FormatToLongDateStringForUI(selectedDate));

            if (SelectedWorkingDaysAndTime != null)
            {
                var isNormalTimePeriods = IsNormalTimePeriods(SelectedWorkingDaysAndTime.dateTimeRangeType);

                if (isNormalTimePeriods)
                {
                    ShowNormalTimePeriods(SelectedWorkingDaysAndTime);
                }
                else
                {
                    ShowAllDayOrAmPmOrExceptionTimePeriods(SelectedWorkingDaysAndTime);
                }

                if (divDayEventItems.Visible || divMorningEventItems.Visible || divAfternoonEventItems.Visible)
                {
                    divBottomGray.Visible = true;
                }
            }
        }

        /// <summary>
        /// Shows all day or am pm or exception time periods.
        /// </summary>
        /// <param name="dateAndTimesModel">The date and times model.</param>
        private void ShowAllDayOrAmPmOrExceptionTimePeriods(DateTimeRangePageModel dateAndTimesModel)
        {
            if (dateAndTimesModel != null && dateAndTimesModel.times != null && dateAndTimesModel.date != null)
            {
                var hideTimePeriod = dateAndTimesModel.hideInspTimesInACA;
                var timePeriods = from t in dateAndTimesModel.times
                                  where t != null
                                  && t.startDate != null
                                  && t.endDate != null
                                  && dateAndTimesModel.date.Value.Date == t.startDate.Value.Date
                                  && dateAndTimesModel.date.Value.Date == t.endDate.Value.Date
                                  orderby t.startDate.Value ascending
                                  select t;

                divDayEventItems.Visible = true;
                phDayEventItems.Controls.Clear();
                phDayEventItems.Controls.Add(new LiteralControl("<div class=\"ACA_SmLabel ACA_SmLabel_FontSize ACA_LeftPadding\">"));

                foreach (var t in timePeriods)
                {
                    var isRadioButtonEnabled = HasEnoughUnits;
                    var radioButton = CreateRadioButton(dateAndTimesModel.dateTimeRangeType, t.startDate.Value, t.endDate.Value, t.amOrPM, hideTimePeriod, isRadioButtonEnabled);
                    phDayEventItems.Controls.Add(radioButton);
                    phDayEventItems.Controls.Add(new LiteralControl("<br />"));
                }

                phDayEventItems.Controls.Add(new LiteralControl("</div>"));
            }
        }

        /// <summary>
        /// Shows the normal time periods.
        /// </summary>
        /// <param name="dateAndTimesModel">The date and times model.</param>
        private void ShowNormalTimePeriods(DateTimeRangePageModel dateAndTimesModel)
        {
            if (dateAndTimesModel != null && dateAndTimesModel.times != null && dateAndTimesModel.date != null)
            {
                var hideTimePeriod = dateAndTimesModel.hideInspTimesInACA;
                var timePeriodGroups = from t in dateAndTimesModel.times
                                       where t != null
                                       && t.startDate != null
                                       && t.endDate != null
                                       && dateAndTimesModel.date.Value.Date == t.startDate.Value.Date
                                       && dateAndTimesModel.date.Value.Date == t.endDate.Value.Date
                                       let isAM = t.startDate.Value.Hour < 12
                                       orderby t.startDate.Value ascending
                                       group t by isAM into g
                                       let minStartDate = g.Min(p => p.startDate.Value)
                                       let maxStartDate = g.Max(p => p.endDate.Value)
                                       select new { minStartDate, maxStartDate, isAM = g.Key, timePeriods = g };

                foreach (var g in timePeriodGroups)
                {
                    var divEventItems = g.isAM ? divMorningEventItems : divAfternoonEventItems;
                    divEventItems.Visible = true;

                    var amOrPmText = g.isAM ? GetTextByKey("aca_calendar_morning") : GetTextByKey("aca_calendar_afternoon");
                    var timePeriodText = string.Format("{0} ({1} - {2})", amOrPmText, I18nDateTimeUtil.FormatToTimeStringForUI(g.minStartDate, true), I18nDateTimeUtil.FormatToTimeStringForUI(g.maxStartDate, true));
                    var text = hideTimePeriod ? amOrPmText : timePeriodText;
                    var labelSection = g.isAM ? lblMorningTime : lblAfternoonTime;
                    labelSection.Text = text;

                    var phEventItems = g.isAM ? phMorningEventItems : phAfternoonEventItems;
                    phEventItems.Controls.Clear();
                    var timePeriodList = g.timePeriods.ToList();
                    BuildNormalTimePeriodLayout(dateAndTimesModel.dateTimeRangeType, hideTimePeriod, phEventItems, timePeriodList);
                }
            }
        }

        /// <summary>
        /// Builds the normal time period layout.
        /// </summary>
        /// <param name="rangeType">Type of the range.</param>
        /// <param name="hideTimePeriod">If set to <c>true</c> [hide time period].</param>
        /// <param name="phEventItems">The PH event items.</param>
        /// <param name="calendarTimePeriods">The calendar time periods.</param>
        private void BuildNormalTimePeriodLayout(string rangeType, bool hideTimePeriod, PlaceHolder phEventItems, List<CalendarTimePeriod> calendarTimePeriods)
        {
            phEventItems.Controls.Clear();

            if (calendarTimePeriods != null)
            {
                const int ITEMS_PER_ROW = 4;
                int emptyItemsCount = ITEMS_PER_ROW - (calendarTimePeriods.Count % ITEMS_PER_ROW);
                phEventItems.Controls.Add(new LiteralControl("<div class=\"ACA_SmLabel ACA_SmLabel_FontSize aca_radio ACA_LeftPadding\"><table role=\"presentation\" class=\"ACA_FullWidthTable\" style=\"table-layout:fixed;\">"));

                for (int i = 0; i < calendarTimePeriods.Count + emptyItemsCount; i++)
                {
                    var trBegin = i % ITEMS_PER_ROW == 0 ? "<tr>" : string.Empty;
                    var trEnd = i % ITEMS_PER_ROW == 3 ? "</tr>" : string.Empty;
                    phEventItems.Controls.Add(new LiteralControl(string.Format("{0}<td class=\"ACA_NLong\"><div style=\"white-space:nowrap;\">", trBegin)));

                    if (i < calendarTimePeriods.Count)
                    {
                        var t = calendarTimePeriods[i];
                        var isRadioButtonEnabled = HasEnoughUnits;
                        var radioButton = CreateRadioButton(rangeType, t.startDate.Value, t.endDate.Value, t.amOrPM, hideTimePeriod, isRadioButtonEnabled);
                        phEventItems.Controls.Add(radioButton);
                    }
                    else
                    {
                        phEventItems.Controls.Add(new LiteralControl("&nbsp;"));
                    }

                    phEventItems.Controls.Add(new LiteralControl(string.Format("</div></td>{0}", trEnd)));
                }

                phEventItems.Controls.Add(new LiteralControl("</table></div>"));
            }
        }

        /// <summary>
        /// Creates the radio button control.
        /// </summary>
        /// <param name="rangeType">Type of the range.</param>
        /// <param name="startTime">The start time.</param>
        /// <param name="endTime">The end time.</param>
        /// <param name="amOrPm">The am or pm.</param>
        /// <param name="hideTime">If set to <c>true</c> [hide time].</param>
        /// <param name="enabled">If set to <c>true</c> [enabled].</param>
        /// <returns>The radio button control.</returns>
        private AccelaRadioButton CreateRadioButton(string rangeType, DateTime startTime, DateTime endTime, string amOrPm, bool hideTime, bool enabled)
        {
            var timeOption = ConverToInspectionTimeOption(rangeType, amOrPm);
            var isToSaveSpecificTime = timeOption == InspectionTimeOption.SpecificTime || timeOption == InspectionTimeOption.Unknow;

            AccelaRadioButton radioButton = new AccelaRadioButton();
            radioButton.Text = BuildTimePeriodText(timeOption, startTime, endTime, hideTime);
            radioButton.GroupName = "EventItemGroup";
            radioButton.Enabled = enabled;

            if (radioButton.Enabled)
            {
                radioButton.ID = "EventItem" + endTime.ToFileTimeUtc();

                HiddenField hfStartTime = new HiddenField();
                hfStartTime.ID = radioButton.ID + "_DateValue";
                DateTime dateValue = isToSaveSpecificTime ? startTime : startTime.Date;
                hfStartTime.Value = I18nDateTimeUtil.FormatToDateTimeStringForWebService(dateValue);
                radioButton.Controls.Add(hfStartTime);

                HiddenField hfEndTime = new HiddenField();
                hfEndTime.ID = radioButton.ID + "_EndDateValue";
                dateValue = isToSaveSpecificTime ? endTime : endTime.Date;
                hfEndTime.Value = I18nDateTimeUtil.FormatToDateTimeStringForWebService(dateValue);
                radioButton.Controls.Add(hfEndTime);

                HiddenField timeOptionHiddenField = new HiddenField();
                timeOptionHiddenField.ID = radioButton.ID + "_TimeOption";
                timeOptionHiddenField.Value = timeOption.ToString();
                radioButton.Controls.Add(timeOptionHiddenField);

                if ((isToSaveSpecificTime && LastSelectedDateTime != null && LastSelectedDateTime.Value == startTime)
                    || (!isToSaveSpecificTime && LastSelectedDateTime != null && LastSelectedDateTime.Value.Date == startTime.Date && LastSelectedTimeOption == timeOption))
                {
                    radioButton.Checked = true;
                }

                // add the onclick script.
                if (enabled && !string.IsNullOrEmpty(CalendarTimeSelected))
                {
                    radioButton.Attributes.Add("onclick", string.Format("{0}(this);", CalendarTimeSelected));
                }
            }

            return radioButton;
        }

        /// <summary>
        /// Builds the time period text.
        /// </summary>
        /// <param name="inspectionTimeOption">Type of the time.</param>
        /// <param name="startTime">The start time.</param>
        /// <param name="endTime">The end time.</param>
        /// <param name="hideTime">If set to <c>true</c> [hide time].</param>
        /// <returns>The time period text.</returns>
        private string BuildTimePeriodText(InspectionTimeOption inspectionTimeOption, DateTime startTime, DateTime endTime, bool hideTime)
        {
            string result = string.Empty;
            string timeBlockString = string.Format("{0} - {1}", I18nDateTimeUtil.FormatToTimeStringForUI(startTime, true), I18nDateTimeUtil.FormatToTimeStringForUI(endTime, true));
            string preText = string.Empty;

            switch (inspectionTimeOption)
            {
                case InspectionTimeOption.AllDay:
                    preText = GetTextByKey("aca_calendar_allday");
                    break;
                case InspectionTimeOption.AM:
                    preText = GetTextByKey("aca_calendar_morning");
                    break;
                case InspectionTimeOption.PM:
                    preText = GetTextByKey("aca_calendar_afternoon");
                    break;
                default:
                    preText = string.Empty;
                    break;
            }

            if (hideTime)
            {
                result = !string.IsNullOrEmpty(preText) ? preText : timeBlockString;
            }
            else
            {
                result = !string.IsNullOrEmpty(preText) ? string.Format("{0} ({1})", preText, timeBlockString) : timeBlockString;
            }

            return result;
        }

        /// <summary>
        /// Convers to inspection time option.
        /// </summary>
        /// <param name="rangeType">Type of the range.</param>
        /// <param name="amOrPM">The am or PM.</param>
        /// <returns>inspection time option.</returns>
        private InspectionTimeOption ConverToInspectionTimeOption(string rangeType, string amOrPM)
        {
            var result = InspectionTimeOption.Unknow;

            if (TYPE_START_END_TIME.Equals(rangeType, StringComparison.OrdinalIgnoreCase))
            {
                result = InspectionTimeOption.SpecificTime;
            }
            else if (TYPE_ALL_DAY.Equals(rangeType, StringComparison.OrdinalIgnoreCase))
            {
                result = InspectionTimeOption.AllDay;
            }
            else if (TYPE_AM_PM.Equals(rangeType, StringComparison.OrdinalIgnoreCase))
            {
                result = AM.Equals(amOrPM, StringComparison.OrdinalIgnoreCase) ? InspectionTimeOption.AM : InspectionTimeOption.PM;
            }

            return result;
        }

        /// <summary>
        /// Determines whether [is normal time periods] [the specified range type].
        /// </summary>
        /// <param name="rangeType">Type of the range.</param>
        /// <returns>
        /// <c>true</c> If [is normal time periods] [the specified range type]; otherwise, <c>false</c>.
        /// </returns>
        private bool IsNormalTimePeriods(string rangeType)
        {
            bool result = false;

            if (TYPE_START_END_TIME.Equals(rangeType, StringComparison.OrdinalIgnoreCase))
            {
                result = true;
            }

            return result;
        }

        /// <summary>
        /// Determines whether [has any active time] [the specified date and times model].
        /// </summary>
        /// <param name="dateAndTimesModel">The date and times model.</param>
        /// <returns>
        /// <c>true</c> If [has any active time] [the specified date and times model]; otherwise, <c>false</c>.
        /// </returns>
        private bool HasAnyActiveTime(DateTimeRangePageModel dateAndTimesModel)
        {
            var result = false;

            if (dateAndTimesModel != null && dateAndTimesModel.times != null)
            {
                result = dateAndTimesModel.times.Any(p => p.active);
            }

            return result;
        }

        /// <summary>
        /// Get current selected radio control
        /// </summary>
        /// <returns>AccelaRadioButton for selected event items.</returns>
        private AccelaRadioButton GetSelectedEventItemControl()
        {
            ArrayList controls = new ArrayList();

            if (phMorningEventItems.Visible)
            {
                controls.AddRange(phMorningEventItems.Controls);
            }

            if (phAfternoonEventItems.Visible)
            {
                controls.AddRange(phAfternoonEventItems.Controls);
            }

            if (phDayEventItems.Visible)
            {
                controls.AddRange(phDayEventItems.Controls);
            }

            var result = controls.ToArray().FirstOrDefault(p => p is AccelaRadioButton && ((AccelaRadioButton)p).Checked) as AccelaRadioButton;
            return result;
        }

        /// <summary>
        /// Get the month span from date1 to date2.
        /// </summary>
        /// <param name="date1">The DateTime date1.</param>
        /// <param name="date2">The DateTime date2.</param>
        /// <returns>The month span.</returns>
        private int SubtractDateMonth(DateTime date1, DateTime date2)
        {
            return (date1.Year * 12) + date1.Month - (date2.Year * 12) - date2.Month;
        }

        #endregion Methods
    }
}
