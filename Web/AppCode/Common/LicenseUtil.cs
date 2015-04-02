#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: LicenseUtil.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2009-2014
 *
 *  Description:
 *  License Utilities.
 *
 *  Notes:
 *      $Id: LicenseUtil.cs 144292 2009-08-25 14:09:43Z ACHIEVO\vera.zhao $.
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
using System.Text.RegularExpressions;
using Accela.ACA.BLL.AddressBuilder;
using Accela.ACA.BLL.APO;
using Accela.ACA.BLL.Cap;
using Accela.ACA.BLL.Common;
using Accela.ACA.BLL.People;
using Accela.ACA.Common;
using Accela.ACA.Common.Util;
using Accela.ACA.Web.Component;
using Accela.ACA.Web.UserInfo;
using Accela.ACA.WSProxy;

namespace Accela.ACA.Web.Common
{
    /// <summary>
    /// License utility class
    /// </summary>
    public static class LicenseUtil
    {
        /// <summary>
        /// Indicate how many largest experience records will be show in Certified Businesses download file.
        /// </summary>
        public const int EXPERIENCES_TOP_NUMBER = 3;

        /// <summary>
        /// Check if expired license is enable to use or not
        /// </summary>
        /// <returns>true if expired license is enable to use; otherwise,false.</returns>
        public static bool EnableExpiredLicense()
        {
            return IsYes4EnableExpiredLicByValue(BizDomainConstant.STD_ITEM_ENABLE_EXPIRED_LICENSE);
        }

        /// <summary>
        /// Check if the license is unavailable.
        /// </summary>
        /// <param name="license">LicenseModel4WS need be checked.</param>
        /// <param name="capType">CapTypeModel object that relates a CAP</param>
        /// <returns>true if the license is available;otherwise,false.</returns>
        public static bool IsAvailableLicense(LicenseModel4WS license, CapTypeModel capType)
        {
            if (!EnableExpiredLicense() || license == null || string.IsNullOrEmpty(license.stateLicense))
            {
                return true;
            }

            bool isAvailableLic = true;

            LicenseModel4WS[] licenses = new LicenseModel4WS[1];
            licenses[0] = license;

            ILicenseBLL licenseBll = ObjectFactory.GetObject(typeof(ILicenseBLL)) as ILicenseBLL;
            LicenseModel4WS[] licenseList = licenseBll.GetLicPermissionByLPTypeAndCapType(licenses, capType);

            if (licenseList != null && licenseList.Length > 0)
            {
                isAvailableLic = IsAvailableLicense(licenseList[0]);
            }

            return isAvailableLic;
        }

        /// <summary>
        /// Check if the license is available to use.
        /// </summary>
        /// <param name="licenseModel">LicenseModel4WS need be checked</param>
        /// <returns>true if it's available(not expired or expired but allow to use); otherwise,false.</returns>
        public static bool IsAvailableLicense(LicenseModel4WS licenseModel)
        {
            bool isAvailable = true;

            if (licenseModel != null)
            {
                bool isLicExpired = licenseModel.licExpired;
                bool isInsExpired = licenseModel.insExpired;
                bool isBizLicExpired = licenseModel.bizLicExpired;

                RecordTypeLicTypePermissionModel4WS licTypePermission = licenseModel.licTypePermission;

                // either license or insurance is not available, or both of them are not available.
                if (!IsAvailableLicense(isLicExpired, isInsExpired, isBizLicExpired, licTypePermission))
                {
                    isAvailable = false;
                }
            }

            return isAvailable;
        }

        /// <summary>
        /// check if the license is available.
        /// </summary>
        /// <param name="islicExpired">true if license is expired</param>
        /// <param name="isInsExpired">true if insurance is expired</param>
        /// <param name="isBizLicExpired">true if business license is expired</param>
        /// <param name="licTypePermission"><c>RecordTypeLicTypePermissionModel4WS</c> object</param>
        /// <returns>true if the license is available;otherwise,false.</returns>
        public static bool IsAvailableLicense(bool islicExpired, bool isInsExpired, bool isBizLicExpired, RecordTypeLicTypePermissionModel4WS licTypePermission)
        {
            bool isAvailable = true;

            if (licTypePermission == null)
            {
                // no role-based permissions, don't allow to use expired license.
                if (islicExpired || isInsExpired || isBizLicExpired)
                {
                    isAvailable = false;
                }
            }
            else
            {
                if ((islicExpired && !licTypePermission.licExpEnabled4ACA) ||
                    (isInsExpired && !licTypePermission.insExpEnabled4ACA) ||
                     (isBizLicExpired && !licTypePermission.bizLicExpEnabled4ACA))
                {
                    isAvailable = false;
                }
            }

            return isAvailable;
        }

        /// <summary>
        /// Check if the license is expired.
        /// </summary>
        /// <param name="license">LicenseModel4WS object.</param>
        /// <returns>true if the license is expired;otherwise,return false.</returns>
        public static bool IsExpiredLicense(LicenseModel4WS license)
        {
            bool isLicExpried = false;

            if (license != null && EnableExpiredLicense())
            {
                if (license.licExpired || license.insExpired || license.bizLicExpired)
                {
                    isLicExpried = true;
                }
            }

            return isLicExpried;
        }

        /// <summary>
        /// Check current user's licenses, get expired license number.
        /// </summary>
        /// <returns>String list for the user's expired license(s) number.</returns>
        public static IList<string> GetExpiredLicNum4User()
        {
            if (AppSession.IsAdmin ||
                AppSession.User.IsAnonymous ||
                !EnableExpiredLicense())
            {
                return null;
            }

            LicenseModel4WS[] licenses = AppSession.User.Licenses;
            IList<string> listExpiredLicNums = new List<string>();

            if (licenses != null)
            {
                bool enableNotifyExpLic = IsYes4EnableExpiredLicByValue(BizDomainConstant.STD_ITEM_ENABLE_NOTIFY_EXPIRED_LICENSE_AT_LOGIN);
                bool enableNotifyExpIns = IsYes4EnableExpiredLicByValue(BizDomainConstant.STD_ITEM_ENABLE_NOTIFY_EXPIRED_INSURANCE_AT_LOGIN);
                bool enableNotifyExpBusLic = IsYes4EnableExpiredLicByValue(BizDomainConstant.STD_ITEM_ENABLE_NOTIFY_EXPIRED_BUSINESS_LICENSE_AT_LOGIN);

                // check the license is expired or not
                foreach (LicenseModel4WS license in licenses)
                {
                    if (license == null)
                    {
                        continue;
                    }

                    if ((license.licExpired && enableNotifyExpLic) ||
                        (license.insExpired && enableNotifyExpIns) ||
                        (license.bizLicExpired && enableNotifyExpBusLic))
                    {
                        listExpiredLicNums.Add(license.stateLicense);
                    }
                }
            }

            return listExpiredLicNums;
        }

        /// <summary>
        /// Check LPs for CAPs, get CAPs that has invalid LPs.
        /// </summary>
        /// <param name="capModels">CapModel4WS array</param>
        /// <returns>CAP ids with invalid LPs.</returns>
        public static string GetInvalidCapIDsByCheckLicense(CapModel4WS[] capModels)
        {
            if (!EnableExpiredLicense() || capModels == null)
            {
                return string.Empty;
            }

            StringBuilder sbInvalidCapIDs = new StringBuilder();

            ILicenseBLL licenseBll = ObjectFactory.GetObject(typeof(ILicenseBLL)) as ILicenseBLL;
            CapIDModel4WS[] invalidCapIDs = licenseBll.GetInvalidCapIDsByCheckLicense(capModels);

            if (invalidCapIDs != null && invalidCapIDs.Length > 0)
            {
                foreach (CapIDModel4WS capIDModel in invalidCapIDs)
                {
                    if (capIDModel != null)
                    {
                        sbInvalidCapIDs.Append(capIDModel.customID);
                        sbInvalidCapIDs.Append(ACAConstant.COMMA);
                        sbInvalidCapIDs.Append(ACAConstant.BLANK);
                    }
                }

                if (sbInvalidCapIDs.Length > 1)
                {
                    sbInvalidCapIDs.Remove(sbInvalidCapIDs.Length - 2, 2); // remove the last comma and black.
                }
            }

            return sbInvalidCapIDs.ToString();
        }

