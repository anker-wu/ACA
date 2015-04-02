#region Header

/**
 *  Accela Citizen Access
 *  File: AssetSearch.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2013-2014
 *
 *  Description:
 *  
 *
 *  Notes:
 * $Id: AssetSearch.cs 134054 2013-10-16 18:03:54Z ACHIEVO\eric.he $.
 *  Revision History
 *  Date,                  Who,                 What
 */

#endregion Header

using System;
using System.Collections.Generic;
using System.Web.UI;
using Accela.ACA.BLL.Asset;
using Accela.ACA.Common;
using Accela.ACA.Common.Common;
using Accela.ACA.Common.Util;
using Accela.ACA.Web.Common;
using Accela.ACA.Web.Common.Control;
using Accela.ACA.Web.FormDesigner;
using Accela.ACA.WSProxy;

namespace Accela.ACA.Web.Component
{
    /// <summary>
    /// Asset search class
    /// </summary>
    public partial class AssetSearch : FormDesignerBaseControl
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the AssetSearch class.
        /// </summary>
        public AssetSearch()
            : base(GviewID.SearchForAsset)
        {
        }

        #endregion Constructors

        /// <summary>
        /// Gets the ClientID for support form designer
        /// </summary>
        public string PrefixForDesigner
        {
            get
            {
                return txtAssetId.ClientID.Replace("txtAssetId", string.Empty);
            }
        }

        /// <summary>
        /// Gets or sets the asset type cap type models
        /// </summary>
        public List<XAssetTypeCapTypeModel> AssetTypeCapTypeModels
        {
            get
            {
                return ViewState["AssetTypeCapTypeModels"] as List<XAssetTypeCapTypeModel>;
            }

            set
            {
                ViewState["AssetTypeCapTypeModels"] = value;
            }
        }

        /// <summary>
        /// Gets or sets Permission Model
        /// </summary>
        protected override GFilterScreenPermissionModel4WS Permission
        {
            get
            {
                base.Permission = new GFilterScreenPermissionModel4WS();
                base.Permission.permissionLevel = GViewConstant.PERMISSION_ASSETS;
                base.Permission.permissionValue = string.Empty;
                return base.Permission;
            }

            set
            {
                base.Permission = value;
            }
        }

        /// <summary>
        /// Raises the event.
        /// </summary>
        /// <param name="e">An object that contains the event data.</param>
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            if (!IsPostBack)
            {
                List<XAssetTypeCapTypeModel> assetTypeCapTypeModels = GetAssetGroupTypeListByCapType();

                if (assetTypeCapTypeModels != null && assetTypeCapTypeModels.Count > 0)
                {
                    DropDownListBindUtil.BindAssetGroup(ddlAssetGroup, assetTypeCapTypeModels);
                }
                else
                {
                    DropDownListBindUtil.BindAssetGroup(ddlAssetGroup);
                }

                DropDownListBindUtil.BindStreetSuffix(ddlStreetType);
                DropDownListBindUtil.BindStreetDirection(ddlStreetSuffix);
                DropDownListBindUtil.BindAssetClassType(ddlClassType);
                DropDownListBindUtil.BindDDL(null, ddlAssetType);

                ddlCountryRegion.BindItems();
            }

