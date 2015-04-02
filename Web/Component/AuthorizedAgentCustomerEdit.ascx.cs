#region Header
/**
 * <pre>
 * 
 *  Accela Citizen Access
 *  File: AuthorizedAgentCustomerEdit.ascx.cs
 * 
 *  Accela, Inc.
 *  Copyright (C): 2013-2014
 * 
 *  Description:
 * 
 *  Notes:
 * $Id: AuthorizedAgentCustomerEdit.ascx.cs 181867 2011-09-27 08:06:18Z ACHIEVO\daly.zeng $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */
#endregion Header

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Script.Serialization;
using System.Web.UI;
using System.Web.UI.WebControls;

using Accela.ACA.BLL.APO;
using Accela.ACA.BLL.People;
using Accela.ACA.Common;
using Accela.ACA.Common.Util;
using Accela.ACA.ExpressionBuild;
using Accela.ACA.Web.AuthorizedAgent;
using Accela.ACA.Web.Common;
using Accela.ACA.Web.Common.Control;
using Accela.ACA.Web.ExpressionBuild;
using Accela.ACA.Web.People;
using Accela.ACA.Web.Util;
using Accela.ACA.WSProxy;
using Accela.Web.Controls;

namespace Accela.ACA.Web.Component
{
    /// <summary>
    /// This class provider the authorized agent customer edit component.
    /// </summary>
    public partial class AuthorizedAgentCustomerEdit : FormDesignerWithExpressionControl
    {
        #region Fields

        /// <summary>
        /// Indicating whether the contact address is enabled.
        /// </summary>
        private readonly bool _isContactAddressEnabled = StandardChoiceUtil.IsEnableContactAddress();

        /// <summary>
        ///  Gets or sets the contact section position.
        /// </summary>
        private ACAConstant.AuthAgentCustomerSectionPosition _sectionPosition;

        /// <summary>
        /// Gets or sets a value indicating whether the customer detail from can edit or not.
        /// </summary>
        private bool _isEditable = true;

        /// <summary>
        /// ExpressionFactory class's instance
        /// </summary>
        private ExpressionFactory _expressionInstance;

        /// <summary>
        /// The session parameter string
        /// </summary>
        private string _sessionParameterString = string.Empty;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="AuthorizedAgentCustomerEdit" /> class.
        /// </summary>
        public AuthorizedAgentCustomerEdit()
            : base(GviewID.AuthAgentCustomerSearchForm)
        {
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        ///  Gets or sets the contact section position.
        /// </summary>
        public ACAConstant.AuthAgentCustomerSectionPosition SectionPosition
        {
            get
            {
                return _sectionPosition;
            }

            set
            {
                _sectionPosition = value;

                if (SectionPosition == ACAConstant.AuthAgentCustomerSectionPosition.DetailForm)
                {
                    ViewId = GviewID.AuthAgentCustomerDetail;
                }
                else if (SectionPosition == ACAConstant.AuthAgentCustomerSectionPosition.SearchForm)
                {
                    ViewId = GviewID.AuthAgentCustomerSearchForm;

                    // Remove force require setting for Search form in ACA Admin.
                    ddlContactTypeFlag.Attributes.Remove("IsFieldRequired");
                }
            }
        }

        /// <summary>
        ///  Gets or sets the contact section position.
        /// </summary>
        public ACAConstant.ContactSectionPosition ContactSectionPosition
        {
            get;
            set;
        }

        /// <summary>
        /// Gets the identity labels.
        /// </summary>
        public Dictionary<long, string> IdentityFieldLabels
        {
            get
            {
                //28, 29 Is AA gview id,use it only for match identity label key & value.
                return new Dictionary<long, string>
                {
                    { 28, txtSSN.LabelKey },
                    { 29, txtFein.LabelKey },
                    { 39, txtPassportNumber.LabelKey },
                    { 40, txtDriverLicenseNbr.LabelKey },
                    { 41, txtDriverLicenseState.LabelKey },
                    { 42, txtStateIdNbr.LabelKey },
                    { 5, txtEmail.LabelKey }
                };
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
                    ExpressionControls = CollectExpressionInputControls(GviewID.AuthAgentCustomerDetail, string.Empty, peopleTemplate, ACAConstant.REFERENCE_CONTACT_TEMPLATE_FIELD_PREFIX);
                    var argumentPKModels = new List<ExpressionRuntimeArgumentPKModel>();
                    var contactArgPk = new ExpressionRuntimeArgumentPKModel();
                    contactArgPk.portletID = (long?)ExpressionType.AuthAgent_Customer_Detail;
                    contactArgPk.viewKey1 = ddlContactType.SelectedValue;
                    contactArgPk.viewKey2 = string.Empty;
                    contactArgPk.viewKey3 = string.Empty;
                    argumentPKModels.Add(contactArgPk);

                    if (genericTemplate.ASITUITables != null)
                    {
                        foreach (var viewKey3 in genericTemplate.ASITUITables)
                        {
                            var genericTemplateTableArgPk = new ExpressionRuntimeArgumentPKModel();
                            genericTemplateTableArgPk.portletID = (long?)ExpressionType.AuthAgent_Customer_Detail;
                            genericTemplateTableArgPk.viewKey1 = ddlContactType.SelectedValue;
                            genericTemplateTableArgPk.viewKey2 = genericTemplate.GenericTemplateGroupCode;
                            genericTemplateTableArgPk.viewKey3 = viewKey3;
                            argumentPKModels.Add(genericTemplateTableArgPk);
                        }
                    }
                    else
                    {
                        var genericTemplateArgPk = new ExpressionRuntimeArgumentPKModel();
                        genericTemplateArgPk.portletID = (long?)ExpressionType.AuthAgent_Customer_Detail;
                        genericTemplateArgPk.viewKey1 = ddlContactType.SelectedValue;
                        genericTemplateArgPk.viewKey2 = genericTemplate.GenericTemplateGroupCode;
                        genericTemplateArgPk.viewKey3 = string.Empty;
                        argumentPKModels.Add(genericTemplateArgPk);
                    }

                    _expressionInstance = new ExpressionFactory(string.Empty, ExpressionType.AuthAgent_Customer_Detail, ExpressionControls, argumentPKModels, genericTemplate.UITables, ClientID);
                }

                return _expressionInstance;
            }
        }

        /// <summary>
        /// Gets or sets Permission.
        /// </summary>
        protected override GFilterScreenPermissionModel4WS Permission
        {
            get
            {
                string permissionValue = string.Empty;

                if (SectionPosition == ACAConstant.AuthAgentCustomerSectionPosition.DetailForm)
                {
                    permissionValue = ddlContactType.SelectedValue.Trim().Replace(ACAConstant.SPLIT_CHAR4 + ACAConstant.SPLIT_DOUBLE_VERTICAL, string.Empty);
                }

                base.Permission = ControlBuildHelper.GetPermissionWithGenericTemplate(ViewId, GViewConstant.PERMISSION_PEOPLE, permissionValue);

                return base.Permission;
            }

            set
            {
                base.Permission = value;
            }
        }

        /// <summary>
        /// Gets the session parameter string.
        /// </summary>
        /// <value>
        /// The session parameter string.
        /// </value>
        protected string SessionParameterString
        {
            get
            {
                if (string.IsNullOrEmpty(_sessionParameterString))
                {
                    ContactSessionParameter parameter = new ContactSessionParameter();
                    JavaScriptSerializer javaScriptSerializer = new JavaScriptSerializer();
                    
                    parameter.ContactExpressionType = ExpressionType.AuthAgent_Customer_Detail;
                    parameter.Process.CallbackFunctionName = ClientID;
                    parameter.ShowDetail = false;
                    parameter.ContactType = ddlContactType.GetValue();
                    parameter.ContactSectionPosition = ContactSectionPosition;

                    _sessionParameterString = javaScriptSerializer.Serialize(parameter);
                }

                return _sessionParameterString;
            }
        }

        /// <summary>
        ///  Gets or sets the contact type flag.
        /// </summary>
        protected string ContactTypeFlag
        {
            get;
            set;
        }

