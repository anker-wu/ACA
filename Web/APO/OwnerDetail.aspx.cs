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
 *  Description:
 *
 *  Notes:
 *      $Id: OwnerDetail.aspx.cs 277585 2014-08-18 11:09:47Z ACHIEVO\james.shi $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using System;
using System.Data;
using System.Web;
using System.Web.UI.WebControls;
using Accela.ACA.BLL.APO;
using Accela.ACA.Common;
using Accela.ACA.Common.Util;
using Accela.ACA.Web.Common;
using Accela.ACA.WSProxy;
using Accela.Web.Controls;

namespace Accela.ACA.Web.APO
{
    /// <summary>
    /// Page to display owner detail
    /// </summary>
    public partial class OwnerDetail : BasePage
    {
        #region Fields

        /// <summary>
        /// owner model.
        /// </summary>
        private OwnerModel _ownerModel = null;

        /// <summary>
        /// owner reference number for internal APO.
        /// </summary>
        private string _ownerNum;

        /// <summary>
        /// source sequence number.
        /// </summary>
        private string _sourceSeqNbr;

        /// <summary>
        /// owner unique id for external APO.
        /// </summary>
        private string _ownerUID;
        
        /// <summary>
        /// Reference owner model with key value.
        /// Include reference owner number, owner unique id, sourceSequenceNumber.
        /// </summary>
        private OwnerModel _ownerPK = null;

        #endregion Fields

        #region Methods

        /// <summary>
        /// Load the owner detail, AddressListView and ParcelView when the page is loaded
        /// </summary>
        /// <param name="sender">An object that contains the event sender.</param>
        /// <param name="e">An System.EventArgs object that contains the event data.</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            LoadDialogCss();

            if (AppSession.IsAdmin)
            {
                addressList.Visible = true;
                addressList.BindDataSource(APOUtil.BuildAPODataTable(null));

                parcelList.Visible = true;
                parcelList.BindDataSource(APOUtil.BuildParcelDataTable(null));
            }
            else
            {
                try
                {
                    _sourceSeqNbr = Request.QueryString[ACAConstant.REQUEST_PARMETER_OWNER_SEQUENCE];
                    _ownerNum = Request.QueryString[ACAConstant.REQUEST_PARMETER_OWNER_NUMBER];
                    _ownerUID = Request.QueryString[ACAConstant.REQUEST_PARMETER_OWNER_UID];
                    
                    OwnerModel ownerPK = new OwnerModel();
                    ownerPK.ownerNumber = StringUtil.ToLong(_ownerNum);
                    ownerPK.sourceSeqNumber = StringUtil.ToLong(_sourceSeqNbr);
                    ownerPK.UID = _ownerUID;
                    _ownerPK = ownerPK;

                    _ownerModel = this.GetOwnerModel(_ownerPK);
                    if (!IsPostBack)
                    {
                        addressList.RefAPODataSource = null;
                        parcelList.ParcelDataSource = null;
                        LoadOwnerView();

                        LoadAddressListView(0, null);
                        LoadParcelView(0, null);
                    }
                }
                catch
                {
                }
            }
        }

        /// <summary>
        /// Address List GridViewIndex Changing
        /// </summary>
        /// <param name="sender">An object that contains the event sender.</param>
        /// <param name="e">A System.EventArgs object containing the event data.</param>
        protected void Address_GridViewIndexChanging(object sender, GridViewPageEventArgs e)
        {
            PaginationModel pageInfo = PaginationUtil.GetPageInfoByID(addressList.ClientID);

            if (e.NewPageIndex > pageInfo.EndPage)
            {
                LoadAddressListView(e.NewPageIndex, pageInfo.SortExpression);
            }
        }

        /// <summary>
        /// PermitList GridViewSort
        /// </summary>
        /// <param name="sender">An object that contains the event sender.</param>
        /// <param name="e">A System.EventArgs object containing the event data.</param>
        protected void Address_GridViewSort(object sender, AccelaGridViewSortEventArgs e)
        {
            PaginationModel pageInfo = PaginationUtil.GetPageInfoByID(addressList.ClientID);
            pageInfo.SortExpression = e.GridViewSortExpression;
        }

        /// <summary>
        /// Parcel List GridViewIndex Changing
        /// </summary>
        /// <param name="sender">An object that contains the event sender.</param>
        /// <param name="e">A System.EventArgs object containing the event data</param>
        protected void Parcel_GridViewIndexChanging(object sender, GridViewPageEventArgs e)
        {
            PaginationModel pageInfo = PaginationUtil.GetPageInfoByID(parcelList.ClientID);

            if (e.NewPageIndex > pageInfo.EndPage)
            {
                LoadParcelView(e.NewPageIndex, pageInfo.SortExpression);
            }
        }

