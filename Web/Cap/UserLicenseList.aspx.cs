#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: UserLicenseList.aspx.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2007-2014
 *
 *  Description: Select a date for schedule an inspection
 *
 *  Notes:
 *      $Id: UserLicenseList.aspx.cs 277932 2014-08-22 10:29:52Z ACHIEVO\james.shi $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using Accela.ACA.BLL.Common;
using Accela.ACA.BLL.People;
using Accela.ACA.Common;
using Accela.ACA.Common.Log;
using Accela.ACA.Common.Util;
using Accela.ACA.Web.Common;
using Accela.ACA.Web.Common.Control;
using Accela.ACA.WSProxy;
using Accela.Web.Controls;
using log4net;

namespace Accela.ACA.Web.Cap
{
    /// <summary>
    /// the class for userLicenseList.
    /// </summary>
    public partial class UserLicenseList : BasePage
    {
        #region Fields

        /// <summary>
        /// selected license model.
        /// </summary>
        private const string SELECTED_LICENSE_MODEL = "SELECTED_LICENSE_MODEL";

        /// <summary>
        /// trade name list.
        /// </summary>
        private const string TRADE_NAME_LIST = "TRADE_NAME_LIST";

        /// <summary>
        /// Loggers object.
        /// </summary>
        private static readonly ILog Logger = LogFactory.Instance.GetLogger(typeof(UserLicenseList));

        #endregion Fields

        #region Properties

        /// <summary>
        /// Gets or sets license model.
        /// </summary>
        public LicenseModel4WS SelectedLicenseModel
        {
            get
            {
                if (ViewState[SELECTED_LICENSE_MODEL] == null)
                {
                    return null;
                }

                return (LicenseModel4WS)ViewState[SELECTED_LICENSE_MODEL];
            }

            set
            {
                ViewState[SELECTED_LICENSE_MODEL] = value;
            }
        }

