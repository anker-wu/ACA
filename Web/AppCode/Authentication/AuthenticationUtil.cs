#region Header

/**
* <pre>
*  Accela Citizen Access
*  File: AuthenticationUtil.cs
*
*  Accela, Inc.
*  Copyright (C): 2014
*
*  Description: The interface defined the common properties and motheds for authentication adapter.
*
* </pre>
*/

#endregion

using System;
using System.Web;
using Accela.ACA.BLL.Account;
using Accela.ACA.Common;
using Accela.ACA.SSOInterface;
using Accela.ACA.WSProxy;

namespace Accela.ACA.Web.Util
{
    /// <summary>
    /// Class Authentication UTIL.
    /// </summary>
    public static class AuthenticationUtil
    {
        /// <summary>
        /// The current adapter
        /// </summary>
        private static readonly IAuthAdapterBase CurrentAdapter = ObjectFactory.GetObject(typeof(IAuthAdapter)) as IAuthAdapterBase;

        /// <summary>
        /// Gets the current authentication adapter.
        /// </summary>
        /// <value>The current authentication adapter.</value>
        public static IAuthAdapterBase CurrentAuthAdapter
        {
            get
            {
                return CurrentAdapter;
            }
        }

        /// <summary>
        /// Gets a value indicating whether this instance is internal authentication adapter.
        /// </summary>
        /// <value><c>true</c> if this instance is internal authentication adapter; otherwise, <c>false</c>.</value>
        public static bool IsInternalAuthAdapter
        {
            get
            {
                return CurrentAdapter.IsInternalAuthAdapter;
            }
        }

        /// <summary>
        /// Gets a value indicating whether the current user is authenticated.
        /// </summary>
        public static bool IsAuthenticated
        {
            get
            {
                return CurrentAdapter.IsAuthenticated;
            }
        }

        /// <summary>
        /// Gets the login Url for the current authentication adapter.
        /// </summary>
        public static string LoginUrl
        {
            get
            {
                return CurrentAdapter.LoginUrl;
            }
        }

        /// <summary>
        /// Gets the logout Url for the current authentication adapter.
        /// </summary>
        public static string LogoutUrl
        {
            get
            {
                return CurrentAdapter.LogoutUrl;
            }
        }

        /// <summary>
        /// Gets the register Url for the current authentication adapter.
        /// </summary>
        public static string RegisterUrl
        {
            get
            {
                return CurrentAdapter.RegisterUrl;
            }
        }

        /// <summary>
        /// Gets a value indicating whether this instance is authentication adapter.
        /// </summary>
        /// <value><c>true</c> if this instance is authentication adapter; otherwise, <c>false</c>.</value>
        public static bool IsAuthAdapter
        {
            get
            {
                return CurrentAdapter is IAuthAdapter;
            }
        }

        /// <summary>
        /// Gets a value indicating whether this instance is authentication adapter v1.
        /// </summary>
        /// <value><c>true</c> if this instance is authentication adapter v1; otherwise, <c>false</c>.</value>
        public static bool IsAuthAdapterV1
        {
            get
            {
                return CurrentAdapter is IAuthAdapterV1;
            }
        }

        /// <summary>
        /// Gets the login URL target.
        /// </summary>
        /// <value>The login URL target.</value>
        public static string LoginUrlTarget
        {
            get
            {
                var tempAdapter = CurrentAuthAdapter as IAuthAdapterV1;

                if (tempAdapter == null)
                {
                    return string.Empty;
                }
                else
                {
                    return tempAdapter.LoginUrlTarget;
                }
            }
        }

        /// <summary>
        /// Gets a value indicating whether this instance is need registration.
        /// </summary>
        /// <value><c>true</c> if this instance is need registration; otherwise, <c>false</c>.</value>
        public static bool IsNeedRegistration
        {
            get
            {
                var tempAdapter = CurrentAuthAdapter as IAuthAdapterV1;

                if (tempAdapter == null)
                {
                    return false;
                }
                else
                {
                    return tempAdapter.IsNeedRegistration;
                }
            }
        }

        /// <summary>
        /// Redirect user to login page.
        /// </summary>
        public static void RedirectToLoginPage()
        {
            CurrentAdapter.RedirectToLoginPage();
        }

        /// <summary>
        /// Redirect user to login page with the extra query strings.
        /// </summary>
        /// <param name="extraQueryString">Pass through the extra query strings to login page.</param>
        public static void RedirectToLoginPage(string extraQueryString)
        {
            CurrentAdapter.RedirectToLoginPage(extraQueryString);
        }

        /// <summary>
        /// Redirect user to login page with the specified return Url and the extra query strings.
        /// </summary>
        /// <param name="returnUrl">specified the return Url instead of the request Url.</param>
        /// <param name="extraQueryString">Pass through the extra query strings to login page.</param>
        public static void RedirectToLoginPage(string returnUrl, string extraQueryString)
        {
            CurrentAdapter.RedirectToLoginPage(returnUrl, extraQueryString);
        }

        /// <summary>
        /// Validate user's credentials and create mapping user in Accela system.
        /// </summary>
        /// <param name="username">User identity.</param>
        /// <param name="password">User password.</param>
        /// <returns>public user information or error message</returns>
        public static PublicUserModel4WS ValidateUser(string username, string password)
        {
            IAuthAdapter authAdapter = CurrentAdapter as IAuthAdapter;

            if (authAdapter == null)
            {
                throw new NotImplementedException();
            }
            else
            {
                return authAdapter.ValidateUser(username, password);
            }
        }

        /// <summary>
        /// Signs the out.
        /// </summary>
        /// <param name="context">The context.</param>
        public static void Signout(HttpContext context)
        {
            IAuthAdapterV1 authAdapter = CurrentAdapter as IAuthAdapterV1;

            if (authAdapter == null)
            {
                return;
            }
            else
            {
                authAdapter.Signout(context);
            }
        }
    }
}