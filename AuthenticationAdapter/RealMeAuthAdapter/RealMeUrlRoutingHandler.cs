#region Header

/**
* <pre>
*  Accela Citizen Access
*  File: RealMeUrlRoutingHandler.cs
*
*  Accela, Inc.
*  Copyright (C): 2014
*
*  Description: The common functions for RealMe  Url routing handler.
*
* </pre>
*/

#endregion

using System;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;
using Accela.ACA.SSOInterface;
using log4net;

namespace Accela.ACA.RealMeAccessGate
{
    /// <summary>
    /// Real me Url routing handler class
    /// </summary>
    public class RealMeUrlRoutingHandler : IHttpHandler, IRequiresSessionState
    {
        #region Fields

        /// <summary>
        /// _log object
        /// </summary>
        private static readonly ILog Log = LogFactory.Instance.GetLogger(typeof(RealMeAuthAdapter));

        /// <summary>
        /// The RealMe authorize adapter.
        /// </summary>
        private static readonly RealMeAuthAdapter RealMeAuthAdapter = new RealMeAuthAdapter();

        #endregion

        /// <summary>
        /// Gets a value indicating whether another request can use this instance.
        /// </summary>
        public bool IsReusable
        {
            get { return false; }
        }

        /// <summary>
        ///  Handle the http request.
        /// </summary>
        /// <param name="context">Current http context.</param>
        public void ProcessRequest(HttpContext context)
        {
            var request = context.Request;

            try
            {
                if (request.QueryString.AllKeys.Contains(Constant.REQUESTED_REALME_URL))
                {
                    string strScript = string.Format(
                        "<div style='margin-top:6px;'>{0}</div><script>window.top.location.href='{1}';</script>",
                        ACAContext.Instance.GetGUITextByKey("aca_access_protected_resource_msg_redirecting3rdparty2login"),
                        request.QueryString[Constant.REQUESTED_REALME_URL]);
                    context.Response.Write(strScript);
                    return;
                }

                if (Log.IsDebugEnabled)
                {
                    Log.DebugFormat("RealMeUrlRoutingHandler-> ProcessRequest: requestUrl={0}", request.Url.AbsoluteUri);
                }

                if (Common.IsContainsArrary(request.QueryString.AllKeys, RealMeAuthAdapter.RealMeQueryString))
                {
                    DoPostRedirection();
                }
            }
            catch (Exception exp)
            {
                if (!(exp is ThreadAbortException))
                {
                    Log.Error(exp.Message, exp);
                    ACAContext.Instance.ShowMessage(exp.Message, true);
                }
            }

            context.Response.StatusCode = Constant.HTTP_STATUS_CODE_404_RESOURCE_NOT_FOUND;
        }

        /// <summary>
        /// Do post redirection
        /// </summary>
        private void DoPostRedirection()
        {
            // Get security token and then save it.
            SecurityToken token = RealMeAuthAdapter.GetSecurityToken();
            var result = RealMeAuthAdapter.ValidateSecurityToken(token);
            
            if (result != null && !string.IsNullOrWhiteSpace(result.State))
            {
                byte[] transmittingDatas = MachineKey.Decode(result.State, MachineKeyProtection.Encryption);
                string acaUrl = string.Empty;

                if (transmittingDatas != null)
                {
                    acaUrl = HttpUtility.UrlDecode(transmittingDatas, new UTF8Encoding());

                    if (Log.IsDebugEnabled)
                    {
                        Log.DebugFormat("RealMeUrlRoutingHandler-> DoPostRedirection: acaUrl={0}", acaUrl);
                    }
                }

                if (!string.IsNullOrEmpty(acaUrl))
                {
                    string sessionPattern = string.Concat(@"[?&]{1}(", Constant.ACA_SESSION_ID, "){1}={1}[^&?]*");
                    Regex realMeRegex = new Regex(sessionPattern);
                    acaUrl = realMeRegex.Replace(acaUrl, matchItem => string.Empty);
                    bool isWrapIframe = !ACAContext.Instance.IsAmcaRequest(new Uri(acaUrl))
                        && !Common.IsYes(Common.CollecteValues((new Uri(acaUrl)).Query)[Constant.IS_FROM_SSO_LINK_HANDLER]);

                    Uri acaUri = new Uri(acaUrl);
                    NameValueCollection queryStrings = Common.CollecteValues(acaUri.Query);

                    if (!queryStrings.AllKeys.Contains(Constant.IS_FROM_REAL_ME))
                    {
                        string urlQueryJoinSymbol = acaUri.Query.Length == 0 ? "?" : "&";
                        acaUrl = string.Format("{0}{1}{2}={3}", acaUrl, urlQueryJoinSymbol, Constant.IS_FROM_REAL_ME, Constant.COMMON_Y);
                    }

                    ACAContext.Instance.RedirectWithInIframe(acaUrl, isWrapIframe);
                }
            }
        }
    }
}
