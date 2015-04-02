#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: FeeList.ascx.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2007-2014
 *
 *  Description:
 *
 *  Notes:
 *      $Id: FeeList.ascx.cs 277741 2014-08-20 10:31:02Z ACHIEVO\james.shi $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using System;
using System.Data;
using System.Web.UI.WebControls;

using Accela.ACA.BLL.Finance;
using Accela.ACA.Common;
using Accela.ACA.Common.Util;
using Accela.ACA.Web.Common;
using Accela.ACA.WSProxy;

namespace Accela.ACA.Web.Component
{
    /// <summary>
    /// This class provide the ability to operation Fee list. 
    /// </summary>
    public partial class FeeList : BaseUserControl
    {
        #region Fields

        /// <summary>
        /// Command for pay fees.
        /// </summary>
        private const string CMD_FAYFEES = "payfees";

        #endregion Fields

        #region Properties

        /// <summary>
        ///  Gets or sets no paid data source
        /// </summary>
        public DataTable NopaidGVDataSource
        {
            get
            {
                if (ViewState["NopaidGVDataSource"] != null)
                {
                    return (DataTable)ViewState["NopaidGVDataSource"];
                }

                return null;
            }

            set
            {
                ViewState["NopaidGVDataSource"] = value;
            }
        }

        /// <summary>
        ///  Gets or sets no paid data source
        /// </summary>
        public DataTable PaidGVDataSource
        {
            get
            {
                if (ViewState["PaidGVDataSource"] != null)
                {
                    return (DataTable)ViewState["PaidGVDataSource"];
                }

                return null;
            }

            set
            {
                ViewState["PaidGVDataSource"] = value;
            }
        }

        /// <summary>
        /// Gets or sets for receipt report ID.
        /// </summary>
        public string ReceiptReportID
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
        ///  Gets or sets repost URL
        /// </summary>
        public string ReportName
        {
            get
            {
                if (ViewState["ReportName"] != null)
                {
                    return (string)ViewState["ReportName"];
                }
                else
                {
                    return null;
                }
            }

            set
            {
                ViewState["ReportName"] = value;
            }
        }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Bind fee items list.
        /// </summary>
        /// <param name="capID">a CapIDModel4WS</param>
        public void Initial(CapIDModel4WS capID)
        {
            BindNopaidFeeItem(capID);
            BindPaidFeeItem(capID);
        }

