/**
 * <pre>
 *
 *  Accela
 *  File: AccelaGridViewSortEventArgs.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2014
 *
 *  Description:
 *
 *  Notes:
 * $Id: AccelaGridViewSortEventArgs.cs 276801 2014-08-07 02:14:39Z ACHIEVO\james.shi $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 *  07/10/2007    daly.zeng    Initial.
 * </pre>
 */

using System;

namespace Accela.Web.Controls
{
    /// <summary>
    /// accela grid view sort event
    /// </summary>
    public class AccelaGridViewSortEventArgs : EventArgs
    {
        #region Fields

        /// <summary>
        /// grid view sort expression
        /// </summary>
        private string _gridViewSortExpression;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the AccelaGridViewSortEventArgs class.
        /// </summary>
        /// <param name="gridViewSortExpression">sort expression</param>
        public AccelaGridViewSortEventArgs(string gridViewSortExpression)
        {
            GridViewSortExpression = gridViewSortExpression;
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// Gets or sets grid view sort expression
        /// </summary>
        public string GridViewSortExpression
        {
            get
            {
                return _gridViewSortExpression;
            }

            set
            {
                _gridViewSortExpression = value;
            }
        }

        #endregion Properties
    }
}
