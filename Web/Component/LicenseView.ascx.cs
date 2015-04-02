#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: CapDetail.aspx.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2008-2014
 *
 *  Description:
 *
 *  Notes:
 *      $Id: LicenseView.ascx.cs 277971 2014-08-25 06:10:06Z ACHIEVO\james.shi $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using System;
using Accela.ACA.BLL.Common;
using Accela.ACA.BLL.People;
using Accela.ACA.Common;
using Accela.ACA.Common.Common;
using Accela.ACA.Common.Util;
using Accela.ACA.Web.Common;
using Accela.ACA.Web.Common.Control;
using Accela.ACA.WSProxy;

namespace Accela.ACA.Web.Component
{
    /// <summary>
    /// This class provide the ability to operation LicenseView. 
    /// </summary>
    public partial class LicenseView : BaseUserControl
    {
        /// <summary>
        /// The GView BLL
        /// </summary>
        private static IGviewBll _gviewBll = ObjectFactory.GetObject<IGviewBll>();

        /// <summary>
        /// Gets or sets a value indicating whether current page in cap confirm or not.
        /// </summary>
        public bool IsInConfirmPage
        {
            get;
            set;
        }

        #region Methods

        /// <summary>
        /// Display the License information
        /// </summary>
        /// <param name="licenseModel">License model for displaying</param>
        public void Display(LicenseProfessionalModel licenseModel)
        {
            if (licenseModel == null)
            {
                licenseModel = new LicenseProfessionalModel();
            }

            GFilterScreenPermissionModel4WS permission = new GFilterScreenPermissionModel4WS()
                                                         {
                                                             permissionLevel = GViewConstant.PERMISSION_PEOPLE,
                                                             permissionValue = licenseModel.licenseType
                                                         };

            SimpleViewElementModel4WS[] models = ModelUIFormat.GetSimpleViewElementModelBySectionID(ModuleName, permission, GviewID.LicenseEdit);
            IGviewBll gviewBll = ObjectFactory.GetObject<IGviewBll>();

            if (IsInConfirmPage)
            {
                DisplayInConfirm(licenseModel, models, gviewBll);

                //The attributes from CAP creating hasn't internationalization value, re-get it from cache.
                licenseModel.templateAttributes = CapUtil.SetPeopleTemplateUnitResValue(licenseModel.templateAttributes);
                templateView.DisplayAttributes(licenseModel.templateAttributes);
            }
            else
            {
                string licensePatterns = GetTextByKey(lblLicenseViewPattern.LabelKey);
                DisplayInSpearForm(licensePatterns, licenseModel, models, gviewBll);
            }
        }

