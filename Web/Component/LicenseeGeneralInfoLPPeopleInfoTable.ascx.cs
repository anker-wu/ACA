#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: LicenseeGeneralInfoLPPeopleInfoTable.aspx.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2010-2014
 *
 *  Description:
 *
 *  Notes:
 *      $Id: LicenseeGeneralInfoLPPeopleInfoTable.ascx.cs 185614 2010-12-01 06:22:24Z ACHIEVO\xinter.peng $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using System;
using System.Collections;
using System.Web.UI;

using Accela.ACA.Common;
using Accela.ACA.Common.Util;
using Accela.ACA.Web.Common;
using Accela.ACA.WSProxy;
using Accela.Web.Controls;

namespace Accela.ACA.Web.Component
{
    using System.Linq;

    /// <summary>
    ///  LicenseeGeneralInfo's LP people info
    /// </summary>
    public partial class LicenseeGeneralInfoLPPeopleInfoTable : BaseUserControl
    {
        /// <summary>
        /// display licensee table info
        /// </summary>
        /// <param name="infoTableGrorup">info table group Model</param>
        /// <returns>true: if display successfully,otherwise return false;</returns>
        public bool Display(InfoTableGroupCodeModel4WS infoTableGrorup)
        {
            bool result = false;

            if (infoTableGrorup != null && infoTableGrorup.subgroups != null && infoTableGrorup.subgroups.Length != 0)
            {
                ArrayList infoTableSubGroupList = ArrayList.Adapter(infoTableGrorup.subgroups);
                infoTableSubGroupList.Sort(new InfoTableSubGroupsCompare());
                
                foreach (InfoTableSubgroupModel4WS infoTableSubgroup in infoTableSubGroupList)
                {
                    if (!IsNullSubgroup(infoTableSubgroup))
                    {
                        phLPPeopleInfoTable.Controls.Add(new LiteralControl("<div class=\"container_40\"><table role='presentation' border=\"0\" cellspacing=\"0\" cellpadding=\"0\" width=\"100%\" style=\"table-layout: fixed\">"));
                                                
                        //Display subgroup title 
                        phLPPeopleInfoTable.Controls.Add(new LiteralControl("<tr><td>"));
                        phLPPeopleInfoTable.Controls.Add(new LiteralControl("<div class=\"ACA_TabRow ACA_Title_Text font13px\">" + infoTableSubgroup.dispName + "</div>"));
                        phLPPeopleInfoTable.Controls.Add(new LiteralControl("</td><tr>"));

                        //subgroup-content
                        phLPPeopleInfoTable.Controls.Add(new LiteralControl("<tr><td>"));
                        phLPPeopleInfoTable.Controls.Add(new LiteralControl("<table role='presentation' border=\"0\" cellspacing=\"0\" cellpadding=\"0\" width=\"100%\" style=\"table-layout: fixed\">"));                                                 
                        DisplaySubgroup(infoTableSubgroup);                                                 
                        phLPPeopleInfoTable.Controls.Add(new LiteralControl("</table>"));
                        phLPPeopleInfoTable.Controls.Add(new LiteralControl("</td><tr>"));

                        phLPPeopleInfoTable.Controls.Add(new LiteralControl("</table></div>"));

                        result = true;
                    }
                }
            }

            return result;
        }              

