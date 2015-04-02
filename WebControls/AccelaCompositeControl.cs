#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: AccelaCompositeControl.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2012-2014
 *
 *  Description: The base class of the composite controls.
 *
 *  Notes:
 * $Id: AccelaCompositeControl.cs 238998 2012-12-04 09:10:00Z ACHIEVO\alan.hu $.
 *  Revision History
 *  Date,            Who,        What
 *  Dec 4, 2012      Alan Hu     Initial.
 * </pre>
 */

#endregion Header

using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using Accela.ACA.Common;
using Accela.ACA.Common.Common;
using Accela.Web.Controls.ControlRender;
using AjaxControlToolkit;

[assembly: WebResource("Accela.Web.Controls.AccelaCompositeControl.js", "text/javascript")]

namespace Accela.Web.Controls
{
    /// <summary>
    /// The base class of the composite controls.
    /// </summary>
    public abstract class AccelaCompositeControl : AccelaNonInputCompositeControl, IAccelaControl
    {
        #region Fields

        /// <summary>
        /// the child control id prefix.
        /// </summary>
        protected const string CHILD_CONTROL_ID_PREFIX = "ChildControl";

        /// <summary>
        /// Local field for <see cref="ClientVisible"/> property.
        /// </summary>
        private bool _clientVisible = true;

        /// <summary>
        /// Local field for <see cref="Label"/> property.
        /// </summary>
        private string _label;

        /// <summary>
        /// high light CSS,default value is "HighlightCSSClass".
        /// </summary>
        private string _highlightCssClass = "HighlightCssClass";

        /// <summary>
        /// call back fail function, default value is "doErrorCallbackFun".
        /// </summary>
        private string _callbackFailFunction = "doErrorCallbackFun";

        #endregion

        #region Properties(Implemented from IAccelaControl)

