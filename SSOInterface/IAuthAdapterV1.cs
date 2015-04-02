#region Header

/**
* <pre>
*  Accela Citizen Access
*  File: IAuthAdapterV1.cs
*
*  Accela, Inc.
*  Copyright (C): 2014
*
*  Description: The interface defined the common properties and motheds for authentication adapter.
*
* </pre>
*/

#endregion

using System.Web;

namespace Accela.ACA.SSOInterface
{
    /// <summary>
    /// The interface defined the common properties and methods for authentication adapter.
    /// </summary>
    public interface IAuthAdapterV1 : IAuthAdapterBase
    {
        /// <summary>
        /// Gets the login url target.
        /// </summary>
        string LoginUrlTarget
        {
            get;
        }

        /// <summary>
        /// Gets a value indicating whether need go to registration process to associate external account with public user account.
        /// </summary>
        bool IsNeedRegistration
        {
            get;
        }

        /// <summary>
        /// Log out current system.
        /// </summary>
        /// <param name="context">Current http context.</param>
        void Signout(HttpContext context);
    }
}