        /// <summary>
        /// Get expired and unavailable license numbers for the CAP.
        /// </summary>
        /// <param name="capModel">CapModel4WS object.</param>
        /// <returns>the expired and unavailable license number.If no unavailable licenses,return empty string.</returns>
        public static string GetUnAvailableLPNums4Permit(CapModel4WS capModel)
        {
            if (!EnableExpiredLicense() || capModel == null)
            {
                return string.Empty;
            }

            string unAvailaleLicNums = string.Empty;
            LicenseProfessionalModel licenseProfessional = TempModelConvert.ConvertToLicenseProfessionalModel(capModel.licenseProfessionalModel);

            // Check single LP
            if (licenseProfessional != null)
            {
                LicenseModel4WS license = new LicenseModel4WS();
                license.stateLicense = licenseProfessional.licenseNbr;
                license.licenseType = licenseProfessional.licenseType;
                license.serviceProviderCode = ConfigManager.AgencyCode;

                if (!IsAvailableLicense(license, capModel.capType))
                {
                    //return license.stateLicense;
                    unAvailaleLicNums = license.stateLicense;
                }
            }
            else
            {
                // Check multiple LPs.
                LicenseProfessionalModel[] licenseProfessionals = TempModelConvert.ConvertToLicenseProfessionalModelList(capModel.licenseProfessionalList);

                if (licenseProfessionals != null)
                {
                    unAvailaleLicNums = GetUnAvailableLPNums(licenseProfessionals, capModel.capType);
                }
            }

            return unAvailaleLicNums;
        }

        /// <summary>
        /// Check licenses to get the expired and unavailable license numbers.
        /// </summary>
        /// <param name="licenseProfessionals">LicenseProfessionalModel array need be checked</param>
        /// <param name="capType">CapTypeModel object</param>
        /// <returns>the expired and unavailable license number.</returns>
        public static string GetUnAvailableLPNums(LicenseProfessionalModel[] licenseProfessionals, CapTypeModel capType)
        {
            if (licenseProfessionals == null || licenseProfessionals.Length == 0)
            {
                return string.Empty;
            }

            //Convert LicenseProfessionalModel to LicenseModel4WS.
            IList<LicenseModel4WS> licenseList = new List<LicenseModel4WS>();

            foreach (LicenseProfessionalModel lp in licenseProfessionals)
            {
                if (lp != null && lp.licenseNbr != null && lp.licenseType != null)
                {
                    LicenseModel4WS license = new LicenseModel4WS();
                    license.stateLicense = lp.licenseNbr;
                    license.licenseType = lp.licenseType;
                    license.serviceProviderCode = ConfigManager.AgencyCode;

                    licenseList.Add(license);
                }
            }

            if (0 == licenseList.Count)
            {
                return string.Empty;
            }

            LicenseModel4WS[] licenses = new LicenseModel4WS[licenseList.Count];
            licenseList.CopyTo(licenses, 0);

            // Get LicenseModel4WS array with user-role based permission.
            ILicenseBLL licenseBll = ObjectFactory.GetObject(typeof(ILicenseBLL)) as ILicenseBLL;
            LicenseModel4WS[] licenseModels = licenseBll.GetLicPermissionByLPTypeAndCapType(licenses, capType);

            StringBuilder sbUnAvailaleLicNums = new StringBuilder();

            if (licenseModels != null && licenseModels.Length > 0)
            {
                foreach (LicenseModel4WS license in licenseModels)
                {
                    if (!IsAvailableLicense(license))
                    {
                        sbUnAvailaleLicNums.Append(license.stateLicense);
                        sbUnAvailaleLicNums.Append(ACAConstant.COMMA_BLANK);
                    }
                }
            }

            if (sbUnAvailaleLicNums.Length > 1)
            {
                sbUnAvailaleLicNums.Remove(sbUnAvailaleLicNums.Length - 2, 2); // remove the last ","
            }

            return sbUnAvailaleLicNums.ToString();
        }

        /// <summary>
        /// Get label key for license expiration status
        /// </summary>
        /// <param name="isLicExpired">true if license is expired.</param>
        /// <returns>license expiration status-Expired or Valid</returns>
        public static string GetLicenseStatus(bool isLicExpired)
        {
            string key = isLicExpired ? "licenselist_licensestatus_expired" : "licenselist_licensestatus_valid";

            return LabelUtil.GetTextByKey(key, string.Empty);
        }

        /// <summary>
        /// Get licensees from CAP model, which have same licensee type and license number.
        /// </summary>
        /// <param name="permitLicensees">licensees from session CAP</param>
        /// <param name="curLicensee">user selected licensee from page</param>
        /// <param name="needContainEMSEAttribute">need contains emse attribute or not</param>
        /// <returns>licensed professional model list</returns>
        public static LicenseProfessionalModel[] GetSameTypeNumberLicenses(LicenseProfessionalModel[] permitLicensees, LicenseProfessionalModel curLicensee, bool needContainEMSEAttribute)
        {
            List<LicenseProfessionalModel> licensees = new List<LicenseProfessionalModel>();
            licensees.Add(curLicensee);

            if (permitLicensees != null && permitLicensees.Length > 0)
            {
                foreach (LicenseProfessionalModel licensee in permitLicensees)
                {
                    if (licensee == null || string.IsNullOrEmpty(licensee.licenseType)
                        || string.IsNullOrEmpty(licensee.licenseNbr) || string.IsNullOrEmpty(licensee.agencyCode))
                    {
                        continue;
                    }

                    if (licensee.licenseType.Equals(curLicensee.licenseType, StringComparison.InvariantCulture)
                        && licensee.licenseNbr.Equals(curLicensee.licenseNbr, StringComparison.InvariantCulture)
                        && !licensee.agencyCode.Equals(curLicensee.agencyCode, StringComparison.InvariantCulture)
                        && ((needContainEMSEAttribute && CapUtil.IsContainsEMSEAttribute(licensee.templateAttributes)) || !needContainEMSEAttribute))
                    {
                        LicenseProfessionalModel license = ObjectCloneUtil.DeepCopy<LicenseProfessionalModel>(curLicensee);
                        license.agencyCode = licensee.agencyCode;
                        licensees.Add(license);
                    }
                }
            }

            return licensees.ToArray();
        }

        /// <summary>
        /// research licensee template for each licensee.
        /// </summary>
        /// <param name="licensees">licensed professional models</param>
        /// <param name="moduleName">module name</param>
        /// <returns>The licensed professional models</returns>
        public static LicenseProfessionalModel[] ResearchLicenseTemplates(LicenseProfessionalModel[] licensees, string moduleName)
        {
            if (licensees == null || licensees.Length <= 0)
            {
                return licensees;
            }

            ITemplateBll _templateBll = (ITemplateBll)ObjectFactory.GetObject(typeof(ITemplateBll));

            foreach (LicenseProfessionalModel licensee in licensees)
            {
                if (licensee.templateAttributes == null || licensee.templateAttributes.Length == 0)
                {
                    string agency = string.IsNullOrEmpty(licensee.agencyCode) ? ConfigManager.AgencyCode : licensee.agencyCode;
                    licensee.templateAttributes =
                        _templateBll.GetDailyPeopleTemplateAttributes(licensee.licenseType, AppSession.GetCapModelFromSession(moduleName).capID, licensee.licenseNbr, agency, AppSession.User.PublicUserId);
                }

                // null indicates that the cap is creating and enter cap edit page first time.
                if (licensee.templateAttributes == null)
                {
                    licensee.templateAttributes = GetOriginalLicenseTemplate(licensee.agencyCode, licensee.licenseType, licensee.licenseNbr);

                    //populate the selected license if user select the license before selecting cap type
                    licensee.templateAttributes = FillRefLicenseTemplate(licensee.agencyCode, licensee.licenseType, licensee.licSeqNbr, licensee.templateAttributes);
                }
            }

            return licensees;
        }

        /// <summary>
        /// Reset the agency code to the specified service provider code for super agency when the its own agency code is null.
        /// </summary>
        /// <param name="oldLicenseProfessionalList">The license professional list that will be checked.</param>
        /// <param name="serviceProviderCode">The service provider code.</param>
        /// <returns>The license professional model list that reset.</returns>
        public static LicenseProfessionalModel[] ResetLicenseeAgency(LicenseProfessionalModel4WS[] oldLicenseProfessionalList, string serviceProviderCode)
        {
            if (oldLicenseProfessionalList == null)
            {
                return null;
            }

            LicenseProfessionalModel[] licensees = TempModelConvert.ConvertToLicenseProfessionalModelList(oldLicenseProfessionalList);

            if (StandardChoiceUtil.IsSuperAgency() 
                && licensees != null 
                && licensees.Length > 0 
                && !string.IsNullOrEmpty(serviceProviderCode))
            {
                foreach (LicenseProfessionalModel lp in licensees)
                {
                    lp.agencyCode = string.IsNullOrEmpty(lp.agencyCode) ? serviceProviderCode : lp.agencyCode;
                }
            }

            return licensees;
        }

