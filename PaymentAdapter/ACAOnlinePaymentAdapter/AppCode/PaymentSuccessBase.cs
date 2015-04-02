#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: PaymentSuccessBase.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2013
 *
 *  Description:
 *  The payment success base page.
 *
 *  Notes:
 * $Id: PaymentSuccessBase.cs 135124 2013-03-04 06:01:11Z ACHIEVO\james.shi $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using System;
using System.Collections;
using System.Text;
using Accela.ACA.PaymentAdapter.Service;

namespace Accela.ACA.PaymentAdapter.ACAOnlinePaymentAdapter
{
    /// <summary>
    /// The payment success base page.
    /// </summary>
    public abstract class PaymentSuccessBase : System.Web.UI.Page
    {
        #region Fields

        /// <summary>
        /// logger object.
        /// </summary>
        private static readonly log4net.ILog Logger = LogFactory.Instance.GetLogger(typeof(PaymentSuccessBase));

        #endregion Fields

        #region Abstract Methods

        /// <summary>
        /// Get the payment result model.
        /// </summary>
        /// <param name="parameters">The parameters from payment gateway</param>
        /// <returns>The payment result model.</returns>
        protected abstract PaymentResultModel GetPaymentResultModel(Hashtable parameters);

        #endregion Abstract Methods

        #region Methods

        /// <summary>
        /// Handles the Load event of the Page control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            Hashtable parameters = ParameterHelper.GetParameterMapping(ActionType.FromThirdParty);

            // get transaction id from 3rd payment gateway's return URL
            string transactionID = ParameterHelper.GetParameterByKey(parameters, PaymentConstant.TRANSACTION_ID);
            string applicationID = PaymentHelper.GetDataFromCache<string>(PaymentConstant.APPLICATION_ID);
            string queryString = ParameterHelper.GetReqeustParameters();

            // do validation and get transaction from database.
            OnlinePaymentTransactionModel transModel = GetValidateTransaction(queryString, transactionID, applicationID);

            if (transModel == null)
            {
                return;
            }

            // Log parameters to database and log file
            LogHelper.AudiTrail(
                            "03. Notification from Payment Provider (I)",
                            queryString,
                            transModel.applicationId,
                            transModel.aaTransId,
                            transModel.recFulNam);

            // get the payment result from the 3rd payment gateway
            PaymentResultModel paymentResult = GetPaymentResultModel(parameters);

            // Postback to ACA and complete the payment process in ACA, then return the Redirect URL.
            string redirectUrl = DoPaymentAndReturnRedirectUrl(transModel, paymentResult);

            if (!string.IsNullOrEmpty(redirectUrl))
            {
                Response.Redirect(redirectUrl);
            }
        }

