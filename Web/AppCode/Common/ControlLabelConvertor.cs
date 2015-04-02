#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: ControlLabelConvertor.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2008-2014
 *
 *  Description:
 *  A Implement of ILabelConvertor in ACA.
 *  This class will be invoked mainly by LabelConvertorFactory in Accela.Web.Controls.dll assembly dynamically.
 *  UI code should invoke LabelUtil class but not invoke this class.
 *
 *  Notes:
 *      $Id: ControlLabelConvertor.cs 277071 2014-08-11 03:38:26Z ACHIEVO\james.shi $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using Accela.Web.Controls;

namespace Accela.ACA.Web.Common
{
    /// <summary>
    /// A Implement of ILabelConvertor in ACA. 
    /// This class will be invoked mainly by LabelConvertorFactory in <c>Accela.Web.Controls.dll</c> assembly dynamically. 
    /// UI code should invoke <see cref="LabelUtil"/> class but not invoke this class.
    /// </summary>
    public class ControlLabelConvertor : ILabelConvertor
    {
        #region Methods

        /// <summary>
        /// Get the default language global text(label message) by label key.
        /// </summary>
        /// <param name="key">label key which is unique.</param>
        /// <returns>The default language global text(label message) according the key.if can't find the key, it should return 'N/A'.</returns>
        public string GetDefaultLanguageGlobalTextByKey(string key)
        {
            return LabelUtil.GetDefaultLanguageGlobalTextByKey(key);
        }

        /// <summary>
        /// Get the global text(label message) by label key.
        /// </summary>
        /// <param name="key">label key which is unique.</param>
        /// <returns>The global text(label message) according the key.if can't find the key, it should return 'N/A'.</returns>
        public string GetGlobalTextByKey(string key)
        {
            return LabelUtil.GetGlobalTextByKey(key);
        }

        /// <summary>
        /// Get the GUI text by label key.
        /// </summary>
        /// <param name="key">label key which is unique.</param>
        /// <returns>get GUI text by label key.</returns>
        public string GetGUITextByKey(string key)
        {
            return LabelUtil.GetGUITextByKey(key);
        }

        /// <summary>
        /// Get the text(label message) by label key.
        /// </summary>
        /// <param name="key">label key which is unique.</param>
        /// <param name="moduleName">module name.</param>
        /// <returns>The text(label message) according the key.if can't find the key, it should return 'N/A'.</returns>
        public string GetTextByKey(string key, string moduleName)
        {
            return LabelUtil.GetTextByKey(key, moduleName);
        }

        /// <summary>
        /// Get the default language text(label message) by label key.
        /// </summary>
        /// <param name="key">string label key.</param>
        /// <param name="moduleName">string module name.</param>
        /// <returns>label key value in default language.</returns>
        public string GetDefaultLanguageTextByKey(string key, string moduleName)
        {
            return LabelUtil.GetDefaultLanguageTextByKey(key, moduleName);
        }

        #endregion Methods
    }
}