        /// <summary>
        /// page load event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">event arguments</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                divLicenseViewPattern.Visible = AppSession.IsAdmin && !IsInConfirmPage;
            }
        }

        /// <summary>
        /// Get the field value with blank string.
        /// </summary>
        /// <param name="models">The SimpleViewElementModel</param>
        /// <param name="field">The field.</param>
        /// <param name="controlID">The control id.</param>
        /// <returns>The field value that is visible.</returns>
        private static string GetFieldValueWithBlank(SimpleViewElementModel4WS[] models, string field, string controlID)
        {
            string result = string.Empty;

            if (!string.IsNullOrEmpty(field) && _gviewBll.IsFieldVisible(models, controlID))
            {
                result = string.Format("{0}{1}", ScriptFilter.FilterScript(field), ACAConstant.BLANK);
            }

            return result;
        }

        /// <summary>
        /// Get the field value
        /// </summary>
        /// <param name="models">The SimpleViewElementModel</param>
        /// <param name="field">The field.</param>
        /// <param name="controlID">The control id.</param>
        /// <returns>The field value that is visible.</returns>
        private static string GetFieldValue(SimpleViewElementModel4WS[] models, string field, string controlID)
        {
            string result = string.Empty;

            if (!string.IsNullOrEmpty(field) && _gviewBll.IsFieldVisible(models, controlID))
            {
                result = ScriptFilter.FilterScript(field);
            }

            return result;
        }

        /// <summary>
        /// if not initial all field the view will display previous value.
        /// </summary>
        private void InitialAllField()
        {
            IblSalutation.Text = string.Empty;
            IblFirstName.Text = string.Empty;
            IblMiddleName.Text = string.Empty;
            IblLastName.Text = string.Empty;
            IblChSalutation.Text = string.Empty;
            IblBirthDate.Text = string.Empty;
            IblGender.Text = string.Empty;
            IblNameBusiness.Text = string.Empty;
            IblNameBusiness2.Text = string.Empty;
            lblBusinessLicense.Text = string.Empty;
            IblAddress1.Text = string.Empty;
            IblAddress2.Text = string.Empty;
            IblAddress3.Text = string.Empty;
            IblCity.Text = string.Empty;
            IblState.Text = string.Empty;
            IblZip.Text = string.Empty;
            IblCountry.Text = string.Empty;
            IblPOBox.Text = string.Empty;
            IblHomePhone.Text = string.Empty;
            IblMobilePhone.Text = string.Empty;
            IblFax.Text = string.Empty;
            IblClassCode.Text = string.Empty;
            IblLicState.Text = string.Empty;
            IblLicenseType.Text = string.Empty;
            IblClassCode2.Text = string.Empty;
            IblLicState2.Text = string.Empty;
            IblLicenseNumber.Text = string.Empty;
            IblContactType.Text = string.Empty;
            IblSSN.Text = string.Empty;
            IblFEIN.Text = string.Empty;
            IblContractorLicNo.Text = string.Empty;
            IblContractorBusNam.Text = string.Empty;
        }

        /// <summary>
        /// Display license summary in spear form.
        /// </summary>
        /// <param name="licenseModel">License professional model</param>
        /// <param name="models">Simple view element model</param>
        /// <param name="gviewBll">GView business</param>
        private void DisplayInConfirm(LicenseProfessionalModel licenseModel, SimpleViewElementModel4WS[] models, IGviewBll gviewBll)
        {
            divConfirmLicenseView.Visible = true;
            InitialAllField();

            if (!I18nCultureUtil.IsChineseCultureEnabled)
            {
                // Salutation
                if (!string.IsNullOrEmpty(licenseModel.salutation)
                    && gviewBll.IsFieldVisible(models, "ddlSalutation"))
                {
                    string salutation = StandardChoiceUtil.GetStandardChoiceValueByKey(BizDomainConstant.STD_CAT_SALUTATION, licenseModel.salutation);
                    IblSalutation.Text = string.Format("{0}{1}", ScriptFilter.FilterScript(salutation), ACAConstant.BLANK);
                }
            }

            // First Name
            if (!string.IsNullOrEmpty(licenseModel.contactFirstName)
                && gviewBll.IsFieldVisible(models, "txtFirstName"))
            {
                IblFirstName.Text = string.Format("{0}{1}", ScriptFilter.FilterScript(licenseModel.contactFirstName), ACAConstant.BLANK);
            }

            // Middle Name
            if (!string.IsNullOrEmpty(licenseModel.contactMiddleName)
                && gviewBll.IsFieldVisible(models, "txtMiddleName"))
            {
                IblMiddleName.Text = string.Format("{0}{1}", ScriptFilter.FilterScript(licenseModel.contactMiddleName), ACAConstant.BLANK);
            }

            // Last Name
            if (!string.IsNullOrEmpty(licenseModel.contactLastName)
                && gviewBll.IsFieldVisible(models, "txtLastName"))
            {
                IblLastName.Text = string.Format("{0}{1}", ScriptFilter.FilterScript(licenseModel.contactLastName), ACAConstant.BLANK);
            }

            if (!string.IsNullOrEmpty(licenseModel.suffixName)
                && gviewBll.IsFieldVisible(models, "txtSuffix"))
            {
                lblSuffix.Text = string.Format("{0}{1}", ScriptFilter.FilterScript(licenseModel.suffixName), ACAConstant.BLANK);
            }

            if (I18nCultureUtil.IsChineseCultureEnabled)
            {
                // Chinese Salutation
                if (!string.IsNullOrEmpty(licenseModel.salutation)
                    && gviewBll.IsFieldVisible(models, "ddlSalutation"))
                {
                    string salutation = StandardChoiceUtil.GetStandardChoiceValueByKey(BizDomainConstant.STD_CAT_SALUTATION, licenseModel.salutation);
                    IblChSalutation.Text = ScriptFilter.FilterScript(salutation);
                }
            }

            // Birth Date
            if (licenseModel.birthDate != null && gviewBll.IsFieldVisible(models, "txtBirthDate"))
            {
                IblBirthDate.Text = I18nStringUtil.FormatToTableRow(GetTextByKey("LicenseEdit_LicensePro_label_birthDate"), I18nDateTimeUtil.FormatToDateStringForUI(licenseModel.birthDate.Value));
            }

            // Gender
            if (!string.IsNullOrEmpty(licenseModel.gender)
                && gviewBll.IsFieldVisible(models, "radioListGender"))
            {
                string gender = StandardChoiceUtil.GetGenderByKey(licenseModel.gender);
                IblGender.Text = ScriptFilter.FilterScript(gender);
            }

            // Business Name
            if (!string.IsNullOrEmpty(licenseModel.businessName)
                && gviewBll.IsFieldVisible(models, "txtBusName"))
            {
                IblNameBusiness.Text = ScriptFilter.FilterScript(licenseModel.businessName);
            }

            // Business Name2
            if (!string.IsNullOrEmpty(licenseModel.busName2)
                && gviewBll.IsFieldVisible(models, "txtBusName2"))
            {
                IblNameBusiness2.Text = string.Format("{0}{1}", ACAConstant.SLASH, ScriptFilter.FilterScript(licenseModel.busName2));
            }

            if (!string.IsNullOrEmpty(licenseModel.businessLicense) && gviewBll.IsFieldVisible(models, "txtBusLicense"))
            {
                lblBusinessLicense.Text = ScriptFilter.FilterScript(licenseModel.businessLicense);
            }

            // Address1
            if (!string.IsNullOrEmpty(licenseModel.address1)
                && gviewBll.IsFieldVisible(models, "txtAddress1"))
            {
                IblAddress1.Text = ScriptFilter.FilterScript(licenseModel.address1);
            }

            // Address2
            if (!string.IsNullOrEmpty(licenseModel.address2)
                && gviewBll.IsFieldVisible(models, "txtAddress2"))
            {
                IblAddress2.Text = ScriptFilter.FilterScript(licenseModel.address2);
            }

            // Address3
            if (!string.IsNullOrEmpty(licenseModel.address3)
                && gviewBll.IsFieldVisible(models, "txtAddress3"))
            {
                IblAddress3.Text = ScriptFilter.FilterScript(licenseModel.address3);
            }

            // Email
            if (!string.IsNullOrEmpty(licenseModel.email)
                && gviewBll.IsFieldVisible(models, "txtEmail"))
            {
                lblEmail.Text = ScriptFilter.FilterScript(licenseModel.email);
            }

            // City
            if (!string.IsNullOrEmpty(licenseModel.city)
                && gviewBll.IsFieldVisible(models, "txtCity"))
            {
                IblCity.Text = string.Format("{0}{1}", ScriptFilter.FilterScript(licenseModel.city), ACAConstant.COMMA_BLANK);
            }

            // State
            if (!string.IsNullOrEmpty(licenseModel.resState)
                && gviewBll.IsFieldVisible(models, "txtState")
                && StandardChoiceUtil.IsDisplayLicenseState())
            {
                IblState.Text = string.Format("{0}{1}", ScriptFilter.FilterScript(I18nUtil.DisplayStateForI18N(licenseModel.state, licenseModel.countryCode)), ACAConstant.COMMA_BLANK);
            }

            // Zip
            if (!string.IsNullOrEmpty(licenseModel.zip)
                && gviewBll.IsFieldVisible(models, "txtZipCode"))
            {
                IblZip.Text = ModelUIFormat.FormatZipShow(licenseModel.zip, licenseModel.countryCode);
            }

            // Country Code
            if (!string.IsNullOrEmpty(licenseModel.countryCode)
                && gviewBll.IsFieldVisible(models, "ddlCountry"))
            {
                IblCountry.Text = ScriptFilter.FilterScript(StandardChoiceUtil.GetCountryByKey(licenseModel.countryCode));
            }

            // Post Office Box
            if (!string.IsNullOrEmpty(licenseModel.postOfficeBox)
                && gviewBll.IsFieldVisible(models, "txtPOBox"))
            {
                IblPOBox.Text = I18nStringUtil.FormatToTableRow(GetTextByKey("LicenseEdit_LicensePro_label_poBox"), ScriptFilter.FilterScript(licenseModel.postOfficeBox));
            }

            // Home Phone
            if (!string.IsNullOrEmpty(licenseModel.phone1)
                && gviewBll.IsFieldVisible(models, "txtHomePhone"))
            {
                string temp = ModelUIFormat.FormatPhoneShow(licenseModel.phone1CountryCode, licenseModel.phone1, licenseModel.countryCode);
                IblHomePhone.Text = I18nStringUtil.FormatToTableRow(BasePage.GetStaticTextByKey("LicenseEdit_LicensePro_label_homePhone"), temp);
            }

            // Mobile Phone
            if (!string.IsNullOrEmpty(licenseModel.phone2)
                && gviewBll.IsFieldVisible(models, "txtMobilePhone"))
            {
                string temp = ModelUIFormat.FormatPhoneShow(licenseModel.phone2CountryCode, licenseModel.phone2, licenseModel.countryCode);
                IblMobilePhone.Text = I18nStringUtil.FormatToTableRow(BasePage.GetStaticTextByKey("LicenseEdit_LicensePro_label_mobile"), temp);
            }

            // Fax
            if (!string.IsNullOrEmpty(licenseModel.fax)
                && gviewBll.IsFieldVisible(models, "txtFax"))
            {
                string temp = ModelUIFormat.FormatPhoneShow(licenseModel.faxCountryCode, licenseModel.fax, licenseModel.countryCode);
                IblFax.Text = I18nStringUtil.FormatToTableRow(BasePage.GetStaticTextByKey("LicenseEdit_LicensePro_label_fax"), temp);
            }

            LicenseModel4WS licenseModelRef = new LicenseModel4WS();

            // Get Reference License Model based on License Type & Number to display license state
            if (!string.IsNullOrEmpty(licenseModel.licenseNbr) &&
               !string.IsNullOrEmpty(licenseModel.licenseType))
            {
                LicenseModel4WS license = new LicenseModel4WS();

                license.serviceProviderCode = ConfigManager.AgencyCode;
                license.licenseType = licenseModel.licenseType;
                license.stateLicense = licenseModel.licenseNbr;

                ILicenseBLL licenseBll = (ILicenseBLL)ObjectFactory.GetObject(typeof(ILicenseBLL));
                licenseModelRef = licenseBll.GetLicenseByStateLicNbr(license);
            }

            // License Type
            if (!string.IsNullOrEmpty(licenseModel.resLicenseType)
                && gviewBll.IsFieldVisible(models, "ddlLicenseType"))
            {
                if (!string.IsNullOrEmpty(licenseModel.classCode))
                {
                    //here class code standand for licState
                    IblClassCode.Text = string.Format("{0}{1}", ScriptFilter.FilterScript(licenseModel.classCode), ACAConstant.BLANK);
                }

                if (licenseModelRef != null
                    && !string.IsNullOrEmpty(licenseModelRef.licState)
                    && StandardChoiceUtil.IsDisplayLicenseState())
                {
                    IblLicState.Text = string.Format("{0}{1}", ScriptFilter.FilterScript(I18nUtil.DisplayStateForI18N(licenseModelRef.licState, licenseModelRef.countryCode)), ACAConstant.BLANK);
                }

                IblLicenseType.Text = string.Format("{0}{1}", ScriptFilter.FilterScript(licenseModel.resLicenseType), ACAConstant.BLANK);
            }

            // License Number
            if (!string.IsNullOrEmpty(licenseModel.licenseNbr)
                && gviewBll.IsFieldVisible(models, "txtLicenseNum"))
            {
                if (!string.IsNullOrEmpty(licenseModel.classCode))
                {
                    //here class code standand for licState
                    IblClassCode2.Text = string.Format("{0}{1}", ScriptFilter.FilterScript(licenseModel.classCode), ACAConstant.BLANK);
                }

                if (licenseModelRef != null && !string.IsNullOrEmpty(licenseModelRef.licState) && StandardChoiceUtil.IsDisplayLicenseState())
                {
                    IblLicState2.Text = ScriptFilter.FilterScript(I18nUtil.DisplayStateForI18N(licenseModelRef.licState, licenseModelRef.countryCode));
                }

                IblLicenseNumber.Text = string.Format("{0}{1}", ACAConstant.SPLIT_CHAR4, ScriptFilter.FilterScript(licenseModel.licenseNbr));
            }

            // contact type #
            if (!string.IsNullOrEmpty(licenseModel.typeFlag)
                && gviewBll.IsFieldVisible(models, "ddlContactType"))
            {
                IblContactType.Text = I18nStringUtil.FormatToTableRow(BasePage.GetStaticTextByKey("licenseedit_licensepro_contacttype"), DropDownListBindUtil.GetTypeFlagTextByValue(licenseModel.typeFlag));
            }

            // social security #
            if (!string.IsNullOrEmpty(licenseModel.socialSecurityNumber)
                && gviewBll.IsFieldVisible(models, "txtSSN"))
            {
                IblSSN.Text = I18nStringUtil.FormatToTableRow(BasePage.GetStaticTextByKey("licenseedit_licensepro_ssn"), MaskUtil.FormatSSNShow(licenseModel.socialSecurityNumber));
            }

            // FEIN field#
            if (!string.IsNullOrEmpty(licenseModel.fein)
                && gviewBll.IsFieldVisible(models, "txtFEIN"))
            {
                IblFEIN.Text = I18nStringUtil.FormatToTableRow(BasePage.GetStaticTextByKey("licenseedit_licensepro_fein"), MaskUtil.FormatFEINShow(licenseModel.fein, StandardChoiceUtil.IsEnableFeinMasking()));
            }

            // Contractor License #
            if (!string.IsNullOrEmpty(licenseModel.contrLicNo)
                && gviewBll.IsFieldVisible(models, "txtContractorLicNO"))
            {
                IblContractorLicNo.Text = I18nStringUtil.FormatToTableRow(BasePage.GetStaticTextByKey("licenseedit_licensepro_label_contractorlicno"), licenseModel.contrLicNo);
            }

            // Contractor Business Name
            if (!string.IsNullOrEmpty(licenseModel.contLicBusName)
                && gviewBll.IsFieldVisible(models, "txtContractorBusiName"))
            {
                IblContractorBusNam.Text = I18nStringUtil.FormatToTableRow(BasePage.GetStaticTextByKey("licenseedit_licensepro_label_contractorbusiname"), licenseModel.contLicBusName);
            }

            ModelUIFormat.HiddenEmptyRow(tbLicenseBasicDetail);
            ModelUIFormat.HiddenEmptyRow(tbLicenseExtDetail);
        }

        /// <summary>
        /// Display license summary in cap confirm.
        /// </summary>
        /// <param name="licensePatterns">License patterns</param>
        /// <param name="licenseModel">License professional model</param>
        /// <param name="models">Simple view element model</param>
        /// <param name="gviewBll">GView business</param>
        private void DisplayInSpearForm(string licensePatterns, LicenseProfessionalModel licenseModel, SimpleViewElementModel4WS[] models, IGviewBll gviewBll)
        {
            divSpearFormLicenseView.Visible = true;
            string salutation = string.Empty;
            string chineseSalutation = string.Empty;
            string firstName = string.Empty;
            string middleName = string.Empty;
            string lastName = string.Empty;
            string suffix = string.Empty;
            string birthDate = string.Empty;
            string gender = string.Empty;
            string nameBusiness = string.Empty;
            string nameBusiness2 = string.Empty;
            string businessLicense = string.Empty;
            string address1 = string.Empty;
            string address2 = string.Empty;
            string address3 = string.Empty;
            string email = string.Empty;
            string city = string.Empty;
            string state = string.Empty;
            string zip = string.Empty;
            string country = string.Empty;
            string poBox = string.Empty;
            string homePhone = string.Empty;
            string mobilePhone = string.Empty;
            string fax = string.Empty;
            string classCode = string.Empty;
            string licState = string.Empty;
            string licenseType = string.Empty;
            string classCode2 = string.Empty;
            string licState2 = string.Empty;
            string licenseNumber = string.Empty;
            string contactType = string.Empty;
            string ssn = string.Empty;
            string fein = string.Empty;
            string contractorLicNo = string.Empty;
            string contractorBusNam = string.Empty;
                
            if (!string.IsNullOrEmpty(licenseModel.salutation) && gviewBll.IsFieldVisible(models, "ddlSalutation"))
            {
                string tempValue = StandardChoiceUtil.GetStandardChoiceValueByKey(BizDomainConstant.STD_CAT_SALUTATION, licenseModel.salutation);

                if (!I18nCultureUtil.IsChineseCultureEnabled)
                {
                    // Salutation
                    salutation = tempValue;
                }
                else
                {
                    // Chinese Salutation
                    chineseSalutation = tempValue;
                }
            }
            
            firstName = GetFieldValueWithBlank(models, licenseModel.contactFirstName, "txtFirstName");
            middleName = GetFieldValueWithBlank(models, licenseModel.contactMiddleName, "txtMiddleName");
            lastName = GetFieldValueWithBlank(models, licenseModel.contactLastName, "txtLastName");
            suffix = GetFieldValueWithBlank(models, licenseModel.suffixName, "txtSuffix");
          
            // Birth Date
            if (licenseModel.birthDate != null && gviewBll.IsFieldVisible(models, "txtBirthDate"))
            {
                birthDate = I18nDateTimeUtil.FormatToDateStringForUI(licenseModel.birthDate.Value);
            }

            // Gender
            if (!string.IsNullOrEmpty(licenseModel.gender) && gviewBll.IsFieldVisible(models, "radioListGender"))
            {
                gender = ScriptFilter.FilterScript(StandardChoiceUtil.GetGenderByKey(licenseModel.gender));
            }

            // Business Name
            nameBusiness = GetFieldValue(models, licenseModel.businessName, "txtBusName");

            // Business Name2
            if (!string.IsNullOrEmpty(licenseModel.busName2) && gviewBll.IsFieldVisible(models, "txtBusName2"))
            {
                nameBusiness2 = ScriptFilter.FilterScript(licenseModel.busName2);
            }

            businessLicense = GetFieldValue(models, licenseModel.businessLicense, "txtBusLicense");
            address1 = GetFieldValue(models, licenseModel.address1, "txtAddress1");
            address2 = GetFieldValue(models, licenseModel.address2, "txtAddress2");
            address3 = GetFieldValue(models, licenseModel.address3, "txtAddress3");
            email = GetFieldValue(models, licenseModel.email, "txtEmail");
            city = GetFieldValueWithBlank(models, licenseModel.city, "txtCity");

            // State
            if (!string.IsNullOrEmpty(licenseModel.resState) && gviewBll.IsFieldVisible(models, "txtState") && StandardChoiceUtil.IsDisplayLicenseState())
            {
                state = ScriptFilter.FilterScript(I18nUtil.DisplayStateForI18N(licenseModel.state, licenseModel.countryCode));
            }

            // Zip
            if (!string.IsNullOrEmpty(licenseModel.zip) && gviewBll.IsFieldVisible(models, "txtZipCode"))
            {
                zip = ModelUIFormat.FormatZipShow(licenseModel.zip, licenseModel.countryCode);
            }

            // Country Code
            if (!string.IsNullOrEmpty(licenseModel.countryCode) && gviewBll.IsFieldVisible(models, "ddlCountry"))
            {
                country = ScriptFilter.FilterScript(StandardChoiceUtil.GetCountryByKey(licenseModel.countryCode));
            }

            // Post Office Box
            if (!string.IsNullOrEmpty(licenseModel.postOfficeBox) && gviewBll.IsFieldVisible(models, "txtPOBox"))
            {
                poBox = ScriptFilter.FilterScript(licenseModel.postOfficeBox);
            }

            // Home Phone
            if (!string.IsNullOrEmpty(licenseModel.phone1) && gviewBll.IsFieldVisible(models, "txtHomePhone"))
            {
                homePhone = ModelUIFormat.FormatPhoneShow(licenseModel.phone1CountryCode, licenseModel.phone1, licenseModel.countryCode);
            }

            // Mobile Phone
            if (!string.IsNullOrEmpty(licenseModel.phone2) && gviewBll.IsFieldVisible(models, "txtMobilePhone"))
            {
                mobilePhone = ModelUIFormat.FormatPhoneShow(licenseModel.phone2CountryCode, licenseModel.phone2, licenseModel.countryCode);
            }

            // Fax
            if (!string.IsNullOrEmpty(licenseModel.fax) && gviewBll.IsFieldVisible(models, "txtFax"))
            {
                fax = ModelUIFormat.FormatPhoneShow(licenseModel.faxCountryCode, licenseModel.fax, licenseModel.countryCode);
            }

            LicenseModel4WS licenseModelRef = new LicenseModel4WS();

            // Get Reference License Model based on License Type & Number to display license state
            if (!string.IsNullOrEmpty(licenseModel.licenseNbr) && !string.IsNullOrEmpty(licenseModel.licenseType))
            {
                LicenseModel4WS license = new LicenseModel4WS();
                license.serviceProviderCode = ConfigManager.AgencyCode;
                license.licenseType = licenseModel.licenseType;
                license.stateLicense = licenseModel.licenseNbr;

                ILicenseBLL licenseBll = ObjectFactory.GetObject<ILicenseBLL>();
                licenseModelRef = licenseBll.GetLicenseByStateLicNbr(license);
            }

            // License Type
            if (!string.IsNullOrEmpty(licenseModel.resLicenseType) && gviewBll.IsFieldVisible(models, "ddlLicenseType"))
            {
                if (!string.IsNullOrEmpty(licenseModel.classCode))
                {
                    //here class code standand for licState
                    classCode = string.Format("{0}{1}", ScriptFilter.FilterScript(licenseModel.classCode), ACAConstant.BLANK);
                }

                if (licenseModelRef != null
                    && !string.IsNullOrEmpty(licenseModelRef.licState)
                    && StandardChoiceUtil.IsDisplayLicenseState())
                {
                    licState = string.Format("{0}{1}", ScriptFilter.FilterScript(I18nUtil.DisplayStateForI18N(licenseModelRef.licState, licenseModelRef.countryCode)), ACAConstant.BLANK);
                }

                licenseType = string.Format("{0}{1}", ScriptFilter.FilterScript(licenseModel.resLicenseType), ACAConstant.BLANK);
            }

            // License Number
            if (!string.IsNullOrEmpty(licenseModel.licenseNbr) && gviewBll.IsFieldVisible(models, "txtLicenseNum"))
            {
                if (!string.IsNullOrEmpty(licenseModel.classCode))
                {
                    //here class code standand for licState
                    classCode2 = string.Format("{0}{1}", ScriptFilter.FilterScript(licenseModel.classCode), ACAConstant.BLANK);
                }

                if (licenseModelRef != null && !string.IsNullOrEmpty(licenseModelRef.licState) && StandardChoiceUtil.IsDisplayLicenseState())
                {
                    licState2 = ScriptFilter.FilterScript(I18nUtil.DisplayStateForI18N(licenseModelRef.licState, licenseModelRef.countryCode));
                }

                licenseNumber = ScriptFilter.FilterScript(licenseModel.licenseNbr);
            }

            // contact type #
            if (!string.IsNullOrEmpty(licenseModel.typeFlag) && gviewBll.IsFieldVisible(models, "ddlContactType"))
            {
                contactType = ScriptFilter.FilterScript(DropDownListBindUtil.GetTypeFlagTextByValue(licenseModel.typeFlag));
            }

            // social security #
            if (!string.IsNullOrEmpty(licenseModel.socialSecurityNumber) && gviewBll.IsFieldVisible(models, "txtSSN"))
            {
                ssn = ScriptFilter.FilterScript(MaskUtil.FormatSSNShow(licenseModel.socialSecurityNumber));
            }

            // FEIN field#
            if (!string.IsNullOrEmpty(licenseModel.fein) && gviewBll.IsFieldVisible(models, "txtFEIN"))
            {
                fein = ScriptFilter.FilterScript(MaskUtil.FormatFEINShow(licenseModel.fein, StandardChoiceUtil.IsEnableFeinMasking()));
            }

            // Contractor License #
            if (!string.IsNullOrEmpty(licenseModel.contrLicNo) && gviewBll.IsFieldVisible(models, "txtContractorLicNO"))
            {
                contractorLicNo = ScriptFilter.FilterScript(licenseModel.contrLicNo);
            }

            // Contractor Business Name
            if (!string.IsNullOrEmpty(licenseModel.contLicBusName) && gviewBll.IsFieldVisible(models, "txtContractorBusiName"))
            {
                contractorBusNam = ScriptFilter.FilterScript(licenseModel.contLicBusName);
            }

            string result =
                licensePatterns.Replace("$$Salutation$$", salutation)
                    .Replace("$$FirstName$$", firstName)
                    .Replace("$$MiddleName$$", middleName)
                    .Replace("$$LastName$$", lastName)
                    .Replace("$$Suffix$$", suffix)
                    .Replace("$$ChSalutation$$", chineseSalutation)
                    .Replace("$$BirthDate$$", birthDate)
                    .Replace("$$Gender$$", gender)
                    .Replace("$$BusinessName$$", nameBusiness)
                    .Replace("$$BusinessName2$$", nameBusiness2)
                    .Replace("$$BusinessLicense$$", businessLicense)
                    .Replace("$$AddressLine1$$", address1)
                    .Replace("$$AddressLine2$$", address2)
                    .Replace("$$AddressLine3$$", address3)
                    .Replace("$$City$$", city)
                    .Replace("$$State$$", state)
                    .Replace("$$Zip$$", zip)
                    .Replace("$$Country$$", country)
                    .Replace("$$PostOfficeBox$$", poBox)
                    .Replace("$$HomePhone$$", homePhone)
                    .Replace("$$MobilePhone$$", mobilePhone)
                    .Replace("$$Fax$$", fax)
                    .Replace("$$ClassCode$$", classCode)
                    .Replace("$$LicState$$", licState)
                    .Replace("$$LicenseType$$", licenseType)
                    .Replace("$$ClassCode2$$", classCode2)
                    .Replace("$$LicState2$$", licState2)
                    .Replace("$$LicenseNumber$$", licenseNumber)
                    .Replace("$$ContactType$$", contactType)
                    .Replace("$$SocialSecurityNumber$$", ssn)
                    .Replace("$$Fein$$", fein)
                    .Replace("$$ContractorLicNo$$", contractorLicNo)
                    .Replace("$$ContractorBusName$$", contractorBusNam)
                    .Replace("$$Email$$", email);

            lblLicenseViewInfo.Text = result;
        }

        #endregion Methods
    }
}