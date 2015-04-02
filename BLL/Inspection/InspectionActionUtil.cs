#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: InspectionActionBll.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2010-2014
 *
 *  Description:
 *
 *  Notes:
 *      $Id: InspectionActionUtil.cs 277347 2014-08-14 06:51:12Z ACHIEVO\james.shi $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using Accela.ACA.Common;
using Accela.ACA.Inspection;
using Accela.ACA.WSProxy;

namespace Accela.ACA.BLL.Inspection
{
    /// <summary>
    /// This class provides the ability to inspection action.
    /// </summary>
    internal static class InspectionActionUtil
    {
        #region Fields

        /// <summary>
        /// The action mode of request for Pending
        /// </summary>
        private const string ACT_MODE_REQUEST = "Pending";

        /// <summary>
        /// The action mode of reschedule
        /// </summary>
        private const string ACT_MODE_RESCHEDULE = "Reschedule";

        /// <summary>
        /// The action mode for schedule
        /// </summary>
        private const string ACT_MODE_SCHEDULE = "Schedule";

        #endregion Fields

        #region Properties

        #endregion Properties

        #region Methods

        /// <summary>
        /// get inspection action ENUM
        /// </summary>
        /// <param name="agencyCode">The agency code.</param>
        /// <param name="moduleName">Name of the module.</param>
        /// <param name="recordStatusGroupCode">The record status group code.</param>
        /// <param name="recordStatus">The record status.</param>
        /// <param name="inspectionTypeModel">the instance of InspectionTypeModel</param>
        /// <returns>the InspectionAction type</returns>
        public static InspectionAction GetAction(string agencyCode, string moduleName, string recordStatusGroupCode, string recordStatus, InspectionTypeModel inspectionTypeModel)
        {
            return GetAction(agencyCode, moduleName, recordStatusGroupCode, recordStatus, inspectionTypeModel, null);
        }

        /// <summary>
        /// get inspection action ENUM
        /// </summary>
        /// <param name="agencyCode">The agency code.</param>
        /// <param name="moduleName">Name of the module.</param>
        /// <param name="recordStatusGroupCode">The record status group code.</param>
        /// <param name="recordStatus">The record status.</param>
        /// <param name="inspectionTypeModel">the instance of InspectionTypeModel</param>
        /// <param name="inspectionModel">the instance of InspectionModel</param>
        /// <returns>the InspectionAction type</returns>
        public static InspectionAction GetAction(string agencyCode, string moduleName, string recordStatusGroupCode, string recordStatus, InspectionTypeModel inspectionTypeModel, InspectionModel inspectionModel)
        {
            bool allowMultipleInspections = InspectionPermissionUtil.AllowMultipleInspections(moduleName, inspectionTypeModel.requiredInspection);

            //get schedule type.
            InspectionScheduleType scheduleType = InspectionScheduleTypeUtil.GetScheduleType(inspectionTypeModel);

            //get inspection actual status.
            InspectionStatus actualStatus = InspectionStatusUtil.GetStatus(inspectionTypeModel, inspectionModel);

            //get action
            InspectionAction action = GetAction(actualStatus, scheduleType);

            if (inspectionTypeModel != null && !inspectionTypeModel.isConfiguredInInspFlow && action == InspectionAction.DoComplete)
            {
                action = InspectionAction.None;
            }

            if (inspectionModel != null && allowMultipleInspections && InspectionStatusUtil.IsEndStatus(actualStatus))
            {
                action = InspectionAction.None;
            }

            //if can't be rescheduled, then set None to action.
            if (action == InspectionAction.Reschedule)
            {
                if (!InspectionPermissionUtil.CanBeRescheduled(inspectionTypeModel))
                {
                    action = InspectionAction.None;
                }
                else if (InspectionPermissionUtil.IsOutOfTimeToReschedule(agencyCode, inspectionTypeModel, inspectionModel))
                {
                    action = InspectionAction.RestrictedReschedule;
                }
            }

            return action;
        }

        /// <summary>
        /// get cancel action
        /// </summary>
        /// <param name="agencyCode">The agency code.</param>
        /// <param name="moduleName">Name of the module.</param>
        /// <param name="recordStatusGroupCode">The record status group code.</param>
        /// <param name="recordStatus">The record status.</param>
        /// <param name="inspectionTypeModel">the instance of InspectionTypeModel</param>
        /// <returns>the InspectionAction type</returns>
        public static InspectionAction GetCancelAction(string agencyCode, string moduleName, string recordStatusGroupCode, string recordStatus, InspectionTypeModel inspectionTypeModel)
        {
            return GetCancelAction(agencyCode, moduleName, recordStatusGroupCode, recordStatus, inspectionTypeModel, null);
        }

