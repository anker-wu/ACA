#region Header

/**
 * <pre>
 * 
 *  Accela Citizen Access
 *  File: ICalendarBll.cs
 * 
 *  Accela, Inc.
 *  Copyright (C): 2008-2014
 * 
 *  Description:
 * 
 *  Notes:
 * $Id: ICalendarBll.cs 277008 2014-08-09 08:37:50Z ACHIEVO\james.shi $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 *   10/12/2007      Daly zeng
 * </pre>
 */

#endregion Header

using System;

using Accela.ACA.WSProxy;

namespace Accela.ACA.BLL.Inspection
{
    /// <summary>
    /// Defines method for Calendar
    /// </summary>
    public interface ICalendarBll
    {
        #region Methods

        /// <summary>
        /// Finds out a certain calendar based on agency code, inspection type
        /// </summary>
        /// <param name="serviceProviderCode">agency code</param>
        /// <param name="inspSeqNbr">The sequence number of inspection type</param>
        /// <returns>The calendar model</returns>
        CalendarModel GetACACalendarByInspType(string serviceProviderCode, string inspSeqNbr);

        /// <summary>
        /// Gets the next working days and times.
        /// </summary>
        /// <param name="servProvCode">The agency code.</param>
        /// <param name="fromDate">From date.</param>
        /// <param name="endDate">The end date.</param>
        /// <param name="capID">The cap ID.</param>
        /// <param name="inspectionType">Type of the inspection.</param>
        /// <returns>the working days and times.</returns>
        DateTimeRangePageModel[] GetNextWorkingDaysAndTimes(string servProvCode, DateTime? fromDate, DateTime? endDate, CapIDModel capID, InspectionTypeModel inspectionType);

        /// <summary>
        /// Gets the same day.
        /// </summary>
        /// <param name="servProvCode">The agency code.</param>
        /// <param name="capID">The cap ID.</param>
        /// <param name="inspectionType">Type of the inspection.</param>
        /// <returns>the same day.</returns>
        DateTimeRangePageModel GetSameDay(string servProvCode, CapIDModel capID, InspectionTypeModel inspectionType);

        /// <summary>
        /// Gets the next business day.
        /// </summary>
        /// <param name="servProvCode">The agency code.</param>
        /// <param name="capID">The cap ID.</param>
        /// <param name="inspectionType">Type of the inspection.</param>
        /// <returns>the next business day.</returns>
        DateTimeRangePageModel GetNextBusinessDay(string servProvCode, CapIDModel capID, InspectionTypeModel inspectionType);

        #endregion Methods
    }
}