/**
 * <pre>
 * 
 *  Accela Citizen Access
 *  File: Default.aspx.cs
 * 
 *  Accela, Inc.
 *  Copyright (C): 2008-2014
 * 
 *  Description: Display work location information
 * 
 *  Notes:
 *      $Id: Default.aspx.cs 277316 2014-08-14 02:02:02Z ACHIEVO\james.shi $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

using System;
using System.Text.RegularExpressions;
using System.Web;
using Accela.ACA.BLL.APO;
using Accela.ACA.BLL.Common;
using Accela.ACA.BLL.Report;
using Accela.ACA.Common;
using Accela.ACA.Common.Log;
using Accela.ACA.Common.Util;
using Accela.ACA.Web.Common;
using Accela.ACA.Web.Security;

namespace ACA.Admin
{
    /// <summary>
    /// the class for ACA admin.
    /// </summary>
    public partial class Default : Accela.ACA.Web.AdminBasePage
    {
        #region Fields

        /// <summary>
        /// constant for v360 url.
        /// </summary>
        private const string V360_URL = "V360_URL";

        /// <summary>
        /// log4net Logger
        /// </summary>
        private static readonly log4net.ILog Logger = LogFactory.Instance.GetLogger(typeof(Default));

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
        /// Initializes a new instance of the <see cref="Default"/> class.
        /// </summary>
        public Default()
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
        /// Gets or sets V360 URL Default value.
        /// </summary>
        protected string V360URL { get; set; }

        #endregion Properties

        /// <summary>
        /// get section fields info by section id
        /// </summary>
        /// <param name="sectionInfo">section info contains section id and module name</param>
        /// <param name="permissionLevel">The permission level.</param>
        /// <param name="permissionValue">The permission value.</param>
        /// <returns>a json include the fields info</returns>
        [System.Web.Services.WebMethod(Description = "GetSectionFields", EnableSession = true)]
        public static string GetSectionFields(string sectionInfo, string permissionLevel, string permissionValue)
        {
            string[] sectionInfos = sectionInfo.Split(ACAConstant.SPLIT_CHAR);

            if (sectionInfos.Length != 3 && sectionInfos.Length != 4)
            {
                return null;
            }

            if (GviewID.ExistGenericTemplateViewIds.Contains(sectionInfos[1]) && !string.IsNullOrEmpty(permissionValue))
            {
                ITemplateBll templateBll = ObjectFactory.GetObject<ITemplateBll>();
                GenericTemplateEntityType entityType = GViewUtil.GetEntityType(sectionInfos[1]);
                string templateGroupCode = templateBll.GetTemplateAssociateASIGroup(entityType, permissionValue);

                if (!string.IsNullOrEmpty(templateGroupCode))
                {
                    permissionValue += ACAConstant.SPLIT_DOUBLE_COLON + templateGroupCode;
                }
            }

            IGviewBll gviewBll = ObjectFactory.GetObject<IGviewBll>();

            return gviewBll.GetSimpleViewElementByJsonFormat(ConfigManager.AgencyCode, sectionInfos[0], sectionInfos[1], sectionInfos[2], permissionLevel, permissionValue, AppSession.User.UserID);
        }

        /// <summary>
        /// Get reports by page ID.
        /// </summary>
        /// <param name="moduleName">module name</param>
        /// <param name="pageID">element id of current page</param>
        /// <returns>report json string</returns>
        [System.Web.Services.WebMethod(Description = "GetReports", EnableSession = true)]
        public static string GetReportsByPage(string moduleName, string pageID)
        {
            IReportBll reportBll = ObjectFactory.GetObject(typeof(IReportBll)) as IReportBll;
            return reportBll.GetReportsByPage(moduleName, pageID);
        }

        /// <summary>
        /// Get page size by GView id.
        /// </summary>
        /// <param name="moduleName">module name</param>
        /// <param name="gviewId">GView id</param>
        /// <returns>page size</returns>
        [System.Web.Services.WebMethod(Description = "GetPageSize", EnableSession = true)]
        public static string GetPageSizeByGviewId(string moduleName, string gviewId)
        {
            string pageSize = string.Empty;
            string pageSizeKey = string.Format("{0}_{1}", ACAConstant.ACA_PAGE_SIZE, gviewId);

            IXPolicyBll xPolicyBll = (IXPolicyBll)ObjectFactory.GetObject(typeof(IXPolicyBll));
            pageSize = xPolicyBll.GetValueByKey(pageSizeKey, moduleName);

            return pageSize;
        }

        /// <summary>
        /// Clear all caches.
        /// </summary>
        /// <param name="isLogOutAdmin">true or false.Only admin click Exit button to close ACA admin, the value is true</param>
        [System.Web.Services.WebMethod(Description = "ClearAllCache", EnableSession = true)]
        public static void ClearAllCache(bool isLogOutAdmin)
        {
            ICacheManager cacheManager = (ICacheManager)ObjectFactory.GetObject(typeof(ICacheManager));
            cacheManager.ClearCache(CacheConstant.CacheKeys);

            // In order imrove the performance of Checking URL Referrer, ACA Security Setting is initialized only when the request is scaned (or ACA initializes).
            // Here is one alternative way for ACA Admin to make any change on ACA_SECURITY_SETTING effect immediately without restarting ACA site.
            AntiCsrfAttackUtil.LoadSecuritySetting();

            AppSession.IsAdmin = !isLogOutAdmin;
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
                    Regex reg = new Regex("default.aspx", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant);
                    Response.Redirect(reg.Replace(Request.RawUrl, "Login.aspx"), false);
                    return;
                }

                //set value of public user ID for soap header.
                I18nSoapHeaderExtension.CurrentUser = ACAConstant.ADMIN_CALLER_ID;

                GetV360URL();
                this.Title = GetTextByKey("acc_admin_page_title");
            }
        }

        /// <summary>
        /// On Initialize event
        /// </summary>
        /// <param name="e">event arguments</param>
        protected override void OnInit(EventArgs e)
        {
            if (Logger.IsDebugEnabled)
            {
                Logger.DebugFormat("OnInit of {0}.aspx", this.GetType().BaseType.Name);
            }

            base.OnInit(e);
        }

        /// <summary>
        /// Get V360 URL
        /// </summary>
        private void GetV360URL()
        {
            HttpCookie cookie = Request.Cookies.Get(V360_URL);

            if (cookie != null)
            {
                V360URL = cookie.Value;
            }
        }
    }
}