        /// <summary>
        /// Display SubGroup content
        /// </summary>
        /// <param name="infoTableSubgroup">Info table subgroup model</param>
        private void DisplaySubgroup(InfoTableSubgroupModel4WS infoTableSubgroup)
        {
            InfoTableColumnModel4WS[] infoTableColumns = infoTableSubgroup.columnDefines;

            int columnCount = infoTableColumns.Length;
            int rowCount = infoTableColumns[0].tableValues.Length;

            ArrayList infoTableValueAll = GetAllInfoTableValuesOfSubGroup(infoTableColumns);

            for (int rowIndex = 0; rowIndex < rowCount; rowIndex++)
            {
                //set a row content 
                ArrayList infoTableValuesList = new ArrayList();
                for (int columnIndex = 0; columnIndex < columnCount; columnIndex++)
                {
                    foreach (InfoTableValueModel4WS infoTableValue in infoTableValueAll)
                    {
                        if (infoTableValue != null && infoTableValue.rowNumber == rowIndex.ToString() && infoTableValue.columnNumber == columnIndex.ToString() && infoTableValue.auditStatus != null && infoTableValue.auditStatus.ToUpper() == ACAConstant.TEMPLATE_FIIELD_STATUS_LIC_VERIFICATION_VALUE && IsDispalyColumnForACA(infoTableValue.infoId, infoTableColumns))
                        {
                            infoTableValuesList.Add(infoTableValue);
                            break;
                        }
                    }
                }

                if (infoTableValuesList != null && infoTableValuesList.Count > 0)
                {
                    infoTableValuesList.Sort(new InfoTableValuesCompare());

                    if (rowIndex != 0)
                    {
                        phLPPeopleInfoTable.Controls.Add(new LiteralControl("<tr><td><div class=\"ACA_TabRow ACA_Line_Content\">&nbsp;</div></td></tr>"));
                    }

                    DisplayRow(infoTableValuesList, infoTableColumns);                    
                }
            }
        }

        /// <summary>
        /// display row contents according to info table values and columns info
        /// </summary>
        /// <param name="infoTableValueList">the info table value list</param>
        /// <param name="infoTableColumns">info table columns model</param>
        private void DisplayRow(ArrayList infoTableValueList, InfoTableColumnModel4WS[] infoTableColumns)
        {
            foreach (InfoTableValueModel4WS infoTableValue in infoTableValueList)
            {
                string fieldValue = FormatDisplayValue(infoTableValue.infoId, infoTableColumns, infoTableValue.value);                
                if (string.IsNullOrEmpty(fieldValue))
                {
                    continue;
                }

                string columnName = GetColumnNameById(infoTableValue.infoId, infoTableColumns);                
                phLPPeopleInfoTable.Controls.Add(new LiteralControl("<tr><td style=\"width: 50%; vertical-align: top;\">"));

                phLPPeopleInfoTable.Controls.Add(new LiteralControl("<table role='presentation'><tr>"));
                phLPPeopleInfoTable.Controls.Add(new LiteralControl("<td style=\"vertical-align: top; white-space: nowrap;\"><p>"));

                AccelaLabel lblField = new AccelaLabel();
                AccelaLabel lblFieldValue = new AccelaLabel();

                lblField.Font.Bold = true;

                //It is an AccelaLabel,so it's value are not need to Encode.
                lblField.Text = columnName + ACAConstant.COLON_CHAR;

                phLPPeopleInfoTable.Controls.Add(lblField);

                phLPPeopleInfoTable.Controls.Add(new LiteralControl("</p></td>"));
                phLPPeopleInfoTable.Controls.Add(new LiteralControl("<td style=\"vertical-align: top; padding-left:2px;\"><p class='break-word'>"));

                //It is an AccelaLabel,so it's value are not need to Encode.
                lblFieldValue.Text = fieldValue;
                phLPPeopleInfoTable.Controls.Add(lblFieldValue);

                phLPPeopleInfoTable.Controls.Add(new LiteralControl("</p></td></tr></table>"));

                phLPPeopleInfoTable.Controls.Add(new LiteralControl("</td></tr>"));               
            }
        }

        /// <summary>
        /// format display value
        /// </summary>
        /// <param name="infoId">the info id</param>
        /// <param name="infoTableColumns">the info table columns</param>
        /// <param name="value">the value parameter</param>
        /// <returns>Return the format display value</returns>
        private string FormatDisplayValue(string infoId, InfoTableColumnModel4WS[] infoTableColumns, string value)
        {
            for (int i = 0; i < infoTableColumns.Length; i++)
            {                
                if (infoTableColumns[i] != null && infoTableColumns[i].id == infoId)
                {
                    switch (infoTableColumns[i].fieldType)
                    {
                        case ACAConstant.CONTROL_CHECKBOX_ID:
                            if (string.IsNullOrEmpty(value))
                            {
                                value = ACAConstant.COMMON_UNCHECKED;
                            }

                            value = ModelUIFormat.FormatYNLabel(value);
                            break;

                        case ACAConstant.CONTROL_RADIOBOX_ID:
                            value = ModelUIFormat.FormatYNLabel(value);
                            break;

                        case ACAConstant.CONTROL_SELECTBOX_ID:
                            if (infoTableColumns[i].dropDownValues != null && infoTableColumns[i].dropDownValues.Length != 0)
                            {
                                var selectResValue = infoTableColumns[i].dropDownValues.Where(
                                    o => o.value.Equals(value)).FirstOrDefault();

                                if (selectResValue != null)
                                {
                                    value = selectResValue.label;
                                }
                            }

                            break;
                    }

                    break;
                }
            }

            return value;
        }

