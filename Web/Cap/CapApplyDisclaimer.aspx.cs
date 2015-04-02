#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: CapApplyDisclaimer.aspx.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2008-2014
 *
 *  Description:
 *
 *  Notes:
 *      $Id: CapApplyDisclaimer.aspx.cs 278148 2014-08-28 07:30:09Z ACHIEVO\james.shi $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Accela.ACA.BLL;
using Accela.ACA.BLL.Cap;
using Accela.ACA.BLL.Common;
using Accela.ACA.Common;
using Accela.ACA.Common.Common;
using Accela.ACA.Common.Util;
using Accela.ACA.Web.Common;
using Accela.ACA.Web.Util;
using Accela.ACA.WSProxy;
using Newtonsoft.Json;

namespace Accela.ACA.Web.Cap
{
    /// <summary>
    /// This class provide the ability to operation cap apply disclaimer.
    /// </summary>
    public partial class CapApplyDisclaimer : BasePage
    {
        #region Fields

        /// <summary>
        /// filter name.
        /// </summary>
        private string _filterName = string.Empty;

        /// <summary>
        /// jump CAP type
        /// </summary>
        private string _reqCAPType = string.Empty;

        /// <summary>
        /// jump module name
        /// </summary>
        private string _reqModuleName = string.Empty;

        /// <summary>
        /// the link item corresponding with request module name
        /// </summary>
        private LinkItem _linkItem = null;

        /// <summary>
        /// the tab item corresponding with request module name
        /// </summary>
        private TabItem _tabItem = null;

        /// <summary>
        /// Indicates the anonymous user can create cap type or not.
        /// </summary>
        private bool _anonymousCreateAllowed = true;

        #endregion Fields

        #region Methods

        /// <summary>
        /// OnPreLoad event method.
        /// </summary>
        /// <param name="e">EventArgs e</param>
        protected override void OnPreLoad(EventArgs e)
        {
            base.OnPreLoad(e);

            termAccept.Attributes["title"] = HttpUtility.HtmlAttributeEncode(
                LabelUtil.RemoveHtmlFormat(GetTextByKey("per_permitHome_label_acceptTerms")));

            // if it from super agency's deep link
            if (!IsPostBack && !AppSession.IsAdmin && AppSession.User.IsAnonymous)
            {
                string deepRecordType = string.Empty;
                string deepModuleName = string.Empty;
                string deepAgencyCode = ConfigManager.AgencyCode;

                //it means that from super agency service
                if (!string.IsNullOrWhiteSpace(Request.QueryString[UrlConstant.DEEPLINK_MULTIPLE_SERVICES_DATAKEY]))
                {
                    ServiceDataFromDeepLink serviceDataFromDeepLink;
                    var selectedServices = GetSelectedServicesFromDeepLink(out serviceDataFromDeepLink, false);

                    if (selectedServices != null && selectedServices.Length > 0)
                    {
                        IsForceLogin = false;

                        foreach (ServiceModel selectedService in selectedServices)
                        {
                            if (selectedService.capType == null)
                            {
                                continue;
                            }

                            /*
                             * if a service anonymousCreateAllowed is N,then redirect to login page
                             * else if a service anonymousCreateAllowed is empty,following the module configuration
                             */
                            if (ValidationUtil.IsNo(selectedService.capType.anonymousCreateAllowed))
                            {
                                _anonymousCreateAllowed = false;
                                return;
                            }
                            else if (string.IsNullOrEmpty(selectedService.capType.anonymousCreateAllowed))
                            {
                                IsForceLogin = true;
                            }
                        }

                        deepModuleName = serviceDataFromDeepLink.Module;
                        deepRecordType = serviceDataFromDeepLink.MasterRecordType;

                        if (selectedServices.Length == 1 && selectedServices[0].capType != null)
                        {
                            IPageflowBll pageflowBll = ObjectFactory.GetObject<IPageflowBll>();
                            PageFlowGroupModel pageflowGroup = pageflowBll.GetPageflowGroupByCapType(selectedServices[0].capType);

                            if (pageflowGroup != null)
                            {
                                deepRecordType = CAPHelper.GetCapTypeValue(selectedServices[0].capType);
                                deepAgencyCode = selectedServices[0].servPorvCode;
                            }
                        }
                    }
                }
                else if (!string.IsNullOrWhiteSpace(Request.QueryString[UrlConstant.CAPTYPE]) && !string.IsNullOrWhiteSpace(Request.QueryString["Module"]))
                {
                    //normal angecy 
                    IsForceLogin = false;
                    deepRecordType = Request.QueryString[UrlConstant.CAPTYPE];
                    deepModuleName = ModuleName;
                }

                /*
                 * if cap type anonymousCreateAllowed is N,then redirect to login page
                 * else if cap type anonymousCreateAllowed is empty,following the module configuration
                 */
                if (!string.IsNullOrEmpty(deepModuleName) && !string.IsNullOrEmpty(deepRecordType) && !string.IsNullOrEmpty(deepAgencyCode))
                {
                    CapTypeModel capType = CapUtil.ConstructCAPTypeModel(deepModuleName, deepRecordType, deepAgencyCode);

                    if (capType != null && !string.IsNullOrEmpty(capType.group) && ValidationUtil.IsNo(capType.anonymousCreateAllowed))
                    {
                        _anonymousCreateAllowed = false;
                        return;
                    }

                    if (capType != null && string.IsNullOrEmpty(capType.anonymousCreateAllowed))
                    {
                        IsForceLogin = true;
                    }
                }
            }
        }

