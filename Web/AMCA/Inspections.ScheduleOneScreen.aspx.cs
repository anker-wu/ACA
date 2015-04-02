/**
* <pre>
* 
*  Accela Citizen Access
*  File: InspectionsScheduleOneScreen.aspx.cs
* 
*  Accela, Inc.
*  Copyright (C): 2007-2012
* 
*  Description:
*  This screen accepts the month, day, year, and time of day that
*  the inspection is to be scheduled on.
* 
*  Notes:
*      $Id$.
*  Revision History
*  &lt;Date&gt;,		&lt;Who&gt;,			&lt;What&gt;
*  08/01/2008           dave.brewster           Created screen.
*  10-09-2008           DWB                     2008 Mobile ACA 6.7.0 interface redesign
*  03/19/2009           DWB                     Added code format <hr> elements for small devices
*                                               and added code to have failed inspection schedule and
*                                               cancel requestreturn to the ScheduleOneScreen.aspx 
*                                               page so the user can pick a new date (This was a requirement
*                                               for Sac County that was added to ACA.)
*  04/01/2009           Dave Brewster           Added code to presever AltID and pass it to other pages.
*  10-10-2009           Dave Brewster           Added new breadcrumb logic.
*  12/03/2009           Dave Brewster           Corrected Hide Inspection Times in ACA logic to implenent 
*                                               block by "Day" logic.
* </pre>
*/
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Linq;

using Accela.ACA.Common;
using Accela.ACA.Common.Common;
using Accela.ACA.Common.Util;
using Accela.ACA.BLL.Common;
using Accela.ACA.BLL.Inspection;
using Accela.ACA.Inspection;
using Accela.ACA.Web.Common;
using Accela.ACA.Web.Inspection;
using Accela.ACA.WSProxy;
/// <summary>
/// 
/// </summary>
public partial class InspectionsScheduleOneScreen : AccelaPage
{
    #region Fields

    /// <summary>
    /// the view id number.
    /// </summary>
    protected long viewId = 5018;

    /// <summary>
    /// range type: All Day
    /// </summary>
    private const string TYPE_ALL_DAY = "ALL_DAY";

    /// <summary>
    /// range type: AM/PM
    /// </summary>
    private const string TYPE_AM_PM = "AM_OR_PM";

    /// <summary>
    /// range type: specific time range
    /// </summary>
    private const string TYPE_START_END_TIME = "START_END_TIME";

    /// <summary>
    /// AM const string
    /// </summary>
    private const string AM = "am";

    /// <summary>
    /// aca calendar model.
    /// </summary>
    private CalendarModel _calendarModel = null;

    #endregion Fields

    public StringBuilder CalenderMonthYear = new StringBuilder();
    public StringBuilder MonthList = new StringBuilder();
    public StringBuilder DaysList = new StringBuilder();
    public StringBuilder HoursList = new StringBuilder();
    public StringBuilder MinutesList = new StringBuilder();
    public StringBuilder AmPmList = new StringBuilder();
    public StringBuilder ErrorMessage = new StringBuilder();
    public StringBuilder Breadcrumbs = new StringBuilder();
    public StringBuilder HiddenFields = new StringBuilder();
    public StringBuilder TimeList = new StringBuilder();
    public StringBuilder RecordInfo = new StringBuilder();
    public StringBuilder ButtonCaption = new StringBuilder();
    public string BackForwardLinks = string.Empty;
    public string NextPage = string.Empty;
    public string MonthLabel = string.Empty;
    public string DayLabel = string.Empty;
    public string TimeLabel = string.Empty;
    public string Comments = string.Empty;
    public string Tip = string.Empty;
    public string PageTitle = string.Empty;

    private string PermitNo = string.Empty;
    private string PermitType = string.Empty;
    private string SearchBy = string.Empty;
    private string SearchType = string.Empty;
    private string ViewPermitPageNo = string.Empty;  // ResultPage for "View Permits" breadcrumb link.
    private string InspectionsPageNo = string.Empty; // ResultPage for "View Permits > Inspections" Breadcrumb link
    private string Mode = string.Empty;
    private string Action = string.Empty;
    private string InspSeqNum = string.Empty;
    private string InspUnits = string.Empty;
    private string TheMonth = string.Empty;
    private string TheDay = string.Empty;
    private string InAdvance = string.Empty;
    private string ScheduleFailedMessage = string.Empty;
    private string AltID = string.Empty;
    private bool isRequestPending = false;
    private string isReadyTimeEnabled = string.Empty;
    private bool isRequestSameDayNextDay = false;
    private string InspectionId = string.Empty;
    private string ScheduleManner = string.Empty;

    private CapModel4WS capModel = null;
    private string currentAgency = string.Empty;
    private DateTime currentAgencyDate = DateTime.MinValue;

    private bool HasEnoughUnits = false;
    private DateTimeRangePageModel SelectedWorkingDaysAndTime = null;
    private DateTime? scheduledDateTime = null;
    private InspectionTimeOption finishTimeOption = InspectionTimeOption.Unknow;
    private string isGoToNextPage = ACAConstant.COMMON_N;

    /// <summary>
    /// Current Agency Code based on CAP Model in Session
    /// </summary>
    private string CurrentAgency
    {
        get
        {
            if (String.IsNullOrEmpty(currentAgency))
            {
                currentAgency = CapUtil.GetAgencyCode(ModuleName);
            }

            return currentAgency;
        }
    }

    /// <summary>
    /// Current Agency Date
    /// </summary>
    private DateTime CurrentAgencyAgencyDate
    {
        get
        {
            if (currentAgencyDate == DateTime.MinValue)
            {
                ITimeZoneBll timeBll = (ITimeZoneBll)ObjectFactory.GetObject(typeof(ITimeZoneBll));
                currentAgencyDate = timeBll.GetAgencyCurrentDate(CurrentAgency);
            }

            return currentAgencyDate;
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
            return Session["WorkingDaysAndTimes"] as DateTimeRangePageModel[];
        }

        set
        {
            Session["WorkingDaysAndTimes"] = value;
        }
    }

