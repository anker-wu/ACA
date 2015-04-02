#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: TemplateEdit.ascx.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2009-2014
 *
 *  Description:
 *
 *  Notes:
 *      $Id: TemplateEdit.ascx.cs 277971 2014-08-25 06:10:06Z ACHIEVO\james.shi $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using System;
using System.Collections.Generic;
using System.Text;
using System.Web.UI.WebControls;

using Accela.ACA.BLL.APO;
using Accela.ACA.Common;
using Accela.ACA.Common.Util;
using Accela.ACA.Web.Common;
using Accela.ACA.Web.Common.Control;
using Accela.ACA.WSProxy;
using Accela.ACA.WSProxy.Common;
using Accela.Web.Controls;
using Accela.Web.Controls.ControlRender;

namespace Accela.ACA.Web.Component
{
    /// <summary>
    /// Provides the ability to dynamical display or get the custom attributes for APO or people template.
    /// </summary>
    public partial class TemplateEdit : BaseUserControl
    {
        #region Fields

        /// <summary>
        /// String builder for script .
        /// </summary>
        private string _validateControlScript = string.Empty;

        /// <summary>
        /// Indicates the dynamical control whether has been created.
        /// </summary>
        private bool _isCreated = false;

        /// <summary>
        /// Indicates the template is for supervisor or not.
        /// </summary>
        private bool _isForSupervisor = false;

        /// <summary>
        /// Script name
        /// </summary>
        private string _scriptName;

        /// <summary>
        /// Template script name
        /// </summary>
        private string _templateScript;

        /// <summary>
        /// Indicate whether the template have always editable field or not.
        /// </summary>
        private bool _hasAlwaysEditableControl = false;

        /// <summary>
        /// Indicate whether the template control need support always edit able or not.
        /// </summary>
        private bool _supportAlwaysEditable = true;

        #endregion Fields

        #region Properties

        /// <summary>
        /// Gets the template user control's panel client id.
        /// </summary>
        public string TemplatePanelClientID
        {
            get
            {
                return ClientID;
            }
        }

        /// <summary>
        /// Gets or sets the template user control's panel client id.       
        /// </summary>
        public string ScriptName
        {
            get
            {
                return _scriptName;
            }

            set
            {
                _scriptName = value;
            }
        }

