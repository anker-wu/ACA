#region Header

/**
 *  Accela Citizen Access
 *  File: IAssetBll.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2013
 *
 *  Description:
 *  
 *
 *  Notes:
 * $Id: IAssetBll.cs 134054 2013-10-16 18:03:54Z ACHIEVO\eric.he $.
 *  Revision History
 *  Date,                  Who,                 What
 */

#endregion Header

using System.Collections.Generic;
using System.Data;
using Accela.ACA.WSProxy;

namespace Accela.ACA.BLL.Asset
{
    /// <summary>
    /// interface of Asset
    /// </summary>
    public interface IAssetBll
    {
        /// <summary>
        /// Build asset array to data table
        /// </summary>
        /// <param name="assetInfo">asset array</param>
        /// <returns>asset data table</returns>
        DataTable BuildAssetDataTable(IEnumerable<AssetMasterModel> assetInfo);

        /// <summary>
        /// Get asset master list by asset model
        /// </summary>
        /// <param name="serviceProviderCode">service provider code</param>
        /// <param name="assetMasterModel">asset model</param>
        /// <param name="capTypeModel">capType Model</param>
        /// <param name="queryFormat">query format</param>
        /// <returns>List of asset master model</returns>
        List<AssetMasterModel> GetAssetListByAssetModel(string serviceProviderCode, AssetMasterModel assetMasterModel, XAssetTypeCapTypeModel capTypeModel, QueryFormat queryFormat);

        /// <summary>
        /// Get asset address list by asset sequence number
        /// </summary>
        /// <param name="serviceProviderCode">service provider code</param>
        /// <param name="assetSeqNbr">asset sequence number</param>
        /// <param name="isMultiple">indicate gets one or more address</param>
        /// <returns>asset address list</returns>
        List<RefAddressModel> GetAssetAddressListByAssetSeqNbr(string serviceProviderCode, string assetSeqNbr, bool isMultiple);

        /// <summary>
        /// get asset type list by asset group
        /// </summary>
        /// <param name="serviceProviderCode">server provider code</param>
        /// <param name="assetGroup">asset group</param>
        /// <returns>List of asset type</returns>
        List<AssetTypeModel> GetAssetTypeListByAssetGroup(string serviceProviderCode, string assetGroup);

        /// <summary>
        /// get asset data model by asset master primary key
        /// </summary>
        /// <param name="serviceProviderCode">server provider code</param>
        /// <param name="assetMasterPk">asset master primary key</param>
        /// <returns>Asset data model</returns>
        AssetDataModel4WS GetAssetDataByAssetPK(string serviceProviderCode, AssetMasterPK assetMasterPk);

        /// <summary>
        /// Gets the asset list by GIS objects.
        /// </summary>
        /// <param name="serviceProviderCode">The service provider code.</param>
        /// <param name="gisObjects">The GIS objects.</param>
        /// <param name="xAssetTypeCapTypeModel">Relationship model for Asset Type and Cap Type</param>
        /// <returns>List of asset master</returns>
        List<AssetMasterModel> GetAssetListByGISObjects(string serviceProviderCode, List<GISObjectModel> gisObjects, XAssetTypeCapTypeModel xAssetTypeCapTypeModel);

        /// <summary>
        /// Get the asset group type by cap type
        /// </summary>
        /// <param name="agencyCode">agency code</param>
        /// <param name="xAssetTypeCapTypeModel">asset type cap type model</param>
        /// <returns>The list of Asset Type cap type model</returns>
        List<XAssetTypeCapTypeModel> GetAssetGroupTypeListByCapType(string agencyCode, XAssetTypeCapTypeModel xAssetTypeCapTypeModel);

        /// <summary>
        /// Gets the asset type security by user group identifier.
        /// </summary>
        /// <param name="agencyCode">The agency code.</param>
        /// <param name="moduleName">Name of the module.</param>
        /// <param name="assetType">Type of the asset.</param>
        /// <param name="securityItem">The security item.</param>
        /// <param name="callerID">The caller identifier.</param>
        /// <returns>security identity</returns>
        string GetAssetTypeSecurityByUserGroupID(string agencyCode, string moduleName, string assetType, string securityItem, string callerID);
    }
}
