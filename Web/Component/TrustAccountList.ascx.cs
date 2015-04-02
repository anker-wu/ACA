#region Header
/**
 * <pre>
 * 
 *  Accela Citizen Access
 *  File: TrustAccountList.ascx.cs
 * 
 *  Accela, Inc.
 *  Copyright (C): 2010-2014
 * 
 *  Description:
 * 
 *  Notes:
 *      $Id: TrustAccountList.ascx.cs 178055 2010-07-30 07:37:23Z ACHIEVO\daly.zeng $.
 *  Revision History
 *  &lt;Date&gt;  &lt;Who&gt;  &lt;What&gt;
 * </pre>
 */
#endregion Header

using System;
using System.Collections.Generic;
using System.Data;
using System.Web;
using System.Web.UI.WebControls;
using Accela.ACA.Common;
using Accela.ACA.Web.Common;
using Accela.ACA.Web.Common.Control;
using Accela.ACA.WSProxy;
using Accela.Web.Controls;

namespace Accela.ACA.Web.Component
{
    /// <summary>
    /// Indicate which local page is account management or cap detail.
    /// </summary>
    public enum TrustAccoutListLocation
    {
        /// <summary>
        /// In Account management.
        /// </summary>
        AcctManager,

        /// <summary>
        /// In cap detail. 
        /// </summary>
        CapDetail
    }

    /// <summary>
    /// UC for display trust account list.
    /// </summary>
    public partial class TrustAccountList : BaseUserControl
    {
        #region Fields

        /// <summary>
        /// Select trust account detail.
        /// </summary>
        protected const string SELECT_ACCOUNT_ID = "SelectTrustAccountID";

        /// <summary>
        /// Select trust account deposit.
        /// </summary>
        protected const string SELECT_DEPOSIT = "SelectTrustAccountDeposit";

        /// <summary>
        /// Indicate which local page is account management or cap detail.
        /// </summary>
        private TrustAccoutListLocation _location = TrustAccoutListLocation.AcctManager;

        #endregion Fields

        #region Properties

        /// <summary>
        /// Gets or sets the view id for contact list
        /// </summary>
        public string GViewID
        {
            get
            {
                return gdvTrustAcctList.GridViewNumber;
            }

            set
            {
                gdvTrustAcctList.GridViewNumber = value;
            }
        }

        /// <summary>
        /// Gets or sets local page type
        /// </summary>
        /// <value>The location.</value>
        public TrustAccoutListLocation Location
        {
            get
            {
                return _location;
            }

            set
            {
                _location = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether edit the form or not.
        /// </summary>
        public bool IsEditable
        {
            get
            {
                if (ViewState["IsEditable"] == null)
                {
                    return true;
                }

                return Convert.ToBoolean(ViewState["IsEditable"]);
            }

            set
            {
                ViewState["IsEditable"] = value;
            }
        }

        /// <summary>
        /// Gets or sets trust account data table.
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
        /// <param name="trustAccounts">trust account array.</param>
        public void BindList(IList<TrustAccountModel> trustAccounts)
        {
            DataSource = ConvertTrustAcctToDataTable(trustAccounts);
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
        /// GridView ContactList row command event method.
        /// </summary>
        /// <param name="sender">An object that contains the event sender.</param>
        /// <param name="e">A System.EventArgs object containing the event data.</param>
        protected void TrustAccountList_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            string acctID = e.CommandArgument.ToString();

            if (e.CommandName == SELECT_ACCOUNT_ID)
            {
                Response.Redirect(string.Format("../Account/TrustAccountDetail.aspx?accountID={0}", HttpUtility.UrlEncode(acctID)));
            }
            else if (e.CommandName == SELECT_DEPOSIT)
            {
                Response.Redirect(string.Format("../Account/TrustAccountDeposit.aspx?accountID={0}", HttpUtility.UrlEncode(acctID)));
            }
            else if (e.CommandName != "Export")
            {
                Bind();
            }
        }

        /// <summary>
        /// GridView ContactList row data bound event.
        /// </summary>
        /// <param name="sender">Object sender</param>
        /// <param name="e">GridViewCommandEventArgs e</param>
        protected void TrustAccountList_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                AccelaLabel lblStatus = (AccelaLabel)e.Row.FindControl("lblStatus");
                AccelaLinkButton btnDeposit = (AccelaLinkButton)e.Row.FindControl("btnDeposit");
                AccelaLinkButton btnAccID = (AccelaLinkButton)e.Row.FindControl("btnAcctID");
                AccelaLabel lblAcctID = e.Row.FindControl("lblAcctID") as AccelaLabel;
                
                if (lblStatus != null)
                {
                    lblStatus.Text = TrustAccountUtil.ConvertStatusField2Display(lblStatus.Text);
                }                

                if (btnDeposit != null)
                {
                    btnDeposit.Text = GetTextByKey("per_trustaccountlist_deposit");
                }

                string trustAccountAgency = Convert.ToString((e.Row.DataItem as DataRowView)[ColumnConstant.TrustAccount.ServProvCode.ToString()]);

                if (!IsEditable || !string.Equals(ConfigManager.SuperAgencyCode, trustAccountAgency, StringComparison.OrdinalIgnoreCase))
                {
                    // Currently trust account function does not support coross agency, so disable the view & deposit functionlity.
                    btnAccID.Visible = false;
                    lblAcctID.Visible = true;
                    btnDeposit.Visible = false;
                }
            }
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
                    gdvTrustAcctList.ShowExportLink = true;
                    gdvTrustAcctList.ExportFileName = "TrustAccountList";
                }
                else
                {
                    gdvTrustAcctList.ShowExportLink = false;
                }
            }
          
