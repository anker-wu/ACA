#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: CapDetail.aspx.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2008-2014
 *
 *  Description:
 *
 *  Notes:
 *      $Id: ParcelDetail.aspx.cs 278175 2014-08-28 09:03:13Z ACHIEVO\james.shi $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using System;
using System.Collections.Generic;
using System.Data;
using System.Web.UI.WebControls;
using Accela.ACA.BLL.APO;
using Accela.ACA.Common;
using Accela.ACA.Common.Util;
using Accela.ACA.Web.Common;
using Accela.ACA.Web.Component;
using Accela.ACA.WSProxy;
using Accela.Web.Controls;

namespace Accela.ACA.Web.APO
{
    /// <summary>
    /// Page to display parcel detail
    /// </summary>
    public partial class ParcelDetail : BasePage
    {
        #region Fields

        /// <summary>
        /// The padding status parcel.
        /// </summary>
        private const string PendingParcel = "P";

        /// <summary>
        /// Creates an instance of ParcelModel 
        /// </summary>
        private ParcelModel _parcelModal = null;

        /// <summary>
        /// parcel reference number for internal APO.
        /// </summary>
        private string _parcelNum;

        /// <summary>
        /// source sequence number
        /// </summary>
        private string _sourceSeqNbr;
        
        /// <summary>
        /// parcel unique id for external APO.
        /// </summary>
        private string _parcelUID;

        /// <summary>
        /// Reference parcel model with key value.
        /// Include reference parcel number, parcel unique id, sourceSequenceNumber.
        /// </summary>
        private ParcelModel _parcelPK = null;

        /// <summary>
        /// Gets or sets ParcelInfo CSS
        /// </summary>
        protected string ParcelInfoCss
        {
            get;
            set;
        }

        #endregion Fields

        #region Methods

        /// <summary>
        /// Raises the page load event
        /// </summary>
        /// <param name="sender">An object that contains the event sender.</param>
        /// <param name="e">A System.EventArgs object containing the event data.</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (AppSession.IsAdmin)
            {
                divRefGenealogyList.Visible = true;
                addressList.BindDataSource(APOUtil.BuildAPODataTable(null));
                ownerList.BindDataSource(APOUtil.BuildOwnerDataTable(null));
            }
            else
            {
                try
                {
                    if (StandardChoiceUtil.IsEnableParcelGenealogy())
                    {
                        divRefGenealogyList.Visible = true;
                    }

                    _parcelNum = Request.QueryString[ACAConstant.REQUEST_PARMETER_PARCEL_NUMBER];
                    _sourceSeqNbr = Request.QueryString[ACAConstant.REQUEST_PARMETER_PARCEL_SEQUENCE];
                    _parcelUID = Request.QueryString[ACAConstant.REQUEST_PARMETER_PARCEL_UID];

                    LoadDialogCss();

                    _parcelModal = this.GetParcelModel(_parcelUID, _parcelNum, _sourceSeqNbr);

                    refGenealogyList.RefParcelNumber = _parcelModal == null ? _parcelNum : _parcelModal.parcelNumber;

                    if (!IsPostBack)
                    {
                        addressList.RefAPODataSource = null;
                        LoadParcelView();
                        LoadAddressView(0, null);

                        //owner webservice doesn't restrict search result, so no action in this interface.
                        LoadOwnerView(0, null);
                        LoadParcelTemplateAttributes();
                    }

                    //if Standard Choice not enable owner section that no display owner section.
                    if (!AppSession.IsAdmin && !StandardChoiceUtil.IsDisplayOwnerSection())
                    {
                        APO_ParcelDetail_lbl_Session_Contacts.Visible = false;
                        ucOwnerList.Visible = false;
                    }
                }
                catch
                {
                }
            }
        }

