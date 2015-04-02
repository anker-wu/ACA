#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: BeginPayment.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2013
 *
 *  Description:
 *  The begin payment page.
 *
 *  Notes:
 * $Id: BeginPayment.cs 135124 2013-03-04 06:01:11Z ACHIEVO\james.shi $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using System;
using System.Collections;
using System.Web;

namespace Accela.ACA.PaymentAdapter.ACAOnlinePaymentAdapter
{
    /// <summary>
    /// Initial Payment
    /// </summary>
    public partial class BeginPayment : BeginPaymentBase
    {
        #region Methods

        /// <summary>
        /// Prepare to checkout and obtain the session token from payment gateway.
        /// </summary>
        /// <param name="parameters">The parameter from ACA.</param>
        /// <param name="sessionToken">The session token obtain from payment gateway.</param>
        /// <param name="errorMessage">The error message throw when handshake with payment gateway.</param>
        /// <returns>Is prepared OK with payment gateway.</returns>
        protected override bool PrepareCheckout(Hashtable parameters, ref string sessionToken, ref string errorMessage)
        {
            /* For test only, it only set sessionToke to random value. 
             * For real envionment, it need handshake with payment gateway and set sessionToken to real token value that need all through the payment transaction.
             * If handshake success, set sessionToken and return true; 
             * If handshake failed, set the errorMessage and return false.
             */
            sessionToken = Guid.NewGuid().ToString();
            return true;
        }

        /// <summary>
        /// Get the URL for redirect to payment gateway.
        /// </summary>
        /// <param name="token">The token value that need all through the payment transaction.</param>
        /// <param name="parameters">The parameters that from ACA.</param>
        /// <returns>The payment gateway's URL that need to redirect to.</returns>
        protected override string GetPaymentRedirectUrl(string token, Hashtable parameters)
        {
            string hostUrl = PaymentUtil.GetConfig("HostURL");
            if (string.IsNullOrEmpty(hostUrl))
            {
                throw new Exception("Can not find HostURL in Adapter.config file, be sure you have configured them.");
            }

            string agencyCode = ParameterHelper.GetParameterByKey(parameters, PaymentConstant.AGENCY_CODE);
            string applicationID = ParameterHelper.GetParameterByKey(parameters, PaymentConstant.APPLICATION_ID);
            string transactionID = ParameterHelper.GetParameterByKey(parameters, PaymentConstant.TRANSACTION_ID);
            string paymentMethod = ParameterHelper.GetParameterByKey(parameters, PaymentConstant.PAYMENT_TYPE);
            string userId = ParameterHelper.GetParameterByKey(parameters, PaymentConstant.USER_ID);
            string userFullName = ParameterHelper.GetParameterByKey(parameters, PaymentConstant.FULL_NAME);
            string userEmail = ParameterHelper.GetParameterByKey(parameters, PaymentConstant.USER_EMAIL);
            string merchantAccountID = ParameterHelper.GetParameterByKey(parameters, PaymentConstant.MERCHANT_ACCOUNT_ID);

            // Get the pay amount and convenience fee, then format it
            string amount = ParameterHelper.GetParameterByKey(parameters, PaymentConstant.PAYMENT_AMOUNT);
            if (string.IsNullOrEmpty(amount))
            {
                amount = "0.00";
            }

            string convFee = ParameterHelper.GetParameterByKey(parameters, PaymentConstant.CONVENIENCE_FEE);
            if (string.IsNullOrEmpty(convFee))
            {
                convFee = "0.00";
            }

            // combin the payment gateway URL and parameters which the payment gateway need. 
            string url = string.Format(
                    "{0}?token={1}&paymentSuccessUrl={2}&paymentFailUrl={3}&paymentCancelUrl={4}&applicationID={5}&transactionID={6}&payAmount={7}&convFee={8}&userID={9}&fullName={10}&userEmail={11}&agencyCode={12}&paymentMethod={13}&merchantAccountID={14}",
                    PaymentUtil.GetConfig("HostURL"),
                    HttpUtility.UrlEncode(token),
                    HttpUtility.UrlEncode(PaymentUtil.GetDomainUrl("PaymentSuccess.aspx")),
                    HttpUtility.UrlEncode(PaymentUtil.GetDomainUrl("PaymentFail.aspx")),
                    HttpUtility.UrlEncode(PaymentUtil.GetDomainUrl("PaymentCancel.aspx")),
                    HttpUtility.UrlEncode(applicationID),
                    HttpUtility.UrlEncode(transactionID),
                    HttpUtility.UrlEncode(amount),
                    HttpUtility.UrlEncode(convFee),
                    HttpUtility.UrlEncode(userId),
                    HttpUtility.UrlEncode(userFullName),
                    HttpUtility.UrlEncode(userEmail),
                    HttpUtility.UrlEncode(agencyCode),
                    HttpUtility.UrlEncode(paymentMethod),
                    HttpUtility.UrlEncode(merchantAccountID));

            return url;
        }

        #endregion Methods
    }
}
