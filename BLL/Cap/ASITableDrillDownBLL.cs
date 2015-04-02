#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: ASITableDrillDownBLL.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2008-2014
 *
 *  Description:
 *
 *  Notes:
 * $Id: ASITableDrillDownBLL.cs 277178 2014-08-12 08:11:48Z ACHIEVO\james.shi $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using System;
using System.Globalization;

using Accela.ACA.Common;
using Accela.ACA.WSProxy;
using Accela.ACA.WSProxy.WSModel;

namespace Accela.ACA.BLL.Cap
{
    /// <summary>
    /// This class provide the ability to operation asiTableDrillDown.
    /// </summary>
    public sealed class ASITableDrillDownBLL : BLL.BaseBll, IASITableDrillDownBLL
    {
        #region Properties

        /// <summary>
        /// Gets an instance of ASITableDrillDownService.
        /// </summary>
        private ASITableDrillDownWebServiceService ASITableDrillDownService
        {
            get
            {
                return WSFactory.Instance.GetWebService<ASITableDrillDownWebServiceService>();
            }
        }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Get current language values base main language values
        /// </summary>
        /// <param name="agencyCode">Agency code</param>
        /// <param name="listValues">main language values</param>
        /// <returns>current language values</returns>
        string[] IASITableDrillDownBLL.GetFieldTexts(string agencyCode, string[] listValues)
        {
            return ASITableDrillDownService.getFieldTexts(agencyCode, listValues);
        }

        /// <summary>
        /// Get first ASIT drill down data.
        /// </summary>
        /// <param name="agencyCode">Agency code</param>
        /// <param name="groupName">group name</param>
        /// <param name="subGroupName">sub Group Name</param>
        /// <returns>first ASIT drill down data</returns>
        ASITableDrillDSeriesModel4WS[] IASITableDrillDownBLL.GetFirstASITableDrillDownDatas(string agencyCode, string groupName, string subGroupName)
        {
            return ASITableDrillDownService.getFirstASITableDrillDownDatas(agencyCode, groupName, subGroupName, "ACA");
        }

        /// <summary>
        /// Get main language values base on Standard Choices ID collections
        /// </summary>
        /// <param name="agencyCode">Agency code</param>
        /// <param name="valueIds">Standard Choices ID collections</param>
        /// <returns>main language values</returns>
        string[] IASITableDrillDownBLL.GetMainLanguageValues(string agencyCode, string[] valueIds)
        {
            long[] passValueIDs = new long[valueIds.Length];

            for (int i = 0; i < valueIds.Length; i++)
            {
                passValueIDs[i] = Convert.ToInt64(valueIds[i], CultureInfo.InvariantCulture);
            }

            return ASITableDrillDownService.getMainLanguageValues(agencyCode, passValueIDs);
        }

        /// <summary>
        /// Get next ASIT drill down data.
        /// </summary>
        /// <param name="agencyCode">Agency code</param>
        /// <param name="listValueIds">Id value list</param>
        /// <param name="seriesId">seriesId value.</param>
        /// <returns>Next ASIT Drill Down Data List.</returns>
        ASITableDrillDSeriesModel4WS[] IASITableDrillDownBLL.GetNextASITableDrillDownDatas(string agencyCode, string[] listValueIds, long seriesId)
        {
            return ASITableDrillDownService.getNextASITableDrillDownDatas(agencyCode, listValueIds, seriesId);
        }

        /// <summary>
        /// Search value from last column content of drill down data table.
        /// </summary>
        /// <param name="agencyCode">Agency code</param>
        /// <param name="groupName">ASI table group name</param>
        /// <param name="subGroupName">ASI table sub group name that is ASI table name</param>
        /// <param name="searchValue">searching content</param>
        /// <returns>The search result array data format as below chars
        /// *a[0][0]=column name, e.g. : ColumnName1
        /// *a[1][0]=languageID : columnValue, e.g. : 1:country</returns>
        string[][] IASITableDrillDownBLL.LoadSearchResultBySearchValue(string agencyCode, string groupName, string subGroupName, string searchValue)
        {
            StringArray[] result = ASITableDrillDownService.loadSearchResultBySearchValue(agencyCode, groupName, subGroupName, searchValue, ACAConstant.GRANTED_RIGHT);
            System.Collections.Generic.List<string[]> resultItems = new System.Collections.Generic.List<string[]>();

            if (result != null)
            {
                for (int i = 0; i < result.Length; i++)
                {
                    resultItems.Add(result[i].item);
                }
            }

            return resultItems.ToArray();
        }

        #endregion Methods
    }
}