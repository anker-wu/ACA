#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: ServiceControl.ascx.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2012-2014
 *
 *  Description:
 *
 *  Notes:
 *      $Id: ServiceControl.ascx.cs 215305 2012-03-08 09:02:11Z ACHIEVO\alan.hu $.
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
using System.Web.UI;
using System.Web.UI.WebControls;

using Accela.ACA.BLL.APO;
using Accela.ACA.BLL.Cap;
using Accela.ACA.BLL.Common;
using Accela.ACA.Common;
using Accela.ACA.Common.Common;
using Accela.ACA.Common.Util;
using Accela.ACA.Web.Common;
using Accela.ACA.WSProxy;
using Accela.Web.Controls;

namespace Accela.ACA.Web.Component
{
    /// <summary>
    /// THe service control.
    /// </summary>
    public partial class ServiceControl : BaseUserControl
    {
        /// <summary>
        /// Flag indicating a service is expired and unavailable.
        /// </summary>
        private const string UNAVAILABLE_SERVICE = "UNAVAILABLE_SERVICE";

        /// <summary>
        /// Flag indicating a service is expired.
        /// </summary>
        private const string EXPIRED_SERVICE = "EXPIRED_SERVICE";

        /// <summary>
        /// Service list.
        /// </summary>
        private const string SERVICE_LIST = "SERVICE_LIST";

        /// <summary>
        /// Original service list for search
        /// </summary>
        private const string ORIGINAL_SERVICE_LIST = "ORIGINAL_SERVICE_LIST";

        /// <summary>
        /// Address list's owner column name in work location page.
        /// </summary>
        private const string ADDRESS_LIST_OWNER_COLUMN_NAME = "lnkOwnerHeader";

        /// <summary>
        /// Address list's parcel column name in work location page.
        /// </summary>
        private const string ADDRESS_LIST_PARCEL_COLUMN_NAME = "lnkParcelHeader";

        /// <summary>
        /// Address list view id in work location page.
        /// </summary>
        private const string ADDRESS_LIST_IN_WORKLOCATION_VIEWID = "60108";

        /// <summary>
        /// Select address.
        /// </summary>
        private const string SELECTED_ADDRESS = "SELECTED_ADDRESS";

        /// <summary>
        /// The service list display
        /// </summary>
        private bool _displayOfServiceitemlist = false;

        /// <summary>
        /// Indicating whether there is only allow single service selection.
        /// </summary>
        private bool? _isSingleService;

        /// <summary>
        /// String list to store the number of expired licenses.
        /// </summary>
        private IList<string> _expiredLicNumList;

        /// <summary>
        /// String list to store the number of unavailable licenses.
        /// </summary>
        private IList<string> _unAvailableLicNumList;

        /// <summary>
        /// Gets or sets a value indicating whether for location.
        /// </summary>
        public bool IsForLocation
        {
            get
            {
                if (ViewState["IsForLocation"] == null)
                {
                    return true;
                }

                return (bool)ViewState["IsForLocation"];
            }

            set
            {
                ViewState["IsForLocation"] = value;
            }
        }        

        /// <summary>
        /// Gets or sets zip code.
        /// </summary>
        public string ZipCode
        {
            get
            {
                if (ViewState["ZipCode"] == null)
                {
                    return string.Empty;
                }

                return ViewState["ZipCode"] as string;
            }

            set
            {
                ViewState["ZipCode"] = value;
            }
        }

