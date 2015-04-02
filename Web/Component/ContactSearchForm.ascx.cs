#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: ContactSearchForm.ascx.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2008-2014
 *
 *  Description:
 *
 *  Notes:
 *      $Id: ContactSearchForm.ascx.cs 277741 2014-08-20 10:31:02Z ACHIEVO\james.shi $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using System;
using System.Collections;
using System.Text;
using System.Web.UI;
using Accela.ACA.BLL.APO;
using Accela.ACA.Common;
using Accela.ACA.Common.Util;
using Accela.ACA.Web.Common;
using Accela.ACA.Web.Util;
using Accela.ACA.WSProxy;
using Accela.Web.Controls;

namespace Accela.ACA.Web.Component
{
    /// <summary>
    /// This class provide the ability to operation contact search form. 
    /// </summary>
    public partial class ContactSearchForm : BaseUserControl
    {
        #region Properties

        /// <summary>
        /// Gets contact client id prefix.
        /// </summary>
        public string ContactIDPrefix
        {
            get
            {
                return ddlContactType.ClientID.Replace("ddlContactType", string.Empty);
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
                sbControls.Append(",").Append(txtWorkPhone.ClientID);
                sbControls.Append(",").Append(txtMobilePhone.ClientID);
                sbControls.Append(",").Append(txtFax.ClientID);
                return sbControls.ToString();
            }
        }

        #endregion Properties

        #region Methods
        /// <summary>
        /// fill contact search form fields with search by contact info.
        /// </summary>
        /// <param name="capContactModel">a CapContactModel4WS</param>
        public void FillContactInfo(CapContactModel4WS capContactModel)
        {
            if (capContactModel == null || capContactModel.people == null)
            {
                return;
            }

            PeopleModel4WS peopleModel4WS = capContactModel.people;
            ddlContactType.SelectedValue = peopleModel4WS.contactType;
            ddlSalutation.SelectedValue = peopleModel4WS.salutation;
            ddlContactTypeFlag.SelectedValue = peopleModel4WS.contactTypeFlag;
            ddlGender.SelectedValue = peopleModel4WS.gender;

            //When the user select conatact type flag organization or others
            if (!ddlContactTypeFlag.SelectedValue.Equals(ContactType4License.Individual.ToString(), StringComparison.InvariantCultureIgnoreCase))
            {
                txtOrganizationName.Text = peopleModel4WS.businessName;
                txtTradeName.Text = peopleModel4WS.tradeName;
                txtFein.Text = peopleModel4WS.fein;
            }

            //When the user select conatact type flag individual or others
            if (!ddlContactTypeFlag.SelectedValue.Equals(ContactType4License.Organization.ToString(), StringComparison.InvariantCultureIgnoreCase))
            {
                txtFirstName.Text = peopleModel4WS.firstName;
                txtMiddleName.Text = peopleModel4WS.middleName;
                txtLastName.Text = peopleModel4WS.lastName;
                txtFullName.Text = peopleModel4WS.fullName;
                txtTitle.Text = peopleModel4WS.title;
                txtSSN.Text = peopleModel4WS.socialSecurityNumber;
            }

            txtBirthDate.Text2 = peopleModel4WS.birthDate;

            string countryCode = peopleModel4WS.countryCode;

            if (peopleModel4WS.compactAddress != null)
            {
                countryCode = peopleModel4WS.compactAddress.countryCode;
                DropDownListBindUtil.SetCountrySelectedValue(ddlCountry, peopleModel4WS.compactAddress.countryCode, false, true, true);
                txtStreetAdd1.Text = peopleModel4WS.compactAddress.addressLine1;
                txtStreetAdd2.Text = peopleModel4WS.compactAddress.addressLine2;
                txtStreetAdd3.Text = peopleModel4WS.compactAddress.addressLine3;
                txtCity.Text = peopleModel4WS.compactAddress.city;
                txtState.Text = peopleModel4WS.compactAddress.state;
                txtZip.Text = ModelUIFormat.FormatZipShow(peopleModel4WS.compactAddress.zip, countryCode);
            }

            txtPOBox.Text = peopleModel4WS.postOfficeBox;
            txtHomePhone.Text = ModelUIFormat.FormatPhone4EditPage(peopleModel4WS.phone1, countryCode);
            txtHomePhone.CountryCodeText = peopleModel4WS.phone1CountryCode;
            txtMobilePhone.Text = ModelUIFormat.FormatPhone4EditPage(peopleModel4WS.phone2, countryCode);
            txtMobilePhone.CountryCodeText = peopleModel4WS.phone2CountryCode;
            txtWorkPhone.Text = ModelUIFormat.FormatPhone4EditPage(peopleModel4WS.phone3, countryCode);
            txtWorkPhone.CountryCodeText = peopleModel4WS.phone3CountryCode;
            txtFax.Text = peopleModel4WS.fax;
            txtFax.CountryCodeText = peopleModel4WS.faxCountryCode;
            txtEmail.Text = peopleModel4WS.email;

            DropDownListBindUtil.SetCountrySelectedValue(ddlBirthplaceCountry, peopleModel4WS.birthRegion, false, true, true);
            ddlPreferredChannel.SelectedValue = peopleModel4WS.preferredChannel;
            ddlRace.SelectedValue = peopleModel4WS.race;
            txtPassportNumber.Text = peopleModel4WS.passportNumber;
            txtDriverLicenseNumber.Text = peopleModel4WS.driverLicenseNbr;
            ddlDriverLicenseState.Text = peopleModel4WS.driverLicenseState;
            txtStateNumber.Text = peopleModel4WS.stateIDNbr;
            txtBirthplaceCity.Text = peopleModel4WS.birthCity;
            ddlBirthplaceState.Text = peopleModel4WS.birthState;
            txtBusinessName2.Text = peopleModel4WS.businessName2;
            txtDeceasedDate.Text = peopleModel4WS.deceasedDate;
            txtSuffix.Text = peopleModel4WS.namesuffix;
        }

