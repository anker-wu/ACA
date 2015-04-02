#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: IExaminationBll.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2009-2014
 *
 *  Description:
 *  ExaminationBll interface..
 *
 *  Notes:
 * $Id: IExaminationBll.cs 139167 2009-07-15 06:20:30Z ACHIEVO\jackie.yu $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using System;
using System.Collections.Generic;

using Accela.ACA.WSProxy;

namespace Accela.ACA.BLL.LicenseCertification
{
    /// <summary>
    /// Examination Interface
    /// </summary>
    public interface IExaminationBll
    {
        /// <summary>
        /// Update Exam List By CSV File
        /// </summary>
        /// <param name="attachment">Attachment Model</param>
        /// <param name="provider">Provider Model</param>
        /// <returns>
        /// Update Exam List Success or Fail
        /// Return Code: ENUM UploadExamReturnCode
        /// GENERIC_ERROR_CODE = -1; Upload exam by CSV is failed. It's generic Error Code for upload, if system meets EMSEException, IOException or technical difficulty.
        /// UPLOAD_SUCCESSFUL = 0; Upload exam by CSV is successful only, if auto-update option is disabled.
        /// SUCCESSFUL = 1; Upload and update exam automatically by CSV is successful both, if auto-update exam option is enabled.
        /// EMPTY_LIST = 2; Upload exam by CSV is failed, get empty list by CSV file. the file failed to update because Record IDs in the file cannot be identified.
        /// CONFIG_ERROR = 3; Upload exam by CSV is failed, The configuration error. Such as EXAM_CSV_FORMAT configuration within standard choice.
        /// AUTO_UPDATE_EXAM_FAILED = 4; Upload exam by CSV is successful, but at least one record updating is failed.
        /// </returns>
        EMSEResultBaseModel4WS UpdateExamListByCSV(AttachmentModel attachment, ProviderPKModel provider);

        /// <summary>
        /// Schedules the exam.
        /// </summary>
        /// <param name="paramModel">The schedule parameter.</param>
        /// <returns>Whether successful.</returns>
        bool ScheduleExam(ScheduleExamParamVO paramModel);

        /// <summary>
        /// Reschedules the exam.
        /// </summary>
        /// <param name="paramModel">The schedule parameter.</param>
        /// <returns>Whether successful.</returns>
        bool RescheduleExam(ScheduleExamParamVO paramModel);

        /// <summary>
        /// Cancel schedule exam.
        /// </summary>
        /// <param name="paramModel">The schedule parameter.</param>
        /// <returns>Whether successful.</returns>
        bool UnScheduleExam(ScheduleExamParamVO paramModel);

        /// <summary>
        /// Gets the available schedule.
        /// </summary>
        /// <param name="paramModel">The schedule parameter.</param>
        /// <returns>Exam schedule model.</returns>
        ExamScheduleViewModel[] GetAvailableSchedule(ExamScheduleSearchModel paramModel);

        /// <summary>
        /// Deletes the exam.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns>Boolean value.</returns>
        bool DeleteExam(ExaminationModel model);

        /// <summary>
        /// Gets the exam by PK.
        /// </summary>
        /// <param name="pkModel">The PK model.</param>
        /// <returns>Exam information.</returns>
        ExaminationModel GetExamByPK(ExaminationPKModel pkModel);

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
        bool HasWorkflowRestriction(CapTypeModel capType, CapIDModel capID, RefExaminationModel refExam, string from, string callerID);

        /// <summary>
        /// Gets the SSO link.
        /// </summary>
        /// <param name="scheduleExamID">The schedule exam ID.</param>
        /// <param name="provider">The provider.</param>
        /// <returns>String of link URL.</returns>
        string GetSSOLink(string scheduleExamID, ProviderPKModel provider);

        /// <summary>
        /// Updates the exam.
        /// </summary>
        /// <param name="examinationModel">The examination model.</param>
        /// <param name="primaryContactSeqNbr">The primary contact sequence NBR.</param>
        /// <returns>Whether successful</returns>
        bool UpdateExam(ExaminationModel examinationModel, string primaryContactSeqNbr);

        /// <summary>
        /// Gets the exam schedule view model.
        /// </summary>
        /// <param name="examScheduleViewModel">The exam schedule view model.</param>
        /// <returns>Examination schedule result view</returns>
        ExamScheduleViewModel GetExamScheduleViewModel(ExamScheduleViewModel examScheduleViewModel);

        /// <summary>
        /// Gets the exam list by cap ID.
        /// </summary>
        /// <param name="capIdModel">The cap id model.</param>
        /// <returns>Daily examination information</returns>
        ExaminationModel[] GetExamListByCapID(CapIDModel capIdModel);

