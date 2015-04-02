/**
* <pre>
*
*  Accela Citizen Access
*  File: ILabelConvertor.cs
*
*  Accela, Inc.
*  Copyright (C): 2008-2014
*
*  Description:
*  Label Convertor interface.Convert the key to text for web control to display correctlly.
*
*  Notes:
*      $Id: ILabelConvertor.cs 276801 2014-08-07 02:14:39Z ACHIEVO\james.shi $.
*  Revision History
*  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
* </pre>
*/
namespace Accela.Web.Controls
{
    /// <summary>
    /// Label Convertor interface.Convert the key to text for web control to display correctly.
    /// </summary>
    public interface ILabelConvertor
    {
        #region Methods

        /// <summary>
        /// Get the default language global text(label message) by label key.
        /// </summary>
        /// <param name="key">label key which is unique.</param>
        /// <returns>The default language global text(label message) according the key.if can't find the key, it should return 'N/A'.</returns>
        string GetDefaultLanguageGlobalTextByKey(string key);

        /// <summary>
        /// Get the global text(label message) by label key.
        /// </summary>
        /// <param name="key">label key which is unique.</param>
        /// <returns>The global text(label message) according the key.if can't find the key, it should return 'N/A'.</returns>
        string GetGlobalTextByKey(string key);

        /// <summary>
        /// Get the GUI text by label key.
        /// </summary>
        /// <param name="key">label key which is unique.</param>
        /// <returns>get GUI text by label key.</returns>
        string GetGUITextByKey(string key);

        /// <summary>
        /// Get the text(label message) by label key.
        /// </summary>
        /// <param name="key">label key which is unique.</param>
        /// <param name="moduleName">module name.</param>
        /// <returns>The text(label message) according the key.if can't find the key, it should return 'N/A'.</returns>
        string GetTextByKey(string key, string moduleName);

        /// <summary>
        /// Get the default language text(label message) by label key.
        /// </summary>
        /// <param name="key">string label key.</param>
        /// <param name="moduleName">string module name.</param>
        /// <returns>label key value in default language.</returns>
        string GetDefaultLanguageTextByKey(string key, string moduleName);

        #endregion Methods
    }
}