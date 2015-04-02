#region Header
/**
 * <pre>
 * 
 *  Accela Citizen Access
 *  File: Global.asax.cs
 * 
 *  Accela, Inc.
 *  Copyright (C): 2013-2014
 * 
 *  Description: Handle events of the Web Application life cycle.
 * </pre>
 */
#endregion

using System;
using System.Globalization;
using System.Text;
using System.Threading;
using System.Web;
using System.Web.Http;
using System.Web.Routing;
using System.Web.SessionState;
using Accela.ACA.BLL.Attachment;
using Accela.ACA.BLL.Common;
using Accela.ACA.Common;
using Accela.ACA.Common.Common;
using Accela.ACA.Common.Util;
using Accela.ACA.Web.Common;
using Accela.ACA.Web.Util;
using log4net;

namespace Accela.ACA.Web
{
    /// <summary>
    /// Handle the Web Application life cycle.
    /// </summary>
    public class Global : HttpApplication
    {
        /// <summary>
        /// Log instance.
        /// </summary>
        private static readonly ILog Log = LogManager.GetLogger(typeof(Global));

        #region Application events

        /// <summary>
        /// Web API use session state
        /// </summary>
        public override void Init()
        {
            this.PostAuthenticateRequest += (sender, e) => HttpContext.Current.SetSessionStateBehavior(SessionStateBehavior.Required);
            base.Init();
        }

        /// <summary>
        /// Handle the Application Start event.
        /// </summary>
        /// <param name="sender">Trigger object.</param>
        /// <param name="e">Event arguments.</param>
        protected void Application_Start(object sender, EventArgs e)
        {
            // For Web Api.
            RouteTable.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{action}/{id}",
                defaults: new { action = RouteParameter.Optional, id = RouteParameter.Optional });

            /*
             * Copy customize dlls to bin folder.
             * The copy function need lay in the first place. Because the dll may mapping config use the spring framework.
             */
            string sourcePath = Server.MapPath(FileUtil.ApplicationRoot + "Customize/DLL");
            string destPath = Server.MapPath(FileUtil.ApplicationRoot + "Bin");
            FileUtil.CopyFilesInDirectory(sourcePath, destPath);

            // Initialize log4net configuration.
            log4net.Config.XmlConfigurator.Configure();

            // Start async upload timer.
            IEDMSDocumentBll documentBll = ObjectFactory.GetObject<IEDMSDocumentBll>();
            documentBll.StartAsyncUploadTimer();

            // Load global cache.
            ICacheManager cacheManager = ObjectFactory.GetObject<ICacheManager>();
            cacheManager.LoadGlobalCache();
        }

        /// <summary>
        /// Handle the Application BeginRequest event.
        /// </summary>
        /// <param name="sender">Trigger object.</param>
        /// <param name="e">Event arguments.</param>
        protected void Application_BeginRequest(object sender, EventArgs e)
        {
            /*
             * Set Culture info for all Request Thread.
             * Move the Thread Culture to here.Because the web service need set the Culture. In basepage.cs don't support it.
             */
            CultureInfo cultureInfo = CultureInfo.CreateSpecificCulture(I18nCultureUtil.UserPreferredCulture);
            Thread.CurrentThread.CurrentCulture = cultureInfo;
            Thread.CurrentThread.CurrentUICulture = cultureInfo;

            //Update the lastest request time.
            SessionTimeoutUtil.UpdateLastestRequestTime(Request, Response);
        }

        /// <summary>
        /// Handle the Application Acquire Request State event.
        /// </summary>
        /// <param name="sender">Trigger object.</param>
        /// <param name="e">Event arguments.</param>
        protected void Application_AcquireRequestState(object sender, EventArgs e)
        {
            // Create a anonymous user context if current user context is null.
            HandleAnonymousSession();
        }

