#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: PlanBll.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2008-2014
 *
 *  Description:
 *
 *  Notes:
 * $Id: PlanBll.cs 277347 2014-08-14 06:51:12Z ACHIEVO\james.shi $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using System;

using Accela.ACA.Common;
using Accela.ACA.Common.Log;
using Accela.ACA.WSProxy;

using log4net;

namespace Accela.ACA.BLL.Plan
{
    /// <summary>
    /// This class provide the ability to operation plan.
    /// </summary>
    public class PlanBll : BaseBll, IPlanBll
    {
        #region Properties

        /// <summary>
        /// Gets an instance of PlanService.
        /// </summary>
        private PlanWebServiceService PlanService
        {
            get
            {
                return WSFactory.Instance.GetWebService<PlanWebServiceService>();
            }
        }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Create public user's plan model.
        /// </summary>
        /// <param name="model4WS">a PublicUserPlanModel</param>
        /// <returns>Public user info</returns>
        public PublicUserPlanModel4WS CreatePublicUserPlan(PublicUserPlanModel4WS model4WS)
        {
            try
            {
                return PlanService.createPublicUserPlan(model4WS);
            }
            catch (Exception e)
            {
                throw new ACAException(e);
            }
        }

        /// <summary>
        /// Get a public user's plan by service provider code, plan sequence number and public user ID.
        /// </summary>
        /// <param name="sequenceNumber">Plan sequence number</param>
        /// <returns>Public user plan info</returns>
        public PublicUserPlanModel4WS GetPublicUserPlanByPK(long sequenceNumber)
        {
            try
            {
                return PlanService.getPublicUserPlanByPK(AgencyCode, sequenceNumber, User.PublicUserId);
            }
            catch (Exception e)
            {
                throw new ACAException(e);
            }
        }

        /// <summary>
        /// Get public user's plans by service provider code and public user ID.
        /// </summary>
        /// <param name="serviceProviderCode">Service provider code</param>
        /// <param name="publicUserID">Public user ID</param>
        /// <returns>Array of PublicUserPlanModel4WS</returns>
        public Array GetPublicUserPlans(string serviceProviderCode, string publicUserID)
        {
            try
            {
                return PlanService.getPublicUserPlans(serviceProviderCode, publicUserID);
            }
            catch (Exception e)
            {
                throw new ACAException(e);
            }
        }

        /// <summary>
        /// Get public user's plans by transaction ID.
        /// </summary>
        /// <param name="tranSeqNum">Transaction ID</param>
        /// <returns>Array of PublicUserPlanModel4WS</returns>
        public Array GetPublicUserPlansByTransactionID(long tranSeqNum)
        {
            try
            {
                return PlanService.getPublicUserPlansByTransactionID(AgencyCode, tranSeqNum, User.PublicUserId);
            }
            catch (Exception e)
            {
                throw new ACAException(e);
            }
        }

        /// <summary>
        /// Get public user's plans with status "uploaded" by service provider code and public user ID.
        /// </summary>
        /// <returns>Array of PublicUserPlanModel4WS</returns>
        public Array GetPublicUserUploadedPlans()
        {
            try
            {
                return PlanService.getPublicUserUploadedPlans(AgencyCode, User.PublicUserId);
            }
            catch (Exception e)
            {
                throw new ACAException(e);
            }
        }

        /// <summary>
        /// Remove public user's plan model.
        /// </summary>
        /// <param name="model4WS">a PublicUserPlanModel</param>
        public void RemovePublicUserPlan(PublicUserPlanModel4WS model4WS)
        {
            try
            {
                PlanService.removePublicUserPlan(model4WS);
            }
            catch (Exception e)
            {
                throw new ACAException(e);
            }
        }

        /// <summary>
        /// Update the transaction ID of an array of plans
        /// </summary>
        /// <param name="planSeqNum">Array of plan sequence ID</param>
        /// <returns>The transaction ID</returns>
        public long UpdatePublicUserPlanTransaction(long[] planSeqNum)
        {
            try
            {
                return PlanService.updatePublicUserPlanTransaction(AgencyCode, planSeqNum, User.PublicUserId);
            }
            catch (Exception e)
            {
                throw new ACAException(e);
            }
        }

        /// <summary>
        /// Update the status of plans which apply in on transaction.
        /// </summary>
        /// <param name="tranSeqNum">Transaction ID</param>
        /// <param name="status">string of status</param>
        public void UpdatePublicUserPlanStatus(long tranSeqNum, string status)
        {
            try
            {
                PlanService.updatePublicUserPlanStatus(AgencyCode, tranSeqNum.ToString(), status, User.PublicUserId);
            }
            catch (Exception e)
            {
                throw new ACAException(e);
            }
        }

        #endregion Methods
    }
}