        /// <summary>
        /// Page load method.
        /// </summary>
        /// <param name="sender">Object sender.</param>
        /// <param name="e">EventArgs e.</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (AppSession.IsAdmin)
            {
                return;
            }

            _filterName = Request.QueryString["FilterName"] == null ? string.Empty : HttpUtility.UrlEncode(Request.QueryString["FilterName"]);
            _reqCAPType = Request.QueryString[UrlConstant.CAPTYPE] == null ? string.Empty : Request.QueryString[UrlConstant.CAPTYPE];
            _reqModuleName = Request.QueryString["Module"] == null ? string.Empty : Request.QueryString["Module"];

            List<LinkItem> links = TabUtil.GetCreationLinkItemList(ModuleName, true);
            bool isCreateCapByService = ValidationUtil.IsYes(Request.QueryString["createRecordByService"]);

            if (CloneRecordUtil.IsCloneRecord(Request))
            {
                return;
            }
            else
            {
                if (isCreateCapByService)
                {
                    if (links.Exists(f => f.Key.Equals(ModuleName + ACAConstant.SPLIT_CHAR5 + ACAConstant.MODULE_CREATION_BY_SERVICE_KEY_TAIL, StringComparison.OrdinalIgnoreCase)))
                    {
                        _linkItem = links.Find(f => f.Key.Equals(ModuleName + ACAConstant.SPLIT_CHAR5 + ACAConstant.MODULE_CREATION_BY_SERVICE_KEY_TAIL, StringComparison.OrdinalIgnoreCase));
                    }
                }
                else if (links.Exists(f => f.Key.Equals(ModuleName + ACAConstant.SPLIT_CHAR5 + ACAConstant.MODULE_CREATION_KEY_TAIL, StringComparison.OrdinalIgnoreCase)))
                {
                    _linkItem = links.Find(f => f.Key.Equals(ModuleName + ACAConstant.SPLIT_CHAR5 + ACAConstant.MODULE_CREATION_KEY_TAIL, StringComparison.OrdinalIgnoreCase));
                }
            }

            _tabItem = TabUtil.GetTabItemWithModuleName(ModuleName);

