/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: ILicensingValidationBll.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2007-2014
 *
 *  Description:
 *  Gets all basic and common data to avoid being passed by each method parameters.
 *
 *  Notes:
 * $Id: ILicensingValidationBll.cs 277004 2014-08-09 07:44:41Z ACHIEVO\james.shi $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */
namespace Accela.ACA.BLL
{
    /// <summary>
    /// Defines methods for licensing validation function.
    /// </summary>
    public interface ILicensingValidationBll
    {
        #region Methods
        /// <summary>
        /// Check User License
        /// if return 0, means expired
        /// if return -1,means the add-on is not purchased
        /// if return positive number, means the purchased license count
        /// </summary>
        /// <returns>product license</returns>
        int CheckProductLicense();

        #endregion Methods
    }
}