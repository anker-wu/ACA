#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: APOBll.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2008-2014
 *
 *  Description:
 *
 *  Notes:
 *      $Id: APOBll.cs 278175 2014-08-28 09:03:13Z ACHIEVO\james.shi $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using System;
using System.Collections.Generic;
using System.Data;
using Accela.ACA.BLL.AddressBuilder;
using Accela.ACA.Common;
using Accela.ACA.Common.Util;
using Accela.ACA.WSProxy;

namespace Accela.ACA.BLL.APO
{
    /// <summary>
    /// This class provide the ability to operation APO.
    /// </summary>
    public sealed class APOBll : BaseBll, IAPOBll
    {
        #region Properties

        /// <summary>
        /// Gets an instance of CapService.
        /// </summary>
        private CapWebServiceService CapService
        {
            get
            {
                return WSFactory.Instance.GetWebService<CapWebServiceService>();
            }
        }

        /// <summary>
        /// Gets an instance of OwnerService.
        /// </summary>
        private OwnerWebServiceService OwnerService
        {
            get
            {
                return WSFactory.Instance.GetWebService<OwnerWebServiceService>();
            }
        }

        /// <summary>
        /// Gets an instance of ParcelService.
        /// </summary>
        private ParcelWebServiceService ParcelService
        {
            get
            {
                return WSFactory.Instance.GetWebService<ParcelWebServiceService>();
            }
        }

        /// <summary>
        /// Gets an instance of APOWebServiceService.
        /// </summary>
        private APOWebServiceService APOService
        {
            get
            {
                return WSFactory.Instance.GetWebService<APOWebServiceService>();
            }
        }

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
        /// Get APO data by address
        /// </summary>
        /// <param name="serviceProviderCode">The agency code</param>
        /// <param name="addressModel">The RefAddressModel model</param>
        /// <param name="queryFormat">The Query Format</param>
        /// <param name="isForceValid">Is Force Valid</param>
        /// <returns>A data table</returns>
        /// <exception cref="ACAException">Exception from web service.</exception>
        /// <remarks>1. Construct a RefAddressModel model and call SearchAPO to get a list of ParcelInfoModel.
        /// 2. call BuildAPODataTable() method to fill APO information into a data table
        /// 3. return the converted data table.</remarks>
        SearchResultModel IAPOBll.GetAPOListByAddress(string serviceProviderCode, RefAddressModel addressModel, QueryFormat queryFormat, bool isForceValid)
        {
            ParcelInfoModel parcelInfo = new ParcelInfoModel();

            // Don't change the addressModel, its value will be used later, for example Browser Back action.
            parcelInfo.RAddressModel = ObjectCloneUtil.DeepCopy<RefAddressModel>(addressModel);
            parcelInfo.RAddressModel.auditDate = null;
            SearchResultModel parcelInfos = null;

            if (addressModel.houseNumberStart != null)
            {
                parcelInfo.RAddressModel.houseNumberStartFrom = null;
                parcelInfo.RAddressModel.houseNumberStartTo = null;
            }

            if (addressModel.houseNumberEnd != null)
            {
                parcelInfo.RAddressModel.houseNumberEndFrom = null;
                parcelInfo.RAddressModel.houseNumberEndTo = null;
            }

            try
            {
                parcelInfos = QueryAPO(serviceProviderCode, parcelInfo, queryFormat, isForceValid);
            }
            catch (Exception e)
            {
                throw new ACAException(e);
            }

            return parcelInfos;
        }

