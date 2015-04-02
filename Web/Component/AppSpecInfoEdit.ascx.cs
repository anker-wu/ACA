#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: AppSpecInfoEdit.ascx.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2008-2014
 *
 *  Description:
 *
 *  Notes:
 *      $Id: AppSpecInfoEdit.ascx.cs 277585 2014-08-18 11:09:47Z ACHIEVO\james.shi $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Accela.ACA.BLL.Cap;
using Accela.ACA.BLL.Common;
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
    /// the class for AppSpecInfoEdit.
    /// </summary>
    public partial class AppSpecInfoEdit : ASIBaseUC
    {
        #region Fields

        /// <summary>
        /// the default length of control
        /// </summary>
        private const string CONTROL_LIST = "ControlList";

        /// <summary>
        /// indicates which group a field belongs to
        /// </summary>
        private int groupIndex = 0; 

        #endregion Fields

        #region Properties

        /// <summary>
        /// Gets or sets control list.
        /// </summary>
        public Hashtable ControlList
        {
            get
            {
                Hashtable ht = ViewState[CONTROL_LIST] as Hashtable;

                if (ht == null)
                {
                    ht = new Hashtable();
                }

                return ht;
            }

            set
            {
                ViewState[CONTROL_LIST] = value;
            }
        }

        /// <summary>
        /// Gets or sets focus click id.
        /// </summary>
        public string SkippingToParentClickID
        {
            get
            {
                return ViewState["SkippingToParentClickID"] as string;
            }

            set
            {
                ViewState["SkippingToParentClickID"] = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this control is displayed in the confirm page
        /// </summary>
        public bool IsInConfirmPage
        {
            get
            {
                return Convert.ToBoolean(ViewState["IsInConfirmPage"]);
            }

            set
            {
                ViewState["IsInConfirmPage"] = value;
            }
        }

        /// <summary>
        /// Gets or sets the ASI group model list.
        /// </summary>
        /// <value>The ASI group model list.</value>
        public AppSpecificInfoGroupModel4WS[] ASIGroupModelList
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets a value indicating whether whether Is for Fee Estimator or not.
        /// is4FeeEstimator is true,App Spec Info only display Fee Cal required Fields.added by Jesse.Zhao,
        /// </summary>
        public bool Is4FeeEstimator 
        { 
            get; 
            set;
        }

        /// <summary>
        /// Gets or sets a value indicating whether Is Convert to App or not.
        /// </summary>
        public bool IsConvertToApp 
        { 
            get;
            set;
        }

        #endregion Properties

        #region Methods

        /// <summary>
        /// get App Spec Info group which existed fee calculation fields.
        /// </summary>
        /// <param name="appSpecInfoGroup">appSpecInfoGroup array</param>
        /// <returns>ASIGroupModel Array.</returns>
        public static AppSpecificInfoGroupModel4WS[] GetFeeCalculationFields(AppSpecificInfoGroupModel4WS[] appSpecInfoGroup)
        {
            List<AppSpecificInfoGroupModel4WS> appSpecInfoGroupList = new List<AppSpecificInfoGroupModel4WS>();

            foreach (AppSpecificInfoGroupModel4WS model in appSpecInfoGroup)
            {
                //If a group has no fields
                if (model.fields == null)
                {
                    continue;
                }

                List<AppSpecificInfoModel4WS> fieldsList = new List<AppSpecificInfoModel4WS>();
                bool isExistedFeeRequiredField = false;
                foreach (AppSpecificInfoModel4WS item in model.fields)
                {
                    if (item == null)
                    {
                        continue;
                    }

                    if ((ValidationUtil.IsYes(item.requiredFeeCalc) || !string.IsNullOrEmpty(item.feeIndicator)) && ValidationUtil.IsYes(item.vchDispFlag))
                    {
                        isExistedFeeRequiredField = true;
                        fieldsList.Add(item);
                    }
                }

                if (isExistedFeeRequiredField)
                {
                    model.fields = fieldsList.ToArray();
                    appSpecInfoGroupList.Add(model);
                }
            }

            return appSpecInfoGroupList.ToArray();
        }

        /// <summary>
        /// Assign values to UI controls from a session object
        /// </summary>
        public void Display()
        {
            try
            {
                BindPlumbingInfo();

                if (ControlList.Count == 0)
                {
                    return;
                }

                //assing values to corresponding controls on UI
                foreach (string id in ControlList.Keys)
                {
                    WebControl control = (WebControl)FindControl(id);

                    if (control == null)
                    {
                        continue;
                    }

                    if ((control as TextBox) != null)
                    {
                        string textBoxValue = ControlList[id].ToString();

                        if (control is AccelaCalendarText)
                        {
                            textBoxValue = I18nDateTimeUtil.ConvertDateStringFromWebServiceToUI(textBoxValue);
                        }

                        (control as TextBox).Text = textBoxValue;
                        continue;
                    }

                    if ((control as AccelaDropDownList) != null)
                    {
                        DropDownListBindUtil.SetSelectedValue(control as AccelaDropDownList, ControlList[id].ToString());
                        continue;
                    }

                    if ((control as CheckBox) != null)
                    {
                        (control as CheckBox).Checked = ControlList[id].ToString() == "CHECKED" ? true : false;
                        continue;
                    }

                    if ((control as RadioButtonList) != null)
                    {
                        if (ControlList[id].ToString() != string.Empty)
                        {
                            (control as RadioButtonList).SelectedValue = ControlList[id].ToString();
                        }

                        continue;
                    }
                }
            }
            catch (ACAException e)
            {
                MessageUtil.ShowMessage(Page, MessageType.Error, e.Message);
            }
        }

        /// <summary>
        /// Construct a array to contain all AppSpecificInfoGroupModel4WS models
        /// </summary>
        /// <param name="capModel">Cap Model</param>
        /// <param name="appSpecInfoGroups">appSpecInfoGroup array</param>
        public void SaveAppSpecInfo(CapModel4WS capModel, AppSpecificInfoGroupModel4WS[] appSpecInfoGroups)
        {
            int group = 0;
            int field = 0;

            if (appSpecInfoGroups == null)
            {
                return;
            }

            AppSpecificInfoGroupModel4WS[] tempAppSpecInfoGroups = appSpecInfoGroups;
            
            foreach (AppSpecificInfoGroupModel4WS groupModel in tempAppSpecInfoGroups)
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

                    string controlIdPrefix;

                    switch (int.Parse(appSpecInfoModel.fieldType))
                    {
                        case (int)FieldType.HTML_RADIOBOX:
                            controlIdPrefix = "rdo";
                            break;
                        case (int)FieldType.HTML_SELECTBOX:
                            controlIdPrefix = ACAConstant.DROPDOWNLIST_CONTROLID_PREFIXION;
                            break;
                        case (int)FieldType.HTML_TEXTAREABOX:
                            controlIdPrefix = "txt";
                            break;
                        case (int)FieldType.HTML_CHECKBOX:
                            controlIdPrefix = "chk";
                            break;
                        default:
                            controlIdPrefix = "txt";
                            break;
                    }

                    string controlId = appSpecInfoModel.serviceProviderCode + ExpressionFactory.SPLIT_CHAR + controlIdPrefix + ExpressionFactory.SPLIT_CHAR + group + ExpressionFactory.SPLIT_CHAR + field;

                    Control control = phPlumbingGroup.FindControl(controlId);

                    //Get the default value for each field from UI
                    appSpecInfoModel.resChecklistComment = GetAppSpecInfoFieldLabel(control);
                    appSpecInfoModel.checklistComment = GetAppSpecInfoFieldValue(control);
                    appSpecInfoModel.auditid = callerID;
                    appSpecInfoModel.actStatus = groupModel.groupCode;
                    appSpecInfoModel.checkboxDesc = appSpecInfoModel.fieldLabel;

                    field++;
                }

                // change the CapModel's appSpecificInfoGroups
                for (int i = 0; i < capModel.appSpecificInfoGroups.Length; i++)
                {
                    AppSpecificInfoGroupModel4WS asiGroupModel = capModel.appSpecificInfoGroups[i];

                    if (asiGroupModel.groupCode == groupModel.groupCode && asiGroupModel.groupName == groupModel.groupName &&
                        asiGroupModel.capID != null && asiGroupModel.capID.Equals(groupModel.capID))
                    {
                        capModel.appSpecificInfoGroups[i] = groupModel;

                        break;
                    }
                }

                group++;
            }
        }

        /// <summary>
        /// Raises the page load event
        /// </summary>
        /// <param name="sender">An object that contains the event sender.</param>
        /// <param name="e">A System.EventArgs object containing the event data.</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            InitDrillDown();
            hlEnd.NextControlClientID = SkippingToParentClickID;

            //Only SPEAR Form and at firstly load page, it need trig drill down onchange event.
            if (!this.IsPostBack && !IsInConfirmPage)
            {
                ScriptManager.RegisterStartupScript(this, typeof(Page), "TrigDrillDownOnChange", "TrigDrillDownOnChange();", true);
            }
        }

        /// <summary>
        /// copy item value by filed name.
        /// </summary>
        /// <param name="source">AppSpecificInfoGroup source.</param>
        /// <param name="des">AppSpecificInfoGroupModel description.</param>
        private void CopyItemValue(AppSpecificInfoGroupModel4WS[] source, AppSpecificInfoGroupModel4WS[] des)
        {
            if (des == null || source == null)
            {
                return;
            }

            foreach (AppSpecificInfoGroupModel4WS modelSource in source)
            {
                foreach (AppSpecificInfoGroupModel4WS modelDes in des)
                {
                    if (modelSource.groupCode == modelDes.groupCode)
                    {
                        foreach (AppSpecificInfoModel4WS itemSource in modelSource.fields)
                        {
                            foreach (AppSpecificInfoModel4WS itemDes in modelDes.fields)
                            {
                                if (itemSource.fieldLabel == itemDes.fieldLabel)
                                {
                                    itemDes.checklistComment = itemSource.checklistComment;
                                    itemDes.resChecklistComment = itemSource.resChecklistComment;
                                    itemDes.resFiledLabel = itemSource.resFiledLabel;
                                }
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Get the filed text for a specified control
        /// </summary>
        /// <param name="control">Control object for ASI field</param>
        /// <returns>AppSpecInfoField Label</returns>
        private string GetAppSpecInfoFieldLabel(Control control)
        {
            string result = string.Empty;

            if (control == null)
            {
                return result;
            }

            if (control is AccelaNumberText)
            {
                result = (control as AccelaNumberText).GetInvariantDecimalText();
            }
            else if ((control as TextBox) != null)
            {
                result = (control as TextBox).Text.Trim();
            }
            else if ((control as DropDownList) != null)
            {
                if (string.IsNullOrEmpty((control as DropDownList).SelectedItem.Value))
                {
                    result = string.Empty;
                }
                else
                {
                    result = (control as DropDownList).SelectedItem.Text;
                }
            }
            else if ((control as CheckBox) != null)
            {
                CheckBox item = control as CheckBox;

                if (item.Checked)
                {
                    result = ModelUIFormat.FormatYNLabel(ACAConstant.COMMON_Yes);
                }
                else
                {
                    result = ModelUIFormat.FormatYNLabel(ACAConstant.COMMON_No);
                }
            }
            else if ((control as RadioButtonList) != null)
            {
                ListItem item = (control as RadioButtonList).SelectedItem;

                if (item != null && !string.IsNullOrEmpty(item.Value))
                {
                    result = item.Value;
                }
            }

            return result;
        }

        /// <summary>
        /// Get the value for a specified control
        /// </summary>
        /// <param name="control">Control object for ASI field</param>
        /// <returns>AppSpecInfoField Value</returns>
        private string GetAppSpecInfoFieldValue(Control control)
        {
            string value = string.Empty;

            if (control == null)
            {
                return value;
            }

            if (control is AccelaCalendarText)
            {
                value = (control as TextBox).Text.Trim();
                DateTime parsingDate = DateTime.Now;
                bool isDate = I18nDateTimeUtil.TryParseFromUI(value, out parsingDate);

                if (isDate)
                {
                    value = I18nDateTimeUtil.FormatToDateStringForWebService(parsingDate);
                }
                else
                {
                    value = string.Empty;
                }
            }
            else if (control is AccelaNumberText)
            {
                value = (control as AccelaNumberText).GetInvariantDecimalText();
            }
            else if ((control as TextBox) != null)
            {
                value = (control as TextBox).Text.Trim();
            }
            else if ((control as DropDownList) != null)
            {
                value = (control as DropDownList).SelectedValue;
            }
            else if ((control as CheckBox) != null)
            {
                value = (control as CheckBox).Checked ? ACAConstant.COMMON_CHECKED : ACAConstant.COMMON_UNCHECKED;
            }
            else if ((control as RadioButtonList) != null)
            {
                value = Request.Params[control.UniqueID] == null ? string.Empty : Request.Params[control.UniqueID];
            }

            //if we find the control in the control list, save the control value
            if (ControlList.Contains(control.ID))
            {
                ControlList[control.ID] = value;
            }

            return value;
        }

        /// <summary>
        /// get all sub cap models for super agency.It will be used for ASI Expression.
        /// </summary>
        /// <param name="appSpecificInfoGroup">The app specific info group.</param>
        /// <returns>
        /// cap type name.It will be used in create controls.
        /// </returns>
        private string GetSubCapModels(AppSpecificInfoGroupModel4WS appSpecificInfoGroup)
        {
            if (StandardChoiceUtil.IsSuperAgency() && appSpecificInfoGroup != null)
            {
                string key = appSpecificInfoGroup.capID.toKey();

                // some sub ASI group may belong to the same cap model.
                if (!SubCapModels.ContainsKey(key))
                {
                    ICapTypeBll capTypeBll = (ICapTypeBll)ObjectFactory.GetObject(typeof(ICapTypeBll));
                    CapTypeModel capTypeModel = capTypeBll.GetCapTypeByCapID(appSpecificInfoGroup.capID);
                    CapModel4WS capModel = new CapModel4WS();
                    capModel.capID = appSpecificInfoGroup.capID;
                    capModel.capType = capTypeModel;
                    capModel.moduleName = capTypeModel.group;
                    capModel.appSpecificInfoGroups = new AppSpecificInfoGroupModel4WS[1] { appSpecificInfoGroup };
                    CapModel4WS partialCap = AppSession.GetCapModelFromSession(ModuleName);

                    if (partialCap != null)
                    {
                        capModel.auditID = partialCap.auditID;
                    }

                    SubCapModels.Add(key, capModel);
                    return CAPHelper.GetAliasOrCapTypeLabel(capModel.capType);
                }

                // if the ASI belongs to a same cap that has added. find this cap and get it's cap type name.
                foreach (CapModel4WS cap in SubCapModels.Values)
                {
                    if (cap.capID.id1.Equals(appSpecificInfoGroup.capID.id1) &&
                        cap.capID.id2.Equals(appSpecificInfoGroup.capID.id2) &&
                        cap.capID.id3.Equals(appSpecificInfoGroup.capID.id3) &&
                        cap.capID.serviceProviderCode.Equals(appSpecificInfoGroup.capID.serviceProviderCode))
                    {
                        return CAPHelper.GetAliasOrCapTypeLabel(cap.capType);
                    }
                }
            }

            return string.Empty;
        }

        /// <summary>
        /// Show Application specific information on UI
        /// </summary>
        /// <param name="source">The AppSpecificInfoGroupModel list</param>
        private void ShowAppSpecInfoGroup(AppSpecificInfoGroupModel4WS[] source)
        {
            AppSpecificInfoGroupModel4WS[] appSpecInfoGroup = source;

            ILogoBll logo = ObjectFactory.GetObject<ILogoBll>();

            groupIndex = 0; //indicates which group a field belongs to
            ControlList.Clear();
            AllControls.Clear();
            phPlumbingGroup.Controls.Clear();

            SubCapModels.Clear();

            ASITableDrillDownModel4WS[] asiDrillDownModels = null;
            string prevAgencyCode = string.Empty;
            string prevASIGroup = string.Empty;
            phPlumbingGroup.Controls.Add(new LiteralControl("<table role='presentation' style=\"width:100%;border-collapse: collapse;\">"));

            //appSpecInfoGroup need be sorted by agency. 
            foreach (AppSpecificInfoGroupModel4WS specificInfo in appSpecInfoGroup)
            {
                AppSpecificInfoGroupModel4WS model = specificInfo;

                //If a group has no fields
                if (model == null || model.fields == null || (StandardChoiceUtil.IsSuperAgency() && model.capID == null))
                {
                    continue;
                }

                string agencyCode = model.capID == null ? ConfigManager.AgencyCode : model.capID.serviceProviderCode;
                string asiSubGroupSecurity = ASISecurityUtil.GetASISecurity(agencyCode, specificInfo.groupCode, specificInfo.groupName, string.Empty, ModuleName);

                if (ACAConstant.ASISecurity.None.Equals(asiSubGroupSecurity)
                    || ASISecurityUtil.IsAllFieldsNoAccess(specificInfo, ModuleName))
                {
                    continue;
                }
                
                if (!StandardChoiceUtil.IsSuperAgency() && model.capID == null)
                {
                    CapModel4WS capModel = AppSession.GetCapModelFromSession(this.ModuleName);
                    model.capID = capModel.capID;
                }

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
                ASITableDrillDownModel4WS curASIDrillDown = asiDrillDownInstance.GetCurASIDrillDown(asiDrillDownModels, model, ModuleName);

                //change ASI data source.
                model = asiDrillDownInstance.ChangeASIDataSource(model, curASIDrillDown, groupIndex);

                StringBuilder subGroup = new StringBuilder();
                subGroup.Append("<tr>");
                subGroup.Append("<td>");
                subGroup.Append("<table role='presentation'><tr><td class='ACA_Title_Text font13px' valign=\"middle\">");  //table for sub group title

                string captype = GetSubCapModels(model);

                string groupName = I18nStringUtil.GetString(model.resAlternativeLabel, model.alternativeLabel, model.resGroupName, model.groupName);

                bool isSuperCAP = CapUtil.IsSuperCAP(ModuleName);

                if (model.capID != null && isSuperCAP)
                {
                    string logoDescription = string.Format("{0} ({1})", groupName, model.capID.serviceProviderCode);
                    string agencyLogo = CapUtil.GetAgencyLogoHtml(model.capID.serviceProviderCode, ModuleName);

                    subGroup.Append(agencyLogo);
                    subGroup.Append("<div class=\"ACA_ValCal_Title font10px\">");
                    subGroup.Append(logoDescription);
                    subGroup.Append("</div>");
                }
                else 
                {
                    //The agency is normal agency.
                    subGroup.Append(groupName);
                }

                subGroup.Append("</td></tr></table>");
                phPlumbingGroup.Controls.Add(new LiteralControl(subGroup.ToString()));

                string subGroupInstruction = I18nStringUtil.GetCurrentLanguageString(model.resInstruction, model.instruction);
                phPlumbingGroup.Controls.Add(new LiteralControl(string.Format("<div class='ACA_Section_Instruction ACA_Section_Instruction_FontSize'>{0}</div>", subGroupInstruction)));
                
                if (model.columnLayout == 2) 
                {
                    // Layout as two columns
                    LayouASIWith2Columns(appSpecInfoGroup, model, curASIDrillDown, captype);
                }
                else
                {
                    // Layout as one column
                    LayoutASIWith1Column(appSpecInfoGroup, model, curASIDrillDown, captype);
                }

                if (appSpecInfoGroup.Length > 1 && groupIndex < appSpecInfoGroup.Length - 1 && appSpecInfoGroup[groupIndex].fields != null)
                {
                    phPlumbingGroup.Controls.Add(new LiteralControl("<div class=\"ACA_TabRow ACA_Line_Content\">&nbsp;</div>"));
                }

                phPlumbingGroup.Controls.Add(new LiteralControl("</td></tr>"));

                groupIndex++;
            }

            phPlumbingGroup.Controls.Add(new LiteralControl("</table>"));
        }

        /// <summary>
        /// Layout ASI with one column
        /// </summary>
        /// <param name="appSpecInfoGroup">AppSpecificInfoGroupModel4WS array</param>
        /// <param name="model">AppSpecificInfoGroupModel4WS object</param>
        /// <param name="curASIDrillDown">ASITableDrillDownModel4WS object</param>
        /// <param name="captype">cap type string</param>
        private void LayoutASIWith1Column(AppSpecificInfoGroupModel4WS[] appSpecInfoGroup, AppSpecificInfoGroupModel4WS model, ASITableDrillDownModel4WS curASIDrillDown, string captype)
        {
            HtmlGenericControl divSubGroup = new HtmlGenericControl("div");
            divSubGroup.Attributes.Add("class", "ACA_Row");

            //Default label layout is horizontal.
            ControlLayoutType layoutType = model.labelDisplay == LabelDisplay.TOP.ToString() ? ControlLayoutType.Vertical : ControlLayoutType.Horizontal;

            foreach (AppSpecificInfoModel4WS item in model.fields)
            {
                WebControl control = CreateWebControl(item, appSpecInfoGroup, captype, Is4FeeEstimator, curASIDrillDown);

                if (control == null)
                {
                    continue;
                }

                bool isHidden = ValidationUtil.IsHidden(item.vchDispFlag);

                string asiFieldSecurity = ASISecurityUtil.GetASISecurity(item, ModuleName);

                if (ACAConstant.ASISecurity.None.Equals(asiFieldSecurity))
                {
                    isHidden = true;
                }

                if (control is CheckBox)
                {
                    CheckBox cb = (CheckBox)control;
                    cb.InputAttributes.Add("SecurityType", asiFieldSecurity);
                }
                else
                {
                    control.Attributes.Add("SecurityType", asiFieldSecurity);
                }

                SetControlStyle(layoutType, item, control, isHidden);

                HtmlGenericControl divControl = new HtmlGenericControl("div");

                if (isHidden)
                {
                    divControl.Attributes.Add("class", "ACA_Hide");
                    control.Attributes.Add(ACAConstant.IS_HIDDEN, "true");
                }

                // div for one field
                if (layoutType == ControlLayoutType.Horizontal)
                {
                    divControl.Style.Add(HtmlTextWriterStyle.MarginBottom, "5px");
                }

                divControl.Controls.Add(control);
                divSubGroup.Controls.Add(divControl);
                phPlumbingGroup.Controls.Add(divSubGroup);

                AddRequiredValidator4Radio(item, control);
                ControlList.Add(control.ID, control);
            }
        }

        /// <summary>
        /// Layout ASI fields with two columns
        /// AppSpecificInfoGroupModel4WS.labelDisplay - field display style
        /// AppSpecificInfoGroupModel4WS.columnArrangement - ASI column arrangement
        /// </summary>
        /// <param name="appSpecInfoGroup">AppSpecificInfoGroupModel4WS array</param>
        /// <param name="model">AppSpecificInfoGroupModel4WS object</param>
        /// <param name="curASIDrillDown">ASITableDrillDownModel4WS object</param>
        /// <param name="captype">cap type string</param>
        private void LayouASIWith2Columns(AppSpecificInfoGroupModel4WS[] appSpecInfoGroup, AppSpecificInfoGroupModel4WS model, ASITableDrillDownModel4WS curASIDrillDown, string captype)
        {
            //default label layout is left and default column arrangement is horizontal.
            ControlLayoutType layoutType = model.labelDisplay == LabelDisplay.TOP.ToString() ? ControlLayoutType.Vertical : ControlLayoutType.Horizontal;
            string columnArrangement = string.IsNullOrEmpty(model.columnArrangement) ? ControlLayoutType.Horizontal.ToString() : model.columnArrangement;

            //get columnLayout from AA classic.default is 1 column.
            string columnLayout = Convert.ToString(model.columnLayout);
            columnLayout = string.IsNullOrEmpty(columnLayout) ? "1" : columnLayout;
            Table tbSubGroup = new Table();
            tbSubGroup.ID = "tbSubGroup" + groupIndex;

            tbSubGroup.CssClass = "ACA_TDAlignLeftOrRightTop collapse_table aca_asi_table";
            tbSubGroup.Attributes.Add("columnArrangement", columnArrangement);
            tbSubGroup.Attributes.Add("columnLayout", columnLayout);
            tbSubGroup.Attributes.Add("role", "presentation");
            int i = 0;
            bool hasHiddenControl = false;

            foreach (AppSpecificInfoModel4WS item in model.fields)
            {
                WebControl control = CreateWebControl(item, (AppSpecificInfoGroupModel4WS[])appSpecInfoGroup, captype, Is4FeeEstimator, curASIDrillDown);
                
                if (control == null)
                {
                    continue;
                }

                bool isHidden = ValidationUtil.IsHidden(item.vchDispFlag);

                string asiFieldSecurity = ASISecurityUtil.GetASISecurity(item, ModuleName);

                if (ACAConstant.ASISecurity.None.Equals(asiFieldSecurity))
                {
                    isHidden = true;
                }

                if (control is CheckBox)
                {
                    CheckBox cb = (CheckBox)control;
                    cb.InputAttributes.Add("SecurityType", asiFieldSecurity);
                }
                else
                {
                    control.Attributes.Add("SecurityType", asiFieldSecurity);
                }

                SetControlStyle(layoutType, item, control, isHidden);

                //TableRow row = null;
                TableCell cell = new TableCell();
                cell.Controls.Add(control);
                cell.Attributes.Add("index", i.ToString());
                cell.Style.Add(HtmlTextWriterStyle.Width, "50%");

                if (layoutType == ControlLayoutType.Horizontal)
                {
                    cell.Style.Add(HtmlTextWriterStyle.PaddingBottom, "5px");
                }

                // layout asi column with horizontal style.
                if (columnArrangement == ControlLayoutType.Horizontal.ToString()) 
                {
                    if (i % 2 == 0)  
                    {
                        //add a new row for asi field with odd index
                        TableRow row = new TableRow();
                        tbSubGroup.Rows.Add(row);
                    }

                    tbSubGroup.Rows[i / 2].Cells.Add(cell);
                }
                else
                {
                    // vertical layout
                    int totalLen = model.fields.Length;
                    int rowLength = totalLen % 2 == 0 ? totalLen / 2 : (totalLen / 2) + 1;

                    if (i < rowLength)  
                    {
                        //add a new row
                        TableRow row = new TableRow();
                        tbSubGroup.Rows.Add(row);
                        tbSubGroup.Rows[i].Cells.Add(cell);
                    }
                    else
                    {
                        tbSubGroup.Rows[i - rowLength].Cells.Add(cell);
                    }
                }

                if (isHidden)
                {
                    cell.Attributes.Add("class", "ACA_Hide");
                    control.Attributes.Add(ACAConstant.IS_HIDDEN, "true");
                    hasHiddenControl = true;
                }

                AddRequiredValidator4Radio(item, control);
                ControlList.Add(control.ID, control);
                i++;
            }

            phPlumbingGroup.Controls.Add(tbSubGroup);

            if (hasHiddenControl)
            {
                //re-order ASI table if it has hidden control(s)
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ReSortASI" + tbSubGroup.ID, "LayoutASI($get('" + tbSubGroup.ClientID + "'));", true);
            }
        }

        /// <summary>
        /// Set style(layout, label width and control width) for ASI control.
        /// </summary>
        /// <param name="layoutType">ControlLayoutType EUNM, indicates field layout with vertical or horizontal</param>
        /// <param name="item">AppSpecificInfoModel4WS object</param>
        /// <param name="control">WebControl for asi field</param>
        /// <param name="isHidden">true if asi control is hidden</param>
        private void SetControlStyle(ControlLayoutType layoutType, AppSpecificInfoModel4WS item, WebControl control, bool isHidden)
        {
            if (control is IAccelaControl)
            {
                (control as IAccelaControl).LayoutType = layoutType;
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
        }

        /// <summary>
        /// Add required field validation for radio box.
        /// </summary>
        /// <param name="item">AppSpecificInfoModel4WS object.</param>
        /// <param name="control">WebControl for asi field</param>
        private void AddRequiredValidator4Radio(AppSpecificInfoModel4WS item, WebControl control)
        {
            if (int.Parse(item.fieldType) == (int)FieldType.HTML_RADIOBOX
                && ValidationUtil.IsYes(item.requiredFlag)
                && !ValidationUtil.IsHidden(item.vchDispFlag) && !(control as IAccelaControl).IsHidden)
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
        }

        /// <summary>
        /// Bind data to UI controls
        /// </summary>
        private void BindPlumbingInfo()
        {
            CapModel4WS capModel = AppSession.GetCapModelFromSession(ModuleName);
            if (capModel.capType == null)
            {
                return;
            }

            try
            {
                if (!ASISecurityUtil.GetASISecurity(ASIGroupModelList, ModuleName).Equals(ACAConstant.ASISecurity.None))
                {
                    ShowAppSpecInfoGroup(ASIGroupModelList);
                }
            }
            catch (ACAException e)
            {
                MessageUtil.ShowMessage(Page, MessageType.Error, e.Message);
            }
        }

        #endregion Methods
    }
}