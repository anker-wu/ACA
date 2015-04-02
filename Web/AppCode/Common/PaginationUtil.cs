#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: PaginationUtil.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2011-2014
 *
 *  Description:
 *
 *  Notes:
 *      $Id: PaginationUtil.cs 277071 2014-08-11 03:38:26Z ACHIEVO\james.shi $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using System.Collections;
using System.Data;
using System.Web;
using Accela.ACA.BLL.Common;
using Accela.ACA.Common;
using Accela.ACA.WSProxy;

namespace Accela.ACA.Web.Common
{
    /// <summary>
    /// pagination utility class
    /// </summary>
    public static class PaginationUtil
    {
        #region Fields

        #endregion Fields

        #region Methods
        /// <summary>
        /// get pagination model by clientID as key.
        /// </summary>
        /// <param name="clientID">grid view clientID</param>
        /// <returns>pagination model</returns>
        public static PaginationModel GetPageInfoByID(string clientID)
        {
            PaginationModel pageInfo = (PaginationModel)HttpContext.Current.Session[clientID];

            if (pageInfo == null)
            {
                pageInfo = new PaginationModel();
                pageInfo.ListID = clientID;
                HttpContext.Current.Session[clientID] = pageInfo;
            }

            return pageInfo;
        }

        /// <summary>
        /// get query format model.
        /// </summary>
        /// <param name="pageInfo">Pagination Model</param>
        /// <returns>QueryFormat model</returns>
        public static QueryFormat GetQueryFormatModel(PaginationModel pageInfo)
        {
            QueryFormat queryFormat = new QueryFormat();

            if (pageInfo.CurrentPageIndex.Equals(0))
            {
                pageInfo.StartDBRow = 0;
                pageInfo.IsSearchAllStartRow = false;
            }

            int startRow = pageInfo.StartDBRow > 0 ? pageInfo.StartDBRow : (pageInfo.CurrentPageIndex * pageInfo.CustomPageSize) + 1;
            int endRow = startRow + (pageInfo.CustomPageSize * ACAConstant.DEFAULT_PAGECOUNT) - 1;

            //query pageSize*defaultPageCount + 1(eg.101) records to avoid no data after click "..." link in gridView.
            endRow += ACAConstant.ADDITIONAL_RECORDS_COUNT;

            queryFormat.startRow = startRow;
            queryFormat.endRow = endRow;

            return queryFormat;
        }

        /// <summary>
        /// Construct New DataSource with old data source.
        /// </summary>
        /// <typeparam name="T">Generic type</typeparam>
        /// <param name="previous">old data source</param>
        /// <param name="next">new data source</param>
        /// <param name="pageInfo">The page info.</param>
        /// <returns>The merged data source.</returns>
        public static T MergeDataSource<T>(T previous, T next, PaginationModel pageInfo)
        {
            T result = previous == null ? next : previous;

            if (previous != null)
            {
                if (next is DataTable)
                {
                    //remove 101th record because it will be re-search out at next query.(eg.0-100,100-200)
                    DataTable previousData = result as DataTable;

                    if (previousData.Rows.Count % (pageInfo.CustomPageSize * ACAConstant.DEFAULT_PAGECOUNT) == ACAConstant.ADDITIONAL_RECORDS_COUNT)
                    {
                        previousData.Rows.RemoveAt(previousData.Rows.Count - ACAConstant.ADDITIONAL_RECORDS_COUNT);
                    }

                    previousData.Merge(next as DataTable);
                }

                if (next != null && next.GetType().FullName.StartsWith("System.Collections.Generic.List`1"))
                {
                    IList previousData = result as IList;

                    if (previousData.Count % (pageInfo.CustomPageSize * ACAConstant.DEFAULT_PAGECOUNT) == ACAConstant.ADDITIONAL_RECORDS_COUNT)
                    {
                        previousData.RemoveAt(previousData.Count - ACAConstant.ADDITIONAL_RECORDS_COUNT);
                    }

                    System.Reflection.MethodInfo mInfo = result.GetType().GetMethod("AddRange");

                    if (mInfo != null)
                    {
                        object val = mInfo.Invoke(result, new object[] { next });
                    }
                }
            }

            pageInfo.StartPage = pageInfo.CurrentPageIndex;
            pageInfo.EndPage = pageInfo.StartPage + ACAConstant.DEFAULT_PAGECOUNT - 1;

            return result;
        }

        /// <summary>
        /// get query format model for quick query of record list.
        /// </summary>
        /// <param name="pageInfo">Pagination Model</param>
        /// <param name="viewId">The view id.</param>
        /// <param name="dataFilterId">the data filter id.</param>
        /// <returns>QueryFormat model</returns>
        public static QueryFormat GetQuickQueryFormatModel(PaginationModel pageInfo, int viewId, long dataFilterId)
        {
            QueryFormat queryFormat = new QueryFormat();

            if (pageInfo.CurrentPageIndex.Equals(0))
            {
                pageInfo.StartDBRow = 0;
                pageInfo.IsSearchAllStartRow = false;
            }

            int startRow = pageInfo.StartDBRow > 0 ? pageInfo.StartDBRow : (pageInfo.CurrentPageIndex * pageInfo.CustomPageSize) + 1;
            int endRow = startRow + (pageInfo.CustomPageSize * ACAConstant.DEFAULT_PAGECOUNT) - 1;

            //query pageSize*defaultPageCount + 1(eg.101) records to avoid no data after click "..." link in gridView.
            endRow += ACAConstant.ADDITIONAL_RECORDS_COUNT;

            queryFormat.startRow = startRow;
            queryFormat.endRow = endRow;

            IDataFilterBll dataFilterBll = ObjectFactory.GetObject<IDataFilterBll>();
            XDataFilterElementModel[] dataFilterElementModels = dataFilterBll.GetXDataFilterElementByDataFilterId(ConfigManager.AgencyCode, viewId, dataFilterId, AppSession.User.UserSeqNum);
            queryFormat.quickQuery = dataFilterElementModels;

            return queryFormat;
        }

        #endregion Methods
    }
}