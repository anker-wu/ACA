/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: MaskUtil.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2013-2014
 *
 *  Description:
 *  MaskUtil for operating the mask.
 *
 *  Notes:
 * $Id: MaskUtil.cs 276545 2014-08-04 02:15:30Z ACHIEVO\james.shi $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

using Accela.ACA.Common.Common;

namespace Accela.ACA.Common.Util
{
    /// <summary>
    /// Provide a class to operate mask.
    /// </summary>
    public static class MaskUtil
    {
        #region Fields

        /// <summary>
        /// The default fein mask
        /// </summary>
        private const string DEFAULT_FEIN_MASK = "***********";

        #endregion Fields

        #region Properties

        /// <summary>
        /// Gets validate the SSN field.
        /// </summary>
        public static string SSNValidationExpression
        {
            get
            {
                MaskConverter converter = new MaskConverter(SSNMaskFromAA);
                return converter.ValidationExpression;
            }
        }

        /// <summary>
        /// Gets the SSN mask of the Standard Choice.
        /// </summary>
        public static string SSNMaskFromAA
        {
            get
            {
                string result = GetMaskFromAA(BizDomainConstant.STD_MASKS_ITEM_SSN);

                return string.IsNullOrEmpty(result) ? "[*][*][*]-[*][*]-****" : ConvertMaskFromAA(result);
            }
        }

        /// <summary>
        /// Gets the FEIN mask of the Standard Choice.
        /// </summary>
        public static string FEINMaskFromAA
        {
            get
            {
                string result = GetMaskFromAA(BizDomainConstant.STD_MASKS_ITEM_FEIN);

                return string.IsNullOrEmpty(result) ? DEFAULT_FEIN_MASK : ConvertMaskFromAA(result);
            }
        }

        #endregion Properties

        #region Public method

        /// <summary>
        /// Update FEIN according to user input.
        /// </summary>
        /// <param name="fein">old FEIN value without mask</param>
        /// <param name="maskFein">masked FEIN value</param>
        /// <returns>unmasked FEIN value</returns>
        public static string UpdateFEIN(string fein, string maskFein)
        {
            return Update(fein, maskFein, FEINMaskFromAA != DEFAULT_FEIN_MASK);
        }

        /// <summary>
        /// Update social security number according to user input. 
        /// After remove all the "-" characters of SSN, update <c>maskSsn</c> with the character in the related position of SSN.
        /// </summary>
        /// <param name="ssn">social security number</param>
        /// <param name="maskSsn">masked social security</param>
        /// <returns>unmasked social security</returns>
        public static string UpdateSSN(string ssn, string maskSsn)
        {
            return Update(ssn, maskSsn, true);
        }

        /// <summary>
        /// Update the masked value with the character in the related position of the old value and get a unmasked value. 
        /// If the dash "-" is a mask, need to remove all the dash "-" of the old value, then update.
        /// Otherwise, no need to remove them.
        /// </summary>
        /// <param name="oldValue">old value without mask</param>
        /// <param name="maskValue">masked value</param>
        /// <param name="dashAsMask">Indicate whether the dash "-" is a mask.</param>
        /// <returns>unmasked value</returns>
        public static string Update(string oldValue, string maskValue, bool dashAsMask)
        {
            if (!string.IsNullOrEmpty(maskValue)
                && !string.IsNullOrEmpty(oldValue)
                && maskValue.Contains(ACAConstant.DELIMITER_STAR))
            {
                int validIndex = 0;
                string result = null;

                if (dashAsMask)
                {
                    oldValue = oldValue.Replace("-", string.Empty);
                }

                for (int i = 0; i < maskValue.Length; i++)
                {
                    if (maskValue[i] == ACAConstant.DELIMITER_STAR[0] && validIndex < oldValue.Length)
                    {
                        result += oldValue[validIndex];
                        validIndex++;
                    }
                    else if (maskValue[i] == '-')
                    {
                        result += "-";
                    }
                    else
                    {
                        result += maskValue[i];
                        validIndex++;
                    }
                }

                maskValue = result;
            }

            return maskValue;
        }

