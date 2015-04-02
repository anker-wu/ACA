#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: ICommonPaymentBll.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2009-2014
 *
 *  Description:
 *
 *  Notes:
 * $Id: ICommonPaymentBll.cs 131464 2009-05-20 01:42:02Z ACHIEVO\jackie.yu $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 *   10/12/2007      Daly zeng
 * </pre>
 */

#endregion Header

using System;

using Accela.ACA.WSProxy;

namespace Accela.ACA.BLL.Finance
{
    /// <summary>
    /// common payment interface
    /// </summary>
    public interface ICommonPaymentBll
    {
        /// <summary>
        /// initialize online payment
        /// </summary>
        /// <param name="capIDModelArray">the capID model</param>
        /// <param name="parameters">Payment Extra Parameters</param>
        /// <param name="paymentProvider">adapter name. e.g. <c>Govolution</c></param>
        /// <returns>list of the TransactionModel</returns>
        TransactionModel[] InitialOnlinePayment(CapIDModel[] capIDModelArray, string parameters, string paymentProvider);

        /// <summary>
        /// deal with the complete payment process
        /// </summary>
        /// <param name="servProvCode">server provider code</param>
        /// <param name="paymentModel">the payment model</param>
        /// <param name="parameters">Payment Extra Parameters</param>
        /// <param name="callerID">Method Invoker</param>
        /// <returns>the result of online payment</returns>
        OnlinePaymentResultModel CompleteOnlinePayment(string servProvCode, PaymentModel paymentModel, string parameters, string callerID);

        /// <summary>
        /// Resume the record that have paid in payment gateway.
        /// </summary>
        /// <param name="servProvCode">The agency code.</param>
        /// <param name="capID">The record id.</param>
        /// <param name="callerID">The caller id.</param>
        /// <returns>Return the OnlinePaymentResultModel.</returns>
        OnlinePaymentResultModel ResumeOnlinePayment(string servProvCode, CapIDModel capID, string callerID);

        /// <summary>
        /// Get all transaction records related to one transaction id
        /// </summary>
        /// <param name="servProvCode">Agency Code</param>
        /// <param name="transactionId">Transaction ID</param>
        /// <returns>Transaction Records</returns>
        TransactionModel[] GetTransactionModelById(string servProvCode, string transactionId);

        /// <summary>
        /// get Caps from batch transaction NBR.
        /// </summary>
        /// <param name="servProvCode">Agency Code</param>
        /// <param name="batchTransNbr">batch transaction number.</param>
        /// <param name="callerID">Method Invoker</param>
        /// <returns>cap information list</returns>
        CapIDModel[] GetCapListByTransCode(string servProvCode, long batchTransNbr, string callerID);

        /// <summary>
        /// Get cap id model 
        /// </summary>
        /// <param name="servProvCode">agency name</param>
        /// <param name="entityID">Entity id</param>
        /// <returns>cap id object</returns>
        CapIDModel GetCapIDModelByEntityID(string servProvCode, string entityID);

        /// <summary>
        /// Gets payment result
        /// </summary>
        /// <param name="servProvCode">server provider code</param>
        /// <param name="transactionID">transaction ID</param>
        /// <param name="callerID">Method Invoker</param>
        /// <returns>the result of online payment</returns>
        OnlinePaymentResultModel GetPaymentResult(string servProvCode, string transactionID, string callerID);
    }
}
