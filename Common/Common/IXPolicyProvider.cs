/**
 * <pre>
 *
 *  Accela
 *  File: IBizdomainProvider.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2009-2014
 *
 *  Description:
 *
 *  Notes:
 * $Id: IXPolicyProvider.cs 276545 2014-08-04 02:15:30Z ACHIEVO\james.shi $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */
namespace Accela.ACA.Common
{
    /// <summary>
    /// Defined an interface for IXPolicy Provider.
    /// </summary>
    public interface IXPolicyProvider
    {
        #region Methods

        /// <summary>
        /// Gets specify single value by XPolicy key under ACA_CONFIGS.
        /// </summary>
        /// <param name="key">XPolicy key in ACA_CONFIGS category.</param>
        /// <returns>XPolicy item value, return string.Empty if the key doesn't be found</returns>
        string GetSingleValueByKey(string key);

        /// <summary>
        /// get XPolicy value by key
        /// </summary>
        /// <param name="key">XPolicy key</param>
        /// <param name="moduleName">current module</param>
        /// <returns>XPolicy value</returns>
        string GetSingleValueByKey(string key, string moduleName);

        /// <summary>
        /// Get single value by XPolicy key.
        /// </summary>
        /// <param name="key">policy key</param>
        /// <param name="levelType">level type</param>
        /// <param name="levelData">level data</param>
        /// <returns>XPolicy value</returns>
        string GetSingleValueByKey(string key, string levelType, string levelData);

        #endregion Methods
    }
}