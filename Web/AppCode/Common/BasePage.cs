#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: BasePage.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2007-2014
 *
 *  Description:
 *
 *  Notes:
 *      $Id: BasePage.cs 279179 2014-10-13 08:59:15Z ACHIEVO\james.shi $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using Accela.ACA.BLL.Admin;
using Accela.ACA.BLL.Common;
using Accela.ACA.BLL.People;
using Accela.ACA.Common;
using Accela.ACA.Common.Common;
using Accela.ACA.Common.Log;
using Accela.ACA.Common.Util;
using Accela.ACA.Web.Common;
using Accela.ACA.Web.Common.Control;
using Accela.ACA.Web.Common.GlobalSearch;
using Accela.ACA.Web.ExpressionBuild;
using Accela.ACA.Web.Util;
using Accela.Web.Controls;
using log4net;
using Newtonsoft.Json;

namespace Accela.ACA.Web
{
    /// <summary>
    /// Base page for all ACA pages.
    /// </summary>
    public class BasePage : Page, IPage
    {
        #region Fields

        /// <summary>
        /// Logger instance.
        /// </summary>
        private static readonly ILog Logger = LogFactory.Instance.GetLogger(typeof(BasePage));
        
        /// <summary>
        /// time ticks.
        /// </summary>
        private long _timeFlag;

