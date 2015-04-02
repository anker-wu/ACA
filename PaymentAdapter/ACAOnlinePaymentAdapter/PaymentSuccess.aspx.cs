#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: PaymentSuccess.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2013
 *
 *  Description:
 *  The payment success page.
 *
 *  Notes:
 * $Id: PaymentSuccess.cs 135124 2013-03-04 06:01:11Z ACHIEVO\james.shi $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using System.Collections;

namespace Accela.ACA.PaymentAdapter.ACAOnlinePaymentAdapter
{
    /// <summary>
    /// The payment success page.
    /// </summary>
    public partial class PaymentSuccess : PaymentSuccessBase
    {
        #region Fields

        /// <summary>
        /// logger object.
        /// </summary>
        private static readonly log4net.ILog Logger = LogFactory.Instance.GetLogger(typeof(PaymentSuccess));
        
        #endregion Fields

        #region Methods

        /// <summary>
        /// Get the payment result model.
        /// </summary>
        /// <param name="parameters">The parameters from payment gateway</param>
        /// <returns>The payment result model.</returns>
        protected override PaymentResultModel GetPaymentResultModel(Hashtable parameters)
        {
            /* For test we only set the required fields and some optional fields.
             * In real envionment, it need use the 3rd payment gateway's payment result and get the all fields that from payment gateway.
             */
            PaymentResultModel paymentResultResult = new PaymentResultModel();

            paymentResultResult.TransactionId = ParameterHelper.GetParameterByKey(parameters, PaymentConstant.TRANSACTION_ID);
            paymentResultResult.PaymentAmount = ParameterHelper.GetParameterByKey(parameters, PaymentConstant.PAYMENT_AMOUNT);
            paymentResultResult.ConvenienceFee = ParameterHelper.GetParameterByKey(parameters, PaymentConstant.CONVENIENCE_FEE);
            paymentResultResult.ProcTransactionId = ParameterHelper.GetParameterByKey(parameters, PaymentConstant.PROC_TRANS_ID);
            paymentResultResult.PaymentMethod = ParameterHelper.GetParameterByKey(parameters, PaymentConstant.PAYMENT_TYPE) == "EC"
                                            ? PaymentConstant.PAY_METHOD_CHECK
                                            : PaymentConstant.PAY_METHOD_CREDIT_CARD;

            Logger.InfoFormat(
                "The payment result from payment provider: \nTransactionId={0}\nPaymentAmount={1}\nConvenienceFee={2}\nProcTransactionId={3}\nPaymentMethod={4}\n",
                paymentResultResult.TransactionId,
                paymentResultResult.PaymentAmount,
                paymentResultResult.ConvenienceFee,
                paymentResultResult.ProcTransactionId,
                paymentResultResult.PaymentMethod);

            return paymentResultResult;
        }
        
        #endregion Methods
    }
}