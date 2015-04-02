#region Header

/**
* <pre>
*
*  Accela Citizen Access
*  File: GridViewBuildHelper.cs
*
*  Accela, Inc.
*  Copyright (C): 2011-2014
*
*  Description:
*  The grid view builder util.
*
*  Notes:
*      $Id: GridViewBuildHelper.cs 205560 2011-10-17 07:54:38Z ACHIEVO\alan.hu $.
*  Revision History
*  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
* </pre>
*/

#endregion Header

using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using Accela.ACA.BLL.APO;
using Accela.ACA.BLL.Common;
using Accela.ACA.Common;
using Accela.ACA.Common.Common;
using Accela.ACA.Common.Util;
using Accela.ACA.Web.AppCode.Common;
using Accela.ACA.WSProxy;
using Accela.ACA.WSProxy.Common;
using Accela.Web.Controls;
using Accela.Web.Controls.ControlRender;
using AccelaWebControlExtender;

namespace Accela.ACA.Web.Common.Control
{
    /// <summary>
    /// The grid view builder utility.
    /// </summary>
    public static class GridViewBuildHelper
    {
        #region Fields

        /// <summary>
        /// The key that store the merged attribute names information.
        /// </summary>
        private const string MERGED_ATTRIBUTE_KEY = "MergedAttributeNames";
        
        /// <summary>
        /// The default property name of the template attribute in MODEL.
        /// </summary>
        private const string DEFAULT_TEMPLATE_PROPERTY_NAME = "templateAttributes";

        #endregion Fields

        #region Delegate

        /// <summary>
        /// The delegate for getting the data source.
        /// </summary>
        /// <param name="queryFormat">The query format model.</param>
        /// <returns>The data source.</returns>
        public delegate DownloadResultModel DelegateGetDataSource(QueryFormat queryFormat);

        /// <summary>
        /// The delegate for formatting the export content.
        /// </summary>
        /// <param name="dataRow">The data row.</param>
        /// <param name="visibleColumns">The visible columns</param>
        /// <returns>The formatted data row.</returns>
        public delegate Dictionary<string, string> DelegateExportFormat(DataRow dataRow, List<string> visibleColumns);

        #endregion Delegate

        #region Public Methods

        /// <summary>
        /// Initializes the grid with template column.
        /// </summary>
        /// <param name="grid">The grid view.</param>
        /// <param name="moduleName">The module name.</param>
        /// <param name="templateGenus">The template genus, ex: LP, Contact.</param>
        public static void InitializeGridWithTemplate(AccelaGridView grid, string moduleName, string templateGenus)
        {
            if (string.IsNullOrEmpty(grid.GridViewNumber))
            {
                return;
            }

            // set the grid's template genus.
            grid.ExtendedProperties.Add(ACAConstant.TEMPLATE_GENUS_LEVEL_TYPE, templateGenus);

            // get the data source about simple view element model list.
            string callerId = AppSession.IsAdmin ? ACAConstant.ADMIN_CALLER_ID : AppSession.User.UserID;
            IGviewBll gviewBll = ObjectFactory.GetObject<IGviewBll>();

            GFilterScreenPermissionModel4WS permission = new GFilterScreenPermissionModel4WS();
            permission.permissionLevel = ACAConstant.TEMPLATE_GENUS_LEVEL_TYPE;
            permission.permissionValue = templateGenus;

            SimpleViewElementModel4WS[] models = gviewBll.GetSimpleViewElementModel(moduleName, permission, grid.GridViewNumber, callerId);
            List<string> transContactTypes = GetContactTypes(ContactTypeSource.Transaction);
            List<string> refContactTypes = GetContactTypes(ContactTypeSource.Reference);
            models = GViewUtil.FilterPeopleTempate(models, grid.GridViewNumber, transContactTypes, refContactTypes);

            Dictionary<string, IList<SimpleViewElementModel4WS>> dictTemplateModel = TemplateUtil.GetMergedTemplateViewElement(models);

            // synchronizate the models, remove the model that has merged.
            SimpleViewElementModel4WS[] mergedModels = TemplateUtil.MergeTemplateSimpleViewElement(models, dictTemplateModel);

            // set the grid column visible before the template column generate.
            string gridColumnsVisible = GetGridColumnsVisible(grid, mergedModels);
            grid.GridColumnsVisible = gridColumnsVisible;

            // set template fields to grid
            foreach (IList<SimpleViewElementModel4WS> templateModels in dictTemplateModel.Values)
            {
                SetSingleTemplateField2Grid(grid, templateModels, templateGenus);
            }

            // hide the grid columns' visible
            HideGridViewColumnsBySimpleViewElementModel(grid, mergedModels, gridColumnsVisible);

            // set the grid columns' property, ex: display order
            SetGridColumnPropertiesBySimpleViewElement(grid, mergedModels);
        }

        /// <summary>
        /// set grid view's columns
        /// </summary>
        /// <param name="grid">GridView object.</param>
        /// <param name="moduleName">Module name.</param>
        /// <param name="isAdmin">If is admin.</param>
        public static void SetSimpleViewElements(AccelaGridView grid, string moduleName, bool isAdmin)
        {
            if (!string.IsNullOrEmpty(grid.GridViewNumber))
            {
                IGviewBll gviewBll = (IGviewBll)ObjectFactory.GetObject(typeof(IGviewBll));
                SimpleViewElementModel4WS[] models = gviewBll.GetSimpleViewElementModel(moduleName, grid.GridViewNumber);

                HideGridViewColumnsBySimpleViewElementModel(grid, models);

                SetGridColumnPropertiesBySimpleViewElement(grid, models);
            }
        }

