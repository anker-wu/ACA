#region Header

/**
* <pre>
*  Accela Citizen Access
*  File: RealMeAuthAdapter.cs
*
*  Accela, Inc.
*  Copyright (C): 2014
*
*  Description: The common functions for RealMe authentication adapter.
*
* </pre>
*/

#endregion

using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.Security;
using System.Web.SessionState;
using System.Web.UI;
using Accela.ACA.RealMeAccessGate.RealMeService;
using Accela.ACA.SSOInterface;
using Accela.ACA.SSOInterface.Constant;
using Accela.ACA.SSOInterface.Model;
using log4net;

namespace Accela.ACA.RealMeAccessGate
{
    /// <summary>
    /// RealMe authenticated adapter.
    /// </summary>
    public class RealMeAuthAdapter : IAuthAdapterV1
    {
        #region Fields

        /// <summary>
        /// _log object
        /// </summary>
        private static readonly ILog Log = LogFactory.Instance.GetLogger(typeof(RealMeAuthAdapter));

        /// <summary>
        /// RealMe query string parameters collection.
        /// </summary>
        private static IEnumerable<string> _realMeQueryString = new List<string>
                {
                    Constant.RealMeParamName_SAMLart,
                    Constant.RealMeParamName_RelayState,
                    Constant.RealMeParamName_SigAlg,
                    Constant.RealMeParamName_Signature
                };
        
        /// <summary>
        /// RealMe related settings.
        /// </summary>
        private static AppSettingsSection _realMeSettings;
        
        /// <summary>
        /// RealMe web service proxy.
        /// </summary>
        private static RealMe _proxy = new RealMe();

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="RealMeAuthAdapter"/> class.
        /// </summary>
        public RealMeAuthAdapter()
        {
            //Load RealMe configuration.
            string dllFilePath = Assembly.GetExecutingAssembly().CodeBase;
            Uri dllPathUri = new Uri(dllFilePath);
            Configuration config = ConfigurationManager.OpenExeConfiguration(dllPathUri.AbsolutePath);
            _realMeSettings = (AppSettingsSection)config.GetSection(Constant.ConfigSectionName);

            ACAContext.Events.PreUserCreation += AuthenticationEvents_ACACreatePublicUserBefore;
        }

        #endregion

        #region Properties
        
        /// <summary>
        /// Gets a value indicating whether the current request is authenticated.
        /// If exist relationship between RealMe and public user then signon directly, otherwise redirect to ACA registration page or AMCA default page.
        /// </summary>
        public bool IsAuthenticated
        {
            get
            {
                try
                {
                    bool isAuthed = false;
                    HttpRequest request = HttpContext.Current.Request;

                    //Just redirection back from RealMe login successful.
                    bool redirectionFromRealMe = Common.IsYes(request.QueryString[Constant.IS_FROM_REAL_ME]);

                    //Due to RealMe do not support RealMe single sign off, so needn't validate RealMe login status after login success.
                    if (!ACAContext.Instance.IsAnonymousUser)
                    {
                        isAuthed = HttpContext.Current.Request.IsAuthenticated;
                    }
                    else
                    {
                        SecurityToken token = GetSecurityToken();
                        ValidateLoginResult validateLoginResult = ValidateSecurityToken(token);

                        //Handle the validateLogin result after filter the application error handler page request.
                        if (validateLoginResult != null && ACAContext.Instance.IsRawRequest())
                        {
                            isAuthed = IsLoginValid(validateLoginResult, redirectionFromRealMe);
                        }
                    }

                    return isAuthed;
                }
                catch (Exception exp)
                {
                    Log.Error(exp.Message, exp);
                    return false;
                }
            }
        }

        /// <summary>
        /// Gets a value indicating whether the current authentication adapter is the embedded internal authentication adapter.
        /// Only the internal authentication adapter return true, all of other external authentication adapters must return false.
        /// </summary>
        public bool IsInternalAuthAdapter
        {
            get
            {
                return false;
            }
        }

        /// <summary>
        /// Gets the login Url for the current authentication adapter.
        /// </summary>
        public string LoginUrl
        {
            get
            {
                return GetRealMeLoginUrl(FileUtil.AppendAbsolutePath(SSOConstant.URL_WELCOME_PAGE));
            }
        }

        /// <summary>
        /// Gets the logout Url for the current authentication adapter.
        /// </summary>
        public string LogoutUrl
        {
            get
            {
                return FileUtil.AppendApplicationRoot(SSOConstant.URL_LOGOUT);
            }
        }

