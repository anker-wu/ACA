#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: APOLookUp.aspx.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2007-2014
 *
 *  Description:
 *
 *  Notes:
 *      $Id: APOLookUp.aspx.cs 278214 2014-08-29 06:14:02Z ACHIEVO\james.shi $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

using Accela.ACA.BLL.APO;
using Accela.ACA.BLL.Cap;
using Accela.ACA.BLL.Common;
using Accela.ACA.BLL.People;
using Accela.ACA.Common;
using Accela.ACA.Common.Util;
using Accela.ACA.Web.AppCode.Common;
using Accela.ACA.Web.Common;
using Accela.ACA.Web.Common.Control;
using Accela.ACA.WSProxy;
using Accela.Web.Controls;

namespace Accela.ACA.Web.APO
{
    /// <summary>
    /// APO lookup page
    /// </summary>
    public partial class APOLookUp : BasePage
    {
        #region Fields

        /// <summary>
        /// parcel id mask
        /// </summary>
        protected const string PARCEL_ID_MASK = "parcelIdMask";

        /// <summary>
        /// Command constant "selected license".
        /// </summary>
        private const string COMMAND_SELECTED_LICENSEE = "selectedLicensee";

        /// <summary>
        /// view state constant 
        /// </summary>
        private const string LICENSE_PROFESSIONAL_SOURCE = "LicenseProfessionalSource";

        /// <summary>
        /// is masked or not
        /// </summary>
        private bool _isMasked;

        /// <summary>
        /// the general search model
        /// </summary>
        private GeneralSearchModel _generalSearchModel = null;

        /// <summary>
        /// whether exist the template columns or not
        /// </summary>
        private bool? _isExistTemplateColumn = null;

        /// <summary>
        /// the APO BLL
        /// </summary>
        private IAPOBll _apoBll = ObjectFactory.GetObject<IAPOBll>();

        /// <summary>
        /// the CAP BLL
        /// </summary>
        private ICapBll _capBll = ObjectFactory.GetObject<ICapBll>();

        /// <summary>
        /// the License BLL
        /// </summary>
        private ILicenseBLL _licenseBll = ObjectFactory.GetObject<ILicenseBLL>();

        #endregion Fields

        #region Properties

        /// <summary>
        /// Gets a value indicating whether the Parcel Id whether need to be displayed as mask.
        /// </summary>
        protected bool IsMasked
        {
            get
            {
                return _isMasked;
            }
        }

        /// <summary>
        /// Gets or sets the parcel id mask.
        /// </summary>
        protected string ParcelIDMask
        {
            get
            {
                if (ViewState[PARCEL_ID_MASK] == null)
                {
                    ViewState[PARCEL_ID_MASK] = string.Empty;
                }
                else
                {
                    _isMasked = true;
                }

                return ViewState[PARCEL_ID_MASK].ToString();
            }

            set
            {
                ViewState[PARCEL_ID_MASK] = value;
            }
        }

        /// <summary>
        /// Gets Selected Module.
        /// </summary>
        private string SelectedModule
        {
            get
            {
                string selectedModule = string.Empty;

                if (ddlModule.Visible == false && ddlModule.Items.Count == 2)
                {
                    selectedModule = ddlModule.Items[1].Value;
                }

                if (ddlModule.Visible && ddlModule.SelectedItem != null)
                {
                    selectedModule = ddlModule.SelectedItem.Value;
                }

                return selectedModule;
            }
        }

        /// <summary>
        /// Gets all phone control id.
        /// </summary>
        private string RelevantControlIDs
        {
            get
            {
                StringBuilder sbControls = new StringBuilder();
                sbControls.Append(txtPhone1.ClientID);
                sbControls.Append(",").Append(txtPhone2.ClientID);
                sbControls.Append(",").Append(txtFax.ClientID);

                return sbControls.ToString();
            }
        }

        /// <summary>
        /// Gets all phone control id.
        /// </summary>
        private string RelevantOwnerPhoneIDs
        {
            get
            {                
                StringBuilder sbControls = new StringBuilder();
                sbControls.Append(ownerLookupForm.OwnerPhoneID);
                sbControls.Append(",").Append(ownerLookupForm.OwnerFaxID);

                return sbControls.ToString();
            }
        }

        /// <summary>
        /// Gets or sets the permit data source.
        /// </summary>
        /// <value>The permit data source.</value>
        private DataTable PermitDataSource
        {
            get
            {
                return ViewState["PermitSource"] as DataTable;
            }

            set
            {
                ViewState["PermitSource"] = value;
            }
        }

        /// <summary>
        /// Gets or sets the license professional data source.
        /// </summary>
        /// <value>The license professional data source.</value>
        private IList<LicenseProfessionalModel> LPDataSource
        {
            get
            {
                return ViewState[LICENSE_PROFESSIONAL_SOURCE] as IList<LicenseProfessionalModel>;
            }

            set
            {
                ViewState[LICENSE_PROFESSIONAL_SOURCE] = value;
            }
        }

        /// <summary>
        /// Gets or sets the reference parcel model.
        /// Include reference parcel number, parcel unique id, source Sequence Number.
        /// </summary>
        private ParcelModel ParcelPK
        {
            get
            {
                return ViewState["ParcelPK"] as ParcelModel;
            }

            set
            {
                ViewState["ParcelPK"] = value;
            }
        }

        /// <summary>
        /// Gets or sets the reference address model.
        /// Include refAddressID, refAddressUID, source Sequence Number.
        /// </summary>
        private RefAddressModel RefAddressPK
        {        
            get
            {
                return ViewState["RefAddressPK"] as RefAddressModel;
            }

            set
            {
                ViewState["RefAddressPK"] = value;
            }
        }

        /// <summary>
        /// Gets or sets the reference owner model.
        /// </summary>
        private OwnerModel RefOwnerPK
        {
            get
            {
                return ViewState["RefOwnerPK"] as OwnerModel;
            }

            set
            {
                ViewState["RefOwnerPK"] = value;
            }
        }

        /// <summary>
        /// Gets or sets APO query Info.
        /// </summary>
        private ApoQueryInfo ApoQueryInfo
        {
            get
            {
                ApoQueryInfo info = (ApoQueryInfo)Session[SessionConstant.SESSION_APO_QUERY];

                if (info == null)
                {
                    info = new ApoQueryInfo();
                    Session[SessionConstant.SESSION_APO_QUERY] = info;
                }

                return info;
            }

            set
            {
                Session[SessionConstant.SESSION_APO_QUERY] = value;
            }
        }

        /// <summary>
        /// Gets the general search model
        /// </summary>
        private GeneralSearchModel GeneralSearchModel
        {
            get
            {
                if (_generalSearchModel != null)
                {
                    return _generalSearchModel;
                }

                // 1. get permit information
                CapModel4WS capModel = new CapModel4WS();
                capModel.altID = txtAPO_Search_by_Permit_PermitNumber.Text.Trim();
                capModel.specialText = txtAPO_Search_by_Permit_ProjectName.Text.Trim();

                if (txtAPO_Search_by_Permit_StartDate.Visible && !string.IsNullOrEmpty(txtAPO_Search_by_Permit_StartDate.Text.Trim()))
                {
                    capModel.fileDate = I18nDateTimeUtil.ConvertDateTimeStringFromUIToWebService(txtAPO_Search_by_Permit_StartDate.Text.Trim());
                }

                if (txtAPO_Search_by_Permit_EndDate.Visible && !string.IsNullOrEmpty(txtAPO_Search_by_Permit_EndDate.Text.Trim()))
                {
                    capModel.endFileDate = I18nDateTimeUtil.FormatToDateTimeStringForWebService(I18nDateTimeUtil.ParseFromUI(txtAPO_Search_by_Permit_EndDate.Text.Trim()).AddDays(1).AddSeconds(-1));
                }

                capModel.moduleName = SelectedModule;

                // get selected CAP Type
                capModel.capType = CapUtil.GetCAPTypeModelByString(capModel.moduleName, ddlAPO_Search_by_Permit_Permit.SelectedValue, ddlAPO_Search_by_Permit_Permit.SelectedItem.Text);

                // 2. get module name list
                List<string> moduleNameList = null;
                if (string.IsNullOrEmpty(capModel.moduleName))
                {
                    Dictionary<string, string> allModules = TabUtil.GetAllEnableModules(false);
                    if (allModules != null && allModules.Count > 0)
                    {
                        moduleNameList = allModules.Keys.ToList();
                    }
                }

                // 3. get the hidden view element name
                string[] hiddenViewElementNames = ControlBuildHelper.GetHiddenViewElementNames(GviewID.LookUpByPermit, ModuleName);

                // 4. set the general search model
                _generalSearchModel = new GeneralSearchModel();
                _generalSearchModel.CapModel = capModel;
                _generalSearchModel.ModuleNameList = moduleNameList;
                _generalSearchModel.HiddenViewEltNames = hiddenViewElementNames;

                return _generalSearchModel;
            }
        }

        /// <summary>
        /// Gets a value indicating whether it exists template column of the <c>gdvLicenseeList</c>.
        /// </summary>
        private bool IsExistTemplateColumn
        {
            get
            {
                return _isExistTemplateColumn != null ? _isExistTemplateColumn.Value : GridViewBuildHelper.IsExistTemplateColumn(gdvLicenseeList);
            }
        }

        #endregion Properties

        #region Public/Protected Methods

        /// <summary>
        /// Get instruction by label key
        /// </summary>
        /// <param name="labelKey">label key string.</param>
        /// <returns>instruction value</returns>
        [System.Web.Services.WebMethod(Description = "Get value by label key", EnableSession = true)]
        public static string GetInstructionByKey(string labelKey)
        {
            return GetStaticTextByKey(labelKey);
        }

        /// <summary>
        /// Raises the <c>System.Web.UI.Control.Init</c> event.
        /// </summary>
        /// <param name="e">An System.EventArgs object that contains the event data.</param>
        protected override void OnInit(EventArgs e)
        {
            GridViewBuildHelper.InitializeGridWithTemplate(gdvLicenseeList, ModuleName, BizDomainConstant.STD_CAT_LICENSE_TYPE);
            GridViewBuildHelper.SetSimpleViewElements(dgvPermitList, ModuleName, AppSession.IsAdmin);
            RefAPOAddressList1.GridViewDownloadAll += RefAPOAddressList1_GridViewDownloadAll;
            RefAddressLookUpList.GridViewDownloadAll += RefAPOAddressList1_GridViewDownloadAll;
            RefParcelLookUpList.GridViewDownloadAll += RefAPOAddressList1_GridViewDownloadAll;
            RefOwnerLookUpList.GridViewDownloadAll += RefAPOAddressList1_GridViewDownloadAll;
            AssociatedAddressList.GridViewDownloadAll += AssociatedAddressList_GridViewDownloadAll;
            AssociatedParcelList.GridViewDownloadAll += AssociatedParcelList_GridViewDownloadAll;
            AssociatedOwnerList.GridViewDownloadAll += AssociatedOwnerList_GridViewDownloadAll;
            AssociatedAPOList.GridViewDownloadAll += AssociatedAPOList_GridViewDownloadAll;
            RefAddressLookUpList.AddressSelected += AddressList_AddressSelected;
            RefParcelLookUpList.ParcelSelected += ParcelList_ParcelSelected;
            RefOwnerLookUpList.OwnerRetrieved += OwnerList_OwnerRetrieved;
            AssociatedParcelList.ParcelSelected += ParcelList_ParcelSelected;

            ddlCountry.BindItems();
            ddlCountry.SetCountryControls(txtZipCode, txtState, txtPhone1, txtPhone2, txtFax);
            
            ddlCountry.RelevantControlIDs = RelevantControlIDs;
            ddlCountry.RegisterScripts();

            InitalExport();
            base.OnInit(e);
        }

        /// <summary>
        /// Raises the page load event
        /// </summary>
        /// <param name="sender">An object that contains the event sender.</param>
        /// <param name="e">A System.EventArgs object containing the event data.</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            ControlUtil.ApplyRegionalSetting(IsPostBack, true, true, false, ddlCountry);

            //set the caphome page no cache.
            Response.Cache.SetCacheability(System.Web.HttpCacheability.NoCache);
            Response.Cache.SetNoStore();

            if (!IsPostBack)
            {
                BindSearchList();
                InitPage();

                if (ddlSearchType.Items.Count > 0)
                {
                    SetDivIsVisible();
                }

                BindDropdownList();

                IBizDomainBll bizBll = ObjectFactory.GetObject<IBizDomainBll>();

                IList<ItemValue> items = bizBll.GetBizDomainList(ConfigManager.AgencyCode, BizDomainConstant.STD_CAT_PARCEL_MASK, false);

                if (items.Count > 1)
                {
                    foreach (ItemValue item in items)
                    {
                        if (item.Key == "Parcel_ID")
                        {
                            ParcelIDMask = item.Value.ToString();
                        }
                    }
                }
                else
                {
                    ParcelIDMask = string.Empty;
                }

                if (!AppSession.IsAdmin && ApoQueryInfo != null)
                {
                    if (ApoQueryInfo.IsFromSearchPage)
                    {
                        ReloadData();
                        ApoQueryInfo.IsFromSearchPage = false;
                    }
                    else
                    {
                        InitStatusControl();
                    }
                }
                else if (AppSession.IsAdmin)
                {
                    dgvPermitList.Visible = true;
                    divPageResult.Visible = true;
                    divPagePermitListResult.Visible = true;
                    dgvPermitList.DataSource = APOUtil.BuildAPODataTable(null);
                    dgvPermitList.DataBind();

                    gdvLicenseeList.Visible = true;
                    gdvLicenseeList.DataSource = new List<LicenseModel4WS>();
                    gdvLicenseeList.DataBind();
                    divLicenseeResult.Visible = true;

                    divResultPermitList.Visible = true;
                    divResultLicenseeList.Visible = true;
                    RefAPOAddressList1.Visible = true;
                    RefAPOAddressList1.BindDataSource(APOUtil.BuildAPODataTable(null));
                    RefAddressLookUpList.Visible = true;
                    RefAddressLookUpList.BindDataSource(null);
                    RefParcelLookUpList.Visible = true;
                    RefParcelLookUpList.BindDataSource(null);
                    RefOwnerLookUpList.Visible = true;
                    RefOwnerLookUpList.BindDataSource(null);

                    divAssociatedAddress.Visible = true;
                    AssociatedAddressList.BindDataSource(null);
                    divAssociatedParcel.Visible = true;
                    AssociatedParcelList.BindDataSource(null);
                    divAssociatedOwner.Visible = true;
                    AssociatedOwnerList.BindDataSource(null);
                    divAssociatedAPOList.Visible = true;
                    AssociatedAPOList.BindDataSource(null);
                }

                if (!AppSession.IsAdmin)
                {
                    if (string.IsNullOrEmpty(lblSearchInstruction.Text))
                    {
                        divInstruction.Visible = false;
                    }
                }
            }
            else
            {
                if (PermitDataSource != null)
                {
                    //ApoQueryInfo ApoQueryInfo = (ApoQueryInfo)Session[SessionConstant.SESSION_APO_QUERY];
                    if (ApoQueryInfo != null && ApoQueryInfo.APOPermitSource != null)
                    {
                        DataView dataView = new DataView(ApoQueryInfo.APOPermitSource);
                        dataView.Sort = ApoQueryInfo.APOPermitPageSort;
                        dgvPermitList.DataSource = dataView.ToTable();
                        dgvPermitList.DataBind();
                    }
                    else
                    {
                        dgvPermitList.DataSource = PermitDataSource;
                        dgvPermitList.DataBind();
                    }
                }

                if (LPDataSource != null)
                {
                    if (ApoQueryInfo != null && ApoQueryInfo.APOLicenseeSource != null)
                    {
                        BindSessionLicenseeList(ApoQueryInfo);
                    }
                    else
                    {
                        DataTable dt = GridViewBuildHelper.MergeTemplateAttributes2DataTable(gdvLicenseeList, LPDataSource);
                        gdvLicenseeList.DataSource = dt;
                        gdvLicenseeList.DataBind();
                    }
                }

                //hide this button when in admin mode
                btnNewSearch.Visible = !AppSession.IsAdmin;
            }

