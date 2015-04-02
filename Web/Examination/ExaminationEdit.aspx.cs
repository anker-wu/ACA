#region Header
/**
 * <pre>
 * 
 *  Accela Citizen Access
 *  File: ExaminationEdit.cs
 * 
 *  Accela, Inc.
 *  Copyright (C): 2011-2014
 * 
 *  Description:
 * 
 *  Notes:
 *
 * </pre>
 */
#endregion Header

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Services;
using System.Web.UI;

using Accela.ACA.BLL.LicenseCertification;
using Accela.ACA.Common;
using Accela.ACA.Common.Util;
using Accela.ACA.Web.Common;
using Accela.ACA.WSProxy;

namespace Accela.ACA.Web.Examination
{
    /// <summary>
    /// Examination edit page.
    /// </summary>
    public partial class ExaminationEdit : PopupDialogBasePage
    {       
        /// <summary>
        /// Do schedule / reschedule / request operation.
        /// </summary>
        /// <param name="moduleName">The module name.</param>
        /// <param name="examNumber">The examination number.</param>
        /// <returns>Return the error message, if not empty, show that there is error.</returns>
        [WebMethod(Description = "CAPSubmit", EnableSession = true)]
        public static string CAPSubmit(string moduleName, long? examNumber)
        {
            string message = string.Empty;
            var examBll = ObjectFactory.GetObject<IExaminationBll>();

            //prepare CAP model and array list.
            CapModel4WS capModel = AppSession.GetCapModelFromSession(moduleName);
            if (capModel == null)
            {
                return string.Empty;
            }

            try
            {
                ExaminationModel exam = capModel.examinationList.First(p => p.examinationPKModel.examNbr == examNumber);
                if (exam != null && exam.examinationPKModel != null && exam.examinationPKModel.examNbr != null)
                {
                    string primaryContactSeqNbr = string.Empty;
                    CapContactModel4WS primaryCapContact = ExaminationScheduleUtil.GetCapPrimaryContact(capModel);

                    if (primaryCapContact != null
                        && ExaminationUtil.IsPassedExamination(exam.gradingStyle, exam.finalScore, exam.passingScore)
                        && ACAConstant.EXAMINATION_STATUS_COMPLETED_PENDING.Equals(exam.examStatus, StringComparison.InvariantCultureIgnoreCase))
                    {
                        primaryContactSeqNbr = primaryCapContact.refContactNumber;
                    }

                    examBll.UpdateExam(exam, primaryContactSeqNbr);
                }
            }
            catch (ACAException ex)
            {
                return ex.Message;
            }

            return message;
        }

        /// <summary>
        /// Page load initial
        /// </summary>
        /// <param name="sender">the sender object.</param>
        /// <param name="e">the event arguments.</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            this.SetPageTitleKey("aca_cap_detail_exam_edit_label_title");
            this.SetDialogMaxHeight("600");
            ExaminationDetailEdit.DataChanged += new CommonEventHandler(ExaminationChanged);
            if (!IsPostBack)
            {
                if (!string.IsNullOrEmpty(Request.QueryString["ExaminationNum"]))
                {
                    CapModel4WS capModel = AppSession.GetCapModelFromSession(ModuleName);
                    ExaminationModel exam = GetExamFromListByExamNum(Request.QueryString["ExaminationNum"]);
                    ExaminationDetailEdit.DataSource = exam;

                    /*
                     * If Exam is from reference(entityID has value) or is in Pending Completed status, the form will be read-only for public user.
                     */
                    ExaminationDetailEdit.IsEditable =
                        exam != null
                        && !exam.entityID.HasValue
                        && !ACAConstant.EXAMINATION_STATUS_COMPLETED_PENDING.Equals(exam.examStatus, StringComparison.InvariantCultureIgnoreCase)
                        && (capModel == null || !capModel.IsForRenew);
                }
            }
        }

        /// <summary>
        /// Get examination from CAP examination list
        /// </summary>
        /// <param name="examNum">Examination number</param>
        /// <returns>Examination model</returns>
        private ExaminationModel GetExamFromListByExamNum(string examNum)
        {
            ExaminationModel exam = new ExaminationModel();
            CapModel4WS capModel = AppSession.GetCapModelFromSession(ModuleName);
            if (capModel != null && capModel.examinationList != null && capModel.examinationList.Length > 0)
            {
                foreach (ExaminationModel examination in capModel.examinationList)
                {
                    if (examination.examinationPKModel.examNbr != null && examNum.Equals(examination.examinationPKModel.examNbr.ToString()))
                    {
                        exam = examination;
                        break;
                    }
                }
            }

            return exam;
        }

