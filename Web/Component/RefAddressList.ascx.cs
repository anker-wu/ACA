#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: CapDetail.aspx.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2008-2014
 *
 *  Description:
 *
 *  Notes:
 *      $Id: RefAddressList.ascx.cs 277971 2014-08-25 06:10:06Z ACHIEVO\james.shi $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using System;
using System.Data;
using System.Web.UI.WebControls;

using Accela.ACA.BLL.APO;
using Accela.ACA.Common;
using Accela.ACA.Web.Common;
using Accela.ACA.Web.Common.Control;
using Accela.ACA.WSProxy;

namespace Accela.ACA.Web.Component
{
    /// <summary>
    /// This class provide the ability to operation RefAddressList.
    /// </summary>
    public partial class RefAddressList : BaseUserControl
    {
        #region Properties

        /// <summary>
        /// Gets or sets  address data table.
        /// </summary>
        private DataTable DataSource
        {
            get
            {
                if (ViewState["DataSource"] == null)
                {
                    ViewState["DataSource"] = new DataTable();
                }

                return (DataTable)ViewState["DataSource"];
            }

            set
            {
                ViewState["DataSource"] = value;
            }
        }

        #endregion Properties

        #region Methods

        /// <summary>
        /// bind contact list by address list data source.
        /// </summary>
        /// <param name="refAddresses">refAddress array.</param>
        public void BindList(RefAddressModel[] refAddresses)
        {
            DataSource = ConvertAddressToDataTable(refAddresses);
            Bind();
        }

        /// <summary>
        /// Raises the page load event
        /// </summary>
        /// <param name="sender">An object that contains the event sender.</param>
        /// <param name="e">A System.EventArgs object containing the event data.</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            //when export CSV form request, it need Re-bind GV.
            if (IsPostBack && Request.Form["__EVENTTARGET"] != null && Request.Form["__EVENTTARGET"].IndexOf("btnExport") > -1)
            {
                Bind();
            }
        }

        /// <summary>
        /// GridView Address row command event method.
        /// </summary>
        /// <param name="sender">An object that contains the event sender.</param>
        /// <param name="e">A System.EventArgs object containing the event data.</param>
        protected void RefAddress_RowCommand(object sender, GridViewCommandEventArgs e)
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
                    dgvRefAddress.ShowExportLink = true;
                    dgvRefAddress.ExportFileName = "AssociatedAddresses";
                }
                else
                {
                    dgvRefAddress.ShowExportLink = false;
                }
            }

            GridViewBuildHelper.SetSimpleViewElements(dgvRefAddress, ModuleName, AppSession.IsAdmin);
            base.OnInit(e);
        }

        /// <summary>
        /// bind address list data source.
        /// </summary>
        private void Bind()
        {
            DataView dv = new DataView(DataSource);

            if (!string.IsNullOrEmpty(dgvRefAddress.GridViewSortExpression))
            {
                dv.Sort = dgvRefAddress.GridViewSortExpression + " " + dgvRefAddress.GridViewSortDirection;
            }

            dgvRefAddress.DataSource = dv.ToTable();
            dgvRefAddress.EmptyDataText = AppSession.IsAdmin ? string.Empty : GetTextByKey("per_permitList_messagel_noRecord");
            dgvRefAddress.DataBind();
        }

        /// <summary>
        /// build data table for address list.
        /// </summary>
        /// <param name="refAddresses">address array.</param>
        /// <returns>dataTable for address array</returns>
        private DataTable ConvertAddressToDataTable(RefAddressModel[] refAddresses)
        {
            DataTable dtAddress = ConstructAddressDataTable();

            if (refAddresses != null && refAddresses.Length > 0)
            {
                foreach (RefAddressModel refAddress in refAddresses)
                {
                    if (refAddress == null)
                    {
                        continue;
                    }

                    DataRow drAddress = dtAddress.NewRow();
                    IAddressBll addressBll = (IAddressBll)ObjectFactory.GetObject(typeof(IAddressBll));
                    drAddress[ColumnConstant.RefAddressList.Address.ToString()] = addressBll.GenerateAddressString(TrustAccountUtil.ConvertAddressModelbyReference(refAddress));

                    dtAddress.Rows.Add(drAddress);
                }
            }

            return dtAddress;
        }

        /// <summary>
        /// Construct a new DataTable for Address.
        /// </summary>
        /// <returns>A DataTable that contains 1 columns, full address</returns>
        private DataTable ConstructAddressDataTable()
        {
            DataTable addressTable = new DataTable();
            addressTable.Columns.Add(ColumnConstant.RefAddressList.Address.ToString());

            return addressTable;
        }

        #endregion Methods
    }
}