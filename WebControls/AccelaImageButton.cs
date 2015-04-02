#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: AccelaImageButton.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2010-2014
 *
 *  Description:
 *
 *  Notes:
 * $Id: AccelaImageButton.cs 176401 2010-06-25 12:11:30Z ACHIEVO\grady.lu $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using System;
using System.Collections.Specialized;
using System.Text.RegularExpressions;
using System.Web.UI;
using System.Web.UI.WebControls;

using Accela.ACA.Common;
using Accela.ACA.Common.Common;
using Accela.ACA.Common.Util;
using Accela.Web.Controls.ControlRender;

namespace Accela.Web.Controls
{
    /// <summary>
    /// Image button control.
    /// </summary>
    public class AccelaImageButton : ImageButton, IAccelaControl
    {
        #region Fields

        /// <summary>
        /// is client visible,default value is true
        /// </summary>
        private bool _clientVisible = true;

        /// <summary>
        /// filed alignment, default value is left to right
        /// </summary>
        private FieldAlignment _fieldAlignment = FieldAlignment.LTR;

        /// <summary>
        /// unit Type
        /// </summary>
        private string _fieldUnit = string.Empty;

        /// <summary>
        /// field width
        /// </summary>
        private string _fieldWidth = string.Empty;

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
        /// label width
        /// </summary>
        private string _labelWidth = string.Empty;

        /// <summary>
        /// layout Type
        /// </summary>
        private ControlLayoutType _layoutType = ControlLayoutType.Horizontal;

        /// <summary>
        /// sub label string.
        /// </summary>
        private string _subLabel;

        #endregion Fields

        #region Properties

        /// <summary>
        /// Gets or sets a value indicating whether Client visible.
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
        /// Gets or sets FieldUnit
        /// </summary>
        public string FieldUnit
        {
            get
            {
                return _fieldUnit;
            }

            set
            {
                _fieldUnit = value;
            }
        }

