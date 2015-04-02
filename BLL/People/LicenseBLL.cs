#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: LicenseBLL.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2007-2014
 *
 *  Description:
 *
 *  Notes:
 * $Id: LicenseBLL.cs 278415 2014-09-03 08:45:04Z ACHIEVO\james.shi $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;

using Accela.ACA.BLL.Common;
using Accela.ACA.Common;
using Accela.ACA.Common.Log;
using Accela.ACA.Common.Util;
using Accela.ACA.WSProxy;

using log4net;

namespace Accela.ACA.BLL.People
{
    /// <summary>
    /// This class provide the ability to operation license.
    /// </summary>
    public class LicenseBLL : BaseBll, ILicenseBLL
    {
        #region Fields

        /// <summary>
        /// logger object.
        /// </summary>
        private static readonly ILog Logger = LogFactory.Instance.GetLogger(typeof(LicenseBLL));

        #endregion Fields

        #region Properties

        /// <summary>
        /// Gets an instance of ContractorLicenseService.
        /// </summary>
        private ContractorLicenseWebServiceService ContractorLicenseService
        {
            get
            {
                return WSFactory.Instance.GetWebService<ContractorLicenseWebServiceService>();
            }
        }

        /// <summary>
        /// Gets an instance of LicenseService.
        /// </summary>
        private LicenseWebServiceService LicenseService
        {
            get
            {
                return WSFactory.Instance.GetWebService<LicenseWebServiceService>();
            }
        }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Construct a new DataTable for License.
        /// </summary>
        /// <returns>a data table contains 9 columns below:
        /// LicenseNumber
        /// LicenseType
        /// ContractName
        /// LicenseIssueDate
        /// LicenseExpirationDate
        /// Phone
        /// Fax
        /// Address1
        /// Address2</returns>
        public DataTable ConstructLicenseDataTable()
        {
            DataTable licenseTable = new DataTable();

            licenseTable.Columns.Add("RowIndex");
            licenseTable.Columns.Add("LicenseNumber");
            licenseTable.Columns.Add("LicenseType");
            licenseTable.Columns.Add("LicenseTypeText");
            licenseTable.Columns.Add("ContractName");
            licenseTable.Columns.Add("LicenseIssueDate", typeof(DateTime));
            licenseTable.Columns.Add("LicenseExpirationDate", typeof(DateTime));
            licenseTable.Columns.Add("PhoneIDD");
            licenseTable.Columns.Add("Phone");
            licenseTable.Columns.Add("FaxIDD");
            licenseTable.Columns.Add("Fax");
            licenseTable.Columns.Add("ContractorLicNO");
            licenseTable.Columns.Add("ContractorBusiName");
            licenseTable.Columns.Add("Address1");
            licenseTable.Columns.Add("Address2");
            licenseTable.Columns.Add("Address3");

            //Added for license spear form
            licenseTable.Columns.Add("LicenseSeqNumber");
            licenseTable.Columns.Add("Salutation");
            licenseTable.Columns.Add("FirstName");
            licenseTable.Columns.Add("MiddleName");
            licenseTable.Columns.Add("LastName");
            licenseTable.Columns.Add("BirthDate", typeof(DateTime));
            licenseTable.Columns.Add("Gender");
            licenseTable.Columns.Add("BusinessName");
            licenseTable.Columns.Add("BusinessName2");
            licenseTable.Columns.Add("BusinessLicense");
            licenseTable.Columns.Add("Email");
            licenseTable.Columns.Add("Country");
            licenseTable.Columns.Add("CountryCode");
            licenseTable.Columns.Add("City");
            licenseTable.Columns.Add("State");
            licenseTable.Columns.Add("Zip");
            licenseTable.Columns.Add("PostOfficeBox");
            licenseTable.Columns.Add("MobilePhoneIDD");
            licenseTable.Columns.Add("MobilePhone");
            licenseTable.Columns.Add("AgencyCode");
            licenseTable.Columns.Add("Suffix");

            // Fields for expired license/insurance/business license
            licenseTable.Columns.Add("IsLicExpired", typeof(bool));
            licenseTable.Columns.Add("IsInsExpired", typeof(bool));
            licenseTable.Columns.Add("IsBusLicExpired", typeof(bool));
            licenseTable.Columns.Add("LicTypePermission", typeof(RecordTypeLicTypePermissionModel4WS));

            licenseTable.Columns.Add("ContactType");
            licenseTable.Columns.Add("MaskedSSN");
            licenseTable.Columns.Add("SSN");
            licenseTable.Columns.Add("FEIN");
            licenseTable.Columns.Add("MaskedFEIN");

            licenseTable.Columns.Add(ColumnConstant.TEAMPLATE_ATTRIBUTE, typeof(TemplateAttributeModel[]));

            return licenseTable;
        }

