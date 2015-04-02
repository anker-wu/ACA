#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: GovolutionAdapter.aspx.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2009-2011
 *
 *  Description:
 *
 *  Notes:
 *      $Id: GovolutionAdapter.aspx.cs 139202 2009-07-15 08:06:19Z ACHIEVO\jackie.yu $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using System;
using System.Collections;
using Accela.ACA.Payment;
using Accela.ACA.PaymentAdapter.Service;
using System.Web;

namespace Accela.ACA.PaymentAdapter.GovolutionAdapter
{
    /// <summary>
    /// Govolution redirect URL to this page, here will redirect the page to responding ACA page.
    /// </summary>
    public partial class PaymentRedirect : System.Web.UI.Page
    {
        /// <summary>
        /// logger object.
        /// </summary>
        private static readonly log4net.ILog _logger = LogFactory.Instance.GetLogger(typeof(PaymentRedirect));

        /// <summary>
        /// Handles the Load event of the Page control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            // 09. Back from Provider (I)
            Hashtable parameters = ParameterHelper.GetParameterMapping(ActionType.FromThirdParty);
            string queryString = ParameterHelper.GetReqeustParameters();
            string transactionID = ParameterHelper.GetParameterByKey(parameters, PaymentConstant.TRANSACTION_ID);
            string applicationID = ParameterHelper.GetParameterByKey(parameters, PaymentConstant.APPLICATION_ID);

            if (String.IsNullOrEmpty(transactionID) || String.IsNullOrEmpty(applicationID))
            {
                HandleError("Can not get transaction from web service or redirect url is empty.");
            }

            OnlinePaymentTransactionModel transModel = null;

            // Get transaction model from database
            PaymentService payment = new PaymentService();
            transModel = payment.GetPaymentGatewayTrans(transactionID, applicationID);

            // 10. Redirect to ACA (O)
            if (transModel != null && !String.IsNullOrEmpty(transModel.redirectToACAURL))
            {
                // Log parameters to Database and log file
                LogHelper.AudiTrail(
                                           "09. Back from Provider (I)",
                                           queryString,
                                           transModel.applicationId,
                                           transModel.aaTransId,
                                           transModel.recFulNam);

                // Update status to database
                GovolutionUtil.UpdateTransaction(transModel, parameters);

                // Convert return code for ACA
                parameters[PaymentConstant.RETURN_CODE] = ConvertReturnCode4ACA();

                string msg = PaymentHelper.GetDataFromCache<string>(transactionID);
                
                if (parameters != null && !String.IsNullOrEmpty(msg))
                {
                    parameters[PaymentConstant.USER_MESSAGE] = msg;
                }

                // Convert parameters to standard
                string paramToACA = ParameterHelper.CombineResponseText(parameters, "&");

                // 10. Redirect to ACA (O)
                LogHelper.AudiTrail(
                                           "10. Redirect to ACA (O)",
                                           paramToACA,
                                           transModel.applicationId,
                                           transModel.aaTransId,
                                           transModel.recFulNam);

                Response.Redirect(String.Format("{0}?{1}", transModel.redirectToACAURL, paramToACA));
            }
            else
            {
                HandleError("Can not get transaction from web service or redirect url is empty.");
            }
        }

        /// <summary>
        /// handle the error and redirect
        /// </summary>
        /// <param name="errorMsg">the error message</param>
        private void HandleError(string errorMsg)
        {
            _logger.Error(errorMsg);
            string returnCode = ConvertReturnCode4ACA();
            string message = ParameterHelper.GetParameterByKey("user_message");
            PaymentHelper.HandleErrorRedirect(message, returnCode);
        }

        /// <summary>
        /// Convert return code from Govolution to ACA
        /// </summary>
        /// <returns></returns>
        private string ConvertReturnCode4ACA()
        {
            string returnCode = String.Empty;
            string code = ParameterHelper.GetParameterByKey("p");

            switch (code)
            {
                case "1":
                    returnCode = PaymentConstant.SUCCESS_CODE;
                    break;
                case "4":
                    returnCode = PaymentConstant.EXIT_CODE;
                    break;
                default:
                    returnCode = PaymentConstant.FAILURE_CODE;
                    break;
            }

            return returnCode;
        }
    }
}