        /// <summary>
        /// Do payment and return the redirect url
        /// </summary>
        /// <param name="transModel">The transaction model.</param>
        /// <param name="paymentResult">The payment result model.</param>
        /// <returns>The redirect url</returns>
        private string DoPaymentAndReturnRedirectUrl(OnlinePaymentTransactionModel transModel, PaymentResultModel paymentResult)
        {
            if (transModel == null || paymentResult == null)
            {
                Logger.Info("transModel is null or PaymentResult is null.");
                return null;
            }

            string transactionID = paymentResult.TransactionId;
            string applicationID = PaymentHelper.GetDataFromCache<string>(PaymentConstant.APPLICATION_ID);

            // update the transaction from 3rd payment gateway
            Util.UpdateTransactionFromPaymentGateway(transModel, paymentResult);

            // the following parameters are required to postback to ACA
            StringBuilder responseData = new StringBuilder();
            responseData.AppendFormat("&{0}={1}", PaymentConstant.TRANSACTION_ID, paymentResult.TransactionId);
            responseData.AppendFormat("&{0}={1}", PaymentConstant.PAYMENT_AMOUNT, paymentResult.PaymentAmount);
            responseData.AppendFormat("&{0}={1}", PaymentConstant.CONVENIENCE_FEE, paymentResult.ConvenienceFee);
            responseData.AppendFormat("&{0}={1}", PaymentConstant.PROC_TRANS_ID, paymentResult.ProcTransactionId);

            // the following parameters are optional to postback to ACA
            responseData.AppendFormat("&{0}={1}", PaymentConstant.PROC_TRANS_TYPE, paymentResult.PaymentMethod);
            responseData.AppendFormat("&{0}={1}", PaymentConstant.CC_TYPE, paymentResult.CreditCardType);
            responseData.AppendFormat("&{0}={1}", PaymentConstant.CC_AuthCode, paymentResult.CCAuthCode);
            responseData.AppendFormat("&{0}={1}", PaymentConstant.CC_NUMBER, paymentResult.CardNumber);
            responseData.AppendFormat("&{0}={1}", PaymentConstant.PAYEE, paymentResult.Payee);
            responseData.AppendFormat("&{0}={1}", PaymentConstant.PAYEE_ADDRESS, paymentResult.PayeeAddress);
            responseData.AppendFormat("&{0}={1}", PaymentConstant.PAYEE_PHONE, paymentResult.PayeePhone);
            responseData.AppendFormat("&{0}={1}", PaymentConstant.PAYMENT_COMMENT, paymentResult.PaymentComment);
            responseData.AppendFormat("&{0}={1}", PaymentConstant.CUSTOM_BATCH_TRANS_NBR, paymentResult.CustomBatchTransNbr);

            Logger.InfoFormat("Begin to do postback to ACA, URL:{0}?{1}.", transModel.notificationURL, responseData);

            // Log parameters to database and log file
            LogHelper.AudiTrail(
                            "04. Notification to ACA (O)",
                            responseData.ToString(),
                            transModel.applicationId,
                            transModel.aaTransId,
                            transModel.recFulNam);

            /* Get response result from ACA, if create record successful, it will return success=true
               The return parameters from ACA is: success, user_message, TransactionID
            */
            string data = PaymentHelper.DoPostBack(transModel.notificationURL, responseData.ToString());

            if (!string.IsNullOrEmpty(data))
            {
                data = data.Replace("user_message", PaymentConstant.USER_MESSAGE);
            }

            // Log parameters to database and log file
            LogHelper.AudiTrail(
                            "05. Notification Response from ACA (I)",
                            data,
                            transModel.applicationId,
                            transModel.aaTransId,
                            transModel.recFulNam);

            Hashtable responseParams = ParameterHelper.SplitResponseText(ActionType.FromACA, data, '&');
            Util.UpdateTransactionFromACA(transModel, responseParams);

            string paramToACA = GetRedirectUrlParamToACA(data, applicationID, transactionID);
            string returnRedirectUrl = string.Format("{0}?{1}", transModel.redirectToACAURL, paramToACA);

            Logger.Info(returnRedirectUrl);

            return returnRedirectUrl;
        }

        /// <summary>
        /// Contact the redirect url parameters
        /// </summary>
        /// <param name="responseText">The response text with url parameter</param>
        /// <param name="applicationID">The application id</param>
        /// <param name="transactionID">The transaction id</param>
        /// <returns>The redirect url parameters</returns>
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

        /// <summary>
        /// Validate and get the transaction model.
        /// </summary>
        /// <param name="queryString">The query string.</param>
        /// <param name="transactionID">The transaction id.</param>
        /// <param name="applicationID">The application id.</param>
        /// <returns>The transaction model.</returns>
        private OnlinePaymentTransactionModel GetValidateTransaction(string queryString, string transactionID, string applicationID)
        {
            if (string.IsNullOrEmpty(queryString))
            {
                Logger.Error("Notification from Provider (I), the parameter is empty.");
            }

            // handle error if transaction id or application id is null.
            if (string.IsNullOrEmpty(transactionID) || string.IsNullOrEmpty(applicationID))
            {
                string message = string.Format(
                         "Can not get transactionID from 3rd payment gateway or applicationID from configuration, the queryString is {0}.",
                         string.IsNullOrEmpty(queryString) ? "empty" : queryString);

                Logger.Error(message);
                PaymentHelper.HandleErrorRedirect(message, PaymentConstant.FAILURE_CODE);
                return null;
            }

            // Get the transaction model from database
            PaymentService payment = new PaymentService();
            OnlinePaymentTransactionModel transModel = payment.GetPaymentGatewayTrans(transactionID, applicationID);

            // handle error if transaction model is null.
            if (transModel == null)
            {
                string message = string.Format(
                    "OnlinePaymentTransactionModel is null, transactionID is {0}, applicationID is {1}.",
                    transactionID,
                    applicationID);

                Logger.Error(message);
                PaymentHelper.HandleErrorRedirect(message, PaymentConstant.FAILURE_CODE);
                return null;
            }

            return transModel;
        }

        #endregion Methods
    }
}