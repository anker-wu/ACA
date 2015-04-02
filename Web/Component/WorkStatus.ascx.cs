#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: WorkStatus.ascx.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2008-2014
 *
 *  Description:
 *
 *  Notes:
 *      $Id: WorkStatus.ascx.cs 278771 2014-09-13 03:41:14Z ACHIEVO\james.shi $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * 01-23-2008           Daly zeng               Initial
 * </pre>
 */

#endregion Header

using System;
using System.Collections;
using System.Data;
using System.Text;
using Accela.ACA.BLL.Common;
using Accela.ACA.BLL.WorkFlow;
using Accela.ACA.Common;
using Accela.ACA.Common.Common;
using Accela.ACA.Common.Util;
using Accela.ACA.Web.Common;
using Accela.ACA.WSProxy;

namespace Accela.ACA.Web.Component
{
    /// <summary>
    /// This class provide the ability to operation WorkStatus.
    /// </summary>
    public partial class WorkStatus : PermitDetailBaseUserControl
    {
        #region Fields

        /// <summary>
        /// Table row even.
        /// </summary>
        private const string TAB_ROW_EVEN = "ACA_TabRow_Even ACA_TabRow_Even_FontSize";

        /// <summary>
        /// Table row odd.
        /// </summary>
        private const string TAB_ROW_ODD = "ACA_TabRow_Odd ACA_TabRow_Odd_FontSize";

        /// <summary>
        /// Compete image html.
        /// </summary>
        private string _competeImgHtml;

        /// <summary>
        /// Active image html.
        /// </summary>
        private string _activeImgHtml;

        /// <summary>
        /// Data table ADHOC.
        /// </summary>
        private DataTable _dtAdHoc;

        /// <summary>
        /// Data table for ADHOC history.
        /// </summary>
        private DataTable _dtAdHocHistory;

        /// <summary>
        /// Data table processing history list..
        /// </summary>
        private DataTable _dtProcessingHistoryList;

        /// <summary>
        /// Data table processing list..
        /// </summary>
        private DataTable _dtProcessingList;

        /// <summary>
        /// ADHOC history list.
        /// </summary>
        private TaskItemModel4WS[] _hocHistoryList;

        /// <summary>
        /// ADHOC list.
        /// </summary>
        private TaskItemModel4WS[] _hocList;

        /// <summary>
        /// Hash table for history.
        /// </summary>
        private Hashtable _htIsHistory = new Hashtable();

        /// <summary>
        /// Table ADHOC.
        /// </summary>
        private StringBuilder _tableAdHoc = new StringBuilder();

        /// <summary>
        /// table content.
        /// </summary>
        private StringBuilder _tableContent = new StringBuilder();

        /// <summary>
        /// Table row even.
        /// </summary>
        private string _trCss = TAB_ROW_EVEN;

        #endregion Fields

        #region Methods

        /// <summary>
        /// Get AdHoc content through module name
        /// </summary>
        /// <param name="moduleName">module name</param>
        /// <returns>AdHoc content as string</returns>
        public string GetAdHocContent(string moduleName)
        {
            CapModel4WS cap = AppSession.GetCapModelFromSession(moduleName);
            _dtAdHoc = CreateAdHocDataSource(cap);
            BuildProcessingRootNode(true, _tableAdHoc, _dtAdHoc, _dtAdHocHistory, moduleName);
            return _tableAdHoc.ToString();
        }

