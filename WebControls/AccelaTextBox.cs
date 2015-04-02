#region Header

/**
 * <pre>
 *
 *  Accela
 *  File: AccelaTextBox.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2007-2014
 *
 *  Description:
 *
 *  Notes:
 * $Id: AccelaTextBox.cs 279268 2014-10-16 07:47:19Z ACHIEVO\james.shi $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using System;
using System.Globalization;
using System.Web.UI;
using System.Web.UI.WebControls;

using Accela.ACA.Common;
using Accela.ACA.Common.Common;
using Accela.ACA.Common.Util;
using Accela.Web.Controls.ControlRender;

using AjaxControlToolkit;

[assembly: WebResource("Accela.Web.Controls.Assets.soundex.png", "image/png")]

namespace Accela.Web.Controls
{
    /// <summary>
    /// provide a text box to input string
    /// </summary>
    public class AccelaTextBox : TextBox, IAccelaControl
    {
        #region Fields

        /// <summary>
        /// The JS show loading string
        /// </summary>
        private const string JS_SHOW_LOADING = "var p = new ProcessLoading();p.showLoading();";

        /// <summary>
        /// whether check custom validation or not
        /// </summary>
        private bool _checkCustomValidation;

        /// <summary>
        /// whether check max length or not
        /// </summary>
        private bool _checkMaxLength;

        /// <summary>
        /// custom validation function name
        /// </summary>
        private string _customValidationFunction = string.Empty;

        /// <summary>
        /// custom validation message key
        /// </summary>
        private string _customValidationMessageKey = string.Empty;

        /// <summary>
        /// string validate
        /// </summary>
        private string _validate = string.Empty;

        /// <summary>
        /// call back fail function, default value is "doErrorCallbackFun".
        /// </summary>
        private string _callbackFailFunction = "doErrorCallbackFun";

        /// <summary>
        /// check min length or not
        /// </summary>
        private bool _checkMinLength;

        /// <summary>
        /// check control required or not
        /// </summary>
        private bool _checkRequired;

        /// <summary>
        /// client script, default value is "true"
        /// </summary>
        private string _clientScript = "true";

        /// <summary>
        /// is client visible,default value is true
        /// </summary>
        private bool _clientVisible = true;

        /// <summary>
        /// is client always editable,default value is false;
        /// </summary>
        private bool _isAlwaysEditable = false;

        /// <summary>
        /// filed alignment, default value is left to right
        /// </summary>
        private FieldAlignment _fieldAlignment = FieldAlignment.LTR;

        /// <summary>
        /// whether hide required indicate.
        /// </summary>
        private bool _hideRequireIndicate;

        /// <summary>
        /// high light CSS class,default value is "HighlightCSSClass".
        /// </summary>
        private string _highlightCssClass = "HighlightCssClass";

        /// <summary>
        /// whether display label,default value is true.
        /// </summary>
        private bool _isDisplayLabel = true;

        /// <summary>
        /// label string.
        /// </summary>
        private string _label;

        /// <summary>
        /// whether set focus on error, default value is true.
        /// </summary>
        private bool _setFocusOnError = true;

        /// <summary>
        /// sub label string.
        /// </summary>
        private string _subLabel;

        /// <summary>
        /// valid chars
        /// </summary>
        private string _validChars;

        /// <summary>
        /// validation expression
        /// </summary>
        private string _validationExpression = string.Empty;
 
        /// <summary>
        /// Position ID value
        /// </summary>
        private AutoFillPosition _positionID;

        /// <summary>
        /// auto fill type
        /// </summary>
        private AutoFillType _autoFillType = AutoFillType.None;
        
        /// <summary>
        /// layout Type
        /// </summary>
        private ControlLayoutType _layoutType = ControlLayoutType.Vertical;

        /// <summary>
        /// unit Type
        /// </summary>
        private string _fieldUnit = string.Empty;

        /// <summary>
        /// label width
        /// </summary>
        private string _labelWidth = string.Empty;

        /// <summary>
        /// field width
        /// </summary>
        private string _fieldWidth = string.Empty;

        /// <summary>
        /// Watermark text
        /// </summary>
        private string _watermarkText = string.Empty;

        /// <summary>
        /// Watermark CSS Class, default value is "watermark"
        /// </summary>
        private string _watermarkCssClass = "watermark";

        /// <summary>
        /// The need show loading
        /// </summary>
        private bool _needShowLoading = true;

        #endregion Fields

        #region Properties

        /// <summary>
        /// Gets or sets a value indicating whether [trim value].
        /// </summary>
        /// <value><c>true</c> if [trim value]; otherwise, <c>false</c>.</value>
        public bool TrimValue { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [need show loading].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [need show loading]; otherwise, <c>false</c>.
        /// </value>
        public bool NeedShowLoading
        {
            get
            {
                return _needShowLoading;
            }

            set
            {
                _needShowLoading = value;
            }
        }

        /// <summary>
        /// Gets or sets the callback javascript function
        /// </summary>
        public string CallbackFailFunction
        {
            get
            {
                return _callbackFailFunction;
            }

            set
            {
                _callbackFailFunction = value;
            }
        }

        /// <summary>
        /// Gets or sets check UserControl's values validation function
        /// </summary>
        public string CheckControlValueValidateFunction { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether check custom validation or not
        /// </summary>
        public bool CheckCustomValidation
        {
            get
            {
                return _checkCustomValidation;
            }

            set
            {
                _checkCustomValidation = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether client-side validation is enabled..
        /// </summary>
        public string ClientScript
        {
            get
            {
                return _clientScript;
            }

            set
            {
                _clientScript = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the control is visible in client
        /// </summary>
        public bool ClientVisible
        {
            get
            {
                return _clientVisible;
            }

            set
            {
                _clientVisible = value;
            }
        }

        /// <summary>
        /// Gets or sets InstructionAlign
        /// </summary>
        public InstructionAlign InstructionAlign
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets a value indicating whether the control is always editable in client.
        /// </summary>
        public bool IsAlwaysEditable
        {
            get
            {
                return _isAlwaysEditable;
            }

            set
            {
                _isAlwaysEditable = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the UniqueID is fixed(is same with the control ID).
        /// </summary>
        public bool IsFixedUniqueID
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets a value indicating whether the control is hidden in client
        /// </summary>
        public bool IsHidden
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets a value indicating whether custom client validation function.
        /// </summary>
        public string CustomValidationFunction
        {
            get
            {
                return _customValidationFunction;
            }

            set
            {
                _customValidationFunction = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether custom validation message.It is visible when validation is failed
        /// for custom validation
        /// </summary>
        public string CustomValidationMessageKey
        {
            get
            {
                return _customValidationMessageKey;
            }

            set
            {
                _customValidationMessageKey = value;
            }
        }

        /// <summary>
        /// Gets or sets the field alignment
        /// </summary>
        public FieldAlignment FieldAlignment
        {
            get
            {
                return _fieldAlignment;
            }

            set
            {
                _fieldAlignment = value;
            }
        }

        /// <summary>
        /// Gets or sets filter type. A the type of filter to apply, as a comma-separated combination of Numbers, LowercaseLetters, 
        /// UppercaseLetters, and Custom. If Custom is specified, the ValidChars field will be used in addition 
        /// to other settings such as Numbers. 
        /// </summary>
        public FilterTypes FilterType { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether display the error icon.
        /// </summary>
        public bool HideRequireIndicate
        {
            get
            {
                return _hideRequireIndicate;
            }

            set
            {
                _hideRequireIndicate = value;
            }
        }

        /// <summary>
        /// Gets or sets the highlight CSS.
        /// </summary>
        public string HighlightCssClass
        {
            get
            {
                return _highlightCssClass;
            }

            set
            {
                _highlightCssClass = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether control label span is or isn't display
        /// </summary>
        public bool IsDisplayLabel
        {
            get
            {
                return _isDisplayLabel;
            }

            set
            {
                _isDisplayLabel = value;
            }
        }

        /// <summary>
        /// Gets or sets Label
        /// </summary>
        public string Label
        {
            get
            {
                return _label;
            }

            set
            {
                //_label = value;(Updated by Peter 12/6/2007)
                _label = ScriptFilter.FilterScript(value, false);
            }
        }

        /// <summary>
        /// Gets or sets Label Key
        /// </summary>
        public string LabelKey { get; set; }

        /// <summary>
        /// Gets or sets ToolTipLabelKey
        /// </summary>
        public string ToolTipLabelKey
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets a value indicating whether minimum length of the text value of this element.
        /// </summary>
        public int MinLength { get; set; }

        /// <summary>
        /// Override the NamingContainer attribute to fixing the UniqueID if <see cref="IsFixedUniqueID"/> is true.
        /// </summary>
        public override Control NamingContainer
        {
            get
            {
                if (IsFixedUniqueID)
                {
                    return null;
                }
                else
                {
                    return base.NamingContainer;
                }
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether focus is set to the control specified by the System.Web.UI.WebControls.BaseValidator.ControlToValidate property when validation fails.
        /// </summary>
        public virtual bool SetFocusOnError
        {
            get
            {
                return !AccessibilityUtil.AccessibilityEnabled && this._setFocusOnError;
            }

            set
            {
                _setFocusOnError = value;
            }
        }

        /// <summary>
        /// Gets or sets Sub Label
        /// </summary>
        public string SubLabel
        {
            get
            {
                return _subLabel;
            }

            set
            {
                //_subLabel = value;(Updated by Peter 12/6/2007)
                _subLabel = Accela.ACA.Common.Common.ScriptFilter.FilterScript(value, false);
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the tooltip message
        /// </summary>
        public string TooltipMessage { get; set; }

        /// <summary>
        /// Gets or sets valid chars. A string consisting of all characters considered valid for the text field, if "Custom" is specified as the filter type. 
        /// Otherwise this parameter is ignored.
        /// </summary>
        public string ValidChars
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
        /// Gets or sets a value for validation. required;MaxLength;MinLength;
        /// </summary>
        public virtual string Validate
        {
            get
            {
                return _validate;
            }

            set
            {
                this._checkRequired = false;
                this._checkMinLength = false;
                this._checkMaxLength = false;
                this._checkCustomValidation = false;
                _validate = value;

                if (string.IsNullOrEmpty(value))
                {
                    return;
                }

                string[] parms = value.Split(';'); 

                foreach (string parm in parms)
                {
                    //if (parm.ToLower().Equals("required"))
                    if (parm.Equals("required", StringComparison.InvariantCultureIgnoreCase))
                    {
                        this._checkRequired = true;
                    }

                    //if (parm.ToLower().Equals("minlength"))
                    if (parm.Equals("minlength", StringComparison.InvariantCultureIgnoreCase))
                    {
                        this._checkMinLength = true;
                    }

                    //if (parm.ToLower().Equals("maxlength"))
                    if (parm.Equals("maxlength", StringComparison.InvariantCultureIgnoreCase))
                    {
                        this._checkMaxLength = true;
                    }

                    if (parm.Equals("customvalidation", StringComparison.InvariantCultureIgnoreCase))
                    {
                        this._checkCustomValidation = true;
                    }
                }
            }
        }

        /// <summary>
        /// Gets or sets a value for ValidationExpression.
        /// </summary>
        public virtual string ValidationExpression
        {
            get
            {
                return _validationExpression;
            }

            set
            {
                _validationExpression = value;
            }
        }

        /// <summary>
        /// Gets or sets a value for  Position ID.
        /// </summary>
        public AutoFillPosition PositionID
        {
            get
            {
                return _positionID;
            }

            set
            {
                _positionID = value;
            }
        }

        /// <summary>
        /// Gets or sets auto fill type.
        /// </summary>
        public AutoFillType AutoFillType
        {
            get
            {
                return _autoFillType;
            }

            set
            {
                _autoFillType = value;
            }
        }

        /// <summary>
        /// Gets or sets control's Layout Type
        /// </summary>
        public ControlLayoutType LayoutType
        {
            get
            {
                return this._layoutType;
            }

            set
            {
                _layoutType = value;
            }
        }

        /// <summary>
        /// Gets or sets Field's Unit Type
        /// </summary>
        public string FieldUnit
        {
            get
            {
                return this._fieldUnit;
            }

            set
            {
                this._fieldUnit = value;
            }
        }

        /// <summary>
        /// Gets or sets Unit Width.
        /// </summary>
        public string UnitWidth
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets Field label's width
        /// </summary>
        public string LabelWidth
        {
            get
            {
                return this._labelWidth;
            }

            set
            {
                this._labelWidth = value;
            }
        }

        /// <summary>
        /// Gets or sets Field Value's width
        /// </summary>
        public string FieldWidth
        {
            get
            {
                return this._fieldWidth;
            }

            set
            {
                this._fieldWidth = value;
            }
        }

        /// <summary>
        /// Gets or sets Watermark Text
        /// </summary>
        public string WatermarkText
        {
            get
            {
                //(char)9;Used to distinguish 'Watermark text' and 'Fields value'
                if (!string.IsNullOrEmpty(this._watermarkText) && !AccelaControlRender.IsAdminRender(this))
                {
                    return this._watermarkText + (char)9;
                }
                else
                {
                    return this._watermarkText;
                }
            }

            set
            {
                this._watermarkText = value;
            }
        }

        /// <summary>
        /// Gets or sets Watermark CSS Class
        /// </summary>
        public string WatermarkCssClass
        {
            get
            {
                return this._watermarkCssClass;
            }

            set
            {
                this._watermarkCssClass = value;
            }
        }

        /// <summary>
        /// Gets or sets the extend control html, this html will render before the help icon.
        /// </summary>
        public string ExtendControlHtml
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the double value.
        /// set the property will format the property Text.
        /// </summary>
        /// <value>
        /// The double value.
        /// </value>
        public double? DoubleValue
        {
            get
            {
                return CommonUtil.GetDoubleValue(Text);
            }

            set
            {
                this.Text = value == null ? string.Empty : I18nNumberUtil.FormatNumberForInput(value.Value);
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether [enable SoundEX search].
        /// </summary>
        /// <value><c>true</c> if [enable SoundEX search]; otherwise, <c>false</c>.</value>
        public bool EnableSoundexSearch
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets a value indicating whether current textbox is used for the special validation logic.
        /// The validation logic implemented by the hidden TextBox with a required validator, for Section validation and Form validation.
        /// </summary>
        public bool ValidationByHiddenTextBox 
        { 
            get; 
            set; 
        }

        /// <summary>
        /// Gets or sets a value indicating whether the textbox is readonly.
        /// To resolve the readonly value lost issue(#53650).
        /// </summary>
        public new bool ReadOnly
        {
            get
            {
                return Convert.ToBoolean(ViewState["NewReadOnly"]);
            }

            set
            {
                if (value)
                {
                    Attributes.Add("readonly", "readonly");
                }
                else
                {
                    Attributes.Remove("readonly");
                }

                ViewState["NewReadOnly"] = value;
            }
        }

        /// <summary>
        /// Gets or sets the additional expression validate message. It is append to Expression Validator after the "CustomValidationMessage".
        /// </summary>
        /// <value>The customize expression message.</value>
        protected string AddionalExpressionValidateMessage
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets Editable Class.
        /// </summary>
        protected string CssClassForEdit
        {
            get
            {
                return ViewState["CssClassForEdit"] as string;
            }

            set
            {
                ViewState["CssClassForEdit"] = value;
            }
        }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Clear current control value
        /// </summary>
        public virtual void ClearValue()
        {
            this.Text = string.Empty;
        }

        /// <summary>
        /// Disable current control to make it readonly.
        /// </summary>
        public virtual void DisableEdit()
        {
            this.ReadOnly = true;

            // first time,stores original CssClass to cache to viewstate.
            if (CssClassForEdit == null)
            {
                CssClassForEdit = this.CssClass;
            }

            // updated CssClass to append readonly css.
            this.CssClass = CssClassForEdit + " " + WebControlConstant.CSS_CLASS_READONLY;
        }

        /// <summary>
        /// Enable current control to make it be editable.
        /// </summary>
        public virtual void EnableEdit()
        {
            this.ReadOnly = false;

            // restore the CssClass to original CssClass from ViewState
            if (CssClassForEdit != null)
            {
                this.CssClass = CssClassForEdit;
            }
        }

        /// <summary>
        /// Get control id.
        /// </summary>
        /// <returns>control id</returns>
        public string GetControlID()
        {
            return ClientID;
        }

        /// <summary>
        /// get default label
        /// </summary>
        /// <returns>default label</returns>
        public string GetDefaultLabel()
        {
            if (!string.IsNullOrEmpty(LabelKey))
            {
                return Accela.ACA.Common.Common.ScriptFilter.FilterScript(LabelConvertUtil.GetGUITextByKey(LabelKey), false);
            }

            return string.Empty;
        }

        /// <summary>
        /// get default language sub label
        /// </summary>
        /// <returns>default language sub label</returns>
        public string GetDefaultLanguageSubLabel()
        {
            if (!string.IsNullOrEmpty(LabelKey))
            {
                return Accela.ACA.Common.Common.ScriptFilter.FilterScript(LabelConvertUtil.GetDefaultLanguageGlobalTextByKey(LabelKey + WebControlConstant.LABEL_KEY_SUB_SUFFIX), false);
            }

            return string.Empty;
        }

        /// <summary>
        /// get default language text
        /// </summary>
        /// <returns>default language text</returns>
        public string GetDefaultLanguageText()
        {
            if (!string.IsNullOrEmpty(LabelKey))
            {
                return Accela.ACA.Common.Common.ScriptFilter.FilterScript(LabelConvertUtil.GetDefaultLanguageTextByKey(LabelKey, GetModuleName()), false);
            }

            return string.Empty;
        }

        /// <summary>
        /// get default sub label
        /// </summary>
        /// <returns>default sub label</returns>
        public string GetDefaultSubLabel()
        {
            if (!string.IsNullOrEmpty(LabelKey))
            {
                return Accela.ACA.Common.Common.ScriptFilter.FilterScript(LabelConvertUtil.GetGUITextByKey(LabelKey + WebControlConstant.LABEL_KEY_SUB_SUFFIX), false);
            }

            return string.Empty;
        }

        /// <summary>
        /// get default label
        /// </summary>
        /// <returns>default label</returns>
        public string GetLabel()
        {
            if (string.IsNullOrEmpty(Label) && !string.IsNullOrEmpty(LabelKey))
            {
                Label = LabelConvertUtil.GetTextByKey(LabelKey, this);
            }

            return Label;
        }

        /// <summary>
        /// get ToolTip label
        /// </summary>
        /// <returns>ToolTip label</returns>
        public string GetToolTipLabel()
        {
            return string.IsNullOrEmpty(ToolTipLabelKey) ? string.Empty : LabelConvertUtil.GetTextByKey(ToolTipLabelKey, this);
        }

        /// <summary>
        /// get sub label
        /// </summary>
        /// <returns>sub label string</returns>
        public string GetSubLabel()
        {
            if (!string.IsNullOrEmpty(LabelKey))
            {
                SubLabel = LabelConvertUtil.GetTextByKey(LabelKey + WebControlConstant.LABEL_KEY_SUB_SUFFIX, this);
            }

            return SubLabel;
        }

        /// <summary>
        /// Gets value from current control.
        /// if subclass is textbox, it returns Text value.
        /// if subclass is RadioButtonList or dropdownlist, it returns selected value.
        /// </summary>
        /// <returns>selected value or Text of control.</returns>
        public string GetValue()
        {
            return TrimValue ? Text.Trim() : Text;
        }

        /// <summary>
        /// Gets the invariant double text.
        /// </summary>
        /// <returns>invariant double text.</returns>
        public string GetInvariantDoubleText()
        {
            double? doubleValue = DoubleValue;
            string result = doubleValue == null ? string.Empty : I18nNumberUtil.ConvertNumberToInvariantString(doubleValue.Value);
            return result;
        }

        /// <summary>
        /// Gets the invariant decimal text
        /// </summary>
        /// <returns>invariant decimal text.</returns>
        public string GetInvariantDecimalText()
        {
            decimal parseDec;

            if (decimal.TryParse(GetValue(), NumberStyles.Any, CultureInfo.CurrentUICulture, out parseDec))
            {
                return parseDec.ToString(CultureInfo.InvariantCulture);
            }

            return string.Empty;
        }

        /// <summary>
        /// Gets watermark text
        /// </summary>
        /// <returns>watermark text</returns>
        public string GetWatermarkText()
        {
            if (!string.IsNullOrEmpty(LabelKey))
            {
                string _tempval = LabelConvertUtil.GetTextByKey(LabelKey + WebControlConstant.LABEL_KEY_WATERMARK_SUFFIX, this);
                WatermarkText = System.Web.HttpUtility.HtmlDecode(_tempval);
            }

            return WatermarkText;
        }

        /// <summary>
        /// Initial ajax extender control.
        /// </summary>
        public void InitExtenderControl()
        {
            if (!this.IsHidden || ValidationByHiddenTextBox)
            {
                RequiredValidator();
                LengthValidator();
                CustomValidator();
                ExpressionValidator();
                WatermarkExtender();
            }

            if (FilterType != 0)
            {
                FilteredTextBoxExtender();
            }
        }

        /// <summary>
        /// if to hide the required indicate
        /// </summary>
        /// <returns>Hide the required indicate or not</returns>
        public bool IsHideRequireIndicate()
        {
            return _hideRequireIndicate;
        }

        /// <summary>
        /// Check whether this control is required input.
        /// </summary>
        /// <returns>Whether this control is required input</returns>
        public bool IsRequired()
        {
            return _checkRequired;
        }

        /// <summary>
        /// render element
        /// </summary>
        /// <param name="w">HtmlTextWriter object</param>
        public void RenderElement(HtmlTextWriter w)
        {
            //text box's title need add water mark value.
            string[] titleValue = { ControlRenderUtil.GetToolTip(this), WatermarkText };
            string toolTip = DataUtil.ConcatStringWithSplitChar(titleValue, ACAConstant.BLANK);
            this.ToolTip = string.IsNullOrEmpty(toolTip) || (NamingContainer is AccelaMultipleControl) ? ToolTip : toolTip;

            if (FieldAlignment == FieldAlignment.RTL)
            {
                Attributes.CssStyle.Add(HtmlTextWriterStyle.Direction, "rtl");
            }

            base.Render(w);
            this.RenderChildren(w);
        }

        /// <summary>
        /// Sets value to current control.
        /// if subclass is textbox, it set value to Text.
        /// if subclass is RadioButtonList or dropdownlist, it set value as selected.
        /// </summary>
        /// <param name="value">a value to be set to current control.</param>
        public void SetValue(string value)
        {
            this.Text = value;
        }

        /// <summary>
        /// Add expand attribute
        /// </summary>
        /// <param name="control">Control object</param>
        /// <param name="page">Page that the control belong to</param>
        /// <param name="controlId">control id.</param>
        /// <param name="attributeName">attribute name</param>
        /// <param name="attributeValue">attribute value</param>
        /// <param name="encode">is encoded or not</param>
        internal static void AddExpandoAttribute(Control control, Page page, string controlId, string attributeName, string attributeValue, bool encode)
        {
            ScriptManager.RegisterExpandoAttribute(control, controlId, attributeName, attributeValue, encode);
        }

        /// <summary>
        /// New a instance of required field validator,it can be override in subclass to new a instance of other validator
        /// </summary>
        /// <returns>BaseValidator object</returns>
        protected virtual BaseValidator CreateRequireValidator()
        {
            return new RequiredFieldValidator();
        }

        /// <summary>
        /// Create Validator Callback Extender for control
        /// </summary>
        /// <param name="targetControlID">target control id</param>
        protected void CreateValidatorCallbackExtender(string targetControlID)
        {
            CreateValidatorCallbackExtender(targetControlID, null);
        }

        /// <summary>
        /// Create Validator Callback Extender for control
        /// </summary>
        /// <param name="targetControlID">target control id</param>
        /// <param name="checkControlValueValidateFunction">check control value validate function name</param>
        protected void CreateValidatorCallbackExtender(string targetControlID, string checkControlValueValidateFunction)
        {
            ValidatorCallbackExtender vce4Required = new ValidatorCallbackExtender();
            vce4Required.TargetControlID = targetControlID;
            vce4Required.HighlightCssClass = _highlightCssClass;
            vce4Required.CallbackFailFunction = _callbackFailFunction;
            vce4Required.CallbackControlID = ID;
            vce4Required.CheckControlValueValidateFunction = checkControlValueValidateFunction;
            vce4Required.ValidationByHiddenTextBox = ValidationByHiddenTextBox;
            Controls.Add(vce4Required);
        }

        /// <summary>
        /// custom validation for unknown validator
        /// </summary>
        /// <returns>CustomValidator control</returns>
        protected virtual CustomValidator CustomValidator()
        {
            if (_checkCustomValidation && !string.IsNullOrEmpty(_customValidationFunction))
            {
                CustomValidator customVad = new CustomValidator();
                customVad.ID = ID + "_custom_vad";
                customVad.ControlToValidate = ID;

                customVad.SetFocusOnError = this.SetFocusOnError;
                customVad.Display = ValidatorDisplay.None;
                customVad.ClientValidationFunction = this._customValidationFunction;
                customVad.EnableClientScript = !ClientScript.Equals("false", StringComparison.InvariantCultureIgnoreCase);

                string erroMessage = LabelConvertUtil.GetGlobalTextByKey(CustomValidationMessageKey);
                customVad.ErrorMessage = erroMessage.Replace("'", "\\'");
                Controls.Add(customVad);
                CreateValidatorCallbackExtender(customVad.ID);
                return customVad;
            }

            return null;
        }

        /// <summary>
        /// validate the illegal character in the textbox field.
        /// </summary>
        /// <returns>RegularExpressionValidator object</returns>
        protected virtual RegularExpressionValidator ExpressionValidator()
        {
            if (!string.IsNullOrEmpty(ValidationExpression))
            {
                RegularExpressionValidator expressionVad = new RegularExpressionValidator();
                expressionVad.ID = ID + "_expression_vad";
                expressionVad.ControlToValidate = ID;
                expressionVad.ValidationExpression = ValidationExpression;
                expressionVad.ErrorMessage = LabelConvertUtil.GetGlobalTextByKey(CustomValidationMessageKey).Replace("'", "\\'") + AddionalExpressionValidateMessage;
                expressionVad.SetFocusOnError = this.SetFocusOnError;
                expressionVad.Display = ValidatorDisplay.None;
                expressionVad.EnableClientScript = !ClientScript.Equals("false", StringComparison.InvariantCultureIgnoreCase);
                Controls.Add(expressionVad);

                CreateValidatorCallbackExtender(expressionVad.ID);

                return expressionVad;
            }

            return null;
        }

        /// <summary>
        /// MaxLength Validator
        /// </summary>
        /// <returns>TextLengthValidator object</returns>
        protected virtual TextLengthValidator LengthValidator()
        {
            if (this._checkMaxLength || this._checkMinLength)
            {
                int maxLength = MaxLength;

                if (this is AccelaPhoneText)
                {
                    AccelaPhoneText phoneText = (AccelaPhoneText)this;
                    maxLength = phoneText.PhoneMaxLength;
                }

                if (this is AccelaZipText)
                {
                    AccelaZipText zipText = (AccelaZipText)this;
                    maxLength = string.IsNullOrEmpty(zipText.ZipMaskFromAA) ? zipText.ZipMaxLength : zipText.ZipMaskFromAA.Length;
                }

                TextLengthValidator lengthVad = new TextLengthValidator();
                lengthVad.ID = ID + "_length_vad";
                lengthVad.ControlToValidate = ID;
                lengthVad.MaxLength = maxLength;
                lengthVad.MinLength = MinLength;
                lengthVad.SetFocusOnError = this.SetFocusOnError;

                lengthVad.DisplayCharactersEntered = true;
                lengthVad.Display = ValidatorDisplay.None;
                string erroMessage = string.Empty;

                //--"Must be {0}-{1} characters"
                erroMessage = string.Format(LabelConvertUtil.GetTextByKey("ACA_AccelaTextBox_MustBeBetweenMinMaxLenth", GetModuleName()), MinLength, maxLength);
                if (this._checkMaxLength && !this._checkMinLength)
                {
                    //erroMessage = "Must be less than " + MaxLength + " characters";
                    //--"Must be less than {0} characters"
                    erroMessage = string.Format(LabelConvertUtil.GetTextByKey("ACA_AccelaTextBox_MustBeLessThanMaxLength", GetModuleName()), maxLength);
                }
                else if (!this._checkMaxLength && this._checkMinLength)
                {
                    //erroMessage = "No less than " + MinLength + " characters";
                    //--"No less than {0} characters"
                    int actualMinLength = MinLength;

                    if (this is AccelaPhoneText)
                    {
                        string phoneMask = (this as AccelaPhoneText).Mask;
                        actualMinLength = phoneMask.Replace("-", string.Empty).Replace(" ", string.Empty).Length;
                    }

                    erroMessage = string.Format(LabelConvertUtil.GetTextByKey("ACA_AccelaTextBox_NoLessThanMinLength", GetModuleName()), actualMinLength);
                }

                if (MinLength == maxLength && MinLength != 0)
                {
                    if (this is AccelaNumberText)
                    {
                        //erroMessage = "Must be " + MinLength + " digits";
                        //--"Must be {0} digits"
                        erroMessage = string.Format(LabelConvertUtil.GetTextByKey("ACA_AccelaTextBox_MustBeXDigits", GetModuleName()), MinLength);
                    }
                    else
                    {
                        //erroMessage = "Must be " + MinLength + " characters";
                        //--"Must be {0} characters"
                        erroMessage = string.Format(LabelConvertUtil.GetTextByKey("ACA_AccelaTextBox_MustBeXChars", GetModuleName()), MinLength);
                    }
                }

                lengthVad.ErrorMessage = erroMessage.Replace("'", "\\'");
                lengthVad.EnableClientScript = !ClientScript.Equals("false", StringComparison.InvariantCultureIgnoreCase);
                Controls.Add(lengthVad);

                CreateValidatorCallbackExtender(lengthVad.ID);

                return lengthVad;
            }

            return null;
        }

        /// <summary>
        /// Raises the System.Web.UI.Control.PreRender event.
        /// </summary>
        /// <param name="e">A System.EventArgs that contains the event data.</param>
        protected override void OnPreRender(EventArgs e)
        {
            GetWatermarkText();
            bool isAdmin = AccelaControlRender.IsAdminRender(this);

            //The following attributes are only used to control the Admin Console display
            if (isAdmin)
            {
                Attributes.Add("watermarkText", WatermarkText);
                Attributes.Add("watermarkCssClass", WatermarkCssClass);
            }

            if (ValidationByHiddenTextBox)
            {
                Attributes.Add("validationByHiddenTextBox", "true");
            }

            if (_needShowLoading && AutoPostBack && !isAdmin
                && (string.IsNullOrEmpty(Attributes["onchange"]) || !Attributes["onchange"].Contains(JS_SHOW_LOADING)))
            {
                Attributes.Add("onchange", JS_SHOW_LOADING + Attributes["onchange"]);
            }

            if (TrimValue)
            {
                Attributes.Add("onblur", "javascript:trimInput('" + ClientID + "');");
            }

            IAccelaControlRender render = (IAccelaControlRender)ObjectFactory.GetObject(typeof(IAccelaControlRender), AccelaControlRender.IsAdminRender(this));
            render.OnPreRender(this);

            base.OnPreRender(e);
        }

        /// <summary>
        /// Displays the text-box on the client.
        /// </summary>
        /// <param name="writer">A System.Web.UI.HtmlTextWriter that contains the output stream to render on the client.</param>
        protected override void Render(System.Web.UI.HtmlTextWriter writer)
        {
            if (DesignMode)
            {
                return;
            }

            if (this is AccelaNumberText && this.Parent is AccelaPhoneText) 
            {
                // for the country code control
                this.RenderElement(writer);
            }
            else
            {
                AccelaControlRender.Render(writer, this);
            }
        }

        /// <summary>
        /// add required validator and validator callback extender 
        /// </summary>
        /// <param name="baseReq">validator control</param>
        /// <param name="id">validator control id</param>
        /// <param name="errorMessage">validator's error message</param>
        protected void RequireValidator(BaseValidator baseReq, string id, string errorMessage)
        {
            baseReq.ControlToValidate = ID;
            baseReq.ID = id;
            baseReq.SetFocusOnError = this.SetFocusOnError;
            baseReq.Display = ValidatorDisplay.None;
            baseReq.ErrorMessage = errorMessage.Replace("'", "\\'");
            baseReq.EnableClientScript = !ClientScript.Equals("false", StringComparison.InvariantCultureIgnoreCase);
            Controls.Add(baseReq);

            CreateValidatorCallbackExtender(baseReq.ID, (baseReq is RequiredFieldValidator || baseReq is NumberRequiredValidator) ? CheckControlValueValidateFunction : null);
        }

        /// <summary>
        /// Add watermark extender
        /// </summary>
        protected void WatermarkExtender()
        {
            if (!string.IsNullOrEmpty(this.WatermarkText))
            {
                TextBoxWatermarkExtender watermarkExd = new TextBoxWatermarkExtender();
                watermarkExd.ID = this.ClientID + "_watermark_exd";
                watermarkExd.BehaviorID = this.ClientID + "_watermark_bhv";
                watermarkExd.TargetControlID = this.ID;
                watermarkExd.WatermarkCssClass = this.WatermarkCssClass;
                watermarkExd.WatermarkText = this.WatermarkText;
                Controls.Add(watermarkExd);
            }
        }

        /// <summary>
        /// Required Validator
        /// </summary>
        protected virtual void RequiredValidator()
        {
            if (this._checkRequired)
            {
                RequireValidator(CreateRequireValidator(), ID + "_baseReq", string.Empty);
            }
        }

        /// <summary>
        /// Restores view-state information from a previous request that was saved with the <see cref="M:System.Web.UI.WebControls.WebControl.SaveViewState" /> method.
        /// </summary>
        /// <param name="savedState">An object that represents the control state to restore.</param>
        protected override void LoadViewState(object savedState)
        {
            base.LoadViewState(savedState);
            string scripts = this.Attributes["onchange"];

            if (!string.IsNullOrEmpty(scripts) && scripts.IndexOf("RunExpression") > -1)
            {
                this.Attributes.Remove("onchange");
            }
        }

        /// <summary>
        /// add filtered text box extender
        /// </summary>
        private void FilteredTextBoxExtender()
        {
            FilteredTextBoxExtender filterExt = new FilteredTextBoxExtender();
            filterExt.ID = ID + "_filter_exd";
            filterExt.TargetControlID = ID;

            filterExt.ValidChars = ValidChars;

            filterExt.FilterType = this.FilterType;
            Controls.Add(filterExt);
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

        #endregion Methods
    }
}