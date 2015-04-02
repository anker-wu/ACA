#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: ContinuingEducationSummary.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2009-2013
 *
 *  Description:
 *
 *  Notes:
 * $Id: ContinuingEducationSummary.cs 144261 2009-08-26 10:23:37Z ACHIEVO\jackie.yu $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using System;

namespace Accela.ACA.WSProxy.Education
{
    /// <summary>
    /// This object class for continuing education summary.
    /// </summary>
    [Serializable]
    public class ContinuingEducationSummary
    {
        /// <summary>
        /// Gets or sets ContinuingEducationSummary.EducationName
        /// </summary>
        public string EducaitonName
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets ContinuingEducationSummary.HoursRequired
        /// </summary>
        public double RequiredHours
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets ContinuingEducationSummary.HoursCompleted
        /// </summary>
        public double CompletedHours
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is require Continuing Education.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is require Continuing Education; otherwise, <c>false</c>.
        /// </value>
        public bool IsRequireCE
        {
            get;
            set;
        }
        
        /// <summary>
        /// Gets ContinuingEducationSummary.HoursBalance.
        /// </summary>
        public double BalanceHours
        {
            get
            {
                double balanceHours = RequiredHours - CompletedHours;

                return balanceHours > 0 ? Math.Round(balanceHours,2) : 0;
            }
        }
    }
}
