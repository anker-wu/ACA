#region Header

/*
 * <pre>
 *  Accela Citizen Access
 *  File: ASITable.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2007-2014
 *
 *  Description:
 * use it to define some common function for ASI table object on web layer
 *  Notes:
 * $Id: ASITable.cs 276901 2014-08-08 02:13:05Z ACHIEVO\james.shi $.
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
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

using Accela.ACA.Common;
using Accela.ACA.Common.Common;
using Accela.ACA.Common.Util;
using Accela.ACA.ExpressionBuild;
using Accela.ACA.UI.Model;
using Accela.ACA.Web.ExpressionBuild;
using Accela.ACA.Web.Util;
using Accela.ACA.WSProxy;

namespace Accela.ACA.Web.Common
{
    /// <summary>
    /// ASITable class
    /// </summary>
    public class ASITable
    {
        #region Fields

        /// <summary>
        /// UI data of ASI Tables.
        /// </summary>
        private ASITUITable[] _asitUITables;

        /// <summary>
        /// The cap model
        /// </summary>
        private CapModel4WS _capModel;

        /// <summary>
        /// Indicating whether the current request is post back.
        /// </summary>
        private bool _isPostBack;

        /// <summary>
        /// Module name.
        /// </summary>
        private string _moduleName;

        /// <summary>
        /// ASI Table buffer
        /// </summary>
        private StringBuilder _strASIT = new StringBuilder(string.Empty);

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the ASITable class.
        /// </summary>
        /// <param name="capModel">cap model instance</param>
        /// <param name="moduleName">the module name</param>
        /// <param name="isPostBack">Indicating whether the current request is post back</param>
        /// <param name="needRunExpression">whether need run expression.</param>
        public ASITable(CapModel4WS capModel, string moduleName, bool isPostBack, bool needRunExpression = true)
        {
            _capModel = capModel;
            _moduleName = moduleName;
            _isPostBack = isPostBack;
            string uiDataKey = PageFlowConstant.SECTION_NAME_ASIT;

            if (_capModel != null && _capModel.appSpecTableGroups != null)
            {
                AppSpecificTableModel4WS[] allTables = CapUtil.GetAllVisibleASITables(_moduleName, _capModel.appSpecTableGroups);

                if (!_isPostBack)
                {
                    _asitUITables = ASITUIModelUtil.ConvertASITablesToUITables(allTables, _moduleName, false, string.Empty);

                    if (AppSession.GetCapModelFromSession(moduleName) == null)
                    {
                        //Expression Factory will retrieve cap model from session and TradeNameDetail page does not set the cap model to session.
                        AppSession.SetCapModelToSession(moduleName, capModel);
                    }

                    //Use the module name as the UI data key and set ui data to UI container.
                    UIModelUtil.SetDataToUIContainer(_asitUITables, UIDataType.ASIT, uiDataKey);

                    if (needRunExpression)
                    {
                        //Run onload expression for ASI tables.
                        RunASITOnLoadExpression(_asitUITables, _moduleName, uiDataKey);
                    }
                }

                _asitUITables = UIModelUtil.GetDataFromUIContainer(UIDataType.ASIT, new string[] { uiDataKey }) as ASITUITable[];
            }
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// Gets all column value 
        /// </summary>
        public string StrASIT
        {
            get
            {
                return _strASIT.ToString();
            }
        }

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

        #endregion Properties

        #region Methods

        /// <summary>
        /// display in detail view
        /// </summary>
        /// <returns>the placeholder object</returns>
        public PlaceHolder DisplayInDetailView()
        {
            PlaceHolder phAppInfoTable = new PlaceHolder();

            DisplayASITGroup(phAppInfoTable);

            return phAppInfoTable;
        }

        /// <summary>
        /// display in trade name detail view
        /// </summary>
        /// <returns>the placeholder object</returns>
        public PlaceHolder DisplayInTradeNameDetailView()
        {
            PlaceHolder phAppInfoTable = new PlaceHolder();

            if (_asitUITables == null || _asitUITables.Length == 0)
            {
                return phAppInfoTable;
            }

            //loop each asit ui table in each group and create corresponding control on UI
            foreach (ASITUITable appTable in _asitUITables)
            {
                DataTable dt = ASITUIModelUtil.ConvertASITableToDataTable(appTable);
                Table table = new Table();
                table.CellPadding = 0;
                table.CellSpacing = 0;
                table.Width = Unit.Pixel(712);
                table.Attributes.Add(ACAConstant.SUMMARY, LabelUtil.GetGlobalTextByKey("aca_asit_datatablesummary"));
                table.Caption = LabelUtil.GetGlobalTextByKey("aca_caption_asit_datatable");

                //add table title
                TableRow titleRow = new TableRow();
                TableCell titleCell = new TableCell();
                titleCell.Controls.Add(new LiteralControl("<div style='padding-left:0px;font-size:1.1em;width:712px;' class=\"ACA_TabRow ACA_Title_Text font13px\">" + appTable.TableTitle + "</div>"));
                titleCell.ColumnSpan = dt.Columns.Count;
                titleRow.Cells.Add(titleCell);
                table.Controls.Add(titleRow);

                //add table header for all columns
                TableRow headerRow = new TableRow();

                foreach (DataColumn column in dt.Columns)
                {
                    if (column == null || ASITUITable.RowIdentityColumnName.Equals(column.ColumnName))
                    {
                        continue;
                    }

                    TableHeaderCell headerCell = new TableHeaderCell();
                    headerCell.Attributes["scope"] = Scope.col.ToString();
                    headerCell.Style["padding"] = "0";
                    headerCell.Controls.Add(new LiteralControl("<div style='padding-left:0px;width:" + Convert.ToString(712 / dt.Columns.Count) + "px;'>" + column.ColumnName + "</div>"));
                    headerRow.Cells.Add(headerCell);
                }

                headerRow.Attributes["class"] = "ACA_TabRow_Header ACA_TabRow_Header_FontSize";
                table.Controls.Add(headerRow);
                int itemIndex = 0;

                // add all data rows
                foreach (DataRow dr in dt.Rows)
                {
                    TableRow row = new TableRow();
                    string cssClass = itemIndex % 2 == 0 ? "ACA_TabRow_Even ACA_TabRow_Even_FontSize" : "ACA_TabRow_Odd ACA_TabRow_Odd_FontSize";
                    row.CssClass = cssClass;
                    bool isItemEmpty = true;

                    foreach (DataColumn column in dt.Columns)
                    {
                        if (column == null || ASITUITable.RowIdentityColumnName.Equals(column.ColumnName))
                        {
                            continue;
                        }

                        TableCell cell = new TableCell();
                        cell.Style["vertical-align"] = "top";
                        string displayValue = ScriptFilter.FilterScript(Convert.ToString(dr[column.ColumnName]));
                        cell.Controls.Add(new LiteralControl("<div style='padding-left:0px;width:" + Convert.ToString(712 / dt.Columns.Count) + "px;'>" + displayValue + "</div>"));

                        if (!string.IsNullOrEmpty(displayValue))
                        {
                            _strASIT.Append(displayValue);
                            isItemEmpty = false;
                        }

                        row.Cells.Add(cell);
                    }

                    itemIndex++;

                    //if all columns are empty,cann't add the empty row.
                    if (!isItemEmpty)
                    {
                        table.Controls.Add(row);
                    }
                }

                phAppInfoTable.Controls.Add(table);
            }

            return phAppInfoTable;
        }

        /// <summary>
        /// Run on load expression for ASIT.
        /// </summary>
        /// <param name="asiUITables">The ASI UI tables</param>
        /// <param name="moduleName">module name</param>
        /// <param name="uiDataKey">The UI data key</param>
        private static void RunASITOnLoadExpression(ASITUITable[] asiUITables, string moduleName, string uiDataKey)
        {
            ExpressionFactory asitExpressionInstance = new ExpressionFactory(moduleName, ExpressionType.ASI_Table, null, null, asiUITables, null, uiDataKey);
            var expResults = asitExpressionInstance.RunExpressionForOnLoad();

            foreach (var exp in expResults)
            {
                ExpressionUtil.HandleASITExpressionResult(exp.Key, exp.Value, false);
            }
        }

        /// <summary>
        /// Get the valid index
        /// </summary>
        /// <param name="table">the ASIT table model</param>
        /// <param name="originalItemIndex">original ASIT column index</param>
        /// <param name="currentRowIndex">current ASIT row index</param>
        /// <returns>the valid index</returns>
        private int GetValidIndex(AppSpecificTableModel4WS table, int originalItemIndex, int currentRowIndex)
        {
            int validIndex = -1;

            if (table == null || table.columns == null)
            {
                return validIndex;
            }

            AppSpecificTableColumnModel4WS currentColumn = table.columns[originalItemIndex];

            if (!ACAConstant.INVALID_STATUS.Equals(currentColumn.recStatus, StringComparison.OrdinalIgnoreCase))
            {
                int maxValidColumn = table.columns.Length;
                validIndex = (currentRowIndex * maxValidColumn) + originalItemIndex;
            }

            return validIndex;
        }

        /// <summary>
        /// get selected resource item value.
        /// </summary>
        /// <param name="tableField">ASIT table field</param>
        /// <returns>the res item value</returns>
        private string GetSelectedResItemValue(AppSpecificTableField4WS tableField)
        {
            if (tableField != null && (tableField.selectOptions == null ||
                tableField.selectOptions.Length == 0))
            {
                return tableField.inputValue;
            }

            if (tableField != null)
            {
                foreach (RefAppSpecInfoDropDownModel4WS item in tableField.selectOptions)
                {
                    if (item.attrValue.Equals(tableField.inputValue, StringComparison.InvariantCultureIgnoreCase))
                    {
                        return item.resAttrValue;
                    }
                }    
            }

            return null;
        }

        /// <summary>
        /// display ASIT group
        /// </summary>
        /// <param name="phAppInfoTable">place holder</param>
        private void DisplayASITGroup(PlaceHolder phAppInfoTable)
        {
            //loop each field in each group and create corresponding control on UI
            if (_asitUITables == null || _asitUITables.Length == 0)
            {
                return;
            }

            var resultTableList = new List<HtmlTable>();

            foreach (ASITUITable appTable in _asitUITables)
            {
                bool isEmptyTable = true;
                var asitFieldsWithResValues = ASITUIModelUtil.GetASITFieldsWithResValues(appTable);
                HtmlTable table = new HtmlTable();
                table.CellPadding = 0;
                table.CellSpacing = 0;
                table.Attributes.Add("role", "presentation");
                HtmlTableRow titleRow = new HtmlTableRow();
                HtmlTableCell titleCell = new HtmlTableCell();

                if (appTable.Rows != null && appTable.Rows.Count != 0)
                {
                    titleCell.Attributes.Add("class", "MoreDetail_ItemCol1");
                    titleCell.Controls.Add(new LiteralControl("<div style='width:100%;' class='ACA_TabRow ACA_Title_Text font12px ACA_DivPadding0'>" + StringUtil.GetString(appTable.AlternativeLabel, appTable.TableTitle) + "</div>"));
                    titleRow.Controls.Add(titleCell);
                    table.Controls.Add(titleRow);

                    foreach (UIRow asitRow in appTable.Rows)
                    {
                        HtmlTableRow row = new HtmlTableRow();
                        HtmlTableCell cell = new HtmlTableCell();
                        cell.Width = "100%";
                        cell.Attributes.Add("class", "ACA_AlignLeftOrRight");
                        HtmlGenericControl contentHGC = new HtmlGenericControl();
                        StringBuilder buf = new StringBuilder();

                        foreach (ASITUIField field in asitRow.Fields)
                        {
                            string resFieldValue = ASITUIModelUtil.GetASITFieldResValue(field, asitFieldsWithResValues);
                            string displayValue = I18nStringUtil.GetString(resFieldValue, field.Value);

                            if (field.IsHidden || field.IsHiddenByExp || string.IsNullOrEmpty(displayValue))
                            {
                                continue;
                            }

                            if (field.Type == ((int)FieldType.HTML_TEXTBOX_OF_DATE).ToString() && IsDisplayHijriCalendar)
                            {
                                displayValue = HijriDateUtil.ToHijriDate(displayValue);
                            }

                            isEmptyTable = false;
                            StringBuilder fieldBuf = new StringBuilder();
                            StringBuilder valueBuf = new StringBuilder();
                            fieldBuf.Append("<span class='ACA_SmLabelBolder font11px'>");
                            fieldBuf.Append(ScriptFilter.FilterScript(field.Label) + ":</span>");
                            valueBuf.Append(ScriptFilter.FilterScript(displayValue));
                            _strASIT.Append(ScriptFilter.FilterScript(displayValue));
                            buf.Append("<div class='MoreDetail_ItemCol MoreDetail_ItemCol1'>" + fieldBuf.ToString() + "</div>");
                            buf.Append("<div class='MoreDetail_ItemCol MoreDetail_ItemCol2'>");
                            buf.Append(@"<span class='ACA_SmLabel ACA_SmLabel_FontSize'>" + valueBuf.ToString() + "</span>");
                            buf.Append("</div>");
                        }

                        string start = "<div class='MoreDetail_Item'>" + buf.ToString() + "</div>";
                        cell.Controls.Add(new LiteralControl(start));
                        row.Controls.Add(cell);
                        table.Controls.Add(row);
                    }
                }

                if (!isEmptyTable)
                {
                    resultTableList.Add(table);
                }
            }

            //Add data table and split line to container.
            for (int i = 0; i < resultTableList.Count; i++)
            {
                HtmlTable curTable = resultTableList[i];

                if (i < resultTableList.Count - 1)
                {
                    HtmlTableRow footRow = new HtmlTableRow();
                    HtmlTableCell footCell = new HtmlTableCell();
                    string css = @"<div class='ACA_TabRow ACA_BkGray'>&nbsp;</div>";
                    footCell.Controls.Add(new LiteralControl(css));
                    footRow.Controls.Add(footCell);
                    curTable.Controls.Add(footRow);
                }

                phAppInfoTable.Controls.Add(curTable);
            }
        }

        #endregion Methods
    }
}
