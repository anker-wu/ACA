#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: InspectionStatusBll.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2010-2014
 *
 *  Description:
 *
 *  Notes:
 *      $Id: InspectionStatusUtil.cs 278225 2014-08-29 08:02:26Z ACHIEVO\james.shi $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using System;

using Accela.ACA.Common;
using Accela.ACA.Inspection;
using Accela.ACA.WSProxy;

namespace Accela.ACA.BLL.Inspection
{
    /// <summary>
    /// This class provides the ability to operation inspection status.
    /// </summary>
    internal static class InspectionStatusUtil
    {
        #region Fields

        /// <summary>
        /// Inspection status for Cancelled
        /// </summary>
        private const string INSP_STATUS_CANCELLED = "Cancelled";

        /// <summary>
        /// Inspection status description for Cancelled
        /// </summary>
        private const string INSP_STATUS_DESC_CANCELLED = "Insp Cancelled";

        /// <summary>
        /// Inspection status description for pending
        /// </summary>
        private const string INSP_STATUS_DESC_PENDING = "Insp Pending";

        /// <summary>
        /// Inspection description (could be get from InspectionModel.ActivityModel.documentDescription)
        /// </summary>
        /// <remarks>
        /// Rules for identifying actual inspection status:
        /// 1.Pending (new inspection status, not result Pending)
        /// ActivityModel.documentDescription: Inspection Pending
        /// ActivityModel.status: Pending
        /// 2.Schedule
        /// ActivityModel.documentDescription: Inspection Scheduled
        /// ActivityModel.status: Scheduled
        /// 3.Reschedule
        /// ActivityModel.documentDescription: Inspection Rescheduled
        /// ActivityModel.status: Rescheduled
        /// 4.Cancelled
        /// ActivityModel.documentDescription: Inspection Cancelled
        /// ActivityModel.status: Cancelled
        /// 5.Result Pending
        /// ActivityModel.documentDescription: Inspection Scheduled
        /// InspectionResultGroupModel.resultType: PENDING
        /// 6.Result Denied
        /// ActivityModel.documentDescription: Inspection Cancelled
        /// InspectionResultGroupModel.resultType: DENIED
        /// 7.Result Approved
        /// ActivityModel.documentDescription: Inspection Completed
        /// InspectionResultGroupModel.resultType: APPROVED
        /// </remarks>
        private const string INSP_STATUS_DESC_RESCHEDULED = "Insp Rescheduled";

        /// <summary>
        /// Inspection status description for result completed
        /// </summary>
        private const string INSP_STATUS_DESC_RESULT = "Insp Completed";

        /// <summary>
        /// Inspection status description for scheduled
        /// </summary>
        private const string INSP_STATUS_DESC_SCHEDULED = "Insp Scheduled";

        /// <summary>
        /// Inspection status for pending
        /// </summary>
        private const string INSP_STATUS_PENDING = "Pending";

        /// <summary>
        /// Inspection status (could be get from InspectionModel.ActivityModel.status)
        /// </summary>
        /// <remarks>
        /// Note: InspectionModel.ActivityModel.status could be :
        /// a.one of below definitions
        /// b.one of result values
        /// </remarks>
        private const string INSP_STATUS_RESCHEDULED = "Rescheduled";

        /// <summary>
        /// Inspection result type (InspectionTypeModel.InspectionResultGroupModel.InspectionResultModel[].resultType)
        /// </summary>
        /// <remarks>
        /// Note: inspection result value(InspectionTypeModel.InspectionResultGroupModel.InspectionResultModel[].resultValue)
        /// is customized from result type.
        /// </remarks>
        private const string INSP_STATUS_RESULT_APPROVED = "APPROVED";

        /// <summary>
        /// Inspection status result for denied
        /// </summary>
        private const string INSP_STATUS_RESULT_DENIED = "DENIED";

        /// <summary>
        /// Inspection status result for pending
        /// </summary>
        private const string INSP_STATUS_RESULT_PENDING = "PENDING";

        /// <summary>
        /// Inspection status for scheduled
        /// </summary>
        private const string INSP_STATUS_SCHEDULED = "Scheduled";

        #endregion Fields

        #region Methods

