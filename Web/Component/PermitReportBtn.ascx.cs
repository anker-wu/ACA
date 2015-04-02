#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: PermitReportBtn.ascx.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2008-2014
 *
 *  Description:
 *
 *  Notes:
 *      $Id: PermitReportBtn.ascx.cs 278429 2014-09-03 11:08:02Z ACHIEVO\james.shi $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using System;
using System.Linq;
using System.Web.UI;
using Accela.ACA.BLL.Cap;
using Accela.ACA.BLL.Report;
using Accela.ACA.Common;
using Accela.ACA.Common.Common;
using Accela.ACA.Common.Log;
using Accela.ACA.Common.Util;
using Accela.ACA.Web.AuthorizedAgent;
using Accela.ACA.Web.Common;
using Accela.ACA.WSProxy;
using log4net;

namespace Accela.ACA.Web.Component
{
    /// <summary>
    /// This class provide the ability to operation PermitReport button.
    /// </summary>
    public partial class PermitReportBtn : BaseUserControl
    {
        #region Fields

        /// <summary>
        /// logger object.
        /// </summary>
        private static readonly ILog Logger = LogFactory.Instance.GetLogger(typeof(PermitReportBtn));

        /// <summary>
        /// Is auto issue.
        /// </summary>
        private string _isAutoIssue = string.Empty;

        #endregion Fields

        #region Properties

        /// <summary>
        /// Gets or sets the button properties.
        /// </summary>
        /// <value>
        /// The button properties.
        /// </value>
        public ReportButtonPropertyModel4WS[] ButtonProperties
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets a value indicating whether is in shopping cart or not.
        /// </summary>
        public bool IsInShoppingCart
        {
            get
            {
                if (ViewState["IsInShoppingCart"] == null)
                {
                    return false;
                }

                return (bool)ViewState["IsInShoppingCart"];
            }

            set
            {
                ViewState["IsInShoppingCart"] = value;
            }
        }

        /// <summary>
        /// Gets or sets receipt number.
        /// </summary>
        public string BatchTransactionNbr
        {
            get
            {
                if (ViewState["BatchTransactionNbr"] == null)
                {
                    return null;
                }
                else
                {
                    return (string)ViewState["BatchTransactionNbr"];
                }
            }

            set
            {
                ViewState["BatchTransactionNbr"] = value;
            }
        }

