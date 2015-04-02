#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: RefParcelLookUpList.aspx.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2014
 *
 *  Description:
 *
 *  Notes:
 *      $Id: RefParcelLookUpList.aspx.cs 269110 2014-07-15 08:24:39Z ACHIEVO\canon.wu $.
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
using Accela.ACA.Web.Common;
using Accela.ACA.Web.Common.Control;
using Accela.ACA.WSProxy;
using Accela.Web.Controls;

namespace Accela.ACA.Web.Component
{
    /// <summary>
    /// RefParcelLookUpList Control.
    /// </summary>
    public partial class RefParcelLookUpList : BaseUserControl
    {
        #region Fields

        /// <summary>
        /// The padding status parcel.
        /// </summary>
        private const string PENDING_PARCEL = "P";

        /// <summary>
        /// whether show map or not
        /// </summary>
        private bool _isShowMap = true;

        /// <summary>
        /// grid view row command event.
        /// </summary>
        public event CommonEventHandler ParcelSelected;

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
        public string RetieveLinkLabelKey
        {
            get
            {
                return ViewState["RetieveLinkLabelKey"] as string;
            }

            set
            {
                ViewState["RetieveLinkLabelKey"] = value;
            }
        }

        /// <summary>
        /// Gets or sets the view id for Reference Address list
        /// </summary>
        public string GViewID
        {
            get
            {
                return dgvRefParcelLookUpList.GridViewNumber;
            }

            set
            {
                dgvRefParcelLookUpList.GridViewNumber = value;
            }
        }

        /// <summary>
        /// Gets the display count.
        /// </summary>
        public string CountSummary
        {
            get
            {
                return dgvRefParcelLookUpList.CountSummary;
            }
        }

        /// <summary>
        /// Gets the page size of GridView 
        /// </summary>
        public int PageSize
        {
            get
            {
                return dgvRefParcelLookUpList.PageSize;
            }
        }

        /// <summary>
        /// Gets or sets the reference Parcel data source.
        /// </summary>
        public DataTable RefParcelDataSource
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
        /// Gets or sets a value indicating the export GridView's name 
        /// </summary>
        public string ExportFileName
        {
            get
            {
                return dgvRefParcelLookUpList.ExportFileName;
            }

            set
            {
                dgvRefParcelLookUpList.ExportFileName = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether show export link.
        /// </summary>
        public bool ShowExportLink
        {
            get
            {
                return dgvRefParcelLookUpList.ShowExportLink;
            }

            set
            {
                dgvRefParcelLookUpList.ShowExportLink = value;
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

        #endregion Properties

        #region Public Methods

        /// <summary>
        /// Close the map
        /// </summary>
        public void CloseMap()
        {
            mapParcel.CloseMap();
        }

        /// <summary>
        /// bind reference address data source
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
            InitGISMapForParcelList();

            if (dt == null || dt.Rows.Count == 0)
            {
                dgvRefParcelLookUpList.EmptyDataText = AppSession.IsAdmin ? string.Empty : GetTextByKey("per_permitList_messagel_noRecord");
                dgvRefParcelLookUpList.DataSource = null;
                dgvRefParcelLookUpList.DataBind();

                return;
            }

            RefParcelDataSource = dt;
            DataView dv = new DataView(dt);
            string gridViewSort = GetGridViewSort();

            if (!string.IsNullOrEmpty(pageSort))
            {
                dv.Sort = pageSort;
            }
            else if (!string.IsNullOrEmpty(dgvRefParcelLookUpList.GridViewSortExpression))
            {
                dv.Sort = dgvRefParcelLookUpList.GridViewSortExpression + " " + dgvRefParcelLookUpList.GridViewSortDirection;
            }
            else if (!string.IsNullOrEmpty(gridViewSort))
            {
                dv.Sort = gridViewSort;
            }

            if (resetPageIndex)
            {
                dgvRefParcelLookUpList.PageIndex = 0;
            }
            else if (pageIndex != 0)
            {
                dgvRefParcelLookUpList.PageIndex = pageIndex;
            }

            dgvRefParcelLookUpList.Visible = true;
            dgvRefParcelLookUpList.DataSource = dv.ToTable();
            dgvRefParcelLookUpList.DataBind();
        }

        /// <summary>
        /// Clear selected items.
        /// </summary>
        public void ClearSelectedItems()
        {
            dgvRefParcelLookUpList.IsClearSelectedItems = true;
        }

        /// <summary>
        /// Retrieve associated records for the selected parcel record.
        /// </summary>
        /// <param name="parcelRow">Parcel data row</param>
        public void RetrieveAssociatedData(DataRow parcelRow)
        {
            ParcelSelected(null, new CommonEventArgs(parcelRow));
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

        #endregion Public Methods

        #region Protected Methods

        /// <summary>
        /// Raises the <c>System.Web.UI.Control.Init</c> event.
        /// </summary>
        /// <param name="e">An System.EventArgs object that contains the event data.</param>
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            //if standard choice display owner section is "N" and is in daily side, set Action column hidden in Associated Parcel List.
            if (!AppSession.IsAdmin && !StandardChoiceUtil.IsDisplayOwnerSection() && GViewID == GviewID.AssociatedParcelList)
            {
                GridViewBuildHelper.SetHiddenColumn(dgvRefParcelLookUpList, new[] { "Action" });
            }

            GridViewBuildHelper.SetSimpleViewElements(dgvRefParcelLookUpList, ModuleName, false);
        }

        /// <summary>
        /// Handles the RowCommand event.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Web.UI.WebControls.GridViewCommandEventArgs"/> instance containing the event data.</param>
        protected void RefParcelList_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            //Bind RefAddress Data Source.
            BindDataSource(RefParcelDataSource);

            if (e.CommandName == "Header")
            {
                dgvRefParcelLookUpList.GridViewSortExpression = e.CommandArgument.ToString();
            }
            else
            {
                if (e.CommandName == "RetrieveData" || e.CommandName == APOShowType.ShowParcel.ToString())
                {
                    int index = int.Parse(e.CommandArgument.ToString());
                    GridViewRow row = dgvRefParcelLookUpList.Rows[index];
                    DataRow parcelRow = ((DataTable)dgvRefParcelLookUpList.DataSource).Rows[row.DataItemIndex];

                    //Update the selected pageIndex and sortExpression to session.
                    UpdateAPOQueryInfo();

                    if (e.CommandName == "RetrieveData" && ParcelSelected != null)
                    {
                        ParcelSelected(sender, new CommonEventArgs(parcelRow));
                    }
                    else
                    {
                        RedirectToParcelDetail(parcelRow);
                    }
                }
            }
        }

        /// <summary>
        /// Handles the RowDataBound event.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Web.UI.WebControls.GridViewRowEventArgs"/> instance containing the event data.</param>
        protected void RefParcelList_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                string parcelStatus = ((DataRowView)e.Row.DataItem)["ParcelStatus"].ToString();
                ((AccelaLabel)e.Row.FindControl("lblStatus")).Text = GetDisplayParcelStatus(parcelStatus);
            }
        }

