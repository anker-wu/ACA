#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: ProxyUser.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2010-2014
 *
 *  Description:
 *
 *  Notes:
 * $Id: ProxyUserBll.cs 277347 2014-08-14 06:51:12Z ACHIEVO\james.shi $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using System;

using Accela.ACA.Common;
using Accela.ACA.WSProxy;

namespace Accela.ACA.BLL.ProxyUser
{
    /// <summary>
    /// Deal with report business logic class.
    /// </summary>
    public class ProxyUserBll : BaseBll, IProxyUserBll
    {
        #region Properties

        /// <summary>
        /// Gets an instance of ReportService.
        /// </summary>
        /// <value>The proxy user service.</value>
        private ProxyUserWebServiceService ProxyUserService
        {
            get
            {
                return WSFactory.Instance.GetWebService<ProxyUserWebServiceService>();
            }
        }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Get Proxy Users
        /// </summary>
        /// <param name="agencyCode">the agency code.</param>
        /// <param name="userSeqNum">the user sequence number.</param>
        /// <returns>Proxy public user information.</returns>
        /// <exception cref="DataValidateException">{ <c>agencyCode, userSeqNum</c> } is null</exception>
        /// <exception cref="ACAException">Exception from web service.</exception>
        public PublicUserModel4WS GetProxyUsers(string agencyCode, long userSeqNum)
        {
            if (string.IsNullOrEmpty(agencyCode) || userSeqNum == 0)
            {
                throw new DataValidateException(new string[] { "agencyCode", "userSeqNum" });
            }

            try
            {
                PublicUserModel4WS allUsers = ProxyUserService.getProxyUsers(agencyCode, userSeqNum);

                return allUsers;
            }
            catch (Exception e)
            {
                throw new ACAException(e);
            }
        }

        /// <summary>
        /// Delete Proxy User.
        /// </summary>
        /// <param name="xProxyUser">proxy user model.</param>
        /// <param name="callerID">caller ID.</param>
        /// <exception cref="DataValidateException">{ <c>xProxyUser.serviceProviderCode, callerID</c> } is null</exception>
        /// <exception cref="ACAException">Exception from web service.</exception>
        public void DeleteProxyUser(XProxyUserModel xProxyUser, string callerID)
        {
            if (xProxyUser == null || string.IsNullOrEmpty(xProxyUser.serviceProviderCode) || string.IsNullOrEmpty(callerID))
            {
                throw new DataValidateException(new string[] { "xProxyUser.serviceProviderCode", "callerID" });
            }

            try
            {
                ProxyUserService.deleteProxyUser(xProxyUser, callerID);
            }
            catch (Exception e)
            {
                throw new ACAException(e);
            }
        }

        /// <summary>
        /// Update Proxy user Status
        /// </summary>
        /// <param name="xProxyUser">proxy user model.</param>
        /// <param name="callerID">caller ID.</param>
        /// <exception cref="DataValidateException">{ <c>xProxyUser.serviceProviderCode, callerID</c> } is null</exception>
        /// <exception cref="ACAException">Exception from web service.</exception>
        public void UpdateProxyStatus(XProxyUserModel xProxyUser, string callerID)
        {
            if (xProxyUser == null || string.IsNullOrEmpty(xProxyUser.serviceProviderCode) || string.IsNullOrEmpty(callerID))
            {
                throw new DataValidateException(new string[] { "xProxyUser.serviceProviderCode", "callerID" });
            }

            try
            {
                ProxyUserService.updateProxyStatus(xProxyUser, callerID);
            }
            catch (Exception e)
            {
                throw new ACAException(e);
            }
        }

        /// <summary>
        /// Create Proxy User
        /// </summary>
        /// <param name="xProxyUser">proxy user model.</param>
        /// <param name="proxyUserEmail">the proxy user email.</param>
        /// <param name="callerID">caller ID.</param>
        /// <exception cref="DataValidateException">{ <c>xProxyUser.serviceProviderCode, proxy User Email, callerID</c> } is null</exception>
        /// <exception cref="ACAException">Exception from web service.</exception>
        public void CreateProxyUser(XProxyUserModel xProxyUser, string proxyUserEmail, string callerID)
        {
            if (xProxyUser == null || string.IsNullOrEmpty(xProxyUser.serviceProviderCode) || string.IsNullOrEmpty(proxyUserEmail) || string.IsNullOrEmpty(callerID))
            {
                throw new DataValidateException(new string[] { "xProxyUser.serviceProviderCode", "proxy User Email", "callerID" });
            }

            try
            {
                ProxyUserService.createProxyUser(xProxyUser, proxyUserEmail, callerID);
            }
            catch (Exception e)
            {
                throw new ACAException(e);
            }
        }

        /// <summary>
        /// Update Proxy user Status
        /// </summary>
        /// <param name="permissions">proxy user model.</param>
        /// <param name="callerID">caller ID.</param>
        /// <exception cref="DataValidateException">{ <c>permissions, callerID</c> } is null</exception>
        /// <exception cref="ACAException">Exception from web service.</exception>
        public void UpdatePermissions(XProxyUserPermissionModel[] permissions, string callerID)
        {
            if (permissions == null || permissions.Length == 0 || string.IsNullOrEmpty(callerID))
            {
                throw new DataValidateException(new string[] { "permissions", "callerID" });
            }

            try
            {
                ProxyUserService.updatePermissions(permissions, callerID);
            }
            catch (ACAException e)
            {
                throw new ACAException(e);
            }
        }

        /// <summary>
        /// validate Email
        /// </summary>
        /// <param name="email">the email address.</param>
        /// <param name="callerID">caller ID.</param>
        /// <returns>email error message.</returns>
        /// <exception cref="DataValidateException">{ <c>email, callerID</c> } is null</exception>
        public string ValidateEmail(string email, string callerID)
        {
            string message = string.Empty;

            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(callerID))
            {
                throw new DataValidateException(new string[] { "email", "callerID" });
            }

            try
            {
                ProxyUserService.validateEmail(AgencyCode, email, callerID);

                return message;
            }
            catch (Exception e)
            {
                message = e.Message;
                return message;
            }
        }

        /// <summary>
        /// validate Payment permission.
        /// </summary>
        /// <param name="capIDs">the cap ids need validate</param>
        /// <param name="callerID">the caller ID.</param>
        /// <returns>cap id model list that don't have permission.</returns>
        /// <exception cref="DataValidateException">{ <c>capIDs, callerID</c> } is null</exception>
        /// <exception cref="ACAException">Exception from web service.</exception>
        public CapIDModel[] ValidateProxyUserPaymentPermission(CapIDModel[] capIDs, string callerID)
        {
            if (capIDs == null || capIDs.Length < 1 || string.IsNullOrEmpty(callerID))
            {
                throw new DataValidateException(new string[] { "capIDs", "callerID" });
            }

            try
            {
                return ProxyUserService.validateProxyUserPaymentPermission(AgencyCode, capIDs, callerID);
            }
            catch (Exception e)
            {
                throw new ACAException(e);
            }
        }

        #endregion Methods
    }
}