        /// <summary>
        /// Construct Multiple License DataTable format.
        /// </summary>
        /// <returns>data table</returns>
        public DataTable ConstructMultipleLicenseDataTable()
        {
            //reuse ConstructLicenseDataTable function.
            DataTable dtLicense = ConstructLicenseDataTable();
            dtLicense.Columns.Add("LicenseProfessionalModel", typeof(LicenseProfessionalModel));

            return dtLicense;
        }

        /// <summary>
        /// Method to delete a contractor license mapping for a user.
        /// </summary>
        /// <param name="agencyCode">agency code</param>
        /// <param name="licenseModel">license model</param>
        /// <param name="callID">caller id number.</param>
        /// <param name="userSeq">user sequence number</param>
        /// <exception cref="ACAException">Exception from web service.</exception>
        public void DeleteContracotrLicensePK(string agencyCode, LicenseModel4WS licenseModel, string callID, string userSeq)
        {
            try
            {
                DeleteContracotrLicensePK inputParam = new DeleteContracotrLicensePK();

                inputParam.licenseSeqNBR = licenseModel.licSeqNbr;
                inputParam.licenseType = licenseModel.licenseType;
                inputParam.servProvCode = licenseModel.serviceProviderCode;
                inputParam.userSeqNBR = userSeq;

                DeleteContracotrLicensePK[] pks = new DeleteContracotrLicensePK[1];
                pks[0] = inputParam;

                ContractorLicenseService.deleteContractorLicense(agencyCode, pks);
            }
            catch (Exception e)
            {
                throw new ACAException(e);
            }
        }

        /// <summary>
        /// Method to get contractor license list by user.
        /// </summary>
        /// <param name="agencyCode">agency code</param>
        /// <param name="userSeqNbr">user sequence number</param>
        /// <returns>Array of ContractorLicenseModel4WS</returns>
        /// <exception cref="ACAException">Exception from web service.</exception>
        public ContractorLicenseModel4WS[] GetContrLicListByUserSeqNBR(string agencyCode, string userSeqNbr)
        {
            try
            {
                return ContractorLicenseService.getContrLicListByUserSeqNBR(userSeqNbr, agencyCode);
            }
            catch (Exception e)
            {
                throw new ACAException(e);
            }
        }

        /// <summary>
        /// Gets licenses of a contractor according to if license of contractor is valid.
        /// </summary>
        /// <param name="servProvCode">the Service Provider Code</param>
        /// <param name="userSeqNBR">the user sequence number</param>
        /// <param name="isValid">If true, get only valid licenses. else, get all licenses including invalid license.</param>
        /// <returns>LicenseModel array</returns>
        public LicenseModel4WS[] GetContractorLicenseValidList(string servProvCode, string userSeqNBR, bool isValid)
        {
            if (string.IsNullOrEmpty(servProvCode) || string.IsNullOrEmpty(userSeqNBR))
            {
                throw new DataValidateException(new string[] { "servProvCode", "userSeqNBR" });
            }

            try
            {
                return ContractorLicenseService.getContractorLicenseValidList(servProvCode, userSeqNBR, isValid);
            }
            catch (Exception e)
            {
                throw new ACAException(e);
            }
        }

        /// <summary>
        /// get license by license sequence number
        /// </summary>
        /// <param name="licSeqNbr">license sequence number</param>
        /// <param name="userSeqNum">the user sequence number</param>
        /// <param name="servProvCode">the Service Provider Code</param>
        /// <returns>License Model</returns>
        public LicenseModel4WS GetLicenseByLicSeqNbr(string licSeqNbr, string userSeqNum, string servProvCode)
        {
            ContractorLicenseModel4WS[] lincenseArray = GetContrLicListByUserSeqNBR(servProvCode, userSeqNum);
            LicenseModel4WS licenseModel = null;

            if (lincenseArray != null && lincenseArray.Length > 0)
            {
                foreach (ContractorLicenseModel4WS conLicModel in lincenseArray)
                {
                    if ((conLicModel != null) && (conLicModel.license != null) && (licSeqNbr == conLicModel.license.licSeqNbr))
                    {
                        licenseModel = conLicModel.license;
                        break;
                    }
                }
            }

            return licenseModel;
        }

