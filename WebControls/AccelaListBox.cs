#region Header

/**
 * <pre>
 *
 *  Accela
 *  File: AccelaListBox.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2007-2014
 *
 *  Description:
 *
 *  Notes:
 * $Id: AccelaListBox.cs 276801 2014-08-07 02:14:39Z ACHIEVO\james.shi $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using Accela.ACA.Common;
using Accela.ACA.Common.Util;
using Accela.Web.Controls.ControlRender;
using AjaxControlToolkit;

namespace Accela.Web.Controls
{
    /// <summary>
    /// provide a ListBox to select item, support select multiple items.
    /// </summary>
    public class AccelaListBox : ListBox, IAccelaControl
    {
        #region Fields

        /// <summary>
        /// Default ListBox CSS
        /// </summary>
        private const string DEFAULT_LISTBOX_CSS = "LISTBOX";

        /// <summary>
        /// call back fail function, default value is "doErrorCallbackFun".
        /// </summary>
        private string _callbackFailFunction = "doErrorCallbackFun";

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
        /// field alignment, default value is left to right
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
        /// ValidatorCallbackExtender object
        /// </summary>
        private ValidatorCallbackExtender _vce = null;

        /// <summary>
        /// layout Type
        /// </summary>
        private ControlLayoutType _layoutType = ControlLayoutType.Vertical;

        /// <summary>
        /// the unit Type
        /// </summary>
        private string _fieldUnit = string.Empty;

        /// <summary>
        /// the label width
        /// </summary>
        private string _labelWidth = string.Empty;

        /// <summary>
        /// the field width
        /// </summary>
        private string _fieldWidth = string.Empty;

        #endregion Fields

        /// <summary>
        /// Initializes a new instance of the <see cref="AccelaListBox"/> class.
        /// </summary>
        public AccelaListBox()
        {
            IsAlwaysEditable = false;
        }

        #region Properties

        /// <summary>
        /// Gets or sets the callback javascript function.
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
        /// Gets or sets check UserControl's values validation function.
        /// </summary>
        public string CheckControlValueValidateFunction { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether client-side validation is enabled.
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
        /// Gets or sets a value indicating whether the control is visible in client.
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
        public bool IsAlwaysEditable { get; set; }

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
                _label = ACA.Common.Common.ScriptFilter.FilterScript(value, false);
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
        /// Gets or sets the tool tip label key.
        /// </summary>
        /// <value>The tool tip label key.</value>
        public string ToolTipLabelKey
        {
            get;
            set;
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
        /// Gets or sets max length for standard choice item 
        /// </summary>
        public int MaxValueLength
        {
            get;
            set;
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

        /// <summary>
        /// Gets or sets the option group value.
        /// </summary>
        /// <value>The option group value.</value>
        private string OptionGroupValue
        {
            get
            {
                string s = (string)ViewState["OptionGroupValue"];

                return (s == null) ? ACAConstant.OPTION_GROUP : s;
            }

            set
            {
                ViewState["OptionGroupValue"] = value;
            }
        }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Clear current control value
        /// </summary>
        public void ClearValue()
        {
            if (this.Items.Count > 0)
            {
                foreach (ListItem item in Items)
                {
                    item.Selected = false;
                }
            }
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
        /// Gets value with separate char \f from current control.
        /// </summary>
        /// <returns>selected values of selected.</returns>
        public string GetValue()
        {
            var selectedValues = GetSelectedValue();
            var result = string.Join(ACAConstant.SPLIT_CHAR.ToString(), selectedValues.ToArray());
            return result;
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
            //if text area control, it need add element "label".
            w.Write(string.Format("<label for={0}></label>", ClientID));
            string toolTip = ControlRenderUtil.GetToolTip(this);
            ToolTip = string.IsNullOrEmpty(toolTip) ? ToolTip : toolTip;

            if (DesignMode)
            {
                return;
            }

            if (FieldAlignment == FieldAlignment.RTL)
            {
                Attributes.CssStyle.Add(HtmlTextWriterStyle.Direction, "rtl");
            }

            base.Render(w);
            this.RenderChildren(w);
        }

        /// <summary>
        /// Sets value to current control.
        /// </summary>
        /// <param name="value">A value to be set to current control (note: the real values are separated by char \f).</param>
        public void SetValue(string value)
        {
            var selectedValues = string.IsNullOrEmpty(value) ? null : value.Split(ACAConstant.SPLIT_CHAR);

            if (selectedValues != null)
            {
                var selectedValueList = new List<string>();
                selectedValueList.AddRange(selectedValues);

                foreach (ListItem item in Items)
                {
                    item.Selected = selectedValueList.Contains(item.Value) ? true : false;
                }
            }
        }

        /// <summary>
        /// Gets selected value from current control.
        /// </summary>
        /// <returns>selected values of selected.</returns>
        public List<string> GetSelectedValue()
        {
            List<string> result = new List<string>();

            foreach (ListItem item in Items)
            {
                if (item.Selected)
                {
                    result.Add(item.Value);
                }
            }

            return result;
        }

        /// <summary>
        /// Sets value as selected.
        /// </summary>
        /// <param name="value">A value to be set to current control.</param>
        public void SetSelectedValue(IList<string> value)
        {
            if (Items.Count == 0)
            {
                return;
            }

            if (value != null && value.Count > 0)
            {
                foreach (ListItem item in Items)
                {
                    if (value.Contains(item.Value))
                    {
                        item.Selected = true;
                    }
                    else
                    {
                        item.Selected = false;
                    }
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
            IAccelaControlRender render = (IAccelaControlRender)ObjectFactory.GetObject(typeof(IAccelaControlRender), AccelaControlRender.IsAdminRender(this));
            render.OnPreRender(this);

            foreach (ListItem item in Items)
            {
                // set an attribute to pop up a meesage indicating the item's value when mousing on each item.
                item.Attributes.Add("title", item.Value);
            }

            base.OnPreRender(e);
        }

        /// <summary>
        /// Displays the drop down list on the client.
        /// </summary>
        /// <param name="writer">A System.Web.UI.HtmlTextWriter that contains the output stream to render on the client.</param>
        protected override void Render(HtmlTextWriter writer)
        {
            this.CssClass += " " + DEFAULT_LISTBOX_CSS;

            if (DesignMode)
            {
                base.Render(writer);
                return;
            }

            AccelaControlRender.Render(writer, this);
        }

        /// <summary>
        /// load view state.
        /// </summary>
        /// <param name="savedState">saved view state</param>
        protected override void LoadViewState(object savedState)
        {
            base.LoadViewState(savedState);
            string scripts = this.Attributes["onchange"];

            if (!string.IsNullOrEmpty(scripts) && scripts.IndexOf("RunExpression") > -1)
            {
                this.Attributes.Remove("onchange");
            }

            if (Page.IsPostBack)
            {
                string postBackSoureID = Page.Request.Form[Page.postEventSourceID];
                if (!string.IsNullOrEmpty(postBackSoureID))
                {
                    string postBackClientID = string.Join(ACAConstant.SPLIT_CHAR5, postBackSoureID.Split(ACAConstant.SPLIT_CHAR6));

                    if (postBackClientID == ClientID && AccessibilityUtil.AccessibilityEnabled)
                    {
                        Focus();
                    }
                }
            }
        }

        /// <summary>
        /// render dropdown list contents to current write.
        /// </summary>
        /// <param name="writer">the HtmlTextWriter writer</param>
        protected override void RenderContents(HtmlTextWriter writer)
        {
            // if or not display optionGroup's EndTag
            bool writerEndTag = false;

            foreach (ListItem li in this.Items)
            {
                // if not set optgroup property then render option
                if (li.Value != this.OptionGroupValue)
                {
                    // render option
                    RenderListItem(li, writer);
                }
                else
                {
                    if (writerEndTag)
                    {
                        // display OptionGroup's EndTag
                        RenderOptionGroupEndTag(writer);
                    }
                    else
                    {
                        writerEndTag = true;
                    }

                    // display OptionGroup's BeginTag
                    RenderOptionGroupBeginTag(li, writer);
                }
            }

            if (writerEndTag)
            {
                // display OptionGroup's EndTag
                RenderOptionGroupEndTag(writer);
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
            if (DesignMode)
            {
                return;
            }

            if (_checkRequired)
            {
                RequiredFieldValidator req = new RequiredFieldValidator();
                req.ControlToValidate = this.ID;
                req.ID = this.ID + "_req";
                req.Display = ValidatorDisplay.None;
                req.EnableClientScript = !ClientScript.Equals("false", StringComparison.InvariantCultureIgnoreCase);
                req.SetFocusOnError = this.SetFocusOnError;
                this.Controls.Add(req);

                _vce = new ValidatorCallbackExtender();
                _vce.TargetControlID = req.ID;
                _vce.HighlightCssClass = this._highlightCssClass;
                _vce.CallbackFailFunction = this._callbackFailFunction;
                _vce.CheckControlValueValidateFunction = CheckControlValueValidateFunction;
                _vce.CallbackControlID = this.ID;
                this.Controls.Add(_vce);
            }
        }

        /// <summary>
        /// Display OptionGroup's BeginTag
        /// </summary>
        /// <param name="li">the ListItem</param>
        /// <param name="writer">the HtmlTextWriter writer</param>
        private void RenderOptionGroupBeginTag(ListItem li, HtmlTextWriter writer)
        {
            writer.WriteBeginTag(ACAConstant.OPTION_GROUP);
            writer.WriteAttribute("label", li.Text);

            foreach (string key in li.Attributes.Keys)
            {
                writer.WriteAttribute(key, li.Attributes[key]);
            }

            writer.Write(HtmlTextWriter.TagRightChar);
            writer.WriteLine();
        }

        /// <summary>
        /// Display OptionGroup's EndTag
        /// </summary>
        /// <param name="writer">the HtmlTextWriter writer</param>
        private void RenderOptionGroupEndTag(HtmlTextWriter writer)
        {
            writer.WriteEndTag(ACAConstant.OPTION_GROUP);
            writer.WriteLine();
        }

        /// <summary>
        /// Display Option
        /// </summary>
        /// <param name="li">the ListItem</param>
        /// <param name="writer">the HtmlTextWriter writer</param>
        private void RenderListItem(ListItem li, HtmlTextWriter writer)
        {
            writer.WriteBeginTag("option");
            writer.WriteAttribute("value", li.Value, true);

            if (li.Selected)
            {
                writer.WriteAttribute("selected", "selected", false);
            }

            foreach (string key in li.Attributes.Keys)
            {
                writer.WriteAttribute(key, li.Attributes[key]);
            }

            writer.Write(HtmlTextWriter.TagRightChar);
            HttpUtility.HtmlEncode(li.Text, writer);
            writer.WriteEndTag("option");
            writer.WriteLine();
        }

        #endregion Methods
    }
}