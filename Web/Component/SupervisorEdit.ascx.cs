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
 *      $Id: SupervisorEdit.ascx.cs 277971 2014-08-25 06:10:06Z ACHIEVO\james.shi $.
 *  Revision History
 *  &lt;Date&gt;, &lt;Who&gt;, &lt;What&gt;
 * </pre>
 */

using System;
using System.Collections;
using System.Data;
using System.Globalization;
using System.Text;
using System.Web.UI.WebControls;

using Accela.ACA.Common;
using Accela.ACA.Common.Util;
using Accela.ACA.Web.Common;
using Accela.ACA.Web.Common.Control;
using Accela.ACA.WSProxy;
using Accela.Web.Controls;

using AjaxControlToolkit;

namespace Accela.ACA.Web.Component
{
    /// <summary>
    /// Provides the ability to dynamical display or get the custom EMSE attributes for APO or people template.
    /// </summary>
    public partial class SupervisorEdit : BaseUserControl
    {
        #region Memeber Variables

        /// <summary>
        /// Drop down list width
        /// </summary>
        private const int DROPDOWNLIST_WIDTH = 240;

        /// <summary>
        /// Indicates the dynamic controls whether has been created.
        /// </summary>
        private bool _isCreated = false;

        /// <summary>
        /// Script for supervisor
        /// </summary>
        private string _script4Supervisor = string.Empty;

        /// <summary>
        /// Script name.
        /// </summary>
        private string _scriptName;

        #endregion  Memeber Variables

        /// <summary>
        /// Gets or sets script name for supervisor
        /// </summary>
        public string ScriptName4Supervisor
        {
            get
            {
                if (ViewState["ScriptName4Supervisor"] != null)
                {
                    return ViewState["ScriptName4Supervisor"].ToString();
                }

                return string.Empty;
            }

            set
            {
                ViewState["ScriptName4Supervisor"] = value;
            }
        }

        /// <summary>
        /// Gets string builder for supervisor script.
        /// </summary>
        public string Script4Supervisor
        {
            get
            {
                return _script4Supervisor;
            }
        }
                
        #region Private properties

        /// <summary>
        /// Gets or sets EMSE template attributes objects used in current control.
        /// </summary>
        private TemplateAttributeModel[] Fields
        {
            get
            {
                return ViewState[ScriptName4Supervisor] as TemplateAttributeModel[];
            }

            set
            {
                ViewState[ScriptName4Supervisor] = value;
            }
        }

        /// <summary>
        /// Gets current component's template control id prefix.
        /// </summary>
        private string TemplateControlIDPrefix
        {
            get
            {
                return ACA.Common.ACAConstant.CAP_PROFESSIONAL_TEMPLATE_FIELD_PREFIX;
            }
        }

        #endregion Private properties

        #region Public Methods

        /// <summary>
        /// Displays the template or daily attributes to UI.
        /// if there is value in attribute object, the value need to be set as default value.
        /// </summary>
        /// <remarks>
        /// 1.Stores the attributeModels array to ViewState so that 
        ///   the current control can get the attributeModels from ViewState when the page is PostBack
        /// 2.Invoke CreateControls() to create all of web controls dynamically.
        /// </remarks>
        /// <param name="attributeModelList">TemplateAttributeModel array to be displayed as a template.</param>
        public void DisplayEMSEAttributes(TemplateAttributeModel[] attributeModelList)
        {
            TemplateAttributeModel[] attributeModels = FilterEMSETemplateAttributes(attributeModelList);
            //// stores the attributes data to view state.
            Fields = attributeModels;

            //// Create control dynamically according to the attribute models 
            CreateControls();

            //// fill the attributeModels value to created controls.
            FillRefEMSEAttributeValues(attributeModels);
        }