        /// <summary>
        /// Gets license by state license number.
        /// If license type does not match the result license type, return null.
        /// </summary>
        /// <param name="licenseModel4WS">containing ServiceProviderCode and StateLicense</param>
        /// <returns>LicenseModel4WS object.</returns>
        /// <exception cref="DataValidateException">{ <c>licenseModel4WS, licenseModel4WS.ServiceProviderCode, licenseModel4WS.ServiceProviderCode, licenseModel4WS.licenseType</c> } is null</exception>
        /// <exception cref="ACAException">Exception from web service.</exception>
        public LicenseModel4WS GetLicenseByStateLicNbr(LicenseModel4WS licenseModel4WS)
        {
            if (licenseModel4WS == null || string.IsNullOrEmpty(licenseModel4WS.serviceProviderCode) || string.IsNullOrEmpty(licenseModel4WS.stateLicense) || string.IsNullOrEmpty(licenseModel4WS.licenseType))
            {
                throw new DataValidateException(new string[] { "licenseModel4WS", "licenseModel4WS.ServiceProviderCode", "licenseModel4WS.ServiceProviderCode", "licenseModel4WS.licenseType" });
            }

            try
            {
                return LicenseService.getLicenseByStateLicNbr(licenseModel4WS);
            }
            catch (Exception e)
            {
                throw new ACAException(e);
            }
        }

        /// <summary>
        /// Gets highest priority condition and notice conditions related with license.
        /// </summary>
        /// <remarks>
        /// 1. LicenseWebService. getLicenseCondition to return.
        /// </remarks>
        /// <param name="agencyCode">agency Code</param>
        /// <param name="licenseSeqNbr">license sequence number</param>
        /// <param name="callerID">public user ID</param>
        /// <returns>ParcelCondition Model</returns>
        public LicenseModel4WS GetLicenseCondition(string agencyCode, long licenseSeqNbr, string callerID)
        {
            if (string.IsNullOrEmpty(agencyCode) || string.IsNullOrEmpty(callerID))
            {
                throw new DataValidateException(new string[] { "agencyCode", "callerID" });
            }

            try
            {
                return LicenseService.getLicenseCondition(new string[] { agencyCode }, licenseSeqNbr, callerID);
            }
            catch (Exception e)
            {
                throw new ACAException(e);
            }
        }

        /// <summary>
        /// Gets highest priority condition and notice conditions related with license.
        /// </summary>
        /// <param name="agencyCodes">all selected services' agency codes</param>
        /// <param name="licenseSeqNo">license Sequence value.</param>
        /// <returns>LicenseModel4WS object.</returns>
        /// <exception cref="DataValidateException">{ <c>agencyCodes, callerID</c> } is null</exception>
        /// <exception cref="ACAException">Exception from web service.</exception>
        /// <remarks>1. LicenseWebService. getLicenseCondition to return.</remarks>
        public LicenseModel4WS GetLicenseCondition(string[] agencyCodes, long licenseSeqNo)
        {
            string callerID = User.PublicUserId;

            if (agencyCodes == null || agencyCodes.Length == 0 || string.IsNullOrEmpty(callerID))
            {
                throw new DataValidateException(new string[] { "agencyCodes", "callerID" });
            }

            try
            {
                return LicenseService.getLicenseCondition(agencyCodes, licenseSeqNo, callerID);
            }
            catch (Exception e)
            {
                throw new ACAException(e);
            }
        }

