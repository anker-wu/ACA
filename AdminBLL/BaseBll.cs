#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: BaseBll.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2008-2014
 *
 *  Description:
 *
 *  Notes:
 * $Id: BaseBll.cs 277080 2014-08-11 06:28:07Z ACHIEVO\james.shi $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using Accela.ACA.BLL;
using Accela.ACA.BLL.Account;
using Accela.ACA.Common;

namespace Accela.ACA.AdminBLL
{
    /// <summary>
    /// An base class for all of business classes.
    /// </summary>
    public abstract class BaseBll
    {
        #region Properties

        /// <summary>
        /// Gets the current agency code.
        /// </summary>
        protected string AgencyCode
        {
            get
            {
                return CommonData.AgencyCode;
            }
        }

        /// <summary>
        /// Gets the current thread culture language code. e.g: en-Us,FR,de
        /// </summary>
        protected string Language
        {
            get
            {
                return CommonData.Language;
            }
        }

        /// <summary>
        /// Gets the current user is accessing system.
        /// </summary>
        protected User User
        {
            get
            {
                return CommonData.CurrentUser;
            }
        }

        /// <summary>
        /// Gets a value indicating whether current agency is super agency
        /// </summary>
        protected bool IsSuperAgency
        {
            get
            {
                return CommonData.IsSuperAgency;
            }
        }

        /// <summary>
        /// Gets the ICommonData instance.
        /// </summary>
        private ICommonData CommonData
        {
            get
            {
                return ObjectFactory.GetObject(typeof(ICommonData)) as ICommonData;
            }
        }

        #endregion Properties
    }
}