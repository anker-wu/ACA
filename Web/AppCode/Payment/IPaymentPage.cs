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
 * $Id: IPaymentPage.cs 277174 2014-08-12 07:38:31Z ACHIEVO\james.shi $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using Accela.ACA.Common;
using Accela.ACA.WSProxy;

namespace Accela.ACA.Web.Payment
{
    /// <summary>
    /// payment interface
    /// </summary>
    public interface IPaymentPage
    {
        #region Methods

        /// <summary>
        /// Gets the payment entity type
        /// </summary>
        ACAConstant.PaymentEntityType PaymentEntityType { get; }

        /// <summary>
        /// Construct a CheckModel4WS from user's input
        /// </summary>
        /// <returns>The check model.</returns>
        CheckModel4WS GetCheckModel();

        /// <summary>
        /// Construct a CreditCardModel4WS from user's input
        /// </summary>
        /// <returns>The credit card model.</returns>
        CreditCardModel4WS GetCreditCardModel();

        /// <summary>
        /// Get the payment method(credit card, check, trust account)
        /// </summary>
        /// <returns>The payment method.</returns>
        PaymentMethod GetPaymentMethod();

        /// <summary>
        /// Get total fee for the shopping cart items
        /// </summary>
        /// <returns>The total fee.</returns>
        double GetTotalFee();

        /// <summary>
        /// Construct a TrustAccountModel from user's input
        /// </summary>
        /// <returns>The trust account model.</returns>
        TrustAccountModel GetTrustAccountModel();
        
        #endregion Methods
    }
}