        /// <summary>
        /// Gets or sets a value indicating whether the control is visible in client.
        /// Seems only used in Admin side.
        /// </summary>
        public virtual bool ClientVisible
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
        /// Gets or sets a value to control the help icon's position.
        /// </summary>
        public virtual InstructionAlign InstructionAlign
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets a value indicating whether the control is always editable.
        /// </summary>
        public virtual bool IsAlwaysEditable
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
        /// Gets or sets a value indicating whether the control is hidden.
        /// </summary>
        public virtual bool IsHidden
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the control's layout.
        /// </summary>
        public virtual ControlLayoutType LayoutType
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the unit for the field or label's width.
        /// </summary>
        public virtual string FieldUnit
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the control label's width.
        /// </summary>
        public virtual string LabelWidth
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the input control's width in this control.
        /// </summary>
        public virtual string FieldWidth
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the control unit's width.
        /// </summary>
        public virtual string UnitWidth
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the additional html, this html will render before the help icon.
        /// </summary>
        public virtual string ExtendControlHtml
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the control label.
        /// </summary>
        public virtual string Label
        {
            get
            {
                return _label;
            }

            set
            {
                _label = ScriptFilter.FilterScript(value, false);
            }
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets a value indicating whether [validation ignore case].
        /// </summary>
        /// <value><c>true</c> if [validation ignore case]; otherwise, <c>false</c>.</value>
        public bool ValidationIgnoreCase { get; set; }

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
        /// Gets or sets ToolTipLabelKey
        /// </summary>
        public string ToolTipLabelKey 
        {  
            get;
            set; 
        }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is field required.
        /// </summary>
        /// <value><c>true</c> if this instance is field required; otherwise, <c>false</c>.</value>
        public bool IsFieldRequired { get; set; }

        #endregion

        #region Methods(Implemented from IAccelaControl)

         /// <summary>
        /// Gets control's client ID.
        /// </summary>
        /// <returns>Control's client ID.</returns>
        public virtual string GetControlID()
        {
            return ClientID;
        }

        /// <summary>
        /// Gets the control label, if label is empty but has label key, will gets the label by label key.
        /// </summary>
        /// <returns>Control label.</returns>
        public virtual string GetLabel()
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
        /// Call the base render method to render this control.
        /// </summary>
        /// <param name="w">Html text writer for control render output.</param>
        public virtual void RenderElement(System.Web.UI.HtmlTextWriter w)
        {
            base.Render(w);
        }

        /// <summary>
        /// Clear the control value.
        /// </summary>
        public abstract void ClearValue();

        /// <summary>
        /// Disable the control to make it readonly.
        /// </summary>
        public abstract void DisableEdit();

        /// <summary>
        /// Enable the control to make it editable.
        /// </summary>
        public abstract void EnableEdit();

        /// <summary>
        /// Gets the control value.
        /// </summary>
        /// <returns>Control value.</returns>
        public abstract string GetValue();

        /// <summary>
        /// if to hide the required indicate
        /// </summary>
        /// <returns>Hide the required indicate or not</returns>
        public abstract bool IsHideRequireIndicate();

        /// <summary>
        /// Check whether this control is required input.
        /// </summary>
        /// <returns>Whether this control is required input</returns>
        public abstract bool IsRequired();

        /// <summary>
        /// Sets value to current control.
        /// if subclass is textbox, it set value to Text.
        /// if subclass is RadioButtonList or dropdownlist, it set value as selected.
        /// </summary>
        /// <param name="value">a value to be set to current control.</param>
        public abstract void SetValue(string value);

        #endregion

        #region Methods

        /// <summary>
        /// Override the OnPreRender method to render the specific controls or extenders. 
        /// </summary>
        /// <param name="e">Event arguments.</param>
        protected override void OnPreRender(EventArgs e)
        {
            IAccelaControlRender render = (IAccelaControlRender)ObjectFactory.GetObject(typeof(IAccelaControlRender), AccelaControlRender.IsAdminRender(this));

            bool isAdmin = AccelaControlRender.IsAdminRender(this);

            if (!isAdmin && Page != null)
            {
                //Register the client scripts resource.
                ScriptManager.RegisterClientScriptResource(Page, GetType(), "Accela.Web.Controls.AccelaCompositeControl.js");
            }

            render.OnPreRender(this);
        }

        /// <summary>
        /// Override the Render method to render this control uses the Accela Control Render.
        /// </summary>
        /// <param name="writer">Html text writer for control render output.</param>
        protected override void Render(System.Web.UI.HtmlTextWriter writer)
        {
            AccelaControlRender.Render(writer, this);
        }

        /// <summary>
        /// Create required validate control
        /// </summary>
        /// <param name="id">The new required validate control's id</param>
        /// <param name="clientFunction">The client validate function</param>
        /// <param name="controlToValidate">The input control to validate</param>
        protected void CreateRequiredValidate(string id, string clientFunction, WebControl controlToValidate)
        {
            var customValidator = new CustomValidator();
            customValidator.ID = id;
            customValidator.ControlToValidate = controlToValidate.ID;
            customValidator.Display = ValidatorDisplay.None;
            customValidator.ClientValidationFunction = clientFunction;
            customValidator.ErrorMessage = string.Empty;
            customValidator.ValidateEmptyText = true;
            Controls.Add(customValidator);
            CreateValidatorCallbackExtender(customValidator.ID, controlToValidate.ClientID);
        }

        /// <summary>
        /// Create validate call back extender.
        /// </summary>
        /// <param name="targetControlID">The target control id.</param>
        /// <param name="currentValidateClientID">The current child control when validate.</param>
        protected void CreateValidatorCallbackExtender(string targetControlID, string currentValidateClientID)
        {
            var validator = new ValidatorCallbackExtender();
            validator.TargetControlID = targetControlID;
            validator.CallbackControlID = ID;
            validator.CurrentValidateControlClientID = currentValidateClientID;
            validator.HighlightCssClass = _highlightCssClass;
            validator.CallbackFailFunction = _callbackFailFunction;
            validator.CheckControlValueValidateFunction = null;
            validator.ValidationByHiddenTextBox = false;
            validator.ValidationIgnoreCase = ValidationIgnoreCase;
            Controls.Add(validator);
        }

        #endregion
    }
}