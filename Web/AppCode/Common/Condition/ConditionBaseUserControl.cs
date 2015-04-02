#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: ConditionBaseUserControl.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2013-2014
 *
 *  Description:
 * The condition base user contorl
 *
 *  Notes:
 *      $Id: ConditionBaseUserControl.cs 131474 2009-05-20 02:34:33Z ACHIEVO\jackie.yu $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

using Accela.ACA.Common;
using Accela.ACA.Common.Common;
using Accela.ACA.Common.Util;
using Accela.ACA.WSProxy;
using Accela.Web.Controls;

namespace Accela.ACA.Web.Common
{
    /// <summary>
    /// The base class for condition user control.
    /// </summary>
    public abstract class ConditionBaseUserControl : BaseUserControl
    {
        #region Fields

        /// <summary>
        /// The button command name of showing those met
        /// </summary>
        protected const string BUTTON_COMMAND_SHOW_MET = "ShowMet";

        /// <summary>
        /// The button command name of hiding those met
        /// </summary>
        protected const string BUTTON_COMMAND_HIDE_MET = "HideMet";

        /// <summary>
        /// Limited the string value with specific length.
        /// </summary>
        private const int LIMITED_STRING_LENGTH = 200;

        /// <summary>
        /// Whether display condition of approval
        /// </summary>
        private bool? _isDisplayConditionsOfApproval;

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets a value indicating whether need grouped by record type
        /// </summary>
        public bool IsGroupByRecordType
        {
            get
            {
                object obj = ViewState["IsGroupByRecordType"];

                if (obj != null)
                {
                    return (bool)obj;
                }

                return false;
            }

            set
            {
                ViewState["IsGroupByRecordType"] = value;
            }
        }

        /// <summary>
        /// Gets or sets the label key of the general conditions' title.
        /// </summary>
        public string GeneralConditionsTitleLabelKey
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the label key of the conditions of approval's title.
        /// </summary>
        public string ConditionsOfApprovalTitleLabelKey
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the label key of the general conditions' pattern.
        /// </summary>
        public string GeneralConditionsPatternLabelKey
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets a value indicating whether hide the link for [hide/view] those met
        /// </summary>
        public bool HideLink4ViewMet
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets a value indicating whether condition date can config
        /// </summary>
        public bool ConditionDateUnconfigurable
        {
            get;
            set;
        }        

        /// <summary>
        /// Gets or sets the data source of general condition list.
        /// </summary>
        protected List<ConditionViewModel> GeneralConditionsDataSource
        {
            get
            {
                return ViewState["GeneralConditionsDataSource"] as List<ConditionViewModel>;
            }

            set
            {
                ViewState["GeneralConditionsDataSource"] = value;
            }
        }

        /// <summary>
        /// Gets or sets the data source of condition of approval list.
        /// </summary>
        protected List<ConditionViewModel> ConditionsOfApprovalDataSource
        {
            get
            {
                return ViewState["ConditionsOfApprovalDataSource"] as List<ConditionViewModel>;
            }

            set
            {
                ViewState["ConditionsOfApprovalDataSource"] = value;
            }
        }

