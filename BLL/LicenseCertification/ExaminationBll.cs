#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: ExaminationBll.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2009-2014
 *
 *  Description:
 *  Examination Business Logic
 *
 *  Notes:
 * $Id: ExaminationBll.cs 139167 2009-07-17 17:20:30Z ACHIEVO\solt.su $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using System;
using Accela.ACA.Common;
using Accela.ACA.WSProxy;

namespace Accela.ACA.BLL.LicenseCertification
{
    /// <summary>
    /// This class provide the ability to operation daily side Examination.
    /// </summary>
    public class ExaminationBll : BaseBll, IExaminationBll
    {
        #region Fields

        #endregion

        #region Properties

        /// <summary>
        /// Gets an instance of ExaminationService.
        /// </summary>
        private ExaminationWebServiceService ExaminationService
        {
            get
            {
                return WSFactory.Instance.GetWebService<ExaminationWebServiceService>();
            }
        }

        /// <summary>
        /// Gets an instance of EDMSDocumentUploadService.
        /// </summary>
        private EDMSDocumentUploadWebServiceService EDMSDocumentUploadService
        {
            get
            {
                return WSFactory.Instance.GetWebService3<EDMSDocumentUploadWebServiceService>();
            }
        }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Update Exam List By CSV File
        /// </summary>
        /// <param name="attachment">Attachment Model</param>
        /// <param name="provider">Provider Model</param>
        /// <returns>Update Exam List Success or Fail
        /// Return Code: ENUM UploadExamReturnCode
        /// GENERIC_ERROR_CODE = -1; Upload exam by CSV is failed. It's generic Error Code for upload, if system meets EMSEException, IOException or technical difficulty.
        /// UPLOAD_SUCCESSFUL = 0; Upload exam by CSV is successful only, if auto-update option is disabled.
        /// SUCCESSFUL = 1; Upload and update exam automatically by CSV is successful both, if auto-update exam option is enabled.
        /// EMPTY_LIST = 2; Upload exam by CSV is failed, get empty list by CSV file. the file failed to update because Record IDs in the file cannot be identified.
        /// CONFIG_ERROR = 3; Upload exam by CSV is failed, The configuration error. Such as EXAM_CSV_FORMAT configuration within standard choice.
        /// AUTO_UPDATE_EXAM_FAILED = 4; Upload exam by CSV is successful, but at least one record updating is failed.</returns>
        /// <exception cref="ACAException">Exception from web service.</exception>
        EMSEResultBaseModel4WS IExaminationBll.UpdateExamListByCSV(AttachmentModel attachment, ProviderPKModel provider)
        {
            try
            {
                return EDMSDocumentUploadService.updateExamListByCSV(attachment, provider);
            }
            catch (Exception exception)
            {
                throw new ACAException(exception);
            }
        }

        /// <summary>
        /// Schedules the exam.
        /// </summary>
        /// <param name="paramModel">The schedule parameter.</param>
        /// <returns>Whether successful.</returns>
        /// <exception cref="ACAException">Exception from web service.</exception>
        public bool ScheduleExam(ScheduleExamParamVO paramModel)
        {
            try
            {
                return ExaminationService.scheduleExam(paramModel);
            }
            catch (Exception exception)
            {
                throw new ACAException(exception);
            }
        }

        /// <summary>
        /// Reschedules the exam.
        /// </summary>
        /// <param name="paramModel">The schedule parameter.</param>
        /// <returns>Whether successful.</returns>
        /// <exception cref="ACAException">Exception from web service.</exception>
        public bool RescheduleExam(ScheduleExamParamVO paramModel)
        {
            try
            {
                return ExaminationService.rescheduleExam(paramModel);
            }
            catch (Exception exception)
            {
                throw new ACAException(exception);
            }
        }

        /// <summary>
        /// Cancel schedule exam.
        /// </summary>
        /// <param name="paramModel">The schedule parameter.</param>
        /// <returns>Whether successful.</returns>
        /// <exception cref="ACAException">Exception from web service.</exception>
        public bool UnScheduleExam(ScheduleExamParamVO paramModel)
        {
            try
            {
                return ExaminationService.unScheduleExam(paramModel);
            }
            catch (Exception exception)
            {
                throw new ACAException(exception);
            }
        }

