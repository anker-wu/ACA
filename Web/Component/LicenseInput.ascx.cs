#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: LicenseInput.ascx.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2013-2014
 *
 *  Description:
 *
 *  Notes:
 *      $Id: LicenseInput.ascx.cs 257891 2013-09-28 09:56:18Z ACHIEVO\alan.hu $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web.UI;

using Accela.ACA.BLL.APO;
using Accela.ACA.BLL.Cap;
using Accela.ACA.BLL.People;
using Accela.ACA.Common;
using Accela.ACA.Common.Util;
using Accela.ACA.ExpressionBuild;
using Accela.ACA.Web.Common;
using Accela.ACA.Web.Common.Control;
using Accela.ACA.Web.ExpressionBuild;
using Accela.ACA.WSProxy;

namespace Accela.ACA.Web.Component
{
    /// <summary>
    /// This class provide the ability to operation NewLicenseEdit. 
    /// </summary>
    public partial class LicenseInput : FormDesignerWithExpressionControl
    {
        #region Fields

        /// <summary>
        /// if daily user input custom data, it's licenseNumber will be saved as -3
        /// </summary>
        private const string DAILY_LICENSE_NUMBER = "-3";

        /// <summary>
        /// Template BLL.
        /// </summary>
        private ITemplateBll _templateBll = ObjectFactory.GetObject<ITemplateBll>();

        /// <summary>
        /// indicate the license form is editable or not.
        /// </summary>
        private bool _isEditable = true;

        /// <summary>
        /// ExpressionFactory class's instance
        /// </summary>
        private ExpressionFactory _expressionInstance;

        #endregion Fields

        /// <summary>
        /// Initializes a new instance of the <see cref="LicenseInput"/> class.
        /// </summary>
        public LicenseInput()
            : base(GviewID.LicenseEdit)
        {
        }

        #region Properties

