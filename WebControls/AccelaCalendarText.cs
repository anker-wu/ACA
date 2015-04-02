#region Header

/*
 * <pre>
 *
 *  Accela Citizen Access
 *  File: AccelaCalendarText.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2007-2014
 *
 *  Description:
 *
 *  Notes:
 * $Id: AccelaCalendarText.cs 278471 2014-09-04 08:53:54Z ACHIEVO\james.shi $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using System;
using System.Threading;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

using Accela.ACA.Common;
using Accela.ACA.Common.Util;
using Accela.Web.Controls.ControlRender;

using AjaxControlToolkit;

[assembly: WebResource("Accela.Web.Controls.Calendar.HijriDate.js", "text/javascript")]

namespace Accela.Web.Controls
{
    /// <summary>
    /// provide a date box to input date
    /// </summary>
    public class AccelaCalendarText : AccelaMaskText
    {
        #region Fields

        /// <summary>
        /// The UAE of Arab culture
        /// </summary>
        private const string AR_CULTURE = "ar-AE";

        /// <summary>
        /// Is show image or not
        /// </summary>
        private bool _showImage = true;

        #endregion Fields

        #region Properties

        /// <summary>
        /// Gets mask value
        /// </summary>
        public override string Mask
        {
            get
            {
                return I18nDateTimeUtil.ShortDateMask;
            }
        }

        /// <summary>
        /// Gets MaskType
        /// </summary>
        public override MaskedEditType MaskType
        {
            get
            {
                return MaskedEditType.Date;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether to auto complete
        /// </summary>
        public override bool AutoComplete
        {
            get
            {
                return true;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether show image or not
        /// false: Default calendar
        /// true: Calendar with an associated button
        /// </summary>
        public bool ShowImage
        {
            get
            {
                return _showImage;
            }

            set
            {
                _showImage = value;
            }
        }

        /// <summary>
        /// Gets or sets the date time object
        /// </summary>
        public object Text2
        {
            get
            {
                if (string.IsNullOrEmpty(this.Text))
                {
                    this.ViewState["Text2"] = string.Empty;
                }

                return this.ViewState["Text2"] as object;
            }

            set
            {
                this.ViewState["Text2"] = value;
                SetText(value);
            }
        }

        /// <summary>
        /// Gets or sets DateType
        /// </summary>
        public DateType DateType
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is hijri date.
        /// </summary>
        /// <value><c>true</c> if this instance is hijri date; otherwise, <c>false</c>.</value>
        public bool IsHijriDate
        {
            get;
            set;
        }

        /// <summary>
        ///  Gets the current text
        /// </summary>
        public override string Text
        {
            get
            {
                return base.Text;
            }

            set
            {
                if (IsHijriDate && IsCalendarExtenderCreated)
                {
                    IsCalendarExtenderCreated = false;
                    value = HijriDateUtil.ToGregorianDate(value);
                }

                base.Text = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is calendar extender created.
        /// </summary>
        /// <value><c>true</c> if this instance is calendar extender created; otherwise, <c>false</c>.</value>
        private bool IsCalendarExtenderCreated
        {
            get
            {
                if (ViewState["IsCalendarExtenderCreated"] == null)
                {
                    return false;
                }

                return (bool)ViewState["IsCalendarExtenderCreated"];
            }

            set
            {
                ViewState["IsCalendarExtenderCreated"] = value;
            }
        }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Compares this AccelaCalendarText's DateTime to a given object.  Null is considered less than any instance.
        /// </summary>
        /// <param name="accelaCalendarText">The AccelaCalendarText object.</param>
        /// <returns>Compares the value of this instance to a specified value and 
        /// returns an boolean that indicates whether this instance is later than the specified value.</returns>
        public bool IsLaterThan(AccelaCalendarText accelaCalendarText)
        {
            return this.CompareTo(accelaCalendarText) > 0;
        }

        /// <summary>
        /// clear mask on lost focus
        /// </summary>
        /// <returns>return true if it need to clear,otherwise return false</returns>
        protected override bool ClearMaskOnLostFocus()
        {
            return true;
        }

        /// <summary>
        /// Get invalid value message
        /// </summary>
        /// <returns>invalid message</returns>
        protected override string GetInvalidValueMessage()
        {
            return string.Format(LabelConvertUtil.GetTextByKey("ACA_AccelaCalendarText_InvalidValueMessage", GetModuleName()), I18nDateTimeUtil.ShortDatePattern);
        }

        /// <summary>
        /// override MaskedValidator
        /// </summary>
        /// <returns>MaskedEditValidator object</returns>
        protected override Tuple<MaskedEditExtender, MaskedEditValidator> MaskedValidator()
        {
            var result = base.MaskedValidator();

            var maskedVad = result.Item2;
            maskedVad.IsHijriDate = IsHijriDate;

            string minimumValueString = IsHijriDate ? I18nDateTimeUtil.FormatToDateStringForUI(new DateTime(1168, 3, 17)) : I18nDateTimeUtil.FormatToDateStringForUI(new DateTime(1755, 1, 1));
            maskedVad.MinimumValue = minimumValueString;
            maskedVad.MinimumValueMessage = string.Format(LabelConvertUtil.GetTextByKey("ACA_AccelaCalendarText_MinimumValueMessage", GetModuleName()), FormatMinimunValue(minimumValueString));

            if (IsHijriDate)
            {
                string maximumValue = I18nDateTimeUtil.FormatToDateStringForUI(new DateTime(9666, 4, 2));
                maskedVad.MaximumValue = maximumValue;
                maskedVad.MaximumValueMessage = LabelConvertUtil.GetTextByKey("aca_accelacalendartext_msg_invaliddate", GetModuleName());
            }

            return result;
        }

        /// <summary>
        /// Raises the System.Web.UI.Control.PreRender event.
        /// </summary>
        /// <param name="e">A System.EventArgs that contains the event data.</param>
        protected override void OnPreRender(EventArgs e)
        {
            if (this.Text2 != null && string.IsNullOrEmpty(this.Text))
            {
                SetText(this.Text2);
            }
            
            CalendarExtender();
            base.OnPreRender(e);
            ScriptManager.RegisterClientScriptResource(Page, typeof(CalendarExtender), "Accela.Web.Controls.Calendar.HijriDate.js");
        }

        /// <summary>
        /// Set the Text value
        /// </summary>
        /// <param name="text2">date type value</param>
        private void SetText(object text2)
        {
            if (text2 == null)
            {
                this.Text = string.Empty;
                return;
            }

            DateTime date = DateTime.MinValue;
            bool isDateTime = false;

            if (text2 is DateTime)
            {
                isDateTime = true;
                date = (DateTime)text2;
            }
            else
            {
                isDateTime = I18nDateTimeUtil.TryParseFromWebService(text2.ToString(), out date);
            }

            if (isDateTime)
            {
                switch (this.DateType)
                {
                    case DateType.LongDate:
                        this.Text = I18nDateTimeUtil.FormatToLongDateStringForUI(date);
                        break;
                    case DateType.DateAndTime:
                        this.Text = I18nDateTimeUtil.FormatToDateTimeStringForUI(date);
                        break;
                    case DateType.ShortDate:
                    default:
                        this.Text = I18nDateTimeUtil.FormatToDateStringForUI(date);
                        break;
                }
            }
        }

        /// <summary>
        /// extender for calendar
        /// </summary>
        private void CalendarExtender()
        {
            CalendarExtender calendarExd = new CalendarExtender();

            if (IsHijriDate)
            {
                Attributes.Add("DateMask", Mask);
                var extenderTargetControl = new HtmlInputHidden { ID = string.Concat(ClientID, ACAConstant.CLIENT_STATE) };
                extenderTargetControl.Value = ACAConstant.ISLAMIC_CALENDAR;
                Controls.Add(extenderTargetControl);
                IsCalendarExtenderCreated = true;
            }

            calendarExd.TargetControlID = ID;
            calendarExd.IsHijriCalendar = IsHijriDate;
            calendarExd.BehaviorID = ClientID + "_calendar_bhv";
            calendarExd.EnabledOnClient = !ReadOnly && !ControlRenderUtil.IsAdminRender(this);
            calendarExd.Format = I18nDateTimeUtil.ShortDatePattern;
            calendarExd.FirstDayOfWeek = (FirstDayOfWeek)Enum.Parse(typeof(FirstDayOfWeek), I18nCultureUtil.UserPreferredCultureInfo.DateTimeFormat.FirstDayOfWeek.ToString());
            calendarExd.PrevArrowTitle = LabelConvertUtil.GetTextByKey("aca_calendar_label_prevarrowtitle|tip", GetModuleName());
            calendarExd.NextArrowTitle = LabelConvertUtil.GetTextByKey("aca_calendar_label_nextarrowtitle|tip", GetModuleName());
            calendarExd.TitleOnLastFocus = LabelConvertUtil.GetTextByKey("aca_calendar_label_titleonlastfocus|tip", GetModuleName());
            calendarExd.DaysCaption = LabelConvertUtil.GetTextByKey("aca_caption_control_calendar", GetModuleName());
            calendarExd.DaysSummary = LabelConvertUtil.GetTextByKey("aca_summary_control_calendar", GetModuleName());
            calendarExd.FormBeginText = LabelConvertUtil.GetTextByKey("img_alt_form_begin", GetModuleName());
            calendarExd.FormEndText = LabelConvertUtil.GetTextByKey("img_alt_form_end", GetModuleName());
            calendarExd.ImagePath = Page.ResolveUrl("~/Customize/images/");

            // if calendar with button
            if (ShowImage)
            {
                HtmlAnchor anchor = new HtmlAnchor();
                anchor.ID = ID + "_calendar_button";
                anchor.Title = LabelConvertUtil.GetTextByKey("ACA_AccelaCalendarText_CalendarAlt", GetModuleName());

                HtmlImage imgCalendar = new HtmlImage();
                imgCalendar.Src = "~/App_Themes/Default/assets/" + (IsHijriDate ? "Calendar_hijri.png" : "Calendar_scheduleHS.png");
                imgCalendar.Alt = anchor.Title;
                anchor.Controls.Add(imgCalendar);

                if (calendarExd.EnabledOnClient)
                {
                    // Use onclick to prevent the attribute Href to be fired by clicking.
                    anchor.Attributes.Add("onclick", "return false;");

                    anchor.Attributes.Add("href", "#");
                    anchor.Attributes.Add("class", "calendar_button NotShowLoading");

                    if (IsAlwaysEditable)
                    {
                        //Add Always Editable attribute for client js.
                        anchor.Attributes.Add("data-editable", "true");
                    }
                }
                else
                {
                    anchor.Disabled = true;
                    anchor.Attributes.Add("class", "calendar_button NotShowLoading ButtonDisabled");
                }

                Controls.Add(new LiteralControl(" "));
                Controls.Add(anchor);

                calendarExd.PopupButtonID = anchor.ID;
            }

            Controls.Add(calendarExd);
        }

        /// <summary>
        ///  Compares this AccelaCalendarText's DateTime to a given object.  Null is considered less than any instance. 
        /// </summary>
        /// <param name="accelaCalendarText">The AccelaCalendarText object.</param>
        /// <returns>
        /// Compares the value of this instance to a specified value and 
        /// returns an integer that indicates whether this instance is earlier than, the same as, or later than the specified value.
        /// </returns>
        private int CompareTo(AccelaCalendarText accelaCalendarText)
        {
            if (accelaCalendarText == null)
            {
                return 1;
            }

            string strDateTime1 = this.Text;
            string strDateTime2 = accelaCalendarText.Text;

            if (!string.IsNullOrEmpty(strDateTime1) && !string.IsNullOrEmpty(strDateTime2))
            {
                DateTime dt1 = I18nDateTimeUtil.ParseFromUI(strDateTime1);
                DateTime dt2 = I18nDateTimeUtil.ParseFromUI(strDateTime2);     

                return DateTime.Compare(dt1, dt2);
            }

            return 0;
        }

        /// <summary>
        /// If the culture is Arab, we should add some tag around datetime,otherwise it maybe not format correctly
        /// </summary>
        /// <param name="minimumValueString">minimum value</param>
        /// <returns>format minimum value</returns>
        private string FormatMinimunValue(string minimumValueString)
        {
            if (Thread.CurrentThread.CurrentCulture.Name != AR_CULTURE)
            {
                return minimumValueString;
            }
            else
            {
                return string.Format("<span dir=\"ltr\">{0}</span>", minimumValueString);
            }
        }

        #endregion Methods
    }
}