        /// <summary>
        /// Query APO information by a specified cap number
        /// </summary>
        /// <param name="serviceProviderCode">The agency code</param>
        /// <param name="capModel">The CapModel4WS model. Only cap id field is needed in this method</param>
        /// <param name="callerID">The public user id</param>
        /// <param name="queryFormat">Query Format</param>
        /// <returns>An array of ParcelInfoModel</returns>
        /// <exception cref="DataValidateException">{ <c>serviceProviderCode</c> } is null</exception>
        /// <exception cref="ACAException">Exception from web service.</exception>
        /// <remarks>1. Call getAPOListByCap method of ParcelWebService
        /// to return a array of ParcelInfoModel model.
        /// 2. Call BuildAPODataTable() method to fill APO information into a data table
        /// 3. Return the converted data table.</remarks>
        ParcelInfoModel[] IAPOBll.GetAPOListByCap(string serviceProviderCode, CapModel4WS capModel, string callerID, QueryFormat queryFormat)
        {
            if (string.IsNullOrEmpty(serviceProviderCode))
            {
                throw new DataValidateException(new string[] { "serviceProviderCode" });
            }

            try
            {
                ParcelInfoModel[] parcelInfos = CapService.getAPOListByCap(serviceProviderCode, capModel, queryFormat, callerID);

                return parcelInfos;
            }
            catch (Exception e)
            {
                throw new ACAException(e);
            }
        }

        /// <summary>
        /// Get APO data by parcel owner
        /// </summary>
        /// <param name="serviceProviderCode">The agency code</param>
        /// <param name="owner">The OwnerModel model</param>
        /// <param name="queryFormat">The Query Format</param>
        /// <param name="isForceValid">Is Force Valid</param>
        /// <returns>A data table</returns>
        /// <remarks>1. Construct a OwnerModel model with a set of owner search criteria(in propertyLookUp page) or only owner sequence number and owner number(in detail detail page).
        /// 2. call SearchAPO to get a list of ParcelInfoModel.
        /// 3. call BuildAPODataTable() method to fill APO information into a data table
        /// 4. return the converted data table.</remarks>
        SearchResultModel IAPOBll.GetAPOListByOwner(string serviceProviderCode, OwnerModel owner, QueryFormat queryFormat, bool isForceValid)
        {
            ParcelInfoModel parcelInfo = new ParcelInfoModel();
            parcelInfo.ownerModel = owner;
            SearchResultModel parcelInfos = QueryAPO(serviceProviderCode, parcelInfo, queryFormat, isForceValid);

            return parcelInfos;
        }

        /// <summary>
        /// Get APO data by parcel
        /// </summary>
        /// <param name="serviceProviderCode">The agency code</param>
        /// <param name="parcel">The ParcelModel model</param>
        /// <param name="queryFormat">The Query Format</param>
        /// <param name="isForceValid">Is Force Valid</param>
        /// <returns>A data table</returns>
        /// <remarks>1. Construct a ParcelModel model with a set of parcel search criteria(in propertyLookUp page) or only parcel sequence number and parcel number(in parcel detail page).
        /// 2. call SearchAPO to get a list of ParcelInfoModel.
        /// 3. call BuildAPODataTable() method to fill APO information into a data table
        /// 4. return the converted data table.</remarks>
        SearchResultModel IAPOBll.GetAPOListByParcel(string serviceProviderCode, ParcelModel parcel, QueryFormat queryFormat, bool isForceValid)
        {
            ParcelInfoModel parcelInfo = new ParcelInfoModel();
            parcelInfo.parcelModel = parcel;
            SearchResultModel parcelInfos = QueryAPO(serviceProviderCode, parcelInfo, queryFormat, isForceValid);

            return parcelInfos;
        }

        /// <summary>
        /// get APO information with licensee.
        /// </summary>
        /// <param name="license">licensee model</param>
        /// <param name="queryFormat">query format model</param>
        /// <returns>a data table for APO</returns>
        ParcelInfoModel[] IAPOBll.GetAPOListByLP(LicenseModel4WS license, QueryFormat queryFormat)
        {
            ParcelInfoModel[] parcelInfos = ParcelService.getAPOListByLicense(license, queryFormat);

            return parcelInfos;
        }

