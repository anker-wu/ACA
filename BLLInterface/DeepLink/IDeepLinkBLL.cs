#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: IDeepLinkBLL.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2013-2014
 *
 *  Description:
 *
 *  Notes:
 * $Id: IDeepLinkBLL.cs 155305 2013-10-10 15:30:06Z ACHIEVO\peter.peng $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Accela.ACA.WSProxy;

namespace Accela.ACA.BLL.Common
{
    /// <summary>
    /// An interface that defines all approach to operate the deep link transaction record.
    /// </summary>
    public interface IDeepLinkBLL
    {
        /// <summary>
        /// Create the deep link audit trail record
        /// </summary>
        /// <param name="deepLinkAuditTrailModel">Deep Link Audit Trail Record</param>
        /// <param name="callerId">Invoke Caller</param>
        /// <returns>The stored deep link audit trail record</returns>
        DeepLinkAuditTrailModel CreateDeepLinkAuditTrail(DeepLinkAuditTrailModel deepLinkAuditTrailModel, string callerId);

        /// <summary>
        /// Get the matched deep link audit trail record
        /// </summary>
        /// <param name="deepLinkAuditTrailModel">Deep Link Audit Trail Record</param>
        /// <param name="callerId">Invoke Caller</param>
        /// <returns>The updated deep link audit trail record</returns>
        bool UpdateDeepLinkAuditTrail(DeepLinkAuditTrailModel deepLinkAuditTrailModel, string callerId);

        /// <summary>
        /// Remove the specified deep link transaction record
        /// </summary>
        /// <param name="deepLinkAuditTrailModel">The specified deep link transaction record</param>
        /// <param name="callerId">Invoke Caller</param>
        /// <returns>true - success, false - failure</returns>
        bool RemoveDeepLinkAuditTrail(DeepLinkAuditTrailModel deepLinkAuditTrailModel, string callerId);

        /// <summary>
        /// Get the deep link audit trail record
        /// </summary>
        /// <param name="guid">The GUID</param>
        /// <param name="callerId">Invoke Caller</param>
        /// <returns>The matched deep link audit trail record</returns>
        DeepLinkAuditTrailModel GetDeepLinkAuditTrail(string guid, string callerId);
    }
}
