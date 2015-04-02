#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: IOnlinePaymenBll.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2008-2014
 *
 *  Description:
 *
 *  Notes:
 * $Id: IOnlinePaymenBll.cs 277347 2014-08-14 06:51:12Z ACHIEVO\james.shi $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 *   10/12/2007      Daly zeng
 * </pre>
 */

#endregion Header

using Accela.ACA.WSProxy;

namespace Accela.ACA.BLL.Finance
{
    /// <summary>
    /// Defines credit card, on line payment and EMSE methods for payment.
    /// </summary>
    public interface IOnlinePaymenBll
    {
        #region Methods

        /// <summary>
        /// Calculate Convenience Fee
        /// </summary>
        /// <param name="agencyCode">The agency code.</param>
        /// <param name="amount">the amount of charge</param>
        /// <param name="paymentMethod">The payment method, Credit Card/Check.</param>
        /// <param name="merchantAccountID">The merchant account ID.</param>
        /// <returns>credit card entity</returns>
        CreditCardModel4WS CalculateConvenienceFee(string agencyCode, double amount, string paymentMethod, string merchantAccountID);

        /// <summary>
        /// Calculate Convenience Fee
        /// </summary>
        /// <param name="servProvCode">Agency code</param>
        /// <param name="adapterName">the payment adapter name</param>
        /// <param name="capIds">the CapIDModel array</param>
        /// <param name="paymentMethod">The payment method, Credit Card/Check.</param>
        /// <param name="merchantAccountID">The merchant account ID.</param>
        /// <returns>credit card entity</returns>
        CreditCardModel4WS CalculateConvenienceFeeForShoppingCart(string servProvCode, string adapterName, CapIDModel[] capIds, string paymentMethod, string merchantAccountID);

        /// <summary>
        /// Call EMSE to pay online.
        /// </summary>
        /// <param name="capIDModel">CapIDModel4WS entity</param>
        /// <param name="acaModel">ACAModel4WS entity</param>
        /// <param name="urlParameters">url Parameters</param>
        /// <param name="callerID">caller ID number</param>
        /// <returns>EMSE Result entity</returns>
        EMSEResultBaseModel4WS CallEMSEtoPayOnline(CapIDModel4WS capIDModel, ACAModel4WS acaModel, string urlParameters, string callerID);

        /// <summary>
        ///  Credit payment for self plan checking.
        /// </summary>
        /// <param name="ccModel">Credit Card entity</param>
        /// <param name="paymentModel">payment entity</param>
        /// <returns>Online Payment result entity</returns>
        OnLinePaymentReturnInfo4WS CreditCardPayment4PlanReview(CreditCardModel4WS ccModel, PaymentModel paymentModel);

        /// <summary>
        /// Gets receipt number by cap ID. 
        /// </summary>
        /// <param name="capID">cap ID</param>
        /// <param name="qf">query format</param>
        /// <param name="callerID">caller ID</param>
        /// <returns>receipt number</returns>
        string GetReceiptNoByCapID(CapIDModel4WS capID, QueryFormat4WS qf, string callerID);

        /// <summary>
        /// Online payment process
        /// </summary>
        /// <param name="capIDModel">CapIDModel4WS entity</param>
        /// <param name="acaModel">ACAModel entity</param>
        /// <param name="notificationURL">notificationURL string</param>
        /// <param name="callerID">callerID number</param>
        /// <returns>EMSEResultBaseModel4WS entity</returns>
        EMSEResultBaseModel4WS OnlinePaymentProcess(CapIDModel4WS capIDModel, ACAModel4WS acaModel, string notificationURL, string callerID);

        /// <summary>
        /// Get the merchant account by cap type.
        /// </summary>
        /// <param name="capTypeModel">The cap type, the agency code/group/type/sub-type/category are required.</param>
        /// <param name="needDecrypt">The merchant account need decrypt or not.</param>
        /// <returns>return the merchant account.</returns>
        RefMerchantAccountModel SearchMerchantAccount(CapTypeModel capTypeModel, bool needDecrypt);

        /// <summary>
        /// Update the transactions' status to Fail
        /// </summary>
        /// <param name="servProvCode">the agency code</param>
        /// <param name="batchTransCode">The batch transaction number</param>
        /// <param name="errorMessage">error message from payment</param>
        void UpdateFailStatus4AllTransactions(string servProvCode, long batchTransCode, string errorMessage);

        #endregion Methods
    }
}