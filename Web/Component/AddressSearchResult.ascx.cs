#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: AddressSearchResult.ascx.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2014
 *
 *  Description:
 *
 *  Notes:
 *      $Id: AddressSearchResult.ascx.cs 260158 2014-06-23 07:53:41Z ACHIEVO\james.shi $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using System;
using System.Data;
using System.Web.UI.WebControls;

using Accela.ACA.BLL.APO;
using Accela.ACA.BLL.Common;
using Accela.ACA.Common;
using Accela.ACA.Common.Util;
using Accela.ACA.Web.Common;
using Accela.ACA.WSProxy;
using Accela.Web.Controls;

namespace Accela.ACA.Web.Component
{
    /// <summary>
    /// the class for address search result in Spear Form.
    /// </summary>
    public partial class AddressSearchResult : APOSearchResultBase
    {
        #region Fields

        /// <summary>
        /// Reference address model with key value.
        /// Include refAddressID, refAddressUID, sourceSequenceNumber.
        /// </summary>
        private RefAddressModel _refAddressPK;

        /// <summary>
        /// A RefAddressModel include condition information.
        /// </summary>
        private RefAddressModel _refAddressModelWithCondition;

        #endregion Fields

        #region Properties

        /// <summary>
        /// Gets selected address.
        /// </summary>
        public AddressModel SelectedAddress
        {
            get
            {
                return ucAddressList.SelectedAddress;
            }
        }

        /// <summary>
        /// Gets selected parcel.
        /// </summary>
        public ParcelModel SelectedParcel
        {
            get
            {
                ParcelModel parcel = ucParcelList.SelectedParcelPK;

                ucParcelList.GetDuplicatedAPOKeys(parcel);

                return parcel;
            }
        }

        /// <summary>
        /// Gets selected owner.
        /// </summary>
        public OwnerModel SelectedOwner
        {
            get
            {
                OwnerModel owner = ucOwnerList.SelectedOwnerKey;

                ucOwnerList.GetDuplicatedAPOKeys(owner);

                return owner;
            }
        }

