#region Header
/**
 * <pre>
 * 
 *  Accela Citizen Access
 *  File: AvailableExaminationScheduleList.aspx.cs
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
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using Accela.ACA.BLL.Common;
using Accela.ACA.BLL.LicenseCertification;
using Accela.ACA.Common;
using Accela.ACA.Common.Util;
using Accela.ACA.Web.Common;
using Accela.ACA.WSProxy;

namespace Accela.ACA.Web.Examination
{
    /// <summary>
    /// Available Examination Schedule List
    /// </summary>
    public partial class AvailableExaminationScheduleList : ExaminationScheduleBasePage
    {
        #region Properties

        /// <summary>
        /// Gets or sets the re-schedule provider NBR.
        /// </summary>
        /// <value>The re schedule provider NBR.</value>
        public string ReScheduleProviderNbr
        {
            get
            {
                if (ViewState["ReScheduleProviderNbr"] == null)
                {
                    return string.Empty;
                }

                return ViewState["ReScheduleProviderNbr"].ToString();
            }

            set
            {
                ViewState["ReScheduleProviderNbr"] = value;
            }
        }

        /// <summary>
        /// Gets or sets the search entity.
        /// </summary>
        /// <value>The search entity.</value>
        private ExamScheduleSearchModel SearchEntity
        {
            get { return ViewState["AvailableExaminationScheduleList"] as ExamScheduleSearchModel; }

            set { ViewState["AvailableExaminationScheduleList"] = value; }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Validates the schedule date.
        /// </summary>
        /// <param name="startDate">The start date.</param>
        /// <param name="endDate">The end date.</param>
        /// <param name="module">The module.</param>
        /// <returns>The error message when validate schedule date failed.</returns>
        [WebMethod(Description = "ValidateScheduleDate", EnableSession = true)]
        public static string ValidateScheduleDate(string startDate, string endDate, string module)
        {
            return ExaminationScheduleUtil.ValidateScheduleDate(startDate, endDate, module);
        }

        /// <summary>
        /// Handles the Load event of the Page control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            this.SetPageTitleKey("aca_exam_schedule_availablelist_title");
            this.SetDialogMaxHeight("600");

            if (!IsPostBack)
            {
                if (!AppSession.IsAdmin)
                {
                    ITimeZoneBll providerBll = (ITimeZoneBll)ObjectFactory.GetObject(typeof(ITimeZoneBll));
                    DateTime agencyTime = providerBll.GetAgencyCurrentDate(ExaminationWizardParameter.RecordAgencyCode);

                    RefAvailableExaminationScheduleSearchForm.BindDDL(ExaminationWizardParameter.RecordAgencyCode, ExaminationWizardParameter.ExaminationNbr);
                    
                    SearchEntity = new ExamScheduleSearchModel()
                                       {
                                           examName = ExaminationWizardParameter.ExaminationName,
                                           entityType = DocumentEntityType.Examination,
                                           servProvCode = ExaminationWizardParameter.RecordAgencyCode,
                                           fromACA = true,
                                           startDate = agencyTime,
                                           endDate = agencyTime.Date.AddYears(1).AddDays(1).AddSeconds(-1)
                                       };

                    if (string.IsNullOrEmpty(ExaminationWizardParameter.ExaminationNbr))
                    {
                        SearchEntity.entityID = null;
                    }
                    else
                    {
                        SearchEntity.entityID = long.Parse(ExaminationWizardParameter.ExaminationNbr);
                    }

                    SetDefaultProvider();
                    RefAvailableExaminationScheduleSearchForm.SetControlValue4Back(ExaminationWizardParameter, SearchEntity);

                    SearchExamSchedule(SearchEntity);
                    lblScheduleExamination.Text = string.Format(GetTextByKey("aca_exam_schedule_availablelist_options"), ExaminationWizardParameter.ExaminationName);

                    SetWizardButtonDisable(lnkContinue.ClientID, true);
                }
                else
                {
                    RefAvailableExaminationScheduleList.DataSource = new DataTable();
                    RefAvailableExaminationScheduleList.BindAvailableExaminationScheduleList(true, false, true);
                    RefAvailableExaminationScheduleSearchForm.BindDDL(ExaminationWizardParameter.RecordAgencyCode, string.Empty);
                }
            }
        }

        /// <summary>
        /// Handles the Click event of the Continue Button control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected void ContinueButton_Click(object sender, EventArgs e)
        {
            RefAvailableExaminationScheduleList.GetSelectedExamination(ExaminationWizardParameter);

            if (bool.Parse(ExaminationWizardParameter.IsExternal) && !bool.Parse(ExaminationWizardParameter.HasPrimaryContact))
            {
                MessageUtil.ShowMessageInPopup(Page, MessageType.Error, GetTextByKey("aca_exam_schedule_message_noprimarycontact"));
            }
            else
            {
                GetSearchValueToParameter(ExaminationWizardParameter, SearchEntity);
                string url = "ExaminationScheduleConfirm.aspx";
                url = string.Format("{0}?{1}", url, Request.QueryString.ToString());
                url = ExaminationParameterUtil.UpdateURLAndSaveParameters(url, ExaminationWizardParameter);

                Response.Redirect(url);
            }
        }

        /// <summary>
        /// Handles the Click event of the Back Button control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected void BackButton_Click(object sender, EventArgs e)
        {
            string previousURL = ExaminationScheduleUtil.GetWizardPreviousURL(ExaminationWizardParameter);

            if (!string.IsNullOrEmpty(previousURL))
            {
                Response.Redirect(previousURL);
            }
        }

        /// <summary>
        /// Handles the Click event of the Search Button control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected void SearchButton_Click(object sender, EventArgs e)
        {
            Page.FocusElement(btnSearch.ClientID);
            SearchEntity.city = null;
            SearchEntity.state = null;
            SearchEntity.providerNbr = null;
            SearchEntity.startDate = null;
            SearchEntity.endDate = null;
            var model = RefAvailableExaminationScheduleSearchForm.GetScheduleSearchModel(SearchEntity);

            if (model != null)
            {
                SearchExamSchedule(SearchEntity);
            }

            RefAvailableExaminationScheduleList.ClearRadioButtonStatus();
            SetWizardButtonDisable(lnkContinue.ClientID, true);
        }

        /// <summary>
        /// Searches the exam schedule.
        /// </summary>
        /// <param name="examScheduleSearchModel">The exam schedule search model.</param>
        private void SearchExamSchedule(ExamScheduleSearchModel examScheduleSearchModel)
        {
            IExaminationBll examBll = ObjectFactory.GetObject<IExaminationBll>();
            RefAvailableExaminationScheduleList.DataSource = ExaminationScheduleUtil.ConvertExaminationSchduleModelToDataTable(
                                                                examBll.GetAvailableSchedule(examScheduleSearchModel));            
            RefAvailableExaminationScheduleList.BindAvailableExaminationScheduleList(true, true, true);
        }

        /// <summary>
        /// Sets the default provider.
        /// </summary>
        private void SetDefaultProvider()
        {
            if (!string.IsNullOrEmpty(ExaminationWizardParameter.ExamReScheduleProviderNo) 
                && !string.IsNullOrEmpty(ExaminationWizardParameter.ExamReScheduleProviderNo))
            {
                string examProviderNbr = RefAvailableExaminationScheduleSearchForm.SetAndGetProviderSelectValue(
                    ExaminationWizardParameter.ExamReScheduleProviderName + ACAConstant.SPLITLINE + ExaminationWizardParameter.ExamReScheduleProviderNo);

                if (!string.IsNullOrEmpty(examProviderNbr))
                {
                    ReScheduleProviderNbr = examProviderNbr;
                    SearchEntity.providerNbr = long.Parse(examProviderNbr);
                }
            }
        }

        /// <summary>
        /// Gets the search value to parameter.
        /// </summary>
        /// <param name="parameter">The parameter.</param>
        /// <param name="searchModel">The search model.</param>
        private void GetSearchValueToParameter(ExaminationParameter parameter, ExamScheduleSearchModel searchModel)
        {
            parameter.ConditionCity = searchModel.city;
            parameter.ConditionState = searchModel.state;
            parameter.ConditionProviderNbr = searchModel.providerNbr.ToString();

            if (searchModel.startDate == null)
            {
                parameter.ConditionStartTime = string.Empty;
            }
            else
            {
                parameter.ConditionStartTime = I18nDateTimeUtil.FormatToDateTimeStringForUI(searchModel.startDate.Value);
            }

            if (searchModel.endDate == null)
            {
                parameter.ConditionEndTime = string.Empty;
            }
            else
            {
                parameter.ConditionEndTime = I18nDateTimeUtil.FormatToDateTimeStringForUI(searchModel.endDate.Value);
            }
        }

        #endregion
    }
}