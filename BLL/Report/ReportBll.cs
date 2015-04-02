#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: Report.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2008-2014
 *
 *  Description:
 *
 *  Notes:
 * $Id: ReportBll.cs 277347 2014-08-14 06:51:12Z ACHIEVO\james.shi $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Accela.ACA.BLL.Common;
using Accela.ACA.Common;
using Accela.ACA.Common.Util;
using Accela.ACA.WSProxy;

namespace Accela.ACA.BLL.Report
{
    /// <summary>
    /// Deal with report business logic class.
    /// </summary>
    public class ReportBll : BaseBll, IReportBll
    {
        #region Fields

        /// <summary>
        /// initial permission.
        /// </summary>
        private const char PERMISSION_GRANT = '1';

        #endregion Fields

        #region Properties

        /// <summary>
        /// Gets an instance of ReportService.
        /// </summary>
        /// <value>The report service.</value>
        private ReportWebServiceService ReportService
        {
            get
            {
                return WSFactory.Instance.GetWebService<ReportWebServiceService>();
            }
        }

        #endregion Properties

        #region Methods

        /// <summary>
        /// For RTF report ,add all session parameter to ReportInfoModel, RTF Report use them in report possibly
        /// </summary>
        /// <param name="reportInfo">a ReportInfoModel4WS</param>
        /// <param name="sessionVariable">Session variable value</param>
        /// <returns>report information</returns>
        public ReportInfoModel4WS AddSessionVariableToParam(ReportInfoModel4WS reportInfo, Hashtable sessionVariable)
        {
            ArrayList alKeys = new ArrayList();
            ArrayList alValues = new ArrayList();
            string[] existedParameterKeys = reportInfo.reportParameterMapKeys;
            string[] existedParameterValues = reportInfo.reportParametersMapValues;

            if (!sessionVariable.ContainsKey(SessionVaraibleType.ModuleName.ToString()))
            {
                string module = ACAConstant.MODULE_ALL.Equals(reportInfo.module, StringComparison.InvariantCulture) ? string.Empty : reportInfo.module;
                sessionVariable.Add(SessionVaraibleType.ModuleName.ToString(), module);
            }

            if (existedParameterKeys != null && existedParameterKeys.Length > 0)
            {
                for (int i = 0; i < existedParameterKeys.Length; i++)
                {
                    alKeys.Add(existedParameterKeys[i]);
                    alValues.Add(existedParameterValues[i]);
                }
            }

            foreach (string curValue in Enum.GetNames(typeof(SessionContextValue)))
            {
                if (!IsExistParameter(existedParameterKeys, curValue))
                {
                    alKeys.Add(SessionConstant.SESSION_VARIABLE_PREFIX + curValue);
                    alValues.Add(GetSessionContextValue(curValue, sessionVariable));
                }
            }

            reportInfo.reportParameterMapKeys = (string[])alKeys.ToArray(typeof(string));
            reportInfo.reportParametersMapValues = (string[])alValues.ToArray(typeof(string));

            return reportInfo;
        }

