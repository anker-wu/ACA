#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: IAPOBll.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2007-2014
 *
 *  Description:
 *
 *  Notes:
 * $Id: IAPOBll.cs 278175 2014-08-28 09:03:13Z ACHIEVO\james.shi $.
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
    /// Defines methods for APO BLL's logic.
    /// </summary>
    public interface IAPOBll
    {
        #region Methods

        /// <summary>Get APO data by address</summary>
        /// <remarks> 
        /// 1. Construct a RefAddressModel model and call SearchAPO to get a list of ParcelInfoModel.
        /// 2. call BuildAPODataTable() method to fill APO information into a data table
        /// 3. return the converted data table.
        /// </remarks>
        /// <param name="serviceProviderCode"> The agency code </param>
        /// <param name="addressModel"> The RefAddressModel model</param>
        /// <param name="queryFormat"> The Query Format</param>
        /// <param name="isForceValid"> Is Force Valid</param>
        /// <returns>A data table</returns>
        SearchResultModel GetAPOListByAddress(string serviceProviderCode, RefAddressModel addressModel, QueryFormat queryFormat, bool isForceValid);

        /// <summary>
        /// Query APO information by a specified cap number
        /// </summary>
        /// <remarks> 
        /// 1. Call getAPOListByCap method of ParcelWebService
        ///    to return a array of ParcelInfoModel model.
        /// 2. Call BuildAPODataTable() method to fill APO information into a data table
        /// 3. Return the converted data table.
        /// </remarks>
        /// <param name="serviceProviderCode">The agency code</param>
        /// <param name="capModel"> The CapModel4WS model. Only cap id field is needed in this method
        /// </param>
        /// <param name="callerID"> The public user id</param>
        /// <param name="queryFormat"> Query Format</param>
        /// <returns>An array of ParcelInfoModel </returns>
        ParcelInfoModel[] GetAPOListByCap(string serviceProviderCode, CapModel4WS capModel, string callerID, QueryFormat queryFormat);

        /// <summary>Get APO data by parcel owner</summary>
        /// <remarks> 
        /// 1. Construct a OwnerModel model with a set of owner search criteria(in propertyLookUp page) or only owner sequence number and owner number(in detail detail page).
        /// 2. call SearchAPO to get a list of ParcelInfoModel.
        /// 3. call BuildAPODataTable() method to fill APO information into a data table
        /// 4. return the converted data table.
        /// </remarks>
        /// <param name="serviceProviderCode"> The agency code </param>
        /// <param name="owner"> The OwnerModel model</param>
        /// <param name="queryFormat"> The Query Format</param>
        /// <param name="isForceValid"> Is Force Valid</param>
        /// <returns>A data table</returns>
        SearchResultModel GetAPOListByOwner(string serviceProviderCode, OwnerModel owner, QueryFormat queryFormat, bool isForceValid);

        /// <summary>Get APO data by parcel</summary>
        /// <remarks> 
        /// 1. Construct a ParcelModel model with a set of parcel search criteria(in propertyLookUp page) or only parcel sequence number and parcel number(in parcel detail page).
        /// 2. call SearchAPO to get a list of ParcelInfoModel.
        /// 3. call BuildAPODataTable() method to fill APO information into a data table
        /// 4. return the converted data table.
        /// </remarks>
        /// <param name="serviceProviderCode"> The agency code </param>
        /// <param name="parcel"> The ParcelModel model</param>
        /// <param name="queryFormat"> The Query Format</param>
        /// <param name="isForceValid"> Is Force Valid</param>
        /// <returns>A data table</returns>
        SearchResultModel GetAPOListByParcel(string serviceProviderCode, ParcelModel parcel, QueryFormat queryFormat, bool isForceValid);

        /// <summary>Get APO data by owner key value; this method support both internal APO and XAPO</summary>
        /// <remarks> 
        /// 1. Encapsulate Owner key value (include sequence number and owner number or owner UID) into OwnerModel as parameter(in detail page).
        /// 2. Call SearchAPO to get a list of ParcelInfoModel.
        /// 3. Call BuildAPODataTable() method to fill APO information into a DataTable
        /// 4. Return the converted DataTable.
        /// </remarks>
        /// <param name="agencyCode"> The agency code </param>
        /// <param name="ownerPK"> OwnerModel with PK value.</param>
        /// <param name="queryFormat"> The query format </param>
        /// <returns>A DataTable</returns>
        SearchResultModel GetAddressListByOwnerPK(string agencyCode, OwnerModel ownerPK, QueryFormat queryFormat);

        /// <summary>Get APO data by parcel key value; this method support both internal APO and XAPO</summary>
        /// <remarks> 
        /// 1. Encapsulate Parcel key value (include sequence number and parcel number or parcel UID) into ParcelModel as parameter (in parcel detail page).
        /// 2. Call SearchAPO to get a list of ParcelInfoModel.
        /// 3. Call BuildAPODataTable() method to fill APO information into a DataTable
        /// 4. Return the converted DataTable.
        /// </remarks>
        /// <param name="agencyCode"> The agency code </param>
        /// <param name="parcelPK"> Parcel model with PK value</param>
        /// <param name="isForGenealogy"> Is For Genealogy</param>
        /// <param name="queryFormat"> Query format</param>
        /// <returns>A DataTable</returns>
        SearchResultModel GetAddressListByParcelPK(string agencyCode, ParcelModel parcelPK, bool isForGenealogy, QueryFormat queryFormat);

        /// <summary>
        /// get APO information with licensee.
        /// </summary>
        /// <param name="license">licensee model</param>
        /// <param name="queryFormat">query format model</param>
        /// <returns>a data table for APO</returns>
        ParcelInfoModel[] GetAPOListByLP(LicenseModel4WS license, QueryFormat queryFormat);

        /// <summary>Get APO data by address</summary>
        /// <remarks> 
        /// 1. Construct a RefAddressModel4WS model and call SearchAPO to get a list of ParcelInfoModel4WS.
        /// 2. call BuildAPODataTable() method to fill APO information into a data table
        /// 3. return the converted data table.
        /// </remarks>
        /// <param name="serviceProviderCode"> The agency code </param>
        /// <param name="addressModel"> The RefAddressModel4WS model</param>
        /// <param name="queryFormat"> The queryFormat</param>
        /// <param name="isForceValid">spear form not search disabled parcel.</param>
        /// <returns>return ParcelInfoModel array</returns>
        SearchResultModel GetParcelInfoByAddress(string serviceProviderCode, RefAddressModel addressModel, QueryFormat queryFormat, bool isForceValid);

        /// <summary>Get APO data by parcel</summary>
        /// <remarks> 
        /// 1. Construct a ParcelModel4WS model with a set of parcel search criteria(in propertyLookUp page) or only parcel sequence number and parcel number(in parcel detail page).
        /// 2. call SearchAPO to get a list of ParcelInfoModel4WS.
        /// 3. call BuildAPODataTable() method to fill APO information into a data table
        /// 4. return the converted data table.
        /// </remarks>
        /// <param name="serviceProviderCode"> The agency code </param>
        /// <param name="parcel"> The ParcelModel4WS model</param>
        /// <param name="queryFormat"> Query format</param>
        /// <param name="isForceValid"> Is Force Valid</param>
        /// <returns>A ParcelInfoModel4WS array</returns>
        SearchResultModel GetParcelInfoByParcel(string serviceProviderCode, ParcelModel parcel, QueryFormat queryFormat, bool isForceValid);

        /// <summary>
        /// Get the Ref Address List.
        /// </summary>
        /// <param name="serviceProviderCode">Agency code</param>
        /// <param name="addressModel">Address Model</param>
        /// <param name="queryFormat">Query Format of address</param>
        /// <returns>RefAddressModel List</returns>
        SearchResultModel GetRefAddressList(string serviceProviderCode, RefAddressModel addressModel, QueryFormat queryFormat);

        /// <summary>
        /// Gets parcel associated address records.
        /// </summary>
        /// <param name="serviceProviderCode">Agency code</param>
        /// <param name="parcelPKModel">Parcel PK Model</param>
        /// <param name="filterDisableOwner">Indicate whether filter disabled owners.</param>
        /// <returns>Associated Address List</returns>
        RefAddressModel[] GetRefAddressListByParcelPK(string serviceProviderCode, ParcelModel parcelPKModel, bool filterDisableOwner);

        /// <summary>
        /// Gets parcel associated address records via send back from gis map.
        /// </summary>
        /// <param name="serviceProviderCode">Agency code</param>
        /// <param name="parcelPKModel">Parcel PK Model</param>
        /// <param name="queryFormat">Query Format of parcel</param>
        /// <returns>Associated Address List</returns>
        SearchResultModel GetRefAddressListByParcel(string serviceProviderCode, ParcelModel parcelPKModel, QueryFormat queryFormat);

        /// <summary>
        /// Gets ref parcel list.
        /// </summary>
        /// <param name="serviceProviderCode">Agency code</param>
        /// <param name="parcelModel">Parcel model</param>
        /// <param name="queryFormat">Query format</param>
        /// <returns>Ref parcel list</returns>
        SearchResultModel GetRefParcelList(string serviceProviderCode, ParcelModel parcelModel, QueryFormat queryFormat);

        /// <summary>
        /// Owner Section in Parcel Detail
        /// Query related owner information to a specified parcel.
        /// </summary>
        /// 1. call GetOwnerListByParcelPKs method of OwnerService
        ///    to return an array of OwnerModel.
        /// 2. call BuildOwnerDataTable() method to fill APO information into a DataTable
        /// 3. return the converted DataTableDataTable.
        /// <param name="agencyCode">Agency Code.</param>
        /// <param name="parcelPKArray"> The ParcelModel Array with parcel PK value. 
        /// We can pass multiple parcel numbers to get multiple owners or just a single parcel number.</param>
        /// <param name="filterDisableOwner">Indicate whether filter disabled owners.</param>
        /// <param name="queryFormat">Owner list query format.</param>
        /// <returns> a DataTable of owner information. This DataTable consists of three columns:Name, relationship and address </returns>
        SearchResultModel GetOwnerListByParcelPKs(string agencyCode, ParcelModel[] parcelPKArray, bool filterDisableOwner, QueryFormat queryFormat);

        /// <summary>
        /// Get owner list by a specified parcel.
        /// </summary>
        /// <param name="agencyCode">Agency Code.</param>
        /// <param name="ownerModel">An OwnerCompModel</param>
        /// <param name="queryFormat">Owner list query format.</param>
        /// <returns>A SearchResultModel</returns>
        SearchResultModel GetRefOwnerList(string agencyCode, OwnerCompModel ownerModel, QueryFormat queryFormat);

        #endregion Methods
    }
}
