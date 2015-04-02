#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: IAuthenticationAPI.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2014
 *
 *  Description:
 *
 *
 *  Notes:
 * $Id: IAuthenticationAPI.cs 277080 2014-08-11 06:28:07Z ACHIEVO\james.shi $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using System;
using Accela.ACA.SSOInterface.Model;

namespace Accela.ACA.SSOInterface
{
    /// <summary>
    /// Interface Authentication API
    /// </summary>
    public interface IACASSOContext
    {
        /// <summary>
        /// Gets a value indicating whether this instance is anonymous user.
        /// </summary>
        /// <value><c>true</c> if this instance is anonymous user; otherwise, <c>false</c>.</value>
        bool IsAnonymousUser { get; }

        /// <summary>
        /// Gets the protocol.
        /// </summary>
        /// <value>The protocol.</value>
        string Protocol { get; }

        /// <summary>
        /// Determines whether [is exist SSO relationship] [the specified external user name].
        /// </summary>
        /// <param name="externalUserName">Name of the external user.</param>
        /// <param name="accountType">Type of the account.</param>
        /// <returns><c>true</c> if [is exist SSO relationship] [the specified external user name]; otherwise, <c>false</c>.</returns>
        bool IsExistSSORelationship(string externalUserName, string accountType);

        /// <summary>
        /// Creates the user context.
        /// </summary>
        /// <param name="externalUserName">Name of the external user.</param>
        /// <param name="accountType">Type of the account.</param>
        /// <returns>User information.</returns>
        UserModel CreateUserContext(string externalUserName, string accountType);

        /// <summary>
        /// Redirects the automatic registration.
        /// </summary>
        /// <param name="userModel">The user model.</param>
        /// <param name="returnUrl">The return URL.</param>
        /// <param name="isWrapFrame">if set to <c>true</c> [is wrap frame].</param>
        void RedirectToRegistration(UserModel userModel, string returnUrl, bool isWrapFrame = false);

        /// <summary>
        /// Gets the user by SSO link token.
        /// </summary>
        /// <param name="token">The token.</param>
        /// <param name="isExpireSSOLink">if set to <c>true</c> [is expire SSO link].</param>
        /// <returns>User information.</returns>
        UserModel GetUserBySSOLinkToken(string token, bool isExpireSSOLink = false);

        /// <summary>
        /// Gets the unique identifier text by key.
        /// </summary>
        /// <param name="labelKey">The label key.</param>
        /// <returns>label text.</returns>
        string GetGUITextByKey(string labelKey);

        /// <summary>
        /// Redirects the with information iFrame.
        /// </summary>
        /// <param name="currentUrl">The current URL.</param>
        /// <param name="isWrapFrame">if set to <c>true</c> [is wrap frame].</param>
        void RedirectWithInIframe(string currentUrl, bool isWrapFrame = false);

        /// <summary>
        /// Show message to ACA default page.
        /// </summary>
        /// <param name="msg">Error message.</param>
        /// <param name="isForceRedirectDefault">Whether redirect to default page</param>
        void ShowMessage(string msg, bool isForceRedirectDefault = false);

        /// <summary>
        /// Get a value which indicates whether the specified HTTP request comes from Accela Mobile Citizen Access
        /// (AMCA).AMCA is a web application which is embedded in Accela Citizen Access. AMCA does not support anonymous user while Accela Citizen Access does.
        /// </summary>
        /// <returns>True for AMCA request, otherwise false.</returns>
        bool IsAmcaRequest();

        /// <summary>
        /// Get a value which indicates whether the specified HTTP request comes from Accela Mobile Citizen Access
        /// (AMCA).AMCA is a web application which is embedded in Accela Citizen Access. AMCA does not support anonymous user while Accela Citizen Access does.
        /// </summary>
        /// <param name="uri">Service URI.</param>
        /// <returns>True for AMCA request, otherwise false.</returns>
        bool IsAmcaRequest(Uri uri);

        /// <summary>
        /// Check current request url whether is raw request, rather than application error handle page request.
        /// </summary>
        /// <returns>True: raw request, False: application error handle page request.</returns>
        bool IsRawRequest();
    }
}