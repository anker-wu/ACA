#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: AppSpecInfoView.ascx.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2008-2014
 *
 *  Description:
 *
 *  Notes:
 *      $Id: AppSpecInfoView.ascx.cs 278418 2014-09-03 08:54:25Z ACHIEVO\james.shi $.
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

using Accela.ACA.Common;
using Accela.ACA.Common.Common;
using Accela.ACA.Common.Util;
using Accela.ACA.Web.Common;
using Accela.ACA.Web.ExpressionBuild;
using Accela.ACA.WSProxy;
using Accela.Web.Controls;

namespace Accela.ACA.Web.Component
{
    /// <summary>
    /// the class for AppSpecInfoView.
    /// </summary>
    public partial class AppSpecInfoView : BaseUserControl
    {
        /// <summary>
        /// Gets a value indicating whether display Hijri Calendar 
        /// </summary>
        private bool IsDisplayHijriCalendar
        {
            get
            {
                return StandardChoiceUtil.IsEnableDisplayISLAMICCalendar();
            }
        }

        #region Methods

        /// <summary>
        /// Display specific information
        /// </summary>
        /// <param name="appSpecInfoGroups">App SpecInfo Group Model Array</param>
        /// <param name="editBtnClientClick">Edit Button Client Click</param>
        /// <param name="editBtnHandler">Edit Button Handler</param>
        /// <param name="sectionName">section name.</param>
        /// <param name="componentSeqNbr">Component Sequence Number</param>
        public void Display(AppSpecificInfoGroupModel4WS[] appSpecInfoGroups, string editBtnClientClick, EventHandler editBtnHandler, string sectionName, long componentSeqNbr)
        {
            try
            {
                //loop each field in each group and create corresponding control on UI
                if (appSpecInfoGroups == null)
                {
                    return;
                }

                if (ACAConstant.ASISecurity.None.Equals(ASISecurityUtil.GetASISecurity(appSpecInfoGroups, ModuleName)))
                {
                    return;
                }

                phPlumbingGroup.Controls.Clear();

                int groupIndex = 0; //indicates which group a field belongs to
                var expressionResult = ExpressionUtil.GetExpressionResultFromSession();

                foreach (AppSpecificInfoGroupModel4WS groupModel in appSpecInfoGroups)
                {
                    if (groupModel.fields == null)
                    {
                        continue;
                    }

                    string agencyCode = groupModel.capID == null ? ConfigManager.AgencyCode : groupModel.capID.serviceProviderCode;

                    if (ACAConstant.ASISecurity.None.Equals(ASISecurityUtil.GetASISecurity(agencyCode, groupModel.groupCode, groupModel.groupName, string.Empty, ModuleName))
                        || ASISecurityUtil.IsAllFieldsNoAccess(groupModel, ModuleName))
                    {
                        continue;
                    }

                    StringBuilder subGroup = new StringBuilder();
                    subGroup.Append("<div class=\"ACA_TabRow ACA_Title_Text ACA_SmLabel\">");
                    subGroup.Append("<table cellspacing=\"0\" cellpadding=\"0\" border=\"0\" class=\"ACA_FullWidthTable\" role=\"presentation\">");
                    subGroup.Append("<tbody><tr>");
                    subGroup.Append("<td>");

                    string groupName = I18nStringUtil.GetString(groupModel.resAlternativeLabel, groupModel.alternativeLabel, groupModel.resGroupName, groupModel.groupName);

                    bool isSuperCAP = CapUtil.IsSuperCAP(ModuleName);
                    if (groupModel.capID != null && isSuperCAP)
                    {
                        string logoDescription = string.Format("{0} ({1})", groupName, groupModel.capID.serviceProviderCode);
                        string agencyLogo = CapUtil.GetAgencyLogoHtml(groupModel.capID.serviceProviderCode, ModuleName);

                        if (!string.IsNullOrEmpty(agencyLogo))
                        {
                            subGroup.Append("<table role='presentation'><tr><td>");
                            subGroup.Append(agencyLogo);
                        }

                        subGroup.Append("<div class=\"ACA_ValCal_Title ACA_Label_FontSize_Restore\">");
                        subGroup.Append(logoDescription);
                        subGroup.Append("</div>");

                        if (!string.IsNullOrEmpty(agencyLogo))
                        {
                            subGroup.Append("</td></tr></table>");
                        }
                    }
                    else
                    {
                        //Normal agency.
                        if (groupModel.capID == null)
                        {
                            CapModel4WS capModel = AppSession.GetCapModelFromSession(ModuleName);
                            groupModel.capID = capModel != null ? capModel.capID : null;
                        }

                        subGroup.AppendFormat("<span class=\"ACA_FLeft ACA_Label_FontSize_Restore\">{0}</span>", groupName);
                    }

                    subGroup.Append("</td>");
                    subGroup.Append("<td><div class=\"ACA_FRight\"><table cellspacing=\"0\" cellpadding=\"0\" border=\"0\" role=\"presentation\"><tbody><tr><td><div class=\"ACA_Title_Button\">");
                    phPlumbingGroup.Controls.Add(new LiteralControl(subGroup.ToString()));

                    // add the edit button
                    string btnEditId = string.Format("btnEdit{0}_{1}Info", groupModel.groupCode, groupIndex);
                    CreateLinkButton(phPlumbingGroup.Controls, btnEditId, editBtnClientClick, editBtnHandler, sectionName, componentSeqNbr);

                    subGroup = new StringBuilder();
                    subGroup.Append("</div></td>");
                    subGroup.Append("<td><div class=\"ACA_Title_Button\">&nbsp;&nbsp;&nbsp;</div></td></tr></tbody></table></div></td>");
                    subGroup.Append("</tr></tbody>");
                    subGroup.Append("</table>");
                    subGroup.Append("</div>");

                    phPlumbingGroup.Controls.Add(new LiteralControl(subGroup.ToString()));

                    List<AppSpecificInfoModel4WS> visibleFields = new List<AppSpecificInfoModel4WS>();
                    List<AppSpecificInfoModel4WS> hiddenFields = new List<AppSpecificInfoModel4WS>();

                    ExpressionFieldModel[] expFields = null;

                    if (groupModel.capID != null && expressionResult != null && expressionResult.Count > 0
                        && expressionResult.ContainsKey(groupModel.capID.id1 + groupModel.capID.id2 + groupModel.capID.id3))
                    {
                        expFields = expressionResult[groupModel.capID.id1 + groupModel.capID.id2 + groupModel.capID.id3].Value.fields;
                    }

                    foreach (AppSpecificInfoModel4WS field in groupModel.fields)
                    {
                        if (ValidationUtil.IsHidden(field.vchDispFlag))
                        {
                            hiddenFields.Add(field);
                            continue;
                        }

                        ExpressionFieldModel expField = null;

                        if (expFields != null && expFields.Length > 0)
                        {
                            string key = string.Format("ASI::{0}::{1}", field.checkboxType, field.fieldLabel);
                            expField = expFields.FirstOrDefault(f => f.variableKey.Equals(key) && f.servProvCode.Equals(field.serviceProviderCode));
                        }

                        if (expField != null && expField.hidden != null && expField.hidden.Value)
                        {
                            hiddenFields.Add(field);
                            continue;
                        }

                        visibleFields.Add(field);
                    }

                    RenderHiddenFields(hiddenFields);

                    int itemIndex = 0;
                    var isOddColumn = false;

                    foreach (AppSpecificInfoModel4WS item in visibleFields)
                    {
                        if (item == null || item.vchDispFlag == null || ValidationUtil.IsNo(item.vchDispFlag))
                        {
                            continue;
                        }

                        if (ACAConstant.ASISecurity.None.Equals(ASISecurityUtil.GetASISecurity(item, ModuleName)))
                        {
                            continue;
                        }

                        string fieldLable = Server.HtmlEncode(CapUtil.GetASIFieldLabel(item));
                        ExpressionFieldModel expField = null;

                        if (expFields != null && expFields.Length > 0)
                        {
                            string key = string.Format("ASI::{0}::{1}", item.checkboxType, item.fieldLabel);
                            expField = expFields.FirstOrDefault(f => f.variableKey.Equals(key) && f.servProvCode.Equals(item.serviceProviderCode));
                        }

                        string fieldValue = ScriptFilter.FilterScript(CapUtil.GetASIFieldValue(item, expField));

                        if (IsDisplayHijriCalendar && item.fieldType == ((int)FieldType.HTML_TEXTBOX_OF_DATE).ToString())
                        {
                            fieldValue = HijriDateUtil.ToHijriDate(fieldValue);
                        }

                        if (groupModel.columnLayout == 2)
                        {
                            isOddColumn = itemIndex % 2 == 0;
                            LayoutASIWith2Columns(fieldLable, fieldValue, isOddColumn);

                            itemIndex++;
                        }
                        else
                        {
                            LayoutASIWith1Column(fieldLable, fieldValue);
                        }
                    }

                    //The last column is odd column add div close tag.
                    if (groupModel.columnLayout == 2 && isOddColumn)
                    {
                        phPlumbingGroup.Controls.Add(new LiteralControl("</div>"));
                    }

                    if (appSpecInfoGroups.Length > 1 && groupIndex < appSpecInfoGroups.Length - 1)
                    {
                        phPlumbingGroup.Controls.Add(new LiteralControl("<div class=\"ACA_TabRow ACA_Line_Content\">&nbsp;</div>"));
                    }

                    groupIndex++;
                }
            }
            catch (ACAException e)
            {
                MessageUtil.ShowMessage(Page, MessageType.Error, e.Message);
            }
        }

