    #region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: Navigation.aspx.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2007-2014
 *
 *  Description:
 *
 *  Notes:
 *      $Id: Navigation.ascx.cs 278837 2014-09-15 15:42:15Z ACHIEVO\james.shi $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using Accela.ACA.BLL;
using Accela.ACA.BLL.Common;
using Accela.ACA.BLL.MyCollection;
using Accela.ACA.BLL.People;
using Accela.ACA.BLL.Report;
using Accela.ACA.BLL.ShoppingCart;
using Accela.ACA.Common;
using Accela.ACA.Common.Common;
using Accela.ACA.Common.Log;
using Accela.ACA.Common.Util;
using Accela.ACA.Web.Common;
using Accela.ACA.Web.SocialMedia;
using Accela.ACA.Web.Util;
using Accela.ACA.WSProxy;
using log4net;

namespace Accela.ACA.Web.Component
{
    /// <summary>
    /// Navigation page
    /// </summary>
    public partial class Navigation : BaseUserControl
    {
        #region Fields

        /// <summary>
        /// This announcement list control ID
        /// </summary>
        private const string ComponentAnnouncementListID = "ctl00$PlaceHolderMain$AnnouncementListComponent";

        /// <summary>
        /// Logger instance.
        /// </summary>
        private static readonly ILog Logger = LogFactory.Instance.GetLogger(typeof(Navigation));

        /// <summary>
        /// collection image url
        /// </summary>
        private string _collectionImgUrl = string.Empty;

        /// <summary>
        ///  Is amount equal zero
        /// </summary>
        private bool _isAmountEqualZero = false;

        /// <summary>
        /// Redirect collection management page
        /// </summary>
        private string _redirectCollectionManagementPage = "#";

        /// <summary>
        /// if it is opened in ACA Admin and show the registered welcome page
        /// </summary>
        private bool _isAdminModeRegistered;

        /// <summary>
        /// when click a report link in page header's report list to print a report, it is true.
        /// </summary>
        private bool isPrintingReport = false;

        /// <summary>
        /// This indicating the value whether current page is on announcement list 
        /// </summary>
        private bool _isOnAnnouncementList = false;

        /// <summary>
        /// Indicating the value whether use the announcement function
        /// </summary>
        private bool _useAnnouncement = false;

        /// <summary>
        /// Indicating the value the announcement interval value
        /// </summary>
        private string _announcementInterval;

        #endregion Fields

        #region Properties 
        
        /// <summary>
        /// Gets the value indicating whether on announcement list page
        /// </summary>
        public string OnAnnouncementList
        {
            get
            {
                return _isOnAnnouncementList.ToString();
            }
        }

        /// <summary>
        /// Gets a value indicating whether use the announcement function
        /// </summary>
        public bool UseAnnouncement
        {
            get
            {
                return _useAnnouncement;
            } 
        }

        /// <summary>
        /// Gets the announcement interval value
        /// </summary>
        public string AnnouncementInterval
        {
            get
            {
                return _announcementInterval;
            } 
        }