        /// <summary>
        /// get parameters of a report.
        /// </summary>
        /// <param name="sessionVariable">Session variable.</param>
        /// <param name="reportInfoModel">The report info model.</param>
        /// <param name="reportType">Type of the report.</param>
        /// <returns>Parameter array.</returns>
        /// <exception cref="ACAException">Exception from web service.</exception>
        public ParameterModel4WS[] GetParametersByReportId(Hashtable sessionVariable, ReportInfoModel4WS reportInfoModel, string reportType)
        {
            string moduleName = (string)sessionVariable[SessionVaraibleType.ModuleName.ToString()];

            try
            {
                ParameterModel4WS[] parameterModels = null;

                if (reportInfoModel != null)
                {
                    parameterModels = ReportService.getParametersByReportId(reportInfoModel);
                    parameterModels = RemoveFixParamsForBtnReport(parameterModels, reportType);
                }

                //get session variable value to set parameter default value.
                if (parameterModels != null)
                {
                    ArrayList parameters = new ArrayList();

                    foreach (ParameterModel4WS param in parameterModels)
                    {
                        if (string.IsNullOrEmpty(param.dispDefaultValue))
                        {
                            param.dispDefaultValue = param.defaultValue;
                        }

                        ParameterModel4WS parameter = SetDefaultValueforOneSessionVariable(param, sessionVariable, moduleName);

                        if (ACAConstant.COMMON_N.Equals(parameter.parameterVisible) && ACAConstant.COMMON_Y.Equals(parameter.parameterRequired) && string.IsNullOrEmpty(parameter.dispDefaultValue))
                        {
                            parameter.parameterVisible = ACAConstant.COMMON_Y;
                        }

                        parameters.Add(parameter);
                    }

                    parameterModels = (ParameterModel4WS[])parameters.ToArray(typeof(ParameterModel4WS));

                    parameterModels = RemoveNullandInvisibleParams(parameterModels);
                }

                return parameterModels;
            }
            catch (Exception e)
            {
                throw new ACAException(e);
            }
        }

        /// <summary>
        /// Get report button property for trust account.
        /// </summary>
        /// <param name="reportButtonInfo">reportButtonInfo4WS model.</param>
        /// <returns>a ReportButtonProperty Model.</returns>
        public ReportButtonPropertyModel4WS GetReportButtonProperty4TrustAccount(ReportButtonInfoModel4WS reportButtonInfo)
        {
            return ReportService.getReportButtonProperty4TrustAccount(reportButtonInfo);
        }

        /// <summary>
        /// Gets report button to display in permit detail page.
        /// </summary>
        /// <param name="capID">Cap ID Model.</param>
        /// <param name="callerID">caller ID.</param>
        /// <param name="module">module name.</param>
        /// <returns>Array of ReportButtonPropertyModel for web service.</returns>
        /// <exception cref="DataValidateException">{ <c>capID, capID.serviceProviderCode, callerID, module</c> } is null</exception>
        /// <exception cref="ACAException">Exception from web service.</exception>
        public ReportButtonPropertyModel4WS[] GetReportButtonProperty(CapIDModel4WS capID, string callerID, string module)
        {
            if (capID == null || string.IsNullOrEmpty(capID.serviceProviderCode) || string.IsNullOrEmpty(callerID) || string.IsNullOrEmpty(module))
            {
                throw new DataValidateException(new string[] { "capID", "capID.serviceProviderCode", "callerID", "module" });
            }

            try
            {
                ReportButtonInfoModel4WS reportButtonInfo4WS = new ReportButtonInfoModel4WS();
                reportButtonInfo4WS.callerID = callerID;
                reportButtonInfo4WS.capID = capID;
                reportButtonInfo4WS.currentModule = module;

                ////if the method call by CapCompetions page,the capid module only have agency code.
                if (string.IsNullOrEmpty(capID.id1))
                {
                    reportButtonInfo4WS.multipleCaps = true;
                }
                //// if superAgency is null, that means common agency or superagency cap . If it is a
                //// sub cap in superAgency, replace the callID by delegateUser.
                string superAgency = string.Empty;

                if (this.IsSuperAgency && capID.serviceProviderCode.CompareTo(this.AgencyCode) != 0)
                {
                    superAgency = this.AgencyCode;
                }

                return ReportService.getReportButtonProperty(superAgency, reportButtonInfo4WS);
            }
            catch (Exception e)
            {
                throw new ACAException(e);
            }
        }

