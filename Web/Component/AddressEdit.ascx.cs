#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: AddressEdit.ascx.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2007-2014
 *
 *  Description:
 *
 *  Notes:
 *      $Id: AddressEdit.ascx.cs 277585 2014-08-18 11:09:47Z ACHIEVO\james.shi $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.Script.Serialization;
using System.Web.UI;
using System.Web.UI.WebControls;

using Accela.ACA.BLL.AddressBuilder;
using Accela.ACA.BLL.APO;
using Accela.ACA.BLL.Common;
using Accela.ACA.Common;
using Accela.ACA.Common.Common;
using Accela.ACA.Common.Util;
using Accela.ACA.ExpressionBuild;
using Accela.ACA.Web.Common;
using Accela.ACA.Web.Common.Control;
using Accela.ACA.Web.ExpressionBuild;
using Accela.ACA.WSProxy;
using Accela.Web.Controls;
using Newtonsoft.Json;

namespace Accela.ACA.Web.Component
{
    /// <summary>
    /// Address section control
    /// </summary>
    public partial class AddressEdit : FormDesignerWithExpressionControl
    {
        #region Fields

        /// <summary>
        /// Address list's parcel column name in work location page.
        /// </summary>
        protected const string ADDRESS_LIST_PARCEL_COLUMN_NAME = "lnkParcelHeader";

        /// <summary>
        ///  Address list's owner column name in work location page.
        /// </summary>
        protected const string ADDRESS_LIST_OWNER_COLUMN_NAME = "lnkOwnerHeader";

        /// <summary>
        /// Function name for checking control value in address section
        /// </summary>
        protected const string CONTROL_VALUE_VALIDATE_FUNCTION = "AddressEdit_CheckControlValueValidate";

        /// <summary>
        /// Script name for checking value validation of address section
        /// </summary>
        protected const string TEMPLETE_CONTROL_VALUE_VALIDATE_FUNCTION = "Templete_AddressEdit_CheckControlValueValidate";

        /// <summary>
        /// Max length for address description
        /// </summary>
        private const int ADDRESSDESC_MAXLENGTH = 255;

        /// <summary>
        /// Max length for street name
        /// </summary>
        private const int STREETNAME_MAXLENGTH = 1024;

        /// <summary>
        /// Initial color of search and clear buttons
        /// </summary>
        private System.Drawing.Color _initColor; //FF doesn't support to make color gray when button is disabled.

        /// <summary>
        /// indicate the APO has lock condition or not
        /// </summary>
        private bool _isAPOLocked;

        /// <summary>
        /// indicate the the address form is editable or not.
        /// </summary>
        private bool _isEditable = true;

        /// <summary>
        /// R-Reference,need reference data
        /// T-Transactional, user input data.
        /// N-No Limitation,both reference and daily
        /// </summary>
        private string _validateFlag = string.Empty;

        /// <summary>
        /// ExpressionFactory class's instance
        /// </summary>
        private ExpressionFactory _expressionInstance;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the AddressEdit class.
        /// </summary>
        public AddressEdit()
            : base(GviewID.AddressEdit)
        {
        }

        #endregion Constructors

        #region Events

        /// <summary>
        /// AddressEditCompleted event 
        /// </summary>
        public event CommonEventHandler AddressEditCompleted;

        #endregion Events

        #region Properties

        /// <summary>
        /// Gets or sets condition data source
        /// </summary>
        public DataTable ConditionGridViewDataSource
        {
            get
            {
                if (ViewState["ConditionDataSource"] == null)
                {
                    return null;
                }

                return (DataTable)ViewState["ConditionDataSource"];
            }

            set
            {
                ViewState["ConditionDataSource"] = value;
            }
        }

        /// <summary>
        /// Gets or sets external Owner for Super Agency
        /// </summary>
        public RefOwnerModel ExternalOwnerForSuperAgency
        {
            get
            {
                return ViewState["ExternalOwnerForSuperAgency"] as RefOwnerModel;
            }

            set
            {
                ViewState["ExternalOwnerForSuperAgency"] = value;
            }
        }

        /// <summary>
        /// Gets or sets External Owner for Super Agency
        /// </summary>
        public ParcelModel ExternalParcelForSuperAgency
        {
            get
            {
                return ViewState["ExternalParcelForSuperAgency"] as ParcelModel;
            }

            set
            {
                ViewState["ExternalParcelForSuperAgency"] = value;
            }
        }