        /// <summary>
        /// Get processing content through module name.
        /// </summary>
        /// <param name="moduleName">Module name</param>
        /// <returns>Processing content as string</returns>
        public string GetProcessingContent(string moduleName)
        {
            //string moduleName = System.Web.HttpContext.Current.Request.QueryString[ACAConstant.MODULE_NAME];
            CapModel4WS cap = AppSession.GetCapModelFromSession(moduleName);
            ProcessingStatus ps = new ProcessingStatus(cap);
            _dtProcessingList = ps.CreateDataSource();

            if (_dtProcessingList.Rows.Count > 0)
            {
                _dtProcessingHistoryList = _dtProcessingList.Clone();
                ps.SetProcessingHistoryList(_dtProcessingList, ref _dtProcessingHistoryList);
                BuildProcessingRootNode(false, _tableContent, _dtProcessingList, _dtProcessingHistoryList, moduleName);
                return _tableContent.ToString();
            }

            return string.Empty;
        }

        /// <summary>
        /// Raises the page load event
        /// </summary>
        /// <param name="sender">An object that contains the event sender.</param>
        /// <param name="e">A System.EventArgs object containing the event data.</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (AppSession.IsAdmin)
            {
                divProcessInstruction.Visible = true;
            }

            if (!Page.IsPostBack)
            {
                instructionText.InnerHtml = GetTextByKey("acaadmin_workflow_msg_processinstruction");
                string activeTitle = GetTextByKey("workstatus_icontitle_active");
                string completeTitle = GetTextByKey("workstatus_icontitle_complete");
                _activeImgHtml = string.Format("<img title='" + activeTitle + "' src='{0}' alt='{1}'/>", ImageUtil.GetImageURL("active.png"), GetTextByKey("img_alt_workflow_active"));
                _competeImgHtml = string.Format("<img title='" + completeTitle + "' src='{0}' alt='{1}'/>", ImageUtil.GetImageURL("complete.png"), GetTextByKey("img_alt_workflow_complete"));
            }
        }

        /// <summary>
        /// Add Additional Information Table Html Script
        /// </summary>
        /// <param name="drHistory">DataRow of Task Specific Information History Table</param>
        /// <param name="moduleName">Module name.</param>
        private void AppendAdditionalInfoTable(DataRow drHistory, string moduleName)
        {
            this.AppendAdditionalInfoTable(drHistory, _tableContent, moduleName);
        }

        /// <summary>
        /// Add Additional Information Table Html Script
        /// </summary>
        /// <param name="drHistory">DataRow of Task Specific Information History Table</param>
        /// <param name="_text">Html Text Writer</param>
        /// <param name="moduleName">Module name</param>
        private void AppendAdditionalInfoTable(DataRow drHistory, StringBuilder _text, string moduleName)
        {
            _text.Append("<table role='presentation' width='100%' border='1' cellpadding='0' cellspacing='0'>");

            TaskSpecificInfoModel4WS[] taskSpecItems = drHistory["AdditionalInfoItem"] as TaskSpecificInfoModel4WS[];
            if (taskSpecItems == null || taskSpecItems.Length == 0 || IsDisplayAdditionalInfoTable(drHistory) == false)
            {
                _text.Append("<tr></tr></table>");
                return;
            }
            else
            {
                _text.Append(string.Format("<tr><td colspan=2 class='ACA_ALeft'><div class='Header_h4'><span style='font-weight:100'>{0}</span>&nbsp;&nbsp;</div></td></tr>", LabelUtil.GetTextByKey("ACA_WorkStatus_Label_AdditionalInfo", moduleName)));
            }

            _text.Append("<tr><td width='3%' align='right'></td><td width='97%' class='ACA_ALeft'>");

            if (taskSpecItems != null || taskSpecItems.Length > 0)
            {
                _text.Append("<table role='presentation' width='100%' border='1' cellpadding='0' cellspacing='0' style='table-layout:fixed;'>");

                for (int i = 0; i < taskSpecItems.Length; i++)
                {
                    if (taskSpecItems[i].fieldLabel != null && taskSpecItems[i].fieldLabel.Trim().Length > 0 && taskSpecItems[i].checklistComment != null && taskSpecItems[i].checklistComment.Trim().Length > 0
                        && taskSpecItems[i].vchDispFlag.Trim().ToUpper().Equals("Y"))
                    {
                        _text.Append("<tr>");
                        _text.Append("<td width='25%' class='ACA_ALeft' style='word-wrap:break-word;'><div class='Header_h4'><span style='font-weight:100'>" + ScriptFilter.FilterScript(taskSpecItems[i].fieldLabel) + "</span></div></td>");
                        _text.Append("<td width='*' class='ACA_ALeft' style='word-wrap:break-word;'><div class='Header_h4'><span style='font-weight:100'>" + ScriptFilter.FilterScript(taskSpecItems[i].checklistComment) + "</span></div></td>");
                        _text.Append("</tr>");
                    }
                }

                _text.Append("</table>");
            }

            _text.Append("</td></tr></table>");
        }

