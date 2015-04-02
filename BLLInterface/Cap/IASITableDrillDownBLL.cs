#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: IASITableDrillDownBLL.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2008-2014
 *
 *  Description:
 *
 *  Notes:
 * $Id: IASITableDrillDownBLL.cs 276976 2014-08-08 10:10:01Z ACHIEVO\james.shi $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using System;

using Accela.ACA.WSProxy;

namespace Accela.ACA.BLL.Cap
{
    /// <summary>
    /// Defines methods for ASI Table Drill Down. 
    /// </summary>
    public partial interface IASITableDrillDownBLL
    {
        #region Methods

        /// <summary>
        /// Get current language values base main language values
        /// </summary>
        /// <param name="agencyCode">Agency code</param>
        /// <param name="listValues">main language values</param>
        /// <returns>current language values</returns>
        string[] GetFieldTexts(string agencyCode, string[] listValues);

        /// <summary>
        /// Get first ASIT drill down data.
        /// </summary>
        /// <param name="agencyCode">Agency code</param>
        /// <param name="groupName">group name</param>
        /// <param name="subGroupName">sub Group Name</param>
        /// <returns>first ASIT drill down data</returns>
        ASITableDrillDSeriesModel4WS[] GetFirstASITableDrillDownDatas(string agencyCode, string groupName, string subGroupName);

        /// <summary>
        /// Get main language values base on Standard Choices ID collections
        /// </summary>
        /// <param name="agencyCode">Agency code</param>
        /// <param name="valueIds">Standard Choices ID collections</param>
        /// <returns>main language values</returns>
        string[] GetMainLanguageValues(string agencyCode, string[] valueIds);

        /// <summary>
        /// Get next ASIT drill down data.
        /// </summary>
        /// <param name="agencyCode">Agency code</param>
        /// <param name="listValueIds">Id value list</param>
        /// <param name="seriesId">seriesId value.</param>
        /// <returns>Next ASIT Drill Down Data List.</returns>
        ASITableDrillDSeriesModel4WS[] GetNextASITableDrillDownDatas(string agencyCode, string[] listValueIds, long seriesId);

        /// <summary>
        ///  Search value from last column content of drill down data table.
        /// </summary>
        /// <param name="agencyCode">Agency code</param>
        /// <param name="groupName">ASI table group name</param>
        /// <param name="subGroupName">ASI table sub group name that is ASI table name</param>
        /// <param name="searchValue">searching content</param>
        /// <returns> 
        /// The search result array data format as below chars      
        /// *a[0][0]=column name, e.g. : ColumnName1
        /// *a[1][0]=languageID : columnValue, e.g. : 1:country
        /// </returns>
        string[][] LoadSearchResultBySearchValue(string agencyCode, string groupName, string subGroupName, string searchValue);

        #endregion Methods
    }
}