        /// <summary>
        /// Gets the available schedule.
        /// </summary>
        /// <param name="paramModel">The schedule parameter.</param>
        /// <returns>Exam schedule model.</returns>
        /// <exception cref="ACAException">Exception from web service.</exception>
        public ExamScheduleViewModel[] GetAvailableSchedule(ExamScheduleSearchModel paramModel)
        {
            try
            {
                return ExaminationService.getAvailableSchedule(paramModel);
            }
            catch (Exception exception)
            {
                throw new ACAException(exception);
            }
        }

        /// <summary>
        /// Deletes the exam.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns>Boolean value.</returns>
        /// <exception cref="ACAException">Exception from web service.</exception>
        public bool DeleteExam(ExaminationModel model)
        {
            try
            {
                return ExaminationService.deleteExam(model);
            }
            catch (Exception exception)
            {
                throw new ACAException(exception);
            }
        }

        /// <summary>
        /// Gets the exam by PK.
        /// </summary>
        /// <param name="pkModel">The PK model.</param>
        /// <returns>Exam information.</returns>
        /// <exception cref="ACAException">Exception from web service.</exception>
        public ExaminationModel GetExamByPK(ExaminationPKModel pkModel)
        {
            try
            {
                return ExaminationService.getExamByPK(pkModel);
            }
            catch (Exception exception)
            {
                throw new ACAException(exception);
            }
        }

        /// <summary>
        /// Determines whether [has workflow restriction] [the specified cap type].
        /// </summary>
        /// <param name="capType">Type of the cap.</param>
        /// <param name="capID">The cap ID.</param>
        /// <param name="refExam">The ref exam.</param>
        /// <param name="from">From the "SPEAR" or "Detail".</param>
        /// <param name="callerID">The caller ID.</param>
        /// <returns>
        /// <c>true</c> if [has workflow restriction] [the specified cap type]; otherwise, <c>false</c>.
        /// </returns>
        public bool HasWorkflowRestriction(CapTypeModel capType, CapIDModel capID, RefExaminationModel refExam, string from, string callerID)
        {
            try
            {
                return ExaminationService.hasWorkflowRestriction(capType, capID, refExam, from, callerID);
            }
            catch (Exception exception)
            {
                throw new ACAException(exception);
            }
        }

        /// <summary>
        /// Gets the SSO link.
        /// </summary>
        /// <param name="scheduleExamID">The schedule exam ID.</param>
        /// <param name="provider">The provider.</param>
        /// <returns>String of link URL.</returns>
        public string GetSSOLink(string scheduleExamID, ProviderPKModel provider)
        {
            try
            {
                return ExaminationService.getSSOLink(scheduleExamID, provider);
            }
            catch (Exception exception)
            {
                throw new ACAException(exception);
            }
        }

        /// <summary>
        /// Updates the exam.
        /// </summary>
        /// <param name="examinationModel">The examination model.</param>
        /// <param name="primaryContactSeqNbr">The primary contact sequence NBR.</param>
        /// <returns>Whether successful</returns>
        /// <exception cref="ACAException">Exception from web service.</exception>
        public bool UpdateExam(ExaminationModel examinationModel, string primaryContactSeqNbr)
        {
            try
            {
                return ExaminationService.updateExam(AgencyCode, examinationModel, primaryContactSeqNbr);
            }
            catch (Exception exception)
            {
                throw new ACAException(exception);
            }
        }

        /// <summary>
        /// Gets the exam schedule view model.
        /// </summary>
        /// <param name="examScheduleViewModel">The exam schedule view model.</param>
        /// <returns>Examination schedule result view</returns>
        /// <exception cref="ACAException">Exception from web service.</exception>
        public ExamScheduleViewModel GetExamScheduleViewModel(ExamScheduleViewModel examScheduleViewModel)
        {
            try
            {
                return ExaminationService.getExamScheduleViewModel(examScheduleViewModel);
            }
            catch (Exception exception)
            {
                throw new ACAException(exception);
            }
        }

        /// <summary>
        /// Gets the exam list by cap ID.
        /// </summary>
        /// <param name="capIdModel">The cap id model.</param>
        /// <returns>Daily examination information</returns>
        /// <exception cref="ACAException">Exception from web service.</exception>
        public ExaminationModel[] GetExamListByCapID(CapIDModel capIdModel)
        {
            try
            {
                return ExaminationService.getExamListByCapID(capIdModel);
            }
            catch (Exception exception)
            {
                throw new ACAException(exception);
            }
        }

