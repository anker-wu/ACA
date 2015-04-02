#region Header

/**
 *  Accela Citizen Access
 *  File: AssetBll.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2013-2014
 *
 *  Description:
 *  
 *
 *  Notes:
 * $Id: AssetBll.cs 134054 2013-10-16 18:03:54Z ACHIEVO\eric.he $.
 *  Revision History
 *  Date,                  Who,                 What
 */

#endregion Header

using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Accela.ACA.Common;
using Accela.ACA.WSProxy;

namespace Accela.ACA.BLL.Asset
{
    /// <summary>
    /// This class provide the ability to operation Asset.
    /// </summary>
    public class AssetBll : BaseBll, IAssetBll
    {
        /// <summary>
        /// Gets an instance of Asset Web Service.
        /// </summary>
        private AssetWebserviceService AssetWebservice
        {
            get
            {
                return WSFactory.Instance.GetWebService<AssetWebserviceService>();
            }
        }

        /// <summary>
        /// Build asset array to data table
        /// </summary>
        /// <param name="assetInfo">asset array</param>
        /// <returns>asset data table</returns>
        public DataTable BuildAssetDataTable(IEnumerable<AssetMasterModel> assetInfo)
        {
            DataTable dtAsset = new DataTable();
            dtAsset.Columns.Add("g1AssetSequenceNumber", typeof(string));
            dtAsset.Columns.Add("g1AssetID", typeof(string));
            dtAsset.Columns.Add("g1AssetName", typeof(string));
            dtAsset.Columns.Add("g1AssetGroup", typeof(string));
            dtAsset.Columns.Add("g1AssetType", typeof(string));
            dtAsset.Columns.Add("g1ClassType", typeof(string));
            dtAsset.Columns.Add("streetStart", typeof(string));
            dtAsset.Columns.Add("streetEnd", typeof(string));
            dtAsset.Columns.Add("county", typeof(string));
            dtAsset.Columns.Add("countryCode", typeof(string));
            dtAsset.Columns.Add("AssetMasterModel", typeof(AssetMasterModel));

            if (assetInfo == null)
            {
                return dtAsset;
            }

            foreach (AssetMasterModel model in assetInfo)
            {
                DataRow dr = dtAsset.NewRow();
                dr[0] = model.g1AssetSequenceNumber;
                dr[1] = model.g1AssetID;
                dr[2] = model.g1AssetName;
                dr[3] = model.g1AssetGroup;
                dr[4] = model.g1AssetType;
                dr[5] = model.g1ClassType;
                dr[6] = model.streetStart;
                dr[7] = model.streetEnd;
                dr[8] = model.county;
                dr[9] = model.countryCode;
                dr[10] = model;

                dtAsset.Rows.Add(dr);
            }

            DataView dv = dtAsset.DefaultView;
            dv.Sort = "g1AssetID ASC";
            dtAsset = dv.ToTable();

            return dtAsset;
        }

        /// <summary>
        /// Get asset master list by asset model
        /// </summary>
        /// <param name="serviceProviderCode">service provider code</param>
        /// <param name="assetMasterModel">asset model</param>
        /// <param name="capTypeModel">capType Model</param>
        /// <param name="queryFormat">query format</param>
        /// <returns>List of asset master model</returns>
        /// <exception cref="DataValidateException">{ <c>assetMasterModel</c> } is null</exception>
        /// <exception cref="ACAException">Exception from web service.</exception>
        public List<AssetMasterModel> GetAssetListByAssetModel(string serviceProviderCode, AssetMasterModel assetMasterModel, XAssetTypeCapTypeModel capTypeModel, QueryFormat queryFormat)
        {
            if (assetMasterModel == null)
            {
                throw new DataValidateException(new[] { "assetMasterModel" });
            }

            try
            {
                AssetMasterModel[] assetMasterModels = AssetWebservice.getAssetListByAssetModel(serviceProviderCode, assetMasterModel, capTypeModel, queryFormat);

                if (assetMasterModels != null)
                {
                    return assetMasterModels.ToList();
                }

                return null;
            }
            catch (Exception e)
            {
                throw new ACAException(e);
            }
        }

        /// <summary>
        /// Get asset address list by asset sequence number
        /// </summary>
        /// <param name="serviceProviderCode">service provider code</param>
        /// <param name="assetSeqNbr">asset sequence number</param>
        /// <param name="isMultiple">indicate gets one or more address</param>
        /// <returns>asset address list</returns>
        /// <exception cref="ACAException">Exception from web service.</exception>
        public List<RefAddressModel> GetAssetAddressListByAssetSeqNbr(string serviceProviderCode, string assetSeqNbr, bool isMultiple)
        {
            try
            {
                List<RefAddressModel> lstRefAddress = new List<RefAddressModel>();
                RefAddressModel[] arrRefAddress = AssetWebservice.getAssetAddressByAssetSeqNbr(serviceProviderCode, assetSeqNbr, isMultiple);

                if (arrRefAddress != null && arrRefAddress.Length > 0)
                {
                    lstRefAddress = arrRefAddress.ToList();
                }

                return lstRefAddress;
            }
            catch (Exception e)
            {
                throw new ACAException(e);
            }
        }