            if (!AppSession.IsAdmin && !IsPostBack)
            {
                SetCurrentCityAndState();
            }

            if (AppSession.IsAdmin)
            {
                dvSearchByAddress.Visible = true;
                dvSearchByParcel.Visible = true;
                dvSearchByOwner.Visible = true;
                dvSearchByPermit.Visible = true;
                dvSearchByLicensee.Visible = true;
                ddlSearchType.AutoPostBack = false;
                ddlSearchType.Attributes.Add("onchange", "ChangeType(this)");

                ddlModule.AutoPostBack = false;
                ddlAPO_Search_by_Permit_Permit.AutoPostBack = false;
            }
        }

        /// <summary>
        /// Raise PreRender Event.
        /// </summary>
        /// <param name="sender">the sender object</param>
        /// <param name="e">the event handle</param>
        protected void Page_PreRender(object sender, EventArgs e)
        {
            if (!AppSession.IsAdmin)
            {
                SearchType searchType = (SearchType)int.Parse(ddlSearchType.SelectedValue.Replace("-", string.Empty));

                switch (searchType)
                {
                    case SearchType.Address:
                        if (ApoQueryInfo.APOAddressSource != null)
                        {
                            ShowResultInfo(ApoQueryInfo.APOAddressSource.Rows.Count, RefAddressLookUpList.CountSummary, false);
                        }
             
                        break;
                    case SearchType.Parcel:
                        if (ApoQueryInfo.APOParcelSource != null)
                        {
                            ShowResultInfo(ApoQueryInfo.APOParcelSource.Rows.Count, RefParcelLookUpList.CountSummary, false);
                        }

                        break;
                    case SearchType.Owner:
                        if (ApoQueryInfo.APOOwnerSource != null)
                        {
                            ShowResultInfo(ApoQueryInfo.APOOwnerSource.Rows.Count, RefOwnerLookUpList.CountSummary, false);
                        }

                        break;
                    default:
                        if (ApoQueryInfo.APOAddressSource != null)
                        {
                            ShowResultInfo(ApoQueryInfo.APOAddressSource.Rows.Count, RefAPOAddressList1.CountSummary, false);
                        }

                        break;
                }
            }
        }

        /// <summary>
        /// Raise RestSearchButton_Click event
        /// </summary>
        /// <param name="sender">the sender object</param>
        /// <param name="e">the event handle</param>
        protected void ResetSearchButton_Click(object sender, EventArgs e)
        {
            try
            {
                divPagePermitListResult.Visible = false;
                divLicenseeResult.Visible = false;
                divAPOResult.Visible = false;
                divPageResult.Visible = false;

                RefAPOAddressList1.CloseMap();
                MessageUtil.HideMessageByControl(Page);
                ControlUtil.ClearValue(this, new[] { ddlSearchType.ID });
                InitPage();

                SearchType searchType = (SearchType)int.Parse(ddlSearchType.SelectedValue.Replace("-", string.Empty));

                switch (searchType)
                {
                    case SearchType.Address:
                        addressLookupForm.ClearRegionalSetting();
                        break;
                    case SearchType.Owner:
                        ownerLookupForm.ClearRegionalSetting();
                        break;
                    case SearchType.License:
                        ControlUtil.ClearRegionalSetting(ddlCountry, true, string.Empty, null, string.Empty);
                        break;
                }

                ScriptManager.RegisterClientScriptBlock(Page, GetType(), "GotoSearchForm_Start", "scrollIntoView('SearchForm_Start');", true);
            }
            catch (ACAException ex)
            {
                MessageUtil.ShowMessageByControl(Page, MessageType.Error, ex.Message);

                //hidden result list or not found message.
                InitDisplay();
            }
        }

        /// <summary>
        /// Raises "new search" button click event
        /// </summary>
        /// <param name="sender">An object that contains the event sender.</param>
        /// <param name="e">A System.EventArgs object containing the event data.</param>
        protected void NewSearchButton_Click(object sender, EventArgs e)
        {
            try
            {
                RefAPOAddressList1.RefAPODataSource = null;
                RefAddressLookUpList.RefAddressDataSource = null;
                RefParcelLookUpList.RefParcelDataSource = null;
                RefOwnerLookUpList.OwnerDataSource = null;
                AssociatedAddressList.RefAddressDataSource = null;
                AssociatedParcelList.RefParcelDataSource = null;
                AssociatedOwnerList.OwnerDataSource = null;
                AssociatedAPOList.RefAPODataSource = null;
                PermitDataSource = null;
                LPDataSource = null;
                SearchType searchType = (SearchType)int.Parse(ddlSearchType.SelectedValue.Replace("-", string.Empty));
                ShowDisplay(searchType);

                //Set the Address or Parcel associated data section not visible if operate new search.
                divAssociatedAddress.Visible = false;
                divAssociatedParcel.Visible = false;
                divAssociatedOwner.Visible = false;
                divAssociatedAPOList.Visible = false;

                switch (searchType)
                {
                    case SearchType.Address:
                       
                        // Check empty input on external source.
                        if (ValidateIsEmptyForExternalSearchCondition(SearchType.Address))
                        {
                            return;
                        }

                        RefAddressLookUpList.CloseMap();
                        SearchRefAddress(0, null, true);
                        break;

                    case SearchType.Parcel:

                        // Check empty input on external source.
                        if (ValidateIsEmptyForExternalSearchCondition(SearchType.Parcel))
                        {
                            return;
                        }

                        RefParcelLookUpList.CloseMap();
                        SearchRefParcel(0, null, true);
                        break;

                    case SearchType.Owner:

                        // Check empty input on external source.
                        if (ValidateIsEmptyForExternalSearchCondition(SearchType.Owner))
                        {
                            return;
                        }

                        SearchRefOwner(0, null, true);
                        break;

                    case SearchType.Permit:
                        ////Check input condition is validate.
                        if (!CheckInputConditionForPermit())
                        {
                            RecordSearchResultInfo.Hide();
                            divResultPermitList.Visible = false;
                            divPagePermitListResult.Visible = false;

                            return;
                        }
                        else
                        {
                            MessageUtil.HideMessageByControl(Page);
                            dgvPermitList.Visible = true;
                        }

                        RefAPOAddressList1.CloseMap();
                        SearchPermit(0, null);
                        break;
                    case SearchType.License:
                        ApoQueryInfo.APOLicenseePageIndex = 0;
                        gdvLicenseeList.PageIndex = 0;
                        RefAPOAddressList1.CloseMap();
                        SearchLicensees(0, null);
                        break;
                }

                Page.FocusElement(btnNewSearch.ClientID);

                MessageUtil.HideMessageByControl(Page);
                ScriptManager.RegisterStartupScript(Page, GetType(), "GotoPageResult", "scrollIntoView('PageResult');", true);
            }
            catch (ACAException ex)
            {
                MessageUtil.ShowMessageByControl(Page, MessageType.Error, ex.Message);
                ScriptManager.RegisterStartupScript(Page, GetType(), "GotoMessageBar", "scrollIntoView('messageSpan');", true);

                //hidden result list or not found message.
                InitDisplay();
            }
        }

        /// <summary>
        /// Handles the IndexChanged event of the <c>ddlSearchType</c> control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        protected void SearchTypeDropdown_IndexChanged(object sender, EventArgs e)
        {
            if (ddlSearchType.Items.Count < 1)
            {
                return;
            }

            string selectedValue = ddlSearchType.SelectedValue;

            if (AppSession.IsAdmin)
            {
                //Re-bind search type drop-down to get the latest values.
                ddlSearchType.Items.Clear();
                BindSearchList();
                ddlSearchType.SetValue(selectedValue);
            }

            InitPage();
            SetDivIsVisible();
            InitDisplay();
            InitStatusControl();
            InitDataSource();

            if (AppSession.IsAdmin)
            {
                ddlSearchType.StdCategory = BizDomainConstant.STD_CAT_APO_LOOKUP_TYPE;
                BindAllDropdownList();
            }

            if (!AppSession.IsAdmin)
            {
                //Need clear all the validate message on current page after changing search type.
                MessageUtil.HideMessageByControl(Page);
                ControlUtil.ApplyRegionalSetting(IsPostBack, true, true, false, ddlCountry);
                addressLookupForm.ApplyRegionalSetting(false);
                ownerLookupForm.ApplyRegionalSetting(false);
                SetCurrentCityAndState();

                if (string.IsNullOrEmpty(lblSearchInstruction.Text))
                {
                    divInstruction.Visible = false;
                }
                else
                {
                    divInstruction.Visible = true;
                }
            }
        }
        
        /// <summary>
        /// Raise grid view sort event.
        /// </summary>
        /// <param name="sender">An object that contains the event sender.</param>
        /// <param name="e">An <c>AccelaGridViewSortEventArgsobject</c> that contains the event data.</param>
        protected void PermitList_GridViewSort(object sender, AccelaGridViewSortEventArgs e)
        {
            if (ApoQueryInfo != null)
            {
                ApoQueryInfo.APOPermitPageSort = e.GridViewSortExpression;
                PaginationModel pageInfo = PaginationUtil.GetPageInfoByID(dgvPermitList.ClientID);
                pageInfo.SortExpression = e.GridViewSortExpression;
            }
        }