        /// <summary>
        ///  Gets or sets a value indicating whether the customer detail form is editable.
        /// </summary>
        protected bool IsEditable
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
        /// Gets the authorized service setting model
        /// </summary>
        private AuthorizedServiceSettingModel AuthSettingModel
        {
            get
            {
                if (ViewState["AuthSettingModel"] == null)
                {
                    ViewState["AuthSettingModel"] = AuthorizedAgentServiceUtil.GetAuthorizedServiceSetting();
                }

                return ViewState["AuthSettingModel"] as AuthorizedServiceSettingModel;
            }
        }

        /// <summary>
        /// Gets or sets the Cap Contact search model.
        /// </summary>
        private CapContactModel SearchModelForCustomer
        {
            get
            {
                return Session["CapContactSearchModel"] as CapContactModel;
            }

            set
            {
                Session["CapContactSearchModel"] = value;
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
                sbControls.Append(",").Append(txtPhone3.ClientID);
                sbControls.Append(",").Append(txtFax.ClientID);

                return sbControls.ToString();
            }
        }
        
        /// <summary>
        ///  Gets or sets a value indicating whether the contact type is editable when it is reference contact.
        /// </summary>
        private bool IsRefContactTypeEditable
        {
            get
            {
                object result = ViewState["IsRefContactTypeEditable"];

                if (result == null)
                {
                    return true;
                }
                else
                {
                    return bool.Parse(result.ToString());
                }
            }

            set
            {
                ViewState["IsRefContactTypeEditable"] = value;
            }
        }
        #endregion Properties

        #region Public Methods

        /// <summary>
        /// Set current City and State
        /// </summary>
        public void SetCurrentCityAndState()
        {
            AutoFillCityAndStateUtil.SetCurrentCity(txtCity, ModuleName);
            AutoFillCityAndStateUtil.SetCurrentState(txtState, ModuleName);
        }

        /// <summary>
        /// Fill the customer form
        /// </summary>
        /// <param name="capContactModel">The cap contact model</param>
        public void FillCustomerForm(CapContactModel capContactModel)
        {
            if (capContactModel == null || capContactModel.people == null)
            {
                return;
            }

            PeopleModel people = capContactModel.people;

            // 1. fill the standard fields
            FillCustomerForm4StandardFields(capContactModel);

            // 2. bind contact address list when only search out one reference contact.
            if (_isContactAddressEnabled
                && people.contactAddressLists != null
                && people.contactAddressLists.Length > 0)
            {
                ucContactAddressList.IsRefContact = !string.IsNullOrEmpty(people.contactSeqNumber);
                ucContactAddressList.Display(ObjectConvertUtil.ConvertArrayToList(people.contactAddressLists), false);
            }

            // 3. fill the templateEdit fields
            ResetTemplate();

            // clear template when there is no contact type is specified
            if (string.IsNullOrEmpty(people.contactType))
            {
                peopleTemplate.DisplayAttributes(null, ACAConstant.REFERENCE_CONTACT_TEMPLATE_FIELD_PREFIX);
                genericTemplate.ResetControl();
            }
            else
            {
                CreatePeopleTemplate(people.contactType);
                ITemplateBll templateBll = ObjectFactory.GetObject<ITemplateBll>();
                TemplateAttributeModel[] attributes = templateBll.GetRefPeopleTemplateAttributes(people.contactType, people.contactSeqNumber, ConfigManager.AgencyCode, AppSession.User.PublicUserId);
                peopleTemplate.DisplayAttributes(attributes, ACAConstant.REFERENCE_CONTACT_TEMPLATE_FIELD_PREFIX);

                if (SectionPosition == ACAConstant.AuthAgentCustomerSectionPosition.DetailForm)
                {
                    /*
                     * For single contact section, the contact type of reference contact may not match the current contact section.
                     * So merge the fields' values from the reference contact to the template fields associated with the current contact type.
                     */
                    people.template = MergeGenericTemplate(people.template, people.contactType);
                }

                genericTemplate.ResetControl();
                genericTemplate.Display(people.template);
            }

            /* if current contact type is Transactions. contact type dropdown list can't be disable.
             * only contact is refrence and contact type not Transactions contact dropdown list is disable.
             */
            if (!string.IsNullOrEmpty(Request["id"]) && ddlContactType.SelectedIndex > 0 && !string.IsNullOrEmpty(ddlContactType.SelectedValue))
            {
                IsRefContactTypeEditable = false;
            }
            else
            {
                IsRefContactTypeEditable = true;
            }

            if (!string.IsNullOrEmpty(hdnContactSeqNumber.Value))
            {
                //Cache the reference data.
                RefEntityCache = people;
                DisableCustomerFormBySetting();
            }
        }

        /// <summary>
        /// Fill the customer form's standard fields.
        /// </summary>
        /// <param name="capContactModel">The CapContactModel.</param>
        public void FillCustomerForm4StandardFields(CapContactModel capContactModel)
        {
            ControlUtil.ClearValue(this, null);
            IsAppliedRegional = true;

            if (capContactModel == null || capContactModel.people == null)
            {
                return;
            }

            PeopleModel people = capContactModel.people;
            bool isSearchForm = SectionPosition == ACAConstant.AuthAgentCustomerSectionPosition.SearchForm;

            // fill the fields only for search form
            if (isSearchForm)
            {
                txtCustomerID.Text = people.contactSeqNumber;
                txtLicenseID.Text = capContactModel.capID.customID;
                DropDownListBindUtil.SetSelectedValue(ddlRecordType, Request.QueryString[UrlConstant.CAPTYPE]);
            }

            DropDownListBindUtil.SetSelectedValue(ddlContactType, people.contactType);
            DropDownListBindUtil.SetSelectedValue(ddlSalutation, people.salutation);
            DropDownListBindUtil.SetSelectedValue(ddlContactTypeFlag, people.contactTypeFlag);
            ContactTypeFlag = people.contactTypeFlag == null ? string.Empty : people.contactTypeFlag;

            DropDownListBindUtil.SetSelectedValue(ddlPreferredChannel, people.preferredChannel == null ? string.Empty : people.preferredChannel.ToString());

            if (!string.IsNullOrEmpty(Request["id"]))
            {
                hdnContactSeqNumber.Value = people.contactSeqNumber;
            }

            txtFirstName.Text = people.firstName;
            txtLastName.Text = people.lastName;
            txtMiddleName.Text = people.middleName;
            txtFullName.Text = people.fullName;
            txtPostOfficeBox.Text = people.postOfficeBox;
            txtEmail.Text = people.email;
            DropDownListBindUtil.SetSelectedValueForRadioList(rdolistGender, people.gender);
            txtTradeName.Text = people.tradeName;
            txtTitle.Text = people.title;

            txtFein.Text = MaskUtil.FormatFEINShow(people.fein, StandardChoiceUtil.IsEnableFeinMasking());
            hfFEIN.Value = people.fein;

            txtSSN.Text = MaskUtil.FormatSSNShow(people.socialSecurityNumber);
            hfSSN.Value = people.socialSecurityNumber;

            txtBusinessName.Text = people.businessName;
            txtBusinessName2.Text = people.businessName2;
            DropDownListBindUtil.SetCountrySelectedValue(ddlBirthCountry, people.birthRegion, false, true, false);
            txtBirthCity.Text = people.birthCity;
            txtBirthState.Text = people.birthState;
            txtSuffix.Text = people.namesuffix;
            DropDownListBindUtil.SetSelectedValue(ddlRace, people.race);
            txtPassportNumber.Text = people.passportNumber;
            txtDriverLicenseNbr.Text = people.driverLicenseNbr;
            txtStateIdNbr.Text = people.stateIDNbr;
            txtNotes.Text = people.comment;
            txtBirthDate.Text2 = people.birthDate;
            txtDeceasedDate.Text2 = people.deceasedDate;

            CompactAddressModel addressModel = people.compactAddress;

            if (addressModel != null)
            {
                DropDownListBindUtil.SetCountrySelectedValue(ddlCountry, addressModel.countryCode, false, true, isSearchForm);

                txtPhone1.CountryCodeText = people.phone1CountryCode;
                txtPhone1.Text = ModelUIFormat.FormatPhone4EditPage(people.phone1, addressModel.countryCode);
                txtPhone2.CountryCodeText = people.phone2CountryCode;
                txtPhone2.Text = ModelUIFormat.FormatPhone4EditPage(people.phone2, addressModel.countryCode);
                txtPhone3.CountryCodeText = people.phone3CountryCode;
                txtPhone3.Text = ModelUIFormat.FormatPhone4EditPage(people.phone3, addressModel.countryCode);
                txtFax.CountryCodeText = people.faxCountryCode;
                txtFax.Text = ModelUIFormat.FormatPhone4EditPage(people.fax, addressModel.countryCode);
                txtZip.Text = ModelUIFormat.FormatZipShow(addressModel.zip, addressModel.countryCode, false);

                txtAddressLine1.Text = addressModel.addressLine1;
                txtAddressLine2.Text = addressModel.addressLine2;
                txtAddressLine3.Text = addressModel.addressLine3;
                txtCity.Text = addressModel.city;
                txtState.Text = addressModel.state;
            }
            else
            {
                DropDownListBindUtil.SetCountrySelectedValue(ddlCountry, string.Empty, false, true, isSearchForm);
            }

            txtDriverLicenseState.Text = people.driverLicenseState;
        }

