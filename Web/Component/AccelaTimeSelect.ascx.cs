/**
 * <pre>
 * 
 *  Accela Citizen Access
 *  File: AccelaTimeSelect.ascx.cs
 * 
 *  Accela, Inc.
 *  Copyright (C): 2010-2014
 * 
 *  Description:
 * 
 *  Notes:
 *      $Id: AccelaTimeSelect.ascx.cs 278076 2014-08-27 06:22:20Z ACHIEVO\james.shi $.
 *  Revision History
 *  &lt;Date&gt;,       &lt;Who&gt;,        &lt;What&gt;
 * </pre>
 */

using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;

using Accela.ACA.Common;
using Accela.ACA.Common.Util;
using Accela.ACA.Web.Common;

namespace Accela.ACA.Web.Component
{
    /// <summary>
    /// Accela time select control
    /// </summary>
    public partial class AccelaTimeSelect : BaseUserControl
    {
        /// <summary>
        /// the key of AM
        /// </summary>
        private const string AM_KEY = "am";

        /// <summary>
        /// the key of PM
        /// </summary>
        private const string PM_KEY = "pm";

        /// <summary>
        /// default hour format pattern
        /// </summary>
        private const string DEFAULT_HOUR_FORMAT_PATTERN = "00";

        /// <summary>
        /// default minute format pattern
        /// </summary>
        private const string DEFAULT_MINUTE_FORMAT_PATTERN = "00";

        /// <summary>
        /// sub-label key suffix
        /// </summary>
        private const string SUBLABEL_KEY_SUFFIX = "|sub";

        /// <summary>
        /// initial time
        /// </summary>
        private DateTime? _initialTime = null;

