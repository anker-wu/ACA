#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: PaymentRedirect.aspx.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2009-2014
 *
 *  Description:
 *
 *  Notes:
 * $Id: PaymentRedirect.aspx.cs 277625 2014-08-19 07:06:19Z ACHIEVO\james.shi $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using System;

using Accela.ACA.Common.Log;

using log4net;

namespace Accela.ACA.Web.Payment.Govolution
{
    /// <summary>
    /// This class provide the ability to operation GOVOLUTION Payment Redirect.
    /// </summary>
    public partial class Payment_Govolution_PaymentRedirect : System.Web.UI.Page
    {
        #region Fields
        /// <summary>
        /// logger object.
        /// </summary>
        private static readonly ILog Logger = LogFactory.Instance.GetLogger(typeof(Payment_Govolution_PaymentRedirect));

        #endregion Fields

        #region Methods

        /// <summary>
        /// Raises the page load event
        /// </summary>
        /// <param name="sender">An object that contains the event sender.</param>
        /// <param name="e">A System.EventArgs object containing the event data.</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            Response.Buffer = true;

            LogPostbackData();

            string redirectedURL = string.Empty;
            try
            {
                //string transactionID = Request[INITIATION_REMITTANCE_ID];
                //string keyOfsavedData = this.GetKeyOfSavedData(transactionID);
                //IPayment paymentProcessor = PaymentHelper.GetDataFromCache<IPayment>(keyOfsavedData);
                IHandler handler = PaymentProcessorFactory.CreateHandler();
                redirectedURL = handler.HandlePaymentResult(this);
            }
            catch (Exception ex)
            {
                Logger.Error("Error occurred during handling payment result.", ex);
                string errorMessageID = DateTime.Now.Ticks.ToString();
                Session[errorMessageID] = ex.Message;
                redirectedURL = string.Format("../PaymentErrorPage.aspx?ErrorMessageID={0}", errorMessageID);
            }

            Logger.InfoFormat("begin to redirect to url={0} \n\r", redirectedURL);
            Response.Clear();
            Response.Redirect(redirectedURL, false);
        }

        /// <summary>
        /// Log post back data
        /// </summary>
        private void LogPostbackData()
        {
            string allRequestString = PaymentHelper.GetPostbackDataString();
            Logger.InfoFormat("Entering PaymentRedirect.aspx Page, the request data to this page are: {0}\n\r", allRequestString);
        }

        #endregion Methods
    }
}