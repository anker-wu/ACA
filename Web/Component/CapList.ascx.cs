#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: CapDetail.aspx.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2007-2014
 *
 *  Description: Display CAP list
 *
 *  Notes:
 *      $Id: CapList.ascx.cs 278296 2014-09-01 08:35:43Z ACHIEVO\james.shi $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

using Accela.ACA.BLL.AddressBuilder;
using Accela.ACA.BLL.Cap;
using Accela.ACA.Common;
using Accela.ACA.Common.Common;
using Accela.ACA.Common.Util;
using Accela.ACA.Web.AuthorizedAgent;
using Accela.ACA.Web.Common;
using Accela.ACA.Web.Common.Control;
using Accela.ACA.WSProxy;
using Accela.Web.Controls;

namespace Accela.ACA.Web.Component
{
    /// <summary>
    /// the class for CapList.
    /// </summary>
    public partial class CapList : BaseUserControl
    {
        #region Fields

        /// <summary>
        /// Licensee is in property look up, it hasn't the module name, so give it a constant.
        /// </summary>
        private const string MODULE_NAME_FOR_NOMODULE = "NOMODULE";

        /// <summary>
        /// export file name.
        /// </summary>
        private string _exportFileName = string.Empty;

        /// <summary>
        /// is contain partial cap.
        /// </summary>
        private bool _isContainPartialCap = false;

