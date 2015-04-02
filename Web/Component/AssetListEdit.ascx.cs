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
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI.WebControls;
using Accela.ACA.Common;
using Accela.ACA.Web.Common;
using Accela.ACA.Web.Common.Control;
using Accela.ACA.WSProxy;
using Accela.Web.Controls;

namespace Accela.ACA.Web.Component
{
    /// <summary>
    /// Asset list class
    /// </summary>
    public partial class AssetListEdit : BaseUserControl
    {
        #region Properties

        /// <summary>
        /// The command remove asset
        /// </summary>
        public const string COMMAND_REMOVE_ASSET = "COMMAND_REMOVE_ASSET";

        /// <summary>
        /// Gets or sets the confirm or detail page.
        /// </summary>
        /// <value>
        /// The confirm or detail page.
        /// </value>
        public string ConfirmOrDetailPage
        {
            get
            {
                return ViewState["IsInConfirmOrDetailPage"] as string;
            }

            set
            {
                ViewState["IsInConfirmOrDetailPage"] = value;
            }
        }

        /// <summary>
        /// Gets or sets the hash table user controls.
        /// </summary>
        /// <value>
        /// The hash table user controls.
        /// </value>
        public Hashtable HtUserControls
        {
            get 
            { 
                return ViewState["HtUserControls"] as Hashtable;
            }

            set
            {
                ViewState["HtUserControls"] = value;
            }
        }

        /// <summary>
        /// Gets or sets the asset master list.
        /// </summary>
        /// <value>
        /// The asset master list.
        /// </value>
        public List<AssetMasterModel> AssetMasterList
        {
            get
            {
                if (CapModel.assetList == null)
                {
                    return null;
                }

                return CapModel.assetList.ToList();
            }

            set
            {
                CapModel.assetList = value != null ? value.ToArray() : new List<AssetMasterModel>().ToArray();
                AppSession.SetCapModelToSession(ModuleName, CapModel);
            }
        }

        /// <summary>
        /// Gets the cap model.
        /// </summary>
        private CapModel4WS CapModel
        {
            get
            {
                return AppSession.GetCapModelFromSession(ModuleName);
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// set the section required property
        /// </summary>
        /// <param name="isRequired">indicate if the section is required</param>
        public void SetSectionRequired(bool isRequired)
        {
            gvAssetList.IsRequired = isRequired;
        }

        /// <summary>
        /// Displays the specified asset master models.
        /// </summary>
        /// <param name="assetMasterModels">The asset master models.</param>
        public void Display(List<AssetMasterModel> assetMasterModels)
        {
            if (!AppSession.IsAdmin)
            {
                if (assetMasterModels != null)
                {
                    gvAssetList.DataSource = assetMasterModels;
                    gvAssetList.EmptyDataText = AppSession.IsAdmin ? string.Empty : GetTextByKey("aca_assetlistedit_msg_noasset");
                    gvAssetList.DataBind();
                    panAssetList.Update();
                }
                else
                {
                    BindAssetList();
                }
            }
        }

        /// <summary>
        /// Binds the asset list.
        /// </summary>
        public void BindAssetList()
        {
            if (AssetMasterList == null)
            {
                AssetMasterList = new List<AssetMasterModel>();
            }

            divAssetList.Visible = true;
            gvAssetList.Visible = true;
            gvAssetList.DataSource = AssetMasterList;
            gvAssetList.EmptyDataText = AppSession.IsAdmin ? string.Empty : GetTextByKey("aca_assetlistedit_msg_noasset");
            gvAssetList.DataBind();
            panAssetList.Update();
        }

        /// <summary>
        /// On initial event
        /// </summary>
        /// <param name="e">The event arguments</param>
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            DialogUtil.RegisterScriptForDialog(Page);
            mapAsset.Visible = StandardChoiceUtil.IsShowMap4SelectObject(ModuleName);
        }

        /// <summary>
        /// Handles the Load event of the Page control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The Event Args instance containing the event data.</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                bool isConfirmPage = ACAConstant.ASSETLIST_LAYOUT_CAP_CONFIRM.Equals(ConfirmOrDetailPage, StringComparison.InvariantCultureIgnoreCase);
                bool isDetailPage = ACAConstant.ASSETLIST_LAYOUT_CAP_DETAIL.Equals(ConfirmOrDetailPage, StringComparison.InvariantCultureIgnoreCase);

                if (isDetailPage)
                {
                    gvAssetList.GridViewNumber = GviewID.AssetCapDetail;
                }

                DialogUtil.RegisterScriptForDialog(Page);

                // SetHiddenColumn function should be execute before gridview data binding. 
                if ((isConfirmPage || isDetailPage) && !AppSession.IsAdmin)
                {
                    GridViewBuildHelper.SetHiddenColumn(gvAssetList, new[] { "Action" });
                    btnLookUp.Visible = false;
                    mapPanel.Visible = false;
                }

                GridViewBuildHelper.SetSimpleViewElements(gvAssetList, ModuleName, AppSession.IsAdmin);

                if (!AppSession.IsAdmin)
                {
                    btnLookUp.OnClientClick = "SetNotAskForSPEAR();ShowSearchFormDialog(this,'" + ModuleName + "')";
                    BindAssetList();
                }
                else
                {
                    mapPanel.Visible = string.IsNullOrEmpty(ConfirmOrDetailPage);
                    lblAttachAssetSuccess.Visible = true;
                    lblAttachAssetExisted.Visible = true;
                    lblAttachNoAssociated.Visible = true;
                    lblRemoveAssetSuccess.Visible = true;

                    bool isConfirmOrDetailPage = !string.IsNullOrEmpty(ConfirmOrDetailPage);
                    divActionNotice.Visible = !isConfirmOrDetailPage;
                    btnLookUp.Visible = !isConfirmOrDetailPage;

                    divAssetList.Visible = true;
                    gvAssetList.Visible = true;
                    gvAssetList.DataSource = new List<AssetMasterModel>();
                    divAssetList.DataBind();
                }
            }
        }

