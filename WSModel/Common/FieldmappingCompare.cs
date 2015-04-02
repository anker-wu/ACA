#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: FieldmappingCompare.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2009
 *
 *  Description:
 *  
 *
 *  Notes:
 *  $Id: FieldmappingCompare.cs 130988 2009-9-5  10:38:01Z ACHIEVO\jackie.yu $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using System;
using System.Collections;

namespace Accela.ACA.Common
{
    /// <summary>
    /// Fieldmapping Compare class for global search
    /// </summary>
    public class FieldmappingCompare : IComparer
    {
        /// <summary>
        /// global search type
        /// </summary>
        private GlobalSearchType _globalSearchType;

        /// <summary>
        /// Initializes a new instance of the <see cref="FieldmappingCompare"/> class.
        /// </summary>
        /// <param name="globalSearchType">Type of the global search.</param>
        public FieldmappingCompare(string globalSearchType)
        {
            this._globalSearchType = (GlobalSearchType)Enum.Parse(typeof(GlobalSearchType), globalSearchType, true);
        }

        #region IComparer Members

        /// <summary>
        /// Compare the property for sorting
        /// </summary>
        /// <param name="x">first object to compare</param>
        /// <param name="y">second object to compare</param>
        /// <returns>the object which has the smaller order number</returns>
        public int Compare(object x, object y)
        {
            FieldMappingAttribute xAttribute = x as FieldMappingAttribute;
            FieldMappingAttribute yAttribute = y as FieldMappingAttribute;
            int result = -1;

            switch (this._globalSearchType)
            {
                case GlobalSearchType.ADDRESS:
                    result = xAttribute.SearchbyByAddressOrder - yAttribute.SearchbyByAddressOrder;
                    break;
                case GlobalSearchType.PARCEL:
                    result = xAttribute.SearchbyByParcelOrder - yAttribute.SearchbyByParcelOrder;
                    break;
                default:
                    result = xAttribute.Order - yAttribute.Order;
                    break;
            }

            return result;
        }

        #endregion
    }
}