#region Header
/**
 * <pre>
 * 
 *  Accela Citizen Access
 *  File: ChangeSessionID.aspx.cs
 * 
 *  Accela, Inc.
 *  Copyright (C): 2013-2014
 * 
 *  Description:
 * </pre>
 */
#endregion

using System;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using Accela.ACA.BLL.Account;
using Accela.ACA.BLL.Common;
using Accela.ACA.Common;
using Accela.ACA.Common.Log;
using Accela.ACA.Common.Util;
using Accela.ACA.Web.Common;
using log4net;

namespace Accela.ACA.Web
{
    /// <summary>
    /// Change Session ID after user success login.
    /// </summary>
    public class ChangeSessionID : Page
    {
        /* ===================================================
         * This page in only used for internal authentication.
         * ===================================================*/

        /// <summary>
        /// Log object.
        /// </summary>
        private static readonly ILog Log = LogFactory.Instance.GetLogger(typeof(ChangeSessionID));

        /// <summary>
        /// Handle Page_Load event.
        /// </summary>
        /// <param name="sender">Trigger object.</param>
        /// <param name="e">Event argument.</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            /*
             * For Security reason, the session ID should be updated after user's success login.
             */

            if (Request.UrlReferrer == null || Page.IsPostBack || AppSession.User == null)
            {
                throw new InvalidOperationException();
            }

            // Keep the user context.
            User user = ObjectCloneUtil.DeepCopy(AppSession.User);

            if (Log.IsDebugEnabled)
            {
                Log.DebugFormat("A user logged in. User ID:{0}, user sequence:{1}", user.UserID, user.UserSeqNum);
                Log.DebugFormat("Drop current session. Session ID:{0}", Session.SessionID);
            }

            // Clear the authentication ticket and drop the current session.
            FormsAuthentication.SignOut();
            Session.Clear();
            Session.Abandon();

            // Create the new session ID.
            string newSessionId = AccountUtil.CreateNewSessionID(HttpContext.Current);

            if (Log.IsDebugEnabled)
            {
                Log.DebugFormat("New session created. Session ID:{0}", newSessionId);
            }

            // Save the user context to cache.
            var cacheManager = ObjectFactory.GetObject<ICacheManager>();
            cacheManager.AddSingleItemToCache(newSessionId, user, 60);
            string url = Request.UrlReferrer.AbsolutePath.EndsWith("SecurityQuestionVerification.aspx", StringComparison.InvariantCultureIgnoreCase)
                            ? ACAConstant.URL_DEFAULT
                            : Request.UrlReferrer.AbsoluteUri;
            
            // Keep the query strings.
            if (Request.QueryString.Count > 0)
            {
                if (url.IndexOf('?') > 0)
                {
                    url += "&";
                }
                else
                {
                    url += "?";
                }

                url += Request.QueryString.ToString();
            }

            if (Log.IsDebugEnabled)
            {
                Log.DebugFormat("Redirect to original Url:{0}", url);
            }

            // Redirect the original request Url.
            Response.Redirect(url);
        }
    }
}