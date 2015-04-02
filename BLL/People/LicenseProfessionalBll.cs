#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: LicenseProfessionalBll.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2008-2014
 *
 *  Description:
 *
 *  Notes:
 * $Id: LicenseProfessionalBll.cs 277347 2014-08-14 06:51:12Z ACHIEVO\james.shi $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 *  2008-08-14           Kale.huang
 * </pre>
 */

#endregion Header

using System;
using System.Data;

using Accela.ACA.Common;
using Accela.ACA.Common.Util;
using Accela.ACA.WSProxy;

namespace Accela.ACA.BLL.People
{
    /// <summary>
    /// This class provide the ability to operation LP.
    /// </summary>
    public class LicenseProfessionalBll : BaseBll, ILicenseProfessionalBll
    {
        #region Properties

        /// <summary>
        /// Gets an instance of LicenseProfessionalService.
        /// </summary>
        private LicenseProfessionalWebServiceService LicenseProfessionalService
        {
            get
            {
                return WSFactory.Instance.GetWebService<LicenseProfessionalWebServiceService>();
            }
        }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Construct a new DataTable for Trade Licenses.
        /// </summary>
        /// <returns>a data table contains 5 columns below:
        /// TradeLicenseNumber
        /// PermitType
        /// capID1
        /// capID2
        /// capID3</returns>
        public DataTable ConstructTradeLicenseDataTable()
        {
            DataTable dtLicenses = new DataTable();

            dtLicenses.Columns.Add("TradeLicenseNumber");
            dtLicenses.Columns.Add("PermitType");
            dtLicenses.Columns.Add("DateEntered", typeof(DateTime));
            dtLicenses.Columns.Add("capID1");
            dtLicenses.Columns.Add("capID2");
            dtLicenses.Columns.Add("capID3");
            dtLicenses.Columns.Add("AgencyCode");

            return dtLicenses;
        }

        /// <summary>
        /// Construct a new DataTable for Trade Names.
        /// </summary>
        /// <returns>a data table contains 6 columns below:
        /// License Professional Number
        /// License Professional Type
        /// EnglishName
        /// ArabicName
        /// LicenseExpirationDate
        /// LicenseProfessionalStatus</returns>
        public DataTable ConstructTradeNameDataTable()
        {
            DataTable licenseTable = new DataTable();

            licenseTable.Columns.Add("LicenseNumber");
            licenseTable.Columns.Add("LicenseType");
            licenseTable.Columns.Add("EnglishName");
            licenseTable.Columns.Add("ArabicName");
            licenseTable.Columns.Add("LicenseExpirationDate", typeof(DateTime));
            licenseTable.Columns.Add("LicenseStatus");
            licenseTable.Columns.Add("LicenseSeqNbr");
            licenseTable.Columns.Add("AgencyCode");
            licenseTable.Columns.Add("SearchLicenseType");

            return licenseTable;
        }

        /// <summary>
        /// Get all Trade Names by public user ID.
        /// </summary>
        /// <param name="moduleName">module name</param>
        /// <returns>Array of LicenseModel4WS</returns>
        public LicenseModel4WS[] GetTradeNameList(string moduleName)
        {
            try
            {
                return LicenseProfessionalService.getTradeNames(AgencyCode, User.PublicUserId, moduleName, ACAConstant.REQUEST_PARMETER_TRADE_NAME);
            }
            catch (Exception e)
            {
                throw new ACAException(e);
            }
        }

        /// <summary>
        /// Method to get Trade Name list
        /// </summary>
        /// <param name="lpModel">License Model</param>
        /// <param name="capTypePickerName">"TradeName" or "TradeLicense" (hardcode)</param>
        /// <param name="moduleName">module name.</param>
        /// <param name="queryFormat">Query format for ACA</param>
        /// <returns>Array of LicenseModel4WS</returns>
        public SearchResultModel GetTradeNameListByLicenseModel(LicenseModel lpModel, string capTypePickerName, string moduleName, QueryFormat queryFormat)
        {
            try
            {
                return LicenseProfessionalService.getTradeNameListByLicenseModel(AgencyCode, User.PublicUserId, lpModel, capTypePickerName, moduleName, queryFormat);
            }
            catch (Exception e)
            {
                throw new ACAException(e);
            }
        }

