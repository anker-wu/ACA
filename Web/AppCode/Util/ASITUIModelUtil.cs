#region Header

/**
* <pre>
*  Accela Citizen Access
*  File: ASITUIModelUtil.cs
*
*  Accela, Inc.
*  Copyright (C): 2011-2014
*
*  Description: ASIT UI models convertion utility.
*
*  Notes:
* $Id: ASITUIModelUtil.cs 171222 2010-04-21 16:10:00Z ACHIEVO\alan.hu $.
*  Revision History
*  Date,            Who,        What
*  Aug 16, 2011      Alan Hu     Initial.
* </pre>
*/

#endregion

using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

using Accela.ACA.BLL.Cap;
using Accela.ACA.Common;
using Accela.ACA.Common.Util;
using Accela.ACA.UI.Model;
using Accela.ACA.Web.Common;
using Accela.ACA.Web.Common.Control;
using Accela.ACA.WSProxy;

namespace Accela.ACA.Web.Util
{
    /// <summary>
    /// ASIT UI models convert utility
    /// </summary>
    public static class ASITUIModelUtil
    {
        #region Fields

        /// <summary>
        /// Event target ID for handle asit PostBack behaviors.
        /// </summary>
        public const string ASIT_POSTEVENT_TARGET_ID = "$HANDLE_ASIT_POSTEVENT";

        /// <summary>
        /// Event target ID for sync asit UI copy data.
        /// </summary>
        public const string ASIT_POSTEVENT_SYNCUICOPYDATA = "$SYNC_ASIT_UICOPYDATA";

        #endregion

