#region Header

/**
 *  Accela Citizen Access
 *  File: ItemValueComparer.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2008-2014
 *
 *  Description:
 *   Provide Item Value Comparer.
 *
 *  Notes:
 * $Id: ItemValueComparer.cs 276545 2014-08-04 02:15:30Z ACHIEVO\james.shi $.
 *  Revision History
 *  Date,                  Who,                 What
 */

#endregion Header

using System.Collections;
using System.Collections.Generic;
using System.Globalization;

namespace Accela.ACA.Common
{
    /// <summary>
    /// ItemValue comparer.
    /// </summary>
    public sealed class ItemValueComparer : IComparer<ItemValue>
    {
        #region Fields

        /// <summary>
        /// instance item value comparer.
        /// </summary>
        private static readonly ItemValueComparer InstanceField = new ItemValueComparer();

        /// <summary>
        /// Instance for key comparing
        /// </summary>
        private static readonly ItemValueComparer InstanceForKeyComparingField = new ItemValueComparer(true);

        /// <summary>
        /// compare Info
        /// </summary>
        private CompareInfo _compareInfo;

        /// <summary>
        /// use key to compare.
        /// </summary>
        private bool _useKeyToCompare;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Prevents a default instance of the ItemValueComparer class from being created.
        /// </summary>
        private ItemValueComparer()
        {
            _compareInfo = CultureInfo.InvariantCulture.CompareInfo;
        }

        /// <summary>
        /// Initializes a new instance of the ItemValueComparer class.
        /// </summary>
        /// <param name="useKeyToCompare">use key to compare</param>
        private ItemValueComparer(bool useKeyToCompare) : this()
        {
            _useKeyToCompare = useKeyToCompare;
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// Gets instance for item value comparer.
        /// </summary>
        public static ItemValueComparer Instance
        {
            get
            {
                return InstanceField;
            }
        }

        /// <summary>
        /// Gets Instance for key comparing
        /// </summary>
        public static ItemValueComparer InstanceForKeyComparing
        {
            get
            {
                return InstanceForKeyComparingField;
            }
        }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Compare item.
        /// </summary>
        /// <param name="x">x: item content</param>
        /// <param name="y">y: item content</param>
        /// <param name="options">compare options.</param>
        /// <returns>the result for compare.</returns>
        public int Compare(ItemValue x, ItemValue y, CompareOptions options)
        {
            string a = string.Empty;
            string b = string.Empty;

            if (_useKeyToCompare)
            {
                a = (null == x) ? null : x.Key as string;
                b = (null == y) ? null : y.Key as string;
            }
            else
            {
                a = (null == x) ? null : x.Value as string;
                b = (null == y) ? null : y.Value as string;
            }

            if (null != a && null != b)
            {
                return _compareInfo.Compare(a, b, options);
            }
            else
            {
                return Comparer.Default.Compare(a, b);
            }
        }

        /// <summary>
        /// ItemValue Compare method
        /// </summary>
        /// <param name="x">x: item content.</param>
        /// <param name="y">y: item content.</param>
        /// <returns>the result info for compare.</returns>
        int IComparer<ItemValue>.Compare(ItemValue x, ItemValue y)
        {
            return Compare(x, y, CompareOptions.None);
        }

        #endregion Methods
    }
}