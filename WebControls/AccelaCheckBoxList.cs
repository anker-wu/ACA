#region Header

/**
 * <pre>
 *
 *  Accela
 *  File: AccelaCheckBoxList.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2008-2014
 *
 *  Description:
 *
 *  Notes:
 * $Id: AccelaCheckBoxList.cs 276936 2014-08-08 06:37:04Z ACHIEVO\james.shi $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;

using Accela.ACA.Common;
using Accela.ACA.Common.Util;
using Accela.Web.Controls.ControlRender;
using AccelaWebControlExtender;
using AjaxControlToolkit;

namespace Accela.Web.Controls
{
    /// <summary>
    /// provide a check-box list 
    /// </summary>
    public class AccelaCheckBoxList : CheckBoxList, IAccelaControl
    {
        #region Fields

        /// <summary>
        /// call back fail function, default value is "doErrorCallbackFun".
        /// </summary>
        private string _callbackFailFunction = "doErrorCallbackFun";

        /// <summary>
        /// check required or not
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
        /// label key string.
        /// </summary>
        private string _labelKey;

        /// <summary>
        /// checkbox list required field validator
        /// </summary>
        private CheckBoxListRequiredFieldValidator _req = null;

        /// <summary>
        /// whether set focus on error, default value is true.
        /// </summary>
        private bool _setFocusOnError = true;

        /// <summary>
        /// sub label string.
        /// </summary>
        private string _subLabel;

        /// <summary>
        /// ValidatorCallbackExtender object
        /// </summary>
        private ValidatorCallbackExtender _vce = null;

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
        /// field width.
        /// </summary>
        private string _fieldWidth = string.Empty;

        #endregion Fields

        #region Properties

        /// <summary>
        /// Gets or sets ToolTipLabelKey
        /// </summary>
        public string ToolTipLabelKey
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the callback javascript function
        /// </summary>
        public string CallbackFailFunction
        {
            get
            {
                return this._callbackFailFunction;
            }

            set
            {
                this._callbackFailFunction = value;
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
        /// Gets or sets check UserControl's values validation function
        /// </summary>
        public string CheckControlValueValidateFunction { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether client-side validation is enabled..
        /// </summary>
        public string ClientScript
        {
            get
            {
                return this._clientScript;
            }

            set
            {
                this._clientScript = value;
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
        /// Gets or sets a value indicating whether display the error icon.
        /// </summary>
        public bool HideRequireIndicate
        {
            get
            {
                return this._hideRequireIndicate;
            }

            set
            {
                this._hideRequireIndicate = value;
            }
        }

        /// <summary>
        /// Gets or sets the highlight CSS.
        /// </summary>
        public string HighlightCssClass
        {
            get
            {
                return this._highlightCssClass;
            }

            set
            {
                this._highlightCssClass = value;
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
        /// Gets or sets label value
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
                _label = Accela.ACA.Common.Common.ScriptFilter.FilterScript(value, false);
            }
        }

        /// <summary>
        /// Gets or sets Label Key
        /// </summary>
        public string LabelKey
        {
            get
            {
                return _labelKey;
            }

            set
            {
                _labelKey = value;
            }
        }

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
        /// Gets or sets a value indicating whether it is required
        /// </summary>
        public bool Required
        {
            get
            {
                return _checkRequired;
            }

            set
            {
                _checkRequired = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether focus is set to the control specified by the System.Web.UI.WebControls.BaseValidator.ControlToValidate property when validation fails.
        /// </summary>
        public bool SetFocusOnError
        {
            get
            {
                return AccessibilityUtil.AccessibilityEnabled ? false : this._setFocusOnError;
            }

            set
            {
                this._setFocusOnError = value;
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
        /// Gets or sets Field Value's width.
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
        /// Gets or sets the extend control html, this html will render before the help icon.
        /// </summary>
        public string ExtendControlHtml
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets UnitWidth
        /// </summary>
        public string UnitWidth
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets Editable Class.
        /// </summary>
        private string CssClassForEdit
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
        public void ClearValue()
        {
            this.SelectedIndex = -1;
        }

        /// <summary>
        /// Disable current control to make it readonly.
        /// </summary>
        public void DisableEdit()
        {
            this.Enabled = false;

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
        public void EnableEdit()
        {
            this.Enabled = true;

            // restore the CssClass to original CssClass from ViewState
            if (CssClassForEdit != null)
            {
                this.CssClass = CssClassForEdit;
            }
        }

        /// <summary>
        /// Get control id
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
            throw new NotImplementedException();
        }

        /// <summary>
        /// initial daily extender control
        /// </summary>
        public void InitExtenderControl()
        {
            if (!this.IsHidden)
            {
                //RequiredValidator();
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
            this.ToolTip = ControlRenderUtil.GetToolTip(this);

            base.Render(w);

            if (_req != null)
            {
                _req.RenderControl(w);
            }

            if (_vce != null)
            {
                _vce.RenderControl(w);
            }
        }

        /// <summary>
        /// Sets value to current control.
        /// if subclass is textbox, it set value to Text.
        /// if subclass is RadioButtonList or dropdownlist, it set value as selected.
        /// </summary>
        /// <param name="value">a value to be set to current control.</param>
        public void SetValue(string value)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Get select values from list.
        /// </summary>
        /// <returns>selected values of control.</returns>
        public string[] GetSelectValues()
        {
            List<string> result = new List<string>();

            foreach (ListItem item in this.Items)
            {
                if (item.Selected)
                {
                    result.Add(item.Value);
                }
            }

            return result.ToArray();
        }

        /// <summary>
        /// Set values to control.
        /// </summary>
        /// <param name="selectedValues">The selected values.</param>
        public void SetSelectValues(string[] selectedValues)
        {
            foreach (ListItem item in this.Items)
            {
                if (selectedValues.Contains(item.Value))
                {
                    item.Selected = true;
                }
            }
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
        /// Required Validator
        /// </summary>
        public void RequiredValidator()
        {
            if (_checkRequired)
            {
                _req = new CheckBoxListRequiredFieldValidator();
                _req.ControlToValidate = this.ID;
                _req.ID = this.ID + "_req";
                _req.Display = ValidatorDisplay.None;
                _req.EnableClientScript = !ClientScript.Equals("false", StringComparison.InvariantCultureIgnoreCase);
                _req.SetFocusOnError = this.SetFocusOnError;

                this.Parent.Controls.Add(_req);

                _vce = new ValidatorCallbackExtender();
                _vce.ID = this.ID + "_vce";
                _vce.TargetControlID = _req.ID;
                _vce.HighlightCssClass = this._highlightCssClass;
                _vce.CallbackFailFunction = this._callbackFailFunction;
                _vce.CallbackControlID = ID;
                _vce.CheckControlValueValidateFunction = CheckControlValueValidateFunction;
                this.Parent.Controls.Add(_vce);
            }
        }

        /// <summary>
        /// Creates a new System.Web.UI.ControlCollection object to hold the child controls
        /// (both literal and server) of the server control.
        /// </summary>
        /// <returns>A System.Web.UI.ControlCollection object to contain the current server control's
        /// child server controls.</returns>
        protected override System.Web.UI.ControlCollection CreateControlCollection()
        {
            return new ControlCollection(this);
        }

        /// <summary>
        /// Raises the System.Web.UI.Control.PreRender event.
        /// </summary>
        /// <param name="e">A System.EventArgs that contains the event data.</param>
        protected override void OnPreRender(EventArgs e)
        {
            if (string.IsNullOrEmpty(CssClass))
            {
                CssClass = "aca_checkboxlist aca_checkbox_fontsize";
            }
            else if (!CssClass.Contains("aca_checkboxlist aca_checkbox_fontsize"))
            {
                CssClass += " aca_checkboxlist aca_checkbox_fontsize";
            }

            if (RepeatLayout == RepeatLayout.Table)
            {
                Attributes.Add("role", "presentation");
            }

            IAccelaControlRender render = (IAccelaControlRender)ObjectFactory.GetObject(typeof(IAccelaControlRender), AccelaControlRender.IsAdminRender(this));
            render.OnPreRender(this);

            base.OnPreRender(e);
        }

        /// <summary>
        /// Displays the check box on the client.
        /// </summary>
        /// <param name="writer">A System.Web.UI.HtmlTextWriter that contains the output stream to render on the client.</param>
        protected override void Render(System.Web.UI.HtmlTextWriter writer)
        {
            AccelaControlRender.Render(writer, this);

            bool isWebCtrlExtenderRendered = false;
            bool isHelperExtenderRendered = false;
            foreach (Control control in Controls)
            {
                if (isWebCtrlExtenderRendered && isHelperExtenderRendered)
                {
                    break;
                }
                else if (control is AccelaWebControlExtenderExtender && !isWebCtrlExtenderRendered)
                {
                    control.RenderControl(writer);
                    isWebCtrlExtenderRendered = true;
                }
                else if (control is HelperExtender && !isHelperExtenderRendered)
                {
                    control.RenderControl(writer);
                    isHelperExtenderRendered = true;
                }
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

        #endregion Methods
    }
}