        /// <summary>
        /// Gets Report Detail info.
        /// </summary>
        /// <param name="reportId">report id.</param>
        /// <param name="agencyCode">agency code(super agency or sub agency or single agency).</param>
        /// <returns>ReportDetailModel4WS model.</returns>
        /// <exception cref="DataValidateException">{ <c>Report ID</c> } is null</exception>
        /// <exception cref="ACAException">Exception from web service.</exception>
        public ReportDetailModel4WS GetReportDetail(string reportId, string agencyCode)
        {
            if (string.IsNullOrEmpty(reportId))
            {
                throw new DataValidateException(new string[] { "Report ID" });
            }

            try
            {
                string callerID = this.User == null ? string.Empty : this.User.PublicUserId;

                return ReportService.getReportDetail(agencyCode, long.Parse(reportId), callerID);
            }
            catch (Exception e)
            {
                throw new ACAException(e);
            }
        }

        /// <summary>
        /// Gets reports of Report List to display in each page
        /// </summary>
        /// <param name="capID">CapIDModel for web service.</param>
        /// <param name="module">module name.</param>
        /// <param name="pageID">string page id.</param>
        /// <returns>Array of ReportButtonPropertyModel for web service.</returns>
        /// <exception cref="ACAException">Exception from web service.</exception>
        public ReportButtonPropertyModel4WS[] GetReportLinkProperty(CapIDModel4WS capID, string module, string pageID)
        {
            try
            {
                ReportButtonInfoModel4WS reportButtonInfo4WS = new ReportButtonInfoModel4WS();
                reportButtonInfo4WS.callerID = this.User == null ? string.Empty : this.User.PublicUserId;
                reportButtonInfo4WS.capID = capID == null ? null : capID;
                reportButtonInfo4WS.currentModule = module;

                ReportButtonPropertyModel4WS[] reports = ReportService.getReportLinkProperty(this.SuperAgencyCode, reportButtonInfo4WS);

                if (reports != null && reports.Length > 0)
                {
                    string category = ACAConstant.ACA_REPORT_ROLE_SETTING;
                    string levelType = string.IsNullOrEmpty(module) ? ACAConstant.LEVEL_TYPE_AGENCY : ACAConstant.LEVEL_TYPE_MODULE;
                    string levelData = string.IsNullOrEmpty(module) ? this.SuperAgencyCode : module;

                    IXPolicyBll policyBll = (IXPolicyBll)ObjectFactory.GetObject(typeof(IXPolicyBll));
                    XPolicyModel[] privileges = policyBll.GetPolicyListByCategory(category, levelType, levelData);
                    XPolicyModel[] pagePolicies = GetPolicyListByPage(levelType, levelData, pageID);

                    reports = FilterReports(reports, pagePolicies, privileges);
                }

                return reports;
            }
            catch (Exception e)
            {
                throw new ACAException(e);
            }
        }

        /// <summary>
        /// Get reports associated a single module and all module,if module name is null ,get reports associated all module.
        /// For ACA Admin.
        /// </summary>
        /// <param name="moduleName">Module Name,</param>
        /// <returns>ReportButtonPropertyModel4WS array.</returns>
        public ReportButtonPropertyModel4WS[] GetReportProperties(string moduleName)
        {
            ////just for admin.
            return new ReportButtonPropertyModel4WS[0];
        }

        /// <summary>
        /// Get Report Roles
        /// </summary>
        /// <param name="moduleName">module name.</param>
        /// <returns>a role string.</returns>
        public string GetReportRoles(string moduleName)
        {
            return string.Empty;
        }

        /// <summary>
        /// Get Reports by page id
        /// </summary>
        /// <param name="moduleName">module name</param>
        /// <param name="pageID">current page's id</param>
        /// <returns>empty string. it's for admin</returns>
        public string GetReportsByPage(string moduleName, string pageID)
        {
            return string.Empty;
        }

