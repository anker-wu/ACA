#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: MaskedEditValidator.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2008-2014
 *
 *  Description:
 *
 *  Notes:
 * $Id: MaskedEditValidator.cs 276801 2014-08-07 02:14:39Z ACHIEVO\james.shi $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */
//// (c) Copyright Microsoft Corporation.
//// This source is subject to the Microsoft Permissive License.
//// See http://www.microsoft.com/resources/sharedsource/licensingbasics/sharedsourcelicenses.mspx.
//// All other rights reserved.
//// Product      : MaskedEdit Validator Control
//// Version      : 1.0.0.0
//// Date         : 10/23/2006
//// Development  : Fernando Cerqueira
////

#endregion Header

using System;
using System.ComponentModel;
using System.Globalization;
using System.Web.UI;
using System.Web.UI.WebControls;

[assembly: System.Web.UI.WebResource("Accela.Web.Controls.MaskedEdit.MaskedEditValidator.js", "text/javascript")]

namespace AjaxControlToolkit
{
    /// <summary>
    /// Provide validator for masked text
    /// </summary>
    public class MaskedEditValidator : System.Web.UI.WebControls.BaseValidator
    {
        #region Fields

        /// <summary>
        /// string _clientValidationFunction, it's default value is empty.
        /// </summary>
        private string _clientValidationFunction = string.Empty;

        /// <summary>
        /// string _controlExtender, default value is empty.
        /// </summary>
        private string _controlExtender = string.Empty;

        /// <summary>
        /// initial value, default value is empty.
        /// </summary>
        private string _initialValue = string.Empty;

        /// <summary>
        /// Whether is valid empty, default value is true.
        /// </summary>
        private bool _isValidEmpty = true;

        /// <summary>
        /// maximum value, default value is empty.
        /// </summary>
        private string _maximumValue = string.Empty;

        /// <summary>
        /// empty message, default value is empty.
        /// </summary>
        private string _messageEmpty = string.Empty;

        /// <summary>
        /// invalid message value, default value is empty.
        /// </summary>
        private string _messageInvalid = string.Empty;

        /// <summary>
        /// max message value, default value is empty.
        /// </summary>
        private string _messageMax = string.Empty;

        /// <summary>
        /// min message value, default value is empty.
        /// </summary>
        private string _messageMin = string.Empty;

        /// <summary>
        /// tip message value, default value is empty.
        /// </summary>
        private string _messageTip = string.Empty;

        /// <summary>
        /// minimum message value, default value is empty.
        /// </summary>
        private string _minimumValue = string.Empty;

        /// <summary>
        /// validation expression value, default value is empty.
        /// </summary>
        private string _validationExpression = string.Empty;

        #endregion Fields

        #region Events

        /// <summary>
        /// event MaskedEditServerValidator
        /// </summary>
        public event EventHandler<ServerValidateEventArgs> MaskedEditServerValidator;

        #endregion Events

        #region Properties

        /// <summary>
        /// Gets or sets client validation function name
        /// </summary>
        [DefaultValue("")]
        [Category("MaskedEdit")]
        public string ClientValidationFunction
        {
            get
            {
                if (_clientValidationFunction == null)
                {
                    return string.Empty;
                }

                return _clientValidationFunction;
            }

            set
            {
                _clientValidationFunction = value;
            }
        }

        /// <summary>
        /// Gets or sets control extender value
        /// </summary>
        [DefaultValue("")]
        [TypeConverter(typeof(MaskedEditTypeConvert))]
        [RequiredProperty]
        [Category("MaskedEdit")]
        public string ControlExtender
        {
            get
            {
                if (_controlExtender == null)
                {
                    return string.Empty;
                }

                return _controlExtender;
            }

            set
            {
                _controlExtender = value;
            }
        }

        /// <summary>
        /// Gets or sets empty value message
        /// </summary>
        [DefaultValue("")]
        [Category("MaskedEdit")]
        public string EmptyValueMessage
        {
            get
            {
                if (_messageEmpty == null)
                {
                    return string.Empty;
                }

                return _messageEmpty;
            }

            set
            {
                _messageEmpty = value;
            }
        }

