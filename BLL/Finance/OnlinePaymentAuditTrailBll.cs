#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: OnlinePaymentAuditTrailBll.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2013-2014
 *
 *  Description:
 *
 * </pre>
 */

#endregion Header

using System;

using Accela.ACA.Common;
using Accela.ACA.WSProxy;

namespace Accela.ACA.BLL.Finance
{
    /// <summary>
    /// This class provide the ability to operate payment log.
    /// </summary>
    public class OnlinePaymentAuditTrailBll : BaseBll, IOnlinePaymentAuditTrailBll
    {
        #region Properties

        /// <summary>
        /// Gets an instance of OnlinePaymentWebService.
        /// </summary>
        private PaymentGatewayWebServiceService PaymentGatewayWebService
        {
            get
            {
                return WSFactory.Instance.GetWebService<PaymentGatewayWebServiceService>();
            }
        }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Create Audit Trial
        /// </summary>
        /// <param name="logModel">Model of OnlinePaymentTransactionModel</param>
        /// <returns>On Line Payment Audit Trail Return Info</returns>
        OnlinePaymentAudiTrailModel IOnlinePaymentAuditTrailBll.CreateAudiTrail(OnlinePaymentAudiTrailModel logModel)
        {
            try
            {
                return PaymentGatewayWebService.createAudiTrail(logModel);
            }
            catch (Exception e)
            {
                throw new ACAException(e);
            }
        }

        /// <summary>
        /// Get Audit Trials
        /// </summary>
        /// <param name="logModel">Models of OnlinePaymentTransactionModel</param>
        /// <returns>On Line Payment Audit Trail Return Info</returns>
        OnlinePaymentAudiTrailModel[] IOnlinePaymentAuditTrailBll.GetAudiTrailModels(OnlinePaymentAudiTrailModel logModel)
        {
            try
            {
                return PaymentGatewayWebService.getAudiTrailModels(logModel);
            }
            catch (Exception e)
            {
                throw new ACAException(e);
            }
        }
        #endregion Methods
    }
}