        /// <summary>
        /// Layout asi view with one column
        /// </summary>
        /// <param name="fieldLabel">Label of the control.</param>
        /// <param name="fieldValue">Value of the control.</param>
        private void LayoutASIWith1Column(string fieldLabel, string fieldValue)
        {
            string labelWidth = StandardChoiceUtil.GetASILabelWidth();
            string widthStyle = string.IsNullOrEmpty(labelWidth) ? string.Empty : " style='width:" + labelWidth + ";'";

            string htmlTemplate = string.Format(
                                                @"<div class='ACA_TabRow ACA_SmLabel ACA_SmLabel_FontSize ACA_Overflow'>
                                                    <div attr='parent_table' class='ACA_FLeft ACA_SmLabelBolder'{0}>{1}</div>
                                                    <div class='ACA_Page_NoScrollBar ACA_SmLabel'>{2}</div>
                                                  </div>",
                                                widthStyle,
                                                fieldLabel,
                                                fieldValue);

            phPlumbingGroup.Controls.Add(new LiteralControl(htmlTemplate));
        }

        /// <summary>
        /// Layout asi view with two column
        /// </summary>
        /// <param name="fieldLabel">The field label.</param>
        /// <param name="fieldValue">The value control.</param>
        /// <param name="isOddColumn">Is odd column or not.</param>
        private void LayoutASIWith2Columns(string fieldLabel, string fieldValue, bool isOddColumn)
        {
            string labelWidth = StandardChoiceUtil.GetASILabelWidth();
            string widthStyle = string.IsNullOrEmpty(labelWidth) ? string.Empty : " style='width:" + labelWidth + ";'";

            string htmlTemplate = string.Empty;

            if (isOddColumn)
            {
                htmlTemplate += "<div class='ACA_TabRow ACA_SmLabel ACA_SmLabel_FontSize ACA_Overflow'>";
            }

            htmlTemplate += string.Format(
                                          @"<div class='ACA_FLeft {3}'>
                                                <span attr='parent_table' class='ACA_SmLabelBolder'{0}>{1}</span>
                                                <span class='ACA_SmLabel'>{2}</span>
                                           </div>",
                                          widthStyle,
                                          fieldLabel,
                                          fieldValue,
                                          isOddColumn ? "ASIReview2OddColumn" : "ASIReview2EvenColumn");

            if (!isOddColumn)
            {
                htmlTemplate += "</div>";
            }

            phPlumbingGroup.Controls.Add(new LiteralControl(htmlTemplate));
        }