        /// <summary>
        /// get all info table values according to info table columns for one subgroup
        /// </summary>
        /// <param name="infoTableColumns">info table columns model</param>
        /// <returns>info table values model</returns>
        private ArrayList GetAllInfoTableValuesOfSubGroup(InfoTableColumnModel4WS[] infoTableColumns)
        {
            ArrayList infoTableValuesList = new ArrayList();
            for (int j = 0; j < infoTableColumns.Length; j++)
            {
                InfoTableValueModel4WS[] infoTableValues = infoTableColumns[j].tableValues;

                if (infoTableValues != null)
                {
                    for (int tableValueItem = 0; tableValueItem < infoTableValues.Length; tableValueItem++)
                    {
                        if (!string.IsNullOrEmpty(infoTableColumns[j].fieldType))
                        {
                            switch (int.Parse(infoTableColumns[j].fieldType))
                            {
                                case (int)FieldType.HTML_TEXTBOX_OF_DATE:
                                    infoTableValues[tableValueItem].value = I18nDateTimeUtil.FormatToDateStringForUI(infoTableValues[tableValueItem].value);
                                    break;
                                case (int)FieldType.HTML_TEXTBOX_OF_NUMBER:
                                    infoTableValues[tableValueItem].value = I18nNumberUtil.FormatNumberForUI(infoTableValues[tableValueItem].value);
                                    break;
                                case (int)FieldType.HTML_TEXTBOX_OF_TIME:
                                    DateTime dt;

                                    if (I18nDateTimeUtil.TryParseFromWebService(infoTableValues[tableValueItem].value, out dt))
                                    {
                                        infoTableValues[tableValueItem].value = I18nDateTimeUtil.FormatToTimeStringForUI(dt, false);
                                    }

                                    break;
                                case (int)FieldType.HTML_TEXTBOX_OF_CURRENCY:
                                    infoTableValues[tableValueItem].value = I18nNumberUtil.FormatMoneyForUI(infoTableValues[tableValueItem].value);
                                    break;
                            }
                        }

                        infoTableValuesList.Add(infoTableValues[tableValueItem]);
                    }
                }
            }

            return infoTableValuesList;
        }

        /// <summary>
        /// get column name according to info id
        /// </summary>
        /// <param name="infoId">the info id</param>
        /// <param name="infoTableColumns">info table columns model</param>
        /// <returns>Return the column name by id</returns>
        private string GetColumnNameById(string infoId, InfoTableColumnModel4WS[] infoTableColumns)
        {
            string columnName = string.Empty;
            for (int i = 0; i < infoTableColumns.Length; i++)
            {
                if (infoTableColumns[i] != null && infoTableColumns[i].id == infoId)
                {
                    columnName = string.IsNullOrEmpty(infoTableColumns[i].dispName) ? infoTableColumns[i].name : infoTableColumns[i].dispName;
                    break;
                }
            }

            return columnName;
        }

        /// <summary>
        /// judge if display column according to info id 
        /// </summary>
        /// <param name="infoId">the info id</param>
        /// <param name="infoTableColumns">info table columns</param>
        /// <returns>true or false</returns>
        private bool IsDispalyColumnForACA(string infoId, InfoTableColumnModel4WS[] infoTableColumns)
        {
            bool isDisplayColumn = false;
            for (int i = 0; i < infoTableColumns.Length; i++)
            {
                if (infoTableColumns[i].id == infoId && !IsNullColumn(infoTableColumns[i]))
                {                    
                    isDisplayColumn = true;
                    break; 
                }
            }

            return isDisplayColumn;
        }