            if (!IsPostBack)
            {
                bool isEnableCreateApplication = FunctionTable.IsEnableCreateApplication();

                if (_linkItem == null || _tabItem == null || !isEnableCreateApplication)
                {
                    if (string.IsNullOrEmpty(_reqCAPType) || string.IsNullOrEmpty(_reqModuleName) || !isEnableCreateApplication)
                    {
                        MessageUtil.ShowMessage(Page, MessageType.Notice, GetTextByKey("acc_login_label_invalidlink"));
                        btnNextStep.Visible = false;
                        return;
                    }

                    if (AppSession.User.IsAnonymous)
                    {
                        AuthenticationUtil.RedirectToLoginPage(Page.ResolveUrl(ACAConstant.URL_DEFAULT), "ReturnMessage=" + Server.UrlEncode(GetTextByKey("acc_login_label_invalidlink")));
                    }
                    else
                    {
                        Response.Redirect(Page.ResolveUrl(ACAConstant.URL_DEFAULT));
                    }
                }

                // if it is anonymous and the link is set as not anonymous to use.
                if ((AppSession.User == null || AppSession.User.IsAnonymous) && (IsForceLoginToApplyPermit(ModuleName) || !_linkItem.IsAnonymousInRoles) && IsForceLogin)
                {
                    AuthenticationUtil.RedirectToLoginPage("TabName" + ACAConstant.EQUAL_MARK + ModuleName);
                }

                // if it is disallow anonymous user to create then redirect to login page.
                if (!_anonymousCreateAllowed)
                {
                    AuthenticationUtil.RedirectToLoginPage("TabName=" + ModuleName);
                }

                // if it is register and the link is set as not register to use.
                if (!AppSession.User.IsAnonymous && (!_linkItem.IsRegisterInRoles || !_tabItem.IsRegisterInRoles))
                {
                    MessageUtil.ShowMessage(Page, MessageType.Notice, GetTextByKey("acc_login_label_invalidlink"));
                    btnNextStep.Visible = false;
                    return;
                }

                if (!string.IsNullOrEmpty(_reqCAPType) && !string.IsNullOrEmpty(_reqModuleName) && !IsCapTypeAccessible(_linkItem.Module, _reqCAPType, _linkItem))
                {
                    string returnMessage = string.Empty;

                    if (AppSession.User.IsAnonymous)
                    {
                        returnMessage = Server.UrlEncode(GetTextByKey("acc_login_label_forceLoginNote"));
                        AuthenticationUtil.RedirectToLoginPage("ReturnMessage=" + returnMessage);
                    }
                    else
                    {
                        returnMessage = Server.UrlEncode(GetTextByKey("acc_login_label_nopermissioncaptype"));
                        Response.Redirect(FileUtil.ApplicationRoot + "Welcome.aspx?" + ACAConstant.AMPERSAND + "ReturnMessage=" + returnMessage);
                    }
                }

                btnNextStep.AccessKey = AccessibilityUtil.GetAccessKey(AccessKeyType.SubmitForm);

                if (!CloneRecordUtil.IsCloneRecord(Request))
                {
                    // Cap home don't need cap model session, so here need to clear session
                    AppSession.SetCapModelToSession(ModuleName, null);
                }

                //show a message after creating another application from continue shopping in cap fee page.
                ShowMessage4ContinueShopping();

                //By pass Disclaimer page for Agent and Agent Clerk and deeplink
                if (AppSession.User.IsAgentClerk || AppSession.User.IsAuthorizedAgent || ValidationUtil.IsYes(Request.QueryString[UrlConstant.IS_SKIP_DISCLAIMER]))
                {
                    NextStepButton_Click(null, null);
                }
            }
        }

