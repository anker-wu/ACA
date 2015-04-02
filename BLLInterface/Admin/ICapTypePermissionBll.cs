#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: ICapTypePermissionBll.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2009-2014
 *
 *  Description:
 *  Interface define for ICapTypePermissionBll.
 *
 *  Notes:
 * $Id: ICapTypePermissionBll.cs 277178 2014-08-12 08:11:48Z ACHIEVO\james.shi $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using System.Collections;

using Accela.ACA.WSProxy;
using Accela.ACA.WSProxy.WSModel;

namespace Accela.ACA.BLL
{
    /// <summary>
    /// Defines many methods for Cap type permission.
    /// </summary>
    public interface ICapTypePermissionBll
    {
        #region Methods

        /// <summary>
        /// Get Cap Type Permission List. Filter Cache record by Cap Type information
        /// </summary>
        /// <param name="agencyCode">agency code</param>
        /// <param name="capTypePermission">the capType Permission Model</param>
        /// <returns>the CapTypePermission list.</returns>
        CapTypePermissionModel[] GetCapTypePermissions(string agencyCode, CapTypePermissionModel capTypePermission);

        /// <summary>
        /// Get Cap Type List.
        /// </summary>
        /// <param name="agencyCode">agency code</param>
        /// <param name="capTypePermission">the capType Permission Model</param>
        /// <returns>the CapTypePermission list.</returns>
        CapTypePermissionModel[] GetPermissionByControllerType(string agencyCode, CapTypePermissionModel capTypePermission);

        /// <summary>
        /// Is document deletable by Application status
        /// </summary>
        /// <param name="capStatus">CAP status</param>
        /// <param name="buttonName">button name</param>
        /// <param name="capType">CapTypeModel object</param>
        /// <returns>true if the document is deletable;otherwise,false</returns>
        bool IsDeletableDocumentByAppStatus(string capStatus, string buttonName, CapTypeModel capType);

        /// <summary>
        /// Get Cap type filter name by app type.
        /// </summary>
        /// <param name="agencyCode">The agency code.</param>
        /// <param name="capTypePermission">the cap type permission model</param>
        /// <returns>cap type filter name.</returns>
        string GetCapTypeFilterByAppStatus(string agencyCode, CapTypePermissionModel capTypePermission);

        #endregion Methods
    }
}