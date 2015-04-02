#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: ISSOBll.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2007-2014
 *
 *  Description:
 *
 *  Notes:
 * $Id: ISSOBll.cs 276976 2014-08-08 10:10:01Z ACHIEVO\james.shi $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 *  03/20/2008    Steven.lee    Initial version.
 * </pre>
 */

#endregion Header

namespace Accela.ACA.BLL.Account
{
    /// <summary>
    /// Defines some authenticate methods.
    /// </summary>
    public interface ISSOBll
    {
        #region Methods

        /// <summary>
        /// Authenticates User in V360 System 
        /// </summary>
        /// <param name="userId">Login User ID</param>
        /// <param name="ssoSessionId">Session ID</param>
        /// <returns>activity user ID</returns>
        string Authenticate(string userId, string ssoSessionId);

        /// <summary>
        /// Signs on V360 system and get the session ID
        /// </summary>
        /// <param name="servProvCode">Service Provide Code</param>
        /// <param name="userId">Login User ID</param>
        /// <param name="password">User Password</param>
        /// <returns>activity user ID</returns>
        string Signon(string servProvCode, string userId, string password);

        #endregion Methods
    }
}