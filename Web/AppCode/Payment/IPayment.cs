#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: IPayment.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2009-2014
 *
 *  Description:
 *  IPayment interface..
 *
 *  Notes:
 * $Id: IPayment.cs 277174 2014-08-12 07:38:31Z ACHIEVO\james.shi $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using System.Collections;
using System.Web.UI;

using Accela.ACA.WSProxy;

namespace Accela.ACA.Web.Payment
{
    /// <summary>
    /// payment interface
    /// </summary>
    public interface IPayment
    {
        #region Methods

        /// <summary>
        /// Complete payment
        /// </summary>
        /// <param name="currentPage">current Page</param>
        /// <returns>The online payment result model</returns>
        OnlinePaymentResultModel CompletePayment(Page currentPage);

        /// <summary>
        /// Get the 3rd page height to make ACA iFrame page meet the 3rd page height automatically.
        /// </summary>
        /// <returns>The 3rd page height.</returns>
        int Get3rdPageHeight();

        /// <summary>
        /// Get payment config
        /// </summary>
        /// <param name="policy">the XPolicy</param>
        /// <returns>XPolicy hashtable.</returns>
        Hashtable GetEPaymentConfig(XPolicyModel policy);

        /// <summary>
        /// Initiate payment
        /// </summary>
        /// <param name="currentPage">current Page</param>
        /// <returns>The online payment result model</returns>
        OnlinePaymentResultModel InitiatePayment(Page currentPage);

        /// <summary>
        /// Verify payment
        /// </summary>
        /// <param name="currentPage">current Page</param>
        /// <returns>The online payment result model</returns>
        OnlinePaymentResultModel VerifyPayment(Page currentPage);

        #endregion Methods
    }
}