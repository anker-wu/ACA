#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: PaymentSuccess.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2010-2011
 *
 *  Description:
 *  
 *
 *  Notes:
 * $Id: PaymentSuccess.cs 135124 2010-07-20 06:01:11Z ACHIEVO\james.shi $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

namespace Accela.ACA.PaymentAdapter.TPEAdapterWeb
{
    using System;
    using System.Collections;
    using System.Text;
    using System.Web;

    using Accela.ACA.PaymentAdapter.Service;
    using TPEWebService;

    /// <summary>
    /// deal with successful payment
    /// </summary>
    public partial class PaymentSuccess : System.Web.UI.Page
    {
        /// <summary>
        /// logger object.
        /// </summary>
        private static readonly log4net.ILog _logger = LogFactory.Instance.GetLogger(typeof(PaymentSuccess));

        /// <summary>
        /// time ticks.
        /// </summary>
        private long _timeFlag;

        /// <summary>
        /// the reponse text
        /// </summary>
        private string _responseText = "success=false";

        /// <summary>
        /// Initializes a new instance of the PaymentSuccess class.
        /// </summary>
        public PaymentSuccess()
        {
            _timeFlag = DateTime.Now.Ticks;

            _logger.DebugFormat("---Page {0} Load begin [{1}]---", this.GetType().FullName, _timeFlag.ToString());
        }

        /// <summary>
        /// do dispose when leave this page
        /// </summary>
        public override void Dispose()
        {
            base.Dispose();

            _logger.DebugFormat("---Page {0} Load End [{1}]---", this.GetType().FullName, _timeFlag.ToString());
        } 

        /// <summary>
        /// Handles the Load event of the Page control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            string redirectUrl = String.Empty;
            string parametersFrom3rdParty = PaymentUtil.GetPostbackDataString();
            _logger.Info("QueryString or Form data from third party payment provider: " + parametersFrom3rdParty);

            // 05. Notification from Provider (I)
            Hashtable parameters = ParameterHelper.GetParameterMapping(ActionType.FromThirdParty);
            string token = ParameterHelper.GetParameterByKey(parameters, PaymentConstant.PROC_TRANS_ID);

