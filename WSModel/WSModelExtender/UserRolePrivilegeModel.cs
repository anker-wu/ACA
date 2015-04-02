#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: UserRolePrivilegeModel.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2013
 *
 *  Description:
 *
 *  Notes:
 *      $Id: UserRolePrivilegeModel.cs 238264 2013-05-22 08:31:18Z ACHIEVO\jone.lu $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using System;

namespace Accela.ACA.WSProxy
{
    /// <summary>
    /// UserRolePrivilegeModel Extender
    /// </summary>
    public partial class UserRolePrivilegeModel
    {
        /// <summary>
        /// Gets or sets a value indicating if agent and agency clerk user role are in register users group.
        /// as agent and agent clerk do not belong to registered users in some special settings currently,
        /// e.g. Admin->module settings->"Record Detail Section Configuration" and "Report Display Controls",
        /// so we add the property to distinct such situations.
        /// if the value is true, means both Agent and Agent Clerk role belong to register users,
        /// otherwise, they do not belong to register users.
        /// </summary>
        public bool AgentOrClerkNotInRegisteredUsers
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets a value indicating the agent is allow or not.
        /// </summary>
        public bool AgentAllowed
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets a value indicating the agent clerk is allow or not.
        /// </summary>
        public bool AgentClerkAllowed
        {
            get;
            set;
        }
    }
}