        /// <summary>
        /// Validates the contact address.
        /// </summary>
        /// <returns>validate message</returns>
        public string ValidateContactAddress()
        {
            PeopleModel people = GetPeopleModel(false);

            return ucContactAddressList.Validate(people.contactAddressLists, true, true);
        }

        /// <summary>
        /// Get the people model.
        /// </summary>
        /// <param name="ignoreVisible">Is Ignore Visible Flag</param>
        /// <returns>The people model.</returns>
        public PeopleModel GetPeopleModel(bool ignoreVisible = true)
        {
            PeopleModel people = new PeopleModel();

            string countryCode = ControlUtil.GetControlValue(ddlCountry, ignoreVisible);
            people.countryCode = countryCode;
            people.serviceProviderCode = ConfigManager.AgencyCode;
            people.auditStatus = ACAConstant.VALID_STATUS;
            people.contactSeqNumber = SectionPosition == ACAConstant.AuthAgentCustomerSectionPosition.SearchForm ? ControlUtil.GetControlValue(txtCustomerID, ignoreVisible) : hdnContactSeqNumber.Value;
            people.firstName = ControlUtil.GetControlValue(txtFirstName, ignoreVisible);
            people.lastName = ControlUtil.GetControlValue(txtLastName, ignoreVisible);
            people.middleName = ControlUtil.GetControlValue(txtMiddleName, ignoreVisible);
            people.fullName = ControlUtil.GetControlValue(txtFullName, ignoreVisible);
            people.phone1 = txtPhone1.GetPhone(ddlCountry.SelectedValue.Trim(), ignoreVisible);
            people.phone1CountryCode = ControlUtil.GetCountryCodeText(txtPhone1, ignoreVisible);
            people.phone2 = txtPhone2.GetPhone(ddlCountry.SelectedValue.Trim(), ignoreVisible);
            people.phone2CountryCode = ControlUtil.GetCountryCodeText(txtPhone2, ignoreVisible);
            people.phone3 = txtPhone3.GetPhone(ddlCountry.SelectedValue.Trim(), ignoreVisible);
            people.phone3CountryCode = ControlUtil.GetCountryCodeText(txtPhone3, ignoreVisible);
            people.fax = txtFax.GetPhone(ddlCountry.SelectedValue.Trim(), ignoreVisible);
            people.faxCountryCode = ControlUtil.GetCountryCodeText(txtFax, ignoreVisible);
            people.postOfficeBox = ControlUtil.GetControlValue(txtPostOfficeBox, ignoreVisible);
            people.email = ControlUtil.GetControlValue(txtEmail, ignoreVisible);
            people.salutation = ControlUtil.GetControlValue(ddlSalutation, ignoreVisible);
            people.gender = ControlUtil.GetControlValue(rdolistGender, ignoreVisible);
            people.contactType = ControlUtil.GetControlValue(ddlContactType, ignoreVisible);
            people.contactTypeFlag = ControlUtil.GetControlValue(ddlContactTypeFlag, ignoreVisible);
            people.tradeName = ControlUtil.GetControlValue(txtTradeName, ignoreVisible);
            people.title = ControlUtil.GetControlValue(txtTitle, ignoreVisible);
            people.businessName = ControlUtil.GetControlValue(txtBusinessName, ignoreVisible);
            people.businessName2 = ControlUtil.GetControlValue(txtBusinessName2, ignoreVisible);
            people.birthCity = ControlUtil.GetControlValue(txtBirthCity, ignoreVisible);
            people.birthState = ControlUtil.GetControlValue(txtBirthState, ignoreVisible);
            people.birthRegion = ControlUtil.GetControlValue(ddlBirthCountry, ignoreVisible);
            people.namesuffix = ControlUtil.GetControlValue(txtSuffix, ignoreVisible);
            people.race = ControlUtil.GetControlValue(ddlRace, ignoreVisible);
            people.comment = ControlUtil.GetControlValue(txtNotes, ignoreVisible);

            string birthDate = txtBirthDate.Text.Trim();
            if (!string.IsNullOrEmpty(birthDate))
            {
                people.birthDate = I18nDateTimeUtil.ParseFromUI(birthDate);
            }

            string deceasedDate = txtDeceasedDate.Text.Trim();
            if (!string.IsNullOrEmpty(deceasedDate))
            {
                people.deceasedDate = I18nDateTimeUtil.ParseFromUI(deceasedDate);
            }

            AppendIdentityFieldsValue(people, ignoreVisible);

            // address model
            CompactAddressModel addressModel = new CompactAddressModel();
            addressModel.addressLine1 = ControlUtil.GetControlValue(txtAddressLine1, ignoreVisible);
            addressModel.addressLine2 = ControlUtil.GetControlValue(txtAddressLine2, ignoreVisible);
            addressModel.addressLine3 = ControlUtil.GetControlValue(txtAddressLine3, ignoreVisible);
            addressModel.city = ControlUtil.GetControlValue(txtCity, ignoreVisible);
            addressModel.state = ControlUtil.GetControlValue(txtState, ignoreVisible);
            addressModel.zip = txtZip.Visible || ignoreVisible ? txtZip.GetZip(ddlCountry.SelectedValue) : string.Empty;

            addressModel.countryCode = countryCode;

            people.compactAddress = addressModel;

            // Detail form fields
            if (SectionPosition == ACAConstant.AuthAgentCustomerSectionPosition.DetailForm)
            {
                string preferredChannel = ControlUtil.GetControlValue(ddlPreferredChannel, ignoreVisible);
                people.preferredChannel = string.IsNullOrEmpty(preferredChannel) ? (int?)null : int.Parse(preferredChannel);

                people.auditID = AppSession.User.PublicUserId;

                if (_isContactAddressEnabled)
                {
                    people.contactAddressLists = ucContactAddressList.GetContactAddresses();
                }

                people.attributes = peopleTemplate.GetAttributeModels();
                people.template = genericTemplate.GetTemplateModel(true);

                PeopleModel4WS people4WS = TempModelConvert.ConvertToPeopleModel4WS(people);
                PeopleUtil.SetPeopleTemplateContactSeqNum(people4WS);

                if (!string.IsNullOrEmpty(hdnContactSeqNumber.Value))
                {
                    //Merge the reference data and UI data to prevent lose the data which fields hidden by form designer.
                    CapUtil.MergeRefDataToUIData<PeopleModel, PeopleModel>(
                        ref people,
                        "peopleModel",
                        "people*",
                        "contactSeqNumber",
                        hdnContactSeqNumber.Value,
                        string.Empty,
                        RefEntityCache,
                        Permission,
                        ViewId);
                }
            }

            return people;
        }

        #endregion Public Methods

        #region Protected Events