        /// <summary>
        /// hide grid view's column
        /// </summary>
        /// <param name="grid">AccelaGridView object</param>
        /// <param name="moduleName">module name</param>
        public static void HideGridViewColumns(AccelaGridView grid, string moduleName)
        {
            IGviewBll gviewBll = (IGviewBll)ObjectFactory.GetObject(typeof(IGviewBll));
            SimpleViewElementModel4WS[] models = gviewBll.GetSimpleViewElementModel(moduleName, grid.GridViewNumber);

            HideGridViewColumnsBySimpleViewElementModel(grid, models);
        }

        /// <summary>
        /// Merges the template attributes with the standard data to data table.
        /// </summary>
        /// <typeparam name="T">The type of the list.</typeparam>
        /// <param name="gridView">The grid view.</param>
        /// <param name="list">The ICollection data source.</param>
        /// <param name="templatePropertyName">The template property name that bind to the type of T.</param>
        /// <returns> Return the data table merges template attributes with the standard data.</returns>
        public static DataTable MergeTemplateAttributes2DataTable<T>(GridView gridView, ICollection<T> list, string templatePropertyName = DEFAULT_TEMPLATE_PROPERTY_NAME)
        {
            if (list == null || list.Count == 0 || gridView == null)
            {
                return null;
            }

            DataTable dtStandard = ObjectConvertUtil.ConvertModels2DataTable(list.ToArray(), false, false);

            return MergeTemplateAttributes2DataTable(gridView, dtStandard, templatePropertyName);
        }

        /// <summary>
        /// Merges the template attributes with the standard data to data table.
        /// </summary>
        /// <param name="gridView">The grid view.</param>
        /// <param name="dtSource">The data table source.</param>
        /// <param name="templatePropertyName">The template property name that belong to the data table's column.</param>
        /// <returns> Return the data table merges template attributes with the standard data.</returns>
        public static DataTable MergeTemplateAttributes2DataTable(GridView gridView, DataTable dtSource, string templatePropertyName = ColumnConstant.TEAMPLATE_ATTRIBUTE)
        {
            // if dtSource.Rows.Count = 0, it need contruct the datatable structect.
            if (dtSource == null || gridView == null)
            {
                return null;
            }

            // 1. define the data table columns.
            IList<DataColumn> templateColumns = ConvertTemplateField2DataColumns(gridView.Columns, dtSource);

            DataTable dtResult = dtSource.Copy();
            dtResult.Columns.AddRange(templateColumns.ToArray());

            // 2. add the template value to the data table.
            if (dtResult.Columns.Contains(templatePropertyName) && dtResult.Rows.Count > 0)
            {
                foreach (DataRow dr in dtResult.Rows)
                {
                    object[] attributeModels = dr[templatePropertyName] as object[];

                    SetTemplateRowData(dr, templateColumns, attributeModels);
                }
            }

            return dtResult;
        }

        /// <summary>
        /// Determines whether this grid view exist template column.
        /// </summary>
        /// <param name="grid">The grid view.</param>
        /// <returns>Return true if this grid view exist template column; otherwise, false.</returns>
        public static bool IsExistTemplateColumn(GridView grid)
        {
            bool result = false;

            foreach (DataControlField column in grid.Columns)
            {
                AccelaTemplateField templateField = column as AccelaTemplateField;

                if (templateField != null && templateField.Visible && !templateField.IsStandard)
                {
                    result = true;
                    break;
                }
            }

            return result;
        }

        /// <summary>
        /// Get all template attribute names.
        /// </summary>
        /// <param name="grid">grid view control</param>
        /// <returns>Array of attribute name.</returns>
        public static string[] GetTemplateAttributeNames(GridView grid)
        {
            List<string> columns = new List<string>();

            foreach (DataControlField column in grid.Columns)
            {
                AccelaTemplateField templateField = column as AccelaTemplateField;

                if (templateField != null && templateField.Visible && !templateField.IsStandard)
                {
                    columns.AddRange(templateField.MergedAttributeNames);
                }
            }

            return columns.ToArray();
        }

        /// <summary>
        /// Downloads all.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="Accela.Web.Controls.GridViewDownloadEventArgs"/> instance containing the event data.</param>
        /// <param name="delegateGetDataSource">The delegate get data source.</param>
        public static void DownloadAll(object sender, GridViewDownloadEventArgs e, DelegateGetDataSource delegateGetDataSource)
        {
            DownloadAll(sender, e, delegateGetDataSource, null);
        }

