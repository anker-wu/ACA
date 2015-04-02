#region Header

/**
* <pre>
*  Accela Citizen Access
*  File: ActionFilterModule.cs
*
*  Accela, Inc.
*  Copyright (C): 2013-2014
*
*  Description: Url routing http handler.
*
*  Notes:
* $Id: ActionFilterModule.cs 171222 2010-04-21 16:10:00Z ACHIEVO\alan.hu $.
*  Revision History
*  Date,            Who,        What
*  May 30, 2013      Daniel Shi     Initial.
* </pre>
*/

#endregion

using System;
using System.Web;
using System.Web.UI;
using Accela.ACA.Common;
using Accela.ACA.Common.Log;
using Accela.ACA.Web.AuthorizedAgent;
using Accela.ACA.Web.SocialMedia;
using Accela.ACA.Web.Util;
using log4net;

namespace Accela.ACA.Web.Common
{
    /// <summary>
    /// Action filter module.
    /// </summary>
    public class ActionFilterModule : IHttpModule
    {
        #region IHttpModule Fields

        /// <summary>
        /// Logger instance.
        /// </summary>
        protected static readonly ILog Logger = LogFactory.Instance.GetLogger(typeof(ActionFilterModule));

        #endregion

        #region IHttpModule Methods

        /// <summary>
        /// release resource
        /// </summary>
        public void Dispose()
        {
        }

        /// <summary>
        /// Initializes the module by hooking the application's BeginRequest event if indicated by the config settings.
        /// </summary>
        /// <param name="context">The HttpApplication this module is bound to.</param>
        public void Init(HttpApplication context)
        {
            if (context != null)
            {
                context.AcquireRequestState += new EventHandler(Context_AcquireRequestState);
            }
        }

        /// <summary>
        /// redirect to https if current is http and needs to redirect
        /// </summary>
        /// <param name="sender">The sender object.</param>
        /// <param name="e">The event arguments.</param>
        private void Context_AcquireRequestState(object sender, EventArgs e)
        {
            HttpContext context = HttpContext.Current;
            var requestPath = context.Request.Url.AbsolutePath;

            if (requestPath.EndsWith(ACAConstant.URL_WELCOME_PAGE, StringComparison.InvariantCultureIgnoreCase))
            {
                var user = AppSession.User;
                SocialMediaUtil.TryRedirectToFacebookHome(false);

                if (user != null && !user.IsAnonymous)
                {
                    if (user.IsAgentClerk)
                    {
                        context.Response.Redirect("~/AuthorizedAgent/FishingAndHunttingLicense/SearchCustomer.aspx", true);
                    }
                    else if (user.IsAuthorizedAgent)
                    {
                        context.Response.Redirect("~/Account/AccountManager.aspx", true);
                    }
                    else if (user.IsInspector
                            && StandardChoiceUtil.IsEnabledUploadInspectionResult()
                            && AuthenticationUtil.IsInternalAuthAdapter
                            && user.Licenses != null && user.Licenses.Length > 0
                            && context.Request.UrlReferrer != null
                            && (context.Request.UrlReferrer.LocalPath.EndsWith(AuthenticationUtil.LoginUrl, StringComparison.InvariantCultureIgnoreCase) // From Login to Welcome
                               || context.Request.UrlReferrer.LocalPath.EndsWith("Account/ChangePassword.aspx", StringComparison.InvariantCultureIgnoreCase)
                               || context.Request.UrlReferrer.LocalPath.EndsWith("Account/SecurityQuestionVerification.aspx", StringComparison.InvariantCultureIgnoreCase)
                               || context.Request.UrlReferrer.LocalPath.EndsWith("Account/SecurityQuestionUpdate.aspx", StringComparison.InvariantCultureIgnoreCase)
                               || context.Request.UrlReferrer.LocalPath.EndsWith("Account/RegisterConfirm.aspx", StringComparison.InvariantCultureIgnoreCase)
                               || (context.Request.Form["__EVENTTARGET"] == null && context.Request.UrlReferrer.LocalPath.EndsWith(ACAConstant.URL_WELCOME_PAGE, StringComparison.InvariantCultureIgnoreCase) && string.IsNullOrEmpty(context.Request.UrlReferrer.Query)) /* From Welcome(Login Box) to Welcome */))
                    {
                        context.Response.Redirect("~/Inspection/InspectionResultUpload.aspx?TabName=APO");
                    }
                }
            }
            else if (requestPath.EndsWith("SearchCustomer.aspx", StringComparison.InvariantCultureIgnoreCase)
                || requestPath.EndsWith("CustomerDetail.aspx", StringComparison.InvariantCultureIgnoreCase))
            {
                /*
                 * Do not allow anonymous user and normal regisgered user(not Agent and not Clerk) to access to
                 *  SearchCustomer page and CustomerDetail page, these two pages are special for Authorized Agent
                 *  and Authorized Agent Clerk.
                 */
                if (!AppSession.IsAdmin)
                {
                    if (AppSession.User == null || AppSession.User.IsAnonymous)
                    {
                        AuthenticationUtil.RedirectToLoginPage();
                    }
                    else if (!string.IsNullOrEmpty(HttpContext.Current.Request[Page.postEventSourceID])
                        && !AuthorizedAgentServiceUtil.HasAuthorizedServiceConfig())
                    {
                        // Disallow any action operation when then Setting or Client or Printer in invalid status.
                        HttpContext.Current.Response.Redirect(AuthenticationUtil.LogoutUrl);
                    }
                    else if (!AppSession.User.IsAgentClerk && !AppSession.User.IsAuthorizedAgent)
                    {
                        context.Response.Redirect(ACAConstant.URL_DEFAULT, true);
                    }
                }
            }
            else if (requestPath.EndsWith("AccountManager.aspx", StringComparison.InvariantCultureIgnoreCase))
            {
                /*
                 * Do not allow Agent Clerk to access to Account Manager page.
                 */
                if (AppSession.User != null && AppSession.User.IsAgentClerk)
                {
                    context.Response.Redirect("~/AuthorizedAgent/FishingAndHunttingLicense/SearchCustomer.aspx", true);
                }
            }
        }

        #endregion
    }
}