        /// <summary>
        /// Get APO data by owner key value; this method support both internal APO and XAPO
        /// </summary>
        /// <param name="agencyCode">The agency code</param>
        /// <param name="ownerPK">OwnerModel with PK value.</param>
        /// <param name="queryFormat">The query format</param>
        /// <returns>A DataTable</returns>
        /// <remarks>1. Encapsulate Owner key value (include sequence number and owner number or owner UID) into OwnerModel as parameter(in detail page).
        /// 2. Call SearchAPO to get a list of ParcelInfoModel.
        /// 3. Call BuildAPODataTable() method to fill APO information into a DataTable
        /// 4. Return the converted DataTable.</remarks>
        SearchResultModel IAPOBll.GetAddressListByOwnerPK(string agencyCode, OwnerModel ownerPK, QueryFormat queryFormat)
        {
            ParcelInfoModel parcelInfo = new ParcelInfoModel();
            parcelInfo.ownerModel = ownerPK;
            SearchResultModel result = QueryAPO(agencyCode, parcelInfo, queryFormat, false);
            ParcelInfoModel[] parcelInfos = ObjectConvertUtil.ConvertObjectArray2EntityArray<ParcelInfoModel>(result.resultList);
            List<ParcelInfoModel> addressList = new List<ParcelInfoModel>();

            if (parcelInfos != null)
            {
                foreach (ParcelInfoModel parcelInfoModel in parcelInfos)
                {
                    if (parcelInfoModel == null || parcelInfoModel.RAddressModel == null)
                    {
                        continue;
                    }

                    if (parcelInfoModel.RAddressModel.refAddressId != null || parcelInfoModel.RAddressModel.UID != null)
                    {
                        addressList.Add(parcelInfoModel);
                    }
                }
            }

            result.resultList = addressList.ToArray();

            return result;
        }

        /// <summary>
        /// Get APO data by parcel key value; this method support both internal APO and XAPO
        /// </summary>
        /// <param name="agencyCode">The agency code</param>
        /// <param name="parcelPK">Parcel model with PK value</param>
        /// <param name="isForGenealogy">Is For Genealogy</param>
        /// <param name="queryFormat">Query format</param>
        /// <returns>A DataTable</returns>
        /// <remarks>1. Encapsulate Parcel key value (include sequence number and parcel number or parcel UID) into ParcelModel as parameter (in parcel detail page).
        /// 2. Call SearchAPO to get a list of ParcelInfoModel.
        /// 3. Call BuildAPODataTable() method to fill APO information into a DataTable
        /// 4. Return the converted DataTable.</remarks>
        SearchResultModel IAPOBll.GetAddressListByParcelPK(string agencyCode, ParcelModel parcelPK, bool isForGenealogy, QueryFormat queryFormat)
        {
            ParcelInfoModel parcelInfo = new ParcelInfoModel();
            parcelInfo.parcelModel = parcelPK;
            SearchResultModel searchResult = new SearchResultModel();
            List<ParcelInfoModel> addressList = new List<ParcelInfoModel>();

            if (isForGenealogy)
            {
                searchResult.resultList = ParcelService.getParcelInfoListByParcelPK(agencyCode, parcelPK);
            }
            else
            {
                searchResult = ParcelService.getAPOList(agencyCode, parcelInfo, true, queryFormat, false);
            }

            ParcelInfoModel[] parcelInfos = ObjectConvertUtil.ConvertObjectArray2EntityArray<ParcelInfoModel>(searchResult.resultList);

            if (parcelInfos != null)
            {
                foreach (ParcelInfoModel parcelInfoModel in parcelInfos)
                {
                    if (parcelInfoModel == null || parcelInfoModel.RAddressModel == null)
                    {
                        continue;
                    }

                    if (parcelInfoModel.RAddressModel.refAddressId != null || parcelInfoModel.RAddressModel.UID != null)
                    {
                        addressList.Add(parcelInfoModel);
                    }
                }
            }

            searchResult.resultList = addressList.ToArray();

            return searchResult;
        }