        /// <summary>
        /// Downloads all.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="Accela.Web.Controls.GridViewDownloadEventArgs"/> instance containing the event data.</param>
        /// <param name="delegateGetDataSource">The delegate that get data source.</param>
        /// <param name="delegateExportFormat">The delegate that export format.</param>
        public static void DownloadAll(object sender, GridViewDownloadEventArgs e, DelegateGetDataSource delegateGetDataSource, DelegateExportFormat delegateExportFormat)
        {
            AccelaGridView gridView = sender as AccelaGridView;

            if (gridView == null || gridView.IsShortList)
            {
                return;
            }

            // search all data source for download
            DataTable dataSource = GetDownloadDataSource(sender, delegateGetDataSource);
            string exportedContent = GetExportedContent(dataSource, e, delegateExportFormat);

            if (!string.IsNullOrEmpty(exportedContent))
            {
                gridView.ExportedContent = exportedContent;
            }
        }

        /// <summary>
        /// Get the grid view's header row.
        /// </summary>
        /// <param name="gridView">The grid view.</param>
        /// <returns>Return the grid view's header row.</returns>
        public static GridViewRow GetHeaderRow(AccelaGridView gridView)
        {
            // if the data bind not empty, use the grid view's header row directly.
            if (gridView.Rows.Count > 0 && gridView.HeaderRow != null)
            {
                return gridView.HeaderRow;
            }

            // if the data bind is empty, the header row is contained in the [Controls].
            GridViewRow headerRow = null;
            Table gridTable = gridView.Controls[0] as Table;

            if (gridTable != null)
            {
                foreach (TableRow tableRow in gridTable.Rows)
                {
                    GridViewRow gridViewRow = tableRow as GridViewRow;

                    if (gridViewRow != null && gridViewRow.RowType == DataControlRowType.Header)
                    {
                        headerRow = gridViewRow;
                        break;
                    }
                }
            }

            return headerRow;
        }

        /// <summary>
        /// Set should hidden columns by columnId.
        /// </summary>
        /// <param name="gridView">the grid view</param>
        /// <param name="columnIds">the grid view column id</param>
        public static void SetHiddenColumn(AccelaGridView gridView, IEnumerable<string> columnIds)
        {
            foreach (string columnId in columnIds)
            {
                foreach (DataControlField column in gridView.Columns)
                {
                    AccelaTemplateField templateField = column as AccelaTemplateField;

                    if (templateField != null 
                        && templateField.ColumnId != null 
                        && templateField.ColumnId.Equals(columnId, StringComparison.InvariantCultureIgnoreCase))
                    {
                        templateField.HideField = true;
                        break;
                    }
                }
            }
        }

        /// <summary>
        /// Hide the column by the specified columnId in the specified <see cref="AccelaGridView"/>.
        /// </summary>
        /// <param name="gridView">the grid view</param>
        /// <param name="columnId">the column id hidden</param>
        public static void HiddGridViewColumn(AccelaGridView gridView, string columnId)
        {
            foreach (DataControlField column in gridView.Columns)
            {
                AccelaTemplateField templateField = column as AccelaTemplateField;

                if (templateField != null
                    && templateField.ColumnId != null
                    && templateField.ColumnId.Equals(columnId, StringComparison.InvariantCultureIgnoreCase))
                {
                    column.Visible = false;
                    break;
                }
            }
        }

        #endregion Public Methods

        #region Private Methods

        /// <summary>
        /// Sets the single template field to grid view.
        /// </summary>
        /// <param name="grid">The grid view.</param>
        /// <param name="templateModels">The SimpleViewElementModel list, the model list have the same label.</param>
        /// <param name="templateGenus">The template genus.</param>
        private static void SetSingleTemplateField2Grid(AccelaGridView grid, IList<SimpleViewElementModel4WS> templateModels, string templateGenus)
        {
            if (grid == null || templateModels == null || templateModels.Count == 0)
            {
                return;
            }

            SimpleViewElementModel4WS templateModel = templateModels[0];
            string elementType = templateModel.elementType;
            bool hasFindTemplateModel = false;

            // Gets the template attribute name list, they have the same display label, so need merge.
            IList<string> viewElementNameList = new List<string>();
            foreach (SimpleViewElementModel4WS model in templateModels)
            {
                if (model != null)
                {
                    viewElementNameList.Add(model.viewElementName);

                    // if the template customized which stored in DB
                    if (!hasFindTemplateModel && !model.isOriginalTemplate)
                    {
                        templateModel = model;
                        hasFindTemplateModel = true;
                    }

                    // set the element type, if they have different, set the default control type as [Text].
                    if (elementType != model.elementType)
                    {
                        elementType = ControlType.Text.ToString();
                    }
                }
            }

            int fieldWidth = templateModel.elementWidth > 0 ? templateModel.elementWidth : AccelaGridView.DEFAULT_GRIDVIEW_TEMPLATE_COLUMN_WIDTH;
            ControlType dataType;
            Enum.TryParse(elementType, true, out dataType);

            AccelaTemplateField templateColumn = new AccelaTemplateField();
            templateColumn.AttributeName = TemplateUtil.GetTemplateControlID(templateModel.viewElementName);
            templateColumn.MergedAttributeNames = viewElementNameList;
            templateColumn.IsStandard = false;
            templateColumn.HeaderStyle.Width = Unit.Pixel(fieldWidth);
            templateColumn.ItemStyle.Width = Unit.Pixel(fieldWidth);
            templateColumn.HeaderTemplate = CreateHeaderTemplate(grid, templateModel, templateGenus);
            templateColumn.ItemTemplate = CreateDataItemTemplate(templateModel);
            templateColumn.BindDataType = dataType;

            grid.Columns.Add(templateColumn);
        }

