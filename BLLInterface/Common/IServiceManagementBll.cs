#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: IServiceManagementBll.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2012-2014
 *
 *  Description:
 *
 *  Notes:
 * $Id: IServiceProviderBll.cs 206093 2011-10-26 01:49:26Z ACHIEVO\alan.hu $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 *   10/12/2007      Daly zeng
 * </pre>
 */

#endregion Header

using System.Collections.Generic;
using Accela.ACA.WSProxy;

namespace Accela.ACA.BLL.Common
{
    /// <summary>
    /// Defines methods for Service Management BLL.
    /// </summary>
    public interface IServiceManagementBll
    {
        #region Methods

        /// <summary>
        ///  get service by service name
        /// </summary>
        /// <param name="serviceModel">The service model.</param>
        /// <returns>return the service</returns>
        ServiceModel[] GetServiceByServiceName(ServiceModel serviceModel);

        /// <summary>
        ///  get Service list by address
        /// </summary>
        /// <param name="refAddressModel">RefAddress Model</param>
        /// <param name="isForLocation">is for location path</param>
        /// <param name="currentUserSeqNbr">The current user sequence number.</param>
        /// <param name="initialUser">The initial user, if the current user have delegate user.</param>
        /// <returns>array of service</returns>
        XServiceGroupModel[] GetServices(RefAddressModel refAddressModel, bool isForLocation, string currentUserSeqNbr, string initialUser);

        /// <summary>
        /// Gets service list for deep link.
        /// </summary>
        /// <param name="currentUserSeqNbr">The current user sequence number.</param>
        /// <returns>The array of service.</returns>
        ServiceModel[] GetServices4DeepLink(string currentUserSeqNbr);

        /// <summary>
        /// save service group setting
        /// </summary>
        /// <param name="servProvCode">the agency code.</param>
        /// <param name="xServiceGroupList">service group list.</param>
        /// <param name="callerID">caller id.</param>
        /// <returns>The array of service group.</returns>
        IList<XServiceGroupModel> SaveServiceGroupSetting(string servProvCode, XServiceGroupModel[] xServiceGroupList, string callerID);

        /// <summary>
        /// delete service group setting
        /// </summary>
        /// <param name="servProvCode">the agency code.</param>
        /// <param name="serviceGroupSeqNbr">the service group sequence number and can for I18N.</param>
        void DeleteServiceGroupSetting(string servProvCode, string serviceGroupSeqNbr);

        /// <summary>
        /// get service and group mapping
        /// </summary>
        /// <param name="servPorvCode">the agency code.</param>
        /// <param name="serviceGroupSeqNbr">the service group sequence number and can for I18N.</param>
        /// <returns>return the service group.</returns>
        XServiceGroupModel[] GetXServiceGroup(string servPorvCode, string serviceGroupSeqNbr);

        /// <summary>
        /// get all service group
        /// </summary>
        /// <param name="servProvCode">the agency code</param>
        /// <returns>the service group</returns>
        IList<ServiceGroupModel> GetAllServiceGroup(string servProvCode);

        /// <summary>
        /// Get All Services
        /// </summary>
        /// <param name="servProvCode">the agency code</param>
        /// <param name="callerID">caller id</param>
        /// <returns>all services</returns>
        IList<ServiceModel> GetAllService(string servProvCode, string callerID);

        #endregion Methods
    }
}