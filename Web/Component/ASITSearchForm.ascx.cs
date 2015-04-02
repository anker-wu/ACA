#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: ASITSearchForm.ascx.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2014
 *
 *  Description: Search by ASIT
 *
 *  Notes:
 *      $Id: ASITSearchForm.ascx.cs 274156 2014-06-26 08:58:15Z ACHIEVO\james.shi $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;

using Accela.ACA.BLL.Cap;
using Accela.ACA.Common;
using Accela.ACA.Common.Util;
using Accela.ACA.UI.Model;
using Accela.ACA.Web.Common;
using Accela.ACA.Web.Common.Control;
using Accela.ACA.Web.Util;
using Accela.ACA.WSProxy;
using Accela.Web.Controls;

namespace Accela.ACA.Web.Component
{
    /// <summary>
    /// the class for ASITSearchForm
    /// </summary>
    public partial class ASITSearchForm : BaseUserControl
    {
        /// <summary>
        /// text area max input length.
        /// </summary>
        private const int MAX_LENGTH = 4000;
     
        /// <summary>
        /// load the all controls
        /// </summary>
        private List<WebControl> _allControl = new List<WebControl>();

        /// <summary>
        /// UI data values
        /// </summary>
        private ASITUITable[] uiData;

        /// <summary>
        /// Gets or sets ASIT group info when expand/collapse ASI search form.
        /// </summary>
        public ASITUITable[] TempASITGroupInfo
        {
            get
            {
                return ViewState["TempASITGroupInfo"] as ASITUITable[];
            }

            set
            {
                ViewState["TempASITGroupInfo"] = value;
            }
        }

        /// <summary>
        /// Bind data to UI controls
        /// </summary>
        /// <param name="capTypeModel">CapType Model</param>
        public void BindUIControls(CapTypeModel capTypeModel)
        {
            if (TempASITGroupInfo == null)
            {
                SetASITable(capTypeModel);
            }
                
            CreateASITable(TempASITGroupInfo);
        }

        /// <summary>
        /// set ASITable
        /// </summary>
        /// <param name="capType">cap type model</param>
        /// <param name="searchGroups">app specific table group model</param>
        public void SetASITable(CapTypeModel capType, AppSpecificTableGroupModel4WS[] searchGroups = null)
        {
            List<AppSpecificInfoModel4WS> searchs = null;
            
            if (searchGroups != null)
            {
                searchs = new List<AppSpecificInfoModel4WS>();

                foreach (var appSpecTable in searchGroups)
                {
                    if (appSpecTable == null || appSpecTable.searchInfoModels == null)
                    {
                        continue;
                    }

                    searchs.AddRange(appSpecTable.searchInfoModels);
                }
            }

            IAppSpecificInfoBll asiBll = ObjectFactory.GetObject<IAppSpecificInfoBll>();
            AppSpecificTableGroupModel4WS[] groupModel = asiBll.GetSearchableAppSpecTableFieldList(capType);

            if (groupModel == null)
            {
                ClearControls();
                return;
            }

            List<AppSpecificTableModel4WS> asiTables = new List<AppSpecificTableModel4WS>();

            foreach (AppSpecificTableGroupModel4WS asitGroup in groupModel)
            {
                if (asitGroup == null || asitGroup.tablesMapValues == null)
                {
                    continue;
                }

                foreach (AppSpecificTableModel4WS table in asitGroup.tablesMapValues)
                {
                    if (table.columns == null)
                    {
                        continue;
                    }

                    foreach (AppSpecificTableColumnModel4WS column in table.columns)
                    {
                        column.defaultValue = string.Empty;
                        column.resDefaultValue = string.Empty;

                        if (searchs == null)
                        {
                            continue;
                        }

                        AppSpecificInfoModel4WS asiInfoModel = searchs.ToArray().FirstOrDefault(
                            f => f.groupCode == column.groupName && f.checkboxDesc == column.columnName
                                    && f.checkboxType == column.tableName
                                    && f.fieldType == column.columnType);

                        if (asiInfoModel != null)
                        {
                            column.defaultValue = asiInfoModel.checklistComment;
                            column.resDefaultValue = asiInfoModel.resChecklistComment;
                        }
                    }

                    asiTables.AddRange(asitGroup.tablesMapValues);
                }
            }

            uiData = ASITUIModelUtil.ConvertASITablesToUITables(asiTables.ToArray(), ModuleName, false, string.Empty, true);

            List<ASITUITable> dTables = new List<ASITUITable>();

            foreach (ASITUITable asituiTable in uiData)
            {
                ASITUITable dTable = ASITUIModelUtil.CreateNewRow4ASITUITable(asituiTable, 1, string.Empty);
                dTables.Add(dTable);
            }

            TempASITGroupInfo = dTables.ToArray();
        }