        /// <summary>
        /// Gets or sets trade name  model list.
        /// </summary>
        private LicenseModel4WS[] TradeNameList
        {
            get
            {
                if (ViewState[TRADE_NAME_LIST] == null)
                {
                    ILicenseProfessionalBll lpBll = ObjectFactory.GetObject<ILicenseProfessionalBll>();
                    ViewState[TRADE_NAME_LIST] = lpBll.GetTradeNameList(ModuleName);
                }

                return (LicenseModel4WS[])ViewState[TRADE_NAME_LIST];
            }

            set
            {
                ViewState[TRADE_NAME_LIST] = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether proxy user has the delegated create permission.
        /// </summary>
        private bool HasDelegatedCreatePermission
        {
            get
            {
                if (ViewState["DelegatedCreatePermission"] == null)
                {
                    return false;
                }

                return (bool)ViewState["DelegatedCreatePermission"];
            }

            set
            {
                ViewState["DelegatedCreatePermission"] = value;
            }
        }

        #endregion Properties

        #region Methods

        /// <summary>
        /// On initial event method
        /// </summary>
        /// <param name="e">A System.EventArgs object containing the event data.</param>
        protected override void OnInit(EventArgs e)
        {
            GridViewBuildHelper.SetSimpleViewElements(gdvTradeNameList, ModuleName, AppSession.IsAdmin);
            base.OnInit(e);
        }

        /// <summary>
        /// Raises the page load event
        /// </summary>
        /// <param name="sender">An object that contains the event sender.</param>
        /// <param name="e">A System.EventArgs object containing the event data.</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (!AppSession.IsAdmin)
                {
                    BindInitUserList();
                }
                else
                {
                    //BindUserType();
                    rdMySelf.AutoPostBack = false;
                    rdInitUsers.AutoPostBack = false;
                    divDelegateSection.Visible = true;
                }

                btnValidationLicense.AccessKey = AccessibilityUtil.GetAccessKey(AccessKeyType.SubmitForm);
            }
            else
            {
                MessageUtil.HideMessageByControl(Page);
            }

            string filterName = HttpUtility.UrlEncode(Request.QueryString["FilterName"]);
            string createdBy = "&createdBy=" + ACAConstant.PUBLIC_USER_NAME + AppSession.User.UserSeqNum;
            bool isDeepLinkMultipleRecordsCreation = !string.IsNullOrWhiteSpace(Request.QueryString[UrlConstant.DEEPLINK_MULTIPLE_SERVICES_DATAKEY]);

            if (rdInitUsers.Checked && !string.IsNullOrEmpty(ddlInitUserList.SelectedValue))
            {
                createdBy = "&createdBy=" + ACAConstant.PUBLIC_USER_NAME + ddlInitUserList.SelectedValue;
            }

            if (StandardChoiceUtil.IsSuperAgency() && !CloneRecordUtil.IsCloneRecord(Request) && !isDeepLinkMultipleRecordsCreation)
            {
                string postUrl = "WorkLocation.aspx?Module=" + ModuleName + "&FilterName=" + filterName + createdBy;
                if (!string.IsNullOrEmpty(Request.QueryString[UrlConstant.CAPTYPE]))
                {
                    postUrl += ACAConstant.AMPERSAND + UrlConstant.CAPTYPE + ACAConstant.EQUAL_MARK + Request.QueryString[UrlConstant.CAPTYPE];
                }

                if (ValidationUtil.IsYes(Request.QueryString["createRecordByService"]))
                {
                    postUrl += "&createRecordByService=" + ACAConstant.COMMON_YES;
                }

                if (!HasDelegatedCreatePermission)
                {
                    Response.Redirect(postUrl);
                }

                btnContinueToConfirm.PostBackUrl = postUrl;
                return;
            }

            if (!CloneRecordUtil.IsCloneRecord(Request))
            {
                // UserLicenseList don't need cap model session, so here need to clear session
                AppSession.SetCapModelToSession(ModuleName, null);
            }

            if (!isDeepLinkMultipleRecordsCreation)
            {
                //Cleare the selected service if current request is not came from multiple records creation deep link.
                AppSession.SetSelectedServicesToSession(null);
            }

            if (AppSession.IsAdmin)
            {
                ILicenseProfessionalBll lpBll = ObjectFactory.GetObject(typeof(ILicenseProfessionalBll)) as ILicenseProfessionalBll;

                //in admin page,show the trade name list header to support change.
                divTradeNameList.Visible = true;
                gdvTradeNameList.DataSource = lpBll.ConstructTradeNameDataTable();
                gdvTradeNameList.DataBind();

                return;
            }

            // if this page from feeEstimator,temporary save the from page to viewState
            int currentStep = 1;
            int currentPage = 1;

            //string from = Request.QueryString["isFeeEstimator"] == null ? string.Empty : Request.QueryString["isFeeEstimator"].ToUpper();
            string from = Request.QueryString["isFeeEstimator"] == null ? string.Empty : Request.QueryString["isFeeEstimator"].ToUpperInvariant();
            string url = "CapType.aspx?Module=" + ModuleName + "&stepNumber={0}&pageNumber={1}&isFeeEstimator=" + from + createdBy;

            if (CloneRecordUtil.IsCloneRecord(Request))
            {
                url += ACAConstant.AMPERSAND + ACAConstant.IS_CLONE_RECORD + ACAConstant.EQUAL_MARK + ACAConstant.COMMON_TRUE;
            }

            if (ValidationUtil.IsYes(Request.QueryString[ACAConstant.IS_SUBAGENCY_CAP]))
            {
                url += ACAConstant.AMPERSAND + ACAConstant.IS_SUBAGENCY_CAP + ACAConstant.EQUAL_MARK + ACAConstant.COMMON_Y;
            }

            if (ValidationUtil.IsYes(Request.QueryString["createRecordByService"]))
            {
                url += "&createRecordByService=" + ACAConstant.COMMON_YES;
            }

            if (isDeepLinkMultipleRecordsCreation)
            {
                //Keep this flag to CapType.aspx page because CapType page will call back to this page to get the selected license.
                url += "&" + UrlConstant.DEEPLINK_MULTIPLE_SERVICES_DATAKEY + "=" + Request.QueryString[UrlConstant.DEEPLINK_MULTIPLE_SERVICES_DATAKEY];
            }

            if (!string.IsNullOrEmpty(Request.QueryString[UrlConstant.AgencyCode]))
            {
                url += "&" + UrlConstant.AgencyCode + "=" + Request.QueryString[UrlConstant.AgencyCode];
            }

            if (!string.IsNullOrEmpty(Request.QueryString[UrlConstant.PAGEFLOW_GROUP_CODE]))
            {
                url += "&" + UrlConstant.PAGEFLOW_GROUP_CODE + "=" + Server.UrlEncode(Request.QueryString[UrlConstant.PAGEFLOW_GROUP_CODE]);
            }

            string reqCAPType = Request.QueryString[UrlConstant.CAPTYPE] ?? string.Empty;
            string reqModuleName = Request.QueryString["Module"] ?? string.Empty;

            if (!string.IsNullOrEmpty(reqCAPType) && !string.IsNullOrEmpty(reqModuleName))
            {
                url += ACAConstant.AMPERSAND + UrlConstant.CAPTYPE + ACAConstant.EQUAL_MARK + HttpUtility.UrlEncode(reqCAPType);
            } 

            if (filterName == ACAConstant.REQUEST_PARMETER_TRADE_NAME)
            {
                //redirect to create trade name
                Response.Redirect(string.Format("CapType.aspx?Module={0}&stepNumber={1}&pageNumber={2}&FilterName={3}&TabName={4}", ModuleName, currentStep, currentPage, Request["FilterName"], Request["TabName"]), true);
            }
            else if (filterName == ACAConstant.REQUEST_PARMETER_TRADE_LICENSE)
            {
                divLicenseSelector.Visible = false;
                divValidationLicense.Visible = false;
                divTradeNameList.Visible = true;
                BindTradeNamelist();
                return;
            }
            else
            {
                if (!string.IsNullOrEmpty(filterName))
                {
                    url += "&FilterName=" + filterName;
                }
            }

            LicenseModel4WS[] licenseList = AppSession.User.UserModel4WS.licenseModel;

            // if there is no license for the account, don't display license selection page.
            if ((licenseList == null || licenseList.Length == 0) && !HasDelegatedCreatePermission)
            {
                Response.Redirect(string.Format(url + "&TabName=" + Request["TabName"], currentStep, currentPage), true);
            }

            url = string.Format(url, currentStep, currentPage + 1);
            btnContinueToConfirm.PostBackUrl = url;

            if (!IsPostBack)
            {
                // Bind License List 
                if (divDelegateSection.Visible)
                {
                    InitUserChanged();
                }
                else
                {
                    DropDownListBindUtil.BindValidLicense(licenseList, ddlLicenseID, true);
                }
            }

            btnValidationLicense.LabelKey = CapUtil.GetContinueButtonLabelKey(this.ModuleName, from);
        }