        /// <summary>
        /// Handles report main method by report web service.
        /// </summary>
        /// <param name="reportInfoModel4WS">ReportInfoModel for web service.</param>
        /// <param name="reportType">Report Type:1.PRINT_PERMIT_REPORT;2.PRINT_PERMIT_SUMMARY_REPORT3.PRINT_PAYMENT_RECEIPT_REPORT</param>
        /// <returns>ReportResultModel4WS report.</returns>
        /// <exception cref="DataValidateException">{ <c>reportInfoModel4WS, EDMSEntityIdModel, serviceProviderCode, callerID</c> } is null</exception>
        /// <exception cref="ACAException">Exception from web service.</exception>
        public ReportResultModel4WS HandleReport(ReportInfoModel4WS reportInfoModel4WS, string reportType)
        {
            if (reportInfoModel4WS == null || reportInfoModel4WS.edMSEntityIdModel == null || string.IsNullOrEmpty(reportInfoModel4WS.servProvCode) || string.IsNullOrEmpty(reportInfoModel4WS.callerId))
            {
                throw new DataValidateException(new string[] { "reportInfoModel4WS", "EDMSEntityIdModel", "serviceProviderCode", "callerID" });
            }

            try
            {
                // if superAgency is null, that means common agency or superagency cap . If it is a
                // sub cap in superAgency, replace the callID by delegateUser.
                string superAgency = string.Empty;
                if (this.IsSuperAgency && reportInfoModel4WS.servProvCode.CompareTo(this.AgencyCode) != 0)
                {
                    superAgency = this.AgencyCode;
                }

                return ReportService.handleReport(superAgency, reportInfoModel4WS, reportType);
            }
            catch (Exception e)
            {
                throw new ACAException(e);
            }
        }

        /// <summary>
        /// Send Report in Email
        /// </summary>
        /// <param name="reportInfoModel4WS">Report Info Model</param>
        /// <exception cref="DataValidateException">{ <c>reportInfoModel4WS, EDMSEntityIdModel, serviceProviderCode, callerID</c> } is null</exception>
        /// <exception cref="ACAException">Exception from web service.</exception>
        public void SendReportInEmail(ReportInfoModel4WS reportInfoModel4WS)
        {
            if (reportInfoModel4WS == null || reportInfoModel4WS.edMSEntityIdModel == null || string.IsNullOrEmpty(reportInfoModel4WS.servProvCode) || string.IsNullOrEmpty(reportInfoModel4WS.callerId))
            {
                throw new DataValidateException(new string[] { "reportInfoModel4WS", "EDMSEntityIdModel", "serviceProviderCode", "callerID" });
            }

            try
            {
                ReportService.sendReportInEmail(reportInfoModel4WS);
            }
            catch (Exception e)
            {
                throw new ACAException(e);
            }
        }

        /// <summary>
        /// if some parameter need be show in parameter form ,then the return value is true ,else the return value is false.
        /// </summary>
        /// <param name="reportParameters">all report parameters.</param>
        /// <returns>true / false.</returns>
        public bool HasShowParameter(ParameterModel4WS[] reportParameters)
        {
            bool needShowParameter = false;

            if (reportParameters == null || reportParameters.Length == 0)
            {
                return needShowParameter;
            }

            foreach (ParameterModel4WS param in reportParameters)
            {
                if (ACAConstant.COMMON_Y.Equals(param.parameterVisible))
                {
                    needShowParameter = true;
                    break;
                }
            }

            return needShowParameter;
        }

        /// <summary>
        /// get the reports that have privilege.
        /// </summary>
        /// <param name="reports">all reports for current module or home page.</param>
        /// <param name="privileges">privilege from session for reports of Report List.</param>
        /// <returns>ReportButtonPropertyModel4WS array.</returns>
        private ReportButtonPropertyModel4WS[] FilterReportByPrivilege(ReportButtonPropertyModel4WS[] reports, XPolicyModel[] privileges)
        {
            if (reports == null || reports.Length == 0 || privileges == null || privileges.Length == 0)
            {
                return reports;
            }

            ArrayList filteredReports = new ArrayList();

            foreach (ReportButtonPropertyModel4WS report in reports)
            {
                ReportButtonPropertyModel4WS needAddReport = GetOneReportHasPriviLege(report, privileges);
                if (needAddReport != null)
                {
                    filteredReports.Add(needAddReport);
                }
            }

            return (ReportButtonPropertyModel4WS[])filteredReports.ToArray(typeof(ReportButtonPropertyModel4WS));
        }

