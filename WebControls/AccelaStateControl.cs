#region Header

/**
 * <pre>
 *
 *  Accela
 *  File: AccelaStateControl.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2007-2014
 *
 *  Description:
 *
 *  Notes:
 * $Id: AccelaStateControl.cs 279011 2014-09-24 09:19:15Z ACHIEVO\alan.hu $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Web.UI;
using System.Web.UI.WebControls;
using Accela.ACA.Common;
using Accela.Web.Controls.ControlRender;

namespace Accela.Web.Controls
{
    /// <summary>
    /// Provide a control for state 
    /// </summary>
    public class AccelaStateControl : AccelaCompositeControl
    {
        #region Fields

        /// <summary>
        /// child control id
        /// </summary>
        private const string CHILD_CONTROL_ID = "State1"; // don't change this name, which will be used in ACA admin scripts sectionProperties.js

        /// <summary>
        /// state max length
        /// </summary>
        private const int STATEMAXLENGTH = 30;

        /// <summary>
        /// Indicates is load or not
        /// </summary>
        private bool _isLoaded = false;

        /// <summary>
        /// AccelaDropDownList for state
        /// </summary>
        private AccelaDropDownList ddlState = null;

        /// <summary>
        /// AccelaTextBox for state
        /// </summary>
        private AccelaTextBox txtState = null;

        /// <summary>
        /// layout Type
        /// </summary>
        private ControlLayoutType _layoutType = ControlLayoutType.Vertical;

        /// <summary>
        /// validation string
        /// </summary>
        private string _validateString = string.Empty;

        /// <summary>
        /// The check control value validate function
        /// </summary>
        private string _checkControlValueValidateFunction = string.Empty;

        /// <summary>
        /// auto fill type field
        /// </summary>
        private AutoFillType _autoFillType;

        /// <summary>
        /// position id field
        /// </summary>
        private AutoFillPosition _positionId;

        #endregion Fields

        #region Properties

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
        /// Gets client id
        /// </summary>
        public override string ClientID
        {
            get
            {
                return GetControlID();
            }
        }

        /// <summary>
        /// Gets or sets Auto Fill Type.
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
        /// Gets or sets Position ID,It mark the place of city or state control.
        /// </summary>
        public AutoFillPosition PositionID
        {
            get
            {
                return _positionId;
            }

            set
            {
                _positionId = value;
            }
        }

        /// <summary>
        /// Gets or sets res text
        /// </summary>
        public string ResText
        {
            get
            {
                EnsureChildControls();

                if (IsPresentAsText)
                {
                    return txtState.Text;
                }
                else
                {
                    if (ddlState.SelectedItem != null && !string.IsNullOrEmpty(ddlState.SelectedItem.Value))
                    {
                        return ddlState.SelectedItem.Text;
                    }
                    else
                    {
                        return string.Empty;
                    }
                }
            }

            set
            {
                EnsureChildControls();

                if (IsPresentAsText)
                {
                    txtState.Text = value;
                }
                else
                {
                    ddlState.ClearSelection();
                    ListItem selectedItem = ddlState.Items.FindByText(value);

                    if (selectedItem != null)
                    {
                        selectedItem.Selected = true;
                    }
                    else
                    {
                        if (this.ddlState.Items.Count > 0)
                        {
                            this.ddlState.SelectedIndex = 0;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Gets or sets text
        /// </summary>
        [Browsable(true)]
        public string Text
        {
            get
            {
                EnsureChildControls();

                if (IsPresentAsText)
                {
                    return txtState.Text;
                }
                else
                {
                    if (ddlState.SelectedItem != null)
                    {
                        return ddlState.SelectedItem.Value;
                    }
                    else
                    {
                        return string.Empty;
                    }
                }
            }

            set
            {
                EnsureChildControls();

                if (IsPresentAsText)
                {
                    txtState.Text = value;
                }
                else
                {
                    ddlState.ClearSelection();
                    bool isFound = false;

                    foreach (ListItem item in ddlState.Items)
                    {
                        if (item.Value.Equals(value, StringComparison.InvariantCultureIgnoreCase))
                        {
                            ddlState.SelectedValue = item.Value;
                            isFound = true;
                            break;
                        }
                    }

                    if (!isFound && ddlState.Items.Count > 0)
                    {
                        ddlState.SelectedIndex = 0;
                    }
                }
            }
        }

        /// <summary>
        /// Gets or sets validate string. if it is required
        /// </summary>
        public string Validate
        {
            get
            {
                return _validateString;
            }

            set
            {
                _validateString = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is present text.
        /// </summary>
        /// <value><c>true</c> if this instance is present textbox; otherwise, <c>false</c>.</value>
        public bool IsPresentAsText
        {
            get
            {
                if (!_isLoaded)
                {
                    _isLoaded = true;
                }

                // if in admin ,then it use textbox control.
                if (AccelaControlRender.IsAdminRender(this))
                {
                    return true;
                }

                bool isPresentAsText = false;

                if (ViewState["IsPresentAsText"] != null)
                {
                    bool.TryParse(ViewState["IsPresentAsText"].ToString(), out isPresentAsText);
                }

                return isPresentAsText;
            }

            set
            {
                this.ChildControlsCreated = false;
                ViewState["IsPresentAsText"] = value;
                EnsureChildControls();
            }
        }

        /// <summary>
        /// Gets or sets control's Layout Type
        /// </summary>
        public override ControlLayoutType LayoutType
        {
            get
            {
                return this._layoutType;
            }

            set
            {
                if (this.txtState != null)
                {
                    this.txtState.LayoutType = value;
                }

                if (this.ddlState != null)
                {
                    this.ddlState.LayoutType = value;
                }

                _layoutType = value;
            }
        }

        /// <summary>
        /// Gets or sets the width of the control.
        /// </summary>
        public override Unit Width
        {
            get
            {
                if (IsPresentAsText)
                {
                    return txtState.Width;
                }
                else
                {
                    return ddlState.Width;
                }
            }

            set
            {
                if (IsPresentAsText)
                {
                    txtState.Width = value;
                }
                else
                {
                    ddlState.Width = value;
                }
            }
        }

        /// <summary>
        /// Gets or sets the height of the control.
        /// </summary>
        public override Unit Height
        {
            get
            {
                if (IsPresentAsText)
                {
                    return txtState.Height;
                }
                else
                {
                    return ddlState.Height;
                }
            }

            set
            {
                if (IsPresentAsText)
                {
                    txtState.Height = value;
                }
                else
                {
                    ddlState.Height = value;
                }
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether [read only].
        /// </summary>
        /// <value><c>true</c> if [read only]; otherwise, <c>false</c>.</value>
        private bool ReadOnly
        {
            get
            {
                object state = ViewState["IsReadOnly"];
                return state != null && bool.Parse(state.ToString());
            }

            set
            {
                ViewState["IsReadOnly"] = value;
            }
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
        /// Clear control value
        /// </summary>
        public override void ClearValue()
        {
            EnsureChildControls();

            if (IsPresentAsText)
            {
                (txtState as IAccelaControl).ClearValue();
            }
            else
            {
                (ddlState as IAccelaControl).ClearValue();
            }
        }

        /// <summary>
        /// Add some attribute into the control.
        /// </summary>
        /// <param name="key">Attribute name</param>
        /// <param name="value">Attribute value</param>
        public void AddAttribute(string key, string value)
        {
            if (IsPresentAsText)
            {
                if (txtState as AccelaTextBox != null)
                {
                    (txtState as AccelaTextBox).Attributes[key] += value;
                }
            }
            else
            {
                if (ddlState as AccelaDropDownList != null)
                {
                    (ddlState as AccelaDropDownList).Attributes[key] += value;
                }
            }
        }

        /// <summary>
        /// Disabled edit
        /// </summary>
        public override void DisableEdit()
        {
            EnsureChildControls();
            ReadOnly = true;

            if (IsPresentAsText)
            {
                (txtState as IAccelaControl).DisableEdit();
            }
            else
            {
                (ddlState as IAccelaControl).DisableEdit();
            }

            // first time,stores original CssClass to cache to viewstate.
            if (CssClassForEdit == null)
            {
                CssClassForEdit = CssClass;
            }

            // updated CssClass to append readonly css.
            CssClass = CssClassForEdit + " " + WebControlConstant.CSS_CLASS_READONLY;
        }

        /// <summary>
        /// Enable edit control
        /// </summary>
        public override void EnableEdit()
        {
            EnsureChildControls();
            ReadOnly = false;

            if (IsPresentAsText)
            {
                (txtState as IAccelaControl).EnableEdit();
            }
            else
            {
                (ddlState as IAccelaControl).EnableEdit();
            }

            // restore the CssClass to original CssClass from ViewState
            if (CssClassForEdit != null)
            {
                CssClass = CssClassForEdit;
            }
        }

        /// <summary>
        /// Get control id
        /// </summary>
        /// <returns>control id.</returns>
        public override string GetControlID()
        {
            EnsureChildControls();

            if (IsPresentAsText)
            {
                return this.txtState.ClientID;
            }
            else
            {
                return this.ddlState.ClientID;
            }
        }

        /// <summary>
        /// get default label
        /// </summary>
        /// <returns>default label</returns>
        public override string GetDefaultLabel()
        {
            EnsureChildControls();
            if (IsPresentAsText)
            {
                return (txtState as IAccelaControl).GetDefaultLabel();
            }
            else
            {
                return (ddlState as IAccelaControl).GetDefaultLabel();
            }
        }

        /// <summary>
        /// get default language sub label
        /// </summary>
        /// <returns>default language sub label</returns>
        public override string GetDefaultLanguageSubLabel()
        {
            EnsureChildControls();

            if (IsPresentAsText)
            {
                return (txtState as IAccelaControl).GetDefaultLanguageSubLabel();
            }
            else
            {
                return (ddlState as IAccelaControl).GetDefaultLanguageSubLabel();
            }
        }

        /// <summary>
        /// get default language text
        /// </summary>
        /// <returns>default language text</returns>
        public override string GetDefaultLanguageText()
        {
            EnsureChildControls();

            if (IsPresentAsText)
            {
                return (txtState as IAccelaControl).GetDefaultLanguageText();
            }
            else
            {
                return (ddlState as IAccelaControl).GetDefaultLanguageText();
            }
        }

        /// <summary>
        /// get default sub label
        /// </summary>
        /// <returns>default sub label</returns>
        public override string GetDefaultSubLabel()
        {
            EnsureChildControls();

            if (IsPresentAsText)
            {
                return (txtState as IAccelaControl).GetDefaultSubLabel();
            }
            else
            {
                return (ddlState as IAccelaControl).GetDefaultSubLabel();
            }
        }

        /// <summary>
        /// get default label
        /// </summary>
        /// <returns>default label</returns>
        public override string GetLabel()
        {
            EnsureChildControls();

            if (IsPresentAsText)
            {
                return (txtState as IAccelaControl).GetLabel();
            }
            else
            {
                return (ddlState as IAccelaControl).GetLabel();
            }
        }

        /// <summary>
        /// get sub label
        /// </summary>
        /// <returns>sub label string</returns>
        public override string GetSubLabel()
        {
            EnsureChildControls();

            if (IsPresentAsText)
            {
                return (txtState as IAccelaControl).GetSubLabel();
            }
            else
            {
                return (ddlState as IAccelaControl).GetSubLabel();
            }
        }

        /// <summary>
        /// Gets value from current control.
        /// if subclass is textbox, it returns Text value.
        /// if subclass is RadioButtonList or dropdownlist, it returns selected value.
        /// </summary>
        /// <returns>selected value or Text of control.</returns>
        public override string GetValue()
        {
            EnsureChildControls();

            if (IsPresentAsText)
            {
                return (txtState as IAccelaControl).GetValue();
            }
            else
            {
                return (ddlState as IAccelaControl).GetValue();
            }
        }

        /// <summary>
        /// initial daily extender control
        /// </summary>
        public override void InitExtenderControl()
        {
            //not need extender
        }

        /// <summary>
        /// Sets value to current control.
        /// if subclass is textbox, it set value to Text.
        /// if subclass is RadioButtonList or dropdownlist, it set value as selected.
        /// </summary>
        /// <param name="value">a value to be set to current control.</param>
        public override void SetValue(string value)
        {
            EnsureChildControls();

            if (IsPresentAsText)
            {
                (txtState as IAccelaControl).SetValue(value);
            }
            else
            {
                (ddlState as IAccelaControl).SetValue(value);
            }
        }

        /// <summary>
        /// if to hide the required indicate
        /// </summary>
        /// <returns>Hide the required indicate or not</returns>
        public override bool IsHideRequireIndicate()
        {
            EnsureChildControls();

            if (IsPresentAsText)
            {
                return (txtState as IAccelaControl).IsHideRequireIndicate();
            }
            else
            {
                return (ddlState as IAccelaControl).IsHideRequireIndicate();
            }
        }

        /// <summary>
        /// Check whether this control is required input.
        /// </summary>
        /// <returns>Whether this control is required input</returns>
        public override bool IsRequired()
        {
            EnsureChildControls();

            if (IsPresentAsText)
            {
                return (txtState as IAccelaControl).IsRequired();
            }
            else
            {
                return (ddlState as IAccelaControl).IsRequired();
            }
        }

        /// <summary>
        /// Binds the items for DDL.
        /// </summary>
        /// <param name="stateList">The state list.</param>
        public void BindItemsForDDL(Dictionary<string, string> stateList)
        {
            if (DesignMode)
            {
                return;
            }

            EnsureChildControls();
            this.ddlState.Items.Clear();

            if (stateList != null && stateList.Count != 0)
            {
                foreach (KeyValuePair<string, string> keyValuePair in stateList)
                {
                    this.ddlState.Items.Add(new ListItem(keyValuePair.Value, keyValuePair.Key));
                }
            }

            // added --select--
            this.ddlState.Items.Insert(0, new ListItem(LabelConvertUtil.GetGlobalTextByKey("aca_common_select"), string.Empty));

            this.ddlState.MaxValueLength = StandardChoiceMaxLength.MAX_LENGTH_STATES;
        }

        /// <summary>
        /// render element
        /// </summary>
        /// <param name="w">HtmlTextWriter object</param>
        public override void RenderElement(HtmlTextWriter w)
        {
            this.ToolTip = ControlRenderUtil.GetToolTip(this);

            EnsureChildControls();

            if (IsPresentAsText)
            {
                (txtState as IAccelaControl).RenderElement(w);
            }
            else
            {
                (ddlState as IAccelaControl).RenderElement(w);
            }
        }

        /// <summary>
        /// override CreateControlCollection
        /// </summary>
        protected override void CreateChildControls()
        {
            Controls.Clear();

            if (HasChildViewState)
            {
                ClearChildViewState();
            }

            txtState = new AccelaTextBox();
            
            txtState.MaxLength = STATEMAXLENGTH;

            if (!string.IsNullOrEmpty(LabelKey))
            {
                txtState.LabelKey = LabelKey;
            }

            this.Controls.Add(txtState);

            ddlState = new AccelaDropDownList();

            if (!string.IsNullOrEmpty(LabelKey))
            {
                ddlState.LabelKey = LabelKey;
            }

            this.Controls.Add(ddlState);

            if (IsPresentAsText)
            {
                txtState.ID = CHILD_CONTROL_ID;
                ddlState.ID = CHILD_CONTROL_ID + "ddl";
            }
            else
            {
                txtState.ID = CHILD_CONTROL_ID + "txt";
                ddlState.ID = CHILD_CONTROL_ID;
            }
        }

        /// <summary>
        /// Raises the <see cref="E:System.Web.UI.Control.PreRender"/> event.
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs"/> object that contains the event data.</param>
        protected override void OnPreRender(EventArgs e)
        {
            if (IsPresentAsText)
            {
                txtState.CheckControlValueValidateFunction = _checkControlValueValidateFunction;
                txtState.Validate = _validateString;
                txtState.LabelKey = LabelKey;
                txtState.ClientVisible = ClientVisible;
                txtState.AutoFillType = _autoFillType;
                txtState.PositionID = _positionId;
                txtState.IsDisplayLabel = IsDisplayLabel;
                txtState.CssClass = CssClass;
                txtState.ReadOnly = ReadOnly;
            }
            else
            {
                ddlState.CheckControlValueValidateFunction = _checkControlValueValidateFunction;
                ddlState.Required = _validateString.IndexOf("required", StringComparison.InvariantCultureIgnoreCase) > -1;
                ddlState.LabelKey = LabelKey;
                ddlState.ClientVisible = ClientVisible;
                ddlState.AutoFillType = _autoFillType;
                ddlState.PositionID = _positionId;
                ddlState.IsDisplayLabel = IsDisplayLabel;
                ddlState.CssClass = CssClass;
                ddlState.ReadOnly = ReadOnly;
            }

            base.OnPreRender(e);
        }

        /// <summary>
        /// Displays the state control on the client.
        /// </summary>
        /// <param name="writer">A System.Web.UI.HtmlTextWriter that contains the output stream to render on the client.</param>
        protected override void Render(HtmlTextWriter writer)
        {
            EnsureChildControls();

            if (DesignMode)
            {
                return;
            }

            if (IsPresentAsText)
            {
                AccelaControlRender.Render(writer, txtState);
            }
            else
            {
                AccelaControlRender.Render(writer, ddlState);
            }
        }

        /// <summary>
        /// Restores view-state information from a previous request that was saved with the <see cref="M:System.Web.UI.WebControls.WebControl.SaveViewState"/> method.
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
            else
            {
                scripts = this.Attributes["onclick"];

                if (!string.IsNullOrEmpty(scripts) && scripts.IndexOf("RunExpression") > -1)
                {
                    this.Attributes.Remove("onclick");
                }
            }
        }

        #endregion Methods
    }
}