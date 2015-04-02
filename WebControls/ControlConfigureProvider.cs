#region Header

/**
 * <pre>
 *
 *  Accela
 *  File: ControlConfigureProvider.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2008-2014
 *
 *  Description:
 *
 *  Notes:
 * $Id: ControlConfigureProvider.cs 278415 2014-09-03 08:45:04Z ACHIEVO\james.shi $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using System.Collections.Generic;

using Accela.ACA.Common;
using Accela.ACA.Common.Util;

namespace Accela.Web.Controls
{
    /// <summary>
    /// provide control configure
    /// </summary>
    public class ControlConfigureProvider
    {
        #region Methods

        /// <summary>
        /// Indicates whether the country code textbox enabled. the default value should be disable.
        /// </summary>
        /// <returns>true-country code textbox enabled, false - disabled</returns>
        public static bool IsCountryCodeEnabled()
        {
            return IsBizDomainEnable(BizDomainConstant.STD_CAT_PHONE_NUMBER_IDD_ENABLE, true, false); 
        }

        /// <summary>
        /// Indicates whether  enable the spell check
        /// </summary>
        /// <returns>true-enabled, false - disabled</returns>
        public static bool IsSpellCheckEnabled()
        {
            return IsBizDomainEnable(BizDomainConstant.STD_SPELL_CHECKER_ENABLED, true, false);
        }

        /// <summary>
        /// Indicates whether enable shopping cart.
        /// </summary>
        /// <returns>Enable shopping cart or not</returns>
        public static bool IsEnableShoppingCart()
        {
            IXPolicyProvider policyBll = ObjectFactory.GetObject<IXPolicyProvider>();
            string isEnableShoppingCart = policyBll.GetSingleValueByKey(XPolicyConstant.ACA_ENABLE_SHOPPING_CART);

            // if the value is 'Yes' or 'Y', display the user initial, otherwise display the user name.
            return ValidationUtil.IsYes(isEnableShoppingCart);
        }

        /// <summary>
        /// get customize page size defined in ACA Admin.
        /// </summary>
        /// <param name="moduleName">module name</param>
        /// <param name="gviewId">grid view id</param>
        /// <returns>page size</returns>
        public static string GetPageSize(string moduleName, string gviewId)
        {
            string customPageSize = string.Empty;
            string pageSizeKey = string.Format("{0}_{1}", ACAConstant.ACA_PAGE_SIZE, gviewId);

            IXPolicyProvider policyProvider = (IXPolicyProvider)ObjectFactory.GetObject(typeof(IXPolicyProvider)) as IXPolicyProvider;
            customPageSize = policyProvider.GetSingleValueByKey(pageSizeKey, moduleName);
            
            return customPageSize;
        }

        /// <summary>
        /// Indicates whether  key is yes
        /// </summary>
        /// <param name="category">standard choice key</param>
        /// <param name="readKey">get not the value but the key</param>
        /// <param name="onlyReadValue">get the value even if it is empty</param>
        /// <returns>true-enabled, false - disabled</returns>
        private static bool IsBizDomainEnable(string category, bool readKey, bool onlyReadValue)
        {
            IBizdomainProvider bizProvider = ObjectFactory.GetObject<IBizdomainProvider>();
            IList<ItemValue> items = bizProvider.GetBizDomainList(category);

            if (items != null && items.Count > 0 && items[0] != null)
            {
                string result = items[0].Key;

                if (onlyReadValue)
                {
                    result = items[0].Value == null ? string.Empty : items[0].Value.ToString();
                }
                else if (!readKey && items[0].Value != null)
                {
                    result = string.IsNullOrEmpty(items[0].Value.ToString()) ? items[0].Key : items[0].Value.ToString();
                }

                return ValidationUtil.IsYes(result);
            }

            return false;
        }

        #endregion Methods
    }
}