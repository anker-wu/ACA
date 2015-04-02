#region Header

/**
 * <pre>
 *
 *  Accela
 *  File: CheckBoxRequiredFieldValidator.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2009-2014
 *
 *  Description:
 *
 *  Notes:
 * $Id: CheckBoxRequiredFieldValidator.cs 131439 2009-05-19 11:07:26Z ACHIEVO\jackie.yu $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using System;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Accela.Web.Controls
{
    /// <summary>
    /// Provide required validator for check-box
    /// </summary>
    public class CheckBoxRequiredFieldValidator : BaseValidator
    {
        #region Methods

        /// <summary>
        /// Adds HTML attributes and styles that need to be rendered to the specified
        /// System.Web.UI.HtmlTextWriter instance.
        /// </summary>
        /// <param name="writer">An System.Web.UI.HtmlTextWriter that represents the output stream to render
        /// HTML content on the client.</param>
        protected override void AddAttributesToRender(HtmlTextWriter writer)
        {
            base.AddAttributesToRender(writer);

            if (RenderUplevel)
            {
                Type scriptManagerType = System.Web.Compilation.BuildManager.GetType("System.Web.UI.ScriptManager", false);
                scriptManagerType.InvokeMember(
                    "RegisterExpandoAttribute",
                    System.Reflection.BindingFlags.InvokeMethod | System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static,
                    null,
                    null,
                    new object[] { this, this.ClientID, "evaluationfunction", "chkVerify", false });
            }
        }

        /// <summary>
        /// override ControlPropertiesValid
        /// </summary>
        /// <returns>true-control properties is valid</returns>
        protected override bool ControlPropertiesValid()
        {
            return true;
        }

        /// <summary>
        /// Check evaluate is checked or not
        /// </summary>
        /// <returns>true or false-evaluate is checked or not</returns>
        protected override bool EvaluateIsValid()
        {
            CheckBox chk = (CheckBox)FindControl(ControlToValidate);

            if (chk.Checked)
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Raises the System.Web.UI.Control.PreRender event. 
        /// </summary>
        /// <param name="e">A System.EventArgs that contains the event data.</param>
        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);

            if (RenderUplevel)
            {
                //register required verify function for checkbox.
                StringBuilder sb_Script = new StringBuilder();
                sb_Script.Append("function chkVerify(val) {");
                sb_Script.Append("var col = document.getElementById(val.controltovalidate);");
                sb_Script.Append("if (col && col.type == 'checkbox') {");
                sb_Script.Append("if (col.checked) { return true; }");
                sb_Script.Append("}");
                sb_Script.Append("return false;");
                sb_Script.Append("}");
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "chkValidatorScript", sb_Script.ToString(), true);
            }
        }

        #endregion Methods
    }
}