    /// <summary>
    /// Displays Months List with Year for selection
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Load(object sender, EventArgs e)
    {
        ValidationChecks("Inspections.ScheduleOneScreen.aspx");

        //get inspection from teh app session
        string typeRowNumber = MyProxy.GetFieldValue("TypeRowNumber", false);
        int rowNumber = typeRowNumber != string.Empty ? int.Parse(typeRowNumber) : int.Parse(MyProxy.GetFieldValue("RowNumber", false));
        InspectionViewModel listRow = null;
        InspectionTypeDataModel typeRow = null;
        InspectionScheduleType scheduleType;
        bool isRescheduleOrCancel = true;

        isGoToNextPage = MyProxy.GetFieldValue("isGoToNextPage", false);

        if (string.IsNullOrEmpty(isGoToNextPage))
        {
            isGoToNextPage = ACAConstant.COMMON_N;
        }

        if (typeRowNumber != string.Empty)
        {
            isRescheduleOrCancel = false;
            List<InspectionTypeDataModel> inspectionTypeModels = (List<InspectionTypeDataModel>)Session["AMCA_WIZARD_INSPECTIONS"];
            typeRow = inspectionTypeModels[rowNumber];
        }
        else
        {
            List<InspectionViewModel> inspectionViewModels = (List<InspectionViewModel>)Session["AMCA_INSPECTION_MODELS"];

            InspectionId = MyProxy.GetFieldValue("InspectionId", false);
            int inspectionID;
            if (!int.TryParse(InspectionId, out inspectionID))
            {
                inspectionID = 0; // this should never happen - 
            }
            for (rowNumber = 0; rowNumber < inspectionViewModels.Count; rowNumber++)
            {
                if (inspectionID == inspectionViewModels[rowNumber].ID)
                {
                    listRow = inspectionViewModels[rowNumber];
                    InspectionId = listRow.ID.ToString();
                    break;
                }
            }
        }

        // get current state and parameters
        PermitNo = MyProxy.GetFieldValue("PermitNo", false);
        AltID = MyProxy.GetFieldValue("AltID", false);
        PermitType = MyProxy.GetFieldValue("PermitType", false);
        SearchBy = MyProxy.GetFieldValue("SearchBy", false);
        SearchType = MyProxy.GetFieldValue("SearchType", false);
        ViewPermitPageNo = MyProxy.GetFieldValue("ViewPermitPageNo", false);
        InspectionsPageNo = MyProxy.GetFieldValue("InspectionsPageNo", false);
        Mode = MyProxy.GetFieldValue("Mode", false);
        ScheduleManner = MyProxy.GetFieldValue(ACAConstant.INSPECTION_SCHEDULING_MANNER, false);
        ScheduleFailedMessage = MyProxy.GetFieldValue("ErrorMsg", false);
        string InspType = string.Empty;
        string inspStatus = string.Empty;
        bool inspectionRequired = false;
        string inspectionGroup = string.Empty;

        if (isRescheduleOrCancel)
        {
            InspSeqNum = listRow.TypeID.ToString(); // MyProxy.GetFieldValue("InspSeqNum", false);
            InspUnits = listRow.InspectionDataModel.Units.ToString(); // MyProxy.GetFieldValue("InspUnits", false);
            InAdvance = listRow.InspectionDataModel.InAdvance ? "Y" : "N";
            InspectionId = listRow.ID.ToString();
            isReadyTimeEnabled = listRow.InspectionDataModel.ReadyTimeEnabled ? "Y" : "N";
            InspType = listRow.InspectionDataModel.Type; // MyProxy.GetFieldValue("InspType", false);
            inspStatus = listRow.InspectionDataModel.Status.ToString(); // MyProxy.GetFieldValue("InspStatus", false);
            scheduleType = listRow.InspectionDataModel.ScheduleType;
            inspectionRequired = listRow.InspectionDataModel.Required;
            inspectionGroup = listRow.InspectionDataModel.Group;
        }
        else
        {
            InspSeqNum = typeRow.TypeID.ToString(); // MyProxy.GetFieldValue("InspSeqNum", false);
            InspUnits = typeRow.Units.ToString(); // MyProxy.GetFieldValue("InspUnits", false);
            InAdvance = typeRow.InAdvance ? "Y" : "N";
            isReadyTimeEnabled = typeRow.ReadyTimeEnabled ? "Y" : "N";
            InspType = typeRow.Type; // MyProxy.GetFieldValue("InspType", false);
            inspStatus = typeRow.Status.ToString(); // MyProxy.GetFieldValue("InspStatus", false);
            scheduleType = typeRow.ScheduleType;
            inspectionRequired = typeRow.Required;
            inspectionGroup = typeRow.Group;
        }

        if (isReadyTimeEnabled != "Y")
        {
            isReadyTimeEnabled = "N";
        }

        // initiaize breadcrumbs
        string breadCrumbIndex = MyProxy.GetFieldValue("BreadCrumbIndex", false);
        bool isElipseLink = MyProxy.GetFieldValue("IsElipseLink", false) != string.Empty;
        StringBuilder sbWork = new StringBuilder();
        bool isBreadcrumbPagingMode = MyProxy.GetFieldValue("PagingMode", false) == "true";


        string RescheduleRestrictionSettings = MyProxy.GetFieldValue(ACAConstant.INSPECTION_RESCHEDULE_RESTRICTION_SETTINGS, false);
        if (RescheduleRestrictionSettings == string.Empty)
        {
            RescheduleRestrictionSettings = MyProxy.GetFieldValue(ACAConstant.INSPECTION_RESCHEDULE_RESTRICTION_SETTINGS + "0", false);
            RescheduleRestrictionSettings += "|" + MyProxy.GetFieldValue(ACAConstant.INSPECTION_RESCHEDULE_RESTRICTION_SETTINGS + "1", false);
            RescheduleRestrictionSettings += "|" + MyProxy.GetFieldValue(ACAConstant.INSPECTION_RESCHEDULE_RESTRICTION_SETTINGS + "2", false);
            RescheduleRestrictionSettings += "|" + MyProxy.GetFieldValue(ACAConstant.INSPECTION_RESCHEDULE_RESTRICTION_SETTINGS + "3", false);
        }
        string CancellationRestrictionSettings = MyProxy.GetFieldValue(ACAConstant.INSPECTION_CANCELLATION_RESTRICTION_SETTINGS, false);
        if (CancellationRestrictionSettings == string.Empty)
        {
            CancellationRestrictionSettings = MyProxy.GetFieldValue(ACAConstant.INSPECTION_CANCELLATION_RESTRICTION_SETTINGS + "0", false);
            CancellationRestrictionSettings += "|" + MyProxy.GetFieldValue(ACAConstant.INSPECTION_CANCELLATION_RESTRICTION_SETTINGS + "1", false);
            CancellationRestrictionSettings += "|" + MyProxy.GetFieldValue(ACAConstant.INSPECTION_CANCELLATION_RESTRICTION_SETTINGS + "2", false);
            CancellationRestrictionSettings += "|" + MyProxy.GetFieldValue(ACAConstant.INSPECTION_CANCELLATION_RESTRICTION_SETTINGS + "3", false);
        }

        if (ScheduleFailedMessage.Length > 0)
        {
            ErrorMessage.Append(ErrorFormat);
            ErrorMessage.Append(ScheduleFailedMessage);
            ErrorMessage.Append(ErrorFormatEnd);
        }

        Action = MyProxy.GetFieldValue("Action", false);
        TheMonth = MyProxy.GetFieldValue("cboYear", false);
        TheDay = MyProxy.GetFieldValue("cboDay", false);

        capModel = AppSession.GetCapModelFromSession(ModuleName);


        InspectionParameter inspectionParameter = new InspectionParameter();
        inspectionParameter.AgencyCode = CurrentAgency;
        inspectionParameter.Action = EnumUtil<InspectionAction>.Parse(Action, InspectionAction.None);
        inspectionParameter.Type = InspType;
        inspectionParameter.InAdvance = (InAdvance == ACAConstant.COMMON_Y ? true : false);
        inspectionParameter.ID = InspectionId;
        inspectionParameter.TypeID = InspSeqNum;
        inspectionParameter.ScheduleType = scheduleType;
        inspectionParameter.ReadyTimeEnabled = (isReadyTimeEnabled == ACAConstant.COMMON_Y ? true : false);
        inspectionParameter.Required = inspectionRequired;

        CapIDModel recordIDModel = TempModelConvert.Trim4WSOfCapIDModel(capModel.capID);

        InspectionTypeModel inspectionTypeModel = new InspectionTypeModel();
        inspectionTypeModel.sequenceNumber = long.Parse(InspSeqNum);
        inspectionTypeModel.groupCode = inspectionGroup;
        inspectionTypeModel.type = InspType;

        SetUIBySchedulingManner(inspectionParameter);

        Range<DateTime?> activityDateRange = new Range<DateTime?>();

        if (isGoToNextPage == ACAConstant.COMMON_Y)
        {
            activityDateRange = GetActivityDate(out finishTimeOption, inspectionParameter);
            scheduledDateTime = activityDateRange.LowerBound;
        }

        // retrieves List of Months
        List<DateTime> months = MyProxy.MonthsRetrieve(CurrentAgencyAgencyDate);

        if (MyProxy.OnErrorReturn)
        {  // Proxy Exception 
            ErrorMessage.Append(ErrorFormat);
            ErrorMessage.Append(MyProxy.ExceptionMessage);
            ErrorMessage.Append(ErrorFormatEnd);
        }

        Tip = "Only available dates and times are displayed.";

        iPhonePageTitle = "Schedule Inspection";
        if (isRequestPending || isRequestSameDayNextDay)
        {
            iPhonePageTitle = "Request Inspection";
            string newTip = GetSameDayNextDaySelectionTip();
            if (newTip != null && newTip != string.Empty)
            {
                Tip = newTip;
            }
        }
        if (!isiPhone)
        {
            PageTitle = "<div id=\"pageTitle\">" + iPhonePageTitle + "</div><hr />";
        }
        else
        {
            ButtonCaption.Append("<center>");
        }
        ButtonCaption.Append("<input id=\"Submit1\" type=\"submit\" value=\"");

        RecordInfo.Append("<Label id=\"pageSectionTitle\">Record No: </Label>");
        RecordInfo.Append("<Label id=\"pageLineText\">");
        RecordInfo.Append(AltID);
        RecordInfo.Append("</Label>");
        RecordInfo.Append("<br>");
        RecordInfo.Append("<Label id=\"pageSectionTitle\">Inspection Type: </Label>");
        RecordInfo.Append("<Label id=\"pageLineText\">");
        RecordInfo.Append(InspType);
        RecordInfo.Append("</Label>");
        RecordInfo.Append("<br>");

        if (capModel.addressModel != null && capModel.addressModel.displayAddress != null)
        {
            RecordInfo.Append("<Label id=\"pageSectionTitle\">Location: </Label>");
            RecordInfo.Append("<br><Label id=\"pageLineText\">");
            RecordInfo.Append(capModel.addressModel.displayAddress);
            RecordInfo.Append("</Label>");
        }

        string sStyle = "<a id=\"pageLineLink\" href=\"";
        bool bLoadDaysList = false;

        if (string.IsNullOrEmpty(TheMonth) && !isRequestPending && !isRequestSameDayNextDay)
        {
            isGoToNextPage = ACAConstant.COMMON_N;
            MonthLabel = "<div id=\"pageSectionTitle\"><label\">Select Month:</label></div>";
            DayLabel = "";
            TimeLabel = "";
            int monthsInList = 0;
            MonthList.Append("<div id=\"pageText\">");
            MonthList.Append("<select class=\"pageTextInput\" id=\"cboMonthYear\" name=\"cboYear\">");

            for (int i = 0; i < months.Count; i++)
            {
                DateTime dt = months[i];
                if (TheMonth == string.Empty)
                {
                    TheMonth = dt.ToString("MM-yyyy");
                }
                monthsInList++;
                MonthList.Append("<option value=" + dt.ToString("MM-yyyy") + ">" + dt.ToString("MMMM yyyy") + "</option>");
            }

            MonthList.Append(" </select>");
            MonthList.Append(" </div>");
            NextPage = "Inspections.ScheduleOneScreen.aspx?State=" + State
                + "&InspectionId=" + InspectionId
                + "&" + ACAConstant.INSPECTION_IN_ADVANCE + "=" + InAdvance
                + "&Action=" + Action
                + "&PagingMode=true"
                + "&Module=" + ModuleName;

            if (monthsInList == 1)
            {
                bLoadDaysList = true;
                MonthList = new StringBuilder();
            }
            else
            {
                ButtonCaption.Append("Continue\">");
            }
        }
        else
        {
            bLoadDaysList = true;
        }

        string errorLocation = "Process Days of Month";

        if (bLoadDaysList)
        {
            if (string.IsNullOrEmpty(TheDay) && !isRequestPending && !isRequestSameDayNextDay) //schedule using calendar,select date
            {
                errorLocation = "Loading available days";
                try
                {
                    isGoToNextPage = ACAConstant.COMMON_N;

                    HiddenFields.Append(HTML.PresentHiddenField("cboYear", TheMonth));
                    string[] month_year = TheMonth.Split('-');
                    DateTime aMonthYear = DateTime.Parse(month_year[0] + "/1/" + month_year[1]);

                    MonthLabel = "<div id=\"pageSectionTitle\"><label >Month: </label>"
                        + "<span id=\"pageLineText\">" + aMonthYear.ToString("MMMM yyyy") + "</span>"
                        + " "
                        + sStyle + "Inspections.ScheduleOneScreen.aspx?State=" + State
                        + "&InspectionId=" + InspectionId
                        + "&PermitNo=" + PermitNo
                        + "&AltID=" + AltID
                        + "&PermitType=" + PermitType
                        + "&SearchBy=" + SearchBy
                        + "&SearchType=" + SearchType
                        + "&ViewPermitPageNo=" + ViewPermitPageNo
                        + "&InspectionsPageNo=" + InspectionsPageNo
                        + "&InspSeqNum=" + InspSeqNum
                        + "&InspUnits=" + InspUnits
                        + "&InspType=" + InspType
                        + "&Mode=" + Mode
                        + "&Module=" + ModuleName
                        + "&RowNumber=" + rowNumber
                        + "&TypeRowNumber=" + typeRowNumber
                        + "&PagingMode=true"
                        + "&" + ACAConstant.INSPECTION_IN_ADVANCE + "=" + InAdvance
                        + "\"> Change </a>"
                        + "</div>";
                    TimeLabel = "";
                    DayLabel = "<div id=\"pageSectionTitle\">Select Date: </div>";
                    DaysList.Append("<div =\"pageText\">");

                    List<DateTime> Days = new List<DateTime>();
                    int MonthIndex = 0;
                    errorLocation = "Locate first month";

                    if (months.Count > 0)
                    {
                        for (MonthIndex = 0; MonthIndex < months.Count; MonthIndex++)
                        {
                            DateTime dt = Convert.ToDateTime(months[MonthIndex]);

                            if (dt.ToString("MM-yyyy") == (month_year[0] + "-" + month_year[1]))
                            {
                                break;
                            }
                        }
                    }
                    errorLocation = "Read days of month";

                    DateTime startDate = Convert.ToDateTime(months[MonthIndex]);
                    DateTime endDate = startDate.AddMonths(1).AddDays(-1);

                    var calendarBll = ObjectFactory.GetObject<ICalendarBll>();
                    WorkingDaysAndTimes = calendarBll.GetNextWorkingDaysAndTimes(CurrentAgency, startDate, endDate, recordIDModel, inspectionTypeModel);

                    if (WorkingDaysAndTimes != null && WorkingDaysAndTimes.Count() > 0)
                    {
                        foreach (DateTimeRangePageModel dateTimeRangePageModel in WorkingDaysAndTimes.OrderBy(o => o.date))
                        {
                            if (dateTimeRangePageModel.date != null && HasAnyActiveTime(dateTimeRangePageModel))
                            {
                                Days.Add(dateTimeRangePageModel.date.Value);
                            }
                        }
                    }

                    if (Days.Count == 0)
                    {
                        DaysList.Append("<i>No available dates were found for this inspection type.</i>");
                    }
                    else
                    {
                        DaysList.Append("<select class=\"pageTextInput\" id=\"cboDayList\" name=\"cboDay\">");
                        errorLocation = "Build days of list";

                        for (int j = 0; j < Days.Count; j++)
                        {
                            DateTime oneDay = Days[j];
                            DaysList.Append("<option value=" + oneDay.ToString("dd") + ">" + oneDay.ToString("d (dddd)") + "</option>");
                        }

                        DaysList.Append(" </select>");
                    }

                    NextPage = "Inspections.ScheduleOneScreen.aspx?State=" + State
                            + "&InspectionId=" + InspectionId
                            + "&" + ACAConstant.INSPECTION_IN_ADVANCE + "=" + InAdvance
                            + "&Action=" + Action
                            + "&PagingMode=true"
                            + "&Module=" + ModuleName;

                    ButtonCaption.Append("Continue\">");
                    DaysList.Append("</div>");
                    DaysList.Append("<br/>");
                }
                catch (Exception ex)
                {
                    ErrorMessage.Append(ErrorFormat);
                    ErrorMessage.Append(errorLocation + ": ");
                    ErrorMessage.Append(ex.Message);
                    ErrorMessage.Append(ErrorFormatEnd);
                }
            }
            else
            {
                #region is finish oper

                if (isGoToNextPage == ACAConstant.COMMON_Y)
                {
                    if (scheduledDateTime != null || inspectionParameter.ScheduleType == InspectionScheduleType.RequestOnlyPending || inspectionParameter.ScheduleType == InspectionScheduleType.RequestSameDayNextDay)
                    {
                        inspectionParameter.ScheduledDateTime = scheduledDateTime;
                        inspectionParameter.EndScheduledDateTime = activityDateRange.UpperBound;
                        inspectionParameter.TimeOption = finishTimeOption;

                        bool isFailed = false;
                        AvailableTimeResultModel availableTimeResultModel = MyProxy.GetAvailableTimeResult(scheduledDateTime, inspectionParameter, recordIDModel);

                        CalendarModel acaCalendarModel = GetcalendarModel();
                        double calendarAttempts = Convert.ToDouble(acaCalendarModel.calendarAttempts);

                        if (availableTimeResultModel != null && scheduledDateTime != null)
                        {
                            // Show message that no available time found when exceed the count of trying find the available time.
                            if (!isFailed && availableTimeResultModel.flag != ACAConstant.INSPECTION_FLAG_SUCCESS)
                            {
                                ErrorMessage.Append(ErrorFormat);

                                if (calendarAttempts < 1)
                                {
                                    ErrorMessage.Append(GetTextByKey("aca_inspection_msg_novalidate_datetime"));
                                }
                                else
                                {
                                    DateTime noAvailableTimeStart = scheduledDateTime.Value;
                                    DateTime noAvailableTimeEnd = scheduledDateTime.Value.AddDays(calendarAttempts - 1);

                                    string noAvailableTimeStartString = I18nDateTimeUtil.FormatToInspectionDateTimeStringForUI(noAvailableTimeStart);
                                    string noAvailableTimeEndString = I18nDateTimeUtil.FormatToInspectionDateTimeStringForUI(noAvailableTimeEnd.Date.AddHours(23).AddMinutes(59));
                                    
                                    ErrorMessage.Append(string.Format(GetTextByKey("aca_inspection_message_no_validate_datetime"), noAvailableTimeStartString, noAvailableTimeEndString));
                                }

                                ErrorMessage.Append(ErrorFormatEnd);

                                isFailed = true;
                            }

                            // show error message if the selected date cannot found enough inspection unit. 
                            if (!isFailed && !string.IsNullOrEmpty(availableTimeResultModel.startTime)
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
                                if ((inspectionParameter.ScheduleType == InspectionScheduleType.RequestSameDayNextDay && DateTime.Compare(scheduledDateTime.Value.Date, availableTimeResultModel.scheduleDate.Value.Date) < 0)
                                    || (inspectionParameter.ScheduleType != InspectionScheduleType.RequestSameDayNextDay && !IsGivenDateTimeAvailable(scheduledDateTime, startAvailableTime, availableTimeResultModel.startAMPM, finishTimeOption)))
                                {
                                    ErrorMessage.Append(ErrorFormat);

                                    if (calendarAttempts < 1)
                                    {
                                        ErrorMessage.Append(GetTextByKey("aca_inspection_msg_novalidate_datetime"));
                                    }
                                    else
                                    {
                                        string stratAvailableTimeInfo = I18nDateTimeUtil.FormatToInspectionDateTimeStringForUI(startAvailableTime);
                                        ErrorMessage.Append(string.Format(GetTextByKey("aca_inspection_message_validate_datetime"), stratAvailableTimeInfo));
                                    }
                                    
                                    ErrorMessage.Append(ErrorFormatEnd);

                                    isFailed = true;
                                }
                            }
                        }

                        string requestDayOption = string.Empty;
                        if (inspectionParameter.ScheduleType == InspectionScheduleType.RequestSameDayNextDay)
                        {
                            requestDayOption = Convert.ToString(GetRequestSameDayNextDayOption());
                        }

                        if (!isFailed)
                        {
                            string strStartScheduledDateTime = scheduledDateTime != null ? I18nDateTimeUtil.FormatToDateTimeStringForWebService(scheduledDateTime.Value) : string.Empty;
                            string strEndScheduledDateTime = activityDateRange.UpperBound != null ? I18nDateTimeUtil.FormatToDateTimeStringForWebService(activityDateRange.UpperBound.Value) : string.Empty;

                            NextPage = "Inspections.Schedule.aspx?State=" + State
                                       + "&" + ACAConstant.INSPECTION_IN_ADVANCE + "=" + InAdvance
                                       + "&Action=" + Action
                                       + "&Module=" + ModuleName
                                       + "&scheduledDateTime=" + strStartScheduledDateTime
                                       + "&endScheduledDateTime=" + strEndScheduledDateTime
                                       + "&finishTimeOption=" + finishTimeOption.ToString()
                                       + "&rDayOption=" + requestDayOption;

                            Server.Transfer(NextPage);
                        }
                    }
                }

                #endregion

                if (!isRequestPending && !isRequestSameDayNextDay) //schedule using calendar,after select date
                {
                    errorLocation = "Load times of day";
                    try
                    {
                        HiddenFields.Append(HTML.PresentHiddenField("cboYear", TheMonth));
                        HiddenFields.Append(HTML.PresentHiddenField("cboDay", TheDay));

                        string[] month_year = TheMonth.Split('-');
                        DateTime aMonthYear = DateTime.Parse(month_year[0] + "/1/" + month_year[1]);
                        MonthLabel = "<div id=\"pageSectionTitle\"><label >Month: </label>"
                            + "<span id=\"pageLineText\">" + aMonthYear.ToString("MMMM yyyy") + "</span>"
                            + " "
                            + sStyle + "Inspections.ScheduleOneScreen.aspx?State=" + State
                            + "&InspectionId=" + InspectionId
                            + "&PermitNo=" + PermitNo
                            + "&AltID=" + AltID
                            + "&PermitType=" + PermitType
                            + "&" + ACAConstant.INSPECTION_IN_ADVANCE + "=" + InAdvance
                            + "&SearchBy=" + SearchBy
                            + "&SearchType=" + SearchType
                            + "&ViewPermitPageNo=" + ViewPermitPageNo
                            + "&InspectionsPageNo=" + InspectionsPageNo
                            + "&InspSeqNum=" + InspSeqNum
                            + "&InspUnits=" + InspUnits
                            + "&InspType=" + InspType
                            + "&Module=" + ModuleName
                            + "&Mode=" + Mode
                            + "&RowNumber=" + rowNumber
                            + "&TypeRowNumber=" + typeRowNumber
                            + "&PagingMode=true"
                            + "\"> Change </a>"
                            + "</div>";

                        DateTime aDayOfMonth = DateTime.Parse(month_year[0] + "/" + TheDay + "/" + month_year[1]);

                        DayLabel = "<div id=\"pageSectionTitle\"><label\">Date: </label>"
                            + "<span id=\"pageLineText\">" + aDayOfMonth.ToString("d (dddd)") + "</span>"
                            + " "
                            + sStyle + "Inspections.ScheduleOneScreen.aspx?State=" + State
                            + "&InspectionId=" + InspectionId
                            + "&PermitNo=" + PermitNo
                            + "&AltID=" + AltID
                            + "&PermitType=" + PermitType
                            + "&" + ACAConstant.INSPECTION_IN_ADVANCE + "=" + InAdvance
                            + "&SearchBy=" + SearchBy
                            + "&SearchType=" + SearchType
                            + "&ViewPermitPageNo=" + ViewPermitPageNo
                            + "&InspectionsPageNo=" + InspectionsPageNo
                            + "&cboYear=" + TheMonth
                            + "&InspSeqNum=" + InspSeqNum
                            + "&InspUnits=" + InspUnits
                            + "&InspType=" + InspType
                            + "&Module=" + ModuleName
                            + "&Mode=" + Mode
                            + "&RowNumber=" + rowNumber
                            + "&TypeRowNumber=" + typeRowNumber
                            + "&PagingMode=true"
                            + "\"> Change </a>"
                            + "</div>";

                        errorLocation = "Time of Day Retrieve";

                        ShowTimePeriods(aDayOfMonth, inspectionParameter, recordIDModel);

                        isGoToNextPage = ACAConstant.COMMON_Y;
                    }
                    catch (Exception ex)
                    {
                        ErrorMessage.Append(ErrorFormat);
                        ErrorMessage.Append(errorLocation + ": ");
                        ErrorMessage.Append(ex.Message);
                        ErrorMessage.Append(ErrorFormatEnd);
                    }
                }
                else //isRequestPending or isRequestSameDayNextDay 
                {
                    try
                    {
                        HiddenFields.Append(HTML.PresentHiddenField("isRequestPending", "Y"));
                        if (isRequestSameDayNextDay) //isRequestSameDayNextDay
                        {
                            errorLocation = "Create Same Day - Next Day options";

                            InitSameDayNextDaySelection(inspectionTypeModel, inspectionParameter, recordIDModel);
                        }
                        else if (isReadyTimeEnabled == ACAConstant.COMMON_Y)
                        {
                            // Add ready time lable and time of day drop down lists here

                            Tip = GetTextByKey("per_inspectionshedule_label_selectreadytime");

                            StringBuilder work = new StringBuilder();
                            work.Append("<select class=\"pageTextInputTimeValue\" id=\"cboHourList\" name=\"cboHour\">");
                            errorLocation = "Build Hour list";
                            for (int j = 1; j <= 12; j++)
                            {
                                work.Append("<option value=" + j.ToString("00") + ">" + j.ToString("00") + "</option>");
                            }
                            work.Append(" </select>");
                            work.Append(":");

                            work.Append("<select class=\"pageTextInputTimeValue\" id=\"cboMinuteList\" name=\"cboMinute\">");
                            errorLocation = "Build Minute list";
                            for (int j = 0; j <= 60; j++)
                            {
                                work.Append("<option value=" + j.ToString("00") + ">" + j.ToString("00") + "</option>");
                            }
                            work.Append(" </select>");
                            work.Append(":");

                            work.Append("<select class=\"pageTextInputTimeValue\" id=\"cboAmPmList\" name=\"cboAmPm\">");
                            work.Append("<option value=" + GetTextByKey("common_am_label") + ">" + GetTextByKey("common_am_label") + "</option>");
                            work.Append("<option value=" + GetTextByKey("common_pm_label") + ">" + GetTextByKey("common_pm_label") + "</option>");
                            work.Append(" </select>");

                            TimeList.Append("<div id=\"pageText\">");
                            TimeList.Append(work.ToString());
                            TimeList.Append("</div>");
                        }

                        isGoToNextPage = ACAConstant.COMMON_Y;
                    }
                    catch (Exception ex)
                    {
                        ErrorMessage.Append(ErrorFormat);
                        ErrorMessage.Append(errorLocation + ": ");
                        ErrorMessage.Append(ex.Message);
                        ErrorMessage.Append(ErrorFormatEnd);
                    }
                }

                Comments = "<div id=\"pageSectionTitle\"><label>Comments (optional): </label>"
                         + "<textarea name=\"Comments\" cols=\"20\" rows=\"4\" class=\"pageTextAreaInput\">" + MyProxy.GetFieldValue("Comments", false) + "</textarea><br />"
                         + "</div><br/>";

                NextPage = "Inspections.ScheduleOneScreen.aspx?State=" + State
                         + "&InspectionId=" + InspectionId
                         + "&" + ACAConstant.INSPECTION_IN_ADVANCE + "=" + InAdvance
                         + "&Action=" + Action
                         + "&PagingMode=true"
                         + "&Module=" + ModuleName;

                ButtonCaption.Append("Continue\">");
            }
        }

        if (isiPhone)
        {
            ButtonCaption.Append("</center>");
        }

        #region head link
        try
        {
            errorLocation = "Build Breadcrumbs";
            string[] resceduleRestrictions = RescheduleRestrictionSettings.Split('|');
            string[] cancelRestrictions = CancellationRestrictionSettings.Split('|');
            sbWork.Append("&PermitNo=" + PermitNo);
            sbWork.Append("&AltID=" + AltID);
            sbWork.Append("&PermitType=" + PermitType);
            sbWork.Append("&SearchBy=" + SearchBy);
            sbWork.Append("&SearchType=" + SearchType);
            sbWork.Append("&Mode=" + Mode);
            sbWork.Append("&Module=" + ModuleName);
            sbWork.Append("&RowNumber=" + rowNumber);
            sbWork.Append("&TypeRowNumber=" + typeRowNumber);
            sbWork.Append("&InspectionsPageNo=" + InspectionsPageNo);
            sbWork.Append("&ViewPermitPageNo=" + ViewPermitPageNo);
            sbWork.Append("&" + ACAConstant.INSPECTION_IN_ADVANCE + "=" + InAdvance);
            sbWork.Append("&" + ACAConstant.INSPECTION_SCHEDULING_MANNER + "=" + ScheduleManner);
            sbWork.Append("&" + ACAConstant.INSPECTION_RESCHEDULE_RESTRICTION_SETTINGS + "0=" + resceduleRestrictions[0]);
            sbWork.Append("&" + ACAConstant.INSPECTION_RESCHEDULE_RESTRICTION_SETTINGS + "1=" + resceduleRestrictions[1]);
            sbWork.Append("&" + ACAConstant.INSPECTION_RESCHEDULE_RESTRICTION_SETTINGS + "2=" + resceduleRestrictions[2]);
            sbWork.Append("&" + ACAConstant.INSPECTION_RESCHEDULE_RESTRICTION_SETTINGS + "3=" + resceduleRestrictions[3]);
            sbWork.Append("&" + ACAConstant.INSPECTION_CANCELLATION_RESTRICTION_SETTINGS + "0=" + cancelRestrictions[0]);
            sbWork.Append("&" + ACAConstant.INSPECTION_CANCELLATION_RESTRICTION_SETTINGS + "1=" + cancelRestrictions[1]);
            sbWork.Append("&" + ACAConstant.INSPECTION_CANCELLATION_RESTRICTION_SETTINGS + "2=" + cancelRestrictions[2]);
            sbWork.Append("&" + ACAConstant.INSPECTION_CANCELLATION_RESTRICTION_SETTINGS + "3=" + cancelRestrictions[3]);
            sbWork.Append("&InspUnits=" + InspUnits);
            sbWork.Append("&InspSeqNum=" + InspSeqNum);
            sbWork.Append("&InspType=" + InspType);
            sbWork.Append("&InspStatus=" + inspStatus);
            sbWork.Append("&InspectionId=" + InspectionId);
            sbWork.Append("&Action=" + Action);
            sbWork.Append("&cboYear=" + TheMonth);
            sbWork.Append("&cboDay=" + TheDay);
            sbWork.Append("&" + ACAConstant.INSPECTION_IS_READY_TIME_ENABLED + "=" + isReadyTimeEnabled);

            Breadcrumbs = BreadCrumbHelper("Inspections.ScheduleOneScreen.aspx", sbWork, iPhonePageTitle, breadCrumbIndex, isElipseLink, false, isBreadcrumbPagingMode, true);
            BackForwardLinks = BackLinkHelper(CurrentBreadCrumbIndex.ToString());

            // Save for next form
            HiddenFields.Append(HTML.PresentHiddenField("InspectionId", InspectionId));
            HiddenFields.Append(HTML.PresentHiddenField("PermitNo", PermitNo));
            HiddenFields.Append(HTML.PresentHiddenField("AltID", AltID));
            HiddenFields.Append(HTML.PresentHiddenField("PermitType", PermitType));
            HiddenFields.Append(HTML.PresentHiddenField("SearchBy", SearchBy));
            HiddenFields.Append(HTML.PresentHiddenField("SearchType", SearchType));
            HiddenFields.Append(HTML.PresentHiddenField("ViewPermitPageNo", ViewPermitPageNo));
            HiddenFields.Append(HTML.PresentHiddenField("InspectionsPageNo", InspectionsPageNo));
            HiddenFields.Append(HTML.PresentHiddenField("Mode", Mode));
            HiddenFields.Append(HTML.PresentHiddenField("Module", ModuleName));
            HiddenFields.Append(HTML.PresentHiddenField("RowNumber", rowNumber.ToString()));
            HiddenFields.Append(HTML.PresentHiddenField("TypeRowNumber", typeRowNumber));

            HiddenFields.Append(HTML.PresentHiddenField("InspSeqNum", InspSeqNum));
            HiddenFields.Append(HTML.PresentHiddenField("InspUnits", InspUnits));
            HiddenFields.Append(HTML.PresentHiddenField("TheMonth", TheMonth));
            HiddenFields.Append(HTML.PresentHiddenField(ACAConstant.INSPECTION_SCHEDULING_MANNER, ScheduleManner));
            HiddenFields.Append(HTML.PresentHiddenField("InspType", InspType));
            HiddenFields.Append(HTML.PresentHiddenField("InspStatus", inspStatus));
            HiddenFields.Append(HTML.PresentHiddenField(ACAConstant.INSPECTION_RESCHEDULE_RESTRICTION_SETTINGS, RescheduleRestrictionSettings));
            HiddenFields.Append(HTML.PresentHiddenField(ACAConstant.INSPECTION_CANCELLATION_RESTRICTION_SETTINGS, CancellationRestrictionSettings));
            HiddenFields.Append(HTML.PresentHiddenField("ScheduleOneScreenBreadcrumbIndex", CurrentBreadCrumbIndex.ToString()));
            HiddenFields.Append(HTML.PresentHiddenField(ACAConstant.INSPECTION_IS_READY_TIME_ENABLED, isReadyTimeEnabled));
            HiddenFields.Append(HTML.PresentHiddenField("isGoToNextPage", isGoToNextPage));
        }
        catch (Exception ex)
        {
            ErrorMessage.Append(ErrorFormat);
            ErrorMessage.Append(errorLocation + ": ");
            ErrorMessage.Append(ex.Message);
            ErrorMessage.Append(ErrorFormatEnd);
        }
        #endregion
    }
    #region DWB - 7.0 - Code copied from Cap/InspectionSchedule.aspx.cs

