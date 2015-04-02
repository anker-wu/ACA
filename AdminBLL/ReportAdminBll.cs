#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: Report.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2009-2014
 *
 *  Description:
 *
 *  Notes:
 * $Id: ReportAdminBll.cs 277080 2014-08-11 06:28:07Z ACHIEVO\james.shi $.
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
using Accela.ACA.BLL.Report;
using Accela.ACA.Common;
using Accela.ACA.Common.Common;
using Accela.ACA.Common.Log;
using Accela.ACA.Common.Util;
using Accela.ACA.WSProxy;

using log4net;

namespace Accela.ACA.AdminBLL
{
    /// <summary>
    /// Provided methods for reports.
    /// </summary>
    public class ReportAdminBll : BaseBll, IReportBll
    {
        #region Fields

        /// <summary>
        /// an instance of ILog
        /// </summary>
        private static readonly ILog Logger = LogFactory.Instance.GetLogger(typeof(ReportAdminBll));

        #endregion Fields

        #region Properties

        /// <summary>
        /// Gets an instance of ReportService.
        /// </summary>
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
            return new ReportInfoModel4WS();
        }

        /// <summary>
        /// get parameters of a report.
        /// </summary>
        /// <param name="sessionVariable">Session variable.</param>
        /// <param name="reportInfoModel">The report info model.</param>
        /// <param name="reportType">Type of the report.</param>
        /// <returns>Parameter array.</returns>
        public ParameterModel4WS[] GetParametersByReportId(Hashtable sessionVariable, ReportInfoModel4WS reportInfoModel, string reportType)
        {
            return new ParameterModel4WS[0];
        }

        /// <summary>
        /// Get report button property for trust account.
        /// </summary>
        /// <param name="reportButtonInfo">reportButtonInfo4WS model.</param>
        /// <returns>a ReportButtonProperty Model.</returns>
        public ReportButtonPropertyModel4WS GetReportButtonProperty4TrustAccount(ReportButtonInfoModel4WS reportButtonInfo)
        {
            return new ReportButtonPropertyModel4WS();
        }

        /// <summary>
        /// Gets report button to display in permit detail page.
        /// </summary>
        /// <param name="capID">Cap ID information.</param>
        /// <param name="callerID">string caller ID.</param>
        /// <param name="module">module name.</param>
        /// <returns>Array of Report Button Property.</returns>
        public ReportButtonPropertyModel4WS[] GetReportButtonProperty(CapIDModel4WS capID, string callerID, string module)
        {
            return new ReportButtonPropertyModel4WS[0];
        }

        /// <summary>
        /// Gets Report Detail info
        /// </summary>
        /// <param name="reportId">report id.</param>
        /// <param name="agencyCode">agency code(super agency or sub agency or single agency).</param>
        /// <returns>Report Detail.</returns>
        public ReportDetailModel4WS GetReportDetail(string reportId, string agencyCode)
        {
            return new ReportDetailModel4WS();
        }

        /// <summary>
        /// Gets report Link Report to display in each page.
        /// </summary>
        /// <param name="capID">Cap ID information.</param>
        /// <param name="module">module name</param>
        /// <param name="pageID">string page id</param>
        /// <returns>Array of Report Link Property.</returns>
        public ReportButtonPropertyModel4WS[] GetReportLinkProperty(CapIDModel4WS capID, string module, string pageID)
        {
            return new ReportButtonPropertyModel4WS[0];
        }

        /// <summary>
        /// Get reports associated a single module and all module,if module name is null ,get reports associated all module.
        /// For ACA Admin.
        /// </summary>
        /// <param name="moduleName">Module Name</param>
        /// <returns>Report Button Property array</returns>
        /// <exception cref="ACAException">Exception from web service.</exception>
        public ReportButtonPropertyModel4WS[] GetReportProperties(string moduleName)
        {
            if (Logger.IsDebugEnabled)
            {
                Logger.Debug("Begin ReportAdminBll.GetReportProperties()");
            }

            try
            {
                ReportButtonPropertyModel4WS[] response = ReportService.getReportProperties(AgencyCode, moduleName, ACAConstant.ADMIN_CALLER_ID);

                if (Logger.IsDebugEnabled)
                {
                    Logger.Debug("End ReportAdminBll.GetReportProperties()");
                }

                return response;
            }
            catch (Exception e)
            {
                throw new ACAException(e);
            }
        }