            try
            {
                using (ServiceWebClient proxy = new ServiceWebClient())
                {
                    PaymentResult paymentResult = proxy.QueryPayment(token);

                    string allInfoFromThirdParty = paymentResult==null ? String.Empty : 
                        ",\nADDRESS1: " + paymentResult.ADDRESS1 +
                        ",\nADDRESS2: " + paymentResult.ADDRESS2 +
                        ",\nAUTHCODE: " + paymentResult.AUTHCODE +
                        ",\nAVSResponse: " + paymentResult.AVSResponse +
                        ",\nBillingName: " + paymentResult.BillingName +
                        ",\nCITY: " + paymentResult.CITY +
                        ",\nCOUNTRY: " + paymentResult.COUNTRY +
                        ",\nCreditCardType: " + paymentResult.CreditCardType +
                        ",\nCVVResponse: " + paymentResult.CVVResponse +
                        ",\nEMAIL: " + paymentResult.EMAIL +
                        ",\nEMAIL1: " + paymentResult.EMAIL1 +
                        ",\nEMAIL2: " + paymentResult.EMAIL2 +
                        ",\nEMAIL3: " + paymentResult.EMAIL3 +
                        ",\nExpirationDate: " + paymentResult.ExpirationDate +
                        ",\nExtensionData: " + paymentResult.ExtensionData +
                        ",\nFAILCODE: " + paymentResult.FAILCODE +
                        ",\nFAILMESSAGE: " + paymentResult.FAILMESSAGE +
                        ",\nFAX: " + paymentResult.FAX +
                        ",\nLAST4NUMBER: " + paymentResult.LAST4NUMBER +
                        ",\nLOCALREFID: " + paymentResult.LOCALREFID +
                        ",\nNAME: " + paymentResult.NAME +
                        ",\nORDERID: " + paymentResult.ORDERID +
                        ",\nPAYTYPE: " + paymentResult.PAYTYPE +
                        ",\nPHONE: " + paymentResult.PHONE +
                        ",\nRECEIPTDATE: " + paymentResult.RECEIPTDATE +
                        ",\nRECEIPTTIME: " + paymentResult.RECEIPTTIME +
                        ",\nSTATE: " + paymentResult.STATE +
                        ",\nTOTALAMOUNT: " + paymentResult.TOTALAMOUNT +
                        ",\nZIP: " + paymentResult.ZIP;
                    _logger.Info("Data from third party payment provider after user paid successfully: " + allInfoFromThirdParty);
                    
                    if (paymentResult != null && paymentResult.FAILCODE.ToUpper() == "N")
                    {
                        redirectUrl = DoPaymentAndReturnRedirectUrl(parameters, paymentResult);
                    }
                    else
                    {
                        string message = "The payment failed which query from TPE.";

                        if (paymentResult == null)
                        {
                            message += "PaymentResult is NULL";
                        }
                        else
                        {
                            message += " PaymentResult.FAILCODE = " + paymentResult.FAILCODE + "\nMessage: " + paymentResult.FAILMESSAGE;
                        }

                        _logger.Error(message);
                        PaymentHelper.HandleErrorRedirect(message, PaymentConstant.FAILURE_CODE);
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.Error("Error Occurred when handling payment success procedure.", ex);
                PaymentHelper.HandleErrorRedirect(ex.Message, PaymentConstant.FAILURE_CODE);
            }

            if (!String.IsNullOrEmpty(redirectUrl))
            {
                Response.Redirect(redirectUrl); 
            }
        }

        /// <summary>
        /// Do payment and return the redirect url
        /// </summary>
        /// <param name="parameters">the parameters from third party</param>
        /// <param name="paymentResult">the payment result which query from third party</param>
        /// <returns>the redirect url</returns>
        private string DoPaymentAndReturnRedirectUrl(Hashtable parameters, PaymentResult paymentResult)
        {
            string returnRedirectUrl = String.Empty;

            string transactionID = paymentResult.LOCALREFID;
            string applicationID = PaymentHelper.GetDataFromCache<string>(PaymentConstant.APPLICATION_ID);
            string queryString = ParameterHelper.GetReqeustParameters();

            if (String.IsNullOrEmpty(queryString))
            {
                _logger.Error("05. Notification from Provider (I), the parameter is empty.");
            }

            OnlinePaymentTransactionModel transModel = null;

            if (!String.IsNullOrEmpty(transactionID) && !String.IsNullOrEmpty(applicationID))
            {
                // Get transaction model from database
                PaymentService payment = new PaymentService();
                transModel = payment.GetPaymentGatewayTrans(transactionID, applicationID);

                if (transModel != null)
                {
                    // Log parameters to Database and log file
                    LogHelper.AudiTrail(
                                            "05. Notification from Provider (I)",
                                            queryString,
                                            transModel.applicationId,
                                            transModel.aaTransId,
                                            transModel.recFulNam);

                    // Update transaction
                    TPEUtil.UpdateTransaction(transModel, parameters);

                    // Convert the parameters' name from TPE to matched ACA
                    StringBuilder responseData = new StringBuilder();

                    responseData.Append(ParameterHelper.CombineResponseText(parameters, "&"));
                    if (responseData.Length > 0)
                    {
                        responseData.Remove(responseData.Length - 1, 1);
                    }

                    responseData.AppendFormat("&{0}={1}", PaymentConstant.TRANSACTION_ID, paymentResult.LOCALREFID);
                    responseData.AppendFormat("&{0}={1}", PaymentConstant.CC_NUMBER, paymentResult.LAST4NUMBER);
                    responseData.AppendFormat("&{0}={1}", PaymentConstant.CC_AuthCode, paymentResult.AUTHCODE);

                    //responseData.AppendFormat("&{0}={1}", PaymentConstant.PAYMENT_AMOUNT, paymentResult.TOTALAMOUNT);
                    responseData.AppendFormat("&{0}={1}", PaymentConstant.PAYMENT_AMOUNT, transModel.paymentAmount);  // for test, beacuse the fee changed
                    responseData.AppendFormat("&{0}={1}", PaymentConstant.CONVENIENCE_FEE, transModel.convenienceFee);
                    responseData.AppendFormat("&{0}={1}", PaymentConstant.CC_TYPE, paymentResult.CreditCardType);
                    responseData.AppendFormat("&{0}={1}", PaymentConstant.PROC_TRANS_TYPE, paymentResult.PAYTYPE);
                    responseData.AppendFormat("&{0}={1}", PaymentConstant.PAYEE, paymentResult.NAME);
                    responseData.AppendFormat("&{0}={1}", PaymentConstant.PAYEE_ADDRESS, String.Format("{0},{1} {2} {3} {4}", paymentResult.ADDRESS1, paymentResult.ADDRESS2, paymentResult.CITY, paymentResult.STATE, paymentResult.COUNTRY));
                    responseData.AppendFormat("&{0}={1}", PaymentConstant.PAYEE_PHONE, paymentResult.PHONE);

                    _logger.InfoFormat("Begin to do postback to ACA, URL:{0}", transModel.notificationURL);

                    // 06. Notification to ACA (O)
                    LogHelper.AudiTrail(
                                            "06. Notification to ACA (O)",
                                            responseData.ToString(),
                                            transModel.applicationId,
                                            transModel.aaTransId,
                                            transModel.recFulNam);

                    // Get response result from ACA, if create cap successful, it will return success=true
                    string data = PaymentHelper.DoPostBack(transModel.notificationURL, responseData.ToString());

                    if (!String.IsNullOrEmpty(data))
                    {
                        data = data.Replace("user_message", PaymentConstant.USER_MESSAGE);
                    }

                    // Log parameters to Database and log file
                    LogHelper.AudiTrail(
                                            "07. Notification Response from ACA (I)",
                                            data,
                                            transModel.applicationId,
                                            transModel.aaTransId,
                                            transModel.recFulNam);

                    // Update transaction when finished payment
                    Hashtable responseParams = ParameterHelper.SplitResponseText(ActionType.FromACA, data, '&');
                    TPEUtil.UpdateTransaction(transModel, responseParams);

                    // Get formated parameters for TPE
                    string result = this.FormatPostbackData(data, transactionID);

                    // Log parameters to Database and log file
                    LogHelper.AudiTrail(
                                        "08. Notification Response to Provider (O)",
                                        result,
                                        transModel.applicationId,
                                        transModel.aaTransId,
                                        transModel.recFulNam);

                    // 08. Notification Response to Provider (O)
                    if (!String.IsNullOrEmpty(result))
                    {
                        result = result.Replace("user_message", PaymentConstant.USER_MESSAGE);
                    }

                    string paramToACA = GetRedirectUrlParamToACA(result, applicationID, transactionID);
                    returnRedirectUrl = String.Format("{0}?{1}", transModel.redirectToACAURL, paramToACA);

                    _logger.Info(returnRedirectUrl);
                }
                else
                {
                    // Log exception message
                    string message = "OnlinePaymentTransactionModel is null.";
                    _logger.Error(message);

                    string paramToACA = GetRedirectUrlParamToACA(_responseText, applicationID, transactionID);

                    returnRedirectUrl = String.Format("{0}?{1}", transModel.redirectToACAURL, paramToACA);
                }
            }
            else
            {
                // Log exception message
                string message = String.IsNullOrEmpty(queryString) ? "empty" : queryString;
                _logger.ErrorFormat("Can not get transactionID from TPE or applicationID from configuration, the queryString is {0}", message);

                string paramToACA = GetRedirectUrlParamToACA(_responseText, applicationID, transactionID);

                returnRedirectUrl = String.Format("{0}?{1}", transModel.redirectToACAURL, paramToACA);
            }

            return returnRedirectUrl;
        }

        /// <summary>
        /// Format postback data to suit with the TPE parameters
        /// </summary>
        /// <param name="responseText">response data</param>
        /// <param name="transactionId">transaction id</param>
        /// <returns>the formatted response data </returns>
        private string FormatPostbackData(string responseText, string transactionId)
        {
            Hashtable param = ParameterHelper.SplitResponseText(ActionType.FromACA, responseText, '&');

            string success = ParameterHelper.GetParameterByKey(param, PaymentConstant.ACA_TRANS_SUCCESS);
            string message = ParameterHelper.GetParameterByKey(param, PaymentConstant.USER_MESSAGE);

            if (!String.IsNullOrEmpty(message))
            {
                message = HttpUtility.UrlEncode(message);
                PaymentHelper.SetDataToCache<string>(transactionId, message, 10);
            }

            return String.Format("success={0}&user_message={1}&remittance_id={2}", success, message, transactionId);
        }

        /// <summary>
        /// contact the redirect url's parameters
        /// </summary>
        /// <param name="responseText">the reponse text with url's parameter</param>
        /// <param name="applicationID">application id</param>
        /// <param name="transactionID">transaction id</param>
        /// <returns>the redirect url's parameters</returns>
        private string GetRedirectUrlParamToACA(string responseText, string applicationID, string transactionID)
        {
            Hashtable param = ParameterHelper.SplitResponseText(ActionType.FromACA, responseText, '&');

            string success = ParameterHelper.GetParameterByKey(param, PaymentConstant.ACA_TRANS_SUCCESS);
            string message = ParameterHelper.GetParameterByKey(param, PaymentConstant.USER_MESSAGE);

            StringBuilder paramToACA = new StringBuilder();

            if ("true".Equals(success, StringComparison.OrdinalIgnoreCase))
            {
                paramToACA.AppendFormat("{0}={1}", PaymentConstant.RETURN_CODE, PaymentConstant.SUCCESS_CODE);
            }
            else
            {
                paramToACA.AppendFormat("{0}={1}", PaymentConstant.RETURN_CODE, PaymentConstant.FAILURE_CODE);
            }

            paramToACA.AppendFormat("&{0}={1}", PaymentConstant.APPLICATION_ID, applicationID);
            paramToACA.AppendFormat("&{0}={1}", PaymentConstant.TRANSACTION_ID, transactionID);
            paramToACA.AppendFormat("&{0}={1}", PaymentConstant.USER_MESSAGE, message);

            return paramToACA.ToString();
        }     
    }
}
