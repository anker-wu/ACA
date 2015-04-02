#region Header

/**
 * <pre>
 *
 *  Accela
 *  File: AccelaNumberText.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2007-2014
 *
 *  Description:
 *
 *  Notes:
 * $Id: AccelaNumberText.cs 276801 2014-08-07 02:14:39Z ACHIEVO\james.shi $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using System;
using System.Web.UI;
using System.Web.UI.WebControls;

using Accela.ACA.Common;
using Accela.ACA.Common.Util;

using AjaxControlToolkit;

namespace Accela.Web.Controls
{
    /// <summary>
    /// provide a number box to input number
    /// </summary>
    public class AccelaNumberText : AccelaTextBox
    {
        #region Fields

        /// <summary>
        /// check decimal digits length or not
        /// </summary>
        private bool _checkDecimalDigitsLength = false;

        /// <summary>
        /// check maximum value or not
        /// </summary>
        private bool _checkMaximumValue = false;

        /// <summary>
        /// check min value or not
        /// </summary>
        private bool _checkMinimumValue = false;

        /// <summary>
        /// check decimal total length or not(Max Length and decimal fraction)
        /// </summary>
        private bool _checkDecimalTotalLength = false;

        /// <summary>
        /// decimal digits
        /// </summary>
        private int _decimalDigits = 0;

        /// <summary>
        /// is for country code
        /// </summary>
        private bool _is4CountryCode = false;

        /// <summary>
        /// maximum value
        /// </summary>
        private decimal _maximumValue = 0;

        /// <summary>
        /// minimum value
        /// </summary>
        private decimal _minimumValue = 0;

        /// <summary>
        /// valid char.
        /// </summary>
        private string _validChars = I18nNumberUtil.NumberDecimalSeparator + ".";

        /// <summary>
        /// valid negative
        /// </summary>
        private string _validNegative = I18nNumberUtil.NegativeSign;

        /// <summary>
        /// is need dot or not
        /// </summary>
        private bool isNeedDot = true;

        /// <summary>
        /// is need negative or not
        /// </summary>
        private bool isNeedNegative = false;

        #endregion Fields

        #region Properties

        /// <summary>
        /// Gets or sets decimal digits length
        /// </summary>
        public int DecimalDigitsLength
        {
            get
            {
                return _decimalDigits;
            }

            set
            {
                _decimalDigits = int.Parse(value.ToString());
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the control is used for country code
        /// </summary>
        public bool Is4CountryCode
        {
            get
            {
                return _is4CountryCode;
            }

            set
            {
                _is4CountryCode = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether need dot .if is true,the control can input the ValidChars,otherwise can not
        /// </summary>
        public bool IsNeedDot
        {
            get
            {
                return isNeedDot;
            }

            set
            {
                isNeedDot = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether need negative. if is true,the control can input the negative,otherwise can not
        /// </summary>
        public bool IsNeedNegative
        {
            get
            {
                return isNeedNegative;
            }

            set
            {
                isNeedNegative = value;
            }
        }

        /// <summary>
        /// Gets or sets max value. Max value who in order to limit the input text value to less than it
        /// </summary>
        public decimal MaximumValue
        {
            get
            {
                return _maximumValue;
            }

            set
            {
                _maximumValue = value;
            }
        }

        /// <summary>
        /// Gets or sets min value. Mini value who in order to limit the input text value to bigger than it
        /// </summary>
        public decimal MinimumValue
        {
            get
            {
                return _minimumValue;
            }

            set
            {
                _minimumValue = value;
            }
        }

        /// <summary>
        /// Gets or sets the char can be input
        /// </summary>
        public new string ValidChars
        {
            get
            {
                return _validChars;
            }

            set
            {
                _validChars = value;
            }
        }

        /// <summary>
        /// Gets or sets the Negative can be input
        /// </summary>
        public string ValidNegative
        {
            get
            {
                return _validNegative;
            }

            set
            {
                _validNegative = value;
            }
        }

        /// <summary>
        /// Sets validate
        /// </summary>
        public override string Validate
        {
            set
            {
                this._checkDecimalDigitsLength = false;
                this._checkMaximumValue = false;
                this._checkMinimumValue = false;
                this._checkDecimalTotalLength = false;
                base.Validate = value;

                if (string.IsNullOrEmpty(value))
                {
                    return;
                }

                string[] parms = value.Split(';');

                foreach (string parm in parms)
                {
                    //if (parm.ToLower().Equals("decimaldigitslength"))
                    if (parm.Equals("decimaldigitslength", StringComparison.InvariantCultureIgnoreCase))
                    {
                        this._checkDecimalDigitsLength = true;
                    }
                    else if (parm.Equals("maximumvalue", StringComparison.InvariantCultureIgnoreCase))
                    {
                        this._checkMaximumValue = true;
                    }
                    else if (parm.Equals("minimumvalue", StringComparison.InvariantCultureIgnoreCase))
                    {
                        this._checkMinimumValue = true;
                    }
                    else if (parm.Equals("decimaltotallength", StringComparison.InvariantCultureIgnoreCase))
                    {
                        this._checkDecimalTotalLength = true;
                    }
                }
            }
        }

        /// <summary>
        /// Gets max integer length
        /// </summary>
        private string MaxIntegerLength
        {
            get
            {
                int maxIntegerLength = MaxLength - DecimalDigitsLength - 1;
                return maxIntegerLength.ToString();
            }
        }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Create range validator
        /// </summary>
        /// <returns>RangeValidator object</returns>
        protected virtual System.Web.UI.WebControls.RangeValidator CreateRangeValidator()
        {
            return new RangeValidator();
        }

        /// <summary>
        /// Create regular expression validator
        /// </summary>
        /// <returns>RegularExpressionValidator object</returns>
        protected virtual System.Web.UI.WebControls.RegularExpressionValidator CreateRegularExpressionValidator()
        {
            return new RegularExpressionValidator();
        }

        /// <summary>
        /// Create number range validator
        /// </summary>
        /// <param name="baseNumReq">RangeValidator object</param>
        /// <param name="id">string control id</param>
        /// <param name="errorMessage">error message</param>
        protected void NumberRangeValidator(RangeValidator baseNumReq, string id, string errorMessage)
        {
            if (this._checkMaximumValue || this._checkMinimumValue)
            {
                baseNumReq.ControlToValidate = ID;
                baseNumReq.ID = id;
                baseNumReq.Display = ValidatorDisplay.None;
                baseNumReq.ErrorMessage = errorMessage.Replace("'", "\\'");
                baseNumReq.MinimumValue = this.MinimumValue.ToString();
                baseNumReq.MaximumValue = this.MaximumValue.ToString();
                baseNumReq.Type = ValidationDataType.Double;
                baseNumReq.EnableClientScript = true;
                baseNumReq.SetFocusOnError = this.SetFocusOnError;
                Controls.Add(baseNumReq);
                CreateValidatorCallbackExtender(baseNumReq.ID, (baseNumReq is RangeValidator) ? CheckControlValueValidateFunction : null);
            }
        }

        /// <summary>
        /// Create number expression validator
        /// </summary>
        /// <param name="baseNumReq">RegularExpressionValidator object</param>
        /// <param name="id">string control id</param>
        /// <param name="errorMessage">error message</param>
        protected void NumberRegularExpressionValidator(RegularExpressionValidator baseNumReq, string id, string errorMessage)
        {
            if ((this._checkDecimalTotalLength || this._checkDecimalDigitsLength) && this.MaxLength != 0)
            {
                baseNumReq.ControlToValidate = ID;
                baseNumReq.ID = id;
                baseNumReq.Display = ValidatorDisplay.None;
                baseNumReq.ErrorMessage = errorMessage.Replace("'", "\\'");

                if (this._checkDecimalTotalLength)
                {
                    baseNumReq.ValidationExpression = "^-?\\d{1," + this.MaxLength + "}(\\" + I18nNumberUtil.NumberDecimalSeparator + "\\d{1," + DecimalDigitsLength + "})?$";
                }
                else
                {
                    baseNumReq.ValidationExpression = "^-?\\d{1," + this.MaxIntegerLength + "}(\\" + I18nNumberUtil.NumberDecimalSeparator + "\\d{1," + DecimalDigitsLength + "})?$";
                }

                baseNumReq.EnableClientScript = true;
                baseNumReq.SetFocusOnError = this.SetFocusOnError;
                Controls.Add(baseNumReq);
                CreateValidatorCallbackExtender(baseNumReq.ID, (baseNumReq is RegularExpressionValidator) ? CheckControlValueValidateFunction : null);
            }
        }

        /// <summary>
        /// Raises the System.Web.UI.Control.PreRender event.
        /// </summary>
        /// <param name="e">A System.EventArgs that contains the event data.</param>
        protected override void OnPreRender(EventArgs e)
        {
            if (!AccelaControlRender.IsAdminRender(this))
            {
                FilteredTextBoxExtender();
                InitNumberRequiredValidator();
            }

            base.OnPreRender(e);
        }

        /// <summary>
        /// add filtered text box extender
        /// </summary>
        private void FilteredTextBoxExtender()
        {
            FilteredTextBoxExtender filterExt = new FilteredTextBoxExtender();
            filterExt.ID = ID + "_filter_exd";
            filterExt.TargetControlID = ID;

            if (isNeedDot)
            {
                filterExt.ValidChars = ValidChars;
            }

            if (isNeedNegative)
            {
                filterExt.ValidChars = ValidNegative;
            }

            if (isNeedDot && isNeedNegative)
            {
                filterExt.ValidChars = ValidChars + ValidNegative;
            }

            filterExt.FilterType = AjaxControlToolkit.FilterTypes.Numbers | FilterTypes.Custom;
            AppendNewAttribute(Attributes, "onkeypress", "filter(this," + this.isNeedDot.ToString().ToLowerInvariant() + "," + this.isNeedNegative.ToString().ToLowerInvariant() + ",event);");
            AppendNewAttribute(Attributes, "onkeyup", "replaceDecimalSeparator(this, event);");
            AppendNewAttribute(Attributes, "onblur", "LimitParse(this,event);");
            AppendNewAttribute(Attributes, "onchange", "replaceDecimalSeparator(this, event);");
            Attributes.Add("validChars", ValidChars);
            Controls.Add(filterExt);
        }

        /// <summary>
        /// Appends the new attribute.
        /// </summary>
        /// <param name="attributeCollection">The attribute collection.</param>
        /// <param name="attributeKey">The attribute key.</param>
        /// <param name="attributeValue">The attribute value.</param>
        private void AppendNewAttribute(AttributeCollection attributeCollection, string attributeKey, string attributeValue)
        {
            string oldAttributeValue = string.IsNullOrWhiteSpace(attributeCollection[attributeKey]) ? string.Empty : attributeCollection[attributeKey];

            if (!oldAttributeValue.Contains(attributeValue))
            {
                string newAttributeValue = attributeValue + " " + oldAttributeValue;
                attributeCollection[attributeKey] = newAttributeValue;
            }
        }

        /// <summary>
        /// Gets the current runtime page's module name.
        /// if current page doesn't belong to any module, returns string.Empty
        /// </summary>
        /// <returns>module name.</returns>
        private string GetModuleName()
        {
            if (this.Page is IPage)
            {
                return (Page as IPage).GetModuleName();
            }
            else
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// Number Required Validator
        /// </summary>
        private void InitNumberRequiredValidator()
        {
            //NumberRegularExpressionValidator(CreateRegularExpressionValidator(), ID + "_baseNumExp", "Integer length<" + MaxIntegerLength + " digits,decimal fraction length==" + this.DecimalDigitsLength + " digits.");
            //NumberRangeValidator(CreateRangeValidator(), ID + "_baseNumRan", "Please input area number(" + this.MinimumValue + "~" + this.MaximumValue + ")");

            //--"Integer length<{0} digits,decimal fraction length<{1} digits.";
            int maxDecimalIntegerLength = MaxLength - DecimalDigitsLength;
            int maxDecimalDigitsLength = DecimalDigitsLength + 1;
            string strNumRegular = LabelConvertUtil.GetTextByKey(
                "ACA_AccelaNumberText_NumberRegularExpressionValidator",
                GetModuleName());
            string message = string.Format(strNumRegular, maxDecimalIntegerLength, maxDecimalDigitsLength);

            if (this._checkDecimalTotalLength)
            {
                message = LabelConvertUtil.GetTextByKey("aca_accelanumbertext_decimalfractionregularexpressionvalidator", GetModuleName());
                message = string.Format(message, maxDecimalDigitsLength.ToString());
            }

            NumberRegularExpressionValidator(CreateRegularExpressionValidator(), ID + "_baseNumExp", message);

            //--"Please input area number({0}~{1})";
            string strRegular = LabelConvertUtil.GetTextByKey("ACA_AccelaNumberText_NumberRangeValidator", GetModuleName());
            NumberRangeValidator(CreateRangeValidator(), ID + "_baseNumRan", string.Format(strRegular, this.MinimumValue, this.MaximumValue));
        }

        #endregion Methods
    }
}
