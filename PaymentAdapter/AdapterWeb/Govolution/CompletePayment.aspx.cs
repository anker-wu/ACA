#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: CompletePayment.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2009-2011
 *
 *  Description:
 *  
 *
 *  Notes:
 * $Id: CompletePayment.cs 135124 2009-08-17 06:01:11Z ACHIEVO\cary.cao $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using System;
using System.Collections;
using Accela.ACA.PaymentAdapter;
using Accela.ACA.PaymentAdapter.Service;
using System.Text;
using System.Web;

namespace Accela.ACA.PaymentAdapter.GovolutionAdapter
{
    /// <summary>
    ///  complete payment
    /// </summary>
    public partial class CompletePayment : System.Web.UI.Page
    {
        /// <summary>
        /// logger object.
        /// </summary>
        private static readonly log4net.ILog _logger = LogFactory.Instance.GetLogger(typeof(CompletePayment));

        /// <summary>
        /// Handles the Load event of the Page control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            string param = PaymentUtil.GetPostbackDataString();
            _logger.Info("Params from 3rd: " + param);

            // 05. Notification from Provider (I)
            Hashtable parameters = ParameterHelper.GetParameterMapping(ActionType.FromThirdParty);
            
            string queryString = ParameterHelper.GetReqeustParameters();
            string transactionID = ParameterHelper.GetParameterByKey(parameters, PaymentConstant.TRANSACTION_ID);
            string applicationID = ParameterHelper.GetParameterByKey(parameters, PaymentConstant.APPLICATION_ID);

            if (String.IsNullOrEmpty(queryString))
            {
                _logger.Error("05. Notification from Provider (I), the parameter is empty.");
            }

            OnlinePaymentTransactionModel transModel = null;
            string responseText = "success=false";

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
                    GovolutionUtil.UpdateTransaction(transModel, parameters);

                    // Convert the parameters' name from Govolution to matched ACA
                    string responseData = ParameterHelper.CombineResponseText(parameters, "&");

                    // 06. Notification to ACA (O)
                    LogHelper.AudiTrail(
                                            "06. Notification to ACA (O)",
                                            responseData,
                                            transModel.applicationId,
                                            transModel.aaTransId,
                                            transModel.recFulNam);

                    // Get response result from ACA, if create cap successful, it will return success=true
                    //string notificationURL = "http://220.232.135.235/web/payment/PaymentPostback.aspx";
                    //string data = PaymentHelper.DoPostBack(notificationURL, responseData);
                    string data = PaymentHelper.DoPostBack(transModel.notificationURL, responseData);

                    if (!String.IsNullOrEmpty(data))
                    {
                        data = data.Replace("user_message", "UserMessage");
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
                    GovolutionUtil.UpdateTransaction(transModel, responseParams);

                    // Get formated parameters for Govolution
                    string result = this.FormatPostbackData(data);

                    // Log parameters to Database and log file
                    LogHelper.AudiTrail(
                                        "08. Notification Response to Provider (O)",
                                        result,
                                        transModel.applicationId,
                                        transModel.aaTransId,
                                        transModel.recFulNam);

                    // 08. Notification Response to Provider (O)
                    Response.Write(result);
                }
                else
                {
                    // Log exception message
                    string message = "OnlinePaymentTransactionModel is null.";
                    _logger.Error(message);
                    Response.Write(responseText);
                }
            }
            else
            {
                // Log exception message
                string message = String.IsNullOrEmpty(queryString) ? "empty" : queryString;
                _logger.ErrorFormat("Can not get transactionID or applicationID from Govolution, the queryString is {0}", message);
                Response.Write(responseText);
            }
        }

        /// <summary>
        /// Format postback data to suit with the Govolution parameters
        /// </summary>
        /// <param name="responseText">response data</param>
        /// <returns>the formatted response data </returns>
        private string FormatPostbackData(string responseText)
        {
            Hashtable param = ParameterHelper.SplitResponseText(ActionType.FromACA, responseText, '&');

            string success = ParameterHelper.GetParameterByKey(param, PaymentConstant.ACA_TRANS_SUCCESS);
            string transactionId = ParameterHelper.GetParameterByKey(param, PaymentConstant.TRANSACTION_ID);
            string message = ParameterHelper.GetParameterByKey(param, PaymentConstant.USER_MESSAGE);

            if (!String.IsNullOrEmpty(message))
            {
                message = HttpUtility.UrlEncode(message);
                PaymentHelper.SetDataToCache<string>(transactionId, message, 10);
            }

            return String.Format("success={0}&user_message={1}&remittance_id={2}", success,message, transactionId);
        }
    }
}