        /// <summary>
        /// Get field value stored in forms of TemplateModel.
        /// </summary>
        /// <param name="template">TemplateModel containing fields value.</param>
        /// <param name="fieldName">Field name</param>
        /// <returns>value from template forms</returns>
        public static object GetValueFromTemplateFroms(TemplateModel template, string fieldName)
        {
            if (template == null || template.templateForms == null)
            {
                return null;
            }

            object value = null;

            var filedValue = (from groups in template.templateForms
                              where groups != null && groups.subgroups != null
                              from subGroup in groups.subgroups
                              where subGroup != null && subGroup.fields != null
                              from field in subGroup.fields
                              where field != null && field.defaultValue != null && string.Equals(field.displayFieldName, fieldName, StringComparison.InvariantCultureIgnoreCase)
                              select field.defaultValue).SingleOrDefault();
            value = filedValue;

            return value;
        }

        /// <summary>
        /// Get field value stored in table of TemplateModel.
        /// </summary>
        /// <param name="template">TemplateModel to be searched.</param>
        /// <param name="fieldName">Field name</param>
        /// <returns>List from template tables</returns>
        public static List<object> GetValueFromTemplateTables(TemplateModel template, string fieldName)
        {
            if (template == null || template.templateTables == null)
            {
                return null;
            }

            List<object> resultList = new List<object>();

            var filedValue = from groups in template.templateTables
                              where groups != null && groups.subgroups != null
                              from subGroup in groups.subgroups
                              where subGroup != null && subGroup.rows != null
                              from row in subGroup.rows
                              where row != null && row.values != null
                              from value in row.values
                              where value != null && string.Equals(value.fieldName, fieldName, StringComparison.InvariantCultureIgnoreCase)
                              select value.value;

            foreach (object item in filedValue)
            {
                resultList.Add(item);
            }

            return resultList;
        }

        /// <summary>
        /// Concatenate value got from TemplateModel with comma.
        /// </summary>
        /// <param name="list">Template field value list</param>
        /// <returns>String after Concatenated.</returns>
        public static string ConcatTemplateValue(List<object> list)
        {
            if (list == null)
            {
                return null;
            }

            string concatedValue = string.Empty;

            for (int i = 0; i < list.Count; i++)
            {
                if (list[i] != null)
                {
                    concatedValue += list[i].ToString() + ACAConstant.COMMA_BLANK;
                }
            }

            if (!string.IsNullOrEmpty(concatedValue))
            {
                concatedValue = concatedValue.Substring(0, concatedValue.Length - ACAConstant.COMMA_BLANK.Length);
            }

            return concatedValue;
        }

        /// <summary>
        /// Construct a new DataTable for License.
        /// </summary>
        /// <returns>
        /// Construct licensee dataTable
        /// </returns>
        public static DataTable CreateDataTable4License()
        {
            DataTable licenseeDataTable = new DataTable();
            licenseeDataTable.Columns.Add(ColumnConstant.RefLicenseProfessional.StateLicense.ToString());
            licenseeDataTable.Columns.Add(ColumnConstant.RefLicenseProfessional.LicenseType.ToString());
            licenseeDataTable.Columns.Add(ColumnConstant.RefLicenseProfessional.TypeFlag.ToString());
            licenseeDataTable.Columns.Add(ColumnConstant.RefLicenseProfessional.MaskedSSN.ToString());
            licenseeDataTable.Columns.Add(ColumnConstant.RefLicenseProfessional.Fein.ToString());
            licenseeDataTable.Columns.Add(ColumnConstant.RefLicenseProfessional.BusinessName.ToString());
            licenseeDataTable.Columns.Add(ColumnConstant.RefLicenseProfessional.BusinessLicense.ToString());
            licenseeDataTable.Columns.Add(ColumnConstant.RefLicenseProfessional.ContactFirstName.ToString());
            licenseeDataTable.Columns.Add(ColumnConstant.RefLicenseProfessional.ContactMiddleName.ToString());
            licenseeDataTable.Columns.Add(ColumnConstant.RefLicenseProfessional.ContactLastName.ToString());
            licenseeDataTable.Columns.Add(ColumnConstant.RefLicenseProfessional.FullAddress.ToString());
            licenseeDataTable.Columns.Add(ColumnConstant.RefLicenseProfessional.LicenseExpirationDate.ToString());
            licenseeDataTable.Columns.Add(ColumnConstant.RefLicenseProfessional.InsuranceExpDate.ToString());
            licenseeDataTable.Columns.Add(ColumnConstant.RefLicenseProfessional.LicenseBoard.ToString());
            licenseeDataTable.Columns.Add(ColumnConstant.RefLicenseProfessional.LicenseIssueDate.ToString());
            licenseeDataTable.Columns.Add(ColumnConstant.RefLicenseProfessional.LicenseLastRenewalDate.ToString());
            licenseeDataTable.Columns.Add(ColumnConstant.RefLicenseProfessional.City.ToString());
            licenseeDataTable.Columns.Add(ColumnConstant.RefLicenseProfessional.State.ToString());
            licenseeDataTable.Columns.Add(ColumnConstant.RefLicenseProfessional.Zip.ToString());
            licenseeDataTable.Columns.Add(ColumnConstant.RefLicenseProfessional.Address1.ToString());
            licenseeDataTable.Columns.Add(ColumnConstant.RefLicenseProfessional.Address2.ToString());
            licenseeDataTable.Columns.Add(ColumnConstant.RefLicenseProfessional.Address3.ToString());
            licenseeDataTable.Columns.Add(ColumnConstant.RefLicenseProfessional.BusName2.ToString());
            licenseeDataTable.Columns.Add(ColumnConstant.RefLicenseProfessional.Title.ToString());
            licenseeDataTable.Columns.Add(ColumnConstant.RefLicenseProfessional.Policy.ToString());
            licenseeDataTable.Columns.Add(ColumnConstant.RefLicenseProfessional.InsuranceCo.ToString());
            licenseeDataTable.Columns.Add(ColumnConstant.RefLicenseProfessional.ResState.ToString());
            licenseeDataTable.Columns.Add(ColumnConstant.RefLicenseProfessional.CountryCode.ToString());

            return licenseeDataTable;
        }

        /// <summary>
        /// Create the address that contains address1, address2, address3, city, state, zip and country info.
        /// </summary>
        /// <param name="licensee">The license model.</param>
        /// <returns>The address information.</returns>
        public static string GetAddressDetail4License(LicenseModel4WS licensee)
        {
            string result = string.Empty;

            if (licensee != null)
            {
                string country = string.Empty;

                if (!string.IsNullOrEmpty(licensee.countryCode))
                {
                    country = StandardChoiceUtil.GetCountryByKey(licensee.countryCode);
                }

                IAddressBuilderBll addressBuilderBll = ObjectFactory.GetObject<IAddressBuilderBll>();
                result = addressBuilderBll.Build4License(licensee, country);
            }

            return result;
        }

        /// <summary>
        /// Create DataTable for certified business list.
        /// </summary>
        /// <returns>DataTable which column contains all certified business information.</returns>
        public static DataTable CreateDataTable4CertBusiness()
        {
            DataTable dt = new DataTable();

            dt.Columns.Add("businessName", typeof(string));
            dt.Columns.Add("certification", typeof(string));
            dt.Columns.Add("ownerEthnicity", typeof(string));
            dt.Columns.Add("contactLastName", typeof(string));
            dt.Columns.Add("contactFirstName", typeof(string));
            dt.Columns.Add("contactMiddleName", typeof(string));
            dt.Columns.Add("fax", typeof(string));
            dt.Columns.Add("phone1", typeof(string));
            dt.Columns.Add("email", typeof(string));
            dt.Columns.Add("city", typeof(string));
            dt.Columns.Add("state", typeof(string));
            dt.Columns.Add("zip", typeof(string));
            dt.Columns.Add("largestContract", typeof(decimal));
            dt.Columns.Add("strLargestContract", typeof(string));
            dt.Columns.Add("seqNumber", typeof(string));
            dt.Columns.Add("vendorDba", typeof(string));
            dt.Columns.Add("contactName", typeof(string));
            dt.Columns.Add("address2", typeof(string));
            dt.Columns.Add("address3", typeof(string));
            dt.Columns.Add("stateLicense", typeof(string));
            dt.Columns.Add("licenseType", typeof(string));
            dt.Columns.Add("agencyCode", typeof(string));
            dt.Columns.Add("countryCode", typeof(string));
            
            for (int i = 1; i <= EXPERIENCES_TOP_NUMBER; i++)
            {
                dt.Columns.Add("clientName" + i, typeof(string));
                dt.Columns.Add("jobValue" + i, typeof(string));
                dt.Columns.Add("workDate" + i, typeof(string));
                dt.Columns.Add("description" + i, typeof(string));
            }

            return dt;
        }

