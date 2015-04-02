#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: GlobalSearchParameter.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2009-2014
 *
 *  Description:
 *  
 *
 *  Notes:
 *  $Id: GlobalSearchParameter.cs 130988 2009-8-20  11:18:01Z ACHIEVO\jackie.yu $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using System;
using System.Collections;
using System.Collections.Generic;

using Accela.ACA.Common;

namespace Accela.ACA.Web.Common.GlobalSearch
{
    /// <summary>
    /// Global Search Parameter
    /// </summary>
    [Serializable]
    public class GlobalSearchParameter
    {
        #region Private Fields

        /// <summary>
        /// Search type
        /// </summary>
        private GlobalSearchType _globalSearchType;

        /// <summary>
        /// Query text
        /// </summary>
        private string _queryText;

        /// <summary>
        /// Sort column name
        /// </summary>
        private string _sortColulmn;

        /// <summary>
        /// Sort direction
        /// </summary>
        private string _sortDirection;

        /// <summary>
        /// Page index
        /// </summary>
        private int _pageIndex;

        /// <summary>
        /// The page size
        /// </summary>
        private int _pageSize;

        /// <summary>
        /// Module array
        /// </summary>
        private string[] _moduleArray;

        /// <summary>
        /// Total records from web service
        /// </summary>
        private int _totalRecordsFromWS;

        /// <summary>
        /// Pagination information
        /// </summary>
        private List<PaginationInfo> _paginationInfoCollection;

        /// <summary>
        /// Record count from web service each time
        /// </summary>
        private int _recordCount;

        /// <summary>
        /// Cap results form web service
        /// </summary>
        private List<CAPView4UI> _capResults;

        /// <summary>
        /// LP result from web service
        /// </summary>
        private List<LPView4UI> _lpResults;
            
        /// <summary>
        /// APO result from web service
        /// </summary>
        private List<APOView4UI> _apoResults;

        /// <summary>
        /// Start number
        /// </summary>
        private int _startNumber;

