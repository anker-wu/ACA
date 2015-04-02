#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: CapCompletionBasePage.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2013-2014
 *
 *  Description:
 *
 *  Notes:
 *      $Id: CapCompletionBasePage.ascx.cs 251056 2013-06-07 07:09:27Z ACHIEVO\daniel.shi $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using System;
using System.Web.Services;
using Accela.ACA.BLL.Report;
using Accela.ACA.Common;
using Accela.ACA.Common.Log;
using Accela.ACA.WSProxy;
using log4net;

namespace Accela.ACA.Web.Common.CAP
{
    /// <summary>
    /// Cap Completion Base Page handler the common method
    /// </summary>
    public class CapCompletionBasePage : BasePage
    {
        /// <summary>
        /// The logger info.
        /// </summary>
        private static readonly ILog Logger = LogFactory.Instance.GetLogger(typeof(CapCompletionBasePage));

        /// <summary>
        /// Schedules the examinations.
        /// </summary>
        /// <param name="moduleName">Name of the module.</param>
        /// <param name="capId">The cap id.</param>
        /// <param name="reportId">The report id.</param>
        /// <param name="reprintReasonId">The reprint reason id.</param>
        /// <returns>The Label Report url.</returns>
        [WebMethod(EnableSession = true, Description = "Generate Label Report")]
        public static string GenerateLabelReport(string moduleName, string capId, string reportId, string reprintReasonId)
        {
            try
            {
                string serviceUrl = ReportUtil.GenerateReportFile(ACAConstant.PRINT_LABEL_VIEWID, reportId, moduleName, capId, reprintReasonId);
                Logger.InfoFormat("Generate the print label report successfully for {0}, the service url is {1}:", AppSession.User.PublicUserId, serviceUrl);

                return serviceUrl;
            }
            catch (Exception ex)
            {
                Logger.Error("Failed to generate the print label report for " + AppSession.User.PublicUserId + ". More detail:" + ex);
                throw ex;
            }
        }

        /// <summary>
        /// Handlers the reprint.
        /// </summary>
        /// <param name="agencyCode">The agency code.</param>
        /// <param name="moduleName">Name of the module.</param>
        /// <param name="reportId">The report id.</param>
        /// <param name="capId">The cap id.</param>
        /// <returns>The reprint's result.</returns>
        [WebMethod(EnableSession = true, Description = "Handler Report")]
        public static string HandlerReport(string agencyCode, string moduleName, string reportId, string capId)
        {
            try
            {
                bool isReprint = false;
                bool accessable = true;
                bool lastChance = false;
                IReportBll reportBll = ObjectFactory.GetObject<IReportBll>();
                ReportDetailModel4WS detailModel = null;

                if (!string.IsNullOrEmpty(reportId))
                {
                    detailModel = reportBll.GetReportDetail(reportId, agencyCode);
                }

                if (detailModel != null)
                {
                    accessable = ReportUtil.HandlerReprint(detailModel, capId, true, ref isReprint, ref lastChance);
                }

                string result = string.Format(
                    "\"RunPrint\":{0},\"IsReprint\":{1},\"LastChance\":{2}",
                    accessable.ToString().ToLower(),
                    isReprint.ToString().ToLower(),
                    lastChance.ToString().ToLower());
                return "{" + result + "}";
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                throw ex;
            }
        }
    }
}
