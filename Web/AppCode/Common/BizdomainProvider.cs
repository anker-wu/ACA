#region Header

/**
 * <pre>
 *
 *  Accela
 *  File: BizdomainProvider.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2008-2014
 *
 *  Description:
 *
 *  Notes:
 * $Id: BizdomainProvider.cs 276901 2014-08-08 02:13:05Z ACHIEVO\james.shi $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using System.Collections.Generic;

using Accela.ACA.BLL.Common;
using Accela.ACA.Common;

namespace Accela.ACA.Web.Common
{
    /// <summary>
    /// Provider simple standard choice get approach.
    /// </summary>
    public class BizdomainProvider : IBizdomainProvider
    {
        #region Methods

        /// <summary>
        /// Method to get biz domain(standard choice) object list.
        /// </summary>
        /// <param name="bizName">biz domain name</param>
        /// <returns>ItemValue list.</returns>
        public IList<ItemValue> GetBizDomainList(string bizName)
        {
            IBizDomainBll bizBll = ObjectFactory.GetObject(typeof(IBizDomainBll)) as IBizDomainBll;
            IList<ItemValue> items = bizBll.GetBizDomainList(ConfigManager.AgencyCode, bizName, false);

            return items;
        }

        /// <summary>
        /// Gets specify single value by standard choice value under ACA_CONFIGS.
        /// </summary>
        /// <param name="key">standard choice value in ACA_CONFIGS category.</param>
        /// <returns>standard choice value, return string.Empty if the key doesn't be found</returns>
        public string GetSingleValueForACAConfigs(string key)
        {
            IBizDomainBll bizBll = ObjectFactory.GetObject(typeof(IBizDomainBll)) as IBizDomainBll;
            string singleValue = bizBll.GetValueForACAConfig(ConfigManager.AgencyCode, key);

            return singleValue;
        }

        /// <summary>
        /// Get the standard choice item value of the biz domain of the current agency.
        /// </summary>
        /// <param name="bizDomain">biz domain name</param>
        /// <param name="bizDomainItemKey">The standard choice item key</param>
        /// <returns>The standard choice item description.</returns>
        public string GetBizDomainItemDesc(string bizDomain, string bizDomainItemKey)
        {
            IBizDomainBll bizBll = ObjectFactory.GetObject<IBizDomainBll>();

            return bizBll.GetBizDomainItemDesc(ConfigManager.AgencyCode, bizDomain, bizDomainItemKey);
        }

        #endregion Methods
    }
}