        /// <summary>
        /// Initializes a new instance of the GlobalSearchParameter class.
        /// </summary>
        /// <param name="searchType">search type</param>
        public GlobalSearchParameter(GlobalSearchType searchType)
        {
            this._queryText = string.Empty;
            this._globalSearchType = searchType;
            this._paginationInfoCollection = new List<PaginationInfo>();
            this._capResults = new List<CAPView4UI>();
            this._lpResults = new List<LPView4UI>();
            this._apoResults = new List<APOView4UI>();
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets the search type
        /// </summary>
        public GlobalSearchType GlobalSearchType
        {
            get { return _globalSearchType; }
            set { _globalSearchType = value; }
        }

        /// <summary>
        /// Gets or sets the query text
        /// </summary>
        public string QueryText
        {
            get { return _queryText; }
            set { _queryText = value; }
        }

        /// <summary>
        /// Gets or sets the sort column name
        /// </summary>
        public string SortColulmn
        {
            get { return _sortColulmn; }
            set { _sortColulmn = value; }
        }

        /// <summary>
        /// Gets or sets the sort direction
        /// </summary>
        public string SortDirection
        {
            get { return _sortDirection; }
            set { _sortDirection = value; }
        }

        /// <summary>
        /// Gets or sets the page index
        /// </summary>
        public int PageIndex
        {
            get { return _pageIndex; }
            set { _pageIndex = value; }
        }

        /// <summary>
        /// Gets or sets the page size
        /// </summary>
        public int PageSize
        {
            get { return _pageSize; }
            set { _pageSize = value; }
        }

        /// <summary>
        /// Gets or sets the module names
        /// </summary>
        public string[] ModuleArray
        {
            get { return _moduleArray; }
            set { _moduleArray = value; }
        }

        /// <summary>
        /// Gets or sets the pagination info
        /// </summary>
        public List<PaginationInfo> PaginationInfoCollection
        {
            get { return _paginationInfoCollection; }
            set { _paginationInfoCollection = value; }
        }

        /// <summary>
        /// Gets or sets the record count
        /// </summary>
        public int RecordCount
        {
            get { return _recordCount; }
            set { _recordCount = value; }
        }

        /// <summary>
        /// Gets or sets the cap results
        /// </summary>
        public List<CAPView4UI> CapResults
        {
            get { return _capResults; }
            set { _capResults = value; }
        }

        /// <summary>
        /// Gets or sets the LP results
        /// </summary>
        public List<LPView4UI> LpResults
        {
            get { return _lpResults; }
            set { _lpResults = value; }
        }

        /// <summary>
        /// Gets or sets the apo results
        /// </summary>
        public List<APOView4UI> ApoResults
        {
            get { return _apoResults; }
            set { _apoResults = value; }
        }

        /// <summary>
        /// Gets or sets total records from web service
        /// </summary>
        public int TotalRecordsFromWS
        {
            get { return _totalRecordsFromWS; }
            set { _totalRecordsFromWS = value; }
        }

        /// <summary>
        /// Gets or sets the start number
        /// </summary>
        public int StartNumber
        {
            get { return _startNumber; }
            set { _startNumber = value; }
        }

        #endregion

        #region Internal Methods

        /// <summary>
        /// Get current start number
        /// </summary>
        /// <returns>the start number</returns>
        internal int GetCurrentStartNumber()
        {
            int startNumber = 0;

            PaginationInfo currentPagination = this.GetPaginationInfo(this.PageIndex);

            if (currentPagination != null)
            {
                startNumber = currentPagination.StartNumber;
            }
            else
            {
                PaginationInfo previousPagination = this.GetPreviousPagination();

                if (previousPagination != null)
                {
                    startNumber = previousPagination.NextStartNumber;
                }
            }

            return startNumber;
        }

        /// <summary>
        /// Get pagination info
        /// </summary>
        /// <param name="pageIndex">the page index</param>
        /// <returns>the instance of paginationInfo entity</returns>
        internal PaginationInfo GetPaginationInfo(int pageIndex)
        {
            PaginationInfo pagination = null;

            // get current pagination info
            foreach (PaginationInfo item in this.PaginationInfoCollection)
            {
                if (item.StartPageIndex <= pageIndex && item.EndPageIndex >= pageIndex)
                {
                    pagination = item;
                    break;
                }
            }

            return pagination;
        }

        /// <summary>
        /// Need request new records
        /// </summary>
        /// <returns>true: get results from web service</returns>
        internal bool NeedRequestNewRecords()
        {
            PaginationInfo pagination = null;
            bool needRequestNewRecords = true;

            if (this.PaginationInfoCollection != null && this.PaginationInfoCollection.Count > 0)
            {
                pagination = this.PaginationInfoCollection[this.PaginationInfoCollection.Count - 1];
            }

            if (pagination != null)
            {
                if (this.PageIndex >= pagination.StartPageIndex && this.PageIndex <= pagination.EndPageIndex)
                {
                    needRequestNewRecords = false;
                }
            }

            return needRequestNewRecords;
        }

        /// <summary>
        /// Update pagination info
        /// </summary>
        /// <param name="records">record count per each web service request</param>
        /// <param name="startNumber">the current start number</param>
        /// <param name="parameter">global search parameter</param>
        /// <param name="remainedList">remained APOView4UI list</param>
        internal void UpdatePaginationInfo(int records, int startNumber, GlobalSearchParameter parameter, List<APOView4UI> remainedList)
        {
            PaginationInfo pagination = this.GetPaginationInfo(parameter.PageIndex);
            bool isRecordChanged = pagination != null && pagination.Records != records && records != 0;
            bool isNewRecord = pagination == null && records != 0;
            int remains = remainedList == null ? 0 : remainedList.Count;

            RemoveInvalidPaginationInfo(parameter.PageIndex);

            if (isNewRecord)
            {
                pagination = new PaginationInfo();

                PaginationInfo previousPagination = this.GetPreviousPagination();

                if (previousPagination != null)
                {
                    pagination.StartNumber = previousPagination.NextStartNumber;
                    pagination.StartPageIndex = previousPagination.EndPageIndex + 1;
                }

                this.PaginationInfoCollection.Add(pagination);
            }

            if (isRecordChanged || isNewRecord)
            {
                int pageCount = (records - remains) / parameter.PageSize;
                int residualRecCount = (records - remains) % parameter.PageSize;

                //The additional one record should not be record in new page or else the next pagesize*defaultpage records will not be search out.
                if (residualRecCount != 0 &&
                    (residualRecCount != ACAConstant.ADDITIONAL_RECORDS_COUNT || pageCount < ACAConstant.DEFAULT_PAGECOUNT))
                {
                    pageCount++;
                }

                pagination.EndPageIndex = pagination.StartPageIndex + pageCount - 1;
                pagination.NextStartNumber = startNumber - ACAConstant.ADDITIONAL_RECORDS_COUNT;
                pagination.Records = records;
                pagination.RemainedAPORecords = remainedList;
            }
        }

        /// <summary>
        /// Get previous total counts
        /// </summary>
        /// <returns>total counts</returns>
        internal int GetPreviousTotalCounts()
        {
            int totals = 0;

            foreach (PaginationInfo info in this.PaginationInfoCollection)
            {
                if (info.EndPageIndex < this.PageIndex)
                {
                    if (info.RemainedAPORecords != null)
                    {
                        totals += info.Records - info.RemainedAPORecords.Count;

                        //Remove additional record from the previous search result.(101->100)
                        if (totals % (PageSize * ACAConstant.DEFAULT_PAGECOUNT) == ACAConstant.ADDITIONAL_RECORDS_COUNT)
                        {
                            totals -= ACAConstant.ADDITIONAL_RECORDS_COUNT;
                        }
                    }
                    else
                    {
                        totals += info.Records;
                    }
                }
            }

            return totals;
        }

        /// <summary>
        /// Get remained records
        /// </summary>
        /// <param name="pageIndex">the page index</param>
        /// <returns>remain APO records</returns>
        internal List<APOView4UI> GetRemainedRecords(int pageIndex)
        {
            List<APOView4UI> remainList = new List<APOView4UI>();

            // get previous pagination
            foreach (PaginationInfo previousPagination in this.PaginationInfoCollection)
            {
                if (previousPagination.EndPageIndex + 1 == pageIndex)
                {
                    remainList = previousPagination.RemainedAPORecords;
                    break;
                }
            }

            return remainList;
        }

        /// <summary>
        /// Remove invalid pagination info which StartPageIndex is greater than current pageIndex.
        /// </summary>
        /// <param name="currentPageIndex">current page index</param>
        private void RemoveInvalidPaginationInfo(int currentPageIndex)
        {
            // update exist item
            int count = this.PaginationInfoCollection.Count;

            // remove all list after this item
            for (int i = count - 1; i >= 0; i--)
            {
                PaginationInfo info = this.PaginationInfoCollection[i];

                if (info.StartPageIndex > currentPageIndex)
                {
                    this.PaginationInfoCollection.RemoveAt(i);
                }
            }
        }

        /// <summary>
        /// Get previous pagination
        /// </summary>
        /// <returns>previous pagination</returns>
        private PaginationInfo GetPreviousPagination()
        {
            PaginationInfo pagination = null;

            if (this.PaginationInfoCollection.Count > 0)
            {
                pagination = this.PaginationInfoCollection[this.PaginationInfoCollection.Count - 1];
            }

            return pagination;
        }

        #endregion
    }
}