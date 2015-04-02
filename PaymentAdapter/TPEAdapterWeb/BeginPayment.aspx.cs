#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: BeginPayment.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2010-2011
 *
 *  Description:
 *  Begin Payment
 *
 *  Notes:
 * $Id: BeginPayment.cs 135124 2010-07-19 06:01:11Z ACHIEVO\james.shi $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

namespace Accela.ACA.PaymentAdapter.TPEAdapterWeb
{
    using System;
    using System.Collections;
    using System.Web;
    using TPEWebService;

    /// <summary>
    /// Initial Payment
    /// </summary>
    public partial class BeginPayment : System.Web.UI.Page
    {
        /// <summary>
        /// logger object.
        /// </summary>
        private static readonly log4net.ILog _logger = LogFactory.Instance.GetLogger(typeof(BeginPayment));

        /// <summary>
        /// time ticks.
        /// </summary>
        private long _timeFlag;

        /// <summary>
        /// the watch instance to diagnostic the GetToken method
        /// </summary>
        private System.Diagnostics.Stopwatch _watchGetToken = null;

        /// <summary>
        /// Stopwatch instance.
        /// </summary>
        private System.Diagnostics.Stopwatch _watch = null;

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the BeginPayment class.
        /// </summary>
        public BeginPayment()
        {
            _timeFlag = DateTime.Now.Ticks;
            _watch = new System.Diagnostics.Stopwatch();
            _watch.Start();
            _logger.DebugFormat("---Page {0} Load begin [{1}]---", this.GetType().FullName, _timeFlag.ToString());
        }
        #endregion

        /// <summary>
        /// do dispose when leave this page
        /// </summary>
        public override void Dispose()
        {
            base.Dispose();

            _watch.Stop();
            _logger.DebugFormat("Page {0} Run Total Time : {1} ms.", this.GetType().FullName, _watch.ElapsedMilliseconds.ToString());
            _logger.DebugFormat("---Page {0} Load End [{1}]---", this.GetType().FullName, _timeFlag.ToString());
            _watch = null;
        } 

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
            string applicationID = ParameterHelper.GetParameterByKey(parameters, PaymentConstant.APPLICATION_ID);
            string transactionID = ParameterHelper.GetParameterByKey(parameters, PaymentConstant.TRANSACTION_ID);
            string agencyCode = ParameterHelper.GetParameterByKey(parameters, PaymentConstant.AGENCY_CODE);

            if (String.IsNullOrEmpty(queryString) || String.IsNullOrEmpty(redirectURL) || String.IsNullOrEmpty(transactionID))
            {
                _logger.Error("01. Request from ACA (I): The query string is null.");
                HttpContext.Current.Response.Write("The query string is null.");
                return;
            }

            // Save redirectURL to cache, this url would be used when TPE throw an exception
            PaymentHelper.SetDataToCache<string>(PaymentConstant.REDIRECT_URL, redirectURL, 30);

            // Save applicationID to cache, this application be used in payment success
            PaymentHelper.SetDataToCache<string>(PaymentConstant.APPLICATION_ID, applicationID, 40);

            // Log parameters to Database and log file
            LogHelper.AudiTrail(parameters, "01. Request from ACA (I)", queryString);

            // Create transaction record according to the request parameter.
            PaymentHelper.InitialTransaction(parameters);

            // Get the pay amount and convenience fee, then format it
            string amount = ParameterHelper.GetParameterByKey(parameters, PaymentConstant.PAYMENT_AMOUNT);
            if (String.IsNullOrEmpty(amount))
            {
                amount = "0.00";
            }

            string convFee = ParameterHelper.GetParameterByKey(parameters, PaymentConstant.CONVENIENCE_FEE);
            if (String.IsNullOrEmpty(convFee))
            {
                convFee = "0.00";
            }

            string totalAmount = String.Empty;
            string errorMessage = String.Empty;

            if (IsValidCurrency(amount) && IsValidCurrency(convFee))
            {
                totalAmount = String.Format("{0:0.00}", Double.Parse(amount) + Double.Parse(convFee));
            }
            else
            {
                errorMessage = "The format of pay amount or convenience fee is wrong";
                _logger.Error(errorMessage);
                PaymentHelper.HandleErrorRedirect(errorMessage, PaymentConstant.FAILURE_CODE);
                return;
            }