        /// <summary>
        /// Stopwatch instance.
        /// </summary>
        private System.Diagnostics.Stopwatch _watch = null;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the BasePage class.
        /// </summary>
        public BasePage()
        {
            if (Logger.IsDebugEnabled)
            {
                _timeFlag = DateTime.Now.Ticks;
                _watch = new System.Diagnostics.Stopwatch();
                _watch.Start();
                Logger.DebugFormat("==={0}.aspx Load begin [{1}]===", this.GetType().BaseType.Name, _timeFlag.ToString());
            }
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// Gets the application root, ends with /
        /// </summary>
        public static string ApplicationRoot
        {
            get
            {
                return FileUtil.ApplicationRoot;
            }
        }

        /// <summary>
        /// Gets the Customize path root, ends with /
        /// </summary>
        public static string CustomizeFolderRoot
        {
            get
            {
                return FileUtil.CustomizeFolderRoot;
            }
        }
        
        /// <summary>
        /// Gets current page's element id
        /// </summary>
        public virtual string PageID
        {
            get
            {
                string path = HttpContext.Current.Request.AppRelativeCurrentExecutionFilePath;

                bool isCapCompletions = Regex.IsMatch(path, "CapCompletions.aspx", RegexOptions.IgnoreCase);

                if (isCapCompletions)
                {
                    //CapCompletions.aspx is used for super agency,It's not available in ACA Admin.
                    //It should use the reports that configured in CapCompletion page.
                    path = Regex.Replace(path, "CapCompletions.aspx", "CapCompletion.aspx", RegexOptions.IgnoreCase);
                }

                // Convert the relative path to match the url stored in aca_admin_tree
                // e.g. ~/Welcome.aspx -> ../Welcome.aspx (aca_admin_tree use '../' as relative root)
                if (path.StartsWith("~"))
                {
                    path = path.Replace("~", "..");
                }

                IAdminBll adminBll = ObjectFactory.GetObject(typeof(IAdminBll)) as IAdminBll;
                string pageID = adminBll.GetPageIDbyUrl(path);

                return pageID;
            }
        }

        /// <summary>
        /// Gets or sets the style sheet
        /// </summary>
        public override string StyleSheetTheme
        {
            get
            {
                //return Accela.ACA.Common.Util.I18nCultureUtil.UserPreferredCulture;
                return "Default";
            }

            set
            {
                base.StyleSheetTheme = value;
            }
        }

        /// <summary>
        /// Gets a value indicating whether the controls in current page is rendered as admin mode.
        /// true - All controls will be rendered as daily mode.
        /// false- All controls will be rendered as admin mode
        /// </summary>
        bool IPage.IsControlRenderAsAdmin
        {
            get
            {
                return AppSession.IsAdmin;
            }
        }
        
        /// <summary>
        /// Gets the module name from request.
        /// </summary>
        protected string ModuleName
        {
            get
            {
                return GetModuleName(Request);
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether it is need force login(default value:true).
        /// </summary>
        protected bool IsForceLogin
        {
            get
            {
                if (ViewState["IsForceLogin"] != null)
                {
                    return (bool)ViewState["IsForceLogin"];
                }

                return true;
            }

            set
            {
                ViewState["IsForceLogin"] = value;
            }
        }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Is contact address user in daily as primary.
        /// </summary>
        /// <param name="contactAddressID">the contact address id.</param>
        /// <param name="agencyCode">The agency code.</param>
        /// <returns>
        /// true or false.
        /// </returns>
        [WebMethod(Description = "Is CA Used In Daily As Primary", EnableSession = true)]
        public static string GetContactAddressUsedInDailyAsPrimary(string contactAddressID, string agencyCode)
        {
            IPeopleBll peopleBll = ObjectFactory.GetObject<IPeopleBll>();
            string isPrimary = peopleBll.IsContactAddressUsedInDailyAsPrimary(agencyCode, long.Parse(contactAddressID)) ? ACAConstant.COMMON_Y : string.Empty;

            var timeBll = ObjectFactory.GetObject<ITimeZoneBll>();
            string agencyDate = I18nDateTimeUtil.FormatToDateStringForUI(timeBll.GetAgencyCurrentDate(agencyCode));

            return "{\"IsPrimary\":\"" + isPrimary + "\", \"AgencyDate\":\"" + agencyDate + "\"}";
        }

        /// <summary>
        /// Clear UI data from session.
        /// </summary>
        /// <param name="serializedUIDataTypes">Serialized UIDataType array.</param>
        [WebMethod(EnableSession = true)]
        public static void ClearUIData(string serializedUIDataTypes)
        {
            string[] dataTypes = JsonConvert.DeserializeObject<string[]>(serializedUIDataTypes);

            if (dataTypes != null && dataTypes.Length > 0)
            {
                foreach (string dataType in dataTypes)
                {
                    UIDataType uiDataType;

                    if (Enum.TryParse(dataType, out uiDataType))
                    {
                        ExpressionUtil.RemoveInputVariablesFromSession(uiDataType);
                        UIModelUtil.SetDataToUIContainer(null, uiDataType);
                    }
                }
            }
        }

        /// <summary>
        /// set value into AppSession.
        /// </summary>
        /// <param name="isEnableAccessibility">Whether it is enable accessibility or not.</param>
        [WebMethod(Description = "set value into Cookie", EnableSession = true)]
        public static void SetAccessibilityCookie(bool isEnableAccessibility)
        {
            AccessibilityUtil.AccessibilityEnabled = isEnableAccessibility;
        }

        /// <summary>
        /// return full virtual path with application root appended.
        /// </summary>
        /// <param name="partialPath">partial path.</param>
        /// <returns>the full virtual path.</returns>
        public static string AppendApplicationRoot(string partialPath)
        {
            return FileUtil.AppendApplicationRoot(partialPath);
        }

        /// <summary>
        /// return full virtual path with Customize Folder Root appended.
        /// </summary>
        /// <param name="partialPath">partial path.</param>
        /// <returns>the full virtual path.</returns>
        public static string AppendCustomizeFolderRoot(string partialPath)
        {
            return FileUtil.AppendCustomizeFolderRoot(partialPath);
        }

        /// <summary>
        /// Build Tab Item
        /// </summary>
        /// <param name="tab">the instance of TabItem</param>
        /// <param name="setLabel">is set label</param>
        public static void BuildTabItem(TabItem tab, bool setLabel)
        {
            string label = LabelUtil.GetSuperAgencyTextByKey(tab.Label, tab.Module);

            if (label == LabelUtil.GetGlobalTextByKey("aca_sys_default_module_name"))
            {
                label = DataUtil.AddBlankToString(tab.Module);
            }

            tab.Title = LabelUtil.RemoveHtmlFormat(label);

            if (setLabel)
            {
                tab.Label = label;
            }
        }

        /// <summary>
        /// combine the path.
        /// </summary>
        /// <param name="path1">the part one of path.</param>
        /// <param name="path2">the part two of path.</param>
        /// <returns>the path has been combined.</returns>
        public static string CombineWebPath(string path1, string path2)
        {
            return FileUtil.CombineWebPath(path1, path2);
        }

        /// <summary>
        /// Get the static text(label message) by label key.
        /// </summary>
        /// <param name="key">label key which is unique.</param>
        /// <returns>The text(label message) according the key.if can't find the key, return string.Empty.</returns>
        public static string GetStaticTextByKey(string key)
        {
            return LabelUtil.GetTextByKey(key, HttpContext.Current.Request[ACAConstant.MODULE_NAME]);
        }

        /// <summary>
        /// Get the module name from current request url.
        /// </summary>
        /// <param name="request">The HttpRequest.</param>
        /// <returns>The module name.</returns>
        public virtual string GetModuleName(HttpRequest request)
        {
            string moduleName = ScriptFilter.AntiXssHtmlEncode(request.QueryString[ACAConstant.MODULE_NAME]);

            if (moduleName != null && moduleName.IndexOf(",") > 0)
            {
                moduleName = moduleName.Split(new char[] { ',' })[0];
            }

            return moduleName;
        }

        /// <summary>
        /// do dispose when leave this page
        /// </summary>
        public override void Dispose()
        {
            base.Dispose();

            if (Logger.IsDebugEnabled)
            {
                _watch.Stop();
                Logger.DebugFormat("==={0}.aspx Load End   [{1}],costs(ms): {2} ms ===", this.GetType().BaseType.Name, _timeFlag.ToString(), _watch.ElapsedMilliseconds.ToString());
                _watch = null;
            }
        }

        /// <summary>
        /// Get the text(label message) by label key.
        /// </summary>
        /// <param name="key">label key which is unique.</param>
        /// <returns>The text(label message) according the key.if can't find the key, return string.Empty.</returns>
        public string GetTextByKey(string key)
        {
            return LabelUtil.GetTextByKey(key, ModuleName);
        }

        /// <summary>
        /// Get the text(label message) by label key.
        /// </summary>
        /// <param name="key">label key which is unique.</param>
        /// <returns>The text(label message) according the key.if can't find the key, return string.Empty.</returns>
        public string GetSuperAgencyTextByKey(string key)
        {
            return LabelUtil.GetSuperAgencyTextByKey(key, ModuleName);
        }

        /// <summary>
        /// Get the text(label message) by label key.
        /// </summary>
        /// <param name="key">label key which is unique.</param>
        /// <param name="moduleName">Module Name</param>
        /// <returns>The text(label message) according the key.if can't find the key, return string.Empty.</returns>
        public string GetTextByKey(string key, string moduleName)
        {
            return LabelUtil.GetTextByKey(key, moduleName);
        }

        /// <summary>
        /// Construct title value with image alt value with blank. 
        /// </summary>
        /// <param name="alt">the label key</param>
        /// <param name="title">the title of the key.</param>
        /// <returns>the title value</returns>
        public string GetTitleByKey(string alt, string title)
        {
            return GetTitleByKey(alt, title, true);
        }

        /// <summary>
        /// Gets the title by key.
        /// </summary>
        /// <param name="alt">The alt string.</param>
        /// <param name="title">The title string.</param>
        /// <param name="needEncode">Indicating whether the text needs to be encoded or not</param>
        /// <returns>the last title string</returns>
        public string GetTitleByKey(string alt, string title, bool needEncode)
        {
            return LabelUtil.GetTitleByKey(alt, title, needEncode, ModuleName);
        }

        /// <summary>
        /// Gets the current runtime page's module name.
        /// if current page doesn't belong to any module,returns string.Empty.
        /// </summary>
        /// <returns>module name.</returns>
        string IPage.GetModuleName()
        {
            return this.ModuleName;
        }

        /// <summary>
        /// Indicate force user login to apply permit or not.
        /// </summary>
        /// <param name="moduleName">The module name.</param>
        /// <returns>True if force login to apply permit, false otherwise.</returns>
        protected bool IsForceLoginToApplyPermit(string moduleName)
        {
            bool isForceLoginToApply = false;
            bool isLogin = AuthenticationUtil.IsAuthenticated;
            List<LinkItem> links = TabUtil.GetCreationLinkItemList(moduleName, false);

            LinkItem linkItem = null;

            if (links != null && links.Count > 0)
            {
                linkItem = links[0];
            }

            if (!AppSession.IsAdmin && !isLogin && (linkItem == null || IsFeatureForceLogin(linkItem.Url) || !linkItem.IsAnonymousInRoles))
            {
                isForceLoginToApply = true;
            }

            return isForceLoginToApply;
        }

        /// <summary>
        /// change master page
        /// </summary>
        protected virtual void ChangeMasterPage()
        {
            MasterPageFile = FileUtil.ApplicationRoot + ConfigManager.MasterPage;
        }

        /// <summary>
        /// change theme
        /// </summary>
        protected void ChangeTheme()
        {
            //Page.StyleSheetTheme = Accela.ACA.Common.Util.I18nCultureUtil.UserPreferredCulture;
            Page.Theme = I18nCultureUtil.UserPreferredCulture;
        }

        /// <summary>
        /// scroll to top
        /// </summary>
        protected virtual void GotoTop()
        {
            if (!AppSession.IsAdmin)
            {
                string script = @"var curWindow = this.window;
                                  curWindow.scrollTo(0, 0);
                                  while (curWindow.parent!= null && curWindow.parent != curWindow) { 
                                        try{                                           
                                            curWindow = curWindow.parent;
                                            curWindow.scrollTo(0, 0);
                                        }catch(ex){
                                           
                                        }
                                  };";

                ClientScript.RegisterStartupScript(this.GetType(), "GotoTop", script, true);
            }
        }

        /// <summary>
        /// On Initial event
        /// </summary>
        /// <param name="e">event arguments</param>
        protected override void OnInit(EventArgs e)
        {
            if (Logger.IsDebugEnabled)
            {
                Logger.DebugFormat("OnInit of {0}.aspx", this.GetType().BaseType.Name);
            }

            base.OnInit(e);

            if (!Page.IsPostBack)
            {
                GotoTop();
            }

            //Base style to meet the WCAG 2.0, should be placed at the first.
            AddDefaultAccessibilityCssStyle();
            AddSocialMediaCssStyle();

            //To meet the high contrast in WCAG 2.0, should be placed at the end.
            AddAccessibilityCssStyle();

            AddCustomizedCssStyle();

            if (StandardChoiceUtil.IsEnableCustomizationPerPage())
            {
                AddCustomizedCssAndScript("GlobalCustomScriptBefore");
            }
        }

        /// <summary>
        /// On PreRender Complete Event.
        /// </summary>
        /// <param name="e">The event arguments.</param>
        protected override void OnPreRenderComplete(EventArgs e)
        {
            base.OnPreRenderComplete(e);

            // Add Page Customization File(css and javascript)
            if (StandardChoiceUtil.IsEnableCustomizationPerPage())
            {
                string path = Request.Url.LocalPath;

                if (path.EndsWith(".aspx", StringComparison.OrdinalIgnoreCase))
                {
                    // Remove extention .aspx and application path
                    path = path.Replace(".aspx", string.Empty).Substring(Request.ApplicationPath.Length);

                    AddCustomizedCssAndScript(path);
                    AddCustomizedCssAndScript("GlobalCustomScriptAfter");
                }
            }
        }

        /// <summary>
        /// On load event.
        /// </summary>
        /// <param name="e">The event arguments.</param>
        protected override void OnLoad(EventArgs e)
        {
            // The validate function move from OnInit to OnLoad, because if postback sometimes it need the value from page to validate.
            if (!AppSession.IsAdmin && IsForceLogin)
            {
                ForceLoginValidation();
            }

            base.OnLoad(e);
        }

        /// <summary>
        /// on Pre Initial event
        /// </summary>
        /// <param name="e">event arguments</param>
        protected override void OnPreInit(EventArgs e)
        {
            ProductLicenseValidation.ValidateProductLicense(false);

            // When login admin site, all daily pages must contain isAdmin in url. Login admin then enter daily directly.
            if (AppSession.IsAdmin && !ACAConstant.COMMON_Y.Equals(Request.QueryString["isAdmin"], StringComparison.InvariantCultureIgnoreCase))
            {
                // FileUpload doesn't need to change session since it is in IFrame. otherwise in admin page when clicking record detail page,the session is set to daily
                if (Request.Url.AbsoluteUri.IndexOf("/FileUpload/", StringComparison.InvariantCultureIgnoreCase) == -1 && Request.Url.AbsoluteUri.IndexOf("Error.aspx", StringComparison.InvariantCultureIgnoreCase) == -1)
                {
                    // in daily mode forece session back to daily mode from admin
                    AppSession.IsAdmin = false;
                }
            }

            ValidateXSS();

            ClearQuerySession();
            ChangeTheme();
            ChangeMasterPage();
            HandleAnonymousSession();

            //set value of public user ID for soap header.
            I18nSoapHeaderExtension.CurrentUser = AppSession.IsAdmin ? ACAConstant.ADMIN_CALLER_ID : AppSession.User == null ? string.Empty : AppSession.User.PublicUserId;

            //For security, all pages should not be cached for logined user
            if (AppSession.User != null && !AppSession.User.IsAnonymous && !AppSession.IsAdmin)
            {
                Response.Cache.SetCacheability(System.Web.HttpCacheability.NoCache);
                Response.Cache.SetNoStore();
            }

            RecordUrl();
        }

        /// <summary>
        /// OnPreRender event
        /// </summary>
        /// <param name="e">event arguments</param>
        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);
            string target = Request.Params["__EVENTTARGET"];
            string args = Request.Params["__EVENTARGUMENT"];

            if (!string.IsNullOrEmpty(target) && target.EndsWith("4btnExport", StringComparison.OrdinalIgnoreCase) && string.IsNullOrEmpty(args))
            {
                string[] ids = target.Split('$');
                string clientId = target.Replace("$" + ids[ids.Length - 1], string.Empty).Replace("$", "_");
                AccelaGridView grid = GetGridViewByID(clientId, Controls);

                if (grid != null)
                {
                    grid.Export();
                }
            }
            else if (!string.IsNullOrEmpty(target) && target.EndsWith("4btnExport", StringComparison.OrdinalIgnoreCase) && !string.IsNullOrEmpty(args))
            {
                // the target string must match global_search_CAP or global_search_LP or global_search_PARCEL
                string[] splitArray = args.Split(ACAConstant.SPLIT_CHAR4URL1);

                if (splitArray.Length == 2)
                {
                    string gridId = splitArray[0];
                    string searchType = splitArray[1];
                    AccelaGridView grid = GetGridViewByID(gridId, Controls);
                    string[] hiddenViewElementNames = ControlBuildHelper.GetHiddenViewElementNames(grid.GridViewNumber, ModuleName);
                    GlobalSearchManager.Export(grid, searchType, hiddenViewElementNames);
                }
            }
            else if (!string.IsNullOrEmpty(target) && target.EndsWith("4btnShortList", StringComparison.OrdinalIgnoreCase) && string.IsNullOrEmpty(args))
            {
                string[] ids = target.Split('$');
                string clientId = target.Replace("$" + ids[ids.Length - 1], string.Empty).Replace("$", "_");
                AccelaGridView grid = GetGridViewByID(clientId, Controls);

                if (grid != null)
                {
                    grid.ShortListAction();
                }
            }

            if (StandardChoiceUtil.IsEnableBrowserDetect())
            {
                BrowserDetectUtil.Detect(ModuleName, Session, Page);
            }
        }

        /// <summary>
        /// record url
        /// </summary>
        protected virtual void RecordUrl()
        {
            if (AppSession.IsAdmin)
            {
                Session[ACAConstant.CURRENT_URL] = null;
            }
            else
            {
                string hideHeader = Request["HideHeader"];

                if (!string.Equals(hideHeader, ACAConstant.COMMON_TRUE, StringComparison.CurrentCultureIgnoreCase))
                {
                    Session[ACAConstant.CURRENT_URL] = Request.Url.AbsoluteUri;
                }
            }
        }

        /// <summary>
        /// Goto the login page.
        /// </summary>
        protected void GotoLoginPage()
        {
            string rawUrl = Request.RawUrl.ToLower();

            // the two page is in pop up window, so we need close the pop up window in the same time.
            if (rawUrl.IndexOf("showreport.aspx") >= 0 || rawUrl.IndexOf("reportparameter.aspx") >= 0)
            {
                string scripts = "<script>window.opener.location.href='" + AuthenticationUtil.LoginUrl + "';window.close();</script>";
                Response.Write(scripts);
                Response.End();
            }
            else
            {
                AuthenticationUtil.RedirectToLoginPage();
            }
        }

        /// <summary>
        /// check if force login
        /// </summary>
        /// <param name="url">the url from request or special one.</param>
        /// <returns>true if force login</returns>
        protected virtual bool IsFeatureForceLogin(string url)
        {
            IBizDomainBll bizDomain = (IBizDomainBll)ObjectFactory.GetObject(typeof(IBizDomainBll));
            return bizDomain.IsForceLogin(ModuleName, url, null);
        }

        /// <summary>
        /// If refresh the current page, the Request.UrlReferrer.LocalPath will change to <c>"/" or "/default.aspx"</c>.
        /// The <c>"/" or "/default.aspx"</c> is the default patch of the start page on current Accela City Access site.
        /// </summary>
        /// <returns>Indicating whether it is refresh on current page or not.</returns>
        protected bool IsRefreshOnCurrentpage()
        {
            bool isRefresh =
                    Request.UrlReferrer != null
                    && (Request.UrlReferrer.LocalPath.EndsWith("/", StringComparison.InvariantCultureIgnoreCase)
                        || Request.UrlReferrer.LocalPath.EndsWith("/default.aspx", StringComparison.InvariantCultureIgnoreCase));

            return isRefresh;
        }

        /// <summary>
        /// clear permit and apo query session
        /// </summary>
        private void ClearQuerySession()
        {
            string url = Request.RawUrl.ToLowerInvariant();

            if (url.IndexOf("Error.aspx", StringComparison.InvariantCultureIgnoreCase) > -1)
            {
                return;
            }

            if (Session[SessionConstant.SESSION_PERMITQUERYINFO] != null)
            {
                if (url.IndexOf("capedit.aspx", StringComparison.InvariantCultureIgnoreCase) == -1
                    && url.IndexOf("capdetail.aspx", StringComparison.InvariantCultureIgnoreCase) == -1
                    && url.IndexOf("caphome.aspx", StringComparison.InvariantCultureIgnoreCase) == -1
                    && url.IndexOf("fileuploadpage.aspx", StringComparison.InvariantCultureIgnoreCase) == -1
                    && url.IndexOf("tradenamedetail.aspx", StringComparison.InvariantCultureIgnoreCase) == -1
                    && url.IndexOf("attachmentslist.aspx", StringComparison.InvariantCultureIgnoreCase) == -1
                    && url.IndexOf("reportparameter.aspx", StringComparison.InvariantCultureIgnoreCase) == -1
                    && url.IndexOf("relatedrecords.aspx", StringComparison.InvariantCultureIgnoreCase) == -1)
                {
                    Session[SessionConstant.SESSION_PERMITQUERYINFO] = null;
                }
            }

            if (Session[SessionConstant.SESSION_APO_QUERY] != null)
            {
                if (url.IndexOf("addressdetail.aspx", StringComparison.InvariantCultureIgnoreCase) == -1
                    && url.IndexOf("apolookup.aspx", StringComparison.InvariantCultureIgnoreCase) == -1
                    && url.IndexOf("ownerdetail.aspx", StringComparison.InvariantCultureIgnoreCase) == -1
                    && url.IndexOf("parceldetail.aspx", StringComparison.InvariantCultureIgnoreCase) == -1
                    && url.IndexOf("reportparameter.aspx", StringComparison.InvariantCultureIgnoreCase) == -1
                    && url.IndexOf("RecordByGISObject.aspx", StringComparison.InvariantCultureIgnoreCase) == -1
                    && url.IndexOf("ShowGeoDocuments.aspx", StringComparison.InvariantCultureIgnoreCase) == -1
                    && url.IndexOf("CapApplyDisclaimer.aspx", StringComparison.InvariantCultureIgnoreCase) == -1
                    && url.IndexOf("CapDetail.aspx", StringComparison.InvariantCultureIgnoreCase) == -1
                    && url.IndexOf("CapEdit.aspx", StringComparison.InvariantCultureIgnoreCase) == -1
                    && url.IndexOf("CapType.aspx", StringComparison.InvariantCultureIgnoreCase) == -1
                    && url.IndexOf("CapFees.aspx", StringComparison.InvariantCultureIgnoreCase) == -1)
                {
                    Session[SessionConstant.SESSION_APO_QUERY] = null;
                }
            }

            if (Session[SessionConstant.SESSION_EDUCATION_QUERYINFO] != null)
            {
                if (url.IndexOf("LicenseeDetail.aspx", StringComparison.InvariantCultureIgnoreCase) == -1
                    && url.IndexOf("FoodFacilityInspectionDetail", StringComparison.InvariantCultureIgnoreCase) == -1
                    && url.IndexOf("CertifiedBusinessDetail", StringComparison.InvariantCultureIgnoreCase) == -1
                    && url.IndexOf("ContinuingEducationDetail.aspx", StringComparison.InvariantCultureIgnoreCase) == -1
                    && url.IndexOf("EducationDetail.aspx", StringComparison.InvariantCultureIgnoreCase) == -1
                    && url.IndexOf("ExaminationDetail.aspx", StringComparison.InvariantCultureIgnoreCase) == -1
                    && url.IndexOf("ProviderDetail.aspx", StringComparison.InvariantCultureIgnoreCase) == -1
                    && url.IndexOf("AttachmentsList.aspx", StringComparison.InvariantCultureIgnoreCase) == -1
                    && url.IndexOf("FileUploadPage.aspx", StringComparison.InvariantCultureIgnoreCase) == -1
                    && url.IndexOf("capdetail.aspx", StringComparison.InvariantCultureIgnoreCase) == -1
                    && url.IndexOf("attachmentslist.aspx", StringComparison.InvariantCultureIgnoreCase) == -1
                    && url.IndexOf("PropertyLookUp.aspx", StringComparison.InvariantCultureIgnoreCase) == -1
                    && url.IndexOf("reportparameter.aspx", StringComparison.InvariantCultureIgnoreCase) == -1)
                {
                    Session[SessionConstant.SESSION_EDUCATION_QUERYINFO] = null;
                }
            }
        }

        /// <summary>
        /// do anonymous login
        /// </summary>
        private void DoAnonymousLogin()
        {
            try
            {
                AccountUtil.CreateUserContext(AccountUtil.MakeAnonymousUser());
            }
            catch (ACAException ex)
            {
                MessageUtil.ShowMessage(Page, MessageType.Error, ex.Message);
            }
        }

        /// <summary>
        /// Judge Access right and login statues of the feature.
        /// </summary>
        private void ForceLoginValidation()
        {
            // if feature need to force login, and user have not login, redirect to login page else do nothing.
            bool isLogin = AuthenticationUtil.IsAuthenticated;

            if (!isLogin && IsFeatureForceLogin(Request.RawUrl))
            {
                GotoLoginPage();
            }
        }

        /// <summary>
        /// Get GridView by id
        /// </summary>
        /// <param name="id">the child control id</param>
        /// <param name="controls">control collection</param>
        /// <returns>the GridView control</returns>
        private AccelaGridView GetGridViewByID(string id, ControlCollection controls)
        {
            AccelaGridView grid = null;
            foreach (Control control in controls)
            {
                if (control is AccelaGridView && ((AccelaGridView)control).ClientID == id)
                {
                    grid = (AccelaGridView)control;
                    break;
                }
                else if (control.Controls.Count > 0)
                {
                    grid = GetGridViewByID(id, control.Controls);

                    if (grid != null)
                    {
                        break;
                    }
                }
            }

            return grid;
        }

        /// <summary>
        /// handle anonymous session
        /// </summary>
        private void HandleAnonymousSession()
        {
            /*
            * This logic to handle this kind of scenario - the user is signed in but the user session is timed out.
            * But if ACA is integrated with SSO, typically the AppSession.User will be initialized in IAuthAdapter.IsAuthenticated property,
            *  so we changed the code logic as below.
            *  the logic will determine the IsAuthenticated flag first and then to determine if the AppSession.User is null to prevent some SSO logic error.
            */
            if (AuthenticationUtil.IsAuthenticated && AppSession.User == null)
            {
                string path = Request.Url.AbsolutePath;

                if (path.IndexOf("login.aspx", StringComparison.InvariantCultureIgnoreCase) != -1
                    || path.IndexOf("error.aspx", StringComparison.InvariantCultureIgnoreCase) != -1)
                {
                    return;
                }

                GotoLoginPage();
            }
            else if (AppSession.User == null)
            {
                // Current request is not authenticated and User session is null, initial current request as an Anonymous user.
                DoAnonymousLogin();
            }
        }

        /// <summary>
        /// validate XSS
        /// </summary>
        private void ValidateXSS()
        {
            string queryString = Request.QueryString.ToString();

            if (string.IsNullOrEmpty(queryString))
            {
                return;
            }
         
            queryString = System.Web.HttpUtility.UrlDecode(queryString);

            IList<string> contentArray = new List<string>();
            contentArray.Add(queryString);

            if (ScriptFilter.IsUnSafeData(contentArray))
            {
                throw new InvalidDataException(this.GetTextByKey("aca_unsafe_data_warning_msg"));
            }
        }

        /// <summary>
        /// Append customized CSS style in the header of master page
        /// </summary>
        private void AddCustomizedCssStyle()
        {
            AddCssStyleFile("CustomerCssStyle", string.Format("{0}{1}{2}{3}{4}",  "~/Handlers/CustomizedCssStyle.ashx", ACAConstant.QUESTION_MARK, UrlConstant.AgencyCode, ACAConstant.EQUAL_MARK, ScriptFilter.AntiXssHtmlAttributeEncode(ConfigManager.AgencyCode)));
        }

        /// <summary>
        /// Adds the social media CSS style.
        /// </summary>
        private void AddSocialMediaCssStyle()
        {
            if (!string.IsNullOrEmpty(ConfigManager.FacebookAppId))
            {
                AddCssStyleFile("SocialMediaCssStyle", "~/App_Themes/SocialMedia/socialmedia.css");
                AddCssStyleFile("FacebookCssStyle", "~/App_Themes/SocialMedia/Facebook.css");
            }
        }

        /// <summary>
        /// Adds the default CSS style for accessibility support.
        /// </summary>
        private void AddDefaultAccessibilityCssStyle()
        {
            if (AccessibilityUtil.AccessibilityEnabled)
            {
                InsertCssStyleFile("DefaultAccessibilityCssStyle", "~/App_Themes/Accessibility/AccessibilityDefault.css", true);
            }
        }

        /// <summary>
        /// Adds the CSS style for accessibility support.
        /// </summary>
        private void AddAccessibilityCssStyle()
        {
            if (AccessibilityUtil.AccessibilityEnabled)
            {
                AddCssStyleFile("AccessibilityCssStyle", "~/App_Themes/Accessibility/Accessibility.css");
            }
        }

        /// <summary>
        /// Add CSS style file in page head.
        /// </summary>
        /// <param name="id">control Id</param>
        /// <param name="fileName">CSS style file name</param>
        private void AddCssStyleFile(string id, string fileName)
        {
            InsertCssStyleFile(id, fileName, false);
        }

        /// <summary>
        /// Add CSS style file in page head.
        /// </summary>
        /// <param name="id">control Id</param>
        /// <param name="fileName">CSS style file name</param>
        /// <param name="isInsertToFirst">whether insert the file in the first place of all CSS styles.</param>
        private void InsertCssStyleFile(string id, string fileName, bool isInsertToFirst)
        {
            HtmlGenericControl link = new HtmlGenericControl("link");
            link.ID = id;
            link.Attributes["style"] = "text/css";
            link.Attributes["rel"] = "stylesheet";
            link.Attributes["href"] = Page.ResolveUrl(fileName);

            HtmlHead header;

            if (this.Master != null)
            {
                header = this.Master.FindControl("Head1") as HtmlHead;
            }
            else
            {
                header = this.Header;
            }

            if (header != null && header.FindControl(link.ID) == null)
            {
                if (isInsertToFirst)
                {
                    header.Controls.AddAt(0, link);
                }
                else
                {
                    header.Controls.Add(link);
                }
            }
        }

        /// <summary>
        /// Add Custom Javascript and CSS Style Sheet
        /// </summary>
        /// <param name="path">The path</param>
        private void AddCustomizedCssAndScript(string path)
        {
            // Default Language
            CustomizationType customizationType = FileUtil.GetCustomizationType(path, string.Empty);
            AddPageCustomizedCssAndScript(customizationType, path, string.Empty);

            // Support I18N
            if (!string.IsNullOrWhiteSpace(I18nCultureUtil.UserPreferredCulture))
            {
                customizationType = FileUtil.GetCustomizationType(path, I18nCultureUtil.UserPreferredCulture);
                AddPageCustomizedCssAndScript(customizationType, path, I18nCultureUtil.UserPreferredCulture);
            }
        }

        /// <summary>
        /// Add Customized CSS StyleSheet and javascript
        /// </summary>
        /// <param name="customizationType">Customization Type</param>
        /// <param name="path">page path</param>
        /// <param name="culture">The culture</param>
        private void AddPageCustomizedCssAndScript(CustomizationType customizationType, string path, string culture)
        {
            string customizationDir = ConfigManager.CustomizationDirectory;

            if (!string.IsNullOrWhiteSpace(culture))
            {
                culture = ACAConstant.SPLIT_CHAR4 + I18nCultureUtil.UserPreferredCulture;
            }

            // Link css style sheet
            if ((customizationType & CustomizationType.Css) == CustomizationType.Css)
            {
                AddCssStyleFile("PageCustomizeCssStyle_" + path + culture, "~/" + customizationDir + "/" + path.ToLower() + culture + ".css");
            }

            // Include javascript
            if ((customizationType & CustomizationType.Javascript) == CustomizationType.Javascript)
            {
                ScriptManager.RegisterClientScriptInclude(Page, GetType(), "PageCustomizeJavascript_" + path + culture, Page.ResolveUrl("~/" + customizationDir + "/" + path.ToLower() + culture + ".js"));
            }
        }

        #endregion Methods
    }
}
