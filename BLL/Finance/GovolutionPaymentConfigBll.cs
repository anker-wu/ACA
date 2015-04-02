#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: GovolutionPaymentConfigBll.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2009-2014
 *
 *  Description:
 *
 *  Notes:
 * $Id: GovolutionPaymentConfigBll.cs 277347 2014-08-14 06:51:12Z ACHIEVO\james.shi $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using System;
using System.Collections;

using Accela.ACA.Common;
using Accela.ACA.WSProxy;

namespace Accela.ACA.BLL.Finance
{
    /// <summary>
    /// This class provide the ability to operation payment config.
    /// </summary>
    internal class GovolutionPaymentConfigBll : IEPaymentConfigBll
    {
        #region Methods

        /// <summary>
        /// get the e payment configuration
        /// </summary>
        /// <param name="policy"> a policy model</param>
        /// <returns>
        /// the returned hashtable contains the following key-values:
        /// Adapter=specific adapter name
        /// Online_URL=specific online URL
        /// ApplicationID=specific application id
        /// ApplicationVersion=specific application version
        /// </returns>
        Hashtable IEPaymentConfigBll.GetEPaymentConfig(XPolicyModel policy)
        {
            if (policy == null)
            {
                return null;
            }

            Hashtable items = new Hashtable(StringComparer.InvariantCultureIgnoreCase);
            DataUtil.ParseString(items, policy.data1, ';', '=');
            DataUtil.ParseString(items, policy.data4, ';', '=');

            return items;
        }

        #endregion Methods
    }
}