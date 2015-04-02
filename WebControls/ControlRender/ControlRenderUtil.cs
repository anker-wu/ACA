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
 * $Id: ControlRenderUtil.cs 278523 2014-09-05 05:36:03Z ACHIEVO\james.shi $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using Accela.ACA.Common;
using Accela.ACA.Common.Common;
using Accela.ACA.Common.Util;
using Accela.ACA.WSProxy;
using Accela.ACA.WSProxy.Common;
using AccelaWebControlExtender;

namespace Accela.Web.Controls.ControlRender
{
    /// <summary>
    /// Instruction Alignment.
    /// </summary>
    public enum InstructionAlign
    {
        /// <summary>
        /// instruction icon in the right of control.
        /// </summary>
        Right,

        /// <summary>
        /// instruction icon behind the label.
        /// </summary>
        Left
    }

    /// <summary>
    /// Control Render Utility 
    /// </summary>
    public class ControlRenderUtil
    {
        /// <summary>
        /// Render Expression Message Label
        /// </summary>
        /// <param name="control">Accela Control</param>
        /// <returns>Html Text</returns>
        public static string RenderExpressionMessageLabel(IAccelaControl control)
        {
            string htmlText = string.Empty;

            string labelClass = "ACA_ExpressionMessageLabel";

            if (control != null)
            {
                if (control.LayoutType == ControlLayoutType.Vertical)
                {
                    labelClass = "ACA_VerticalExpressionMessageLabel";
                }

                htmlText = string.Format("<span id='{0}_label_exp' class='{1}'></span>", control.GetControlID(), labelClass);
            }

            return htmlText;
        }

        /// <summary>
        /// Render Error Message Label
        /// </summary>
        /// <param name="control">Accela Control</param>
        /// <returns>Html Text</returns>
        public static string RenderErrorMessageLabel(IAccelaControl control)
        {
            string htmlText = string.Empty;

            if (control != null)
            {
                htmlText = string.Format("<span id='{0}_label_2' class='ACA_ErrorMessageLabel'></span>", control.GetControlID());
            }

            return htmlText;
        }

        /// <summary>
        /// Render Error Indicator Label
        /// </summary>
        /// <param name="control">Accela Control</param>
        /// <returns>Html Text</returns>
        public static string RenderErrorIndicator(IAccelaControl control)
        {
            string htmlText = string.Empty;
            string style = "display:none;";

            if (control != null)
            {
                string src = (control as WebControl).Page.ClientScript.GetWebResourceUrl(typeof(IAccelaControl), "Accela.Web.Controls.Assets.error_16.gif");
                string altText = ScriptFilter.AntiXssHtmlEncode(LabelConvertUtil.RemoveHtmlFormat(LabelConvertUtil.GetTextByKey("aca_global_js_showerror_alt", control as WebControl)));
 
                if (control.LayoutType == ControlLayoutType.Horizontal)
                {
                    style = style + "margin-top:0px;margin-bottom:0px;";
                }

                htmlText = string.Format("<div id='{0}_err_indicator' class='ACA_Error_Indicator' style='{1}'><img src='{2}' alt='{3}' /></div>", control.GetControlID(), style, src, altText);
            }

            return htmlText;
        }

        /// <summary>
        /// Render RequiredIndicator
        /// </summary>
        /// <param name="control">Accela Control</param>
        /// <returns>Html Text</returns>
        public static string RenderRequiredIndicator(IAccelaControl control)
        {
            StringBuilder htmlText = new StringBuilder();

            if (control != null)
            {
                htmlText.AppendFormat("<span id='{0}_label_0'>", control.GetControlID());

                if (control.IsRequired() && !control.IsHideRequireIndicate())
                {
                   htmlText.Append("<div class='ACA_Required_Indicator'>*</div>");
                }

                htmlText.Append("</span>");
            }

            return htmlText.ToString();
        }

        /// <summary>
        /// Render Field Unit Label
        /// </summary>
        /// <param name="control">Accela Control</param>
        /// <returns>Html Text</returns>
        public static string RenderFieldUnitLabel(IAccelaControl control)
        {
            string htmlText = string.Empty;
            string style = string.Empty;

            if (control != null)
            {
                // Render Field Unit Label
                if (!string.IsNullOrEmpty(control.FieldUnit))
                {
                    htmlText = string.Format("<span class = 'ACA_Label ACA_Unit'>{0}</span>", control.FieldUnit);
                }
            }

            return htmlText;
        }

