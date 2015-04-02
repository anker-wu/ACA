#region Header

/*
 * <pre>
 *  Accela Citizen Access
 *  File: AntiCsrfModule.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2013-2014
 *
 *  Description: A http module to prevent the Cross-Site Request Forgery attacks.
 *
 * </pre>
 */

#endregion

using System;
using System.Web;
using System.Web.UI;
using Accela.ACA.Common.Log;
using log4net;

namespace Accela.ACA.Web.Security
{
    /// <summary>
    /// A http module to prevent the Cross-Site Request Forgery attacks.
    /// </summary>
    public class AntiCsrfModule : IHttpModule
    {
        /// <summary>
        /// Form field name used to register a hidden field into the web form and to validate each POST request.
        /// </summary>
        private const string FormFieldName = "CSRF_TOKEN";

        /// <summary>
        /// Cookie name used to validate each POST request.
        /// </summary>
        private const string CookieName = "ACA_CSRF_TOKEN";

        /// <summary>
        /// Context item name used to temporary store the token into http context.
        /// </summary>
        private const string ContextItemName = "CSRF_TOKEN";

        /// <summary>
        /// Log instance.
        /// </summary>
        private static readonly ILog Log = LogFactory.Instance.GetLogger(typeof(AntiCsrfModule));

        /// <summary>
        /// Dispose resources.
        /// </summary>
        public void Dispose()
        {
        }

        /// <summary>
        /// Initialize the CSRF handler.
        /// </summary>
        /// <param name="context">Current Http Application instance.</param>
        public void Init(HttpApplication context)
        {
            context.PostMapRequestHandler += new EventHandler(PostMapRequestEventHandler);
            context.PreRequestHandlerExecute += new EventHandler(PreRequestHandlerExecute);
            context.PreSendRequestHeaders += new EventHandler(PreSendRequestHeaders);
        }

        /// <summary>
        /// Handle the Application PostMapRequest event.
        /// </summary>
        /// <param name="sender">Trigger object.</param>
        /// <param name="e">Event arguments.</param>
        protected void PostMapRequestEventHandler(object sender, EventArgs e)
        {
            // Security to check Referer in http header
            HttpApplication application = sender as HttpApplication;

            if (application != null
                && application.Context != null)
            {
                if (AntiCsrfAttackUtil.IsNeedCheckUrlReferrer(application.Context))
                {
                    if (AntiCsrfAttackUtil.IsCsrfAttackByUrlReferrer(application.Context))
                    {
                        RaiseCsrfException("Untrusted Post Request.");
                    }
                }
            }
        }

        /// <summary>
        /// Handle the PreRequestHandlerExecute event to validate the HTTP request to prevent CSRF attack.
        /// </summary>
        /// <param name="sender">Trigger object, should be a HttpApplication instance.</param>
        /// <param name="e">Event arguments.</param>
        private void PreRequestHandlerExecute(object sender, EventArgs e)
        {
            HttpApplication application = sender as HttpApplication;

            if (application != null && application.Context != null && application.Context.Handler != null)
            {
                HttpContext context = application.Context;
                bool isSuppressed = AntiCsrfAttackUtil.IsSuppressed(context.Handler);

                Page page = context.Handler as Page;

                if (!isSuppressed && page != null)
                {
                    page.Load += new EventHandler(PageLoad);
                    page.PreRender += new EventHandler(PagePreRender);

                    // If a POST request do not have the matched Cookie token & Form token, treat it as the CSRF attack.
                    if ("POST".Equals(context.Request.RequestType, StringComparison.OrdinalIgnoreCase)
                        && "POST".Equals(context.Request.HttpMethod, StringComparison.OrdinalIgnoreCase))
                    {
                        HttpCookie csrfCookie = context.Request.Cookies[CookieName];
                        string csrfFieldValue = context.Request.Form[FormFieldName];

                        if (csrfCookie == null || string.IsNullOrEmpty(csrfFieldValue) || !csrfFieldValue.Equals(csrfCookie.Value))
                        {
                            if (csrfCookie != null)
                            {
                                csrfCookie.Expires = DateTime.Now.AddDays(-1);
                            }

                            Log.ErrorFormat("Capture a potential CSRF forgery attack. Request url:{0}", context.Request.Url.AbsoluteUri);
                            RaiseCsrfException("The CSRF token does not match.");
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Handle the PreSendRequestHeaders event to create the validation cookie.
        /// </summary>
        /// <param name="sender">Trigger object, should be a HttpApplication instance.</param>
        /// <param name="e">Event arguments.</param>
        private void PreSendRequestHeaders(object sender, EventArgs e)
        {
            HttpApplication application = sender as HttpApplication;

            if (application != null && application.Context != null)
            {
                HttpContext context = application.Context;

                if (context.Items[ContextItemName] != null)
                {
                    HttpCookie csrfCookie = new HttpCookie(CookieName, context.Items[ContextItemName].ToString())
                    {
                        HttpOnly = true,
                        Secure = "HTTPS".Equals(context.Request.Url.Scheme, StringComparison.OrdinalIgnoreCase)
                    };
                    context.Response.AppendCookie(csrfCookie);
                }
            }
        }

        /// <summary>
        /// Handle Page Load event and to validate the special request.
        /// </summary>
        /// <param name="sender">Trigger object, should be a Page instance.</param>
        /// <param name="e">Event arguments.</param>
        private void PageLoad(object sender, EventArgs e)
        {
            Page page = sender as Page;

            // If the Page is post-back and the http method is GET, treat as the CSRF.
            if (page != null && page.IsPostBack && "GET".Equals(page.Request.HttpMethod, StringComparison.OrdinalIgnoreCase))
            {
                Log.ErrorFormat("Capture a potential CSRF attack, Page is PostBack but http method is GET. Request url:{0}", page.Request.Url.AbsoluteUri);
                RaiseCsrfException("The http mothod for the POST request is GET.");
            }
        }

        /// <summary>
        /// Handle Page PreRender event and to generate token and render the hidden field.
        /// </summary>
        /// <param name="sender">Trigger object, should be a Page instance.</param>
        /// <param name="e">Event arguments.</param>
        private void PagePreRender(object sender, EventArgs e)
        {
            Page page = sender as Page;

            if (page != null && page.Form != null)
            {
                string csrfToken;
                HttpContext context = HttpContext.Current;
                HttpCookie csrfCookie = page.Request.Cookies[CookieName];

                if (csrfCookie == null || string.IsNullOrEmpty(csrfCookie.Value))
                {
                    csrfToken = Guid.NewGuid().ToString("N");
                    context.Items[ContextItemName] = csrfToken;
                }
                else
                {
                    csrfToken = csrfCookie.Value;
                    Guid guid;

                    if (!Guid.TryParse(csrfToken, out guid))
                    {
                        csrfCookie.Expires = DateTime.Now.AddDays(-1);

                        Log.ErrorFormat("Capture a potential CSRF attack, Invalid CSRF token. Request url:{0}", page.Request.Url.AbsoluteUri);
                        RaiseCsrfException("The CSRF token is invalid.");
                    }
                }

                ScriptManager.RegisterHiddenField(page, FormFieldName, csrfToken);
            }
        }

        /// <summary>
        /// Throw the exception if occurs the potential cross-site request forgery attacks.
        /// </summary>
        /// <param name="cause">Detail cause.</param>
        private void RaiseCsrfException(string cause)
        {
            throw new InvalidOperationException("Potential cross-site request forgery attacks." + cause);
        }
    }
}