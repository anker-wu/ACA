#region Header

/**
 * <pre>
 *
 *  Accela
 *  File: CheckBoxListRequiredFieldValidator.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2008-2014
 *
 *  Description:
 *
 *  Notes:
 * $Id: CheckBoxListRequiredFieldValidator.cs 276801 2014-08-07 02:14:39Z ACHIEVO\james.shi $.
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
    /// Provide required validator for check-box list
    /// </summary>
    public class CheckBoxListRequiredFieldValidator : BaseValidator
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
                ScriptManager.RegisterExpandoAttribute(this, this.ClientID, "evaluationfunction", "cb_vefify", false);
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
            //server side validation return ture, for bug #40131
            //don't need validate.
            return true;
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
                StringBuilder sb_Script = new StringBuilder();
                sb_Script.Append("<script language=\"javascript\">");
                sb_Script.Append("\r");
                sb_Script.Append("\r");
                sb_Script.Append("function cb_vefify(val) {");
                sb_Script.Append("\r");

                sb_Script.Append("var value = val.controltovalidate");

                sb_Script.Append("\r");
                sb_Script.Append("var col = value.all;");
                sb_Script.Append("\r");
                sb_Script.Append("if ( col != null ) {");
                sb_Script.Append("\r");

                sb_Script.Append("for ( i = 0; i < col.length; i++ ) {");
                sb_Script.Append("\r");
                sb_Script.Append("if (col.item(i).tagName == \"INPUT\") {");
                sb_Script.Append("\r");
                sb_Script.Append("if ( col.item(i).checked ) {");
                sb_Script.Append("\r");
                sb_Script.Append("\r");
                sb_Script.Append("return true;");
                sb_Script.Append("\r");
                sb_Script.Append("}");
                sb_Script.Append("\r");
                sb_Script.Append("}");
                sb_Script.Append("\r");
                sb_Script.Append("}");
                sb_Script.Append("\r");
                sb_Script.Append("\r");
                sb_Script.Append("\r");
                sb_Script.Append("return false;");
                sb_Script.Append("\r");
                sb_Script.Append("}");
                sb_Script.Append("\r");
                sb_Script.Append("}");
                sb_Script.Append("\r");
                sb_Script.Append("</script>");
                this.Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "RBLScript", sb_Script.ToString());
            }
        }

        #endregion Methods
    }
}