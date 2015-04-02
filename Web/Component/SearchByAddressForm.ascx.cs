#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: CapDetail.aspx.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2011-2014
 *
 *  Description:
 *
 *  Notes:
 *      $Id: SearchByAddress.ascx.cs 195950 2011-05-09 04:03:34Z ACHIEVO\grady.zeng $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using System;
using Accela.ACA.BLL.APO;
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
    /// Search by address form
    /// </summary>
    public partial class SearchByAddressForm : FormDesignerBaseControl
    {
        #region Fields

        /// <summary>
        /// Address description max length.
        /// </summary>
        private const int ADDRESSDESC_MAXLENGTH = 255;

        /// <summary>
        /// Street name max length.
        /// </summary>
        private const int STREETNAME_MAXLENGTH = 1024;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="SearchByAddressForm"/> class.
        /// </summary>
        public SearchByAddressForm()
            : base(GviewID.SearchByAddress)
        {
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// Gets or sets Permission.
        /// </summary>
        protected override WSProxy.GFilterScreenPermissionModel4WS Permission
        {
            get
            {
                base.Permission = new WSProxy.GFilterScreenPermissionModel4WS();
                base.Permission.permissionLevel = "APO";
                base.Permission.permissionValue = "ADDRESS";
                return base.Permission;
            }

            set
            {
                base.Permission = value;
            }
        }

        #endregion Properties

        #region Public Methods

        /// <summary>
        /// Check input condition
        /// </summary>
        /// <returns>return true if check pass.</returns>
        public bool CheckInputConditionForAddress()
        {
            return !(CheckStandardFields() && templateEdit.IsControlsValueEmpty());
        }
       
        /// <summary>
        /// Fill address query fields with address model
        /// </summary>
        /// <param name="addressModel">A RefAddressModel</param>
        public void FillAddressInfo(RefAddressModel addressModel)
        {
            DropDownListBindUtil.SetCountrySelectedValue(ddlCountry, addressModel.countryCode, false, true, true);

            txtNumber.TextFrom = addressModel.houseNumberStartFrom.ToString();
            txtNumber.TextTo = addressModel.houseNumberStartTo.ToString();
            txtStreetEnd.TextFrom = addressModel.houseNumberEndFrom.ToString();
            txtStreetEnd.TextTo = addressModel.houseNumberEndTo.ToString();
            txtStartFraction.Text = addressModel.houseFractionStart;
            txtEndFraction.Text = addressModel.houseFractionEnd;
            ddlDirection.SelectedValue = addressModel.streetDirection;
            txtPrefix.Text = addressModel.streetPrefix;
            txtStreetName.Text = addressModel.streetName;
            ddlStreetSuffix.SelectedValue = addressModel.streetSuffix;
            ddlStreetSuffixDirection.SelectedValue = addressModel.streetSuffixdirection;

            ddlUnitType.SelectedValue = addressModel.unitType;
            txtUnitNo.Text = GetStreetAndUnitNumber(addressModel.unitRangeStart, addressModel.unitRangeEnd, addressModel.unitStart, false);
            txtUnitEnd.Text = addressModel.unitEnd;

            txtSecondaryRoad.Text = addressModel.secondaryRoad;
            txtSecondaryRoadNo.Text = Convert.ToString(addressModel.secondaryRoadNumber);

            txtNeighborhoodP.Text = addressModel.neighborhoodPrefix;
            txtNeighborhood.Text = addressModel.neighborhood;

            txtDescription.Text = addressModel.addressDescription;
            txtDistance.DoubleValue = addressModel.distance;
            txtXCoordinator.DoubleValue = addressModel.XCoordinator;
            txtYCoordinator.DoubleValue = addressModel.YCoordinator;

            txtInspectionDP.Text = addressModel.inspectionDistrictPrefix;
            txtInspectionD.Text = addressModel.inspectionDistrict;

            txtCity.Text = addressModel.city;
            txtCounty.Text = addressModel.county;
            txtState.Text = addressModel.state;
            txtAppZipSearchPermit.Text = ModelUIFormat.FormatZipShow(addressModel.zip, addressModel.countryCode, false);

            //chkPrimary.Checked = addressModel.primaryFlag == ACAConstant.COMMON_Y ? true : false;
            txtStreetAddress.Text = addressModel.fullAddress;

            txtAddressLine1.Text = addressModel.addressLine1;
            txtAddressLine2.Text = addressModel.addressLine2;

            txtLevelPrefix.Text = addressModel.levelPrefix;
            txtLevelNbrStart.Text = addressModel.levelNumberStart;
            txtLevelNbrEnd.Text = addressModel.levelNumberEnd;
            txtHouseAlphaStart.Text = addressModel.houseNumberAlphaStart;
            txtHouseAlphaEnd.Text = addressModel.houseNumberAlphaEnd;

            templateEdit.DisplayAttributes(addressModel.templates, ACAConstant.CAP_ADDRESS_TEMPLATE_FIELD_PREFIX, true);
        }
       
        /// <summary>
        /// Get control prefix.
        /// </summary>
        /// <returns>Control prefix</returns>
        public string GetPrefix()
        {
            return txtNumber.ClientID.Replace("txtNumber", string.Empty);
        }

        /// <summary>
        /// Get ref address model from form.
        /// </summary>
        /// <returns>Address Model</returns>
        public RefAddressModel GetRefAddressModel()
        {
            RefAddressModel addressModel = new RefAddressModel();

            /*
             * If only one parameter of street number range(from/to) inputed, then do not use range search.
             * But use only one parameter to fix search.
             */
            Range<int?> streetNoRange = Range<int>.GetRangeValue(txtNumber.TextFrom, txtNumber.TextTo);
            addressModel.houseNumberStart = streetNoRange.SingleValue;
            addressModel.houseNumberStartFrom = streetNoRange.LowerBound;
            addressModel.houseNumberStartTo = streetNoRange.UpperBound;

            streetNoRange = Range<int>.GetRangeValue(txtStreetEnd.TextFrom, txtStreetEnd.TextTo);
            addressModel.houseNumberEnd = streetNoRange.SingleValue;
            addressModel.houseNumberEndFrom = streetNoRange.LowerBound;
            addressModel.houseNumberEndTo = streetNoRange.UpperBound;

            if (!string.IsNullOrEmpty(txtUnitNo.Text))
            {
                string unitStartText = txtUnitNo.Text;

                if (unitStartText.IndexOf(ACAConstant.SPLIT_CHAR4, StringComparison.Ordinal) > 0)
                {
                    string[] unitStartRanges = unitStartText.Split(Convert.ToChar(ACAConstant.SPLIT_CHAR4));

                    if (unitStartRanges.Length >= 2)
                    {
                        addressModel.unitRangeStart = unitStartRanges[0];
                        addressModel.unitRangeEnd = unitStartRanges[1];
                    }
                }
                else
                {
                    addressModel.unitStart = unitStartText;
                }
            }

            addressModel.streetDirection = ddlDirection.SelectedValue.Trim();
            addressModel.streetName = txtStreetName.Text.Trim();
            addressModel.streetSuffix = ddlStreetSuffix.SelectedValue.Trim();
            addressModel.unitType = ddlUnitType.SelectedValue.Trim();
            addressModel.city = txtCity.Text.Trim();
            addressModel.zip = txtAppZipSearchPermit.GetZip(ddlCountry.SelectedValue.Trim());

            addressModel.streetPrefix = txtPrefix.Text.Trim();
            
            addressModel.unitEnd = txtUnitEnd.Text.Trim();

            addressModel.county = txtCounty.Text.Trim();
            addressModel.countryCode = ddlCountry.SelectedValue.Trim();

            addressModel.state = txtState.Text;

            addressModel.houseFractionStart = txtStartFraction.Text.Trim();
            addressModel.houseFractionEnd = txtEndFraction.Text.Trim();

            addressModel.streetSuffixdirection = ddlStreetSuffixDirection.SelectedValue.Trim();

            if (txtDescription.Text.Trim().Length > ADDRESSDESC_MAXLENGTH)
            {
                addressModel.addressDescription = txtDescription.Text.Trim().Substring(0, ADDRESSDESC_MAXLENGTH);
            }

            addressModel.addressDescription = txtDescription.Text.Trim();
            addressModel.distance = txtDistance.DoubleValue;

            addressModel.secondaryRoad = txtSecondaryRoad.Text.Trim();
            addressModel.secondaryRoadNumber = StringUtil.ToInt(txtSecondaryRoadNo.Text.Trim());

            addressModel.inspectionDistrictPrefix = txtInspectionDP.Text.Trim();
            addressModel.inspectionDistrict = txtInspectionD.Text.Trim();

            addressModel.neighborhoodPrefix = txtNeighborhoodP.Text.Trim();
            addressModel.neighborhood = txtNeighborhood.Text.Trim();

            addressModel.XCoordinator = StringUtil.ToInt(txtXCoordinator.Text.Trim());
            addressModel.YCoordinator = StringUtil.ToInt(txtYCoordinator.Text.Trim());

            if (txtStreetAddress.Text.Trim().Length > STREETNAME_MAXLENGTH)
            {
                addressModel.fullAddress = txtStreetAddress.Text.Trim().Substring(0, STREETNAME_MAXLENGTH);
            }

            addressModel.fullAddress = txtStreetAddress.Text.Trim();

            addressModel.addressLine1 = txtAddressLine1.Text.Trim();
            addressModel.addressLine2 = txtAddressLine2.Text.Trim();

            addressModel.levelPrefix = txtLevelPrefix.Text.Trim();
            addressModel.levelNumberStart = txtLevelNbrStart.Text.Trim();
            addressModel.levelNumberEnd = txtLevelNbrEnd.Text.Trim();
            addressModel.houseNumberAlphaStart = txtHouseAlphaStart.Text.Trim();
            addressModel.houseNumberAlphaEnd = txtHouseAlphaEnd.Text.Trim();

            addressModel.templates = templateEdit.GetAttributeModels();

            return addressModel;
        }

        /// <summary>
        /// Initializes form fields and template fields.
        /// </summary>
        public void InitUI()
        {
            DropDownListBindUtil.BindStreetDirection(ddlDirection);
            DropDownListBindUtil.BindStreetSuffix(ddlStreetSuffix);
            DropDownListBindUtil.BindUnitType(ddlUnitType);
            ddlCountry.BindItems();
            DropDownListBindUtil.BindStreetDirection(ddlStreetSuffixDirection);

            DisplayTemplateFields();
        }

        /// <summary>
        /// Set current City and State
        /// </summary>
        public void SetCurrentCityAndState()
        {
            AutoFillCityAndStateUtil.SetCurrentCity(txtCity, ModuleName);
            AutoFillCityAndStateUtil.SetCurrentState(txtState, ModuleName);
        }

        /// <summary>
        /// Set default value
        /// </summary>
        public void SetDefaultValue()
        {
            ControlUtil.ClearValue(this, null);
        }

        /// <summary>
        /// Initializes Address Form
        /// </summary>
        public void InitAddressForm()
        {
            DropDownListBindUtil.BindStreetDirection(ddlDirection);
            DropDownListBindUtil.BindStreetSuffix(ddlStreetSuffix);
            DropDownListBindUtil.BindUnitType(ddlUnitType);
            DropDownListBindUtil.BindStreetDirection(ddlStreetSuffixDirection);

            ControlUtil.ClearRegionalSetting(ddlCountry, true, string.Empty, null, string.Empty);
        }

        /// <summary>
        /// Applies the regional setting.
        /// </summary>
        /// <param name="getDefault">If set to <c>true</c> [get default].</param>
        public void ApplyRegionalSetting(bool getDefault)
        {
            ControlUtil.ApplyRegionalSetting(IsPostBack, true, true, getDefault, ddlCountry);
        }

        #endregion Public Methods

        #region Protected Methods

        /// <summary>
        /// <c>OnInit</c> event method
        /// </summary>
        /// <param name="e">EventArgs e</param>
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            ddlCountry.SetCountryControls(txtAppZipSearchPermit, txtState, null);
        }

        /// <summary>
        /// Page load
        /// </summary>
        /// <param name="sender">The sender event.</param>
        /// <param name="e">EventArgs e</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!AppSession.IsAdmin)
            {
                SetCurrentCityAndState();
            }

            ApplyRegionalSetting(false);

            phContent.TemplateControlIDPrefix = ACAConstant.CAP_ADDRESS_TEMPLATE_FIELD_PREFIX;
            ControlBuildHelper.HideStandardFields(GviewID.SearchByAddress, ModuleName, Controls, AppSession.IsAdmin, Permission);   
        }

        /// <summary>
        /// Overwrite pre render 
        /// </summary>
        /// <param name="e">EventArgs e</param>
        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);
            InitFormDesignerPlaceHolder(phContent);
        }

        #endregion Protected Methods

        #region Private Methods

        /// <summary>
        /// Check standard fields whether is empty or not.
        /// </summary>
        /// <returns>The input value flag</returns>
        private bool CheckStandardFields()
        {
            bool result = false;

            if (txtAppZipSearchPermit.Text.Trim().EndsWith("-", StringComparison.InvariantCulture))
            {
                txtAppZipSearchPermit.Text = txtAppZipSearchPermit.Text.Trim().Replace("-", string.Empty).Trim();
            }

            if (string.IsNullOrEmpty(txtNumber.GetValue())
               && ddlDirection.SelectedIndex < 1 &&
               string.IsNullOrEmpty(txtStreetName.Text.Trim())
               && ddlStreetSuffix.SelectedIndex < 1
               && ddlUnitType.SelectedIndex < 1
               && string.IsNullOrEmpty(txtUnitNo.Text.Trim())
               && string.IsNullOrEmpty(txtCity.Text.Trim())
               && string.IsNullOrEmpty(txtState.Text)
               && string.IsNullOrEmpty(txtAppZipSearchPermit.Text.Trim())
               && string.IsNullOrEmpty(txtPrefix.Text.Trim())
               && string.IsNullOrEmpty(txtStreetEnd.GetValue())
               && string.IsNullOrEmpty(txtUnitEnd.Text.Trim())
               && string.IsNullOrEmpty(txtCounty.Text.Trim())
               && ddlCountry.SelectedIndex < 1
               && string.IsNullOrEmpty(txtStartFraction.Text.Trim())
               && string.IsNullOrEmpty(txtEndFraction.Text.Trim())
               && ddlStreetSuffixDirection.SelectedIndex < 1
               && string.IsNullOrEmpty(txtDescription.Text.Trim())
               && string.IsNullOrEmpty(txtDistance.Text.Trim())
               && string.IsNullOrEmpty(txtSecondaryRoad.Text.Trim())
               && string.IsNullOrEmpty(txtSecondaryRoadNo.Text.Trim())
               && string.IsNullOrEmpty(txtInspectionDP.Text.Trim())
               && string.IsNullOrEmpty(txtInspectionD.Text.Trim())
               && string.IsNullOrEmpty(txtNeighborhoodP.Text.Trim())
               && string.IsNullOrEmpty(txtNeighborhood.Text.Trim())
               && string.IsNullOrEmpty(txtXCoordinator.Text.Trim())
               && string.IsNullOrEmpty(txtYCoordinator.Text.Trim())
               && string.IsNullOrEmpty(txtAddressLine1.Text.Trim())
               && string.IsNullOrEmpty(txtAddressLine2.Text.Trim())
               && string.IsNullOrEmpty(txtStreetAddress.Text.Trim())
               && string.IsNullOrEmpty(txtLevelPrefix.Text.Trim())
               && string.IsNullOrEmpty(txtLevelNbrStart.Text.Trim())
               && string.IsNullOrEmpty(txtLevelNbrEnd.Text.Trim())
               && string.IsNullOrEmpty(txtHouseAlphaStart.Text.Trim())
               && string.IsNullOrEmpty(txtHouseAlphaEnd.Text.Trim()))
            {
                result = true;
            }

            return result;
        }

        /// <summary>
        /// Display template fields.
        /// </summary>
        private void DisplayTemplateFields()
        {
            ITemplateBll templateBll = ObjectFactory.GetObject<ITemplateBll>();
            TemplateAttributeModel[] attributes = templateBll.GetAPOTemplateAttributes(TemplateType.CAP_ADDRESS, ConfigManager.AgencyCode, ACAConstant.ADMIN_CALLER_ID);           
            templateEdit.DisplayAttributes(attributes, ACAConstant.CAP_ADDRESS_TEMPLATE_FIELD_PREFIX);
        }

        /// <summary>
        /// Get street number according to range start and range end.
        /// </summary>
        /// <param name="rangeStart">The range Start</param>
        /// <param name="rangeEnd">The range End</param>
        /// <param name="streetOrUnitNumber">The street or Unit Number</param>
        /// <param name="isSetEmpty">Set street no. or unit no. to Empty when public user enters range search criteria</param>
        /// <returns>The street Number</returns>
        private string GetStreetAndUnitNumber(string rangeStart, string rangeEnd, string streetOrUnitNumber, bool isSetEmpty)
        {
            if (!string.IsNullOrEmpty(rangeStart) || !string.IsNullOrEmpty(rangeEnd))
            {
                if (isSetEmpty)
                {
                    streetOrUnitNumber = string.Empty;
                }
                else
                {
                    streetOrUnitNumber = string.Format("{0}{1}{2}", rangeStart, ACAConstant.SPLIT_CHAR4, rangeEnd);
                }
            }

            return streetOrUnitNumber;
        }

        #endregion Private Methods
    }
}