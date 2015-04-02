#region Header

/**
* <pre>
*  Accela Citizen Access
*  File: ValidateLoginResult.cs
*
*  Accela, Inc.
*  Copyright (C): 2014
*
*  Description: The security token representation.
*
* </pre>
*/

#endregion

namespace Accela.ACA.RealMeAccessGate
{
    /// <summary>
    /// Validate login result.
    /// </summary>
    public class ValidateLoginResult
    {
        /// <summary>
        /// Gets or sets the state.
        /// </summary>
        public string State { get; set; }

        /// <summary>
        /// Gets or sets the status code.
        /// </summary>
        public string StatusCode { get; set; }

        /// <summary>
        /// Gets or sets the user name.
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// Gets or sets the result message.
        /// </summary>
        public string ResultMessage { get; set; }

        /// <summary>
        /// Gets or sets the security token.
        /// </summary>
        public string SecurityToken { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether is associated with an user.
        /// </summary>
        public bool IsAssociatedWithUser { get; set; }
    }
}