    /// <summary>
    /// set ui by scheduling
    /// </summary>
    /// <param name="inspectionParameter">the InspectionParameter object</param>
    ///  <returns></returns>
    private void SetUIBySchedulingManner(InspectionParameter inspectionParameter)
    {
        isRequestPending = false;
        isRequestSameDayNextDay = false;

        switch (inspectionParameter.ScheduleType)
        {
            case InspectionScheduleType.None:
            // this will not happen since scheduile none will not have a schedule, reschdule, or cancel link to click.
            case InspectionScheduleType.RequestOnlyPending:
                // for AMCA - skip to enter comment section
                // no dates to display
                isRequestPending = true;
                break;
            case InspectionScheduleType.RequestSameDayNextDay:
                isRequestSameDayNextDay = true;
                break;
            case InspectionScheduleType.ScheduleUsingCalendar:
            case InspectionScheduleType.Unknown:
            default:
                break;
        }
    }

    /// <summary>
    /// Get ACA Calendar Model
    /// </summary>
    /// <returns>a CalendarModel</returns>
    private CalendarModel GetcalendarModel()
    {
        if (_calendarModel == null)
        {
            //string inspeSeqNbr = Request.QueryString[ACAConstant.INSPECTION_SEQUENCE_NUBMER];
            ICalendarBll celendarBll = (ICalendarBll)ObjectFactory.GetObject(typeof(ICalendarBll));
            _calendarModel = celendarBll.GetACACalendarByInspType(CurrentAgency, InspSeqNum);
        }

        return _calendarModel;
    }

