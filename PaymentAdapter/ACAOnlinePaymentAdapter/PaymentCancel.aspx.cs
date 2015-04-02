#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: PaymentCancel.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2013
 *
 *  Description:
 *  The payment cancel page.
 *
 *  Notes:
 * $Id: PaymentCancel.cs 135124 2013-03-04 06:01:11Z ACHIEVO\james.shi $.
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
    /// The payment cancel page.
    /// </summary>
    public partial class PaymentCancel : System.Web.UI.Page
    {
        /// <summary>
        /// logger object.
        /// </summary>
        private static readonly log4net.ILog Logger = LogFactory.Instance.GetLogger(typeof(PaymentCancel));

        /// <summary>
        /// Handles the Load event of the Page control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            Hashtable parameters = ParameterHelper.GetParameterMapping(ActionType.FromThirdParty);

            string transactionID = ParameterHelper.GetParameterByKey(parameters, PaymentConstant.TRANSACTION_ID);
            string urlParameters = string.Format("&{0}={1}", PaymentConstant.TRANSACTION_ID, transactionID);

            string errorMessage = "User cancelled the payment in payment provider.";
            Logger.Error(errorMessage);

            PaymentHelper.HandleErrorRedirect(errorMessage, PaymentConstant.FAILURE_CODE, urlParameters);
        }
    }
}