        /// <summary>
        /// ShowOnMap event handler
        /// </summary>
        /// <param name="sender">An object that contains the event sender.</param>
        /// <param name="e">A System.EventArgs object containing the event data.</param>
        protected void MapParcel_ShowOnMap(object sender, EventArgs e)
        {
            ACAMap map = sender as ACAMap;
            _parcelNum = Request.QueryString[ACAConstant.REQUEST_PARMETER_PARCEL_NUMBER];
            _sourceSeqNbr = Request.QueryString[ACAConstant.REQUEST_PARMETER_PARCEL_SEQUENCE];
            _parcelUID = Request.QueryString[ACAConstant.REQUEST_PARMETER_PARCEL_UID];

            _parcelModal = this.GetParcelModel(_parcelUID, _parcelNum, _sourceSeqNbr);

            //there will be multiple parcel models by the given parcel number and
            //the source sequence number of the first parcel will be returned which may not be consistent
            //with the Source number passed to BIZ
            _parcelModal.sourceSeqNumber = long.Parse(_sourceSeqNbr);

            string sourceNumbs = Request.QueryString[ACAConstant.REQUEST_PARMETER_APO_SOURCE_NUMBERS];
            if (!string.IsNullOrEmpty(sourceNumbs))
            {
                string[] sourceNumbers = sourceNumbs.Split(',');
                List<DuplicatedAPOKeyModel> apoKeys = new List<DuplicatedAPOKeyModel>();
                foreach (string item in sourceNumbers)
                {
                    apoKeys.Add(new DuplicatedAPOKeyModel()
                    {
                        sourceNumber = item
                    });
                }

                _parcelModal.duplicatedAPOKeys = apoKeys.ToArray();
            }
            
            ACAGISModel gisModel = GISUtil.CreateACAGISModel();
            gisModel.Context = map.AGISContext;
            gisModel.ModuleName = ModuleName;
            GISUtil.SetPostUrl(this, gisModel);
            List<ParcelModel> list = new List<ParcelModel>();
            list.Add(_parcelModal);
            gisModel.UserGroups.Add(AppSession.User.IsAnonymous ? GISUserGroup.Anonymous.ToString() : GISUserGroup.Register.ToString());
            gisModel.ParcelModels = list.ToArray();
            gisModel.IsMiniMap = map.IsMiniMode;
            gisModel.Agency = ConfigManager.AgencyCode;
            map.ACAGISModel = gisModel;
        }

        /// <summary>
        /// address list index change
        /// </summary>
        /// <param name="sender">An object that contains the event sender.</param>
        /// <param name="e">A System.EventArgs object containing the event data.</param>
        protected void Address_GridViewIndexChanging(object sender, GridViewPageEventArgs e)
        {
            PaginationModel pageInfo = PaginationUtil.GetPageInfoByID(addressList.ClientID);

            if (e.NewPageIndex > pageInfo.EndPage)
            {
                LoadAddressView(e.NewPageIndex, pageInfo.SortExpression);
            }
        }

        /// <summary>
        /// Parcel associated owner List index change
        /// </summary>
        /// <param name="sender">An object that contains the event sender.</param>
        /// <param name="e">A System.EventArgs object containing the event data.</param>
        protected void RefOwner_GridViewIndexChanging(object sender, GridViewPageEventArgs e)
        {
            PaginationModel pageInfo = PaginationUtil.GetPageInfoByID(ownerList.ClientID);

            if (e.NewPageIndex > pageInfo.EndPage)
            {
                 LoadOwnerView(e.NewPageIndex, pageInfo.SortExpression);
            }
        }

        /// <summary>
        /// address list GridViewSort
        /// </summary>
        /// <param name="sender">An object that contains the event sender.</param>
        /// <param name="e">A System.EventArgs object containing the event data.</param>
        protected void Address_GridViewSort(object sender, AccelaGridViewSortEventArgs e)
        {
            PaginationModel pageInfo = PaginationUtil.GetPageInfoByID(addressList.ClientID);
            pageInfo.SortExpression = e.GridViewSortExpression;
        }

        /// <summary>
        /// Parcel associated owner list GridViewSort
        /// </summary>
        /// <param name="sender">An object that contains the event sender.</param>
        /// <param name="e">A System.EventArgs object containing the event data.</param>
        protected void RefOwner_GridViewSort(object sender, AccelaGridViewSortEventArgs e)
        {
            PaginationModel pageInfo = PaginationUtil.GetPageInfoByID(ownerList.ClientID);
            pageInfo.SortExpression = e.GridViewSortExpression;
        } 

        /// <summary>
        /// Load Dialog CSS.
        /// </summary>
        private void LoadDialogCss()
        {
            string hideHeader = Request["HideHeader"];
            if (string.Equals(hideHeader, "True", StringComparison.CurrentCultureIgnoreCase))
            {
                string className = MainContent.Attributes["class"];
                className += " ACA_Dialog_Content";
                MainContent.Attributes.Add("class", className);
            }

            if (string.Equals(hideHeader, ACAConstant.COMMON_TRUE, StringComparison.CurrentCultureIgnoreCase))
            {
                mapParcel.Visible = false;
                dvMiniMap.Visible = true;
                dvParcelInfo.Attributes["class"] = "td_mini_map_left";
                ParcelInfoCss = "ACA_LeftOrRightForParcelWithMiniMap";
                tblParcelLegal.Attributes["class"] = "ACA_ParcelDetailStyleWithMinaMap";
                miniMapParcel.Visible = StandardChoiceUtil.IsShowMap4ShowObject(this.ModuleName);
                miniMapParcel.ShowMap();
            }
            else
            {
                tblParcelLegal.Attributes["class"] = "ACA_ParcelDetailStyle";
                ParcelInfoCss = "ACA_LeftOrRightForParcel";
                dvMiniMap.Visible = false;
                miniMapParcel.Visible = false;
                mapParcel.Visible = StandardChoiceUtil.IsShowMap4ShowObject(this.ModuleName);
            }
        }