        /// <summary>
        /// The next step button click event.
        /// </summary>
        /// <param name="sender">The sender object.</param>
        /// <param name="e">The event arguments.</param>
        protected void NextStepButton_Click(object sender, EventArgs e)
        {
            string url = string.Empty;

            bool isSuperAgency = StandardChoiceUtil.IsSuperAgency();
            bool isEnableProxyUser = StandardChoiceUtil.IsEnableProxyUser();

            if (!string.IsNullOrWhiteSpace(Request.QueryString[UrlConstant.DEEPLINK_MULTIPLE_SERVICES_DATAKEY]))
            {
                url = BuildUrlForMultipleRecordsCreation();
            }
            else if (isSuperAgency && !isEnableProxyUser && !CloneRecordUtil.IsCloneRecord(Request))
            {
                if (!string.IsNullOrEmpty(_reqCAPType) && !string.IsNullOrEmpty(_reqModuleName))
                {
                    url = "WorkLocation.aspx?Module=" + ModuleName + "&FilterName=" + _filterName + ACAConstant.AMPERSAND + UrlConstant.CAPTYPE + ACAConstant.EQUAL_MARK + _reqCAPType + ACAConstant.AMPERSAND + "TabName" + ACAConstant.EQUAL_MARK + _linkItem.Module;
                }
                else
                {
                    url = "WorkLocation.aspx?Module=" + ModuleName + "&FilterName=" + _filterName;
                }
            }
            else
            {
                if (!string.IsNullOrEmpty(_reqCAPType) && !string.IsNullOrEmpty(_reqModuleName))
                {
                    url = "UserLicenseList.aspx?Module=" + _linkItem.Module + "&FilterName=" + _filterName + ACAConstant.AMPERSAND + UrlConstant.CAPTYPE + ACAConstant.EQUAL_MARK + _reqCAPType + ACAConstant.AMPERSAND + "TabName" + ACAConstant.EQUAL_MARK + _linkItem.Module;
                }
                else
                {
                    url = "UserLicenseList.aspx?Module=" + ModuleName + "&FilterName=" + _filterName;
                }

                if (CloneRecordUtil.IsCloneRecord(Request))
                {
                    url += ACAConstant.AMPERSAND + ACAConstant.IS_CLONE_RECORD + ACAConstant.EQUAL_MARK + ACAConstant.COMMON_TRUE;
                }

                if (!string.IsNullOrEmpty(Request.QueryString[UrlConstant.PAGEFLOW_GROUP_CODE]))
                {
                    url += "&" + UrlConstant.PAGEFLOW_GROUP_CODE + "=" + Server.UrlEncode(Request.QueryString[UrlConstant.PAGEFLOW_GROUP_CODE]);
                }
            }

            if (!string.IsNullOrEmpty(Request.QueryString[UrlConstant.AgencyCode]))
            {
                url += "&" + UrlConstant.AgencyCode + "=" + Request.QueryString[UrlConstant.AgencyCode];
            }

            if (ValidationUtil.IsYes(Request.QueryString[ACAConstant.IS_SUBAGENCY_CAP]))
            {
                url += "&" + ACAConstant.IS_SUBAGENCY_CAP + ACAConstant.EQUAL_MARK + ACAConstant.COMMON_Y;
            }

            if (ValidationUtil.IsYes(Request.QueryString["createRecordByService"]))
            {
                url += "&createRecordByService=" + ACAConstant.COMMON_YES;
            }

            Response.Redirect(url);
        }

        /// <summary>
        /// Build next step url for multiple records creation from deep link.
        /// </summary>
        /// <returns>The next step url for multiple records creation from deep link.</returns>
        private string BuildUrlForMultipleRecordsCreation()
        {
            string url = "UserLicenseList.aspx?";
            ServiceDataFromDeepLink serviceDataFromDeepLink;
            var selectedServices = GetSelectedServicesFromDeepLink(out serviceDataFromDeepLink, true);

            if (selectedServices != null && selectedServices.Length > 0)
            {
                AppSession.SetSelectedServicesToSession(selectedServices);
                url += "Module=" + serviceDataFromDeepLink.Module + "&TabName=" + serviceDataFromDeepLink.Module;
                bool useMasterRecordType = true;

                if (selectedServices.Length == 1 && selectedServices[0].capType != null)
                {
                    string recordType = CAPHelper.GetCapTypeValue(selectedServices[0].capType);
                    string agencyCode = selectedServices[0].servPorvCode;

                    IPageflowBll pageflowBll = ObjectFactory.GetObject<IPageflowBll>();
                    PageFlowGroupModel pageflowGroup = pageflowBll.GetPageflowGroupByCapType(selectedServices[0].capType);

                    if (pageflowGroup != null)
                    {
                        useMasterRecordType = false;
                        url += "&" + UrlConstant.CAPTYPE + "=" + HttpUtility.UrlEncode(recordType);
                        url += "&" + UrlConstant.AgencyCode + "=" + agencyCode;
                        url += "&" + ACAConstant.IS_SUBAGENCY_CAP + "=" + ACAConstant.COMMON_Y;
                    }
                }

                if (useMasterRecordType)
                {
                    url += "&" + UrlConstant.CAPTYPE + "=" + HttpUtility.UrlEncode(serviceDataFromDeepLink.MasterRecordType);
                }

                url += "&" + UrlConstant.DEEPLINK_MULTIPLE_SERVICES_DATAKEY + "=" + Request.QueryString[UrlConstant.DEEPLINK_MULTIPLE_SERVICES_DATAKEY];
            }

            return url;
        }