        /// <summary>
        /// Gets a value indicating whether display condition of approval
        /// </summary>
        protected bool IsDisplayConditionsOfApproval
        {
            get
            {
                if (_isDisplayConditionsOfApproval == null)
                {
                    _isDisplayConditionsOfApproval = StandardChoiceUtil.IsDisplayConditionsOfApproval();
                }

                return _isDisplayConditionsOfApproval.Value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether show condition bar or not
        /// </summary>
        protected bool ShowConditionBar
        {
            get;
            set;
        }

        #endregion Properties
        
        #region Public Method

        /// <summary>
        /// Display the conditions, this overload will show the condition bar that show locked, hold or notice message.
        /// </summary>
        /// <param name="capWithConditionModel">A CapWithConditionModel4WS model.</param>
        /// <returns>whether the conditions display success or not.</returns>
        public bool Display(CapWithConditionModel4WS capWithConditionModel)
        {
            if (capWithConditionModel == null)
            {
                return false;
            }

            bool conditionBarSetOK = SetConditionBar(capWithConditionModel);

            // bind the condition list
            if (conditionBarSetOK)
            {
                ShowConditionBar = true;
            }

            SetConditionListDataSource(capWithConditionModel.conditionModelArray);
            BindConditionList();

            return IsDisplayConditionSection();
        }

        /// <summary>
        /// Displays the conditions.
        /// </summary>
        /// <param name="dictCondition">The condition dictionary.</param>
        /// <returns>whether the conditions display success or not.</returns>
        public bool Display(Dictionary<CapTypeModel, NoticeConditionModel[]> dictCondition)
        {
            SetConditionListDataSource(dictCondition);
            BindConditionList();

            return IsDisplayConditionSection();
        }

        #endregion Public Method

        #region Protected Methods

        /// <summary>
        /// Bind the condition list
        /// </summary>
        protected abstract void BindConditionList();

        /// <summary>
        /// Is display condition section or not
        /// </summary>
        /// <returns>return true or false to indicate the condition section show or not.</returns>
        protected abstract bool IsDisplayConditionSection();

        /// <summary>
        /// Set the condition bar's label
        /// </summary>
        /// <param name="dictControlLabel">The condition bar's control label</param>
        protected abstract void SetConditionBarLabel(Dictionary<string, string> dictControlLabel);

        /// <summary>
        /// List of general conditions row data bound
        /// </summary>
        /// <param name="row">Grid View Row</param>
        /// <param name="pageSize">Page size</param>
        protected void GeneralConditionsListRowDataBound(GridViewRow row, int pageSize)
        {
            if (row.RowType == DataControlRowType.DataRow)
            {
                ConditionViewModel itemData = row.DataItem as ConditionViewModel;

                if (itemData != null)
                {
                    bool isNewPage = row.RowIndex % pageSize == 0;

                    if (IsGroupByRecordType && (isNewPage || !itemData.HideRecordType))
                    {
                        HtmlGenericControl divGeneralConditionsRecordType = (HtmlGenericControl)row.FindControl("divGeneralConditionsRecordType");
                        AccelaLabel lblGeneralConditionsRecordType = (AccelaLabel)row.FindControl("lblGeneralConditionsRecordType");

                        divGeneralConditionsRecordType.Visible = true;
                        lblGeneralConditionsRecordType.Text = itemData.RecordType;
                    }

                    if (isNewPage || !itemData.HideGroup)
                    {
                        HtmlGenericControl divGeneralConditionsGroupName = (HtmlGenericControl)row.FindControl("divGeneralConditionsGroupName");
                        AccelaLabel lblGeneralConditionsGroupName = (AccelaLabel)row.FindControl("lblGeneralConditionsGroupName");
                        AccelaLabel lblGeneralConditionsGroupCount = (AccelaLabel)row.FindControl("lblGeneralConditionsGroupCount");

                        divGeneralConditionsGroupName.Visible = true;
                        lblGeneralConditionsGroupName.Text = itemData.GroupName;
                        lblGeneralConditionsGroupCount.Text = GetGroupStatisticsInfo(GeneralConditionsDataSource, itemData.GroupName, false);
                    }

                    if (isNewPage || !itemData.HideConditionType)
                    {
                        HtmlGenericControl divGeneralConditionsType = (HtmlGenericControl)row.FindControl("divGeneralConditionsType");
                        AccelaLabel lblGeneralConditionsType = (AccelaLabel)row.FindControl("lblGeneralConditionsType");

                        divGeneralConditionsType.Visible = true;
                        lblGeneralConditionsType.Text = itemData.ConditionType;
                    }

                    AccelaLabel lblGeneralConditionsInfo = (AccelaLabel)row.FindControl("lblGeneralConditionsInfo");
                    string generalConditionsPattern = LabelUtil.GetTextByKey(GeneralConditionsPatternLabelKey, ModuleName);

                    lblGeneralConditionsInfo.Text = ReplacePattern(generalConditionsPattern, itemData, false);
                }
            }
        }

        /// <summary>
        /// The list of condition of approval row data bound
        /// </summary>
        /// <param name="row">Grid View Row</param>
        /// <param name="pageSize">Page size</param>
        /// <param name="hideMetCondition">Hide met condition or not</param>
        protected void ConditionsOfApprovalListRowDataBound(GridViewRow row, int pageSize, bool hideMetCondition)
        {
            if (row.RowType == DataControlRowType.DataRow)
            {
                ConditionViewModel itemData = row.DataItem as ConditionViewModel;

                if (itemData != null)
                {
                    bool isNewPage = (row.RowIndex % pageSize == 0) ? true : false;

                    if (IsGroupByRecordType && (isNewPage || !itemData.HideRecordType))
                    {
                        HtmlGenericControl divConditionsOfApprovalRecordType = (HtmlGenericControl)row.FindControl("divConditionsOfApprovalRecordType");
                        AccelaLabel lblConditionsOfApprovalRecordType = (AccelaLabel)row.FindControl("lblConditionsOfApprovalRecordType");

                        divConditionsOfApprovalRecordType.Visible = true;
                        lblConditionsOfApprovalRecordType.Text = itemData.RecordType;
                    }

                    if (isNewPage || !itemData.HideGroup)
                    {
                        HtmlGenericControl divConditionsOfApprovalGroupName = (HtmlGenericControl)row.FindControl("divConditionsOfApprovalGroupName");
                        AccelaLabel lblConditionsOfApprovalGroupName = (AccelaLabel)row.FindControl("lblConditionsOfApprovalGroupName");
                        AccelaLabel lblConditionsOfApprovalGroupCount = (AccelaLabel)row.FindControl("lblConditionsOfApprovalGroupCount");

                        divConditionsOfApprovalGroupName.Visible = true;
                        lblConditionsOfApprovalGroupName.Text = itemData.GroupName;

                        lblConditionsOfApprovalGroupCount.Text = GetGroupStatisticsInfo(ConditionsOfApprovalDataSource, itemData.GroupName, hideMetCondition);
                    }

                    if (isNewPage || !itemData.HideConditionType)
                    {
                        HtmlGenericControl divConditionsOfApprovalType = (HtmlGenericControl)row.FindControl("divConditionsOfApprovalType");
                        AccelaLabel lblConditionsOfApprovalType = (AccelaLabel)row.FindControl("lblConditionsOfApprovalType");

                        divConditionsOfApprovalType.Visible = true;
                        lblConditionsOfApprovalType.Text = itemData.ConditionType;
                    }
                }
            }
        }

        /// <summary>
        /// Get the status count info of the group
        /// </summary>
        /// <param name="conditions">condition or condition of approval data source</param>
        /// <param name="groupName">group name</param>
        /// <param name="hideMet">if set to <c>true</c> [hide met].</param>
        /// <returns> string the status count info of the group</returns>
        protected string GetGroupStatisticsInfo(IList<ConditionViewModel> conditions, string groupName, bool hideMet)
        {
            StringBuilder sb = new StringBuilder();

            var statusGroups =
                from f in conditions
                where f.GroupName == groupName && (!hideMet || f.IsApplied == hideMet)
                group f by new { f.Status } into g
                select new { g.Key, g };

            foreach (var statusGroupItem in statusGroups)
            {
                sb.AppendFormat(" {0} {1},", statusGroupItem.g.Count(), statusGroupItem.Key.Status);
            }

            if (sb.Length > 0)
            {
                return "-" + sb.Remove(sb.Length - 1, 1);
            }

            return string.Empty;
        }

        /// <summary>
        /// Build the export Content
        /// </summary>
        /// <param name="conditionPatternLabelKeys">The label key of the condition pattern</param>
        /// <param name="list">The export data source</param>
        /// <returns>the export data</returns>
        protected string BuildExportContent(string[] conditionPatternLabelKeys, List<ConditionViewModel> list)
        {
            Dictionary<string, KeyValuePair<string, int>> sFields = new Dictionary<string, KeyValuePair<string, int>>();

            foreach (string patternLabelKey in conditionPatternLabelKeys)
            {
                string pattern = LabelUtil.GetTextByKey(patternLabelKey, ModuleName);

                AddExportContentField(sFields, pattern, ConditionsUtil.ListItemVariables.ConditionName, "per_conditionList_Label_name");
                AddExportContentField(sFields, pattern, ConditionsUtil.ListItemVariables.Status, "per_conditionList_Label_status");
                AddExportContentField(sFields, pattern, ConditionsUtil.ListItemVariables.Severity, "per_conditionList_Label_severity");
                AddExportContentField(sFields, pattern, ConditionsUtil.ListItemVariables.Priority, "aca_label_conditionlist_priority");
                AddExportContentField(sFields, pattern, ConditionsUtil.ListItemVariables.ShortComments, "aca_label_conditionlist_shortcomments");
                AddExportContentField(sFields, pattern, ConditionsUtil.ListItemVariables.LongComments, "aca_label_conditionlist_longcomments");
                AddExportContentField(sFields, pattern, ConditionsUtil.ListItemVariables.StatusDate, "aca_label_conditionlist_statusdate");
                AddExportContentField(sFields, pattern, ConditionsUtil.ListItemVariables.ActionByDept, "aca_label_conditionlist_actiondepartment");
                AddExportContentField(sFields, pattern, ConditionsUtil.ListItemVariables.ActionByUser, "aca_label_conditionlist_actionuser");
                AddExportContentField(sFields, pattern, ConditionsUtil.ListItemVariables.AppliedByDept, "aca_label_conditionlist_applieddepartment");
                AddExportContentField(sFields, pattern, ConditionsUtil.ListItemVariables.AppliedByUser, "aca_label_conditionlist_applieduser");
                AddExportContentField(sFields, pattern, ConditionsUtil.ListItemVariables.AppliedDate, "per_conditionList_Label_appliedDate");
                AddExportContentField(sFields, pattern, ConditionsUtil.ListItemVariables.EffectiveDate, "per_conditionList_Label_effectiveDate");
                AddExportContentField(sFields, pattern, ConditionsUtil.ListItemVariables.ExpirationDate, "per_conditionList_Label_expiredDate");
                AddExportContentField(sFields, pattern, ConditionsUtil.ListItemVariables.AdditionalInformation, "aca_label_conditionlist_additionalinformation");
            }

            // needFields will select and sort the exist fields of the pattern.
            var needFields = sFields.OrderBy(o => o.Value.Value);

            StringBuilder sbContent = new StringBuilder();
            StringBuilder sbPattern = new StringBuilder();

            // add the fixed header Record Type/Group Name/Condition Type
            if (IsGroupByRecordType)
            {
                sbContent.AppendFormat("{0}{1}", LabelUtil.GetTextByKey("aca_label_conditionlist_recordtype", ModuleName), ACAConstant.CultureInfoSplitChar);
            }

            sbContent.AppendFormat("{0}{1}", LabelUtil.GetTextByKey("aca_label_conditionlist_groupname", ModuleName), ACAConstant.CultureInfoSplitChar);
            sbContent.AppendFormat("{0}{1}", LabelUtil.GetTextByKey("aca_label_conditionlist_conditiontype", ModuleName), ACAConstant.CultureInfoSplitChar);

            // Add Header
            foreach (var itemField in needFields)
            {
                sbPattern.AppendFormat("{0}{1}", itemField.Key, ACAConstant.CultureInfoSplitChar);
                sbContent.AppendFormat("{0}{1}", LabelUtil.GetTextByKey(itemField.Value.Key, ModuleName), ACAConstant.CultureInfoSplitChar);
            }

            string strPattern = sbPattern.ToString();

            if (sbContent.Length > 0 && strPattern.Length > 0)
            {
                sbContent.Length -= 1;
                strPattern = strPattern.TrimEnd(ACAConstant.CultureInfoSplitChar.ToArray());
            }

            sbContent.Append("\r\n");
            strPattern += "\r\n";

            // Add content
            foreach (ConditionViewModel row in list)
            {
                if (IsGroupByRecordType)
                {
                    sbContent.AppendFormat("{0}{1}", ScriptFilter.FormatCSVContent(row.RecordType, true), ACAConstant.CultureInfoSplitChar);
                }

                sbContent.AppendFormat("{0}{1}", ScriptFilter.FormatCSVContent(row.GroupName, true), ACAConstant.CultureInfoSplitChar);
                sbContent.AppendFormat("{0}{1}", ScriptFilter.FormatCSVContent(row.ConditionType, true), ACAConstant.CultureInfoSplitChar);

                sbContent.Append(ReplacePattern(strPattern, row, true));
            }

            return sbContent.ToString();
        }

        /// <summary>
        /// Replace the pattern with the conditionViewModel data
        /// </summary>
        /// <param name="pattern">the Pattern</param>
        /// <param name="conditionViewModel">conditionViewModel data</param>
        /// <param name="isCSVFormat">Indicates whether it is CSV format.</param>
        /// <param name="isShowViewDetail">Indicates whether show view condition detail page.</param>
        /// <returns>the replaced pattern data</returns>
        protected string ReplacePattern(string pattern, ConditionViewModel conditionViewModel, bool isCSVFormat, bool isShowViewDetail = false)
        {
            if (string.IsNullOrEmpty(pattern))
            {
                return string.Empty;
            }

            string conditionName = FormatContent(conditionViewModel.ConditionName, isCSVFormat);
            string status = FormatContent(conditionViewModel.Status, isCSVFormat);
            string severity = FormatContent(conditionViewModel.Severity, isCSVFormat);
            string priority = FormatContent(conditionViewModel.PriorityText, isCSVFormat);
            string shortComments = FormatContent(conditionViewModel.ShortComments, isCSVFormat);
            string statusDate = FormatContent(conditionViewModel.StatusDateString, isCSVFormat);
            string appliedDate = FormatContent(conditionViewModel.AppliedDateString, isCSVFormat);
            string effectiveDate = FormatContent(conditionViewModel.EffectiveDateString, isCSVFormat);
            string expirationDate = FormatContent(conditionViewModel.ExpirationDateString, isCSVFormat);
            string additionalInformation = FormatContent(conditionViewModel.AdditionalInformation, isCSVFormat);
            string longComments = FormatContent(conditionViewModel.LongComments, isCSVFormat);
            string appliedByDept = FormatContent(conditionViewModel.AppliedByDept, isCSVFormat);
            string appliedByUser = FormatContent(conditionViewModel.AppliedByUser, isCSVFormat);
            string actionByDept = FormatContent(conditionViewModel.ActionByDept, isCSVFormat);
            string actionByUser = FormatContent(conditionViewModel.ActionByUser, isCSVFormat);
            string additionalInformationPart = Server.HtmlDecode(additionalInformation);
            string additionalInfoWithoutHtmlTag = ScriptFilter.RemoveHTMLTag(additionalInformationPart);

            if (!string.IsNullOrEmpty(additionalInfoWithoutHtmlTag) && additionalInfoWithoutHtmlTag.Length > LIMITED_STRING_LENGTH && !isCSVFormat)
            {
                //Sets the condition additional information value to session.
                string conditionKey = conditionViewModel.ServiceProviderCode + "_" + conditionViewModel.ConditionNbr.ToString();
                Dictionary<string, string> additionalInfo = AppSession.GetConditionAdditionalInfoFromSession();

                if (additionalInfo == null)
                {
                    additionalInfo = new Dictionary<string, string>();
                }
                
                if (additionalInfo.ContainsKey(conditionKey))
                {
                    additionalInfo[conditionKey] = additionalInformationPart;
                }
                else
                {
                    additionalInfo.Add(conditionKey, additionalInformationPart);
                }

                AppSession.SetConditionAdditionalInfoToSession(additionalInfo);

                //Initial the more link and get the additional information part value.
                string viewDetail = LabelUtil.GetGlobalTextByKey("aca_common_label_more");
                string more = string.Format(
                                    "<a href=\"javascript:void(0);\" class=\"NotShowLoading\" onclick=\"ViewConditionAdditionalInfoDetail(this,'{0}','{1}');\">{2}</a>",
                                    conditionViewModel.ServiceProviderCode, 
                                    conditionViewModel.ConditionNbr, 
                                    viewDetail);
                string tempContent = additionalInfoWithoutHtmlTag.Substring(0, LIMITED_STRING_LENGTH);

                additionalInformationPart = string.Format("{0}... <u><b>{1}</b></u>", tempContent, more);
            }          

            string result = pattern.Replace(ConditionsUtil.ListItemVariables.ConditionName, conditionName)
                                   .Replace(ConditionsUtil.ListItemVariables.Status, status)
                                   .Replace(ConditionsUtil.ListItemVariables.Severity, severity)
                                   .Replace(ConditionsUtil.ListItemVariables.Priority, priority)
                                   .Replace(ConditionsUtil.ListItemVariables.ShortComments, shortComments)
                                   .Replace(ConditionsUtil.ListItemVariables.StatusDate, statusDate)
                                   .Replace(ConditionsUtil.ListItemVariables.AppliedDate, appliedDate)
                                   .Replace(ConditionsUtil.ListItemVariables.EffectiveDate, effectiveDate)
                                   .Replace(ConditionsUtil.ListItemVariables.ExpirationDate, expirationDate)
                                   .Replace(ConditionsUtil.ListItemVariables.LongComments, longComments)
                                   .Replace(ConditionsUtil.ListItemVariables.AppliedByDept, appliedByDept)
                                   .Replace(ConditionsUtil.ListItemVariables.AppliedByUser, appliedByUser)
                                   .Replace(ConditionsUtil.ListItemVariables.ActionByDept, actionByDept)
                                   .Replace(ConditionsUtil.ListItemVariables.ActionByUser, actionByUser)
                                   .Replace(ConditionsUtil.ListItemVariables.AdditionalInformation, additionalInformationPart);

            if (isShowViewDetail)
            {
                CapModel4WS capModel = AppSession.GetCapModelFromSession(ModuleName);
                CapIDModel4WS capID = capModel.capID;
                string viewDetail = GetTextByKey("aca_recorddetail_conditionofapprovaldetail_link");

                result = result.Replace(ConditionsUtil.ListItemVariables.ViewDetail, "<a href=\"javascript:void(0);\" class=\"NotShowLoading\" onclick=\"ViewConditionDetail(this,'" + capID.serviceProviderCode + "','" + capID.id1 + "','" + capID.id2 + "','" + capID.id3 + "','" + conditionViewModel.ConditionNbr + "','" + ModuleName + "');\">" + viewDetail + "</a>");
            }

            return result;
        }

        /// <summary>
        /// Initialize the approval condition's export link. 
        /// </summary>
        /// <param name="gdvConditionsOfApprovalList">The grid view for conditions of approval list.</param>
        protected void InitialExport(AccelaGridView gdvConditionsOfApprovalList)
        {
            if (!AppSession.IsAdmin && StandardChoiceUtil.IsEnableExport2CSV())
            {
                gdvConditionsOfApprovalList.ShowExportLink = true;
                gdvConditionsOfApprovalList.ExportFileName = "ConditionsOfApprovalList";
            }
            else
            {
                gdvConditionsOfApprovalList.ShowExportLink = false;
            }
        }

        #endregion Protected Methods

        #region Private Methods

        /// <summary>
        /// Sets the condition bar.
        /// </summary>
        /// <param name="capWithConditionModel">The CapWithConditionModel4WS model.</param>
        /// <returns>Whether the condition bar set success or not.</returns>
        private bool SetConditionBar(CapWithConditionModel4WS capWithConditionModel)
        {
            bool success = false;

            if (capWithConditionModel == null || capWithConditionModel.conditionModel == null)
            {
                return false;
            }

            // get the condition serverity
            string conditionSeverity = string.Empty;
            string conditionSeverityText = string.Empty;
            NoticeConditionModel primaryCondition = capWithConditionModel.conditionModel;

            if (primaryCondition.impactCode != null)
            {
                conditionSeverity = primaryCondition.impactCode.ToUpperInvariant();
                conditionSeverityText = LabelUtil.GetGuiTextForCapConditionSeverity(conditionSeverity);

                if (StandardChoiceUtil.IsSuperAgency())
                {
                    conditionSeverity = FilterServiceLockCondition(capWithConditionModel, conditionSeverity);
                }
            }

            if (string.IsNullOrEmpty(conditionSeverity))
            {
                return false;
            }

            // get the applied date string
            string appliedDateString = string.Empty;

            if (primaryCondition.auditDate != null)
            {
                DateTime appliedDate = primaryCondition.auditDate.Value;
                appliedDateString = string.Format("<span dir=\"ltr\">{0}</span>.", I18nDateTimeUtil.FormatToDateStringForUI(appliedDate));
            }

            string resConditionDescription = I18nStringUtil.GetString(primaryCondition.resConditionDescription, primaryCondition.conditionDescription);
            resConditionDescription = ScriptFilter.FilterScript(resConditionDescription);

            Dictionary<string, string> dictControlLabel = new Dictionary<string, string>();

            // get the condition bar infomation by the condition serverity
            if (conditionSeverity.Equals(ACAConstant.LOCK_CONDITION, StringComparison.InvariantCulture))
            {
                string text = BuildConditionBarMessage("per_condition_locked", appliedDateString, resConditionDescription, conditionSeverityText, capWithConditionModel.conditionModelArray);
                dictControlLabel.Add(conditionSeverity, text);
                
                success = true;
            }
            else if (conditionSeverity.Equals(ACAConstant.HOLD_CONDITION, StringComparison.InvariantCulture))
            {
                string text = BuildConditionBarMessage("per_condition_hold", appliedDateString, resConditionDescription, conditionSeverityText, capWithConditionModel.conditionModelArray);
                dictControlLabel.Add(conditionSeverity, text);
                
                success = true;
            }
            else if (conditionSeverity.Equals(ACAConstant.NOTICE_CONDITION, StringComparison.InvariantCulture))
            {
                string text = BuildConditionBarMessage("per_condition_notice", appliedDateString, resConditionDescription, conditionSeverityText, capWithConditionModel.conditionModelArray);
                dictControlLabel.Add(conditionSeverity, text);
                
                success = true;
            }
            else if (conditionSeverity.Equals(ACAConstant.REQUIRED_CONDITION, StringComparison.InvariantCulture))
            {
                string text = BuildConditionBarMessage("aca_conditionbar_required", appliedDateString, resConditionDescription, conditionSeverityText, capWithConditionModel.conditionModelArray);
                dictControlLabel.Add(conditionSeverity, text);

                success = true;
            }

            SetConditionBarLabel(dictControlLabel);

            return success;
        }

        /// <summary>
        /// Build the condition bar message.
        /// </summary>
        /// <param name="conditionType">condition message type</param>
        /// <param name="dateDescription">date description.</param>
        /// <param name="resConditionDescription">condition description</param>
        /// <param name="capConditionText">cap condition</param>
        /// <param name="conditionArr">The condition array.</param>
        /// <returns>Condition Message</returns>
        private string BuildConditionBarMessage(string conditionType, string dateDescription, string resConditionDescription, string capConditionText, NoticeConditionModel[] conditionArr)
        {
            StringBuilder warningInfo = new StringBuilder();

            warningInfo.Append(GetTextByKey(conditionType).Trim() + "&nbsp;" + dateDescription);
            warningInfo.Append("<br/>");
            warningInfo.Append("<table role='presentation'>");
            warningInfo.Append("<tr valign=\"top\"><td style=\"white-space:nowrap;\">");
            warningInfo.Append(ConditionsUtil.AddSpaceWithFormat(GetTextByKey("per_condition_description")));
            warningInfo.Append("</td><td class='condition_notice_name'>");

            warningInfo.Append(resConditionDescription);
            warningInfo.Append("</td><td align=\"left\">");
            warningInfo.Append(ConditionsUtil.AddSpaceWithFormat(GetTextByKey("per_severity_descirption")));
            warningInfo.Append("</td><td>");
            warningInfo.Append(ScriptFilter.FilterScript(capConditionText));
            warningInfo.Append("</td></tr></table>");

            warningInfo.Append(ConditionsUtil.ComposeConditionSummary(conditionArr));

            return warningInfo.ToString();
        }

        /// <summary>
        /// Filter service locked condition in super agency.
        /// </summary>
        /// <param name="capWithConditionModel">a CapWithConditionModel4WS</param>
        /// <param name="conditionSeverity">the condition severity.</param>
        /// <returns>string for filter service</returns>
        private string FilterServiceLockCondition(CapWithConditionModel4WS capWithConditionModel, string conditionSeverity)
        {
            if (capWithConditionModel == null || capWithConditionModel.conditionModelArray == null)
            {
                return string.Empty;
            }

            // Need filter the service lock condition, so if the highest condition is service lock, should find the next one to replace it.
            if (conditionSeverity.Equals(ACAConstant.SERVICE_LOCK_CONDITION, StringComparison.InvariantCultureIgnoreCase))
            {
                conditionSeverity = string.Empty;
            }

            IList<NoticeConditionModel> tempConditions = new List<NoticeConditionModel>();

            foreach (NoticeConditionModel conditionModel in capWithConditionModel.conditionModelArray)
            {
                if (conditionModel == null || conditionModel.impactCode == null)
                {
                    tempConditions.Add(conditionModel);
                }
                else if (!conditionModel.impactCode.Equals(ACAConstant.SERVICE_LOCK_CONDITION, StringComparison.InvariantCultureIgnoreCase))
                {
                    if (string.IsNullOrEmpty(conditionSeverity))
                    {
                        conditionSeverity = conditionModel.impactCode.ToUpperInvariant();
                    }

                    tempConditions.Add(conditionModel);
                }
            }

            if (tempConditions.Count > 0)
            {
                NoticeConditionModel[] noticeModels = new NoticeConditionModel[tempConditions.Count];
                tempConditions.CopyTo(noticeModels, 0);

                capWithConditionModel.conditionModelArray = noticeModels;
            }

            return conditionSeverity;
        }

        /// <summary>
        /// Sets the condition list data source.
        /// </summary>
        /// <param name="conditions">The conditions array.</param>
        private void SetConditionListDataSource(NoticeConditionModel[] conditions)
        {
            if (GeneralConditionsDataSource == null)
            {
                GeneralConditionsDataSource = ConditionsUtil.GetConditionViewList(conditions, false);
            }

            if (IsDisplayConditionsOfApproval && ConditionsOfApprovalDataSource == null)
            {
                ConditionsOfApprovalDataSource = ConditionsUtil.GetConditionViewList(conditions, true);
            }
        }

        /// <summary>
        /// Sets the condition list data source.
        /// </summary>
        /// <param name="dictCondition">The condition dictionary.</param>
        private void SetConditionListDataSource(Dictionary<CapTypeModel, NoticeConditionModel[]> dictCondition)
        {
            if (GeneralConditionsDataSource == null)
            {
                GeneralConditionsDataSource = ConditionsUtil.GetConditionViewList(dictCondition, false, IsGroupByRecordType);
            }

            if (IsDisplayConditionsOfApproval && ConditionsOfApprovalDataSource == null)
            {
                ConditionsOfApprovalDataSource = ConditionsUtil.GetConditionViewList(dictCondition, true, IsGroupByRecordType);
            }
        }

        /// <summary>
        /// Format content if need.
        /// </summary>
        /// <param name="content">The content string.</param>
        /// <param name="isCSVFormat">Indicates whether it is CSV format.</param>
        /// <returns>Return the formatted content if need.</returns>
        private string FormatContent(string content, bool isCSVFormat)
        {
            return isCSVFormat ? ScriptFilter.FormatCSVContent(content, true) : content;
        }

        /// <summary>
        /// Add the export content's field
        /// </summary>
        /// <param name="dictField">The the field that add to dictionary</param>
        /// <param name="pattern">The pattern that for export</param>
        /// <param name="dictKey">The key for dictionary</param>
        /// <param name="labelKey">The label key for field</param>
        private void AddExportContentField(Dictionary<string, KeyValuePair<string, int>> dictField, string pattern, string dictKey, string labelKey)
        {
            int index = pattern.IndexOf(dictKey);

            if (!dictField.ContainsKey(dictKey) && index > -1)
            {
                dictField.Add(dictKey, new KeyValuePair<string, int>(labelKey, index));
            }
        }

        #endregion Private Methods
    }
}