        /// <summary>
        /// Get Trade License Caps associate to a trade name
        /// </summary>
        /// <param name="lpMode4WS">LicenseModel4WS of associated to a trade name(serviceProviderCode,license Sequence NBR,licenseType,stateLicense)</param>
        /// <param name="queryFormat">query Format</param>
        /// <returns>Array of Trade License</returns>
        /// <exception cref="ACAException">Exception from web service.</exception>
        public DataTable GetAssociatedLicense(LicenseModel4WS lpMode4WS, QueryFormat4WS queryFormat)
        {
            try
            {
                DataTable dtLicenses = ConstructTradeLicenseDataTable();
                CapIDModel4WS[] capIDModels = LicenseProfessionalService.getCapIDListByLP(lpMode4WS, queryFormat);

                if (capIDModels == null || 0 == capIDModels.Length)
                {
                    return dtLicenses;
                }

                Cap.ICapBll capBll = (Cap.ICapBll)ObjectFactory.GetObject(typeof(Cap.ICapBll));

                foreach (CapIDModel4WS capIDModel in capIDModels)
                {
                    CapWithConditionModel4WS capWithConditionModel = capBll.GetCapViewBySingle(capIDModel, this.User.UserSeqNum, ACAConstant.COMMON_N, IsSuperAgency);

                    if (capWithConditionModel != null && capWithConditionModel.capModel != null)
                    {
                        DataRow dr = dtLicenses.NewRow();

                        dr["TradeLicenseNumber"] = capWithConditionModel.capModel.altID ?? string.Empty;
                        dr["PermitType"] = Cap.CAPHelper.GetAliasOrCapTypeLabel(capWithConditionModel.capModel);
                        dr["capID1"] = capWithConditionModel.capModel.capID.id1;
                        dr["capID2"] = capWithConditionModel.capModel.capID.id2;
                        dr["capID3"] = capWithConditionModel.capModel.capID.id3;

                        dr["DateEntered"] = I18nDateTimeUtil.ParseFromWebService4DataTable(capWithConditionModel.capModel.fileDate);

                        if (capWithConditionModel.capModel.capID != null && !string.IsNullOrEmpty(capWithConditionModel.capModel.capID.serviceProviderCode))
                        {
                            dr["AgencyCode"] = capWithConditionModel.capModel.capID.serviceProviderCode;
                        }
                        else
                        {
                            dr["AgencyCode"] = ACAConstant.AgencyCode;
                        }

                        dtLicenses.Rows.Add(dr);
                    }
                }

                dtLicenses.DefaultView.Sort = "DateEntered DESC";

                return dtLicenses;
            }
            catch (Exception e)
            {
                throw new ACAException(e);
            }
        }

        /// <summary>
        /// Get License Professional list by Cap ID.
        /// </summary>
        /// <param name="agencyCode">agency code</param>
        /// <param name="capID">CapID model</param>
        /// <param name="callerID">user sequence number</param>
        /// <returns>license professional model list</returns>
        public LicenseProfessionalModel[] GetLPListByCapID(string agencyCode, CapIDModel4WS capID, string callerID)
        {
            try
            {
                LicenseProfessionalModel[] licenseProModelList = TempModelConvert.ConvertToLicenseProfessionalModelList(LicenseProfessionalService.getLPListByCapID(agencyCode, capID, callerID));

                if (licenseProModelList != null && licenseProModelList.Length > 0)
                {
                    foreach (LicenseProfessionalModel lp in licenseProModelList)
                    {
                        if (lp == null)
                        {
                            continue;
                        }

                        lp.agencyCode = lp.capID == null ? string.Empty : lp.capID.serviceProviderCode;
                    }
                }

                return licenseProModelList;
            }
            catch (Exception e)
            {
                throw new ACAException(e);
            }
        }

        /// <summary>
        /// Create or Edit License Professional model.
        /// </summary>
        /// <param name="agencyCode">agency code</param>
        /// <param name="licenses">license professional models with attribute.</param>
        /// <param name="callerID">caller id.</param>
        public void CreateOrUpdateLicenseProfessionals(string agencyCode, LicenseProfessionalModel[] licenses, string callerID)
        {
            try
            {
                LicenseProfessionalService.createOrUpdateLicenseProfessionals(agencyCode, TempModelConvert.ConvertToLicenseProfessionalModel4WSList(licenses), callerID);
            }
            catch (Exception e)
            {
                throw new ACAException(e);
            }
        }

        #endregion Methods
    }
}