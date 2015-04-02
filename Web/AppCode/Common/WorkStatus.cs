#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: WorkStatus.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2008-2014
 *
 *  Description:
 *
 *  Notes:
 *      $Id: WorkStatus.cs 277071 2014-08-11 03:38:26Z ACHIEVO\james.shi $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * 12-02-2008           Rainbow Zhou               Initial
 * </pre>
 */

#endregion Header

using System;
using System.Data;
using System.Text;

using Accela.ACA.BLL.Common;
using Accela.ACA.Common;
using Accela.ACA.Common.Common;
using Accela.ACA.Common.Util;
using Accela.ACA.WSProxy;

namespace Accela.ACA.Web.Common
{
    /// <summary>
    /// This class is used for construct workflow in cap detail page.
    /// </summary>
    public class WorkStatus
    {
        #region Fields

        /// <summary>
        /// the width of the process status' first cell
        /// </summary>
        private const string PROCESS_STATUS_FIRSTCELL = "38px";

        /// <summary>
        /// the width of the process status' second cell
        /// </summary>
        private const string PROCESS_STATUS_SECONDCELL = "95%";

        /// <summary>
        /// the width of process status' task name
        /// </summary>
        private const string PROCESS_STATUS_TASKNAME = "770px";

        /// <summary>
        /// the CSS of the even row
        /// </summary>
        private const string TAB_ROW_EVEN = "ACA_TabRow_Even ACA_TabRow_Even_FontSize";

        /// <summary>
        /// the CSS of the odd row
        /// </summary>
        private const string TAB_ROW_ODD = "ACA_TabRow_Odd ACA_TabRow_Odd_FontSize";

        /// <summary>
        /// the processing history list
        /// </summary>
        private DataTable _dtProcessingHistoryList;

        /// <summary>
        /// the processing list
        /// </summary>
        private DataTable _dtProcessingList;

        /// <summary>
        /// the module name
        /// </summary>
        private string _moduleName = string.Empty;

        /// <summary>
        /// the agency code
        /// </summary>
        private string _agencyCode = string.Empty;

        /// <summary>
        /// the table content that render to page
        /// </summary>
        private StringBuilder _tableContent = new StringBuilder();

        /// <summary>
        /// the table row's CSS
        /// </summary>
        private string _trCss = TAB_ROW_EVEN;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the WorkStatus class.
        /// </summary>
        /// <param name="agencyCode">agency Code</param>
        /// <param name="moduleName">module name</param>
        public WorkStatus(string agencyCode, string moduleName)
        {
            _moduleName = moduleName;
            _agencyCode = agencyCode;
        }

        #endregion Constructors

        #region Methods

        /// <summary>
        /// Get processing content through module name.
        /// </summary>
        /// <returns>processing content as string</returns>
        public string GetProcessingContent()
        {
            CapModel4WS cap = AppSession.GetCapModelFromSession(_moduleName);
            ProcessingStatus ps = new ProcessingStatus(cap);
            _dtProcessingList = ps.CreateDataSource();

            string result = string.Empty;

            if (_dtProcessingList != null &&
                _dtProcessingList.Rows.Count > 0)
            {
                _dtProcessingHistoryList = _dtProcessingList.Clone();
                ps.SetProcessingHistoryList(_dtProcessingList, ref _dtProcessingHistoryList);
                BuildProcessingRootNode(_tableContent, _dtProcessingList, _dtProcessingHistoryList);
                result = _tableContent.ToString();
            }

            return result;
        }

