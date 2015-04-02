#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: CommonPaymentBll.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2009-2014
 *
 *  Description:
 *
 *  Notes:
 * $Id: CommonPaymentBll.cs 132442 2009-07-27 05:27:19Z ACHIEVO\cary.cao $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 *  05/10/2007    troy.yang    Initial version.
 * </pre>
 */

#endregion Header

using System;
using Accela.ACA.BLL.Cap;
using Accela.ACA.Common;
using Accela.ACA.Common.Util;
using Accela.ACA.WSProxy;

namespace Accela.ACA.BLL.Finance
{
    /// <summary>
    /// payment business
    /// </summary>
    public class CommonPaymentBll : BaseBll, ICommonPaymentBll
    {
        #region Fields

        #endregion Fields

        #region Properties

        /// <summary>
        /// Gets an instance of CommonPaymentService.
        /// </summary>
        private CommonPaymentWebServiceService CommonPaymentService
        {
            get
            {
                return WSFactory.Instance.GetWebService<CommonPaymentWebServiceService>();
            }
        }

        #endregion Properties

        #region ICommonPaymentBll Members

        /// <summary>
        /// initialize online payment
        /// </summary>
        /// <param name="capIDModelArray">the capID model</param>
        /// <param name="parameters">Payment Extra Parameters</param>
        /// <param name="paymentProvider">adapter name. e.g. <c>Govolution</c></param>
        /// <returns>list of the TransactionModel</returns>
        /// <exception cref="ACAException">Exception from web service.</exception>
        TransactionModel[] ICommonPaymentBll.InitialOnlinePayment(CapIDModel[] capIDModelArray, string parameters, string paymentProvider)
        {
            try
            {
                return CommonPaymentService.initialOnlinePayment(capIDModelArray, parameters, paymentProvider, User.PublicUserId);
            }
            catch (Exception e)
            {
                throw new ACAException(e);
            }
        }

        /// <summary>
        /// deal with the complete payment process
        /// </summary>
        /// <param name="servProvCode">server provider code</param>
        /// <param name="paymentModel">the payment model</param>
        /// <param name="parameters">Payment Extra Parameters</param>
        /// <param name="callerID">Method Invoker</param>
        /// <returns>the result of online payment</returns>
        /// <exception cref="DataValidateException">{ <c>paymentModel</c> } is null</exception>
        /// <exception cref="ACAException">Exception from web service.</exception>
        OnlinePaymentResultModel ICommonPaymentBll.CompleteOnlinePayment(string servProvCode, PaymentModel paymentModel, string parameters, string callerID)
        {
            if (paymentModel == null)
            {
                throw new DataValidateException(new string[] { "paymentModel" });
            }

            try
            {
                return CommonPaymentService.completeOnlinePayment(servProvCode, paymentModel, parameters, callerID);
            }
            catch (Exception e)
            {
                throw new ACAException(e);
            }
        }

        /// <summary>
        /// Get all transaction records related to one transaction id
        /// </summary>
        /// <param name="servProvCode">Agency Code</param>
        /// <param name="transactionId">Transaction ID</param>
        /// <returns>Transaction Records</returns>
        /// <exception cref="DataValidateException">{ <c>transactionId</c> } is null</exception>
        /// <exception cref="ACAException">Exception from web service.</exception>
        public TransactionModel[] GetTransactionModelById(string servProvCode, string transactionId)
        {
            if (string.IsNullOrEmpty(transactionId))
            {
                throw new DataValidateException(new string[] { "transactionId" });
            }

            try
            {
                return CommonPaymentService.getTransactionModelById(servProvCode, transactionId);
            }
            catch (Exception e)
            {
                throw new ACAException(e);
            }
        }

        /// <summary>
        /// Resume the record that have paid in payment gateway.
        /// </summary>
        /// <param name="servProvCode">The agency code.</param>
        /// <param name="capID">The record id.</param>
        /// <param name="callerID">The caller id.</param>
        /// <returns>Return the OnlinePaymentResultModel.</returns>
        /// <exception cref="DataValidateException">{ <c>CapIDModel</c> } is null</exception>
        /// <exception cref="ACAException">Exception from web service.</exception>
        OnlinePaymentResultModel ICommonPaymentBll.ResumeOnlinePayment(string servProvCode, CapIDModel capID, string callerID)
        {
            if (capID == null)
            {
                throw new DataValidateException(new string[] { "CapIDModel" });
            }

            try
            {
                return CommonPaymentService.resumeOnlinePayment(servProvCode, capID, callerID);
            }
            catch (Exception e)
            {
                throw new ACAException(e);
            }
        }

        /// <summary>
        /// get Caps from batch transaction NBR.
        /// </summary>
        /// <param name="servProvCode">Agency Code</param>
        /// <param name="batchTransNbr">batch transaction number.</param>
        /// <param name="callerID">Method Invoker</param>
        /// <returns>cap information list</returns>
        /// <exception cref="DataValidateException">{ <c>batchTransNbr</c> } is null</exception>
        /// <exception cref="ACAException">Exception from web service.</exception>
        public CapIDModel[] GetCapListByTransCode(string servProvCode, long batchTransNbr, string callerID)
        {
            if (batchTransNbr == 0)
            {
                throw new DataValidateException(new string[] { "batchTransNbr" });
            }

            try
            {
                return CommonPaymentService.getCapListByTransCode(servProvCode, batchTransNbr, callerID);
            }
            catch (Exception e)
            {
                throw new ACAException(e);
            }
        }

        /// <summary>
        /// Get cap id model
        /// </summary>
        /// <param name="servProvCode">agency name</param>
        /// <param name="entityID">Entity id</param>
        /// <returns>cap id object</returns>
        public CapIDModel GetCapIDModelByEntityID(string servProvCode, string entityID)
        {
            if (entityID != null)
            {
                ICapBll capBll = ObjectFactory.GetObject<ICapBll>();
                CapIDModel4WS selectedCapId = capBll.GetCapIDByAltID(servProvCode, entityID);
                return TempModelConvert.Trim4WSOfCapIDModel(selectedCapId);
            }

            return null;
        }

        /// <summary>
        /// Gets payment result
        /// </summary>
        /// <param name="servProvCode">server provider code</param>
        /// <param name="transactionID">transaction ID</param>
        /// <param name="callerID">Method Invoker</param>
        /// <returns>the result of online payment</returns>
        /// <exception cref="DataValidateException">{ <c>transactionID</c> } is null</exception>
        /// <exception cref="ACAException">Exception from web service.</exception>
        OnlinePaymentResultModel ICommonPaymentBll.GetPaymentResult(string servProvCode, string transactionID, string callerID)
        {
            if (transactionID == null)
            {
                throw new DataValidateException(new string[] { "transactionID" });
            }

            try
            {
                return CommonPaymentService.getPaymentResult(servProvCode, transactionID, callerID);
            }
            catch (Exception e)
            {
                throw new ACAException(e);
            }
        }

        #endregion
    }
}