        /// <summary>
        /// Convert to ASI tables to ASIT UI tables.
        /// </summary>
        /// <param name="asiTables">Array of ASI table object.</param>
        /// <param name="moduleName">Module name.</param>
        /// <param name="isInSpearForm">Indicating whether the UI data is using in spear form.</param>
        /// <param name="sectionInfo">the UI table section info(Format:<c>title/findex</c>).</param>
        /// <param name="isASITSearchForm">is ASIT search form</param>
        /// <returns>Array of ASIT UI table.</returns>
        public static ASITUITable[] ConvertASITablesToUITables(AppSpecificTableModel4WS[] asiTables, string moduleName, bool isInSpearForm, string sectionInfo, bool isASITSearchForm = false)
        {
            if (asiTables == null)
            {
                return null;
            }

            IList<ASITUITable> asitUIs = new List<ASITUITable>();

            foreach (AppSpecificTableModel4WS asiTable in asiTables)
            {
                if (asiTable == null)
                {
                    continue;
                }

                string tableSecurity = ACAConstant.ASISecurity.Full;

                if (!isASITSearchForm)
                {
                    tableSecurity = ASISecurityUtil.GetASITSecurity(asiTable, moduleName);

                    if (ValidationUtil.IsNo(asiTable.vchDispFlag) ||
                        ACAConstant.ASISecurity.None.Equals(tableSecurity) ||
                        ASISecurityUtil.IsAllFieldsNoAccess(asiTable, moduleName))
                    {
                        continue;
                    }
                }

                //Fill the table info.
                ASITUITable asitUI = new ASITUITable();
                asitUI.AgencyCode = asiTable.columns[0].servProvCode;
                asitUI.GroupName = asiTable.groupName;
                asitUI.TableName = asiTable.tableName;

                if (asiTable.templateLayoutConfig != null)
                {
                    TemplateLayoutConfigModel templateConfig = asiTable.templateLayoutConfig;

                    string resButtonAddLabel = string.Empty;
                    string resButtonAddMoreLabel = string.Empty;
                    string resButtonEditLabel = string.Empty;
                    string resButtonDeleteLabel = string.Empty;
                    string resInstruction = string.Empty;
                    string resAlternativeLabel = string.Empty;

                    if (templateConfig.i18NModel != null)
                    {
                        TemplateLayoutConfigI18NModel i18NModel = templateConfig.i18NModel;

                        resButtonAddLabel = i18NModel.buttonAddRowLabel;
                        resButtonAddMoreLabel = i18NModel.buttonAddMoreLabel;
                        resButtonEditLabel = i18NModel.buttonEditRowLabel;
                        resButtonDeleteLabel = i18NModel.buttonDeleteRowLabel;
                        resInstruction = i18NModel.instruction;
                        resAlternativeLabel = i18NModel.alternativeLabel;
                    }

                    asitUI.ButtonAddLabel = I18nStringUtil.GetString(resButtonAddLabel, templateConfig.buttonAddRowLabel);
                    asitUI.ButtonAddMoreLabel = I18nStringUtil.GetString(resButtonAddMoreLabel, templateConfig.buttonAddMoreLabel);
                    asitUI.ButtonEditLabel = I18nStringUtil.GetString(resButtonEditLabel, templateConfig.buttonEditRowLabel);
                    asitUI.ButtonDeleteLabel = I18nStringUtil.GetString(resButtonDeleteLabel, templateConfig.buttonDeleteRowLabel);
                    asitUI.ButtonAddDisplay = templateConfig.buttonAddDisplay;
                    asitUI.ButtonEditDisplay = templateConfig.buttonEditDisplay;
                    asitUI.ButtonDeleteDisplay = templateConfig.buttonDeleteDisplay;
                    asitUI.Instruction = I18nStringUtil.GetString(resInstruction, templateConfig.instruction);
                    asitUI.AlternativeLabel = I18nStringUtil.GetString(resAlternativeLabel, templateConfig.alternativeLabel); 
                }

                if (!string.IsNullOrEmpty(sectionInfo))
                {
                    string[] arrSectionInfo = sectionInfo.Split(ACAConstant.SPLIT_CHAR);

                    if (arrSectionInfo.Length > 1)
                    {
                        asitUI.SectionTitle = arrSectionInfo[0];
                        asitUI.SectionIndex = int.Parse(arrSectionInfo[1]);
                    }
                }

                asitUI.TableKey = GetTableKey(asitUI.AgencyCode, asitUI.GroupName, asitUI.TableName);
                asitUI.TableTitle = I18nStringUtil.GetString(asiTable.resTableName, asiTable.tableName);

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

                if (isInSpearForm)
                {
                    /*
                     * In spear form, needs to determine the current table whether has drill-down data.
                     * If the asi table has drill-down data, Add button (split button) will become a normal button.
                     */
                    ASITDrillDownUtil asitDrillDown = new ASITDrillDownUtil();
                    DataTable drillDownData = asitDrillDown.GetFirstDrillDownDataList(asitUI.AgencyCode, asitUI.GroupName, asitUI.TableName);
                    asitUI.HasDrillDownData = drillDownData.Rows.Count > 0 && asitDrillDown.IsRequireDrillDownData();
                }

                asitUI.TemplateRow = new UIRow();

                //Create tempate row.
                foreach (var asitColumn in asiTable.columns)
                {
                    string columnSecurity = ACAConstant.Security.Full;

                    if (!isASITSearchForm)
                    {
                        columnSecurity = ASISecurityUtil.GetASITSecurity(asitColumn.servProvCode, asitColumn.groupName, asitColumn.tableName, asitColumn.columnName, moduleName);
                    }

                    bool isHidden = NeedHideColumn4ASIT(asitColumn) || ACAConstant.ASISecurity.None.Equals(columnSecurity);

                    ASITUIField field = new ASITUIField();
                    field.Name = asitColumn.columnName;
                    field.ResName = asitColumn.resColumnName;

                    string fieldLabel = string.Empty;

                    if (asitColumn.templateLayoutConfig != null)
                    {
                        TemplateLayoutConfigModel fieldTemplateConfig = asitColumn.templateLayoutConfig;

                        string resAlternativeLabel = string.Empty;
                        string resInstruction = string.Empty;
                        string resWaterMark = string.Empty;

                        if (fieldTemplateConfig.i18NModel != null)
                        {
                            TemplateLayoutConfigI18NModel i18NModel = fieldTemplateConfig.i18NModel;

                            resAlternativeLabel = i18NModel.alternativeLabel;
                            resInstruction = i18NModel.instruction;
                            resWaterMark = i18NModel.waterMark;
                        }

                        fieldLabel = I18nStringUtil.GetString(resAlternativeLabel, fieldTemplateConfig.alternativeLabel);
                        field.Instruction = I18nStringUtil.GetCurrentLanguageString(resInstruction, fieldTemplateConfig.instruction);
                        field.Watermark = I18nStringUtil.GetCurrentLanguageString(resWaterMark, fieldTemplateConfig.waterMark);
                    }

                    field.Label = I18nStringUtil.GetString(fieldLabel, asitColumn.resColumnName, asitColumn.columnName);
                    field.Type = asitColumn.columnType;
                    field.DefaultValue = I18nStringUtil.GetString(asitColumn.resDefaultValue, asitColumn.defaultValue);
                    field.Value = field.DefaultValue;
                    field.IsHidden = isHidden;
                    field.IsReadOnly = ACAConstant.ASISecurity.Read.Equals(columnSecurity);
                    field.IsRequired = ValidationUtil.IsYes(asitColumn.requiredFlag);
                    field.MaxLength = int.Parse(asitColumn.maxLength);
                    field.DisplayLength = int.Parse(asitColumn.displayLength);
                    field.RequiredFeeCalc = asitColumn.requiredFeeCalc;
                    field.SecurityValue = columnSecurity;
                    field.ValueList = asitColumn.valueList;

                    asitUI.TemplateRow.Fields.Add(field);
                }

                asitUIs.Add(asitUI);

                if (asiTable.tableField == null || asiTable.tableField.Length == 0)
                {
                    //Table is empty.
                    continue;
                }

                long maxRowIndex = asiTable.tableField.Max(f => f.rowIndex);
                asitUI.MaxRowIndexInDB = maxRowIndex < 0 ? 0 : maxRowIndex;
                long dRowIdx = long.MinValue;
                UIRow currentRow = null;
                int colIdx = 0;

                //Create data row.
                foreach (var sField in asiTable.tableField)
                {
                    long sRowIdx = BizRowIndex2UIRowIndex(sField.rowIndex, asitUI.MaxRowIndexInDB);

                    if (!dRowIdx.Equals(sRowIdx))
                    {
                        dRowIdx = sRowIdx;
                        colIdx = 0;
                        currentRow = new UIRow();
                        currentRow.RowIndex = dRowIdx;
                        asitUI.Rows.Add(currentRow);
                    }

                    ASITUIField defaultField = asitUI.TemplateRow.Fields[colIdx] as ASITUIField;
                    ASITUIField field = ObjectCloneUtil.DeepCopy(defaultField);
                    field.FieldID = ControlBuildHelper.GetASITFieldID(asitUI.TableKey, currentRow.RowIndex, colIdx.ToString());
                    field.IsReadOnly = field.IsReadOnly || sField.readOnly;
                    field.IsRequired = field.IsRequired || sField.required;
                    field.Value = sField.inputValue;
                    field.IsDrillDown = sField.drillDown;

                    currentRow.Fields.Add(field);
                    colIdx++;
                }
            }

            return asitUIs.ToArray();
        }

