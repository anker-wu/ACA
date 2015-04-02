#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: ContactAddressEdit.aspx.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2011-2014
 *
 *  Description:
 *
 *  Notes:
 *      $Id: ContactAddressEdit.ascx.cs 277634 2014-08-19 08:16:23Z ACHIEVO\james.shi $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;

using Accela.ACA.BLL.APO;
using Accela.ACA.BLL.Common;
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
    /// Edit form of the contact address.
    /// </summary>
    public partial class ContactAddressEdit : FormDesignerWithExpressionControl
    {
        #region Fields

        /// <summary>
        /// A value to distinguish the Save Validated Contact Address action.
        /// </summary>
        public const string SAVE_VALIDATED_CONTACT_ADDRESS = "$SaveValidatedContactAddress$";

        /// <summary>
        /// A value to indicating have not any records been selected.
        /// </summary>
        private const int NO_SELECTED = -1;

        /// <summary>
        /// Indicating whether the contact address form is editable.
        /// </summary>
        private bool _isEditable;

        /// <summary>
        /// Indicating whether the contact address form is need to validate reference data.
        /// </summary>
        private bool _isValidate;

        /// <summary>
        /// Indicating whether the contact address form is need to validate reference data.
        /// </summary>
        private ACAConstant.ContactSectionPosition _contactSectionPosition;

        /// <summary>
        /// ExpressionFactory class's instance
        /// </summary>
        private ExpressionFactory _expressionInstance;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the ContactAddressEdit class.
        /// </summary>
        public ContactAddressEdit()
            : base(GviewID.ContactAddress)
        {
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the row index of this contact address.
        /// </summary>
        public int RowIndex { get; set; }

        /// <summary>
        /// Gets or sets a value indicating current contact type.
        /// </summary>
        public ExpressionType ContactExpressionType { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the contact address form is editable.
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
        /// Gets or sets a value indicating whether the contact address form is need to validate reference data.
        /// </summary>
        public bool IsValidate
        {
            get
            {
                return _isValidate;
            }

            set
            {
                _isValidate = value;
            }
        }

        /// <summary>
        /// Gets or sets the contact section position.
        /// </summary>
        public ACAConstant.ContactSectionPosition ContactSectionPosition
        {
            get
            {
                return _contactSectionPosition;
            }

            set
            {
                _contactSectionPosition = value;

                ViewId = PeopleUtil.GetContactAddressEditGviewID(value);
            }
        }

        /// <summary>
        /// Gets UpdatePanel control of edit form.
        /// </summary>
        public UpdatePanel UpdatePanel
        {
            get
            {
                return contactAddressEditPanel;
            }
        }

        /// <summary>
        /// Gets or sets the contact address list client empty validation script function's name.
        /// </summary>
        public string ListEmptyValidationScriptFuncionName
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets a value indicating whether the contact address is enabled.
        /// </summary>
        public bool IsEnabled
        {
            get; 
            set;
        }

        /// <summary>
        /// Gets a value indicating whether the country is hidden.
        /// </summary>
        public bool IsCountryHidden
        {
            get { return bool.Parse(Request.QueryString[UrlConstant.IS_COUNTRY_HIDDEN]); }
        }

        /// <summary>
        /// Gets or sets the permission for form designer.
        /// </summary>
        protected override GFilterScreenPermissionModel4WS Permission
        {
            get
            {
                base.Permission = new GFilterScreenPermissionModel4WS();
                base.Permission.permissionLevel = GViewConstant.SECTION_CONTACT_ADDRESS;

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
                    ExpressionControls = CollectExpressionInputControls(GviewID.ContactAddress, ModuleName, null, null);
                    _expressionInstance = new ExpressionFactory(ModuleName, ContactAddressExpressionType, ExpressionControls);
                }

                return _expressionInstance;
            }
        }

        /// <summary>
        /// Gets all phone controls id for Country field.
        /// </summary>
        private string PhoneControlIDs
        {
            get
            {
                StringBuilder sbControls = new StringBuilder();
                sbControls.Append(txtPhone.ClientID);
                sbControls.Append(",").Append(txtFax.ClientID);
                return sbControls.ToString();
            }
        }

        /// <summary>
        /// Gets current contact expression type.
        /// </summary>
        private ExpressionType ContactAddressExpressionType
        {
            get
            {
                ExpressionType expressionType = ExpressionType.RefContact_Address;

                switch (ContactExpressionType)
                {
                    case ExpressionType.Applicant:
                    case ExpressionType.Contact_1:
                    case ExpressionType.Contact_2:
                    case ExpressionType.Contact_3:
                    case ExpressionType.Contacts:
                        expressionType = ExpressionType.Contact_Address;
                        break;
                    case ExpressionType.ReferenceContact:
                        expressionType = ExpressionType.RefContact_Address;
                        break;
                    case ExpressionType.AuthAgent_Customer_Detail:
                        expressionType = ExpressionType.AuthAgent_Address;
                        break;
                }

                return expressionType;
            }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Bind the address type fields based on the contact type.
        /// </summary>
        /// <param name="contactType">Contact type.</param>
        public void BindAddressType(string contactType)
        {
            DropDownListBindUtil.BindContactAddressType(ddlAddressType, contactType);
        }

        /// <summary>
        /// Disable contact address edit form.
        /// </summary>
        public void DisableEditForm()
        {
            if (!AppSession.IsAdmin)
            {
                //string[] filterControlIds = new string[] { contactAddressList.ID };
                string[] filterControlIds = new string[] { };
                DisableEdit(this, filterControlIds);
            }
        }

        /// <summary>
        /// Enable contact address edit form.
        /// </summary>
        public void EnableEditForm()
        {
            if (IsEditable)
            {
                EnableEdit(this, null);
            }
        }

        /// <summary>
        /// Clear contact address form and set all fields to the initial status, such as hidden fields, regional settings.
        /// </summary>
        /// <param name="clearCountry">
        /// Indicate whether need to clear the country fields.
        /// true - clear the country/state fields and clear the mask format for Phone/Zip fields.
        /// false - apply default regional settings, set default value for country/state fields, set default format for phone/zip fields.
        /// </param>
        public void ResetContactAddressForm(bool clearCountry)
        {
            hdfDeactivateFlag.Value = ACAConstant.COMMON_N;

            ClearEditForm(clearCountry);

            //click clear button in contact section, no need set city & state to default. only click clear button the 'clearcountry = false'.
            if (!clearCountry)
            {
                SetCurrentCityAndState();
            }

            divDeactiveContactAddress.Visible = false;
        }

        /// <summary>
        /// Save contact address information into contact address list.
        /// </summary>
        /// <param name="contactAddress">Contact Address Model</param>
        /// <returns>New Contact Address Model</returns>
        public ContactAddressModel CollectContactAddressInfo(ContactAddressModel contactAddress)
        {
            ContactAddressModel newContactAddress = null;
            bool isDeactivateLogic = ValidationUtil.IsYes(hdfDeactivateFlag.Value);

            if (isDeactivateLogic)
            {
                newContactAddress = GetContactAddressInfo(null);
            }
            else
            {
                newContactAddress = GetContactAddressInfo(ObjectCloneUtil.DeepCopy(contactAddress));
            }

            return newContactAddress;
        }

        /// <summary>
        /// Clear the contact address edit form and hidden fields.
        /// </summary>
        /// <param name="clearRegional">if set to <c>true</c> [clear regional], else apply default regional settings.</param>
        public void ClearEditForm(bool clearRegional)
        {
            string[] filterControlIDs = null;

            EnableEditForm();

            if (ValidationUtil.IsYes(hdfDeactivateFlag.Value))
            {
                /* Diactivate contact address should disable contact address type, 
                 * Click clear button should keep the disable status & contact address type value.
                 * when call EnableEditForm will enable contact address type, so should disable contact type again.
                 */
                filterControlIDs = new string[] { ddlAddressType.ID, txtDeactivatedContactAddressEndDate.ID };
                ddlAddressType.DisableEdit();
            }

            ControlUtil.ClearValue(this, filterControlIDs);

            if (clearRegional)
            {
                /*
                 * Clear the country/state fields and clear the mask format for Phone/Zip fields.
                 * If the country field is invisible, apply the default regional settings.
                 */
                ControlUtil.ClearRegionalSetting(ddlCountry, false, this.ModuleName, this.Permission, this.ViewId);
            }
            else
            {
                /*
                 * Attach SelectedIndexChange event to country field to create association between country field and State/Phone/Zip fields.
                 * And apply default regional settings.
                 */
                ControlUtil.ApplyRegionalSetting(false, false, true, true, ddlCountry);
                SetDefaultAdress();
            }

            hdfDisableContactForm.Value = string.Empty;
            hdfDisableFormByDeactivate.Value = string.Empty;
        }

        /// <summary>
        /// Need validate contact address
        /// </summary>
        /// <param name="contactAddress">the contact address model</param>
        /// <returns>true or false.</returns>
        public bool NeedValidateContactAddress(ContactAddressModel contactAddress)
        {
            bool needValidate = false;

            if (!string.IsNullOrEmpty(ddlAddressType.SelectedValue) && StandardChoiceUtil.IsEnableContactAddressValidation())
            {
                IRefAddressBll refAddressBll = (IRefAddressBll)ObjectFactory.GetObject(typeof(IRefAddressBll));
                XContactAddressTypeSettingModel contactAddressTypeSetting = refAddressBll.GetContactAddressValidationSetting(CapUtil.GetAgencyCode(ModuleName), ddlAddressType.SelectedValue);
                ContactAddressValidationSettingModel validationSetting = contactAddressTypeSetting != null ? contactAddressTypeSetting.contactAddressValidationSettingModel : null;

                if (validationSetting != null
                    && !string.IsNullOrEmpty(validationSetting.sourceName)
                    && validationSetting.required
                    && (validationSetting.CountryList == null || validationSetting.CountryList.Length == 0 || validationSetting.CountryList.Contains(ddlCountry.SelectedValue)))
                {
                    needValidate = true;
                }
            }

            return needValidate;
        }

        /// <summary>
        /// Save contact address information
        /// </summary>
        /// <param name="newContactAddress">the contact address model.</param>
        /// <param name="dataSource">data source</param>
        /// <param name="selectIndex">select index</param>
        /// <returns>Error Message</returns>
        public string SaveContactAddress(ContactAddressModel newContactAddress, List<ContactAddressModel> dataSource, int selectIndex)
        {
            string errorMsg = string.Empty;
            bool isDeactivateLogic = ValidationUtil.IsYes(hdfDeactivateFlag.Value);
            ContactAddressModel oldContactAddress = dataSource.FirstOrDefault(f => f.RowIndex == selectIndex);
            ContactAddressModel[] duplicateContactAddresses = GetDuplicatedContactAddresses(newContactAddress);
            var isContactAddressDuplicate = duplicateContactAddresses != null && duplicateContactAddresses.Any();

            if (isDeactivateLogic)
            {
                //Disallow to deactivate a primary contact address and the new contact address is duplicate to itself.
                if (isContactAddressDuplicate
                    && duplicateContactAddresses[0].contactAddressPK.addressID == oldContactAddress.contactAddressPK.addressID)
                {
                    ddlAddressType.DisableEdit();
                    errorMsg = GetTextByKey("aca_contactaddress_msg_duplicatetoself");
                    return errorMsg;
                }

                oldContactAddress.auditModel.auditStatus = ACAConstant.INVALID_STATUS;
                oldContactAddress.auditModel.auditID = AppSession.User.PublicUserId;
                string endDate = txtDeactivatedContactAddressEndDate.Text.Trim();

                if (!string.IsNullOrEmpty(endDate))
                {
                    oldContactAddress.expirationDate = I18nDateTimeUtil.ParseFromUI(endDate);
                }
                else
                {
                    oldContactAddress.expirationDate = null;
                }

                Save(oldContactAddress, dataSource, selectIndex);
                newContactAddress.replaceAddressID = oldContactAddress.contactAddressPK.addressID;
                selectIndex = NO_SELECTED;

                if (isContactAddressDuplicate)
                {
                    /*
                     * To deactivate a primary contact address and the new contact address is duplicate to the existing one.
                     * So use the existing one to replace the deactivated contact address directly.
                     */
                    UpdateReplaceIDByPK(duplicateContactAddresses[0].contactAddressPK, oldContactAddress.contactAddressPK.addressID, dataSource);

                    return errorMsg;
                }
            }
            else
            {
                if (isContactAddressDuplicate)
                {
                    errorMsg = GetTextByKey("aca_contactaddress_msg_duplicate");
                    return errorMsg;
                }
            }

            //Set Primary address must after duplicate check. this method will overwrite other contact address's primary flag.
            newContactAddress.primary = SetPrimaryAddress();

            //Save new contact address
            SimpleAuditModel auditModel = new SimpleAuditModel();
            auditModel.auditID = AppSession.User.PublicUserId;
            auditModel.auditStatus = ACAConstant.VALID_STATUS;
            newContactAddress.auditModel = auditModel;

            Save(newContactAddress, dataSource, selectIndex);

            return errorMsg;
        }

        /// <summary>
        /// Gets a value indicating whether contact addresses is valid.
        /// </summary>
        /// <param name="contactAddresses">the contact address.</param>
        /// <param name="isLocate">the is locate.</param>
        /// <returns>true or false.</returns>
        public bool IsPassContactAddressValidation(ContactAddressModel[] contactAddresses, bool isLocate)
        {
            bool isPass = true;

            if (IsNeedValidateContactAddress())
            {
                isPass = false;
            }

            return isPass;
        }

        /// <summary>
        /// Indicating the form data whether is valid.
        /// </summary>
        /// <returns>True if the contact is valid</returns>
        public bool IsNeedValidateContactAddress()
        {
            bool needValidate = true;

            if (!IsEditable || !IsEnabled)
            {
                needValidate = false;
            }

            return needValidate;
        }

        /// <summary>
        /// Loads the selected contact address info.
        /// </summary>
        /// <param name="contactAddress">The contact address.</param>
        /// <returns>Return a boolean value indicates if the edit form is readonly.</returns>
        public bool LoadSelectedContactAddressInfo(ContactAddressModel contactAddress)
        {
            bool isDisable = false;

            if (contactAddress != null)
            {
                /*
                 * Below scenario need to disable the whole contact address form:
                 * 1. When "Contact Address Edit" is disabled (typical for external contact address)
                 * 2. When data source is Reference and contact address from reference
                 * 3. Select a deactivated contact address
                 * 4. In Account Management page and the "Contact Address Maintenance" is disabled.
                 * 5. In Customer detail page, the IsEditable property in Admin is setted as false.
                 */
                if (!StandardChoiceUtil.IsEnableContactAddressEdit()
                    || (ValidationUtil.IsTrue(hdfDisableContactForm.Value) && IsValidate)
                    || ACAConstant.INVALID_STATUS.Equals(contactAddress.auditModel.auditStatus, StringComparison.InvariantCultureIgnoreCase)
                    || (!ACAConstant.ContactSectionPosition.SpearForm.Equals(ContactSectionPosition) && !ACAConstant.ContactSectionPosition.AuthAgentCustomerDetail.Equals(ContactSectionPosition) && !StandardChoiceUtil.IsEnableContactAddressMaintenance())
                    || (ACAConstant.ContactSectionPosition.AuthAgentCustomerDetail.Equals(ContactSectionPosition) && !StandardChoiceUtil.IsCustomerDetailEditable())
                    || !IsEditable)
                {
                    DisableEditForm();
                    hdfDisableFormByDeactivate.Value = ACAConstant.COMMON_TRUE;
                    isDisable = true;
                }
                else
                {
                    EnableEditForm();
                    hdfDisableFormByDeactivate.Value = ACAConstant.COMMON_FALSE;
                    isDisable = false;
                }

                FillContactAddressInfo(contactAddress);
                divDeactiveContactAddress.Visible = false;
                hdfDeactivateFlag.Value = ACAConstant.COMMON_N;
            }
            else
            {
                //For the first time to load we should make sure the address type is default value
                SetDefaultAdress();
            }

            return isDisable;
        }

        /// <summary>
        /// Deactivates the contact address.
        /// </summary>
        /// <param name="contactAddress">The contact address.</param>
        public void DeactivateContactAddress(ContactAddressModel contactAddress)
        {
            if (contactAddress != null)
            {
                hdfDeactivateFlag.Value = ACAConstant.COMMON_Y;
                ClearEditForm(false);
                ddlAddressType.DisableEdit();
                DropDownListBindUtil.SetSelectedValue(ddlAddressType, contactAddress.addressType);
                divDeactiveContactAddress.Visible = true;

                SetDeactivatedContactAddressEndDate();
            }
        }

        #endregion Public Methods

        #region Protected Methods

        /// <summary>
        /// Handle the <c>OnInit</c> event.
        /// </summary>
        /// <param name="e">Event argument.</param>
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            if (!IsPostBack)
            {
                DropDownListBindUtil.BindContactAddressType(ddlAddressType, null);
                DropDownListBindUtil.BindStreetSuffix(ddlStreetType);
                DropDownListBindUtil.BindStreetDirection(ddlStreetDirection);
                DropDownListBindUtil.BindUnitType(ddlUnitType);
                DropDownListBindUtil.BindStreetDirection(ddlStreetSuffixDirection);
            }

            ddlCountry.BindItems();
            ddlCountry.SetCountryControls(txtZip, txtState, txtPhone, txtFax);
        }

        /// <summary>
        /// Handle the Page_Load event.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="e">Event argument.</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            ddlCountry.RelevantControlIDs = PhoneControlIDs;
            ddlCountry.RegisterScripts();

            // set the editable according to the ContactAddressMaintenance switch
            if (!ACAConstant.ContactSectionPosition.SpearForm.Equals(ContactSectionPosition))
            {
                IsEditable = IsEditable && StandardChoiceUtil.IsEnableContactAddressMaintenance();
            }

            if (!IsAppliedRegional)
            {
                ControlUtil.ApplyRegionalSetting(IsPostBack, false, true, !IsPostBack, ddlCountry);

                if (!AppSession.IsAdmin && !IsPostBack)
                {
                    SetCurrentCityAndState();
                }

                IsAppliedRegional = true;
            }

            if (!Visible)
            {
                return;
            }

            if (AppSession.IsAdmin)
            {
                divDeactiveContactAddress.Visible = true;
            }

            if (!ACAConstant.ContactSectionPosition.SpearForm.Equals(ContactSectionPosition))
            {
                ckbPrimary.Visible = false;
            }
        }

        /// <summary>
        /// Override OnPreRender event to initializes some parameters.
        /// </summary>
        /// <param name="e">Event argument.</param>
        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);
            ControlBuildHelper.AddValidationForStandardFields(ViewId, ModuleName, Permission, Controls);
            InitFormDesignerPlaceHolder(fdphContentAddress);

            //Switch the label key for Authorized Agent Customer Detail page.
            ChangeLabelKey();
        }

        /// <summary>
        /// Collect the expression input controls.
        /// </summary>
        /// <param name="viewID">view id for expression.</param>
        /// <param name="moduleName">module name.</param>
        /// <param name="templateEdit">template control</param>
        /// <param name="templatePrefix">template prefix.</param>
        /// <returns>Expression Controls</returns>
        protected override Dictionary<string, WebControl> CollectExpressionInputControls(string viewID, string moduleName, TemplateEdit templateEdit, string templatePrefix)
        {
            Dictionary<string, WebControl> expressionControls = base.CollectExpressionInputControls(viewID, moduleName, templateEdit, templatePrefix);

            /*
             * The "validateFlag" field is set in Contact Address Validation logic, the field need pass to Expression Builder,
             *   in Expressino Builder, agency will use the flag to determine if need to set some fields as Read-only.
             * This field not displayed in UI and just used in Contact Address Expression.
             */
            CapModel4WS capModel = AppSession.GetCapModelFromSession(moduleName);
            string ctlID = ExpressionUtil.GetFullControlFieldName(capModel, validateFlag.ID);
            expressionControls.Add(ctlID, validateFlag);

            return expressionControls;
        }

        #endregion Protected Methods

        #region Private Methods

        /// <summary>
        /// set the default address type
        /// </summary>
        private void SetDefaultAdress()
        {
            IBizDomainBll bizBll = ObjectFactory.GetObject<IBizDomainBll>();
            string singleValue = bizBll.GetValueForACAConfig(ConfigManager.AgencyCode, BizDomainConstant.STD_CONTACT_ADDRESS_TYPE);
            DropDownListBindUtil.SetSelectedValue(ddlAddressType, singleValue);
        }

        /// <summary>
        /// Gets the duplicated contact addresses.
        /// </summary>
        /// <param name="contactAddress">The exist contact address model list.</param>
        /// <returns>Duplicated contact addresses</returns>
        private ContactAddressModel[] GetDuplicatedContactAddresses(ContactAddressModel contactAddress)
        {
            ContactAddressModel[] duplicatedContactAddresses = null;

            PeopleModel4WS people = ContactUtil.GetPeopleModelFromContactSessionParameter();
            ContactAddressModel[] existContactAddresses = people.contactAddressList;
            if (existContactAddresses != null && existContactAddresses.Length > 0)
            {
                var peopleBll = ObjectFactory.GetObject<IPeopleBll>();
                duplicatedContactAddresses = peopleBll.GetDuplicateContactAddressList(existContactAddresses, contactAddress);
            }

            return duplicatedContactAddresses;
        }

        /// <summary>
        /// Fill contact address data to edit form.
        /// </summary>
        /// <param name="contactAddress">The contact address model with data.</param>
        private void FillContactAddressInfo(ContactAddressModel contactAddress)
        {
            if (contactAddress == null)
            {
                return;
            }

            DropDownListBindUtil.SetSelectedValue(ddlAddressType, contactAddress.addressType);
            txtStartDate.Text2 = contactAddress.effectiveDate;
            txtEndDate.Text2 = contactAddress.expirationDate;
            txtRecipient.Text = contactAddress.recipient;
            txtFullAddress.Text = contactAddress.fullAddress;
            txtAddressLine1.Text = contactAddress.addressLine1;
            txtAddressLine2.Text = contactAddress.addressLine2;
            txtAddressLine3.Text = contactAddress.addressLine3;
            txtStreetStart.Text = Convert.ToString(contactAddress.houseNumberStart);
            txtStreetEnd.Text = Convert.ToString(contactAddress.houseNumberEnd);
            DropDownListBindUtil.SetSelectedValue(ddlStreetDirection, contactAddress.streetDirection);
            txtPrefix.Text = contactAddress.streetPrefix;
            txtStreetName.Text = contactAddress.streetName;
            DropDownListBindUtil.SetSelectedValue(ddlStreetType, contactAddress.streetSuffix);
            DropDownListBindUtil.SetSelectedValue(ddlUnitType, contactAddress.unitType);
            txtUnitStart.Text = contactAddress.unitStart;
            txtUnitEnd.Text = contactAddress.unitEnd;
            DropDownListBindUtil.SetSelectedValue(ddlStreetSuffixDirection, contactAddress.streetSuffixDirection);

            DropDownListBindUtil.SetCountrySelectedValue(ddlCountry, contactAddress.countryCode, false, true, false);
            IsAppliedRegional = true;
            txtCity.Text = contactAddress.city;
            txtState.Text = contactAddress.state;
            txtZip.Text = ModelUIFormat.FormatZipShow(contactAddress.zip, contactAddress.countryCode, false);
            txtPhone.Text = ModelUIFormat.FormatPhone4EditPage(contactAddress.phone, contactAddress.countryCode);
            txtPhone.CountryCodeText = contactAddress.phoneCountryCode;
            txtFax.Text = ModelUIFormat.FormatPhone4EditPage(contactAddress.fax, contactAddress.countryCode);
            txtFax.CountryCodeText = contactAddress.faxCountryCode;

            txtLevelPrefix.Text = contactAddress.levelPrefix;
            txtLevelNbrStart.Text = contactAddress.levelNumberStart;
            txtLevelNbrEnd.Text = contactAddress.levelNumberEnd;
            txtHouseAlphaStart.Text = contactAddress.houseNumberAlphaStart;
            txtHouseAlphaEnd.Text = contactAddress.houseNumberAlphaEnd;
            ckbPrimary.Checked = ACAConstant.COMMON_Y.Equals(contactAddress.primary, StringComparison.InvariantCultureIgnoreCase);
            validateFlag.Text = contactAddress.validateFlag;
        }

        /// <summary>
        /// Collect contact address data from edit form.
        /// </summary>
        /// <param name="contactAddressInfo">Contact address model needed to fill data.</param>
        /// <returns>Contact address model, create a instance if the <see cref="contactAddressInfo"/> object is null.</returns>
        private ContactAddressModel GetContactAddressInfo(ContactAddressModel contactAddressInfo)
        {
            CapModel4WS currentCap = AppSession.GetCapModelFromSession(ModuleName);

            if (contactAddressInfo == null)
            {
                ContactAddressPKModel contactAddressPK = new ContactAddressPKModel();
                contactAddressPK.serviceProviderCode = ACAConstant.ContactSectionPosition.SpearForm.Equals(ContactSectionPosition) ? currentCap.capID.serviceProviderCode : ConfigManager.AgencyCode;

                PeopleModel4WS people = ContactUtil.GetPeopleModelFromContactSessionParameter();
                ContactAddressModel[] contactAddressList = people.contactAddressList;
                long? minAddressID = null;

                if (contactAddressList != null && contactAddressList.Length > 0)
                {
                    minAddressID = contactAddressList.ToList().Min(c => c.contactAddressPK.addressID);
                }

                /*
                 * For newly added contact address by daily side,
                 *  the addressID attribute must be less than 0 and different item use the different sequence.
                 * For example: -1, -2, -3...
                 * The special logic is requested by AA.
                 */
                minAddressID = minAddressID == null || minAddressID > -1 ? -1 : minAddressID - 1;
                contactAddressPK.addressID = minAddressID;
                contactAddressInfo = new ContactAddressModel();
                contactAddressInfo.contactAddressPK = contactAddressPK;
            }

            // Set audit model
            if (contactAddressInfo.auditModel == null)
            {
                contactAddressInfo.auditModel = new SimpleAuditModel();
            }

            contactAddressInfo.auditModel.auditID = AppSession.User.PublicUserId;
            contactAddressInfo.auditModel.auditStatus = ACAConstant.VALID_STATUS;

            if (string.IsNullOrEmpty(txtStartDate.Text))
            {
                contactAddressInfo.effectiveDate = null;
            }
            else
            {
                contactAddressInfo.effectiveDate = I18nDateTimeUtil.ParseFromUI(txtStartDate.Text);
            }

            if (string.IsNullOrEmpty(txtEndDate.Text))
            {
                contactAddressInfo.expirationDate = null;
            }
            else
            {
                contactAddressInfo.expirationDate = I18nDateTimeUtil.ParseFromUI(txtEndDate.Text);
            }

            contactAddressInfo.addressType = ddlAddressType.SelectedValue;
            contactAddressInfo.recipient = txtRecipient.Text.Trim();
            contactAddressInfo.fullAddress = txtFullAddress.Text.Trim();
            contactAddressInfo.addressLine1 = txtAddressLine1.Text.Trim();
            contactAddressInfo.addressLine2 = txtAddressLine2.Text.Trim();
            contactAddressInfo.addressLine3 = txtAddressLine3.Text.Trim();
            contactAddressInfo.houseNumberStart = StringUtil.ToInt(txtStreetStart.Text.Trim());
            contactAddressInfo.houseNumberEnd = StringUtil.ToInt(txtStreetEnd.Text.Trim());
            contactAddressInfo.streetDirection = ddlStreetDirection.SelectedValue;
            contactAddressInfo.streetPrefix = txtPrefix.Text.Trim();
            contactAddressInfo.streetName = txtStreetName.Text.Trim();
            contactAddressInfo.streetSuffix = ddlStreetType.SelectedValue;
            contactAddressInfo.unitType = ddlUnitType.SelectedValue;
            contactAddressInfo.unitStart = txtUnitStart.Text.Trim();
            contactAddressInfo.unitEnd = txtUnitEnd.Text.Trim();
            contactAddressInfo.streetSuffixDirection = ddlStreetSuffixDirection.SelectedValue;
            contactAddressInfo.countryCode = ddlCountry.SelectedValue;
            contactAddressInfo.city = txtCity.Text.Trim();
            contactAddressInfo.state = txtState.Text.Trim();
            contactAddressInfo.zip = txtZip.GetZip(ddlCountry.SelectedValue);
            contactAddressInfo.phone = txtPhone.GetPhone(ddlCountry.SelectedValue);
            contactAddressInfo.phoneCountryCode = txtPhone.CountryCodeText.Trim();
            contactAddressInfo.fax = txtFax.GetPhone(ddlCountry.SelectedValue);
            contactAddressInfo.faxCountryCode = txtFax.CountryCodeText.Trim();

            contactAddressInfo.levelPrefix = txtLevelPrefix.Text.Trim();
            contactAddressInfo.levelNumberStart = txtLevelNbrStart.Text.Trim();
            contactAddressInfo.levelNumberEnd = txtLevelNbrEnd.Text.Trim();
            contactAddressInfo.houseNumberAlphaStart = txtHouseAlphaStart.Text.Trim();
            contactAddressInfo.houseNumberAlphaEnd = txtHouseAlphaEnd.Text.Trim();

            return contactAddressInfo;
        }

        /// <summary>
        /// Update replace address ID by contact address PK.
        /// </summary>
        /// <param name="contactAddressPK">Contact address PK</param>
        /// <param name="replaceAddressID">Replace address ID</param>
        /// <param name="dataSource">contact address data source</param>
        private void UpdateReplaceIDByPK(ContactAddressPKModel contactAddressPK, long? replaceAddressID, List<ContactAddressModel> dataSource)
        {
            if (dataSource == null || contactAddressPK == null)
            {
                return;
            }

            var selectedItem = dataSource.SingleOrDefault(p =>
                p.contactAddressPK.serviceProviderCode == contactAddressPK.serviceProviderCode &&
                p.contactAddressPK.addressID == contactAddressPK.addressID);

            if (selectedItem == null)
            {
                return;
            }

            selectedItem.replaceAddressID = replaceAddressID;
            var position = dataSource.IndexOf(selectedItem);
            dataSource.RemoveAt(position);
            dataSource.Insert(position, selectedItem);
        }

        /// <summary>
        /// save contact address into data source
        /// </summary>
        /// <param name="contactAddress">current contact address</param>
        /// <param name="dataSource">contact address data source</param>
        /// <param name="selectIndex">select index</param>
        private void Save(ContactAddressModel contactAddress, List<ContactAddressModel> dataSource, int selectIndex)
        {
            if (selectIndex != -1)
            {
                int position = dataSource.IndexOf(dataSource.FirstOrDefault(f => f.RowIndex == selectIndex));
                dataSource.RemoveAt(position);
                dataSource.Insert(position, contactAddress);
            }
            else
            {
                int rowIndex = 0;

                if (dataSource.Count > 0)
                {
                    rowIndex = dataSource.Max(p => p.RowIndex) + 1;
                }

                contactAddress.RowIndex = rowIndex;
                dataSource.Add(contactAddress);
            }
        }

        /// <summary>
        /// Set deactivated contact address end date.
        /// </summary>
        private void SetDeactivatedContactAddressEndDate()
        {
            if (!AppSession.IsAdmin && divDeactiveContactAddress.Visible)
            {
                ITimeZoneBll timeBll = (ITimeZoneBll)ObjectFactory.GetObject(typeof(ITimeZoneBll));
                DateTime endDate = timeBll.GetAgencyCurrentDate(ConfigManager.AgencyCode);
                txtDeactivatedContactAddressEndDate.Text2 = endDate;
            }
        }

        /// <summary>
        /// Set primary address flag and set other contact address not primary address.
        /// </summary>
        /// <returns>primary address or not</returns>
        private string SetPrimaryAddress()
        {
            string isPrimaryAddress = ckbPrimary.Checked ? ACAConstant.COMMON_Y : ACAConstant.COMMON_N;
            PeopleModel4WS people = ContactUtil.GetPeopleModelFromContactSessionParameter();
            ContactAddressModel[] contactAddressList = people.contactAddressList;
            if (ckbPrimary.Checked && contactAddressList != null)
            {
                foreach (ContactAddressModel contactAddress in contactAddressList)
                {
                    contactAddress.primary = ACAConstant.COMMON_N;
                }
            }

            return isPrimaryAddress;
        }

        /// <summary>
        /// Set default value for City and State fields.
        /// </summary>
        private void SetCurrentCityAndState()
        {
            AutoFillCityAndStateUtil.SetCurrentCity(txtCity, ModuleName);
            AutoFillCityAndStateUtil.SetCurrentState(txtState, ModuleName);
        }

        /// <summary>
        /// Change the label key
        /// </summary>
        private void ChangeLabelKey()
        {
            if (this.ViewId == GviewID.AuthAgentCustomerDetailCAForm)
            {
                //lblContactAddressListInstruction.LabelKey = "aca_authagent_contactaddress_label_instruction";
                //lblContactAddressListTitle.LabelKey = "aca_authagent_contactaddress_label_title_listview";
                //lblAddContactAddress.LabelKey = "aca_authagent_contactaddress_label_addcontactaddress";
                //lblEditContactAddress.LabelKey = "aca_authagent_contactaddress_label_editcontactaddress";
                lblReplaceContactAddressHint.LabelKey = "aca_authagent_contactaddress_label_replace_hint";
                txtDeactivatedContactAddressEndDate.LabelKey = "aca_authagent_contactaddress_label_deactivate_enddate";
                lblReplaceContactAddressTitle.LabelKey = "aca_authagent_contactaddress_label_replace_title";

                ddlAddressType.LabelKey = "aca_authagent_contactaddress_label_addresstype";
                txtStartDate.LabelKey = "aca_authagent_contactaddress_label_startdate";
                txtEndDate.LabelKey = "aca_authagent_contactaddress_label_enddate";
                txtRecipient.LabelKey = "aca_authagent_contactaddress_label_recipient";
                txtFullAddress.LabelKey = "aca_authagent_contactaddress_label_fulladdress";
                txtAddressLine1.LabelKey = "aca_authagent_contactaddress_label_addressline1";
                txtAddressLine2.LabelKey = "aca_authagent_contactaddress_label_addressline2";
                txtAddressLine3.LabelKey = "aca_authagent_contactaddress_label_addressline3";
                txtStreetStart.LabelKey = "aca_authagent_contactaddress_label_streetstart";
                txtStreetEnd.LabelKey = "aca_authagent_contactaddress_label_streetend";
                ddlStreetDirection.LabelKey = "aca_authagent_contactaddress_label_direction";
                txtPrefix.LabelKey = "aca_authagent_contactaddress_label_prefix";
                txtStreetName.LabelKey = "aca_authagent_contactaddress_label_streetname";
                ddlStreetType.LabelKey = "aca_authagent_contactaddress_label_streettype";
                ddlUnitType.LabelKey = "aca_authagent_contactaddress_label_unittype";
                txtUnitStart.LabelKey = "aca_authagent_contactaddress_label_unitstart";
                txtUnitEnd.LabelKey = "aca_authagent_contactaddress_label_unitend";
                ddlStreetSuffixDirection.LabelKey = "aca_authagent_contactaddress_label_streetsuffixdirection";
                ddlCountry.LabelKey = "aca_authagent_contactaddress_label_country";
                txtCity.LabelKey = "aca_authagent_contactaddress_label_city";
                txtState.LabelKey = "aca_authagent_contactaddress_label_state";
                txtZip.LabelKey = "aca_authagent_contactaddress_label_zip";
                txtPhone.LabelKey = "aca_authagent_contactaddress_label_phone";
                txtFax.LabelKey = "aca_authagent_contactaddress_label_fax";
                txtLevelPrefix.LabelKey = "aca_authagent_contactaddress_label_levelprefix";
                txtLevelNbrStart.LabelKey = "aca_authagent_contactaddress_label_levelnumberstart";
                txtLevelNbrEnd.LabelKey = "aca_authagent_contactaddress_label_levelnumberend";
                txtHouseAlphaStart.LabelKey = "aca_authagent_contactaddress_label_housealphastart";
                txtHouseAlphaEnd.LabelKey = "aca_authagent_contactaddress_label_housealphaend";
            }
        }

        #endregion Private Methods
    }
}