        /// <summary>
        /// Gets or sets collection image url
        /// </summary>
        protected string CollectionImgUrl
        {
            get
            {
                return _collectionImgUrl;
            }

            set
            {
                _collectionImgUrl = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the amount is zero 
        /// </summary>
        protected bool IsAmountEqualZero
        {
            get
            {
                return _isAmountEqualZero;
            }

            set
            {
                _isAmountEqualZero = value;
            }
        }

        /// <summary>
        /// Gets or sets collection management page url
        /// </summary>
        protected string RedirectCollectionManagementPage
        {
            get
            {
                return _redirectCollectionManagementPage;
            }

            set
            {
                _redirectCollectionManagementPage = value;
            }
        }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Set sopping cart number
        /// </summary>
        /// <param name="shoppingCartItemNumber">string shopping cart item number</param>
        public void SetShoppingCartNumber(string shoppingCartItemNumber)
        {
            if (string.IsNullOrEmpty(shoppingCartItemNumber))
            {
                shoppingCartItemNumber = "0";
            }

            if (!AppSession.IsAdmin)
            {
                lblShoppingCart.Text = string.Format("{0} ({1})", GetSuperAgencyTextByKey("com_headNav_label_carttitle"), shoppingCartItemNumber);
            }
        }

        /// <summary>
        /// Raises the page load event
        /// </summary>
        /// <param name="sender">An object that contains the event sender.</param>
        /// <param name="e">A System.EventArgs object containing the event data.</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            _isAdminModeRegistered = !string.IsNullOrEmpty(Request["registered"]);
            string tabName = Request.QueryString["TabName"];
            if (string.IsNullOrEmpty(tabName))
            {
                tabName = Request.QueryString["Module"];
            }

            BindTabs(tabName, _isAdminModeRegistered);

            if (!AppSession.IsAdmin && !GlobalSearchUtil.IsGlobalSearchEnabled())
            {
                ucGlobalSearch.Visible = false;
            }

            if (AppSession.IsAdmin && !_isAdminModeRegistered)
            {
                btnRegister.Visible = true;
                lblAdminReports.Visible = true;

                beforeLogin.Visible = true;

                // config accessibility special function
                ConfigSupportAccessibility();
                return;
            }

            if (!IsPostBack)
            {
                DisplayLoginInfo();
                SetAccountManagmentLinkVisibility();
            }

            _collectionImgUrl = ImageUtil.GetImageURL("caret_expanded.gif");

            // Get the latest cap ids when user clicking a report.
            if (IsPostBack && Request["__EVENTTARGET"] == btnPostForReport.ClientID)
            {
                isPrintingReport = true;
                string altIDs = GetAltIDs();
                ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "openParamWindow", "openParamWin(" + altIDs + ");", true);
            }

            if (AppSession.User != null && !AppSession.User.IsAnonymous && !AppSession.User.IsAgentClerk && !AppSession.User.IsAuthorizedAgent)
            {
                int myCollectionAmount = 0;
                string script = BuildMyCollectionHTML(out myCollectionAmount);

                if (!AppSession.IsAdmin)
                {
                    _redirectCollectionManagementPage = FileUtil.AppendApplicationRoot("/MyCollection/MyCollectionManagement.aspx");
                    lblMyCollection.Text = string.Format("{0} ({1})", GetSuperAgencyTextByKey("mycollection_global_label_collection"), myCollectionAmount);
                }
                
                if (myCollectionAmount == 0)
                {
                    _isAmountEqualZero = true;
                }

                if (Visible)
                {
                    Page.ClientScript.RegisterClientScriptBlock(Page.GetType(), "register", script, true);
                }
            }

            // config accessibility special function
            ConfigSupportAccessibility();

            _useAnnouncement = StandardChoiceUtil.IsEnableAnnouncement();
            _announcementInterval = StandardChoiceUtil.GetAnnouncementInterval();

            if (string.IsNullOrEmpty(_announcementInterval) || _announcementInterval.Equals("0"))
            {
                _announcementInterval = "5";
            }

            AnnouncementList announcementListComponent = (AnnouncementList)Page.FindControl(ComponentAnnouncementListID);
           
            if (announcementListComponent != null)
            { 
               _isOnAnnouncementList = true; 
            }
            else
            {
                _isOnAnnouncementList = false; 
            }

            btnLogout.HRef = btnLogout.Visible ? AuthenticationUtil.LogoutUrl : string.Empty;
            btnRegister.HRef = btnRegister.Visible ? AuthenticationUtil.RegisterUrl : string.Empty;

            if (btnLogin.Visible)
            {
                btnLogin.HRef = AuthenticationUtil.LoginUrl;
                string loginUrlTarget = AuthenticationUtil.LoginUrlTarget;

                if (!string.IsNullOrEmpty(loginUrlTarget))
                {
                    btnLogin.Target = string.Concat(ACAConstant.SPLIT_CHAR5, loginUrlTarget.ToLower());
                }
            }

            if (AppSession.User != null && !AppSession.User.IsAnonymous && (AppSession.User.IsAgentClerk || AppSession.User.IsAuthorizedAgent))
            {
                ucGlobalSearch.Visible = false;
            }
        } 