        /// <summary>
        /// triggered after education saved
        /// </summary>
        /// <param name="sender">An object that contains the event sender.</param>
        /// <param name="arg">A CommonEventArgs object containing the education models.</param>
        private void ExaminationChanged(object sender, CommonEventArgs arg)
        {
            if (arg != null && arg.ArgObject != null)
            {
                CapModel4WS capModel = AppSession.GetCapModelFromSession(ModuleName);

                if (capModel != null && IsValidExam(ExaminationDetailEdit.DataSource, capModel))
                {
                    ExaminationModel detailExaminationModel = ExaminationDetailEdit.DataSource;
                    UpdateCAPExamination(capModel, detailExaminationModel);
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "examinationFinish", "examinationFinish('" + detailExaminationModel.examinationPKModel.examNbr + "')", true);
                }
            }
        }

        /// <summary>
        /// Check examination is validate or not
        /// </summary>
        /// <param name="detailExaminationModel">The ExaminationModel</param>
        /// <param name="capModel">The CapModel</param>
        /// <returns>true or false</returns>
        private bool IsValidExam(ExaminationModel detailExaminationModel, CapModel4WS capModel)
        {
            if (ACAConstant.EXAMINATION_STATUS_PENDING.Equals(detailExaminationModel.examStatus, StringComparison.InvariantCultureIgnoreCase))
            {
                List<ExaminationModel> checkList = capModel.examinationList.Where(o => o.RowIndex != detailExaminationModel.RowIndex
                    && o.examStatus.Equals(detailExaminationModel.examStatus, StringComparison.InvariantCultureIgnoreCase)
                    && o.examName.Equals(detailExaminationModel.examName, StringComparison.InvariantCultureIgnoreCase)).ToList();

                if (checkList != null && checkList.Count > 0)
                {
                    MessageUtil.ShowMessageInPopup(Page, MessageType.Error, string.Format(GetTextByKey("examination_edit_add_examination_name_check"), detailExaminationModel.examName));
                    return false;
                }

                if (detailExaminationModel.endTime != null)
                {
                    if (detailExaminationModel.endTime < DateTime.Now)
                    {
                        MessageUtil.ShowMessageInPopup(Page, MessageType.Error, GetTextByKey("aca_examination_edit_check_datatime_later"));
                        return false;
                    }
                }
                else
                {
                    if (detailExaminationModel.examDate != null && detailExaminationModel.examDate < DateTime.Now)
                    {
                        MessageUtil.ShowMessageInPopup(Page, MessageType.Error, GetTextByKey("aca_examination_edit_check_datatime_later"));
                        return false;
                    }
                }
            }

            if (ACAConstant.EXAMINATION_STATUS_COMPLETED_PENDING.Equals(detailExaminationModel.examStatus, StringComparison.InvariantCultureIgnoreCase)
                || ACAConstant.EXAMINATION_STATUS_COMPLETED.Equals(detailExaminationModel.examStatus, StringComparison.InvariantCultureIgnoreCase))
            {
                if (detailExaminationModel.endTime != null)
                {
                    if (detailExaminationModel.endTime > DateTime.Now)
                    {
                        MessageUtil.ShowMessageInPopup(Page, MessageType.Error, GetTextByKey("aca_examination_edit_check_datatime_before"));
                        return false;
                    }
                }
                else
                {
                    if (detailExaminationModel.examDate != null && detailExaminationModel.examDate > DateTime.Now)
                    {
                        MessageUtil.ShowMessageInPopup(Page, MessageType.Error, GetTextByKey("aca_examination_edit_check_datatime_before"));
                        return false;
                    }
                }
            }

            if (detailExaminationModel.endTime != null && detailExaminationModel.startTime != null && detailExaminationModel.endTime <= detailExaminationModel.startTime)
            {
                MessageUtil.ShowMessageInPopup(Page, MessageType.Error, GetTextByKey("aca_examination_edit_check_starttime_endtime"));
                return false;
            }

            if (!ACAConstant.EXAMINATION_STATUS_COMPLETED_PENDING.Equals(detailExaminationModel.examStatus, StringComparison.InvariantCultureIgnoreCase)
                && !ACAConstant.EXAMINATION_STATUS_COMPLETED_SCHEDULE.Equals(detailExaminationModel.examStatus, StringComparison.InvariantCultureIgnoreCase)
                && !ACAConstant.EXAMINATION_STATUS_COMPLETED.Equals(detailExaminationModel.examStatus, StringComparison.InvariantCultureIgnoreCase))
            {
                IExaminationBll examinationBLL = (IExaminationBll)ObjectFactory.GetObject(typeof(IExaminationBll));
                bool isWrokflowRestricted = examinationBLL.IsWrokflowRestricted(detailExaminationModel, capModel, "DETAIL", AppSession.User.UserID);

                if (isWrokflowRestricted)
                {
                    MessageUtil.ShowMessageInPopup(Page, MessageType.Error, string.Format(GetTextByKey("examination_edit_add_restrict"), detailExaminationModel.examName));
                    return false;
                }    
            }

            return true;
        }

        /// <summary>
        /// Updates the CAP examination.
        /// </summary>
        /// <param name="capModel">The cap model.</param>
        /// <param name="detailExaminationModel">The detail examination model.</param>
        private void UpdateCAPExamination(CapModel4WS capModel, ExaminationModel detailExaminationModel)
        {
            if (detailExaminationModel != null && detailExaminationModel.examinationPKModel.examNbr != 0)
            {
                for (int i = 0; i < capModel.examinationList.Length; i++)
                {
                    if (capModel.examinationList[i].examinationPKModel.examNbr == detailExaminationModel.examinationPKModel.examNbr)
                    {
                        capModel.examinationList[i] = detailExaminationModel;
                        break;
                    }
                }
            }
            else
            {
                detailExaminationModel.examinationPKModel.examNbr = capModel.examinationList.Length + 1;
                IList<ExaminationModel> examList = ObjectConvertUtil.ConvertArrayToList(capModel.examinationList);
                examList.Add(detailExaminationModel);
                capModel.examinationList = ((List<ExaminationModel>)examList).ToArray();
            }
        }
    }
}