        /// <summary>
        /// Method to get license types
        /// </summary>
        /// <param name="agencyCode">agency code</param>
        /// <returns>License Type Table</returns>
        public SortedList GetLicenseTypes(string agencyCode)
        {
            try
            {
                IBizDomainBll standChoiceBll = ObjectFactory.GetObject(typeof(IBizDomainBll)) as IBizDomainBll;
                IList<ItemValue> stdItems = standChoiceBll.GetBizDomainList(agencyCode, BizDomainConstant.STD_CAT_LICENSE_TYPE, false, 0);

                SortedList slLicense = new SortedList();

                if (stdItems != null &&
                    stdItems.Count > 0)
                {
                    foreach (ItemValue item in stdItems)
                    {
                        if (!slLicense.Contains(item.Key))
                        {
                            slLicense.Add(item.Key, item.Value);
                        }
                    }
                }

                return slLicense;
            }
            catch (Exception e)
            {
                throw new ACAException(e);
            }
        }

        /// <summary>
        /// Method to get license list that is valid.
        /// </summary>
        /// <param name="agenceCode">agency code</param>
        /// <param name="userSeqNbr">user sequence number</param>
        /// <param name="licenseModel">license model</param>
        /// <param name="needTemplateAttributes">if need template attributes.</param>
        /// <param name="capType">CapTypeModel object</param>
        /// <param name="queryFormat">The query format.</param>
        /// <returns>Array of LicenseModel4WS</returns>
        /// <exception cref="ACAException">Exception from web service.</exception>
        public SearchResultModel GetLicenseValidList(string agenceCode, long userSeqNbr, LicenseModel4WS licenseModel, bool needTemplateAttributes, CapTypeModel capType, QueryFormat queryFormat)
        {
            try
            {
                return ContractorLicenseService.getLicenseValidList(agenceCode, userSeqNbr, licenseModel, needTemplateAttributes, queryFormat, capType);
            }
            catch (Exception e)
            {
                throw new ACAException(e);
            }
        }

        /// <summary>
        /// get license that is valid to register.
        /// </summary>
        /// <param name="licenseModel4WS">licenseModel4WS object.</param>
        /// <returns>license date table</returns>
        /// <exception cref="ACAException">Exception from web service.</exception>
        public DataTable GetRegistralLicense(LicenseModel4WS licenseModel4WS)
        {
            try
            {
                List<LicenseModel4WS> licenseModelArray = new List<LicenseModel4WS>();
                LicenseModel4WS response = GetLicenseByStateLicNbr(licenseModel4WS);

                if (response != null)
                {
                    licenseModelArray.Add(response);
                }

                return BuildLicenseDataTable(licenseModelArray.ToArray());
            }
            catch (Exception e)
            {
                throw new ACAException(e);
            }
        }

        /// <summary>
        /// Method to add a license to a user.
        /// </summary>
        /// <param name="licenseModel">license model</param>
        /// <returns>-2---license is invalid    0---license is not exist     1---Already has the same type license   2---valid ok</returns>
        /// <exception cref="DataValidateException">{ <c>licenseModel</c> } is null</exception>
        /// <exception cref="ACAException">Exception from web service.</exception>
        int ILicenseBLL.IssueContractorLicense(LicenseModel4WS licenseModel)
        {
            if (licenseModel == null)
            {
                throw new DataValidateException(new string[] { "licenseModel" });
            }

            try
            {
                long licenseSeqNbr = 0;
                long.TryParse(licenseModel.licSeqNbr, out licenseSeqNbr);
                bool isAutoApproved = false;

                // get auto approved flag
                IBizDomainBll bizBll = (IBizDomainBll)ObjectFactory.GetObject(typeof(IBizDomainBll));
                IList<ItemValue> autoApprovedList = bizBll.GetBizDomainList(AgencyCode, BizDomainConstant.STD_LICENSE_AUTO_APPROVED, false);

                if (autoApprovedList != null && autoApprovedList.Count > 0)
                {
                    ItemValue autoApproved = autoApprovedList[0];

                    if (ValidationUtil.IsYes(autoApproved.Key))
                    {
                        isAutoApproved = true;
                    }
                }

                int response = ContractorLicenseService.issueContractorLicense(AgencyCode, User.UserSeqNum, licenseModel.licenseType, licenseModel.stateLicense, licenseSeqNbr, false, isAutoApproved, User.PublicUserId);

                return response;
            }
            catch (Exception e)
            {
                throw new ACAException(e);
            }
        }

