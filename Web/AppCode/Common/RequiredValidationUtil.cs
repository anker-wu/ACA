#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: RequiredValidationUtil.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2009-2014
 *
 *  Description:
 *  This class deal with time zone conversion
 *
 *  Notes:
 * $Id: RequiredValidationUtil.cs 277071 2014-08-11 03:38:26Z ACHIEVO\james.shi $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Accela.ACA.Common;
using Accela.ACA.Common.Util;
using Accela.ACA.Web.Common;
using Accela.ACA.Web.Common.Control;
using Accela.ACA.Web.Util;
using Accela.ACA.WSProxy;

namespace Accela.ACA.BLL.Common
{
    /// <summary>
    /// It providers to validation required fields in list.
    /// </summary>
    public class RequiredValidationUtil
    {
        /// <summary>
        /// Validate required fields' value in list.
        /// </summary>
        /// <param name="moduleName">module name</param>
        /// <param name="permission">the permission</param>
        /// <param name="sectionId">section id</param>
        /// <param name="models">model list</param>
        /// <returns>
        /// True - indicate all required fields have the exact value.
        /// False - at least one required fields exists the empty or null value.
        /// </returns>
        public static bool ValidateFields4List(string moduleName, GFilterScreenPermissionModel4WS permission, string sectionId, IEnumerable<object> models)
        {
            bool validateRequired = true;

            if (models == null)
            {
                return validateRequired;
            }

            foreach (object model in models)
            {
                validateRequired = ValidateFields4Object(moduleName, permission, sectionId, model);

                if (!validateRequired)
                {
                    break;
                }
            }

            return validateRequired;
        }

        /// <summary>
        /// Validates the fields for object.
        /// </summary>
        /// <param name="moduleName">Name of the module.</param>
        /// <param name="permission">The permission.</param>
        /// <param name="sectionId">The section id.</param>
        /// <param name="model">The model.</param>
        /// <returns><c>true</c> if validate successful, <c>false</c> otherwise</returns>
        public static bool ValidateFields4Object(string moduleName, GFilterScreenPermissionModel4WS permission, string sectionId, object model)
        {
            bool validateRequired = true;
            bool validateLicenseType = true;

            if (model == null)
            {
                return validateRequired;
            }

            string countryCode = string.Empty;

            if (model is ExaminationModel)
            {
                ExaminationModel examinationModel = (ExaminationModel)model;

                //Need not validate for examination which are Pending status.
                if (ACAConstant.EXAMINATION_STATUS_PENDING.Equals(examinationModel.examStatus, StringComparison.InvariantCultureIgnoreCase))
                {
                    return validateRequired;
                }
            }
            else if (model is CapContactModel4WS)
            {
                var contact = model as CapContactModel4WS;
                countryCode = contact.people.compactAddress.countryCode;
            }
            else if (model is LicenseProfessionalModel)
            {
                var lpModel = model as LicenseProfessionalModel;
                validateLicenseType = LicenseUtil.IsValidLicenseType(lpModel.licenseType);
                countryCode = lpModel.countryCode;
            }
            else if (model is ContactAddressModel)
            {
                var contact = model as ContactAddressModel;
                countryCode = contact.countryCode;

                //Does not to validate deactivated contact addresses.
                if (ACAConstant.INVALID_STATUS.Equals(contact.auditModel.auditStatus, StringComparison.InvariantCultureIgnoreCase))
                {
                    return validateRequired;
                }
            }

            SimpleViewElementModel4WS[] simpleViewElementmodels = GetViewElementModels(moduleName, permission, sectionId);
            ICacheManager cacheManager = ObjectFactory.GetObject<ICacheManager>();
            Dictionary<string, string> state;
            RegionalModel regionalSetting = cacheManager.GetRegionalModelByCountry(countryCode, out state);
            bool useZip = regionalSetting == null ? true : !ValidationUtil.IsNo(regionalSetting.useZipCode);

            if (!ValidationUtil.ValidateRequiredValue(model, simpleViewElementmodels, useZip))
            {
                validateRequired = false;
            }

            return validateRequired && validateLicenseType;
        }

