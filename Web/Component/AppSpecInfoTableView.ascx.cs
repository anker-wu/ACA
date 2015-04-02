#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: CapDetail.aspx.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2008-2014
 *
 *  Description:
 *
 *  Notes:
 *      $Id: AppSpecInfoTableView.ascx.cs 278418 2014-09-03 08:54:25Z ACHIEVO\james.shi $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;

using Accela.ACA.Common;
using Accela.ACA.Common.Common;
using Accela.ACA.Common.Util;
using Accela.ACA.UI.Model;
using Accela.ACA.Web.Common;
using Accela.ACA.Web.Util;
using Accela.ACA.WSProxy;
using Accela.Web.Controls;

namespace Accela.ACA.Web.Component
{
    /// <summary>
    /// the class for AppSpecInfoTableView.
    /// </summary>
    public partial class AppSpecInfoTableView : BaseUserControl
    {
        #region Fields

        /// <summary>
        /// default table width
        /// </summary>
        private const string TabWidth = "770px";
        
        /// <summary>
        /// indicating whether have visible tables
        /// </summary>
        private bool haveVisibleTables = false;

        /// <summary>
        /// edit button click function
        /// </summary>
        private string _editBtnClickScripts;

        /// <summary>
        /// edit button click handler.
        /// </summary>
        private EventHandler _editBtnEventHandler;

        /// <summary>
        /// the component SequenceNumber.
        /// </summary>
        private long _componentSeqNbr;

        #endregion Fields

        #region Property

        /// <summary>
        /// Gets all asi table in current cap type.
        /// </summary>
        public ASITUITable[] ASITUIData
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the list of sub cap models
        /// </summary>
        public IDictionary<string, CapModel4WS> SubCapModels
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets or sets a value indicating whether the control is template table control
        /// </summary>
        public bool IsTemplateTable
        {
            get;
            set;
        }

        /// <summary>
        /// Gets sequence number of asi table 
        /// </summary>
        public string ASITUIDataKey
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets a value indicating whether display Hijri Calendar 
        /// </summary>
        private bool IsDisplayHijriCalendar
        {
            get
            {
                return !IsTemplateTable && StandardChoiceUtil.IsEnableDisplayISLAMICCalendar();
            }
        }

        #endregion Property

        #region Methods

        /// <summary>
        /// create the table again, and show it.
        /// </summary>
        /// <param name="appSpecTableGroups">The app spec table groups.</param>
        /// <param name="editBtnClientClick">Client click event scripts for edit button</param>
        /// <param name="editBtnHandler">Behind click event handler for edit button</param>
        /// <param name="sectionName">section name.</param>
        /// <param name="componentSeqNbr">The component SequenceNumber.</param>
        public void Display(AppSpecificTableGroupModel4WS[] appSpecTableGroups, string editBtnClientClick, EventHandler editBtnHandler, string sectionName, long componentSeqNbr)
        {
            _editBtnClickScripts = editBtnClientClick;
            _editBtnEventHandler = editBtnHandler;
            ASITUIDataKey = sectionName;
            _componentSeqNbr = componentSeqNbr;

            var allVisibleGroups = new List<AppSpecificTableGroupModel4WS>();
            AppSpecificTableModel4WS[] allVisibleTables = CapUtil.GetAllVisibleASITables(ModuleName, appSpecTableGroups, allVisibleGroups);
            SubCapModels = CapUtil.GetSubCapsByASITGroups(ModuleName, allVisibleGroups);

            if (allVisibleTables != null && allVisibleTables.Length > 0)
            {
                haveVisibleTables = true;

                if (!IsPostBack)
                {
                    ASITUIData = ASITUIModelUtil.ConvertASITablesToUITables(allVisibleTables, ModuleName, false, string.Empty);
                    UIModelUtil.SetDataToUIContainer(ASITUIData, UIDataType.ASIT, ASITUIDataKey);
                }
                else
                {
                    ASITUIData = UIModelUtil.GetDataFromUIContainer(UIDataType.ASIT, new[] { ASITUIDataKey }) as ASITUITable[];
                }
            }
        }

        /// <summary>
        /// create the table again, and show it.
        /// </summary>
        /// <param name="templateGroups">The template table groups.</param>
        public void Display(TemplateGroup[] templateGroups)
        {
            TemplateSubgroup[] allVisibleTables = CapUtil.GetGenericTemplateTables(ModuleName, templateGroups);

            if (allVisibleTables != null && allVisibleTables.Length > 0)
            {
                haveVisibleTables = true;
                ASITUIData = ASITUIModelUtil.ConvertTemplateTablesToUITables(allVisibleTables, ModuleName, this.ClientID, string.Empty);
            }

            CreateTable();
        }

