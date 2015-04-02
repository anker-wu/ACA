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
using Accela.ACA.Common;
using Accela.ACA.Web.Common;
using Accela.ACA.WSProxy;
using Accela.Web.Controls;

namespace Accela.ACA.Web.Component
{
    /// <summary>
    /// the class for address search result in Spear Form.
    /// </summary>
    public partial class ParcelSearchResult : APOSearchResultBase
    {
        #region Fields

        /// <summary>
        /// A ParcelModel include condition information.
        /// </summary>
        private ParcelModel _parcelModelWithCondition;

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
                return ucParcelList.SelectedParcel;
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
        public ParcelModel ParcelModelWithCondition
        {
            get
            {
                return _parcelModelWithCondition;
            }
        }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Load Address detail and associated Parcel/Owner List when the page is loaded.
        /// </summary>
        public void LoadAPOSearchResult()
        {
            if (AppSession.IsAdmin)
            {
                divParcelAssociates.Visible = true;

                ucAddressList.BindDataSource(APOUtil.BuildAddressDataTable(null));
                ucParcelList.BindDataSource(APOUtil.BuildParcelDataTable(null));
                ucOwnerList.BindDataSource(APOUtil.BuildOwnerDataTable(null));
            }
            else
            {
                if (IsFromMap)
                {
                    if (IsCreateCapFromGIS)
                    {
                        LoadParcelListByGIS(0, null, (ACAGISModel)APOSessionParameter.SearchCriterias);
                    }
                    else
                    {
                        LoadParcelListByGIS(0, null, (ParcelModel)APOSessionParameter.SearchCriterias);
                    }
                }
                else
                {
                    try
                    {
                        LoadParcelList(0, null);
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
        /// Show all condition message of APO at the bottom
        /// </summary>
        /// <param name="address">An AddressModel</param>
        /// <param name="parcel">A ParcelModel</param>
        /// <param name="owner">An OwnerModel</param>
        /// <returns>true or false. false if condition is lock, otherwise, return true.</returns>
        public bool ShowCondition(AddressModel address, ParcelModel parcel, OwnerModel owner)
        {
            RefAddressModel refAddressModel = new RefAddressModel();
            ParcelModel parcelModel = new ParcelModel();
            OwnerModel ownerModel = new OwnerModel();
            ParcelModel parcelPk = ucParcelList.SelectedParcelPK;

            if (address != null)
            {
                refAddressModel.refAddressId = address.refAddressId;
                refAddressModel.UID = address.UID;
                refAddressModel.sourceNumber = Convert.ToInt32(parcelPk.sourceSeqNumber);
                refAddressModel.duplicatedAPOKeys = address.duplicatedAPOKeys;
            }

            if (owner != null)
            {
                ownerModel.ownerNumber = owner.ownerNumber;
                ownerModel.UID = owner.UID;
                ownerModel.sourceSeqNumber = parcelPk.sourceSeqNumber;
                ownerModel.duplicatedAPOKeys = owner.duplicatedAPOKeys;
            }

            if (parcel != null)
            {
                parcelModel.parcelNumber = parcel.parcelNumber;
                parcelModel.UID = parcel.UID;
                parcelModel.sourceSeqNumber = parcelPk.sourceSeqNumber;
                parcelModel.duplicatedAPOKeys = parcel.duplicatedAPOKeys;
            }

            ParcelInfoModel parcelInfo = new ParcelInfoModel();
            parcelInfo.RAddressModel = refAddressModel;
            parcelInfo.parcelModel = parcelModel;
            parcelInfo.ownerModel = ownerModel;

            IParcelBll parcelBll = ObjectFactory.GetObject<IParcelBll>();
            _parcelModelWithCondition = parcelBll.GetParcelCondition(CapUtil.GetAgencyCodeList(ModuleName), parcelInfo);

            return ShowCondition(_parcelModelWithCondition);
        }

        /// <summary>
        /// Raises the <c>System.Web.UI.Control.Ini</c>t event.
        /// </summary>
        /// <param name="e">An System.EventArgs object that contains the event data.</param>
        protected override void OnInit(EventArgs e)
        {
            ucAddressList.IsValidate = IsValidate;
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
        /// Change page index in parcel list.
        /// </summary>
        /// <param name="sender">An object that contains the event sender.</param>
        /// <param name="e">A System.EventArgs object containing the event data.</param>
        protected void Parcel_GridViewIndexChanging(object sender, GridViewPageEventArgs e)
        {
            PaginationModel pageInfo = PaginationUtil.GetPageInfoByID(ucParcelList.ClientID);

            if (e.NewPageIndex > pageInfo.EndPage)
            {
                if (IsFromMap)
                {
                    if (IsCreateCapFromGIS)
                    {
                        LoadParcelListByGIS(e.NewPageIndex, pageInfo.SortExpression, (ACAGISModel)pageInfo.SearchCriterias[0]);
                    }
                    else
                    {
                        LoadParcelListByGIS(e.NewPageIndex, pageInfo.SortExpression, (ParcelModel)pageInfo.SearchCriterias[0]);
                    }
                }
                else
                {
                    LoadParcelList(e.NewPageIndex, pageInfo.SortExpression);
                }
            }
        }

        /// <summary>
        /// Sort in parcel list.
        /// </summary>
        /// <param name="sender">An object that contains the event sender.</param>
        /// <param name="e">A System.EventArgs object containing the event data.</param>
        protected void Parcel_GridViewSort(object sender, AccelaGridViewSortEventArgs e)
        {
            PaginationModel pageInfo = PaginationUtil.GetPageInfoByID(ucParcelList.ClientID);
            pageInfo.SortExpression = e.GridViewSortExpression;
        }

        /// <summary>
        /// Change page index in owner list.
        /// </summary>
        /// <param name="sender">An object that contains the event sender.</param>
        /// <param name="e">A System.EventArgs object containing the event data.</param>
        protected void Owner_GridViewIndexChanging(object sender, GridViewPageEventArgs e)
        {
            PaginationModel pageInfo = PaginationUtil.GetPageInfoByID(ucOwnerList.ClientID);

            if (e.NewPageIndex > pageInfo.EndPage)
            {
                LoadOwnerList(e.NewPageIndex, pageInfo.SortExpression);
            }
        }

        /// <summary>
        /// Sort in owner list.
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
            DataTable dt = GetParcelList(currentPageIndex, sortExpression);

            LoadParcelList(dt);
        }

        /// <summary>
        /// research apo list when create cap from gis map.
        /// </summary>
        /// <param name="currentPageIndex">Current page index</param>
        /// <param name="sortExpression">Sort expression</param>
        /// <param name="gisModel">A ACAGISModel object to use to search</param>
        private void LoadParcelListByGIS(int currentPageIndex, string sortExpression, ACAGISModel gisModel)
        {
            DataTable dt = GetParcelList(currentPageIndex, sortExpression, gisModel);

            LoadParcelList(dt);
        }

        /// <summary>
        /// get apo by gis sent feature
        /// </summary>
        /// <param name="currentPageIndex">Current page index</param>
        /// <param name="sortExpression">Sort expression</param>
        /// <param name="parcelModel">A ParcelModel object to use to search</param>
        private void LoadParcelListByGIS(int currentPageIndex, string sortExpression, ParcelModel parcelModel)
        {
            PaginationModel pageInfo = GetParcelPaginationInfo(currentPageIndex, sortExpression);
            pageInfo.SearchCriterias = new object[] { parcelModel };
            QueryFormat queryFormat = PaginationUtil.GetQueryFormatModel(pageInfo);

            IAPOBll apoBll = ObjectFactory.GetObject<IAPOBll>();
            SearchResultModel result = apoBll.GetRefParcelList(ConfigManager.AgencyCode, parcelModel, queryFormat);
            pageInfo.StartDBRow = result.startRow;

            DataTable dt = APOUtil.BuildParcelDataTable(result.resultList);
            dt = PaginationUtil.MergeDataSource<DataTable>(ucParcelList.ParcelDataSource, dt, pageInfo);

            LoadParcelList(dt);
        }

        /// <summary>
        /// Load address list by specified data source.
        /// </summary>
        /// <param name="datasource">Data source</param>
        private void LoadParcelList(DataTable datasource)
        {
            ucParcelList.BindDataSource(datasource);

            if (datasource.Rows.Count == 1)
            {
                /*
                 * If the parcel search returns only one match, and this parcel is associated with only one address and one owner, 
                 * the APO information will be auto-populated into the APO sections in the page flow.
                 */
                ucParcelList.SelectRow(0);

                LoadParcelAssociates();

                if ((ucAddressList.DataSource == null || ucAddressList.DataSource.Rows.Count <= 1)
                    && (ucOwnerList.OwnerDataSource == null || ucOwnerList.OwnerDataSource.Rows.Count <= 1))
                {
                    OnlyOneAtMost = true;
                }
            }
        }

        /// <summary>
        /// Load associated address and owner by selected parcel.
        /// </summary>
        private void LoadParcelAssociates()
        {
            divParcelAssociates.Visible = true;

            // Load address list
            ucAddressList.DataSource = null;
            ucAddressList.ClearSelectedItems();
            LoadAddressList();

            if (ucAddressList.DataSource != null && ucAddressList.DataSource.Rows.Count == 1)
            {
                ucAddressList.SelectRow(0);
            }

            // Load owner list
            if (!StandardChoiceUtil.IsDisplayOwnerSection())
            {
                // if Standard Choice not enable owner section that no display owner section.
                lblOwnerSession.Visible = false;
                ucOwnerList.Visible = false;
            }
            else
            {
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
        /// Load owner list
        /// </summary>
        /// <param name="currentPageIndex">Current page index</param>
        /// <param name="sortExpression">Sort expression</param>
        private void LoadOwnerList(int currentPageIndex, string sortExpression)
        {
            PaginationModel pageInfo = PaginationUtil.GetPageInfoByID(ucOwnerList.ClientID);
            pageInfo.SortExpression = sortExpression;
            pageInfo.CurrentPageIndex = currentPageIndex;
            pageInfo.CustomPageSize = ucOwnerList.PageSize;
            QueryFormat queryFormat = PaginationUtil.GetQueryFormatModel(pageInfo);

            IAPOBll apoBll = ObjectFactory.GetObject<IAPOBll>();
            SearchResultModel searchResult = apoBll.GetOwnerListByParcelPKs(ConfigManager.AgencyCode, new[] { ucParcelList.SelectedParcelPK }, true, queryFormat);

            pageInfo.StartDBRow = searchResult.startRow;

            DataTable dt = APOUtil.BuildOwnerDataTable(searchResult.resultList);
            dt = PaginationUtil.MergeDataSource<DataTable>(ucOwnerList.OwnerDataSource, dt, pageInfo);

            ucOwnerList.BindDataSource(dt);
        }

        /// <summary>
        /// Load address list
        /// </summary>
        private void LoadAddressList()
        {
            IAPOBll apoBll = ObjectFactory.GetObject<IAPOBll>();
            RefAddressModel[] refAddressInfos = apoBll.GetRefAddressListByParcelPK(ConfigManager.AgencyCode, ucParcelList.SelectedParcelPK, true);

            DataTable dt = APOUtil.BuildAddressDataTable(refAddressInfos, AddressFormatType.LONG_ADDRESS_NO_FORMAT);

            if (dt.Columns.Contains("FullAddress"))
            {
                DataView dv = dt.DefaultView;
                dv.Sort = "FullAddress ASC";
                dt = dv.ToTable();
            }

            ucAddressList.BindDataSource(dt);
        }

        /// <summary>
        /// Get address list when create a cap from GIS map.
        /// </summary>
        /// <param name="currentPageIndex">Current PageIndex</param>
        /// <param name="sortExpression">Sort Expression</param>
        /// <param name="gisModel">The ACAGISModel</param>
        /// <returns>A DataTable</returns>
        private DataTable GetParcelList(int currentPageIndex, string sortExpression, ACAGISModel gisModel)
        {
            PaginationModel pageInfo = GetParcelPaginationInfo(currentPageIndex, sortExpression);
            pageInfo.SearchCriterias = new object[] { gisModel };

            ParcelModel[] parcelInfos = GISUtil.GetParcelListByGISModel(pageInfo, gisModel);
            DataTable dt = CreateDataSource(parcelInfos);

            return PaginationUtil.MergeDataSource<DataTable>(ucParcelList.ParcelDataSource, dt, pageInfo);
        }

        /// <summary>
        /// Get address list from search form
        /// </summary>
        /// <param name="currentPageIndex">Current PageIndex</param>
        /// <param name="sortExpression">Sort Expression</param>
        /// <returns>A DataTable</returns>
        private DataTable GetParcelList(int currentPageIndex, string sortExpression)
        {
            ParcelModel parcel = APOSessionParameter.SearchCriterias as ParcelModel;

            PaginationModel pageInfo = GetParcelPaginationInfo(currentPageIndex, sortExpression);
            QueryFormat queryFormat = PaginationUtil.GetQueryFormatModel(pageInfo);

            IAPOBll apoBll = ObjectFactory.GetObject<IAPOBll>();
            SearchResultModel result = apoBll.GetRefParcelList(ConfigManager.AgencyCode, parcel, queryFormat);
            pageInfo.StartDBRow = result.startRow;

            DataTable dt = CreateDataSource(result.resultList);

            return PaginationUtil.MergeDataSource<DataTable>(ucParcelList.ParcelDataSource, dt, pageInfo);
        }

        /// <summary>
        /// Get address's pagination information.
        /// </summary>
        /// <param name="currentPageIndex">Current page index</param>
        /// <param name="sortExpression">Sort expression</param>
        /// <returns>A PaginationModel object</returns>
        private PaginationModel GetParcelPaginationInfo(int currentPageIndex, string sortExpression)
        {
            PaginationModel pageInfo = PaginationUtil.GetPageInfoByID(ucParcelList.ClientID);
            pageInfo.SortExpression = sortExpression;
            pageInfo.CurrentPageIndex = currentPageIndex;
            pageInfo.CustomPageSize = ucParcelList.PageSize;

            return pageInfo;
        }

        /// <summary>
        /// Create the data source for address list
        /// </summary>
        /// <param name="parcelInfos">ParcelModel array</param>
        /// <returns>data source for address list</returns>
        private DataTable CreateDataSource(object[] parcelInfos)
        {
            DataTable dt = APOUtil.BuildParcelDataTable(parcelInfos);

            if (dt.Columns.Contains("ParcelNumber"))
            {
                DataView dv = dt.DefaultView;
                dv.Sort = "ParcelNumber ASC";
                dt = dv.ToTable();
            }

            return dt;
        }

        /// <summary>
        /// Show parcel's Condition
        /// </summary>
        /// <param name="parcelModel">A ParcelModel</param>
        /// <returns>true or false. return false if condition is lock, otherwise, return true.</returns>
        private bool ShowCondition(ParcelModel parcelModel)
        {
            divConditon.Visible = false;
            ucConditon.HideCondition();

            bool notLock = ConditionsUtil.ShowCondition(ucConditon, parcelModel);

            divConditon.Visible = ucConditon.ConditionResult != ConditionResult.None;

            return notLock;
        }

        #endregion Methods
    }
}
