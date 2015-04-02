#region Header

/**
* <pre>
*  Accela Citizen Access
*  File: Constant.cs
*
*  Accela, Inc.
*  Copyright (C): 2014
*
*  Description: The common constant variables for RealMe authentication adapter.
*
* </pre>
*/

#endregion

namespace Accela.ACA.RealMeAccessGate
{
    /// <summary>
    /// The common constant variables for RealMe authentication adapter.
    /// </summary>
    public static class Constant
    {
        /// <summary>
        /// RealMe configuration section name.
        /// </summary>
        public const string ConfigSectionName = "appSettings";

        /// <summary>
        /// <see cref="SAMLart"/> cookie name.
        /// </summary>
        public const string RealMeParamName_SAMLart = "SAMLart";

        /// <summary>
        /// RelayState cookie name.
        /// </summary>
        public const string RealMeParamName_RelayState = "RelayState";

        /// <summary>
        /// <see cref="SigAlg"/> cookie name.
        /// </summary>
        public const string RealMeParamName_SigAlg = "SigAlg";

        /// <summary>
        /// signature cookie name.
        /// </summary>
        public const string RealMeParamName_Signature = "Signature";

        /// <summary>
        /// The RealMe account type.
        /// </summary>
        public const string SSO_ACCOUNT_TYPE_REALME = "RealMe";

        /// <summary>
        /// The email request query token
        /// </summary>
        public const string EMAIL_QUERY_TOKEN = "token";

        /// <summary>
        /// Temporary user session
        /// </summary>
        public const string TEMPORARY_USER_SESSION = "UserModel";

        /// <summary>
        /// session security token
        /// </summary>
        public const string SESSION_SECURITY_TOKEN = "SecurityToken";

        /// <summary>
        /// Http status code - 404
        /// </summary>
        public const int HTTP_STATUS_CODE_404_RESOURCE_NOT_FOUND = 404;

        /// <summary>
        /// Is from real me
        /// </summary>
        public const string IS_FROM_REAL_ME = "fromRealMe";

        /// <summary>
        /// Is from SSO link handler.
        /// </summary>
        public const string IS_FROM_SSO_LINK_HANDLER = "fromSSOLinkHandler";

        /// <summary>
        /// The ACA session id 
        /// </summary>
        public const string ACA_SESSION_ID = "acaSessionId";

        /// <summary>
        /// The RealMe destination Url.
        /// </summary>
        public const string REQUESTED_REALME_URL = "realMeUrl";

        /// <summary>
        /// common Y constant.
        /// </summary>
        public const string COMMON_Y = "Y";

        /// <summary>
        /// RealMe login status - returned by validateLogin interface
        /// </summary>
        public enum RealMeLoginStatus
        {
            /// <summary>
            /// Unknown status.
            /// </summary>
            None,

            /// <summary>
            /// User login successfully.
            /// </summary>
            SUCCESS,

            /// <summary>
            /// User cancelled.
            /// </summary>
            CANCELLED,

            /// <summary>
            /// Error happened.
            /// </summary>
            ERROR,

            /// <summary>
            /// User is deleted.
            /// </summary>
            ERROR_USER_DELETED,

            /// <summary>
            /// User is inactive.
            /// </summary>
            ERROR_USER_INACTIVE
        }
    }
}