        /// <summary>
        /// Get Report Roles
        /// </summary>
        /// <param name="moduleName">module name</param>
        /// <returns>json string for report role privilege</returns>
        public string GetReportRoles(string moduleName)
        {
            string levelType = string.IsNullOrEmpty(moduleName) ? ACAConstant.LEVEL_TYPE_AGENCY : ACAConstant.LEVEL_TYPE_MODULE;
            string isModuleStr = string.IsNullOrEmpty(moduleName) ? ACAConstant.COMMON_N : ACAConstant.COMMON_Y;
            string levelData = string.IsNullOrEmpty(moduleName) ? AgencyCode : moduleName;
            StringBuilder roleDatas = new StringBuilder();

            //get report info
            ReportButtonPropertyModel4WS[] reports = GetReportProperties(moduleName);
            if (reports == null || reports.Length == 0)
            {
                return string.Format("var ReportRoleList=[[]];var isModule=\"{0}\";", isModuleStr);
            }

            //get privilege info
            IXPolicyBll xpolicyBll = (IXPolicyBll)ObjectFactory.GetObject(typeof(IXPolicyBll));
            XPolicyModel[] policyModels = xpolicyBll.GetPolicyListByCategory(ACAConstant.ACA_REPORT_ROLE_SETTING, levelType, levelData);

            //construct role string with report info and privilege info.
            roleDatas = roleDatas.Append("var ReportRoleList=[");

            if (policyModels == null || policyModels.Length == 0)
            {
                var roles = from r in reports select BuildReportPrivilegeJson(r, null);

                roleDatas.Append(roles.Aggregate((str1, str2) => str1 + str2));
            }
            else
            {
                var storedRoles = from p in policyModels join r in reports on p.data4 equals r.reportId select BuildReportPrivilegeJson(r, p);

                // get all reports data, if the report is new, set the role value as "1000"
                var roles = storedRoles.Union(from r in reports where !policyModels.Select(p => p.data4).Contains(r.reportId) select BuildReportPrivilegeJson(r, null));

                roleDatas.Append(roles.Aggregate((str1, str2) => str1 + str2));
            }

            roleDatas.Remove(roleDatas.Length - 1, 1);
            roleDatas.Append("];");
            roleDatas.AppendFormat("var isModule=\"{0}\";", isModuleStr);

            return roleDatas.ToString();
        }

        /// <summary>
        /// Get Reports by page id
        /// </summary>
        /// <param name="moduleName">module name</param>
        /// <param name="pageID">string page id</param>
        /// <returns>Json string for reports</returns>
        public string GetReportsByPage(string moduleName, string pageID)
        {
            string levelType = string.Empty;
            string levelData = string.Empty;

            // agency level
            if (string.IsNullOrEmpty(moduleName))
            {
                levelType = ACAConstant.LEVEL_TYPE_AGENCY;
                levelData = AgencyCode;
            }
            else
            {
                levelType = ACAConstant.LEVEL_TYPE_MODULE;
                levelData = moduleName;
            }

            //get report info
            ReportButtonPropertyModel4WS[] reports = GetReportProperties(moduleName);

            if (reports == null || reports.Length == 0)
            {
                return string.Empty;
            }

            //get policy data
            XPolicyModel[] policies = GetPolicyListByPage(levelType, levelData, pageID);

            //construct string with report info and policy info.
            StringBuilder sbReportJson = new StringBuilder("[");

            // if no data in policy table, get all reports and set the visible as true
            if (policies == null || policies.Length == 0)
            {
                var data = from r in reports select BuildReportPageJson(r, null);

                sbReportJson.Append(data.Aggregate((str1, str2) => str1 + str2));
            }
            else
            {
                // Union policy data and report data
                var storedDatas = from p in policies join r in reports on p.data4 equals r.reportId select BuildReportPageJson(r, p);

                // union policy and report data, if the report is new, set the visible is true
                var data = storedDatas.Union(from r in reports where !policies.Select(p => p.data4).Contains(r.reportId) select BuildReportPageJson(r, null));

                sbReportJson.Append(data.Aggregate((str1, str2) => str1 + str2));
            }

            sbReportJson.Remove(sbReportJson.Length - 1, 1); // remove the last ","
            sbReportJson.Append("]");

            return sbReportJson.ToString();
        }