        /// <summary>
        /// check to see if can ignore inspection flow restriction
        /// </summary>
        /// <param name="scheduleType">the instance of scheduleType</param>
        /// <returns>true: If RequestOnlyPending or RequestSameDayNextDay</returns>
        public static bool CanIgnoreFlow(InspectionScheduleType scheduleType)
        {
            bool result = false;

            switch (scheduleType)
            {
                case InspectionScheduleType.RequestOnlyPending:
                case InspectionScheduleType.RequestSameDayNextDay:
                    result = true;
                    break;
            }

            return result;
        }

        /// <summary>
        /// check to see if can ignore inspection flow restriction
        /// </summary>
        /// <param name="operation">inspection operation</param>
        /// <returns>true:ignore flow, false:can't ignore flow</returns>
        public static bool CanIgnoreFlow(InspectionAction operation)
        {
            bool result = operation == InspectionAction.Request;

            return result;
        }

        /// <summary>
        /// Get inspection actual status
        /// </summary>
        /// <param name="inspectionTypeModel">The InspectionTypeModel</param>
        /// <returns>The InspectionStatus</returns>
        public static InspectionStatus GetStatus(InspectionTypeModel inspectionTypeModel)
        {
            return GetStatus(inspectionTypeModel, null);
        }

        /// <summary>
        /// Get inspection actual status
        /// </summary>
        /// <param name="inspectionTypeModel">the instance of InspectionTypeModel</param>
        /// <param name="inspectionModel">the instance of InspectionModel</param>
        /// <returns>the type of InspectionStatus</returns>
        public static InspectionStatus GetStatus(InspectionTypeModel inspectionTypeModel, InspectionModel inspectionModel)
        {
            //if is unknown.
            if (inspectionTypeModel == null)
            {
                return InspectionStatus.Unknown;
            }

            //check if can ignore flow 
            bool canIgnoreFlow = false;

            if (inspectionModel != null)
            {
                InspectionScheduleType scheduleType = InspectionScheduleTypeUtil.GetScheduleType(inspectionTypeModel);
                canIgnoreFlow = CanIgnoreFlow(scheduleType);
            }

            //get inspection status
            string inspectionStatus = (inspectionModel == null || inspectionModel.activity == null || string.IsNullOrEmpty(inspectionModel.activity.status)) ? string.Empty : inspectionModel.activity.status;

            //get inspection document description
            string documentDescription = (inspectionModel == null || inspectionModel.activity == null || string.IsNullOrEmpty(inspectionModel.activity.documentDescription)) ? string.Empty : inspectionModel.activity.documentDescription;

            //get flag indicating whether or not current inspection has been resulted.
            bool hasBeenResulted = INSP_STATUS_DESC_RESULT.Equals(documentDescription, StringComparison.InvariantCultureIgnoreCase);

            //get result type
            string resultType = GetResultType(inspectionStatus, inspectionTypeModel);

            //get created_by_ACA
            string createdByACA = (inspectionModel == null || inspectionModel.activity == null || string.IsNullOrEmpty(inspectionModel.activity.createdByACA)) ? string.Empty : inspectionModel.activity.createdByACA;
            bool isCreatedByACA = ACAConstant.COMMON_Y.Equals(createdByACA, StringComparison.InvariantCultureIgnoreCase);

            InspectionStatus actualStatus = InspectionStatus.Unknown;

            if (inspectionTypeModel != null && inspectionModel == null && inspectionTypeModel.isConfiguredInInspFlow && inspectionTypeModel.isCompleted)
            {
                //if the standchoise KEEP_MULTIPLE_COMPLETE_ACTIVE set to Yes
                if (inspectionTypeModel.isActive)
                {
                    if (INSP_STATUS_DESC_SCHEDULED.Equals(documentDescription, StringComparison.InvariantCultureIgnoreCase) &&
                INSP_STATUS_SCHEDULED.Equals(inspectionStatus, StringComparison.InvariantCultureIgnoreCase))
                    {
                        actualStatus = InspectionStatus.Scheduled;
                    }
                    else
                    {
                        actualStatus = InspectionStatus.FlowCompletedButActive;
                    }
                }
                else
                {
                    actualStatus = InspectionStatus.FlowCompleted;
                }
            }
            else if (inspectionTypeModel != null 
                && inspectionModel == null 
                && inspectionTypeModel.isConfiguredInInspFlow 
                && !(inspectionTypeModel.isActive || inspectionTypeModel.isInAdvance) 
                && !canIgnoreFlow)
            {
                //if flow prerequisite is not met.
                actualStatus = InspectionStatus.FlowPrerequisiteNotMet;
            }
            else if (!hasBeenResulted && inspectionTypeModel != null && inspectionModel == null)
            {
                //if is initial(required/optional)
                if (ACAConstant.COMMON_Y.Equals(inspectionTypeModel.requiredInspection, StringComparison.InvariantCulture))
                {
                    actualStatus = InspectionStatus.InitialRequired;
                }
                else
                {
                    actualStatus = InspectionStatus.InitialOptional;
                }
            }
            else if (INSP_STATUS_DESC_PENDING.Equals(documentDescription, StringComparison.InvariantCultureIgnoreCase) &&
                INSP_STATUS_PENDING.Equals(inspectionStatus, StringComparison.InvariantCultureIgnoreCase))
            {
                //if is pending
                actualStatus = isCreatedByACA ? InspectionStatus.PendingByACA : InspectionStatus.PendingByV360;
            }
            else if (INSP_STATUS_DESC_SCHEDULED.Equals(documentDescription, StringComparison.InvariantCultureIgnoreCase) &&
                INSP_STATUS_SCHEDULED.Equals(inspectionStatus, StringComparison.InvariantCultureIgnoreCase))
            {
                //if is scheduled
                actualStatus = InspectionStatus.Scheduled;
            }
            else if (INSP_STATUS_DESC_RESCHEDULED.Equals(documentDescription, StringComparison.InvariantCultureIgnoreCase) &&
                INSP_STATUS_RESCHEDULED.Equals(inspectionStatus, StringComparison.InvariantCultureIgnoreCase))
            {
                //if is rescheduled.
                actualStatus = InspectionStatus.Rescheduled;
            }
            else if (INSP_STATUS_DESC_CANCELLED.Equals(documentDescription, StringComparison.InvariantCultureIgnoreCase) &&
                INSP_STATUS_CANCELLED.Equals(inspectionStatus, StringComparison.InvariantCultureIgnoreCase))
            {
                //if is cancelled.
                actualStatus = InspectionStatus.Canceled;
            }
            else if (INSP_STATUS_DESC_SCHEDULED.Equals(documentDescription, StringComparison.InvariantCultureIgnoreCase) &&
                INSP_STATUS_RESULT_PENDING.Equals(resultType, StringComparison.InvariantCultureIgnoreCase))
            {
                //if is result pending.
                actualStatus = InspectionStatus.ResultPending;
            }
            else if (INSP_STATUS_DESC_RESULT.Equals(documentDescription, StringComparison.InvariantCultureIgnoreCase) &&
                INSP_STATUS_RESULT_APPROVED.Equals(resultType, StringComparison.InvariantCultureIgnoreCase))
            {
                //if is result approved.
                actualStatus = InspectionStatus.ResultApproved;
            }
            else if (INSP_STATUS_DESC_CANCELLED.Equals(documentDescription, StringComparison.InvariantCultureIgnoreCase) &&
                INSP_STATUS_RESULT_DENIED.Equals(resultType, StringComparison.InvariantCultureIgnoreCase))
            {
                //if is result denied.
                actualStatus = InspectionStatus.ResultDenied;
            }

            return actualStatus;
        }