        /// <summary>
        /// Format social security number by the SSN mask of the Standard Choice.
        /// </summary>
        /// <param name="ssn">social security number</param>
        /// <returns>SSN mask, such as: ***-**-????.</returns>
        public static string FormatSSNShow(string ssn)
        {
            if (!string.IsNullOrEmpty(ssn))
            {
                MaskConverter converter = new MaskConverter(SSNMaskFromAA);
                ssn = Format(ssn, converter);
            }

            return ssn;
        }

        /// <summary>
        /// Format the FEIN by the FEIN mask of the Standard Choice. 
        /// if isEnableFeinMasking is false, only enable format it and not enable mask it.
        /// </summary>
        /// <param name="fein">the FEIN ready to format.</param>
        /// <param name="isEnableFeinMasking">True if enable FEIN masking, false otherwise.</param>
        /// <returns>The SSN mask format, such as: **-***12345 or *****123456.</returns>
        public static string FormatFEINShow(string fein, bool isEnableFeinMasking)
        {
            if (string.IsNullOrEmpty(fein))
            {
                return fein;
            }

            string mask = FEINMaskFromAA;
            MaskConverter converter = new MaskConverter(mask);
            return Format(fein, converter, isEnableFeinMasking, mask != DEFAULT_FEIN_MASK);
        }
        
        /// <summary>
        /// Format the source string by the specified mask
        /// </summary>
        /// <param name="source">The source string ready to format.</param>
        /// <param name="converter">An object convert the mask of AA(standard choice) to ACA's.</param>
        /// <returns>Formatted value, such as: **-***1234 or *****1234.</returns>
        public static string Format(string source, MaskConverter converter)
        {
            return Format(source, converter, true, true);
        }

        /// <summary>
        /// Format the source string by the specified mask. 
        /// if isEnableMasking is false, not enable mask it.
        /// </summary>
        /// <param name="source">The source string ready to format.</param>
        /// <param name="converter">An object convert the mask of AA(standard choice) to ACA's.</param>
        /// <param name="isEnableMasking">If false, not enable mask it.</param>
        /// <param name="dashAsMask">Indicate whether the dash "-" is a mask.</param>
        /// <returns>Formatted value, such as: **-***1234 or *****1234.</returns>
        public static string Format(string source, MaskConverter converter, bool isEnableMasking, bool dashAsMask)
        {
            if (string.IsNullOrEmpty(source)
                || converter == null
                || string.IsNullOrEmpty(converter.StarFormat)
                || string.IsNullOrEmpty(converter.MaskFormat))
            {
                return source;
            }

            string result = string.Empty;
            string mask;

            if (dashAsMask)
            {
                source = source.Replace("-", string.Empty);
                mask = converter.Mask.Replace("-", string.Empty);
            }
            else
            {
                mask = converter.Mask;
            }

            string starFormat = converter.StarFormat;
            string maskFormat = converter.MaskFormat;
            int sourceIndex = 0;

            for (int i = 0; i < starFormat.Length; i++)
            {
                if (isEnableMasking && starFormat[i] == ACAConstant.DELIMITER_STAR[0])
                {
                    if (source.Length > sourceIndex
                        && IsValidMaskValue(mask, sourceIndex, source.Substring(sourceIndex, 1)))
                    {
                        result += ACAConstant.DELIMITER_STAR;
                    }
                    else
                    {
                        // When the char is invalid, replace it with space.
                        result += " ";
                    }

                    sourceIndex++;
                }
                else if (i < maskFormat.Length && maskFormat[i] == '-')
                {
                    result += "-";
                }
                else
                {
                    if (sourceIndex >= source.Length)
                    {
                        break;
                    }

                    result += source[sourceIndex];
                    sourceIndex++;
                }
            }

            return result;
        }

        /// <summary>
        /// Get the mask of the current agency from the standard choice.
        /// </summary>
        /// <param name="bizDomainItemKey">The standard choice item key</param>
        /// <returns>The mask from AA.</returns>
        public static string GetMaskFromAA(string bizDomainItemKey)
        {
            IBizdomainProvider bizProvider = ObjectFactory.GetObject<IBizdomainProvider>();
            IEnumerable<ItemValue> items = bizProvider.GetBizDomainList(BizDomainConstant.STD_MASKS);

            if (items == null)
            {
                return null;
            }

            var value = (from item in items where item != null && bizDomainItemKey.Equals(item.Key) select item.Value).FirstOrDefault();

            return value == null ? null : value.ToString();
        }