        /// <summary>
        /// Get license array for checking expired license
        /// </summary>
        /// <param name="licenses">LicenseModel4WS array</param>
        /// <param name="capType">Cap Type  object</param>
        /// <returns>LicenseModel4WS array with user-role permission</returns>
        LicenseModel4WS[] ILicenseBLL.GetLicPermissionByLPTypeAndCapType(LicenseModel4WS[] licenses, CapTypeModel capType)
        {
            return LicenseService.getLicPermissionByLPTypeAndCapType(licenses, capType);
        }

        /// <summary>
        /// Get CAP ids with license(s) is/are invalid(expired and not available).
        /// </summary>
        /// <param name="capModels">CapModel4WS array need be checked.</param>
        /// <returns>invalid CapIDModel4WS array</returns>
        CapIDModel4WS[] ILicenseBLL.GetInvalidCapIDsByCheckLicense(CapModel4WS[] capModels)
        {
            return LicenseService.getInvalidCapIDsByCheckLicense(AgencyCode, capModels);
        }

        /// <summary>
        /// Query the license professional by license professional information.
        /// </summary>
        /// <param name="lpModel">The license professional.</param>
        /// <param name="needTemplateAttribute">if set to <c>true</c> [need template attribute].</param>
        /// <param name="format">The format.</param>
        /// <returns>A DataTable that contains matching licenses.
        /// The data table contains 9 columns below
        /// LicenseNumber
        /// LicenseType
        /// ContractName
        /// LicenseIssueDate
        /// LicenseExpirationDate
        /// Phone
        /// Fax
        /// Address1
        /// Address2</returns>
        /// <exception cref="DataValidateException">{ <c>lpModel</c> } is null</exception>
        /// <exception cref="ACAException">Exception from web service.</exception>
        /// <remarks>1. call getLicenseProfessionalsByLPModel method of LicenseWebService
        /// to return LicenseProfessionalModel list.
        /// 2. call BuildLicenseDataTable() method, convert the LicenseProfessionalModel list to a data table.
        /// 3. return the converted data table.</remarks>
        public LicenseProfessionalModel[] QueryLicenses(LicenseProfessionalModel lpModel, bool needTemplateAttribute, QueryFormat format)
        {
            if (lpModel == null)
            {
                throw new DataValidateException(new string[] { "lpModel" });
            }

            try
            {
                LicenseProfessionalModel[] lpSearchResult = LicenseService.getLicenseProfessionalsByLPModel(AgencyCode, lpModel, User.PublicUserId, needTemplateAttribute, format);
                CommonUtil.AdjustTime4LicenseProfessionalBirthDate(lpSearchResult);
                return lpSearchResult;
            }
            catch (Exception e)
            {
                throw new ACAException(e);
            }
        }

        /// <summary>
        /// Get License professional types by the AppSpecificInfoGroupModel4WS of parent cap
        /// </summary>
        /// <param name="appSpecificInfoGroups">Array of AppSpecificInfoGroupModel4WS</param>
        /// <returns>all license types of the selected services</returns>
        /// <exception cref="ACAException">Exception from web service.</exception>
        public string[] GetLPType(AppSpecificInfoGroupModel4WS[] appSpecificInfoGroups)
        {
            try
            {
                return LicenseService.getLPType(appSpecificInfoGroups, User.PublicUserId);
            }
            catch (Exception e)
            {
                throw new ACAException(e);
            }
        }

        /// <summary>
        /// Convert license professional model to data row.
        /// </summary>
        /// <param name="licenseProfessional">license professional model</param>
        /// <param name="rowIndex">The row index.</param>
        /// <returns>license professional model as a data row.</returns>
        public DataRow ConvertLPModelToDataRow(LicenseProfessionalModel licenseProfessional, int rowIndex)
        {
            DataRow drLicense = null;

            if (licenseProfessional != null)
            {
                DataTable dtLicense = ConstructMultipleLicenseDataTable();

                //reuse the build license professional data row function to add a new column "LicenseProfessionalModel".
                drLicense = BuildLicenseProfessionalDataRow(licenseProfessional, dtLicense, rowIndex);
                drLicense["LicenseProfessionalModel"] = licenseProfessional;
            }

            return drLicense;
        }

