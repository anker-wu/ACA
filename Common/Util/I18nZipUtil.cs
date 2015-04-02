/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: I18nZipUtil.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2008-2014
 *
 *  Description:
 *  I18nZipUtil for getting I18n Zip information.
 *
 *  Notes:
 * $Id: I18nZipUtil.cs 278338 2014-09-02 05:56:58Z ACHIEVO\james.shi $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;

namespace Accela.ACA.Common.Util
{
    /// <summary>
    /// Provide a class to operation zip in I18n.
    /// </summary>
    public static class I18nZipUtil
    {
        #region Properties

        /// <summary>
        /// Gets validate the zip field.
        /// </summary>
        public static string ValidationExpression
        {
            get
            {
                return @"^.*$";
            }
        }

        /// <summary>
        /// Gets Zip mask
        /// </summary>
        public static string ZipMask
        {
            get
            {
                return "*********";
            }
        }

        /// <summary>
        /// Gets filter Non alphanumeric expression.
        /// </summary>
        public static string FilterNonAlphanumericExpression
        {
            get
            {
                return @"[^A-Za-z0-9]+";
            }
        }

        /// <summary>
        /// Formats the zip by mask.
        /// </summary>
        /// <param name="mask">The mask.</param>
        /// <param name="zipWithOutFormat">The zip with out format.</param>
        /// <returns>The formatted zip</returns>
        public static string FormatZipByMask(string mask, string zipWithOutFormat)
        {
            if (!string.IsNullOrEmpty(zipWithOutFormat))
            {
                Regex filterReg = new Regex(FilterNonAlphanumericExpression);
                zipWithOutFormat = filterReg.Replace(zipWithOutFormat, string.Empty);
            }

            StringBuilder zipWithFormat = new StringBuilder();

            if (string.IsNullOrEmpty(mask))
            {
                zipWithFormat.Append(zipWithOutFormat);
            }
            else if (!string.IsNullOrEmpty(zipWithOutFormat))
            {
                Regex regex = new Regex(MaskChars.OptionalMatchExpression);
                string optionMask = regex.Match(mask).Value;

                if (!string.IsNullOrEmpty(optionMask))
                {
                    mask = mask.Replace(optionMask, string.Empty);
                }

                int overPosition = FormatZipByMask(mask, zipWithOutFormat, 0, zipWithFormat);

                if (!string.IsNullOrEmpty(optionMask) && overPosition < zipWithOutFormat.Length - 1)
                {
                    FormatZipByMask(optionMask, zipWithOutFormat, overPosition, zipWithFormat);
                }
            }

            return zipWithFormat.ToString();
        }

        /// <summary>
        /// Unformat zip according to the mask.
        /// </summary>
        /// <param name="mask">The mask.</param>
        /// <param name="zipWithFormat">The zip with format.</param>
        /// <returns>The unformatted zip.</returns>
        public static string UnFormatZipByMask(string mask, string zipWithFormat)
        {
            StringBuilder zipWithOutFormat = new StringBuilder();
            zipWithOutFormat.Append(zipWithFormat);

            string stringMask = mask.Replace(MaskChars.NumericChar, ACAConstant.SPLIT_CHAR).Replace(
                                MaskChars.LetterChar, ACAConstant.SPLIT_CHAR);
            List<string> recordMaskChar = new List<string>();

            foreach (var maskChar in stringMask)
            {
                if (!ACAConstant.SPLIT_CHAR.Equals(maskChar) 
                    && !MaskChars.LeftBracket.Equals(maskChar) 
                    && !MaskChars.RightBracket.Equals(maskChar))
                {
                    recordMaskChar.Add(maskChar.ToString(CultureInfo.InvariantCulture));
                }
            }

            foreach (var maskChar in recordMaskChar)
            {
                zipWithOutFormat.Replace(maskChar, string.Empty);
            }

            return zipWithOutFormat.ToString().Trim();
        }

        /// <summary>
        /// format zip
        /// </summary>
        /// <param name="mask">mask string</param>
        /// <param name="zipWithOutFormat">the source zip, no format</param>
        /// <param name="startPosition">The Cursor in <paramref name="zipWithOutFormat"/>, start position to get substring.</param>
        /// <param name="zipWithFormat">format zip builder</param>
        /// <returns>The position of  <paramref name="zipWithOutFormat"/> </returns>
        private static int FormatZipByMask(string mask, string zipWithOutFormat, int startPosition, StringBuilder zipWithFormat)
        {
            string stringMask = mask.Replace(MaskChars.NumericChar, ACAConstant.SPLIT_CHAR)
                                .Replace(MaskChars.LetterChar, ACAConstant.SPLIT_CHAR);
            int i = startPosition;

            foreach (var maskChar in stringMask)
            {
                string zipChar = i < zipWithOutFormat.Length ? zipWithOutFormat.Substring(i, 1) : string.Empty;

                if (maskChar.Equals(MaskChars.LeftBracket) || maskChar.Equals(MaskChars.RightBracket))
                {
                    continue;
                }
                else if (!maskChar.Equals(ACAConstant.SPLIT_CHAR))
                {
                    zipWithFormat.Append(maskChar);
                }
                else
                {
                    zipWithFormat.Append(zipChar);
                    i++;
                }
            }

            return i;
        }

        #endregion Properties
    }
}