#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: PaginationModel.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2011-2014
 *
 *  Description:
 *
 *  Notes:
 *      $Id: PaginationModel.cs 277071 2014-08-11 03:38:26Z ACHIEVO\james.shi $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using System;

namespace Accela.ACA.Web.Common
{
    /// <summary>
    /// pagination model
    /// </summary>
    [Serializable]
    public class PaginationModel
    {
        #region Properties

        /// <summary>
        /// Gets or sets start page.
        /// </summary>
        public int StartPage
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets end page.
        /// </summary>
        public int EndPage
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets start DB row.
        /// </summary>
        public int StartDBRow
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets grid view page size.
        /// </summary>
        public int CustomPageSize
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets current page index.
        /// </summary>
        public int CurrentPageIndex
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets sort expression
        /// </summary>
        public string SortExpression
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets grid view control id as a key stored in session
        /// </summary>
        public string ListID
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets condition for search.
        /// </summary>
        public object[] SearchCriterias
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets a value indicating whether search my cap list or not.
        /// </summary>
        public bool IsSearchAllStartRow
        {
            get;
            set;
        }

        #endregion Properties
    }
}