#region Header

/**
* <pre>
*  Accela Citizen Access
*  File: Constant.cs
*
*  Accela, Inc.
*  Copyright (C): 2012
*
*  Description: The common constant variables for OAM authentication adapter.
*
* </pre>
*/

#endregion

namespace Accela.ACA.OAMAccessGate
{
    /// <summary>
    /// The common constant variables for OAM authentication adapter.
    /// </summary>
    public static class Constant
    {
        /// <summary>
        /// OAM configuration section name.
        /// </summary>
        public const string ConfigSectionName = "OAMConfiguration";

        /// <summary>
        /// A cookie name to indicates a user status that user is just signed in.
        /// </summary>
        public const string CookieName_JustSignedIn = "OAMSignedIn";

        /// <summary>
        /// OAM user ID in policy response after reqeust resource is authorizte.
        /// </summary>
        public const string HeaderName_OAMUser = "OAM_REMOTE_USER";

        /// <summary>
        /// Policy response type name of the Header variable.
        /// </summary>
        public const string PolicyResponse_HeaderType = "HeaderVar";

        /// <summary>
        /// Policy response value in OAM SDK to indicates the response is not found.
        /// </summary>
        public const string PolicyResponse_NotFound = "NOT_FOUND";

        /// <summary>
        /// A session key to indicates the last user ID.
        /// </summary>
        public const string SessionName_LastUserID = "OAMLastUserID";

        /// <summary>
        /// Single sign-on cookie name for OAM webgate 10g.
        /// </summary>
        public const string SSOCookieName = "ObSSOCookie";

        /// <summary>
        /// Single sign-on cookie value before login.
        /// </summary>
        public const string SSOCookieValue_BeforeLogin = "loggedoutcontinue";
    }
}