        /// <summary>
        /// Gets or sets city.
        /// </summary>
        public string City
        {
            get
            {
                if (ViewState["City"] == null)
                {
                    return string.Empty;
                }

                return ViewState["City"] as string;
            }

            set
            {
                ViewState["City"] = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether there is only allow single service selection.
        /// </summary>
        public bool SingleServiceOnly
        {
            get
            {
                if (_isSingleService == null)
                {
                    IBizDomainBll bizDomainBll = ObjectFactory.GetObject<IBizDomainBll>();
                    _isSingleService = bizDomainBll.IsSingleServiceOnly(this.ModuleName);
                }

                return _isSingleService.Value;
            }

            set
            {
                _isSingleService = value;
            }
        }

        /// <summary>
        /// Gets or sets service list.
        ///     The dictionary key is the service group code or I18n value
        ///     and the dictionary value is the service list which is associated with the service group .
        /// </summary>
        private Dictionary<string, List<ServiceModel>> ServiceList
        {
            get
            {
                if (ViewState[SERVICE_LIST] == null)
                {
                    return null;
                }

                return ViewState[SERVICE_LIST] as Dictionary<string, List<ServiceModel>>;
            }

            set
            {
                ViewState[SERVICE_LIST] = value;
            }
        }

        /// <summary>
        /// Gets or sets the service list for search
        /// </summary>
        private Dictionary<string, List<ServiceModel>> OriginalServiceList
        {
            get
            {
                if (ViewState[ORIGINAL_SERVICE_LIST] == null)
                {
                    return null;
                }

                return ViewState[ORIGINAL_SERVICE_LIST] as Dictionary<string, List<ServiceModel>>;
            }

            set
            {
                ViewState[ORIGINAL_SERVICE_LIST] = value;
            }
        }

        /// <summary>
        /// Gets or sets selected address.
        /// </summary>
        private RefAddressModel SelectedAddress
        {
            get
            {
                if (ViewState[SELECTED_ADDRESS] == null)
                {
                    return null;
                }

                return (RefAddressModel)ViewState[SELECTED_ADDRESS];
            }

            set
            {
                ViewState[SELECTED_ADDRESS] = value;
            }
        }

        /// <summary>
        /// Bind service list
        /// </summary>
        /// <param name="serviceModels">The service model list.</param>
        /// <param name="alwaysShowSearchBar"> Always show the search bar if the caller come from the SearchServiceList function.</param>
        public void BindServiceList(Dictionary<string, List<ServiceModel>> serviceModels, bool alwaysShowSearchBar = false)
        {
            hdnSelectedServices.Value = string.Empty;
            IServiceManagementBll serviceManagementBll = (IServiceManagementBll)ObjectFactory.GetObject(typeof(IServiceManagementBll));

            if (serviceModels == null)
            {
                string initialUserSeqNum = string.Empty;
                string createdBy = Request["createdBy"];

                if (!string.IsNullOrEmpty(createdBy) && createdBy != AppSession.User.PublicUserId)
                {
                    initialUserSeqNum = createdBy.Replace(ACAConstant.PUBLIC_USER_NAME, string.Empty);
                }

                XServiceGroupModel[] serviceGroups = serviceManagementBll.GetServices(SelectedAddress, IsForLocation, AppSession.User.UserSeqNum, initialUserSeqNum);
                ServiceList = GetServPorvCodeList(serviceGroups);
                OriginalServiceList = null;
                txtSearch.Text = string.Empty;
            }
            else
            {
                ServiceList = serviceModels;
            }

            if (AppSession.IsAdmin)
            {
                ServiceList = null;
            }

            string text = GetTextByKey("superAgency_workLocation_label_selectService");
            string appendPrefix = GetTextByKey("superAgency_workLocation_label_serviceFound");

            if (ServiceList != null && ServiceList.Count > 0)
            {
                if (IsForLocation)
                {
                    int serviceCount = 0;

                    foreach (KeyValuePair<string, List<ServiceModel>> item in ServiceList)
                    {
                        serviceCount += item.Value.Count;
                    }

                    lblSelectService.Text = DataUtil.StringFormat(text + "(" + appendPrefix + "):", serviceCount);
                    btnContinue.Visible = true;
                }
                else
                {
                    lblSelectService.Visible = false;
                }

                rptAgency.DataSource = ServiceList;
                rptAgency.DataBind();

                // bind service group listbox
                if (!IsPostBack)
                {
                    Dictionary<string, string> serviceGroups = new Dictionary<string, string>();

                    if (ServiceList != null && ServiceList.Count > 0)
                    {
                        foreach (var service in ServiceList)
                        {
                            serviceGroups.Add(service.Key, ScriptFilter.FilterScript(service.Key));
                        }
                    }

                    cbListServiceGroup.DataSource = serviceGroups;
                    cbListServiceGroup.DataValueField = "Key";
                    cbListServiceGroup.DataTextField = "Value";
                    cbListServiceGroup.DataBind();
                }
                
                divNoResult.Visible = false;
                hasService.Value = "1";
                divServicePan.Visible = true;
                divSearchBar.Visible = true;
                BindScript(true);
            }
            else
            {
                if (IsForLocation)
                {
                    divSearchBar.Visible = false;
                }

                //show official web site if there is no services available for the selected address.
                ShowOfficialSite(false);
                divServicePan.Visible = false;
                BindScript(false);
            }

            if (alwaysShowSearchBar)
            {
                divSearchBar.Visible = true;
            }
        }

        /// <summary>
        /// Get service list when user select one address.
        /// </summary>
        /// <param name="parcelInfo">The parcel info.</param>
        public void BindServiceListByParcel(ParcelInfoModel parcelInfo)
        {
            if (parcelInfo != null && parcelInfo.RAddressModel != null && !parcelInfo.RAddressModel.sourceNumber.HasValue)
            {
                IServiceProviderBll provider = ObjectFactory.GetObject(typeof(IServiceProviderBll)) as IServiceProviderBll;
                ServiceProviderModel service = provider.GetServiceProviderByPK(ConfigManager.AgencyCode, AppSession.User.UserID);
                parcelInfo.RAddressModel.sourceNumber = (int)service.sourceNumber;
            }

            this.SelectedAddress = parcelInfo.RAddressModel;

            IGviewBll gviewBll = (IGviewBll)ObjectFactory.GetObject(typeof(IGviewBll));
            SimpleViewElementModel4WS[] viewModels = gviewBll.GetSimpleViewElementModel(ModuleName, ADDRESS_LIST_IN_WORKLOCATION_VIEWID);
            bool hasParcelOwnerColumns =
                (from vm in viewModels
                 where (ADDRESS_LIST_PARCEL_COLUMN_NAME.Equals(vm.viewElementName) && ACAConstant.VALID_STATUS.Equals(vm.recStatus)) ||
                       (ADDRESS_LIST_OWNER_COLUMN_NAME.Equals(vm.viewElementName) && ACAConstant.VALID_STATUS.Equals(vm.recStatus))
                 select vm).Any();

            if (!hasParcelOwnerColumns)
            {
                AppSession.SelectedParcelInfo = ResetParcelOwnerIntoParcelInfo(parcelInfo, this.SelectedAddress);
            }
            else
            {
                AppSession.SelectedParcelInfo = parcelInfo;
            }

            BindServiceList(null);
        }

        /// <summary>
        /// Show official web site Url
        /// </summary>
        /// <param name="show">True or false.</param>
        public void ShowOfficialSite(bool show)
        {
            if (show)
            {
                divServicePan.Visible = false;
                BindScript(false);
            }

            divNoResult.Visible = true;
            string message = string.Empty;

            if (IsForLocation)
            {
                string officeSite = StandardChoiceUtil.GetOfficialWebSite();

                // if user hasn't configured the office site url, don't need to show the url information
                if (string.IsNullOrEmpty(officeSite))
                {
                    message = GetTextByKey("superAgency_workLocation_label_noAddressResults");
                }
                else
                {
                    message = GetTextByKey("superAgency_workLocation_label_visitOfficialSite"); //show message as no result and visit web site.
                }
            }
            else
            {
                message = GetTextByKey("aca_serviceselection_msg_noservicereturn");
            }

            lblNoResultMsg.Text = message;

            if (AccessibilityUtil.AccessibilityEnabled && !string.IsNullOrEmpty(message))
            {
                MessageUtil.ShowAlertMessage(this, message);
            }
        }

        /// <summary>
        /// The page load method
        /// </summary>
        /// <param name="sender">The sender object</param>
        /// <param name="e">The event handle</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (AppSession.IsAdmin)
                {
                    divServiceList.Visible = false;
                    lblSelectService.Visible = false;
                    divSearchBar.Visible = true;

                    if (IsForLocation)
                    {
                        divForAdminShowService.Visible = true;
                        divNoResult.Visible = true;
                        divForAdminShowResults.Visible = true;
                    }
                }

                if (IsForLocation)
                {
                    divFilter.Visible = false;
                }
            }

            if (IsForLocation && AppSession.IsAdmin)
            {
                divServicePan.Visible = true;
            }

            if (!AppSession.IsAdmin && IsForLocation && !string.IsNullOrEmpty(StandardChoiceUtil.GetOfficialWebSite()))
            {
                webSite.Visible = true;
                webSite.HRef = ScriptFilter.FilterScript(GetOfficialSiteUrlWithHttp());
            }

            if (IsForLocation)
            {
                divSearchBar.Visible = false;

                if (!AppSession.IsAdmin)
                {
                    divServicePan.Visible = false;
                    divNoResult.Visible = false;
                    btnContinue.Visible = false;
                }
            }
        }

