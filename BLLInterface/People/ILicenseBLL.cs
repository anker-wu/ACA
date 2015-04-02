#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: ILicenseBLL.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2008-2014
 *
 *  Description:
 *
 *  Notes:
 * $Id: ILicenseBLL.cs 277008 2014-08-09 08:37:50Z ACHIEVO\james.shi $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 *   10/12/2007      Daly zeng
 * </pre>
 */

#endregion Header

using System;
using System.Collections;
using System.Data;

using Accela.ACA.WSProxy;

namespace Accela.ACA.BLL.People
{
    /// <summary>
    /// Defines method signs for License.
    /// </summary>
    public interface ILicenseBLL
    {
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
        /// Address2
        /// </returns>
        DataTable ConstructLicenseDataTable();

        /// <summary>
        /// Method to delete a contractor license mapping for a user.
        /// </summary>
        /// <param name="agencyCode">agency code</param>
        /// <param name="licenseModel">license model</param>
        /// <param name="callID">caller id number.</param>
        /// <param name="userSeq">user sequence number</param>
        void DeleteContracotrLicensePK(string agencyCode, LicenseModel4WS licenseModel, string callID, string userSeq);

        /// <summary>
        /// Method to get contractor license list by user.
        /// </summary>
        /// <param name="agencyCode">agency code</param>
        /// <param name="userSeqNbr">user sequence number</param>
        /// <returns>Array of ContractorLicenseModel4WS</returns>
        ContractorLicenseModel4WS[] GetContrLicListByUserSeqNBR(string agencyCode, string userSeqNbr);

        /// <summary>
        /// Gets licenses of a contractor according to if license of contractor is valid.
        /// </summary>
        /// <param name="servProvCode">the Service Provider Code</param>
        /// <param name="userSeqNBR">the user sequence number</param>
        /// <param name="isValid">If true, get only valid licenses. else, get all licenses including invalid license.</param>
        /// <returns>Array of LicenseModel4WS</returns>
        LicenseModel4WS[] GetContractorLicenseValidList(string servProvCode, string userSeqNBR, bool isValid);

        /// <summary>
        /// get license by license sequence number
        /// </summary>
        /// <param name="licSeqNbr">license sequence number</param>
        /// <param name="userSeqNum">the user sequence number</param>
        /// <param name="servProvCode">the Service Provider Code</param>
        /// <returns>LicenseModel4WS object.</returns>
        LicenseModel4WS GetLicenseByLicSeqNbr(string licSeqNbr, string userSeqNum, string servProvCode);

        /// <summary>
        /// Gets license by state license number.
        /// If license type does not match the result license type, return null.
        /// </summary>
        /// <param name="licenseModel4WS">containing ServiceProviderCode and StateLicense</param>
        /// <returns>LicenseModel4WS object.</returns>
        LicenseModel4WS GetLicenseByStateLicNbr(LicenseModel4WS licenseModel4WS);

        /// <summary>
        /// Gets highest priority condition and notice conditions related with license.
        /// </summary>
        /// <remarks>
        /// 1. LicenseWebService. getLicenseCondition to return.
        /// </remarks>
        /// <param name="agencyCode">agency Code</param>
        /// <param name="licenseSeqNbr">license Sequence value.</param>
        /// <param name="callerID">public user ID</param>
        /// <returns>ParcelConditionModel4WS object.</returns>
        LicenseModel4WS GetLicenseCondition(string agencyCode, long licenseSeqNbr, string callerID);

        /// <summary>
        /// Gets highest priority condition and notice conditions related with license.
        /// </summary>
        /// <remarks>
        /// 1. LicenseWebService. getLicenseCondition to return.
        /// </remarks>
        /// <param name="agencyCodes">all selected services' agency codes</param>
        /// <param name="licenseSeqNo">license Sequence value.</param>
        /// <returns>LicenseModel4WS object.</returns>
        LicenseModel4WS GetLicenseCondition(string[] agencyCodes, long licenseSeqNo);

        /// <summary>
        /// Method to get license types
        /// </summary>
        /// <param name="agencyCode">agency code</param>
        /// <returns>Array of String</returns>
        SortedList GetLicenseTypes(string agencyCode);

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
        SearchResultModel GetLicenseValidList(string agenceCode, long userSeqNbr, LicenseModel4WS licenseModel, bool needTemplateAttributes, CapTypeModel capType, QueryFormat queryFormat);

        /// <summary>
        /// get license that is valid to register.
        /// </summary>
        /// <param name="licenseModel4WS">licenseModel4WS object.</param>
        /// <returns>license date table</returns>
        DataTable GetRegistralLicense(LicenseModel4WS licenseModel4WS);

