#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: ServiceManagementBlll.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2012-2014
 *
 *  Description:
 *
 *  Notes:
 * $Id: ServiceProviderBll.cs 206093 2011-10-26 01:49:26Z ACHIEVO\alan.hu $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using System;
using System.Collections.Generic;

using Accela.ACA.Common;
using Accela.ACA.WSProxy;

namespace Accela.ACA.BLL.Common
{
    /// <summary>
    /// This class provide the ability to operation service provider.
    /// </summary>
    public class ServiceManagementBll : BaseBll, IServiceManagementBll
    {
        #region Properties

        /// <summary>
        /// Gets an instance of ServiceProviderService.
        /// </summary>
        private ServiceManagementWebServiceService ServiceManagementService
        {
            get
            {
                return WSFactory.Instance.GetWebService<ServiceManagementWebServiceService>();
            }
        }

        #endregion Properties

        #region Methods

        /// <summary>
        ///  get service by service name
        /// </summary>
        /// <param name="serviceModel">The service model.</param>
        /// <returns>return the service</returns>
        public ServiceModel[] GetServiceByServiceName(ServiceModel serviceModel)
        {
            try
            {
                ServiceModel[] serviceList = ServiceManagementService.getServiceByServiceName(serviceModel, User.UserSeqNum);

                return serviceList;
            }
            catch (Exception e)
            {
                throw new ACAException(e);
            }
        }

        /// <summary>
        ///  get Service list by address
        /// </summary>
        /// <param name="refAddressModel">RefAddress Model</param>
        /// <param name="isForLocation">is for location path</param>
        /// <param name="currentUserSeqNbr">The current user sequence number.</param>
        /// <param name="initialUser">The initial user, if the current user have delegate user.</param>
        /// <returns>array of service</returns>
        public XServiceGroupModel[] GetServices(RefAddressModel refAddressModel, bool isForLocation, string currentUserSeqNbr, string initialUser)
        {
            try
            {
                XServiceGroupModel[] serviceList = ServiceManagementService.getServices(AgencyCode, refAddressModel, isForLocation, currentUserSeqNbr, initialUser);

                return serviceList;
            }
            catch (Exception e)
            {
                throw new ACAException(e);
            }
        }

        /// <summary>
        /// Gets service list for deep link.
        /// </summary>
        /// <param name="currentUserSeqNbr">The current user sequence number.</param>
        /// <returns>The array of service.</returns>
        public ServiceModel[] GetServices4DeepLink(string currentUserSeqNbr)
        {
            try
            {
                ServiceModel[] serviceList = ServiceManagementService.getServices4DeepLink(AgencyCode, currentUserSeqNbr);

                return serviceList;
            }
            catch (Exception e)
            {
                throw new ACAException(e);
            }
        }

        /// <summary>
        /// save service group setting
        /// </summary>
        /// <param name="servProvCode">the agency code.</param>
        /// <param name="xServiceGroupList">service group list.</param>
        /// <param name="callerID">caller id.</param>
        /// <returns>The array of service group.</returns>
        public IList<XServiceGroupModel> SaveServiceGroupSetting(string servProvCode, XServiceGroupModel[] xServiceGroupList, string callerID)
        {
            return null;
        }

        /// <summary>
        /// delete service group setting
        /// </summary>
        /// <param name="servProvCode">the agency code.</param>
        /// <param name="serviceGroupSeqNbr">the service group sequence number and can for I18N.</param>
        public void DeleteServiceGroupSetting(string servProvCode, string serviceGroupSeqNbr)
        {
        }

        /// <summary>
        /// get service and group mapping
        /// </summary>
        /// <param name="servPorvCode">the agency code.</param>
        /// <param name="serviceGroupSeqNbr">the service group sequence number and can for I18N.</param>
        /// <returns>return the service group.</returns>
        public XServiceGroupModel[] GetXServiceGroup(string servPorvCode, string serviceGroupSeqNbr)
        {
            return null;
        }

        /// <summary>
        /// get all service group
        /// </summary>
        /// <param name="servProvCode">the agency code</param>
        /// <returns>the service group</returns>
        public IList<ServiceGroupModel> GetAllServiceGroup(string servProvCode)
        {
            return null;
        }

        /// <summary>
        /// Get All Services
        /// </summary>
        /// <param name="servProvCode">the agency code</param>
        /// <param name="callerID">caller id</param>
        /// <returns>all services</returns>
        public IList<ServiceModel> GetAllService(string servProvCode, string callerID)
        {
            return null;
        }

        #endregion Methods
    }
}