        /// <summary>
        /// Page load event handler.
        /// </summary>
        /// <param name="sender">Sender object</param>
        /// <param name="e">Event arguments</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            CreateTable();
        }

        /// <summary>
        /// create link button
        /// </summary>
        /// <param name="controls">a control collection that the link button added to</param>
        /// <param name="id">the id of the link button</param>
        /// <param name="asiTable">ASIT Table model</param>
        private void CreateLinkButton(ControlCollection controls, string id, ASITUITable asiTable)
        {
            AccelaButton button = new AccelaButton();
            button.ID = id;
            button.LabelKey = "per_permitConfirm_label_editButton";
            button.Click += _editBtnEventHandler;
            button.DivEnableCss = "ACA_SmButton ACA_SmButton_FontSize ACA_Button_Text ACA_FRight ACA_ASITButtonContainer";
            button.DivDisableCss = "ACA_SmButtonDisable ACA_SmButtonDisable_FontSize ACA_Button_Text ACA_FRight ACA_ASITButtonContainer";
            button.CausesValidation = false;
            button.CommandArgument = string.Format("{1}{0}{2}", ACAConstant.SPLIT_CHAR, _componentSeqNbr, ASITUIDataKey);

            if (!string.IsNullOrEmpty(_editBtnClickScripts))
            {
                button.OnClientClick = _editBtnClickScripts;
            }

            if (asiTable.IsReadOnly)
            {
                button.Enabled = false;
            }

            controls.Add(button);
        }