        /// <summary>
        /// Gets the inspection actual status.
        /// </summary>
        /// <param name="inspectionModel">The inspection model.</param>
        /// <returns>the type of InspectionStatus</returns>
        public static InspectionStatus GetStatus(InspectionModel inspectionModel)
        {
            if (inspectionModel == null)
            {
                return InspectionStatus.Unknown;
            }

            //get inspection status
            string inspectionStatus = (inspectionModel == null || inspectionModel.activity == null || string.IsNullOrEmpty(inspectionModel.activity.status)) ? string.Empty : inspectionModel.activity.status;

            //get inspection document description
            string documentDescription = (inspectionModel == null || inspectionModel.activity == null || string.IsNullOrEmpty(inspectionModel.activity.documentDescription)) ? string.Empty : inspectionModel.activity.documentDescription;

            //get flag indicating whether or not current inspection has been resulted.
            bool hasBeenResulted = INSP_STATUS_DESC_RESULT.Equals(documentDescription, StringComparison.InvariantCultureIgnoreCase);

            //get result type
            string resultType = inspectionModel.activity.inspResultType;

            //get created_by_ACA
            string createdByACA = (inspectionModel == null || inspectionModel.activity == null || string.IsNullOrEmpty(inspectionModel.activity.createdByACA)) ? string.Empty : inspectionModel.activity.createdByACA;
            bool isCreatedByACA = ACAConstant.COMMON_Y.Equals(createdByACA, StringComparison.InvariantCultureIgnoreCase);

            InspectionStatus actualStatus = hasBeenResulted ? InspectionStatus.ResultPending : InspectionStatus.Unknown;

            if (INSP_STATUS_DESC_PENDING.Equals(documentDescription, StringComparison.InvariantCultureIgnoreCase) &&
                INSP_STATUS_PENDING.Equals(inspectionStatus, StringComparison.InvariantCultureIgnoreCase))
            {
                //if is pending
                actualStatus = isCreatedByACA ? InspectionStatus.PendingByACA : InspectionStatus.PendingByV360;
            }
            else if (INSP_STATUS_DESC_SCHEDULED.Equals(documentDescription, StringComparison.InvariantCultureIgnoreCase) &&
                INSP_STATUS_SCHEDULED.Equals(inspectionStatus, StringComparison.InvariantCultureIgnoreCase))
            {
                //if is scheduled
                actualStatus = InspectionStatus.Scheduled;
            }
            else if (INSP_STATUS_DESC_RESCHEDULED.Equals(documentDescription, StringComparison.InvariantCultureIgnoreCase) &&
                INSP_STATUS_RESCHEDULED.Equals(inspectionStatus, StringComparison.InvariantCultureIgnoreCase))
            {
                //if is rescheduled.
                actualStatus = InspectionStatus.Rescheduled;
            }
            else if (INSP_STATUS_DESC_CANCELLED.Equals(documentDescription, StringComparison.InvariantCultureIgnoreCase) &&
                INSP_STATUS_CANCELLED.Equals(inspectionStatus, StringComparison.InvariantCultureIgnoreCase))
            {
                //if is cancelled.
                actualStatus = InspectionStatus.Canceled;
            }
            else if (INSP_STATUS_DESC_SCHEDULED.Equals(documentDescription, StringComparison.InvariantCultureIgnoreCase) &&
                INSP_STATUS_RESULT_PENDING.Equals(resultType, StringComparison.InvariantCultureIgnoreCase))
            {
                //if is result pending.
                actualStatus = InspectionStatus.ResultPending;
            }
            else if (INSP_STATUS_DESC_RESULT.Equals(documentDescription, StringComparison.InvariantCultureIgnoreCase) &&
                INSP_STATUS_RESULT_APPROVED.Equals(resultType, StringComparison.InvariantCultureIgnoreCase))
            {
                //if is result approved.
                actualStatus = InspectionStatus.ResultApproved;
            }
            else if (INSP_STATUS_DESC_CANCELLED.Equals(documentDescription, StringComparison.InvariantCultureIgnoreCase) &&
                INSP_STATUS_RESULT_DENIED.Equals(resultType, StringComparison.InvariantCultureIgnoreCase))
            {
                //if is result denied.
                actualStatus = InspectionStatus.ResultDenied;
            }

            return actualStatus;
        }

