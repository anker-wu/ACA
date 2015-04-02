#region Header

/**
 * <pre>
 *
 *  Accela
 *  File: IAccelaControl.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2008-2014
 *
 *  Description:
 *
 *  Notes:
 * $Id: IAccelaControl.cs 276801 2014-08-07 02:14:39Z ACHIEVO\james.shi $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using System.Web.UI;
using Accela.Web.Controls.ControlRender;

namespace Accela.Web.Controls
{
    /// <summary>
    /// a empty interface used to indicate if the control need support admin extender
    /// </summary>
    public interface IAccelaBaseControl
    {
    }

    /// <summary>
    /// the control which needs extender control need to implement this interface
    /// </summary>
    public interface IAccelaWithExtenderControl : IAccelaBaseControl
    {
        /// <summary>
        /// initial daily extender control
        /// </summary>
        void InitExtenderControl();
    }

    /// <summary>
    /// Each control must implement this interface.
    /// </summary>
    public interface IAccelaControl : IAccelaNonInputControl
    {
        #region Properties

        /// <summary>
        /// Gets or sets a value indicating whether the control is visible in client
        /// </summary>
        bool ClientVisible
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets InstructionAlign to control the help icon position.
        /// </summary>
        InstructionAlign InstructionAlign
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets a value indicating whether the UniqueID is fixed(is same with the control ID).
        /// </summary>
        bool IsFixedUniqueID
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets a value indicating whether the control is hidden in client, ACA will create this control with style 'display' set to none
        /// </summary>
        bool IsHidden
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets a value indicating whether the control will always editable even though the section is readonly or not.
        /// </summary>
        bool IsAlwaysEditable
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets control's Layout Type
        /// </summary>
        ControlLayoutType LayoutType
        {
            get;
            set;
        }
        
        /// <summary>
        /// Gets or sets Field's Unit Type
        /// </summary>
        string FieldUnit
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets Field label's width
        /// </summary>
        string LabelWidth
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets Field Value's width
        /// </summary>
        string FieldWidth
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets Field Unit's Width.
        /// </summary>
        string UnitWidth
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the extend control html, this html will render before the help icon.
        /// </summary>
        string ExtendControlHtml
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the tool tip label key.
        /// </summary>
        /// <value>The tool tip label key.</value>
        string ToolTipLabelKey
        {
            get;
            set;
        }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Clear current control value
        /// </summary>
        void ClearValue();

        /// <summary>
        /// Disable current control to make it readonly.
        /// </summary>
        void DisableEdit();

        /// <summary>
        /// Enable current control to make it be editable.
        /// </summary>
        void EnableEdit();

        /// <summary>
        /// Get Client ID of control.
        /// </summary>
        /// <returns>Client id of control</returns>
        string GetControlID();

        /// <summary>
        /// Get Label of control.
        /// </summary>
        /// <returns>label of control</returns>
        string GetLabel();

        /// <summary>
        /// Get ToolTipLabel
        /// </summary>
        /// <returns>tooltips label</returns>
        string GetToolTipLabel();
        
        /// <summary>
        /// Gets value from current control.
        /// if subclass is textbox, it returns Text value.
        /// if subclass is RadioButtonList or dropdownlist, it returns selected value.
        /// </summary>
        /// <returns>selected value or Text of control.</returns>
        string GetValue();

        /// <summary>
        /// if to hide the required indicate
        /// </summary>
        /// <returns>Hide the required indicate or not</returns>
        bool IsHideRequireIndicate();

        /// <summary>
        /// Check whether this control is required input.
        /// </summary>
        /// <returns>Whether this control is required input</returns>
        bool IsRequired();

        /// <summary>
        /// render control
        /// </summary>
        /// <param name="w">HtmlTextWriter object</param>
        void RenderElement(HtmlTextWriter w);

        /// <summary>
        /// Sets value to current control.
        /// if subclass is textbox, it set value to Text.
        /// if subclass is RadioButtonList or dropdownlist, it set value as selected.
        /// </summary>
        /// <param name="value">a value to be set to current control.</param>
        void SetValue(string value);

        #endregion Methods
    }

    /// <summary>
    /// a accela control base interface
    /// </summary>
    public interface IAccelaNonInputControl : IAccelaWithExtenderControl
    {
        #region Properties

        /// <summary>
        /// Gets or sets a value indicating whether control label display or not
        /// </summary>
        bool IsDisplayLabel
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets Label Key
        /// </summary>
        string LabelKey
        {
            get;
            set;
        }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Get Default Label of control.
        /// </summary>
        /// <returns>Default label of control</returns>
        string GetDefaultLabel();

        /// <summary>
        /// Get Default Language Sub Label of control.
        /// </summary>
        /// <returns>Default language sub label of control</returns>
        string GetDefaultLanguageSubLabel();

        /// <summary>
        /// Get Default Sub Label of control.
        /// </summary>
        /// <returns>Default sub label of control</returns>
        string GetDefaultSubLabel();

        /// <summary>
        /// Get Sub Label of control.
        /// </summary>
        /// <returns>Default sub label of control</returns>
        string GetSubLabel();

        /// <summary>
        /// Get Default Language Label of control.
        /// </summary>
        /// <returns>Default language text of control</returns>
        string GetDefaultLanguageText();

        #endregion Methods
    }
}