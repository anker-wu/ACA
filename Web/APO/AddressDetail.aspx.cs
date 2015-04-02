#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: AddressDetail.aspx.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2008-2014
 *
 *  Description:
 *
 *  Notes:
 *      $Id: AddressDetail.aspx.cs 277585 2014-08-18 11:09:47Z ACHIEVO\james.shi $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.UI.WebControls;
using Accela.ACA.BLL.AddressBuilder;
using Accela.ACA.BLL.APO;
using Accela.ACA.Common;
using Accela.ACA.Common.Log;
using Accela.ACA.Common.Util;
using Accela.ACA.Web.Common;
using Accela.ACA.Web.Component;
using Accela.ACA.WSProxy;
using Accela.Web.Controls;

namespace Accela.ACA.Web.APO
{
    /// <summary>
    /// Page to display address detail
    /// </summary>
    public partial class AddressDetail : BasePage
    {
        #region Fields

        /// <summary>
        /// log4net Logger
        /// </summary>
        private static readonly log4net.ILog Logger = LogFactory.Instance.GetLogger(typeof(AddressDetail));

        /// <summary>
        /// Reference address model with key value.
        /// Include refAddressID, refAddressUID, sourceSequenceNumber.
        /// </summary>
        private RefAddressModel _refAddressPK;

        #endregion Fields

        #region Methods

        /// <summary>
        /// ShowOnMap event handler
        /// </summary>
        /// <param name="sender">An object that contains the event sender.</param>
        /// <param name="e">A System.EventArgs object containing the event data.</param>
        public void MapAddress_ShowOnMap(object sender, EventArgs e)
        {
            ACAMap map = sender as ACAMap;
            RefAddressModel refAddressModel = this.GetRefAddressModel(_refAddressPK);
            IAPOBll apoBll = ObjectFactory.GetObject<IAPOBll>();
            SearchResultModel result = apoBll.GetParcelInfoByAddress(ConfigManager.AgencyCode, _refAddressPK, null, false); 
            ParcelInfoModel[] apolist = ObjectConvertUtil.ConvertObjectArray2EntityArray<ParcelInfoModel>(result.resultList);

            if (refAddressModel != null)
            {
                refAddressModel.parcelLists = apolist;
            }

            string sourceNumbs = Request.QueryString[ACAConstant.REQUEST_PARMETER_APO_SOURCE_NUMBERS];
            if (!string.IsNullOrEmpty(sourceNumbs))
            {
                string[] sourceNumbers = sourceNumbs.Split(',');
                List<DuplicatedAPOKeyModel> apoKeys = new List<DuplicatedAPOKeyModel>();
                foreach (string item in sourceNumbers)
                {
                    if (!string.IsNullOrEmpty(item))
                    {
                        apoKeys.Add(new DuplicatedAPOKeyModel()
                        {
                            sourceNumber = item
                        });
                    }
                }

                refAddressModel.duplicatedAPOKeys = apoKeys.ToArray();
            }

            ACAGISModel gisModel = GISUtil.CreateACAGISModel();
            gisModel.Context = map.AGISContext;
            gisModel.UserGroups.Add(AppSession.User.IsAnonymous ? GISUserGroup.Anonymous.ToString() : GISUserGroup.Register.ToString());
            GISUtil.SetPostUrl(this, gisModel);
            List<RefAddressModel> list = new List<RefAddressModel>();
            list.Add(refAddressModel);
            gisModel.IsMiniMap = map.IsMiniMode;
            gisModel.Agency = ConfigManager.AgencyCode;
            gisModel.RefAddressModels = list.ToArray();
            map.ACAGISModel = gisModel;
        }

        /// <summary>
        /// Load Address detail and Parcel List when the page is loaded
        /// </summary>
        /// <param name="sender">An object that contains the event sender.</param>
        /// <param name="e">An System.EventArgs object that contains the event data.</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            InitRefAddressPKModel();

            if (!IsPostBack)
            {
                if (AppSession.IsAdmin)
                {
                    parcellList.BindDataSource(APOUtil.BuildParcelDataTable(null));
                }
                else
                {
                    try
                    {
                        RefAddressModel refAddressModel = this.GetRefAddressModel(_refAddressPK);
                        parcellList.ParcelDataSource = null;
                        LoadAddressView(refAddressModel);
                        LoadParcelListView(0, null);

                        LoadDialogCss();
                    }
                    catch (Exception ex)
                    {
                        Logger.ErrorFormat("Error occurred, error message:{0}", ex);
                        ucParcelList.Visible = false;
                    }
                }
            }
        }

        /// <summary>
        /// ParcelList GridViewIndexChanging
        /// </summary>
        /// <param name="sender">An object that contains the event sender.</param>
        /// <param name="e">A System.EventArgs object containing the event data.</param>
        protected void Parcel_GridViewIndexChanging(object sender, GridViewPageEventArgs e)
        {
            PaginationModel pageInfo = PaginationUtil.GetPageInfoByID(parcellList.ClientID);

            if (e.NewPageIndex > pageInfo.EndPage)
            {
                LoadParcelListView(e.NewPageIndex, pageInfo.SortExpression);
            }
        }