        /// <summary>
        /// set certified business DataSource  from IList to DataTable for binding on account of some values need to be got from template.
        /// </summary>
        /// <param name="licenseList">License list to be set</param>
        /// <param name="needExperience">Need experience or not.</param>
        /// <returns>DataTable of license list</returns>
        public static DataTable SetDataSource2DataTable4BindList(IList<LicenseModel> licenseList, bool needExperience)
        {
            DataTable dt = CreateDataTable4CertBusiness();

            foreach (LicenseModel license in licenseList)
            {
                DataRow dr = dt.NewRow();

                dr["businessName"] = license.businessName;
                dr["contactLastName"] = license.contactLastName;
                dr["contactFirstName"] = license.contactFirstName;
                dr["contactMiddleName"] = license.contactMiddleName;
                dr["fax"] = ModelUIFormat.FormatPhoneShow(license.faxCountryCode, license.fax, license.countryCode);
                dr["phone1"] = ModelUIFormat.FormatPhoneShow(license.phone1CountryCode, license.phone1, license.countryCode);
                dr["email"] = license.EMailAddress;
                dr["city"] = license.city;
                dr["state"] = license.state;
                dr["zip"] = license.zip;
                dr["seqNumber"] = license.licSeqNbr;

                List<object> certification = new List<object>();
                certification = GetValueFromTemplateTables(license.template, ACAConstant.NIGP_FIELD_CERTIFICATION_TYPE);
                dr["certification"] = ConcatTemplateValue(certification);

                List<object> ownerEthnicity = new List<object>();
                ownerEthnicity = GetValueFromTemplateTables(license.template, ACAConstant.NIGP_FIELD_ETHNICITY);
                dr["ownerEthnicity"] = ConcatTemplateValue(ownerEthnicity);

                object largestContract = GetValueFromTemplateFroms(license.template, ACAConstant.NIGP_FIELD_MAX_CONTRACT);
                decimal amount = 0;

                if (largestContract != null)
                {
                    decimal.TryParse(largestContract.ToString(), NumberStyles.Currency, CultureInfo.InvariantCulture, out amount);                   
                }

                dr["largestContract"] = amount;
                dr["strLargestContract"] = amount == 0 ? string.Empty : I18nNumberUtil.FormatMoneyForUI(amount);
                dr["vendorDba"] = license.busName2;
                dr["contactName"] = UserUtil.FormatToFullName(license.contactFirstName, license.contactMiddleName, license.contactLastName);
                dr["address2"] = license.address2;
                dr["address3"] = license.address3;
                dr["stateLicense"] = license.stateLicense;
                dr["licenseType"] = license.licenseType;
                dr["agencyCode"] = license.serviceProviderCode;
                dr["countryCode"] = license.countryCode;

                // get the lagerst 3 experiences and format to display
                if (needExperience)
                {
                    DataTable dtExperiences = GetExperiences(license.template, EXPERIENCES_TOP_NUMBER);

                    for (int i = 0; i < dtExperiences.Rows.Count; i++)
                    {
                        int j = i + 1;
                        string jobValue = dtExperiences.Rows[i]["JobValue"].ToString();
                        object objWorkDate = dtExperiences.Rows[i]["WorkDate"];
                        DateTime workDate = DateTime.MinValue;

                        if (objWorkDate != DBNull.Value && objWorkDate is DateTime)
                        {
                            workDate = (DateTime)objWorkDate;
                        }
                        else
                        {
                            I18nDateTimeUtil.TryParseFromUI(I18nDateTimeUtil.FormatToDateStringForUI(objWorkDate), out workDate);
                        }

                        dr["clientName" + j] = dtExperiences.Rows[i]["ClientName"];
                        dr["jobValue" + j] = string.IsNullOrWhiteSpace(jobValue) || jobValue.Equals("0") ? string.Empty : I18nNumberUtil.FormatMoneyForUI(jobValue);
                        dr["workDate" + j] = workDate != DateTime.MinValue ? I18nDateTimeUtil.FormatToDateStringForUI(workDate) : string.Empty;
                        dr["description" + j] = dtExperiences.Rows[i]["Description"];
                    }
                }

                dt.Rows.Add(dr);
            }

            return dt;
        }

        /// <summary>
        /// Get certified experience record.
        /// </summary>
        /// <param name="licenseTemplate">The license template.</param>
        /// <param name="topNumber">top number</param>
        /// <returns>experience list</returns>
        public static DataTable GetExperiences(TemplateModel licenseTemplate, int topNumber = 0)
        {
            DataTable dataSource = CreateExperienceDataTable();
            DataTable dtResult = dataSource.Clone();

            List<object> experienceClient = GetValueFromTemplateTables(licenseTemplate, ACAConstant.NIGP_FIELD_CONTRACT_EXPERIENCE_CLIENT);
            List<object> experienceDesc = GetValueFromTemplateTables(licenseTemplate, ACAConstant.NIGP_FIELD_CONTRACT_EXPERIENCE_DESCRIPTION);
            List<object> experienceWorkDate = GetValueFromTemplateTables(licenseTemplate, ACAConstant.NIGP_FIELD_CONTRACT_EXPERIENCE_WORKDATE);
            List<object> experienceJobValue = GetValueFromTemplateTables(licenseTemplate, ACAConstant.NIGP_FIELD_CONTRACT_EXPERIENCE_JOBVALUE);

            if (experienceDesc != null && experienceDesc.Count > 0)
            {
                for (int i = 0; i < experienceDesc.Count; i++)
                {
                    if (experienceJobValue[i] == null && experienceDesc[i] == null && experienceWorkDate[i] == null)
                    {
                        continue;
                    }

                    // get and format the job value.
                    decimal jobValue = 0;
                    if (experienceJobValue[i] != null)
                    {
                        decimal.TryParse(experienceJobValue[i].ToString(), out jobValue);
                    }

                    // get and format the work date.
                    DateTime date = DateTime.MinValue;
                    if (experienceWorkDate[i] != null)
                    {
                        I18nDateTimeUtil.TryParseFromUI(I18nDateTimeUtil.FormatToDateStringForUI(experienceWorkDate[i]), out date);
                    }

                    DataRow dr = dataSource.NewRow();

                    dr["ClientName"] = experienceClient[i] == null ? string.Empty : experienceClient[i];
                    dr["JobValue"] = jobValue.ToString();
                    dr["Description"] = experienceDesc[i] == null ? string.Empty : experienceDesc[i];
                    dr["WorkDate"] = date;

                    dataSource.Rows.Add(dr);
                }
            }

            DataView dv = dataSource.DefaultView;
            dv.Sort = "JobValue Desc";
            dataSource = dv.ToTable();

            //get the topNumber records from the largest experiences
            if (topNumber > 0 && dataSource.Rows.Count > topNumber)
            {
                for (int i = 0; i < topNumber; i++)
                {
                    dtResult.ImportRow(dataSource.Rows[i]);
                }
            }
            else
            {
                dtResult = dataSource;
            }         

            return dtResult;
        }

        /// <summary>
        /// get user licenses by user sequence number.
        /// </summary>
        /// <param name="agencyCode">agency code</param>
        /// <param name="userSeqNumber">sequence number</param>
        /// <returns>license model list</returns>
        public static ContractorLicenseModel4WS[] GetContractorLicenseByUserSeqNumber(string agencyCode, string userSeqNumber)
        {
            if (AppSession.User.AllContractorLicenses == null)
            {
                AppSession.User.AllContractorLicenses = new Dictionary<string, ContractorLicenseModel4WS[]>();
            }

            if (AppSession.User.AllContractorLicenses.ContainsKey(agencyCode))
            {
                return AppSession.User.AllContractorLicenses[agencyCode];
            }

            ILicenseBLL licenseBll = ObjectFactory.GetObject<ILicenseBLL>();
            ContractorLicenseModel4WS[] contractorLicenses = licenseBll.GetContrLicListByUserSeqNBR(agencyCode, userSeqNumber);
            AppSession.User.AllContractorLicenses.Add(agencyCode, contractorLicenses);

            return contractorLicenses;
        }

