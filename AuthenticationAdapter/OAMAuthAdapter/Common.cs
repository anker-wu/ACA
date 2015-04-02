#region Header

/**
* <pre>
*  Accela Citizen Access
*  File: Common.cs
*
*  Accela, Inc.
*  Copyright (C): 2012
*
*  Description: The common functions for OAM authentication adapter.
*
* </pre>
*/

#endregion

using System;
using System.Web;
using Accela.ACA.Common.Util;

namespace Accela.ACA.OAMAccessGate
{
    /// <summary>
    /// The common functions for OAM authentication adapter.
    /// </summary>
    public static class Common
    {
        /// <summary>
        /// Gets the SSO cookie from the current http request.
        /// </summary>
        /// <param name="request">Current http request.</param>
        /// <returns>The value of the SSO cookie or null.</returns>
        public static string GetSSOCookie(HttpRequest request)
        {
            string cookieValue = null;
            HttpCookie cookie = request.Cookies[Constant.SSOCookieName];

            if (cookie != null)
            {
                cookieValue = cookie.Value;
            }

            return cookieValue;
        }

        /// <summary>
        /// Sets the SSO cookie to the current http response.
        /// </summary>
        /// <param name="request">Current http request.</param>
        /// <param name="response">Current http response.</param>
        /// <param name="cookieValue">The SSO cookie value.</param>
        public static void SetSSOCookie(HttpRequest request, ref HttpResponse response, string cookieValue)
        {
            HttpCookie cookie = new HttpCookie(Constant.SSOCookieName, cookieValue);
            cookie.HttpOnly = true;
            cookie.Secure = "HTTPS".Equals(request.Url.Scheme, StringComparison.OrdinalIgnoreCase);
            response.SetCookie(cookie);
        }

        /// <summary>
        /// Remove the SSO cookie from the current http response.
        /// </summary>
        /// <param name="request">Current http request.</param>
        /// <param name="response">Current http response.</param>
        public static void RemoveSSOCookie(HttpRequest request, ref HttpResponse response)
        {
            HttpCookie cookie = request.Cookies[Constant.SSOCookieName];

            if (cookie != null)
            {
                cookie.Expires = DateTime.Now.AddYears(-1);
                response.SetCookie(cookie);
            }
        }

        /// <summary>
        /// Gets a value based on the JustSignIn cookie to indicates whether the user is signed in.
        /// </summary>
        /// <param name="request">Current http request.</param>
        /// <param name="response">Current http response.</param>
        /// <returns>true or false.</returns>
        public static bool IsJustSignedIn(HttpRequest request, ref HttpResponse response)
        {
            bool isJustSignedIn = false;
            HttpCookie cookie = request.Cookies[Constant.CookieName_JustSignedIn];

            if (cookie != null && ValidationUtil.IsTrue(cookie.Value))
            {
                isJustSignedIn = true;
                cookie.Expires = DateTime.Now.AddYears(-1);
                response.SetCookie(cookie);
            }

            return isJustSignedIn;
        }

        /// <summary>
        /// Set the cookie to indicates the user is just signed in.
        /// </summary>
        /// <param name="request">Current http request.</param>
        /// <param name="response">Current http response.</param>
        public static void SetSignInFlagCookie(HttpRequest request, ref HttpResponse response)
        {
            HttpCookie cookieSignInFlag = new HttpCookie(Constant.CookieName_JustSignedIn, "true");
            cookieSignInFlag.HttpOnly = true;
            cookieSignInFlag.Secure = "HTTPS".Equals(request.Url.Scheme, StringComparison.OrdinalIgnoreCase);
            response.SetCookie(cookieSignInFlag);
        }
    }
}