        /// <summary>
        /// Render Field's Sub Label
        /// </summary>
        /// <param name="control">Accela Control</param>
        /// <returns>Html Text</returns>
        public static string RenderFieldSubLabel(IAccelaControl control)
        {
            StringBuilder htmlText = new StringBuilder();

            if (control != null)
            {
                string subLabel = control.GetSubLabel();

                IAccelaControlRender render = (IAccelaControlRender)ObjectFactory.GetObject(typeof(IAccelaControlRender), IsAdminRender(control));
                
                if (render.IsRenderSubLabel(subLabel))
                {
                    //ControlType='1' is for body text, here we use it temporarily for field instruction
                    htmlText.AppendFormat("<div id='{0}_sub_label' ControlType='1'", control.GetControlID());

                    if ((control is AccelaRadioButtonList)
                        && string.IsNullOrEmpty((control as AccelaRadioButtonList).SubLabel)
                        && ((control as AccelaRadioButtonList).ListType == RadioButtonListType.PaymentMethod))
                    {
                        htmlText.Append(" class='ACA_Hide'>");
                    }
                    else
                    {
                        htmlText.Append(" class='ACA_Sub_Label ACA_Hide'>");
                    }

                    htmlText.Append(FormatFeeIndicator(subLabel));
                    htmlText.Append("</div>");
                }
            }

            return htmlText.ToString();
        }

        /// <summary>
        /// Render Field's Help Icon
        /// </summary>
        /// <param name="control">Accela Control</param>
        /// <param name="isHasSubLabel">Check if Field has sub label</param>
        /// <returns>help icon html.</returns>
        public static string RenderHelpIcon(IAccelaControl control, bool isHasSubLabel)
        {
            StringBuilder htmlText = new StringBuilder();

            if (control != null)
            {
                string altText = ScriptFilter.AntiXssHtmlEncode(LabelConvertUtil.RemoveHtmlFormat(LabelConvertUtil.GetTextByKey("aca_field_helpwindow_title", control as WebControl)));

                htmlText.Append("<a title=\"" + altText + "\" id='" + control.GetControlID() + "_help' href='javascript:void(0);' onclick='return false;' ");
                htmlText.Append("class='ACA_Help_Icon ACA_FRight NotShowLoading");
                htmlText.Append(isHasSubLabel ? " ACA_Show" : " ACA_Hide");
                htmlText.Append("'><img src='" + (control as WebControl).Page.ClientScript.GetWebResourceUrl(
                    typeof(IAccelaControl), "Accela.Web.Controls.Helper.help_icon.png"));
                htmlText.Append("' alt='" + altText + "' /></a>");
            }

            return htmlText.ToString();
        }

        /// <summary>
        /// Render Field's Spelling Checker
        /// </summary>
        /// <param name="control">Accela Control</param>
        /// <returns>Html Text</returns>
        public static string RenderSpellingChecker(IAccelaControl control)
        {
            StringBuilder htmlText = new StringBuilder();

            bool isAddSpellCheck = control is AccelaTextBox
                   && !((AccelaTextBox)control).ReadOnly
                   && ((AccelaTextBox)control).TextMode == TextBoxMode.MultiLine
                   && !IsAdminRender(control)
                   && ControlConfigureProvider.IsSpellCheckEnabled();

            if (isAddSpellCheck)
            {
                htmlText.AppendFormat(
                    "<a href=\"#\" style=\"cursor:pointer\" class=\"font11px NotShowLoading\" onclick=\"if (typeof(SetNotAskForSPEAR)!='undefined') SetNotAskForSPEAR();DoSpellCheck('{0}://{1}:{2}{3}', '{4}', event);return false;\" >{5}</a>",
                    HttpContext.Current.Request.Url.Scheme,
                    HttpContext.Current.Request.Url.Host,
                    HttpContext.Current.Request.Url.Port,
                    HttpContext.Current.Request.ApplicationPath,
                    ((AccelaTextBox)control).ClientID,
                    LabelConvertUtil.GetGlobalTextByKey("acc_spell_check_a"));
            }

            return htmlText.ToString();
        }

