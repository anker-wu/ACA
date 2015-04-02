#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: ViewBll.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2008-2014
 *
 *  Description:
 *
 *  Notes:
 * $Id: ViewBll.cs 277347 2014-08-14 06:51:12Z ACHIEVO\james.shi $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using System.Collections;

using Accela.ACA.BLL.Common;
using Accela.ACA.Common;
using Accela.ACA.Common.Util;

namespace Accela.ACA.BLL.View
{
    /// <summary>
    /// This class provide the ability to operation view.
    /// </summary>
    public class ViewBll : BaseBll, IViewBll
    {
        #region Methods

        /// <summary>
        /// Get all label keys collection.
        /// </summary>
        /// <param name="agencyCode">agency code</param>
        /// <param name="cultureName">language culture type</param>
        /// <returns>label keys collection</returns>
        public Hashtable GetLabelKeys(string agencyCode, string cultureName)
        {
            ICacheManager cacheManager = ObjectFactory.GetObject<ICacheManager>();
            return cacheManager.GetCachedItem(agencyCode, agencyCode + ACAConstant.SPLIT_CHAR + I18nCultureUtil.AppendCultureFlag(CacheConstant.CACHE_KEY_LABEL, cultureName));
        }

        /// <summary>
        /// Get all label keys collection base.
        /// </summary>
        /// <param name="agencyCode">agency code</param>
        /// <returns>label keys collection</returns>
        public Hashtable GetLabelKeys(string agencyCode)
        {
            return GetLabelKeys(agencyCode, I18nCultureUtil.UserPreferredCulture);
        }

        #endregion Methods
    }
}