#region Header

/**
 *  Accela Citizen Access
 *  File: AssetAddressList.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2013-2014
 *
 *  Description:
 *  
 *
 *  Notes:
 * $Id: AssetAddressList.cs 134054 2013-10-16 18:03:54Z ACHIEVO\eric.he $.
 *  Revision History
 *  Date,                  Who,                 What
 */

#endregion Header

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;
using Accela.ACA.BLL.Cap;
using Accela.ACA.Common;
using Accela.ACA.Common.Util;
using Accela.ACA.Web.Common;
using Accela.ACA.Web.Common.Control;
using Accela.ACA.WSProxy;
using Accela.Web.Controls;

namespace Accela.ACA.Web.Component
{
    /// <summary>
    /// Asset address list class
    /// </summary>
    public partial class AssetAddressList : BaseUserControl
    {
        #region Properties

        /// <summary>
        /// The command get address
        /// </summary>
        public const string COMMAND_GET_ADDRESS = "COMMAND_GET_ADDRESS";

        /// <summary>
        /// Gets or sets the asset reference address list.
        /// </summary>
        /// <value>
        /// The asset reference address list.
        /// </value>
        public List<RefAddressModel> AssetRefAddressList
        {
            get
            {
                return (List<RefAddressModel>)ViewState["AssetRefAddressList"];
            }

            set
            {
                ViewState["AssetRefAddressList"] = value;
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Binds the asset address list.
        /// </summary>
        public void BindAssetAddressList()
        {
            divAssetAddressList.Visible = true;
            gvAssetAddress.Visible = true;
            gvAssetAddress.DataSource = AssetRefAddressList;
            gvAssetAddress.EmptyDataText = AppSession.IsAdmin ? string.Empty : GetTextByKey("aca_assetaddresslist_msg_noassetaddress");
            gvAssetAddress.DataBind();
        }

        /// <summary>
        /// Raises the initial event.
        /// </summary>
        /// <param name="e">An System.EventArgs object that contains the event data.</param>
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            if (!CheckContainAddress() && !AppSession.IsAdmin)
            {
                GridViewBuildHelper.SetHiddenColumn(gvAssetAddress, new[] { "Action" });
            }

            GridViewBuildHelper.SetSimpleViewElements(gvAssetAddress, ModuleName, AppSession.IsAdmin);
        }

        /// <summary>
        /// Handles the Page Index Changing event of the Asset Address grid view control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The Grid View Page Event Args instance containing the event data.</param>
        protected void GvAssetAddress_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            PaginationModel pageInfo = PaginationUtil.GetPageInfoByID(gvAssetAddress.ClientID);

            if (e.NewPageIndex >= pageInfo.EndPage)
            {
                BindAssetAddressList();
            }
        }

        /// <summary>
        /// Handles the Grid View Sort event of the Asset Address Grid View control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The Grid View Sort Event Args instance containing the event data.</param>
        protected void GvAssetAddress_GridViewSort(object sender, AccelaGridViewSortEventArgs e)
        {
            PaginationModel pageInfo = PaginationUtil.GetPageInfoByID(gvAssetAddress.ClientID);
            pageInfo.SortExpression = e.GridViewSortExpression;
        }

        /// <summary>
        /// Handles the Command event of the GetAddress control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The EventArgs instance containing the event data.</param>
        protected void LnkGetAddress_Click(object sender, EventArgs e)
        {
            AccelaLinkButton getAddressButton = (AccelaLinkButton)sender;

            if (getAddressButton.CommandArgument != null)
            {
                string id = getAddressButton.CommandArgument;

                if (id != null)
                {
                    FillAddressToCap(id);
                }
            }
        }

        /// <summary>
        /// Handles the RowDataBound event of the Asset Address Grid View control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The GridViewRowEventArgs instance containing the event data.</param>
        protected void GvAssetAddress_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                RefAddressModel addressModel = (RefAddressModel)e.Row.DataItem;
                LinkButton lnkGetAddress = (LinkButton)e.Row.FindControl("lnkGetAddress");

                lnkGetAddress.Text = GetTextByKey("aca_assetaddresslist_label_getaddress");
                lnkGetAddress.Visible = true;

                AccelaLabel lblCountry = (AccelaLabel)e.Row.FindControl("lblCountryRegion");

                if (lblCountry != null && addressModel != null)
                {
                    lblCountry.Text = StandardChoiceUtil.GetCountryByKey(addressModel.countryCode);
                }
            }
        }

        /// <summary>
        /// Fills the address to cap.
        /// </summary>
        /// <param name="refAddressId">The reference address identifier.</param>
        private void FillAddressToCap(string refAddressId)
        {
            RefAddressModel refAddressModel = new RefAddressModel();

            if (AssetRefAddressList != null && AssetRefAddressList.Count > 0)
            {
                refAddressModel = AssetRefAddressList.SingleOrDefault(o => o.refAddressId.ToString().Equals(refAddressId, StringComparison.InvariantCultureIgnoreCase));

                if (refAddressModel == null)
                {
                    return;
                }
            }

            CapModel4WS capModel = AppSession.GetCapModelFromSession(ModuleName);

            //1.1. then we should set address model to cap ref address
            if (capModel != null)
            {
                AddressModel addressModel = new AddressModel();
                addressModel = CapUtil.ConvertRefAddressModel2AddressModel(refAddressModel);

                capModel.addressModel = addressModel;
            }

            AppSession.SetCapModelToSession(ModuleName, capModel);

            MessageUtil.ShowMessageByControl(Page, MessageType.Success, LabelUtil.GetTextByKey("aca_assetlist_label_autofillsuccessful", ConfigManager.AgencyCode, ModuleName).Replace("'", "\\'"));
        }

        /// <summary>
        /// Checks the contain address.
        /// </summary>
        /// <returns>if contain address then return true else false</returns>
        private bool CheckContainAddress()
        {
            bool isShowAction = ValidationUtil.IsYes(Request.QueryString[UrlConstant.ASSET_ISSHOW_ACTION]);

            if (!isShowAction)
            {
                return false;
            }

            PageFlowGroupModel pageflowGroup = AppSession.GetPageflowGroupFromSession();
            List<ComponentModel> cptList = new List<ComponentModel>();

            foreach (StepModel step in pageflowGroup.stepList)
            {
                foreach (PageModel page in step.pageList)
                {
                    foreach (ComponentModel component in page.componentList)
                    {
                        if (component.componentName.Equals(GViewConstant.SECTION_ADDRESS, StringComparison.InvariantCultureIgnoreCase) 
                            || component.componentName.Equals(GViewConstant.SECTION_ASSETS, StringComparison.InvariantCultureIgnoreCase))  
                        {
                            cptList.Add(component);
                        }

                        if (cptList.Count == 2)
                        {
                            bool isContainSamePage = cptList[0].pageID == cptList[1].pageID;
                            bool isContainAddressAfter = cptList[0].componentID == (long)PageFlowComponent.ASSETS;

                            return isContainSamePage || isContainAddressAfter;
                        }
                    }
                }
            }

            return false;
        }

        #endregion Methods
    }
}