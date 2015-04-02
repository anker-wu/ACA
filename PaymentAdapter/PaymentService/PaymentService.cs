/**
 *  Accela Citizen Access
 *  File: OnlinePaymentTransactionModel.cs
 * 
 *  Accela, Inc.
 *  Copyright (C): 2009-2011
 * 
 *  Description:
 *   Provide Key-Value pair object.
 * 
 *  Notes:
 * $Id: OnlinePaymentTransactionModel.cs 130107 2009-07-21 12:23:56Z ACHIEVO\cary.cao $.
 *  Revision History
 *  Date,                  Who,                 What
 */

namespace Accela.ACA.PaymentAdapter.Service
{
    /// <summary>
    /// the payment service
    /// </summary>
    public class PaymentService
    {
        /// <summary>
        /// logger object.
        /// </summary>
        private static readonly log4net.ILog _logger = LogFactory.Instance.GetLogger(typeof(PaymentService));

        #region Transaction

        /// <summary>
        /// Create payment transaction
        /// </summary>
        /// <param name="transactionModel">transaction model</param>
        /// <returns>the transaction model after commit to database</returns>
        public OnlinePaymentTransactionModel CreatePaymentGatewayTrans(OnlinePaymentTransactionModel transactionModel)
        {
            PaymentGatewayWebServiceService gateway = WSFactory.Instance.GetPaymentGatewayWebService();

            OnlinePaymentTransactionModel result = null;

            try
            {
                result = gateway.createTrans(transactionModel);
            }
            catch (System.Exception ex)
            {
                _logger.Error("Error occurred when creating payment gateway transaction.", ex);
            }

            return result;
        }

        /// <summary>
        /// Update payment transaction
        /// </summary>
        /// <param name="transactionModel">transaction model</param>
        public void UpdatePaymentGatewayTrans(OnlinePaymentTransactionModel transactionModel)
        {
            PaymentGatewayWebServiceService gateway = WSFactory.Instance.GetPaymentGatewayWebService();
            gateway.updateTrans(transactionModel);
        }

        /// <summary>
        /// Get payment transaction
        /// </summary>
        /// <param name="aaTransId">AA transaction id</param>
        /// <param name="applicationId">application id</param>
        /// <returns>payment transaction model</returns>
        public OnlinePaymentTransactionModel GetPaymentGatewayTrans(string aaTransId, string applicationId)
        {
            PaymentGatewayWebServiceService gateway = WSFactory.Instance.GetPaymentGatewayWebService();
            OnlinePaymentTransactionModel result = gateway.getTransModels(applicationId, aaTransId);

            return result;
        }

        #endregion

        #region Trace

        /// <summary>
        /// Create log for payment
        /// </summary>
        /// <param name="logModel">payment log model</param>
        /// <returns>the log model after commit to database</returns>
        public OnlinePaymentAudiTrailModel CreatePaymentGatewayLog(OnlinePaymentAudiTrailModel logModel)
        {
            PaymentGatewayWebServiceService gateway = WSFactory.Instance.GetPaymentGatewayWebService();
            OnlinePaymentAudiTrailModel result = gateway.createAudiTrail(logModel);
            return result;
        }

        /// <summary>
        /// Get payment logs
        /// </summary>
        /// <param name="aaTransId">AA transaction id</param>
        /// <param name="applicationId">application id</param>
        /// <returns>the transaction logs</returns>
        public OnlinePaymentAudiTrailModel[] GetPaymentGatewayLogs(string aaTransId, string applicationId)
        {
            OnlinePaymentAudiTrailModel logModel = new OnlinePaymentAudiTrailModel();
            logModel.aaTransId = aaTransId;
            logModel.applicationId = applicationId;

            PaymentGatewayWebServiceService gateway = WSFactory.Instance.GetPaymentGatewayWebService();
            OnlinePaymentAudiTrailModel[] result = gateway.getAudiTrailModels(logModel);
            return result;
        }

        #endregion
    }
}
