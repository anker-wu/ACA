#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: ContactUtil.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2013-2014
 *
 *  Description:
 *  Contact Utilities.
 *
 *  Notes:
 *      $Id: ContactUtil.cs 144292 2013-11-05 14:09:43Z ACHIEVO\james.shi $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Web.UI.WebControls;
using Accela.ACA.BLL.AddressBuilder;
using Accela.ACA.BLL.APO;
using Accela.ACA.BLL.Common;
using Accela.ACA.BLL.LicenseCertification;
using Accela.ACA.BLL.People;
using Accela.ACA.Common;
using Accela.ACA.Common.Log;
using Accela.ACA.Common.Util;
using Accela.ACA.Web.Common.Control;
using Accela.ACA.Web.Component;
using Accela.ACA.Web.Examination;
using Accela.ACA.Web.People;
using Accela.ACA.Web.Util;
using Accela.ACA.WSProxy;
using log4net;

namespace Accela.ACA.Web.Common
{
    /// <summary>
    /// Contact utility class
    /// </summary>
    public static class ContactUtil
    {
        /// <summary>
        /// The log for net instance
        /// </summary>
        private static ILog _loger = LogFactory.Instance.GetLogger("CommonUtil");

        /// <summary>
        /// Checks the current page whether contain one Contact section only.
        /// </summary>
        /// <param name="currentStep">current step</param>
        /// <param name="currentPage">current page</param>
        /// <param name="componentModel">get the component Model</param>
        /// <returns>If the current page have contain one contact section only then return true else false</returns>
        public static bool IsContainContactSectionOnly(int currentStep, int currentPage, ref ComponentModel componentModel)
        {
            PageFlowGroupModel pageflowGroup = AppSession.GetPageflowGroupFromSession();

            bool isContactSectionOnly = false;
            int components = pageflowGroup.stepList[currentStep].pageList[currentPage].componentList.Length;

            if (components == 1)
            {
                long componentID = pageflowGroup.stepList[currentStep].pageList[currentPage].componentList[0].componentID;
                componentModel = pageflowGroup.stepList[currentStep].pageList[currentPage].componentList[0];

                if (componentID == (long)PageFlowComponent.APPLICANT
                    || componentID == (long)PageFlowComponent.CONTACT_1
                    || componentID == (long)PageFlowComponent.CONTACT_2
                    || componentID == (long)PageFlowComponent.CONTACT_3)
                {
                    isContactSectionOnly = true;
                }
            }

            return isContactSectionOnly;
        }

        /// <summary>
        /// Checks the contain section whether Is First Show.
        /// </summary>
        /// <param name="currentStep">The current step.</param>
        /// <param name="currentPage">The current page.</param>
        /// <returns>if contain address then return true else false</returns>
        public static bool IsFirstShowContactSection(int currentStep, int currentPage)
        {
            PageFlowGroupModel pageflowGroup = AppSession.GetPageflowGroupFromSession();

            foreach (StepModel step in pageflowGroup.stepList)
            {
                foreach (PageModel page in step.pageList)
                {
                    foreach (ComponentModel component in page.componentList)
                    {
                        if (component.componentID == (long)PageFlowComponent.APPLICANT
                            || component.componentID == (long)PageFlowComponent.CONTACT_1
                            || component.componentID == (long)PageFlowComponent.CONTACT_2
                            || component.componentID == (long)PageFlowComponent.CONTACT_3)
                        {
                            long firstPageId = component.pageID;
                            long firstStepId = page.stepID;

                            return firstPageId == pageflowGroup.stepList[currentStep].pageList[currentPage].pageID
                                   && firstStepId == pageflowGroup.stepList[currentStep].pageList[currentPage].stepID;
                        }
                    }
                }
            }

            return false;
        }

        /// <summary>
        /// Search for a contact whose component name match the specified string. This method does not perform a case-sensitive search.
        /// </summary>
        /// <param name="contactsGroup">The collection of contacts in which this method searches.</param>
        /// <returns>The contact with the specified component name, if found; otherwise, null.</returns>
        public static CapContactModel4WS GetContactFromLocateCustomer(CapContactModel4WS[] contactsGroup)
        {
            if (contactsGroup == null || contactsGroup.Length == 0)
            {
                return null;
            }

            List<CapContactModel4WS> contacts = new List<CapContactModel4WS>(contactsGroup);
            CapContactModel4WS result = contacts.Find(contact => !string.IsNullOrEmpty(contact.refContactNumber));

            return result;
        }

        /// <summary>
        /// Sets the contact type of the single contact.
        /// </summary>
        /// <param name="capModel">The cap Model.</param>
        /// <param name="sectionContactType">The contact section contact type.</param>
        /// <param name="moduleName">Name of the module.</param>
        public static void MergeTemplateFieldByContactType(CapModel4WS capModel, string sectionContactType, string moduleName)
        {
            CapContactModel4WS contact = GetContactFromLocateCustomer(capModel.contactsGroup);

            if (contact != null
                && contact.people != null)
            {
                PeopleModel4WS people = contact.people;

                ITemplateBll templateBll = ObjectFactory.GetObject<ITemplateBll>();
                TemplateAttributeModel[] peopleTemplates = templateBll.GetPeopleTemplateAttributes(sectionContactType, ConfigManager.AgencyCode, AppSession.User.PublicUserId);
                TemplateModel genericTemplate = templateBll.GetContactTemplates(ConfigManager.AgencyCode, sectionContactType, false, AppSession.User.UserSeqNum);

                //merge people template
                TemplateUtil.MergePeopleTemplateAttributes(people.attributes, peopleTemplates);
                people.attributes = peopleTemplates;

                //merge generic templdate
                GenericTemplateUtil.MergeGenericTemplate(people.template, genericTemplate, moduleName);
                people.template = genericTemplate;
            }
        }

        /// <summary>
        /// Merges the contact template model.
        /// </summary>
        /// <param name="people">The people.</param>
        /// <param name="contactType">Type of the contact.</param>
        /// <param name="moduleName">Name of the module.</param>
        public static void MergeContactTemplateModel(PeopleModel4WS people, string contactType, string moduleName)
        {
            ITemplateBll templateBll = ObjectFactory.GetObject<ITemplateBll>();
            TemplateAttributeModel[] peopleTemplates = templateBll.GetPeopleTemplateAttributes(contactType, ConfigManager.AgencyCode, AppSession.User.PublicUserId);
            TemplateModel genericTemplate = templateBll.GetContactTemplates(ConfigManager.AgencyCode, contactType, false, AppSession.User.UserSeqNum);

            //merge people template
            TemplateUtil.MergePeopleTemplateAttributes(people.attributes, peopleTemplates);
            people.attributes = peopleTemplates;

            //merge generic templdate
            GenericTemplateUtil.MergeGenericTemplate(people.template, genericTemplate, moduleName);
            people.template = genericTemplate;
        }

        /// <summary>
        /// Determines whether [is object empty] [the specified cap contact model].
        /// </summary>
        /// <param name="capContactModel">The cap contact model.</param>
        /// <param name="needInitSubObject">if set to <c>true</c> [need Initial Sub-Object].</param>
        /// <returns>The object is empty or not.</returns>
        public static bool IsObjectEmpty(this CapContactModel4WS capContactModel, bool needInitSubObject)
        {
            _loger.Debug("start capContactModel SerializeObject");
            bool result = false;

            if (capContactModel == null)
            {
                return true;
            }

            CapContactModel4WS tempContactModel = new CapContactModel4WS();

            if (needInitSubObject)
            {
                tempContactModel.capID = new CapIDModel4WS();
                tempContactModel.people = new PeopleModel4WS();
                tempContactModel.people.compactAddress = new CompactAddressModel4WS();
                tempContactModel.people.compactAddress.countryZip = string.Empty;
            }

            byte[] objectByte1 = CommonUtil.SerializeObject(tempContactModel);
            byte[] objectByte2 = CommonUtil.SerializeObject(capContactModel);

            string md5Hash1 = BitConverter.ToString(MD5.Create().ComputeHash(objectByte1));
            string md5Hash2 = BitConverter.ToString(MD5.Create().ComputeHash(objectByte2));

            result = string.Equals(md5Hash1, md5Hash2);

            _loger.Debug("end capContactModel SerializeObject");
            return result;
        }

