#region Header

/**
 *  Accela Citizen Access
 *  File: AssetResult.ascx.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2013-2014
 *
 *  Description:
 *  
 *
 *  Notes:
 * $Id: AssetResult.ascx.cs 134054 2013-10-16 18:03:54Z ACHIEVO\eric.he $.
 *  Revision History
 *  Date,                  Who,                 What
 */

#endregion Header

using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using Accela.ACA.BLL.Asset;
using Accela.ACA.Common;
using Accela.ACA.Common.Util;
using Accela.ACA.Web.AppCode.Common;
using Accela.ACA.Web.Common;
using Accela.ACA.Web.Common.Control;
using Accela.ACA.WSProxy;
using Accela.Web.Controls;

namespace Accela.ACA.Web.Component
{
    /// <summary>
    /// Asset result class
    /// </summary>
    public partial class AssetResult : BaseUserControl
    {
        /// <summary>
        /// The export file name for asset search result list.
        /// </summary>
        private const string EXPORT_FILE_NAME = "AssetResults";

        #region Properties

        /// <summary>
        /// Gets or sets the asset search model
        /// </summary>
        public AssetMasterModel AssetSearchModel
        {
            get
            {
                return (AssetMasterModel)ViewState["AssetSearchModel"];
            }

            set
            {
                ViewState["AssetSearchModel"] = value;
            }
        }

        /// <summary>
        /// Gets or sets asset data source
        /// </summary>
        private DataTable AssetDataSource
        {
            get
            {
                return (DataTable)ViewState["AssetDataSource"];
            }

            set
            {
                ViewState["AssetDataSource"] = value;
            }
        }

        /// <summary>
        /// Gets the cap model.
        /// </summary>
        /// <value>
        /// The cap model.
        /// </value>
        private CapModel4WS CapModel
        {
            get
            {
                return AppSession.GetCapModelFromSession(ModuleName);
            }
        }

        #endregion Properties

        /// <summary>
        /// Raises the event.
        /// </summary>
        /// <param name="e">An object that contains the event data.</param>
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            DialogUtil.RegisterScriptForDialog(Page);
            GridViewBuildHelper.SetSimpleViewElements(gvAssetResultList, ModuleName, AppSession.IsAdmin);
            InitalExport();
        }