        /// <summary>
        /// Gets selected service for the multiple records creation from deep link.
        /// </summary>
        /// <param name="serviceData">return the service data model.</param>
        /// <param name="clearApplication">clear application</param>
        /// <returns>Service model array.</returns>
        private ServiceModel[] GetSelectedServicesFromDeepLink(out ServiceDataFromDeepLink serviceData, bool clearApplication)
        {
            ServiceModel[] selectedServices = null;

            string dataKey = Request.QueryString[UrlConstant.DEEPLINK_MULTIPLE_SERVICES_DATAKEY];

            //Use data key to get service data from Application State.
            IDeepLinkBLL deepLinkBLL = ObjectFactory.GetObject<IDeepLinkBLL>();
            DeepLinkAuditTrailModel deepLinkAuditTrail = deepLinkBLL.GetDeepLinkAuditTrail(dataKey, AppSession.User == null ? "PUBLICUSER0" : AppSession.User.PublicUserId);

            serviceData = null;

            if (deepLinkAuditTrail != null)
            {
                serviceData = (ServiceDataFromDeepLink)JsonConvert.DeserializeObject(deepLinkAuditTrail.serviceData, typeof(ServiceDataFromDeepLink));

                if (clearApplication)
                {
                    deepLinkBLL.RemoveDeepLinkAuditTrail(deepLinkAuditTrail, AppSession.User == null ? "PUBLICUSER0" : AppSession.User.PublicUserId);
                }

                if (serviceData != null && serviceData.ServiceList != null && serviceData.ServiceList.Count > 0)
                {
                    //Get available services of current user.
                    IServiceManagementBll serviceManagementBll = ObjectFactory.GetObject<IServiceManagementBll>();
                    ServiceModel[] availableServices = serviceManagementBll.GetServices4DeepLink(AppSession.User.UserSeqNum);
                    List<ServiceModel> tempServiceList;

                    if (availableServices != null && availableServices.Length > 0)
                    {
                        //Validate the service list based on the available services.
                        tempServiceList = new List<ServiceModel>();

                        foreach (var item in serviceData.ServiceList)
                        {
                            tempServiceList.AddRange(
                                availableServices.Where(s =>
                                                        s.servPorvCode.Equals(item.Agency, StringComparison.OrdinalIgnoreCase)
                                                        && s.serviceName.Equals(item.Name, StringComparison.OrdinalIgnoreCase)));
                        }

                        selectedServices = tempServiceList.ToArray();
                    }
                }
            }

            return selectedServices;
        }

        /// <summary>
        /// Indicate the cap type can be accessible for the module name
        /// </summary>
        /// <param name="moduleName">module name</param>
        /// <param name="capTypeString">cap type</param>
        /// <param name="linkItem">link item</param>
        /// <returns>Whether the cap type can be accessible for the module name.</returns>
        private bool IsCapTypeAccessible(string moduleName, string capTypeString, LinkItem linkItem)
        {
            bool isCapTypeAccessible = false;
            ICapTypeFilterBll capTypFiltereBll = (ICapTypeFilterBll)ObjectFactory.GetObject(typeof(ICapTypeFilterBll));
            _filterName = capTypFiltereBll.GetCapTypeFilterByLabelKey(ConfigManager.AgencyCode, linkItem.Module, linkItem.Label);
            
            CapTypeModel capType = CapUtil.ConstructCAPTypeModel(moduleName, capTypeString, ConfigManager.AgencyCode);
            if (capType != null && !string.IsNullOrEmpty(capType.group) && !string.IsNullOrEmpty(capType.type) && !string.IsNullOrEmpty(capType.subType) && !string.IsNullOrEmpty(capType.category))
            {
                ICapTypeBll capTypeBll = (ICapTypeBll)ObjectFactory.GetObject(typeof(ICapTypeBll));
                isCapTypeAccessible = capTypeBll.IsCapTypeAccessible(capType, moduleName, _filterName, ACAConstant.VCH_TYPE_VHAPP, AppSession.User.PublicUserId);
            }

            return isCapTypeAccessible;
        }

        /// <summary>
        /// show a message after creating another application from continue shopping in cap fee page.
        /// </summary>
        private void ShowMessage4ContinueShopping()
        {
            string altID = Request.QueryString["altID"];
            string message = string.Format(GetTextByKey("aca_createanotherapplication_success"), altID);
            bool isCreateAnotherApplication = ValidationUtil.IsYes(Request.QueryString["isCreateAnotherApplication"]);

            if (isCreateAnotherApplication)
            {
                MessageUtil.ShowMessage(Page, MessageType.Success, message);
            }
        }

        #endregion Methods
    }
}
