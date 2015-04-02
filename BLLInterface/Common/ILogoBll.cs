#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: ILogoBll.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2007-2014
 *
 *  Description:
 *
 *
 *  Notes:
 * $Id: ILogoBll.cs 277004 2014-08-09 07:44:41Z ACHIEVO\james.shi $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using Accela.ACA.WSProxy;

namespace Accela.ACA.BLL.Common
{
    /// <summary>
    /// Defines methods for Logo BLL.
    /// </summary>
    public interface ILogoBll
    {
        #region Methods

        /// <summary>
        /// Get logo of specified agency from cache
        /// </summary>
        /// <param name="agencyCode">the super agency code</param>
        /// <returns>a logo object instance</returns>
        LogoModel GetAgencyLogo(string agencyCode);

        /// <summary>
        /// Gets agency logo model by types
        /// </summary>
        /// <param name="agencyCode">The agency code.</param>
        /// <param name="logoType">Type of the logo.</param>
        /// <returns>agency logo model</returns>
        LogoModel GetAgencyLogoByType(string agencyCode, string logoType);

        #endregion Methods
    }
}