        /// <summary>
        /// Check whether has at least one search criteria has been entered when search by Contact. 
        /// </summary>
        /// <returns>true: input at least one condition; false: doesn't input any condition</returns>
        public bool CheckCondition()
        {
            bool isInputCondition = true;

            string zip = txtZip.Text.Trim();

            if (zip.EndsWith("-", StringComparison.InvariantCulture))
            {
                zip = zip.Replace("-", string.Empty).Trim();
            }

            if (ddlContactType.SelectedIndex < 1 && ddlContactTypeFlag.SelectedIndex < 1 && ddlSalutation.SelectedIndex < 1 && txtTitle.Text.Trim().Length == 0
                && txtFirstName.Text.Trim().Length == 0 && txtMiddleName.Text.Trim().Length == 0
                && txtBirthDate.Text.Trim().Length == 0 && ddlGender.SelectedIndex < 1
                && txtLastName.Text.Trim().Length == 0 && txtFullName.Text.Trim().Length == 0 && txtOrganizationName.Text.Trim().Length == 0 && txtTradeName.Text.Trim().Length == 0
                && txtSSN.Text.Trim().Length == 0 && txtFein.Text.Trim().Length == 0 && ddlCountry.SelectedIndex < 1
                && txtStreetAdd1.Text.Trim().Length == 0 && txtStreetAdd2.Text.Trim().Length == 0 && txtStreetAdd3.Text.Trim().Length == 0
                && txtCity.Text.Trim().Length == 0 && txtState.Text.Trim().Length == 0 && zip.Length == 0
                && txtPOBox.Text.Trim().Length == 0 && txtHomePhone.Text.Trim().Length == 0 && txtHomePhone.CountryCodeText.Trim().Length == 0
                && txtWorkPhone.Text.Trim().Length == 0 && txtWorkPhone.CountryCodeText.Trim().Length == 0
                && txtMobilePhone.Text.Trim().Length == 0 && txtMobilePhone.CountryCodeText.Trim().Length == 0
                && txtFax.Text.Trim().Length == 0 && txtFax.CountryCodeText.Trim().Length == 0 && txtEmail.Text.Trim().Length == 0
                && ddlPreferredChannel.SelectedIndex < 1 && ddlRace.SelectedIndex < 1 && txtPassportNumber.Text.Trim().Length == 0 && txtDriverLicenseNumber.Text.Trim().Length == 0
                && ddlDriverLicenseState.Text.Trim().Length == 0 && txtStateNumber.Text.Trim().Length == 0 && ddlBirthplaceCountry.SelectedIndex < 1
                && txtBirthplaceCity.Text.Trim().Length == 0 && ddlBirthplaceState.Text.Trim().Length == 0 && txtBusinessName2.Text.Trim().Length == 0 && txtDeceasedDate.Text.Trim().Length == 0
                && string.IsNullOrEmpty(txtSuffix.Text.Trim()))
            {
                isInputCondition = false;
            }

            return isInputCondition;
        }

