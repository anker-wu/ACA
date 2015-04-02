#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: ContactInfo.ascx.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2013-2014
 *
 *  Description:
 *
 *  Notes:
 *      $Id: ContactInfo.ascx.cs 258008 2013-10-08 03:50:48Z ACHIEVO\blues.gao $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.UI;
using System.Web.UI.WebControls;

using Accela.ACA.BLL.APO;
using Accela.ACA.BLL.Cap;
using Accela.ACA.BLL.Common;
using Accela.ACA.BLL.People;
using Accela.ACA.Common;
using Accela.ACA.Common.Util;
using Accela.ACA.ExpressionBuild;
using Accela.ACA.UI.Model;
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
    /// the class for Contact information.
    /// </summary>
    public partial class ContactInfo : FormDesignerWithExpressionControl
    {
        #region Fields

        /// <summary>
        /// the contact type's item, organization.
        /// </summary>
        protected readonly string ORGANIZATION = ContactType4License.Organization.ToString().ToLower();

        /// <summary>
        /// the contact type's item, individual.
        /// </summary>
        protected readonly string INDIVIDUAL = ContactType4License.Individual.ToString().ToLower();

        /// <summary>
        /// ExpressionFactory class's instance
        /// </summary>
        private ExpressionFactory _expressionInstance;

        /// <summary>
        /// Indicating whether the contact address is enabled.
        /// </summary>
        private bool _isContactAddressEnabled = StandardChoiceUtil.IsEnableContactAddress();

        /// <summary>
        /// indicate whether to show contact type field
        /// </summary>
        private bool _isShowContactType;

        /// <summary>
        /// R-Reference,need reference data
        /// T-Transactional, user input data.
        /// N-No Limitation,both reference and daily
        /// </summary>
        private string _validateFlag = string.Empty;

        /// <summary>
        /// indicate the contact form is validate or not.
        /// </summary>
        private bool _isValidate;

        /// <summary>
        ///  The contact section position.
        /// </summary>
        private ACAConstant.ContactSectionPosition _contactSectionPosition;

        /// <summary>
        /// The is for search
        /// </summary>
        private bool _isForSearch;

        #endregion Fields

        /// <summary>
        /// Initializes a new instance of the <see cref="ContactInfo"/> class.
        /// </summary>
        public ContactInfo()
            : base(GviewID.ContactEdit)
        {
        }

        #region Events

        /// <summary>
        /// Contact type flag drop down value changed event.
        /// </summary>
        public event CommonEventHandler ContactTypeFlagChangedEvent;

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the contact sequence number.
        /// </summary>
        public string ContactSeqNumber
        {
            get
            {
                return hdnContactSeqNumber.Value;
            }

            set
            {
                hdnContactSeqNumber.Value = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this page is opened from the contact add new page.
        /// </summary>
        public bool IsOpenedFromContactAddNew
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the parent id.
        /// </summary>
        public string ParentID 
        { 
            get;
            set;
        }

        /// <summary>
        /// Gets or sets current contact expression type.
        /// </summary>
        public ExpressionType ContactExpressionType 
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the current contact type name.
        /// </summary>
        public string ContactType
        {
            get
            {
                return hfContatType.Value;
            }

            set
            {
                DropDownListBindUtil.SetSelectedValue(ddlContactType, value);
                hfContatType.Value = value;
                contactAddressList.ContactType = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether Is Need Selected LicenseCertification
        /// </summary>
        public bool IsNeedSelectedLicenseCertification
        {
            get
            {
                object obj = ViewState["IsNeedSelectedLicenseCertification"];
                return obj != null && bool.Parse(obj.ToString());
            }

            set
            {
                ViewState["IsNeedSelectedLicenseCertification"] = value;
            } 
        }

        /// <summary>
        /// Gets or sets value whether is organization
        /// </summary>
        public string ContactTypeFlag
        {
            get
            {
                object obj = ViewState["ContactTypeFlag"];

                if (obj == null)
                {
                    return string.Empty;
                }

                return obj.ToString();
            }

            set
            {
                ViewState["ContactTypeFlag"] = value;
            } 
        }

        /// <summary>
        /// Gets or sets a value indicating whether the data is new.
        /// </summary>
        public bool IsForNew
        {
            get
            { 
                object obj = ViewState["IsForNew"];
                return obj != null && bool.Parse(obj.ToString());
            }

            set
            {
                ViewState["IsForNew"] = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the form be used to search contact.
        /// </summary>
        public bool IsForSearch
        {
            get
            {
                return _isForSearch;
            }

            set
            {
                _isForSearch = value;

                // if this user control used for search, these control not validate.
                txtSSN.IsIgnoreValidate = IsForSearch;
                txtAppEmail.IsIgnoreValidate = IsForSearch;
                txtAppFein.IsIgnoreValidate = IsForSearch;
                txtAppPhone1.IsIgnoreValidate = IsForSearch;
                txtAppPhone2.IsIgnoreValidate = IsForSearch;
                txtAppPhone3.IsIgnoreValidate = IsForSearch;
                txtAppFax.IsIgnoreValidate = IsForSearch;
                txtAppZipApplicant.IsIgnoreValidate = IsForSearch;

                templateEdit.Visible = !IsForSearch;
                genericTemplate.Visible = !IsForSearch;
                divContactAddressList.Visible = !IsForSearch && _isContactAddressEnabled;

                if (_isForSearch)
                {
                    // Remove force require setting for Search form in ACA Admin.
                    ddlContactTypeFlag.Attributes.Remove("IsFieldRequired");
                }
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether IsShowContactType which Indicates whether to show the contact type field
        /// </summary>
        public bool IsShowContactType
        {
            get
            {
                return _isShowContactType;
            }

            set
            {
                _isShowContactType = value;
                ddlContactType.IsHidden = !value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether to Focus Add Contact button.
        /// </summary>
        public bool IsNeedFocusElement
        {
            get;
            set;
        }

        /// <summary>
        ///  Gets or sets the contact section position.
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
                contactAddressList.ContactSectionPosition = ContactSectionPosition;
                ViewId = PeopleUtil.GetContactGViewID(ContactSectionPosition);
                InitContactTypeList(ddlContactType);
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
        /// Sets the asit edit pop page section info(Format:title/index )
        /// </summary>
        public string SectionInfo
        {
            set
            {
                genericTemplate.SectionInfo = value;
            }
        }

        /// <summary>
        /// Gets the identity labels.
        /// </summary>
        public Dictionary<long, string> IdentityFieldLabels
        {
            get
            {
                //28,29..Is AA gview id,use it only for match identity label key & value.
                return new Dictionary<long, string>
                    {
                        { 28, txtSSN.LabelKey },
                        { 29, txtAppFein.LabelKey },
                        { 39, txtPassportNumber.LabelKey },
                        { 40, txtDriverLicenseNumber.LabelKey },
                        { 41, ddlDriverLicenseState.LabelKey },
                        { 42, txtStateNumber.LabelKey }
                    };
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether Auto-Fill whether need to be displayed.
        /// true-show,false-hidden, the default value is true.
        /// </summary>
        public bool ShowChk
        {
            get
            {
                return ViewState["Show_Check"] == null || ViewState["Show_Check"].ToString() != "0";
            }

            set
            {
                ViewState["Show_Check"] = value ? "1" : "0";
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the smart choice validate flag for license
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
                contactAddressList.IsValidate = value;
            }
        }

        /// <summary>
        /// Gets or sets the validate flag.
        /// </summary>
        /// <value>
        /// The validate flag.
        /// </value>
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
        /// Gets or sets a value indicating whether edit the form.
        /// </summary>
        public bool IsEditable
        {
            get
            {
                object result = ViewState["IsEditable"];

                if (result == null)
                {
                    IsEditable = true;
                    return true;
                }
                else
                {
                    return bool.Parse(result.ToString());
                }
            }

            set
            {
                ViewState["IsEditable"] = value;
                contactAddressList.IsEditable = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the form be used in Multiple contact edit form.
        /// </summary>
        public bool IsMultipleContact { get; set; }

        /// <summary>
        /// Gets control id prefix for each type of SPEAR form component.
        /// </summary>
        /// <returns>control id prefix.</returns>
        public string TemplateControlIDPrefix
        {
            get
            {
                string controlIDPrefix = string.Empty;

                switch (ContactExpressionType)
                {
                    case ExpressionType.Applicant:
                        controlIDPrefix = ACAConstant.CAP_APPLICANT_TEMPLATE_FIELD_PREFIX;
                        break;
                    case ExpressionType.Contact_1:
                        controlIDPrefix = ACAConstant.CAP_CONTACT1_TEMPLATE_FIELD_PREFIX;
                        break;
                    case ExpressionType.Contact_2:
                        controlIDPrefix = ACAConstant.CAP_CONTACT2_TEMPLATE_FIELD_PREFIX;
                        break;
                    case ExpressionType.Contact_3:
                        controlIDPrefix = ACAConstant.CAP_CONTACT3_TEMPLATE_FIELD_PREFIX;
                        break;
                    case ExpressionType.Contacts:
                        controlIDPrefix = ACAConstant.CAP_CONTACTS_TEMPLATE_FIELD_PREFIX;
                        break;
                    case ExpressionType.ReferenceContact:
                    case ExpressionType.AuthAgent_Customer_Detail:
                        controlIDPrefix = ACAConstant.REFERENCE_CONTACT_TEMPLATE_FIELD_PREFIX;
                        break;
                }

                return controlIDPrefix;
            }
        }

        /// <summary>
        /// Gets a value indicating whether contact address list is null or not.
        /// </summary>
        public bool IsAddressListEmpty
        {
            get
            {
                return contactAddressList.DataSource.Count < 1;
            }
        }

        /// <summary>
        /// Sets a value indicating whether current contact is reference or not.
        /// </summary>
        public bool IsRefContact
        {
            set
            {
                contactAddressList.IsRefContact = value;
            }
        }

        /// <summary>
        /// Gets a value indicating whether the country is hidden.
        /// </summary>
        public bool IsCountryHidden
        {
            get
            {
                return !ddlAppCountry.Visible;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the contact address list is initialized.
        /// </summary>
        public bool IsListInitialized
        {
            get
            {
                if (ViewState["IsListInitialized"] != null)
                {
                    return (bool)ViewState["IsListInitialized"];
                }

                return false;
            }

            set
            {
                ViewState["IsListInitialized"] = value;
            }
        }

        /// <summary>
        /// Gets or sets the primary contact flag.
        /// </summary>
        public string PrimaryContactFlag
        {
            get
            {
                return Convert.ToString(ViewState["Flag"]);
            }

            set
            {
                ViewState["Flag"] = value;
            }
        }

        /// <summary>
        /// Sets a value indicating whether the people template control need support always edit able or not.
        /// </summary>
        public bool SupportAlwaysEditable
        {
            set
            {
                templateEdit.SupportAlwaysEditable = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether country initialize.
        /// </summary>
        protected bool IsInitCountry
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets Permission.
        /// </summary>
        protected override GFilterScreenPermissionModel4WS Permission
        {
            get
            {
                string permissionValue = IsForSearch ? string.Empty : ContactType.Trim().Replace(ACAConstant.SPLIT_CHAR4 + ACAConstant.SPLIT_DOUBLE_VERTICAL, string.Empty);
                
                base.Permission = ControlBuildHelper.GetPermissionWithGenericTemplate(ViewId, GViewConstant.PERMISSION_PEOPLE, permissionValue);

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
                    ExpressionControls = CollectExpressionInputControls(GviewID.ContactEdit, ModuleName, templateEdit, TemplateControlIDPrefix);
                    var argumentPKModels = new List<ExpressionRuntimeArgumentPKModel>();
                    var contactArgPk = new ExpressionRuntimeArgumentPKModel();
                    contactArgPk.portletID = (long?)ContactExpressionType;
                    contactArgPk.viewKey1 = ContactType;
                    contactArgPk.viewKey2 = string.Empty;
                    contactArgPk.viewKey3 = string.Empty;
                    argumentPKModels.Add(contactArgPk);

                    if (genericTemplate.ASITUITables != null)
                    {
                        foreach (var viewKey3 in genericTemplate.ASITUITables)
                        {
                            var genericTemplateTableArgPk = new ExpressionRuntimeArgumentPKModel();
                            genericTemplateTableArgPk.portletID = (long?)ContactExpressionType;
                            genericTemplateTableArgPk.viewKey1 = ContactType;
                            genericTemplateTableArgPk.viewKey2 = genericTemplate.GenericTemplateGroupCode;
                            genericTemplateTableArgPk.viewKey3 = viewKey3;
                            argumentPKModels.Add(genericTemplateTableArgPk);
                        }
                    }
                    else
                    {
                        var genericTemplateArgPk = new ExpressionRuntimeArgumentPKModel();
                        genericTemplateArgPk.portletID = (long?)ContactExpressionType;
                        genericTemplateArgPk.viewKey1 = ContactType;
                        genericTemplateArgPk.viewKey2 = genericTemplate.GenericTemplateGroupCode;
                        genericTemplateArgPk.viewKey3 = string.Empty;
                        argumentPKModels.Add(genericTemplateArgPk);
                    }

                    _expressionInstance = new ExpressionFactory(ModuleName, ContactExpressionType, ExpressionControls, argumentPKModels, genericTemplate.UITables, ClientID);
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
                sbControls.Append(txtAppPhone1.ClientID);
                sbControls.Append(",").Append(txtAppPhone2.ClientID);
                sbControls.Append(",").Append(txtAppPhone3.ClientID);
                sbControls.Append(",").Append(txtAppFax.ClientID);
                return sbControls.ToString();
            }
        }

        /// <summary>
        /// Gets a value indicating whether the contact type editable settings or not.
        /// </summary>
        private bool ContactTypePermission
        {
            get
            {
                if (!AppSession.IsAdmin && ACAConstant.ContactSectionPosition.ModifyReferenceContact.Equals(ContactSectionPosition))
                {
                    return ContactUtil.GetContactTypeEditablePermissionByContactSeqNbr(Request.QueryString[UrlConstant.CONTACT_SEQ_NUMBER]);
                }

                return true;
            }
        }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Get contact condition list and contact message
        /// </summary>
        /// <param name="value">The auto fill value</param>
        /// <param name="moduleName">module name</param>
        /// <param name="prefix">prefix name</param>
        /// <returns>Contact Condition information(json format)</returns>
        public static string GetConditionInfo(string value, string moduleName, string prefix)
        {
            StringBuilder condition = new StringBuilder();
            StringBuilder message = new StringBuilder();
            string seqNumber = string.Empty;
            ConditionNoticeModel notice = new ConditionNoticeModel();
            ConditionType conditionType = new ConditionType();

            if (!string.IsNullOrEmpty(value))
            {
                JavaScriptSerializer javaScriptSerializer = new JavaScriptSerializer();
                AutoFillParameter parameter = javaScriptSerializer.Deserialize<AutoFillParameter>(value);

                if (parameter != null)
                {
                    switch (parameter.AutoFillType)
                    {
                        case ACAConstant.AutoFillType4SpearForm.Contact:
                            //Parameters: "Contact|contactSeqNumber|contactType".
                            IConditionBll conditionBll = (IConditionBll)ObjectFactory.GetObject(typeof(IConditionBll));
                            seqNumber = parameter.EntityRefId;
                            notice = conditionBll.GetContactConditionNotices(seqNumber);
                            conditionType = ConditionType.Contact;
                            break;

                        case ACAConstant.AutoFillType4SpearForm.ContactOwner:
                            //Parameters: "Owner|ownerNumber|sourceSeqNumber|ownerUID".
                            IOwnerBll ownerBll = (IOwnerBll)ObjectFactory.GetObject(typeof(IOwnerBll));
                            string ownerNumber = parameter.EntityId;
                            string sourceSeqNumber = parameter.EntityType;
                            string ownerUID = parameter.EntityRefId;
                            OwnerModel ownerModel = ownerBll.GetOwnerCondition(ConfigManager.AgencyCode, sourceSeqNumber, ownerNumber, ownerUID);

                            if (ownerModel != null)
                            {
                                notice.NoticeConditions = ownerModel.noticeConditions;
                                notice.HighestCondition = ownerModel.hightestCondition;
                                conditionType = ConditionType.OwnerInContact;
                            }

                            break;

                        case ACAConstant.AutoFillType4SpearForm.License:
                            //Parameters:"License|stateLicense|licenseType|licenseSeqNumber". 
                            ILicenseBLL licenseBll = (ILicenseBLL)ObjectFactory.GetObject(typeof(ILicenseBLL));
                            seqNumber = parameter.EntityRefId;
                            LicenseModel4WS licenseModel = licenseBll.GetLicenseCondition(ConfigManager.AgencyCode, long.Parse(seqNumber), AppSession.User.PublicUserId);

                            if (licenseModel != null)
                            {
                                notice.NoticeConditions = licenseModel.noticeConditions;
                                notice.HighestCondition = licenseModel.hightestCondition;
                                conditionType = ConditionType.License;
                            }

                            break;
                    }
                }
            }

            if (notice != null)
            {
                message = ConditionsUtil.BuildMessage(notice.HighestCondition, moduleName, notice.NoticeConditions, conditionType);

                if (notice.NoticeConditions != null)
                {
                    DataTable dtContactCondition = ConditionsUtil.GetConditionDataSource(notice.NoticeConditions);
                    condition = ConditionsUtil.BuildConditionList(dtContactCondition, moduleName, prefix, GviewID.ContactConditionList);
                }
            }

            string conditionInfo =
                "{Condition:\"" + (condition == null ? string.Empty : HttpUtility.HtmlEncode(condition.ToString()))
                + "\",Message:\"" + (message == null ? string.Empty : HttpUtility.HtmlEncode(message.ToString())) + "\"}";

            return conditionInfo;
        }

        /// <summary>
        /// Creates an original contact template.
        /// </summary>
        /// <param name="contactType">contact type.</param>
        public void CreateOriginalContactTemplate(string contactType)
        {
            if (string.IsNullOrEmpty(contactType))
            {
                templateEdit.DisplayAttributes(null, TemplateControlIDPrefix);
            }
            else
            {
                ITemplateBll templateBll = ObjectFactory.GetObject<ITemplateBll>();
                TemplateAttributeModel[] attributes = templateBll.GetPeopleTemplateAttributes(contactType, ConfigManager.AgencyCode, AppSession.User.PublicUserId);
                templateEdit.DisplayAttributes(attributes, TemplateControlIDPrefix);
            }
        }

        /// <summary>
        /// Create generic template
        /// </summary>
        /// <param name="contactType">Contact type.</param>
        public void CreateGenericTemplate(string contactType)
        {
            if (string.IsNullOrEmpty(contactType))
            {
                genericTemplate.ResetControl();
            }
            else
            {
                HideGenericTemplate4Registration();

                ITemplateBll templateBll = ObjectFactory.GetObject<ITemplateBll>();
                TemplateModel model = templateBll.GetContactTemplates(ConfigManager.AgencyCode, contactType, false, AppSession.User.UserSeqNum);
                genericTemplate.Display(model);
            }
        }

        /// <summary>
        /// Display contact addresses to contact address list.
        /// </summary>
        /// <param name="contactAddresses">Contact address list.</param>
        /// <param name="needValidateAddressType">Indicates whether need to validate the required settings for contact address type.</param>
        public void Display(IList<ContactAddressModel> contactAddresses, bool needValidateAddressType)
        {
            contactAddressList.Display(contactAddresses);
            contactAddressList.Validate(contactAddressList.DataSource, needValidateAddressType, true);
            IsListInitialized = true;
        }

        /// <summary>
        /// Clear the contact address list.
        /// </summary>
        public void ClearAddressList()
        {
            contactAddressList.Display(null);
        }

        /// <summary>
        /// Gets CapContactModel4WS model from UI fields.
        /// PeopleModel4WS is built in this method.
        /// </summary>
        /// <param name="capContactModel">The cap contact model.</param>
        /// <returns>CapContactModel4WS object.</returns>
        public CapContactModel4WS GetContactModel(CapContactModel4WS capContactModel = null)
        {
            CapContactModel4WS capContact = capContactModel ?? new CapContactModel4WS();
            PeopleModel4WS people = GetPeopleModel();
            capContact.people = people;

            // If hiden the contact permission in contact edit must be set the "" value.
            if (radioListContactPermission.Visible)
            {
                capContact.accessLevel = radioListContactPermission.GetValue();
            }

            capContact.validateFlag = ValidateFlag;

            if (!string.IsNullOrEmpty(hdnRefContactSeqNumber.Value))
            {
                capContact.refContactNumber = hdnRefContactSeqNumber.Value;
            }

            return capContact;
        }

        /// <summary>
        /// Gets People model from UI fields.
        /// PeopleModel4WS is built in this method.
        /// </summary>
        /// <param name="ignoreVisible">if set to <c>true</c> [ignore visible].</param>
        /// <returns>A <see cref="PeopleModel4WS" /> object</returns>
        public PeopleModel4WS GetPeopleModel(bool ignoreVisible = true)
        {
            PeopleModel4WS people = ContactUtil.GetPeopleModelFromContactSessionParameter() ?? new PeopleModel4WS();

            people.salutation = ControlUtil.GetControlValue(ddlAppSalutation, ignoreVisible);
            people.email = ControlUtil.GetControlValue(txtAppEmail, ignoreVisible);

            people.businessName = ControlUtil.GetControlValue(txtAppOrganizationName, ignoreVisible);
            people.businessName2 = ControlUtil.GetControlValue(txtBusinessName2, ignoreVisible);
            people.tradeName = ControlUtil.GetControlValue(txtAppTradeName, ignoreVisible);
            people.fein = MaskUtil.UpdateFEIN(hfFEIN.Value, ControlUtil.GetControlValue(txtAppFein, ignoreVisible));
            hfFEIN.Value = people.fein;
            txtAppFein.Text = MaskUtil.FormatFEINShow(ControlUtil.GetControlValue(txtAppFein, ignoreVisible), StandardChoiceUtil.IsEnableFeinMasking());

            people.firstName = ControlUtil.GetControlValue(txtAppFirstName, ignoreVisible);
            people.lastName = ControlUtil.GetControlValue(txtAppLastName, ignoreVisible);
            people.fullName = ControlUtil.GetControlValue(txtAppFullName, ignoreVisible);
            people.middleName = ControlUtil.GetControlValue(txtAppMiddleName, ignoreVisible);
            people.title = ControlUtil.GetControlValue(txtTitle, ignoreVisible);
            people.socialSecurityNumber = MaskUtil.UpdateSSN(hfSSN.Value, ControlUtil.GetControlValue(txtSSN, ignoreVisible));
            hfSSN.Value = people.socialSecurityNumber;
            txtSSN.Text = MaskUtil.FormatSSNShow(ControlUtil.GetControlValue(txtSSN, ignoreVisible));

            if (txtAppBirthDate.Visible && !string.IsNullOrEmpty(txtAppBirthDate.Text.Trim()))
            {
                people.birthDate = I18nDateTimeUtil.ConvertDateTimeStringFromUIToWebService(txtAppBirthDate.Text.Trim());
            }
            else
            {
                people.birthDate = null;
            }

            if (ddlContactTypeFlag.SelectedValue.ToLower().Equals(ORGANIZATION))
            {
                people.gender = string.Empty;
            }
            else
            {
                people.gender = ControlUtil.GetControlValue(radioListAppGender);
            }

            people.phone1 = txtAppPhone1.GetPhone(ddlAppCountry.SelectedValue.Trim(), ignoreVisible);
            people.phone1CountryCode = ControlUtil.GetCountryCodeText(txtAppPhone1, ignoreVisible);
            people.phone2 = txtAppPhone2.GetPhone(ddlAppCountry.SelectedValue.Trim(), ignoreVisible);
            people.phone2CountryCode = ControlUtil.GetCountryCodeText(txtAppPhone2, ignoreVisible);
            people.phone3 = txtAppPhone3.GetPhone(ddlAppCountry.SelectedValue.Trim(), ignoreVisible);
            people.phone3CountryCode = ControlUtil.GetCountryCodeText(txtAppPhone3, ignoreVisible);
            people.fax = txtAppFax.GetPhone(ddlAppCountry.SelectedValue.Trim(), ignoreVisible);
            people.faxCountryCode = ControlUtil.GetCountryCodeText(txtAppFax, ignoreVisible);
            people.namesuffix = ControlUtil.GetControlValue(txtAppSuffix, ignoreVisible);
            people.birthCity = ControlUtil.GetControlValue(txtBirthplaceCity, ignoreVisible);
            people.stateIDNbr = ControlUtil.GetControlValue(txtStateNumber, ignoreVisible);

            if (txtDeceasedDate.Visible && !string.IsNullOrEmpty(txtDeceasedDate.Text.Trim()))
            {
                people.deceasedDate = I18nDateTimeUtil.ConvertDateTimeStringFromUIToWebService(txtDeceasedDate.Text.Trim());
            }
            else
            {
                people.deceasedDate = null;
            }

            people.passportNumber = ControlUtil.GetControlValue(txtPassportNumber, ignoreVisible);
            people.driverLicenseNbr = ControlUtil.GetControlValue(txtDriverLicenseNumber, ignoreVisible);
            people.race = ControlUtil.GetControlValue(ddlRace, ignoreVisible);
            people.birthState = ControlUtil.GetControlValue(ddlBirthplaceState, ignoreVisible);
            people.birthRegion = ControlUtil.GetControlValue(ddlBirthplaceCountry, ignoreVisible);
            people.driverLicenseState = ControlUtil.GetControlValue(ddlDriverLicenseState, ignoreVisible);
            people.preferredChannel = ControlUtil.GetControlValue(ddlPreferredChannel, ignoreVisible);
            people.comment = ControlUtil.GetControlValue(txtNotes, ignoreVisible);

            // International switch turn on.
            people.countryCode = ddlAppCountry.SelectedValue.Trim();

            CapModel4WS capModel = AppSession.GetCapModelFromSession(ModuleName);
            string agencyCode = ConfigManager.AgencyCode;

            if (ACAConstant.ContactSectionPosition.SpearForm.Equals(ContactSectionPosition))
            {
                agencyCode = capModel.capID.serviceProviderCode;
                people.auditID = AppSession.User.PublicUserId;
                IEnumerable<PeopleAKAModel> peopleAkaModels = txtAKAName.GetValue(
                    hdnRefContactSeqNumber.Value,
                    agencyCode,
                    AppSession.User.PublicUserId,
                    ACAConstant.VALID_STATUS);
                people.peopleAKAList = peopleAkaModels != null ? peopleAkaModels.ToArray() : null;
            }

            people.serviceProviderCode = agencyCode;
            people.postOfficeBox = ControlUtil.GetControlValue(txtAppPOBox, ignoreVisible);
            people.auditStatus = ACAConstant.VALID_STATUS;
            people.flag = PrimaryContactFlag;

            CompactAddressModel4WS compactAddress = new CompactAddressModel4WS();
            compactAddress.addressLine1 = ControlUtil.GetControlValue(txtAppStreetAdd1, ignoreVisible);
            compactAddress.addressLine2 = ControlUtil.GetControlValue(txtAppStreetAdd2, ignoreVisible);
            compactAddress.addressLine3 = ControlUtil.GetControlValue(txtAppStreetAdd3, ignoreVisible);
            compactAddress.city = ControlUtil.GetControlValue(txtAppCity, ignoreVisible);
            compactAddress.state = ControlUtil.GetControlValue(txtAppState, ignoreVisible);
            compactAddress.resState = txtAppState.Visible || ignoreVisible ? txtAppState.ResText.Trim() : string.Empty;
            compactAddress.zip = txtAppZipApplicant.Visible || ignoreVisible ? txtAppZipApplicant.GetZip(ddlAppCountry.SelectedValue.Trim()) : string.Empty;
            compactAddress.countryCode = ddlAppCountry.SelectedValue.Trim();
            
            people.compactAddress = compactAddress;

            people.contactSeqNumber = hdnContactSeqNumber.Value;
            people.contactType = ContactType;
            people.contactTypeFlag = ddlContactTypeFlag.SelectedValue;
            people.attributes = templateEdit.GetAttributeModels();
            people.template = genericTemplate.GetTemplateModel(true);

            PeopleUtil.SetPeopleTemplateContactSeqNum(people);

            if (!string.IsNullOrEmpty(hdnRefContactSeqNumber.Value))
            {
                //Merge the reference data and UI data to prevent lose the data which fields hidden by form designer.
                CapUtil.MergeRefDataToUIData<PeopleModel4WS, PeopleModel4WS>(
                    ref people,
                    "peopleModel",
                    "people*",
                    "contactSeqNumber",
                    hdnRefContactSeqNumber.Value,
                    ModuleName,
                    RefEntityCache,
                    Permission,
                    ViewId);
            }

            /* If the SSN field is set hidden in ACA admin, the people.socialSecurityNumber will be null after get the value from ACA contact form on Account Management page.
             * After update the contact record in ACA, the people.socialSecurityNumber will merged back with referData. So set value to people.maskedSsn need after the
             * CapUtil.MergeRefDataToUIData function. Otherwise, the SSN value will lost if hide the field in ACA after update the contact record on AccountContactEdit page. 
             */
            people.maskedSsn = people.socialSecurityNumber;

            ClearRefEntityCache();

            return people;
        }

        /// <summary>
        /// Gets the search condition
        /// </summary>
        /// <returns>PeopleModel object.</returns>
        public PeopleModel GetSearchCondition()
        {
            PeopleModel people = new PeopleModel();
            people.contactType = ddlContactType.SelectedValue;
            people.contactTypeFlag = ddlContactTypeFlag.SelectedValue;
            people.salutation = ddlAppSalutation.SelectedValue;
            people.businessName = txtAppOrganizationName.Text.Trim();
            people.tradeName = txtAppTradeName.Text.Trim();
            people.fein = txtAppFein.Text.Trim();
            people.firstName = txtAppFirstName.Text.Trim();
            people.middleName = txtAppMiddleName.Text.Trim();
            people.lastName = txtAppLastName.Text.Trim();
            people.fullName = txtAppFullName.Text.Trim();
            people.title = txtTitle.Text.Trim();
            people.socialSecurityNumber = txtSSN.Text.Trim();

            string birthDate = txtAppBirthDate.Text.Trim();

            if (!string.IsNullOrEmpty(birthDate))
            {
                people.birthDate = I18nDateTimeUtil.ParseFromUI(birthDate);
            }
            else
            {
                people.birthDate = null;
            }

            people.gender = radioListAppGender.SelectedValue;
            people.postOfficeBox = txtAppPOBox.Text.Trim();
            people.phone1 = txtAppPhone1.GetPhone(ddlAppCountry.SelectedValue.Trim());
            people.phone1CountryCode = txtAppPhone1.Visible ? txtAppPhone1.CountryCodeText.Trim() : string.Empty;
            people.phone2 = txtAppPhone2.GetPhone(ddlAppCountry.SelectedValue.Trim());
            people.phone2CountryCode = txtAppPhone2.Visible ? txtAppPhone2.CountryCodeText.Trim() : string.Empty;
            people.phone3 = txtAppPhone3.GetPhone(ddlAppCountry.SelectedValue.Trim());
            people.phone3CountryCode = txtAppPhone3.Visible ? txtAppPhone3.CountryCodeText.Trim() : string.Empty;
            people.fax = txtAppFax.GetPhone(ddlAppCountry.SelectedValue);
            people.faxCountryCode = txtAppFax.Visible ? txtAppFax.CountryCodeText.Trim() : string.Empty;
            people.email = txtAppEmail.Text.Trim();
            people.preferredChannel = string.IsNullOrEmpty(ddlPreferredChannel.SelectedValue) ? (int?)null : int.Parse(ddlPreferredChannel.SelectedValue);
            people.race = ddlRace.SelectedValue;
            people.passportNumber = txtPassportNumber.Text.Trim();
            people.driverLicenseNbr = txtDriverLicenseNumber.Text.Trim();
            people.driverLicenseState = ddlDriverLicenseState.Text.Trim();
            people.stateIDNbr = txtStateNumber.Text.Trim();
            people.birthRegion = ddlBirthplaceCountry.SelectedValue;
            people.birthCity = txtBirthplaceCity.Text.Trim();
            people.birthState = ddlBirthplaceState.Text.Trim();
            people.businessName2 = txtBusinessName2.Text.Trim();
            people.auditStatus = ACAConstant.VALID_STATUS;
            people.namesuffix = txtAppSuffix.Text.Trim();
            people.comment = txtNotes.Text.Trim();

            string deceasedDate = txtDeceasedDate.Text.Trim();

            if (!string.IsNullOrEmpty(deceasedDate))
            {
                people.deceasedDate = I18nDateTimeUtil.ParseFromUI(deceasedDate);
            }
            else
            {
                people.deceasedDate = null;
            }

            CompactAddressModel compactAddress = new CompactAddressModel();
            compactAddress.countryCode = ddlAppCountry.SelectedValue;
            compactAddress.addressLine1 = txtAppStreetAdd1.Text.Trim();
            compactAddress.addressLine2 = txtAppStreetAdd2.Text.Trim();
            compactAddress.addressLine3 = txtAppStreetAdd3.Text.Trim();
            compactAddress.city = txtAppCity.Text.Trim();
            compactAddress.state = txtAppState.Text.Trim();
            compactAddress.zip = txtAppZipApplicant.GetZip(ddlAppCountry.SelectedValue.Trim());
            people.compactAddress = compactAddress;
            people.countryCode = ControlUtil.GetControlValue(ddlAppCountry);

            string agencyCode = ConfigManager.AgencyCode;
            bool isInspearForm = ACAConstant.ContactSectionPosition.SpearForm.Equals(ContactSectionPosition);

            if (isInspearForm)
            {
                CapModel4WS capModel = AppSession.GetCapModelFromSession(ModuleName);
                agencyCode = capModel.capID.serviceProviderCode;
            }

            people.serviceProviderCode = agencyCode;

            return people;
        }

        /// <summary>
        /// Gets the people model and get country visible status in this view.
        /// When country hidden, the country value may need clear that section in account register/edit.
        /// </summary>
        /// <param name="isCountryHidden">if set to <c>true</c> [is country hidden].</param>
        /// <returns>the web service model for people</returns>
        public PeopleModel4WS GetPeopleModel(ref bool isCountryHidden)
        {
            isCountryHidden = !ddlAppCountry.Visible;

            return GetPeopleModel();
        }

        /// <summary>
        /// Get contact addresses from contact address list.
        /// </summary>
        /// <returns>Array of contact address.</returns>
        public ContactAddressModel[] GetContactAddressList()
        {
            return contactAddressList.GetContactAddresses();
        }

        /// <summary>
        /// clear contact data in the form
        /// </summary>
        /// <param name="clearCountry">if set to <c>true</c> [clear country].</param>
        /// <param name="isResetTemplate">if set to <c>true</c> [reset template].</param>
        public void ClearContactForm(bool clearCountry, bool isResetTemplate = true)
        {
            string[] filterControlIDs = null;

            if (ddlContactType.IsHidden)
            {
                filterControlIDs = new string[] { ddlContactType.ID };
            }

            ControlUtil.ClearValue(this, filterControlIDs);
            ucConditon.HideCondition();

            filterControlIDs = genericTemplate.ReadOnlyControlIds != null && genericTemplate.ReadOnlyControlIds.Count > 0
                ? genericTemplate.ReadOnlyControlIds.ToArray()
                : null;
            EnableContactForm(filterControlIDs);

            if (clearCountry && !IsForSearch)
            {
                ControlUtil.ClearRegionalSetting(ddlAppCountry, IsForSearch, ModuleName, Permission, ViewId);
                ControlUtil.ClearRegionalSetting(ddlBirthplaceCountry, IsForSearch, ModuleName, Permission, ViewId);
            }
            else
            {
                ControlUtil.ApplyRegionalSetting(false, IsForSearch, true, !IsForSearch, ddlAppCountry);
                ControlUtil.ApplyRegionalSetting(false, IsForSearch, true, !IsForSearch, ddlBirthplaceCountry);
            }

            if (isResetTemplate)
            {
                //Clear generic template table.
                genericTemplate.IsReadOnly = false;
                ResetTemplate();

                CreateOriginalContactTemplate(ContactType.Trim());
                CreateGenericTemplate(ContactType.Trim());
            }

            ContactTypeFlag = string.Empty;
            hdnDisableStatus.Value = string.Empty;
            hfLockStandardFileds.Value = string.Empty;
            hdnContactSeqNumber.Value = string.Empty;
            hdnRefContactSeqNumber.Value = string.Empty;
        }

        /// <summary>
        /// reset the template field.
        /// </summary>
        public void ResetTemplate()
        {
            templateEdit.ResetControl();
            genericTemplate.ResetControl();
        }

        /// <summary>
        /// Initial Country control
        /// </summary>
        public void InitCountry()
        {
            ddlAppCountry.RelevantControlIDs = RelevantControlIDs;
            ddlAppCountry.RegisterScripts();
        }

        /// <summary>
        /// Set current City and State
        /// </summary>
        public void SetCurrentCityAndState()
        {
            AutoFillCityAndStateUtil.SetCurrentCity(txtAppCity, ModuleName);
            AutoFillCityAndStateUtil.SetCurrentState(txtAppState, ModuleName);
        }

        /// <summary>
        /// True must fill all required field, False skip validate required field
        /// </summary>
        /// <param name="contactOrder">contact order</param>
        public void SetSectionRequired(string contactOrder)
        {
            radioListAppGender.AddRequiredValidator(contactOrder);
            radioListContactPermission.AddRequiredValidator(contactOrder);
        }

        /// <summary>
        /// when set the IsEditable false in ACA admin disable the contact form.
        /// </summary>
        /// <param name="isEditable">a value indicating for Editable</param>
        /// <param name="filterControlIDs">Some control IDs need to keep always editable.</param>
        public void DisableContactForm(bool isEditable, string[] filterControlIDs)
        {
            if (!isEditable)
            {
                DisableContactEdit(filterControlIDs);
            }
        }

        /// <summary>
        /// get full access and no access permission according to 'Filter search result' in ACA admin.
        /// </summary>
        /// <param name="contactType">Contact type.</param>
        /// <returns>"F" means full access or "N" means no access</returns>
        public string GetDefaultContactPermisssion(string contactType)
        {
            string defaultValue = string.Empty;

            if (!IsForSearch)
            {
                defaultValue = ContactUtil.GetDefaultContactPermisssion(contactType, ViewId, ModuleName, radioListContactPermission.ID);
            }

            return defaultValue;
        }

        /// <summary>
        /// clean license form , temple field and button visible.
        /// </summary>
        /// <param name="clearCountry">if set to <c>true</c> [clear country].</param>
        /// <param name="isResetTemplate">if set to <c>true</c> [reset template].</param>
        public void ResetContactForm(bool clearCountry, bool isResetTemplate = true)
        {
            ClearContactForm(clearCountry, isResetTemplate);

            if (!clearCountry)
            {
                SetCurrentCityAndState();
            }
        }

        /// <summary>
        /// Indicating the form data whether is valid.
        /// </summary>
        /// <returns>True if the contact is valid</returns>
        public bool IsNeedValidateContactAddress()
        {
            bool needValidate = !(!IsEditable || IsFormEmpty() || !_isContactAddressEnabled);

            return needValidate;
        }

        /// <summary>
        /// Validate whether has at least one search criteria has been entered when search by Contact. 
        /// </summary>
        /// <returns>true: input at least one condition; false: doesn't input any condition</returns>
        public bool ValidateSearchCondition()
        {
            bool isInputCondition = true;

            string zip = txtAppZipApplicant.Text.Trim();

            if (zip.EndsWith("-", StringComparison.InvariantCulture))
            {
                zip = zip.Replace("-", string.Empty).Trim();
            }

            if (string.IsNullOrEmpty(ControlUtil.GetControlValue(ddlContactType)) 
                && string.IsNullOrEmpty(ControlUtil.GetControlValue(ddlContactTypeFlag)) 
                && string.IsNullOrEmpty(ControlUtil.GetControlValue(ddlAppSalutation)) 
                && txtTitle.Text.Trim().Length == 0
                && txtAppFirstName.Text.Trim().Length == 0 
                && txtAppMiddleName.Text.Trim().Length == 0
                && txtAppBirthDate.Text.Trim().Length == 0 
                && radioListAppGender.SelectedIndex < 0
                && txtAppLastName.Text.Trim().Length == 0 
                && txtAppFullName.Text.Trim().Length == 0 
                && txtAppOrganizationName.Text.Trim().Length == 0 
                && txtAppTradeName.Text.Trim().Length == 0
                && txtSSN.Text.Trim().Length == 0 
                && txtAppFein.Text.Trim().Length == 0 
                && string.IsNullOrEmpty(ControlUtil.GetControlValue(ddlAppCountry))
                && txtAppStreetAdd1.Text.Trim().Length == 0 
                && txtAppStreetAdd2.Text.Trim().Length == 0
                && txtAppStreetAdd3.Text.Trim().Length == 0
                && txtAppCity.Text.Trim().Length == 0
                && string.IsNullOrEmpty(ControlUtil.GetControlValue(txtAppState))
                && zip.Length == 0
                && txtAppPOBox.Text.Trim().Length == 0 
                && txtAppPhone1.Text.Trim().Length == 0 
                && txtAppPhone1.CountryCodeText.Trim().Length == 0
                && txtAppPhone3.Text.Trim().Length == 0 
                && txtAppPhone3.CountryCodeText.Trim().Length == 0
                && txtAppPhone2.Text.Trim().Length == 0 
                && txtAppPhone2.CountryCodeText.Trim().Length == 0
                && txtAppFax.Text.Trim().Length == 0 
                && txtAppFax.CountryCodeText.Trim().Length == 0 
                && txtAppEmail.Text.Trim().Length == 0
                && string.IsNullOrEmpty(ControlUtil.GetControlValue(ddlPreferredChannel)) 
                && string.IsNullOrEmpty(ControlUtil.GetControlValue(ddlRace)) 
                && txtPassportNumber.Text.Trim().Length == 0 
                && txtDriverLicenseNumber.Text.Trim().Length == 0
                && string.IsNullOrEmpty(ControlUtil.GetControlValue(ddlDriverLicenseState)) 
                && txtStateNumber.Text.Trim().Length == 0
                && string.IsNullOrEmpty(ControlUtil.GetControlValue(ddlBirthplaceCountry))
                && txtBirthplaceCity.Text.Trim().Length == 0 
                && string.IsNullOrEmpty(ControlUtil.GetControlValue(ddlBirthplaceState)) 
                && txtBusinessName2.Text.Trim().Length == 0 
                && txtDeceasedDate.Text.Trim().Length == 0
                && string.IsNullOrEmpty(txtAppSuffix.Text.Trim())
                && string.IsNullOrEmpty(txtNotes.Text.Trim()))
            {
                isInputCondition = false;
            }

            return isInputCondition;
        }

        /// <summary>
        /// Displays people/contact information to UI.
        /// </summary>
        /// <param name="people">a PeopleModel4WS</param>
        public void Display(PeopleModel4WS people)
        {
            DisplayPeople(people, people == null ? null : people.contactType, string.Empty, string.Empty);
        }

        /// <summary>
        /// Load contact properties
        /// </summary>
        public void LoadContactProperties()
        {
            //when do post back, the view state have not update when the control value changed by js.
            if (Request.Form[radioListAppGender.UniqueID] == null)
            {
                radioListAppGender.SelectedIndex = -1;
            }

            string contactType = ContactType.Trim();

            if (!IsForSearch)
            {
                if (AppSession.IsAdmin)
                {
                    /*
                     * In Admin, the Hidden contact type will be popuplated as the "ACAConstant.SPLIT_CHAR4 + ACAConstant.SPLIT_DOUBLE_VERTICAL + Contact Type"
                     *  to implement the related logic for form designer. So remove the useless part before get the actually contact type.
                     */
                    contactType = contactType.Replace(ACAConstant.SPLIT_CHAR4 + ACAConstant.SPLIT_DOUBLE_VERTICAL, string.Empty);
                }
                else if (!IsAppliedRegional)
                {
                    SetCurrentCityAndState();
                }

                ResetTemplate();
                CreateOriginalContactTemplate(contactType);
                CreateGenericTemplate(contactType);
            }

            // Marked 'contacts' privilege in aca admin 'filter search result',set 'full access' as default value, otherwise set 'no access'.
            SetDefaultValue4ContactPermission(contactType);
        }

        /// <summary>
        /// Displays people/contact information to UI.
        /// </summary>
        /// <param name="people">a PeopleModel4WS</param>
        /// <param name="contactType">Contact type</param>
        /// <param name="refContactNumber">Reference contact number</param>
        /// <param name="accessLevel">access level</param>
        public void DisplayPeople(PeopleModel4WS people, string contactType, string refContactNumber, string accessLevel)
        {
            ContactType = contactType;

            if (people != null)
            {
                if (ACAConstant.ContactSectionPosition.SpearForm.Equals(ContactSectionPosition))
                {
                    ConditionsUtil.ShowCondition(ucConditon, people);
                }

                string countrycode = people.compactAddress != null ? people.compactAddress.countryCode : string.Empty;
                string zip = people.compactAddress != null ? people.compactAddress.zip : string.Empty;

                DropDownListBindUtil.SetCountrySelectedValue(ddlAppCountry, countrycode, false, true, IsForSearch);
                DropDownListBindUtil.SetCountrySelectedValue(ddlBirthplaceCountry, people.birthRegion, false, true, IsForSearch);
                IsAppliedRegional = true;
                DropDownListBindUtil.SetSelectedValue(ddlAppSalutation, people.salutation);
                DropDownListBindUtil.SetSelectedValue(ddlContactTypeFlag, people.contactTypeFlag);
                txtTitle.Text = people.title;

                txtAppOrganizationName.Text = people.businessName;
                txtBusinessName2.Text = people.businessName2;
                txtAppTradeName.Text = people.tradeName;
                txtAppFein.Text = MaskUtil.FormatFEINShow(people.fein, StandardChoiceUtil.IsEnableFeinMasking());
                hfFEIN.Value = people.fein;

                txtBirthplaceCity.Text = people.birthCity;
                txtStateNumber.Text = people.stateIDNbr;
                txtAKAName.SetValue(people.peopleAKAList);
                ddlBirthplaceState.Text = people.birthState;
                txtDeceasedDate.Text2 = people.deceasedDate;
                txtPassportNumber.Text = people.passportNumber;
                ddlDriverLicenseState.Text = people.driverLicenseState;
                txtDriverLicenseNumber.Text = people.driverLicenseNbr;
                DropDownListBindUtil.SetSelectedValue(ddlRace, people.race);
                txtNotes.Text = people.comment;
                DropDownListBindUtil.SetSelectedValue(ddlPreferredChannel, people.preferredChannel);

                txtAppMiddleName.Text = people.middleName;
                txtAppFirstName.Text = people.firstName;
                txtAppLastName.Text = people.lastName;
                txtAppFullName.Text = people.fullName;
                txtTitle.Text = people.title;
                txtSSN.Text = MaskUtil.FormatSSNShow(people.socialSecurityNumber);
                hfSSN.Value = people.socialSecurityNumber;

                txtAppSuffix.Text = people.namesuffix;
                txtAppBirthDate.Text2 = people.birthDate;
                DropDownListBindUtil.SetSelectedValueForRadioList(radioListAppGender, people.gender);
                txtAppEmail.Text = people.email;

                txtAppPhone1.CountryCodeText = people.phone1CountryCode;
                txtAppPhone1.Text = ModelUIFormat.FormatPhone4EditPage(people.phone1, countrycode);
                txtAppPhone2.CountryCodeText = people.phone2CountryCode;
                txtAppPhone2.Text = ModelUIFormat.FormatPhone4EditPage(people.phone2, countrycode);
                txtAppPhone3.CountryCodeText = people.phone3CountryCode;
                txtAppPhone3.Text = ModelUIFormat.FormatPhone4EditPage(people.phone3, countrycode);
                txtAppFax.CountryCodeText = people.faxCountryCode;
                txtAppFax.Text = ModelUIFormat.FormatPhone4EditPage(people.fax, countrycode);
                txtAppZipApplicant.Text = ModelUIFormat.FormatZipShow(zip, countrycode, false);
                txtAppPOBox.Text = people.postOfficeBox;
                hdnContactSeqNumber.Value = people.contactSeqNumber;
                hdnRefContactSeqNumber.Value = refContactNumber;

                if (ACAConstant.ContactSectionPosition.SpearForm.Equals(ContactSectionPosition))
                {
                    radioListContactPermission.SetValue(accessLevel);
                }

                if (people.compactAddress != null)
                {
                    txtAppCity.Text = people.compactAddress.city;
                    txtAppState.Text = people.compactAddress.state;
                    txtAppStreetAdd1.Text = people.compactAddress.addressLine1;
                    txtAppStreetAdd2.Text = people.compactAddress.addressLine2;
                    txtAppStreetAdd3.Text = people.compactAddress.addressLine3;
                }
                else
                {
                    txtAppCity.Text = string.Empty;
                    txtAppState.Text = string.Empty;
                    txtAppStreetAdd1.Text = string.Empty;
                    txtAppStreetAdd2.Text = string.Empty;
                    txtAppStreetAdd3.Text = string.Empty;
                }

                if (IsShowContactType)
                {
                    ResetTemplate();
                }

                bool isReturn = false;

                // clear template when there is no contact type is specified
                if (string.IsNullOrEmpty(contactType))
                {
                    templateEdit.DisplayAttributes(null, TemplateControlIDPrefix);
                    isReturn = true;
                }

                if ((people.attributes == null || people.attributes.Length == 0) && !isReturn)
                {
                    CreateOriginalContactTemplate(contactType);
                    isReturn = true;
                }

                if (!isReturn)
                {
                    templateEdit.DisplayAttributes(people.attributes, TemplateControlIDPrefix);
                }

                bool disableEditForm = !IsEditable
                    || (IsValidate && !string.IsNullOrEmpty(refContactNumber))
                    || ACAConstant.ContactSectionPosition.RegisterAccountConfirm.Equals(ContactSectionPosition)
                    || ACAConstant.ContactSectionPosition.SpearFormCloseMatchConfirm.Equals(ContactSectionPosition)
                    || ACAConstant.ContactSectionPosition.RegisterAccountComplete.Equals(ContactSectionPosition)
                    || ACAConstant.ContactSectionPosition.RegisterClerkConfirm.Equals(ContactSectionPosition);

                hdnDisableStatus.Value = Convert.ToString(disableEditForm);

                // Hidden any other action link when enter register account confirm page.
                genericTemplate.IsReadOnly = disableEditForm;

                if (people.template == null)
                {
                    CreateGenericTemplate(contactType);
                }
                else
                {
                    HideGenericTemplate4Registration();

                    genericTemplate.Display(people.template);
                }

                // daily will disable or enable this edit form again.at PageLoaded js function
                if (disableEditForm)
                {
                    txtAppZipApplicant.SetZipFromAA(zip);
                    string[] filterControlIDs = ContactTypePermission 
                                                ? TemplateUtil.GetAlwaysEditableControlIDs(people.attributes, TemplateControlIDPrefix) 
                                                : null;

                    DisableContactEdit(filterControlIDs);
                }
                else
                {
                    string[] excludedControlIds = genericTemplate.ReadOnlyControlIds != null && genericTemplate.ReadOnlyControlIds.Count > 0
                                                ? genericTemplate.ReadOnlyControlIds.ToArray()
                                                : null;
                    EnableContactForm(excludedControlIds);
                }

                if (!string.IsNullOrEmpty(refContactNumber))
                {
                    //Cache the reference data.
                    RefEntityCache = people;
                }

                // Load the contact address list
                contactAddressList.Display(people.contactAddressList);

                if (string.IsNullOrEmpty(accessLevel))
                {
                    //Marked 'contacts' privilege in aca admin 'filter search result',set 'full access' as default value, otherwise set 'no access'.
                    SetDefaultValue4ContactPermission(contactType);
                }
            }
        }

        /// <summary>
        /// Set view id.
        /// </summary>
        /// <param name="viewId">The view ID</param>
        public void SetViewId(string viewId)
        {
            ViewId = viewId;
        }

        /// <summary>
        /// Indicating the form data whether is valid.
        /// </summary>
        /// <returns>True if the contact is valid</returns>
        public bool IsDataValid()
        {
            bool isValid = !IsEditable || !IsValidate || IsFormEmpty() || !string.IsNullOrEmpty(hdnRefContactSeqNumber.Value);
            return isValid;
        }

        /// <summary>
        /// Is pass contact address validate.
        /// </summary>
        /// <param name="contactAddresses">the contact address.</param>
        /// <returns>true or false.</returns>
        public bool IsPassContactAddressValidation(ContactAddressModel[] contactAddresses)
        {
            bool isPass = !(IsNeedValidateContactAddress() && !string.IsNullOrEmpty(ValidateContactAddress(contactAddresses)));

            return isPass;
        }

        /// <summary>
        /// Validate contact address information.
        /// </summary>
        /// <param name="contactAddresses">contact address array</param>
        /// <returns>error message if any.</returns>
        public string ValidateContactAddress(ContactAddressModel[] contactAddresses)
        {
            contactAddressList.ContactType = ContactType;

            return contactAddressList.Validate(contactAddresses, true, true);
        }

        /// <summary>
        /// Initialize the page's title.
        /// </summary>
        /// <param name="labelKey">The label key of the page's title</param>
        public void InitTitleBar(string labelKey)
        {
            AccelaDropDownList dropDownList = new AccelaDropDownList();
            dropDownList.ID = "contacttype";
            dropDownList.AutoPostBack = false;
            dropDownList.Attributes.Add("onfocus", "javascript:if(typeof(parent.GetOldValueBeforeValueChange)!='undefined'){parent.GetOldValueBeforeValueChange(this);}");
            dropDownList.Attributes.Add("onchange", "javascript:if(typeof(parent.ChangedItemsValidateBeforePostBack)!='undefined'){parent.ChangedItemsValidateBeforePostBack(this,window);}");
            InitContactTypeList(dropDownList);
            sectionTitleBar.AddToolBarControls(dropDownList);

            dropDownList.SelectedIndexChanged += (obj, args) =>
            {
                AccelaDropDownList senderControl = obj as AccelaDropDownList;

                if (senderControl != null)
                {
                    ContactType = senderControl.SelectedValue;
                }

                LoadContactProperties();
            };

            sectionTitleBar.Visible = true;
            sectionTitleBar.PermissionValueId = dropDownList.ClientID;
            sectionTitleBar.LabelKey = labelKey;
            sectionTitleBar.SectionID = ModuleName + ACAConstant.SPLIT_CHAR + ViewId + ACAConstant.SPLIT_CHAR + ClientID + "_";
        }

        /// <summary>
        /// Sync contact people.
        /// </summary>
        /// <returns>Contact parameters Model</returns>
        public ContactSessionParameter SyncContactPeopleToConfirmCloseMatch()
        {
            ContactSessionParameter parametersModel = AppSession.GetContactSessionParameter();
            PeopleModel4WS closeMatchPeople = ContactUtil.GetPeopleModelFromContactSessionParameter(parametersModel);
            PeopleModel4WS userInputPeople = PeopleUtil.GetInputPeopleForCloseMatchFromSession();

            if (rdoAcceptCloseMatchConfirm.Checked || rdoUpdateCloseMatchConfirm.Checked)
            {
                if (!ConditionsUtil.ShowCondition(ucConditon, closeMatchPeople))
                {
                    return null;
                }

                parametersModel.Process.ContactProcessType = ContactProcessType.SelectContactFromCloseMatch;

                if (rdoUpdateCloseMatchConfirm.Checked)
                {
                    UpdateCloseMatchPeopleModel(userInputPeople, closeMatchPeople);
                }
            }
            else if (rdoRejectCloseMatchConfirm.Checked)
            {
                if (parametersModel.Data.DataObject is CapContactModel4WS)
                {
                    CapContactModel4WS capContact = parametersModel.Data.DataObject as CapContactModel4WS;
                    capContact.people = userInputPeople;
                    capContact.refContactNumber = string.Empty;
                }
            }

            PeopleUtil.RemoveInputPeopleForCloseMatchFromSession();

            return parametersModel;
        }

        /// <summary>
        /// Set Close Match For spear form.
        /// </summary>
        public void SetSpearFormCloseMatch()
        {
            divAcceptCloseMatchConfirm.Visible = true;
            divUpdateCloseMatchConfirm.Visible = true;
            divRejectCloseMatchConfirm.Visible = true;

            if (AppSession.IsAdmin)
            {
                lblConfirmCloseMatchConfirmTitle.Visible = true;
            }
            else
            {
                rdoAcceptCloseMatchConfirm.InputAttributes.Add("onclick", "if (typeof(CloseMatchConfirmClick) != 'undefined') { CloseMatchConfirmClick(); }");
                rdoUpdateCloseMatchConfirm.InputAttributes.Add("onclick", "if (typeof(CloseMatchConfirmClick) != 'undefined') { CloseMatchConfirmClick(); }");
                rdoRejectCloseMatchConfirm.InputAttributes.Add("onclick", "if (typeof(CloseMatchConfirmClick) != 'undefined') { CloseMatchConfirmClick(); }");
            }
        }

        /// <summary>
        /// Raises the <see cref="E:System.Web.UI.Control.Init" /> event.
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs" /> object that contains the event data.</param>
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            genericTemplate.ASITUIDataKey = ClientID;
            DropDownListBindUtil.BindSalutation(ddlAppSalutation);
            DropDownListBindUtil.BindGender(radioListAppGender);
            DropDownListBindUtil.BindStandardChoise(ddlRace, BizDomainConstant.STD_CAT_RACE);
            DropDownListBindUtil.BindContactType4License(ddlContactTypeFlag);
            DropDownListBindUtil.BindPreferredChannel(ddlPreferredChannel, ModuleName);
            ddlAppCountry.BindItems();
            ddlBirthplaceCountry.BindItems();
            ddlAppCountry.SetCountryControls(txtAppZipApplicant, new AccelaStateControl[] { txtAppState, ddlDriverLicenseState }, txtAppPhone1, txtAppPhone2, txtAppPhone3, txtAppFax);
            ddlBirthplaceCountry.SetCountryControls(null, ddlBirthplaceState, null);

            if (!IsPostBack)
            {
                if (AppSession.IsAdmin)
                {
                    ddlContactType.AutoPostBack = false;

                    if (!IsForSearch)
                    {
                        ddlContactType.Attributes.Add("onfocus", "javascript:if(typeof(parent.GetOldValueBeforeValueChange)!='undefined'){parent.GetOldValueBeforeValueChange(this);}");
                        ddlContactType.Attributes.Add("onchange", "javascript:if(typeof(parent.ChangedItemsValidateBeforePostBack)!='undefined'){parent.ChangedItemsValidateBeforePostBack(this,window);}");   
                    }
                }
            }
        }

        /// <summary>
        /// Handle the contact type dropdownlist selected changed event to load the template field
        /// </summary>
        /// <param name="sender">the sender object</param>
        /// <param name="e">the event arguments</param>
        protected void ContactType_SelectedIndexChanged(object sender, EventArgs e)
        {
            ContactType = ddlContactType.SelectedValue;
            LoadContactProperties();
        }

        /// <summary>
        /// Handle the contact type flag dropdownlist selected changed event.
        /// </summary>
        /// <param name="sender">the sender object</param>
        /// <param name="e">the event arguments</param>
        protected void ContactTypeFlag_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ContactTypeFlagChangedEvent != null)
            {
                ContactTypeFlagChangedEvent(sender, new CommonEventArgs(ddlContactTypeFlag.SelectedValue));
            }
        }

        /// <summary>
        /// override PreRender event.
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs" /> object that contains the event data.</param>
        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);

            if (ComponentDataSource.Transactional.Equals(ValidateFlag)
                || (ComponentDataSource.NoLimitation.Equals(ValidateFlag) && !StandardChoiceUtil.AutoSyncPeople(ModuleName, PeopleType.Contact)))
            {
                txtAKAName.IsHidden = true;   
            }

            ControlBuildHelper.SetPropertyForStandardFields(ViewId, ModuleName, Permission, Controls, IsEditable);

            phContent.TemplateControlIDPrefix = TemplateControlIDPrefix;
            InitFormDesignerPlaceHolder(phContent);
            SetLockStandardFieldsFlag();
            InitAka();
        }

        /// <summary>
        /// Raises the page load event
        /// </summary>
        /// <param name="sender">An object that contains the event sender.</param>
        /// <param name="e">A System.EventArgs object containing the event data.</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsAppliedRegional)
            {
                ControlUtil.ApplyRegionalSetting(IsPostBack, IsForSearch, true, !IsPostBack && !IsForSearch, ddlAppCountry);
                ControlUtil.ApplyRegionalSetting(IsPostBack, IsForSearch, true, !IsPostBack && !IsForSearch, ddlBirthplaceCountry);

                if (!AppSession.IsAdmin && !IsPostBack && IsForNew)
                {
                    SetCurrentCityAndState();
                }

                IsAppliedRegional = true;
            }

            if (IsShowContactType && IsPostBack && (ACAConstant.ContactSectionPosition.RegisterAccountConfirm.Equals(ContactSectionPosition) ||
                ACAConstant.ContactSectionPosition.RegisterClerkConfirm.Equals(ContactSectionPosition)))
            {
                //Re-bind contact type drop down to get the latest choices in admin. 
                ContactType = Request.Form[ddlContactType.UniqueID];
            }
            
            InitCountry();

            if (!AppSession.IsAdmin)
            {
                ddlContactTypeFlag.Attributes.Add("onchange", ddlContactTypeFlag.ClientID + "_onchange(true);");
                ddlContactType.Attributes.Add("onchange", ddlContactType.ClientID + "_onchange();");

                if (!IsPostBack)
                {
                    if (IsNeedSelectedLicenseCertification)
                    {
                        ddlContactTypeFlag.AutoPostBack = true;
                    }

                    if (IsForSearch)
                    {
                        ddlContactType.AutoPostBack = false;
                        ClearContactForm(false);
                        SetCurrentCityAndState();
                    }
                    else
                    {
                        /*
                         * Search form will hide the contact address list no matter if the contact address enabled.
                         * So just consider the conatct address switch for edit form.
                         */
                        divContactAddressList.Visible = _isContactAddressEnabled;
                    }
                }

                if (ACAConstant.ContactSectionPosition.ModifyReferenceContact.Equals(ContactSectionPosition) && !ContactTypePermission)
                {
                    genericTemplate.IsReadOnly = true;
                    contactAddressList.DisableEditForm();
                }
            }
            else
            {
                contactAddressList.Display(null);
                ucConditon.HideCondition();
            }
        }

        /// <summary>
        /// Rebind account contact list.
        /// </summary>
        /// <param name="sender">An object that contains the event sender.</param>
        /// <param name="e">An System.EventArgs object that contains the event data.</param>
        protected void RefreshAddressListButton_Click(object sender, EventArgs e)
        {
            List<ContactAddressModel> addressList = new List<ContactAddressModel>();
            PeopleModel4WS people = ContactUtil.GetPeopleModelFromContactSessionParameter();

            if (people != null)
            {
                addressList = people.contactAddressList.ToList();
            }

            Display(addressList, true);

            ActionType actionType = hfIsForNewContactAddress.Value == "1" ? ActionType.AddSuccess : ActionType.UpdateSuccess;
            contactAddressList.ShowActionNoticeMessage(actionType);
            Page.FocusElement(string.Format("{0}_lnkAddContactAddress", ClientID));
        }

        /// <summary>
        /// Handle the ContactAddressSelected event, populate contact information into contact address edit form.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="arg">Event argument.</param>
        protected void ContactAddressList_ContactAddressSelected(object sender, CommonEventArgs arg)
        {
            //PeopleUtil.SaveTempContactAddresses(ContactSeqNumber, contactAddressList.DataSource == null ? null : contactAddressList.DataSource.ToArray());
        }

        /// <summary>
        /// Handle the ContactAddressSelected event, populate contact information into contact address edit form.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="arg">Event argument.</param>
        protected void ContactAddressList_ContactAddressDeactivate(object sender, CommonEventArgs arg)
        {
            //PeopleUtil.SaveTempContactAddresses(ContactSeqNumber, contactAddressList.DataSource == null ? null : contactAddressList.DataSource.ToArray());
        }

        /// <summary>
        /// Handle the DataSourceChanged event, to control the contact edit form.
        /// </summary>
        /// <param name="sender">Sender object.</param>
        /// <param name="arg">Event argument.</param>
        protected void ContactAddressList_DataSourceChanged(object sender, CommonEventArgs arg)
        {
            ContactAddressModel[] addressList = contactAddressList.DataSource == null
                                                ? null
                                                : contactAddressList.DataSource.ToArray();
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

           //Generic templateEdit Part
            CapModel4WS capModel = AppSession.GetCapModelFromSession(moduleName);
            var templateModel = genericTemplate.GetTemplateModel(false);

            /*
             * For expression running, if there is contact type field and the view element ddlContactType had removed by fix bug #52903. 
             * The contact type field can not be found in such gviews (12,121,118,123,124,125). So expression can not execute correctly.
             * So, add the control of contact type field for the gviews into expression controls.
             */
            string[] expFields = new[] { "contactsModel*contactType", "contactsModel1*contactType", "contactsModel2*contactType", "contactType", "applicant*contactType" };

            foreach (string expField in expFields)
            {
                string ctlId = ExpressionUtil.GetFullControlFieldName(capModel, expField);
                expressionControls.Add(ctlId, ddlContactType);
            }

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

                StringBuilder fieldName = new StringBuilder(ContactExpressionType.ToString());
                fieldName.Append(ACAConstant.SPLIT_CHAR5);
                fieldName.Append(ExpressionUtil.FilterSpciefCharForControlName(Server.UrlEncode(field.subgroupName.ToUpper())));
                fieldName.Append(ACAConstant.SPLIT_CHAR5);
                fieldName.Append(ExpressionUtil.FilterSpciefCharForControlName(Server.UrlEncode(field.fieldName)));
                var expCtlKey4Template = ExpressionUtil.GetFullControlFieldName(capModel, fieldName.ToString());

                if (!expressionControls.ContainsKey(expCtlKey4Template))
                {
                    expressionControls.Add(expCtlKey4Template, control);
                }
            }

            return expressionControls;
        }

        /// <summary>
        /// Disables the contact edit.
        /// </summary>
        /// <param name="filterControlIDs">Some control IDs need to keep always editable.</param>
        private void DisableContactEdit(string[] filterControlIDs)
        {
            if (ContactSectionPosition == ACAConstant.ContactSectionPosition.RegisterAccountConfirm
                || ContactSectionPosition == ACAConstant.ContactSectionPosition.SpearFormCloseMatchConfirm
                || ContactSectionPosition == ACAConstant.ContactSectionPosition.RegisterClerkConfirm)
            {
                DisableAllEdit(this);
            }
            else
            {
                DisableEdit(this, filterControlIDs);
            }
        }

        /// <summary>
        /// Set full access and no access permission according to 'Filter search result' in ACA admin.
        /// </summary>
        /// <param name="contactType">Type of the contact.</param>
        private void SetDefaultValue4ContactPermission(string contactType)
        {
            if (!AppSession.IsAdmin && ACAConstant.ContactSectionPosition.SpearForm.Equals(ContactSectionPosition))
            {
                string capContactPermission = GetDefaultContactPermisssion(contactType);
                radioListContactPermission.SetValue(capContactPermission);
            }
        }

        /// <summary>
        /// Initialize contact type list.
        /// </summary>
        /// <param name="dropDownList">AccelaDropDownList control to be Initialized.</param>
        private void InitContactTypeList(AccelaDropDownList dropDownList)
        {
            ContactSessionParameter parametersModel = AppSession.GetContactSessionParameter();

            switch (ContactSectionPosition)
            {
                case ACAConstant.ContactSectionPosition.RegisterAccount:
                case ACAConstant.ContactSectionPosition.RegisterAccountConfirm:
                case ACAConstant.ContactSectionPosition.AddReferenceContact:
                case ACAConstant.ContactSectionPosition.ModifyReferenceContact:
                    dropDownList.SourceType = DropDownListDataSourceType.Database;
                    break;
                case ACAConstant.ContactSectionPosition.RegisterClerk:
                case ACAConstant.ContactSectionPosition.RegisterClerkConfirm:
                case ACAConstant.ContactSectionPosition.EditClerk:
                    break;
                default:
                    if (AppSession.IsAdmin && !IsForSearch)
                    {
                        dropDownList.SourceType = DropDownListDataSourceType.STDandXPolicy;
                    }

                    break;
            }

            if (ContactSectionPosition == ACAConstant.ContactSectionPosition.SpearForm && IsForSearch)
            {
                DropDownListBindUtil.BindReferenceContactTypeFilterBySearchSetting(dropDownList);
            }
            else if (ContactSectionPosition == ACAConstant.ContactSectionPosition.SpearForm || ContactSectionPosition == ACAConstant.ContactSectionPosition.SpearFormCloseMatchConfirm)
            {
                DropDownListBindUtil.BindContactTypeWithPageFlow(
                    dropDownList,
                    ModuleName,
                    IsMultipleContact,
                    parametersModel != null ? parametersModel.PageFlowComponent.ComponentName : null);
            }
            else
            {
                DropDownListBindUtil.BindContactType(dropDownList, ContactTypeSource.Reference);
            }
        }

        /// <summary>
        /// If all contact's fields is empty then return true.
        /// </summary>
        /// <returns>isAllEmpty true or false</returns>
        private bool IsFormEmpty()
        {
            bool isAllEmpty = false;
            bool isContactTypeEmpty = ddlContactType.IsHidden || string.IsNullOrEmpty(ddlContactType.SelectedValue);
            bool isContactPermissionEmpty = !radioListContactPermission.Visible || string.IsNullOrEmpty(radioListContactPermission.GetValue());

            if (isContactTypeEmpty
                && ddlContactTypeFlag.SelectedValue == string.Empty
                && ddlAppSalutation.SelectedValue == string.Empty 
                && txtTitle.Text.Trim() == string.Empty 
                && txtAppFirstName.Text.Trim() == string.Empty
                && txtAppMiddleName.Text.Trim() == string.Empty
                && txtAppLastName.Text.Trim() == string.Empty
                && txtAppFullName.Text.Trim() == string.Empty
                && txtAppBirthDate.Text.Trim() == string.Empty
                && radioListAppGender.SelectedValue == string.Empty
                && txtAppOrganizationName.Text.Trim() == string.Empty
                && txtBusinessName2.Text.Trim() == string.Empty
                && txtAppCity.Text.Trim() == string.Empty
                && txtAppState.Text.Trim() == string.Empty
                && txtAppZipApplicant.GetZip(ControlUtil.GetControlValue(ddlAppCountry)) == string.Empty
                && txtAppTradeName.Text.Trim() == string.Empty
                && txtSSN.Text.Trim() == string.Empty
                && txtAppFein.Text.Trim() == string.Empty
                && ControlUtil.GetControlValue(ddlAppCountry) == string.Empty
                && txtAppPOBox.Text.Trim() == string.Empty
                && txtAppPhone1.Text.Trim() == string.Empty
                && txtAppPhone3.Text.Trim() == string.Empty
                && txtAppPhone2.Text.Trim() == string.Empty
                && txtAppFax.Text.Trim() == string.Empty
                && txtAppEmail.Text.Trim() == string.Empty
                && isContactPermissionEmpty
                && txtAppStreetAdd1.Text.Trim() == string.Empty
                && txtAppStreetAdd2.Text.Trim() == string.Empty
                && txtAppStreetAdd3.Text.Trim() == string.Empty
                && txtAppSuffix.Text.Trim() == string.Empty
                && txtBirthplaceCity.Text.Trim() == string.Empty
                && txtStateNumber.Text.Trim() == string.Empty
                && txtDeceasedDate.Text.Trim() == string.Empty
                && txtPassportNumber.Text.Trim() == string.Empty
                && txtDriverLicenseNumber.Text.Trim() == string.Empty
                && ddlRace.SelectedValue == string.Empty
                && ddlBirthplaceState.Text == string.Empty
                && ControlUtil.GetControlValue(ddlBirthplaceCountry) == string.Empty
                && ddlDriverLicenseState.Text == string.Empty
                && templateEdit.IsControlsValueEmpty())
            {
                isAllEmpty = true;
            }

            return isAllEmpty;
        }

        /// <summary>
        /// Enable contact Form
        /// </summary>
        /// <param name="filterControlIDs">The filter control i ds.</param>
        private void EnableContactForm(string[] filterControlIDs = null)
        {
            //if the contact section editable value is false, not to clean the form disable state.
            if (IsEditable)
            {
                EnableEdit(this, filterControlIDs);
            }
        }

        /// <summary>
        /// Initial AKA control.
        /// </summary>
        private void InitAka()
        {
            var gviewBll = (IGviewBll)ObjectFactory.GetObject(typeof(IGviewBll));
            var models = ModelUIFormat.GetSimpleViewElementModelBySectionID(ModuleName, Permission, ViewId);
            txtAKAName.IsFirstNameHidden = !gviewBll.IsFieldVisible(models, "txtAppFirstName");
            txtAKAName.IsMiddleNameHidden = !gviewBll.IsFieldVisible(models, "txtAppMiddleName");
            txtAKAName.IsLastNameHidden = !gviewBll.IsFieldVisible(models, "txtAppLastName");
            txtAKAName.IsFullNameHidden = !gviewBll.IsFieldVisible(models, "txtAppFullName");
        }

        /// <summary>
        /// Set lock standard fields flag.
        /// </summary>
        private void SetLockStandardFieldsFlag()
        {
            if (ComponentDataSource.NoLimitation.Equals(ValidateFlag) && StandardChoiceUtil.AutoSyncPeople(ModuleName, PeopleType.Contact))
            {
                hfLockStandardFileds.Value = ACAConstant.COMMON_Y;
                bool isAsynPostBack = ScriptManager.GetCurrent(Page).IsInAsyncPostBack;

                if (!string.IsNullOrEmpty(hdnRefContactSeqNumber.Value) && !isAsynPostBack)
                {
                    //Contact data is come from reference and is not postback.
                    string scripts = ClientID + "_LockStandardFields();";
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), ClientID + "_LockStandardFields", scripts, true);
                }
            }
        }

        /// <summary>
        /// Hides the generic template for registration.
        /// </summary>
        private void HideGenericTemplate4Registration()
        {
            bool isHideGTemplate = !PeopleUtil.IsVisibleGenericTemplate(ContactSectionPosition);
            genericTemplate.IsHideGenericTemplate = isHideGTemplate;
            genericTemplate.IsHideTemplateTable = isHideGTemplate;
        }

        /// <summary>
        /// Use the input people to update the close match people.
        /// The input people contact type is the same as the close match people contact type.
        /// </summary>
        /// <param name="inputPeople">the input people.</param>
        /// <param name="closeMatchPeople">the close match people.</param>
        private void UpdateCloseMatchPeopleModel(PeopleModel4WS inputPeople, PeopleModel4WS closeMatchPeople)
        {
            UpdateCloseMatchStandardFields(inputPeople, closeMatchPeople);
            UpdateCloseMatchTemplateFields(inputPeople, closeMatchPeople);
        }

        /// <summary>
        /// Use the input people to update the close match people standard fields.
        /// The input people model contact type is the same as the close people model contact type.
        /// </summary>
        /// <param name="inputPeople">the input people.</param>
        /// <param name="closeMatchPeople">the close match people.</param>
        private void UpdateCloseMatchStandardFields(PeopleModel4WS inputPeople, PeopleModel4WS closeMatchPeople)
        {
            //Stardand Fields
            UpdateIndividualOrganizationFields(inputPeople, closeMatchPeople);

            closeMatchPeople.email = txtAppEmail.Visible ? inputPeople.email : closeMatchPeople.email;
            closeMatchPeople.businessName2 = txtBusinessName2.Visible ? inputPeople.businessName2 : closeMatchPeople.businessName2;
            closeMatchPeople.contactTypeFlag = ddlContactTypeFlag.Visible ? inputPeople.contactTypeFlag : closeMatchPeople.contactTypeFlag;
            closeMatchPeople.namesuffix = txtAppSuffix.Visible ? inputPeople.namesuffix : closeMatchPeople.namesuffix;
            closeMatchPeople.preferredChannel = ddlPreferredChannel.Visible ? inputPeople.preferredChannel : closeMatchPeople.preferredChannel;
            closeMatchPeople.comment = txtNotes.Visible ? inputPeople.comment : closeMatchPeople.comment;
            closeMatchPeople.postOfficeBox = txtAppPOBox.Visible ? inputPeople.postOfficeBox : closeMatchPeople.postOfficeBox;

            PeopleUtil.SetContactAddressEntityID(inputPeople.contactAddressList, closeMatchPeople.contactSeqNumber);
            closeMatchPeople.contactAddressList = inputPeople.contactAddressList;

            closeMatchPeople.countryCode = inputPeople.countryCode;
            closeMatchPeople.serviceProviderCode = inputPeople.serviceProviderCode;
            closeMatchPeople.auditStatus = ACAConstant.VALID_STATUS;

            if (txtAppPhone1.Visible)
            {
                closeMatchPeople.phone1 = inputPeople.phone1;
                closeMatchPeople.phone1CountryCode = inputPeople.phone1CountryCode;
            }

            if (txtAppPhone2.Visible)
            {
                closeMatchPeople.phone2 = inputPeople.phone2;
                closeMatchPeople.phone2CountryCode = inputPeople.phone2CountryCode;
            }

            if (txtAppPhone3.Visible)
            {
                closeMatchPeople.phone3 = inputPeople.phone3;
                closeMatchPeople.phone3CountryCode = inputPeople.phone3CountryCode;
            }

            if (txtAppFax.Visible)
            {
                closeMatchPeople.fax = inputPeople.fax;
                closeMatchPeople.faxCountryCode = inputPeople.faxCountryCode;
            }

            if (ACAConstant.ContactSectionPosition.SpearFormCloseMatchConfirm.Equals(ContactSectionPosition))
            {
                closeMatchPeople.auditID = AppSession.User.PublicUserId;
            }

            if (inputPeople.compactAddress != null && (txtAppStreetAdd1.Visible || txtAppStreetAdd2.Visible || txtAppStreetAdd3.Visible
                || txtAppCity.Visible || txtAppState.Visible || txtAppZipApplicant.Visible || ddlAppCountry.Visible))
            {
                if (closeMatchPeople.compactAddress == null)
                {
                    closeMatchPeople.compactAddress = new CompactAddressModel4WS();
                }

                closeMatchPeople.compactAddress.addressLine1 = txtAppStreetAdd1.Visible ? inputPeople.compactAddress.addressLine1 : closeMatchPeople.compactAddress.addressLine1;
                closeMatchPeople.compactAddress.addressLine2 = txtAppStreetAdd2.Visible ? inputPeople.compactAddress.addressLine2 : closeMatchPeople.compactAddress.addressLine2;
                closeMatchPeople.compactAddress.addressLine3 = txtAppStreetAdd3.Visible ? inputPeople.compactAddress.addressLine3 : closeMatchPeople.compactAddress.addressLine3;
                closeMatchPeople.compactAddress.city = txtAppCity.Visible ? inputPeople.compactAddress.city : closeMatchPeople.compactAddress.city;
                closeMatchPeople.compactAddress.zip = txtAppZipApplicant.Visible ? inputPeople.compactAddress.zip : closeMatchPeople.compactAddress.zip;

                if (txtAppState.Visible)
                {
                    closeMatchPeople.compactAddress.state = inputPeople.compactAddress.state;
                    closeMatchPeople.compactAddress.resState = inputPeople.compactAddress.resState;
                }

                closeMatchPeople.compactAddress.countryCode = inputPeople.compactAddress.countryCode;
            }
        }

        /// <summary>
        /// Use the input people to update the close match people standard INDIVIDUAL/ORGANIZATION fields.
        /// The input people model contact type is the same as the close people model contact type.
        /// </summary>
        /// <param name="inputPeople">the input people.</param>
        /// <param name="closeMatchPeople">the close match people.</param>
        private void UpdateIndividualOrganizationFields(PeopleModel4WS inputPeople, PeopleModel4WS closeMatchPeople)
        {
            string inputContactTypeFlag = ddlContactTypeFlag.Visible ? ddlContactTypeFlag.SelectedValue : string.Empty;

            if (string.Equals(inputContactTypeFlag, INDIVIDUAL, StringComparison.OrdinalIgnoreCase))
            {
                closeMatchPeople.businessName = string.Empty;
                closeMatchPeople.tradeName = string.Empty;
                closeMatchPeople.fein = string.Empty;
            }
            else
            {
                closeMatchPeople.businessName = txtAppOrganizationName.Visible ? inputPeople.businessName : closeMatchPeople.businessName;
                closeMatchPeople.tradeName = txtAppTradeName.Visible ? inputPeople.tradeName : closeMatchPeople.tradeName;
                closeMatchPeople.fein = txtAppFein.Visible ? inputPeople.fein : closeMatchPeople.fein;
            }

            if (string.Equals(inputContactTypeFlag, ORGANIZATION, StringComparison.OrdinalIgnoreCase))
            {
                closeMatchPeople.firstName = string.Empty;
                closeMatchPeople.lastName = string.Empty;
                closeMatchPeople.fullName = string.Empty;
                closeMatchPeople.middleName = string.Empty;
                closeMatchPeople.socialSecurityNumber = string.Empty;
                closeMatchPeople.maskedSsn = string.Empty;
                closeMatchPeople.title = string.Empty;
                closeMatchPeople.salutation = string.Empty;
                closeMatchPeople.gender = string.Empty;
                closeMatchPeople.birthDate = null;
                closeMatchPeople.birthCity = string.Empty;
                closeMatchPeople.birthState = string.Empty;
                closeMatchPeople.birthRegion = string.Empty;
                closeMatchPeople.race = string.Empty;
                closeMatchPeople.deceasedDate = null;
                closeMatchPeople.passportNumber = string.Empty;
                closeMatchPeople.driverLicenseNbr = string.Empty;
                closeMatchPeople.driverLicenseState = string.Empty;
                closeMatchPeople.stateIDNbr = string.Empty;

                if (ACAConstant.ContactSectionPosition.SpearFormCloseMatchConfirm.Equals(ContactSectionPosition))
                {
                    closeMatchPeople.peopleAKAList = null;
                }
            }
            else
            {
                closeMatchPeople.firstName = txtAppFirstName.Visible ? inputPeople.firstName : closeMatchPeople.firstName;
                closeMatchPeople.lastName = txtAppLastName.Visible ? inputPeople.lastName : closeMatchPeople.lastName;
                closeMatchPeople.fullName = txtAppFullName.Visible ? inputPeople.fullName : closeMatchPeople.fullName;
                closeMatchPeople.middleName = txtAppMiddleName.Visible ? inputPeople.middleName : closeMatchPeople.middleName;
                closeMatchPeople.socialSecurityNumber = txtSSN.Visible ? inputPeople.socialSecurityNumber : closeMatchPeople.socialSecurityNumber;
                closeMatchPeople.maskedSsn = closeMatchPeople.socialSecurityNumber;
                closeMatchPeople.title = txtTitle.Visible ? inputPeople.title : closeMatchPeople.title;
                closeMatchPeople.salutation = ddlAppSalutation.Visible ? inputPeople.salutation : closeMatchPeople.salutation;
                closeMatchPeople.gender = radioListAppGender.Visible ? inputPeople.gender : closeMatchPeople.gender;
                closeMatchPeople.birthDate = txtAppBirthDate.Visible ? inputPeople.birthDate : closeMatchPeople.birthDate;
                closeMatchPeople.birthCity = txtBirthplaceCity.Visible ? inputPeople.birthCity : closeMatchPeople.birthCity;
                closeMatchPeople.birthState = ddlBirthplaceState.Visible ? inputPeople.birthState : closeMatchPeople.birthState;
                closeMatchPeople.birthRegion = inputPeople.birthRegion;
                closeMatchPeople.race = ddlRace.Visible ? inputPeople.race : closeMatchPeople.race;
                closeMatchPeople.deceasedDate = txtDeceasedDate.Visible ? inputPeople.deceasedDate : closeMatchPeople.deceasedDate;
                closeMatchPeople.passportNumber = txtPassportNumber.Visible ? inputPeople.passportNumber : closeMatchPeople.passportNumber;
                closeMatchPeople.driverLicenseNbr = txtDriverLicenseNumber.Visible ? inputPeople.driverLicenseNbr : closeMatchPeople.driverLicenseNbr;
                closeMatchPeople.driverLicenseState = ddlDriverLicenseState.Visible ? inputPeople.driverLicenseState : closeMatchPeople.driverLicenseState;
                closeMatchPeople.stateIDNbr = txtStateNumber.Visible ? inputPeople.stateIDNbr : closeMatchPeople.stateIDNbr;

                if (ACAConstant.ContactSectionPosition.SpearFormCloseMatchConfirm.Equals(ContactSectionPosition))
                {
                    if (txtAKAName.Visible)
                    {
                        if (inputPeople.peopleAKAList != null)
                        {
                            foreach (PeopleAKAModel akaModel in inputPeople.peopleAKAList)
                            {
                                akaModel.contactNumber = long.Parse(closeMatchPeople.contactSeqNumber, CultureInfo.InvariantCulture);
                            }
                        }

                        closeMatchPeople.peopleAKAList = inputPeople.peopleAKAList;
                    }
                }
            }
        }

        /// <summary>
        /// Use the input people model to update the close people template fields.
        /// The input people model contact type is the same as the close people model contact type.
        /// </summary>
        /// <param name="inputPeople">the input people.</param>
        /// <param name="closeMatchPeople">the close match people.</param>
        private void UpdateCloseMatchTemplateFields(PeopleModel4WS inputPeople, PeopleModel4WS closeMatchPeople)
        {
            //People template Fields
            if (closeMatchPeople.attributes != null || inputPeople.attributes != null)
            {
                foreach (TemplateAttributeModel field in closeMatchPeople.attributes)
                {
                    // Finds web control by control id
                    WebControl control = templateEdit.FindControl(TemplateUtil.GetTemplateControlID(field.attributeName, TemplateControlIDPrefix)) as WebControl;

                    if (control == null || !(control is IAccelaControl) || !control.Visible)
                    {
                        continue;
                    }

                    TemplateAttributeModel inputAttributeModel = inputPeople.attributes.FirstOrDefault(f => string.Equals(field.attributeName, f.attributeName)
                                                                                                        && field.attributeValueDataType == f.attributeValueDataType);

                    if (inputAttributeModel == null)
                    {
                        continue;
                    }

                    field.attributeValue = inputAttributeModel.attributeValue;
                }
            }

            PeopleUtil.SetPeopleTemplateContactSeqNum(closeMatchPeople);

            //Generic template form
            var sFields = GenericTemplateUtil.GetAllFields(inputPeople.template);
            var tFields = GenericTemplateUtil.GetAllFields(closeMatchPeople.template);

            if (tFields != null && sFields != null)
            {
                foreach (var tFld in tFields)
                {
                    string controlID = ControlBuildHelper.GetGenericTemplateControlID(tFld);
                    var control = genericTemplate.FindControl(controlID) as WebControl;

                    if (control == null || !control.Visible)
                    {
                        continue;
                    }

                    var sFld = sFields.FirstOrDefault(f => string.Equals(tFld.fieldName, f.fieldName, StringComparison.OrdinalIgnoreCase)
                                                        && tFld.fieldType == f.fieldType);

                    if (sFld != null)
                    {
                        tFld.defaultValue = sFld.defaultValue;
                    }
                }
            }

            //Generic template table
            if (closeMatchPeople.template != null && closeMatchPeople.template.templateTables != null
                && inputPeople.template != null && inputPeople.template.templateTables != null)
            {
                TemplateSubgroup[] inputTables = CapUtil.GetGenericTemplateTableSubGroups(ModuleName, inputPeople.template.templateTables);
                TemplateSubgroup[] closeMatchTables = CapUtil.GetGenericTemplateTables(ModuleName, closeMatchPeople.template.templateTables, false);
                GenericTemplateUtil.MergeGenericTemplateTable(inputTables, closeMatchTables);
            }
        }

        #endregion Methods
    }
}