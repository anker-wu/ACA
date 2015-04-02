#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: DeepLinkBll.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2013-2014
 *
 *  Description:
 *
 *  Notes:
 * $Id: DeepLinkBll.cs 155305 2013-10-10 15:10:06Z ACHIEVO\peter.peng $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using System;
using Accela.ACA.Common;
using Accela.ACA.WSProxy;

namespace Accela.ACA.BLL.Common
{
    /// <summary>
    /// Class DeepLink Business.
    /// </summary>
    public class DeepLinkBll : BaseBll, IDeepLinkBLL 
    {
        #region Properties

        /// <summary>
        /// Gets an instance of AppStatusGroupService.
        /// </summary>
        private DeepLinkWebServiceService DeepLinkWebService
        {
            get
            {
                return WSFactory.Instance.GetWebService<DeepLinkWebServiceService>();
            }
        }

        #endregion Properties

        /// <summary>
        /// Create the deep link audit trail record
        /// </summary>
        /// <param name="deepLinkAuditTrailModel">Deep Link Audit Trail Record</param>
        /// <param name="callerId">Invoke Caller</param>
        /// <returns>The stored deep link audit trail record</returns>
        /// <exception cref="DataValidateException">{ <c>deepLinkAuditTrailModel, deepLinkAuditTrailModel.GUID</c> } is null</exception>
        /// <exception cref="ACAException">Exception from web service.</exception>
        public DeepLinkAuditTrailModel CreateDeepLinkAuditTrail(DeepLinkAuditTrailModel deepLinkAuditTrailModel, string callerId)
        {
            if (deepLinkAuditTrailModel == null || string.IsNullOrEmpty(deepLinkAuditTrailModel.GUID))
            {
                throw new DataValidateException(new string[] { "deepLinkAuditTrailModel", "deepLinkAuditTrailModel.GUID" });
            }

            try
            {
                return DeepLinkWebService.createDeepLinkAuditTrail(this.AgencyCode, deepLinkAuditTrailModel, callerId);
            }
            catch (Exception e)
            {
                throw new ACAException(e);
            }
        }

        /// <summary>
        /// Get the matched deep link audit trail record
        /// </summary>
        /// <param name="deepLinkAuditTrailModel">Deep Link Audit Trail Record</param>
        /// <param name="callerId">Invoke Caller</param>
        /// <returns>The updated deep link audit trail record</returns>
        /// <exception cref="DataValidateException">{ <c>deepLinkAuditTrailModel, deepLinkAuditTrailModel.GUID</c> } is null</exception>
        /// <exception cref="ACAException">Exception from web service.</exception>
        public bool UpdateDeepLinkAuditTrail(DeepLinkAuditTrailModel deepLinkAuditTrailModel, string callerId)
        {
            if (deepLinkAuditTrailModel == null || string.IsNullOrEmpty(deepLinkAuditTrailModel.GUID))
            {
                throw new DataValidateException(new string[] { "deepLinkAuditTrailModel", "deepLinkAuditTrailModel.GUID" });
            }

            try
            {
                return DeepLinkWebService.updateDeepLinkAuditTrail(this.AgencyCode, deepLinkAuditTrailModel, callerId);
            }
            catch (Exception e)
            {
                throw new ACAException(e);
            }
        }

        /// <summary>
        /// Remove the specified deep link transaction record
        /// </summary>
        /// <param name="deepLinkAuditTrailModel">The specified deep link transaction record</param>
        /// <param name="callerId">Invoke Caller</param>
        /// <returns>true - success, false - failure</returns>
        /// <exception cref="DataValidateException">{ <c>deepLinkAuditTrailModel, deepLinkAuditTrailModel.GUID</c> } is null</exception>
        /// <exception cref="ACAException">Exception from web service.</exception>
        public bool RemoveDeepLinkAuditTrail(DeepLinkAuditTrailModel deepLinkAuditTrailModel, string callerId)
        {
            if (deepLinkAuditTrailModel == null || string.IsNullOrEmpty(deepLinkAuditTrailModel.GUID))
            {
                throw new DataValidateException(new string[] { "deepLinkAuditTrailModel", "deepLinkAuditTrailModel.GUID" });
            }

            try
            {
                return DeepLinkWebService.removeDeepLinkAuditTrail(this.AgencyCode, deepLinkAuditTrailModel, callerId);
            }
            catch (Exception e)
            {
                throw new ACAException(e);
            }
        }

        /// <summary>
        /// Get the deep link audit trail record
        /// </summary>
        /// <param name="guid">The GUID</param>
        /// <param name="callerId">Invoke Caller</param>
        /// <returns>The matched deep link audit trail record</returns>
        /// <exception cref="DataValidateException">{ <c>guid</c> } is null</exception>
        /// <exception cref="ACAException">Exception from web service.</exception>
        public DeepLinkAuditTrailModel GetDeepLinkAuditTrail(string guid, string callerId)
        {
            if (string.IsNullOrEmpty(guid))
            {
                throw new DataValidateException(new string[] { "guid" });
            }

            try
            {
                return DeepLinkWebService.getDeepLinkAuditTrail(this.AgencyCode, guid, callerId);
            }
            catch (Exception e)
            {
                throw new ACAException(e);
            }
        }
    }
}