    /// <summary>
    /// Get same day next day selection tip.
    /// </summary>
    /// <returns>The tip format string.</returns>
    private string GetSameDayNextDaySelectionTip()
    {
        string calendarCutOffTime = String.Empty;
        CalendarModel calendar = this.GetcalendarModel();

        if (calendar != null)
        {
            calendarCutOffTime = I18nDateTimeUtil.ConvertTimeStringFromWebServiceToUI(calendar.calendarCutOffTime, true);
        }

        string tipPattern = LabelUtil.GetTextByKey("per_scheduleinspection_label_samedaynextdayselectiontip", ModuleName);
        string tip = String.Empty;

        if (!String.IsNullOrEmpty(calendarCutOffTime))
        {
            tip = string.Format(tipPattern, calendarCutOffTime);
        }

        return tip;
    }

    /// <summary>
    /// Get the text(label message) by label key.
    /// </summary>
    /// <param name="key">label key which is unique.</param>
    /// <returns>The text(label message) accroding the key.if can't find the key, return String.Empty.</returns>
    private string GetTextByKey(string key)
    {
        return LabelUtil.GetTextByKey(key, ModuleName);
    }

    #region Get Activity Date

    /// <summary>
    /// Get the activity date.
    /// </summary>
    /// <param name="timeOption">The time option.</param>
    /// <returns>Return the activity date.</returns>
    private Range<DateTime?> GetActivityDate(out InspectionTimeOption timeOption, InspectionParameter inspectionParameter)
    {
        Range<DateTime?> result = new Range<DateTime?>();
        timeOption = InspectionTimeOption.Unknow;

        switch (inspectionParameter.ScheduleType)
        {
            case InspectionScheduleType.RequestOnlyPending:
                timeOption = InspectionTimeOption.SpecificTime;
                result.LowerBound = isReadyTimeEnabled == ACAConstant.COMMON_Y ? GetReadyDateTime() : null;
                break;
            case InspectionScheduleType.RequestSameDayNextDay:
                timeOption = InspectionTimeOption.AllDay;
                result.LowerBound = GetRequestSameDayNextDaySelectedDate();
                break;
            case InspectionScheduleType.ScheduleUsingCalendar:
            case InspectionScheduleType.Unknown:
            default:
                timeOption = GetCalendarSelectedTimeOption();
                result = GetCalendarSelectedDateTime();
                break;
        }

        return result;
    }