        /// <summary>
        /// Gets the header template.
        /// </summary>
        /// <param name="grid">The grid view.</param>
        /// <param name="model">The SimpleViewElementModel4WS object.</param>
        /// <param name="templateGenus">The template genus.</param>
        /// <returns>return the header template.</returns>
        private static ITemplate CreateHeaderTemplate(AccelaGridView grid, SimpleViewElementModel4WS model, string templateGenus)
        {
            PlaceHolder headerControl = new PlaceHolder();
            string controlID = TemplateUtil.GetTemplateControlID(model.viewElementName);

            // 1. add the div begin tag
            Literal literal = new Literal();
            literal.Text = "<div class=\"ACA_Header_Row\">";
            headerControl.Controls.Add(literal);

            // 2. add the header label
            if (AppSession.IsAdmin)
            {
                LinkButton headerLink = new LinkButton();
                headerLink.CommandName = "Header";
                headerLink.CausesValidation = false;

                AccelaLabel label = new AccelaLabel();
                label.ID = controlID;
                label.Text = model.labelValue;
                label.IsGridViewHeadLabel = true;
                label.IsNeedEncode = false;

                // add the template attribute
                TemplateAttribute templateAttribute = new TemplateAttribute();
                templateAttribute.AttributeName = controlID;
                templateAttribute.DefaultLanguageLabel = !string.IsNullOrEmpty(model.labelId) ? model.labelId : model.labelValue;

                // Create the design support extender for template header label.
                // The header label is dynamic create, so the grid not exist the extender control.
                AccelaWebControlExtenderExtender extender = ControlRenderUtil.CreateDesinSupportExtender(label, templateAttribute);
                extender.ViewElementID = model.viewElementName;
                extender.SectionID = grid.GridViewNumber;
                extender.GridColumnsVisible = grid.GridColumnsVisible;
                extender.TemplateGenus = templateGenus;

                headerLink.Controls.Add(label);
                headerControl.Controls.Add(headerLink);
            }
            else
            {
                GridViewHeaderLabel headerLink = new GridViewHeaderLabel();
                headerLink.CommandName = "Header";
                headerLink.CausesValidation = false;
                headerLink.ID = controlID;
                headerLink.Text = string.Format("<span>{0}</span>", model.labelValue);
                headerLink.SortExpression = controlID;
                headerControl.Controls.Add(headerLink);
            }

            // 3. add the div end tag
            literal = new Literal();
            literal.Text = "</div>";
            headerControl.Controls.Add(literal);

            ITemplate headerTemplate = new DynamicTemplate(ListItemType.Header, headerControl);
            return headerTemplate;
        }

        /// <summary>
        /// Gets the data item template.
        /// </summary>
        /// <param name="model">The SimpleViewElementModel4WS object.</param>
        /// <returns>Return the data item template.</returns>
        private static ITemplate CreateDataItemTemplate(SimpleViewElementModel4WS model)
        {
            // add item template, cannot add HtmlGenericControl
            AccelaLabel itemLabel = new AccelaLabel();
            itemLabel.ID = TemplateUtil.GetTemplateControlID(model.viewElementName);

            DynamicTemplate itemTemplate = new DynamicTemplate(ListItemType.Item, itemLabel);
            itemTemplate.BindPropertyName = "Text";
            itemTemplate.BindDataExpression = itemLabel.ID;
            itemTemplate.BindControlID = itemLabel.ID;

            return itemTemplate;
        }

        /// <summary>
        /// Convert the grid template field to data table columns.
        /// </summary>
        /// <param name="gridColumns">The grid column list.</param>
        /// <param name="dtSource">If the source type is data table, need the data table source.</param>
        /// <returns> Return the data table columns for template fields.</returns>
        private static IList<DataColumn> ConvertTemplateField2DataColumns(DataControlFieldCollection gridColumns, DataTable dtSource)
        {
            IList<DataColumn> result = new List<DataColumn>();

            foreach (TemplateField templateField in gridColumns)
            {
                // convert the grid template field to data table column
                AccelaTemplateField field = templateField as AccelaTemplateField;

                if (field != null && !field.IsStandard && (dtSource == null || !dtSource.Columns.Contains(field.AttributeName)))
                {
                    Type dataType;
                    switch (field.BindDataType)
                    {
                        case ControlType.Number:
                            dataType = typeof(double);
                            break;
                        case ControlType.Date:
                            dataType = typeof(DateTime);
                            break;
                        default:
                            dataType = typeof(string);
                            break;
                    }

                    DataColumn dataColumn = new DataColumn(field.AttributeName, dataType);
                    dataColumn.ExtendedProperties.Add(MERGED_ATTRIBUTE_KEY, field.MergedAttributeNames);

                    result.Add(dataColumn);
                }
            }

            return result;
        }

