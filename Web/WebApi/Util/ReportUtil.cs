#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: AccountBLL.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2014
 *
 *  Description:
 *
 *  Notes:
 *  $Id:ReportUtil.cs 77905 2014-08-22 16:14:28Z ACHIEVO\Shine.yan $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion

using System;
using Accela.ACA.BLL.Finance;
using Accela.ACA.BLL.Report;
using Accela.ACA.Common;
using Accela.ACA.Common.Util;
using Accela.ACA.Web.Common;
using Accela.ACA.Web.WebApi.Entity.Adapter;
using Accela.ACA.WSProxy;

namespace Accela.ACA.Web.WebApi.Util
{
    /// <summary>
    /// New UI report utility
    /// </summary>
    public static class NewUiReportUtil
    {
        /// <summary>
        /// report button model
        /// </summary>
        private static CustomCapView4Ui reportButtonModel;

        /// <summary>
        /// Displays report buttons which are visible for the current user according to the Admin settings
        /// </summary>
        /// <param name="capIdModel">a capIdModel</param>      
        /// <param name="moduleName">module name</param>
        /// <returns>reportButton model</returns>
        public static CustomCapView4Ui DisplayReportButton(CapIDModel4WS capIdModel, string moduleName)
        {
            reportButtonModel = new CustomCapView4Ui();
            string receiptNbr = GetReceiptNbr(capIdModel);
            bool isEnableReportForAnonymousUser = StandardChoiceUtil.IsEnableReportForAnonymousUser();
            IReportBll reportBll = (IReportBll)ObjectFactory.GetObject(typeof(IReportBll));
            ReportButtonPropertyModel4WS[] arrayRBTModel = reportBll.GetReportButtonProperty(capIdModel, AppSession.User.PublicUserId, moduleName);

            foreach (ReportButtonPropertyModel4WS ws in arrayRBTModel)
            {
                string buttonName = ws.buttonName;
                string errorInfo = ws.errorInfo;
                string reportID = ws.reportId;
                string reportName = I18nStringUtil.GetString(ws.resReportName, ws.reportName);
                bool isDisplayed = ws.isDisplayed;

                if (ACAConstant.PRINT_PERMIT_REPORT.Equals(buttonName, StringComparison.InvariantCulture) && isEnableReportForAnonymousUser)
                {
                    DisplayPermitReportBtn(errorInfo, isDisplayed, reportID, buttonName, reportName);
                    continue;
                }
                else if (ACAConstant.PRINT_PAYMENT_RECEIPT_REPORT.Equals(buttonName, StringComparison.InvariantCulture) && isEnableReportForAnonymousUser)
                {
                    DisplayReceiptReportBtn(errorInfo, isDisplayed, receiptNbr, reportID, reportName, buttonName);
                    continue;
                }
                else if (ACAConstant.PRINT_PERMIT_SUMMARY_REPORT.Equals(buttonName, StringComparison.InvariantCulture))
                {
                    DisplayPermitSummaryReportBtn(errorInfo, isDisplayed, reportID, buttonName, reportName);
                    continue;
                }
            }

            return reportButtonModel;
        }

        /// <summary>
        /// Display Receipt Report Button
        /// </summary>
        /// <param name="errorInfo">error Info message</param>
        /// <param name="isDisplayed">is displayed.</param>
        /// <param name="receiptNbr">receipt Number</param>
        /// <param name="reportID">report id string.</param>
        /// <param name="reportName">report name</param>
        /// <param name="reportType">report Type</param>
        private static void DisplayReceiptReportBtn(string errorInfo, bool isDisplayed, string receiptNbr, string reportID, string reportName, string reportType)
        {           
            if (!string.IsNullOrEmpty(reportID))
            {
                reportButtonModel.PrintReceiptReportId = reportID;
            }

            if (string.IsNullOrEmpty(errorInfo) && isDisplayed && !string.IsNullOrEmpty(receiptNbr))
            {
                reportButtonModel.IsShowPrintReceipt = true;
                reportButtonModel.PrintReceiptReportName = reportName;
                reportButtonModel.PrintReceiptReportId = reportID;
                reportButtonModel.PrintReceiptReportType = reportType;
            }
            else
            {
                reportButtonModel.IsShowPrintReceipt = false;
            }
        }

        /// <summary>
        /// Display Permit Report Button
        /// </summary>
        /// <param name="errorInfo">error Info message</param>
        /// <param name="isDisplayed">is displayed.</param>
        /// <param name="reportID">report id string.</param>
        /// <param name="reportType">report type</param>
        /// <param name="reportName">report name</param>
        private static void DisplayPermitReportBtn(string errorInfo, bool isDisplayed, string reportID, string reportType, string reportName)
        {
            if (!string.IsNullOrEmpty(reportID))
            {
                reportButtonModel.PrintPermitReportId = reportID;
            }

            if (string.IsNullOrEmpty(errorInfo) && isDisplayed)
            {
                reportButtonModel.IsShowPrintPermit = true;
                reportButtonModel.PrintPermitReportName = reportName;
                reportButtonModel.PrintPermitReportType = reportType;
            }
            else
            {
                reportButtonModel.IsShowPrintPermit = false;
            }
        }

        /// <summary>
        /// Display Permit Summary Report Button
        /// </summary>
        /// <param name="errorInfo">error Info message.</param>
        /// <param name="isDisplayed">Is Displayed</param>
        /// <param name="reportID">report ID string.</param>
        /// <param name="reportType">report Type</param>
        /// <param name="reportName">report Name</param>
        private static void DisplayPermitSummaryReportBtn(string errorInfo, bool isDisplayed, string reportID, string reportType, string reportName)
        {
            if (!string.IsNullOrEmpty(reportID))
            {
                reportButtonModel.PrintSummaryReportId = reportID;
            }

            if (string.IsNullOrEmpty(errorInfo) && isDisplayed)
            {
                reportButtonModel.IsShowPrintSummary = true;
                reportButtonModel.PrintSummaryReportName = reportName;
                reportButtonModel.PrintSummaryReportType = reportType;
            }
            else
            {
                reportButtonModel.IsShowPrintSummary = false;
            }
        }

        /// <summary>
        /// Get Receipt Number
        /// </summary>
        /// <param name="capIdModel">capId Model</param>
        /// <returns>Receipt number</returns>
        private static string GetReceiptNbr(CapIDModel4WS capIdModel)
        {
            string receiptNbr = string.Empty;
            IOnlinePaymenBll paymentBll = (IOnlinePaymenBll)ObjectFactory.GetObject(typeof(IOnlinePaymenBll));
            receiptNbr = paymentBll.GetReceiptNoByCapID(capIdModel, null, AppSession.User.PublicUserId);
            return receiptNbr;
        }
    }
}