        /// <summary>
        /// Handles the PageIndexChanged event.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected void RefParcelList_PageIndexChanged(object sender, EventArgs e)
        {
            if (mapParcel.Opened)
            {
                mapParcel.ShowMap();
                mapPanel.Update();
            }
        }

        /// <summary>
        /// Fire page index changing event
        /// </summary>
        /// <param name="sender">Object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void RefParcelList_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            //Bind RefAddress Data Source.
            BindDataSource(RefParcelDataSource);

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
            //Bind RefAddress Data Source.
            BindDataSource(RefParcelDataSource);

            if (GridViewSort != null)
            {
                GridViewSort(sender, e);
            }
        }

        /// <summary>
        /// Handles the ShowACAMap event of the mapParcel control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected void MapParcel_ShowACAMap(object sender, EventArgs e)
        {
            //Bind RefAddress Data Source.
            BindDataSource(RefParcelDataSource);

            ACAGISModel gisModel = GISUtil.CreateACAGISModel();
            gisModel.Context = mapParcel.AGISContext;
            gisModel.UserGroups.Add(AppSession.User.IsAnonymous ? GISUserGroup.Anonymous.ToString() : GISUserGroup.Register.ToString());
            gisModel.ModuleName = ModuleName;

            DataTable dt = null; //gv.GetSelectedData(gv.DataSource as DataTable, gv.PageIndex);
            if (dgvRefParcelLookUpList.DataSource != null)
            {
                if (dgvRefParcelLookUpList.DataSource is DataTable)
                {
                    dt = dgvRefParcelLookUpList.DataSource as DataTable;
                }
                else
                {
                    DataView dv = dgvRefParcelLookUpList.DataSource as DataView;
                    dt = dv.Table;
                }

                dt = dgvRefParcelLookUpList.GetSelectedData(dt, dgvRefParcelLookUpList.PageIndex);
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

                    ParcelModel refParcelPK = new ParcelModel();
                    refParcelPK.parcelNumber = parcelInfo.parcelModel.parcelNumber;
                    refParcelPK.sourceSeqNumber = parcelInfo.parcelModel.sourceSeqNumber;
                    refParcelPK.UID = item["ParcelUID"].ToString();

                    IAPOBll apoBll = ObjectFactory.GetObject<IAPOBll>();
                    SearchResultModel searchResult = apoBll.GetAddressListByParcelPK(ConfigManager.AgencyCode, refParcelPK, false, null);
                    DataTable addressTable = APOUtil.BuildAddressDataTable(searchResult.resultList);

                    if (addressTable.Rows.Count > 0)
                    {
                        foreach (DataRow addressRow in addressTable.Rows)
                        {
                            parcelInfo.RAddressModel = new RefAddressModel();
                            parcelInfo.RAddressModel = APOUtil.InitialRefAddressModel(addressRow, parcelNbr);
                        }

                        parcelInfos.Add(parcelInfo);
                    }
                    else
                    {
                        parcelInfos.Add(parcelInfo);
                    }
                }

                gisModel.ParcelInfoModels = parcelInfos.ToArray();
            }