        /// <summary>
        /// Sets the template row data.
        /// </summary>
        /// <param name="dr">The data row.</param>
        /// <param name="templateColumns">The template columns.</param>
        /// <param name="attributeModels">The attribute models that contain the template field data.</param>
        private static void SetTemplateRowData(DataRow dr, IList<DataColumn> templateColumns, object[] attributeModels)
        {
            if (attributeModels == null || attributeModels.Length == 0)
            {
                return;
            }

            ITemplateBll templateBll = (ITemplateBll)ObjectFactory.GetObject(typeof(ITemplateBll));
            TemplateAttributeModel[] allAttributesFromCache = null;

            foreach (DataColumn column in templateColumns)
            {
                // get the merged attribute name
                IList<string> mergedAttributeNames = column.ExtendedProperties[MERGED_ATTRIBUTE_KEY] as IList<string>;

                if (mergedAttributeNames == null || mergedAttributeNames.Count == 0)
                {
                    continue;
                }

                // set the data row value via the template attribute.
                foreach (object obj in attributeModels)
                {
                    TemplateAttributeModel attrModel = obj as TemplateAttributeModel;

                    if (attrModel == null)
                    {
                        continue;
                    }

                    string viewElementName = attrModel.attributeName + ACAConstant.SPLIT_DOUBLE_COLON + attrModel.templateType;

                    if (mergedAttributeNames.Contains(viewElementName))
                    {
                        if (!string.IsNullOrEmpty(attrModel.attributeValue))
                        {
                            object columnValue = null;
                            ControlType controlType = ControlType.Text;
                            Enum.TryParse(attrModel.attributeValueDataType, out controlType);

                            switch (controlType)
                            {
                                case ControlType.Date:
                                    columnValue = I18nDateTimeUtil.ParseFromWebService(attrModel.attributeValue);
                                    break;
                                case ControlType.Number:
                                    columnValue = I18nNumberUtil.ParseNumberFromWebService(attrModel.attributeValue);
                                    break;
                                case ControlType.Radio:
                                    if (ValidationUtil.IsYes(attrModel.attributeValue))
                                    {
                                        columnValue = BasePage.GetStaticTextByKey("ACA_RadioButtonText_Yes");
                                    }
                                    else if (ValidationUtil.IsNo(attrModel.attributeValue))
                                    {
                                        columnValue = BasePage.GetStaticTextByKey("ACA_RadioButtonText_No");
                                    }

                                    break;
                                case ControlType.DropdownList:
                                    if (allAttributesFromCache == null)
                                    {
                                        allAttributesFromCache = templateBll.GetPeopleTemplateAttributes(
                                            attrModel.templateType, attrModel.serviceProviderCode, AppSession.User.PublicUserId);
                                    }

                                    columnValue = TemplateUtil.GetI18NValueForDropDownList(attrModel, allAttributesFromCache);
                                    break;
                                default:
                                    columnValue = attrModel.attributeValue;
                                    break;
                            }

                            dr[column.ColumnName] = columnValue;
                        }
                        else
                        {
                            dr[column.ColumnName] = DBNull.Value;
                        }

                        break;
                    }
                }
            }
        }

        /// <summary>
        /// Hide the grid view's column
        /// </summary>
        /// <param name="grid">The grid view.</param>
        /// <param name="models">The SimpleViewElementModel.</param>
        /// <param name="gridColumnsVisible">The string of the grid columns visible.</param>
        private static void HideGridViewColumnsBySimpleViewElementModel(AccelaGridView grid, SimpleViewElementModel4WS[] models, string gridColumnsVisible = null)
        {
            if (grid == null || models == null || models.Length == 0)
            {
                return;
            }

            // it is the total width of the visible columns.
            int totalColumnWidth = 0;
            DataControlField lastColumn = null;
            int maxDisplayOrder = 0;

            // set the dictionary model, so get the related model convenience.
            Dictionary<string, SimpleViewElementModel4WS> dictModels = new Dictionary<string, SimpleViewElementModel4WS>();
            foreach (SimpleViewElementModel4WS model in models)
            {
                dictModels.Add(model.viewElementName, model);
            }

            foreach (DataControlField gridColumn in grid.Columns)
            {
                if ((gridColumn is TemplateField) && (((TemplateField)gridColumn).HeaderTemplate == null || !gridColumn.ShowHeader))
                {
                    //TemplateFields.ShowHeader = false, means that the GView does not control the column.
                    //If the column is visible, need to count the width of the column.
                    if (gridColumn.Visible)
                    {
                        if (gridColumn.HeaderStyle.Width.IsEmpty)
                        {
                            gridColumn.HeaderStyle.Width = Unit.Pixel(0);
                        }
                        else
                        {
                            totalColumnWidth += (int)gridColumn.HeaderStyle.Width.Value;
                        }
                    }

                    continue;
                }
                
                SimpleViewElementModel4WS model = GetRelatedModelByGridColumn(gridColumn, dictModels);
                AccelaTemplateField accelaTemplateField = gridColumn as AccelaTemplateField;
                
                if (model == null)
                {
                    if (accelaTemplateField != null && accelaTemplateField.HideField4Export == true)
                    {
                        gridColumn.HeaderStyle.CssClass = "ACA_Hide";
                        gridColumn.ItemStyle.CssClass = "ACA_Hide";
                    }
                    else
                    {
                        gridColumn.Visible = false;
                    }

                    continue;
                }

                bool isStatusVisible;
                    
                if (accelaTemplateField != null && accelaTemplateField.HideField)
                {
                    isStatusVisible = false;
                }
                else
                {
                    isStatusVisible = model.recStatus != ACAConstant.INVALID_STATUS;    
                }
               
                // set display order
                int displayOrder = !string.IsNullOrEmpty(model.displayOrder) ? int.Parse(model.displayOrder) : 0;

                if (maxDisplayOrder <= displayOrder && isStatusVisible)
                {
                    lastColumn = gridColumn;
                    maxDisplayOrder = displayOrder;
                }

                // set visible and width
                gridColumn.Visible = isStatusVisible || AppSession.IsAdmin;

                if (isStatusVisible || AppSession.IsAdmin)
                {
                    if (model.elementWidth > 0)
                    {
                        gridColumn.HeaderStyle.Width = Unit.Pixel(model.elementWidth);
                        gridColumn.ItemStyle.Width = Unit.Pixel(model.elementWidth);
                    }

                    if (isStatusVisible)
                    {
                        totalColumnWidth += (int)gridColumn.HeaderStyle.Width.Value;
                    }
                }
            }

            // set the column width
            int defaultWidth = grid.Width.IsEmpty ? AccelaGridView.DEFAULT_GRIDVIEW_WIDTH : (int)grid.Width.Value;

            grid.ScrollWidth = defaultWidth;

            if (totalColumnWidth > defaultWidth)
            {
                grid.Width = Unit.Pixel(totalColumnWidth);
            }
            else
            {
                grid.RealWidth = defaultWidth;

                var checkboxWidth = grid.AutoGenerateCheckBoxColumn ? 20 : 0;
                var radioColumnWidth = grid.AutoGenerateRadioButtonColumn ? 20 : 0;

                //the width should be total sorted column width and not include the sorted by GVElement last column width.
                if (lastColumn != null)
                {
                    int excludeLastColumnWidth = totalColumnWidth - Convert.ToInt32(lastColumn.HeaderStyle.Width.Value);
                    int lastColumnWidth = defaultWidth - excludeLastColumnWidth - checkboxWidth - radioColumnWidth;

                    lastColumn.HeaderStyle.Width = Unit.Pixel(lastColumnWidth);
                    lastColumn.ItemStyle.Width = Unit.Pixel(lastColumnWidth);
                }
            }

            // set the grid columns visible
            if (string.IsNullOrEmpty(gridColumnsVisible) && AppSession.IsAdmin)
            {
                gridColumnsVisible = GetGridColumnsVisible(grid, models, defaultWidth);
            }

            grid.GridColumnsVisible = gridColumnsVisible;
        }