        /// <summary>
        /// Get APO data by address
        /// </summary>
        /// <param name="serviceProviderCode">The agency code</param>
        /// <param name="addressModel">The RefAddressModel4WS model</param>
        /// <param name="queryFormat">The queryFormat</param>
        /// <param name="isForceValid">spear form not search disabled parcel.</param>
        /// <returns>return ParcelInfoModel array</returns>
        /// <remarks>1. Construct a RefAddressModel4WS model and call SearchAPO to get a list of ParcelInfoModel4WS.
        /// 2. call BuildAPODataTable() method to fill APO information into a data table
        /// 3. return the converted data table.</remarks>
        SearchResultModel IAPOBll.GetParcelInfoByAddress(string serviceProviderCode, RefAddressModel addressModel, QueryFormat queryFormat, bool isForceValid)
        {
            ParcelInfoModel parcelInfo = new ParcelInfoModel();
            DateTime? date = null;

            if (addressModel != null)
            {
                // Don't change the addressModel, its value will be used later, for example Browser Back action.
                RefAddressModel refAddressModelClone = ObjectCloneUtil.DeepCopy<RefAddressModel>(addressModel); 

                parcelInfo.RAddressModel = refAddressModelClone;
                date = parcelInfo.RAddressModel.auditDate;
                parcelInfo.RAddressModel.auditDate = null;

                if (addressModel.houseNumberStart != null)
                {
                    parcelInfo.RAddressModel.houseNumberStartFrom = null;
                    parcelInfo.RAddressModel.houseNumberStartTo = null;
                }

                if (addressModel.houseNumberEnd != null)
                {
                    parcelInfo.RAddressModel.houseNumberEndFrom = null;
                    parcelInfo.RAddressModel.houseNumberEndTo = null;
                }
            }

            try
            {
                SearchResultModel parcelInfos = QueryAPO(serviceProviderCode, parcelInfo, queryFormat, isForceValid);

                return parcelInfos;
            }
            finally
            {
                parcelInfo.RAddressModel.auditDate = date;
            }
        }

        /// <summary>
        /// Get APO data by parcel
        /// </summary>
        /// <param name="serviceProviderCode">The agency code</param>
        /// <param name="parcel">The ParcelModel4WS model</param>
        /// <param name="queryFormat">Query format</param>
        /// <param name="isForceValid">Is Force Valid</param>
        /// <returns>A ParcelInfoModel4WS array</returns>
        /// <remarks>1. Construct a ParcelModel4WS model with a set of parcel search criteria(in propertyLookUp page) or only parcel sequence number and parcel number(in parcel detail page).
        /// 2. call SearchAPO to get a list of ParcelInfoModel4WS.
        /// 3. call BuildAPODataTable() method to fill APO information into a data table
        /// 4. return the converted data table.</remarks>
        SearchResultModel IAPOBll.GetParcelInfoByParcel(string serviceProviderCode, ParcelModel parcel, QueryFormat queryFormat, bool isForceValid)
        {
            ParcelInfoModel parcelInfo = new ParcelInfoModel();
            parcelInfo.parcelModel = parcel;
            SearchResultModel parcelInfos = QueryAPO(serviceProviderCode, parcelInfo, queryFormat, isForceValid);

            return parcelInfos;
        }

        /// <summary>
        /// Get the Ref Address List.
        /// </summary>
        /// <param name="serviceProviderCode">Agency code</param>
        /// <param name="addressModel">Address Model</param>
        /// <param name="queryFormat">Query Format of address</param>
        /// <returns>RefAddressModel List</returns>
        /// <exception cref="DataValidateException">{ <c>serviceProviderCode</c> } is null</exception>
        /// <exception cref="ACAException">Exception from web service.</exception>
        SearchResultModel IAPOBll.GetRefAddressList(string serviceProviderCode, RefAddressModel addressModel, QueryFormat queryFormat)
        {
            if (string.IsNullOrEmpty(serviceProviderCode))
            {
                throw new DataValidateException(new[] { "serviceProviderCode" });
            }
            
            try
            {
                if (addressModel != null)
                {
                    RefAddressModel searchModel = ObjectCloneUtil.DeepCopy(addressModel);

                    searchModel.auditDate = null;

                    if (searchModel.houseNumberStart != null)
                    {
                        searchModel.houseNumberStartFrom = null;
                        searchModel.houseNumberStartTo = null;
                    }

                    if (searchModel.houseNumberEnd != null)
                    {
                        searchModel.houseNumberEndFrom = null;
                        searchModel.houseNumberEndTo = null;
                    }

                    SearchResultModel searchResult = APOService.getRefAddressList(serviceProviderCode, searchModel, queryFormat);
                    return searchResult;
                }

                return null;
            }
            catch (Exception e)
            {
                throw new ACAException(e);
            }
        }