        /// <summary>
        /// Add Task Status and Comment Html Script
        /// </summary>
        /// <param name="dr">DataRow of Task Status and Comment</param>
        /// <param name="moduleName">Module name.</param>
        /// <returns>True: Added Task Status Record; False: Not Added Task Status Record</returns>
        private bool AppendCommentTable(DataRow dr, string moduleName)
        {
            return AppendCommentTable(dr, _tableContent, _dtProcessingHistoryList, false, moduleName);
        }

        /// <summary>
        /// Add Task Status and Comment Html Script
        /// </summary>
        /// <param name="dr">DataRow of Task Status and Comment</param>
        /// <param name="_text">Html Text Writer</param>
        /// <param name="_dtHistory">History Data Table</param>
        /// <param name="isAcHoc">Is ADHoc flag.</param>
        /// <param name="moduleName">Module name.</param>
        /// <returns>True: Added Task Status Record; False: Not Added Task Status Record</returns>
        private bool AppendCommentTable(DataRow dr, StringBuilder _text, DataTable _dtHistory, bool isAcHoc, string moduleName)
        {
            int count = 0;

            if (!string.IsNullOrEmpty(dr["Action"].ToString()))
            {
                string commentID = CommonUtil.GetRandomUniqueID();

                _text.Append("<table role='presentation' width='100%' border='1' cellpadding='0' cellspacing='0'>");

                if (_dtHistory != null)
                {
                    DataRow[] historyDRs;

                    if (isAcHoc == false)
                    {
                        historyDRs = _dtHistory.Select("ParentID=" + dr["ID"]);
                    }
                    else
                    {
                        historyDRs = _dtAdHocHistory.Select("StepNumber=" + dr["StepNumber"]);
                    }

                    int index = 0;
                    foreach (DataRow historyDR in historyDRs)
                    {
                        index++;
                        if (index == 1)
                        {
                            _text.Append("<tr class='ACA_TabRow_Bold'>");
                        }
                        else
                        {
                            _text.Append("<tr class='ACA_TabRow_Italic'>");
                        }

                        bool isDisplayComment = false;

                        string comment = historyDR["Comment"] == null ? string.Empty : historyDR["Comment"].ToString().Trim();

                        // If 'comment' isn't null and isn't ACHocworkflow. should display by owned privileges.
                        // If only set contact permission,we must be consider the permission set in capcontact model in spear form.
                        if (!string.IsNullOrEmpty(comment))
                        {
                            // If workflow
                            if (!isAcHoc)
                            {
                                if (ValidationUtil.IsYes(historyDR["IsRestrictView"].ToString()))
                                {
                                    UserRolePrivilegeModel role = historyDR["UserRolePrivilege"] as UserRolePrivilegeModel;
                                    var proxyUserRoleBll = ObjectFactory.GetObject<IProxyUserRoleBll>();
                                    isDisplayComment = proxyUserRoleBll.HasReadOnlyPermission(AppSession.GetCapModelFromSession(moduleName), role);
                                }
                            }
                            else
                            {
                                isDisplayComment = true;
                            }
                        }

                        if (isDisplayComment)
                        {
                            _text.AppendFormat(
                                            "<td width='3%' class='ACA_ALeft'><a href='javascript:void(0)' onclick='ControlDisplay($get(\"" + commentID + "\"),this,true)' class=\"NotShowLoading\"><img src='{0}' alt='{1}'  class='ACA_NoBorder'></a></td>",
                                            ImageUtil.GetImageURL("plus_expand.gif"), 
                                            GetTextByKey("img_alt_expand_icon"));
                        }
                        else
                        {
                            _text.Append("<td width='3%' class='ACA_ALeft'></td>");
                        }

                        count = count + 1;
                        _text.Append("<td class='ACA_ALeft' width='715px;'><div class='Header_h4'>");
                        _text.Append("<table role='presentation' width='100%' border='0' cellpadding='0' cellspacing='0'>");
                        _text.Append("<tr>");
                        _text.Append("<td class='ACA_ALeft' width='40%'>" + historyDR["Action"] + "</td>");
                        _text.Append("<td class='ACA_ALeft' width='20%'>" + historyDR["ActionTime"] + "</td>");
                        _text.Append("<td class='ACA_ALeft' width='40%'>" + historyDR["UserName"] + "</td>");
                        _text.Append("</tr>");
                        _text.Append("</table>");
                        _text.Append("</div></td>");
                        _text.Append("</tr>");

                        if (isDisplayComment)
                        {
                            _text.Append("<tr id='" + commentID + "' style='display:none'>");
                            _text.Append("<td width='3%' align='right'></td>");
                            _text.Append("<td width='97%' class='ACA_ALeft'><div class='Header_h4'>");
                            _text.Append("  <table role='presentation' border='0' cellpadding='0' cellspacing='0'>");
                            _text.Append("  <tr>");
                            _text.Append("  <td><span style='font-size:1.2em;color: #555555' class='ACA_Comments'>" + LabelUtil.GetTextByKey("ACA_Common_Label_Comment", moduleName) + "</span></td>");
                            _text.Append("  <td>&nbsp;&nbsp;</td>");
                            _text.Append("  <td><span style='font-weight:100'>" + ScriptFilter.FilterScript(historyDR["Comment"].ToString()) + "</span></td>");
                            _text.Append("  </tr>");
                            _text.Append("  </table>");
                            _text.Append("</div></td>");
                            _text.Append("</tr>");
                        }

                        // Don't need to add a line if there is only one status.
                        if (historyDR != historyDRs[historyDRs.Length - 1] || IsDisplayAdditionalInfoTable(dr))
                        {
                            _text.Append("<tr>");
                            _text.Append("<td class='ACA_ALeft' width='100%' colspan='3'><hr></td>");
                            _text.Append("</tr>");
                        }

                        commentID = CommonUtil.GetRandomUniqueID();
                    }
                }

                _text.Append("</table>");
            }

            return count > 0;
        }

