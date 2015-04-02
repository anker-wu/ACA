#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: ASISearchForm.ascx.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2009-2014
 *
 *  Description: Search by ASI
 *
 *  Notes:
 *      $Id: ASISearchForm.ascx.cs 277910 2014-08-22 09:09:15Z ACHIEVO\james.shi $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using System;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using Accela.ACA.BLL.Cap;
using Accela.ACA.Common;
using Accela.ACA.Common.Util;
using Accela.ACA.Web.Common;
using Accela.ACA.Web.Common.Control;
using Accela.ACA.Web.ExpressionBuild;
using Accela.ACA.WSProxy;
using Accela.Web.Controls;
using AjaxControlToolkit;

namespace Accela.ACA.Web.Component
{
    /// <summary>
    /// the class for ASISearchForm
    /// </summary>
    public partial class ASISearchForm : ASIBaseUC
    {
        #region Properties

        /// <summary>
        /// Gets or sets ASI group info when expand/collapse ASI search form.
        /// </summary>
        public AppSpecificInfoGroupModel4WS[] TempASIGroupInfo
        {
            get
            {
                return ViewState["TempASIGroupInfo"] as AppSpecificInfoGroupModel4WS[];
            }

            set
            {
                ViewState["TempASIGroupInfo"] = value;
            }
        }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Bind data to UI controls
        /// </summary>
        /// <param name="capType">CapType Model</param>
        public void BindPlumbingInfo(CapTypeModel capType)
        {
            try
            {
                //if temp ASI group info is not null, bind plumbing by viewState ASI group.
                if (TempASIGroupInfo != null)
                {
                    ShowAppSpecInfoGroup(TempASIGroupInfo);
                    return;
                }

                if (capType == null)
                {
                    ClearControls();
                    return;
                }

                IAppSpecificInfoBll asiBll = ObjectFactory.GetObject<IAppSpecificInfoBll>();
                Array groupList = asiBll.GetRefSearchableAppSpecInfoFieldList(capType);

                //Display message by empty ASI.
                if (groupList == null)
                {
                    ClearControls();
                    return;
                }

                string agencyCode = string.IsNullOrEmpty(capType.serviceProviderCode) ? ConfigManager.AgencyCode : capType.serviceProviderCode;
                AppSpecificInfoGroupModel4WS[] appSpecInfoGroup = ConVertRefAppSpecInfoModel(groupList, agencyCode);
                SortASIByAgencyAndGroup(appSpecInfoGroup);
                TempASIGroupInfo = appSpecInfoGroup;
                ShowAppSpecInfoGroup(appSpecInfoGroup);
            }
            catch (ACAException e)
            {
                MessageUtil.ShowMessage(Page, MessageType.Error, e.Message);
            }
        }

        /// <summary>
        /// clear ASI fields controls.
        /// </summary>
        public void ClearControls()
        {
            AllControls.Clear();
            phPlumbingGroup.Controls.Clear();
        }

        /// <summary>
        /// Construct a array  to contain all AppSpecificInfoGroupModel4WS models from web.
        /// </summary>
        /// <returns>AppSpecificInfoGroupModel4WS array</returns>
        public AppSpecificInfoGroupModel4WS[] GetAppSpecInfo()
        {
            int group = 0;
            int field = 0;

            if (TempASIGroupInfo == null)
            {
                return null;
            }

            AppSpecificInfoGroupModel4WS[] appSpecInfoGroups = TempASIGroupInfo;

            foreach (AppSpecificInfoGroupModel4WS groupModel in appSpecInfoGroups)
            {
                //if group doesn't have any field
                if (groupModel.fields == null)
                {
                    continue;
                }

                field = 0; //init the field index in the group
                string callerID = AppSession.User.PublicUserId;

                foreach (AppSpecificInfoModel4WS appSpecInfoModel in groupModel.fields)
                {
                    if (appSpecInfoModel == null)
                    {
                        continue;
                    }

                    appSpecInfoModel.attributeValue = GetAppSpecInfoFieldValue(group, field);
                    appSpecInfoModel.resAttributeValue = GetAppSpecInfoFieldValue(group, field);
                    appSpecInfoModel.checklistComment = GetAppSpecInfoFieldValue(group, field);
                    appSpecInfoModel.resChecklistComment = GetAppSpecInfoFieldLabel(group, field);
                    appSpecInfoModel.auditid = callerID;
                    appSpecInfoModel.actStatus = groupModel.groupCode;
                    appSpecInfoModel.checkboxDesc = appSpecInfoModel.fieldLabel;
                    field++;
                }

                group++;
            }

            return appSpecInfoGroups;
        }

