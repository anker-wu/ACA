#region Header

/**
* <pre>
*  Accela Citizen Access
*  File: InternalAuthAdapter.cs
*
*  Accela, Inc.
*  Copyright (C): 2012-2014
*
*  Description: The internal authentication adapter implemented the IAuthAdapter interface.
*
* </pre>
*/

#endregion

using System.Web;
using System.Web.Security;
using Accela.ACA.Common;
using Accela.ACA.WSProxy;

namespace Accela.ACA.BLL.Account
{
    /// <summary>
    /// The internal authentication adapter implemented the <c>IAuthAdapter</c> interface.
    /// </summary>
    public class InternalAuthAdapter : IAuthAdapter
    {
        /// <summary>
        /// Gets a value indicating whether the current user is authenticated.
        /// </summary>
        public bool IsAuthenticated
        {
            get
            {
                return HttpContext.Current.Request.IsAuthenticated;
            }
        }

        /// <summary>
        /// Gets a value indicating whether the current authentication adapter is the embedded internal authentication adapter.
        /// Only the internal authentication adapter return true, all of other external authentication adapters must return false.
        /// </summary>
        public bool IsInternalAuthAdapter
        {
            get { return true; }
        }

        /// <summary>
        /// Gets the login Url for the current authentication adapter.
        /// </summary>
        public string LoginUrl
        {
            get
            {
                return FormsAuthentication.LoginUrl;
            }
        }

        /// <summary>
        /// Gets the logout Url for the current authentication adapter.
        /// </summary>
        public string LogoutUrl
        {
            get
            {
                return HttpContext.Current.Response.ApplyAppPathModifier("~/Logout.aspx");
            }
        }

        /// <summary>
        /// Gets the register Url for the current authentication adapter.
        /// </summary>
        public string RegisterUrl
        {
            get
            {
                return HttpContext.Current.Response.ApplyAppPathModifier("~/Account/RegisterDisclaimer.aspx");
            }
        }

        /// <summary>
        /// Redirect user to login page.
        /// </summary>
        public void RedirectToLoginPage()
        {
            FormsAuthentication.RedirectToLoginPage();
            HttpContext.Current.Response.End();
        }

        /// <summary>
        /// Redirect user to login page with the extra query strings.
        /// </summary>
        /// <param name="extraQueryString">Pass through the extra query strings to login page.</param>
        public void RedirectToLoginPage(string extraQueryString)
        {
            FormsAuthentication.RedirectToLoginPage(extraQueryString);
            HttpContext.Current.Response.End();
        }

        /// <summary>
        /// Redirect user to login page with the specified return Url and the extra query strings.
        /// </summary>
        /// <param name="returnUrl">Specify the return Url instead of the request Url.</param>
        /// <param name="extraQueryString">Pass through the extra query strings to login page.</param>
        public void RedirectToLoginPage(string returnUrl, string extraQueryString)
        {
            string loginUrl = LoginUrl;

            if (loginUrl.IndexOf('?') >= 0)
            {
                loginUrl += "&";
            }
            else
            {
                loginUrl += "?";
            }

            loginUrl += UrlConstant.RETURN_URL + "=" + HttpUtility.UrlEncode(returnUrl);

            if (!string.IsNullOrEmpty(extraQueryString))
            {
                loginUrl += "&" + extraQueryString;
            }

            HttpContext.Current.Response.Redirect(loginUrl);
        }

        /// <summary>
        /// Validate user's credentials and create mapping user in Accela system.
        /// </summary>
        /// <param name="username">User identity.</param>
        /// <param name="password">User password.</param>
        /// <returns>Result model.</returns>
        public PublicUserModel4WS ValidateUser(string username, string password)
        {
            IAccountBll accountBll = ObjectFactory.GetObject<IAccountBll>();
            return accountBll.ValidateUser(username, password);
        }
    }
}