        /// <summary>
        /// Initial event method
        /// </summary>
        /// <param name="e">EventArgs e</param>
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            genericTemplate.ASITUIDataKey = ClientID;
            DropDownListBindUtil.BindSalutation(ddlSalutation);
            DropDownListBindUtil.BindGender(rdolistGender);
            DropDownListBindUtil.BindStandardChoise(ddlRace, BizDomainConstant.STD_CAT_RACE);
            DropDownListBindUtil.BindContactType4License(ddlContactTypeFlag);
            DropDownListBindUtil.BindRecordTypeByFilterName(ddlRecordType, AuthSettingModel.ModuleName, AuthSettingModel.CapTypeFilterName);
            DropDownListBindUtil.BindPreferredChannel(ddlPreferredChannel, AuthSettingModel.ModuleName);

            string contactSeqNbr = Request["id"];
            bool isContactTypeEmptyOrDisable = false;

            //if it is edit page, check contact empty or disable, if it is, Bind ContactType InRegistration
            if (!string.IsNullOrEmpty(contactSeqNbr))
            {
                PeopleModel people = AppSession.GetPeopleModelFromSession(contactSeqNbr);

                if (people == null || string.IsNullOrEmpty(people.contactType) || !ContactUtil.IsContactTypeEnable(people.contactType, ContactTypeSource.Reference, string.Empty, string.Empty))
                {
                    isContactTypeEmptyOrDisable = true;
                }
            }

            if (SectionPosition == ACAConstant.AuthAgentCustomerSectionPosition.DetailForm && (string.IsNullOrEmpty(contactSeqNbr) || AppSession.IsAdmin || isContactTypeEmptyOrDisable))
            {
                DropDownListBindUtil.BindContactTypeInRegistration(ddlContactType, XPolicyConstant.CONTACT_TYPE_CUSTOMERDETAIL);
            }
            else
            {
                DropDownListBindUtil.BindContactType(ddlContactType, ContactTypeSource.Reference);
            }

            ddlCountry.BindItems();
            ddlBirthCountry.BindItems();

            ddlCountry.SetCountryControls(txtZip, new[] { txtState, txtDriverLicenseState }, txtPhone1, txtPhone2, txtPhone3, txtFax);
            ddlBirthCountry.SetCountryControls(null, txtBirthState, null);
            ucContactAddressList.IsEditable = 
                AppSession.IsAdmin 
                || (_isContactAddressEnabled && (string.IsNullOrEmpty(Request["id"]) || StandardChoiceUtil.IsCustomerDetailEditable()));

            // display contact address
            if ((!IsPostBack && _isContactAddressEnabled) || AppSession.IsAdmin)
            {
                ucContactAddressList.Display(null, false);
            }

            if (!IsPostBack)
            {
                // template fields not support search
                if (SectionPosition == ACAConstant.AuthAgentCustomerSectionPosition.SearchForm)
                {
                    ddlContactType.AutoPostBack = false;
                    ddlContactType.SelectedIndexChanged -= ContactType_SelectedIndexChanged;
                }
                else if (AppSession.IsAdmin)
                {
                    ddlContactType.AutoPostBack = false;
                    ddlContactType.Attributes.Add("onfocus", "javascript:if(typeof(parent.GetOldValueBeforeValueChange)!='undefined'){parent.GetOldValueBeforeValueChange(this);}");
                    ddlContactType.Attributes.Add("onchange", "javascript:if(typeof(parent.ChangedItemsValidateBeforePostBack)!='undefined'){parent.ChangedItemsValidateBeforePostBack(this,window);}");
                }

                // Initial related template and contact address if there only one contact type available.
                if (!AppSession.IsAdmin && !string.IsNullOrEmpty(ddlContactType.SelectedValue.Trim()))
                {
                    SetTemplateAndContactAddress(ddlContactType.SelectedValue.Trim());
                }
            }
        }

        /// <summary>
        /// Page load event method
        /// </summary>
        /// <param name="sender">The sender object.</param>
        /// <param name="e">The EventArgs object.</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            //Add additional instruction for the Contact Form will be changes after selecting a contact type.
            if (ddlContactType.Visible && ddlContactType.AutoPostBack)
            {
                ddlContactType.ToolTipLabelKey = "aca_common_msg_dropdown_updateformlayout_tip";
            }

            ddlCountry.RelevantControlIDs = RelevantControlIDs;
            ddlCountry.RegisterScripts();

            string contactType = ddlContactType.SelectedValue.Trim();
            ucContactAddressList.ContactType = contactType;

            bool isSearchForm = SectionPosition == ACAConstant.AuthAgentCustomerSectionPosition.SearchForm;

            //The field value formated by regional settings should not validate in pure search page.
            if (!IsAppliedRegional)
            {
                ControlUtil.ApplyRegionalSetting(IsPostBack, isSearchForm, true, !IsPostBack && !isSearchForm, ddlCountry);
                ControlUtil.ApplyRegionalSetting(IsPostBack, isSearchForm, true, !IsPostBack && !isSearchForm, ddlBirthCountry);

                if (!AppSession.IsAdmin && !IsPostBack)
                {
                    SetCurrentCityAndState();
                }
            }

            //The field value key mask format should not validate in pure search page.
            if (isSearchForm)
            {
                txtSSN.IsIgnoreValidate = true;
                txtFein.IsIgnoreValidate = true;
                txtEmail.IsIgnoreValidate = true;
            }

            refContactList.ContactSelected += RefContactList_Selected;

            // hide the Enable Soundex Search/Buttons Section in customer detail page
            if (!IsPostBack)
            {
                // bind contact list in admin side.
                if (AppSession.IsAdmin)
                {
                    if (SectionPosition != ACAConstant.AuthAgentCustomerSectionPosition.DetailForm)
                    {
                        BindRefContactsList();
                    }
                }
                else
                {
                    chkEnableSoundexSearch.Attributes.Add("onclick", "SwitchSoundexSearch(this.checked, '" + UpdatePanel4ContactType.ClientID + "');");
                    RelocateCustomerSearch();
                }

                if (SectionPosition == ACAConstant.AuthAgentCustomerSectionPosition.DetailForm)
                {
                    if (AppSession.IsAdmin)
                    {
                        ddlContactType.Attributes.Add("IsDBRequired", "true");
                    }

                    chkEnableSoundexSearch.Visible = false;
                    divButtons.Visible = false;

                    // show contact address
                    divContactAddress.Visible = _isContactAddressEnabled || AppSession.IsAdmin;

                    // Use the search condition to fill the new Customer form.
                    if (string.IsNullOrEmpty(hdnContactSeqNumber.Value) && SearchModelForCustomer != null)
                    {
                        FillStandardFieldsBySearchCondition(SearchModelForCustomer);
                    }
                }
                else
                {
                    chkEnableSoundexSearch.Checked = ValidationUtil.IsYes(Request.QueryString["soundex"]);
                }
            }

            if (IsPostBack)
            {
                // hide alter message
                MessageUtil.HideMessageByControl(Page);
            }

