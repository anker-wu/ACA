#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: V360SSOBll.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2008-2014
 *
 *  Description:
 *
 *  Notes:
 * $Id: SSOBll.cs 277178 2014-08-12 08:11:48Z ACHIEVO\james.shi $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 *  05/10/2007    troy.yang    Initial version.
 * </pre>
 */

#endregion Header

using System;
using Accela.ACA.Common;
using Accela.ACA.Common.Log;
using Accela.ACA.WSProxy;
using log4net;

namespace Accela.ACA.BLL.Account
{
    /// <summary>
    /// This class provide the ability to operation SSO.
    /// </summary>
    public class SSOBll : BaseBll, ISSOBll
    {
        #region Fields

        /// <summary>
        /// Logger Object.
        /// </summary>
        private static readonly ILog Logger = LogFactory.Instance.GetLogger(typeof(SSOBll));

        #endregion Fields

        #region Properties

        /// <summary>
        /// Gets an instance of SSOService.
        /// </summary>
        private SSOWebServiceService SSOService
        {
            get
            {
                return WSFactory.Instance.GetWebService<SSOWebServiceService>();
            }
        }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Authenticates User in V360 System
        /// </summary>
        /// <param name="userId">Login User ID</param>
        /// <param name="ssoSessionId">Session ID</param>
        /// <returns>activity user ID</returns>
        /// <exception cref="ACAException">Exception from web service.</exception>
        public string Authenticate(string userId, string ssoSessionId)
        {
            if (Logger.IsDebugEnabled)
            {
                Logger.Debug("Begin SSOBll.authenticate()");
            }

            try
            {
                string response = SSOService.authenticate(userId, ssoSessionId);

                if (Logger.IsDebugEnabled)
                {
                    Logger.Debug("End SSOBll.authenticate()");
                }

                return response;
            }
            catch (Exception e)
            {
                throw new ACAException(e);
            }
        }

        /// <summary>
        /// Signs on V360 system and get the session ID
        /// </summary>
        /// <param name="servProvCode">Service Provide Code</param>
        /// <param name="userId">Login User ID</param>
        /// <param name="password">User Password</param>
        /// <returns>activity user ID</returns>
        /// <exception cref="ACAException">Exception from web service.</exception>
        public string Signon(string servProvCode, string userId, string password)
        {
            if (Logger.IsDebugEnabled)
            {
                Logger.Debug("Begin SSOBll.signon()");
            }

            try
            {
                string response = SSOService.signon(servProvCode, userId, password);

                if (Logger.IsDebugEnabled)
                {
                    Logger.Debug("End SSOBll.signon()");
                }

                return response;
            }
            catch (Exception e)
            {
                throw new ACAException(e);
            }
        }

        #endregion Methods
    }
}