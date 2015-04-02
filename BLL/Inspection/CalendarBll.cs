#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: CalendarBll.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2008-2014
 *
 *  Description:
 *
 *  Notes:
 * $Id: CalendarBll.cs 277347 2014-08-14 06:51:12Z ACHIEVO\james.shi $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 *  05/14/2007    troy.yang    Initial version.
 * </pre>
 */

#endregion Header

using System;
using System.Collections;
using System.Collections.Generic;

using Accela.ACA.BLL.Common;
using Accela.ACA.Common;
using Accela.ACA.Common.Log;
using Accela.ACA.Common.Util;
using Accela.ACA.WSProxy;

using log4net;

namespace Accela.ACA.BLL.Inspection
{
    /// <summary>
    /// This class provide the ability to operation calendar.
    /// </summary>
    public class CalendarBll : BaseBll, ICalendarBll
    {
        #region Fields

        #endregion Fields

        #region Properties

        /// <summary>
        /// Gets an instance of CalendarService.
        /// </summary>
        private CalendarWebServiceService CalendarService
        {
            get
            {
                return WSFactory.Instance.GetWebService<CalendarWebServiceService>();
            }
        }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Finds out a certain calendar based on agency code, inspection type
        /// </summary>
        /// <param name="serviceProviderCode">agency code</param>
        /// <param name="inspSeqNbr">The sequence number of inspection type</param>
        /// <returns>The calendar model</returns>
        public CalendarModel GetACACalendarByInspType(string serviceProviderCode, string inspSeqNbr)
        {
            if (string.IsNullOrEmpty(serviceProviderCode) || string.IsNullOrEmpty(inspSeqNbr))
            {
                throw new DataValidateException(new string[] { "serviceProviderCode", "calendarID" });
            }

            try
            {
                CalendarModel calendar = CalendarService.getACACalendarByInspType(serviceProviderCode, inspSeqNbr);

                return calendar;
            }
            catch (Exception e)
            {
                throw new ACAException(e);
            }
        }

        /// <summary>
        /// Gets the next working days and times.
        /// </summary>
        /// <param name="servProvCode">The agency code.</param>
        /// <param name="fromDate">From date.</param>
        /// <param name="endDate">The end date.</param>
        /// <param name="capID">The cap ID.</param>
        /// <param name="inspectionType">Type of the inspection.</param>
        /// <returns>the working days and times.</returns>
        /// <exception cref="DataValidateException">{ <c>servProvCode, capIDModel, inspectionType</c> } is null</exception>
        /// <exception cref="ACAException">Exception from web service.</exception>
        public DateTimeRangePageModel[] GetNextWorkingDaysAndTimes(string servProvCode, DateTime? fromDate, DateTime? endDate, CapIDModel capID, InspectionTypeModel inspectionType)
        {
            if (string.IsNullOrEmpty(servProvCode) || capID == null || inspectionType == null)
            {
                throw new DataValidateException(new string[] { "servProvCode", "capIDModel", "inspectionType" });
            }

            try
            {
                var fromDateValue = fromDate == null ? DateTime.MinValue : fromDate.Value;
                var fromDateSpecified = fromDate != null;
                var endDateValue = endDate == null ? DateTime.MaxValue : endDate.Value;
                var endDateSpecified = endDate != null;
                var workingDaysAndTimes = CalendarService.getNextWorkingDaysAndTimes(servProvCode, fromDateValue, fromDateSpecified, endDateValue, endDateSpecified, capID, inspectionType, User.PublicUserId);

                return workingDaysAndTimes;
            }
            catch (Exception e)
            {
                throw new ACAException(e);
            }
        }

        /// <summary>
        /// Gets the same day.
        /// </summary>
        /// <param name="servProvCode">The agency code.</param>
        /// <param name="capID">The cap ID.</param>
        /// <param name="inspectionType">Type of the inspection.</param>
        /// <returns>the same day.</returns>
        /// <exception cref="DataValidateException">{ <c>servProvCode, capIDModel, inspectionType</c> } is null</exception>
        /// <exception cref="ACAException">Exception from web service.</exception>
        public DateTimeRangePageModel GetSameDay(string servProvCode, CapIDModel capID, InspectionTypeModel inspectionType)
        {
            if (string.IsNullOrEmpty(servProvCode) || capID == null || inspectionType == null)
            {
                throw new DataValidateException(new string[] { "servProvCode", "capIDModel", "inspectionType" });
            }

            try
            {
                var workingDayAndTimes = CalendarService.getSameDay(servProvCode, capID, inspectionType, User.PublicUserId);

                return workingDayAndTimes;
            }
            catch (Exception e)
            {
                throw new ACAException(e);
            }
        }

        /// <summary>
        /// Gets the next business day.
        /// </summary>
        /// <param name="servProvCode">The agency code.</param>
        /// <param name="capID">The cap ID.</param>
        /// <param name="inspectionType">Type of the inspection.</param>
        /// <returns>the next business day.</returns>
        /// <exception cref="DataValidateException">{ <c>servProvCode, capIDModel, inspectionType</c> } is null</exception>
        /// <exception cref="ACAException">Exception from web service.</exception>
        public DateTimeRangePageModel GetNextBusinessDay(string servProvCode, CapIDModel capID, InspectionTypeModel inspectionType)
        {
            if (string.IsNullOrEmpty(servProvCode) || capID == null || inspectionType == null)
            {
                throw new DataValidateException(new string[] { "servProvCode", "capIDModel", "inspectionType" });
            }

            try
            {
                var workingDayAndTimes = CalendarService.getNextDay(servProvCode, capID, inspectionType, User.PublicUserId);

                return workingDayAndTimes;
            }
            catch (Exception e)
            {
                throw new ACAException(e);
            }
        }

        #endregion Methods
    }
}