        /// <summary>
        /// show permit address
        /// </summary>
        private bool _showPermitAddress = false;

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
        /// Gets or sets export GridView's name 
        /// </summary>
        public string ExportFileName
        {
            get
            {
                return _exportFileName;
            }

            set
            {
                _exportFileName = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether map is windowless model.
        /// </summary>
        public bool Windowless
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets a value indicating whether force login to apply permit or not.
        /// </summary>
        public bool IsForceLoginToApplyPermit
        {
            get
            {
                if (ViewState["IsForceLoginToApplyPermit"] != null)
                {
                    return (bool)ViewState["IsForceLoginToApplyPermit"];
                }
                else
                {
                    return false;
                }
            }

            set
            {
                ViewState["IsForceLoginToApplyPermit"] = value;
            }
        }

        /// <summary>
        /// Gets or sets data source
        /// </summary>
        public DataTable GridViewDataSource
        {
            get
            {
                string moduleName = string.IsNullOrEmpty(ModuleName) ? MODULE_NAME_FOR_NOMODULE : ModuleName;

                if (ViewState[moduleName] != null)
                {
                    return (DataTable)ViewState[moduleName];
                }
                else
                {
                    return null;
                }
            }

            set
            {
                string moduleName = string.IsNullOrEmpty(ModuleName) ? MODULE_NAME_FOR_NOMODULE : ModuleName;

                ViewState[moduleName] = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether select CAPs whether contain partial CAP.
        /// </summary>
        public bool IsContainPartialCap
        {
            get
            {
                return _isContainPartialCap;
            }

            set
            {
                _isContainPartialCap = value;
            }
        }

        /// <summary>
        /// Gets the page size of GridView 
        /// </summary>
        public int PageSize
        {
            get
            {
                return this.gdvPermitList.PageSize;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether show PermitAddress column
        /// </summary>
        public bool ShowPermitAddress
        {
            get
            {
                return _showPermitAddress;
            }

            set
            {
                _showPermitAddress = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether be used for my licensee or not.
        /// </summary>
        public bool IsForLicensee
        {
            get
            {
                if (ViewState["IsForLicensee"] != null)
                {
                    return Convert.ToBoolean(ViewState["IsForLicensee"]);
                }
                else
                {
                    return false;
                }
            }

            set
            {
                ViewState["IsForLicensee"] = value;
            }
        }

        /// <summary>
        /// Gets or sets the view id for cap list
        /// </summary>
        public string GViewID
        {
            get
            {
                return gdvPermitList.GridViewNumber;
            }

            set
            {
                gdvPermitList.GridViewNumber = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether need to display Map on page.
        /// </summary>
        public bool IsHideMap
        {
            get;
            set;
        }

        /// <summary>
        /// Gets the display count
        /// </summary>
        public string CountSummary
        {
            get
            {
                return gdvPermitList.CountSummary;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether display checkbox column or not.
        /// </summary>
        public bool AutoGenerateCheckBoxColumn
        {
            get
            {
                return gdvPermitList.AutoGenerateCheckBoxColumn;
            }

            set
            {
                gdvPermitList.AutoGenerateCheckBoxColumn = value;
            }
        }

        /// <summary>
        /// Gets the selected Items Field Client ID
        /// </summary>
        public string SelectedItemsFieldClientID
        {
            get
            {
                return gdvPermitList.GetSelectedItemsFieldClientID();
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether be used for my collection or not.
        /// </summary>
        protected bool IsForCollection
        {
            get
            {
                if (ViewState["IsForCollection"] != null)
                {
                    return Convert.ToBoolean(ViewState["IsForCollection"]);
                }
                else
                {
                    return false;
                }
            }

            set
            {
                ViewState["IsForCollection"] = value;
            }
        }

        /// <summary>
        /// Gets or sets the moduleName of base user control.
        /// </summary>
        protected override string ModuleName
        {
            get
            {
                if (ViewState["SectionModuleName"] != null)
                {
                    return ViewState["SectionModuleName"].ToString();
                }
                else
                {
                    return base.ModuleName;
                }
            }

            set
            {
                ViewState["SectionModuleName"] = value;
            }
        }
        
        #endregion Properties

        #region Public Methods

        /// <summary>
        /// get address from SimpleCapModel4WS
        /// </summary>
        /// <param name="capMode">cap model for aca.</param>
        /// <returns>string for permit address.</returns>
        public static string GetPermitAddressForMap(SimpleCapModel capMode)
        {
            string result = string.Empty;

            if (capMode != null && capMode.addressModel != null)
            {
                IAddressBuilderBll addressBuilderBll = ObjectFactory.GetObject(typeof(IAddressBuilderBll)) as IAddressBuilderBll;
                result = addressBuilderBll.Build4Map(capMode.addressModel);
            }

            return result;
        }

        /// <summary>
        /// get address for display in permit list
        /// </summary>
        /// <param name="capMode">cap model for aca.</param>
        /// <returns>string for permit address.</returns>
        public static string GetPermitAddress(SimpleCapModel capMode)
        {
            string result = string.Empty;

            if (capMode != null && capMode.addressModel != null)
            {
                IAddressBuilderBll addressBuilderBll = ObjectFactory.GetObject(typeof(IAddressBuilderBll)) as IAddressBuilderBll;
                result = addressBuilderBll.BuildAddressByFormatType(capMode.addressModel, null, AddressFormatType.SHORT_ADDRESS_NO_FORMAT);
            }

            return result;
        }

        /// <summary>
        /// Bind CapList.
        /// </summary>
        /// <param name="capList">SimpleCapModel4WS list.</param>
        public void BindCapList(DataTable capList)
        {
            BindCapList(capList, 0, gdvPermitList.GridViewSortExpression);
        }

        /// <summary>
        /// Bind cap list data source
        /// </summary>
        /// <param name="capList">Cap model list</param>
        /// <param name="pageIndex">the page index.</param>
        /// <param name="sort">string for sort</param>
        public void BindCapList(DataTable capList, int pageIndex, string sort)
        {
            DataTable dataTable = capList;

            //if the first parameter is null, get SimpleCapModel4WS from GridViewDataSource
            if (dataTable == null)
            {
                if (GridViewDataSource == null)
                {
                    dataTable = CreateTable();
                }
                else
                {
                    dataTable = GridViewDataSource;
                }
            }

            if (sort == null)
            {
                // make sure the Renew License link is always to be showed at top of list when the list is loaded.
                sort = "RenewalSort DESC";
            }

            string[] s = sort.Trim().Split(' ');

            if (s.Length == 2)
            {
                gdvPermitList.GridViewSortExpression = s[0];
                gdvPermitList.GridViewSortDirection = s[1];
                DataView dataView = new DataView(dataTable);
                dataView.Sort = sort;
                dataTable = dataView.ToTable();
            }

            GridViewDataSource = dataTable;
            BindDataSource(GridViewDataSource, pageIndex, true);
        }

        /// <summary>
        /// Bind CAPs for module of my collection.
        /// </summary>
        /// <param name="collectionModuleName">my collection name</param>
        /// <param name="dtCAPs">dataTable for CAP</param>
        /// <param name="pageIndex">the page index</param>
        /// <param name="sort">string for sort.</param>
        /// <param name="isForCollection">Is for collection.</param>
        public void BindCapList(string collectionModuleName, DataTable dtCAPs, int pageIndex, string sort, bool isForCollection)
        {
            ModuleName = collectionModuleName;
            IsForCollection = isForCollection;

            BindDataToPermitList(dtCAPs, pageIndex, sort);

            //According to configuration in ACA admin to show or hide columns of GridView.
            GridViewBuildHelper.HideGridViewColumns(gdvPermitList, collectionModuleName);
        }

        /// <summary>
        /// Bind data to permit list.
        /// </summary>
        /// <param name="data">Permit data</param>
        /// <param name="pageIndex">the page index.</param>
        /// <param name="sort">string for sort</param>
        public void BindDataToPermitList(DataTable data, int pageIndex, string sort)
        {
            if (!data.Columns.Contains("RenewalSort"))
            {
                data = AddRenewalColumn(data);
            }

            if (sort == null)
            {
                sort = "RenewalSort DESC";
            }

            string[] s = sort.Trim().Split(' ');
            DataView dataView = data.DefaultView;

            if (s.Length == 2)
            {
                gdvPermitList.GridViewSortExpression = s[0];
                gdvPermitList.GridViewSortDirection = s[1];
                dataView.Sort = sort;
            }

            GridViewDataSource = dataView.ToTable();
            BindDataSource(dataView.ToTable(), pageIndex, true);
        }

        /// <summary>
        /// Clear selected items.
        /// </summary>
        public void ClearSelectedItems()
        {
            this.gdvPermitList.IsClearSelectedItems = true;
        }

        /// <summary>
        /// Get selected alt id of selected caps
        /// </summary>
        /// <returns>alt id array</returns>
        public string[] GetSelectedAltIDs()
        {
            DataTable dtSelectedItems = this.gdvPermitList.GetSelectedData(GridViewDataSource);

            if (dtSelectedItems == null || dtSelectedItems.Rows.Count == 0)
            {
                return null;
            }

            string[] altIDs = new string[dtSelectedItems.Rows.Count];

            for (int i = 0; i < dtSelectedItems.Rows.Count; i++)
            {
                altIDs[i] = dtSelectedItems.Rows[i]["agencyCode"].ToString() 
                            + ACAConstant.SPLIT_CHAR4 + dtSelectedItems.Rows[i]["capID1"].ToString() 
                            + ACAConstant.SPLIT_CHAR4 + dtSelectedItems.Rows[i]["capID2"].ToString()
                            + ACAConstant.SPLIT_CHAR4 + dtSelectedItems.Rows[i]["capID3"].ToString();
            }

            return altIDs;
        }

        /// <summary>
        /// Get selected items and set to SimpleCapModel4WS model.
        /// </summary>
        /// <returns>SimpleCapModel4WS array</returns>
        public SimpleCapModel[] GetSelectedCAPs()
        {
            DataTable dtSelectedItems = this.gdvPermitList.GetSelectedData(GridViewDataSource);

            if (dtSelectedItems == null || dtSelectedItems.Rows.Count == 0)
            {
                return null;
            }

            int selectedCAPsCount = dtSelectedItems.Rows.Count;

            DataTable dtRealCAPs = FilterPartialCAPs(dtSelectedItems);

            int realCAPsCount = dtRealCAPs.Rows.Count;

            if (realCAPsCount < selectedCAPsCount)
            {
                _isContainPartialCap = true;
            }

            SimpleCapModel[] simpleCapModelList = new SimpleCapModel[realCAPsCount];
            SimpleCapModel simpleCapModel = null;

            for (int i = 0; i < realCAPsCount; i++)
            {
                simpleCapModel = new SimpleCapModel();
                CapIDModel capIdModel = new CapIDModel();

                capIdModel.serviceProviderCode = dtRealCAPs.Rows[i]["agencyCode"].ToString();
                capIdModel.ID1 = dtRealCAPs.Rows[i]["capID1"].ToString();
                capIdModel.ID2 = dtRealCAPs.Rows[i]["capID2"].ToString();
                capIdModel.ID3 = dtRealCAPs.Rows[i]["capID3"].ToString();

                simpleCapModel.capID = capIdModel;
                simpleCapModelList[i] = simpleCapModel;
            }

            return simpleCapModelList;
        }

        /// <summary>
        /// Get selected items and set to shopping cart item model or to clone record. include real cap and partial cap.
        /// </summary>
        /// <returns>Data table of Selected CAPS.</returns>
        public DataTable GetSelectedCAPItems()
        {
            DataTable dtSelectedItems = this.gdvPermitList.GetSelectedData(GridViewDataSource);

            return dtSelectedItems;
        }        
        
        /// <summary>
        /// according the standard choice to display or not.
        /// </summary>
        /// <param name="showExportLink">show export link</param>
        public void InitialExport(bool showExportLink)
        {
            gdvPermitList.ShowExportLink = showExportLink;
        }

        /// <summary>
        /// Close the map
        /// </summary>
        public void CloseMap()
        {
            mapCapList.CloseMap();
        }

        /// <summary>
        /// create data table by given cap list
        /// </summary>
        /// <param name="capList">Cap model list</param>
        /// <returns>data source for UI</returns>
        public DataTable CreateDataSource(object[] capList)
        {
            DataTable dt = CreateTable();

            if (capList == null || capList.Length == 0)
            {
                return dt;
            }

            int index = 0;

            foreach (object obj in capList)
            {
                SimpleCapModel cap = obj as SimpleCapModel;

                if (cap == null)
                {
                    continue;
                }

                string permitNumber = cap.altID != null ? cap.altID : string.Empty;
                string permitType = CAPHelper.GetAliasOrCapTypeLabel(cap);
                string status = I18nStringUtil.GetString(cap.resCapStatus, cap.capStatus);
                string capClass = cap.capClass != null ? cap.capClass : string.Empty;
                bool hasNoPaidFees = cap.noPaidFeeFlag;
                string renewalStatus = cap.renewalStatus != null ? cap.renewalStatus : string.Empty;
                string description = string.Empty;

                if (cap.capDetailModel != null)
                {
                    description = cap.capDetailModel.shortNotes != null ? cap.capDetailModel.shortNotes : string.Empty;
                }

                DataRow dr = dt.NewRow();
                dr["CapIndex"] = index;
                dr["PermitNumber"] = permitNumber;
                dr["PermitType"] = permitType;
                dr["Description"] = description;
                dr["Status"] = status;
                dr["CapClass"] = capClass;
                dr["capID1"] = cap.capID.ID1;
                dr["capID2"] = cap.capID.ID2;
                dr["capID3"] = cap.capID.ID3;
                dr["ProjectName"] = cap.specialText;
                dr["HasNoPaidFees"] = hasNoPaidFees;
                dr["RenewalStatus"] = renewalStatus;
                dr["hasPrivilegeToHandleCap"] = cap.hasPrivilegeToHandleCap;
                dr["EnglishTradeName"] = cap.englishTradeName;
                dr["ArabicTradeName"] = cap.arabicTradeName;
                dr["relatedTradeLicense"] = cap.relatedTradeLic;
                dr["filterName"] = cap.capType.filterName;
                dr["isAmendable"] = cap.capType.isAmendable;
                dr["licenseType"] = cap.licenseType;
                dr["isTNExpired"] = cap.isTNExpired;
                dr["ModuleName"] = cap.moduleName;
                dr["Address"] = GetPermitAddress(cap);
                dr["PaymentStatus"] = cap.paymentStatus;

                if (cap.capID != null && !string.IsNullOrEmpty(cap.capID.serviceProviderCode))
                {
                    dr["agencyCode"] = cap.capID.serviceProviderCode;
                }
                else
                {
                    dr["agencyCode"] = ConfigManager.AgencyCode;
                }

                if (_showPermitAddress)
                {
                    dr["PermitAddress"] = GetPermitAddressForMap(cap);
                }

                dr["Date"] = cap.fileDate == null ? DBNull.Value : (object)cap.fileDate;
                dr["expirationDate"] = cap.expDate == null ? DBNull.Value : (object)cap.expDate;
                dr["RenewalSort"] = renewalStatus == ACAConstant.RENEWAL_INCOMPLETE ? 1 : 0;
                dr["CreatedBy"] = cap.createdByDisplay;
                dr["RelatedRecords"] = cap.relatedRecordsCount;
                dr["AppStatusGroup"] = cap.statusGroupCode;
                dr["AppStatus"] = cap.capStatus;

                dt.Rows.Add(dr);
                index++;
            }

            return dt;
        }

        /// <summary>
        /// Check the permission to redirect to record detail
        /// </summary>
        /// <param name="row">Data Row</param>
        /// <returns>Permission on Record Detail Flag</returns>
        public bool CheckPermissionToRecordDetail(DataRow row)
        {
            if (!CapUtil.IsPartialCap(row["CapClass"].ToString())
                && ((IsForLicensee && Convert.ToBoolean(row["hasPrivilegeToHandleCap"])) || !IsForLicensee || (GViewID == GviewID.GISCapList) || (GViewID == GviewID.FacebookMySharedList))
                && !ACAConstant.PAYMENT_STATUS_PAID.Equals(row["PaymentStatus"]))
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Construct permit number URL.
        /// </summary>
        /// <param name="row">DataRow type</param>
        /// <returns>URL string</returns>
        public string ConstructRecordDetailUrl(DataRow row)
        {
            var moduleName = row["ModuleName"].ToString();
            var capIDModel = new CapIDModel4WS
            {
                id1 = row["capID1"].ToString(),
                id2 = row["capID2"].ToString(),
                id3 = row["capID3"].ToString(),
                serviceProviderCode = row["agencyCode"].ToString()
            };

            var url = string.Format(
                "~/Cap/CapDetail.aspx?Module={0}&TabName={0}&capID1={1}&capID2={2}&capID3={3}&{4}={5}&{6}={7}",
                ScriptFilter.AntiXssUrlEncode(moduleName),
                capIDModel.id1,
                capIDModel.id2,
                capIDModel.id3,
                UrlConstant.AgencyCode,
                Server.UrlEncode(capIDModel.serviceProviderCode),
                ACAConstant.IS_TO_SHOW_INSPECTION,
                Request.QueryString[ACAConstant.IS_TO_SHOW_INSPECTION]);

            return url;
        }

        #endregion Public Methods

        #region Protected Methods

        /// <summary>
        /// <c>OnInit</c> event method
        /// </summary>
        /// <param name="e">A System.EventArgs object containing the event data.</param>
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            mapCapList.Visible = !IsHideMap && StandardChoiceUtil.IsShowMap4ShowObject(this.ModuleName);
            HideColumnsLinksForSpecialCase();
            DialogUtil.RegisterScriptForDialog(this.Page);

            InitCheckBox();
        }

        /// <summary>
        /// Override OnPreRender event to change the label key.
        /// </summary>
        /// <param name="e">Event argument.</param>
        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);

            ChangeLabelKey();
        }

        /// <summary>
        /// Page index changed event.
        /// </summary>
        /// <param name="sender">An object that contains the event sender.</param>
        /// <param name="e">A System.EventArgs object containing the event data.</param>
        protected void PermitList_PageIndexChanged(object sender, EventArgs e)
        {
            if (mapCapList.Opened)
            {
                mapCapList.ShowMap();
                mapPanel.Update();
            }
        }

        /// <summary>
        /// ShowOnMap event handler
        /// </summary>
        /// <param name="sender">An object that contains the event sender.</param>
        /// <param name="e">A System.EventArgs object containing the event data.</param>
        protected void MapCapList_ShowACAMap(object sender, EventArgs e)
        {
            ACAGISModel gisModel = GISUtil.CreateACAGISModel();
            gisModel.Context = mapCapList.AGISContext;
            gisModel.UserGroups.Add(AppSession.User.IsAnonymous ? GISUserGroup.Anonymous.ToString() : GISUserGroup.Register.ToString());
            GISUtil.SetPostUrl(this.Page, gisModel);
            gisModel.ModuleName = ModuleName;
            gisModel.Windowless = Windowless;
            DataTable dt = null;
            if (gdvPermitList.DataSource is DataTable)
            {
                dt = gdvPermitList.GetSelectedData(gdvPermitList.DataSource as DataTable, gdvPermitList.PageIndex);
            }
            else
            {
                DataView dv = gdvPermitList.DataSource as DataView;
                dt = dv.Table;
                dt = gdvPermitList.GetSelectedData(dt, gdvPermitList.PageIndex);
            }

            if (dt != null && dt.Rows.Count > 0)
            {
                List<CapIDModel> capIds = new List<CapIDModel>();
                foreach (DataRow item in dt.Rows)
                {
                    string paymentStatus = item["PaymentStatus"] as string;

                    if (!ACAConstant.PAYMENT_STATUS_PAID.Equals(paymentStatus, StringComparison.InvariantCultureIgnoreCase))
                    {
                        CapIDModel capId = new CapIDModel()
                        {
                            ID1 = (string)item["capID1"],
                            ID2 = (string)item["capID2"],
                            ID3 = (string)item["capID3"],
                            serviceProviderCode = (string)item["agencyCode"]
                        };

                        capIds.Add(capId);
                    }
                }

                if (capIds.Count == 0)
                {
                    gisModel.HasPermission4Show = false;
                    mapCapList.ACAGISModel = gisModel;

                    MessageUtil.ShowMessageByControl(Page, MessageType.Notice, GetTextByKey("aca_caplist_msg_showmapdenied"));
                    return;
                }

                gisModel.CapIDModels = capIds.ToArray();
            }

            mapCapList.ACAGISModel = gisModel;            
        }

        /// <summary>
        /// Raises the page load event
        /// </summary>
        /// <param name="sender">An object that contains the event sender.</param>
        /// <param name="e">A System.EventArgs object containing the event data.</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack && lblNeedReBind.Value.CompareTo("false") != 0)
            {
                BindDataSource(GridViewDataSource, 0, false);
            }
            else
            {
                lblNeedReBind.Value = string.Empty;
            }

            InitCartButton();
            InitCollectionButton();
            InitCloneRecordButton();

            //set the CSV file name.Mypermit20090303.csv
            gdvPermitList.ExportFileName = _exportFileName;

            if (!IsPostBack)
            {
                ClearSessions();
            }

            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "lblNeedReBind", "function SetReBindValue(clientID){ var NeedReBind=document.getElementById(clientID);NeedReBind.value=\"false\";}", true);
        }

        /// <summary>
        /// fire sorted event.
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void PermitList_GridViewSort(object sender, AccelaGridViewSortEventArgs e)
        {
            GridViewDataSource.DefaultView.Sort = e.GridViewSortExpression;
            GridViewDataSource = GridViewDataSource.DefaultView.ToTable();

            if (GridViewSort != null)
            {
                GridViewSort(sender, e);
            }            
        }

        /// <summary>
        /// fire page index changing event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void PermitList_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            if (PageIndexChanging != null)
            {
                PageIndexChanging(sender, e);
            }
        }

        /// <summary>
        /// GridView PermitList RowCommand
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">GridViewCommandEventArgs e</param>
        protected void PermitList_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Action" || e.CommandName == "RenewalAction")
            {
                string url = CapUtil.GetActionCommandUrl(Page, e.CommandArgument);

                if (string.IsNullOrEmpty(url))
                {
                    BindDataSource(GridViewDataSource, 0, false);
                    return;
                }

                if (url.StartsWith("~/", StringComparison.InvariantCultureIgnoreCase))
                {
                    url = ResolveUrl(url);
                }

                if (GViewID == GviewID.GISCapList)
                {
                    ScriptManager.RegisterClientScriptBlock(updatePanel, updatePanel.GetType(), "GotoUrl", "parent.location.href=' " + url + "'", true);
                }
                else
                {
                    Response.Redirect(url);
                }
            }
        }

        /// <summary>
        /// GridView PermitList RowDataBound
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">GridViewCommandEventArgs e</param>
        protected void PermitList_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                DataRowView rowView = (DataRowView)e.Row.DataItem;

                Label lblStatus = (Label)e.Row.FindControl("lblStatus");
                LinkButton btnStatus = (LinkButton)e.Row.FindControl("btnStatus");
                Label lblPermitNum = (Label)e.Row.FindControl("lblPermitNumber");
                HyperLink hlPermitNumber = (HyperLink)e.Row.FindControl("hlPermitNumber");

                string clickFunc = string.Format("SetReBindValue('{0}')", lblNeedReBind.ClientID);
                string capClass = rowView["CapClass"].ToString();
                string moduleName = rowView["ModuleName"].ToString();
                string agencyCode = rowView["agencyCode"].ToString();
                bool hasPrivilegeToHandleCap = (bool)rowView["hasPrivilegeToHandleCap"];
                bool isPartialCap = CapUtil.IsPartialCap(capClass);

                CapIDModel4WS capIDModel = new CapIDModel4WS();
                capIDModel.id1 = rowView["capID1"].ToString();
                capIDModel.id2 = rowView["capID2"].ToString();
                capIDModel.id3 = rowView["capID3"].ToString(); 
                capIDModel.serviceProviderCode = agencyCode;

                // set the cap related records' number to display
                SetRelatedRecordsText(e.Row, capIDModel, moduleName);

                object paymentStatus = rowView["PaymentStatus"];
                bool hasNopaidFee = (bool)rowView["HasNoPaidFees"];

                if (ACAConstant.PAYMENT_STATUS_PAID.Equals(paymentStatus))
                {
                    /*
                    * filter the permission for the Completed Paid Record
                    * Partial cap: Must be a registered user and has privilege and the cap creation function does not been disabled by customize permission.
                    * Real cap: Must enable PayFeeDue and the payment function does not been disabled by customize permission.
                    */
                    if ((isPartialCap && (AppSession.User.IsAnonymous || !hasPrivilegeToHandleCap || !FunctionTable.IsEnableCreateApplication())) ||
                        (!isPartialCap && (!hasNopaidFee || StandardChoiceUtil.IsRemovePayFee(capIDModel.serviceProviderCode) || !FunctionTable.IsEnableMakePayment())))
                    {
                        lblStatus.Visible = true;
                        btnStatus.Visible = false;
                    }
                    else
                    {
                        btnStatus.CommandArgument = GetRowButtonCommandArgument(capIDModel, ACAConstant.ACTION_COMPLETE_PAID);
                        btnStatus.Text = GetTextByKey("aca_caplist_label_completepaidrecord");

                        lblStatus.Visible = false;
                        btnStatus.Visible = true;
                    }

                    lblPermitNum.Visible = true;
                    hlPermitNumber.Visible = false;
                }
                else if (!isPartialCap && ((IsForLicensee && hasPrivilegeToHandleCap) || !IsForLicensee || (GViewID == GviewID.GISCapList) || (GViewID == GviewID.FacebookMySharedList)))
                {
                    lblStatus.Visible = true;
                    btnStatus.Visible = false;

                    // set permit number button
                    lblPermitNum.Visible = false;
                    SetPermitNumberLink(hlPermitNumber, rowView.Row);

                    // set pay fees button
                    if (hasNopaidFee && !StandardChoiceUtil.IsRemovePayFee(capIDModel.serviceProviderCode)
                        && FunctionTable.IsEnableMakePayment())
                    {
                        LinkButton btnFeeStatus = (LinkButton)e.Row.FindControl("btnFeeStatus");

                        btnFeeStatus.Visible = true;
                        btnFeeStatus.CommandArgument = GetRowButtonCommandArgument(capIDModel, "PayFees");
                        btnFeeStatus.Text = GetTextByKey("per_permitList_label_payFees");
                        btnFeeStatus.OnClientClick = clickFunc;
                    }

                    // set renewal status button and label status
                    var btnRenewalStatus = (LinkButton)e.Row.FindControl("btnRenewalStatus");

                    if (CapUtil.InitRenewalButton(btnRenewalStatus, rowView.Row))
                    {
                        var renewalStatus = rowView["RenewalStatus"].ToString();
                        btnRenewalStatus.OnClientClick = clickFunc;
                        SetLabelStatusText(lblStatus, renewalStatus);
                    }

                    // set request trade license button
                    var btnRequestTradeLic = (LinkButton)e.Row.FindControl("btnRequestTradeLic");

                    if (CapUtil.InitTradeLicenseButton(btnRequestTradeLic, rowView.Row))
                    {
                        btnRequestTradeLic.OnClientClick = clickFunc;
                    }

                    // set amendment button
                    bool isAmendable = rowView["isAmendable"].ToString() == ACAConstant.COMMON_Y;
                    if (!AppSession.User.IsAnonymous && isAmendable
                        && FunctionTable.IsEnableCreateAmendment())
                    {
                        LinkButton btnAmend = (LinkButton)e.Row.FindControl("btnCreateAmendment");

                        btnAmend.Visible = true;
                        btnAmend.CommandArgument = GetRowButtonCommandArgument(capIDModel, "Amend");
                        string amendmentKey = (AppSession.User.IsAuthorizedAgent || AppSession.User.IsAgentClerk) ? "aca_authagent_licenselist_label_amendment" : "per_permitList_label_amendment";
                        btnAmend.Text = GetTextByKey(amendmentKey);
                        btnAmend.OnClientClick = clickFunc;
                    }

                    Label lblRenewalDetail = (Label)e.Row.FindControl("lblRenewalDetail");
                    HtmlAnchor lnkRenewalDetail = (HtmlAnchor)e.Row.FindControl("lnkRenewalDetail");

                    if (CapUtil.InitRenewalDetailButton(lblRenewalDetail, lnkRenewalDetail, rowView.Row))
                    {
                        lblStatus.Visible = false;
                    }
                }
                else
                {
                    //don't allow view permit detail if Partial Cap is incomplete
                    lblStatus.Visible = false;
                    btnStatus.Visible = true;
                    lblPermitNum.Visible = true;
                    hlPermitNumber.Visible = false;

                    bool isPending4Payment = capClass.Equals(ACAConstant.CAP_STATUS_PENDING, StringComparison.InvariantCulture);

                    if (AppSession.User.IsAnonymous || !hasPrivilegeToHandleCap ||
                        (isPending4Payment && !FunctionTable.IsEnableMakePayment()) ||
                        (!isPending4Payment && !FunctionTable.IsEnableCreateApplication()))
                    {
                        btnStatus.Visible = false;
                    }
                    else
                    {
                        if (isPending4Payment)
                        {
                            // pass 6 parameter: capID1,2,3 activate flag, agency code, customID.
                            string customID = rowView["PermitNumber"].ToString();

                            btnStatus.CommandArgument = GetRowButtonCommandArgument(capIDModel, ACAConstant.CAP_STATUS_PENDING, customID);
                            btnStatus.Text = GetTextByKey("per_permitlist_label_pendingpayment");
                        }
                        else
                        {
                            btnStatus.CommandArgument = GetRowButtonCommandArgument(capIDModel, string.Empty);
                            btnStatus.Text = GetTextByKey("per_permitList_label_resumeApplication");
                        }

                        btnStatus.OnClientClick = clickFunc;
                    }
                }

                Label lblModuleName = (Label)e.Row.FindControl("lblModuleName");
                if (lblModuleName != null)
                {
                    string displayModuleName = I18nUtil.GetResModuleName(moduleName);

                    if (string.IsNullOrEmpty(displayModuleName))
                    {
                        displayModuleName = moduleName;
                    }

                    lblModuleName.Text = displayModuleName;
                }
            }
            else if (e.Row.RowType == DataControlRowType.Header)
            {
                SetModuleName2HeaderLabel(e);
            }
        }
        
        #endregion Protected Methods

        #region Private Methods

        /// <summary>
        /// GridView PermitList download event method.
        /// </summary>
        /// <param name="sender">An object that contains the event sender.</param>
        /// <param name="e">A GridViewDownloadEventArgs object containing the event data.</param>
        protected void PermitList_GridViewDownload(object sender, GridViewDownloadEventArgs e)
        {
            // execute the download event that setting the exported content.
            if (GridViewDownloadAll != null)
            {
                GridViewDownloadAll(sender, e);
            }
        }

        /// <summary>
        /// Gets the row button CommandArgument.
        /// </summary>
        /// <param name="capIDModel">The CapIDModel.</param>
        /// <param name="actionFlag">The action flag.</param>
        /// <param name="externalFlag">The external flag, default value is null.</param>
        /// <returns>Return the row button CommandArgument</returns>
        private static string GetRowButtonCommandArgument(CapIDModel4WS capIDModel, string actionFlag, string externalFlag = null)
        {
            string result = string.Format(
                                    "{1}{0}{2}{0}{3}{0}{4}{0}{5}",
                                   ACAConstant.COMMA,
                                   capIDModel.id1,
                                   capIDModel.id2,
                                   capIDModel.id3,
                                   actionFlag,
                                   capIDModel.serviceProviderCode);

            if (!string.IsNullOrEmpty(externalFlag))
            {
                result += ACAConstant.COMMA + externalFlag;
            }

            return result;
        }

        /// <summary>
        /// create blank structure for cap list
        /// </summary>
        /// <returns>blank table for cap list</returns>
        private static DataTable CreateTable()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add(new DataColumn("CapIndex", typeof(string)));
            dt.Columns.Add(new DataColumn("Date", typeof(DateTime)));
            dt.Columns.Add(new DataColumn("PermitNumber", typeof(string)));
            dt.Columns.Add(new DataColumn("PermitType", typeof(string)));
            dt.Columns.Add(new DataColumn("Description", typeof(string)));
            dt.Columns.Add(new DataColumn("Status", typeof(string)));
            dt.Columns.Add(new DataColumn("CapClass", typeof(string)));
            dt.Columns.Add(new DataColumn("capID1", typeof(string)));
            dt.Columns.Add(new DataColumn("capID2", typeof(string)));
            dt.Columns.Add(new DataColumn("capID3", typeof(string)));
            dt.Columns.Add(new DataColumn("ProjectName", typeof(string)));
            dt.Columns.Add(new DataColumn("HasNoPaidFees", typeof(bool)));
            dt.Columns.Add(new DataColumn("RenewalStatus", typeof(string)));
            dt.Columns.Add(new DataColumn("expirationDate", typeof(DateTime)));
            dt.Columns.Add(new DataColumn("PermitAddress", typeof(string)));
            dt.Columns.Add(new DataColumn("RenewalSort", typeof(int)));
            dt.Columns.Add(new DataColumn("hasPrivilegeToHandleCap", typeof(bool)));
            dt.Columns.Add(new DataColumn("agencyCode", typeof(string)));
            dt.Columns.Add(new DataColumn("EnglishTradeName", typeof(string)));
            dt.Columns.Add(new DataColumn("ArabicTradeName", typeof(string)));
            dt.Columns.Add(new DataColumn("relatedTradeLicense", typeof(string)));
            dt.Columns.Add(new DataColumn("filterName", typeof(string)));
            dt.Columns.Add(new DataColumn("isAmendable", typeof(string)));
            dt.Columns.Add(new DataColumn("licenseType", typeof(string)));
            dt.Columns.Add(new DataColumn("isTNExpired", typeof(bool)));
            dt.Columns.Add(new DataColumn("ModuleName", typeof(string)));
            dt.Columns.Add(new DataColumn("Address", typeof(string)));
            dt.Columns.Add(new DataColumn("CreatedBy", typeof(string)));
            dt.Columns.Add(new DataColumn("RelatedRecords", typeof(int)));
            dt.Columns.Add(new DataColumn("AppStatusGroup", typeof(string)));
            dt.Columns.Add(new DataColumn("AppStatus", typeof(string)));
            dt.Columns.Add(new DataColumn("PaymentStatus", typeof(string)));

            return dt;
        }

        /// <summary>
        /// Sets the link of the permit number.
        /// </summary>
        /// <param name="hlPermitNumber">The hlPermitNumber control.</param>
        /// <param name="rowView">Data Row</param>
        private void SetPermitNumberLink(HyperLink hlPermitNumber, DataRow rowView)
        {
            hlPermitNumber.Visible = true;

            if (GViewID == GviewID.GISCapList)
            {
                hlPermitNumber.Target = "_parent";
            }

            string url = ConstructRecordDetailUrl(rowView);

            hlPermitNumber.NavigateUrl = ResolveUrl(url);
        }

        /// <summary>
        /// Sets the cap related records' number to display.
        /// </summary>
        /// <param name="gridRow">The GridViewRow object.</param>
        /// <param name="capIDModel">The CapID model.</param>
        /// <param name="moduleName">The module name.</param>
        private void SetRelatedRecordsText(GridViewRow gridRow, CapIDModel4WS capIDModel, string moduleName)
        {
            DataRowView rowView = (DataRowView)gridRow.DataItem;
            Literal litRelatedRecords = (Literal)gridRow.FindControl("litRelatedRecords");

            if (litRelatedRecords == null || rowView.DataView.Table.Columns["RelatedRecords"] == null)
            {
                return;
            }

            int relatedRecords = (int)rowView["RelatedRecords"];
            relatedRecords = relatedRecords < 0 ? 0 : relatedRecords;

            // set the litRelatedRecords' text, the authorized agent associated cap list also need show the related cap.
            if (relatedRecords > 0 && (GviewID.PermitList.Equals(GViewID) || GviewID.AuthAgentCustomerAssociatedLicenseList.Equals(GViewID)))
            {
                Random rd = new Random();
                string lnkRelatedRecordID = Convert.ToString(rd.Next());
                byte[] args = Encoding.UTF8.GetBytes(string.Format("{1}{0}{2}{0}{3}{0}{4}{0}{5}", ACAConstant.SPLIT_CHAR, capIDModel.id1, capIDModel.id2, capIDModel.id3, capIDModel.serviceProviderCode, moduleName));
                string url = string.Format(ResolveUrl("~/Cap/RelatedRecords.aspx?module={0}&args={1}"), moduleName, HttpUtility.UrlEncode(Convert.ToBase64String(args)));
                
                if (gdvPermitList.GridViewNumber == GviewID.AuthAgentCustomerAssociatedLicenseList)
                {
                    AuthorizedServiceSettingModel authServiceSetting = AuthorizedAgentServiceUtil.GetAuthorizedServiceSetting();
                    url += "&" + UrlConstant.FILTER_NAME + "=" + authServiceSetting.CapTypeFilterName;
                }

                string showRelatedJs = "ACADialog.popup({url:'" + url + "',width:800, objectTarget:'" + lnkRelatedRecordID + "'});";
                litRelatedRecords.Text = string.Format("<a id='{2}' href=\"javascript:{0}\">{1}</a> ", showRelatedJs, relatedRecords, lnkRelatedRecordID);
            }
            else
            {
                litRelatedRecords.Text = relatedRecords.ToString();
            }
        }

        /// <summary>
        /// Sets the label status text.
        /// </summary>
        /// <param name="lblStatus">The label status.</param>
        /// <param name="renewalStatus">The renewal status.</param>
        private void SetLabelStatusText(Label lblStatus, string renewalStatus)
        {
            // get the label key accoding the renewal status
            string labelKey = string.Empty;

            if (ACAConstant.RENEWAL_REVIEW.Equals(renewalStatus, StringComparison.InvariantCulture))
            {
                labelKey = "per_permitList_label_renewalReviewing";
            }
            else if (ACAConstant.DEFERPAY_RENEWAL.Equals(renewalStatus, StringComparison.InvariantCulture))
            {
                labelKey = "per_permitList_label_renewal_deferpay";
            }
            
            // set the lable status' text
            if (!string.IsNullOrEmpty(labelKey))
            {
                if (!string.IsNullOrEmpty(lblStatus.Text) && lblStatus.Text.Trim() != string.Empty)
                {
                    lblStatus.Text += "<BR/>";
                }

                lblStatus.Text += GetTextByKey(labelKey);
            }
        }

        /// <summary>
        /// control shopping cart link display.
        /// </summary>
        private void InitCartButton()
        {
            if (!AppSession.User.IsAnonymous && StandardChoiceUtil.IsEnableShoppingCart() && !IsForLicensee &&
                gdvPermitList.GridViewNumber != GviewID.AuthAgentCustomerAssociatedLicenseList)
            {
                gdvPermitList.ShowCartLink = true;
            }
            else
            {
                gdvPermitList.ShowCartLink = false;
            }
        }

        /// <summary>
        /// control collection link display.
        /// </summary>
        private void InitCollectionButton()
        {
            if (!AppSession.User.IsAnonymous && !IsForLicensee && !IsForCollection &&
                gdvPermitList.GridViewNumber != GviewID.AuthAgentCustomerAssociatedLicenseList)
            {
                gdvPermitList.ShowAdd2CollectionLink = true;
            }
            else
            {
                gdvPermitList.ShowAdd2CollectionLink = false;
            }
        }

        /// <summary>
        /// control clone record link display.
        /// </summary>
        private void InitCloneRecordButton()
        {
            // if cap list in show record list, hide the clone link, too.
            if (CloneRecordUtil.IsDisplayCloneButton(null, null, this.ModuleName, false) && !IsForceLoginToApplyPermit && GViewID != GviewID.GISCapList &&
                gdvPermitList.GridViewNumber != GviewID.AuthAgentCustomerAssociatedLicenseList)
            {
                gdvPermitList.ShowCloneRecordLink = true;
            }
            else
            {
                gdvPermitList.ShowCloneRecordLink = false;
            }
        }

        /// <summary>
        /// Add the RenewalSort column to the data table and set the value(0/1) to RenewalSort column for sorting.
        /// </summary>
        /// <param name="data">the data table need to add the new column</param>
        /// <returns>the new data table with new RenewalSort column.</returns>
        private DataTable AddRenewalColumn(DataTable data)
        {
            data.Columns.Add(new DataColumn("RenewalSort", typeof(int)));

            foreach (DataRow dr in data.Rows)
            {
                dr["RenewalSort"] = dr["RenewalStatus"].ToString() == ACAConstant.RENEWAL_INCOMPLETE ? 1 : 0;
            }

            return data;
        }

        /// <summary>
        /// Bind cap list data source
        /// </summary>
        /// <param name="dataTable">data table source</param>
        /// <param name="pageIndex">the page index.</param>
        /// <param name="reset">reset grid view setting</param>
        private void BindDataSource(DataTable dataTable, int pageIndex, bool reset)
        {
            GridViewBuildHelper.SetSimpleViewElements(gdvPermitList, ModuleName, false);
            gdvPermitList.DataSource = dataTable;

            if (reset)
            {
                gdvPermitList.PageIndex = pageIndex;
            }
            
            gdvPermitList.EmptyDataText = AppSession.IsAdmin ? string.Empty : GetTextByKey("per_permitList_messagel_noRecord");
            gdvPermitList.DataBind();
            gdvPermitList.Attributes.Add("PageCount", gdvPermitList.PageCount.ToString());
        }

        /// <summary>
        /// Filter partial CAP from selected CAPs.
        /// </summary>
        /// <param name="dtSelectedCAPs">DataTable for Selected CAPs</param>
        /// <returns>DataTable for CAP.</returns>
        private DataTable FilterPartialCAPs(DataTable dtSelectedCAPs)
        {
            DataTable dtRealCAPs = dtSelectedCAPs.Clone();

            int selectedCAPsCount = dtSelectedCAPs.Rows.Count;

            for (int i = 0; i < selectedCAPsCount; i++)
            {
                string capClass = dtSelectedCAPs.Rows[i]["CapClass"].ToString();

                if (string.IsNullOrEmpty(capClass) || capClass == ACAConstant.COMPLETED)
                {
                    dtRealCAPs.ImportRow(dtSelectedCAPs.Rows[i]);
                }
            }

            return dtRealCAPs;
        }

        /// <summary>
        /// Re-assign module name for GirdView's header.
        /// </summary>
        /// <param name="e">EventArgs e</param>
        private void SetModuleName2HeaderLabel(GridViewRowEventArgs e)
        {
            GridViewHeaderLabel lnkDateHeader = e.Row.FindControl("lnkDateHeader") as GridViewHeaderLabel;
            lnkDateHeader.ModuleName = ModuleName;

            GridViewHeaderLabel lnkPermitNumberHeader = e.Row.FindControl("lnkPermitNumberHeader") as GridViewHeaderLabel;
            lnkPermitNumberHeader.ModuleName = ModuleName;

            GridViewHeaderLabel lnkPermitTypeHeader = e.Row.FindControl("lnkPermitTypeHeader") as GridViewHeaderLabel;
            lnkPermitTypeHeader.ModuleName = ModuleName;

            GridViewHeaderLabel lnkDescHeader = e.Row.FindControl("lnkDescHeader") as GridViewHeaderLabel;
            lnkDescHeader.ModuleName = ModuleName;

            GridViewHeaderLabel lnkPermitSearchProjectNameHeader = e.Row.FindControl("lnkPermitSearchProjectNameHeader") as GridViewHeaderLabel;
            lnkPermitSearchProjectNameHeader.ModuleName = ModuleName;

            GridViewHeaderLabel lnkEnglishNameHeader = e.Row.FindControl("lnkEnglishNameHeader") as GridViewHeaderLabel;
            lnkEnglishNameHeader.ModuleName = ModuleName;

            GridViewHeaderLabel lnkArabicNameHeader = e.Row.FindControl("lnkArabicNameHeader") as GridViewHeaderLabel;
            lnkArabicNameHeader.ModuleName = ModuleName;

            GridViewHeaderLabel lnkExpirationDateHeader = e.Row.FindControl("lnkExpirationDateHeader") as GridViewHeaderLabel;
            lnkExpirationDateHeader.ModuleName = ModuleName;

            GridViewHeaderLabel lnkAgencyHeader = e.Row.FindControl("lnkAgencyHeader") as GridViewHeaderLabel;
            lnkAgencyHeader.ModuleName = ModuleName;

            GridViewHeaderLabel lnkStatusHeader = e.Row.FindControl("lnkStatusHeader") as GridViewHeaderLabel;
            lnkStatusHeader.ModuleName = ModuleName;

            AccelaLabel lnkActionsHeader = e.Row.FindControl("lnkActionsHeader") as AccelaLabel;
            lnkActionsHeader.ModuleName = ModuleName;
        }

        /// <summary>
        /// Clear Sessions.
        /// </summary>
        private void ClearSessions()
        {
            Session[SessionConstant.APO_SESSION_PARCELMODEL] = null;
        }

        /// <summary>
        /// Hide the Columns or Links for the special case.
        /// </summary>
        private void HideColumnsLinksForSpecialCase()
        {
            //Hide Action/Related Records column in licensee detail page,display it in cap home and my collection.
            if (GviewID.LicenseDetailCapList.Equals(GViewID))
            {
                //Related Records Column
                gdvPermitList.Columns[11].Visible = false;

                //Create By Column
                gdvPermitList.Columns[12].Visible = false;

                //Action Column
                gdvPermitList.Columns[14].Visible = false;
            }
        }

        /// <summary>
        /// Display or hide check box column
        /// </summary>
        private void InitCheckBox()
        {
            if ((!gdvPermitList.ShowAdd2CollectionLink && !gdvPermitList.ShowCartLink && !gdvPermitList.ShowCloneRecordLink && IsHideMap) ||
                gdvPermitList.GridViewNumber == GviewID.AuthAgentCustomerAssociatedLicenseList)
            {
                gdvPermitList.AutoGenerateCheckBoxColumn = false;
            }
            else
            {
                gdvPermitList.AutoGenerateCheckBoxColumn = true;
            }
        }

        /// <summary>
        /// Change the label key
        /// </summary>
        private void ChangeLabelKey()
        {
            if (gdvPermitList.GridViewNumber == GviewID.AuthAgentCustomerAssociatedLicenseList)
            {
                GridViewRow headerRow = GridViewBuildHelper.GetHeaderRow(gdvPermitList);

                ((IAccelaNonInputControl)headerRow.FindControl("lnkActionsHeader")).LabelKey = "aca_authagent_licenselist_label_action";
                ((IAccelaNonInputControl)headerRow.FindControl("lnkStatusHeader")).LabelKey = "aca_authagent_licenselist_label_status";
                ((IAccelaNonInputControl)headerRow.FindControl("lnkCreatedByHeader")).LabelKey = "aca_authagent_licenselist_label_createdby";
                ((IAccelaNonInputControl)headerRow.FindControl("lnkDescHeader")).LabelKey = "aca_authagent_licenselist_label_description";
                ((IAccelaNonInputControl)headerRow.FindControl("lnkModuleHeader")).LabelKey = "aca_authagent_licenselist_label_modulename";
                ((IAccelaNonInputControl)headerRow.FindControl("lnkDateHeader")).LabelKey = "aca_authagent_licenselist_label_opendate";
                ((IAccelaNonInputControl)headerRow.FindControl("lnkAddressHeader")).LabelKey = "aca_authagent_licenselist_label_permitaddress";
                ((IAccelaNonInputControl)headerRow.FindControl("lnkArabicNameHeader")).LabelKey = "aca_authagent_licenselist_label_permitarabictradename";
                ((IAccelaNonInputControl)headerRow.FindControl("lnkEnglishNameHeader")).LabelKey = "aca_authagent_licenselist_label_permitenglishtradename";
                ((IAccelaNonInputControl)headerRow.FindControl("lnkPermitNumberHeader")).LabelKey = "aca_authagent_licenselist_label_permitnumber";
                ((IAccelaNonInputControl)headerRow.FindControl("lnkPermitSearchProjectNameHeader")).LabelKey = "aca_authagent_licenselist_label_permitsearchprojectname";
                ((IAccelaNonInputControl)headerRow.FindControl("lnkPermitTypeHeader")).LabelKey = "aca_authagent_licenselist_label_permittype";
                ((IAccelaNonInputControl)headerRow.FindControl("lnkRelatedRecordsHeader")).LabelKey = "aca_authagent_licenselist_label_relatedrecords";
                ((IAccelaNonInputControl)headerRow.FindControl("lnkAgencyHeader")).LabelKey = "aca_authagent_licenselist_label_agencycode";
                ((IAccelaNonInputControl)headerRow.FindControl("lnkExpirationDateHeader")).LabelKey = "aca_authagent_licenselist_label_expirationdate";
            }
        }

        #endregion Private Methods
    }
}
