#region Header

/**
* <pre>
*
*  Accela Citizen Access
*  File: ControlBuilder.cs
*
*  Accela, Inc.
*  Copyright (C): 2008-2014
*
*  Description:
*  Control Builder Util.
*
*  Notes:
*      $Id: ControlBuildHelper.cs 279229 2014-10-15 08:50:37Z ACHIEVO\james.shi $.
*  Revision History
*  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
* </pre>
*/

#endregion Header

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Accela.ACA.BLL.APO;
using Accela.ACA.BLL.Cap;
using Accela.ACA.BLL.Common;
using Accela.ACA.Common;
using Accela.ACA.Common.Common;
using Accela.ACA.Common.Util;
using Accela.ACA.UI.Model;
using Accela.ACA.WSProxy;
using Accela.ACA.WSProxy.Common;
using Accela.Web.Controls;
using Accela.Web.Controls.ControlRender;

namespace Accela.ACA.Web.Common.Control
{
    /// <summary>
    /// Control Builder Tools
    /// </summary>
    public static class ControlBuildHelper
    {
        #region Fields

        /// <summary>
        /// Checked value of the CheckBox field.
        /// </summary>
        public const string CHK_SELECT = "on";

        /// <summary>
        /// Default width for control
        /// </summary>
        public const int DEFAULT_CONTROL_WIDTH = 140; // 20 chars

        /// <summary>
        /// Public variable colon
        /// </summary>
        public const string COLON = ":";

        /// <summary>
        /// Max length attribute value
        /// </summary>
        private const int MAX_LENGTH_ATTRIBUTE_VALUE = 200;

        /// <summary>
        /// True string format
        /// </summary>
        private const string TRUE_FLAG = "true";

        #endregion Fields

        #region Methods

        /// <summary>
        /// Get the Permission object
        /// </summary>
        /// <param name="sectionId">section id</param>
        /// <param name="permissionLevel">permission Level</param>
        /// <param name="permissionValue">permission Value</param>
        /// <param name="genTemplate">The generic template.</param>
        /// <returns>permission entity.</returns>
        public static GFilterScreenPermissionModel4WS GetPermissionWithGenericTemplate(string sectionId, string permissionLevel, string permissionValue, TemplateModel genTemplate = null)
        {
            GFilterScreenPermissionModel4WS permission = new GFilterScreenPermissionModel4WS();
            permission.permissionLevel = permissionLevel;
            permission.permissionValue = permissionValue;

            if (genTemplate != null && genTemplate.templateForms != null && genTemplate.templateForms.Length != 0)
            {
                permission.permissionValue += ACAConstant.SPLIT_DOUBLE_COLON + genTemplate.templateForms[0].groupName;
            }
            else if (GviewID.ExistGenericTemplateViewIds.Contains(sectionId) && !string.IsNullOrEmpty(permissionValue))
            {
                ITemplateBll templateBll = ObjectFactory.GetObject<ITemplateBll>();
                GenericTemplateEntityType entityType = GViewUtil.GetEntityType(sectionId);
                string templateGroupCode = templateBll.GetTemplateAssociateASIGroup(entityType, permissionValue);

                if (!string.IsNullOrEmpty(templateGroupCode))
                {
                    permission.permissionValue += ACAConstant.SPLIT_DOUBLE_COLON + templateGroupCode;
                }
            }

            return permission;
        }

        /// <summary>
        /// add required validation with standard fields according to Admin config
        /// </summary>
        /// <param name="sectionId">section Id</param>
        /// <param name="moduleName">module name</param>
        /// <param name="permission">GFilterScreenPermission model.</param>
        /// <param name="controls">Control collection.</param>
        public static void AddValidationForStandardFields(string sectionId, string moduleName, GFilterScreenPermissionModel4WS permission, ControlCollection controls)
        {
            IGviewBll gviewBll = ObjectFactory.GetObject<IGviewBll>();

            SimpleViewElementModel4WS[] models = gviewBll.GetSimpleViewElementModel(moduleName, permission, sectionId, AppSession.User.UserID);
            
            if (models == null)
            {
                return;
            }

            foreach (SimpleViewElementModel4WS model in models)
            {
                if (ValidationUtil.IsNo(model.standard) || 
                    ACAConstant.INVALID_STATUS.Equals(model.recStatus, StringComparison.OrdinalIgnoreCase) ||
                    string.Equals(model.elementType, ControlType.Line.ToString(), StringComparison.InvariantCultureIgnoreCase))
                {
                    continue;
                }

                if (ValidationUtil.IsYes(model.required))
                {
                    AddRequiredStandardFields(model, controls);
                }
            }
        }

        /// <summary>
        /// if section is not required, add the validate function to each field that is set to required.
        /// </summary>
        /// <param name="controls">section's controls</param>
        /// <param name="functionName">validate function name</param>
        public static void AddValidationFuctionForRequiredFields(ControlCollection controls, string functionName)
        {
            foreach (System.Web.UI.Control control in controls)
            {
                if (control is IAccelaControl)
                {
                    if (control is AccelaTextBox)
                    {
                        AccelaTextBox accelaCtl = control as AccelaTextBox;
                        if (!string.IsNullOrEmpty(accelaCtl.Validate) &&
                            accelaCtl.Validate.IndexOf("required", StringComparison.InvariantCultureIgnoreCase) > -1)
                        {
                            accelaCtl.CheckControlValueValidateFunction = functionName;
                        }
                    }
                    else if (control is AccelaStateControl)
                    {
                        AccelaStateControl accelaCtl = control as AccelaStateControl;
                        if (!string.IsNullOrEmpty(accelaCtl.Validate) &&
                            accelaCtl.Validate.IndexOf("required", StringComparison.InvariantCultureIgnoreCase) > -1)
                        {
                            accelaCtl.CheckControlValueValidateFunction = functionName;
                        }
                    }
                    else if (control is AccelaDropDownList)
                    {
                        AccelaDropDownList accelaCtl = control as AccelaDropDownList;
                        if (accelaCtl.Required)
                        {
                            accelaCtl.CheckControlValueValidateFunction = functionName;
                        }
                    }
                    else if (control is AccelaRadioButtonList)
                    {
                        AccelaRadioButtonList accelaCtl = control as AccelaRadioButtonList;
                        if (accelaCtl.Required)
                        {
                            accelaCtl.CheckControlValueValidateFunction = functionName;
                        }
                    }
                    else if (control is AccelaCheckBox)
                    {
                        AccelaCheckBox accelaCtl = control as AccelaCheckBox;
                        if (accelaCtl.Required)
                        {
                            accelaCtl.CheckControlValueValidateFunction = functionName;
                        }
                    }
                }
                else if (control.HasControls())
                {
                    AddValidationFuctionForRequiredFields(control.Controls, functionName);
                }
            }
        }

        /// <summary>
        /// Set property(required and disable) for  accela control.
        /// </summary>
        /// <param name="sectionId">section Id</param>
        /// <param name="moduleName">module name</param>
        /// <param name="permission">permission object</param>
        /// <param name="controls">control collection</param>
        /// <param name="needResetControlStatus">is need to reset the control's status, enable/disable</param>
        public static void SetPropertyForStandardFields(string sectionId, string moduleName, GFilterScreenPermissionModel4WS permission, ControlCollection controls, bool needResetControlStatus)
        {
            IGviewBll gviewBll = ObjectFactory.GetObject<IGviewBll>();

            SimpleViewElementModel4WS[] models = gviewBll.GetSimpleViewElementModel(moduleName, permission, sectionId, AppSession.User.UserID);

            if (models == null)
            {
                return;
            }

            foreach (SimpleViewElementModel4WS model in models)
            {
                if (ValidationUtil.IsNo(model.standard) ||
                    ACAConstant.INVALID_STATUS.Equals(model.recStatus, StringComparison.OrdinalIgnoreCase) ||
                    string.Equals(model.elementType, ControlType.Line.ToString(), StringComparison.InvariantCultureIgnoreCase))
                {
                    continue;
                }

                if (ValidationUtil.IsYes(model.required))
                {
                    AddRequiredStandardFields(model, controls);
                }

                if (needResetControlStatus && !AppSession.IsAdmin && ACAConstant.VALID_STATUS.Equals(model.recStatus, StringComparison.OrdinalIgnoreCase))
                {
                    DisableControl(model, controls);
                }
            }
        }
        
