#region Header

/**
* <pre>
*  Accela Citizen Access
*  File: UIUtil.cs
*
*  Accela, Inc.
*  Copyright (C): 2011
*
*  Description: UI models convertion utility.
*
*  Notes:
* $Id: UIUtil.cs 171222 2010-04-21 16:10:00Z ACHIEVO\alan.hu $.
*  Revision History
*  Date,            Who,        What
*  Jul 6, 2011      Alan Hu     Initial.
* </pre>
*/

#endregion

using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

using Accela.ACA.BLL.Cap;
using Accela.ACA.Common;
using Accela.ACA.Common.Util;
using Accela.ACA.UI;
using Accela.ACA.Web.Common;
using Accela.ACA.Web.Common.Control;
using Accela.ACA.WSProxy;

namespace Accela.ACA.Web.Util
{
    /// <summary>
    /// A enumeration to separate the data type in the data container.
    /// </summary>
    public enum UIDataType
    {
        /// <summary>
        /// UI data for ASI.
        /// </summary>
        ASI,

        /// <summary>
        /// UI data for ASIT.
        /// </summary>
        ASIT,

        /// <summary>
        /// Copy of UI data for ASIT.
        /// </summary>
        ASITCopy,
        
        /// <summary>
        /// Edit UI data for ASIT.
        /// </summary>
        ASITEdit,

        /// <summary>
        /// UI data for Contact.
        /// </summary>
        Contact,

        /// <summary>
        /// UI data for Licensed Professional.
        /// </summary>
        LP
    }