        /// <summary>
        /// Gets or sets script name
        /// </summary>
        public string TemplateScript
        {
            get
            {
                return _templateScript;
            }

            set
            {
                _templateScript = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the template have always editable field or not.
        /// </summary>
        public bool HasAlwaysEditableControl
        {
            get
            {
                return _hasAlwaysEditableControl;
            }

            set
            {
                _hasAlwaysEditableControl = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether is for supervisor template
        /// </summary>
        public bool IsForSupervisor
        {
            get
            {
                if (ViewState["IsForSupervisor"] == null)
                {
                    return false;
                }

                return (bool)ViewState["IsForSupervisor"];
            }

            set
            {
                ViewState["IsForSupervisor"] = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether is for search.
        /// </summary>
        public bool IsForSearch
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets all of template attributes objects used in current control.
        /// </summary>
        public TemplateAttributeModel[] Fields
        {
            get
            {
                return ViewState["Template_Attributes"] as TemplateAttributeModel[];
            }

            set
            {
                ViewState["Template_Attributes"] = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the template control need support always edit able or not.
        /// </summary>
        public bool SupportAlwaysEditable
        {
            get
            {
                return _supportAlwaysEditable;
            }

            set
            {
                _supportAlwaysEditable = value;
            }
        }

        /// <summary>
        /// Gets or sets current component's template control id prefix.
        /// </summary>
        private string ControlIDPrefix
        {
            get
            {
                if (ViewState["ControlIDPrefix"] == null)
                {
                    return string.Empty;
                }

                return ViewState["ControlIDPrefix"].ToString();
            }

            set
            {
                ViewState["ControlIDPrefix"] = value;
            }
        }

        #endregion Properties

        #region Methods
        /// <summary>
        /// Displays the template or daily attributes to UI.
        /// if there is value in attribute object, the value need to be set as default value.
        /// </summary>
        /// <remarks>
        /// 1.Stores the attributeModels array to ViewState so that 
        ///   the current control can get the attributeModels from ViewState when the page is PostBack
        /// 2.Invoke CreateControls() to create all of web controls dynamically.
        /// </remarks>
        /// <param name="attributeModels">TemplateAttributeModel array to be displayed as a template.</param>
        /// <param name="controlIDPrefix">template control id prefix</param>
        /// <param name="needFillValue">In search form, need fill out the user input values after user use the "IE back" function to reload the search from. </param>
        public void DisplayAttributes(TemplateAttributeModel[] attributeModels, string controlIDPrefix, bool needFillValue = false)
        {
            CreateAttriControls(attributeModels, controlIDPrefix);

            //Need not to set the default value for template field in ACA Admin
            if (!AppSession.IsAdmin && (needFillValue || !IsForSearch))
            {
                // fill the attributeModels value to created controls.
                FillAttributeValues(attributeModels);
            }
        }

        /// <summary>
        /// Fill the template attribute data to current template attribute.
        /// then presents the value to control field.
        /// </summary>
        /// <remarks>
        /// 1.Loop attribute models to match each attribute with previous stored into ViewState.
        ///   If it is matched, set the value to relevant field/control.
        /// </remarks>
        /// <param name="attributeModels">Reference attribute data.</param>
        public void FillAttributeValues(TemplateAttributeModel[] attributeModels)
        {
            //1. Validaton
            if (attributeModels == null || attributeModels.Length == 0)
            {
                return;
            }

            Fields = attributeModels;

            //2. Fills refrence values to current existed control.
            foreach (TemplateAttributeModel field in attributeModels)
            {
                // the control type is invalid, loop to next item.
                if (!ControlBuildHelper.IsValidControlType(field.attributeValueDataType))
                {
                    continue;
                }

                string controlId = TemplateUtil.GetTemplateControlID(field.attributeName, ControlIDPrefix);

                // Finds web control by control id
                WebControl control = FindControl(controlId) as WebControl;

                if (control != null && control is IAccelaControl)
                {
                    string value4Setting = ModelUIFormat.GetI18NTemplateValue(field);

                    if (control is AccelaRadioButtonList)
                    {
                        AccelaRadioButtonList ctl = control as AccelaRadioButtonList;

                        value4Setting = ValidationUtil.IsYes(value4Setting) ? ACA.Common.ACAConstant.COMMON_Yes : value4Setting;
                        value4Setting = ValidationUtil.IsNo(value4Setting) ? ACA.Common.ACAConstant.COMMON_No : value4Setting;

                        ctl.SetValue(value4Setting);
                    }
                    else if (control is IAccelaControl)
                    {
                        IAccelaControl ctl = control as IAccelaControl;
                        ctl.SetValue(value4Setting);
                    }
                }
            }
        }

        /// <summary>
        /// Gets all of attribute objects.
        /// </summary>
        /// <param name="isForSearch">is for search</param>
        /// <returns>AttributeModel4WS array.</returns>
        public TemplateAttributeModel[] GetAttributeModels(bool isForSearch = false)
        {
            return GetAttributeModels(string.Empty, isForSearch);
        }

        /// <summary>
        /// Gets attribute objects.
        /// </summary>
        /// <param name="templateType">template type</param>
        /// <param name="isForSearch">is for search</param>
        /// <returns>template attribute model list</returns>
        public TemplateAttributeModel[] GetAttributeModels(string templateType, bool isForSearch)
        {
            // if there is no fields to be created, return
            if (Fields == null || Fields.Length == 0)
            {
                return null;
            }

            List<TemplateAttributeModel> result = new List<TemplateAttributeModel>();

            foreach (TemplateAttributeModel field in Fields)
            {
                // contiue when the control type is invalid
                if (!ControlBuildHelper.IsValidControlType(field.attributeValueDataType))
                {
                    continue;
                }

                if (!string.IsNullOrEmpty(templateType) && !string.Equals(field.templateType, templateType, StringComparison.InvariantCultureIgnoreCase))
                {
                    continue;
                }

                // The hidden template fields not need in APO search
                if (field.entityType.Equals(TemplateEntityType.APOTEMPLATE) && isForSearch && ValidationUtil.IsNo(field.vchFlag))
                {
                    continue;
                }

                if (IsUnavailableField(field))
                {
                    //if template type is public user , sent the hide field return to the server.
                    if (ACAConstant.COMMON_N.Equals(field.vchFlag) && field.templateType == ACAConstant.PUBLIC_USER)
                    {
                        result.Add(field);
                    }

                    continue;
                }

                // Finds web control by control id
                WebControl control = FindControl(TemplateUtil.GetTemplateControlID(field.attributeName, ControlIDPrefix)) as WebControl;

                if (control != null && control is IAccelaControl)
                {
                    IAccelaControl ctl = control as IAccelaControl;
                    string controlValue = ctl.GetValue().Trim();

                    if (!string.IsNullOrEmpty(control.Attributes[ACAConstant.TEMPLATE_FIELD_NAME_ATTRIBUTE_FOR_EMSEDDL]) && string.IsNullOrEmpty(controlValue))
                    {
                        string controlFormID =
                            control.ClientID.Substring(0, control.ClientID.Length - control.ID.Length).Replace('_', '$') + control.ID;

                        if (Request.Form[controlFormID] != null)
                        {
                            controlValue = Request.Form[controlFormID].ToString();
                        }
                    }

                    if (!string.IsNullOrEmpty(controlValue))
                    {
                        if (ControlType.Date.ToString().Equals(field.attributeValueDataType, StringComparison.OrdinalIgnoreCase))
                        {
                            controlValue = I18nDateTimeUtil.ConvertDateStringFromUIToWebService(controlValue);
                        }
                        else if (ControlType.Number.ToString().Equals(field.attributeValueDataType, StringComparison.OrdinalIgnoreCase))
                        {
                            double parsedNumber = 0;
                            bool isParsedNumberOK = I18nNumberUtil.TryParseNumberFromInput(controlValue, out parsedNumber);

                            controlValue = isParsedNumberOK ? I18nNumberUtil.ConvertNumberToInvariantString(parsedNumber) : controlValue;
                        }
                        else if (ControlType.Radio.ToString().Equals(field.attributeValueDataType, StringComparison.OrdinalIgnoreCase))
                        {
                            controlValue = controlValue.Equals(ACAConstant.COMMON_YES, StringComparison.InvariantCultureIgnoreCase) ? ACAConstant.COMMON_Y : ACAConstant.COMMON_N;
                        }
                    }

                    field.attributeValue = controlValue;
                    result.Add(field);
                }
            }

            return result.ToArray();
        }

        /// <summary>
        /// Judge if all the control value is empty
        /// </summary>
        /// <returns>True-all fields is empty, otherwise returns false.</returns>
        public bool IsControlsValueEmpty()
        {
            TemplateAttributeModel[] attris = GetAttributeModels();

            return TemplateUtil.IsEmpty(attris);
        }

        /// <summary>
        /// Clear Controls if need add different template controls
        /// </summary>
        public void ResetControl()
        {
            // Need clear viewstate when clear date.
            Fields = null;
            _isCreated = false;

            Controls.Clear();
        }

        /// <summary>
        /// Judge the field available or not 
        /// </summary>
        /// <param name="field">Template field</param>
        /// <returns>True or false</returns>
        public bool IsUnavailableField(TemplateAttributeModel field)
        {
            bool isUnavailable = false;

            if (StandardChoiceUtil.IsSuperAgency() && !IsForSupervisor && !string.IsNullOrEmpty(field.attributeScriptCode))
            {
                isUnavailable = true;
            }

            if (StandardChoiceUtil.IsSuperAgency() && IsForSupervisor && string.IsNullOrEmpty(field.attributeScriptCode))
            {
                isUnavailable = true;
            }

            //Always generate field control for APO template even if the displayable is No.
            if (!field.entityType.Equals(TemplateEntityType.APOTEMPLATE))
            {
                if (string.IsNullOrEmpty(field.vchFlag) || ACAConstant.COMMON_N.Equals(field.vchFlag))
                {
                    isUnavailable = true;
                }
            }

            return isUnavailable;
        }

        /// <summary>
        /// After LoadViewState event, re-create the all of template control according to previous stored to ViewState.
        /// Avoid losing the dynamical control when page is post back.
        /// </summary>
        /// <param name="savedState">Saved state.</param>
        protected override void LoadViewState(object savedState)
        {
            base.LoadViewState(savedState);

            // Create dynamicall controls from view state
            if (Fields == null || Fields.Length == 0)
            {
                return;
            }

            CreateControls();
        }

        /// <summary>
        /// PreRender method for templateEdit
        /// </summary>
        /// <param name="e">EventArgs e</param>
        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);
            StringBuilder javascript = new StringBuilder();
            javascript.Append("<script type=\"text/javascript\">");
            javascript.Append(TemplateScript);

            if (!string.IsNullOrEmpty(ScriptName))
            {
                javascript.Append(_validateControlScript);
            }

            javascript.Append("</script>");

            Page.ClientScript.RegisterClientScriptBlock(Page.GetType(), ClientID, javascript.ToString());
        }

        /// <summary>
        /// Displays the template or daily attributes to UI without the attributes value.
        /// </summary>
        /// <remarks>
        /// 1.Stores the attributeModels array to ViewState so that 
        ///   the current control can get the attributeModels from ViewState when the page is PostBack
        /// 2.Invoke CreateControls() to create all of web controls dynamically.
        /// </remarks>
        /// <param name="attributeModels">TemplateAttributeModel array to be displayed as a template.</param>
        /// <param name="controlIDPrefix">Template control id prefix</param>
        private void CreateAttriControls(TemplateAttributeModel[] attributeModels, string controlIDPrefix)
        {
            // stores the attributes data to view state.
            Fields = attributeModels;
            ControlIDPrefix = controlIDPrefix;

            if (attributeModels == null)
            {
                return;
            }

            // Create control dynamically according to the attribute models
            CreateControls();
        }

        /// <summary>
        /// Get the client script's empty validation function name.
        /// </summary>
        /// <param name="isRequired">Is required or not.</param>
        /// <returns>Return the client script's validation function name.</returns>
        private string GetEmptyValidateFunctionName(bool isRequired)
        {
            if (isRequired && !string.IsNullOrEmpty(ScriptName))
            {
                return ScriptName.Replace("Templete_", string.Empty);
            }

            return string.Empty;
        }

        /// <summary>
        /// Get Template Type
        /// </summary>
        /// <returns>Template type</returns>
        private string GetTemplateType()
        {
            string type = string.Empty;

            if (ControlIDPrefix.StartsWith(ACAConstant.CAP_CONTACTS_TEMPLATE_FIELD_PREFIX)
                || ControlIDPrefix.StartsWith(ACAConstant.CAP_APPLICANT_TEMPLATE_FIELD_PREFIX)
                || ControlIDPrefix.StartsWith(ACAConstant.CAP_CONTACT1_TEMPLATE_FIELD_PREFIX)
                || ControlIDPrefix.StartsWith(ACAConstant.CAP_CONTACT2_TEMPLATE_FIELD_PREFIX)
                || ControlIDPrefix.StartsWith(ACAConstant.CAP_CONTACT3_TEMPLATE_FIELD_PREFIX)
                || ControlIDPrefix.StartsWith(ACAConstant.CAP_PROFESSIONAL_TEMPLATE_FIELD_PREFIX)
                || ControlIDPrefix.StartsWith(ACAConstant.REFERENCE_CONTACT_TEMPLATE_FIELD_PREFIX))
            {
                type = "People";
            }
            else if (ControlIDPrefix.StartsWith(ACAConstant.CAP_ADDRESS_TEMPLATE_FIELD_PREFIX)
                || ControlIDPrefix.StartsWith(ACAConstant.CAP_PARCEL_TEMPLATE_FIELD_PREFIX)
                || ControlIDPrefix.StartsWith(ACAConstant.CAP_OWNER_TEMPLATE_FIELD_PREFIX))
            {
                type = "APO";
            }

            return type;
        }

        /// <summary>
        /// Dynamically create attribute controls and initial control value..
        /// </summary>
        /// <remarks>
        /// 1.Gets all AttributeModels from ViewState stored by DisplayAttributes() method.
        /// 2.Loop attributesModels, then create different control according to attribute type 
        ///   and initial default value to control.
        /// </remarks>
        private void CreateControls()
        {
            // Avoids controls to be created repetitively.
            if (_isCreated)
            {
                return;
            }
            else
            {
                _isCreated = true;
            }

            Controls.Clear();

            StringBuilder validateControlScript = new StringBuilder();
            validateControlScript.Append("function ");
            validateControlScript.Append(ScriptName);
            validateControlScript.Append("(){");

            // If there is no fields to be created, return
            if (Fields == null || Fields.Length == 0)
            {
                validateControlScript.Append("return true;}");
                return;
            }

            int fieldCount = Fields.Length;

            validateControlScript.Append("var isNotEmpty=false;");
            validateControlScript.Append("var control;");

            StringBuilder setTemplateValueScript = new StringBuilder("function " + ClientID + "_SetTemplateValue(json){\r\n");

            ITemplateBll templateBll = ObjectFactory.GetObject<ITemplateBll>();

            // Get template type like APO and People
            string type = GetTemplateType();

            // According to the attribute data type to create relevant control.
            for (int i = 0; i < fieldCount; i++)
            {
                TemplateAttributeModel field = Fields[i];

                // Contiue when the control type is invalid
                if (!ControlBuildHelper.IsValidControlType(field.attributeValueDataType))
                {
                    continue;
                }

                if (IsUnavailableField(field))
                {
                    continue;
                }

                // Fill Dropdown List Items
                templateBll.FillTemplateSelectOption(field, type);

                // cConverts TemplateAttributeModel model to IControlEntity.
                IControlEntity ctlEntity = ControlBuildHelper.BuildControlEntity(field, ControlIDPrefix);

                if (IsForSearch)
                {
                    ctlEntity.Required = false;
                    ctlEntity.DefaultValue = string.Empty;
                }

                string controlType = field.attributeValueDataType.ToUpperInvariant();

                if (!controlType.Equals(ControlType.Radio.ToString(), StringComparison.InvariantCultureIgnoreCase))
                {
                    setTemplateValueScript.Append("SetValueById('");
                }
                else
                {
                    setTemplateValueScript.Append("SetRadioListValue('");
                }

                // 1.Create text box control
                if (controlType.Equals(ControlType.Text.ToString(), StringComparison.InvariantCultureIgnoreCase))
                {
                    AccelaTextBox textbox = ControlBuildHelper.CreateTextBox(ctlEntity);

                    if (AppSession.IsAdmin)
                    {
                        TemplateAttribute templateAttribute = new TemplateAttribute(field);
                        ControlRenderUtil.CreateDesinSupportExtender(textbox, templateAttribute);
                    }

                    textbox.WatermarkText = I18nStringUtil.GetCurrentLanguageString(field.resWaterMark, field.waterMark);
                    textbox.SubLabel = I18nStringUtil.GetCurrentLanguageString(field.resInstruction, field.instruction);
                    HandleFieldDisplayConfig(textbox, field);
                    textbox.Attributes.Add("datatype", field.attributeValueDataType);
                    Controls.Add(textbox);
                    textbox.CheckControlValueValidateFunction = GetEmptyValidateFunctionName(ctlEntity.Required);
                    setTemplateValueScript.Append(textbox.ClientID);

                    // Add the empty validation client script
                    validateControlScript.Append(TemplateUtil.CreateEmptyValidationScript(FieldType.HTML_TEXTBOX, textbox.ClientID));
                }
                else if (controlType.Equals(ControlType.Number.ToString(), StringComparison.InvariantCultureIgnoreCase))
                {
                    // 2.Create number textbox control
                    AccelaNumberText numText = ControlBuildHelper.CreateNumberText(ctlEntity);

                    if (AppSession.IsAdmin)
                    {
                        TemplateAttribute templateAttribute = new TemplateAttribute(field);
                        ControlRenderUtil.CreateDesinSupportExtender(numText, templateAttribute);
                    }

                    numText.WatermarkText = I18nStringUtil.GetCurrentLanguageString(field.resWaterMark, field.waterMark);
                    numText.SubLabel = I18nStringUtil.GetCurrentLanguageString(field.resInstruction, field.instruction);
                    HandleFieldDisplayConfig(numText, field);
                    numText.Attributes.Add("datatype", field.attributeValueDataType);
                    Controls.Add(numText);
                    numText.CheckControlValueValidateFunction = GetEmptyValidateFunctionName(ctlEntity.Required);
                    setTemplateValueScript.Append(numText.ClientID);

                    // Add the empty validation client script
                    validateControlScript.Append(TemplateUtil.CreateEmptyValidationScript(FieldType.HTML_TEXTBOX_OF_NUMBER, numText.ClientID));
                }
                else if (controlType.Equals(ControlType.Date.ToString(), StringComparison.InvariantCultureIgnoreCase))
                {
                    // 3.Create date control
                    ctlEntity.DefaultValue = I18nDateTimeUtil.ConvertDateStringFromWebServiceToUI(ctlEntity.DefaultValue);
                    AccelaCalendarText calendar = ControlBuildHelper.CreateCalendar(ctlEntity);

                    if (AppSession.IsAdmin)
                    {
                        TemplateAttribute templateAttribute = new TemplateAttribute(field);
                        ControlRenderUtil.CreateDesinSupportExtender(calendar, templateAttribute);
                    }

                    calendar.WatermarkText = I18nStringUtil.GetCurrentLanguageString(field.resWaterMark, field.waterMark);
                    calendar.SubLabel = I18nStringUtil.GetCurrentLanguageString(field.resInstruction, field.instruction);
                    HandleFieldDisplayConfig(calendar, field);
                    calendar.Attributes.Add("datatype", field.attributeValueDataType);
                    Controls.Add(calendar);
                    calendar.CheckControlValueValidateFunction = GetEmptyValidateFunctionName(ctlEntity.Required);
                    setTemplateValueScript.Append(calendar.ClientID);

                    // Add the empty validation client script
                    validateControlScript.Append(TemplateUtil.CreateEmptyValidationScript(FieldType.HTML_TEXTBOX_OF_DATE, calendar.ClientID));
                }
                else if (controlType.Equals(ControlType.DropdownList.ToString(), StringComparison.InvariantCultureIgnoreCase))
                {
                    // 4.Create dropdownlist control
                    AccelaDropDownList ddl = ControlBuildHelper.CreateDropDownList(ctlEntity);

                    if (AppSession.IsAdmin)
                    {
                        TemplateAttribute templateAttribute = new TemplateAttribute(field);
                        ControlRenderUtil.CreateDesinSupportExtender(ddl, templateAttribute);
                    }

                    ddl.SubLabel = I18nStringUtil.GetCurrentLanguageString(field.resInstruction, field.instruction);
                    HandleFieldDisplayConfig(ddl, field);
                    if (!string.IsNullOrEmpty(field.attributeScriptCode))
                    {
                        ddl.Attributes.Add(ACAConstant.TEMPLATE_FIELD_NAME_ATTRIBUTE_FOR_EMSEDDL, field.attributeName);
                    }

                    ddl.Attributes.Add("datatype", field.attributeValueDataType);
                    Controls.Add(ddl);
                    ddl.CheckControlValueValidateFunction = GetEmptyValidateFunctionName(ctlEntity.Required);
                    setTemplateValueScript.Append(ddl.ClientID);

                    // Add the empty validation client script
                    validateControlScript.Append(TemplateUtil.CreateEmptyValidationScript(FieldType.HTML_SELECTBOX, ddl.ClientID));
                }
                else if (controlType.Equals(ControlType.Radio.ToString(), StringComparison.InvariantCultureIgnoreCase))
                {
                    // 5.Create radio list control
                    AccelaRadioButtonList radioList = ControlBuildHelper.CreateRadioList(ctlEntity);
                    
                    if (AppSession.IsAdmin)
                    {
                        TemplateAttribute templateAttribute = new TemplateAttribute(field);
                        ControlRenderUtil.CreateDesinSupportExtender(radioList, templateAttribute);
                    }

                    radioList.SubLabel = I18nStringUtil.GetCurrentLanguageString(field.resInstruction, field.instruction);
                    HandleFieldDisplayConfig(radioList, field);
                    radioList.Attributes.Add("datatype", field.attributeValueDataType);
                    Controls.Add(radioList);

                    radioList.CheckControlValueValidateFunction = GetEmptyValidateFunctionName(ctlEntity.Required);
                    radioList.AddRequiredValidator();
                    setTemplateValueScript.Append(radioList.ClientID);

                    // Add the empty validation client script
                    validateControlScript.Append(TemplateUtil.CreateEmptyValidationScript(FieldType.HTML_RADIOBOX, radioList.ClientID));
                }

                setTemplateValueScript.Append("',json.");
                setTemplateValueScript.Append(ctlEntity.ControlID);
                setTemplateValueScript.Append(");\r\n");
            }

            setTemplateValueScript.Append("}");
            validateControlScript.Append("return true;}");
            TemplateScript = setTemplateValueScript.ToString();

            if (!string.IsNullOrEmpty(ScriptName))
            {
                _validateControlScript = validateControlScript.ToString();
            }
        }

        /// <summary>
        /// Is always edit control or not.
        /// </summary>
        /// <param name="control">Accela control.</param>
        /// <param name="field">Template attribute model.</param>
        private void HandleFieldDisplayConfig(WebControl control, TemplateAttributeModel field)
        {
            if (!(control is IAccelaControl))
            {
                return;
            }

            if (_supportAlwaysEditable && ACAConstant.TEMPLATE_FIIELD_STATUS_ALWAYSEDITABLE.Equals(field.vchFlag, StringComparison.InvariantCultureIgnoreCase))
            {
                ((IAccelaControl)control).IsAlwaysEditable = true;
                control.Attributes.Add("data-editable", "true");
                _hasAlwaysEditableControl = true;
            }
            else if (ValidationUtil.IsNo(field.vchFlag))
            {
                ((IAccelaControl)control).IsHidden = true;

                //Should setting ishidden attribute for expression
                control.Attributes.Add(ACAConstant.IS_HIDDEN, ACAConstant.COMMON_TRUE);
            }
            else
            {
                control.Attributes.Remove("data-editable");
            }
        }

        #endregion Methods
    }
}
