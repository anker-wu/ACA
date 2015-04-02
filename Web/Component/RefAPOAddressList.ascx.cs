#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: RefAPOAddressList.ascx.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2007-2014
 *
 *  Description:
 *
 *  Notes:
 *      $Id: RefAPOAddressList.ascx.cs 278666 2014-09-10 08:07:18Z ACHIEVO\james.shi $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Web.UI;
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
    /// The Reference APO address list.
    /// </summary>
    public partial class RefAPOAddressList : BaseUserControl
    {
        #region Fields

        /// <summary>
        /// The padding status parcel.
        /// </summary>
        private const string PENDING_PARCEL = "P";

        /// <summary>
        /// The biz domain list
        /// </summary>
        private IList<ItemValue> _bizDomainList;

        /// <summary>
        /// whether show map or not
        /// </summary>
        private bool _isShowMap = true;

        #endregion Fields

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
        /// grid view download event.
        /// </summary>
        public event GridViewDownloadEventHandler GridViewDownloadAll;

        #endregion Events

        #region Properties

        /// <summary>
        /// Gets or sets a value indicating APO search type (address,owner,........)
        /// </summary>
        public int ComponentType
        {
            get
            {
                int _componentType = -1;

                if (ViewState["ComponentType"] != null)
                {
                    return (int)ViewState["ComponentType"];
                }

                return _componentType;
            }

            set
            {
                ViewState["ComponentType"] = value;
            }
        }

        /// <summary>
        /// Gets or sets the view id for APO list
        /// </summary>
        public string GViewID
        {
            get
            {
                return gvAPOList.GridViewNumber;
            }

            set
            {
                gvAPOList.GridViewNumber = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether hide page header.
        /// </summary>
        public bool HideHeader
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets a value indicating export GridView's name 
        /// </summary>
        public string ExportFileName
        {
            get
            {
                return gvAPOList.ExportFileName;
            }

            set
            {
                gvAPOList.ExportFileName = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether show map.
        /// </summary>
        public bool IsShowMap
        {
            get
            {
                return _isShowMap;
            }

            set
            {
                _isShowMap = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether show RadioButtonColumn
        /// </summary>
        public bool IsShowRadioButtonColumn
        {
            get
            {
                return gvAPOList.AutoGenerateRadioButtonColumn;
            }

            set
            {
                gvAPOList.AutoGenerateRadioButtonColumn = value;
            }
        }

        /// <summary>
        /// Gets the page size of GridView 
        /// </summary>
        public int PageSize
        {
            get
            {
                return gvAPOList.PageSize;
            }
        }

        /// <summary>
        /// Gets or sets the reference APO data source.
        /// </summary>
        public DataTable RefAPODataSource
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
        /// Gets the display count.
        /// </summary>
        public string CountSummary
        {
            get
            {
                return gvAPOList.CountSummary;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether whether APO record only includes address or parcel or owner record or not.
        /// </summary>
        public bool IsSingleAPOData 
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets a value indicating whether show export link or not.
        /// </summary>
        public bool ShowExportLink
        {
            get
            {
                return gvAPOList.ShowExportLink;
            }

            set
            {
                gvAPOList.ShowExportLink = value;
            }
        }

        #endregion Properties

        #region Public Methods

        /// <summary>
        /// bind data source
        /// </summary>
        /// <param name="dt">data table.</param>
        public void BindDataSource(DataTable dt)
        {
            BindDataSource(dt, false);
        }

        /// <summary>
        /// Bind data source
        /// </summary>
        /// <param name="dt">Data table.</param>
        /// <param name="resetPageIndex">Reset page index flag</param>
        /// <param name="type">The search type for Look Up by Property Information</param>
        public void BindDataSource(DataTable dt, bool resetPageIndex, APOShowType type = APOShowType.None)
        {
            if (type != APOShowType.None && dt != null && dt.Rows.Count == 1)
            {
                RedirectToAPODetail(dt.Rows[0], type);
            }

            InitAddressList();

            if (dt == null || dt.Rows.Count == 0)
            {
                gvAPOList.EmptyDataText = AppSession.IsAdmin ? string.Empty : GetTextByKey("per_permitList_messagel_noRecord");
                gvAPOList.DataSource = null;
                gvAPOList.DataBind();

                return;
            }

            RefAPODataSource = dt;
            GenerateTableWithMask(dt);
            DataView dv = new DataView(dt);
            string gridViewSort = GetGridViewSort();

            if (!string.IsNullOrEmpty(gvAPOList.GridViewSortExpression))
            {
                dv.Sort = gvAPOList.GridViewSortExpression + " " + gvAPOList.GridViewSortDirection;
            }
            else if (!string.IsNullOrEmpty(gridViewSort))
            {
                dv.Sort = gridViewSort;
            }

            gvAPOList.Visible = true;
            gvAPOList.DataSource = dv.ToTable();

            if (resetPageIndex)
            {
                gvAPOList.PageIndex = 0;
            }

            gvAPOList.DataBind();
        }

        /// <summary>
        /// Clear selected items.
        /// </summary>
        public void ClearSelectedItems()
        {
            this.gvAPOList.IsClearSelectedItems = true;
        }

        /// <summary>
        /// Close the map
        /// </summary>
        public void CloseMap()
        {
            mapAddress.CloseMap();
        }

        /// <summary>
        /// Gets the selected data.
        /// </summary>
        /// <returns>The selected data.</returns>
        public DataTable GetSelectData()
        {
            return gvAPOList.GetSelectedData(gvAPOList.DataSource as DataTable);
        }

        /// <summary>
        /// Bind data source
        /// </summary>
        /// <param name="dt">the data table to be bind.</param>
        /// <param name="pageIndex">GridView page index</param>
        /// <param name="pageSort">GridView sort</param>
        public void BindDataSource(DataTable dt, int pageIndex, string pageSort)
        {
            GridViewBuildHelper.SetSimpleViewElements(gvAPOList, ModuleName, false);

            if (dt == null || dt.Rows.Count == 0)
            {
                gvAPOList.EmptyDataText = AppSession.IsAdmin ? string.Empty : GetTextByKey("per_permitList_messagel_noRecord");
                gvAPOList.DataSource = null;
                gvAPOList.DataBind();

                return;
            }

            InitAddressList();

            RefAPODataSource = dt;
            GenerateTableWithMask(dt);
            DataView dv = new DataView(dt);

            if (pageSort != string.Empty)
            {
                dv.Sort = pageSort;
            }

            gvAPOList.Visible = true;
            gvAPOList.PageIndex = pageIndex;
            gvAPOList.DataSource = dv.ToTable();
            gvAPOList.DataBind();
        }

        /// <summary>
        /// according the standard choice to display or not.
        /// </summary>
        /// <param name="showExportLink">If show ExportLink, set true; Otherwise false.</param>
        public void InitialExport(bool showExportLink)
        {
            gvAPOList.ShowExportLink = showExportLink;
        }

        /// <summary>
        /// Gets the display parcel status.
        /// </summary>
        /// <param name="parcelStatus">The parcel status.</param>
        /// <returns>The display parcel status.</returns>
        public string GetDisplayParcelStatus(string parcelStatus)
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
        /// Validate the APO data whether its single APO(Only Address or parcel or owner) data or not.
        /// </summary>
        /// <param name="apoRow">APO data row</param>
        /// <returns>APO result model</returns>
        public APOResultModel ValidateSingleAPOData(DataRow apoRow)
        {
            APOResultModel apoResult = new APOResultModel();

            string fullAddress = apoRow["FullAddress"] != null ? apoRow["FullAddress"].ToString() : string.Empty;
            string parcelNumber = apoRow["ParcelNumber"] != null ? apoRow["ParcelNumber"].ToString() : string.Empty;
            string ownerFullName = apoRow["OwnerFullName"] != null ? apoRow["OwnerFullName"].ToString() : string.Empty;
            bool isAddressVisible = GetColumnVisibleProperty("lnkAddressHeader");
            bool isParcelVisible = GetColumnVisibleProperty("lnkParcelNumberHeader");
            bool isOwnerVisible = GetColumnVisibleProperty("lnkOwnerHeader");

            apoResult.IsOnlyAddress = (!string.IsNullOrEmpty(fullAddress) && isAddressVisible)
                                 && (string.IsNullOrEmpty(ownerFullName) || !isOwnerVisible)
                                 && (string.IsNullOrEmpty(parcelNumber) || !isParcelVisible);

            apoResult.IsOnlyParcel = (!string.IsNullOrEmpty(parcelNumber) && isParcelVisible)
                                && (string.IsNullOrEmpty(ownerFullName) || !isOwnerVisible)
                                && (string.IsNullOrEmpty(fullAddress) || !isAddressVisible);

            apoResult.IsOnlyOwner = (!string.IsNullOrEmpty(ownerFullName) && isOwnerVisible)
                               && (string.IsNullOrEmpty(parcelNumber) || !isParcelVisible)
                               && (string.IsNullOrEmpty(fullAddress) || !isAddressVisible);

            apoResult.IsSingleAPOData = apoResult.IsOnlyAddress || apoResult.IsOnlyParcel || apoResult.IsOnlyOwner;

            return apoResult;
        }

        #endregion Public Methods

        #region Protected Methods

        /// <summary>
        /// Raises the <c>System.Web.UI.Control.Init</c> event.
        /// </summary>
        /// <param name="e">An System.EventArgs object that contains the event data.</param>
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            //if standard choice display owner section is "N" and is in daily side, set owner column is hidden.
            if (!AppSession.IsAdmin && !StandardChoiceUtil.IsDisplayOwnerSection())
            {
                GridViewBuildHelper.SetHiddenColumn(gvAPOList, new[] { "Owner" });
            }

            GridViewBuildHelper.SetSimpleViewElements(gvAPOList, ModuleName, false);
        }

        /// <summary>
        /// Fire page index changing event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void RefAddressList_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            if (PageIndexChanging != null)
            {
                PageIndexChanging(sender, e);
            }
        }

        /// <summary>
        /// Fire sorted event.
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void RefAddressList_GridViewSort(object sender, AccelaGridViewSortEventArgs e)
        {
            if (GridViewSort != null)
            {
                GridViewSort(sender, e);
            }
        }

        /// <summary>
        /// Handles the ShowACAMap event of the mapAddress control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected void MapAddress_ShowACAMap(object sender, EventArgs e)
        {
            ACAGISModel gisModel = GISUtil.CreateACAGISModel();
            gisModel.Context = mapAddress.AGISContext;
            gisModel.UserGroups.Add(AppSession.User.IsAnonymous ? GISUserGroup.Anonymous.ToString() : GISUserGroup.Register.ToString());
            gisModel.ModuleName = ModuleName;
            SearchType searchType = (SearchType)this.ComponentType;
            DataTable dt = null;

            if (gvAPOList.DataSource != null)
            {
                if (gvAPOList.DataSource is DataTable)
                {
                    dt = gvAPOList.DataSource as DataTable;
                }
                else
                {
                    DataView dv = gvAPOList.DataSource as DataView;
                    dt = dv.Table;
                }

                dt = gvAPOList.GetSelectedData(dt, gvAPOList.PageIndex);
            }

            if (dt != null && dt.Rows.Count > 0)
            {
                List<ParcelInfoModel> parcelInfos = new List<ParcelInfoModel>();

                foreach (DataRow item in dt.Rows)
                {
                    ParcelInfoModel parcelInfo = new ParcelInfoModel();
                    string parcelNbr = item["ParcelNumber"] as string;
                    string unmaskedparcelNbr = item["UnmaskedParcelNumber"] as string;
                    
                    if (!string.IsNullOrEmpty(parcelNbr))
                    {
                        parcelInfo.parcelModel = new ParcelModel();
                        parcelInfo.parcelModel.parcelNumber = parcelNbr;
                        parcelInfo.parcelModel.unmaskedParcelNumber = unmaskedparcelNbr;
                        parcelInfo.parcelModel.sourceSeqNumber = long.Parse(item["ParcelSequenceNumber"].ToString());
                        parcelInfo.parcelModel.duplicatedAPOKeys = item["ParcelAPOKeys"] as DuplicatedAPOKeyModel[];
                    }

                    RefAddressModel address = new RefAddressModel();

                    address = APOUtil.InitialRefAddressModel(item, parcelNbr);
                    parcelInfo.RAddressModel = address;
                    parcelInfos.Add(parcelInfo);
                }

                switch (searchType)
                {
                    case SearchType.Address:
                        gisModel.AddressInfoModels = parcelInfos.ToArray();
                        break;
                    case SearchType.Parcel:
                        gisModel.ParcelInfoModels = parcelInfos.ToArray();
                        break;
                    case SearchType.License:
                        gisModel.ParcelInfoModels = parcelInfos.ToArray();
                        break;
                }
            }

            UpdateAPOQueryInfo();

            GISUtil.SetPostUrl(this.Page, gisModel);
            mapAddress.ACAGISModel = gisModel;
        }

        /// <summary>
        /// Handles the PageIndexChanged event.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected void RefAddressList_PageIndexChanged(object sender, EventArgs e)
        {
            if (mapAddress.Opened)
            {
                mapAddress.ShowMap();
                mapPanel.Update();
            }
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
                BindDataSource(RefAPODataSource);
            }
        }

        /// <summary>
        /// Handles the RowCommand event.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Web.UI.WebControls.GridViewCommandEventArgs"/> instance containing the event data.</param>
        protected void RefAddressList_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int index;

            if (e.CommandName == "Header")
            {
                gvAPOList.GridViewSortExpression = e.CommandArgument.ToString();
            }
            else if (e.CommandName == APOShowType.ShowAddress.ToString() || e.CommandName == APOShowType.ShowParcel.ToString() || e.CommandName == APOShowType.ShowOwner.ToString())
            {
                index = int.Parse(e.CommandArgument.ToString());
                GridViewRow row = gvAPOList.Rows[index];
                DataRow apoRow = ((DataTable)gvAPOList.DataSource).Rows[row.DataItemIndex];
                APOShowType showName = EnumUtil<APOShowType>.Parse(e.CommandName);
                RedirectToAPODetail(apoRow, showName);
            }
        }

        /// <summary>
        /// Handles the RowDataBound event.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Web.UI.WebControls.GridViewRowEventArgs"/> instance containing the event data.</param>
        protected void RefAddressList_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                switch (ComponentType)
                {
                    case (int)SearchType.Address:
                        if (Request.QueryString["childParcelNum"] != null && !string.IsNullOrEmpty(Request.QueryString["childParcelNum"].ToString()))
                        {
                            ((AccelaLabel)e.Row.FindControl("lblParcelNumber")).Text = Request.QueryString["childParcelNum"].ToString();
                        }

                        e.Row.FindControl("divParcelNum").Visible = true;
                        e.Row.FindControl("divLnkParcelNum").Visible = false;
                        e.Row.FindControl("divOwner").Visible = true;
                        e.Row.FindControl("divlnkOwner").Visible = false;
                        e.Row.FindControl("divAddress").Visible = false;
                        e.Row.FindControl("divLnkAddress").Visible = true;
                        break;

                    case (int)SearchType.Parcel:
                        e.Row.FindControl("divParcelNum").Visible = false;
                        e.Row.FindControl("divLnkParcelNum").Visible = true;
                        e.Row.FindControl("divOwner").Visible = true;
                        e.Row.FindControl("divlnkOwner").Visible = false;
                        e.Row.FindControl("divAddress").Visible = true;
                        e.Row.FindControl("divLnkAddress").Visible = false;

                        string parcelStatus = ((DataRowView)e.Row.DataItem)["ParcelStatus"].ToString();
                        ((AccelaLabel)e.Row.FindControl("lblParcelStatus")).Text = GetDisplayParcelStatus(parcelStatus);
                        break;

                    case (int)SearchType.Owner:
                        e.Row.FindControl("divParcelNum").Visible = false;
                        e.Row.FindControl("divLnkParcelNum").Visible = true;
                        e.Row.FindControl("divOwner").Visible = false;
                        e.Row.FindControl("divlnkOwner").Visible = false;
                        e.Row.FindControl("divAddress").Visible = false;
                        e.Row.FindControl("divLnkAddress").Visible = true;
                        break;
                    case (int)SearchType.License:
                        e.Row.FindControl("divParcelNum").Visible = false;
                        e.Row.FindControl("divLnkParcelNum").Visible = true;
                        e.Row.FindControl("divOwner").Visible = false;
                        e.Row.FindControl("divlnkOwner").Visible = true;
                        e.Row.FindControl("divAddress").Visible = false;
                        e.Row.FindControl("divLnkAddress").Visible = true;
                        break;
                }
            }
        }

        #endregion Protected Methods

        #region Private Methods

        /// <summary>
        /// RefAddressList download event method.
        /// </summary>
        /// <param name="sender">An object that contains the event sender.</param>
        /// <param name="e">A GridViewDownloadEventArgs object containing the event data.</param>
        protected void RefAddressList_GridViewDownload(object sender, GridViewDownloadEventArgs e)
        {
            // execute the download event that setting the exported content.
            if (GridViewDownloadAll != null)
            {
                GridViewDownloadAll(sender, e);
            }
        }

        /// <summary>
        /// Initializes Address list
        /// </summary>
        private void InitAddressList()
        {
            SearchType searchType = (SearchType)this.ComponentType;
            if (searchType == SearchType.Owner)
            {
                mapAddress.HideGISButton();
                gvAPOList.AutoGenerateCheckBoxColumn = false;
            }
            else
            {
                if (IsShowMap && StandardChoiceUtil.IsShowMap4ShowObject(this.ModuleName))
                {
                    gvAPOList.AutoGenerateCheckBoxColumn = true;
                    gvAPOList.CheckBoxColumnIndex = 0;
                    mapAddress.ShowGISButton();
                }
                else
                {
                    gvAPOList.AutoGenerateCheckBoxColumn = false;
                    mapAddress.HideGISButton();
                }
            }
        }

        /// <summary>
        /// Updates the APO query info.
        /// </summary>
        private void UpdateAPOQueryInfo()
        {
            ApoQueryInfo apoQueryInfo = (ApoQueryInfo)Session[SessionConstant.SESSION_APO_QUERY];

            if (apoQueryInfo != null)
            {
                apoQueryInfo.IsFromSearchPage = true;
                apoQueryInfo.APOAddressPageIndex = gvAPOList.PageIndex;

                if (!string.IsNullOrEmpty(gvAPOList.GridViewSortExpression))
                {
                    apoQueryInfo.APOAddressPageSort = gvAPOList.GridViewSortExpression + " " + gvAPOList.GridViewSortDirection;
                }

                Session[SessionConstant.SESSION_APO_QUERY] = apoQueryInfo;
            }
        }

        /// <summary>
        /// Format the parcel number with the mask
        /// </summary>
        /// <param name="dt">Data table.</param>
        /// <returns>The data table with mask.</returns>
        private DataTable GenerateTableWithMask(DataTable dt)
        {
            bool isMasked = false;
            bool isInData = false;
            ItemValue itmMask = new ItemValue();
            IBizDomainBll bizBll = (IBizDomainBll)ObjectFactory.GetObject(typeof(IBizDomainBll));
            _bizDomainList = bizBll.GetBizDomainList(ConfigManager.AgencyCode, BizDomainConstant.STD_CAT_PARCEL_MASK, false);

            if (_bizDomainList == null || _bizDomainList.Count == 0)
            {
                return dt;
            }

            foreach (ItemValue itm in _bizDomainList)
            {
                if (itm.Key == "Parcel_ID_Masked_in_DB")
                {
                    if (itm.Value.ToString() == ACAConstant.COMMON_YES)
                    {
                        return dt;
                    }

                    if (itm.Value.ToString() == ACAConstant.COMMON_NO)
                    {
                        isInData = true;
                    }

                    if (itm.Key == "Parcel_ID" && itm.Value != null)
                    {
                        isMasked = true;
                        itmMask = itm;
                    }
                }
            }

            if (isInData && isMasked)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    DataRow row = dt.Rows[i];
                    row["ParcelNumber"] = MakeMaskParcelNumber(row["ParcelNumber"].ToString(), itmMask.Value.ToString());
                }
            }

            return dt;
        }

        /// <summary>
        /// Gets the grid view sort.
        /// </summary>
        /// <returns>The grid view sort.</returns>
        private string GetGridViewSort()
        {
            ApoQueryInfo apoQueryInfo = (ApoQueryInfo)Session[SessionConstant.SESSION_APO_QUERY];
            if (apoQueryInfo != null && !string.IsNullOrEmpty(apoQueryInfo.APOAddressPageSort))
            {
                return apoQueryInfo.APOAddressPageSort;
            }

            return string.Empty;
        }

        /// <summary>
        /// this function is used to format the parcel number with the given mask value
        /// </summary>
        /// <param name="parcelNum">The parcel number.</param>
        /// <param name="mask">The mask for the number.</param>
        /// <returns>The parcel number with mask.</returns>
        private string MakeMaskParcelNumber(string parcelNum, string mask)
        {
            StringBuilder newStr = new StringBuilder();
            int tempNumLength = 0;

            for (int i = 0; i < mask.Length; i++)
            {
                string maskUnit = mask.Substring(i, 1);
                if (tempNumLength == parcelNum.Length)
                {
                    continue;
                }

                if (maskUnit == "-")
                {
                    if (parcelNum.Substring(tempNumLength, 1) != "-")
                    {
                        newStr.Append("-");
                    }
                }
                else
                {
                    newStr.Append(parcelNum.Substring(tempNumLength, 1));
                    tempNumLength++;
                }
            }

            return newStr.ToString();
        }

        /// <summary>
        /// Navigate To APO detail page.
        /// </summary>
        /// <param name="apoRow">APO data row</param>
        /// <param name="showName">Search Type Name</param>
        private void RedirectToAPODetail(DataRow apoRow, APOShowType showName)
        {
            APOShowType searchTypeName = showName;

            //ShowAddressByLP and ShowAddressByCap
            if (showName == APOShowType.ShowAddressByLp)
            {
                APOResultModel apoResult = ValidateSingleAPOData(apoRow);

                if (apoResult.IsOnlyAddress)
                {
                    searchTypeName = APOShowType.ShowAddress;
                }
                else if (apoResult.IsOnlyParcel)
                {
                    searchTypeName = APOShowType.ShowParcel;
                }
                else if (apoResult.IsOnlyOwner)
                {
                    searchTypeName = APOShowType.ShowOwner;
                }
                else
                {
                    searchTypeName = APOShowType.None;
                }

                IsSingleAPOData = apoResult.IsSingleAPOData;
            }
            else if (showName == APOShowType.ShowAddressByCap)
            {
                searchTypeName = APOShowType.ShowAddress;
            }

            // if the searchTypeName value is empty, it means the list have more than one link
            if (searchTypeName == APOShowType.None)
            {
                return;
            }

            // navigate to APO detail page
            DuplicatedAPOKeyModel[] apoKeys = apoRow["AddressAPOKeys"] as DuplicatedAPOKeyModel[];
            List<string> sourceNumbers = new List<string>();

            if (apoKeys != null)
            {
                sourceNumbers.AddRange(from key in apoKeys where key != null select key.sourceNumber);
            }

            string addressId  = apoRow["AddressID"] != null ? apoRow["AddressID"].ToString() : string.Empty;
            string addressUid = apoRow["AddressUid"] != null ? apoRow["AddressUid"].ToString() : string.Empty;
            string addressSequence = apoRow["AddressSequenceNumber"] != null ? apoRow["AddressSequenceNumber"].ToString() : string.Empty;
           
            string parcelNum = apoRow["ParcelNumber"] != null ? apoRow["ParcelNumber"].ToString() : string.Empty;
            string parcelUid = apoRow["ParcelUID"] != null ? apoRow["ParcelUID"].ToString() : string.Empty;
            string parcelSequence = apoRow["ParcelSequenceNumber"] != null ? apoRow["ParcelSequenceNumber"].ToString() : string.Empty;

            string ownerNum = apoRow["OwnerNumber"] != null ? apoRow["OwnerNumber"].ToString() : string.Empty;
            string ownerUid = apoRow["OwnerUID"] != null ? apoRow["OwnerUID"].ToString() : string.Empty;
            string ownerSequence = apoRow["OwnerSequenceNumber"] != null ? apoRow["OwnerSequenceNumber"].ToString() : string.Empty;

            string url = string.Empty;

            if (GViewID != GviewID.GISAPOList)
            {
                UpdateAPOQueryInfo();
            }

            string urlParam = string.Format(
                "&{0}={1}&{2}={3}&{4}={5}&{6}={7}",
                ACAConstant.REQUEST_PARMETER_REFADDRESS_ID,
                UrlEncode(addressId),
                ACAConstant.REQUEST_PARMETER_ADDRESS_SEQUENCE,
                UrlEncode(addressSequence),
                ACAConstant.REQUEST_PARMETER_PARCEL_NUMBER,
                UrlEncode(parcelNum),
                UrlConstant.AgencyCode,
                Page.Request[UrlConstant.AgencyCode]);
               
            switch (searchTypeName)
            {
                case APOShowType.ShowAddress:
                    url = string.Format(
                        "APO/AddressDetail.aspx?{0}={1}&" + UrlConstant.HIDERHEADER + "={2}&{3}={4}",
                        ACAConstant.REQUEST_PARMETER_REFADDRESS_UID,
                        UrlEncode(addressUid),
                        Page.Request["HideHeader"],
                        ACAConstant.REQUEST_PARMETER_APO_SOURCE_NUMBERS,
                        UrlEncode(string.Join(",", sourceNumbers)));
                    break;
                case APOShowType.ShowParcel:
                    url = string.Format(
                        "APO/ParcelDetail.aspx?{0}={1}&{2}={3}&{4}={5}",
                        ACAConstant.REQUEST_PARMETER_PARCEL_SEQUENCE,
                        UrlEncode(parcelSequence),
                        ACAConstant.REQUEST_PARMETER_PARCEL_UID,
                        UrlEncode(parcelUid),
                        ACAConstant.REQUEST_PARMETER_APO_SOURCE_NUMBERS,
                        UrlEncode(string.Join(",", sourceNumbers)));

                    if (GViewID == GviewID.GISAPOList)
                    {
                        string scripts = "parent.location.href='" + FileUtil.AppendApplicationRoot(url + urlParam) + "'";
                        ScriptManager.RegisterStartupScript(updatePanel, updatePanel.GetType(), "ShowParcel", scripts, true);
                        return;
                    }

                    break;
                case APOShowType.ShowOwner:
                    url = string.Format(
                        "APO/OwnerDetail.aspx?{0}={1}&{2}={3}&{4}={5}&{6}={7}&{8}={9}&{10}={11}&" + UrlConstant.HIDERHEADER + "={12}",
                        ACAConstant.REQUEST_PARMETER_OWNER_NUMBER,
                        UrlEncode(ownerNum),
                        ACAConstant.REQUEST_PARMETER_OWNER_SEQUENCE,
                        UrlEncode(ownerSequence),
                        ACAConstant.REQUEST_PARMETER_OWNER_UID,
                        UrlEncode(ownerUid),
                        ACAConstant.REQUEST_PARMETER_PARCEL_UID,
                        UrlEncode(parcelUid),
                        ACAConstant.REQUEST_PARMETER_PARCEL_SEQUENCE,
                        UrlEncode(parcelSequence),
                        ACAConstant.REQUEST_PARMETER_REFADDRESS_UID,
                        UrlEncode(addressUid),
                        Page.Request["HideHeader"]);
                    break;
            }

            string redirectUrl = FileUtil.AppendApplicationRoot(url + urlParam);
            Response.Redirect(redirectUrl);
        }

        /// <summary>
        /// Get the GridView Header Visible property.
        /// </summary>
        /// <param name="attributeName">GridView Header Attribute Name</param>
        /// <returns>Column visible flag</returns>
        private bool GetColumnVisibleProperty(string attributeName)
        {
            foreach (DataControlField gridColumn in gvAPOList.Columns)
            {
                AccelaTemplateField accelaTemplateField = gridColumn as AccelaTemplateField;

                if (accelaTemplateField != null && accelaTemplateField.AttributeName.Equals(attributeName, StringComparison.InvariantCultureIgnoreCase))
                {
                    return accelaTemplateField.Visible;
                }
            }

            return false;
        }

        #endregion Private Methods
    }
}