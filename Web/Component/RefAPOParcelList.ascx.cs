#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: RefAPOParcelList.ascx.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2007-2014
 *
 *  Description:
 *
 *  Notes:
 *      $Id: RefAPOParcelList.ascx.cs 277971 2014-08-25 06:10:06Z ACHIEVO\james.shi $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using System;
using System.Collections.Generic;
using System.Data;
using System.Web.UI.WebControls;
using Accela.ACA.Common;
using Accela.ACA.Common.Util;
using Accela.ACA.Web.Common;
using Accela.ACA.Web.Common.Control;
using Accela.ACA.WSProxy;
using Accela.Web.Controls;

namespace Accela.ACA.Web.Component
{
    /// <summary>
    /// This class provide the ability to operation RefAPOParcelList.
    /// </summary>
    public partial class RefAPOParcelList : BaseUserControl
    {
        /// <summary>
        /// The padding status parcel.
        /// </summary>
        private const string PENDING_PARCEL = "P";

        #region Events

        /// <summary>
        /// grid view sorted event.
        /// </summary>
        public event GridViewSortedEventHandler GridViewSort;

        /// <summary>
        /// grid view page index changing event
        /// </summary>
        public event GridViewPageEventHandler PageIndexChanging;

        /// <summary>
        /// Occurs when a radio button is clicked in a System.Web.UI.WebControls.GridView control.
        /// </summary>
        public event EventHandler Select;

        #endregion Events

        #region Properties

        /// <summary>
        /// Gets or sets a value indicating whether show map.
        /// </summary>
        public bool IsShowMap
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets parcel's data source.
        /// </summary>
        public DataTable ParcelDataSource
        {
            get 
            {
                return ViewState["DataSrc"] as DataTable;
            }

            set
            {
                ViewState["DataSrc"] = value;
            }
        }

        /// <summary>
        /// Gets grid page size.
        /// </summary>
        public int PageSize
        {
            get
            {
                return gv.PageSize;
            }
        }

        /// <summary>
        /// Gets or sets a string to identify the grid in whole ACA system
        /// </summary>
        public string GridViewNumber
        {
            get
            {
                return gv.GridViewNumber;
            }

            set
            {
                gv.GridViewNumber = value;
            }
        }

        /// <summary>
        /// Gets the selected row index.
        /// </summary>
        public int SelectedRowIndex
        {
            get
            {
                List<int> indexes = gv.GetSelectedRowIndexes();
                return indexes.Count == 0 ? -1 : indexes[0];
            }
        }

        /// <summary>
        /// Gets the selected row.
        /// </summary>
        public DataRow SelectedRow
        {
            get
            {
                int dataIndex = SelectedRowIndex;

                return dataIndex == -1 ? null : ParcelDataSource.Rows[SelectedRowIndex];
            }
        }
        
        /// <summary>
        /// Gets the selected parcel model.
        /// </summary>
        public ParcelModel SelectedParcel
        {
            get
            {
                return GetParcel(SelectedRow);
            }
        }