        /// <summary>
        /// Initial search by contact section.
        /// </summary>
        public void InitContactForm()
        {
            ControlUtil.ClearValue(this, null);
            DropDownListBindUtil.BindContactType(ddlContactType, ModuleName, ContactTypeSource.Transaction);
            DropDownListBindUtil.BindSalutation(ddlSalutation);
            DropDownListBindUtil.BindContactType4License(ddlContactTypeFlag);
            DropDownListBindUtil.BindGender(ddlGender);
            DropDownListBindUtil.BindStandardChoise(ddlRace, BizDomainConstant.STD_CAT_RACE);
            DropDownListBindUtil.BindPreferredChannel(ddlPreferredChannel, ModuleName);

            divNoContactTemplate.Visible = AppSession.IsAdmin;

            ControlUtil.ClearRegionalSetting(ddlCountry, true, string.Empty, null, string.Empty);
        }

        /// <summary>
        /// Applies the regional setting.
        /// </summary>
        /// <param name="getDefault">if set to <c>true</c> [get default].</param>
        public void ApplyRegionalSetting(bool getDefault)
        {
            ControlUtil.ApplyRegionalSetting(IsPostBack, true, true, getDefault, ddlCountry);
            ControlUtil.ApplyRegionalSetting(IsPostBack, true, true, getDefault, ddlBirthplaceCountry);
        }