        /// <summary>
        /// Gets the register Url for the current authentication adapter.
        /// </summary>
        public string RegisterUrl
        {
            get
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// Gets the login currentUrl target.
        /// </summary>
        public string LoginUrlTarget
        {
            get
            {
                return _realMeSettings.Settings["LoginUrlTarget"].Value;
            }
        }

        /// <summary>
        /// Gets a value indicating whether need go to registration process to associate external account with public user account.
        /// </summary>
        public bool IsNeedRegistration
        {
            get 
            { 
                return true; 
            }
        }

        /// <summary>
        /// Gets the RealMe query string parameters collection.
        /// </summary>
        internal static IEnumerable<string> RealMeQueryString
        {
            get
            {
                return _realMeQueryString;
            }
        }

        /// <summary>
        /// Gets or sets the security token
        /// </summary>
        private string SecurityToken
        {
            get
            {
                return Convert.ToString(HttpContext.Current.Session[Constant.SESSION_SECURITY_TOKEN]);
            }

            set
            {
                HttpContext.Current.Session[Constant.SESSION_SECURITY_TOKEN] = value;
            }
        }
        
        #endregion Properties

        #region Methods

        /// <summary>
        /// ACA the create public user before implement.
        /// </summary>
        /// <param name="artModel">The art model.</param>
        /// <returns>Return Result Model.</returns>
        public ReturnResultModel AuthenticationEvents_ACACreatePublicUserBefore(UserCreationEventArgs artModel)
        {
            return null;
        }
        
        /// <summary>
        /// Sign out current system.
        /// </summary>
        /// <param name="context">Current http context.</param>
        public void Signout(HttpContext context)
        {
            ClearRealMeCookie();

            if (ACAContext.Instance.IsAmcaRequest(HttpContext.Current.Request.UrlReferrer))
            {
                HttpResponse response = HttpContext.Current.Response;
                string amcaUrl = response.ApplyAppPathModifier(SSOConstant.AMCA_DEFAULT_PAGE);
                response.Redirect(amcaUrl);
            }
        }

        /// <summary>
        /// Redirect to login page.
        /// </summary>
        public void RedirectToLoginPage()
        {
            RedirectToLoginPage(string.Empty);
        }

        /// <summary>
        /// Redirect to login page.
        /// </summary>
        /// <param name="extraQueryString">Extra query string.</param>
        public void RedirectToLoginPage(string extraQueryString)
        {
            HttpRequest request = HttpContext.Current.Request;
            RedirectToLoginPage(request.Url.AbsoluteUri, extraQueryString);
        }

        /// <summary>
        /// Redirect to login page.
        /// </summary>
        /// <param name="returnUrl">Returned currentUrl.</param>
        /// <param name="extraQueryString">Extra query string.</param>
        public void RedirectToLoginPage(string returnUrl, string extraQueryString)
        {
            if (!string.IsNullOrWhiteSpace(extraQueryString))
            {
                Uri returnUri = new Uri(returnUrl);
                string urlQueryJoinSymbol = returnUri.Query.Length == 0 ? "?" : "&";
                returnUrl = string.Format("{0}{1}{2}", returnUrl, urlQueryJoinSymbol, extraQueryString);
            }

            string realMeLoginUrl = GetRealMeLoginUrl(returnUrl);
            RedirectWithoutIframe(realMeLoginUrl);
        }

        /// <summary>
        /// Validate security token.
        /// </summary>
        /// <param name="token">Security token object.</param>
        /// <returns>Whether security token is valid.</returns>
        internal ValidateLoginResult ValidateSecurityToken(SecurityToken token)
        {
            if (token.HasValue)
            {
                ValidateLoginResult validateResult = null;
                HttpRequest request = HttpContext.Current.Request;
                string ipAddress = request.UserHostAddress;
                string userAgent = request.UserAgent;
                string statusCode = string.Empty;
                string username = string.Empty;
                string resultMessage = string.Empty;
                string securityToken = string.Empty;
                string isAssociatedWithuser = string.Empty;

                try
                {
                    string state = _proxy.validateLogin(ipAddress, userAgent, token.SAMLArt, token.RelayState, token.SigAlg, token.Signature, out statusCode, out username, out resultMessage, out securityToken, out isAssociatedWithuser);
                    validateResult = new ValidateLoginResult();
                    validateResult.State = state;
                    validateResult.StatusCode = statusCode;
                    validateResult.UserName = Common.IsYes(_realMeSettings.Settings["IsReturnUserName"].Value) ? username : string.Empty; //TODO
                    validateResult.ResultMessage = resultMessage;
                    validateResult.SecurityToken = securityToken;
                    validateResult.IsAssociatedWithUser = Convert.ToBoolean(isAssociatedWithuser);
                    
                    if (Log.IsDebugEnabled)
                    {
                        JavaScriptSerializer javaScriptSerializer = new JavaScriptSerializer();
                        Log.DebugFormat("RealMeAuthAdapter-> ValidateSecurityToken: validateResult={0}", javaScriptSerializer.Serialize(validateResult));
                    }
                }
                catch (Exception exp)
                {
                    Log.Error(exp.Message, exp);
                }
                
                return validateResult;
            }

            return null;
        }

        /// <summary>
        /// Redirect to the page without ACA frame.
        /// </summary>
        /// <param name="url">The currentUrl</param>
        internal void RedirectWithoutIframe(string url)
        {
            Page curPage = HttpContext.Current.Handler as Page;

            if (HttpContext.Current.Handler is RealMeHttpHandler || ACAContext.Instance.IsAmcaRequest())
            {
                HttpContext.Current.Response.Redirect(url);
            }
            else if (curPage != null)
            {
                string targetUrl = FileUtil.AppendAbsolutePath(string.Format("RealMeUrlRouting.ashx?{0}={1}", Constant.REQUESTED_REALME_URL, HttpUtility.UrlEncode(url)));
                HttpContext.Current.Response.Redirect(targetUrl);
            }
        }
        
        /// <summary>
        /// Get security token.
        /// </summary>
        /// <returns>Security token.</returns>
        internal SecurityToken GetSecurityToken()
        {
            SecurityToken token = GetSecurityTokenFromUrl();

            /*If current Url does not contain any token parameter,
             * then try to get token parameters from cookie.
             * */
            if (!token.HasValue)
            {
                token = GetSecurityTokenFromCookie(token);
            }

            if (Log.IsDebugEnabled)
            {
                JavaScriptSerializer javaScriptSerializer = new JavaScriptSerializer();
                Log.DebugFormat("RealMeAuthAdapter-> GetSecurityToken: SecurityToken={0}", javaScriptSerializer.Serialize(token));
            }

            return token;
        }

        /// <summary>
        /// Handle the RealMe validate login result.
        /// </summary>
        /// <param name="validateLoginResult">validate login result.</param>
        /// <param name="redirectionFromRealMe">redirection from RealMe.</param>
        /// <returns>is authenticated.</returns>
        private bool IsLoginValid(ValidateLoginResult validateLoginResult, bool redirectionFromRealMe)
        {
            bool isAuthed = false;

            if (validateLoginResult == null)
            {
                return false;
            }
            
            Constant.RealMeLoginStatus statusCode = EnumUtil<Constant.RealMeLoginStatus>.Parse(validateLoginResult.StatusCode);
            string resultMessage = validateLoginResult.ResultMessage;
            HttpContext current = HttpContext.Current;

            if (statusCode == Constant.RealMeLoginStatus.SUCCESS)
            {
                UserModel userModel = null;
                bool isAmcaRequest = ACAContext.Instance.IsAmcaRequest();

                try
                {
                    if (!string.IsNullOrEmpty(validateLoginResult.UserName))
                    {
                        userModel = ACAContext.Instance.CreateUserContext(validateLoginResult.UserName, Constant.SSO_ACCOUNT_TYPE_REALME);
                    }

                    if (Log.IsDebugEnabled)
                    {
                        Log.DebugFormat(
                            "RealMeAuthAdapter-> IsLoginValid: associatedPublicUserId={0}, redirectionFromRealMe={1}",
                            userModel == null ? string.Empty : userModel.UserId, 
                            redirectionFromRealMe);
                    }
                }
                catch (Exception exp)
                {
                    if (redirectionFromRealMe)
                    {
                        ACAContext.Instance.ShowMessage(exp.Message);
                        return false;
                    }
                }

                if (userModel != null)
                {
                    isAuthed = true;

                    if (!isAmcaRequest)
                    {
                        current.Response.Redirect(current.Request.Url.AbsoluteUri);
                    }
                }
                else if (redirectionFromRealMe)
                {
                    userModel = new UserModel();
                    userModel.SSOType = Constant.SSO_ACCOUNT_TYPE_REALME;
                    userModel.SSOUserName = validateLoginResult.UserName;
                    SecurityToken = validateLoginResult.SecurityToken;
                    ACAContext.Instance.RedirectToRegistration(userModel, current.Request.Url.AbsoluteUri);
                }
            }
            else 
            {
                if (string.IsNullOrEmpty(resultMessage))
                {
                    resultMessage = string.Concat("Validate login failed. ", validateLoginResult.StatusCode);
                }

                throw new Exception(resultMessage);
            }

            return isAuthed;
        }

        /// <summary>
        /// Save RealMe cookies.
        /// </summary>
        /// <param name="token">Security token object.</param>
        private void SaveSecurityToken2Cookie(SecurityToken token)
        {
            if (!token.HasValue)
            {
                return;
            }

            Common.SaveDataToCookie(token.SAMLArt, Constant.RealMeParamName_SAMLart);
            Common.SaveDataToCookie(token.RelayState, Constant.RealMeParamName_RelayState);
            Common.SaveDataToCookie(token.SigAlg, Constant.RealMeParamName_SigAlg);
            Common.SaveDataToCookie(token.Signature, Constant.RealMeParamName_Signature);
        }

        /// <summary>
        /// Request RealMe for login URL.
        /// </summary>
        /// <param name="requestedAcaUrl">Target URL after successfully authenticated by RealMe.</param>
        /// <returns>RealMe login URL.</returns>
        private string GetRealMeLoginUrl(string requestedAcaUrl)
        {
            Uri originalUri = new Uri(requestedAcaUrl);
            Uri targetUri = new Uri(string.Format("{0}://{1}{2}", ACAContext.Instance.Protocol, originalUri.Authority, originalUri.PathAndQuery));
            requestedAcaUrl = targetUri.AbsoluteUri;
            HttpSessionState session = HttpContext.Current.Session;

            //Using http handler to transfer ACA request.
            string interceptOrUrl = FileUtil.AppendAbsolutePath("RealMeUrlRouting.ashx");
            string parameter = string.Concat(requestedAcaUrl, "&", Constant.ACA_SESSION_ID, "=", session.SessionID);

            if (HttpContext.Current.Handler is RealMeHttpHandler)
            {
                parameter = string.Concat(parameter, "&", Constant.IS_FROM_SSO_LINK_HANDLER, "=", Constant.COMMON_Y);
            }

            string state = MachineKey.Encode(HttpUtility.UrlEncodeToBytes(parameter, new UTF8Encoding()), MachineKeyProtection.Encryption);

            try
            {
                string redirectionUrl = _proxy.getLoginURL(state, interceptOrUrl);

                if (Log.IsDebugEnabled)
                {
                    Log.DebugFormat("RealMeAuthAdapter-> GetRealMeLoginUrl: requestingUrl={0}", parameter);
                }

                return redirectionUrl;
            }
            catch (Exception exp)
            {
                Log.Error(exp.Message, exp);
                throw exp;
            }
        }

        /// <summary>
        /// Get current url parameter keys.
        /// </summary>
        /// <returns>url parameter key array.</returns>
        private NameValueCollection GetCurrentUrlParameterKeys()
        {
            HttpRequest request = HttpContext.Current.Request;
            NameValueCollection parameterCollection = new NameValueCollection();
            Uri requestUri = request.Url;

            if (requestUri.ToString().Count(c => c == '?') >= 2)
            {
                string url = requestUri.AbsoluteUri.Replace("?", "&");
                parameterCollection = Common.CollecteValues(url.Substring(url.IndexOf('&') + 1));
            }
            else
            {
                parameterCollection = request.QueryString;
            }

            return parameterCollection;
        }

       /// <summary>
       /// Get security token from URL.
       /// </summary>
       /// <returns>Security token object.</returns>
        private SecurityToken GetSecurityTokenFromUrl()
        {
            SecurityToken token = new SecurityToken();
            NameValueCollection urlParametes = GetCurrentUrlParameterKeys();

            if (Common.IsContainsArrary(urlParametes.AllKeys, RealMeQueryString))
            {
                token.SAMLArt = urlParametes[Constant.RealMeParamName_SAMLart];
                token.RelayState = urlParametes[Constant.RealMeParamName_RelayState];
                token.SigAlg = urlParametes[Constant.RealMeParamName_SigAlg];
                token.Signature = urlParametes[Constant.RealMeParamName_Signature];
                
                // Save or update security token while currentUrl parameters can provide by the way.
                SaveSecurityToken2Cookie(token);
            }

            return token;
        }

        /// <summary>
        /// Try to get security token from cookie while URL does not have these parameters.
        /// </summary>
        /// <param name="token">Security token object from Url.</param>
        /// <returns>Security token object.</returns>
        private SecurityToken GetSecurityTokenFromCookie(SecurityToken token)
        {
            if (token == null)
            {
                token = new SecurityToken();
            }

            if (!token.HasValue)
            {
                token.SAMLArt = Common.GetDataFromCookie(Constant.RealMeParamName_SAMLart);
                token.RelayState = Common.GetDataFromCookie(Constant.RealMeParamName_RelayState);
                token.SigAlg = Common.GetDataFromCookie(Constant.RealMeParamName_SigAlg);
                token.Signature = Common.GetDataFromCookie(Constant.RealMeParamName_Signature);
            }

            return token;
        }

        /// <summary>
        /// Clear RealMe cookie.
        /// </summary>
        private void ClearRealMeCookie()
        {
            foreach (string cookieName in RealMeQueryString)
            {
                Common.SaveDataToCookie(string.Empty, cookieName, -30);
            }
        }

        #endregion Methods
    }
}