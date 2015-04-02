#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: DepositCompletion.ascx.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2010-2014
 *
 *  Description:
 *
 *  Notes:
 *      $Id: DepositCompletion.aspx.cs 277625 2014-08-19 07:06:19Z ACHIEVO\james.shi $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using System;
using System.Web;
using Accela.ACA.BLL.Report;
using Accela.ACA.Common;
using Accela.ACA.Common.Util;
using Accela.ACA.Web.Common;
using Accela.ACA.WSProxy;

namespace Accela.ACA.Web.Account
{
    /// <summary>
    /// deposit completion for trust account page.
    /// </summary>
    public partial class DepositCompletion : BasePage
    {
        #region private property

        /// <summary>
        /// Gets <c>BatchTransactionNbr</c>. if the button in completion page, use this attribute.
        /// </summary>
        private string ReceiptNbr
        {
            get
            {
                OnlinePaymentResultModel onlinePaymentResultModel = AppSession.GetOnlinePaymentResultModelFromSession();

                //long data type default value is 0;
                long receiptNbr = 0;

                if (onlinePaymentResultModel != null && onlinePaymentResultModel.entityPaymentResultModels != null && onlinePaymentResultModel.entityPaymentResultModels.Length > 0)
                {
                    //Only a element exist in entityPaymentResult array.
                    receiptNbr = onlinePaymentResultModel.entityPaymentResultModels[0].receiptNbr;
                }

                return receiptNbr == 0 ? string.Empty : receiptNbr.ToString();
            }
        }

        /// <summary>
        /// Gets trust account id.
        /// </summary>
        private string TrustAccountID
        {
            get
            {
                OnlinePaymentResultModel onlinePaymentResultModel = AppSession.GetOnlinePaymentResultModelFromSession();
                string trustAccountID = string.Empty;

                if (onlinePaymentResultModel != null && onlinePaymentResultModel.entityPaymentResultModels != null && onlinePaymentResultModel.entityPaymentResultModels.Length > 0)
                {
                    trustAccountID = onlinePaymentResultModel.entityPaymentResultModels[0].entityID;
                }

                return string.IsNullOrEmpty(trustAccountID) ? string.Empty : trustAccountID;
            }
        }

        /// <summary>
        /// Gets deposit amount.
        /// </summary>
        private double DepositAmount
        {
            get
            {
                OnlinePaymentResultModel onlinePaymentResultModel = AppSession.GetOnlinePaymentResultModelFromSession();
                double depositAmount = 0.0;

                if (onlinePaymentResultModel != null && onlinePaymentResultModel.entityPaymentResultModels != null && onlinePaymentResultModel.entityPaymentResultModels.Length > 0)
                {
                    depositAmount = onlinePaymentResultModel.entityPaymentResultModels[0].paidAmount;
                }

                return depositAmount;
            }
        }

        #endregion

        #region method

        /// <summary>
        /// Raises the page load event
        /// </summary>
        /// <param name="sender">An object that contains the event sender.</param>
        /// <param name="e">A System.EventArgs object containing the event data.</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            DepositSuccessInfo.Show(MessageType.Success, "aca_depositcompletion_text_depositsuccessinfo", MessageSeperationType.Bottom);
            if (AppSession.IsAdmin)
            {
                lblDepositCount.LabelKey = "aca_depositcompletion_label_depositaccount";
            }
            else
            {
                try
                {
                    if (!IsPostBack)
                    {
                        lblDepositCount.Text = string.Format(
                                                            GetTextByKey("aca_depositcompletion_label_depositaccount"),
                                                            I18nNumberUtil.FormatMoneyForUI(DepositAmount),
                                                            TrustAccountID);

                        //1. Get report button property information.
                        ReportButtonPropertyModel4WS reportButtonProperty = GetReportButtonProperty();

                        //2. Display receipty button by report button property model.
                        DisplayReceiptButton(reportButtonProperty);
                    }
                }
                catch (System.Exception ex)
                {
                }
            }
        }

        /// <summary>
        /// view permit detail link
        /// </summary>
        /// <param name="sender">Object sender.</param>
        /// <param name="e">EventArgs e.</param>
        protected void ViewAccountDetailButton_Click(object sender, EventArgs e)
        {
            Response.Redirect(string.Format("../Account/TrustAccountDetail.aspx?accountID={0}", HttpUtility.UrlEncode(TrustAccountID)));
        }

        /// <summary>
        /// Display receipt button.
        /// </summary>
        /// <param name="reportButtonProperty">a report button property model.</param>
        private void DisplayReceiptButton(ReportButtonPropertyModel4WS reportButtonProperty)
        {
            if (reportButtonProperty != null)
            {
                string buttonName = reportButtonProperty.buttonName;
                string errorInfo = reportButtonProperty.errorInfo;
                string reportID = reportButtonProperty.reportId;
                string reportName = I18nStringUtil.GetString(reportButtonProperty.resReportName, reportButtonProperty.reportName);

                if (ACAConstant.PRINT_TRUST_ACCOUNT_RECEIPT_REPORT.Equals(buttonName, StringComparison.InvariantCulture))
                {
                    if (string.IsNullOrEmpty(errorInfo))
                    {
                        lnkPrintReceipt.ToolTip = reportName;
                        lnkPrintReceipt.ReportID = reportID;
                        string receiptReportUrl = string.Format(ACAConstant.REPORT_PARAMETER_PAGE + "?reportType={0}&reportID={1}&recepitNbr={2}", ACAConstant.PRINT_TRUST_ACCOUNT_RECEIPT_REPORT, reportID, ReceiptNbr);
                        lnkPrintReceipt.Attributes.Add("href", "javascript:print_onclick('" + receiptReportUrl + "')");
                    }
                    else
                    {
                        lnkPrintReceipt.Visible = false;
                    }
                }
            }
        }

        /// <summary>
        /// Get report button property
        /// </summary>
        /// <returns>a report button property model.</returns>
        private ReportButtonPropertyModel4WS GetReportButtonProperty()
        {
            ReportButtonInfoModel4WS reportButtonInfo4WS = BuildReportButtonInfo();
            IReportBll reportBll = (IReportBll)ObjectFactory.GetObject(typeof(IReportBll));

            return reportBll.GetReportButtonProperty4TrustAccount(reportButtonInfo4WS);
        }

        /// <summary>
        /// build report button information.
        /// </summary>
        /// <returns>a report button information model.</returns>
        private ReportButtonInfoModel4WS BuildReportButtonInfo()
        {
            ReportButtonInfoModel4WS reportButtonInfo = new ReportButtonInfoModel4WS();
            reportButtonInfo.capID = new CapIDModel4WS();
            reportButtonInfo.capID.serviceProviderCode = ConfigManager.AgencyCode;
            reportButtonInfo.buttonViewID = GviewID.TrustAccoutReportButton;

            return reportButtonInfo;
        }

        #endregion method
    }
}
