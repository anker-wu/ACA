#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: PeopleUtil.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2012-2014
 *
 *  Description:
 *
 *  Notes:
 *      $Id: PaginationUtil.cs 213498 2012-02-13 06:43:13Z ACHIEVO\alan.hu $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using Accela.ACA.BLL.APO;
using Accela.ACA.BLL.Common;
using Accela.ACA.BLL.People;
using Accela.ACA.Common;
using Accela.ACA.Common.Util;
using Accela.ACA.Web.Common.Control;
using Accela.ACA.Web.Util;
using Accela.ACA.WSProxy;

namespace Accela.ACA.Web.Common
{
    /// <summary>
    /// People utility class
    /// </summary>
    public static class PeopleUtil
    {
        #region Fields

        /// <summary>
        /// Gets the identity labels.
        /// </summary>
        public static Dictionary<long, string> IdentityFieldLabels
        {
            get
            {
                //28,29..Is AA gview id,use it only for match identity label key & value.
                return new Dictionary<long, string>
                {
                    { 28, "aca_contactedit_ssn" },
                    { 29, "per_appinfoedit_label_fein" },
                    { 39, "aca_contactedit_label_passport_number" },
                    { 40, "aca_contactedit_label_driver_license_number" },
                    { 41, "aca_contactedit_label_driver_license_state" },
                    { 42, "aca_contactedit_label_state_id_number" },
                    { 5, "per_appInfoEdit_label_email" }
                };
            }
        }

        /// <summary>
        /// Gets the duplicate identity fields key.
        /// </summary>
        public static List<long> DuplicateIndentityFields
        {
            get;

            private set;
        }

        #endregion Fields

        #region Methods

        /// <summary>
        /// Get generic template's Visible
        /// </summary>
        /// <param name="contactSectionPosition">contact section position</param>
        /// <returns>generic template's Visible</returns>
        public static bool IsVisibleGenericTemplate(ACAConstant.ContactSectionPosition contactSectionPosition)
        {
            string key = string.Empty;
            IBizDomainBll bizBll = ObjectFactory.GetObject<IBizDomainBll>();

            switch (contactSectionPosition)
            {
                case ACAConstant.ContactSectionPosition.RegisterAccount:
                case ACAConstant.ContactSectionPosition.RegisterAccountConfirm:
                case ACAConstant.ContactSectionPosition.RegisterExistingAccount:
                    key = BizDomainConstant.STD_HIDE_CONTACT_GENERIC_TEMPLATE_FOR_CITIZEN_REGISTRATION;
                    break;
                case ACAConstant.ContactSectionPosition.RegisterClerk:
                case ACAConstant.ContactSectionPosition.RegisterClerkConfirm:
                    key = BizDomainConstant.STD_HIDE_CONTACT_GENERIC_TEMPLATE_FOR_CLERK_REGISTRATION;
                    break;
            }

            string singleValue = bizBll.GetValueForACAConfig(ConfigManager.AgencyCode, key);
            return !ValidationUtil.IsYes(singleValue);
        }