        /// <summary>
        /// Convert to template subgroup to ASIT UI tables.
        /// </summary>
        /// <param name="templateSubgroups">Array of template subgroup object.</param>
        /// <param name="moduleName">Module name.</param>
        /// <param name="tableKeyPrefix">The table key prefix to prevent the name conflict.</param>
        /// <param name="sectionInfo">The UI table section info(Format:<c>title/findex</c>).</param>
        /// <returns>Array of ASIT UI table.</returns>
        public static ASITUITable[] ConvertTemplateTablesToUITables(TemplateSubgroup[] templateSubgroups, string moduleName, string tableKeyPrefix, string sectionInfo)
        {
            if (templateSubgroups == null)
            {
                return null;
            }

            IList<ASITUITable> asitUIs = new List<ASITUITable>();

            foreach (TemplateSubgroup templateSubgroup in templateSubgroups)
            {
                if (templateSubgroup == null)
                {
                    continue;
                }

                string tableSecurity = ASISecurityUtil.GetASITSecurity(templateSubgroup, moduleName);

                if ((templateSubgroup.acaTemplateConfigModel != null && ValidationUtil.IsNo(templateSubgroup.acaTemplateConfigModel.acaDisplayFlag)) ||
                    ACAConstant.ASISecurity.None.Equals(tableSecurity) ||
                    ASISecurityUtil.IsAllFieldsNoAccess(templateSubgroup, moduleName))
                {
                    continue;
                }

                //Fill the table info.
                ASITUITable asitUI = new ASITUITable();

                asitUI.AgencyCode = (templateSubgroup.fields != null && templateSubgroup.fields.Length > 0) ? templateSubgroup.fields[0].serviceProviderCode : string.Empty;
                asitUI.GroupName = (templateSubgroup.fields != null && templateSubgroup.fields.Length > 0) ? templateSubgroup.fields[0].groupName : string.Empty;
                asitUI.TableName = templateSubgroup.subgroupName;
                asitUI.TableKey = GetTableKey(asitUI.AgencyCode, asitUI.GroupName, asitUI.TableName, tableKeyPrefix);
                asitUI.TableTitle = I18nStringUtil.GetString(templateSubgroup.displayName, templateSubgroup.subgroupName);
                asitUI.IsTemplateTable = true;

                if (templateSubgroup.acaTemplateConfigModel != null)
                {
                    ACATemplateConfigModel acaTemplateConfig = templateSubgroup.acaTemplateConfigModel;

                    asitUI.Instruction = I18nStringUtil.GetString(acaTemplateConfig.resInstruction, acaTemplateConfig.instruction);
                    asitUI.ButtonAddLabel = I18nStringUtil.GetString(acaTemplateConfig.resButtonAddLabel, acaTemplateConfig.buttonAddLabel);
                    asitUI.ButtonAddMoreLabel = I18nStringUtil.GetString(acaTemplateConfig.resButtonAddMoreLabel, acaTemplateConfig.buttonAddMoreLabel);
                    asitUI.ButtonEditLabel = I18nStringUtil.GetString(acaTemplateConfig.resButtonEditLabel, acaTemplateConfig.buttonEditLabel);
                    asitUI.ButtonDeleteLabel = I18nStringUtil.GetString(acaTemplateConfig.resButtonDeleteLabel, acaTemplateConfig.buttonDeleteLabel);
                    asitUI.ButtonAddDisplay = acaTemplateConfig.buttonAddDisplay;
                    asitUI.ButtonEditDisplay = acaTemplateConfig.buttonEditDisplay;
                    asitUI.ButtonDeleteDisplay = acaTemplateConfig.buttonDeleteDisplay;
                    asitUI.AlternativeLabel = I18nStringUtil.GetString(acaTemplateConfig.resFieldLabel, acaTemplateConfig.fieldLabel);
                }

                if (ACAConstant.ASISecurity.Read.Equals(tableSecurity, StringComparison.OrdinalIgnoreCase))
                {
                    asitUI.IsReadOnly = true;
                }

                asitUI.TemplateRow = new UIRow();

                //Create template row.
                foreach (var templateField in templateSubgroup.fields)
                {
                    string fieldSecurity = ASISecurityUtil.GetASITSecurity(
                        templateField.serviceProviderCode, templateField.groupName, templateField.subgroupName, templateField.fieldName, moduleName);
                    bool isHidden = NeedHideColumn4ASIT(templateField) || ACAConstant.ASISecurity.None.Equals(fieldSecurity);

                    ASITUIField field = new ASITUIField();
                    field.Name = templateField.fieldName;
                    field.ResName = templateField.displayFieldName;
                    field.Type = templateField.fieldType.ToString();
                    field.DefaultValue = templateField.defaultValue;
                    field.Value = templateField.defaultValue;
                    field.IsHidden = isHidden;
                    field.IsReadOnly = ACAConstant.ASISecurity.Read.Equals(fieldSecurity);
                    field.IsRequired = ValidationUtil.IsYes(templateField.requireFlag);
                    field.MaxLength = templateField.maxLen;

                    string altLabel = string.Empty;

                    if (templateField.acaTemplateConfig != null)
                    {
                        ACATemplateConfigModel fieldTemplateConfig = templateField.acaTemplateConfig;

                        altLabel = I18nStringUtil.GetString(fieldTemplateConfig.resFieldLabel, fieldTemplateConfig.fieldLabel);
                        field.Instruction = I18nStringUtil.GetCurrentLanguageString(fieldTemplateConfig.resInstruction, fieldTemplateConfig.instruction);
                        field.Watermark = I18nStringUtil.GetCurrentLanguageString(fieldTemplateConfig.resWaterMark, fieldTemplateConfig.waterMark);
                    }

                    field.Label = I18nStringUtil.GetString(altLabel, templateField.displayFieldName, templateField.fieldName);
                    field.DisplayLength = templateField.displayLen == 0 ? -1 : templateField.displayLen;
                    field.SecurityValue = fieldSecurity;

                    if (templateField.options != null && templateField.options.Length > 0)
                    {
                        var fieldValueList = new List<RefAppSpecInfoDropDownModel4WS>();

                        foreach (var templateOption in templateField.options)
                        {
                            RefAppSpecInfoDropDownModel4WS option = new RefAppSpecInfoDropDownModel4WS();
                            option.attrValue = templateOption.key;
                            option.groupCode = templateField.groupName;
                            option.groupName = templateField.subgroupName;
                            option.resAttrValue = templateOption.value;
                            option.fieldLabel = templateField.fieldName;
                            option.serviceProviderCode = templateField.serviceProviderCode;
                            fieldValueList.Add(option);
                        }

                        field.ValueList = fieldValueList;
                    }

                    asitUI.TemplateRow.Fields.Add(field);
                }

                if (!string.IsNullOrEmpty(sectionInfo))
                {
                    string[] arrSectionInfo = sectionInfo.Split(ACAConstant.SPLIT_CHAR);

                    if (arrSectionInfo.Length > 1)
                    {
                        asitUI.SectionTitle = arrSectionInfo[0];
                        asitUI.SectionIndex = int.Parse(arrSectionInfo[1]);
                    }
                }

                asitUIs.Add(asitUI);

                if (templateSubgroup.rows == null || templateSubgroup.rows.Length == 0)
                {
                    //Table is empty.
                    continue;
                }

                //Create data row.
                for (int rowIndex = 0; rowIndex < templateSubgroup.rows.Length; rowIndex++)
                {
                    TemplateRow sRow = templateSubgroup.rows[rowIndex];
                    UIRow currentRow = new UIRow();
                    currentRow.RowIndex = rowIndex;
                    asitUI.Rows.Add(currentRow);

                    int colIdx = 0;

                    foreach (UIField uiField in asitUI.TemplateRow.Fields)
                    {
                        ASITUIField defaultField = uiField as ASITUIField;
                        ASITUIField field = ObjectCloneUtil.DeepCopy(defaultField);
                        field.FieldID = ControlBuildHelper.GetASITFieldID(asitUI.TableKey, currentRow.RowIndex, colIdx.ToString());
                        var tempField = sRow.values.FirstOrDefault(w => w.fieldName == field.Name);

                        if (tempField != null)
                        {
                            field.GenericTemplateSeqNum = tempField.seqNum;
                            field.Value = tempField.value;
                        }

                        currentRow.Fields.Add(field);
                        colIdx++;
                    }
                }
            }

            return asitUIs.ToArray();
        }