        /// <summary>
        /// Page PreRender event method
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void Page_PreRender(object sender, EventArgs e)
        {
            if (StandardChoiceUtil.IsEnableShoppingCart() && AppSession.User != null && !AppSession.User.IsAnonymous)
            {
                string cartItemNumber = AppSession.GetCartItemNumberFromSession();
                if (string.IsNullOrEmpty(cartItemNumber))
                {
                    IShoppingCartBll shoppingCartBll = ObjectFactory.GetObject<IShoppingCartBll>();
                    cartItemNumber = shoppingCartBll.GetShoppingCartItemsCount().ToString();
                    AppSession.SetCartItemNumberToSession(cartItemNumber);
                }

                SetShoppingCartNumber(cartItemNumber);
            }

            if (!isPrintingReport)
            {
                BindReportList();
            }

            HandleSplit();

            //Hide the logout link for facebook user.
            if (!AppSession.IsAdmin && SocialMediaUtil.IsFacebookAppLogin)
            {
                btnLogout.Visible = false;
                lblSplit3.Visible = false;
            }

            if (!AppSession.IsAdmin)
            {
                InitAnnouncementWebService();
            }
        }

        /// <summary>
        /// Set account management link visibility based on standard choice settings.
        /// </summary>
        private void SetAccountManagmentLinkVisibility()
        {
            //if user isn't anonymous.
            if (AppSession.User != null && !AppSession.User.IsAnonymous)
            {
                managerURL.Visible = lblSplit3.Visible = StandardChoiceUtil.IsAccountManagementEnabled();
            }

            //Hide the account management link for agent clerk user.
            if (AppSession.User != null && AppSession.User.IsAgentClerk)
            {
                managerURL.Visible = false;
            }
        }

        /// <summary>
        /// Append the filter name into url when the filter name is not empty.
        /// </summary>
        /// <param name="tab">table item.</param>
        /// <param name="link">the link item.</param>
        /// <param name="filterName">the filter name.</param>
        private void AppendFilterToTab(TabItem tab, LinkItem link, string filterName)
        {
            if (string.IsNullOrEmpty(filterName))
            {
                return;
            }

            if (GetFileNameFromUrl(link.Url).Equals(GetFileNameFromUrl(tab.Url), StringComparison.InvariantCultureIgnoreCase))
            {
                if (tab.Url.IndexOf("FilterName") < 0)
                {
                    tab.Url += "&FilterName=" + filterName;
                }
            }
        }

        /// <summary>
        /// display or hidden split sign.
        /// </summary>
        private void HandleSplit()
        {
            if (!AppSession.IsAdmin)
            {
                if (beforeLogin.Visible)
                {
                    accessibilitySplit.Visible = divAccessibilityBefore.Visible && (btnRegister.Visible || ACAConstant.COMMON_Y.Equals(hdnShowReportLink.Value) || btnLogin.Visible);
                    lblSplit2.Visible = btnRegister.Visible && (ACAConstant.COMMON_Y.Equals(hdnShowReportLink.Value) || btnLogin.Visible);
                    reportSplit.Visible = ACAConstant.COMMON_Y.Equals(hdnShowReportLink.Value) && btnLogin.Visible;
                    ucAnnBeforeLogin.FindControl("lblSplitAnnouncement").Visible = divAccessibilityBefore.Visible || btnRegister.Visible || ACAConstant.COMMON_Y.Equals(hdnShowReportLink.Value) || btnLogin.Visible;
                    lblSplitAccessibility.Visible = btnRegister.Visible || ACAConstant.COMMON_Y.Equals(hdnShowReportLink.Value) || btnLogin.Visible;
                }
                else
                {
                    collectionSplit.Visible = divShoppingCart.Visible || ACAConstant.COMMON_Y.Equals(hdnShowReportLink.Value) || managerURL.Visible || btnLogout.Visible;
                    lblSplitShoppingCart.Visible = divShoppingCart.Visible && (ACAConstant.COMMON_Y.Equals(hdnShowReportLink.Value) || managerURL.Visible || btnLogout.Visible);
                    reportSplit2.Visible = ACAConstant.COMMON_Y.Equals(hdnShowReportLink.Value) && (managerURL.Visible || btnLogout.Visible);
                    lblSplit3.Visible = managerURL.Visible && btnLogout.Visible;
                }
            }
        }