        /// <summary>
        /// Filter reports. first by page setting, then by role setting
        /// </summary>
        /// <param name="reports">all available reports</param>
        /// <param name="pagePolicies">reports stored in pages setting</param>
        /// <param name="privileges">report privileges</param>
        /// <returns>ReportButtonPropertyModel4WS array</returns>
        private ReportButtonPropertyModel4WS[] FilterReports(ReportButtonPropertyModel4WS[] reports, XPolicyModel[] pagePolicies, XPolicyModel[] privileges)
        {
            //Filter reports by page setting
            if (pagePolicies != null && pagePolicies.Length > 0)
            {
                // Get reports that stored in policy and visible is true
                // data2 - visible(Y/N)  data4 - report id
                var pageReports = from r in reports join p in pagePolicies on r.reportId equals p.data4 where p.data2.Equals(ACAConstant.COMMON_Y) select r;

                // Get all reports, including stored in policy and not stored yet.
                var showReports = pageReports.Union(from r in reports where !pagePolicies.Select(p => p.data4).Contains(r.reportId) select r);

                var orderedReports = showReports.OrderBy(p => p.resReportName);

                reports = orderedReports.ToArray();
            }

            return FilterReportByPrivilege(reports, privileges);
        }

        /// <summary>
        /// decide a report whether has privilege.if the report has privilege ,then return the report ,else return null.
        /// </summary>
        /// <param name="report">Report button property model.</param>
        /// <param name="privileges">all Privileges.</param>
        /// <returns>ReportButtonPropertyModel4WS model.</returns>
        private ReportButtonPropertyModel4WS GetOneReportHasPriviLege(ReportButtonPropertyModel4WS report, XPolicyModel[] privileges)
        {
            bool addedXPolicy = false;
            ReportButtonPropertyModel4WS currentReport = null;

            foreach (XPolicyModel privilege in privileges)
            {
                if (report.reportId != null && report.reportId.Equals(privilege.data4, StringComparison.InvariantCulture))
                {
                    if (HasPrivilege(privilege.data2))
                    {
                        currentReport = report;
                        break;
                    }

                    addedXPolicy = true;
                    break;
                }
            }

            if (!addedXPolicy)
            {
                currentReport = report;
            }

            return currentReport;
        }

        /// <summary>
        /// Get policy list by page id
        /// </summary>
        /// <param name="levelType">agency or module</param>
        /// <param name="levelData">agency code or module name</param>
        /// <param name="pageID">string page id</param>
        /// <returns>XPolicyModel array</returns>
        private XPolicyModel[] GetPolicyListByPage(string levelType, string levelData, string pageID)
        {
            IXPolicyBll xpolicyBll = (IXPolicyBll)ObjectFactory.GetObject(typeof(IXPolicyBll));
            List<XPolicyModel> policies = xpolicyBll.GetPolicyListByPolicyName(BizDomainConstant.STD_CAT_ACA_CONFIGS, AgencyCode);

            if (policies == null || policies.Count == 0)
            {
                return null;
            }

            // get policy list of page level. data3-page id
            var pagePolicies = from p in policies where p.data1 == ACAConstant.ACA_REPORT_PAGE_SETTING && p.level == levelType && p.levelData == levelData && p.data3 == pageID select p;

            return pagePolicies.ToArray();
        }