        /// <summary>
        /// Gets the in-available exams by cap id
        /// </summary>
        /// <param name="capIds">the cap ids to get exam list</param>
        /// <returns>return the in-available exam list of each cap.</returns>
        public ExaminationModel[] GetInavailableExamListByCapIds(CapIDModel4WS[] capIds)
        {
            try
            {
                return ExaminationService.getInavailableExamListByCapIds(capIds);
            }
            catch (Exception exception)
            {
                throw new ACAException(exception);
            }
        }

        /// <summary>
        /// Gets the exam schedule view model by exam sequence NBR.
        /// </summary>
        /// <param name="servProvCode">The agency code.</param>
        /// <param name="examSeqNbr">The exam sequence NBR.</param>
        /// <returns>Examination schedule result view</returns>
        /// <exception cref="ACAException">Exception from web service.</exception>
        public ExamScheduleViewModel GetExamScheduleViewModelByExamSeqNbr(string servProvCode, long examSeqNbr)
        {
            try
            {
                return ExaminationService.getExamScheduleViewModelByExamSeqNbr(servProvCode, examSeqNbr, true);
            }
            catch (Exception exception)
            {
                throw new ACAException(exception);
            }
        }

        /// <summary>
        /// Indicate the examination is restrict or not.
        /// </summary>
        /// <param name="examinationModel">Examination model.</param>
        /// <param name="capModel">cap model.</param>
        /// <param name="from">from client.</param>
        /// <param name="callID">call ID.</param>
        /// <returns><c>true</c> if [is work flow restricted] [the specified examination model]; otherwise, <c>false</c>.</returns>
        /// <exception cref="ACAException">Exception from web service.</exception>
        public bool IsWrokflowRestricted(ExaminationModel examinationModel, CapModel4WS capModel, string from, string callID)
        {
            try
            {
                bool isRestrict = false;

                if (examinationModel != null && examinationModel.refExamSeq != null)
                {
                    RefExaminationModel refExam = new RefExaminationModel();
                    refExam.examName = examinationModel.examName;
                    refExam.refExamNbr = examinationModel.refExamSeq;
                    refExam.serviceProviderCode = ACAConstant.AgencyCode;

                    if (capModel != null && capModel.capDetailModel != null)
                    {
                        isRestrict = this.HasWorkflowRestriction(
                            capModel.capType, capModel.capDetailModel.capID, refExam, from, callID);
                    }
                    else
                    {
                        isRestrict = this.HasWorkflowRestriction(capModel.capType, null, refExam, from, callID);
                    }
                }

                return isRestrict;
            }
            catch (Exception exception)
            {
                throw new ACAException(exception);
            }
        }

        /// <summary>
        /// Schedules the paid exam by PK.
        /// </summary>
        /// <param name="examinationPk">The examination PK.</param>
        /// <param name="callerId">The caller id.</param>
        /// <returns>Whether successful</returns>
        /// <exception cref="ACAException">Exception from web service.</exception>
        public bool SchedulePaidExamByPK(ExaminationPKModel examinationPk, string callerId)
        {
            try
            {
                return ExaminationService.schedulePaidExamByPK(examinationPk, callerId);
            }
            catch (Exception exception)
            {
                throw new ACAException(exception);
            }
        }

        /// <summary>
        /// Schedules all paid exam.
        /// </summary>
        /// <param name="capIds">The cap id.</param>
        /// <param name="callerId">The caller id.</param>
        /// <returns>Schedule result</returns>
        /// <exception cref="ACAException">Exception from web service.</exception>
        public string ScheduleAllPaidExam(CapIDModel[] capIds, string callerId)
        {
            try
            {
                return ExaminationService.scheduleAllPaidExam(capIds, callerId);
            }
            catch (Exception exception)
            {
                throw new ACAException(exception);
            }
        }

        /// <summary>
        /// Gets the exam schedule available seats.
        /// </summary>
        /// <param name="examinationPk">The examination PK.</param>
        /// <returns>available seat</returns>
        /// <exception cref="ACAException">Exception from web service.</exception>
        public int GetExamScheduleAvailableSeats(ExaminationPKModel examinationPk)
        {
            try
            {
                return ExaminationService.getExamScheduleAvailableSeats(examinationPk);
            }
            catch (Exception exception)
            {
                throw new ACAException(exception);
            }
        }