        /// <summary>
        /// Gets the string of the grid columns visible.
        /// </summary>
        /// <param name="grid">The grid view.</param>
        /// <param name="models">The SimpleViewElementModel.</param>
        /// <param name="defaultTableWidth">The grid view table's default width.</param>
        /// <returns> Return the string of the grid column visible.</returns>
        private static string GetGridColumnsVisible(AccelaGridView grid, SimpleViewElementModel4WS[] models, int? defaultTableWidth = null)
        {
            if (!AppSession.IsAdmin)
            {
                return string.Empty;
            }

            GridViewJson json = new GridViewJson();
            json.IsAdmin = true;

            // get the grid view's column dictionary<AttributeName, Column>
            Dictionary<string, DataControlField> dictGridColumn = GetGridColumns2Dictionray(grid);

            foreach (SimpleViewElementModel4WS model in models)
            {
                bool isStatusVisible = model.recStatus != ACAConstant.INVALID_STATUS;
                string viewElementName = string.Empty;
                int elementWidth = 0;

                // standard column
                if (!ValidationUtil.IsNo(model.standard))
                {
                    if (!dictGridColumn.ContainsKey(model.viewElementName))
                    {
                        continue;
                    }

                    DataControlField gridColumn = dictGridColumn[model.viewElementName];

                    if (gridColumn == null || !gridColumn.ShowHeader)
                    {
                        continue;
                    }

                    elementWidth = isStatusVisible ? (int)gridColumn.HeaderStyle.Width.Value : model.elementWidth;
                    viewElementName = model.viewElementName;
                }
                else
                {
                    if (isStatusVisible && model.elementWidth <= 0)
                    {
                        elementWidth = AccelaGridView.DEFAULT_GRIDVIEW_TEMPLATE_COLUMN_WIDTH;
                    }
                    else
                    {
                        elementWidth = model.elementWidth;
                    }

                    viewElementName = TemplateUtil.GetTemplateControlID(model.viewElementName);
                }

                ConsructVisibleString(json, viewElementName, isStatusVisible, elementWidth);
            }

            // set the grid's width
            json.DefaultTableWidth = defaultTableWidth == null ? AccelaGridView.DEFAULT_GRIDVIEW_WIDTH : defaultTableWidth.Value;

            return SerializationUtil.ToJSON(json);
        }