        /// <summary>
        /// Get Attribute Value for display(I18N) and handle with the value of Checked, Yes/No type for display.
        /// </summary>
        /// <param name="tsiModel">The TaskSpecificInfoModel4WS.</param>
        /// <returns>The check list comment.</returns>
        private static string GetChecklistCommentForDisplay(TaskSpecificInfoModel4WS tsiModel)
        {
            if (tsiModel == null)
            {
                return string.Empty;
            }

            string checklistComment = tsiModel.checklistComment;
            int fieldType = 0;

            // Get Field's Data Type
            if (ValidationUtil.IsNumber(tsiModel.checkboxInd))
            {
                fieldType = Convert.ToInt32(tsiModel.checkboxInd);
            }

            switch (fieldType)
            {
                case (int)FieldType.HTML_TEXTBOX_OF_NUMBER:
                case (int)FieldType.HTML_TEXTBOX_OF_CURRENCY:
                    if (ValidationUtil.IsNumber(checklistComment))
                    {
                        checklistComment = I18nNumberUtil.FormatNumberForUI(tsiModel.checklistComment);
                    }

                    break;
                case (int)FieldType.HTML_CHECKBOX:
                    checklistComment = ModelUIFormat.FormatYNLabel(tsiModel.checklistComment);
                    break;
                case (int)FieldType.HTML_RADIOBOX:
                    checklistComment = ModelUIFormat.FormatYNLabel(tsiModel.checklistComment);
                    break;
                case (int)FieldType.HTML_TEXTBOX_OF_DATE:
                    if (!string.IsNullOrEmpty(checklistComment))
                    {
                        DateTime dt = new DateTime();
                        bool isDate = I18nDateTimeUtil.TryParseFromWebService(checklistComment, out dt);
                        if (isDate)
                        {
                            checklistComment = I18nDateTimeUtil.ConvertDateStringFromWebServiceToUI(checklistComment);
                        }
                    }

                    break;
                case (int)FieldType.HTML_SELECTBOX:
                    if (tsiModel.valueList != null &&
                        tsiModel.valueList.Length > 0)
                    {
                        RefAppSpecInfoDropDownModel4WS[] refASIDDArray = tsiModel.valueList;
                        for (int k = 0; k < refASIDDArray.Length; k++)
                        {
                            if (refASIDDArray[k] != null && refASIDDArray[k].attrValue != null &&
                                refASIDDArray[k].attrValue.Equals(checklistComment))
                            {
                                checklistComment = I18nStringUtil.GetString(refASIDDArray[k].resAttrValue, refASIDDArray[k].attrValue);
                                break;
                            }
                        }
                    }

                    break;
                default:
                    break;
            }

            return checklistComment;
        }

        /// <summary>
        /// Add Additional Information Table Html Script
        /// </summary>
        /// <param name="drHistory">DataRow of Task Specific Information History Table</param>
        private void AppendAdditionalInfoTable(DataRow drHistory)
        {
            AppendAdditionalInfoTable(drHistory, _tableContent);
        }

        /// <summary>
        /// Add Additional Information Table Html Script
        /// </summary>
        /// <param name="drHistory">DataRow of Task Specific Information History Table</param>
        /// <param name="tableContent">Html Text Writer</param>
        private void AppendAdditionalInfoTable(DataRow drHistory, StringBuilder tableContent)
        {
            tableContent.Append("<table role='presentation' width='100%' border='1' cellpadding='0' cellspacing='0'>");

            TaskSpecificInfoModel4WS[] taskSpecItems = drHistory["AdditionalInfoItem"] as TaskSpecificInfoModel4WS[];
            if (taskSpecItems == null || taskSpecItems.Length == 0 ||
                IsDisplayAdditionalInfoTable(drHistory) == false)
            {
                tableContent.Append("<tr></tr></table>");
                return;
            }
            else
            {
                tableContent.Append(string.Format("<tr><td colspan=2 class='ACA_ALeft'><span>{0}</span>&nbsp;&nbsp;</td></tr>", LabelUtil.GetTextByKey("ACA_WorkStatus_Label_AdditionalInfo", _agencyCode, _moduleName)));
            }

            tableContent.Append("<tr><td width='3%' align='right'></td><td width='97%' class='ACA_ALeft'>");

            if (taskSpecItems.Length > 0)
            {
                tableContent.Append("<table role='presentation' width='100%' border='1' cellpadding='0' cellspacing='0' style='table-layout:fixed;'>");

                for (int i = 0; i < taskSpecItems.Length; i++)
                {
                    if (taskSpecItems[i] != null && taskSpecItems[i].checkboxDesc != null && taskSpecItems[i].checkboxDesc.Trim().Length > 0 && taskSpecItems[i].checklistComment != null && taskSpecItems[i].checklistComment.Trim().Length > 0 &&
                        taskSpecItems[i].vchDispFlag.Trim().ToUpper().Equals("Y"))
                    {
                        TaskSpecificInfoModel4WS tsiModel = taskSpecItems[i];
                        tableContent.Append("<tr>");
                        tableContent.Append("<td width='25%' class='ACA_ALeft' style='word-wrap:break-word;'><span>" + ScriptFilter.FilterScript(I18nStringUtil.GetString(tsiModel.resCheckboxDesc, tsiModel.checkboxDesc)) + "</span></td>");
                        tableContent.Append("<td width='*' class='ACA_ALeft' style='word-wrap:break-word;'><span>" + ScriptFilter.FilterScript(GetChecklistCommentForDisplay(tsiModel)) + "</span></td>");
                        tableContent.Append("</tr>");
                    }
                }

                tableContent.Append("</table>");
            }

            tableContent.Append("</td></tr></table>");
        }

