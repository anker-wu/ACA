#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: PaymentFail.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2013
 *
 *  Description:
 *  The payment failed page.
 *
 *  Notes:
 * $Id: PaymentFail.cs 135124 2013-03-04 06:01:11Z ACHIEVO\james.shi $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using System;
using System.Collections;

namespace Accela.ACA.PaymentAdapter.ACAOnlinePaymentAdapter
{
    /// <summary>
    /// The payment failed page.
    /// </summary>
    public partial class PaymentFail : System.Web.UI.Page
    {
        #region Fields

        /// <summary>
        /// logger object.
        /// </summary>
        private static readonly log4net.ILog Logger = LogFactory.Instance.GetLogger(typeof(BeginPayment));

        #endregion Fields

        /// <summary>
        /// Handles the Load event of the Page control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            Hashtable parameters = ParameterHelper.GetParameterMapping(ActionType.FromThirdParty);

            string transactionID = ParameterHelper.GetParameterByKey(parameters, PaymentConstant.TRANSACTION_ID);
            string applicationID = ParameterHelper.GetParameterByKey(parameters, PaymentConstant.APPLICATION_ID);
            string paymentAmount = ParameterHelper.GetParameterByKey(parameters, PaymentConstant.PAYMENT_AMOUNT);
            string convFee = ParameterHelper.GetParameterByKey(parameters, PaymentConstant.CONVENIENCE_FEE);
            string queryString = ParameterHelper.GetReqeustParameters();

            Logger.InfoFormat("Payment provider redirect to ACA Adapter: {0}", queryString);

            string errormessage = string.Format(
                "Payment fail in payment provider, Transaction ID: {0}, Application ID: {1}, Payment Amount: {2}, Convenience Fee: {3}.",
                transactionID,
                applicationID,
                paymentAmount,
                convFee);

            Logger.Error(errormessage);

            string urlParameters = string.Format("&{0}={1}", PaymentConstant.TRANSACTION_ID, transactionID);

            PaymentHelper.HandleErrorRedirect(errormessage, PaymentConstant.FAILURE_CODE, urlParameters);
        }
    }
}