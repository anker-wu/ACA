#region Header
/**
 * <pre>
 * 
 *  Accela Citizen Access
 *  File: ExaminationCancellation.aspx.cs
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
using System.Linq;
using System.Web.UI;
using Accela.ACA.BLL.LicenseCertification;
using Accela.ACA.Common;
using Accela.ACA.Common.Util;
using Accela.ACA.Web.Common;
using Accela.ACA.WSProxy;

namespace Accela.ACA.Web.Examination
{
    /// <summary>
    /// Examination Cancellation 
    /// </summary>
    public partial class ExaminationCancellation : ExaminationScheduleBasePage
    {
        /// <summary>
        /// Handles the Load event of the Page control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            this.SetPageTitleKey("aca_exam_schedule_cancel_confirm_title");
            this.SetDialogMaxHeight("600");

            if (!AppSession.IsAdmin)
            {
                if (bool.Parse(ExaminationWizardParameter.IsAllowedBeyondDate))
                {
                    string message = string.Format(
                                                GetTextByKey("aca_exam_schedule_message_cancel_beyond"),
                                                ExaminationWizardParameter.ExamReScheduleProviderName,
                                                ExaminationWizardParameter.ExaminationName,
                                                ExaminationWizardParameter.ExamScheduleDate);

                    MessageUtil.ShowMessageInPopup(Page, MessageType.Notice, message);
                }

                if (!IsPostBack)
                {
                    ExaminationReasonList.BindReason(ExaminationWizardParameter.ReasonID, 0);
                    SetWizardButtonDisable(lnkContinue.ClientID, true);

                    //set no reason notice
                    lblNoInspectionTypesFound.Visible = ExaminationReasonList.DataSource.Rows.Count == 0;
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
            string selectReason = ExaminationReasonList.GetSelectedReason();

            if (!string.IsNullOrEmpty(selectReason))
            {
                long dailySeqNumber = long.Parse(ExaminationWizardParameter.DailyExaminationNbr);
                var recordModel = AppSession.GetCapModelFromSession(this.ModuleName);
                var examModel = recordModel.examinationList.FirstOrDefault(o => o.examinationPKModel.examNbr == dailySeqNumber);
                var paramVo = new ScheduleExamParamVO()
                                  {
                                      examSeqNbr = dailySeqNumber,
                                      examName = ExaminationWizardParameter.ExaminationName,
                                      reschedCancelReason = selectReason,
                                      serviceProviderCode = ExaminationWizardParameter.RecordAgencyCode,
                                      callerID = AppSession.User.PublicUserId,
                                      isBeyondAllowanceDate = string.IsNullOrEmpty(ExaminationWizardParameter.IsAllowedBeyondDate)
                                        ? false : bool.Parse(ExaminationWizardParameter.IsAllowedBeyondDate),
                                      fromACA = true
                                  };

                paramVo.capID = new CapIDModel()
                {
                    ID1 = ExaminationWizardParameter.RecordID1,
                    ID2 = ExaminationWizardParameter.RecordID2,
                    ID3 = ExaminationWizardParameter.RecordID3,
                    serviceProviderCode = ExaminationWizardParameter.RecordAgencyCode
                };

                var providerModel = new ProviderModel4WS()
                {
                    serviceProviderCode = ExaminationWizardParameter.RecordAgencyCode,
                    providerName = ExaminationWizardParameter.ExamReScheduleProviderName,
                    providerNo = ExaminationWizardParameter.ExamReScheduleProviderNo
                };

                IProviderBll prividerBll = (IProviderBll)ObjectFactory.GetObject(typeof(IProviderBll));
                ProviderModel4WS[] providerArray = prividerBll.GetProviderList(providerModel, null);

                //is external provider
                bool isExternal = false;

                if (providerArray != null && providerArray.Length > 0)
                {
                    paramVo.providerNbr = providerArray[0].providerNbr;
                    isExternal = !string.IsNullOrEmpty(providerArray[0].externalExamURL);
                }

                if (string.IsNullOrEmpty(ExaminationWizardParameter.ExamScheduleDate))
                {
                    paramVo.examDate = null;
                }
                else
                {
                    paramVo.examDate = I18nDateTimeUtil.ParseFromUI(ExaminationWizardParameter.ExamScheduleDate);
                }

                if (string.IsNullOrEmpty(ExaminationWizardParameter.ExamScheduleStartTime))
                {
                    paramVo.startTime = null;
                }
                else
                {
                    paramVo.startTime = I18nDateTimeUtil.ParseFromUI(ExaminationWizardParameter.ExamScheduleDate + " " + ExaminationWizardParameter.ExamScheduleStartTime);
                }

                if (string.IsNullOrEmpty(ExaminationWizardParameter.ExamScheduleEndTime))
                {
                    paramVo.endTime = null;
                }
                else
                {
                    paramVo.endTime = I18nDateTimeUtil.ParseFromUI(ExaminationWizardParameter.ExamScheduleEndTime + " " + ExaminationWizardParameter.ExamScheduleDate);
                }

                /*
                 * For UnSchedule process, if the exam is external, we need pass the template to biz interface, 
                 *  since biz interface will not to search template data for external exam.
                 */
                if (isExternal)
                {
                    paramVo.template = examModel.template;
                }

                IExaminationBll examBll = (IExaminationBll)ObjectFactory.GetObject(typeof(IExaminationBll));

                try
                {
                    if (examBll.UnScheduleExam(paramVo))
                    {
                        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "cancelkey", "CancelSuccessful()", true);
                    }
                }
                catch (Exception ex)
                {
                    //it is need update parent ui, must be show parent's loading.
                    //there is hidden parent loading.
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "cancelkey", "parent.hideDialogLoading();", true);
                    MessageUtil.ShowMessageInPopup(Page, MessageType.Error, ex.Message);
                }
            }
        }

        /// <summary>
        /// Handles the PageIndexChanging event of the ExaminationReasonList1 control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Web.UI.WebControls.GridViewPageEventArgs"/> instance containing the event data.</param>
        protected void ExaminationReasonList_PageIndexChanging(object sender, System.Web.UI.WebControls.GridViewPageEventArgs e)
        {
            SetContinueButtonStatus();
        }

        /// <summary>
        /// Sets the continue button status.
        /// </summary>
        private void SetContinueButtonStatus()
        {
            bool wizardButtonDisabled = string.IsNullOrEmpty(ExaminationReasonList.HfSelectedReasonValue);
            SetWizardButtonDisable(lnkContinue.ClientID, wizardButtonDisabled);
        }
    }
}