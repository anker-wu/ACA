#region Header

/**
* <pre>
*  Accela Citizen Access
*  File: UrlRoutingHandler.cs
*
*  Accela, Inc.
*  Copyright (C): 2011-2014
*
*  Description: Url routing http handler.
*
*  Notes:
* $Id: FileUploadHandler.ashx.cs 171222 2010-04-21 16:10:00Z ACHIEVO\alan.hu $.
*  Revision History
*  Date,            Who,        What
*  Sep 23, 2011      Alan Hu     Initial.
* </pre>
*/

#endregion

using System.Configuration;
using System.Web;
using System.Web.SessionState;

using Accela.ACA.BLL.Common;
using Accela.ACA.Common;
using Accela.ACA.Common.Util;
using Accela.ACA.Web.Common;

namespace Accela.ACA.Web.Handlers
{
    /// <summary>
    /// routing url path
    /// </summary>
    public class UrlRoutingHandler : IHttpHandler, IHttpHandlerCommon, IRequiresSessionState
    {
        #region Properties

        /// <summary>
        /// Gets a value indicating whether the handler can be reused.
        /// </summary>
        public bool IsReusable
        {
            get
            {
                return false;
            }
        }

        /// <summary>
        /// Gets or sets the config section.
        /// </summary>
        /// <value>The config section.</value>
        public HttpHandlerConfigObject ConfigObject { get; set; }

        /// <summary>
        /// Gets the request.
        /// </summary>
        /// <value>The http request.</value>
        protected HttpRequest Request
        {
            get { return HttpContext.Current.Request; }
        }

        /// <summary>
        /// Gets the response.
        /// </summary>
        /// <value>The http response.</value>
        protected HttpResponse Response
        {
            get { return HttpContext.Current.Response; }
        }

        /// <summary>
        /// Gets the agency code.
        /// </summary>
        /// <value>The agency code.</value>
        protected string AgencyCode
        {
            get
            {
                return Request.QueryString[UrlConstant.AgencyCode];
            }
        }

        /// <summary>
        /// Gets the cap id1.
        /// </summary>
        /// <value>The cap id1.</value>
        protected string CapId1
        {
            get
            {
                return Request.QueryString["capID1"];
            }
        }

        /// <summary>
        /// Gets the cap id2.
        /// </summary>
        /// <value>The cap id2.</value>
        protected string CapId2
        {
            get
            {
                return Request.QueryString["capID2"];
            }
        }

        /// <summary>
        /// Gets the cap id3.
        /// </summary>
        /// <value>The cap id3.</value>
        protected string CapId3
        {
            get
            {
                return Request.QueryString["capID3"];
            }
        }

        /// <summary>
        /// Gets the name of the module.
        /// </summary>
        /// <value>The name of the module.</value>
        protected string ModuleName
        {
            get
            {
                return Request.QueryString["Module"];
            }
        }

        /// <summary>
        /// Gets Current User Sequence Number
        /// </summary>
        protected string UserSeqNum
        {
            get
            {
                return (AppSession.User == null || string.IsNullOrEmpty(AppSession.User.UserSeqNum)) ? ACAConstant.ANONYMOUS_FLAG : AppSession.User.UserSeqNum;
            }
        }

        /// <summary>
        /// Gets current request root path ends with "/"
        /// </summary>
        protected string RootPath
        {
            get
            {
                string rootPath = string.Format("{0}://{1}{2}", ConfigManager.Protocol, Request.Url.Authority, Request.ApplicationPath);

                if (!rootPath.EndsWith("/"))
                {
                    rootPath += "/";
                }

                return rootPath;
            }
        }

        #endregion

        #region Method

        /// <summary>
        /// Tries the validation.
        /// </summary>
        /// <param name="type">The handler type.</param>
        /// <returns>validation parameters successful</returns>
        public bool ValidateParams(ServiceType type)
        {
            bool isValidation = IsParamsValidated();

            if (!isValidation)
            {
                RedirectToErrorPage(LabelUtil.GetGUITextByKey("aca_urlrouting_label_invalid_urlparameters"));
            }

            // validat the culture parameter
            if (!I18nCultureUtil.ValidateCulture(Request.QueryString["culture"]))
            {
                RedirectToErrorPage(string.Format(LabelUtil.GetGUITextByKey("aca_common_label_invalidculture"), Request.QueryString["culture"]));
            }

            return isValidation;
        }

