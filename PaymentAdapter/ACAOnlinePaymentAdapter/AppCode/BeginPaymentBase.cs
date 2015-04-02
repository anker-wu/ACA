#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: BeginPaymentBase.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2013
 *
 *  Description:
 *  The begin payment base page.
 *
 *  Notes:
 * $Id: BeginPaymentBase.cs 135124 2013-03-04 06:01:11Z ACHIEVO\james.shi $.
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
    /// The begin payment base page.
    /// </summary>
    public abstract class BeginPaymentBase : System.Web.UI.Page
    {
        #region Fields

        /// <summary>
        /// logger object.
        /// </summary>
        private static readonly log4net.ILog Logger = LogFactory.Instance.GetLogger(typeof(BeginPaymentBase));

        /// <summary>
        /// time ticks.
        /// </summary>
        private long _timeFlag;

        /// <summary>
        /// Stopwatch instance.
        /// </summary>
        private System.Diagnostics.Stopwatch _watch = null;

        #endregion Fields

        #region Constructors & Dispose

        /// <summary>
        /// Initializes a new instance of the BeginPaymentBase class.
        /// </summary>
        public BeginPaymentBase()
        {
            this._timeFlag = DateTime.Now.Ticks;
            this._watch = new System.Diagnostics.Stopwatch();
            this._watch.Start();
            Logger.DebugFormat("---Page {0} Load begin [{1}]---", GetType().FullName, this._timeFlag);
        }

        /// <summary>
        /// do dispose when leave this page
        /// </summary>
        public override void Dispose()
        {
            base.Dispose();

            this._watch.Stop();
            Logger.DebugFormat("Page {0} Run Total Time : {1} ms.", GetType().FullName, this._watch.ElapsedMilliseconds);
            Logger.DebugFormat("---Page {0} Load End [{1}]---", GetType().FullName, this._timeFlag);
            this._watch = null;
        }

        #endregion Constructors & Dispose

        #region Abstract Methods

        /// <summary>
        /// Prepare to checkout and obtain the session token from payment gateway.
        /// </summary>
        /// <param name="parameters">The parameter from ACA.</param>
        /// <param name="sessionToken">The session token obtain from payment gateway.</param>
        /// <param name="errorMessage">The error message throw when handshake with payment gateway.</param>
        /// <returns>Is prepared OK with payment gateway.</returns>
        protected abstract bool PrepareCheckout(Hashtable parameters, ref string sessionToken, ref string errorMessage);

        /// <summary>
        /// Get the URL for redirect to payment gateway.
        /// </summary>
        /// <param name="sessionToken">The session token that need all through the payment transaction.</param>
        /// <param name="parameters">The parameters that from ACA.</param>
        /// <returns>The payment gateway's URL that need to redirect to.</returns>
        protected abstract string GetPaymentRedirectUrl(string sessionToken, Hashtable parameters);

        #endregion Abstract Methods

        #region Methods

        /// <summary>
        /// Handles the Load event of the Page control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            // 01. Request from ACA (I)
            string queryString = ParameterHelper.GetReqeustParameters();

            Hashtable parameters = ParameterHelper.GetParameterMapping(ActionType.FromACA);
            string redirectURL = ParameterHelper.GetParameterByKey(parameters, PaymentConstant.REDIRECT_URL);
            string applicationID = ParameterHelper.GetParameterByKey(parameters, PaymentConstant.APPLICATION_ID);
            string transactionID = ParameterHelper.GetParameterByKey(parameters, PaymentConstant.TRANSACTION_ID);

            if (string.IsNullOrEmpty(queryString) || string.IsNullOrEmpty(redirectURL) || string.IsNullOrEmpty(transactionID))
            {
                Logger.Error("01. Request from ACA (I): The query string is null.");
                HttpContext.Current.Response.Write("The query string is null.");
                return;
            }

            // Save redirectURL to cache, this url would be used when payment provider throw an exception
            PaymentHelper.SetDataToCache(PaymentConstant.REDIRECT_URL, redirectURL, 40);

            // Save applicationID to cache, this application be used in payment success
            PaymentHelper.SetDataToCache(PaymentConstant.APPLICATION_ID, applicationID, 40);

            // Log parameters to Database and log file
            LogHelper.AudiTrail(parameters, "01. Request from ACA (I)", queryString);

            // Create transaction record according to the request parameter.
            PaymentHelper.InitialTransaction(parameters);

            // Prepare to checkout and obtain the seesion token from payment gateway.
            string sessionToken = string.Empty;
            string errorMessage = string.Empty;
            bool isCheckoutReady = PrepareCheckout(parameters, ref sessionToken, ref errorMessage);

            if (!isCheckoutReady)
            {
                Logger.Error(errorMessage);
                PaymentHelper.HandleErrorRedirect(errorMessage, PaymentConstant.FAILURE_CODE);
                return;
            }

            // Build Payment redirect URL.
            string url = GetPaymentRedirectUrl(sessionToken, parameters);

            // Log the whole redirect URL to DB and log file.
            LogHelper.AudiTrail(parameters, "02. Redirect to Payment Provider (O)", url);

            // Redirect to Payment Provider
            Response.Redirect(url);
        }

        #endregion Methods
    }
}