#region Header

/**
* <pre>
*  Accela Citizen Access
*  File: IAuthAdapter.cs
*
*  Accela, Inc.
*  Copyright (C): 2012-2014
*
*  Description: The interface defined the common properties and motheds for authentication adapter.
*
* </pre>
*/

#endregion

using Accela.ACA.SSOInterface;
using Accela.ACA.WSProxy;

namespace Accela.ACA.BLL.Account
{
    /// <summary>
    /// The interface defined the common properties and methods for authentication adapter.
    /// </summary>
    public interface IAuthAdapter : IAuthAdapterBase
    {
        /// <summary>
        /// Validate user's credentials and create mapping user in Accela system.
        /// </summary>
        /// <param name="username">User identity.</param>
        /// <param name="password">User password.</param>
        /// <returns>public user information or error message</returns>
        PublicUserModel4WS ValidateUser(string username, string password);
    }
}