        /// <summary>
        /// judge if a column is null 
        /// </summary>
        /// <param name="infoTableColumns">info table column</param>
        /// <returns>true or false</returns>
        private bool IsNullColumn(InfoTableColumnModel4WS infoTableColumns)
        {
            bool isNullCol = true;
            if (infoTableColumns != null && infoTableColumns.displayLicVeriForACA != null && infoTableColumns.displayLicVeriForACA.ToUpper() == ACAConstant.COMMON_Y && infoTableColumns.tableValues != null)
            {
                foreach (InfoTableValueModel4WS infoTableValue in infoTableColumns.tableValues)
                {
                    if (infoTableValue != null && infoTableValue.auditStatus != null && infoTableValue.auditStatus.ToUpper() == ACAConstant.TEMPLATE_FIIELD_STATUS_LIC_VERIFICATION_VALUE)
                    {
                        isNullCol = false;
                        break;
                    }
                }
            }

            return isNullCol;
        }

        /// <summary>
        /// judge if the subgroup is null
        /// </summary>
        /// <param name="infoTabelSubgroup">info table subgroup model</param>
        /// <returns>true of false</returns>
        private bool IsNullSubgroup(InfoTableSubgroupModel4WS infoTabelSubgroup)
        {
            bool isNull = true;
            if (infoTabelSubgroup.columnDefines != null && infoTabelSubgroup.columnDefines.Length > 0)
            {
                for (int i = 0; i < infoTabelSubgroup.columnDefines.Length; i++)
                {
                    InfoTableColumnModel4WS infoTableColumn = infoTabelSubgroup.columnDefines[i];
                    if (!IsNullColumn(infoTableColumn))
                    {
                        isNull = false;
                        break;
                    }
                }
            }

            return isNull;
        }

        /// <summary>
        /// comparer the display order for info table subgroup by ASIT order.
        /// </summary>
        private class InfoTableSubGroupsCompare : IComparer
        {
            /// <summary>
            /// Compares two objects and returns a value indicating whether one is less than, equal to, or greater than the other.
            /// </summary>
            /// <param name="x">The first object to compare.</param>
            /// <param name="y">The second object to compare.</param>
            /// <returns>Value Condition Less than zero x is less than y. Zero x equals y. Greater than zero x is greater than y.</returns>
            int IComparer.Compare(object x, object y)
            {
                InfoTableSubgroupModel4WS infoTableSubgroup1 = (InfoTableSubgroupModel4WS)x;
                InfoTableSubgroupModel4WS infoTableSubgroup2 = (InfoTableSubgroupModel4WS)y;
                if (infoTableSubgroup1.displayOrder != null && infoTableSubgroup2.displayOrder != null)
                {
                    double displayOrder1 = I18nNumberUtil.ParseNumberFromWebService(infoTableSubgroup1.displayOrder);
                    double displayOrder2 = I18nNumberUtil.ParseNumberFromWebService(infoTableSubgroup2.displayOrder);
                    return displayOrder1.CompareTo(displayOrder2);
                }
                else
                {
                    return 0;
                }
            }
        }

        /// <summary>
        /// comparer the display order for info table values by ASIT order.
        /// </summary>
        private class InfoTableValuesCompare : IComparer
        {
            /// <summary>
            /// Compares two objects and returns a value indicating whether one is less than, equal to, or greater than the other.
            /// </summary>
            /// <param name="x">The first object to compare.</param>
            /// <param name="y">The second object to compare.</param>
            /// <returns>Value Condition Less than zero x is less than y. Zero x equals y. Greater than zero x is greater than y.</returns>
            int IComparer.Compare(object x, object y)
            {
                InfoTableValueModel4WS infoTableValue1 = (InfoTableValueModel4WS)x;
                InfoTableValueModel4WS infoTableValue2 = (InfoTableValueModel4WS)y;
                double displayOrder1 = I18nNumberUtil.ParseNumberFromWebService(infoTableValue1.columnNumber);
                double displayOrder2 = I18nNumberUtil.ParseNumberFromWebService(infoTableValue2.columnNumber);
                return displayOrder1.CompareTo(displayOrder2);
            }
        }
    }
}