        /// <summary>
        /// Gets or sets the label key.
        /// </summary>
        /// <value>The label key.</value>
        public string LabelKey
        {
            get
            {
                if (ViewState[this.ID + "AccelaTimeSelectLabelKey"] == null)
                {
                    return string.Empty;
                }

                return (string)ViewState[this.ID + "AccelaTimeSelectLabelKey"];
            }

            set
            {
                ViewState[this.ID + "AccelaTimeSelectLabelKey"] = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether sub label is visible.
        /// </summary>
        /// <value><c>true</c> if sub label is visible; otherwise, <c>false</c>.</value>
        public bool SubLabelVisible
        {
            get
            {
                if (ViewState[this.ID + "SubLabelVisible"] == null)
                {
                    return true;
                }

                return (bool)ViewState[this.ID + "SubLabelVisible"];
            }

            set
            {
                ViewState[this.ID + "SubLabelVisible"] = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether current time is using 12-hour format.
        /// use 12-hour format by default.
        /// </summary>
        /// <value>
        /// <c>true</c> if current time is using 12-hour format; otherwise, <c>false</c>.
        /// </value>
        public bool IsUsing12HourFormat
        {
            get
            {
                if (ViewState[this.ID + "IsUsing12HourFormat"] == null)
                {
                    return true;
                }

                return (bool)ViewState[this.ID + "IsUsing12HourFormat"];
            }

            set
            {
                ViewState[this.ID + "IsUsing12HourFormat"] = value;
            }
        }

        /// <summary>
        /// Gets or sets the time object.
        /// </summary>
        /// <value>
        /// The time object.
        /// </value>
        public DateTime Time
        {
            get
            {
                DateTime? selectedTime = GetSelectedTime();
                DateTime result = selectedTime == null ? _initialTime == null ? DateTime.Now : _initialTime.Value : selectedTime.Value;
                return result;
            }

            set
            {
                _initialTime = value;
            }
        }

        /// <summary>
        /// Gets the time string.
        /// </summary>
        /// <returns>the time string.</returns>
        public string GetTimeString()
        {
            string timePattern = @"{0}{1}{2} {3}";
            string timeString = string.Format(timePattern, ddlHours.SelectedValue, ACAConstant.TIME_SEPARATOR, ddlMinutes.SelectedValue, ddlAmPm.Visible ? ddlAmPm.SelectedValue : string.Empty);
            return timeString;
        }

        /// <summary>
        /// Handles the Load event of the Page control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                this.InitTimeSelect();
                lblTimeSubTitle.Visible = this.SubLabelVisible;
            }

            if (_initialTime != null)
            {
                SetSelectedTime(_initialTime.Value);
            }
        }

        /// <summary>
        /// Initializes the time select control
        /// </summary>
        private void InitTimeSelect()
        {
            InitLabel();
            InitHoursDDL();
            InitTimeSeparator();
            InitMinutesDDL();

            if (IsUsing12HourFormat)
            {
                InitTimeAmPm();
            }
            else
            {
                lblAmPm.Visible = false;
                ddlAmPm.Visible = false;
            }

            string helpMessage = GetTextByKey("aca_dorpdown_help_message");
            ddlHours.ToolTip = string.Concat(GetTextByKey("aca_timeselect_msg_selecthour_tip"), ACAConstant.BLANK, helpMessage);
            ddlMinutes.ToolTip = string.Concat(GetTextByKey("aca_timeselect_msg_selectminute_tip"), ACAConstant.BLANK, helpMessage);
            ddlAmPm.ToolTip = string.Concat(GetTextByKey("aca_timeselect_msg_selectampm_tip"), ACAConstant.BLANK, helpMessage);
        }

        /// <summary>
        /// Initializes the label.
        /// </summary>
        private void InitLabel()
        {
            lblTimeTitle.LabelKey = this.LabelKey;
            lblTimeSubTitle.LabelKey = string.Concat(this.LabelKey, SUBLABEL_KEY_SUFFIX);
        }

        /// <summary>
        /// Initializes the hours DDL.
        /// </summary>
        private void InitHoursDDL()
        {
            IList<ListItem> resultList = new List<ListItem>();
            int theMinHour = IsUsing12HourFormat ? 1 : 0;
            int theMaxHour = IsUsing12HourFormat ? 12 : 23;

            for (int i = theMinHour; i <= theMaxHour; i++)
            {
                ListItem tempItemValue = new ListItem(i.ToString(DEFAULT_HOUR_FORMAT_PATTERN), i.ToString());
                resultList.Add(tempItemValue);
            }

            DropDownListBindUtil.BindDDL(resultList, ddlHours, false);
        }

        /// <summary>
        /// Initializes the time separator.
        /// </summary>
        private void InitTimeSeparator()
        {
            lblTimeSeparator.Text = I18nCultureUtil.UserPreferredCultureInfo.DateTimeFormat.TimeSeparator;
        }

        /// <summary>
        /// Initializes the minutes DDL.
        /// </summary>
        private void InitMinutesDDL()
        {
            IList<ListItem> resultList = new List<ListItem>();

            for (int i = 0; i <= 59; i++)
            {
                ListItem tempItemValue = new ListItem(i.ToString(DEFAULT_MINUTE_FORMAT_PATTERN), i.ToString());
                resultList.Add(tempItemValue);
            }

            DropDownListBindUtil.BindDDL(resultList, ddlMinutes, false);
        }

        /// <summary>
        /// Initializes the time am/pm.
        /// </summary>
        private void InitTimeAmPm()
        {
            IList<ListItem> resultList = new List<ListItem>();

            ListItem tempItemValue = new ListItem(GetTextByKey("common_am_label"), AM_KEY);
            resultList.Add(tempItemValue);
            tempItemValue = new ListItem(GetTextByKey("common_pm_label"), PM_KEY);
            resultList.Add(tempItemValue);

            DropDownListBindUtil.BindDDL(resultList, ddlAmPm, false, false);
        }

        /// <summary>
        /// Sets the selected time.
        /// </summary>
        /// <param name="datetime">The datetime.</param>
        private void SetSelectedTime(DateTime datetime)
        {
            if (datetime != DateTime.MinValue)
            {
                int currentHour = datetime.Hour;

                if (IsUsing12HourFormat)
                {
                    if (currentHour > 12)
                    {
                        currentHour = currentHour - 12;
                    }
                    else if (currentHour == 0)
                    {
                        currentHour = 12;
                    }

                    ddlAmPm.SelectedValue = datetime.Hour < 12 ? AM_KEY : PM_KEY;
                }
                
                ddlHours.SelectedValue = currentHour.ToString();
                ddlMinutes.SelectedValue = datetime.Minute.ToString();
            }
        }

        /// <summary>
        /// Gets the selected time.
        /// </summary>
        /// <returns>the selected time.</returns>
        private DateTime? GetSelectedTime()
        {
            DateTime? result = null;

            if (ddlHours.Items.Count > 0 && ddlMinutes.Items.Count > 0)
            {
                int selectedHour = int.Parse(ddlHours.SelectedValue);
                int selectedMinute = int.Parse(ddlMinutes.SelectedValue);

                if (IsUsing12HourFormat)
                {
                    bool isAM = ddlAmPm.SelectedValue.Equals(AM_KEY);

                    if (isAM && selectedHour == 12)
                    {
                        selectedHour = 0;
                    }
                    else if (!isAM && selectedHour < 12)
                    {
                        selectedHour = selectedHour + 12;
                    }
                }

                result = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, selectedHour, selectedMinute, 0);
            }

            return result;
        }
    }
}