        /// <summary>
        /// PermitList GridViewSort
        /// </summary>
        /// <param name="sender">An object that contains the event sender.</param>
        /// <param name="e">A System.EventArgs object containing the event data.</param>
        protected void Parcel_GridViewSort(object sender, AccelaGridViewSortEventArgs e)
        {
            PaginationModel pageInfo = PaginationUtil.GetPageInfoByID(parcelList.ClientID);
            pageInfo.SortExpression = e.GridViewSortExpression;
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
        }

        /// <summary>
        /// Load the APO listView
        /// </summary>
        /// <param name="currentPageIndex">Current Page Index.</param>
        /// <param name="sortExpression">Sort Expression.</param>
        private void LoadAddressListView(int currentPageIndex, string sortExpression)
        {
            if (_ownerModel == null)
            {
                return;
            }

            PaginationModel pageInfo = PaginationUtil.GetPageInfoByID(addressList.ClientID);
            pageInfo.SortExpression = sortExpression;
            pageInfo.CurrentPageIndex = currentPageIndex;
            pageInfo.CustomPageSize = addressList.PageSize;
            QueryFormat queryFormat = PaginationUtil.GetQueryFormatModel(pageInfo);
            IAPOBll apoBll = ObjectFactory.GetObject<IAPOBll>();
            SearchResultModel result = apoBll.GetAddressListByOwnerPK(ConfigManager.AgencyCode, _ownerModel, queryFormat);
            pageInfo.StartDBRow = result.startRow;
            DataTable dt = APOUtil.BuildAPODataTable(result.resultList);
            dt = PaginationUtil.MergeDataSource<DataTable>(addressList.RefAPODataSource, dt, pageInfo);

            if (dt == null || dt.Rows.Count == 0)
            {
                lblPropertyInfo.Text = string.Empty;
                addressList.BindDataSource(dt);
                return;
            }

            bool found = false;
            string filterStr = string.Empty;

            if ((!string.IsNullOrEmpty(Request.Params[ACAConstant.REQUEST_PARMETER_REFADDRESS_ID])
                    || !string.IsNullOrEmpty(Request.Params[ACAConstant.REQUEST_PARMETER_REFADDRESS_UID]))
                    && !string.IsNullOrEmpty(Request.Params[ACAConstant.REQUEST_PARMETER_ADDRESS_SEQUENCE]))
            {
                filterStr = string.Format(
                                            "AddressSequenceNumber={0}",
                                            Request.Params[ACAConstant.REQUEST_PARMETER_ADDRESS_SEQUENCE]);

                if (!string.IsNullOrEmpty(Request.Params[ACAConstant.REQUEST_PARMETER_REFADDRESS_ID]))
                {
                    filterStr = filterStr + " and " +
                                string.Format(
                                                "AddressID={0}",
                                                Request.Params[ACAConstant.REQUEST_PARMETER_REFADDRESS_ID]);
                }

                if (!string.IsNullOrEmpty(Request.Params[ACAConstant.REQUEST_PARMETER_REFADDRESS_UID]))
                {
                    filterStr = filterStr + " and " +
                                string.Format(
                                                "AddressUID='{0}'",
                                                Request.Params[ACAConstant.REQUEST_PARMETER_REFADDRESS_UID]);
                }
            }

            string parcelFilter = string.Empty;
            if ((!string.IsNullOrEmpty(Request.Params[ACAConstant.REQUEST_PARMETER_PARCEL_NUMBER])
                    || !string.IsNullOrEmpty(Request.Params[ACAConstant.REQUEST_PARMETER_PARCEL_UID]))
                    && !string.IsNullOrEmpty(Request.Params[ACAConstant.REQUEST_PARMETER_PARCEL_SEQUENCE]))
            {
                if (filterStr != string.Empty)
                {
                    filterStr = filterStr + " and ";
                }

                parcelFilter = string.Format(
                                            "ParcelSequenceNumber={0}",
                                             Request.Params[ACAConstant.REQUEST_PARMETER_PARCEL_SEQUENCE]);

                if (!string.IsNullOrEmpty(Request.Params[ACAConstant.REQUEST_PARMETER_PARCEL_NUMBER]))
                {
                    parcelFilter = parcelFilter + " and " +
                                   string.Format(
                                                "ParcelNumber='{0}'",
                                                 HttpUtility.UrlDecode(Request.Params[ACAConstant.REQUEST_PARMETER_PARCEL_NUMBER]));
                }

                if (!string.IsNullOrEmpty(Request.Params[ACAConstant.REQUEST_PARMETER_PARCEL_UID]))
                {
                    parcelFilter = parcelFilter + " and " +
                                   string.Format(
                                                "ParcelUID='{0}'",
                                                 Request.Params[ACAConstant.REQUEST_PARMETER_PARCEL_UID]);
                }

                filterStr = filterStr + parcelFilter;
            }

            if (filterStr != string.Empty)
            {
                DataRow[] rows = dt.Select(filterStr);

                if (rows.Length == 0 && filterStr != parcelFilter && parcelFilter != string.Empty)
                {
                    rows = dt.Select(parcelFilter);
                }

                if (rows.Length > 0)
                {
                    found = true;
                    lblPropertyInfo.Text = rows[0]["FULLADDRESS"] == null ? string.Empty : rows[0]["FULLADDRESS"].ToString();
                }
            }

            if (!found)
            {
                //01/21/2008 peter.pan
                lblPropertyInfo.Text = dt.Rows[0]["FULLADDRESS"] == DBNull.Value ? string.Empty : dt.Rows[0]["FULLADDRESS"].ToString();
            }

            addressList.ComponentType = 0;
            addressList.BindDataSource(dt);
            addressList.Visible = true;
        }

