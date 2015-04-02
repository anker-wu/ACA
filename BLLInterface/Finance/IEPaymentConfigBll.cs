#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: IEPaymentConfigBll.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2009-2014
 *
 *  Description:
 *
 *  Notes:
 * $Id: IEPaymentConfigBll.cs 277347 2014-08-14 06:51:12Z ACHIEVO\james.shi $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using System.Collections;

using Accela.ACA.WSProxy;

namespace Accela.ACA.BLL.Finance
{
    /// <summary>
    /// Defines methods for EPayment config BLL.
    /// </summary>
    public interface IEPaymentConfigBll
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
        Hashtable GetEPaymentConfig(XPolicyModel policy);

        #endregion Methods
    }
}