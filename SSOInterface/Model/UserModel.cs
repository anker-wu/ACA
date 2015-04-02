#region Header

/**
* <pre>
*  Accela Citizen Access
*  File: UserModel.cs
*
*  Accela, Inc.
*  Copyright (C): 2014
*
*  Description: The user model.
*
* </pre>
*/

#endregion

using System;

namespace Accela.ACA.SSOInterface.Model
{
    /// <summary>
    /// User model.
    /// </summary>
    [Serializable]
    public class UserModel
    {
        /// <summary>
        /// Gets or sets the user unique identifier.
        /// </summary>
        /// <value>The user unique identifier.</value>
        public string UserId
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the email.
        /// </summary>
        /// <value>The email.</value>
        public string Email
        {
            get; 
            set;
        }

        /// <summary>
        /// Gets or sets the people model.
        /// </summary>
        /// <value>The people model.</value>
        public UserContactModel[] PeopleModel
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the user sequence number.
        /// </summary>
        /// <value>The user sequence number.</value>
        public string UserSeqNum
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the type of the account.
        /// </summary>
        /// <value>The type of the account.</value>
        public string AccountType { get; set; }

        /// <summary>
        /// Gets or sets the type of the SSO.
        /// </summary>
        /// <value>The type of the SSO.</value>
        public string SSOType
        {
            get; 
            set;
        }

        /// <summary>
        /// Gets or sets the name of the SSO user.
        /// </summary>
        /// <value>The name of the SSO user.</value>
        public string SSOUserName
        {
            get; 
            set;
        }
    }
}