        /// <summary>
        /// Add Task Status and Comment Html Script
        /// </summary>
        /// <param name="dr">DataRow of Task Status and Comment</param>
        /// <returns>True: Added Task Status Record; False: Not Added Task Status Record</returns>
        private bool AppendCommentTable(DataRow dr)
        {
            return AppendCommentTable(dr, _tableContent, _dtProcessingHistoryList);
        }

        /// <summary>
        /// Add Task Status and Comment Html Script
        /// </summary>
        /// <param name="dr">DataRow of Task Status and Comment</param>
        /// <param name="tableContent">Html Text Writer</param>
        /// <param name="dtHistory">History Data Table</param>
        /// <returns>True: Added Task Status Record; False: Not Added Task Status Record</returns>
        private bool AppendCommentTable(DataRow dr, StringBuilder tableContent, DataTable dtHistory)
        {
            string action = Convert.ToString(dr["Action"]);

            if (string.IsNullOrEmpty(action) || dtHistory == null ||
                dtHistory.Rows.Count == 0)
            {
                return false;
            }

            int count = 0;

            string commentID = CommonUtil.GetRandomUniqueID();

            tableContent.Append("<table role='presentation' width='100%' border='1' cellpadding='0' cellspacing='0'>");

            DataRow[] historyDRs = dtHistory.Select("ParentID=" + dr["ID"]);     
            
            int index = 0;
            int historyRows = historyDRs.Length;
            foreach (DataRow historyDR in historyDRs)
            {
                index++;
                if (index == 1)
                {
                    tableContent.Append("<tr class='ACA_TabRow_Bold'>");
                }
                else
                {
                    tableContent.Append("<tr class='ACA_TabRow_Italic'>");
                }

                bool isDisplayComment = false;

                string comment = historyDR["Comment"] == null ? string.Empty : historyDR["Comment"].ToString().Trim();

                //if 'comment' isn't null should display by owned privileges.
                //If only set contact permission,we must be consider the permission set in capcontact model in spear form.
                if (!string.IsNullOrEmpty(comment))
                {
                    if (ValidationUtil.IsYes(historyDR["IsRestrictView"].ToString()))
                    {
                        UserRolePrivilegeModel role = historyDR["UserRolePrivilege"] as UserRolePrivilegeModel;
                        var proxyUserRoleBll = ObjectFactory.GetObject<IProxyUserRoleBll>();
                        isDisplayComment = proxyUserRoleBll.HasReadOnlyPermission(AppSession.GetCapModelFromSession(_moduleName), role);
                    }
                }

                if (isDisplayComment)
                {
                    string altText = ScriptFilter.AntiXssHtmlEncode(LabelUtil.RemoveHtmlFormat(LabelUtil.GetTextByKey("img_alt_expand_icon", _agencyCode, _moduleName)));
                    string titleText = ScriptFilter.AntiXssHtmlEncode(LabelUtil.RemoveHtmlFormat(LabelUtil.GetTextByKey("ACA_Common_Label_Comment", _agencyCode, _moduleName)));
                    tableContent.AppendFormat("<td width='3%' class='ACA_vertical_align'><a id='lnk_{0}' href=\"javascript:void(0)\" title=\"{2}\" onclick='ControlDisplay($get(\"{0}\"), $get(\"img_{0}\"),true,$get(\"lnk_{0}\"),null)' class=\"NotShowLoading\"><img id='img_{0}' src='{1}' alt=\"{2}\" class=\"ACA_NoBorder\" style='cursor: pointer; padding-bottom: 3px;'></a></td>", commentID, ImageUtil.GetImageURL("plus_expand.gif"), altText);
                }
                else
                {
                    tableContent.Append("<td width='3%' class='ACA_ALeft'></td>");
                }

                count = count + 1;
                tableContent.Append("<td class='ACA_ALeft' width='715px;'>");
                tableContent.Append("<table role='presentation' width='100%' border='0' cellpadding='0' cellspacing='0'>");
                tableContent.Append("<tr>");
                tableContent.Append("<td class='ACA_ALeft' width='30%'>" + FormatProcessFromDataRow2String(historyDR) + "</td>");
                tableContent.Append("</tr>");
                tableContent.Append("</table>");
                tableContent.Append("</td>");
                tableContent.Append("</tr>");

                if (isDisplayComment)
                {
                    tableContent.Append("<tr id='" + commentID + "' style='display:none'>");
                    tableContent.Append("<td width='3%' align='right'></td>");
                    tableContent.Append("<td width='97%' class='ACA_ALeft'>");
                    tableContent.Append("<table role='presentation' border='0' cellpadding='0' cellspacing='0'>");
                    tableContent.Append("<tr>");

                    string commentDisplay = LabelUtil.GetTextByKey("ACA_Common_Label_Comment", _agencyCode, _moduleName);
                    tableContent.Append("<td><span style='font-size:1.2em;color: #555555;' class='ACA_Comments'>" + commentDisplay + "</span></td>");
                    tableContent.Append("<td>&nbsp;&nbsp;</td>");

                    tableContent.Append("<td><span>" + ScriptFilter.FilterScript(historyDR["Comment"].ToString()) + "</span></td>");
                    tableContent.Append("</tr>");
                    tableContent.Append("</table>");
                    tableContent.Append("</td>");
                    tableContent.Append("</tr>");
                }

                // Don't need to add a line if there is only one status.
                if (historyDR != historyDRs[historyDRs.Length - 1] ||
                    IsDisplayAdditionalInfoTable(dr))
                {
                    tableContent.Append("<tr>");
                    tableContent.Append("<td class='ACA_ALeft' width='100%' colspan='3'><hr></td>");
                    tableContent.Append("</tr>");
                }

                commentID = CommonUtil.GetRandomUniqueID();
            }

            tableContent.Append("</table>");

            return count > 0;
        }

