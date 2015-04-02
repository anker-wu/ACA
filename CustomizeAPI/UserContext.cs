#region Header

/**
 *  Accela Citizen Access
 *  File: UserContext.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2011-2014
 *
 *  Description:
 *   It provides the user context.
 *
 *  Notes:
 * $Id: UserContext.cs 192687 2011-03-14 05:38:13Z ACHIEVO\daly.zeng $.
 *  Revision History
 *  Date,                  Who,                 What
 */

#endregion Header

using System.Collections.Generic;

namespace Accela.ACA.CustomizeAPI
{
    /// <summary>
    /// It provides the user context.
    /// </summary>
    [System.SerializableAttribute]
    public class UserContext
    {
        /// <summary>
        /// Gets or sets the login name.
        /// </summary>
        public string LoginName
        {
            get;

            set;
        }

        /// <summary>
        /// Gets or sets the first name.
        /// </summary>
        public string FirstName
        {
            get;

            set;
        }

        /// <summary>
        /// Gets or sets the name of the middle.
        /// </summary>
        public string MiddleName
        {
            get;

            set;
        }
        
        /// <summary>
        /// Gets or sets the last name.
        /// </summary>
        public string LastName
        {
            get;

            set;
        }

        /// <summary>
        /// Gets or sets the caller ID.
        /// </summary>
        public string CallerID
        {
            get;

            set;
        }

        /// <summary>
        /// Gets or sets the user token.
        /// </summary>
        public string UserToken
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the permission that customer granted.
        /// </summary>
        public IDictionary<FunctionItem, bool> Permissions
        {
            get;

            set;
        }
    }
}