        /// <summary>
        /// Handles report main method by report web service.
        /// </summary>
        /// <param name="reportInfoModel4WS">ReportInfoModel for web service</param>
        /// <param name="reportType">Report Type:1.PRINT_PERMIT_REPORT
        /// 2.PRINT_PERMIT_SUMMARY_REPORT
        /// 3.PRINT_PAYMENT_RECEIPT_REPORT</param>
        /// <returns>a ReportResultModel4WS</returns>
        public ReportResultModel4WS HandleReport(ReportInfoModel4WS reportInfoModel4WS, string reportType)
        {
            return new ReportResultModel4WS();
        }

        /// <summary>
        /// Send Report in Email
        /// </summary>
        /// <param name="reportInfoModel4WS">Report Info Model</param>
        public void SendReportInEmail(ReportInfoModel4WS reportInfoModel4WS)
        {
            return;
        }

        /// <summary>
        /// if some parameters need be fill in parameter window,return true.
        /// </summary>
        /// <param name="reportParameters">Report Parameters</param>
        /// <returns>true means show parameter window</returns>
        public bool HasShowParameter(ParameterModel4WS[] reportParameters)
        {
            return false;
        }

        /// <summary>
        /// Build json string for report page setting, including report id,report name and is visible flag.
        /// </summary>
        /// <param name="report">ReportButtonPropertyModel4WS-report model</param>
        /// <param name="policy">XPolicyModel-policy model</param>
        /// <returns>json string for report page setting</returns>
        private string BuildReportPageJson(ReportButtonPropertyModel4WS report, XPolicyModel policy)
        {
            string jsonFormat = "\"reportID\":\"{0}\",\"reportName\":\"{1}\",\"visible\":\"{2}\"";
            string visibleFlag = ACAConstant.COMMON_Y; // default is visible

            string reportID = report.reportId;
            string reportName = GetReportName(report.resReportName, report.reportName);

            if (policy != null)
            {
                visibleFlag = policy.data2; // data2 - visible of page setting, it's value is Y/N
            }

            string preBraces = "{";
            string afterBraces = "},";
            string content = string.Format(jsonFormat, reportID, reportName, visibleFlag);

            return preBraces + content + afterBraces; // build json string as {"reportID","reportName","visibleFlage"},
        }

        /// <summary>
        /// Build json string for report privilege. Include report id, report name and user privilege.
        /// </summary>
        /// <param name="report">ReportButtonPropertyModel4WS-report model</param>
        /// <param name="policy">XPolicyModel-policy model</param>
        /// <returns>json string</returns>
        private string BuildReportPrivilegeJson(ReportButtonPropertyModel4WS report, XPolicyModel policy)
        {
            string jsonFormat = "[\"{0}\",\"{1}\",{2}],";
            string privilege = "1,0,0,0,0,0"; // means all ACA user

            string reportName = GetReportName(report.resReportName, report.reportName);

            // if policy is not null, means the report has privilege stored in xpolicy.
            if (policy != null && policy.data2 != null)
            {
                string seed = string.Empty;

                // data2-user privilege
                privilege = policy.data2.ToCharArray().Aggregate(seed, (c1, c2) => c1 + "," + c2.ToString());
                jsonFormat = "[\"{0}\",\"{1}\"{2}],";
            }

            return string.Format(jsonFormat, report.reportId, reportName, privilege);
        }

        /// <summary>
        /// Get policy list by page id
        /// </summary>
        /// <param name="levelType">agency or module</param>
        /// <param name="levelData">agency name or module name</param>
        /// <param name="pageID">current page id</param>
        /// <returns>XPolicyModel[] array that has been configured in page</returns>
        private XPolicyModel[] GetPolicyListByPage(string levelType, string levelData, string pageID)
        {
            IXPolicyBll xpolicyBll = (IXPolicyBll)ObjectFactory.GetObject(typeof(IXPolicyBll));
            List<XPolicyModel> policies = xpolicyBll.GetPolicyListByPolicyName(BizDomainConstant.STD_CAT_ACA_CONFIGS, AgencyCode);

            if (policies == null || policies.Count == 0)
            {
                return null;
            }

            // get policy list of page level
            var pagePolicies = from p in policies where p.data1 == ACAConstant.ACA_REPORT_PAGE_SETTING && p.level == levelType && p.levelData == levelData && p.data3 == pageID select p;

            return pagePolicies.ToArray();
        }

        /// <summary>
        /// Get report name 
        /// </summary>
        /// <param name="stringArray">string array</param>
        /// <returns>report name</returns>
        private string GetReportName(params string[] stringArray)
        {
            string reportName = I18nStringUtil.GetString(stringArray);

            return ScriptFilter.EncodeJson(reportName);
        }

        #endregion Methods
    }
}