        #endregion

        #region GridView Event

        /// <summary>
        /// Handles the RemoveCommand event of the AssetList control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The EventArgs instance containing the event data.</param>
        protected void LnkRemove_Click(object sender, EventArgs e)
        {
            AccelaLinkButton lnkRemove = (AccelaLinkButton)sender;

            if (lnkRemove.CommandName.Equals(COMMAND_REMOVE_ASSET) && lnkRemove.CommandArgument != null)
            {
                string[] ids = lnkRemove.CommandArgument.Split(ACAConstant.SPLIT_CHAR);

                if (ids != null && ids.Length == 2)
                {
                    string assetId = ids[0];
                    string assetSeqNum = ids[1];
                    RemoveAsset(assetId, assetSeqNum);
                }
            }
        }

        /// <summary>
        /// Handles the RowDataBound event of the Asset List Grid View control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The GridViewRowEventArgs instance containing the event data.</param>
        protected void GvAssetList_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                AssetMasterModel assetMasterModel = (AssetMasterModel)e.Row.DataItem;
                AccelaLinkButton lnkAssetID = (AccelaLinkButton)e.Row.FindControl("lnkAssetID");
                LinkButton lnkRemove = (LinkButton)e.Row.FindControl("lnkRemove");

                lnkRemove.Text = GetTextByKey("aca_assetlist_label_actionremove");
                lnkRemove.Visible = true;
                lnkRemove.OnClientClick = string.Format("return RemoveAsset('{0}');", lnkRemove.UniqueID);

                bool isConfirmOrDetailPage = 
                    ACAConstant.ASSETLIST_LAYOUT_CAP_CONFIRM.Equals(ConfirmOrDetailPage, StringComparison.InvariantCultureIgnoreCase) 
                    || ACAConstant.ASSETLIST_LAYOUT_CAP_DETAIL.Equals(ConfirmOrDetailPage, StringComparison.InvariantCultureIgnoreCase);

                byte[] args = Encoding.UTF8.GetBytes(string.Format(
                                                        "{1}{0}{2}{0}{3}{0}{4}{0}{5}{0}{6}", 
                                                        ACAConstant.SPLIT_CHAR, 
                                                        assetMasterModel.g1AssetSequenceNumber.ToString(),
                                                        assetMasterModel.g1AssetID,
                                                        assetMasterModel.g1AssetGroup,
                                                        assetMasterModel.g1AssetType,
                                                        assetMasterModel.g1AssetName,
                                                        assetMasterModel.g1ClassType));

                string showAssetDatilJs = string.Format(
                                                        "ShowAssetInfo('{0}','{1}','{2}','{3}');return false;", 
                                                        ModuleName, 
                                                        isConfirmOrDetailPage ? ACAConstant.COMMON_NO : ACAConstant.COMMON_YES, 
                                                        HttpUtility.UrlEncode(Convert.ToBase64String(args)),
                                                        lnkAssetID.ClientID);

                lnkAssetID.OnClientClick = showAssetDatilJs;

                AccelaLabel lblCountry = (AccelaLabel)e.Row.FindControl("lblCountry");

