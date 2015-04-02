#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: ILicenseProfessional.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2008-2014
 *
 *  Description:
 *
 *  Notes:
 * $Id: ILicenseProfessionalBll.cs 277004 2014-08-09 07:44:41Z ACHIEVO\james.shi $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 *   08/14/2008         Kale Huang
 * </pre>
 */

#endregion Header

using System.Data;

using Accela.ACA.WSProxy;

namespace Accela.ACA.BLL.People
{
    /// <summary>
    /// Defines method signs for License Professional.
    /// </summary>
    public interface ILicenseProfessionalBll
    {
        #region Methods

        /// <summary>
        /// Construct a new DataTable for Trade Names.
        /// </summary>
        /// <returns>a data table contains 6 columns below: 
        /// License Professional Number
        /// License Professional Type
        /// EnglishName
        /// ArabicName
        /// LicenseExpirationDate
        /// LicenseProfessionalStatus
        /// </returns>
        DataTable ConstructTradeNameDataTable();

        /// <summary>
        /// Get all Trade Names by public user ID.
        /// </summary>
        /// <param name="moduleName">module name</param>
        /// <returns>Array of LicenseModel4WS</returns>
        LicenseModel4WS[] GetTradeNameList(string moduleName);

        /// <summary>
        /// Method to get Trade Name list
        /// </summary>
        /// <param name="lpModel">LicenseModel4WS object.</param>
        /// <param name="capTypePickerName">"TradeName" or "TradeLicense" (hardcode)</param>
        /// <param name="moduleName">module Name</param>
        /// <param name="queryFormat">Query format for ACA</param>
        /// <returns>Array of LicenseModel4WS</returns>
        SearchResultModel GetTradeNameListByLicenseModel(LicenseModel lpModel, string capTypePickerName, string moduleName, QueryFormat queryFormat);

        /// <summary>
        /// Get Trade License Caps associate to a trade name
        /// </summary>
        /// <param name="lpMode4WS">LicenseModel4WS of associated to a trade name(serviceProviderCode,license Sequence NBR,licenseType,stateLicense)</param>
        /// <param name="queryFormat">query Format</param>
        /// <returns>Array of Trade License</returns>
        DataTable GetAssociatedLicense(LicenseModel4WS lpMode4WS, QueryFormat4WS queryFormat);

        /// <summary>
        /// Get License Professional list by Cap ID.
        /// </summary>
        /// <param name="agencyCode">agency code</param>
        /// <param name="capID">CapID model</param>
        /// <param name="callerID">user sequence number</param>
        /// <returns>license professional model list</returns>
        LicenseProfessionalModel[] GetLPListByCapID(string agencyCode, CapIDModel4WS capID, string callerID);

        /// <summary>
        /// Create or Edit License Professional model.
        /// </summary>
        /// <param name="agencyCode">agency code</param>
        /// <param name="licenses">license professional models with attribute.</param>
        /// <param name="callerID">Caller ID.</param>
        void CreateOrUpdateLicenseProfessionals(string agencyCode, LicenseProfessionalModel[] licenses, string callerID);
    
        #endregion Methods
    }
}