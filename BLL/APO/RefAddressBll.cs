#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: RefAddressBll.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2008-2014
 *
 *  Description:
 *
 *  Notes:
 *      $Id: RefAddressBll.cs 277178 2014-08-12 08:11:48Z ACHIEVO\james.shi $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 *  05/18/2007    Dylan.Liang    Initial.
 * </pre>
 */

#endregion Header

using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

using Accela.ACA.Common;
using Accela.ACA.Common.Log;
using Accela.ACA.Common.Util;
using Accela.ACA.WSProxy;
using log4net;

namespace Accela.ACA.BLL.APO
{
    /// <summary>
    /// This class provide the ability to operation refAddress.
    /// </summary>
    public class RefAddressBll : BaseBll, IRefAddressBll
    {
        #region Fields

        /// <summary>
        /// Logger object.
        /// </summary>
        private static readonly ILog Logger = LogFactory.Instance.GetLogger(typeof(RefAddressBll));

        #endregion Fields

        #region Properties

        /// <summary>
        /// Gets an instance of RefAddressService.
        /// </summary>
        private RefAddressWebServiceService RefAddressService
        {
            get
            {
                return WSFactory.Instance.GetWebService<RefAddressWebServiceService>();
            }
        }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Construct a new DataTable for Address.
        /// </summary>
        /// <param name="addressArray">RefAddress Model Array.</param>
        /// <returns>A DataTable that contains only one column:full address.</returns>
        DataTable IRefAddressBll.BuildAddressTable(RefAddressModel[] addressArray)
        {
            DataTable table = new DataTable();

            table.Columns.Add("FullAddress");

            if (addressArray == null || addressArray.Length < 1)
            {
                return table;
            }

            foreach (RefAddressModel model in addressArray)
            {
                DataRow dr = table.NewRow();

                AddressModel addressModel = ConvertRefAddressModel2AddressModel(model);

                IAddressBll addressBll = (IAddressBll)ObjectFactory.GetObject(typeof(IAddressBll));
                dr["FullAddress"] = addressBll.GenerateAddressString(addressModel);

                table.Rows.Add(dr);
            }

            return table;
        }

        /// <summary>
        /// Construct a new DataTable for Address.
        /// </summary>
        /// <returns>A DataTable that contains 2 columns, AddressId and assembled address string.</returns>
        public DataTable ConstructAddressDataTable()
        {
            DataTable addressTable = new DataTable();

            addressTable.Columns.Add("AddressId");
            addressTable.Columns.Add("Address");

            return addressTable;
        }

        /// <summary>
        /// Query address detail information.
        /// </summary>
        /// <param name="agencyCode">Agency Code.</param>
        /// <param name="refAddressPK">RefAddressModel with PK value.</param>
        /// <returns>RefAddressModel information</returns>
        /// <exception cref="ACAException">Exception from web service.</exception>
        /// <remarks>1. This interface supports both internal APO and external APO.
        /// 2. Call getRefAddressByPK method of RefAddressService for getting RefAddressModel model by reference address's PK value.</remarks>
        RefAddressModel IRefAddressBll.GetAddressByPK(string agencyCode, RefAddressModel refAddressPK)
        {
            if (Logger.IsDebugEnabled)
            {
                Logger.Debug("Begin IRefAddressBll.GetAddressByPK()");
            }

            try
            {
                RefAddressModel refAddressModel = RefAddressService.getRefAddressByPK(agencyCode, refAddressPK);

                if (Logger.IsDebugEnabled)
                {
                    Logger.Debug("End IRefAddressBll.GetAddressByPK()");
                }

                return refAddressModel;
            }
            catch (Exception e)
            {
                throw new ACAException(e);
            }
        }