        /// <summary>
        /// Determines whether [is contact type disable].
        /// </summary>
        /// <param name="contactSeqNbr">The contact sequence NBR.</param>
        /// <param name="contactType">Type of the contact.</param>
        /// <returns><c>true</c> if [is contact type enable] [the specified contact type]; otherwise, <c>false</c>.</returns>
        public static bool IsContactTypeEnable4AcountContactEdit(string contactSeqNbr, string contactType)
        {
            PeopleModel4WS[] approvedContacts = AppSession.User.ApprovedContacts;

            if (string.IsNullOrEmpty(contactSeqNbr) || approvedContacts == null)
            {
                return true;
            }

            if (string.IsNullOrEmpty(contactType))
            {
                contactType = (from contact in approvedContacts
                               where contact != null && contactSeqNbr.Equals(contact.contactSeqNumber)
                               select contact.contactType).FirstOrDefault();
            }

            return IsContactTypeEnable(contactType, ContactTypeSource.Reference, string.Empty, string.Empty);
        }

        /// <summary>
        /// Determines whether [is contact type enable] [the specified contact type].
        /// </summary>
        /// <param name="contactType">Type of the contact.</param>
        /// <param name="contactTypeSource">The contact type source.</param>
        /// <param name="moduleName">The moduleName.</param>
        /// <param name="serviceProviderCode">The serviceProviderCode.</param>
        /// <param name="isFilterContactTypeByXPolicy">is filter contactType by XPolicy.</param>
        /// <returns><c>true</c> if [is contact type enable] [the specified contact type]; otherwise, <c>false</c>.</returns>
        public static bool IsContactTypeEnable(string contactType, string contactTypeSource, string moduleName, string serviceProviderCode, bool isFilterContactTypeByXPolicy = true)
        {
            IBizDomainBll bizDomainBll = ObjectFactory.GetObject<IBizDomainBll>();
            bool isContactTypeEnable = false;

            if (string.IsNullOrEmpty(serviceProviderCode))
            {
                serviceProviderCode = ConfigManager.AgencyCode;
            }

            if (!string.IsNullOrEmpty(contactType))
            {
                IList<ItemValue> contactItems = bizDomainBll.GetContactTypeList(serviceProviderCode, false, contactTypeSource);

                if (isFilterContactTypeByXPolicy)
                {
                    contactItems = GetEnableContactTypeListFromAdmin(contactItems, serviceProviderCode, moduleName);
                }

                if (contactItems != null)
                {
                    isContactTypeEnable = contactItems.Any(o => contactType.Equals(o.Key));
                }
            }

            return isContactTypeEnable;
        }

        /// <summary>
        /// Displays the required contact type indicator.
        /// </summary>
        /// <param name="moduleName">Name of the module.</param>
        /// <param name="componentName">Name of the component.</param>
        /// <returns>A html string indicating the required contact type.</returns>
        public static string DisplayRequiredContactTypeIndicator(string moduleName, string componentName)
        {
            PageFlowGroupModel pageFlow = AppSession.GetPageflowGroupFromSession();
            string agencyCode = pageFlow != null ? pageFlow.serviceProviderCode : ConfigManager.AgencyCode;
            string pageFlowGroupName = pageFlow != null ? pageFlow.pageFlowGrpCode : null;

            XEntityPermissionModel xentity = new XEntityPermissionModel();
            xentity.entityId = moduleName;
            xentity.entityId2 = pageFlowGroupName;
            xentity.servProvCode = agencyCode;
            xentity.entityType = XEntityPermissionConstant.CONTACT_DATA_VALIDATION;
            xentity.componentName = componentName;
            List<ContactTypeUIModel> contactTypeUIModels = DropDownListBindUtil.GetContactTypesByXEntity(xentity, true);

            StringBuilder contactTypeRow = new StringBuilder();
            StringBuilder contactTypeIndicator = new StringBuilder();

            if (contactTypeUIModels != null && contactTypeUIModels.Count > 0)
            {
                CapModel4WS capModel4WS = AppSession.GetCapModelFromSession(moduleName);

                foreach (ContactTypeUIModel contactTypeUIModel in contactTypeUIModels)
                {
                    if (string.IsNullOrEmpty(contactTypeUIModel.MinNum))
                    {
                        continue;
                    }

                    bool isComplete = false;
                    CapContactModel4WS[] capContacts = capModel4WS.contactsGroup;

                    if (capContacts != null && capContacts.Length > 0)
                    {
                        IEnumerable<CapContactModel4WS> capContactTypes =
                            capContacts.Where(
                                c => c.people.contactType.Equals(contactTypeUIModel.Key, StringComparison.InvariantCultureIgnoreCase)
                                     && c.componentName.Equals(componentName, StringComparison.InvariantCultureIgnoreCase));

                        isComplete = capContactTypes.Count() >= int.Parse(contactTypeUIModel.MinNum);
                    }

                    CreateRequiredContactTypeIndicator(isComplete, contactTypeUIModel.Key, contactTypeUIModel.MinNum, agencyCode, moduleName, contactTypeRow);
                }
            }

            if (!string.IsNullOrEmpty(contactTypeRow.ToString()))
            {
                contactTypeIndicator.AppendFormat("<table summary= '{0}'><caption>{1}</caption>", LabelUtil.GetTextByKey("aca_contacttypeindicator_summary", agencyCode, moduleName), LabelUtil.GetTextByKey("aca_contacttypeindicator_caption", agencyCode, moduleName));
                contactTypeIndicator.Append("<tr>");
                contactTypeIndicator.Append("<th scope='col'></th>");
                contactTypeIndicator.AppendFormat("<th scope='col' class='Header'>{0}</th>", LabelUtil.GetTextByKey("aca_contactlist_label_requiredcontacttype", agencyCode, moduleName));
                contactTypeIndicator.AppendFormat("<th scope='col' class='Header'>{0}</th>", LabelUtil.GetTextByKey("aca_contactlist_label_requiredcontacttypeindicator_minimum", agencyCode, moduleName));
                contactTypeIndicator.Append("</tr>");
                contactTypeIndicator.Append(contactTypeRow);
                contactTypeIndicator.Append("</table>");
            }

            return contactTypeIndicator.ToString();
        }

        /// <summary>
        /// Search for a contact whose component name match the specified string. This method does not perform a case-sensitive search.
        /// </summary>
        /// <param name="contactsGroup">The collection of contacts in which this method searches.</param>
        /// <param name="componentName">The component name to be found.</param>
        /// <returns>The contact with the specified component name, if found; otherwise, null.</returns>
        public static CapContactModel4WS FindContactWithComponentName(CapContactModel4WS[] contactsGroup, string componentName)
        {
            if (contactsGroup == null || contactsGroup.Length == 0)
            {
                return null;
            }

            if (string.IsNullOrEmpty(componentName))
            {
                return null;
            }

            List<CapContactModel4WS> contacts = new List<CapContactModel4WS>(contactsGroup);
            CapContactModel4WS result = contacts.Find(contact => componentName.Equals(contact.componentName, StringComparison.InvariantCultureIgnoreCase));

            return result;
        }

        /// <summary>
        /// Finds the name of the contact list with component.
        /// </summary>
        /// <param name="contactsGroup">The contacts group.</param>
        /// <param name="componentName">Name of the component.</param>
        /// <returns>The Cap Contact Models for specified component</returns>
        public static CapContactModel4WS[] FindContactListWithComponentName(CapContactModel4WS[] contactsGroup, string componentName)
        {
            List<CapContactModel4WS> contactsGroupList = new List<CapContactModel4WS>();

            if (contactsGroup != null && contactsGroup.Length != 0)
            {
                foreach (CapContactModel4WS contact in contactsGroup)
                {
                    if (contact == null ||
                        !componentName.Equals(contact.componentName, StringComparison.InvariantCultureIgnoreCase))
                    {
                        continue;
                    }

                    contactsGroupList.Add(contact);
                }
            }

            return contactsGroupList.ToArray();
        }

