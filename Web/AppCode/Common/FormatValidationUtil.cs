#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: FormatValidationUtil.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2010-2014
 *
 *  Description:
 *  This class deal with time zone conversion
 *
 *  Notes:
 * $Id: FormatValidationUtil.cs 278338 2014-09-02 05:56:58Z ACHIEVO\james.shi $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Accela.ACA.BLL.Common;
using Accela.ACA.Common;
using Accela.ACA.Common.Util;
using Accela.ACA.Web.Common.Control;
using Accela.ACA.WSProxy;

namespace Accela.ACA.Web.Common
{
    /// <summary>
    /// validate field's format.
    /// </summary>
    public static class FormatValidationUtil
    {
        /// <summary>
        /// validate fields format in the contact list.
        /// </summary>
        /// <param name="moduleName">module name</param>
        /// <param name="sectionId">section id</param>
        /// <param name="models">cap contact model list</param>
        /// <param name="isContactDataSourceNoLimitation">DataSource property for contact section.</param>
        /// <param name="contactSectionPosition">contact section position information</param>
        /// <returns>boolean value</returns>
        public static bool ValidateFormat4ContactList(string moduleName, string sectionId, CapContactModel4WS[] models, bool isContactDataSourceNoLimitation, ACAConstant.ContactSectionPosition contactSectionPosition)
        {
            bool isValidate = true;
            
            if (models != null && models.Length > 0)
            {
                bool isAutoSynContact = StandardChoiceUtil.AutoSyncPeople(moduleName, PeopleType.Contact);

                foreach (CapContactModel4WS model in models)
                {
                    PeopleModel4WS people = model.people;

                    /*
                    * If Sync Contact switch is enabled, needs to lock standard fields for below scenario:
                    * If DataSource property of Contact section is 'NoLimitation', all standard fields will
                    *   be locked except the field is requried but filed value is empty. No need validate format.
                    */
                    if (isContactDataSourceNoLimitation && isAutoSynContact && !string.IsNullOrEmpty(model.refContactNumber))
                    {
                        continue;
                    }

                    GFilterScreenPermissionModel4WS permission =
                        ControlBuildHelper.GetPermissionWithGenericTemplate(sectionId, GViewConstant.PERMISSION_PEOPLE, people.contactType, people.template);
                    IGviewBll gviewBll = ObjectFactory.GetObject<IGviewBll>();
                    SimpleViewElementModel4WS[] simpleViewElementmodels = RequiredValidationUtil.GetViewElementModels(moduleName, permission, sectionId);

                    if ((gviewBll.IsFieldVisible(simpleViewElementmodels, "txtAppPhone1") && !PhoneValidate(people.phone1, people.countryCode)) ||
                        (gviewBll.IsFieldVisible(simpleViewElementmodels, "txtAppPhone2") && !PhoneValidate(people.phone2, people.countryCode)) ||
                        (gviewBll.IsFieldVisible(simpleViewElementmodels, "txtAppPhone3") && !PhoneValidate(people.phone3, people.countryCode)) ||
                        (gviewBll.IsFieldVisible(simpleViewElementmodels, "txtAppFax") && !PhoneValidate(people.fax, people.countryCode)))
                    {
                        isValidate = false;

                        break;
                    }

                    if (people.compactAddress != null &&
                        gviewBll.IsFieldVisible(simpleViewElementmodels, "txtAppZipApplicant") && !ZipValidate(people.compactAddress.zip, people.compactAddress.countryCode))
                    {
                        isValidate = false;

                        break;
                    }

                    if (gviewBll.IsFieldVisible(simpleViewElementmodels, "txtSSN") && !SSNValidate(people.socialSecurityNumber))
                    {
                        isValidate = false;

                        break;
                    }

                    if (gviewBll.IsFieldVisible(simpleViewElementmodels, "txtAppEmail") && !EmailValidate(people.email))
                    {
                        isValidate = false;

                        break;
                    }

                    isValidate = ValidateFormat4ContactAddressList(moduleName, people.contactAddressList, GviewID.ContactAddress, contactSectionPosition);
                }
            }

            return isValidate;
        }

