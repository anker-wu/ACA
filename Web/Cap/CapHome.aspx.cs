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
 *      $Id: CapHome.aspx.cs 278359 2014-09-02 08:34:43Z ACHIEVO\james.shi $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;

using Accela.ACA.BLL;
using Accela.ACA.BLL.APO;
using Accela.ACA.BLL.Cap;
using Accela.ACA.BLL.Common;
using Accela.ACA.BLL.People;
using Accela.ACA.Common;
using Accela.ACA.Common.Common;
using Accela.ACA.Common.Log;
using Accela.ACA.Common.Util;
using Accela.ACA.Web.AppCode.Common;
using Accela.ACA.Web.Common;
using Accela.ACA.Web.Common.Control;
using Accela.ACA.WSProxy;
using Accela.Web.Controls;
using log4net;

namespace Accela.ACA.Web.Cap
{
    /// <summary>
    /// This class provide the ability to operation CAP home. 
    /// </summary>
    public partial class CapHome : BasePage, IReportVariable
    {
        #region Fields

        /// <summary>
        /// permit for grid view
        /// </summary>
        private const string PERMIT_GRIDVIEW = "ctl00_PlaceHolderMain_PermitList";

        /// <summary>
        /// search result GV.
        /// </summary>
        private const string SEARCHRESULT_GRIDVIEW = "ctl00_PlaceHolderMain_dgvPermitList";

        /// <summary>
        /// logger object.
        /// </summary>
        private static readonly ILog Logger = LogFactory.Instance.GetLogger(typeof(CapHome));

        /// <summary>
        /// The general search model
        /// </summary>
        private GeneralSearchModel _generalSearchModel;

        /// <summary>
        /// The address search model
        /// </summary>
        private RefAddressModel _addressSearchModel;

        /// <summary>
        /// The license professional search model
        /// </summary>
        private LicenseProfessionalModel _lpSearchModel;

        /// <summary>
        /// The contact search model
        /// </summary>
        private CapContactModel4WS _capContactSearchModel;

        /// <summary>
        /// The license search model
        /// </summary>
        private LicenseModel _licenseSearchModel;

        /// <summary>
        /// The cap search model
        /// </summary>
        private CapModel4WS _capSearchModel;

        /// <summary>
        /// The cap general search model
        /// </summary>
        private CapModel4WS _capGCSearchModel;

        #endregion Fields

        #region Properties