        /// <summary>
        /// Enables processing of HTTP Web requests by a custom HttpHandler that implements the <see cref="T:System.Web.IHttpHandler"/> interface.
        /// </summary>
        /// <param name="context">An <see cref="T:System.Web.HttpContext"/> object that provides references to the intrinsic server objects (for example, Request, Response, Session, and Server) used to service HTTP requests.</param>
        public virtual void ProcessRequest(HttpContext context)
        {
            try
            {
                if (ConfigObject != null)
                {
                    // set culture
                    string culture = context.Request.QueryString["culture"];

                    if (!string.IsNullOrWhiteSpace(culture))
                    {
                        I18nCultureUtil.UserPreferredCulture = culture;
                    }

                    string defaultPath = string.Format("{0}://{1}{2}", ConfigManager.Protocol, Request.Url.Authority, Request.ApplicationPath);

                    if (!defaultPath.EndsWith("/"))
                    {
                        defaultPath += "/";
                    }

                    string capdetailpath = ConfigObject.Url;

                    if (ConfigObject.Url.StartsWith("~/"))
                    {
                        capdetailpath = capdetailpath.Replace("~/", defaultPath);
                    }

                    capdetailpath = string.Format("{0}?{1}", capdetailpath, context.Request.QueryString);

                    HttpContext.Current.Session[ACAConstant.CURRENT_URL] = capdetailpath;
                    Response.Redirect(defaultPath + ConfigurationManager.AppSettings["DefaultPageFile"]);
                }
            }
            catch (ACAException ex)
            {
                RedirectToErrorPage(ex.Message);
            }
        }

        /// <summary>
        /// Set culture 
        /// </summary>
        protected void SetContextCulture()
        {
            // set culture
            string culture = Request.QueryString["culture"];

            if (!string.IsNullOrWhiteSpace(culture))
            {
                I18nCultureUtil.UserPreferredCulture = culture;
            }
        }

        /// <summary>
        /// Redirect to target url 
        /// </summary>
        /// <param name="redirectUrl">target url</param>
        protected void RedirectToURL(string redirectUrl)
        {
            redirectUrl = UrlHelper.CombineQueryString(redirectUrl, Request.QueryString.ToString());

            if (redirectUrl.StartsWith("~/"))
            {
                redirectUrl = redirectUrl.Replace("~/", RootPath);
            }

            UrlHelper.RedirectForDeepLink(redirectUrl);
        }

        /// <summary>
        /// Get defined redirect URL template which is defined in UrlRoutingHandler.config.
        /// </summary>
        /// <returns>Full URL path</returns>
        protected string GetUrlTemplate()
        {
            return ConfigObject.Url.Replace("~/", RootPath);
        }

        /// <summary>
        ///  Not Need validate(CapId1, CapId2, CapId3,ModuleName)
        /// </summary>
        /// <returns>Pass return true. Else redirect error page</returns>
        protected virtual bool IsParamsValidated()
        {
            return true;
        }

        /// <summary>
        /// Tries the validation.
        /// </summary>
        /// <param name="parameters">parameters list</param>
        /// <returns>validation parameters successful</returns>
        protected bool ValidateParams(params string[] parameters)
        {
            foreach (var parameter in parameters)
            {
                if (string.IsNullOrEmpty(parameter))
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Go to error page
        /// </summary>
        /// <param name="errorMessage">Error Message</param>
        protected void RedirectToErrorPage(string errorMessage)
        {
            errorMessage = string.IsNullOrEmpty(errorMessage) ? WebConstant.ExceptionUtilDefaultValue : errorMessage;

            // Keep the error message in cache for 60 seconds for pass it to error dipslay page.
            ICacheManager cacheManager = ObjectFactory.GetObject<ICacheManager>();
            string errorId = CommonUtil.GetRandomUniqueID("N");
            cacheManager.AddSingleItemToCache(errorId, errorMessage, 60);

            UrlHelper.RedirectForDeepLink(string.Format("Error.aspx?ErrorId={0}", errorId));
        }

        #endregion
    }
}