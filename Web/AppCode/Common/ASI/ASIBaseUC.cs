#region Header

/*
 * <pre>
 *  Accela Citizen Access
 *  File: ASIBaseUC.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2007-2014
 *
 *  Description:
 * use it to define some common function for ASI table and ASI
 *  Notes:
 * $Id: ASIBaseUC.cs 276684 2014-08-05 08:58:28Z ACHIEVO\james.shi $.
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

using Accela.ACA.Common;
using Accela.ACA.Common.Util;
using Accela.ACA.Web.Common;
using Accela.ACA.Web.Common.Control;
using Accela.ACA.Web.ExpressionBuild;
using Accela.ACA.WSProxy;
using Accela.Web.Controls;

namespace Accela.ACA.Web
{
    /// <summary>
    ///  ASI and ASI Table basic controller
    /// </summary>
    public class ASIBaseUC : PermitDetailBaseUserControl
    {
        #region Fields

        /// <summary>
        /// text area max input length.
        /// </summary>
        protected const int MAX_LENGTH_4K = 4000;

        /// <summary>
        /// Expression execute field
        /// </summary>
        protected readonly string EXPRESSION_S_FIELD = "EXECUTE_FIELD";

        /// <summary>
        /// Display currency symbol
        /// </summary>
        private const string DISPLAY_CURRENCY_SYMBOL = "DisplayCurrencySymbol";

        /// <summary>
        /// ASI search form.
        /// </summary>
        private const string ASI_SEARCH_FORM = "ASI_SEARCH_FORM";

        /// <summary>
        /// The collection of all Controls
        /// </summary>
        private Dictionary<string, WebControl> _allControls;

        /// <summary>
        /// The list of sub cap models
        /// </summary>
        private IDictionary<string, CapModel4WS> _subCapModels;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the ASIBaseUC class.
        /// </summary>
        public ASIBaseUC()
        {
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// Gets or sets expression instance.
        /// </summary>
        public ExpressionFactory ExpressionInstance
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the list of sub cap models
        /// </summary>
        public IDictionary<string, CapModel4WS> SubCapModels
        {
            get
            {
                if (_subCapModels == null)
                {
                    _subCapModels = new Dictionary<string, CapModel4WS>();
                }

                return _subCapModels;
            }

            set
            {
                _subCapModels = value;
            }
        }

        /// <summary>
        /// Gets or sets the collection of all Controls
        /// </summary>
        public Dictionary<string, WebControl> AllControls
        {
            get
            {
                if (_allControls == null)
                {
                    _allControls = new Dictionary<string, WebControl>();
                }

                return _allControls;
            }

            set
            {
                _allControls = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether ASI search form. 
        /// </summary>
        protected bool IsASISearchForm
        {
            get
            {
                if (ViewState[ASI_SEARCH_FORM] == null)
                {
                    ViewState[ASI_SEARCH_FORM] = false;
                }

                return (bool)ViewState[ASI_SEARCH_FORM];
            }

            set
            {
                ViewState[ASI_SEARCH_FORM] = value;
            }
        }

        /// <summary>
        /// Gets a value indicating whether display Hijri Calendar 
        /// </summary>
        protected bool IsDisplayHijriCalendar
        {
            get
            {
                return NeedShowHijriCalendar && StandardChoiceUtil.IsEnableDisplayISLAMICCalendar();
            }
        }

        /// <summary>
        /// Gets a value indicating whether need show Hijri Calendar 
        /// </summary>
        protected virtual bool NeedShowHijriCalendar
        {
            get { return true; }
        }

        /// <summary>
        /// Gets a value indicating whether display currency symbol
        /// </summary>
        private bool IsDisplayCurrencySymbol
        {
            get
            {
                if (ViewState[DISPLAY_CURRENCY_SYMBOL] == null)
                {
                    ViewState[DISPLAY_CURRENCY_SYMBOL] = StandardChoiceUtil.IsDisplayCurrencySymbol();
                }

                return (bool)ViewState[DISPLAY_CURRENCY_SYMBOL];
            }
        }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Judge if exists the ASI fields.
        /// </summary>
        /// <param name="appSpecInfoGroup">The AppSpecificInfoGroupModel list.</param>
        /// <returns>If exists the ASI fields, return true else return false.</returns>
        public static bool ExistsASIFields(AppSpecificInfoGroupModel4WS[] appSpecInfoGroup)
        {
            if (appSpecInfoGroup == null || appSpecInfoGroup.Length == 0)
            {
                return false;
            }

            bool exists = false;

            foreach (AppSpecificInfoGroupModel4WS model in appSpecInfoGroup)
            {
                //If a group has no fields
                if (model != null && model.fields != null)
                {
                    exists = true;
                    break;
                }
            }

            return exists;
        }

        /// <summary>
        /// Judge if exists the ASIT fields.
        /// </summary>
        /// <param name="appSpecificTableGroup">The AppSpecificTableGroupModel4WS list.</param>
        /// <returns>If exists the ASI fields, return true else return false.</returns>
        public static bool ExistsASITFields(AppSpecificTableGroupModel4WS[] appSpecificTableGroup)
        {
            if (appSpecificTableGroup == null || appSpecificTableGroup.Length == 0)
            {
                return false;
            }

            bool exists = false;

            foreach (AppSpecificTableGroupModel4WS model in appSpecificTableGroup)
            {
                if (model == null || model.tablesMapValues == null)
                {
                    continue;
                }

                foreach (AppSpecificTableModel4WS table in model.tablesMapValues)
                {
                    if (table.columns != null && table.columns.Length > 0)
                    {
                        exists = true;
                        break;
                    }
                }

                if (exists)
                {
                    break;
                }
            }

            return exists;
        }

        /// <summary>
        /// filter control name for expression in ASI
        /// </summary>
        /// <param name="str">control name</param>
        /// <returns>final control name</returns>
        public static string FilterSpciefCharForControlName(string str)
        {
            if (!string.IsNullOrEmpty(str))
            {
                str = str.Replace('.', '_');
                str = str.Replace(' ', '_');
                str = str.Replace(',', '_');
                str = str.Replace('+', '_');
                str = str.Replace('"', '_');
                str = str.Replace('^', '_');
                str = str.Replace('\\', '_');
                str = str.Replace(':', '_');
                str = str.Replace('-', '_');
            }

            return str;
        }

        /// <summary>
        /// Gets the index of the spec info group.
        /// </summary>
        /// <param name="specInfoGroupModels">The spec info group models.</param>
        /// <param name="specificInfoModel">The specific info model.</param>
        /// <returns>
        /// get the specific information group index.
        /// </returns>
        public static int GetSpecInfoGroupIndex(AppSpecificInfoGroupModel4WS[] specInfoGroupModels, AppSpecificInfoModel4WS specificInfoModel)
        {
            int groupIndex = 0;

            if (specInfoGroupModels == null || specificInfoModel == null || specificInfoModel.serviceProviderCode == null || 
                specificInfoModel.actStatus == null || specificInfoModel.checkboxType == null)
            {
                return groupIndex;
            }

            for (int i = 0; i < specInfoGroupModels.Length; i++)
            {
                AppSpecificInfoGroupModel4WS appSpecInfoGroup = specInfoGroupModels[i];

                if (appSpecInfoGroup == null || appSpecInfoGroup.capID == null)
                {
                    continue;
                }

                if (specificInfoModel.serviceProviderCode.Equals(appSpecInfoGroup.capID.serviceProviderCode, StringComparison.InvariantCultureIgnoreCase)
                    && specificInfoModel.actStatus.Equals(appSpecInfoGroup.groupCode, StringComparison.InvariantCultureIgnoreCase)
                    && specificInfoModel.checkboxType.Equals(appSpecInfoGroup.groupName, StringComparison.InvariantCultureIgnoreCase))
                {
                    groupIndex = i;
                    break;
                }
            }

            return groupIndex;
        }

        /// <summary>
        /// Gets the index of the spec info.
        /// </summary>
        /// <param name="specInfoGroupModels">The spec info group models.</param>
        /// <param name="specificInfoModel">The specific info model.</param>
        /// <returns>
        /// get the specific information sub group index.
        /// </returns>
        public static int GetSpecInfoIndex(AppSpecificInfoGroupModel4WS[] specInfoGroupModels, AppSpecificInfoModel4WS specificInfoModel)
        {
            int itemIndex = 0;

            if (specInfoGroupModels == null || specificInfoModel == null || specificInfoModel.serviceProviderCode == null || 
                specificInfoModel.actStatus == null || specificInfoModel.checkboxType == null || specificInfoModel.checkboxDesc == null)
            {
                return itemIndex;
            }
            
            foreach (AppSpecificInfoGroupModel4WS appSpecInfoGroup in specInfoGroupModels)
            {
                if (appSpecInfoGroup == null || appSpecInfoGroup.capID == null)
                {
                    continue;
                }

                if (!specificInfoModel.serviceProviderCode.Equals(appSpecInfoGroup.capID.serviceProviderCode, StringComparison.InvariantCultureIgnoreCase)
                    || !specificInfoModel.actStatus.Equals(appSpecInfoGroup.groupCode, StringComparison.InvariantCultureIgnoreCase)
                    || !specificInfoModel.checkboxType.Equals(appSpecInfoGroup.groupName, StringComparison.InvariantCultureIgnoreCase))
                {
                    continue;
                }

                for (int i = 0; i < appSpecInfoGroup.fields.Length; i++)
                {
                    AppSpecificInfoModel4WS appSpecInfo = appSpecInfoGroup.fields[i];

                    if (specificInfoModel.checkboxDesc.Equals(appSpecInfo.checkboxDesc, StringComparison.InvariantCultureIgnoreCase))
                    {
                        itemIndex = i;
                        break;
                    }
                }
            }

            return itemIndex;
        }

        /// <summary>
        /// calculate current asi control in which page.
        /// </summary>
        /// <param name="currentStep">The current step.</param>
        /// <param name="currentPage">The current page.</param>
        /// <returns>current asi control in which page.</returns>
        public static int GetCurrentPageIndex(int currentStep, int currentPage)
        {
            PageFlowGroupModel pageflowGroup = AppSession.GetPageflowGroupFromSession();
            int currentPageIndex = 0;

            if (pageflowGroup != null)
            {
                for (int i = 0; i < currentStep; i++)
                {
                    if (pageflowGroup.stepList != null && pageflowGroup.stepList.Length > 0
                            && pageflowGroup.stepList[i] != null && pageflowGroup.stepList[i].pageList != null)
                    {
                        currentPageIndex += pageflowGroup.stepList[i].pageList.Length;
                    }
                }
            }

            currentPageIndex += currentPage + 1;

            return currentPageIndex;
        }

        /// <summary>
        /// Get section page index.
        /// </summary>
        /// <param name="sectionName">The section name.</param>
        /// <returns>The index of the section page.</returns>
        public static int GetSectionPageIndex(string sectionName)
        {
            int sectionPageIndex = 1;
            PageFlowGroupModel pageflowGroup = AppSession.GetPageflowGroupFromSession();
            StepModel[] steps = pageflowGroup.stepList;

            foreach (StepModel step in steps)
            {
                PageModel[] pages = step.pageList;

                foreach (PageModel page in pages)
                {
                    ComponentModel[] comps = page.componentList;

                    if (comps.Count(comp => comp.componentName.ToUpper().Equals(sectionName, StringComparison.InvariantCulture)) > 0)
                    {
                        return sectionPageIndex;
                    }

                    sectionPageIndex++;
                }
            }

            return sectionPageIndex;
        }

        /// <summary>
        /// Get the page count.
        /// </summary>
        /// <returns>page count.</returns>
        public static int GetPagesCount()
        {
            PageFlowGroupModel pageflowGroup = AppSession.GetPageflowGroupFromSession();
            int pageCount = 0;

            if (pageflowGroup != null)
            {
                pageCount = pageflowGroup.stepList.SelectMany(step => step.pageList).Count();
            }

            return pageCount;
        }

        /// <summary>
        /// Removes the redundant data of ASIs whose group and subgroup match the specified strings from the instance of CapModel4WS.
        /// </summary>
        /// <param name="capModel">The instance of CapModel4WS whose ASI data will be removed.</param>
        /// <param name="group">The ASI group name to be checked.</param>
        /// <param name="subGroup">The ASI subgroup name to be checked.</param>
        public static void RemoveRedundantASIs(CapModel4WS capModel, string group, string subGroup)
        {
            // For the data of type ASI, in order to remove the subgroup, we need to set all the fields to empty.
            if (capModel != null && capModel.appSpecificInfoGroups != null && capModel.appSpecificInfoGroups.Length > 0
                && !string.IsNullOrEmpty(group) && !string.IsNullOrEmpty(subGroup))
            {
                List<AppSpecificInfoGroupModel4WS> tmpASIGroup = new List<AppSpecificInfoGroupModel4WS>(capModel.appSpecificInfoGroups);
                AppSpecificInfoGroupModel4WS deletedItem = tmpASIGroup.Find(item => group.Equals(item.groupCode, StringComparison.InvariantCultureIgnoreCase) && subGroup.Equals(item.groupName, StringComparison.InvariantCultureIgnoreCase));

                if (deletedItem != null)
                {
                    foreach (var field in deletedItem.fields)
                    {
                        field.checklistComment = string.Empty;
                    }
                }

                capModel.appSpecificInfoGroups = tmpASIGroup.ToArray();
            }
        }

        /// <summary>
        /// Removes the redundant data of ASITs whose group and subgroup match the specified strings from the instance of CapModel4WS.
        /// </summary>
        /// <param name="capModel">The instance of CapModel4WS whose ASIT data will be removed.</param>
        /// <param name="group">The ASIT group name to be checked.</param>
        /// <param name="subGroup">The ASIT subgroup name to be checked.</param>
        public static void RemoveRedundantASITs(CapModel4WS capModel, string group, string subGroup)
        {
            // For the data of type ASIT, in order to remove the subgroup, we set it to null simply.              
            if (capModel != null && capModel.appSpecTableGroups != null && capModel.appSpecTableGroups.Length > 0
                && !string.IsNullOrEmpty(group) && !string.IsNullOrEmpty(subGroup))
            {
                List<AppSpecificTableGroupModel4WS> tmpASITGroup = new List<AppSpecificTableGroupModel4WS>(capModel.appSpecTableGroups);

                AppSpecificTableGroupModel4WS removeASITGroup = tmpASITGroup.Find(item => group.Equals(item.groupName, StringComparison.InvariantCultureIgnoreCase) && subGroup.Equals(item.tablesMap[0], StringComparison.InvariantCultureIgnoreCase));

                if (removeASITGroup != null && removeASITGroup.tablesMapValues != null && removeASITGroup.tablesMapValues.Length > 0)
                {
                    AppSpecificTableModel4WS table = removeASITGroup.tablesMapValues[0];
                    table.rowIndex = null;
                    table.tableField = null;
                    table.tableFieldValues = null;
                }

                capModel.appSpecTableGroups = tmpASITGroup.ToArray();
            }
        }

        /// <summary>
        /// Structures the DDL control ID.
        /// </summary>
        /// <param name="groupIndex">Index of the group.</param>
        /// <param name="itemIndex">Index of the item.</param>
        /// <returns>a string ID for DDL.</returns>
        public string StructureDDLControlID(int groupIndex, int itemIndex)
        {
            StringBuilder idBuilder = new StringBuilder();
            idBuilder.Append(ACAConstant.DROPDOWNLIST_CONTROLID_PREFIXION);
            idBuilder.Append(ExpressionFactory.SPLIT_CHAR);
            idBuilder.Append(groupIndex.ToString());
            idBuilder.Append(ExpressionFactory.SPLIT_CHAR);
            idBuilder.Append(itemIndex.ToString());
            return idBuilder.ToString();
        }

        /// <summary>
        /// Creates the web control.
        /// </summary>
        /// <param name="item">The ASI field item.</param>
        /// <param name="specInfoGroupModels">The specific information group models.</param>
        /// <param name="capType">Type of the cap.</param>
        /// <param name="is4FeeEstimator">if set to <c>true</c> [is4 fee estimator].</param>
        /// <param name="drillDownModel">The drill down model.</param>
        /// <returns>a created web control.</returns>
        protected WebControl CreateWebControl(AppSpecificInfoModel4WS item, AppSpecificInfoGroupModel4WS[] specInfoGroupModels, string capType, bool is4FeeEstimator, ASITableDrillDownModel4WS drillDownModel)
        {
            if (item == null)
            {
                return null;
            }
            
            string finalControlName = string.Empty;
            StringBuilder controlName = new StringBuilder();
            controlName.Append(ACAConstant.EXP_ASI_FIELD_NAME_PREFIX);
            controlName.Append(ExpressionFactory.SPLIT_CHAR);
            controlName.Append(FilterSpciefCharForControlName(Server.UrlEncode(item.checkboxType)));
            controlName.Append(ExpressionFactory.SPLIT_CHAR);
            controlName.Append(FilterSpciefCharForControlName(Server.UrlEncode(item.fieldLabel)));

            if (StandardChoiceUtil.IsSuperAgency())
            {
                controlName.Append(ACAConstant.SPLIT_CHAR);
                controlName.Append(item.serviceProviderCode + ACAConstant.SPLIT_CHAR);
                controlName.Append(FilterSpciefCharForControlName(Server.UrlEncode(capType)));
                finalControlName = controlName.ToString();
            }
            else
            {
                finalControlName = ExpressionUtil.GetFullControlFieldName(CapModel, controlName.ToString());
            }

            int groupIndex = GetSpecInfoGroupIndex(specInfoGroupModels, item);
            int itemIndex = GetSpecInfoIndex(specInfoGroupModels, item);

            WebControl control = null;
            var isCheckbox = false;

            switch (int.Parse(item.fieldType))
            {
                case (int)FieldType.HTML_RADIOBOX:
                    control = CreateRadioButtonList(item, groupIndex, itemIndex);
                    break;
                case (int)FieldType.HTML_SELECTBOX:
                    ASIDrillDownUtil asiDrillDownInstance = new ASIDrillDownUtil();
                    bool isDrillDField = asiDrillDownInstance.IsDrillDownField(drillDownModel, item.checkboxDesc);
                    control = CreateDropdownList(item, groupIndex, itemIndex, isDrillDField);

                    if (isDrillDField)
                    {
                        control = asiDrillDownInstance.AddAttributeForDrillDown(control, item, specInfoGroupModels, drillDownModel);
                        control = asiDrillDownInstance.DisableControlForDrillDown(control, item, specInfoGroupModels, drillDownModel);

                        //if drop down is enable and with one option, it need auto trig drill down event.
                        if (control.Enabled)
                        {
                            AccelaDropDownList ddlControl = control as AccelaDropDownList;
                            
                            if (ddlControl != null && ddlControl.Required && ddlControl.Items.Count == 2 && ddlControl.Items.IndexOf(DropDownListBindUtil.DefaultListItem) > -1)
                            {
                                control.Attributes.Add("IsAutoTrig", "true");
                            }
                        }
                    }

                    if (ValidationUtil.IsYes(item.requiredFlag))
                    {
                        control.Attributes.Add("Required", "true");
                    }

                    break;
                case (int)FieldType.HTML_TEXTAREABOX:
                    control = CreateTextbox(item, true, groupIndex, itemIndex, is4FeeEstimator);
                    break;
                case (int)FieldType.HTML_CHECKBOX:
                    isCheckbox = true;
                    control = CreateCheckbox(item, groupIndex, itemIndex);
                    break;
                default:
                    control = CreateTextbox(item, false, groupIndex, itemIndex, is4FeeEstimator);
                    break;
            }

            if (!IsASISearchForm)
            {
                string asiFieldSecurity = ASISecurityUtil.GetASISecurity(item, ModuleName);

                //Disable web control if it is readonly by ASI security.
                ASISecurityUtil.DisableFieldForSecurity(
                    control,
                    (FieldType)Enum.Parse(typeof(FieldType), item.fieldType),
                    ACAConstant.ASISecurity.Read.Equals(asiFieldSecurity));

                if (ACAConstant.ASISecurity.None.Equals(asiFieldSecurity))
                {
                    (control as IAccelaControl).IsHidden = true;

                    //Should setting ishidden attribute for expression, if security hidden the control, should not display by expression.
                    control.Attributes.Add(ACAConstant.IS_HIDDEN, ACAConstant.COMMON_TRUE);
                }
            } 
            
            if (item != null)
            {
                control.ID = item.serviceProviderCode + ExpressionFactory.SPLIT_CHAR + control.ID;
                string agencyCode = !string.IsNullOrEmpty(item.serviceProviderCode) ? item.serviceProviderCode.ToLower() : ACAConstant.AgencyCode.ToLower();

                var targetControlAtributes = isCheckbox ? (control as CheckBox).InputAttributes : control.Attributes;
                targetControlAtributes.Add("GroupIndex", groupIndex.ToString());
                targetControlAtributes.Add("ItemIndex", itemIndex.ToString());
                targetControlAtributes.Add("agency", agencyCode);
                targetControlAtributes.Add("isasi", "true");
                targetControlAtributes.Add("expCombinedKey", finalControlName);
            }

            if (!AllControls.ContainsKey(finalControlName))
            {
                AllControls.Add(finalControlName, control);
            }

            return control;
        }

        /// <summary>
        /// Initials the drill down web method.
        /// </summary>
        protected void InitDrillDown()
        {
            ScriptManager smg = ScriptManager.GetCurrent(Page);
            smg.EnablePageMethods = true;
            ServiceReference srDrillDown = new ServiceReference("~/WebService/DrillDownService.asmx");
            smg.Services.Add(srDrillDown);
        }

        /// <summary>
        /// sort asi by it's agency code and ASI group
        /// </summary>
        /// <param name="appSpeciInfoGroup">App Specific Information Group array</param>
        protected void SortASIByAgencyAndGroup(AppSpecificInfoGroupModel4WS[] appSpeciInfoGroup)
        {
            if (appSpeciInfoGroup == null || appSpeciInfoGroup.Length == 1)
            {
                return;
            }

            for (int i = 0; i < appSpeciInfoGroup.Length - 1; i++)
            {
                for (int j = i + 1; j < appSpeciInfoGroup.Length; j++)
                {
                    string comparedAgency = appSpeciInfoGroup[i].capID == null ? ConfigManager.AgencyCode : appSpeciInfoGroup[i].capID.serviceProviderCode;
                    string referAgency = appSpeciInfoGroup[j].capID == null ? ConfigManager.AgencyCode : appSpeciInfoGroup[j].capID.serviceProviderCode;
                    string comparedGroup = appSpeciInfoGroup[i].groupCode;
                    string referGroup = appSpeciInfoGroup[j].groupCode;

                    if (comparedAgency.CompareTo(referAgency) > 0 || (comparedAgency.CompareTo(referAgency) == 0 && comparedGroup.CompareTo(referGroup) > 0))
                    {
                        AppSpecificInfoGroupModel4WS tempModel = appSpeciInfoGroup[i];
                        appSpeciInfoGroup[i] = appSpeciInfoGroup[j];
                        appSpeciInfoGroup[j] = tempModel;
                    }
                }
            }
        }

        /// <summary>
        /// Clone expression parameters
        /// </summary>
        /// <param name="originalExpressionParam">original expression parameters</param>
        /// <returns>cloned expression parameters</returns>
        private ExpressionDTOModel CloneExpressionParam(ExpressionDTOModel originalExpressionParam)
        {
            ExpressionDTOModel clone = new ExpressionDTOModel();
            clone.behavior = originalExpressionParam.behavior;
            clone.viewKey1 = originalExpressionParam.viewKey1;

            if (originalExpressionParam.inputParams != null &&
                originalExpressionParam.inputParams.Length > 0)
            {
                clone.inputParams = new ExpressionFieldModel[originalExpressionParam.inputParams.Length];
                for (int i = 0; i < originalExpressionParam.inputParams.Length; i++)
                {
                    clone.inputParams[i] = CloneExpressionProperties(originalExpressionParam.inputParams[i]);
                }
            }

            if (originalExpressionParam.runResult != null &&
                originalExpressionParam.runResult.Length > 0)
            {
                clone.runResult = new ExpressionFieldModel[originalExpressionParam.runResult.Length];
                for (int i = 0; i < originalExpressionParam.runResult.Length; i++)
                {
                    clone.runResult[i] = CloneExpressionProperties(originalExpressionParam.runResult[i]);
                }
            }

            return clone;
        }

        /// <summary>
        /// Clone expression properties
        /// </summary>
        /// <param name="orginal">original expression properties</param>
        /// <returns>cloned expression properties</returns>
        private ExpressionFieldModel CloneExpressionProperties(ExpressionFieldModel orginal)
        {
            ExpressionFieldModel expressionParam = new ExpressionFieldModel();
            expressionParam.columnCount = orginal.columnCount;
            expressionParam.columnIndex = orginal.columnIndex;
            expressionParam.dbRequired = orginal.dbRequired;
            expressionParam.expressionName = orginal.expressionName;
            expressionParam.fireEvent = orginal.fireEvent;
            expressionParam.label = orginal.label;
            expressionParam.message = orginal.message;
            expressionParam.name = orginal.name;
            expressionParam.readOnly = orginal.readOnly;
            expressionParam.recDate = orginal.recDate;
            expressionParam.recFulNam = orginal.recFulNam;
            expressionParam.recStatus = orginal.recStatus;
            expressionParam.refColumnName = orginal.refColumnName;
            expressionParam.required = orginal.required;
            expressionParam.rowIndex = orginal.rowIndex;
            expressionParam.servProvCode = orginal.servProvCode;
            expressionParam.type = orginal.type;
            expressionParam.usage = orginal.usage;
            expressionParam.value = orginal.value;
            expressionParam.variableKey = orginal.variableKey;
            expressionParam.viewID = orginal.viewID;
            return expressionParam;
        }

        /// <summary>
        /// Convert string to field alignment
        /// </summary>
        /// <param name="s">alignment string.</param>
        /// <returns>FieldAlignment.LTR or FieldAlignment.RTL</returns>
        private FieldAlignment ConvertString2FieldAlignment(string s)
        {
            return (string.IsNullOrEmpty(s) || s == "L") ? FieldAlignment.LTR : FieldAlignment.RTL;
        }

        /// <summary>
        /// Create Checkbox control
        /// </summary>
        /// <param name="item">application specific info model</param>
        /// <param name="groupIndex">group index</param>
        /// <param name="itemIndex">item index.</param>
        /// <returns>created check box</returns>
        private CheckBox CreateCheckbox(AppSpecificInfoModel4WS item, int groupIndex, int itemIndex)
        {
            //string label = String.Empty;
            AccelaCheckBox checkbox = new AccelaCheckBox();

            checkbox.ID = "chk_" + groupIndex + "_" + itemIndex;
            string tempFieldLabel = CapUtil.GetASIFieldLabel(item);
            string checkboxLabelPattern = I18nCultureUtil.UserPreferredCultureInfo.TextInfo.IsRightToLeft ? ":({1}){0}" : "{0}({1}):";
            checkbox.Label = item.requiredFeeCalc == ACAConstant.COMMON_Y && IsDisplayCurrencySymbol ? string.Format(checkboxLabelPattern, tempFieldLabel, I18nNumberUtil.CurrencySymbol) : tempFieldLabel;
            checkbox.Label = Server.HtmlEncode(checkbox.Label);
            string value = I18nStringUtil.GetString(item.resChecklistComment, item.checklistComment);
            checkbox.Checked = (ACAConstant.COMMON_CHECKED.Equals(value) || value == ModelUIFormat.FormatYNLabel(ACAConstant.COMMON_Yes)) ? true : false;
            checkbox.FieldAlignment = ConvertString2FieldAlignment(item.alignment);
            checkbox.Style.Add("white-space", "nowrap");
            checkbox.SubLabel = I18nStringUtil.GetCurrentLanguageString(item.resInstruction, item.instruction);

            checkbox.Attributes.Add(ACAConstant.ASI_GROUP_NAME, item.groupCode);
            checkbox.Attributes.Add(ACAConstant.ASI_SUB_GROUP_NAME, item.checkboxType);
            checkbox.Attributes.Add(ACAConstant.ASI_FIELD_NAME, item.fieldLabel);

            if (ValidationUtil.IsHidden(item.vchDispFlag))
            {
                checkbox.IsHidden = true;
                checkbox.Attributes.Add(ACAConstant.IS_HIDDEN, "true");
            }
            else if (ValidationUtil.IsYes(item.requiredFlag))
            {
                checkbox.Required = true;
            }

            return checkbox;
        }
        
        /// <summary>
        /// Creates the dropdown list.
        /// </summary>
        /// <param name="item">The ASI field item.</param>
        /// <param name="groupIndex">Index of the group.</param>
        /// <param name="itemIndex">Index of the item.</param>
        /// <param name="isDrillDownField">if set to <c>true</c> [is drill down field].</param>
        /// <returns>a created Accela drop down</returns>
        private AccelaDropDownList CreateDropdownList(AppSpecificInfoModel4WS item, int groupIndex, int itemIndex, bool isDrillDownField)
        {
            AccelaDropDownList dropdownlist = new AccelaDropDownList();
            dropdownlist.EnableViewState = false;
            dropdownlist.ID = StructureDDLControlID(groupIndex, itemIndex);
            dropdownlist.Label = Server.HtmlEncode(CapUtil.GetASIFieldLabel(item));
            dropdownlist.SubLabel = I18nStringUtil.GetCurrentLanguageString(item.resInstruction, item.instruction);

            dropdownlist.Attributes.Add(ACAConstant.ASI_GROUP_NAME, item.groupCode);
            dropdownlist.Attributes.Add(ACAConstant.ASI_SUB_GROUP_NAME, item.checkboxType);
            dropdownlist.Attributes.Add(ACAConstant.ASI_FIELD_NAME, item.fieldLabel);

            ListItem dropdownListItem = new ListItem(WebConstant.DropDownDefaultText, string.Empty);
            dropdownlist.Items.Add(dropdownListItem);
            dropdownListItem.Selected = true;
            dropdownlist.FieldAlignment = ConvertString2FieldAlignment(item.alignment);

            //if the ASI field associated to drill down, drill down would reset the value list, it is possible to be null.
            if (item.valueList == null && !isDrillDownField)
            {
                return dropdownlist;
            }

            if (ValidationUtil.IsHidden(item.vchDispFlag))
            {
                dropdownlist.IsHidden = true;
                dropdownlist.Attributes.Add(ACAConstant.IS_HIDDEN, "true");
            }
            else if (ValidationUtil.IsYes(item.requiredFlag))
            {
                dropdownlist.Required = true;
            }

            if (item.requiredFeeCalc == ACAConstant.COMMON_Y && IsDisplayCurrencySymbol)
            {
                string dropdownlistLabelPattern = I18nCultureUtil.UserPreferredCultureInfo.TextInfo.IsRightToLeft ? "({1}){0}" : "{0}({1})";
                dropdownlist.Label = string.Format(dropdownlistLabelPattern, dropdownlist.Label, I18nNumberUtil.CurrencySymbol);
            }

            //if the ASI field associated to drill down, drill down would reset the value list, it is possible to be null.
            if (item.valueList != null && item.valueList.Length > 0)
            {
                foreach (RefAppSpecInfoDropDownModel4WS dropdownItem in item.valueList)
                {
                    if (dropdownItem == null)
                    {
                        continue;
                    }

                    dropdownListItem = new ListItem(I18nStringUtil.GetString(dropdownItem.resAttrValue, dropdownItem.attrValue), dropdownItem.attrValue);
                    dropdownlist.Items.Add(dropdownListItem);
                }
            }

            if (item.checklistComment != null && item.checklistComment != string.Empty)
            {
                DropDownListBindUtil.SetSelectedValue(dropdownlist, item.checklistComment);
            }

            return dropdownlist;
        }

        /// <summary>
        /// create RadioButtonList control
        /// </summary>
        /// <param name="item">application specific info model</param>
        /// <param name="groupIndex">group index</param>
        /// <param name="itemIndex">item index.</param>
        /// <returns>created radio button list</returns>
        private RadioButtonList CreateRadioButtonList(AppSpecificInfoModel4WS item, int groupIndex, int itemIndex)
        {
            AccelaRadioButtonList radioButton = new AccelaRadioButtonList();
            radioButton.EnableViewState = false;
            radioButton.Label = Server.HtmlEncode(CapUtil.GetASIFieldLabel(item));
            radioButton.FieldAlignment = ConvertString2FieldAlignment(item.alignment);
            radioButton.SubLabel = I18nStringUtil.GetCurrentLanguageString(item.resInstruction, item.instruction);

            radioButton.Attributes.Add(ACAConstant.ASI_GROUP_NAME, item.groupCode);
            radioButton.Attributes.Add(ACAConstant.ASI_SUB_GROUP_NAME, item.checkboxType);
            radioButton.Attributes.Add(ACAConstant.ASI_FIELD_NAME, item.fieldLabel);

            if (ValidationUtil.IsHidden(item.vchDispFlag))
            {
                radioButton.IsHidden = true;
                radioButton.Attributes.Add(ACAConstant.IS_HIDDEN, "true");
            }
            else if (ValidationUtil.IsYes(item.requiredFlag))
            {
                radioButton.Required = true;
            }

            if (item.requiredFeeCalc == ACAConstant.COMMON_Y && IsDisplayCurrencySymbol)
            {
                string radioButtonLabelPattern = I18nCultureUtil.UserPreferredCultureInfo.TextInfo.IsRightToLeft ? ":({1}){0}" : "{0}({1}):";
                radioButton.Label = string.Format(radioButtonLabelPattern, radioButton.Label, I18nNumberUtil.CurrencySymbol);
            }

            radioButton.RepeatDirection = System.Web.UI.WebControls.RepeatDirection.Horizontal;
            radioButton.ID = "rdo_" + groupIndex + "_" + itemIndex;
            radioButton.Attributes.Add("onclick", "hideCbError(this)");

            //In ASI - use 'Yes' or 'No' as radio-control's value.
            ListItem radioY = new ListItem(GetTextByKey("ACA_RadioButtonText_Yes"), ACAConstant.COMMON_Yes);
            ListItem radioN = new ListItem(GetTextByKey("ACA_RadioButtonText_No"), ACAConstant.COMMON_No);

            radioButton.Items.Add(radioY);
            radioButton.Items.Add(radioN);

            if (!string.IsNullOrEmpty(item.checklistComment))
            {
                string selectedValue = I18nStringUtil.GetString(item.resChecklistComment, item.checklistComment);

                if (ValidationUtil.IsYes(selectedValue))
                {
                    selectedValue = ACAConstant.COMMON_Yes;
                }

                if (ValidationUtil.IsNo(selectedValue))
                {
                    selectedValue = ACAConstant.COMMON_No;
                }

                radioButton.SelectedValue = selectedValue;
            }

            return radioButton;
        }

        /// <summary>
        /// Creates the textbox.
        /// </summary>
        /// <param name="item">The ASI field item.</param>
        /// <param name="multiline">if set to <c>true</c> [multiline].</param>
        /// <param name="groupIndex">Index of the group.</param>
        /// <param name="itemIndex">Index of the item.</param>
        /// <param name="is4FeeEstimator">if set to <c>true</c> [is4 fee estimator].</param>
        /// <returns>created text box</returns>
        private AccelaTextBox CreateTextbox(AppSpecificInfoModel4WS item, bool multiline, int groupIndex, int itemIndex, bool is4FeeEstimator)
        {
            AccelaTextBox textbox = CreateTextbox(item);
            textbox.ID = "txt_" + groupIndex + "_" + itemIndex;
            textbox.WatermarkText = I18nStringUtil.GetCurrentLanguageString(item.resWaterMark, item.waterMark);
            textbox.SubLabel = I18nStringUtil.GetCurrentLanguageString(item.resInstruction, item.instruction);
            textbox.Attributes.Add(ACAConstant.ASI_GROUP_NAME, item.groupCode);
            textbox.Attributes.Add(ACAConstant.ASI_SUB_GROUP_NAME, item.checkboxType);
            textbox.Attributes.Add(ACAConstant.ASI_FIELD_NAME, item.fieldLabel);

            int fieldType = int.Parse(item.fieldType);
            string checklistComment = I18nStringUtil.GetString(item.resChecklistComment, item.checklistComment);
            textbox.Text = ControlBuildHelper.FormatFieldValue((FieldType)fieldType, checklistComment);

            textbox.Label = Server.HtmlEncode(CapUtil.GetASIFieldLabel(item));

            if (item.requiredFeeCalc == ACAConstant.COMMON_Y && IsDisplayCurrencySymbol)
            {
                string textBoxLabelPattern = I18nCultureUtil.UserPreferredCultureInfo.TextInfo.IsRightToLeft ? "({1}){0}" : "{0}({1})";
                textbox.Label = string.Format(textBoxLabelPattern, textbox.Label, I18nNumberUtil.CurrencySymbol);
            }

            int itemLength = int.Parse(item.maxLength);

            if (is4FeeEstimator && itemLength > 16)
            {
                itemLength = 16;
            }

            //TextArea limited to 4000 matching with AA Classic
            if (itemLength > MAX_LENGTH_4K || itemLength <= 0)
            {
                itemLength = MAX_LENGTH_4K;
            }

            //It need max length limit in ASI SPEAR form, but it needn't in ASI search form.
            if (!IsASISearchForm)
            {
                if (itemLength == MAX_LENGTH_4K && int.Parse(item.fieldType) == (int)FieldType.HTML_TEXTBOX_OF_CURRENCY)
                {
                    textbox.MaxLength = ACAConstant.CURRENCY_MAX_LENGTH;
                }
                else
                {
                    textbox.MaxLength = itemLength;
                }
            }

            if (ValidationUtil.IsHidden(item.vchDispFlag))
            {
                textbox.IsHidden = true;
                textbox.Attributes.Add(ACAConstant.IS_HIDDEN, "true");
            }
            else if (ValidationUtil.IsYes(item.requiredFlag))
            {
                if (textbox.MaxLength > 0)
                {
                    textbox.Validate = "required;maxlength";
                }
                else
                {
                    textbox.Validate = "required";
                }
            }
            else
            {
                if (textbox.MaxLength > 0)
                {
                    textbox.Validate = "maxlength";
                }
            }

            if (multiline)
            {
                textbox.TextMode = TextBoxMode.MultiLine;
                textbox.Rows = 5;
            }

            textbox.CssClass = "ACA_NLonger";

            return textbox;
        }

        /// <summary>
        /// Creates the textbox.
        /// </summary>
        /// <param name="item">The ASI field item.</param>
        /// <returns>created text box</returns>
        private AccelaTextBox CreateTextbox(AppSpecificInfoModel4WS item)
        {
            AccelaTextBox textbox;

            switch (int.Parse(item.fieldType))
            {
                case (int)FieldType.HTML_TEXTBOX_OF_DATE:
                    textbox = new AccelaCalendarText { IsHijriDate = IsDisplayHijriCalendar };
                    break;
                case (int)FieldType.HTML_TEXTBOX_OF_CURRENCY:
                    textbox = new AccelaNumberText();
                    ((AccelaNumberText)textbox).DecimalDigitsLength = ACAConstant.CURRENCY_MAX_DIGITS_LENGTH;
                    ((AccelaNumberText)textbox).Validate = "DecimalTotalLength";
                    ((AccelaNumberText)textbox).IsNeedNegative = true;

                    break;
                case (int)FieldType.HTML_TEXTBOX_OF_NUMBER:
                    textbox = new AccelaNumberText();
                    ((AccelaNumberText)textbox).IsNeedNegative = true;

                    break;
                case (int)FieldType.HTML_TEXTBOX_OF_TIME:
                    textbox = new AccelaTimeText();
                    ((AccelaTimeText)textbox).IsIgnoreValidate = IsASISearchForm;
                    break;
                default:
                    textbox = new AccelaTextBox();
                    break;
            }

            textbox.FieldAlignment = ConvertString2FieldAlignment(item.alignment);

            return textbox;
        }

        #endregion Methods

        #region Nested Types

        /// <summary>
        /// The expression relation
        /// </summary>
        protected struct ExpressionRelation
        {
            #region Fields

            /// <summary>
            /// Control id.
            /// </summary>
            public string ControlID;

            /// <summary>
            /// Expression name
            /// </summary>
            public string ExperssionName;

            #endregion Fields
        }

        /// <summary>
        /// The expression system input parameters
        /// </summary>
        private static class ExpressionSysInputParams
        {
            #region Fields

            /// <summary>
            /// Cap ID 1 string name
            /// </summary>
            public const string CapID1 = "capID1";

            /// <summary>
            /// Cap ID 2 string name
            /// </summary>
            public const string CapID2 = "capID2";

            /// <summary>
            /// Cap ID 3 string name
            /// </summary>
            public const string CapID3 = "capID3";

            /// <summary>
            /// department string name
            /// </summary>
            public const string Department = "department";

            /// <summary>
            /// First Name string name
            /// </summary>
            public const string FirstName = "firstName";

            /// <summary>
            /// Last Name string name.
            /// </summary>
            public const string LastName = "lastName";

            /// <summary>
            /// Middle string Name
            /// </summary>
            public const string MiddleName = "middleName";

            /// <summary>
            /// Module string name
            /// </summary>
            public const string Module = "module";

            /// <summary>
            /// The email for the Public User
            /// </summary>
            public const string Publicuser_email = "publicuser_email";

            /// <summary>
            /// Server provider code
            /// </summary>
            public const string ServProvCode = "servProvCode";

            /// <summary>
            /// Today flag
            /// </summary>
            public const string Today = "today";

            /// <summary>
            /// User Group
            /// </summary>
            public const string UserGroup = "userGroup";

            /// <summary>
            /// User ID name
            /// </summary>
            public const string UserID = "userID";

            /// <summary>
            /// User Full Name
            /// </summary>
            public const string Userfullname = "userfullname";

            #endregion Fields
        }

        #endregion Nested Types
    }
}
