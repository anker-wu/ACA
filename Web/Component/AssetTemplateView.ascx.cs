#region Header

/**
 *  Accela Citizen Access
 *  File: AssetTemplateView.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2013-2014
 *
 *  Description:
 *  
 *
 *  Notes:
 * $Id: AssetDetailView.cs 134054 2013-10-16 18:03:54Z ACHIEVO\eric.he $.
 *  Revision History
 *  Date,                  Who,                 What
 */

#endregion Header

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using Accela.ACA.Common;
using Accela.ACA.Common.Common;
using Accela.ACA.Common.Util;
using Accela.ACA.Web.Common;
using Accela.ACA.WSProxy;
using Accela.Web.Controls;

namespace Accela.ACA.Web.Component
{
    /// <summary>
    /// Asset template view class
    /// </summary>
    public partial class AssetTemplateView : BaseUserControl
    {
        /// <summary>
        /// The column width
        /// </summary>
        private static readonly string COLUMN_WIDTH = Convert.ToString(Math.Floor((1.00 / 10) * 748));

        /// <summary>
        /// The row number
        /// </summary>
        private int _rowNumber = 0;

        /// <summary>
        /// Displays the specified template model.
        /// </summary>
        /// <param name="templateModel">The template model.</param>
        public void Display(AssetDataModel4WS templateModel)
        {
            DisplayFieldsLayout(templateModel);
            DisplayFieldTablesLayout(templateModel);
        }

        /// <summary>
        /// Handle ItemDataBound event for Template repeater.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">Event argument.</param>
        protected void TemplateFieldsList_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                DataAttributeModel field = e.Item.DataItem as DataAttributeModel;

                if (field == null ||
                    (string.IsNullOrEmpty(field.g1AttributeValue) && string.IsNullOrEmpty(field.resG1AttributeValue)))
                {
                    e.Item.Visible = false;
                    return;
                }

                AccelaNameValueLabel lblField1 = e.Item.FindControl("lblField1") as AccelaNameValueLabel;
                AccelaNameValueLabel lblField2 = e.Item.FindControl("lblField2") as AccelaNameValueLabel;

                if (lblField1 != null && lblField2 != null)
                {
                    if (_rowNumber % 2 == 0)
                    {
                        lblField1.Text = field.g1AttributeName + ":";
                        lblField1.Value = ModelUIFormat.FormatAssetTemplateField(field.resG1AttributeValue, field.g1AttributeValue, field.r1AttributeValueType);
                        lblField2.Visible = false;
                    }
                    else
                    {
                        lblField2.Text = field.g1AttributeName + ":";
                        lblField2.Value = ModelUIFormat.FormatAssetTemplateField(field.resG1AttributeValue, field.g1AttributeValue, field.r1AttributeValueType);
                        lblField1.Visible = false;
                    }
                }

