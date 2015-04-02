#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: RefLicenseSearchForm.aspx.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2008-2014
 *
 *  Description:
 *
 *  Notes:
 *      $Id: RefLicenseeSearchForm.ascx.cs 277971 2014-08-25 06:10:06Z ACHIEVO\james.shi $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using Accela.ACA.BLL.People;
using Accela.ACA.Common;
using Accela.ACA.Common.Util;
using Accela.ACA.Web.Common;
using Accela.ACA.Web.Common.Control;
using Accela.ACA.Web.FormDesigner;
using Accela.ACA.WSProxy;
using Accela.Web.Controls;

namespace Accela.ACA.Web.Component
{
    /// <summary>
    /// search reference license form.
    /// </summary>
    public partial class RefLicenseeSearchForm : FormDesignerBaseControl
    {
        #region Fields

        /// <summary>
        /// provider kay.
        /// </summary>
        private const string PROVIDER = "provider";

        /// <summary>
        /// licensee kay.
        /// </summary>
        private const string LICENSEE = "licensee";

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the RefLicenseeSearchForm class.
        /// </summary>
        public RefLicenseeSearchForm()
            : base(GviewID.SearchForLicensee)
        {
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// Gets or sets the search type with license or food facility
        /// </summary>
        public GeneralInformationSearchType SearchType
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets Permission
        /// </summary>
        protected override GFilterScreenPermissionModel4WS Permission
        {
            get
            {
                base.Permission = new GFilterScreenPermissionModel4WS();

                if (SearchType == GeneralInformationSearchType.Search4FoodFacilityInspection)
                {
                    base.Permission.permissionLevel = "SEARCH_FOR_FOOD_FACILITY";
                }
                else
                {                    
                    base.Permission.permissionLevel = "SEARCH_FOR_LICENSEE";
                }
                
                return base.Permission;
            }

            set
            {
                base.Permission = value;
            }
        }

        /// <summary>
        /// Gets all phone control id.
        /// </summary>
        private string RelevantControlIDs
        {
            get
            {
                StringBuilder sbControls = new StringBuilder();
                sbControls.Append(txtPhone1.ClientID);
                sbControls.Append(",").Append(txtPhone2.ClientID);
                sbControls.Append(",").Append(txtFax.ClientID);

                return sbControls.ToString();
            }
        }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Initial search form by license.
        /// </summary>
        public void InitLicenseForm()
        {
            txtCity.Text = string.Empty;
            txtState.Text = string.Empty;
            txtLicenseState.Text = string.Empty;
            txtZipCode.Text = string.Empty;
            txtAddress1.Text = string.Empty;
            txtAddress2.Text = string.Empty;
            txtAddress3.Text = string.Empty;
            txtBusiName.Text = string.Empty;
            txtBusiName2.Text = string.Empty;
            txtBusiLicense.Text = string.Empty;
            txtFirstName.Text = string.Empty;
            txtMiddleInitial.Text = string.Empty;
            txtLastName.Text = string.Empty;
            txtLicenseNumber.Text = string.Empty;
            txtProviderName.Text = string.Empty;
            txtProviderNumber.Text = string.Empty;
            txtFax.Text = string.Empty;
            txtFax.CountryCodeText = string.Empty;
            txtPhone1.Text = string.Empty;
            txtPhone1.CountryCodeText = string.Empty;
            txtPhone2.Text = string.Empty;
            txtPhone2.CountryCodeText = string.Empty;
            txtSSN.Text = string.Empty;
            txtFEIN.Text = string.Empty;
            txtContractorLicNO.Text = string.Empty;
            txtContractorBusiName.Text = string.Empty;
            DropDownListBindUtil.BindLicenseType(ddlLicenseType);
            txtTitle.Text = string.Empty;
            txtInsuranceCompany.Text = string.Empty;
            txtInsurancePolicy.Text = string.Empty;
            txtZipCode.ClearValue();

            if (SearchType == GeneralInformationSearchType.Search4FoodFacilityInspection)
            {
                DropDownListBindUtil.BindLincenseType4FoodFacility(ddlLicenseType);
            }

            DropDownListBindUtil.BindLicensingBoard(ddlLicensingBoard);
            DropDownListBindUtil.BindContactType4License(ddlContactType);

            ControlUtil.ClearRegionalSetting(ddlCountry, true, string.Empty, null, string.Empty);
            ddlCountry.RelevantControlIDs = RelevantControlIDs;
            ddlCountry.RegisterScripts();
        }

        /// <summary>
        /// Show refLicense info.
        /// </summary>
        /// <param name="userInputData">Provider and license information</param>
        public void ShowLicenseInfo(Hashtable userInputData)
        {
            ProviderModel4WS provider = (ProviderModel4WS)userInputData[PROVIDER];
            LicenseModel4WS license = (LicenseModel4WS)userInputData[LICENSEE];

            if (license != null)
            {
                DropDownListBindUtil.SetCountrySelectedValue(ddlCountry, license.countryCode, false, true, true);
                ddlLicenseType.SelectedValue = license.licenseType;
                txtLicenseNumber.Text = license.stateLicense;
                ddlContactType.SelectedValue = license.typeFlag;
                txtSSN.Text = license.socialSecurityNumber;
                txtFEIN.Text = license.fein;
                ddlLicensingBoard.SelectedValue = license.licenseBoard;

                txtBusiName.Text = license.businessName;
                txtBusiName2.Text = license.busName2;
                txtBusiLicense.Text = license.businessLicense;
                txtFirstName.Text = license.contactFirstName;
                txtMiddleInitial.Text = license.contactMiddleName;
                txtLastName.Text = license.contactLastName;

                txtAddress1.Text = license.address1;
                txtAddress2.Text = license.address2;
                txtAddress3.Text = license.address3;

                txtCity.Text = license.city;
                txtState.Text = license.state;
                txtLicenseState.Text = license.licState;
                txtZipCode.Text = ModelUIFormat.FormatZipShow(license.zip, license.countryCode, false);

                txtPhone1.CountryCodeText = license.phone1CountryCode;
                txtPhone1.Text = ModelUIFormat.FormatPhone4EditPage(license.phone1, license.countryCode);
                txtPhone2.CountryCodeText = license.phone2CountryCode;
                txtPhone2.Text = ModelUIFormat.FormatPhone4EditPage(license.phone2, license.countryCode);
                txtFax.CountryCodeText = license.faxCountryCode;
                txtFax.Text = ModelUIFormat.FormatPhone4EditPage(license.fax, license.countryCode);

                txtContractorLicNO.Text = license.contrLicNo;
                txtContractorBusiName.Text = license.contLicBusName;
                txtTitle.Text = license.title;
                txtInsuranceCompany.Text = license.insuranceCo;
                txtInsurancePolicy.Text = license.policy;
            }

            if (provider != null)
            {
                txtProviderName.Text = provider.providerName;
                txtProviderNumber.Text = provider.providerNo;
            }
        }

        /// <summary>
        /// Applies the regional setting.
        /// </summary>
        public void ApplyRegionalSetting()
        {
            ControlUtil.ApplyRegionalSetting(IsPostBack, true, true, false, ddlCountry);
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
        /// Get license list according to user input.
        /// </summary>
        /// <param name="queryFormat">The query format.</param>
        /// <returns>LicenseModel4WS list</returns>
        public IList<LicenseModel4WS> SearchLicensee(QueryFormat queryFormat)
        {
            IList<LicenseModel4WS> licenseList = new List<LicenseModel4WS>();
            ProviderModel4WS provider = GetProviderInformation();
            LicenseModel4WS license = GetLicenseInformation();
            bool isFoodFacility = SearchType == GeneralInformationSearchType.Search4FoodFacilityInspection;

            ILicenseBLL licenseBll = (ILicenseBLL)ObjectFactory.GetObject(typeof(ILicenseBLL));
            LicenseModel4WS[] refLicesneList = licenseBll.GetLicenseList(license, provider, isFoodFacility, queryFormat);

            if (refLicesneList != null && refLicesneList.Length > 0)
            {
                foreach (LicenseModel4WS licensee in refLicesneList)
                {
                    if (licensee != null)
                    {
                        licenseList.Add(licensee);
                    }
                }
            }

            return licenseList;
        }

        /// <summary>
        /// Packaging provider and licensee information.
        /// </summary>
        /// <returns>Hashtable value</returns>
        public Hashtable GetSearchCondition()
        {
            Hashtable userInputData = new Hashtable();
            userInputData.Add(PROVIDER, GetProviderInformation());
            userInputData.Add(LICENSEE, GetLicenseInformation());

            return userInputData;
        }

        /// <summary>
        /// Raises the initial event
        /// </summary>
        /// <param name="e">A System.EventArgs object containing the event data.</param>
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            // set the validation
            string sectionId = string.Empty;
            if (SearchType == GeneralInformationSearchType.Search4FoodFacilityInspection)
            {
                sectionId = GviewID.SearchForFoodFacility;
            }
            else
            {
                sectionId = GviewID.SearchForLicensee;
            }

            ddlCountry.BindItems();
            ddlCountry.SetCountryControls(txtZipCode, new[] { txtState, txtLicenseState }, txtPhone1, txtPhone2, txtFax);

            ControlBuildHelper.AddValidationForStandardFields(sectionId, ModuleName, Permission, Controls);
        }

        /// <summary>
        /// Raises the page load event
        /// </summary>
        /// <param name="sender">An object that contains the event sender.</param>
        /// <param name="e">A System.EventArgs object containing the event data.</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (SearchType == GeneralInformationSearchType.Search4Licensee && !AppSession.IsAdmin)
            {
                ddlLicensingBoard.SelectedIndexChanged += LicensingBoardDropdown_SelectedIndexChanged;
            }

            if (StandardChoiceUtil.IsLicensingBoardRequired())
            {
                ddlLicensingBoard.Required = true;
            }

            ApplyRegionalSetting();

            // initital form designer
            if (SearchType == GeneralInformationSearchType.Search4FoodFacilityInspection)
            {
                ViewId = GviewID.SearchForFoodFacility;
            }
            else
            {
                ViewId = GviewID.SearchForLicensee;
            }

            // it should execute both first load and postback
            ChangeLabelkey(SearchType);
        }

