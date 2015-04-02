#region Header

/**
 * <pre>
 *
 *  Accela
 *  File: IBizdomainProvider.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2008-2014
 *
 *  Description:
 *
 *  Notes:
 * $Id: IBizdomainProvider.cs 276545 2014-08-04 02:15:30Z ACHIEVO\james.shi $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using System.Collections.Generic;

namespace Accela.ACA.Common
{
    /// <summary>
    /// provides interface for BIZ domain provider. 
    /// </summary>
    public interface IBizdomainProvider
    {
        #region Methods

        /// <summary>
        /// Method to get biz domain(standard choice) object list.
        /// </summary>
        /// <param name="bizName">biz domain name</param>
        /// <returns>ItemValue list.</returns>
        IList<ItemValue> GetBizDomainList(string bizName);

        /// <summary>
        /// Gets specify single value by standard choice value under ACA_CONFIGS.
        /// </summary>
        /// <param name="key">standard choice value in ACA_CONFIGS category.</param>
        /// <returns>standard choice value, return string.Empty if the key doesn't be found</returns>
        string GetSingleValueForACAConfigs(string key);

        /// <summary>
        /// Get the standard choice item value of the biz domain of the current agency.
        /// </summary>
        /// <param name="bizDomain">biz domain name</param>
        /// <param name="bizDomainItemKey">The standard choice item key</param>
        /// <returns>The standard choice item description</returns>
        string GetBizDomainItemDesc(string bizDomain, string bizDomainItemKey);

        #endregion Methods
    }
}