        /// <summary>
        /// Fill the reference license template value to current template.
        /// </summary>
        /// <param name="agencyCode">agency code</param>
        /// <param name="templateType">template type</param>
        /// <param name="licenseSeqNum">license sequence number</param>
        /// <param name="initialAttributes">initial attributes</param>
        /// <returns>template models</returns>
        public static TemplateAttributeModel[] FillRefLicenseTemplate(string agencyCode, string templateType, string licenseSeqNum, TemplateAttributeModel[] initialAttributes)
        {
            if (string.IsNullOrEmpty(templateType) || string.IsNullOrEmpty(licenseSeqNum))
            {
                return initialAttributes;
            }

            ITemplateBll _templateBll = (ITemplateBll)ObjectFactory.GetObject(typeof(ITemplateBll));
            string agency = string.IsNullOrEmpty(agencyCode) ? ConfigManager.AgencyCode : agencyCode;
            TemplateAttributeModel[] attributes = _templateBll.GetRefPeopleTemplateAttributes(templateType, licenseSeqNum, agency, AppSession.User.PublicUserId);

            if (attributes != null && attributes.Length > 0)
            {
                initialAttributes = FillRefAttributeValues(initialAttributes, attributes);
            }

            return initialAttributes;
        }

        /// <summary>
        /// Determine to show Request a Trade License link
        /// </summary>
        /// <param name="agency">Agency Code</param>
        /// <param name="module">Module Name</param>
        /// <param name="group">App Status Group Code</param>
        /// <param name="status">App Status</param>
        /// <returns>true or false</returns>
        public static bool IsDisplayRequestTradeLicenseLink(string agency, string module, string group, string status)
        {
            bool isDispalyReqTradLicLink = false;

            ICacheManager cacheManager = ObjectFactory.GetObject<ICacheManager>();
            AppStatusGroupModel4WS appStatusGroupModel = cacheManager.GetAppStatusGroupModel(agency, module, group, status);

            if (appStatusGroupModel != null)
            {
                isDispalyReqTradLicLink = ValidationUtil.IsYes(appStatusGroupModel.displayRequestTradeLic);
            }

            return isDispalyReqTradLicLink;
        }

        /// <summary>
        /// Combine the source license list <see cref="sourceLicenseList"/> into the target license list <see cref="targetLicenseList"/> and return merged list.
        /// </summary>
        /// <param name="sourceLicenseList">Source licensed professional list.</param>
        /// <param name="targetLicenseList">Target licensed professional list.</param>
        /// <returns>The combined licensed professional array.</returns>
        public static LicenseProfessionalModel4WS[] AppendCapLPToGroup(IList<LicenseProfessionalModel4WS> sourceLicenseList, LicenseProfessionalModel4WS[] targetLicenseList)
        {
            if (sourceLicenseList == null || sourceLicenseList.Count == 0)
            {
                return targetLicenseList;
            }
             
            if (targetLicenseList == null || targetLicenseList.Length == 0)
            {
                return sourceLicenseList.ToArray();
            }

            string sourceCompentName = sourceLicenseList[0].componentName;

            List<LicenseProfessionalModel4WS> result = targetLicenseList.Where(f => !sourceCompentName.Equals(f.componentName)).ToList();
            result.AddRange(sourceLicenseList);

            return result.ToArray();
        }

        /// <summary>
        /// Remove the LP with component name from LP group.
        /// </summary>
        /// <param name="lpGroup">The LP group.</param>
        /// <param name="componentName">The component name.</param>
        /// <returns>The result that remove the LP with component name.</returns>
        public static LicenseProfessionalModel4WS[] RemoveLPWithComponentNameFromGroup(LicenseProfessionalModel4WS[] lpGroup, string componentName)
        {
            if (lpGroup == null)
            {
                return null;
            }

            if (string.IsNullOrEmpty(componentName))
            {
                return lpGroup;
            }

            List<LicenseProfessionalModel4WS> results = new List<LicenseProfessionalModel4WS>(lpGroup);
            results.RemoveAll(lp => lp.componentName == componentName);

            return results.ToArray();
        }

        /// <summary>
        /// Searches for an License Professional whose component name match the specified string. This method does not perform a case-sensitive search.
        /// </summary>
        /// <param name="capModel">The cap model.</param>
        /// <param name="componentName">The component name to be found.</param>
        /// <returns>The License Professional with the specified component name, if found; otherwise, null.</returns>
        public static LicenseProfessionalModel4WS FindLicenseProfessionalWithComponentName(CapModel4WS capModel, string componentName)
        {
            LicenseProfessionalModel4WS[] result = FindLicenseProfessionalsWithComponentName(capModel, componentName);

            return result != null ? result[0] : null;
        }

        /// <summary>
        /// Searches for a list of License Professionals whose component name match the specified string. This method does not perform a case-sensitive search.
        /// </summary>
        /// <param name="capModel">The cap model.</param>
        /// <param name="componentName">The component name to be found.</param>
        /// <returns>The list of License Professionals with the specified component name, if found; otherwise, null.</returns>
        public static LicenseProfessionalModel4WS[] FindLicenseProfessionalsWithComponentName(CapModel4WS capModel, string componentName)
        {
            if (capModel == null || capModel.licenseProfessionalList == null || capModel.licenseProfessionalList.Length == 0 || string.IsNullOrEmpty(componentName))
            {
                return null;
            }

            List<LicenseProfessionalModel4WS> lpBelongToLPList = new List<LicenseProfessionalModel4WS>();

            foreach (var lp in capModel.licenseProfessionalList)
            {
                if (componentName.Equals(lp.componentName, StringComparison.InvariantCultureIgnoreCase))
                {
                    lpBelongToLPList.Add(lp);
                }
            }

            if (lpBelongToLPList.Count > 0)
            {
                return lpBelongToLPList.ToArray();
            }

            if (capModel.licenseProfessionalList.Length == 1
                && string.IsNullOrEmpty(capModel.licenseProfessionalList[0].componentName))
            {
                capModel.licenseProfessionalModel = null;
                capModel.licenseProfessionalList[0].componentName = componentName;

                return capModel.licenseProfessionalList;
            }

            return null;
        }
        
        /// <summary>
        /// Prepares the licenses for copying record.
        /// </summary>
        /// <param name="capModel">The cap model to be checked.</param>
        /// <param name="pageFlowGroup">The page flow to be checked.</param>
        public static void PrepareLicensesForCopyingRecord(CapModel4WS capModel, PageFlowGroupModel pageFlowGroup)
        {
            if (capModel == null || capModel.licenseProfessionalList == null || capModel.IsLicensesChecked4Record)
            {
                capModel.IsLicensesChecked4Record = true;
                return;
            }

            if (pageFlowGroup == null || pageFlowGroup.stepList == null)
            {
                capModel.IsLicensesChecked4Record = true;
                return;
            }

            //The List of the name of components for LP/LP List Components in capModel.licenseProfessionalList.
            List<string> componentNames = (from lp in capModel.licenseProfessionalList
                                           where !string.IsNullOrEmpty(lp.componentName)
                                           select lp.componentName).Distinct().ToList();

            //The List of the name of components for LP/LP List Components are include in current page flow.
            List<string> existComponentNames = new List<string>();

            bool isTheSamePageFlow = PageFlowUtil.IsSamePageflow4License(pageFlowGroup, componentNames, ref existComponentNames);

            // remove the component name for the LP records beyond the existComponentNames from cap's license professional list.
            if (!isTheSamePageFlow)
            {
                foreach (LicenseProfessionalModel4WS lpModel in capModel.licenseProfessionalList)
                {
                    if (existComponentNames.Count >= 0 && !existComponentNames.Contains(lpModel.componentName))
                    {
                        lpModel.componentName = null;
                    }
                }
            }

            capModel.IsLicensesChecked4Record = true;
        }

