#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: AMCAWorkStatus.ascx.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2011
 *
 *  Description:
 *
 *  Notes:
 *      $Id: AMCAWorkStatus.cs 209510 2011-12-12 09:36:53Z ACHIEVO\daly.zeng $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * 01-23-2008           Dave               Initial
 * </pre>
 */

#endregion Header

using System;
using System.Collections;
using System.Data;
using System.Text;
using System.Web;

using Accela.ACA.BLL.Common;
using Accela.ACA.BLL.WorkFlow;
using Accela.ACA.Common;
using Accela.ACA.Common.Common;
using Accela.ACA.Common.Util;
using Accela.ACA.Web.Common;
using Accela.ACA.WSProxy;
using Accela.ACA.WSProxy.WSModel;

namespace Accela.ACA.Web.Component
{
    /// <summary>
    /// This class provide the ablity to operation WorkStatus.
    /// </summary>
    public class AMCAWorkStatus
    {
        #region Fields

        private const string PROCESS_STATUS_FIRSTCELL = "38px";
        private const string PROCESS_STATUS_SECONDCELL = "95%";
        private const string PROCESS_STATUS_TASKNAME = "770px";
        private const string TAB_ROW_EVEN = "ACA_TabRow_Even ACA_TabRow_Even_FontSize";
        private const string TAB_ROW_ODD = "ACA_TabRow_Odd ACA_TabRow_Odd_FontSize";
        private const string TSI_CONTROL_TYPE_CHECKBOX = "9";
        private const string TSI_CONTROL_TYPE_DATE = "2";
        private const string TSI_CONTROL_TYPE_DROPDOWN = "5";
        private const string TSI_CONTROL_TYPE_RADIOBUTTON = "3";

        private DataTable _dtProcessingHistoryList;
        private DataTable _dtProcessingList;
        private Hashtable _htHistory = new Hashtable();
        private string _moduleName = String.Empty;
        private StringBuilder _tableContent = new StringBuilder();
        private string _trCss = TAB_ROW_EVEN;

        private string _commentURLLinks = string.Empty;
        private string _state = string.Empty;
        private string _pageTitle = string.Empty;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the WorkStatus class.
        /// </summary>
        /// <param name="moduleName">module name</param>
        public AMCAWorkStatus(string moduleName, string commentURLLinks, string state, string pageTitle)
        {
            _moduleName = moduleName;
            _commentURLLinks = commentURLLinks;
            _state = state;
            _pageTitle = pageTitle;
        }

        #endregion Constructors

        #region Methods

