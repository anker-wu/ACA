#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: AddressSearchList.ascx.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2014
 *
 *  Description:
 *
 *  Notes:
 *      $Id: AddressSearchList.ascx.cs 260158 2014-06-23 07:53:41Z ACHIEVO\james.shi $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using System;
using System.Collections.Generic;
using System.Data;
using System.Web.UI.WebControls;

using Accela.ACA.Common.Util;
using Accela.ACA.Web.Common;
using Accela.ACA.Web.Common.Control;
using Accela.ACA.WSProxy;
using Accela.Web.Controls;

namespace Accela.ACA.Web.Component
{
    /// <summary>
    /// the class for searched address list.
    /// </summary>
    public partial class AddressSearchList : BaseUserControl
    {
        #region Events

        /// <summary>
        /// Occurs when one of the pager buttons is clicked, but before the System.Web.UI.WebControls.GridView control handles the paging operation.
        /// </summary>
        public event GridViewPageEventHandler PageIndexChanging;

        /// <summary>
        /// Occurs when accela GridView has been sorted. notice that the GridView's Sorted or Sorting event is not validated.
        /// </summary>
        public event GridViewSortedEventHandler GridViewSort;

        /// <summary>
        /// Occurs when a radio button is clicked in a System.Web.UI.WebControls.GridView control.
        /// </summary>
        public event EventHandler Select;

        #endregion Events

        #region Properties

        /// <summary>
        /// Gets or sets a value indicating whether the smart choice validate for address
        /// </summary>
        public bool IsValidate
        {
            get;
            set;
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

                return dataIndex == -1 ? null : DataSource.Rows[SelectedRowIndex];
            }
        }

        /// <summary>
        /// Gets the selected address model.
        /// </summary>
        public AddressModel SelectedAddress
        {
            get
            {
                return GetAddressModel(SelectedRow);
            }
        }

        /// <summary>
        /// Gets the selected RefAddressModel only include PK information.
        /// </summary>
        public RefAddressModel SelectedRefAddressPK
        {
            get
            {
                DataRow dr = SelectedRow;
                RefAddressModel refAddressPK = new RefAddressModel();
                refAddressPK.refAddressId = StringUtil.ToLong(dr["AddressID"].ToString());
                refAddressPK.sourceNumber = StringUtil.ToInt(dr["AddressSequenceNumber"].ToString());
                refAddressPK.UID = dr["AddressUID"].ToString();

                return refAddressPK;
            }
        }

        /// <summary>
        /// Gets or sets the data source of address search list.
        /// </summary>
        public DataTable DataSource
        {
            get
            {
                return ViewState["AddressSearchListDataSrc"] as DataTable;
            }

            set
            {
                ViewState["AddressSearchListDataSrc"] = value;
            }
        }

        /// <summary>
        /// Gets or sets the number of records to display on a page in a System.Web.UI.WebControls.GridView control.
        /// </summary>
        public int PageSize
        {
            get
            {
                return gv.PageSize;
            }

            set
            {
                gv.PageSize = value;
            }
        }

        /// <summary>
        /// Gets or sets page index.
        /// </summary>
        public int PageIndex
        {
            get
            {
                return gv.PageIndex;
            }

            set
            {
                gv.PageIndex = value;
            }
        }

        /// <summary>
        /// Gets or sets a string to identify the GridView in whole aca system
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
        /// Gets or sets a value indicating whether auto post back when select an address.
        /// </summary>
        public bool AutoPostBackOnSelect
        {
            get;
            set;
        }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Bind Data Source
        /// </summary>
        /// <param name="dt">Data table.</param>
        public void BindDataSource(DataTable dt)
        {
            DataSource = dt;

            if (dt == null || dt.Rows.Count == 0)
            {
                gv.EmptyDataText = AppSession.IsAdmin ? string.Empty : GenerateNoSearchResultMessage();
            }
            else if (!string.IsNullOrEmpty(gv.GridViewSortExpression))
            {
                DataView dv = new DataView(dt);
                dv.Sort = gv.GridViewSortExpression + " " + gv.GridViewSortDirection;
                DataSource = dv.ToTable();
            }

            gv.Visible = true;
            gv.DataSource = DataSource;
            gv.DataBind();
        }

        /// <summary>
        /// Select the specified row
        /// </summary>
        /// <param name="rowIndex">Row index</param>
        public void SelectRow(int rowIndex)
        {
            gv.SelectRow(gv.Rows[rowIndex]);
        }

        /// <summary>
        /// Clear selected items.
        /// </summary>
        public void ClearSelectedItems()
        {
            gv.IsClearSelectedItems = true;
        }

