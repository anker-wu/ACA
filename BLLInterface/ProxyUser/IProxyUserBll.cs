#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: IProxyUserBll.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2010-2014
 *
 *  Description:
 *
 *  Notes:
 * $Id: IProxyUserBll.cs 277008 2014-08-09 08:37:50Z ACHIEVO\james.shi $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using Accela.ACA.WSProxy;

namespace Accela.ACA.BLL.ProxyUser
{
    /// <summary>
    /// interface of Report
    /// </summary>
    public interface IProxyUserBll
    {
        #region Methods

        /// <summary>
        /// Get Proxy Users
        /// </summary>
        /// <param name="agencyCode">the agency code.</param>
        /// <param name="userSeqNum">the user sequence number.</param>
        /// <returns>Proxy public user information.</returns>
        PublicUserModel4WS GetProxyUsers(string agencyCode, long userSeqNum);

        /// <summary>
        /// For RTF report ,add all session parameter to ReportInfoModel, RTF Report use them in report possibly
        /// </summary>
        /// <param name="xProxyUser">The executable proxy user.</param>
        /// <param name="callerID">The caller unique identifier.</param>
        void DeleteProxyUser(XProxyUserModel xProxyUser, string callerID);

        /// <summary>
        /// get parameters of a report.
        /// </summary>
        /// <param name="xProxyUser">The executable proxy user.</param>
        /// <param name="callerID">The caller unique identifier.</param>
        void UpdateProxyStatus(XProxyUserModel xProxyUser, string callerID);

        /// <summary>
        /// Gets report button to display in permit detail page.
        /// </summary>
        /// <param name="xProxyUser">The executable proxy user.</param>
        /// <param name="proxyUserEmail">The proxy user email.</param>
        /// <param name="callerID">string caller ID.</param>
        void CreateProxyUser(XProxyUserModel xProxyUser, string proxyUserEmail, string callerID);

        /// <summary>
        /// update proxy user permissions.
        /// </summary>
        /// <param name="permissions">the permission model.</param>
        /// <param name="callerID">The caller ID.</param>
        void UpdatePermissions(XProxyUserPermissionModel[] permissions, string callerID);

        /// <summary>
        /// validate Email
        /// </summary>
        /// <param name="email">the email address.</param>
        /// <param name="callerID">caller ID.</param>
        /// <returns>validation email result.</returns>
        string ValidateEmail(string email, string callerID);
                
        /// <summary>
        /// validate Payment permission.
        /// </summary>
        /// <param name="capIDs">the cap ids need validate</param>
        /// <param name="callerID">the caller ID.</param>
        /// <returns>cap id model list.</returns>
        CapIDModel[] ValidateProxyUserPaymentPermission(CapIDModel[] capIDs, string callerID);

        #endregion Methods
    }
}