        /// <summary>
        /// Raises grid view page index change event
        /// </summary>
        /// <param name="sender">An object that contains the event sender.</param>
        /// <param name="e">An <c>AccelaGridViewSortEventArgsobject</c> that contains the event data.</param>
        protected void PermitList_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            if (ApoQueryInfo != null)
            {
                ApoQueryInfo.APOPermitPageIndex = e.NewPageIndex;
                PaginationModel pageInfo = PaginationUtil.GetPageInfoByID(dgvPermitList.ClientID);

                if (e.NewPageIndex > pageInfo.EndPage)
                {
                    SearchPermit(e.NewPageIndex, pageInfo.SortExpression);
                }
            }
        }

        /// <summary>
        /// Search APO list by CapIDModel
        /// </summary>
        /// <param name="sender">An object that contains the event sender.</param>
        /// <param name="e">An <c>AccelaGridViewSortEventArgsobject</c> that contains the event data.</param>
        protected void PermitList_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "DisplayAPO")
            {
                e.FocusRowCellByName("hlPermitNumber");
                string[] capIDS = e.CommandArgument.ToString().Split(',');
                RefAPOAddressList1.RefAPODataSource = null;
                RefAPOAddressList1.ClearSelectedItems();
                RefAPOAddressList1.CloseMap();
                SearchAPOListByCap(capIDS[0], capIDS[1], capIDS[2], capIDS[3], 0, null);
            }
        }

        /// <summary>
        /// Raises grid view row bound event
        /// </summary>
        /// <param name="sender">An object that contains the event sender.</param>
        /// <param name="e">An <c>AccelaGridViewSortEventArgsobject</c> that contains the event data.</param>
        protected void PermitList_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                DataRowView rowView = (DataRowView)e.Row.DataItem;

                string capID1 = (string)rowView["capID1"];
                string capID2 = (string)rowView["capID2"];
                string capID3 = (string)rowView["capID3"];
                string agencyCode = rowView["AgencyCode"].ToString();
                string moduleName = (string)rowView["ModuleName"];

                LinkButton hlPermitNumber = (LinkButton)e.Row.FindControl("hlPermitNumber");
                hlPermitNumber.CommandArgument = capID1 + "," + capID2 + "," + capID3 + "," + agencyCode;
                Literal litRelatedRecords = e.Row.FindControl("litRelatedRecords") as Literal;
                if (litRelatedRecords != null)
                {
                    int relatedRecords = (int)rowView["RelatedRecords"];
                    if (relatedRecords > 0)
                    {
                        Random rd = new Random();
                        string lnkRelatedRecordID = Convert.ToString(rd.Next());
                        byte[] args = Encoding.UTF8.GetBytes(string.Format("{1}{0}{2}{0}{3}{0}{4}{0}{5}", ACAConstant.SPLIT_CHAR, capID1, capID2, capID3, agencyCode, moduleName));
                        string url = string.Format(ResolveUrl("~/Cap/RelatedRecords.aspx?module={0}&args={1}"), moduleName, HttpUtility.UrlEncode(Convert.ToBase64String(args)));
                        string showRelatedJs = "ACADialog.popup({url:'" + url + "',width:800,height:500,objectTarget:'" + lnkRelatedRecordID + "'});";
                        litRelatedRecords.Text = string.Format("<a id='{2}' href=\"javascript:{0}\">{1}</a> ", showRelatedJs, relatedRecords, lnkRelatedRecordID);
                    }
                    else
                    {
                        litRelatedRecords.Text = relatedRecords.ToString();
                    }
                }
            }
        }

        /// <summary>
        /// Handles the GridViewDownload event of the permit list.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="Accela.Web.Controls.GridViewDownloadEventArgs"/> instance containing the event data.</param>
        protected void PermitList_GridViewDownload(object sender, GridViewDownloadEventArgs e)
        {
            GridViewBuildHelper.DownloadAll(sender, e, GetPermitByQueryFormat);
        }

        /// <summary>
        /// Raises grid view row bound event
        /// </summary>
        /// <param name="sender">An object that contains the event sender.</param>
        /// <param name="e">An <c>AccelaGridViewSortEventArgsobject</c> that contains the event data.</param>
        protected void LicenseeList_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                DataRow row = ((DataRowView)e.Row.DataItem).Row;

                LinkButton hlPermitNumber = (LinkButton)e.Row.FindControl("lnkLicenseRefNumber");
                hlPermitNumber.CommandArgument = row["licenseNbr"] + "," + row["licenseType"];
            }
        }

        /// <summary>
        /// Handles the GridViewDownload event of the licensee list.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="Accela.Web.Controls.GridViewDownloadEventArgs"/> instance containing the event data.</param>
        protected void LicenseeList_GridViewDownload(object sender, GridViewDownloadEventArgs e)
        {
            GridViewBuildHelper.DownloadAll(sender, e, GetLicenseesByQueryFormat, LicenseesExportFormat);
        }

        /// <summary>
        /// fire sorted event.
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void LicenseeList_GridViewSort(object sender, AccelaGridViewSortEventArgs e)
        {
            if (ApoQueryInfo != null)
            {
                ApoQueryInfo.APOLicenseePageSort = e.GridViewSortExpression;
                PaginationModel pageInfo = PaginationUtil.GetPageInfoByID(gdvLicenseeList.ClientID);
                pageInfo.SortExpression = e.GridViewSortExpression;
            }
        }

        /// <summary>
        /// page index changing event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void LicenseeList_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            if (ApoQueryInfo != null)
            {
                ApoQueryInfo.APOLicenseePageIndex = e.NewPageIndex;
                PaginationModel pageInfo = PaginationUtil.GetPageInfoByID(gdvLicenseeList.ClientID);

                if (e.NewPageIndex > pageInfo.EndPage)
                {
                     SearchLicensees(e.NewPageIndex, pageInfo.SortExpression);
                }
            }
        }

        /// <summary>
        /// <c>gdvLicenseeList</c> row command event method.
        /// </summary>
        /// <param name="sender">An object that contains the event sender.</param>
        /// <param name="args">A System.EventArgs object containing the event data.</param>
        protected void LicenseeList_RowCommand(object sender, GridViewCommandEventArgs args)
        {
            //if e is null, not process
            if (args == null || args.CommandArgument == null)
            {
                return;
            }

            if (args.CommandName == COMMAND_SELECTED_LICENSEE)
            {
                args.FocusRowCellByName("lnkLicenseRefNumber");
                string[] searchParams = args.CommandArgument.ToString().Split(',');
                RefAPOAddressList1.RefAPODataSource = null;
                RefAPOAddressList1.ClearSelectedItems();
                RefAPOAddressList1.CloseMap();
                SearchAPOListByLP(searchParams[0], searchParams[1], 0, null);
            }
            else
            {
                DataTable dt = GridViewBuildHelper.MergeTemplateAttributes2DataTable(gdvLicenseeList, ApoQueryInfo.APOLicenseeSource);
                gdvLicenseeList.DataSource = dt;
                gdvLicenseeList.DataBind();
            }
        }

        /// <summary>
        /// RefAPOAddressList1 page index changing event.
        /// </summary>
        /// <param name="sender">An object that contains the event sender.</param>
        /// <param name="e">A GridViewPageEventArgs object containing the event data.</param>
        protected void RefAPO_GridViewIndexChanging(object sender, GridViewPageEventArgs e)
        {
            SearchType searchType = (SearchType)ApoQueryInfo.APOSearchTypeIndex;
            PaginationModel pageInfo = new PaginationModel();

            switch (searchType)
            {
                case SearchType.Address:
                    pageInfo = PaginationUtil.GetPageInfoByID(RefAddressLookUpList.ClientID + searchType);
                    break;
                case SearchType.Parcel:
                    pageInfo = PaginationUtil.GetPageInfoByID(RefParcelLookUpList.ClientID + searchType);
                    break;
                case SearchType.Owner:
                    pageInfo = PaginationUtil.GetPageInfoByID(RefOwnerLookUpList.ClientID + searchType);
                    break;
                default:
                    pageInfo = PaginationUtil.GetPageInfoByID(RefAPOAddressList1.ClientID + searchType);
                    break;
            }

            if (e.NewPageIndex > pageInfo.EndPage)
            {
                switch (searchType)
                {
                    case SearchType.Address:
                        SearchRefAddress(e.NewPageIndex, pageInfo.SortExpression, false);
                        break;

                    case SearchType.Parcel:
                        SearchRefParcel(e.NewPageIndex, pageInfo.SortExpression, false);
                        break;

                    case SearchType.Owner:
                        SearchRefOwner(e.NewPageIndex, pageInfo.SortExpression, false);
                        break;

                    case SearchType.License:
                        SearchAPOListByLP((string)pageInfo.SearchCriterias[0], (string)pageInfo.SearchCriterias[1], e.NewPageIndex, pageInfo.SortExpression);
                        break;

                    case SearchType.Permit:
                        SearchAPOListByCap((string)pageInfo.SearchCriterias[0], (string)pageInfo.SearchCriterias[1], (string)pageInfo.SearchCriterias[2], (string)pageInfo.SearchCriterias[3], e.NewPageIndex, pageInfo.SortExpression);
                        break;
                }
            }
        }

        /// <summary>
        /// Parcel associated address list index change
        /// </summary>
        /// <param name="sender">An object that contains the event sender.</param>
        /// <param name="e">A System.EventArgs object containing the event data.</param>
        protected void AssociatedAddress_GridViewIndexChanging(object sender, GridViewPageEventArgs e)
        {
            PaginationModel pageInfo = PaginationUtil.GetPageInfoByID(AssociatedAddressList.ClientID);

            if (e.NewPageIndex > pageInfo.EndPage)
            {
                LoadAssociatedAddressData(e.NewPageIndex, pageInfo.SortExpression);
            }
        }

        /// <summary>
        /// Address associated parcel List index change
        /// </summary>
        /// <param name="sender">An object that contains the event sender.</param>
        /// <param name="e">A System.EventArgs object containing the event data.</param>
        protected void AssociatedParcel_GridViewIndexChanging(object sender, GridViewPageEventArgs e)
        {
            PaginationModel pageInfo = PaginationUtil.GetPageInfoByID(AssociatedParcelList.ClientID);

            if (e.NewPageIndex > pageInfo.EndPage)
            {
                DataTable parcelTable = GetAssociatedParcelDataTable(e.NewPageIndex, pageInfo.SortExpression);
                LoadAssociatedParcelData(parcelTable, string.Empty);
            }
        }

        /// <summary>
        /// Parcel associated owner List index change
        /// </summary>
        /// <param name="sender">An object that contains the event sender.</param>
        /// <param name="e">A System.EventArgs object containing the event data.</param>
        protected void AssociatedOwner_GridViewIndexChanging(object sender, GridViewPageEventArgs e)
        {
            PaginationModel pageInfo = PaginationUtil.GetPageInfoByID(AssociatedOwnerList.ClientID);

            if (e.NewPageIndex > pageInfo.EndPage)
            {
                LoadAssociatedOwnerData(e.NewPageIndex, pageInfo.SortExpression);
            }
        }

        /// <summary>
        /// Owner associated APO List index change
        /// </summary>
        /// <param name="sender">An object that contains the event sender.</param>
        /// <param name="e">A System.EventArgs object containing the event data.</param>
        protected void AssociatedAPOList_GridViewIndexChanging(object sender, GridViewPageEventArgs e)
        {
            PaginationModel pageInfo = PaginationUtil.GetPageInfoByID(AssociatedAPOList.ClientID);

            if (e.NewPageIndex > pageInfo.EndPage)
            {
                LoadAssociatedAPOData(e.NewPageIndex, pageInfo.SortExpression);
            }
        }

        /// <summary>
        /// PermitList GridViewSort
        /// </summary>
        /// <param name="sender">An object that contains the event sender.</param>
        /// <param name="e">A System.EventArgs object containing the event data.</param>
        protected void RefAPO_GridViewSort(object sender, AccelaGridViewSortEventArgs e)
        {
            SearchType searchType = (SearchType)ApoQueryInfo.APOSearchTypeIndex;
            PaginationModel pageInfo = new PaginationModel();

            switch (searchType)
            {
                case SearchType.Address:
                    pageInfo = PaginationUtil.GetPageInfoByID(RefAddressLookUpList.ClientID + searchType);
                    break;
                case SearchType.Parcel:
                    pageInfo = PaginationUtil.GetPageInfoByID(RefParcelLookUpList.ClientID + searchType);
                    break;
                case SearchType.Owner:
                    pageInfo = PaginationUtil.GetPageInfoByID(RefOwnerLookUpList.ClientID + searchType);
                    break;
                default:
                    pageInfo = PaginationUtil.GetPageInfoByID(RefAPOAddressList1.ClientID + searchType);
                    break;
            }

            pageInfo.SortExpression = e.GridViewSortExpression;
        }

        /// <summary>
        /// Parcel Associated Address List GridViewSort
        /// </summary>
        /// <param name="sender">An object that contains the event sender.</param>
        /// <param name="e">A System.EventArgs object containing the event data.</param>
        protected void AssociatedAddress_GridViewSort(object sender, AccelaGridViewSortEventArgs e)
        {
            PaginationModel pageInfo = PaginationUtil.GetPageInfoByID(AssociatedAddressList.ClientID);
            pageInfo.SortExpression = e.GridViewSortExpression;
        }

        /// <summary>
        /// Address associated parcel list GridViewSort
        /// </summary>
        /// <param name="sender">An object that contains the event sender.</param>
        /// <param name="e">A System.EventArgs object containing the event data.</param>
        protected void AssociatedParcel_GridViewSort(object sender, AccelaGridViewSortEventArgs e)
        {
            PaginationModel pageInfo = PaginationUtil.GetPageInfoByID(AssociatedParcelList.ClientID);
            pageInfo.SortExpression = e.GridViewSortExpression;
        }

        /// <summary>
        /// Parcel associated owner list GridViewSort
        /// </summary>
        /// <param name="sender">An object that contains the event sender.</param>
        /// <param name="e">A System.EventArgs object containing the event data.</param>
        protected void AssociatedOwner_GridViewSort(object sender, AccelaGridViewSortEventArgs e)
        {
            PaginationModel pageInfo = PaginationUtil.GetPageInfoByID(AssociatedOwnerList.ClientID);
            pageInfo.SortExpression = e.GridViewSortExpression;
        }

        /// <summary>
        /// Owner associated APO list GridViewSort
        /// </summary>
        /// <param name="sender">An object that contains the event sender.</param>
        /// <param name="e">A System.EventArgs object containing the event data.</param>
        protected void AssociatedAPOList_GridViewSort(object sender, AccelaGridViewSortEventArgs e)
        {
            PaginationModel pageInfo = PaginationUtil.GetPageInfoByID(AssociatedAPOList.ClientID);
            pageInfo.SortExpression = e.GridViewSortExpression;
        }

        /// <summary>
        /// Update Permit Type List when Module Name is changed.
        /// </summary>
        /// <param name="sender">Module Dropdown List Control</param>
        /// <param name="e">Event Argument</param>
        protected void ModuleDropdown_IndexChanged(object sender, EventArgs e)
        {
            InitPermitType();
            Page.FocusElement(ddlModule.ClientID);
        }

        /// <summary>
        /// Address List row command event method.
        /// </summary>
        /// <param name="sender">An sender object that contains the event sender.</param>
        /// <param name="arg">A System.CommonEventArgs object containing the event data.</param>
        protected void AddressList_AddressSelected(object sender, CommonEventArgs arg)
        {
            if (arg == null || !(arg.ArgObject is DataRow))
            {
                return;
            }

            //1. Get the Address Associated Parcel and Owner data.
            DataRow addressRow = arg.ArgObject as DataRow;

            RefAddressPK = InitRefAddressPKModel(addressRow);

            divAssociatedAddress.Visible = false;
            divAssociatedAPOList.Visible = false;
            divAssociatedParcel.Visible = true;
            AssociatedParcelList.RefParcelDataSource = null;
            AssociatedParcelList.RetieveLinkLabelKey = "aca_apo_label_parcellist_retrieveassociatedowner";
            AssociatedParcelList.ExportFileName = "AssociatdParcelList";

            string fullAddress = addressRow["FullAddress"].ToString();

            DataTable parcelTable = GetAssociatedParcelDataTable(0, null);
            LoadAssociatedParcelData(parcelTable, fullAddress);
            UpdatePanelForParcel.Update();

            //Load the parcel associated owner section if only one parcel record display in address associated parcel list.
            if (parcelTable.Rows.Count == 1 && !AppSession.IsAdmin && StandardChoiceUtil.IsDisplayOwnerSection())
            {
                divAssociatedOwner.Visible = true;
                AssociatedOwnerList.OwnerDataSource = null;
                AssociatedOwnerList.ExportFileName = "AssociatedOwnerList";
                ParcelPK = InitRefParcelPKModel(parcelTable.Rows[0]);

                //Set the IE back flag for parcel associated owner data.
                AssociatedOwnerList.IsRedirectFromSearchPage = true;

                LoadAssociatedOwnerData(0, null);
            }
            else
            {
                divAssociatedOwner.Visible = false;
            }

            UpdatePanelForOwner.Update();
        }

        /// <summary>
        /// Parcel List row command event method.
        /// </summary>
        /// <param name="sender">An sender object that contains the event sender.</param>
        /// <param name="arg">A System.CommonEventArgs object containing the event data.</param>
        protected void ParcelList_ParcelSelected(object sender, CommonEventArgs arg)
        {
            if (arg == null || !(arg.ArgObject is DataRow))
            {
                return;
            }

            //1. Get the address associated Parcel and Owner data.
            DataRow parcelRow = arg.ArgObject as DataRow;
            ParcelPK = InitRefParcelPKModel(parcelRow);

            SearchType searchType = (SearchType)int.Parse(ddlSearchType.SelectedValue.Replace("-", string.Empty));

            if (searchType == SearchType.Parcel)
            {
                //Initial the associated section visible for parcel associated data.
                divAssociatedParcel.Visible = false;
                divAssociatedAddress.Visible = true;
                AssociatedAddressList.RefAddressDataSource = null;
                AssociatedAddressList.ExportFileName = "AssociatedAddressList";

                LoadAssociatedAddressData(0, null);
                UpdatePanelForAddress.Update();
            }

            //If standard choice display owner section is "N" and is in daily side, set owner section and list hidden.
            if (!AppSession.IsAdmin && StandardChoiceUtil.IsDisplayOwnerSection())
            {
                divAssociatedOwner.Visible = true;
                AssociatedOwnerList.OwnerDataSource = null;
                AssociatedOwnerList.ExportFileName = "AssociatedOwnerList";

                //Set the IE back flag for parcel associated owner data.
                AssociatedOwnerList.IsRedirectFromSearchPage = true;

                LoadAssociatedOwnerData(0, null);
            }
            else
            {
                divAssociatedOwner.Visible = false;
            }

            UpdatePanelForOwner.Update();
        }

        /// <summary>
        /// Owner List row command event method.
        /// </summary>
        /// <param name="sender">An sender object that contains the event sender.</param>
        /// <param name="arg">A System.CommonEventArgs object containing the event data.</param>
        protected void OwnerList_OwnerRetrieved(object sender, CommonEventArgs arg)
        {
            if (arg == null || !(arg.ArgObject is DataRow))
            {
                return;
            }

            //1. Get the address associated Parcel and Owner data.
            DataRow ownerRow = arg.ArgObject as DataRow;
            RefOwnerPK = APOUtil.GetOwnerModel(ownerRow);

            divAssociatedAddress.Visible = false;
            divAssociatedParcel.Visible = false;
            divAssociatedOwner.Visible = false;
            divAssociatedAPOList.Visible = true;
            AssociatedAPOList.RefAPODataSource = null;
            AssociatedAPOList.ExportFileName = "AssociatedParcelAndAddressList";

            LoadAssociatedAPOData(0, null);
            UpdateParcelForAPOList.Update();
        }

        #endregion Public/Protected Methods

        #region Private Methods

        /// <summary>
        /// create blank structure for cap list
        /// </summary>
        /// <returns>blank table for cap list</returns>
        private static DataTable CreateTable4SimpleCapModel()
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
            dt.Columns.Add(new DataColumn("AgencyCode", typeof(string)));
            dt.Columns.Add(new DataColumn("RelatedRecords", typeof(int)));
            dt.Columns.Add(new DataColumn("ModuleName", typeof(string)));
            return dt;
        }

        /// <summary>
        /// Initialize StatusControl
        /// </summary>
        private void InitStatusControl()
        {
            string selectedValue = ddlSearchType.SelectedValue;

            if (selectedValue.IndexOf("||", StringComparison.InvariantCulture) != -1)
            {
                selectedValue = selectedValue.Substring(0, selectedValue.IndexOf("||", StringComparison.InvariantCulture)); // remove bizDomainValue
            }

            int index = int.Parse(selectedValue.Replace("-", string.Empty));

            switch (index)
            {
                case 0:
                    addressLookupForm.SetAddessStatus();
                    break;

                case 1:
                    parcelLookupForm.SetParcelStauts();
                    break;

                case 2:
                    ownerLookupForm.SetOwnerStatus();
                    break;
            }
        }

        /// <summary>
        /// Set current City and State
        /// </summary>
        private void SetCurrentCityAndState()
        {
            addressLookupForm.SetCityAndState();
            ownerLookupForm.SetCityAndState();
            ////for licensee search section.
            AutoFillCityAndStateUtil.SetCurrentCity(txtCity, ModuleName);
            AutoFillCityAndStateUtil.SetCurrentState(txtState, ModuleName);
        }

        /// <summary>
        /// Bind cap list data source
        /// </summary>
        /// <param name="capList">Cap model list</param>
        private void BindCapList(DataTable capList)
        {
            PackageAPObyPermit(capList);

            //sort by date
            dgvPermitList.GridViewSortExpression = "Date";
            dgvPermitList.GridViewSortDirection = "DESC";
            DataView dataView = new DataView(capList);
            dataView.Sort = "Date" + " DESC";
            capList = dataView.ToTable();

            PermitDataSource = capList;
            dgvPermitList.DataSource = PermitDataSource; // dataTable;
            dgvPermitList.PageIndex = 0;
            dgvPermitList.EmptyDataText = AppSession.IsAdmin ? string.Empty : GetTextByKey("per_permitList_messagel_noRecord");
            dgvPermitList.DataBind();
        }

        /// <summary>
        /// Initialize Page Fields
        /// </summary>
        private void InitPage()
        {
            ITimeZoneBll timeBll = (ITimeZoneBll)ObjectFactory.GetObject(typeof(ITimeZoneBll));
            DateTime endDate = timeBll.GetAgencyCurrentDate(ConfigManager.AgencyCode);
            DateTime startDate = CapUtil.GetCapDefaultFindDateRange(ConfigManager.AgencyCode, endDate);           

            txtAPO_Search_by_Permit_EndDate.Text2 = endDate;
            txtAPO_Search_by_Permit_StartDate.Text2 = startDate;
            lblSearchInstruction.Text = GetTextByKey("apo_lookup_instruction_searchbyaddress");

            ownerLookupForm.SetAPOOwnerCountry(RelevantOwnerPhoneIDs);
            addressLookupForm.ShowTemplateFields();
            parcelLookupForm.ShowTemplateFields();
            ownerLookupForm.ShowTemplateFields();
        }

        /// <summary>
        /// Fill dropdownlist of search condition items
        /// </summary>
        private void BindDropdownList()
        {
            // Fill Modules
            DropDownListBindUtil.BindModules(ddlModule, false);

            //Fill permit tab dropdown list
            InitPermitType();

            //Bind all dropdown list.
            BindAllDropdownList();
        }

        /// <summary>
        /// Bind the search list.
        /// </summary>
        private void BindSearchList()
        {
            IList<ItemValue> stdItems = DropDownListBindUtil.GetHardcodeItems4DDL(ddlSearchType, BizDomainConstant.STD_CAT_APO_LOOKUP_TYPE, ModuleName);

            if (stdItems != null && stdItems.Count > 0)
            {
                foreach (ItemValue item in stdItems)
                {
                    /*
                     * if standard choice display owner section is "N" and current item is search by owner.
                     * filter this item in search list.
                     */
                    if (!AppSession.IsAdmin
                        && !StandardChoiceUtil.IsDisplayOwnerSection()
                        && item.Key.Equals(XPolicyConstant.APO_LOOKUP_ITEM_DATA2_LOOK_UP_BY_OWNER, StringComparison.InvariantCultureIgnoreCase))
                    {
                        continue;
                    }

                    ddlSearchType.Items.Add(new ListItem(item.Key, item.Value.ToString()));
                }

                //if only 1 option, display value as lable. hide dropdown.
                if (!AppSession.IsAdmin && stdItems.Count == 1)
                {
                    ddlSearchType.Visible = false;
                }
            }
            else
            {
                btnNewSearch.Visible = false;
            }
        }

        /// <summary>
        /// Bind all dropdown list.
        /// </summary>
        private void BindAllDropdownList()
        {
            addressLookupForm.BindAllDropdownList();            

            DropDownListBindUtil.BindLicenseType(ddlLicenseType);
            DropDownListBindUtil.BindContactType4License(ddlContactType); 
        }

        /// <summary>
        /// compare start date with end date.
        /// if start date bigger than end date that is wrong
        /// </summary>
        /// <param name="beginDate">begin date.</param>
        /// <param name="endDate">end date string.</param>
        /// <returns>true if start date smaller than end date. </returns>
        private bool CheckDate(string beginDate, string endDate)
        {
            if (!string.IsNullOrEmpty(beginDate) && !string.IsNullOrEmpty(endDate))
            {
                DateTime dtBegin = I18nDateTimeUtil.ParseFromUI(beginDate);
                DateTime dtEnd = I18nDateTimeUtil.ParseFromUI(endDate).AddDays(1).AddSeconds(-1);

                if (dtBegin > dtEnd)
                {
                    string msg = GetTextByKey("per_permitList_msg_date_start_end");

                    MessageUtil.ShowMessageByControl(Page, MessageType.Error, msg);

                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Check whether has at least one search criteria has been entered when search by Permit.
        /// </summary>
        /// <returns>true: input at least one condition; false: doesn't input any condition</returns>
        private bool CheckInputConditionForPermit()
        {
            if (txtAPO_Search_by_Permit_StartDate.Visible && txtAPO_Search_by_Permit_EndDate.Visible)
            {
                return CheckDate(txtAPO_Search_by_Permit_StartDate.Text, txtAPO_Search_by_Permit_EndDate.Text);
            }

            return true;
        }

        /// <summary>
        /// Get permit data, search type by Permit.
        /// </summary>
        /// <param name="currentPageIndex">The current page index.</param>
        /// <param name="sortExpression">The sort expression.</param>
        /// <returns>SimpleCapModel4WS array</returns>
        private DataTable CreateCapDataSource(int currentPageIndex, string sortExpression)
        {
            // set the page info and query format
            PaginationModel pageInfo = PaginationUtil.GetPageInfoByID(dgvPermitList.ClientID);
            pageInfo.SortExpression = sortExpression;
            pageInfo.CurrentPageIndex = currentPageIndex;
            pageInfo.CustomPageSize = dgvPermitList.PageSize;
            QueryFormat queryFormat = PaginationUtil.GetQueryFormatModel(pageInfo);

            // set the search criteria from the general search model
            CapModel4WS capModel = GeneralSearchModel.CapModel;
            List<string> moduleNameList = GeneralSearchModel.ModuleNameList;
            string[] hiddenViewEltNames = GeneralSearchModel.HiddenViewEltNames;

            // search records
            SearchResultModel capResult = _capBll.GetCapList4ACA(capModel, queryFormat, AppSession.User.UserSeqNum, null, false, moduleNameList, pageInfo.IsSearchAllStartRow, hiddenViewEltNames);
            pageInfo.StartDBRow = capResult.startRow;
            pageInfo.IsSearchAllStartRow = capResult.searchAllStartRow;
            DataTable capList = PaginationUtil.MergeDataSource(PermitDataSource, ConvertSimpleCapModel2DataTable(capResult.resultList), pageInfo);

            return capList;
        }

        /// <summary>
        /// create data table by given cap list
        /// </summary>
        /// <param name="capList">Cap model list, its inner type must be SimpleCapModel.</param>
        /// <returns>data source for UI</returns>
        private DataTable ConvertSimpleCapModel2DataTable(object[] capList)
        {
            DataTable dt = CreateTable4SimpleCapModel();

            if (capList == null)
            {
                return dt;
            }

            int index = 0;

            foreach (object objCap in capList)
            {
                SimpleCapModel cap = objCap as SimpleCapModel;

                if (cap == null)
                {
                    continue;
                }

                string permitNumber = cap.altID != null ? cap.altID : string.Empty;
                string permitType = CAPHelper.GetAliasOrCapTypeLabel(cap);
                string status = cap.capStatus != null ? cap.capStatus : string.Empty;
                string capStatus = cap.capClass != null ? cap.capClass : string.Empty;

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
                dr["CapClass"] = capStatus;
                dr["capID1"] = cap.capID.ID1;
                dr["capID2"] = cap.capID.ID2;
                dr["capID3"] = cap.capID.ID3;
                dr["ProjectName"] = cap.specialText;

                if (cap.capID != null && !string.IsNullOrEmpty(cap.capID.serviceProviderCode))
                {
                    dr["AgencyCode"] = cap.capID.serviceProviderCode;
                }
                else
                {
                    dr["AgencyCode"] = ACAConstant.AgencyCode;
                }

                dr["Date"] = cap.fileDate == null ? DBNull.Value : (object)cap.fileDate;
                dr["RelatedRecords"] = cap.relatedRecordsCount;
                dr["ModuleName"] = cap.moduleName;
                dt.Rows.Add(dr);
                index++;
            }

            return dt;
        }

        /// <summary>
        /// Initialize all visual control not include Search criteria.
        /// </summary>
        private void InitDisplay()
        {
            divPageResult.Visible = false;
        }

        /// <summary>
        /// Clear the data source for each control after changing search criteria.
        /// </summary>
        private void InitDataSource()
        {
            RefAddressLookUpList.RefAddressDataSource = null;
            RefParcelLookUpList.RefParcelDataSource = null;
            RefOwnerLookUpList.OwnerDataSource = null;
            RefAPOAddressList1.RefAPODataSource = null;
            AssociatedAddressList.RefAddressDataSource = null;
            AssociatedParcelList.RefParcelDataSource = null;
            AssociatedOwnerList.OwnerDataSource = null;
            AssociatedAPOList.RefAPODataSource = null;
        }

        /// <summary>
        /// Initialize Permit Type based on selected module
        /// </summary>
        private void InitPermitType()
        {
            ddlAPO_Search_by_Permit_Permit.Items.Clear();

            DropDownListBindUtil.BindDDL(CapUtil.GetPermitTypesByModuleName(SelectedModule), ddlAPO_Search_by_Permit_Permit);
        }
         
        /// <summary>
        /// Initialize the GridView's export link visible. 
        /// </summary>
        private void InitalExport()
        {
            if (!AppSession.IsAdmin && StandardChoiceUtil.IsEnableExport2CSV())
            {
                dgvPermitList.ShowExportLink = true;
                dgvPermitList.ExportFileName = "APORecordList";
                gdvLicenseeList.ShowExportLink = true;
                gdvLicenseeList.ExportFileName = "APOLicenseeList";
                RefAPOAddressList1.InitialExport(true);
            }
            else
            {
                dgvPermitList.ShowExportLink = false;
                gdvLicenseeList.ShowExportLink = false;
                RefAPOAddressList1.InitialExport(false);
            }
        }

        /// <summary>
        /// Package Session APO list by address
        /// </summary>
        /// <param name="dt">data table</param>
        private void PackageAPObyAddress(DataTable dt)
        {
            Hashtable htAddress = addressLookupForm.GetAddress();
            ApoQueryInfo.APOAddressFilter = htAddress;
            ApoQueryInfo.APOAddressSource = dt;
            ApoQueryInfo.APOSearchTypeIndex = (int)SearchType.Address;
            Session[SessionConstant.SESSION_APO_QUERY] = ApoQueryInfo;
        }

        /// <summary>
        /// Package Session APO list by Owner
        /// </summary>
        /// <param name="dt">data table</param>
        private void PackageAPObyOwner(DataTable dt)
        {
            Hashtable htOwner = ownerLookupForm.GetOwnerHashTable();
            ApoQueryInfo.APOOwnerFilter = htOwner;
            ApoQueryInfo.APOOwnerSource = dt;
            ApoQueryInfo.APOSearchTypeIndex = (int)SearchType.Owner;
            Session[SessionConstant.SESSION_APO_QUERY] = ApoQueryInfo;
        }

        /// <summary>
        /// Package Session APO list by Parcel
        /// </summary>
        /// <param name="dt">data table</param>
        private void PackageAPObyParcel(DataTable dt)
        {
            Hashtable htParcel = parcelLookupForm.GetParcelHashTable();
 
            ApoQueryInfo.APOParcelFilter = htParcel;
            ApoQueryInfo.APOParcelSource = dt;
            ApoQueryInfo.APOSearchTypeIndex = (int)SearchType.Parcel;
            Session[SessionConstant.SESSION_APO_QUERY] = ApoQueryInfo;
        }

        /// <summary>
        /// Package Session APO list by Permit
        /// </summary>
        /// <param name="dt">data table</param>
        private void PackageAPObyPermit(DataTable dt)
        {
            Hashtable htPermit = new Hashtable(5);

            htPermit.Add("txtAPO_Search_by_Permit_PermitNumber", txtAPO_Search_by_Permit_PermitNumber.Text.Trim());
            htPermit.Add("txtAPO_Search_by_Permit_ProjectName", txtAPO_Search_by_Permit_ProjectName.Text.Trim());
            htPermit.Add("txtAPO_Search_by_Permit_StartDate", txtAPO_Search_by_Permit_StartDate.Text.Trim());
            htPermit.Add("txtAPO_Search_by_Permit_EndDate", txtAPO_Search_by_Permit_EndDate.Text.Trim());
            htPermit.Add("ddlModule", ddlModule.SelectedValue.Trim());
            htPermit.Add("ddlAPO_Search_by_Permit_Permit", ddlAPO_Search_by_Permit_Permit.SelectedValue.Trim());

            ApoQueryInfo.APOPermitFilter = htPermit;
            ApoQueryInfo.APOPermitSource = dt;
            ApoQueryInfo.APOSearchTypeIndex = (int)SearchType.Permit;
        }

        /// <summary>
        /// Reload APO list by address
        /// </summary>
        private void ReloadAPObyAddress()
        {
            ddlSearchType.SelectedValue = ApoQueryInfo.APOSearchTypeIndex.ToString();

            SetDivIsVisible();

            Hashtable htAddress = ApoQueryInfo.APOAddressFilter;

            if (htAddress != null)
            {
                addressLookupForm.SetAddressValue(htAddress);                
            }

            DataTable dt = ApoQueryInfo.APOAddressSource;

            if (dt != null && dt.Rows.Count > 0)
            {
                RefAPOAddressList1.Visible = false;
                RefAddressLookUpList.Visible = true;
                RefParcelLookUpList.Visible = false;
                RefOwnerLookUpList.Visible = false;
                divAssociatedAddress.Visible = false;
                divAssociatedParcel.Visible = false;
                divAssociatedOwner.Visible = false;
                divAssociatedAPOList.Visible = false;
                RefAddressLookUpList.BindDataSource(dt, false, ApoQueryInfo.APOAddressPageIndex, ApoQueryInfo.APOAddressPageSort);
                RefAddressLookUpList.ExportFileName = "APOAddress";
                ShowResultInfo(dt.Rows.Count, RefAddressLookUpList.CountSummary, false);

                //Need auto display the addresss associated data if only one address record found.
                if (dt.Rows.Count == 1)
                {
                    RefAddressLookUpList.RetrieveAssociatedData(dt.Rows[0]);
                }
            }
        }

        /// <summary>
        /// Reload APO list by Owner
        /// </summary>
        private void ReloadAPObyOwner()
        {
            ddlSearchType.SelectedValue = ApoQueryInfo.APOSearchTypeIndex.ToString();
            SetDivIsVisible();
            Hashtable htOwner = ApoQueryInfo.APOOwnerFilter;

            if (htOwner != null)
            {
                ownerLookupForm.SetOwnerValue(htOwner);
            }

            DataTable dt = ApoQueryInfo.APOOwnerSource;

            if (dt != null && dt.Rows.Count > 0)
            {
                RefAPOAddressList1.Visible = false;
                RefAddressLookUpList.Visible = false;
                RefParcelLookUpList.Visible = false;
                RefOwnerLookUpList.Visible = true;
                divAssociatedAddress.Visible = false;
                divAssociatedParcel.Visible = false;
                divAssociatedOwner.Visible = false;
                divAssociatedAPOList.Visible = false;
                RefOwnerLookUpList.BindDataSource(dt, false, ApoQueryInfo.APOOwnerPageIndex, ApoQueryInfo.APOOwnerPageSort);
                RefOwnerLookUpList.ExportFileName = "APOOwner";

                ShowResultInfo(dt.Rows.Count, RefOwnerLookUpList.CountSummary, false);

                //Need auto display the owner associated data if only one owner record found.
                if (dt.Rows.Count == 1)
                {
                    RefOwnerLookUpList.RetrieveAssociatedData(dt.Rows[0]);
                }
            }
        }

        /// <summary>
        /// Reload APO list by Parcel
        /// </summary>
        private void ReloadAPObyParcel()
        {
            ddlSearchType.SelectedValue = ApoQueryInfo.APOSearchTypeIndex.ToString();
            SetDivIsVisible();
            Hashtable htParcel = ApoQueryInfo.APOParcelFilter;

            if (htParcel != null)
            {
                parcelLookupForm.SetPacelValue(htParcel);                
            }

            DataTable dt = ApoQueryInfo.APOParcelSource;

            if (dt != null && dt.Rows.Count > 0)
            {
                RefAPOAddressList1.Visible = false;
                RefAddressLookUpList.Visible = false;
                RefParcelLookUpList.Visible = true;
                RefOwnerLookUpList.Visible = false;
                divAssociatedAddress.Visible = false;
                divAssociatedParcel.Visible = false;
                divAssociatedOwner.Visible = false;
                divAssociatedAPOList.Visible = false;
                RefParcelLookUpList.RetieveLinkLabelKey = "aca_apo_label_parcellist_retrieveassociateddata";
                RefParcelLookUpList.BindDataSource(dt, false, ApoQueryInfo.APOParcelPageIndex, ApoQueryInfo.APOParcelPageSort);
                RefParcelLookUpList.ExportFileName = "APOParcel";
                ShowResultInfo(dt.Rows.Count, RefParcelLookUpList.CountSummary, false);

                //Need auto display the address associated data if only one parcel record found.
                if (dt.Rows.Count == 1)
                {
                    RefParcelLookUpList.RetrieveAssociatedData(dt.Rows[0]);
                }
            }
        }

        /// <summary>
        /// Reload APO list by Permit
        /// </summary>
        private void ReloadAPObyPermit()
        {
            ddlSearchType.SelectedValue = ApoQueryInfo.APOSearchTypeIndex.ToString();
            SetDivIsVisible();
            Hashtable htPermit = ApoQueryInfo.APOPermitFilter;

            if (htPermit != null)
            {
                txtAPO_Search_by_Permit_PermitNumber.Text = htPermit["txtAPO_Search_by_Permit_PermitNumber"].ToString();
                txtAPO_Search_by_Permit_ProjectName.Text = htPermit["txtAPO_Search_by_Permit_ProjectName"].ToString();
                txtAPO_Search_by_Permit_StartDate.Text = htPermit["txtAPO_Search_by_Permit_StartDate"].ToString();
                txtAPO_Search_by_Permit_EndDate.Text = htPermit["txtAPO_Search_by_Permit_EndDate"].ToString();
                ddlModule.SelectedValue = htPermit["ddlModule"].ToString();

                InitPermitType();

                ddlAPO_Search_by_Permit_Permit.SelectedValue = htPermit["ddlAPO_Search_by_Permit_Permit"].ToString();
            }

            ReloadPermit(ApoQueryInfo);
        }

        /// <summary>
        /// Bind cap list data source
        /// </summary>
        /// <param name="apoQueryInfo">APO query info</param>
        private void ReloadCapList(ApoQueryInfo apoQueryInfo)
        {
            DataTable dataTable = apoQueryInfo.APOPermitSource;

            dgvPermitList.PageIndex = apoQueryInfo.APOPermitPageIndex;

            DataView dataView = new DataView(dataTable);

            if (!string.IsNullOrEmpty(apoQueryInfo.APOPermitPageSort))
            {
                dataView.Sort = apoQueryInfo.APOPermitPageSort;
            }

            dataTable = dataView.ToTable();

            dgvPermitList.EmptyDataText = GetTextByKey("per_permitList_messagel_noRecord");

            PermitDataSource = dataTable;
            dgvPermitList.DataSource = PermitDataSource; // dataTable;
            dgvPermitList.DataBind();

            DataTable dt = apoQueryInfo.APOAddressSource;

            if (dt != null && dt.Rows.Count == 1)
            {
                RefAPOAddressList1.Visible = false;
            }
            else
            {
                RefAPOAddressList1.Visible = true;
                RefAPOAddressList1.ComponentType = (int)SearchType.Address;
                RefAPOAddressList1.BindDataSource(dt, apoQueryInfo.APOAddressPageIndex, apoQueryInfo.APOAddressPageSort);
                RefAPOAddressList1.ExportFileName = "APORecordList";
            
                divAPOResult.Visible = true;
                RefAddressLookUpList.Visible = false;
                RefParcelLookUpList.Visible = false;
                RefOwnerLookUpList.Visible = false;
            }
        }

        /// <summary>
        /// IE Back load data
        /// </summary>
        private void ReloadData()
        {
            SetDivIsVisible();

            //ApoQueryInfo ApoQueryInfo = (ApoQueryInfo)Session[SessionConstant.SESSION_APO_QUERY];
            SearchType searchType = (SearchType)ApoQueryInfo.APOSearchTypeIndex;

            ShowDisplay(searchType);

            switch (searchType)
            {
                case SearchType.Address:
                    ReloadAPObyAddress();
                    break;

                case SearchType.Parcel:
                    ReloadAPObyParcel();
                    break;

                case SearchType.Owner:
                    ReloadAPObyOwner();
                    break;
                case SearchType.Permit:
                    ////Check input condition is validate.
                    if (!CheckInputConditionForPermit())
                    {
                        RecordSearchResultInfo.Hide();
                        divResultPermitList.Visible = false;

                        return;
                    }
                    else
                    {
                        dgvPermitList.Visible = true;
                    }

                    ReloadAPObyPermit();
                    break;
                case SearchType.License:
                    ReloadAPObyLicensee();
                    break;
            }
        }

        /// <summary>
        /// Reload Permit list on the page, search type by Permit.
        /// </summary>
        /// <param name="apoQueryInfo">APO query info</param>
        private void ReloadPermit(ApoQueryInfo apoQueryInfo)
        {
            if (apoQueryInfo != null
                && apoQueryInfo.APOPermitSource != null
                && apoQueryInfo.APOPermitSource.Rows.Count > 0)
            {
                ReloadCapList(apoQueryInfo);
                divResultPermitList.Visible = true;
                dgvPermitList.Visible = true;
                RecordSearchResultInfo.Display(dgvPermitList.CountSummary);
            }
            else
            {
                divResultPermitList.Visible = false;
                RecordSearchResultInfo.Display(0);
            }
        }

        /// <summary>
        /// Search Reference Address
        /// </summary>
        /// <param name="currentPageIndex">The current page index.</param>
        /// <param name="sortExpression">The sort expression.</param>
        /// <param name="isClickSearchButton">if set to "true" [is click search button].</param>
        private void SearchRefAddress(int currentPageIndex, string sortExpression, bool isClickSearchButton)
        {
            RefAddressModel addressModel = addressLookupForm.GetRefAddress();
            PaginationModel pageInfo = PaginationUtil.GetPageInfoByID(RefAddressLookUpList.ClientID + SearchType.Address);
            pageInfo.SortExpression = sortExpression;
            pageInfo.CurrentPageIndex = currentPageIndex;
            pageInfo.CustomPageSize = RefAddressLookUpList.PageSize;
            QueryFormat queryFormat = PaginationUtil.GetQueryFormatModel(pageInfo);
            SearchResultModel searchResult = _apoBll.GetRefAddressList(ConfigManager.AgencyCode, addressModel, queryFormat);

            if (searchResult != null)
            {
                pageInfo.StartDBRow = searchResult.startRow;
                DataTable dt = APOUtil.BuildAddressDataTable(searchResult.resultList);

                dt = PaginationUtil.MergeDataSource<DataTable>(RefAddressLookUpList.RefAddressDataSource, dt, pageInfo);
                PackageAPObyAddress(dt);
                RefAddressLookUpList.ExportFileName = "APOAddress";
                RefAddressLookUpList.ClearSelectedItems();
                RefAddressLookUpList.BindDataSource(dt, true);
                ShowResultInfo(dt.Rows.Count, RefAddressLookUpList.CountSummary, isClickSearchButton);

                //Need auto display the address associated data if only one parcel record found.
                if (dt.Rows.Count == 1)
                {
                    RefAddressLookUpList.RetrieveAssociatedData(dt.Rows[0]);
                }
            }
        }

        /// <summary>
        /// Search Reference Parcel
        /// </summary>
        /// <param name="currentPageIndex">Current PageIndex</param>
        /// <param name="sortExpression">Sort Expression</param>
        /// <param name="isClickSearchButton">isClickSearchButton flag</param>
        private void SearchRefParcel(int currentPageIndex, string sortExpression, bool isClickSearchButton)
        {
            ParcelModel parcelModel = parcelLookupForm.GetParcel();
            PaginationModel pageInfor = PaginationUtil.GetPageInfoByID(RefParcelLookUpList.ClientID + SearchType.Parcel);
            pageInfor.SortExpression = sortExpression;
            pageInfor.CurrentPageIndex = currentPageIndex;
            pageInfor.CustomPageSize = RefParcelLookUpList.PageSize;
            QueryFormat queryFormat = PaginationUtil.GetQueryFormatModel(pageInfor);
            SearchResultModel searchResult = _apoBll.GetRefParcelList(ConfigManager.AgencyCode, parcelModel, queryFormat);

            if (searchResult != null)
            {
                pageInfor.StartDBRow = searchResult.startRow;
                DataTable dt = APOUtil.BuildParcelDataTable(searchResult.resultList);
                dt = PaginationUtil.MergeDataSource<DataTable>(RefParcelLookUpList.RefParcelDataSource, dt, pageInfor);
                PackageAPObyParcel(dt);

                if (!AppSession.IsAdmin && StandardChoiceUtil.IsDisplayOwnerSection())
                {
                    RefParcelLookUpList.RetieveLinkLabelKey = "aca_apo_label_parcellist_retrieveassociateddata";
                }
                else
                {
                    RefParcelLookUpList.RetieveLinkLabelKey = "aca_apo_label_parcellist_retrieveassociatedaddress";
                }

                RefParcelLookUpList.ExportFileName = "APOParcel";
                RefParcelLookUpList.ClearSelectedItems();
                RefParcelLookUpList.BindDataSource(dt, true);
                ShowResultInfo(dt.Rows.Count, RefParcelLookUpList.CountSummary, isClickSearchButton);

                //Need auto display the parcel associated data if only one parcel record found.
                if (dt.Rows.Count == 1)
                {
                    RefParcelLookUpList.RetrieveAssociatedData(dt.Rows[0]);
                }
            }
        }

        /// <summary>
        /// Search Reference Owner.
        /// </summary>
        /// <param name="currentPageIndex">The current page index.</param>
        /// <param name="sortExpression">The sort expression.</param>
        /// <param name="isClickSearchButton">if set to <c>true</c> [is click search button].</param>
        private void SearchRefOwner(int currentPageIndex, string sortExpression, bool isClickSearchButton)
        {
            OwnerCompModel ownerModel = ownerLookupForm.GetOwner();
            PaginationModel pageInfor = PaginationUtil.GetPageInfoByID(RefOwnerLookUpList.ClientID + SearchType.Owner);
            pageInfor.SortExpression = sortExpression;
            pageInfor.CurrentPageIndex = currentPageIndex;
            pageInfor.CustomPageSize = RefOwnerLookUpList.PageSize;
            QueryFormat queryFormat = PaginationUtil.GetQueryFormatModel(pageInfor);
            SearchResultModel result = _apoBll.GetRefOwnerList(ConfigManager.AgencyCode, ownerModel, queryFormat);
            pageInfor.StartDBRow = result.startRow;

            DataTable dt = APOUtil.BuildOwnerDataTable(result.resultList);
            dt = PaginationUtil.MergeDataSource<DataTable>(RefOwnerLookUpList.OwnerDataSource, dt, pageInfor);
            PackageAPObyOwner(dt);

            RefOwnerLookUpList.ExportFileName = "APOOwner";
            RefOwnerLookUpList.ClearSelectedItems();
            RefOwnerLookUpList.BindDataSource(dt, true);
            ShowResultInfo(dt.Rows.Count, RefOwnerLookUpList.CountSummary, isClickSearchButton);

            //Need auto display the owner associated data if only one owner record found.
            if (dt.Rows.Count == 1)
            {
                RefOwnerLookUpList.RetrieveAssociatedData(dt.Rows[0]);
            }
        }

        /// <summary>
        /// Display Permit list on the page, search type by Permit.
        /// </summary>
        /// <param name="currentPageIndex">The current page index.</param>
        /// <param name="sortExpression">The sort expression.</param>
        private void SearchPermit(int currentPageIndex, string sortExpression)
        {
            DataTable capList = CreateCapDataSource(currentPageIndex, sortExpression);

            if (capList != null && capList.Rows.Count > 0)
            {
                BindCapList(capList);
                divResultPermitList.Visible = true;
                dgvPermitList.Visible = true;
                RecordSearchResultInfo.Display(dgvPermitList.CountSummary);
                RefAPOAddressList1.ExportFileName = "APORecordList";
            }
            else
            {
                RecordSearchResultInfo.Display(0);
                divResultPermitList.Visible = false;
            }
        }

        /// <summary>
        /// Display page element by different search type
        /// </summary>
        private void SetDivIsVisible()
        {
            List<HtmlGenericControl> liDiv = new List<HtmlGenericControl>();
            liDiv.Add(dvSearchByAddress);
            liDiv.Add(dvSearchByParcel);
            liDiv.Add(dvSearchByOwner);
            liDiv.Add(dvSearchByPermit);
            liDiv.Add(dvSearchByLicensee);

            string selectedValue = ddlSearchType.SelectedValue;

            if (selectedValue.IndexOf("||", StringComparison.InvariantCulture) != -1)
            {
                selectedValue = selectedValue.Substring(0, selectedValue.IndexOf("||", StringComparison.InvariantCulture)); // remove bizDomainValue
            }

            int index = int.Parse(selectedValue.Replace("-", string.Empty));

            for (int i = 0; i < liDiv.Count; i++)
            {
                if (i == index)
                {
                    ((HtmlGenericControl)liDiv[i]).Visible = true;
                }
                else
                {
                    ((HtmlGenericControl)liDiv[i]).Visible = false;
                }
            }

            string prefix = null;
            string instructionKey = string.Empty;
            GFilterScreenPermissionModel4WS permission = new GFilterScreenPermissionModel4WS();
            string viewId = string.Empty;
            ControlCollection hideContainer = Controls;

            switch (index)
            {
                case 0:
                    prefix = addressLookupForm.UserControlClientID;
                    instructionKey = "apo_lookup_instruction_searchbyaddress";
                    permission.permissionLevel = GViewConstant.PERMISSION_APO;
                    permission.permissionValue = GViewConstant.SECTION_ADDRESS;
                    viewId = GviewID.LookUpByAddress;
                    hideContainer = dvSearchByAddress.Controls;
                    break;
                case 1:
                    prefix = parcelLookupForm.UserControlClientID;
                    instructionKey = "apo_lookup_instruction_searchbyparcel";
                    permission.permissionLevel = GViewConstant.PERMISSION_APO;
                    permission.permissionValue = GViewConstant.SECTION_PARCEL;
                    viewId = GviewID.LookUpByParcel;
                    hideContainer = dvSearchByParcel.Controls;
                    break;
                case 2:
                    prefix = ownerLookupForm.UserControlClientID;
                    instructionKey = "apo_lookup_instruction_searchbyowner";
                    permission.permissionLevel = GViewConstant.PERMISSION_APO;
                    permission.permissionValue = GViewConstant.SECTION_OWNER;
                    viewId = GviewID.LookUpByOwner;
                    hideContainer = dvSearchByOwner.Controls;
                    break;
                case 3:
                    prefix = txtAPO_Search_by_Permit_PermitNumber.ClientID.Replace("txtAPO_Search_by_Permit_PermitNumber", string.Empty);
                    instructionKey = "apo_lookup_instruction_searchbypermit";
                    viewId = GviewID.LookUpByPermit;
                    hideContainer = dvSearchByPermit.Controls;
                    break;
                case 4:
                    prefix = txtLicenseNumber.ClientID.Replace("txtLicenseNumber", string.Empty);
                    instructionKey = "apo_lookup_instruction_searchbylicensee";
                    viewId = GviewID.LookUpByLicensee;
                    hideContainer = dvSearchByLicensee.Controls;
                    break;
            }

            lblSelectedSearchType.InnerText = ddlSearchType.SelectedItem.Text;
            lblSearchInstruction.Text = GetTextByKey(instructionKey);

            ddlSearchType.SectionID = string.Format("{0}" + ACAConstant.SPLIT_CHAR + "{1}" + ACAConstant.SPLIT_CHAR + "{2}", ConfigManager.AgencyCode, viewId, prefix);
            if (permission != null && permission.permissionLevel != null)
            {
                ControlBuildHelper.HideStandardFields(viewId, string.Empty, hideContainer, AppSession.IsAdmin, permission);
            }
            else
            {
                ControlBuildHelper.HideStandardFields(viewId, string.Empty, hideContainer, AppSession.IsAdmin);
            }

            if (index == 4)
            {
                ShowLicenseeDivSections();
            }

            InitPermitType();
        }

        /// <summary>
        /// According to search type, display or hide search result.
        /// </summary>
        /// <param name="searchType">search type</param>
        private void ShowDisplay(SearchType searchType)
        {
            if (searchType == SearchType.Permit)
            {
                divPagePermitListResult.Visible = true;
                divLicenseeResult.Visible = false;
                divAPOResult.Visible = false;
            }
            else if (searchType == SearchType.License)
            {
                divLicenseeResult.Visible = true;
                divPagePermitListResult.Visible = false;
                divAPOResult.Visible = false;
            }
            else
            {
                divPagePermitListResult.Visible = false;
                divLicenseeResult.Visible = false;
                divAPOResult.Visible = true;
            }

            divPageResult.Visible = true;
        }

        /// <summary>
        /// Reload APO list by licensee
        /// </summary>
        private void ReloadAPObyLicensee()
        {
            //ApoQueryInfo ApoQueryInfo = (ApoQueryInfo)Session[SessionConstant.SESSION_APO_QUERY];
            ddlSearchType.SelectedValue = ApoQueryInfo.APOSearchTypeIndex.ToString();
            SetDivIsVisible();
            LicenseProfessionalModel license = ApoQueryInfo.APOLicenseeFilter;
            ShowLicenseInfo(license);
            ReloadLicenseeList(ApoQueryInfo);
        }

        /// <summary>
        /// Validate div row section.
        /// </summary>
        private void ShowLicenseeDivSections()
        {
            divLicenseType.Visible = ddlLicenseType.Visible || txtLicenseNumber.Visible;
            divBusinessName1.Visible = txtBusiName.Visible;
            divBusinessName2.Visible = txtBusiName2.Visible;
            divBusinessLicense.Visible = txtBusiLicense.Visible;
            divLicenseName.Visible = txtFirstName.Visible || txtMiddleInitial.Visible || txtLastName.Visible;
            divCountry.Visible = ddlCountry.Visible;
            divAddress1.Visible = txtAddress1.Visible;
            divAddress2.Visible = txtAddress2.Visible;
            divAddress3.Visible = txtAddress3.Visible;
            divState.Visible = txtState.Visible || txtCity.Visible || txtZipCode.Visible;
            divPhone.Visible = txtPhone1.Visible || txtPhone2.Visible || txtFax.Visible;
            divContactType.Visible = ddlContactType.Visible;
            divSSN.Visible = txtSSN.Visible || txtFEIN.Visible;
            divContractorLicNO.Visible = txtContractorLicNO.Visible || txtContractorBusiName.Visible;
        }

        /// <summary>
        /// Display Permit list on the page, search type by Permit.
        /// </summary>
        /// <param name="currentPageIndex">The current page index.</param>
        /// <param name="sortExpression">The sort expression.</param>
        private void SearchLicensees(int currentPageIndex, string sortExpression)
        {
            IList<LicenseProfessionalModel> licenseModels = SearchLPModel(currentPageIndex, sortExpression);
            bool hasLicenseModel = licenseModels != null && licenseModels.Count > 0;

            if (hasLicenseModel)
            {
                BindLicenseeList(licenseModels);
                RefAPOAddressList1.ExportFileName = "APOLicenseeList";
            }

            SetLicenseUI(hasLicenseModel);
        }

        /// <summary>
        /// Bind licensee list data source
        /// </summary>
        /// <param name="apoQueryInfo">APO query info</param>
        private void ReloadLicenseeList(ApoQueryInfo apoQueryInfo)
        {
            IList<LicenseProfessionalModel> licenseModels = apoQueryInfo.APOLicenseeSource;

            if (licenseModels != null)
            {
                LPDataSource = licenseModels;
                BindSessionLicenseeList(apoQueryInfo);

                DataTable dt = apoQueryInfo.APOAddressSource;

                APOResultModel apoResult = RefAPOAddressList1.ValidateSingleAPOData(dt.Rows[0]);
                RefAPOAddressList1.IsSingleAPOData = apoResult.IsSingleAPOData;

                if (dt != null && dt.Rows.Count == 1 && RefAPOAddressList1.IsSingleAPOData)
                {
                    LPSearchResultInfo.Display(gdvLicenseeList.CountSummary);
                    RefAPOAddressList1.Visible = false;
                }
                else
                {
                    RefAPOAddressList1.Visible = true;
                    RefAPOAddressList1.ComponentType = (int)SearchType.License;
                    RefAPOAddressList1.BindDataSource(dt, apoQueryInfo.APOAddressPageIndex, apoQueryInfo.APOAddressPageSort);
                    RefAPOAddressList1.ExportFileName = "APOLicenseeList";
                    divAPOResult.Visible = true;
                    RefAddressLookUpList.Visible = false;
                    RefParcelLookUpList.Visible = false;
                    RefOwnerLookUpList.Visible = false;
                    SetLicenseUI(true);
                }
            }
        }

        /// <summary>
        /// Bind license professional list data source
        /// </summary>
        /// <param name="licenseModels">license professional model list</param>
        private void BindLicenseeList(IList<LicenseProfessionalModel> licenseModels)
        {
            PackageAPObyLicensee(licenseModels);
            LPDataSource = licenseModels;

            DataTable dt = GridViewBuildHelper.MergeTemplateAttributes2DataTable(gdvLicenseeList, licenseModels);
            gdvLicenseeList.DataSource = dt;
            gdvLicenseeList.EmptyDataText = GetTextByKey("per_permitList_messagel_noRecord");
            gdvLicenseeList.DataBind();
        }

        /// <summary>
        /// Package Session APO list by licensee
        /// </summary>
        /// <param name="licenseModels">licensed professional models</param>
        private void PackageAPObyLicensee(IList<LicenseProfessionalModel> licenseModels)
        {
            ApoQueryInfo.APOLicenseeFilter = GetLicenseInformation();
            ApoQueryInfo.APOLicenseeSource = licenseModels;
            ApoQueryInfo.APOSearchTypeIndex = (int)SearchType.License;
        }

        /// <summary>
        /// get license professional list according to user input.
        /// </summary>
        /// <param name="currentPageIndex">The current page index.</param>
        /// <param name="sortExpression">The sort expression.</param>
        /// <returns>LicenseProfessionalModel list</returns>
        private IList<LicenseProfessionalModel> SearchLPModel(int currentPageIndex, string sortExpression)
        {
            IList<LicenseProfessionalModel> resultList = new List<LicenseProfessionalModel>();
            LicenseProfessionalModel license = GetLicenseInformation();
            PaginationModel pageInfo = PaginationUtil.GetPageInfoByID(gdvLicenseeList.ClientID);
            pageInfo.SortExpression = sortExpression;
            pageInfo.CurrentPageIndex = currentPageIndex;
            pageInfo.CustomPageSize = gdvLicenseeList.PageSize;
            QueryFormat queryFormat = PaginationUtil.GetQueryFormatModel(pageInfo);
            bool isExistTemplateColumn = GridViewBuildHelper.IsExistTemplateColumn(gdvLicenseeList);
            LicenseProfessionalModel[] licenseList = _licenseBll.QueryLicenses(license, isExistTemplateColumn, queryFormat);

            if (licenseList != null && licenseList.Length > 0)
            {
                foreach (LicenseProfessionalModel lpModel in licenseList)
                {
                    if (lpModel != null)
                    {
                        resultList.Add(lpModel);
                    }
                }
            }

            resultList = PaginationUtil.MergeDataSource<IList<LicenseProfessionalModel>>(LPDataSource, resultList, pageInfo);

            return resultList;
        }

        /// <summary>
        /// get license professional information according to user input
        /// </summary>
        /// <returns>License Professional Model</returns>
        private LicenseProfessionalModel GetLicenseInformation()
        {
            LicenseProfessionalModel license = new LicenseProfessionalModel();
            license.city = txtCity.Text.Trim();
            license.state = txtState.Text.Trim();
            license.zip = txtZipCode.GetZip(ddlCountry.SelectedValue.Trim());
            license.address1 = txtAddress1.Text.Trim();
            license.address2 = txtAddress2.Text.Trim();
            license.address3 = txtAddress3.Text.Trim();
            license.countryCode = ddlCountry.SelectedValue;
            license.businessName = txtBusiName.Text.Trim();
            license.busName2 = txtBusiName2.Text.Trim();
            license.businessLicense = txtBusiLicense.Text.Trim();
            license.contactFirstName = txtFirstName.Text.Trim();
            license.contactMiddleName = txtMiddleInitial.Text.Trim();
            license.contactLastName = txtLastName.Text.Trim();
            license.licenseNbr = txtLicenseNumber.Text.Trim();
            license.fax = txtFax.GetPhone(ddlCountry.SelectedValue.Trim());
            license.faxCountryCode = txtFax.CountryCodeText.Trim();
            license.phone1 = txtPhone1.GetPhone(ddlCountry.SelectedValue.Trim());
            license.phone1CountryCode = txtPhone1.CountryCodeText.Trim();
            license.phone2 = txtPhone2.GetPhone(ddlCountry.SelectedValue.Trim());
            license.phone2CountryCode = txtPhone2.CountryCodeText.Trim();
            license.licenseType = ddlLicenseType.SelectedValue;
            license.typeFlag = ddlContactType.SelectedValue;
            license.socialSecurityNumber = txtSSN.Text.Trim();
            license.maskedSsn = MaskUtil.FormatSSNShow(txtSSN.Text.Trim());
            license.fein = txtFEIN.Text.Trim();
            license.agencyCode = ConfigManager.AgencyCode;
            license.contrLicNo = txtContractorLicNO.Text.Trim();
            license.contLicBusName = txtContractorBusiName.Text.Trim();

            return license;
        }
        
        /// <summary>
        /// show License info.
        /// </summary>
        /// <param name="license">license professional information</param>
        private void ShowLicenseInfo(LicenseProfessionalModel license)
        {
            if (license != null)
            {
                DropDownListBindUtil.SetCountrySelectedValue(ddlCountry, license.countryCode, false, true, true);
                ddlLicenseType.SelectedValue = license.licenseType;
                txtLicenseNumber.Text = license.licenseNbr;
                ddlContactType.SelectedValue = license.typeFlag;
                txtSSN.Text = license.socialSecurityNumber;
                txtFEIN.Text = license.fein;

                txtBusiName.Text = license.businessName;
                txtBusiName2.Text = license.busName2;
                txtBusiLicense.Text = license.businessLicense;
                txtFirstName.Text = license.contactFirstName;
                txtMiddleInitial.Text = license.contactMiddleName;
                txtLastName.Text = license.contactLastName;

                txtAddress1.Text = license.address1;
                txtAddress2.Text = license.address2;
                txtAddress3.Text = license.address3;

                txtCity.Text = license.city;
                txtState.Text = license.state;
                txtZipCode.Text = ModelUIFormat.FormatZipShow(license.zip, license.countryCode, false);

                txtPhone1.CountryCodeText = license.phone1CountryCode;
                txtPhone1.Text = ModelUIFormat.FormatPhone4EditPage(license.phone1, license.countryCode);
                txtPhone2.CountryCodeText = license.phone2CountryCode;
                txtPhone2.Text = ModelUIFormat.FormatPhone4EditPage(license.phone2, license.countryCode);
                txtFax.CountryCodeText = license.faxCountryCode;
                txtFax.Text = ModelUIFormat.FormatPhone4EditPage(license.fax, license.countryCode);

                txtContractorLicNO.Text = license.contrLicNo;
                txtContractorBusiName.Text = license.contLicBusName;
            }
        }

        /// <summary>
        /// Bind licensee list.
        /// </summary>
        /// <param name="apoQueryInfo">apo query information</param>
        private void BindSessionLicenseeList(ApoQueryInfo apoQueryInfo)
        {
            if (apoQueryInfo == null)
            {
                return;
            }

            int pageIndex = apoQueryInfo.APOLicenseePageIndex;
            DataTable dt = GridViewBuildHelper.MergeTemplateAttributes2DataTable(gdvLicenseeList, apoQueryInfo.APOLicenseeSource);

            if (!string.IsNullOrEmpty(apoQueryInfo.APOLicenseePageSort))
            {
                dt.DefaultView.Sort = apoQueryInfo.APOLicenseePageSort;
                dt = dt.DefaultView.ToTable();
            }

            gdvLicenseeList.DataSource = dt;

            if (pageIndex >= 0)
            {
                gdvLicenseeList.PageIndex = pageIndex;
            }

            gdvLicenseeList.EmptyDataText = AppSession.IsAdmin ? string.Empty : GetTextByKey("per_permitList_messagel_noRecord");
            gdvLicenseeList.DataBind();
        }

        /// <summary>
        /// show apo matched records summary information
        /// </summary>
        /// <param name="totalCount">The total count.</param>
        /// <param name="matchesRecord">The matches record</param>
        /// <param name="isClickSearchButton">if set to <c>true</c> [is click search button].</param>
        private void ShowResultInfo(int totalCount, string matchesRecord, bool isClickSearchButton)
        {
            SearchType searchType = (SearchType)int.Parse(ddlSearchType.SelectedValue.Replace("-", string.Empty));   

            if (totalCount > 0)
            {
                switch (searchType)
                {
                    case SearchType.Address:
                        RefAddressLookUpList.Visible = true;
                        RefAPOAddressList1.Visible = false;
                        RefParcelLookUpList.Visible = false;
                        RefOwnerLookUpList.Visible = false;
                        APOSearchResultInfo.Display(matchesRecord);
                        break;
                    case SearchType.Parcel:
                        RefParcelLookUpList.Visible = true;
                        RefAPOAddressList1.Visible = false;
                        RefAddressLookUpList.Visible = false;
                        RefOwnerLookUpList.Visible = false;
                        APOSearchResultInfo.Display(matchesRecord);
                        break;
                    case SearchType.Owner:
                        RefOwnerLookUpList.Visible = true;
                        RefParcelLookUpList.Visible = false;
                        RefAPOAddressList1.Visible = false;
                        RefAddressLookUpList.Visible = false;
                        APOSearchResultInfo.Display(matchesRecord);
                        break;
                    default:
                        RefAPOAddressList1.Visible = true;
                        RefAddressLookUpList.Visible = false;
                        RefParcelLookUpList.Visible = false;
                        RefOwnerLookUpList.Visible = false;

                        if (totalCount == 1)
                        {
                            if (searchType == SearchType.License && !RefAPOAddressList1.IsSingleAPOData)
                            {
                                APOSearchResultInfo.Display(matchesRecord);
                            }
                            else
                            {
                                RefAPOAddressList1.Visible = false;
                                APOSearchResultInfo.Hide();
                            }
                        }
                        else
                        {
                            APOSearchResultInfo.Display(matchesRecord);
                        }

                        break;
                }
            }
            else
            {
                if (isClickSearchButton)
                {
                    APOSearchResultInfo.Display(0);
                    RefAPOAddressList1.Visible = false;
                    RefAddressLookUpList.Visible = false;
                    RefParcelLookUpList.Visible = false;
                    RefOwnerLookUpList.Visible = false;
                }    
            }
        }

        /// <summary>
        /// display license UI.
        /// </summary>
        /// <param name="isShowResult">if set to <c>true</c> [is show result].</param>
        private void SetLicenseUI(bool isShowResult)
        {
            if (isShowResult)
            {
                gdvLicenseeList.Visible = true;
                divResultLicenseeList.Visible = true;
                LPSearchResultInfo.Display(gdvLicenseeList.CountSummary);
            }
            else
            {
                divResultLicenseeList.Visible = false;
                LPSearchResultInfo.Display(0);
            }
        }

        /// <summary>
        /// Searches the APO list by LP.
        /// </summary>
        /// <param name="licenseNumber">The license number.</param>
        /// <param name="licenseType">Type of the license.</param>
        /// <param name="currentPageIndex">The current page index.</param>
        /// <param name="sortExpression">The sort expression.</param>
        private void SearchAPOListByLP(string licenseNumber, string licenseType, int currentPageIndex, string sortExpression)
        {
            LicenseModel4WS license = new LicenseModel4WS();
            license.stateLicense = licenseNumber;
            license.licenseType = licenseType;
            license.serviceProviderCode = ConfigManager.AgencyCode;

            PaginationModel pageInfo = PaginationUtil.GetPageInfoByID(RefAPOAddressList1.ClientID + SearchType.License);
            pageInfo.SortExpression = sortExpression;
            pageInfo.CurrentPageIndex = currentPageIndex;
            pageInfo.CustomPageSize = RefAPOAddressList1.PageSize;
            pageInfo.SearchCriterias = new object[] { licenseNumber, licenseType };
            QueryFormat queryFormat = PaginationUtil.GetQueryFormatModel(pageInfo);
            ParcelInfoModel[] apoResult = _apoBll.GetAPOListByLP(license, queryFormat);
            DataTable dt = APOUtil.BuildAPODataTable(apoResult);

            if (dt.Rows.Count == 0)
            {
                APOSearchResultInfo.Hide();
            }

            dt = PaginationUtil.MergeDataSource<DataTable>(RefAPOAddressList1.RefAPODataSource, dt, pageInfo);
            ApoQueryInfo.APOAddressSource = dt;
            ApoQueryInfo.APOLicenseePageIndex = gdvLicenseeList.PageIndex;

            if (!string.IsNullOrEmpty(gdvLicenseeList.GridViewSortExpression))
            {
                ApoQueryInfo.APOLicenseePageSort = gdvLicenseeList.GridViewSortExpression + " " + gdvLicenseeList.GridViewSortDirection;
            }

            RefAPOAddressList1.Visible = true;
            RefAPOAddressList1.ExportFileName = "APOList";
            RefAPOAddressList1.ComponentType = (int)SearchType.License;
            RefAPOAddressList1.BindDataSource(dt, true, APOShowType.ShowAddressByLp);
            divAPOResult.Visible = true;
            RefAddressLookUpList.Visible = false;
            RefParcelLookUpList.Visible = false;
            RefOwnerLookUpList.Visible = false;
            ScriptManager.RegisterClientScriptBlock(Page, GetType(), "GotoAPOResult", "scrollIntoView('APOResult');", true);
        }

        /// <summary>
        /// Searches the APO list by cap.
        /// </summary>
        /// <param name="capID1">The cap id1.</param>
        /// <param name="capID2">The cap id2.</param>
        /// <param name="capID3">The cap id3.</param>
        /// <param name="serviceProviderCode">The service provider code.</param>
        /// <param name="currentPageIndex">The current page index.</param>
        /// <param name="sortExpression">The sort expression.</param>
        private void SearchAPOListByCap(string capID1, string capID2, string capID3, string serviceProviderCode, int currentPageIndex, string sortExpression)
        {
            CapModel4WS capModel = new CapModel4WS();
            CapIDModel4WS capIdModel = new CapIDModel4WS();
            capIdModel.id1 = capID1;
            capIdModel.id2 = capID2;
            capIdModel.id3 = capID3;
            capIdModel.serviceProviderCode = serviceProviderCode;
            capModel.capID = capIdModel;

            PaginationModel pageInfo = PaginationUtil.GetPageInfoByID(RefAPOAddressList1.ClientID + SearchType.Permit);
            pageInfo.SortExpression = sortExpression;
            pageInfo.CurrentPageIndex = currentPageIndex;
            pageInfo.CustomPageSize = RefAPOAddressList1.PageSize;
            pageInfo.SearchCriterias = new object[] { capID1, capID2, capID3, serviceProviderCode };
            QueryFormat queryFormat = PaginationUtil.GetQueryFormatModel(pageInfo);

            //must pass agency code of current ACA site instead of current record
            ParcelInfoModel[] apoResult = _apoBll.GetAPOListByCap(ConfigManager.AgencyCode, capModel, AppSession.User.UserSeqNum, queryFormat);
            DataTable dt = APOUtil.BuildAPODataTable(apoResult);

            if (dt.Rows.Count == 0)
            {
                APOSearchResultInfo.Hide();
            }

            dt = PaginationUtil.MergeDataSource<DataTable>(RefAPOAddressList1.RefAPODataSource, dt, pageInfo);
            ApoQueryInfo.APOAddressSource = dt;
            ApoQueryInfo.APOPermitPageIndex = dgvPermitList.PageIndex;

            if (!string.IsNullOrEmpty(dgvPermitList.GridViewSortExpression))
            {
                ApoQueryInfo.APOPermitPageSort = dgvPermitList.GridViewSortExpression + " " + dgvPermitList.GridViewSortDirection;
            }

            RefAPOAddressList1.Visible = true;
            RefAPOAddressList1.ExportFileName = "APOList";
            RefAPOAddressList1.ComponentType = (int)SearchType.Address;
            RefAPOAddressList1.BindDataSource(dt, true, APOShowType.ShowAddressByCap);
            divAPOResult.Visible = true;
            RefAddressLookUpList.Visible = false;
            RefParcelLookUpList.Visible = false;
            RefOwnerLookUpList.Visible = false;
            ScriptManager.RegisterClientScriptBlock(Page, GetType(), "GotoAPOResult", "scrollIntoView('APOResult');", true);
        }

        /// <summary>
        /// Handles the GridViewDownloadAll event of the APO control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="Accela.Web.Controls.GridViewDownloadEventArgs"/> instance containing the event data.</param>
        private void RefAPOAddressList1_GridViewDownloadAll(object sender, GridViewDownloadEventArgs e)
        {
            SearchType searchType = (SearchType)ApoQueryInfo.APOSearchTypeIndex;

            switch (searchType)
            {
                case SearchType.Address:
                    GridViewBuildHelper.DownloadAll(sender, e, GetAddressByQueryFormat);
                    break;
                case SearchType.Parcel:
                    GridViewBuildHelper.DownloadAll(sender, e, GetParcelByQueryFormat, APOListForParcelExportFormat);
                    break;
                case SearchType.Owner:
                    GridViewBuildHelper.DownloadAll(sender, e, GetOwnerByQueryFormat);
                    break;
                case SearchType.License:
                    GridViewBuildHelper.DownloadAll(sender, e, GetAPOListByLPQueryFormat);
                    break;
                case SearchType.Permit:
                    GridViewBuildHelper.DownloadAll(sender, e, GetAPOListByCapQueryFormat);
                    break;
            }
        }

        /// <summary>
        /// Handles the GridViewDownloadAll event of the Associated Address.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="Accela.Web.Controls.GridViewDownloadEventArgs"/> instance containing the event data.</param>
        private void AssociatedAddressList_GridViewDownloadAll(object sender, GridViewDownloadEventArgs e)
        {
            GridViewBuildHelper.DownloadAll(sender, e, GetAssociatedAddressByQueryFormat);
        }

        /// <summary>
        /// Handles the GridViewDownloadAll event of the Associated Parcel.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="Accela.Web.Controls.GridViewDownloadEventArgs"/> instance containing the event data.</param>
        private void AssociatedParcelList_GridViewDownloadAll(object sender, GridViewDownloadEventArgs e)
        {
            GridViewBuildHelper.DownloadAll(sender, e, GetAssociatedParcelByQueryFormat, AssocitedParcelExportFormat);
        }

        /// <summary>
        /// Handles the GridViewDownloadAll event of the Associated Owner.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="Accela.Web.Controls.GridViewDownloadEventArgs"/> instance containing the event data.</param>
        private void AssociatedOwnerList_GridViewDownloadAll(object sender, GridViewDownloadEventArgs e)
        {
            GridViewBuildHelper.DownloadAll(sender, e, GetAssociatedOwnerByQueryFormat);
        }

                /// <summary>
        /// Handles the GridViewDownloadAll event of the Associated Parcel and Address.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="Accela.Web.Controls.GridViewDownloadEventArgs"/> instance containing the event data.</param>
        private void AssociatedAPOList_GridViewDownloadAll(object sender, GridViewDownloadEventArgs e)
        {
            GridViewBuildHelper.DownloadAll(sender, e, GetAssociatedAPOByQueryFormat);
        }

        #region Get Download Data Source

        /// <summary>
        /// Get the APO list by address with the search criteria.
        /// </summary>
        /// <param name="queryFormat">the query format</param>
        /// <returns>download result model that contains the search table result</returns>
        private DownloadResultModel GetAddressByQueryFormat(QueryFormat queryFormat)
        {
            RefAddressModel refAddressModel = addressLookupForm.GetRefAddress();
            SearchResultModel result = _apoBll.GetRefAddressList(ConfigManager.AgencyCode, refAddressModel, queryFormat);

            DownloadResultModel model = new DownloadResultModel();
            model.StartDBRow = result.startRow;
            model.DataSource = APOUtil.BuildAddressDataTable(result.resultList);

            return model;
        }

        /// <summary>
        /// Get the APO list by owner with the search criteria.
        /// </summary>
        /// <param name="queryFormat">the query format</param>
        /// <returns>download result model that contains the search table result</returns>
        private DownloadResultModel GetOwnerByQueryFormat(QueryFormat queryFormat)
        {
            OwnerCompModel ownerModel = ownerLookupForm.GetOwner();
            SearchResultModel result = _apoBll.GetRefOwnerList(ConfigManager.AgencyCode, ownerModel, queryFormat);

            DownloadResultModel model = new DownloadResultModel();
            model.StartDBRow = result.startRow;
            model.DataSource = APOUtil.BuildOwnerDataTable(result.resultList);

            return model;
        }

        /// <summary>
        /// Get the APO list by parcel with the search criteria.
        /// </summary>
        /// <param name="queryFormat">the query format</param>
        /// <returns>download result model that contains the search table result</returns>
        private DownloadResultModel GetParcelByQueryFormat(QueryFormat queryFormat)
        {
            ParcelModel parcelModel = parcelLookupForm.GetParcel();
            SearchResultModel result = _apoBll.GetRefParcelList(ConfigManager.AgencyCode, parcelModel, queryFormat);

            DownloadResultModel model = new DownloadResultModel();
            model.StartDBRow = result.startRow;
            model.DataSource = APOUtil.BuildParcelDataTable(result.resultList);

            return model;
        }

        /// <summary>
        /// Get the permit list by the search criteria.
        /// </summary>
        /// <param name="queryFormat">the query format</param>
        /// <returns>download result model that contains the search table result</returns>
        private DownloadResultModel GetPermitByQueryFormat(QueryFormat queryFormat)
        {
            CapModel4WS capModel = GeneralSearchModel.CapModel;
            List<string> moduleNameList = GeneralSearchModel.ModuleNameList;
            string[] hiddenViewEltNames = GeneralSearchModel.HiddenViewEltNames;

            SearchResultModel capResult = _capBll.GetCapList4ACA(capModel, queryFormat, AppSession.User.UserSeqNum, null, false, moduleNameList, true, hiddenViewEltNames);
            DataTable result = ConvertSimpleCapModel2DataTable(capResult.resultList);

            DownloadResultModel model = new DownloadResultModel();
            model.StartDBRow = capResult.startRow;
            model.DataSource = result;

            return model;
        }

        /// <summary>
        /// Get the licensee list by the search criteria.
        /// </summary>
        /// <param name="queryFormat">the query format</param>
        /// <returns>download result model that contains the search table result</returns>
        private DownloadResultModel GetLicenseesByQueryFormat(QueryFormat queryFormat)
        {
            LicenseProfessionalModel licenseProfessionalModel = GetLicenseInformation();
            LicenseProfessionalModel[] licenseList = _licenseBll.QueryLicenses(licenseProfessionalModel, IsExistTemplateColumn, queryFormat);

            IList<LicenseProfessionalModel> resultList = new List<LicenseProfessionalModel>();
            if (licenseList != null && licenseList.Length > 0)
            {
                foreach (LicenseProfessionalModel lpModel in licenseList)
                {
                    if (lpModel != null)
                    {
                        resultList.Add(lpModel);
                    }
                }
            }

            DataTable result = GridViewBuildHelper.MergeTemplateAttributes2DataTable(gdvLicenseeList, resultList);

            DownloadResultModel model = new DownloadResultModel();
            model.StartDBRow = 0;
            model.DataSource = result;

            return model;
        }

        /// <summary>
        /// Get the associated address list by the selected parcel.
        /// </summary>
        /// <param name="queryFormat">The query format</param>
        /// <returns>Download result model that contains the search table result</returns>
        private DownloadResultModel GetAssociatedAddressByQueryFormat(QueryFormat queryFormat)
        {
            RefAddressModel[] searchResult = _apoBll.GetRefAddressListByParcelPK(ConfigManager.AgencyCode, ParcelPK, false);

            DownloadResultModel model = new DownloadResultModel();
            model.DataSource = APOUtil.BuildAddressDataTable(searchResult);

            return model;
        }

        /// <summary>
        /// Get the associated parcel list by the selected address.
        /// </summary>
        /// <param name="queryFormat">The query format</param>
        /// <returns>Download result model that contains the search table result</returns>
        private DownloadResultModel GetAssociatedParcelByQueryFormat(QueryFormat queryFormat)
        {
            IParcelBll parcelBll = ObjectFactory.GetObject<IParcelBll>();
            ParcelModel[] parcels = parcelBll.GetParcelListByRefAddressPK(ConfigManager.AgencyCode, RefAddressPK, queryFormat);
            DataTable parcelTable = APOUtil.BuildParcelDataTable(parcels);

            DownloadResultModel model = new DownloadResultModel();
            model.DataSource = parcelTable;

            return model;
        }

        /// <summary>
        /// Get the associated owner list by the selected parcel.
        /// </summary>
        /// <param name="queryFormat">The query format</param>
        /// <returns>Download result model that contains the search table result</returns>
        private DownloadResultModel GetAssociatedOwnerByQueryFormat(QueryFormat queryFormat)
        {
            SearchResultModel searchResult = _apoBll.GetOwnerListByParcelPKs(ConfigManager.AgencyCode, new[] { ParcelPK }, false, queryFormat);
            DownloadResultModel model = new DownloadResultModel();

            model.StartDBRow = searchResult.startRow;
            model.DataSource = APOUtil.BuildOwnerDataTable(searchResult.resultList);

            return model;
        }

                /// <summary>
        /// Get the associated parcel and address list by the selected owner.
        /// </summary>
        /// <param name="queryFormat">The query format</param>
        /// <returns>Download result model that contains the search table result</returns>
        private DownloadResultModel GetAssociatedAPOByQueryFormat(QueryFormat queryFormat)
        {
            SearchResultModel searchResult = _apoBll.GetAddressListByOwnerPK(ConfigManager.AgencyCode, RefOwnerPK, queryFormat);
            DownloadResultModel model = new DownloadResultModel();

            model.StartDBRow = searchResult.startRow;
            model.DataSource = APOUtil.BuildAPODataTable(searchResult.resultList);

            return model;
        }   

        /// <summary>
        /// Get the APO list by CAP
        /// </summary>
        /// <param name="queryFormat">the query format</param>
        /// <returns>download result model that contains the search table result</returns>
        private DownloadResultModel GetAPOListByCapQueryFormat(QueryFormat queryFormat)
        {
            PaginationModel pageInfo = PaginationUtil.GetPageInfoByID(RefAPOAddressList1.ClientID + SearchType.Permit);

            CapModel4WS capModel = new CapModel4WS();
            CapIDModel4WS capIdModel = new CapIDModel4WS();
            capIdModel.id1 = pageInfo.SearchCriterias[0].ToString();
            capIdModel.id2 = pageInfo.SearchCriterias[1].ToString();
            capIdModel.id3 = pageInfo.SearchCriterias[2].ToString();
            capIdModel.serviceProviderCode = ConfigManager.AgencyCode;
            capModel.capID = capIdModel;

            // must pass agency code of current ACA site instead of current record
            ParcelInfoModel[] apoResult = _apoBll.GetAPOListByCap(ConfigManager.AgencyCode, capModel, AppSession.User.UserSeqNum, queryFormat);
            DataTable result = APOUtil.BuildAPODataTable(apoResult);

            DownloadResultModel model = new DownloadResultModel();
            model.StartDBRow = 0;
            model.DataSource = result;

            return model;
        }
        
        /// <summary>
        /// Get the APO list by LP
        /// </summary>
        /// <param name="queryFormat">the query format</param>
        /// <returns>download result model that contains the search table result</returns>
        private DownloadResultModel GetAPOListByLPQueryFormat(QueryFormat queryFormat)
        {
            PaginationModel pageInfo = PaginationUtil.GetPageInfoByID(RefAPOAddressList1.ClientID + SearchType.License);

            LicenseModel4WS license = new LicenseModel4WS();
            license.stateLicense = pageInfo.SearchCriterias[0].ToString();
            license.licenseType = pageInfo.SearchCriterias[1].ToString();
            license.serviceProviderCode = ConfigManager.AgencyCode;

            ParcelInfoModel[] apoResult = _apoBll.GetAPOListByLP(license, queryFormat);
            DataTable result = APOUtil.BuildAPODataTable(apoResult);

            DownloadResultModel model = new DownloadResultModel();
            model.StartDBRow = 0;
            model.DataSource = result;

            return model;
        }

        #endregion Get Download Data Source

        /// <summary>
        /// Set the Licensees list format for the export.
        /// </summary>
        /// <param name="dataRow">The data row.</param>
        /// <param name="visibleColumns">The visible columns.</param>
        /// <returns>The formatted export content.</returns>
        private Dictionary<string, string> LicenseesExportFormat(DataRow dataRow, List<string> visibleColumns)
        {
            Dictionary<string, string> result = new Dictionary<string, string>();
            const string ColumnTypeFlag = "typeFlag";

            if (visibleColumns.Contains(ColumnTypeFlag))
            {
                object objContactType = dataRow[ColumnTypeFlag];

                if (objContactType != null)
                {
                    string contactType = DropDownListBindUtil.GetTypeFlagTextByValue(objContactType.ToString());
                    result.Add(ColumnTypeFlag, contactType);
                }
            }

            return result;
        }

        /// <summary>
        /// The APO list for parcel's export format.
        /// </summary>
        /// <param name="dataRow">The data row.</param>
        /// <param name="visibleColumns">The visible columns.</param>
        /// <returns>The formatted export content.</returns>
        private Dictionary<string, string> APOListForParcelExportFormat(DataRow dataRow, List<string> visibleColumns)
        {
            Dictionary<string, string> result = new Dictionary<string, string>();
            const string ColumnParcelStatus = "ParcelStatus";

            if (visibleColumns.Contains(ColumnParcelStatus))
            {
                object objParcelStatus = dataRow[ColumnParcelStatus];
                if (objParcelStatus != null)
                {
                    result.Add(ColumnParcelStatus, RefAPOAddressList1.GetDisplayParcelStatus(objParcelStatus.ToString()));
                }
            }

            return result;
        }

        /// <summary>
        /// The associated parcel list export format.
        /// </summary>
        /// <param name="dataRow">The data row.</param>
        /// <param name="visibleColumns">The visible columns.</param>
        /// <returns>The formatted export content.</returns>
        private Dictionary<string, string> AssocitedParcelExportFormat(DataRow dataRow, List<string> visibleColumns)
        {
            Dictionary<string, string> result = new Dictionary<string, string>();
            const string ColumnParcelStatus = "ParcelStatus";

            if (visibleColumns.Contains(ColumnParcelStatus))
            {
                object objParcelStatus = dataRow[ColumnParcelStatus];
                if (objParcelStatus != null)
                {
                    result.Add(ColumnParcelStatus, AssociatedParcelList.GetDisplayParcelStatus(objParcelStatus.ToString()));
                }
            }

            return result;
        }

        /// <summary>
        /// Valid empty search condition if search external APO data.
        /// </summary>
        /// <param name="searchType">Search Type</param>
        /// <returns>IsEmpty flag</returns>
        private bool ValidateIsEmptyForExternalSearchCondition(SearchType searchType)
        {
            bool isEmpty = false;

            switch (searchType)
            {
                case SearchType.Address:
                    isEmpty = APOUtil.IsEmpty(addressLookupForm.GetRefAddress()) && StandardChoiceUtil.IsExternalAddressSource();
                    break;
                case SearchType.Parcel:
                    isEmpty = APOUtil.IsEmpty(parcelLookupForm.GetParcel()) && StandardChoiceUtil.IsExternalParcelSource();
                    break;
                case SearchType.Owner:
                    isEmpty = APOUtil.IsEmpty(ownerLookupForm.GetOwner()) && StandardChoiceUtil.IsExternalOwnerSource();
                    break;
            }

            if (isEmpty)
            {
                divAPOResult.Visible = false;

                switch (searchType)
                {
                    case SearchType.Address:
                        PackageAPObyAddress(null);
                        break;
                    case SearchType.Parcel:
                        PackageAPObyParcel(null);
                        break;
                    case SearchType.Owner:
                        PackageAPObyOwner(null);
                        break;
                }

                MessageUtil.ShowMessageByControl(Page, MessageType.Error, GetTextByKey("aca_apo_msg_searchcriteria_required"));
            }

            return isEmpty;
        }

        /// <summary>
        /// Gets the ref addressPK model by data row.
        /// </summary>
        /// <param name="addressRow">Address data row</param>
        /// <returns>Ref addressPK model</returns>
        private RefAddressModel InitRefAddressPKModel(DataRow addressRow)
        {
            // Initial the Ref Address PK Model.
            string addressId = addressRow["AddressID"] != null ? addressRow["AddressID"].ToString() : string.Empty;
            string addressUid = addressRow["AddressUid"] != null ? addressRow["AddressUid"].ToString() : string.Empty;
            string addressSequence = addressRow["AddressSequenceNumber"] != null ? addressRow["AddressSequenceNumber"].ToString() : string.Empty;

            RefAddressModel refAddressPK = new RefAddressModel();
            refAddressPK.refAddressId = StringUtil.ToLong(addressId);
            refAddressPK.UID = addressUid;
            refAddressPK.sourceNumber = StringUtil.ToInt(addressSequence);

            return refAddressPK;
        }

        /// <summary>
        /// Gets the ref parcelPK model by data row.
        /// </summary>
        /// <param name="parcelRow">Parcel data row</param>
        /// <returns>Ref parcelPK model</returns>
        private ParcelModel InitRefParcelPKModel(DataRow parcelRow)
        {
            // Initial the Ref Parcel PK Model.
            string parcelNumber = parcelRow["ParcelNumber"] != null
                                      ? parcelRow["ParcelNumber"].ToString()
                                      : string.Empty;
            string parcelUid = parcelRow["ParcelUID"] != null
                                      ? parcelRow["ParcelUID"].ToString()
                                      : string.Empty;
            string parcelSequence = parcelRow["ParcelSequenceNumber"] != null
                                      ? parcelRow["ParcelSequenceNumber"].ToString()
                                      : string.Empty;

            ParcelModel refParcelPK = new ParcelModel();
            refParcelPK.parcelNumber = parcelNumber;
            refParcelPK.UID = parcelUid;
            refParcelPK.sourceSeqNumber = StringUtil.ToLong(parcelSequence);

            return refParcelPK;
        }

        /// <summary>
        /// Gets the associated address records by selected parcel record.
        /// </summary>
        /// <param name="currentPageIndex">Current Page Index</param>
        /// <param name="sortExpression">Sort Expression</param>
        private void LoadAssociatedAddressData(int currentPageIndex, string sortExpression)
        {
            if (ParcelPK == null)
            {
                return;
            }

            string addressTitle = DataUtil.StringFormat(GetTextByKey("aca_apo_label_parcellist_associatedaddress"), ParcelPK.parcelNumber);
            lblRefAddressList.Text = addressTitle;

            PaginationModel pageInfo = PaginationUtil.GetPageInfoByID(AssociatedAddressList.ClientID);
            pageInfo.SortExpression = sortExpression;
            pageInfo.CurrentPageIndex = currentPageIndex;
            pageInfo.CustomPageSize = AssociatedAddressList.PageSize;
            RefAddressModel[] searchResult = _apoBll.GetRefAddressListByParcelPK(ConfigManager.AgencyCode, ParcelPK, false);

            DataTable dt = APOUtil.BuildAddressDataTable(searchResult);
            dt = PaginationUtil.MergeDataSource<DataTable>(AssociatedAddressList.RefAddressDataSource, dt, pageInfo);

            AssociatedAddressList.BindDataSource(dt);
        }

        /// <summary>
        /// Load Associated Parcel Data.
        /// </summary>
        /// <param name="dt">Parcel data table</param>
        /// <param name="fullAddress">Full Address</param>
        private void LoadAssociatedParcelData(DataTable dt, string fullAddress)
        {
            if (!string.IsNullOrEmpty(fullAddress))
            {
                string parcelTitle = DataUtil.StringFormat(GetTextByKey("aca_apo_label_addresslist_associatedparcel"), fullAddress);
                lblRefParcelList.Text = parcelTitle;
            }

            AssociatedParcelList.BindDataSource(dt);
        }

        /// <summary>
        /// Gets the associated parcel records by selected address record.
        /// </summary>
        /// <param name="currentPageIndex">Current Page Index</param>
        /// <param name="sortExpression">Sort Expression</param>
        /// <returns>Parcel data table</returns>
        private DataTable GetAssociatedParcelDataTable(int currentPageIndex, string sortExpression)
        {
            if (RefAddressPK == null)
            {
                return null;
            }

            PaginationModel pageInfo = PaginationUtil.GetPageInfoByID(AssociatedParcelList.ClientID);
            pageInfo.SortExpression = sortExpression;
            pageInfo.CurrentPageIndex = currentPageIndex;
            pageInfo.CustomPageSize = AssociatedParcelList.PageSize;
            QueryFormat queryFormat = PaginationUtil.GetQueryFormatModel(pageInfo);
            IParcelBll parcelBll = ObjectFactory.GetObject<IParcelBll>();
            ParcelModel[] parcels = parcelBll.GetParcelListByRefAddressPK(ConfigManager.AgencyCode, RefAddressPK, queryFormat);
            DataTable parcelTable = APOUtil.BuildParcelDataTable(parcels);
            parcelTable = PaginationUtil.MergeDataSource<DataTable>(AssociatedParcelList.RefParcelDataSource, parcelTable, pageInfo);

            return parcelTable;
        }

        /// <summary>
        /// Gets the parcel associated owner data
        /// </summary>
        /// <param name="currentPageIndex">Current pageIndex</param>
        /// <param name="sortExpression">Sort Expression</param>
        private void LoadAssociatedOwnerData(int currentPageIndex, string sortExpression)
        {
            string ownerTitle = DataUtil.StringFormat(GetTextByKey("aca_apo_label_parcellist_associatedowner"), ParcelPK.parcelNumber);
            lblParcelAssociatedOwnerList.Text = ownerTitle;

            if (ParcelPK == null)
            {
                return;
            }

            PaginationModel pageInfo = PaginationUtil.GetPageInfoByID(AssociatedOwnerList.ClientID);
            pageInfo.SortExpression = sortExpression;
            pageInfo.CurrentPageIndex = currentPageIndex;
            pageInfo.CustomPageSize = AssociatedOwnerList.PageSize;
            QueryFormat queryFormat = PaginationUtil.GetQueryFormatModel(pageInfo);

            IAPOBll apoBll = ObjectFactory.GetObject<IAPOBll>();
            SearchResultModel searchResult = apoBll.GetOwnerListByParcelPKs(ConfigManager.AgencyCode, new[] { ParcelPK }, false, queryFormat);

            pageInfo.StartDBRow = searchResult.startRow;

            DataTable dt = APOUtil.BuildOwnerDataTable(searchResult.resultList);
            dt = PaginationUtil.MergeDataSource<DataTable>(AssociatedOwnerList.OwnerDataSource, dt, pageInfo);

            AssociatedOwnerList.BindDataSource(dt);
        }

        /// <summary>
        /// Gets the owner associated parcel and address data.
        /// </summary>
        /// <param name="currentPageIndex">Current pageIndex</param>
        /// <param name="sortExpression">Sort Expression</param>
        private void LoadAssociatedAPOData(int currentPageIndex, string sortExpression)
        {
            string sectionTitle = DataUtil.StringFormat(GetTextByKey("aca_apo_label_ownerlist_associateddata"), RefOwnerPK.ownerFullName);
            lblOwnerAssociatedAPOList.Text = sectionTitle;

            if (RefOwnerPK == null)
            {
                return;
            }

            PaginationModel pageInfor = PaginationUtil.GetPageInfoByID(AssociatedAPOList.ClientID);
            pageInfor.SortExpression = sortExpression;
            pageInfor.CurrentPageIndex = currentPageIndex;
            pageInfor.CustomPageSize = AssociatedAPOList.PageSize;
            QueryFormat queryFormat = PaginationUtil.GetQueryFormatModel(pageInfor);
            SearchResultModel result = _apoBll.GetAPOListByOwner(ConfigManager.AgencyCode, RefOwnerPK, queryFormat, false);
            pageInfor.StartDBRow = result.startRow;
            AssociatedAPOList.ComponentType = (int)SearchType.Owner;

            DataTable dt = null;

            if (APOUtil.IsAssociatedParcelData(result.resultList))
            {
                dt = APOUtil.BuildAPODataTable(result.resultList);
                dt = PaginationUtil.MergeDataSource(AssociatedAPOList.RefAPODataSource, dt, pageInfor);
            }

            AssociatedAPOList.BindDataSource(dt);
        }

        #endregion Private Methods
    }
}
