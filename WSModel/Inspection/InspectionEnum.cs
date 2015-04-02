#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: InspectionStatus.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2010-2011
 *
 *  Description:
 *
 *  Notes:
 *      $Id: InspectionEnum.cs 204256 2011-09-23 06:56:58Z ACHIEVO\alan.hu $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Accela.ACA.Inspection
{
    /// <summary>
    /// Inspection status
    /// </summary>
    public enum InspectionStatus
    {
        /// <summary>
        /// Un-known = 0.
        /// </summary>
        Unknown = 0,

        /// <summary>
        /// Flow prerequisite not met
        /// </summary>
        FlowPrerequisiteNotMet,

        /// <summary>
        /// Flow completed
        /// </summary>
        FlowCompleted,

        /// <summary>
        /// Flow completed but active
        /// </summary>
        FlowCompletedButActive,

        /// <summary>
        /// Initial required
        /// </summary>
        InitialRequired,

        /// <summary>
        /// Initial optional
        /// </summary>
        InitialOptional,

        /// <summary>
        /// Pending by V360
        /// </summary>
        PendingByV360,

        /// <summary>
        /// Pending byACA
        /// </summary>
        PendingByACA,

        /// <summary>
        /// Scheduled constant.
        /// </summary>
        Scheduled,

        /// <summary>
        /// Cancelled constant.
        /// </summary>
        Canceled,

        /// <summary>
        /// Re-scheduled constant.
        /// </summary>
        Rescheduled,

        /// <summary>
        /// Result Pending
        /// </summary>
        ResultPending,

        /// <summary>
        /// Result denied.
        /// </summary>
        ResultDenied,

        /// <summary>
        /// Result approved.
        /// </summary>
        ResultApproved
    }

    /// <summary>
    /// Inspection action.
    /// </summary>
    public enum InspectionAction
    {
        /// <summary>
        /// None = 0 constant.
        /// </summary>
        None = 0,

        /// <summary>
        /// View inspection details
        /// </summary>
        ViewDetails,

        /// <summary>
        /// Do prerequisite not met
        /// </summary>
        DoPrerequisiteNotMet,

        /// <summary>
        /// Request constant.
        /// </summary>
        Request,

        /// <summary>
        /// Schedule constant.
        /// </summary>
        Schedule,

        /// <summary>
        /// Re-schedule constant.
        /// </summary>
        Reschedule,

        /// <summary>
        /// Restricted reschedule
        /// </summary>
        RestrictedReschedule,

        /// <summary>
        /// Cancel constant.
        /// </summary>
        Cancel,

        /// <summary>
        /// Restricted cancel
        /// </summary>
        RestrictedCancel,

        /// <summary>
        /// Do complete constant
        /// </summary>
        DoComplete,

        /// <summary>
        /// Print inspection details
        /// </summary>
        PrintDetails
    }

    /// <summary>
    /// Scheduling schedule type.
    /// </summary>
    public enum InspectionScheduleType
    {
        /// <summary>
        /// Unknown = 0,
        /// </summary>
        Unknown = 0,

        /// <summary>
        /// None (no setting type)
        /// </summary>
        None,

        /// <summary>
        /// Schedule using calendar
        /// </summary>
        ScheduleUsingCalendar,

        /// <summary>
        /// Request only pending
        /// </summary>
        RequestOnlyPending,

        /// <summary>
        /// Request same day next day.
        /// </summary>
        RequestSameDayNextDay
    }

    /// <summary>
    /// Inspection request day option
    /// </summary>
    public enum InspectionRequestDayOption
    {
        /// <summary>
        /// SameDay = 1,
        /// </summary>
        SameDay = 1,

        /// <summary>
        /// Next business day
        /// </summary>
        NextBusinessDay,

        /// <summary>
        /// Next available day
        /// </summary>
        NextAvailableDay
    }

    /// <summary>
    /// Inspection contact change option
    /// </summary>
    public enum InspectionContactChangeOption
    {
        /// <summary>
        /// Unknown option
        /// </summary>
        Unknown,

        /// <summary>
        /// Select an exist contact
        /// </summary>
        Select,

        /// <summary>
        /// specify an new contact
        /// </summary>
        Specify
    }
    
    /// <summary>
    /// Inspection time option
    /// </summary>
    public enum InspectionTimeOption
    {
        /// <summary>
        /// indicates unknow time option
        /// </summary>
        Unknow,

        /// <summary>
        /// indicates the specific time
        /// </summary>
        SpecificTime,

        /// <summary>
        /// indicates the morning
        /// </summary>
        AM,

        /// <summary>
        /// indicates the afternoon
        /// </summary>
        PM,

        /// <summary>
        /// indicates all day
        /// </summary>
        AllDay
    }
}