        /// <summary>
        /// Bind Report List
        /// </summary>
        private void BindReportList()
        {
            if (AppSession.IsAdmin)
            {
                this.lblAdminReports.Style.Add("display", string.Empty);
                this.lblAdminReport2.Style.Add("display", string.Empty);
                this.lblAdminReports.LabelKey = "aca_report_label";
                this.lblAdminReport2.LabelKey = "aca_report_label";
                return;
            }
            else
            {
                this.lblAdminReports.Style.Add("display", "none");
                this.lblAdminReport2.Style.Add("display", "none");
            }

            StringBuilder sb = new StringBuilder();

            CapModel4WS capModel = AppSession.GetCapModelFromSession(ModuleName);
            CapIDModel4WS capIDModel = capModel == null ? null : capModel.capID;

            // if the current page has it's own page id, get it. Otherwise, get the BasePage's
            IPage currentPage = this.Page as IPage;
            string pageID = currentPage.PageID;

            // Get reports by page id
            IReportBll reportBll = ObjectFactory.GetObject<IReportBll>();
            ReportButtonPropertyModel4WS[] reports = null;

            if (!IsPostBack)
            {
                reports = reportBll.GetReportLinkProperty(capIDModel, ModuleName, pageID);
                Session[SessionConstant.SESSION_GLOBAL_REPORT] = reports;
            }
            else
            {
                reports = Session[SessionConstant.SESSION_GLOBAL_REPORT] == null ? null : (ReportButtonPropertyModel4WS[])Session[SessionConstant.SESSION_GLOBAL_REPORT];
            }

            string flag = afterLogin.Visible ? "true" : "false";

            if (reports == null || reports.Length == 0)
            {
                hdnShowReportLink.Value = ACAConstant.COMMON_N;
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "HideReports", "hideReportLink(" + flag + ");", true);
            }
            else
            {
                hdnShowReportLink.Value = ACAConstant.COMMON_Y;
                string reportLable = string.Format(GetSuperAgencyTextByKey("aca_report_label"), reports.Length.ToString());

                string reportHtml = BuildReportHtml(reports);
                sb.Append(reportHtml);

                string scriptFunc = string.Format("bindReports({0},{1},'{2}');", sb.ToString(), flag, reportLable);
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "Reports", scriptFunc, true);
            }
        }

        /// <summary>
        /// Bind the tab list.
        /// </summary>
        /// <param name="tabName">string tab name</param>
        /// <param name="isAdminModeRegistered">is registered mode in admin site</param>
        private void BindTabs(string tabName, bool isAdminModeRegistered)
        {
            if (!AppSession.IsAdmin)
            {
                isAdminModeRegistered = false;
            }

            IList<TabItem> tabList = null;

            try
            {
                tabList = GetTabsList(isAdminModeRegistered);

                if (tabList == null || tabList.Count == 0)
                {
                    return;
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                return;
            }

            TabItemCollection list = new TabItemCollection();

            // Hide Module list for Agent and Agent Clerk.
            if (AppSession.User == null || (!AppSession.User.IsAgentClerk && !AppSession.User.IsAuthorizedAgent))
            {
                foreach (TabItem tabItem in tabList)
                {
                    TabItem tab = new TabItem();
                    tab.Key = tabItem.Key;
                    tab.Label = tabItem.Label;
                    tab.Title = !string.IsNullOrEmpty(tabItem.Title) ? LabelUtil.RemoveHtmlFormat(tabItem.Title).Replace("'", "&#39;").Replace("\"", "&quot;") : string.Empty;
                    tab.Url = tabItem.Url;
                    tab.Order = tabItem.Order;
                    tab.Module = tabItem.Module;

                    foreach (LinkItem linkItem in tabItem.Children)
                    {
                        // filter the items according the global function table which agency-defined
                        if (string.IsNullOrEmpty(linkItem.Label) || !TabUtil.HasPermission(linkItem))
                        {
                            continue;
                        }

                        TabItem link = new TabItem();
                        link.Key = tabItem.Key;
                        link.Label = linkItem.Label;
                        link.Order = linkItem.Order;
                        link.Url = linkItem.Url;
                        tab.ChildItems.Add(link);
                    }

                    list.Add(tab);
                }
            }

            if (!string.IsNullOrEmpty(tabName))
            {
                Session[ACAConstant.ACTIVE_TAB_NAME] = tabName;
            }
            else
            {
                string module = Request.QueryString["module"];
                if (!string.IsNullOrEmpty(module))
                {
                    tabName = Session[ACAConstant.ACTIVE_TAB_NAME] as string;
                }
            }

            if (string.IsNullOrEmpty(tabName))
            {
                tabName = "home";
            }

            ucTabBar.IsRTL = I18nCultureUtil.UserPreferredCultureInfo.TextInfo.IsRightToLeft;
            ucTabBar.CurrentTabName = tabName;
            ucTabBar.HomeTabName = GetSuperAgencyTextByKey("aca_sys_default_home");
            ucTabBar.IsAdmin = AppSession.IsAdmin;
            ucTabBar.TabItems = list;
        }

        /// <summary>
        /// Build my collection menu.
        /// </summary>
        /// <param name="myCollectionAmount">my collection amount.</param>
        /// <returns>my collection html string</returns>
        private string BuildMyCollectionHTML(out int myCollectionAmount)
        {
            MyCollectionModel[] myCollections = GetMyCollectionsFromSession();

            if (myCollections == null || myCollections.Length == 0)
            {
                myCollectionAmount = 0;
                return string.Empty;
            }

            myCollectionAmount = myCollections.Length;

            string urlGotoDetailPage = FileUtil.AppendApplicationRoot("/MyCollection/MyCollectionDetail.aspx?collectionId=");
            string urlGotoManagementPage = FileUtil.AppendApplicationRoot("/MyCollection/MyCollectionManagement.aspx");

            StringBuilder sb = new StringBuilder();

            sb.Append("window.onload=function(){");
            sb.Append("var objTimeout = null;");
            sb.Append(" var links = [");
            string collectionName = string.Empty;

            if (myCollectionAmount > 5)
            {
                for (int i = 0; i < 5; i++)
                {
                    collectionName = ScriptFilter.FilterScript(myCollections[i].collectionName).Replace("'", "&#39;").Replace("\"", "&quot;");
                    sb.Append(string.Format("[['html','<a id=\"{3}\" onfocus=\"SetFocusInForCollection();\" onblur=\"SetBlurOutForCollection();\" href={0}{1}>{2}</a>'],['menuFocusId','{4}']],", urlGotoDetailPage, myCollections[i].collectionId, collectionName, "collection" + myCollections[i].collectionId, "collection" + myCollections[i].collectionId));
                }

                sb.Append(string.Format("[['align','right'],['html','<a  href={0}>{1}</a>']]", urlGotoManagementPage, GetSuperAgencyTextByKey("mycollection_homepage_label_more")));
            }
            else
            {
                foreach (MyCollectionModel myCollection in myCollections)
                {
                    collectionName = ScriptFilter.FilterScript(myCollection.collectionName).Replace("'", "&#39;").Replace("\"", "&quot;");
                    sb.Append(string.Format("[['html','<a id=\"{3}\" onfocus=\"SetFocusInForCollection();\" onblur=\"SetBlurOutForCollection();\" href={0}{1}>{2}</a>'],['menuFocusId','{4}']],", urlGotoDetailPage, myCollection.collectionId, collectionName, "collection" + myCollection.collectionId, "collection" + myCollection.collectionId));
                }

                sb.Remove(sb.Length - 1, 1);
            }

            sb.Append("];");

            bool isRTL = I18nCultureUtil.UserPreferredCultureInfo.TextInfo.IsRightToLeft;
            string strRTL = string.Empty;

            if (isRTL)
            {
                strRTL = "1";
            }

            sb.Append("var prop=[['isRTL'," + strRTL + "],['sign','collection'],['align','right'],['menuId','dropmenu'],['targetId','tbCollection']];");
            sb.Append("if(typeof(hideMenu) == 'undefined'){return;}");
            sb.Append("var events=[[['eventType','click'],['func',hideMenu]],[['eventType','mouseout'],['func',hideMenu]],[['eventType','mouseover'],['func',displayMenu]]];");
            sb.Append("collection_menu=new Menu(links,prop,events);");
            sb.Append("if ($.browser.opera) { SetTabIndexForOpera();}");
            sb.Append("};");

            return sb.ToString();
        }

        /// <summary>
        /// Get my collections from session.
        /// </summary>
        /// <returns>my collection models</returns>
        private MyCollectionModel[] GetMyCollectionsFromSession()
        {
            IMyCollectionBll myCollectionBll = ObjectFactory.GetObject<IMyCollectionBll>();

            //Get my collection models from session.
            MyCollectionModel[] myCollections = AppSession.GetMyCollectionsFromSession();

            if (myCollections == null)
            {
                myCollections = myCollectionBll.GetMyCollection(ConfigManager.SuperAgencyCode, AppSession.User.PublicUserId);
                AppSession.SetMyCollectionsToSession(myCollections);
            }

            return myCollections;
        }

        /// <summary>
        /// Get my collection amount.
        /// </summary>
        /// <returns>my collection amount</returns>
        private int GetMyCollectionAmount()
        {
            MyCollectionModel[] myCollections = GetMyCollectionsFromSession();

            int myCollectionAmout = 0;

            if (myCollections != null && myCollections.Length > 0)
            {
                myCollectionAmout = myCollections.Length;
            }

            return myCollectionAmout;
        }

        /// <summary>
        /// Build html for report list
        /// </summary>
        /// <param name="reports">ReportButtonPropertyModel4WS array</param>
        /// <returns>report html string</returns>
        private string BuildReportHtml(ReportButtonPropertyModel4WS[] reports)
        {
            StringBuilder sbReport = new StringBuilder();

            string reportLable = string.Format(GetSuperAgencyTextByKey("aca_report_label"), reports.Length.ToString());

            // put the report names into the report list
            // sbReport.Append("var reports = [");
            sbReport.Append("[");
            sbReport.AppendFormat("[['html','<div class=\"Header_h3\">{0}:</div>'],['type','label']],", reportLable);

            foreach (ReportButtonPropertyModel4WS report in reports)
            {
                string url = string.Format("{0}Report/ReportParameter.aspx?module={1}&reportID={2}&reportType={3}", FileUtil.ApplicationRoot, ScriptFilter.AntiXssUrlEncode(ModuleName), report.reportId, report.buttonName);

                if (Request["collectionID"] != null)
                {
                    url += "&collectionID=" + Request["collectionID"].ToString();
                }

                string reportName = I18nStringUtil.GetString(report.resReportName, report.reportName);

                // build three properties: html,url and isMultiWindow
                sbReport.Append("[");

                string imgPath = ImageUtil.GetImageURL("notice_16_new.gif");
                string imgHtml = string.Empty;

                if (!string.IsNullOrEmpty(report.description))
                {
                    string description = report.description.Replace("\r\n", "&#13;"); // replace the "enter" key as html flag
                    imgHtml = string.Format("<img style=\"vertical-align:bottom;\" src=\"{0}\" title=\"{1}\" alt=\"{2}\" />", imgPath, ScriptFilter.EncodeJson(description), ScriptFilter.EncodeJson(description));
                }

                sbReport.AppendFormat("['html','<a style=\"margin:8px;\" id=\"{2}\" href=\"javascript:void(0);\" onfocus=\"SetFocusIn();\" onblur=\"SetBlurOut();\" >{0}</a>{1}'],", ScriptFilter.EncodeJson(reportName), imgHtml, "report" + report.reportId);

                sbReport.AppendFormat("['url','{0}'],", url);
                sbReport.AppendFormat("['isMultiWindow',{0}],", report.isMultipleWindow ? "true" : "false");
                sbReport.AppendFormat("['menuFocusId','{0}']", "report" + report.reportId);
                sbReport.Append("],");
            }

            sbReport.Remove(sbReport.Length - 1, 1); // remove the last ","
            sbReport.Append("]");

            return sbReport.ToString();
        }

        /// <summary>
        /// This method is to get Account Information.Display information base on login or not.
        /// </summary>
        private void DisplayLoginInfo()
        {
            if ((AuthenticationUtil.IsAuthenticated && AppSession.User != null && !AppSession.User.IsAnonymous) || _isAdminModeRegistered)
            {
                string formatText = BasePage.GetStaticTextByKey("aca_user_name");
                string userName = LabelUtil.RemoveHtmlFormat(formatText).Replace("\n", string.Empty).Trim();

                IPeopleBll peopleBll = ObjectFactory.GetObject<IPeopleBll>();
                string currentUserName = _isAdminModeRegistered ? "Administrator" : peopleBll.GetContactUserName(AppSession.User.UserModel4WS);
                string formatUserName = string.IsNullOrEmpty(formatText) ? currentUserName : formatText.Replace(userName, currentUserName);
                lblUserName.ToolTip = currentUserName;
                lblUserName.Text = HttpUtility.HtmlEncode(formatUserName.Replace(currentUserName, DataUtil.TruncateString(currentUserName, 25)));

                afterLogin.Visible = true;
            }

            if (!StandardChoiceUtil.IsEnableShoppingCart())
            {
                divShoppingCart.Visible = false;
            }

            beforeLogin.Visible = !afterLogin.Visible;

            //If enable new ui, will hide 
            if (StandardChoiceUtil.IsEnableNewTemplate() && !AppSession.IsAdmin)
            {
                divNavigation.Attributes.CssStyle.Add("display", "none");
            }

            // if registration links is disabled, hidden the registration link.
            if (beforeLogin.Visible && !StandardChoiceUtil.IsRegistrationEnabled() && !AppSession.IsAdmin)
            {
                btnRegister.Visible = false;
                this.lblSplit2.Visible = false;
            }

            // if login links is disabled, hidden the login link.
            if (!StandardChoiceUtil.IsLoginEnabled() && !AppSession.IsAdmin)
            {
                this.lblSplit2.Visible = false;
                btnLogout.Visible = false;
                btnLogin.Visible = false;
            }
        }

        /// <summary>
        /// Get current page's alt ids.
        /// </summary>
        /// <returns>string alt ids</returns>
        private string GetAltIDs()
        {
            string[] altIDs = null;

            if (this.Page is IReportVariable)
            {
                altIDs = (Page as IReportVariable).CapAltIDs;
            }

            StringBuilder sbIDs = new StringBuilder();

            if (altIDs != null && altIDs.Length > 0)
            {
                foreach (string id in altIDs)
                {
                    sbIDs.Append("'");
                    sbIDs.Append(id);
                    sbIDs.Append("',");
                }

                sbIDs.Remove(sbIDs.Length - 1, 1);
            }

            return string.Format("[{0}]", sbIDs);
        }

        /// <summary>
        /// Get file name from url.
        /// </summary>
        /// <param name="url">string url</param>
        /// <returns>string filter name</returns>
        private string GetFileNameFromUrl(string url)
        {
            int startIndex = url.LastIndexOf('/');
            int endIndex = url.IndexOf('?');

            string urlPageName = url.Substring(startIndex + 1, endIndex - startIndex - 1);
            return urlPageName;
        }

        /// <summary>
        /// Build a full url.
        /// </summary>
        /// <param name="moduleName">module name.</param>
        /// <param name="key">string key</param>
        /// <returns>string filter name.</returns>
        private string GetFilterName(string moduleName, string key)
        {
            ICapTypeFilterBll capTypFiltereBll = ObjectFactory.GetObject<ICapTypeFilterBll>();

            return capTypFiltereBll.GetCapTypeFilterByLabelKey(ConfigManager.SuperAgencyCode, moduleName, key);
        }

        /// <summary>
        /// Gets tab index by tab name.
        /// </summary>
        /// <param name="tabName">string tab name.</param>
        /// <param name="tabList">all of tabs to be retrieved.</param>
        /// <param name="useDefault">is use default.</param>
        /// <returns>the tab index value that matched.</returns>
        private int GetTabIndexByName(string tabName, IList<TabItem> tabList, bool useDefault)
        {
            if (tabList == null || tabList.Count == 0)
            {
                return -1;
            }

            if (string.IsNullOrEmpty(tabName))
            {
                tabName = GetSuperAgencyTextByKey("aca_sys_default_home");
            }

            int index = 0;
            bool isFound = false;

            foreach (TabItem tab in tabList)
            {
                //if (tab.Title.ToLower() == tabName.ToLower())
                if (tab.Title.Equals(tabName, StringComparison.InvariantCultureIgnoreCase))
                {
                    isFound = true;
                    break;
                }

                index++;
            }

            // if there is no matched tab name, set it to first tab (home tab).
            if (!isFound)
            {
                if (useDefault)
                {
                    return GetTabIndexByName(null, tabList, false);
                }
                else
                {
                    return 0;
                }
            }

            return index;
        }

        /// <summary>
        /// Get the Tab list from DB
        /// </summary>
        /// <param name="isAdminModeRegistered">IsAdminModeRegistered Flag</param>
        /// <returns>IList of TabItem.</returns>
        private IList<TabItem> GetTabsList(bool isAdminModeRegistered)
        {
            IList<TabItem> tabList = TabUtil.GetTabList(isAdminModeRegistered);

            if (tabList == null)
            {
                return null;
            }

            IList<TabItem> visibleTabs = new List<TabItem>();

            // build full url for tab,append the TabName parameter to url
            foreach (TabItem tab in tabList)
            {
                // if this tab needn't to show as tab.
                if (!tab.TabVisible || string.IsNullOrEmpty(tab.Label))
                {
                    continue;
                }

                BasePage.BuildTabItem(tab, true);

                string filterName = string.Empty;

                tab.Url = RebuildUrl(tab.Url, tab.Key, filterName);

                foreach (LinkItem link in tab.Children)
                {
                    filterName = GetFilterName(link.Module, link.Label);

                    //Append the filter name into url when the filter name is not empty.
                    AppendFilterToTab(tab, link, filterName);

                    link.Label = LabelUtil.GetSuperAgencyTextByKey(link.Label, link.Module);
                    link.Url = RebuildUrl(link.Url, tab.Key, filterName);
                }

                visibleTabs.Add(tab);
            }

            return visibleTabs;
        }

        /// <summary>
        /// Build a full url.
        /// </summary>
        /// <param name="partialUrl">partial url.</param>
        /// <param name="tabName">string tab name</param>
        ///  <param name="filterName">string filter name</param>
        /// <returns>string full url.</returns>
        private string RebuildUrl(string partialUrl, string tabName, string filterName)
        {
            string url = FileUtil.AppendApplicationRoot(partialUrl);

            // if there is no parameter in url, append "?", otherwise append "&"
            if (url.IndexOf("?", StringComparison.InvariantCulture) == -1)
            {
                url += "?";
            }
            else
            {
                url += "&";
            }

            url += "TabName=" + tabName;

            if (!string.IsNullOrEmpty(filterName))
            {
                url += "&FilterName=" + HttpUtility.UrlEncode(filterName);
            }

            return url;
        }

        /// <summary>
        /// config support accessibility at navigation
        /// </summary>
        private void ConfigSupportAccessibility()
        {
            if (StandardChoiceUtil.AccessibilitySwitchEnabled())
            {
                HtmlGenericControl divAccessibility = afterLogin.Visible ? divAccessibilityAfter : divAccessibilityBefore;
                HtmlInputCheckBox chkAccessibility = afterLogin.Visible ? chkAccessibilityAfter : chkAccessibilityBefore;
                divAccessibility.Visible = true;
                chkAccessibility.Attributes.Add("title", LabelUtil.RemoveHtmlFormat(GetSuperAgencyTextByKey("aca_daily_accessibility")));
                chkAccessibility.Checked = AccessibilityUtil.AccessibilityEnabled;

                if (!AppSession.IsAdmin)
                {
                    chkAccessibility.Attributes.Add("onclick", "SupportAccessibility(this);");

                    string script = "var chkID ='" + ScriptFilter.AntiXssJavaScriptEncode(chkAccessibility.ClientID) + "';";
                    script += "var divAccessibility = '" + ScriptFilter.AntiXssJavaScriptEncode(divAccessibility.ClientID) + "';";
                    ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "supportAccessibility", script, true);
                }
            }
            else
            {
                AccessibilityUtil.AccessibilityEnabled = false;
            }
        }

        /// <summary>
        /// Register announcement web service.
        /// </summary>
        private void InitAnnouncementWebService()
        {
            ScriptManager smg = ScriptManager.GetCurrent(Page);
            smg.EnablePageMethods = true;
            ServiceReference strAnnoucement = new ServiceReference("~/WebService/AnnouncementService.asmx");
            smg.Services.Add(strAnnoucement);
        }

        #endregion Methods
    }
}
