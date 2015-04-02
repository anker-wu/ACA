#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: TimeZoneBll.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2008-2014
 *
 *  Description:
 *  This class deal with time zone conversion
 *
 *  Notes:
 * $Id: TimeZoneBll.cs 277237 2014-08-13 01:17:52Z ACHIEVO\james.shi $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using System;

using Accela.ACA.Common;
using Accela.ACA.Common.Log;
using Accela.ACA.Common.Util;
using Accela.ACA.WSProxy;

using log4net;

namespace Accela.ACA.BLL.Common
{
    /// <summary>
    /// This class provide the ability to operation time zone.
    /// </summary>
    public class TimeZoneBll : BaseBll, ITimeZoneBll
    {
        #region Properties

        /// <summary>
        /// Gets an instance of TimeZoneService.
        /// </summary>
        private TimeZoneWebServiceService TimeZoneService
        {
            get
            {
                return WSFactory.Instance.GetWebService<TimeZoneWebServiceService>();
            }
        }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Gets a string of agency current date 
        /// </summary>
        /// <param name="serviceProviderCode">The service provider code</param>
        /// <returns>agency current date</returns>
        public DateTime GetAgencyCurrentDate(string serviceProviderCode)
        {
            if (string.IsNullOrEmpty(serviceProviderCode))
            {
                throw new DataValidateException(new string[] { "serviceProviderCode" });
            }

            try
            {
                string datetime = TimeZoneService.getAgencyCurrentDate(serviceProviderCode);
                return I18nDateTimeUtil.ParseFromWebService(datetime);
            }
            catch (Exception e)
            {
                throw new ACAException(e);
            }
        }

        #endregion Methods
    }
}