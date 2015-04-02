/**
 * <pre>
 * 
 *  Accela Citizen Access
 *  File: InspectionReadyTime.ascx.cs
 * 
 *  Accela, Inc.
 *  Copyright (C): 2010-2014
 * 
 *  Description:
 * 
 *  Notes:
 *      $Id: InspectionReadyTime.ascx.cs 277741 2014-08-20 10:31:02Z ACHIEVO\james.shi $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

using System;

using Accela.ACA.BLL.Common;
using Accela.ACA.BLL.Inspection;
using Accela.ACA.Common;
using Accela.ACA.Common.Log;
using Accela.ACA.Common.Util;
using Accela.ACA.Web.Common;
using Accela.ACA.WSProxy;

namespace Accela.ACA.Web.Component
{
    /// <summary>
    /// inspection Ready Time
    /// </summary>
    public partial class InspectionReadyTime : BaseUserControl
    {
        #region Fields

        /// <summary>
        /// log4net Logger
        /// </summary>
        private static readonly log4net.ILog Logger = LogFactory.Instance.GetLogger(typeof(InspectionReadyTime));
        
        #endregion Fields

        #region Properties

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
        /// Gets or sets the last selected ready time.
        /// </summary>
        /// <value>The last selected ready time.</value>
        public DateTime? LastSelectedReadyTime
        {
            get
            {
                return ViewState["LastSelectedReadyTime"] as DateTime?;
            }

            set
            {
                ViewState["LastSelectedReadyTime"] = value;
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
        /// Gets the ready date time.
        /// </summary>
        /// <returns>the ready date time.</returns>
        public DateTime? GetReadyDateTime()
        {
            DateTime? result = null;

            //check if agency time later than cut off time
            string selectedTimeString = readyTimeSelect.GetTimeString();
            ITimeZoneBll timeBll = (ITimeZoneBll)ObjectFactory.GetObject(typeof(ITimeZoneBll));
            DateTime agencyCurrentDate = timeBll.GetAgencyCurrentDate(AgencyCode);
            result = ParseTime(agencyCurrentDate, selectedTimeString);
            bool isLaterThanAgencyDate = false;

            if (result != null && result.Value > agencyCurrentDate)
            {
                isLaterThanAgencyDate = true;
            }

            //check if agency time later than cut off time
            bool isLaterThanCutOffTime = false;
            CalendarModel calendar = this.GetACACalendarModel();

            if (calendar != null && !string.IsNullOrEmpty(calendar.calendarCutOffTime))
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
                        readyTimeSelect.IsUsing12HourFormat = I18nDateTimeUtil.IsUsing12HourFormat(true);
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
                string tip = GetScheduleCutOffTip();

                if (string.IsNullOrEmpty(tip))
                {
                    lblReadyTimeTip.Visible = false;
                }
                else
                {
                    lblReadyTimeTip.Text = tip;
                }
            }

            readyTimeSelect.SubLabelVisible = false;

            if (LastSelectedReadyTime != null)
            {
                readyTimeSelect.Time = LastSelectedReadyTime.Value;
            }
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
            string dateTimeString = string.Format("{0} {1}", dateString, timeString);
            bool parsingResult = I18nDateTimeUtil.TryParseFromWebService(dateTimeString, out parsingResultDateTime);

            if (parsingResult)
            {
                result = parsingResultDateTime;
            }

            return result;
        }

        /// <summary>
        /// Gets the schedule cut off tip.
        /// </summary>
        /// <returns>the schedule cut off tip.</returns>
        private string GetScheduleCutOffTip()
        {
            string calendarCutOffTime = string.Empty;
            CalendarModel calendar = GetACACalendarModel();

            if (calendar != null)
            {
                calendarCutOffTime = I18nDateTimeUtil.ConvertTimeStringFromWebServiceToUI(calendar.calendarCutOffTime, true);
            }

            string tipPattern = GetTextByKey("per_scheduleinspection_label_samedaynextdayselectiontip");
            string tip = string.Empty;

            if (!string.IsNullOrEmpty(calendarCutOffTime))
            {
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

        #endregion Methods
    }
}