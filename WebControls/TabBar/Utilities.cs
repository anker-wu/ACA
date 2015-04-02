#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: Utilities.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2009-2014
 *
 *  Description:
 *
 *  Notes:
 * $Id: Utilities.cs 276801 2014-08-07 02:14:39Z ACHIEVO\james.shi $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using System;
using System.Globalization;
using System.Text;

namespace Accela.Web.Controls.Navigation
{
    /// <summary>
    /// Utilities for the tab bar
    /// </summary>
    public sealed class Utilities
    {
        #region Methods

        /// <summary>
        /// Converts a DateTime to a corresponding JavaScript Date object.
        /// </summary>
        /// <param name="d">DateTime to convert.</param>
        /// <returns>A string that represents a JavaScript instantiation of
        /// a corresponding Date object.</returns>
        public static string ConvertDateTimeToJsDate(DateTime d)
        {
            StringBuilder dateConstructor = new StringBuilder();
            dateConstructor.Append("new Date(");
            dateConstructor.Append(d.Year);
            dateConstructor.Append(",");
            dateConstructor.Append(d.Month - 1); // In JavaScript, months are zero-delimited
            dateConstructor.Append(",");
            dateConstructor.Append(d.Day);
            dateConstructor.Append(",");
            dateConstructor.Append(d.Hour);
            dateConstructor.Append(",");
            dateConstructor.Append(d.Minute);
            dateConstructor.Append(",");
            dateConstructor.Append(d.Second);

            if (d.Millisecond > 0)
            {
                dateConstructor.Append(",");
                dateConstructor.Append(d.Millisecond);
            }

            dateConstructor.Append(")");
            return dateConstructor.ToString();
        }

        /// <summary>
        /// Replaced special character to suit the javascript json
        /// </summary>
        /// <param name="str">string content</param>
        /// <returns>an Json string</returns>
        public static string ConvertToJsonString(string str)
        {
            if (string.IsNullOrEmpty(str))
            {
                return string.Empty;
            }

            return "'" + str.Replace("\\", "\\\\").Replace("'", "\\'").Replace("\n", "\\n").Replace("\t", "\\t").Replace("\r", string.Empty) + "'";
        }

        /// <summary>
        /// Replaced special character to suit the javascript json depend on the input object type
        /// </summary>
        /// <param name="item">object item</param>
        /// <returns>javascript string</returns>
        public static string ObjectToJavaScriptString(object item)
        {
            if (item is string)
            {
                return "'" + ((string)item).Replace("\\", "\\\\").Replace("'", "\\'").Replace("\n", "\\n").Replace("\t", "\\t").Replace("\r", string.Empty) + "'";
            }
            else if (item is bool)
            {
                return item.ToString().ToLower();
            }
            else if (item is Enum)
            {
                return ((int)item).ToString();
            }
            else if (item is DateTime)
            {
                return Utilities.ConvertDateTimeToJsDate((DateTime)item);
            }
            else if (item is decimal || item is double)
            {
                return string.Format(CultureInfo.InvariantCulture, "{0}", item);
            }
            else if (item is short || item is int || item is long)
            {
                return item.ToString();
            }
            else if (item != null)
            {
                return ObjectToJavaScriptString(item.ToString());
            }
            else
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// Parse text align
        /// </summary>
        /// <param name="o">an object type</param>
        /// <returns>TextAlign object</returns>
        internal static TextAlign ParseTextAlign(object o)
        {
            if (o == null || o.ToString() == string.Empty)
            {
                return new TextAlign();
            }

            try
            {
                return (TextAlign)Enum.Parse(typeof(TextAlign), o.ToString(), true);
            }
            catch
            {
                throw new FormatException("'" + o.ToString() + "' can not be parsed as a TextAlign.You can try Left,Right,Center.");
            }
        }

        #endregion Methods
    }
}