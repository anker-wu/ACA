#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: IRefAddressBll.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2008-2014
 *
 *  Description:
 *
 *  Notes:
 * $Id: IAddressBll.cs 276976 2014-08-08 10:10:01Z ACHIEVO\james.shi $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 *  03/20/2008    Sawyer.Liu    Initial version.
 * </pre>
 */

#endregion Header

using System.Data;

using Accela.ACA.WSProxy;

namespace Accela.ACA.BLL.APO
{
    /// <summary>
    /// Defines many methods for address's logic.
    /// </summary>
    public interface IAddressBll
    {
        #region Methods

        /// <summary>
        /// Generate Address string from a specified AddressModel model.
        /// </summary>
        /// <param name="addressModel">a AddressModel model.</param>
        /// <returns>an assembled address string</returns>
        string GenerateAddressString(AddressModel addressModel);

        /// <summary>
        /// Gets Address info by the address info.
        /// </summary>
        /// <param name="capIdModel4ws">the cap id model</param>
        /// <param name="refAddressModel">the ref-address model contain search criteria</param>
        /// <param name="format">the query format</param>
        /// <returns>daily address list</returns>
        DataTable GetDailyAddressesByRefAddressModel(CapIDModel4WS capIdModel4ws, RefAddressModel refAddressModel, QueryFormat format);

        #endregion Methods
    }
}