        /// <summary>
        /// Is reference people exist when search reference contact by key.
        /// </summary>
        /// <param name="people">the people model</param>
        /// <param name="identityFieldLabels">the identity field id that mapping the label key.</param>
        /// <param name="messageKey">the message label key</param>
        /// <returns>true or false</returns>
        public static string GetIdentityKeyMessage(PeopleModel4WS people, Dictionary<long, string> identityFieldLabels, string messageKey)
        {
            string identityKeyMessage = string.Empty;

            if (people == null)
            {
                return identityKeyMessage;
            }

            var peopleBll = ObjectFactory.GetObject<IPeopleBll>();
            var searchPeople = new PeopleModel();

            searchPeople.socialSecurityNumber = people.socialSecurityNumber;
            searchPeople.driverLicenseNbr = people.driverLicenseNbr;
            searchPeople.driverLicenseState = people.driverLicenseState;
            searchPeople.stateIDNbr = people.stateIDNbr;
            searchPeople.passportNumber = people.passportNumber;
            searchPeople.fein = people.fein;
            searchPeople.contactSeqNumber = people.contactSeqNumber;
            searchPeople.email = people.email;

            var identityFieldValues = GetIdentityFiledDictionary(searchPeople);

            var contactIdentities = peopleBll.GetIdentityValidationResult(searchPeople);
            bool idFieldChanged = false;
            PeopleModel selectedPeopleModel = AppSession.GetPeopleModelFromSession(people.contactSeqNumber);

            /*
             * 1. For customer detail:
             *  a. when edit a contact, selectedPeopleModel is not null: if id field changed, need validation.     
             *  b. when new a contact, selectedPeopleModel is null, need validation.
             * 2. Other: need validation.
             */

            if (selectedPeopleModel != null && contactIdentities != null)
            {
                var identityFieldValuesInSession = GetIdentityFiledDictionary(selectedPeopleModel);

                foreach (var genericIdentityFieldModel in contactIdentities)
                {
                    if (genericIdentityFieldModel.viewElements == null || !genericIdentityFieldModel.viewElements.Any())
                    {
                        continue;
                    }

                    foreach (var viewElement in genericIdentityFieldModel.viewElements)
                    {
                        string sessionValue = string.Empty;
                        string inputValue = string.Empty;

                        if (identityFieldValuesInSession.TryGetValue(viewElement.viewElementID, out sessionValue)
                            && identityFieldValues.TryGetValue(viewElement.viewElementID, out inputValue)
                            && !string.Equals(sessionValue, inputValue, StringComparison.InvariantCultureIgnoreCase))
                        {
                            idFieldChanged = true;
                            break;
                        }
                    }

                    if (idFieldChanged)
                    {
                        break;
                    }
                }
            }

            identityKeyMessage = idFieldChanged || selectedPeopleModel == null
                                     ? GetErrorMessage(contactIdentities, identityFieldLabels, identityFieldValues, messageKey)
                                     : string.Empty;

            return identityKeyMessage;
        }

        /// <summary>
        /// Builds the owner mail address string.
        /// </summary>
        /// <param name="ownerModel">The owner model.</param>
        /// <returns>The owner mail address.</returns>
        public static string BuildOwnerMailAddressString(OwnerModel ownerModel)
        {
            StringBuilder mailAddress = new StringBuilder();

            if (!string.IsNullOrEmpty(ownerModel.mailAddress1))
            {
                mailAddress.Append(ownerModel.mailAddress1);
            }

            if (!string.IsNullOrEmpty(ownerModel.mailAddress2))
            {
                mailAddress.Append(" " + ownerModel.mailAddress2);
            }

            if (!string.IsNullOrEmpty(ownerModel.mailAddress3))
            {
                mailAddress.Append(" " + ownerModel.mailAddress3);
            }

            if (!string.IsNullOrEmpty(ownerModel.mailCity))
            {
                mailAddress.Append(" " + ownerModel.mailCity);
            }

            if (!string.IsNullOrEmpty(ownerModel.mailState))
            {
                mailAddress.Append("," + I18nUtil.DisplayStateForI18N(ownerModel.mailState, ownerModel.mailCountry));
            }

            if (!string.IsNullOrEmpty(ownerModel.mailZip))
            {
                mailAddress.Append(" " + ModelUIFormat.FormatZipShow(ownerModel.mailZip, ownerModel.mailCountry));
            }

            if (!string.IsNullOrEmpty(ownerModel.mailCountry))
            {
                mailAddress.Append(ACAConstant.HTML_BR + StandardChoiceUtil.GetCountryByKey(ownerModel.mailCountry));
            }

            return mailAddress.ToString();
        }

        /// <summary>
        /// Set Search and Sync flag for contact data before save contact data to backend.
        /// </summary>
        /// <param name="capModel4WS">the cap model</param>
        /// <param name="moduleName">Module name.</param>
        /// <param name="isForSaveAndResume">Will not set search flag in Save and Resume logic.</param>
        public static void SetSearchAndSyncFlag4Contact(CapModel4WS capModel4WS, string moduleName, bool isForSaveAndResume)
        {
            CapContactModel4WS[] contacts = capModel4WS.contactsGroup;

            if (contacts != null && contacts.Length != 0)
            {
                foreach (CapContactModel4WS contact in contacts)
                {
                    SetSearchAndSyncFlag4Contact(contact, moduleName, isForSaveAndResume);
                }
            }

            if (capModel4WS.applicantModel != null)
            {
                SetSearchAndSyncFlag4Contact(capModel4WS.applicantModel, moduleName, isForSaveAndResume);
            }

            if (capModel4WS.capContactModel != null)
            {
                SetSearchAndSyncFlag4Contact(capModel4WS.capContactModel, moduleName, isForSaveAndResume);
            }
        }

