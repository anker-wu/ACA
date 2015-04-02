#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: IParcelBll.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2007-2014
 *
 *  Description:
 *
 *  Notes:
 * $Id: IParcelBll.cs 277008 2014-08-09 08:37:50Z ACHIEVO\james.shi $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 *  03/20/2008    Steven.lee    Initial version.
 * </pre>
 */

#endregion Header

using Accela.ACA.WSProxy;

namespace Accela.ACA.BLL.APO
{
    /// <summary>
    /// Defines methods for parcel function.
    /// </summary>
    public interface IParcelBll
    {       
        #region Methods

        /// <summary>
        /// Query parcel detail information
        /// </summary>
        /// <remarks> 
        /// 1. This interface supports both internal APO and external APO.
        /// 2. Call getParcelByPK method of ParcelService for getting ParcelModel model by reference parcel's PK value.
        /// </remarks>
        /// <param name="agencyCode">Agency Code.</param>
        /// <param name="parcelPK">ParcelModel with PK value.</param>
        /// <returns>a ParcelModel model</returns>
        ParcelModel GetParcelByPK(string agencyCode, ParcelModel parcelPK);

        /// <summary>
        /// Gets highest priority condition and notice conditions related with parcel.
        /// </summary>
        /// <remarks>
        /// 1. call  ParcelWebService.getParcelCondition to return.
        /// </remarks>
        /// <param name="servProvCodes">agency code</param>
        /// <param name="parcelInfo">ParcelInfoModel include addressId, addressUID, parcelNumber, parcelUID, ownerNumber, ownerUID.</param>
        /// <returns>ParcelModel with highlight and notice Conditions values.</returns>
        ParcelModel GetParcelCondition(string[] servProvCodes, ParcelInfoModel parcelInfo);

        /// <summary>
        /// Query related parcel to the specified address.
        /// </summary>
        /// <param name="agencyCode">The agency code</param>
        /// <param name="refAddressPK">Ref-AddressModel with PK value</param>
        /// <param name="queryFormat">The query format.</param>
        /// <returns>A DataTable</returns>
        /// <remarks>
        /// 1. Call getParcelListByRefAddressID method of Parcel WebServices to get an array of ParcelModel model
        /// 2. call BuildParcelDataTable method to fill parcel data into the table
        /// </remarks>
        ParcelModel[] GetParcelListByRefAddressPK(string agencyCode, RefAddressModel refAddressPK, QueryFormat queryFormat);

        /// <summary>
        /// Query related parcel information to the specified owner
        /// </summary>
        /// <param name="agencyCode">The Agency Code.</param>
        /// <param name="ownerPK">The OwnerModel with owner PK value</param>
        /// <param name="callerID">The public user id</param>
        /// <param name="queryFormat">The query format.</param>
        /// <returns>a DataTable of parcel data. This table contains four columns: Parcel Number, Lot, Block, Subdivision</returns>
        /// <remarks>
        /// 1. call getParcelListByOwnerNBR method of ParcelWebService
        /// to return a list of ParcelModel model.
        /// 2. call BuildParcelDataTable() method to fill parcel information into a DataTable
        /// 3. return the converted DataTable.
        /// </remarks>
        ParcelModel[] GetParcelListByOwnerPK(string agencyCode, OwnerModel ownerPK, string callerID, QueryFormat queryFormat);

        /// <summary>
        /// Get TrustAccount Parcel list by Trust account number
        /// </summary>
        /// <param name="accountNbr">trust account number.</param>
        /// <returns>ParcelModel array</returns>
        ParcelInfoModel[] GetParcelListByTrustAccount(string accountNbr);

        /// <summary>
        /// Gets Parcel Genealogy.
        /// </summary>
        /// <param name="agencyCode">agency code</param>
        /// <param name="parcelNumber">the parcel Number.</param>
        /// <returns>GenealogyTransactionModel model list.</returns>
        GenealogyTransactionModel[] GetParcelGenealogy(string agencyCode, string parcelNumber);

        #endregion Methods
    }
}