        /// <summary>
        /// Get Parcel PK model.
        /// </summary>
        /// <param name="parcelUID">Parcel UID.</param>
        /// <param name="parcelNum">The ParcelNumber.</param>
        /// <param name="sourceSeqNbr">The Parcel SourceSequenceNumber.</param>
        /// <returns>Parcel PK model.</returns>
        private ParcelModel GetParcelPKModel(string parcelUID, string parcelNum, string sourceSeqNbr)
        {
            ParcelModel parcelPK = new ParcelModel();
            parcelPK.UID = parcelUID;
            parcelPK.parcelNumber = parcelNum;
            parcelPK.sourceSeqNumber = StringUtil.ToLong(sourceSeqNbr);

            return parcelPK;
        }

        /// <summary>
        /// Gets the parcel model.
        /// </summary>
        /// <param name="parcelUID">Parcel UID.</param>
        /// <param name="parcelNum">The ParcelNumber.</param>
        /// <param name="sourceSeqNbr">The Parcel SourceSequenceNumber.</param>
        /// <returns>the parcel model.</returns>
        private ParcelModel GetParcelModel(string parcelUID, string parcelNum, string sourceSeqNbr)
        {
            _parcelPK = GetParcelPKModel(parcelUID, parcelNum, sourceSeqNbr);

            ParcelModel result = null;

            IParcelBll parcelBll = (IParcelBll)ObjectFactory.GetObject(typeof(IParcelBll));
            result = parcelBll.GetParcelByPK(ConfigManager.AgencyCode, _parcelPK);

            if (result == null && Request.QueryString["childParcelNum"] != null && !string.IsNullOrEmpty(Request.QueryString["childParcelNum"].ToString()))
            {
                _parcelPK = GetParcelPKModel(parcelUID, Request.QueryString["childParcelNum"].ToString(), sourceSeqNbr);
                result = parcelBll.GetParcelByPK(ConfigManager.AgencyCode, _parcelPK);
            }

            return result;
        }

        /// <summary>
        /// fill data for address object
        /// </summary>
        /// <param name="currentPageIndex">Current Page Index.</param>
        /// <param name="sortExpression">Sort Expression.</param>
        private void LoadAddressView(int currentPageIndex, string sortExpression)
        {
            if (_parcelModal == null)
            {
                return;
            }

            PaginationModel pageInfo = PaginationUtil.GetPageInfoByID(addressList.ClientID);
            pageInfo.SortExpression = sortExpression;
            pageInfo.CurrentPageIndex = currentPageIndex;
            pageInfo.CustomPageSize = addressList.PageSize;
            QueryFormat queryFormat = PaginationUtil.GetQueryFormatModel(pageInfo);
            IAPOBll apoBll = ObjectFactory.GetObject<IAPOBll>();
            bool isForGenealogy = Request.QueryString["isForGenealogy"] != null && ACAConstant.COMMON_ONE.Equals(Request.QueryString["isForGenealogy"].ToString());
            SearchResultModel searchResult = apoBll.GetAddressListByParcelPK(ConfigManager.AgencyCode, _parcelPK, isForGenealogy, queryFormat);
            pageInfo.StartDBRow = searchResult.startRow;
            DataTable dt = APOUtil.BuildAPODataTable(searchResult.resultList);
            dt = PaginationUtil.MergeDataSource<DataTable>(addressList.RefAPODataSource, dt, pageInfo);
            addressList.ComponentType = 0;
            addressList.BindDataSource(dt);
        }

