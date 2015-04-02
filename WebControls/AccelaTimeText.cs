#region Header

/**
 * <pre>
 *
 *  Accela
 *  File: AccelaTimeText.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2007-2014
 *
 *  Description:
 *
 *  Notes:
 * $Id: AccelaTimeText.cs 276801 2014-08-07 02:14:39Z ACHIEVO\james.shi $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using System;
using System.Web.UI;

using Accela.ACA.Common;
using Accela.ACA.Common.Util;
using AjaxControlToolkit;

namespace Accela.Web.Controls
{
    /// <summary>
    /// provide a time box to input time
    /// </summary>
    public class AccelaTimeText : AccelaMaskText
    {
        #region Fields

        /// <summary>
        /// The time text time mask.
        /// </summary>
        private string _timeMask = I18nDateTimeUtil.ShortTimeMask;

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the time mask
        /// </summary>
        public override string Mask
        {
            get
            {
                //return "99:99:99";
                return _timeMask;
            }

            set
            {
                _timeMask = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether validate time format or not.
        /// </summary>
        public bool IsIgnoreValidate
        { 
            get;
            set;
        }

        /// <summary>
        /// Gets or sets a value indicating whether time format am/pm or not.
        /// </summary>
        public bool IsUsedForAMPM
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the max value.
        /// </summary>
        public string MaxValue
        {
            get;
            set;
        }

         /// <summary>
        /// Gets or sets the min value.
        /// </summary>
        public string MinValue
        {
            get;
            set;
        }        

        /// <summary>
        /// Gets Time Mask Type
        /// </summary>
        public override AjaxControlToolkit.MaskedEditType MaskType
        {
            get
            {
                return AjaxControlToolkit.MaskedEditType.Time;
            }
        }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Whether Clear Mask when Lost Focus.
        /// </summary>
        /// <returns>return true if it need to clear,otherwise return false</returns>
        protected override bool ClearMaskOnLostFocus()
        {
            return true;
        }

        /// <summary>
        /// Validate the illegal character in the text-box field.
        /// </summary>
        /// <returns>MaskedEditValidator object</returns>
        protected override Tuple<MaskedEditExtender, MaskedEditValidator> MaskedValidator()
        {
            var result = base.MaskedValidator();
            MaskedEditValidator maskedVad = result.Item2;
            maskedVad.Enabled = !IsIgnoreValidate;
            maskedVad.MaximumValue = MaxValue;
            maskedVad.MinimumValue = MinValue;

            if (IsUsedForAMPM)
            {
                //H means 12 hours time format
                maskedVad.InvalidValueMessage = I18nDateTimeUtil.ShortTimePattern.IndexOf("H") == -1 ? LabelConvertUtil.GetTextByKey("aca_time_12HourErrMsg_label", this.GetModuleName()).Replace("'", "\\'") : LabelConvertUtil.GetTextByKey("aca_time_24HourErrMsg_label", this.GetModuleName()).Replace("'", "\\'");
            }

            return result;
        }

        /// <summary>
        /// Render html text
        /// </summary>
        /// <param name="w">html text writer</param>
        protected override void Render(HtmlTextWriter w)
        {
            if (!IsIgnoreValidate)
            {
                if (!string.IsNullOrEmpty(this.Text))
                {
                    if (!IsUsedForAMPM && this.Text.Trim().Length < 5)
                    {
                        DateTime resultDateTime;
                        bool parseResult = I18nDateTimeUtil.TryParseFromUI(this.Text.Trim(), out resultDateTime);

                        if (parseResult)
                        {
                            this.Text = I18nDateTimeUtil.FormatToTimeStringForWebService(resultDateTime);
                        }
                        else
                        {
                            this.Text = string.Empty;
                        }
                    }
                }
            }

            base.Render(w);
        }

        /// <summary>
        /// Get invalid value message
        /// </summary>
        /// <returns>invalid value message</returns>
        protected override string GetInvalidValueMessage()
        {
            //return "Enter as hh:mm:ss";
            return LabelConvertUtil.GetTextByKey("ACA_AccelaTimeText_InvalidTimeFormat", GetModuleName());
        }

        #endregion Methods
    }
}