        /// <summary>
        /// get a array  to contain all AppSpecificInfoGroupModel4WS models from web.
        /// </summary>
        /// <returns>AppSpecificInfoGroupModel4WS array</returns>
        public AppSpecificTableGroupModel4WS[] GetAppSpecTable()
        {
            if (TempASITGroupInfo == null)
            {
                return null;
            }

            AppSpecificTableGroupModel4WS appSpecTableGroup = new AppSpecificTableGroupModel4WS();
            List<AppSpecificInfoModel4WS> searchInfoModels = new List<AppSpecificInfoModel4WS>();

            string fieldValue = string.Empty;
            foreach (ASITUITable uiTable in TempASITGroupInfo)
            {
                if (uiTable.Rows.Count == 0 || uiTable.Rows[0].Fields == null)
                {
                    continue;
                }

                foreach (var uiField in uiTable.Rows[0].Fields)
                {
                    ASITUIField asitField = uiField as ASITUIField;

                    if (asitField == null)
                    {
                        continue;
                    }

                    fieldValue = GetAppSpecTableFieldValue(asitField);

                    if (string.IsNullOrEmpty(fieldValue))
                    {
                        continue;
                    }

                    AppSpecificInfoModel4WS asiInfoModel = new AppSpecificInfoModel4WS();

                    asiInfoModel.groupCode = uiTable.GroupName;
                    asiInfoModel.checkboxDesc = asitField.Name;
                    asiInfoModel.checkboxType = uiTable.TableName;
                    asiInfoModel.fieldType = asitField.Type;
                    asiInfoModel.checklistComment = fieldValue;
                    searchInfoModels.Add(asiInfoModel);
                }
            }

            appSpecTableGroup.searchInfoModels = searchInfoModels.ToArray();

            return new[] { appSpecTableGroup };
        }

        /// <summary>
        /// clear ASIT fields controls.
        /// </summary>
        public void ClearControls()
        {
            phASITSearchForm.Controls.Clear();
        }

        /// <summary>
        /// Get flag for whether empty search criteria.
        /// </summary>
        /// <returns>true / false</returns>
        public bool IsEmptySearchCriteria()
        {
            bool isEmptySearchCriteria = true;

            if (_allControl.Count > 0)
            {
                foreach (IAccelaControl accelaControl in _allControl)
                {
                    if (accelaControl is CheckBox)
                    {
                        isEmptySearchCriteria = !(accelaControl as CheckBox).Checked;
                    }
                    else
                    {
                        isEmptySearchCriteria = string.IsNullOrEmpty(accelaControl.GetValue().Trim());
                    }

                    if (!isEmptySearchCriteria)
                    {
                        break;
                    }
                }
            }

            return isEmptySearchCriteria;
        }

        /// <summary>
        /// Raises the page load event
        /// </summary>
        /// <param name="sender">An object that contains the event sender.</param>
        /// <param name="e">A System.EventArgs object containing the event data.</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (TempASITGroupInfo != null && TempASITGroupInfo.Length > 0)
            {
                BindUIControls(null);
            }
        }