    /// <summary>
    /// UI models convertion utility
    /// </summary>
    public static class UIUtil
    {
        /// <summary>
        /// Convert to ASI tables to ASIT UI tables.
        /// </summary>
        /// <param name="asiTables">Array of ASI table object.</param>
        /// <param name="moduleName">Module name.</param>
        /// <returns>Array of ASIT UI table.</returns>
        public static ASITUITable[] ConvertASITablesToUITables(AppSpecificTableModel4WS[] asiTables, string moduleName)
        {
            if (asiTables == null)
            {
                return null;
            }

            IList<ASITUITable> asitUIs = new List<ASITUITable>();
            ASISecurityUtil securityUtil = new ASISecurityUtil();

            foreach (AppSpecificTableModel4WS asiTable in asiTables)
            {
                if (asiTable == null)
                {
                    continue;
                }

                string tableSecurity = securityUtil.GetASITSecurity(asiTable, moduleName);

                if (ValidationUtil.IsNo(asiTable.vchDispFlag) ||
                    ACAConstant.ASISecurity.None.Equals(tableSecurity) ||
                    securityUtil.IsAllFieldsNoAccess(asiTable, moduleName))
                {
                    continue;
                }

                ASITUITable asitUI = new ASITUITable();
                asitUI.AgencyCode = asiTable.columns[0].servProvCode;
                asitUI.GroupName = asiTable.groupName;
                asitUI.TableName = asiTable.tableName;
                asitUI.TableTitle = I18nStringUtil.GetString(asiTable.resTableName, asiTable.tableName);
                asitUI.Instruction = I18nStringUtil.GetString(asiTable.resInstruction, asiTable.instruction);
                asitUI.CapID = new CapIDModel4WS
                                    {
                                        serviceProviderCode = asiTable.columns[0].servProvCode,
                                        id1 = asiTable.columns[0].b1PerId1,
                                        id2 = asiTable.columns[0].b1PerId2,
                                        id3 = asiTable.columns[0].b1PerId3
                                    };

                if (ACAConstant.ASISecurity.Read.Equals(tableSecurity, StringComparison.OrdinalIgnoreCase))
                {
                    asitUI.IsReadOnly = true;
                }

                asitUI.TemplateRow = new UIRow();

                //Create tempate row.
                for (int i = 0; i < asiTable.defaultField.Length; i++)
                {
                    AppSpecificTableField4WS defaultField = asiTable.defaultField[i];
                    AppSpecificTableColumnModel4WS asitColumn = asiTable.columns[i];
                    string columnSecurity = securityUtil.GetASITSecurity(asitColumn, moduleName);
                    bool isHidden = ValidationUtil.NeedHideColumn4ASIT(asitColumn) || ACAConstant.ASISecurity.None.Equals(columnSecurity);

                    ASITUIField field = new ASITUIField();
                    field.Name = asitColumn.columnName;
                    field.Label = I18nStringUtil.GetString(asitColumn.resColumnName, asitColumn.columnName);
                    field.Type = asitColumn.columnType;
                    field.DefaultValue = I18nStringUtil.GetString(asitColumn.resDefaultValue, asitColumn.defaultValue);
                    field.Value = field.DefaultValue;
                    field.IsHidden = isHidden;
                    field.IsReadOnly = ACAConstant.ASISecurity.Read.Equals(columnSecurity);
                    field.IsRequired = ValidationUtil.IsYes(asitColumn.requiredFlag);
                    field.MaxLength = int.Parse(asitColumn.maxLength);
                    field.Instruction = I18nStringUtil.GetCurrentLanguageString(asitColumn.resInstruction, asitColumn.instruction);
                    field.Watermark = I18nStringUtil.GetCurrentLanguageString(asitColumn.resWaterMark, asitColumn.waterMark);
                    field.DisplayLength = int.Parse(asitColumn.displayLength);
                    field.RequiredFeeCalc = asitColumn.requiredFeeCalc;
                    field.SecurityValue = columnSecurity;
                    field.ValueList = asitColumn.valueList;

                    asitUI.TemplateRow.Fields.Add(field);
                }

                asitUIs.Add(asitUI);
                string rowIdx = string.Empty;
                UIRow currentRow = null;
                int colIdx = 0;

                if (asiTable.rowIndex == null)
                {
                    continue;
                }

                //Create data row.
                for (int i = 0; i < asiTable.rowIndex.Length; i++)
                {
                    if (!rowIdx.Equals(asiTable.rowIndex[i]))
                    {
                        rowIdx = asiTable.rowIndex[i];
                        colIdx = 0;
                        currentRow = new UIRow();
                        currentRow.RowIndex = Math.Abs(Convert.ToInt32(rowIdx));
                        asitUI.Rows.Add(currentRow);
                    }

                    ASITUIField defaultField = asitUI.TemplateRow.Fields[colIdx] as ASITUIField;
                    ASITUIField field = ObjectCloneUtil.DeepCopy(defaultField);
                    field.FieldID = ControlBuildHelper.GetASITFieldID(asitUI.AgencyCode, asitUI.GroupName, asitUI.TableName, currentRow.RowIndex, colIdx);
                    field.Value = asiTable.tableField[i].inputValue;
                    field.IsDrillDown = asiTable.tableField[i].drillDown;

                    currentRow.Fields.Add(field);
                    colIdx++;
                }
            }

            return asitUIs.ToArray();
        }

