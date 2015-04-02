#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: FeeList.ascx.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2007-2014
 *
 *  Description:
 *
 *  Notes:
 *      $Id: GeneralSearchForm.ascx.cs 142669 2009-08-10 08:49:02Z ACHIEVO\grady.lu $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using Accela.ACA.Common.Common;

namespace Accela.ACA.Web.Component
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.UI;
    using System.Web.UI.WebControls;
    using Accela.ACA.BLL;
    using Accela.ACA.BLL.Cap;
    using Accela.ACA.BLL.Common;
    using Accela.ACA.Common;
    using Accela.ACA.Common.Util;
    using Accela.ACA.Web.Common;
    using Accela.ACA.Web.Common.Control;
    using Accela.ACA.Web.FormDesigner;
    using Accela.ACA.WSProxy;
    using Accela.Web.Controls;

    /// <summary>
    /// Cap General search section
    /// </summary>
    public partial class GeneralSearchForm : FormDesignerBaseControl
    {
        #region Fields

        /// <summary>
        /// Address description max length.
        /// </summary>
        private const int ADDRESSDESC_MAXLENGTH = 255;

        /// <summary>
        /// address template type
        /// </summary>
        private const string ADDRESS_TEMPLATE_TYPE = "ADDRESS";

        /// <summary>
        /// parcel template type.
        /// </summary>
        private const string PARCEL_TEMPLATE_TYPE = "PARCEL";

        /// <summary>
        /// street name max length.
        /// </summary>
        private const int STREETNAME_MAXLENGTH = 1024;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="GeneralSearchForm"/> class.
        /// </summary>
        public GeneralSearchForm()
            : base(GviewID.GeneralSearch)
        {
        }

        #endregion Constructors

        #region Events

        /// <summary>
        /// PermitType changed event handler.
        /// </summary>
        public event EventHandler PermitTypeChanged;

        /// <summary>
        /// SubAgency changed event handler.
        /// </summary>
        public event EventHandler SubAgencyChanged;

        #endregion Events

        #region Properties

        /// <summary>
        /// Gets or sets StartDate.
        /// </summary>
        public object StartDate
        {
            get
            {
                return txtGSStartDate.Text2;
            }

            set
            {
                txtGSStartDate.Text2 = value;
            }
        }

        /// <summary>
        /// Gets SubAgency dropdownlist.
        /// </summary>
        public AccelaDropDownList SubAgencyDropDownList
        {
            get
            {
                return ddlGSSubAgency;
            }
        }

        /// <summary>
        /// Gets or sets permission.
        /// </summary>
        protected override WSProxy.GFilterScreenPermissionModel4WS Permission
        {
            get
            {
                base.Permission = new GFilterScreenPermissionModel4WS();
                base.Permission.permissionLevel = "CAP_GENERALSEARCH";
                return base.Permission;
            }

            set
            {
                base.Permission = value;
            }
        }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Auto fill city and state.
        /// </summary>
        public void AutoFillCityAndState()
        {
            AutoFillCityAndStateUtil.SetCurrentCity(txtGSCity, ModuleName);
            AutoFillCityAndStateUtil.SetCurrentState(ddlGSState, ModuleName);
        }

        /// <summary>
        /// /// Check whether has least one search criteria has been entered when search by general search.
        /// </summary>
        /// <returns>true or false.</returns>
        public bool CheckInputConditionForGeneral()
        {
            if (CheckStandardFields())
            {
                ShowSearchCriteriaRequiredMessage(GetTextByKey("per_permitList_SearchCriteria_Required"));
                return false;
            }

            if (txtGSStartDate.Visible && txtGSEndDate.Visible && txtGSStartDate.IsLaterThan(txtGSEndDate))
            {
                    string msg = GetTextByKey("per_permitList_msg_date_start_end");
                    ShowSearchCriteriaRequiredMessage(msg);
                    return false;
            }

            return true;
        }

        /// <summary>
        /// fill permit query fields with general search
        /// </summary>
        /// <param name="capModel">a CapModel4WS</param>
        public void FillGeneralSearch(CapModel4WS capModel)
        {
            txtGSPermitNumber.Text = capModel.altID;

            txtGSProjectName.Text = capModel.specialText;

            txtGSStartDate.Text2 = capModel.fileDate;

            txtGSEndDate.Text2 = capModel.endFileDate;

            CapTypeModel capTypeModel = capModel.capType;

            if (capTypeModel != null)
            {
                //fill agency.
                ddlGSSubAgency.SelectedValue = capTypeModel.serviceProviderCode;
                BindPermitType(ddlGSPermitType, ddlGSSubAgency.SelectedValue);

                if (!string.IsNullOrEmpty(ddlGSSubAgency.SelectedValue))
                {
                    ddlGSPermitType.Enabled = true;

                    if (SubAgencyChanged != null)
                    {
                        SubAgencyChanged(null, null);
                    }
                }

                //fill cap type
                if (!string.IsNullOrEmpty(capTypeModel.resAlias) && !string.IsNullOrEmpty(capTypeModel.group))
                {
                    ddlGSPermitType.SelectedValue = CAPHelper.GetCapTypeValue(capTypeModel);
                }

                if (!string.IsNullOrEmpty(ddlGSPermitType.SelectedValue))
                {
                    ICapTypeBll capTypeBll = ObjectFactory.GetObject<ICapTypeBll>();
                    AppStatusGroupModel4WS[] appStatusGroupModels = capTypeBll.GetAppStatusByCapType(GetSelectedCapTypeModel(ddlGSSubAgency.SelectedValue, ddlGSPermitType));
                    DropDownListBindUtil.BindCapStatus(ddlGSCapStatus, ModuleName, appStatusGroupModels);
                    ddlGSCapStatus.Enabled = true;
                    ddlGSCapStatus.SelectedValue = capModel.capStatus;
                }
            }

            if (capModel.licenseProfessionalModel != null)
            {
                ddlGSLicenseType.SelectedValue = capModel.licenseProfessionalModel.licenseType;
                txtGSLicenseNumber.Text = capModel.licenseProfessionalModel.licenseNbr;
                txtGSFirstName.Text = capModel.licenseProfessionalModel.contactFirstName;
                txtGSLastName.Text = capModel.licenseProfessionalModel.contactLastName;
                txtGSBusiName.Text = capModel.licenseProfessionalModel.businessName;
                txtGSBusiName2.Text = capModel.licenseProfessionalModel.busName2;
                txtGSBusiLicense.Text = capModel.licenseProfessionalModel.businessLicense;
                ddlGSContactType.SelectedValue = capModel.licenseProfessionalModel.typeFlag;
                txtGSSSN.Text = capModel.licenseProfessionalModel.socialSecurityNumber;
                txtGSFEIN.Text = capModel.licenseProfessionalModel.fein;
            }

            if (capModel.parcelModel != null && capModel.parcelModel.parcelModel != null)
            {
                txtGSParcelNo.Text = capModel.parcelModel.parcelModel.parcelNumber;
                txtLot.Text = capModel.parcelModel.parcelModel.lot;
                ddlSubdivision.SelectedValue = capModel.parcelModel.parcelModel.subdivision;
            }

            if (capModel.addressModel != null)
            {
                DropDownListBindUtil.SetCountrySelectedValue(ddlGSCountry, capModel.addressModel.countryCode, false, true, true);
                txtGSNumber.TextFrom = capModel.addressModel.houseNumberStartFrom.ToString();
                txtGSNumber.TextTo = capModel.addressModel.houseNumberStartTo.ToString();
                txtGSStreetEnd.TextFrom = capModel.addressModel.houseNumberEndFrom.ToString();
                txtGSStreetEnd.TextTo = capModel.addressModel.houseNumberEndTo.ToString();
                txtGSEndFraction.Text = capModel.addressModel.houseFractionEnd;
                ddlGSDirection.SelectedValue = capModel.addressModel.streetDirection;
                txtGSPrefix.Text = capModel.addressModel.streetPrefix;
                txtGSStreetName.Text = capModel.addressModel.streetName;
                ddlGSStreetSuffix.SelectedValue = capModel.addressModel.streetSuffix;
                txtGSStartFraction.Text = capModel.addressModel.houseFractionStart;
                txtGSStartFraction.Text = capModel.addressModel.houseFractionStart;
                ddlGSStreetSuffixDirection.SelectedValue = capModel.addressModel.streetSuffixdirection;

                ddlGSUnitType.SelectedValue = capModel.addressModel.unitType;
                txtGSUnitNo.Text = GetStreetAndUnitNumber(capModel.addressModel.unitRangeStart, capModel.addressModel.unitRangeEnd, capModel.addressModel.unitStart, false);
                txtGSUnitEnd.Text = capModel.addressModel.unitEnd;

                txtGSSecondaryRoad.Text = capModel.addressModel.secondaryRoad;
                txtGSSecondaryRoadNo.Text = Convert.ToString(capModel.addressModel.secondaryRoadNumber);

                txtGSNeighborhoodP.Text = capModel.addressModel.neighborhoodPrefix;
                txtGSNeighborhood.Text = capModel.addressModel.neighborhood;

                txtGSDescription.Text = capModel.addressModel.addressDescription;

                txtGSDistance.DoubleValue = capModel.addressModel.distance;
                txtGSXCoordinator.DoubleValue = capModel.addressModel.XCoordinator;
                txtGSYCoordinator.DoubleValue = capModel.addressModel.YCoordinator;

                txtGSInspectionDP.Text = capModel.addressModel.inspectionDistrictPrefix;
                txtGSInspectionD.Text = capModel.addressModel.inspectionDistrict;

                txtGSCity.Text = capModel.addressModel.city;
                txtGSCounty.Text = capModel.addressModel.county;
                ddlGSState.Text = capModel.addressModel.state;
                txtGSAppZipSearchPermit.Text = ModelUIFormat.FormatZipShow(capModel.addressModel.zip, capModel.addressModel.countryCode, false);

                txtGSStreetAddress.Text = capModel.addressModel.fullAddress;

                txtGSAddressLine1.Text = capModel.addressModel.addressLine1;
                txtGSAddressLine2.Text = capModel.addressModel.addressLine2;

                txtLevelPrefix.Text = capModel.addressModel.levelPrefix;
                txtLevelNbrStart.Text = capModel.addressModel.levelNumberStart;
                txtLevelNbrEnd.Text = capModel.addressModel.levelNumberEnd;
                txtHouseAlphaStart.Text = capModel.addressModel.houseNumberAlphaStart;
                txtHouseAlphaEnd.Text = capModel.addressModel.houseNumberAlphaEnd;

                if (capModel.addressModel.templates != null && capModel.addressModel.templates.Length > 0)
                {
                    templateSearch.DisplayAttributes(capModel.addressModel.templates, ACAConstant.CAP_GENERAL_SEARCH_TEMPLATE_FIELD_FREFIX);
                }
            }
        }

        /// <summary>
        /// Get CapModel
        /// </summary>
        /// <returns>Cap Model</returns>
        public CapModel4WS GetCapModel4WS()
        {
            CapModel4WS capModel = new CapModel4WS();

            //// parcel model information
            string parcelNum = txtGSParcelNo.Text.Trim();
            CapParcelModel capParcelModel = new CapParcelModel();
            ParcelModel parcelModel = new ParcelModel();
            parcelModel.parcelNumber = parcelNum;
            parcelModel.lot = txtLot.Text.Trim();
            parcelModel.subdivision = ddlSubdivision.SelectedValue.Trim();
            parcelModel.templates = templateSearch.GetAttributeModels(PARCEL_TEMPLATE_TYPE, true);
            capParcelModel.parcelModel = parcelModel;
            capModel.parcelModel = capParcelModel;

            //Permit information
            capModel.altID = txtGSPermitNumber.Text.Trim();
            capModel.specialText = txtGSProjectName.Text.Trim();

            if (txtGSStartDate.Visible && !string.IsNullOrEmpty(txtGSStartDate.Text.Trim()))
            {
                capModel.fileDate = I18nDateTimeUtil.ConvertDateTimeStringFromUIToWebService(txtGSStartDate.Text.Trim());
            }

            if (txtGSEndDate.Visible && !string.IsNullOrEmpty(txtGSEndDate.Text.Trim()))
            {
                capModel.endFileDate = I18nDateTimeUtil.FormatToDateTimeStringForWebService(I18nDateTimeUtil.ParseFromUI(txtGSEndDate.Text.Trim()).AddDays(1).AddSeconds(-1));
            }

            capModel.moduleName = ModuleName;

            CapTypeModel capTypeModel = new CapTypeModel();

            string selectedAgencyCode = StandardChoiceUtil.IsSuperAgency() ? null : ConfigManager.AgencyCode;

            if (!string.IsNullOrEmpty(ddlGSSubAgency.SelectedValue))
            {
                selectedAgencyCode = ddlGSSubAgency.SelectedValue;
            }

            capTypeModel.serviceProviderCode = selectedAgencyCode;

            if (!string.IsNullOrEmpty(ddlGSPermitType.SelectedValue))
            {
                string[] capLevels = ddlGSPermitType.SelectedValue.Trim().Split('/');
                capTypeModel.resAlias = ddlGSPermitType.SelectedItem.Text;
                capTypeModel.moduleName = ModuleName;
                capTypeModel.group = capLevels[0];
                capTypeModel.type = capLevels[1];
                capTypeModel.subType = capLevels[2];
                capTypeModel.category = capLevels[3];
            }

            capModel.capType = capTypeModel;

            if (!string.IsNullOrEmpty(ddlGSCapStatus.SelectedValue))
            {
                capModel.capStatus = ddlGSCapStatus.SelectedValue;
            }

            //License information
            LicenseProfessionalModel licenseModel = new LicenseProfessionalModel();
            licenseModel.licenseType = ddlGSLicenseType.SelectedValue;
            licenseModel.licenseNbr = txtGSLicenseNumber.Text.Trim();
            licenseModel.licenseType = ddlGSLicenseType.SelectedValue.Trim();
            licenseModel.contactFirstName = txtGSFirstName.Text.Trim();
            licenseModel.contactLastName = txtGSLastName.Text.Trim();
            licenseModel.businessName = txtGSBusiName.Text.Trim();
            licenseModel.busName2 = txtGSBusiName2.Text.Trim();
            licenseModel.businessLicense = txtGSBusiLicense.Text.Trim();
            licenseModel.typeFlag = ddlGSContactType.SelectedValue;
            licenseModel.socialSecurityNumber = txtGSSSN.Text.Trim();
            licenseModel.maskedSsn = MaskUtil.FormatSSNShow(txtGSSSN.Text.Trim());
            licenseModel.fein = txtGSFEIN.Text.Trim();
            capModel.licenseProfessionalModel = TempModelConvert.ConvertToLicenseProfessionalModel4WS(licenseModel);

            //Worklocation information
            AddressModel addressModel = new AddressModel();

            addressModel.countryCode = ddlGSCountry.SelectedValue.Trim();
            addressModel.houseFractionEnd = txtGSEndFraction.Text.Trim();
            addressModel.streetDirection = ddlGSDirection.SelectedValue.Trim();
            addressModel.streetPrefix = txtGSPrefix.Text.Trim();
            addressModel.streetName = txtGSStreetName.Text.Trim();
            addressModel.streetSuffix = ddlGSStreetSuffix.SelectedValue.Trim();
            addressModel.houseFractionStart = txtGSStartFraction.Text.Trim();
            addressModel.streetSuffixdirection = ddlGSStreetSuffixDirection.SelectedValue.Trim();
            addressModel.unitType = ddlGSUnitType.SelectedValue.Trim();
            addressModel.unitEnd = txtGSUnitEnd.Text.Trim();
            addressModel.secondaryRoad = txtGSSecondaryRoad.Text.Trim();
            addressModel.secondaryRoadNumber = StringUtil.ToInt(txtGSSecondaryRoadNo.Text.Trim());

            addressModel.neighborhoodPrefix = txtGSNeighborhoodP.Text.Trim();
            addressModel.neighborhood = txtGSNeighborhood.Text.Trim();

            if (txtGSDescription.Text.Trim().Length > ADDRESSDESC_MAXLENGTH)
            {
                addressModel.addressDescription = txtGSDescription.Text.Trim().Substring(0, ADDRESSDESC_MAXLENGTH);
            }
            else
            {
                addressModel.addressDescription = txtGSDescription.Text.Trim();
            }

            addressModel.distance = txtGSDistance.DoubleValue;
            addressModel.XCoordinator = txtGSXCoordinator.DoubleValue;
            addressModel.YCoordinator = txtGSYCoordinator.DoubleValue;

            addressModel.inspectionDistrictPrefix = txtGSInspectionDP.Text.Trim();
            addressModel.inspectionDistrict = txtGSInspectionD.Text.Trim();

            addressModel.city = txtGSCity.Text.Trim();
            addressModel.county = txtGSCounty.Text.Trim();
            addressModel.state = ddlGSState.Text;
            addressModel.zip = txtGSAppZipSearchPermit.GetZip(ddlGSCountry.SelectedValue);

            if (txtGSStreetAddress.Text.Trim().Length > STREETNAME_MAXLENGTH)
            {
                addressModel.fullAddress = txtGSStreetAddress.Text.Trim().Substring(0, STREETNAME_MAXLENGTH);
            }
            else
            {
                addressModel.fullAddress = txtGSStreetAddress.Text.Trim();
            }

            addressModel.addressLine1 = txtGSAddressLine1.Text.Trim();
            addressModel.addressLine2 = txtGSAddressLine2.Text.Trim();

            addressModel.levelPrefix = txtLevelPrefix.Text.Trim();
            addressModel.levelNumberStart = txtLevelNbrStart.Text.Trim();
            addressModel.levelNumberEnd = txtLevelNbrEnd.Text.Trim();
            addressModel.houseNumberAlphaStart = txtHouseAlphaStart.Text.Trim();
            addressModel.houseNumberAlphaEnd = txtHouseAlphaEnd.Text.Trim();

            // address template fields.
            addressModel.templates = templateSearch.GetAttributeModels(ADDRESS_TEMPLATE_TYPE, true);

            if (!string.IsNullOrEmpty(txtGSUnitNo.Text))
            {
                string unitStartText = txtGSUnitNo.Text;

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

            /*
             * If only one parameter of street number range(from/to) inputed, then do not use range search.
             * But use only one parameter to fix search.
             */
            Range<int?> streetNoRange = Range<int>.GetRangeValue(txtGSNumber.TextFrom, txtGSNumber.TextTo);
            addressModel.houseNumberStart = streetNoRange.SingleValue;
            addressModel.houseNumberStartFrom = streetNoRange.LowerBound;
            addressModel.houseNumberStartTo = streetNoRange.UpperBound;

            streetNoRange = Range<int>.GetRangeValue(txtGSStreetEnd.TextFrom, txtGSStreetEnd.TextTo);
            addressModel.houseNumberEnd = streetNoRange.SingleValue;
            addressModel.houseNumberEndFrom = streetNoRange.LowerBound;
            addressModel.houseNumberEndTo = streetNoRange.UpperBound;

            capModel.addressModel = addressModel;
            capModel.moduleName = ModuleName;

            return capModel;
        }

        /// <summary>
        /// Get Cap type.
        /// </summary>
        /// <returns>Cap Type Model</returns>
        public CapTypeModel GetCapType()
        {
            return GetSelectedCapTypeModel(ddlGSSubAgency.SelectedValue, ddlGSPermitType);
        }

        /// <summary>
        /// Get prefix of control.
        /// </summary>
        /// <returns>LastName client id</returns>
        public string GetPrefix()
        {
            return txtGSLastName.ClientID.Replace("txtGSLastName", string.Empty);
        }

        /// <summary>
        /// Gets a Cap type model by cap type drop down list.
        /// </summary>
        /// <param name="ddlAgencySelectedValue">dropdownlist for agency.</param>
        /// <param name="ddlPermitType">dropdownlist for permit type</param>
        /// <returns>a CapTypeModel</returns>
        public CapTypeModel GetSelectedCapTypeModel(string ddlAgencySelectedValue, AccelaDropDownList ddlPermitType)
        {
            if (ddlPermitType == null)
            {
                return null;
            }

            CapTypeModel capTypeModel = new CapTypeModel();

            //permit type isn't "--select--".
            if (!string.IsNullOrEmpty(ddlPermitType.SelectedValue))
            {
                string[] capLevels = ddlPermitType.SelectedValue.Trim().Split('/');

                capTypeModel.resAlias = ddlPermitType.SelectedItem.Text;
                capTypeModel.moduleName = ModuleName;
                capTypeModel.group = capLevels.Length >= 0 ? capLevels[0] : string.Empty;
                capTypeModel.type = capLevels.Length >= 1 ? capLevels[1] : string.Empty;
                capTypeModel.subType = capLevels.Length >= 2 ? capLevels[2] : string.Empty;
                capTypeModel.category = capLevels.Length >= 3 ? capLevels[3] : string.Empty;
            }

            capTypeModel.serviceProviderCode = ConfigManager.AgencyCode;

            if (!string.IsNullOrEmpty(ddlAgencySelectedValue))
            {
                capTypeModel.serviceProviderCode = ddlAgencySelectedValue;
            }

            return capTypeModel;
        }

        /// <summary>
        /// Reset General search form.
        /// </summary>
        /// <param name="endDate">End date</param>
        /// <param name="startDate">Start date</param>
        public void ResetGeneralSearchForm(DateTime endDate, DateTime startDate)
        {
            txtGSProjectName.Text = string.Empty;
            txtGSPermitNumber.Text = string.Empty;
            txtGSLicenseNumber.Text = string.Empty;
            txtGSFirstName.Text = string.Empty;
            txtGSLastName.Text = string.Empty;
            txtGSBusiName.Text = string.Empty;
            txtGSBusiName2.Text = string.Empty;
            txtGSBusiLicense.Text = string.Empty;
            txtLot.Text = string.Empty;
            txtGSParcelNo.Text = string.Empty;
            txtGSNumber.TextFrom = string.Empty;
            txtGSNumber.TextTo = string.Empty;
            txtGSStreetEnd.TextFrom = string.Empty;
            txtGSStreetEnd.TextTo = string.Empty;
            txtGSEndFraction.Text = string.Empty;
            txtGSPrefix.Text = string.Empty;
            txtGSStreetName.Text = string.Empty;
            txtGSStartFraction.Text = string.Empty;
            txtGSStartFraction.Text = string.Empty;
            txtGSUnitNo.Text = string.Empty;
            txtGSUnitEnd.Text = string.Empty;
            txtGSSecondaryRoad.Text = string.Empty;
            txtGSSecondaryRoadNo.Text = string.Empty;
            txtGSNeighborhoodP.Text = string.Empty;
            txtGSNeighborhood.Text = string.Empty;
            txtGSDescription.Text = string.Empty;
            txtGSDistance.Text = string.Empty;
            txtGSXCoordinator.Text = string.Empty;
            txtGSYCoordinator.Text = string.Empty;
            txtGSInspectionDP.Text = string.Empty;
            txtGSInspectionD.Text = string.Empty;
            txtGSCity.Text = string.Empty;
            txtGSCounty.Text = string.Empty;
            ddlGSState.Text = string.Empty;
            txtGSAppZipSearchPermit.Text = string.Empty;
            txtGSStreetAddress.Text = string.Empty;
            txtGSAddressLine1.Text = string.Empty;
            txtGSAddressLine2.Text = string.Empty;

            txtLevelPrefix.Text = string.Empty;
            txtLevelNbrStart.Text = string.Empty;
            txtLevelNbrEnd.Text = string.Empty;
            txtHouseAlphaStart.Text = string.Empty;
            txtHouseAlphaEnd.Text = string.Empty;

            txtGSSSN.Text = string.Empty;
            txtGSFEIN.Text = string.Empty;
            DropDownListBindUtil.SetSelectedToFirstItem(ddlGSUnitType);
            DropDownListBindUtil.SetSelectedToFirstItem(ddlGSDirection);
            DropDownListBindUtil.SetSelectedToFirstItem(ddlGSPermitType);
            DropDownListBindUtil.SetSelectedToFirstItem(ddlGSStreetSuffix);
            DropDownListBindUtil.SetSelectedToFirstItem(ddlGSStreetSuffixDirection);
            DropDownListBindUtil.SetSelectedToFirstItem(ddlGSLicenseType);
            DropDownListBindUtil.SetSelectedToFirstItem(ddlSubdivision);
            DropDownListBindUtil.SetSelectedToFirstItem(ddlGSSubAgency);
            DropDownListBindUtil.SetSelectedToFirstItem(ddlGSContactType);
            DropDownListBindUtil.SetSelectedToFirstItem(ddlGSCapStatus);
            ddlGSCapStatus.Enabled = false;

            txtGSStartDate.Text2 = startDate;
            txtGSEndDate.Text2 = endDate;

            ControlUtil.ClearRegionalSetting(ddlGSCountry, true, string.Empty, null, string.Empty);
        }

        /// <summary>
        /// Initializes GeneralSearch Form
        /// </summary>
        public void InitGeneralSearchForm()
        {
            DropDownListBindUtil.BindSubdivision(ddlSubdivision);
            DropDownListBindUtil.BindSubAgencies(ddlGSSubAgency);
            DropDownListBindUtil.BindLicenseType(ddlGSLicenseType);
            DropDownListBindUtil.BindStreetDirection(ddlGSDirection);
            DropDownListBindUtil.BindStreetSuffix(ddlGSStreetSuffix);
            DropDownListBindUtil.BindUnitType(ddlGSUnitType);
            DropDownListBindUtil.BindContactType4License(ddlGSContactType);
            DropDownListBindUtil.BindStreetDirection(ddlGSStreetSuffixDirection);
            DropDownListBindUtil.BindCapStatus(ddlGSCapStatus, ModuleName, null);

            ddlGSCountry.RegisterScripts();

            if (!ddlGSSubAgency.Visible || !string.IsNullOrEmpty(ddlGSSubAgency.SelectedValue) || AppSession.IsAdmin)
            {
                BindPermitType(ddlGSPermitType, ddlGSSubAgency.SelectedValue);
            }
            else
            {
                DropDownListBindUtil.BindDDL(null, ddlGSPermitType, true, false);
            }

            /*
             * In admin, the permitType field always is enabled.
             * In normal agency, the permitType field always is enabled.
             * In super agency, the permitType field's status controled by subAgency field, see "FillGeneralSearch" method.
             */

            if (StandardChoiceUtil.IsSuperAgency())
            {
                IGviewBll gviewBll = ObjectFactory.GetObject<IGviewBll>();
                bool isGSSubAgencyVisible = gviewBll.IsFieldVisible(ModuleName, Permission, GviewID.GeneralSearch, "ddlGSSubAgency", AppSession.User.UserID);

                if (isGSSubAgencyVisible)
                {
                    ddlGSPermitType.Enabled = false;
                }
            }
            else
            {
                ddlGSPermitType.Enabled = true;
            }
        }

        /// <summary>
        /// Applies the regional setting.
        /// </summary>
        /// <param name="getDefault">if set to <c>true</c> [get default].</param>
        public void ApplyRegionalSetting(bool getDefault)
        {
            ControlUtil.ApplyRegionalSetting(IsPostBack, true, true, getDefault, ddlGSCountry);
        }

        /// <summary>
        /// Initializes ACAMap
        /// </summary>
        /// <param name="e">EventArgs object</param>
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            ddlGSCountry.BindItems();
            ddlGSCountry.SetCountryControls(txtGSAppZipSearchPermit, ddlGSState, null);
        }

        /// <summary>
        /// page load for general search form.
        /// </summary>
        /// <param name="sender">sender object.</param>
        /// <param name="e">event arguments</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            ApplyRegionalSetting(false);

            if (AppSession.IsAdmin)
            {
                //in normal agency admin, agency list only 2 items,'--select--' and current agency,
                bool isNormalAgency = ddlGSSubAgency.Items.Count == 2 && ddlGSSubAgency.Items[1].Value == ConfigManager.AgencyCode;
                ddlGSSubAgency.AutoPostBack = !isNormalAgency;
            }
            else
            {
                //in ACA super agency daily, Agency/CAP type drop down list can trig server event method, in ACA admin it is needn't trig server event method.
                ddlGSSubAgency.AutoPostBack = StandardChoiceUtil.IsSuperAgency();
                ddlGSPermitType.AutoPostBack = true;
            }

            ControlBuildHelper.HideStandardFields(GviewID.GeneralSearch, ModuleName, Controls, AppSession.IsAdmin, Permission);

            phContent.TemplateControlIDPrefix = ACAConstant.CAP_GENERAL_SEARCH_TEMPLATE_FIELD_FREFIX;
            InitFormDesignerPlaceHolder(phContent);
        }

        /// <summary>
        /// GSPermitType Dropdown List IndexChange
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">A System.EventArgs object containing the event data.</param>
        protected void GSPermitType_IndexChange(object sender, EventArgs e)
        {
            //if super agency admin, it needn't trigger event.
            if (AppSession.IsAdmin)
            {
                return;
            }

            if (string.IsNullOrEmpty(ddlGSPermitType.SelectedValue))
            {
                ddlGSCapStatus.Enabled = false;
                DropDownListBindUtil.BindCapStatus(ddlGSCapStatus, ModuleName, null);
            }
            else
            {
                ddlGSCapStatus.Enabled = true;
                ICapTypeBll capTypeBll = (ICapTypeBll)ObjectFactory.GetObject(typeof(ICapTypeBll));
                AppStatusGroupModel4WS[] appStatusGroupModels = capTypeBll.GetAppStatusByCapType(GetSelectedCapTypeModel(ddlGSSubAgency.SelectedValue, ddlGSPermitType));
                DropDownListBindUtil.BindCapStatus(ddlGSCapStatus, ModuleName, appStatusGroupModels);
                Page.FocusElement(ddlGSPermitType.ClientID);
            }

            if (PermitTypeChanged != null)
            {
                PermitTypeChanged(sender, e);
            }
        }

        /// <summary>
        /// GSSubAgency Dropdown List IndexChanged
        /// </summary>
        /// <param name="sender">An object that contains the event sender.</param>
        /// <param name="e">A System.EventArgs object containing the event data.</param>
        protected void GSSubAgency_IndexChanged(object sender, EventArgs e)
        {
            //if normal agency, it needn't trigger event.
            if (!StandardChoiceUtil.IsSuperAgency() && !AppSession.IsAdmin)
            {
                return;
            }

            BindPermitType(ddlGSPermitType, ddlGSSubAgency.SelectedValue);

            //in ACA admin, need to bind CAP type by agency code, and hide button area.
            if (AppSession.IsAdmin)
            {
                ControlBuildHelper.HideStandardFields(GviewID.GeneralSearch, ModuleName, Controls, AppSession.IsAdmin, Permission);
                return;
            }

            //not-selected any agency.
            if (string.IsNullOrEmpty(ddlGSSubAgency.SelectedValue))
            {
                ddlGSCapStatus.Enabled = false;
                ddlGSPermitType.Enabled = false;
            }
            else
            {
                ddlGSPermitType.Enabled = true;
            }

            if (SubAgencyChanged != null)
            {
                SubAgencyChanged(sender, e);
            }

            Page.FocusElement(ddlGSSubAgency.ClientID);
        }

        /// <summary>
        /// Bind cap type and display them on the screen
        /// </summary>
        /// <param name="ddlPermit">AccelaDropDownList for permit.</param>
        /// <param name="agencyCode">the agency code.</param>
        private void BindPermitType(AccelaDropDownList ddlPermit, string agencyCode)
        {
            if (ddlPermit == null)
            {
                return;
            }

            CapTypeModel[] permitTypelist = GetCapTypeList(agencyCode);

            IList<ListItem> lstPermitType = new List<ListItem>();

            if (permitTypelist != null)
            {
                SortedList sortedList = new SortedList();
                foreach (CapTypeModel typemodel in permitTypelist)
                {
                    //sortedList[typemodel.alias] = typemodel;
                    string key = CAPHelper.GetAliasOrCapTypeLabel(typemodel);

                    if (sortedList.ContainsKey(key))
                    {
                        key = key + CAPHelper.GetCapTypeValue(typemodel);
                    }

                    if (!sortedList.ContainsKey(key))
                    {
                        sortedList[key] = typemodel;
                    }
                }

                foreach (DictionaryEntry de in sortedList)
                {
                    CapTypeModel typemodel = (CapTypeModel)de.Value;
                    ListItem item = new ListItem();

                    item.Text = CAPHelper.GetAliasOrCapTypeLabel(typemodel);
                    item.Value = CAPHelper.GetCapTypeValue(typemodel);
                    lstPermitType.Add(item);
                }
            }

            ddlPermit.Items.Clear();
            DropDownListBindUtil.BindDDL(lstPermitType, ddlPermit, true, false);
        }

        /// <summary>
        /// Check standard fields
        /// </summary>
        /// <returns>return true if all field is empty</returns>
        private bool CheckStandardFields()
        {
            if (txtGSAppZipSearchPermit.Text.Trim().EndsWith("-", StringComparison.InvariantCulture))
            {
                txtGSAppZipSearchPermit.Text = txtGSAppZipSearchPermit.Text.Trim().Replace("-", string.Empty);
            }

            if (string.IsNullOrEmpty(txtGSBusiName.Text.Trim())
                && string.IsNullOrEmpty(txtGSBusiName2.Text.Trim())
                && string.IsNullOrEmpty(txtGSBusiLicense.Text.Trim())
                && string.IsNullOrEmpty(txtGSCity.Text.Trim())
                && (!txtGSEndDate.Visible || string.IsNullOrEmpty(txtGSEndDate.Text.Trim()))
                && string.IsNullOrEmpty(txtGSFirstName.Text.Trim())
                && string.IsNullOrEmpty(txtGSLastName.Text.Trim())
                && string.IsNullOrEmpty(txtGSPermitNumber.Text.Trim())
                && string.IsNullOrEmpty(txtGSProjectName.Text.Trim())
                && (!txtGSStartDate.Visible || string.IsNullOrEmpty(txtGSStartDate.Text.Trim()))
                && string.IsNullOrEmpty(txtGSStreetName.Text.Trim())
                && string.IsNullOrEmpty(txtGSNumber.GetValue())
                && string.IsNullOrEmpty(txtGSUnitNo.Text.Trim())
                && string.IsNullOrEmpty(txtLot.Text.Trim())
                && string.IsNullOrEmpty(txtGSStartFraction.Text.Trim())
                && string.IsNullOrEmpty(txtGSStreetEnd.GetValue())
                && string.IsNullOrEmpty(txtGSEndFraction.Text.Trim())
                && string.IsNullOrEmpty(txtGSPrefix.Text.Trim())
                && string.IsNullOrEmpty(txtGSUnitEnd.Text.Trim())
                && string.IsNullOrEmpty(txtGSSecondaryRoad.Text.Trim())
                && string.IsNullOrEmpty(txtGSSecondaryRoadNo.Text.Trim())
                && string.IsNullOrEmpty(txtGSNeighborhoodP.Text.Trim())
                && string.IsNullOrEmpty(txtGSNeighborhood.Text.Trim())
                && string.IsNullOrEmpty(txtGSDescription.Text.Trim())
                && string.IsNullOrEmpty(txtGSDistance.Text.Trim())
                && string.IsNullOrEmpty(txtGSXCoordinator.Text.Trim())
                && string.IsNullOrEmpty(txtGSYCoordinator.Text.Trim())
                && string.IsNullOrEmpty(txtGSInspectionDP.Text.Trim())
                && string.IsNullOrEmpty(txtGSInspectionD.Text.Trim())
                && string.IsNullOrEmpty(txtGSCounty.Text.Trim())
                && string.IsNullOrEmpty(txtGSStreetAddress.Text.Trim())
                && string.IsNullOrEmpty(txtGSAddressLine1.Text.Trim())
                && string.IsNullOrEmpty(txtGSAddressLine2.Text.Trim())
                && string.IsNullOrEmpty(txtLevelPrefix.Text.Trim())
                && string.IsNullOrEmpty(txtLevelNbrStart.Text.Trim())
                && string.IsNullOrEmpty(txtLevelNbrEnd.Text.Trim())
                && string.IsNullOrEmpty(txtHouseAlphaStart.Text.Trim())
                && string.IsNullOrEmpty(txtHouseAlphaEnd.Text.Trim())
                && ddlGSUnitType.SelectedIndex == 0
                && ddlGSStreetSuffix.SelectedIndex == 0
                && ddlGSStreetSuffixDirection.SelectedIndex == 0
                && ddlGSState.Text == string.Empty
                && ddlGSCountry.SelectedIndex < 1
                && ddlGSPermitType.SelectedIndex == 0
                && ddlGSLicenseType.SelectedIndex == 0
                && ddlGSDirection.SelectedIndex == 0
                && ddlSubdivision.SelectedIndex == 0
                && ddlGSSubAgency.SelectedIndex == 0
                && ddlGSContactType.SelectedIndex == 0                
                && string.IsNullOrEmpty(txtGSLicenseNumber.Text.Trim())
                && string.IsNullOrEmpty(txtGSParcelNo.Text.Trim())
                && string.IsNullOrEmpty(txtGSAppZipSearchPermit.Text.Trim())
                && string.IsNullOrEmpty(txtGSSSN.Text.Trim())
                && string.IsNullOrEmpty(txtGSFEIN.Text.Trim())
                && ddlGSCapStatus.SelectedIndex == 0)
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Construct Cap Type Permission Model
        /// </summary>
        /// <param name="capTypePermission">the CapTypePermission model</param>
        /// <returns>return the CapTypePermission model</returns>
        private CapTypePermissionModel ConstructCapTypePermissionModel(CapTypePermissionModel capTypePermission)
        {
            CapTypePermissionModel newCapTypePermission = new CapTypePermissionModel();
            newCapTypePermission.serviceProviderCode = capTypePermission.serviceProviderCode;
            newCapTypePermission.moduleName = capTypePermission.moduleName;
            newCapTypePermission.group = capTypePermission.group;
            newCapTypePermission.type = capTypePermission.type;
            newCapTypePermission.subType = capTypePermission.subType;
            newCapTypePermission.category = capTypePermission.category;
            newCapTypePermission.controllerType = ControllerType.CAPSEARCHFILTER.ToString();
            newCapTypePermission.entityType = EntityType.LICENSETYPE.ToString();

            return newCapTypePermission;
        }

        /// <summary>
        /// Filter Cap Type List
        /// </summary>
        /// <param name="capTypePermissionList">the capTypePermission model List</param>
        /// <returns>The capTypePermission model list. </returns>
        private CapTypeModel[] FilterCapTypeList(CapTypePermissionModel[] capTypePermissionList)
        {
            var userRoleBll = ObjectFactory.GetObject<IUserRoleBll>();
            ICapTypePermissionBll capTypePermissionBll = (ICapTypePermissionBll)ObjectFactory.GetObject(typeof(ICapTypePermissionBll));
            UserRolePrivilegeModel userRoleModel = new UserRolePrivilegeModel();
            IList<CapTypeModel> capTypeList = new List<CapTypeModel>();

            for (int i = 0; i < capTypePermissionList.Length; i++)
            {
                CapTypeModel capType = new CapTypeModel();

                userRoleModel = userRoleBll.ConvertToUserRolePrivilegeModel(string.IsNullOrEmpty(capTypePermissionList[i].entityPermission) ? ACAConstant.DEFAULT_PERMISSION : capTypePermissionList[i].entityPermission);
                CapTypePermissionModel lpTypeModel = ConstructCapTypePermissionModel(capTypePermissionList[i]);

                CapTypePermissionModel[] capTypePermissions = capTypePermissionBll.GetCapTypePermissions(capTypePermissionList[i].serviceProviderCode, lpTypeModel);

                ArrayList lpTypes = new ArrayList();
                if (capTypePermissions != null && capTypePermissions.Length > 0)
                {
                    foreach (CapTypePermissionModel model in capTypePermissions)
                    {
                        if (model.entityType == EntityType.LICENSETYPE.ToString())
                        {
                            lpTypes.Add(model.entityKey1);
                        }
                    }
                }

                userRoleModel.licenseTypeRuleArray = (string[])lpTypes.ToArray(typeof(string));

                if (userRoleBll.IsCapTypeHasRight(userRoleModel, false))
                {
                    //The alias in capTypePermissionList equals disAlias in CapTypeModel.
                    capType.resAlias = capTypePermissionList[i].alias;
                    capType.moduleName = capTypePermissionList[i].moduleName;
                    capType.group = capTypePermissionList[i].group;
                    capType.type = capTypePermissionList[i].type;
                    capType.subType = capTypePermissionList[i].subType;
                    capType.category = capTypePermissionList[i].category;
                    capType.serviceProviderCode = capTypePermissionList[i].serviceProviderCode;

                    capTypeList.Add(capType);
                }
            }

            if (capTypeList.Count > 0)
            {
                ICapTypeBll capTypeBll = ObjectFactory.GetObject<ICapTypeBll>();
                return capTypeBll.GetCapTypeListByPKs(capTypeList.ToArray());
            }

            return null;
        }

        /// <summary>
        /// Get Cap Type List.
        /// </summary>
        /// <param name="agencyCode">The agency code.</param>
        /// <returns>The Cap Type list.</returns>
        private CapTypeModel[] GetCapTypeList(string agencyCode)
        {
            //if agency drop down seleced null, get web config agency code.
            if (string.IsNullOrEmpty(agencyCode))
            {
                agencyCode = ConfigManager.AgencyCode;
            }

            bool isCurrentAgency = agencyCode.Equals(ConfigManager.AgencyCode, StringComparison.InvariantCulture);

            CapTypeModel[] captypes = null;

            IXPolicyBll xPolicyBll = ObjectFactory.GetObject<IXPolicyBll>();
            XpolicyUserRolePrivilegeModel policy = xPolicyBll.GetPolicy(agencyCode, ACAConstant.USER_ROLE_POLICY_FOR_CAP_SEARCH, ACAConstant.LEVEL_TYPE_MODULE, ModuleName);

            if (!AppSession.IsAdmin && isCurrentAgency && policy != null)
            {
                bool isCapTypeLevel = ACAConstant.COMMON_ONE.Equals(policy.data4);

                if (isCapTypeLevel)
                {
                    ICapTypePermissionBll capTypePermissionBll = (ICapTypePermissionBll)ObjectFactory.GetObject(typeof(ICapTypePermissionBll));

                    CapTypePermissionModel capTypePermission = new CapTypePermissionModel();
                    capTypePermission.serviceProviderCode = agencyCode;
                    capTypePermission.controllerType = ControllerType.CAPSEARCHFILTER.ToString();
                    capTypePermission.entityType = EntityType.GENERAL.ToString();
                    capTypePermission.moduleName = ModuleName;

                    CapTypePermissionModel[] capTypePermissionList = capTypePermissionBll.GetPermissionByControllerType(agencyCode, capTypePermission);

                    if (capTypePermissionList != null && capTypePermissionList.Length > 0)
                    {
                        captypes = new CapTypeModel[capTypePermissionList.Length];
                        captypes = FilterCapTypeList(capTypePermissionList);
                    }
                }
                else
                {
                    var userRoleBll = ObjectFactory.GetObject<IUserRoleBll>();
                    UserRolePrivilegeModel userRoleModel = new UserRolePrivilegeModel();
                    userRoleModel = userRoleBll.ConvertToUserRolePrivilegeModel(policy.data3);

                    if (userRoleBll.IsCapTypeHasRight(userRoleModel, true))
                    {
                        ICapTypeBll capTypeBll = (ICapTypeBll)ObjectFactory.GetObject(typeof(ICapTypeBll));
                        CapTypeModel[] capTypeList = capTypeBll.GetCapTypeList(agencyCode, ModuleName, null);

                        if (capTypeList != null && capTypeList.Length > 0)
                        {
                            captypes = new CapTypeModel[capTypeList.Length];
                            captypes = capTypeList;
                        }
                    }
                }
            }
            else
            {
                ICapTypeBll capTypeBll = (ICapTypeBll)ObjectFactory.GetObject(typeof(ICapTypeBll));
                CapTypeModel[] capTypeList = capTypeBll.GetCapTypeList(agencyCode, ModuleName, null);

                if (capTypeList != null && capTypeList.Length > 0)
                {
                    captypes = new CapTypeModel[capTypeList.Length];
                    captypes = capTypeList;
                }
            }

            return captypes;
        }

        /// <summary>
        /// get street number according to range start and range end.
        /// </summary>
        /// <param name="rangeStart">the range Start</param>
        /// <param name="rangeEnd">the range End</param>
        /// <param name="streetOrUnitNumber">the street or Unit Number</param>
        /// <param name="isSetEmpty">Set street no. or unit no. to Empty when public user enters range search criteria</param>
        /// <returns>the street Number</returns>
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

        /// <summary>
        /// Show Error message and scroll to this message.
        /// </summary>
        /// <param name="message">the error message.</param>
        private void ShowSearchCriteriaRequiredMessage(string message)
        {
            MessageUtil.ShowMessageByControl(Page, MessageType.Error, message);
            ScriptManager.RegisterClientScriptBlock(Page, GetType(), "GotoMessageBar", "scrollIntoView('messageSpan');", true);
        }

        #endregion Methods
    }
}