        #endregion public method

        #region Private method

        /// <summary>
        /// Convert the mask of AA(standard choice) when it include the single star(*) char which not included in '[]'. 
        /// In AA(standard choice), the single star(*) indicate this char can be any digit on input and must be hidden on view. 
        /// In <see cref="MaskConverter"/>, it only indicate any digit. 
        /// So, before using <see cref="MaskConverter"/>, should convert the single star(*) to '[*]'. 
        /// When the mask of AA(standard choice) no longer include the single star(*), this method can be removed.
        /// </summary>
        /// <param name="maskFromAA">The mask from AA.</param>
        /// <returns>The converted mask.</returns>
        private static string ConvertMaskFromAA(string maskFromAA)
        {
            if (string.IsNullOrEmpty(maskFromAA) || maskFromAA.IndexOf('*') < 0)
            {
                return maskFromAA;
            }

            string destMask = null;

            for (int i = 0; i < maskFromAA.Length; i++)
            {
                char currMask = maskFromAA[i];

                switch (currMask)
                {
                    case '[':
                        // Get the mask between '[' and ']'.
                        int multTypeEnd = maskFromAA.IndexOf(']', i);

                        // invalid mask: [
                        if (multTypeEnd < i) 
                        {
                            continue;
                        }

                        // invalid mask: []
                        if (multTypeEnd == i + 1)
                        {
                            i++;
                            continue;
                        }

                        // valid mask: [xxx]
                        destMask += maskFromAA.Substring(i, multTypeEnd - i + 1);

                        i = multTypeEnd;
                        break;
                    case ']':
                        // Invalid end flag of multiple type
                        continue;
                    case '*':
                        // Fix the single star to the correct format
                        destMask += "[*]";
                        break;
                    default:
                        destMask += currMask;
                        break;
                }
            }

            return destMask;
        }

        /// <summary>
        /// Validate whether the value in the specified mask's position is valid.
        /// </summary>
        /// <param name="mask">
        /// The ACA's mask. 
        /// 9 = only numeric 
        /// L,l = only letter 
        /// u = only lower letter 
        /// U = only upper letter 
        /// S = only numeric and letter 
        /// $ = only letter and spaces 
        /// C,c = only Custom - read from this._Filtered 
        /// A,a = only letter and Custom 
        /// N,n = only numeric and Custom 
        /// ? = any digit
        /// </param>
        /// <param name="position">The position check in the mask</param>
        /// <param name="maskValue">The checked value</param>
        /// <returns>true or false</returns>
        private static bool IsValidMaskValue(string mask, int position, string maskValue)
        {
            // Star(*) is valid char.
            if (ACAConstant.DELIMITER_STAR.Equals(maskValue))
            {
                return true;
            }

            bool result = true;

            // Ignore the custom mask.
            if (mask.Length > position)
            {
                switch (mask[position])
                {
                    case '9':
                    case 'N':
                    case 'n':
                        // 0-9
                        result = ValidationUtil.IsNumber(maskValue);
                        break;
                    case 'L':
                    case 'l':
                    case 'A':
                    case 'a':
                        // a-zA-Z
                        result = (new Regex("^(-?[a-zA-Z])$")).IsMatch(maskValue);
                        break;
                    case 'u':
                        // a-z
                        result = (new Regex("^(-?[a-z])$")).IsMatch(maskValue);
                        break;
                    case 'U':
                        // A-Z
                        result = (new Regex("^(-?[A-Z])$")).IsMatch(maskValue);
                        break;
                    case 'S':
                        // a-zA-Z0-9
                        result = (new Regex("^(-?[a-zA-Z0-9])$")).IsMatch(maskValue);
                        break;
                    case '$':
                        // a-zA-Z and spaces(\s)
                        result = (new Regex("^(-?[a-zA-Z\\s])$")).IsMatch(maskValue);
                        break;
                    case '?':
                        // any digit
                        break;
                    default:
                        // constant value
                        result = mask[position] == maskValue[0];
                        break;
                }
            }

            return result;
        }

        #endregion Private method
    }
}
