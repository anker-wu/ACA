#region Header

/**
 *  Accela Citizen Access
 *  File: AssetListEdit.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2013-2014
 *
 *  Description:
 *  
 *
 *  Notes:
 * $Id: AssetListEdit.cs 134054 2013-10-16 18:03:54Z ACHIEVO\eric.he $.
 *  Revision History
 *  Date,                  Who,                 What
 */

#endregion Header

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using Accela.ACA.BLL.Asset;
using Accela.ACA.Common;
using Accela.ACA.Common.Util;
using Accela.ACA.Web.Common;
using Accela.ACA.WSProxy;

namespace Accela.ACA.Web.Component
{
    /// <summary>
    /// Asset list edit for gis
    /// </summary>
    public partial class AssetListEdit
    {
        /// <summary>
        /// ShowOnMap event handler
        /// </summary>
        /// <param name="sender">An object that contains the event sender.</param>
        /// <param name="e">A System.EventArgs object containing the event data.</param>
        protected void MapAsset_ShowOnMap(object sender, EventArgs e)
        {
            ACAGISModel gisModel = GISUtil.CreateACAGISModel();
            gisModel.Context = mapAsset.AGISContext;

            if (AppSession.User.IsAnonymous)
            {
                gisModel.UserGroups.Add(GISUserGroup.Anonymous.ToString());
            }
            else
            {
                gisModel.UserGroups.Add(GISUserGroup.Register.ToString());
            }

            GISUtil.SetPostUrl(Page, gisModel);

            gisModel.ModuleName = ModuleName;
            gisModel.IsMiniMap = mapAsset.IsMiniMode;
            gisModel.Agency = ConfigManager.AgencyCode;

            mapAsset.ACAGISModel = gisModel;
        }

        /// <summary>
        /// Raise post back button click event
        /// </summary>
        /// <param name="sender">An object that contains the event sender.</param>
        /// <param name="e">An System.EventArgs object that contains the event data.</param>
        protected void BtnPostback_Click(object sender, EventArgs e)
        {
            InitialiteFromGis();
        }

        /// <summary>
        /// Initial from GIS.
        /// </summary>
        private void InitialiteFromGis()
        {
            if (Request.Form["__EVENTTARGET"] != btnPostback.UniqueID)
            {
                return;
            }

            string xml = Request.Form["__EVENTARGUMENT"];
            ACAGISModel model = SerializationUtil.XmlDeserialize(xml, typeof(ACAGISModel)) as ACAGISModel;

            if (model != null && ACAConstant.AGIS_COMMAND_SEND_ASSET.Equals(model.CommandName, StringComparison.InvariantCultureIgnoreCase))
            {
                if (model.GisObjects != null)
                {
                    SearchAssetListByGis(model.GisObjects);
                }
                else
                {
                    string script = string.Format("alert('{0}');", string.Format(LabelUtil.GetGlobalTextByKey("aca_assetresult_label_attachnoassetbygis").Replace("'", "\\'"), string.Empty));
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(), "ShowNoRecordMessage", script, true);
                    return;
                }
            }
        }

        /// <summary>
        /// Gets the asset list by gis.
        /// </summary>
        /// <param name="gisObjects">The gis objects.</param>
        /// <returns>The list of asset master model from gis</returns>
        private List<AssetMasterModel> GetAssetListByGis(GISObjectModel[] gisObjects)
        {
            IAssetBll assetBll = ObjectFactory.GetObject<IAssetBll>();
            List<AssetMasterModel> assetMasterModels = new List<AssetMasterModel>();

            if (gisObjects != null)
            {
                CapModel4WS capModel = AppSession.GetCapModelFromSession(ModuleName);
                XAssetTypeCapTypeModel xAssetTypeCapType = new XAssetTypeCapTypeModel();

                if (capModel != null && capModel.capType != null)
                {
                    xAssetTypeCapType.servProvCode = ConfigManager.AgencyCode;
                    xAssetTypeCapType.group = capModel.capType.group;
                    xAssetTypeCapType.subType = capModel.capType.subType;
                    xAssetTypeCapType.type = capModel.capType.type;
                    xAssetTypeCapType.category = capModel.capType.category;
                }

                assetMasterModels = assetBll.GetAssetListByGISObjects(ConfigManager.AgencyCode, gisObjects.ToList(), xAssetTypeCapType);
            }

            return assetMasterModels;
        }

        /// <summary>
        /// Searches the asset list by gis.
        /// </summary>
        /// <param name="gisObjects">The gis objects.</param>
        private void SearchAssetListByGis(GISObjectModel[] gisObjects)
        {
            try
            {
                int failCount = 0;
                int noAssociatedCount = 0;
                int successCount = 0;
                List<AssetMasterModel> gisAssetList = GetAssetListByGis(gisObjects);

                if (gisAssetList == null || gisAssetList.Count == 0)
                {
                    string message = string.Format(
                        LabelUtil.GetGlobalTextByKey("aca_assetresult_label_attachnoassetbygis").Replace("'", "\\'"), 
                        gisObjects != null ? gisObjects.Length.ToString() : string.Empty);

                    string script = string.Format("alert('{0}');", message);
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(), "ShowNoRecordMessage", script, true);
                    return;
                }

                if (gisObjects != null)
                {
                    noAssociatedCount = gisObjects.Count() - gisAssetList.Count;
                }
                
                // 1. merge gis data to cap asset
                List<AssetMasterModel> existAssetList = (CapModel != null && CapModel.assetList != null)
                                                        ? CapModel.assetList.ToList()
                                                        : new List<AssetMasterModel>();

                List<AssetMasterModel> mergedGisAssetList = new List<AssetMasterModel>();

                if (existAssetList.Count == 0)
                {
                    mergedGisAssetList = gisAssetList;
                    successCount = gisAssetList.Count();
                }
                else
                {
                    mergedGisAssetList = existAssetList;

                    foreach (AssetMasterModel assetMaster in gisAssetList)
                    {
                        if (existAssetList.Contains(assetMaster, new AssetMasterModel.Comparer()))
                        {
                            failCount += 1;
                        }
                        else
                        {
                            mergedGisAssetList.Add(assetMaster);
                            successCount += 1;
                        }
                    }
                }

                // 2.show messge for merged result
                if (mergedGisAssetList.Count > 0)
                {
                    CapModel.assetList = mergedGisAssetList.ToArray();
                    ShowAttchResultMessage(successCount, failCount, noAssociatedCount);
                }

                mapAsset.CloseMap();
                mapPanel.Update();

                BindAssetList();
            }
            catch (ACAException ex)
            {
                MessageUtil.ShowMessageByControl(Page, MessageType.Error, ex.Message);
            }
        }
    }
}