        /// <summary>
        /// Gets the res status string.
        /// </summary>
        /// <param name="inspectionStatus">The inspection status.</param>
        /// <param name="inspectionModel">The inspection model.</param>
        /// <returns>the res status string.</returns>
        public static string GetResStatusString(InspectionStatus inspectionStatus, InspectionModel inspectionModel)
        {
            string result = string.Empty;

            if (inspectionModel != null && inspectionModel.activity != null)
            {
                bool isResultStatus = InspectionStatusUtil.IsResultStatus(inspectionStatus);
                bool isUserDefinedResultStatus = InspectionStatusUtil.IsUserDefinedResultStatus(inspectionModel.activity.status);

                //get inspection status
                string resStatusString = inspectionModel.activity.resStatus;

                if (string.IsNullOrEmpty(resStatusString) && isResultStatus && isUserDefinedResultStatus)
                {
                    resStatusString = inspectionModel.activity.status;
                }

                result = resStatusString;
            }

            return result;
        }

        /// <summary>
        /// get next actual status (after be operated).
        /// </summary>
        /// <param name="action">The action.</param>
        /// <returns>next actual status</returns>
        public static string GetNextSystemDefinedStatus(InspectionAction action)
        {
            string result = string.Empty;

            switch (action)
            {
                case InspectionAction.Request:
                    result = INSP_STATUS_PENDING;
                    break;

                case InspectionAction.Schedule:
                    result = INSP_STATUS_SCHEDULED;
                    break;

                case InspectionAction.Reschedule:
                    result = INSP_STATUS_RESCHEDULED;
                    break;

                case InspectionAction.Cancel:
                    result = INSP_STATUS_CANCELLED;
                    break;

                case InspectionAction.DoComplete:
                case InspectionAction.DoPrerequisiteNotMet:
                case InspectionAction.None:
                default:
                    result = string.Empty;
                    break;
            }

            return result;
        }