        /// <summary>
        /// Format Fee Indicator in Label
        /// </summary>
        /// <param name="label">label html</param>
        /// <returns>result label</returns>
        public static string FormatFeeIndicator(string label)
        {
            string resultLabel = string.Empty;

            if (!string.IsNullOrEmpty(label))
            {
                string feeIndicator = string.Format("({0})", I18nNumberUtil.CurrencySymbol);

                if (label.IndexOf(feeIndicator, StringComparison.InvariantCulture) != -1)
                {
                    resultLabel = label.Replace(feeIndicator, "<span>" + feeIndicator + "</span>");
                }
                else
                {
                    resultLabel = label;
                }
            }

            return resultLabel;
        }

        /// <summary>
        /// Indicates current web controls whether need to be presented as admin mode.
        /// </summary>
        /// <param name="control">web control</param>
        /// <returns>true - render it as admin mode.
        ///          false - render it as daily mode.
        /// </returns>
        public static bool IsAdminRender(IAccelaControl control)
        {
            bool isAdmin = false;
            WebControl webControl = control as WebControl;

            if (webControl != null && webControl.Page is IPage)
            {
                isAdmin = (webControl.Page as IPage).IsControlRenderAsAdmin;
            }

            return isAdmin;
        }

        /// <summary>
        /// Get control's title value.
        /// </summary>
        /// <param name="control">accela control</param>
        /// <returns>title value:label+required</returns>
        public static string GetToolTip(IAccelaControl control)
        {
            string fieldUnit = string.Empty;
            string fieldRequired = string.Empty;
            string fieldTooltip = string.Empty;

            if (control.IsRequired() && !control.IsHideRequireIndicate())
            {
                fieldRequired = LabelConvertUtil.GetGlobalTextByKey("aca_required_field");
            }

            if (!string.IsNullOrEmpty(control.FieldUnit))
            {
                fieldUnit = control.FieldUnit;
            }

            if (!string.IsNullOrEmpty(control.ToolTipLabelKey))
            {
                fieldTooltip = control.GetToolTipLabel();
            }

            string[] toolTip = { fieldUnit, fieldRequired, fieldTooltip };
            return LabelConvertUtil.RemoveHtmlFormat(DataUtil.ConcatStringWithSplitChar(toolTip, ACAConstant.BLANK));
        }

        /// <summary>
        /// Create design support extender
        /// </summary>
        /// <param name="control">WebControl object</param>
        /// <returns>AccelaWebControlExtenderExtender object</returns>
        public static AccelaWebControlExtenderExtender CreateDesinSupportExtender(WebControl control)
        {
            return CreateDesinSupportExtender(control, null);
        }