            if (!AppSession.IsAdmin)
            {
                ddlContactTypeFlag.Attributes.Add("onchange", "ddlContactTypeFlag_onchange();");
            }
        }

        /// <summary>
        /// Is attach expression to control or not.
        /// </summary>
        /// <returns>true:need attach;false:not attach</returns>
        protected override bool IsAttachExpressionToControl()
        {
            if (!AppSession.IsAdmin && SectionPosition == ACAConstant.AuthAgentCustomerSectionPosition.DetailForm)
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// override PreRender event.
        /// </summary>
        /// <param name="e">The event argument.</param>
        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);

            bool needResetControlStatus = IsEditable && !string.IsNullOrEmpty(hdnContactSeqNumber.Value);
            ControlBuildHelper.SetPropertyForStandardFields(ViewId, string.Empty, Permission, Controls, needResetControlStatus);

            /* if current contact type is Transactions. contact type dropdown list can't be disable.
             * only contact is refrence and contact type not Transactions contact dropdown list is disable.
             */
            if (!IsRefContactTypeEditable)
            {
                ddlContactType.DisableEdit();
            }

            phContent.TemplateControlIDPrefix = ACAConstant.REFERENCE_CONTACT_TEMPLATE_FIELD_PREFIX;
            InitFormDesignerPlaceHolder(phContent);
        }

        /// <summary>
        /// The search customer handler.
        /// </summary>
        /// <param name="sender">The object sender.</param>
        /// <param name="e">The EventArgs e.</param>
        protected void SearchButton_Click(object sender, EventArgs e)
        {
            if (IsSearchFieldsAllEmpty())
            {
                string message = GetTextByKey("aca_authagent_customersearch_msg_searchrequired");

                MessageUtil.ShowMessageByControl(Page, MessageType.Error, message);
                ScriptManager.RegisterClientScriptBlock(Page, GetType(), "GotoMessageBar", "scrollIntoView('messageSpan');", true);

                divAddCustomer.Visible = false;
                divResultNotice.Visible = false;
                refContactList.Visible = false;

                return;
            }

            Page.FocusElement(btnSearch.ClientID);
            imgErrorIcon.Visible = false;

            refContactList.RefContactsDataSource = null;
            refContactList.GViewContactList.PageIndex = 0;
            SearchRefContact(0, null);
            BindSearchResult();
        }

        /// <summary>
        /// The clear button handler.
        /// </summary>
        /// <param name="sender">The object sender.</param>
        /// <param name="e">The EventArgs e.</param>
        protected void ClearButton_Click(object sender, EventArgs e)
        {
            ControlUtil.ClearValue(this, null);

            if (SectionPosition == ACAConstant.AuthAgentCustomerSectionPosition.SearchForm)
            {
                ControlUtil.ApplyRegionalSetting(false, true, true, false, ddlCountry);
                ControlUtil.ApplyRegionalSetting(false, true, true, false, ddlBirthCountry);
            }
            else
            {
                ControlUtil.ClearRegionalSetting(ddlCountry, false, string.Empty, Permission, ViewId);
                ControlUtil.ClearRegionalSetting(ddlBirthCountry, false, string.Empty, Permission, ViewId);
            }

            divAddCustomer.Visible = false;
            divResultNotice.Visible = false;
            refContactList.Visible = false;

            Page.FocusElement(btnClear.ClientID);
        }

        /// <summary>
        /// Relocate the Customer search.
        /// </summary>
        protected void RelocateCustomerSearch()
        {
            string needRelocate = Request.QueryString["relocate"];

            if (ValidationUtil.IsYes(needRelocate) && SearchModelForCustomer != null)
            {
                CapContactModel capContact = SearchModelForCustomer;
                FillCustomerForm4StandardFields(capContact);
            }
        }

        /// <summary>
        /// The add new customer handler.
        /// </summary>
        /// <param name="sender">The object sender.</param>
        /// <param name="e">The EventArgs e.</param>
        protected void AddCustomerLink_Click(object sender, EventArgs e)
        {
            string url = string.Format(
                       "CustomerDetail.aspx?soundex={0}",
                       chkEnableSoundexSearch.Checked ? ACAConstant.COMMON_Y : ACAConstant.COMMON_N);

            Response.Redirect(url);
        }

        /// <summary>
        /// Handle the contact type dropdownlist selected changed event to load the template field
        /// </summary>
        /// <param name="sender">the sender object</param>
        /// <param name="e">the event arguments</param>
        protected void ContactType_SelectedIndexChanged(object sender, EventArgs e)
        {
            //when do post back, the view state have not update when the control value changed by js.
            if (Request.Form[rdolistGender.UniqueID] == null)
            {
                rdolistGender.SelectedIndex = -1;
            }

            string contactType = ddlContactType.SelectedValue.Trim();

            SetTemplateAndContactAddress(contactType);
            ClearExpressionValue(false);
            ContactSessionParameter contactSessionParameter = AppSession.GetContactSessionParameter();

            if (contactSessionParameter != null)
            {
                contactSessionParameter.ContactType = contactType;
                AppSession.SetContactSessionParameter(contactSessionParameter);
            }

            if (!string.IsNullOrEmpty(hdnContactSeqNumber.Value))
            {
                DisableCustomerFormBySetting();
            }

            CustomerEditPanel.Update();
            ucContactAddressList.HideValidateErrorMessage();
        }

        /// <summary>
        /// Handle the DataSourceChanged event, to control the contact edit form.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="arg">Event argument.</param>
        protected void ContactAddressList_DataSourceChanged(object sender, CommonEventArgs arg)
        {
            ContactAddressModel[] addressList = ucContactAddressList.DataSource == null
                                                ? null
                                                : ucContactAddressList.DataSource.ToArray();

            ContactUtil.SetContactAddressListToContactSessionParameter(addressList);
        }

        /// <summary>
        /// Insert all license's field control into someone collection.
        /// </summary>
        /// <param name="viewID">view id for expression.</param>
        /// <param name="moduleName">module name.</param>
        /// <param name="templateEdit">template control</param>
        /// <param name="templatePrefix">template prefix.</param>
        /// <returns>Expression Controls</returns>
        protected override Dictionary<string, WebControl> CollectExpressionInputControls(string viewID, string moduleName, TemplateEdit templateEdit, string templatePrefix)
        {
            Dictionary<string, WebControl> expressionControls = base.CollectExpressionInputControls(viewID, moduleName, templateEdit, templatePrefix);

            //Generic templateEdit part
            CapModel4WS capModel = AppSession.GetCapModelFromSession(moduleName);
            var templateModel = genericTemplate.GetTemplateModel(false);

            if (templateModel == null || templateModel.templateForms == null)
            {
                return expressionControls;
            }

            var templateFields = (from groups in templateModel.templateForms
                                  where groups != null && groups.subgroups != null
                                  from subGroup in groups.subgroups
                                  where subGroup != null && subGroup.fields != null
                                  from field in subGroup.fields
                                  select field).ToList();

            if (templateFields.Count == 0)
            {
                return expressionControls;
            }

            foreach (var field in templateFields)
            {
                var control = genericTemplate.FindControl(ControlBuildHelper.GetGenericTemplateControlID(field)) as WebControl;

                if (control == null)
                {
                    continue;
                }

                string controlFieldFullName = string.Format(
                                                            "{0}{1}{2}{3}{4}",
                                                            ExpressionType.AuthAgent_Customer_Detail.ToString(),
                                                            ACAConstant.SPLIT_CHAR5,
                                                            ExpressionUtil.FilterSpciefCharForControlName(Server.UrlEncode(field.subgroupName.ToUpper())),
                                                            ACAConstant.SPLIT_CHAR5,
                                                            ExpressionUtil.FilterSpciefCharForControlName(Server.UrlEncode(field.fieldName)));

                var expCtlKey4Template = ExpressionUtil.GetFullControlFieldName(capModel, controlFieldFullName);

                if (!expressionControls.ContainsKey(expCtlKey4Template))
                {
                    expressionControls.Add(expCtlKey4Template, control);
                }
            }

            return expressionControls;
        }

        #endregion Protected Events

        #region Private Events

        /// <summary>
        /// Handle ContactSelected event;populate contact information into contact form
        /// </summary>
        /// <param name="sender">An object that contains the event sender.</param>
        /// <param name="arg">A CommonEventArgs object containing the contact model.</param>
        private void RefContactList_Selected(object sender, CommonEventArgs arg)
        {
            //select a contact to edit
            Page.FocusElement(btnSearch.ClientID);
            imgErrorIcon.Visible = false;

            if (arg == null || arg.ArgObject == null)
            {
                return;
            }

            PeopleModel people = (PeopleModel)arg.ArgObject;

            if (people != null)
            {
                AppSession.SetPeopleModelToSession(people.contactSeqNumber, people);

                string url = string.Format(
                    "CustomerDetail.aspx?id={0}&{1}={2}&soundex={3}", 
                    people.contactSeqNumber, 
                    UrlConstant.CAPTYPE, 
                    ddlRecordType.SelectedValue, 
                    chkEnableSoundexSearch.Checked ? ACAConstant.COMMON_Y : ACAConstant.COMMON_N);
                Response.Redirect(url);
            }
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Set templates and contact address.
        /// </summary>
        /// <param name="contactType">The contact type</param>
        private void SetTemplateAndContactAddress(string contactType)
        {
            ResetTemplate();
            CreatePeopleTemplate(contactType);
            CreateGenericTemplate(contactType);

            ucContactAddressList.ContactType = contactType;
        }

        /// <summary>
        /// Search reference contact data.
        /// </summary>
        /// <param name="currentPageIndex">current page index</param>
        /// <param name="sortExpression">sort expression</param>
        private void SearchRefContact(int currentPageIndex, string sortExpression)
        {
            IPeopleBll peopleBll = ObjectFactory.GetObject<IPeopleBll>();
            SearchModelForCustomer = GetCapContactModel();

            PaginationModel pageInfo = PaginationUtil.GetPageInfoByID(refContactList.ClientID);
            pageInfo.SortExpression = sortExpression;
            pageInfo.CurrentPageIndex = currentPageIndex;
            pageInfo.CustomPageSize = refContactList.GViewContactList.PageSize;
            QueryFormat queryFormat = PaginationUtil.GetQueryFormatModel(pageInfo);
            string capTypeAlias = ddlRecordType.SelectedItem == null ? string.Empty : ddlRecordType.SelectedItem.Text;
            CapTypeModel capTypeModel = CapUtil.GetCAPTypeModelByString(AuthSettingModel.ModuleName, ddlRecordType.SelectedValue, capTypeAlias);

            PeopleModel[] searchResult = peopleBll.SearchCustomerList(SearchModelForCustomer, chkEnableSoundexSearch.Checked, AuthSettingModel.CapTypeFilterName, AuthSettingModel.ModuleName, capTypeModel, queryFormat);
            IList<PeopleModel> peopleList = ObjectConvertUtil.ConvertArrayToList(searchResult);

            peopleList = PaginationUtil.MergeDataSource(refContactList.RefContactsDataSource, peopleList, pageInfo);
            refContactList.RefContactsDataSource = peopleList;
        }

        /// <summary>
        /// Bind reference contact data.
        /// </summary>
        private void BindSearchResult()
        {
            if (refContactList.RefContactsDataSource.Count > 1)
            {
                BindRefContactsList();
            }
            else if (refContactList.RefContactsDataSource.Count == 1)
            {
                // if only one result is returned just redirect to customer detail page.
                PeopleModel people = refContactList.RefContactsDataSource[0];

                if (people != null)
                {
                    AppSession.SetPeopleModelToSession(people.contactSeqNumber, people);

                    Response.Redirect(string.Format("CustomerDetail.aspx?id={0}&{1}={2}&soundex={3}", people.contactSeqNumber, UrlConstant.CAPTYPE, ddlRecordType.SelectedValue, chkEnableSoundexSearch.Checked ? ACAConstant.COMMON_Y : ACAConstant.COMMON_N));
                }
            }
            else
            {
                refContactList.Visible = true;
                divAddCustomer.Visible = true;
                divResultNotice.Visible = false;
                refContactList.BindRefContactsList();
            }
        }

        /// <summary>
        /// Gets the CapContactModel
        /// </summary>
        /// <returns>Get the CapContactModel</returns>
        private CapContactModel GetCapContactModel()
        {
            // cap ID model
            CapIDModel capIDModel = new CapIDModel();
            capIDModel.serviceProviderCode = ConfigManager.AgencyCode;
            capIDModel.customID = ControlUtil.GetControlValue(txtLicenseID);

            // combine the search model.
            CapContactModel result = new CapContactModel();
            result.capID = capIDModel;
            result.people = GetPeopleModel(false);

            return result;
        }

        /// <summary>
        /// bind data list.
        /// </summary>
        private void BindRefContactsList()
        {
            refContactList.BindRefContactsList();
            refContactList.Visible = true;
            divAddCustomer.Visible = true;
            lblResultNotice.Visible = true;
            divResultNotice.Visible = true;

            if (!AppSession.IsAdmin)
            {
                lblResultNotice.Text = string.Format(GetTextByKey("aca_authagent_customersearch_msg_searchresultnotice"), refContactList.GViewContactList.CountSummary);
            }
        }

        /// <summary>
        /// Create the people templateEdit.
        /// </summary>
        /// <param name="contactType">contact type.</param>
        private void CreatePeopleTemplate(string contactType)
        {
            if (string.IsNullOrEmpty(contactType))
            {
                peopleTemplate.DisplayAttributes(null, ACAConstant.REFERENCE_CONTACT_TEMPLATE_FIELD_PREFIX);
            }
            else
            {
                ITemplateBll templateBll = ObjectFactory.GetObject<ITemplateBll>();
                TemplateAttributeModel[] attributes = templateBll.GetPeopleTemplateAttributes(contactType, ConfigManager.AgencyCode, AppSession.User.PublicUserId);

                ResetTemplateFieldDisplay(attributes);
                peopleTemplate.DisplayAttributes(attributes, ACAConstant.REFERENCE_CONTACT_TEMPLATE_FIELD_PREFIX);
            }
        }

        /// <summary>
        /// Create generic templateEdit
        /// </summary>
        /// <param name="contactType">Contact type.</param>
        private void CreateGenericTemplate(string contactType)
        {
            if (string.IsNullOrEmpty(contactType))
            {
                genericTemplate.ResetControl();
            }
            else
            {
                ITemplateBll templateBll = ObjectFactory.GetObject<ITemplateBll>();
                TemplateModel model = templateBll.GetContactTemplates(ConfigManager.AgencyCode, contactType.Replace(ACAConstant.SPLIT_CHAR4 + ACAConstant.SPLIT_DOUBLE_VERTICAL, string.Empty), false, AppSession.User.UserSeqNum);
                genericTemplate.Display(model);
            }
        }

        /// <summary>
        /// Reset template field display property. // james, is it necessary now?
        /// Currently, only License form support the Always Editable functionality, so change 'E' to 'Y', means displayable.
        /// </summary>
        /// <param name="attributes">Template Attribute Model</param>
        private void ResetTemplateFieldDisplay(TemplateAttributeModel[] attributes)
        {
            if (attributes == null)
            {
                return;
            }

            foreach (TemplateAttributeModel field in attributes)
            {
                if (ACAConstant.TEMPLATE_FIIELD_STATUS_ALWAYSEDITABLE.Equals(field.vchFlag, StringComparison.OrdinalIgnoreCase))
                {
                    //"Y" means template field ACA Display = 'Display'
                    field.vchFlag = ACAConstant.COMMON_Y;
                }
            }
        }

        /// <summary>
        /// reset the template field.
        /// </summary>
        private void ResetTemplate()
        {
            peopleTemplate.ResetControl();
            genericTemplate.ResetControl();
        }

        /// <summary>
        /// Merge the field values from a template model to the templateEdit fields associated with the specific contact type.
        /// </summary>
        /// <param name="sourceTemplate">Source templateEdit model</param>
        /// <param name="contactType">Contact Type</param>
        /// <returns>return the template model associated with the specific contact type.</returns>
        private TemplateModel MergeGenericTemplate(TemplateModel sourceTemplate, string contactType)
        {
            ITemplateBll templateBll = ObjectFactory.GetObject<ITemplateBll>();
            TemplateModel targetTemplate = templateBll.GetContactTemplates(ConfigManager.AgencyCode, contactType, false, AppSession.User.PublicUserId);

            if (targetTemplate == null)
            {
                return null;
            }

            GenericTemplateUtil.MergeGenericTemplate(sourceTemplate, targetTemplate, string.Empty);

            return targetTemplate;
        }

        /// <summary>
        /// Check the search fields is all empty
        /// </summary>
        /// <returns>Return true if search fields all empty.</returns>
        private bool IsSearchFieldsAllEmpty()
        {
            return
                string.IsNullOrEmpty(ControlUtil.GetControlValue(txtLicenseID)) &&
                string.IsNullOrEmpty(ControlUtil.GetControlValue(txtCustomerID)) &&
                string.IsNullOrEmpty(ControlUtil.GetControlValue(txtFirstName)) &&
                string.IsNullOrEmpty(ControlUtil.GetControlValue(txtLastName)) &&
                string.IsNullOrEmpty(ControlUtil.GetControlValue(txtMiddleName)) &&
                string.IsNullOrEmpty(ControlUtil.GetControlValue(txtFullName)) &&
                string.IsNullOrEmpty(ControlUtil.GetControlValue(txtPhone1)) &&
                string.IsNullOrEmpty(txtPhone1.CountryCodeText) &&
                string.IsNullOrEmpty(ControlUtil.GetControlValue(txtPhone2)) &&
                string.IsNullOrEmpty(txtPhone2.CountryCodeText) &&
                string.IsNullOrEmpty(ControlUtil.GetControlValue(txtPhone3)) &&
                string.IsNullOrEmpty(txtPhone3.CountryCodeText) &&
                string.IsNullOrEmpty(ControlUtil.GetControlValue(txtFax)) &&
                string.IsNullOrEmpty(txtFax.CountryCodeText) &&
                string.IsNullOrEmpty(ControlUtil.GetControlValue(ddlCountry)) &&
                string.IsNullOrEmpty(ControlUtil.GetControlValue(txtPostOfficeBox)) &&
                string.IsNullOrEmpty(ControlUtil.GetControlValue(txtEmail)) &&
                string.IsNullOrEmpty(ControlUtil.GetControlValue(ddlSalutation)) &&
                string.IsNullOrEmpty(ControlUtil.GetControlValue(rdolistGender)) &&
                string.IsNullOrEmpty(ControlUtil.GetControlValue(ddlContactType)) &&
                string.IsNullOrEmpty(ControlUtil.GetControlValue(ddlContactTypeFlag)) &&
                string.IsNullOrEmpty(ControlUtil.GetControlValue(txtTradeName)) &&
                string.IsNullOrEmpty(ControlUtil.GetControlValue(txtTitle)) &&
                string.IsNullOrEmpty(ControlUtil.GetControlValue(txtFein)) &&
                string.IsNullOrEmpty(ControlUtil.GetControlValue(txtSSN)) &&
                string.IsNullOrEmpty(ControlUtil.GetControlValue(txtBusinessName)) &&
                string.IsNullOrEmpty(ControlUtil.GetControlValue(txtBusinessName2)) &&
                string.IsNullOrEmpty(ControlUtil.GetControlValue(txtBirthCity)) &&
                string.IsNullOrEmpty(ControlUtil.GetControlValue(txtBirthState)) &&
                string.IsNullOrEmpty(ControlUtil.GetControlValue(ddlBirthCountry)) &&
                string.IsNullOrEmpty(ControlUtil.GetControlValue(txtSuffix)) &&
                string.IsNullOrEmpty(ControlUtil.GetControlValue(ddlRace)) &&
                string.IsNullOrEmpty(ControlUtil.GetControlValue(txtPassportNumber)) &&
                string.IsNullOrEmpty(ControlUtil.GetControlValue(txtDriverLicenseNbr)) &&
                string.IsNullOrEmpty(ControlUtil.GetControlValue(txtDriverLicenseState)) &&
                string.IsNullOrEmpty(ControlUtil.GetControlValue(txtStateIdNbr)) &&
                string.IsNullOrEmpty(ControlUtil.GetControlValue(txtNotes)) &&
                string.IsNullOrEmpty(ControlUtil.GetControlValue(txtBirthDate)) &&
                string.IsNullOrEmpty(ControlUtil.GetControlValue(txtDeceasedDate)) &&
                string.IsNullOrEmpty(ControlUtil.GetControlValue(txtAddressLine1)) &&
                string.IsNullOrEmpty(ControlUtil.GetControlValue(txtAddressLine2)) &&
                string.IsNullOrEmpty(ControlUtil.GetControlValue(txtAddressLine3)) &&
                string.IsNullOrEmpty(ControlUtil.GetControlValue(txtCity)) &&
                string.IsNullOrEmpty(ControlUtil.GetControlValue(txtState)) &&
                string.IsNullOrEmpty(ControlUtil.GetControlValue(ddlRecordType));
        }

        /// <summary>
        /// Get the visible identity controls.
        /// If have configured the contact identity controls, find the identity controls which is visible in search form.
        /// </summary>
        /// <returns>Return the visible identity controls.</returns>
        private List<IAccelaControl> GetVisibleIdentityControls()
        {
            // get customer identity fields
            IPeopleBll peopleBll = ObjectFactory.GetObject<IPeopleBll>();
            GenericIdentityFieldModel[] contactIdentities = peopleBll.GetContactIdentityFields(ConfigManager.AgencyCode);

            if (contactIdentities == null || contactIdentities.Length == 0)
            {
                return null;
            }

            List<IAccelaControl> result = new List<IAccelaControl>();

            // mapping the contact identities' view id to AccelaControl
            Dictionary<long, IAccelaControl> mapping = new Dictionary<long, IAccelaControl>();
            mapping.Add(28, txtSSN);
            mapping.Add(29, txtFein);
            mapping.Add(39, txtPassportNumber);
            mapping.Add(40, txtDriverLicenseNbr);
            mapping.Add(41, txtDriverLicenseState);
            mapping.Add(42, txtStateIdNbr);

            foreach (var identity in contactIdentities)
            {
                foreach (var viewElement in identity.viewElements)
                {
                    if (mapping.ContainsKey(viewElement.viewElementID))
                    {
                        IAccelaControl accelaControl = mapping[viewElement.viewElementID];

                        if (((WebControl)accelaControl).Visible)
                        {
                            result.Add(accelaControl);
                        }
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// Get the contact identity controls' value
        /// </summary>
        /// <param name="people">The people model</param>
        /// /// <param name="ignoreVisible">if ignoreVisible is true then we get the visible value</param>
        private void AppendIdentityFieldsValue(PeopleModel people, bool ignoreVisible = true)
        {
            if (people == null)
            {
                return;
            }

            people.fein = MaskUtil.UpdateFEIN(hfFEIN.Value, ControlUtil.GetControlValue(txtFein, ignoreVisible));
            hfFEIN.Value = people.fein;
            txtFein.Text = MaskUtil.FormatFEINShow(ControlUtil.GetControlValue(txtFein, ignoreVisible), StandardChoiceUtil.IsEnableFeinMasking());

            people.socialSecurityNumber = MaskUtil.UpdateSSN(hfSSN.Value, ControlUtil.GetControlValue(txtSSN, ignoreVisible));
            hfSSN.Value = people.socialSecurityNumber;
            people.maskedSsn = people.socialSecurityNumber;
            txtSSN.Text = MaskUtil.FormatSSNShow(ControlUtil.GetControlValue(txtSSN, ignoreVisible));

            people.passportNumber = ControlUtil.GetControlValue(txtPassportNumber, ignoreVisible);
            people.driverLicenseNbr = ControlUtil.GetControlValue(txtDriverLicenseNbr, ignoreVisible);
            people.driverLicenseState = ControlUtil.GetControlValue(txtDriverLicenseState, ignoreVisible);
            people.stateIDNbr = ControlUtil.GetControlValue(txtStateIdNbr, ignoreVisible);
        }

        /// <summary>
        /// Disable customer form by setting.
        /// </summary>
        private void DisableCustomerFormBySetting()
        {
            IsEditable = StandardChoiceUtil.IsCustomerDetailEditable();

            if (!IsEditable)
            {
                DisableEdit(this, null);
                genericTemplate.IsReadOnly = true;
                ucContactAddressList.DisableEditForm();
            }
            else if (!_isContactAddressEnabled)
            {
                ucContactAddressList.DisableEditForm();
            }
        }

        /// <summary>
        /// Fills the standard fields by search condition. Fills the standard fields by search condition. in customer detail, when value is empty not need set value.
        /// </summary>
        /// <param name="capContact">The cap contact.</param>
        private void FillStandardFieldsBySearchCondition(CapContactModel capContact)
        {
            if (capContact == null || capContact.people == null)
            {
                return;
            }

            bool isSearchForm = SectionPosition == ACAConstant.AuthAgentCustomerSectionPosition.SearchForm;
            PeopleModel people = capContact.people;

            if (!string.IsNullOrEmpty(people.contactType))
            {
                DropDownListBindUtil.SetSelectedValue(ddlContactType, people.contactType);
            }

            if (!string.IsNullOrEmpty(people.salutation))
            {
                DropDownListBindUtil.SetSelectedValue(ddlSalutation, people.salutation);
            }

            if (!string.IsNullOrEmpty(people.contactTypeFlag))
            {
                DropDownListBindUtil.SetSelectedValue(ddlContactTypeFlag, people.contactTypeFlag);
            }

            if (people.preferredChannel != null)
            {
                DropDownListBindUtil.SetSelectedValue(ddlPreferredChannel, people.preferredChannel.ToString());
            }

            if (!string.IsNullOrEmpty(people.firstName))
            {
                txtFirstName.Text = people.firstName;
            }

            if (!string.IsNullOrEmpty(people.lastName))
            {
                txtLastName.Text = people.lastName;
            }

            if (!string.IsNullOrEmpty(people.middleName))
            {
                txtMiddleName.Text = people.middleName;
            }

            if (!string.IsNullOrEmpty(people.fullName))
            {
                txtFullName.Text = people.fullName;
            }

            if (!string.IsNullOrEmpty(people.postOfficeBox))
            {
                txtPostOfficeBox.Text = people.postOfficeBox;
            }

            if (!string.IsNullOrEmpty(people.email))
            {
                txtEmail.Text = people.email;
            }

            if (!string.IsNullOrEmpty(people.gender))
            {
                DropDownListBindUtil.SetSelectedValueForRadioList(rdolistGender, people.gender);
            }

            if (!string.IsNullOrEmpty(people.tradeName))
            {
                txtTradeName.Text = people.tradeName;
            }

            if (!string.IsNullOrEmpty(people.title))
            {
                txtTitle.Text = people.title;
            }

            if (!string.IsNullOrEmpty(people.fein))
            {
                txtFein.Text = MaskUtil.FormatFEINShow(people.fein, StandardChoiceUtil.IsEnableFeinMasking());
                hfFEIN.Value = people.fein;
            }

            if (!string.IsNullOrEmpty(people.socialSecurityNumber))
            {
                txtSSN.Text = MaskUtil.FormatSSNShow(people.socialSecurityNumber);
                hfSSN.Value = people.socialSecurityNumber;
            }

            if (!string.IsNullOrEmpty(people.businessName))
            {
                txtBusinessName.Text = people.businessName;
            }

            if (!string.IsNullOrEmpty(people.businessName2))
            {
                txtBusinessName2.Text = people.businessName2;
            }

            if (!string.IsNullOrEmpty(people.birthRegion))
            {
                DropDownListBindUtil.SetCountrySelectedValue(ddlBirthCountry, people.birthRegion, false, true, isSearchForm);
            }

            if (!string.IsNullOrEmpty(people.birthCity))
            {
                txtBirthCity.Text = people.birthCity;
            }

            if (!string.IsNullOrEmpty(people.birthState))
            {
                txtBirthState.Text = people.birthState;
            }

            if (!string.IsNullOrEmpty(people.namesuffix))
            {
                txtSuffix.Text = people.namesuffix;
            }

            if (!string.IsNullOrEmpty(people.race))
            {
                DropDownListBindUtil.SetSelectedValue(ddlRace, people.race);
            }

            if (!string.IsNullOrEmpty(people.passportNumber))
            {
                txtPassportNumber.Text = people.passportNumber;
            }

            if (!string.IsNullOrEmpty(people.driverLicenseNbr))
            {
                txtDriverLicenseNbr.Text = people.driverLicenseNbr;
            }

            if (!string.IsNullOrEmpty(people.stateIDNbr))
            {
                txtStateIdNbr.Text = people.stateIDNbr;
            }

            if (!string.IsNullOrEmpty(people.comment))
            {
                txtNotes.Text = people.comment;
            }

            if (people.birthDate != null)
            {
                txtBirthDate.Text2 = people.birthDate;
            }

            if (people.deceasedDate != null)
            {
                txtDeceasedDate.Text2 = people.deceasedDate;
            }

            CompactAddressModel addressModel = people.compactAddress;

            if (addressModel != null)
            {
                IBizdomainProvider bizProvider = ObjectFactory.GetObject<IBizdomainProvider>();
                IList<ItemValue> defaultCountry = bizProvider.GetBizDomainList(BizDomainConstant.STD_COUNTRY_DEFAULT_VALUE);
                IList<ItemValue> countryIdds = bizProvider.GetBizDomainList(BizDomainConstant.STD_CAT_COUNTRY_IDD);
                string defaultCountryCode = defaultCountry.Count == 0 ? string.Empty : defaultCountry[0].Key;
                bool notEmptyAndDefaultCountry = !string.IsNullOrEmpty(addressModel.countryCode)
                                                 && !addressModel.countryCode.Equals(defaultCountryCode, StringComparison.InvariantCultureIgnoreCase);
                string phoneCountryIdd = string.Empty;

                if (notEmptyAndDefaultCountry)
                {
                    ItemValue countryIdd = countryIdds.FirstOrDefault(o => o.Key.Equals(addressModel.countryCode, StringComparison.InvariantCultureIgnoreCase));
                    phoneCountryIdd = countryIdd == null ? string.Empty : countryIdd.Value.ToString();
                    DropDownListBindUtil.SetCountrySelectedValue(ddlCountry, addressModel.countryCode, false, true, isSearchForm);
                }

                if (!string.IsNullOrEmpty(people.phone1CountryCode))
                {
                    txtPhone1.CountryCodeText = people.phone1CountryCode;
                }
                else if (notEmptyAndDefaultCountry)
                {
                    txtPhone1.CountryCodeText = phoneCountryIdd;
                }

                if (!string.IsNullOrEmpty(people.phone1))
                {
                    txtPhone1.Text = ModelUIFormat.FormatPhone4EditPage(people.phone1, addressModel.countryCode);
                }

                if (!string.IsNullOrEmpty(people.phone2CountryCode))
                {
                    txtPhone2.CountryCodeText = people.phone2CountryCode;
                }
                else if (notEmptyAndDefaultCountry)
                {
                    txtPhone2.CountryCodeText = phoneCountryIdd;
                }

                if (!string.IsNullOrEmpty(people.phone2))
                {
                    txtPhone2.Text = ModelUIFormat.FormatPhone4EditPage(people.phone2, addressModel.countryCode);
                }

                if (!string.IsNullOrEmpty(people.phone3CountryCode))
                {
                    txtPhone3.CountryCodeText = people.phone3CountryCode;
                }
                else if (notEmptyAndDefaultCountry)
                {
                    txtPhone3.CountryCodeText = phoneCountryIdd;
                }

                if (!string.IsNullOrEmpty(people.phone3))
                {
                    txtPhone3.Text = ModelUIFormat.FormatPhone4EditPage(people.phone3, addressModel.countryCode);
                }

                if (!string.IsNullOrEmpty(people.faxCountryCode))
                {
                    txtFax.CountryCodeText = people.faxCountryCode;
                }
                else if (notEmptyAndDefaultCountry)
                {
                    txtFax.CountryCodeText = phoneCountryIdd;
                }

                if (!string.IsNullOrEmpty(people.fax))
                {
                    txtFax.Text = ModelUIFormat.FormatPhone4EditPage(people.fax, addressModel.countryCode);
                }

                if (!string.IsNullOrEmpty(addressModel.zip))
                {
                    txtZip.Text = ModelUIFormat.FormatZipShow(addressModel.zip, addressModel.countryCode, false);
                }

                if (!string.IsNullOrEmpty(addressModel.addressLine1))
                {
                    txtAddressLine1.Text = addressModel.addressLine1;
                }

                if (!string.IsNullOrEmpty(addressModel.addressLine2))
                {
                    txtAddressLine2.Text = addressModel.addressLine2;
                }

                if (!string.IsNullOrEmpty(addressModel.addressLine3))
                {
                    txtAddressLine3.Text = addressModel.addressLine3;
                }

                if (!string.IsNullOrEmpty(addressModel.city))
                {
                    txtCity.Text = addressModel.city;
                }

                if (!string.IsNullOrEmpty(addressModel.state))
                {
                    txtState.Text = addressModel.state;
                }
            }

            if (!string.IsNullOrEmpty(people.driverLicenseState))
            {
                txtDriverLicenseState.Text = people.driverLicenseState;
            }
        }

        #endregion Private Methods
    }
}