        /// <summary>
        /// Determines whether current status is result status.
        /// </summary>
        /// <param name="actualStatus">The actual status.</param>
        /// <returns>
        /// <c>true</c> if is result status; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsResultStatus(InspectionStatus actualStatus)
        {
            bool result = false;

            switch (actualStatus)
            {
                case InspectionStatus.ResultApproved:
                case InspectionStatus.ResultDenied:
                case InspectionStatus.ResultPending:
                    result = true;
                    break;
            }

            return result;
        }

        /// <summary>
        /// Determines whether current status is processing status.
        /// </summary>
        /// <param name="actualStatus">The actual status.</param>
        /// <returns>
        /// <c>true</c> if current status is processing status.; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsProcessingStatus(InspectionStatus actualStatus)
        {
            bool result = false;

            switch (actualStatus)
            {
                case InspectionStatus.FlowCompletedButActive:
                case InspectionStatus.InitialOptional:
                case InspectionStatus.InitialRequired:
                case InspectionStatus.PendingByACA:
                case InspectionStatus.PendingByV360:
                case InspectionStatus.ResultPending:
                case InspectionStatus.Scheduled:
                    result = true;
                    break;
            }

            return result;
        }

        /// <summary>
        /// Determines whether current status is end status.
        /// </summary>
        /// <param name="actualStatus">The actual status.</param>
        /// <returns>
        /// <c>true</c> if current status is end status; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsEndStatus(InspectionStatus actualStatus)
        {
            bool result = false;

            switch (actualStatus)
            {
                case InspectionStatus.FlowCompleted:
                case InspectionStatus.ResultApproved:
                case InspectionStatus.ResultDenied:
                case InspectionStatus.Canceled:
                case InspectionStatus.Rescheduled:
                case InspectionStatus.Unknown:
                    result = true;
                    break;
            }

            return result;
        }

        /// <summary>
        /// Determines whether current result status is user-defined.
        /// </summary>
        /// <param name="resultStatus">The result status.</param>
        /// <returns>
        /// <c>true</c> if current result status is user-defined; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsUserDefinedResultStatus(string resultStatus)
        {
            bool result = !INSP_STATUS_RESULT_APPROVED.Equals(resultStatus, StringComparison.InvariantCultureIgnoreCase) 
                          && !INSP_STATUS_RESULT_DENIED.Equals(resultStatus, StringComparison.InvariantCultureIgnoreCase) 
                          && !INSP_STATUS_RESULT_PENDING.Equals(resultStatus, StringComparison.InvariantCultureIgnoreCase);

            return result;
        }

        /// <summary>
        /// Gets inspection result type.
        /// </summary>
        /// <param name="inspectionStatus">The inspection status.</param>
        /// <param name="inspectionTypeModel">The inspection type model.</param>
        /// <returns>inspection result type</returns>
        public static string GetResultType(string inspectionStatus, InspectionTypeModel inspectionTypeModel)
        {
            //get result type
            string resultType = inspectionStatus;

            if (!string.IsNullOrEmpty(inspectionStatus) && inspectionTypeModel != null && inspectionTypeModel.resultGroup != null &&
                inspectionTypeModel.resultGroup.inspectionResultModels != null)
            {
                foreach (InspectionResultModel resultModel in inspectionTypeModel.resultGroup.inspectionResultModels)
                {
                    if (inspectionStatus.Equals(resultModel.resultValue, StringComparison.InvariantCultureIgnoreCase))
                    {
                        resultType = string.IsNullOrEmpty(resultModel.resultType) ? string.Empty : resultModel.resultType;
                        break;
                    }
                }
            }

            return resultType;
        }
        #endregion Methods
    }
}
