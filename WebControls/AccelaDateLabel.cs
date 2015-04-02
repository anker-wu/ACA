#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: AccelaDateLabel.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2009-2014
 *
 *  Description:
 *
 *  Notes:
 * $Id: AccelaDateLabel.cs 131439 2009-05-19 11:07:26Z ACHIEVO\jackie.yu $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using Accela.ACA.Common.Util;

namespace Accela.Web.Controls
{
    /// <summary>
    /// The date type
    /// </summary>
    public enum DateType
    {
        /// <summary>
        /// short date format
        /// </summary>
        ShortDate = 0,

        /// <summary>
        /// long date format
        /// </summary>
        LongDate = 1,

        /// <summary>
        /// date and time format
        /// </summary>
        DateAndTime = 2,

        /// <summary>
        /// only display time
        /// </summary>
        OnlyTime = 3,
    }

    /// <summary>
    /// The label to access DateTime data
    /// </summary>
    public sealed class AccelaDateLabel : Label
    {
        #region Public Properties

        /// <summary>
        /// Gets or sets the date time object
        /// </summary>
        public object Text2
        {
            get
            {
                return this.ViewState["Text2"] as object;
            }

            set
            {
                this.ViewState["Text2"] = value;
            }
        }

        /// <summary>
        /// Gets or sets the date time object
        /// </summary>
        public override string Text
        {
            get 
            {
                return this.GetText();
            }

            set
            {
                base.Text = value;
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
        /// Gets or sets a value indicating whether this instance is inspection time.
        /// </summary>
        /// <value><c>true</c> if this instance is inspection time; otherwise, <c>false</c>.</value>
        public bool IsInspectionTime
        {
            get; 
            set; 
        }

        #endregion

        #region Protected Methods

        /// <summary>
        /// Gets the text.
        /// </summary>
        /// <returns>the formatted text</returns>
        private string GetText()
        {
            if (this.Text2 == null || !string.IsNullOrEmpty(base.Text))
            {
                return base.Text;
            }

            string result = string.Empty;
            DateTime date = DateTime.MinValue;
            bool isDateTime = false;

            if (this.Text2 is DateTime)
            {
                isDateTime = true;
                date = (DateTime)this.Text2;
            }
            else
            {
                isDateTime = I18nDateTimeUtil.TryParseFromWebService(this.Text2.ToString(), out date);
            }

            if (isDateTime)
            {
                switch (this.DateType)
                {
                    case DateType.LongDate:
                        result = I18nDateTimeUtil.FormatToLongDateStringForUI(date);
                        break;
                    case DateType.DateAndTime:
                        result = I18nDateTimeUtil.FormatToDateTimeStringForUI(date);
                        break;
                    case DateType.OnlyTime:
                        result = I18nDateTimeUtil.FormatToTimeStringForUI(date, IsInspectionTime);
                        break;
                    case DateType.ShortDate:
                    default:
                        result = I18nDateTimeUtil.FormatToDateStringForUI(date);
                        break;
                }
            }

            return result;
        }

        #endregion
    }
}