        /// <summary>
        /// Get values from session for 'SessionVariable' parameters.
        /// </summary>
        /// <param name="paramType">parameter default value ,It is parameter type</param>
        /// <param name="sessionVariable">Session Variable Value</param>
        /// <returns>parameter value from session</returns>
        private string GetSessionContextValue(string paramType, Hashtable sessionVariable)
        {
            CapModel4WS capModel = (CapModel4WS)sessionVariable[SessionVaraibleType.CapModel.ToString()];
            string moduleName = (string)sessionVariable[SessionVaraibleType.ModuleName.ToString()];
            string collectionID = (string)sessionVariable[SessionVaraibleType.CollectionID.ToString()];
            string trustAccountReceiptID = (string)sessionVariable[SessionVaraibleType.TrustAccountReceptID.ToString()];
            string transactionID = (string)sessionVariable[SessionVaraibleType.TransactionID.ToString()];

            string sessionValue = string.Empty;

            if (capModel != null && capModel.capID != null)
            {
                if (SessionContextValue.capID1.ToString().Equals(paramType))
                {
                    sessionValue = capModel.capID.id1 ?? string.Empty;
                }
                else if (SessionContextValue.capID2.ToString().Equals(paramType))
                {
                    sessionValue = capModel.capID.id2 ?? string.Empty;
                }
                else if (SessionContextValue.capID3.ToString().Equals(paramType))
                {
                    sessionValue = capModel.capID.id3 ?? string.Empty;
                }
                else if (SessionContextValue.altID.ToString().Equals(paramType))
                {
                    sessionValue = capModel.capID.customID ?? string.Empty;
                }
                else if (SessionContextValue.capID.ToString().Equals(paramType))
                {
                    StringBuilder sb = new StringBuilder();
                    sb.Append(capModel.capID.id1).Append(string.IsNullOrEmpty(capModel.capID.id2) ? string.Empty : ("-" + capModel.capID.id2)).Append(string.IsNullOrEmpty(capModel.capID.id3)
                                                                                                                                                          ? string.Empty : ("-" + capModel.capID.id3));
                    sessionValue = sb.ToString();
                }
                else if (SessionContextValue.parcelID.ToString().Equals(paramType) && capModel.parcelModel != null)
                {
                    sessionValue = capModel.parcelModel.parcelNo ?? string.Empty;
                }
            }

            if (this.User != null)
            {
                if (SessionContextValue.firstName.ToString().Equals(paramType))
                {
                    sessionValue = this.User.FirstName ?? string.Empty;
                }
                else if (SessionContextValue.middleName.ToString().Equals(paramType))
                {
                    sessionValue = this.User.MiddleName ?? string.Empty;
                }
                else if (SessionContextValue.lastName.ToString().Equals(paramType))
                {
                    sessionValue = this.User.LastName ?? string.Empty;
                }
                else if (SessionContextValue.userID.ToString().Equals(paramType))
                {
                    sessionValue = this.User.PublicUserId ?? string.Empty;
                }
                else if (SessionContextValue.publicUserID.ToString().Equals(paramType))
                {
                    sessionValue = this.User.UserID ?? string.Empty;
                }
                else if (SessionContextValue.userFullName.ToString().Equals(paramType))
                {
                    sessionValue = this.User.FullName ?? string.Empty;
                }
                else if (SessionContextValue.stateLicNum.ToString().Equals(paramType) && this.User.UserModel4WS.licenseModel != null && this.User.UserModel4WS.licenseModel.Length > 0)
                {
                    sessionValue = this.User.UserModel4WS.licenseModel[0].stateLicense ?? string.Empty;
                }
            }

            if (SessionContextValue.module.ToString().Equals(paramType))
            {
                sessionValue = moduleName ?? string.Empty;
            }
            else if (SessionContextValue.servProvCode.ToString().Equals(paramType))
            {
                sessionValue = this.AgencyCode ?? string.Empty;
            }
            else if (SessionContextValue.today.ToString().Equals(paramType))
            {
                sessionValue = I18nDateTimeUtil.FormatToDateStringForUI(System.DateTime.Now);
            }
            else if (SessionContextValue.collectionID.ToString().Equals(paramType))
            {
                sessionValue = collectionID ?? string.Empty;
            }
            else if (SessionContextValue.language.ToString().Equals(paramType))
            {
                //e.g. en
                sessionValue = I18nCultureUtil.GetLanguageCode(I18nCultureUtil.UserPreferredCultureInfo);
            }
            else if (SessionContextValue.trustAccountReceptID.ToString().Equals(paramType))
            {
                sessionValue = trustAccountReceiptID ?? string.Empty;
            }
            else if (SessionContextValue.transactionID.ToString().Equals(paramType))
            {
                sessionValue = transactionID ?? string.Empty;
            }

            return sessionValue;
        }

