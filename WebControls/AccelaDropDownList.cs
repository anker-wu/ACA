#region Header

/**
 * <pre>
 *
 *  Accela
 *  File: AccelaDropDownList.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2008-2014
 *
 *  Description:
 *
 *  Notes:
 * $Id: AccelaDropDownList.cs 276801 2014-08-07 02:14:39Z ACHIEVO\james.shi $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using System;
using System.ComponentModel;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using Accela.ACA.Common;
using Accela.ACA.Common.Util;
using Accela.Web.Controls.ControlRender;

using AjaxControlToolkit;

namespace Accela.Web.Controls
{
    #region Enumerations

    /// <summary>
    /// the type of data source used to fill dropdownlist
    /// </summary>
    public enum DropDownListDataSourceType
    {
        /// <summary>
        /// drop down items from data base
        /// </summary>
        Database = 0,

        /// <summary>
        /// standard choice items
        /// </summary>
        StandardChoice = 1,

        /// <summary>
        /// hard code items
        /// </summary>
        HardCode = 2,

        /// <summary>
        /// permit type
        /// </summary>
        PermitType = 11,

        /// <summary>
        /// standard choice and XPolicy.
        /// </summary>
        STDandXPolicy = 16
    }

    /// <summary>
    /// the type of data used to show.
    /// </summary>
    public enum DropDownListShowType
    {
        /// <summary>
        /// show value
        /// </summary>
        ShowValue = 0,

        /// <summary>
        /// show description
        /// </summary>
        ShowDescription = 1,

        /// <summary>
        /// show value and description
        /// </summary>
        ShowBoth = 2
    }

    #endregion Enumerations

    /// <summary>
    /// provide a drop-down list
    /// </summary>
    public class AccelaDropDownList : DropDownList, IAccelaControl
    {
        #region Fields

        /// <summary>
        /// The JS show loading
        /// </summary>
        private const string JsShowLoading = "var p = new ProcessLoading();p.showLoading();";

        /// <summary>
        /// call back fail function, default value is "doErrorCallbackFun".
        /// </summary>
        private string _callbackFailFunction = "doErrorCallbackFun";

        /// <summary>
        /// check control value validate function.
        /// </summary>
        private string _checkControlValueValidateFunction;

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
        /// label key string.
        /// </summary>
        private string _labelKey;

        /// <summary>
        /// section id
        /// </summary>
        private string _sectionId;

        /// <summary>
        /// Position value
        /// </summary>
        private AutoFillPosition _positionID;

        /// <summary>
        /// whether set editable on empty
        /// </summary>
        private bool _setEditableOnEmpty;

        /// <summary>
        /// whether set focus on error, default value is true.
        /// </summary>
        private bool _setFocusOnError = true;

        /// <summary>
        /// dropdown show type
        /// </summary>
        private DropDownListShowType _showType = DropDownListShowType.ShowValue;

        /// <summary>
        /// drop down source type
        /// </summary>
        private DropDownListDataSourceType _sourceType = DropDownListDataSourceType.Database;

        /// <summary>
        /// standard choice category
        /// </summary>
        private string _stdCategory;

        /// <summary>
        /// sub label string.
        /// </summary>
        private string _subLabel;

        /// <summary>
        /// auto fill type
        /// </summary>
        private AutoFillType _autoFillType = AutoFillType.None;

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
        /// The field width
        /// </summary>
        private string _fieldWidth = string.Empty;

        /// <summary>
        /// The need show loading
        /// </summary>
        private bool _needShowLoading = true;

        #endregion Fields

        #region Properties

        /// <summary>
        /// Gets or sets ToolTipLabelKey
        /// </summary>
        public virtual string ToolTipLabelKey
        {
            get;
            set;
        }

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
        /// Gets or sets InstructionAlign
        /// </summary>
        public InstructionAlign InstructionAlign
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
        /// Gets or sets a value indicating whether the control is not render label
        /// </summary>
        public bool IsHiddenLabel
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets a value indicating whether the control auto fill State.
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
        /// Gets or sets section ID. if current label is section label,this property identify the id of the section
        /// </summary>
        public string SectionID
        {
            get
            {
                return _sectionId;
            }

            set
            {
                _sectionId = value;
            }
        }

        /// <summary>
        /// Gets or sets Position ID, It mark the place of city or state control.
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
        /// Gets or sets a value indicating whether when on item in dropdown set it to be editable
        /// </summary>
        public bool SetEditableOnEmpty
        {
            get
            {
                return _setEditableOnEmpty;
            }

            set
            {
                _setEditableOnEmpty = value;
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
        /// Gets or sets the show type
        /// </summary>
        public DropDownListShowType ShowType
        {
            get
            {
                return _showType;
            }

            set
            {
                _showType = value;
            }
        }

        /// <summary>
        /// Gets or sets the source type
        /// </summary>
        public DropDownListDataSourceType SourceType
        {
            get
            {
                return _sourceType;
            }

            set
            {
                _sourceType = value;
            }
        }

        /// <summary>
        /// Gets or sets the bind standard choice category name
        /// </summary>
        public string StdCategory
        {
            get
            {
                return _stdCategory;
            }

            set
            {
                _stdCategory = value;
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
        /// Gets or sets a value indicating whether the control is readonly.
        /// </summary>
        public bool ReadOnly
        {
            get
            {
                object state = ViewState["IsReadOnly"];
                bool result = state == null ? false : bool.Parse(state.ToString());
                return result;
            }

            set
            {
                ViewState["IsReadOnly"] = value;
            }
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
        /// Gets or sets the option group value.
        /// </summary>
        /// <value>The option group value.</value>
        private string OptionGroupValue
        {
            get
            {
                string s = (string)ViewState["OptionGroupValue"];

                return s ?? ACAConstant.OPTION_GROUP;
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
                this.SelectedIndex = 0;
            }
        }

        /// <summary>
        /// Disable current control to make it readonly.
        /// </summary>
        public void DisableEdit()
        {
            ReadOnly = true;

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
            ReadOnly = false;

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
            return this.SelectedValue;
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
            string toolTip = ControlRenderUtil.GetToolTip(this);
            ToolTip = string.IsNullOrEmpty(toolTip) ? ToolTip : toolTip;

            if (ToolTip != null && ToolTip.Trim() == string.Empty)
            {
                ToolTip = string.Empty;
            }

            if (DesignMode)
            {
                return;
            }

            string helpMessage = LabelConvertUtil.GetGlobalTextByKey("aca_dorpdown_help_message");
            ToolTip = !string.IsNullOrEmpty(ToolTip) ? string.Concat(ToolTip, ACAConstant.BLANK, helpMessage) : helpMessage;

            if (this.SetEditableOnEmpty && Items.Count <= 0)
            {
                AccelaTextBox tb = new AccelaTextBox();
                tb.LabelKey = this.LabelKey;
                this.LabelKey = string.Empty;
                tb.ID = "ctl00_PlaceHolderMain_" + ID;
                this.Controls.Clear();
                tb.RenderElement(w);
            }
            else
            {
                if (FieldAlignment == FieldAlignment.RTL)
                {
                    Attributes.CssStyle.Add(HtmlTextWriterStyle.Direction, "rtl");
                }

                base.Render(w);
                this.RenderChildren(w);
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
            int indexForSpecifiedValue = -1;

            if (Items.Count == 0)
            {
                return;
            }

            foreach (ListItem item in this.Items)
            {
                if (item.Value.Equals(value, StringComparison.InvariantCultureIgnoreCase))
                {
                    indexForSpecifiedValue = Items.IndexOf(item);
                    break;
                }
            }

            this.SelectedIndex = indexForSpecifiedValue;            
        }

        /// <summary>
        /// when only one choice is available, set default item "--Select--" will not be selected and the one available selection will be selected.
        /// </summary>
        public void SetAvailableItemSelected()
        {
            if (Items != null)
            {
                //default item, value as empty, text as "--select--" message ;
                ListItem defaultItem = new ListItem(LabelConvertUtil.GetGlobalTextByKey("aca_common_select"), string.Empty);

                if (Items.Count == 2 && Items.IndexOf(defaultItem) > -1)
                {
                    Items.Remove(defaultItem);
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
            bool isAdmin = AccelaControlRender.IsAdminRender(this);

            IAccelaControlRender render = (IAccelaControlRender)ObjectFactory.GetObject(typeof(IAccelaControlRender), isAdmin);
            render.OnPreRender(this);
            base.OnPreRender(e);

            if (_needShowLoading && AutoPostBack && !isAdmin
                && (string.IsNullOrEmpty(Attributes["onchange"]) || !Attributes["onchange"].Contains(JsShowLoading)))
            {
                Attributes.Add("onchange", JsShowLoading + Attributes["onchange"]);
            }

            if (ReadOnly)
            {
                ScriptManager.RegisterStartupScript(Page, GetType(), "DisableDropDownList_" + this.ClientID, "SetFieldToDisabled('" + this.ClientID + "', true);", true);
            }
        }

        /// <summary>
        /// Displays the drop down list on the client.
        /// </summary>
        /// <param name="writer">A System.Web.UI.HtmlTextWriter that contains the output stream to render on the client.</param>
        protected override void Render(HtmlTextWriter writer)
        {
            if (DesignMode)
            {
                return;
            }

            var ddl = this as AccelaDropDownList;

            if (ddl.Parent is AccelaTimeSelection)
            {
                // for the time selection control
                ddl.RenderElement(writer);
            }
            else
            {
                AccelaControlRender.Render(writer, this);
            }
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
        /// Renders the items in the <see cref="T:System.Web.UI.WebControls.ListControl" /> control.
        /// </summary>
        /// <param name="writer">The <see cref="T:System.Web.UI.HtmlTextWriter" /> that represents the output stream used to write content to a Web page.</param>
        protected override void RenderContents(HtmlTextWriter writer)
        {
            // if or not display optionGroup's EndTag
            bool writerEndTag = false;

            if (this.Required)
            {
                SetAvailableItemSelected();
            }

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
        /// Renders the option group begin tag.
        /// </summary>
        /// <param name="li">The item list.</param>
        /// <param name="writer">The writer.</param>
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
        /// Renders the option group end tag.
        /// </summary>
        /// <param name="writer">The writer.</param>
        private void RenderOptionGroupEndTag(HtmlTextWriter writer)
        {
            writer.WriteEndTag(ACAConstant.OPTION_GROUP);
            writer.WriteLine();
        }

        /// <summary>
        /// Display Option
        /// </summary>
        /// <param name="li">the item list</param>
        /// <param name="writer">html writer</param>
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