        /// <summary>
        /// Builds the owner model.
        /// </summary>
        /// <param name="ownerPK">The owner PK model for web service.</param>
        /// <returns>the owner model.</returns>
        private OwnerModel GetOwnerModel(OwnerModel ownerPK)
        {
            OwnerModel result = null;

            IOwnerBll ownerBll = (IOwnerBll)ObjectFactory.GetObject(typeof(IOwnerBll));
            result = ownerBll.GetOwnerByPK(ConfigManager.AgencyCode, ownerPK);

            /*
             * As designed - current page does not display template fields.
             * And can not search out address data by templates if the owner is from third party(XAPO).
             * So clear the templates.
             */
            result.templates = null;

            return result;
        }

        /// <summary>
        /// Load the Owner detail
        /// </summary>
        private void LoadOwnerView()
        {
            lblOwnerName.Text = _ownerModel.ownerFullName;
            lblOwnerDetail.Text = PeopleUtil.BuildOwnerMailAddressString(_ownerModel);
            string phoneText = ModelUIFormat.FormatPhoneShow(_ownerModel.phoneCountryCode, _ownerModel.phone, _ownerModel.mailCountry);
            lblOwnerPhone.Text = I18nStringUtil.FormatToTableRow(LabelUtil.GetTextByKey("apo_owner_phone", ModuleName), phoneText);
            string faxText = ModelUIFormat.FormatPhoneShow(_ownerModel.faxCountryCode, _ownerModel.fax, _ownerModel.mailCountry);
            lblOwnerFax.Text = I18nStringUtil.FormatToTableRow(LabelUtil.GetTextByKey("apo_owner_fax", ModuleName), faxText);
            lblOwnerEmail.Text = _ownerModel.email;
            string displayOwnerStatus = string.Empty;

            if (ACAConstant.VALID_STATUS.Equals(_ownerModel.ownerStatus, StringComparison.InvariantCultureIgnoreCase))
            {
                displayOwnerStatus = GetTextByKey("aca_owner_label_statusenable");
            }
            else if (ACAConstant.INVALID_STATUS.Equals(_ownerModel.ownerStatus, StringComparison.InvariantCultureIgnoreCase))
            {
                displayOwnerStatus = GetTextByKey("aca_owner_label_statusdisabled");
            }

            lblOwnerStatus.Text = I18nStringUtil.FormatToTableRow(LabelUtil.GetTextByKey("aca_owner_label_statustitle", ModuleName), displayOwnerStatus);
        }

        /// <summary>
        /// Load the parcel view
        /// </summary>
        /// <param name="currentPageIndex">Current Page Index.</param>
        /// <param name="sortExpression">Sort Expression.</param>
        private void LoadParcelView(int currentPageIndex, string sortExpression)
        {
            string callID = AppSession.User.UserID;
            PaginationModel pageInfo = PaginationUtil.GetPageInfoByID(parcelList.ClientID);
            pageInfo.SortExpression = sortExpression;
            pageInfo.CurrentPageIndex = currentPageIndex;
            pageInfo.CustomPageSize = addressList.PageSize;
            QueryFormat queryFormat = PaginationUtil.GetQueryFormatModel(pageInfo);
            IParcelBll parcelBll = ObjectFactory.GetObject<IParcelBll>();
            ParcelModel[] parcels = parcelBll.GetParcelListByOwnerPK(ConfigManager.AgencyCode, _ownerPK, callID, queryFormat);
            DataTable dt = APOUtil.BuildParcelDataTable(parcels);
            dt = PaginationUtil.MergeDataSource<DataTable>(parcelList.ParcelDataSource, dt, pageInfo);

            parcelList.BindDataSource(dt);
            parcelList.Visible = true;
        }

        #endregion Methods
    }
}
