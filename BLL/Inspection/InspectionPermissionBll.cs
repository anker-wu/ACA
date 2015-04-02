#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: InspectionPermissionBll.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2010-2014
 *
 *  Description:
 *
 *  Notes:
 * $Id: InspectionPermissionBll.cs 277347 2014-08-14 06:51:12Z ACHIEVO\james.shi $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
  * </pre>
 */

#endregion Header

using Accela.ACA.WSProxy;

namespace Accela.ACA.BLL.Inspection
{
    /// <summary>
    /// Defines method signs for Inspection permission business.
    /// </summary>
    public class InspectionPermissionBll : BaseBll, IInspectionPermissionBll
    {
        #region Methods

        /// <summary>
        /// Check whether allows display of optional inspections.
        /// </summary>
        /// <param name="agencyCode">The agency code.</param>
        /// <param name="moduleName">Name of the module.</param>
        /// <returns>
        /// <c>true</c> if allow display of optional inspections; otherwise, <c>false</c>.
        /// </returns>
        public bool AllowDisplayOfOptionalInspections(string agencyCode, string moduleName)
        {
            return InspectionPermissionUtil.AllowDisplayOfOptionalInspections(agencyCode, moduleName);
        }

        /// <summary>
        /// Check whether allows the schedule action.
        /// </summary>
        /// <param name="agencyCode">The agency code.</param>
        /// <param name="moduleName">Name of the module.</param>
        /// <param name="isCurrentUserAnonymous">if set to <c>true</c> [is current user anonymous].</param>
        /// <param name="recordModel">The record model.</param>
        /// <returns>
        /// true:allow schedule, false:not allow schedule
        /// </returns>
        public bool AllowSchedule(string agencyCode, string moduleName, bool isCurrentUserAnonymous, CapModel4WS recordModel)
        {
            return InspectionPermissionUtil.AllowSchedule(agencyCode, moduleName, isCurrentUserAnonymous, recordModel);
        }

        /// <summary>
        /// Check the contact right (input or view) according to the provided policy name
        /// </summary>
        /// <param name="capModel">the cap model</param>
        /// <param name="agencyCode">the agency code</param>
        /// <param name="moduleName">the module name</param>
        /// <param name="policyName">the policy name</param>
        /// <returns>Return true if have right, else false.</returns>
        public bool CheckContactRight(CapModel4WS capModel, string agencyCode, string moduleName, string policyName)
        {
            return InspectionPermissionUtil.CheckContactRight(capModel, agencyCode, moduleName, policyName);
        }

        #endregion Methods
    }
}