        /// <summary>
        /// Validate required fields' value in license professional list.
        /// </summary>
        /// <param name="moduleName">module name</param>
        /// <param name="sectionId">section id</param>
        /// <param name="licensePros">license professional model array</param>
        /// <returns>True or False</returns>
        public static bool ValidateFields4LPList(string moduleName, string sectionId, LicenseProfessionalModel[] licensePros)
        {
            bool validateRequired = true;

            if (licensePros != null && licensePros.Length > 0)
            {
                foreach (LicenseProfessionalModel licenseProfessional in licensePros)
                {
                    GFilterScreenPermissionModel4WS permission = new GFilterScreenPermissionModel4WS()
                                                                 {
                                                                     permissionLevel = GViewConstant.PERMISSION_PEOPLE,
                                                                     permissionValue = licenseProfessional.licenseType
                                                                 };

                    validateRequired = ValidateFields4Object(moduleName, permission, sectionId, licenseProfessional);

                    if (!validateRequired)
                    {
                        break;
                    }
                }
            }

            //it need validate template fields required
            if (validateRequired)
            {
                if (licensePros != null && licensePros.Length > 0)
                {
                    foreach (LicenseProfessionalModel licenseProfessional in licensePros)
                    {
                        if (licenseProfessional != null && !ValidateFields4Template(licenseProfessional.templateAttributes))
                        {
                            return false;
                        }
                    }
                }
            }

            return validateRequired;
        }

        /// <summary>
        /// Validate required fields' value in contact list.
        /// </summary>
        /// <param name="moduleName">module name</param>
        /// <param name="sectionId">section id</param>
        /// <param name="contacts">contact model array</param>
        /// <param name="contactSectionPosition">contact section position information</param>
        /// <returns>True or False</returns>
        public static bool ValidateFields4ContactList(string moduleName, string sectionId, CapContactModel4WS[] contacts, ACAConstant.ContactSectionPosition contactSectionPosition)
        {
            bool validateRequired = true;

            if (contacts != null && contacts.Length > 0)
            {
                foreach (CapContactModel4WS contact in contacts)
                {
                    if (contact.people == null || string.IsNullOrEmpty(contact.people.contactType))
                    {
                        validateRequired = false;
                        break;
                    }

                    GFilterScreenPermissionModel4WS permission =
                        ControlBuildHelper.GetPermissionWithGenericTemplate(sectionId, GViewConstant.PERMISSION_PEOPLE, contact.people.contactType, contact.people.template);
                    
                    //Add contact address list fields required validation.
                    validateRequired = ValidateFields4Object(moduleName, permission, sectionId, contact)
                        && ValidateFields4ContactAddressList(moduleName, contact.people.contactAddressList, GviewID.ContactAddress, contactSectionPosition);
                    
                    if (!validateRequired)
                    {
                        break;
                    }
                }
            }
            
            //it need validate template fields required
            if (validateRequired)
            {
                if (contacts != null && contacts.Length > 0)
                {
                    foreach (CapContactModel4WS contact in contacts)
                    {
                        if (contact != null && contact.people != null
                            && (!ValidateFields4Template(contact.people.attributes) || !ValidateFields4GenericTemplate(contact.people.template, moduleName)))
                        {
                            return false;
                        }
                    }
                }
            }

            return validateRequired;
        }

        /// <summary>
        /// validate template fields required
        /// </summary>
        /// <param name="fields">templateAttributeModel array.</param>
        /// <returns>true or false</returns>
        public static bool ValidateFields4Template(TemplateAttributeModel[] fields)
        {
            if (fields == null || fields.Length == 0)
            {
                return true;
            }

            bool validateRequired = true;

            foreach (TemplateAttributeModel field in fields)
            {
                if (field == null
                    || string.IsNullOrEmpty(field.vchFlag)
                    || ACAConstant.COMMON_N.Equals(field.vchFlag, StringComparison.InvariantCultureIgnoreCase))
                {
                    continue;
                }

                //if field is required but field's value is empty.
                if (ACAConstant.COMMON_Y.Equals(field.attributeValueReqFlag, StringComparison.InvariantCulture) && string.IsNullOrEmpty(field.attributeValue))
                {
                    validateRequired = false;
                    break;
                }
            }

            return validateRequired;
        }