        /// <summary>
        /// get cancel action
        /// </summary>
        /// <param name="agencyCode">The agency code.</param>
        /// <param name="moduleName">Name of the module.</param>
        /// <param name="recordStatusGroupCode">The record status group code.</param>
        /// <param name="recordStatus">The record status.</param>
        /// <param name="inspectionTypeModel">the instance of InspectionTypeModel</param>
        /// <param name="inspectionModel">the instance of InspectionModel</param>
        /// <returns>the InspectionAction type</returns>
        public static InspectionAction GetCancelAction(string agencyCode, string moduleName, string recordStatusGroupCode, string recordStatus, InspectionTypeModel inspectionTypeModel, InspectionModel inspectionModel)
        {
            //get inspection actual status.
            InspectionStatus actualStatus = InspectionStatusUtil.GetStatus(inspectionTypeModel, inspectionModel);

            //check to see if inspection schedule can be cancelled.
            bool canBeCancelled = InspectionPermissionUtil.CanBeCancelled(inspectionTypeModel);

            //get cancel action
            InspectionAction action = GetCancelAction(actualStatus, canBeCancelled);

            //check to see if cancel is out of time
            if ((InspectionStatus.Scheduled == actualStatus || InspectionStatus.ResultPending == actualStatus)
                && InspectionAction.Cancel == action
                && InspectionPermissionUtil.IsOutOfTimeToCancel(agencyCode, inspectionTypeModel, inspectionModel))
            {
                action = InspectionAction.RestrictedCancel;
            }

            return action;
        }

        /// <summary>
        /// Get action mode.
        /// </summary>
        /// <param name="action">the type of InspectionAction</param>
        /// <returns>base on the InspectionAction</returns>
        public static string GetActionMode(InspectionAction action)
        {
            string result = string.Empty;

            if (InspectionPermissionUtil.CanBeOperated(action))
            {
                switch (action)
                {
                    case InspectionAction.Request:
                        result = ACT_MODE_REQUEST;
                        break;
                    case InspectionAction.Schedule:
                        result = ACT_MODE_SCHEDULE;
                        break;
                    case InspectionAction.Reschedule:
                        result = ACT_MODE_RESCHEDULE;
                        break;
                    case InspectionAction.DoPrerequisiteNotMet:
                    case InspectionAction.Cancel:
                    case InspectionAction.DoComplete:
                    case InspectionAction.None:
                    default:
                        result = string.Empty;
                        break;
                }
            }

            return result;
        }

        /// <summary>
        /// get inspection action ENUM
        /// </summary>
        /// <param name="actualStatus">the InspectionStatus type</param>
        /// <param name="scheduleType">the instance of InspectionScheduleType</param>
        /// <returns>the InspectionAction type</returns>
        private static InspectionAction GetAction(InspectionStatus actualStatus, InspectionScheduleType scheduleType)
        {
            InspectionAction result = InspectionAction.None;

            if (scheduleType == InspectionScheduleType.Unknown)
            {
                return result;
            }

            switch (actualStatus)
            {
                case InspectionStatus.FlowPrerequisiteNotMet:
                    switch (scheduleType)
                    {
                        case InspectionScheduleType.RequestOnlyPending:
                        case InspectionScheduleType.RequestSameDayNextDay:
                            result = InspectionAction.Request;
                            break;
                        case InspectionScheduleType.None:
                        case InspectionScheduleType.ScheduleUsingCalendar:
                        default:
                            result = InspectionAction.DoPrerequisiteNotMet;
                            break;
                    }

                    break;

                case InspectionStatus.InitialRequired:
                case InspectionStatus.InitialOptional:
                case InspectionStatus.PendingByV360:
                case InspectionStatus.FlowCompletedButActive:
                    switch (scheduleType)
                    {
                        case InspectionScheduleType.None:
                            result = InspectionAction.None;
                            break;
                        case InspectionScheduleType.RequestOnlyPending:
                        case InspectionScheduleType.RequestSameDayNextDay:
                            result = InspectionAction.Request;
                            break;
                        default:
                            result = InspectionAction.Schedule;
                            break;
                    }

                    break;

                case InspectionStatus.PendingByACA:
                    switch (scheduleType)
                    {
                        case InspectionScheduleType.None:
                        case InspectionScheduleType.RequestOnlyPending:
                        case InspectionScheduleType.RequestSameDayNextDay:
                            result = InspectionAction.None;
                            break;
                        default:
                            result = InspectionAction.Schedule;
                            break;
                    }

                    break;

                case InspectionStatus.Scheduled:
                    switch (scheduleType)
                    {
                        case InspectionScheduleType.None:
                        case InspectionScheduleType.RequestOnlyPending:
                        case InspectionScheduleType.RequestSameDayNextDay:
                            result = InspectionAction.None;
                            break;
                        default:
                            result = InspectionAction.Reschedule;
                            break;
                    }

                    break;

                case InspectionStatus.ResultApproved:
                    result = InspectionAction.DoComplete;
                    break;
                case InspectionStatus.Rescheduled:
                case InspectionStatus.Unknown:
                case InspectionStatus.Canceled:
                case InspectionStatus.ResultPending:
                case InspectionStatus.ResultDenied:
                case InspectionStatus.FlowCompleted:
                default:
                    result = InspectionAction.None;
                    break;
            }

            return result;
        }

        /// <summary>
        /// get cancel action
        /// </summary>
        /// <param name="actualStatus">the InspectionStatus type</param>
        /// <param name="canBeCancelled">can be cancelled?</param>
        /// <returns>the InspectionAction type</returns>
        private static InspectionAction GetCancelAction(InspectionStatus actualStatus, bool canBeCancelled)
        {
            InspectionAction result = InspectionAction.None;

            if (canBeCancelled)
            {
                switch (actualStatus)
                {
                    case InspectionStatus.PendingByACA:
                    case InspectionStatus.Scheduled:
                        result = InspectionAction.Cancel;
                        break;
                }
            }

            return result;
        }

        #endregion Methods
    }
}