        /// <summary>
        /// PermitList GridViewSort
        /// </summary>
        /// <param name="sender">An object that contains the event sender.</param>
        /// <param name="e">A System.EventArgs object containing the event data.</param>
        protected void Parcel_GridViewSort(object sender, AccelaGridViewSortEventArgs e)
        {
            PaginationModel pageInfo = PaginationUtil.GetPageInfoByID(parcellList.ClientID);
            pageInfo.SortExpression = e.GridViewSortExpression;
        }

        /// <summary>
        /// Initial the _refAddressPK model by query string.
        /// </summary>
        private void InitRefAddressPKModel()
        {
            string refAddressID = Request.QueryString[ACAConstant.REQUEST_PARMETER_REFADDRESS_ID];
            string sourceSeqNbr = Request.QueryString[ACAConstant.REQUEST_PARMETER_ADDRESS_SEQUENCE];
            string refAddessUID = Request.QueryString[ACAConstant.REQUEST_PARMETER_REFADDRESS_UID];

            _refAddressPK = new RefAddressModel();
            _refAddressPK.refAddressId = StringUtil.ToLong(refAddressID);
            _refAddressPK.sourceNumber = StringUtil.ToInt(sourceSeqNbr);
            _refAddressPK.UID = refAddessUID;
        }

        /// <summary>
        /// Load Dialog CSS.
        /// </summary>
        private void LoadDialogCss()
        {
            string hideHeader = Request.QueryString["HideHeader"];
            if (string.Equals(hideHeader, "True", StringComparison.CurrentCultureIgnoreCase))
            {
                string className = MainContent.Attributes["class"];
                className += " ACA_Dialog_Content";
                MainContent.Attributes.Add("class", className);
            }

            if (string.Equals(hideHeader, ACAConstant.COMMON_TRUE, StringComparison.CurrentCultureIgnoreCase))
            {
                mapAddress.Visible = false;
                miniMapAddress.Visible = StandardChoiceUtil.IsShowMap4ShowObject(this.ModuleName);
                miniMapAddress.ShowMap();
            }
            else
            {
                miniMapAddress.Visible = false;
                mapAddress.Visible = StandardChoiceUtil.IsShowMap4ShowObject(this.ModuleName);
            }
        }

        /// <summary>
        /// Builds the refAddress model.
        /// </summary>
        /// <param name="refAddressPK">The refAddress for model.</param>
        /// <returns>the refAddress model.</returns>
        private RefAddressModel GetRefAddressModel(RefAddressModel refAddressPK)
        {
            RefAddressModel result = null;

            IRefAddressBll addressBll = (IRefAddressBll)ObjectFactory.GetObject(typeof(IRefAddressBll));
            result = addressBll.GetAddressByPK(ConfigManager.AgencyCode, refAddressPK);

            return result;
        }

        /// <summary>
        /// Load Address detail
        /// </summary>
        /// <param name="addressModel">The address model.</param>
        private void LoadAddressView(RefAddressModel addressModel)
        {
            IAddressBuilderBll addressBuilderBll = ObjectFactory.GetObject(typeof(IAddressBuilderBll)) as IAddressBuilderBll;
            string addressString = addressBuilderBll.BuildAddressByFormatType(addressModel, null, AddressFormatType.SHORT_ADDRESS_WITH_FORMAT);

            lblPropertyInfo.Text = addressString;
            lblAddressDetail.Text = addressString;
            string displayAddressStatus = string.Empty;

            if (ACAConstant.VALID_STATUS.Equals(addressModel.addressStatus, StringComparison.InvariantCultureIgnoreCase))
            {
                displayAddressStatus = GetTextByKey("aca_address_label_statusenable");
            }
            else if (ACAConstant.INVALID_STATUS.Equals(addressModel.addressStatus, StringComparison.InvariantCultureIgnoreCase))
            {
                displayAddressStatus = GetTextByKey("aca_address_label_statusdisabled");
            }

            lblAddressStatus.Text = I18nStringUtil.FormatToTableRow(LabelUtil.GetTextByKey("aca_address_label_statustitle", ModuleName), displayAddressStatus);
        }

        /// <summary>
        /// Load Parcel view
        /// </summary>
        /// <param name="currentPageIndex">Current PageIndex.</param>
        /// <param name="sortExpression">Sort expression.</param>
        private void LoadParcelListView(int currentPageIndex, string sortExpression)
        {
            PaginationModel pageInfo = PaginationUtil.GetPageInfoByID(parcellList.ClientID);
            pageInfo.SortExpression = sortExpression;
            pageInfo.CurrentPageIndex = currentPageIndex;
            pageInfo.CustomPageSize = parcellList.PageSize;
            QueryFormat queryFormat = PaginationUtil.GetQueryFormatModel(pageInfo);
            IParcelBll parcelBll = ObjectFactory.GetObject<IParcelBll>();

            ParcelModel[] parcels = parcelBll.GetParcelListByRefAddressPK(ConfigManager.AgencyCode, _refAddressPK, queryFormat);
            DataTable dt = APOUtil.BuildParcelDataTable(parcels);
            dt = PaginationUtil.MergeDataSource<DataTable>(parcellList.ParcelDataSource, dt, pageInfo);

            parcellList.BindDataSource(dt);
        } 

        #endregion Methods
    }
}