        /// <summary>
        /// Converts TemplateAttributeModel to IControlEntity interface.
        /// </summary>
        /// <param name="field">TemplateAttributeModel model</param>
        /// <param name="prefix4ControlID">Prefix for template control id.</param>
        /// <returns>An instance of IControlEntity.</returns>
        public static IControlEntity BuildControlEntity(TemplateAttributeModel field, string prefix4ControlID)
        {
            ControlEntity ctlEntity = new ControlEntity();

            ctlEntity.ControlID = TemplateUtil.GetTemplateControlID(field.attributeName.Trim(), prefix4ControlID);
            ctlEntity.Label = I18nStringUtil.GetString(field.attributeLabel, field.attributeName);
            ctlEntity.Instruction = I18nStringUtil.GetCurrentLanguageString(field.resInstruction, field.instruction);
            ctlEntity.Watermark = I18nStringUtil.GetCurrentLanguageString(field.resWaterMark, field.waterMark);

            //Need not to set the default value for template field in ACA Admin
            if (!AppSession.IsAdmin)
            {
                ctlEntity.DefaultValue = field.attributeValue;
            }

            ctlEntity.Required = ACAConstant.COMMON_Y.Equals(field.attributeValueReqFlag, StringComparison.InvariantCulture);
            ctlEntity.UnitType = I18nStringUtil.GetCurrentLanguageString(field.resAttributeUnitType, field.attributeUnitType);

            if (field.selectOptions != null &&
                field.selectOptions.Length > 0)
            {
                foreach (TemplateAttrValueModel attValue in field.selectOptions)
                {
                    ItemValue item = new ItemValue();
                    item.Key = attValue.attributeValue;
                    item.Value = I18nStringUtil.GetString(attValue.resAttributeValue, attValue.attributeValue);

                    ctlEntity.Items.Add(item);
                }
            }

            return ctlEntity;
        }

        /// <summary>
        /// Construct the ControlEntity object for generic template field for UI control creation.
        /// </summary>
        /// <param name="field">Generic template field.</param>
        /// <returns>Instance of IControlEntity.</returns>
        public static IControlEntity BuildControlEntity(GenericTemplateAttribute field)
        {
            ControlEntity ctlEntity = new ControlEntity();
            ctlEntity.ControlID = GetGenericTemplateControlID(field);
            ctlEntity.Label = I18nStringUtil.GetString(field.displayFieldName, field.fieldName);

            if (field.acaTemplateConfig != null)
            {
                ACATemplateConfigModel configModel = field.acaTemplateConfig;
                string altLabel = I18nStringUtil.GetString(configModel.resFieldLabel, configModel.fieldLabel);
                ctlEntity.Label = I18nStringUtil.GetString(altLabel, ctlEntity.Label);
                ctlEntity.Instruction = I18nStringUtil.GetCurrentLanguageString(configModel.resInstruction, configModel.instruction);
                ctlEntity.Watermark = I18nStringUtil.GetCurrentLanguageString(configModel.resWaterMark, configModel.waterMark);
            }           

            //Need not to set the default value for template field in ACA Admin
            if (!AppSession.IsAdmin)
            {
                ctlEntity.DefaultValue = field.defaultValue;
            }

            ctlEntity.Required = ACAConstant.COMMON_Y.Equals(field.requireFlag, StringComparison.InvariantCulture);
            ctlEntity.UnitType = field.unitType;

            if (field.options  != null && field.options.Length > 0)
            {
                foreach (var option in field.options)
                {
                    ItemValue item = new ItemValue();
                    item.Key = option.key;
                    item.Value = option.value;

                    ctlEntity.Items.Add(item);
                }
            }

            return ctlEntity;
        }

        /// <summary>
        /// Convert char Width to Pixel Width. 
        /// </summary>
        /// <param name="charWidth">char width</param>
        /// <returns>Pixel Width</returns>
        public static int ConvertCharWidthToPixWidth(string charWidth)
        {
            int width = DEFAULT_CONTROL_WIDTH;
            if (ValidationUtil.IsInt(charWidth))
            {
                //for current css font the control width should be char width * 7.
                width = Convert.ToInt32(charWidth) * 7;
            }

            if (width <= 0)
            {
                width = DEFAULT_CONTROL_WIDTH;
            }

            return width;
        }

        /// <summary>
        /// Creates a calendar control by the IControlEntity data.
        /// </summary>
        /// <param name="control">a instance of IControlEntity.</param>
        /// <returns>AccelaCalendarText web control.</returns>
        public static AccelaCalendarText CreateCalendar(IControlEntity control)
        {
            AccelaCalendarText calendar = new AccelaCalendarText();
            calendar.ID = control.ControlID;
            calendar.Label = control.Label + COLON;
            calendar.LabelKey = control.LabelKey;
            calendar.Validate = control.Required ? "required" : string.Empty;
            calendar.Text = control.DefaultValue;
            calendar.CssClass = "ACA_NShot";
            if (!string.IsNullOrEmpty(control.UnitType))
            {
                calendar.FieldUnit = control.UnitType;
            }

            return calendar;
        }

        /// <summary>
        /// Create cap type drop down list control
        /// </summary>
        /// <param name="control">IControlEntity object</param>
        /// <param name="moduleName">module name</param>
        /// <param name="vchType">string control type</param>
        /// <returns>AccelaDropDownList object</returns>
        public static AccelaDropDownList CreateCapTypeDropDownList(IControlEntity control, string moduleName, string vchType)
        {
            AccelaDropDownList ddl = new AccelaDropDownList();
            ddl.ID = control.ControlID;
            ddl.Label = control.Label + COLON;
            ddl.LabelKey = control.LabelKey;
            ddl.Required = control.Required;

            // get cap type items
            ICapTypeBll capTypeBll = (ICapTypeBll)ObjectFactory.GetObject(typeof(ICapTypeBll));
            List<ListItem> listItems = capTypeBll.GetCapTypeItems(moduleName, vchType);

            // if no specific module, don't need the default value "--select--".
            bool needDefalutValue = string.IsNullOrEmpty(moduleName) ? false : true;

            DropDownListBindUtil.BindDDL(listItems, ddl, needDefalutValue, needDefalutValue);

            ddl.SetValue(control.DefaultValue);
            if (!string.IsNullOrEmpty(control.UnitType))
            {
                ddl.FieldUnit = control.UnitType;
            }

            return ddl;
        }

        /// <summary>
        /// Creates a DropDownList control by the IControlEntity data.
        /// </summary>
        /// <param name="control">a instance of IControlEntity.</param>
        /// <returns>AccelaDropDownList web control.</returns>
        public static AccelaDropDownList CreateDropDownList(IControlEntity control)
        {
            AccelaDropDownList ddl = new AccelaDropDownList();
            ddl.ID = control.ControlID;
            ddl.Label = control.Label + COLON;
            ddl.LabelKey = control.LabelKey;
            ddl.Required = control.Required;
            ddl.SubLabel = control.Instruction;

            List<ListItem> listItems = new List<ListItem>();
            foreach (ItemValue item in control.Items)
            {
                ListItem listItem = new ListItem();
                listItem.Value = item.Key;
                if (item.Value != null)
                {
                    listItem.Text = item.Value.ToString();
                }
                else
                {
                    listItem.Text = string.Empty;
                }

                listItems.Add(listItem);
            }

            // Binding the Drop Down list should not be sorted because its items were ordered. 
            DropDownListBindUtil.BindDDL(listItems, ddl, true, false);
            DropDownListBindUtil.SetSelectedValue(ddl, control.DefaultValue);

            if (!string.IsNullOrEmpty(control.UnitType))
            {
                ddl.FieldUnit = control.UnitType;
            }

            return ddl;
        }

        /// <summary>
        /// Creates the list box.
        /// </summary>
        /// <param name="control">The control.</param>
        /// <param name="allowMultipleSelection">if set to <c>true</c> [allow multiple selection].</param>
        /// <returns>
        /// the list box.
        /// </returns>
        public static AccelaListBox CreateListBox(IControlEntity control, bool allowMultipleSelection)
        {
            var lb = new AccelaListBox();
            lb.ID = control.ControlID;
            lb.Label = control.Label + COLON;
            lb.LabelKey = control.LabelKey;
            lb.Required = control.Required;
            lb.SelectionMode = allowMultipleSelection ? ListSelectionMode.Multiple : ListSelectionMode.Single;
            
            List<ListItem> listItems = new List<ListItem>();

            foreach (ItemValue item in control.Items)
            {
                ListItem listItem = new ListItem();
                listItem.Value = item.Key;
                listItem.Text = item.Value != null ? item.Value.ToString() : string.Empty;
                listItems.Add(listItem);
            }

            lb.Items.AddRange(listItems.ToArray());
            lb.SetValue(control.DefaultValue);

            if (!string.IsNullOrEmpty(control.UnitType))
            {
                lb.FieldUnit = control.UnitType;
            }

            return lb;
        }

