#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: AccelaCheckBox.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2008-2014
 *
 *  Description:
 *
 *  Notes:
 * $Id: AccelaCheckBox.cs 278304 2014-09-01 09:48:24Z ACHIEVO\james.shi $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using System;
using System.ComponentModel;
using System.IO;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;

using Accela.ACA.Common;
using Accela.ACA.Common.Util;
using Accela.Web.Controls.ControlRender;

using AjaxControlToolkit;

namespace Accela.Web.Controls
{
    /// <summary>
    /// Provide a check box
    /// </summary>
    public class AccelaCheckBox : CheckBox, IAccelaControl
    {
        #region Fields

        /// <summary>
        /// call back fail function, default value is "doErrorCallbackFun".
        /// </summary>
        private string _callbackFailFunction = "doErrorCallbackFun";

        /// <summary>
        /// check control value validate function.
        /// </summary>
        private string _checkControlValueValidateFunction;

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
        /// whether set focus on error, default value is true.
        /// </summary>
        private bool _setFocusOnError = true;

        /// <summary>
        /// sub label string.
        /// </summary>
        private string _subLabel;

        /// <summary>
        /// layout Type
        /// </summary>
        private ControlLayoutType _layoutType = ControlLayoutType.Vertical;

        /// <summary>
        /// unit Type string
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
        /// Gets or sets the ToolTipLabelKey
        /// </summary>
        /// <value>The tool tip label key.</value>
        public string ToolTipLabelKey
        {
            get;
            set;
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
        /// Gets or sets a value indicating whether the UniqueID is fixed(is same with the control ID).
        /// </summary>
        public bool IsFixedUniqueID
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets a value indicating whether the control is in ASI Table
        /// </summary>
        public bool IsInASITable
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets check UserControl's values validation function
        /// </summary>
        public string CheckControlValueValidateFunction
        {
            get
            {
                return _checkControlValueValidateFunction;
            }

            set
            {
                _checkControlValueValidateFunction = value;
            }
        }

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
        /// Gets or sets a value indicating whether the control is hidden in client
        /// </summary>
        public bool IsHidden
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the field alignment
        /// </summary>
        public FieldAlignment FieldAlignment
        {
            get
            {
                return this.TextAlign == TextAlign.Right ? FieldAlignment.LTR : FieldAlignment.RTL;
            }

            set
            {
                this.TextAlign = value == FieldAlignment.LTR ? TextAlign.Right : TextAlign.Left;
            }
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
        /// Gets or sets label Key
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
        /// Gets or sets a value indicating whether if it is required
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
        /// Gets or sets sub Label
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
        /// Gets or sets Editable CSS.
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

        /// <summary>
        /// Gets or sets a value indicating whether the control is readonly.
        /// </summary>
        private bool ReadOnly
        {
            get
            {
                return ViewState["IsReadOnly"] != null && bool.Parse(ViewState["IsReadOnly"].ToString());
            }

            set
            {
                ViewState["IsReadOnly"] = value;
            }
        }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Clear current control value
        /// </summary>
        public void ClearValue()
        {
            this.Checked = false;
        }

        /// <summary>
        /// Disable current control to make it readonly.
        /// </summary>
        public void DisableEdit()
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
        public void EnableEdit()
        {
            this.ReadOnly = false;

            // restore the CssClass to original CssClass from ViewState
            if (CssClassForEdit != null)
            {
                this.CssClass = CssClassForEdit;
            }
        }

        /// <summary>
        /// get control id
        /// </summary>
        /// <returns>string control id</returns>
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
            if (!string.IsNullOrEmpty(Text))
            {
                return string.Empty;
            }

            if (!string.IsNullOrEmpty(LabelKey))
            {
                Text = LabelConvertUtil.GetTextByKey(LabelKey, this);
            }

            if (!string.IsNullOrEmpty(Label))
            {
                return Label;
            }

            return string.Empty;
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
            if (this.Checked)
            {
                return "Y";
            }
            else
            {
                return "N";
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
        /// initial daily extender control
        /// </summary>
        public void InitExtenderControl()
        {
            if (!this.IsHidden)
            {
                RequiredValidator();
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
            //get the title value
            string title = ControlRenderUtil.GetToolTip(this);

            if (string.IsNullOrEmpty(GetLabel()))
            {
                string[] toolTip = { Text, title };
                title = DataUtil.ConcatStringWithSplitChar(toolTip, ACAConstant.BLANK);
            }

            title = LabelConvertUtil.RemoveHtmlFormat(title);

            if (!string.IsNullOrEmpty(title))
            {
                InputAttributes.Add("title", title);    
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
            //if ("Y".Equals(value))
            if ("Y".Equals(value, StringComparison.InvariantCulture))
            {
                this.Checked = true;
            }
            else
            {
                this.Checked = false;
            }
        }

        /// <summary>
        /// Creates a new System.Web.UI.ControlCollection object to hold the child controls
        /// (both literal and server) of the server control.
        /// </summary>
        /// <returns>A System.Web.UI.ControlCollection object to contain the current server control's
        /// child server controls.</returns>
        protected override ControlCollection CreateControlCollection()
        {
            return new ControlCollection(this);
        }

        /// <summary>
        /// Raises the System.Web.UI.Control.Initial event.
        /// </summary>
        /// <param name="e">An System.EventArgs object that contains the event data.</param>
        protected override void OnInit(EventArgs e)
        {
            if (string.IsNullOrEmpty(this.CssClass))
            {
                this.CssClass = "aca_checkbox aca_checkbox_fontsize";
            }
            else
            {
                this.CssClass += " aca_checkbox aca_checkbox_fontsize";
            }
        }

        /// <summary>
        /// Raises the System.Web.UI.Control.PreRender event.
        /// </summary>
        /// <param name="e">A System.EventArgs that contains the event data.</param>
        protected override void OnPreRender(EventArgs e)
        {
            IAccelaControlRender render = (IAccelaControlRender)ObjectFactory.GetObject(typeof(IAccelaControlRender), AccelaControlRender.IsAdminRender(this));
            render.OnPreRender(this);

            base.OnPreRender(e);

            if (ReadOnly)
            {
                ScriptManager.RegisterStartupScript(Page, GetType(), "DisableCheckBox_" + this.ClientID, "SetFieldToDisabled('" + this.ClientID + "', true);", true);
            }
        }

        /// <summary>
        /// Displays the checkbox on the client.
        /// </summary>
        /// <param name="writer">A System.Web.UI.HtmlTextWriter that contains the output stream to render on the client.</param>
        protected override void Render(HtmlTextWriter writer)
        {
            if (DesignMode)
            {
                return;
            }

            AccelaControlRender.Render(writer, this);
        }

        /// <summary>
        /// Loads the previously saved view state of the <see cref="T:System.Web.UI.WebControls.CheckBox" /> control.
        /// </summary>
        /// <param name="savedState">An object that contains the saved view state values for the control.</param>
        protected override void LoadViewState(object savedState)
        {
            base.LoadViewState(savedState);
            string scripts = this.Attributes["onclick"];

            if (!string.IsNullOrEmpty(scripts) && scripts.IndexOf("RunExpression") > -1)
            {
                this.Attributes.Remove("onclick");
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
        /// Required Validator
        /// </summary>
        private void RequiredValidator()
        {
            if (_checkRequired)
            {
                CheckBoxRequiredFieldValidator req = new CheckBoxRequiredFieldValidator();
                req.ID = this.ID + "_req";
                req.ControlToValidate = this.ID;
                req.EnableClientScript = !ClientScript.Equals("false", StringComparison.InvariantCultureIgnoreCase);
                req.SetFocusOnError = this.SetFocusOnError;
                this.Controls.Add(req);

                ValidatorCallbackExtender vce1 = new ValidatorCallbackExtender();
                vce1.ID = this.ID + "_vce";
                vce1.TargetControlID = req.ID;
                vce1.HighlightCssClass = this._highlightCssClass;
                vce1.CallbackFailFunction = this._callbackFailFunction;
                vce1.CheckControlValueValidateFunction = CheckControlValueValidateFunction;
                vce1.CallbackControlID = this.ID;
                this.Controls.Add(vce1);
            }
        }

        #endregion Methods
    }
}