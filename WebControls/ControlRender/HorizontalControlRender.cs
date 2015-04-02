#region Header

/**
 * <pre>
 *
 *  Accela
 *  File: AccelaButton.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2009-2014
 *
 *  Description:This class is used for rendering control.
 *
 *  Notes:
 * $Id: HorizontalControlRender.cs 276801 2014-08-07 02:14:39Z ACHIEVO\james.shi $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using System;
using System.Linq;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;

using Accela.ACA.Common;
using Accela.ACA.Common.Util;

namespace Accela.Web.Controls.ControlRender
{
    /// <summary>
    /// Class Name: HorizontalControlRender
    /// Description: Render control in Horizontal Layout. 
    /// </summary>
    public class HorizontalControlRender
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="HorizontalControlRender"/> class.
        /// </summary>
        public HorizontalControlRender()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="HorizontalControlRender"/> class.
        /// </summary>
        /// <param name="htmlWriter">The Html Text Writer.</param>
        /// <param name="accelaControl">All Kinds of Accela Control.</param>
        public HorizontalControlRender(HtmlTextWriter htmlWriter, IAccelaControl accelaControl)
        {
            Writer = htmlWriter;
            Control = accelaControl;
        }

        #region Properties

        /// <summary>
        /// Gets or sets Html Text Writer
        /// </summary>
        protected HtmlTextWriter Writer { get; set; }

        /// <summary>
        /// Gets or sets All Kinds of Accela Control
        /// </summary>
        /// <value>The control.</value>
        protected IAccelaControl Control { get; set; }

        #endregion

        #region Properties

        /// <summary>
        /// Gets and sets current Control ID
        /// </summary>
        protected string CurrentControlID
        {
            get
            {
                return this.Control.GetControlID();
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Get control's title value.
        /// </summary>
        /// <param name="control">accela control</param>
        /// <returns>title value:label+required</returns>
        public static string GetToolTip(IAccelaControl control)
        {
            string fieldLabel = string.Empty;
            string fieldUnit = string.Empty;
            string fieldRequired = string.Empty;

            if (control.IsDisplayLabel)
            {
                fieldLabel = control.GetLabel();
            }

            if (control.IsRequired() && !control.IsHideRequireIndicate())
            {
                fieldRequired = LabelConvertUtil.GetGlobalTextByKey("aca_required_field");
            }

            if (!string.IsNullOrEmpty(control.FieldUnit))
            {
                fieldUnit = control.FieldUnit;
            }

            string[] toolTip = { fieldLabel, fieldUnit, fieldRequired };
            return LabelConvertUtil.RemoveHtmlFormat(DataUtil.ConcatStringWithSplitChar(toolTip, ACAConstant.BLANK));
        }

        /// <summary>
        /// Render Control for Horizontal layout
        /// </summary>
        public virtual void Render()
        {
            /*
             * control horizontal layout:
             * _________________________________________
             * |error indicator                        |
             * -----------------------------------------
             * |required|label|help Icon|input box|unit|
             * -----------------------------------------
             */

            string displayStyle = string.Empty;

            if (Control.IsHidden)
            {
                displayStyle = "style='display:none;'";
            }

            Writer.Write("<table role='presentation' parentcontrol='true' cellpadding=0 cellspacing=0 id='" + CurrentControlID + "_table' class='ACA_TDAlignLeftOrRightTop' " + displayStyle + ">");
            Writer.Write("<tr>");
            Writer.Write("<td>");

            // Render Control Layout
            Writer.Write("<table role='presentation' parentcontrol='true' cellpadding=0 cellspacing=0 id='" + CurrentControlID + "_element_group'>");
            Writer.Write("<tr>");

            Writer.Write("<td>");
            Writer.Write("<table role='presentation' cellpadding=0 cellspacing=0>");
            Writer.Write("<tr>");

            // 1 Render Error Indicator in the top.
            Writer.Write("<td align='right' valign='top'><div>");
            Writer.Write(ControlRenderUtil.RenderErrorIndicator(Control));
            Writer.Write("</div></td>");

            // 2. Render Error Message
            Writer.Write("<td class='ACA_Label font12px ACA_FLeft' align='left' valign='center'>");
            Writer.Write(ControlRenderUtil.RenderErrorMessageLabel(Control));
            Writer.Write("</td>");

            // 3. Render Expression Message
            Writer.Write("<td align='left' valign='center'>");
            Writer.Write(ControlRenderUtil.RenderExpressionMessageLabel(Control));
            Writer.Write("</td>");

            Writer.Write("</tr>");
            Writer.Write("</table>");
            Writer.Write("</td>");
            Writer.Write("</tr>");

            Writer.Write("<tr>");
            Writer.Write("<td>");
            Writer.Write("<table role='presentation' cellpadding=0 cellspacing=0>");
            Writer.Write("<tr>");

            //Render table for required, label, help icon
            Writer.Write("<td align='left' style='width:" + Control.LabelWidth + "'>");
            Writer.Write("<table role='presentation' cellpadding='0' style='width:" + Control.LabelWidth + "' cellspacing='0'>");
            Writer.Write("<tr>");
            Writer.Write("<td align='left' valign='top'>");

            // 4.Render required indicator
            Writer.Write("<span class='ACA_Label font12px'>" + ControlRenderUtil.RenderRequiredIndicator(Control) + "</span>");

            // 5. Render Label
            string label = Control.GetLabel();
            if (string.IsNullOrEmpty(label))
            {
                Writer.Write("<span class='ACA_Hide'>");
            }
            else
            {
                Writer.Write("<span class='ACA_Label font12px'>");
            }

            if (Control.IsDisplayLabel && !string.IsNullOrEmpty(label))
            {
                Writer.Write("<label id='{0}_label_1' for='{0}'>", CurrentControlID);
                Writer.Write(ControlRenderUtil.FormatFeeIndicator(label));
                Writer.Write("</label>");

                if (Control is AccelaTextBox && (Control as AccelaTextBox).EnableSoundexSearch)
                {
                    Writer.Write(ControlRenderUtil.RenderSoundexIcon(Control as AccelaTextBox));
                }
            }
            else if (Control is AccelaCheckBox)
            {
                (Control as AccelaCheckBox).LabelAttributes.Add(HtmlTextWriterAttribute.Id.ToString(), CurrentControlID + "_label_1");
            }

            if (!string.IsNullOrEmpty(Control.ExtendControlHtml))
            {
                Writer.Write(Control.ExtendControlHtml);
            }

            Writer.Write("</span></td>");

            //6.Render Help Icon && sub label
            string subLabelHtml = ControlRenderUtil.RenderFieldSubLabel(Control);
            bool isHasSubLabel = !string.IsNullOrEmpty(Control.GetSubLabel());
           
            if (!isHasSubLabel)
            {
                Writer.Write("<td>");
            }
            else if (Control is AccelaCheckBox)
            {
                Writer.Write("<td  class='aca_checkbox ACA_Help_Icon_Container'>");
            }
            else
            {
                Writer.Write("<td  class='ACA_Help_Icon_Container'>");
            }

            // 6.1 Render Sub Label.(Sub Label was hidden; click the help icon, show the sub label content)
            Writer.Write(subLabelHtml);

            // 6.2 Render help icon
            Writer.Write(ControlRenderUtil.RenderHelpIcon(Control, isHasSubLabel));

            Writer.Write("</td>");
            Writer.Write("</tr>");
            Writer.Write("</table>");
            Writer.Write("</td>");

            // 7. Render Field Control
            if (Control is AccelaCheckBox)
            {
                Writer.Write("<td class='ACA_Control_Display ACA_Label'>");
            }
            else
            {
                Writer.Write("<td class='ACA_Control_Display'>");
            }

            RenderFieldControl();
            Writer.Write("</td>");

            //8.Render Unit Type.
            if (!string.IsNullOrEmpty(Control.FieldUnit))
            {
                Writer.Write("<td>&nbsp;");
                RenderFieldUnitLabel();
                Writer.Write("</td>");
            }

            Writer.Write("</tr>");
            Writer.Write("</table>");
            Writer.Write("</td>");

            Writer.Write("</tr>");
            
            //9. render spelling checker.
            Writer.Write("<tr>");
            Writer.Write("<td style=\"padding-left:" + Control.LabelWidth + "\">");
            RenderSpellingChecker(Control);
            Writer.Write("</td>");

            Writer.Write("</tr>");

            Writer.Write("</table>");
            Writer.Write("</td>");
            Writer.Write("</tr>");
            Writer.Write("</table>");
        }

        /// <summary>
        /// Render Field Unit
        /// </summary>
        public virtual void RenderFieldUnitLabel()
        {
            //2.render Unit type
            Writer.Write(ControlRenderUtil.RenderFieldUnitLabel(Control));
        }

        /// <summary>
        /// Render spelling checker.
        /// </summary>
        /// <param name="control">Accela Control</param>
        public virtual void RenderSpellingChecker(IAccelaControl control)
        {
            Writer.Write(ControlRenderUtil.RenderSpellingChecker(Control));
        }

        /// <summary>
        /// Render Field Control and its sub label
        /// </summary>
        protected virtual void RenderFieldControl()
        {
            if (Control is AccelaCheckBoxList || Control is AccelaCheckBox || Control is AccelaRadioButton || Control is AccelaRadioButtonList)
            {
                Writer.Write("<div class='ACA_Form' >");
            }
            else
            {
                Writer.Write("<div class='ACA_Form ACA_Nowrap'>");
            }

            if (ControlRenderUtil.IsAdminRender(Control) && Control is AccelaCheckBoxList)
            {
                AccelaCheckBoxList cbList = Control as AccelaCheckBoxList;
                if (cbList.Items.Count == 0)
                {
                    Writer.Write(string.Format("<input type=\"hidden\" id=\"{0}\">", cbList.ClientID));
                }
            }

            // 1. Render Field Control
            Control.RenderElement(Writer);
            Writer.Write("</div>");
        }

        #endregion
    }
}