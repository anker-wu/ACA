#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: RecordByGISObject.aspx.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2010-2014
 *
 *  Description:
 *
 *  Notes:
 *      $Id: RecordByGISObject.aspx.cs 181867 2010-09-30 08:06:18Z ACHIEVO\daly.zeng $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

using System.Web.UI.WebControls;
using Accela.ACA.BLL.APO;
using Accela.ACA.BLL.Cap;
using Accela.ACA.Common;
using Accela.ACA.Common.Util;
using Accela.ACA.Web.Common;
using Accela.ACA.WSProxy;
using Accela.Web.Controls;

namespace Accela.ACA.Web.GIS
{
    /// <summary>
    /// the class of search result by GIS object.
    /// </summary>
    public partial class RecordByGISObject : PopupDialogBasePage
    {
        #region Property

        /// <summary>
        /// Gets the module name that only for searching cap list.
        /// Because the cap list needs get the layout settings from global level, 
        /// So not use the common "Module" parameter name in the query string to avoid the cap list to get the module level configuration.
        /// </summary>
        private string Module
        {
            get
            {
                return Request["ModuleName"];
            }
        }

        /// <summary>
        /// Gets the GIS model list.
        /// </summary>
        private List<GISObjectModel> GISModelList
        {
            get
            {
                string gisId = Request["GisId"];
                string layerId = Request["LayerId"];
                string serviceId = Request["ServiceId"];
                GISObjectModel gisObject = new GISObjectModel();
                List<GISObjectModel> gisObjects = new List<GISObjectModel>();
                gisObject.gisId = gisId;
                gisObject.layerId = layerId;
                gisObject.serviceID = serviceId;
                gisObjects.Add(gisObject);

                return gisObjects;
            }
        }

        #endregion property

        #region Methods

        /// <summary>
        /// Raises the page load event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            this.SetPageTitleKey("aca_gis_object_search_result_title");
            this.SetDialogMaxHeight("600");
            this.RefAPOAddressList1.ComponentType = (int)SearchType.Parcel;
            this.capList.InitialExport(false);

            if (!IsPostBack && !AppSession.IsAdmin)
            {
                RefAPOAddressList1.RefAPODataSource = null;
                capList.GridViewDataSource = null;
                DataTable dtAPO = GetAPOByGIS(0, null);
                DataTable dtCAP = GetCapByGIS(0, null);
                InitUI(dtAPO, dtCAP);
                BindAPOData(dtAPO);
                BindCAPData(dtCAP);
            }
            else if (AppSession.IsAdmin)
            {
                DataTable dt = APOUtil.BuildAPODataTable(null);
                this.RefAPOAddressList1.BindDataSource(dt);

                ICapBll capBll = ObjectFactory.GetObject(typeof(ICapBll)) as ICapBll;
                capList.BindDataToPermitList(new DataTable(), 0, string.Empty);
            }
        }

        /// <summary>
        /// The RefAddress index changing event.
        /// </summary>
        /// <param name="sender">The sender object.</param>
        /// <param name="e">The event arguments.</param>
        protected void RefAddress_GridViewIndexChanging(object sender, GridViewPageEventArgs e)
        {
            PaginationModel pageInfo = PaginationUtil.GetPageInfoByID(RefAPOAddressList1.ClientID);

            if (e.NewPageIndex > pageInfo.EndPage)
            {
                DataTable dt = GetAPOByGIS(e.NewPageIndex, pageInfo.SortExpression);
                BindAPOData(dt);
            }
        }

        /// <summary>
        /// PermitList GridViewSort
        /// </summary>
        /// <param name="sender">An object that contains the event sender.</param>
        /// <param name="e">A System.EventArgs object containing the event data.</param>
        protected void RefAddress_GridViewSort(object sender, AccelaGridViewSortEventArgs e)
        {
            PaginationModel pageInfo = PaginationUtil.GetPageInfoByID(RefAPOAddressList1.ClientID);
            pageInfo.SortExpression = e.GridViewSortExpression;
        }

        /// <summary>
        /// permit list grid view index change event
        /// </summary>
        /// <param name="sender">An object that contains the event sender.</param>
        /// <param name="e">A System.EventArgs object containing the event data.</param>
        protected void PermitList_GridViewIndexChanging(object sender, GridViewPageEventArgs e)
        {
            PaginationModel pageInfo = PaginationUtil.GetPageInfoByID(capList.ClientID);

            if (e.NewPageIndex > pageInfo.EndPage)
            {
                DataTable dt = GetCapByGIS(e.NewPageIndex, pageInfo.SortExpression);
                BindCAPData(dt);
            }
        }

        /// <summary>
        /// PermitList GridViewSort
        /// </summary>
        /// <param name="sender">An object that contains the event sender.</param>
        /// <param name="e">A System.EventArgs object containing the event data.</param>
        protected void PermitList_GridViewSort(object sender, AccelaGridViewSortEventArgs e)
        {
            PaginationModel pageInfo = PaginationUtil.GetPageInfoByID(capList.ClientID);
            pageInfo.SortExpression = e.GridViewSortExpression;
        }

