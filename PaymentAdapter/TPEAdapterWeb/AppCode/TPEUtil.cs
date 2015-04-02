#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: TPEUtil.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2010
 *
 *  Description:
 *  
 *
 *  Notes:
 * $Id: TPEUtil.cs 135124 2010-07-26 06:01:11Z ACHIEVO\james.shi $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using System;
using System.Collections;
using Accela.ACA.PaymentAdapter.Service;

namespace Accela.ACA.PaymentAdapter.TPEAdapterWeb
{
    /// <summary>
    /// TPE utility
    /// </summary>
    public static class TPEUtil
    {
        /// <summary>
        /// Credit card
        /// </summary>
        private const string CREDIT_CARD = "1";

        /// <summary>
        /// update transaction
        /// </summary>
        /// <param name="transModel">OnlinePaymentTransaction model</param>
        /// <param name="paramHT">parameter key value pairs</param>
        public static void UpdateTransaction(OnlinePaymentTransactionModel transModel, Hashtable paramHT)
        {
            string cardType = GetCardType(paramHT[PaymentConstant.PAYMENT_TYPE]);
            transModel.ccType = UpdateTransactionField(cardType, transModel.ccType);
            transModel.aaTransStatus = UpdateTransactionField(paramHT[PaymentConstant.ACA_TRANS_SUCCESS], transModel.aaTransStatus);
            transModel.procTransId = UpdateTransactionField(paramHT[PaymentConstant.PROC_TRANS_ID], transModel.procTransId);
            transModel.convenienceFee = UpdateTransactionField(paramHT[PaymentConstant.CONVENIENCE_FEE], transModel.convenienceFee);
            transModel.procTransStatus = UpdateTransactionField(paramHT[PaymentConstant.PROC_TRANS_STATUS], transModel.procTransStatus);
            transModel.applicationId = UpdateTransactionField(paramHT[PaymentConstant.APPLICATION_ID], transModel.applicationId);
            transModel.paymentAmount = UpdateTransactionField(paramHT[PaymentConstant.PAYMENT_AMOUNT], transModel.paymentAmount);
            transModel.procTransResultCode = UpdateTransactionField(paramHT[PaymentConstant.RETURN_CODE], transModel.procTransResultCode);
            transModel.procTransMessage = UpdateTransactionField(paramHT[PaymentConstant.USER_MESSAGE], transModel.procTransMessage);
            transModel.procTransType = UpdateTransactionField(paramHT[PaymentConstant.PAYMENT_TYPE], transModel.procTransType);
            transModel.ccNumber = UpdateTransactionField(paramHT[PaymentConstant.CC_NUMBER], transModel.ccNumber);

            PaymentService payment = new PaymentService();
            payment.UpdatePaymentGatewayTrans(transModel);
        }

        /// <summary>
        /// Get card type, If payment_type equals 1, set ccType to "Credit Card", else set to "Check"
        /// </summary>
        /// <param name="paymentType">the payment_type parameter</param>
        /// <returns>the card type</returns>
        private static string GetCardType(object paymentType)
        {
            string cardType = PaymentUtil.ParseObjectToString(paymentType);

            // 1: credit card; 2,3: check
            if (cardType.Equals(CREDIT_CARD, StringComparison.CurrentCultureIgnoreCase))
            {
                return "Credit Card";
            }
            else if (cardType == "2" || cardType == "3")   
            {
                return "Check";
            }
            else
            {
                return String.Empty;
            }
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
            return String.IsNullOrEmpty(value) ? originValue : value;
        }
    }
}