        /// <summary>
        /// AppendImage Split Line
        /// </summary>
        /// <param name="dr">The data row .</param>
        /// <param name="drs">DataRow array</param>
        private void AppendImgSplitLine(DataRow dr, DataRow[] drs)
        {
        }

        /// <summary>
        /// Append Image table Html
        /// </summary>
        /// <param name="dr">The data row.</param>
        /// <param name="trID">Table row id.</param>
        private void AppendImgTDHtml(DataRow dr, string trID)
        {
            AppendImgTDHtml(false, dr, trID, _tableContent);
        }

        /// <summary>
        /// Append Image table Html
        /// </summary>
        /// <param name="isAcHoc">Is ADBOC flag.</param>
        /// <param name="dr">The data row.</param>
        /// <param name="trID">Table row id.</param>
        /// <param name="_text">String builder</param>
        private void AppendImgTDHtml(bool isAcHoc, DataRow dr, string trID, StringBuilder _text)
        {
            _text.Append("<td class='ACA_ARight' width='5%'>");
            if ((bool)dr["IsComplete"])
            {
                _text.Append(_competeImgHtml);
                _text.Append("&nbsp;&nbsp;");
            }

            if ((bool)dr["IsActive"])
            {
                _text.Append(_activeImgHtml);
                _text.Append("&nbsp;&nbsp;");
            }

            if (isAcHoc)
            {
                if (_htIsHistory.ContainsKey(dr["StepNumber"].ToString()) == false)
                {
                    _htIsHistory.Add(dr["StepNumber"].ToString(), null);
                    bool chk = false;

                    if (_hocHistoryList != null)
                    {
                        foreach (TaskItemModel4WS tiws in _hocHistoryList)
                        {
                            if (dr["StepNumber"].ToString() == tiws.stepNumber.ToString())
                            {
                                chk = true;
                                break;
                            }
                        }

                        if (chk)
                        {
                            _text.AppendFormat("<a href='javascript:void(0);' onclick='ControlDisplay($get(\"" + trID + "\"),this,false)' class=\"NotShowLoading\"><img src='{0}' alt='{1}'  class='ACA_NoBorder'></a>", ImageUtil.GetImageURL("caret_collapsed.gif"), GetTextByKey("img_alt_expand_icon"));
                        }
                    }
                }
            }
            else
            {
                DataRow[] drs = _dtProcessingList.Select("ParentID=" + int.Parse(dr["ID"].ToString()));
                DataRow[] historyList = _dtProcessingHistoryList.Select("ParentID=" + int.Parse(dr["ID"].ToString()));

                if ((drs.Length > 0 || bool.Parse(dr["IsComplete"].ToString())) || (!string.IsNullOrEmpty(dr["Action"].ToString()) && historyList.Length > 0))
                {
                    _text.AppendFormat("<a href='javascript:void(0);'  onclick='ControlDisplay($get(\"" + trID + "\"),this,false)' class=\"NotShowLoading\"><img src='{0}' alt='{1}' class='ACA_NoBorder'></a>", ImageUtil.GetImageURL("caret_collapsed.gif"), GetTextByKey("img_alt_expand_icon"));
                }
            }

            _text.Append("</td>");
        }

