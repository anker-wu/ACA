#region Header

/**
 * <pre>
 *
 *  Accela
 *  File: AccelaButton.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2010-2014
 *
 *  Description:This class is used for rendering control.
 *
 *  Notes:
 * $Id: ImageButtonRender.cs 179249 2010-08-18 09:36:02Z ACHIEVO\daly.zeng $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using Accela.ACA.Common;

namespace Accela.Web.Controls.ControlRender
{
    /// <summary>
    /// Class ImageButtonRender.
    /// </summary>
    public class ImageButtonRender : HorizontalControlRender
    {
        #region Methods

        /// <summary>
        /// Initializes a new instance of the ImageButtonRender class.
        /// </summary>
        /// <param name="htmlWriter">The Html Text Writer.</param>
        /// <param name="accelaControl">All Kinds of Accela Control.</param>
        public ImageButtonRender(HtmlTextWriter htmlWriter, IAccelaControl accelaControl) : base(htmlWriter, accelaControl)
        {
        }

        /// <summary>
        /// Render Control for Horizontal layout
        /// </summary>
        public override void Render()
        {
            string displayStyle = string.Empty;

            if (Control.IsHidden)
            {
                displayStyle = "style='display:none;'";
            }

            //string subLabelHtml = ControlRenderUtil.RenderFieldSubLabel(_control);
            string subLabelHtml = string.Empty;

            //Render Icon
            bool isHasSubLabel = !string.IsNullOrEmpty(subLabelHtml);

            // 1. Render Control Layout
            Writer.Write("<table role='presentation' parentcontrol='true' cellpadding=0 cellspacing=0 id='" + CurrentControlID + "_table' " + displayStyle + ">");
            Writer.Write("<tr>");

            // 2. Render Label,sub label,help icon
            Writer.Write("<td style='width:" + Control.LabelWidth + ";overflow:auto' align='left' valign='center'>");
            RenderFieldLabel();
            Writer.Write("</td>");                      

            // 3. Render Field Control and Unit
            Writer.Write("<td class='ACA_Control_Display'>");

            // 3.1 Render Field Control
            RenderFieldControl();
            Writer.Write("</td>");
            
            if (isHasSubLabel)
            {
                Writer.Write("<td vAlign='top'>");
                Writer.Write("<table role='presentation' cellpadding='0' cellspacing='0' border='0' class='subTable'><tr>");
                Writer.Write("<td>");

                // 3.3 Render Sub Label.(Sub Label was hidden; click the help icon, show the sub label content)
                Writer.Write(subLabelHtml);

                // 3.4 Render help icon
                Writer.Write(ControlRenderUtil.RenderHelpIcon(Control, isHasSubLabel));

                Writer.Write("</td></tr></table>");
                Writer.Write("</td>");
            }

            // 4. Render Error Message
            Writer.Write("<td align='left' valign='center'>");
            Writer.Write(ControlRenderUtil.RenderErrorMessageLabel(Control));
            Writer.Write("</td>");

            // 5. Render Expression Message
            Writer.Write("<td align='left' valign='center'>");
            Writer.Write(ControlRenderUtil.RenderExpressionMessageLabel(Control));
            Writer.Write("</td>");

            Writer.Write("</tr>");
            Writer.Write("</table>");
        }

        /// <summary>
        /// Render Field Control and its sub label
        /// </summary>
        protected override void RenderFieldControl()
        {
            Writer.Write("<div runat='server' class='ACA_Nowrap' id='" + CurrentControlID + "_element_group'>");

            // 1. Render Field Control
            Control.RenderElement(Writer);

            //2.render Unit type
            Writer.Write(ControlRenderUtil.RenderFieldUnitLabel(Control));

            // 3. Render Speclling Checker
            Writer.Write(ControlRenderUtil.RenderSpellingChecker(Control));

            Writer.Write("</div>");
        }
        
        /// <summary>
        /// Render Field's Label
        /// </summary>
        private void RenderFieldLabel()
        {
            string label = Control.GetLabel();

            if (string.IsNullOrEmpty(label))
            {
                Writer.Write("<div class='ACA_Hide'>");
            }
            else
            {
                Writer.Write("<div class='ACA_Label font12px aca_imagebutton_label' style='width:" + Control.LabelWidth + "'>");
            }

            // 3.1 Render red asterisk in the front of control's label when the control is set as required
            Writer.Write(ControlRenderUtil.RenderRequiredIndicator(Control));

            // 3.2 Render Control's label
            if (Control.IsDisplayLabel && !string.IsNullOrEmpty(label))
            {
                Writer.Write("<label id='{0}_label_1' for='{0}'>", CurrentControlID);
                Writer.Write(ControlRenderUtil.FormatFeeIndicator(label));
                Writer.Write("</label>");
            }
           
            Writer.Write("</div>");
        }

        #endregion
    }
}
