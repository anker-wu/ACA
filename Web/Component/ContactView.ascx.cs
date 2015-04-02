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
 *      $Id: ContactView.ascx.cs 277741 2014-08-20 10:31:02Z ACHIEVO\james.shi $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using System;
using Accela.ACA.BLL.Common;
using Accela.ACA.Common;
using Accela.ACA.Common.Common;
using Accela.ACA.Common.Util;
using Accela.ACA.Web.Common;
using Accela.ACA.Web.Common.Control;
using Accela.ACA.WSProxy;

namespace Accela.ACA.Web.Component
{
    /// <summary>
    /// the class for ContactView.
    /// </summary>
    public partial class ContactView : BaseUserControl
    {
        #region Properties

        /// <summary>
        /// Gets the label key of contact view pattern
        /// </summary>
        public string ContactViewPatternLabelKey
        {
            get
            {
                string labelkey = string.Empty;

                switch (ContactSectionPosition)
                {
                    case ACAConstant.ContactSectionPosition.RegisterAccount:
                    case ACAConstant.ContactSectionPosition.RegisterExistingAccount:
                        {
                            labelkey = "aca_contactview_label_patternregisteraccount";
                            break;
                        }

                    case ACAConstant.ContactSectionPosition.RegisterClerk:
                    case ACAConstant.ContactSectionPosition.EditClerk:
                        {
                            labelkey = "aca_contactview_label_patternclerk";
                            break;
                        }

                    case ACAConstant.ContactSectionPosition.SpearForm:
                        {
                            labelkey = "aca_contactview_label_pattern";
                            break;
                        }
                }

                return labelkey;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this control is displayed in the confirm page.
        /// </summary>
        public bool IsInConfirmPage
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the contact section position.
        /// </summary>
        public ACAConstant.ContactSectionPosition ContactSectionPosition
        {
            get;
            set;
        }

        /// <summary>
        /// Gets a value indicating whether to show the access level.
        /// </summary>
        protected bool ShowAccessLevel
        {
            get
            {
                // Access Level field should not displayed in Enter Information/Clerk Information/Clerk Registration Summary page
                return ContactSectionPosition != ACAConstant.ContactSectionPosition.RegisterAccount
                       && ContactSectionPosition != ACAConstant.ContactSectionPosition.EditClerk
                       && ContactSectionPosition != ACAConstant.ContactSectionPosition.RegisterClerk;
            }
        }

        /// <summary>
        /// Gets the view id.
        /// </summary>
        private string ViewId
        {
            get
            {
                return IsInConfirmPage ? GviewID.ContactEdit : PeopleUtil.GetContactGViewID(ContactSectionPosition);
            }
        }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Displays capContactModel information
        /// </summary>
        /// <param name="capContactModel">a CapContactModel4WS object.</param>
        public void Display(CapContactModel4WS capContactModel)
        {
            if (capContactModel == null)
            {
                return;
            }

            Display(capContactModel.people, capContactModel.accessLevel);
        }

        /// <summary>
        /// Display the contact info
        /// </summary>
        /// <param name="people">A cap contact model.</param>
        /// <param name="accessLevel">the contact access level</param>
        public void Display(PeopleModel4WS people, string accessLevel)
        {
            if (people == null)
            {
                return;
            }

            if (IsInConfirmPage)
            {
                lblContractBasic.Text = ModelUIFormat.FormatCapContactModel4Basic(people, ModuleName, true);
                lblContractExt.Text = ModelUIFormat.FormatCapContactModel4Ext(people, ModuleName, accessLevel);

                DisplayTemplate(people);
            }
            else
            {
                string contactViewLabelKey = LabelUtil.GetTextByKey(ContactViewPatternLabelKey, ModuleName);
                lblContactViewInfo.Text = ReplacePattern(contactViewLabelKey, people, accessLevel);
            }
        }

        /// <summary>
        /// Raises the page load event
        /// </summary>
        /// <param name="sender">An object that contains the event sender.</param>
        /// <param name="e">A System.EventArgs object containing the event data.</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsInConfirmPage)
            {
                divContactInfo.Visible = true;
            }
            else
            {
                divContactViewInfo.Visible = true;
            }

            if (!IsPostBack)
            {
                if (AppSession.IsAdmin && !IsInConfirmPage)
                {
                    lblContactViewPattern.LabelKey = ContactViewPatternLabelKey;
                    divContactView.Visible = true;
                }
            }
        }

        /// <summary>
        /// Display the people template and the generic template.
        /// </summary>
        /// <param name="people">The displayed people.</param>
        private void DisplayTemplate(PeopleModel4WS people)
        {
            if (people != null)
            {
                pnlTemplate.Visible = true;
                this.templateView.DisplayAttributes(people.attributes);
                this.genericTemplate.Display(people.template);
            }
            else
            {
                this.templateView.DisplayAttributes(null);
                this.genericTemplate.Display(null);
            }
        }

        /// <summary>
        /// Replace the pattern with the contactModel data
        /// </summary>
        /// <param name="pattern">the Pattern</param>
        /// <param name="people">CapContactModel4WS data</param>
        /// <param name="accessLevel">the contact access level</param>
        /// <returns>the replaced pattern data</returns>
        private string ReplacePattern(string pattern, PeopleModel4WS people, string accessLevel)
        {
            if (string.IsNullOrEmpty(pattern) || people == null)
            {
                return string.Empty;
            }

            GFilterScreenPermissionModel4WS permission = ControlBuildHelper.GetPermissionWithGenericTemplate(ViewId, GViewConstant.PERMISSION_PEOPLE, people.contactType, people.template);

            SimpleViewElementModel4WS[] models = ModelUIFormat.GetSimpleViewElementModelBySectionID(ModuleName, permission, ViewId);
            IGviewBll gviewBll = ObjectFactory.GetObject<IGviewBll>();

            //Available variable
            string contactTypeText = string.Empty;
            string contactTypeFlag = string.Empty;
            string salutation = string.Empty;
            string title = string.Empty;
            string firstName = string.Empty;
            string middleName = string.Empty;
            string lastName = string.Empty;
            string fullName = string.Empty;
            string namesuffix = string.Empty;
            string birthDate = string.Empty;
            string gender = string.Empty;
            string businessName = string.Empty;
            string businessName2 = string.Empty;
            string tradeName = string.Empty;
            string socialSecurityNumber = string.Empty;
            string fein = string.Empty;
            string addressLine1 = string.Empty;
            string addressLine2 = string.Empty;
            string addressLine3 = string.Empty;
            string city = string.Empty;
            string state = string.Empty;
            string zip = string.Empty;
            string country = string.Empty;
            string postOfficeBox = string.Empty;
            string phone1 = string.Empty;
            string phone2 = string.Empty;
            string phone3 = string.Empty;
            string fax = string.Empty;
            string email = string.Empty;
            string accessText = string.Empty;
            string birthplaceCity = string.Empty;
            string birthplaceState = string.Empty;
            string birthplaceCountry = string.Empty;
            string race = string.Empty;
            string deceasedDate = string.Empty;
            string passportNumber = string.Empty;
            string driverLicenseNumber = string.Empty;
            string driverLicenseState = string.Empty;
            string stateNumber = string.Empty;
            string preferredChannel = string.Empty;
            string notes = string.Empty;

            //Basic contact fields
            if (!string.IsNullOrEmpty(people.contactType))
            {
                contactTypeText = StandardChoiceUtil.GetContactTypeByKey(people.contactType);
            }

            if (!string.IsNullOrEmpty(people.contactTypeFlag))
            {
                contactTypeFlag = ScriptFilter.FilterScript(DropDownListBindUtil.GetTypeFlagTextByValue(people.contactTypeFlag));
            }

            if (!string.IsNullOrEmpty(people.salutation) && !I18nCultureUtil.IsChineseCultureEnabled && gviewBll.IsFieldVisible(models, "ddlAppSalutation"))
            {
                salutation = ScriptFilter.FilterScript(StandardChoiceUtil.GetStandardChoiceValueByKey(BizDomainConstant.STD_CAT_SALUTATION, people.salutation));
            }

            if (!string.IsNullOrEmpty(people.title) && gviewBll.IsFieldVisible(models, "txtTitle"))
            {
                title = ScriptFilter.FilterScript(people.title);
            }

            if (!string.IsNullOrEmpty(people.firstName) && gviewBll.IsFieldVisible(models, "txtAppFirstName"))
            {
                firstName = ScriptFilter.FilterScript(people.firstName);
            }

            if (!string.IsNullOrEmpty(people.middleName) && gviewBll.IsFieldVisible(models, "txtAppMiddleName"))
            {
                middleName = ScriptFilter.FilterScript(people.middleName);
            }

            if (!string.IsNullOrEmpty(people.lastName) && gviewBll.IsFieldVisible(models, "txtAppLastName"))
            {
                lastName = ScriptFilter.FilterScript(people.lastName);
            }

            if (!string.IsNullOrEmpty(people.fullName) && gviewBll.IsFieldVisible(models, "txtAppFullName"))
            {
                fullName = ScriptFilter.FilterScript(people.fullName);
            }

            if (!string.IsNullOrEmpty(people.namesuffix) && gviewBll.IsFieldVisible(models, "txtAppSuffix"))
            {
                namesuffix = ScriptFilter.FilterScript(people.namesuffix);
            }

            if (people.birthDate != null && gviewBll.IsFieldVisible(models, "txtAppBirthDate"))
            {
                birthDate = ScriptFilter.FilterScript(I18nDateTimeUtil.FormatToDateStringForUI(people.birthDate));
            }

            if (!string.IsNullOrEmpty(people.gender) && gviewBll.IsFieldVisible(models, "radioListAppGender"))
            {
                gender = ScriptFilter.FilterScript(StandardChoiceUtil.GetGenderByKey(people.gender));
            }

            if (!string.IsNullOrEmpty(people.businessName) && gviewBll.IsFieldVisible(models, "txtAppOrganizationName"))
            {
                businessName = ScriptFilter.FilterScript(people.businessName);
            }

            if (!string.IsNullOrEmpty(people.businessName2) && gviewBll.IsFieldVisible(models, "txtBusinessName2"))
            {
                businessName2 = ScriptFilter.FilterScript(people.businessName2);
            }

            if (!string.IsNullOrEmpty(people.tradeName) && gviewBll.IsFieldVisible(models, "txtAppTradeName"))
            {
                tradeName = ScriptFilter.FilterScript(people.tradeName);
            }

            if (!string.IsNullOrEmpty(people.socialSecurityNumber) && gviewBll.IsFieldVisible(models, "txtSSN"))
            {
                socialSecurityNumber = ScriptFilter.FilterScript(MaskUtil.FormatSSNShow(people.socialSecurityNumber));
            }

            if (!string.IsNullOrEmpty(people.fein) && gviewBll.IsFieldVisible(models, "txtAppFein"))
            {
                fein = ScriptFilter.FilterScript(MaskUtil.FormatFEINShow(people.fein, StandardChoiceUtil.IsEnableFeinMasking()));
            }

            if (people.compactAddress != null)
            {
                if (!string.IsNullOrEmpty(people.compactAddress.addressLine1) && gviewBll.IsFieldVisible(models, "txtAppStreetAdd1"))
                {
                    addressLine1 = ScriptFilter.FilterScript(people.compactAddress.addressLine1);
                }

                if (!string.IsNullOrEmpty(people.compactAddress.addressLine2) && gviewBll.IsFieldVisible(models, "txtAppStreetAdd2"))
                {
                    addressLine2 = ScriptFilter.FilterScript(people.compactAddress.addressLine2);
                }

                if (!string.IsNullOrEmpty(people.compactAddress.addressLine3) && gviewBll.IsFieldVisible(models, "txtAppStreetAdd3"))
                {
                    addressLine3 = ScriptFilter.FilterScript(people.compactAddress.addressLine3);
                }

                if (!string.IsNullOrEmpty(people.compactAddress.city) && gviewBll.IsFieldVisible(models, "txtAppCity"))
                {
                    city = ScriptFilter.FilterScript(people.compactAddress.city);
                }

                if (!string.IsNullOrEmpty(people.compactAddress.state) && gviewBll.IsFieldVisible(models, "txtAppState"))
                {
                    state = ScriptFilter.FilterScript(I18nUtil.DisplayStateForI18N(people.compactAddress.state, people.compactAddress.countryCode));
                }

                if (!string.IsNullOrEmpty(people.compactAddress.zip) && gviewBll.IsFieldVisible(models, "txtAppZipApplicant"))
                {
                    zip = ModelUIFormat.FormatZipShow(people.compactAddress.zip, people.compactAddress.countryCode);
                }

                if (!string.IsNullOrEmpty(people.compactAddress.countryCode) && gviewBll.IsFieldVisible(models, "ddlAppCountry"))
                {
                    country = ScriptFilter.FilterScript(StandardChoiceUtil.GetCountryByKey(people.compactAddress.countryCode));
                }
            }

            if (!string.IsNullOrEmpty(people.postOfficeBox) && gviewBll.IsFieldVisible(models, "txtAppPOBox"))
            {
                postOfficeBox = ScriptFilter.FilterScript(people.postOfficeBox);
            }

            //Extense contact fields
            string countryCode = people.compactAddress != null ? people.compactAddress.countryCode : people.countryCode;

            if (!string.IsNullOrEmpty(people.phone1) && gviewBll.IsFieldVisible(models, "txtAppPhone1"))
            {
                phone1 = string.Format("{0}{1}", ModelUIFormat.FormatPhoneCountryCodeShow(people.phone1CountryCode), ModelUIFormat.FormatPhone4EditPage(people.phone1, countryCode));
            }

            if (!string.IsNullOrEmpty(people.phone2) && gviewBll.IsFieldVisible(models, "txtAppPhone2"))
            {
                phone2 = string.Format("{0}{1}", ModelUIFormat.FormatPhoneCountryCodeShow(people.phone2CountryCode), ModelUIFormat.FormatPhone4EditPage(people.phone2, countryCode));
            }

            if (!string.IsNullOrEmpty(people.phone3) && gviewBll.IsFieldVisible(models, "txtAppPhone3"))
            {
                phone3 = string.Format("{0}{1}", ModelUIFormat.FormatPhoneCountryCodeShow(people.phone3CountryCode), ModelUIFormat.FormatPhone4EditPage(people.phone3, countryCode));
            }

            if (!string.IsNullOrEmpty(people.fax) && gviewBll.IsFieldVisible(models, "txtAppFax"))
            {
                fax = string.Format("{0}{1}", ModelUIFormat.FormatPhoneCountryCodeShow(people.faxCountryCode), ModelUIFormat.FormatPhone4EditPage(people.fax, countryCode));
            }

            if (!string.IsNullOrEmpty(people.email) && gviewBll.IsFieldVisible(models, "txtAppEmail"))
            {
                email = ScriptFilter.FilterScript(people.email);
            }

            // Access Level field should not displayed in Enter Information/Clerk Information/Clerk Registration Summary page
            if (ShowAccessLevel
                && !string.IsNullOrEmpty(accessLevel)
                && gviewBll.IsFieldVisible(models, "radioListContactPermission"))
            {
                accessText = ScriptFilter.FilterScript(DropDownListBindUtil.GetContactPermissionTextByValue(accessLevel, ModuleName), false);
            }

            //Contact use for edit summery
            if (!string.IsNullOrEmpty(people.birthCity) && gviewBll.IsFieldVisible(models, "txtBirthplaceCity"))
            {
                birthplaceCity = ScriptFilter.FilterScript(people.birthCity);
            }

            if (!string.IsNullOrEmpty(people.birthState) && gviewBll.IsFieldVisible(models, "ddlBirthplaceState"))
            {
                birthplaceState = ScriptFilter.FilterScript(people.birthState);
            }

            if (!string.IsNullOrEmpty(people.birthRegion) && gviewBll.IsFieldVisible(models, "ddlBirthplaceCountry"))
            {
                birthplaceCountry = ScriptFilter.FilterScript(StandardChoiceUtil.GetCountryByKey(people.birthRegion));
            }

            if (!string.IsNullOrEmpty(people.race) && gviewBll.IsFieldVisible(models, "ddlRace"))
            {
                race = ScriptFilter.FilterScript(people.race);
            }

            if (!string.IsNullOrEmpty(people.deceasedDate) && gviewBll.IsFieldVisible(models, "txtDeceasedDate"))
            {
                deceasedDate = ScriptFilter.FilterScript(I18nDateTimeUtil.FormatToDateStringForUI(people.deceasedDate));
            }

            if (!string.IsNullOrEmpty(people.passportNumber) && gviewBll.IsFieldVisible(models, "txtPassportNumber"))
            {
                passportNumber = ScriptFilter.FilterScript(people.passportNumber);
            }

            if (!string.IsNullOrEmpty(people.driverLicenseNbr) && gviewBll.IsFieldVisible(models, "txtDriverLicenseNumber"))
            {
                driverLicenseNumber = ScriptFilter.FilterScript(people.driverLicenseNbr);
            }

            if (!string.IsNullOrEmpty(people.driverLicenseState) && gviewBll.IsFieldVisible(models, "ddlDriverLicenseState"))
            {
                driverLicenseState = ScriptFilter.FilterScript(people.driverLicenseState);
            }

            if (!string.IsNullOrEmpty(people.stateIDNbr) && gviewBll.IsFieldVisible(models, "txtStateNumber"))
            {
                stateNumber = ScriptFilter.FilterScript(people.stateIDNbr);
            }

            if (!string.IsNullOrEmpty(people.preferredChannel) && gviewBll.IsFieldVisible(models, "ddlPreferredChannel"))
            {
                preferredChannel = StandardChoiceUtil.GetStandardChoiceValueByKey(BizDomainConstant.STD_CONTACT_PREFERRED_CHANNEL, people.preferredChannel);
                preferredChannel = ScriptFilter.FilterScript(preferredChannel);
            }

            if (!string.IsNullOrEmpty(people.comment) && gviewBll.IsFieldVisible(models, "txtNotes"))
            {
                notes = ScriptFilter.FilterScript(people.comment);
            }

            string result = pattern.Replace("$$ContactType$$", contactTypeText)
                                   .Replace("$$ContactTypeFlag$$", contactTypeFlag)
                                   .Replace("$$Salutation$$", salutation)
                                   .Replace("$$Title$$", title)
                                   .Replace("$$FirstName$$", firstName)
                                   .Replace("$$MiddleName$$", middleName)
                                   .Replace("$$LastName$$", lastName)
                                   .Replace("$$FullName$$", fullName)
                                   .Replace("$$NameSuffix$$", namesuffix)
                                   .Replace("$$BirthDate$$", birthDate)
                                   .Replace("$$Gender$$", gender)
                                   .Replace("$$BusinessName$$", businessName)
                                   .Replace("$$BusinessName2$$", businessName2)
                                   .Replace("$$TradeName$$", tradeName)
                                   .Replace("$$SocialSecurityNumber$$", socialSecurityNumber)
                                   .Replace("$$Fein$$", fein)
                                   .Replace("$$AddressLine1$$", addressLine1)
                                   .Replace("$$AddressLine2$$", addressLine2)
                                   .Replace("$$AddressLine3$$", addressLine3)
                                   .Replace("$$City$$", city)
                                   .Replace("$$State$$", state)
                                   .Replace("$$Zip$$", zip)
                                   .Replace("$$CountryCode$$", country)
                                   .Replace("$$PostOfficeBox$$", postOfficeBox)
                                   .Replace("$$Phone1$$", phone1)
                                   .Replace("$$Phone2$$", phone2)
                                   .Replace("$$Phone3$$", phone3)
                                   .Replace("$$Fax$$", fax)
                                   .Replace("$$Email$$", email)
                                   .Replace("$$AccessLevel$$", accessText)
                                   .Replace("$$BirthplaceCity$$", birthplaceCity)
                                   .Replace("$$BirthplaceState$$", birthplaceState)
                                   .Replace("$$BirthplaceCountry$$", birthplaceCountry)
                                   .Replace("$$Race$$", race)
                                   .Replace("$$DeceasedDate$$", deceasedDate)
                                   .Replace("$$PassportNumber$$", passportNumber)
                                   .Replace("$$DriverLicenseNumber$$", driverLicenseNumber)
                                   .Replace("$$DriverLicenseState$$", driverLicenseState)
                                   .Replace("$$StateNumber$$", stateNumber)
                                   .Replace("$$PreferredChannel$$", preferredChannel)
                                   .Replace("$$Notes$$", notes);

            return result;
        }

        #endregion Methods
    }
}