        /// <summary>
        /// Build top layer data node and call sub processing function to build all data list
        /// </summary>
        /// <param name="isAdHoc">Is ADHOC flag.</param>
        /// <param name="_text">String builder.</param>
        /// <param name="_dt">Data table.</param>
        /// <param name="_dtHistory">Data table history.</param>
        /// <param name="moduleName">Module name.</param>
        private void BuildProcessingRootNode(bool isAdHoc, StringBuilder _text, DataTable _dt, DataTable _dtHistory, string moduleName)
        {
            if (_dt.Rows.Count > 0)
            {
                _text.Append("<table role='presentation' width='736px border='1' cellpadding='0' cellspacing='0'>");
                DataTable dt = _dt.Clone();
                DataRow[] drs;

                if (isAdHoc == false)
                {
                    drs = _dt.Select("ParentID=0");
                }
                else
                {
                    drs = _dt.Select();
                }

                foreach (DataRow dr in drs)
                {
                    _trCss = _trCss == TAB_ROW_EVEN ? TAB_ROW_ODD : TAB_ROW_EVEN;

                    if (isAdHoc == false)
                    {
                        DataRow[] drss = _dt.Select("ParentID=" + dr["ID"]);
                        FillRowInDataTable(drss, ref dt);
                    }

                    if (dt.Rows.Count > 0)
                    {
                        _text.Append("<tr class='" + _trCss + "'>");
                        _text.Append("<td class='ACA_ALeft' valign='top' width='100%' colSpan='2'>");

                        string trID = CommonUtil.GetRandomUniqueID();
                        _text.Append("<table role='presentation' width='100%' border='1' cellpadding='0' cellspacing='0'>");
                        _text.Append("<tr>");
                        AppendImgTDHtml(isAdHoc, dr, trID, _text);
                        _text.Append("<td class='ACA_ALeft' width='95%'><div class='Header_h4'>" + dr["TaskName"] + "</div></td>");
                        _text.Append("</tr>");

                        _text.Append("<tr id='" + trID + "' style='display:none'>");
                        _text.Append("<td width='5%'></td>");
                        _text.Append("<td width='95%'>");

                        if (AppendCommentTable(dr, _text, _dtHistory, isAdHoc, moduleName))
                        {
                            AppendAdditionalInfoTable(dr, _text, moduleName);
                        }

                        if (isAdHoc == false)
                        {
                            AppendImgSplitLine(dr, drs);
                            BuildProcessingSubNode(dt, moduleName);
                        }

                        _text.Append("</td></tr>");

                        _text.Append("</table>");

                        _text.Append("</td></tr>");
                        dt.Clear();
                    }
                    else
                    {
                        string trID = CommonUtil.GetRandomUniqueID();
                        _text.Append("<tr class='" + _trCss + "'>");
                        AppendImgTDHtml(isAdHoc, dr, trID, _text);
                        _text.Append("<td class='ACA_ALeft' width='95%'><div class='Header_h4'>" + dr["TaskName"] + "</div></td>");
                        _text.Append("</tr>");

                        _text.Append("<tr class='ACA_TabRow_Odd ACA_TabRow_Odd_FontSize' id='" + trID + "' style='display:none'>");
                        _text.Append("<td width='5%'></td>");
                        _text.Append("<td class='ACA_ALeft' width='95%'>");

                        if (AppendCommentTable(dr, _text, _dtHistory, isAdHoc, moduleName))
                        {
                            AppendAdditionalInfoTable(dr, _text, moduleName);
                        }

                        _text.Append("</td>");
                        _text.Append("</tr>");
                    }
                }

                _text.Append("</table>");
            }
        }