        /// <summary>
        /// get asset type list by asset group
        /// </summary>
        /// <param name="serviceProviderCode">server provider code</param>
        /// <param name="assetGroup">asset group</param>
        /// <returns>List of asset type</returns>
        /// <exception cref="ACAException">Exception from web service.</exception>
        public List<AssetTypeModel> GetAssetTypeListByAssetGroup(string serviceProviderCode, string assetGroup)
        {
            try
            {
                List<AssetTypeModel> lstAssetType = new List<AssetTypeModel>();
                AssetTypeModel[] arrAssetType = AssetWebservice.getAssetTypeListByAssetGroup(serviceProviderCode, assetGroup);

                if (arrAssetType != null && arrAssetType.Length > 0)
                {
                    lstAssetType = arrAssetType.ToList();
                }

                return lstAssetType;
            }
            catch (Exception e)
            {
                throw new ACAException(e);
            }
        }

        /// <summary>
        /// get asset data model by asset master primary key
        /// </summary>
        /// <param name="serviceProviderCode">server provider code</param>
        /// <param name="assetMasterPk">asset master primary key</param>
        /// <returns>Asset data model</returns>
        /// <exception cref="ACAException">Exception from web service.</exception>
        public AssetDataModel4WS GetAssetDataByAssetPK(string serviceProviderCode, AssetMasterPK assetMasterPk)
        {
            try
            {
                AssetDataModel4WS assetDataModel = AssetWebservice.getAssetDataByAssetPK(serviceProviderCode, assetMasterPk);

                return assetDataModel;
            }
            catch (Exception e)
            {
                throw new ACAException(e);
            }
        }

        /// <summary>
        /// Gets the asset list by GIS objects.
        /// </summary>
        /// <param name="serviceProviderCode">The service provider code.</param>
        /// <param name="gisObjects">The GIS objects.</param>
        /// <param name="xAssetTypeCapTypeModel">Relationship model between Asset Type and Cap Type</param>
        /// <returns>List of asset master</returns>
        /// <exception cref="ACAException">ACA Exception</exception>
        public List<AssetMasterModel> GetAssetListByGISObjects(string serviceProviderCode, List<GISObjectModel> gisObjects, XAssetTypeCapTypeModel xAssetTypeCapTypeModel)
        {
            try
            {
                if (gisObjects == null)
                {
                    return null;
                }

                AssetMasterModel[] assetMasterModels = AssetWebservice.getAssetListByGISObjects(serviceProviderCode, gisObjects.ToArray(), xAssetTypeCapTypeModel);

                if (assetMasterModels != null)
                {
                    return assetMasterModels.ToList();
                }

                return null;
            }
            catch (Exception e)
            {
                throw new ACAException(e);
            }
        }

        /// <summary>
        /// Get the asset group type by cap type
        /// </summary>
        /// <param name="agencyCode">agency code</param>
        /// <param name="xAssetTypeCapTypeModel">asset type cap type model</param>
        /// <returns>The list of Asset Type cap type model</returns>
        /// <exception cref="ACAException">Exception from web service.</exception>
        public List<XAssetTypeCapTypeModel> GetAssetGroupTypeListByCapType(string agencyCode, XAssetTypeCapTypeModel xAssetTypeCapTypeModel)
        {
            try
            {
                XAssetTypeCapTypeModel[] assetTypeCapTypeModels = AssetWebservice.getAssetGroupTypeListByCapType(agencyCode, xAssetTypeCapTypeModel);

                if (assetTypeCapTypeModels != null)
                {
                    return assetTypeCapTypeModels.ToList();
                }

                return null;
            }
            catch (Exception e)
            {
                throw new ACAException(e);
            }
        }

        /// <summary>
        /// Gets the asset type security by user group identifier.
        /// </summary>
        /// <param name="agencyCode">The agency code.</param>
        /// <param name="moduleName">Name of the module.</param>
        /// <param name="assetType">Type of the asset.</param>
        /// <param name="securityItem">The security item.</param>
        /// <param name="callerID">The caller identifier.</param>
        /// <returns>security identity</returns>
        /// <exception cref="ACAException">Exception from web service.</exception>
        public string GetAssetTypeSecurityByUserGroupID(string agencyCode, string moduleName, string assetType, string securityItem, string callerID)
        {
            try
            {
                string security = AssetWebservice.getAssetTypeSecurityByUserGroupID(agencyCode, moduleName, assetType, securityItem, callerID);
                return security;
            }
            catch (Exception e)
            {
                throw new ACAException(e);
            }
        }
    }
}