#region Header

/**
* <pre>
*
*  Accela Citizen Access
*  File: LabelConvertUtil.cs
*
*  Accela, Inc.
*  Copyright (C): 2008-2014
*
*  Description:
*  An util class for web control to get label text by label key.
*
*  Notes:
*      $Id: LabelConvertUtil.cs 276801 2014-08-07 02:14:39Z ACHIEVO\james.shi $.
*  Revision History
*  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
* </pre>
*/

#endregion Header

using System;
using System.Text.RegularExpressions;
using System.Web.UI.WebControls;

using Accela.ACA.Common;

namespace Accela.Web.Controls
{
    /// <summary>
    /// An utility class for web control to get label text by label key.
    /// </summary>
    internal static class LabelConvertUtil
    {
        #region Methods

        /// <summary>
        /// Get the default language global text(label message) by label key.
        /// </summary>
        /// <param name="key">label key which is unique.</param>
        /// <returns>The global text(label message) according the key.if can't find the key, it should return 'N/A'.</returns>
        internal static string GetDefaultLanguageGlobalTextByKey(string key)
        {
            ILabelConvertor lc = LabelConvertorFactory.Instant.GetLabelConvertor();

            return lc.GetDefaultLanguageGlobalTextByKey(key);
        }

        /// <summary>
        /// Get the default language global text(label message) by label key.
        /// </summary>
        /// <param name="key">label key which is unique.</param>
        /// <param name="moduleName">string module name.</param>
        /// <returns>The text(label message) according the key.</returns>
        internal static string GetDefaultLanguageTextByKey(string key, string moduleName)
        {
            ILabelConvertor lc = LabelConvertorFactory.Instant.GetLabelConvertor();

            return lc.GetDefaultLanguageTextByKey(key, moduleName);
        }

        /// <summary>
        /// Get the global text(label message) by label key.
        /// </summary>
        /// <param name="key">label key which is unique.</param>
        /// <returns>The global text(label message) according the key.if can't find the key, it should return 'N/A'.</returns>
        internal static string GetGlobalTextByKey(string key)
        {
            ILabelConvertor lc = LabelConvertorFactory.Instant.GetLabelConvertor();

            return lc.GetGlobalTextByKey(key);
        }

        /// <summary>
        /// Get the GUI text by label key.
        /// </summary>
        /// <param name="key">label key which is unique.</param>
        /// <returns>get GUI text by label key.</returns>
        internal static string GetGUITextByKey(string key)
        {
            ILabelConvertor lc = LabelConvertorFactory.Instant.GetLabelConvertor();

            return lc.GetGUITextByKey(key);
        }

        /// <summary>
        /// Get the text(label message) by label key.
        /// </summary>
        /// <param name="key">label key which is unique.</param>
        /// <returns>The text(label message) according the key.if can't find the key</returns>
        /// <param name="moduleName">module name.</param>
        internal static string GetTextByKey(string key, string moduleName)
        {
            ILabelConvertor lc = LabelConvertorFactory.Instant.GetLabelConvertor();

            return lc.GetTextByKey(key, moduleName);
        }

        /// <summary>
        /// Get the text(label message) by label key.
        /// </summary>
        /// <param name="key">label key which is unique.</param>
        /// <param name="control">the control which has the key</param>
        /// <returns>the content of the key</returns>
        internal static string GetTextByKey(string key, WebControl control)
        {
            if (control.Page is IPage)
            {
                return ((IPage)control.Page).GetTextByKey(key);
            }
            else
            {
                return GetTextByKey(key, string.Empty);
            }
        }

        /// <summary>
        /// Get the text(label message) by label key.
        /// </summary>
        /// <param name="key">label key which is unique.</param>
        /// <param name="moduleName">Module Name</param>
        /// <param name="control">the control which has the key</param>
        /// <returns>the content of the key</returns>
        internal static string GetTextByKey(string key, string moduleName, WebControl control)
        {
            if (control.Page is IPage)
            {
                return ((IPage)control.Page).GetTextByKey(key, moduleName);
            }
            else
            {
                return GetTextByKey(key, string.Empty);
            }
        }

        /// <summary>
        /// Get the text(label message) by label key.
        /// </summary>
        /// <param name="key">label key which is unique.</param>
        /// <param name="control">the control which has the key</param>
        /// <returns>the content of the key</returns>
        internal static string GetSuperAgencyTextByKey(string key, WebControl control)
        {
            if (control.Page is IPage)
            {
                return ((IPage)control.Page).GetSuperAgencyTextByKey(key);
            }
            else
            {
                return GetTextByKey(key, string.Empty);
            }
        }

        /// <summary>
        /// Remove html format
        /// </summary>
        /// <param name="value">the input value</param>
        /// <returns>the value without html tag</returns>
        internal static string RemoveHtmlFormat(string value)
        {
            string result = value;

            if (!string.IsNullOrEmpty(value) && value.IndexOf("<", StringComparison.Ordinal) > -1)
            {
                string pattern = @"(?></?\w+)(?>(?:[^<>'""]+|'[^']*'|""[^""]*"")*)>";
                Regex reg = new Regex(pattern, RegexOptions.Compiled);
                result = reg.Replace(value, string.Empty);
            }

            return result;
        }

        #endregion Methods
    }
}