        /// <summary>
        /// Fill the reference template attribute data to current template attribute.
        /// then presents the value to control field.
        /// </summary>
        /// <remarks>
        /// 1.Loop reference attribute models to match each attribute with previous stored into ViewState.
        ///   If it is matched, set the value to relevant field/control.
        /// </remarks>
        /// <param name="attributeModelList">Reference attribute data.</param>
        public void FillRefEMSEAttributeValues(TemplateAttributeModel[] attributeModelList)
        {
            //validation begin
            TemplateAttributeModel[] attributeModels = FilterEMSETemplateAttributes(attributeModelList);

            // 1. Validaton
            if (attributeModels == null || attributeModels.Length == 0)
            {
                return;
            }

            if (Fields == null || Fields.Length == 0)
            {
                return;
            }

            // 2. Fills refrence values to current existed control.
            foreach (TemplateAttributeModel field in attributeModels)
            {
                // The control type is invalid, loop to next item.
                if (!ControlBuildHelper.IsValidControlType(field.attributeValueDataType))
                {
                    continue;
                }

                string controlId = TemplateUtil.GetTemplateControlID(field.attributeName, TemplateControlIDPrefix);

                // Finds web control by control id
                WebControl control = FindControl(controlId) as WebControl;

                if (control != null && control is IAccelaControl)
                {
                    IAccelaControl ctl = control as IAccelaControl;
                    string value4Setting = ModelUIFormat.GetI18NTemplateValue(field);
                    ctl.SetValue(value4Setting);
                }
            }
        }

        /// <summary>
        /// Gets all of attribute objects.
        /// </summary>
        /// <returns>AttributeModel4WS array.</returns>
        public TemplateAttributeModel[] GetEMSEAttributeModels()
        {
            // if there is no fields to be created, return
            if (Fields == null || Fields.Length == 0)
            {
                return null;
            }

            TemplateAttributeModel[] result = Fields;

            foreach (TemplateAttributeModel field in Fields)
            {
                // contiue when the control type is invalid
                if (!ControlBuildHelper.IsValidControlType(field.attributeValueDataType))
                {
                    continue;
                }

                // Finds web control by control id
                WebControl control = FindControl(TemplateUtil.GetTemplateControlID(field.attributeName, TemplateControlIDPrefix)) as WebControl;

                if (control != null && control is IAccelaControl)
                {
                    IAccelaControl ctl = control as IAccelaControl;
                    string controlValue = ctl.GetValue();

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
                    }

                    field.attributeValue = controlValue;
                }
            }

            return result;
        }

        #endregion Public Methods