        /// <summary>
        /// Append the image html
        /// </summary>
        /// <param name="dr">The data row.</param>
        /// <param name="trID">The table row's id.</param>
        /// <param name="isHasRecord">Whether has record.</param>
        private void AppendImgTDHtml(DataRow dr, string trID, bool isHasRecord)
        {
            AppendImgTDHtml(dr, trID, _tableContent, isHasRecord);
        }

        /// <summary>
        /// Append the image html
        /// </summary>
        /// <param name="dr">The data row.</param>
        /// <param name="trID">The table row's id.</param>
        /// <param name="tableContent">The table content.</param>
        /// <param name="isHasRecord">Whether has record.</param>
        private void AppendImgTDHtml(DataRow dr, string trID, StringBuilder tableContent, bool isHasRecord)
        {
            tableContent.Append("<td style='width:" + PROCESS_STATUS_FIRSTCELL + ";' class=\"ACA_ARight\">");
            tableContent.Append("<nobr>");

            //current status is complete, it will display icon by refenerce action status,
            if ((bool)dr["IsComplete"])
            {
                string actionStatus = dr["ActionStatusFlag"] == null ? string.Empty : dr["ActionStatusFlag"].ToString();

                if (WFActStatus.GoToBranchTask.Equals(actionStatus) || WFActStatus.GoToLoopTask.Equals(actionStatus))
                {
                    //if refenerce action status is "Go to Branch Task" or "Go to Loop Task", it will display active icon.
                    tableContent.Append(string.Format("<img title='{0}' src='{1}' alt='{2}' />", LabelUtil.GetTextByKey("workstatus_icontitle_previously_active", _agencyCode, _moduleName), ImageUtil.GetImageURL("asterisk_orange.png"), LabelUtil.GetTextByKey("img_alt_workflow_previously_active", _agencyCode, _moduleName)));
                }
                else
                {
                    //if refenerce action status is "Go to Next Task " or empty, it will display complete icon.
                    tableContent.Append(string.Format("<img title='{0}' src='{1}' alt='{2}' />", LabelUtil.GetTextByKey("workstatus_icontitle_complete", _agencyCode, _moduleName), ImageUtil.GetImageURL("complete.png"), LabelUtil.GetTextByKey("img_alt_workflow_complete", _agencyCode, _moduleName)));
                }

                tableContent.Append("&nbsp;&nbsp;");
            }
            else if ((bool)dr["IsActive"])
            {
                string activeTitle = LabelUtil.GetTextByKey("workstatus_icontitle_active", _agencyCode, _moduleName);
                activeTitle = string.Format("<img title='{0}' src='{1}' alt='{2}' />", activeTitle, ImageUtil.GetImageURL("active.png"), activeTitle);
                tableContent.Append(activeTitle);
                tableContent.Append("&nbsp;&nbsp;");
            }       
  
            DataRow[] historyList = _dtProcessingHistoryList.Select("ParentID=" + int.Parse(dr["ID"].ToString()));

            if (isHasRecord || (historyList.Length > 0 && !string.IsNullOrEmpty(Convert.ToString(dr["Action"]))))
            {
                string altText = ScriptFilter.AntiXssHtmlEncode(LabelUtil.RemoveHtmlFormat(LabelUtil.GetTextByKey("img_alt_expand_icon", _agencyCode, _moduleName)));
                tableContent.AppendFormat("<a id='lnk_{0}' href=\"javascript:void(0)\" title=\"{2}\" onclick='ControlDisplay($get(\"{0}\"), $get(\"img_{0}\"),false,$get(\"lnk_{0}\"),null)' class=\"NotShowLoading\"><img id='img_{0}' src='{1}' alt=\"{2}\" class=\"ACA_NoBorder\" style='cursor: hand'></a>", trID, ImageUtil.GetImageURL("caret_collapsed.gif"), altText);
                tableContent.Append("&nbsp;&nbsp;");
            }       

            tableContent.Append("</nobr></td>");
        }