        /// <summary>
        /// Get the value for a specified control
        /// </summary>
        /// <param name="file">ASIT UI Fields</param>
        /// <returns>AppSpecTableField Value</returns>
        private string GetAppSpecTableFieldValue(ASITUIField file)
        {
            string value = string.Empty;

            foreach (IAccelaControl accelaControl in _allControl)
            {
                if (accelaControl == null)
                {
                    continue;
                }

                if (((WebControl)accelaControl).ID != file.FieldID)
                {
                    continue;
                }

                if (accelaControl is AccelaNumberText)
                {
                    value = (accelaControl as AccelaNumberText).GetInvariantDoubleText();
                }
                else if (accelaControl is TextBox)
                {
                    TextBox textBox = accelaControl as TextBox;

                    if (accelaControl is AccelaCalendarText)
                    {
                        value = I18nDateTimeUtil.ConvertDateStringFromUIToWebService(textBox.Text.Trim());
                    }
                    else
                    {
                        string strValue = textBox.Text.Trim();

                        //if text area and string length over 4000, it need limit as 4000.
                        value = textBox.Rows > 1 && strValue.Length >= MAX_LENGTH ? strValue.Substring(0, MAX_LENGTH - 1) : strValue;
                    }
                }
                else if (accelaControl is AccelaDropDownList)
                {
                    value = accelaControl.GetValue();
                }
                else if (accelaControl is AccelaCheckBox)
                {
                    value = (accelaControl as CheckBox).Checked ? "CHECKED" : string.Empty;
                }
                else if (accelaControl is RadioButtonList)
                {
                    ListItem item = (accelaControl as RadioButtonList).SelectedItem;
                    value = item == null ? string.Empty : item.Value;
                }

                break;
            }

            return value;
        }

        /// <summary>
        /// create ASIT table
        /// </summary>
        /// <param name="uiData">ASIT UI Table</param>
        private void CreateASITable(ASITUITable[] uiData)
        {
            phASITSearchForm.Controls.Clear();

            if (uiData == null)
            {
                return;
            }

            phASITSearchForm.Controls.Add(new LiteralControl("<table role='presentation' style=\"width:100%;border-collapse: collapse;\">"));
            int groupIndex = 0;

            foreach (ASITUITable sTable in uiData)
            {
                if (sTable.Rows == null || sTable.Rows.Count == 0
                    || sTable.Rows[0].Fields == null || sTable.Rows[0].Fields.Count == 0)
                {
                    continue;
                }

                StringBuilder groupHeader = new StringBuilder();
                groupHeader.Append("<tr>");
                groupHeader.Append("<td>");
                groupHeader.Append("<table role='presentation'><tr><td class='ACA_Title_Text font13px'  valign=\"middle\">");
                groupHeader.Append(StringUtil.GetString(sTable.AlternativeLabel, sTable.TableTitle));
                groupHeader.Append("</td></tr></table>");

                phASITSearchForm.Controls.Add(new LiteralControl(groupHeader.ToString()));

                //loop each field in a group and create corresponding controls
                phASITSearchForm.Controls.Add(new LiteralControl("<table role='presentation' class='ACA_TDAlignLeftOrRightTop' style=\"border-collapse: collapse;\">"));

                // loop for columns
                foreach (ASITUIField field in sTable.Rows[0].Fields)
                {
                    field.IsRequired = false;

                    if (field.IsHidden)
                    {
                        continue;
                    }

                    WebControl control = ControlBuildHelper.ASIT.CreateWebControl(field, false, true);

                    if (!(control is IAccelaControl))
                    {
                        continue;
                    }

                    if (int.Parse(field.Type) == (int)FieldType.HTML_SELECTBOX)
                    {
                        AccelaDropDownList ddl = (AccelaDropDownList)control;
                        DropDownListBindUtil.SetSelectedValue(ddl, field.Value);
                    }
                    
                    (control as IAccelaControl).LayoutType = ControlLayoutType.Horizontal;
                    (control as IAccelaControl).LabelWidth = StandardChoiceUtil.GetASILabelWidth();

                    _allControl.Add(control);

                    phASITSearchForm.Controls.Add(new LiteralControl("<tr><td>" +
                                                                     "<table role='presentation' attr='parent_table' style='margin-bottom:5px;'><tr><td>"));
                    phASITSearchForm.Controls.Add(control);
                    phASITSearchForm.Controls.Add(new LiteralControl("</td></tr></table>" +
                                                                     "</td></tr>"));
                }

                phASITSearchForm.Controls.Add(new LiteralControl("</table>"));

                if (uiData.Length > 1 && groupIndex < uiData.Length - 1 && uiData[groupIndex].Rows[0].Fields != null)
                {
                    phASITSearchForm.Controls.Add(new LiteralControl("<div class=\"ACA_TabRow ACA_Line_Content\">&nbsp;</div>"));
                }

                phASITSearchForm.Controls.Add(new LiteralControl("</td></tr>"));
                groupIndex++;
            }

            phASITSearchForm.Controls.Add(new LiteralControl("</table>"));
        }
    }
}