#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: EnumUtil.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2010-2014
 *
 *  Description:
 *  utitily for enum.
 *
 *  Notes:
 * $Id: EnumUtil.cs 276473 2014-08-01 06:43:29Z ACHIEVO\james.shi $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using System;

namespace Accela.ACA.Common.Util
{
    /// <summary>
    /// The enumeration utility
    /// </summary>
    /// <typeparam name="T">the enumeration type</typeparam>
    public static class EnumUtil<T> where T : struct, IConvertible
    {
        /// <summary>
        /// Parses the specified enumeration string.
        /// when no enumeration value matched, the default value will be returned.
        /// </summary>
        /// <param name="enumString">The enumeration string.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns>parsed enumeration value</returns>
        public static T Parse(string enumString, T defaultValue)
        {
            if (!string.IsNullOrEmpty(enumString))
            {
                T newEnumVlaue;
                return Enum.TryParse(enumString, true, out newEnumVlaue) ? newEnumVlaue : defaultValue;
            }
            else
            {
                return defaultValue;
            }
        }

        /// <summary>
        /// Parses the specified enumeration string.
        /// the provided string must be exact, otherwise, the method will throw exception.
        /// </summary>
        /// <param name="enumString">The enumeration string.</param>
        /// <returns>parsed enumeration value</returns>
        public static T Parse(string enumString)
        {
            T newEnumVlaue;
            return Enum.TryParse(enumString, true, out newEnumVlaue) ? newEnumVlaue : default(T);
        }
    }
}