        /// <summary>
        /// Gets or sets data source
        /// </summary>
        public DataTable GridViewDataSource
        {
            get
            {
                return (DataTable)ViewState["AddressModels"];
            }

            set
            {
                ViewState["AddressModels"] = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the APO has lock condition or not
        /// </summary>
        public bool IsAPOLocked
        {
            get
            {
                return _isAPOLocked;
            }

            set
            {
                _isAPOLocked = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether edit the form.
        /// </summary>
        public bool IsEditable
        {
            get
            {
                return _isEditable;
            }

            set
            {
                _isEditable = value;
            }
        }

        /// <summary>
        /// Sets a value indicating whether the section is required.
        /// True must fill all required field, False skip validate required field
        /// </summary>
        public bool IsSectionRequired
        {
            set
            {
                if (!value)
                {
                    ControlBuildHelper.AddValidationFuctionForRequiredFields(Controls, CONTROL_VALUE_VALIDATE_FUNCTION);
                    templateEdit.ScriptName = TEMPLETE_CONTROL_VALUE_VALIDATE_FUNCTION;
                }
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the control is in work location page or not
        /// </summary>
        public bool IsWorkLocationPage
        {
            get
            {
                if (ViewState["IsWorkLocationPage"] == null)
                {
                    return false;
                }

                return (bool)ViewState["IsWorkLocationPage"];
            }

            set
            {
                ViewState["IsWorkLocationPage"] = value;
            }
        }

        /// <summary>
        /// Gets or sets focus click id.
        /// </summary>
        public string SkippingToParentClickID
        {
            get
            {
                return ViewState["SkippingToParentClickID"] as string;
            }

            set
            {
                ViewState["SkippingToParentClickID"] = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the smart choice validate for address
        /// </summary>
        public bool IsValidate
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the flag of the data comes from.
        /// </summary>
        public string ValidateFlag
        {
            get
            {
                return _validateFlag;
            }

            set
            {
                _validateFlag = value;

                if (ComponentDataSource.Reference.Equals(value))
                {
                    IsValidate = true;
                }
            }
        }

        /// <summary>
        /// Gets or sets Permission.
        /// </summary>
        protected override GFilterScreenPermissionModel4WS Permission
        {
            get
            {
                base.Permission = new GFilterScreenPermissionModel4WS();
                base.Permission.permissionLevel = "APO";
                base.Permission.permissionValue = "ADDRESS";
                return base.Permission;
            }

            set
            {
                base.Permission = value;
            }
        }

        /// <summary>
        /// Gets function name to update address and associates.
        /// </summary>
        protected string UpdateAddressAndAssociatesFunctionName
        {
            get
            {
                return ClientID + "_UpdateAddressAndAssociates";
            }
        }

        /// <summary>
        /// Gets Expression Factory Instance.
        /// </summary>
        protected override ExpressionFactory ExpressionInstance
        {
            get
            {
                if (_expressionInstance == null)
                {
                    ExpressionControls = CollectExpressionInputControls(GviewID.AddressEdit, ModuleName, templateEdit, ACAConstant.CAP_ADDRESS_TEMPLATE_FIELD_PREFIX);
                    _expressionInstance = new ExpressionFactory(ModuleName, ExpressionType.Address, ExpressionControls);
                }

                return _expressionInstance;
            }
        }

        /// <summary>
        /// Gets all control ids that needn't to be controlled
        /// </summary>
        private string[] FilteredControlIDs
        {
            get
            {
                return new string[] { "mapAddress" };
            }
        }

        /// <summary>
        /// Gets address list flag.
        /// </summary>
        private string PageInfoID
        {
            get
            {
                return IsFromMap 
                       ? (IsCreateCapFromGIS ? ACAConstant.AGIS_COMMAND_CREATE_CAP : mapAddress.ClientID)
                       : ucAddressList.ClientID;
            }
        }

        #endregion Properties

        #region Public Methods

        /// <summary>
        /// Fill address information into address form
        /// </summary>
        /// <param name="addressModel">The AddressModel model</param>
        /// <param name="isInConfirmPage">A boolean value to indicate whether the current page is the permit confirmation page</param>
        /// <param name="isNeedShowCondition">if set to <c>true</c> [is work location select].</param>
        public void DisplayAddress(AddressModel addressModel, bool isInConfirmPage, bool isNeedShowCondition)
        {
            DisplayAddress(false, addressModel, isInConfirmPage, isNeedShowCondition);
        }
        
        /// <summary>
        /// Update address fields after refresh
        /// </summary>
        public void UpdateAfterRefresh()
        {
            panAddress.Update();
        }

        /// <summary>
        /// display condition
        /// </summary>
        /// <param name="noticeConditions">notice Conditions</param>
        /// <param name="hightestCondition">highest Condition</param>
        /// <param name="conditionType">condition Type</param>
        public void DisplayCondition(NoticeConditionModel[] noticeConditions, NoticeConditionModel hightestCondition, ConditionType conditionType)
        {
            ucConditon.IsShowCondition(noticeConditions, hightestCondition, ConditionType.Address);
        }

        /// <summary>
        /// Fill address information into address form
        /// </summary>
        /// <param name="isReferenceAddress">A boolean value to indicate whether the address is a reference address</param>
        /// <param name="addressModel">The AddressModel model</param>
        /// <param name="isInConfirmPage">A boolean value to indicate whether the current page is the permit confirmation page</param>
        /// <param name="isNeedShowCondition">if set to <c>true</c> [is work location select].</param>
        public void DisplayAddress(bool isReferenceAddress, AddressModel addressModel, bool isInConfirmPage, bool isNeedShowCondition)
        {
            imgErrorIcon.Visible = false;
            ucConditon.HideCondition();

            // if address model is null indicates that user is applying for a permit at first time.
            if (addressModel == null)
            {
                ClearAddressForm();
                CreateOriginalAddressTemplate();
                DropDownListBindUtil.SetCountrySelectedValue(ddlCountry, string.Empty, true, true, false);
                IsAppliedRegional = true;

                if (!IsPostBack && !AppSession.IsAdmin)
                {
                    SetCurrentCityAndState();
                }
            }
            else
            {
                ServiceModel[] services = AppSession.GetSelectedServicesFromSession();

                // if save and resume and confirm page, don't need to show condition info
                if (isNeedShowCondition && (addressModel.refAddressId != null || !string.IsNullOrEmpty(addressModel.UID))
                    && services != null && !isInConfirmPage)
                {
                    bool isNotLocked = ShowCondition(addressModel, ExternalParcelForSuperAgency, ExternalOwnerForSuperAgency);

                    // for super agency,if one of the APO is locked,let the APO sections empty and disabled the address and parcel sections.
                    if (!isNotLocked)
                    {
                        IsAPOLocked = true;

                        CreateOriginalAddressTemplate();
                        DisableAddressFormAndButton();
                        return;
                    }
                }
                
                if (!AppSession.IsAdmin)
                {
                    txtStreetNo.Text = Convert.ToString(addressModel.houseNumberStart);
                    DropDownListBindUtil.SetSelectedValue(ddlStreetDirection, addressModel.streetDirection);
                    DropDownListBindUtil.SetSelectedValue(ddlStreetSuffix, addressModel.streetSuffix);
                    DropDownListBindUtil.SetSelectedValue(ddlUnitType, addressModel.unitType);
                    txtStreetName.Text = addressModel.streetName;
                    txtCity.Text = addressModel.city;
                    
                    txtUnitNo.Text = addressModel.unitStart;
                    txtCounty.Text = addressModel.county;
                    txtRefAddressId.Value = Convert.ToString(addressModel.refAddressId);
                    txtAddressUID.Value = addressModel.UID;

                    txtPrefix.Text = addressModel.streetPrefix;
                    txtStreetEnd.Text = Convert.ToString(addressModel.houseNumberEnd);
                    txtUnitEnd.Text = addressModel.unitEnd;
                    DropDownListBindUtil.SetCountrySelectedValue(ddlCountry, addressModel.countryCode, false, true, false);
                    IsAppliedRegional = true;

                    txtZip.Text = ModelUIFormat.FormatZipShow(addressModel.zip, addressModel.countryCode, false); //for zip show

                    txtState.Text = addressModel.state;
                    txtStartFraction.Text = addressModel.houseFractionStart;
                    txtEndFraction.Text = addressModel.houseFractionEnd;
                    DropDownListBindUtil.SetSelectedValue(ddlStreetSuffixDirection, addressModel.streetSuffixdirection);
                    txtDescription.Text = addressModel.addressDescription;
                    txtDistance.DoubleValue = addressModel.distance;
                    txtSecondaryRoad.Text = addressModel.secondaryRoad;
                    txtSecondaryRoadNo.Text = Convert.ToString(addressModel.secondaryRoadNumber);
                    txtInspectionD.Text = addressModel.inspectionDistrict;
                    txtInspectionDP.Text = addressModel.inspectionDistrictPrefix;
                    txtNeighborhoodP.Text = addressModel.neighborhoodPrefix;
                    txtNeighborhood.Text = addressModel.neighborhood;
                    txtXCoordinator.DoubleValue = addressModel.XCoordinator;
                    txtYCoordinator.DoubleValue = addressModel.YCoordinator;
                    txtStreetAddress.Text = addressModel.fullAddress;

                    txtAddressLine1.Text = addressModel.addressLine1;
                    txtAddressLine2.Text = addressModel.addressLine2;

                    txtLevelPrefix.Text = addressModel.levelPrefix;
                    txtLevelNbrStart.Text = addressModel.levelNumberStart;
                    txtLevelNbrEnd.Text = addressModel.levelNumberEnd;
                    txtHouseAlphaStart.Text = addressModel.houseNumberAlphaStart;
                    txtHouseAlphaEnd.Text = addressModel.houseNumberAlphaEnd;

                    hfDuplicateAddressKeys.Value = (addressModel.duplicatedAPOKeys == null || addressModel.duplicatedAPOKeys.Length == 0) ? string.Empty : JsonConvert.SerializeObject(addressModel.duplicatedAPOKeys);

                    if (addressModel.refAddressId != null || !string.IsNullOrEmpty(addressModel.UID))
                    {
                        //Cache the reference data.
                        RefEntityCache = addressModel;
                    }
                }

                ShowTemplateFields(isReferenceAddress, addressModel);

                if (IsValidate)
                {
                    txtZip.SetZipFromAA(addressModel.zip);

                    if ((addressModel.refAddressId != null && addressModel.refAddressId != 0)
                        || !string.IsNullOrEmpty(addressModel.UID))
                    {
                        DisableAddressForm();
                    }
                }
            }

            if (isInConfirmPage)
            {
                chkAutoFillAddressInfo.Visible = false;
                ddlAutoFillAddressInfo.Visible = false;
            }

            if (AppSession.IsAdmin)
            {
                divAutoFill.Visible = true;
            }

            // if set the property in aca admin false, address cannot be changed.
            if (!IsEditable && !AppSession.IsAdmin)
            {
                DisableAddressFormAndButton();
            }
        }

        /// <summary>
        /// Get a AddressModel
        /// </summary>
        /// <param name="addressmodel">AddressModel object</param>
        /// <returns>The AddressModel</returns>
        public AddressModel GetAddressModel(AddressModel addressmodel)
        {
            if (addressmodel == null)
            {
                addressmodel = new AddressModel();
            }

            CapModel4WS capModel = AppSession.GetCapModelFromSession(ModuleName);
            string agencyCode = capModel.capID.serviceProviderCode;
            GetAddressModelFromControl(agencyCode, ref addressmodel);
            addressmodel.templates = templateEdit.GetAttributeModels();

            if (!string.IsNullOrEmpty(txtRefAddressId.Value))
            {
                //Merge the reference data and UI data to prevent lose the data which fields hidden by form designer.
                CapUtil.MergeRefDataToUIData<AddressModel, RefAddressModel>(
                    ref addressmodel,
                    "addressList",
                    string.Empty,
                    string.Format("{0}{1}{2}{1}{3}", "sourceNumber", ACAConstant.SPLIT_CHAR, "refAddressId", "UID"),
                    string.Format("{0}{1}{2}{1}{3}", txtSourceSeq.Value, ACAConstant.SPLIT_CHAR, txtRefAddressId.Value, addressmodel.UID),
                    ModuleName,
                    RefEntityCache,
                    Permission,
                    ViewId);
            }

            ClearRefEntityCache();

            return addressmodel;
        }

        /// <summary>
        /// validate address, if validate flag in smart choice is true, then 
        /// </summary>
        /// <returns>True if the address is valid</returns>
        public bool ValidateAddress()
        {
            bool isNotValidate = false;

            if (!IsEditable
                || APOUtil.IsEmpty(GetRefAddressModel())
                || !IsValidate
                || (!string.IsNullOrEmpty(txtRefAddressId.Value) || !string.IsNullOrEmpty(txtAddressUID.Value)))
            {
                isNotValidate = true;
            }
            else
            {
                imgErrorIcon.Visible = true;
            }

            return isNotValidate;
        }

        #endregion Public Methods

        #region Protected Methods

        /// <summary>
        /// Raises the <c>System.Web.UI.Control.Init</c> event.
        /// </summary>
        /// <param name="e">An System.EventArgs object that contains the event data.</param>
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            ddlCountry.BindItems();

            if (!IsPostBack)
            {
                DropDownListBindUtil.BindStreetSuffix(ddlStreetSuffix);
                DropDownListBindUtil.BindStreetDirection(ddlStreetDirection);
                DropDownListBindUtil.BindUnitType(ddlUnitType);
                DropDownListBindUtil.BindStreetDirection(ddlStreetSuffixDirection);
                BindAutoFillItems();
                _initColor = btnSearch.ForeColor;
            }

            // if in work location page, Address Number, Address Name are required.
            if (IsWorkLocationPage)
            {               
                btnSearch.CausesValidation = true;
                btnSearch.ForeColor = _initColor;
            }

            ddlCountry.SetCountryControls(txtZip, txtState, null);

            ControlBuildHelper.AddValidationForStandardFields(GviewID.AddressEdit, ModuleName, Permission, Controls);

            mapAddress.Visible = StandardChoiceUtil.IsShowMap4SelectObject(ModuleName);
        }

        /// <summary>
        /// Raises the page load event
        /// </summary>
        /// <param name="sender">An object that contains the event sender.</param>
        /// <param name="e">A System.EventArgs object containing the event data.</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request["__EVENTTARGET"] != null && Request["__EVENTTARGET"].IndexOf("lnkAddress") > -1)
            {
                ucConditon.HideCondition();
                ScriptManager.RegisterStartupScript(Page, GetType(), "InitialHideLinkCondtion", "initialAddressConditionStatus='0';", true);
            }

            if (!IsAppliedRegional)
            {
                ControlUtil.ApplyRegionalSetting(IsPostBack, false, true, !IsPostBack, ddlCountry);
            }

            ddlAutoFillAddressInfo.Enabled = chkAutoFillAddressInfo.Checked;

            if (!AppSession.IsAdmin)
            {
                ddlAutoFillAddressInfo.Attributes.Add("onchange", "ddlAutoFillAddressChanged();");
                chkAutoFillAddressInfo.Attributes.Add("onclick", "chkAutoFillAddressChanged();");
            }

            chkAutoFillAddressInfo.Attributes.Add("title", GetTextByKey(chkAutoFillAddressInfo.LabelKey));

            if (!Page.IsPostBack)
            {
                ucConditon.IsWorkLocationPage = IsWorkLocationPage;

                if (IsWorkLocationPage)
                {
                    ucConditon.Visible = false;
                }
            }

            if (!IsPostBack)
            {
                InitUI();
            }

            if (!IsWorkLocationPage)
            {
                ucAddressList.Visible = false;
            }

            hlEnd.NextControlClientID = SkippingToParentClickID;

            phContent.TemplateControlIDPrefix = ACAConstant.CAP_ADDRESS_TEMPLATE_FIELD_PREFIX;
        }

        /// <summary>
        /// Clear address form
        /// </summary>
        /// <param name="sender">An object that contains the event sender.</param>
        /// <param name="e">An System.EventArgs object that contains the event data.</param>
        protected void ClearAddressButton_Click(object sender, EventArgs e)
        {
            AppSession.SelectedParcelInfo = null;
            ucAddressList.Visible = false; // when user click clear button,hide the address results.
            ClearAddressForm();
            ClearRefEntityCache();
            ControlUtil.ClearRegionalSetting(ddlCountry, false, ModuleName, Permission, ViewId);
            CapModel4WS capModel = AppSession.GetCapModelFromSession(ModuleName);

            if (capModel != null && capModel.addressModel != null)
            {
                /*
                 * If a cap with address data saved as a partial cap and resume back,
                 * the CapModel.addressModel.addressId will has a value, the value is daily side sequence ID.
                 * the addressId to be used in java side to determine the Create or Update logic.
                 * So we need to keep the addressId to prevent occurs the logic error in java side.
                 */
                AddressModel emptyAddress = new AddressModel();
                emptyAddress.addressId = capModel.addressModel.addressId;
                capModel.addressModel = emptyAddress;

                AppSession.SetCapModelToSession(ModuleName, capModel);
            }

            ddlAutoFillAddressInfo.Enabled = chkAutoFillAddressInfo.Checked;
            hfDuplicateAddressKeys.Value = string.Empty;
            Page.FocusElement(btnSearch.ClientID);
            ClearExpressionValue(true);
        }

        /// <summary>
        /// Raise post back button click event
        /// </summary>
        /// <param name="sender">An object that contains the event sender.</param>
        /// <param name="e">An System.EventArgs object that contains the event data.</param>
        protected void PostbackButton_Click(object sender, EventArgs e)
        {
            InitialiteFromGIS();
        }

        /// <summary>
        /// Search addresses
        /// </summary>
        /// <param name="sender">An object that contains the event sender.</param>
        /// <param name="e">An System.EventArgs object that contains the event data.</param>
        protected void SearchButton_Click(object sender, EventArgs e)
        {
            IsFromMap = false;
            IsCreateCapFromGIS = false;
            imgErrorIcon.Visible = false;
            ucConditon.HideCondition();

            if (IsWorkLocationPage)
            {
                GridViewDataSource = null;
                Page.FocusElement(btnSearch.ClientID);

                LoadAddressList(0, null);
            }
            else
            {
                RefAddressModel searchCriterias = GetRefAddressModel();

                // Check empty input on external source.
                if (StandardChoiceUtil.IsExternalAddressSource() && APOUtil.IsEmpty(searchCriterias))
                {
                    MessageUtil.ShowMessageByControl(Page, MessageType.Error, GetTextByKey("aca_apo_msg_searchcriteria_required"));
                    return;
                }

                // Save APO session parameter
                APOSessionParameterModel sessionParameter = GetAPOSessionParameter();
                sessionParameter.SearchCriterias = searchCriterias;
                AppSession.SetAPOSessionParameter(sessionParameter);

                // Open the search list page
                OpenSearchResultPage();
            }
        }

        /// <summary>
        /// Update address and associated parcel/owner after selecting them.
        /// </summary>
        /// <param name="sender">An object that contains the event sender.</param>
        /// <param name="e">An System.EventArgs object that contains the event data.</param>
        protected void UpdateAddressAndAssociatesButton_Click(object sender, EventArgs e)
        {
            Page.FocusElement(btnSearch.ClientID);
            imgErrorIcon.Visible = false;

            APOSessionParameterModel sessionParameter = AppSession.GetAPOSessionParameter();

            if (!string.IsNullOrEmpty(sessionParameter.ErrorMessage))
            {
                // Fail to load data
                MessageUtil.ShowMessageByControl(Page, MessageType.Error, sessionParameter.ErrorMessage);
                return;
            }

            // Display address information
            DisplayAddress(true, sessionParameter.SelectedAddress, false, false);

            // Display condition
            if (sessionParameter.ConditionInfo != null)
            {
                ConditionsUtil.ShowCondition(ucConditon, (RefAddressModel)sessionParameter.ConditionInfo);
            }

            // Display associated parcel and owner information
            if (AddressEditCompleted != null)
            {
                ParcelModel parcel = sessionParameter.SelectedParcel;
                OwnerModel owner = sessionParameter.SelectedOwner;
                string duplicateParcelKey = parcel == null || parcel.duplicatedAPOKeys == null || parcel.duplicatedAPOKeys.Length == 0
                    ? string.Empty
                    : JsonConvert.SerializeObject(parcel.duplicatedAPOKeys);
                string duplicateOwnerKey = owner == null || owner.duplicatedAPOKeys == null || owner.duplicatedAPOKeys.Length == 0
                    ? string.Empty
                    : JsonConvert.SerializeObject(owner.duplicatedAPOKeys);

                Hashtable htArgs = new Hashtable();
                htArgs.Add("SequenceNumber", parcel == null ? string.Empty : Convert.ToString(parcel.sourceSeqNumber));
                htArgs.Add("ParcelNumber", parcel == null ? string.Empty : parcel.parcelNumber);
                htArgs.Add("OwnerNumber", owner == null ? string.Empty : Convert.ToString(owner.ownerNumber));
                htArgs.Add("AddressUID", sessionParameter.SelectedAddress.UID);
                htArgs.Add("ParcelUID", parcel == null ? string.Empty : parcel.UID);
                htArgs.Add("OwnerUID", owner == null ? string.Empty : owner.UID);
                htArgs.Add("duplicateParcelKey", duplicateParcelKey);
                htArgs.Add("duplicateOwnerKey", duplicateOwnerKey);
                htArgs.Add("IsFromMap", IsFromMap);

                AddressEditCompleted(this, new CommonEventArgs(htArgs));
            }

            IsFromMap = false;
        }

        /// <summary>
        /// Raises address list page index changing command event - select address
        /// </summary>
        /// <param name="sender">An object that contains the event sender.</param>
        /// <param name="e">An GridViewCommandEventArgs object that contains the event data.</param>
        protected void AddressList_GridViewIndexChanging(object sender, GridViewPageEventArgs e)
        {
            PaginationModel pageInfo = PaginationUtil.GetPageInfoByID(PageInfoID);

            if (e.NewPageIndex > pageInfo.EndPage)
            {
                if (IsFromMap)
                {
                    if (IsCreateCapFromGIS)
                    {
                        LoadAddressListByGIS(e.NewPageIndex, pageInfo.SortExpression, (ACAGISModel)pageInfo.SearchCriterias[0]);
                    }
                    else
                    {
                        LoadAddressListByGIS(e.NewPageIndex, pageInfo.SortExpression, (RefAddressModel)pageInfo.SearchCriterias[0]);
                    }
                }
                else
                {
                    LoadAddressList(e.NewPageIndex, pageInfo.SortExpression);
                }
            }
        }

        /// <summary>
        /// PermitList GridViewSort
        /// </summary>
        /// <param name="sender">An object that contains the event sender.</param>
        /// <param name="e">A System.EventArgs object containing the event data.</param>
        protected void AddressList_GridViewSort(object sender, AccelaGridViewSortEventArgs e)
        {
            PaginationModel pageInfo = PaginationUtil.GetPageInfoByID(PageInfoID);
            pageInfo.SortExpression = e.GridViewSortExpression;
        }

        /// <summary>
        /// override PreRender event.
        /// </summary>
        /// <param name="e">The event argument.</param>
        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);

            InitFormDesignerPlaceHolder(phContent);

            /*
             * For this user control, the street number have two controls for search and edit in ACA Admin/Daily side.
             * 1. SPEAR Form, use AccelaNumberText for input.
             * 2. Search Form, use AccelaRangeNumberText control for range search.
             */
            if (IsWorkLocationPage)
            {
                foreach (SimpleViewElementModel4WS item in phContent.SimpleViewModel.simpleViewElements)
                {
                    if (item.viewElementName == txtStreetNo.ID)
                    {
                        item.viewElementName = txtStreetNo4Search.ID;
                        txtStreetNo4Search.IsFieldRequired = ValidationUtil.IsYes(item.required);
                    }

                    if (item.viewElementName == txtStreetEnd.ID)
                    {
                        item.viewElementName = txtStreetEnd4Search.ID;
                        txtStreetEnd4Search.IsFieldRequired = ValidationUtil.IsYes(item.required);
                    }
                }
            }
            else
            {
                foreach (SimpleViewElementModel4WS item in phContent.SimpleViewModel.simpleViewElements)
                {
                    if (item.viewElementName == txtStreetNo4Search.ID)
                    {
                        item.viewElementName = txtStreetNo.ID;
                        txtStreetNo.Validate += ValidationUtil.IsYes(item.required) ? ";required" : string.Empty;
                    }

                    if (item.viewElementName == txtStreetEnd4Search.ID)
                    {
                        item.viewElementName = txtStreetEnd.ID;
                        txtStreetEnd.Validate += ValidationUtil.IsYes(item.required) ? ";required" : string.Empty;
                    }
                }
            }
        }

        #endregion Protected Methods

        #region Private Methods

        /// <summary>
        /// Gets the address model from control.
        /// </summary>
        /// <param name="agencyCode">The agency code.</param>
        /// <param name="addressmodel">Address model.</param>
        private void GetAddressModelFromControl(string agencyCode, ref AddressModel addressmodel)
        {
            if (addressmodel == null)
            {
                addressmodel = new AddressModel();
            }

            addressmodel.serviceProviderCode = agencyCode;
            addressmodel.city = txtCity.Text.Trim();
            addressmodel.houseNumberStart = StringUtil.ToInt(txtStreetNo.Text.Trim());
            addressmodel.unitStart = txtUnitNo.Text.Trim();
            addressmodel.city = txtCity.Text;
            addressmodel.zip = txtZip.GetZip(ddlCountry.SelectedValue.Trim());
            addressmodel.streetName = txtStreetName.Text.Trim();
            addressmodel.county = txtCounty.Text.Trim();
            addressmodel.refAddressId = StringUtil.ToLong(txtRefAddressId.Value);
            addressmodel.UID = txtAddressUID.Value;

            addressmodel.streetSuffix = ddlStreetSuffix.SelectedValue;
            addressmodel.resStreetSuffix = GetDLLSelectedText(ddlStreetSuffix);
            addressmodel.streetDirection = ddlStreetDirection.SelectedValue;
            addressmodel.resStreetDirection = GetDLLSelectedText(ddlStreetDirection);
            addressmodel.unitType = ddlUnitType.SelectedValue;
            addressmodel.resUnitType = GetDLLSelectedText(ddlUnitType);
            addressmodel.auditID = AppSession.User.PublicUserId;
            addressmodel.auditStatus = ACAConstant.VALID_STATUS;

            //the B1_PRIMARY_ADDR_FLG automatically default to 'Y' when applying for a permit in ACA
            addressmodel.primaryFlag = ACAConstant.COMMON_Y;

            addressmodel.streetPrefix = txtPrefix.Text.Trim();
            addressmodel.houseNumberEnd = StringUtil.ToInt(txtStreetEnd.Text.Trim());
            addressmodel.unitEnd = txtUnitEnd.Text.Trim();

            addressmodel.countryCode = ddlCountry.SelectedValue.Trim();

            addressmodel.state = txtState.Text;
            addressmodel.resState = txtState.ResText;

            addressmodel.houseFractionStart = txtStartFraction.Text.Trim();
            addressmodel.houseFractionEnd = txtEndFraction.Text.Trim();

            addressmodel.streetSuffixdirection = ddlStreetSuffixDirection.SelectedValue.Trim();
            addressmodel.resStreetSuffixdirection = GetDLLSelectedText(ddlStreetSuffixDirection);

            if (txtDescription.Text.Trim().Length > ADDRESSDESC_MAXLENGTH)
            {
                addressmodel.addressDescription = txtDescription.Text.Trim().Substring(0, ADDRESSDESC_MAXLENGTH);
            }

            addressmodel.addressDescription = txtDescription.Text.Trim();
            addressmodel.distance = txtDistance.DoubleValue;

            addressmodel.secondaryRoad = txtSecondaryRoad.Text.Trim();
            addressmodel.secondaryRoadNumber = StringUtil.ToInt(txtSecondaryRoadNo.Text.Trim());

            addressmodel.inspectionDistrictPrefix = txtInspectionDP.Text.Trim();
            addressmodel.inspectionDistrict = txtInspectionD.Text.Trim();

            addressmodel.neighborhoodPrefix = txtNeighborhoodP.Text.Trim();
            addressmodel.neighborhood = txtNeighborhood.Text.Trim();

            addressmodel.XCoordinator = txtXCoordinator.DoubleValue;
            addressmodel.YCoordinator = txtYCoordinator.DoubleValue;

            if (txtStreetAddress.Text.Trim().Length > STREETNAME_MAXLENGTH)
            {
                addressmodel.fullAddress = txtStreetAddress.Text.Trim().Substring(0, STREETNAME_MAXLENGTH);
            }

            addressmodel.fullAddress = txtStreetAddress.Text.Trim();

            addressmodel.addressLine1 = txtAddressLine1.Text.Trim();
            addressmodel.addressLine2 = txtAddressLine2.Text.Trim();

            addressmodel.levelPrefix = txtLevelPrefix.Text.Trim();
            addressmodel.levelNumberStart = txtLevelNbrStart.Text.Trim();
            addressmodel.levelNumberEnd = txtLevelNbrEnd.Text.Trim();
            addressmodel.houseNumberAlphaStart = txtHouseAlphaStart.Text.Trim();
            addressmodel.houseNumberAlphaEnd = txtHouseAlphaEnd.Text.Trim();

            if (!string.IsNullOrEmpty(txtRefAddressId.Value))
            {
                addressmodel.refAddressId = StringUtil.ToLong(txtRefAddressId.Value);
            }

            addressmodel.duplicatedAPOKeys =
                JsonConvert.DeserializeObject<DuplicatedAPOKeyModel[]>(hfDuplicateAddressKeys.Value);
        }

        /// <summary>
        /// Load address list when search in Work Location.
        /// </summary>
        /// <param name="currentPageIndex">Current page index.</param>
        /// <param name="sortExpression">Sort expression.</param>
        private void LoadAddressList(int currentPageIndex, string sortExpression)
        {
            RefAddressModel searchCriterias = GetRefAddressModel();

            // Check empty input on external source.
            if (StandardChoiceUtil.IsExternalAddressSource() && APOUtil.IsEmpty(searchCriterias))
            {
                MessageUtil.ShowMessageByControl(Page, MessageType.Error, GetTextByKey("aca_apo_msg_searchcriteria_required"));
                return;
            }

            GridViewDataSource = GetAddressList(currentPageIndex, sortExpression, searchCriterias);

            // Display address info list
            LoadAddressList();
        }

        /// <summary>
        /// Load address list in Work Location.
        /// </summary>
        /// <param name="refAddressModel">The RefAddressModel</param>
        private void LoadAddressList(RefAddressModel refAddressModel = null)
        {
            ucAddressList.Visible = true;

            if (refAddressModel != null)
            {
                ucAddressList.ZipCode = refAddressModel.zip;
                ucAddressList.City = refAddressModel.city;
            }
            else
            {
                ucAddressList.ZipCode = txtZip.Text;
                ucAddressList.City = txtCity.Text;
            }

            AppSession.SelectedParcelInfo = null;
            ucAddressList.BindAddressList(GridViewDataSource);
        }

        /// <summary>
        /// Get address list from gis map.
        /// </summary>
        /// <param name="currentPageIndex">Current page index</param>
        /// <param name="sortExpression">Sort expression</param>
        /// <param name="gisModel">The ACAGISModel</param>
        /// <returns>A DataTable</returns>
        private DataTable GetAddressList(int currentPageIndex, string sortExpression, ACAGISModel gisModel)
        {
            //Search APO when create a cap from GIS map.
            PaginationModel pageInfo = GetAddressPaginationInfo(currentPageIndex, sortExpression);
            pageInfo.SearchCriterias = new object[] { gisModel };

            ParcelInfoModel[] parcelInfos = GISUtil.GetAPOListByGISModel(pageInfo, gisModel);

            //if current page is worklocation, need filter record without address.
            if (IsWorkLocationPage && parcelInfos != null && parcelInfos.Length > 0)
            {
                List<ParcelInfoModel> list = new List<ParcelInfoModel>();

                foreach (ParcelInfoModel item in parcelInfos)
                {
                    if (item.RAddressModel != null)
                    {
                        list.Add(item);
                    }
                }

                parcelInfos = list.ToArray();
            }

            DataTable dt = CreateDataSource(parcelInfos);

            return PaginationUtil.MergeDataSource<DataTable>(GridViewDataSource, dt, pageInfo);
        }

        /// <summary>
        /// Get address list from search form
        /// </summary>
        /// <param name="currentPageIndex">Current page index</param>
        /// <param name="sortExpression">Sort expression</param>
        /// <param name="searchCriterias">Search criteria</param>
        /// <returns>A DataTable</returns>
        private DataTable GetAddressList(int currentPageIndex, string sortExpression, RefAddressModel searchCriterias)
        {
            ParcelInfoModel[] parcelInfos = null;

            IGviewBll gviewBll = ObjectFactory.GetObject<IGviewBll>();
            SimpleViewElementModel4WS[] viewModels = gviewBll.GetSimpleViewElementModel(ModuleName, GviewID.AddressListInWorkLocation);
            bool hasParcelOwnerColumns =
                (from vm in viewModels
                 where (ADDRESS_LIST_PARCEL_COLUMN_NAME.Equals(vm.viewElementName) && ACAConstant.VALID_STATUS.Equals(vm.recStatus)) ||
                 (ADDRESS_LIST_OWNER_COLUMN_NAME.Equals(vm.viewElementName) && ACAConstant.VALID_STATUS.Equals(vm.recStatus))
                 select vm).Count() > 0;
            PaginationModel pageInfo = GetAddressPaginationInfo(currentPageIndex, sortExpression);
            QueryFormat queryFormat = PaginationUtil.GetQueryFormatModel(pageInfo);

            if (IsWorkLocationPage && !hasParcelOwnerColumns)
            {
                IRefAddressBll refAddressBll = ObjectFactory.GetObject<IRefAddressBll>();
                SearchResultModel result = refAddressBll.GetRefAddressByRefAddressModelParcelModel(ConfigManager.AgencyCode, searchCriterias, null, queryFormat);
                pageInfo.StartDBRow = result.startRow;

                if (result.resultList != null && result.resultList.Length > 0)
                {
                    parcelInfos = new ParcelInfoModel[result.resultList.Length];

                    for (int i = 0; i < result.resultList.Length; i++)
                    {
                        parcelInfos[i] = new ParcelInfoModel();
                        parcelInfos[i].RAddressModel = result.resultList[i] as RefAddressModel;
                    }
                }
            }
            else
            {
                IAPOBll apoBll = ObjectFactory.GetObject<IAPOBll>();
                SearchResultModel result = apoBll.GetParcelInfoByAddress(ConfigManager.AgencyCode, searchCriterias, queryFormat, true);
                pageInfo.StartDBRow = result.startRow;
                parcelInfos = ObjectConvertUtil.ConvertObjectArray2EntityArray<ParcelInfoModel>(result.resultList);
            }

            return PaginationUtil.MergeDataSource<DataTable>(GridViewDataSource, CreateDataSource(parcelInfos), pageInfo);
        }

        /// <summary>
        /// Bind owner item into dropdownlist.
        /// </summary>
        private void BindAutoFillItems()
        {
            IList<ListItem> items = new List<ListItem>();
            ListItem item = new ListItem();

            var proxyUserRoleBll = ObjectFactory.GetObject<IProxyUserRoleBll>();
            PublicUserModel4WS user = proxyUserRoleBll.GetCurrentUser(AppSession.GetCapModelFromSession(ModuleName));

            if (user.addressList != null && user.addressList.Length > 0)
            {
                foreach (RefAddressModel refAddress in user.addressList)
                {
                    // Check the audit status.
                    if (refAddress == null || !ACAConstant.VALID_STATUS.Equals(refAddress.auditStatus))
                    {
                        continue;
                    }

                    IAddressBuilderBll addressBuilderBll = ObjectFactory.GetObject<IAddressBuilderBll>();
                    string addressName = addressBuilderBll.BuildAddressByFormatType(refAddress, null, AddressFormatType.SHORT_ADDRESS_NO_FORMAT);

                    if (string.IsNullOrEmpty(addressName) || addressName.Trim().Length == 0)
                    {
                        continue;
                    }

                    item = new ListItem();
                    item.Text = addressName;

                    AutoFillParameter parameter = new AutoFillParameter()
                    {
                        AutoFillType = ACAConstant.AutoFillType4SpearForm.Address,
                        SectionId = ID,
                        EntityId = refAddress.sourceNumber.HasValue ? refAddress.sourceNumber.ToString() : string.Empty,
                        EntityType = refAddress.refAddressId.HasValue ? refAddress.refAddressId.ToString() : string.Empty,
                        EntityRefId = refAddress.UID
                    };

                    JavaScriptSerializer javaScriptSerializer = new JavaScriptSerializer();
                    item.Value = javaScriptSerializer.Serialize(parameter);
                    items.Add(item);
                }

                DropDownListBindUtil.BindDDL(items, ddlAutoFillAddressInfo, false);
            }

            if (items.Count <= 0 && !AppSession.IsAdmin)
            {
                chkAutoFillAddressInfo.Visible = false;
                divAutoFill.Visible = false;
            }
        }

        /// <summary>
        /// clear address data
        /// </summary>
        private void ClearAddressForm()
        {
            ControlUtil.ClearValue(this, FilteredControlIDs);
            hfDisableFormFlag.Value = string.Empty;
            txtRefAddressId.Value = string.Empty;
            txtAddressUID.Value = string.Empty;
            ucConditon.HideCondition();

            EnableAddressForm();
        }

        /// <summary>
        /// Create the data source for address list in Work Location.
        /// </summary>
        /// <param name="parcelInfos">ParcelInfoModel array</param>
        /// <returns>data source for address list</returns>
        private DataTable CreateDataSource(ParcelInfoModel[] parcelInfos)
        {
            if (parcelInfos != null && parcelInfos.Length > 0)
            {
                for (int i = 0; i < parcelInfos.Length; i++)
                {
                    parcelInfos[i].RowIndex = i;
                }
            }

            ucAddressList.ParcelInfoList = parcelInfos;
            DataTable dt = APOUtil.BuildAPODataTable(parcelInfos, AddressFormatType.LONG_ADDRESS_NO_FORMAT);

            if (dt.Columns.Contains("FullAddress"))
            {
                DataView dv = dt.DefaultView;
                dv.Sort = "FullAddress ASC";
                dt = dv.ToTable();
            }

            return dt;
        }

        /// <summary>
        /// Creates an original address template.
        /// </summary>
        private void CreateOriginalAddressTemplate()
        {
            ITemplateBll templateBll = ObjectFactory.GetObject<ITemplateBll>();
            TemplateAttributeModel[] attributes;
            attributes = templateBll.GetAPOTemplateAttributes(TemplateType.CAP_ADDRESS, ConfigManager.AgencyCode, AppSession.User.PublicUserId);   
            templateEdit.DisplayAttributes(attributes, ACAConstant.CAP_ADDRESS_TEMPLATE_FIELD_PREFIX);
        }

        /// <summary>
        /// Display address form
        /// </summary>
        private void DisableAddressForm()
        {
            DisableEdit(this, FilteredControlIDs);
        }

        /// <summary>
        /// when set editable property to false in aca admin the function will be raised.
        /// </summary>
        private void DisableAddressFormAndButton()
        {
            DisableEdit(this, FilteredControlIDs);
            btnSearch.Enabled = false;
            btnClearAddress.Enabled = false;
        }

        /// <summary>
        /// Enable address form 
        /// </summary>
        private void EnableAddressForm()
        {
            EnableEdit(this, FilteredControlIDs);
        }

        /// <summary>
        /// Fill reference template information into template fields
        /// </summary>
        /// <param name="addressId">The reference address id</param>
        /// <param name="attributes">Template attributes</param>
        private void FillTemplate(string addressId, TemplateAttributeModel[] attributes)
        {
            ITemplateBll templateBll = ObjectFactory.GetObject<ITemplateBll>();

            if (attributes == null)
            {
                attributes = templateBll.GetRefAPOTemplateAttributes(
                    TemplateType.CAP_ADDRESS,
                    addressId,
                    ConfigManager.AgencyCode,
                    AppSession.User.PublicUserId);
            }
            else
            {
                attributes = templateBll.FillTemplateDropDownList(TemplateType.CAP_ADDRESS, attributes);
            }

            templateEdit.FillAttributeValues(attributes);
        }

        /// <summary>
        /// Get ref address model
        /// </summary>
        /// <returns>The RefAddressModel</returns>
        private RefAddressModel GetRefAddressModel()
        {
            // construct a address model to search addresses.
            RefAddressModel addressModel = new RefAddressModel();

            if (IsWorkLocationPage)
            {
                //If only one parameter of range text inputed, than do not use range search. Use only one street number to fix search.
                Range<int?> streetNoRange = Range<int>.GetRangeValue(txtStreetNo4Search.TextFrom, txtStreetNo4Search.TextTo);
                addressModel.houseNumberStart = streetNoRange.SingleValue;
                addressModel.houseNumberStartFrom = streetNoRange.LowerBound;
                addressModel.houseNumberStartTo = streetNoRange.UpperBound;

                streetNoRange = Range<int>.GetRangeValue(txtStreetEnd4Search.TextFrom, txtStreetEnd4Search.TextTo);
                addressModel.houseNumberEnd = streetNoRange.SingleValue;
                addressModel.houseNumberEndFrom = streetNoRange.LowerBound;
                addressModel.houseNumberEndTo = streetNoRange.UpperBound;
            }
            else
            {
                addressModel.houseNumberStart = StringUtil.ToInt(txtStreetNo.Text.Trim());
                addressModel.houseNumberEnd = StringUtil.ToInt(txtStreetEnd.Text.Trim());
            }

            addressModel.streetDirection = ddlStreetDirection.SelectedValue;
            addressModel.resStreetDirection = GetDLLSelectedText(ddlStreetDirection);
            addressModel.streetName = txtStreetName.Text.Trim();
            addressModel.streetSuffix = ddlStreetSuffix.SelectedValue;
            addressModel.resStreetSuffix = GetDLLSelectedText(ddlStreetSuffix);

            addressModel.unitType = ddlUnitType.SelectedValue;
            addressModel.resUnitType = GetDLLSelectedText(ddlUnitType);
            addressModel.unitStart = txtUnitNo.Text.Trim();

            string zipCode = txtZip.GetZip(ddlCountry.SelectedValue.Trim());
            addressModel.zip = zipCode.Trim();
            addressModel.city = ControlUtil.GetControlValue(txtCity);

            addressModel.addressStatus = ACAConstant.VALID_STATUS;

            addressModel.streetPrefix = txtPrefix.Text.Trim();
            addressModel.unitEnd = txtUnitEnd.Text.Trim();

            addressModel.county = txtCounty.Text.Trim();
            addressModel.countryCode = ControlUtil.GetControlValue(ddlCountry);

            addressModel.state = ControlUtil.GetControlValue(txtState);

            addressModel.auditStatus = ACAConstant.VALID_STATUS;

            addressModel.houseFractionStart = txtStartFraction.Text.Trim();
            addressModel.houseFractionEnd = txtEndFraction.Text.Trim();

            addressModel.streetSuffixdirection = ddlStreetSuffixDirection.SelectedValue.Trim();
            addressModel.resStreetSuffixdirection = GetDLLSelectedText(ddlStreetSuffixDirection);

            if (txtDescription.Text.Trim().Length > ADDRESSDESC_MAXLENGTH)
            {
                addressModel.addressDescription = txtDescription.Text.Trim().Substring(0, ADDRESSDESC_MAXLENGTH);
            }
            else
            {
                addressModel.addressDescription = txtDescription.Text.Trim();
            }

            addressModel.distance = txtDistance.DoubleValue;

            addressModel.secondaryRoad = txtSecondaryRoad.Text.Trim();
            addressModel.secondaryRoadNumber = StringUtil.ToInt(txtSecondaryRoadNo.Text.Trim());

            addressModel.inspectionDistrictPrefix = txtInspectionDP.Text.Trim();
            addressModel.inspectionDistrict = txtInspectionD.Text.Trim();

            addressModel.neighborhoodPrefix = txtNeighborhoodP.Text.Trim();
            addressModel.neighborhood = txtNeighborhood.Text.Trim();

            addressModel.XCoordinator = txtXCoordinator.DoubleValue;
            addressModel.YCoordinator = txtYCoordinator.DoubleValue;

            if (txtStreetAddress.Text.Trim().Length > STREETNAME_MAXLENGTH)
            {
                addressModel.fullAddress = txtStreetAddress.Text.Trim().Substring(0, STREETNAME_MAXLENGTH);
            }
            else
            {
                addressModel.fullAddress = txtStreetAddress.Text.Trim();
            }

            addressModel.addressLine1 = txtAddressLine1.Text.Trim();
            addressModel.addressLine2 = txtAddressLine2.Text.Trim();

            addressModel.levelPrefix = txtLevelPrefix.Text.Trim();
            addressModel.levelNumberStart = txtLevelNbrStart.Text.Trim();
            addressModel.levelNumberEnd = txtLevelNbrEnd.Text.Trim();
            addressModel.houseNumberAlphaStart = txtHouseAlphaStart.Text.Trim();
            addressModel.houseNumberAlphaEnd = txtHouseAlphaEnd.Text.Trim();

            addressModel.templates = templateEdit.GetAttributeModels(true);

            return addressModel;
        }

        /// <summary>
        /// Initializes the UI when page post back
        /// </summary>
        private void InitUI()
        {
            InitialiteFromSession();
            InitButton();
        }

        /// <summary>
        /// Set current City and State
        /// </summary>
        private void SetCurrentCityAndState()
        {
            AutoFillCityAndStateUtil.SetCurrentCity(txtCity, ModuleName);
            AutoFillCityAndStateUtil.SetCurrentState(txtState, ModuleName);
        }

        /// <summary>
        /// Show all condition message of APO at the bottom
        /// </summary>
        /// <param name="address">Selected Address</param>
        /// <param name="parcel">External Parcel</param>
        /// <param name="owner">External Owner</param>
        /// <returns>true or false.</returns>
        private bool ShowCondition(AddressModel address, ParcelModel parcel, RefOwnerModel owner)
        {
            string sourceNumber = null;
            if (AppSession.SelectedParcelInfo != null && AppSession.SelectedParcelInfo.RAddressModel != null)
            {
                if (AppSession.SelectedParcelInfo.RAddressModel.sourceNumber != null)
                {
                    sourceNumber = AppSession.SelectedParcelInfo.RAddressModel.sourceNumber.ToString();
                }
            }
            else
            {
                // Get APO source number 
                string servProvCode = address.serviceProviderCode;

                if (!string.IsNullOrEmpty(servProvCode))
                {
                    IServiceProviderBll serviceProviderBll = ObjectFactory.GetObject<IServiceProviderBll>();
                    ServiceProviderModel servProviderModel = serviceProviderBll.GetServiceProviderByPK(servProvCode, AppSession.User.PublicUserId);

                    if (servProviderModel != null)
                    {
                        sourceNumber = servProviderModel.sourceNumber.ToString();
                    }
                }
            }

            // 1. Prepare the key data for Parcel Info Model
            RefAddressModel refAddressModel = null;
            ParcelModel parcelModel = null;
            OwnerModel refOwnerModel = null;

            if (address != null)
            {               
                refAddressModel = new RefAddressModel();
                refAddressModel.refAddressId = address.refAddressId;
                refAddressModel.UID = address.UID;
                refAddressModel.sourceNumber = StringUtil.ToInt(sourceNumber);
                refAddressModel.duplicatedAPOKeys = address.duplicatedAPOKeys;
            }

            if (parcel != null)
            {
                parcelModel = new ParcelModel();
                parcelModel.parcelNumber = parcel.parcelNumber;
                parcelModel.UID = parcel.UID;
                parcelModel.sourceSeqNumber = StringUtil.ToLong(sourceNumber);
                parcelModel.duplicatedAPOKeys = parcel.duplicatedAPOKeys;
            }            

            if (owner != null)
            {
                refOwnerModel = new OwnerModel();
                refOwnerModel.ownerNumber = StringUtil.ToLong(Convert.ToString(owner.ownerNumber));
                refOwnerModel.UID = owner.UID;
                refOwnerModel.sourceSeqNumber = StringUtil.ToLong(sourceNumber);
                refOwnerModel.duplicatedAPOKeys = owner.duplicatedAPOKeys;
            }

            ParcelInfoModel parcelInfo = new ParcelInfoModel();
            parcelInfo.RAddressModel = refAddressModel;
            parcelInfo.parcelModel = parcelModel;
            parcelInfo.ownerModel = refOwnerModel;

            // 2. Show the Codition message of APO here.
            IRefAddressBll addressBll = ObjectFactory.GetObject<IRefAddressBll>();
            RefAddressModel refAddressModelWithCondition = addressBll.GetAddressCondition(CapUtil.GetAgencyCodeList(ModuleName), parcelInfo);

            return ConditionsUtil.ShowCondition(ucConditon, refAddressModelWithCondition);
        }

        /// <summary>
        /// Show address template fields.
        /// </summary>
        /// <param name="isReferenceAddress">True if is reference address</param>
        /// <param name="addressModel">The AddressModel</param>
        private void ShowTemplateFields(bool isReferenceAddress, AddressModel addressModel)
        {
            if (addressModel == null)
            {
                return;
            }

            if (isReferenceAddress)
            {
                FillTemplate(Convert.ToString(addressModel.refAddressId), addressModel.templates);
            }
            else
            {
                ITemplateBll templateBll = ObjectFactory.GetObject<ITemplateBll>();

                if (AppSession.IsAdmin)
                {
                    TemplateAttributeModel[] attributes = templateBll.GetAPOTemplateAttributes(TemplateType.CAP_ADDRESS, ConfigManager.AgencyCode, ACAConstant.ADMIN_CALLER_ID);
                    templateEdit.DisplayAttributes(attributes, ACAConstant.CAP_ADDRESS_TEMPLATE_FIELD_PREFIX);
                }
                else
                {
                    if (addressModel.templates != null)
                    {
                        addressModel.templates = templateBll.FillTemplateDropDownList(TemplateType.CAP_ADDRESS, addressModel.templates);

                        // when resume an application or click back button or in confirm page(edit mode), get it from address model.
                        templateEdit.DisplayAttributes(addressModel.templates, ACAConstant.CAP_ADDRESS_TEMPLATE_FIELD_PREFIX);
                    }
                    else
                    {
                        //at first time in cap edit page, addressModel.attributes is null for super agency.
                        TemplateAttributeModel[] attributes = templateBll.GetRefAPOTemplateAttributes(TemplateType.CAP_ADDRESS, txtRefAddressId.Value, ConfigManager.AgencyCode, AppSession.User.PublicUserId);

                        templateEdit.DisplayAttributes(attributes, ACAConstant.CAP_ADDRESS_TEMPLATE_FIELD_PREFIX);
                    }
                }
            }
        }

        /// <summary>
        /// when set component property data source 'Transactional',
        /// should hidden search button and clean button.
        /// </summary>
        private void InitButton()
        {
            if (ComponentDataSource.Transactional.Equals(ValidateFlag))
            {
                liSearchButton.Visible = false;
            }
        }

        /// <summary>
        /// Get address's pagination information.
        /// </summary>
        /// <param name="currentPageIndex">Current page index</param>
        /// <param name="sortExpression">Sort expression</param>
        /// <returns>A PaginationModel object</returns>
        private PaginationModel GetAddressPaginationInfo(int currentPageIndex, string sortExpression)
        {
            PaginationModel pageInfo = PaginationUtil.GetPageInfoByID(PageInfoID);
            pageInfo.SortExpression = sortExpression;
            pageInfo.CurrentPageIndex = currentPageIndex;
            pageInfo.CustomPageSize = ucAddressList.PageSize;

            return pageInfo;
        }

        /// <summary>
        /// Gets new APO session parameters.
        /// </summary>
        /// <returns>An APOSessionParameterModel</returns>
        private APOSessionParameterModel GetAPOSessionParameter()
        {
            APOSessionParameterModel sessionParameter = new APOSessionParameterModel();
            sessionParameter.IsCreateCapFromGIS = IsCreateCapFromGIS;
            sessionParameter.IsFromMap = IsFromMap;
            sessionParameter.IsValidate = IsValidate;
            sessionParameter.CallbackFunctionName = UpdateAddressAndAssociatesFunctionName;
            sessionParameter.ExternalOwnerForSuperAgency = ExternalOwnerForSuperAgency;
            sessionParameter.ExternalParcelForSuperAgency = ExternalParcelForSuperAgency;

            return sessionParameter;
        }

        /// <summary>
        /// Open the search result page
        /// </summary>
        private void OpenSearchResultPage()
        {
            string script = string.Format("{0}_OpenAddressSearchResult();", ClientID);
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "OpenAddressSearchResult" + CommonUtil.GetRandomUniqueID(), script, true);
        }

        #endregion Private Methods
    }
}