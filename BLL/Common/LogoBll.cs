#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: LogoBll.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2008-2014
 *
 *  Description:
 *  Cache content provider, it assists the CahceManager to work.
 *
 *  Notes:
 * $Id: LogoBll.cs 277237 2014-08-13 01:17:52Z ACHIEVO\james.shi $.
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
using Accela.ACA.WSProxy.WSModel;
using log4net;

namespace Accela.ACA.BLL.Common
{
    /// <summary>
    /// This class provide the ability to operation logo.
    /// </summary>
    public class LogoBll : BaseBll, ILogoBll
    {
        #region Properties

        /// <summary>
        /// Gets an instance of InspectionService.
        /// </summary>
        /// <value>The agency prof web service.</value>
        private AgencyProfWebServiceService AgencyProfWebService
        {
            get
            {
                return WSFactory.Instance.GetWebService<AgencyProfWebServiceService>();
            }
        }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Get logo of specified agency from cache
        /// </summary>
        /// <param name="agencyCode">the super agency code</param>
        /// <returns>a logo object instance</returns>
        /// <exception cref="ACAException">Exception from web service.</exception>
        public LogoModel GetAgencyLogo(string agencyCode)
        {
            try
            {
                ICacheManager cacheManager = ObjectFactory.GetObject<ICacheManager>();
                Hashtable htLogos = cacheManager.GetCachedItem(agencyCode, I18nCultureUtil.AppendUserPreferredCultureFlag(CacheConstant.CACHE_KEY_LOGO));

                if (htLogos.ContainsKey(agencyCode) && htLogos[agencyCode] != null)
                {
                    return (LogoModel)htLogos[agencyCode];
                }

                return null;
            }
            catch (Exception e)
            {
                throw new ACAException(e);
            }
        }

        /// <summary>
        /// Gets agency logo model by types
        /// </summary>
        /// <param name="agencyCode">The agency code.</param>
        /// <param name="logoType">Type of the logo.</param>
        /// <returns>agency logo model</returns>
        /// <exception cref="ACAException">Exception from web service.</exception>
        public LogoModel GetAgencyLogoByType(string agencyCode, string logoType)
        {
            try
            {
                ICacheManager cacheManager = ObjectFactory.GetObject<ICacheManager>();
                string cacheKey = I18nCultureUtil.AppendUserPreferredCultureFlag(CacheConstant.CACHE_KEY_COMMON_LOGO);
                Hashtable htLogos = cacheManager.GetCachedItem(agencyCode, cacheKey);

                LogoModel result = null;

                if (htLogos.ContainsKey(logoType) && htLogos[logoType] != null)
                {
                    result = htLogos[logoType] as LogoModel;
                }
                else
                {
                    var logoList = AgencyProfWebService.getLogoListByLogoTypes(agencyCode, new string[] { logoType });

                    if (logoList != null && logoList.Length > 0)
                    {
                        result = logoList[0];
                        htLogos[logoType] = result;
                        cacheManager.UpdateSingleCacheItem(cacheKey, htLogos);
                    }
                }

                return result;
            }
            catch (Exception e)
            {
                throw new ACAException(e);
            }
        }

        #endregion Methods
    }
}