        /// <summary>
        /// Gets the selected caps' alt ids.
        /// </summary>
        string[] IReportVariable.CapAltIDs
        {
            get
            {
                string[] myPermitsAltIDs = PermitList.GetSelectedAltIDs();
                string[] searchedPermitsAltIDs = null;

                if (dvSearchList.Visible)
                {
                    searchedPermitsAltIDs = dgvPermitList.GetSelectedAltIDs();
                }

                // Union the cap ids of my permit list and searched permit list
                IList<string> unionAltIDs = DataUtil.GetUnionArrays(myPermitsAltIDs, searchedPermitsAltIDs);

                string[] altIDs = null;

                if (unionAltIDs != null && unionAltIDs.Count > 0)
                {
                    altIDs = new string[unionAltIDs.Count];
                    unionAltIDs.CopyTo(altIDs, 0);
                }

                return altIDs;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether is right to left.
        /// </summary>
        protected bool IsRightToLeft { get; set; }

        /// <summary>
        /// Gets a value indicating whether need to display Map on page.
        /// </summary>
        protected bool IsShowMap
        {
            get
            {
                // Admin mode, Hidden the MAP because the slow to load VE scripts from the third site.
                if (AppSession.IsAdmin)
                {
                    return false;
                }

                return StandardChoiceUtil.IsShowMap4ShowObject(ModuleName);
            }
        }

        /// <summary>
        /// Gets or sets GV for address.
        /// </summary>
        private DataTable GridViewDataSourceForAddress
        {
            get
            {
                return (DataTable)ViewState["AddressModules"];
            }

            set
            {
                ViewState["AddressModules"] = value;
            }
        }

        /// <summary>
        /// Gets or sets GV for address.
        /// </summary>
        private DataTable GridViewDataSourceForLicense
        {
            get
            {
                return (DataTable)ViewState["LicenseModules"];
            }

            set
            {
                ViewState["LicenseModules"] = value;
            }
        }

        /// <summary>
        /// Gets or sets GV for trade name.
        /// </summary>
        private DataTable GridViewDataSourceForTradeName
        {
            get
            {
                return (DataTable)ViewState["TradeNameModules"];
            }

            set
            {
                ViewState["TradeNameModules"] = value;
            }
        }

        /// <summary>
        /// Gets or sets query Info.
        /// </summary>
        private PermitQueryInfo QueryInfo
        {
            get
            {
                if (Session[SessionConstant.SESSION_PERMITQUERYINFO] != null)
                {
                    PermitQueryInfo info = (PermitQueryInfo)Session[SessionConstant.SESSION_PERMITQUERYINFO];

                    if (info.ModuleName == ModuleName)
                    {
                        return info;
                    }
                    else
                    {
                        Session[SessionConstant.SESSION_PERMITQUERYINFO] = null;
                    }
                }

                return null;
            }

            set
            {
                Session[SessionConstant.SESSION_PERMITQUERYINFO] = value;
            }
        }

        /// <summary>
        /// Gets the address search model
        /// </summary>
        private RefAddressModel AddressSearchModel
        {
            get
            {
                if (_addressSearchModel == null)
                {
                    _addressSearchModel = searchByAddress.GetRefAddressModel();
                }

                return _addressSearchModel;
            }
        }

        /// <summary>
        /// Gets the license professional search model.
        /// </summary>
        private LicenseProfessionalModel LPSearchModel
        {
            get
            {
                if (_lpSearchModel == null)
                {
                    _lpSearchModel = new LicenseProfessionalModel();

                    _lpSearchModel.licenseType = ddlLicenseType.SelectedValue.Trim();
                    _lpSearchModel.licenseNbr = txtLicenseNumber.Text.Trim();
                    _lpSearchModel.contactFirstName = txtFirstName.Text.Trim();
                    _lpSearchModel.contactLastName = txtLastName.Text.Trim();
                    _lpSearchModel.businessName = txtBusiName.Text.Trim();
                    _lpSearchModel.busName2 = txtBusiName2.Text.Trim();
                    _lpSearchModel.businessLicense = txtBusiLicense.Text.Trim();
                    _lpSearchModel.contrLicNo = txtContractorLicNO.Text.Trim();
                    _lpSearchModel.contLicBusName = txtContractorBusiName.Text.Trim();
                    _lpSearchModel.typeFlag = ddlContactType.SelectedValue.Trim();
                    _lpSearchModel.socialSecurityNumber = txtSSN.Text.Trim();
                    _lpSearchModel.maskedSsn = MaskUtil.FormatSSNShow(txtSSN.Text.Trim());
                    _lpSearchModel.fein = txtFEIN.Text.Trim();
                }

                return _lpSearchModel;
            }
        }

        /// <summary>
        /// Gets the contact search model
        /// </summary>
        private CapContactModel4WS CapContactSearchModel
        {
            get
            {
                if (_capContactSearchModel == null)
                {
                    _capContactSearchModel = contactSearchForm.GetCapContactModel();
                }

                return _capContactSearchModel;
            }
        }

        /// <summary>
        /// Gets the license search model
        /// </summary>
        private LicenseModel LicenseSearchModel
        {
            get
            {
                if (_licenseSearchModel == null)
                {
                    _licenseSearchModel = new LicenseModel();

                    if (ddlTradeNameType.SelectedIndex != 0)
                    {
                        _licenseSearchModel.licenseType = ddlTradeNameType.SelectedValue.Trim();
                    }

                    if (ddlTradeNameRecordStatus.SelectedIndex != 0)
                    {
                        _licenseSearchModel.licenseStatus = ddlTradeNameRecordStatus.SelectedValue.Trim();
                    }

                    _licenseSearchModel.businessName = txtEnglishTradeName.Text.Trim();
                    _licenseSearchModel.busName2 = txtArabicTradeName.Text.Trim();
                    _licenseSearchModel.serviceProviderCode = ConfigManager.AgencyCode;
                }

                return _licenseSearchModel;
            }
        }

        /// <summary>
        /// Gets the cap search model
        /// </summary>
        private CapModel4WS CapSearchModel
        {
            get
            {
                if (_capSearchModel == null)
                {
                    _capSearchModel = new CapModel4WS();

                    //Permit information
                    _capSearchModel.altID = txtPermitNumber.Text.Trim();
                    _capSearchModel.specialText = txtProjectName.Text.Trim();

                    if (txtStartDate.Visible && !string.IsNullOrEmpty(txtStartDate.Text.Trim()))
                    {
                        _capSearchModel.fileDate = I18nDateTimeUtil.ConvertDateTimeStringFromUIToWebService(txtStartDate.Text.Trim());
                    }

                    if (txtEndDate.Visible && !string.IsNullOrEmpty(txtEndDate.Text.Trim()))
                    {
                        _capSearchModel.endFileDate = I18nDateTimeUtil.FormatToDateTimeStringForWebService(I18nDateTimeUtil.ParseFromUI(txtEndDate.Text.Trim()).AddDays(1).AddSeconds(-1));
                    }

                    _capSearchModel.moduleName = ModuleName;

                    CapTypeModel capTypeModel = new CapTypeModel();

                    string selectedAgencyCode = StandardChoiceUtil.IsSuperAgency() ? null : ConfigManager.AgencyCode;

                    if (!string.IsNullOrEmpty(ddlPermitSubAgency.SelectedValue))
                    {
                        selectedAgencyCode = ddlPermitSubAgency.SelectedValue;
                    }

                    capTypeModel.serviceProviderCode = selectedAgencyCode;

                    if (!string.IsNullOrEmpty(ddlPermitType.SelectedValue))
                    {
                        string[] capLevels = ddlPermitType.SelectedValue.Trim().Split('/');
                        capTypeModel.resAlias = ddlPermitType.SelectedItem.Text;
                        capTypeModel.moduleName = ModuleName;
                        capTypeModel.group = capLevels[0];
                        capTypeModel.type = capLevels[1];
                        capTypeModel.subType = capLevels[2];
                        capTypeModel.category = capLevels[3];
                    }

                    _capSearchModel.capType = capTypeModel;

                    //if search by permit for ASI search form has expanded, and criteria isn't empty.It need add search criteria.
                    if (ACAConstant.COMMON_Y.Equals(hfASIExpanded.Value) && !asiPermitForm.IsEmptySearchCriteria())
                    {
                        _capSearchModel.appSpecificInfoGroups = asiPermitForm.GetAppSpecInfo();
                    }

                    //if search by permit for ASIT search form has expanded, and criteria isn't empty.It need add search criteria.
                    if (ValidationUtil.IsYes(hfASIExpanded.Value) && !asitPermitForm.IsEmptySearchCriteria())
                    {
                        _capSearchModel.appSpecTableGroups = asitPermitForm.GetAppSpecTable();
                    }

                    if (!string.IsNullOrEmpty(ddlCapStatus.SelectedValue))
                    {
                        _capSearchModel.capStatus = ddlCapStatus.SelectedValue;
                    }
                }

                return _capSearchModel;
            }
        }

        /// <summary>
        /// Gets the cap general search model
        /// </summary>
        private CapModel4WS CapGCSearchModel
        {
            get
            {
                if (_capGCSearchModel == null)
                {
                    _capGCSearchModel = generalSearchForm.GetCapModel4WS();

                    //if General search by ASI search expanded, and criteria isn't empty.
                    if (ACAConstant.COMMON_Y.Equals(hfASIExpanded.Value) && !asiGSForm.IsEmptySearchCriteria())
                    {
                        _capGCSearchModel.appSpecificInfoGroups = asiGSForm.GetAppSpecInfo();
                    }

                    //if General search by ASIT search expanded, and criteria isn't empty.
                    if (ValidationUtil.IsYes(hfASIExpanded.Value) && !asitGSForm.IsEmptySearchCriteria())
                    {
                        _capGCSearchModel.appSpecTableGroups = asitGSForm.GetAppSpecTable();
                    }
                }

                return _capGCSearchModel;
            }
        }

        /// <summary>
        /// Gets the cap search type
        /// </summary>
        private PermitSearchType CapSearchType
        {
            get
            {
                string selectedValue = ddlSearchType.SelectedValue;

                if (selectedValue.IndexOf("||") != -1)
                {
                    selectedValue = selectedValue.Substring(0, selectedValue.IndexOf("||")); // remove bizDomainValue
                }

                PermitSearchType searchType = (PermitSearchType)int.Parse(selectedValue.Replace("-", string.Empty));

                return searchType;
            }
        }

        /// <summary>
        /// Gets Quick Query List
        /// </summary>
        private XDataFilterModel[] DataFilterList
        {
            get
            {
                XDataFilterModel[] dataFilterList = ViewState["QuickQueryList"] as XDataFilterModel[];

                if (dataFilterList == null)
                {
                    IDataFilterBll dataFilterBll = ObjectFactory.GetObject<IDataFilterBll>();

                    dataFilterList = dataFilterBll.GetXDataFilterByViewId(ConfigManager.AgencyCode, int.Parse(GviewID.PermitList), ModuleName, ACAConstant.QUICK_QUERY, AppSession.User.UserSeqNum);

                    ViewState["QuickQueryList"] = dataFilterList;
                }

                return dataFilterList;
            }
        }

        /// <summary>
        /// Gets the default data filter id
        /// </summary>
        private long DefaultDataFilterId
        {
            get
            {
                long dataFilterId = 0;
                string datafilterName = Request.QueryString["QuickQuery"];

                if (!string.IsNullOrEmpty(datafilterName) && DataFilterList != null && DataFilterList.Length > 0)
                {
                    // Quick Query Name may contains some special character like blank
                    datafilterName = System.Web.HttpUtility.UrlDecode(datafilterName);

                    foreach (XDataFilterModel dataFilter in DataFilterList)
                    {
                        if (datafilterName.Equals(dataFilter.datafilterName, StringComparison.OrdinalIgnoreCase))
                        {
                            dataFilterId = dataFilter.datafilterId;
                            break;
                        }
                    }
                }

                return dataFilterId;
            }
        }

        /// <summary>
        /// Gets a value indicating whether to show my permit list.
        /// </summary>
        private bool ShowMyPermitList
        {
            get
            {
                if (!AppSession.IsAdmin)
                {
                    //if the url don't contains the SHOW_MY_PERMIT_LIST, it will only show the permit list
                    return !ValidationUtil.IsNo(Request.QueryString[UrlConstant.SHOW_MY_PERMIT_LIST]) && !AppSession.User.IsAnonymous;
                }

                return true;
            }
        }

        /// <summary>
        /// Gets the search type.
        /// </summary>
        private PermitSearchType SearchType
        {
            get
            {
                string strSearchType = Request.QueryString[UrlConstant.CAPHOME_SEARCH_TYPE];

                return EnumUtil<PermitSearchType>.Parse(strSearchType, PermitSearchType.General);
            }
        }

        #endregion Properties

        #region Public/Protected Methods

        /// <summary>
        /// Get instruction by label key
        /// </summary>
        /// <param name="labelKey">label key string.</param>
        /// <param name="moduleName">module name string.</param>
        /// <returns>instruction value</returns>
        [System.Web.Services.WebMethod(Description = "Get value by label key", EnableSession = true)]
        public static string GetInstructionByKey(string labelKey, string moduleName)
        {
            return LabelUtil.GetTextByKey(labelKey, moduleName);
        }

        /// <summary>
        /// ContactList row command event method.
        /// </summary>
        /// <param name="sender">An sender object that contains the event sender.</param>
        /// <param name="arg">A System.CommonEventArgs object containing the event data.</param>
        protected void ContactList_ContactSelected(object sender, CommonEventArgs arg)
        {
            if (arg == null || arg.ArgObject == null || !(arg.ArgObject is Array))
            {
                return;
            }

            //1. Get query condition for capContactModel.
            CapContactModel4WS contact = null;
            object[] args = (object[])arg.ArgObject;

            if (args.Length == 2)
            {
                contact = (CapContactModel4WS)args[1];
            }

            if (contact != null)
            {
                dgvPermitList.GridViewDataSource = null;
                dgvPermitList.CloseMap();
                QueryCapByContact(contact, 0, null);
            }
        }

        /// <summary>
        /// ContactList page index event method.
        /// </summary>
        /// <param name="sender">An sender object that contains the event sender.</param>
        /// <param name="arg">A System.CommonEventArgs object containing the event data.</param>
        protected void ContactList_PageIndexChanging(object sender, GridViewPageEventArgs arg)
        {
            if (arg != null && QueryInfo != null)
            {
                QueryInfo.SearchResultPageIndex = arg.NewPageIndex;
                PaginationModel pageInfo = PaginationUtil.GetPageInfoByID(PermitSearchType.ByContact.ToString());

                if (arg.NewPageIndex > pageInfo.EndPage)
                {
                    CreateContactDataSource(arg.NewPageIndex, pageInfo.SortExpression);
                }
            }
        }

        /// <summary>
        /// On Initial event method.
        /// </summary>
        /// <param name="e">A System.EventArgs object containing the event data.</param>
        protected override void OnInit(EventArgs e)
        {
            GridViewBuildHelper.InitializeGridWithTemplate(dgvLicense, ModuleName, BizDomainConstant.STD_CAT_LICENSE_TYPE);
            GridViewBuildHelper.SetSimpleViewElements(dgvForAddress, ModuleName, AppSession.IsAdmin);
            GridViewBuildHelper.SetSimpleViewElements(dgvTradeName, ModuleName, AppSession.IsAdmin);

            // When the User is Anonymous, the permit number and license number field is requried.
            if (!AppSession.IsAdmin && AppSession.User.IsAnonymous)
            {
                txtPermitNumber.Validate = "required";
                txtLicenseNumber.Validate = "required";
            }

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
            if (!AppSession.IsAdmin && AppSession.User.IsAnonymous)
            {
                // The Records section should not display for anonymous user.
                divMyPermitListSectionHeader.Visible = false;
            }

            //set the caphome page no cache.
            Response.Cache.SetCacheability(System.Web.HttpCacheability.NoCache);
            Response.Cache.SetNoStore();

            PermitList.GridViewSort += new GridViewSortedEventHandler(PermitList_GridViewSort);
            PermitList.PageIndexChanging += new GridViewPageEventHandler(PermitList_GridViewIndexChanging);
            PermitList.GridViewDownloadAll += PermitList_GridViewDownloadAll;
            contactList.ContactSelected += new CommonEventHandler(ContactList_ContactSelected);
            contactList.PageIndexChanging += new GridViewPageEventHandler(ContactList_PageIndexChanging);
            contactList.GridViewSort += new GridViewSortedEventHandler(GridView_GridViewSort);
            contactList.GridViewDownloadAll += ContactList_GridViewDownloadAll;
            contactList.ParentContainer = ACAConstant.CAP_HOME_PAGE;
            dgvPermitList.GridViewDownloadAll += DgvPermitList_GridViewDownloadAll;

            dgvPermitList.IsForceLoginToApplyPermit = IsForceLoginToApplyPermit(ModuleName);

            // Cap home don't need cap model session, so here need to clear session
            AppSession.SetCapModelToSession(ModuleName, null);
            AppSession.SetSelectedServicesToSession(null); // Here need to clear service session.
            InitCrossModuleUI();

            if (IsPostBack)
            {
                //Set selected items to SimpleCapModelList property for CollectionEdit control.
                if (!string.IsNullOrEmpty(Request.Form["__EVENTTARGET"]))
                {
                    if (Request.Form["__EVENTTARGET"].IndexOf("btnAdd") > -1 && hfGridId.Value.IndexOf(PERMIT_GRIDVIEW) > -1)
                    {
                        addForMyPermits.SimpleCapModelList = PermitList.GetSelectedCAPs();
                        addForMyPermits.IsContainPartialCap = PermitList.IsContainPartialCap;
                    }
                    else if (Request.Form["__EVENTTARGET"].IndexOf("btnAdd") > -1 && hfGridId.Value.IndexOf(SEARCHRESULT_GRIDVIEW) > -1)
                    {
                        addForMyPermits.SimpleCapModelList = dgvPermitList.GetSelectedCAPs();
                        addForMyPermits.IsContainPartialCap = dgvPermitList.IsContainPartialCap;
                    }

                    if (Request.Form["__EVENTTARGET"].IndexOf("btnAppendToCart") > -1 && Request.Form["__EVENTARGUMENT"].IndexOf(PERMIT_GRIDVIEW) > -1)
                    {
                        if (AddToCart(false))
                        {
                            return;
                        }
                    }
                    else if (Request.Form["__EVENTTARGET"].IndexOf("btnAppendToCart") > -1 && Request.Form["__EVENTARGUMENT"].IndexOf(SEARCHRESULT_GRIDVIEW) > -1)
                    {
                        if (AddToCart(true))
                        {
                            return;
                        }
                    }

                    if (Request.Form["__EVENTTARGET"].IndexOf("btnCloneRecord") > -1 && Request.Form["__EVENTARGUMENT"].IndexOf(PERMIT_GRIDVIEW) > -1)
                    {
                        if (!CloneRecord(false))
                        {
                            return;
                        }
                    }
                    else if (Request.Form["__EVENTTARGET"].IndexOf("btnCloneRecord") > -1 && Request.Form["__EVENTARGUMENT"].IndexOf(SEARCHRESULT_GRIDVIEW) > -1)
                    {
                        if (!CloneRecord(true))
                        {
                            return;
                        }
                    }

                    if (Request.Form["__EVENTTARGET"].IndexOf("4btnExport") > -1)
                    {
                        if (GridViewDataSourceForAddress != null)
                        {
                            dgvForAddress.DataSource = GridViewDataSourceForAddress;
                        }

                        if (GridViewDataSourceForLicense != null)
                        {
                            dgvLicense.DataSource = GridViewDataSourceForLicense;
                        }

                        if (GridViewDataSourceForTradeName != null)
                        {
                            dgvTradeName.DataSource = GridViewDataSourceForTradeName;
                        }
                    }
                }

                //hide this button when in admin mode
                btnNewSearch.Visible = !AppSession.IsAdmin;

                //Reset Data when postback
                ResetData();
                return;
            }

            if (!IsPostBack)
            {
                //Initial Search Additional Criterial expand image attributes on General Search and Search by Permit Information page.
                imgGSLoadASI.Alt = GetTextByKey("img_alt_expand_icon");
                imgGSLoadASI.Src = ImageUtil.GetImageURL("caret_collapsed.gif");
                imgPermitLoadASI.Alt = GetTextByKey("img_alt_expand_icon");
                imgPermitLoadASI.Src = ImageUtil.GetImageURL("caret_collapsed.gif");
            }

            InitUI();

            string altId = Request.QueryString["altId"];
            string successMsgKey = Request.QueryString["successMsgKey"];
            string successMsg = string.Format(GetTextByKey(successMsgKey), altId, GetTextByKey("per_permitList_label_history"));
            MessageUtil.ShowMessage(Page, MessageType.Success, successMsg);

            LoadPermitQueryInfo();

            if (AppSession.IsAdmin)
            {
                IRefAddressBll refAddressBll = ObjectFactory.GetObject(typeof(IRefAddressBll)) as IRefAddressBll;
                dvAddressResult.Visible = true;
                dgvForAddress.Visible = true;
                dgvForAddress.DataSource = refAddressBll.ConstructAddressDataTable();
                dgvForAddress.DataBind();

                ILicenseBLL licenseBll = ObjectFactory.GetObject(typeof(ILicenseBLL)) as ILicenseBLL;
                dvLicenseResult.Visible = true;
                dgvLicense.Visible = true;
                dgvLicense.DataSource = licenseBll.ConstructLicenseDataTable();
                dgvLicense.DataBind();

                ILicenseProfessionalBll lpBll = ObjectFactory.GetObject(typeof(ILicenseProfessionalBll)) as ILicenseProfessionalBll;
                dvTradeNameResult.Visible = true;
                dgvTradeName.Visible = true;
                dgvTradeName.DataSource = lpBll.ConstructTradeNameDataTable();
                dgvTradeName.DataBind();

                contactList.DataSource = null;
                contactList.BindContactList();
                divContactResult.Visible = true;

                PermitList.BindCapList(null);
            }

            if (!AppSession.IsAdmin)
            {
                if (string.IsNullOrEmpty(lblSearchInstruction.Text))
                {
                    divInstruction.Visible = false;
                }
            }
        }

        /// <summary>
        /// permit list grid view index change event
        /// </summary>
        /// <param name="sender">An object that contains the event sender.</param>
        /// <param name="e">A System.EventArgs object containing the event data.</param>
        protected void PermitList_GridViewIndexChanging(object sender, GridViewPageEventArgs e)
        {
            PaginationModel pageInfo = PaginationUtil.GetPageInfoByID(PermitList.ClientID);

            if (e.NewPageIndex > pageInfo.EndPage)
            {
                QueryMyPermit(e.NewPageIndex, pageInfo.SortExpression, 0);
            }
        }

        /// <summary>
        /// PermitList GridViewSort
        /// </summary>
        /// <param name="sender">An object that contains the event sender.</param>
        /// <param name="e">A System.EventArgs object containing the event data.</param>
        protected void PermitList_GridViewSort(object sender, AccelaGridViewSortEventArgs e)
        {
            //when user trigger this Gridview pageIndexChanged event, permitList.pageIndex will be set to zero
            int currentPageIndex = 0;
            PaginationModel pageInfo = PaginationUtil.GetPageInfoByID(PermitList.ClientID);
            pageInfo.SortExpression = e.GridViewSortExpression;

            //when sorting gridview, we use GridViewDataSource as the data, so ,needn't pass the first parameters
            PermitList.BindCapList(null, currentPageIndex, e.GridViewSortExpression);
        }

        /// <summary>
        /// DataFilter DropDown event.
        /// </summary>
        /// <param name="sender">An object that contains the event sender.</param>
        /// <param name="e">A System.EventArgs object containing the event data.</param>
        protected void DataFilterDropDown_IndexChanged(object sender, EventArgs e)
        {
            string selectedValue = "0";

            if (!AppSession.IsAdmin)
            {
                PermitList.ClearSelectedItems();

                if (!string.IsNullOrEmpty(ddlDataFilter.SelectedValue))
                {
                    selectedValue = ddlDataFilter.SelectedValue;
                }

                QueryMyPermit(0, null, long.Parse(selectedValue), true);
            }
        }

        /// <summary>
        /// GSLoadASI button OnClick
        /// </summary>
        /// <param name="sender">An object that contains the event sender.</param>
        /// <param name="e">A System.EventArgs object containing the event data.</param>
        protected void GSLoadASIButton_OnClick(object sender, EventArgs e)
        {
            //1.Bind ASI search form.
            CapTypeModel capTypeModel = generalSearchForm.GetCapType();
            asiGSForm.BindPlumbingInfo(capTypeModel);
            asitGSForm.BindUIControls(capTypeModel);

            if (asiGSForm.TempASIGroupInfo == null && asitGSForm.TempASITGroupInfo == null)
            {
                divGSNoASIResult.Visible = true;
                divGSASITResult.Visible = false;
            }
            else
            {
                divGSNoASIResult.Visible = false;
                divGSASITResult.Visible = true;
            }

            //2.Disable load ASI search form link button.
            EnableGSLoadASILink(false);

            //3.auto fit search map position when ASI expand/collapse.
            AccessibilityUtil.FocusElement("btnGSExpandCollapse");
        }

        /// <summary>
        /// Search Button
        /// </summary>
        /// <param name="sender">An object that contains the event sender.</param>
        /// <param name="e">A System.EventArgs object containing the event data.</param>
        protected void NewSearchButton_Click(object sender, EventArgs e)
        {
            //btnNewSearch.Focus();
            ScriptManager.RegisterClientScriptBlock(Page, GetType(), "FocusSearchButton", "FocusSearchButton();", true);
            dgvPermitList.ClearSelectedItems();

            lblResult.Text = string.Empty;
            InitDisplay();
            InitDataSource();

            if (QueryInfo != null)
            {
                QueryInfo.SearchResultPageIndex = 0;
            }

            switch (CapSearchType)
            {
                case PermitSearchType.General:

                    ExpandCollapse();

                    //Check input condition is validate.
                    if (!generalSearchForm.CheckInputConditionForGeneral())
                    {
                        return;
                    }

                    dgvPermitList.CloseMap();
                    CreateCapDataSourceByGC(0, null);

                    break;
                case PermitSearchType.ByAddress:

                    //Check input condition is validate.
                    if (!searchByAddress.CheckInputConditionForAddress())
                    {
                        ShowSearchCriteriaRequiredMessage(GetTextByKey("per_permitList_SearchCriteria_Required"));
                        return;
                    }

                    dgvForAddress.PageIndex = 0;
                    CreateAddressDataSource(0, null);
                    break;

                case PermitSearchType.ByLicense:

                    //Check input condition is validate.
                    if (!CheckInputConditionForLicense())
                    {
                        return;
                    }

                    dgvLicense.PageIndex = 0;
                    CreateLicenseDataSource(0, null);
                    break;

                case PermitSearchType.ByPermit:

                    ExpandCollapse();

                    //Check input condition is validate.
                    if (!CheckInputConditionForPermit())
                    {
                        return;
                    }

                    dgvPermitList.CloseMap();
                    CreateCapDataSource(0, null);
                    break;
                case PermitSearchType.ByTradeName:

                    //Check input condition is validate.
                    if (!CheckInputConditionForTradeName())
                    {
                        return;
                    }

                    dgvTradeName.PageIndex = 0;
                    SearchTradeName(CreateTradeNameDataSource(0, null));
                    break;
                case PermitSearchType.ByContact:

                    //1. check input search critiera-at least input one critiera
                    if (!contactSearchForm.CheckCondition())
                    {
                        ShowSearchCriteriaRequiredMessage(GetTextByKey("per_permitList_SearchCriteria_Required"));
                        return;
                    }

                    //get the contact list by search critiera
                    CreateContactDataSource(0, null);
                    break;
            }

            if (!AppSession.User.IsAnonymous)
            {
                ScriptManager.RegisterClientScriptBlock(Page, GetType(), "HiddenAddForm", "HiddenAddForm();", true);
            }

            ScriptManager.RegisterClientScriptBlock(Page, GetType(), "GotoPageResult", "scrollIntoView('PageResult');", true);
        }

        /// <summary>
        /// Raise the ResetSearch Button Click event
        /// </summary>
        /// <param name="sender">An object that contains the event sender.</param>
        /// <param name="e">A System.EventArgs object containing the event data.</param>
        protected void ResetSearchButton_Click(object sender, EventArgs e)
        {
            lblResult.Text = string.Empty;
           
            if (!AppSession.User.IsAnonymous)
            {
                ScriptManager.RegisterClientScriptBlock(Page, GetType(), "HiddenAddForm", "HiddenAddForm();", true);
            }

            ChangeSearchType();
            ScriptManager.RegisterClientScriptBlock(Page, GetType(), "GotoSearchForm_Start", "scrollIntoView('SearchForm_Start');", true);
        }

        /// <summary>
        /// PermitCollapseASI button OnClick
        /// </summary>
        /// <param name="sender">An object that contains the event sender.</param>
        /// <param name="e">A System.EventArgs object containing the event data.</param>
        protected void PermitLoadASIButton_OnClick(object sender, EventArgs e)
        {
            //1.Bind ASI search form.
            CapTypeModel capTypeModel = GetSelectedCapTypeModel(ddlPermitSubAgency.SelectedValue, ddlPermitType);
            asiPermitForm.BindPlumbingInfo(capTypeModel);
            asitPermitForm.BindUIControls(capTypeModel);

            if (asiPermitForm.TempASIGroupInfo == null && asitPermitForm.TempASITGroupInfo == null)
            {
                divPermitNoASIResult.Visible = true;
                divPermitASITResult.Visible = false;
            }
            else
            {
                divPermitNoASIResult.Visible = false;
                divPermitASITResult.Visible = true;
            }

            //2.Disable load ASI search form link button.
            EnablePermitLoadASILink(false);

            //3.auto fit search map position when ASI expand/collapse.
            updatePanel.FocusElement("btnPermitExpandCollapse");
        }

        /// <summary>
        /// PermitSubAgency dropdown IndexChanged
        /// </summary>
        /// <param name="sender">An object that contains the event sender.</param>
        /// <param name="e">A System.EventArgs object containing the event data.</param>
        protected void PermitSubAgencyDropDown_IndexChanged(object sender, EventArgs e)
        {
            //if normal agency, it needn't trigger event.
            if (!StandardChoiceUtil.IsSuperAgency() && !AppSession.IsAdmin)
            {
                return;
            }

            BindPermitType(ddlPermitType, ddlPermitSubAgency.SelectedValue);
            InitASIControl();
            InitASITControl();

            //not-selected any agency.
            if (string.IsNullOrEmpty(ddlPermitSubAgency.SelectedValue))
            {
                ddlCapStatus.Enabled = false;
                InitPermitExpendASILink(false);
            }
            else
            {
                ddlPermitType.Enabled = true;
                EnablePermitLoadASILink(true);
                lnkPermitLoadASI.Enabled = true;
                lnkPermitLoadASI.ToolTip = GetTitleByKey("img_alt_expand_icon", "per_permitList_label_GS_expandASI", false);
            }
        }

        /// <summary>
        /// Raise PermitType DropDown IndexChanged
        /// </summary>
        /// <param name="sender">An object that contains the event sender.</param>
        /// <param name="e">A System.EventArgs object containing the event data.</param>
        protected void PermitTypeDropDown_IndexChanged(object sender, EventArgs e)
        {
            InitASIControl();
            InitASITControl();

            if (string.IsNullOrEmpty(ddlPermitType.SelectedValue))
            {
                ddlCapStatus.Enabled = false;
                DropDownListBindUtil.BindCapStatus(ddlCapStatus, ModuleName, null);
            }
            else
            {
                ddlCapStatus.Enabled = true;
                ICapTypeBll capTypeBll = (ICapTypeBll)ObjectFactory.GetObject(typeof(ICapTypeBll));
                AppStatusGroupModel4WS[] appStatusGroupModels = capTypeBll.GetAppStatusByCapType(GetSelectedCapTypeModel(ddlPermitSubAgency.SelectedValue, ddlPermitType));
                DropDownListBindUtil.BindCapStatus(ddlCapStatus, ModuleName, appStatusGroupModels);
                Page.FocusElement(ddlPermitType.ClientID);
            }

            CapTypeModel capType = GetSelectedCapTypeModel(ddlPermitSubAgency.SelectedValue, ddlPermitType);

            if (ACAConstant.COMMON_Y.Equals(hfASIExpanded.Value))
            {
                asiPermitForm.BindPlumbingInfo(capType);
                asitPermitForm.BindUIControls(capType);

                if (asiPermitForm.TempASIGroupInfo == null && asitPermitForm.TempASITGroupInfo == null)
                {
                    divPermitNoASIResult.Visible = true;
                    divPermitASITResult.Visible = false;
                }
                else
                {
                    divPermitNoASIResult.Visible = false;
                    divPermitASITResult.Visible = true;
                }
            }
            else
            {
                EnablePermitLoadASILink(true);
            }
        }

        /// <summary>
        /// Dropdown list command
        /// </summary>
        /// <param name="sender">An object that contains the event sender.</param>
        /// <param name="e">A System.EventArgs object containing the event data.</param>
        protected void SearchTypeDropDown_IndexChanged(object sender, EventArgs e)
        {
            string selectedValue = ddlSearchType.SelectedValue;

            if (AppSession.IsAdmin)
            {
                ////Re-bind search type drop-down to get the latest values.
                ddlSearchType.Items.Clear();
                BindSearchList();
                ddlSearchType.SetValue(selectedValue);
            }

            QueryInfo = null;
            ChangeSearchType();

            if (AppSession.IsAdmin)
            {
                ddlSearchType.StdCategory = BizDomainConstant.STD_CAT_CAP_SEARCH_TYPE;
            }
            else
            {
                if (string.IsNullOrEmpty(lblSearchInstruction.Text))
                {
                    divInstruction.Visible = false;
                }
                else
                {
                    divInstruction.Visible = true;
                }
            }

            if (!AppSession.User.IsAnonymous)
            {
                ScriptManager.RegisterClientScriptBlock(Page, GetType(), "HiddenAddForm", "HiddenAddForm();", true);
            }
        }

        /// <summary>
        /// Get and display permit list, search by Address.
        /// </summary>
        /// <param name="sender">An object that contains the event sender.</param>
        /// <param name="e">An GridViewCommandEventArgs object that contains the event data.</param>
        protected void ForAddressGridView_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "showCAPs")
            {
                dgvPermitList.ClearSelectedItems();
                int index = Convert.ToInt32(e.CommandArgument);
                GridViewRow row = dgvForAddress.Rows[index];
                AccelaLabel lblAddressRowIndex = (AccelaLabel)row.FindControl("lblAddressRowIndex");
                AccelaLabel lblAddress = (AccelaLabel)row.FindControl("lblAddress");
                AccelaLabel lblAgency = (AccelaLabel)row.FindControl("lblAgency");
                LinkButton lnkAddressButton = (LinkButton)row.FindControl("lbAddress");
                dgvPermitList.GridViewDataSource = null;
                dgvPermitList.CloseMap();
                QueryCapByAddress(lblAddressRowIndex.Text, lblAgency.Text, 0, null);

                divSeparateLine.Visible = true;
                lblResult.Text = lblAddress.Text;
                QueryInfo.LabelForPermit = lblAddress.Text;
                QueryInfo.HasSearchPermit = true;
                Page.FocusElement(lnkAddressButton.ClientID);
            }
            else
            {
                dgvForAddress.DataSource = GridViewDataSourceForAddress;
                dgvForAddress.DataBind();
            }

            if (!AppSession.User.IsAnonymous)
            {
                ScriptManager.RegisterClientScriptBlock(Page, GetType(), "HiddenAddForm", "HiddenAddForm();", true);
            }
        }

