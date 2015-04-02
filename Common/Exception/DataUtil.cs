#region Header

/**
 *  Accela Citizen Access
 *  File: DataUtil.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2008-2014
 *
 *  Description:
 *   It provide the data related utility to serve the framework.
 *
 *  Notes:
 * $Id: DataUtil.cs 276545 2014-08-04 02:15:30Z ACHIEVO\james.shi $.
 *  Revision History
 *  Date,                  Who,                 What
 *  05/17/2007             troy.yang            Initial.
 */

#endregion Header

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Accela.ACA.Common
{
    /// <summary>
    /// Provide some methods to handle data
    /// </summary>
    public static class DataUtil
    {
        #region Methods

        /// <summary>
        /// add a blank to string before the uppercase letter except the first one
        /// </summary>
        /// <param name="value">content that need add blank</param>
        /// <returns>string that has blank</returns>
        public static string AddBlankToString(string value)
        {
            if (!string.IsNullOrEmpty(value))
            {
                value = value.Trim();
                if (value.Length > 1)
                {
                    for (int i = 1; i < value.Length; i++)
                    {
                        int ascii = (int)value[i];

                        if (ascii > 64 && ascii < 91)
                        {
                            int ascii2 = (int)value[i - 1];

                            //If current char is in A~Z and previous char is in a~z, insert a space (Bug #40747).
                            if (ascii2 != 32 && (ascii2 >= 97 && ascii2 <= 122))
                            {
                                value = value.Insert(i, " ");
                            }
                        }
                    }
                }
            }

            return value;
        }

        /// <summary>
        /// Union two string arrays.
        /// </summary>
        /// <param name="array1">first string array</param>
        /// <param name="array2">second string array</param>
        /// <returns>IList that combined the two arrays</returns>
        public static IList<string> GetUnionArrays(string[] array1, string[] array2)
        {
            IList<string> unionList = new List<string>();

            if (array1 == null || array1.Length == 0)
            {
                unionList = array2;
            }
            else
            {
                // Union the two arrays
                foreach (string s in array1)
                {
                    unionList.Add(s);
                }

                if (array2 != null && array2.Length > 0)
                {
                    foreach (string str in array2)
                    {
                        if (!unionList.Contains(str))
                        {
                            unionList.Add(str);
                        }
                    }
                }
            }

            return unionList;
        }

        /// <summary>
        /// Parse string
        /// </summary>
        /// <param name="items">a Hashtable</param>
        /// <param name="input">input string</param>
        /// <param name="firstSplitChar">first split char</param>
        /// <param name="secondSplitChar">second split char</param>
        /// <param name="thirdSplitChar">third split char</param>
        public static void ParseString(Hashtable items, string input, char firstSplitChar, char secondSplitChar, char thirdSplitChar)
        {
            if (!string.IsNullOrEmpty(input))
            {
                string[] data = input.Split(firstSplitChar);

                if (data.Length == 2)
                {
                    Hashtable child = new Hashtable();
                    ParseString(child, data[1], secondSplitChar, thirdSplitChar);
                    items.Add(data[0].Trim(), child);
                }
            }
        }

        /// <summary>
        /// Parse String
        /// </summary>
        /// <param name="items">Hash table item</param>
        /// <param name="input">the input string</param>
        /// <param name="firstSplitChar">first split char</param>
        /// <param name="secondSplitChar">second split char</param>
        public static void ParseString(Hashtable items, string input, char firstSplitChar, char secondSplitChar)
        {
            if (!string.IsNullOrEmpty(input))
            {
                string[] data = input.Split(firstSplitChar);

                foreach (string d in data)
                {
                    if (!string.IsNullOrEmpty(d))
                    {
                        ParseString(items, d, secondSplitChar);
                    }
                }
            }
        }

        /// <summary>
        /// Parse string
        /// </summary>
        /// <param name="items">Hash table item</param>
        /// <param name="input">the input string</param>
        /// <param name="firstSplitChar">first split char</param>
        public static void ParseString(Hashtable items, string input, char firstSplitChar)
        {
            if (!string.IsNullOrEmpty(input))
            {
                string[] kv = input.Split(firstSplitChar);

                if (kv.Length == 2)
                {
                    if (!items.ContainsKey(kv[0]))
                    {
                        items.Add(kv[0].Trim(), kv[1].Trim());
                    }
                }
            }
        }

        /// <summary>
        /// Replaces the format item in a specified System.string with the text equivalent
        /// of the value of a specified System.string value.
        /// </summary>
        /// <param name="originalString">require to processing string</param>
        /// <param name="args">string element</param>
        /// <returns>finally string value.</returns>
        public static string StringFormat(string originalString, params object[] args)
        {
            if (args == null || originalString == null)
            {
                return originalString;
            }

            string result = originalString;

            for (int i = 0; i < args.Length; i++)
            {
                string arg = string.Empty;

                if (args[i] != null)
                {
                    arg = args[i].ToString();
                }

                string tempStr = "{" + i.ToString() + "}";

                result = result.Replace(tempStr, arg);
            }

            return result;
        }

        /// <summary>
        /// concatenate the string with blank.
        /// e.g. if args[0] = "a",args[1]="b",args[2]="c", the result will be "a b c";
        /// if args[0] = "",args[1]="b",args[2]="c", the result will be "b c";
        /// </summary>
        /// <param name="args">string element</param>
        /// <param name="splitChar">The split char,such as a hyphen or a blank.</param>
        /// <returns>finally string value.</returns>
        public static string ConcatStringWithSplitChar(IEnumerable<string> args, string splitChar)
        {
            if (args == null || args.Count() == 0)
            {
                return string.Empty;
            }

            List<string> list = new List<string>();

            foreach (string argument in args)
            {
                if (!string.IsNullOrEmpty(argument))
                {
                    list.Add(argument);
                }
            }

            if (list.Count == 0)
            {
                return string.Empty;
            }

            StringBuilder formatString = new StringBuilder();

            for (int i = 0; i < list.Count; i++)
            {
                if (i > 0)
                {
                    formatString.Append(splitChar);
                }

                formatString.Append("{");
                formatString.Append(i.ToString());
                formatString.Append("}");
            }

            return string.Format(formatString.ToString(), list.ToArray());
        }

        /// <summary>
        /// Truncate string
        /// </summary>
        /// <param name="str">the string need truncated</param>
        /// <param name="length">the length</param>
        /// <returns>string been truncated</returns>
        public static string TruncateString(string str, int length)
        {
            if (!string.IsNullOrEmpty(str) && length != 0 && str.Length > length)
            {
                str = str.Substring(0, length) + "...";
            }

            return str;
        }

        /// <summary>
        /// Filter escape characters.
        /// </summary>
        /// <param name="s">string content</param>
        /// <returns>string that has been filtered escape char</returns>
        public static string FilterEscapeChars(string s)
        {
            if (!string.IsNullOrEmpty(s))
            {
                s = Regex.Replace(s, "\n", " ", RegexOptions.CultureInvariant);
                s = Regex.Replace(s, "\t", " ", RegexOptions.CultureInvariant);
                s = Regex.Replace(s, "\v", " ", RegexOptions.CultureInvariant);
                s = Regex.Replace(s, "\b", " ", RegexOptions.CultureInvariant);
                s = Regex.Replace(s, "\r", " ", RegexOptions.CultureInvariant);
                s = Regex.Replace(s, "\f", " ", RegexOptions.CultureInvariant);
                s = Regex.Replace(s, "\a", " ", RegexOptions.CultureInvariant);
            }

            return s;
        }

        /// <summary>
        /// Gets format message keys, such as [serviceProviderCode,capID]
        /// </summary>
        /// <param name="array">string array</param>
        /// <returns>formatted string</returns>
        public static string FormatArray(string[] array)
        {
            if (array == null)
            {
                return string.Empty;
            }

            string result = "[";

            foreach (string s in array)
            {
                result += s + ", ";
            }

            int index = result.LastIndexOf(',');

            if (index > 0)
            {
                result = result.Substring(0, index);
            }

            result += "]";

            return result;
        }

        /// <summary>
        /// Concatenate string list to string with comma and black.
        /// </summary>
        /// <param name="strList">string list</param>
        /// <returns>connected string</returns>
        public static string ConcatStringListWithComma(IList<string> strList)
        {
            if (strList == null || strList.Count == 0)
            {
                return string.Empty;
            }

            StringBuilder sbConcat = new StringBuilder();

            foreach (string str in strList)
            {
                sbConcat.Append(str);
                sbConcat.Append(ACAConstant.COMMA_BLANK);
            }

            if (sbConcat.Length > 1)
            {
                sbConcat.Remove(sbConcat.Length - 2, 2); // remove the last comma and blank.
            }

            return sbConcat.ToString();
        }

        #endregion Methods
    }
}