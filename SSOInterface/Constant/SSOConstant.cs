#region Header

/**
* <pre>
*  Accela Citizen Access
*  File: SSOConstant.cs
*
*  Accela, Inc.
*  Copyright (C): 2014
*
*  Description: The people model.
*
* </pre>
*/

#endregion

namespace Accela.ACA.SSOInterface.Constant
{
    /// <summary>
    /// Class SSO Constant
    /// </summary>
    public static class SSOConstant
    {
        #region Fields

        /// <summary>
        /// AMCA default page
        /// </summary>
        public const string AMCA_DEFAULT_PAGE = "~/amca/default.aspx";

        /// <summary>
        /// ACA register url.
        /// </summary>
        public const string ACA_REGISTER_URL = "Account/RegisterDisclaimer.aspx";

        /// <summary>
        /// url welcome page
        /// </summary>
        public const string URL_WELCOME_PAGE = "Welcome.aspx";

        /// <summary>
        /// url logout page
        /// </summary>
        public const string URL_LOGOUT = "Logout.aspx";

        #endregion Fields

        /// <summary>
        /// ENUM Gender
        /// </summary>
        public enum Gender
        {
            /// <summary>
            /// not specific gender
            /// </summary>
            None,

            /// <summary>
            /// female tag
            /// </summary>
            F,

            /// <summary>
            /// male tag
            /// </summary>
            M
        }
    }
}