        /// <summary>
        /// Build top layer data node and call sub processing function to build all data list
        /// </summary>
        /// <param name="tableContent">The Table tag used to record process content.</param>
        /// <param name="dtProcessStatus">The process status list.</param>
        /// <param name="dtHistory">The History item list.</param>
        private void BuildProcessingRootNode(StringBuilder tableContent, DataTable dtProcessStatus, DataTable dtHistory)
        {
            if (dtProcessStatus == null ||
                dtProcessStatus.Rows.Count == 0)
            {
                return;
            }

            tableContent.Append("<table width='100%' role='presentation' border='1' cellpadding='0' cellspacing='0'>");
            DataTable dt = dtProcessStatus.Clone();
            DataRow[] drs = dtProcessStatus.Select("ParentID=0");        

            foreach (DataRow dr in drs)
            {
                _trCss = _trCss == TAB_ROW_EVEN ? TAB_ROW_ODD : TAB_ROW_EVEN;
 
                DataRow[] drss = dtProcessStatus.Select("ParentID=" + dr["ID"]);
                FillRowInDataTable(drss, ref dt);
      
                if (dt.Rows.Count > 0)
                {
                    tableContent.Append("<tr class='" + _trCss + "'>");
                    tableContent.Append("<td class='ACA_ALeft' valign='top' width='100%' colSpan='2'>");

                    string trID = CommonUtil.GetRandomUniqueID();
                    tableContent.Append("<table role='presentation' width='100%' border='1' cellpadding='0' cellspacing='0'>");
                    tableContent.Append("<tr>");
                    AppendImgTDHtml(dr, trID, tableContent, true);
                    tableContent.Append("<td class='ACA_ALeft' width='" + PROCESS_STATUS_TASKNAME + "' >" + dr["TaskName"] + "</td>");
                    tableContent.Append("</tr>");

                    tableContent.Append("<tr id='" + trID + "' style='display:none'>");
                    tableContent.Append("<td width='" + PROCESS_STATUS_FIRSTCELL + "' ></td>");

                    //The row total width is 770px, left column width is 38px, so that right column is 732px;
                    tableContent.Append("<td width='732px;' >");

                    if (AppendCommentTable(dr, tableContent, dtHistory))
                    {
                        AppendAdditionalInfoTable(dr, tableContent);
                    }

                    BuildProcessingSubNode(dt);                    

                    tableContent.Append("</td></tr>");

                    tableContent.Append("</table>");

                    tableContent.Append("</td></tr>");
                    dt.Clear();
                }
                else
                {
                    string trID = CommonUtil.GetRandomUniqueID();
                    tableContent.Append("<tr class='" + _trCss + "'>");
                    AppendImgTDHtml(dr, trID, tableContent, false);
                    tableContent.Append("<td class='ACA_ALeft' width='" + PROCESS_STATUS_TASKNAME + "' >" + dr["TaskName"] + "</td>");
                    tableContent.Append("</tr>");

                    tableContent.Append("<tr class='" + _trCss + "' id='" + trID + "' style='display:none'>");
                    tableContent.Append("<td width='" + PROCESS_STATUS_FIRSTCELL + "' ></td>");
                    tableContent.Append("<td class='ACA_ALeft' width='" + PROCESS_STATUS_SECONDCELL + "' >");

                    if (AppendCommentTable(dr, tableContent, dtHistory))
                    {
                        AppendAdditionalInfoTable(dr, tableContent);
                    }

                    tableContent.Append("</td>");
                    tableContent.Append("</tr>");
                }
            }

            tableContent.Append("</table>");
        }

