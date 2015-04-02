#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: BeginPayment.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2009-2010
 *
 *  Description:
 *  Begin Payment
 *
 *  Notes:
 * $Id: BeginPayment.cs 135124 2009-07-21 06:01:11Z ACHIEVO\cary.cao $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

namespace Accela.ACA.PaymentAdapter.GovolutionAdapter
{
    using System;
    using System.Collections;
    using Accela.ACA.Payment;

    /// <summary>
    /// Initial Payment
    /// </summary>
    public partial class BeginPayment : System.Web.UI.Page
    {
        /// <summary>
        /// Handles the Load event of the Page control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            // 01. Request from ACA (I)
            Hashtable parameters = ParameterHelper.GetParameterMapping(ActionType.FromACA);
            string queryString = ParameterHelper.GetReqeustParameters();
            string redirectURL = ParameterHelper.GetParameterByKey(parameters, PaymentConstant.REDIRECT_URL);
            string transactionID = ParameterHelper.GetParameterByKey(parameters, PaymentConstant.TRANSACTION_ID);

            // Log parameters to Database and log file
            LogHelper.AudiTrail(parameters, "01. Request from ACA (I)", queryString);

            // Save redirectURL to cache, this url would be used when Govolution throw an exception
            PaymentHelper.SetDataToCache<string>(PaymentConstant.REDIRECT_URL, redirectURL, 30);

            // Create transaction record according to the request parameter.
            PaymentHelper.InitialTransaction(parameters);

            // Build dynamic key-value pairs which used to replace the placeholder in the "RedirectURLParameters" setting
            Hashtable urlParams = new Hashtable();
            urlParams.Add("{AATransID}", transactionID);

            // Build Govolution redirect URL.
            string url = PaymentHelper.BuildOnlinePaymentURL(urlParams);

            // Log the whole redirect URL to DB and log file.
            LogHelper.AudiTrail(parameters, "02. Redirect to Provider (O)", url);

            // 02. Redirect to Provider (O)
            Response.Redirect(url);
        }
    }
}
