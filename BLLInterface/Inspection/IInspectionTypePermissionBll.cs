#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: IInspectionTypePermissionBll.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2010-2014
 *
 *  Description:
 *
 *  Notes:
 * $Id: IInspectionTypePermissionBll.cs 178037 2010-07-30 06:25:12Z ACHIEVO\daly.zeng $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 *   10/12/2007      Daly zeng
 * </pre>
 */

#endregion Header

using System.Collections.Generic;

using Accela.ACA.Inspection;
using Accela.ACA.WSProxy;

namespace Accela.ACA.BLL.Inspection
{
    /// <summary>
    /// Defines method sign for Inspection Type Permission.
    /// </summary>
    public interface IInspectionTypePermissionBll
    {
        #region Methods
        /// <summary>
        /// Save action permission settings
        /// </summary>
        /// <param name="insActionPermission">array for inspection action permission.</param>
        /// <returns>save successful?</returns>
        bool SaveActionPermissionSettings(InspectionActionPermissionModel[] insActionPermission);

        /// <summary>
        /// Get inspection action permissions.
        /// </summary>
        /// <param name="recordStatusGroupCode">inspection status group code</param>
        /// <param name="recordStatus">inspection status.</param>
        /// <param name="inspectionGroupCode">inspection group code</param>
        /// <returns>array for inspection action permission.</returns>
        IList<InspectionActionPermissionModel> GetInspectionActionPermissions(string recordStatusGroupCode, string recordStatus, string inspectionGroupCode);

        /// <summary>
        /// Get inspection schedule type.
        /// </summary>
        /// <param name="inspectionTypeModel">the inspectionTypeModel</param>
        /// <returns>Return the inspection schedule type.</returns>
        InspectionScheduleType GetScheduleType(InspectionTypeModel inspectionTypeModel);
        #endregion
    }
}