        /// <summary>
        /// Validation License.
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void ValidationLicense(object sender, EventArgs e)
        {
            try
            {
                bool isSuperAgency = StandardChoiceUtil.IsSuperAgency();

                if (isSuperAgency)
                {
                    ForWardToNextPage();
                    return;
                }

                // validate External license professional
                EMSEResultBaseModel4WS resultModel = EmseUtil.RunEMSEValidationLicense(ACAConstant.EMSE_SELECT_LICENSE_VALIDATION, AppSession.User.UserID, this.SelectedLicenseModel);
                
                // whether display error message to page
                if (resultModel != null && resultModel.returnCode == EmseUtil.RETURN_CODE_FOR_DISPLAY_MESSAGE)
                {
                    string returnMessage = resultModel.returnMessage;
                    MessageUtil.ShowMessage(this.Page, MessageType.Error, returnMessage);
                    return;
                }
                else
                {
                    ForWardToNextPage();
                }
            }
            catch (ACAException ex)
            {
                Logger.Error(ex);
                string msg = ex.Message;
                MessageUtil.ShowMessage(this.Page, MessageType.Error, msg);
            }
        }

        /// <summary>
        /// LicenseID dropdown SelectedIndexChanged
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void LicenseIDDropDown_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.SelectLicense();
            Page.FocusElement(ddlLicenseID.ClientID);
        }

        /// <summary>
        /// User type selected index changed.
        /// </summary> 
        /// <param name="sender">the sender object</param>
        /// <param name="e">the event handle.</param>
        protected void UserType_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            this.licenseInfo.Visible = false;
            this.SelectedLicenseModel = null;
            string radioId = ((AccelaRadioButton)sender).ID;

