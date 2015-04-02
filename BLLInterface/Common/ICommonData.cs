#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: ICommonData.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2007-2014
 *
 *  Description:
 *  Gets all basic and common data to avoid being passed by each method parameters.
 *
 *  Notes:
 * $Id: ICommonData.cs 277004 2014-08-09 07:44:41Z ACHIEVO\james.shi $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using Accela.ACA.BLL.Account;

namespace Accela.ACA.BLL
{
    /// <summary>
    /// Gets all basic and common data to avoid being passed by each method parameters.
    /// </summary>
    public interface ICommonData
    {
        #region Properties

        /// <summary>
        /// Gets the current agency code.
        /// </summary>
        string AgencyCode
        {
            get;
        }

        /// <summary>
        /// Gets the current agency code.
        /// </summary>
        string SuperAgencyCode
        {
            get;
        }

        /// <summary>
        /// Gets the current user is accessing system.
        /// </summary>
        User CurrentUser
        {
            get;
        }

        /// <summary>
        /// Gets a value indicating whether current agency is super agency
        /// </summary>
        bool IsSuperAgency
        {
            get;
        }

        /// <summary>
        /// Gets a value indicating whether in ACA Admin.
        /// </summary>
        bool IsAdmin
        {
            get;
        }

        /// <summary>
        /// Gets the current thread culture language code. e.g.: <c>en-Us,fr,de</c>
        /// </summary>
        string Language
        {
            get;
        }

        #endregion Properties
    }
}