        /// <summary>
        /// Method to add a license to a user.
        /// </summary>
        /// <param name="licenseModel">license model</param>
        /// <returns>  -2---license is invalid    0---license is not exist     1---Already has the same type license   2---valid ok</returns>
        int IssueContractorLicense(LicenseModel4WS licenseModel);

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
        /// <remarks>1. call getLicenseProfessionalsByLPModel method of LicenseWebService
        /// to return LicenseProfessionalModel list.
        /// 2. call BuildLicenseDataTable() method, convert the LicenseProfessionalModel list to a data table.
        /// 3. return the converted data table.</remarks>
        LicenseProfessionalModel[] QueryLicenses(LicenseProfessionalModel lpModel, bool needTemplateAttribute, QueryFormat format);

        /// <summary>
        /// Get License professional types by the AppSpecificInfoGroupModel4WS of parent cap 
        /// </summary>
        /// <param name="appSpecificInfoGroups">Array of AppSpecificInfoGroupModel4WS</param>
        /// <returns>all license types of the selected services</returns>
        string[] GetLPType(AppSpecificInfoGroupModel4WS[] appSpecificInfoGroups);

        /// <summary>
        /// Convert license professional model to data row.
        /// </summary>
        /// <param name="lpModel">license professional model</param>
        /// <param name="rowIndex">the row index.</param>
        /// <returns>license professional model as a data row.</returns>
        DataRow ConvertLPModelToDataRow(LicenseProfessionalModel lpModel, int rowIndex);

        /// <summary>
        /// convert LP model to data table for Multiple LP.
        /// </summary>
        /// <param name="lpModel">license professional model List</param>
        /// <returns>license professional data table</returns>
        DataTable ConvertLPModelToDataTable(LicenseProfessionalModel[] lpModel);

        /// <summary>
        /// Construct Multiple License DataTable format.
        /// </summary>
        /// <returns>data table</returns>
        DataTable ConstructMultipleLicenseDataTable();

        /// <summary>
        /// get license according to provider name, number and license information.
        /// for search licensee.
        /// </summary>
        /// <param name="license">License Model</param>
        /// <param name="provider">Provider Model</param>
        /// <param name="isFoodFacility">whether it is food facility</param>
        /// <param name="queryFormat">The query format.</param>
        /// <returns>License Model list</returns>
        LicenseModel4WS[] GetLicenseList(LicenseModel4WS license, ProviderModel4WS provider, bool isFoodFacility, QueryFormat queryFormat);
        
        /// <summary>
        /// Get specified single LicenseModel4WS.
        /// </summary>
        /// <param name="license">Searched license model.</param>
        /// <param name="needTemplate">True indicates the model returned with TemplateModel, otherwise without.</param>
        /// <returns>Specified LicenseModel4WS.</returns>
        LicenseModel4WS GetLicense(LicenseModel4WS license, bool needTemplate); 

        /// <summary>
        /// Get license array for checking expired license
        /// </summary>
        /// <param name="licenses">LicenseModel4WS array</param>
        /// <param name="capType">Cap Type  object</param>
        /// <returns>LicenseModel4WS array with user-role permission</returns>
        LicenseModel4WS[] GetLicPermissionByLPTypeAndCapType(LicenseModel4WS[] licenses, CapTypeModel capType);

        /// <summary>
        /// Get CAP ids with license(s) is/are invalid(expired and not available).
        /// </summary>
        /// <param name="capModels">CapModel4WS array need be checked.</param>
        /// <returns>invalid CapIDModel4WS array</returns>
        CapIDModel4WS[] GetInvalidCapIDsByCheckLicense(CapModel4WS[] capModels);

        /// <summary>
        /// get license according to search license and query format information.
        /// </summary>
        /// <param name="searchLicense">SearchLicense Model</param>
        /// <param name="needExperience">Need experience or not.</param>
        /// <param name="queryFormat">format model</param>
        /// <returns>License Model list</returns>
        LicenseModel[] GetLicenseProfessionals(SearchLicenseModel searchLicense, bool needExperience, QueryFormat queryFormat);

        /// <summary>
        /// Get ACA daily license sequence number.
        /// </summary>
        /// <param name="licenseProfessional">a license professional model.</param>
        /// <returns>string for license sequence number.</returns>
        string GetDailyLicenseSeqNumber(LicenseProfessionalModel licenseProfessional);

        /// <summary>
        /// Populate the LicenseModel4WS list to a DataTable.
        /// </summary>
        /// <param name="licenseModel4WSList">An array of LicenseModel4WS model.</param>
        /// <returns>A DataTable of licenses.</returns>
        DataTable BuildLicenseDataTable(LicenseModel4WS[] licenseModel4WSList);

        /// <summary>
        /// Populate the LicenseProfessionalModel list to a DataTable.
        /// </summary>
        /// <param name="licenseModelList">The license model list.</param>
        /// <returns>A DataTable of licenses.</returns>
        DataTable BuildLicenseProfessionDataTable(LicenseProfessionalModel[] licenseModelList);

        #endregion Methods
    }
}
