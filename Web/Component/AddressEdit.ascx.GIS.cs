#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: AddressEdit.ascx.GIS.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2010-2014
 *
 *  Description:
 *
 *  Notes:
 *      $Id: AddressEdit.ascx.cs 181867 2010-09-30 08:06:18Z ACHIEVO\daly.zeng $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using System;
using System.Collections.Generic;
using System.Data;
using System.Web.UI;

using Accela.ACA.BLL.APO;
using Accela.ACA.Common;
using Accela.ACA.Common.Util;
using Accela.ACA.Web.Common;
using Accela.ACA.WSProxy;

namespace Accela.ACA.Web.Component
{
    /// <summary>
    /// Address section control
    /// </summary>
    public partial class AddressEdit
    {
        #region Properties

        /// <summary>
        /// Gets or sets a value indicating whether address list is from map.
        /// </summary>
        private bool IsFromMap
        {
            get
            {
                if (ViewState["IsFromMap"] != null)
                {
                    return (bool)ViewState["IsFromMap"];
                }

                return false;
            }

            set
            {
                ViewState["IsFromMap"] = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether create cap from map or not
        /// </summary>
        private bool IsCreateCapFromGIS
        {
            get
            {
                return Convert.ToBoolean(ViewState["IsCreateCapFromGIS"]);
            }

            set
            {
                ViewState["IsCreateCapFromGIS"] = value;
            }
        }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Click GIS button event handler.
        /// </summary>
        /// <param name="sender">An object that contains the event sender.</param>
        /// <param name="e">A System.EventArgs object containing the event data.</param>
        protected void MapAddress_ShowOnMap(object sender, EventArgs e)
        {
            string serviceProviderCode = ConfigManager.AgencyCode;
            RefAddressModel addressModel = GetRefAddressModel();
            ACAGISModel model = GISUtil.CreateACAGISModel();
            model.ModuleName = ModuleName;
            model.Context = mapAddress.AGISContext;
            if (!IsWorkLocationPage)
            {
                model.IsHideSendAddress = !IsEditable;
            }

            GISUtil.SetPostUrl(Page, model);

            if (AppSession.User.IsAnonymous)
            {
                model.UserGroups.Add(GISUserGroup.Anonymous.ToString());
            }
            else
            {
                model.UserGroups.Add(GISUserGroup.Register.ToString());
            }

            if (APOUtil.IsEmpty(addressModel))
            {
                mapAddress.ACAGISModel = model;
            }
            else
            {
                IAPOBll apoBll = ObjectFactory.GetObject<IAPOBll>();
                SearchResultModel result = apoBll.GetParcelInfoByAddress(serviceProviderCode, addressModel, null, true);
                ParcelInfoModel[] dt = ObjectConvertUtil.ConvertObjectArray2EntityArray<ParcelInfoModel>(result.resultList);

                if (dt != null && dt.Length > 0)
                {
                    List<ParcelInfoModel> parcelInfos = new List<ParcelInfoModel>();

                    foreach (ParcelInfoModel parcelInfo in dt)
                    {
                        if (parcelInfo.RAddressModel != null)
                        {
                            parcelInfo.RAddressModel.country = StandardChoiceUtil.GetCountryByKey(parcelInfo.RAddressModel.countryCode);
                        }

                        parcelInfos.Add(parcelInfo);
                    }

                    model.AddressInfoModels = parcelInfos.ToArray();
                }
                else
                {
                    List<RefAddressModel> refAddressList = new List<RefAddressModel>();
                    refAddressList.Add(addressModel);
                    model.RefAddressModels = refAddressList.ToArray();
                }

                mapAddress.ACAGISModel = model;
            }
        }

        /// <summary>
        /// This method retrieve all address information from Request.form collection. This request is sent from GIS viewer.
        /// </summary>
        private void InitialiteFromGIS()
        {
            if (Request.Form["__EVENTTARGET"].Replace("$", "_") == btnPostback.ClientID)
            {
                imgErrorIcon.Visible = false;
                ucConditon.HideCondition();
                GridViewDataSource = null;

                bool isCloseMap = true;
                IsFromMap = true;
                IsCreateCapFromGIS = false;
                string xml = Request.Form["__EVENTARGUMENT"];
                ACAGISModel model = SerializationUtil.XmlDeserialize(xml, typeof(ACAGISModel)) as ACAGISModel;
               
                if (string.Equals(model.CommandName, "Send_Address", StringComparison.CurrentCultureIgnoreCase))
                {
                    RefAddressModel refAddressModel = null;

                    if (model.RefAddressModels != null && model.RefAddressModels.Length > 0)
                    {
                        refAddressModel = model.RefAddressModels[0];
                        refAddressModel.auditStatus = ACAConstant.VALID_STATUS;

                        if (IsWorkLocationPage)
                        {
                            LoadAddressByGISSendAddress(refAddressModel);
                        }
                        else if (IsEditable)
                        {
                            // Save search conditions
                            APOSessionParameterModel sessionParameter = GetAPOSessionParameter();
                            sessionParameter.SearchCriterias = model;
                            AppSession.SetAPOSessionParameter(sessionParameter);

                            // Open the search list page
                            OpenSearchResultPage();
                        }
                    }
                    else
                    {
                        isCloseMap = false;
                    }
                }
                else if (string.Equals(model.CommandName, ACAConstant.AGIS_COMMAND_SEND_FEATURES, StringComparison.CurrentCultureIgnoreCase))
                {
                    if ((model.GisObjects != null && model.GisObjects.Length > 0) || (model.ParcelModels != null && model.ParcelModels.Length > 0))
                    {
                        if (IsWorkLocationPage)
                        {
                            isCloseMap = LoadAddressByGISSendFeatures(model);
                        }
                        else if (IsEditable)
                        {
                            // Save search conditions
                            APOSessionParameterModel sessionParameter = GetAPOSessionParameter();
                            sessionParameter.SearchCriterias = model;
                            AppSession.SetAPOSessionParameter(sessionParameter);

                            // Open the search list page
                            OpenSearchResultPage();
                        }
                    }
                    else
                    {
                        isCloseMap = false;
                    }
                }

                //close map
                if (isCloseMap)
                {
                    mapAddress.CloseMap(); 
                    mapPanel.Update();
                }
            }
        }

        /// <summary>
        /// Send Features from the Map.
        /// </summary>
        /// <param name="model">ACAGISModel Object</param>
        /// <returns>If need close map, return true.</returns>
        private bool LoadAddressByGISSendFeatures(ACAGISModel model)
        {
            bool isClose = true;
            ParcelModel parcelmodel = new ParcelModel();

            if (model.GisObjects != null && model.GisObjects.Length == 1)
            {
                parcelmodel.gisObjectList = model.GisObjects;
            }
            else if (model.ParcelModels != null && model.ParcelModels.Length == 1)
            {
                parcelmodel = model.ParcelModels[0];
            }

            parcelmodel.auditStatus = ACAConstant.VALID_STATUS;
            PaginationModel pageInfo = GetPaginationInfo(parcelmodel);
            QueryFormat queryFormat = PaginationUtil.GetQueryFormatModel(pageInfo);

            IAPOBll apoBll = ObjectFactory.GetObject<IAPOBll>();
            SearchResultModel result = apoBll.GetParcelInfoByParcel(ConfigManager.AgencyCode, parcelmodel, queryFormat, true);
            pageInfo.StartDBRow = result.startRow;
            ParcelInfoModel[] parcelInfos = ObjectConvertUtil.ConvertObjectArray2EntityArray<ParcelInfoModel>(result.resultList);

            if (parcelInfos != null && IsWorkLocationPage)
            {
                List<ParcelInfoModel> parcelList = new List<ParcelInfoModel>();

                foreach (ParcelInfoModel item in parcelInfos)
                {
                    if (item.RAddressModel != null)
                    {
                        parcelList.Add(item);
                    }
                }

                parcelInfos = parcelList.ToArray();
            }

            LoadAddressByGIS(null, parcelInfos);

            if (parcelInfos == null || parcelInfos.Length == 0)
            {
                isClose = false;
            }

            return isClose;
        }

        /// <summary>
        /// Initialize section when create cap by map is not null.
        /// </summary>
        private void InitialiteFromSession()
        {
            if (Session[SessionConstant.SESSION_CREATE_RECORD_BY_MAP] == null)
            {
                return;
            }

            ACAGISModel gisModel = Session[SessionConstant.SESSION_CREATE_RECORD_BY_MAP] as ACAGISModel;

            if (gisModel == null || gisModel.ModuleName != ModuleName)
            {
                return;
            }

            /*
             * create cap from map,search apolist logic in GISPostBack.aspx page,data bind in address edit page
             * so,need research data base on address list to support get more than 100 records.
             */
            if (gisModel.ParcelInfoModels != null && gisModel.ParcelInfoModels.Length > 0
                && (IsWorkLocationPage || (gisModel.RefAddressModels != null && gisModel.RefAddressModels.Length == 1)))
            {
                IsFromMap = true;
                IsCreateCapFromGIS = true;
                GridViewDataSource = null;
                GISUtil.RemoveACAGISModelFromSession(ModuleName);

                if (IsWorkLocationPage)
                {
                    LoadAddressListByGIS(0, null, gisModel);
                }
                else
                {
                    // Save search conditions
                    APOSessionParameterModel sessionParameter = GetAPOSessionParameter();
                    sessionParameter.SearchCriterias = gisModel;
                    AppSession.SetAPOSessionParameter(sessionParameter);

                    // Open the search list page
                    OpenSearchResultPage();
                }
            }
        }

        /// <summary>
        /// Get apo by gis sent feature.
        /// </summary>
        /// <param name="refAddressModel">RefAddress model</param>
        private void LoadAddressByGISSendAddress(RefAddressModel refAddressModel)
        {
            PaginationModel pageInfo = GetPaginationInfo(refAddressModel);
            QueryFormat queryFormat = PaginationUtil.GetQueryFormatModel(pageInfo);
            IAPOBll apoBll = ObjectFactory.GetObject<IAPOBll>();
            SearchResultModel result = apoBll.GetParcelInfoByAddress(ConfigManager.AgencyCode, refAddressModel, queryFormat, true);
            pageInfo.StartDBRow = result.startRow;
            ParcelInfoModel[] parcelInfos = ObjectConvertUtil.ConvertObjectArray2EntityArray<ParcelInfoModel>(result.resultList);

            LoadAddressByGIS(refAddressModel, parcelInfos);
        }

        /// <summary>
        /// Load address information by GIS.
        /// </summary>
        /// <param name="refAddressModel">RefAddress model</param>
        /// <param name="parcelInfos">ParcelInfoModel array</param>
        private void LoadAddressByGIS(RefAddressModel refAddressModel, ParcelInfoModel[] parcelInfos)
        {
            if (parcelInfos != null && parcelInfos.Length > 0)
            {
                LoadAddressList(parcelInfos, PaginationUtil.GetPageInfoByID(PageInfoID));
            }
            else if (IsEditable && refAddressModel != null)
            {
                DisplayAddress(true, CapUtil.ConvertRefAddressModel2AddressModel(refAddressModel), false, true);
            }
            else if (refAddressModel == null)
            {
                ScriptManager.RegisterClientScriptBlock(
                    Page,
                    GetType(),
                    "ShowNoRecordMessage",
                    "alert('" + GetTextByKey("aca_gis_no_record_message") + "');",
                    true);
            }
        }

        /// <summary>
        /// Load address list when create cap from gis map in Work Location.
        /// </summary>
        /// <param name="currentPageIndex">Current page index</param>
        /// <param name="sortExpression">Sort expression</param>
        /// <param name="gisModel">A ACAGISModel object to use to search</param>
        private void LoadAddressListByGIS(int currentPageIndex, string sortExpression, ACAGISModel gisModel)
        {
            GridViewDataSource = GetAddressList(currentPageIndex, sortExpression, gisModel);

            LoadAddressList(
                gisModel == null || gisModel.ParcelInfoModels == null || gisModel.ParcelInfoModels.Length == 0
                    ? null
                    : gisModel.ParcelInfoModels[0].RAddressModel);
        }

        /// <summary>
        /// Load address list by GIS sent feature in Work Location.
        /// </summary>
        /// <param name="currentPageIndex">Current page index</param>
        /// <param name="sortExpression">Sort expression</param>
        /// <param name="refAddressModel">A RefAddressModel object to use to search</param>
        private void LoadAddressListByGIS(int currentPageIndex, string sortExpression, RefAddressModel refAddressModel)
        {
            PaginationModel pageInfo = GetAddressPaginationInfo(currentPageIndex, sortExpression);
            pageInfo.SearchCriterias = new object[] { refAddressModel };
            QueryFormat queryFormat = PaginationUtil.GetQueryFormatModel(pageInfo);

            IAPOBll apoBll = ObjectFactory.GetObject<IAPOBll>();
            SearchResultModel result = apoBll.GetParcelInfoByAddress(ConfigManager.AgencyCode, refAddressModel, queryFormat, true);
            pageInfo.StartDBRow = result.startRow;
            ParcelInfoModel[] parcelInfos = ObjectConvertUtil.ConvertObjectArray2EntityArray<ParcelInfoModel>(result.resultList);

            LoadAddressList(parcelInfos, pageInfo);
        }

        /// <summary>
        /// Load address list by GIS parcel and pagination information in Work Location.
        /// </summary>
        /// <param name="parcelInfos">ParcelInfoModel array</param>
        /// <param name="pageInfo">Pagination information</param>
        private void LoadAddressList(ParcelInfoModel[] parcelInfos, PaginationModel pageInfo)
        {
            DataTable dt = APOUtil.BuildAPODataTable(parcelInfos);
            dt = PaginationUtil.MergeDataSource<DataTable>(GridViewDataSource, dt, pageInfo);
            GridViewDataSource = dt;

            if (parcelInfos != null && parcelInfos.Length > 0)
            {
                ucAddressList.ParcelInfoList = parcelInfos;
                LoadAddressList();
            }
        }

        /// <summary>
        /// Get pagination information by search criteria.
        /// </summary>
        /// <param name="searchCriteria">Search criteria</param>
        /// <returns>A PaginationModel</returns>
        private PaginationModel GetPaginationInfo(object searchCriteria)
        {
            PaginationModel pageInfo = PaginationUtil.GetPageInfoByID(PageInfoID);
            pageInfo.SortExpression = null;
            pageInfo.CurrentPageIndex = 0;
            pageInfo.SearchCriterias = new object[] { searchCriteria };
            pageInfo.CustomPageSize = ucAddressList.PageSize;

            return pageInfo;
        }

        #endregion Methods
    }
}