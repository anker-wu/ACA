#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: ShowReport.aspx.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2009-2014
 *
 *  Description:
 *
 *  Notes:
 * $Id: ShowReport.aspx.cs 115461 2008-12-15 10:11:57Z ACHIEVO\jackie.yu $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using System;
using System.Collections;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

using Accela.ACA.BLL.Report;
using Accela.ACA.Common;
using Accela.ACA.Common.Common;
using Accela.ACA.Common.Log;
using Accela.ACA.Web.Common;
using Accela.ACA.WSProxy;

using log4net;

namespace Accela.ACA.Web.Report
{
    /// <summary>
    /// Show report main class
    /// </summary>
    public partial class ShowReport : BasePageWithoutMaster
    {
        #region Fields

        /// <summary>
        /// Logger object.
        /// </summary>
        private static readonly ILog Logger = LogFactory.Instance.GetLogger(typeof(ShowReport));

        #endregion Fields

        #region Properties

        /// <summary>
        /// Gets selected ALtIDs in permit list.
        /// </summary>
        private string AltIDs
        {
            get
            {
                string id = Request.QueryString[ACAConstant.ID];
                return string.IsNullOrEmpty(id) ? string.Empty : id;
            }
        }

        /// <summary>
        /// Gets or sets sub CAP ID.
        /// </summary>
        private CapIDModel4WS SubCapID
        {
            get
            {
                if (ViewState["subCapID"] == null)
                {
                    return null;
                }
                else
                {
                    return (CapIDModel4WS)ViewState["subCapID"];
                }
            }

            set
            {
                ViewState["subCapID"] = value;
            }
        }

        /// <summary>
        /// Gets or sets moduleName
        /// </summary>
        private string SubModule
        {
            get
            {
                if (ViewState["SubModule"] == null)
                {
                    return string.Empty;
                }
                else
                {
                    return (string)ViewState["SubModule"];
                }
            }

            set
            {
                ViewState["SubModule"] = value;
            }
        }

        #endregion Properties

        #region Methods

        /// <summary>
        /// page load function.
        /// </summary>
        /// <param name="sender">it is the sender from page.</param>
        /// <param name="e">event args.</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            SubCapID = ReportUtil.GetSubCapID();

            if (Request.QueryString["SubModule"] != null)
            {
                string subModule = Request.QueryString["SubModule"];
                SubModule = subModule;
            }

            try
            {
                string reportType = Request.QueryString["reportType"];
                string reportId = Request.QueryString["reportID"];
                CapIDModel4WS capIdModel = GetCapID();
                string moduleName = GetModuleName();

                if (!ReportUtil.CheckPermissionOnReport(reportType, reportId, capIdModel, moduleName))
                {
                    // Without permission, it needs to redirect to welcome page.
                    Response.Redirect(ACAConstant.URL_DEFAULT);
                }

                //if displaySendEmail = 'Y', then redirect to display send email page.
                RedirectToDisplaySendEmailPage(reportType);
                string emailReportName = ReportUtil.GetEmailReportName();
                ReportInfoModel4WS reportInfoModel4WS = ReportUtil.GetReportInfoModel4WS(reportType, reportId, SubCapID, ModuleName, emailReportName);

                IReportBll reportBll = (IReportBll)ObjectFactory.GetObject(typeof(IReportBll));

                ReportResultModel4WS reportResultModel4WS = reportBll.HandleReport(reportInfoModel4WS, reportType);

                OpenReport(reportResultModel4WS);
            }
            catch (ACAException ex)
            {
                Logger.Error(ex);
                ReportUtil.ShowError(Page, ModuleName, "aca_common_report_runningerror_label");
                return;
            }
        }

        /// <summary>
        /// Get Sub CAP ID Model
        /// </summary>
        /// <returns>CapID Model</returns>
        private CapIDModel4WS GetCapID()
        {
            CapIDModel4WS capId = ReportUtil.GetSubCapID();

            // Excluded when passing CAP ID List
            string capIDs = Server.UrlDecode(Request.QueryString[ACAConstant.ID]);
            if (capId == null && string.IsNullOrEmpty(capIDs))
            {
                CapModel4WS capModel = AppSession.GetCapModelFromSession(ModuleName);

                if (capModel != null && capModel.capID != null)
                {
                    capId = capModel.capID;
                }
            }

            return capId;
        }

        /// <summary>
        /// Get Module Name for Report Parameter
        /// </summary>
        /// <returns>Module Name</returns>
        private string GetModuleName()
        {
            string tmpModuleName = SubModule;

            if (string.IsNullOrEmpty(tmpModuleName) && !string.IsNullOrEmpty(ModuleName))
            {
                tmpModuleName = ModuleName;
            }

            return tmpModuleName;
        }