        /// <summary>
        /// grid view ForAddress download event method.
        /// </summary>
        /// <param name="sender">An object that contains the event sender.</param>
        /// <param name="e">A GridViewDownloadEventArgs object containing the event data.</param>
        protected void ForAddressGridView_GridViewDownloadAll(object sender, GridViewDownloadEventArgs e)
        {
            GridViewBuildHelper.DownloadAll(sender, e, GetAddressByQueryFormat);
        }

        /// <summary>
        /// grid view TradeName download event method.
        /// </summary>
        /// <param name="sender">An object that contains the event sender.</param>
        /// <param name="e">A GridViewDownloadEventArgs object containing the event data.</param>
        protected void TradeNameGridView_GridViewDownloadAll(object sender, GridViewDownloadEventArgs e)
        {
            GridViewBuildHelper.DownloadAll(sender, e, GetTradeNameByQueryFormat);
        }

        /// <summary>
        /// Get and display permit list, search by license.
        /// </summary>
        /// <param name="sender">An object that contains the event sender.</param>
        /// <param name="e">A System.EventArgs object containing the event data.</param>
        protected void LicenseGridView_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "showCAPs")
            {
                e.FocusRowCellByName("lbLicense");
                dgvPermitList.ClearSelectedItems();
                int index = Convert.ToInt32(e.CommandArgument);
                GridViewRow row = dgvLicense.Rows[index];
                AccelaLabel lblLicenseID = (AccelaLabel)row.FindControl("lblLicenseID");
                AccelaLabel lblLicenseType = (AccelaLabel)row.FindControl("lblLicenseType");
                AccelaLabel lblLicenseTypeText = (AccelaLabel)row.FindControl("lblLicenseTypeText");
                AccelaLabel lblAgencyCode = (AccelaLabel)row.FindControl("lblAgencyCode");
                dgvPermitList.GridViewDataSource = null;
                dgvPermitList.CloseMap();
                QueryCapByLicenseID(lblLicenseID.Text, lblLicenseType.Text, lblLicenseTypeText.Text, lblAgencyCode.Text, 0, null);
                QueryInfo.HasSearchPermit = true;
                divSeparateLine.Visible = true;
            }
            else
            {
                DataTable dt = GridViewBuildHelper.MergeTemplateAttributes2DataTable(dgvLicense, GridViewDataSourceForLicense);
                dgvLicense.DataSource = dt;
                dgvLicense.DataBind();
            }