        /// <summary>
        /// Build sub data node and leaf node.
        /// </summary>
        /// <param name="processingList">Special layer data node</param>
        /// <param name="moduleName">Module name.</param>
        private void BuildProcessingSubNode(DataTable processingList, string moduleName)
        {
            _tableContent.Append("<table role='presentation' width='100%' border='1' cellpadding='0' cellspacing='0'>");
            DataTable dt = processingList.Clone();
            dt.Clear();
            foreach (DataRow dr in processingList.Rows)
            {
                string filterExpression = "ParentID=" + dr["ID"];
                DataRow[] drs = _dtProcessingList.Select(filterExpression);
                FillRowInDataTable(drs, ref dt);

                if (dt.Rows.Count > 0)
                {
                    _tableContent.Append("<tr>");
                    _tableContent.Append("<td class='ACA_ALeft' valign='top' width='100%' colSpan='2'>");
                    string trID = CommonUtil.GetRandomUniqueID();
                    _tableContent.Append("<table role='presentation' width='100%' border='1' cellpadding='0' cellspacing='0'>");
                    _tableContent.Append("<tr>");
                    AppendImgTDHtml(dr, trID);
                    _tableContent.Append("<td class='ACA_ALeft' width='95%'><div class='Header_h4'>" + dr["TaskName"] + "</div></td>");
                    _tableContent.Append("</tr>");
                    _tableContent.Append("<tr id='" + trID + "' style='display:none'>");
                    _tableContent.Append("<td width='5%'></td>");
                    _tableContent.Append("<td width='95%'>");

                    if (AppendCommentTable(dr, moduleName))
                    {
                        AppendAdditionalInfoTable(dr, moduleName);
                    }

                    AppendImgSplitLine(dr, drs);
                    BuildProcessingSubNode(dt, moduleName);
                    _tableContent.Append("</td></tr>");
                    _tableContent.Append("</table>");
                    _tableContent.Append("</td></tr>");
                }
                else
                {
                    string trID = CommonUtil.GetRandomUniqueID();
                    _tableContent.Append("<tr>");
                    AppendImgTDHtml(dr, trID);
                    _tableContent.Append("<td class='ACA_ALeft' width='95%'><div class='Header_h4'>" + dr["TaskName"] + "</div></td>");
                    _tableContent.Append("</tr>");

                    _tableContent.Append("<tr id='" + trID + "' style='display:none'>");
                    _tableContent.Append("<td width='5%'></td>");
                    _tableContent.Append("<td class='ACA_ALeft' width='95%'>");

                    if (AppendCommentTable(dr, moduleName))
                    {
                        AppendAdditionalInfoTable(dr, moduleName);
                    }

                    _tableContent.Append("</td>");
                    _tableContent.Append("</tr>");
                }
            }

            _tableContent.Append("</table>");
        }