        /// <summary>
        /// Fill data for owner object
        /// </summary>
        /// <param name="currentPageIndex">Current Page Index.</param>
        /// <param name="sortExpression">Sort Expression.</param>
        private void LoadOwnerView(int currentPageIndex, string sortExpression)
        {
            if (_parcelModal == null)
            {
                return;
            }

            PaginationModel pageInfo = PaginationUtil.GetPageInfoByID(ownerList.ClientID);
            pageInfo.SortExpression = sortExpression;
            pageInfo.CurrentPageIndex = currentPageIndex;
            pageInfo.CustomPageSize = ownerList.PageSize;
            QueryFormat queryFormat = PaginationUtil.GetQueryFormatModel(pageInfo);

            IAPOBll apoBll = ObjectFactory.GetObject<IAPOBll>();
            SearchResultModel searchResult = apoBll.GetOwnerListByParcelPKs(ConfigManager.AgencyCode, new[] { _parcelPK }, false, queryFormat);

            pageInfo.StartDBRow = searchResult.startRow;

            DataTable dt = APOUtil.BuildOwnerDataTable(searchResult.resultList);
            dt = PaginationUtil.MergeDataSource<DataTable>(ownerList.OwnerDataSource, dt, pageInfo);

            ownerList.BindDataSource(dt);
        }

        /// <summary>
        /// fill data for Parcel Template Attributes
        /// </summary>
        private void LoadParcelTemplateAttributes()
        {
            ITemplateBll templateBll = ObjectFactory.GetObject<ITemplateBll>();
            TemplateAttributeModel[] attributeModels;

            if (_parcelModal == null)
            {
                return;
            }

            if (_parcelModal.templates != null && _parcelModal.templates.Length > 0)
            {
                attributeModels = _parcelModal.templates;
            }
            else
            {
                attributeModels = templateBll.GetRefAPOTemplateAttributes(TemplateType.CAP_PARCEL, _parcelModal.parcelNumber, ConfigManager.AgencyCode, AppSession.User.PublicUserId);
            }

            List<TemplateAttributeModel> availableAttributes = new List<TemplateAttributeModel>();

            // 1.Handle the label name: replace the label with name when label is empty.
            // Repeater will bind attributeLabel
            if (attributeModels != null && attributeModels.Length > 0)
            {
                foreach (TemplateAttributeModel item in attributeModels)
                {
                    if (string.IsNullOrEmpty(item.attributeLabel))
                    {
                        item.attributeLabel = item.attributeName;
                    }

                    if (!string.IsNullOrEmpty(item.vchFlag) && !ACAConstant.COMMON_N.Equals(item.vchFlag))
                    {
                        availableAttributes.Add(item);
                    }
                }
            }

            // 2.Bind data to present the attributes
            rptAttribute.DataSource = availableAttributes.ToArray();
            rptAttribute.DataBind();
        }

        /// <summary>
        /// fill parcel object data in control
        /// </summary>
        private void LoadParcelView()
        {
            string parcelNumber = _parcelModal.parcelNumber;

            if (Request.QueryString["childParcelNum"] != null && !string.IsNullOrEmpty(Request.QueryString["childParcelNum"].ToString()))
            {
                parcelNumber = Request.QueryString["childParcelNum"].ToString();
            }

            lblParcelNumber.Text = parcelNumber;
            lblParcelLot.Text = _parcelModal.lot;
            lblParcelBlock.Text = _parcelModal.block;
            lblParcelSubdivision.Text = I18nStringUtil.GetString(_parcelModal.resSubdivision, _parcelModal.subdivision);

            string displayParcleStatus = string.Empty;

            if (ACAConstant.VALID_STATUS.Equals(_parcelModal.parcelStatus, StringComparison.InvariantCultureIgnoreCase))
            {
                displayParcleStatus = GetTextByKey("aca_parcel_label_statusenable");
            }
            else if (ACAConstant.INVALID_STATUS.Equals(_parcelModal.parcelStatus, StringComparison.InvariantCultureIgnoreCase))
            {
                displayParcleStatus = GetTextByKey("aca_parcel_label_statusdisabled");
            }
            else if (PendingParcel.Equals(_parcelModal.parcelStatus, StringComparison.InvariantCultureIgnoreCase))
            {
                displayParcleStatus = GetTextByKey("aca_parcel_label_statuspending");
            }

            lblStatus.Text = displayParcleStatus;
            lblParcelBook.Text = _parcelModal.book;
            lblParcelPage.Text = _parcelModal.page;
            lblParcelArea.Text = Convert.ToString(_parcelModal.parcelArea);
            lblParcelLand.Text = Convert.ToString(_parcelModal.landValue);
            lblParcelImproved.Text = Convert.ToString(_parcelModal.improvedValue);
            lblParcelExemption.Text = Convert.ToString(_parcelModal.exemptValue);
            lblParcelLegal.Text = _parcelModal.legalDesc;
            lblparcelTract.Text = _parcelModal.tract;
        }

        #endregion Methods
    }
}
