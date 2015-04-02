/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: I18nPhoneUtil.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2008-2014
 *
 *  Description:
 *  I18nPhoneUtil for getting I18n Phone information.
 *
 *  Notes:
 * $Id: I18nPhoneUtil.cs 278338 2014-09-02 05:56:58Z ACHIEVO\james.shi $.
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
    /// provides I18n phone to serve the framework. 
    /// </summary>
    public static class I18nPhoneUtil
    {
        #region Properties

        ///// <summary>
        ///// Gets Phone mask
        ///// </summary>
        //public static string PhoneMask
        //{
        //    get
        //    {
        //        return "999-999-9999";
        //    }
        //}

        /// <summary>
        /// Gets the 40 digital phone number, only number is allowed.
        /// </summary>
        public static string PhoneNumberMask
        {
            get
            {
                return "########################################";
            }
        }

        /// <summary>
        /// Gets validate the phone field.
        /// </summary>
        public static string ValidationExpression
        {
            get
            {
                return @"^[0-9]*$|^[0-9]*\s*$";
            }
        }

        /// <summary>
        /// Gets filter not a number expression.
        /// </summary>
        public static string FilterNaNExpression
        {
            get
            {
                return @"[^0-9]+";
            }
        }

        /// <summary>
        /// Formats the phone by mask.
        /// </summary>
        /// <param name="mask">The mask.</param>
        /// <param name="phoneWithOutFormat">The phone number with out format.</param>
        /// <returns>The formatted phone number.</returns>
        public static string FormatPhoneByMask(string mask, string phoneWithOutFormat)
        {
            if (!string.IsNullOrEmpty(phoneWithOutFormat))
            {
                Regex filterReg = new Regex(FilterNaNExpression);
                phoneWithOutFormat = filterReg.Replace(phoneWithOutFormat, string.Empty);
            }

            StringBuilder zipWithFormat = new StringBuilder();

            if (string.IsNullOrEmpty(mask))
            {
                zipWithFormat.Append(phoneWithOutFormat);
            }
            else if (!string.IsNullOrEmpty(phoneWithOutFormat))
            {
                int i = 0;

                foreach (var maskChar in mask)
                {
                    string zipChar = i < phoneWithOutFormat.Length ? phoneWithOutFormat.Substring(i, 1) : string.Empty;

                    if (!maskChar.Equals(MaskChars.NumericChar))
                    {
                        zipWithFormat.Append(maskChar);
                    }
                    else
                    {
                        zipWithFormat.Append(zipChar);
                        i++;
                    }
                }
            }

            return zipWithFormat.ToString();
        }

        /// <summary>
        /// Unformat the phone according to the mask.
        /// </summary>
        /// <param name="mask">The mask.</param>
        /// <param name="phoneWithFormat">The phone with format.</param>
        /// <returns>The unformatted phone number.</returns>
        public static string UnFormatPhoneByMask(string mask, string phoneWithFormat)
        {
            StringBuilder zipWithOutFormat = new StringBuilder();
            zipWithOutFormat.Append(phoneWithFormat);

            string stringMask = mask.Replace(MaskChars.NumericChar, ACAConstant.SPLIT_CHAR);
            List<string> recordMaskChar = new List<string>();

            foreach (var maskChar in stringMask)
            {
                if (!ACAConstant.SPLIT_CHAR.Equals(maskChar))
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

        #endregion Properties
    }
}