        /// <summary>
        /// Build sub data node and leaf node.
        /// </summary>
        /// <param name="processingList">Special layer data node</param>
        private void BuildProcessingSubNode(DataTable processingList)
        {
            _tableContent.Append("<table role='presentation'  width='100%' border='1' cellpadding='0' cellspacing='0'>");
            DataTable dt = processingList.Clone();
            foreach (DataRow dr in processingList.Rows)
            {
                // remove all DataTable group by process task.
                dt.Clear();
                string filterExpression = "ParentID=" + dr["ID"];
                DataRow[] drs = _dtProcessingList.Select(filterExpression);
                FillRowInDataTable(drs, ref dt);

                if (dt.Rows.Count > 0)
                {
                    _tableContent.Append("<tr>");
                    _tableContent.Append("<td class='ACA_ALeft' valign='top' width='100%' colSpan='2'>");
                    string trID = CommonUtil.GetRandomUniqueID();
                    _tableContent.Append("<table role='presentation'  width='100%' border='1' cellpadding='0' cellspacing='0'>");
                    _tableContent.Append("<tr>");
                    AppendImgTDHtml(dr, trID, true);
                    _tableContent.Append("<td class='ACA_ALeft' width='" + PROCESS_STATUS_TASKNAME + "' >" + dr["TaskName"] + "</td>");
                    _tableContent.Append("</tr>");
                    _tableContent.Append("<tr id='" + trID + "' style='display:none'>");
                    _tableContent.Append("<td width='" + PROCESS_STATUS_FIRSTCELL + "' ></td>");
                    _tableContent.Append("<td width='" + PROCESS_STATUS_SECONDCELL + "' >");

                    if (AppendCommentTable(dr))
                    {
                        AppendAdditionalInfoTable(dr);
                    }

                    BuildProcessingSubNode(dt);
                    _tableContent.Append("</td></tr>");
                    _tableContent.Append("</table>");
                    _tableContent.Append("</td></tr>");
                }
                else
                {
                    string trID = CommonUtil.GetRandomUniqueID();
                    _tableContent.Append("<tr>");
                    AppendImgTDHtml(dr, trID, false);
                    _tableContent.Append("<td class='ACA_ALeft' width='" + PROCESS_STATUS_TASKNAME + "' >" + dr["TaskName"] + "</td>");
                    _tableContent.Append("</tr>");

                    _tableContent.Append("<tr id='" + trID + "' style='display:none'>");
                    _tableContent.Append("<td width='" + PROCESS_STATUS_FIRSTCELL + "' ></td>");
                    _tableContent.Append("<td class='ACA_ALeft' width='" + PROCESS_STATUS_SECONDCELL + "' >");
                    if (AppendCommentTable(dr))
                    {
                        AppendAdditionalInfoTable(dr);
                    }

                    _tableContent.Append("</td>");
                    _tableContent.Append("</tr>");
                }
            }

            _tableContent.Append("</table>");
        }