            if (radioId == "rdMySelf")
            {
                rdInitUsers.Checked = false;
                ddlInitUserList.SelectedIndex = 0;
                ddlInitUserList.Enabled = false;
                DropDownListBindUtil.BindValidLicense(AppSession.User.UserModel4WS.licenseModel, ddlLicenseID, true);
                InitLicenseSection(AppSession.User.UserModel4WS.licenseModel);
                Page.FocusElement(rdMySelf.ClientID);
            }
            else
            {
                rdMySelf.Checked = false;
                DropDownListBindUtil.BindDDL(null, ddlLicenseID, true);
                ddlInitUserList.SetAvailableItemSelected();
                InitUserChanged();
                ddlInitUserList.Enabled = true;
                Page.FocusElement(rdInitUsers.ClientID);
            }
        }

        /// <summary>
        /// TradeNameList RowDataBound
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void TradeNameList_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                AccelaLabel lblTradeNumber = (AccelaLabel)e.Row.FindControl("lblNumber");
                AccelaLinkButton btnRequestLicense = (AccelaLinkButton)e.Row.FindControl("btnRequestLicense");
                AccelaLabel lblType = (AccelaLabel)e.Row.FindControl("lblType");
                
                DataRowView rowView = (DataRowView)e.Row.DataItem;
                bool isTNExpired = (bool)rowView["IsTNExpired"];
                string appStatusGroup = rowView["AppStatusGroup"] as string;
                string appStatus = rowView["AppStatus"] as string;

                if (!isTNExpired
                    && LicenseUtil.IsDisplayRequestTradeLicenseLink(ConfigManager.AgencyCode, ModuleName, appStatusGroup, appStatus))
                {
                    btnRequestLicense.Visible = true;
                    btnRequestLicense.Text = GetTextByKey("per_tradeName_msg_requestTradeLicense");

                    string url = string.Format(
                                            "CapType.aspx?Module={0}&stepNumber=1&pageNumber=2&filterName={1}&TabName={2}&licenseNumber={3}&licenseType={4}",
                                            ModuleName,
                                            Request["filterName"],
                                            Request["TabName"],
                                            lblTradeNumber.Text,
                                            lblType.Text);
                    btnRequestLicense.PostBackUrl = url;
                    btnRequestLicense.CommandArgument = lblTradeNumber.Text;
                }
            }
        }

        /// <summary>
        /// User type selected index changed.
        /// </summary> 
        /// <param name="sender">the sender object.</param>
        /// <param name="e">the event handle.</param>
        protected void InitUserListDropDown_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            InitUserChanged();
            Page.FocusElement(ddlInitUserList.ClientID);
        }

        /// <summary>
        /// Format license Model for Basic
        /// </summary>
        /// <param name="licenseModel">license model</param>
        /// <returns>string format.</returns>
        private static string FormatlicenseModelModel4Basic(LicenseModel4WS licenseModel)
        {
            if (licenseModel != null)
            {
                StringBuilder buf = new StringBuilder();
                if (!string.IsNullOrEmpty(licenseModel.contactFirstName))
                {
                    buf.Append("<strong>");
                    buf.Append(licenseModel.contactFirstName);
                    buf.Append(" ");
                    buf.Append("</strong>");
                }

                if (!string.IsNullOrEmpty(licenseModel.contactLastName))
                {
                    buf.Append("<strong>");
                    buf.Append(licenseModel.contactLastName);
                    buf.Append("</strong>");
                }

                if (!string.IsNullOrEmpty(licenseModel.businessName))
                {
                    buf.Append(ACAConstant.HTML_BR);
                    buf.Append(licenseModel.businessName);
                }

                if (!string.IsNullOrEmpty(licenseModel.busName2))
                {
                    buf.Append(ACAConstant.SLASH);
                    buf.Append(licenseModel.busName2);
                }

                if (!string.IsNullOrEmpty(licenseModel.address1))
                {
                    buf.Append(ACAConstant.HTML_BR);
                    buf.Append(licenseModel.address1);
                }

                if (!string.IsNullOrEmpty(licenseModel.address2))
                {
                    buf.Append(ACAConstant.HTML_BR);
                    buf.Append(licenseModel.address2);
                }

                if (!string.IsNullOrEmpty(licenseModel.address3))
                {
                    buf.Append(ACAConstant.HTML_BR);
                    buf.Append(licenseModel.address3);
                }

                if (!string.IsNullOrEmpty(licenseModel.city))
                {
                    buf.Append(ACAConstant.HTML_BR);
                    buf.Append(licenseModel.city);
                    buf.Append(", ");
                }

                if (!string.IsNullOrEmpty(licenseModel.state))
                {
                    buf.Append(I18nUtil.DisplayStateForI18N(licenseModel.state, licenseModel.countryCode));
                    buf.Append(" ");
                }

                if (!string.IsNullOrEmpty(licenseModel.zip))
                {
                    buf.Append(ModelUIFormat.FormatZipShow(licenseModel.zip, licenseModel.countryCode));
                }

                if (!string.IsNullOrEmpty(licenseModel.countryCode))
                {
                    buf.Append(ACAConstant.HTML_BR);
                    string country = StandardChoiceUtil.GetCountryByKey(licenseModel.countryCode);
                    buf.Append(country);
                }

                return buf.ToString();
            }

            return string.Empty;
        }

        /// <summary>
        /// Format license Model for Ext
        /// </summary>
        /// <param name="licenseModel">license model.</param>
        /// <returns>string format.</returns>
        private static string FormatlicenseModelModel4Ext(LicenseModel4WS licenseModel)
        {
            if (licenseModel == null)
            {
                return string.Empty;
            }

            StringBuilder buf = new StringBuilder();

            buf.Append("<div>");
            buf.Append(I18nStringUtil.FormatToTableRow(BasePage.GetStaticTextByKey("per_selectLicense_label_txtAppPhone1"), ModelUIFormat.FormatPhoneShow(licenseModel.phone1CountryCode, licenseModel.phone1, licenseModel.countryCode)));
            buf.Append("</div>");
            buf.Append("<div>");
            buf.Append(I18nStringUtil.FormatToTableRow(BasePage.GetStaticTextByKey("per_selectLicense_label_txtAppPhone2"), ModelUIFormat.FormatPhoneShow(licenseModel.phone2CountryCode, licenseModel.phone2, licenseModel.countryCode)));
            buf.Append("</div>");
            buf.Append("<div>");
            buf.Append(I18nStringUtil.FormatToTableRow(BasePage.GetStaticTextByKey("per_selectLicense_label_txtAppFax"), ModelUIFormat.FormatPhoneShow(licenseModel.faxCountryCode, licenseModel.fax, licenseModel.countryCode)));
            buf.Append("</div>");

            if (!string.IsNullOrEmpty(licenseModel.licenseType))
            {
                buf.Append("<div>");
                string[] licenseInfo = new string[2];
                licenseInfo[0] = StandardChoiceUtil.IsDisplayLicenseState() ? I18nUtil.DisplayStateForI18N(licenseModel.licState, licenseModel.countryCode) : string.Empty;
                licenseInfo[1] = I18nStringUtil.GetString(licenseModel.resLicenseType, licenseModel.licenseType);

                string infoWithBlank = DataUtil.ConcatStringWithSplitChar(licenseInfo, ACAConstant.BLANK);
                buf.Append(I18nStringUtil.FormatToTableRow(infoWithBlank));
                buf.Append("</div>");
            }

            if (!string.IsNullOrEmpty(licenseModel.stateLicense))
            {
                buf.Append("<div>");
                string[] licenseInfo = new string[2];
                licenseInfo[0] = StandardChoiceUtil.IsDisplayLicenseState() ? I18nUtil.DisplayStateForI18N(licenseModel.licState, licenseModel.countryCode) : string.Empty;
                licenseInfo[1] = licenseModel.stateLicense;

                string infoWithSplit = DataUtil.ConcatStringWithSplitChar(licenseInfo, ACAConstant.SPLIT_CHAR4);
                buf.Append(I18nStringUtil.FormatToTableRow(infoWithSplit));
                buf.Append("</div>");
            }

            if (licenseModel.contrLicNo != null)
            {
                buf.Append("<div>");
                buf.Append(I18nStringUtil.FormatToTableRow(BasePage.GetStaticTextByKey("per_selectlicense_label_contractorlicno"), licenseModel.contrLicNo.ToString()));
                buf.Append("</div>");
            }

            if (!string.IsNullOrEmpty(licenseModel.contLicBusName))
            {
                buf.Append("<div>");
                buf.Append(I18nStringUtil.FormatToTableRow(BasePage.GetStaticTextByKey("per_selectlicense_label_contractorbusiname"), licenseModel.contLicBusName));
                buf.Append("</div>");
            }

            return buf.ToString();
        }

        /// <summary>
        /// initial user changed.
        /// </summary>
        private void InitUserChanged()
        {
            if (StandardChoiceUtil.IsSuperAgency() && !CloneRecordUtil.IsCloneRecord(Request))
            {
                return;
            }

            if (!string.IsNullOrEmpty(ddlInitUserList.SelectedValue) && AppSession.User.UserModel4WS != null
                    && AppSession.User.UserModel4WS.initialUsers != null && AppSession.User.UserModel4WS.initialUsers.Length > 0)
            {
                PublicUserModel4WS user = AppSession.User.UserModel4WS.initialUsers.Where(p => p.userSeqNum == ddlInitUserList.SelectedValue).Single();

                if (user != null)
                {
                    LicenseModel4WS[] licenseList = user.licenseModel;
                    DropDownListBindUtil.BindValidLicense(licenseList, ddlLicenseID, true);
                    InitLicenseSection(licenseList);
                }
            }
            else
            {
                DropDownListBindUtil.BindDDL(null, ddlLicenseID);
            }
        }

        /// <summary>
        /// Forward to next page.
        /// </summary>
        private void ForWardToNextPage()
        {
            string initialUser = string.Empty;

            if (rdInitUsers.Checked && rdInitUsers.Visible)
            {
                if (string.IsNullOrEmpty(ddlInitUserList.SelectedValue))
                {
                    MessageUtil.ShowMessage(this.Page, MessageType.Error, BasePage.GetStaticTextByKey("proxy_select_user_name"));
                    return;
                }

                initialUser = ddlInitUserList.SelectedValue;
            }

            if (!CheckPermissionFromDeepLink(initialUser))
            {
                MessageUtil.ShowMessage(Page, MessageType.Error, GetTextByKey("aca_userlicenselist_msg_service_access_denied"));
                return;
            }

            if (CloneRecordUtil.IsCloneRecord(Request))
            {
                CapModel4WS cap = AppSession.GetCapModelFromSession(ModuleName);

                if (!LicenseUtil.IsAvailableLicense(SelectedLicenseModel, cap.capType))
                {
                    MessageUtil.ShowMessage(Page, MessageType.Error, GetTextByKey("per_applypermit_selecttype_error_unavailablelicense"));
                    return;
                }
            }
            
            // trigger the btnContinueToConfirm button
            string triggerContinueScript = string.Format("var button = $get('{0}');", btnContinueToConfirm.ClientID);
            triggerContinueScript +=
                @"
                    if(document.all){button.click();}
                    else
                    {
                        var evt = document.createEvent('MouseEvents');
                        evt.initMouseEvent('click',true,true,document.defaultView,1,0,0,0,0,false,false,false,false,0,null);
                        button.dispatchEvent(evt);
                    }";
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "forwardToNextPage", triggerContinueScript, true);
        }

        /// <summary>
        /// Check permission that come from deep link.
        /// </summary>
        /// <param name="initialUser">The initial user.</param>
        /// <returns>Return true if have permission.</returns>
        private bool CheckPermissionFromDeepLink(string initialUser)
        {
            bool isDeepLinkMultipleRecordsCreation = !string.IsNullOrWhiteSpace(Request.QueryString[UrlConstant.DEEPLINK_MULTIPLE_SERVICES_DATAKEY]);

            // return ture if not from deep link
            if (!isDeepLinkMultipleRecordsCreation)
            {
                return true;
            }

            IServiceManagementBll serviceManagementBll = ObjectFactory.GetObject<IServiceManagementBll>();
            ServiceModel[] selectedServices = AppSession.GetSelectedServicesFromSession();

            if (selectedServices == null)
            {
                return false;
            }

            XServiceGroupModel[] serviceGroups = serviceManagementBll.GetServices(null, false, AppSession.User.UserSeqNum, initialUser);
            ServiceModel[] allServices = null;

            if (serviceGroups != null && serviceGroups.Length > 0)
            {
                allServices = serviceGroups.Select(s => s.service).ToArray();
            }

            if (allServices == null)
            {
                return false;
            }

            foreach (ServiceModel selectedItem in selectedServices)
            {
                bool isContains = allServices.Contains(selectedItem, new ServiceModel.Comparer());

                if (!isContains)
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Bind Trade Name list.
        /// </summary>
        private void BindTradeNamelist()
        {
            if (TradeNameList == null || TradeNameList.Length == 0)
            {
                // show error message
                divInstruction.Visible = false;
                MessageUtil.ShowMessage(Page, MessageType.Error, GetTextByKey("per_tradeName_msg_noTradeName"));

                return;
            }
            else if (TradeNameList.Length == 1)
            {
                //SelectedLicenseModel=TradeNameList[0];
                string licenseNumber = TradeNameList[0].stateLicense;
                string licenseType = TradeNameList[0].licenseType;

                string url = string.Format("CapType.aspx?Module={0}&stepNumber=1&pageNumber=1&filterName={1}&TabName={2}&licenseNumber={3}&licenseType={4}", ModuleName, Request["filterName"], Request["TabName"], licenseNumber, licenseType);
                Response.Redirect(url);
            }
            else
            {
                gdvTradeNameList.DataSource = CreateDataTable(TradeNameList);
                gdvTradeNameList.DataBind();
            }
        }

        /// <summary>
        /// Create DataTable
        /// </summary>
        /// <param name="licenseList">license model array.</param>
        /// <returns>data table for license model array.</returns>
        private DataTable CreateDataTable(LicenseModel4WS[] licenseList)
        {
            DataTable dtTradName = new DataTable();
            dtTradName.Columns.Add("TradeNumber", typeof(string));
            dtTradName.Columns.Add("EnglishName", typeof(string));
            dtTradName.Columns.Add("Name2", typeof(string));
            dtTradName.Columns.Add("Status", typeof(string));
            dtTradName.Columns.Add("RelatedTradeLicense", typeof(string));
            dtTradName.Columns.Add("Type", typeof(string));
            dtTradName.Columns.Add("IsTNExpired", typeof(bool));
            dtTradName.Columns.Add("AppStatusGroup", typeof(string));
            dtTradName.Columns.Add("AppStatus", typeof(string));

            // add rows to datatable
            foreach (LicenseModel4WS tradeName in licenseList)
            {
                string licenseStatus = I18nStringUtil.GetString(tradeName.resLicenseStatus, tradeName.licenseStatus);

                DataRow dr = dtTradName.NewRow();
                dr[0] = tradeName.stateLicense;
                dr[1] = tradeName.businessName;
                dr[2] = tradeName.busName2;
                dr[3] = licenseStatus;
                dr[4] = tradeName.relatedTradeLic;
                dr[5] = tradeName.licenseType;
                dr[6] = tradeName.isTNExpired;
                dr["AppStatusGroup"] = tradeName.statusGroupCode;
                dr["AppStatus"] = tradeName.licenseStatus;

                dtTradName.Rows.Add(dr);
            }

            return dtTradName;
        }

        /// <summary>
        /// Display license.
        /// </summary>
        /// <param name="license">license model</param>
        private void DisplayLicense(LicenseModel4WS license)
        {
            lblLicenseBasic.Text = FormatlicenseModelModel4Basic(license);
            lblLicenseExt.Text = FormatlicenseModelModel4Ext(license);
        }

        /// <summary>
        /// select license.
        /// </summary>
        private void SelectLicense()
        {
            LicenseModel4WS selectedLicense = null;

            List<LicenseModel4WS> licenseList = new List<LicenseModel4WS>();

            if (rdMySelf.Checked || !rdMySelf.Visible)
            {
                licenseList = AppSession.User.UserModel4WS.licenseModel.ToList();
            }
            else if (AppSession.User.UserModel4WS.initialUsers != null && AppSession.User.UserModel4WS.initialUsers.Length > 0)
            {
                PublicUserModel4WS user = AppSession.User.UserModel4WS.initialUsers.Where(p => p.userSeqNum == ddlInitUserList.SelectedValue).Single();

                if (user != null)
                {
                    licenseList = user.licenseModel.ToList();
                }
            }

            if (licenseList != null && licenseList.Count > 0)
            {
                foreach (LicenseModel4WS license in licenseList)
                {
                    if (ddlLicenseID.SelectedValue == license.licSeqNbr)
                    {
                        selectedLicense = license;
                        break;
                    }
                }
            }

            if (selectedLicense == null)
            {
                this.licenseInfo.Visible = false;

                //If user selected "None Applicable", assign an empty model to avoid the license copy.
                this.SelectedLicenseModel = new LicenseModel4WS();
            }
            else
            {
                this.SelectedLicenseModel = selectedLicense;
                this.DisplayLicense(selectedLicense);
                this.licenseInfo.Visible = true;
            }

            CheckLicense(selectedLicense);
        }

        /// <summary>
        /// Check if the selected license is expired or not. Display a message if it's expired.
        /// </summary>
        /// <param name="license">the user selected license</param>
        private void CheckLicense(LicenseModel4WS license)
        {
            // license is expired.
            if (LicenseUtil.IsExpiredLicense(license))
            {
                MessageUtil.ShowMessageByControl(Page, MessageType.Notice, GetTextByKey("per_applypermit_selectlicense_message_expiredlicense"));
            }
            else
            {
                MessageUtil.HideMessageByControl(Page);
            }
        }

        /// <summary>
        /// Bind initial User List.
        /// </summary> 
        private void BindInitUserList()
        {
            if (!StandardChoiceUtil.IsEnableProxyUser())
            {
                return;
            }

            PublicUserModel4WS[] initUsers = AppSession.User.UserModel4WS.initialUsers;
            List<ListItem> items = new List<ListItem>();

            if (initUsers != null && initUsers.Length > 0)
            {
                foreach (PublicUserModel4WS initUser in initUsers)
                {
                    /*
                     * If public user does not associated with any contact or the contact association is not approved,
                     *  do not display the proxy user.
                     */

                    if (initUser.peopleModel == null || initUser.peopleModel.Length == 0)
                    {
                        continue;
                    }

                    if (initUser.peopleModel.Count(p =>
                            ContractorPeopleStatus.Approved.Equals(p.contractorPeopleStatus, StringComparison.OrdinalIgnoreCase)
                            || string.IsNullOrEmpty(p.contractorPeopleStatus)) == 0)
                    {
                        continue;
                    }

                    if (initUser.proxyUserModel.XProxyUserPermissionModels == null || initUser.proxyUserModel.XProxyUserPermissionModels.Length == 0)
                    {
                        continue;
                    }

                    XProxyUserPermissionModel[] delegatePermissions = initUser.proxyUserModel.XProxyUserPermissionModels.Where(p => p.levelType == ACAConstant.LEVEL_TYPE_MODULE && p.levelData == ModuleName).ToArray();

                    if (delegatePermissions == null || delegatePermissions.Length < 1)
                    {
                        continue;
                    }

                    string role = delegatePermissions[0].permission;

                    var proxyUserRoleBll = ObjectFactory.GetObject<IProxyUserRoleBll>();
                    ProxyUserRolePrivilegeModel4WS rolePrivilege = proxyUserRoleBll.ConvertToUserRolePrivilegeModel(role);

                    if (rolePrivilege.createApplicationAllowed)
                    {
                        IPeopleBll peopleBll = (IPeopleBll)ObjectFactory.GetObject(typeof(IPeopleBll));
                        string currentUserName = peopleBll.GetContactUserName(initUser);

                        items.Add(new ListItem(currentUserName, initUser.userSeqNum));
                        HasDelegatedCreatePermission = true;
                    }
                }
            }

            if (HasDelegatedCreatePermission)
            {
                divDelegateSection.Visible = true;
                InitLicenseSection(null);
                DropDownListBindUtil.BindDDL(items, ddlInitUserList);
                ddlInitUserList.SetAvailableItemSelected();
                InitUserChanged();
            }
            else
            {
                InitLicenseSection(AppSession.User.UserModel4WS.licenseModel);
            }
        }

        /// <summary>
        /// Initial License section.
        /// </summary>
        /// <param name="licenseList">the license list.</param>
        private void InitLicenseSection(LicenseModel4WS[] licenseList)
        {
            if (licenseList == null || licenseList.Length == 0 || (StandardChoiceUtil.IsSuperAgency() && !CloneRecordUtil.IsCloneRecord(Request)))
            {
                divInstruction.Visible = false;
                LicensePanel.Visible = false;
            }
            else
            {
                divInstruction.Visible = true;
                LicensePanel.Visible = true;
            }
        }

        #endregion Methods
    }
}
