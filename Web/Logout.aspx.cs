/**
 * <pre>
 * 
 *  Accela Citizen Access
 *  File: Logout.aspx.cs
 * 
 *  Accela, Inc.
 *  Copyright (C): 2007-2014
 * 
 *  Description:
 *  login page.
 * 
 *  Notes:
 *      $Id: Logout.aspx.cs 278243 2014-08-29 10:19:31Z ACHIEVO\james.shi $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

using System;
using System.Web;
using System.Web.Security;
using System.Web.UI;

using Accela.ACA.SSOInterface;
using Accela.ACA.Web.Common;
using Accela.ACA.Web.Util;

/// <summary>
/// class for logout.
/// </summary>
public partial class Logout : Page
{
    /// <summary>
    /// Gets the current authentication adapter.
    /// </summary>
    protected IAuthAdapterBase CurrentAuthAdapter
    {
        get
        {
            return AuthenticationUtil.CurrentAuthAdapter;
        }
    }

    /* ===================================================
     * This page in only used for internal authentication.
     * ===================================================*/

    /// <summary>
    /// Handle <c>OnInit</c> event.
    /// </summary>
    /// <param name="e">Event arguments.</param>
    protected override void OnInit(EventArgs e)
    {
        base.OnInit(e);

        /*
         * For security reason, do not allow unauthenticated user and external resource to access the logout page,
         *  to resolve below security issue by IBM AppScan:
         * -----
         * The same request was sent twice in different sessions and the same response was received.
         * This shows that none of the parameters are dynamic (session identifiers are sent only in cookies)
         *  and therefore that the application is vulnerable to this issue.
         * -----
         */
        if (!Request.IsAuthenticated
            || (Request.UrlReferrer != null && FileUtil.IsExternalUrl(Request.UrlReferrer.AbsoluteUri)))
        {
            throw new InvalidOperationException();
        }
    }

    /// <summary>
    /// Raises the page load event
    /// </summary>
    /// <param name="sender">An object that contains the event sender.</param>
    /// <param name="e">A System.EventArgs object containing the event data.</param>
    protected void Page_Load(object sender, EventArgs e)
    {
        //this logout page is used to avoid history.back button issue, when user logout and then click history.back,
        //we will go back to login page and the user message will display to us even though we has signout.

        /* In order to solve the issue "Session Not Invalidated After Logout" that was reported by AppScan*/

        // Clear the authentication ticket
        FormsAuthentication.SignOut();

        // Clear the contents of the current session
        Session.Clear();

        // Tell the system to drop the session reference so that it does 
        // not need to be carried around with the user
        Session.Abandon();

        //Change session ID after user logout.
        string sessionCookieName = ConfigManager.SessionStateCookieName;

        if (Request.Cookies[sessionCookieName] != null)
        {
            // Create the new session ID.
            AccountUtil.CreateNewSessionID(HttpContext.Current);
        }

        //Clear external account login status.
        AuthenticationUtil.Signout(Context);
        
        //for IE, we use window.location.href="Login.aspx";
        //for Firefox, we must click the loginLink link to redirect, in order to redirect automatically, we use
        //lnk.dispatchEvent(evt)(see the code in logout.aspx) to call the linkbutton event
        loginLink.PostBackUrl = "Login.aspx";
        if (Request.Params[postEventSourceID] == "loginLink")
        {
            LoginLink_Click(null, null);
        }
    }

    /// <summary>
    /// loginLink Click
    /// </summary>
    /// <param name="sender">object sender</param>
    /// <param name="e">EventArgs e</param>
    protected void LoginLink_Click(object sender, EventArgs e)
    {
        Response.Redirect("~/Login.aspx");
    }
}