        /// <summary>
        /// Get flag for whether empty search criteria.
        /// </summary>
        /// <returns>true / false</returns>
        public bool IsEmptySearchCriteria()
        {
            bool isEmptySearchCriteria = true;

            if (phPlumbingGroup.Controls != null && phPlumbingGroup.Controls.Count > 0)
            {
                foreach (Control control in phPlumbingGroup.Controls)
                {
                    if (control is TextBox)
                    {
                        isEmptySearchCriteria = string.IsNullOrEmpty((control as TextBox).Text.Trim());
                    }
                    else if (control is DropDownList)
                    {
                        isEmptySearchCriteria = string.IsNullOrEmpty((control as DropDownList).SelectedValue);
                    }
                    else if (control is CheckBox)
                    {
                        isEmptySearchCriteria = !(control as CheckBox).Checked;
                    }
                    else if (control is RadioButtonList)
                    {
                        isEmptySearchCriteria = string.IsNullOrEmpty((control as RadioButtonList).SelectedValue);
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
            InitDrillDown();

            //bind ASI group from ViewState.
            if (TempASIGroupInfo != null && TempASIGroupInfo.Length > 0)
            {
                BindPlumbingInfo(null);
            }

            IsASISearchForm = true;
        }

        /// <summary>
        /// Convert RefAppSpecInfoModel
        /// </summary>
        /// <param name="refAppSpecInfoGroups">refAppSpecInfoGroups array</param>
        /// <param name="agencyCode">The agency code.</param>
        /// <returns>AppSpecificInfoGroupModel array</returns>
        private static AppSpecificInfoGroupModel4WS[] ConVertRefAppSpecInfoModel(Array refAppSpecInfoGroups, string agencyCode)
        {
            if (refAppSpecInfoGroups == null)
            {
                return null;
            }

            int group = 0;
            int field = 0;
            AppSpecificInfoGroupModel4WS[] appSpecInfoGroups = null;

            appSpecInfoGroups = new AppSpecificInfoGroupModel4WS[refAppSpecInfoGroups.Length];

            foreach (RefAppSpecInfoGroupModel4WS groupModel in refAppSpecInfoGroups)
            {
                if (groupModel == null)
                {
                    continue;
                }

                if (groupModel.fieldList == null)
                {
                    continue;
                }

                field = 0; //init the field index in the group
                AppSpecificInfoGroupModel4WS appSpecInfoGroupModel = new AppSpecificInfoGroupModel4WS();

                AppSpecificInfoModel4WS[] appSpecInfoFields = new AppSpecificInfoModel4WS[groupModel.fieldList.Length];

                foreach (RefAppSpecInfoFieldModel4WS specInfofield in groupModel.fieldList)
                {
                    //Copy values to the newly-contructed appSpecInfoModel
                    AppSpecificInfoModel4WS appSpecInfoModel = new AppSpecificInfoModel4WS();
                    appSpecInfoModel.auditid = specInfofield.auditID;
                    appSpecInfoModel.auditStatus = specInfofield.auditStatus;
                    appSpecInfoModel.BGroupDspOrder = specInfofield.belongGroupOrder;
                    appSpecInfoModel.checkboxDesc = I18nStringUtil.GetString(specInfofield.resFieldLabel, specInfofield.fieldLabel);
                    appSpecInfoModel.checkboxInd = specInfofield.fieldType + string.Empty;
                    appSpecInfoModel.actStatus = groupModel.groupCode;
                    appSpecInfoModel.groupCode = groupModel.groupCode;
                    appSpecInfoModel.checkboxType = specInfofield.checkboxType;

                    appSpecInfoModel.displayLength = specInfofield.displayLength.ToString();
                    appSpecInfoModel.displayOrder = long.Parse(specInfofield.displayOrder);
                    appSpecInfoModel.feeIndicator = specInfofield.feeIndicator;
                    appSpecInfoModel.fieldLabel = specInfofield.fieldLabel;
                    appSpecInfoModel.resFiledLabel = specInfofield.resFieldLabel;
                    appSpecInfoModel.alternativeLabel = specInfofield.alternativeLabel;
                    appSpecInfoModel.resAlternativeLabel = specInfofield.resAlternativeLabel;
                    appSpecInfoModel.instruction = specInfofield.instruction;
                    appSpecInfoModel.resInstruction = specInfofield.resInstruction;
                    appSpecInfoModel.waterMark = specInfofield.waterMark;
                    appSpecInfoModel.resWaterMark = specInfofield.resWaterMark;
                    appSpecInfoModel.fieldType = specInfofield.fieldType.ToString();
                    appSpecInfoModel.maxLength = specInfofield.maxLength.ToString();
                    appSpecInfoModel.unit = specInfofield.unit;
                    appSpecInfoModel.validationScriptName = specInfofield.validationScriptName;
                    appSpecInfoModel.alignment = specInfofield.alignment;
                    appSpecInfoModel.serviceProviderCode = groupModel.serviceProviderCode;

                    if (specInfofield.valueList != null)
                    {
                        appSpecInfoModel.valueList = specInfofield.valueList;
                    }

                    appSpecInfoModel.vchDispFlag = specInfofield.vchDispFlag;
                    appSpecInfoModel.requiredFeeCalc = specInfofield.requiredFeeCalc;

                    appSpecInfoFields[field] = appSpecInfoModel;

                    field++;
                }

                appSpecInfoGroupModel.capID = new CapIDModel4WS();
                appSpecInfoGroupModel.capID.serviceProviderCode = agencyCode;
                appSpecInfoGroupModel.checkBoxDesc = null;
                appSpecInfoGroupModel.checkBoxGroup = groupModel.checkboxGroup;
                appSpecInfoGroupModel.checkBoxInd = groupModel.checkboxGroupOrder;
                appSpecInfoGroupModel.fields = appSpecInfoFields;
                appSpecInfoGroupModel.groupCode = groupModel.groupCode;
                appSpecInfoGroupModel.groupName = groupModel.groupName;
                appSpecInfoGroupModel.resGroupName = groupModel.resGroupName;
                appSpecInfoGroupModel.tableGroupName = groupModel.tableGroupName;
                appSpecInfoGroupModel.alternativeLabel = groupModel.alternativeLabel;
                appSpecInfoGroupModel.resAlternativeLabel = groupModel.resAlternativeLabel;
                appSpecInfoGroups[group] = appSpecInfoGroupModel;

                group++;
            }

            return appSpecInfoGroups;
        }

        /// <summary>
        /// Get the filed text for a specified control
        /// </summary>
        /// <param name="group">ASI group.</param>
        /// <param name="field">ASI fields.</param>
        /// <returns>AppSpecInfoField Label</returns>
        private string GetAppSpecInfoFieldLabel(int group, int field)
        {
            string result = string.Empty;

            foreach (Control control in this.phPlumbingGroup.Controls)
            {
                if ((control as TextBox == null) && (control as DropDownList == null) && (control as CheckBox == null) && (control as RadioButtonList == null))
                {
                    continue;
                }

                //Get the corresponding group index in group list from the id of a control
                string[] idParts = control.ID.Split(ExpressionFactory.SPLIT_CHAR);
                int groupIndex = -1;
                int fieldIndex = -1;

                //Control ID's format is "label name+ExpressionFactory.SPLIT_CHAR+groupIndex+ExpressionFactory.SPLIT_CHAR+fieldIndex";
                if (idParts.Length > 2)
                {
                    if (ValidationUtil.IsInt(idParts[idParts.Length - 2]))
                    {
                        groupIndex = int.Parse(idParts[idParts.Length - 2]);
                    }

                    if (ValidationUtil.IsInt(idParts[idParts.Length - 1]))
                    {
                        fieldIndex = int.Parse(idParts[idParts.Length - 1]);
                    }
                }

                if ((group == groupIndex) && (field == fieldIndex))
                {
                    if (control is AccelaNumberText)
                    {
                        result = (control as AccelaNumberText).GetInvariantDoubleText();
                    }
                    else if ((control as TextBox) != null)
                    {
                        TextBox textBox = control as TextBox;
                        string strValue = textBox.Text.Trim();

                        //if text area and string length over 4000, it need limit as 4000.
                        result = textBox.Rows > 1 && strValue.Length >= MAX_LENGTH_4K ? strValue.Substring(0, MAX_LENGTH_4K - 1) : strValue;
                    }
                    else if ((control as DropDownList) != null)
                    {
                        if (!string.IsNullOrEmpty((control as DropDownList).SelectedItem.Value))
                        {
                            result = (control as DropDownList).SelectedItem.Text;
                        }
                    }
                    else if ((control as CheckBox) != null)
                    {
                        result = (control as CheckBox).Checked ? GetTextByKey("ACA_RadioButtonText_Yes") : GetTextByKey("ACA_RadioButtonText_No");
                    }
                    else if ((control as RadioButtonList) != null)
                    {
                        ListItem item = (control as RadioButtonList).SelectedItem;

                        if (item != null && !string.IsNullOrEmpty(item.Value))
                        {
                            if (ValidationUtil.IsNo(item.Value))
                            {
                                result = GetTextByKey("ACA_RadioButtonText_No");
                            }
                            else
                            {
                                result = GetTextByKey("ACA_RadioButtonText_Yes");
                            }
                        }
                    }

                    break;
                }
            }

            return result;
        }

        /// <summary>
        /// Get the value for a specified control
        /// </summary>
        /// <param name="group">ASI group.</param>
        /// <param name="field">ASI fields.</param>
        /// <returns>AppSpecInfoField Value</returns>
        private string GetAppSpecInfoFieldValue(int group, int field)
        {
            string value = string.Empty;

            foreach (Control control in this.phPlumbingGroup.Controls)
            {
                if ((control as TextBox == null) && (control as DropDownList == null) && (control as CheckBox == null) && (control as RadioButtonList == null))
                {
                    continue;
                }

                string[] idParts = control.ID.Split(ExpressionFactory.SPLIT_CHAR);
                int groupIndex = -1;
                int fieldIndex = -1;

                //Control ID's format is "label name+ExpressionFactory.SPLIT_CHAR+groupIndex+ExpressionFactory.SPLIT_CHAR+fieldIndex";
                if (idParts.Length > 2)
                {
                    if (ValidationUtil.IsInt(idParts[idParts.Length - 2]))
                    {
                        groupIndex = int.Parse(idParts[idParts.Length - 2]);
                    }

                    if (ValidationUtil.IsInt(idParts[idParts.Length - 1]))
                    {
                        fieldIndex = int.Parse(idParts[idParts.Length - 1]);
                    }
                }   

                if ((group == groupIndex) && (field == fieldIndex))
                {
                    if (control is AccelaNumberText)
                    {
                        value = (control as AccelaNumberText).GetInvariantDoubleText();
                    }
                    else if ((control as TextBox) != null)
                    {
                        TextBox textBox = control as TextBox;

                        if (control is AccelaCalendarText)
                        {
                            value = I18nDateTimeUtil.ConvertDateStringFromUIToWebService(textBox.Text.Trim());
                        }
                        else
                        {
                            string strValue = textBox.Text.Trim();

                            //if text area and string length over 4000, it need limit as 4000.
                            value = textBox.Rows > 1 && strValue.Length >= MAX_LENGTH_4K ? strValue.Substring(0, MAX_LENGTH_4K - 1) : strValue;
                        }
                    }
                    else if ((control as DropDownList) != null)
                    {
                        value = (control as DropDownList).SelectedValue;
                    }
                    else if ((control as CheckBox) != null)
                    {
                        value = (control as CheckBox).Checked ? "CHECKED" : string.Empty;
                    }
                    else if ((control as RadioButtonList) != null)
                    {
                        ListItem item = (control as RadioButtonList).SelectedItem;
                        value = item == null ? string.Empty : item.Value;
                    }

                    break;
                }
            }

            return value;
        }

        /// <summary>
        /// Show Application specific information on UI
        /// </summary>
        /// <param name="appSpecInfoGroup">appSpecInfoGroup array</param>
        private void ShowAppSpecInfoGroup(Array appSpecInfoGroup)
        {
            if (appSpecInfoGroup == null || appSpecInfoGroup.Length == 0)
            {
                return;
            }

            int groupIndex = 0; //indicates which group a field belongs to
            AllControls.Clear();
            phPlumbingGroup.Controls.Clear();
            phPlumbingGroup.Controls.Add(new LiteralControl("<table role='presentation' style=\"width:100%;border-collapse: collapse;\">"));

            ASITableDrillDownModel4WS[] asiDrillDownModels = null;
            string prevAgencyCode = string.Empty;
            string prevASIGroup = string.Empty;

            //appSpecInfoGroup need be sorted by agency.
            foreach (AppSpecificInfoGroupModel4WS specificInfo in appSpecInfoGroup)
            {
                AppSpecificInfoGroupModel4WS model = specificInfo;

                //If a group has no fields
                if (model == null || model.fields == null)
                {
                    continue;
                }
                
                string agencyCode = model.capID == null ? ConfigManager.AgencyCode : model.capID.serviceProviderCode;

                //System gets a drill down list associated to current ASI group 
                //when agency isn't the same agency as prev ASI sub group's agency.
                if (string.IsNullOrEmpty(prevAgencyCode) || !prevAgencyCode.Equals(agencyCode, StringComparison.InvariantCulture)
                    || (prevAgencyCode.Equals(agencyCode, StringComparison.InvariantCulture) && !prevASIGroup.Equals(model.groupCode, StringComparison.InvariantCulture)))
                {
                    IAppSpecificInfoBll appSpecificInfoBll = (IAppSpecificInfoBll)ObjectFactory.GetObject(typeof(IAppSpecificInfoBll));
                    asiDrillDownModels = appSpecificInfoBll.GetASIDrillDown(agencyCode, model.groupCode);
                }

                prevAgencyCode = agencyCode;
                prevASIGroup = model.groupCode;
                ASIDrillDownUtil asiDrillDownInstance = new ASIDrillDownUtil();

                //get drill down for cur ASI sub group.
                ASITableDrillDownModel4WS curASIDrillDown = asiDrillDownInstance.GetCurASIDrillDown(asiDrillDownModels, model, string.Empty);

                //change ASI data source.
                model = asiDrillDownInstance.ChangeASIDataSource(model, curASIDrillDown, groupIndex);

                StringBuilder subGroup = new StringBuilder();
                subGroup.Append("<tr>");
                subGroup.Append("<td>");
                subGroup.Append("<table role='presentation'><tr><td class='ACA_Title_Text font13px'  valign=\"middle\">");

                //display sub group name.
                subGroup.Append(I18nStringUtil.GetString(model.resAlternativeLabel, model.alternativeLabel, model.resGroupName, model.groupName));
                subGroup.Append("</td></tr></table>");

                phPlumbingGroup.Controls.Add(new LiteralControl(subGroup.ToString()));

                //loop each field in a group and create corresponding controls
                phPlumbingGroup.Controls.Add(new LiteralControl("<table role='presentation' class='ACA_TDAlignLeftOrRightTop' style=\"border-collapse: collapse;\">"));
                
                foreach (AppSpecificInfoModel4WS item in model.fields)
                {
                    WebControl control = CreateWebControl(item, (AppSpecificInfoGroupModel4WS[])appSpecInfoGroup, null, false, curASIDrillDown);
                   
                    if (control == null)
                    {
                        continue;
                    }

                    control.EnableViewState = true;

                    bool isHidden = ValidationUtil.IsHidden(item.vchDispFlag);

                    if (control is IAccelaControl)
                    {
                        // For ASI section, every control is in Horizontal Layout
                        (control as IAccelaControl).LayoutType = ControlLayoutType.Horizontal;
                        (control as IAccelaControl).LabelWidth = StandardChoiceUtil.GetASILabelWidth();

                        // For ASI Section, CheckBox and RadioButton does have its own width.
                        if (!(control is AccelaCheckBox) && !(control is AccelaRadioButtonList))
                        {
                            int width = ControlBuildHelper.ConvertCharWidthToPixWidth(item.displayLength);

                            // Fix DropdownList width
                            if ((control is AccelaDropDownList || control is DropDownList) && width == ControlBuildHelper.DEFAULT_CONTROL_WIDTH)
                            {
                                control.CssClass += " asi_ddl_width";
                            }
                            else
                            {
                                // Use unit EM instead of PX, keep the decimal
                                control.Style.Add("width", (width / 10.0) + "em");
                            }
                        }

                        // For ASI Section, maybe some fields have unit type.
                        if (!string.IsNullOrEmpty(item.unit) && !isHidden)
                        {
                            (control as IAccelaControl).FieldUnit = I18nStringUtil.GetString(item.resAttributeUnitType, item.unit);
                        }
                    }

                    phPlumbingGroup.Controls.Add(new LiteralControl("<tr><td>"));

                    if (isHidden)
                    {
                        control.Attributes.Add(ACAConstant.IS_HIDDEN, "true");
                    }

                    phPlumbingGroup.Controls.Add(new LiteralControl("<table role='presentation' attr='parent_table' style='margin-bottom:5px;'><tr><td>"));
                    phPlumbingGroup.Controls.Add(control);
                    phPlumbingGroup.Controls.Add(new LiteralControl("</td></tr></table>"));

                    if (int.Parse(item.fieldType) == (int)FieldType.HTML_RADIOBOX
                        && ValidationUtil.IsYes(item.requiredFlag)
                        && !isHidden)
                    {
                        RadioButtonListRequiredFieldValidator reqValidator = new RadioButtonListRequiredFieldValidator();
                        reqValidator.ID = control.ID + "_req";
                        reqValidator.Display = ValidatorDisplay.None;
                        reqValidator.ControlToValidate = control.ID;
                        reqValidator.SetFocusOnError = true;

                        ValidatorCallbackExtender reqValidatorExt = new ValidatorCallbackExtender();
                        reqValidatorExt.ID = control.ID + "_req_ext";
                        reqValidatorExt.TargetControlID = reqValidator.ID;
                        reqValidatorExt.CallbackFailFunction = "doErrorCallbackFun";
                        reqValidatorExt.CallbackControlID = control.ID;
                        reqValidatorExt.HighlightCssClass = "HighlightCssClass";

                        phPlumbingGroup.Controls.Add(reqValidator);
                        phPlumbingGroup.Controls.Add(reqValidatorExt);
                    }

                    phPlumbingGroup.Controls.Add(new LiteralControl("</td></tr>"));    
                }
               
                phPlumbingGroup.Controls.Add(new LiteralControl("</table>"));

                if (((AppSpecificInfoGroupModel4WS[])appSpecInfoGroup)[groupIndex].fields != null)
                {
                    this.phPlumbingGroup.Controls.Add(new LiteralControl("<div class=\"ACA_TabRow ACA_Line_Content\">&nbsp;</div>"));
                }

                phPlumbingGroup.Controls.Add(new LiteralControl("</td></tr>"));

                groupIndex++;
            }

            phPlumbingGroup.Controls.Add(new LiteralControl("</table>"));
        }

        #endregion Methods
    }
}