    #region RequestOnlyPending readyTime

    /// <summary>
    /// Gets the ready date time.
    /// </summary>
    /// <returns>the ready date time.</returns>
    private DateTime? GetReadyDateTime()
    {
        DateTime? result = null;

        //check if agency time later than cut off time
        string selectedTimeString = GetReadyTimeString();

        DateTime agencyCurrentDate = CurrentAgencyAgencyDate;
        result = ParseTime(agencyCurrentDate, selectedTimeString);
        bool isLaterThanAgencyDate = false;

        if (result != null && result.Value > agencyCurrentDate)
        {
            isLaterThanAgencyDate = true;
        }

        //check if agency time later than cut off time
        bool isLaterThanCutOffTime = false;
        CalendarModel calendar = GetcalendarModel();

        if (calendar != null && !String.IsNullOrEmpty(calendar.calendarCutOffTime))
        {
            string calendarCutOffTime = I18nDateTimeUtil.FormatTimeStringForWebService(calendar.calendarCutOffTime, true);
            DateTime? cutOffDateTime = ParseTime(agencyCurrentDate, calendarCutOffTime);

            if (cutOffDateTime != null && cutOffDateTime.Value > agencyCurrentDate)
            {
                isLaterThanCutOffTime = true;
            }
        }

        if (isLaterThanAgencyDate || isLaterThanCutOffTime)
        {
            result = result.Value.AddDays(1);
        }

        return result;
    }

