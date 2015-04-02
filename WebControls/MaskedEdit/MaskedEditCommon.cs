#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: MaskedEditCommon.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2008-2014
 *
 *  Description:
 *
 *  Notes:
 * $Id: MaskedEditCommon.cs 276801 2014-08-07 02:14:39Z ACHIEVO\james.shi $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */
//// (c) Copyright Microsoft Corporation.
//// This source is subject to the Microsoft Permissive License.
//// See http://www.microsoft.com/resources/sharedsource/licensingbasics/sharedsourcelicenses.mspx.
//// All other rights reserved.
////
//// Product      : MaskedEdit Extend/Validator Control
//// Version      : 1.0.0.0
//// Date         : 10/23/2006
//// Development  : Fernando Cerqueira
////

#endregion Header

using System;
using System.Globalization;
using System.Text;
using System.Web;

namespace AjaxControlToolkit
{
    /// <summary>
    /// Common class for masked control 
    /// </summary>
    public static class MaskedEditCommon
    {
        #region Fields

        /// <summary>
        /// string default culture
        /// </summary>
        private const string DefaultCulture = "en-US";

        /// <summary>
        /// editing mask
        /// </summary>
        private const string CHAR_EDIT_MASK = "9L$CAN?";

        /// <summary>
        /// special mask
        /// </summary>
        private const string CHAR_SPECIAL_MASK = "-/:.,";

        /// <summary>
        /// escape char
        /// </summary>
        private const string CHAR_ESCAPE = "\\";

        /// <summary>
        /// string for numbers 
        /// </summary>
        private const string CHAR_NUMBERS = "0123456789";

        #endregion Fields

        #region Methods

        /// <summary>
        /// Convert mask string
        /// </summary>
        /// <param name="text">string input text</param>
        /// <returns>converted string</returns>
        public static string ConvertMask(string text)
        {
            if (null == text)
            {
                throw new ArgumentNullException("text");
            }

            StringBuilder maskConv = new StringBuilder();
            StringBuilder qtdmask = new StringBuilder();
            string maskchar = string.Empty;
            int i = 0;

            for (i = 0; i < text.Length; i++)
            {
                if (CHAR_EDIT_MASK.IndexOf(text.Substring(i, 1)) != -1)
                {
                    if (qtdmask.Length == 0)
                    {
                        maskConv.Append(text.Substring(i, 1));
                        qtdmask.Length = 0;
                        maskchar = text.Substring(i, 1);
                    }
                    else if (text.Substring(i, 1) == "9")
                    {
                        qtdmask.Append("9");
                    }
                    else if (text.Substring(i, 1) == "0")
                    {
                        qtdmask.Append("0");
                    }
                }
                else if (CHAR_EDIT_MASK.IndexOf(text.Substring(i, 1)) == -1 && text.Substring(i, 1) != "{" && text.Substring(i, 1) != "}")
                {
                    if (qtdmask.Length == 0)
                    {
                        maskConv.Append(text.Substring(i, 1));
                        qtdmask.Length = 0;
                        maskchar = string.Empty;
                    }
                    else
                    {
                        if (CHAR_NUMBERS.IndexOf(text.Substring(i, 1)) != -1)
                        {
                            qtdmask.Append(text.Substring(i, 1));
                        }
                    }
                }
                else if (text.Substring(i, 1) == "{" && qtdmask.Length == 0)
                {
                    qtdmask.Length = 0;
                    qtdmask.Append("0");
                }
                else if (text.Substring(i, 1) == "}" && qtdmask.Length != 0)
                {
                    int qtddup = int.Parse(qtdmask.ToString(), CultureInfo.InvariantCulture) - 1;

                    if (qtddup > 0)
                    {
                        int q = 0;
                        for (q = 0; q < qtddup; q++)
                        {
                            maskConv.Append(maskchar);
                        }
                    }

                    qtdmask.Length = 0;
                    qtdmask.Append("0");
                    maskchar = string.Empty;
                }
            }

            return maskConv.ToString();
        }