        /// <summary>
        /// Gets the selected ParcelModel only include PK information.
        /// </summary>
        public ParcelModel SelectedParcelPK
        {
            get
            {
                return GetParcelKey(SelectedRow);
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether allow to select by Radio Button.
        /// </summary>
        public bool AllowSelectingByRadioButton
        {
            get
            {
                return gv.AutoGenerateRadioButtonColumn;
            }

            set
            {
                gv.AutoGenerateRadioButtonColumn = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether auto post back when select a parcel.
        /// </summary>
        public bool AutoPostBackOnSelect
        {
            get;
            set;
        }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Select the specified row
        /// </summary>
        /// <param name="rowIndex">Row index</param>
        /// <returns>true or false, indicating whether success to select the specified row.</returns>
        public bool SelectRow(int rowIndex)
        {
            gv.SelectRow(gv.Rows[rowIndex]);

            return true;
        }

        /// <summary>
        /// Get duplicated APO keys for parcel.
        /// </summary>
        /// <param name="parcelModel">A ParcelModel will be updated</param>
        public void GetDuplicatedAPOKeys(ParcelModel parcelModel)
        {
            if (parcelModel == null)
            {
                return;
            }

            DataRow dr = SelectedRow;

            if (dr != null && dr["ParcelAPOKeys"] != DBNull.Value && !string.IsNullOrEmpty(dr["ParcelAPOKeys"].ToString()))
            {
                parcelModel.duplicatedAPOKeys = dr["ParcelAPOKeys"] as DuplicatedAPOKeyModel[];
            }
        }

        /// <summary>
        /// ShowOnMap event handler for mapParcel
        /// </summary>
        /// <param name="sender">The object sender.</param>
        /// <param name="e">An System.EventArgs object that contains the event data.</param>
        public void MapParcel_ShowACAMap(object sender, EventArgs e)
        {
            ACAGISModel model = GISUtil.CreateACAGISModel();
            model.ModuleName = ModuleName;
            model.UserGroups.Add(AppSession.User.IsAnonymous ? GISUserGroup.Anonymous.ToString() : GISUserGroup.Register.ToString());
            GISUtil.SetPostUrl(this.Page, model);
            DataTable dt = null;
            if (gv.DataSource is DataTable)
            {
                dt = gv.GetSelectedData(gv.DataSource as DataTable, gv.PageIndex);
            }
            else
            {
                DataView dv = gv.DataSource as DataView;
                dt = dv.Table;
                dt = gv.GetSelectedData(dt, gv.PageIndex);
            }

            if (dt != null && dt.Rows.Count > 0)
            {
                List<ParcelModel> list = new List<ParcelModel>();
                foreach (DataRow item in dt.Rows)
                {
                    string parcelNumber = item["ParcelNumber"] as string;
                    string unmaskedParcelNo = item["UnmaskedParcelNumber"] as string;
                    if (!string.IsNullOrEmpty(parcelNumber))
                    {
                        ParcelModel parcel = new ParcelModel();
                        parcel.parcelNumber = parcelNumber;
                        parcel.unmaskedParcelNumber = unmaskedParcelNo;
                        list.Add(parcel);
                    }
                }

                model.ParcelModels = list.ToArray();
                mapParcel.ACAGISModel = model;
            }
            else
            {
                mapParcel.ACAGISModel = null;
            }
        }

        /// <summary>
        /// Bind Data Source
        /// </summary>
        /// <param name="dt">data table source.</param>
        public void BindDataSource(DataTable dt)
        {
            ParcelDataSource = dt;

            if (dt == null || dt.Rows.Count == 0)
            {
                gv.EmptyDataText = AppSession.IsAdmin ? string.Empty : GetTextByKey("per_permitList_messagel_noRecord");
            }
            else if (!string.IsNullOrEmpty(gv.GridViewSortExpression))
            {
                DataView dv = new DataView(dt);
                dv.Sort = gv.GridViewSortExpression + " " + gv.GridViewSortDirection;
                ParcelDataSource = dv.ToTable();
            }

            gv.Visible = true;
            gv.DataSource = ParcelDataSource;
            gv.DataBind();
        }

        /// <summary>
        /// Clear selected items.
        /// </summary>
        public void ClearSelectedItems()
        {
            gv.IsClearSelectedItems = true;
        }

        /// <summary>
        /// Get Display Parcel Status.
        /// </summary>
        /// <param name="parcelStatus">Parcel status</param>
        /// <returns>Parcel Status</returns>
        protected string GetDisplayParcelStatus(string parcelStatus)
        {
            string displayParcleStatus = string.Empty;

            if (ACAConstant.VALID_STATUS.Equals(parcelStatus, StringComparison.InvariantCultureIgnoreCase))
            {
                displayParcleStatus = GetTextByKey("aca_parcel_label_statusenable");
            }
            else if (ACAConstant.INVALID_STATUS.Equals(parcelStatus, StringComparison.InvariantCultureIgnoreCase))
            {
                displayParcleStatus = GetTextByKey("aca_parcel_label_statusdisabled");
            }
            else if (PENDING_PARCEL.Equals(parcelStatus, StringComparison.InvariantCultureIgnoreCase))
            {
                displayParcleStatus = GetTextByKey("aca_parcel_label_statuspending");
            }

            return displayParcleStatus;
        }

        /// <summary>
        /// Rewrite  <c>OnInit</c> method to initialize component.
        /// </summary>
        /// <param name="e">A System.EventArgs Object containing the event data.</param>
        protected override void OnInit(EventArgs e)
        {
            GridViewBuildHelper.SetSimpleViewElements(gv, ModuleName, AppSession.IsAdmin);
            if (IsShowMap)
            {
                gv.AutoGenerateCheckBoxColumn = true;
                gv.CheckBoxColumnIndex = 0;
            }
            else
            {
                gv.AutoGenerateCheckBoxColumn = false;
            }

            mapParcel.Visible = IsShowMap;

            base.OnInit(e);
        }

        /// <summary>
        /// Raises the page load event
        /// </summary>
        /// <param name="sender">An object that contains the event sender.</param>
        /// <param name="e">A System.EventArgs object containing the event data.</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack)
            {
                BindDataSource(ParcelDataSource);
            }
        }

        /// <summary>
        /// Select a parcel.
        /// </summary>
        /// <param name="sender">An object that contains the event sender.</param>
        /// <param name="e">A System.EventArgs object containing the event data.</param>
        protected void SelectParcelButton_OnClick(object sender, EventArgs e)
        {
            if (Select != null)
            {
                Select(this, e);
            }
        }

        /// <summary>
        /// Grid row data bound.
        /// </summary>
        /// <param name="sender">An object that contains the event sender.</param>
        /// <param name="e">A System.EventArgs object containing the event data.</param>
        protected void RefParcelList_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (AllowSelectingByRadioButton)
                {
                    LinkButton lnkNumber = (LinkButton)e.Row.FindControl("lnkNumber");
                    lnkNumber.Visible = false;

                    AccelaLabel lblNumber = (AccelaLabel)e.Row.FindControl("lblNumber");
                    lblNumber.Visible = true;
                }
            }
        }

        /// <summary>
        /// GV RowCommand
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">GridViewCommandEventArgs e</param>
        protected void RefParcelList_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "ShowDetail")
            {
                int index = int.Parse(e.CommandArgument.ToString());
                GridViewRow row = gv.Rows[index];
                string parcelNum = ((Label)row.FindControl("lblNumber")).Text;
                string parcelSeq = ((Label)row.FindControl("lblSequenceNumber")).Text;
                string parcelUID = ((Label)row.FindControl("lblParcelUID")).Text;

                string url = string.Format(
                        "../APO/ParcelDetail.aspx?{0}={1}&{2}={3}&{4}={5}&{6}={7}&{8}={9}&HideHeader={10}&{11}={12}",
                        ACAConstant.REQUEST_PARMETER_PARCEL_NUMBER,
                        UrlEncode(parcelNum),
                        ACAConstant.REQUEST_PARMETER_PARCEL_SEQUENCE,
                        UrlEncode(parcelSeq),
                        ACAConstant.REQUEST_PARMETER_PARCEL_UID,
                        UrlEncode(parcelUID),
                        ACAConstant.REQUEST_PARMETER_REFADDRESS_ID,
                        Request.Params[ACAConstant.REQUEST_PARMETER_REFADDRESS_ID],
                        ACAConstant.REQUEST_PARMETER_ADDRESS_SEQUENCE,
                        Request.Params[ACAConstant.REQUEST_PARMETER_ADDRESS_SEQUENCE],
                        Page.Request.QueryString["HideHeader"],
                        UrlConstant.AgencyCode,
                        Page.Request.QueryString[UrlConstant.AgencyCode]);

                Response.Redirect(url);
            }

