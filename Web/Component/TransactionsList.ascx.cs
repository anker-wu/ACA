#region Header
/**
 * <pre>
 * 
 *  Accela Citizen Access
 *  File: TransactionsList.ascx.cs
 * 
 *  Accela, Inc.
 *  Copyright (C): 2010-2014
 * 
 *  Description:
 * 
 *  Notes:
 *      $Id: TransactionsList.ascx.cs 178055 2010-07-30 07:37:23Z ACHIEVO\daly.zeng $.
 *  Revision History
 *  &lt;Date&gt;  &lt;Who&gt;  &lt;What&gt;
 * </pre>
 */
#endregion Header

using System;
using System.Collections.Generic;
using System.Data;
using System.Web.UI.WebControls;

using Accela.ACA.BLL.Common;
using Accela.ACA.Common;
using Accela.ACA.Common.Util;
using Accela.ACA.Web.Common;
using Accela.ACA.Web.Common.Control;
using Accela.ACA.WSProxy;
using Accela.Web.Controls;

namespace Accela.ACA.Web.Component
{
    /// <summary>
    /// UC for display transaction list.
    /// </summary>
    public partial class TransactionsList : BaseUserControl
    {
        #region Properties

        /// <summary>
        /// Gets or sets transaction data table.
        /// </summary>
        private DataTable DataSource
        {
            get
            {
                if (ViewState["DataSource"] == null)
                {
                    ViewState["DataSource"] = new DataTable();
                }

                return ViewState["DataSource"] as DataTable;
            }

            set
            {
                ViewState["DataSource"] = value;
            }
        }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Bind contact list by contact list data source.
        /// </summary>
        /// <param name="trustAccount">transaction array</param>
        public void BindList(TrustAccountModel trustAccount)
        {
            DataSource = ConvertTransactionDataTable(trustAccount);
            Bind();
        }

        /// <summary>
        /// Raises the page load event
        /// </summary>
        /// <param name="sender">An object that contains the event sender.</param>
        /// <param name="e">A System.EventArgs object containing the event data.</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            // When export CSV form request, it need Re-bind GV.
            if (IsPostBack && Request.Form["__EVENTTARGET"] != null && Request.Form["__EVENTTARGET"].IndexOf("btnExport") > -1)
            {
                Bind();
            }
        }