        /// <summary>
        /// Gets highest priority condition and notice conditions related with address.
        /// </summary>
        /// <param name="servProvCodes">agency code list</param>
        /// <param name="parcelInfo">ParcelInfoModel include addressId, addressUID, parcelNumber, parcelUID, ownerNumber, ownerUID.</param>
        /// <returns>An RefAddressModel object includes highlight and noticeConditions.</returns>
        /// <exception cref="DataValidateException">{ <c>servProvCodes, parcelInfo, parcelInfo.raddressModel</c> } is null</exception>
        /// <exception cref="ACAException">Exception from web service.</exception>
        /// <remarks>1. call RefAddressService.getAddressCondition to return.</remarks>
        RefAddressModel IRefAddressBll.GetAddressCondition(string[] servProvCodes, ParcelInfoModel parcelInfo)
        {
            if (Logger.IsDebugEnabled)
            {
                Logger.Debug("Begin RefAddressBll.GetAddressCondition()");
            }
             
            if (servProvCodes == null || servProvCodes.Length == 0 || parcelInfo == null || parcelInfo.RAddressModel == null)
            {
                throw new DataValidateException(new string[] { "servProvCodes", "parcelInfo", "parcelInfo.raddressModel" });
            }

            try
            {
                RefAddressModel addressConditionModel = RefAddressService.getAddressCondition(servProvCodes, parcelInfo, User.PublicUserId);
               
                if (Logger.IsDebugEnabled)
                {
                    Logger.Debug("End RefAddressBll.GetAddressCondition()");
                }

                return addressConditionModel;
            }
            catch (Exception e)
            {
                throw new ACAException(e);
            }
        }

        /// <summary>
        /// get Address Condition
        /// </summary>
        /// <param name="serviceProviderCodes">service provider codes</param>
        /// <param name="addressID">address ID.</param>
        /// <returns>address condition</returns>
        /// <exception cref="ACAException">Exception from web service.</exception>
        public RefAddressModel GetAddressCondition(string[] serviceProviderCodes, long addressID)
        {
            if (Logger.IsDebugEnabled)
            {
                Logger.Debug("Begin RefAddressBll.GetAddressCondition()");
            }

            try
            {
                RefAddressModel addressConditionModel = RefAddressService.getAddressCondition4SuperAgency(serviceProviderCodes, addressID, User.UserID);

                if (Logger.IsDebugEnabled)
                {
                    Logger.Debug("End RefAddressBll.GetAddressCondition()");
                }

                return addressConditionModel;
            }
            catch (Exception e)
            {
                throw new ACAException(e);
            }
        }

        /// <summary>
        /// get address info parcel number by address model.
        /// </summary>
        /// <param name="serviceProviderCode">service Provider Code</param>
        /// <param name="refAddressModel">refAddressModel entity</param>
        /// <param name="isUnique">whether unique parcel number.</param>
        /// <returns>address parcel NBR</returns>
        /// <exception cref="DataValidateException">{ <c>serviceProviderCode, refAddressModel</c> } is null</exception>
        /// <exception cref="ACAException">Exception from web service.</exception>
        public RefAddressModel[] GetAddressParcelNbrByAddressModel(string serviceProviderCode, RefAddressModel refAddressModel, bool isUnique)
        {
            if (Logger.IsDebugEnabled)
            {
                Logger.Debug("Begin RefAddressBll.getRefAddressByRefAddressModelParcelModel()");
            }

            if (string.IsNullOrEmpty(serviceProviderCode) || refAddressModel == null)
            {
                throw new DataValidateException(new string[] { "serviceProviderCode", "refAddressModel" });
            }

            try
            {
                RefAddressModel[] response = RefAddressService.getAddressParcelNbrByAddressModel(serviceProviderCode, refAddressModel, isUnique);

                if (Logger.IsDebugEnabled)
                {
                    Logger.Debug("End RefAddressBll.getRefAddressByRefAddressModelParcelModel()");
                }

                return response;
            }
            catch (Exception e)
            {
                throw new ACAException(e);
            }
        }

        /// <summary>
        /// Get Trust account address list by account number
        /// </summary>
        /// <param name="acctNumber">account number</param>
        /// <returns>Array of RefAddressModel for web service</returns>
        /// <exception cref="DataValidateException">{ <c>acctNumber</c> } is null</exception>
        /// <exception cref="ACAException">Exception from web service.</exception>
        public RefAddressModel[] GetAddressListByTrustAccount(string acctNumber)
        {
            if (Logger.IsDebugEnabled)
            {
                Logger.Debug("Begin TrustAccountBll.GetAddressListByTrustAccount()");
            }

            if (acctNumber == null)
            {
                throw new DataValidateException(new string[] { "acctNumber" });
            }

            try
            {
                RefAddressModel[] refAddresses = RefAddressService.getAddressListByTrustAccount(AgencyCode, acctNumber);

                if (Logger.IsDebugEnabled)
                {
                    Logger.Debug("End TrustAccountBll.GetAddressListByTrustAccount()");
                }

                return refAddresses;
            }
            catch (Exception e)
            {
                throw new ACAException(e);
            }
        }

