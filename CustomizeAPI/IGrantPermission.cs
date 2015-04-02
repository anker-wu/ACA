#region Header

/**
 *  Accela Citizen Access
 *  File: IGrantPermission.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2011
 *
 *  Description:
 *   The interface is need the customer implemented that grant the customize permission.
 *
 *  Notes:
 * $Id: IGrantPermission.cs 192687 2011-03-14 05:38:13Z ACHIEVO\daly.zeng $.
 *  Revision History
 *  Date,                  Who,                 What
 */

#endregion Header

namespace Accela.ACA.CustomizeAPI
{
    /// <summary>
    /// The interface that grant the customize permission.
    /// </summary>
    public interface IGrantPermission
    {
        /// <summary>
        /// Grants the permission.
        /// </summary>
        void GrantPermission();
    }
}