        /// <summary>
        /// Create design support extender
        /// </summary>
        /// <param name="control">WebControl object</param>
        /// <param name="templateAttribute">The template attribute.</param>
        /// <returns>AccelaWebControlExtenderExtender object</returns>
        public static AccelaWebControlExtenderExtender CreateDesinSupportExtender(WebControl control, TemplateAttribute templateAttribute)
        {
            AccelaWebControlExtenderExtender extender = new AccelaWebControlExtenderExtender();
            if (null == templateAttribute)
            {
                extender.IsTemplateField = false;
            }
            else
            {
                extender.IsTemplateField = true;
                extender.TemplateAttribute = templateAttribute;
            }

            extender.TargetControlID = control.ID;
            extender.ControlID = control.ID;
            if (control is IAccelaControl)
            {
                IAccelaControl ac = (IAccelaControl)control;
                extender.DefaultSubLabel = ac.GetDefaultSubLabel();
                extender.DefaultLanguageSubLabel = ac.GetDefaultLanguageSubLabel();
                extender.ClientVisible = ac.ClientVisible;
            }

            if (control is IAccelaNonInputControl)
            {
                extender.DefaultLabel = ((IAccelaNonInputControl)control).GetDefaultLabel();
                extender.LabelKey = ((IAccelaNonInputControl)control).LabelKey;
                extender.DefaultLanguageText = extender.IsTemplateField ? templateAttribute.DefaultLanguageLabel : ((IAccelaNonInputControl)control).GetDefaultLanguageText();
            }

            if (control is AccelaLinkButton)
            {
                AccelaLinkButton linkButton = (AccelaLinkButton)control;

                extender.ReportID = linkButton.ReportID;
                extender.EnableConfigureURL = linkButton.EnableConfigureURL;
                extender.EnableRecordTypeFilter = linkButton.EnableRecordTypeFilter;
                extender.ModuleName = linkButton.ModuleName;

                if (control is GridViewHeaderLabel)
                {
                    SetGridViewHeadInfo(control, extender);
                }
            }
            else if (control is AccelaLabel)
            {
                AccelaLabel label = (AccelaLabel)control;
                extender.LabelType = (int)label.LabelType;
                extender.SectionID = label.SectionID;
                extender.ModuleName = label.ModuleName;
                if (label.LabelType == LabelType.ApplicantText)
                {
                    extender.RestrictDisplay = (int)label.RestrictDisplay;
                }
                else if (label.LabelType == LabelType.HidableLabel)
                {
                    extender.ClientVisible = label.IsNotHided;
                }
                else if (label.LabelType == LabelType.BodyText || label.LabelType == LabelType.PageInstruction)
                {
                    // 1 indicates the latel type is for body text
                    control.Attributes.Add("ControlType", "1");
                }

                if (label.IsGridViewHeadLabel)
                {
                    SetGridViewHeadInfo(control, extender);
                }
            }
            else if (control is AccelaSectionTitleBar)
            {
                AccelaSectionTitleBar label = control as AccelaSectionTitleBar;
                extender.LabelType = (int)label.LabelType;
                extender.SectionID = label.SectionID;
                extender.PermissionValueId = label.PermissionValueId;
            }
            else if (control is AccelaDropDownList)
            {
                AccelaDropDownList ddl = (AccelaDropDownList)control;
                extender.SourceType = (int)ddl.SourceType;
                extender.StdCategory = ddl.StdCategory;
                extender.SectionID = ddl.SectionID;
                extender.ShowType = (int)ddl.ShowType;
                extender.MaxLength = ddl.MaxValueLength;
                extender.IsHiddenLabel = ddl.IsHiddenLabel;
            }
            else if (control is AccelaGridView)
            {
                AccelaGridView gv = (AccelaGridView)control;
                extender.SectionID = string.Format("{0}" + ACAConstant.SPLIT_CHAR + "{1}" + ACAConstant.SPLIT_CHAR, gv.GetModuleName(), gv.GridViewNumber);
                extender.GridColumnsVisible = gv.GridColumnsVisible;
                extender.GridViewRealWidth = gv.RealWidth;
                extender.GridViewAllowPaging = gv.AllowPaging;
                extender.GridViewPageSize = gv.PageSize;

                if (gv.ExtendedProperties != null && gv.ExtendedProperties.Contains(ACAConstant.TEMPLATE_GENUS_LEVEL_TYPE))
                {
                    extender.TemplateGenus = Convert.ToString(gv.ExtendedProperties[ACAConstant.TEMPLATE_GENUS_LEVEL_TYPE]);
                }
            }
            else if (control is AccelaRadioButton)
            {
                AccelaRadioButton radio = (AccelaRadioButton)control;
                extender.LabelType = (int)radio.Type;
                extender.SectionID = radio.SectionID;
                extender.ModuleName = radio.ModuleName;
                extender.SubContainerClientID = radio.SubContainerClientID;
            }

            if (control is AccelaDropDownList)
            {
                AccelaDropDownList ddl = (AccelaDropDownList)control;
                extender.ElementType = typeof(AccelaDropDownList).Name;
                extender.PositionID = ((int)ddl.PositionID).ToString();
                extender.AutoFillType = (int)ddl.AutoFillType;
            }
            else if (control is AccelaRadioButtonList)
            {
                AccelaRadioButtonList radioButtonList = (AccelaRadioButtonList)control;
                extender.ListType = (int)radioButtonList.ListType;
                extender.ElementType = control.GetType().Name;
            }
            else if (control is AccelaTextBox)
            {
                AccelaTextBox tb = (AccelaTextBox)control;
                extender.PositionID = ((int)tb.PositionID).ToString();
                extender.AutoFillType = (int)tb.AutoFillType;
                extender.ElementType = control.GetType().Name;
            }
            else if (control is AccelaDiv)
            {
                AccelaDiv div = (AccelaDiv)control;
                extender.ElementType = control.GetType().Name;
                extender.DivType = (int)div.DivType;
                extender.SectionID = div.SectionID;
            }
            else
            {
                extender.ElementType = control.GetType().Name;
            }

            if (control is AccelaLabel)
            {
                AccelaLabel label = control as AccelaLabel;
                ((AccelaLabel)control).Children.Add(extender);
            }
            else if (control is AccelaLinkButton)
            {
                AccelaLinkButton label = control as AccelaLinkButton;
                ((AccelaLinkButton)control).Children.Add(extender);
            }
            else if (control is AccelaRadioButton)
            {
                AccelaRadioButton radio = control as AccelaRadioButton;
                ((AccelaRadioButton)control).Children.Add(extender);
            }
            else
            {
                control.Controls.Add(extender);
            }

            return extender;
        }