                if (lblCountry != null)
                {
                    lblCountry.Text = StandardChoiceUtil.GetCountryByKey(assetMasterModel.countryCode);
                }
            }
        }

        /// <summary>
        /// Handles the Grid View Sort event of the Asset List Grid View control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The Grid View Sort Event Args instance containing the event data.</param>
        protected void GvAssetList_GridViewSort(object sender, AccelaGridViewSortEventArgs e)
        {
            PaginationModel pageInfo = PaginationUtil.GetPageInfoByID(gvAssetList.ClientID);
            pageInfo.SortExpression = e.GridViewSortExpression;
        }

        /// <summary>
        /// Handles the Page Index Changing event of the Asset List Grid View control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The Grid View Page Event Args instance containing the event data.</param>
        protected void GvAssetList_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            PaginationModel pageInfo = PaginationUtil.GetPageInfoByID(gvAssetList.ClientID);

            if (e.NewPageIndex >= pageInfo.EndPage)
            {
                BindAssetList();
            }
        }

        /// <summary>
        /// Handles the Click event of the Refresh Asset List button control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The Event Args instance containing the event data.</param>
        protected void BtnRefreshAssetList_Click(object sender, EventArgs e)
        {
            if (Request.Form["__EVENTTARGET"] == btnRefreshAssetList.UniqueID)
            {
                string resultCount = Request.Form["__EVENTARGUMENT"];
                RefreshAssetList(resultCount);
            }
        }

        #endregion

        /// <summary>
        /// Refresh the asset list.
        /// </summary>
        /// <param name="resultCount"> attach result count</param>
        private void RefreshAssetList(string resultCount)
        {
            if (AppSession.IsAdmin)
            {
                return;
            }

            if (AssetMasterList != null)
            {
                if (!string.IsNullOrEmpty(resultCount))
                {
                    string[] count = resultCount.Split(ACAConstant.SPLIT_CHAR4URL1);

                    if (count.Length == 2)
                    {
                        int successCount = 0;
                        int failedCount = 0;

                        int.TryParse(count[0], out successCount);
                        int.TryParse(count[1], out failedCount);

                        ShowAttchResultMessage(successCount, failedCount);
                    }

                    gvAssetList.DataSource = AssetMasterList;
                    gvAssetList.EmptyDataText = AppSession.IsAdmin ? string.Empty : GetTextByKey("aca_assetlistedit_msg_noasset");
                    gvAssetList.DataBind();
                    panAssetList.Update();
                }
            }
            else
            {
                BindAssetList();
            }
        }

        /// <summary>
        /// Removes the asset.
        /// </summary>
        /// <param name="assetId">The asset identifier.</param>
        /// <param name="assetSeqNum">The asset sequence number.</param>
        private void RemoveAsset(string assetId, string assetSeqNum)
        {
            List<AssetMasterModel> newAssetDataList = new List<AssetMasterModel>();

            foreach (var model in AssetMasterList)
            {
                if (model.g1AssetID == assetId && model.g1AssetSequenceNumber.ToString() == assetSeqNum)
                {
                    continue;
                }

                newAssetDataList.Add(model);
            }

            divActionNotice.Visible = true;
            divImgSuccess.Visible = true;
            lblAttachAssetSuccess.Visible = false;
            lblRemoveAssetSuccess.Visible = true;

            AssetMasterList = newAssetDataList;
            gvAssetList.DataSource = AssetMasterList;
            gvAssetList.DataBind();
            divAssetList.Visible = true;
        }

        #region show message function

        /// <summary>
        /// Shows the success message.
        /// </summary>
        /// <param name="successCount">The success count.</param>
        private void ShowSuccessMessage(int successCount)
        {
            string messageSuccess = string.Format(LabelUtil.GetGlobalTextByKey("aca_assetresult_label_attachsuccess").Replace("'", "\\'"), successCount);
            divImgSuccess.Visible = true;
            lblAttachAssetSuccess.Visible = true;
            lblAttachAssetSuccess.Text = messageSuccess;

            divActionNotice.Visible = true;
        }

        /// <summary>
        /// Shows the existed message.
        /// </summary>
        /// <param name="existedCount">The existed count.</param>
        private void ShowExistedMessage(int existedCount)
        {
            string messageExisted = string.Format(LabelUtil.GetGlobalTextByKey("aca_assetresult_label_attachexisted").Replace("'", "\\'"), existedCount);
            lblAttachAssetExisted.Visible = true;
            lblAttachAssetExisted.Text = messageExisted;

            divActionNotice.Visible = true;
        }

        /// <summary>
        /// Shows the no associated message.
        /// </summary>
        /// <param name="noAssociatedCount">The no associated count.</param>
        private void ShowNoAssociatedMessage(int noAssociatedCount)
        {
            string messageNoAssociated = string.Format(LabelUtil.GetGlobalTextByKey("aca_assetresult_label_attachnoassetbygis").Replace("'", "\\'"), noAssociatedCount);
            lblAttachNoAssociated.Visible = true;
            lblAttachNoAssociated.Text = messageNoAssociated;

            divActionNotice.Visible = true;
        }

        /// <summary>
        /// Shows the attach result message.
        /// </summary>
        /// <param name="successCount">The success count.</param>
        /// <param name="existedCount">The existed count.</param>
        /// <param name="noAssociatedCount">The no associated count</param>
        private void ShowAttchResultMessage(int successCount = 0, int existedCount = 0, int noAssociatedCount = 0)
        {
            if (successCount > 0)
            {
                divImgSuccess.Visible = true;
            }
            else if (existedCount > 0 || noAssociatedCount > 0)
            {
                divImgFailed.Visible = true;
            }

            if (successCount > 0)
            {
                ShowSuccessMessage(successCount);
            }

            if (existedCount > 0)
            {
                ShowExistedMessage(existedCount);
            }

            if (noAssociatedCount > 0)
            {
                ShowNoAssociatedMessage(noAssociatedCount);
            }
        }
        #endregion
    }
}