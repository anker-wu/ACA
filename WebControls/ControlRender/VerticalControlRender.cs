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
 * $Id: VerticalControlRender.cs 276801 2014-08-07 02:14:39Z ACHIEVO\james.shi $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using System;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using Accela.ACA.Common;
using Accela.ACA.Common.Util;

namespace Accela.Web.Controls.ControlRender
{
    /// <summary>
    /// Render control in Horizontal Layout. 
    /// </summary>
    public class VerticalControlRender
    {
        #region Fields

        /// <summary>
        /// Html Text Writer
        /// </summary>
        private HtmlTextWriter _writer;

        /// <summary>
        /// All Kinds of Accela _control
        /// </summary>
        private IAccelaControl _control;

        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="VerticalControlRender"/> class.
        /// </summary>
        /// <param name="writer">The writer.</param>
        /// <param name="control">The control.</param>
        public VerticalControlRender(HtmlTextWriter writer, IAccelaControl control)
        {
            _writer = writer;
            _control = control;
        }

        #region Properties

        /// <summary>
        /// Gets and sets current Control ID
        /// </summary>
        private string CurrentControlID
        {
            get
            {
                return this._control.GetControlID();
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Render Control for Vertical layout
        /// </summary>
        public void Render()
        {
            string displayStyle = string.Empty;

            if (_control.IsHidden)
            {
                displayStyle = "style='display:none;'";
            }

            // 1. Render Control Layout
            _writer.Write("<table role='presentation' parentcontrol='true' cellpadding=0 cellspacing=0 id='" + CurrentControlID + "_table' class='ACA_TDAlignLeftOrRightTop' " + displayStyle + ">");

            if (_control is AccelaCheckBox && ((AccelaCheckBox)_control).IsInASITable)
            {
                _writer.Write("<tr style='height:40px;' vAlign='bottom'>");
            }
            else
            {
                _writer.Write("<tr>");
            }

            // 3. Render Label, Field, Sub Label
            _writer.Write("<td>");

            _writer.Write("<div class='ACA_Form' id='" + CurrentControlID + "_element_group'>");

            // 3.0 Render Error Indicator
            //_writer.Write(ControlRenderUtil.RenderErrorIndicator(_control));

            // 3.1 Render Field Label and help message
            bool isHasSubLabel = !string.IsNullOrEmpty(_control.GetSubLabel());
            bool isHasLabel = RenderFieldLabel(isHasSubLabel);
            
            // 3.2 Render Field Control
            RenderFieldControl(isHasLabel, isHasSubLabel);
            
            // 3.3 Render Sub Label
            string subLabelHtml = ControlRenderUtil.RenderFieldSubLabel(_control);
            _writer.Write(subLabelHtml);
            
            // 3.4 Render Speclling Checker
            _writer.Write(ControlRenderUtil.RenderSpellingChecker(_control));
            _writer.Write("</div>");

            _writer.Write("</td>");

            _writer.Write("</tr>");
            _writer.Write("</table>");
        }

        /// <summary>
        /// Render Field's Label
        /// </summary>
        /// <param name="isHasSubLabel">Check if Field has sub label</param>
        /// <returns>Whether render filed label</returns>
        private bool RenderFieldLabel(bool isHasSubLabel)
        {
            bool isHasLabel = true;
            string label = string.Empty;

            if (!(_control is AccelaDropDownList && ((AccelaDropDownList)_control).IsHiddenLabel))
            {
                label = _control.GetLabel();
            }

            if (string.IsNullOrEmpty(label) && !_control.IsRequired())
            {
                _writer.Write("<div class='ACA_Label font12px'>");
                isHasLabel = false;
            }
            else
            {
                if (_control is AccelaDropDownList && ((AccelaDropDownList)_control).IsHiddenLabel)
                {
                    _writer.Write("<div class='ACA_Label font12px ACA_LabelHeight ACA_FLeft'>");
                }
                else
                {
                    _writer.Write("<div class='ACA_Label font12px ACA_LabelHeight'>");
                }
            }

            if (isHasLabel)
            {
                if (_control.InstructionAlign == InstructionAlign.Right)
                {
                    _writer.Write("<table role='presentation' cellpadding='0' cellspacing='0' border='0' class='subTable'><tr><td>");
                }
                else
                {
                    _writer.Write("<table role='presentation' cellpadding='0' cellspacing='0' border='0'><tr><td>");
                }
            }

            // 0. Render Error Indicator
            _writer.Write(ControlRenderUtil.RenderErrorIndicator(_control));

            // 1. Render red asterisk before required control's label.            
            _writer.Write(ControlRenderUtil.RenderRequiredIndicator(_control));

            // 2. Render control's label
            if (!string.IsNullOrEmpty(label) && _control.IsDisplayLabel)
            {
                _writer.Write("<label id='{0}_label_1' for='{0}'>", CurrentControlID);
                _writer.Write(ControlRenderUtil.FormatFeeIndicator(label));
                _writer.Write("</label>");

                if (_control is AccelaTextBox && (_control as AccelaTextBox).EnableSoundexSearch)
                {
                    _writer.Write(ControlRenderUtil.RenderSoundexIcon(_control as AccelaTextBox));
                }
            }
            else if (_control is AccelaCheckBox)
            {
                AccelaCheckBox accelaCheckBox = _control as AccelaCheckBox;

                if (accelaCheckBox.IsInASITable)
                {
                    _writer.Write("<span style='height:10px;display:inline-block;'>");
                    _writer.Write("</span>");
                }

                accelaCheckBox.LabelAttributes.Add(HtmlTextWriterAttribute.Id.ToString(), CurrentControlID + "_label_1");
            }

            // write the extend control
            if (!string.IsNullOrEmpty(_control.ExtendControlHtml))
            {
                _writer.Write(_control.ExtendControlHtml);
            }

            // 3. Render error message label
            _writer.Write(ControlRenderUtil.RenderErrorMessageLabel(_control));

            // 4. Render expression message label
            _writer.Write(ControlRenderUtil.RenderExpressionMessageLabel(_control));

            if (isHasLabel)
            {
                _writer.Write("</td>");

                if (isHasSubLabel || ControlRenderUtil.IsAdminRender(_control))
                {
                    if (!isHasSubLabel)
                    {
                        _writer.Write("<td>");
                    }
                    else if (_control is AccelaCheckBox)
                    {
                        _writer.Write("<td class='aca_checkbox ACA_Help_Icon_Container'>");
                    }
                    else
                    {
                        _writer.Write("<td class='ACA_Help_Icon_Container'>");
                    }

                    _writer.Write(ControlRenderUtil.RenderHelpIcon(_control, isHasSubLabel));
                    _writer.Write("</td>");
                }

                _writer.Write("</tr></table>");
            }

            _writer.Write("</div>");

            return isHasLabel;
        }

        /// <summary>
        /// Render Field Control
        /// </summary>
        /// <param name="isHasLabel">Check if Field has label</param>
        /// <param name="isHasSubLabel">Check if Field has sub label</param>
        private void RenderFieldControl(bool isHasLabel, bool isHasSubLabel)
        {
            if (_control is AccelaCalendarText)
            {
                _writer.Write("<div style='margin-bottom: 2px;'>");
                _writer.Write("<table role='presentation' class='subTable' cellpadding='0' cellspacing='0' border='0'><tr><td style='width:100%; '>");

                _control.RenderElement(_writer);

                _writer.Write("</td><td style=\"white-space: nowrap;\">");
                _writer.Write(ControlRenderUtil.RenderFieldUnitLabel(_control));
                _writer.Write("</td></tr></table>");
            }
            else
            {
                if (I18nCultureUtil.UserPreferredCultureInfo.TextInfo.IsRightToLeft
                    && (_control is AccelaDropDownList || _control is DropDownList))
                {
                    //_writer.Write("<div style='margin-bottom: 2px;float:right; padding-right: 1px;'>");
                    _writer.Write("<div class = 'ACA_RTLDropDownList'>");
                }
                else if (I18nCultureUtil.UserPreferredCultureInfo.TextInfo.IsRightToLeft
                    && (_control is AccelaRadioButtonList || _control is RadioButtonList))
                {
                    _writer.Write("<div style='margin-bottom: 2px; padding-right: 1px;'>");
                }
                else
                {
                    if (ControlRenderUtil.IsAdminRender(_control) && _control is AccelaCheckBoxList)
                    {
                        AccelaCheckBoxList cbList = _control as AccelaCheckBoxList;
                        if (cbList.Items.Count == 0)
                        {
                            _writer.Write(string.Format("<input type=\"hidden\" id=\"{0}\">", cbList.ClientID));
                        }
                    }

                    if (_control is AccelaDropDownList && ((AccelaDropDownList)_control).IsHiddenLabel)
                    {
                        _writer.Write("<div class='ACA_FLeft'>");
                    }
                    else
                    {
                        _writer.Write("<div style='margin-bottom: 2px;'>");
                    }
                }

                if (_control is AccelaCheckBox)
                {
                    _writer.Write("<table role='presentation' cellpadding='0' cellspacing='0' border='0' class='subTable'><tr><td class='ACA_Label' id='" + CurrentControlID + "_td'  style='width:100%; '>");
                }
                else
                {
                    _writer.Write("<table role='presentation' cellpadding='0' cellspacing='0' border='0' class='subTable'><tr><td id='" + CurrentControlID + "_td' style='width:100%; '>");
                }

                _control.RenderElement(_writer);

                //render the unit type
                _writer.Write("</td><td style=\"white-space: nowrap; padding-top: 5px;\">&nbsp;");
                _writer.Write(ControlRenderUtil.RenderFieldUnitLabel(_control));
                _writer.Write("</td>");

                if (!isHasLabel)
                {
                    if (ControlRenderUtil.IsAdminRender(_control) || isHasSubLabel)
                    {
                        if (!isHasSubLabel)
                        {
                            _writer.Write("<td>");
                        }
                        else if (_control is AccelaCheckBox)
                        {
                            _writer.Write("<td  class='aca_checkbox ACA_Help_Icon_Container'>");
                        }
                        else
                        {
                            _writer.Write("<td class='ACA_Help_Icon_Container'>");
                        }

                        _writer.Write(ControlRenderUtil.RenderHelpIcon(_control, isHasSubLabel));
                        _writer.Write("</td>");
                    }
                }

                _writer.Write("</tr></table>");
                _writer.Write("</div>");
            }
        }

        #endregion
    }
}
