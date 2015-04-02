#region Header

/**
 *  Accela Citizen Access
 *  File: Range.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2011-2014
 *
 *  Description:
 *   Provide object range class, for example: datetime range.
 *
 *  Notes:
 * $Id: Range.cs 205254 2011-10-11 05:15:27Z ACHIEVO\daly.zeng $.
 *  Revision History
 *  Date,                  Who,                 What
 */

#endregion Header

using Accela.ACA.Common.Util;

namespace Accela.ACA.Common.Common
{
    /// <summary>
    /// Provide object range class.
    /// </summary>
    /// <typeparam name="T">The type that support range, for example datetime.</typeparam>
    public class Range<T>
    {
        #region Properties

        /// <summary>
        /// Gets or sets the single value
        /// </summary>
        public T SingleValue
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the lower bound.
        /// </summary>
        /// <value>The lower bound.</value>
        public T LowerBound
        {
            get;

            set;
        }

        /// <summary>
        /// Gets or sets the upper bound.
        /// </summary>
        /// <value>The upper bound.</value>
        public T UpperBound
        {
            get;

            set;
        }

        /// <summary>
        /// Get the range values and return the validate result.
        /// </summary>
        /// <param name="lowerBound">The lower bound</param>
        /// <param name="upperBound">The upper bound</param>
        /// <returns>Return the range values.</returns>
        public static Range<int?> GetRangeValue(string lowerBound, string upperBound)
        {
            var result = new Range<int?>();

            if (string.IsNullOrEmpty(lowerBound) || string.IsNullOrEmpty(upperBound))
            {
                string singleValue = !string.IsNullOrEmpty(lowerBound) ? lowerBound : upperBound;
                result.SingleValue = !string.IsNullOrEmpty(singleValue) ? StringUtil.ToInt(singleValue.Trim()) : null;

                if (!string.IsNullOrEmpty(lowerBound))
                {
                    result.LowerBound = StringUtil.ToInt(lowerBound.Trim());
                }

                if (!string.IsNullOrEmpty(upperBound))
                {
                    result.UpperBound = StringUtil.ToInt(upperBound.Trim());
                }
            }
            else
            {
                result.LowerBound = StringUtil.ToInt(lowerBound.Trim());
                result.UpperBound = StringUtil.ToInt(upperBound.Trim());
            }

            return result;
        }

        #endregion Properties
    }
}