        /// <summary>
        /// Gets the grid view's column dictionary.
        /// format: key = AttributeName, value = grid view's column
        /// </summary>
        /// <param name="grid">The grid view.</param>
        /// <returns>Return the grid view's column dictionary.</returns>
        private static Dictionary<string, DataControlField> GetGridColumns2Dictionray(AccelaGridView grid)
        {
            Dictionary<string, DataControlField> result = new Dictionary<string, DataControlField>();

            if (grid == null || grid.Columns.Count == 0)
            {
                return result;
            }

            foreach (DataControlField column in grid.Columns)
            {
                if (column is AccelaTemplateField)
                {
                    string key = ((AccelaTemplateField)column).AttributeName;

                    if (!string.IsNullOrEmpty(key))
                    {
                        result.Add(key, column);
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// Sets the grid column properties by SimpleViewElementModel.
        /// </summary>
        /// <param name="grid">The grid view.</param>
        /// <param name="models">The SimpleViewElementModel.</param>
        private static void SetGridColumnPropertiesBySimpleViewElement(AccelaGridView grid, SimpleViewElementModel4WS[] models)
        {
            if (models == null || models.Length == 0)
            {
                return;
            }

            SimpleViewElementModel4WS[] lstModel = models.OrderBy(m => !ValidationUtil.IsInt(m.displayOrder) ? 0 : Convert.ToInt32(m.displayOrder)).ToArray();
            List<ColumnProperty> columnProperties = new List<ColumnProperty>();

            foreach (SimpleViewElementModel4WS item in lstModel)
            {
                if (!string.IsNullOrEmpty(item.viewElementName))
                {
                    ColumnProperty cp = new ColumnProperty();
                    cp.ElementName = !ValidationUtil.IsNo(item.standard) ? item.viewElementName : TemplateUtil.GetTemplateControlID(item.viewElementName);
                    cp.DisplayOrder = item.displayOrder;
                    cp.Visible = item.recStatus == null ? ACAConstant.INVALID_STATUS : item.recStatus;

                    columnProperties.Add(cp);
                }
            }

            grid.ColumnProperties = columnProperties.ToArray();
        }

        /// <summary>
        /// Construct json string
        /// </summary>
        /// <param name="json">GridViewJson object.</param>
        /// <param name="name">string name</param>
        /// <param name="visible">true if it's visible.</param>
        /// <param name="width">string width.</param>
        private static void ConsructVisibleString(GridViewJson json, string name, bool visible, int width)
        {
            GridViewColumnVisible column = new GridViewColumnVisible();
            column.Name = name;
            column.Visible = visible;
            column.Width = width.ToString();
            column.FixWidth = width.ToString();

            json.GridViewColumnVisibleList.Add(column);
        }

        /// <summary>
        /// Gets the related model by grid view column.
        /// </summary>
        /// <param name="gridColumn">The grid view column.</param>
        /// <param name="dictModels">The models that format of Dictionary, key: ViewElementName, value: Model.</param>
        /// <returns>Return the related model, if cannot find, return null.</returns>
        private static SimpleViewElementModel4WS GetRelatedModelByGridColumn(DataControlField gridColumn, Dictionary<string, SimpleViewElementModel4WS> dictModels)
        {
            AccelaTemplateField gridColumnTemplateField = gridColumn as AccelaTemplateField;

            if (gridColumnTemplateField == null)
            {
                return null;
            }

            // get the model from standard grid column.
            string attributeName = gridColumnTemplateField.AttributeName;
            if (gridColumnTemplateField.IsStandard && !string.IsNullOrEmpty(attributeName) && dictModels.ContainsKey(attributeName))
            {
                return dictModels[attributeName];
            }

            // get the model from the template grid column.
            IList<string> mergedAttributeNames = gridColumnTemplateField.MergedAttributeNames;
            if (!gridColumnTemplateField.IsStandard && mergedAttributeNames != null && mergedAttributeNames.Count > 0)
            {
                foreach (string viewElementName in gridColumnTemplateField.MergedAttributeNames)
                {
                    if (dictModels.ContainsKey(viewElementName))
                    {
                        return dictModels[viewElementName];
                    }
                }
            }

            return null;
        }

        /// <summary>
        /// Gets the download data source.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="delegateGetDataSource">The delegate get data source.</param>
        /// <returns>return the download data source.</returns>
        private static DataTable GetDownloadDataSource(object sender, DelegateGetDataSource delegateGetDataSource)
        {
            DataTable result = null;
            int startRow = 1;
            int endRow = ACAConstant.GRIDVIEW_FULL_DOWNLOAD_EACH_NUM;
            QueryFormat queryFormat = new QueryFormat();

            // loop get the data source, each time get one batch records.
            while (true)
            {
                // get the records by query format
                queryFormat.startRow = startRow;
                queryFormat.endRow = endRow;

                DownloadResultModel downloadResultModel = delegateGetDataSource(queryFormat);
                DataTable dataSourcePiece = downloadResultModel.DataSource;

                // merge the records
                if (result == null)
                {
                    result = dataSourcePiece;
                }
                else if (dataSourcePiece != null && dataSourcePiece.Rows.Count > 0)
                {
                    result.Merge(dataSourcePiece);
                }

                if (dataSourcePiece == null || dataSourcePiece.Rows.Count < ACAConstant.GRIDVIEW_FULL_DOWNLOAD_EACH_NUM)
                {
                    break;
                }

                if (downloadResultModel.StartDBRow == 0)
                {
                    startRow += ACAConstant.GRIDVIEW_FULL_DOWNLOAD_EACH_NUM;
                }
                else
                {
                    startRow = downloadResultModel.StartDBRow;
                }

                endRow = startRow + ACAConstant.GRIDVIEW_FULL_DOWNLOAD_EACH_NUM - 1;
            }

            // sort the data source
            string sortExpression = string.Empty;
            string sortDirection = string.Empty;

            if (sender is AccelaGridView)
            {
                AccelaGridView gdv = sender as AccelaGridView;
                sortExpression = gdv.GridViewSortExpression;
                sortDirection = gdv.GridViewSortDirection;
            }

            if (!string.IsNullOrEmpty(sortExpression) && !string.IsNullOrEmpty(sortDirection))
            {
                DataView dv = new DataView(result);
                dv.Sort = string.Format("{0} {1}", sortExpression, sortDirection);
                result = dv.ToTable();
            }

            return result;
        }

        /// <summary>
        /// Gets the exported content.
        /// </summary>
        /// <param name="dataSource">The data source.</param>
        /// <param name="args">The <see cref="Accela.Web.Controls.GridViewDownloadEventArgs"/> instance containing the event data.</param>
        /// <param name="delegateExportFormat">The delegate export format.</param>
        /// <returns>Return the exported content.</returns>
        private static string GetExportedContent(DataTable dataSource, GridViewDownloadEventArgs args, DelegateExportFormat delegateExportFormat)
        {
            if (args == null || args.ExportParameters == null)
            {
                return null;
            }

            StringBuilder sbExportContent = new StringBuilder();
            List<ExportParameter> exportParameters = args.ExportParameters;
            List<string> visibleColumn = new List<string>();

            // 1. set the header content for download
            foreach (ExportParameter exportParameter in exportParameters)
            {
                sbExportContent.Append(exportParameter.Header);
                sbExportContent.Append(ACAConstant.CultureInfoSplitChar);

                visibleColumn.Add(exportParameter.ExportDataField);
            }

            sbExportContent.Append("\r\n");

            // 2. set the data item content for download
            foreach (DataRow dataRow in dataSource.Rows)
            {
                // format the export content if need
                Dictionary<string, string> dictFormat = null;
                if (delegateExportFormat != null)
                {
                    dictFormat = delegateExportFormat(dataRow, visibleColumn);
                }

                // convert the data item content to CSV field format
                foreach (ExportParameter exportParameter in exportParameters)
                {
                    string columnName = exportParameter.ExportDataField;

                    if (string.IsNullOrEmpty(columnName))
                    {
                        continue;
                    }

                    object objColumnValue = dataRow[columnName];
                    if (objColumnValue == null)
                    {
                        sbExportContent.Append(ACAConstant.CultureInfoSplitChar);
                        continue;
                    }

                    // external format for the data source
                    string columnValue = objColumnValue.ToString();
                    if (dictFormat != null && dictFormat.ContainsKey(columnName))
                    {
                        columnValue = dictFormat[columnName] ?? string.Empty;
                    }

                    // continue loop when it is empty string
                    if (string.IsNullOrEmpty(columnValue))
                    {
                        sbExportContent.Append(ACAConstant.CultureInfoSplitChar);
                        continue;
                    }

                    // external format using the parameter 'ExportFormat'
                    switch (exportParameter.ExportFormat)
                    {
                        case ExportFormats.ShortDate:
                            DateTime date = DateTime.MinValue;
                            I18nDateTimeUtil.TryParseFromUI(columnValue, out date);

                            columnValue = date != DateTime.MinValue ? I18nDateTimeUtil.FormatToDateStringForUI(date) : string.Empty;
                            break;
                        case ExportFormats.OnlyTime:
                            DateTime dateTime = DateTime.MinValue;
                            I18nDateTimeUtil.TryParseFromUI(columnValue, out dateTime);

                            columnValue = dateTime != DateTime.MinValue ? I18nDateTimeUtil.FormatToTimeStringForUI(dateTime, false) : string.Empty;
                            break;
                        case ExportFormats.SSN:
                            columnValue = MaskUtil.FormatSSNShow(columnValue);
                            break;
                        case ExportFormats.FEIN:
                            columnValue = MaskUtil.FormatFEINShow(columnValue, StandardChoiceUtil.IsEnableFeinMasking());
                            break;
                        case ExportFormats.FilterScript:
                            columnValue = ScriptFilter.FilterScript(columnValue);
                            break;
                        default:
                            break;
                    }
                        
                    // format to the CSV file
                    string formattedCSVValue = ScriptFilter.FormatCSVContent(columnValue, true);

                    sbExportContent.Append(formattedCSVValue);
                    sbExportContent.Append(ACAConstant.CultureInfoSplitChar);
                }

                sbExportContent.Append("\r\n");
            }

            // 3. return the content
            return sbExportContent.ToString();
        }

        /// <summary>
        /// Get Contact type
        /// </summary>
        /// <param name="contactTypeSource">the contact type source.</param>
        /// <returns>contact type list.</returns>
        private static List<string> GetContactTypes(string contactTypeSource)
        {
            List<string> contactTypes = new List<string>();
            IBizDomainBll standChoiceBll = ObjectFactory.GetObject<IBizDomainBll>();
            IList<ItemValue> stdItems = standChoiceBll.GetContactTypeList(ConfigManager.AgencyCode, false, contactTypeSource);

            if (stdItems != null && stdItems.Count > 0)
            {
                foreach (ItemValue itemValue in stdItems)
                {
                    contactTypes.Add(itemValue.Key);
                }
            }

            return contactTypes;
        }

        #endregion Private Methods
    }
}
