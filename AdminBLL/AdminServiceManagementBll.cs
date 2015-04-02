#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: AdminServiceManagementBll.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2013-2014
 *
 *  Description:
 *
 *
 *  Notes:
 * $Id: AdminServiceManagementBll.cs 240567 2013-09-17 09:16:37Z ACHIEVO\jone.lu $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using System;
using System.Collections.Generic;

using Accela.ACA.BLL.Common;
using Accela.ACA.Common;
using Accela.ACA.WSProxy;

namespace Accela.ACA.AdminBLL
{
    /// <summary>
    /// This class is operation service filter in ACA admin. 
    /// </summary>
    public class AdminServiceManagementBll : BaseBll, IServiceManagementBll
    {
        #region Properties

        /// <summary>
        /// Gets an instance of CapTypeFilterWebService.
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
        /// get service by service name
        /// </summary>
        /// <param name="serviceModel">The service model.</param>
        /// <returns>return the service</returns>
        public ServiceModel[] GetServiceByServiceName(ServiceModel serviceModel)
        {
            return null;
        }

        /// <summary>
        /// get Service list by address
        /// </summary>
        /// <param name="refAddressModel">RefAddress Model</param>
        /// <param name="isForLocation">is for location path</param>
        /// <param name="currentUserSeqNbr">The current user sequence number.</param>
        /// <param name="initialUser">The initial user, if the current user have delegate user.</param>
        /// <returns>array of service</returns>
        public XServiceGroupModel[] GetServices(RefAddressModel refAddressModel, bool isForLocation, string currentUserSeqNbr, string initialUser)
        {
            return null;
        }

        /// <summary>
        /// Gets service list for deep link.
        /// </summary>
        /// <param name="currentUserSeqNbr">The current user sequence number.</param>
        /// <returns>The array of service.</returns>
        public ServiceModel[] GetServices4DeepLink(string currentUserSeqNbr)
        {
            return null;
        }

        /// <summary>
        /// save service group setting
        /// </summary>
        /// <param name="servProvCode">the agency code.</param>
        /// <param name="xServiceGroupList">service group list.</param>
        /// <param name="callerID">caller id.</param>
        /// <returns>The array of service group.</returns>
        /// <exception cref="ACAException">Exception from web service.</exception>
        public IList<XServiceGroupModel> SaveServiceGroupSetting(string servProvCode, XServiceGroupModel[] xServiceGroupList, string callerID)
        {
            try
            {
                XServiceGroupModel[] xServiceGroups = ServiceManagementService.saveServicesGroupSetting(servProvCode, xServiceGroupList, callerID);

                return xServiceGroups;
            }
            catch (Exception e)
            {
                throw new ACAException(e);
            }
        }

        /// <summary>
        /// delete service group setting
        /// </summary>
        /// <param name="servProvCode">the agency code.</param>
        /// <param name="serviceGroupSeqNbr">the service group sequence number and can for I18N.</param>
        /// <exception cref="ACAException">Exception from web service.</exception>
        public void DeleteServiceGroupSetting(string servProvCode, string serviceGroupSeqNbr)
        {
            try
            {
                ServiceManagementService.deleteServiceGroupSetting(servProvCode, serviceGroupSeqNbr);
            }
            catch (Exception e)
            {
                throw new ACAException(e);
            }
        }

        /// <summary>
        /// get service and group mapping
        /// </summary>
        /// <param name="servPorvCode">the agency code.</param>
        /// <param name="serviceGroupSeqNbr">the service group sequence number and can for I18N.</param>
        /// <returns>return the service group.</returns>
        /// <exception cref="ACAException">Exception from web service.</exception>
        public XServiceGroupModel[] GetXServiceGroup(string servPorvCode, string serviceGroupSeqNbr)
        {
            try
            {
                XServiceGroupModel[] xServiceGroups = ServiceManagementService.getXServiceGroup(servPorvCode, serviceGroupSeqNbr);

                return xServiceGroups;
            }
            catch (Exception e)
            {
                throw new ACAException(e);
            }
        }

        /// <summary>
        /// get all service group
        /// </summary>
        /// <param name="servProvCode">the agency code</param>
        /// <returns>the service group</returns>
        /// <exception cref="ACAException">Exception from web service.</exception>
        public IList<ServiceGroupModel> GetAllServiceGroup(string servProvCode)
        {
            try
            {
                IList<ServiceGroupModel> groups = ServiceManagementService.getAllServiceGroup(servProvCode);

                return groups;
            }
            catch (Exception e)
            {
                throw new ACAException(e);
            }
        }

        /// <summary>
        /// Get All Services
        /// </summary>
        /// <param name="servProvCode">the agency code</param>
        /// <param name="callerID">caller id</param>
        /// <returns>all services</returns>
        /// <exception cref="ACAException">Exception from web service.</exception>
        public IList<ServiceModel> GetAllService(string servProvCode, string callerID)
        {
            try
            {
                IList<ServiceModel> services = ServiceManagementService.getAllService(servProvCode, callerID);

                return services;
            }
            catch (Exception e)
            {
                throw new ACAException(e);
            }
        }

        #endregion Methods
    }
}