        /// <summary>
        /// Creates a Number TextBox control by the IControlEntity data.
        /// </summary>
        /// <param name="control">a instance of IControlEntity.</param>
        /// <returns>AccelaNumberText web control.</returns>
        public static AccelaNumberText CreateNumberText(IControlEntity control)
        {
            AccelaNumberText numberText = new AccelaNumberText();
            numberText.ID = control.ControlID;
            numberText.Label = control.Label + COLON;
            numberText.LabelKey = control.LabelKey;
            numberText.MaxLength = control.MaxLength;
            numberText.Validate = control.Required ? "required" : string.Empty;
            numberText.Text = I18nNumberUtil.ConvertNumberFromWebServiceToInput(control.DefaultValue);
            numberText.CssClass = control.CssClass;
            numberText.MaxLength = 30;
            if (!string.IsNullOrEmpty(control.UnitType))
            {
                numberText.FieldUnit = control.UnitType;
            }

            return numberText;
        }

        /// <summary>
        /// Creates a RadioButton control by the IControlEntity data.
        /// </summary>
        /// <param name="control">a instance of IControlEntity.</param>
        /// <returns>AccelaRadioButton web control.</returns>
        public static AccelaRadioButtonList CreateRadioList(IControlEntity control)
        {
            AccelaRadioButtonList radioList = new AccelaRadioButtonList();
            radioList.ID = control.ControlID;
            radioList.Label = control.Label + COLON;
            radioList.LabelKey = control.LabelKey;
            radioList.RepeatDirection = RepeatDirection.Horizontal;
            radioList.Required = control.Required;

            //ListItem li = new ListItem(ACAConstant.COMMON_Y, ACAConstant.COMMON_Y);
            ListItem li = new ListItem(BasePage.GetStaticTextByKey("ACA_RadioButtonText_Yes"), ACAConstant.COMMON_Yes);

            if (ValidationUtil.IsYes(control.DefaultValue))
            {
                li.Selected = true;
            }

            radioList.Items.Add(li);

            //li = new ListItem(ACAConstant.COMMON_N, ACAConstant.COMMON_N);
            li = new ListItem(BasePage.GetStaticTextByKey("ACA_RadioButtonText_No"), ACAConstant.COMMON_No);
                        
            if (ValidationUtil.IsNo(control.DefaultValue))
            {
                li.Selected = true;
            }

            radioList.Items.Add(li);
            if (!string.IsNullOrEmpty(control.UnitType))
            {
                radioList.FieldUnit = control.UnitType;
            }

            return radioList;
        }

        /// <summary>
        /// Creates a TextBox control by the IControlEntity data.
        /// </summary>
        /// <param name="control">a instance of IControlEntity.</param>
        /// <returns>AccelaTextBox web control.</returns>
        public static AccelaTextBox CreateTextBox(IControlEntity control)
        {
            AccelaTextBox txtBox = new AccelaTextBox();
            txtBox.ID = control.ControlID;
            txtBox.Label = control.Label + COLON;
            txtBox.LabelKey = control.LabelKey;
            txtBox.Validate = control.Required ? "required" : string.Empty;
            txtBox.Text = control.DefaultValue;
            txtBox.CssClass = control.CssClass;
            txtBox.MaxLength = MAX_LENGTH_ATTRIBUTE_VALUE;
            if (!string.IsNullOrEmpty(control.UnitType))
            {
                txtBox.FieldUnit = control.UnitType;
            }

            return txtBox;
        }

        /// <summary>
        /// Create a label control is specially for unit.
        /// </summary>
        /// <param name="unitName">unit name.</param>
        /// <returns>AccelaLabel control.</returns>
        public static AccelaLabel CreateUnitLabel(string unitName)
        {
            AccelaLabel label = new AccelaLabel();
            label.Text = unitName;

            return label;
        }

        /// <summary>
        /// create a unit label for control.
        /// </summary>
        /// <param name="unitName">string unit name</param>
        /// <param name="parentControl">parent control</param>
        /// <param name="vchDispFlag">the display flag of Yes, Hidden or No</param>
        public static void CreateUnitLabel(string unitName, System.Web.UI.Control parentControl, string vchDispFlag)
        {
            bool isHiddenControl = false;

            if (!string.IsNullOrEmpty(vchDispFlag) 
                && vchDispFlag.Equals(ACAConstant.COMMON_H, StringComparison.InvariantCultureIgnoreCase))
            {
                isHiddenControl = true;
            }

            if (!string.IsNullOrEmpty(unitName) && !isHiddenControl)
            {
                Literal lit = new Literal();
                lit.Text = "<div class=\"ACA_Template_Unit\">";
                parentControl.Controls.Add(lit);

                AccelaLabel lblUnit = ControlBuildHelper.CreateUnitLabel(unitName);
                parentControl.Controls.Add(lblUnit);

                lit = new Literal();
                lit.Text = "</div>";
                parentControl.Controls.Add(lit);
            }
        }

        /// <summary>
        /// Generate field label and append currency symbol if the field's RequiredFeeCalculate is Y.
        /// </summary>
        /// <param name="originalLabel">Original field label</param>
        /// <param name="requiredFeeCalc">Fee calculate flag</param>
        /// <returns>Field label may include the currency symbol.</returns>
        public static string CreateLabelWithCurrencySymbol(string originalLabel, string requiredFeeCalc)
        {
            string label = originalLabel;

            if (ValidationUtil.IsYes(requiredFeeCalc))
            {
                string labelPatternForMoneySymbol = I18nCultureUtil.UserPreferredCultureInfo.TextInfo.IsRightToLeft ? "({1}){0}" : "{0}({1})";
                label = string.Format(labelPatternForMoneySymbol, label, I18nNumberUtil.CurrencySymbol);
            }

            return label;
        }

        /// <summary>
        /// Form Field value by field type for ASI/ASIT/Generic template.
        /// </summary>
        /// <param name="fieldType">field type</param>
        /// <param name="value">target format value</param>
        /// <returns>return formatted value.</returns>
        public static string FormatFieldValue(FieldType fieldType, string value)
        {
            string result = string.Empty;

            switch (fieldType)
            {
                case FieldType.HTML_TEXTBOX_OF_TIME:
                    result = value;
                    break;
                case FieldType.HTML_TEXTBOX_OF_DATE:
                    result = I18nDateTimeUtil.ConvertDateStringFromWebServiceToUI(value);
                    break;
                case FieldType.HTML_TEXTBOX_OF_CURRENCY:
                case FieldType.HTML_TEXTBOX_OF_NUMBER:
                    result = I18nNumberUtil.ConvertDecimalFromWebServiceToInput(value);
                    break;
                default:
                    result = value;
                    break;
            }

            return result;
        }

        /// <summary>
        /// get value of specify control 
        /// </summary>
        /// <param name="ctl">specify control </param>
        /// <returns>control value</returns>
        public static string GetControlValue(WebControl ctl)
        {
            string controlValue = string.Empty;
            if (ctl as RadioButtonList != null)
            {
                RadioButtonList rb = ctl as RadioButtonList;
                controlValue = rb.SelectedValue.ToString();
            }

            if (ctl as AccelaDropDownList != null)
            {
                AccelaDropDownList rb = ctl as AccelaDropDownList;
                controlValue = rb.SelectedValue.ToString();
            }

            if (ctl is AccelaNumberText)
            {
                AccelaNumberText rb = ctl as AccelaNumberText;
                controlValue = rb.GetInvariantDoubleText();
            }
            else if (ctl is AccelaTextBox)
            {
                AccelaTextBox rb = ctl as AccelaTextBox;
                controlValue = rb.Text;
            }
            else if (ctl is TextBox)
            {
                TextBox rb = ctl as TextBox;
                controlValue = rb.Text;
            }

            if (ctl as CheckBoxList != null)
            {
                CheckBoxList rb = ctl as CheckBoxList;
                controlValue = rb.SelectedValue.ToString();
            }

            if (ctl as CheckBox != null)
            {
                CheckBox rb = ctl as CheckBox;
                controlValue = rb.Text;
            }

            return controlValue;
        }