    /// <summary>
    /// Gets the ready time string.
    /// </summary>
    /// <returns>the time string.</returns>
    private string GetReadyTimeString()
    {
        string timePattern = @"{0}{1}{2} {3}";

        string timeString = String.Format(timePattern, MyProxy.GetFieldValue("cboHour", false), ACAConstant.TIME_SEPARATOR, MyProxy.GetFieldValue("cboMinute", false), MyProxy.GetFieldValue("cboAmPm", false));
        return timeString;
    }
    /// <summary>
    /// Parses the time.
    /// </summary>
    /// <param name="agencyDateTime">The agency date time.</param>
    /// <param name="timeString">The time string.</param>
    /// <returns>the parsed time</returns>
    private DateTime? ParseTime(DateTime agencyDateTime, string timeString)
    {
        DateTime? result = null;
        DateTime parsingResultDateTime = agencyDateTime;
        string dateString = I18nDateTimeUtil.FormatToDateStringForWebService(agencyDateTime);
        string dateTimeString = String.Format("{0} {1}", dateString, timeString);
        bool parsingResult = I18nDateTimeUtil.TryParseFromWebService(dateTimeString, out parsingResultDateTime);

        if (parsingResult)
        {
            result = parsingResultDateTime;
        }

        return result;
    }

    #endregion RequestOnlyPending readyTime

    #region RequestSameDayNextDay

    /// <summary>
    /// Gets the selected date.
    /// </summary>
    /// <returns>the selected date.</returns>
    private DateTime? GetRequestSameDayNextDaySelectedDate()
    {
        DateTime? result = null;
        string requestSameDayNextDayOptionValue = MyProxy.GetFieldValue("requestSameDayNextDayOption", false);

        if (!string.IsNullOrEmpty(requestSameDayNextDayOptionValue))
        {
            string[] vals = requestSameDayNextDayOptionValue.Split('|');
            if (vals.Length == 2)
            {
                DateTime preResultDateTime = DateTime.MinValue;
                I18nDateTimeUtil.TryParseFromWebService(vals[0], out preResultDateTime);

                if (preResultDateTime != DateTime.MinValue)
                {
                    result = preResultDateTime;
                }
            }
        }

        return result;
    }

    /// <summary>
    /// Gets the request day option.
    /// </summary>
    /// <returns>the request day option.</returns>
    private InspectionRequestDayOption GetRequestSameDayNextDayOption()
    {
        var result = InspectionRequestDayOption.SameDay;

        string requestSameDayNextDayOptionValue = MyProxy.GetFieldValue("requestSameDayNextDayOption", false);

        if (!string.IsNullOrEmpty(requestSameDayNextDayOptionValue))
        {
            string[] vals = requestSameDayNextDayOptionValue.Split('|');
            if (vals.Length == 2)
            {
                result = EnumUtil<InspectionRequestDayOption>.Parse(vals[1], InspectionRequestDayOption.SameDay);
            }
        }

        return result;
    }