            _watchGetToken = new System.Diagnostics.Stopwatch();
            _watchGetToken.Start();

            string token = GetToken(agencyCode, transactionID, totalAmount, ref errorMessage);

            _watchGetToken.Stop();
            _logger.DebugFormat("GetToken() Run Total Time : {0} ms.", _watchGetToken.ElapsedMilliseconds.ToString());
            _watchGetToken = null;

            if (String.IsNullOrEmpty(token))
            {
                _logger.Error(errorMessage);
                PaymentHelper.HandleErrorRedirect(errorMessage, PaymentConstant.FAILURE_CODE);
            }
            else
            {
                // Build dynamic key-value pairs which used to replace the placeholder in the "RedirectURLParameters" setting
                Hashtable urlParams = new Hashtable();
                urlParams.Add("{token}", token);

                // Build Payment redirect URL.
                string url = PaymentHelper.BuildOnlinePaymentURL(urlParams);

                // Log the whole redirect URL to DB and log file.
                LogHelper.AudiTrail(parameters, "02. Redirect to Provider (O)", url);

                // 02. Redirect to Provider (O)
                Response.Redirect(url);
            }
        }

        /// <summary>
        /// Get the token that url need.
        /// </summary>
        /// <param name="agencyCode">The agency code</param>
        /// <param name="transactionID">The transaction id</param>
        /// <param name="payAmount">The pay amount</param>
        /// <param name="errorMessage">The error message</param>
        /// <returns>return the token value</returns>
        private string GetToken(string agencyCode, string transactionID, string payAmount, ref string errorMessage)
        {
            string token = String.Empty;
            PaymentInfo paymentInfo = new PaymentInfo();

            paymentInfo.STATECD = PaymentHelper.GetThirdPartyStaticValue(agencyCode, "STATECD");
            paymentInfo.SERVICECODE = PaymentHelper.GetThirdPartyStaticValue(agencyCode, "SERVICECODE");
            paymentInfo.MERCHANTID = PaymentHelper.GetThirdPartyStaticValue(agencyCode, "MERCHANTID");
            paymentInfo.MERCHANTKEY = PaymentHelper.GetThirdPartyStaticValue(agencyCode, "MERCHANTKEY");

            paymentInfo.UNIQUETRANSID = transactionID;
            paymentInfo.LOCALREFID = transactionID;
            paymentInfo.AMOUNT = payAmount;

            paymentInfo.HREFSUCCESS = PaymentUtil.GetDomainUrl("PaymentSuccess.aspx");
            paymentInfo.HREFFAILURE = PaymentUtil.GetDomainUrl("PaymentFailure.aspx");
            paymentInfo.HREFCANCEL = PaymentUtil.GetDomainUrl("PaymentCancel.aspx");
            paymentInfo.HREFDUPLICATE = PaymentUtil.GetDomainUrl("PaymentDuplicate.aspx");

            try
            {
                using (ServiceWebClient proxy = new ServiceWebClient())
                {                
                    PreparePaymentResult result = proxy.PreparePayment(paymentInfo);
                    if (result != null)
                    {
                        if (result.ERRORCODE == 0)
                        {
                            token = result.TOKEN;
                            _logger.InfoFormat("token from 3th party payment provider:{0}",token);
                        }
                        else
                        {
                            errorMessage = result.ERRORMESSAGE;
                        }
                    }
                    else
                    {
                        errorMessage = "Unexpected exception occurred<br/>";
                    } 
                }                
            }
            catch (Exception ex)
            {
                _logger.Error("Error occurred when getting token from third party.", ex);
                errorMessage = ex.Message;
            }

            return token;
        }

        /// <summary>
        /// judge if it is the valid currency format
        /// </summary>
        /// <param name="currency">the currency</param>
        /// <returns>return it is valid or not currency format</returns>
        private bool IsValidCurrency(string currency)
        {
            bool isValid = false;
            double dbValue = 0;

            if (Double.TryParse(currency, out dbValue))
            {
                if (!currency.Contains("."))
                {
                    isValid = true;
                }
                else
                {
                    int precision = currency.Length - currency.IndexOf('.') - 1;
                    if (precision <= 2)
                    {
                        isValid = true;
                    }
                }
            }

            return isValid;
        }      
    }
}