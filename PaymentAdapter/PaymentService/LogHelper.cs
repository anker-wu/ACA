/**
 *  Accela Citizen Access
 *  File: LogHelper.cs
 * 
 *  Accela, Inc.
 *  Copyright (C): 2009-2013
 * 
 *  Description:
 *  
 * 
 *  Notes:
 * $Id: LogHelper.cs 130107 2009-07-21 12:23:56Z ACHIEVO\cary.cao $.
 *  Revision History
 *  Date,                  Who,                 What
 */

using System;
using System.Collections;
using System.Web;
using Accela.ACA.PaymentAdapter.Service;
using log4net;

namespace Accela.ACA.PaymentAdapter
{
    /// <summary>
    /// the log helper class
    /// </summary>
    public sealed class LogHelper
    {
        /// <summary>
        /// logger instatnce.
        /// </summary>
        private static readonly ILog _logger = LogFactory.Instance.GetLogger(string.Empty);

        /// <summary>
        /// log data to database and log file
        /// </summary>
        /// <param name="parameters">the parameters</param>
        /// <param name="processName">the process name</param>
        /// <param name="content">the content</param>
        public static void AudiTrail(Hashtable parameters, string processName, string content)
        {
            // Log to database
            OnlinePaymentAudiTrail(parameters, processName, content);

            // Log to log file
            string msg = String.Concat(processName, ": ", content);
            _logger.Info(msg);
        }

        /// <summary>
        /// log data to database and log file
        /// </summary>
        /// <param name="title">the process name</param>
        /// <param name="content">the log content</param>
        /// <param name="applicationId">aa application id</param>
        /// <param name="transactionId">aa transaction id</param>
        /// <param name="fullName">public user id</param>
        public static void AudiTrail(string title, string content, string applicationId, string transactionId, string fullName)
        {
            // Log to database
            OnlinePaymentAudiTrail(title, content, applicationId, transactionId, fullName);

            // Log to log file
            string msg = String.Concat(title, ": ", content);
            _logger.Info(msg);
        }

        #region Public Methods

        /// <summary>
        /// Online payment log
        /// </summary>
        /// <param name="parameters">the parameters key and value pairs</param>
        /// <param name="processName">process name</param>
        /// <param name="content">the content to be saved</param>
        public static void OnlinePaymentAudiTrail(Hashtable parameters, string processName, string content)
        {
            OnlinePaymentAudiTrailModel logModel = new OnlinePaymentAudiTrailModel();
            logModel.applicationId = PaymentHelper.GetDataFromCache<string>(PaymentConstant.APPLICATION_ID);
            logModel.aaTransId = ParameterHelper.GetParameterByKey(parameters, PaymentConstant.TRANSACTION_ID);
            logModel.processContent = HttpUtility.UrlDecode(content);
            logModel.processName = processName;
            logModel.recFulNam = ParameterHelper.GetParameterByKey(parameters, PaymentConstant.USER_ID);
            logModel.recDate = DateTime.Now;

            try
            {
                PaymentGatewayWebServiceService gateway = WSFactory.Instance.GetPaymentGatewayWebService();
                gateway.createAudiTrail(logModel);
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
                _logger.Error("Application ID: " + logModel.applicationId);
                _logger.Error("Transaction ID: " + logModel.aaTransId);
                _logger.Error("Process Content: " + logModel.processContent);
                _logger.Error("Process Name: " + logModel.processName);
                _logger.Error("User: " + logModel.recFulNam);
                _logger.Error("Date: " + logModel.recDate);
                _logger.Error(ex.StackTrace);
            }
        }

        /// <summary>
        /// log data to database
        /// </summary>
        /// <param name="title">the process name</param>
        /// <param name="content">the log content</param>
        /// <param name="applicationId">aa application id</param>
        /// <param name="transactionId">aa transaction id</param>
        /// <param name="fullName">public user id</param>
        public static void OnlinePaymentAudiTrail(string title, string content, string applicationId, string transactionId, string fullName)
        {
            OnlinePaymentAudiTrailModel logModel = new OnlinePaymentAudiTrailModel();
            logModel.aaTransId = transactionId;
            logModel.applicationId = applicationId;
            logModel.processContent = content;
            logModel.processName = title;
            logModel.recFulNam = fullName;
            logModel.recDate = DateTime.Now;

            try
            {
                PaymentGatewayWebServiceService gateway = WSFactory.Instance.GetPaymentGatewayWebService();
                gateway.createAudiTrail(logModel);
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
                _logger.Error("Application ID: " + logModel.applicationId);
                _logger.Error("Transaction ID: " + logModel.aaTransId);
                _logger.Error("Process Content: " + logModel.processContent);
                _logger.Error("Process Name: " + logModel.processName);
                _logger.Error("User: " + logModel.recFulNam);
                _logger.Error("Date: " + logModel.recDate);
                _logger.Error(ex.StackTrace);
            }
        }

        #endregion
    }
}