        /// <summary>
        /// Gets the condition validated result.
        /// </summary>
        public ConditionResult ConditionResult
        {
            get
            {
                return ucConditon.ConditionResult;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether there is only one record at most in address/parcel/owner list.
        /// </summary>
        public bool OnlyOneAtMost { get; set; }

        /// <summary>
        /// Gets the RefAddressModel with condition.
        /// </summary>
        public RefAddressModel RefAddressModelWithCondition
        {
            get
            {
                return _refAddressModelWithCondition;
            }
        }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Get a RefAddressModel from GIS
        /// </summary>
        /// <returns>A RefAddressModel</returns>
        public RefAddressModel GetGISRefAddress()
        {
            if (IsFromMap)
            {
                ACAGISModel gisModel = (ACAGISModel)APOSessionParameter.SearchCriterias;

                if (gisModel.RefAddressModels != null && gisModel.RefAddressModels.Length > 0)
                {
                    return gisModel.RefAddressModels[0];
                }
            }

            return null;
        }

        /// <summary>
        /// Load Address detail and associated Parcel/Owner List when the page is loaded.
        /// </summary>
        public void LoadAPOSearchResult()
        {
            if (AppSession.IsAdmin)
            {
                divAddressAssociates.Visible = true;
                divOwnerList.Visible = true;

                ucAddressList.BindDataSource(APOUtil.BuildAddressDataTable(null));
                ucParcelList.BindDataSource(APOUtil.BuildParcelDataTable(null));
                ucOwnerList.BindDataSource(APOUtil.BuildOwnerDataTable(null));
            }
            else
            {
                if (IsFromMap)
                {
                    LoadAddressListByGIS(0, null, (ACAGISModel)APOSessionParameter.SearchCriterias);
                }
                else
                {
                    try
                    {
                        LoadAddressList(0, null);
                    }
                    catch (ACAException ex)
                    {
                        APOSessionParameter.ErrorMessage = ex.Message;
                        AppSession.SetAPOSessionParameter(APOSessionParameter);
                    }
                }
            }
        }

        /// <summary>
        /// Show all condition message of APO
        /// </summary>
        /// <param name="address">An AddressModel</param>
        /// <returns>true or false. false if condition is lock, otherwise, return true.</returns>
        public bool ShowCondition(AddressModel address)
        {
            ServiceModel[] services = AppSession.GetSelectedServicesFromSession();

            // if save and resume and confirm page, don't need to show condition info
            if ((address.refAddressId != null || !string.IsNullOrEmpty(address.UID)) && services != null)
            {
                OwnerModel owner = new OwnerModel();

                if (ExternalOwnerForSuperAgency.l1OwnerNumber.HasValue)
                {
                    owner.ownerNumber = long.Parse(ExternalOwnerForSuperAgency.l1OwnerNumber.Value.ToString());
                }

                owner.UID = ExternalOwnerForSuperAgency.UID;
                owner.duplicatedAPOKeys = ExternalOwnerForSuperAgency.duplicatedAPOKeys;

                int? sourceNumber = null;

                if (AppSession.SelectedParcelInfo != null && AppSession.SelectedParcelInfo.RAddressModel != null)
                {
                    if (AppSession.SelectedParcelInfo.RAddressModel.sourceNumber != null)
                    {
                        sourceNumber = AppSession.SelectedParcelInfo.RAddressModel.sourceNumber;
                    }
                }
                else
                {
                    // Get APO source number 
                    string servProvCode = address.serviceProviderCode;

                    if (!string.IsNullOrEmpty(servProvCode))
                    {
                        IServiceProviderBll serviceProviderBll = ObjectFactory.GetObject<IServiceProviderBll>();
                        ServiceProviderModel servProviderModel = serviceProviderBll.GetServiceProviderByPK(servProvCode, AppSession.User.PublicUserId);

                        if (servProviderModel != null)
                        {
                            sourceNumber = (int)servProviderModel.sourceNumber;
                        }
                    }
                }

                return ShowCondition(address, ExternalParcelForSuperAgency, owner, sourceNumber);
            }

            return true;
        }

        /// <summary>
        /// Show all condition message of APO at the bottom
        /// </summary>
        /// <param name="address">An AddressModel</param>
        /// <param name="parcel">A ParcelModel</param>
        /// <param name="owner">An OwnerModel</param>
        /// <returns>true or false. false if condition is lock, otherwise, return true.</returns>
        public bool ShowCondition(AddressModel address, ParcelModel parcel, OwnerModel owner)
        {
            _refAddressPK = ucAddressList.SelectedRefAddressPK;

            return ShowCondition(address, parcel, owner, _refAddressPK.sourceNumber);
        }

        /// <summary>
        /// Raises the <c>System.Web.UI.Control.Init</c> event.
        /// </summary>
        /// <param name="e">An System.EventArgs object that contains the event data.</param>
        protected override void OnInit(EventArgs e)
        {
            ucAddressList.IsValidate = IsValidate;

            if (!StandardChoiceUtil.IsDisplayOwnerSection())
            {
                // If no display owner section, no need to postback when select parcel.
                ucParcelList.AutoPostBackOnSelect = false;
            }
        }

        /// <summary>
        /// Show associated parcel and owner list after selecting an address.
        /// </summary>
        /// <param name="sender">An object that contains the event sender.</param>
        /// <param name="e">An System.EventArgs object that contains the event data.</param>
        protected void Address_Select(object sender, EventArgs e)
        {
            divConditon.Visible = false;
            ucConditon.HideCondition();

            LoadAddressAssociates();
        }

        /// <summary>
        /// Show associated address and owner list after selecting a parcel.
        /// </summary>
        /// <param name="sender">An object that contains the event sender.</param>
        /// <param name="e">An System.EventArgs object that contains the event data.</param>
        protected void Parcel_Select(object sender, EventArgs e)
        {
            divConditon.Visible = false;
            ucConditon.HideCondition();

            LoadParcelAssociates();
        }

        /// <summary>
        /// Changing address list page index from Spear Form search.
        /// </summary>
        /// <param name="sender">An object that contains the event sender.</param>
        /// <param name="e">An GridViewCommandEventArgs object that contains the event data.</param>
        protected void Address_GridViewIndexChanging(object sender, GridViewPageEventArgs e)
        {
            PaginationModel pageInfo = PaginationUtil.GetPageInfoByID(ucAddressList.ClientID);

            if (e.NewPageIndex > pageInfo.EndPage)
            {
                if (IsFromMap)
                {
                    LoadAddressListByGIS(e.NewPageIndex, pageInfo.SortExpression, (ACAGISModel)pageInfo.SearchCriterias[0]);
                }
                else
                {
                    LoadAddressList(e.NewPageIndex, pageInfo.SortExpression);
                }
            }
        }

        /// <summary>
        /// Sort address list from Spear Form search.
        /// </summary>
        /// <param name="sender">An object that contains the event sender.</param>
        /// <param name="e">A System.EventArgs object containing the event data.</param>
        protected void Address_GridViewSort(object sender, AccelaGridViewSortEventArgs e)
        {
            PaginationModel pageInfo = PaginationUtil.GetPageInfoByID(ucAddressList.ClientID);
            pageInfo.SortExpression = e.GridViewSortExpression;
        }

        /// <summary>
        /// Changing owner list page index from Spear Form search.
        /// </summary>
        /// <param name="sender">An object that contains the event sender.</param>
        /// <param name="e">An GridViewCommandEventArgs object that contains the event data.</param>
        protected void Owner_GridViewIndexChanging(object sender, GridViewPageEventArgs e)
        {
            PaginationModel pageInfo = PaginationUtil.GetPageInfoByID(ucOwnerList.ClientID);

            if (e.NewPageIndex > pageInfo.EndPage)
            {
                LoadOwnerList(e.NewPageIndex, pageInfo.SortExpression);
            }
        }

        /// <summary>
        /// Sort owner list from Spear Form search.
        /// </summary>
        /// <param name="sender">An object that contains the event sender.</param>
        /// <param name="e">A System.EventArgs object containing the event data.</param>
        protected void Owner_GridViewSort(object sender, AccelaGridViewSortEventArgs e)
        {
            PaginationModel pageInfo = PaginationUtil.GetPageInfoByID(ucOwnerList.ClientID);
            pageInfo.SortExpression = e.GridViewSortExpression;
        }

        /// <summary>
        /// Load parcel list
        /// </summary>
        /// <param name="currentPageIndex">Current page index</param>
        /// <param name="sortExpression">Sort expression</param>
        private void LoadParcelList(int currentPageIndex, string sortExpression)
        {
            PaginationModel pageInfo = PaginationUtil.GetPageInfoByID(ucParcelList.ClientID);
            pageInfo.SortExpression = sortExpression;
            pageInfo.CurrentPageIndex = currentPageIndex;
            pageInfo.CustomPageSize = ucParcelList.PageSize;
            QueryFormat queryFormat = PaginationUtil.GetQueryFormatModel(pageInfo);
            IParcelBll parcelBll = ObjectFactory.GetObject<IParcelBll>();

            ParcelModel[] parcels = parcelBll.GetParcelListByRefAddressPK(ConfigManager.AgencyCode, _refAddressPK, queryFormat);
            DataTable dt = APOUtil.BuildParcelDataTable(parcels);
            dt = PaginationUtil.MergeDataSource<DataTable>(ucParcelList.ParcelDataSource, dt, pageInfo);

            ucParcelList.BindDataSource(dt);
        }

        /// <summary>
        /// Load owner list
        /// </summary>
        /// <param name="currentPageIndex">Current page index</param>
        /// <param name="sortExpression">Sort expression</param>
        private void LoadOwnerList(int currentPageIndex, string sortExpression)
        {
            ParcelModel parcelkey = ucParcelList.SelectedParcelPK;
            DataTable dt = null;

            if (parcelkey != null)
            {
                PaginationModel pageInfo = PaginationUtil.GetPageInfoByID(ucOwnerList.ClientID);
                pageInfo.SortExpression = sortExpression;
                pageInfo.CurrentPageIndex = currentPageIndex;
                pageInfo.CustomPageSize = ucOwnerList.PageSize;
                QueryFormat queryFormat = PaginationUtil.GetQueryFormatModel(pageInfo);

                IAPOBll apoBll = ObjectFactory.GetObject<IAPOBll>();
                SearchResultModel searchResult = apoBll.GetOwnerListByParcelPKs(ConfigManager.AgencyCode, new[] { parcelkey }, true, queryFormat);

                pageInfo.StartDBRow = searchResult.startRow;
                dt = APOUtil.BuildOwnerDataTable(searchResult.resultList);
                dt = PaginationUtil.MergeDataSource<DataTable>(ucOwnerList.OwnerDataSource, dt, pageInfo);
            }

            ucOwnerList.BindDataSource(dt);
        }

        /// <summary>
        /// Load address list by search input.
        /// </summary>
        /// <param name="currentPageIndex">Current page index</param>
        /// <param name="sortExpression">Sort expression</param>
        private void LoadAddressList(int currentPageIndex, string sortExpression)
        {
            DataTable dt = GetAddressList(currentPageIndex, sortExpression);

            LoadAddressList(dt);
        }

        /// <summary>
        /// research apo list when create cap from gis map.
        /// </summary>
        /// <param name="currentPageIndex">Current page index</param>
        /// <param name="sortExpression">Sort expression</param>
        /// <param name="gisModel">A ACAGISModel object to use to search</param>
        private void LoadAddressListByGIS(int currentPageIndex, string sortExpression, ACAGISModel gisModel)
        {
            DataTable dt = null;

            if (IsCreateCapFromGIS)
            {
                // Get address list when create cap from GIS map.
                dt = GetAddressList(currentPageIndex, sortExpression, gisModel);
            }
            else
            {
                // Get address list by GIS Send Address or Features.
                dt = GetAddressListByGIS(currentPageIndex, sortExpression, gisModel);

                if ((dt == null || dt.Rows.Count == 0)
                    && (gisModel.RefAddressModels != null && gisModel.RefAddressModels.Length > 0))
                {
                    OnlyOneAtMost = true;
                }
            }

            LoadAddressList(dt);
        }

        /// <summary>
        /// Load address list by specified data source.
        /// </summary>
        /// <param name="datasource">Data source</param>
        private void LoadAddressList(DataTable datasource)
        {
            ucAddressList.BindDataSource(datasource);

            if (datasource.Rows.Count == 1)
            {
                /*
                 * If the address search returns only one match, and this address is associated with only one parcel and one owner, 
                 * the APO information will be auto-populated into the APO sections in the page flow.
                 */
                ucAddressList.SelectRow(0);

                LoadAddressAssociates();

                if ((ucParcelList.ParcelDataSource == null || ucParcelList.ParcelDataSource.Rows.Count <= 1)
                    && (ucOwnerList.OwnerDataSource == null || ucOwnerList.OwnerDataSource.Rows.Count <= 1))
                {
                    OnlyOneAtMost = true;
                }
            }
        }

        /// <summary>
        /// Load associated parcel by selected address.
        /// </summary>
        private void LoadAddressAssociates()
        {
            divAddressAssociates.Visible = true;
            _refAddressPK = ucAddressList.SelectedRefAddressPK;
            _refAddressPK.auditStatus = ACAConstant.VALID_STATUS;

            ucParcelList.ParcelDataSource = null;
            ucParcelList.ClearSelectedItems();
            LoadParcelList(0, null);

            if (ucParcelList.ParcelDataSource != null && ucParcelList.ParcelDataSource.Rows.Count == 1)
            {
                ucParcelList.SelectRow(0);

                LoadParcelAssociates();
            }
            else if (divOwnerList.Visible)
            {
                // Hide owner list due to no selected parcel.
                divOwnerList.Visible = false;
            }
        }

        /// <summary>
        /// Load associated owner by selected parcel.
        /// </summary>
        private void LoadParcelAssociates()
        {
            //if Standard Choice not enable owner section that no display owner section.
            if (StandardChoiceUtil.IsDisplayOwnerSection())
            {
                divOwnerList.Visible = true;

                ucOwnerList.OwnerDataSource = null;
                ucOwnerList.ClearSelectedItems();
                LoadOwnerList(0, null);

                if (ucOwnerList.OwnerDataSource != null && ucOwnerList.OwnerDataSource.Rows.Count == 1)
                {
                    ucOwnerList.SelectRow(0);
                }
            }
        }

        /// <summary>
        /// Get address list when create a cap from GIS map.
        /// </summary>
        /// <param name="currentPageIndex">current PageIndex</param>
        /// <param name="sortExpression">sort Expression</param>
        /// <param name="gisModel">ACA GISModel</param>
        /// <returns>A DataTable</returns>
        private DataTable GetAddressList(int currentPageIndex, string sortExpression, ACAGISModel gisModel)
        {
            PaginationModel pageInfo = GetAddressPaginationInfo(currentPageIndex, sortExpression);
            pageInfo.SearchCriterias = new object[] { gisModel };
            RefAddressModel[] refAddressInfos = GISUtil.GetRefAddressListByGISModel(pageInfo, gisModel);
            DataTable dt = CreateDataSource(refAddressInfos);

            return PaginationUtil.MergeDataSource<DataTable>(ucAddressList.DataSource, dt, pageInfo);
        }

        /// <summary>
        /// Get address list from search form
        /// </summary>
        /// <param name="currentPageIndex">current PageIndex</param>
        /// <param name="sortExpression">sort Expression</param>
        /// <returns>A DataTable</returns>
        private DataTable GetAddressList(int currentPageIndex, string sortExpression)
        {
            RefAddressModel refAddressModel = APOSessionParameter.SearchCriterias as RefAddressModel;
            PaginationModel pageInfo = GetAddressPaginationInfo(currentPageIndex, sortExpression);
            QueryFormat queryFormat = PaginationUtil.GetQueryFormatModel(pageInfo);

            IAPOBll apoBll = ObjectFactory.GetObject<IAPOBll>();
            SearchResultModel result = apoBll.GetRefAddressList(ConfigManager.AgencyCode, refAddressModel, queryFormat);
            pageInfo.StartDBRow = result.startRow;

            return PaginationUtil.MergeDataSource<DataTable>(ucAddressList.DataSource, CreateDataSource(result.resultList), pageInfo);
        }

        /// <summary>
        /// Get address list by GIS Send Address or Features.
        /// </summary>
        /// <param name="currentPageIndex">Current page index</param>
        /// <param name="sortExpression">Sort expression</param>
        /// <param name="gisModel">A ACAGISModel object to use to search</param>
        /// <returns>A DataTable</returns>
        private DataTable GetAddressListByGIS(int currentPageIndex, string sortExpression, ACAGISModel gisModel)
        {
            PaginationModel pageInfo = GetAddressPaginationInfo(currentPageIndex, sortExpression);
            pageInfo.SearchCriterias = new object[] { gisModel };
            IAPOBll apoBll = ObjectFactory.GetObject<IAPOBll>();
            DataTable dt = null;

            if (gisModel.RefAddressModels != null && gisModel.RefAddressModels.Length > 0)
            {
                // GIS Send Address
                QueryFormat queryFormat = PaginationUtil.GetQueryFormatModel(pageInfo);
                SearchResultModel result = apoBll.GetRefAddressList(ConfigManager.AgencyCode, gisModel.RefAddressModels[0], queryFormat);
                pageInfo.StartDBRow = result.startRow;
                dt = CreateDataSource(result.resultList);
            }
            else
            {
                // GIS Send Features
                if (gisModel.GisObjects != null && gisModel.GisObjects.Length == 1)
                {
                    ParcelModel parcelModel = new ParcelModel();
                    parcelModel.auditStatus = ACAConstant.VALID_STATUS;
                    parcelModel.gisObjectList = gisModel.GisObjects;

                    QueryFormat queryFormat = PaginationUtil.GetQueryFormatModel(pageInfo);
                    SearchResultModel result = apoBll.GetRefAddressListByParcel(ConfigManager.AgencyCode, parcelModel, queryFormat);
                    pageInfo.StartDBRow = result.startRow;
                    dt = CreateDataSource(result.resultList);
                }
                else if (gisModel.ParcelModels != null && gisModel.ParcelModels.Length == 1)
                {
                    RefAddressModel[] refAddressInfos = apoBll.GetRefAddressListByParcelPK(ConfigManager.AgencyCode, gisModel.ParcelModels[0], true);
                    dt = CreateDataSource(refAddressInfos);
                }
            }

            return PaginationUtil.MergeDataSource<DataTable>(ucAddressList.DataSource, dt, pageInfo);
        }

        /// <summary>
        /// Get address's pagination information.
        /// </summary>
        /// <param name="currentPageIndex">Current page index</param>
        /// <param name="sortExpression">Sort expression</param>
        /// <returns>A PaginationModel object</returns>
        private PaginationModel GetAddressPaginationInfo(int currentPageIndex, string sortExpression)
        {
            PaginationModel pageInfo = PaginationUtil.GetPageInfoByID(ucAddressList.ClientID);
            pageInfo.SortExpression = sortExpression;
            pageInfo.CurrentPageIndex = currentPageIndex;
            pageInfo.CustomPageSize = ucAddressList.PageSize;

            return pageInfo;
        }

        /// <summary>
        /// Create the data source for address list
        /// </summary>
        /// <param name="refAddressInfos">RefAddressModel array</param>
        /// <returns>data source for address list</returns>
        private DataTable CreateDataSource(object[] refAddressInfos)
        {
            DataTable dt = APOUtil.BuildAddressDataTable(refAddressInfos, AddressFormatType.LONG_ADDRESS_NO_FORMAT);

            if (dt.Columns.Contains("FullAddress"))
            {
                DataView dv = dt.DefaultView;
                dv.Sort = "FullAddress ASC";
                dt = dv.ToTable();
            }

            return dt;
        }

        /// <summary>
        /// Show all condition message of APO at the bottom
        /// </summary>
        /// <param name="address">An AddressModel</param>
        /// <param name="parcel">A ParcelModel</param>
        /// <param name="owner">An OwnerModel</param>
        /// <param name="sourceNumber">Source number</param>
        /// <returns>true or false. false if condition is lock, otherwise, return true.</returns>
        private bool ShowCondition(AddressModel address, ParcelModel parcel, OwnerModel owner, int? sourceNumber)
        {
            // 1. Prepare the key data for Parcel Info Model
            RefAddressModel refAddressModel = new RefAddressModel();
            ParcelModel parcelModel = new ParcelModel();
            OwnerModel refOwnerModel = new OwnerModel();

            if (address != null)
            {
                refAddressModel.refAddressId = address.refAddressId;
                refAddressModel.UID = address.UID;
                refAddressModel.sourceNumber = sourceNumber;
                refAddressModel.duplicatedAPOKeys = address.duplicatedAPOKeys;
            }

            if (parcel != null)
            {
                parcelModel.parcelNumber = parcel.parcelNumber;
                parcelModel.UID = parcel.UID;
                parcelModel.sourceSeqNumber = sourceNumber;
                parcelModel.duplicatedAPOKeys = parcel.duplicatedAPOKeys;
            }

            if (owner != null)
            {
                refOwnerModel.ownerNumber = StringUtil.ToLong(Convert.ToString(owner.ownerNumber));
                refOwnerModel.UID = owner.UID;
                refOwnerModel.sourceSeqNumber = sourceNumber;
                refOwnerModel.duplicatedAPOKeys = owner.duplicatedAPOKeys;
            }

            ParcelInfoModel parcelInfo = new ParcelInfoModel();
            parcelInfo.RAddressModel = refAddressModel;
            parcelInfo.parcelModel = parcelModel;
            parcelInfo.ownerModel = refOwnerModel;

            // 2. Show the Codition message of APO here.
            IRefAddressBll addressBll = ObjectFactory.GetObject<IRefAddressBll>();
            _refAddressModelWithCondition = addressBll.GetAddressCondition(CapUtil.GetAgencyCodeList(ModuleName), parcelInfo);

            return ShowCondition(_refAddressModelWithCondition);
        }

        /// <summary>
        /// Show Address Condition
        /// </summary>
        /// <param name="refAddressModel">A RefAddressModel</param>
        /// <returns>true or false. return false if condition is lock, otherwise, return true.</returns>
        private bool ShowCondition(RefAddressModel refAddressModel)
        {
            divConditon.Visible = false;
            ucConditon.HideCondition();

            bool notLock = ConditionsUtil.ShowCondition(ucConditon, refAddressModel);

            divConditon.Visible = ucConditon.ConditionResult != ConditionResult.None;

            return notLock;
        }

        #endregion Methods
    }
}