        /// <summary>
        /// validate ASIT fields required
        /// </summary>
        /// <param name="table">AppSpecificTableModel4WS asit model</param>
        /// <param name="moduleName">module name</param>
        /// <returns>true or false</returns>
        public static bool ValidateFields4ASIT(AppSpecificTableModel4WS table, string moduleName)
        {
            bool validateRequired = true;

            if (table.tableField != null && table.tableField.Length > 0)
            {
                foreach (AppSpecificTableField4WS field in table.tableField)
                {
                    if (field != null)
                    {
                        foreach (AppSpecificTableColumnModel4WS column in table.columns)
                        {
                            if (string.Equals(column.columnName, field.fieldLabel, StringComparison.OrdinalIgnoreCase))
                            {
                                string asitSecurity = ASISecurityUtil.GetASITSecurity(column.servProvCode, column.groupName, column.tableName, column.columnName, moduleName);

                                //if field is required and visible and editable, but field's value is empty.
                                if (field.required
                                    && string.IsNullOrEmpty(field.inputValue)
                                    && ACAConstant.ASISecurity.Full.Equals(asitSecurity, StringComparison.OrdinalIgnoreCase)  
                                    && ValidationUtil.IsYes(column.vchDispFlag))
                                {
                                    validateRequired = false;
                                    break;
                                }
                            }
                        }
                    }
                }
            }

            return validateRequired;
        }   

        /// <summary>
        /// Get simple view element models by module name and section id.
        /// </summary>
        /// <param name="moduleName">module name</param>
        /// <param name="permission">the permission</param>
        /// <param name="sectionId">section id</param>
        /// <returns>simple view element models</returns>
        public static SimpleViewElementModel4WS[] GetViewElementModels(string moduleName, GFilterScreenPermissionModel4WS permission, string sectionId)
        {
            //Get simple view element models by module name and sectio Id.
            IGviewBll gviewBll = (IGviewBll)ObjectFactory.GetObject(typeof(IGviewBll));
            SimpleViewElementModel4WS[] simpleViewElementmodels = gviewBll.GetSimpleViewElementModel(moduleName, permission, sectionId, AppSession.User.UserID);
            
            return simpleViewElementmodels;
        }

        /// <summary>
        /// Validate contact address fields.
        /// </summary>
        /// <param name="moduleName">module name</param>
        /// <param name="contactAddresses">associated contact addresses</param>
        /// <param name="sectionID">Specific the Section ID/View ID which need to validate.</param>
        /// <param name="contactSectionPosition">contact section position information</param>
        /// <returns>pass or fail</returns>
        public static bool ValidateFields4ContactAddressList(string moduleName, IEnumerable<ContactAddressModel> contactAddresses, string sectionID, ACAConstant.ContactSectionPosition contactSectionPosition)
        {
            bool validateRequired = true;

            if (contactAddresses != null 
                && StandardChoiceUtil.IsEnableContactAddressEdit()
                && (ACAConstant.ContactSectionPosition.SpearForm.Equals(contactSectionPosition) || StandardChoiceUtil.IsEnableContactAddressMaintenance()))
            {
                GFilterScreenPermissionModel4WS permission = new GFilterScreenPermissionModel4WS()
                {
                    permissionLevel = GViewConstant.SECTION_CONTACT_ADDRESS,
                };

                validateRequired = ValidateFields4List(moduleName, permission, sectionID, contactAddresses);
            }

            return validateRequired;
        }