        /// <summary>
        /// Convert license model to license professional model.
        /// </summary>
        /// <param name="licenseList">license model list</param>
        /// <param name="componentName">component name</param>
        /// <returns>license professional model list</returns>
        public static List<LicenseProfessionalModel> ConvertLicenseModel2LicenseProfessionalModel(IEnumerable<LicenseModel4WS> licenseList, string componentName)
        {
            List<LicenseProfessionalModel> lpList = new List<LicenseProfessionalModel>();

            foreach (var license in licenseList)
            {
                LicenseProfessionalModel licenseProModel = new LicenseProfessionalModel();
                licenseProModel.agencyCode = license.serviceProviderCode;
                licenseProModel.licenseType = license.licenseType;
                licenseProModel.resLicenseType = license.resLicenseType;
                licenseProModel.licenseNbr = license.stateLicense;
                licenseProModel.salutation = license.salutation;
                licenseProModel.contactFirstName = license.contactFirstName;
                licenseProModel.contactMiddleName = license.contactMiddleName;
                licenseProModel.contactLastName = license.contactLastName;
                licenseProModel.suffixName = license.suffixName;

                if (!string.IsNullOrEmpty(license.birthDate))
                {
                    licenseProModel.birthDate = I18nDateTimeUtil.ParseFromWebService(license.birthDate);
                }

                if (!string.IsNullOrEmpty(license.licenseIssueDate))
                {
                    licenseProModel.licesnseOrigIssueDate = I18nDateTimeUtil.ParseFromWebService(license.licenseIssueDate);
                }

                if (!string.IsNullOrEmpty(license.licenseExpirationDate))
                {
                    licenseProModel.licenseExpirDate = I18nDateTimeUtil.ParseFromWebService(license.licenseExpirationDate);
                }

                licenseProModel.gender = license.gender;
                licenseProModel.businessName = license.businessName;
                licenseProModel.busName2 = license.busName2;
                licenseProModel.businessLicense = license.businessLicense;
                licenseProModel.countryCode = license.countryCode;
                licenseProModel.address1 = license.address1;
                licenseProModel.address2 = license.address2;
                licenseProModel.address3 = license.address3;
                licenseProModel.city = license.city;
                licenseProModel.state = license.state;
                licenseProModel.resState = license.resState;
                licenseProModel.phone1 = license.phone1;
                licenseProModel.phone1CountryCode = license.phone1CountryCode;
                licenseProModel.phone2 = license.phone2;
                licenseProModel.phone2CountryCode = license.phone2CountryCode;
                licenseProModel.fax = license.fax;
                licenseProModel.faxCountryCode = license.faxCountryCode;
                licenseProModel.contrLicNo = license.contrLicNo;
                licenseProModel.contLicBusName = license.contLicBusName;
                licenseProModel.email = license.emailAddress;

                //The serDes field is a required field. any string can be assigned to this field here.
                licenseProModel.serDes = "Description";
                licenseProModel.auditID = license.auditID;
                licenseProModel.auditStatus = license.auditStatus;
                licenseProModel.zip = license.zip;
                licenseProModel.postOfficeBox = license.postOfficeBox;
                licenseProModel.licSeqNbr = license.licSeqNbr;
                licenseProModel.typeFlag = license.typeFlag;
                licenseProModel.socialSecurityNumber = license.socialSecurityNumber;
                licenseProModel.maskedSsn = license.maskedSsn;
                licenseProModel.fein = license.fein;
                licenseProModel.templateAttributes = license.templateAttributes;
                licenseProModel.componentName = componentName;
                licenseProModel.TemporaryID = CommonUtil.GetRandomUniqueID();
                lpList.Add(licenseProModel);
            }

            return lpList;
        }

        /// <summary>
        /// Convert license model to license professional model.
        /// </summary>
        /// <param name="licenseList">license model list</param>
        /// <param name="componentName">component name</param>
        /// <returns>license professional model list</returns>
        public static List<LicenseProfessionalModel4WS> ConvertLicenseModel2LicenseProfessionalModel4WS(IEnumerable<LicenseModel4WS> licenseList, string componentName)
        {
            List<LicenseProfessionalModel4WS> lpList = new List<LicenseProfessionalModel4WS>();

            foreach (var license in licenseList)
            {
                LicenseProfessionalModel4WS licenseProModel = new LicenseProfessionalModel4WS();
                licenseProModel.agencyCode = license.serviceProviderCode;
                licenseProModel.licenseType = license.licenseType;
                licenseProModel.resLicenseType = license.resLicenseType;
                licenseProModel.licenseNbr = license.stateLicense;
                licenseProModel.salutation = license.salutation;
                licenseProModel.contactFirstName = license.contactFirstName;
                licenseProModel.contactMiddleName = license.contactMiddleName;
                licenseProModel.contactLastName = license.contactLastName;
                licenseProModel.suffixName = license.suffixName;
                licenseProModel.birthDate = license.birthDate;
                licenseProModel.licesnseOrigIssueDate = license.licenseIssueDate;
                licenseProModel.licenseExpirDate = license.licenseExpirationDate;
                licenseProModel.gender = license.gender;
                licenseProModel.businessName = license.businessName;
                licenseProModel.busName2 = license.busName2;
                licenseProModel.businessLicense = license.businessLicense;
                licenseProModel.countryCode = license.countryCode;
                licenseProModel.address1 = license.address1;
                licenseProModel.address2 = license.address2;
                licenseProModel.address3 = license.address3;
                licenseProModel.city = license.city;
                licenseProModel.state = license.state;
                licenseProModel.resState = license.resState;
                licenseProModel.phone1 = license.phone1;
                licenseProModel.phone1CountryCode = license.phone1CountryCode;
                licenseProModel.phone2 = license.phone2;
                licenseProModel.phone2CountryCode = license.phone2CountryCode;
                licenseProModel.fax = license.fax;
                licenseProModel.faxCountryCode = license.faxCountryCode;
                licenseProModel.contrLicNo = license.contrLicNo;
                licenseProModel.contLicBusName = license.contLicBusName;
                licenseProModel.email = license.emailAddress;

                //The serDes field is a required field. any string can be assigned to this field here.
                licenseProModel.serDes = "Description";
                licenseProModel.auditID = license.auditID;
                licenseProModel.auditStatus = license.auditStatus;
                licenseProModel.zip = license.zip;
                licenseProModel.postOfficeBox = license.postOfficeBox;
                licenseProModel.licSeqNbr = license.licSeqNbr;
                licenseProModel.typeFlag = license.typeFlag;
                licenseProModel.socialSecurityNumber = license.socialSecurityNumber;
                licenseProModel.maskedSsn = license.maskedSsn;
                licenseProModel.fein = license.fein;
                licenseProModel.componentName = componentName;
                licenseProModel.TemporaryID = CommonUtil.GetRandomUniqueID();
                licenseProModel.attributes = license.templateAttributes;
                lpList.Add(licenseProModel);
            }

            return lpList;
        }

        /// <summary>
        /// Removes the redundant LPs whose component name match the specified string from the instance of CapModel4WS.
        /// </summary>
        /// <param name="capModel">The instance of CapModel4WS whose contact data will be removed.</param>
        /// <param name="componentName">The component name to be checked.</param>
        public static void RemoveRedundantLPsWithComponentName(CapModel4WS capModel, string componentName)
        {
            if (capModel != null && capModel.licenseProfessionalList != null && capModel.licenseProfessionalList.Length > 0)
            {
                List<LicenseProfessionalModel4WS> tmpList = new List<LicenseProfessionalModel4WS>(capModel.licenseProfessionalList);
                tmpList.RemoveAll(lp => componentName.Equals(lp.componentName, StringComparison.InvariantCultureIgnoreCase));
                capModel.licenseProfessionalList = tmpList.ToArray();
            }
        }

        /// <summary>
        /// Determine current license is exist in License list or not.
        /// </summary>
        /// <param name="moduleName">module name</param>
        /// <param name="licenseList">License Professional Model</param>
        /// <param name="lpList">The license Professional list.</param>
        /// <param name="errorMsg">error message</param>
        /// <returns>true or false</returns>
        public static bool IsDuplicateLP(string moduleName, IEnumerable<LicenseModel4WS> licenseList, IEnumerable<LicenseProfessionalModel4WS> lpList, ref string errorMsg)
        {
            if (licenseList == null || !licenseList.Any() || lpList == null || !lpList.Any())
            {
                return false;
            }

            List<string> duplicateLicenseNbr = new List<string>();
            List<LicenseModel4WS> licenses4Compare = licenseList.ToList();

            /*
             * In multiple agency, it validates duplicate before add new license which the license from super agency.
             * It uses the new license (Super Agency) to validate duplicate, exists follow case:
             * 1. The CapModel4WS.licenseProfessionalList exists super agency, it validate OK.
             * 2. The CapModel4WS.licenseProfessionalList not exists super agency, 
             *    it need first to copy the license from the new license to the sub agency license, then validate.
             */
            if (StandardChoiceUtil.IsSuperAgencyIgnoreSubAgency())
            {
                List<string> agencyCodes = new List<string>();

                foreach (LicenseProfessionalModel4WS lp in lpList)
                {
                    if (!agencyCodes.Contains(lp.agencyCode))
                    {
                        agencyCodes.Add(lp.agencyCode);
                    }
                }

                foreach (LicenseModel4WS licenseModel in licenseList)
                {
                    if (agencyCodes.Contains(licenseModel.serviceProviderCode))
                    {
                        continue;
                    }

                    foreach (string agencyCode in agencyCodes)
                    {
                        LicenseModel4WS newLicense = new LicenseModel4WS
                                                         {
                                                             serviceProviderCode = agencyCode,
                                                             licenseType = licenseModel.licenseType,
                                                             stateLicense = licenseModel.stateLicense,
                                                             TemporaryID = licenseModel.TemporaryID
                                                         };

                        licenses4Compare.Add(newLicense);
                    }
                }
            }

            foreach (LicenseModel4WS license in licenses4Compare)
            {
                foreach (LicenseProfessionalModel4WS lp in lpList)
                {
                    if (lp.licenseType.Equals(license.licenseType, StringComparison.InvariantCultureIgnoreCase)
                        && string.Equals(lp.licenseNbr, license.stateLicense, StringComparison.InvariantCultureIgnoreCase)
                        && string.Equals(lp.agencyCode, license.serviceProviderCode, StringComparison.InvariantCultureIgnoreCase)
                        && !string.Equals(lp.TemporaryID, license.TemporaryID, StringComparison.InvariantCultureIgnoreCase))
                    {
                        if (!duplicateLicenseNbr.Contains(license.stateLicense))
                        {
                            duplicateLicenseNbr.Add(license.stateLicense);
                        }
                    }
                }
            }

            if (duplicateLicenseNbr.Count > 0)
            {
                string errorValue = DataUtil.ConcatStringWithSplitChar(duplicateLicenseNbr, ACAConstant.COMMA_BLANK);
                string errorPara = LabelUtil.GetTextByKey("per_multilicense_error_duplicatelicense", moduleName);
                errorMsg = DataUtil.StringFormat(errorPara, errorValue);
                return true;
            }

            return false;
        }