        /// <summary>
        /// Set contact sequence number to people template fields.
        /// </summary>
        /// <param name="people">The people model.</param>
        public static void SetPeopleTemplateContactSeqNum(PeopleModel4WS people)
        {
            if (people != null && people.attributes != null)
            {
                foreach (var item in people.attributes)
                {
                    TemplateAttributeModel field = item as TemplateAttributeModel;
                    field.templateObjectNum = people.contactSeqNumber;
                }
            }
        }

        /// <summary>
        /// Filter inactivate contact address
        /// </summary>
        /// <param name="peopleModel">The people model</param>
        public static void FilterInactivateContactAddress(PeopleModel4WS peopleModel)
        {
            bool isPrimaryContactAddressRequired = StandardChoiceUtil.IsPrimaryContactAddressRequired();
            ContactAddressModel[] contactAddresses = peopleModel.contactAddressList;

            if (contactAddresses != null)
            {
                IEnumerable<ContactAddressModel> activeContactAddresses = contactAddresses.Where(
                    p => ACAConstant.VALID_STATUS.Equals(p.auditModel.auditStatus, StringComparison.InvariantCultureIgnoreCase));

                if (activeContactAddresses.Any())
                {
                    if (isPrimaryContactAddressRequired && activeContactAddresses.Count() == 1)
                    {
                        activeContactAddresses.First().primary = ACAConstant.COMMON_Y;
                    }

                    peopleModel.contactAddressList = activeContactAddresses.ToArray();
                }
                else
                {
                    peopleModel.contactAddressList = null;
                }
            }
        }

        /// <summary>
        /// Merge the field values from a template model to the template fields associated with the specific contact type.
        /// </summary>
        /// <param name="sourceTemplate">Source template model</param>
        /// <param name="contactType">Contact Type</param>
        /// <param name="moduleName">Module Name</param>
        /// <returns>return the template model associated with the specific contact type.</returns>
        public static TemplateModel MergeGenericTemplate(TemplateModel sourceTemplate, string contactType, string moduleName)
        {
            ITemplateBll _templateBll = ObjectFactory.GetObject<ITemplateBll>();
            TemplateModel targetTemplate = _templateBll.GetContactTemplates(ConfigManager.AgencyCode, contactType, false, AppSession.User.PublicUserId);

            if (targetTemplate == null)
            {
                return null;
            }

            GenericTemplateUtil.MergeGenericTemplate(sourceTemplate, targetTemplate, moduleName);

            return targetTemplate;
        }

        /// <summary>
        /// Get the editing contact model in cap edit page.
        /// </summary>
        /// <param name="moduleName">The module name.</param>
        /// <param name="contactSeqNbr">The contact sequence number.</param>
        /// <returns>The people model.</returns>
        public static PeopleModel4WS GetEditingContactInCapEdit(string moduleName, string contactSeqNbr)
        {
            CapModel4WS capModel = AppSession.GetCapModelFromSession(moduleName);

            if (capModel == null || capModel.contactsGroup == null)
            {
                return null;
            }

            PeopleModel4WS people = null;
            CapContactModel4WS capContact = null;

            foreach (CapContactModel4WS item in capModel.contactsGroup)
            {
                if (item.people != null && item.people.contactSeqNumber == contactSeqNbr)
                {
                    capContact = item;
                    break;
                }
            }

            if (capContact != null)
            {
                people = capContact.people;
            }

            return people;
        }

        #region Contact edit

        /// <summary>
        /// Clear the contact temp information in the session
        /// </summary>
        public static void ClearTempContact()
        {
            RemoveTempCapContact();
            System.Web.HttpContext.Current.Session.Remove("temp_isCountryHidden");
            ClearTempPreviousContactSeqNumber();
        }

        /// <summary>
        /// Remove the contact from the session
        /// </summary>
        /// <param name="contactSeqNumber">contact sequence number</param>
        public static void RemoveTempContact(string contactSeqNumber)
        {
            AppSession.SetPeopleModelToSession(contactSeqNumber, null);
        }