        /// <summary>
        /// Copies data row list into a data table, preserving any
        /// property settings, as well as original and current values.
        /// </summary>
        /// <param name="drs">The process status.</param>
        /// <param name="dt">The processing list.</param>
        private void FillRowInDataTable(DataRow[] drs, ref DataTable dt)
        {
            foreach (DataRow dr in drs)
            {
                dt.ImportRow(dr);
            }
        }

        /// <summary>
        /// Check if display the Additional Information Table
        /// </summary>
        /// <param name="drHistory">DataRow of Task Specific Information History Table</param>
        /// <returns>True: Display; False: Not Display</returns>
        private bool IsDisplayAdditionalInfoTable(DataRow drHistory)
        {
            if (drHistory["AdditionalInfoItem"] == null)
            {
                return false;
            }

            TaskSpecificInfoModel4WS[] taskSpecItems = drHistory["AdditionalInfoItem"] as TaskSpecificInfoModel4WS[];

            if (taskSpecItems == null ||
                taskSpecItems.Length == 0)
            {
                return false;
            }

            bool isDisplayAdditionInfoTable = false;

            for (int i = 0; i < taskSpecItems.Length; i++)
            {
                if (taskSpecItems[i].fieldLabel != null && 
                    taskSpecItems[i].fieldLabel.Trim().Length > 0 && 
                    taskSpecItems[i].checklistComment != null && 
                    taskSpecItems[i].checklistComment.Trim().Length > 0 &&
                    ValidationUtil.IsYes(taskSpecItems[i].vchDispFlag))
                {
                    isDisplayAdditionInfoTable = true;
                    break;
                }
            }

            return isDisplayAdditionInfoTable;
        }

        /// <summary>
        /// Indicates whether need to display the Optional email address for work flow task.
        /// </summary>
        /// <param name="optionTask">V360 option value</param>
        /// <returns>True if mask Display E-mail in V360 to display e-mail information in Cap detail; Otherwise false.</returns>
        private bool IsDisplayEmailinTask(string optionTask)
        {
            IXPolicyBll policyBll = ObjectFactory.GetObject<IXPolicyBll>();
            string isDisplayEmailAddress = policyBll.GetValueByKey(ACAConstant.ACA_ENABLE_WF_DISP_EMAIL, _moduleName);

            return ValidationUtil.IsYes(isDisplayEmailAddress) && ValidationUtil.IsYes(optionTask);
        }

