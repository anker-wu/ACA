#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: ReportParameter.aspx.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2009-2014
 *
 *  Description:
 *
 *  Notes:
 *      $Id: CapDetail.aspx.cs 127094 2009-04-15 11:07:16Z ACHIEVO\vera.zhao $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;
using Accela.ACA.BLL.Common;
using Accela.ACA.BLL.Report;
using Accela.ACA.Common;
using Accela.ACA.Common.Log;
using Accela.ACA.Common.Util;
using Accela.ACA.Web.Common;
using Accela.ACA.Web.Common.Control;
using Accela.ACA.WSProxy;
using Accela.Web.Controls;
using log4net;

namespace Accela.ACA.Web.Report
{
    /// <summary>
    /// class of report parameter
    /// </summary>
    public partial class ReportParameter : BasePageWithoutMaster
    {
        #region Fields

        /// <summary>
        /// Logger object.
        /// </summary>
        private static readonly ILog Logger = LogFactory.Instance.GetLogger(typeof(ReportParameter));

        #endregion Fields

        #region Properties

        /// <summary>
        /// Gets or sets a value indicating whether show process loading for Show Report.
        /// </summary>
        protected bool ShowProcessLoading4ShowReport { get; set; }

        #endregion Properties

        #region Methods

        /// <summary>
        /// page load event
        /// </summary>
        /// <param name="sender">object event sender</param>
        /// <param name="e">event args</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            string reportID = Request["reportID"];
            string reportType = Request["reportType"];
            CapIDModel4WS capIdModel = GetCapID();
            string moduleName = GetModuleName();

