#region Header

/**
* <pre>
*  Accela Citizen Access
*  File: PublicUserModel4WS.cs
*
*  Accela, Inc.
*  Copyright (C): 2014
*
*  Description: Extend the PublicUserModel4WS.
*
* </pre>
*/

#endregion

namespace Accela.ACA.WSProxy
{
    /// <summary>
    /// Extend the PublicUserModel4WS
    /// </summary>
    public partial class PublicUserModel4WS
    {
        /// <summary>
        /// Gets or sets the SSO type.
        /// </summary>
        public string SSOType
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the SSO user name.
        /// </summary>
        public string SSOUserName
        {
            get;
            set;
        }
    }
}