        /// <summary>
        /// Validate div row section.
        /// </summary>
        public void ShowDivSections()
        {
            divName.Visible = ddlSalutation.Visible || txtTitle.Visible || txtFirstName.Visible || txtMiddleName.Visible || txtLastName.Visible || txtFullName.Visible;
            divIdentity.Visible = txtBirthDate.Visible || ddlGender.Visible;
            divBusname.Visible = txtOrganizationName.Visible;
            divTradeName.Visible = txtTradeName.Visible;
            divFein.Visible = txtFein.Visible;
            divAddress1.Visible = txtStreetAdd1.Visible;
            divAddress2.Visible = txtStreetAdd2.Visible;
            divAddress3.Visible = txtStreetAdd3.Visible;
            divCity.Visible = txtCity.Visible;
            divState.Visible = txtState.Visible;
            divZipText.Visible = txtZip.Visible || txtPOBox.Visible;
            divCountry.Visible = ddlCountry.Visible;
            divPhone.Visible = txtHomePhone.Visible || txtWorkPhone.Visible || txtMobilePhone.Visible;
            divFax.Visible = txtFax.Visible;
            divEmail.Visible = txtEmail.Visible;
            divContactTypeFlag.Visible = ddlContactTypeFlag.Visible;
            divPreferredChannelAndRace.Visible = ddlPreferredChannel.Visible || ddlRace.Visible;
            divIndentity.Visible = txtPassportNumber.Visible || txtDriverLicenseNumber.Visible || ddlDriverLicenseState.Visible || txtStateNumber.Visible;
            divRegion.Visible = ddlBirthplaceCountry.Visible || txtBirthplaceCity.Visible || ddlBirthplaceState.Visible;
            divBusinessNameAndDeceasedDate.Visible = txtBusinessName2.Visible || txtDeceasedDate.Visible || txtSuffix.Visible;
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
        /// Reset the template Fields.
        /// </summary>
        public void ReSetTemplateFields()
        {
            EnableLoadContactTemplateLink(true);
            templateEdit.ResetControl();
            templateEdit.DisplayAttributes(null, ACAConstant.CAP_CONTACTS_TEMPLATE_FIELD_PREFIX);
            genericTemplate.ResetControl();
        }

        /// <summary>
        /// Keep Template Fields Display.
        /// </summary>
        public void InitTemplateFieldsDisplay()
        {
            bool collapseTemplate = !ACAConstant.COMMON_Y.Equals(hfContactExpanded.Value);
            EnableLoadContactTemplateLink(collapseTemplate);
        }

        /// <summary>
        /// Get cap contact model by web form.
        /// </summary>
        /// <returns>CapContactModel from contact form info.</returns>
        public CapContactModel4WS GetCapContactModel()
        {
            PeopleModel4WS peopleModel = new PeopleModel4WS();
            peopleModel.contactType = ddlContactType.SelectedValue;
            peopleModel.contactTypeFlag = ddlContactTypeFlag.SelectedValue;
            peopleModel.salutation = ddlSalutation.SelectedValue;

            peopleModel.businessName = this.txtOrganizationName.Text.Trim();
            peopleModel.tradeName = this.txtTradeName.Text.Trim();
            peopleModel.fein = this.txtFein.Text.Trim();
            
            peopleModel.firstName = txtFirstName.Text.Trim();
            peopleModel.middleName = this.txtMiddleName.Text.Trim();
            peopleModel.lastName = this.txtLastName.Text.Trim();
            peopleModel.fullName = this.txtFullName.Text.Trim();
            peopleModel.title = txtTitle.Text.Trim();
            peopleModel.socialSecurityNumber = this.txtSSN.Text.Trim();
            peopleModel.birthDate = I18nDateTimeUtil.ConvertDateTimeStringFromUIToWebService(txtBirthDate.Text.Trim());
            peopleModel.gender = this.ddlGender.SelectedValue;
            peopleModel.postOfficeBox = this.txtPOBox.Text.Trim();
            peopleModel.phone1 = txtHomePhone.GetPhone(this.ddlCountry.SelectedValue.Trim());
            peopleModel.phone1CountryCode = this.txtHomePhone.CountryCodeText.Trim();
            peopleModel.phone2 = txtMobilePhone.GetPhone(this.ddlCountry.SelectedValue.Trim());
            peopleModel.phone2CountryCode = this.txtMobilePhone.CountryCodeText.Trim();
            peopleModel.phone3 = txtWorkPhone.GetPhone(this.ddlCountry.SelectedValue.Trim());
            peopleModel.phone3CountryCode = this.txtWorkPhone.CountryCodeText.Trim();
            peopleModel.fax = txtFax.GetPhone(this.ddlCountry.SelectedValue.Trim());
            peopleModel.faxCountryCode = this.txtFax.CountryCodeText.Trim();
            peopleModel.email = txtEmail.Text.Trim();

            peopleModel.preferredChannel = ddlPreferredChannel.SelectedValue;
            peopleModel.race = ddlRace.SelectedValue;
            peopleModel.passportNumber = txtPassportNumber.Text.Trim();
            peopleModel.driverLicenseNbr = txtDriverLicenseNumber.Text.Trim();
            peopleModel.driverLicenseState = ddlDriverLicenseState.Text.Trim();
            peopleModel.stateIDNbr = txtStateNumber.Text.Trim();
            peopleModel.birthRegion = ddlBirthplaceCountry.SelectedValue;
            peopleModel.birthCity = txtBirthplaceCity.Text.Trim();
            peopleModel.birthState = ddlBirthplaceState.Text.Trim();
            peopleModel.businessName2 = txtBusinessName2.Text.Trim();
            peopleModel.deceasedDate = I18nDateTimeUtil.ConvertDateTimeStringFromUIToWebService(txtDeceasedDate.Text.Trim());
            peopleModel.namesuffix = txtSuffix.Text.Trim();

            peopleModel.attributes = GetAttributeModel();
            peopleModel.template = genericTemplate.GetTemplateModel(true, true);
            CompactAddressModel4WS compactAddress = new CompactAddressModel4WS();
            compactAddress.countryCode = ddlCountry.SelectedValue;
            compactAddress.addressLine1 = txtStreetAdd1.Text.Trim();
            compactAddress.addressLine2 = txtStreetAdd2.Text.Trim();
            compactAddress.addressLine3 = txtStreetAdd3.Text.Trim();
            compactAddress.city = this.txtCity.Text.Trim();
            compactAddress.state = this.txtState.Text.Trim();
            compactAddress.zip = this.txtZip.GetZip(ddlCountry.SelectedValue.Trim());

            peopleModel.compactAddress = compactAddress;
            CapContactModel4WS capContactModel = new CapContactModel4WS();
            capContactModel.people = peopleModel;

            return capContactModel;
        }

        /// <summary>
        /// Raises the <see cref="E:System.Web.UI.Control.Init" /> event.
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs" /> object that contains the event data.</param>
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            ddlCountry.BindItems();
            ddlCountry.SetCountryControls(
                txtZip,
                new AccelaStateControl[] { txtState, ddlDriverLicenseState },
                txtHomePhone,
                txtMobilePhone,
                txtWorkPhone,
                txtFax);

            ddlBirthplaceCountry.BindItems();
            ddlBirthplaceCountry.SetCountryControls(null, ddlBirthplaceState, null);
        }