        /// <summary>
        /// Removes all redundant LPs from the instance of CapModel4WS.
        /// </summary>
        /// <param name="capModel">The instance of CapModel4WS whose contact data will be removed.</param>
        public static void RemoveRedundantLPs(CapModel4WS capModel)
        {
            if (capModel != null && capModel.licenseProfessionalList != null && capModel.licenseProfessionalList.Length > 0)
            {
                string pattern = @"\w+_\d+";
                Regex rgx = new Regex(pattern);

                List<LicenseProfessionalModel4WS> tmpList = new List<LicenseProfessionalModel4WS>(capModel.licenseProfessionalList);
                tmpList.RemoveAll(lp => string.IsNullOrEmpty(lp.componentName) || !rgx.Match(lp.componentName).Success);

                capModel.licenseProfessionalList = tmpList.ToArray();
            }
        }

        /// <summary>
        /// Validate single license section required and format.
        /// </summary>
        /// <param name="license">LicenseProfessionalModel model</param>
        /// <param name="moduleName">module name</param>
        /// <param name="isEditable">LicenseEdit isEditable</param>
        /// <param name="isValidate">LicenseEdit isValidate</param>
        /// <returns>true or false</returns>
        public static bool ValidateRequiredField4SingleLicense(LicenseProfessionalModel license, string moduleName, bool isEditable, bool isValidate)
        {
            if (license == null)
            {
                return true;
            }

            bool isSucceeded = true;

            if (!isEditable || isValidate)
            {
                List<TemplateAttributeModel> fields = new List<TemplateAttributeModel>();
                fields.AddRange(TemplateUtil.GetAlwaysEditableRequiredTemplateFields(license.templateAttributes));

                if (fields.Any())
                {
                    isSucceeded = RequiredValidationUtil.ValidateFields4Template(fields.ToArray());
                }
            }
            else
            {
                LicenseProfessionalModel[] licenses = new[] { license };

                if (!RequiredValidationUtil.ValidateFields4LPList(moduleName, GviewID.LicenseEdit, licenses)
                    || !FormatValidationUtil.ValidateFormat4LPList(moduleName, GviewID.LicenseEdit, licenses))
                {
                    isSucceeded = false;
                }
            }

            return isSucceeded;
        }

        /// <summary>
        /// Get public user associated licenses.
        /// </summary>
        /// <param name="moduleName">module name</param>
        /// <returns>license model list</returns>
        public static LicenseModel4WS[] GetPublicUserLicenses(string moduleName)
        {
            IProxyUserRoleBll proxyUserRoleBll = ObjectFactory.GetObject<IProxyUserRoleBll>();
            PublicUserModel4WS user = proxyUserRoleBll.GetCurrentUser(AppSession.GetCapModelFromSession(moduleName));

            if (user != null)
            {
                return user.licenseModel;
            }

            return null;
        }

        /// <summary>
        /// Indicate license has condition or not.
        /// </summary>
        /// <param name="moduleName">module name</param>
        /// <param name="licenseSeqNbr">license sequence number</param>
        /// <returns>boolean value</returns>
        public static bool HasLicenseCondition(string moduleName, long licenseSeqNbr)
        {
            bool hasCondition = true;
            LicenseModel4WS licenseModel = GetLicenseCondition(moduleName, licenseSeqNbr);

            if (licenseModel == null 
                || licenseModel.noticeConditions == null 
                || licenseModel.noticeConditions.Length == 0
                || licenseModel.hightestCondition == null 
                || string.IsNullOrEmpty(licenseModel.hightestCondition.impactCode))
            {
                hasCondition = false;
            }

            return hasCondition;
        }

        /// <summary>
        /// get license condition 
        /// </summary>
        /// <param name="moduleName">module name</param>
        /// <param name="licenseSeqNumber">license Sequence Number</param>
        /// <returns>license pro model.</returns>
        public static LicenseModel4WS GetLicenseCondition(string moduleName, long licenseSeqNumber)
        {
            ILicenseBLL licenseBll = (ILicenseBLL)ObjectFactory.GetObject(typeof(ILicenseBLL));
            LicenseModel4WS licenseModel;

            if (!CapUtil.IsSuperCAP(moduleName))
            {
                licenseModel = licenseBll.GetLicenseCondition(ConfigManager.AgencyCode, licenseSeqNumber, AppSession.User.PublicUserId);
                return licenseModel;
            }

            ServiceModel[] services = AppSession.GetSelectedServicesFromSession();
            IList<string> agencies = new List<string>();

            if (services != null)
            {
                foreach (ServiceModel service in services)
                {
                    if (!agencies.Contains(service.servPorvCode))
                    {
                        agencies.Add(service.servPorvCode);
                    }
                }
            }
            else
            {
                // from resume,get agencies from child caps.
                CapModel4WS parentCap = AppSession.GetCapModelFromSession(moduleName);

                ICapBll capBll = (ICapBll)ObjectFactory.GetObject(typeof(ICapBll));
                CapIDModel4WS[] childCapIDs = capBll.GetChildCaps(parentCap.capID);

                if (childCapIDs != null)
                {
                    foreach (CapIDModel4WS childCap in childCapIDs)
                    {
                        if (!agencies.Contains(childCap.serviceProviderCode))
                        {
                            agencies.Add(childCap.serviceProviderCode);
                        }
                    }
                }
            }

            string[] agencyList = new string[agencies.Count];
            agencies.CopyTo(agencyList, 0);

            licenseModel = licenseBll.GetLicenseCondition(agencyList, licenseSeqNumber);
            return licenseModel;
        }

        /// <summary>
        /// Get LicenseType is exist in standard choice
        /// </summary>
        /// <param name="licenseType">License Type</param>
        /// <returns>true:exist; false: no exist </returns>
        public static bool IsValidLicenseType(string licenseType)
        {
            bool isExist = false;
            IBizDomainBll standChoiceBll = ObjectFactory.GetObject<IBizDomainBll>();
            IList<ItemValue> stdItems = standChoiceBll.GetBizDomainList(ConfigManager.AgencyCode, BizDomainConstant.STD_CAT_LICENSE_TYPE, false);
            
            if (stdItems != null)
            {
                isExist = stdItems.Any(itemValue => licenseType == itemValue.Key);
            }

            return isExist;
        }

        /// <summary>
        /// if conditionModel is not null, show locked, hold or notice message.
        /// </summary>
        /// <param name="moduleName">module name </param>
        /// <param name="licenseSeqNumber">the license sequence number</param>
        /// <param name="licenseCondition">the license condition.</param>
        /// <returns>true or false.</returns>
        public static bool IsLockedCondition(string moduleName, long licenseSeqNumber, Conditions licenseCondition)
        {
            bool isLocked = false;
            LicenseModel4WS licenseModel = GetLicenseCondition(moduleName, licenseSeqNumber);
            bool hasCondition = HasLicenseCondition(moduleName, licenseSeqNumber);

            if (!hasCondition)
            {
                return false;
            }

            isLocked = licenseCondition.IsShowCondition(licenseModel.noticeConditions, licenseModel.hightestCondition, ConditionType.License);

            return isLocked;
        }