        /// <summary>
        /// Gets or sets capIdModel.
        /// </summary>
        public CapIDModel4WS CapID
        {
            get
            {
                if (ViewState["CapID"] == null)
                {
                    return null;
                }

                return (CapIDModel4WS)ViewState["CapID"];
            }

            set
            {
                ViewState["CapID"] = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether succeeded
        /// </summary>
        public bool Failed
        {
            get
            {
                if (ViewState["Failed"] == null)
                {
                    return true;
                }

                return (bool)ViewState["Failed"];
            }

            set
            {
                ViewState["Failed"] = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether force login to apply permit or not.
        /// </summary>
        public bool IsForceLoginToApplyPermit
        {
            get
            {
                if (ViewState["IsForceLoginToApplyPermit"] != null)
                {
                    return (bool)ViewState["IsForceLoginToApplyPermit"];
                }

                return true;
            }

            set
            {
                ViewState["IsForceLoginToApplyPermit"] = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether has fee.
        /// </summary>
        public bool HasFee
        {
            get
            {
                if (ViewState["HasFee"] == null)
                {
                    return false;
                }

                return Convert.ToBoolean(ViewState["HasFee"]);
            }

            set
            {
                ViewState["HasFee"] = value;
            }
        }

        /// <summary>
        /// Gets or sets receipt page Module list.
        /// </summary>
        public string ModuleList
        {
            get
            {
                return Convert.ToString(ViewState["ModuleList"]);
            }

            set
            {
                ViewState["ModuleList"] = value;
            }
        }

        /// <summary>
        /// Gets or sets BatchTransactionNumber. if the button in completion page, use this attribute.
        /// </summary>
        public string ReceiptNbr
        {
            get
            {
                if (ViewState["ReceiptNbr"] == null)
                {
                    return null;
                }
                
                return (string)ViewState["ReceiptNbr"];
            }

            set
            {
                ViewState["ReceiptNbr"] = value;
            }
        }

        /// <summary>
        /// Gets or sets Cap ID for sent email.
        /// </summary>
        public string CapIDArray
        {
            get
            {
                if (ViewState["CapIDArray"] == null)
                {
                    return null;
                }

                return (string)ViewState["CapIDArray"];
            }

            set
            {
                ViewState["CapIDArray"] = value;
            }
        }

        /// <summary>
        /// Gets a value indicating whether auto issue license flag.
        /// </summary>
        private bool IsAutoIssueLicense
        {
            get
            {
                if (_isAutoIssue == string.Empty)
                {
                    ICapBll capBll = ObjectFactory.GetObject<ICapBll>();

                    _isAutoIssue = capBll.IsAutoIssue4RenewalByChildCapID(CapID).ToString();
                }

                return Convert.ToBoolean(_isAutoIssue);
            }
        }

        /// <summary>
        /// Gets a value indicating whether the IsRenewal flag.
        /// </summary>
        private bool IsRenewal
        {
            get
            {
                return ACAConstant.COMMON_Y.Equals(
                                                    Request[ACAConstant.REQUEST_PARMETER_ISRENEWAL],
                                                    StringComparison.InvariantCultureIgnoreCase);
            }
        }

        /// <summary>
        /// Gets or sets stores License Report ID.
        /// </summary>
        private string LicenseReportID
        {
            get
            {
                if (ViewState[ACAConstant.PRINT_LICENSE_REPORT] == null)
                {
                    ViewState[ACAConstant.PRINT_LICENSE_REPORT] = ACAConstant.NONASSIGN_NUMBER;
                }

                return (string)ViewState[ACAConstant.PRINT_LICENSE_REPORT];
            }

            set
            {
                ViewState[ACAConstant.PRINT_LICENSE_REPORT] = value;
            }
        }

        /// <summary>
        /// Gets or sets Permit Report ID.
        /// </summary>
        private string PermitReportID
        {
            get
            {
                if (ViewState[ACAConstant.PRINT_PERMIT_REPORT] == null)
                {
                    ViewState[ACAConstant.PRINT_PERMIT_REPORT] = ACAConstant.NONASSIGN_NUMBER;
                }

                return (string)ViewState[ACAConstant.PRINT_PERMIT_REPORT];
            }

            set
            {
                ViewState[ACAConstant.PRINT_PERMIT_REPORT] = value;
            }
        }

        /// <summary>
        /// Gets or sets Receipt Report ID.
        /// </summary>
        private string ReceiptReportID
        {
            get
            {
                if (ViewState[ACAConstant.PRINT_PAYMENT_RECEIPT_REPORT] == null)
                {
                    ViewState[ACAConstant.PRINT_PAYMENT_RECEIPT_REPORT] = ACAConstant.NONASSIGN_NUMBER;
                }

                return (string)ViewState[ACAConstant.PRINT_PAYMENT_RECEIPT_REPORT];
            }

            set
            {
                ViewState[ACAConstant.PRINT_PAYMENT_RECEIPT_REPORT] = value;
            }
        }

        /// <summary>
        /// Gets or sets  Summary Report ID.
        /// </summary>
        private string SummaryReportID
        {
            get
            {
                if (ViewState[ACAConstant.PRINT_PERMIT_SUMMARY_REPORT] == null)
                {
                    ViewState[ACAConstant.PRINT_PERMIT_SUMMARY_REPORT] = ACAConstant.NONASSIGN_NUMBER;
                }

                return (string)ViewState[ACAConstant.PRINT_PERMIT_SUMMARY_REPORT];
            }

            set
            {
                ViewState[ACAConstant.PRINT_PERMIT_SUMMARY_REPORT] = value;
            }
        }

        /// <summary>
        /// Gets or sets stores print label report ID.
        /// </summary>
        private string PrintLabelReportID
        {
            get
            {
                if (ViewState[ACAConstant.PRINT_LABEL_VIEWID] == null)
                {
                    ViewState[ACAConstant.PRINT_LABEL_VIEWID] = ACAConstant.NONASSIGN_NUMBER;
                }

                return (string)ViewState[ACAConstant.PRINT_LABEL_VIEWID];
            }

            set
            {
                ViewState[ACAConstant.PRINT_LABEL_VIEWID] = value;
            }
        }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Page Load Event method.
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Failed)
            {
                // Display Report Button on page.
                DisplayReportButton();
                DisplayCloneButton();
                BindReportButtonUrl();
            }

            if (AppSession.IsAdmin)
            {
                if (IsRenewal)
                {
                    divPrintLicense.Visible = true;
                }
                else
                {
                    divPrintPermit.Visible = true;
                    divPrintSummary.Visible = true;
                }

                divPrintReceipt.Visible = true;

                if (AuthorizedAgentServiceUtil.HasAuthorizedServiceConfig() && !IsInShoppingCart && !IsRenewal)
                {
                    divPrintLabel.Visible = true;
                }

                if (CapID != null && !string.IsNullOrEmpty(CapID.serviceProviderCode) && !string.IsNullOrEmpty(CapID.id1))
                {
                    divCloneRecord.Visible = true;
                }
            }
        }

        /// <summary>
        /// <c>OnInit</c> event method
        /// </summary>
        /// <param name="e">A System.EventArgs object containing the event data.</param>
        protected override void OnInit(EventArgs e)
        {
            DialogUtil.RegisterScriptForDialog(Page);
        }

        /// <summary>
        /// Gets clone url used by UI page.
        /// </summary>
        /// <returns>Return clone url used by UI page.</returns>
        protected string GetClonePageUrl()
        {
            string cloneUrl = CloneRecordUtil.BuildClonePageUrl(TempModelConvert.Trim4WSOfCapIDModel(CapID), ModuleName);
            return cloneUrl;
        }

        /// <summary>
        /// Bind Report Button Url
        /// </summary>
        private void BindReportButtonUrl()
        {
            string batchTransactionNbr = BatchTransactionNbr;
            string reportModuleName = string.IsNullOrEmpty(ModuleName) ? ModuleList : ModuleName;

            CapModel4WS capModel = AppSession.GetCapModelFromSession(reportModuleName);
            string agencyCode = ConfigManager.AgencyCode;
            
            if (ValidationUtil.IsYes(Request.QueryString[ACAConstant.IS_SUBAGENCY_CAP]))
            {
                agencyCode = capModel.capID.serviceProviderCode;
            }

            //Receipt Report Button
            string receiptNbr = string.Empty;

            if (StandardChoiceUtil.IsSuperAgency() || StandardChoiceUtil.IsEnableShoppingCart())
            {
                receiptNbr = ACAConstant.NONASSIGN_NUMBER;
            }
            else
            {
                receiptNbr = ReceiptNbr;
            }

            if (!AppSession.IsAdmin)
            {
                //Permit Report Button
                string permitReportUrl = string.Format(
                                                       ACAConstant.REPORT_PARAMETER_PAGE + "?Module={0}&{1}={2}&{3}={4}&{5}={6}&{7}={8}&{9}={10}",
                                                       reportModuleName,
                                                       "reportType",
                                                       ACAConstant.PRINT_PERMIT_REPORT,
                                                       "reportID",
                                                       PermitReportID,
                                                       "batchTransactionNbr",
                                                       batchTransactionNbr,
                                                       ACAConstant.ID,
                                                       CapIDArray,
                                                       UrlConstant.AgencyCode,
                                                       agencyCode);
                lnkPrintPermit.Attributes.Add("onclick", "print_onclick('" + permitReportUrl + "');return false;");
   
                //Receipt Button
                string receiptReportUrl = string.Format(
                                                        ACAConstant.REPORT_PARAMETER_PAGE + "?Module={0}&{1}={2}&{3}={4}&{5}={6}&{7}={8}&{9}={10}&{11}={12}",
                                                        reportModuleName,
                                                        "reportType",
                                                        ACAConstant.PRINT_PAYMENT_RECEIPT_REPORT,
                                                        "RecepitNbr",
                                                        receiptNbr,
                                                        "reportID",
                                                        ReceiptReportID,
                                                        "batchTransactionNbr",
                                                        batchTransactionNbr,
                                                        ACAConstant.ID,
                                                        CapIDArray,
                                                        UrlConstant.AgencyCode,
                                                        agencyCode);
                lnkPrintReceipt.Attributes.Add("onclick", "print_onclick('" + receiptReportUrl + "');return false;");

                //Summary Report Button
                string summaryReportUrl = string.Format(
                                                        ACAConstant.REPORT_PARAMETER_PAGE + "?Module={0}&{1}={2}&{3}={4}&{5}={6}&{7}={8}&{9}={10}",
                                                        reportModuleName,
                                                        "reportType",
                                                        ACAConstant.PRINT_PERMIT_SUMMARY_REPORT,
                                                        "reportID",
                                                        SummaryReportID,
                                                        "batchTransactionNbr",
                                                        batchTransactionNbr,
                                                        ACAConstant.ID,
                                                        CapIDArray,
                                                        UrlConstant.AgencyCode,
                                                        agencyCode);
                lnkPrintSummary.Attributes.Add("onclick", "print_onclick('" + summaryReportUrl + "');return false;");

                //License Report Button
                string licenseReportUrl = string.Format(
                                                        ACAConstant.REPORT_PARAMETER_PAGE + "?Module={0}&{1}={2}&{3}={4}&{5}={6}&{7}={8}&{9}={10}",
                                                        reportModuleName,
                                                        "reportType",
                                                        ACAConstant.PRINT_LICENSE_REPORT,
                                                        "reportID",
                                                        LicenseReportID,
                                                        "batchTransactionNbr",
                                                        batchTransactionNbr,
                                                        ACAConstant.ID,
                                                        CapIDArray,
                                                        UrlConstant.AgencyCode,
                                                        agencyCode);
                lnkPrintLicense.Attributes.Add("onclick", "print_onclick('" + licenseReportUrl + "');return false;");
            }

            if (!IsInShoppingCart
                && (AppSession.User.IsAgentClerk || AppSession.User.IsAuthorizedAgent)
                && !AppSession.IsAdmin
                && AppSession.User.IsAuthAgentNeedPrinter)
            {
                try
                {
                    ParameterModel4WS[] parameters = ReportUtil.GetReportParameters(PrintLabelReportID, ACAConstant.PRINT_LABEL_VIEWID, reportModuleName);
                    var visibleParameters = parameters == null
                                                ? null
                                                : parameters.Where(o => ACAConstant.COMMON_Y.Equals(o.parameterVisible)).ToList();

                    IReportBll reportBll = ObjectFactory.GetObject<IReportBll>();
                    ReportDetailModel4WS detailModel = null;

                    string capId = string.IsNullOrEmpty(CapIDArray) ? capModel.capID.customID : CapIDArray;

                    if (!string.IsNullOrEmpty(PrintLabelReportID))
                    {
                        detailModel = reportBll.GetReportDetail(PrintLabelReportID, ConfigManager.AgencyCode);
                    }

                    if (detailModel != null)
                    {
                        if (!ReportUtil.HandlerReprint(detailModel, capId, false))
                        {
                            ScriptManager.RegisterStartupScript(Page, GetType(), "disablebutton" + Guid.NewGuid(), "<script>DisablePrintButton('" + lnkPrintLabel.ClientID + "', true);</script>", false);
                        }
                        else
                        {
                            string receiptLabelUrl =
                                string.Format(
                                            ACAConstant.REPORT_PARAMETER_PAGE
                                            + "?Module={0}&{1}={2}&{3}={4}&{5}={6}&{7}={8}&{9}={10}&{11}={12}&subID1={13}&subID2={14}&subID3={15}&subCustomerID={16}",
                                            reportModuleName,
                                            "reportType",
                                            ACAConstant.PRINT_LABEL_VIEWID,
                                            "RecepitNbr",
                                            receiptNbr,
                                            "reportID",
                                            PrintLabelReportID,
                                            "batchTransactionNbr",
                                            batchTransactionNbr,
                                            ACAConstant.ID,
                                            CapIDArray,
                                            UrlConstant.AgencyCode,
                                            ConfigManager.AgencyCode,
                                            capModel.capID.id1,
                                            capModel.capID.id2,
                                            capModel.capID.id3,
                                            capModel.capID.customID);

                            bool hasParameters = visibleParameters != null && visibleParameters.Count > 0;
                            receiptLabelUrl = FileUtil.AppendApplicationRoot(receiptLabelUrl.Replace("..", string.Empty));
                            string handlerPrintString = string.Format(
                                                                    "handlerPrint('{0}', '{1}', '{2}','{3}', '{4}', {5},'{6}');return false;",
                                                                    lnkPrintLabel.ClientID,
                                                                    reportModuleName,
                                                                    PrintLabelReportID,
                                                                    ConfigManager.AgencyCode,
                                                                    capId,
                                                                    hasParameters.ToString().ToLower(),
                                                                    receiptLabelUrl);
                            lnkPrintLabel.Attributes.Add("onclick", handlerPrintString);
                            bool isReprint = ReportUtil.IsLabelReprint(capId);

                            ScriptManager.RegisterStartupScript(this, GetType(), "registerrepring", "var isLabelRePrint=" + isReprint.ToString().ToLower() + ";", true);
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageUtil.ShowMessage(Page, MessageType.Error, ex.Message);
                }
            }
        }

        /// <summary>
        /// Display License Report Button.
        /// </summary>
        /// <param name="errorInfo">string for error Info </param>
        /// <param name="isDisplayed">is displayed.</param>
        /// <param name="reportID">report id number</param>
        /// <param name="reportName">report name.</param>
        private void DisplayLicenseReportBtn(string errorInfo, bool isDisplayed, string reportID, string reportName)
        {
            if (!string.IsNullOrEmpty(reportID))
            {
                lnkPrintLicense.ReportID = reportID;
            }

            if (string.IsNullOrEmpty(errorInfo) && isDisplayed)
            {
                divPrintLicense.Visible = true;
                lnkPrintLicense.ToolTip = reportName;
                LicenseReportID = reportID;
            }
            else
            {
                Logger.Error(errorInfo);
                divPrintLicense.Visible = false;
            }
        }

        /// <summary>
        /// Display Print Label Report Button.
        /// </summary>
        /// <param name="errorInfo">string for error Info </param>
        /// <param name="isDisplayed">is displayed.</param>
        /// <param name="reportID">report id number</param>
        /// <param name="reportName">report name.</param>
        private void DisplayPrintLabelReportBtn(string errorInfo, bool isDisplayed, string reportID, string reportName)
        {
            if (!string.IsNullOrEmpty(reportID))
            {
                lnkPrintLabel.ReportID = reportID;
            }

            if (string.IsNullOrEmpty(errorInfo) && isDisplayed)
            {
                divPrintLabel.Visible = true;
                lnkPrintLabel.ToolTip = reportName;
                PrintLabelReportID = reportID;
            }
            else
            {
                Logger.Error(errorInfo);
                divPrintLabel.Visible = false;
            }
        }

        /// <summary>
        /// Display Permit Report Button
        /// </summary>
        /// <param name="errorInfo">string for error Info </param>
        /// <param name="isDisplayed">is displayed.</param>
        /// <param name="reportID">report id number</param>
        /// <param name="reportName">report name.</param>
        private void DisplayPermitReportBtn(string errorInfo, bool isDisplayed, string reportID, string reportName)
        {
            if (!string.IsNullOrEmpty(reportID))
            {
                lnkPrintPermit.ReportID = reportID;
            }

            if (string.IsNullOrEmpty(errorInfo) && isDisplayed)
            {
                divPrintPermit.Visible = true;
                lnkPrintPermit.ToolTip = reportName;
                PermitReportID = reportID;
            }
            else
            {
                Logger.Error(errorInfo);
                divPrintPermit.Visible = false;
            }
        }

        /// <summary>
        /// display permit summary report button.
        /// </summary>
        /// <param name="errorInfo">string for error Info </param>
        /// <param name="isDisplayed">is displayed.</param>
        /// <param name="reportID">report id number</param>
        /// <param name="reportName">report name.</param>
        private void DisplayPermitSummaryReportBtn(string errorInfo, bool isDisplayed, string reportID, string reportName)
        {
            if (!string.IsNullOrEmpty(reportID))
            {
                lnkPrintSummary.ReportID = reportID;
            }

            if (string.IsNullOrEmpty(errorInfo) && isDisplayed)
            {
                divPrintSummary.Visible = true;
                lnkPrintSummary.ToolTip = reportName;
                SummaryReportID = reportID;
            }
            else
            {
                Logger.Error(errorInfo);
                divPrintSummary.Visible = false;
            }
        }

        /// <summary>
        /// display receipt report button.
        /// </summary>
        /// <param name="errorInfo">string for error Info </param>
        /// <param name="isDisplayed">is displayed.</param>
        /// <param name="reportID">report id number</param>
        /// <param name="reportName">report name.</param>
        private void DisplayReceiptReportBtn(string errorInfo, bool isDisplayed, string reportID, string reportName)
        {
            string receiptNbr = string.Empty;
            if (StandardChoiceUtil.IsSuperAgency() || StandardChoiceUtil.IsEnableShoppingCart())
            {
                receiptNbr = ACAConstant.NONASSIGN_NUMBER;
            }
            else
            {
                receiptNbr = ReceiptNbr;
            }

            if (!string.IsNullOrEmpty(reportID))
            {
                lnkPrintReceipt.ReportID = reportID;
            }

            if (string.IsNullOrEmpty(errorInfo) && isDisplayed && !string.IsNullOrEmpty(receiptNbr))
            {
                divPrintReceipt.Visible = true;
                lnkPrintReceipt.ToolTip = reportName;
                ReceiptReportID = reportID;
            }
            else
            {
                if (!string.IsNullOrEmpty(errorInfo))
                {
                    Logger.Error(errorInfo);
                }

                if (string.IsNullOrEmpty(receiptNbr))
                {
                    Logger.Error("Because no receipt number, Receipt Report Button will disappears in the page.");
                }

                divPrintReceipt.Visible = false;
            }
        }

        /// <summary>
        /// Display Report Button
        /// </summary>
        private void DisplayReportButton()
        {
            bool isEnableReportForAnonymousUser = StandardChoiceUtil.IsEnableReportForAnonymousUser();

            IReportBll reportBll = (IReportBll)ObjectFactory.GetObject(typeof(IReportBll));

            ReportButtonPropertyModel4WS[] arrayRBTModel = null;

            // In order to improve performance, move the web service call to CapCompletions page to reduce call times.
            if (IsInShoppingCart)
            {
                arrayRBTModel = ButtonProperties;
            }
            else
            {
                arrayRBTModel = reportBll.GetReportButtonProperty(CapID, AppSession.User.PublicUserId, ModuleName);
            }

            if (arrayRBTModel == null || arrayRBTModel.Length == 0)
            {
                return;
            }

            foreach (ReportButtonPropertyModel4WS ws in arrayRBTModel)
            {
                string buttonName = ws.buttonName;
                string errorInfo = ws.errorInfo;
                string reportID = ws.reportId;
                string reportName = I18nStringUtil.GetString(ws.resReportName, ws.reportName);
                bool isDisplayed = ws.isDisplayed;

                //if (ACAConstant.PRINT_PERMIT_REPORT.Equals(buttonName) && isEnableReportForAnonymousUser && !IsRenewal)
                if (ACAConstant.PRINT_PERMIT_REPORT.Equals(buttonName, StringComparison.InvariantCulture) && isEnableReportForAnonymousUser && !IsRenewal)
                {
                    DisplayPermitReportBtn(errorInfo, isDisplayed, reportID, reportName);
                    continue;
                }
                else if (HasFee && ACAConstant.PRINT_PAYMENT_RECEIPT_REPORT.Equals(buttonName, StringComparison.InvariantCulture) && isEnableReportForAnonymousUser)
                {
                    DisplayReceiptReportBtn(errorInfo, isDisplayed, reportID, reportName);
                    continue;
                }
                else if (ACAConstant.PRINT_PERMIT_SUMMARY_REPORT.Equals(buttonName, StringComparison.InvariantCulture) && !IsRenewal)
                {
                    DisplayPermitSummaryReportBtn(errorInfo, isDisplayed, reportID, reportName);
                    continue;
                }
                else if (ACAConstant.PRINT_LICENSE_REPORT.Equals(buttonName, StringComparison.InvariantCulture) && IsRenewal && IsAutoIssueLicense && !StandardChoiceUtil.IsSuperAgency())
                {
                    DisplayLicenseReportBtn(errorInfo, isDisplayed, reportID, reportName);
                    continue;
                }
                else if (ACAConstant.PRINT_LABEL_VIEWID.Equals(buttonName, StringComparison.InvariantCulture)
                    && !IsInShoppingCart
                    && (AppSession.User.IsAuthorizedAgent || AppSession.User.IsAgentClerk)
                    && AppSession.User.IsAuthAgentNeedPrinter)
                {
                    DisplayPrintLabelReportBtn(errorInfo, isDisplayed, reportID, reportName);
                    continue;
                }
            }
        }

        /// <summary>
        /// Display Clone button or not.
        /// </summary>
        private void DisplayCloneButton()
        {
            if (CapID != null && !string.IsNullOrEmpty(CapID.serviceProviderCode) && !string.IsNullOrEmpty(CapID.id1))
            {
                ICapTypeBll capTypeBll = (ICapTypeBll)ObjectFactory.GetObject(typeof(ICapTypeBll));
                CapTypeModel capType = capTypeBll.GetCapTypeByCapID(CapID);

                if (CloneRecordUtil.IsDisplayCloneButton(capType, TempModelConvert.Trim4WSOfCapIDModel(CapID), ModuleName, true) && !IsForceLoginToApplyPermit
                    && (AppSession.IsAdmin || FunctionTable.IsEnableCloneRecord()))
                {
                    divCloneRecord.Visible = true;
                }
            }
        }

        #endregion Methods
    }
}