        /// <summary>
        /// Gets value from UI control for ASIT fields and generic template fields.
        /// </summary>
        /// <param name="request">HttpRequest object.</param>
        /// <param name="controlType">control type</param>
        /// <param name="controlID">Control ID.</param>
        /// <param name="originalValue">Original value.</param>
        /// <param name="isForSearchForm">For search form, CheckBox control will got the empty value if the CheckBox is unchecked.</param>
        /// <returns>String of control value.</returns>
        public static string GetControlValue(HttpRequest request, int controlType, string controlID, string originalValue, bool isForSearchForm = false)
        {
            string result = originalValue;

            switch (controlType)
            {
                case (int)FieldType.HTML_SELECTBOX:
                    if (request.Params[controlID] != null)
                    {
                        result = request.Params[controlID];
                    }

                    //dropdownlist default value is "--select--"
                    if (WebConstant.DropDownDefaultText.Equals(result, StringComparison.InvariantCulture))
                    {
                        result = string.Empty;
                    }

                    break;
                case (int)FieldType.HTML_CHECKBOX:
                    /* Precondition: the checkbox will not exists [Disabled] attribute and default value, OR the request.Params[controlID] cannot get value.
                     * In current system, the [Disabled] attribute replaced by [Readonly].
                     */ 
                    result = isForSearchForm ? string.Empty : ACAConstant.COMMON_UNCHECKED;

                    if (request.Params[controlID] != null)
                    {
                        result = request.Params[controlID];

                        if (CHK_SELECT.Equals(result, StringComparison.InvariantCulture)
                            || ACAConstant.COMMON_CHECKED.Equals(result, StringComparison.InvariantCulture))
                        {
                            result = ACAConstant.COMMON_CHECKED;
                        }
                    }

                    break;
                case (int)FieldType.HTML_TEXTBOX_OF_DATE:
                    if (request.Params[controlID] != null)
                    {
                        result = request.Params[controlID];

                        if (!string.IsNullOrEmpty(result))
                        {
                            try
                            {
                                string controlClientState = controlID + ACAConstant.CLIENT_STATE;

                                foreach (var key in request.Params.AllKeys)
                                {
                                    if (!string.IsNullOrEmpty(key)
                                        && key.EndsWith(controlClientState)
                                        && request[key].Equals(ACAConstant.ISLAMIC_CALENDAR, StringComparison.OrdinalIgnoreCase))
                                    {
                                        result = HijriDateUtil.ToGregorianDate(result);
                                        break;
                                    }
                                }

                                result = I18nDateTimeUtil.ConvertDateStringFromUIToWebService(result);
                            }
                            catch (Exception ex)
                            {
                                System.Console.WriteLine(ex.Message);
                            }
                        }
                    }

                    break;

                case (int)FieldType.HTML_TEXTBOX_OF_CURRENCY:
                case (int)FieldType.HTML_TEXTBOX_OF_NUMBER:
                    if (request.Params[controlID] != null)
                    {
                        result = request.Params[controlID];
                        double parsedNumber = 0;

                        if (I18nNumberUtil.TryParseNumberFromInput(result, out parsedNumber))
                        {
                            result = I18nNumberUtil.ConvertNumberToInvariantString(parsedNumber);
                        }
                    }

                    break;
                case (int)FieldType.HTML_RADIOBOX:
                    if (request.Params[controlID] != null)
                    {
                        result = request.Params[controlID];
                    }
                    else
                    {
                        result = string.Empty;
                    }

                    break;
                default:
                    if (request.Params[controlID] != null)
                    {
                        result = request.Params[controlID];
                    }

                    break;
            }

            return result;
        }

        /// <summary>
        /// Generate control ID for generic template control.
        /// </summary>
        /// <param name="template">Generic template model.</param>
        /// <returns>String of control ID.</returns>
        public static string GetGenericTemplateControlID(GenericTemplateAttribute template)
        {
            /*
             * For generic template fields, the attribute name and the java side 
             * element name of the SimpleViewElement model must with the same generate rules.
             */
            string attributeName = string.Format(
                "{1}{0}{2}{0}{3}{0}{4}",
                ACAConstant.SPLIT_DOUBLE_COLON,
                ////change it to config agency code. because in supper agency we using the supper agency code to get the 
                //form layout(form designer). if using the sub agency code, the template control ID couldn't mathch in form 
                //designer.
                ConfigManager.AgencyCode,
                template.groupName,
                template.subgroupName,
                template.fieldName);

            return TemplateUtil.GetTemplateControlID(attributeName);
        }

        /// <summary>
        /// Get ASIT field ID by ASIT attributes.
        /// </summary>
        /// <param name="tableKey">Table key.</param>
        /// <param name="rowIdx">Row index.</param>
        /// <param name="colIdxOrFieldName">
        /// Column index or field name. For ASIT:user column index, For Generic template table use field name.
        /// </param>
        /// <returns>Control ID.</returns>
        public static string GetASITFieldID(string tableKey, long rowIdx, string colIdxOrFieldName)
        {
            return string.Format("ASIT_{0}_{1}_{2}", tableKey, rowIdx, colIdxOrFieldName);
        }

        /// <summary>
        /// Get the hidden view element name list.
        /// </summary>
        /// <param name="gvId">Grid view ID.</param>
        /// <param name="moduleName">Module name</param>
        /// <returns>Array of visible view element id.</returns>
        public static string[] GetHiddenViewElementNames(string gvId, string moduleName)
        {
            string[] hiddenViewEltNames = null;

            if (!string.IsNullOrEmpty(gvId))
            {
                IGviewBll gviewBll = (IGviewBll)ObjectFactory.GetObject(typeof(IGviewBll));
                SimpleViewElementModel4WS[] viewElementModels = gviewBll.GetSimpleViewElementModel(moduleName, gvId);
                List<string> hiddenEltNameList = new List<string>();

                if (viewElementModels != null)
                {
                    foreach (SimpleViewElementModel4WS viewElement in viewElementModels)
                    {
                        if (ACAConstant.INVALID_STATUS.Equals(viewElement.recStatus, StringComparison.OrdinalIgnoreCase))
                        {
                            hiddenEltNameList.Add(viewElement.viewElementName);
                        }
                    }
                }

                hiddenViewEltNames = hiddenEltNameList.ToArray();
            }

            return hiddenViewEltNames;
        }

        /// <summary>
        /// hide standard fields according to Admin config
        /// </summary>
        /// <param name="sectionId">section id</param>
        /// <param name="moduleName">module name</param>
        /// <param name="controls">ControlCollection object</param>
        /// <param name="isAdmin">if in admin mode</param>
        public static void HideStandardFields(string sectionId, string moduleName, ControlCollection controls, bool isAdmin)
        {
            HideStandardFields(sectionId, moduleName, controls, isAdmin, null);
        }

        /// <summary>
        /// hide standard fields according to Admin config according to GFilterScreenPermissionModel4WS object
        /// </summary>
        /// <param name="sectionId">section id</param>
        /// <param name="moduleName">module name</param>
        /// <param name="controls">ControlCollection object</param>
        /// <param name="isAdmin">if in admin mode</param>
        /// <param name="permission">GFilterScreenPermissionModel4WS object</param>
        public static void HideStandardFields(string sectionId, string moduleName, ControlCollection controls, bool isAdmin, GFilterScreenPermissionModel4WS permission)
        {
            IGviewBll gviewBll = (IGviewBll)ObjectFactory.GetObject(typeof(IGviewBll));
            SimpleViewElementModel4WS[] models = null;

            if (permission == null)
            {
                models = gviewBll.GetSimpleViewElementModel(moduleName, sectionId);
            }
            else
            {
                models = gviewBll.GetSimpleViewElementModel(moduleName, permission, sectionId, AppSession.User.UserID);
            }

            if (models != null)
            {
                foreach (SimpleViewElementModel4WS model in models)
                {
                    if (model.recStatus == ACAConstant.VALID_STATUS)
                    {
                        continue;
                    }

                    HideStandardFields(model.viewElementName, controls, isAdmin);
                }
            }
        }

        /// <summary>
        /// Indicates the control type whether is supported in current ACA.
        /// </summary>
        /// <param name="controlType">control type.</param>
        /// <returns>True-Valid, False-invalid</returns>
        public static bool IsValidControlType(string controlType)
        {
            if (string.IsNullOrEmpty(controlType))
            {
                return false;
            }

            bool isValid = false;

            foreach (string item in Enum.GetNames(typeof(ControlType)))
            {
                if (item.Equals(controlType, StringComparison.InvariantCultureIgnoreCase))
                {
                    isValid = true;
                    break;
                }
            }

            return isValid;
        }

        /// <summary>
        /// Set value to section instruction.
        /// </summary>
        /// <param name="label">label control used to load value</param>
        /// <param name="value">the instruction value</param>
        public static void SetInstructionValue(AccelaLabel label, string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                label.CssClass = "ACA_Hide";
            }
            else
            {
                label.IsNeedEncode = false;
                label.Text = value;
            }
        }

