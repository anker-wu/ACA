#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: RedirectModule.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2009-2014
 *
 *  Description:
 *  
 *
 *  Notes:
 *      $Id: RedirectModule.cs 2009-12-30 02:52:17Z ACHIEVO\weiky.chen $.
 *  Revision History
 *  <Date>,    <Who>,    <What>
 * </pre>
 */

#endregion Header

using System;
using System.Configuration;
using System.Web;
using Accela.ACA.Common.Log;
using log4net;

namespace Accela.ACA.Web.Common
{
    /// <summary>
    /// use to redirect http to https
    /// </summary>
    public class RedirectModule : IHttpModule
    {
        #region IHttpModule Members

        /// <summary>
        /// Logger instance.
        /// </summary>
        protected static readonly ILog Logger = LogFactory.Instance.GetLogger(typeof(RedirectModule));

        /// <summary>
        /// is https supported
        /// </summary>
        private static bool _httpsSupported = true; 

        /// <summary>
        /// release resource
        /// </summary>
        public void Dispose()
        {
        }

        /// <summary>
        /// Initializes the module by hooking the application's BeginRequest event if indicated by the config settings.
        /// </summary>
        /// <param name="context">The HttpApplication this module is bound to.</param>
        public void Init(HttpApplication context)
        {
            if (context != null)
            {
                context.AcquireRequestState += new EventHandler(Context_AcquireRequestState);
            }
        }

        /// <summary>
        /// redirect to https if current is http and needs to redirect
        /// </summary>
        /// <param name="sender">The sender object.</param>
        /// <param name="e">The event arguments.</param>
        private void Context_AcquireRequestState(object sender, EventArgs e)
        {
            HttpContext context = HttpContext.Current;
            string aspxPostFix = ".aspx/";
            string httpMethodType = "GET";
            int startPosition = context.Request.Path.LastIndexOf(aspxPostFix, StringComparison.CurrentCultureIgnoreCase);

            if (startPosition > 0 && httpMethodType.Equals(context.Request.HttpMethod.ToUpper()))
            {
                string result = context.Request.Path.Substring(startPosition + aspxPostFix.Length);

                if (result.Length > 0)
                {
                    string error = string.Format(LabelUtil.GetDefaultLanguageGlobalTextByKey("aca_httpmodule_msg_security"), result);
                    throw new InvalidOperationException(error);
                }
            }

            if (_httpsSupported && context.Request.Url.Scheme.ToLower() == "http" && ConfigurationManager.AppSettings["RediectToHTTPS"] == "true")
            {
                string previousUrl = context.Request.Url.AbsoluteUri;
                try
                {
                    string httpsUrl = "https://" + context.Request.Url.Authority + context.Request.Url.PathAndQuery;

                    // if system doesn't support https(e.g in deve degugging state), it should still use http but not break the application. 
                    context.Response.Redirect(httpsUrl, false);
                }
                catch (Exception ex)
                {
                    Logger.ErrorFormat("{0} {1}", "The web site doesn't support https, please check whether the key RediectToHTTPS is configed correctly in web.config.", ex);

                    // https doesn't be supported.so only http is allowed.
                    _httpsSupported = false;
                    context.Response.Redirect(previousUrl);
                }
                finally
                {
                    //Terminate current request.
                    context.Response.End();
                }
            }
        }
        #endregion
    }
}