        /// <summary>
        /// Indicating whether collection contains an simple cap model.
        /// </summary>
        /// <param name="capModel">SimpleCapModel Object.</param>
        /// <param name="list">SimpleCapModel array</param>
        /// <returns>return true if it contains.</returns>
        private bool IsContains(SimpleCapModel capModel, List<SimpleCapModel> list)
        {
            if (capModel == null || list == null)
            {
                return false;
            }

            bool isExist = false;

            foreach (SimpleCapModel item in list)
            {
                if (string.Equals(item.altID, capModel.altID))
                {
                    isExist = true;
                    break;
                }
            }

            return isExist;
        }

        /// <summary>
        /// Get APO by GIS.
        /// </summary>
        /// <param name="currentPageIndex">The current page index.</param>
        /// <param name="sortExpression">the sort expression.</param>
        /// <returns>The APO that get from GIS.</returns>
        private DataTable GetAPOByGIS(int currentPageIndex, string sortExpression)
        {
            ParcelModel parcelmodel = new ParcelModel();
            parcelmodel.gisObjectList = GISModelList.ToArray();
            PaginationModel pageInfo = PaginationUtil.GetPageInfoByID(RefAPOAddressList1.ClientID);
            pageInfo.SortExpression = sortExpression;
            pageInfo.CurrentPageIndex = currentPageIndex;
            pageInfo.CustomPageSize = RefAPOAddressList1.PageSize;
            QueryFormat queryFormat = PaginationUtil.GetQueryFormatModel(pageInfo);
            IAPOBll apoBll = ObjectFactory.GetObject(typeof(IAPOBll)) as IAPOBll;
            SearchResultModel result = apoBll.GetAPOListByParcel(ConfigManager.AgencyCode, parcelmodel, queryFormat, false);
            pageInfo.StartDBRow = result.startRow;
            DataTable dt = APOUtil.BuildAPODataTable(result.resultList);
            dt = PaginationUtil.MergeDataSource<DataTable>(RefAPOAddressList1.RefAPODataSource, dt, pageInfo);

            return dt;
        }

        /// <summary>
        /// Get CAP by GIS.
        /// </summary>
        /// <param name="currentPageIndex">The current page index.</param>
        /// <param name="sortExpression">the sort expression.</param>
        /// <returns>The CAP that get from GIS.</returns>
        private DataTable GetCapByGIS(int currentPageIndex, string sortExpression)
        {
            CapModel4WS capmodel = new CapModel4WS();
            capmodel.moduleName = Module;
            capmodel.gisObjects = GISModelList.ToArray();
            PaginationModel pageInfo = PaginationUtil.GetPageInfoByID(capList.ClientID);
            pageInfo.SortExpression = sortExpression;
            pageInfo.CurrentPageIndex = currentPageIndex;
            pageInfo.CustomPageSize = capList.PageSize;
            QueryFormat queryFormat = PaginationUtil.GetQueryFormatModel(pageInfo);
            List<string> moduleNames = null;
            if (string.IsNullOrEmpty(Module))
            {
                Dictionary<string, string> allModules = TabUtil.GetAllEnableModules(false);
                if (allModules != null && allModules.Count > 0)
                {
                    moduleNames = allModules.Keys.ToList();
                }
            }
            else
            {
                moduleNames = new List<string>();
                moduleNames.Add(Module);
                if (CapUtil.EnableCrossModuleSearch())
                {
                    IList<string> crossModules = TabUtil.GetCrossModules(Module);
                    if (crossModules.Count > 0)
                    {
                        moduleNames.AddRange(crossModules.ToArray());
                    }
                }
            }

            /*
             * Here are two request to search record:
             * 1. Search records by GIS object.
             * 2. Search records by parcel object.
             * 3. Merge records list.
             */

            //Search record by GIS object.
            ICapBll capBll = ObjectFactory.GetObject(typeof(ICapBll)) as ICapBll;
            SearchResultModel searchGisResult = capBll.GetCapList4ACA(capmodel, queryFormat, AppSession.User.UserSeqNum, null, false, moduleNames, false, null);
            SimpleCapModel[] simpleCaps = ObjectConvertUtil.ConvertObjectArray2EntityArray<SimpleCapModel>(searchGisResult.resultList);
            List<SimpleCapModel> list = new List<SimpleCapModel>();

            if (simpleCaps != null && simpleCaps.Length > 0)
            {
                list.AddRange(simpleCaps);
            }

            //Search record by parcel object.
            capmodel = new CapModel4WS();
            capmodel.moduleName = Module;
            capmodel.parcelModel = new CapParcelModel();
            capmodel.parcelModel.parcelModel = new ParcelModel();
            capmodel.parcelModel.parcelModel.gisObjectList = GISModelList.ToArray();
            SearchResultModel searchResult = capBll.GetCapList4ACA(capmodel, queryFormat, AppSession.User.UserSeqNum, null, false, moduleNames, false, null);
            SimpleCapModel[] simpleCaps1 = ObjectConvertUtil.ConvertObjectArray2EntityArray<SimpleCapModel>(searchResult.resultList);

            if (simpleCaps1 != null && simpleCaps1.Length > 0)
            {
                foreach (SimpleCapModel item in simpleCaps1)
                {
                    if (!IsContains(item, list))
                    {
                        list.Add(item);
                    }
                }
            }

            // Sort record list by creation date DESC.
            list.Sort(new Comparison<SimpleCapModel>(this.CompareSimpleCapModel));

            DataTable dt = capList.CreateDataSource(list.ToArray());
            dt = PaginationUtil.MergeDataSource<DataTable>(capList.GridViewDataSource, dt, pageInfo);

            return dt;
        }

