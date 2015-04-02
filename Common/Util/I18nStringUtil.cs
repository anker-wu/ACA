#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: I18nStringUtil.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2008-2014
 *
 *  Description:
 *  I18nStringUtil for getting I18n string information.
 *
 *  Notes:
 * $Id: I18nStringUtil.cs 276545 2014-08-04 02:15:30Z ACHIEVO\james.shi $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using System.Text;

namespace Accela.ACA.Common.Util
{
    /// <summary>
    /// Provide a class to operation string in I18n.
    /// </summary>
    public static class I18nStringUtil
    {
        #region Methods

        /// <summary>
        /// Format to plain row.
        /// </summary>
        /// <param name="cellStringArray">The string array.</param>
        /// <param name="splitChars">The split char.</param>
        /// <returns>Return the format string with split char.</returns>
        public static string FormatToPlainRow(string[] cellStringArray, string splitChars)
        {
            if (cellStringArray == null || cellStringArray.Length == 0)
            {
                return string.Empty;
            }

            StringBuilder builder = new StringBuilder();

            foreach (string s in cellStringArray)
            {
                if (!string.IsNullOrEmpty(s))
                {                    
                    builder.Append(s);
                    builder.Append(splitChars);
                }
            }

            if (builder.Length > 0)
            {
                builder.Remove(builder.Length - splitChars.Length, splitChars.Length);
            }

            return builder.ToString();
        }

        /// <summary>
        /// Format to plain row
        /// </summary>
        /// <param name="cellStringArray">cell string array</param>
        /// <returns>Format string</returns>
        public static string FormatToTableRow(params string[] cellStringArray)
        {
            return FormatToTableRow(cellStringArray.Length, cellStringArray);
        }

        /// <summary>
        /// Format to plain row
        /// </summary>
        /// <param name="countOfCellsInOneRow">count of cells in one row.</param>
        /// <param name="cellStringArray">cell string array</param>
        /// <returns>Format string</returns>
        public static string FormatToTableRow(int countOfCellsInOneRow, params string[] cellStringArray)
        {
            int countOfCells = countOfCellsInOneRow <= 0 ? 1 : countOfCellsInOneRow;

            if (null == cellStringArray)
            {
                return string.Empty;
            }

            StringBuilder builder = new StringBuilder();
            builder.Append("<table role='presentation' class='ACA_TDAlignLeftOrRightTop' border='0' cellpadding='0' cellspacing='0'>");
            int stringCount = 0;

            foreach (string s in cellStringArray)
            {
                stringCount++;
                bool needAddBeginTRTag = 1 == (stringCount % countOfCells) || cellStringArray.Length == 1;
                bool needAddEndTRTag = 0 == (stringCount % countOfCells) || (stringCount == cellStringArray.Length);

                if (needAddBeginTRTag)
                {
                    builder.Append("<tr>");
                }

                builder.Append(string.Format("<td style='vertical-align:top;'>{0}</td>", null == s ? "&nbsp;" : s));

                if (needAddEndTRTag)
                {
                    builder.Append("</tr>");
                }
            }

            builder.Append("</table>");

            return builder.ToString();
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

        /// <summary>
        /// Get the attribute string value for current language.
        /// </summary>
        /// <param name="resVal">Attribute value for current language</param>
        /// <param name="defVal">Attribute value for default language</param>
        /// <returns>Attribute value</returns>
        public static string GetCurrentLanguageString(string resVal, string defVal)
        {
            if (I18nCultureUtil.IsMultiLanguageEnabled)
            {
                return resVal;
            }
            else
            {
                return defVal;
            }
        }

        #endregion Methods
    }
}
