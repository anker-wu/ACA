#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: PaginationInfo.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2009
 *
 *  Description:
 *  
 *
 *  Notes:
 *  $Id: PaginationInfo.cs 130988 2009-8-27  8:36:01Z ACHIEVO\jackie.yu $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using System;
using System.Collections.Generic;

using Accela.ACA.Web.Common.GlobalSearch;

namespace Accela.ACA.Web.Common.GlobalSearch
{
    /// <summary>
    /// Pagination Information
    /// </summary>
    [Serializable]
    public class PaginationInfo
    {
        #region Private Fields

        /// <summary>
        /// The start index for current data range
        /// </summary>
        private int _startPageIndex;

        /// <summary>
        /// The end index for current data range
        /// </summary>
        private int _endPageIndex;

        /// <summary>
        /// start number
        /// </summary>
        private int _startNumber;

        /// <summary>
        /// Next start number
        /// </summary>
        private int _nextStartNumber;

        /// <summary>
        /// Actual record count. 
        /// For CAP/LP, it is record count returned from web service.
        /// For APO,it is record count calculated by sum([each parcel count]*[related address count]*[related owner count])
        /// </summary>
        private int _records;

        /// <summary>
        /// Remained APO records.
        /// if requested count is 100, but actual APO records is 306, then 300 records will be used this time, 6 records will be saved and used by subsequent list.
        /// </summary>
        private List<APOView4UI> _remainedAPORecords;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the PaginationInfo class.
        /// </summary>
        public PaginationInfo()
        {
            _remainedAPORecords = new List<APOView4UI>();
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Gets or sets the start page index
        /// </summary>
        public int StartPageIndex
        {
            get { return _startPageIndex; }
            set { _startPageIndex = value; }
        }

        /// <summary>
        /// Gets or sets the end page index
        /// </summary>
        public int EndPageIndex
        {
            get { return _endPageIndex; }
            set { _endPageIndex = value; }
        }

        /// <summary>
        /// Gets or sets the next start number
        /// </summary>
        public int NextStartNumber
        {
            get { return _nextStartNumber; }
            set { _nextStartNumber = value; }
        }

        /// <summary>
        /// Gets or sets the start number
        /// </summary>
        public int StartNumber
        {
            get { return _startNumber; }
            set { _startNumber = value; }
        }

        /// <summary>
        /// Gets or sets actual record count. 
        /// For CAP/LP, it is record count returned from web service.
        /// For APO,it is record count calculated by sum([each parcel count]*[related address count]*[related owner count])
        /// </summary>
        public int Records
        {
            get { return _records; }
            set { _records = value; }
        }

        /// <summary>
        /// Gets or sets the remained APO records
        /// </summary>
        public List<APOView4UI> RemainedAPORecords
        {
            get { return _remainedAPORecords; }
            set { _remainedAPORecords = value; }
        }

        #endregion
    }
}