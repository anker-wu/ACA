#region Header

/**
* <pre>
*  Accela Citizen Access
*  File: CookieConstant.cs
*
*  Accela, Inc.
*  Copyright (C): 2013-2014
*
*  Description: A static class to define common cookie constant variables.
*
* </pre>
*/

#endregion

namespace Accela.ACA.Common
{
    /// <summary>
    /// A static class to define common cookie constant variables.
    /// </summary>
    public static class CookieConstant
    {
        /// <summary>
        /// Cookie key of the browser compatibility check status for current session.
        /// </summary>
        public const string BROWSER_DETECT_FLAG = "ACA_BROWSER_DETECT_FLAG";

        /// <summary>
        /// Cookie key of user preferred culture.
        /// </summary>
        public const string USER_PREFERRED_CULTURE = "ACA_USER_PREFERRED_CULTURE";

        /// <summary>
        /// Cookie key of announcement status.
        /// </summary>
        public const string USER_READ_ANNOUNCEMENT = "ACA_USER_READ_ANNOUNCEMENT";

        /// <summary>
        /// Cookie key of accessibility support status.
        /// </summary>
        public const string SUPPORT_ACCESSSIBILITY = "ACA_COOKIE_SUPPORT_ACCESSSIBILITY";

        /// <summary>
        /// Cookie key of remembered login user name.
        /// </summary>
        public const string REMEMBERED_USER_NAME = "ACA_REMEMBERED_USER_NAME";

        /// <summary>
        /// Cookie key of remembered latest request time.
        /// </summary>
        public const string LASTEST_REQUEST_TIME = "LASTEST_REQUEST_TIME";
    }
}