        /// <summary>
        /// Gets the according permit address Array by CAP ID Array.
        /// One CAP Id should return one address accordingly,
        /// if there is no address according to CAP Id,the value should be null((addresses[i] = null)).
        /// </summary>
        /// <param name="capIDs">CapIDModel4WS Array</param>
        /// <returns>daily address list.</returns>
        public RefAddressModel[] GetAddressesByCapIDs(CapIDModel4WS[] capIDs)
        {
            return null;
        }

        /// <summary>
        /// get contact address types by contact type.
        /// </summary>
        /// <param name="serviceProviderCode">agency code</param>
        /// <param name="contactType">contact type</param>
        /// <returns>contact address types</returns>
        public XRefContactAddressTypeModel[] GetContactAddressTypeByContactType(string serviceProviderCode, string contactType)
        {
            return RefAddressService.getContactAddressTypesByContactType(serviceProviderCode, contactType);
        }

        /// <summary>
        /// get contact address validation setting.
        /// </summary>
        /// <param name="serviceProviderCode">service provider code</param>
        /// <param name="contactAddressType">contact address type</param>
        /// <returns>contact address type setting information</returns>
        public XContactAddressTypeSettingModel GetContactAddressValidationSetting(string serviceProviderCode, string contactAddressType)
        {
            return RefAddressService.getContactAddressValidationSetting(serviceProviderCode, contactAddressType);
        }

        /// <summary>
        /// Get RefAddress List of Look Up Work Location Result.
        /// </summary>
        /// <param name="serviceProviderCode">Service Provider Code</param>
        /// <param name="refAddressModel">RefAddress Model for Web Service</param>
        /// <param name="parcelModel">Parcel Model for Web Service</param>
        /// <param name="format">the query format</param>
        /// <returns>Array of reference address information</returns>
        /// <exception cref="DataValidateException">{ <c>serviceProviderCode, refAddressModel</c> } is null</exception>
        /// <exception cref="ACAException">Exception from web service.</exception>
        public SearchResultModel GetRefAddressByRefAddressModelParcelModel(string serviceProviderCode, RefAddressModel refAddressModel, ParcelModel parcelModel, QueryFormat format)
        {
            if (Logger.IsDebugEnabled)
            {
                Logger.Debug("Begin RefAddressBll.getRefAddressByRefAddressModelParcelModel()");
            }

            if (string.IsNullOrEmpty(serviceProviderCode) || refAddressModel == null)
            {
                throw new DataValidateException(new string[] { "serviceProviderCode", "refAddressModel" });
            }

            try
            {
                // Don't change the refAddressModel, its value will be used later, for example Browser Back action.
                RefAddressModel refAddressModelClone = ObjectCloneUtil.DeepCopy<RefAddressModel>(refAddressModel);

                if (refAddressModel.houseNumberStart != null)
                {
                    refAddressModelClone.houseNumberStartFrom = null;
                    refAddressModelClone.houseNumberStartTo = null;
                }

                if (refAddressModel.houseNumberEnd != null)
                {
                    refAddressModelClone.houseNumberEndFrom = null;
                    refAddressModelClone.houseNumberEndTo = null;
                }

                SearchResultModel response = RefAddressService.getRefAddressByRefAddressModelParcelModel(serviceProviderCode, refAddressModelClone, parcelModel, format);

                if (Logger.IsDebugEnabled)
                {
                    Logger.Debug("End RefAddressBll.getRefAddressByRefAddressModelParcelModel()");
                }

                return response;
            }
            catch (Exception e)
            {
                throw new ACAException(e);
            }
        }

        /// <summary>
        /// Validate contact address from external.
        /// </summary>
        /// <param name="serviceProviderCode">service provider code</param>
        /// <param name="contactAddress">contact address</param>
        /// <returns>validated contact address list</returns>
        public List<ContactAddressModel> GetContactAddressListFromExternal(string serviceProviderCode, ContactAddressModel contactAddress)
        {
            List<ContactAddressModel> validatedAddressList = new List<ContactAddressModel>();
            ContactAddressModel[] validatedAddressArray = RefAddressService.getContactAddressListFromExternal(serviceProviderCode, contactAddress);

            if (validatedAddressArray != null)
            {
                validatedAddressList = validatedAddressArray.ToList();
            }

            return validatedAddressList;
        }