        /// <summary>
        /// GridView Transaction row command event method.
        /// </summary>
        /// <param name="sender">An object that contains the event sender.</param>
        /// <param name="e">A System.EventArgs object containing the event data.</param>
        protected void TransactionList_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            Bind();
        }

        /// <summary>
        /// <c>OnInit</c> event method.
        /// </summary>
        /// <param name="e">A System.EventArgs object containing the event data.</param>
        protected override void OnInit(EventArgs e)
        {
            if (!IsPostBack)
            {
                if (!AppSession.IsAdmin && StandardChoiceUtil.IsEnableExport2CSV())
                {
                    gdvTransactionList.ShowExportLink = true;
                    gdvTransactionList.ExportFileName = "TransactionList";
                }
                else
                {
                    gdvTransactionList.ShowExportLink = false;
                }
            }

            GridViewBuildHelper.SetSimpleViewElements(gdvTransactionList, ModuleName, AppSession.IsAdmin);
            base.OnInit(e);
        }

        /// <summary>
        /// Bind contact list by contact list data source.
        /// </summary>
        private void Bind()
        {
            DataView dv = new DataView(DataSource);

            if (!string.IsNullOrEmpty(gdvTransactionList.GridViewSortExpression))
            {
                dv.Sort = gdvTransactionList.GridViewSortExpression + " " + gdvTransactionList.GridViewSortDirection;
            }

            gdvTransactionList.DataSource = dv.ToTable();
            gdvTransactionList.EmptyDataText = AppSession.IsAdmin ? string.Empty : GetTextByKey("per_permitList_messagel_noRecord");
            gdvTransactionList.DataBind();
        }

        /// <summary>
        /// Build data table for transaction list.
        /// </summary>
        /// <param name="trustAccount">Trust account</param>
        /// <returns>DataTable for transaction array</returns>
        private DataTable ConvertTransactionDataTable(TrustAccountModel trustAccount)
        {
            IBizDomainBll bizBll = (IBizDomainBll)ObjectFactory.GetObject(typeof(IBizDomainBll));
            DataTable dtTransaction = ConstructTransactiontDataTable();

            if (trustAccount != null && trustAccount.trustAccountTransactionModels != null && trustAccount.trustAccountTransactionModels.Length > 0)
            {
                TrustAccountTransactionModel[] transactions = trustAccount.trustAccountTransactionModels;

                if (transactions != null && transactions.Length > 0)
                {
                    foreach (TrustAccountTransactionModel transaction in transactions)
                    {
                        if (transaction == null)
                        {
                            continue;
                        }

                        DataRow drTrustAcct = dtTransaction.NewRow();

                        drTrustAcct[ColumnConstant.Transaction.TransID.ToString()] = transaction.transSeq;
                        drTrustAcct[ColumnConstant.Transaction.AccountID.ToString()] = transaction.acctID;
                        string resTransactionType = bizBll.GetValueForStandardChoice(ConfigManager.AgencyCode, BizDomainConstant.STD_TRANSACTION_TYPE, transaction.transType);
                        drTrustAcct[ColumnConstant.Transaction.TransType.ToString()] = I18nStringUtil.GetString(resTransactionType, transaction.transType);
                        drTrustAcct[ColumnConstant.Transaction.TransAmount.ToString()] = transaction.transAmount;
                        drTrustAcct[ColumnConstant.Transaction.TargetAccountID.ToString()] = transaction.targetAcctID;
                        drTrustAcct[ColumnConstant.Transaction.RecordID.ToString()] = GetRecordID(transaction.capID);
                        drTrustAcct[ColumnConstant.Transaction.ALTID.ToString()] = transaction.altID;
                        drTrustAcct[ColumnConstant.Transaction.ClientTransNumber.ToString()] = FormatNumber(transaction.clientTransNbr);
                        drTrustAcct[ColumnConstant.Transaction.ClientReceiptNumber.ToString()] = FormatNumber(transaction.clientReceiptNbr);
                        drTrustAcct[ColumnConstant.Transaction.OfficeCode.ToString()] = transaction.officeCode;
                        drTrustAcct[ColumnConstant.Transaction.TransDate.ToString()] = transaction.recDate == null ? DBNull.Value : (object)transaction.recDate;
                        string resDepositMethod = bizBll.GetValueForStandardChoice(ConfigManager.AgencyCode, BizDomainConstant.STD_PAYMENT_PROCESSING_METHOD, transaction.depositMethod);
                        drTrustAcct[ColumnConstant.Transaction.DepositMethod.ToString()] = I18nStringUtil.GetString(resDepositMethod, transaction.depositMethod);
                        drTrustAcct[ColumnConstant.Transaction.TransactionCode.ToString()] = transaction.procTransID;
                        drTrustAcct[ColumnConstant.Transaction.CashDrawerID.ToString()] = transaction.terminalID;
                        drTrustAcct[ColumnConstant.Transaction.Comments.ToString()] = transaction.comment;
                        drTrustAcct[ColumnConstant.Transaction.CustomizedReceiptNumber.ToString()] = transaction.receiptCustomizedNbr;
                        drTrustAcct[ColumnConstant.Transaction.ReferenceNumber.ToString()] = transaction.paymentRefNbr;
                        drTrustAcct[ColumnConstant.Transaction.CCAuthCode.ToString()] = transaction.ccAuthCode;
                        drTrustAcct[ColumnConstant.Transaction.Payor.ToString()] = transaction.payor;
                        drTrustAcct[ColumnConstant.Transaction.Received.ToString()] = transaction.receivedType;

                        dtTransaction.Rows.Add(drTrustAcct);
                    }
                }
            }

            return dtTransaction;
        }

        /// <summary>
        /// Get record id by cap id model.
        /// </summary>
        /// <param name="capID">The cap id model.</param>
        /// <returns>The record id.</returns>
        private string GetRecordID(CapIDModel capID)
        {
            string recordID = string.Empty;

            if (capID != null && !string.IsNullOrEmpty(capID.ID1) && !string.IsNullOrEmpty(capID.ID2) && !string.IsNullOrEmpty(capID.ID3))
            {
                recordID = string.Format("{0}{1}{2}{3}{4}", capID.ID1, ACAConstant.SPLIT_CHAR4, capID.ID2, ACAConstant.SPLIT_CHAR4, capID.ID3); 
            }

            return recordID;
        }

        /// <summary>
        /// Format string number. if number as "0", show empty. else show self value.
        /// </summary>
        /// <param name="number">String for number.</param>
        /// <returns>Format string number.</returns>
        private string FormatNumber(long? number)
        {
            string formatNumber = string.Empty;

            if (number.HasValue)
            {
                formatNumber = number.ToString();

                // If value as "0", so that format value as empty.
                if (ACAConstant.COMMON_ZERO.Equals(formatNumber))
                {
                    return string.Empty;
                }
            }

            return formatNumber;
        }

        /// <summary>
        /// Construct a new DataTable for Transaction.
        /// </summary>
        /// <returns>
        /// Construct transaction dataTable
        /// </returns>
        private DataTable ConstructTransactiontDataTable()
        {
            DataTable trustAcctDataTable = new DataTable();

            trustAcctDataTable.Columns.Add(ColumnConstant.Transaction.TargetAccountID.ToString());
            trustAcctDataTable.Columns.Add(ColumnConstant.Transaction.RecordID.ToString());
            trustAcctDataTable.Columns.Add(ColumnConstant.Transaction.ClientTransNumber.ToString());
            trustAcctDataTable.Columns.Add(ColumnConstant.Transaction.OfficeCode.ToString());
            trustAcctDataTable.Columns.Add(ColumnConstant.Transaction.CashDrawerID.ToString());
            trustAcctDataTable.Columns.Add(ColumnConstant.Transaction.Comments.ToString());
            trustAcctDataTable.Columns.Add(ColumnConstant.Transaction.ReferenceNumber.ToString());
            trustAcctDataTable.Columns.Add(ColumnConstant.Transaction.Payor.ToString());
            trustAcctDataTable.Columns.Add(ColumnConstant.Transaction.Received.ToString());
            trustAcctDataTable.Columns.Add(ColumnConstant.Transaction.TransDate.ToString(), typeof(DateTime));
            trustAcctDataTable.Columns.Add(ColumnConstant.Transaction.TransType.ToString());
            trustAcctDataTable.Columns.Add(ColumnConstant.Transaction.TransAmount.ToString(), typeof(double));
            trustAcctDataTable.Columns.Add(ColumnConstant.Transaction.ALTID.ToString());
            trustAcctDataTable.Columns.Add(ColumnConstant.Transaction.ClientReceiptNumber.ToString());
            trustAcctDataTable.Columns.Add(ColumnConstant.Transaction.AccountID.ToString());
            trustAcctDataTable.Columns.Add(ColumnConstant.Transaction.TransID.ToString());
            trustAcctDataTable.Columns.Add(ColumnConstant.Transaction.CustomizedReceiptNumber.ToString());
            trustAcctDataTable.Columns.Add(ColumnConstant.Transaction.CCAuthCode.ToString());
            trustAcctDataTable.Columns.Add(ColumnConstant.Transaction.TransactionCode.ToString());
            trustAcctDataTable.Columns.Add(ColumnConstant.Transaction.DepositMethod.ToString());

            return trustAcctDataTable;
        }

        #endregion Methods
    }
}