                _rowNumber++;
            }
        }

        /// <summary>
        /// Displays the fields layout.
        /// </summary>
        /// <param name="templateModel">The template model.</param>
        private void DisplayFieldsLayout(AssetDataModel4WS templateModel)
        {
            if (templateModel == null)
            {
                return;
            }

            templateFieldsList.DataSource = templateModel.dataAttributeList;
            templateFieldsList.DataBind();
        }

        /// <summary>
        /// Displays the field tables layout.
        /// </summary>
        /// <param name="templateModel">The template model.</param>
        private void DisplayFieldTablesLayout(AssetDataModel4WS templateModel)
        {
            if (templateModel == null)
            {
                return;
            }

            //Table data
            AssetAttrTableValueModel[] assetAttributeModels = templateModel.assetAttrTableValueList;

            //Table structure
            AttrTableAttributeModel[] tableAttributes = templateModel.refAttrTableAttriList;

            if (assetAttributeModels == null || assetAttributeModels.Length == 0 || tableAttributes == null || tableAttributes.Length == 0)
            {
                return;
            }

            divGenericTemplateTable.Visible = true;
            string[] tableNames = assetAttributeModels.Select(o => o.tableName).Distinct().ToArray();
            int? maxRowNumber = assetAttributeModels.Max(o => o.rowNumber);

            foreach (string tableName in tableNames)
            {
                // 1. Set the Asset template table name
                string assetTableName = GetAssetTemplateTableNameHtml(tableName, assetAttributeModels);
                phAssetTable.Controls.Add(new LiteralControl(assetTableName));

                // 2. Set the Asset template data table
                phAssetTable.Controls.Add(new LiteralControl("<div class=\"ACA_Grid_OverFlow templatetable\">"));

                Table assetDataTable = new Table();
                assetDataTable.CssClass = "ACA_TabRow ACA_GridView ACA_Grid_Caption";
                assetDataTable.Attributes.Add(ACAConstant.SUMMARY, GetTextByKey("aca_assettemplateview_msg_datatablesummary"));
                assetDataTable.Caption = GetTextByKey("aca_caption_asset_attributetable");
                
                // 2.1. Set the Asset template data header
                TableRow assetTemplateDataHeader = GetAssetTemplateDataHeader(tableAttributes.Where(o => tableName.Equals(o.r1AttributeTableName, StringComparison.InvariantCultureIgnoreCase)), COLUMN_WIDTH);

                assetDataTable.Rows.Add(assetTemplateDataHeader);

                // 2.2. Set the Asset template data body
                TableRow[] bodyRows = GetAssetTemplateDataBodyRows(tableName, assetAttributeModels, tableAttributes, maxRowNumber);
                assetDataTable.Rows.AddRange(bodyRows);
                
                phAssetTable.Controls.Add(assetDataTable);
                phAssetTable.Controls.Add(new LiteralControl("</div>"));
            }
        }

        /// <summary>
        /// Get the asset template table name's html.
        /// </summary>
        /// <param name="tableName">The table name.</param>
        /// <param name="templateTableFields">The template table filed list.</param>
        /// <returns>The asset template table name's html.</returns>
        private string GetAssetTemplateTableNameHtml(string tableName, IEnumerable<AssetAttrTableValueModel> templateTableFields)
        {
            StringBuilder tableString = new StringBuilder();
            tableString.Append("<div class=\"ACA_TabRow ACA_Title_Text ACA_SmLabel\">");
            tableString.Append("<table cellspacing=\"0\" cellpadding=\"0\" border=\"0\" class=\"ACA_FullWidthTable\" role=\"presentation\">");
            tableString.Append("<tbody><tr>");
            tableString.Append("<td>");
            tableString.AppendFormat("<span class=\"ACA_FLeft tablename\">{0}</span>", tableName);
            tableString.Append("</td>");
            tableString.Append("<td><div class=\"ACA_FRight\"><table cellspacing=\"0\" cellpadding=\"0\" border=\"0\" role=\"presentation\"><tbody><tr><td><div class=\"ACA_Title_Button\">");
            tableString.Append("</div></td>");
            tableString.Append("<td><div class=\"ACA_Title_Button\">&nbsp;&nbsp;&nbsp;</div></td></tr></tbody></table></div></td>");
            tableString.Append("</tr>");

            if (!templateTableFields.Any(o => tableName.Equals(o.tableName)))
            {
                tableString.AppendFormat(
                    "<tr><td colspan='2' class='ACA_SmLabel ACA_SmLabel_FontSize_Restore'>{0}</td></tr>",
                    GetTextByKey("aca_assettemplateview_msg_nodata"));
            }

            tableString.Append("</tbody></table>");
            tableString.Append("</div>");

            return tableString.ToString();
        }

        /// <summary>
        /// Creates the table header.
        /// </summary>
        /// <param name="header">The header.</param>
        /// <param name="width">The width.</param>
        /// <returns>Table row object</returns>
        private TableRow GetAssetTemplateDataHeader(IEnumerable<AttrTableAttributeModel> header, string width)
        {
            TableRow tr = new TableRow();
            tr.CssClass = "ACA_TabRow_Header ACA_TabRow_Header_FontSize";

            foreach (var column in header)
            {
                if (column == null)
                {
                    continue;
                }

                TableHeaderCell tc = new TableHeaderCell();
                tc.Attributes["scope"] = Scope.col.ToString();
                tc.CssClass += "ACA_AlignLeftOrRight";
                tc.Style["width"] = width + "px";
                tc.Controls.Add(new LiteralControl(column.r1AttributeName));
                tr.Cells.Add(tc);
            }

            return tr;
        }

        /// <summary>
        /// Get the asset template data body rows.
        /// </summary>
        /// <param name="tableName">The table name.</param>
        /// <param name="assetAttrTableValues">The asset table value filed list.</param>
        /// <param name="tableAttributes">The table attribute field list.</param>
        /// <param name="maxRowNumber">The max row number.</param>
        /// <returns>The asset template data body rows.</returns>
        private TableRow[] GetAssetTemplateDataBodyRows(string tableName, AssetAttrTableValueModel[] assetAttrTableValues, AttrTableAttributeModel[] tableAttributes, int? maxRowNumber)
        {
            int itemIndex = 0;
            List<TableRow> results = new List<TableRow>();

            if (assetAttrTableValues == null)
            {
                return results.ToArray();
            }

            for (int rowNumber = 1; rowNumber <= maxRowNumber; rowNumber++)
            {
                TableRow tr = new TableRow();
                string cssClass = itemIndex % 2 == 0
                    ? "ACA_TabRow_Even ACA_TabRow_Even_FontSize"
                    : "ACA_TabRow_Odd ACA_TabRow_Odd_FontSize";
                tr.CssClass = cssClass;
                bool isRowEmpty = true;

                foreach (var attribute in tableAttributes)
                {
                    AssetAttrTableValueModel tableColumn = assetAttrTableValues.First(
                        o => o.rowNumber == rowNumber && o.r1AttributeName.Equals(attribute.r1AttributeName) && tableName.Equals(o.tableName));

                    if (tableColumn == null)
                    {
                        continue;
                    }

                    TableCell tc = new TableCell();
                    tc.Style["width"] = COLUMN_WIDTH + "px";

                    string tdValue = I18nStringUtil.GetString(tableColumn.resR1AttributeValue, tableColumn.dispR1AttributeValue, tableColumn.r1AttributeValue);
                    tdValue = ModelUIFormat.FormatAssetTemplateField(tdValue, string.Empty, tableColumn.r1AttributeValueType);

                    if (!string.IsNullOrEmpty(tdValue))
                    {
                        isRowEmpty = false;
                    }

                    tc.Controls.Add(new LiteralControl(ScriptFilter.FilterScript(tdValue)));
                    tr.Cells.Add(tc);
                }

                if (!isRowEmpty)
                {
                    itemIndex++;
                }

                results.Add(tr);
            }

            return results.ToArray();
        }
    }
}