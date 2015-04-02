#region Header

/**
 *  Accela Citizen Access
 *  File: AssetDetailView.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2013
 *
 *  Description:
 *  
 *
 *  Notes:
 * $Id: AssetDetailView.cs 134054 2013-10-16 18:03:54Z ACHIEVO\eric.he $.
 *  Revision History
 *  Date,                  Who,                 What
 */

#endregion Header

using Accela.ACA.Common;
using Accela.ACA.Web.Common;
using Accela.ACA.WSProxy;

namespace Accela.ACA.Web.Component
{
    /// <summary>
    /// Asset detail view class
    /// </summary>
    public partial class AssetDetailView : BaseUserControl
    {
        /// <summary>
        /// Gets or sets Asset master model
        /// </summary>
        public AssetMasterModel AssetMaster
        {
            get
            {
                if (ViewState["AssetMaster"] != null)
                {
                    return (AssetMasterModel)ViewState["AssetMaster"];
                }

                return new AssetMasterModel();
            }

            set
            {
                ViewState["AssetMaster"] = value;
            }
        }

        /// <summary>
        /// Display Asset Information
        /// </summary>
        /// <param name="asset">Asset Model</param>
        /// <param name="assetDataModel">The asset data model.</param>
        public void Display(AssetMasterModel asset, AssetDataModel4WS assetDataModel)
        {
            try
            {
                if (asset != null)
                {
                    AssetMasterModel assetMaster = new AssetMasterModel();

                    assetMaster.g1AssetID = asset.g1AssetID;
                    assetMaster.g1AssetName = asset.g1AssetName;
                    assetMaster.g1AssetGroup = asset.g1AssetGroup;
                    assetMaster.g1AssetType = asset.g1AssetType;
                    assetMaster.g1ClassType = asset.g1ClassType;

                    AssetMaster = assetMaster;
                }

                lblAssetId.Value = AssetMaster.g1AssetID;
                lblAssetName.Value = AssetMaster.g1AssetName;
                lblAssetGroup.Value = AssetMaster.g1AssetGroup;
                lblAssetType.Value = AssetMaster.g1AssetType;
                lblClassType.Value = AssetMaster.g1ClassType;

                AssetTemplateView.Display(assetDataModel);
            }
            catch (ACAException e)
            {
                MessageUtil.ShowMessage(Page, MessageType.Error, e.Message);
            }
        }
    }
}