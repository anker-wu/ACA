#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: IInspectionPermissionBll.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2010
 *
 *  Description:
 *
 *  Notes:
 * $Id: IInspectionPermissionBll.cs 187347 2010-12-24 02:53:29Z ACHIEVO\daly.zeng $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
  * </pre>
 */

#endregion Header

using System;
using System.Collections.Generic;

using Accela.ACA.Inspection;
using Accela.ACA.WSProxy;

namespace Accela.ACA.BLL.Inspection
{
    /// <summary>
    /// Defines method signs for Inspection permission BLL.
    /// </summary>
    public interface IInspectionPermissionBll
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
        bool AllowDisplayOfOptionalInspections(string agencyCode, string moduleName);

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
        bool AllowSchedule(string agencyCode, string moduleName, bool isCurrentUserAnonymous, CapModel4WS recordModel);

        /// <summary>
        /// Check the contact right (input or view) according to the provided policy name
        /// </summary>
        /// <param name="capModel">the cap model</param>
        /// <param name="agencyCode">the agency code</param>
        /// <param name="moduleName">the module name</param>
        /// <param name="policyName">the policy name</param>
        /// <returns>Return true if have right, else false.</returns>
        bool CheckContactRight(CapModel4WS capModel, string agencyCode, string moduleName, string policyName);

        #endregion Methods
    }
}
