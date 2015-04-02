#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: RefAddressLookUpList.aspx.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2014
 *
 *  Description:
 *
 *  Notes:
 *      $Id: RefAddressLookUpList.aspx.cs 269110 2014-07-15 08:24:39Z ACHIEVO\canon.wu $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;

using Accela.ACA.BLL.APO;
using Accela.ACA.Common;
using Accela.ACA.Common.Util;
using Accela.ACA.Web.Common;
using Accela.ACA.Web.Common.Control;
using Accela.ACA.WSProxy;
using Accela.Web.Controls;

namespace Accela.ACA.Web.Component
{
    /// <summary>
    /// RefAddressLookUpList Control.
    /// </summary>
    public partial class RefAddressLookUpList : BaseUserControl
    {
        #region Fields

        /// <summary>
        /// whether show map or not
        /// </summary>
        private bool _isShowMap = true;

        /// <summary>
        /// grid view row command event.
        /// </summary>
        public event CommonEventHandler AddressSelected;

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
        /// Gets or sets the view id for Reference Address list
        /// </summary>
        public string GViewID
        {
            get
            {
                return dgvRefAddressLookUpList.GridViewNumber;
            }

            set
            {
                dgvRefAddressLookUpList.GridViewNumber = value;
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
        /// Gets or sets a value indicating export GridView's name 
        /// </summary>
        public string ExportFileName
        {
            get
            {
                return this.dgvRefAddressLookUpList.ExportFileName;
            }

            set
            {
                this.dgvRefAddressLookUpList.ExportFileName = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether show export link.
        /// </summary>
        public bool ShowExportLink
        {
            get
            {
                return dgvRefAddressLookUpList.ShowExportLink;
            }

            set
            {
                dgvRefAddressLookUpList.ShowExportLink = value;
            }
        }

        /// <summary>
        /// Gets the page size of GridView 
        /// </summary>
        public int PageSize
        {
            get
            {
                return dgvRefAddressLookUpList.PageSize;
            }
        }

        /// <summary>
        /// Gets or sets the reference Address data source.
        /// </summary>
        public DataTable RefAddressDataSource
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
                return dgvRefAddressLookUpList.CountSummary;
            }
        }

        #endregion Properties

        #region Public Methods

        /// <summary>
        /// Close the map
        /// </summary>
        public void CloseMap()
        {
            mapAddress.CloseMap();
        }

        /// <summary>
        /// Clear selected items.
        /// </summary>
        public void ClearSelectedItems()
        {
            dgvRefAddressLookUpList.IsClearSelectedItems = true;
        }

        /// <summary>
        /// Bind reference address data source
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
        /// <param name="pageIndex">GridView page index</param>
        /// <param name="pageSort">GridView sort</param>
        public void BindDataSource(DataTable dt, bool resetPageIndex, int pageIndex = 0, string pageSort = null)
        {
            InitGISMapForAddressList();

            if (dt == null || dt.Rows.Count == 0)
            {
                dgvRefAddressLookUpList.EmptyDataText = AppSession.IsAdmin ? string.Empty : GetTextByKey("per_permitList_messagel_noRecord");
                dgvRefAddressLookUpList.DataSource = null;
                dgvRefAddressLookUpList.DataBind();

                return;
            }

            RefAddressDataSource = dt;
            DataView dv = new DataView(dt);
            string gridViewSort = GetGridViewSort();

            if (!string.IsNullOrEmpty(pageSort))
            {
                dv.Sort = pageSort;
            }
            else if (!string.IsNullOrEmpty(dgvRefAddressLookUpList.GridViewSortExpression))
            {
                dv.Sort = dgvRefAddressLookUpList.GridViewSortExpression + " " + dgvRefAddressLookUpList.GridViewSortDirection;
            }
            else if (!string.IsNullOrEmpty(gridViewSort))
            {
                dv.Sort = gridViewSort;
            }

            if (resetPageIndex)
            {
                dgvRefAddressLookUpList.PageIndex = 0;
            }
            else if (pageIndex != 0)
            {
                dgvRefAddressLookUpList.PageIndex = pageIndex;
            }

            dgvRefAddressLookUpList.Visible = true;
            dgvRefAddressLookUpList.DataSource = dv.ToTable();
            dgvRefAddressLookUpList.DataBind();
        }

        /// <summary>
        /// Retrieve associated records for the selected address record.
        /// </summary>
        /// <param name="addressRow">Address data row</param>
        public void RetrieveAssociatedData(DataRow addressRow)
        {
            AddressSelected(null, new CommonEventArgs(addressRow));
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

            GridViewBuildHelper.SetSimpleViewElements(dgvRefAddressLookUpList, ModuleName, false);
        }

        /// <summary>
        /// Handles the RowCommand event.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Web.UI.WebControls.GridViewCommandEventArgs"/> instance containing the event data.</param>
        protected void RefAddress_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            //Bind RefAddress Data Source.
            BindDataSource(RefAddressDataSource);

            if (e.CommandName == "Header")
            {
                dgvRefAddressLookUpList.GridViewSortExpression = e.CommandArgument.ToString();
            }
            else
            {
                if (e.CommandName == "RetrieveData" || e.CommandName == APOShowType.ShowAddress.ToString())
                {
                    int index = int.Parse(e.CommandArgument.ToString());
                    GridViewRow row = dgvRefAddressLookUpList.Rows[index];
                    DataRow addressRow = ((DataTable)dgvRefAddressLookUpList.DataSource).Rows[row.DataItemIndex];

                    //Update the selected pageIndex and sortExpression to session.
                    UpdateAPOQueryInfo();

                    if (e.CommandName == "RetrieveData" && AddressSelected != null)
                    {
                        AddressSelected(sender, new CommonEventArgs(addressRow));
                    }
                    else
                    {
                        RedirectToAddressDetail(addressRow);
                    }
                }
            }
        }

        /// <summary>
        /// Handles the ShowACAMap event of the mapAddress control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected void MapAddress_ShowACAMap(object sender, EventArgs e)
        {
            //Bind RefAddress Data Source.
            BindDataSource(RefAddressDataSource);

            ACAGISModel gisModel = GISUtil.CreateACAGISModel();
            gisModel.Context = mapAddress.AGISContext;
            gisModel.UserGroups.Add(AppSession.User.IsAnonymous ? GISUserGroup.Anonymous.ToString() : GISUserGroup.Register.ToString());
            gisModel.ModuleName = ModuleName;

            DataTable dt = null;
            if (dgvRefAddressLookUpList.DataSource != null)
            {
                if (dgvRefAddressLookUpList.DataSource is DataTable)
                {
                    dt = dgvRefAddressLookUpList.DataSource as DataTable;
                }
                else
                {
                    DataView dv = dgvRefAddressLookUpList.DataSource as DataView;
                    dt = dv.Table;
                }

                dt = dgvRefAddressLookUpList.GetSelectedData(dt, dgvRefAddressLookUpList.PageIndex);
            }

            if (dt != null && dt.Rows.Count > 0)
            {
                List<ParcelInfoModel> parcelInfos = new List<ParcelInfoModel>();
                foreach (DataRow item in dt.Rows)
                {
                    ParcelInfoModel parcelInfo = new ParcelInfoModel();

                    //Need get address associated parcels before plot to map.
                    RefAddressModel refAddressPK = new RefAddressModel();
                    RefAddressModel address = APOUtil.InitialRefAddressModel(item);

                    //Get addresss associated parcel records by ref address PK model.
                    refAddressPK.refAddressId = address.refAddressId;
                    refAddressPK.sourceNumber = address.sourceNumber;
                    refAddressPK.UID = address.UID;

                    IParcelBll parcelBll = (IParcelBll)ObjectFactory.GetObject(typeof(IParcelBll));
                    ParcelModel[] parcels = parcelBll.GetParcelListByRefAddressPK(ConfigManager.AgencyCode, refAddressPK, null);
                    DataTable parcelTable = APOUtil.BuildParcelDataTable(parcels);

                    if (parcelTable.Rows.Count > 0)
                    {
                        for (int index = 0; index < parcelTable.Rows.Count; index++)
                        {
                            string parcelNbr = parcelTable.Rows[index]["ParcelNumber"].ToString();
                            string unmaskedparcelNbr = parcelTable.Rows[index]["UnmaskedParcelNumber"].ToString();

                            if (!string.IsNullOrEmpty(parcelNbr))
                            {
                                parcelInfo.parcelModel = new ParcelModel();
                                parcelInfo.parcelModel.parcelNumber = parcelNbr;
                                parcelInfo.parcelModel.unmaskedParcelNumber = unmaskedparcelNbr;
                                parcelInfo.parcelModel.sourceSeqNumber = StringUtil.ToInt(parcelTable.Rows[index]["ParcelSequenceNumber"].ToString());
                                parcelInfo.parcelModel.duplicatedAPOKeys = parcelTable.Rows[index]["ParcelAPOKeys"] as DuplicatedAPOKeyModel[];
                                parcelInfo.parcelModel.UID = parcelTable.Rows[index]["ParcelUID"].ToString();

                                parcelInfo.RAddressModel = new RefAddressModel();
                                parcelInfo.RAddressModel = address;
                                parcelInfo.RAddressModel.parcelNumber = parcelNbr;
                                parcelInfos.Add(parcelInfo);
                            }
                        }
                    }
                    else
                    {
                        parcelInfo.RAddressModel = address;
                        parcelInfos.Add(parcelInfo);
                    }
                }

                gisModel.AddressInfoModels = parcelInfos.ToArray();
            }

            UpdateAPOQueryInfo();

            GISUtil.SetPostUrl(this.Page, gisModel);
            mapAddress.ACAGISModel = gisModel;
        }

        /// <summary>
        /// GridView download event method.
        /// </summary>
        /// <param name="sender">An object that contains the event sender.</param>
        /// <param name="e">A GridViewDownloadEventArgs object containing the event data.</param>
        protected void RefAddress_GridViewDownload(object sender, GridViewDownloadEventArgs e)
        {
            //Bind RefAddress Data Source.
            BindDataSource(RefAddressDataSource);

            // execute the download event that setting the exported content.
            if (GridViewDownloadAll != null)
            {
                GridViewDownloadAll(sender, e);
            }
        }

        /// <summary>
        /// Handles the PageIndexChanged event.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected void RefAddress_PageIndexChanged(object sender, EventArgs e)
        {
            if (mapAddress.Opened)
            {
                mapAddress.ShowMap();
                mapPanel.Update();
            }
        }

        /// <summary>
        /// fire page index changing event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void RefAddress_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            //Bind RefAddress Data Source.
            BindDataSource(RefAddressDataSource);

            if (PageIndexChanging != null)
            {
                PageIndexChanging(sender, e);
            }
        }

