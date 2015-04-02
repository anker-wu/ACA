#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: Util.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2013
 *
 *  Description:
 *  The utility class.
 *
 *  Notes:
 * $Id: Util.cs 135124 2013-03-04 06:01:11Z ACHIEVO\james.shi $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using System;
using System.Collections;
using Accela.ACA.PaymentAdapter.Service;

namespace Accela.ACA.PaymentAdapter.ACAOnlinePaymentAdapter
{
    /// <summary>
    /// The utility class
    /// </summary>
    public class Util
    {
        /// <summary>
        /// update the transaction from the ACA response.
        /// </summary>
        /// <param name="transModel">The transaction model for update.</param>
        /// <param name="parameters">The parameters that ACA response.</param>
        public static void UpdateTransactionFromACA(OnlinePaymentTransactionModel transModel, Hashtable parameters)
        {
            transModel.aaTransId = UpdateTransactionField(parameters[PaymentConstant.TRANSACTION_ID], transModel.aaTransId);
            transModel.aaTransStatus = UpdateTransactionField(parameters[PaymentConstant.ACA_TRANS_SUCCESS], transModel.aaTransStatus);
            transModel.procTransMessage = UpdateTransactionField(parameters[PaymentConstant.USER_MESSAGE], transModel.procTransMessage);

            PaymentService payment = new PaymentService();
            payment.UpdatePaymentGatewayTrans(transModel);
        }

        /// <summary>
        /// update the transaction from the payment gateway
        /// </summary>
        /// <param name="transModel">The transaction model for update.</param>
        /// <param name="paymentResult">The payment result that from payment gateway.</param>
        public static void UpdateTransactionFromPaymentGateway(OnlinePaymentTransactionModel transModel, PaymentResultModel paymentResult)
        {
            transModel.ccType = UpdateTransactionField(paymentResult.CreditCardType, transModel.ccType);
            transModel.procTransId = UpdateTransactionField(paymentResult.ProcTransactionId, transModel.procTransId);
            transModel.convenienceFee = UpdateTransactionField(paymentResult.ConvenienceFee, transModel.convenienceFee);
            transModel.paymentAmount = UpdateTransactionField(paymentResult.PaymentAmount, transModel.paymentAmount);
            transModel.procTransType = UpdateTransactionField(paymentResult.PaymentMethod, transModel.procTransType);
            transModel.ccNumber = UpdateTransactionField(paymentResult.CardNumber, transModel.ccNumber);

            PaymentService payment = new PaymentService();
            payment.UpdatePaymentGatewayTrans(transModel);
        }

        /// <summary>
        /// Update the transaction field if new value is not empty
        /// </summary>
        /// <param name="newValue">the value used to update the transaction</param>
        /// <param name="originValue">the origin value</param>
        /// <returns>the value to be updated</returns>
        private static string UpdateTransactionField(object newValue, string originValue)
        {
            string value = PaymentUtil.ParseObjectToString(newValue);
            return string.IsNullOrEmpty(value) ? originValue : value;
        }
    }
}