        /// <summary>
        /// Save the contact to the session
        /// </summary>
        /// <param name="people">The people model.</param>
        public static void SaveTempContact(PeopleModel4WS people)
        {
            AppSession.SetPeopleModelToSession(people.contactSeqNumber, TempModelConvert.ConvertToPeopleModel(people));
        }

        /// <summary>
        /// Save the previous contact sequence number to the session.
        /// </summary>
        /// <param name="contactSeqNumber">contact sequence number</param>
        public static void SaveTempPreviousContactSeqNumber(string contactSeqNumber)
        {
            System.Web.HttpContext.Current.Session["temp_prev_contact_seq_number"] = contactSeqNumber;
        }

        /// <summary>
        /// Get the previous contact sequence number from the session.
        /// </summary>
        /// <returns>The pervious contact sequence number.</returns>
        public static string GetTempPreviousContactSeqNumber()
        {
            return System.Web.HttpContext.Current.Session["temp_prev_contact_seq_number"] as string;
        }

        /// <summary>
        /// Clear the previous contact sequence number from the session.
        /// </summary>
        public static void ClearTempPreviousContactSeqNumber()
        {
            System.Web.HttpContext.Current.Session.Remove("temp_prev_contact_seq_number");
        }

        /// <summary>
        /// Save a value indicating whether the country is hidden to the session.
        /// </summary>
        /// <param name="isCountryHidden">Whether the country is hidden.</param>
        public static void SaveTempIsCountryHidden(bool isCountryHidden)
        {
            System.Web.HttpContext.Current.Session["temp_isCountryHidden"] = isCountryHidden;
        }

        /// <summary>
        /// Gets a value indicating whether the country is hidden from the session.
        /// </summary>
        /// <returns>Indicate whether the country is hidden.</returns>
        public static bool GetTempIsCountryHidden()
        {
            object temp = System.Web.HttpContext.Current.Session["temp_isCountryHidden"];

            return temp != null && bool.Parse(temp.ToString());
        }

        /// <summary>
        /// Clear the contact look up session.
        /// </summary>
        public static void ClearTempContactLookUp()
        {
            System.Web.HttpContext.Current.Session.Remove("temp_contact_lookup");
        }

        /// <summary>
        /// Save the contact look up to session.
        /// </summary>
        /// <param name="people">The people model.</param>
        public static void SaveTempContactLookUp(PeopleModel4WS people)
        {
            System.Web.HttpContext.Current.Session["temp_contact_lookup"] = people;
        }

        /// <summary>
        /// Get the contact look up from session.
        /// </summary>
        /// <returns>The contact look up.</returns>
        public static PeopleModel4WS GetTempContactLookUp()
        {
            return System.Web.HttpContext.Current.Session["temp_contact_lookup"] as PeopleModel4WS;
        }