        /// <summary>
        /// if current user has license ,then the return value is true, else the return value is false.
        /// </summary>
        /// <returns><c>true</c> if [has available license]; otherwise, <c>false</c>.</returns>
        private bool HasAvailableLicense()
        {
            if (this.User == null || this.User.UserModel4WS == null || this.User.UserModel4WS.licenseModel == null || this.User.UserModel4WS.licenseModel.Length == 0)
            {
                return false;
            }

            bool hasLicense = false;
            LicenseModel4WS[] licenses = this.User.UserModel4WS.licenseModel;

            foreach (LicenseModel4WS license in licenses)
            {
                if (ACAConstant.VALID_STATUS.Equals(license.auditStatus, StringComparison.InvariantCultureIgnoreCase))
                {
                    hasLicense = true;
                    break;
                }
            }

            return hasLicense;
        }

        /// <summary>
        /// if user has one privilege in the following privileges: All ACA User/Anonymity User/Registered User/LP,
        /// then the return value is true, else the return value is false.
        /// </summary>
        /// <param name="privilege">a string with all privilege(for example:0100).</param>
        /// <returns><c>true</c> if the specified privilege has privilege; otherwise, <c>false</c>.</returns>
        private bool HasPrivilege(string privilege)
        {
            bool hasPrivilege = false;
            if (string.IsNullOrEmpty(privilege))
            {
                return hasPrivilege;
            }

            char[] privilegeArray = privilege.ToCharArray();

            ////for All ACA User
            if (privilegeArray.Length > 0 && PERMISSION_GRANT.Equals(privilegeArray[0]))
            {
                hasPrivilege = true;
            }
            else if (privilegeArray.Length > 1 && PERMISSION_GRANT.Equals(privilegeArray[1]) && (this.User == null || this.User.IsAnonymous))
            {
                ////for Anonymity User
                hasPrivilege = true;
            }
            else if (privilegeArray.Length > 2 && PERMISSION_GRANT.Equals(privilegeArray[2]) && this.User != null && !this.User.IsAnonymous && !this.User.IsAuthorizedAgent && !this.User.IsAgentClerk)
            {
                // in current report related logic scenario, Agent and Agent Clerk are not belonged to RegisteredUsers Group.
                ////for Registered User
                hasPrivilege = true;
            }
            else if (privilegeArray.Length > 3 && PERMISSION_GRANT.Equals(privilegeArray[3]) && HasAvailableLicense())
            {
                ////for license user
                hasPrivilege = true;
            }
            else if (privilegeArray.Length > 4 && PERMISSION_GRANT.Equals(privilegeArray[4]) && User != null && User.IsAuthorizedAgent)
            {
                // for agent user
                hasPrivilege = true;
            }
            else if (privilegeArray.Length > 5 && PERMISSION_GRANT.Equals(privilegeArray[5]) && User != null && User.IsAgentClerk)
            {
                // for agent clerk user
                hasPrivilege = true;
            }

            return hasPrivilege;
        }

        /// <summary>
        /// if the parameter has been added into report Info model ,return true ,else false.
        /// </summary>
        /// <param name="existedParameterKeys">added parameters.</param>
        /// <param name="parameterKey">current parameter.</param>
        /// <returns>true / false.</returns>
        private bool IsExistParameter(string[] existedParameterKeys, string parameterKey)
        {
            bool isExist = false;

            if (existedParameterKeys == null || existedParameterKeys.Length <= 0)
            {
                return isExist;
            }

            for (int i = 0; i < existedParameterKeys.Length; i++)
            {
                string paramKey = SessionConstant.SESSION_VARIABLE_PREFIX + parameterKey;
                if (paramKey.Equals(existedParameterKeys[i], StringComparison.InvariantCulture))
                {
                    isExist = true;
                    break;
                }
            }

            return isExist;
        }

