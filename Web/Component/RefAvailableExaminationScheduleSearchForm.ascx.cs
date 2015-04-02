#region Header
/**
 * <pre>
 * 
 *  Accela Citizen Access
 *  File: RefAvailableExaminationScheduleSearchForm.ascx.cs
 * 
 *  Accela, Inc.
 *  Copyright (C): 2011-2014
 * 
 *  Description:
 * 
 *  Notes:
 * $Id: ExamScheduleViewModel.cs 181867 2011-09-27 08:06:18Z ACHIEVO\daly.zeng $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */
#endregion Header

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Accela.ACA.BLL.Common;
using Accela.ACA.Common;
using Accela.ACA.Common.Util;
using Accela.ACA.Web.Common;
using Accela.ACA.Web.Examination;
using Accela.ACA.WSProxy;

namespace Accela.ACA.Web.Component
{
    /// <summary>
    /// Available Examination Schedule Search Form
    /// </summary>
    public partial class RefAvailableExaminationScheduleSearchForm : BaseUserControl
    {
        #region Properties

        /// <summary>
        /// Gets or sets the grid view id.
        /// </summary>
        /// <value>The grid view id.</value>
        public string GridViewId
        {
            get
            {
                if (ViewState["GridViewId"] == null)
                {
                    return string.Empty;
                }

                return ViewState["GridViewId"].ToString();
            }

            set
            {
                ViewState["GridViewId"] = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is in popup.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is in popup; otherwise, <c>false</c>.
        /// </value>
        public bool IsInPopup
        {
            get
            {
                if (ViewState["IsInPopup"] == null)
                {
                    return false;
                }

                return bool.Parse(ViewState["IsInPopup"].ToString());
            }

            set
            {
                ViewState["IsInPopup"] = value;
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Gets the schedule search model.
        /// </summary>
        /// <param name="searchEntity">The search entity.</param>
        /// <returns>examination schedule search model</returns>
        public ExamScheduleSearchModel GetScheduleSearchModel(ExamScheduleSearchModel searchEntity)
        {
            MessageUtil.HideMessage(Page);
            ITimeZoneBll providerBll = ObjectFactory.GetObject<ITimeZoneBll>();
            DateTime agencyTime = providerBll.GetAgencyCurrentDate(ConfigManager.AgencyCode);

            searchEntity.city = string.IsNullOrEmpty(ddlCity.SelectedValue.Trim()) ? null : ddlCity.SelectedValue.Trim();
            searchEntity.state = string.IsNullOrEmpty(ddlState.SelectedValue.Trim())
                                     ? null
                                     : ddlState.SelectedValue.Trim();

            if (string.IsNullOrEmpty(ddlProvider.SelectedValue))
            {
                searchEntity.providerNbr = null;
            }
            else
            {
                searchEntity.providerNbr = long.Parse(ddlProvider.SelectedValue);
            }

            if (string.IsNullOrEmpty(dateFromTime.Text.Trim()))
            {
                searchEntity.startDate = null;
            }
            else
            {
                DateTime dtBegin = I18nDateTimeUtil.ParseFromUI(dateFromTime.Text.Trim());

                if (dtBegin == agencyTime.Date)
                {
                    dtBegin = agencyTime;
                }

                searchEntity.startDate = dtBegin;
            }

            if (string.IsNullOrEmpty(dateToTime.Text.Trim()))
            {
                searchEntity.endDate = null;
            }
            else
            {
                searchEntity.endDate = I18nDateTimeUtil.ParseFromUI(dateToTime.Text.Trim()).AddDays(1).AddSeconds(-1);
            }

            return searchEntity;
        }

        /// <summary>
        /// Binds the DDL.
        /// </summary>
        /// <param name="servProvCode">The server provider code.</param>
        /// <param name="examNbr">The exam NBR.</param>
        public void BindDDL(string servProvCode, string examNbr)
        {
            DropDownListBindUtil.BindProviderInfoByExam(servProvCode, examNbr, ddlProvider, ddlCity, ddlState);
        }

        /// <summary>
        /// Sets the DDL value.
        /// </summary>
        /// <param name="provider">The provider.</param>
        /// <returns>select provider value</returns>
        public string SetAndGetProviderSelectValue(string provider)
        {
            return DropDownListBindUtil.SetAndGetSelectValue(ddlProvider, provider);
        }

        /// <summary>
        /// Sets the control value for back page.
        /// </summary>
        /// <param name="parameter">The parameter.</param>
        /// <param name="searchModel">The search model.</param>
        public void SetControlValue4Back(ExaminationParameter parameter, ExamScheduleSearchModel searchModel)
        {
            if (!string.IsNullOrEmpty(parameter.ConditionCity))
            {
                DropDownListBindUtil.SetSelectedValue(ddlCity, parameter.ConditionCity);
                searchModel.city = parameter.ConditionCity;
            }

            if (!string.IsNullOrEmpty(parameter.ConditionProviderNbr))
            {
                DropDownListBindUtil.SetSelectedValue(ddlProvider, parameter.ConditionProviderNbr);
                searchModel.providerNbr = long.Parse(parameter.ConditionProviderNbr);
            }

            if (!string.IsNullOrEmpty(parameter.ConditionState))
            {
                DropDownListBindUtil.SetSelectedValue(ddlState, parameter.ConditionState);
                searchModel.state = parameter.ConditionState;
            }

            if (!string.IsNullOrEmpty(parameter.ConditionStartTime))
            {
                DateTime startDate = I18nDateTimeUtil.ParseFromUI(parameter.ConditionStartTime);
                dateFromTime.Text2 = startDate;
                searchModel.startDate = startDate;
            }

            if (!string.IsNullOrEmpty(parameter.ConditionEndTime))
            {
                DateTime endDate = I18nDateTimeUtil.ParseFromUI(parameter.ConditionEndTime);
                dateToTime.Text2 = endDate;
                searchModel.endDate = endDate;
            }
        }

        /// <summary>
        /// Handles the Load event of the Page control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                SetDefaultValue();
            }
        }

        /// <summary>
        /// Raises the <see cref="E:System.Web.UI.Control.PreRender"/> event.
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs"/> object that contains the event data.</param>
        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);

            if (GridViewId == GviewID.ExaminationSchedule)
            {
                ddlProvider.LabelKey = "aca_exam_schedule_availablelist_provider";
                ddlCity.LabelKey = "aca_exam_schedule_availablelist_city";
                ddlState.LabelKey = "aca_exam_schedule_availablelist_state";
                dateFromTime.LabelKey = "aca_exam_schedule_availablelist_from";
                dateToTime.LabelKey = "aca_exam_schedule_availablelist_to";
            }
            else if (GridViewId == GviewID.CheckexaminationSchedule)
            {
                ddlProvider.LabelKey = "aca_exam_detail_provider";
                ddlCity.LabelKey = "aca_exam_detail_city";
                ddlState.LabelKey = "aca_exam_detail_state";
                dateFromTime.LabelKey = "aca_exam_detail_from";
                dateToTime.LabelKey = "aca_exam_detail_to";
            }
        }

        /// <summary>
        /// Sets the default value.
        /// </summary>
        private void SetDefaultValue()
        {
            ITimeZoneBll providerBll = ObjectFactory.GetObject<ITimeZoneBll>();
            DateTime agencyTime = providerBll.GetAgencyCurrentDate(ConfigManager.AgencyCode);

            if (!(dateFromTime.Text2 is DateTime))
            {
                dateFromTime.Text2 = agencyTime.Date;
            }

            if (!(dateToTime.Text2 is DateTime))
            {
                dateToTime.Text2 = agencyTime.Date.AddYears(1);
            }
        }

        /// <summary>
        /// Shows the message.
        /// </summary>
        /// <param name="messageType">Type of the message.</param>
        /// <param name="msg">The MSG.</param>
        private void ShowMessage(MessageType messageType, string msg)
        {
            if (IsInPopup)
            {
                MessageUtil.ShowMessageInPopup(Page, messageType, msg);
            }
            else
            {
                MessageUtil.ShowMessage(Page, messageType, msg);
            }
        }

        #endregion
    }
}