    #endregion RequestSameDayNextDay

    #region Schedule Using Calendar

    /// <summary>
    /// Gets the selected time string.
    /// </summary>
    /// <returns>the selected time string.</returns>
    private InspectionTimeOption GetCalendarSelectedTimeOption()
    {
        var result = InspectionTimeOption.Unknow;

        string calendarValue = MyProxy.GetFieldValue("cboScheduleUsingCalendarTime", false);

        if (calendarValue != null)
        {
            string[] calendarVals = calendarValue.Split('|');

            if (calendarVals.Length == 3)
            {
                result = EnumUtil<InspectionTimeOption>.Parse(calendarVals[2], InspectionTimeOption.Unknow);
            }
        }

        return result;
    }

    /// <summary>
    /// Gets the selected date time.
    /// </summary>
    /// <returns>the selected date time.</returns>
    private Range<DateTime?> GetCalendarSelectedDateTime()
    {
        Range<DateTime?> result = new Range<DateTime?>();

        string calendarValue = MyProxy.GetFieldValue("cboScheduleUsingCalendarTime", false);

        if (calendarValue != null)
        {
            string[] calendarVals = calendarValue.Split('|');

            if (calendarVals.Length == 3)
            {
                result.LowerBound = I18nDateTimeUtil.ParseFromWebService(calendarVals[0]);
                result.UpperBound = I18nDateTimeUtil.ParseFromWebService(calendarVals[1]);
            }
        }

        return result;
    }

    #endregion Schedule Using Calendar

    #endregion Get Activity Date

    #region RequestSameDayNextDay

    /// <summary>
    /// initiate same day next day selection.
    /// </summary>
    /// <param name="inspectionTypeModel">Type of the inspection.</param>
    /// <param name="inspectionParameter">the InspectionParameter object</param>
    /// <param name="recordIDModel">the CapIDModel object</param>
    private void InitSameDayNextDaySelection(InspectionTypeModel inspectionTypeModel, InspectionParameter inspectionParameter, CapIDModel recordIDModel)
    {
        string radioIdHTML = "<input type=\"radio\" id=\"{0}\" name=\"requestSameDayNextDayOption\" value=\"{1}|{4}\" {2}/><label for=\"{0}\"><b>{3}</b></label><br/>";

        string isChecked = "checked";
        InspectionRequestDayOption lastRequestDayOption = GetRequestSameDayNextDayOption();

        //set same day option
        DateTime? sameDate = GetSameDate(recordIDModel, inspectionTypeModel);
        bool isSameDayAvailable = IsDayAvailable(sameDate, inspectionParameter, recordIDModel);

        if (isSameDayAvailable)
        {
            TimeList.AppendFormat(radioIdHTML, "requestSameDayNextDayOption1", I18nDateTimeUtil.FormatToDateStringForWebService(sameDate), isChecked, "Same Day", InspectionRequestDayOption.SameDay);
            isChecked = string.Empty;
        }
        else
        {
            TimeList.AppendFormat(radioIdHTML, "requestSameDayNextDayOption1", I18nDateTimeUtil.FormatToDateStringForWebService(sameDate), "disabled", "Same Day", InspectionRequestDayOption.SameDay);
        }

        //set next business day option
        DateTime? nextBusinessDate = GetNextBusinessDate(recordIDModel, inspectionTypeModel);
        bool isNextBusinessDayAvailable = IsDayAvailable(nextBusinessDate, inspectionParameter, recordIDModel);

        if (isNextBusinessDayAvailable)
        {
            TimeList.AppendFormat(radioIdHTML, "requestSameDayNextDayOption2", I18nDateTimeUtil.FormatToDateStringForWebService(nextBusinessDate), isChecked, "Next Business Day", InspectionRequestDayOption.NextBusinessDay);
            isChecked = string.Empty;
        }
        else
        {
            TimeList.AppendFormat(radioIdHTML, "requestSameDayNextDayOption2", I18nDateTimeUtil.FormatToDateStringForWebService(nextBusinessDate), "disabled", "Next Business Day", InspectionRequestDayOption.NextBusinessDay);
        }

        // next available day
        TimeList.AppendFormat(radioIdHTML, "requestSameDayNextDayOption3", "", isChecked, "Next Available Day", InspectionRequestDayOption.NextAvailableDay);
    }

    /// <summary>
    /// Gets the same date.
    /// </summary>
    /// <param name="recordIDModel">The record ID.</param>
    /// <param name="inspectionTypeModel">Type of the inspection.</param>
    /// <returns>the same date.</returns>
    private DateTime? GetSameDate(CapIDModel recordIDModel, InspectionTypeModel inspectionTypeModel)
    {
        DateTime? result = null;
        var calendarBll = ObjectFactory.GetObject<ICalendarBll>();

        var dateTimeRangeDisplayModel = calendarBll.GetSameDay(CurrentAgency, recordIDModel, inspectionTypeModel);

        if (dateTimeRangeDisplayModel != null && dateTimeRangeDisplayModel.times != null)
        {
            var validTimePeriods = from t in dateTimeRangeDisplayModel.times
                                   where t != null
                                   && t.active
                                   select t;

            if (validTimePeriods.Count() > 0)
            {
                result = dateTimeRangeDisplayModel.date;
            }
        }

        return result;
    }

    /// <summary>
    /// Gets the next business date.
    /// </summary>
    /// <param name="recordIDModel">The record ID.</param>
    /// <param name="inspectionTypeModel">Type of the inspection.</param>
    /// <returns>the next business date.</returns>
    private DateTime? GetNextBusinessDate(CapIDModel recordIDModel, InspectionTypeModel inspectionTypeModel)
    {
        DateTime? result = null;

        var calendarBll = ObjectFactory.GetObject<ICalendarBll>();
        var dateTimeRangeDisplayModel = calendarBll.GetNextBusinessDay(CurrentAgency, recordIDModel, inspectionTypeModel);

        if (dateTimeRangeDisplayModel != null && dateTimeRangeDisplayModel.times != null)
        {
            var validTimePeriods = from t in dateTimeRangeDisplayModel.times
                                   where t != null
                                   && t.active
                                   select t;

            if (validTimePeriods.Count() > 0)
            {
                result = dateTimeRangeDisplayModel.date;
            }
        }

        return result;
    }

    /// <summary>
    /// Determines whether [is same day available] [the specified same date].
    /// </summary>
    /// <param name="theDate">The same/next date.</param>
    /// <param name="inspectionParameter">the InspectionParameter object</param>
    /// <param name="recordIDModel">the CapIDModel object</param>
    /// <returns>
    /// <c>true</c> if [is same day available] [the specified same date]; otherwise, <c>false</c>.
    /// </returns>
    private bool IsDayAvailable(DateTime? theDate, InspectionParameter inspectionParameter, CapIDModel recordIDModel)
    {
        bool result = false;

        if (theDate != null)
        {
            AvailableTimeResultModel availableTimeResultModel = MyProxy.GetAvailableTimeResult(theDate.Value.Date, inspectionParameter, recordIDModel);

            if (availableTimeResultModel != null && availableTimeResultModel.flag == ACAConstant.INSPECTION_FLAG_SUCCESS && theDate.Value == availableTimeResultModel.scheduleDate)
            {
                result = true;
            }
        }

        return result;
    }

    #endregion RequestSameDayNextDay

    #region schedule using calendar

