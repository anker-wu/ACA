#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: SessionTimeoutUtil.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2014
 *
 *  Description:
 *  Provider a tool to access web.config.
 *
 *  Notes:
 *      $Id: SessionTimeoutUtil.cs 183096 2010-10-27 01:49:43Z ACHIEVO\daly.zeng $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using System;
using System.Web;
using Accela.ACA.BLL.Common;
using Accela.ACA.Common;
using Accela.ACA.Web.Common;

namespace Accela.ACA.Web.Util
{
    /// <summary>
    /// The utility class about the session timeout.
    /// </summary>
    public static class SessionTimeoutUtil
    {
        /// <summary>
        /// The default warning time. if there is not setting the value for the 'Timeout Warning' under standard choice 'ACA_CONFIGS'
        /// </summary>
        private const string DEFAULT_TIMEOUT_WARNING_TIME = "5";

        /// <summary>
        /// Get warning time of session timeout from standard choice 'ACA_CONFIGS';
        /// </summary>
        /// <returns>The warning time.</returns>
        public static int GetWarningTime()
        {
            string agencyCode = ConfigManager.AgencyCode;

            if (string.IsNullOrEmpty(agencyCode))
            {
                return 0;
            }

            int warningTime = 0;

            try
            {
                IBizDomainBll bizDomain = ObjectFactory.GetObject<IBizDomainBll>();
                string warningTimeOut = bizDomain.GetValueForACAConfig(agencyCode, XPolicyConstant.WARNING_TIMEOUT, DEFAULT_TIMEOUT_WARNING_TIME);
                warningTime = (int)Math.Floor(double.Parse(warningTimeOut));
            }
            catch (Exception)
            {
                // If has error happen, then set it as the default value;
                warningTime = Convert.ToInt32(DEFAULT_TIMEOUT_WARNING_TIME);
            }

            //If the warning time less than 0, then set it as the default value;
            if (warningTime < 0)
            {
                warningTime = Convert.ToInt32(DEFAULT_TIMEOUT_WARNING_TIME);
            }

            /* 
             * If the warning time greater than session timeout, then set the warning timeout equals session timeout. 
             * Because the warning timeout cannot greater than the session timeout.
             */
            if (warningTime > ConfigManager.SessionStateTimeout)
            {
                warningTime = ConfigManager.SessionStateTimeout;
            }

            return warningTime;
        }

        /// <summary>
        /// Get timeout time of session timeout.
        /// </summary>
        /// <returns>the timeout time of session timeout.</returns>
        public static int GetTimeoutTime()
        {
            return ConfigManager.SessionStateTimeout;
        }

        /// <summary>
        /// Update latest request time.
        /// </summary>
        /// <param name="request">The HttpRequest.</param>
        /// <param name="response">The HttpResponse.</param>
        public static void UpdateLastestRequestTime(HttpRequest request, HttpResponse response)
        {
            string curTime = DateTime.Now.ToFileTime().ToString();
            HttpCookie cookie = request.Cookies[CookieConstant.LASTEST_REQUEST_TIME];

            if (cookie != null)
            {
                cookie.Value = curTime;
            }
            else
            {
                cookie = new HttpCookie(CookieConstant.LASTEST_REQUEST_TIME, curTime);
            }

            response.AppendCookie(cookie);
        }
    }
}