            // The 'ShowHeader' attribute to be used to indicating whether the column is controls by Gview.
            gdvTrustAcctList.Columns[1].ShowHeader = Location == TrustAccoutListLocation.CapDetail;
            gdvTrustAcctList.Columns[1].Visible = Location == TrustAccoutListLocation.CapDetail;
            GridViewBuildHelper.SetSimpleViewElements(gdvTrustAcctList, this.ModuleName, AppSession.IsAdmin);
            base.OnInit(e);
        }

        /// <summary>
        /// Construct a new DataTable for Trust Account.
        /// </summary>
        /// <returns>
        /// Construct Trust Account dataTable
        /// </returns>
        private static DataTable ConstructTrustAccountDataTable()
        {
            DataTable trustAcctDataTable = new DataTable();
            trustAcctDataTable.Columns.Add(ColumnConstant.TrustAccount.AccountID.ToString());
            trustAcctDataTable.Columns.Add(ColumnConstant.TrustAccount.Primary.ToString());
            trustAcctDataTable.Columns.Add(ColumnConstant.TrustAccount.Balance.ToString(), typeof(double));
            trustAcctDataTable.Columns.Add(ColumnConstant.TrustAccount.Description.ToString());
            trustAcctDataTable.Columns.Add(ColumnConstant.TrustAccount.Status.ToString());
            trustAcctDataTable.Columns.Add(ColumnConstant.TrustAccount.LedgerAccount.ToString());
            trustAcctDataTable.Columns.Add(ColumnConstant.TrustAccount.ServProvCode.ToString());
            return trustAcctDataTable;
        }

        /// <summary>
        /// Bind contact list by contact list data source.
        /// </summary>
        private void Bind()
        {
            DataView dv = new DataView(DataSource);

            if (!string.IsNullOrEmpty(gdvTrustAcctList.GridViewSortExpression))
            {
                dv.Sort = gdvTrustAcctList.GridViewSortExpression + " " + gdvTrustAcctList.GridViewSortDirection;
            }

            gdvTrustAcctList.DataSource = dv.ToTable();
            gdvTrustAcctList.EmptyDataText = AppSession.IsAdmin ? string.Empty : GetTextByKey("per_permitList_messagel_noRecord");
            gdvTrustAcctList.DataBind();
        }

        /// <summary>
        /// Build data table for trust account list.
        /// </summary>
        /// <param name="trustAccts">Trust account array</param>
        /// <returns>DataTable for trust account array</returns>
        private DataTable ConvertTrustAcctToDataTable(IList<TrustAccountModel> trustAccts)
        {
            DataTable dtTrustAcct = ConstructTrustAccountDataTable();

            if (trustAccts != null && trustAccts.Count > 0)
            {
                foreach (TrustAccountModel trustAcct in trustAccts)
                {
                    if (trustAcct == null)
                    {
                        continue;
                    }

                    DataRow drTrustAcct = dtTrustAcct.NewRow();
                    drTrustAcct[ColumnConstant.TrustAccount.AccountID.ToString()] = trustAcct.acctID;
                    drTrustAcct[ColumnConstant.TrustAccount.Primary.ToString()] = trustAcct.primary;
                    drTrustAcct[ColumnConstant.TrustAccount.Balance.ToString()] = trustAcct.acctBalance.Value;
                    drTrustAcct[ColumnConstant.TrustAccount.Description.ToString()] = trustAcct.description;
                    drTrustAcct[ColumnConstant.TrustAccount.Status.ToString()] = trustAcct.acctStatus;
                    drTrustAcct[ColumnConstant.TrustAccount.LedgerAccount.ToString()] = trustAcct.ledgerAccount;
                    drTrustAcct[ColumnConstant.TrustAccount.ServProvCode.ToString()] = trustAcct.servProvCode;
                    dtTrustAcct.Rows.Add(drTrustAcct);
                }
            }

            return dtTrustAcct;
        }

        #endregion Methods
    }
}