    /// <summary>
    /// show time periods
    /// </summary>
    /// <param name="selectedDate">the schedule date</param>
    /// <param name="inspectionParameter">the InspectionParameter object</param>
    /// <param name="recordIDModel">the CapIDModel object</param>
    private void ShowTimePeriods(DateTime selectedDate, InspectionParameter inspectionParameter, CapIDModel recordIDModel)
    {
        InspectionParameter tempInspectionParameter = new InspectionParameter();
        tempInspectionParameter.AgencyCode = inspectionParameter.AgencyCode;
        tempInspectionParameter.Action = inspectionParameter.Action;
        tempInspectionParameter.Type = inspectionParameter.Type;
        tempInspectionParameter.InAdvance = inspectionParameter.InAdvance;
        tempInspectionParameter.ID = inspectionParameter.ID;
        tempInspectionParameter.TypeID = inspectionParameter.TypeID;
        tempInspectionParameter.Required = inspectionParameter.Required;
        tempInspectionParameter.ScheduleType = inspectionParameter.ScheduleType;
        tempInspectionParameter.ReadyTimeEnabled = inspectionParameter.ReadyTimeEnabled;
        tempInspectionParameter.TimeOption = InspectionTimeOption.Unknow;

        AvailableTimeResultModel availableTimeResultModel = MyProxy.GetAvailableTimeResult(selectedDate, tempInspectionParameter, recordIDModel);

        if (availableTimeResultModel != null && availableTimeResultModel.flag == ACAConstant.INSPECTION_FLAG_SUCCESS && selectedDate == availableTimeResultModel.scheduleDate)
        {
            HasEnoughUnits = true;
        }
        else
        {
            HasEnoughUnits = false;
        }

        SelectedWorkingDaysAndTime = WorkingDaysAndTimes == null ? null : Array.Find<DateTimeRangePageModel>(WorkingDaysAndTimes, p => p.date == selectedDate);

        if (SelectedWorkingDaysAndTime != null)
        {
            var isNormalTimePeriods = IsNormalTimePeriods(SelectedWorkingDaysAndTime.dateTimeRangeType);

            TimeList.Append("<div id=\"pageSectionTitle\">");
            TimeList.Append("<label>Time: </label>");
            TimeList.Append("</div>");
            TimeList.Append("<div id=\"pageText\">");

            StringBuilder stringBuilderOption = new StringBuilder();

            if (isNormalTimePeriods)
            {
                ShowNormalTimePeriods(SelectedWorkingDaysAndTime, stringBuilderOption);
            }
            else
            {
                ShowAllDayOrAmPmOrExceptionTimePeriods(SelectedWorkingDaysAndTime, stringBuilderOption);
            }

            if (stringBuilderOption.Length > 0)
            {
                TimeList.Append("<select class=\"pageTextInput100\" id=\"cboScheduleUsingCalendarTime\" name=\"cboScheduleUsingCalendarTime\">");
                TimeList.Append(stringBuilderOption);
                TimeList.Append(" </select>");
            }
            else
            {
                TimeList.Append("<i>No available times were found for this inspection type.</i>");
            }

            TimeList.Append("</div>");
        }
    }

    /// <summary>
    /// Shows all day or am pm or exception time periods.
    /// </summary>
    /// <param name="dateAndTimesModel">The date and times model.</param>
    /// <param name="stringBuilderOption">the select time option stringbuilder</param>
    private void ShowAllDayOrAmPmOrExceptionTimePeriods(DateTimeRangePageModel dateAndTimesModel, StringBuilder stringBuilderOption)
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

            foreach (var t in timePeriods)
            {
                var isRadioButtonEnabled = HasEnoughUnits;
                string strOption = CreateTimeOption(dateAndTimesModel.dateTimeRangeType, t.startDate.Value, t.endDate.Value, t.amOrPM, hideTimePeriod, isRadioButtonEnabled);

                stringBuilderOption.Append(strOption);
            }
        }
    }

    /// <summary>
    /// Shows the normal time periods.
    /// </summary>
    /// <param name="dateAndTimesModel">The date and times model.</param>
    /// <param name="stringBuilderOption">the select time option stringbuilder</param>
    private void ShowNormalTimePeriods(DateTimeRangePageModel dateAndTimesModel, StringBuilder stringBuilderOption)
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
                var timePeriodList = g.timePeriods.ToList();
                BuildNormalTimePeriodLayout(dateAndTimesModel.dateTimeRangeType, hideTimePeriod, timePeriodList, stringBuilderOption);
            }
        }
    }

    /// <summary>
    /// Builds the normal time period layout.
    /// </summary>
    /// <param name="rangeType">Type of the range.</param>
    /// <param name="hideTimePeriod">if set to <c>true</c> [hide time period].</param>
    /// <param name="calendarTimePeriods">The calendar time periods.</param>
    /// <param name="stringBuilderOption">the select time option stringbuilder</param>
    private void BuildNormalTimePeriodLayout(string rangeType, bool hideTimePeriod, List<CalendarTimePeriod> calendarTimePeriods, StringBuilder stringBuilderOption)
    {
        if (calendarTimePeriods != null)
        {
            for (int i = 0; i < calendarTimePeriods.Count; i++)
            {
                var t = calendarTimePeriods[i];
                var isRadioButtonEnabled = HasEnoughUnits;
                string strOption = CreateTimeOption(rangeType, t.startDate.Value, t.endDate.Value, t.amOrPM, hideTimePeriod, isRadioButtonEnabled);
                stringBuilderOption.Append(strOption);
            }
        }
    }

    /// <summary>
    /// Creates the radio button control.
    /// </summary>
    /// <param name="rangeType">Type of the range.</param>
    /// <param name="startTime">The start time.</param>
    /// <param name="endTime">The end time.</param>
    /// <param name="amOrPm">The am or pm.</param>
    /// <param name="hideTime">if set to <c>true</c> [hide time].</param>
    /// <param name="enabled">if set to <c>true</c> [enabled].</param>
    /// <returns>the radio button control.</returns>
    private string CreateTimeOption(string rangeType, DateTime startTime, DateTime endTime, string amOrPm, bool hideTime, bool enabled)
    {
        var timeOption = ConverToInspectionTimeOption(rangeType, amOrPm);
        var isToSaveSpecificTime = timeOption == InspectionTimeOption.SpecificTime || timeOption == InspectionTimeOption.Unknow;

        string selectTimeOption = string.Empty;

        if (enabled)
        {
            var startDateValue = isToSaveSpecificTime ? startTime : startTime.Date;
            var endDateValue = isToSaveSpecificTime ? endTime : endTime.Date;

            string optionStartDateValue = I18nDateTimeUtil.FormatToDateTimeStringForWebService(startDateValue);
            string optionEndDateValue = I18nDateTimeUtil.FormatToDateTimeStringForWebService(endDateValue);
            string optionText = BuildTimePeriodText(timeOption, startTime, endTime, amOrPm, hideTime);

            string selectAttr = string.Empty;

            if ((isToSaveSpecificTime && scheduledDateTime != null && scheduledDateTime.Value == startTime)
                || (!isToSaveSpecificTime && scheduledDateTime != null && scheduledDateTime.Value.Date == startTime.Date && finishTimeOption == timeOption))
            {
                selectAttr = "selected=\"selected\"";
            }

            selectTimeOption = string.Format("<option value=\"{0}|{1}|{2}\" {3}>{4}</option>", optionStartDateValue, optionEndDateValue, timeOption.ToString(), selectAttr, optionText);
        }

        return selectTimeOption;
    }

    /// <summary>
    /// Builds the time period text.
    /// </summary>
    /// <param name="inspectionTimeOption">Type of the time.</param>
    /// <param name="startTime">The start time.</param>
    /// <param name="endTime">The end time.</param>
    /// <param name="amOrPM">The am or PM.</param>
    /// <param name="hideTime">if set to <c>true</c> [hide time].</param>
    /// <returns>the time period text.</returns>
    private string BuildTimePeriodText(InspectionTimeOption inspectionTimeOption, DateTime startTime, DateTime endTime, string amOrPM, bool hideTime)
    {
        string result = String.Empty;
        string timeBlockString = String.Format("{0} - {1}", I18nDateTimeUtil.FormatToTimeStringForUI(startTime, true), I18nDateTimeUtil.FormatToTimeStringForUI(endTime, true));
        string preText = String.Empty;

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
            case InspectionTimeOption.SpecificTime:
            default:
                preText = String.Empty;
                break;
        }

        if (hideTime)
        {
            result = !String.IsNullOrEmpty(preText) ? preText : timeBlockString;
        }
        else
        {
            result = !String.IsNullOrEmpty(preText) ? String.Format("{0} ({1})", preText, timeBlockString) : timeBlockString;
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
    /// <c>true</c> if [is normal time periods] [the specified range type]; otherwise, <c>false</c>.
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
    /// <c>true</c> if [has any active time] [the specified date and times model]; otherwise, <c>false</c>.
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
    /// 1. the start time is not available if return avaliable time is before start time.
    /// 2. the start time is not available if start time is AM or PM and return avaliable time is not same as start time.
    /// </summary>
    /// <param name="activityDate"></param>
    /// <param name="startAvailableTime4Compare"></param>
    /// <param name="availableStartAMPM"></param>
    /// <param name="timeOption"></param>
    /// <returns></returns>
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

    #endregion schedule using calendar

    #endregion
}