        /// <summary>
        /// Raises the PreRender event
        /// </summary>
        /// <param name="e">A System.EventArgs object containing the event data.</param>
        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);

            foreach (Control ctrl in phContent.Controls)
            {
                ctrl.Visible = true;
            }

            InitFormDesignerPlaceHolder(phContent);
        }

        /// <summary>
        /// Render event method.
        /// </summary>
        /// <param name="writer">writer string.</param>
        protected override void Render(HtmlTextWriter writer)
        {
            if (!IsPostBack && !AppSession.IsAdmin)
            {
                SetCurrentCityAndState();
            }

            if (SearchType == GeneralInformationSearchType.Search4Licensee && !AppSession.IsAdmin)
            {
                ddlLicensingBoard.AutoPostBack = ddlLicenseType.Visible;
            }

            base.Render(writer);
        }

        /// <summary>
        /// Handle the licensing board dropdownlist selected changed event.
        /// </summary>
        /// <param name="sender">The sender object</param>
        /// <param name="e">The event arguments</param>
        protected void LicensingBoardDropdown_SelectedIndexChanged(object sender, EventArgs e)
        {
            string licensingBoard = ddlLicensingBoard.SelectedValue;
            string preSelectedLicType = ddlLicenseType.SelectedValue;

            // Bind the license type and set the previous selected license type.
            DropDownListBindUtil.BindLicenseType(ddlLicenseType, ModuleName, licensingBoard);

            // reset the previous selected license type
            ListItem selectedItem = ddlLicenseType.Items.FindByValue(preSelectedLicType);

            if (selectedItem != null)
            {
                ddlLicenseType.SelectedValue = preSelectedLicType;
            }
            else
            {
                ddlLicenseType.SelectedIndex = -1;
            }

            Page.FocusElement(ddlLicensingBoard.ClientID);
        }

        /// <summary>
        /// Get provider information according to user input.
        /// </summary>
        /// <returns>Provider Model</returns>
        private ProviderModel4WS GetProviderInformation()
        {
            ProviderModel4WS provider = new ProviderModel4WS();
            provider.providerName = txtProviderName.Text.Trim();
            provider.providerNo = txtProviderNumber.Text.Trim();

            return provider;
        }

        /// <summary>
        /// Get license information according to user input
        /// </summary>
        /// <returns>License Model</returns>
        private LicenseModel4WS GetLicenseInformation()
        {
            LicenseModel4WS license = new LicenseModel4WS();
            license.city = txtCity.Text.Trim();
            license.state = txtState.Text.Trim();
            license.zip = txtZipCode.GetZip(this.ddlCountry.SelectedValue.Trim());
            license.address1 = txtAddress1.Text.Trim();
            license.address2 = txtAddress2.Text.Trim();
            license.address3 = txtAddress3.Text.Trim();
            license.countryCode = ddlCountry.SelectedValue;
            license.businessName = txtBusiName.Text.Trim();
            license.busName2 = txtBusiName2.Text.Trim();
            license.businessLicense = txtBusiLicense.Text.Trim();
            license.contactFirstName = txtFirstName.Text.Trim();
            license.contactMiddleName = txtMiddleInitial.Text.Trim();
            license.contactLastName = txtLastName.Text.Trim();
            license.licState = txtLicenseState.Text.Trim();
            license.stateLicense = txtLicenseNumber.Text.Trim();
            license.licenseBoard = ddlLicensingBoard.SelectedValue;
            license.fax = txtFax.GetPhone(this.ddlCountry.SelectedValue.Trim());
            license.faxCountryCode = txtFax.CountryCodeText.Trim();
            license.phone1 = txtPhone1.GetPhone(this.ddlCountry.SelectedValue.Trim());
            license.phone1CountryCode = txtPhone1.CountryCodeText.Trim();
            license.phone2 = txtPhone2.GetPhone(this.ddlCountry.SelectedValue.Trim());
            license.phone2CountryCode = txtPhone2.CountryCodeText.Trim();
            license.licenseType = ddlLicenseType.SelectedValue;
            license.typeFlag = ddlContactType.SelectedValue;
            license.socialSecurityNumber = txtSSN.Text.Trim();
            license.maskedSsn = MaskUtil.FormatSSNShow(txtSSN.Text.Trim());
            license.fein = txtFEIN.Text.Trim();
            license.serviceProviderCode = ConfigManager.AgencyCode;
            license.contrLicNo = txtContractorLicNO.Text.Trim();
            license.contLicBusName = txtContractorBusiName.Text.Trim();
            license.acaPermission = ACAConstant.COMMON_Y;

            license.title = txtTitle.Text.Trim();
            license.insuranceCo = txtInsuranceCompany.Text.Trim();
            license.policy = txtInsurancePolicy.Text.Trim();
            return license;
        }

        /// <summary>
        /// Change the label key
        /// </summary>
        /// <param name="searchType">The search type.</param>
        private void ChangeLabelkey(GeneralInformationSearchType searchType)
        {
            switch (searchType)
            {
                case GeneralInformationSearchType.Search4Licensee:
                    ddlLicenseType.LabelKey = "aca_licensee_licenseType";
                    txtLicenseNumber.LabelKey = "aca_licensee_stateLicenseNum";
                    txtLicenseState.LabelKey = "aca_licensee_licensestate";
                    ddlContactType.LabelKey = "aca_licensee_contacttype";
                    txtSSN.LabelKey = "aca_licensee_ssn";
                    txtFEIN.LabelKey = "aca_licensee_fein";
                    txtProviderName.LabelKey = "aca_licensee_providername";
                    txtProviderNumber.LabelKey = "aca_licensee_providernumber";
                    ddlLicensingBoard.LabelKey = "aca_licensee_licensingboard";
                    txtFirstName.LabelKey = "aca_licensee_firstname";
                    txtMiddleInitial.LabelKey = "aca_licensee_middleinitial";
                    txtLastName.LabelKey = "aca_licensee_lastname";
                    txtBusiName.LabelKey = "aca_licensee_businessname";
                    txtBusiName2.LabelKey = "aca_licensee_businessname2";
                    txtBusiLicense.LabelKey = "aca_licensee_businesslicense";
                    ddlCountry.LabelKey = "aca_licensee_country";
                    txtAddress1.LabelKey = "aca_licensee_address1";
                    txtAddress2.LabelKey = "aca_licensee_address2";
                    txtAddress3.LabelKey = "aca_licensee_address3";
                    txtCity.LabelKey = "aca_licensee_city";
                    txtState.LabelKey = "aca_licensee_state";
                    txtZipCode.LabelKey = "aca_licensee_zip";
                    txtPhone1.LabelKey = "aca_licensee_phone1";
                    txtPhone2.LabelKey = "aca_licensee_phone2";
                    txtFax.LabelKey = "aca_licensee_fax";
                    txtContractorLicNO.LabelKey = "aca_licensee_contractorlicno";
                    txtContractorBusiName.LabelKey = "aca_licensee_contractorbusiname";

                    txtTitle.LabelKey = "aca_licensee_title";
                    txtInsuranceCompany.LabelKey = "aca_licensee_insurancecompany";
                    txtInsurancePolicy.LabelKey = "aca_licensee_insurancepolicy";
                    break;
                case GeneralInformationSearchType.Search4FoodFacilityInspection:
                    ddlLicenseType.LabelKey = "aca_foodfacility_label_licensetype";
                    txtLicenseNumber.LabelKey = "aca_foodfacility_label_statelicensenum";
                    txtLicenseState.LabelKey = "aca_foodfacility_label_licensestate";
                    ddlContactType.LabelKey = "aca_foodfacility_label_contacttype";
                    txtSSN.LabelKey = "aca_foodfacility_label_ssn";
                    txtFEIN.LabelKey = "aca_foodfacility_label_fein";
                    txtProviderName.LabelKey = "aca_foodfacility_label_providername";
                    txtProviderNumber.LabelKey = "aca_foodfacility_label_providernumber";
                    ddlLicensingBoard.LabelKey = "aca_foodfacility_label_licensingboard";
                    txtFirstName.LabelKey = "aca_foodfacility_label_firstname";
                    txtMiddleInitial.LabelKey = "aca_foodfacility_label_middleinitial";
                    txtLastName.LabelKey = "aca_foodfacility_label_lastname";
                    txtBusiName.LabelKey = "aca_foodfacility_label_businessname";
                    txtBusiName2.LabelKey = "aca_foodfacility_label_businessname2";
                    txtBusiLicense.LabelKey = "aca_foodfacility_label_businesslicense";
                    ddlCountry.LabelKey = "aca_foodfacility_label_country";
                    txtAddress1.LabelKey = "aca_foodfacility_label_address1";
                    txtAddress2.LabelKey = "aca_foodfacility_label_address2";
                    txtAddress3.LabelKey = "aca_foodfacility_label_address3";
                    txtCity.LabelKey = "aca_foodfacility_label_city";
                    txtState.LabelKey = "aca_foodfacility_label_state";
                    txtZipCode.LabelKey = "aca_foodfacility_label_zip";
                    txtPhone1.LabelKey = "aca_foodfacility_label_phone1";
                    txtPhone2.LabelKey = "aca_foodfacility_label_phone2";
                    txtFax.LabelKey = "aca_foodfacility_label_fax";
                    txtContractorLicNO.LabelKey = "aca_foodfacility_label_contractorlicno";
                    txtContractorBusiName.LabelKey = "aca_foodfacility_label_contractorbusiname";

                    txtTitle.LabelKey = "aca_foodfacility_label_title";
                    txtInsuranceCompany.LabelKey = "aca_foodfacility_label_insurancecompany";
                    txtInsurancePolicy.LabelKey = "aca_foodfacility_label_insurancepolicy";
                    break;
            }
        }

        #endregion Methods
    }
}