        /// <summary>
        /// Raises the <c>System.Web.UI.Control.Init</c> event.
        /// </summary>
        /// <param name="e">An System.EventArgs object that contains the event data.</param>
        protected override void OnInit(EventArgs e)
        {
            GridViewBuildHelper.SetSimpleViewElements(gv, ModuleName, AppSession.IsAdmin);
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
                BindDataSource(DataSource);
            }
        }

        /// <summary>
        /// Raises ref-address list row command event - select address
        /// </summary>
        /// <param name="sender">An object that contains the event sender.</param>
        /// <param name="e">An GridViewCommandEventArgs object that contains the event data.</param>
        protected void AddressList_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Header")
            {
                gv.GridViewSortExpression = e.CommandArgument.ToString();
            }
        }

        /// <summary>
        /// Raises ref-address list page index changing command event - select address
        /// </summary>
        /// <param name="sender">An object that contains the event sender.</param>
        /// <param name="e">An GridViewCommandEventArgs object that contains the event data.</param>
        protected void Address_GridViewIndexChanging(object sender, GridViewPageEventArgs e)
        {
            if (PageIndexChanging != null)
            {
                PageIndexChanging(sender, e);
            }
        }

        /// <summary>
        /// PermitList GridViewSort
        /// </summary>
        /// <param name="sender">An object that contains the event sender.</param>
        /// <param name="e">A System.EventArgs object containing the event data.</param>
        protected void Address_GridViewSort(object sender, AccelaGridViewSortEventArgs e)
        {
            DataSource = gv.DataSource as DataTable;

            if (GridViewSort != null)
            {
                GridViewSort(sender, e);
            }
        }

        /// <summary>
        /// Select an address.
        /// </summary>
        /// <param name="sender">An object that contains the event sender.</param>
        /// <param name="e">A System.EventArgs object containing the event data.</param>
        protected void SelectAddressButton_OnClick(object sender, EventArgs e)
        {
            if (Select != null)
            {
                Select(this, e);
            }
        }

        /// <summary>
        /// Gets the address model from data row.
        /// </summary>
        /// <param name="dataRow">Data row.</param>
        /// <returns>Address model.</returns>
        private AddressModel GetAddressModel(DataRow dataRow)
        {
            if (dataRow == null)
            {
                return null;
            }

            AddressModel addressmodel = new AddressModel();

            addressmodel.city = Convert.ToString(dataRow["City"]);
            addressmodel.houseNumberStart = StringUtil.ToInt(Convert.ToString(dataRow["HouseNumberStart"]));
            addressmodel.unitStart = Convert.ToString(dataRow["UnitStart"]);
            addressmodel.city = Convert.ToString(dataRow["City"]);
            addressmodel.zip = Convert.ToString(dataRow["Zip"]);
            addressmodel.streetName = Convert.ToString(dataRow["StreetName"]);
            addressmodel.county = Convert.ToString(dataRow["County"]);
            addressmodel.refAddressId = StringUtil.ToLong(Convert.ToString(dataRow["AddressID"]));
            addressmodel.UID = Convert.ToString(dataRow["AddressUID"]);
            addressmodel.streetSuffix = Convert.ToString(dataRow["StreetSuffix"]);
            addressmodel.streetDirection = Convert.ToString(dataRow["StreetDirection"]);
            addressmodel.unitType = Convert.ToString(dataRow["UnitType"]);
            addressmodel.streetPrefix = Convert.ToString(dataRow["streetPrefix"]);
            addressmodel.houseNumberEnd = StringUtil.ToInt(Convert.ToString(dataRow["houseNumberEnd"]));
            addressmodel.unitEnd = Convert.ToString(dataRow["unitEnd"]);
            addressmodel.countryCode = Convert.ToString(dataRow["CountryCode"]);
            addressmodel.state = Convert.ToString(dataRow["State"]);
            addressmodel.houseFractionStart = Convert.ToString(dataRow["houseFractionStart"]);
            addressmodel.houseFractionEnd = Convert.ToString(dataRow["houseFractionEnd"]);
            addressmodel.streetSuffixdirection = Convert.ToString(dataRow["streetSuffixdirection"]);
            addressmodel.addressDescription = Convert.ToString(dataRow["addressDescription"]);
            addressmodel.distance = CommonUtil.GetDoubleValue(Convert.ToString(dataRow["distance"]));
            addressmodel.secondaryRoad = Convert.ToString(dataRow["secondaryRoad"]);
            addressmodel.secondaryRoadNumber = StringUtil.ToInt(Convert.ToString(dataRow["secondaryRoadNumber"]));
            addressmodel.inspectionDistrictPrefix = Convert.ToString(dataRow["inspectionDistrictPrefix"]);
            addressmodel.inspectionDistrict = Convert.ToString(dataRow["inspectionDistrict"]);
            addressmodel.neighborhoodPrefix = Convert.ToString(dataRow["neighberhoodPrefix"]);
            addressmodel.neighborhood = Convert.ToString(dataRow["neighborhood"]);
            addressmodel.XCoordinator = CommonUtil.GetDoubleValue(Convert.ToString(dataRow["xcoordinator"]));
            addressmodel.YCoordinator = CommonUtil.GetDoubleValue(Convert.ToString(dataRow["ycoordinator"]));
            addressmodel.fullAddress = Convert.ToString(dataRow["fullAddress0"]);
            addressmodel.addressLine1 = Convert.ToString(dataRow["AddressLine1"]);
            addressmodel.addressLine2 = Convert.ToString(dataRow["AddressLine2"]);
            addressmodel.levelPrefix = Convert.ToString(dataRow["LevelPrefix"]);
            addressmodel.levelNumberStart = Convert.ToString(dataRow["LevelNumberStart"]);
            addressmodel.levelNumberEnd = Convert.ToString(dataRow["LevelNumberEnd"]);
            addressmodel.houseNumberAlphaStart = Convert.ToString(dataRow["HouseAlphaStart"]);
            addressmodel.houseNumberAlphaEnd = Convert.ToString(dataRow["HouseAlphaEnd"]);

            if (DBNull.Value != dataRow["AddressAPOKeys"])
            {
                addressmodel.duplicatedAPOKeys = dataRow["AddressAPOKeys"] as DuplicatedAPOKeyModel[];
            }

            if (DBNull.Value != dataRow["AddressAttributes"])
            {
                addressmodel.templates = (TemplateAttributeModel[])dataRow["AddressAttributes"];
            }

            return addressmodel;
        }

        /// <summary>
        /// Get record not found message
        /// </summary>
        /// <returns>Message to display</returns>
        private string GenerateNoSearchResultMessage()
        {
            if (IsValidate)
            {
                return GetTextByKey("per_workLocationInfo_label_NoAddressFound");
            }
            else
            {
                return GetTextByKey("per_workLocationInfo_label_ManuallyEnterAddress");
            }
        }

        #endregion Methods
    }
}
