#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: AjaxControlToolkit.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2008-2014
 *
 *  Description:
 *
 *  Notes:
 * $Id: MaskedEditExtender.cs 276801 2014-08-07 02:14:39Z ACHIEVO\james.shi $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */
//// (c) Copyright Microsoft Corporation.
//// This source is subject to the Microsoft Permissive License.
//// See http://www.microsoft.com/resources/sharedsource/licensingbasics/sharedsourcelicenses.mspx.
//// All other rights reserved.
//// Product      : MaskedEdit Extender
//// Version      : 1.0.0.0
//// Date         : 11/08/2006
//// Development  : Fernando Cerqueira

#endregion Header

using System;
using System.ComponentModel;
using System.Globalization;
using System.Web.UI;
using System.Web.UI.WebControls;

using Accela.ACA.Common.Util;

[assembly: WebResource("Accela.Web.Controls.MaskedEdit.MaskedEditBehavior.js", "text/javascript")]
[assembly: WebResource("Accela.Web.Controls.MaskedEdit.MaskedEditValidator.js", "text/javascript")]

namespace AjaxControlToolkit
{
    /// <summary>
    /// Extender for masked edit control
    /// </summary>
    [Designer("AjaxControlToolkit.MaskedEditDesigner, AjaxControlToolkit")]
    [ClientScriptResource("AjaxControlToolkit.MaskedEditBehavior", "Accela.Web.Controls.MaskedEdit.MaskedEditValidator.js")]
    [ClientScriptResource("AjaxControlToolkit.MaskedEditBehavior", "Accela.Web.Controls.MaskedEdit.MaskedEditBehavior.js")]
    [TargetControlType(typeof(TextBox))]
    public class MaskedEditExtender : ExtenderControlBase
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the MaskedEditExtender class.the Enable client state for communicating default focus
        /// </summary>
        public MaskedEditExtender()
        {
            EnableClientState = true;
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// Gets or sets a value indicating whether AMPM is accepted.
        /// </summary>
        [DefaultValue(false)]
        [ExtenderControlProperty]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1705:LongAcronymsShouldBePascalCased", MessageId = "Member", Justification = "Alternative of AmPm violates another rule")]
        public bool AcceptAMPM
        {
            get
            {
                return GetPropertyValue("AcceptAmPm", false);
            }

            set
            {
                if (MaskType == MaskedEditType.Time)
                {
                    SetPropertyValue("AcceptAmPm", value);
                }
            }
        }

        /// <summary>
        ///  Gets or sets AcceptNegative
        /// </summary>
        [DefaultValue(MaskedEditShowSymbol.None)]
        [ExtenderControlProperty]
        public MaskedEditShowSymbol AcceptNegative
        {
            get
            {
                return GetPropertyValue("AcceptNegative", MaskedEditShowSymbol.None);
            }

            set
            {
                if (MaskType == MaskedEditType.Number)
                {
                    SetPropertyValue("AcceptNegative", value);
                }
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether it's auto complete.
        /// </summary>
        [DefaultValue(true)]
        [ExtenderControlProperty]
        public bool AutoComplete
        {
            get
            {
                return GetPropertyValue("AutoComplete", true);
            }

            set
            {
                SetPropertyValue("AutoComplete", value);
            }
        }

        /// <summary>
        /// Gets or sets auto complete value
        /// </summary>
        [DefaultValue("")]
        [ExtenderControlProperty]
        public string AutoCompleteValue
        {
            get
            {
                return GetPropertyValue("AutoCompleteValue", string.Empty);
            }

            set
            {
                SetPropertyValue("AutoCompleteValue", value);
            }
        }

        /// <summary>
        /// Gets or sets century value.
        /// </summary>
        [DefaultValue(1900)]
        [ExtenderControlProperty]
        public int Century
        {
            get
            {
                return GetPropertyValue("Century", 1900);
            }

            set
            {
                if (value.ToString(CultureInfo.InvariantCulture).Length != 4)
                {
                    //throw new ArgumentException("The Century must have 4 digits.");
                    string strExc = Accela.Web.Controls.LabelConvertUtil.GetTextByKey("ACA_MaskedEditExtender_CenturyLengthError", string.Empty);
                    throw new ArgumentException(strExc);
                }
                else
                {
                    SetPropertyValue("Century", value);
                }
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether clear mask when lost focus.
        /// </summary>
        [DefaultValue(true)]
        [ExtenderControlProperty]
        public bool ClearMaskOnLostFocus
        {
            get
            {
                return GetPropertyValue("ClearMaskOnLostfocus", true);
            }

            set
            {
                SetPropertyValue("ClearMaskOnLostfocus", value);
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether clear text when text invalid
        /// </summary>
        [DefaultValue(false)]
        [ExtenderControlProperty]
        public bool ClearTextOnInvalid
        {
            get
            {
                return GetPropertyValue("ClearTextOnInvalid", false);
            }

            set
            {
                SetPropertyValue("ClearTextOnInvalid", value);
            }
        }

        /// <summary>
        /// Gets or sets CultureAMPMPlaceholder value.
        /// </summary>
        [Browsable(false)]
        [ExtenderControlProperty]
        [EditorBrowsable(EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1705:LongAcronymsShouldBePascalCased", MessageId = "Member", Justification = "Alternative of AmPm violates another rule")]
        public string CultureAMPMPlaceholder
        {
            get
            {
                return GetPropertyValue("CultureAMPMPlaceholder", "AM;PM");
            }

            set
            {
                SetPropertyValue("CultureAMPMPlaceholder", value);
            }
        }

        /// <summary>
        /// Gets or sets a value for CultureCurrencySymbolPlaceholder
        /// </summary>
        [Browsable(false)]
        [ExtenderControlProperty]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public string CultureCurrencySymbolPlaceholder
        {
            get
            {
                return GetPropertyValue("CultureCurrencySymbolPlaceholder", string.Empty);
            }

            set
            {
                SetPropertyValue("CultureCurrencySymbolPlaceholder", value);
            }
        }

        /// <summary>
        /// Gets or sets value for culture data format
        /// </summary>
        [Browsable(false)]
        [ExtenderControlProperty]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public string CultureDateFormat
        {
            get
            {
                return GetPropertyValue("CultureDateFMT", "MDY");
            }

            set
            {
                SetPropertyValue("CultureDateFMT", value);
            }
        }

        /// <summary>
        /// Gets or sets a value for culture data place holder 
        /// </summary>
        [Browsable(false)]
        [ExtenderControlProperty]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public string CultureDatePlaceholder
        {
            get
            {
                return GetPropertyValue("CultureDatePlaceholder", "/");
            }

            set
            {
                SetPropertyValue("CultureDatePlaceholder", value);
            }
        }

        /// <summary>
        /// Gets or sets a value for culture decimal place holder
        /// </summary>
        [Browsable(false)]
        [ExtenderControlProperty]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public string CultureDecimalPlaceholder
        {
            get
            {
                return GetPropertyValue("CultureDecimalPlaceholder", ".");
            }

            set
            {
                SetPropertyValue("CultureDecimalPlaceholder", value);
            }
        }

        /// <summary>
        /// Gets or sets a value for culture name
        /// </summary>
        [DefaultValue("")]
        [ExtenderControlProperty]
        public string CultureName
        {
            get
            {
                return GetPropertyValue("Culture", string.Empty);
            }

            set
            {
                SetPropertyValue("Culture", value);
            }
        }

        /// <summary>
        /// Gets or sets a value for culture thousands place holder
        /// </summary>
        [Browsable(false)]
        [ExtenderControlProperty]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public string CultureThousandsPlaceholder
        {
            get
            {
                return GetPropertyValue("CultureThousandsPlaceholder", ",");
            }

            set
            {
                SetPropertyValue("CultureThousandsPlaceholder", value);
            }
        }

        /// <summary>
        /// Gets or sets a value for culture time place holder
        /// </summary>
        [Browsable(false)]
        [ExtenderControlProperty]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public string CultureTimePlaceholder
        {
            get
            {
                return GetPropertyValue("CultureTimePlaceholder", ":");
            }

            set
            {
                SetPropertyValue("CultureTimePlaceholder", value);
            }
        }

        /// <summary>
        /// Gets or sets a value for display money
        /// </summary>
        [DefaultValue(MaskedEditShowSymbol.None)]
        [ExtenderControlProperty]
        public MaskedEditShowSymbol DisplayMoney
        {
            get
            {
                return GetPropertyValue("DisplayMoney", MaskedEditShowSymbol.None);
            }

            set
            {
                if (MaskType == MaskedEditType.Number)
                {
                    SetPropertyValue("DisplayMoney", value);
                }
            }
        }

        /// <summary>
        /// Gets or sets a value for filtered
        /// </summary>
        [DefaultValue("")]
        [ExtenderControlProperty]
        public string Filtered
        {
            get
            {
                return GetPropertyValue("Filtered", string.Empty);
            }

            set
            {
                SetPropertyValue("Filtered", value);
            }
        }

        /// <summary>
        /// Gets or sets a value for input direction
        /// </summary>
        [DefaultValue(MaskedEditInputDirection.LeftToRight)]
        [ExtenderControlProperty]
        public MaskedEditInputDirection InputDirection
        {
            get
            {
                return GetPropertyValue<MaskedEditInputDirection>("InputDirection", MaskedEditInputDirection.LeftToRight);
            }

            set
            {
                SetPropertyValue<MaskedEditInputDirection>("InputDirection", value);
            }
        }

        /// <summary>
        /// Gets or sets a value for mask
        /// </summary>
        [RequiredProperty]
        [DefaultValue("")]
        [ExtenderControlProperty]
        public string Mask
        {
            get
            {
                return GetPropertyValue("Mask", string.Empty);
            }

            set
            {
                if (!ValidateMaskType())
                {
                    //throw new ArgumentException("Validate Type and/or Mask is invalid!");
                    string strExc = Accela.Web.Controls.LabelConvertUtil.GetTextByKey("ACA_MaskedEditExtender_TypeMuskError", string.Empty);
                    throw new ArgumentException(strExc);
                }

                SetPropertyValue("Mask", value);
            }
        }

        /// <summary>
        /// Gets or sets a value for mask type
        /// </summary>
        [DefaultValue(MaskedEditType.None)]
        [ExtenderControlProperty]
        public MaskedEditType MaskType
        {
            get
            {
                return GetPropertyValue("MaskType", MaskedEditType.None);
            }

            set
            {
                SetPropertyValue("MaskType", value);

                switch (value)
                {
                    case MaskedEditType.Date:
                        {
                            SetPropertyValue("AcceptAmPm", false);
                            SetPropertyValue("AcceptNegative", MaskedEditShowSymbol.None);
                            SetPropertyValue("DisplayMoney", MaskedEditShowSymbol.None);
                            SetPropertyValue("Century", 0);
                            break;
                        }

                    case MaskedEditType.None:
                        {
                            SetPropertyValue("AcceptAmPm", false);
                            SetPropertyValue("AcceptNegative", MaskedEditShowSymbol.None);
                            SetPropertyValue("DisplayMoney", MaskedEditShowSymbol.None);
                            break;
                        }

                    case MaskedEditType.Time:
                        {
                            SetPropertyValue("AcceptNegative", MaskedEditShowSymbol.None);
                            SetPropertyValue("DisplayMoney", MaskedEditShowSymbol.None);
                            break;
                        }

                    case MaskedEditType.Number:
                        {
                            SetPropertyValue("AcceptAmPm", false);
                            break;
                        }
                }
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether message validator tip displays.
        /// </summary>
        [DefaultValue(true)]
        [ExtenderControlProperty]
        public bool MessageValidatorTip
        {
            get
            {
                return GetPropertyValue("MessageValidatorTip", true);
            }

            set
            {
                SetPropertyValue("MessageValidatorTip", value);
            }
        }

        /// <summary>
        /// Gets or sets CSS negative when on blur.
        /// </summary>
        [DefaultValue("MaskedEditBlurNegative")]
        [ExtenderControlProperty]
        public string OnBlurCssNegative
        {
            get
            {
                return GetPropertyValue("OnBlurCssNegative", "MaskedEditBlurNegative");
            }

            set
            {
                SetPropertyValue("OnBlurCssNegative", value);
            }
        }

        /// <summary>
        /// Gets or sets a value for CSS when on focus
        /// </summary>
        [DefaultValue("MaskedEditFocus")]
        [ExtenderControlProperty]
        public string OnFocusCssClass
        {
            get
            {
                return GetPropertyValue("OnFocusCssClass", "MaskedEditFocus");
            }

            set
            {
                SetPropertyValue("OnFocusCssClass", value);
            }
        }

        /// <summary>
        /// Gets or sets CSS negative when on focus
        /// </summary>
        [DefaultValue("MaskedEditFocusNegative")]
        [ExtenderControlProperty]
        public string OnFocusCssNegative
        {
            get
            {
                return GetPropertyValue("OnFocusCssNegative", "MaskedEditFocusNegative");
            }

            set
            {
                SetPropertyValue("OnFocusCssNegative", value);
            }
        }

        /// <summary>
        /// Gets or sets a CSS value when invalid
        /// </summary>
        [DefaultValue("MaskedEditError")]
        [ExtenderControlProperty]
        public string OnInvalidCssClass
        {
            get
            {
                return GetPropertyValue("OnInvalidCssClass", "MaskedEditError");
            }

            set
            {
                SetPropertyValue("OnInvalidCssClass", value);
            }
        }

        /// <summary>
        /// Gets or sets a value for prompt char
        /// </summary>
        [DefaultValue("_")]
        [ExtenderControlProperty]
        public string PromptCharacter
        {
            get
            {
                return GetPropertyValue("PromptChar", "_");
            }

            set
            {
                SetPropertyValue("PromptChar", value);
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether show star character.
        /// </summary>
        [DefaultValue(false)]
        [ExtenderControlProperty]
        public bool ShowStarCharacter
        {
            get
            {
                return GetPropertyValue("ShowStarChar", false);
            }

            set
            {
                SetPropertyValue("ShowStarChar", value);
            }
        }

        #endregion Properties

        #region Methods

        /// <summary>
        /// OnLoad override to to see browser culture / If this text box has default focus, use ClientState to let it know
        /// </summary>
        /// <param name="e">event args.</param>
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            ((TextBox)this.FindControl(TargetControlID)).MaxLength = 0;

            System.Globalization.CultureInfo brCult = MaskedEditCommon.GetCultureBrowser(string.Empty);

            if (!string.IsNullOrEmpty(this.CultureName))
            {
                brCult = MaskedEditCommon.GetCultureBrowser(this.CultureName);
            }

            SetPropertyValue("Culture", brCult.ToString());
            SetPropertyValue("CultureAMPMPlaceholder", brCult.DateTimeFormat.AMDesignator + ";" + brCult.DateTimeFormat.PMDesignator);
            SetPropertyValue("CultureCurrencySymbolPlaceholder", brCult.NumberFormat.CurrencySymbol);
            string shortDatePattern = I18nDateTimeUtil.ShortDatePattern; //BrCult.DateTimeFormat.ShortDatePattern;
            string dateSeparator = shortDatePattern.ToLower().Replace("y", string.Empty).Replace("m", string.Empty).Replace("d", string.Empty).Trim().Substring(0, 1); //BrCult.DateTimeFormat.DateSeparator
            string[] fMT = shortDatePattern.Split(dateSeparator.ToCharArray());
            SetPropertyValue("CultureDateFMT", fMT[0].Substring(0, 1).ToUpper(brCult) + fMT[1].Substring(0, 1).ToUpper(brCult) + fMT[2].Substring(0, 1).ToUpper(brCult));
            SetPropertyValue("CultureDatePlaceholder", dateSeparator);
            SetPropertyValue("CultureDecimalPlaceholder", brCult.NumberFormat.NumberDecimalSeparator);
            SetPropertyValue("CultureThousandsPlaceholder", brCult.NumberFormat.NumberGroupSeparator);
            SetPropertyValue("CultureTimePlaceholder", brCult.DateTimeFormat.TimeSeparator);

            // If this textbox has default focus, use ClientState to let it know
            ClientState = (string.Compare(Page.Form.DefaultFocus, TargetControlID, StringComparison.InvariantCultureIgnoreCase) == 0) ? "Focused" : null;
        }

        /// <summary>
        /// Validate mask type
        /// </summary>
        /// <returns>true or false</returns>
        private bool ValidateMaskType()
        {
            bool ret = true;
            string text = Mask;
            MaskedEditType tpvld = MaskType;

            if (!string.IsNullOrEmpty(text) && (tpvld == MaskedEditType.Date || tpvld == MaskedEditType.Time))
            {
                string _maskValid = MaskedEditCommon.GetValidMask(text);

                //date
                if (tpvld == MaskedEditType.Date)
                {
                    ret = false;

                    switch (_maskValid)
                    {
                        case "99/99/9999":
                        case "99/9999/99":
                        case "9999/99/99":
                        case "99/99/99":
                            ret = true;
                            break;
                    }
                }
                else if (tpvld == MaskedEditType.Time)
                {
                    //time
                    ret = false;

                    switch (_maskValid)
                    {
                        case "99:99:99":
                        case "99:99":
                            ret = true;
                            break;
                    }
                }
                else if (tpvld == MaskedEditType.Number)
                {
                    //Number
                    ret = true;

                    for (int i = 0; i < _maskValid.Length; i++)
                    {
                        if (_maskValid.Substring(i, 1) != "9" && _maskValid.Substring(i, 1) != "." && _maskValid.Substring(i, 1) != ",")
                        {
                            ret = false;
                            break;
                        }
                    }
                }
            }

            return ret;
        }

        #endregion Methods
    }
}