        /// <summary>
        /// Format process content from DataRow to string.
        /// </summary>
        /// <param name="dr">The process status list.</param>
        /// <returns>Process content as string.</returns>
        private string FormatProcessFromDataRow2String(DataRow dr)
        {
            string processPattern = LabelUtil.GetTextByKey("acaadmin_workflow_msg_configuration", _agencyCode, _moduleName);

            string dDate = dr["DueDate"] as string;

            // If date is not set, TBD instead.
            dDate = string.IsNullOrEmpty(dDate) ? LabelUtil.GetTextByKey("acaadmin_workflow_msg_nulldate", _agencyCode, _moduleName) : I18nDateTimeUtil.ConvertDateStringFromWebServiceToUI(dDate);
            processPattern = processPattern.Replace(WorkStatusVariables.DueDate, dDate);

            string assignEmail = dr["AssignEmail"] as string;
            string assignedTo = dr["UserName"] as string;
            if (processPattern.Contains(WorkStatusVariables.AssignedTo))
            {
                if (string.IsNullOrEmpty(assignedTo))
                {
                    // If assign-to is not assigned, replace TBD instead.
                    processPattern = processPattern.Replace(WorkStatusVariables.AssignedTo, LabelUtil.GetTextByKey("acaadmin_workflow_msg_nullstaff", _agencyCode, _moduleName));
                }
                else
                {
                    if (!string.IsNullOrEmpty(assignEmail) && IsDisplayEmailinTask(dr["IsDisplayEmail"].ToString()))
                    {
                        string emailTo = " (<a href= mailto:" + assignEmail + ">" + assignEmail + "</a>)";

                        // Mark the last index of assigned-to.
                        int lastIndex = processPattern.IndexOf(WorkStatusVariables.AssignedTo) + WorkStatusVariables.AssignedTo.Length;

                        // Append the email information behind assigned-to.
                        processPattern = processPattern.Insert(lastIndex, emailTo);
                    }

                    processPattern = processPattern.Replace(WorkStatusVariables.AssignedTo, assignedTo);
                }
            }

            string processStatus = dr["Action"] as string;
            processStatus = string.IsNullOrEmpty(processStatus) ? LabelUtil.GetTextByKey("acaadmin_workflow_msg_nullstatus", _agencyCode, _moduleName) : processStatus;
            processPattern = processPattern.Replace(WorkStatusVariables.TaskStatus, processStatus);

            string statusDate = dr["ActionTime"] as string;
            statusDate = string.IsNullOrEmpty(statusDate) ? LabelUtil.GetTextByKey("acaadmin_workflow_msg_nulldate", _agencyCode, _moduleName) : I18nDateTimeUtil.ConvertDateStringFromWebServiceToUI(statusDate);
            processPattern = processPattern.Replace(WorkStatusVariables.StatusDate, statusDate);

            string actionBy = dr["ActionBy"] as string;
            string actionEmail = dr["ActionEmail"] as string;
            if (processPattern.Contains(WorkStatusVariables.ActionBy))
            {
                if (string.IsNullOrEmpty(actionBy))
                {
                    processPattern = processPattern.Replace(WorkStatusVariables.ActionBy, LabelUtil.GetTextByKey("acaadmin_workflow_msg_nullstaff", _agencyCode, _moduleName));
                }
                else
                {
                    if (!string.IsNullOrEmpty(actionEmail) && IsDisplayEmailinTask(dr["IsDisplayEmail"].ToString()))
                    {
                        string emailTo = " (<a href= mailto:" + actionEmail + ">" + actionEmail + "</a>)";

                        // Mark the last index of action-by.
                        int lastIndex = processPattern.IndexOf(WorkStatusVariables.ActionBy) + WorkStatusVariables.ActionBy.Length;

                        // Append the email information behind action-by.
                        processPattern = processPattern.Insert(lastIndex, emailTo);
                    }

                    processPattern = processPattern.Replace(WorkStatusVariables.ActionBy, actionBy);
                }
            }

            return processPattern;
        }

        /// <summary>
        /// List item of work status.
        /// </summary>
        private struct WorkStatusVariables
        {
            /// <summary>
            /// Due date variable: task due date.
            /// </summary>
            public const string DueDate = "$$DueDate$$";

            /// <summary>
            /// Assign-to variable: to whom the task is assigned.
            /// </summary>
            public const string AssignedTo = "$$AssignedTo$$";

            /// <summary>
            /// Task status variable: current status of the task.
            /// </summary>
            public const string TaskStatus = "$$TaskStatus$$";

            /// <summary>
            /// Status date variable: effective date of the current status.
            /// </summary>
            public const string StatusDate = "$$StatusDate$$";

            /// <summary>
            /// Action-by variable: by whom the current status of the task is set.
            /// </summary>
            public const string ActionBy = "$$ActionBy$$";
        }

        #endregion Methods
    }
}
