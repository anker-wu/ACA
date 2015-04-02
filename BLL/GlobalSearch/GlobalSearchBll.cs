#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: GlobalSearchBll.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2009-2014
 *
 *  Description:
 *
 *  Notes:
 * $Id: GlobalSearchBll.cs 131464 2009-08-20 01:42:02Z ACHIEVO\jackie.yu $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using System;
using System.Web;

using Accela.ACA.BLL.Account;
using Accela.ACA.Common;
using Accela.ACA.Common.Log;
using Accela.ACA.WSProxy;

using log4net;

namespace Accela.ACA.BLL.GlobalSearch
{
    /// <summary>
    /// Global search business implement
    /// </summary>
    public class GlobalSearchBll : IGlobalSearchBll
    {
        #region Properties

        /// <summary>
        /// Gets an instance of GlobalSearchService.
        /// </summary>
        private GlobalSearchWebServiceService GlobalSearchService
        {
            get
            {
                return WSFactory.Instance.GetWebService<GlobalSearchWebServiceService>();
            }
        }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Execute query
        /// </summary>
        /// <param name="searchParam">the parameters for global search</param>
        /// <param name="viewElementNames">hidden element name array</param>
        /// <returns>the global search result</returns>
        /// <exception cref="ACAException">Exception from web service.</exception>
        public GlobalSearchResult4WS ExecuteQuery(GlobalSearchParam4WS searchParam, string[] viewElementNames)
        {
            try
            {
                User user = HttpContext.Current.Session[SessionConstant.SESSION_USER] as User;

                if (user  == null)
                {
                    throw new ACAException("No user in current session.");
                }

                return GlobalSearchService.executeQuery(searchParam, user.PublicUserId, viewElementNames);
            }
            catch (Exception e)
            {
                throw new ACAException(e);
            }
        }

        #endregion
    }
}