        /// <summary>
        /// convert LP model to data table for Multiple LP.
        /// </summary>
        /// <param name="lpModel">license professional model List</param>
        /// <returns>license professional data table</returns>
        public DataTable ConvertLPModelToDataTable(LicenseProfessionalModel[] lpModel)
        {
            List<string> licenseTypeNumList = new List<string>();

            DataTable dtLicense = ConstructMultipleLicenseDataTable();
            int rowIndex = 0;

            if (lpModel != null && lpModel.Length > 0)
            {
                foreach (LicenseProfessionalModel licenseProfessional in lpModel)
                {
                    if (licenseProfessional == null)
                    {
                        continue;
                    }

                    string licenseTypeNum = licenseProfessional.licenseType + licenseProfessional.licenseNbr;

                    if (licenseTypeNumList.Contains(licenseTypeNum))
                    {
                        continue;
                    }

                    DataRow dr = ConvertLPModelToDataRow(licenseProfessional, rowIndex);

                    if (dr != null)
                    {
                        DataRow drLicense = dtLicense.NewRow();
                        drLicense.ItemArray = dr.ItemArray;
                        dtLicense.Rows.Add(drLicense);
                        licenseTypeNumList.Add(licenseTypeNum);
                        rowIndex++;
                    }
                }
            }

            return dtLicense;
        }

        /// <summary>
        /// get license according to provider name, number and license information.
        /// for search licensee.
        /// </summary>
        /// <param name="license">License Model</param>
        /// <param name="provider">Provider Model</param>
        /// <param name="isFoodFacility">whether it is food facility</param>
        /// <param name="queryFormat">The query format.</param>
        /// <returns>License Model list</returns>
        public LicenseModel4WS[] GetLicenseList(LicenseModel4WS license, ProviderModel4WS provider, bool isFoodFacility, QueryFormat queryFormat)
        {
            return LicenseService.getLicenseList(AgencyCode, license, provider, isFoodFacility, queryFormat);
        }

        /// <summary>
        /// Get specified single LicenseModel4WS.
        /// </summary>
        /// <param name="license">Searched license model.</param>
        /// <param name="needTemplate">True indicates the model returned with TemplateModel, otherwise without.</param>
        /// <returns>Specified LicenseModel4WS.</returns>
        public LicenseModel4WS GetLicense(LicenseModel4WS license, bool needTemplate)
        {
            return LicenseService.getLicense(license, needTemplate);
        }

        /// <summary>
        /// get license according to search license and query format information.
        /// </summary>
        /// <param name="searchLicense">SearchLicense Model</param>
        /// <param name="needExperience">Need experience or not.</param>
        /// <param name="queryFormat">format model</param>
        /// <returns>License Model list</returns>
        public LicenseModel[] GetLicenseProfessionals(SearchLicenseModel searchLicense, bool needExperience, QueryFormat queryFormat)
        {
            return LicenseService.getLicenseProfessionals(searchLicense, needExperience, queryFormat);
        }