        /// <summary>
        /// According to app info tables, to display all data.
        /// step 1: loop app info table.
        /// step 2: loop rows
        /// step 3: loop columns
        /// </summary>
        private void CreateTable()
        {
            if (!haveVisibleTables)
            {
                return;
            }

            int groupIndex = 0;
            phAppInfoTable.Controls.Clear();
            bool isSuperCAP = false;

            if (!string.IsNullOrEmpty(ModuleName))
            {
                isSuperCAP = CapUtil.IsSuperCAP(ModuleName);
            }

            //loop appinfotable.
            foreach (ASITUITable appInfoTable in ASITUIData)
            {
                StringBuilder subGroup = new StringBuilder();
                subGroup.Append("<div class=\"ACA_TabRow ACA_Title_Text ACA_SmLabel\">");
                subGroup.Append("<table cellspacing=\"0\" cellpadding=\"0\" border=\"0\" class=\"ACA_FullWidthTable\" role=\"presentation\">");
                subGroup.Append("<tbody><tr>");
                subGroup.Append("<td>");

                string asitTitle = StringUtil.GetString(appInfoTable.AlternativeLabel, appInfoTable.TableTitle);

                if (!IsTemplateTable && isSuperCAP)
                {
                    string agencyLogo = CapUtil.GetAgencyLogoHtml(appInfoTable.AgencyCode, ModuleName);

                    subGroup.Append(agencyLogo);
                    subGroup.AppendFormat("<div class=\"ACA_ValCal_Title\">{0} ({1})</div>", asitTitle, appInfoTable.AgencyCode);
                }
                else
                {
                    subGroup.AppendFormat("<span class=\"ACA_FLeft tablename\">{0}</span>", asitTitle);
                }

                subGroup.Append("</td>");
                subGroup.Append("<td><div class=\"ACA_FRight\"><table cellspacing=\"0\" cellpadding=\"0\" border=\"0\" role=\"presentation\"><tbody><tr><td><div class=\"ACA_Title_Button\">");
                phAppInfoTable.Controls.Add(new LiteralControl(subGroup.ToString()));
                
                if (!IsTemplateTable)
                {
                    //add the edit button
                    string btnEditId = string.Format("btnEdit{0}_{1}Info", appInfoTable.GroupName, groupIndex);
                    CreateLinkButton(phAppInfoTable.Controls, btnEditId, appInfoTable);
                }

                subGroup = new StringBuilder();
                subGroup.Append("</div></td>");
                subGroup.Append("<td><div class=\"ACA_Title_Button\">&nbsp;&nbsp;&nbsp;</div></td></tr></tbody></table></div></td>");
                subGroup.Append("</tr>");
                bool isEmpty = appInfoTable.Rows == null || appInfoTable.Rows.Count == 0;

                if (isEmpty)
                {
                    subGroup.AppendFormat("<tr><td colspan='2' class='ACA_SmLabel ACA_SmLabel_FontSize_Restore'>{0}</td></tr>", GetTextByKey("aca_asit_msg_nodata"));
                }

                subGroup.Append("</tbody></table>");
                subGroup.Append("</div>");
                phAppInfoTable.Controls.Add(new LiteralControl(subGroup.ToString()));

                //Only show the edit button in review page if the ASIT sub group has not any data.
                if (isEmpty)
                {
                    groupIndex++;
                    continue;
                }

                DataTable dt = ASITUIModelUtil.ConvertASITableToDataTable(appInfoTable, IsDisplayHijriCalendar);

                Table tb = new Table();

                if (!IsTemplateTable)
                {
                    tb.Style["width"] = TabWidth;
                }

                tb.CssClass = "ACA_TabRow ACA_GridView ACA_Grid_Caption";
                tb.Style["margin-bottom"] = "18px";
                tb.Attributes.Add(ACAConstant.SUMMARY, GetTextByKey("aca_asit_datatablesummary"));
                tb.Caption = GetTextByKey("aca_caption_asit_datatable");

                string width = Convert.ToString(Math.Floor((1.00 / dt.Columns.Count) * 748));
                tb.Rows.Add(CreateTableHeader(dt, width));
                int itemIndex = 0;

                // loop rows
                foreach (DataRow dr in dt.Rows)
                {
                    TableRow tr = new TableRow();
                    string cssClass = itemIndex % 2 == 0 ? "ACA_TabRow_Even ACA_TabRow_Even_FontSize" : "ACA_TabRow_Odd ACA_TabRow_Odd_FontSize";
                    tr.CssClass = cssClass;
                    tr.Style.Add("float", "none");

                    if (!IsTemplateTable)
                    {
                        tr.Style["width"] = TabWidth;
                    }

                    bool isRowEmpty = true;

                    // loop columns
                    foreach (DataColumn column in dt.Columns)
                    {
                        if (column == null || ASITUITable.RowIdentityColumnName.Equals(column.ColumnName))
                        {
                            continue;
                        }

                        TableCell tc = new TableCell();
                        tc.Style["vertical-align"] = "top";
                        tc.Style["width"] = width + "px";
                        tc.Style["padding-right"] = "2px";

                        string tdValue = Convert.ToString(dr[column.ColumnName]);
                        if (!string.IsNullOrEmpty(tdValue))
                        {
                            isRowEmpty = false;
                        }

                        // use ScriptFilter.FilterScript to avoid script attacking
                        tc.Controls.Add(new LiteralControl(ScriptFilter.FilterScript(tdValue)));
                        tr.Cells.Add(tc);
                    }

                    if (!isRowEmpty)
                    {
                        itemIndex++;
                    }

                    tb.Rows.Add(tr);
                }

                phAppInfoTable.Controls.Add(tb);
                groupIndex++;
            }
        }

        /// <summary>
        /// According AppSpecificTableModel to construct a table header.
        /// </summary>
        /// <param name="asiTable">appInfo Table</param>
        /// <param name="columnWidth">column width.</param>
        /// <returns>a TableRow</returns>
        private TableRow CreateTableHeader(DataTable asiTable, string columnWidth)
        {
            TableRow tr = new TableRow();
            tr.CssClass = "ACA_TabRow_Header ACA_TabRow_Header_FontSize";
            tr.Style.Add("float", "none");

            if (!IsTemplateTable)
            {
                tr.Style[HtmlTextWriterStyle.Width] = TabWidth;
            }

            foreach (DataColumn column in asiTable.Columns)
            {
                if (column == null || ASITUITable.RowIdentityColumnName.Equals(column.ColumnName))
                {
                    continue;
                }

                TableHeaderCell tc = new TableHeaderCell();
                tc.Attributes["scope"] = Scope.col.ToString();
                tc.Style["padding-right"] = "2px";
                tc.CssClass += " ACA_AlignLeftOrRight";
                tc.Style["width"]  = Convert.ToString(columnWidth) + "px";
                tc.Style["vertical-align"] = "top";
                tc.Controls.Add(new LiteralControl(ScriptFilter.AntiXssHtmlEncode(column.ColumnName)));
                tr.Cells.Add(tc);
            }

            return tr;
        }

        #endregion Methods
    }
}