        /// <summary>
        /// Compares the simple cap model.
        /// </summary>
        /// <param name="x">First compare record object.</param>
        /// <param name="y">Second compare record object.</param>
        /// <returns>
        /// A signed number indicating the relative values of x and y. Value Type Condition
        ///     Less than zero x is earlier than y. Zero x is the same as y. Greater
        ///     than zero x is later than y.
        /// </returns>
        private int CompareSimpleCapModel(SimpleCapModel x, SimpleCapModel y)
        {
            DateTime? fileDateX = x == null ? DateTime.MinValue : x.fileDate;
            DateTime? fileDateY = y == null ? DateTime.MinValue : y.fileDate;

            if (fileDateX != null && fileDateY != null)
            {
                return DateTime.Compare(fileDateY.Value, fileDateX.Value);
            }
            else if (fileDateX == null && fileDateY != null)
            {
                return 1;
            }
            else if (fileDateX != null && fileDateY == null)
            {
                return -1;
            }

            return 0;
        }

        /// <summary>
        /// Initial the UI.
        /// </summary>
        /// <param name="dtAPO">The APO data.</param>
        /// <param name="dtCAP">The CAP data.</param>
        private void InitUI(DataTable dtAPO, DataTable dtCAP)
        {
            if (dtAPO != null && dtAPO.Rows.Count == 1 && (dtCAP == null || dtCAP.Rows.Count == 0))
            {
                DataRow row = dtAPO.Rows[0];
                string url = string.Format(
                                          "../APO/ParcelDetail.aspx?{0}={1}&{2}={3}&{4}={5}&{6}={7}&{8}={9}&HideHeader={10}&{11}={12}",
                                          ACAConstant.REQUEST_PARMETER_PARCEL_NUMBER,
                                          HttpUtility.UrlEncode(row["ParcelNumber"].ToString()),
                                          ACAConstant.REQUEST_PARMETER_PARCEL_SEQUENCE,
                                          HttpUtility.UrlEncode(row["ParcelSequenceNumber"].ToString()),
                                          ACAConstant.REQUEST_PARMETER_REFADDRESS_ID,
                                          HttpUtility.UrlEncode(row["AddressID"].ToString()),
                                          ACAConstant.REQUEST_PARMETER_ADDRESS_SEQUENCE,
                                          HttpUtility.UrlEncode(row["AddressSequenceNumber"].ToString()),
                                          ACAConstant.REQUEST_PARMETER_PARCEL_UID,
                                          HttpUtility.UrlEncode(row["ParcelUID"].ToString()),
                                          true,
                                          UrlConstant.AgencyCode,
                                          Request[UrlConstant.AgencyCode]);

                Response.Redirect(url);
            }
            else if ((dtAPO == null || dtAPO.Rows.Count == 0) && dtCAP != null && dtCAP.Rows.Count == 1)
            {
                DataRow row = dtCAP.Rows[0];
                string capdetailUrl = string.Format(
                    "../Cap/CapDetail.aspx?Module={0}&{1}={2}&capID1={3}&capID2={4}&capID3={5}&HideHeader={6}",
                    GISModelList,
                    UrlConstant.AgencyCode,
                    HttpUtility.UrlEncode(ConfigManager.AgencyCode),
                    HttpUtility.UrlEncode(row["capID1"].ToString()),
                    HttpUtility.UrlEncode(row["capID2"].ToString()),
                    HttpUtility.UrlEncode(row["capID3"].ToString()),
                    true);
                Response.Redirect(capdetailUrl);
            }
        }

        /// <summary>
        /// Bind APO data.
        /// </summary>
        /// <param name="dtAPO">The APO data.</param>
        private void BindAPOData(DataTable dtAPO)
        {
            if (dtAPO != null && dtAPO.Rows.Count > 0)
            {
                RefAPOAddressList1.ComponentType = (int)SearchType.Parcel;
                RefAPOAddressList1.ExportFileName = "APOParcel";
                RefAPOAddressList1.BindDataSource(dtAPO, true);
            }
            else
            {
                dvAPOList.Visible = false;
            }
        }

        /// <summary>
        /// Bind CAP data.
        /// </summary>
        /// <param name="dtCAP">The CAP data.</param>
        private void BindCAPData(DataTable dtCAP)
        {
            if (dtCAP != null && dtCAP.Rows.Count > 0)
            {
                capList.BindCapList(dtCAP);
            }
            else
            {
                dvCapList.Visible = false;
            }
        }

        #endregion Methods
    }
}