        /// <summary>
        /// Gets parcel associated address records.
        /// </summary>
        /// <param name="serviceProviderCode">Agency code</param>
        /// <param name="parcelPKModel">Parcel PK Model</param>
        /// <param name="filterDisableOwner">Indicate whether filter disabled owners.</param>
        /// <returns>Associated Address List</returns>
        /// <exception cref="DataValidateException">{ <c>serviceProviderCode</c> } is null</exception>
        /// <exception cref="ACAException">Exception from web service.</exception>
        RefAddressModel[] IAPOBll.GetRefAddressListByParcelPK(string serviceProviderCode, ParcelModel parcelPKModel, bool filterDisableOwner)
        {
            if (string.IsNullOrEmpty(serviceProviderCode))
            {
                throw new DataValidateException(new[] { "serviceProviderCode" });
            }

            try
            {
                if (parcelPKModel != null)
                {
                    RefAddressModel[] addressList = APOService.getRefAddressListByParcelPK(serviceProviderCode, parcelPKModel, filterDisableOwner);
                    return addressList;
                }

                return null;
            }
            catch (Exception e)
            {
                throw new ACAException(e);
            }
        }

        /// <summary>
        /// Gets parcel associated address records via send back from gis map.
        /// </summary>
        /// <param name="serviceProviderCode">Agency code</param>
        /// <param name="parcelPKModel">Parcel PK Model</param>
        /// <param name="queryFormat">Query Format of parcel</param>
        /// <returns>Associated Address List</returns>
        /// <exception cref="DataValidateException">{ <c>serviceProviderCode</c> } is null</exception>
        /// <exception cref="ACAException">Exception from web service.</exception>
        SearchResultModel IAPOBll.GetRefAddressListByParcel(string serviceProviderCode, ParcelModel parcelPKModel, QueryFormat queryFormat)
        {
            if (string.IsNullOrEmpty(serviceProviderCode))
            {
                throw new DataValidateException(new[] { "serviceProviderCode" });
            }

            try
            {
                if (parcelPKModel != null)
                {
                    SearchResultModel searchResult = APOService.getRefAddressListByParcel(serviceProviderCode, parcelPKModel, queryFormat);
                    return searchResult;
                }

                return null;
            }
            catch (Exception e)
            {
                throw new ACAException(e);
            }
        }

        /// <summary>
        /// Gets ref parcel list.
        /// </summary>
        /// <param name="serviceProviderCode">Agency code</param>
        /// <param name="parcelModel">Parcel model</param>
        /// <param name="queryFormat">Query format</param>
        /// <returns>Ref parcel list</returns>
        /// <exception cref="DataValidateException">{ <c>serviceProviderCode</c> } is null</exception>
        /// <exception cref="ACAException">Exception from web service.</exception>
        SearchResultModel IAPOBll.GetRefParcelList(string serviceProviderCode, ParcelModel parcelModel, QueryFormat queryFormat)
        {
            if (string.IsNullOrEmpty(serviceProviderCode))
            {
                throw new DataValidateException(new[] { "serviceProviderCode" });
            }

            try
            {
                if (parcelModel != null)
                {
                    SearchResultModel searchResult = APOService.getRefParcelList(serviceProviderCode, parcelModel, queryFormat);
                    return searchResult;
                }

                return null;
            }
            catch (Exception e)
            {
                throw new ACAException(e);
            }
        }