        /// <summary>
        /// Change AdHoc Data Format
        /// </summary>
        /// <param name="toDataFormat">Data Format.</param>
        /// <param name="hoc">TaskItemModel4WS array</param>
        private void ChangeAdHocDataFormat(ref DataTable toDataFormat, TaskItemModel4WS[] hoc)
        {
            foreach (TaskItemModel4WS ttws in hoc)
            {
                DataRow dr = toDataFormat.NewRow();

                dr["TaskName"] = ttws.adHocName;
                dr["UserName"] = ttws.sysUser.fullName;
                string date = string.Empty;

                if (ttws.assignmentDate != null)
                {
                    date = ttws.assignmentDate.Split(' ')[0];
                }

                dr["ActionTime"] = date;
                dr["Action"] = I18nStringUtil.GetString(ttws.resDisposition, ttws.disposition);
                dr["IsComplete"] = ttws.completeFlag == "Y" ? true : false;
                dr["IsActive"] = ttws.activeFlag == "Y" ? true : false;
                dr["Comment"] = I18nStringUtil.GetString(ttws.resDispositionComment, ttws.dispositionComment);
                dr["StepNumber"] = ttws.stepNumber.ToString();
                dr["AdHocName"] = ttws.adHocName;
                dr["AdditionalInfoItem"] = ttws.taskSpecItems;
                toDataFormat.Rows.Add(dr);
            }
        }

        /// <summary>
        /// Create ADHOC DataSource
        /// </summary>
        /// <param name="capModel">A cap model.</param>
        /// <returns>DataTable for ADHOC</returns>
        private DataTable CreateAdHocDataSource(CapModel4WS capModel)
        {
            IWorkflowBll workflowBll = (IWorkflowBll)ObjectFactory.GetObject(typeof(IWorkflowBll));
            _hocHistoryList = workflowBll.GetADHocTaskItemByCapID(capModel.capID, null, null);
            _hocList = workflowBll.GetADHocTaskAuditByCapID(capModel.capID, null, null);

            DataTable dt = new DataTable();
            dt = CreateDataTableStructrue();
            _dtAdHocHistory = new DataTable();
            _dtAdHocHistory = CreateDataTableStructrue();

            if (_hocList != null)
            {
                //divAdHoc.Visible = true;
                ChangeAdHocDataFormat(ref dt, _hocList);
            }

            if (_hocHistoryList != null)
            {
                ChangeAdHocDataFormat(ref _dtAdHocHistory, _hocHistoryList);
            }

            return dt;
        }

