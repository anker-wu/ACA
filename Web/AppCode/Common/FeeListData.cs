#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: FeeListData.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2008-2014
 *
 *  Description:
 *
 *  Notes:
 *      $Id: FeeListData.cs 277071 2014-08-11 03:38:26Z ACHIEVO\james.shi $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using System;
using System.Data;
using System.Web;

using Accela.ACA.BLL.Finance;
using Accela.ACA.Common;
using Accela.ACA.Common.Util;
using Accela.ACA.WSProxy;

namespace Accela.ACA.Web.Common
{
    /// <summary>
    /// Fee List Utility Class
    /// </summary>
    public class FeeListData
    {
        #region Fields

        /// <summary>
        /// Related CAP ID
        /// </summary>
        private CapIDModel4WS _capID = null;
        
        /// <summary>
        /// Module Name
        /// </summary>
        private string _moduleName = "Building";
        
        /// <summary>
        /// No Paid Fee Data Source
        /// </summary>
        private DataTable _noPaidDataSource = null;
        
        /// <summary>
        /// No Paid Fee Total Amount
        /// </summary>
        private string _noPaidTotalAccount = string.Empty;
        
        /// <summary>
        /// Paid Fee Data Source
        /// </summary>
        private DataTable _paidDataSource = null;
        
        /// <summary>
        /// Paid Total Amount
        /// </summary>
        private string _paidTotalAccount = string.Empty;
        
        /// <summary>
        /// Receipt Report ID
        /// </summary>
        private string _receiptReportID = string.Empty;
        