        /// <summary>
        /// Determines whether [is reference data for contact] [the specified user controls].
        /// </summary>
        /// <param name="htUserControls">The user controls.</param>
        /// <param name="capModel">The cap model.</param>
        /// <param name="dictKeysAndNamesForMultipleApplicant">The dictionary keys and names for multiple applicant.</param>
        /// <param name="dictKeysAndNamesForMultipleContact1">The dictionary keys and names for multiple contact1.</param>
        /// <param name="dictKeysAndNamesForMultipleContact2">The dictionary keys and names for multiple contact2.</param>
        /// <param name="dictKeysAndNamesForMultipleContact3">The dictionary keys and names for multiple contact3.</param>
        /// <param name="dictKeysAndNamesForMultipleContactList">The dictionary keys and names for multiple contact list.</param>
        /// <returns>True is Reference else is not</returns>
        public static bool IsReferenceDataForContact(
                                                Hashtable htUserControls,
                                                CapModel4WS capModel,
                                                Dictionary<string, string> dictKeysAndNamesForMultipleApplicant,
                                                Dictionary<string, string> dictKeysAndNamesForMultipleContact1,
                                                Dictionary<string, string> dictKeysAndNamesForMultipleContact2,
                                                Dictionary<string, string> dictKeysAndNamesForMultipleContact3,
                                                Dictionary<string, string> dictKeysAndNamesForMultipleContactList)
        {
            bool isReferenceData = true;

            List<string> contactEditKeys = new List<string>();
            contactEditKeys.AddRange(dictKeysAndNamesForMultipleApplicant.Keys);
            contactEditKeys.AddRange(dictKeysAndNamesForMultipleContact1.Keys);
            contactEditKeys.AddRange(dictKeysAndNamesForMultipleContact2.Keys);
            contactEditKeys.AddRange(dictKeysAndNamesForMultipleContact3.Keys);

            foreach (var key in contactEditKeys)
            {
                ContactEdit contactedit = (ContactEdit)htUserControls[key];
                string errorMsg = IsValidContact(contactedit.ComponentName, contactedit.ValidateFlag, capModel);

                if (!string.IsNullOrEmpty(errorMsg))
                {
                    contactedit.ShowValidateErrorMessage(errorMsg);
                    isReferenceData = false;
                }
            }

            foreach (var keyNamePair in dictKeysAndNamesForMultipleContactList)
            {
                string key = keyNamePair.Key;
                MultiContactsEdit multicontactedit = (MultiContactsEdit)htUserControls[key];
                string errorMsg = IsValidContact(multicontactedit.ComponentName, multicontactedit.ValidateFlag, capModel);

                if (!string.IsNullOrEmpty(errorMsg))
                {
                    multicontactedit.ShowValidateErrorMessage(errorMsg);
                    isReferenceData = false;
                }
            }

            return isReferenceData;
        }

        /// <summary>
        /// valid contact 
        /// </summary>
        /// <param name="capContactModel">cap contact model</param>
        /// <param name="moduleName">module name</param>
        /// <returns>if error message is null,it is valid. else it is invalid.</returns>
        public static string IsValidContact(CapContactModel4WS[] capContactModel, string moduleName)
        {
            string errorMsg = string.Empty;

            if (capContactModel != null)
            {
                foreach (CapContactModel4WS capContact in capContactModel)
                {
                    if (string.IsNullOrEmpty(capContact.refContactNumber))
                    {
                        errorMsg = LabelUtil.GetTextByKey("aca_contactedit_msg_referencedatarequired", moduleName);
                        return errorMsg;
                    }
                }
            }

            return errorMsg;
        }

        /// <summary>
        /// Combines a list of contacts with a array of contacts. Overwriting a contact of the same component name is allowed.
        /// </summary>
        /// <param name="contactsGroup">The array that contains the contacts to be combined.</param>
        /// <param name="contactsList">The list of contacts to be added.</param>
        /// <param name="overwrite">true if the original data can be overwritten; otherwise, false.</param>
        /// <returns>An array which contains all the contacts in the original array and the newly added list.</returns>
        public static CapContactModel4WS[] AppendContactsListToGroup(CapContactModel4WS[] contactsGroup, IList<CapContactModel4WS> contactsList, bool overwrite = true)
        {
            if (contactsList == null || contactsList.Count == 0)
            {
                return contactsGroup;
            }

            if (contactsGroup == null || contactsGroup.Length == 0)
            {
                return contactsList.ToArray();
            }

            IEnumerable<string> componentNames = (from contact in contactsList
                                                  where !string.IsNullOrEmpty(contact.componentName)
                                                  select contact.componentName).Distinct();

            List<CapContactModel4WS> originalContactsList = new List<CapContactModel4WS>(contactsGroup);
            originalContactsList.RemoveAll(contact => string.IsNullOrEmpty(contact.componentName));

            if (overwrite)
            {
                originalContactsList.RemoveAll(contact => componentNames.Contains(contact.componentName));
            }

            List<CapContactModel4WS> results = new List<CapContactModel4WS>(contactsList);
            results.RemoveAll(contact => string.IsNullOrEmpty(contact.componentName));
            results.AddRange(originalContactsList);

            return results.ToArray();
        }

        /// <summary>
        /// Validates the required address type contact section.
        /// </summary>
        /// <param name="capModel">The cap model.</param>
        /// <param name="moduleName">Name of the module.</param>
        /// <param name="contactEdit">The contact edit.</param>
        /// <returns>Return the error message when validate.</returns>
        public static string ValidateRequiredAddressType4ContactSection(CapModel4WS capModel, string moduleName, ContactEdit contactEdit)
        {
            string message = string.Empty;

            if (contactEdit != null)
            {
                bool isContactSectionEditable = contactEdit.IsEditable;
                bool isEnableContactAddress = StandardChoiceUtil.IsEnableContactAddress();

                CapContactModel4WS capContactsGroup = FindContactWithComponentName(capModel.contactsGroup, contactEdit.ComponentName);

                if (capContactsGroup != null && capContactsGroup.people != null && isEnableContactAddress && isContactSectionEditable)
                {
                    PeopleModel4WS people = capContactsGroup.people;
                    message = RequiredValidationUtil.ValidateContactAddressType(people.contactType, people.contactAddressList);
                }
            }

            return message;
        }