        /// <summary>
        /// Get processing content through module name.
        /// </summary>
        /// <returns>processing contenct as string</returns>
        public string GetProcessingContent()
        {
            CapModel4WS cap = AppSession.GetCapModelFromSession(_moduleName);
            ProcessingStatus ps = new ProcessingStatus(cap);
            _dtProcessingList = ps.CreateDataSource();

            string result = String.Empty;

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
        /// <param name="tsiModel"></param>
        /// <returns></returns>
        private static string GetChecklistCommentForDisplay(TaskSpecificInfoModel4WS tsiModel)
        {
            if (tsiModel == null)
            {
                return String.Empty;
            }

            string checklistComment = tsiModel.checklistComment;

            switch (tsiModel.checkboxInd)
            {
                case TSI_CONTROL_TYPE_CHECKBOX:
                    checklistComment = ModelUIFormat.FormatYNLabel(tsiModel.checklistComment);
                    break;
                case TSI_CONTROL_TYPE_RADIOBUTTON:
                    checklistComment = ModelUIFormat.FormatYNLabel(tsiModel.checklistComment);
                    break;
                case TSI_CONTROL_TYPE_DATE:
                    if (!string.IsNullOrEmpty(checklistComment))
                    {
                        DateTime dt = new DateTime();
                        bool isDate = DateTime.TryParse(checklistComment, out dt);
                        if (isDate)
                        {
                            checklistComment = I18nDateTimeUtil.ConvertDateStringFromWebServiceToUI(checklistComment);
                        }
                    }

                    break;
                case TSI_CONTROL_TYPE_DROPDOWN:
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
            this.AppendAdditionalInfoTable(drHistory, _tableContent);
        }

        /// <summary>
        /// Add Additional Information Table Html Script
        /// </summary>
        /// <param name="drHistory">DataRow of Task Specific Information History Table</param>
        /// <param name="tableContent">Html Text Writer</param>
        private void AppendAdditionalInfoTable(DataRow drHistory, StringBuilder tableContent)
        {

            TaskSpecificInfoModel4WS[] taskSpecItems = drHistory["AdditionalInfoItem"] as TaskSpecificInfoModel4WS[];
            if (taskSpecItems == null || taskSpecItems.Length == 0 ||
                IsDisplayAdditionalInfoTable(drHistory) == false)
            {
                tableContent.Append("<table id='2a' width='100%' border='0' cellpadding='0' cellspacing='0'>");
                tableContent.Append("<tr><td></td></tr></table>");
                return;
            }

            // DWB start the table here
            tableContent.Append("<table id='2b' width='100%' border='0' cellpadding='0' cellspacing='0'>");
            tableContent.Append(string.Format("<tr><td colspan=2 class='ACA_ALeft'><span>{0}</span></td></tr>", LabelUtil.GetTextByKey("ACA_WorkStatus_Label_AdditionalInfo", _moduleName)));

            tableContent.Append("<tr><td width='15px' align='right'></td><td width='97%' class='ACA_ALeft'>");

            if (taskSpecItems.Length > 0)
            {
                tableContent.Append("<table id='2c' width='100%' border='0' cellpadding='0' cellspacing='0' style='table-layout:fixed;'>");

                for (int i = 0; i < taskSpecItems.Length; i++)
                {
                    if (taskSpecItems[i] != null && taskSpecItems[i].checkboxDesc != null && taskSpecItems[i].checkboxDesc.Trim().Length > 0 && taskSpecItems[i].checklistComment != null && taskSpecItems[i].checklistComment.Trim().Length > 0 &&
                        taskSpecItems[i].vchDispFlag.Trim().ToUpper().Equals("Y"))
                    {
                        TaskSpecificInfoModel4WS tsiModel = taskSpecItems[i];
                        tableContent.Append("<tr>");
                        tableContent.Append("<td width='100%' class='ACA_ALeft' style='word-wrap:break-word;'><span>" + ScriptFilter.FilterScript(I18nStringUtil.GetString(tsiModel.resCheckboxDesc, tsiModel.checkboxDesc)) + "</span>: "); //</td>");
                        //tableContent.Append("<td width='*' class='ACA_ALeft' style='word-wrap:break-word;'><span>" + ScriptFilter.FilterScript(GetChecklistCommentForDisplay(tsiModel)) + "</span></td>");
                        tableContent.Append("<span>" + ScriptFilter.FilterScript(GetChecklistCommentForDisplay(tsiModel)) + "</span></td>");
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

            if (String.IsNullOrEmpty(action) || dtHistory == null ||
                dtHistory.Rows.Count == 0)
            {
                return false;
            }

            int count = 0;

            string commentID = string.Empty; // System.Guid.NewGuid().ToString();

            tableContent.Append("<table id='4' width='100%' border='0' cellpadding='0' cellspacing='0'>");

            DataRow[] historyDRs = dtHistory.Select("ParentID=" + dr["ID"]);     
            
            int index = 0;
            int historyRows = historyDRs.Length;
            foreach (DataRow historyDR in historyDRs)
            {
                index++;
                count = count + 1;

                bool isDisplayComment = false;

                string comment = historyDR["Comment"] == null ? String.Empty : historyDR["Comment"].ToString().Trim();
                // comment = "Okay, this is a test comment just to see how it is display in the list so I can convert the logic to diaplay it in AMCA and use the standard view.details.aspx page.";

                if (comment.Length > 90)
                {                   
                    string workComment = comment.Substring(0,90)
                        + "..." + "<a href='" + "View.Details.aspx?State=" + _state
                        + "&Type=Workflow"
                        + "&pageTitle=" + _pageTitle.ToString()
                        + "&commentText=" + comment.ToString()
                        + _commentURLLinks
                        + "'>" + "More</a>";
                    comment = workComment;
                }
 
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

                if (index == 1)
                {
                    tableContent.Append("<tr class='ACA_TabRow_Bold'>");
                }
                else
                {
                    tableContent.Append("<tr class='ACA_TabRow_Italic'>");
                }
                tableContent.Append("<td width='20px' class='ACA_ALeft'></td>");
                tableContent.Append("<td class='ACA_ALeft' width='*'>");
                tableContent.Append("<table id='4a' width='*' border='0' cellpadding='0' cellspacing='0'>");
                tableContent.Append("<tr>");
                tableContent.Append("<td class='ACA_ALeft' width='*'>" + FormatProcessFromDataRow2String(historyDR) + "</td>");
                tableContent.Append("</tr>");
                tableContent.Append("</table>");
                tableContent.Append("</td>");

                tableContent.Append("</tr>");

                if (isDisplayComment)
                {
                    string commentDisplay = LabelUtil.GetTextByKey("ACA_Common_Label_Comment", _moduleName);

                    tableContent.Append("<tr>"); //  id='" + commentID + "'>"); // style='display:none'>");
                    tableContent.Append("<td width='20px' class='ACA_ALeft'></td>");
                    tableContent.Append("<td width='*' class='ACA_ALeft'>");
                    tableContent.Append(commentDisplay + " ");
                    tableContent.Append(comment.ToString());
                    tableContent.Append("</td>");
                    tableContent.Append("</tr>");
                }

                // Don't need to add a line if there is only one status.
                if (historyDR != historyDRs[historyDRs.Length - 1] ||
                    IsDisplayAdditionalInfoTable(dr))
                {
                    tableContent.Append("<tr>");
                    //tableContent.Append("<td width='30px' class='ACA_ALeft'></td>");
                    tableContent.Append("<td class='ACA_ALeft' width='*' colspan='3'><hr></td>");
                    tableContent.Append("</tr>");
                }

                commentID = string.Empty; // System.Guid.NewGuid().ToString();
            }

            tableContent.Append("</table>");

            return count > 0;
        }

        private void AppendImgTDHtml(DataRow dr, string trID, bool isHasRecord)
        {
            AppendImgTDHtml(dr, trID, _tableContent, isHasRecord);
        }

        private void AppendImgTDHtml(DataRow dr, string trID, StringBuilder tableContent, bool isHasRecord)
        {
            //tableContent.Append("<td style='width:" + PROCESS_STATUS_FIRSTCELL + ";' class=\"ACA_ARight\">");
            tableContent.Append("<td class='ACA_ARight' width='20px' >");
            // tableContent.Append("<nobr>");

            //current status is complete, it will display icon by refenerce action status,
            if ((bool)dr["IsComplete"])
            {
                string actionStatus = dr["ActionStatusFlag"] == null ? String.Empty : dr["ActionStatusFlag"].ToString();

                if (WFActStatus.GoToBranchTask.Equals(actionStatus) || WFActStatus.GoToLoopTask.Equals(actionStatus))
                {
                    //if refenerce action status is "Go to Branch Task" or "Go to Loop Task", it will display active icon.
                    tableContent.Append(string.Format("<img class='workflowRowIcon' title='{0}' src='{1}' alt='{2}' />", LabelUtil.GetTextByKey("workstatus_icontitle_previously_active", _moduleName), ImageUtil.GetImageURL("asterisk_orange.png"), LabelUtil.GetTextByKey("img_alt_workflow_previously_active", _moduleName)));
                }
                else
                {
                    //if refenerce action status is "Go to Next Task " or empty, it will display complete icon.
                    tableContent.Append(string.Format("<img class='workflowRowIcon' title='{0}' src='{1}' alt='{2}' />", LabelUtil.GetTextByKey("workstatus_icontitle_complete", _moduleName), ImageUtil.GetImageURL("complete.png"), LabelUtil.GetTextByKey("img_alt_workflow_complete", _moduleName)));
                }

                //tableContent.Append("&nbsp;&nbsp;");
                tableContent.Append("&nbsp");
            }
            else if ((bool) dr["IsActive"])
            {
                string activeTitle = LabelUtil.GetTextByKey("workstatus_icontitle_active", _moduleName);
                activeTitle = string.Format("<img class='workflowRowIcon' title='{0}' src='{1}' alt='{2}' />", activeTitle, ImageUtil.GetImageURL("active.png"), activeTitle);
                tableContent.Append(activeTitle);
                //tableContent.Append("&nbsp;&nbsp;");
                tableContent.Append("&nbsp;");
            }       
  
            DataRow[] drs = _dtProcessingList.Select("ParentID=" + int.Parse(dr["ID"].ToString()));
            DataRow[] historyList = _dtProcessingHistoryList.Select("ParentID=" + int.Parse(dr["ID"].ToString()));
            //if (drs.Length > 0 || historyList.Length > 0)
            //{
                //string altText = ScriptFilter.AntiXssHtmlEncode(LabelUtil.RemoveHtmlFormat(LabelUtil.GetTextByKey("img_alt_expand_icon", _moduleName)));
                // tableContent.AppendFormat("<a id='lnk_{0}' href=\"javascript:void(0)\" title=\"{2}\" onclick='ControlDisplay($get(\"{0}\"), $get(\"img_{0}\"),false,$get(\"lnk_{0}\"),null)'><img id='img_{0}' src='{1}' alt=\"{2}\" class=\"ACA_NoBorder\" style='cursor: hand'></a>", trID, ImageUtil.GetImageURL("caret_collapsed.gif"), altText);
                //tableContent.Append("&nbsp;&nbsp;");
            //}       

            // tableContent.Append("2</nobr></td>");
            tableContent.Append("</td>");
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

            tableContent.Append("<table width='98%' border='0' cellpadding='0' cellspacing='0'>");
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

                        string trID = string.Empty; // System.Guid.NewGuid().ToString();
                        tableContent.Append("<table id='1a' width='100%' border='0' cellpadding='0' cellspacing='0'>");
                            tableContent.Append("<tr class='workflowTaskRow'>");
                            AppendImgTDHtml(dr, trID, tableContent, true);
                            //tableContent.Append("<td class='ACA_ALeft' width='" + PROCESS_STATUS_TASKNAME + "' >" + dr["TaskName"] + "</td>");
                            tableContent.Append("<td class='ACA_ALeft' >" + dr["TaskName"] + "</td>");
                            tableContent.Append("</tr>");

                            tableContent.Append("<tr id='" + trID + "'>"); // style='display:none'>");
                            // tableContent.Append("<td width='" + PROCESS_STATUS_FIRSTCELL + "' ></td>");
                            tableContent.Append("<td width='10px' class='ACA_ALeft'></td>");

                            //The row total width is 770px, left column width is 38px, so that right column is 732px;
                            tableContent.Append("<td class='ACA_ALeft'>");

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
                    tableContent.Append("<tr class='" + _trCss + "'>");
                    tableContent.Append("<td class='ACA_ALeft' valign='top' width='100%' colSpan='2'>");

                        tableContent.Append("<table id='1b' width='100%' border='0' cellpadding='0' cellspacing='0'>");
                            string trID = string.Empty; // System.Guid.NewGuid().ToString();
                            tableContent.Append("<tr class='workflowTaskRow'>");
                            AppendImgTDHtml(dr, trID, tableContent, false);
                            //tableContent.Append("<td class='ACA_ALeft' width='" + PROCESS_STATUS_TASKNAME + "' >" + dr["TaskName"] + "</td>");
                            tableContent.Append("<td class='ACA_ALeft'  colSpan='2'>" + dr["TaskName"] + "</td>");
                            tableContent.Append("</tr>");

                            tableContent.Append("<tr>"); // class='" + _trCss + "' id='" + trID + "'>"); // style='display:none'>");
                            // tableContent.Append("<td width='" + PROCESS_STATUS_FIRSTCELL + "' ></td>");
                            tableContent.Append("<td width='10px' class='ACA_ALeft'></td>");
                            //tableContent.Append("<td class='ACA_ALeft' width='" + PROCESS_STATUS_SECONDCELL + "' >");
                            tableContent.Append("<td class='ACA_ALeft' >");

                            if (AppendCommentTable(dr, tableContent, dtHistory))
                            {
                                AppendAdditionalInfoTable(dr, tableContent);
                            }

                            tableContent.Append("</td>");
                            tableContent.Append("</tr>");
                        tableContent.Append("</table>");
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
            _tableContent.Append("<table id='3' width='100%' border='0' cellpadding='0' cellspacing='0'>");
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
                    _tableContent.Append("<td width='20px' class='ACA_ALeft' ></td>");

                    _tableContent.Append("<td class='ACA_ALeft' valign='top' width='*' colSpan='2'>");
                        string trID = string.Empty; // Guid.NewGuid().ToString();
                        _tableContent.Append("<table id='3a'  width='100%' border='0' cellpadding='0' cellspacing='0'>");
                            _tableContent.Append("<tr class='workflowTaskRow'>");
                            AppendImgTDHtml(dr, trID, true);
                            //_tableContent.Append("<td class='ACA_ALeft' width='" + PROCESS_STATUS_TASKNAME + "' >" + dr["TaskName"] + "</td>");
                            _tableContent.Append("<td class='ACA_ALeft'>" + dr["TaskName"] + "</td>");
                            _tableContent.Append("</tr>");
                            _tableContent.Append("<tr id='" + trID + "'>"); // style='display:none'>");
                            //_tableContent.Append("<td width='" + PROCESS_STATUS_FIRSTCELL + "' ></td>");
                            //_tableContent.Append("<td width='" + PROCESS_STATUS_SECONDCELL + "' >");
                            _tableContent.Append("<td width='10px' class='ACA_ALeft' ></td>");
                            _tableContent.Append("<td>");

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
                    _tableContent.Append("<tr>");
                    _tableContent.Append("<td width='20px' class='ACA_ALeft' ></td>");
                    _tableContent.Append("<td class='ACA_ALeft' valign='top' width='*' colSpan='2'>");
                    string trID = string.Empty; // Guid.NewGuid().ToString();
                        _tableContent.Append("<table id='3b'  width='100%' border='0' cellpadding='0' cellspacing='0'>");
                            _tableContent.Append("<tr class='workflowTaskRow'>");
                            AppendImgTDHtml(dr, trID, true);
                            //_tableContent.Append("<td class='ACA_ALeft' width='" + PROCESS_STATUS_TASKNAME + "' >" + dr["TaskName"] + "</td>");
                            _tableContent.Append("<td class='ACA_ALeft'>" + dr["TaskName"] + "</td>");
                            _tableContent.Append("</tr>");

                            _tableContent.Append("<tr id='" + trID + "'>"); // style='display:none'>");
                            //_tableContent.Append("<td width='" + PROCESS_STATUS_FIRSTCELL + "' ></td>");
                            _tableContent.Append("<td width='10px' class='ACA_ALeft'></td>");
                            //_tableContent.Append("<td class='ACA_ALeft' width='" + PROCESS_STATUS_SECONDCELL + "' >");
                            _tableContent.Append("<td class='ACA_ALeft'>");

                            if (AppendCommentTable(dr))
                            {
                                AppendAdditionalInfoTable(dr);
                            }

                            _tableContent.Append("</td>");
                            _tableContent.Append("</tr>");
                        _tableContent.Append("</table>");
                   _tableContent.Append("</td></tr>");
                }
            }

            _tableContent.Append("</table>");
        }

        /// <summary>
        /// Copies drs into a dt, preserving any
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
        /// Check if display the Addtional Information Table
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
        /// <returns>True if mask Display E-mail in V360 to display e-mail information in Capdetail; Otherwise false.</returns>
        private bool IsDisplayEmailinTask(string optionTask)
        {
            IXPolicyBll policyBll = (IXPolicyBll) ObjectFactory.GetObject(typeof(IXPolicyBll));
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
            string processPattern = LabelUtil.GetTextByKey("acaadmin_workflow_msg_configuration", _moduleName);

            string dDate = dr["DueDate"] as string;

            // If date is not set, TBD instead.
            dDate = string.IsNullOrEmpty(dDate) ? LabelUtil.GetTextByKey("acaadmin_workflow_msg_nulldate", _moduleName) : I18nDateTimeUtil.ConvertDateStringFromWebServiceToUI(dDate);
            processPattern = processPattern.Replace(WorkStatusVariables.DueDate, dDate);

            string assignEmail = dr["AssignEmail"] as string;
            string assignedTo = dr["UserName"] as string;
            if (processPattern.Contains(WorkStatusVariables.AssignedTo))
            {
                if (string.IsNullOrEmpty(assignedTo))
                {
                    // If assign-to is not assigned, replace TBD instead.
                    processPattern = processPattern.Replace(WorkStatusVariables.AssignedTo, LabelUtil.GetTextByKey("acaadmin_workflow_msg_nullstaff", _moduleName));
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
            processPattern = processPattern.Replace(WorkStatusVariables.TaskStatus, processStatus);

            string statusDate = dr["ActionTime"] as string;
            statusDate = string.IsNullOrEmpty(statusDate) ? LabelUtil.GetTextByKey("acaadmin_workflow_msg_nulldate", _moduleName) : I18nDateTimeUtil.ConvertDateStringFromWebServiceToUI(statusDate);
            processPattern = processPattern.Replace(WorkStatusVariables.StatusDate, statusDate);

            string actionBy = dr["ActionBy"] as string;
            string actionEmail = dr["ActionEmail"] as string;
            if (processPattern.Contains(WorkStatusVariables.ActionBy))
            {
                if (string.IsNullOrEmpty(actionBy))
                {
                    processPattern = processPattern.Replace(WorkStatusVariables.ActionBy, LabelUtil.GetTextByKey("acaadmin_workflow_msg_nullstaff", _moduleName));
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