        /// <summary>
        /// fire sorted event.
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void RefAddress_GridViewSort(object sender, AccelaGridViewSortEventArgs e)
        {
            //Bind RefAddress Data Source.
            BindDataSource(RefAddressDataSource, false);

            if (GridViewSort != null)
            {
                GridViewSort(sender, e);
            }
        }

        #endregion Protected Methods

        #region Private Methods

        /// <summary>
        /// Initializes Address list
        /// </summary>
        private void InitGISMapForAddressList()
        {
            if (IsShowMap && StandardChoiceUtil.IsShowMap4ShowObject(this.ModuleName))
            {
                dgvRefAddressLookUpList.AutoGenerateCheckBoxColumn = true;
                dgvRefAddressLookUpList.CheckBoxColumnIndex = 0;
                mapAddress.ShowGISButton();
            }
            else
            {
                dgvRefAddressLookUpList.AutoGenerateCheckBoxColumn = false;
                mapAddress.HideGISButton();
            }
        }

        /// <summary>
        /// Navigate To APO detail page.
        /// </summary>
        /// <param name="apoRow">Row Index</param>
        private void RedirectToAddressDetail(DataRow apoRow)
        {
            // navigate to Address detail page
            DuplicatedAPOKeyModel[] apoKeys = apoRow["AddressAPOKeys"] as DuplicatedAPOKeyModel[];
            List<string> sourceNumbers = new List<string>();

            if (apoKeys != null)
            {
                sourceNumbers.AddRange(from key in apoKeys where key != null select key.sourceNumber);
            }

            string addressId = apoRow["AddressID"] != null ? apoRow["AddressID"].ToString() : string.Empty;
            string addressUid = apoRow["AddressUid"] != null ? apoRow["AddressUid"].ToString() : string.Empty;
            string addressSequence = apoRow["AddressSequenceNumber"] != null ? apoRow["AddressSequenceNumber"].ToString() : string.Empty;
            string url = string.Empty;
            string urlParam = string.Format(
                "&{0}={1}&{2}={3}&{4}={5}",
                ACAConstant.REQUEST_PARMETER_REFADDRESS_ID,
                UrlEncode(addressId),
                ACAConstant.REQUEST_PARMETER_ADDRESS_SEQUENCE,
                UrlEncode(addressSequence),
                UrlConstant.AgencyCode,
                Page.Request[UrlConstant.AgencyCode]);

            url = string.Format(
                "APO/AddressDetail.aspx?{0}={1}&" + UrlConstant.HIDERHEADER + "={2}&{3}={4}",
                ACAConstant.REQUEST_PARMETER_REFADDRESS_UID,
                UrlEncode(addressUid),
                Page.Request["HideHeader"],
                ACAConstant.REQUEST_PARMETER_APO_SOURCE_NUMBERS,
                UrlEncode(string.Join(",", sourceNumbers)));

            string redirectUrl = FileUtil.AppendApplicationRoot(url + urlParam);
            Response.Redirect(redirectUrl);
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
        /// Updates the APO query info.
        /// </summary>
        private void UpdateAPOQueryInfo()
        {
            ApoQueryInfo apoQueryInfo = (ApoQueryInfo)Session[SessionConstant.SESSION_APO_QUERY];

            if (apoQueryInfo != null)
            {
                apoQueryInfo.IsFromSearchPage = true;
                apoQueryInfo.APOAddressPageIndex = dgvRefAddressLookUpList.PageIndex;

                if (!string.IsNullOrEmpty(dgvRefAddressLookUpList.GridViewSortExpression))
                {
                    apoQueryInfo.APOAddressPageSort = dgvRefAddressLookUpList.GridViewSortExpression + " " + dgvRefAddressLookUpList.GridViewSortDirection;
                }

                Session[SessionConstant.SESSION_APO_QUERY] = apoQueryInfo;
            }
        }

        #endregion Private Methods
    }
}