        /// <summary>
        /// Register Validation JS
        /// </summary>
        /// <param name="control">The control.</param>
        public static void RegisterValidationJS(Control control)
        {
            string path = HttpContext.Current.Request.ApplicationPath;

            if (!path.EndsWith(ACAConstant.SLASH, StringComparison.InvariantCultureIgnoreCase))
            {
                path += ACAConstant.SLASH;
            }

            string url = path + "Scripts/Validation.js";
            ScriptManager.RegisterClientScriptInclude(control, control.GetType(), "ValidationScripts", url);
        }

        /// <summary>
        /// Render Field's SoundEX Icon
        /// </summary>
        /// <param name="control">Accela TextBox Control</param>
        /// <returns>Return the soundEX icon's html.</returns>
        public static string RenderSoundexIcon(AccelaTextBox control)
        {
            StringBuilder htmlText = new StringBuilder();

            if (control != null)
            {
                string altText = LabelConvertUtil.GetTextByKey("aca_common_label_soundexalt", control);
                altText = ScriptFilter.AntiXssHtmlEncode(LabelConvertUtil.RemoveHtmlFormat(altText));

                string src = control.Page.ClientScript.GetWebResourceUrl(typeof(AccelaTextBox), "Accela.Web.Controls.Assets.soundex.png");

                htmlText.AppendFormat("<span name='{0}_soundex' title='{1}' style='display:none'>", control.GetControlID(), altText);
                htmlText.AppendFormat("<img class='Soundex_Icon' src='{0}' alt='{1}'/>", src, altText);
                htmlText.Append("</span>");
            }

            return htmlText.ToString();
        }

        /// <summary>
        /// set grid view column head information
        /// </summary>
        /// <param name="control">head control</param>
        /// <param name="extender">admin supported extender</param>
        private static void SetGridViewHeadInfo(WebControl control, AccelaWebControlExtenderExtender extender)
        {
            Control grid = control;

            while (grid != null && !(grid is AccelaGridView))
            {
                grid = grid.Parent;
            }

            if (grid != null)
            {
                extender.ViewElementID = control.ID;
                extender.SectionID = ((AccelaGridView)grid).GridViewNumber;
                extender.GridColumnsVisible = ((AccelaGridView)grid).GridColumnsVisible;

                /*
                 * Set control's visible based on the Column's visible status.
                 * The Control's ID must same with AccelaTemplateField's AttributeName.
                 */
                foreach (DataControlField field in ((AccelaGridView)grid).Columns)
                {
                    AccelaTemplateField accelaTemplate = field as AccelaTemplateField;

                    if (accelaTemplate != null && accelaTemplate.AttributeName == control.ID && !accelaTemplate.Visible)
                    {
                        control.Visible = false;
                        break;
                    }
                }

                // set the template genus from the gridview extender
                foreach (Control child in grid.Controls)
                {
                    if (child is AccelaWebControlExtenderExtender)
                    {
                        AccelaWebControlExtenderExtender gridExtender = child as AccelaWebControlExtenderExtender;

                        extender.TemplateGenus = gridExtender.TemplateGenus;
                        break;
                    }
                }
            }
        }
    }
}