        /// <summary>
        /// combine license professional to LP list.
        /// </summary>
        /// <param name="capModel">The cap model.</param>
        public static void InitializeLicenseProfessional4CapModel(CapModel4WS capModel)
        {
            /*
             * There is a field in the CapModel4WS class called licenseProfessionalModel which representing a LP data.
             * Besides is, there is another field called licenseProfessionalList which representing all the LPs associated with the specific record.
             * the JAVA code/EMSE will put one license Professional which from license professional section into this field.  
             * now we stored all license professional in the license professional list, and the licenseProfessionalModel will become obsolete.
             */
            if (capModel == null || capModel.licenseProfessionalModel == null)
            {
                return;
            }

            if (capModel.licenseProfessionalList != null)
            {
                if (!IsDuplicateLP(capModel.licenseProfessionalList, capModel.licenseProfessionalModel))
                {
                    List<LicenseProfessionalModel4WS> originalLpList = new List<LicenseProfessionalModel4WS>(capModel.licenseProfessionalList);
                    originalLpList.Add(capModel.licenseProfessionalModel);
                    capModel.licenseProfessionalList = originalLpList.ToArray();
                }
            }
            else
            {
                capModel.licenseProfessionalList = new[] { capModel.licenseProfessionalModel };
            }

            capModel.licenseProfessionalModel = null;
        }

        /// <summary>
        /// Checks the license data.
        /// </summary>
        /// <param name="htUserControls">The user controls.</param>
        /// <param name="moduleName">Name of the module.</param>
        /// <param name="dictKeysAndNamesForMultipleLP">The dictionary keys and names for multiple LP.</param>
        /// <param name="dictKeysAndNamesForLPList">The dictionary keys and names for LP list.</param>
        /// <returns>True is validate else is not validate</returns>
        public static bool IsReferenceDataForLicense(Hashtable htUserControls, string moduleName, Dictionary<string, string> dictKeysAndNamesForMultipleLP, Dictionary<string, string> dictKeysAndNamesForLPList)
        {
            bool isReferenceData = true;

            if (dictKeysAndNamesForMultipleLP.Count > 0)
            {
                foreach (var keyNamePair in dictKeysAndNamesForMultipleLP)
                {
                    string key = keyNamePair.Key;
                    LicenseEdit licenseEdit = (LicenseEdit)htUserControls[key];

                    if (!licenseEdit.IsDataValid())
                    {
                        string errorMsg = LabelUtil.GetTextByKey("per_license_error_searchClickedRequired", moduleName);
                        licenseEdit.ShowValidateErrorMessage(errorMsg);
                        isReferenceData = false;
                    }
                }
            }

            if (dictKeysAndNamesForLPList.Count > 0)
            {
                string key = dictKeysAndNamesForLPList.ElementAt(0).Key;
                MultiLicensesEdit multiLicensesEdit = (MultiLicensesEdit)htUserControls[key];
                string errorMsg = multiLicensesEdit.CheckLicenses();

                if (!string.IsNullOrEmpty(errorMsg))
                {
                    multiLicensesEdit.ShowValidateErrorMessage(errorMsg);
                    isReferenceData = false;
                }
            }

            return isReferenceData;
        }

        /// <summary>
        /// Merge new licenses with existing licenses, filter the duplicate license.
        /// </summary>
        /// <param name="model">public user model.</param>
        /// <param name="newLicenseList">new license list.</param>
        public static void MergeLicense(PublicUserModel4WS model, LicenseModel4WS[] newLicenseList)
        {
            if (newLicenseList == null)
            {
                return;
            }

            IList<LicenseModel4WS> licenseModelList = new List<LicenseModel4WS>();

            if (model.licenseModel == null)
            {
                licenseModelList = newLicenseList;
            }
            else
            {
                foreach (var item in model.licenseModel)
                {
                    licenseModelList.Add(item);
                }

                foreach (var item in newLicenseList)
                {
                    if (!licenseModelList.Any(f => f.licSeqNbr == item.licSeqNbr))
                    {
                        licenseModelList.Add(item);
                    }
                }
            }

            if (licenseModelList.Any())
            {
                model.licenseModel = licenseModelList.ToArray();
            }
        }

        /// <summary>
        /// Create DataTable for certified experience.
        /// </summary>
        /// <returns>a DataTable including certified experience information.</returns>
        private static DataTable CreateExperienceDataTable()
        {
            DataTable dt = new DataTable();
            
            dt.Columns.Add("ClientName", typeof(string));
            dt.Columns.Add("JobValue", typeof(decimal));
            dt.Columns.Add("WorkDate", typeof(DateTime));
            dt.Columns.Add("Description", typeof(string));

            return dt;
        }

        /// <summary>
        /// Check if the value description of standard choice which belongs to ENABLE_EXPIRED_LICENSE is "Yes"/"Y" 
        /// </summary>
        /// <param name="bizDomainValue">biz domain value</param>
        /// <returns>true if the biz domain value description is "Yes"/"Y"</returns>
        private static bool IsYes4EnableExpiredLicByValue(string bizDomainValue)
        {
            IBizDomainBll bizBll = (IBizDomainBll)ObjectFactory.GetObject(typeof(IBizDomainBll));

            string enabled = bizBll.GetValueForStandardChoice(
                ConfigManager.AgencyCode,
                BizDomainConstant.STD_CAT_ENABLE_EXPIRED_LICENSE,
                bizDomainValue);

            // if the value is 'Yes' or 'Y', return true.
            return ValidationUtil.IsYes(enabled);
        }

        /// <summary>
        /// get original license template
        /// </summary>
        /// <param name="agencyCode">agency code</param>
        /// <param name="licenseType">license type</param>
        /// <param name="licenseNum">license number</param>
        /// <returns>template model</returns>
        private static TemplateAttributeModel[] GetOriginalLicenseTemplate(string agencyCode, string licenseType, string licenseNum)
        {
            TemplateAttributeModel[] attributes;
            ITemplateBll _templateBll = (ITemplateBll)ObjectFactory.GetObject(typeof(ITemplateBll));
            
            if (string.IsNullOrEmpty(licenseNum))
            {
                attributes = _templateBll.GetPeopleTemplateAttributes(licenseType, ConfigManager.AgencyCode, AppSession.User.PublicUserId);
            }
            else
            {
                string servProvCode = string.IsNullOrEmpty(agencyCode) ? ConfigManager.AgencyCode : agencyCode;
                attributes = _templateBll.GetLPAttributes4SupportEMSE(servProvCode, licenseType, string.Empty, licenseNum, AppSession.User.PublicUserId);
            }
            
            return attributes;
        }

        /// <summary>
        /// fill reference attribute values
        /// </summary>
        /// <param name="initialAttributes">initial attributes</param>
        /// <param name="refAttributes">reference attributes</param>
        /// <returns>template models</returns>
        private static TemplateAttributeModel[] FillRefAttributeValues(TemplateAttributeModel[] initialAttributes, TemplateAttributeModel[] refAttributes)
        {
            //1. validation
            if (initialAttributes == null || initialAttributes.Length <= 0 || refAttributes == null || refAttributes.Length <= 0)
            {
                return initialAttributes;
            }

            //2. Fills reference values to current existed field.
            foreach (TemplateAttributeModel initField in initialAttributes)
            {
                if (initField == null || string.IsNullOrEmpty(initField.attributeName) || string.IsNullOrEmpty(initField.templateType))
                {
                    continue;
                }

                foreach (TemplateAttributeModel refField in refAttributes)
                {
                    if (refField == null || string.IsNullOrEmpty(refField.attributeName) || string.IsNullOrEmpty(refField.templateType))
                    {
                        continue;
                    }

                    if (refField.attributeName.Equals(initField.attributeName, StringComparison.InvariantCulture)
                        && refField.templateType.Equals(initField.templateType, StringComparison.InvariantCulture))
                    {
                        initField.attributeValue = refField.attributeValue;
                        break;
                    }
                }
            }

            return initialAttributes;
        }

        /// <summary>
        /// to check license professional whether it exists in LP list. 
        /// </summary>
        /// <param name="licenseProfessionals">The license professional list.</param>
        /// <param name="licenseProfessional">The license professional that to check.</param>
        /// <returns>Return true if exists duplicate LP.</returns>
        private static bool IsDuplicateLP(LicenseProfessionalModel4WS[] licenseProfessionals, LicenseProfessionalModel4WS licenseProfessional)
        {
            if (licenseProfessionals == null || licenseProfessionals.Length == 0 || licenseProfessional == null)
            {
                return false;
            }

            foreach (LicenseProfessionalModel4WS lpModel in licenseProfessionals)
            {
                if (lpModel.agencyCode == licenseProfessional.agencyCode
                    && lpModel.licenseType == licenseProfessional.licenseType
                    && lpModel.licenseNbr == licenseProfessional.licenseNbr)
                {
                    return true;
                }
            }

            return false;
        }
    }
}