#region Header

/**
 * <pre>
 * 
 *  Accela Citizen Access
 *  File: CreatePublicUserArgModel.cs
 * 
 *  Accela, Inc.
 *  Copyright (C): 2014
 * 
 *  Description:
 *  login page.
 * 
 *  Notes:
 *      $Id: Login.aspx.cs 278148 2014-08-28 07:30:09Z ACHIEVO\james.shi $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion

using System;

namespace Accela.ACA.SSOInterface.Model
{
    /// <summary>
    /// Class Create Public User Argument Model
    /// </summary>
    public class UserCreationEventArgs : EventArgs
    {
        /// <summary>
        /// Gets or sets the first name.
        /// </summary>
        /// <value>The first name.</value>
        public string FirstName { get; set; }

        /// <summary>
        /// Gets or sets the last name.
        /// </summary>
        /// <value>The last name.</value>
        public string LastName { get; set; }
    }
}