        /// <summary>
        /// Converts the ASI table to data table.
        /// </summary>
        /// <param name="asiTable">The asi table.</param>
        /// <returns>the data table converted from ASI table.</returns>
        public static DataTable ConvertASITableToDataTable(ASITUITable asiTable)
        {
            if (asiTable == null)
            {
                return null;
            }

            DataTable result = new DataTable();
            var asitFieldsWithResValues = GetASITFieldsWithResValues(asiTable);

            foreach (ASITUIField defaultField in asiTable.TemplateRow.Fields)
            {
                if (!defaultField.IsHidden)
                {
                    result.Columns.Add(defaultField.Label);
                }
            }

            result.Columns.Add(ASITUITable.RowIndexColumnName, typeof(int));

            if (asiTable.Rows != null && asiTable.Rows.Count != 0)
            {
                foreach (UIRow row in asiTable.Rows)
                {
                    DataRow currentRow = result.NewRow();
                    result.Rows.Add(currentRow);
                    currentRow[ASITUITable.RowIndexColumnName] = row.RowIndex;

                    foreach (ASITUIField field in row.Fields)
                    {
                        if (!field.IsHidden && !field.IsHiddenByExp)
                        {
                            string resFieldValue = GetASITFieldResValue(field, asitFieldsWithResValues);
                            currentRow[field.Label] = !String.IsNullOrEmpty(resFieldValue) ? resFieldValue : field.Value;
                        }
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// Create new row for ASIT UI table.
        /// </summary>
        /// <param name="sTable">Source table.</param>
        /// <param name="rowQty">Quantity of new rows.</param>
        /// <returns>A new UI Table contains the new rows.</returns>
        public static ASITUITable CreateNewRow4ASITUITable(ASITUITable sTable, int rowQty)
        {
            if (sTable != null && rowQty > 0)
            {
                ASITUITable dTable = ObjectCloneUtil.DeepCopy(sTable);
                List<UIRow> newRows = new List<UIRow>();
                int curRowIdx = 0;

                if (dTable.Rows != null && dTable.Rows.Count > 0)
                {
                    curRowIdx = dTable.Rows.Max(r => r.RowIndex) + 1;
                }

                for (int i = curRowIdx; i < curRowIdx + rowQty; i++)
                {
                    UIRow newRow = ObjectCloneUtil.DeepCopy(dTable.TemplateRow);
                    newRow.RowIndex = i;

                    for (int j = 0; j < newRow.Fields.Count; j++)
                    {
                        newRow.Fields[j].FieldID = ControlBuildHelper.GetASITFieldID(dTable.AgencyCode, dTable.GroupName, dTable.TableName, i, j);
                    }

                    newRows.Add(newRow);
                }

                dTable.Rows = newRows;

                return dTable;
            }

            return null;
        }

        /// <summary>
        /// Clear the UI date from UI container.
        /// </summary>
        public static void ClearUIData()
        {
            HttpContext.Current.Session[SessionConstant.SESSION_UI_DATA] = null;
        }

        /// <summary>
        /// Get UI data from UI container by UI data type.
        /// </summary>
        /// <param name="dataType">UI data type.</param>
        /// <returns>A dictionary contains the data keys and data tables.</returns>
        public static Dictionary<string, UITable[]> GetDataFromUIContainer(UIDataType dataType)
        {
            Hashtable uiData = HttpContext.Current.Session[SessionConstant.SESSION_UI_DATA] as Hashtable;

            if (uiData == null || !uiData.ContainsKey(dataType))
            {
                return null;
            }

            return uiData[dataType] as Dictionary<string, UITable[]>;
        }

        /// <summary>
        /// Get UI data from UI container.
        /// </summary>
        /// <param name="dataType">UI data type.</param>
        /// <param name="dataKey">UI data key.</param>
        /// <returns>Array of UI table.</returns>
        public static UITable[] GetDataFromUIContainer(UIDataType dataType, string[] dataKey)
        {
            Dictionary<string, UITable[]> uiTableList = GetDataFromUIContainer(dataType);

            if (uiTableList == null)
            {
                return null;
            }

            if (dataKey != null)
            {
                uiTableList = uiTableList.Where(v => dataKey.Contains(v.Key)).ToDictionary(v => v.Key, v => v.Value);
            }

            if (uiTableList.Count > 0)
            {
                switch (dataType)
                {
                    case UIDataType.ASIT:
                    case UIDataType.ASITCopy:
                    case UIDataType.ASITEdit:
                        List<ASITUITable> asitUITables = new List<ASITUITable>();

                        foreach (ASITUITable[] tables in uiTableList.Values)
                        {
                            if (tables != null)
                            {
                                asitUITables.AddRange(tables);
                            }
                        }

                        return asitUITables.ToArray();
                    default:
                        List<UITable> uiTables = new List<UITable>();

                        foreach (UITable[] tables in uiTableList.Values)
                        {
                            if (tables != null)
                            {
                                uiTables.AddRange(tables);
                            }
                        }

                        return uiTables.ToArray();
                }
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Gets the ASIT fields with current language values.
        /// </summary>
        /// <param name="appInfoTable">The app info table.</param>
        /// <returns>
        /// the ASIT fields with current language values.
        /// </returns>
        public static List<ASITUIField> GetASITFieldsWithResValues(ASITUITable appInfoTable)
        {
            List<ASITUIField> result = null;

            var drillDownFields = (from asit in appInfoTable.Rows
                                   where asit != null
                                   from field in asit.Fields
                                   let asitUIField = field as ASITUIField
                                   where field != null
                                   && asitUIField != null
                                   && asitUIField.IsDrillDown
                                   select asitUIField).ToList();
            var drillDownFieldValueArray = (from field in drillDownFields
                                            select field.Value).ToArray();

            var asitDrillDownBLL = ObjectFactory.GetObject<IASITableDrillDownBLL>();
            string[] drillDownFieldValueResArray = drillDownFieldValueArray.Count() == 0 ? null : asitDrillDownBLL.GetFieldTexts(appInfoTable.AgencyCode, drillDownFieldValueArray);

            if (drillDownFieldValueResArray != null && drillDownFieldValueResArray.Length == drillDownFields.Count())
            {
                for (int i = 0; i < drillDownFieldValueResArray.Length; i++)
                {
                    drillDownFields[i].Value = drillDownFieldValueResArray[i];
                }

                result = drillDownFields;
            }

            return result;
        }

        /// <summary>
        /// Set the UI data to UI container.
        /// </summary>
        /// <param name="uiTableList">A dictionary contains the data keys and data tables.</param>
        /// <param name="dataType">UI data type.</param>
        public static void SetDataToUIContainer(Dictionary<string, UITable[]> uiTableList, UIDataType dataType)
        {
            Hashtable uiData = HttpContext.Current.Session[SessionConstant.SESSION_UI_DATA] as Hashtable;

            if (uiData == null)
            {
                uiData = new Hashtable();
            }

            if (!uiData.ContainsKey(dataType))
            {
                uiData.Add(dataType, uiTableList);
            }
            else
            {
                uiData[dataType] = uiTableList;
            }

            HttpContext.Current.Session[SessionConstant.SESSION_UI_DATA] = uiData;
        }

        /// <summary>
        /// Set UI data to UI container.
        /// </summary>
        /// <param name="uiTables">Array of UI table.</param>
        /// <param name="dataType">UI data type.</param>
        /// <param name="dataKey">UI data key.</param>
        public static void SetDataToUIContainer(UITable[] uiTables, UIDataType dataType, string dataKey)
        {
            Dictionary<string, UITable[]> uiTableList = GetDataFromUIContainer(dataType);
            dataKey = String.IsNullOrEmpty(dataKey) ? String.Empty : dataKey;

            if (uiTableList == null)
            {
                uiTableList = new Dictionary<string, UITable[]>();
            }

            if (uiTableList.ContainsKey(dataKey))
            {
                uiTableList[dataKey] = uiTables;
            }
            else
            {
                uiTableList.Add(dataKey, uiTables);
            }

            SetDataToUIContainer(uiTableList, dataType);
        }

        /// <summary>
        /// Sync the ui rows from the source table to the destination table.
        /// The destination table and the source table must have the same structure.
        /// </summary>
        /// <param name="sourTables">Source tables.</param>
        /// <param name="destTables">Destination tables.</param>
        public static void SyncASITUIRowData(ASITUITable[] sourTables, ASITUITable[] destTables)
        {
            if (sourTables == null || destTables == null)
            {
                return;
            }

            for (int i = 0; i < destTables.Length; i++)
            {
                ASITUITable dTable = destTables[i];
                ASITUITable sTable = sourTables.SingleOrDefault(dt =>
                    dt.AgencyCode == dTable.AgencyCode &&
                    dt.GroupName == dTable.GroupName &&
                    dt.TableName == dTable.TableName);

                if (sTable == null)
                {
                    continue;
                }

                IList<int> updatedRowIdxes = new List<int>();

                for (int j = 0; j < dTable.Rows.Count; j++)
                {
                    //Update existing rows.
                    UIRow dRow = dTable.Rows[j];
                    UIRow sRow = sTable.Rows.SingleOrDefault(dr =>
                            dr.RowIndex == dRow.RowIndex);

                    if (sRow != null)
                    {
                        //Update
                        dTable.Rows[j] = ObjectCloneUtil.DeepCopy(sRow);
                        updatedRowIdxes.Add(sRow.RowIndex);
                    }
                }

                var newRows = sTable.Rows.Where(r => !updatedRowIdxes.Contains(r.RowIndex));

                //Add new rows.
                if (newRows.Count() > 0)
                {
                    dTable.Rows.AddRange(newRows);
                    dTable.Rows = dTable.Rows.OrderBy(r => r.RowIndex).ToList();
                }
            }
        }

        /// <summary>
        /// Sync the input values from UI data table to ASI Table biz models.
        /// </summary>
        /// <param name="sourTables">ASIT UI data tables.</param>
        /// <param name="destTables">ASI Table biz models.</param>
        public static void SyncInputValueToASITBizModel(ASITUITable[] sourTables, AppSpecificTableModel4WS[] destTables)
        {
            if (sourTables == null || destTables == null)
            {
                return;
            }

            for (int i = 0; i < destTables.Length; i++)
            {
                AppSpecificTableModel4WS dTable = destTables[i];

                if (dTable == null)
                {
                    continue;
                }

                ASITUITable sTable = sourTables.SingleOrDefault(dt =>
                    dt.AgencyCode == dTable.columns[0].servProvCode &&
                    dt.GroupName == dTable.groupName &&
                    dt.TableName == dTable.tableName);

                if (sTable == null)
                {
                    continue;
                }

                IList<long> updatedRowIdxes = new List<long>();

                //Update existing rows.
                if (dTable.tableField != null && dTable.tableField.Length > 0)
                {
                    for (int j = 0; j < dTable.tableField.Length; j++)
                    {
                        AppSpecificTableField4WS dField = dTable.tableField[j];
                        UIRow sRow = sTable.Rows.Where(r => r.RowIndex == Math.Abs(dField.rowIndex) - 1).SingleOrDefault();

                        if (sRow != null)
                        {
                            ASITUIField sField = sRow.Fields.Where(f => f.Name == dField.fieldLabel).SingleOrDefault() as ASITUIField;

                            if (sField != null)
                            {
                                dTable.tableField[j].inputValue = sField.Value;
                            }

                            updatedRowIdxes.Add(sRow.RowIndex);
                        }
                    }
                }

                //Split the added rows and deleted rows.
                var addedRows = sTable.Rows.Where(r => !updatedRowIdxes.Contains(r.RowIndex));
                var addedRowIdxes = addedRows.Select(r => (long)r.RowIndex);

                //Update deleted rows.
                if (dTable.tableField != null && dTable.tableField.Length > 0)
                {
                    var existingRowIdxes = dTable.tableField.Select(fld => Math.Abs(fld.rowIndex) - 1);
                    var deletedRowIdxes = dTable.tableField.Where(f =>
                        !updatedRowIdxes.Contains(Math.Abs(f.rowIndex) - 1) &&
                        !addedRowIdxes.Contains(Math.Abs(f.rowIndex) - 1)).Select(f => f.rowIndex);

                    if (deletedRowIdxes.Count() > 0)
                    {
                        var remainedFields = dTable.tableField.Where(f => !deletedRowIdxes.Contains(f.rowIndex));
                        dTable.tableField = remainedFields.ToArray();
                    }
                }

                int newRowsQty = addedRows.Count();

                //Update added rows.
                if (newRowsQty > 0)
                {
                    int colsQty = dTable.columns.Length;
                    int existRowsQty = dTable.tableField != null ? dTable.tableField.Length / colsQty : 0;
                    int allRowsQty = existRowsQty + newRowsQty;
                    int allFieldsQty = allRowsQty * colsQty;
                    long curRowIdx = 1;
                    int fieldIdx = 0;
                    AppSpecificTableField4WS[] allFields = new AppSpecificTableField4WS[allFieldsQty];

                    //Copy existing fields.
                    if (dTable.tableField != null && dTable.tableField.Length > 0)
                    {
                        Array.Copy(dTable.tableField, allFields, dTable.tableField.Length);
                        curRowIdx = dTable.tableField.Max(fld => Math.Abs(fld.rowIndex)) + 1;
                        fieldIdx = dTable.tableField.Length;
                    }

                    for (int j = 0; j < newRowsQty; j++)
                    {
                        UIRow sRow = addedRows.ElementAt(j);

                        for (int k = 0; k < colsQty; k++)
                        {
                            ASITUIField sField = sRow.Fields.ElementAtOrDefault(k) as ASITUIField;

                            if (sField != null && dTable.columns[k].columnName == sField.Name)
                            {
                                allFields[fieldIdx] = ObjectCloneUtil.DeepCopy(dTable.defaultField[k]);
                                allFields[fieldIdx].inputValue = sField.Value;
                                allFields[fieldIdx].drillDown = sField.IsDrillDown;

                                //Field's rowIndex is started from 1.
                                //And for new row - the row index must less than 0.
                                allFields[fieldIdx].rowIndex = -(curRowIdx + j);
                            }

                            fieldIdx++;
                        }
                    }

                    dTable.tableField = allFields;
                }

                if (dTable.tableField != null && dTable.tableField.Length > 0 && sTable.Rows != null && sTable.Rows.Count > 0)
                {
                    var indexes = from fld in dTable.tableField
                                  select (-(Math.Abs(fld.rowIndex) - 1)).ToString();
                    dTable.rowIndex = indexes.ToArray();
                    var values = from fld in dTable.tableField
                                 select fld.inputValue;
                    dTable.tableFieldValues = values.ToArray();
                }
                else
                {
                    dTable.tableField = null;
                    dTable.rowIndex = null;
                    dTable.tableFieldValues = null;
                }
            }
        }

        /// <summary>
        /// Gets the ASIT field res value.
        /// </summary>
        /// <param name="currentField">The current field.</param>
        /// <param name="asitFieldsWithResValues">The asit fields with res values.</param>
        /// <returns>
        /// the ASIT field res value.
        /// </returns>
        private static string GetASITFieldResValue(ASITUIField currentField, List<ASITUIField> asitFieldsWithResValues)
        {
            string result = String.Empty;

            if (currentField != null)
            {
                string currentFieldValue = !String.IsNullOrEmpty(currentField.Value) ? currentField.Value : String.Empty;

                if ((int)FieldType.HTML_SELECTBOX == int.Parse(currentField.Type) && currentField.ValueList != null && currentField.ValueList.Count() > 0)
                {
                    var matchedResItem = currentField.ValueList.SingleOrDefault(p => currentFieldValue.Equals(p.attrValue, StringComparison.OrdinalIgnoreCase));
                    result = matchedResItem != null && !String.IsNullOrEmpty(matchedResItem.resAttrValue) ? matchedResItem.resAttrValue : String.Empty;
                }
                else if ((int)FieldType.HTML_CHECKBOX == int.Parse(currentField.Type) || (int)FieldType.HTML_RADIOBOX == int.Parse(currentField.Type))
                {
                    result = ModelUIFormat.FormatYNLabel(currentFieldValue);
                }
                else if ((int)FieldType.HTML_TEXTBOX_OF_DATE == int.Parse(currentField.Type))
                {
                    result = I18nDateTimeUtil.ConvertDateStringFromWebServiceToUI(currentFieldValue);
                }
                else if (currentField.IsDrillDown)
                {
                    ASITUIField currentResField = asitFieldsWithResValues.SingleOrDefault(p => !String.IsNullOrEmpty(p.FieldID) && p.FieldID.Equals(currentField.FieldID, StringComparison.OrdinalIgnoreCase));
                    result = currentResField != null ? currentResField.Value : String.Empty;
                }
            }

            return result;
        }
    }
}