        /// <summary>
        /// Get the address source sequence number by reference address id. This method only supports super agency logic,
        /// because reference APO id has unique index and different children agencies have different source sequence numbers.
        /// If you want to get source sequence number for normal agency, you can invoke the method getServiceProviderByPK()
        /// of ServiceProviderWebService by current service provider code.
        /// </summary>
        /// <param name="refAddressID">Reference Address Number</param>
        /// <returns>string value of source sequence number</returns>
        string IRefAddressBll.GetAddressSourceSeqNbrByRefAddressId(string refAddressID)
        {
            return RefAddressService.getAddressSourceSeqNbrByRefAddressId(refAddressID);
        }

        /// <summary>
        /// Convert RefAddressModel to AddressModel.
        /// </summary>
        /// <param name="refAddressModel">RefAddress Model </param>
        /// <returns>Address Model</returns>
        private static AddressModel ConvertRefAddressModel2AddressModel(RefAddressModel refAddressModel)
        {
            if (refAddressModel == null)
            {
                return null;
            }

            AddressModel addressModel = new AddressModel();

            addressModel.houseNumberStart = refAddressModel.houseNumberStart;
            addressModel.houseNumberEnd = refAddressModel.houseNumberEnd;
            addressModel.houseFractionStart = refAddressModel.houseFractionStart;
            addressModel.houseFractionEnd = refAddressModel.houseFractionEnd;

            addressModel.streetDirection = refAddressModel.streetDirection;
            addressModel.resStreetDirection = refAddressModel.resStreetDirection;
            addressModel.streetPrefix = refAddressModel.streetPrefix;
            addressModel.streetName = refAddressModel.streetName;

            addressModel.streetSuffix = refAddressModel.streetSuffix;
            addressModel.resStreetSuffix = refAddressModel.resStreetSuffix;
            addressModel.streetSuffixdirection = refAddressModel.streetSuffixdirection;
            addressModel.resStreetSuffixdirection = refAddressModel.resStreetSuffixdirection;

            addressModel.unitType = refAddressModel.unitType;
            addressModel.resUnitType = refAddressModel.resUnitType;
            addressModel.unitStart = refAddressModel.unitStart;
            addressModel.unitEnd = refAddressModel.unitEnd;

            addressModel.secondaryRoad = refAddressModel.secondaryRoad;
            addressModel.secondaryRoadNumber = refAddressModel.secondaryRoadNumber;

            addressModel.neighborhoodPrefix = refAddressModel.neighborhoodPrefix;
            addressModel.neighborhood = refAddressModel.neighborhood;

            addressModel.addressDescription = refAddressModel.addressDescription;
            addressModel.distance = refAddressModel.distance;

            addressModel.inspectionDistrict = refAddressModel.inspectionDistrict;
            addressModel.inspectionDistrictPrefix = refAddressModel.inspectionDistrictPrefix;

            addressModel.city = refAddressModel.city;
            addressModel.county = refAddressModel.county;
            addressModel.state = refAddressModel.state;
            addressModel.zip = refAddressModel.zip;

            addressModel.fullAddress = refAddressModel.fullAddress;
            addressModel.addressLine1 = refAddressModel.addressLine1;
            addressModel.addressLine2 = refAddressModel.addressLine2;

            addressModel.levelPrefix = refAddressModel.levelPrefix;
            addressModel.levelNumberStart = refAddressModel.levelNumberStart;
            addressModel.levelNumberEnd = refAddressModel.levelNumberEnd;
            addressModel.houseNumberAlphaStart = refAddressModel.houseNumberAlphaStart;
            addressModel.houseNumberAlphaEnd = refAddressModel.houseNumberAlphaEnd;

            addressModel.refAddressId = refAddressModel.refAddressId;

            addressModel.addressType = refAddressModel.addressType;
            addressModel.addressTypeFlag = refAddressModel.addressTypeFlag;

            addressModel.XCoordinator = refAddressModel.XCoordinator;
            addressModel.YCoordinator = refAddressModel.YCoordinator;

            addressModel.primaryFlag = refAddressModel.primaryFlag;

            return addressModel;
        }

        #endregion Methods
    }
}