        /// <summary>
        /// Gets or sets the section view ID.
        /// </summary>
        /// <value>The section view ID.</value>
        public string SectionViewId
        {
            get
            {
                return ViewId;
            }

            set
            {
                ViewId = value;
            }
        }

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
        /// Gets or sets data source
        /// </summary>
        public DataTable GridViewDataSource
        {
            get
            {
                return (DataTable)ViewState["LicensePros"];
            }

            set
            {
                ViewState["LicensePros"] = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the license's license Sequence NBR equals the capModel's, the LP will set to read-only and needn't to validated by expression
        /// </summary>
        public bool IsLockLicense
        {
            get
            {
                if (ViewState["IsLockLicense"] == null)
                {
                    return false;
                }

                return (bool)ViewState["IsLockLicense"];
            }

            set
            {
                ViewState["IsLockLicense"] = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the smart choice validate flag for license
        /// </summary>
        public bool IsValidate
        {
            get;
            set;
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
        /// Gets or sets a value indicating whether the form be used in Multiple LP edit form.
        /// </summary>
        public bool IsMultipleLicensedProfessional
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets a value indicate section component property
        /// </summary>
        /// <value>The name of the component.</value>
        public string ComponentName
        {
            get; 
            set;
        }

        /// <summary>
        ///  Gets or sets a value indicating whether section required property
        /// </summary>
        public bool IsSectionRequired
        {
            get;
            set;
        }

        /// <summary>
        /// Get / set the base permission value
        /// </summary>
        protected override GFilterScreenPermissionModel4WS Permission
        {
            get
            {
                base.Permission = new GFilterScreenPermissionModel4WS();
                base.Permission.permissionLevel = GViewConstant.PERMISSION_PEOPLE;
                base.Permission.permissionValue = GviewID.LicenseEdit.Equals(SectionViewId, StringComparison.InvariantCultureIgnoreCase) ? ddlLicenseType.SelectedValue : string.Empty;
                
                return base.Permission;
            }

            set
            {
                base.Permission = value;
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
                    ExpressionControls = CollectExpressionInputControls(GviewID.LicenseEdit, ModuleName, null, null);
                    _expressionInstance = new ExpressionFactory(ModuleName, ExpressionType.License_Professional, ExpressionControls);
                }

                return _expressionInstance;
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
                sbControls.Append(txtHomePhone.ClientID);
                sbControls.Append(",").Append(txtMobilePhone.ClientID);
                sbControls.Append(",").Append(txtFax.ClientID);

                return sbControls.ToString();
            }
        }

        /// <summary>
        /// Gets current action type
        /// </summary>
        private string ActionType
        {
            get
            {
                return Request.QueryString[UrlConstant.ACTION_TYPE];
            }
        }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Presents the license professional information to control.
        /// </summary>
        /// <param name="license">License model.</param>
        /// <returns>The form is disabled or not.</returns>
        public bool DisplayLicense(LicenseProfessionalModel license)
        {
            bool isDisable = false;
            string[] alwaysEditableControlIDs = null;
            bool isLookUp = GviewID.LicenseLookUp.Equals(ViewId);
            CapModel4WS capModel = AppSession.GetCapModelFromSession(ModuleName);

            // if license model is null,then init the control's initial value to resolve the page postback problem.
            if (license == null || (string.IsNullOrEmpty(license.licenseNbr) && string.IsNullOrEmpty(license.licenseType)))
            {
                ClearLicenseForm();
                DropDownListBindUtil.SetCountrySelectedValue(ddlCountry, string.Empty, !isLookUp, true, isLookUp);
                IsAppliedRegional = true;

                if (!AppSession.IsAdmin)
                {
                    SetCurrentCityAndState();
                }
            }
            else
            {
                DropDownListBindUtil.SetCountrySelectedValue(ddlCountry, license.countryCode, false, true, isLookUp);
                IsAppliedRegional = true;

                DropDownListBindUtil.SetSelectedValue(ddlLicenseType, license.licenseType);
                txtLicenseNum.Text = license.licenseNbr;
                DropDownListBindUtil.SetSelectedValue(ddlSalutation, license.salutation);
                txtFirstName.Text = license.contactFirstName;
                txtMiddleName.Text = license.contactMiddleName;
                txtLastName.Text = license.contactLastName;
                txtBirthDate.Text2 = license.birthDate;
                txtIssueDate.Text2 = license.licesnseOrigIssueDate;
                txtExpirationDate.Text2 = license.licenseExpirDate;
                DropDownListBindUtil.SetSelectedValueForRadioList(radioListGender, license.gender);
                txtBusName.Text = license.businessName;
                txtBusName2.Text = license.busName2;
                txtBusLicense.Text = license.businessLicense;
                txtSuffix.Text = license.suffixName;

                txtAddress1.Text = license.address1;
                txtAddress2.Text = license.address2;
                txtAddress3.Text = license.address3;
                txtCity.Text = license.city;
                txtState.Text = license.state;

                txtHomePhone.CountryCodeText = license.phone1CountryCode;
                txtHomePhone.Text = ModelUIFormat.FormatPhone4EditPage(license.phone1, license.countryCode);
                txtMobilePhone.CountryCodeText = license.phone2CountryCode;
                txtMobilePhone.Text = ModelUIFormat.FormatPhone4EditPage(license.phone2, license.countryCode);
                txtFax.CountryCodeText = license.faxCountryCode;
                txtFax.Text = ModelUIFormat.FormatPhone4EditPage(license.fax, license.countryCode);
                txtContractorLicNO.Text = string.IsNullOrEmpty(license.contrLicNo) ? string.Empty : license.contrLicNo;
                txtContractorBusiName.Text = license.contLicBusName;

                txtZipCode.Text = ModelUIFormat.FormatZipShow(license.zip, license.countryCode, false);
                txtPOBox.Text = license.postOfficeBox;
                hfLicenseAgencyCode.Value = license.agencyCode;
                hfLicenseTmpID.Value = license.TemporaryID;

                DropDownListBindUtil.SetSelectedValue(ddlContactType, license.typeFlag);
                txtSSN.Text = MaskUtil.FormatSSNShow(license.socialSecurityNumber);
                txtFEIN.Text = MaskUtil.FormatFEINShow(license.fein, StandardChoiceUtil.IsEnableFeinMasking());
                hfSSN.Value = license.socialSecurityNumber;
                hfFEIN.Value = license.fein;
                txtEmail.Text = license.email;

                //when the LP section is used in Multiple LP the temporary fields will work unnormally, this function resolves it.
                if (IsMultipleLicensedProfessional)
                {
                    templateEdit.ResetControl();
                }

                LicenseProfessionalModel[] licensees = LicenseUtil.GetSameTypeNumberLicenses(TempModelConvert.ConvertToLicenseProfessionalModelList(capModel.licenseProfessionalList), license, true);

                licensees = LicenseUtil.ResearchLicenseTemplates(licensees, ModuleName);

                DisplayAttributes(licensees);
                alwaysEditableControlIDs = TemplateUtil.GetAlwaysEditableControlIDs(license.templateAttributes, ACAConstant.CAP_PROFESSIONAL_TEMPLATE_FIELD_PREFIX);

                if (!string.IsNullOrEmpty(license.licSeqNbr))
                {
                    ViewState["LicenseId"] = license.licSeqNbr;
                    hfLicenseProId.Value = license.licSeqNbr;

                    /* 1.according to the validate property to set the field read only or not.
                    * 2.DAILY_LICENSE_NUMBER is used to judge the license comes from reference or not.
                    * the combo condition 1 and 2 is used to select reference licnese before cap type and save and resume LP.
                    */
                    if (IsValidate && license.licSeqNbr != DAILY_LICENSE_NUMBER)
                    {
                        txtZipCode.SetZipFromAA(license.zip);
                        DisableLicenseFormAllFields(alwaysEditableControlIDs);
                        isDisable = true;
                        IsLockLicense = true;
                    }

                    if (DAILY_LICENSE_NUMBER != hfLicenseProId.Value)
                    {
                        //Cache the reference data.
                        RefEntityCache = license;
                    }
                }

                //if create trade license, disable LP form.
                if (Request.QueryString["FilterName"] == ACAConstant.REQUEST_PARMETER_TRADE_LICENSE)
                {
                    DisableLicenseForm(alwaysEditableControlIDs);
                    isDisable = true;
                }

                //in super agency according to the validate property to set the field read only or not.
                if (HasSelectedLicense())
                {
                    if (IsValidate && license.licSeqNbr != DAILY_LICENSE_NUMBER)
                    {
                        DisableLicenseFormAllFields(alwaysEditableControlIDs);
                        isDisable = true;
                    }
                }
            }

            if (!IsEditable && !AppSession.IsAdmin)
            {
                DisableLicenseForm(alwaysEditableControlIDs);
                isDisable = true;
            }

            return isDisable;
        }

        /// <summary>
        /// Gets the License professional information from user input.
        /// </summary>
        /// <param name="licenseSeqNbr">license professional sequence number.</param>
        /// <param name="licenseProModel">The license pro model.</param>
        /// <param name="isSkipLicenseEdit">IsSkipLicenseEdit Flag.</param>
        public void SaveLicenseProfessionalModel(string licenseSeqNbr, LicenseProfessionalModel licenseProModel, bool isSkipLicenseEdit = false)
        {
            CapModel4WS capModel = AppSession.GetCapModelFromSession(ModuleName);
            LicenseProfessionalModel4WS selectedProfessional = null;

            if (capModel != null && capModel.licenseProfessionalList != null)
            {
                selectedProfessional = capModel.licenseProfessionalList.FirstOrDefault(item => item.TemporaryID == licenseProModel.TemporaryID);
            }

            /* Implements the feature 09ACC-01707 make LP be primary as well like APO does.
             * Set the primary flag to 'Y' when applying for a permit in ACA
             */
            licenseProModel.printFlag = ACAConstant.COMMON_Y;

            //if license number is empty or exist license's field isn't empty, it need auto build a license number.
            if (!GetLicenseValidateCondition() && string.IsNullOrEmpty(licenseProModel.licenseNbr))
            {
                //auto get license sequence number.
                ILicenseBLL licenseBll = ObjectFactory.GetObject<ILicenseBLL>();
                licenseProModel.licenseNbr = licenseBll.GetDailyLicenseSeqNumber(licenseProModel);
                txtLicenseNum.Text = licenseProModel.licenseNbr;
            }

            if (string.IsNullOrEmpty(hfLicenseAgencyCode.Value))
            {
                licenseProModel.agencyCode = ConfigManager.AgencyCode;
            }
            else
            {
                licenseProModel.agencyCode = hfLicenseAgencyCode.Value;
            }

            if (selectedProfessional != null)
            {
                List<string> agencyCodeList = new List<string>();

                foreach (LicenseProfessionalModel4WS lp in capModel.licenseProfessionalList)
                {
                    if (!agencyCodeList.Contains(lp.agencyCode))
                    {
                        agencyCodeList.Add(lp.agencyCode);
                    }
                }

                foreach (string agencyCode in agencyCodeList)
                {
                    foreach (LicenseProfessionalModel4WS lp in capModel.licenseProfessionalList)
                    {
                        if (lp.licenseType.Equals(selectedProfessional.licenseType, StringComparison.InvariantCultureIgnoreCase)
                            && lp.licenseNbr.Equals(selectedProfessional.licenseNbr, StringComparison.InvariantCultureIgnoreCase)
                            && lp.agencyCode.Equals(agencyCode, StringComparison.InvariantCultureIgnoreCase))
                        {
                            /*
                             * For licenses edit in super agency, need copy a same updated license model to instead the old one for each sub agency.
                             * else the prior sub agency's license data will be instead by below agency code as same license of B.
                             * case 1: in super agency LP list section, new a license L1 and save, then edit L1 and save.
                             * If there are two sub agency A & B services choosed, after this update, the license L1 of cap A will be instead by B's agency code.
                             * So in the end, the A's license L1 was lost.
                             */
                            LicenseProfessionalModel4WS inputLPModel = TempModelConvert.ConvertToLicenseProfessionalModel4WS(licenseProModel);

                            // selectedProfessional form the cap's lp list, it is same pointer use the this to find index.
                            int updateIndex = capModel.licenseProfessionalList.ToList().IndexOf(lp);
                            inputLPModel.licSeqNbr = lp.licSeqNbr;
                            inputLPModel.attributes = templateEdit.GetAttributeModels();
                            inputLPModel.componentName = ComponentName;
                            inputLPModel.TemporaryID = lp.TemporaryID;
                            inputLPModel.agencyCode = lp.agencyCode;

                            if (string.IsNullOrEmpty(inputLPModel.TemporaryID))
                            {
                                inputLPModel.TemporaryID = CommonUtil.GetRandomUniqueID();
                            }

                            capModel.licenseProfessionalList[updateIndex] = inputLPModel;
                        }
                    }
                }

                AppSession.SetCapModelToSession(ModuleName, capModel);
            }
            else
            {
                ICapBll capBll = ObjectFactory.GetObject<ICapBll>();
                ServiceModel[] services = capBll.GetServicesByRecordID(capModel.capID);
                LicenseProfessionalModel4WS[] newLicenses = CreateLicenseProfessionalModels(services, licenseProModel, GetAttributeModels(licenseProModel), isSkipLicenseEdit);
                List<LicenseProfessionalModel4WS> list = new List<LicenseProfessionalModel4WS>();

                if (capModel.licenseProfessionalList != null && capModel.licenseProfessionalList.Length > 0)
                {
                    //If add LP record to LP section, it needs remove original one after adding another LP record to the LP section
                    if (!IsMultipleLicensedProfessional)
                    {
                        List<LicenseProfessionalModel4WS> license = new List<LicenseProfessionalModel4WS>(capModel.licenseProfessionalList);

                        license.RemoveAll(lp => lp.componentName.Equals(ComponentName, StringComparison.InvariantCultureIgnoreCase));

                        capModel.licenseProfessionalList = license.ToArray();
                    }

                    list.AddRange(capModel.licenseProfessionalList);
                }

                foreach (LicenseProfessionalModel4WS item in newLicenses)
                {
                    if (!list.Any(o => IsSameLicense(o, item)))
                    {
                        item.componentName = ComponentName;
                        item.TemporaryID = string.IsNullOrEmpty(item.TemporaryID) ? CommonUtil.GetRandomUniqueID() : item.TemporaryID;
                        list.Insert(0, item);
                    }
                }

                capModel.licenseProfessionalList = list.ToArray();
                AppSession.SetCapModelToSession(ModuleName, capModel);
            }

            /*
             * If the selected license record is skipped the license edit form and auto added to License List, 
             * it should skip the update the template fields's value for the license record from License Edit form.
             */
            if (!isSkipLicenseEdit)
            {
                // 1.save license license from supervisor to session.
                SaveLicenseesForSuperAgency();

                // 2. update current license tempate fields after edit the license in License Edit Page..
                licenseProModel.templateAttributes = GetAttributeModels(licenseProModel);

                // save current license to session.
                SaveEditedTemplateForSuperAgency(licenseProModel);

                if (!StandardChoiceUtil.IsSuperAgency())
                {
                    licenseProModel.templateAttributes =
                        SetEMSEDDLOption(
                        licenseProModel.agencyCode,
                        licenseProModel.licenseType,
                        licenseProModel.licSeqNbr,
                        licenseProModel.licenseNbr,
                        licenseProModel.templateAttributes);
                }

                if (!string.IsNullOrEmpty(hfLicenseProId.Value) && DAILY_LICENSE_NUMBER != hfLicenseProId.Value)
                {
                    //Merge the reference data and UI data to prevent lose the data which fields hidden by form designer.
                    CapUtil.MergeRefDataToUIData<LicenseProfessionalModel, LicenseProfessionalModel>(
                        ref licenseProModel,
                        string.Empty,
                        string.Empty,
                        "licSeqNbr",
                        hfLicenseProId.Value,
                        ModuleName,
                        RefEntityCache,
                        Permission,
                        ViewId);
                }
            }

            ClearRefEntityCache();
        }

        /// <summary>
        /// Indicating the form data whether is valid.
        /// </summary>
        /// <returns>True if the license is valid</returns>
        public bool IsDataValid()
        {
            bool isValid = !IsEditable
                           || !IsValidate
                           || GetLicenseValidateCondition()
                           || (!string.IsNullOrEmpty(hfLicenseProId.Value) && hfLicenseProId.Value != DAILY_LICENSE_NUMBER);

            return isValid;
        }

        /// <summary>
        /// Check if the current license is available. Show error message if it's expired and not available.
        /// </summary>
        /// <param name="licenseModelList">The license model list.</param>
        /// <returns>return error message if the license is unavailable;otherwise,return empty string.</returns>
        public string IsAvailableLicense(LicenseModel4WS[] licenseModelList)
        {
            string errorMsg = string.Empty;
            CapModel4WS capModel = AppSession.GetCapModelFromSession(ModuleName);
            List<LicenseProfessionalModel4WS> lp4wsList = LicenseUtil.ConvertLicenseModel2LicenseProfessionalModel4WS(licenseModelList, ComponentName);
            LicenseProfessionalModel[] lpList = TempModelConvert.ConvertToLicenseProfessionalModelList(lp4wsList.ToArray());

            // Don't need to judge the big CAP in super agency.
            if (LicenseUtil.EnableExpiredLicense() && !StandardChoiceUtil.IsSuperAgency())
            {
                string unAvailableMsg = LicenseUtil.GetUnAvailableLPNums(lpList, capModel.capType);

                if (!string.IsNullOrEmpty(unAvailableMsg))
                {
                    errorMsg = DataUtil.StringFormat(GetTextByKey("per_multiplelicenses_error_unavailablelicense"), unAvailableMsg);
                }
            }

            // validate duplicate LP
            if (string.IsNullOrEmpty(errorMsg))
            {
                LicenseUtil.IsDuplicateLP(ModuleName, licenseModelList, capModel.licenseProfessionalList, ref errorMsg);
            }

            return errorMsg;
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
        /// Disable license professional form according to the editable property.
        /// </summary>
        /// <param name="alwaysEditableControlIDs">Control IDs for always editable controls</param>
        public void DisableLicenseForm(string[] alwaysEditableControlIDs)
        {
            DisableLicenseFormAllFields(alwaysEditableControlIDs);
        }

        /// <summary>
        /// clean license form , temple field and button visible.
        /// </summary>
        /// <param name="clearRegional">if set to <c>true</c> [clear regional].</param>
        public void ResetLicenseForm(bool clearRegional)
        {
            templateEdit.ResetControl();
            supervisorList.ResetControl();
            IsLockLicense = false;
            ClearLicenseForm();

            if (clearRegional && ActionType != AddDataWay.LookUp)
            {
                ControlUtil.ClearRegionalSetting(ddlCountry, ActionType == AddDataWay.LookUp, ModuleName, Permission, ViewId);
            }
            else
            {
                ControlUtil.ApplyRegionalSetting(false, ActionType == AddDataWay.LookUp, true, ActionType != AddDataWay.LookUp, ddlCountry);
            }
        }

        /// <summary>
        /// Get License Professionals
        /// </summary>
        /// <returns>model for LP</returns>
        public LicenseModel4WS GetLicenseProfessionals()
        {
            // construct a LicenseModel4WS model to search license.
            LicenseModel4WS licenseModel = new LicenseModel4WS();

            licenseModel.licenseType = ddlLicenseType.SelectedValue;
            licenseModel.stateLicense = txtLicenseNum.Text.Trim();
            licenseModel.salutation = ddlSalutation.SelectedValue;
            licenseModel.contactFirstName = txtFirstName.Text.Trim();
            licenseModel.contactMiddleName = txtMiddleName.Text.Trim();
            licenseModel.contactLastName = txtLastName.Text.Trim();
            licenseModel.suffixName = txtSuffix.Text.Trim();
            string birthDate = txtBirthDate.Text.Trim();

            if (!string.IsNullOrEmpty(birthDate))
            {
                licenseModel.birthDate = I18nDateTimeUtil.ConvertDateTimeStringFromUIToWebService(birthDate);
            }
            else
            {
                licenseModel.birthDate = null;
            }

            string issueDate = this.txtIssueDate.Text.Trim();

            if (!string.IsNullOrEmpty(issueDate))
            {
                licenseModel.licenseIssueDate = I18nDateTimeUtil.ConvertDateTimeStringFromUIToWebService(issueDate);
            }
            else
            {
                licenseModel.licenseIssueDate = null;
            }

            string expirationDate = this.txtExpirationDate.Text.Trim();

            if (!string.IsNullOrEmpty(expirationDate))
            {
                licenseModel.licenseExpirationDate = I18nDateTimeUtil.ConvertDateTimeStringFromUIToWebService(expirationDate);
            }
            else
            {
                licenseModel.licenseExpirationDate = null;
            }

            //licenseModel.birthDate = this.txtBirthDate.Text.Trim() == String.Empty ? String.Empty : I18nDateTimeUtil.ConvertDateStringFromUIToWebService(this.txtBirthDate.Text);
            licenseModel.gender = radioListGender.SelectedItem == null ? null : radioListGender.SelectedItem.Value;
            licenseModel.businessName = txtBusName.Text.Trim();
            licenseModel.busName2 = txtBusName2.Text.Trim();
            licenseModel.businessLicense = txtBusLicense.Text.Trim();

            licenseModel.countryCode = ControlUtil.GetControlValue(ddlCountry);
            licenseModel.address1 = txtAddress1.Text.Trim();
            licenseModel.address2 = txtAddress2.Text.Trim();
            licenseModel.address3 = txtAddress3.Text.Trim();
            licenseModel.emailAddress = txtEmail.Text.Trim();
            licenseModel.city = ControlUtil.GetControlValue(txtCity);
            licenseModel.state = ControlUtil.GetControlValue(txtState);
            licenseModel.postOfficeBox = txtPOBox.Text.Trim();
            licenseModel.phone1 = txtHomePhone.GetPhone(ddlCountry.SelectedValue);
            licenseModel.phone1CountryCode = txtHomePhone.Visible ? txtHomePhone.CountryCodeText.Trim() : string.Empty;
            licenseModel.phone2 = txtMobilePhone.GetPhone(ddlCountry.SelectedValue);
            licenseModel.phone2CountryCode = txtMobilePhone.Visible ? txtMobilePhone.CountryCodeText.Trim() : string.Empty;
            licenseModel.fax = txtFax.GetPhone(ddlCountry.SelectedValue);
            licenseModel.faxCountryCode = txtFax.Visible ? txtFax.CountryCodeText.Trim() : string.Empty;
            licenseModel.contrLicNo = txtContractorLicNO.Text.Trim();
            licenseModel.contLicBusName = txtContractorBusiName.Text.Trim();
            licenseModel.auditStatus = ACAConstant.VALID_STATUS;
            licenseModel.serviceProviderCode = ConfigManager.AgencyCode;
            licenseModel.zip = txtZipCode.GetZip(ddlCountry.SelectedValue).Trim();
            licenseModel.typeFlag = ddlContactType.SelectedValue;
            licenseModel.socialSecurityNumber = MaskUtil.UpdateSSN(hfSSN.Value, txtSSN.Text.Trim());
            licenseModel.maskedSsn = MaskUtil.FormatSSNShow(txtSSN.Text.Trim());
            licenseModel.fein = MaskUtil.UpdateFEIN(hfFEIN.Value, txtFEIN.Text.Trim());

            return licenseModel;
        }

        /// <summary>
        /// save current license professional
        /// </summary>
        /// <returns>error message</returns>
        public string SaveCurrentLicenseProfessional()
        {
            string errorMsg = ValidateLicenseInfo();

            if (!string.IsNullOrEmpty(errorMsg))
            {
                return errorMsg;
            }

            LicenseProfessionalModel licenseProModel = GetLicenseModelFormControl();
            SaveLicenseProfessionalModel(hfLicenseProId.Value, licenseProModel);

            hfLicenseProId.Value = string.Empty;

            return null;
        }

        /// <summary>
        /// clear the license input form
        /// </summary>
        /// <param name="clearRegional">Clear country default or not</param>
        public void ClearLicenseInputForm(bool clearRegional)
        {
            ResetLicenseForm(clearRegional);
            ClearRefEntityCache();
            ClearExpressionValue(true);
        }

        /// <summary>
        /// Resets the template.
        /// </summary>
        public void ResetTemplate()
        {
            if (string.IsNullOrEmpty(ddlLicenseType.SelectedValue))
            {
                templateEdit.ResetControl();
            }
        }

        /// <summary>
        /// set license number field autoPostBack property.
        /// </summary>
        /// <param name="autoPostBack">auto Post Back property</param>
        public void SetLicenseNumAutoPostBack(bool autoPostBack)
        {
            txtLicenseNum.AutoPostBack = autoPostBack;
        }

        /// <summary>
        /// Resets the supervisor template.
        /// </summary>
        public void ResetSupervisorTemplate()
        {
            supervisorList.ResetControl();
        }

        /// <summary>
        /// Validate the license search condition.
        /// If the search condition is null, it will prompt error message of "At least one search criteria must be entered.".
        /// </summary>
        /// <returns><c>true</c> if all condition not input, <c>false</c> otherwise.</returns>
        public bool CheckInputConditionForLicense()
        {
            bool isInputCondition = !(radioListGender.SelectedIndex < 0
                                      && string.IsNullOrEmpty(ControlUtil.GetControlValue(ddlLicenseType))
                                      && string.IsNullOrEmpty(ControlUtil.GetControlValue(ddlContactType))
                                      && string.IsNullOrEmpty(ControlUtil.GetControlValue(ddlCountry))
                                      && string.IsNullOrEmpty(ControlUtil.GetControlValue(ddlSalutation))
                                      && string.IsNullOrEmpty(ControlUtil.GetControlValue(txtState))
                                      && string.IsNullOrEmpty(txtLicenseNum.Text.Trim())
                                      && string.IsNullOrEmpty(txtSuffix.Text.Trim())
                                      && string.IsNullOrEmpty(txtPOBox.Text.Trim())
                                      && string.IsNullOrEmpty(txtFirstName.Text.Trim())
                                      && string.IsNullOrEmpty(txtMiddleName.Text.Trim())
                                      && string.IsNullOrEmpty(txtLastName.Text.Trim())
                                      && string.IsNullOrEmpty(txtBusName.Text.Trim())
                                      && string.IsNullOrEmpty(txtBusName2.Text.Trim())
                                      && string.IsNullOrEmpty(txtBusLicense.Text.Trim())
                                      && string.IsNullOrEmpty(txtContractorLicNO.Text.Trim())
                                      && string.IsNullOrEmpty(txtContractorBusiName.Text.Trim())
                                      && string.IsNullOrEmpty(txtSSN.Text.Trim())
                                      && string.IsNullOrEmpty(txtFEIN.Text.Trim())
                                      && string.IsNullOrEmpty(txtCity.Text.Trim())
                                      && string.IsNullOrEmpty(txtContractorBusiName.Text.Trim())
                                      && string.IsNullOrEmpty(txtContractorLicNO.Text.Trim())
                                      && string.IsNullOrEmpty(txtEmail.Text.Trim())
                                      && string.IsNullOrEmpty(txtBirthDate.Text.Trim())
                                      && string.IsNullOrEmpty(txtAddress1.Text.Trim())
                                      && string.IsNullOrEmpty(txtAddress2.Text.Trim())
                                      && string.IsNullOrEmpty(txtAddress3.Text.Trim())
                                      && string.IsNullOrEmpty(txtZipCode.Text.Trim())
                                      && string.IsNullOrEmpty(txtHomePhone.Text.Trim())
                                      && string.IsNullOrEmpty(txtHomePhone.CountryCodeText.Trim())
                                      && string.IsNullOrEmpty(txtMobilePhone.Text.Trim())
                                      && string.IsNullOrEmpty(txtMobilePhone.CountryCodeText.Trim())
                                      && string.IsNullOrEmpty(txtFax.Text.Trim())
                                      && string.IsNullOrEmpty(txtFax.CountryCodeText.Trim()));

            return isInputCondition;
        }

        /// <summary>
        /// on initial event method.
        /// </summary>
        /// <param name="e">EventArgs e</param>
        protected override void OnInit(EventArgs e)
        {
            this.ddlCountry.BindItems();
            ddlCountry.SetCountryControls(txtZipCode, txtState, txtMobilePhone, txtHomePhone, txtFax);

            if (!IsPostBack)
            {
                DropDownListBindUtil.BindLicenseType(ddlLicenseType);
                DropDownListBindUtil.BindSalutation(this.ddlSalutation);
                DropDownListBindUtil.BindGender(this.radioListGender);
                DropDownListBindUtil.BindContactType4License(ddlContactType);

                if (AppSession.IsAdmin)
                {
                    ddlLicenseType.AutoPostBack = false;
                    ddlLicenseType.Attributes.Add("onfocus", "javascript:if(typeof(parent.GetOldValueBeforeValueChange)!='undefined'){parent.GetOldValueBeforeValueChange(this);}");
                    ddlLicenseType.Attributes.Add("onchange", "javascript:if(typeof(parent.ChangedItemsValidateBeforePostBack)!='undefined'){parent.ChangedItemsValidateBeforePostBack(this,window);}");
                }
            }
        }

        /// <summary>
        /// Raises the page load event
        /// </summary>
        /// <param name="sender">An object that contains the event sender.</param>
        /// <param name="e">A System.EventArgs object containing the event data.</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            InitTemplateService();
            ddlCountry.RelevantControlIDs = RelevantControlIDs;
            ddlCountry.RegisterScripts();

            if (IsPostBack)
            {
                if (AppSession.IsAdmin)
                {
                    //Re-bind license type drop down to get the latest choices.
                    string licenseType = this.Request.Form[ddlLicenseType.UniqueID];
                    DropDownListBindUtil.BindLicenseType(ddlLicenseType);
                    DropDownListBindUtil.SetSelectedValue(ddlLicenseType, licenseType);
                }
            }
        }

        /// <summary>
        /// Handles the SelectedIndexChanged event of the License Type control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void LicenseType_SelectedIndexChanged(object sender, EventArgs e)
        {
            CreateOriginalLicenseTemplate(hfLicenseAgencyCode.Value, ddlLicenseType.SelectedValue.Trim(), txtLicenseNum.Text.Trim(), true);
            ClearExpressionValue(false);
            Page.FocusElement(ddlLicenseType.ClientID);
        }

        /// <summary>
        /// Handles the TextChanged event of the License Number control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void LicenseNumber_TextChanged(object sender, EventArgs e)
        {
            //Notice: Needn't deal with license number changed in license lookup page.
            if (txtLicenseNum.AutoPostBack)
            {
                CreateOriginalLicenseTemplate(hfLicenseAgencyCode.Value, ddlLicenseType.SelectedValue.Trim(), txtLicenseNum.Text.Trim(), true);
                Page.FocusElement(txtLicenseNum.ClientID);    
            }
        }

        /// <summary>
        /// OnPreRender event method.
        /// </summary>
        /// <param name="e">A System.EventArgs object containing the event data.</param>
        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);

            ControlBuildHelper.AddValidationForStandardFields(SectionViewId, ModuleName, Permission, Controls);

            phContent.TemplateControlIDPrefix = ACAConstant.CAP_PROFESSIONAL_TEMPLATE_FIELD_PREFIX;
            InitFormDesignerPlaceHolder(phContent, GviewID.LicenseLookUp.Equals(SectionViewId, StringComparison.InvariantCultureIgnoreCase) ? string.Empty : ddlLicenseType.SelectedValue);
           
            bool isLookUp = GviewID.LicenseLookUp.Equals(SectionViewId, StringComparison.InvariantCultureIgnoreCase);
            txtSSN.IsIgnoreValidate = isLookUp;
            txtFEIN.IsIgnoreValidate = isLookUp;
            txtZipCode.IsIgnoreValidate = isLookUp;
            txtHomePhone.IsIgnoreValidate = isLookUp;
            txtMobilePhone.IsIgnoreValidate = isLookUp;
            txtFax.IsIgnoreValidate = isLookUp;
            txtEmail.IsIgnoreValidate = isLookUp;
            ddlLicenseType.AutoPostBack = !isLookUp && !AppSession.IsAdmin;

            if (!IsAppliedRegional)
            {
                ControlUtil.ApplyRegionalSetting(IsPostBack, isLookUp, true, !IsPostBack && !isLookUp, ddlCountry);

                if (!AppSession.IsAdmin && !IsPostBack)
                {
                    SetCurrentCityAndState();
                }
            }

            //Add additional notice instruction for the License Form will be changes after selecting a contact type.
            if (ddlLicenseType.Visible && ddlLicenseType.AutoPostBack)
            {
                ddlLicenseType.ToolTipLabelKey = "aca_common_msg_dropdown_updateformlayout_tip";
            }
        }

        /// <summary>
        /// Initials the template web method.
        /// </summary>
        protected void InitTemplateService()
        {
            ScriptManager smg = ScriptManager.GetCurrent(Page);
            smg.EnablePageMethods = true;
            ServiceReference srTemplate = new ServiceReference("~/WebService/TemplateService.asmx");
            smg.Services.Add(srTemplate);
        }

        /// <summary>
        /// Gets the license model form control.
        /// </summary>
        /// <param name="ignoreVisible">IgnoreVisible Flag</param>
        /// <returns>License Professional Model.</returns>
        private LicenseProfessionalModel GetLicenseModelFormControl(bool ignoreVisible = true)
        {
            LicenseProfessionalModel licenseProModel = new LicenseProfessionalModel();
            licenseProModel.licenseType = ddlLicenseType.SelectedValue;
            licenseProModel.resLicenseType = string.IsNullOrEmpty(ddlLicenseType.SelectedValue) ? string.Empty : ddlLicenseType.SelectedItem.Text.Trim();
            licenseProModel.licenseNbr = ControlUtil.GetControlValue(txtLicenseNum, ignoreVisible);
            licenseProModel.salutation = ControlUtil.GetControlValue(ddlSalutation, ignoreVisible);
            licenseProModel.contactFirstName = ControlUtil.GetControlValue(txtFirstName, ignoreVisible);
            licenseProModel.contactMiddleName = ControlUtil.GetControlValue(txtMiddleName, ignoreVisible);
            licenseProModel.contactLastName = ControlUtil.GetControlValue(txtLastName, ignoreVisible);
            licenseProModel.suffixName = ControlUtil.GetControlValue(txtSuffix, ignoreVisible);

            string birthDate = ControlUtil.GetControlValue(txtBirthDate, ignoreVisible);
            licenseProModel.birthDate = !string.IsNullOrEmpty(birthDate)
                                            ? (DateTime?)I18nDateTimeUtil.ParseFromUI(birthDate) : null;

            string issueDate = ControlUtil.GetControlValue(txtIssueDate);
            licenseProModel.licesnseOrigIssueDate = !string.IsNullOrEmpty(issueDate)
                                                        ? (DateTime?)I18nDateTimeUtil.ParseFromUI(issueDate) : null;

            string expirationDate = ControlUtil.GetControlValue(txtExpirationDate);
            licenseProModel.licenseExpirDate = !string.IsNullOrEmpty(expirationDate)
                                                   ? (DateTime?)I18nDateTimeUtil.ParseFromUI(expirationDate) : null;

            licenseProModel.gender = ControlUtil.GetControlValue(radioListGender, ignoreVisible);
            licenseProModel.businessName = ControlUtil.GetControlValue(txtBusName, ignoreVisible);
            licenseProModel.busName2 = ControlUtil.GetControlValue(txtBusName2, ignoreVisible);
            licenseProModel.businessLicense = ControlUtil.GetControlValue(txtBusLicense, ignoreVisible);

            licenseProModel.countryCode = ddlCountry.SelectedValue;

            licenseProModel.address1 = ControlUtil.GetControlValue(txtAddress1, ignoreVisible);
            licenseProModel.address2 = ControlUtil.GetControlValue(txtAddress2, ignoreVisible);
            licenseProModel.address3 = ControlUtil.GetControlValue(txtAddress3, ignoreVisible);
            licenseProModel.city = ControlUtil.GetControlValue(txtCity, ignoreVisible);
            licenseProModel.state = ControlUtil.GetControlValue(txtState, ignoreVisible);
            licenseProModel.resState = txtState.Visible ? txtState.ResText : string.Empty;
            licenseProModel.phone1 = txtHomePhone.GetPhone(ddlCountry.SelectedValue.Trim());
            licenseProModel.phone1CountryCode = ControlUtil.GetCountryCodeText(txtHomePhone, ignoreVisible);
            licenseProModel.phone2 = txtMobilePhone.GetPhone(ddlCountry.SelectedValue.Trim(), ignoreVisible);
            licenseProModel.phone2CountryCode = ControlUtil.GetCountryCodeText(txtMobilePhone, ignoreVisible);
            licenseProModel.fax = txtFax.GetPhone(ddlCountry.SelectedValue.Trim(), ignoreVisible);
            licenseProModel.faxCountryCode = ControlUtil.GetCountryCodeText(txtFax, ignoreVisible);
            licenseProModel.contrLicNo = ControlUtil.GetControlValue(txtContractorLicNO, ignoreVisible);
            licenseProModel.contLicBusName = ControlUtil.GetControlValue(txtContractorBusiName, ignoreVisible);
            licenseProModel.email = ControlUtil.GetControlValue(txtEmail, ignoreVisible);

            //The serDes field is a required field. any string can be assigned to this field here.
            licenseProModel.serDes = "Description";
            licenseProModel.auditID = AppSession.User.PublicUserId;
            licenseProModel.auditStatus = ACAConstant.VALID_STATUS;
            licenseProModel.zip = txtZipCode.Visible || ignoreVisible ? txtZipCode.GetZip(ddlCountry.SelectedValue.Trim()) : string.Empty;
            licenseProModel.postOfficeBox = ControlUtil.GetControlValue(txtPOBox, ignoreVisible);
            licenseProModel.licSeqNbr = hfLicenseProId.Value;
            licenseProModel.typeFlag = ControlUtil.GetControlValue(ddlContactType, ignoreVisible);
            licenseProModel.socialSecurityNumber = MaskUtil.UpdateSSN(hfSSN.Value, ControlUtil.GetControlValue(txtSSN, ignoreVisible));
            hfSSN.Value = licenseProModel.socialSecurityNumber;
            licenseProModel.maskedSsn = MaskUtil.FormatSSNShow(ControlUtil.GetControlValue(txtSSN, ignoreVisible));
            txtSSN.Text = licenseProModel.maskedSsn;
            licenseProModel.fein = MaskUtil.UpdateFEIN(hfFEIN.Value, ControlUtil.GetControlValue(txtFEIN, ignoreVisible));
            hfFEIN.Value = licenseProModel.fein;

            licenseProModel.TemporaryID = hfLicenseTmpID.Value;

            return licenseProModel;
        }

        /// <summary>
        /// get license professional model
        /// </summary>
        /// <param name="services">service model</param>
        /// <param name="licenseModel">license model</param>
        /// <param name="attributes">The attributes.</param>
        /// <param name="isSkipLicenseEdit">IsSkipLicenseEdit Flag.</param>
        /// <returns>license professional model</returns>
        private LicenseProfessionalModel4WS[] CreateLicenseProfessionalModels(ServiceModel[] services, LicenseProfessionalModel licenseModel, TemplateAttributeModel[] attributes, bool isSkipLicenseEdit)
        {
            List<LicenseProfessionalModel4WS> licenses = new List<LicenseProfessionalModel4WS>();
            List<string> agencies = GetAgencies(services, licenseModel.licenseType);

            foreach (string agency in agencies)
            {
                LicenseProfessionalModel4WS license = TempModelConvert.ConvertToLicenseProfessionalModel4WS(licenseModel);
                
                if (!isSkipLicenseEdit)
                {
                    license.attributes = attributes;
                }

                license.agencyCode = agency;
                licenses.Add(license);
            }

            return licenses.ToArray();
        }

        /// <summary>
        /// clear license data
        /// </summary>
        private void ClearLicenseForm()
        {
            ControlUtil.ClearValue(this, null);

            hfLicenseProId.Value = string.Empty;
            hfLicenseAgencyCode.Value = string.Empty;
            hfSSN.Value = string.Empty;
            hfLicenseTmpID.Value = string.Empty;
            EnableLicenseForm();
        }

        /// <summary>
        /// Create original license template.
        /// </summary>
        /// <param name="agencyCode">agency code</param>
        /// <param name="licenseType">license type</param>
        /// <param name="licenseNum">license number</param>
        /// <param name="isResetTemplate">if set to <c>true</c> [is reset template].</param>
        private void CreateOriginalLicenseTemplate(string agencyCode, string licenseType, string licenseNum, bool isResetTemplate)
        {
            // if re-create original license template, need to clear previous template field to avoid the conflict for loading viewstate
            if (isResetTemplate)
            {
                templateEdit.ResetControl();
            }

            TemplateAttributeModel[] attributes;

            if (string.IsNullOrEmpty(licenseNum))
            {
                attributes = _templateBll.GetPeopleTemplateAttributes(licenseType, ConfigManager.AgencyCode, AppSession.User.PublicUserId);
            }
            else
            {
                string servProvCode = string.IsNullOrEmpty(agencyCode) ? ConfigManager.AgencyCode : agencyCode;
                attributes = _templateBll.GetLPAttributes4SupportEMSE(servProvCode, licenseType, string.Empty, licenseNum, AppSession.User.PublicUserId);
            }

            IList<LicenseProfessionalModel> list = new List<LicenseProfessionalModel>();
            List<string> agencies = new List<string>();

            if (attributes != null)
            {
                ICapBll capBll = ObjectFactory.GetObject(typeof(ICapBll)) as ICapBll;
                CapModel4WS capModel = AppSession.GetCapModelFromSession(ModuleName);

                ServiceModel[] services = capBll.GetServicesByRecordID(capModel.capID);
                agencies = GetAgencies(services, licenseType);

                foreach (string agency in agencies)
                {
                    LicenseProfessionalModel license = CreateLicenseProfessionalModel(attributes, licenseType, agency, licenseNum);
                    if (license != null && !list.Any(f => f.agencyCode == license.agencyCode && f.licenseType == license.licenseType && f.licenseNbr == license.licenseNbr))
                    {
                        list.Add(license);
                    }
                }
            }

            DisplayAttributes(list.ToArray());
        }

        /// <summary>
        /// Create LicenseProfessional Model
        /// </summary>
        /// <param name="attributes">template attribute</param>
        /// <param name="licenseType">license type</param>
        /// <param name="agencyCode">agency code</param>
        /// <param name="licenseNum">license number</param>
        /// <returns>License Professional Model</returns>
        private LicenseProfessionalModel CreateLicenseProfessionalModel(TemplateAttributeModel[] attributes, string licenseType, string agencyCode, string licenseNum)
        {
            LicenseProfessionalModel licenseModel = new LicenseProfessionalModel();
            licenseModel.licenseType = licenseType;
            licenseModel.agencyCode = agencyCode;
            licenseModel.licenseNbr = licenseNum;
            licenseModel.attributes = attributes;
            licenseModel.templateAttributes = attributes;

            return licenseModel;
        }

        /// <summary>
        /// Gets Matched agency array.
        /// </summary>
        /// <param name="services">The services</param>
        /// <param name="licenseType">license type</param>
        /// <returns>Agency Code List</returns>
        private List<string> GetAgencies(ServiceModel[] services, string licenseType)
        {
            List<string> agencies = new List<string>();

            if (services != null)
            {
                foreach (ServiceModel service in services)
                {
                    List<string> licenseTypes = new List<string>();
                    if (service.licProType != null)
                    {
                        licenseTypes.AddRange(service.licProType);
                    }

                    if (licenseTypes.Count > 0 && licenseTypes.Contains(licenseType))
                    {
                        if (!agencies.Contains(service.servPorvCode))
                        {
                            agencies.Add(service.servPorvCode);
                        }
                    }
                }

                if (agencies.Count == 0)
                {
                    foreach (ServiceModel service in services)
                    {
                        if (!agencies.Contains(service.servPorvCode))
                        {
                            agencies.Add(service.servPorvCode);
                        }
                    }
                }
            }

            if (agencies.Count == 0)
            {
                agencies.Add(ConfigManager.AgencyCode);
            }

            return agencies;
        }

        /// <summary>
        /// when set the editable property to false in ACA admin the license form will be locked.
        /// </summary>
        /// <param name="filterControlIDs">Some control IDs need to keep always editable.</param>
        private void DisableLicenseFormAllFields(string[] filterControlIDs = null)
        {
            DisableEdit(this, filterControlIDs);
        }

        /// <summary>
        /// Enable License Form
        /// </summary>
        private void EnableLicenseForm()
        {
            //if the LP section editable value is false, not to clean the form disable state.
            //the special logic for LP temp fields.
            if (IsEditable)
            {
                EnableEdit(this, null);
            }
        }

        /// <summary>
        /// Get the LP types of the selected services via the parent cap's ASI info.
        /// Check if the license info is auto populated when creating caps,
        /// If it's not auto populated,the license section should be editable.
        /// </summary>
        /// <returns>true or false.</returns>
        private bool HasSelectedLicense()
        {
            if (!CapUtil.IsSuperCAP(ModuleName))
            {
                return false;
            }

            // check if the license section is auto populated when creating or resuming. if yes, disabled the form.
            CapModel4WS capModel = AppSession.GetCapModelFromSession(ModuleName);

            LicenseModel4WS[] licenseList = AppSession.User.Licenses;
            ServiceModel[] services = AppSession.GetSelectedServicesFromSession();
            string[] lpTypes = null;

            // in creating process.
            if (services != null)
            {
                List<string> tempLPList = new List<string>();

                for (int i = 0; i < services.Length; i++)
                {
                    if (services[i].licProType != null)
                    {
                        tempLPList.AddRange(services[i].licProType);
                    }
                }

                lpTypes = new string[tempLPList.Count];
                tempLPList.CopyTo(lpTypes);
            }
            else
            {
                if (capModel.appSpecificInfoGroups != null)
                {
                    ILicenseBLL licenseBll = (ILicenseBLL)ObjectFactory.GetObject(typeof(ILicenseBLL));
                    lpTypes = licenseBll.GetLPType(capModel.appSpecificInfoGroups);
                }
            }

            if (lpTypes == null || licenseList == null)
            {
                return false;
            }

            // compare the user's LP type and selected services' LP type
            foreach (LicenseModel4WS license in licenseList)
            {
                for (int i = 0; i < lpTypes.Length; i++)
                {
                    if (license.licenseType == lpTypes[i])
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        /// <summary>
        /// If all license's fields is empty then return true.
        /// </summary>
        /// <returns>isAllEmpty true or false</returns>
        private bool GetLicenseValidateCondition()
        {
            bool isAllEmpty = txtMiddleName.Text.Trim() == string.Empty && txtBusName.Text.Trim() == string.Empty && txtHomePhone.Text.Trim() == string.Empty && txtMobilePhone.Text.Trim() == string.Empty
                              && txtZipCode.GetZip(ControlUtil.GetControlValue(ddlCountry)) == string.Empty && ddlLicenseType.SelectedValue == string.Empty && txtFirstName.Text.Trim() == string.Empty && txtCity.Text.Trim() == string.Empty && txtState.Text == string.Empty
                              && txtLastName.Text.Trim() == string.Empty && txtAddress1.Text.Trim() == string.Empty && txtFax.Text.Trim() == string.Empty && txtContractorLicNO.Text.Trim() == string.Empty && ControlUtil.GetControlValue(ddlCountry) == string.Empty
                              && txtContractorBusiName.Text.Trim() == string.Empty && txtLicenseNum.Text.Trim() == string.Empty && templateEdit.IsControlsValueEmpty() && string.IsNullOrWhiteSpace(txtSuffix.Text) && string.IsNullOrWhiteSpace(txtEmail.Text);

            return isAllEmpty;
        }

        /// <summary>
        /// when save license, we need synchronous the EMSE drop down list options. 
        /// </summary>
        /// <param name="agencyCode">agency code</param>
        /// <param name="licenseType">license type</param>
        /// <param name="licSeqNbr">license sequence number</param>
        /// <param name="licenseNbr">license number</param>
        /// <param name="licenseTemplates">license templates</param>
        /// <returns>Template Attribute Model</returns>
        private TemplateAttributeModel[] SetEMSEDDLOption(string agencyCode, string licenseType, string licSeqNbr, string licenseNbr, TemplateAttributeModel[] licenseTemplates)
        {
            if (string.IsNullOrEmpty(licenseNbr) || string.IsNullOrEmpty(licenseType)
                || licenseTemplates == null || licenseTemplates.Length <= 0)
            {
                return licenseTemplates;
            }

            TemplateAttributeModel[] attributes;
            string realAgencyCode = string.IsNullOrEmpty(agencyCode) ? ConfigManager.AgencyCode : agencyCode;
            attributes = _templateBll.GetLPAttributes4SupportEMSE(realAgencyCode, licenseType, licSeqNbr, licenseNbr, AppSession.User.PublicUserId);

            if (attributes != null && CapUtil.IsContainsEMSEAttribute(attributes))
            {
                foreach (TemplateAttributeModel attribute in attributes)
                {
                    if (attribute == null)
                    {
                        continue;
                    }

                    for (int i = 0; i < licenseTemplates.Length; i++)
                    {
                        TemplateAttributeModel lpTemplate = licenseTemplates[i];

                        if (lpTemplate == null)
                        {
                            continue;
                        }

                        if (!string.IsNullOrEmpty(lpTemplate.attributeScriptCode)
                            && lpTemplate.attributeName.Equals(attribute.attributeName, StringComparison.InvariantCulture))
                        {
                            lpTemplate.selectOptions = attribute.selectOptions;
                            break;
                        }
                    }
                }
            }

            return licenseTemplates;
        }

        /// <summary>
        /// display licensee template
        /// </summary>
        /// <param name="licensees">licensed professional models</param>
        private void DisplayAttributes(LicenseProfessionalModel[] licensees)
        {
            if (licensees == null || licensees.Length <= 0)
            {
                return;
            }

            //The attributes from CAP creating hasn't internationalization value, re-get it from cache.
            templateEdit.DisplayAttributes(licensees[0].templateAttributes, ACAConstant.CAP_PROFESSIONAL_TEMPLATE_FIELD_PREFIX);

            if (StandardChoiceUtil.IsSuperAgency())
            {
                IList<LicenseProfessionalModel> licenseList = new List<LicenseProfessionalModel>();

                foreach (LicenseProfessionalModel licensee in licensees)
                {
                    if (licensee != null && CapUtil.IsContainsEMSEAttribute(licensee.templateAttributes))
                    {
                        licenseList.Add(licensee);
                    }
                }

                supervisorList.DisplaySupervisor4EachLP(licenseList);
            }
        }

        /// <summary>
        /// save licensees to session cap.
        /// </summary>
        private void SaveLicenseesForSuperAgency()
        {
            if (!StandardChoiceUtil.IsSuperAgency())
            {
                return;
            }

            LicenseProfessionalModel[] modifiedLicensees = supervisorList.GetLicensees();
            CapModel4WS capModel = AppSession.GetCapModelFromSession(this.ModuleName);
            LicenseProfessionalModel[] initialLicensees = null;

            if (capModel != null && capModel.licenseProfessionalList != null && capModel.licenseProfessionalList.Length > 0)
            {
                initialLicensees = TempModelConvert.ConvertToLicenseProfessionalModelList(capModel.licenseProfessionalList);
            }

            if (modifiedLicensees != null && modifiedLicensees.Length > 0 && initialLicensees != null && initialLicensees.Length > 0)
            {
                foreach (LicenseProfessionalModel modifiedLicensee in modifiedLicensees)
                {
                    if (string.IsNullOrEmpty(modifiedLicensee.licenseNbr)
                        || string.IsNullOrEmpty(modifiedLicensee.licenseType)
                        || string.IsNullOrEmpty(modifiedLicensee.agencyCode))
                    {
                        continue;
                    }

                    for (int i = 0; i < initialLicensees.Length; i++)
                    {
                        LicenseProfessionalModel initialLicensee = initialLicensees[i];

                        if (initialLicensee == null
                            || string.IsNullOrEmpty(initialLicensee.licenseNbr)
                            || string.IsNullOrEmpty(initialLicensee.licenseType)
                            || string.IsNullOrEmpty(initialLicensee.agencyCode))
                        {
                            continue;
                        }

                        if (IsSameLicense(initialLicensee, modifiedLicensee))
                        {
                            initialLicensees[i].templateAttributes = TemplateUtil.MergeLicensedProfessionalTemplateAttributes(modifiedLicensee.templateAttributes, initialLicensees[i].templateAttributes);
                            initialLicensees[i].attributes = modifiedLicensee.attributes;
                            break;
                        }
                    }
                }

                capModel.licenseProfessionalList = TempModelConvert.ConvertToLicenseProfessionalModel4WSList(initialLicensees);
                AppSession.SetCapModelToSession(ModuleName, capModel);
            }
        }

        /// <summary>
        /// get attributes 
        /// </summary>
        /// <param name="curLicensee">licensed professional user selected from web page</param>
        /// <returns>template models</returns>
        private TemplateAttributeModel[] GetAttributeModels(LicenseProfessionalModel curLicensee)
        {
            TemplateAttributeModel[] attributesWithSupevisor = null;
            TemplateAttributeModel[] attributesFromUI = templateEdit.GetAttributeModels();

            if (StandardChoiceUtil.IsSuperAgency())
            {
                CapModel4WS capModel = AppSession.GetCapModelFromSession(this.ModuleName);

                if (capModel != null && capModel.licenseProfessionalList != null && capModel.licenseProfessionalList.Length > 0)
                {
                    LicenseProfessionalModel[] licensees = TempModelConvert.ConvertToLicenseProfessionalModelList(capModel.licenseProfessionalList);

                    foreach (LicenseProfessionalModel licensee in licensees)
                    {
                        if (licensee == null || string.IsNullOrEmpty(licensee.licenseNbr)
                        || string.IsNullOrEmpty(licensee.licenseType) || string.IsNullOrEmpty(licensee.agencyCode))
                        {
                            continue;
                        }

                        if (IsSameLicense(licensee, curLicensee))
                        {
                            attributesWithSupevisor = licensee.templateAttributes;
                            break;
                        }
                    }
                }

                attributesFromUI = SaveEditedTemplate(attributesWithSupevisor, attributesFromUI);
            }

            return attributesFromUI;
        }

        /// <summary>
        /// judge two licensee same or not.
        /// </summary>
        /// <param name="licensee">one licensee</param>
        /// <param name="sameToLicensee">the other licensee</param>
        /// <returns>true or false</returns>
        private bool IsSameLicense(LicenseProfessionalModel licensee, LicenseProfessionalModel sameToLicensee)
        {
            bool isSameLicense = string.Equals(licensee.licenseType, sameToLicensee.licenseType, StringComparison.InvariantCulture)
                                 && string.Equals(licensee.licenseNbr, sameToLicensee.licenseNbr, StringComparison.InvariantCulture)
                                 && string.Equals(licensee.agencyCode, sameToLicensee.agencyCode, StringComparison.InvariantCulture);

            return isSameLicense;
        }

        /// <summary>
        /// judge two licensee same or not.
        /// </summary>
        /// <param name="licensee">one licensee</param>
        /// <param name="sameToLicensee">the other licensee</param>
        /// <returns>true or false</returns>
        private bool IsSameLicense(LicenseProfessionalModel4WS licensee, LicenseProfessionalModel4WS sameToLicensee)
        {
            bool isSameLicense = string.Equals(licensee.licenseType, sameToLicensee.licenseType, StringComparison.InvariantCulture)
                                 && string.Equals(licensee.licenseNbr, sameToLicensee.licenseNbr, StringComparison.InvariantCulture)
                                 && string.Equals(licensee.agencyCode, sameToLicensee.agencyCode, StringComparison.InvariantCulture);

            return isSameLicense;
        }

        /// <summary>
        /// save normal template which is modified.
        /// </summary>
        /// <param name="currentLicense">license which is displayed in page</param>
        private void SaveEditedTemplateForSuperAgency(LicenseProfessionalModel currentLicense)
        {
            if (!StandardChoiceUtil.IsSuperAgency() || currentLicense == null)
            {
                return;
            }

            CapModel4WS capModel = AppSession.GetCapModelFromSession(this.ModuleName);
            LicenseProfessionalModel[] initialLicensees = null;

            if (capModel != null && capModel.licenseProfessionalList != null && capModel.licenseProfessionalList.Length > 0)
            {
                initialLicensees = TempModelConvert.ConvertToLicenseProfessionalModelList(capModel.licenseProfessionalList);
            }

            if (initialLicensees != null && initialLicensees.Length > 0)
            {
                foreach (LicenseProfessionalModel license in initialLicensees)
                {
                    if (license.licenseType.Equals(currentLicense.licenseType, StringComparison.InvariantCulture)
                        && license.licenseNbr.Equals(currentLicense.licenseNbr, StringComparison.InvariantCulture))
                    {
                        license.templateAttributes = SaveEditedTemplate(license.templateAttributes, currentLicense.templateAttributes);
                    }
                }

                capModel.licenseProfessionalList = TempModelConvert.ConvertToLicenseProfessionalModel4WSList(initialLicensees);
                AppSession.SetCapModelToSession(ModuleName, capModel);
            }
        }

        /// <summary>
        /// save normal template which is modified.
        /// </summary>
        /// <param name="toAttributes">the attributes which will be copied to. </param>
        /// <param name="attributes">the attributes which will be copied.</param>
        /// <returns>template attribute models</returns>
        private TemplateAttributeModel[] SaveEditedTemplate(TemplateAttributeModel[] toAttributes, TemplateAttributeModel[] attributes)
        {
            if (toAttributes == null || toAttributes.Length == 0)
            {
                return attributes;
            }

            if (attributes == null || attributes.Length <= 0)
            {
                return toAttributes;
            }

            foreach (TemplateAttributeModel attribute in attributes)
            {
                if (attribute == null
                    || attribute.attributeName == null
                    || !string.IsNullOrEmpty(attribute.attributeScriptCode))
                {
                    continue;
                }

                foreach (TemplateAttributeModel targetAttributeModel in toAttributes)
                {
                    if (targetAttributeModel == null || targetAttributeModel.attributeName == null)
                    {
                        continue;
                    }

                    if (targetAttributeModel.attributeName.Equals(attribute.attributeName, StringComparison.InvariantCulture))
                    {
                        targetAttributeModel.attributeValue = attribute.attributeValue;
                        break;
                    }
                }
            }

            return toAttributes;
        }

        /// <summary>
        /// Check the license.
        /// </summary>
        /// <returns>error message if the license is not valid;otherwise, return empty string.</returns>
        private string ValidateLicenseInfo()
        {
            string errorMsg = string.Empty;

            CapModel4WS capModel = AppSession.GetCapModelFromSession(ModuleName);

            if (IsDataValid())
            {
                LicenseModel4WS licenseModel = new LicenseModel4WS
                                                   {
                                                       licenseType = ddlLicenseType.SelectedValue,
                                                       stateLicense = txtLicenseNum.Text.Trim(),
                                                       serviceProviderCode = ConfigManager.AgencyCode,
                                                       licSeqNbr = hfLicenseProId.Value,
                                                       TemporaryID = hfLicenseTmpID.Value
                                                   };

                /*
                 * Under Multiple Agencies Environment, if select some services under different agencies to create applications, 
                 * there are same LP records with different agency code in capModel.licenseProfessionalList. So if select a existing LP record in LP list to edit. 
                 * Need get the corresponding agency code for the selected LP record.
                 */
                if (StandardChoiceUtil.IsSuperAgency())
                {
                    if (capModel.licenseProfessionalList != null)
                    {
                        LicenseProfessionalModel license = TempModelConvert.ConvertToLicenseProfessionalModel(
                            capModel.licenseProfessionalList.FirstOrDefault(p => p.TemporaryID.Equals(licenseModel.TemporaryID, StringComparison.InvariantCultureIgnoreCase)));

                        if (license != null)
                        {
                            licenseModel.serviceProviderCode = license.agencyCode;
                        }
                    }
                }

                LicenseModel4WS[] licenseList = new[] { licenseModel };

                // check if the license is expired and not available.
                errorMsg = IsAvailableLicense(licenseList);
            }
            else
            {
                errorMsg = GetTextByKey("per_license_error_searchClickedRequired");
            }

            return errorMsg;
        }

        #endregion Methods
    }
}