        /// <summary>
        /// Validate required contact address type.
        /// </summary>
        /// <param name="contactType">current contact type</param>
        /// <param name="contactAddresses">associated contact addresses</param>
        /// <returns>error message</returns>
        public static string ValidateContactAddressType(string contactType, IEnumerable<ContactAddressModel> contactAddresses)
        {
            if (string.IsNullOrEmpty(contactType))
            {
                return string.Empty;
            }

            string errorMsg = string.Empty;
            List<string> message = new List<string>();
            List<string> associatedAddresses = new List<string>();
            ICacheManager cacheManager = (ICacheManager)ObjectFactory.GetObject(typeof(ICacheManager));
            IEnumerable<string> requiredAddressTypes = cacheManager.GetRequiredContactAddressType(contactType);

            if (requiredAddressTypes == null)
            {
                return string.Empty;
            }

            if (contactAddresses != null)
            {
                var activatedContactAddress = contactAddresses.Where(ca =>
                    ca.auditModel == null
                    || !ACAConstant.INVALID_STATUS.Equals(ca.auditModel.auditStatus, StringComparison.OrdinalIgnoreCase));

                foreach (ContactAddressModel address in activatedContactAddress)
                {
                    associatedAddresses.Add(address.addressType);
                }
            }

            foreach (string addressType in requiredAddressTypes)
            {
                if (!associatedAddresses.Contains(addressType))
                {
                    string i18nAddressType = StandardChoiceUtil.GetStandardChoiceValueByKey(BizDomainConstant.STD_CAT_CONTACT_ADDRESS_TYPE, addressType);
                    message.Add(i18nAddressType);
                }
            }

            if (message.Count > 0)
            {
                string validateAddressTypeTitle = LabelUtil.GetTextByKey("aca_contactaddress_msg_validateaddresstype", null);
                errorMsg = string.Format("{0} {1}.", validateAddressTypeTitle, DataUtil.ConcatStringListWithComma(message));
            }
            
            return errorMsg;
        }

        /// <summary>
        /// Validate contact address type required.
        /// </summary>
        /// <param name="contacts">cap contact model</param>
        /// <returns>Validate or not</returns>
        public static bool ValidateContactAddressType(IEnumerable<CapContactModel4WS> contacts)
        {
            bool validateRequired = true;

            if (contacts != null && contacts.Any())
            {
                foreach (CapContactModel4WS contact in contacts)
                {
                    if (contact.people == null)
                    {
                        continue;
                    }

                    validateRequired = string.IsNullOrEmpty(ValidateContactAddressType(contact.people.contactType, contact.people.contactAddressList));

                    if (!validateRequired)
                    {
                        break;
                    }
                }
            }

            return validateRequired;
        }

        /// <summary>
        /// Gets a value indicating whether Need Validate primary contact address.
        /// </summary>
        /// <param name="isEditable">is editable.</param>
        /// <param name="contactSectionPosition">The contact section position.</param>
        /// <param name="contactType">The contact type.</param>
        /// <param name="contactAddressList">The contact address list.</param>
        /// <returns>true or false.</returns>
        public static bool IsNeedValidateCAPrimary(bool isEditable, ACAConstant.ContactSectionPosition contactSectionPosition, string contactType, IList<ContactAddressModel> contactAddressList)
        {
            bool isNeedValidatePrimary = false;

            if (isEditable && ACAConstant.ContactSectionPosition.SpearForm.Equals(contactSectionPosition) && StandardChoiceUtil.IsPrimaryContactAddressRequired()
                && !string.IsNullOrEmpty(contactType) && (contactAddressList == null || !contactAddressList.Any() || !contactAddressList.Any(c => ACAConstant.COMMON_Y.Equals(c.primary))))
            {
                isNeedValidatePrimary = true;
            }

            return isNeedValidatePrimary;
        }

        /// <summary>
        /// Gets a value indicating whether passed the primary contact address validation.
        /// </summary>
        /// <param name="isEditable">the contact is editable</param>
        /// <param name="contactSectionPosition">contact section position</param>
        /// <param name="contacts">The contact list.</param>
        /// <returns>true or false.</returns>
        public static bool ValidateCAPrimary(bool isEditable, ACAConstant.ContactSectionPosition contactSectionPosition, IEnumerable<CapContactModel4WS> contacts)
        {
            bool isValidate = true;

            if (isEditable && ACAConstant.ContactSectionPosition.SpearForm.Equals(contactSectionPosition)
                && StandardChoiceUtil.IsPrimaryContactAddressRequired() && contacts != null)
            {
                foreach (var contact in contacts)
                {
                    if (contact == null || contact.people == null)
                    {
                        continue;
                    }

                    IList<ContactAddressModel> contactAddressList = contact.people.contactAddressList;

                    if (contactAddressList == null || !contactAddressList.Any() ||
                        !contactAddressList.Any(c => ACAConstant.COMMON_Y.Equals(c.primary)))
                    {
                        isValidate = false;
                    }
                }
            }

            return isValidate;
        }