            if (e.CommandName == "Header")
            {
                gv.GridViewSortExpression = e.CommandArgument.ToString();
            }
        }

        /// <summary>
        /// Fire page index changing event
        /// </summary>
        /// <param name="sender">Object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void RefParcelList_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            if (PageIndexChanging != null)
            {
                PageIndexChanging(sender, e);
            }
        }

        /// <summary>
        /// Fire sorted event.
        /// </summary>
        /// <param name="sender">Object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void RefParcelList_GridViewSort(object sender, AccelaGridViewSortEventArgs e)
        {
            if (GridViewSort != null)
            {
                GridViewSort(sender, e);
            }
        }

        /// <summary>
        /// Gets the parcel model from data row.
        /// </summary>
        /// <param name="dataRow">Data row.</param>
        /// <returns>A ParcelModel</returns>
        private ParcelModel GetParcel(DataRow dataRow)
        {
            if (dataRow == null)
            {
                return null;
            }

            ParcelModel parcel = GetParcelKey(dataRow);

            parcel.unmaskedParcelNumber = dataRow["UnmaskedParcelNumber"].ToString();
            parcel.lot = dataRow["Lot"].ToString();
            parcel.block = dataRow["Block"].ToString();
            parcel.subdivision = dataRow["Subdivision"].ToString();
            parcel.book = dataRow["Book"].ToString();
            parcel.page = dataRow["Page"].ToString();
            parcel.tract = dataRow["Tract"].ToString();
            parcel.legalDesc = dataRow["legalDescription"].ToString();
            parcel.parcelArea = CommonUtil.GetDoubleValue(dataRow["ParcelArea"].ToString());
            parcel.landValue = CommonUtil.GetDoubleValue(dataRow["LandValue"].ToString());
            parcel.improvedValue = CommonUtil.GetDoubleValue(dataRow["ImprovedValue"].ToString());
            parcel.exemptValue = CommonUtil.GetDoubleValue(dataRow["ExceptionValue"].ToString());

            if (dataRow["ParcelAttributes"] != DBNull.Value && !string.IsNullOrEmpty(dataRow["ParcelAttributes"].ToString()))
            {
                parcel.templates = (TemplateAttributeModel[])dataRow["ParcelAttributes"];
            }

            if (dataRow["ParcelAPOKeys"] != DBNull.Value && !string.IsNullOrEmpty(dataRow["ParcelAPOKeys"].ToString()))
            {
                parcel.duplicatedAPOKeys = dataRow["ParcelAPOKeys"] as DuplicatedAPOKeyModel[];
            }

            return parcel;
        }

        /// <summary>
        /// Gets the parcel's keys from data row.
        /// </summary>
        /// <param name="dataRow">A data row</param>
        /// <returns>A <see cref="ParcelModel"/></returns>
        private ParcelModel GetParcelKey(DataRow dataRow)
        {
            if (dataRow == null)
            {
                return null;
            }

            ParcelModel parcel = new ParcelModel();
            parcel.parcelNumber = dataRow["ParcelNumber"].ToString();
            parcel.sourceSeqNumber = long.Parse(dataRow["ParcelSequenceNumber"].ToString());
            parcel.UID = dataRow["ParcelUID"].ToString();

            return parcel;
        }

        #endregion Methods
    }
}