        /// <summary>
        /// Populate the LicenseModel4WS list to a DataTable.
        /// </summary>
        /// <param name="licenseModel4WSList">An array of LicenseModel4WS model.</param>
        /// <returns>A DataTable of licenses.</returns>
        public DataTable BuildLicenseDataTable(LicenseModel4WS[] licenseModel4WSList)
        {
            // create an empty datatable
            DataTable dtLicenses = ConstructLicenseDataTable();
            dtLicenses.Columns.Add("LicenseModel", typeof(LicenseModel4WS));

            if (licenseModel4WSList != null && licenseModel4WSList.Length > 0)
            {
                int rowIndex = 0;
                bool isEnableFeinMasking = IsEnableFeinMasking();

                foreach (LicenseModel4WS license in licenseModel4WSList)
                {
                    DataRow dr = dtLicenses.NewRow();

                    dr["RowIndex"] = rowIndex;
                    dr["LicenseNumber"] = license.stateLicense;
                    dr["LicenseType"] = license.licenseType;
                    dr["LicenseTypeText"] = I18nStringUtil.GetString(license.resLicenseType, license.licenseType);
                    dr["ContractName"] = string.Format("{0} {1} {2}", license.contactFirstName, license.contactMiddleName, license.contactLastName);
                    dr["LicenseIssueDate"] = I18nDateTimeUtil.ParseFromWebService4DataTable(license.licenseIssueDate);
                    dr["LicenseExpirationDate"] = I18nDateTimeUtil.ParseFromWebService4DataTable(license.licenseExpirationDate);

                    dr["PhoneIDD"] = license.phone1CountryCode;
                    dr["Phone"] = license.phone1;
                    dr["FaxIDD"] = license.faxCountryCode;
                    dr["Fax"] = license.fax;
                    dr["ContractorLicNO"] = license.contrLicNo;
                    dr["ContractorBusiName"] = license.contLicBusName;
                    dr["Address1"] = license.address1;
                    dr["Address2"] = license.address2;
                    dr["Address3"] = license.address3;
                    dr["LicenseSeqNumber"] = license.licSeqNbr;
                    dr["Salutation"] = license.salutation;
                    dr["FirstName"] = license.contactFirstName;
                    dr["MiddleName"] = license.contactMiddleName;
                    dr["LastName"] = license.contactLastName;
                    dr["Email"] = license.emailAddress;

                    dr["BirthDate"] = I18nDateTimeUtil.ParseFromWebService4DataTable(license.birthDate);
                    dr["Gender"] = license.gender;
                    dr["BusinessName"] = license.businessName;
                    dr["BusinessName2"] = license.busName2;
                    dr["BusinessLicense"] = license.businessLicense;
                    dr["Country"] = license.country;
                    dr["CountryCode"] = license.countryCode;
                    dr["City"] = license.city;
                    dr["State"] = license.state;
                    dr["Zip"] = license.zip;
                    dr["PostOfficeBox"] = license.postOfficeBox;
                    dr["MobilePhoneIDD"] = license.phone2CountryCode;
                    dr["MobilePhone"] = license.phone2;
                    dr["AgencyCode"] = license.serviceProviderCode;
                    dr["Suffix"] = license.suffixName;

                    dr["IsLicExpired"] = license.licExpired;
                    dr["IsInsExpired"] = license.insExpired;
                    dr["IsBusLicExpired"] = license.bizLicExpired;

                    dr["LicTypePermission"] = license.licTypePermission;

                    dr["ContactType"] = license.typeFlag;
                    dr["MaskedSSN"] = MaskUtil.FormatSSNShow(license.socialSecurityNumber);
                    dr["SSN"] = license.socialSecurityNumber;
                    dr["FEIN"] = license.fein;
                    dr["MaskedFEIN"] = MaskUtil.FormatFEINShow(license.fein, isEnableFeinMasking);
                    dr["LicenseModel"] = license;
                    dr[ColumnConstant.TEAMPLATE_ATTRIBUTE] = license.templateAttributes;

                    dtLicenses.Rows.Add(dr);
                    rowIndex++;
                }
            }

            return dtLicenses;
        }

        /// <summary>
        /// Populate the LicenseProfessionalModel list to a DataTable.
        /// </summary>
        /// <param name="licenseModelList">The license model list.</param>
        /// <returns>A DataTable of licenses.</returns>
        public DataTable BuildLicenseProfessionDataTable(LicenseProfessionalModel[] licenseModelList)
        {
            // create an empty datatable
            DataTable dtLicenses = ConstructLicenseDataTable();
            int rowIndex = 0;

            if (licenseModelList != null && licenseModelList.Length > 0)
            {
                foreach (LicenseProfessionalModel license in licenseModelList)
                {
                    DataRow dr = BuildLicenseProfessionalDataRow(license, dtLicenses, rowIndex);

                    dtLicenses.Rows.Add(dr);
                    rowIndex++;
                }
            }

            return dtLicenses;
        }

        /// <summary>
        /// Get ACA daily license sequence number.
        /// </summary>
        /// <param name="licenseProfessional">a license professional model.</param>
        /// <returns>string for license sequence number.</returns>
        string ILicenseBLL.GetDailyLicenseSeqNumber(LicenseProfessionalModel licenseProfessional)
        {
            return LicenseService.getDailyLicenseSeqNumber(AgencyCode, User.PublicUserId, licenseProfessional);
        }

