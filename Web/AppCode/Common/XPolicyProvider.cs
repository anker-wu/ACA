#region Header

/**
 * <pre>
 *
 *  Accela
 *  File: BizdomainProvider.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2009-2014
 *
 *  Description:
 *
 *  Notes:
 * $Id: XPolicyProvider.cs 277071 2014-08-11 03:38:26Z ACHIEVO\james.shi $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using Accela.ACA.BLL.Common;
using Accela.ACA.Common;

namespace Accela.ACA.Web.Common
{
    /// <summary>
    /// Provider simple XPolicy item get approach.
    /// </summary>
    public class XPolicyProvider : IXPolicyProvider
    {
        #region Methods

        /// <summary>
        /// get XPolicy value by key
        /// </summary>
        /// <param name="key">XPolicy key</param>
        /// <returns>The XPolicy value.</returns>
        public string GetSingleValueByKey(string key)
        {
            IXPolicyBll policyBll = ObjectFactory.GetObject<IXPolicyBll>();
            string singleValue = policyBll.GetValueByKey(key);

            return singleValue;
        }

        /// <summary>
        /// get XPolicy value by key
        /// </summary>
        /// <param name="key">XPolicy key</param>
        /// <param name="moduleName">current module</param>
        /// <returns>XPolicy value</returns>
        public string GetSingleValueByKey(string key, string moduleName)
        {
            IXPolicyBll policyBll = (IXPolicyBll)ObjectFactory.GetObject(typeof(IXPolicyBll));
            string singleValue = policyBll.GetValueByKey(key, moduleName);

            return singleValue;
        }

        /// <summary>
        /// overloaded function
        /// </summary>
        /// <param name="key">XPolicy key</param>
        /// <param name="levelType">global,agency or module</param>
        /// <param name="levelData">real level data</param>
        /// <returns>XPolicy value</returns>
        public string GetSingleValueByKey(string key, string levelType, string levelData)
        {
            IXPolicyBll policyBll = (IXPolicyBll)ObjectFactory.GetObject(typeof(IXPolicyBll));
            string singleValue = policyBll.GetValueByKey(key, levelType, levelData);

            return singleValue;
        }

        #endregion Methods
    }
}