        /// <summary>
        /// To opens report file by ReportResultModel4WS.
        /// </summary>
        /// <param name="reportResultModel4WS">ReportResultModel for web service.</param>
        private void OpenReport(ReportResultModel4WS reportResultModel4WS)
        {
            try
            {
                if (!ReportUtil.IsValidReportResultModel(reportResultModel4WS))
                {
                    //If meet error show message
                    //"We are experiencing report configuration error. Please try again later or contact the City for assistance."
                    ReportUtil.ShowError(Page, ModuleName, "aca_common_report_configerror_label");
                    return;
                }

                byte[] reportContent = reportResultModel4WS.content;
                string[] reponseHeaders = reportResultModel4WS.responseHeaders;
                string[] reponseHeadersValues = reportResultModel4WS.responseHeadersValues;
                Hashtable reponseHeaderMap = new Hashtable();

                for (int i = 0; i < reponseHeaders.Length; i++)
                {
                    string httpHeaderKey = reponseHeaders[i] == null ? string.Empty : reponseHeaders[i].ToLowerInvariant();
                    string httpHeaderValue = reponseHeadersValues[i] == null ? string.Empty : reponseHeadersValues[i];
                    reponseHeaderMap.Add(httpHeaderKey, httpHeaderValue);
                }

                string contentType = (string)reponseHeaderMap["content-type"];
                string cntDisposition = (string)reponseHeaderMap["content-disposition"];

                if (cntDisposition != null && cntDisposition.IndexOf("attachment;", StringComparison.InvariantCulture) != -1)
                {
                    cntDisposition = cntDisposition.Replace("attachment;", string.Empty).Trim();
                }

                Response.Clear();
                Response.Buffer = true;

                //e.g. "application/vnd.ms-word";
                Response.ContentType = contentType;

                if (cntDisposition != null)
                {
                    Response.AddHeader("content-disposition", cntDisposition);
                    ////e.g. "inline;filename=" + reportName + ".doc"
                }

                Regex regCharset = new Regex(@"^.*charset\s*=\s*(?<CharSet>\w+)$", RegexOptions.IgnoreCase | RegexOptions.Singleline);
                string charSet = regCharset.IsMatch(contentType) ? regCharset.Match(contentType).Result("${CharSet}") : string.Empty;

                if (!string.IsNullOrEmpty(charSet))
                {
                    Response.AddHeader("content-type", "text/html; charset=UTF-8");
                    reportContent = Encoding.Convert(Encoding.GetEncoding(charSet), Encoding.UTF8, reportContent);
                }

                Response.AddHeader("content-length", reportContent.Length.ToString());
                Response.OutputStream.Write(reportContent, 0, reportContent.Length);

                Response.Flush();
                Response.Close();
                ////Response.End();
                ////Response.End throw exception as following.
                ////"Unable to evaluate expression because the code is optimized or a native frame is on top of the call stack."
                ////using HttpContext.Current.ApplicationInstance.CompleteRequest() method
                ////instead of Response.End to bypass the code execution to the Application_EndRequest event
                HttpContext.Current.ApplicationInstance.CompleteRequest();
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                throw new ACAException();
            }
        }

        /// <summary>
        /// Redirect to another page which display send email button.
        /// </summary>
        /// <param name="reportType">report Type</param>
        private void RedirectToDisplaySendEmailPage(string reportType)
        {
            CapIDModel4WS capID = ReportUtil.GetCapID(SubCapID, ModuleName);
            string agencyCode = ReportUtil.GetAgency(reportType, SubCapID);

            string reportID = ScriptFilter.AntiXssUrlEncode(HttpContext.Current.Request.QueryString["reportID"]);
            string isDisplaySendEmail = HttpContext.Current.Request.QueryString[ACAConstant.Display_Send_Email];

            IReportBll reportBll = (IReportBll)ObjectFactory.GetObject(typeof(IReportBll));
            ReportDetailModel4WS reportDetail = reportBll.GetReportDetail(reportID, agencyCode);

            if (!string.IsNullOrEmpty(reportID) && reportDetail != null
                && (string.IsNullOrEmpty(isDisplaySendEmail) || !ACAConstant.COMMON_N.Equals(isDisplaySendEmail)))
            {
                string reportAttachContact = reportDetail.attachContact;

                if (!string.IsNullOrEmpty(reportAttachContact)
                    && (ACAConstant.ATTACH_ALL_CAP_CONTACTS.Equals(reportAttachContact)
                            || ACAConstant.ATTACH_PRIMARY_CAP_CONTACTS.Equals(reportAttachContact)))
                {
                    string emailReportName = ReportUtil.GetEmailReportName();
                    Response.Redirect(string.Format("ReportPreview.aspx?{0}&{1}={2}&{3}={4}", HttpContext.Current.Request.QueryString.ToString(), ACAConstant.EMAIL_REPORT_NAME, ScriptFilter.AntiXssUrlEncode(emailReportName), ACAConstant.ATTACH_CONTACT_TYPE, reportDetail.attachContact));
                }
            }
        }
        #endregion Methods
    }
}
