#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: IAdminConfigurationSave.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2007-2014
 *
 *  Description:
 *  Interface define for admin.
 *
 *  Notes:
 * $Id: IAdminConfigurationSave.cs 276976 2014-08-08 10:10:01Z ACHIEVO\james.shi $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using Accela.ACA.WSProxy;

namespace Accela.ACA.BLL.Admin
{
    /// <summary>
    /// Interface define for admin.
    /// </summary>
    public interface IAdminConfigurationSave
    {
        #region Methods
        /// <summary>
        /// Save admin configuration data
        /// </summary>
        /// <param name="configurationModel">admin configuration information</param>
        /// <returns>is saved successful</returns>
        bool SaveAdminConfigurationData(AdminConfigurationModel4WS configurationModel);

        #endregion Methods
    }
}