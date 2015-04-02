#region Header

/**
 * <pre>
 *
 *  Accela
 *  File: CalendarExtender.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2008-2014
 *
 *  Description:
 *
 *  Notes:
 * $Id: CalendarExtender.cs 278471 2014-09-04 08:53:54Z ACHIEVO\james.shi $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */
//// (c) Copyright Microsoft Corporation.
//// This source is subject to the Microsoft Permissive License.
//// See http://www.microsoft.com/resources/sharedsource/licensingbasics/sharedsourcelicenses.mspx.
//// All other rights reserved.

#endregion Header

using System.ComponentModel;
using System.Globalization;
using System.Web.UI;
using System.Web.UI.WebControls;

[assembly: System.Web.UI.WebResource("Accela.Web.Controls.Calendar.CalendarBehavior.js", "text/javascript")]
[assembly: System.Web.UI.WebResource("Accela.Web.Controls.Calendar.Calendar.css", "text/css", PerformSubstitution = true)]
[assembly: System.Web.UI.WebResource("Accela.Web.Controls.Calendar.arrow-left.gif", "image/gif")]
[assembly: System.Web.UI.WebResource("Accela.Web.Controls.Calendar.arrow-right.gif", "image/gif")]

namespace AjaxControlToolkit
{
    /// <summary>
    /// Extender for calendar
    /// </summary>
    [Designer("AjaxControlToolkit.CalendarDesigner, AjaxControlToolkit")]
    [RequiredScript(typeof(CommonToolkitScripts), 0)]
    [RequiredScript(typeof(DateTimeScripts), 1)]
    [RequiredScript(typeof(PopupExtender), 2)]
    [RequiredScript(typeof(AnimationScripts), 3)]
    [RequiredScript(typeof(ThreadingScripts), 4)]
    [TargetControlType(typeof(TextBox))]
    [ClientScriptResource("AjaxControlToolkit.CalendarBehavior", "Accela.Web.Controls.Calendar.CalendarBehavior.js")]
    public class CalendarExtender : ExtenderControlBase
    {
        #region Properties