        /// <summary>
        /// Gets a value indicating whether passed the primary contact address validation.
        /// </summary>
        /// <param name="isEditable">the contact is editable</param>
        /// <param name="contactSectionPosition">contact section position</param>
        /// <param name="contacts">The contact list.</param>
        /// <returns>true or false.</returns>
        public static bool ValidateContactListAddressType(bool isEditable, ACAConstant.ContactSectionPosition contactSectionPosition, IEnumerable<CapContactModel4WS> contacts)
        {
            if (isEditable && ACAConstant.ContactSectionPosition.SpearForm.Equals(contactSectionPosition) && contacts != null)
            {
                foreach (CapContactModel4WS contact in contacts)
                {
                    if (contact == null || contact.people == null || !string.IsNullOrEmpty(contact.refContactNumber))
                    {
                        continue;
                    }

                    IEnumerable<ContactAddressModel> contactAddressList = contact.people.contactAddressList;
                    string msg = ValidateContactAddressType(contact.people.contactType, contactAddressList);

                    if (!string.IsNullOrWhiteSpace(msg))
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        /// <summary>
        /// Validates the license certification list(Continue Education, Education, Examination).
        /// include generic template validate and standard field.
        /// </summary>
        /// <param name="moduleName">Name of the module.</param>
        /// <param name="permission">The form designer permission object.</param>
        /// <param name="sectionId">The section view id.</param>
        /// <param name="models">The need validation's models.</param>
        /// <returns>
        /// Validate or not
        /// </returns>
        public static bool ValidateLicenseCertificationList(string moduleName, GFilterScreenPermissionModel4WS permission, string sectionId, IEnumerable<object> models)
        {
            bool validatePassed = true;

            if (models != null)
            {
                foreach (var licenseCertificationModel in models)
                {
                    string refId = string.Empty;
                    TemplateModel templateModel = null;
                    bool isApprovedOrUsed = false;

                    if (licenseCertificationModel is EducationModel4WS)
                    {
                        var eduModel = licenseCertificationModel as EducationModel4WS;
                        refId = eduModel.RefEduNbr;
                        isApprovedOrUsed = ValidationUtil.IsYes(eduModel.approvedFlag) || eduModel.associatedEduCount > 0;

                        if (string.IsNullOrEmpty(refId))
                        {
                            RefEducationModel4WS refEducation =
                                EducationUtil.GetRefEducationModel(
                                    eduModel.educationPKModel.serviceProviderCode,
                                    eduModel.educationName);

                            if (refEducation != null)
                            {
                                refId = refEducation.refEducationNbr.ToString();
                            }
                        }

                        templateModel = eduModel.template;
                    }
                    else if (licenseCertificationModel is ContinuingEducationModel4WS)
                    {
                        var contEduModel = licenseCertificationModel as ContinuingEducationModel4WS;
                        refId = contEduModel.RefConEduNbr;
                        isApprovedOrUsed = ValidationUtil.IsYes(contEduModel.approvedFlag) || contEduModel.associatedContEduCount > 0;

                        if (string.IsNullOrEmpty(refId))
                        {
                            RefContinuingEducationModel4WS refContEducation =
                                EducationUtil.GetRefContinuingEducationModel(
                                    contEduModel.continuingEducationPKModel.serviceProviderCode,
                                    contEduModel.contEduName);

                            if (refContEducation != null)
                            {
                                refId = refContEducation.refContEduNbr.ToString();
                            }
                        }

                        templateModel = contEduModel.template;
                    }
                    else if (licenseCertificationModel is ExaminationModel)
                    {
                        var examModel = licenseCertificationModel as ExaminationModel;
                        refId = examModel.refExamSeq.ToString();
                        templateModel = examModel.template;
                        isApprovedOrUsed = ValidationUtil.IsYes(examModel.approvedFlag) || examModel.associatedExamCount > 0;

                        //Need not validate for examination which are Pending status.
                        if (ACAConstant.EXAMINATION_STATUS_PENDING.Equals(examModel.examStatus, StringComparison.InvariantCultureIgnoreCase))
                        {
                            continue;
                        }
                    }

                    permission.permissionValue = refId;

                    if (templateModel != null && templateModel.templateForms != null && templateModel.templateForms.Length != 0)
                    {
                        permission.permissionValue += ACAConstant.SPLIT_DOUBLE_COLON + templateModel.templateForms[0].groupName;
                    }

                    validatePassed = isApprovedOrUsed
                                     || (ValidateFields4Object(moduleName, permission, sectionId, licenseCertificationModel)
                                         && ValidateFields4GenericTemplate(templateModel, moduleName));

                    if (!validatePassed)
                    {
                        break;
                    }
                }
            }

            return validatePassed;
        }

        /// <summary>
        /// validate generic template fields required.
        /// </summary>
        /// <param name="templateModel">The template model.</param>
        /// <param name="moduleName">Name of the module.</param>
        /// <returns>
        /// true or false
        /// </returns>
        public static bool ValidateFields4GenericTemplate(TemplateModel templateModel, string moduleName)
        {
            bool validateRequired = true;

            if (templateModel != null)
            {
                // validate field for Generic template
                var fields = GenericTemplateUtil.GetAllFields(templateModel);

                if (fields != null)
                {
                    foreach (var field in fields)
                    {
                        if (field == null)
                        {
                            continue;
                        }

                        string asiSecurity = ASISecurityUtil.GetASISecurity(
                                      field.serviceProviderCode,
                                      field.groupName,
                                      field.subgroupName,
                                      field.fieldName,
                                      moduleName);

                        if (!field.readOnly &&
                            (field.acaTemplateConfig != null && !ACAConstant.COMMON_N.Equals(field.acaTemplateConfig.acaDisplayFlag) && !ACAConstant.COMMON_H.Equals(field.acaTemplateConfig.acaDisplayFlag))
                            && (!ACAConstant.ASISecurity.None.Equals(asiSecurity) && !ACAConstant.ASISecurity.Read.Equals(asiSecurity))
                            && ACAConstant.COMMON_Y.Equals(field.requireFlag, StringComparison.InvariantCulture)
                            && string.IsNullOrEmpty(field.defaultValue))
                        {
                            //if field is required but field's value is empty.
                            validateRequired = false;
                            break;
                        }
                    }
                }

                // validate field for Generic template table
                TemplateSubgroup[] templateTabeles = CapUtil.GetGenericTemplateTables(moduleName, templateModel.templateTables);

                if (templateTabeles == null || templateTabeles.Length == 0)
                {
                    return validateRequired;
                }

                Dictionary<string, bool> fieldsNeedValidate = new Dictionary<string, bool>();
                var allColumns = from form in templateTabeles
                                     where form != null && form.fields != null
                                     from field in form.fields
                                     where field != null
                                     select field;

                foreach (var column in allColumns)
                {
                    string asitStecurity = ASISecurityUtil.GetASITSecurity(
                                      column.serviceProviderCode,
                                      column.groupName,
                                      column.subgroupName,
                                      column.fieldName,
                                      moduleName);

                    bool isNeedValidate = (!column.readOnly && column.acaTemplateConfig != null
                                           && !ACAConstant.COMMON_N.Equals(column.acaTemplateConfig.acaDisplayFlag)
                                           && !ACAConstant.COMMON_H.Equals(column.acaTemplateConfig.acaDisplayFlag))
                                          && (!ACAConstant.ASISecurity.None.Equals(asitStecurity) && !ACAConstant.ASISecurity.Read.Equals(asitStecurity))
                                          && ACAConstant.COMMON_Y.Equals(column.requireFlag, StringComparison.InvariantCulture);

                    fieldsNeedValidate.Add(column.groupName + column.subgroupName + column.fieldName, isNeedValidate);
                }

                var allFields = from table in templateTabeles
                                where table != null && table.rows != null
                                from row in table.rows
                                where row != null && row.values != null
                                from value in row.values
                                where value != null
                                select value;

                foreach (var value in allFields)
                {
                    if (!fieldsNeedValidate[value.groupName + value.subgroupName + value.fieldName])
                    {
                        continue;
                    }

                    if (string.IsNullOrWhiteSpace(value.value))
                    {
                        validateRequired = false;
                        break;
                    }
                }
            }

            return validateRequired;
        }
    }
}