        /// <summary>
        /// Handles the Load event of the Page control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The EventArgs instance containing the event data.</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (!AppSession.IsAdmin)
                {
                    AssetSearchModel = ObjectCloneUtil.DeepCopy(AppSession.GetAssetSearchModelFromSession());

                    if (AssetSearchModel != null && AssetSearchModel.refAddressModel != null)
                    {
                        RefAddressModel addressModel = AssetSearchModel.refAddressModel;

                        /* If only one field value of From/To of street number(start/end) inputed, then do not use range value to search, use only one to fixed search.
                         * So, for search, do not need transfer one of From/To value as search condition.
                         * But for back to prior page, need fill the value of From/To to form.
                         */
                        if (addressModel.houseNumberStartFrom == null || addressModel.houseNumberStartTo == null)
                        {
                            AssetSearchModel.refAddressModel.houseNumberStartFrom = null;
                            AssetSearchModel.refAddressModel.houseNumberStartTo = null;
                        }

                        if (addressModel.houseNumberEndFrom == null || addressModel.houseNumberEndTo == null)
                        {
                            AssetSearchModel.refAddressModel.houseNumberEndFrom = null;
                            AssetSearchModel.refAddressModel.houseNumberEndTo = null;
                        }
                    }

                    SearchAssetList(0, null);
                }
                else
                {
                    IAssetBll assetBll = ObjectFactory.GetObject<IAssetBll>();

                    divResultNotice.Visible = true;
                    gvAssetResultList.Visible = true;
                    divAttach.Visible = true;
                    gvAssetResultList.DataSource = assetBll.BuildAssetDataTable(new AssetMasterModel[0]);
                    gvAssetResultList.DataBind();
                }
            }
            else
            {
                gvAssetResultList.DataSource = AssetDataSource;
            }
        }

        /// <summary>
        /// Handles the Click event of the Back button control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The Event Args instance containing the event data.</param>
        protected void BtnBack_Click(object sender, EventArgs e)
        {
            string url = string.Format(
                                "{0}?module={1}&isPopup=Y&isShowCriteria={2}", 
                                FileUtil.AppendApplicationRoot("/Asset/AssetSearch.aspx"), 
                                ModuleName, 
                                ACAConstant.COMMON_YES);

            Response.Redirect(url);
        }

        /// <summary>
        /// Handles the Click event of the Attach button control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The Event Args instance containing the event data.</param>
        protected void BtnAttach_Click(object sender, EventArgs e)
        {
            ClearSelectedItems();

            DataTable selectData = GetSelectedAsset();

            if (selectData == null || selectData.Rows.Count == 0)
            {
                return;
            }

            // 1. get select data from search asset
            List<AssetMasterModel> selectedAssetList = new List<AssetMasterModel>();

            foreach (DataRow dr in selectData.Rows)
            {
                selectedAssetList.Add((AssetMasterModel)dr["AssetMasterModel"]); 
            }

            // 2. merge select data to cap asset
            int failCount = 0;
            int successCount = 0;

            List<AssetMasterModel> existAssetList = CapModel.assetList.ToList();
            List<AssetMasterModel> mergedAssetList = new List<AssetMasterModel>();

            if (existAssetList.Count == 0)
            {
                mergedAssetList = selectedAssetList;
                successCount = selectedAssetList.Count();
            }
            else
            {
                mergedAssetList = existAssetList;

                foreach (AssetMasterModel assetMaster in selectedAssetList)
                {
                    if (existAssetList.Contains(assetMaster, new AssetMasterModel.Comparer()))
                    {
                        failCount += 1;
                    }
                    else
                    {
                        mergedAssetList.Add(assetMaster);
                        successCount += 1;
                    }
                }
            }

            // show messge for merged result
            if (mergedAssetList.Count > 0)
            {
                AppSession.SetAssetSearchModelToSession(null);
                CapModel.assetList = mergedAssetList.ToArray();

                string script = string.Format("CloseAssetResultDialog('{0}','{1}');", successCount, failCount);
                ScriptManager.RegisterStartupScript(Page, GetType(), "CloseDialog", script, true);
            }
        }

        /// <summary>
        /// Asset List Grid View Sort
        /// </summary>
        /// <param name="sender">An object that contains the event sender.</param>
        /// <param name="e">The Event Args object containing the event data.</param>
        protected void GvAssetResultList_GridViewSort(object sender, AccelaGridViewSortEventArgs e)
        {
            string listID = gvAssetResultList.ClientID;
            PaginationModel pageInfo = PaginationUtil.GetPageInfoByID(listID);
            pageInfo.SortExpression = e.GridViewSortExpression;

            AssetDataSource.DefaultView.Sort = e.GridViewSortExpression;
            AssetDataSource = AssetDataSource.DefaultView.ToTable();
        }

        /// <summary>
        /// Asset Result List Grid View download event method.
        /// </summary>
        /// <param name="sender">An object that contains the event sender.</param>
        /// <param name="e">The Grid View Download Event Args object containing the event data.</param>
        protected void GvAssetResultList_GridViewDownload(object sender, GridViewDownloadEventArgs e)
        {
            GridViewBuildHelper.DownloadAll(sender, e, GetAssetByQueryFormat, GetCountryByCountryCode);
        }

        /// <summary>
        /// Asset List Grid View Index Changing
        /// </summary>
        /// <param name="sender">An object that contains the event sender.</param>
        /// <param name="e">The Event Args object containing the event data.</param>
        protected void GvAssetResultList_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            string listID = gvAssetResultList.ClientID;

            PaginationModel pageInfo = PaginationUtil.GetPageInfoByID(listID);

            if (e.NewPageIndex > pageInfo.EndPage)
            {
                SearchAssetList(e.NewPageIndex, pageInfo.SortExpression);
            }
        }

        /// <summary>
        /// Asset List Grid View Row Data Bound
        /// </summary>
        /// <param name="sender">An object that contains the event sender.</param>
        /// <param name="e">A Event Args object containing the event data.</param>
        protected void GvAssetResultList_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                DataRowView row = (DataRowView)e.Row.DataItem;

                if (row != null)
                {
                    AccelaLinkButton lnkAssetID = (AccelaLinkButton)e.Row.FindControl("lnkAssetID");
                    byte[] args = Encoding.UTF8.GetBytes(string.Format(
                                                            "{1}{0}{2}{0}{3}{0}{4}{0}{5}{0}{6}", 
                                                            ACAConstant.SPLIT_CHAR,
                                                            row["g1AssetSequenceNumber"].ToString(),
                                                            row["g1AssetID"],
                                                            row["g1AssetGroup"],
                                                            row["g1AssetType"],
                                                            row["g1AssetName"],
                                                            row["g1ClassType"]));

                    string showAssetDatilJs = string.Format(
                                                            "ShowAssetDetail('{0}','{1}','{2}','{3}');return false;", 
                                                            ModuleName, 
                                                            ACAConstant.COMMON_NO, 
                                                            HttpUtility.UrlEncode(Convert.ToBase64String(args)),
                                                            lnkAssetID.ClientID);

                    lnkAssetID.OnClientClick = showAssetDatilJs;

                    AccelaLabel lblCountry = (AccelaLabel)e.Row.FindControl("lblCountry");

                    if (lblCountry != null)
                    {
                        lblCountry.Text = StandardChoiceUtil.GetCountryByKey(row["countryCode"].ToString());
                    }
                }
            }
        }

        /// <summary>
        /// Get the selected asset data source.
        /// </summary>
        /// <returns>The selected asset list</returns>
        private DataTable GetSelectedAsset()
        {
            DataTable dtSelectedItems = gvAssetResultList.GetSelectedData(AssetDataSource);

            if (dtSelectedItems == null || dtSelectedItems.Rows.Count == 0)
            {
                MessageUtil.ShowMessageByControl(Page, MessageType.Notice, LabelUtil.GetTextByKey("aca_assetresultlist_msg_selectonerecord", ModuleName).Replace("'", "\\'"));
                return null;
            }

            return dtSelectedItems;
        }

        /// <summary>
        /// Get asset by query criteria
        /// </summary>
        /// <param name="queryFormat">The query format</param>
        /// <returns>download result model that contains trade name list</returns>
        private DownloadResultModel GetAssetByQueryFormat(QueryFormat queryFormat)
        {
            PaginationModel pageInfo = new PaginationModel();

            return GetAssetByQueryFormat(queryFormat, ref pageInfo);
        }

        /// <summary>
        /// Get the asset list by asset master with the search criteria.
        /// </summary>
        /// <param name="queryFormat">the query format</param>
        /// <param name="pageInfo">pagination model</param>
        /// <returns>The download result model that contains the search table result</returns>
        private DownloadResultModel GetAssetByQueryFormat(QueryFormat queryFormat, ref PaginationModel pageInfo)
        {
            IAssetBll assetBll = ObjectFactory.GetObject<IAssetBll>();
            queryFormat.order = "G1_ASSET_ID";

            List<AssetMasterModel> assetMasterModels = assetBll.GetAssetListByAssetModel(ConfigManager.AgencyCode, AssetSearchModel, GetXAssetTypeCapTypeModel(), queryFormat);

            DataTable dt = assetBll.BuildAssetDataTable(assetMasterModels);
            DownloadResultModel model = new DownloadResultModel();

            model.StartDBRow = pageInfo.StartDBRow;
            model.DataSource = dt;

            return model;
        }

        /// <summary>
        /// Get Country By Country Code
        /// </summary>
        /// <param name="dataRow">Data row object</param>
        /// <param name="visibleColumns">The list of visible columns in grid view</param>
        /// <returns>The dictionary for country name</returns>
        private Dictionary<string, string> GetCountryByCountryCode(DataRow dataRow, List<string> visibleColumns)
        {
            if (!visibleColumns.Contains("countryCode"))
            {
                return null;
            }

            Dictionary<string, string> result = new Dictionary<string, string>();

            string countryCode = dataRow["countryCode"] as string;
            string country = StandardChoiceUtil.GetCountryByKey(countryCode);

            result.Add("countryCode", country);

            return result;
        }

        /// <summary>
        /// Clears the selected items.
        /// </summary>
        private void ClearSelectedItems()
        {
            gvAssetResultList.IsClearSelectedItems = true;
        }

        /// <summary>
        /// initial the grid view export link visible. 
        /// </summary>
        private void InitalExport()
        {
            if (!AppSession.IsAdmin && StandardChoiceUtil.IsEnableExport2CSV())
            {
                gvAssetResultList.ShowExportLink = true;
                gvAssetResultList.ExportFileName = EXPORT_FILE_NAME;
            }
            else
            {
                gvAssetResultList.ShowExportLink = false;
            }
        }

        /// <summary>
        /// search asset list by click search button in asset section.
        /// </summary>
        /// <param name="currentPageIndex">page index</param>
        /// <param name="sortExpression">sort expression</param>
        private void SearchAssetList(int currentPageIndex, string sortExpression)
        {
            try
            {
                AssetDataSource = GetAssetDataSource(currentPageIndex, sortExpression);
                BindAssetDataSource();
            }
            catch (ACAException ex)
            {
                MessageUtil.ShowMessageByControl(Page, MessageType.Error, ex.Message);
                gvAssetResultList.Visible = false;
                divResultNotice.Visible = false;
            }
        }

        /// <summary>
        /// Bind search asset data source 
        /// </summary>
        private void BindAssetDataSource()
        {
            //if more than one result is returned display them in the list
            gvAssetResultList.DataSource = AssetDataSource;
            gvAssetResultList.EmptyDataText = AppSession.IsAdmin ? string.Empty : GetTextByKey("aca_assetresultlist_msg_noasset");
            gvAssetResultList.DataBind();

            lblResultNotice.Text = string.Format(GetTextByKey("aca_assetresult_label_countmessage"), gvAssetResultList.CountSummary);
            divResultNotice.Visible = true;
            divAttach.Visible = true;
            btnAttach.Enabled = true;
            gvAssetResultList.Visible = true;

            if ((AssetDataSource == null || AssetDataSource.Rows.Count == 0) && !AppSession.IsAdmin)
            {
                btnAttach.Enabled = false;
            }
        }

        /// <summary>
        /// Get asset by search form
        /// </summary>
        /// <param name="currentPageIndex">current page index</param>
        /// <param name="sortExpression">sort expression</param>
        /// <returns>Data source of asset master</returns>
        private DataTable GetAssetDataSource(int currentPageIndex, string sortExpression)
        {
            PaginationModel pageInfo = PaginationUtil.GetPageInfoByID(gvAssetResultList.ClientID);

            pageInfo.SortExpression = sortExpression;
            pageInfo.CurrentPageIndex = currentPageIndex;
            pageInfo.CustomPageSize = gvAssetResultList.PageSize;

            QueryFormat queryFormat = PaginationUtil.GetQueryFormatModel(pageInfo);
            queryFormat.order = "G1_ASSET_ID";

            IAssetBll assetBll = ObjectFactory.GetObject<IAssetBll>();
            List<AssetMasterModel> assetMasterModels = assetBll.GetAssetListByAssetModel(ConfigManager.AgencyCode, AssetSearchModel, GetXAssetTypeCapTypeModel(), queryFormat);

            if (assetMasterModels != null && assetMasterModels.Count > 0)
            {
                DataTable dtAssetDataSource = assetBll.BuildAssetDataTable(assetMasterModels);
                return PaginationUtil.MergeDataSource(AssetDataSource, dtAssetDataSource, pageInfo);
            }

            return assetBll.BuildAssetDataTable(null);
        }

        /// <summary>
        /// Gets the x asset type cap type model.
        /// </summary>
        /// <returns>The X Asset Type Cap Type Model</returns>
        private XAssetTypeCapTypeModel GetXAssetTypeCapTypeModel()
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

            return xAssetTypeCapType;
        }
    }
}