        /// <summary>
        /// Raises the page load event
        /// </summary>
        /// <param name="sender">An object that contains the event sender.</param>
        /// <param name="e">A System.EventArgs object containing the event data.</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            ApplyRegionalSetting(false);
            ddlCountry.RelevantControlIDs = RelevantControlIDs;
            ddlCountry.RegisterScripts();
            
            if (AppSession.IsAdmin)
            {
                ddlContactType.AutoPostBack = false;
            }
        }

        /// <summary>
        /// Contact Type Selected Changed.
        /// </summary>
        /// <param name="sender">the sender object</param>
        /// <param name="e">then event handle.</param>
        protected void ContactType_SelectedChanged(object sender, EventArgs e)
        {
            if (ACAConstant.COMMON_N.Equals(hfContactExpanded.Value))
            {
                EnableLoadContactTemplateLink(true);
            }

            templateEdit.ResetControl();
            CreateOriginalContactTemplate(ddlContactType.SelectedValue);
            genericTemplate.ResetControl();
            CreateGenericTemplate(ddlContactType.SelectedValue);
            Page.FocusElement(ddlContactType.ClientID);

            divNoContactTemplate.Visible = (templateEdit.Controls.Count == 0 && genericTemplate.Controls.Count == 0) && ACAConstant.COMMON_Y.Equals(hfContactExpanded.Value);
        }

        /// <summary>
        /// LoadContactTemplate Button OnClick
        /// </summary>
        /// <param name="sender">An object that contains the event sender.</param>
        /// <param name="e">A System.EventArgs object containing the event data.</param>
        protected void LoadContactTemplate_OnClick(object sender, EventArgs e)
        {
            //1.Disable load contact template link button.
            EnableLoadContactTemplateLink(false);
            templateEdit.ResetControl();
            CreateOriginalContactTemplate(ddlContactType.SelectedValue);
            genericTemplate.ResetControl();
            CreateGenericTemplate(ddlContactType.SelectedValue);

            divNoContactTemplate.Visible = (templateEdit.Controls.Count == 0 && genericTemplate.Controls.Count == 0) && ACAConstant.COMMON_Y.Equals(hfContactExpanded.Value);
            Page.FocusElement("btnContactTemplateCollapse");
        }