        /// <summary>
        /// Converts the ASI table to data table.
        /// </summary>
        /// <param name="asiTable">The asi table.</param>
        /// <param name="isFormatHijriDate">Is convert date to hijri date.</param>
        /// <returns>the data table converted from ASI table.</returns>
        public static DataTable ConvertASITableToDataTable(ASITUITable asiTable, bool isFormatHijriDate = false)
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
                    string colName = I18nStringUtil.GetString(defaultField.ResName, defaultField.Name);
                    string columnName = ControlBuildHelper.CreateLabelWithCurrencySymbol(colName, defaultField.RequiredFeeCalc);
                    result.Columns.Add(columnName);
                }
            }

            result.Columns.Add(ASITUITable.RowIdentityColumnName, typeof(int));

            if (asiTable.Rows != null && asiTable.Rows.Count != 0)
            {
                foreach (UIRow row in asiTable.Rows)
                {
                    DataRow currentRow = result.NewRow();
                    result.Rows.Add(currentRow);
                    currentRow[ASITUITable.RowIdentityColumnName] = row.RowIndex;

                    foreach (ASITUIField field in row.Fields)
                    {
                        if (!field.IsHidden && !field.IsHiddenByExp)
                        {
                            string resFieldValue = GetASITFieldResValue(field, asitFieldsWithResValues);
                            
                            if (field.Type == ((int)FieldType.HTML_TEXTBOX_OF_DATE).ToString() && isFormatHijriDate)
                            {
                                resFieldValue = HijriDateUtil.ToHijriDate(resFieldValue);
                            }

                            string colName = I18nStringUtil.GetString(field.ResName, field.Name);
                            string columnName = ControlBuildHelper.CreateLabelWithCurrencySymbol(colName, field.RequiredFeeCalc);

                            currentRow[columnName] = I18nStringUtil.GetString(resFieldValue, field.Value);
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
        /// <param name="uiDataKey">The UI data key.</param>
        /// <returns>A new UI Table contains the new rows.</returns>
        public static ASITUITable CreateNewRow4ASITUITable(ASITUITable sTable, int rowQty, string uiDataKey)
        {
            if (sTable != null && rowQty > 0)
            {
                ASITUITable dTable = ObjectCloneUtil.DeepCopy(sTable);
                List<UIRow> newRows = new List<UIRow>();
                long curRowIdx = GetNewRowIndex(dTable);

                for (long i = curRowIdx; i < curRowIdx + rowQty; i++)
                {
                    UIRow newRow = ObjectCloneUtil.DeepCopy(dTable.TemplateRow);
                    newRow.RowIndex = i;

                    for (int j = 0; j < newRow.Fields.Count; j++)
                    {
                        /*
                         * For ASIT fields, field ID format is ASIT_{TableKey:8}_{Row Index}_{Column Index}
                         * For Generic template table fields, field ID format is ASIT_{TableKey:8}_{Row Index}_{Encoded field name:8}
                         */

                        var fieldID = ControlBuildHelper.GetASITFieldID(dTable.TableKey, i, newRow.Fields[j].Name.GetHashCode().ToString("X2"));

                        if (uiDataKey.StartsWith(PageFlowConstant.SECTION_NAME_ASIT))
                        {
                            fieldID = ControlBuildHelper.GetASITFieldID(dTable.TableKey, i, j.ToString());
                        }

                        newRow.Fields[j].FieldID = fieldID;
                    }

                    newRows.Add(newRow);
                }

                dTable.Rows = newRows;

                return dTable;
            }

            return null;
        }

        /// <summary>
        /// Gets the ASIT field res value.
        /// </summary>
        /// <param name="currentField">The current field.</param>
        /// <param name="asitFieldsWithResValues">The asit fields with res values.</param>
        /// <returns>
        /// the ASIT field res value.
        /// </returns>
        public static string GetASITFieldResValue(ASITUIField currentField, List<ASITUIField> asitFieldsWithResValues)
        {
            string result = string.Empty;

            if (currentField != null)
            {
                string currentFieldValue = !string.IsNullOrEmpty(currentField.Value) ? currentField.Value : string.Empty;

                if ((int)FieldType.HTML_SELECTBOX == int.Parse(currentField.Type) && currentField.ValueList != null && currentField.ValueList.Count() > 0)
                {
                    var matchedResItem = currentField.ValueList.SingleOrDefault(p => currentFieldValue.Equals(p.attrValue, StringComparison.OrdinalIgnoreCase));
                    result = matchedResItem != null && !string.IsNullOrEmpty(matchedResItem.resAttrValue) ? matchedResItem.resAttrValue : string.Empty;
                }
                else if ((int)FieldType.HTML_CHECKBOX == int.Parse(currentField.Type) || (int)FieldType.HTML_RADIOBOX == int.Parse(currentField.Type))
                {
                    result = ModelUIFormat.FormatYNLabel(currentFieldValue);
                }
                else if ((int)FieldType.HTML_TEXTBOX_OF_DATE == int.Parse(currentField.Type))
                {
                    result = I18nDateTimeUtil.ConvertDateStringFromWebServiceToUI(currentFieldValue);
                }
                else if ((int)FieldType.HTML_TEXTBOX_OF_CURRENCY == int.Parse(currentField.Type))
                {
                    result = I18nNumberUtil.ConvertNumberStringFromWebServiceToUI(currentFieldValue);
                }
                else if ((int)FieldType.HTML_TEXTBOX_OF_NUMBER == int.Parse(currentField.Type))
                {
                    result = I18nNumberUtil.ConvertNumberStringFromWebServiceToUI(currentFieldValue);
                }
                else if (currentField.IsDrillDown)
                {
                    ASITUIField currentResField = asitFieldsWithResValues.SingleOrDefault(p => !string.IsNullOrEmpty(p.FieldID) && p.FieldID.Equals(currentField.FieldID, StringComparison.OrdinalIgnoreCase));
                    result = currentResField != null ? currentResField.Value : string.Empty;
                }
            }

            return result;
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
            var fieldValues = (from field in drillDownFields
                               select field.Value).ToArray();

            var asitDrillDownBLL = ObjectFactory.GetObject<IASITableDrillDownBLL>();
            string[] resValues = fieldValues.Count() == 0 ? null : asitDrillDownBLL.GetFieldTexts(appInfoTable.AgencyCode, fieldValues);

            if (resValues != null && resValues.Length == drillDownFields.Count())
            {
                var resFields = ObjectCloneUtil.DeepCopy(drillDownFields);

                for (int i = 0; i < resValues.Length; i++)
                {
                    resFields[i].Value = resValues[i];
                }

                result = resFields;
            }

            return result;
        }

        /// <summary>
        /// Get row index for the new row will be added.
        /// </summary>
        /// <param name="asitUITable">ASIT UI table</param>
        /// <returns>row index.</returns>
        public static long GetNewRowIndex(ASITUITable asitUITable)
        {
            if (asitUITable.Rows != null && asitUITable.Rows.Count > 0)
            {
                return asitUITable.Rows.Max(r => r.RowIndex) + 1;
            }
            else
            {
                return BizRowIndex2UIRowIndex(asitUITable.MaxRowIndexInDB + 1, asitUITable.MaxRowIndexInDB);
            }
        }

        /// <summary>
        /// Concatenates the ASITUI tables. If exist the same key (agencyCode+GroupName+TableName+RowIndex) in the two arrays, 
        /// the data with same key in tableArray1 will be returned instead of the data in tableArray2.
        /// </summary>
        /// <param name="tableArray1">The table array1.</param>
        /// <param name="tableArray2">The table array2.</param>
        /// <returns>the concatenated ASITUI tables.</returns>
        public static ASITUITable[] ConcatASITUITables(ASITUITable[] tableArray1, ASITUITable[] tableArray2)
        {
            ASITUITable[] result = null;

            if (tableArray1 == null)
            {
                result = tableArray2;
            }
            else if (tableArray2 == null)
            {
                result = tableArray1;
            }
            else
            {
                var tempArray = ObjectCloneUtil.DeepCopy(tableArray2);
                SyncASITUIRowData(tableArray1, tempArray);

                //Add the new tables to destination table list.
                var newTables = tableArray1.Where(t => !tempArray.Select(p => p.TableKey).Contains(t.TableKey));

                if (newTables.Count() > 0)
                {
                    var newTablesCopy = ObjectCloneUtil.DeepCopy(newTables.ToArray());
                    result = tempArray.Concat(newTablesCopy).ToArray();
                }
                else
                {
                    result = tempArray;
                }
            }

            return result;
        }

        /// <summary>
        /// Generate Table Key for ASIT or Generic template table.
        /// It's generate by the [TableKeyPrefix + "\f"] + AgencyCode + "\f" + GroupName + "\f" + TableName.
        /// </summary>
        /// <param name="agencyCode">Agency code.</param>
        /// <param name="groupName">Group name.</param>
        /// <param name="tableName">Table name.</param>
        /// <param name="tableKeyPrefix">
        /// The table key prefix to prevent the name conflict when the different entity 
        ///     associated with the same generic template table in the same page. 
        /// It is not required.
        /// </param>
        /// <returns>Table Key used to unique indicate a ASI table or generic template table.</returns>
        public static string GetTableKey(string agencyCode, string groupName, string tableName, string tableKeyPrefix = null)
        {
            string templateKey = agencyCode + "\f" + groupName + "\f" + tableName;

            if (!string.IsNullOrWhiteSpace(tableKeyPrefix))
            {
                templateKey = tableKeyPrefix + "\f" + templateKey;
            }

            return templateKey.GetHashCode().ToString("X2");
        }

        /// <summary>
        /// Sync the UI rows from the source table to the destination table.
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

                IList<long> updatedRowIdxes = new List<long>();

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
                    var newRowsCopy = ObjectCloneUtil.DeepCopy(newRows.ToList());
                    dTable.Rows.AddRange(newRowsCopy);
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
                    foreach (var dField in dTable.tableField)
                    {
                        UIRow sRow = sTable.Rows.Where(r => r.RowIndex == BizRowIndex2UIRowIndex(dField.rowIndex, sTable.MaxRowIndexInDB)).SingleOrDefault();

                        if (sRow != null)
                        {
                            ASITUIField sField = sRow.Fields.Where(f => f.Name == dField.fieldLabel).SingleOrDefault() as ASITUIField;

                            if (sField != null)
                            {
                                dField.inputValue = sField.Value;
                            }

                            updatedRowIdxes.Add(sRow.RowIndex);
                        }
                    }
                }

                /*
                 * Added rows is: row index existing in source table but not exists in destination table.
                 * Deleted rows is: row index existing in destination table but not exists in source table.
                 */

                //Split the added rows and deleted rows.
                var addedRows = sTable.Rows.Where(r => !updatedRowIdxes.Contains(r.RowIndex));
                var addedRowIdxes = addedRows.Select(r => (long)r.RowIndex);

                //Update deleted rows.
                if (dTable.tableField != null && dTable.tableField.Length > 0)
                {
                    var deletedFields = dTable.tableField.Where(f =>
                        !updatedRowIdxes.Contains(BizRowIndex2UIRowIndex(f.rowIndex, sTable.MaxRowIndexInDB)) &&
                        !addedRowIdxes.Contains(BizRowIndex2UIRowIndex(f.rowIndex, sTable.MaxRowIndexInDB))).Count();

                    if (deletedFields > 0)
                    {
                        /*
                         * The performance of LambdaExpressionResult.ToArray() is bad when the expression is complicated.
                         * When more than 1,000 members will take tens of seconds of time.
                         * //var remainedFields = dTable.tableField.Where(f => !deletedRowIdxes.Contains(f.rowIndex));
                         * //dTable.tableField = remainedFields.ToArray();
                         */
                        var remainedFields = new List<AppSpecificTableField4WS>();

                        foreach (var fld in dTable.tableField)
                        {
                            var rowIdx = BizRowIndex2UIRowIndex(fld.rowIndex, sTable.MaxRowIndexInDB);

                            if (updatedRowIdxes.Contains(rowIdx) || addedRowIdxes.Contains(rowIdx))
                            {
                                remainedFields.Add(fld);
                            }
                        }

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
                    long curRowIdx = -1;
                    int fieldIdx = 0;
                    AppSpecificTableField4WS[] allFields = new AppSpecificTableField4WS[allFieldsQty];

                    //Copy existing fields.
                    if (dTable.tableField != null && dTable.tableField.Length > 0)
                    {
                        Array.Copy(dTable.tableField, allFields, dTable.tableField.Length);
                        fieldIdx = dTable.tableField.Length;
                        long minRowIndex = dTable.tableField.Min(fld => fld.rowIndex);

                        if (minRowIndex < 0)
                        {
                            curRowIdx = minRowIndex - 1;
                        }
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
                                allFields[fieldIdx].rowIndex = curRowIdx;
                            }

                            fieldIdx++;
                        }

                        curRowIdx--;
                    }

                    dTable.tableField = allFields;
                }
            }
        }

        /// <summary>
        /// Sync the input values from UI data table to template subgroup biz models.
        /// </summary>
        /// <param name="sourTables">ASIT UI data tables.</param>
        /// <param name="destTables">Template sub group biz models.</param>
        public static void SyncInputValueToTemplateTableModel(ASITUITable[] sourTables, TemplateSubgroup[] destTables)
        {
            if (sourTables == null || destTables == null)
            {
                return;
            }

            for (int i = 0; i < destTables.Length; i++)
            {
                TemplateSubgroup dTable = destTables[i];

                if (dTable == null || dTable.fields == null || dTable.fields.Count() == 0)
                {
                    continue;
                }

                string groupName = dTable.fields[0].groupName;
                ASITUITable sTable = sourTables.SingleOrDefault(dt =>
                    dt.GroupName == groupName &&
                    dt.TableName == dTable.subgroupName);

                /*
                 * If the source table does not existing the target table list, keep the data in the target table.
                 * An example, if the table is hidden to the public user by ASI/ASIT security, still need to keep the existing table data.
                 */
                if (sTable == null)
                {
                    continue;
                }

                // If the source table existing in the target table list and does not contain any data, clear the target table.
                if (sTable.Rows.Count == 0)
                {
                    dTable.rows = null;
                    continue;
                }

                dTable.rows = new TemplateRow[sTable.Rows.Count];

                for (int rowIdx = 0; rowIdx < sTable.Rows.Count; rowIdx++)
                {
                    TemplateRow tempRow = new TemplateRow();
                    UIRow uiRow = sTable.Rows[rowIdx];

                    //Row index of generic template table is start with 1.
                    tempRow.rowIndex = rowIdx + 1;
                    tempRow.values = new GenericTemplateTableValue[uiRow.Fields.Count];

                    for (int fieldIdx = 0; fieldIdx < tempRow.values.Length; fieldIdx++)
                    {
                        GenericTemplateTableValue templateTableValue = new GenericTemplateTableValue();
                        ASITUIField uiField = uiRow.Fields[fieldIdx] as ASITUIField;

                        GenericTemplateAttribute templateField = dTable.fields.Where(f => f.fieldName == uiField.Name).FirstOrDefault();

                        templateTableValue.fieldName = templateField.fieldName;
                        templateTableValue.groupName = templateField.groupName;
                        templateTableValue.rowIndex = rowIdx + 1;
                        templateTableValue.seqNum = uiField.GenericTemplateSeqNum;
                        templateTableValue.attributeSeq = templateField.seqNum;
                        templateTableValue.serviceProviderCode = templateField.serviceProviderCode;
                        templateTableValue.subgroupName = templateField.subgroupName;
                        templateTableValue.value = uiField.Value;

                        tempRow.values[fieldIdx] = templateTableValue;
                    }

                    dTable.rows[rowIdx] = tempRow;
                }
            }
        }

        /**********************************************************************************************************
         * ASIT UI table row index is same with the ASIT data row index in database.
         *     - If ASIT data is created by old logic, the row index of the data in the DB is start with 0.
         *     - If ASIT data is created by new logic, the row index of the data in the DB is start with 1.
         * 
         * The old logic use the AppSpecificTableModel4WS.tableFieldValues property (a string array)
         *     - First deletes all data, then create all data.
         * The new logic use the AppSpecificTableModel4WS.tableField property (a AppSpecificTableField4WS array)
         *     - Create/Update/Delete the relevant data based on the row index (AppSpecificTableField4WS.rowIndex).
         * 
         * In the new logic:
         *     - For newly added item in AppSpecificTableModel4WS model:
         *       the row index (AppSpecificTableField4WS.rowIndex) must start with -1. e.g.: -1, -2 ,-3 ...
         ********************************************************************************************************** 
         * For example 1:
         *                  UI  Biz
         * Existing Row     0   0
         * Existing Row     1   1
         * Existing Row     2   2
         *      New Row     3   -1
         *      New Row     4   -2
         *      New Row     5   -3
         * 
         * Max row index in DB: 2
         * 
         * For example 2:
         *                  UI  Biz
         * Existing Row     1   1
         * Existing Row     2   2
         * Existing Row     3   3
         *      New Row     4   -1
         *      New Row     5   -2
         *      New Row     6   -3
         * 
         * Max row index in DB: 3
         * 
         * For example 2:
         *                  UI  Biz
         *      New Row     1   -1
         *      New Row     2   -2
         *      New Row     3   -3
         * 
         * Max row index in DB: 0
         **********************************************************************************************************/

        /// <summary>
        /// Convert biz row index to UI row index for ASIT.
        /// </summary>
        /// <param name="bizRowIndex">The row index in biz object.</param>
        /// <param name="maxRowIndexInDB">The maximum row index in biz object.</param>
        /// <returns>Row index of UI data.</returns>
        private static long BizRowIndex2UIRowIndex(long bizRowIndex, long maxRowIndexInDB)
        {
            if (bizRowIndex >= 0)
            {
                return bizRowIndex;
            }
            else
            {
                return maxRowIndexInDB + Math.Abs(bizRowIndex);
            }
        }

        /// <summary>
        /// Check if need hide the ASI table column.
        /// </summary>
        /// <param name="column">AppSpecificTableColumnModel4WS object.</param>
        /// <returns>true if need hide the column;otherwise,return false.</returns>
        private static bool NeedHideColumn4ASIT(AppSpecificTableColumnModel4WS column)
        {
            bool needHide = false;

            if (column == null ||
                ValidationUtil.IsNo(column.vchDispFlag) ||
                ValidationUtil.IsHidden(column.vchDispFlag) ||
                column.recStatus == ACAConstant.INVALID_STATUS)
            {
                needHide = true;
            }

            return needHide;
        }

        /// <summary>
        /// Check if need hide the ASI table column.
        /// </summary>
        /// <param name="field">Generic template field.</param>
        /// <returns>true if need hide the column;otherwise,return false.</returns>
        private static bool NeedHideColumn4ASIT(GenericTemplateAttribute field)
        {
            bool needHide = false;

            if (field == null || (field.acaTemplateConfig != null 
                && (ValidationUtil.IsNo(field.acaTemplateConfig.acaDisplayFlag) || ValidationUtil.IsHidden(field.acaTemplateConfig.acaDisplayFlag))))
            {
                needHide = true;
            }

            return needHide;
        }
    }
}