        /// <summary>
        /// Create DataTable for Structure
        /// </summary>
        /// <returns>DataTable for structure.</returns>
        private DataTable CreateDataTableStructrue()
        {
            DataTable table = new DataTable("HocTable");
            DataColumn column;

            column = new DataColumn();
            column.DataType = System.Type.GetType("System.Int32");
            column.ColumnName = "ID";
            column.AutoIncrement = true;
            column.Caption = "ID";
            column.ReadOnly = true;
            column.Unique = true;
            column.AutoIncrementSeed = 1;
            column.AutoIncrementStep = 1;
            table.Columns.Add(column);

            column = new DataColumn();
            column.DataType = System.Type.GetType("System.String");
            column.ColumnName = "TaskName";
            column.AutoIncrement = false;
            column.Caption = "TaskName";
            column.ReadOnly = false;
            column.Unique = false;
            table.Columns.Add(column);

            column = new DataColumn();
            column.DataType = System.Type.GetType("System.String");
            column.ColumnName = "UserName";
            column.AutoIncrement = false;
            column.Caption = "UserName";
            column.ReadOnly = false;
            column.Unique = false;
            table.Columns.Add(column);

            column = new DataColumn();
            column.DataType = System.Type.GetType("System.String");
            column.ColumnName = "ActionTime";
            column.AutoIncrement = false;
            column.Caption = "ActionTime";
            column.ReadOnly = false;
            column.Unique = false;
            table.Columns.Add(column);

            column = new DataColumn();
            column.DataType = System.Type.GetType("System.String");
            column.ColumnName = "Action";
            column.AutoIncrement = false;
            column.Caption = "Action";
            column.ReadOnly = false;
            column.Unique = false;
            table.Columns.Add(column);

            column = new DataColumn();
            column.DataType = System.Type.GetType("System.Boolean");
            column.ColumnName = "IsComplete";
            column.AutoIncrement = false;
            column.Caption = "IsComplete";
            column.ReadOnly = false;
            column.Unique = false;
            table.Columns.Add(column);

            column = new DataColumn();
            column.DataType = System.Type.GetType("System.Boolean");
            column.ColumnName = "IsActive";
            column.AutoIncrement = false;
            column.Caption = "IsActive";
            column.ReadOnly = false;
            column.Unique = false;
            table.Columns.Add(column);

            column = new DataColumn();
            column.DataType = System.Type.GetType("System.String");
            column.ColumnName = "Comment";
            column.AutoIncrement = false;
            column.Caption = "Comment";
            column.ReadOnly = false;
            column.Unique = false;
            table.Columns.Add(column);

            column = new DataColumn();
            column.DataType = System.Type.GetType("System.String");
            column.ColumnName = "StepNumber";
            column.AutoIncrement = false;
            column.Caption = "StepNumber";
            column.ReadOnly = false;
            column.Unique = false;
            table.Columns.Add(column);

            column = new DataColumn();
            column.DataType = System.Type.GetType("System.String");
            column.ColumnName = "AdHocName";
            column.AutoIncrement = false;
            column.Caption = "AdHocName";
            column.ReadOnly = false;
            column.Unique = false;
            table.Columns.Add(column);

            column = new DataColumn();
            column.DataType = System.Type.GetType("System.Object");
            column.ColumnName = "AdditionalInfoItem";
            column.AutoIncrement = false;
            column.Caption = "AdditionalInfoItem";
            column.ReadOnly = false;
            column.Unique = false;
            table.Columns.Add(column);

            return table;
        }

        /// <summary>
        /// Fill Row in DataTable
        /// </summary>
        /// <param name="drs">Data table array.</param>
        /// <param name="dt">Data table.</param>
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

            if (taskSpecItems == null || taskSpecItems.Length == 0)
            {
                return false;
            }

            bool isDisplayAdditionInfoTable = false;

            for (int i = 0; i < taskSpecItems.Length; i++)
            {
                if (taskSpecItems[i].fieldLabel != null && taskSpecItems[i].fieldLabel.Trim().Length > 0 && taskSpecItems[i].checklistComment != null && taskSpecItems[i].checklistComment.Trim().Length > 0
                    && taskSpecItems[i].vchDispFlag.Trim().ToUpper().Equals("Y"))
                {
                    isDisplayAdditionInfoTable = true;
                    break;
                }
            }

            return isDisplayAdditionInfoTable;
        }

        #endregion Methods
    }
}