        /// <summary>
        /// Insert exam information to DB and status is <c>readytoscheduleunpaid</c>,
        /// <para>when examination fee is 0, auto schedule it</para>
        /// <para>return true: scheduled successful</para>
        /// <para>return false: examination fee is not 0, need payment</para>
        /// </summary>
        /// <param name="scheduleParam">The schedule parameter.</param>
        /// <returns><c>true</c> if successful, <c>false</c> otherwise.</returns>
        /// <exception cref="ACAException">Exception from web service.</exception>
        public bool PreSchduleExam(ScheduleExamParamVO scheduleParam)
        {
            try
            {
                return ExaminationService.preScheduleExam(scheduleParam);
            }
            catch (Exception exception)
            {
                throw new ACAException(exception);
            }
        }

        /// <summary>
        /// Reset exam for ready to schedule to pending status.
        /// </summary>
        /// <param name="pkModel">The PK model.</param>
        /// <param name="capId">The cap id.</param>
        /// <param name="callerId">The caller id.</param>
        /// <exception cref="ACAException">Exception from web service.</exception>
        public void ResetReady2ScheduleExam(ExaminationPKModel pkModel, CapIDModel capId, string callerId)
        {
            try
            {
                ExaminationService.resetReady2ScheduleExam(pkModel, capId, callerId);
            }
            catch (Exception exception)
            {
                throw new ACAException(exception);
            }
        }

        /// <summary>
        /// Determines whether [has ready to schedule examination] [the specified cap ID list].
        /// </summary>
        /// <param name="capIDList">The cap ID list.</param>
        /// <returns>
        ///   <c>true</c> if [has ready to schedule examination] [the specified cap ID list]; otherwise, <c>false</c>.
        /// </returns>
        public bool HasReady2ScheduleExamination(CapIDModel[] capIDList)
        {
            try
            {
                if (capIDList == null || capIDList.Length == 0)
                {
                    throw new ArgumentException("capIDList");
                }

                return ExaminationService.hasReady2ScheduleExamination(capIDList);
            }
            catch (Exception exception)
            {
                throw new ACAException(exception);
            }
        }

        /// <summary>
        /// Get the reference contact's examination list.
        /// </summary>
        /// <param name="refContactSeqNbr">The reference contact sequence number.</param>
        /// <returns>Examination model array.</returns>
        public ExaminationModel[] GetRefPeopleExamList(string refContactSeqNbr)
        {
            if (string.IsNullOrEmpty(refContactSeqNbr))
            {
                throw new DataValidateException(new string[] { "refContactSeqNbr" });
            }

            try
            {
                return ExaminationService.getRefPeopleExamList(AgencyCode, refContactSeqNbr);
            }
            catch (Exception e)
            {
                throw new ACAException(e);
            }
        }

        /// <summary>
        /// Add or update the reference contact's examination.
        /// </summary>
        /// <param name="examinationModel">The Examination model.</param>
        /// <returns>True: success, otherwise fail.</returns>
        public bool AddOrUpdateRefPeopleExam(ExaminationModel examinationModel)
        {
            try
            {
                return ExaminationService.updateRefPeopleExam(examinationModel, User.PublicUserId);
            }
            catch (Exception exception)
            {
                throw new ACAException(exception);
            }
        }

        /// <summary>
        /// Update Proctor Response Status from Email.
        /// </summary>
        /// <param name="agencyCode">Agency code.</param>
        /// <param name="resID">The reference contact sequence number.</param>
        /// <param name="isReceived">is email response received.</param>
        /// <param name="uuid">UU id.</param>
        /// <param name="callerId">caller id.</param>
        /// <returns>Proctor status: 0. Accept 1.Reject. -1. Others</returns>
        public int UpdateProctorResponseStatus(string agencyCode, string resID, string isReceived, string uuid, string callerId)
        {
            try
            {
                return ExaminationService.updateProctorResponseStatus(agencyCode, resID, isReceived, uuid, callerId);
            }
            catch (Exception exception)
            {
                throw new ACAException(exception);
            }
        }

        #endregion
    }
}