        /// <summary>
        /// Indicates whether enable fein masking.
        /// </summary>
        /// <returns>true or false</returns>
        private bool IsEnableFeinMasking()
        {
            IXPolicyBll policyBll = (IXPolicyBll)ObjectFactory.GetObject(typeof(IXPolicyBll));
            string isEnableFeinMasking = policyBll.GetValueByKey(XPolicyConstant.ITEM_ENABLE_FEIN_MASKING);

            // if the value is 'Yes' or 'Y','Y' enable fein mask, otherwise disable it.
            return ValidationUtil.IsYes(isEnableFeinMasking);
        }

        /// <summary>
        /// Log info when debug is enabled.
        /// </summary>
        /// <param name="debugInfo">debug Info</param>
        private void LogDebugInfo(string debugInfo)
        {
            if (Logger.IsDebugEnabled)
            {
                Logger.Debug(debugInfo);
            }
        }

        /// <summary>
        /// build license professional model as a data row.
        /// </summary>
        /// <param name="license">license professional model</param>
        /// <param name="dtLicenses">data table of license model</param>
        /// <param name="rowIndex">Index of the row.</param>
        /// <returns>build license professional model to data row</returns>
        private DataRow BuildLicenseProfessionalDataRow(LicenseProfessionalModel license, DataTable dtLicenses, int rowIndex)
        {
            DataRow dr = dtLicenses.NewRow();

            dr["RowIndex"] = rowIndex;
            dr["LicenseNumber"] = license.licenseNbr;
            dr["LicenseType"] = license.licenseType;
            dr["LicenseTypeText"] = I18nStringUtil.GetString(license.resLicenseType, license.licenseType);
            dr["ContractName"] = license.contactName;
            
            if (license.licesnseOrigIssueDate == null)
            {
                dr["LicenseIssueDate"] = DBNull.Value;
            }
            else
            {
                dr["LicenseIssueDate"] = license.licesnseOrigIssueDate.Value;
            }

            if (license.licenseExpirDate == null)
            {
                dr["LicenseExpirationDate"] = DBNull.Value;
            }
            else
            {
                dr["LicenseExpirationDate"] = license.licenseExpirDate.Value;
            }

            dr["PhoneIDD"] = license.phone1CountryCode;
            dr["Phone"] = license.phone1;
            dr["FaxIDD"] = license.faxCountryCode;
            dr["Fax"] = license.fax;
            dr["ContractorLicNO"] = license.contrLicNo;
            dr["ContractorBusiName"] = license.contLicBusName;
            dr["Address1"] = license.address1;
            dr["Address2"] = license.address2;
            dr["Address3"] = license.address3;
            dr["LicenseSeqNumber"] = license.licSeqNbr;
            dr["Salutation"] = license.salutation;
            dr["FirstName"] = license.contactFirstName;
            dr["MiddleName"] = license.contactMiddleName;
            dr["LastName"] = license.contactLastName;
            dr["Email"] = license.email;

            if (license.birthDate == null)
            {
                dr["BirthDate"] = DBNull.Value;
            }
            else
            {
                dr["BirthDate"] = license.birthDate.Value;
            }

            dr["Gender"] = license.gender;
            dr["BusinessName"] = license.businessName;
            dr["BusinessName2"] = license.busName2;
            dr["BusinessLicense"] = license.businessLicense;
            dr["Country"] = license.country;
            dr["CountryCode"] = license.countryCode;

            dr["City"] = license.city;
            dr["State"] = license.state;
            dr["Zip"] = license.zip;
            dr["PostOfficeBox"] = license.postOfficeBox;
            dr["MobilePhoneIDD"] = license.phone2CountryCode;
            dr["MobilePhone"] = license.phone2;

            dr["ContactType"] = license.typeFlag;
            dr["MaskedSSN"] = MaskUtil.FormatSSNShow(license.socialSecurityNumber);
            dr["SSN"] = license.socialSecurityNumber;
            dr["MaskedFEIN"] = MaskUtil.FormatFEINShow(license.fein, IsEnableFeinMasking());
            dr["FEIN"] = license.fein;

            if (string.IsNullOrEmpty(license.agencyCode))
            {
                dr["AgencyCode"] = ACAConstant.AgencyCode;
                LogDebugInfo("license.agencyCode is null");
            }
            else
            {
                dr["AgencyCode"] = license.agencyCode;
            }

            dr[ColumnConstant.TEAMPLATE_ATTRIBUTE] = license.templateAttributes;

            return dr;
        }

        #endregion Methods
    }
}