        /// <summary>
        /// Gets or sets FieldWidth
        /// </summary>
        public string FieldWidth
        {
            get
            {
                return _fieldWidth;
            }

            set
            {
                _fieldWidth = value;
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
        /// Gets or sets a value indicating whether the control will always editable even though the section is readonly or not.
        /// </summary>
        /// <value><c>true</c> if this instance is always editable; otherwise, <c>false</c>.</value>
        public bool IsAlwaysEditable
        {
            get;
            set;
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
        /// Gets or sets a value indicating whether the UniqueID is fixed(is same with the control ID).
        /// Is useless for this control.
        /// </summary>
        /// <value><c>true</c> if this instance is fixed unique unique identifier; otherwise, <c>false</c>.</value>
        /// <exception cref="System.NotImplementedException">not need implement this set function</exception>
        public bool IsFixedUniqueID
        {
            get
            {
                return false;
            }

            set
            {
                throw new NotImplementedException();
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether hidden control.
        /// </summary>
        public bool IsHidden
        {
            get;
            set;
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
        /// Gets or sets LabelKey
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
        /// Gets or sets The ToolTipLabelKey
        /// </summary>
        /// <value>The tool tip label key.</value>
        public string ToolTipLabelKey
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets LabelWidth
        /// </summary>
        public string LabelWidth
        {
            get
            {
                return _labelWidth;
            }

            set
            {
                _labelWidth = value;
            }
        }

        /// <summary>
        /// Gets or sets LayoutType
        /// </summary>
        public ControlLayoutType LayoutType
        {
            get
            {
                return this._layoutType;
            }

            set
            {
                this._layoutType = value;
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
                _subLabel = ScriptFilter.FilterScript(value, false);
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
        /// Gets or sets Field Unit's Width.
        /// </summary>
        /// <value>The width of the unit.</value>
        public string UnitWidth
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
        /// Clear Text.
        /// </summary>
        public void ClearValue()
        {
            this.Text = string.Empty;
        }

        /// <summary>
        /// Disable Edit
        /// </summary>
        public void DisableEdit()
        {
            //this.Enabled = false;

            // first time,stores original CssClass to cache to viewstate.
            if (CssClassForEdit == null)
            {
                CssClassForEdit = this.CssClass;
            }

            // updated CssClass to append readonly css.
            this.CssClass = CssClassForEdit + " " + WebControlConstant.CSS_CLASS_READONLY;
        }

        /// <summary>
        /// Enable Edit.
        /// </summary>
        public void EnableEdit()
        {
            //this.ReadOnly = false;

            // restore the CssClass to original CssClass from ViewState
            if (CssClassForEdit != null)
            {
                this.CssClass = CssClassForEdit;
            }
        }

        /// <summary>
        /// Gets control ID.
        /// </summary>
        /// <returns>Client id of control</returns>
        public string GetControlID()
        {
            return ClientID;
        }

        /// <summary>
        /// Get DefaultLabel
        /// </summary>
        /// <returns>Default label of control</returns>
        public string GetDefaultLabel()
        {
            if (!string.IsNullOrEmpty(LabelKey))
            {
                return Accela.ACA.Common.Common.ScriptFilter.FilterScript(LabelConvertUtil.GetGUITextByKey(LabelKey), false);
            }

            return string.Empty;
        }

        /// <summary>
        /// Get Default Language Sub Label
        /// </summary>
        /// <returns>Default language sub label of control</returns>
        public string GetDefaultLanguageSubLabel()
        {
            if (!string.IsNullOrEmpty(LabelKey))
            {
                return Accela.ACA.Common.Common.ScriptFilter.FilterScript(LabelConvertUtil.GetDefaultLanguageGlobalTextByKey(LabelKey + WebControlConstant.LABEL_KEY_SUB_SUFFIX), false);
            }

            return string.Empty;
        }

        /// <summary>
        /// Get DefaultLanguage Text
        /// </summary>
        /// <returns>Default language text of control</returns>
        public string GetDefaultLanguageText()
        {
            if (!string.IsNullOrEmpty(LabelKey))
            {
                return Accela.ACA.Common.Common.ScriptFilter.FilterScript(LabelConvertUtil.GetGlobalTextByKey(LabelKey), false);
            }

            return string.Empty;
        }

        /// <summary>
        /// Get Default Sub Label
        /// </summary>
        /// <returns>Default sub label of control</returns>
        public string GetDefaultSubLabel()
        {
            if (!string.IsNullOrEmpty(LabelKey))
            {
                return Accela.ACA.Common.Common.ScriptFilter.FilterScript(LabelConvertUtil.GetGUITextByKey(LabelKey + WebControlConstant.LABEL_KEY_SUB_SUFFIX), false);
            }

            return string.Empty;
        }

        /// <summary>
        /// Get label
        /// </summary>
        /// <returns>label of control</returns>
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
        /// Get Sub Label of control.
        /// </summary>
        /// <returns>Default sub label of control</returns>
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
            return this.Text;
        }

        /// <summary>
        /// initial daily extender control
        /// </summary>
        public void InitExtenderControl()
        {
        }

        /// <summary>
        /// if to hide the required indicate
        /// </summary>
        /// <returns>Hide the required indicate or not</returns>
        public bool IsHideRequireIndicate()
        {
            return false;
        }

        /// <summary>
        /// Check whether this control is required input.
        /// </summary>
        /// <returns>Whether this control is required input</returns>
        public bool IsRequired()
        {
            return false;
        }

        /// <summary>
        /// render element
        /// </summary>
        /// <param name="w">HtmlTextWriter object</param>
        public void RenderElement(HtmlTextWriter w)
        {
            string toolTip = ControlRenderUtil.GetToolTip(this);
            this.ToolTip = string.IsNullOrEmpty(toolTip) ? ToolTip : toolTip;

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
        /// Determines whether the image has been clicked prior to rendering on the client.
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs" /> object that contains the event data.</param>
        protected override void OnPreRender(EventArgs e)
        {
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

            AccelaControlRender.Render(writer, this);
        }

        /// <summary>
        /// Override LoadPostData method to handle some special cases.
        /// </summary>
        /// <param name="postDataKey">post data key.</param>
        /// <param name="postCollection">post collection.</param>
        /// <returns>success or not.</returns>
        protected override bool LoadPostData(string postDataKey, NameValueCollection postCollection)
        {
            var newPostCollection = postCollection;
            string userAgent = Context.Request.UserAgent;

            /* To resolve the error occurred when click image button in UpdatePanel under IE10+:
             *  the Image Button coordinates are sent as decimal by IE10+, recreating the collection with corrected values. */
            if (userAgent != null 
                && (Regex.IsMatch(userAgent, @"MSIE 10", RegexOptions.IgnoreCase) || Regex.IsMatch(userAgent, @"Trident/7.*rv:11", RegexOptions.IgnoreCase)))
            {
                newPostCollection = new NameValueCollection();

                foreach (string key in postCollection.AllKeys)
                {
                    if (!string.IsNullOrEmpty(key))
                    {
                        string postDataValue = postCollection[key];

                        if (key.EndsWith(".x") || key.EndsWith(".y"))
                        {
                            double newValue;
                            I18nNumberUtil.TryParseNumberFromWebService(postDataValue, out newValue);

                            newPostCollection.Add(key, ((int)Math.Round(newValue)).ToString());
                        }
                        else
                        {
                            newPostCollection.Add(key, postDataValue);
                        }
                    }
                }
            }
            
            return base.LoadPostData(postDataKey, newPostCollection);
        }

        #endregion Methods
    }
}