        /// <summary>
        /// Set value to section instruction.
        /// </summary>
        /// <param name="control">label control used to load value</param>
        /// <param name="value">the instruction value</param>
        public static void SetInstructionValue(HtmlGenericControl control, string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                control.Attributes["class"] = "ACA_Hide";
            }
            else
            {
                control.InnerHtml = value;
            }
        }

        /// <summary>
        /// Set disable status for web controls.
        /// </summary>
        /// <param name="model">SimpleViewElementModel4WS object</param>
        /// <param name="controls">control collection</param>
        private static void DisableControl(SimpleViewElementModel4WS model, ControlCollection controls)
        {
            System.Web.UI.Control control = null;
            GetControlById(model.viewElementName, controls, ref control);

            if (control != null)
            {
                WebControl wctrl = control as WebControl;

                if (ValidationUtil.IsNo(model.isEditable))
                {
                    ((IAccelaControl)control).DisableEdit();

                    /*
                     * Pass the editable status to HTML/JS client.
                     * HTML/JS client will use the editable status to control the field status.
                     */
                    if (wctrl != null)
                    {
                        wctrl.Attributes.Add("data-editable", "false");
                    }
                }
                else
                {
                    ((IAccelaControl)control).EnableEdit();

                    if (wctrl != null)
                    {
                        wctrl.Attributes.Remove("data-editable");
                    }
                }
            }
        }

        /// <summary>
        /// Find the control by control id
        /// </summary>
        /// <param name="controlId">control id</param>
        /// <param name="controls">control collection</param>
        /// <param name="result">control returned</param>
        private static void GetControlById(string controlId, ControlCollection controls, ref System.Web.UI.Control result)
        {
            foreach (System.Web.UI.Control ctrl in controls)
            {
                if (result == null)
                {
                    if (ctrl is IAccelaControl && string.Equals(ctrl.ID, controlId, StringComparison.OrdinalIgnoreCase))
                    {
                        result = ctrl;
                    }
                    else if (ctrl.HasControls())
                    {
                        GetControlById(controlId, ctrl.Controls, ref result);
                    }
                }

                if (result != null)
                {
                    break;
                }
            }
        }

        /// <summary>
        /// Set Required for standard field.
        /// </summary>
        /// <param name="control">target control</param>
        private static void SetRequired(System.Web.UI.Control control)
        {
            if (control is AccelaTextBox)
            {
                AccelaTextBox accelaCtl = control as AccelaTextBox;
                accelaCtl.Validate += string.IsNullOrEmpty(accelaCtl.Validate) ? "required" : ";required";
            }
            else if (control is AccelaStateControl)
            {
                AccelaStateControl accelaCtl = control as AccelaStateControl;
                accelaCtl.Validate += string.IsNullOrEmpty(accelaCtl.Validate) ? "required" : ";required";
            }
            else if (control is AccelaTimeSelection)
            {
                AccelaTimeSelection accelaCtl = control as AccelaTimeSelection;
                accelaCtl.Validate += string.IsNullOrEmpty(accelaCtl.Validate) ? "required" : ";required";
            }
            else if (control is AccelaCheckBoxList)
            {
                AccelaCheckBoxList accelaCtl = control as AccelaCheckBoxList;
                accelaCtl.Required = true;

                if (!AppSession.IsAdmin && accelaCtl.Visible)
                {
                    accelaCtl.RequiredValidator();
                }
            }
            else if (control is AccelaCheckBox)
            {
                AccelaCheckBox accelaCtl = control as AccelaCheckBox;
                accelaCtl.Required = true;
            }
            else if (control is AccelaDropDownList)
            {
                AccelaDropDownList accelaCtl = control as AccelaDropDownList;
                accelaCtl.Required = true;
            }
            else if (control is AccelaRadioButtonList)
            {
                AccelaRadioButtonList accelaCtl = control as AccelaRadioButtonList;
                accelaCtl.Required = true;
                if (!AppSession.IsAdmin)
                {
                    accelaCtl.AddRequiredValidator();
                }
            }
            else if (control is AccelaListBox)
            {
                AccelaListBox accelaCtl = control as AccelaListBox;
                accelaCtl.Required = true;
            }
        }

        /// <summary>
        /// Add required validation with standard field control Who is base on administration configuration.
        /// </summary>
        /// <param name="model">standard field object</param>
        /// <param name="controls">page's all control</param>
        private static void AddRequiredStandardFields(SimpleViewElementModel4WS model, ControlCollection controls)
        {
            foreach (System.Web.UI.Control control in controls)
            {
                if (control is IAccelaControl &&
                    control.ID == model.viewElementName)
                {
                    if (control is AccelaTextBox)
                    {
                        AccelaTextBox accelaCtl = control as AccelaTextBox;
                        accelaCtl.Validate += string.IsNullOrEmpty(accelaCtl.Validate) ? "required" : ";required";
                    }
                    else if (control is AccelaRangeNumberText)
                    {
                        AccelaRangeNumberText accelaCtl = control as AccelaRangeNumberText;
                        accelaCtl.IsFieldRequired = true;
                    }
                    else if (control is AccelaStateControl)
                    {
                        AccelaStateControl accelaCtl = control as AccelaStateControl;
                        accelaCtl.Validate += string.IsNullOrEmpty(accelaCtl.Validate) ? "required" : ";required";
                    }
                    else if (control is AccelaCheckBoxList)
                    {
                        AccelaCheckBoxList accelaCtl = control as AccelaCheckBoxList;
                        accelaCtl.Required = true;

                        /*
                         * At this time, checkbox such as Virtual Folder maybe be invisible. But, it maybe change to visible on render.
                         * So, there is no need to check whether it is visible or not.
                         */
                        if (!AppSession.IsAdmin)
                        {
                            accelaCtl.RequiredValidator();
                        }
                    }
                    else if (control is AccelaCheckBox)
                    {
                        AccelaCheckBox accelaCtl = control as AccelaCheckBox;
                        accelaCtl.Required = true;
                    }
                    else if (control is AccelaDropDownList)
                    {
                        AccelaDropDownList accelaCtl = control as AccelaDropDownList;
                        accelaCtl.Required = true;
                    }
                    else if (control is AccelaRadioButtonList)
                    {
                        AccelaRadioButtonList accelaCtl = control as AccelaRadioButtonList;
                        accelaCtl.Required = true;
                        if (!AppSession.IsAdmin)
                        {
                            accelaCtl.AddRequiredValidator();
                        }
                    }
                    else if (control is AccelaListBox)
                    {
                        AccelaListBox accelaCtl = control as AccelaListBox;
                        accelaCtl.Required = true;
                    }
                    else if (control is AccelaMultipleControl)
                    {
                        AccelaMultipleControl accelaCtl = control as AccelaMultipleControl;
                        accelaCtl.IsFieldRequired = true;
                    }

                    break;
                }
                else if (control.HasControls())
                {
                    AddRequiredStandardFields(model, control.Controls);
                }
            }
        }

        /// <summary>
        /// Create AccelaTextBox based on the field type.
        /// Used by ASIT and Generic template field's creation.
        /// </summary>
        /// <param name="fieldType">value of the Accela.ACA.Common.FieldType.</param>
        /// <param name="isHidden">Indicates the field whether is hidden.</param>
        /// <param name="isRequired">Indicates the field whether is required.</param>
        /// <returns>AccelaTextBox object.</returns>
        private static AccelaTextBox CreateTextbox(int fieldType, bool isHidden, bool isRequired)
        {
            AccelaTextBox textbox;
            switch (fieldType)
            {
                case (int)FieldType.HTML_TEXTBOX_OF_DATE:
                    textbox = new AccelaCalendarText();
                    break;
                case (int)FieldType.HTML_TEXTBOX_OF_CURRENCY:
                    textbox = new AccelaNumberText();
                    textbox.MaxLength = ACAConstant.CURRENCY_MAX_LENGTH;
                    ((AccelaNumberText)textbox).DecimalDigitsLength = ACAConstant.CURRENCY_MAX_DIGITS_LENGTH;
                    ((AccelaNumberText)textbox).Validate = "DecimalDigitsLength";
                    break;
                case (int)FieldType.HTML_TEXTBOX_OF_NUMBER:
                    textbox = new AccelaNumberText();
                    break;
                case (int)FieldType.HTML_TEXTBOX_OF_TIME:
                    textbox = new AccelaTimeText();
                    break;
                default:
                    textbox = new AccelaTextBox();
                    break;
            }

            if (isHidden)
            {
                textbox.IsHidden = true;
                textbox.Attributes.Add(ACAConstant.IS_HIDDEN, TRUE_FLAG);
            }
            else if (isRequired)
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

            return textbox;
        }

        /// <summary>
        /// Hide standard fields.
        /// </summary>
        /// <param name="id">string control id.</param>
        /// <param name="controls">ControlCollection object</param>
        /// <param name="isAdmin">true if in admin</param>
        private static void HideStandardFields(string id, ControlCollection controls, bool isAdmin)
        {
            foreach (System.Web.UI.Control control in controls)
            {
                if (control is IAccelaControl &&
                    control.ID == id)
                {
                    if (isAdmin)
                    {
                        ((IAccelaControl)control).ClientVisible = false;
                    }
                    else
                    {
                        control.Visible = false;
                    }

                    break;
                }
                else if (control.Controls.Count > 0)
                {
                    HideStandardFields(id, control.Controls, isAdmin);
                }
            }
        }

        #endregion Methods

        #region Nested Types

        /// <summary>
        /// Create control for application specific information table
        /// </summary>
        public static class ASIT
        {
            #region Fields

            /// <summary>
            /// Char length.
            /// </summary>
            private const int CHAR_LENGTH = 8;

            /// <summary>
            /// Length for check box.
            /// </summary>
            private const int CHECKBOX_LENGTH = 80;

            /// <summary>
            /// Length for check box.
            /// </summary>
            private const int CHECKBOX_REALLY_LENGTH = 100;

            /// <summary>
            /// Length for date icon.
            /// </summary>
            private const int DATE_ICON_LENGTH = 15;

            /// <summary>
            /// Length for date input.
            /// </summary>
            private const int DATE_TEXTBOX_LENGTH = 72;

            /// <summary>
            /// Length for drop-down.
            /// </summary>
            private const int DROPDOWNLISTH_LENGTH = 126;

            /// <summary>
            /// Length for error icon.
            /// </summary>
            private const int ERROR_ICON = 15;

            /// <summary>
            /// Length for number input.
            /// </summary>
            private const int NUMBER_TEXTBOX_LENGTH = 72;

            /// <summary>
            /// Length for radio button.
            /// </summary>
            private const int RADIOBUTTON_LENGYH = 70;

            /// <summary>
            /// Width for redundant.
            /// </summary>
            private const int REDUNDANT_WIDTH = 5;

            /// <summary>
            /// Length for request fee.
            /// </summary>
            private const int REQUEST_FEE_LENGTH = 4;

            /// <summary>
            /// Length for text area.
            /// </summary>
            private const int TEXTAREA_LENGTH = 146;

            /// <summary>
            /// Default length for text box.
            /// </summary>
            private const int TEXTBOX_LENGTH = 80;

            /// <summary>
            /// Length for text box.
            /// </summary>
            private const int TEXTBOX_REALLY_LENTGH = 170;

            #endregion Fields

            #region Methods

            /// <summary>
            /// According to item type, to create corresponding control.
            /// </summary>
            /// <param name="field">ASIT UI field.</param>
            /// <param name="isTemplateTable">
            /// Is generic template table. There is a different logic between ASIT Y/N field and generic template Y/N field.
            /// </param>
            /// <param name="isASITSearchForm">is asit search form</param>
            /// <returns>Web control.</returns>
            public static WebControl CreateWebControl(ASITUIField field, bool isTemplateTable, bool isASITSearchForm = false)
            {
                if (field == null)
                {
                    return null;
                }

                WebControl control = null;

                switch (int.Parse(field.Type))
                {
                    case (int)FieldType.HTML_RADIOBOX:
                        control = CreateRadioButtonList(field, isTemplateTable, isASITSearchForm);
                        break;
                    case (int)FieldType.HTML_SELECTBOX:
                        control = CreateDropdownList(field, isASITSearchForm);
                        break;
                    case (int)FieldType.HTML_TEXTAREABOX:
                        control = CreateTextbox(field, true, isASITSearchForm);
                        break;
                    case (int)FieldType.HTML_CHECKBOX:
                        control = CreateCheckbox(field, isASITSearchForm);
                        break;
                    default:
                        control = CreateTextbox(field, false, isASITSearchForm);
                        break;
                }

                SetControlLength(field, control);

                if (!isASITSearchForm)
                {
                    //Disable web control if it is readonly by ASIT security.
                    ASISecurityUtil.DisableFieldForSecurity(
                        control,
                        (FieldType)Enum.Parse(typeof(FieldType), field.Type),
                        field.IsReadOnly);
                }

                return control;
            }

            /// <summary>
            /// Get length by column item.
            /// </summary>
            /// <param name="field">ASIT UI field.</param>
            /// <param name="flag">boolean flag.</param>
            /// <returns>the length for the column item.</returns>
            public static int GetLength(ASITUIField field, bool flag)
            {
                //string displayLength = item.DisplayLength;
                int controlWidth = 0;

                if (field.DisplayLength != 0)
                {
                    controlWidth = ConvertCharWidthToPixWidth(field.DisplayLength.ToString());
                }
                else
                {
                    controlWidth = GetDefaultLength(field, flag);
                }

                if ((int.Parse(field.Type) == (int)FieldType.HTML_TEXTBOX_OF_DATE) && flag)
                {
                    controlWidth = DATE_ICON_LENGTH + controlWidth;
                }

                if (flag)
                {
                    controlWidth += REDUNDANT_WIDTH;
                    controlWidth += ERROR_ICON;
                }

                return controlWidth;
            }

            /// <summary>
            /// Create Checkbox control
            /// </summary>
            /// <param name="field">ASIT UI field.</param>
            /// <param name="isASITSearchForm">is asit search form</param>
            /// <returns>Checkbox control.</returns>
            private static CheckBox CreateCheckbox(ASITUIField field, bool isASITSearchForm)
            {
                AccelaCheckBox checkbox = new AccelaCheckBox();
                checkbox.SubLabel = field.Instruction;
                checkbox.ID = field.FieldID;

                if (field.IsHidden)
                {
                    checkbox.Style.Add("display", "none");
                    checkbox.Attributes.Add(ACAConstant.IS_HIDDEN, TRUE_FLAG);
                }

                if (!isASITSearchForm)
                {
                    checkbox.Text = CreateLabelWithCurrencySymbol(field.Label, field.RequiredFeeCalc);
                    checkbox.IsFixedUniqueID = true;

                    if (!field.IsHidden && field.IsRequired)
                    {
                        checkbox.Required = true;
                    }
                }
                else
                {
                    checkbox.Label = CreateLabelWithCurrencySymbol(field.Label, field.RequiredFeeCalc) + COLON;
                }

                checkbox.Checked = field.DefaultValue == "CHECKED" ? true : false;
                checkbox.IsInASITable = true;

                return checkbox;
            }

            /// <summary>
            /// Create DropdownList control
            /// </summary>
            /// <param name="field">ASIT UI field</param>
            /// <param name="isASITSearchForm">is asit search form</param>
            /// <returns>AccelaDropDownList control.</returns>
            private static AccelaDropDownList CreateDropdownList(ASITUIField field, bool isASITSearchForm)
            {
                AccelaDropDownList dropdownlist = new AccelaDropDownList();
                dropdownlist.ID = field.FieldID;
                dropdownlist.SubLabel = field.Instruction;
                dropdownlist.Label = CreateLabelWithCurrencySymbol(field.Label, field.RequiredFeeCalc) + COLON;

                ListItem dropdownListItem = new ListItem(WebConstant.DropDownDefaultText, string.Empty);
                dropdownlist.Items.Add(dropdownListItem);
                dropdownListItem.Selected = true;

                if (field.IsHidden)
                {
                    dropdownlist.IsHidden = true;
                    dropdownlist.Attributes.Add(ACAConstant.IS_HIDDEN, TRUE_FLAG);
                }
                
                if (!isASITSearchForm)
                {
                    dropdownlist.EnableViewState = false;
                    dropdownlist.IsFixedUniqueID = true;

                    if (!field.IsHidden && field.IsRequired)
                    {
                        dropdownlist.Required = true;
                    }
                }

                if (field.ValueList == null)
                {
                    return dropdownlist;
                }

                foreach (RefAppSpecInfoDropDownModel4WS dropdownItem in field.ValueList)
                {
                    if (dropdownItem == null)
                    {
                        continue;
                    }

                    dropdownListItem = new ListItem(I18nStringUtil.GetString(dropdownItem.resAttrValue, dropdownItem.attrValue), dropdownItem.attrValue);

                    dropdownlist.Items.Add(dropdownListItem);
                }

                return dropdownlist;
            }

            /// <summary>
            /// Create RadioButtonList control.
            /// </summary>
            /// <param name="field">ASIT UI field.</param>
            /// <param name="isTemplateTable">Is generic template table.</param>
            /// <param name="isASITSearchForm">Is asit search form</param>
            /// <returns>Created RadioButtonList object.</returns>
            private static RadioButtonList CreateRadioButtonList(ASITUIField field, bool isTemplateTable, bool isASITSearchForm)
            {
                AccelaRadioButtonList radioButton = new AccelaRadioButtonList();
                radioButton.CssClass = "aca_checkbox aca_checkbox_fontsize";
                radioButton.SubLabel = field.Instruction;

                radioButton.Label = CreateLabelWithCurrencySymbol(field.Label, field.RequiredFeeCalc) + COLON;
                 
                if (field.IsHidden)
                {
                    radioButton.IsHidden = true;
                    radioButton.Attributes.Add(ACAConstant.IS_HIDDEN, TRUE_FLAG);
                }

                if (!isASITSearchForm)
                {
                    radioButton.EnableViewState = false;

                    if (!field.IsHidden && field.IsRequired)
                    {
                        radioButton.Required = true;
                    }

                    radioButton.IsFixedUniqueID = true;
                }

                radioButton.RepeatDirection = RepeatDirection.Horizontal;
                radioButton.ID = field.FieldID;
                radioButton.Attributes.Add("onclick", "hideCbError(this)");

                /*
                 * ASIT Y/N field use 'Yes' or 'No' as value.
                 * Generic template Y/N field use 'Y' or 'N' as value.
                 */
                ListItem radioY = new ListItem(BasePage.GetStaticTextByKey("ACA_RadioButtonText_Yes"), isTemplateTable ? ACAConstant.COMMON_Y : ACAConstant.COMMON_Yes);
                ListItem radioN = new ListItem(BasePage.GetStaticTextByKey("ACA_RadioButtonText_No"), isTemplateTable ? ACAConstant.COMMON_N : ACAConstant.COMMON_No);
                radioButton.Items.Add(radioY);
                radioButton.Items.Add(radioN);

                if (!string.IsNullOrEmpty(field.DefaultValue))
                {
                    if (ValidationUtil.IsYes(field.DefaultValue))
                    {
                        radioButton.SelectedValue = ACAConstant.COMMON_Yes;
                    }
                    else
                    {
                        radioButton.SelectedValue = ACAConstant.COMMON_No;
                    }
                }

                radioButton.CellPadding = -1;
                radioButton.CellSpacing = -1;

                return radioButton;
            }

            /// <summary>
            /// Create Textbox control
            /// </summary>
            /// <param name="field">ASIT UI field.</param>
            /// <param name="multiline">true if the column has multiple lines.</param>
            /// <param name="isASITSearchForm">is asit search form</param>
            /// <returns>Created AccelaTextBox object.</returns>
            private static AccelaTextBox CreateTextbox(ASITUIField field, bool multiline, bool isASITSearchForm)    
            {
                AccelaTextBox textbox = ControlBuildHelper.CreateTextbox(int.Parse(field.Type), field.IsHidden, field.IsRequired);
                textbox.ID = field.FieldID;
                textbox.SubLabel = field.Instruction;
                textbox.WatermarkText = field.Watermark;

                switch (int.Parse(field.Type))
                {
                    case (int)FieldType.HTML_TEXTBOX_OF_CURRENCY:
                    case (int)FieldType.HTML_TEXTBOX_OF_NUMBER:
                        ((AccelaNumberText)textbox).IsNeedNegative = true;
                        break;

                    case (int)FieldType.HTML_TEXTBOX_OF_DATE:
                        AccelaCalendarText calendarText = textbox as AccelaCalendarText;
                        calendarText.IsHijriDate = StandardChoiceUtil.IsEnableDisplayISLAMICCalendar();
                        break;
                }

                if (!isASITSearchForm)
                {
                    textbox.IsFixedUniqueID = true;
                }

                if (!string.IsNullOrEmpty(field.DefaultValue))
                {
                    switch (int.Parse(field.Type))
                    {
                        //if column type is "Time" the columnType is 7,So should be format it.
                        case (int)FieldType.HTML_TEXTBOX_OF_TIME:
                            textbox.Text = field.DefaultValue;
                            break;

                        //if column type is "Date" the columnType is 2,So should be format it.
                        case (int)FieldType.HTML_TEXTBOX_OF_DATE:
                            textbox.Text = I18nDateTimeUtil.ConvertDateStringFromWebServiceToUI(field.DefaultValue);
                            break;

                        case (int)FieldType.HTML_TEXTBOX_OF_CURRENCY:
                        case (int)FieldType.HTML_TEXTBOX_OF_NUMBER:
                            textbox.Text = I18nNumberUtil.ConvertNumberFromWebServiceToInput(field.DefaultValue);
                            break;

                        default:
                            textbox.Text = field.DefaultValue;
                            break;
                    }
                }

                textbox.Label = CreateLabelWithCurrencySymbol(field.Label, field.RequiredFeeCalc) + COLON;
                int itemLength = field.MaxLength;

                if (itemLength > MAX_LENGTH_ATTRIBUTE_VALUE ||
                    itemLength <= 0)
                {
                    itemLength = MAX_LENGTH_ATTRIBUTE_VALUE;
                }

                //if the textbox setting MaxLength and the MaxLength small than itemLength use the MaxLength.
                if (textbox.MaxLength == 0 || itemLength < textbox.MaxLength)
                {
                    textbox.MaxLength = itemLength;
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
            /// Get default length by column item style.
            /// </summary>
            /// <param name="field">ASIT UI field.</param>
            /// <param name="flag">
            /// if flag as true, will get a more length, become maybe has a date icon.
            /// </param>
            /// <returns>default length</returns>
            private static int GetDefaultLength(ASITUIField field, bool flag)
            {
                int length = 0;
                switch (int.Parse(field.Type))
                {
                    case (int)FieldType.HTML_RADIOBOX:
                        length = RADIOBUTTON_LENGYH;
                        break;

                    case (int)FieldType.HTML_TEXTBOX_OF_DATE:
                        length = DATE_TEXTBOX_LENGTH;
                        break;

                    case (int)FieldType.HTML_SELECTBOX:
                        // length = DROPDOWNLISTH_LENGTH;//set default value is biggest item of select box items
                        break;

                    case (int)FieldType.HTML_TEXTAREABOX:
                        length = TEXTAREA_LENGTH;
                        break;

                    case (int)FieldType.HTML_TEXTBOX_OF_NUMBER:
                        length = NUMBER_TEXTBOX_LENGTH;
                        break;

                    case (int)FieldType.HTML_CHECKBOX:

                        if (flag)
                        {
                            length = CHECKBOX_REALLY_LENGTH;
                        }
                        else
                        {
                            length = CHECKBOX_LENGTH;
                        }

                        break;

                    default:

                        if (flag)
                        {
                            length = TEXTBOX_REALLY_LENTGH;
                        }
                        else
                        {
                            length = TEXTBOX_LENGTH;
                        }

                        break;
                }

                return length;
            }

            /// <summary>
            /// Control value char is longer than default value,  will get the value char length.
            /// </summary>
            /// <param name="item">AppSpecificTableColumnModel4WS object</param>
            /// <returns>the length of the value</returns>
            private static int GetValueLength(AppSpecificTableColumnModel4WS item)
            {
                int type = int.Parse(item.columnType);

                if (type != (int)FieldType.HTML_SELECTBOX)
                {
                    return 0;
                }

                int length = 0;
                int itemLength = 0;
                string itemText = string.Empty;

                foreach (RefAppSpecInfoDropDownModel4WS dropdownItem in item.valueList)
                {
                    if (dropdownItem == null)
                    {
                        continue;
                    }

                    itemText = dropdownItem.attrValue;

                    if (itemText.Length > 0)
                    {
                        itemLength = itemText.Length * CHAR_LENGTH;
                    }

                    if (itemLength > length)
                    {
                        length = itemLength;
                    }
                }

                return length;
            }

            /// <summary>
            /// Set the control length, if length is null, set default length 206
            /// </summary>
            /// <param name="field">ASIT UI field.</param>
            /// <param name="control">Web control.</param>
            private static void SetControlLength(ASITUIField field, WebControl control)
            {
                // step 1: if set display length, will get the display length
                // step 2: no displaylength, will get the default length.
                int length = GetLength(field, false);

                if (!(control is CheckBox))
                {
                    // fix the width of dropdownlist, the standard
                    if ((control is DropDownList || control is AccelaDropDownList) && (length == DEFAULT_CONTROL_WIDTH))
                    {
                        control.CssClass += " asi_ddl_width";
                    }
                    else
                    {
                        // Use unit EM instead of PX, keep the decimal
                        control.Style.Add("width", (length / 10.0) + "em");
                    }
                }

                control.Style.Add("vertical-align", "top");
            }

            #endregion Methods
        }

        /// <summary>
        /// Control build helper for generic template fields.
        /// </summary>
        public static class GenericTemplate
        {
            #region Properties

            /// <summary>
            /// Gets the module name from request.
            /// </summary>
            private static string ModuleName
            {
                get
                {
                    if (!string.IsNullOrEmpty(HttpContext.Current.Request.QueryString[ACAConstant.MODULE_NAME]))
                    {
                        return ScriptFilter.AntiXssHtmlEncode(HttpContext.Current.Request.QueryString[ACAConstant.MODULE_NAME]);
                    }
                    else
                    {
                        return string.Empty;
                    }
                }
            }

            #endregion Properties

            /// <summary>
            /// Create web control for generic template field.
            /// </summary>
            /// <param name="controlContainer">The control container to be used to create the special validator control.</param>
            /// <param name="field">Generic template field.</param>
            /// <param name="isHidden">if set to <c>true</c> [is hidden].</param>
            /// <returns>Web control.</returns>
            public static WebControl CreateWebControl(System.Web.UI.Control controlContainer, GenericTemplateAttribute field, bool isHidden)
            {
                if (field == null)
                {
                    return null;
                }

                WebControl control = null;
                IControlEntity controlEntity = BuildControlEntity(field);

                switch (field.fieldType)
                {
                    case (int)FieldType.HTML_CHECKBOX:
                        control = CreateCheckbox(controlEntity);
                        break;

                    case (int)FieldType.HTML_RADIOBOX:
                        control = CreateRadioList(controlContainer, controlEntity);
                        break;

                    case (int)FieldType.HTML_SELECTBOX:
                        control = CreateDropDownList(controlEntity);
                        break;

                    case (int)FieldType.HTML_TEXTAREABOX:
                        control = CreateTextbox(field, controlEntity, true);
                        break;

                    default:
                        control = CreateTextbox(field, controlEntity, false);
                        break;
                }

                string asiSecurity = ACAConstant.ASISecurity.Full;

                if (!AppSession.IsAdmin)
                {
                    asiSecurity = ASISecurityUtil.GetASISecurity(field.serviceProviderCode, field.groupName, field.subgroupName, field.fieldName, ModuleName);
                }

                //if generic template field is hidden, not to display, but render it.
                if (isHidden || (field.acaTemplateConfig != null && (ValidationUtil.IsHidden(field.acaTemplateConfig.acaDisplayFlag) || ValidationUtil.IsNo(field.acaTemplateConfig.acaDisplayFlag)))
                    || ACAConstant.ASISecurity.None.Equals(asiSecurity, StringComparison.OrdinalIgnoreCase))
                {
                    ((IAccelaControl)control).IsHidden = true;

                    //Should setting ishidden attribute for expression
                    //if "asi aca display flag" or security hide the control, should not display by expression.
                    control.Attributes.Add(ACAConstant.IS_HIDDEN, TRUE_FLAG);

                    if (control is CheckBox)
                    {
                        CheckBox cb = control as CheckBox;
                        cb.InputAttributes.Add(ACAConstant.IS_HIDDEN, TRUE_FLAG);
                    }
                }

                control.Attributes.Add("data-fieldname", ScriptFilter.EncodeJson(field.fieldName));

                bool isReadOnly = ACAConstant.ASISecurity.Read.Equals(asiSecurity);

                //Disable web control if it is readonly by ASI security.
                ASISecurityUtil.DisableFieldForSecurity(
                    control,
                    (FieldType)Enum.Parse(typeof(FieldType), field.fieldType.ToString(CultureInfo.InvariantCulture.NumberFormat)),
                    isReadOnly);

                if (AppSession.IsAdmin)
                {
                    TemplateAttribute templateAttribute = new TemplateAttribute(field);
                    ControlRenderUtil.CreateDesinSupportExtender(control, templateAttribute);
                }

                return control;
            }

            /// <summary>
            /// Creates a RadioButton control by the IControlEntity data.
            /// </summary>
            /// <param name="controlContainer">The control container to be used to create the special validator control.</param>
            /// <param name="control">a instance of IControlEntity.</param>
            /// <returns>AccelaRadioButton web control.</returns>
            public static AccelaRadioButtonList CreateRadioList(System.Web.UI.Control controlContainer, IControlEntity control)
            {
                AccelaRadioButtonList radioList = new AccelaRadioButtonList();
                radioList.ID = control.ControlID;
                radioList.Label = control.Label + COLON;
                radioList.LabelKey = control.LabelKey;
                radioList.RepeatDirection = RepeatDirection.Horizontal;
                radioList.Required = control.Required;
                radioList.SubLabel = control.Instruction;

                //In generic template solution - use 'Y' or 'N' as radio-control's value.
                ListItem li = new ListItem(BasePage.GetStaticTextByKey("ACA_RadioButtonText_Yes"), ACAConstant.COMMON_Y);

                if (ValidationUtil.IsYes(control.DefaultValue))
                {
                    li.Selected = true;
                }

                radioList.Items.Add(li);

                li = new ListItem(BasePage.GetStaticTextByKey("ACA_RadioButtonText_No"), ACAConstant.COMMON_N);

                if (ValidationUtil.IsNo(control.DefaultValue))
                {
                    li.Selected = true;
                }

                radioList.Items.Add(li);

                if (!string.IsNullOrEmpty(control.UnitType))
                {
                    radioList.FieldUnit = control.UnitType;
                }

                radioList.AddRequiredValidator(controlContainer);

                return radioList;
            }

            /// <summary>
            /// Create CheckBox control for generic template field.
            /// </summary>
            /// <param name="controlEntity">Control entity.</param>
            /// <returns>Checkbox control.</returns>
            private static CheckBox CreateCheckbox(IControlEntity controlEntity)
            {
                var checkbox = new AccelaCheckBox();
                checkbox.ID = controlEntity.ControlID;
                checkbox.SubLabel = controlEntity.Instruction;
                checkbox.Text = controlEntity.Label;
                checkbox.Checked = controlEntity.DefaultValue == "CHECKED" ? true : false;

                /*
                if (field.IsHidden)
                {
                    checkbox.Style.Add("display", "none");
                    checkbox.Attributes.Add(ACAConstant.IS_HIDDEN, TRUE_FLAG);
                }
                else
                */

                if (controlEntity.Required)
                {
                    checkbox.Required = true;
                }

                if (!string.IsNullOrEmpty(controlEntity.UnitType))
                {
                    checkbox.FieldUnit = controlEntity.UnitType;
                }

                return checkbox;
            }

            /// <summary>
            /// Create Textbox control for generic template field.
            /// </summary>
            /// <param name="field">Generic template field.</param>
            /// <param name="controlEntity">Control entity/</param>
            /// <param name="multiline">true if the column has multiple lines.</param>
            /// <returns>Created AccelaTextBox object.</returns>
            private static AccelaTextBox CreateTextbox(GenericTemplateAttribute field, IControlEntity controlEntity, bool multiline)
            {
                AccelaTextBox textbox = ControlBuildHelper.CreateTextbox(field.fieldType, false, controlEntity.Required);
                textbox.ID = controlEntity.ControlID;
                textbox.SubLabel = controlEntity.Instruction;
                textbox.WatermarkText = controlEntity.Watermark;

                if (!string.IsNullOrEmpty(controlEntity.DefaultValue))
                {
                    switch (field.fieldType)
                    {
                        case (int)FieldType.HTML_TEXTBOX_OF_TIME:
                            textbox.Text = controlEntity.DefaultValue;
                            break;

                        case (int)FieldType.HTML_TEXTBOX_OF_DATE:
                            textbox.Text = I18nDateTimeUtil.ConvertDateStringFromWebServiceToUI(controlEntity.DefaultValue);
                            break;

                        case (int)FieldType.HTML_TEXTBOX_OF_CURRENCY:
                        case (int)FieldType.HTML_TEXTBOX_OF_NUMBER:
                            textbox.Text = I18nNumberUtil.ConvertNumberFromWebServiceToInput(controlEntity.DefaultValue);
                            break;

                        default:
                            textbox.Text = controlEntity.DefaultValue;
                            break;
                    }
                }

                textbox.Label = controlEntity.Label + COLON;
                int itemLength = field.maxLen;

                if (itemLength > MAX_LENGTH_ATTRIBUTE_VALUE ||
                    itemLength <= 0)
                {
                    itemLength = MAX_LENGTH_ATTRIBUTE_VALUE;
                }

                //if the textbox setting MaxLength and the MaxLength small than itemLength use the MaxLength.
                if (textbox.MaxLength == 0 || itemLength < textbox.MaxLength)
                {
                    textbox.MaxLength = itemLength;
                }

                if (multiline)
                {
                    textbox.TextMode = TextBoxMode.MultiLine;
                    textbox.Rows = 5;
                }

                textbox.CssClass = "ACA_NLonger";

                if (!string.IsNullOrEmpty(controlEntity.UnitType))
                {
                    textbox.FieldUnit = controlEntity.UnitType;
                }

                return textbox;
            }
        }

        #endregion Nested Types
    }
}