        /// <summary>
        /// Remove the parameters which parameters' name are same as fixed parameters name of report button.
        /// </summary>
        /// <param name="reportParams">all report parameters.</param>
        /// <param name="reportType">report portlet.</param>
        /// <returns>the parameters.</returns>
        private ParameterModel4WS[] RemoveFixParamsForBtnReport(ParameterModel4WS[] reportParams, string reportType)
        {
            if (reportParams == null || string.IsNullOrEmpty(reportType))
            {
                return reportParams;
            }

            ArrayList parameters = new ArrayList();
            List<string> fixParamName = new List<string>();

            if (!ACAConstant.PRINT_REPORT_LIST.Equals(reportType, StringComparison.InvariantCulture))
            {
                fixParamName.AddRange(new string[] { ACAConstant.REPORT_AGENCY_ID, ACAConstant.REPORT_CAP_ID, ACAConstant.REPORT_BATCHTRANSACTION_NBR });

                if (ACAConstant.PRINT_PAYMENT_RECEIPT_REPORT.Equals(reportType, StringComparison.InvariantCulture))
                {
                    fixParamName.Add(ACAConstant.REPORT_RECEIPT_NUMBER);
                }
            }

            string[] fixParameterKeys = fixParamName.ToArray();

            foreach (ParameterModel4WS param in reportParams)
            {
                if (Array.IndexOf<string>(fixParameterKeys, param.parameterSource) == -1)
                {
                    parameters.Add(param);
                }
            }

            return (ParameterModel4WS[])parameters.ToArray(typeof(ParameterModel4WS));
        }

        /// <summary>
        /// remove null and invisible and un-required parameters.
        /// </summary>
        /// <param name="reportParams">original ParameterModel4WS models.</param>
        /// <returns>remove some parameter's ParameterModel4WS models.</returns>
        private ParameterModel4WS[] RemoveNullandInvisibleParams(ParameterModel4WS[] reportParams)
        {
            if (reportParams == null)
            {
                return null;
            }

            ArrayList parameters = new ArrayList();

            foreach (ParameterModel4WS param in reportParams)
            {
                if (ValidationUtil.IsYes(param.parameterRequired) || ValidationUtil.IsYes(param.parameterVisible) || !string.IsNullOrEmpty(param.defaultValue))
                {
                    parameters.Add(param);
                }
            }

            return (ParameterModel4WS[])parameters.ToArray(typeof(ParameterModel4WS));
        }

        /// <summary>
        /// set one session variable default value.
        /// </summary>
        /// <param name="param">original parameter model.</param>
        /// <param name="sessionVariable">Session Variables.</param>
        /// <param name="moduleName">module name.</param>
        /// <returns>set default value Parameter model.</returns>
        private ParameterModel4WS SetDefaultValueforOneSessionVariable(ParameterModel4WS param, Hashtable sessionVariable, string moduleName)
        {
            string defaultValue = param.defaultValue;
            string type = param.parameterType;

            if (ParameterType.SessionVariable.ToString().Equals(type, StringComparison.InvariantCulture))
            {
                if (!SessionContextValue.module.ToString().Equals(defaultValue))
                {
                    defaultValue = GetSessionContextValue(defaultValue, sessionVariable);
                    
                    if (!string.IsNullOrEmpty(defaultValue) || ValidationUtil.IsYes(param.parameterVisible))
                    {
                        param.defaultValue = defaultValue;
                        param.dispDefaultValue = defaultValue;
                    }
                }
                else
                {
                    param.dispDefaultValue = moduleName;
                }
            }

            return param;
        }

        #endregion Methods
    }
}