        /// <summary>
        /// Gets or sets error message
        /// </summary>
        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public new string ErrorMessage
        {
            get
            {
                return base.ErrorMessage;
            }

            set
            {
                base.ErrorMessage = value;
            }
        }

        /// <summary>
        /// Gets or sets initial value
        /// </summary>
        [DefaultValue("")]
        [Category("MaskedEdit")]
        public string InitialValue
        {
            get
            {
                if (_initialValue == null)
                {
                    return string.Empty;
                }

                return _initialValue;
            }

            set
            {
                _initialValue = value;
            }
        }

        /// <summary>
        /// Gets or sets invalid value message
        /// </summary>
        [DefaultValue("")]
        [Category("MaskedEdit")]
        public string InvalidValueMessage
        {
            get
            {
                if (_messageInvalid == null)
                {
                    return string.Empty;
                }

                return _messageInvalid;
            }

            set
            {
                _messageInvalid = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether is empty valid
        /// </summary>
        [DefaultValue(true)]
        [Category("MaskedEdit")]
        public bool IsValidEmpty
        {
            get
            {
                return _isValidEmpty;
            }

            set
            {
                _isValidEmpty = value;
            }
        }

        /// <summary>
        /// Gets or sets maximum value
        /// </summary>
        [DefaultValue("")]
        [Category("MaskedEdit")]
        public string MaximumValue
        {
            get
            {
                if (_maximumValue == null)
                {
                    return string.Empty;
                }

                return _maximumValue;
            }

            set
            {
                _maximumValue = value;
            }
        }

        /// <summary>
        /// Gets or sets maximum value message
        /// </summary>
        [DefaultValue("")]
        [Category("MaskedEdit")]
        public string MaximumValueMessage
        {
            get
            {
                if (_messageMax == null)
                {
                    return string.Empty;
                }

                return _messageMax;
            }

            set
            {
                _messageMax = value;
            }
        }

        /// <summary>
        /// Gets or sets minimum value
        /// </summary>
        [DefaultValue("")]
        [Category("MaskedEdit")]
        public string MinimumValue
        {
            get
            {
                if (_minimumValue == null)
                {
                    return string.Empty;
                }

                return _minimumValue;
            }

            set
            {
                _minimumValue = value;
            }
        }

        /// <summary>
        /// Gets or sets minimum value message
        /// </summary>
        [DefaultValue("")]
        [Category("MaskedEdit")]
        public string MinimumValueMessage
        {
            get
            {
                if (_messageMin == null)
                {
                    return string.Empty;
                }

                return _messageMin;
            }

            set
            {
                _messageMin = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is hijri date.
        /// </summary>
        /// <value><c>true</c> if this instance is hijri date; otherwise, <c>false</c>.</value>
        [DefaultValue(false)]
        [Category("MaskedEdit")]
        public bool IsHijriDate { get; set; }

        /// <summary>
        /// Gets or sets tool tip message
        /// </summary>
        [DefaultValue("")]
        [Category("MaskedEdit")]
        public string TooltipMessage
        {
            get
            {
                if (_messageTip == null)
                {
                    return string.Empty;
                }

                return _messageTip;
            }

            set
            {
                _messageTip = value;
            }
        }

        /// <summary>
        /// Gets or sets validation expression
        /// </summary>
        [DefaultValue("")]
        [Category("MaskedEdit")]
        public string ValidationExpression
        {
            get
            {
                if (_validationExpression == null)
                {
                    return string.Empty;
                }

                return _validationExpression;
            }

            set
            {
                _validationExpression = value;
            }
        }

        /// <summary>
        /// Gets a value indicating whether is partial rendering supported
        /// </summary>
        internal bool IsPartialRenderingSupported
        {
            get
            {
                if (!this.PartialRenderingChecked)
                {
                    Type scriptManagerType = System.Web.Compilation.BuildManager.GetType("System.Web.UI.ScriptManager", false);

                    if (scriptManagerType != null)
                    {
                        object obj2 = this.Page.Items[scriptManagerType];

                        if (obj2 != null)
                        {
                            System.Reflection.PropertyInfo property = scriptManagerType.GetProperty("SupportsPartialRendering");

                            if (property != null)
                            {
                                object obj3 = property.GetValue(obj2, null);
                                this.IsPartialRenderingEnabled = (bool)obj3;
                            }
                        }
                    }

                    this.PartialRenderingChecked = true;
                }

                return this.IsPartialRenderingEnabled;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether partial rendering enabled
        /// </summary>
        private bool IsPartialRenderingEnabled
        {
            get
            {
                object val = ViewState["IsPartialRenderingEnabled"];

                if (val != null)
                {
                    return (bool)val;
                }

                return false;
            }

            set
            {
                ViewState["IsPartialRenderingEnabled"] = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether partial rendering checked
        /// </summary>
        private bool PartialRenderingChecked
        {
            get
            {
                object val = ViewState["PartialRenderingChecked"];

                if (val != null)
                {
                    return (bool)val;
                }

                return false;
            }

            set
            {
                ViewState["PartialRenderingChecked"] = value;
            }
        }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Override AddAttributesToRender
        /// </summary>
        /// <param name="writer">object HtmlTextWriter</param>
        protected override void AddAttributesToRender(HtmlTextWriter writer)
        {
            base.AddAttributesToRender(writer);
            MaskedEditExtender maskExt = (MaskedEditExtender)FindControl(ControlExtender);
            string functionName = string.Empty;

            switch (maskExt.MaskType)
            {
                case MaskedEditType.Date:
                    functionName = "MaskedEditValidatorDate";
                    break;
                case MaskedEditType.Number:
                    functionName = "MaskedEditValidatorNumber";
                    break;
                case MaskedEditType.Time:
                    functionName = "MaskedEditValidatorTime";
                    break;
                default:
                    functionName = "MaskedEditValidatorNone";
                    break;
            }

            if (this.RenderUplevel)
            {
                string clientID = this.ClientID;

                if (!this.IsPartialRenderingSupported)
                {
                    Page.ClientScript.RegisterExpandoAttribute(clientID, "evaluationfunction", functionName);
                }
                else
                {
                    Type scriptManagerType = System.Web.Compilation.BuildManager.GetType("System.Web.UI.ScriptManager", false);

                    // ScriptManager.RegisterExpandoAttribute(this, clientID, "evaluationfunction", "MaskedEditValidatorDate", false);
                    scriptManagerType.InvokeMember(
                        "RegisterExpandoAttribute", 
                        System.Reflection.BindingFlags.InvokeMethod | System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static, 
                        null, 
                        null,
                        new object[] { this, this.ClientID, "evaluationfunction", functionName, false });
                }
            }
        }

        /// <summary>
        /// Override ControlPropertiesValid
        /// </summary>
        /// <returns>Whether property valid.</returns>
        protected override bool ControlPropertiesValid()
        {
            bool isTextControl = FindControl(ControlToValidate) is System.Web.UI.WebControls.TextBox;
            return isTextControl;
        }

        /// <summary>
        /// Override EvaluateIsValid
        /// </summary>
        /// <returns>true if the value in the input control is valid; otherwise, false.</returns>
        protected override bool EvaluateIsValid()
        {
            MaskedEditExtender maskExt = (MaskedEditExtender)FindControl(ControlExtender);
            TextBox target = (TextBox)maskExt.FindControl(ControlToValidate);
            base.ErrorMessage = string.Empty;
            string cssError = string.Empty;
            bool ok = true;

            if (!this.IsValidEmpty)
            {
                if (target.Text.Trim() == this.InitialValue)
                {
                    base.ErrorMessage = this.EmptyValueMessage;
                    cssError = maskExt.OnInvalidCssClass;
                    ok = false;
                }
            }

            if (ok && target.Text.Length != 0 && this.ValidationExpression.Length != 0)
            {
                try
                {
                    System.Text.RegularExpressions.Regex regex = new System.Text.RegularExpressions.Regex(this.ValidationExpression);
                    ok = regex.IsMatch(target.Text);
                }
                catch
                {
                    ok = false;
                }
            }

            if (ok && target.Text.Length != 0)
            {
                string culture = maskExt.CultureName;

                if (string.IsNullOrEmpty(culture))
                {
                    culture = MaskedEditCommon.GetCultureBrowser(string.Empty).ToString();
                }

                string cultureAMPMP = string.Empty;

                if (!string.IsNullOrEmpty(CultureInfo.GetCultureInfo(culture).DateTimeFormat.AMDesignator)
                    && !string.IsNullOrEmpty(CultureInfo.GetCultureInfo(culture).DateTimeFormat.PMDesignator))
                {
                    cultureAMPMP = CultureInfo.GetCultureInfo(culture).DateTimeFormat.AMDesignator + ";" + CultureInfo.GetCultureInfo(culture).DateTimeFormat.PMDesignator;
                }

                switch (maskExt.MaskType)
                {
                    case MaskedEditType.Number:
                        try
                        {
                            CultureInfo cultControl = CultureInfo.GetCultureInfo(culture);
                            decimal numval = decimal.Parse(target.Text, cultControl);
                        }
                        catch
                        {
                            ok = false;
                        }

                        break;
                    case MaskedEditType.Date:
                    case MaskedEditType.Time:
                        int tamtext = target.Text.Length;

                        if (maskExt.MaskType == MaskedEditType.Time && !string.IsNullOrEmpty(cultureAMPMP))
                        {
                            char[] charSeparators = new char[] { ';' };
                            string[] arrAMPM = cultureAMPMP.Split(charSeparators);

                            if (arrAMPM[0].Length != 0)
                            {
                                tamtext -= arrAMPM[0].Length + 1;
                            }
                        }

                        if (MaskedEditCommon.GetValidMask(maskExt.Mask).Length != tamtext)
                        {
                            ok = false;
                        }

                        if (ok)
                        {
                            try
                            {
                                CultureInfo cultControl = CultureInfo.GetCultureInfo(culture);
                                DateTime dtval = System.DateTime.Parse(target.Text, cultControl);
                            }
                            catch
                            {
                                ok = false;
                            }
                        }

                        break;
                }

                if (!ok)
                {
                    base.ErrorMessage = this.InvalidValueMessage;
                    cssError = maskExt.OnInvalidCssClass;
                }

                if (ok && (!string.IsNullOrEmpty(this.MaximumValue) || !string.IsNullOrEmpty(this.MinimumValue)))
                {
                    CultureInfo cultControl = CultureInfo.GetCultureInfo(culture);

                    switch (maskExt.MaskType)
                    {
                        case MaskedEditType.None:
                            {
                                int lenvalue;

                                if (!string.IsNullOrEmpty(this.MaximumValue))
                                {
                                    try
                                    {
                                        lenvalue = int.Parse(this.MaximumValue, cultControl);
                                        ok = lenvalue >= target.Text.Length;
                                    }
                                    catch
                                    {
                                        base.ErrorMessage = this.InvalidValueMessage;
                                        ok = false;
                                    }

                                    if (!ok)
                                    {
                                        base.ErrorMessage = this.MaximumValueMessage;
                                        cssError = maskExt.OnInvalidCssClass;
                                    }
                                }

                                if (!string.IsNullOrEmpty(this.MinimumValue))
                                {
                                    try
                                    {
                                        lenvalue = int.Parse(this.MinimumValue, cultControl);
                                        ok = lenvalue <= target.Text.Length;
                                    }
                                    catch
                                    {
                                        base.ErrorMessage = this.InvalidValueMessage;
                                        ok = false;
                                    }

                                    if (!ok)
                                    {
                                        base.ErrorMessage = this.MinimumValueMessage;
                                        cssError = maskExt.OnInvalidCssClass;
                                    }
                                }

                                break;
                            }

                        case MaskedEditType.Number:
                            {
                                decimal numval = decimal.Parse(target.Text, cultControl);
                                decimal compval;

                                if (!string.IsNullOrEmpty(this.MaximumValue))
                                {
                                    try
                                    {
                                        compval = decimal.Parse(this.MaximumValue, cultControl);
                                        ok = compval >= numval;
                                    }
                                    catch
                                    {
                                        ok = false;
                                    }

                                    if (!ok)
                                    {
                                        base.ErrorMessage = this.MaximumValueMessage;
                                        cssError = maskExt.OnInvalidCssClass;
                                    }
                                }

                                if (!string.IsNullOrEmpty(this.MinimumValue))
                                {
                                    try
                                    {
                                        compval = decimal.Parse(this.MinimumValue, cultControl);
                                        ok = compval <= numval;
                                    }
                                    catch
                                    {
                                        ok = false;
                                    }

                                    if (!ok)
                                    {
                                        base.ErrorMessage = this.MinimumValueMessage;
                                        cssError = maskExt.OnInvalidCssClass;
                                    }
                                }

                                break;
                            }

                        case MaskedEditType.Date:
                        case MaskedEditType.Time:
                            {
                                DateTime dtval = DateTime.Parse(target.Text, cultControl);
                                DateTime dtCompval;

                                if (!string.IsNullOrEmpty(this.MaximumValue))
                                {
                                    try
                                    {
                                        dtCompval = DateTime.Parse(this.MaximumValue, cultControl);
                                        ok = dtCompval >= dtval;
                                    }
                                    catch
                                    {
                                        ok = false;
                                    }

                                    if (!ok)
                                    {
                                        base.ErrorMessage = this.MaximumValueMessage;
                                        cssError = maskExt.OnInvalidCssClass;
                                    }
                                }

                                if (!string.IsNullOrEmpty(this.MinimumValue))
                                {
                                    try
                                    {
                                        dtCompval = DateTime.Parse(this.MinimumValue, cultControl);
                                        ok = dtCompval <= dtval;
                                    }
                                    catch
                                    {
                                        ok = false;
                                    }

                                    if (!ok)
                                    {
                                        base.ErrorMessage = this.MinimumValueMessage;
                                        cssError = maskExt.OnInvalidCssClass;
                                    }
                                }

                                break;
                            }
                    }
                }
            }

            if (ok && MaskedEditServerValidator != null)
            {
                ServerValidateEventArgs serverValidateEventArgs = new ServerValidateEventArgs(target.Text, ok);
                MaskedEditServerValidator(target, serverValidateEventArgs);
                ok = serverValidateEventArgs.IsValid;
            }

            if (!ok)
            {
                // set CSS at server for browser with not implement client validator script (FF, others)
                //MaskedEditSetCssClass(value,CSS)
                string script = "MaskedEditSetCssClass(" + this.ClientID + ",'" + cssError + "');";
                ScriptManager.RegisterStartupScript(this, typeof(MaskedEditValidator), "MaskedEditServerValidator_" + this.ID, script, true);
            }

            return ok;
        }

        /// <summary>
        /// Override OnPreRender
        /// </summary>
        /// <param name="e">event args</param>
        protected override void OnPreRender(System.EventArgs e)
        {
            if (this.EnableClientScript)
            {
                // register Script Resource at current page
                this.Page.ClientScript.RegisterClientScriptResource(typeof(MaskedEditValidator), "Accela.Web.Controls.MaskedEdit.MaskedEditValidator.js");

                MaskedEditExtender maskExt = (MaskedEditExtender)FindControl(ControlExtender);
                TextBox target = (TextBox)maskExt.FindControl(ControlToValidate);

                int firstMaskPos = -1;
                int lastMaskPosition = -1;

                if (maskExt.ClearMaskOnLostFocus)
                {
                    firstMaskPos = 0;
                    lastMaskPosition = MaskedEditCommon.GetValidMask(maskExt.Mask).Length + 1;
                }
                else
                {
                    firstMaskPos = MaskedEditCommon.GetFirstMaskPosition(maskExt.Mask);
                    lastMaskPosition = MaskedEditCommon.GetLastMaskPosition(maskExt.Mask) + 1;
                }

                this.Attributes.Add("IsMaskedEdit", true.ToString().ToLowerInvariant());
                this.Attributes.Add("ValidEmpty", this.IsValidEmpty.ToString().ToLowerInvariant());
                this.Attributes.Add("MaximumValue", this.MaximumValue);
                this.Attributes.Add("MinimumValue", this.MinimumValue);
                this.Attributes.Add("InitialValue", this.InitialValue);
                this.Attributes.Add("ValidationExpression", this.ValidationExpression);
                this.Attributes.Add("ClientValidationFunction", this.ClientValidationFunction);
                this.Attributes.Add("TargetValidator", target.ClientID);
                this.Attributes.Add("EmptyValueMessage", this.EmptyValueMessage);
                this.Attributes.Add("MaximumValueMessage", this.MaximumValueMessage);
                this.Attributes.Add("MinimumValueMessage", this.MinimumValueMessage);
                this.Attributes.Add("InvalidValueMessage", this.InvalidValueMessage);
                this.Attributes.Add("InvalidValueCssClass", maskExt.OnInvalidCssClass);
                this.Attributes.Add("CssBlurNegative", maskExt.OnBlurCssNegative);
                this.Attributes.Add("CssFocus", maskExt.OnFocusCssClass);
                this.Attributes.Add("CssFocusNegative", maskExt.OnFocusCssNegative);
                this.Attributes.Add("TooltipMessage", this.TooltipMessage);
                this.Attributes.Add("FirstMaskPosition", firstMaskPos.ToString(CultureInfo.InvariantCulture));

                switch (maskExt.MaskType)
                {
                    case MaskedEditType.None:
                        {
                            this.Attributes.Add("lastMaskPosition", lastMaskPosition.ToString(CultureInfo.InvariantCulture));
                            break;
                        }

                    case MaskedEditType.Number:
                        {
                            if (maskExt.DisplayMoney != MaskedEditShowSymbol.None)
                            {
                                lastMaskPosition += maskExt.CultureCurrencySymbolPlaceholder.Length + 1;
                            }

                            if (maskExt.AcceptNegative != MaskedEditShowSymbol.None)
                            {
                                if (maskExt.DisplayMoney != MaskedEditShowSymbol.None)
                                {
                                    lastMaskPosition++;
                                }
                                else
                                {
                                    lastMaskPosition += 2;
                                }
                            }

                            this.Attributes.Add("Money", maskExt.CultureCurrencySymbolPlaceholder);
                            this.Attributes.Add("Decimal", maskExt.CultureDecimalPlaceholder);
                            this.Attributes.Add("Thousands", maskExt.CultureThousandsPlaceholder);
                            this.Attributes.Add("lastMaskPosition", lastMaskPosition.ToString(CultureInfo.InvariantCulture));
                            break;
                        }

                    case MaskedEditType.Date:
                        {
                            this.Attributes.Add("DateSeparator", maskExt.CultureDatePlaceholder);
                            this.Attributes.Add("DateFormat", maskExt.CultureDateFormat);
                            this.Attributes.Add("Century", maskExt.Century.ToString(CultureInfo.InvariantCulture));
                            this.Attributes.Add("IsHijriDate", IsHijriDate.ToString().ToLower());
                            this.Attributes.Add("lastMaskPosition", lastMaskPosition.ToString(CultureInfo.InvariantCulture));
                            break;
                        }

                    case MaskedEditType.Time:
                        {
                            if (maskExt.AcceptAMPM)
                            {
                                string[] aSymMask = maskExt.CultureAMPMPlaceholder.Split(";".ToCharArray());

                                if (aSymMask[0].Length != 0)
                                {
                                    lastMaskPosition += aSymMask[0].Length + 1;
                                }
                            }

                            this.Attributes.Add("TimeSeparator", maskExt.CultureTimePlaceholder);
                            this.Attributes.Add("AmPmSymbol", maskExt.CultureAMPMPlaceholder);
                            this.Attributes.Add("lastMaskPosition", lastMaskPosition.ToString(CultureInfo.InvariantCulture));
                            break;
                        }
                }
            }

            base.OnPreRender(e);
        }

        #endregion Methods
    }
}