        /// <summary>
        /// Gets or sets a value indicating whether this instance is RTL.
        /// </summary>
        /// <value><c>true</c> if this instance is RTL; otherwise, <c>false</c>.</value>
        [ExtenderControlProperty]
        [ClientPropertyName("isRTL")]
        public virtual bool IsRTL
        {
            get
            {
                bool isRTL = CultureInfo.CurrentCulture.TextInfo.IsRightToLeft;
                return GetPropertyValue("IsRTL", isRTL);
            }

            set
            {
                SetPropertyValue("IsRTL", value);
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether animated is true
        /// </summary>
        [DefaultValue(true)]
        [ExtenderControlProperty]
        [ClientPropertyName("animated")]
        public virtual bool Animated
        {
            get
            {
                return GetPropertyValue("Animated", true);
            }

            set
            {
                SetPropertyValue("Animated", value);
            }
        }

        /// <summary>
        /// Gets or sets CSS class
        /// </summary>
        [DefaultValue("")]
        [ExtenderControlProperty]
        [ClientPropertyName("cssClass")]
        public virtual string CssClass
        {
            get
            {
                return GetPropertyValue("CssClass", string.Empty);
            }

            set
            {
                SetPropertyValue("CssClass", value);
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether client is enabled
        /// </summary>
        [DefaultValue(true)]
        [ExtenderControlProperty]
        [ClientPropertyName("enabled")]
        public virtual bool EnabledOnClient
        {
            get
            {
                return GetPropertyValue("EnabledOnClient", true);
            }

            set
            {
                SetPropertyValue("EnabledOnClient", value);
            }
        }

        /// <summary>
        /// Gets or sets first day of week
        /// </summary>
        [DefaultValue(FirstDayOfWeek.Default)]
        [ExtenderControlProperty]
        [ClientPropertyName("firstDayOfWeek")]
        public virtual FirstDayOfWeek FirstDayOfWeek
        {
            get
            {
                return GetPropertyValue("FirstDayOfWeek", FirstDayOfWeek.Default);
            }

            set
            {
                SetPropertyValue("FirstDayOfWeek", value);
            }
        }

        /// <summary>
        /// Gets or sets format string
        /// </summary>
        [DefaultValue("d")]
        [ExtenderControlProperty]
        [ClientPropertyName("format")]
        public virtual string Format
        {
            get
            {
                return GetPropertyValue("Format", "d");
            }

            set
            {
                SetPropertyValue("Format", value);
            }
        }

        /// <summary>
        /// Gets or sets OnClientDateSelectionChanged value
        /// </summary>
        [DefaultValue("")]
        [ExtenderControlEvent]
        [ClientPropertyName("dateSelectionChanged")]
        public virtual string OnClientDateSelectionChanged
        {
            get
            {
                return GetPropertyValue("OnClientDateSelectionChanged", string.Empty);
            }

            set
            {
                SetPropertyValue("OnClientDateSelectionChanged", value);
            }
        }

        /// <summary>
        /// Gets or sets OnClientHidden value
        /// </summary>
        [DefaultValue("")]
        [ExtenderControlEvent]
        [ClientPropertyName("hidden")]
        public virtual string OnClientHidden
        {
            get
            {
                return GetPropertyValue("OnClientHidden", string.Empty);
            }

            set
            {
                SetPropertyValue("OnClientHidden", value);
            }
        }

        /// <summary>
        /// Gets or sets OnClientHiding value
        /// </summary>
        [DefaultValue("")]
        [ExtenderControlEvent]
        [ClientPropertyName("hiding")]
        public virtual string OnClientHiding
        {
            get
            {
                return GetPropertyValue("OnClientHiding", string.Empty);
            }

            set
            {
                SetPropertyValue("OnClientHiding", value);
            }
        }

        /// <summary>
        /// Gets or sets OnClientShowing value
        /// </summary>
        [DefaultValue("")]
        [ExtenderControlEvent]
        [ClientPropertyName("showing")]
        public virtual string OnClientShowing
        {
            get
            {
                return GetPropertyValue("OnClientShowing", string.Empty);
            }

            set
            {
                SetPropertyValue("OnClientShowing", value);
            }
        }

        /// <summary>
        /// Gets or sets OnClientShown value
        /// </summary>
        [DefaultValue("")]
        [ExtenderControlEvent]
        [ClientPropertyName("shown")]
        public virtual string OnClientShown
        {
            get
            {
                return GetPropertyValue("OnClientShown", string.Empty);
            }

            set
            {
                SetPropertyValue("OnClientShown", value);
            }
        }

        /// <summary>
        /// Gets or sets PopupButtonID value
        /// </summary>
        [DefaultValue("")]
        [ExtenderControlProperty]
        [ClientPropertyName("button")]
        [ElementReference]
        [IDReferenceProperty]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1706:ShortAcronymsShouldBeUppercase", Justification = "Following ASP.NET AJAX pattern")]
        public virtual string PopupButtonID
        {
            get
            {
                return GetPropertyValue("PopupButtonID", string.Empty);
            }

            set
            {
                SetPropertyValue("PopupButtonID", value);
            }
        }

        /// <summary>
        /// Gets or sets the title of the Previous arrow in popup dialog.
        /// </summary>
        [DefaultValue("")]
        [ExtenderControlProperty]
        [ClientPropertyName("prevArrowTitle")]
        public string PrevArrowTitle
        {
            get
            {
                return GetPropertyValue("PrevArrowTitle", string.Empty);
            }

            set
            {
                SetPropertyValue("PrevArrowTitle", value);
            }
        }

        /// <summary>
        /// Gets or sets the title of the Next arrow in popup dialog.
        /// </summary>
        [DefaultValue("")]
        [ExtenderControlProperty]
        [ClientPropertyName("nextArrowTitle")]
        public string NextArrowTitle
        {
            get
            {
                return GetPropertyValue("NextArrowTitle", string.Empty);
            }

            set
            {
                SetPropertyValue("NextArrowTitle", value);
            }
        }

        /// <summary>
        /// Gets or sets the title on the last focused object in popup dialog.
        /// </summary>
        [DefaultValue("")]
        [ExtenderControlProperty]
        [ClientPropertyName("titleOnLastFocus")]
        public string TitleOnLastFocus
        {
            get
            {
                return GetPropertyValue("TitleOnLastFocus", string.Empty);
            }

            set
            {
                SetPropertyValue("TitleOnLastFocus", value);
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the title on the last focused object in popup dialog.
        /// </summary>
        [DefaultValue(false)]
        [ExtenderControlProperty]
        [ClientPropertyName("isHijriCalendar")]
        public bool IsHijriCalendar
        {
            get
            {
                return GetPropertyValue("IsHijriCalendar", false);
            }

            set
            {
                SetPropertyValue("IsHijriCalendar", value);
            }
        }

        /// <summary>
        /// Gets or sets the caption of days in popup dialog.
        /// </summary>
        [DefaultValue("")]
        [ExtenderControlProperty]
        [ClientPropertyName("daysCaption")]
        public string DaysCaption
        {
            get
            {
                return GetPropertyValue("DaysCaption", string.Empty);
            }

            set
            {
                SetPropertyValue("DaysCaption", value);
            }
        }

        /// <summary>
        /// Gets or sets the summary of days in popup dialog.
        /// </summary>
        [DefaultValue("")]
        [ExtenderControlProperty]
        [ClientPropertyName("daysSummary")]
        public string DaysSummary
        {
            get
            {
                return GetPropertyValue("DaysSummary", string.Empty);
            }

            set
            {
                SetPropertyValue("DaysSummary", value);
            }
        }

        /// <summary>
        /// Gets or sets the title of the Next arrow in popup dialog.
        /// </summary>
        [DefaultValue("")]
        [ExtenderControlProperty]
        [ClientPropertyName("formBeginText")]
        public string FormBeginText
        {
            get
            {
                return GetPropertyValue("FormBeginText", string.Empty);
            }

            set
            {
                SetPropertyValue("FormBeginText", value);
            }
        }

        /// <summary>
        /// Gets or sets the title of the Next arrow in popup dialog.
        /// </summary>
        [DefaultValue("")]
        [ExtenderControlProperty]
        [ClientPropertyName("formEndText")]
        public string FormEndText
        {
            get
            {
                return GetPropertyValue("FormEndText", string.Empty);
            }

            set
            {
                SetPropertyValue("FormEndText", value);
            }
        }

        /// <summary>
        /// Gets or sets the path of images.
        /// </summary>
        [DefaultValue("")]
        [ExtenderControlProperty]
        [ClientPropertyName("imagePath")]
        public string ImagePath
        {
            get
            {
                return GetPropertyValue("ImagePath", string.Empty);
            }

            set
            {
                SetPropertyValue("ImagePath", value);
            }
        }

        #endregion Properties
    }
}