        /// <summary>
        /// Display hidden fields.
        /// </summary>
        /// <param name="hiddenFields">Hidden Fields</param>
        private void RenderHiddenFields(IEnumerable<AppSpecificInfoModel4WS> hiddenFields)
        {
            if (!hiddenFields.Any())
            {
                return;
            }

            foreach (var item in hiddenFields)
            {
                string lblFieldLabel = Server.HtmlEncode(CapUtil.GetASIFieldLabel(item));

                string htmlTemplate = string.Format(
                                                    @"<div class='ACA_Hide'>
                                                        <div>{0}</div>
                                                        <div>{1}</div>
                                                      </div>",
                                                    lblFieldLabel,
                                                    string.Empty);

                phPlumbingGroup.Controls.Add(new LiteralControl(htmlTemplate));
            }
        }

        /// <summary>
        /// create link button
        /// </summary>
        /// <param name="controls">a control collection that the link button added to</param>
        /// <param name="id">the id of the link button</param>
        /// <param name="onClientClick">the button's client event script name</param>
        /// <param name="handler">the button 's event handle</param>
        /// <param name="sectionName">section name.</param>
        /// <param name="componentSeqNbr">The component sequence number that identify the component.</param>
        private void CreateLinkButton(ControlCollection controls, string id, string onClientClick, EventHandler handler, string sectionName, long componentSeqNbr)
        {
            AccelaButton button = new AccelaButton();
            button.ID = id;
            button.LabelKey = "per_permitConfirm_label_editButton";
            button.Click += handler;
            button.DivEnableCss = "ACA_SmButton ACA_SmButton_FontSize ACA_Button_Text ACA_FRight ACA_ASIButtonContainer";
            button.CausesValidation = false;
            button.CommandArgument = string.Format("{1}{0}{2}", ACAConstant.SPLIT_CHAR, componentSeqNbr, sectionName);

            if (!string.IsNullOrEmpty(onClientClick))
            {
                button.OnClientClick = onClientClick;
            }

            controls.Add(button);
        }

        #endregion Methods
    }
}