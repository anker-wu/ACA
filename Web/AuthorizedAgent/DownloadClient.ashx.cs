#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: DownloadClient.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2013-2014
 *
 *  Description:
 *
 *  Notes:
 *      
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using System.IO;
using System.Web;
using System.Web.SessionState;
using Accela.ACA.Web.Common;

namespace Accela.ACA.Web.AuthorizedAgent
{
    /// <summary>
    /// The Download Client
    /// </summary>
    public class DownloadClient : IHttpHandler, IRequiresSessionState
    {
        /// <summary>
        /// Gets a value indicating whether another request can use the <see cref="T:System.Web.IHttpHandler" /> instance.
        /// </summary>
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
            // Prevent the citizen user or anonymous user to download the Authorized Agent Client execute file.
            if (AppSession.User == null || (!AppSession.User.IsAuthorizedAgent && !AppSession.User.IsAgentClerk))
            {
                return;
            }

            string fileNmae = "AuthorizedAgentClient.cs";
            string outputFileName = "AuthorizedAgentClient.exe";
            FileStream fs = new FileStream(context.Request.MapPath("~/ClientBin/" + fileNmae), FileMode.Open, FileAccess.Read);
            byte[] buffer = new byte[fs.Length];
            fs.Read(buffer, 0, buffer.Length);
            fs.Close();

            context.Response.Clear();
            context.Response.AddHeader("Content-Disposition", "attachment; filename=" + outputFileName);
            context.Response.AddHeader("Content-Length", buffer.Length.ToString());

            //resolve the filename with empty space char
            context.Response.ContentType = "application/octet-stream";
            context.Response.OutputStream.Write(buffer, 0, buffer.Length);
            context.Response.Flush();
            context.Response.Close();
        }
    }
}