        /// <summary>
        /// validate fields format in the license professional list.
        /// </summary>
        /// <param name="moduleName">module name</param>
        /// <param name="sectionId">section id</param>
        /// <param name="models">cap contact model list</param>
        /// <returns>boolean value</returns>
        public static bool ValidateFormat4LPList(string moduleName, string sectionId, LicenseProfessionalModel[] models)
        {
            bool isValidate = true;

            if (models != null && models.Length > 0)
            {
                foreach (LicenseProfessionalModel model in models)
                {
                    GFilterScreenPermissionModel4WS permission = new GFilterScreenPermissionModel4WS()
                                                                 {
                                                                     permissionLevel = GViewConstant.PERMISSION_PEOPLE,
                                                                     permissionValue = model.licenseType
                                                                 };

                    SimpleViewElementModel4WS[] simpleViewElementmodels = RequiredValidationUtil.GetViewElementModels(moduleName, permission, sectionId);
                    IGviewBll gviewBll = ObjectFactory.GetObject<IGviewBll>();

                    if ((gviewBll.IsFieldVisible(simpleViewElementmodels, "txtHomePhone") && !PhoneValidate(model.phone1, model.countryCode)) ||
                        (gviewBll.IsFieldVisible(simpleViewElementmodels, "txtMobilePhone") && !PhoneValidate(model.phone2, model.countryCode)) ||
                        (gviewBll.IsFieldVisible(simpleViewElementmodels, "fax") && !PhoneValidate(model.fax, model.countryCode)))
                    {
                        isValidate = false;

                        break;
                    }

                    if (model.zip != null &&
                        gviewBll.IsFieldVisible(simpleViewElementmodels, "txtZipCode") && !ZipValidate(model.zip, model.countryCode))
                    {
                        isValidate = false;

                        break;
                    }

                    if (gviewBll.IsFieldVisible(simpleViewElementmodels, "txtSSN") && !SSNValidate(model.socialSecurityNumber))
                    {
                        isValidate = false;

                        break;
                    }
                }
            }

            return isValidate;
        }

        /// <summary>
        /// Validate fields format in the contact address list.
        /// </summary>
        /// <param name="moduleName">module name</param>
        /// <param name="models">contact address model list</param>
        /// <param name="sectionID">Specific the Section ID/View ID which need to validate.</param>
        /// <param name="contactSectionPosition">contact section position information</param>
        /// <returns>boolean value</returns>
        public static bool ValidateFormat4ContactAddressList(string moduleName, IEnumerable<ContactAddressModel> models, string sectionID, ACAConstant.ContactSectionPosition contactSectionPosition)
        {
            bool isValidate = true;

            if (models != null 
                && StandardChoiceUtil.IsEnableContactAddressEdit()
                && (ACAConstant.ContactSectionPosition.SpearForm.Equals(contactSectionPosition) || StandardChoiceUtil.IsEnableContactAddressMaintenance()))
            {
                IGviewBll gviewBll = ObjectFactory.GetObject<IGviewBll>();

                foreach (ContactAddressModel model in models)
                {
                    //Does not to validate deactivated contact addresses.
                    if (ACAConstant.INVALID_STATUS.Equals(model.auditModel.auditStatus, StringComparison.InvariantCultureIgnoreCase))
                    {
                        continue;
                    }

                    GFilterScreenPermissionModel4WS permission = new GFilterScreenPermissionModel4WS()
                    {
                        permissionLevel = GViewConstant.SECTION_CONTACT_ADDRESS
                    };

                    SimpleViewElementModel4WS[] simpleViewElementmodels = RequiredValidationUtil.GetViewElementModels(moduleName, permission, sectionID);

                    if ((gviewBll.IsFieldVisible(simpleViewElementmodels, "txtPhone") && !PhoneValidate(model.phone, model.countryCode)) ||
                        (gviewBll.IsFieldVisible(simpleViewElementmodels, "txtFax") && !PhoneValidate(model.fax, model.countryCode)))
                    {
                        isValidate = false;
                        break;
                    }
                }
            }

            return isValidate;
        }