        /// <summary>
        /// enable permit Load contact template Link.
        /// </summary>
        /// <param name="isEnable">IsEnable Flag</param>
        private void EnableLoadContactTemplateLink(bool isEnable)
        {
            if (!StandardChoiceUtil.IsDisplayContactTemplateCriteria(ModuleName) && !AppSession.IsAdmin)
            {
                divLoadContactTemplate.Visible = false;
                divExpandCollapse.Visible = false;
                divContactTemplate.Visible = false;
            }
            else
            {
                divLoadContactTemplate.Visible = isEnable;
                divExpandCollapse.Visible = !isEnable;
                divContactTemplate.Visible = !isEnable;
                hfContactExpanded.Value = isEnable ? string.Empty : ACAConstant.COMMON_Y;
            }

            lnkLoadContactTemplate.ToolTip = GetTitleByKey("img_alt_expand_icon", "aca_contactsearchfrom_expand");
        }

        /// <summary>
        /// Creates an original contact template.
        /// </summary>
        /// <param name="contactType">contact type.</param>
        private void CreateOriginalContactTemplate(string contactType)
        {
            if (string.IsNullOrEmpty(contactType))
            {
                templateEdit.DisplayAttributes(null, ACAConstant.CAP_CONTACTS_TEMPLATE_FIELD_PREFIX);
            }
            else
            {
                ITemplateBll templateBll = (ITemplateBll)ObjectFactory.GetObject(typeof(ITemplateBll));
                TemplateAttributeModel[] attributes;
                attributes = templateBll.GetPeopleTemplateAttributes(contactType, ConfigManager.AgencyCode, AppSession.User.PublicUserId);
                templateEdit.DisplayAttributes(attributes, ACAConstant.CAP_CONTACTS_TEMPLATE_FIELD_PREFIX);
            }
        }

        /// <summary>
        /// Create Generic Template
        /// </summary>
        /// <param name="contactType">Contact type</param>
        private void CreateGenericTemplate(string contactType)
        {
            if (string.IsNullOrEmpty(contactType))
            {
                genericTemplate.Display(null);
            }
            else
            {
                ITemplateBll templateBll = (ITemplateBll)ObjectFactory.GetObject(typeof(ITemplateBll));
                TemplateModel templateModel = templateBll.GetContactTemplates(ConfigManager.AgencyCode, contactType, true, AppSession.User.UserSeqNum);
                ClearRequiredAndDefaultValue(templateModel);
                genericTemplate.Display(templateModel);
            }
        }

        /// <summary>
        /// Clear default value and required flag for search form.
        /// </summary>
        /// <param name="templateModel">Template Model</param>
        private void ClearRequiredAndDefaultValue(TemplateModel templateModel)
        {
            var fields = GenericTemplateUtil.GetAllFields(templateModel);

            if (fields != null)
            {
                foreach (var field in fields)
                {
                    if (ValidationUtil.IsYes(field.requireFlag))
                    {
                        field.requireFlag = ACAConstant.COMMON_N;
                    }

                    if (!string.IsNullOrEmpty(field.defaultValue))
                    {
                        field.defaultValue = string.Empty;
                    }
                }
            }
        }

        /// <summary>
        /// If the template field doesn't have value, not include in attribute model.
        /// </summary>
        /// <returns>Template Attribute Model</returns>
        private TemplateAttributeModel[] GetAttributeModel()
        {
            ArrayList attributeArray = new ArrayList();

            if (ACAConstant.COMMON_Y.Equals(hfContactExpanded.Value))
            {
                TemplateAttributeModel[] attributes = templateEdit.GetAttributeModels();

                if (attributes != null && attributes.Length > 0)
                {
                    foreach (TemplateAttributeModel attribute in attributes)
                    {
                        if (!string.IsNullOrEmpty(attribute.attributeValue))
                        {
                            attributeArray.Add(attribute);
                        }
                    }
                }
            }

            if (attributeArray.Count > 0)
            {
                return (TemplateAttributeModel[])attributeArray.ToArray(typeof(TemplateAttributeModel));
            }

            return null;
        }

        #endregion Methods
    }
}
