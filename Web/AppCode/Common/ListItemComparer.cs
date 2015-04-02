#region Header

/**
 *  Accela Citizen Access
 *  File: ListItemComparer.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2008-2014
 *
 *  Description:
 *   Provide List Item Comparer.
 *
 *  Notes:
 * $Id: ListItemComparer.cs 277071 2014-08-11 03:38:26Z ACHIEVO\james.shi $.
 *  Revision History
 *  Date,                  Who,                 What
 */

#endregion Header

using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Web.UI.WebControls;

namespace Accela.ACA.Web.Common
{
    /// <summary>
    /// ListItem comparer.
    /// </summary>
    public sealed class ListItemComparer : IComparer<ListItem>
    {
        #region Fields

        /// <summary>
        /// The instance of the ListItemComparer.
        /// </summary>
        private static readonly ListItemComparer InstanceField = new ListItemComparer();

        /// <summary>
        /// compare information.
        /// </summary>
        private CompareInfo _compareInfo;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Prevents a default instance of the <see cref="ListItemComparer"/> class from being created.
        /// </summary>
        private ListItemComparer()
        {
            _compareInfo = CultureInfo.InvariantCulture.CompareInfo;
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// Gets the instance of the ListItemComparer.
        /// </summary>
        public static ListItemComparer Instance
        {
            get
            {
                return InstanceField;
            }
        }

        #endregion Properties

        #region Methods

        /// <summary>
        /// ListItem Compare method
        /// </summary>
        /// <param name="x">ListItem x</param>
        /// <param name="y">ListItem y</param>
        /// <param name="options">The compare options</param>
        /// <returns>The compare result.</returns>
        public int Compare(ListItem x, ListItem y, CompareOptions options)
        {
            string a = (null == x) ? null : x.Text as string;
            string b = (null == y) ? null : y.Text as string;
            if (null != a &&
                null != b)
            {
                return _compareInfo.Compare(a, b, options);
            }
            else
            {
                return Comparer.Default.Compare(a, b);
            }
        }

        /// <summary>
        /// ListItem Compare method
        /// </summary>
        /// <param name="x">ListItem x</param>
        /// <param name="y">ListItem y</param>
        /// <returns>The compare result.</returns>
        int IComparer<ListItem>.Compare(ListItem x, ListItem y)
        {
            return Compare(x, y, CompareOptions.None);
        }

        #endregion Methods
    }
}