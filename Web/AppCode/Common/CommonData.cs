#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: CommonData.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2007-2014
 *
 *  Description:
 *  Gets all basic and common data to avoid being passed by each method parameters.
 *
 *  Notes:
 * $Id: CommonData.cs 277071 2014-08-11 03:38:26Z ACHIEVO\james.shi $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using Accela.ACA.BLL;
using Accela.ACA.BLL.Account;
using Accela.ACA.Common.Util;
using Accela.ACA.Web.Common;

namespace Accela.ACA.Web
{
    /// <summary>
    /// Provides the common data implementation.
    /// Gets all basic and common data to avoid being passed by each method parameters.
    /// </summary>
    public class CommonData : ICommonData
    {
        #region Properties

        /// <summary>
        /// Gets the current agency code.
        /// </summary>
        public string AgencyCode
        {
            get
            {
                return ConfigManager.AgencyCode;
            }
        }

        /// <summary>
        /// Gets the current agency code.
        /// </summary>
        public string SuperAgencyCode
        {
            get
            {
                return ConfigManager.SuperAgencyCode;
            }
        }

        /// <summary>
        /// Gets the current user is accessing system.
        /// </summary>
        public User CurrentUser
        {
            get
            {
                return AppSession.User;
            }
        }

        /// <summary>
        /// Gets a value indicating whether the current user is accessing system.
        /// </summary>
        public bool IsAdmin
        {
            get
            {
                return AppSession.IsAdmin;
            }
        }

        /// <summary>
        /// Gets a value indicating whether current agency is super agency
        /// </summary>
        public bool IsSuperAgency
        {
            get
            {
                return StandardChoiceUtil.IsSuperAgency();
            }
        }

        /// <summary>
        /// Gets the current thread culture language code. e.g.: <c>en-Us,fr,de</c>
        /// </summary>
        public string Language
        {
            get
            {
                return I18nCultureUtil.UserPreferredCulture;
            }
        }

        #endregion Properties
    }
}