        /// <summary>
        /// Gets the not available exams list by cap ids.
        /// </summary>
        /// <param name="capIds">the cap id models</param>
        /// <returns>return each cap's not available exams</returns>
        ExaminationModel[] GetInavailableExamListByCapIds(CapIDModel4WS[] capIds);

        /// <summary>
        /// Gets the exam schedule view model by exam sequence NBR.
        /// </summary>
        /// <param name="servProvCode">The agency code.</param>
        /// <param name="examSeqNbr">The exam sequence NBR.</param>
        /// <returns>Examination schedule result view</returns>
        ExamScheduleViewModel GetExamScheduleViewModelByExamSeqNbr(string servProvCode, long examSeqNbr);

        /// <summary>
        /// Indicate the examination is restrict or not.
        /// </summary>
        /// <param name="examinationModel">Examination model.</param>
        /// <param name="capModel">cap model.</param>
        /// <param name="from">from client.</param>
        /// <param name="callID">call ID.</param>
        /// <returns><c>true</c> if [is work flow restricted] [the specified examination model]; otherwise, <c>false</c>.</returns>
        bool IsWrokflowRestricted(ExaminationModel examinationModel, CapModel4WS capModel, string from, string callID);

        /// <summary>
        /// Schedules the paid exam by PK.
        /// </summary>
        /// <param name="examinationPk">The examination PK.</param>
        /// <param name="callerId">The caller id.</param>
        /// <returns>Whether successful</returns>
        bool SchedulePaidExamByPK(ExaminationPKModel examinationPk, string callerId);

        /// <summary>
        /// Schedules all paid exam.
        /// </summary>
        /// <param name="capId">The cap id.</param>
        /// <param name="callerId">The caller id.</param>
        /// <returns>Schedule result</returns>
        string ScheduleAllPaidExam(CapIDModel[] capId, string callerId);

        /// <summary>
        /// Gets the exam schedule available seats.
        /// </summary>
        /// <param name="examinationPk">The examination PK.</param>
        /// <returns>available seat</returns>
        int GetExamScheduleAvailableSeats(ExaminationPKModel examinationPk);

        /// <summary>
        /// Insert exam information to DB and status is <c>readytoscheduleunpaid</c>,
        /// <para>when examination fee is 0, auto schedule it</para>
        /// <para>return true: scheduled successful</para>
        /// <para>return false: examination fee is not 0, need payment</para>
        /// </summary>
        /// <param name="scheduleParam">The schedule parameter.</param>
        /// <returns><c>true</c> if successful, <c>false</c> otherwise.</returns>
        bool PreSchduleExam(ScheduleExamParamVO scheduleParam);

        /// <summary>
        /// Reset exam for ready to schedule to pending status.
        /// </summary>
        /// <param name="pkModel">The PK model.</param>
        /// <param name="capId">The cap id.</param>
        /// <param name="callerId">The caller id.</param>
        void ResetReady2ScheduleExam(ExaminationPKModel pkModel, CapIDModel capId, string callerId);

        /// <summary>
        /// Determines whether [has ready to schedule examination] [the specified cap ID list].
        /// </summary>
        /// <param name="capIDList">The cap ID list.</param>
        /// <returns>
        ///   <c>true</c> if [has ready to schedule examination] [the specified cap ID list]; otherwise, <c>false</c>.
        /// </returns>
        bool HasReady2ScheduleExamination(CapIDModel[] capIDList);

        /// <summary>
        /// Get the reference contact's examination list.
        /// </summary>
        /// <param name="refContactSeqNbr">The reference contact sequence number.</param>
        /// <returns>Examination model array.</returns>
        ExaminationModel[] GetRefPeopleExamList(string refContactSeqNbr);

        /// <summary>
        /// Add or update the reference contact's examination.
        /// </summary>
        /// <param name="examinationModel">The Examination model.</param>
        /// <returns>True: success, otherwise fail.</returns>
        bool AddOrUpdateRefPeopleExam(ExaminationModel examinationModel);

        /// <summary>
        /// Update Proctor Response Status from Email.
        /// </summary>
        /// <param name="agencyCode">Agency code.</param>
        /// <param name="resID">The reference contact sequence number.</param>
        /// <param name="isReceived">is email response received.</param>
        /// <param name="uuid">UU id.</param>
        /// <param name="callerId">caller id.</param>
        /// <returns>Proctor status: 0. Accept 1.Reject. -1. Others</returns>
        int UpdateProctorResponseStatus(string agencyCode, string resID, string isReceived, string uuid, string callerId);
    }
}
