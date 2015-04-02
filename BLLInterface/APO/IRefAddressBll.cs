#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: IRefAddressBll.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2007-2014
 *
 *  Description:
 *
 *  Notes:
 * $Id: IRefAddressBll.cs 277008 2014-08-09 08:37:50Z ACHIEVO\james.shi $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 *  03/20/2008    Steven.lee    Initial version.
 * </pre>
 */

#endregion Header

using System.Collections.Generic;
using System.Data;

using Accela.ACA.WSProxy;

namespace Accela.ACA.BLL.APO
{
    /// <summary>
    /// Notice:the model name "RefAddressModel" have been given a wrong name by AA code,
    /// it often confuses us,
    /// Indeed the RefAddressModel stand for AddressModel but not reference address.
    /// </summary>
    public interface IRefAddressBll
    {
        #region Methods

        /// <summary>
        /// Construct a new DataTable for Address.
        /// </summary>
        /// <returns>A DataTable that contains 2 columns, AddressId and assembled address string.</returns>
        DataTable ConstructAddressDataTable();

        /// <summary>
        /// Query address detail information.
        /// </summary>
        /// <remarks> 
        /// 1. This interface supports both internal APO and external APO.
        /// 2. Call getRefAddressByPK method of RefAddressService for getting RefAddressModel model by reference address's PK value.
        /// </remarks>
        /// <param name="agencyCode">Agency Code.</param>
        /// <param name="refAddressPK">RefAddressModel with PK value.</param>
        /// <returns>RefAddressModel information</returns>
        RefAddressModel GetAddressByPK(string agencyCode, RefAddressModel refAddressPK);

        /// <summary>
        /// Gets highest priority condition and notice conditions related with address.
        /// </summary>
        /// <remarks>
        /// 1. call RefAddressService.getAddressCondition to return.
        /// </remarks>
        /// <param name="servProvCodes">agency code list</param>
        /// <param name="parcelInfo">ParcelInfoModel include addressId, addressUID, parcelNumber, parcelUID, ownerNumber, ownerUID.</param>
        /// <returns>An RefAddressModel object includes highlight and noticeConditions.</returns>
        RefAddressModel GetAddressCondition(string[] servProvCodes, ParcelInfoModel parcelInfo);

        /// <summary>
        /// get Address Condition
        /// </summary>
        /// <param name="serviceProviderCodes">service provider codes</param>
        /// <param name="addressID">address ID.</param>
        /// <returns>address condition</returns>
        RefAddressModel GetAddressCondition(string[] serviceProviderCodes, long addressID);

        /// <summary>
        /// get address info parcel number by address model.
        /// </summary>
        /// <param name="serviceProviderCode">service Provider Code</param>
        /// <param name="refAddressModel">refAddressModel entity</param>
        /// <param name="isUnique">whether unique parcel number.</param>
        /// <returns>address parcel NBR</returns>
        RefAddressModel[] GetAddressParcelNbrByAddressModel(string serviceProviderCode, RefAddressModel refAddressModel, bool isUnique);

        /// <summary>
        /// Gets the according permit address Array by CAP ID Array.
        /// One CAP Id should return one address accordingly, 
        /// if there is no address according to CAP Id,the value should be null((addresses[i] = null)).
        /// </summary>
        /// <param name="capIDs">CapIDModel4WS Array</param>
        /// <returns>daily address list.</returns>
        RefAddressModel[] GetAddressesByCapIDs(CapIDModel4WS[] capIDs);

        /// <summary>
        /// Get RefAddress List of Look Up Work Location Result.
        /// </summary>
        /// <param name="serviceProviderCode">Service Provider Code</param>
        /// <param name="refAddressModel">RefAddress Model for Web Service</param>
        /// <param name="parcelModel">Parcel Model for Web Service</param>
        /// <param name="format">the query format</param>
        /// <returns>Array of reference address information</returns>
        SearchResultModel GetRefAddressByRefAddressModelParcelModel(string serviceProviderCode, RefAddressModel refAddressModel, ParcelModel parcelModel, QueryFormat format);

        /// <summary>
        /// Construct a new DataTable for Address.
        /// </summary>
        /// <param name="addressArray">RefAddress Model Array.</param>
        /// <returns>A DataTable that contains only one column:full address.</returns>
        DataTable BuildAddressTable(RefAddressModel[] addressArray);

        /// <summary>
        /// Get Trust account address list by account number
        /// </summary>
        /// <param name="acctNumber">account number</param>
        /// <returns>Array of RefAddressModel for web service</returns>
        RefAddressModel[] GetAddressListByTrustAccount(string acctNumber);

        /// <summary>
        /// Get the address source sequence number by reference address id. This method only supports super agency logic,
        /// because reference APO id has unique index and different children agencies have different source sequence numbers.
        /// If you want to get source sequence number for normal agency, you can invoke the method getServiceProviderByPK()
        /// of ServiceProviderWebService by current service provider code.
        /// </summary>
        /// <param name="refAddressID">Reference Address Number</param>
        /// <returns>string value of source sequence number</returns>
        string GetAddressSourceSeqNbrByRefAddressId(string refAddressID);

        /// <summary>
        /// get contact address types by contact type.
        /// </summary>
        /// <param name="serviceProviderCode">agency code</param>
        /// <param name="contactType">contact type</param>
        /// <returns>contact address types</returns>
        XRefContactAddressTypeModel[] GetContactAddressTypeByContactType(string serviceProviderCode, string contactType);

        /// <summary>
        /// get contact address validation setting.
        /// </summary>
        /// <param name="serviceProviderCode">service provider code</param>
        /// <param name="contactAddressType">contact address type</param>
        /// <returns>contact address type setting information</returns>
        XContactAddressTypeSettingModel GetContactAddressValidationSetting(string serviceProviderCode, string contactAddressType);

        /// <summary>
        /// Validate contact address from external.
        /// </summary>
        /// <param name="serviceProviderCode">service provider code</param>
        /// <param name="contactAddress">contact address</param>
        /// <returns>validated contact address list</returns>
        List<ContactAddressModel> GetContactAddressListFromExternal(string serviceProviderCode, ContactAddressModel contactAddress);

        #endregion Methods
    }
}
