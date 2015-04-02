#region Header
/**
 * <pre>
 * 
 *  Accela Citizen Access
 *  File: ExaminationRescheduleReason.aspx.cs
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
using System.Web.UI.WebControls;
using Accela.ACA.Common;
using Accela.ACA.Web.Common;

namespace Accela.ACA.Web.Examination
{
    /// <summary>
    /// Examination Reschedule Reason
    /// </summary>
    public partial class ExaminationRescheduleReason : ExaminationScheduleBasePage
    {
        /// <summary>
        /// Handles the Load event of the Page control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            SetPageTitleKey("aca_exam_schedule_reschedule_reason_title");
            SetDialogMaxHeight("600");

            if (!AppSession.IsAdmin)
            {
                if (!string.IsNullOrEmpty(ExaminationWizardParameter.IsAllowedBeyondDate)
                    && bool.Parse(ExaminationWizardParameter.IsAllowedBeyondDate))
                {
                    string message = string.Format(
                                                GetTextByKey("aca_exam_schedule_message_reschedule_beyond"),
                                                ExaminationWizardParameter.ExamReScheduleProviderName,
                                                ExaminationWizardParameter.ExaminationName,
                                                ExaminationWizardParameter.ExamScheduleDate);

                    MessageUtil.ShowMessageInPopup(Page, MessageType.Notice, message);
                }

                if (!IsPostBack)
                {
                    ExaminationReasonList.BindReason(ExaminationWizardParameter.ReasonID, 0);

                    ////set no reason notice//
                    lblNoInspectionTypesFound.Visible = ExaminationReasonList.DataSource.Rows.Count == 0;
                    SetContinueButtonStatus();
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
            string selectReason = ExaminationReasonList.HfSelectedReasonValue;

            if (!string.IsNullOrEmpty(selectReason))
            {
                ExaminationScheduleUtil.ClearSearchParameter(ExaminationWizardParameter);
                ExaminationWizardParameter.ReasonID = selectReason;

                string url = "AvailableExaminationScheduleList.aspx";
                url = string.Format("{0}?{1}", url, Request.QueryString);
                url = ExaminationParameterUtil.UpdateURLAndSaveParameters(url, ExaminationWizardParameter);

                Response.Redirect(url);
            }
        }

        /// <summary>
        /// Handles the PageIndexChanging event of the ExaminationReasonList1 control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Web.UI.WebControls.GridViewPageEventArgs"/> instance containing the event data.</param>
        protected void ExaminationReasonList_PageIndexChanging(object sender, GridViewPageEventArgs e)
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