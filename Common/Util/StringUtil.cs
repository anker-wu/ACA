#region Header

/**
 *  Accela Citizen Access
 *  File: StringUtil.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2010-2014
 *
 *  Description:
 *   Provide accessibility function.
 *
 *  Notes:
 * $Id: StringUtil.cs 175327 2010-06-10 09:06:19Z ACHIEVO\daly.zeng $.
 *  Revision History
 *  Date,                  Who,                 What
 */

#endregion Header

using System;
using System.Globalization;

namespace Accela.ACA.Common.Util
{
    /// <summary>
    /// string utility class
    /// </summary>
    public static class StringUtil
    {
        /// <summary>
        /// convert string to nullable integer
        /// </summary>
        /// <param name="str">string value</param>
        /// <returns>nullable integer value</returns>
        public static int? ToInt(string str)
        {
            return string.IsNullOrEmpty(str) ? (int?)null : Convert.ToInt32(str);
        }
    
        /// <summary>
        /// convert string to nullable long
        /// </summary>
        /// <param name="str">string value</param>
        /// <returns>nullable long value</returns>
        public static long? ToLong(string str)
        {
            return string.IsNullOrEmpty(str) ? (long?)null : Convert.ToInt64(str);
        }

        /// <summary>
        /// convert string to nullable double
        /// </summary>
        /// <param name="str">string value</param>
        /// <returns>nullable double value</returns>
        public static double? ToDouble(string str)
        {
            double? result = null;

            if (!string.IsNullOrWhiteSpace(str))
            {
                bool existsComma = str.IndexOf(ACAConstant.COMMA, 0, StringComparison.OrdinalIgnoreCase) != -1;
                bool isDotDecimalSeparator = I18nNumberUtil.NumberDecimalSeparator == ACAConstant.SPOT_CHAR;

                if (existsComma && !isDotDecimalSeparator)
                {
                    result = Convert.ToDouble(str);
                }
                else
                {
                    result = Convert.ToDouble(str, CultureInfo.InvariantCulture);
                }
            }

            return result;
        }

        /// <summary>
        /// Go through the stringArray, if one item is not null or empty, return that item.
        /// If all items are null or empty, return empty string "".
        /// </summary>
        /// <param name="stringArray">string array</param>
        /// <returns>format string.</returns>
        public static string GetString(params string[] stringArray)
        {
            if (null == stringArray)
            {
                return string.Empty;
            }

            foreach (string tempString in stringArray)
            {
                if (!string.IsNullOrEmpty(tempString))
                {
                    return tempString;
                }
            }

            return string.Empty;
        }
    }
}