        /// <summary>
        /// validate phone length
        /// </summary>
        /// <param name="value">phone value</param>
        /// <returns>cross validate or not</returns>
        private static bool ValidatePhoneLength(string value)
        {
            bool crossLengthValidation = true;

            if (value.Length > 40)
            {
                crossLengthValidation = false;
            }

            return crossLengthValidation;
        }

        /// <summary>
        /// validate phone format.
        /// </summary>
        /// <param name="phoneValue">phone value</param>
        /// <param name="countryCode">The country code.</param>
        /// <returns>correct format</returns>
        private static bool PhoneValidate(string phoneValue, string countryCode)
        {
            bool isValidate = true;
            IRegionalBll regionalBll = ObjectFactory.GetObject<IRegionalBll>();
            Dictionary<string, string> states = new Dictionary<string, string>();
            RegionalModel regionalModel = null;

            if (!string.IsNullOrEmpty(countryCode))
            {
                regionalModel = regionalBll.GetRegionalModelByCountry(countryCode, out states);   
            }

            string mask = string.Empty;

            if (regionalModel != null)
            {
                mask = regionalModel.phoneNumMask;
            }

            string phone = I18nPhoneUtil.FormatPhoneByMask(mask, phoneValue);

            if (!string.IsNullOrEmpty(phone) && (!ValidatePhoneLength(phone.Trim()) || !Regex.IsMatch(phone, I18nCultureUtil.PhoneMaskToExpression(mask))))
            {
                isValidate = false;
            }

            return isValidate;
        }

        /// <summary>
        /// validate phone format.
        /// </summary>
        /// <param name="zipValue">zip value</param>
        /// <param name="country">The country.</param>
        /// <returns>correct format</returns>
        private static bool ZipValidate(string zipValue, string country)
        {
            bool isValidate = true;
            IRegionalBll regionalBll = ObjectFactory.GetObject<IRegionalBll>();
            Dictionary<string, string> states = new Dictionary<string, string>();
            RegionalModel regionalModel = null;

            if (!string.IsNullOrEmpty(country))
            {
                regionalModel = regionalBll.GetRegionalModelByCountry(country, out states);
            }

            string mask = string.Empty;

            if (regionalModel != null)
            {
                if (ValidationUtil.IsNo(regionalModel.useZipCode))
                {
                    return isValidate;
                }

                mask = regionalModel.zipCodeMask;
            }

            string zip = I18nZipUtil.FormatZipByMask(mask, zipValue);

            if (!string.IsNullOrEmpty(zipValue) && !string.IsNullOrEmpty(mask) && !Regex.IsMatch(zip, I18nCultureUtil.ZipMaskToExpression(mask)))
            {
                isValidate = false;
            }

            return isValidate;
        }

        /// <summary>
        /// validate SSN format.
        /// </summary>
        /// <param name="ssnValue">SSN value</param>
        /// <returns>correct format</returns>
        private static bool SSNValidate(string ssnValue)
        {
            bool isValidate = true;

            if (!string.IsNullOrEmpty(ssnValue) && !Regex.IsMatch(ssnValue.Trim(), MaskUtil.SSNValidationExpression))
            {
                isValidate = false;
            }

            return isValidate;
        }

        /// <summary>
        /// validate email format.
        /// </summary>
        /// <param name="emailValue">email value</param>
        /// <returns>correct format</returns>
        private static bool EmailValidate(string emailValue)
        {
            bool isValidate = true;

            if (!string.IsNullOrEmpty(emailValue) && !Regex.IsMatch(emailValue.Trim(), I18nEmailUtil.EmailValidationExpression))
            {
                isValidate = false;
            }

            return isValidate;
        }
    }
}