        /// <summary>
        /// Gets culture browser info
        /// </summary>
        /// <param name="preferred">preferred culture</param>
        /// <returns>object CultureInfo</returns>
        public static CultureInfo GetCultureBrowser(string preferred)
        {
            CultureInfo culture = null;

            if (!string.IsNullOrEmpty(preferred))
            {
                culture = GetSpecificCultureInfo(preferred);
            }

            if (culture == null)
            {
                if (HttpContext.Current.Request.UserLanguages == null)
                {
                    culture = GetSpecificCultureInfo(DefaultCulture);
                }
                else
                {
                    foreach (string name in HttpContext.Current.Request.UserLanguages)
                    {
                        culture = GetSpecificCultureInfo(name);

                        if (culture != null)
                        {
                            break;
                        }
                    }
                }
            }

            return culture ?? GetSpecificCultureInfo(DefaultCulture);
        }

        /// <summary>
        /// Gets first mask position
        /// </summary>
        /// <param name="text">string text</param>
        /// <returns>first mask position</returns>
        public static int GetFirstMaskPosition(string text)
        {
            bool flagescape = false;
            text = ConvertMask(text);
            int i;

            for (i = 0; i < text.Length; i++)
            {
                if (text.Substring(i, 1) == CHAR_ESCAPE && !flagescape)
                {
                    flagescape = true;
                }
                else if (CHAR_EDIT_MASK.IndexOf(text.Substring(i, 1)) != -1 && !flagescape)
                {
                    return i;
                }
                else if (flagescape)
                {
                    flagescape = false;
                }
            }

            return -1;
        }

        /// <summary>
        /// Gets last mask position
        /// </summary>
        /// <param name="text">string text</param>
        /// <returns>last mask position</returns>
        public static int GetLastMaskPosition(string text)
        {
            bool flagescape = false;
            text = ConvertMask(text);
            int lastPos = -1;
            int i;

            for (i = 0; i < text.Length; i++)
            {
                if (text.Substring(i, 1) == CHAR_ESCAPE && !flagescape)
                {
                    flagescape = true;
                }
                else if (CHAR_EDIT_MASK.IndexOf(text.Substring(i, 1)) != -1 && !flagescape)
                {
                    lastPos = i;
                }
                else if (flagescape)
                {
                    flagescape = false;
                }
            }

            return lastPos;
        }

        /// <summary>
        /// Gets validate mask
        /// </summary>
        /// <param name="text">string text</param>
        /// <returns>validate mask string</returns>
        public static string GetValidMask(string text)
        {
            int i = 0;
            bool flagescape = false;
            StringBuilder _maskValid = new StringBuilder();
            text = ConvertMask(text);

            while (i < text.Length)
            {
                if (text.Substring(i, 1) == "\\" && flagescape == false)
                {
                    flagescape = true;
                }
                else if (CHAR_EDIT_MASK.IndexOf(text.Substring(i, 1)) == -1)
                {
                    if (flagescape)
                    {
                        flagescape = false;
                    }
                    else
                    {
                        if (CHAR_SPECIAL_MASK.IndexOf(text.Substring(i, 1)) != -1)
                        {
                            _maskValid.Append(text.Substring(i, 1));
                        }
                    }
                }
                else
                {
                    if (flagescape == true)
                    {
                        flagescape = false;
                    }
                    else
                    {
                        _maskValid.Append(text.Substring(i, 1));
                    }
                }

                i++;
            }

            return _maskValid.ToString();
        }

        /// <summary>
        /// Get a specific culture info
        /// </summary>
        /// <param name="name">Name of the culture</param>
        /// <returns>Specific CultureInfo, or null if no matching specific CultureInfo could be found</returns>
        private static CultureInfo GetSpecificCultureInfo(string name)
        {
            try
            {
                CultureInfo culture = CultureInfo.CreateSpecificCulture(name);

                if (culture != null && !culture.IsNeutralCulture)
                {
                    return culture;
                }
            }
            catch (ArgumentException)
            {
            }

            return null;
        }

        #endregion Methods
    }
}