            DialogUtil.RegisterScriptForDialog(Page);
            ControlBuildHelper.AddValidationForStandardFields(GviewID.SearchForAsset, ModuleName, Permission, Controls);
        }

        /// <summary>
        /// Raises the event.
        /// </summary>
        /// <param name="e">An object that contains the event data.</param>
        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);
            InitFormDesignerPlaceHolder(phContent);
        }

        /// <summary>
        /// Handles the Load event of the Page control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The instance containing the event data.</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                bool isShowCriteria = ValidationUtil.IsYes(Request.QueryString[UrlConstant.ASSET_ISSHOW_CRITERIA]);
                
                if (isShowCriteria)
                {
                    ShowAssetSearchModelToUI();
                }
                else
                {
                    AppSession.SetAssetSearchModelToSession(null);
                }
            }
        }

        /// <summary>
        /// Handles the Click event of the Search button control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The instance containing the event data.</param>
        protected void BtnSearch_Click(object sender, EventArgs e)
        {
            if (IsAllStandardFieldsEmpty())
            {
                ShowSearchCriteriaRequiredMessage(GetTextByKey("aca_assetsearch_msg_searchcriteriarequired"));
                return;
            }

            SetAssetSearchModel();
            string url = FileUtil.AppendApplicationRoot("/Asset/AssetResult.aspx") + "?module=" + ModuleName + "&isPopup=Y";
            Response.Redirect(url);
        }

        /// <summary>
        /// Handles the Click event of the Clear button control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The "EventArgs" instance containing the event data.</param>
        protected void BtnClear_Click(object sender, EventArgs e)
        {
            txtAssetId.Text = string.Empty;
            ddlAssetType.Text = string.Empty;
            ddlClassType.Text = string.Empty;
            ddlAssetGroup.Text = string.Empty;
            txtAssetName.Text = string.Empty;

            txtCity.Text = string.Empty;
            txtState.Text = string.Empty;
            txtNeighborhood.Text = string.Empty;
            txtStreetStart.TextFrom = string.Empty;
            txtStreetStart.TextTo = string.Empty;
            txtStreetEnd.TextFrom = string.Empty;
            txtStreetEnd.TextTo = string.Empty;
            txtStreetName.Text = string.Empty;
            txtCounty.Text = string.Empty;
            ddlCountryRegion.Text = string.Empty;
            txtPrefix.Text = string.Empty;
            txtZipCode.Text = string.Empty;
            ddlStreetType.Text = string.Empty;
            ddlStreetSuffix.Text = string.Empty;

            AppSession.SetAssetSearchModelToSession(null);
            ScriptManager.RegisterClientScriptBlock(Page, GetType(), "HideMessage", "hideMessage();", true);
        }

        /// <summary>
        /// Handles the Index Changed event of the Asset Group drop down list control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The Event Args instance containing the event data.</param>
        protected void DdlAssetGroup_IndexChanged(object sender, EventArgs e)
        {
            ddlAssetType.Items.Clear();
            AssetTypeCapTypeModels = GetAssetGroupTypeListByCapType();

            if (!string.IsNullOrEmpty(ddlAssetGroup.SelectedValue) && (AssetTypeCapTypeModels == null || AssetTypeCapTypeModels.Count == 0))
            {
                DropDownListBindUtil.BindAssetTypeByAssetGroup(ddlAssetType, ddlAssetGroup.SelectedValue);
            }
            else if (!string.IsNullOrEmpty(ddlAssetGroup.SelectedValue) && (AssetTypeCapTypeModels != null && AssetTypeCapTypeModels.Count > 0))
            {
                DropDownListBindUtil.BindAssetTypeByAssetGroup(ddlAssetType, ddlAssetGroup.SelectedValue, AssetTypeCapTypeModels);
            }
            else
            {
                DropDownListBindUtil.BindDDL(null, ddlAssetType);
            }

            Page.FocusElement(ddlAssetGroup.ClientID);
        }
        
        /// <summary>
        /// Shows the asset search model to UI.
        /// </summary>
        private void ShowAssetSearchModelToUI()
        {
            // construct a asset model to search Asset.
            AssetMasterModel searchModel = AppSession.GetAssetSearchModelFromSession();
            AssetTypeCapTypeModels = GetAssetGroupTypeListByCapType();

            if (searchModel != null)
            {
                txtAssetId.Text = searchModel.g1AssetID;
                ddlAssetGroup.SelectedValue = searchModel.g1AssetGroup;

                if (!string.IsNullOrEmpty(searchModel.g1AssetGroup))
                {
                    ddlAssetType.Items.Clear();

                    if (!string.IsNullOrEmpty(ddlAssetGroup.SelectedValue) && (AssetTypeCapTypeModels == null || AssetTypeCapTypeModels.Count == 0))
                    {
                        DropDownListBindUtil.BindAssetTypeByAssetGroup(ddlAssetType, searchModel.g1AssetGroup);
                    }
                    else if (!string.IsNullOrEmpty(ddlAssetGroup.SelectedValue) && (AssetTypeCapTypeModels != null && AssetTypeCapTypeModels.Count > 0))
                    {
                        DropDownListBindUtil.BindAssetTypeByAssetGroup(ddlAssetType, searchModel.g1AssetGroup, AssetTypeCapTypeModels);
                    }
                }

                ddlAssetType.SelectedValue = searchModel.g1AssetType;
                ddlClassType.SelectedValue = searchModel.g1ClassType;
                txtAssetName.Text = searchModel.g1AssetName;

                if (searchModel.refAddressModel != null)
                {
                    txtNeighborhood.Text = searchModel.refAddressModel.neighborhood;
                    txtCity.Text = searchModel.refAddressModel.city;
                    txtState.Text = searchModel.refAddressModel.state;
                    txtCounty.Text = searchModel.refAddressModel.county;
                    ddlCountryRegion.SelectedValue = searchModel.refAddressModel.countryCode;
                    ddlStreetType.SelectedValue = searchModel.refAddressModel.streetSuffix;
                    txtStreetName.Text = searchModel.refAddressModel.streetName;
                    ddlStreetSuffix.SelectedValue = searchModel.refAddressModel.streetSuffixdirection;
                    txtZipCode.Text = searchModel.refAddressModel.zip;
                    txtPrefix.Text = searchModel.refAddressModel.streetPrefix;
                    txtStreetStart.TextFrom = searchModel.refAddressModel.houseNumberStartFrom.ToString();
                    txtStreetStart.TextTo = searchModel.refAddressModel.houseNumberStartTo.ToString();
                    txtStreetEnd.TextFrom = searchModel.refAddressModel.houseNumberEndFrom.ToString();
                    txtStreetEnd.TextTo = searchModel.refAddressModel.houseNumberEndTo.ToString();
                }
            }
        }
        
        /// <summary>
        /// Show Error message and scroll to this message.
        /// </summary>
        /// <param name="message">the error message.</param>
        private void ShowSearchCriteriaRequiredMessage(string message)
        {
            MessageUtil.ShowMessageByControl(Page, MessageType.Error, message);
            ScriptManager.RegisterClientScriptBlock(Page, GetType(), "GotoMessageBar", "scrollIntoView('messageSpan');", true);
        }

        /// <summary>
        /// Check standard fields whether all fields is empty or not.
        /// </summary>
        /// <returns>If fill one or more then return false.</returns>
        private bool IsAllStandardFieldsEmpty()
        {
            bool result = string.IsNullOrEmpty(txtAssetId.Text.Trim())
                          && string.IsNullOrEmpty(ControlUtil.GetControlValue(ddlAssetGroup))
                          && string.IsNullOrEmpty(ControlUtil.GetControlValue(ddlAssetType))
                          && string.IsNullOrEmpty(ControlUtil.GetControlValue(ddlClassType))
                          && string.IsNullOrEmpty(txtAssetName.Text.Trim())
                          && string.IsNullOrEmpty(txtStreetName.Text.Trim())
                          && string.IsNullOrEmpty(txtCity.Text.Trim())
                          && string.IsNullOrEmpty(txtState.Text.Trim())
                          && string.IsNullOrEmpty(txtNeighborhood.Text.Trim())
                          && string.IsNullOrEmpty(txtPrefix.Text.Trim())
                          && string.IsNullOrEmpty(txtStreetStart.GetValue())
                          && string.IsNullOrEmpty(txtStreetEnd.GetValue())
                          && string.IsNullOrEmpty(txtCounty.Text.Trim())
                          && string.IsNullOrEmpty(ControlUtil.GetControlValue(ddlCountryRegion))
                          && string.IsNullOrEmpty(ControlUtil.GetControlValue(ddlStreetType))
                          && string.IsNullOrEmpty(ControlUtil.GetControlValue(ddlStreetSuffix))
                          && string.IsNullOrEmpty(txtZipCode.Text.Trim());

            return result;
        }

        /// <summary>
        /// Sets the asset search model.
        /// </summary>
        private void SetAssetSearchModel()
        {
            // construct a asset model to search Asset.
            AssetMasterModel searchModel = new AssetMasterModel();

            searchModel.g1AssetID = txtAssetId.Text.Trim();
            searchModel.g1AssetGroup = ddlAssetGroup.SelectedValue.Trim();
            searchModel.g1AssetType = ddlAssetType.SelectedValue.Trim();
            searchModel.g1ClassType = ddlClassType.SelectedValue.Trim();
            searchModel.g1AssetName = txtAssetName.Text.Trim();

            searchModel.refAddressModel = new RefAddressModel();
            searchModel.refAddressModel.neighborhood = txtNeighborhood.Text.Trim();
            searchModel.refAddressModel.city = txtCity.Text.Trim();
            searchModel.refAddressModel.state = txtState.Text.Trim();
            searchModel.refAddressModel.county = txtCounty.Text.Trim();
            searchModel.refAddressModel.countryCode = ddlCountryRegion.SelectedValue;
            searchModel.refAddressModel.streetSuffix = ddlStreetType.SelectedValue;
            searchModel.refAddressModel.streetName = txtStreetName.Text.Trim();
            searchModel.refAddressModel.streetSuffixdirection = ddlStreetSuffix.SelectedValue;
            searchModel.refAddressModel.zip = txtZipCode.Text.Trim();
            searchModel.refAddressModel.streetPrefix = txtPrefix.Text.Trim();

            /*If only one parameter of the street number start range(from or to) inputed, than do not use range search.
             * But use only one parameter to fix search. 
             * And search with the street number end is same with street number start.
             */
            Range<int?> range = GetRangeValue(txtStreetStart.TextFrom, txtStreetStart.TextTo);
            searchModel.refAddressModel.houseNumberStart = range.SingleValue;
            searchModel.refAddressModel.houseNumberStartFrom = range.LowerBound;
            searchModel.refAddressModel.houseNumberStartTo = range.UpperBound;

            range = GetRangeValue(txtStreetEnd.TextFrom, txtStreetEnd.TextTo);
            searchModel.refAddressModel.houseNumberEnd = range.SingleValue;
            searchModel.refAddressModel.houseNumberEndFrom = range.LowerBound;
            searchModel.refAddressModel.houseNumberEndTo = range.UpperBound;

            AppSession.SetAssetSearchModelToSession(searchModel);
        }

        /// <summary>
        /// Get asset group by cap type
        /// </summary>
        /// <returns>List of asset type cap type model</returns>
        private List<XAssetTypeCapTypeModel> GetAssetGroupTypeListByCapType()
        {
            CapModel4WS capModel = AppSession.GetCapModelFromSession(ModuleName);
            List<XAssetTypeCapTypeModel> assetTypeCapTypeModels = new List<XAssetTypeCapTypeModel>();

            if (capModel != null && capModel.capType != null)
            {
                XAssetTypeCapTypeModel capType = new XAssetTypeCapTypeModel();

                capType.group = capModel.capType.group;
                capType.subType = capModel.capType.subType;
                capType.type = capModel.capType.type;
                capType.category = capModel.capType.category;

                IAssetBll assetBll = ObjectFactory.GetObject<IAssetBll>();

                assetTypeCapTypeModels = assetBll.GetAssetGroupTypeListByCapType(ConfigManager.AgencyCode, capType);
            }

            return assetTypeCapTypeModels;
        }

        /// <summary>
        /// Get the range value.
        /// </summary>
        /// <param name="from">The from value.</param>
        /// <param name="to">The to value.</param>
        /// <returns>The range value.</returns>
        private Range<int?> GetRangeValue(string from, string to)
        {
            Range<int?> result = new Range<int?>();

            if (!string.IsNullOrEmpty(from) && !string.IsNullOrEmpty(to))
            {
                result.LowerBound = StringUtil.ToInt(from.Trim());
                result.UpperBound = StringUtil.ToInt(to.Trim());
            }
            else if (!string.IsNullOrEmpty(from))
            {
                result.SingleValue = StringUtil.ToInt(from.Trim());
                result.LowerBound = StringUtil.ToInt(from.Trim());
            }
            else if (!string.IsNullOrEmpty(to))
            {
                result.SingleValue = StringUtil.ToInt(to.Trim());
                result.UpperBound = StringUtil.ToInt(to.Trim());
            }

            return result;
        }
    }
}