            if (!string.IsNullOrEmpty(reportID))
            {
                try
                {
                    if (!ReportUtil.CheckPermissionOnReport(reportType, reportID, capIdModel, moduleName))
                    {
                        // Without permission, it needs to redirect to welcome page.
                        Response.Redirect(ACAConstant.URL_DEFAULT);
                    }

                    ParameterModel4WS[] parameters = ReportUtil.GetReportParameters(reportID, reportType, ModuleName);
                    AppSession.SetReportParameterToSession(parameters);

                    if (parameters == null || parameters.Length == 0)
                    {
                        RedirectToReportPage();
                    }

                    //check if need show parameters
                    IReportBll reportBll = ObjectFactory.GetObject(typeof(IReportBll)) as IReportBll;

                    if (reportBll.HasShowParameter(parameters))
                    {
                        BuildParameterControls(parameters);
                    }
                    else
                    {
                        RedirectToReportPage();
                    }
                }
                catch (ACAException ex)
                {
                    Logger.Error(ex);
                    ReportUtil.ShowError(Page, ModuleName, "aca_common_report_configerror_label");
                }
            }
            else
            {
                // if report id is null or empty, close the window
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "closeWindow", "window.close();", true);
            }
        }

        /// <summary>
        /// Click save button event
        /// </summary>
        /// <param name="sender">event sender for Save button</param>
        /// <param name="e">event args</param>
        protected void SaveButton_Click(object sender, EventArgs e)
        {
            RedirectToReportPage();
        }

        /// <summary>
        /// Get Sub CAP ID Model
        /// </summary>
        /// <returns>CapID Model</returns>
        private CapIDModel4WS GetCapID()
        {
            CapIDModel4WS capId = ReportUtil.GetSubCapID();

            // Excluded when passing CAP ID List
            string capIDs = Server.UrlDecode(Request.QueryString[ACAConstant.ID]);
            if (capId == null && string.IsNullOrEmpty(capIDs))
            {
                CapModel4WS capModel = AppSession.GetCapModelFromSession(ModuleName);

                if (capModel != null && capModel.capID != null)
                {
                    capId = capModel.capID;
                }
            }

            return capId;
        }

        /// <summary>
        /// Get Module Name for Report Parameter
        /// </summary>
        /// <returns>Module Name</returns>
        private string GetModuleName()
        {
            string tmpModuleName = Request.QueryString["SubModule"];

            if (tmpModuleName == null)
            {
                if (!string.IsNullOrEmpty(ModuleName))
                {
                    tmpModuleName = ModuleName;
                }
            }

            return tmpModuleName;
        }

        /// <summary>
        /// Converts ParameterModel4WS to IControlEntity interface.
        /// </summary>
        /// <param name="model">ParameterModel4WS model</param>
        /// <returns>An instance of IControlEntity.</returns>
        private IControlEntity BuildControlEntity(ParameterModel4WS model)
        {
            ControlEntity ctlEntity = new ControlEntity();
            ctlEntity.ControlID = model.parameterType + "_" + model.prtParameterId.ToString();

            // set the ControlEntity's properties as the parameter's
            ctlEntity.Label = model.dispParameterName;

            if (ParameterType.Dropdown.ToString().Equals(model.parameterType, StringComparison.OrdinalIgnoreCase))
            {
                ctlEntity.DefaultValue = string.Empty;
                BuildItems4Dropdown(ctlEntity, model.dispDefaultValue);
            }
            else
            {
                ctlEntity.DefaultValue = model.dispDefaultValue;
            }

            ctlEntity.Required = ACAConstant.COMMON_Y.Equals(model.parameterRequired, StringComparison.InvariantCulture);

            // if parameter type is variable, get the variables.
            if (model.parameterType.Equals(ParameterType.Variable.ToString(), StringComparison.InvariantCultureIgnoreCase))
            {
                //get variables from xpolicy cache.
                IXPolicyBll xpolicyBll = (IXPolicyBll)ObjectFactory.GetObject(typeof(IXPolicyBll));
                List<XPolicyModel> variables = xpolicyBll.GetPolicyListByPolicyName(BizDomainConstant.STD_CAT_VARIABLE_CONFIG, ConfigManager.AgencyCode);

                if (variables != null)
                {
                    //add variable items
                    foreach (XPolicyModel policy in variables)
                    {
                        ItemValue item = new ItemValue();
                        item.Value = policy.levelData;
                        item.Key = policy.policySeq.ToString();

                        ctlEntity.Items.Add(item);
                    }
                }
            }

            return ctlEntity;
        }

        /// <summary>
        /// Builds the items4 dropdown.
        /// </summary>
        /// <param name="ctlEntity">The CTL entity.</param>
        /// <param name="valueString">The value string.</param>
        private void BuildItems4Dropdown(ControlEntity ctlEntity, string valueString)
        {
            string[] availableValues = null;

            if (!string.IsNullOrEmpty(valueString))
            {
                availableValues = valueString.Split(new[] { ACAConstant.COMMA }, StringSplitOptions.None);
            }

            if (availableValues != null)
            {
                foreach (var value in availableValues)
                {
                    ItemValue item = new ItemValue();
                    item.Value = value;
                    item.Key = value;

                    ctlEntity.Items.Add(item);
                }
            }
        }

        /// <summary>
        /// Create controls based on the report parameters
        /// </summary>
        /// <param name="parameters">ParameterModel4WS array</param>
        private void BuildParameterControls(ParameterModel4WS[] parameters)
        {
            if (parameters == null || parameters.Length == 0)
            {
                return;
            }

            // create table
            Table table = new Table();
            table.Attributes.Add("role", "presentation");

            // create controls for parameters
            foreach (ParameterModel4WS parameter in parameters)
            {
                StringComparison sc = StringComparison.InvariantCultureIgnoreCase;

                if (ACAConstant.COMMON_N.Equals(parameter.parameterVisible, sc))
                {
                    continue;
                }

                // build control entity for each parameter
                WebControl control;
                IControlEntity ctlEntity = BuildControlEntity(parameter);

                string paramterType = parameter.parameterType;

                // if parameter type is cap type or variable, create drop-down control
                if (ParameterType.CapType.ToString().Equals(paramterType, sc))
                {
                    string vchType = ACAConstant.VCH_TYPE_VHAPP; //VHAPP

                    if (ACAConstant.COMMON_Y.Equals(Request.QueryString["isFeeEstimator"], sc))
                    {
                        vchType = ACAConstant.VCH_TYPE_EST; //EST
                    }

                    //control = ControlBuildHelper.CreateDropDownList(ctlEntity);
                    control = ControlBuildHelper.CreateCapTypeDropDownList(ctlEntity, ModuleName, vchType);
                }
                else if (ParameterType.Variable.ToString().Equals(paramterType, sc))
                {
                    control = ControlBuildHelper.CreateDropDownList(ctlEntity);
                }
                else if (ParameterType.Date.ToString().Equals(paramterType, sc))
                {
                    //convert the date string as the correct format.
                    ctlEntity.DefaultValue = I18nDateTimeUtil.ConvertDateStringFromWebServiceToUI(ctlEntity.DefaultValue);
                    control = ControlBuildHelper.CreateCalendar(ctlEntity);
                }
                else if (ParameterType.Number.ToString().Equals(paramterType, sc))
                {
                    control = ControlBuildHelper.CreateNumberText(ctlEntity);
                }
                else if (ParameterType.SessionVariable.ToString().Equals(paramterType, sc))
                {
                    control = ControlBuildHelper.CreateTextBox(ctlEntity);
                }
                else if (ParameterType.Dropdown.ToString().Equals(paramterType, sc))
                {
                    if (ValidationUtil.IsYes(parameter.allowMultipleValue))
                    {
                        control = ControlBuildHelper.CreateListBox(ctlEntity, true);
                    }
                    else
                    {
                        control = ControlBuildHelper.CreateDropDownList(ctlEntity);
                    }
                }
                else
                {
                    control = ControlBuildHelper.CreateTextBox(ctlEntity);
                }

                // create tr to load the parameter control
                TableRow tr = new TableRow();
                TableCell td = new TableCell();

                td.Controls.Add(control);
                tr.Cells.Add(td);
                table.Rows.Add(tr);
            }

            phParameterControls.Controls.Add(table);
        }

        /// <summary>
        /// Get parameter value from UI
        /// </summary>
        /// <param name="parameterType">parameter type</param>
        /// <param name="parameterId">parameter id</param>
        /// <returns>parameter value</returns>
        private string GetParameterValue(string parameterType, long parameterId)
        {
            string value = string.Empty;
            string controlId = parameterType + "_" + parameterId.ToString();

            // Find web control by control id
            WebControl control = FindControl(controlId) as WebControl;

            if (control != null && control is IAccelaControl)
            {
                IAccelaControl ctl = control as IAccelaControl;
                value = ctl.GetValue();

                if (ParameterType.Dropdown.ToString().Equals(parameterType, StringComparison.OrdinalIgnoreCase))
                {
                    value = string.IsNullOrEmpty(value) ? string.Empty : value.Replace(ACAConstant.SPLIT_CHAR.ToString(), ACAConstant.COMMA);
                }

                bool isDateType = parameterType.Equals(ParameterType.Date.ToString(), StringComparison.InvariantCultureIgnoreCase);

                // if Date type, get the right formate string.
                if ((!string.IsNullOrEmpty(value)) && isDateType)
                {
                    value = I18nDateTimeUtil.ConvertDateStringFromUIToWebService(value);
                }
            }

            return value;
        }

        /// <summary>
        /// if no parameter needs input or click save button, redirect to show report page.
        /// </summary>
        private void RedirectToReportPage()
        {
            ParameterModel4WS[] parameters = AppSession.GetReportParameterFromSession();

            // if parameter array is not null, get the values and pass them to show report page.
            if (parameters != null)
            {
                foreach (ParameterModel4WS parameter in parameters)
                {
                    if (parameter == null || ACAConstant.COMMON_N.Equals(parameter.parameterVisible, StringComparison.InvariantCultureIgnoreCase))
                    {
                        continue;
                    }

                    parameter.dispDefaultValue = GetParameterValue(parameter.parameterType, parameter.prtParameterId);
                }
            }

            AppSession.SetReportParameterToSession(parameters);

            this.divReportParamter.Visible = false;
            ShowProcessLoading4ShowReport = true;
        }

        #endregion Methods
    }
}