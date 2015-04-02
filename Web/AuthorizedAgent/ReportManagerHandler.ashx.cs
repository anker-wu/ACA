#region Header
/**
 * <pre>
 * 
 *  Accela Citizen Access
 *  File: ReportManagerHandler.ashx.cs
 * 
 *  Accela, Inc.
 *  Copyright (C): 2013-2014
 * 
 *  Description:
 * 
 *  Notes:
 * $Id: ReportManagerHandler.cs 181867 2011-09-27 08:06:18Z ACHIEVO\daly.zeng $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */
#endregion Header

using System;
using System.IO;
using System.Web;
using Accela.ACA.Common.Log;
using Accela.ACA.Web.Common;
using log4net;

namespace Accela.ACA.Web.AuthorizedAgent
{
    /// <summary>
    /// The Report Manager Handler
    /// </summary>
    public class ReportManagerHandler : IHttpHandler
    {
        /// <summary>
        /// The delete action
        /// </summary>
        protected static readonly string DELETE_ACTION = "Delete";

        /// <summary>
        /// Logger instance.
        /// </summary>
        protected static readonly ILog Logger = LogFactory.Instance.GetLogger(typeof(ReportManagerHandler));

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
            HttpRequest request = context.Request;
            string fileName = request.QueryString["fileName"];
            string action = request.QueryString["action"];

            Logger.InfoFormat("Handle the print label report. File name - {0}, action - {1}", fileName, action);

            if (string.IsNullOrEmpty(action))
            {
                if (!string.IsNullOrEmpty(fileName))
                {
                    string fullPath = ConfigManager.TempDirectory + "\\" + fileName;

                    try
                    {
                        FileStream fs = new FileStream(fullPath, FileMode.Open);
                        byte[] buffer = new byte[fs.Length];
                        fs.Read(buffer, 0, buffer.Length);
                        fs.Close();

                        context.Response.Clear();
                        context.Response.AddHeader("Content-Disposition", "attachment; filename=" + HttpUtility.UrlEncode(fileName).Replace("+", "%20"));
                        context.Response.AddHeader("Content-Length", buffer.Length.ToString());

                        //resolve the filename with empty space char
                        context.Response.ContentType = "application/octet-stream";
                        context.Response.OutputStream.Write(buffer, 0, buffer.Length);
                    }
                    catch (Exception exception)
                    {
                        Logger.ErrorFormat("An error occurred when getting the file {0}", fileName);
                        throw exception;
                    }
                    finally
                    {
                        context.Response.Flush();
                        context.Response.Close();
                    }
                }
            }
            else if (DELETE_ACTION.Equals(action, StringComparison.InvariantCultureIgnoreCase))
            {
                string fullPath = ConfigManager.TempDirectory + "\\" + fileName;

                if (File.Exists(fullPath))
                {
                    File.Delete(fullPath);
                }

                Logger.DebugFormat("The file {0} was deleted successfully.", fileName);
            }
        }
    }
}