        /// <summary>
        /// Report Name
        /// </summary>
        private string _reportName = string.Empty;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the FeeListData class
        /// </summary>
        /// <param name="capID">Related CAP ID</param>
        public FeeListData(CapIDModel4WS capID)
        {
            this.CapID = capID;
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// Gets Head columns
        /// </summary>
        public string[] HeadColumns
        {
            get
            {
                string[] _headColumns = 
                                { 
                                    LabelUtil.GetTextByKey("per_feeDetails_label_unpaidDate", ModuleName),
                                    LabelUtil.GetTextByKey("per_feeDetails_label_unpaidInvoiceNbr", ModuleName),
                                    LabelUtil.GetTextByKey("per_feeDetails_label_unpaidAmount", ModuleName),
                                    string.Empty
                                }; 
                return _headColumns;
            }
        }

        /// <summary>
        /// Gets or sets ModelName
        /// </summary>        
        public string ModuleName
        {
            get
            {
                return _moduleName;
            }

            set
            {
                _moduleName = value;
            }
        }

        /// <summary>
        /// Gets or sets NoPaid DataSource
        /// </summary>
        public DataTable NoPaidDataSource
        {
            get
            {
                object o = HttpContext.Current.Session["NoPaidDataSource"];
                return o != null ? (DataTable)o : null;
            }

            set
            {
                HttpContext.Current.Session["NoPaidDataSource"] = value;
            }
        }

        /// <summary>
        /// Gets or sets NoPaid Total Account
        /// </summary>
        public string NoPaidTotalAccount
        {
            get
            {
                object o = HttpContext.Current.Session["NoPaidTotalAccount"];
                return o != null ? (string)o : null;
            }

            set
            {
                HttpContext.Current.Session["NoPaidTotalAccount"] = value;
            }
        }

        /// <summary>
        /// Gets or sets Paid DataSource
        /// </summary>
        public DataTable PaidDataSource
        {
            get
            {
                object o = HttpContext.Current.Session["PaidDataSource"];
                return o != null ? (DataTable)o : null;
            }

            set
            {
                HttpContext.Current.Session["PaidDataSource"] = value;
            }
        }

        /// <summary>
        /// Gets or sets Paid Total Account
        /// </summary>
        public string PaidTotalAccount
        {
            get
            {
                object o = HttpContext.Current.Session["PaidTotalAccount"];
                return o != null ? (string)o : null;
            }

            set
            {
                HttpContext.Current.Session["PaidTotalAccount"] = value;
            }
        }

        /// <summary>
        /// Gets or sets repost URL
        /// </summary>
        public string ReportName
        {
            get
            {
                return _reportName;
            }

            set
            {
                _reportName = value;
            }
        }

        /// <summary>
        /// Gets or sets CAP ID
        /// </summary>
        private CapIDModel4WS CapID
        {
            get
            {
                return _capID;
            }

            set
            {
                _capID = value;
            }
        }

        #endregion Properties

        #region Methods

        /// <summary>
        /// bind no paid fee data source
        /// </summary>
        public void LoadNopaidFeeItem()
        {
            if (this.NoPaidDataSource != null)
            {
                return; //already in Seesion,no need load again
            }

            double totalBalance = 0;
            CapIDModel4WS capID = this.CapID;

            // Get all of fee items related to the current cap id.
            IFeeBll feeBll = ObjectFactory.GetObject<IFeeBll>();
            totalBalance = feeBll.GetTotalBalanceFee(capID, AppSession.User.PublicUserId);
            if (totalBalance <= 0)
            {
                return;
            }

            F4FeeModel4WS[] noPaidfees = feeBll.GetNoPaidFeeItemByCapID(capID, AppSession.User.PublicUserId);

            if (!AppSession.IsAdmin &&
                (noPaidfees == null || noPaidfees.Length == 0))
            {
                return;
            }

            DataTable dtNoPaid = CreateFeeTable();
            bool isExistingFirstItem = false;

            foreach (F4FeeModel4WS fee in noPaidfees)
            {
                if (fee.f4FeeItemModel != null)
                {
                    bool isEmptyInvoiceNbr = fee.x4FeeItemInvoiceModel.invoiceNbr.Equals(0) ? true : false;
                    bool isEmptyReceiptNbr = fee.receiptNbr.Equals(0) ? true : false;

                    DataRow drItem = dtNoPaid.NewRow();

                    drItem["Date"] = I18nDateTimeUtil.ParseFromWebService4DataTable(fee.f4FeeItemModel.applyDate);
                    drItem["InvoiceNbr"] = isEmptyInvoiceNbr ? string.Empty : fee.x4FeeItemInvoiceModel.invoiceNbr.ToString();
                    drItem["Amount"] = I18nNumberUtil.FormatMoneyForUI(fee.f4FeeItemModel.fee);
                    drItem["ReceiptNbr"] = isEmptyReceiptNbr ? string.Empty : fee.receiptNbr.ToString();

                    //if item is existing, display first item.
                    isExistingFirstItem = true;

                    dtNoPaid.Rows.Add(drItem);
                }
            }

            //initialize sort DESC in date line and invoice number.
            SortDataTable(ref dtNoPaid);

            //if first item is existing, display 'pay fee due' link.
            if (isExistingFirstItem && dtNoPaid != null && dtNoPaid.Rows[0] != null &&
                dtNoPaid.Rows[0]["Operation"] != null)
            {
                dtNoPaid.Rows[0]["Operation"] = LabelUtil.GetTextByKey("per_feeDetails_label_unpaidPayment", this.ModuleName);
            }

            this.NoPaidDataSource = dtNoPaid;
            this.NoPaidTotalAccount = I18nNumberUtil.FormatMoneyForUI(totalBalance);
        }

        /// <summary>
        /// bind paid fee data source
        /// </summary>
        public void LoadPaidFeeItem()
        {
            if (this.PaidDataSource != null)
            {
                return; //already in Seesion,no need load again
            }

            CapIDModel4WS capID = this.CapID;

            // Get all of fee items related to the current cap id.
            IFeeBll feeBll = ObjectFactory.GetObject<IFeeBll>();
            F4FeeModel4WS[] paidfees = feeBll.GetPaidFeeItemByCapID(capID, AppSession.User.PublicUserId);

            if (!AppSession.IsAdmin &&
                (paidfees == null || paidfees.Length == 0))
            {
                return;
            }

            DataTable dtPaid = CreateFeeTable();
            double paidTotal = 0;
            foreach (F4FeeModel4WS fee in paidfees)
            {
                if (fee.f4FeeItemModel != null)
                {
                    bool isEmptyInvoiceNbr = fee.x4FeeItemInvoiceModel.invoiceNbr.Equals(0) ? true : false;
                    bool isEmptyReceiptNbr = fee.receiptNbr.Equals(0) ? true : false;

                    DataRow drItem = dtPaid.NewRow();

                    drItem["Date"] = I18nDateTimeUtil.ParseFromWebService4DataTable(fee.f4FeeItemModel.applyDate);
                    drItem["Amount"] = I18nNumberUtil.FormatMoneyForUI(fee.f4FeeItemModel.fee);
                    drItem["InvoiceNbr"] = isEmptyInvoiceNbr ? null : fee.x4FeeItemInvoiceModel.invoiceNbr.ToString();
                    drItem["ReceiptNbr"] = isEmptyReceiptNbr ? null : fee.receiptNbr.ToString();

                    drItem["ToolTipInfo"] = this.ReportName;
                    drItem["Operation"] = isEmptyInvoiceNbr ? null : LabelUtil.GetTextByKey("per_feeDetails_label_paidViewDetails", this.ModuleName);
                    dtPaid.Rows.Add(drItem);
                    paidTotal += fee.f4FeeItemModel.fee;
                }
            }

            //initialize sort DESC in date line
            SortDataTable(ref dtPaid);
            this.PaidDataSource = dtPaid;
            this.PaidTotalAccount = I18nNumberUtil.FormatMoneyForUI(paidTotal);
        }

        /// <summary>
        /// Create a fee table, no paid and paid table
        /// </summary>
        /// <returns>Table about Fee Information</returns>
        private static DataTable CreateFeeTable()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add(new DataColumn("Date", typeof(DateTime)));
            dt.Columns.Add(new DataColumn("InvoiceNbr", typeof(string)));
            dt.Columns.Add(new DataColumn("Amount", typeof(string)));
            dt.Columns.Add(new DataColumn("Operation", typeof(string)));
            dt.Columns.Add(new DataColumn("ToolTipInfo", typeof(string)));
            dt.Columns.Add(new DataColumn("ReceiptNbr", typeof(string)));
            return dt;
        }

        /// <summary>
        /// Initialize sort for line by date
        /// </summary>
        /// <param name="dt">Data Table</param>
        private void SortDataTable(ref DataTable dt)
        {
            DataView dv = dt.DefaultView;
            dv.Sort = "Date DESC, InvoiceNbr DESC";
            dt = dv.ToTable();
        }

        #endregion Methods
    }
}