        #region Construction/Initial
        /// <summary>
        /// Initial event
        /// </summary>
        /// <param name="e">Event args.</param>
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            if (Accela.ACA.Web.Common.AppSession.IsAdmin)
            {
                Visible = false;
            }
        }

        /// <summary>
        /// After LoadViewState event, re-create the all of template control according to previous stored to ViewState.
        /// Avoid losing the dynamical control when page is post back.
        /// </summary>
        /// <param name="savedState">saved state.</param>
        protected override void LoadViewState(object savedState)
        {
            base.LoadViewState(savedState);

            // create dynamicall controls from view state
            CreateControls();
        }

        #endregion Construction/Initial

        #region Private methods

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

            _isCreated = true;

            pnlEMSETemplate.Controls.Clear();

            StringBuilder script4Supervisor = new StringBuilder();

            script4Supervisor.Append("function ");
            script4Supervisor.Append(ScriptName4Supervisor);
            script4Supervisor.Append("(){");

            //// if there is no fields to be created, return
            if (Fields == null || Fields.Length == 0)
            {
                script4Supervisor.Append("return true;}");
                return;
            }

            int fieldCount = Fields.Length;

            script4Supervisor.Append("var isNotEmpty=false;");
            script4Supervisor.Append("var control;");

            // According to the attribute data type to create relevant control.
            for (int i = 0; i < fieldCount; i++)
            {
                TemplateAttributeModel field = Fields[i];

                // Contiue when the control type is invalid
                if (!ControlBuildHelper.IsValidControlType(field.attributeValueDataType))
                {
                    continue;
                }

                // Converts TemplateAttributeModel model to IControlEntity.
                IControlEntity ctlEntity = ControlBuildHelper.BuildControlEntity(field, TemplateControlIDPrefix);

                //string controlType = field.attributeValueDataType.ToUpper();
                string controlType = field.attributeValueDataType.ToUpperInvariant();

                // Create web control to UI
                Literal lit = new Literal();
                lit.Text = "<div class=\"ACA_TabRow\"><table role='presentation' cellpadding=\"0\" cellspacing=\"0\"><tr><td>";
                pnlEMSETemplate.Controls.Add(lit);

                // 1.create text box control
                // If (controlType == ControlType.Text.ToString().ToUpper())
                if (controlType.Equals(ControlType.Text.ToString(), StringComparison.InvariantCultureIgnoreCase))
                {
                    AccelaTextBox textbox = ControlBuildHelper.CreateTextBox(ctlEntity);
                    textbox.WatermarkText = I18nStringUtil.GetCurrentLanguageString(field.resWaterMark, field.waterMark);
                    textbox.SubLabel = I18nStringUtil.GetCurrentLanguageString(field.resInstruction, field.instruction);

                    pnlEMSETemplate.Controls.Add(textbox);
                    textbox.CheckControlValueValidateFunction = BuidlScript(false, textbox.ClientID);
                }
                else if (controlType.Equals(ControlType.DropdownList.ToString(), StringComparison.InvariantCultureIgnoreCase))
                {
                    // 2.create dropdownlist control      
                    AccelaDropDownList ddl = ControlBuildHelper.CreateDropDownList(ctlEntity);
                    ddl.SubLabel = I18nStringUtil.GetCurrentLanguageString(field.resInstruction, field.instruction);
                    ddl.Width = DROPDOWNLIST_WIDTH;
                    pnlEMSETemplate.Controls.Add(ddl);
                    ddl.CheckControlValueValidateFunction = BuidlScript(false, ddl.ClientID);
                    ddl.Attributes.Add("IsCheckVisible", "true");
                }

                if (!string.IsNullOrEmpty(field.resAttributeUnitType) || !string.IsNullOrEmpty(field.attributeUnitType))
                {
                    // ToDo: Need to implement unit label to be appended to control behind.
                    // If defined the unit, need to append unit label.
                    lit = new Literal();
                    lit.Text = "</td><td class=\"ACA_Label ACA_Label_FontSize\"><div>";
                    pnlEMSETemplate.Controls.Add(lit);
                    
                    AccelaLabel lblUnit = ControlBuildHelper.CreateUnitLabel(I18nStringUtil.GetString(field.resAttributeUnitType, field.attributeUnitType));
                    pnlEMSETemplate.Controls.Add(lblUnit);

                    lit = new Literal();
                    lit.Text = "</div>";
                    pnlEMSETemplate.Controls.Add(lit);
                }

                lit = new Literal();
                lit.Text = "</td></tr></table>";
                pnlEMSETemplate.Controls.Add(lit);
            }

            script4Supervisor.Append("return true;}");

            _script4Supervisor = script4Supervisor.ToString();
        }

        /// <summary>
        /// Build client validate script
        /// </summary>
        /// <param name="isDateControl">If control is data control</param>
        /// <param name="clientID">Control client id</param>
        /// <returns>Script name</returns>
        private string BuidlScript(bool isDateControl, string clientID)
        {
            StringBuilder script4Supervisor = new StringBuilder();

            script4Supervisor.Append("control = $get('");
            script4Supervisor.Append(clientID);
            script4Supervisor.AppendFormat("');if(control)isNotEmpty = GetValue(control){0}.trim() != '';", isDateControl ? ".replace('/','').replace('/','')" : string.Empty);
            script4Supervisor.Append("if(isNotEmpty)return false;");

            _script4Supervisor = script4Supervisor.ToString();

            return string.Empty;
        }

        /// <summary>
        /// Filter template attributes didn't configuration EMSE DropDown list.
        /// </summary>
        /// <param name="attributeModels">Attribute models.</param>
        /// <returns>Template attribute model list</returns>
        private TemplateAttributeModel[] FilterEMSETemplateAttributes(TemplateAttributeModel[] attributeModels)
        {
            if (attributeModels == null || attributeModels.Length == 0)
            {
                return null;
            }

            ArrayList attributesModelList = new ArrayList();

            foreach (TemplateAttributeModel templateAttributeModel in attributeModels)
            {
                if (!string.IsNullOrEmpty(templateAttributeModel.attributeScriptCode))
                {
                    attributesModelList.Add(templateAttributeModel);
                }
            }

            return (TemplateAttributeModel[])attributesModelList.ToArray(typeof(TemplateAttributeModel));
        }

        #endregion Private methods
    }
}