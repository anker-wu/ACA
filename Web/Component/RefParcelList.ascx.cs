#region Header
/**
 * <pre>
 * 
 *  Accela Citizen Access
 *  File: RefParcelList.aspx.cs
 * 
 *  Accela, Inc.
 *  Copyright (C): 2010-2014
 * 
 *  Description:
 * 
 *  Notes:
 *      $Id: RefParcelList.ascx.cs 178055 2010-07-30 07:37:23Z ACHIEVO\daly.zeng $.
 *  Revision History
 *  &lt;Date&gt;  &lt;Who&gt;  &lt;What&gt;
 * </pre>
 */
#endregion Header

using System;
using System.Data;
using System.Web.UI.WebControls;

using Accela.ACA.BLL.AddressBuilder;
using Accela.ACA.Common;
using Accela.ACA.Common.Util;
using Accela.ACA.Web.Common;
using Accela.ACA.Web.Common.Control;
using Accela.ACA.WSProxy;

namespace Accela.ACA.Web.Component
{
    /// <summary>
    /// UC for display ref parcel list.
    /// </summary>
    public partial class RefParcelList : BaseUserControl
    {
        #region Properties

        /// <summary>
        /// Gets or sets data resource table.
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
        /// Bind reference parcel list by parcel list data source.
        /// </summary>
        /// <param name="parcelInfos">reference parcel info model array.</param>
        public void BindList(ParcelInfoModel[] parcelInfos)
        {
            DataSource = ConvertTrustAcctToDataTable(parcelInfos);
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
        /// GridView RefParcel row command event method.
        /// </summary>
        /// <param name="sender">An object that contains the event sender.</param>
        /// <param name="e">A System.EventArgs object containing the event data.</param>
        protected void RefParcelList_RowCommand(object sender, GridViewCommandEventArgs e)
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
                    gdvRefParcelList.ShowExportLink = true;
                    gdvRefParcelList.ExportFileName = "AssociatedParcelList";
                }
                else
                {
                    gdvRefParcelList.ShowExportLink = false;
                }
            }

            //if standard choice display owner section is "N" and is in daily side, set owner column is hidden.
            if (!AppSession.IsAdmin && !StandardChoiceUtil.IsDisplayOwnerSection())
            {
                GridViewBuildHelper.SetHiddenColumn(gdvRefParcelList, new[] { "Owner" });
            }

            GridViewBuildHelper.SetSimpleViewElements(gdvRefParcelList, this.ModuleName, AppSession.IsAdmin);

            base.OnInit(e);
        }

        /// <summary>
        /// Construct a new DataTable for reference parcel.
        /// </summary>
        /// <returns>
        /// Construct reference parcel list dataTable
        /// </returns>
        private static DataTable ConstructRefParcelDataTable()
        {
            DataTable dataTable = new DataTable();

            dataTable.Columns.Add(ColumnConstant.RefParcelList.ParcelNumber.ToString());
            dataTable.Columns.Add(ColumnConstant.RefParcelList.Block.ToString());
            dataTable.Columns.Add(ColumnConstant.RefParcelList.Lot.ToString());
            dataTable.Columns.Add(ColumnConstant.RefParcelList.Subdivision.ToString());
            dataTable.Columns.Add(ColumnConstant.RefParcelList.OwnerFullName.ToString());
            dataTable.Columns.Add(ColumnConstant.RefParcelList.FullAddress.ToString());

            return dataTable;
        }

        /// <summary>
        /// Bind reference parcel list.
        /// </summary>
        private void Bind()
        {
            DataView dv = new DataView(DataSource);

            if (!string.IsNullOrEmpty(gdvRefParcelList.GridViewSortExpression))
            {
                dv.Sort = gdvRefParcelList.GridViewSortExpression + " " + gdvRefParcelList.GridViewSortDirection;
            }

            gdvRefParcelList.DataSource = dv.ToTable();
            gdvRefParcelList.EmptyDataText = AppSession.IsAdmin ? string.Empty : GetTextByKey("per_permitList_messagel_noRecord");
            gdvRefParcelList.DataBind();
        }

        /// <summary>
        /// build data table for parcel list.
        /// </summary>
        /// <param name="parcelInfos">parcel array.</param>
        /// <returns>dataTable for parcel array</returns>
        private DataTable ConvertTrustAcctToDataTable(ParcelInfoModel[] parcelInfos)
        {
            DataTable dtRefParcel = ConstructRefParcelDataTable();

            if (parcelInfos != null && parcelInfos.Length > 0)
            {
                foreach (ParcelInfoModel parcelInfo in parcelInfos)
                {
                    if (parcelInfo == null || parcelInfo.parcelModel == null)
                    {
                        continue;
                    }

                    DataRow drRefParcel = dtRefParcel.NewRow();
                    drRefParcel[ColumnConstant.RefParcelList.ParcelNumber.ToString()] = parcelInfo.parcelModel.parcelNumber;
                    drRefParcel[ColumnConstant.RefParcelList.Lot.ToString()] = parcelInfo.parcelModel.lot;
                    drRefParcel[ColumnConstant.RefParcelList.Block.ToString()] = parcelInfo.parcelModel.block;
                    drRefParcel[ColumnConstant.RefParcelList.Subdivision.ToString()] = I18nStringUtil.GetString(parcelInfo.parcelModel.resSubdivision, parcelInfo.parcelModel.subdivision);

                    if (parcelInfo.RAddressModel != null)
                    {
                        IAddressBuilderBll addressBuilderBll = ObjectFactory.GetObject<IAddressBuilderBll>();
                        drRefParcel[ColumnConstant.RefParcelList.FullAddress.ToString()] = addressBuilderBll.BuildAddressByFormatType(parcelInfo.RAddressModel, null, AddressFormatType.SHORT_ADDRESS_NO_FORMAT);
                    }

                    if (parcelInfo.ownerModel != null)
                    {
                        drRefParcel[ColumnConstant.RefParcelList.OwnerFullName.ToString()] = parcelInfo.ownerModel.ownerFullName;
                    }

                    dtRefParcel.Rows.Add(drRefParcel);
                }
            }

            return dtRefParcel;
        }

        #endregion Methods
    }
}