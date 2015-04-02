#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: InspectionUtil.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2010-2014
 *
 *  Description:
 *
 *  Notes:
 * $Id: InspectionScheduleTypeUtil.cs 277347 2014-08-14 06:51:12Z ACHIEVO\james.shi $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using Accela.ACA.Inspection;
using Accela.ACA.WSProxy;

namespace Accela.ACA.BLL.Inspection
{
    /// <summary>
    /// inspection utility
    /// </summary>
    internal static class InspectionScheduleTypeUtil
    {
        #region Fields

        /// <summary>
        /// schedule type none 
        /// </summary>
        private const string SCHEDULE_TYPE_NONE = "NONE";

        /// <summary>
        /// schedule type request only pending 
        /// </summary>
        private const string SCHEDULE_TYPE_REQUEST_ONLY_PENDING = "REQUEST_ONLY_PENDING";

        /// <summary>
        ///  schedule type request same day next day 
        /// </summary>
        private const string SCHEDULE_TYPE_REQUEST_SAME_DAY_NEXT_DAY = "REQUEST_SAME_DAY_NEXT_DAY";

        /// <summary>
        /// schedule type schedule using calendar 
        /// </summary>
        private const string SCHEDULE_TYPE_SCHEDULE_USING_CALENDAR = "SCHEDULE_USING_CALENDAR";

        #endregion Fields

        #region Methods

        /// <summary>
        /// Gets InspectionScheduleType object from the specified schedule type key(string).
        /// </summary>
        /// <param name="scheduleTypeKey">The schedule type key.</param>
        /// <returns>InspectionScheduleType object</returns>
        public static InspectionScheduleType GetScheduleType(string scheduleTypeKey)
        {
            InspectionScheduleType scheduleType = InspectionScheduleType.Unknown;

            switch (scheduleTypeKey)
            {
                case SCHEDULE_TYPE_NONE:
                    scheduleType = InspectionScheduleType.None;
                    break;
                case SCHEDULE_TYPE_REQUEST_ONLY_PENDING:
                    scheduleType = InspectionScheduleType.RequestOnlyPending;
                    break;
                case SCHEDULE_TYPE_REQUEST_SAME_DAY_NEXT_DAY:
                    scheduleType = InspectionScheduleType.RequestSameDayNextDay;
                    break;
                case SCHEDULE_TYPE_SCHEDULE_USING_CALENDAR:
                default:
                    scheduleType = InspectionScheduleType.ScheduleUsingCalendar;
                    break;
            }

            return scheduleType;
        }

        /// <summary>
        /// Gets InspectionScheduleType object from the specified inspection type model.
        /// </summary>
        /// <param name="inspectionTypeModel">The inspection type model.</param>
        /// <returns>InspectionScheduleType object</returns>
        public static InspectionScheduleType GetScheduleType(InspectionTypeModel inspectionTypeModel)
        {
            InspectionScheduleType scheduleType = InspectionScheduleType.Unknown;
            if (inspectionTypeModel != null &&
                inspectionTypeModel.calendarInspectionType != null)
            {
                scheduleType = GetScheduleType(inspectionTypeModel.calendarInspectionType.scheduleInspectionInACA);
            }

            return scheduleType;
        }

        #endregion Methods
    }
}
