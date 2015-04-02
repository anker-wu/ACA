#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: ServiceProviderBll.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2007-2014
 *
 *  Description:
 *
 *  Notes:
 * $Id: ServiceProviderBll.cs 277355 2014-08-14 07:28:03Z ACHIEVO\james.shi $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using System;
using System.Collections;

using Accela.ACA.Common;
using Accela.ACA.Common.Log;
using Accela.ACA.Common.Util;
using Accela.ACA.WSProxy;

using log4net;

namespace Accela.ACA.BLL.Common
{
    /// <summary>
    /// This class provide the ability to operation service provider.
    /// </summary>
    public class ServiceProviderBll : BaseBll, IServiceProviderBll
    {
        #region Properties

        /// <summary>
        /// Gets an instance of ServiceProviderService.
        /// </summary>
        private ServiceProviderWebServiceService ServiceProviderService
        {
            get
            {
                return WSFactory.Instance.GetWebService<ServiceProviderWebServiceService>();
            }
        }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Gets ServiceProvider Model By PK
        /// </summary>
        /// <param name="serviceProviderCode">service provider code</param>
        /// <param name="callerID">the public user id.</param>
        /// <returns>ServiceProvider Model</returns>
        public ServiceProviderModel GetServiceProviderByPK(string serviceProviderCode, string callerID)
        {
            if (string.IsNullOrEmpty(serviceProviderCode))
            {
                throw new DataValidateException(new string[] { "serviceProviderCode" });
            }

            try
            {
                ICacheManager cacheManager = (ICacheManager)ObjectFactory.GetObject(typeof(ICacheManager));
                Hashtable htServiceProvider = cacheManager.GetCachedItem(AgencyCode, I18nCultureUtil.AppendUserPreferredCultureFlag(CacheConstant.CACHE_KEY_SERVICEPROVIDER));

                ServiceProviderModel model = new ServiceProviderModel();

                if (htServiceProvider != null && htServiceProvider[CacheConstant.CACHE_KEY_SERVICEPROVIDER] != null)
                {
                    model = (ServiceProviderModel)htServiceProvider[CacheConstant.CACHE_KEY_SERVICEPROVIDER];
                }

                return model;
            }
            catch (Exception e)
            {
                throw new ACAException(e);
            }
        }

        /// <summary>
        /// Gets the service provider by source sequence number.
        /// </summary>
        /// <param name="sourceSeqNumer">The source sequence number.</param>
        /// <returns>Service provider information.</returns>
        public ServiceProviderModel[] GetServiceProviderBySourceSeqNumber(long sourceSeqNumer)
        {
            return this.ServiceProviderService.getServiceProviderBySourceSeqNumber(AgencyCode, sourceSeqNumer, User.PublicUserId);
        }

        /// <summary>
        /// Gets agency code list,
        /// In super agency, return all sub agencies includes current agency.
        /// In normal agency, return single agency code.
        /// </summary>
        /// <param name="delegateUser">The delegate user model.</param>
        /// <returns>agency code list</returns>
        /// <exception cref="DataValidateException">{ <c>serviceProviderCode</c> } is null</exception>
        /// <exception cref="ACAException">Exception from web service.</exception>
        public string[] GetSubAgencies(DelegateUserModel delegateUser)
        {
            if (string.IsNullOrEmpty(AgencyCode))
            {
                throw new DataValidateException(new string[] { "serviceProviderCode" });
            }

            try
            {
                return ServiceProviderService.getAgencies(delegateUser);
            }
            catch (Exception e)
            {
                throw new ACAException(e);
            }
        }

        /// <summary>
        /// Gets agency code list,
        /// In super agency, return all sub agencies don't includes current agency.
        /// In normal agency, return single agency code.
        /// </summary>
        /// <param name="callerID">Is for public user</param>
        /// <returns>agency code list</returns>
        /// <exception cref="DataValidateException">{ <c>serviceProviderCode</c> } is null</exception>
        /// <exception cref="ACAException">Exception from web service.</exception>
        public string[] GetSubAgencies(string callerID)
        {
            if (string.IsNullOrEmpty(AgencyCode))
            {
                throw new DataValidateException(new string[] { "serviceProviderCode" });
            }

            try
            {
                return ServiceProviderService.getSubAgencies(SuperAgencyCode, callerID);
            }
            catch (Exception e)
            {
                throw new ACAException(e);
            }
        }

        #endregion Methods
    }
}