        /// <summary>
        /// Get the contact address by the row index from the contact address list
        /// </summary>
        /// <param name="addressList">Contact address list</param>
        /// <param name="rowIndex">Row index</param>
        /// <returns>A contact address</returns>
        public static ContactAddressModel GetContactAddress(ContactAddressModel[] addressList, int rowIndex)
        {
            ContactAddressModel result = null;

            if (addressList != null)
            {
                foreach (ContactAddressModel address in addressList)
                {
                    if (rowIndex == address.RowIndex)
                    {
                        result = address;
                        break;
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// Save a contact to the session.
        /// </summary>
        /// <param name="capContact">a CapContactModel4WS</param>
        public static void SaveTempCapContact(CapContactModel4WS capContact)
        {
            System.Web.HttpContext.Current.Session["temp_cap_contact"] = capContact;
        }

        /// <summary>
        /// Get the contact from the session.
        /// </summary>
        /// <returns>a CapContactModel4WS</returns>
        public static CapContactModel4WS GetTempCapContact()
        {
            return System.Web.HttpContext.Current.Session["temp_cap_contact"] as CapContactModel4WS;
        }

        /// <summary>
        /// Remove the contact from the session.
        /// </summary>
        public static void RemoveTempCapContact()
        {
            System.Web.HttpContext.Current.Session.Remove("temp_cap_contact");
        }

        #endregion Contact edit

        /// <summary>
        /// Get added license array from session.
        /// </summary>
        /// <returns>LicenseModel4WS array</returns>
        public static LicenseModel4WS[] GetArrayLicense()
        {
            ArrayList selectedLicenses = System.Web.HttpContext.Current.Session[SessionConstant.SESSION_REGISTER_LICENSES] as ArrayList;

            if (selectedLicenses == null || selectedLicenses.Count == 0)
            {
                return null;
            }

            return (LicenseModel4WS[])selectedLicenses.ToArray(typeof(LicenseModel4WS));
        }

        /// <summary>
        /// Save a PublicUserModel4WS to session.
        /// </summary>
        /// <param name="publicUser">a PublicUserModel4WS</param>
        public static void SavePublicUserToSession(PublicUserModel4WS publicUser)
        {
            System.Web.HttpContext.Current.Session[SessionConstant.SESSION_REGISTER_USER_MODEL] = publicUser;
        }

        /// <summary>
        /// Get a PublicUserModel4WS from session.
        /// </summary>
        /// <returns>a PublicUserModel4WS</returns>
        public static PublicUserModel4WS GetPublicUserFromSession()
        {
            PublicUserModel4WS model = System.Web.HttpContext.Current.Session[SessionConstant.SESSION_REGISTER_USER_MODEL] as PublicUserModel4WS;
            return model == null ? new PublicUserModel4WS() : model;
        }

        /// <summary>
        /// Remove the PublicUserModel4WS from session.
        /// </summary>
        public static void RemovePublicUserFromSession()
        {
            System.Web.HttpContext.Current.Session.Remove(SessionConstant.SESSION_REGISTER_USER_MODEL);
        }

        /// <summary>
        /// Get the GView ID for the contact.
        /// </summary>
        /// <param name="contactSectionPosition">A ContactSectionPosition</param>
        /// <returns>A GView ID</returns>
        public static string GetContactGViewID(ACAConstant.ContactSectionPosition contactSectionPosition)
        {
            string viewId = string.Empty;

            switch (contactSectionPosition)
            {
                case ACAConstant.ContactSectionPosition.AddReferenceContact:
                    viewId = GviewID.AddReferenceContactForm;
                    break;
                case ACAConstant.ContactSectionPosition.ModifyReferenceContact:
                    viewId = GviewID.ModifyReferenceContactForm;
                    break;
                case ACAConstant.ContactSectionPosition.RegisterAccount:
                case ACAConstant.ContactSectionPosition.RegisterAccountConfirm:
                case ACAConstant.ContactSectionPosition.RegisterExistingAccount:
                    viewId = GviewID.RegistrationContactForm;
                    break;
                case ACAConstant.ContactSectionPosition.RegisterClerk:
                case ACAConstant.ContactSectionPosition.RegisterClerkConfirm:
                    viewId = GviewID.AuthAgentNewClerkContactForm;
                    break;
                case ACAConstant.ContactSectionPosition.EditClerk:
                    viewId = GviewID.AuthAgentEditClerkContactForm;
                    break;
                default:
                    viewId = GviewID.ContactEdit;
                    break;
            }

            return viewId;
        }

        /// <summary>
        /// Get the GView ID for the contact search.
        /// </summary>
        /// <returns>A GView ID</returns>
        public static string GetContactSearchGviewID()
        {
            return GviewID.ContactLookUp;
        }

        /// <summary>
        /// Get the GView ID for the list of the contact address.
        /// </summary>
        /// <param name="contactSectionPosition">A ContactSectionPosition</param>
        /// <returns>A GView ID</returns>
        public static string GetContactAddressListGviewID(ACAConstant.ContactSectionPosition contactSectionPosition)
        {
            return GetContactAddressListGviewID(contactSectionPosition, false);
        }

        /// <summary>
        /// Get the GView ID for the list of the contact address.
        /// </summary>
        /// <param name="contactSectionPosition">A ContactSectionPosition</param>
        /// <param name="isForSearch">A value indicating whether the GView ID is for search list</param>
        /// <returns>A GView ID</returns>
        public static string GetContactAddressListGviewID(ACAConstant.ContactSectionPosition contactSectionPosition, bool isForSearch)
        {
            string viewId = GviewID.ContactAddressList;

            if (isForSearch)
            {
                viewId = GviewID.ContactAddressSearchList;
            }
            else
            {
                switch (contactSectionPosition)
                {
                    case ACAConstant.ContactSectionPosition.RegisterAccount:
                    case ACAConstant.ContactSectionPosition.AddReferenceContact:
                    case ACAConstant.ContactSectionPosition.RegisterAccountConfirm:
                    case ACAConstant.ContactSectionPosition.ModifyReferenceContact:
                    case ACAConstant.ContactSectionPosition.RegisterAccountComplete:
                        viewId = GviewID.RegistrationContactAddressList;
                        break;
                    case ACAConstant.ContactSectionPosition.RegisterClerk:
                    case ACAConstant.ContactSectionPosition.RegisterClerkConfirm:
                    case ACAConstant.ContactSectionPosition.RegisterClerkComplete:
                        viewId = GviewID.AuthAgentNewClerkContactAddressList;
                        break;
                    case ACAConstant.ContactSectionPosition.EditClerk:
                        viewId = GviewID.AuthAgentEditClerkContactAddressList;
                        break;
                    case ACAConstant.ContactSectionPosition.AuthAgentCustomerDetail:
                        viewId = GviewID.AuthAgentCustomerDetailCAList;
                        break;
                    case ACAConstant.ContactSectionPosition.ValidatedContactAddress:
                        viewId = GviewID.ExternalAddressList;
                        break;
                }
            }

            return viewId;
        }

        /// <summary>
        /// Get the GView ID for the edit of the contact address.
        /// </summary>
        /// <param name="contactSectionPosition">A ContactSectionPosition</param>
        /// <returns>A GView ID</returns>
        public static string GetContactAddressEditGviewID(ACAConstant.ContactSectionPosition contactSectionPosition)
        {
            string viewId;

            switch (contactSectionPosition)
            {
                case ACAConstant.ContactSectionPosition.SpearForm:
                case ACAConstant.ContactSectionPosition.SpearFormCloseMatchConfirm:
                    viewId = GviewID.ContactAddress;
                    break;
                case ACAConstant.ContactSectionPosition.RegisterClerk:
                case ACAConstant.ContactSectionPosition.RegisterClerkConfirm:
                case ACAConstant.ContactSectionPosition.EditClerk:
                    viewId = GviewID.AuthAgentEditClerkContactAddressForm;
                    break;
                case ACAConstant.ContactSectionPosition.AuthAgentCustomerDetail:
                    viewId = GviewID.AuthAgentCustomerDetailCAForm;
                    break;
                default:
                    viewId = GviewID.RegistrationContactAddressForm;
                    break;
            }

            return viewId;
        }

        /// <summary>
        /// Gets the required address type instruction message.
        /// </summary>
        /// <param name="moduleName">Current module name</param>
        /// <param name="contactType">Current contact type</param>
        /// <returns>The required instruction</returns>
        public static string GetRequiredAddressInstruction(string moduleName, string contactType)
        {
            string associateAddressType = string.Empty;

            if (!string.IsNullOrEmpty(contactType))
            {
                ICacheManager cacheManager = (ICacheManager)ObjectFactory.GetObject(typeof(ICacheManager));
                IList<string> requiredAddressTypes = cacheManager.GetRequiredContactAddressType(contactType);
                IList<string> i18NRequiredAddressTypes = new List<string>();

                if (requiredAddressTypes != null && requiredAddressTypes.Count > 0)
                {
                    foreach (string requiredAddressType in requiredAddressTypes)
                    {
                        i18NRequiredAddressTypes.Add(StandardChoiceUtil.GetStandardChoiceValueByKey(BizDomainConstant.STD_CAT_CONTACT_ADDRESS_TYPE, requiredAddressType));
                    }

                    associateAddressType = string.Format("{0}{1}", LabelUtil.GetTextByKey("aca_contactaddress_label_associateaddresstype", moduleName), DataUtil.ConcatStringListWithComma(i18NRequiredAddressTypes));
                }
            }

            return associateAddressType;
        }

        /// <summary>
        /// Get the people information by contact sequence number.
        /// </summary>
        /// <param name="contactSeqNbr">Contact sequence number</param>
        /// <returns>People Model</returns>
        public static PeopleModel4WS GetPeopleByContactSeqNbr(string contactSeqNbr)
        {
            if (string.IsNullOrEmpty(contactSeqNbr))
            {
                return null;
            }

            PeopleModel peopleModel = AppSession.GetPeopleModelFromSession(contactSeqNbr);

            if (peopleModel != null)
            {
                return TempModelConvert.ConvertToPeopleModel4WS(peopleModel);
            }

            PublicUserModel4WS publicUserModel = AppSession.User.UserModel4WS;

            if (publicUserModel.peopleModel != null)
            {
                PeopleModel4WS model = publicUserModel.peopleModel.FirstOrDefault(p => contactSeqNbr.Equals(p.contactSeqNumber));

                // Save to the session
                SaveTempContact(model);
                return model;
            }

            return null;
        }

        /// <summary>
        /// Save a PeopleModel4WS to session.
        /// </summary>
        /// <param name="people">the input people</param>
        public static void SaveInputPeopleForCloseMatchToSession(PeopleModel4WS people)
        {
            System.Web.HttpContext.Current.Session[SessionConstant.SESSION_INPUTPEOPLE_FOR_SPEARFORM_CLOSEMATCH] = people;
        }

        /// <summary>
        /// Get a PeopleModel4WS from session.
        /// </summary>
        /// <returns>a input people</returns>
        public static PeopleModel4WS GetInputPeopleForCloseMatchFromSession()
        {
            PeopleModel4WS model = System.Web.HttpContext.Current.Session[SessionConstant.SESSION_INPUTPEOPLE_FOR_SPEARFORM_CLOSEMATCH] as PeopleModel4WS;
            return model;
        }

        /// <summary>
        /// Remove the PeopleModel4WS from session.
        /// </summary>
        public static void RemoveInputPeopleForCloseMatchFromSession()
        {
            System.Web.HttpContext.Current.Session.Remove(SessionConstant.SESSION_INPUTPEOPLE_FOR_SPEARFORM_CLOSEMATCH);
        }

        /// <summary>
        /// set contact address entity id.
        /// </summary>
        /// <param name="contactAddressList">contact address list</param>
        /// <param name="contactSeqNumber">contact sequence number.</param>
        public static void SetContactAddressEntityID(ContactAddressModel[] contactAddressList, string contactSeqNumber)
        {
            if (contactAddressList == null || string.IsNullOrEmpty(contactSeqNumber))
            {
                return;
            }

            foreach (ContactAddressModel addressModel in contactAddressList)
            {
                if (!addressModel.entityID.HasValue)
                {
                    /*
                     * Should fill the entity info for new added contact adress.
                     * Include entityID and entityType, entityType filled in business according to the Contact type(Reference contact/Daily contact),
                     *  so just fill the entityID.
                     */
                    addressModel.entityID = long.Parse(contactSeqNumber, CultureInfo.InvariantCulture);
                }
            }
        }

        /// <summary>
        /// Get identity error message key.
        /// </summary>
        /// <param name="contactIdentities">the contact identity models.</param>
        /// <param name="identityFieldLabels">the labels.</param>
        /// <param name="identityFieldValues">the values.</param>
        /// <param name="messageKey">the message label key.</param>
        /// <returns>the identity error message.</returns>
        private static string GetErrorMessage(GenericIdentityFieldModel[] contactIdentities, Dictionary<long, string> identityFieldLabels, Dictionary<long, string> identityFieldValues, string messageKey)
        {
            var errorMessage = new StringBuilder();
            DuplicateIndentityFields = new List<long>();

            if (contactIdentities != null && contactIdentities.Length > 0)
            {
                errorMessage.Append(LabelUtil.GetTextByKey(messageKey, string.Empty));
                errorMessage.Append("<br/>");

                foreach (var contactIdentity in contactIdentities)
                {
                    if (contactIdentity.viewElements.Length > 1)
                    {
                        foreach (var viewElement in contactIdentity.viewElements)
                        {
                            errorMessage.Append(LabelUtil.GetTextByKey(identityFieldLabels[viewElement.viewElementID], string.Empty).TrimEnd(':'));
                            errorMessage.Append(ACAConstant.SLASH);

                            DuplicateIndentityFields.Add(viewElement.viewElementID);
                        }

                        errorMessage.Remove(errorMessage.Length - 1, 1);
                        errorMessage.Append(ACAConstant.COLON_CHAR);

                        foreach (var viewElement in contactIdentity.viewElements)
                        {
                            errorMessage.Append(identityFieldValues[viewElement.viewElementID]);
                            errorMessage.Append(ACAConstant.SLASH);
                        }

                        errorMessage.Remove(errorMessage.Length - 1, 1);
                    }
                    else if (contactIdentity.viewElements.Length == 1)
                    {
                        errorMessage.Append(LabelUtil.GetTextByKey(identityFieldLabels[contactIdentity.viewElements[0].viewElementID], string.Empty).TrimEnd(':'));
                        errorMessage.Append(ACAConstant.COLON_CHAR);
                        errorMessage.Append(identityFieldValues[Convert.ToInt32(contactIdentity.viewElements[0].viewElementID)]);

                        DuplicateIndentityFields.Add(contactIdentity.viewElements[0].viewElementID);
                    }

                    errorMessage.Append(ACAConstant.HTML_BR);
                }
            }

            return errorMessage.ToString();
        }

        /// <summary>
        /// Gets the identity filed dictionary.
        /// </summary>
        /// <param name="peopleModel">The people model.</param>
        /// <returns>Dictionary of identity field.</returns>
        private static Dictionary<long, string> GetIdentityFiledDictionary(PeopleModel peopleModel)
        {
            //28,29..Is AA gview id,use it only for match identity label key & value.
            var identityFieldValuesInSession = new Dictionary<long, string>
                {
                    { 28, peopleModel.socialSecurityNumber },
                    { 29, peopleModel.fein },
                    { 39, peopleModel.passportNumber },
                    { 40, peopleModel.driverLicenseNbr },
                    { 41, peopleModel.driverLicenseState },
                    { 42, peopleModel.stateIDNbr },
                    { 5, peopleModel.email }
                };

            return identityFieldValuesInSession;
        }

        /// <summary>
        /// Set Search and Sync flag for contact data before save contact data to back end.
        /// </summary>
        /// <param name="contact">the cap contact model</param>
        /// <param name="moduleName">Module name.</param>
        /// <param name="isForSaveAndResume">Will not set search flag in Save and Resume logic.</param>
        private static void SetSearchAndSyncFlag4Contact(CapContactModel4WS contact, string moduleName, bool isForSaveAndResume)
        {
            if (contact == null || contact.people == null)
            {
                return;
            }

            //People model is null, should not search reference contact so should not setting needToSync & needToSearch flag.
            IGviewBll gviewBll = (IGviewBll)ObjectFactory.GetObject(typeof(IGviewBll));
            GFilterScreenPermissionModel4WS permission =
                ControlBuildHelper.GetPermissionWithGenericTemplate(GviewID.ContactEdit, GViewConstant.PERMISSION_PEOPLE, contact.people.contactType, contact.people.template);
            SimpleViewElementModel4WS[] simpleViewElementmodels = gviewBll.GetSimpleViewElementModel(moduleName, permission, GviewID.ContactEdit, AppSession.User.UserID);
            string[] exceptRequiredFields = new string[] { "CapContactModel4WS*people*contactType" };

            if (ValidationUtil.IsAllStandardFieldsEmpty(contact, simpleViewElementmodels, exceptRequiredFields))
            {
                return;
            }

            bool isNeedSyncContact = StandardChoiceUtil.AutoSyncPeople(moduleName, PeopleType.Contact);
            string validateFlag = contact.validateFlag;
            bool isRefContact = !string.IsNullOrEmpty(contact.refContactNumber);

            if (ComponentDataSource.NoLimitation.Equals(validateFlag))
            {
                if (isNeedSyncContact)
                {
                    contact.syncFlag = ACAConstant.COMMON_Y;
                }

                if (!isRefContact && !isForSaveAndResume)
                {
                    contact.searchFlag = ACAConstant.COMMON_Y;
                }
            }
            else if (ComponentDataSource.Reference.Equals(validateFlag) && isNeedSyncContact)
            {
                contact.syncFlag = ACAConstant.COMMON_Y;
            }
        }
        #endregion Methods
    }
}