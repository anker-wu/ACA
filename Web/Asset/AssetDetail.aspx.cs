#region Header

/**
 *  Accela Citizen Access
 *  File: AssetDetail.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2013-2014
 *
 *  Description:
 *  
 *
 *  Notes:
 * $Id: AssetDetail.cs 134054 2013-10-16 18:03:54Z ACHIEVO\eric.he $.
 *  Revision History
 *  Date,                  Who,                 What
 */

#endregion Header

using System;
using System.Collections.Generic;
using System.Text;
using Accela.ACA.BLL.Asset;
using Accela.ACA.Common;
using Accela.ACA.Web.Common;
using Accela.ACA.WSProxy;

namespace Accela.ACA.Web.Asset
{
    /// <summary>
    /// Asset detail class
    /// </summary>
    public partial class AssetDetail : PopupDialogBasePage
    {
        /// <summary>
        /// On initial event
        /// </summary>
        /// <param name="e">event arguments</param>
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            DialogUtil.RegisterScriptForDialog(Page);
        }

        /// <summary>
        /// Handles the Load event of the Page control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The EventArgs instance containing the event data.</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            SetDialogMaxHeight("550");

            if (!AppSession.IsAdmin)
            {
                lblAssetDetailSection.Visible = false;
                SetPageTitleKey("aca_assetdetail_label_assetdetailsectiontitle");
            }

            if (!Page.IsPostBack)
            {
                //Decode the asset info from args querystring.
                string assetArgs = Request.QueryString[UrlConstant.ASSET_DETAIL_ARGS];

                if (string.IsNullOrEmpty(assetArgs))
                {
                    assetAddressList.AssetRefAddressList = new List<RefAddressModel>();
                    assetAddressList.BindAssetAddressList();
                    return;
                }

                string assetData = Encoding.UTF8.GetString(Convert.FromBase64String(assetArgs));
                string[] args = assetData.Split(ACAConstant.SPLIT_CHAR);

                BindAssetDetailAndRefAddress(args);
            }
            else
            {
                assetDetail.Display(null, null);
            }
        }

        /// <summary>
        /// Binds the asset master detail and reference address.
        /// </summary>
        /// <param name="assetArgs">The asset arguments.</param>
        private void BindAssetDetailAndRefAddress(string[] assetArgs)
        {
            try
            {
                if (assetArgs != null && assetArgs.Length == 6)
                {
                    IAssetBll assetBll = ObjectFactory.GetObject<IAssetBll>();
                    AssetMasterModel assetMasterModel = new AssetMasterModel();

                    assetMasterModel.g1AssetSequenceNumber = long.Parse(assetArgs[0]);
                    assetMasterModel.g1AssetID = assetArgs[1];
                    assetMasterModel.g1AssetGroup = assetArgs[2];
                    assetMasterModel.g1AssetType = assetArgs[3];
                    assetMasterModel.g1AssetName = assetArgs[4];
                    assetMasterModel.g1ClassType = assetArgs[5];

                    AssetMasterPK assetMasterPk = new AssetMasterPK();
                    assetMasterPk.g1AssetSequenceNumber = long.Parse(assetArgs[0]);
                    assetMasterPk.g1AssetSequenceNumberSpecified = true;
                    assetMasterPk.serviceProviderCode = ConfigManager.AgencyCode;
                    AssetDataModel4WS assetDataModel = assetBll.GetAssetDataByAssetPK(ConfigManager.AgencyCode, assetMasterPk);

                    assetDetail.Display(assetMasterModel, assetDataModel);

                    string assetGroupType = assetMasterModel.g1AssetGroup + ACAConstant.SLASH + assetMasterModel.g1AssetType;
                    string addressPromission = assetBll.GetAssetTypeSecurityByUserGroupID(ConfigManager.AgencyCode, ModuleName, assetGroupType, ACAConstant.ASSET_SECURITY_ADDRESS_TAB, AppSession.User.PublicUserId);

                    if (addressPromission.Equals(ACAConstant.Security.None, StringComparison.InvariantCultureIgnoreCase))
                    {
                        lblAssetAddressSection.Visible = false;
                        assetAddressList.Visible = false;
                    }
                    else
                    {
                        List<RefAddressModel> refAddressList = assetBll.GetAssetAddressListByAssetSeqNbr(ConfigManager.AgencyCode, assetMasterModel.g1AssetSequenceNumber.ToString(), true);

                        assetAddressList.AssetRefAddressList = refAddressList;
                        assetAddressList.BindAssetAddressList();
                    }
                }
            }
            catch (Exception exception)
            {
                MessageUtil.ShowMessage(this, MessageType.Error, exception.Message);
            }
        }
    }
}