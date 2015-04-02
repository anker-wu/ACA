#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: IOnlinePaymentAuditTrailBll.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2013
 *
 *  Description:
 *
 * </pre>
 */

#endregion Header

using System;

using Accela.ACA.WSProxy;

namespace Accela.ACA.BLL.Finance
{
    /// <summary>
    /// Defines some methods for on line payment audit trial.
    /// </summary>
    public interface IOnlinePaymentAuditTrailBll
    {
        #region Methods

        /// <summary>
        /// Create Audit Trial
        /// </summary>
        /// <param name="logModel">Model of OnlinePaymentAuditTrailModel</param>
        /// <returns>OnlinePaymentAuditTrailModel entity</returns>
        OnlinePaymentAudiTrailModel CreateAudiTrail(OnlinePaymentAudiTrailModel logModel);

        /// <summary>
        /// Get Audit Trial 
        /// </summary>
        /// <param name="logModel">Model of OnlinePaymentAuditTrailModel</param>
        /// <returns>OnlinePaymentAuditTrailModel entity</returns>
        OnlinePaymentAudiTrailModel[] GetAudiTrailModels(OnlinePaymentAudiTrailModel logModel);

        #endregion Methods
    }
}