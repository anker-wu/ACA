#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: InspectionSameDayNextDayInput.ascx.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2011-2014
 *
 *  Description: 
 *
 *  Notes:
 *      $Id: InspectionSameDayNextDayInput.ascx.cs 277741 2014-08-20 10:31:02Z ACHIEVO\james.shi $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;

using Accela.ACA.BLL.Inspection;
using Accela.ACA.Common;
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
    /// inspection same day/next day input page
    /// </summary>
    public partial class InspectionSameDayNextDayInput : BaseUserControl
    {
        #region Fields

        /// <summary>
        /// log4net Logger
        /// </summary>
        private static readonly log4net.ILog Logger = LogFactory.Instance.GetLogger(typeof(InspectionSameDayNextDayInput));

        #endregion Fields

        #region Properties

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
        /// Gets or sets the inspection type units.
        /// </summary>
        /// <value>The inspection type units.</value>
        public double InspectionTypeUnits
        {
            get
            {
                return (double)ViewState["InspectionTypeUnits"];
            }

            set
            {
                ViewState["InspectionTypeUnits"] = value;
            }
        }

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
        /// Gets or sets the last request day option.
        /// </summary>
        /// <value>The last request day option.</value>
        public InspectionRequestDayOption LastRequestDayOption
        {
            get
            {
                return (InspectionRequestDayOption)ViewState["LastRequestDayOption"];
            }

            set
            {
                ViewState["LastRequestDayOption"] = value;
            }
        }

        /// <summary>
        /// Gets or sets the same date.
        /// </summary>
        /// <value>The same date.</value>
        private DateTime? SameDate
        {
            get
            {
                return ViewState["SameDate"] as DateTime?;
            }

            set
            {
                ViewState["SameDate"] = value;
            }
        }

        /// <summary>
        /// Gets or sets the next business date.
        /// </summary>
        /// <value>The next business date.</value>
        private DateTime? NextBusinessDate
        {
            get
            {
                return ViewState["NextBusinessDate"] as DateTime?;
            }

            set
            {
                ViewState["NextBusinessDate"] = value;
            }
        }

        /// <summary>
        /// Gets current Agency Code
        /// </summary>
        /// <value>The current agency.</value>
        private string AgencyCode
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

        #endregion Properties

        #region Methods

        /// <summary>
        /// Gets the selected date.
        /// </summary>
        /// <returns>the selected date.</returns>
        public DateTime? GetSelectedDate()
        {
            DateTime? result = null;

            if (rblSameDay.Checked && rblSameDay.Enabled)
            {
                result = SameDate;
            }
            else if (rblNextBusinessDay.Checked && rblNextBusinessDay.Enabled)
            {
                result = NextBusinessDate;
            }
            else if (rblNextAvailableDay.Checked && rblNextAvailableDay.Enabled)
            {
                result = null;
            }

            return result;
        }

        /// <summary>
        /// Gets the request day option.
        /// </summary>
        /// <returns>the request day option.</returns>
        public InspectionRequestDayOption GetRequestDayOption()
        {
            var result = InspectionRequestDayOption.SameDay;

            if (rblSameDay.Checked && rblSameDay.Enabled)
            {
                result = InspectionRequestDayOption.SameDay;
            }
            else if (rblNextBusinessDay.Checked && rblNextBusinessDay.Enabled)
            {
                result = InspectionRequestDayOption.NextBusinessDay;
            }
            else if (rblNextAvailableDay.Checked && rblNextAvailableDay.Enabled)
            {
                result = InspectionRequestDayOption.NextAvailableDay;
            }

            return result;
        }

        /// <summary>
        /// Validate inspection date time and get the available time result
        /// </summary>
        /// <param name="selectedDate">the selected date</param>
        /// <param name="isBlockedWhenNoInspectorFound">IsBlockedWhenNoInspectorFound Flag</param>
        /// <returns>get the available result.</returns>
        public AvailableTimeResultModel GetAvailableTimeResult(DateTime selectedDate, bool isBlockedWhenNoInspectorFound = true)
        {
            InspectionParameter inspectionParameter = new InspectionParameter();
            inspectionParameter.AgencyCode = AgencyCode;
            inspectionParameter.Action = Action;
            inspectionParameter.Type = InspectionType;
            inspectionParameter.InAdvance = InAdvance;
            inspectionParameter.ID = InspectionID;
            inspectionParameter.TypeID = InspectionTypeID;
            inspectionParameter.Required = Required;
            inspectionParameter.ReadyTimeEnabled = ReadyTimeEnabled;
            inspectionParameter.ScheduleType = ScheduleType;
            inspectionParameter.TimeOption = InspectionTimeOption.AllDay;

            return InspectionViewUtil.GetAvailableTimeResult(selectedDate, RecordIDModel, inspectionParameter, isBlockedWhenNoInspectorFound);
        }

        /// <summary>
        /// Handles the Load event of the Page control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (this.Visible)
            {
                try
                {
                    if (!Page.IsPostBack)
                    {
                        InitUI();
                    }
                }
                catch (ACAException ex)
                {
                    Logger.Error(ex.Message, ex);
                    MessageUtil.ShowMessage(Page, MessageType.Error, ex.Message);
                }
            }
        }

        /// <summary>
        /// Initializes the UI.
        /// </summary>
        private void InitUI()
        {
            if (!AppSession.IsAdmin)
            {
                var acaCalendarModel = GetACACalendarModel();
                InitSameDayNextDaySelection(acaCalendarModel);
                string tip = GetScheduleCutOffTip(acaCalendarModel);
                divSameDayNextDayTip.Visible = string.IsNullOrEmpty(tip) ? false : true;
                lblSameDayNextDayTip.Text = string.IsNullOrEmpty(tip) ? string.Empty : tip;

                // set the UI when back to this page
                switch (LastRequestDayOption)
                {
                    case InspectionRequestDayOption.SameDay:
                        rblSameDay.Checked = true;
                        break;
                    case InspectionRequestDayOption.NextBusinessDay:
                        rblNextBusinessDay.Checked = true;
                        break;
                    case InspectionRequestDayOption.NextAvailableDay:
                        rblNextAvailableDay.Checked = true;
                        break;
                    default:
                        rblSameDay.Checked = true;
                        break;
                }
            }
            else
            {
                rblSameDay.Checked = true;
                rblSameDay.Enabled = true;
                rblNextBusinessDay.Enabled = true;
                rblNextAvailableDay.Enabled = true;
            }
        }

        /// <summary>
        /// initiate same day next day selection.
        /// </summary>
        /// <param name="acaCalendarModel">The aca calendar model.</param>
        private void InitSameDayNextDaySelection(CalendarModel acaCalendarModel)
        {
            bool hasSelectedItem = false;

            //set same day option
            SameDate = GetSameDate();
            bool isSameDayAvailable = IsDayAvailable(SameDate, acaCalendarModel, InspectionTypeUnits);
            rblSameDay.Enabled = isSameDayAvailable;

            if (!hasSelectedItem && isSameDayAvailable)
            {
                hasSelectedItem = true;
                rblSameDay.Checked = true;
            }

            //set next business day option
            NextBusinessDate = GetNextBusinessDate();
            bool isNextBusinessDayAvailable = IsDayAvailable(NextBusinessDate, acaCalendarModel, InspectionTypeUnits);
            rblNextBusinessDay.Enabled = isNextBusinessDayAvailable;

            if (!hasSelectedItem && isNextBusinessDayAvailable)
            {
                hasSelectedItem = true;
                rblNextBusinessDay.Checked = true;
            }

            //set next available day option
            rblNextAvailableDay.Enabled = true;

            if (!hasSelectedItem)
            {
                hasSelectedItem = true;
                rblNextAvailableDay.Checked = true;
            }
        }

        /// <summary>
        /// Gets the same date.
        /// </summary>
        /// <returns>the same date.</returns>
        private DateTime? GetSameDate()
        {
            DateTime? result = null;
            var calendarBll = ObjectFactory.GetObject<ICalendarBll>();
            var inspectionTypeModel = GetBasicInspectionTypeModel();
            var dateTimeRangeDisplayModel = calendarBll.GetSameDay(AgencyCode, RecordIDModel, inspectionTypeModel);

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
        /// <returns>the next business date.</returns>
        private DateTime? GetNextBusinessDate()
        {
            DateTime? result = null;

            var calendarBll = ObjectFactory.GetObject<ICalendarBll>();
            var inspectionTypeModel = GetBasicInspectionTypeModel();
            var dateTimeRangeDisplayModel = calendarBll.GetNextBusinessDay(AgencyCode, RecordIDModel, inspectionTypeModel);

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
        /// <param name="calendar">The calendar.</param>
        /// <param name="inspectionTypeUnits">The inspection type units.</param>
        /// <returns>
        /// <c>true</c> if [is same day available] [the specified same date]; otherwise, <c>false</c>.
        /// </returns>
        private bool IsDayAvailable(DateTime? theDate, CalendarModel calendar, double inspectionTypeUnits)
        {
            bool result = false;

            if (theDate != null && calendar != null && calendar.calendarID != null)
            {
                bool isBlockedWhenNoInspectorFound = StandardChoiceUtil.IsBlockedWhenNoInspectorFound(ConfigManager.AgencyCode);
                AvailableTimeResultModel availableTimeResultModel = GetAvailableTimeResult(theDate.Value.Date, isBlockedWhenNoInspectorFound);

                if (availableTimeResultModel != null && availableTimeResultModel.flag == ACAConstant.INSPECTION_FLAG_SUCCESS && theDate.Value == availableTimeResultModel.scheduleDate)
                {
                    result = true;
                }
            }

            return result;
        }

        /// <summary>
        /// Gets the schedule cut off tip.
        /// </summary>
        /// <param name="acaCalendarModel">The aca calendar model.</param>
        /// <returns>the schedule cut off tip.</returns>
        private string GetScheduleCutOffTip(CalendarModel acaCalendarModel)
        {
            string tip = string.Empty;

            string calendarCutOffTime = string.Empty;

            if (acaCalendarModel != null)
            {
                calendarCutOffTime = I18nDateTimeUtil.ConvertTimeStringFromWebServiceToUI(acaCalendarModel.calendarCutOffTime, true);
            }

            if (!string.IsNullOrEmpty(calendarCutOffTime))
            {
                string tipPattern = GetTextByKey("per_scheduleinspection_label_samedaynextdayselectiontip");
                tip = string.Format(tipPattern, calendarCutOffTime);
            }

            return tip;
        }

        /// <summary>
        /// Gets the ACA calendar model.
        /// </summary>
        /// <returns>the ACA calendar model.</returns>
        private CalendarModel GetACACalendarModel()
        {
            CalendarModel result = null;

            if (!string.IsNullOrEmpty(InspectionTypeID))
            {
                ICalendarBll celendarBll = (ICalendarBll)ObjectFactory.GetObject(typeof(ICalendarBll));
                result = celendarBll.GetACACalendarByInspType(AgencyCode, InspectionTypeID);
            }

            return result;
        }

        /// <summary>
        /// Gets the basic inspection type model.
        /// </summary>
        /// <returns>the basic inspection type model.</returns>
        private InspectionTypeModel GetBasicInspectionTypeModel()
        {
            var result = new InspectionTypeModel();
            result.sequenceNumber = long.Parse(InspectionTypeID);
            result.groupCode = InspectionGroup;
            result.type = InspectionType;

            return result;
        }

        #endregion Methods
    }
}