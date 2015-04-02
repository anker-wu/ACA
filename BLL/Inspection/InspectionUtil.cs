#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: InspectionUtil.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2011-2014
 *
 *  Description:
 *
 *  Notes:
 * $Id: InspectionUtil.cs 266177 2014-02-19 11:13:56Z ACHIEVO\james.shi $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using System;

using Accela.ACA.Common.Util;
using Accela.ACA.Inspection;
using Accela.ACA.WSProxy;

namespace Accela.ACA.BLL.Inspection
{
    /// <summary>
    /// Inspection utility
    /// </summary>
    public class InspectionUtil
    {
        /// <summary>
        /// Gets the activity date.
        /// </summary>
        /// <param name="inspectionModel">The inspection model.</param>
        /// <param name="timeOption">The time option.</param>
        /// <returns>the activity date with timeOption output.</returns>
        public static DateTime? GetActivityDate(InspectionModel inspectionModel, out InspectionTimeOption timeOption)
        {
            DateTime? result = null;
            timeOption = InspectionTimeOption.Unknow;

            if (inspectionModel != null && inspectionModel.activity != null && inspectionModel.activity.activityDate.HasValue)
            {
                DateTime? date = inspectionModel.activity.activityDate;
                string time = inspectionModel.activity.time2;
                string ampm = inspectionModel.activity.time1;

                result = I18nDateTimeUtil.GetInspectionDate(date, time, ampm, out timeOption);
            }

            return result;
        }
    }
}
