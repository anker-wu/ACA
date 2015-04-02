#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: IReportBll.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2007-2014
 *
 *  Description:
 *
 *  Notes:
 * $Id: IReportBll.cs 277080 2014-08-11 06:28:07Z ACHIEVO\james.shi $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using System.Collections;

using Accela.ACA.WSProxy;

namespace Accela.ACA.BLL.Report
{
    /// <summary>
    /// interface of Report
    /// </summary>
    public interface IReportBll
    {
        #region Methods

        /// <summary>
        /// For RTF report ,add all session parameter to ReportInfoModel, RTF Report use them in report possibly
        /// </summary>
        /// <param name="reportInfo">a ReportInfoModel4WS</param>
        /// <param name="sessionVariable">Session variable value</param>
        /// <returns>report information</returns>
        ReportInfoModel4WS AddSessionVariableToParam(ReportInfoModel4WS reportInfo, Hashtable sessionVariable);

        /// <summary>
        /// get parameters of a report.
        /// </summary>
        /// <param name="sessionVariable">Session variable.</param>
        /// <param name="reportInfoModel">The report info model.</param>
        /// <param name="reportType">Type of the report.</param>
        /// <returns>
        /// Parameter array.
        /// </returns>
        ParameterModel4WS[] GetParametersByReportId(Hashtable sessionVariable, ReportInfoModel4WS reportInfoModel, string reportType);

        /// <summary>
        /// Get report button property for trust account.
        /// </summary>
        /// <param name="reportButtonInfo">reportButtonInfo4WS model.</param>
        /// <returns>a ReportButtonProperty Model.</returns>
        ReportButtonPropertyModel4WS GetReportButtonProperty4TrustAccount(ReportButtonInfoModel4WS reportButtonInfo);

        /// <summary>
        /// Gets report button to display in permit detail page.
        /// </summary>
        /// <param name="capID">Cap ID information.</param>
        /// <param name="callerID">string caller ID.</param>
        /// <param name="module">module name.</param>
        /// <returns>Array of Report Button Property.</returns>
        ReportButtonPropertyModel4WS[] GetReportButtonProperty(CapIDModel4WS capID, string callerID, string module);

        /// <summary>
        /// Gets Report Detail info
        /// </summary>
        /// <param name="reportId">report id.</param>
        /// <param name="agencyCode">agency code(super agency or sub agency or single agency).</param>
        /// <returns>Report Detail.</returns>
        ReportDetailModel4WS GetReportDetail(string reportId, string agencyCode);

        /// <summary>
        /// Gets report Link Report to display in each page.
        /// </summary>
        /// <param name="capID">Cap ID information.</param>
        /// <param name="module">module name</param>
        /// <param name="pageID">string page id</param>
        /// <returns>Array of Report Link Property.</returns>
        ReportButtonPropertyModel4WS[] GetReportLinkProperty(CapIDModel4WS capID, string module, string pageID);

        /// <summary>
        /// Get reports associated a single module and all module,if module name is null ,get reports associated all module.
        /// For ACA Admin.
        /// </summary>
        /// <param name="moduleName">Module Name</param>
        /// <returns>Report Button Property array</returns>
        ReportButtonPropertyModel4WS[] GetReportProperties(string moduleName);

        /// <summary>
        /// Get Report Roles
        /// </summary>
        /// <param name="moduleName">module name</param>
        /// <returns>json string for report role privilege</returns>
        string GetReportRoles(string moduleName);

        /// <summary>
        /// Get Reports by page id
        /// </summary>
        /// <param name="moduleName">module name</param>
        /// <param name="pageID">string page id</param>
        /// <returns>Json string for reports</returns>
        string GetReportsByPage(string moduleName, string pageID);

        /// <summary>
        /// Handles report main method by report web service. 
        /// </summary>
        /// <param name="reportInfoModel4WS">ReportInfoModel for web service</param>
        /// <param name="reportType">Report Type:1.PRINT_PERMIT_REPORT
        ///                                      2.PRINT_PERMIT_SUMMARY_REPORT
        ///                                      3.PRINT_PAYMENT_RECEIPT_REPORT</param>
        /// <returns>a ReportResultModel4WS</returns>
        ReportResultModel4WS HandleReport(ReportInfoModel4WS reportInfoModel4WS, string reportType);

        /// <summary>
        /// if some parameters need be fill in parameter window,return true.
        /// </summary>
        /// <param name="reportParameters">Report Parameters</param>
        /// <returns>true means show parameter window</returns>
        bool HasShowParameter(ParameterModel4WS[] reportParameters);

        /// <summary>
        /// Send Report in Email
        /// </summary>
        /// <param name="reportInfoModel4WS">Report Info Model</param>
        void SendReportInEmail(ReportInfoModel4WS reportInfoModel4WS);

        #endregion Methods
    }
}