        /// <summary>
        /// paid fees command.
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void Payfees_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == CMD_FAYFEES)
            {
                //TODO:
                Response.Redirect(string.Format("CapFees.aspx?Module={0}&permitType=PayFees&stepNumber=0&isPay4ExistingCap={1}", this.ModuleName, ACAConstant.COMMON_Y));
            }
            else
            {
                NopaidBind();
            }
        }

        /// <summary>
        /// GridView ViewDetails RowCommand
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">GridViewCommandEventArgs e</param>
        protected void ViewDetails_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            PaidBind();
        }

        /// <summary>
        /// Create a fee table, unpaid and paid table
        /// </summary>
        /// <returns>Data Table for fee.</returns>
        private static DataTable CreateFeeTable()
        {
            DataTable dt = new DataTable();

            dt.Columns.Add(new DataColumn("Date", typeof(DateTime)));
            dt.Columns.Add(new DataColumn("InvoiceNbr", typeof(string)));
            dt.Columns.Add(new DataColumn("Amount", typeof(string)));
            dt.Columns.Add(new DataColumn("Payfees", typeof(string)));
            dt.Columns.Add(new DataColumn("URL", typeof(string)));
            dt.Columns.Add(new DataColumn("ToolTipInfo", typeof(string)));
            dt.Columns.Add(new DataColumn("Operation", typeof(string)));

            return dt;
        }

        /// <summary>
        /// bind no paid fee data source
        /// </summary>
        /// <param name="capID">a CapIDModel4WS</param>
        private void BindNopaidFeeItem(CapIDModel4WS capID)
        {
            // Get all of fee items related to the current cap id.
            IFeeBll feeBll = ObjectFactory.GetObject<IFeeBll>();
            F4FeeModel4WS[] noPaidfees = feeBll.GetNoPaidFeeItemByCapID(capID, AppSession.User.PublicUserId);

            if (!AppSession.IsAdmin && (noPaidfees == null || noPaidfees.Length == 0))
            {
                return;
            }

            divFeeDetailSection.Visible = true;
            DataTable dtNoPaid = CreateFeeTable();
            double noPaidTotal = 0;
            bool isFirstItem = true;
            divOutstanding.Visible = AppSession.IsAdmin;

            foreach (F4FeeModel4WS fee in noPaidfees)
            {
                if (fee.f4FeeItemModel != null)
                {
                    //bool isEmptyInvoiceNbr = fee.x4FeeItemInvoiceModel.invoiceNbr.Equals(0) ? true : false;
                    bool isEmptyInvoiceNbr = fee.x4FeeItemInvoiceModel.invoiceNbr.Equals(0) ? true : false;

                    DataRow drItem = dtNoPaid.NewRow();

                    //drItem["Date"] = I18nDateTimeUtil.ConvertDateStringFromWebServiceToUI(fee.f4FeeItemModel.applyDate);
                    drItem["Date"] = I18nDateTimeUtil.ParseFromWebService4DataTable(fee.f4FeeItemModel.applyDate);
                    drItem["InvoiceNbr"] = isEmptyInvoiceNbr ? string.Empty : fee.x4FeeItemInvoiceModel.invoiceNbr.ToString();
                    drItem["Amount"] = I18nNumberUtil.FormatMoneyForUI(fee.f4FeeItemModel.fee);

                    //if item is existing, display first item.
                    if (isFirstItem)
                    {
                        drItem["Operation"] = GetTextByKey("per_feeDetails_label_unpaidPayment");
                        isFirstItem = false;
                    }

                    dtNoPaid.Rows.Add(drItem);
                    noPaidTotal += fee.f4FeeItemModel.fee;
                    divOutstanding.Visible = true;
                }
            }

            NopaidGVDataSource = dtNoPaid;
            lblUnpaidFeeAmount.Text = I18nNumberUtil.FormatMoneyForUI(noPaidTotal);
            NopaidBind();
        }

        /// <summary>
        /// bind paid fee data source
        /// </summary>
        /// <param name="capID">a CapIDModel4WS</param>
        private void BindPaidFeeItem(CapIDModel4WS capID)
        {
            // Get all of fee items related to the current cap id.
            IFeeBll feeBll = ObjectFactory.GetObject<IFeeBll>();
            F4FeeModel4WS[] paidfees = feeBll.GetPaidFeeItemByCapID(capID, AppSession.User.PublicUserId);

            if (!AppSession.IsAdmin && (paidfees == null || paidfees.Length == 0))
            {
                return;
            }

            divFeeDetailSection.Visible = true;
            divPaid.Visible = AppSession.IsAdmin;
            DataTable dtPaid = CreateFeeTable();
            double paidTotal = 0;

            foreach (F4FeeModel4WS fee in paidfees)
            {
                if (fee.f4FeeItemModel != null)
                {
                    //bool isEmptyInvoiceNbr = fee.x4FeeItemInvoiceModel.invoiceNbr.Equals(0) ? true : false;
                    bool isEmptyInvoiceNbr = fee.x4FeeItemInvoiceModel.invoiceNbr.Equals(0) ? true : false;

                    DataRow drItem = dtPaid.NewRow();

                    drItem["Date"] = I18nDateTimeUtil.ParseFromWebService4DataTable(fee.f4FeeItemModel.applyDate);
                    drItem["Amount"] = I18nNumberUtil.FormatMoneyForUI(fee.f4FeeItemModel.fee);
                    drItem["InvoiceNbr"] = isEmptyInvoiceNbr ? null : fee.x4FeeItemInvoiceModel.invoiceNbr.ToString();

                    string url = GetReportReceiptURL(fee.receiptNbr); //TODO:

                    drItem["URL"] = "JavaScript:feesSection_report_onclick('" + url + "')"; //TODO:
                    drItem["ToolTipInfo"] = ReportName; //TODO:
                    drItem["Operation"] = isEmptyInvoiceNbr ? null : GetTextByKey("per_feeDetails_label_paidViewDetails");
                    dtPaid.Rows.Add(drItem);
                    paidTotal += fee.f4FeeItemModel.fee;
                    divPaid.Visible = true;
                }
            }

            //initialize sort DESC in date line
            SortDataTable(ref dtPaid);
            PaidGVDataSource = dtPaid;
            lblPaidedFeeAmount.Text = I18nNumberUtil.FormatMoneyForUI(paidTotal);
            PaidBind();
        }

        /// <summary>
        /// get report receipt URL
        /// </summary>
        /// <param name="receiptNbr">receipt number</param>
        /// <returns>string receipt url.</returns>
        private string GetReportReceiptURL(long receiptNbr)
        {
            //Receipt Report Button
            string receiptReportUrl = string.Empty;

            receiptReportUrl = string.Format(ACAConstant.REPORT_PARAMETER_PAGE + "?Module={0}&{1}={2}&{3}={4}&{5}={6}", ModuleName, "reportType", ACAConstant.PRINT_PAYMENT_RECEIPT_REPORT, "RecepitNbr", receiptNbr, "reportID", ReceiptReportID);
            return receiptReportUrl;
        }

        /// <summary>
        /// Bind no paid items
        /// </summary>
        private void NopaidBind()
        {
            gdvFeeUnpaidList.DataSource = NopaidGVDataSource;
            gdvFeeUnpaidList.DataBind();
        }

        /// <summary>
        /// bind paid items
        /// </summary>
        private void PaidBind()
        {
            gdvFeePaidedList.DataSource = PaidGVDataSource;
            gdvFeePaidedList.DataBind();
        }

        /// <summary>
        /// initialize sort for line by date
        /// </summary>
        /// <param name="dt">data table.</param>
        private void SortDataTable(ref DataTable dt)
        {
            if (!IsPostBack)
            {
                DataView dv = dt.DefaultView;
                dv.Sort = "Date DESC";
                dt = dv.ToTable();
            }
        }

        #endregion Methods
    }
}