        /// <summary>
        /// Owner Section in Parcel Detail
        /// Query related owner information to a specified parcel.
        /// </summary>
        /// <param name="agencyCode">Agency Code.</param>
        /// <param name="parcelPKArray">The ParcelModel Array with parcel PK value.
        /// We can pass multiple parcel numbers to get multiple owners or just a single parcel number.</param>
        /// <param name="filterDisableOwner">Indicate whether filter disabled owners.</param>
        /// <param name="queryFormat">Owner list query format.</param>
        /// <returns>a DataTable of owner information. This DataTable consists of three columns:Name, relationship and address</returns>
        /// <exception cref="ACAException">Exception from web service.</exception>
        /// 1. call GetOwnerListByParcelPKs method of OwnerService
        /// to return an array of OwnerModel.
        /// 2. call BuildOwnerDataTable() method to fill APO information into a DataTable
        /// 3. return the converted DataTableDataTable.
        SearchResultModel IAPOBll.GetOwnerListByParcelPKs(string agencyCode, ParcelModel[] parcelPKArray, bool filterDisableOwner, QueryFormat queryFormat)
        {
            try
            {
                SearchResultModel searchResult = APOService.getOwnerListByParcelPKs(agencyCode, parcelPKArray, filterDisableOwner, queryFormat);
                return searchResult;
            }
            catch (Exception e)
            {
                throw new ACAException(e);
            }
        }

        /// <summary>
        /// Get owner list by a specified parcel.
        /// </summary>
        /// <param name="agencyCode">Agency Code.</param>
        /// <param name="ownerModel">An OwnerCompModel</param>
        /// <param name="queryFormat">Owner list query format.</param>
        /// <returns>A SearchResultModel</returns>
        /// <exception cref="ACAException">Exception from web service.</exception>
        SearchResultModel IAPOBll.GetRefOwnerList(string agencyCode, OwnerCompModel ownerModel, QueryFormat queryFormat)
        {
            try
            {
                SearchResultModel searchResult = APOService.getRefOwnerList(agencyCode, ownerModel, queryFormat);

                return searchResult;
            }
            catch (Exception e)
            {
                throw new ACAException(e);
            }
        }

        /// <summary>
        /// Query APO information by address, parcel or owner.
        /// </summary>
        /// <param name="serviceProviderCode">Service Provider Code</param>
        /// <param name="parcelInfoModel">
        /// a ParcelInfoModel as query condition.This model contains three models:RefAddressModel, ParcelModel and RAddressModel. 
        /// If we want to search APO by a set of address criteria, only fill the RefAddressModel in the ParcelInfoModel. 
        /// If we want to search APO by a set of parcel criteria, only fill the ParcelModel in this parameter. 
        /// If we want to search APO by a set of owner criteria, only fill the OwnerModel in this parameter
        /// </param>
        /// <param name="queryFormat">query Format</param>
        /// <param name="isForceValid">Is force valid flag</param>
        /// <returns>An array of ParcelInfoModel</returns>
        /// <exception cref="DataValidateException">{ <c>serviceProviderCode</c> } is null</exception>
        /// <exception cref="ACAException">Exception from web service.</exception>
        /// <remarks>1. call getAPOList method of ParcelWebService to return ParcelInfoModel list.</remarks>
        private SearchResultModel QueryAPO(string serviceProviderCode, ParcelInfoModel parcelInfoModel, QueryFormat queryFormat, bool isForceValid)
        {
            if (string.IsNullOrEmpty(serviceProviderCode))
            {
                throw new DataValidateException(new string[] { "serviceProviderCode" });
            }

            try
            {
                SearchResultModel searchResult = new SearchResultModel();
                
                if (parcelInfoModel.parcelModel != null)
                {
                    searchResult = ParcelService.getAPOList(serviceProviderCode, parcelInfoModel, false, queryFormat, isForceValid);
                }
                else if (parcelInfoModel.RAddressModel != null)
                {
                    searchResult = RefAddressService.getAPOList(serviceProviderCode, parcelInfoModel, queryFormat, isForceValid);
                }
                else
                {
                    searchResult = OwnerService.getAPOList(serviceProviderCode, parcelInfoModel, queryFormat, isForceValid);
                }

                return searchResult;
            }
            catch (Exception e)
            {
                throw new ACAException(e);
            }
        }

        #endregion Methods
    }
}
