#region Header

/**
 *  Accela Citizen Access
 *  File: SearchResultUtil.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2012-2014
 *
 *  Description:
 *   It is for Search resut util
 *
 *  Notes:
 * $Id: SearchResultUtil.cs 207483 2011-11-15 08:43:21Z ACHIEVO\daniel.shi $.
 *  Revision History
 *  Date,                  Who,                 What
 */

#endregion Header

namespace Accela.ACA.Common.Util
{
    /// <summary>
    /// Search Result Utility
    /// </summary>
    public class SearchResultUtil
    {
        /// <summary>
        /// Generate the summary information for searched out records.
        /// For example: 1-10 of 100+
        /// </summary>
        /// <param name="totalRowsCount">The total rows count.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <param name="pageIndex">Index of the page.</param>
        /// <returns>The record summary.</returns>
        public static string GenerateRecordsSummary(int totalRowsCount, int pageSize, int pageIndex)
        {
            string countSummary = string.Empty;

            if ((totalRowsCount % (pageSize * ACAConstant.DEFAULT_PAGECOUNT)
                 == ACAConstant.ADDITIONAL_RECORDS_COUNT) && (pageIndex + 1) * pageSize < totalRowsCount)
            {
                countSummary = totalRowsCount - ACAConstant.ADDITIONAL_RECORDS_COUNT + "+";
            }
            else
            {
                countSummary = totalRowsCount.ToString();
            }

            return countSummary;
        }
    }
}