            if (!AppSession.User.IsAnonymous)
            {
                ScriptManager.RegisterClientScriptBlock(Page, GetType(), "HiddenAddForm", "HiddenAddForm();", true);
            }
        }

        /// <summary>
        /// Set Disable date format.
        /// </summary>
        /// <param name="sender">An object that contains the event sender.</param>
        /// <param name="e">A System.EventArgs object containing the event data.</param>
        protected void LicenseGridView_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                DataRowView rowView = (DataRowView)e.Row.DataItem;

                AccelaLabel lblPhone = (AccelaLabel)e.Row.FindControl("lblPhone");
                AccelaLabel lblFax = (AccelaLabel)e.Row.FindControl("lblFax");

                lblPhone.Text = ModelUIFormat.FormatPhoneShow(rowView["PhoneIDD"].ToString(), rowView["Phone"].ToString(), rowView["CountryCode"].ToString());
                lblFax.Text = ModelUIFormat.FormatPhoneShow(rowView["FaxIDD"].ToString(), rowView["Fax"].ToString(), rowView["CountryCode"].ToString());
            }
        }

        /// <summary>
        /// License grid view download event method.
        /// </summary>
        /// <param name="sender">An object that contains the event sender.</param>
        /// <param name="e">A GridViewDownloadEventArgs object containing the event data.</param>
        protected void LicenseGridView_GridViewDownloadAll(object sender, GridViewDownloadEventArgs e)
        {
            GridViewBuildHelper.DownloadAll(sender, e, GetLicenseeByQueryFormat, LicenseeExportFormat);
        }

        /// <summary>
        /// response permit grid view sorted event to record the latest sort expression.
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">A System.EventArgs object containing the event data.</param>
        protected void PLGridView_GridViewSort(object sender, AccelaGridViewSortEventArgs e)
        {
            if (QueryInfo == null)
            {
                return;
            }

            QueryInfo.PermitResultSortExpression = e.GridViewSortExpression;
            PaginationModel pageInfo = PaginationUtil.GetPageInfoByID(QueryInfo.SearchType + dgvPermitList.ClientID);
            pageInfo.SortExpression = e.GridViewSortExpression;

            //when user trigger this Gridview pageIndexChanged event, permitList.pageIndex will be set to zero
            int currentPageIndex = 0;

            //when sorting gridview, we should restore the GridViewDataSource
            switch (QueryInfo.SearchType)
            {
                case PermitSearchType.ByAddress:
                case PermitSearchType.ByLicense:
                case PermitSearchType.ByPermit:
                    dgvPermitList.BindCapList(null, currentPageIndex, QueryInfo.PermitResultSortExpression);
                    QueryInfo.PermitResultPageIndex = 0;
                    break;
                case PermitSearchType.General:
                    dgvPermitList.BindCapList(QueryInfo.ComplexPermitList, currentPageIndex, QueryInfo.PermitResultSortExpression);
                    QueryInfo.PermitResultPageIndex = 0;
                    break;
                case PermitSearchType.ByTradeName:
                    //set first parms
                    dgvPermitList.BindDataToPermitList(QueryInfo.SearchResult, currentPageIndex, QueryInfo.PermitResultSortExpression);
                    QueryInfo.SearchResult = dgvPermitList.GridViewDataSource;
                    QueryInfo.PermitResultPageIndex = 0;
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// response permit grid view page index changing event to record the latest page index.
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">A System.EventArgs object containing the event data.</param>
        protected void PLGridView_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            if (QueryInfo != null)
            {
                QueryInfo.PermitResultPageIndex = e.NewPageIndex;
                PaginationModel pageInfo = PaginationUtil.GetPageInfoByID(QueryInfo.SearchType + dgvPermitList.ClientID);

                if (e.NewPageIndex > pageInfo.EndPage)
                {
                    switch (QueryInfo.SearchType)
                    {
                        case PermitSearchType.ByAddress:
                            QueryCapByAddress((string)pageInfo.SearchCriterias[0], (string)pageInfo.SearchCriterias[1], e.NewPageIndex, pageInfo.SortExpression);
                            break;
                        case PermitSearchType.ByPermit:
                            CreateCapDataSource(e.NewPageIndex, pageInfo.SortExpression);
                            break;
                        case PermitSearchType.General:
                            CreateCapDataSourceByGC(e.NewPageIndex, pageInfo.SortExpression);
                            break;
                        case PermitSearchType.ByLicense:
                            QueryCapByLicenseID((string)pageInfo.SearchCriterias[0], (string)pageInfo.SearchCriterias[1], (string)pageInfo.SearchCriterias[2], (string)pageInfo.SearchCriterias[3], e.NewPageIndex, pageInfo.SortExpression);
                            break;

                        case PermitSearchType.ByContact:
                            QueryCapByContact((CapContactModel4WS)pageInfo.SearchCriterias[0], e.NewPageIndex, pageInfo.SortExpression);
                            break;
                    }
                }
            }
        }

        /// <summary>
        /// Get and display TradeName list, search by Trade Name.
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">A System.EventArgs object containing the event data.</param>
        protected void TradeNameGridView_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            dgvTradeName.DataSource = GridViewDataSourceForTradeName;
            dgvTradeName.DataBind();
            if (!AppSession.User.IsAnonymous)
            {
                ScriptManager.RegisterClientScriptBlock(Page, GetType(), "HiddenAddForm", "HiddenAddForm();", true);
            }
        }

        /// <summary>
        /// grid view TradeName RowDataBound
        /// </summary>
        /// <param name="sender">An object that contains the event sender.</param>
        /// <param name="e">A System.EventArgs object containing the event data.</param>
        protected void TradeNameGridView_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                DataRowView rowView = (DataRowView)e.Row.DataItem;

                HyperLink hlTradeNameNum = (HyperLink)e.Row.FindControl("hlTradeNameNum");

                string lpNum = rowView["LicenseNumber"].ToString();
                string lpType = rowView["SearchLicenseType"].ToString();
                string lpSeqNbr = rowView["LicenseSeqNbr"].ToString();
                string agencyCode = rowView["AgencyCode"].ToString();

                hlTradeNameNum.NavigateUrl = string.Format("../Cap/TradeNameDetail.aspx?Module={0}&lpNum={1}&lpType={2}&LpSeqNbr={3}&{4}={5}", ModuleName, lpNum, lpType, lpSeqNbr, UrlConstant.AgencyCode, agencyCode);
            }
        }

        /// <summary>
        /// response address or license grid view sorted event to record the latest sort expression.
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">A System.EventArgs object containing the event data.</param>
        protected void GridView_GridViewSort(object sender, AccelaGridViewSortEventArgs e)
        {
            if (QueryInfo != null)
            {
                QueryInfo.SearchResultSortExpression = e.GridViewSortExpression;
                PaginationModel pageInfo = PaginationUtil.GetPageInfoByID(QueryInfo.SearchType.ToString());
                pageInfo.SortExpression = e.GridViewSortExpression;
            }

            SetGridViewSort(QueryInfo);
        }

        /// <summary>
        /// response address or license grid view page index changing event to record the latest page index.
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">A System.EventArgs object containing the event data.</param>
        protected void GridView_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            if (QueryInfo != null)
            {
                QueryInfo.SearchResultPageIndex = e.NewPageIndex;
                PaginationModel pageInfo = PaginationUtil.GetPageInfoByID(QueryInfo.SearchType.ToString());

                if (e.NewPageIndex > pageInfo.EndPage)
                {
                    switch (QueryInfo.SearchType)
                    {
                        case PermitSearchType.ByAddress:
                            CreateAddressDataSource(e.NewPageIndex, pageInfo.SortExpression);
                            break;

                        case PermitSearchType.ByLicense:
                            CreateLicenseDataSource(e.NewPageIndex, pageInfo.SortExpression);
                            break;

                        case PermitSearchType.ByTradeName:
                            SearchTradeName(CreateTradeNameDataSource(e.NewPageIndex, pageInfo.SortExpression));
                            break;
                    }
                }
            }
        }

        /// <summary>
        /// sub agency changed.
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">A System.EventArgs object containing the event data.</param>
        protected void GeneralSearch_SubAgencyChanged(object sender, EventArgs e)
        {
            InitGSExpendASILink();

            //in ACA admin, need to bind CAP type by agency code, and hide button area.
            if (AppSession.IsAdmin)
            {
                InitDisplay();
                return;
            }

            InitASIControl();
            InitASITControl();

            //not-selected any agency.
            if (!string.IsNullOrEmpty(generalSearchForm.SubAgencyDropDownList.SelectedValue))
            {
                btnGSLoadASI.Enabled = true;
                btnGSLoadASI.ToolTip = GetTitleByKey("img_alt_expand_icon", "per_permitList_label_permit_expandASI", false);
            }
        }

        /// <summary>
        /// Permit type changed.
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">A System.EventArgs object containing the event data.</param>
        protected void GeneralSearchForm_PermitTypeChanged(object sender, EventArgs e)
        {
            InitASIControl();
            InitASITControl();

            CapTypeModel capType = generalSearchForm.GetCapType();

            //ASI has expanded.
            if (ACAConstant.COMMON_Y.Equals(hfASIExpanded.Value))
            {
                asiGSForm.BindPlumbingInfo(capType);
                asitGSForm.BindUIControls(capType);

                if (asiGSForm.TempASIGroupInfo == null && asitGSForm.TempASITGroupInfo == null)
                {
                    divGSNoASIResult.Visible = true;
                    divGSASITResult.Visible = false;
                }
                else
                {
                    divGSNoASIResult.Visible = false;
                    divGSASITResult.Visible = true;
                }
            }
            else
            {
                EnableGSLoadASILink(true);
            }
        }

        #endregion Public/Protected Methods

        #region Private Methods

        /// <summary>
        /// Handles the GridViewDownloadAll event of the PermitList control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="Accela.Web.Controls.GridViewDownloadEventArgs"/> instance containing the event data.</param>
        private void PermitList_GridViewDownloadAll(object sender, GridViewDownloadEventArgs e)
        {
            if (ddlDataFilter.Visible && !string.IsNullOrEmpty(ddlDataFilter.SelectedValue) && long.Parse(ddlDataFilter.SelectedValue) != 0)
            {
                GridViewBuildHelper.DownloadAll(sender, e, GetPermitByQuickQuery);
            }
            else
            {
                GridViewBuildHelper.DownloadAll(sender, e, GetGeneralSearchByQueryFormat);
            }
        }

        /// <summary>
        /// Handles the GridViewDownloadAll event of the contactList control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="Accela.Web.Controls.GridViewDownloadEventArgs"/> instance containing the event data.</param>
        private void ContactList_GridViewDownloadAll(object sender, GridViewDownloadEventArgs e)
        {
            GridViewBuildHelper.DownloadAll(sender, e, GetContactByQueryFormat, ContactExportFormat);
        }

        /// <summary>
        /// Handles the GridViewDownloadAll event of the permitList control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="Accela.Web.Controls.GridViewDownloadEventArgs"/> instance containing the event data.</param>
        private void DgvPermitList_GridViewDownloadAll(object sender, GridViewDownloadEventArgs e)
        {
            switch (CapSearchType)
            {
                case PermitSearchType.General:
                    GridViewBuildHelper.DownloadAll(sender, e, GetPermitGCByQueryFormat);
                    break;

                case PermitSearchType.ByPermit:
                    GridViewBuildHelper.DownloadAll(sender, e, GetPermitByQueryFormat);
                    break;

                case PermitSearchType.ByAddress:
                    GridViewBuildHelper.DownloadAll(sender, e, GetAddressCapByQueryFormat);
                    break;

                case PermitSearchType.ByLicense:
                    GridViewBuildHelper.DownloadAll(sender, e, GetLicenseCapByQueryFormat);
                    break;
            }
        }

        /// <summary>
        /// Add Cap to shopping cart.
        /// </summary>
        /// <param name="isSearchResult">Is search result.</param>
        /// <returns>Success or failed</returns>
        private bool AddToCart(bool isSearchResult)
        {
            DataTable selectedCaps = new DataTable();

            if (isSearchResult)
            {
                //1.Search permit list.
                selectedCaps = dgvPermitList.GetSelectedCAPItems();
            }
            else
            {
                //2.My Permit List.
                selectedCaps = PermitList.GetSelectedCAPItems();
            }

            return ShoppingCartUtil.AddToCart(Page, selectedCaps, ModuleName);
        }

        /// <summary>
        /// Append string.
        /// </summary>
        /// <param name="stringbuilder">string builder.</param>
        /// <param name="str">string content.</param>
        private void AppendString(StringBuilder stringbuilder, string str)
        {
            if (!string.IsNullOrEmpty(str))
            {
                stringbuilder.Append(ScriptFilter.FilterScript(str) + " ");
            }
        }

        /// <summary>
        /// Bind Permit List for address
        /// </summary>
        /// <param name="capList">SimpleCapModel4WS array.</param>
        /// <param name="pageIndex">the page index.</param>
        /// <param name="sort">the sort string.</param>
        private void BindPermitListForAddress(DataTable capList, int pageIndex, string sort)
        {
            if (!IsAutoSkip2CapDetail(capList))
            {
                dgvPermitList.BindCapList(capList, pageIndex, sort);
                divPermitList.Visible = capList != null && capList.Rows.Count > 0;
                dvSearchList.Visible = true;
                dvPromptForAddress.Visible = true;
                dvResult.Visible = true;
                divSeparateLine.Visible = !QueryInfo.IsBackFromCapDetailByAutoSkip;

                ScrollToCapResult();

                if (capList == null || capList.Rows.Count == 0)
                {
                    InitPermitResult(false, !QueryInfo.IsBackFromCapDetailByAutoSkip);
                }
            }
        }

        /// <summary>
        /// Bind permit list for contact
        /// </summary>
        /// <param name="capList">SimpleCapModel array.</param>
        /// <param name="resultText">Result cap result text</param>
        /// <param name="pageIndex">page index.</param>
        /// <param name="sort">the sort string.</param>
        private void BindPermitListForContact(DataTable capList, string resultText, int pageIndex, string sort)
        {
            if (!IsAutoSkip2CapDetail(capList))
            {
                dgvPermitList.BindCapList(capList, pageIndex, sort);
                divPermitList.Visible = capList != null && capList.Rows.Count > 0;
                dvSearchList.Visible = true;

                dvPromptForContact.Visible = true;
                lblResult.Text = resultText;
                dvResult.Visible = true;
                divSeparateLine.Visible = !QueryInfo.IsBackFromCapDetailByAutoSkip;

                ScrollToCapResult();

                if (capList == null || capList.Rows.Count == 0)
                {
                    InitPermitResult(false, !QueryInfo.IsBackFromCapDetailByAutoSkip);
                }
            }

            updatePanel.Update();
        }

        /// <summary>
        /// Bind Permit List for license
        /// </summary>
        /// <param name="capList">SimpleCapModel4WS array.</param>
        /// <param name="resultText">result text.</param>
        /// <param name="pageIndex">the page index</param>
        /// <param name="sort">the sort string.</param>
        private void BindPermitListForLicense(DataTable capList, string resultText, int pageIndex, string sort)
        {
            if (!IsAutoSkip2CapDetail(capList))
            {
                dgvPermitList.BindCapList(capList, pageIndex, sort);
                divPermitList.Visible = capList != null && capList.Rows.Count > 0;
                dvSearchList.Visible = true;

                dvPromptForLicense.Visible = true;
                lblResult.Text = resultText;
                dvResult.Visible = true;
                divSeparateLine.Visible = !QueryInfo.IsBackFromCapDetailByAutoSkip;

                ScrollToCapResult();

                if (capList == null || capList.Rows.Count == 0)
                {
                    InitPermitResult(false, !QueryInfo.IsBackFromCapDetailByAutoSkip);
                }
            }
        }

        /// <summary>
        /// Bind cap type and display them on the screen
        /// </summary>
        /// <param name="ddlPermit">AccelaDropDownList for permit.</param>
        /// <param name="agencyCode">the agency code.</param>
        private void BindPermitType(AccelaDropDownList ddlPermit, string agencyCode)
        {
            if (ddlPermit == null)
            {
                return;
            }

            CapTypeModel[] permitTypelist = GetCapTypeList(agencyCode);

            IList<ListItem> lstPermitType = new List<ListItem>();

            if (permitTypelist != null)
            {
                SortedList sortedList = new SortedList();
                foreach (CapTypeModel typemodel in permitTypelist)
                {
                    string key = CAPHelper.GetAliasOrCapTypeLabel(typemodel);

                    if (sortedList.ContainsKey(key))
                    {
                        key = key + CAPHelper.GetCapTypeValue(typemodel);
                    }

                    if (!sortedList.ContainsKey(key))
                    {
                        sortedList[key] = typemodel;
                    }
                }

                foreach (DictionaryEntry de in sortedList)
                {
                    CapTypeModel typemodel = (CapTypeModel)de.Value;
                    ListItem item = new ListItem();

                    item.Text = CAPHelper.GetAliasOrCapTypeLabel(typemodel);
                    item.Value = CAPHelper.GetCapTypeValue(typemodel);
                    lstPermitType.Add(item);
                }
            }

            ddlPermit.Items.Clear();
            DropDownListBindUtil.BindDDL(lstPermitType, ddlPermit, true, false);
        }

        /// <summary>
        /// Bind the search list.
        /// </summary>
        private void BindSearchList()
        {
            IList<ItemValue> stdItems = DropDownListBindUtil.GetHardcodeItems4DDL(ddlSearchType, BizDomainConstant.STD_CAT_CAP_SEARCH_TYPE, ModuleName);

            if (stdItems != null && stdItems.Count > 0)
            {
                foreach (ItemValue item in stdItems)
                {
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
        /// Populate the LicenseProfessionalModel list to a DataTable.
        /// </summary>
        /// <param name="licenseModelList">The license model list.</param>
        /// <returns>A DataTable of licenseProfessionals.</returns>
        private DataTable BuildLicenseDataTable(LicenseModel[] licenseModelList)
        {
            ILicenseProfessionalBll lpBll = ObjectFactory.GetObject(typeof(ILicenseProfessionalBll)) as ILicenseProfessionalBll;

            // create an empty datatable
            DataTable dtLicenses = lpBll.ConstructTradeNameDataTable();

            if (licenseModelList == null || licenseModelList.Length <= 0)
            {
                return dtLicenses;
            }

            foreach (LicenseModel license in licenseModelList)
            {
                DataRow dr = dtLicenses.NewRow();

                dr["LicenseNumber"] = license.stateLicense;
                dr["LicenseType"] = I18nStringUtil.GetString(license.resLicenseType, license.licenseType);
                dr["EnglishName"] = license.businessName;
                dr["ArabicName"] = license.busName2;
                if (!string.IsNullOrEmpty(license.agencyCode))
                {
                    dr["AgencyCode"] = license.agencyCode;
                }
                else
                {
                    dr["AgencyCode"] = ACAConstant.AgencyCode;

                    if (Logger.IsDebugEnabled)
                    {
                        Logger.Debug("warning:license.agencyCode is null when search by trade name");
                    }
                }

                dr["LicenseExpirationDate"] = license.licenseExpirationDate == null ? DBNull.Value : (object)license.licenseExpirationDate;
                string licenseStatus = I18nStringUtil.GetString(license.resLicenseStatus, license.licenseStatus);

                dr["LicenseStatus"] = licenseStatus;
                dr["LicenseSeqNbr"] = license.licSeqNbr;
                dr["SearchLicenseType"] = license.licenseType;

                dtLicenses.Rows.Add(dr);
            }

            return dtLicenses;
        }

        /// <summary>
        /// Disable selected search type
        /// </summary>
        private void ChangeSearchType()
        {
            IntialMyPermitFlag();
            InitAllValue();

            if (ddlSearchType.Items.Count == 0)
            {
                return;
            }

            string sectionId = null;
            string prefix = null;
            lblSelectedSearchType.InnerText = ddlSearchType.SelectedItem.Text;

            // PermitSearchType searchType = (PermitSearchType)(int.Parse(ddlSearchType.SelectedValue.Replace("-", string.Empty)));
            string selectedValue = ddlSearchType.SelectedValue;

            if (selectedValue.IndexOf("||") != -1)
            {
                selectedValue = selectedValue.Substring(0, selectedValue.IndexOf("||")); // remove bizDomainValue
            }

            PermitSearchType searchType = (PermitSearchType)int.Parse(selectedValue.Replace("-", string.Empty));

            if (GetCrossModuleSearchStatus() && searchType != PermitSearchType.ByTradeName)
            {
                chkCrossModuleSearch.Visible = true;
            }
            else
            {
                chkCrossModuleSearch.Visible = false;
            }

            string instructionKey = "per_permitlist_instruction_generalsearch";

            switch (searchType)
            {
                case PermitSearchType.General:
                    dvGenearlSearch.Visible = true;
                    sectionId = GviewID.GeneralSearch;
                    prefix = generalSearchForm.GetPrefix();
                    InitGSExpendASILink();

                    if (!AppSession.IsAdmin)
                    {
                        generalSearchForm.ApplyRegionalSetting(false);
                        SetCurrentCityAndState();
                    }

                    break;

                case PermitSearchType.ByAddress:
                    dvSearchForAddress.Visible = true;
                    sectionId = GviewID.SearchByAddress;
                    prefix = searchByAddress.GetPrefix();

                    if (!AppSession.IsAdmin)
                    {
                        searchByAddress.ApplyRegionalSetting(false);
                        SetCurrentCityAndState();
                    }

                    instructionKey = "per_permitlist_instruction_searchbyaddress";

                    break;

                case PermitSearchType.ByLicense:
                    DropDownListBindUtil.BindLicenseType(ddlLicenseType);
                    DropDownListBindUtil.BindContactType4License(ddlContactType);
                    dvSearchForLicensed.Visible = true;
                    sectionId = GviewID.SearchByLicense;
                    prefix = ddlLicenseType.ClientID.Replace("ddlLicenseType", string.Empty);
                    ControlBuildHelper.HideStandardFields(sectionId, ModuleName, dvSearchForLicensed.Controls, AppSession.IsAdmin);

                    if (!AppSession.IsAdmin)
                    {
                        divLicense.Visible = ddlLicenseType.Visible || txtLicenseNumber.Visible;
                        divLicenseName.Visible = txtFirstName.Visible || txtLastName.Visible || txtBusiName.Visible;
                        divLicenseName2.Visible = txtBusiName2.Visible;
                        divBusiLicense.Visible = txtBusiLicense.Visible;
                        divContactType.Visible = ddlContactType.Visible;
                        divSSN.Visible = txtSSN.Visible || txtFEIN.Visible;
                    }

                    instructionKey = "per_permitlist_instruction_searchbylicense";

                    break;

                case PermitSearchType.ByPermit:
                    dvSearchForPermit.Visible = true;

                    sectionId = GviewID.SearchByPermit;
                    prefix = txtPermitNumber.ClientID.Replace("txtPermitNumber", string.Empty);
                    ControlBuildHelper.HideStandardFields(sectionId, ModuleName, dvSearchForPermit.Controls, AppSession.IsAdmin);

                    if (!AppSession.IsAdmin)
                    {
                        ddlCapStatus.Enabled = false;
                        DropDownListBindUtil.BindCapStatus(ddlCapStatus, ModuleName, null);
                        divPermit.Visible = ddlPermitSubAgency.Visible || txtPermitNumber.Visible || ddlPermitType.Visible;
                        dvProjectName.Visible = txtProjectName.Visible || ddlCapStatus.Visible;
                        divPermitDate.Visible = txtStartDate.Visible || txtEndDate.Visible;

                        //in ACA super agency daily, Agency/CAP type drop down list can trig server event method, in ACA admin it is needn't trig server event method.
                        ddlPermitSubAgency.AutoPostBack = StandardChoiceUtil.IsSuperAgency();
                        ddlPermitType.AutoPostBack = true;
                    }

                    BindPermitType(ddlPermitType, ddlPermitSubAgency.SelectedValue);

                    InitPermitExpendASILink(true);

                    instructionKey = "per_permitlist_instruction_searchbypermit";

                    break;

                case PermitSearchType.ByTradeName:
                    dvSearchForTradeName.Visible = true;
                    DropDownListBindUtil.BindLicenseType(ddlTradeNameType);

                    ICapBll capBll = ObjectFactory.GetObject<ICapBll>();
                    AppStatusGroupModel4WS[] appStatusGroupModels = capBll.GetAppStatusGroupBySPC(ConfigManager.AgencyCode, string.Empty);
                    DropDownListBindUtil.BindCapStatus(ddlTradeNameRecordStatus, ModuleName, appStatusGroupModels, XPolicyConstant.RECORD_STATUS_SEARCH_FOR_TRADENAME);

                    sectionId = GviewID.SearchByTradeName;

                    prefix = txtEnglishTradeName.ClientID.Replace("txtEnglishTradeName", string.Empty);
                    ControlBuildHelper.HideStandardFields(sectionId, ModuleName, dvSearchForTradeName.Controls, AppSession.IsAdmin);

                    if (!AppSession.IsAdmin)
                    {
                        dvTradeName1.Visible = txtEnglishTradeName.Visible || txtArabicTradeName.Visible || ddlTradeNameRecordStatus.Visible;
                        dvTradeName2.Visible = ddlTradeNameType.Visible;
                    }

                    instructionKey = "per_permitlist_instruction_searchbytradename";

                    break;

                case PermitSearchType.ByContact:
                    dvSearchForContact.Visible = true;

                    sectionId = GviewID.SearchByContact;
                    prefix = contactSearchForm.ContactIDPrefix;
                    ControlBuildHelper.HideStandardFields(sectionId, ModuleName, dvSearchForContact.Controls, AppSession.IsAdmin);

                    if (!AppSession.IsAdmin)
                    {
                        contactSearchForm.ApplyRegionalSetting(false);
                        contactSearchForm.ShowDivSections();
                        contactSearchForm.SetCurrentCityAndState();
                        contactSearchForm.ReSetTemplateFields();
                    }

                    instructionKey = "per_permitlist_instruction_searchbycontact";

                    break;
            }

            ddlSearchType.SectionID = string.Format("{0}" + ACAConstant.SPLIT_CHAR + "{1}" + ACAConstant.SPLIT_CHAR + "{2}", ModuleName, sectionId, prefix);
            lblSearchInstruction.Text = GetTextByKey(instructionKey);
        }

        /// <summary>
        /// Check whether has least one search criteria has been entered when search by License.
        /// </summary>
        /// <returns>true: input at least one condition; false: doesn't input any condition</returns>
        private bool CheckInputConditionForLicense()
        {
            bool result = true;

            if (ddlLicenseType.SelectedIndex == 0 && ddlContactType.SelectedIndex == 0 && string.IsNullOrEmpty(txtLicenseNumber.Text.Trim()) && string.IsNullOrEmpty(txtFirstName.Text.Trim())
                && string.IsNullOrEmpty(txtLastName.Text.Trim())
                && string.IsNullOrEmpty(txtBusiName.Text.Trim()) && string.IsNullOrEmpty(txtBusiName2.Text.Trim()) && string.IsNullOrEmpty(txtBusiLicense.Text.Trim())
                && string.IsNullOrEmpty(txtContractorLicNO.Text.Trim())
                && string.IsNullOrEmpty(txtContractorBusiName.Text.Trim()) && string.IsNullOrEmpty(txtSSN.Text.Trim()) && string.IsNullOrEmpty(txtFEIN.Text.Trim()))
            {
                ShowSearchCriteriaRequiredMessage(GetTextByKey("per_permitList_SearchCriteria_Required"));
                result = false;
            }

            return result;
        }

        /// <summary>
        /// Check whether has at least one search criteria has been entered when search by Permit.
        /// </summary>
        /// <returns>true: input at least one condition; false: doesn't input any condition</returns>
        private bool CheckInputConditionForPermit()
        {
            if (string.IsNullOrEmpty(txtPermitNumber.Text.Trim()) && string.IsNullOrEmpty(txtProjectName.Text.Trim())
                && (!txtStartDate.Visible || string.IsNullOrEmpty(txtStartDate.Text.Trim())) && (!txtEndDate.Visible || string.IsNullOrEmpty(txtEndDate.Text.Trim())) && ddlPermitType.SelectedIndex == 0
                && ddlPermitSubAgency.SelectedIndex == 0 && IsEmptyASICriteria(hfASIExpanded.Value, false) && ddlCapStatus.SelectedIndex == 0)
            {
                ShowSearchCriteriaRequiredMessage(GetTextByKey("per_permitList_SearchCriteria_Required"));
                return false;
            }

            if (txtStartDate.Visible && txtEndDate.Visible && txtStartDate.IsLaterThan(txtEndDate))
            {                
                string msg = GetTextByKey("per_permitList_msg_date_start_end");
                ShowSearchCriteriaRequiredMessage(msg);
                return false;
            }

            return true;
        }

        /// <summary>
        /// Check whether has at least one search criteria has been entered when search by Trade Name. 
        /// </summary>
        /// <returns>true: input at least one condition; false: doesn't input any condition</returns>
        private bool CheckInputConditionForTradeName()
        {
            bool result = true;

            if (string.IsNullOrEmpty(txtEnglishTradeName.Text.Trim()) && string.IsNullOrEmpty(txtArabicTradeName.Text.Trim()) && ddlTradeNameType.SelectedIndex == 0 && ddlTradeNameRecordStatus.SelectedIndex == 0)
            {
                ShowSearchCriteriaRequiredMessage(GetTextByKey("per_permitList_SearchCriteria_Required"));
                result = false;
            }

            return result;
        }

        /// <summary>
        /// Clone record.
        /// </summary>
        /// <param name="isSearchResult">indicate is search result or not.</param>
        /// <returns>Success or failed</returns>
        private bool CloneRecord(bool isSearchResult)
        {
            DataTable selectedCaps = new DataTable();

            if (isSearchResult)
            {
                //1.Search permit list.
                selectedCaps = dgvPermitList.GetSelectedCAPItems();
            }
            else
            {
                //2.My Permit List.
                selectedCaps = PermitList.GetSelectedCAPItems();
            }

            return CloneRecordUtil.ClonePermitListRecord(Page, selectedCaps, ModuleName);
        }

        /// <summary>
        /// Construct Cap Type Permission Model
        /// </summary>
        /// <param name="capTypePermission">the CapTypePermission model</param>
        /// <returns>return the CapTypePermission model</returns>
        private CapTypePermissionModel ConstructCapTypePermissionModel(CapTypePermissionModel capTypePermission)
        {
            CapTypePermissionModel newCapTypePermission = new CapTypePermissionModel();
            newCapTypePermission.serviceProviderCode = capTypePermission.serviceProviderCode;
            newCapTypePermission.moduleName = capTypePermission.moduleName;
            newCapTypePermission.group = capTypePermission.group;
            newCapTypePermission.type = capTypePermission.type;
            newCapTypePermission.subType = capTypePermission.subType;
            newCapTypePermission.category = capTypePermission.category;
            newCapTypePermission.controllerType = ControllerType.CAPSEARCHFILTER.ToString();
            newCapTypePermission.entityType = EntityType.LICENSETYPE.ToString();

            return newCapTypePermission;
        }

        /// <summary>
        /// Get Address data, search type by Address.
        /// </summary>
        /// <param name="currentPageIndex">The current page index.</param>
        /// <param name="sortExpression">The sort expression.</param>
        private void CreateAddressDataSource(int currentPageIndex, string sortExpression)
        {
            // set the query format
            PaginationModel pageInfo = PaginationUtil.GetPageInfoByID(PermitSearchType.ByAddress.ToString());
            pageInfo.SortExpression = sortExpression;
            pageInfo.CurrentPageIndex = currentPageIndex;
            pageInfo.CustomPageSize = dgvForAddress.PageSize;
            QueryFormat queryFormat = PaginationUtil.GetQueryFormatModel(pageInfo);
            DataTable dtAddress = GetAddressByQueryFormat(queryFormat).DataSource;

            dtAddress = PaginationUtil.MergeDataSource<DataTable>(GridViewDataSourceForAddress, dtAddress, pageInfo);
            SaveQueryInfo(PermitSearchType.ByAddress, chkSearch.Checked, AddressSearchModel, dtAddress, chkCrossModuleSearch.Checked);
            SearchAddress(dtAddress);
        }

        /// <summary>
        /// Get permit data, search type by Permit.
        /// </summary>
        /// <param name="currentPageIndex">The current page index.</param>
        /// <param name="sortExpression">The sort expression.</param>
        private void CreateCapDataSource(int currentPageIndex, string sortExpression)
        {
            PaginationModel pageInfo = PaginationUtil.GetPageInfoByID(PermitSearchType.ByPermit + dgvPermitList.ClientID);
            pageInfo.SortExpression = sortExpression;
            pageInfo.CurrentPageIndex = currentPageIndex;
            pageInfo.CustomPageSize = dgvPermitList.PageSize;
            QueryFormat queryFormat = PaginationUtil.GetQueryFormatModel(pageInfo);
            DataTable dtCap = GetPermitByQueryFormat(queryFormat, ref pageInfo).DataSource;

            DataTable capList = PaginationUtil.MergeDataSource<DataTable>(dgvPermitList.GridViewDataSource, dtCap, pageInfo);
            SaveQueryInfo(PermitSearchType.ByPermit, chkSearch.Checked, CapSearchModel, capList, chkCrossModuleSearch.Checked);
            SearchPermit(capList, pageInfo.CurrentPageIndex, pageInfo.SortExpression);
        }

        /// <summary>
        /// Get permit data by general search.
        /// </summary>
        /// <param name="currentPageIndex">The current page index.</param>
        /// <param name="sortExpression">The sort expression.</param>
        private void CreateCapDataSourceByGC(int currentPageIndex, string sortExpression)
        {            
            PaginationModel pageInfo = PaginationUtil.GetPageInfoByID(PermitSearchType.General + dgvPermitList.ClientID);
            pageInfo.SortExpression = sortExpression;
            pageInfo.CurrentPageIndex = currentPageIndex;
            pageInfo.CustomPageSize = dgvPermitList.PageSize;
            QueryFormat queryFormat = PaginationUtil.GetQueryFormatModel(pageInfo);
            DataTable dtCapGC = GetPermitGCByQueryFormat(queryFormat, ref pageInfo).DataSource;

            DataTable capList = PaginationUtil.MergeDataSource<DataTable>(dgvPermitList.GridViewDataSource, dtCapGC, pageInfo);
            SaveQueryInfo(PermitSearchType.General, chkSearch.Checked, CapGCSearchModel, capList, chkCrossModuleSearch.Checked);
            SearchPermit(capList, pageInfo.CurrentPageIndex, pageInfo.SortExpression);
        }

        /// <summary>
        /// Create contact table data, search type by contact.
        /// </summary>
        /// <param name="currentPageIndex">The current page index.</param>
        /// <param name="sortExpression">The sort expression.</param>
        private void CreateContactDataSource(int currentPageIndex, string sortExpression)
        {
            contactSearchForm.InitTemplateFieldsDisplay();

            // 2. intial contact list page index.
            contactList.PageIndex = 0;

            PaginationModel pageInfo = PaginationUtil.GetPageInfoByID(PermitSearchType.ByContact.ToString());
            pageInfo.SortExpression = sortExpression;
            pageInfo.CurrentPageIndex = currentPageIndex;
            pageInfo.CustomPageSize = contactList.PageSize;
            QueryFormat queryFormat = PaginationUtil.GetQueryFormatModel(pageInfo);

            int startDBRow = 0;
            DataTable dtContact = GetContactByQueryFormat(queryFormat, ref pageInfo).DataSource;
            pageInfo.StartDBRow = startDBRow;

            dtContact = PaginationUtil.MergeDataSource<DataTable>(contactList.DataSource, dtContact, pageInfo);

            // 5. bind contact list to girdview
            contactList.DataSource = dtContact;
            contactList.BindContactList();

            // 6. store the critirea and search result to session for restoring them when user go back to this page.
            SaveQueryInfo(PermitSearchType.ByContact, chkSearch.Checked, CapContactSearchModel, dtContact, chkCrossModuleSearch.Checked);

            // 7. show corresoponding result message
            DisplayContactResultMessage(dtContact.Rows.Count);
        }

        /// <summary>
        /// Get License data, search type by License.
        /// </summary>
        /// <param name="currentPageIndex">The current page index.</param>
        /// <param name="sortExpression">The sort expression.</param>
        private void CreateLicenseDataSource(int currentPageIndex, string sortExpression)
        {
            PaginationModel pageInfo = PaginationUtil.GetPageInfoByID(PermitSearchType.ByLicense.ToString());
            pageInfo.SortExpression = sortExpression;
            pageInfo.CurrentPageIndex = currentPageIndex;
            pageInfo.CustomPageSize = dgvLicense.PageSize;
            QueryFormat queryFormat = PaginationUtil.GetQueryFormatModel(pageInfo);
            DataTable dtLicenses = GetLicenseeByQueryFormat(queryFormat).DataSource;

            dtLicenses = PaginationUtil.MergeDataSource<DataTable>(GridViewDataSourceForLicense, dtLicenses, pageInfo);
            SaveQueryInfo(PermitSearchType.ByLicense, chkSearch.Checked, LPSearchModel, dtLicenses, chkCrossModuleSearch.Checked);
            SearchLicense(dtLicenses);
        }

        /// <summary>
        /// Get Trade Name Data, Search Type by Trade Name.
        /// </summary>
        /// <param name="currentPageIndex">The current page index.</param>
        /// <param name="sortExpression">The sort expression.</param>
        /// <returns>The trade name data table.</returns>
        private DataTable CreateTradeNameDataSource(int currentPageIndex, string sortExpression)
        {
            PaginationModel pageInfo = PaginationUtil.GetPageInfoByID(PermitSearchType.ByTradeName.ToString());
            pageInfo.SortExpression = sortExpression;
            pageInfo.CurrentPageIndex = currentPageIndex;
            pageInfo.CustomPageSize = dgvTradeName.PageSize;
            QueryFormat queryFormat = PaginationUtil.GetQueryFormatModel(pageInfo);
            DataTable dtTradeNames = GetTradeNameByQueryFormat(queryFormat, ref pageInfo).DataSource;

            dtTradeNames = PaginationUtil.MergeDataSource<DataTable>(GridViewDataSourceForTradeName, dtTradeNames, pageInfo);
            SaveQueryInfo(PermitSearchType.ByTradeName, chkSearch.Checked, LicenseSearchModel, dtTradeNames, chkCrossModuleSearch.Checked);

            return dtTradeNames;
        }

        /// <summary>
        /// Enable Contact Result
        /// </summary>
        /// <param name="contactCount">count for contact data table.</param>
        private void DisplayContactResultMessage(int contactCount)
        {
            if (contactCount > 0)
            {
                divContactResult.Visible = true;
                AssociatedSearchResultInfo.Display(contactList.CountSummary);
                updatePanel.Update();
            }
            else
            {
                HideContactResult();
            }
        }

        /// <summary>
        /// Expand Collapse script
        /// </summary>
        private void ExpandCollapse()
        {
            if (CapSearchType == PermitSearchType.General)
            {
                if (ACAConstant.COMMON_N.Equals(hfASIExpanded.Value))
                {
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(), "ExpandCollapseGSASI", string.Format("ExpandCollapseGSASI('{0}');", ACAConstant.COMMON_N), true);
                }
            }
            else if (CapSearchType == PermitSearchType.ByPermit)
            {
                if (ACAConstant.COMMON_N.Equals(hfASIExpanded.Value))
                {
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(), "ExpandCollapsePermitASI", string.Format("ExpandCollapsePermitASI('{0}');", ACAConstant.COMMON_N), true);
                }
            }
        }

        /// <summary>
        /// enable GS Load ASI Link.
        /// </summary>
        /// <param name="isEnable">whether enable?</param>
        private void EnableGSLoadASILink(bool isEnable)
        {
            if (!StandardChoiceUtil.IsDisplayASICriteria(ModuleName) && !AppSession.IsAdmin)
            {
                divGSExpandCollapseASI.Visible = false;
                divGSLoadASI.Visible = false;
                divGSASI.Visible = false;
            }
            else
            {
                divGSLoadASI.Visible = isEnable;
                btnGSLoadASI.ToolTip = GetTitleByKey("img_alt_expand_icon", "per_permitList_label_permit_expandASI", false);
                divGSExpandCollapseASI.Visible = !isEnable;
                divGSASI.Visible = !isEnable;
                lblGSNoASIMsg.Visible = StandardChoiceUtil.IsSuperAgency() && string.IsNullOrEmpty(generalSearchForm.SubAgencyDropDownList.SelectedValue);
                hfASIExpanded.Value = isEnable ? string.Empty : ACAConstant.COMMON_Y;
            }

            ExpandCollapse();
        }

        /// <summary>
        /// enable permit Load ASI Link.
        /// </summary>
        /// <param name="isEnable">whether enable?</param>
        private void EnablePermitLoadASILink(bool isEnable)
        {
            if (!StandardChoiceUtil.IsDisplayASICriteria(ModuleName) && !AppSession.IsAdmin)
            {
                divPermitLoadASI.Visible = false;
                divPermitExpandCollapseASI.Visible = false;
                divASIPermit.Visible = false;
            }
            else
            {
                divPermitLoadASI.Visible = isEnable;
                divPermitExpandCollapseASI.Visible = !isEnable;
                divASIPermit.Visible = !isEnable;
                lblPermitNoASIMsg.Visible = StandardChoiceUtil.IsSuperAgency() && !ddlPermitType.Enabled;
                hfASIExpanded.Value = isEnable ? string.Empty : ACAConstant.COMMON_Y;
            }

            lnkPermitLoadASI.ToolTip = GetTitleByKey("img_alt_expand_icon", "per_permitList_label_GS_expandASI", false);
            ExpandCollapse();
        }

        /// <summary>
        /// fill license query fields with license model
        /// </summary>
        /// <param name="licenseModel">a LicenseProfessionalModel</param>
        private void FillLicenseInfo(LicenseProfessionalModel licenseModel)
        {
            ddlLicenseType.SelectedValue = licenseModel.licenseType;
            ddlContactType.SelectedValue = licenseModel.typeFlag;
            txtLicenseNumber.Text = licenseModel.licenseNbr;
            txtFirstName.Text = licenseModel.contactFirstName;
            txtLastName.Text = licenseModel.contactLastName;
            txtBusiName.Text = licenseModel.businessName;
            txtBusiName2.Text = licenseModel.busName2;
            txtBusiLicense.Text = licenseModel.businessLicense;
            txtSSN.Text = licenseModel.socialSecurityNumber;
            txtFEIN.Text = licenseModel.fein;
            txtContractorBusiName.Text = licenseModel.contLicBusName;
            txtContractorLicNO.Text = licenseModel.contrLicNo;
        }

        /// <summary>
        /// fill permit query fields with cap model
        /// </summary>
        /// <param name="capModel">a CapModel4WS</param>
        private void FillPermitInfo(CapModel4WS capModel)
        {
            //Permit information
            txtPermitNumber.Text = capModel.altID;
            txtProjectName.Text = capModel.specialText;
            txtStartDate.Text2 = capModel.fileDate;
            txtEndDate.Text2 = capModel.endFileDate;

            CapTypeModel capTypeModel = capModel.capType;

            if (capTypeModel != null)
            {
                //Fill agency.
                ddlPermitSubAgency.SelectedValue = capTypeModel.serviceProviderCode;

                //selected any agency. fill cap type and expand/collapse.
                if (!string.IsNullOrEmpty(ddlPermitSubAgency.SelectedValue))
                {
                    BindPermitType(ddlPermitType, ddlPermitSubAgency.SelectedValue);
                    lnkPermitLoadASI.Enabled = true;
                    lnkPermitLoadASI.ToolTip = GetTitleByKey("img_alt_expand_icon", "per_permitList_label_GS_expandASI", false);
                    ddlPermitType.Enabled = true;
                }

                lblPermitNoASIMsg.Visible = StandardChoiceUtil.IsSuperAgency() && string.IsNullOrEmpty(ddlPermitSubAgency.SelectedValue);

                //Fill cap type.
                if (!string.IsNullOrEmpty(capTypeModel.resAlias) && !string.IsNullOrEmpty(capTypeModel.group))
                {
                    ddlPermitType.SelectedValue = CAPHelper.GetCapTypeValue(capTypeModel);
                }

                if (!string.IsNullOrEmpty(ddlPermitType.SelectedValue))
                {
                    ICapTypeBll capTypeBll = (ICapTypeBll)ObjectFactory.GetObject(typeof(ICapTypeBll));
                    AppStatusGroupModel4WS[] appStatusGroupModels = capTypeBll.GetAppStatusByCapType(GetSelectedCapTypeModel(ddlPermitSubAgency.SelectedValue, ddlPermitType));
                    DropDownListBindUtil.BindCapStatus(ddlCapStatus, ModuleName, appStatusGroupModels);
                    ddlCapStatus.Enabled = true;
                    ddlCapStatus.SelectedValue = capModel.capStatus;
                }
            }
        }

        /// <summary>
        /// fill trade name query fields with license professional model
        /// </summary>
        /// <param name="licenseModel">a LicenseModel4WS</param>
        private void FillTradeNameInfo(LicenseModel licenseModel)
        {
            ddlTradeNameType.SelectedValue = licenseModel.licenseType;
            txtEnglishTradeName.Text = licenseModel.businessName;
            txtArabicTradeName.Text = licenseModel.busName2;
            ddlTradeNameRecordStatus.SelectedValue = licenseModel.licenseStatus;
        }

        /// <summary>
        /// Filter Cap Type List
        /// </summary>
        /// <param name="capTypePermissionList">the capTypePermission model List</param>
        /// <returns>The capTypePermission model list. </returns>
        private CapTypeModel[] FilterCapTypeList(CapTypePermissionModel[] capTypePermissionList)
        {
            var userRoleBll = ObjectFactory.GetObject<IUserRoleBll>();
            ICapTypePermissionBll capTypePermissionBll = (ICapTypePermissionBll)ObjectFactory.GetObject(typeof(ICapTypePermissionBll));
            UserRolePrivilegeModel userRoleModel = new UserRolePrivilegeModel();
            ArrayList capTypeArray = new ArrayList();

            for (int i = 0; i < capTypePermissionList.Length; i++)
            {
                CapTypeModel capType = new CapTypeModel();

                userRoleModel = userRoleBll.ConvertToUserRolePrivilegeModel(string.IsNullOrEmpty(capTypePermissionList[i].entityPermission) ? ACAConstant.DEFAULT_PERMISSION : capTypePermissionList[i].entityPermission);
                CapTypePermissionModel lpTypeModel = ConstructCapTypePermissionModel(capTypePermissionList[i]);

                CapTypePermissionModel[] capTypePermissions = capTypePermissionBll.GetCapTypePermissions(capTypePermissionList[i].serviceProviderCode, lpTypeModel);

                ArrayList lpTypes = new ArrayList();
                if (capTypePermissions != null && capTypePermissions.Length > 0)
                {
                    foreach (CapTypePermissionModel model in capTypePermissions)
                    {
                        if (model.entityType == EntityType.LICENSETYPE.ToString())
                        {
                            lpTypes.Add(model.entityKey1);
                        }
                    }
                }

                userRoleModel.licenseTypeRuleArray = (string[])lpTypes.ToArray(typeof(string));

                if (userRoleBll.IsCapTypeHasRight(userRoleModel, false))
                {
                    capType.resAlias = capTypePermissionList[i].alias;
                    capType.moduleName = capTypePermissionList[i].moduleName;
                    capType.group = capTypePermissionList[i].group;
                    capType.type = capTypePermissionList[i].type;
                    capType.subType = capTypePermissionList[i].subType;
                    capType.category = capTypePermissionList[i].category;
                    capType.serviceProviderCode = capTypePermissionList[i].serviceProviderCode;

                    capTypeArray.Add(capType);
                }
            }

            if (capTypeArray != null && capTypeArray.Count > 0)
            {
                ICapTypeBll capTypeBll = (ICapTypeBll)ObjectFactory.GetObject(typeof(ICapTypeBll));
                return capTypeBll.GetCapTypeListByPKs((CapTypeModel[])capTypeArray.ToArray(typeof(CapTypeModel)));
            }

            return null;
        }

        /// <summary>
        /// Get Cap Type List.
        /// </summary>
        /// <param name="agencyCode">The agency code.</param>
        /// <returns>The Cap Type list.</returns>
        private CapTypeModel[] GetCapTypeList(string agencyCode)
        {
            //if agency drop down seleced null, get web config agency code.
            if (string.IsNullOrEmpty(agencyCode))
            {
                agencyCode = ConfigManager.AgencyCode;
            }

            bool isCurrentAgency = agencyCode.Equals(ConfigManager.AgencyCode, StringComparison.InvariantCulture);

            CapTypeModel[] captypes = null;

            IXPolicyBll xPolicyBll = (IXPolicyBll)ObjectFactory.GetObject(typeof(IXPolicyBll));
            XpolicyUserRolePrivilegeModel policy = xPolicyBll.GetPolicy(agencyCode, ACAConstant.USER_ROLE_POLICY_FOR_CAP_SEARCH, ACAConstant.LEVEL_TYPE_MODULE, ModuleName);

            if (!AppSession.IsAdmin && isCurrentAgency && policy != null)
            {
                bool isCapTypeLevel = ACAConstant.COMMON_ONE.Equals(policy.data4);

                if (isCapTypeLevel)
                {
                    ICapTypePermissionBll capTypePermissionBll = (ICapTypePermissionBll)ObjectFactory.GetObject(typeof(ICapTypePermissionBll));

                    CapTypePermissionModel capTypePermission = new CapTypePermissionModel();
                    capTypePermission.serviceProviderCode = agencyCode;
                    capTypePermission.controllerType = ControllerType.CAPSEARCHFILTER.ToString();
                    capTypePermission.entityType = EntityType.GENERAL.ToString();
                    capTypePermission.moduleName = ModuleName;

                    CapTypePermissionModel[] capTypePermissionList = capTypePermissionBll.GetPermissionByControllerType(agencyCode, capTypePermission);

                    if (capTypePermissionList != null && capTypePermissionList.Length > 0)
                    {
                        captypes = new CapTypeModel[capTypePermissionList.Length];
                        captypes = FilterCapTypeList(capTypePermissionList);
                    }
                }
                else
                {
                    var userRoleBll = ObjectFactory.GetObject<IUserRoleBll>();
                    UserRolePrivilegeModel userRoleModel = new UserRolePrivilegeModel();
                    userRoleModel = userRoleBll.ConvertToUserRolePrivilegeModel(policy.data3);

                    if (userRoleBll.IsCapTypeHasRight(userRoleModel, true))
                    {
                        ICapTypeBll capTypeBll = (ICapTypeBll)ObjectFactory.GetObject(typeof(ICapTypeBll));
                        CapTypeModel[] capTypeList = capTypeBll.GetCapTypeList(agencyCode, ModuleName, null);

                        if (capTypeList != null && capTypeList.Length > 0)
                        {
                            captypes = new CapTypeModel[capTypeList.Length];
                            captypes = capTypeList;
                        }
                    }
                }
            }
            else
            {
                ICapTypeBll capTypeBll = (ICapTypeBll)ObjectFactory.GetObject(typeof(ICapTypeBll));
                CapTypeModel[] capTypeList = capTypeBll.GetCapTypeList(agencyCode, ModuleName, null);

                if (capTypeList != null && capTypeList.Length > 0)
                {
                    captypes = new CapTypeModel[capTypeList.Length];
                    captypes = capTypeList;
                }
            }

            return captypes;
        }

        /// <summary>
        /// Get status of cross module search. If return value is false then 
        /// cross module search can't work.
        /// </summary>
        /// <returns>Cross module search is whether enabled.</returns>
        private bool GetCrossModuleSearchStatus()
        {
            bool isEnabled = CapUtil.EnableCrossModuleSearch() && TabUtil.GetCrossModules(ModuleName).Count > 0;
            return isEnabled;
        }

        /// <summary>
        /// Gets a Cap type model by cap type drop down list.
        /// </summary>
        /// <param name="ddlAgencySelectedValue">dropdownlist for agency.</param>
        /// <param name="ddlPermitType">dropdownlist for permit type</param>
        /// <returns>a CapTypeModel</returns>
        private CapTypeModel GetSelectedCapTypeModel(string ddlAgencySelectedValue, AccelaDropDownList ddlPermitType)
        {
            if (ddlPermitType == null)
            {
                return null;
            }

            CapTypeModel capTypeModel = new CapTypeModel();

            //permit type isn't "--select--".
            if (!string.IsNullOrEmpty(ddlPermitType.SelectedValue))
            {
                string[] capLevels = ddlPermitType.SelectedValue.Trim().Split('/');

                capTypeModel.resAlias = ddlPermitType.SelectedItem.Text;
                capTypeModel.moduleName = ModuleName;
                capTypeModel.group = capLevels.Length >= 0 ? capLevels[0] : string.Empty;
                capTypeModel.type = capLevels.Length >= 1 ? capLevels[1] : string.Empty;
                capTypeModel.subType = capLevels.Length >= 2 ? capLevels[2] : string.Empty;
                capTypeModel.category = capLevels.Length >= 3 ? capLevels[3] : string.Empty;
            }

            capTypeModel.serviceProviderCode = ConfigManager.AgencyCode;

            if (!string.IsNullOrEmpty(ddlAgencySelectedValue))
            {
                capTypeModel.serviceProviderCode = ddlAgencySelectedValue;
            }

            return capTypeModel;
        }

        /// <summary>
        /// get selected searching modules.
        /// </summary>
        /// <returns>the select search modules.</returns>
        private List<string> GetselectSearchModules()
        {
            List<string> selectSearchModules = new List<string>();

            if (chkCrossModuleSearch.Visible && chkCrossModuleSearch.Checked)
            {
                selectSearchModules = (List<string>)TabUtil.GetCrossModules(ModuleName);
            }

            return selectSearchModules;
        }

        /// <summary>
        /// Hidden Permit list and clear permit list data.
        /// </summary>
        /// <param name="isShowMessageBar">if set to <c>true</c> [is show message bar].</param>
        /// <param name="isShowSearchList">Hidden/Show search result list.</param>
        private void InitPermitResult(bool isShowMessageBar, bool isShowSearchList)
        {
            if (isShowMessageBar)
            {
                RecordSearchResultInfo.Display(0);
            }
            else
            {
                RecordSearchResultInfo.Hide();
            }

            dvSearchList.Visible = isShowSearchList;
            dvPromptForLicense.Visible = false;
            dvPromptForAddress.Visible = false;
            dvPromptForContact.Visible = false;
            dvResult.Visible = false;
        }

        /// <summary>
        /// Disable error message when Address or License has not been found.
        /// </summary>
        private void HideAddressResult()
        {
            InitDisplay();

            AssociatedSearchResultInfo.Display(0);
        }

        /// <summary>
        /// Disable error message when contact has not been found.
        /// </summary>
        private void HideContactResult()
        {
            InitDisplay();

            AssociatedSearchResultInfo.Display(0);
        }

        /// <summary>
        /// Initial ASI web control.
        /// </summary>
        private void InitASIControl()
        {
            asiGSForm.TempASIGroupInfo = null;
            asiGSForm.ClearControls();

            asiPermitForm.TempASIGroupInfo = null;
            asiPermitForm.ClearControls();
        }

        /// <summary>
        /// Initial ASIT web control.
        /// </summary>
        private void InitASITControl()
        {
            asitGSForm.TempASITGroupInfo = null;
            asitGSForm.ClearControls();
            asitPermitForm.TempASITGroupInfo = null;
            asitPermitForm.ClearControls();
        }

        /// <summary>
        /// Initial all Textbox, dropdownlist and grid view's value
        /// </summary>
        private void InitAllValue()
        {
            InitDisplay();
            InitDataSource();

            if (!AppSession.IsAdmin)
            {
                dvSearchForAddress.Visible = false;
                dvSearchForLicensed.Visible = false;
                dvSearchForPermit.Visible = false;
                dvGenearlSearch.Visible = false;
                dvSearchForTradeName.Visible = false;
                dvSearchForContact.Visible = false;
            }

            ITimeZoneBll timeBll = (ITimeZoneBll)ObjectFactory.GetObject(typeof(ITimeZoneBll));
            DateTime endDate = timeBll.GetAgencyCurrentDate(ConfigManager.AgencyCode);
            DateTime startDate = CapUtil.GetCapDefaultFindDateRange(ConfigManager.AgencyCode, endDate);

            //1.Set Default value to first option in General Search.
            generalSearchForm.ResetGeneralSearchForm(endDate, startDate);

            //2.Set Default value to first option in Search Permit.
            txtProjectName.Text = string.Empty;
            txtPermitNumber.Text = string.Empty;
            DropDownListBindUtil.SetSelectedToFirstItem(ddlPermitType);
            DropDownListBindUtil.SetSelectedToFirstItem(ddlPermitSubAgency);
            txtStartDate.Text2 = startDate;
            txtEndDate.Text2 = endDate;

            //3.Set Default value to first option in Search License.
            txtSSN.Text = string.Empty;
            txtFEIN.Text = string.Empty;
            txtFirstName.Text = string.Empty;
            txtLastName.Text = string.Empty;
            txtBusiName.Text = string.Empty;
            txtBusiName2.Text = string.Empty;
            txtBusiLicense.Text = string.Empty;
            txtLicenseNumber.Text = string.Empty;
            DropDownListBindUtil.SetSelectedToFirstItem(ddlLicenseType);
            DropDownListBindUtil.SetSelectedToFirstItem(ddlContactType);

            //4.Set Default value to first option in Search Address.
            searchByAddress.SetDefaultValue();

            //5.Set Default value to first option for Sarching of Trade Name.
            txtEnglishTradeName.Text = string.Empty;
            txtArabicTradeName.Text = string.Empty;
            DropDownListBindUtil.SetSelectedToFirstItem(ddlTradeNameType);
            DropDownListBindUtil.SetSelectedToFirstItem(ddlTradeNameRecordStatus);

            //6.Set Default value to ASI.
            hfASIExpanded.Value = string.Empty;
            InitASIControl();
            InitASITControl();

            generalSearchForm.InitGeneralSearchForm();
            searchByAddress.InitAddressForm();

            //7.Set Default value to search by contact.
            contactSearchForm.InitContactForm();
        }

        /// <summary>
        /// Initial UI for "cross module search".
        /// </summary>
        private void InitCrossModuleUI()
        {
            if (!Page.IsPostBack)
            {
                bool isEnabled = GetCrossModuleSearchStatus();
                chkCrossModuleSearch.Visible = isEnabled;
            }
        }

        /// <summary>
        /// Clear GridView's Data
        /// </summary>
        private void InitDataSource()
        {
            GridViewDataSourceForAddress = null;
            GridViewDataSourceForLicense = null;
            GridViewDataSourceForTradeName = null;
            dgvPermitList.GridViewDataSource = null;
            contactList.DataSource = null;
            GridViewDataSourceForTradeName = null;
        }

        /// <summary>
        /// Initial all visual control not include Search criteria.
        /// </summary>
        private void InitDisplay()
        {
            divPermitList.Visible = false;
            dvSearchList.Visible = false;
            divSeparateLine.Visible = false;
            RecordSearchResultInfo.Hide();
            AssociatedSearchResultInfo.Hide();

            dvAddressResult.Visible = false;
            dvLicenseResult.Visible = false;
            dvTradeNameResult.Visible = false;
            dgvForAddress.Controls.Clear();
            dgvLicense.Controls.Clear();
            dgvTradeName.Controls.Clear();
            dvPromptForAddress.Visible = false;
            dvPromptForLicense.Visible = false;
            dvPromptForContact.Visible = false;
            dvResult.Visible = false;
            divContactResult.Visible = false;
        }

        /// <summary>
        /// Initial general expend link UI.
        /// </summary>
        private void InitGSExpendASILink()
        {
            if (!StandardChoiceUtil.IsDisplayASICriteria(ModuleName) && !AppSession.IsAdmin)
            {
                divGSExpandCollapseASI.Visible = false;
                divGSLoadASI.Visible = false;
                divGSASI.Visible = false;
                return;
            }

            //in ACA admin, if need hide "Search Additional Criteria" and "Do not include Addtional Criteria"
            if (AppSession.IsAdmin)
            {
                divGSExpandCollapseASI.Visible = true;
                divGSLoadASI.Visible = true;
                divGSASI.Visible = false;
            }
            else
            {
                //if super agency, it needs selected agency and bind cap type by agency code.
                if (StandardChoiceUtil.IsSuperAgency())
                {
                    if (generalSearchForm.SubAgencyDropDownList.Visible && string.IsNullOrEmpty(generalSearchForm.SubAgencyDropDownList.SelectedValue))
                    {
                        btnGSLoadASI.Enabled = false;
                        btnGSLoadASI.ToolTip = GetTitleByKey("img_alt_expand_icon", "per_permitList_label_permit_expandASI", false);
                    }
                }

                InitASIControl();
                EnableGSLoadASILink(true);
            }
        }

        /// <summary>
        /// Initial general expend link UI.
        /// </summary>
        /// <param name="isHideinAdmin">in ACA admin, need hide "Search Additional Criteria" and "Do not include Additional Criteria"</param>
        private void InitPermitExpendASILink(bool isHideinAdmin)
        {
            if (!StandardChoiceUtil.IsDisplayASICriteria(ModuleName) && !AppSession.IsAdmin)
            {
                divPermitLoadASI.Visible = false;
                divPermitExpandCollapseASI.Visible = false;
                divASIPermit.Visible = false;
                return;
            }

            //in ACA admin, if need hide "Search Additional Criteria" and "Do not include Addtional Criteria"
            if (AppSession.IsAdmin)
            {
                divPermitExpandCollapseASI.Visible = !isHideinAdmin;
                divPermitLoadASI.Visible = !isHideinAdmin;
                divASIPermit.Visible = false;
            }
            else
            {
                //if super agency, it needs selected agency and bind cap type by agency code.
                if (StandardChoiceUtil.IsSuperAgency())
                {
                    if (ddlPermitSubAgency.Visible && string.IsNullOrEmpty(ddlPermitSubAgency.SelectedValue))
                    {
                        lnkPermitLoadASI.Enabled = false;
                        lnkPermitLoadASI.ToolTip = GetTitleByKey("img_alt_expand_icon", "per_permitList_label_GS_expandASI", false);
                        ddlPermitType.Enabled = false;
                    }
                }
                else
                {
                    //if normal agecny,  it bind cap type by ACA web configer.
                    BindPermitType(ddlPermitType, ConfigManager.AgencyCode);
                }

                EnablePermitLoadASILink(true);
            }
        }

        /// <summary>
        /// Initial the dropdownlist controls
        /// </summary>
        private void InitUI()
        {
            //Hide search section if passed Search type is none.
            searchSection.Visible = SearchType != PermitSearchType.None;
            divSearchSectionInstruction.Visible = searchSection.Visible;
            divMyPermitListSectionHeader.Visible = ShowMyPermitList;

            //Add for data filter dropdownlist
            lblPermitListHistory.InnerHtml = ScriptFilter.FilterScript(GetTextByKey("per_permitList_label_history"), false);
            spanRecordSectionInstruction.InnerHtml = ScriptFilter.FilterScript(GetTextByKey("per_permitList_label_history|sub"), false);

            if (ShowMyPermitList)
            {
                if (AppSession.IsAdmin)
                {
                    divMyRecordListSection4Daily.Visible = false;
                    spanRecordSectionInstruction.Visible = false;
                }
                else
                {
                    divMyRecordListSection4Admin.Visible = false;
                    long dataFilterId = DefaultDataFilterId;

                    if (!IsPostBack)
                    {
                        //If the "DISPLAY_QUICK_QUERY" is Yes/Y and the current user is not anonymous.
                        ddlDataFilter.Visible = StandardChoiceUtil.IsDisplayQuickQuery(ConfigManager.AgencyCode)
                                                && !AppSession.User.IsAnonymous;

                        if (ddlDataFilter.Visible)
                        {
                            //Init Data Filter Dropdownlist
                            DropDownListBindUtil.BindDataFilter(ddlDataFilter, DataFilterList, DefaultDataFilterId);
                            dataFilterId = string.IsNullOrEmpty(ddlDataFilter.SelectedValue) ? 0 : long.Parse(ddlDataFilter.SelectedValue);
                        }
                    }

                    QueryMyPermit(0, null, dataFilterId);
                }
            }

            // Show Map control
            if (!IsPostBack)
            {
                IsRightToLeft = I18nCultureUtil.UserPreferredCultureInfo.TextInfo.IsRightToLeft;

                //set default value to collection name.
                addForMyPermits.SetCollectionDefaultValue();
            }

            if (searchSection.Visible)
            {
                searchByAddress.InitUI();

                DropDownListBindUtil.BindSubAgencies(ddlPermitSubAgency);

                BindSearchList();

                DropDownListBindUtil.BindLicenseType(ddlLicenseType);
                DropDownListBindUtil.BindContactType4License(ddlContactType);

                SearchDefaultOptionForSearchType();
            }

            if (AppSession.IsAdmin)
            {
                DropDownListBindUtil.BindCapStatus(ddlCapStatus, ModuleName, null);
                BindPermitType(ddlPermitType, ConfigManager.AgencyCode);
                DropDownListBindUtil.BindLicenseType(ddlTradeNameType);

                ICapBll capBll = ObjectFactory.GetObject<ICapBll>();
                AppStatusGroupModel4WS[] appStatusGroupModels = capBll.GetAppStatusGroupBySPC(ConfigManager.AgencyCode, string.Empty);
                DropDownListBindUtil.BindCapStatus(ddlTradeNameRecordStatus, ModuleName, appStatusGroupModels, XPolicyConstant.RECORD_STATUS_SEARCH_FOR_TRADENAME);

                ddlSearchType.AutoPostBack = false;
                ddlSearchType.Attributes.Add("onchange", "ChangeType(this)");
                dvGenearlSearch.Visible = true;
                dvSearchForPermit.Visible = true;
                dvSearchForAddress.Visible = true;
                dvSearchForContact.Visible = true;
                dvSearchForTradeName.Visible = true;
                dvSearchForLicensed.Visible = true;
            }
        }

        /// <summary>
        /// Initial the grid view's export link visible.
        /// </summary>
        private void InitalExport()
        {
            if (!AppSession.IsAdmin && StandardChoiceUtil.IsEnableExport2CSV())
            {
                dgvForAddress.ShowExportLink = true;
                dgvForAddress.ExportFileName = "Address";
                dgvTradeName.ShowExportLink = true;
                dgvTradeName.ExportFileName = "TradeName";
                dgvLicense.ShowExportLink = true;
                dgvLicense.ExportFileName = "License";
                dgvPermitList.InitialExport(true);
                dgvPermitList.ExportFileName = "ReocrdList";
                PermitList.InitialExport(true);
                PermitList.ExportFileName = "MyRecordList";
            }
            else
            {
                dgvForAddress.ShowExportLink = false;
                dgvTradeName.ShowExportLink = false;
                dgvLicense.ShowExportLink = false;
                dgvPermitList.InitialExport(false);
                PermitList.InitialExport(false);
            }
        }

        /// <summary>
        /// Initial my permit flag
        /// </summary>
        private void IntialMyPermitFlag()
        {
            if ((AppSession.User.IsAnonymous && !AppSession.IsAdmin) || ddlSearchType.SelectedValue == ((int)PermitSearchType.ByTradeName).ToString())
            {
                mypermitFlag.Visible = false;
            }
            else
            {
                mypermitFlag.Visible = true;
            }
        }

        /// <summary>
        /// Check whether has at least one search criteria has been entered when expand ASI. 
        /// </summary>
        /// <param name="flagExpandCollapse">Flag for expand/collapse</param>
        /// <param name="isGeneralSearch">Flag which search type</param>
        /// <returns>whether empty criteria?</returns>
        private bool IsEmptyASICriteria(string flagExpandCollapse, bool isGeneralSearch)
        {
            bool isEmptyCriteria = true;
            bool isEmptyCriteriaASIT = true;

            if (ACAConstant.COMMON_Y.Equals(flagExpandCollapse))
            {
                isEmptyCriteria = isGeneralSearch ? asiGSForm.IsEmptySearchCriteria() : asiPermitForm.IsEmptySearchCriteria();
            }

            if (ValidationUtil.IsYes(flagExpandCollapse))
            {
                isEmptyCriteriaASIT = isGeneralSearch ? asitGSForm.IsEmptySearchCriteria() : asitPermitForm.IsEmptySearchCriteria();
            }

            return isEmptyCriteria && isEmptyCriteriaASIT;
        }

        /// <summary>
        /// load permit query info from session that has been saved by previous visit
        /// </summary>
        private void LoadPermitQueryInfo()
        {
            if (!IsPostBack && QueryInfo != null)
            {
                chkSearch.Checked = QueryInfo.IsSearchMyPermit;
                chkCrossModuleSearch.Checked = QueryInfo.IsSearchCrossModule;
                ddlSearchType.SelectedValue = ((int)QueryInfo.SearchType).ToString();
                ResetData();
                ChangeSearchType();
                switch (QueryInfo.SearchType)
                {
                    case PermitSearchType.ByAddress:
                        RefAddressModel addressModel = (RefAddressModel)QueryInfo.SearchModel;
                        searchByAddress.FillAddressInfo(addressModel);
                        dgvForAddress.PageIndex = QueryInfo.SearchResultPageIndex;
                        SearchAddress(PermitQueryInfo.SortDatatable(QueryInfo.SearchResult, QueryInfo.SearchResultSortExpression));

                        if (QueryInfo.HasSearchPermit)
                        {
                            BindPermitListForAddress(QueryInfo.PermitList, QueryInfo.PermitResultPageIndex, QueryInfo.PermitResultSortExpression);
                            lblResult.Text = QueryInfo.LabelForPermit;
                        }

                        break;

                    case PermitSearchType.ByLicense:
                        LicenseProfessionalModel licenseProfessionalModel = (LicenseProfessionalModel)QueryInfo.SearchModel;
                        FillLicenseInfo(licenseProfessionalModel);
                        dgvLicense.PageIndex = QueryInfo.SearchResultPageIndex;
                        SearchLicense(QueryInfo.SearchResult, QueryInfo.SearchResultSortExpression);

                        if (QueryInfo.HasSearchPermit)
                        {
                            BindPermitListForLicense(QueryInfo.PermitList, QueryInfo.LabelForPermit, QueryInfo.PermitResultPageIndex, QueryInfo.PermitResultSortExpression);
                        }

                        break;

                    case PermitSearchType.ByPermit:
                        CapModel4WS capModel = (CapModel4WS)QueryInfo.SearchModel;
                        FillPermitInfo(capModel);

                        //auto fill ASI info
                        if (capModel != null && capModel.appSpecificInfoGroups != null)
                        {
                            asiPermitForm.TempASIGroupInfo = capModel.appSpecificInfoGroups;
                        }

                        if (capModel != null && capModel.appSpecTableGroups != null)
                        {
                            CapTypeModel capTypeModel = GetSelectedCapTypeModel(ddlPermitSubAgency.SelectedValue, ddlPermitType);
                            asitPermitForm.SetASITable(capTypeModel, capModel.appSpecTableGroups);
                        }

                        SearchPermit(QueryInfo.PermitList, QueryInfo.PermitResultPageIndex, QueryInfo.PermitResultSortExpression);
                        break;

                    case PermitSearchType.General:
                        CapModel4WS capModelSC = (CapModel4WS)QueryInfo.SearchModel;

                        generalSearchForm.FillGeneralSearch(capModelSC);

                        //auto fill ASI info
                        if (capModelSC != null && capModelSC.appSpecificInfoGroups != null)
                        {
                            asiGSForm.TempASIGroupInfo = capModelSC.appSpecificInfoGroups;
                        }

                        if (capModelSC != null && capModelSC.appSpecTableGroups != null)
                        {
                            CapTypeModel capTypeModel = generalSearchForm.GetCapType();
                            asitGSForm.SetASITable(capTypeModel, capModelSC.appSpecTableGroups);
                        }

                        SearchPermit(QueryInfo.ComplexPermitList, QueryInfo.PermitResultPageIndex, QueryInfo.PermitResultSortExpression);
                        break;

                    case PermitSearchType.ByTradeName:
                        LicenseModel tradeNameModel = (LicenseModel)QueryInfo.SearchModel;
                        FillTradeNameInfo(tradeNameModel);
                        dgvTradeName.PageIndex = QueryInfo.SearchResultPageIndex;
                        SearchTradeName(PermitQueryInfo.SortDatatable(QueryInfo.SearchResult, QueryInfo.SearchResultSortExpression));
                        break;

                    case PermitSearchType.ByContact:
                        // 1. get search contact critirea
                        CapContactModel4WS capContactModel4WS = (CapContactModel4WS)QueryInfo.SearchModel;

                        //2. Fill contact info by contact model.
                        contactSearchForm.FillContactInfo(capContactModel4WS);

                        // 3. Setting contact list page index.
                        contactList.PageIndex = QueryInfo.SearchResultPageIndex;

                        //4. get the contact data source from previous search result when user click IE back button.
                        DataTable dtContact = QueryInfo.SearchResult;

                        // 5. bind contact list to girdview
                        contactList.DataSource = dtContact;
                        contactList.BindContactList(QueryInfo.SearchResultSortExpression);

                        // 6. show corresoponding result message
                        DisplayContactResultMessage(dtContact.Rows.Count);

                        // 7. bind CAP list to cap gridview
                        if (QueryInfo.HasSearchPermit)
                        {
                            BindPermitListForContact(QueryInfo.PermitList, QueryInfo.LabelForPermit, QueryInfo.PermitResultPageIndex, QueryInfo.PermitResultSortExpression);
                        }

                        break;

                    default:
                        break;
                }
            }
        }

        /// <summary>
        /// Get and display permit list, search by Address.
        /// </summary>
        /// <param name="addressRowIndex">the address row index.</param>
        /// <param name="agencyCode">the agency code.</param>
        /// <param name="currentPageIndex">The current page index.</param>
        /// <param name="sortExpression">The sort expression.</param>
        private void QueryCapByAddress(string addressRowIndex, string agencyCode, int currentPageIndex, string sortExpression)
        {
            string callerId = null;

            callerId = AppSession.User.PublicUserId;

            int index = int.Parse(addressRowIndex);
            DataRow row = GridViewDataSourceForAddress.Rows[index];

            AddressModel address = (AddressModel)row["AddressModel"];
            PaginationModel pageInfo = PaginationUtil.GetPageInfoByID(PermitSearchType.ByAddress + dgvPermitList.ClientID);
            pageInfo.SortExpression = sortExpression;
            pageInfo.CurrentPageIndex = currentPageIndex;
            pageInfo.CustomPageSize = dgvPermitList.PageSize;
            pageInfo.SearchCriterias = new object[] { addressRowIndex, agencyCode };
            QueryFormat queryFormat = PaginationUtil.GetQueryFormatModel(pageInfo);
            ICapBll capBll = (ICapBll)ObjectFactory.GetObject(typeof(ICapBll));
            string[] hiddenViewEltNames = ControlBuildHelper.GetHiddenViewElementNames(dgvPermitList.GViewID, ModuleName);
            SearchResultModel capResult = capBll.GetPermitsByAddress(address, agencyCode, ModuleName, callerId, chkSearch.Checked, GetselectSearchModules(), queryFormat, pageInfo.IsSearchAllStartRow, hiddenViewEltNames);
            pageInfo.StartDBRow = capResult.startRow;
            pageInfo.IsSearchAllStartRow = capResult.searchAllStartRow;

            DataTable capList = PaginationUtil.MergeDataSource<DataTable>(dgvPermitList.GridViewDataSource, dgvPermitList.CreateDataSource(capResult.resultList), pageInfo);
            QueryInfo.PermitList = capList;
            QueryInfo.IsBackFromCapDetailByAutoSkip = false;
            BindPermitListForAddress(capList, pageInfo.CurrentPageIndex, pageInfo.SortExpression);
        }

        /// <summary>
        /// query cap by contact model
        /// </summary>
        /// <param name="capContactModel4WS">cap contact model</param>
        /// <param name="currentPageIndex">The current page index.</param>
        /// <param name="sortExpression">The sort expression.</param>
        private void QueryCapByContact(CapContactModel4WS capContactModel4WS, int currentPageIndex, string sortExpression)
        {
            if (capContactModel4WS.people != null)
            {
                //2. Query CAP list by contactModel.
                PaginationModel pageInfo = PaginationUtil.GetPageInfoByID(PermitSearchType.ByContact + dgvPermitList.ClientID);
                pageInfo.SortExpression = sortExpression;
                pageInfo.CurrentPageIndex = currentPageIndex;
                pageInfo.CustomPageSize = dgvPermitList.PageSize;
                pageInfo.SearchCriterias = new object[] { capContactModel4WS };
                QueryFormat queryFormat = PaginationUtil.GetQueryFormatModel(pageInfo);
                ICapBll capBll = (ICapBll)ObjectFactory.GetObject(typeof(ICapBll));
                string[] hiddenViewEltNames = ControlBuildHelper.GetHiddenViewElementNames(dgvPermitList.GViewID, ModuleName);
                SearchResultModel capResult = capBll.GetPermitsByContact(capContactModel4WS, ModuleName, chkSearch.Checked, GetselectSearchModules(), queryFormat, pageInfo.IsSearchAllStartRow, hiddenViewEltNames);
                pageInfo.StartDBRow = capResult.startRow;
                pageInfo.IsSearchAllStartRow = capResult.searchAllStartRow;
                DataTable capList = PaginationUtil.MergeDataSource<DataTable>(dgvPermitList.GridViewDataSource, dgvPermitList.CreateDataSource(capResult.resultList), pageInfo);
                QueryInfo.PermitList = capList;

                // Display selected contact name
                if (capContactModel4WS.people != null)
                {
                    StringBuilder sbFullName = new StringBuilder();
                    string contactType = I18nStringUtil.GetString(capContactModel4WS.people.resContactType, capContactModel4WS.people.contactType);

                    if (ContactType4License.Organization.ToString().Equals(capContactModel4WS.people.contactTypeFlag, StringComparison.InvariantCultureIgnoreCase))
                    {
                        AppendString(sbFullName, capContactModel4WS.people.businessName);
                    }
                    else if (!string.IsNullOrEmpty(capContactModel4WS.people.fullName))
                    {
                        AppendString(sbFullName, capContactModel4WS.people.fullName);
                    }
                    else
                    {
                        AppendString(sbFullName, capContactModel4WS.people.firstName);
                        AppendString(sbFullName, capContactModel4WS.people.middleName);
                        AppendString(sbFullName, capContactModel4WS.people.lastName);
                        AppendString(sbFullName, capContactModel4WS.people.fullName);
                    }

                    string displayFullName = !string.IsNullOrWhiteSpace(sbFullName.ToString()) ? sbFullName.ToString() : ACAConstant.HTML_NBSP;
                    QueryInfo.LabelForPermit = string.Format("<table role='presentation'><tr><td>{0},</td><td>{1}</td><td>{2}</td></tr></table>", ScriptFilter.FilterScript(contactType), GetTextByKey("per_capHome_contact_name"), displayFullName);
                }

                //4. Bind CAP list to permit list data grid.
                dgvPermitList.ClearSelectedItems();
                QueryInfo.IsBackFromCapDetailByAutoSkip = false;
                BindPermitListForContact(capList, QueryInfo.LabelForPermit, pageInfo.CurrentPageIndex, pageInfo.SortExpression);
                QueryInfo.HasSearchPermit = true;

                ////5. display permit UI
                divSeparateLine.Visible = true;

                if (!AppSession.User.IsAnonymous)
                {
                    ScriptManager.RegisterClientScriptBlock(Page, GetType(), "HiddenAddForm", "HiddenAddForm();", true);
                }
            }
        }

        /// <summary>
        /// Get and display permit list, search by license.
        /// </summary>
        /// <param name="licenseID">string for License id</param>
        /// <param name="licenseType">string for license type</param>
        /// <param name="licenseTypeText">license type text.</param>
        /// <param name="agencyCode">the agency code.</param>
        /// <param name="currentPageIndex">The current page index.</param>
        /// <param name="sortExpression">The sort expression.</param>
        private void QueryCapByLicenseID(string licenseID, string licenseType, string licenseTypeText, string agencyCode, int currentPageIndex, string sortExpression)
        {
            //string agencyCode = ConfigManager.AgencyCode;
            string moduleName = ModuleName;
            string callerId = null;
            callerId = AppSession.User.PublicUserId;
            PaginationModel pageInfo = PaginationUtil.GetPageInfoByID(PermitSearchType.ByLicense + dgvPermitList.ClientID);
            pageInfo.SortExpression = sortExpression;
            pageInfo.CurrentPageIndex = currentPageIndex;
            pageInfo.CustomPageSize = dgvPermitList.PageSize;
            pageInfo.SearchCriterias = new object[] { licenseID, licenseType, licenseTypeText, agencyCode };
            QueryFormat queryFormat = PaginationUtil.GetQueryFormatModel(pageInfo);
            ICapBll capBll = (ICapBll)ObjectFactory.GetObject(typeof(ICapBll));
            string[] hiddenViewEltNames = ControlBuildHelper.GetHiddenViewElementNames(dgvPermitList.GViewID, ModuleName);
            SearchResultModel capResult = capBll.GetPermitsByLicenseId(licenseType, licenseID, agencyCode, moduleName, callerId, chkSearch.Checked, GetselectSearchModules(), queryFormat, pageInfo.IsSearchAllStartRow, hiddenViewEltNames);
            pageInfo.StartDBRow = capResult.startRow;
            pageInfo.IsSearchAllStartRow = capResult.searchAllStartRow;
            DataTable capList = PaginationUtil.MergeDataSource<DataTable>(dgvPermitList.GridViewDataSource, dgvPermitList.CreateDataSource(capResult.resultList), pageInfo);
            QueryInfo.PermitList = capList;
            QueryInfo.LabelForPermit = string.Format("<table role='presentation'><tr><td>{0}</td><td>,</td><td>{1}</td><td>{2}</td></tr></table>", licenseTypeText, GetTextByKey("ACA_CapHome_LicenseNumberText"), licenseID, false);
            QueryInfo.IsBackFromCapDetailByAutoSkip = false;
            BindPermitListForLicense(capList, QueryInfo.LabelForPermit, pageInfo.CurrentPageIndex, pageInfo.SortExpression);
        }

        /// <summary>
        /// Query my permit
        /// </summary>
        /// <param name="currentPageIndex">The current page index.</param>
        /// <param name="sortExpression">The sort expression.</param>
        /// <param name="dataFilterId">The data filter id.</param>
        /// <param name="isNeedClearData">indicate whether need to clear</param>
        private void QueryMyPermit(int currentPageIndex, string sortExpression, long dataFilterId, bool isNeedClearData = false)
        {
            if (AppSession.User.IsAnonymous)
            {
                return;
            }

            string userSeq = string.Empty;
            CapModel4WS capModel = new CapModel4WS();
            capModel.moduleName = ModuleName;
            userSeq = AppSession.User.UserSeqNum;

            if (PermitList.GridViewDataSource != null && isNeedClearData)
            {
                PermitList.GridViewDataSource = null;
            }

            PaginationModel pageInfo = PaginationUtil.GetPageInfoByID(PermitList.ClientID);
            pageInfo.SortExpression = sortExpression;
            pageInfo.CurrentPageIndex = currentPageIndex;
            pageInfo.CustomPageSize = PermitList.PageSize;
            QueryFormat queryFormat = PaginationUtil.GetQuickQueryFormatModel(pageInfo, int.Parse(GviewID.PermitList), dataFilterId);

            ICapBll capBll = (ICapBll)ObjectFactory.GetObject(typeof(ICapBll));
            string[] hiddenViewEltNames = ControlBuildHelper.GetHiddenViewElementNames(PermitList.GViewID, ModuleName);
            SearchResultModel capResult = capBll.GetMyCapList4ACA(ConfigManager.AgencyCode, capModel, queryFormat, userSeq, null, hiddenViewEltNames);

            DataTable capList = PaginationUtil.MergeDataSource<DataTable>(PermitList.GridViewDataSource, PermitList.CreateDataSource(capResult.resultList), pageInfo);
            PermitList.BindCapList(capList);
        }

        /// <summary>
        /// Reset display content when post back.
        /// </summary>
        private void ResetData()
        {
            MessageUtil.HideMessageByControl(Page);
        }

        /// <summary>
        /// Save query info
        /// </summary>
        /// <param name="searchType">search type</param>
        /// <param name="isSearchMyPermit">Is search my permit</param>
        /// <param name="searchModel">search model.</param>
        /// <param name="searchResult">search result.</param>
        /// <param name="isSearchCrossModule">is Search Cross Module</param>
        private void SaveQueryInfo(PermitSearchType searchType, bool isSearchMyPermit, object searchModel, object searchResult, bool isSearchCrossModule)
        {
            if (QueryInfo == null)
            {
                QueryInfo = PermitQueryInfo.CreateInstance(searchType, isSearchMyPermit, ModuleName, searchModel, searchResult, isSearchCrossModule);
            }
            else
            {
                PermitQueryInfo.UpdateInstance(QueryInfo, searchType, isSearchMyPermit, searchModel, searchResult, isSearchCrossModule);
            }
        }

        /// <summary>
        /// scroll screen to cap result list
        /// </summary>
        private void ScrollToCapResult()
        {
            ScriptManager.RegisterClientScriptBlock(Page, GetType(), "GotoCAPResult", "scrollIntoView('PageResult');", true);
        }

        /// <summary>
        /// Display Address list on the page, search type by Address.
        /// </summary>
        /// <param name="dtAddress">data table for address.</param>
        private void SearchAddress(DataTable dtAddress)
        {
            GridViewDataSourceForAddress = dtAddress;

            if (dtAddress.Rows.Count > 0)
            {
                dgvForAddress.DataSource = dtAddress;
                dgvForAddress.DataBind();

                dvAddressResult.Visible = true;
                dgvForAddress.Visible = true;
                AssociatedSearchResultInfo.Display(dgvForAddress.CountSummary);
            }
            else
            {
                HideAddressResult();
            }
        }

        /// <summary>
        /// Set Default option for Search Type.
        /// </summary>
        private void SearchDefaultOptionForSearchType()
        {
            //Set default Search type to general search
            PermitSearchType searchType = SearchType;

            if (searchType != PermitSearchType.None && ddlSearchType.Items.Count > 0)
            {
                string itemValue = ((int)searchType).ToString(CultureInfo.InvariantCulture.NumberFormat);
                ListItem foundItem = ddlSearchType.Items.FindByValue(itemValue);

                if (foundItem != null)
                {
                    int itemIndex = ddlSearchType.Items.IndexOf(foundItem);
                    ddlSearchType.SelectedIndex = itemIndex;
                }
            }

            ChangeSearchType();
        }

        /// <summary>
        /// Display License list on the page, search type by License.
        /// </summary>
        /// <param name="dtLicenses">data table for licenses</param>
        /// <param name="sortExpression">The sort expression.</param>
        private void SearchLicense(DataTable dtLicenses, string sortExpression = null)
        {
            GridViewDataSourceForLicense = dtLicenses;

            if (dtLicenses.Rows.Count > 0)
            {
                DataTable dt = GridViewBuildHelper.MergeTemplateAttributes2DataTable(dgvLicense, dtLicenses);

                if (!string.IsNullOrEmpty(sortExpression))
                {
                    dt.DefaultView.Sort = sortExpression;
                    dt = dt.DefaultView.ToTable();
                }

                dgvLicense.DataSource = dt;
                dgvLicense.DataBind();

                dvLicenseResult.Visible = true;
                dgvLicense.Visible = true;
                AssociatedSearchResultInfo.Display(dgvLicense.CountSummary);
            }
            else
            {
                HideAddressResult();
            }
        }

        /// <summary>
        /// Display Permit list on the page, search type by Permit.
        /// </summary>
        /// <param name="capList">SimpleCapModel4WS array</param>
        /// <param name="pageIndex">the page index.</param>
        /// <param name="sort">the sort string.</param>
        private void SearchPermit(DataTable capList, int pageIndex, string sort)
        {
            if (!IsAutoSkip2CapDetail(capList))
            {
                dgvPermitList.BindCapList(capList, pageIndex, sort);
                divPermitList.Visible = capList != null && capList.Rows.Count > 0;
                dvSearchList.Visible = true;

                if (capList == null || capList.Rows.Count == 0)
                {
                    InitPermitResult(!QueryInfo.IsBackFromCapDetailByAutoSkip, false);
                }
                else
                {
                    RecordSearchResultInfo.Display(dgvPermitList.CountSummary);
                }
            }
        }

        /// <summary>
        /// Display License list on the page, search type by License.
        /// </summary>
        /// <param name="dtTradeNames">data table for trade name.</param>
        private void SearchTradeName(DataTable dtTradeNames)
        {
            GridViewDataSourceForTradeName = dtTradeNames;

            if (dtTradeNames.Rows.Count > 0)
            {
                dgvTradeName.DataSource = dtTradeNames;
                dgvTradeName.DataBind();

                dvTradeNameResult.Visible = true;
                dgvTradeName.Visible = true;
                AssociatedSearchResultInfo.Display(dgvTradeName.CountSummary);
            }
            else
            {
                HideAddressResult();
            }
        }

        /// <summary>
        /// Set current City and State
        /// </summary>
        private void SetCurrentCityAndState()
        {
            generalSearchForm.AutoFillCityAndState();
            searchByAddress.SetCurrentCityAndState();
        }

        /// <summary>
        /// Set GV Sort
        /// </summary>
        /// <param name="queryInfo">Permit query info</param>
        private void SetGridViewSort(PermitQueryInfo queryInfo)
        {
            if (queryInfo != null && queryInfo.SearchResult != null)
            {
                DataView dataView = new DataView(queryInfo.SearchResult);

                if (!string.IsNullOrEmpty(queryInfo.SearchResultSortExpression) &&
                    queryInfo.SearchResult.Columns.Contains(queryInfo.SearchResultSortExpression.Split(' ')[0]))
                {
                    dataView.Sort = queryInfo.SearchResultSortExpression;
                }

                switch (queryInfo.SearchType)
                {
                    case PermitSearchType.ByAddress:
                        dgvForAddress.PageIndex = queryInfo.SearchResultPageIndex;
                        dgvForAddress.DataSource = dataView.ToTable();
                        GridViewDataSourceForAddress = dataView.ToTable();
                        dgvForAddress.DataBind();
                        break;

                    case PermitSearchType.ByLicense:
                        DataTable dt = GridViewBuildHelper.MergeTemplateAttributes2DataTable(dgvLicense, dataView.ToTable());
                        dt.DefaultView.Sort = queryInfo.SearchResultSortExpression;

                        dgvLicense.PageIndex = queryInfo.SearchResultPageIndex;
                        dgvLicense.DataSource = dt;
                        GridViewDataSourceForLicense = dataView.ToTable();
                        dgvLicense.DataBind();
                        break;

                    case PermitSearchType.ByTradeName:
                        dgvTradeName.PageIndex = queryInfo.SearchResultPageIndex;
                        dgvTradeName.DataSource = dataView.ToTable();
                        GridViewDataSourceForTradeName = dataView.ToTable();
                        dgvTradeName.DataBind();
                        break;

                    case PermitSearchType.ByContact:
                        contactList.PageIndex = queryInfo.SearchResultPageIndex;
                        contactList.DataSource = dataView.ToTable();
                        contactList.BindContactList();
                        break;

                    default:
                        break;
                }
            }
        }

        /// <summary>
        /// Show Error message and scroll to this message.
        /// </summary>
        /// <param name="message">the error message.</param>
        private void ShowSearchCriteriaRequiredMessage(string message)
        {
            MessageUtil.ShowMessageByControl(Page, MessageType.Error, message);
            ScriptManager.RegisterClientScriptBlock(Page, GetType(), "GotoMessageBar", "scrollIntoView('messageSpan');", true);
        }

        /// <summary>
        /// Get the general search information by query format.
        /// </summary>
        /// <param name="queryFormat">The query format model.</param>
        /// <returns>download result model that contains general search information.</returns>
        private DownloadResultModel GetGeneralSearchByQueryFormat(QueryFormat queryFormat)
        {
            CapModel4WS capModel = new CapModel4WS();
            capModel.moduleName = ModuleName;
            string userSeq = AppSession.User.IsAnonymous ? string.Empty : AppSession.User.UserSeqNum;
            string[] hiddenViewEltNames = ControlBuildHelper.GetHiddenViewElementNames(PermitList.GViewID, ModuleName);

            // search for download
            ICapBll capBll = ObjectFactory.GetObject<ICapBll>();
            SearchResultModel capResult = capBll.GetMyCapList4ACA(ConfigManager.AgencyCode, capModel, queryFormat, userSeq, null, hiddenViewEltNames);
            DataTable result = PermitList.CreateDataSource(capResult.resultList);

            DownloadResultModel model = new DownloadResultModel();
            model.StartDBRow = capResult.startRow;
            model.DataSource = result;

            return model;
        }

        /// <summary>
        /// Get address list by query condition
        /// </summary>
        /// <param name="queryFormat">the query format</param>
        /// <returns>download result model that contains address list</returns>
        private DownloadResultModel GetAddressByQueryFormat(QueryFormat queryFormat)
        {
            CapIDModel4WS capIdModel = new CapIDModel4WS();
            capIdModel.serviceProviderCode = ConfigManager.AgencyCode;

            IAddressBll addressBll = ObjectFactory.GetObject<IAddressBll>();
            DataTable result = addressBll.GetDailyAddressesByRefAddressModel(capIdModel, AddressSearchModel, queryFormat);

            DownloadResultModel model = new DownloadResultModel();
            model.StartDBRow = 0;
            model.DataSource = result;

            return model;
        }

        /// <summary>
        /// Get contact by query criteria
        /// </summary>
        /// <param name="queryFormat">query format</param>
        /// <param name="pageInfo">pagination model</param>
        /// <returns>download result model that contains contact list</returns>
        private DownloadResultModel GetContactByQueryFormat(QueryFormat queryFormat, ref PaginationModel pageInfo)
        {
            IPeopleBll peopleBLL = ObjectFactory.GetObject<IPeopleBll>();
            SearchResultModel capContactResult = peopleBLL.GetContactListByCapContactModel(ModuleName, CapContactSearchModel, contactList.TemplateAttributeNames, queryFormat);
            pageInfo.StartDBRow = capContactResult.startRow;
            CapContactModel4WS[] capContactModelArray = ObjectConvertUtil.ConvertObjectArray2EntityArray<CapContactModel4WS>(capContactResult.resultList);
            DataTable result = peopleBLL.ConvertContactListToDataTable(capContactModelArray);

            DownloadResultModel model = new DownloadResultModel();
            model.StartDBRow = pageInfo.StartDBRow;
            model.DataSource = result;

            return model;
        }

        /// <summary>
        /// Get contact by query criteria
        /// </summary>
        /// <param name="queryFormat">query format</param>
        /// <returns>download result model that contains contact list</returns>
        private DownloadResultModel GetContactByQueryFormat(QueryFormat queryFormat)
        {
            PaginationModel pageInfo = new PaginationModel();
            DownloadResultModel resultModel = GetContactByQueryFormat(queryFormat, ref pageInfo);

            // merge the template data
            DataTable dtSource = contactList.MergeTemplateData(resultModel.DataSource);
            resultModel.DataSource = dtSource;

            return resultModel;
        }

        /// <summary>
        /// Get license list by query condition
        /// </summary>
        /// <param name="queryFormat">the query format</param>
        /// <returns>download result model that contains license list</returns>
        private DownloadResultModel GetLicenseeByQueryFormat(QueryFormat queryFormat)
        {
            ILicenseBLL licenseBll = ObjectFactory.GetObject<ILicenseBLL>();
            bool isExistTemplateColumn = GridViewBuildHelper.IsExistTemplateColumn(dgvLicense);
            LicenseProfessionalModel[] searchResult = licenseBll.QueryLicenses(LPSearchModel, isExistTemplateColumn, queryFormat);
            DataTable resultList = licenseBll.BuildLicenseProfessionDataTable(searchResult);
            DataTable result = GridViewBuildHelper.MergeTemplateAttributes2DataTable(dgvLicense, resultList);

            DownloadResultModel model = new DownloadResultModel();
            model.StartDBRow = 0;
            model.DataSource = result;

            return model;
        }

        /// <summary>
        /// Get trade name by query criteria
        /// </summary>
        /// <param name="queryFormat">query format</param>
        /// <param name="pageInfo">pagination model</param>
        /// <returns>download result model that contains trade name list</returns>
        private DownloadResultModel GetTradeNameByQueryFormat(QueryFormat queryFormat, ref PaginationModel pageInfo)
        {
            ILicenseProfessionalBll licenseProfessionalBll = ObjectFactory.GetObject<ILicenseProfessionalBll>();
            SearchResultModel searchResult = licenseProfessionalBll.GetTradeNameListByLicenseModel(LicenseSearchModel, ACAConstant.REQUEST_PARMETER_TRADE_NAME, ModuleName, queryFormat);
            pageInfo.StartDBRow = searchResult.startRow;
            LicenseModel[] arrayLP = ObjectConvertUtil.ConvertObjectArray2EntityArray<LicenseModel>(searchResult.resultList);
            DataTable result = BuildLicenseDataTable(arrayLP);

            DownloadResultModel model = new DownloadResultModel();
            model.StartDBRow = pageInfo.StartDBRow;
            model.DataSource = result;

            return model;
        }

        /// <summary>
        /// Get trade name by query criteria
        /// </summary>
        /// <param name="queryFormat">query format</param>
        /// <returns>download result model that contains trade name list</returns>
        private DownloadResultModel GetTradeNameByQueryFormat(QueryFormat queryFormat)
        {
            PaginationModel pageInfo = new PaginationModel();

            return GetTradeNameByQueryFormat(queryFormat, ref pageInfo);
        }

        /// <summary>
        /// Get permit by query criteria
        /// </summary>
        /// <param name="queryFormat">query format</param>
        /// <param name="pageInfo">pagination model</param>
        /// <returns>download result model that contains permit list</returns>
        private DownloadResultModel GetPermitByQueryFormat(QueryFormat queryFormat, ref PaginationModel pageInfo)
        {
            string userSeq = null;

            if (!AppSession.User.IsAnonymous)
            {
                userSeq = AppSession.User.UserSeqNum;
            }

            ICapBll capBll = ObjectFactory.GetObject<ICapBll>();
            string[] hiddenViewEltNames = ControlBuildHelper.GetHiddenViewElementNames(dgvPermitList.GViewID, ModuleName);
            SearchResultModel capResult = capBll.GetCapList4ACA(CapSearchModel, queryFormat, userSeq, null, chkSearch.Checked, GetselectSearchModules(), pageInfo.IsSearchAllStartRow, hiddenViewEltNames);
            pageInfo.StartDBRow = capResult.startRow;
            pageInfo.IsSearchAllStartRow = capResult.searchAllStartRow;
            DataTable result = dgvPermitList.CreateDataSource(capResult.resultList);

            DownloadResultModel model = new DownloadResultModel();
            model.StartDBRow = pageInfo.StartDBRow;
            model.DataSource = result;

            return model;
        }

        /// <summary>
        /// Get permit by query criteria
        /// </summary>
        /// <param name="queryFormat">query format</param>
        /// <returns>download result model that contains permit list</returns>
        private DownloadResultModel GetPermitByQueryFormat(QueryFormat queryFormat)
        {
            PaginationModel pageInfo = new PaginationModel();

            return GetPermitByQueryFormat(queryFormat, ref pageInfo);
        }

        /// <summary>
        /// Get address cap by query criteria
        /// </summary>
        /// <param name="queryFormat">query format</param>
        /// <returns>download result model that contains cap list</returns>
        private DownloadResultModel GetAddressCapByQueryFormat(QueryFormat queryFormat)
        {
            PaginationModel pageInfo = PaginationUtil.GetPageInfoByID(PermitSearchType.ByAddress + dgvPermitList.ClientID);
            string callerId = AppSession.User.PublicUserId;
            int index = int.Parse(pageInfo.SearchCriterias[0].ToString());
            DataRow row = GridViewDataSourceForAddress.Rows[index];
            AddressModel address = (AddressModel)row["AddressModel"];
            string agencyCode = pageInfo.SearchCriterias[1].ToString();

            ICapBll capBll = ObjectFactory.GetObject<ICapBll>();
            string[] hiddenViewEltNames = ControlBuildHelper.GetHiddenViewElementNames(dgvPermitList.GViewID, ModuleName);
            SearchResultModel capResult = capBll.GetPermitsByAddress(address, agencyCode, ModuleName, callerId, chkSearch.Checked, GetselectSearchModules(), queryFormat, pageInfo.IsSearchAllStartRow, hiddenViewEltNames);
            DataTable result = dgvPermitList.CreateDataSource(capResult.resultList);

            DownloadResultModel model = new DownloadResultModel();
            model.StartDBRow = capResult.startRow;
            model.DataSource = result;

            return model;
        }

        /// <summary>
        /// Get license cap by query criteria
        /// </summary>
        /// <param name="queryFormat">query format</param>
        /// <returns>download result model that contains cap list</returns>
        private DownloadResultModel GetLicenseCapByQueryFormat(QueryFormat queryFormat)
        {
            PaginationModel pageInfo = PaginationUtil.GetPageInfoByID(PermitSearchType.ByLicense + dgvPermitList.ClientID);
            string licenseID = pageInfo.SearchCriterias[0].ToString();
            string licenseType = pageInfo.SearchCriterias[1].ToString();
            string agencyCode = pageInfo.SearchCriterias[3].ToString();
            string callerId = AppSession.User.PublicUserId;

            ICapBll capBll = ObjectFactory.GetObject<ICapBll>();
            string[] hiddenViewEltNames = ControlBuildHelper.GetHiddenViewElementNames(dgvPermitList.GViewID, ModuleName);
            SearchResultModel capResult = capBll.GetPermitsByLicenseId(licenseType, licenseID, agencyCode, ModuleName, callerId, chkSearch.Checked, GetselectSearchModules(), queryFormat, pageInfo.IsSearchAllStartRow, hiddenViewEltNames);
            DataTable result = dgvPermitList.CreateDataSource(capResult.resultList);

            DownloadResultModel model = new DownloadResultModel();
            model.StartDBRow = capResult.startRow;
            model.DataSource = result;

            return model;
        }

        /// <summary>
        /// Get general search by query criteria
        /// </summary>
        /// <param name="queryFormat">query format</param>
        /// <param name="pageInfo">pagination model</param>
        /// <returns>download result model that contains general search list</returns>
        private DownloadResultModel GetPermitGCByQueryFormat(QueryFormat queryFormat, ref PaginationModel pageInfo)
        {
            string userSeq = null;

            if (!AppSession.User.IsAnonymous)
            {
                userSeq = AppSession.User.UserSeqNum;
            }

            ICapBll capBll = ObjectFactory.GetObject<ICapBll>();
            string[] hiddenViewEltNames = ControlBuildHelper.GetHiddenViewElementNames(dgvPermitList.GViewID, ModuleName);
            SearchResultModel capResult = capBll.QueryPermitsGC(CapGCSearchModel, GetselectSearchModules(), userSeq, chkSearch.Checked, queryFormat, pageInfo.IsSearchAllStartRow, hiddenViewEltNames);
            pageInfo.StartDBRow = capResult.startRow;
            pageInfo.IsSearchAllStartRow = capResult.searchAllStartRow;
            DataTable result = dgvPermitList.CreateDataSource(capResult.resultList);

            DownloadResultModel model = new DownloadResultModel();
            model.StartDBRow = pageInfo.StartDBRow;
            model.DataSource = result;

            return model;
        }

        /// <summary>
        /// Get general search by query criteria
        /// </summary>
        /// <param name="queryFormat">query format</param>
        /// <returns>download result model that contains general search list</returns>
        private DownloadResultModel GetPermitGCByQueryFormat(QueryFormat queryFormat)
        {
            PaginationModel pageInfo = new PaginationModel();

            return GetPermitGCByQueryFormat(queryFormat, ref pageInfo);
        }

        /// <summary>
        /// Get the download data by quick query
        /// </summary>
        /// <param name="queryFormat">query format</param>
        /// <returns>download result model </returns>
        private DownloadResultModel GetPermitByQuickQuery(QueryFormat queryFormat)
        {
            string userSeq = string.Empty;
            CapModel4WS capModel = new CapModel4WS();
            capModel.moduleName = ModuleName;
            userSeq = AppSession.User.UserSeqNum;

            PaginationModel pageInfo = PaginationUtil.GetPageInfoByID(PermitList.ClientID);
            pageInfo.SortExpression = null;
            pageInfo.CurrentPageIndex = 0;
            pageInfo.CustomPageSize = PermitList.PageSize;
            QueryFormat qf = PaginationUtil.GetQuickQueryFormatModel(pageInfo, int.Parse(GviewID.PermitList), long.Parse(ddlDataFilter.SelectedValue));
            qf.startRow = queryFormat.startRow;
            qf.endRow = queryFormat.endRow;

            ICapBll capBll = ObjectFactory.GetObject<ICapBll>();
            string[] hiddenViewEltNames = ControlBuildHelper.GetHiddenViewElementNames(PermitList.GViewID, ModuleName);
            SearchResultModel capResult = capBll.GetMyCapList4ACA(ConfigManager.AgencyCode, capModel, qf, userSeq, null, hiddenViewEltNames);

            DownloadResultModel model = new DownloadResultModel();
            model.StartDBRow = 0;
            model.DataSource = PermitList.CreateDataSource(capResult.resultList);

            return model;
        }

        /// <summary>
        /// Get license field's formatted mapping
        /// </summary>
        /// <param name="dataRow">The grid view row</param>
        /// <param name="visibleColumns">The visible columns.</param>
        /// <returns> license field's formatted mapping dictionary</returns>
        private Dictionary<string, string> LicenseeExportFormat(DataRow dataRow, List<string> visibleColumns)
        {
            Dictionary<string, string> result = new Dictionary<string, string>();
            const string ColLicenseNumber = "LicenseNumber";
            const string ColContactType = "ContactType";

            if (dataRow != null)
            {
                if (visibleColumns.Contains(ColLicenseNumber))
                {
                    result.Add(ColLicenseNumber, ScriptFilter.FilterScriptEx(dataRow[ColLicenseNumber].ToString()));
                }

                if (visibleColumns.Contains(ColContactType))
                {
                    result.Add(ColContactType, DropDownListBindUtil.GetTypeFlagTextByValue(dataRow[ColContactType].ToString()));
                }
            }

            return result;
        }

        /// <summary>
        /// Get contact field's formatted mapping
        /// </summary>
        /// <param name="dataRow">The grid view row</param>
        /// <param name="visibleColumns">The visible columns.</param>
        /// <returns>contact field's formatted mapping dictionary</returns>
        private Dictionary<string, string> ContactExportFormat(DataRow dataRow, List<string> visibleColumns)
        {
            Dictionary<string, string> result = new Dictionary<string, string>();
            string colSalutation = ColumnConstant.Contact.Salutation.ToString();
            string colGender = ColumnConstant.Contact.Gender.ToString();
            string colContactType = ColumnConstant.Contact.ContactType.ToString();
            string colContactTypeFlag = ColumnConstant.Contact.ContactTypeFlag.ToString();
            string colContactPermission = ColumnConstant.Contact.ContactPermission.ToString();

            if (dataRow != null)
            {
                if (visibleColumns.Contains(colSalutation))
                {
                    result.Add(colSalutation, StandardChoiceUtil.GetStandardChoiceValueByKey(BizDomainConstant.STD_CAT_SALUTATION, dataRow[colSalutation].ToString()));
                }

                if (visibleColumns.Contains(colGender))
                {
                    result.Add(colGender, StandardChoiceUtil.GetGenderByKey(dataRow[colGender].ToString()));
                }

                if (visibleColumns.Contains(colContactType))
                {
                    result.Add(colContactType, StandardChoiceUtil.GetStandardChoiceValueByKey(BizDomainConstant.STD_CAT_CONTACT_TYPE, dataRow[colContactType].ToString()));
                }

                if (visibleColumns.Contains(colContactTypeFlag))
                {
                    result.Add(colContactTypeFlag, DropDownListBindUtil.GetTypeFlagTextByValue(dataRow[colContactTypeFlag].ToString()));
                }

                if (visibleColumns.Contains(colContactPermission))
                {
                    result.Add(colContactPermission, DropDownListBindUtil.GetContactPermissionTextByValue(dataRow[colContactPermission].ToString(), ModuleName));
                }
            }

            return result;
        }

        /// <summary>
        /// Is only search out one record
        /// </summary>
        /// <param name="capList">DataTable capList</param>
        /// <returns>true or false</returns>
        private bool IsAutoSkip2CapDetail(DataTable capList)
        {
            var isOnlyOneRecord = false;

            if (capList != null && capList.Rows != null && capList.Rows.Count == 1)
            {
                DataRow row = capList.Rows[0];

                if (dgvPermitList.CheckPermissionToRecordDetail(row))
                {
                    isOnlyOneRecord = true;

                    if (QueryInfo != null)
                    {
                        QueryInfo.PermitList = null;
                        QueryInfo.ComplexPermitList = null;
                        QueryInfo.IsBackFromCapDetailByAutoSkip = true;
                    }

                    string url = dgvPermitList.ConstructRecordDetailUrl(row);
                    Response.Redirect(url);
                }
            }

            return isOnlyOneRecord;
        }

        #endregion Private Methods
    }
}