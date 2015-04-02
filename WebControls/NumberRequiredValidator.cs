#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: NumberRequiredValidator.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2008-2014
 *
 *  Description:
 *
 *  Notes:
 * $Id: NumberRequiredValidator.cs 276801 2014-08-07 02:14:39Z ACHIEVO\james.shi $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Accela.Web.Controls
{
    /// <summary>
    /// Provide required validator for number
    /// </summary>
    public class NumberRequiredValidator : BaseValidator
    {
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
                writer.AddAttribute("evaluationfunction", "tb_verify");
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
        /// Check evaluate is checked or not
        /// </summary>
        /// <returns>Evaluate is checked or not</returns>
        protected bool EvaluateIsChecked()
        {
            TextBox txt = (TextBox)FindControl(ControlToValidate);

            if (txt.Text.Trim() == string.Empty)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Check whether the value in the input control is valid.
        /// </summary>
        /// <returns>true if the value in the input control is valid; otherwise, false.</returns>
        protected override bool EvaluateIsValid()
        {
            return EvaluateIsChecked();
        }

        #endregion Methods
    }
}