        /// <summary>
        /// Handle the Application PreSendRequestHeaders event.
        /// </summary>
        /// <param name="sender">Trigger object.</param>
        /// <param name="e">Event arguments.</param>
        protected void Application_PreSendRequestHeaders(object sender, EventArgs e)
        {
            /*
             * Set SSL for all cookies according to current http protocol.
             * Also can use requireSSL property to set all cookies' SSL status in system.web/httpCookies section in web.config,
             *     but there cannot to according to current protocol to set them.
             */
            if (Response.Cookies.Count > 0)
            {
                bool isSSL = "HTTPS".Equals(Request.Url.Scheme, StringComparison.OrdinalIgnoreCase);

                foreach (string cookieName in Response.Cookies)
                {
                    HttpCookie cookie = Response.Cookies[cookieName];
                    cookie.Secure = isSSL;

                    if (Log.IsDebugEnabled)
                    {
                        Log.DebugFormat("Cookie [{0}], [Secure] is {1}", cookieName, cookie.Secure);
                    }
                }
            }
        }

        /// <summary>
        /// Handle the Application End event.
        /// </summary>
        /// <param name="sender">Trigger object.</param>
        /// <param name="e">Event arguments.</param>
        protected void Application_End(object sender, EventArgs e)
        {
            //Stop async upload timer.
            IEDMSDocumentBll documentBll = ObjectFactory.GetObject<IEDMSDocumentBll>();
            documentBll.StopAsyncUploadTimer();
        }

        /// <summary>
        /// Handle the Application Error event.
        /// </summary>
        /// <param name="sender">Trigger object.</param>
        /// <param name="e">Event arguments.</param>
        protected void Application_Error(object sender, EventArgs e)
        {
            Exception exception = Server.GetLastError();
            string errMessage = null;

            if (exception != null)
            {
                exception = exception.GetBaseException();
                Server.ClearError();
            }

            if (exception != null)
            {
                StringBuilder sb = new StringBuilder();
                sb.Append(exception.Message);
                sb.Append("\r\nSOURCE: " + exception.Source);
                sb.Append("\r\nTARGETSITE: " + exception.TargetSite);
                sb.Append("\r\nSTACKTRACE: " + exception.StackTrace);

                string detailedErrMessage = sb.ToString();
                Log.Error(detailedErrMessage);

                errMessage = exception.Message;

                if (Request.IsLocal)
                {
                    errMessage = detailedErrMessage;
                }
            }

            if (!string.IsNullOrEmpty(errMessage))
            {
                errMessage = errMessage.Replace("\r\n", ACAConstant.HTML_BR);
                string requestPath = Request.Url.AbsolutePath;

                if (requestPath.EndsWith("Error.aspx", StringComparison.OrdinalIgnoreCase)
                    || requestPath.EndsWith("Error4Popup.aspx", StringComparison.OrdinalIgnoreCase))
                {
                    Response.Write(errMessage);
                }
                else
                {
                    // Keep the error message in cache for 60 seconds for pass it to error dipslay page.
                    ICacheManager cacheManager = ObjectFactory.GetObject<ICacheManager>();
                    string errorId = CommonUtil.GetRandomUniqueID("N");
                    cacheManager.AddSingleItemToCache(errorId, errMessage, 60);

                    string errorUrl = "~/Error.aspx?ErrorId=" + errorId;

                    if (ValidationUtil.IsYes(Request[UrlConstant.IS_POPUP_PAGE]))
                    {
                        errorUrl = "~/Error4Popup.aspx?ErrorId=" + errorId + "&" + UrlConstant.IS_POPUP_PAGE + "="
                                   + ACAConstant.COMMON_Y;
                    }

                    try
                    {
                        Response.Redirect(errorUrl);
                    }
                    catch
                    {
                    }
                }
            }
        }

        #endregion

        #region Session events

        /// <summary>
        /// Handle the Session Start event.
        /// </summary>
        /// <param name="sender">Trigger object.</param>
        /// <param name="e">Event arguments.</param>
        protected void Session_Start(object sender, EventArgs e)
        {
            // Setup I18n Settings
            StandardChoiceUtil.SetupI18nInitialSettings();

            // Initialize the customize permission
            string configPath = Server.MapPath(FileUtil.GetCustomizeMappingConfigPath());
            FunctionTable.InitCustomizePermission(configPath);
        }

        /// <summary>
        /// Handle anonymous session
        /// </summary>
        private void HandleAnonymousSession()
        {
            try
            {
                if (HttpContext.Current.Session != null && AppSession.User == null)
                {
                    AccountUtil.CreateUserContext(AccountUtil.MakeAnonymousUser());
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex.ToString());
            }
        }

        #endregion
    }
}