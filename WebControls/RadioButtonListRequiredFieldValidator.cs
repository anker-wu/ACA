#region Header

/**
 * <pre>
 *
 *  Accela
 *  File: RadioButtonListRequiredFieldValidator.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2008-2014
 *
 *  Description:
 *
 *  Notes:
 * $Id: RadioButtonListRequiredFieldValidator.cs 276801 2014-08-07 02:14:39Z ACHIEVO\james.shi $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using System;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Accela.Web.Controls
{
    /// <summary>
    /// Provide required validator for radio button list
    /// </summary>
    public class RadioButtonListRequiredFieldValidator : BaseValidator
    {
        #region Properties

        /// <summary>
        /// Gets a value indicating whether partial rendering is supported or not
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
        /// Gets or sets a value indicating whether partial rendering is enabled or not
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
        /// Gets or sets a value indicating whether partial rendering is checked or not
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
        /// Adds the HTML attributes and styles that need to be rendered for the control
        /// to the specified System.Web.UI.HtmlTextWriter object.
        /// </summary>
        /// <param name="writer">An System.Web.UI.HtmlTextWriter that represents the output stream to render
        /// HTML content on the client.</param>
        protected override void AddAttributesToRender(HtmlTextWriter writer)
        {
            base.AddAttributesToRender(writer);

            if (RenderUplevel)
            {
                if (!this.IsPartialRenderingSupported)
                {
                    this.Page.ClientScript.RegisterExpandoAttribute(this.ClientID, "evaluationfunction", "cb_vefify", false);
                }
                else
                {
                    ScriptManager.RegisterExpandoAttribute(this, this.ClientID, "evaluationfunction", "cb_vefify", false);
                }
            }
        }

        /// <summary>
        /// Determines whether the control specified by the System.Web.UI.WebControls.BaseValidator.ControlToValidate
        /// property is a valid control.
        /// </summary>
        /// <returns>true if the control specified by System.Web.UI.WebControls.BaseValidator.ControlToValidate 
        /// is a valid control; otherwise, false.</returns>
        protected override bool ControlPropertiesValid()
        {
            return true;
        }

        /// <summary>
        /// Check whether the value in the input control is valid.
        /// </summary>
        /// <returns>true if the value in the input control is valid; otherwise, false.</returns>
        protected override bool EvaluateIsValid()
        {
            //server side validation return ture, for bug #40131
            //don't need validate.
            return true;
        }

        #endregion Methods
    }
}