        /// <summary>
        /// Validates the required fields for section.
        /// </summary>
        /// <param name="capModel">The cap model.</param>
        /// <param name="moduleName">Name of the module.</param>
        /// <param name="htUserControls">The user controls.</param>
        /// <returns>Indicate whether it is validate success.</returns>
        public static bool ValidateRequiredFields4ContactSection(CapModel4WS capModel, string moduleName, Hashtable htUserControls)
        {
            foreach (string key in htUserControls.Keys)
            {
                if (key.StartsWith(PageFlowConstant.SECTION_NAME_CONTACT1_PREFIX)
                    || key.StartsWith(PageFlowConstant.SECTION_NAME_CONTACT2_PREFIX)
                    || key.StartsWith(PageFlowConstant.SECTION_NAME_CONTACT3_PREFIX)
                    || key.StartsWith(PageFlowConstant.SECTION_NAME_APPLICANT_PREFIX))
                {
                    ContactEdit contactEdit = (ContactEdit)htUserControls[key];

                    // Do not validate the contact which Data Source is setting as Reference.
                    if (!contactEdit.IsEditable || (contactEdit.IsValidate && !string.IsNullOrEmpty(contactEdit.RefContactNumber)))
                    {
                        continue;
                    }

                    if (!ValidateContactAddressSection(capModel, moduleName, contactEdit))
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        /// <summary>
        /// Validates the required fields for section.
        /// </summary>
        /// <param name="capModel">The cap model.</param>
        /// <param name="modulename">Name of the module.</param>
        /// <param name="contactComponent">The contact Component.</param>
        /// <returns>check the contact information</returns>
        public static bool ValidateRequiredFields4ContactSection(CapModel4WS capModel, string modulename, ComponentModel contactComponent)
        {
            CapContactModel4WS capContactModel = GetContactFromLocateCustomer(capModel.contactsGroup);

            if (capContactModel != null)
            {
                bool isRefContact = ComponentDataSource.Reference.Equals(contactComponent.validateFlag, StringComparison.InvariantCultureIgnoreCase);
                bool isValidateContact = ValidateRequiredField4SingleContact(modulename, capContactModel, ValidationUtil.IsYes(contactComponent.editableFlag), isRefContact, contactComponent.validateFlag, ACAConstant.ContactSectionPosition.SpearForm);

                if (!isValidateContact)
                {
                    return false;
                }

                //validate for contact section.
                string message = ValidateRequiredAddressType4ContactSection(capModel, ValidationUtil.IsYes(contactComponent.editableFlag));

                if (!isRefContact && !string.IsNullOrEmpty(message))
                {
                    return false;
                }

                //set as primary address
                if (StandardChoiceUtil.IsEnableContactAddress()
                    && StandardChoiceUtil.IsPrimaryContactAddressRequired()
                    && capContactModel.people != null
                    && capContactModel.people.contactAddressList != null
                    && !capContactModel.people.contactAddressList.Any(address => ValidationUtil.IsYes(address.primary)))
                {
                    capContactModel.people.contactAddressList[0].primary = ACAConstant.COMMON_Y;
                }

                bool isContactValidatePrimaryPass = ValidateRequiredPrimaryAddress4ContactSection(capModel, ValidationUtil.IsYes(contactComponent.editableFlag));

                if (!isRefContact && !isContactValidatePrimaryPass)
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Validates the required primary address contact section.
        /// </summary>
        /// <param name="capModel">The cap model.</param>
        /// <param name="moduleName">Name of the module.</param>
        /// <param name="contactEdit">The contact edit.</param>
        /// <returns>Indicate whether it is validate success.</returns>
        public static bool ValidateRequiredPrimaryAddress4ContactSection(CapModel4WS capModel, string moduleName, ContactEdit contactEdit)
        {
            bool isContactValidatePass = true;

            if (contactEdit != null)
            {
                bool isContactSectionEditable = contactEdit.IsEditable;
                bool isEnableContactAddress = StandardChoiceUtil.IsEnableContactAddress();

                CapContactModel4WS capContactModel = FindContactWithComponentName(capModel.contactsGroup, contactEdit.ComponentName);

                /*
                 * Pass the validation in the following scenarios:
                 * 1. Contact Section is not editable
                 * 2. Contact Address not be enabled
                 * 3. Contact model is empty.
                 * 4. Contact model is not empty but passed the Primary validation.
                 */
                isContactValidatePass = !isContactSectionEditable
                                        || !isEnableContactAddress
                                        || capContactModel == null
                                        || RequiredValidationUtil.ValidateCAPrimary(true, contactEdit.ContactSectionPosition, new[] { capContactModel });
            }

            return isContactValidatePass;
        }

        /// <summary>
        /// Removes the contact with component name from contact group.
        /// </summary>
        /// <param name="contactGroup">The contact group.</param>
        /// <param name="componentName">The component name.</param>
        /// <returns>The array that consists of the elements in contactsGroup that do not contain the contact with the specified component name.</returns>
        public static CapContactModel4WS[] RemoveContactWithComponentNameFromGroup(CapContactModel4WS[] contactGroup, string componentName)
        {
            if (contactGroup == null)
            {
                return null;
            }

            if (string.IsNullOrEmpty(componentName))
            {
                return contactGroup;
            }

            List<CapContactModel4WS> results = new List<CapContactModel4WS>(contactGroup);
            results.RemoveAll(contact => contact.componentName == componentName);

            return results.ToArray();
        }

        /// <summary>
        /// Picks out an appropriated contact whose component name match the specified string.
        /// </summary>
        /// <param name="contactsGroup">An array that contains the elements to be picked out.</param>
        /// <param name="sectionNamePrefix">The prefix of the section name to be found.</param>
        /// <returns>The contact with the specified component name, if found; otherwise, null.</returns>
        public static CapContactModel4WS FindContactWithSectionNamePrefix(CapContactModel4WS[] contactsGroup, string sectionNamePrefix)
        {
            if (contactsGroup == null)
            {
                return null;
            }

            if (string.IsNullOrEmpty(sectionNamePrefix))
            {
                return null;
            }

            List<CapContactModel4WS> contacts = InitializeCandidateList(contactsGroup);

            if (contacts == null)
            {
                return null;
            }

            /*
             * The component name looks like the following strings:
             * Applicant_88
             * Applicant_1234
             * Contact1_1234
             * Contact2_1234
             * Contact3_1234
             * MultiContacts_1234
             * 
             * It consists of three parts: the type of a contact, the character '_', and the sequence number in DB.
             * We need to find out the same contact type.
             */
            CapContactModel4WS result = contacts.Find(contact => !string.IsNullOrEmpty(contact.componentName)
                                                                 && contact.componentName.StartsWith(sectionNamePrefix));

            if (result == null)
            {
                bool isContainContactBesidesContactList = contacts.Any(contact => !string.IsNullOrEmpty(contact.componentName)
                                                                                  && !contact.componentName.StartsWith(PageFlowConstant.SECTION_NAME_MULTI_CONTACT_PREFIX));

                /* 
                 * The original set of contacts only contains data of type Contact List.
                 * In this kind of cases, choose a contact in original set of contacts randomly. 
                 */
                if (!isContainContactBesidesContactList)
                {
                    result = contacts.FirstOrDefault();
                }
            }

            return result;
        }

        /// <summary>
        /// Picks out an array of contacts.
        /// </summary>
        /// <param name="contactsGroup">An array that contains the elements to be picked out.</param>
        /// <returns>The array with the specified component name, if found; otherwise, null.</returns>
        public static CapContactModel4WS[] FindAppropriatedContacts(CapContactModel4WS[] contactsGroup)
        {
            if (contactsGroup == null)
            {
                return null;
            }

            List<CapContactModel4WS> contacts = InitializeCandidateList(contactsGroup);

            return contacts.ToArray();
        }

        /// <summary>
        /// Initializes an array whose component do not contain the sequence number.
        /// </summary>
        /// <param name="contactsGroup">An array that contains the elements to be initialized.</param>
        /// <param name="existComponentNames">The existing component names.</param>
        /// <returns>An array that consists of the elements in contactsGroup whose component name do not contain the sequence number. If contactsGroup is empty array, the method returns null.</returns>
        public static CapContactModel4WS[] SetupContactsSourceForCopyRecord(CapContactModel4WS[] contactsGroup, List<string> existComponentNames)
        {
            if (contactsGroup == null)
            {
                return null;
            }

            List<CapContactModel4WS> results = new List<CapContactModel4WS>(contactsGroup);

            /*
             * The component name looks like the following strings:
             * Applicant_88
             * Applicant_1234
             * Contact1_1234
             * Contact2_1234
             * Contact3_1234
             * MultiContacts_1234
             * It consists of three parts: the type of a contact, the character '_', and the sequence number in DB.
             * We need to remove the character and the sequence number since it may be changed and not included in current page flow when copying a record.
             * We should keep the type since we need a way to figure out the corresponding type when copying a record.
             */
            results.ForEach(contact =>
                {
                    int index = contact.componentName.IndexOf('_');

                    if (!string.IsNullOrEmpty(contact.componentName) && index != -1 && existComponentNames.Count >= 0
                        && !existComponentNames.Contains(contact.componentName))
                    {
                        contact.componentName = contact.componentName.Substring(0, index);
                    }
                });

            return results.ToArray();
        }

        /// <summary>
        /// Prepares the contacts for copying record.
        /// </summary>
        /// <param name="capModel">The cap model to be checked.</param>
        /// <param name="pageFlowGroup">The page flow to be checked.</param>
        public static void PrepareContactsForCopyingRecord(CapModel4WS capModel, PageFlowGroupModel pageFlowGroup)
        {
            if (capModel == null || capModel.contactsGroup == null || capModel.IsContactsChecked4Record)
            {
                capModel.IsContactsChecked4Record = true;
                return;
            }

            if (pageFlowGroup == null || pageFlowGroup.stepList == null)
            {
                capModel.IsContactsChecked4Record = true;
                return;
            }

            /*
             * The list of the name of components is initialized by collecting the componentName property of each data in the array of contacts.
             * There may have more than one contacts belonged to a ContactList, so we need to remove the duplicate elements before trying to determine if it is the same page flow.
             */
            List<string> componentNames = (from contact in capModel.contactsGroup
                                           where !string.IsNullOrEmpty(contact.componentName)
                                           select contact.componentName).Distinct().ToList();

            //The List of the name of components for Contact/Contact List Components are include in current page flow.
            List<string> existComponetNames = new List<string>();

            bool isTheSamePageFlow = PageFlowUtil.IsSamePageflow4Contact(pageFlowGroup, componentNames, ref existComponetNames);

            if (!isTheSamePageFlow)
            {
                capModel.contactsGroup = SetupContactsSourceForCopyRecord(capModel.contactsGroup, existComponetNames);
            }

            capModel.IsContactsChecked4Record = true;
        }

        /// <summary>
        /// Removes the redundant contacts whose component name match the specified string from the instance of CapModel4WS.
        /// </summary>
        /// <param name="capModel">The instance of CapModel4WS whose contact data will be removed.</param>
        /// <param name="componentName">The component name to be checked.</param>
        public static void RemoveRedundantContactsWithComponentName(CapModel4WS capModel, string componentName)
        {
            if (capModel != null && capModel.contactsGroup != null && capModel.contactsGroup.Length > 0)
            {
                List<CapContactModel4WS> tmpList = new List<CapContactModel4WS>(capModel.contactsGroup);
                tmpList.RemoveAll(contact => componentName.Equals(contact.componentName, StringComparison.InvariantCultureIgnoreCase));
                capModel.contactsGroup = tmpList.ToArray();
            }
        }

        /// <summary>
        /// Removes all redundant contacts from the instance of CapModel4WS.
        /// </summary>
        /// <param name="capModel">The instance of CapModel4WS whose contact data will be removed.</param>
        public static void RemoveRedundantContacts(CapModel4WS capModel)
        {
            if (capModel != null && capModel.contactsGroup != null && capModel.contactsGroup.Length > 0)
            {
                List<CapContactModel4WS> tmpList = new List<CapContactModel4WS>(capModel.contactsGroup);
                string pattern = @"\w+_\d+";
                Regex rgx = new Regex(pattern);
                tmpList.RemoveAll(contact => !rgx.Match(contact.componentName).Success);
                capModel.contactsGroup = tmpList.ToArray();
            }
        }

        /// <summary>
        /// Gets the people from contact UI process session.
        /// </summary>
        /// <param name="parametersModel">contact UI process session</param>
        /// <returns>return people model</returns>
        public static PeopleModel4WS GetPeopleModelFromContactSessionParameter(ContactSessionParameter parametersModel = null)
        {
            if (parametersModel == null)
            {
                parametersModel = AppSession.GetContactSessionParameter();

                if (parametersModel == null)
                {
                    return null;
                }
            }

            PeopleModel4WS people = null;

            if (parametersModel.Data.DataObject is CapContactModel4WS)
            {
                CapContactModel4WS capContact = parametersModel.Data.DataObject as CapContactModel4WS;
                people = capContact.people;
            }
            else if (parametersModel.Data.DataObject is PeopleModel4WS)
            {
                people = parametersModel.Data.DataObject as PeopleModel4WS;
            }

            return people;
        }

        /// <summary>
        /// Save contact address list to contact UI process session.
        /// </summary>
        /// <param name="addressList">Contact address list</param>
        /// <param name="parametersModel">Contact UI process session</param>
        public static void SetContactAddressListToContactSessionParameter(ContactAddressModel[] addressList, ContactSessionParameter parametersModel = null)
        {
            if (parametersModel == null)
            {
                parametersModel = AppSession.GetContactSessionParameter();
            }

            PeopleModel4WS people = GetPeopleModelFromContactSessionParameter(parametersModel);

            if (people != null)
            {
                people.contactAddressList = addressList;
            }

            AppSession.SetContactSessionParameter(parametersModel);
        }

        /// <summary>
        /// Validate single license section required and format.
        /// </summary>
        /// <param name="moduleName">module name</param>
        /// <param name="capContact">cap contact model</param>
        /// <param name="isEditable">editable property</param>
        /// <param name="isRefContact">is reference contact</param>
        /// <param name="validateFlag">validate flag</param>
        /// <param name="position">the position</param>
        /// <returns>Indicate validate success or not.</returns>
        public static bool ValidateRequiredField4SingleContact(string moduleName, CapContactModel4WS capContact, bool isEditable, bool isRefContact, string validateFlag, ACAConstant.ContactSectionPosition position)
        {
            if (capContact == null)
            {
                return true;
            }

            bool isSucceeded = true;

            if (!isEditable || isRefContact)
            {
                List<TemplateAttributeModel> fields = new List<TemplateAttributeModel>();

                //judge whethere exist template is always,required.
                fields.AddRange(TemplateUtil.GetAlwaysEditableRequiredTemplateFields(capContact.people.attributes));

                if (fields.Any())
                {
                    isSucceeded = RequiredValidationUtil.ValidateFields4Template(fields.ToArray());
                }
            }
            else
            {
                CapContactModel4WS[] capContacts = new[] { capContact };
                bool isContactDataSourceNoLimitation = ComponentDataSource.NoLimitation.Equals(validateFlag, StringComparison.InvariantCultureIgnoreCase);
                isSucceeded = RequiredValidationUtil.ValidateFields4ContactList(moduleName, GviewID.ContactEdit, capContacts, position)
                    && FormatValidationUtil.ValidateFormat4ContactList(moduleName, GviewID.ContactEdit, capContacts, isContactDataSourceNoLimitation, position);
            }

            return isSucceeded;
        }

        /// <summary>
        /// Initializes the contact information of a specific record by combining the values of two fields.
        /// </summary>
        /// <param name="capModel">The record object whose contacts will be initialized.</param>
        public static void InitializeContactsGroup4CapModel(CapModel4WS capModel)
        {
            /*
             * There is a field in the CapModel4WS class called applicantModel which representing a contact data of Applicant type.
             * Besides is, there is another field called contactsGroup which representing all the contacts associated with the specific record.
             * If this record has a contact in Applicant type, one or more than one, the JAVA code will put one of those contacts into the applicantModel. 
             * And if there is an Applicant component inside the page flow, and its customHeading field has not been set to type Applicant, then the corresponding applicantModel will contain a wrong data.
             * So, we need to move the data stored in the applicantModel to contactsGroup. Otherwise, the corresponding record will lose one contact data.
             * 
             * Once the stand-alone applicant data has been moved to the array of contacts representing by contactsGroup variable, there is an extra initialization process that the contact collection 
             * should go through. That is to assign a row index, including contact index that represent the position of the specific contact in the array and 
             * address index that represent the position of the address of the specific contact for each item in the array.
             */
            if (capModel == null)
            {
                return;
            }

            if (capModel.contactsGroup != null && capModel.contactsGroup.Length != 0)
            {
                foreach (var capContact in capModel.contactsGroup.Where(o => string.IsNullOrEmpty(o.componentName)))
                {
                    capContact.componentName = PageFlowConstant.SECTION_NAME_MULTI_CONTACT_PREFIX;
                }
            }

            if (capModel.applicantModel != null)
            {
                bool isApplicantEmpty = capModel.applicantModel.IsObjectEmpty(true);

                if (!isApplicantEmpty && string.IsNullOrEmpty(capModel.applicantModel.componentName))
                {
                    capModel.applicantModel.componentName = PageFlowConstant.SECTION_NAME_MULTI_CONTACT_PREFIX;
                }

                if (!isApplicantEmpty)
                {
                    capModel.contactsGroup = AppendContactsListToGroup(capModel.contactsGroup, new List<CapContactModel4WS> { capModel.applicantModel }, false);
                }

                capModel.applicantModel = null;
            }

            if (capModel.contactsGroup == null || capModel.contactsGroup.Length == 0)
            {
                return;
            }

            // Assign the index for contact and address
            int contactIndex = 0;

            foreach (CapContactModel4WS capContact in capModel.contactsGroup)
            {
                capContact.people.RowIndex = contactIndex;
                contactIndex++;

                if (capContact.people == null || capContact.people.contactAddressList == null)
                {
                    continue;
                }

                int addressIndex = 0;

                foreach (ContactAddressModel addressModel in capContact.people.contactAddressList)
                {
                    addressModel.RowIndex = addressIndex;
                    addressIndex++;
                }
            }
        }

        /// <summary>
        /// Whether exist the reference contact(not organization) in cap model.
        /// </summary>
        /// <param name="capModel">The cap model.</param>
        /// <returns>True: exist, otherwise not exist.</returns>
        public static bool IsRefContactExist(CapModel4WS capModel)
        {
            if (capModel == null || capModel.contactsGroup == null)
            {
                return false;
            }

            return capModel.contactsGroup.Any(f => !string.IsNullOrWhiteSpace(f.refContactNumber)
                && EnumUtil<ContactType4License>.Parse(f.people.contactTypeFlag, ContactType4License.Individual) == ContactType4License.Individual);
        }

        /// <summary>
        /// Check the contact is primary contact or not.
        /// </summary>
        /// <param name="capContactModel">The cap contact model.</param>
        /// <returns>True: is primary contact, otherwise not.</returns>
        public static bool IsPrimaryContact(CapContactModel4WS capContactModel)
        {
            if (capContactModel == null || string.IsNullOrWhiteSpace(capContactModel.refContactNumber) || capContactModel.people == null)
            {
                return false;
            }

            return ValidationUtil.IsYes(capContactModel.people.flag);
        }

        /// <summary>
        /// Whether the ref contact has associate education, examination or continuing education data.
        /// </summary>
        /// <param name="refContactSeqNbr">The ref contact sequence number.</param>
        /// <returns>True: has associate education/Examination/Continue Education</returns>
        public static bool HasAssociateEduExams(string refContactSeqNbr)
        {
            ILicenseCertificationBll licenseCertificationBll = ObjectFactory.GetObject<ILicenseCertificationBll>();
            IExaminationBll examinationBll = ObjectFactory.GetObject<IExaminationBll>();
            EducationModel[] educations = licenseCertificationBll.GetRefPeopleEduList(refContactSeqNbr);
            ExaminationModel[] examinations = examinationBll.GetRefPeopleExamList(refContactSeqNbr);
            ContinuingEducationModel[] contEducations = licenseCertificationBll.GetRefPeopleContEduList(refContactSeqNbr);

            if (educations != null || examinations != null || contEducations != null)
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Is the primary contact has license certification data.
        /// </summary>
        /// <param name="capModel">The cap model.</param>
        /// <param name="primaryContactSeqNbr">The primary contact sequence number.</param>
        /// <returns>True: the primary contact has license certification data; otherwise no.</returns>
        public static bool IsPrimaryContactHasLCData(CapModel4WS capModel, string primaryContactSeqNbr)
        {
            if (capModel == null || string.IsNullOrWhiteSpace(primaryContactSeqNbr))
            {
                return false;
            }

            if ((capModel.educationList != null && capModel.educationList.Any(f => Convert.ToString(f.contactSeqNumber) == primaryContactSeqNbr))
                || (capModel.examinationList != null && capModel.examinationList.Any(f => Convert.ToString(f.contactSeqNumber) == primaryContactSeqNbr))
                || (capModel.contEducationList != null && capModel.contEducationList.Any(f => Convert.ToString(f.contactSeqNumber) == primaryContactSeqNbr)))
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Get error message for duplicate contact address.
        /// </summary>
        /// <param name="moduleName">module name</param>
        /// <param name="agencyCode">agency code</param>
        /// <param name="contactAddressList">contact address list</param>
        /// <returns>error message</returns>
        public static string GetErrorMessage4DuplicateContactAddress(string moduleName, string agencyCode, IEnumerable<ContactAddressModel> contactAddressList)
        {
            string errorMsg = string.Empty;
            IPeopleBll peopleBll = ObjectFactory.GetObject<IPeopleBll>();
            ContactAddressModel[] contactAddresses = contactAddressList == null ? null : contactAddressList.ToArray();
            ContactAddressModel[] duplicateContactAddressList = peopleBll.GetDuplicateContactAddress(agencyCode, contactAddresses);

            if (duplicateContactAddressList != null && duplicateContactAddressList.Length > 0)
            {
                errorMsg = LabelUtil.GetTextByKey("aca_contactaddress_error_duplicatecontactaddress", moduleName);
            }

            return errorMsg;
        }

        /// <summary>
        /// Get duplicate contact address list.
        /// </summary>
        /// <param name="contactAddressList">Contact address list</param>
        /// <param name="targetContactAddress">Contact address that will be checked against.</param>
        /// <returns>Duplicate contact address list.</returns>
        public static ContactAddressModel[] GetDuplicateContactAddressList(IEnumerable<ContactAddressModel> contactAddressList, ContactAddressModel targetContactAddress)
        {
            IPeopleBll peopleBll = ObjectFactory.GetObject<IPeopleBll>();
            ContactAddressModel[] contactAddresses = contactAddressList == null ? null : contactAddressList.ToArray();
            ContactAddressModel[] duplicateContactAddressList = peopleBll.GetDuplicateContactAddressList(contactAddresses, targetContactAddress);

            return duplicateContactAddressList;
        }

        /// <summary>
        /// Select a contact and raise contactSelected event
        /// Build address string for display.
        /// Show combination for Address Line 1, Address Line 2 and Address Line 3 with comma.
        /// If there are no Address line1, line 2 and line3, then display "Full Address".
        /// Furthermore, if there are no Full address, then display follow the standard format like: (Short Format).
        /// </summary>
        /// <param name="contactAddress">Instance of ContactAddressModel.</param>
        /// <returns>Formatted address string.</returns>
        public static string BuildAddress(ContactAddressModel contactAddress)
        {
            string address = string.Empty;

            if (contactAddress != null)
            {
                if (!string.IsNullOrWhiteSpace(contactAddress.addressLine1) ||
                    !string.IsNullOrWhiteSpace(contactAddress.addressLine2) ||
                    !string.IsNullOrWhiteSpace(contactAddress.addressLine3))
                {
                    StringBuilder addressBuilder = new StringBuilder();

                    if (!string.IsNullOrWhiteSpace(contactAddress.addressLine1))
                    {
                        addressBuilder.Append(contactAddress.addressLine1);
                    }

                    if (!string.IsNullOrWhiteSpace(contactAddress.addressLine2))
                    {
                        if (addressBuilder.Length > 0)
                        {
                            addressBuilder.Append(ACAConstant.COMMA_BLANK);
                        }

                        addressBuilder.Append(contactAddress.addressLine2);
                    }

                    if (!string.IsNullOrWhiteSpace(contactAddress.addressLine3))
                    {
                        if (addressBuilder.Length > 0)
                        {
                            addressBuilder.Append(ACAConstant.COMMA_BLANK);
                        }

                        addressBuilder.Append(contactAddress.addressLine3);
                    }

                    address = addressBuilder.ToString();
                }
                else if (!string.IsNullOrWhiteSpace(contactAddress.fullAddress))
                {
                    address = contactAddress.fullAddress;
                }
                else
                {
                    IAddressBuilderBll addressBuilderBll = ObjectFactory.GetObject<IAddressBuilderBll>();
                    address = addressBuilderBll.BuildAddressByFormatType(contactAddress, null, AddressFormatType.SHORT_ADDRESS_NO_FORMAT);
                }
            }

            return address;
        }

        /// <summary>
        /// Get primary contact address validation message.
        /// </summary>
        /// <param name="moduleName">module name</param>
        /// <param name="contactType">contact type</param>
        /// <param name="isEditable">is editable</param>
        /// <param name="contactSectionPosition">contact section position</param>
        /// <param name="contactAddresses">contact address list</param>
        /// <returns>The primary contact address message.</returns>
        public static string GetPrimaryContactAddressMessage(string moduleName, string contactType, bool isEditable, ACAConstant.ContactSectionPosition contactSectionPosition, IList<ContactAddressModel> contactAddresses)
        {
            string message = string.Empty;

            if (RequiredValidationUtil.IsNeedValidateCAPrimary(isEditable, contactSectionPosition, contactType, contactAddresses))
            {
                string i18NContactType = StandardChoiceUtil.GetStandardChoiceValueByKey(BizDomainConstant.STD_CAT_CONTACT_TYPE, contactType);
                message = string.Format("{0} {1}", i18NContactType, LabelUtil.GetTextByKey("aca_contactaddress_message_missprimary", moduleName));
            }

            return message;
        }

        /// <summary>
        /// Check whether need to select license certification data when [Select from Account] or [Look up].
        /// </summary>
        /// <param name="capModel">The cap model.</param>
        /// <param name="contactSessionParameterModel">The contact session parameter model.</param>
        /// <param name="refContactSeqNbr">The ref contact sequence number.</param>
        /// <param name="contactTypeFlag">The contact type flag.</param>
        /// <returns>True: need to select license certification data; otherwise needn't.</returns>
        public static bool IsNeedToSelectLCData(CapModel4WS capModel, ContactSessionParameter contactSessionParameterModel, string refContactSeqNbr, string contactTypeFlag)
        {
            return !string.IsNullOrEmpty(refContactSeqNbr)
                && EnumUtil<ContactType4License>.Parse(contactTypeFlag, ContactType4License.Individual) == ContactType4License.Individual
                && contactSessionParameterModel.ContactSectionPosition == ACAConstant.ContactSectionPosition.SpearForm
                && (contactSessionParameterModel.Process.ContactProcessType == ContactProcessType.Lookup
                    || contactSessionParameterModel.Process.ContactProcessType == ContactProcessType.SelectContactFromAccount)
                && HasAssociateEduExams(refContactSeqNbr)
                && PageFlowUtil.IsEduExamComponentExist()
                && ExaminationScheduleUtil.GetCapPrimaryContact(capModel) == null;
        }

        /// <summary>
        /// Validate contact information to indicate redirect to save and close.
        /// </summary>
        /// <param name="contactSessionParameterModel">The contact session parameter.</param>
        /// <param name="moduleName">The module name.</param>
        /// <returns>validate result</returns>
        public static bool IsPassContactValidateInSpearForm(ContactSessionParameter contactSessionParameterModel, string moduleName)
        {
            bool isPass = true;
            bool isEditable = contactSessionParameterModel.PageFlowComponent.IsEditable;
            string validateFlag = contactSessionParameterModel.PageFlowComponent.ComponentDataSource;
            bool isValidate = ComponentDataSource.Reference.Equals(validateFlag, StringComparison.InvariantCultureIgnoreCase);
            CapContactModel4WS capContact = contactSessionParameterModel.Data.DataObject as CapContactModel4WS;
            PeopleModel4WS people = ContactUtil.GetPeopleModelFromContactSessionParameter(contactSessionParameterModel);
            bool isRefContact = isValidate && !string.IsNullOrEmpty(capContact.refContactNumber);
            bool isEnableContactAddress = StandardChoiceUtil.IsEnableContactAddress();

            if (ACAConstant.ContactSectionPosition.RegisterAccount.Equals(contactSessionParameterModel.ContactSectionPosition)
                || ACAConstant.ContactSectionPosition.RegisterExistingAccount.Equals(contactSessionParameterModel.ContactSectionPosition)
                || ConditionsUtil.HasContactCondition(people)
                || !ContactUtil.ValidateRequiredField4SingleContact(moduleName, capContact, isEditable, isRefContact, validateFlag, contactSessionParameterModel.ContactSectionPosition)
                || (isEditable && !isRefContact && isEnableContactAddress && !string.IsNullOrEmpty(RequiredValidationUtil.ValidateContactAddressType(contactSessionParameterModel.ContactType, people.contactAddressList)))
                || (isEnableContactAddress && !string.IsNullOrEmpty(ContactUtil.GetPrimaryContactAddressMessage(moduleName, contactSessionParameterModel.ContactType, isEditable, contactSessionParameterModel.ContactSectionPosition, people.contactAddressList))))
            {
                isPass = false;
            }

            return isPass;
        }

        /// <summary>
        /// Get the contact type editable permission for the contact type of the selected contact record by Contact Sequence Number.
        /// </summary>
        /// <param name="contactSeqNbr">Contact Sequence Number</param>
        /// <returns>The editable setting for the contact type</returns>
        public static bool GetContactTypeEditablePermissionByContactSeqNbr(string contactSeqNbr)
        {
            PeopleModel4WS peopleModel = PeopleUtil.GetPeopleByContactSeqNbr(contactSeqNbr);

            if (peopleModel != null && !string.IsNullOrEmpty(peopleModel.contactType))
            {
                return GetContactTypeEditablePermission(peopleModel.contactType);
            }

            return true;
        }

        /// <summary>
        /// Get the contact type editable permission for the contact type of the selected contact record.
        /// </summary>
        /// <param name="contactType">Contact type</param>
        /// <returns>The editable setting for the contact type</returns>
        public static bool GetContactTypeEditablePermission(string contactType)
        {
            XEntityPermissionModel xentity = new XEntityPermissionModel();
            xentity.servProvCode = ConfigManager.AgencyCode;
            xentity.entityType = XEntityPermissionConstant.REFERENCE_CONTACT_EDITABLE_BY_CONTACT_TYPE;
            xentity.entityId2 = contactType;

            IXEntityPermissionBll xEntityPermissionBll = ObjectFactory.GetObject<IXEntityPermissionBll>();
            IEnumerable<XEntityPermissionModel> contactTypeEditableEntities = xEntityPermissionBll.GetXEntityPermissions(xentity);

            bool isEditable = true;

            if (contactTypeEditableEntities != null && contactTypeEditableEntities.Count() > 0)
            {
                XEntityPermissionModel entityModel = contactTypeEditableEntities.FirstOrDefault(f => string.Equals(f.entityId2, contactType, StringComparison.InvariantCultureIgnoreCase));

                if (entityModel != null)
                {
                    if (ValidationUtil.IsNo(entityModel.permissionValue))
                    {
                        isEditable = false;
                    }
                }
            }

            return isEditable;
        }

        /// <summary>
        /// Get one people model for edit according to the following priorities:
        ///     1. Use Account Owner contact if existing,
        ///     2. If no Account Owner, use the contact which contact type in the allow register list,
        ///     3. Otherwise randomly use one contact.
        /// </summary>
        /// <param name="peopleModels">People model array.</param>
        /// <returns>People model.</returns>
        public static PeopleModel4WS GetPeopleModelForEdit(PeopleModel4WS[] peopleModels)
        {
            if (peopleModels.Length == 1)
            {
                return peopleModels[0];
            }

            PeopleModel4WS result = peopleModels.FirstOrDefault(f => ValidationUtil.IsYes(f.accountOwner));

            if (result == null)
            {
                List<ListItem> stdItems = DropDownListBindUtil.GetContactTypeItemsInRegistration(XPolicyConstant.CONTACT_TYPE_REGISTERATION);
                result = peopleModels.FirstOrDefault(f => stdItems.Any(t => t.Value == f.contactType));

                if (result == null)
                {
                    Random random = new Random();
                    int randomIndex = random.Next(peopleModels.Count());
                    result = peopleModels.ElementAt(randomIndex);
                }
            }

            return result;
        }

        /// <summary>
        /// Get full access and no access permission according to 'Filter search result' in ACA admin.
        /// </summary>
        /// <param name="contactType">Contact type.</param>
        /// <param name="viewId">View Id.</param>
        /// <param name="moduleName">Module name.</param>
        /// <param name="controlId">Control ID.</param>
        /// <returns>"F" means full access or "N" means no access.</returns>
        public static string GetDefaultContactPermisssion(string contactType, string viewId, string moduleName, string controlId)
        {
            string defaultValue = string.Empty;

            if (AppSession.IsAdmin || string.IsNullOrWhiteSpace(viewId))
            {
                return defaultValue;
            }

            /*
             * To determine whether the "Contact Permission" fields is visible.
             * If the "Contact Permission" is invisible, assign empty value.
             */
            GFilterScreenPermissionModel4WS permission =
                ControlBuildHelper.GetPermissionWithGenericTemplate(viewId, GViewConstant.PERMISSION_PEOPLE, contactType);

            IGviewBll gviewBll = (IGviewBll)ObjectFactory.GetObject(typeof(IGviewBll));
            SimpleViewElementModel4WS[] models = gviewBll.GetSimpleViewElementModel(moduleName, permission, viewId, AppSession.User.UserID);
            SimpleViewElementModel4WS contactPermission = models.Where(p => p.viewElementName.Equals(controlId, StringComparison.InvariantCultureIgnoreCase)).SingleOrDefault();

            if (contactPermission != null
                && !ACAConstant.INVALID_STATUS.Equals(contactPermission.recStatus, StringComparison.InvariantCultureIgnoreCase))
            {
                var userRoleBll = ObjectFactory.GetObject<IUserRoleBll>();
                CapModel4WS capModel = AppSession.GetCapModelFromSession(moduleName);

                if (capModel == null)
                {
                    return defaultValue;
                }

                UserRolePrivilegeModel userRole = userRoleBll.GetRecordSearchRole(ConfigManager.AgencyCode, moduleName, capModel.capType);

                if (userRole.allAcaUserAllowed || userRole.registeredUserAllowed || userRole.contactAllowed)
                {
                    defaultValue = ContactPermission.FullAccess;
                }
                else
                {
                    defaultValue = ContactPermission.NoAccess;
                }
            }

            return defaultValue;
        }

        /// <summary>
        /// Initializes a list of contacts from which the user can choose.
        /// </summary>
        /// <param name="contactsGroup">The collection of contacts from which the candidate can be picked out.</param>
        /// <returns>A list of contacts.</returns>
        private static List<CapContactModel4WS> InitializeCandidateList(CapContactModel4WS[] contactsGroup)
        {
            List<CapContactModel4WS> candidates = new List<CapContactModel4WS>(contactsGroup);

            /*
             * The component name looks like the following strings:
             * Applicant_88
             * Applicant_1234
             * Contact1_1234
             * Contact2_1234
             * Contact3_1234
             * MultiContacts_1234
             * 
             * It consists of three parts: the type of a contact, the character '_', and the sequence number in DB.
             * If a component name does contain all these thress parts, which means it has been used by a component in the page flow feature. In other words, it cannot be used by other components.
             * So we need to remove them from the candidate list.
             */
            string pattern = @"\w+_\d+";
            Regex rgx = new Regex(pattern);
            candidates.RemoveAll(contact => !string.IsNullOrEmpty(contact.componentName)
                                            && rgx.Match(contact.componentName).Success);

            return candidates;
        }

        /// <summary>
        /// Creates the required contact type indicator.
        /// </summary>
        /// <param name="isComplete">if set to <c>true</c> [is complete].</param>
        /// <param name="contactType">Type of the contact.</param>
        /// <param name="minNum">The min number.</param>
        /// <param name="agencyCode">Agency code</param>
        /// <param name="moduleName">Name of the module.</param>
        /// <param name="contactTypeRow">The contact type row.</param>
        private static void CreateRequiredContactTypeIndicator(bool isComplete, string contactType, string minNum, string agencyCode, string moduleName, StringBuilder contactTypeRow)
        {
            string imageType = isComplete ? "complete.png" : "error_16.gif";
            string imageAlt = isComplete ? "img_alt_workflow_complete" : "aca_global_js_showerror_alt";

            contactTypeRow.Append("<tr>");
            contactTypeRow.AppendFormat("<td><img class='Header ACA_NoBorder' alt='{0}' src='{1}'/></td>", LabelUtil.GetTextByKey(imageAlt, agencyCode, moduleName), ImageUtil.GetImageURL(imageType));
            contactTypeRow.AppendFormat("<td>{0}</td>", contactType);
            contactTypeRow.AppendFormat("<td>{0}</td>", minNum);
            contactTypeRow.Append("</tr>");
        }

        /// <summary>
        /// valid contact 
        /// </summary>
        /// <param name="componentName">The component name</param>
        /// <param name="validateFlag">data source type, reference or no limit.</param>
        /// <param name="capModel">cap model</param>
        /// <returns>if error message is null,it is valid, else it is invalid.</returns>
        private static string IsValidContact(string componentName, string validateFlag, CapModel4WS capModel)
        {
            string errorMsg = string.Empty;

            if (ComponentDataSource.Reference.Equals(validateFlag) && capModel.contactsGroup != null)
            {
                foreach (CapContactModel4WS capContact in capModel.contactsGroup)
                {
                    if (componentName.Equals(capContact.componentName) && string.IsNullOrEmpty(capContact.refContactNumber))
                    {
                        errorMsg = LabelUtil.GetTextByKey("aca_contactedit_msg_referencedatarequired", capModel.moduleName);
                        return errorMsg;
                    }
                }
            }

            return errorMsg;
        }

        /// <summary>
        /// Validates the contact section.
        /// </summary>
        /// <param name="capModel">The cap model.</param>
        /// <param name="moduleName">Name of the module.</param>
        /// <param name="contactEdit">The contact edit.</param>
        /// <returns>Indicate whether it is validate success.</returns>
        private static bool ValidateContactAddressSection(CapModel4WS capModel, string moduleName, ContactEdit contactEdit)
        {
            //validate for contact section.
            string message = ValidateRequiredAddressType4ContactSection(capModel, moduleName, contactEdit);

            if (!string.IsNullOrEmpty(message))
            {
                return false;
            }

            bool isContactValidatePrimaryPass = ValidateRequiredPrimaryAddress4ContactSection(capModel, moduleName, contactEdit);

            if (!isContactValidatePrimaryPass)
            {
                if (capModel != null && capModel.contactsGroup != null && capModel.contactsGroup.Any() && capModel.contactsGroup[0].people != null)
                {
                    PeopleModel4WS people = capModel.contactsGroup[0].people;

                    string i18NContactType = StandardChoiceUtil.GetStandardChoiceValueByKey(BizDomainConstant.STD_CAT_CONTACT_TYPE, people.contactType);
                    message = string.Format("{0} {1}", i18NContactType, LabelUtil.GetTextByKey("aca_contactaddress_message_missprimary", moduleName));
                    contactEdit.ShowValidateErrorMessage(message);
                }

                return false;
            }

            return true;
        }

        /// <summary>
        /// Get Enable Contact Type List from Admin
        /// </summary>
        /// <param name="stdItems">The contact items</param>
        /// <param name="agencyCode">The agency code</param>
        /// <param name="moduleName">The module name</param>
        /// <returns>The enable contact type list from admin.</returns>
        private static List<ItemValue> GetEnableContactTypeListFromAdmin(IList<ItemValue> stdItems, string agencyCode, string moduleName)
        {
            if (stdItems == null || stdItems.Count == 0)
            {
                return new List<ItemValue>();
            }

            IXPolicyBll xpolicyBll = ObjectFactory.GetObject<IXPolicyBll>();
            List<XPolicyModel> xpolicyList = null;

            //When standard choice "ENABLE_CONTACT_TYPE_FILTERING_BY_MODULE" is "NO", PageFlow level contact types should ignore module level contact type settings.
            if (StandardChoiceUtil.IsEnableContactTypeFilteringByModule())
            {
                xpolicyList = xpolicyBll.GetPolicyListByPolicyName(XPolicyConstant.CONTACT_TYPE_RESTRICTION_BY_MODULE, agencyCode);
            }

            if (xpolicyList != null && xpolicyList.Count > 0)
            {
                xpolicyList = xpolicyList.Where(
                       p => ACAConstant.LEVEL_TYPE_MODULE.Equals(p.level, StringComparison.OrdinalIgnoreCase)
                    && p.levelData == moduleName
                    && p.data2.Equals(ACAConstant.RECORD_CONTACT_TYPE)).ToList();
            }

            List<ItemValue> contactTypeList = new List<ItemValue>();

            // get the entities that has permission
            foreach (ItemValue itemValue in stdItems.Where(itemValue => itemValue != null))
            {
                if (xpolicyList != null && xpolicyList.Count > 0)
                {
                    // module level uncheck the contact type.
                    IEnumerable<XPolicyModel> moduleLevelContactTypeEnu = xpolicyList.Where(x => x.data1 == itemValue.Key && ValidationUtil.IsNo(x.rightGranted));

                    if (moduleLevelContactTypeEnu.Any())
                    {
                        continue;
                    }
                }

                contactTypeList.Add(itemValue);
            }

            return contactTypeList;
        }

        /// <summary>
        /// Validates the required address type contact section.
        /// </summary>
        /// <param name="capModel">The cap model.</param>
        /// <param name="isEditable">The contact section isEditable property.</param>
        /// <returns>check the require contact address type</returns>
        private static string ValidateRequiredAddressType4ContactSection(CapModel4WS capModel, bool isEditable)
        {
            string message = string.Empty;
            bool isContactSectionEditable = isEditable;
            bool isEnableContactAddress = StandardChoiceUtil.IsEnableContactAddress();

            CapContactModel4WS capContactModel = GetContactFromLocateCustomer(capModel.contactsGroup);

            if (capContactModel != null && capContactModel.people != null && isEnableContactAddress && isContactSectionEditable)
            {
                PeopleModel4WS people = capContactModel.people;
                message = RequiredValidationUtil.ValidateContactAddressType(people.contactType, people.contactAddressList);
            }

            return message;
        }

        /// <summary>
        /// Validates the required primary address contact section.
        /// </summary>
        /// <param name="capModel">The cap model.</param>
        /// <param name="isEditable">The contact section isEditable.</param>
        /// <returns>check the primary contact address</returns>
        private static bool ValidateRequiredPrimaryAddress4ContactSection(CapModel4WS capModel, bool isEditable)
        {
            bool isContactValidatePass = true;
            bool isContactSectionEditable = isEditable;
            bool isEnableContactAddress = StandardChoiceUtil.IsEnableContactAddress();

            CapContactModel4WS capContactModel = GetContactFromLocateCustomer(capModel.contactsGroup);

            /*
             * Pass the validation in the following scenarios:
             * 1. Contact Section is not editable
             * 2. Contact Address not be enabled
             * 3. Contact model is empty.
             * 4. Contact model is not empty but passed the Primary validation.
             */
            isContactValidatePass = !isContactSectionEditable
                                    || !isEnableContactAddress
                                    || capContactModel == null
                                    || RequiredValidationUtil.ValidateCAPrimary(true, ACAConstant.ContactSectionPosition.SpearForm, new[] { capContactModel });

            return isContactValidatePass;
        }
    }
}