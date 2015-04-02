#region Header

/**
* <pre>
*  Accela Citizen Access
*  File: RefreshSessionHandler.ashx.cs
*
*  Accela, Inc.
*  Copyright (C): 2014
*
*  Description: Http handler for files.
*
*  Notes:
* $Id: RefreshSessionHandler.ashx.cs 171222 2010-04-21 16:10:00Z ACHIEVO\alan.hu $.
*  Revision History
*  Date,            Who,        What
*  Sep 23, 2011      Alan Hu     Initial.
* </pre>
*/

#endregion

using System.Web;
using System.Web.SessionState;
using Accela.ACA.Web.Util;

namespace Accela.ACA.Web.Handlers
{
    /// <summary>
    /// Refresh the session and update the latest request time when handle a request.
    /// </summary>
    public class RefreshSessionHandler : IHttpHandler, IRequiresSessionState
    {
        /// <summary>
        /// Gets a value indicating whether another request can use the <see cref="T:System.Web.IHttpHandler" /> instance.
        /// </summary>
        /// <value><c>true</c> if this instance is reusable; otherwise, <c>false</c>.</value>
        /// <returns>true if the <see cref="T:System.Web.IHttpHandler" /> instance is reusable; otherwise, false.</returns>
        public bool IsReusable
        {
            get
            {
                return false;
            }
        }

        /// <summary>
        /// Enables processing of HTTP Web requests by a custom HttpHandler that implements the <see cref="T:System.Web.IHttpHandler" /> interface.
        /// </summary>
        /// <param name="context">An <see cref="T:System.Web.HttpContext" /> object that provides references to the intrinsic server objects (for example, Request, Response, Session, and Server) used to service HTTP requests.</param>
        public void ProcessRequest(HttpContext context)
        {
            //Update the lastest request time.
            SessionTimeoutUtil.UpdateLastestRequestTime(context.Request, context.Response);
        }
    }
}