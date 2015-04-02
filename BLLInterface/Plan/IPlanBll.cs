#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: IPlanBll.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2008-2014
 *
 *  Description:
 *
 *  Notes:
 * $Id: IPlanBll.cs 277004 2014-08-09 07:44:41Z ACHIEVO\james.shi $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 *   10/12/2007      Daly zeng
 * </pre>
 */

#endregion Header

using System;

using Accela.ACA.WSProxy;

namespace Accela.ACA.BLL.Plan
{
    /// <summary>
    /// Defines method signs for Plan.
    /// </summary>
    public interface IPlanBll
    {
        #region Methods

        /// <summary>
        /// Create public user's plan model.
        /// </summary>
        /// <param name="model4WS">A publicUserPlanModel4WS object.</param>
        /// <returns>PublicUserPlanModel4WS object.</returns>
        PublicUserPlanModel4WS CreatePublicUserPlan(PublicUserPlanModel4WS model4WS);

        /// <summary>
        /// Get a public user's plan by service provider code, plan sequence number and public user ID.
        /// </summary>
        /// <param name="sequenceNumber">Plan sequence number</param>
        /// <returns>PublicUserPlanModel4WS object.</returns>
        PublicUserPlanModel4WS GetPublicUserPlanByPK(long sequenceNumber);

        /// <summary>
        /// Get public user's plans by service provider code and public user ID.
        /// </summary>
        /// <param name="serviceProviderCode">Service provider code</param>
        /// <param name="publicUserID">Public user ID</param>
        /// <returns>Array of PublicUserPlanModel4WS</returns>
        Array GetPublicUserPlans(string serviceProviderCode, string publicUserID);

        /// <summary>
        /// Get public user's plans by transaction ID.
        /// </summary>
        /// <param name="tranSeqNum">Transaction ID</param>
        /// <returns>Array of PublicUserPlanModel4WS</returns>
        Array GetPublicUserPlansByTransactionID(long tranSeqNum);

        /// <summary>
        /// Get public user's plans with status "uploaded" by service provider code and public user ID.
        /// </summary>
        /// <returns>Array of PublicUserPlanModel4WS</returns>
        Array GetPublicUserUploadedPlans();

        /// <summary>
        /// Remove public user's plan model.
        /// </summary>
        /// <param name="model4WS">PublicUserPlanModel4WS object.</param>
        void RemovePublicUserPlan(PublicUserPlanModel4WS model4WS);

        /// <summary>
        /// Update the transaction ID of an array of plans
        /// </summary>
        /// <param name="planSeqNum">Array of plan sequence ID</param>
        /// <returns>The transaction ID</returns>
        long UpdatePublicUserPlanTransaction(long[] planSeqNum);

        /// <summary>
        /// Update the status of plans which apply in on transaction.
        /// </summary>
        /// <param name="tranSeqNum">Transaction ID</param>
        /// <param name="status">string of status</param>
        void UpdatePublicUserPlanStatus(long tranSeqNum, string status);

        #endregion Methods
    }
}