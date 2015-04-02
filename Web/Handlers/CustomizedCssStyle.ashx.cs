#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: CustomizedCssStyle.ashx.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2010-2014
 *
 *  Description:
 *
 *  Notes:
 *      $Id: CustomizedCssStyle.ashx.cs 174597 2010-07-30 08:27:28Z ACHIEVO\vera.zhao $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using System.Web;
using System.Web.Services;
using System.Web.SessionState;

using Accela.ACA.Web.Common;

namespace Accela.ACA.Web.Handlers
{
    /// <summary>
    /// Output user own customized CSS style.
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    public class CustomizedCssStyle : IHttpHandler, IRequiresSessionState
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
            context.Response.ContentType = "text/css";
            string cssText = LabelUtil.GetTextContentByKey("aca_css_customizedstyle", ConfigManager.AgencyCode);

            context.Response.Write(cssText);
        }
    }
}