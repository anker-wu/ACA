#region Header

/**
* <pre>
*  Accela Citizen Access
*  File: IXEntityPermissionBll.cs
*
*  Accela, Inc.
*  Copyright (C): 2011
*
*  Description: Interface definition for XEntityPermission operations.
*
*  Notes:
* $Id: IXEntityPermissionBll.cs 171222 2010-04-21 16:10:00Z ACHIEVO\alan.hu $.
*  Revision History
*  Date,            Who,        What
*  Oct 28, 2011      Alan Hu     Initial.
* </pre>
*/

#endregion

using System.Collections.Generic;
using Accela.ACA.WSProxy;

namespace Accela.ACA.BLL.Common
{
    /// <summary>
    /// Interface definition for XEntityPermission operations.
    /// </summary>
    public interface IXEntityPermissionBll
    {
        /// <summary>
        /// Get XEntityPermissionModel collection by given XEntityPermissionModel object.
        /// </summary>
        /// <param name="xEntityPermisson">The XEntityPermissionModel object.</param>
        /// <returns>XEntityPermissionModel collection.</returns>
        IEnumerable<XEntityPermissionModel> GetXEntityPermissions(XEntityPermissionModel xEntityPermisson);

        /// <summary>
        /// Update x entity permissions.
        /// </summary>
        /// <param name="deletePK">A XEntityPermissionModel object be used as the PK to delete existing data.</param>
        /// <param name="xEntityPermissons">XEntityPermissionModel collection.</param>
        void UpdateXEntityPermissions(XEntityPermissionModel deletePK, IEnumerable<XEntityPermissionModel> xEntityPermissons);
    }
}