        /// <summary>
        /// OnPreRender event method.
        /// </summary>
        /// <param name="e">EventArgs e</param>
        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);

            if (!AppSession.IsAdmin)
            {
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), this.ClientID, "if(typeof(initServiceListStatus)== 'function') {initServiceListStatus('" + _displayOfServiceitemlist + "');}", true);
            }
        }

        /// <summary>
        /// The Continue Button Click
        /// </summary>
        /// <param name="sender">Object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void ContinueButton_Click(object sender, EventArgs e)
        {
            ServiceModel[] selectedServices = GetSelectedServices();

            GotoNextStep(selectedServices);
        }

        /// <summary>
        /// Then Search Button Click
        /// </summary>
        /// <param name="sender">Object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void SearchButton_Click(object sender, EventArgs e)
        {
            string keyWord = txtSearch.Text.Trim();
            List<string> serviceGroups = GetSelectedServiceGroups();

            SearchServiceList(serviceGroups, keyWord);

            if (!string.IsNullOrEmpty(keyWord))
            {
                _displayOfServiceitemlist = true;
            }

            //clear the selected service
            hdnSelectedServices.Value = string.Empty;
            Page.FocusElement(btnSearch.ClientID);
        }

        /// <summary>
        /// Check box list data bound event.
        /// </summary>
        /// <param name="sender">Object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void ListServicesCheckBox_DataBound(object sender, EventArgs e)
        {
            CheckBoxList cbListServices = (CheckBoxList)sender;

            for (int i = 0; i < cbListServices.Items.Count; i++)
            {
                cbListServices.Items[i].Attributes.Add("title", cbListServices.Items[i].Text);
                cbListServices.Items[i].Attributes.Add("AgencyPlusServiceName", cbListServices.Items[i].Value);
                cbListServices.Items[i].Attributes.Add("onclick", "RememberSelectedService(this,'" + btnContinue.ClientID + "','" + hdnSelectedServices.ClientID + "',false);");
            }
        }

        /// <summary>
        /// The radio button list data bound event.
        /// </summary>
        /// <param name="sender">The object sender.</param>
        /// <param name="e">The EventArgs e.</param>
        protected void RbListServices_DataBound(object sender, EventArgs e)
        {
            RadioButtonList rbListServices = (RadioButtonList)sender;

            for (int i = 0; i < rbListServices.Items.Count; i++)
            {
                rbListServices.Items[i].Attributes.Add("title", rbListServices.Items[i].Text);
                rbListServices.Items[i].Attributes.Add("AgencyPlusServiceName", rbListServices.Items[i].Value);
                rbListServices.Items[i].Attributes.Add("onclick", "RememberSelectedService(this,'" + btnContinue.ClientID + "','" + hdnSelectedServices.ClientID + "',true);");
            }            
        }

        /// <summary>
        /// The Repeater for item data bound.
        /// </summary>
        /// <param name="sender">The object sender.</param>
        /// <param name="e">The EventArgs e.</param>
        protected void AgencyRepeater_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            AccelaLabel lblGroupCode = (AccelaLabel)e.Item.FindControl("lblGroupCode");
            ListControl listControl;

            if (SingleServiceOnly)
            {
                listControl = (RadioButtonList)e.Item.FindControl("rbListServices");
                listControl.DataBound += RbListServices_DataBound;
            }
            else
            {
                listControl = (CheckBoxList)e.Item.FindControl("cbListServices");
                listControl.DataBound += ListServicesCheckBox_DataBound;
            }

            if (e.Item.DataItem is KeyValuePair<string, List<ServiceModel>>)
            {
                KeyValuePair<string, List<ServiceModel>> dataItem = (KeyValuePair<string, List<ServiceModel>>)e.Item.DataItem;
                listControl.DataSource = BuildServiceTable(dataItem.Value);
                listControl.DataTextField = "DisplayServiceName";
                listControl.DataValueField = "AgencyPlusServiceName";
                listControl.DataBind();

                lblGroupCode.Text = dataItem.Key;
            }

            if (e.Item.ItemIndex == 0)
            {
                Page.FocusElement(listControl.ClientID);
            }

            if (LicenseUtil.EnableExpiredLicense())
            {
                CheckServiceList(listControl, lblGroupCode.Text);
            }
        }

        /// <summary>
        /// The event for checkbox list check.
        /// </summary>
        /// <param name="sender">The object sender.</param>
        /// <param name="e">The EventArgs e.</param>
        protected void ServiceGroupLink_Selected(object sender, EventArgs e)
        {
            txtSearch.Text = string.Empty;
            List<string> serviceGroups = GetSelectedServiceGroups();

            SearchServiceList(serviceGroups, null);

            _displayOfServiceitemlist = false;
        }

        /// <summary>
        /// Append the http:// to url if the url does not start with http://
        /// </summary>
        /// <returns>Office site url</returns>
        protected string GetOfficialSiteUrlWithHttp()
        {
            string url = GetOfficialSiteUrl();
            if (!url.StartsWith("http://") && !url.StartsWith("https://"))
            {
                url = url.Insert(0, "http://");
            }

            return url;
        }

        /// <summary>
        /// Get Official Site Url.
        /// </summary>
        /// <returns>String for site url.</returns>
        protected string GetOfficialSiteUrl()
        {
            string url = StandardChoiceUtil.GetOfficialWebSite();

            if (string.IsNullOrEmpty(url))
            {
                return string.Empty;
            }

            string postfix = string.Empty;

            if (!string.IsNullOrEmpty(ZipCode))
            {
                if (ZipCode.Contains("-"))
                {
                    ZipCode = ZipCode.Replace("-", string.Empty);
                }

                postfix = "zip=" + ZipCode;

                if (!string.IsNullOrEmpty(City))
                {
                    postfix += "&City=" + City;
                }
            }
            else
            {
                // Zip is empty
                if (!string.IsNullOrEmpty(City))
                {
                    postfix += "City=" + City;
                }
            }

            // Append the zip and/or city info to the url.
            if (!string.IsNullOrEmpty(postfix))
            {
                // If there is no parameter in url, append "?", otherwise append "&"
                if (url.Contains("?"))
                {
                    if (url.EndsWith("?"))
                    {
                        url += postfix;
                    }
                    else
                    {
                        // The url has parameter.
                        url += "&" + postfix;
                    }
                }
                else
                {
                    url += "?" + postfix;
                }
            }

            return url;
        }

        /// <summary>
        /// Goto the next step
        /// </summary>
        /// <param name="selectedServices">The selected services</param>
        private void GotoNextStep(ServiceModel[] selectedServices)
        {
            AppSession.SetSelectedServicesToSession(selectedServices);

            string createdBy = "&createdBy=" + ACAConstant.PUBLIC_USER_NAME + AppSession.User.UserSeqNum;
            string moduleName = Request.QueryString["Module"];

            if (!string.IsNullOrEmpty(Request.QueryString["createdBy"]))
            {
                createdBy = "&createdBy=" + Request.QueryString["createdBy"];
            }

            string from = Request.QueryString["isFeeEstimator"] == null ? string.Empty : Request.QueryString["isFeeEstimator"].ToUpperInvariant();
            string url = "~/Cap/CapType.aspx?Module={0}&stepNumber={1}&pageNumber={2}&isFeeEstimator={3}&FilterName=" + Request.QueryString["FilterName"] + createdBy;

            if (!string.IsNullOrEmpty(Request.QueryString[UrlConstant.CAPTYPE]))
            {
                url += ACAConstant.AMPERSAND + UrlConstant.CAPTYPE + ACAConstant.EQUAL_MARK + Request.QueryString[UrlConstant.CAPTYPE].ToString();
            }
            else if (selectedServices != null && selectedServices.Length == 1)
            {
                IPageflowBll pageflowBll = (IPageflowBll)ObjectFactory.GetObject(typeof(IPageflowBll));
                string capTypeUrlParam = CAPHelper.GetCapTypeValue(selectedServices[0].capType);
                CapTypeModel captype = CapUtil.ConstructCAPTypeModel(ModuleName, capTypeUrlParam, selectedServices[0].servPorvCode);
                PageFlowGroupModel pageflowGroup = pageflowBll.GetPageflowGroupByCapType(captype);

                if (pageflowGroup != null)
                {
                    url += ACAConstant.AMPERSAND + UrlConstant.CAPTYPE + ACAConstant.EQUAL_MARK + UrlEncode(capTypeUrlParam) + ACAConstant.AMPERSAND + UrlConstant.AgencyCode + ACAConstant.EQUAL_MARK + selectedServices[0].servPorvCode;
                    url += ACAConstant.AMPERSAND + ACAConstant.IS_SUBAGENCY_CAP + ACAConstant.EQUAL_MARK + ACAConstant.COMMON_Y;
                    moduleName = captype.moduleName;
                }

                url += ACAConstant.AMPERSAND + "TabName" + ACAConstant.EQUAL_MARK + moduleName;
            }

            if (!IsForLocation)
            {
                url += ACAConstant.AMPERSAND + "createRecordByService=" + ACAConstant.COMMON_YES;
            }

            Response.Redirect(string.Format(url, moduleName, 1, 1, from));
        }

        /// <summary>
        /// Bind script
        /// </summary>
        /// <param name="hasService">True or false.</param>
        private void BindScript(bool hasService)
        {
            string hasServiceFlag = ACAConstant.COMMON_TRUE;

            if (!hasService)
            {
                hasServiceFlag = ACAConstant.COMMON_FALSE;
            }

            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowServiceList", "ShowServiceList('" + btnContinue.ClientID + "'," + hasServiceFlag + ");", true);

            if (SingleServiceOnly)
            {
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "SingleServiceSelectionOnly", "SetSingleServiceSelection();", true);
            }
        }

        /// <summary>
        /// Check the service one by one to see if it is unavailable or expired.
        /// </summary>
        /// <param name="listServices">List control.</param>
        /// <param name="groupCode">Group code.</param>
        private void CheckServiceList(ListControl listServices, string groupCode)
        {
            LicenseModel4WS[] licenses = AppSession.User.Licenses;

            if (listServices.Items.Count == 0 || licenses == null || string.IsNullOrEmpty(groupCode))
            {
                return;
            }

            string unavailableLicStr = GetTextByKey("superagency_service_message_unavailablelicense");
            string expiredLicStr = GetTextByKey("superagency_service_message_expiredlicense");
            List<ServiceModel> services = ServiceList[groupCode];

            for (int i = 0; i < listServices.Items.Count; i++)
            {
                string[] agencyPlusService = listServices.Items[i].Value.Split(ACAConstant.SPLIT_CHAR);
                string agency = agencyPlusService[0];
                string serviceName = agencyPlusService[1];

                ServiceModel service = services.FirstOrDefault(f => f.servPorvCode == agency && f.serviceName == serviceName);

                string serviceStatus = GetServiceStatus(service);

                if (!string.IsNullOrEmpty(serviceStatus))
                {
                    string message = string.Empty;
                    string expiredLicNums = string.Empty;
                    bool isAvailableService = true;

                    if (serviceStatus == UNAVAILABLE_SERVICE)
                    {
                        // the service is unavailable due to it's related license are unavailable.
                        message = unavailableLicStr;
                        isAvailableService = false;
                        expiredLicNums = DataUtil.ConcatStringListWithComma(_unAvailableLicNumList);
                    }
                    else if (serviceStatus == EXPIRED_SERVICE)
                    {
                        // the service is available, but some of the licenses are expired.
                        message = expiredLicStr;
                        expiredLicNums = DataUtil.ConcatStringListWithComma(_expiredLicNumList);
                    }

                    // {0}: expired license number; {1}: service name
                    string notice = ScriptFilter.FilterJSChar(
                        DataUtil.StringFormat(message, expiredLicNums, listServices.Items[i].Text));
                    listServices.Items[i].Attributes.Add("onclick", string.Format("ShowExpiredNotice(this,'{0}', '{1}', '{2}')", notice, isAvailableService, spanLicExpiredNotice.ClientID));
                }
            }
        }

        /// <summary>
        /// Get the service status. 
        /// UNAVAILABLE_SERVICE means the service is unavailable; 
        /// EXPIRED_SERVICE means the service has expired license, but it available.
        /// </summary>
        /// <param name="service">ServiceModel need be checked</param>
        /// <returns>A string indicating the service is available,expired or unavailable.</returns>
        private string GetServiceStatus(ServiceModel service)
        {
            string flag = string.Empty;
            int correspondingLicTypsCount = 0;
            _expiredLicNumList = new List<string>();
            _unAvailableLicNumList = new List<string>();

            RecordTypeLicTypePermissionModel[] licTypePermissions = service.licTypePermissions;
            LicenseModel4WS[] licenses = AppSession.User.Licenses;

            // Not license type for the service
            if (licenses == null || licTypePermissions == null || licTypePermissions.Length == 0)
            {
                return flag;
            }

            foreach (RecordTypeLicTypePermissionModel licTypePermission in licTypePermissions)
            {
                if (licTypePermission == null)
                {
                    continue;
                }

                foreach (LicenseModel4WS license in licenses)
                {
                    if (license != null &&
                        licTypePermission.licType.Equals(license.licenseType, StringComparison.InvariantCultureIgnoreCase))
                    {
                        correspondingLicTypsCount++;
                        CheckLicense(license, licTypePermission);

                        break;
                    }
                }
            }

            // All license types relatives the user's for this service are not available
            if (_unAvailableLicNumList.Count > 0 && _unAvailableLicNumList.Count == correspondingLicTypsCount)
            {
                flag = UNAVAILABLE_SERVICE;
            }
            else if (_expiredLicNumList.Count > 0)
            {
                flag = EXPIRED_SERVICE;
            }

            return flag;
        }

        /// <summary>
        /// Gets and reset parcel/owner model into ParcelInfoModel.
        /// </summary>
        /// <param name="parcelInfo">Parcel info model</param>
        /// <param name="address">Address model</param>
        /// <returns>Parcel info model with parcel/owner</returns>
        private ParcelInfoModel ResetParcelOwnerIntoParcelInfo(ParcelInfoModel parcelInfo, RefAddressModel address)
        {
            IAPOBll apoBll = (IAPOBll)ObjectFactory.GetObject(typeof(IAPOBll));
            SearchResultModel searchResult = apoBll.GetAPOListByAddress(ConfigManager.AgencyCode, address, null, true);
            DataTable dt = APOUtil.BuildAPODataTable(searchResult.resultList);

            if (dt.Rows.Count > 0)
            {
                DataRow dr = dt.Rows[0];

                OwnerModel ownerModel = new OwnerModel();
                ownerModel.ownerNumber = StringUtil.ToLong(dr["OwnerNumber"].ToString());
                ownerModel.UID = dr["OwnerUID"].ToString();
                ownerModel.ownerFullName = dr["OwnerFullName"].ToString();
                ownerModel.ownerTitle = dr["OwnerTitle"].ToString();
                ownerModel.mailAddress1 = dr["Address1OfOwner"].ToString();
                ownerModel.mailAddress2 = dr["Address2OfOwner"].ToString();
                ownerModel.mailAddress3 = dr["Address3OfOwner"].ToString();
                ownerModel.mailCity = dr["CityOfOwner"].ToString();
                ownerModel.mailZip = dr["ZipOfOwner"].ToString();
                ownerModel.mailState = dr["StateOfOwner"].ToString();
                ownerModel.mailCountry = dr["CountryOfOwner"].ToString();

                parcelInfo.ownerModel = ownerModel;

                ParcelModel parcelModel = new ParcelModel();
                parcelModel.parcelNumber = dr["ParcelNumber"].ToString();
                parcelModel.UID = dr["ParcelUID"].ToString();
                parcelModel.sourceSeqNumber = StringUtil.ToLong(dr["ParcelSequenceNumber"].ToString());
                parcelModel.lot = dr["LotOfParcel"].ToString();
                parcelModel.block = dr["BlockOfParcel"].ToString();
                parcelModel.subdivision = dr["SubdivisionOfParcel"].ToString();
                parcelModel.book = dr["BookOfParcel"].ToString();
                parcelModel.page = dr["PageOfParcel"].ToString();
                parcelModel.tract = dr["TractOfParcel"].ToString();
                parcelModel.parcelArea = StringUtil.ToDouble(dr["ParcelAreaOfParcel"].ToString());
                parcelModel.legalDesc = dr["LegalDescOfparcel"].ToString();
                parcelModel.landValue = StringUtil.ToDouble(dr["LandValueOfParcel"].ToString());
                parcelModel.improvedValue = StringUtil.ToDouble(dr["ImprovedValueOfParcel"].ToString());
                parcelModel.exemptValue = StringUtil.ToDouble(dr["ExceptionValueOfParcel"].ToString());

                parcelInfo.parcelModel = parcelModel;
            }

            return parcelInfo;
        }

        /// <summary>
        /// Get selected services
        /// They will be used in cap type page to generate children caps.
        /// </summary>
        /// <returns>ServiceModel Array.</returns>
        private ServiceModel[] GetSelectedServices()
        {
            List<ServiceModel> selectedServices = new List<ServiceModel>();

            Array agencyPlusServicesArray = hdnSelectedServices.Value.TrimStart(ACAConstant.SPLIT_CHAR1).TrimEnd(ACAConstant.SPLIT_CHAR1).Split(ACAConstant.SPLIT_CHAR1);

            List<ServiceModel> services = new List<ServiceModel>();

            foreach (KeyValuePair<string, List<ServiceModel>> item in ServiceList)
            {
                services.AddRange(item.Value);
            }

            foreach (string agencyPlusService in agencyPlusServicesArray)
            {
                string[] agencyPlusServiceArray = agencyPlusService.Split(ACAConstant.SPLIT_CHAR);
                string agency = string.Empty;
                string serviceName = string.Empty;

                if (agencyPlusServiceArray.Length >= 1)
                {
                    agency = agencyPlusServiceArray[0];
                }

                if (agencyPlusServiceArray.Length >= 2)
                {
                    serviceName = agencyPlusServiceArray[1];
                }

                if (!selectedServices.Exists(e => e.servPorvCode == agency && e.serviceName == serviceName))
                {
                    var serviceModel = services.FirstOrDefault(f => f.servPorvCode == agency && f.serviceName == serviceName);
                    selectedServices.Add(serviceModel);
                }
            }

            return selectedServices.ToArray();
        }

        /// <summary>
        /// Check the license.
        /// </summary>
        /// <param name="license">LicenseModel4WS object.</param>
        /// <param name="licTypePermission">The record type/license type permission model.</param>
        private void CheckLicense(LicenseModel4WS license, RecordTypeLicTypePermissionModel licTypePermission)
        {
            if (license.licExpired || license.insExpired || license.bizLicExpired)
            {
                _expiredLicNumList.Add(license.stateLicense);

                RecordTypeLicTypePermissionModel4WS licTypePermission4WS = null;

                if (licTypePermission != null)
                {
                    licTypePermission4WS = new RecordTypeLicTypePermissionModel4WS();
                    licTypePermission.licExpEnabled4ACA = licTypePermission.licExpEnabled4ACA;
                    licTypePermission.insExpEnabled4ACA = licTypePermission.insExpEnabled4ACA;
                    licTypePermission.bizLicExpEnabled4ACA = licTypePermission.bizLicExpEnabled4ACA;
                }

                bool isAvailable = LicenseUtil.IsAvailableLicense(license.licExpired, license.insExpired, license.bizLicExpired, licTypePermission4WS);

                if (!isAvailable)
                {
                    _unAvailableLicNumList.Add(license.stateLicense);
                }
            }
        }

        /// <summary>
        /// Construct a new DataTable for service.
        /// </summary>
        /// <param name="serviceList">Service List.</param>
        /// <returns>A DataTable that contains only one column:service name (agency name).</returns>
        private DataTable BuildServiceTable(List<ServiceModel> serviceList)
        {
            DataTable table = new DataTable();

            table.Columns.Add("DisplayServiceName");
            table.Columns.Add("AgencyPlusServiceName");

            foreach (ServiceModel service in serviceList)
            {
                DataRow dr = table.NewRow();
                dr["DisplayServiceName"] = I18nStringUtil.GetString(service.resServiceName, service.serviceName);
                dr["AgencyPlusServiceName"] = service.servPorvCode + ACAConstant.SPLIT_CHAR + service.serviceName;

                table.Rows.Add(dr);
            }

            return table;
        }

        /// <summary>
        /// Construct a new list for agency code.
        /// </summary>
        /// <param name="serviceGroupsParam">Service List.</param>
        /// <returns>List which contains agency name.</returns>
        private Dictionary<string, List<ServiceModel>> GetServPorvCodeList(XServiceGroupModel[] serviceGroupsParam)
        {
            if (serviceGroupsParam == null || serviceGroupsParam.Length == 0)
            {
                return null;
            }

            XServiceGroupModel[] serviceGroups = AppSession.User.IsAnonymous
                         ? serviceGroupsParam.Where(w => !ValidationUtil.IsNo(w.service.capType.anonymousCreateAllowed)).ToArray()
                         : serviceGroupsParam;

            Dictionary<string, List<ServiceModel>> result = new Dictionary<string, List<ServiceModel>>();

            var groups = serviceGroups.Where(w => w.group != null).Select(s => s.group).OrderBy(o => o.sortOrder).ThenBy(o => o.resGroupCode).ThenBy(o => o.groupCode);

            foreach (ServiceGroupModel groupModel in groups)
            {
                string groupCode = I18nStringUtil.GetString(groupModel.resGroupCode, groupModel.groupCode);

                if (result.ContainsKey(groupCode))
                {
                    continue;
                }

                List<ServiceModel> serviceModels = 
                    serviceGroups.Where(w => w.group != null && w.group.groupCode == groupModel.groupCode)
                                        .OrderBy(o => o.sortOrder).ThenBy(o => o.service.resServiceName).ThenBy(o => o.service.serviceName)
                                        .Select(s => s.service).Distinct(new ServiceModel.Comparer()).ToList();

                result.Add(groupCode, serviceModels);
            }

            /*
             * Bind other services which was not associated with the service group.
             * SortOrder by Service name
             */
            List<ServiceModel> others = serviceGroups.Where(w => w.group == null).OrderBy(o => o.service.resServiceName).ThenBy(o => o.service.serviceName).Select(s => s.service).Distinct(new ServiceModel.Comparer()).ToList();

            if (others.Count > 0)
            {
                string otherLabel = LabelUtil.GetGlobalTextByKey("aca_serviceselection_label_othergroup");

                if (!result.ContainsKey(otherLabel))
                {
                    result.Add(otherLabel, new List<ServiceModel>());
                }

                result[otherLabel].AddRange(others);
            }

            return result;
        }

        /// <summary>
        /// Search the service list.
        /// </summary>
        /// <param name="resGroupNames">The group name for I18N.</param>
        /// <param name="keyword">The keyword to search.</param>
        private void SearchServiceList(IList<string> resGroupNames, string keyword)
        {
            if (OriginalServiceList == null)
            {
                OriginalServiceList = ObjectCloneUtil.DeepCopy(ServiceList);
            }

            if (OriginalServiceList == null || ((resGroupNames == null || resGroupNames.Count == 0) && string.IsNullOrEmpty(keyword)))
            {
                BindServiceList(OriginalServiceList, true);
                return;
            }

            ServiceList = new Dictionary<string, List<ServiceModel>>();

            foreach (var item in OriginalServiceList)
            {
                // If it not exists in group name list, not add the services.
                if (resGroupNames != null && resGroupNames.Count() > 0 && !resGroupNames.Contains(item.Key))
                {
                    continue;
                }

                // If search without keyword, add all the services of the group name.
                if (string.IsNullOrEmpty(keyword))
                {
                    ServiceList.Add(item.Key, item.Value);
                    continue;
                }

                // If it contains in the group name list, filter with the keyword.
                List<ServiceModel> serviceModels = item.Value.Where(
                            s => s.serviceName.IndexOf(keyword, StringComparison.InvariantCultureIgnoreCase) > -1
                         || s.resServiceName.IndexOf(keyword, StringComparison.InvariantCultureIgnoreCase) > -1).ToList();

                if (serviceModels.Count > 0)
                {
                    ServiceList.Add(item.Key, serviceModels);
                }
            }

            BindServiceList(ServiceList, true);
        }

        /// <summary>
        /// Get the selected service groups.
        /// </summary>
        /// <returns>Return the selected service groups.</returns>
        private List<string> GetSelectedServiceGroups()
        {
            List<string> serviceGroups = new List<string>();

            foreach (ListItem item in cbListServiceGroup.Items)
            {
                if (item.Selected)
                {
                    serviceGroups.Add(item.Value);
                }
            }

            return serviceGroups;
        }
    }
}