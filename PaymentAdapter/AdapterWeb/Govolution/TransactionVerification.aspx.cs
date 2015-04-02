#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: TranactionVerification.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2009-2011
 *
 *  Description:
 *  
 *
 *  Notes:
 * $Id: TranactionVerification.cs 135124 2009-08-17 06:01:11Z ACHIEVO\cary.cao $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using System;
using System.Collections;
using System.Web;
using Accela.ACA.Payment;
using Accela.ACA.PaymentAdapter.Service;

namespace Accela.ACA.PaymentAdapter.GovolutionAdapter
{
    /// <summary>
    /// verify transaction
    /// </summary>
    public partial class TransactionVerification : System.Web.UI.Page
    {
        /// <summary>
        /// logger object.
        /// </summary>
        private static readonly log4net.ILog _logger = LogFactory.Instance.GetLogger(typeof(TransactionVerification));

        /// <summary>
        /// Handles the Load event of the Page control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            // 03. Verification Request from provider(I)
            Hashtable parameters = ParameterHelper.GetParameterMapping(ActionType.FromThirdParty);
            string queryString = ParameterHelper.GetReqeustParameters();
            string transactionID = ParameterHelper.GetParameterByKey(parameters, PaymentConstant.TRANSACTION_ID);
            string applicationID = ParameterHelper.GetParameterByKey(parameters, PaymentConstant.APPLICATION_ID);

            if (String.IsNullOrEmpty(queryString))
            {
                _logger.Error("03. Verification Request from provider (I), Govolution verification doesn't return any parameter.");
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
                                            "03. Verification Request from provider (I)", 
                                            queryString, 
                                            transModel.applicationId, 
                                            transModel.aaTransId, 
                                            transModel.recFulNam);

                    // Update transaction
                    GovolutionUtil.UpdateTransaction(transModel, parameters);

                    // Verify transaction and combine response text by result
                    if (this.IsPassVerification(transModel, transactionID, applicationID))
                    {
                        responseText = String.Format("amount={0}&continue_processing=true", transModel.paymentAmount);
                    }
                    else
                    {
                        _logger.Error("Verification failed.");
                    }

                    // Update verification result to transaction
                    Hashtable responseParams = ParameterHelper.SplitResponseText(ActionType.FromACA, responseText, '&');
                    GovolutionUtil.UpdateTransaction(transModel, responseParams);

                    // Log the verification result
                    LogHelper.AudiTrail(
                                                "04. Verification Response to Provider (O)",
                                                responseText,
                                                transModel.applicationId,
                                                transModel.aaTransId,
                                                transModel.recFulNam);
                }
                else
                {
                    _logger.Error("can not find OnlinePaymentTransaction in the database.");
                }
            }
            else
            {
                _logger.Error("applicationid or transactionid is empty or null.");
            }

            // 04. Verification Response to Provider (O)
            HttpContext.Current.Response.Write(responseText);
        }

        /// <summary>
        ///  check if pass verifycation
        /// </summary>
        /// <param name="transModel">OnlinePaymentTransaction model</param>
        /// <param name="transactionId">transaction id</param>
        /// <param name="applicationId">application id</param>
        /// <returns>true if pass verification</returns>
        private bool IsPassVerification(OnlinePaymentTransactionModel transModel, string transactionId, string applicationId)
        {
            bool isPass = false;

            if (transModel != null
                && transModel.aaTransId != null
                && transModel.applicationId != null
                && transModel.aaTransId.CompareTo(transactionId) == 0
                && transModel.applicationId.CompareTo(applicationId) == 0)
            {
                isPass = true;
            }

            return isPass;
        }
    }
}