            UpdateAPOQueryInfo();

            GISUtil.SetPostUrl(Page, gisModel);
            mapParcel.ACAGISModel = gisModel;
        }

        /// <summary>
        /// GridView download event method.
        /// </summary>
        /// <param name="sender">An object that contains the event sender.</param>
        /// <param name="e">A GridViewDownloadEventArgs object containing the event data.</param>
        protected void RefParcelList_GridViewDownload(object sender, GridViewDownloadEventArgs e)
        {
            //Bind RefAddress Data Source.
            BindDataSource(RefParcelDataSource);

            // execute the download event that setting the exported content.
            if (GridViewDownloadAll != null)
            {
                GridViewDownloadAll(sender, e);
            }
        }

        #endregion Protected Methods

        #region Private Methods

        /// <summary>
        /// Navigation to Parcel Detail.
        /// </summary>
        /// <param name="parcelRow">Row Index</param>
        private void RedirectToParcelDetail(DataRow parcelRow)
        {
            // navigate to APO detail page
            DuplicatedAPOKeyModel[] apoKeys = parcelRow["ParcelAPOKeys"] as DuplicatedAPOKeyModel[];
            List<string> sourceNumbers = new List<string>();

            if (apoKeys != null)
            {
                sourceNumbers.AddRange(from key in apoKeys where key != null select key.sourceNumber);
            }

            string parcelNum = parcelRow["ParcelNumber"] != null ? parcelRow["ParcelNumber"].ToString() : string.Empty;
            string parcelUid = parcelRow["ParcelUID"] != null ? parcelRow["ParcelUID"].ToString() : string.Empty;
            string parcelSequence = parcelRow["ParcelSequenceNumber"] != null ? parcelRow["ParcelSequenceNumber"].ToString() : string.Empty;
            string url = string.Empty;
            string urlParam = string.Format(
                                            "&{0}={1}&{2}={3}",
                                            ACAConstant.REQUEST_PARMETER_PARCEL_NUMBER,
                                            UrlEncode(parcelNum),
                                            UrlConstant.AgencyCode,
                                            Page.Request[UrlConstant.AgencyCode]);

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
                ScriptManager.RegisterStartupScript(refParcelUpdatePanel, refParcelUpdatePanel.GetType(), "ShowParcel", scripts, true);
                return;
            }

            string redirectUrl = FileUtil.AppendApplicationRoot(url + urlParam);
            Response.Redirect(redirectUrl);
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
                apoQueryInfo.APOParcelPageIndex = dgvRefParcelLookUpList.PageIndex;

                if (!string.IsNullOrEmpty(dgvRefParcelLookUpList.GridViewSortExpression))
                {
                    apoQueryInfo.APOParcelPageSort = dgvRefParcelLookUpList.GridViewSortExpression + " " + dgvRefParcelLookUpList.GridViewSortDirection;
                }

                Session[SessionConstant.SESSION_APO_QUERY] = apoQueryInfo;
            }
        }

        /// <summary>
        /// Gets the grid view sort.
        /// </summary>
        /// <returns>The grid view sort.</returns>
        private string GetGridViewSort()
        {
            ApoQueryInfo apoQueryInfo = (ApoQueryInfo)Session[SessionConstant.SESSION_APO_QUERY];

            if (apoQueryInfo != null && !string.IsNullOrEmpty(apoQueryInfo.APOParcelPageSort))
            {
                return apoQueryInfo.APOParcelPageSort;
            }

            return string.Empty;
        }

        /// <summary>
        /// Initializes GIS Map For Parcel List.
        /// </summary>
        private void InitGISMapForParcelList()
        {
            if (IsShowMap && StandardChoiceUtil.IsShowMap4ShowObject(ModuleName))
            {
                dgvRefParcelLookUpList.AutoGenerateCheckBoxColumn = true;
                dgvRefParcelLookUpList.CheckBoxColumnIndex = 0;
                mapParcel.ShowGISButton();
            }
            else
            {
                dgvRefParcelLookUpList.AutoGenerateCheckBoxColumn = false;
                mapParcel.HideGISButton();
            }
        }

        #endregion Private Methods
    }
}
