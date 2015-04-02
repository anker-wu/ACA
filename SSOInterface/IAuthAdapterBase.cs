#region Header

/**
* <pre>
*  Accela Citizen Access
*  File: IAuthAdapterBase.cs
*
*  Accela, Inc.
*  Copyright (C): 2014
*
*  Description: The people model.
*
* </pre>
*/

#endregion

namespace Accela.ACA.SSOInterface
{
    /// <summary>
    /// Interface of base Authentication Adapter
    /// </summary>
    public interface IAuthAdapterBase
    {
        /// <summary>
        /// Gets a value indicating whether the current user is authenticated.
        /// </summary>
        bool IsAuthenticated
        {
            get;
        }

        /// <summary>
        /// Gets a value indicating whether the current authentication adapter is the embedded internal authentication adapter.
        /// Use the ACA embedded Login/Logout page means it's internal adapter. The LDAP authentication is use the internal adapter.
        /// Only the internal authentication adapter return Yes, all other external authentication adapters must return No.
        /// </summary>
        bool IsInternalAuthAdapter
        {
            get;
        }

        /// <summary>
        /// Gets the login Url for the current authentication adapter.
        /// </summary>
        string LoginUrl
        {
            get;
        }

        /// <summary>
        /// Gets the logout Url for the current authentication adapter.
        /// </summary>
        string LogoutUrl
        {
            get;
        }

        /// <summary>
        /// Gets the register Url for the current authentication adapter.
        /// </summary>
        string RegisterUrl
        {
            get;
        }

        /// <summary>
        /// Redirect user to login page.
        /// </summary>
        void RedirectToLoginPage();

        /// <summary>
        /// Redirect user to login page with the extra query strings.
        /// </summary>
        /// <param name="extraQueryString">Pass through the extra query strings to login page.</param>
        void RedirectToLoginPage(string extraQueryString);

        /// <summary>
        /// Redirect user to login page with the specified return Url and the extra query strings.
        /// </summary>
        /// <param name="returnUrl">specified the return Url instead of the request Url.</param>
        /// <param name="extraQueryString">Pass through the extra query strings to login page.</param>
        void RedirectToLoginPage(string returnUrl, string extraQueryString);
    }
}
