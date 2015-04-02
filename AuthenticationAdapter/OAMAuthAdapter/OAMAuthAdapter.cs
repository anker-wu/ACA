#region Header

/**
* <pre>
*  Accela Citizen Access
*  File: OAMAuthAdapter.cs
*
*  Accela, Inc.
*  Copyright (C): 2012-2014
*
*  Description: The Oracle Access Manager authentication adapter implemented the IAuthAdapter interface.
*
* </pre>
*/

#endregion

using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Security.Authentication;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.SessionState;
using Accela.ACA.BLL.Account;
using Accela.ACA.BLL.Common;
using Accela.ACA.Common;
using Accela.ACA.Common.Config;
using Accela.ACA.Common.Log;
using Accela.ACA.Common.Util;
using Accela.ACA.Web;
using Accela.ACA.Web.Common;
using Accela.ACA.WSProxy;
using log4net;
using Oblix.Access.Common;
using Oblix.Access.Server;

namespace Accela.ACA.OAMAccessGate
{
    /// <summary>
    /// The Oracle Access Manager authentication adapter implemented the IAuthAdapter interface.
    /// This adapter use the Oracle Access SDK to implement an AccessGate of Oracle Access Manager.
    /// </summary>
    public class OAMAuthAdapter : IAuthAdapter, IDisposable
    {
        /// <summary>
        /// Log instance.
        /// </summary>
        private static readonly ILog _log = LogFactory.Instance.GetLogger(typeof(OAMAuthAdapter));

        /// <summary>
        /// LDAP DateTime format.
        /// </summary>
        private static string ldapDataFormat = "yyyyMMddHHmmssZ";

        /// <summary>
        /// LDAP DateTime format pattern.
        /// </summary>
        private static Regex ldapDatePattern = new Regex(@"\d{14}Z", RegexOptions.IgnoreCase);

        /// <summary>
        /// OAM configuration instance.
        /// </summary>
        private OAMConfiguration oamConfig;

        /// <summary>
        /// Initializes a new instance of the OAMAuthAdapter class.
        /// Load the OAM configuration and initialize the ASDK environment.
        /// </summary>
        public OAMAuthAdapter()
        {
            //Load OAM configuration.
            string dllFilePath = Assembly.GetExecutingAssembly().CodeBase;
            Uri dllPathUri = new Uri(dllFilePath, true);
            Configuration config = ConfigurationManager.OpenExeConfiguration(dllPathUri.AbsolutePath);
            oamConfig = (OAMConfiguration)config.GetSection(Constant.ConfigSectionName);

            //Initialize the ASDK environment.
            ObConfigMgd.initialize(oamConfig.ASDKInstallDir);
        }

        /// <summary>
        /// Finalizes an instance of the OAMAuthAdapter class and release the ASDK instance.
        /// </summary>
        ~OAMAuthAdapter()
        {
            ObConfigMgd.shutdown();
        }

        /// <summary>
        /// Gets a value indicating whether the current user is authenticated.
        /// The user information will be synchronized from OAM server, and the mapping account will be created if the user does not exists in ACA.
        /// </summary>
        public bool IsAuthenticated
        {
            get
            {
                bool isAuthed = false;

                HttpRequest request = HttpContext.Current.Request;
                HttpResponse response = HttpContext.Current.Response;
                HttpSessionState session = HttpContext.Current.Session;

                /*
                 * Page redirection matrix:
                 * 1. User just signed out from ACA or other site and then return to ACA: (Last user ID in session but ObSSOCookie is invalid):
                 *      a. Protected resource -> redirect to login page.
                 *      b. Unprotected resource -> redirect to initial page. Because system do not know whehter user is signed in.
                 * 2. User just switch to another user in other site and return to ACA: (Last user ID in session but ObSSOCookie is invalid and User ID is switched):
                 *      a. Protected resource -> redirect to login page -> auto-login with new user ID.
                 *      b. Unprotected resource -> redirect to initial page. Because system do not know whehter user is signed in.
                 * 3. User just login to other and access to ACA(May be ObSSOCookie not exists, Last User ID not in session(user not change)):
                 *      a. Protected resource -> redirect to login page -> auto-login.
                 *      b. Unprotected resource -> redirect to requested resource.
                 */

                try
                {
                    string ssoCookieValue = Common.GetSSOCookie(request);

                    if (!String.IsNullOrWhiteSpace(ssoCookieValue))
                    {
                        /* Got the SSO cookie. */

                        ObUserSessionMgd userSession = new ObUserSessionMgd(ssoCookieValue);
                        ObResourceRequestMgd requestResource = BuildRequestResource(request);

                        if (userSession.Status.IsLoggedIn && userSession.IsAuthorized(requestResource))
                        {
                            /* The SSO cookie is valid and user signed in. */

                            isAuthed = true;
                            PublicUserModel4WS userInfo = BuildUserInfo(userSession, oamConfig.AttributesMapping);

                            bool isUserChanged = IsUserChanged(session, userInfo.userID, isAuthed);

                            if (Common.IsJustSignedIn(request, ref response) || isUserChanged)
                            {
                                /*
                                 * If user just logged in or changed to another user, system will to synchronize user info from OAM to ACA.
                                 * And to initionalize the user context.
                                 */
                                PublicUserModel4WS currentUser = SyncUserInfoToACA(userInfo);

                                //A registered user changed to another registered user(maybe in other site), clear and terminates current session.
                                if (isUserChanged)
                                {
                                    session.Clear();
                                    session.Abandon();
                                }

                                //Initialize the user context.
                                AccountUtil.CreateUserContext(currentUser);

                                //If the user is authenticated, remember the current user ID.
                                session[Constant.SessionName_LastUserID] = userInfo.userID;

                                //A registered user changed to another registered user(maybe in other site), redirect the request to initial page.
                                string initialPage = GetInitialPage(request, response);

                                if (isUserChanged && !request.Url.AbsolutePath.Equals(initialPage, StringComparison.OrdinalIgnoreCase))
                                {
                                    response.Redirect(initialPage);
                                }
                            }
                        }
                        else
                        {
                            if (!userSession.Status.IsLoggedIn)
                            {
                                if (_log.IsDebugEnabled)
                                {
                                    _log.DebugFormat("[{0}]{1}", userSession.Error, userSession.ErrorMessage);
                                }
                            }
                        }
                    }
                }
                catch (ObAccessExceptionMgd accessExp)
                {
                    isAuthed = false;
                    _log.Error(String.Format("[{0}]{1}", accessExp.Code, accessExp.String), accessExp);
                }
                catch (Exception exp)
                {
                    isAuthed = false;
                    _log.Error(exp.Message, exp);
                    throw new ACAException(exp);
                }
                finally
                {
                    if (!isAuthed)
                    {
                        //To determine whether current request needs to login.
                        IBizDomainBll bizDomain = ObjectFactory.GetObject<IBizDomainBll>();
                        bool isForceLogin = bizDomain.IsForceLogin(BasePage.GetModuleName(request), request.RawUrl, null);

                        //If current request is protected but the user is not authenticate, clear and terminates current session and redirect to login page.
                        if (isForceLogin)
                        {
                            session.Clear();
                            RedirectToLoginPage();
                        }

                        //AskChallengeUrl(request, response);
                        Common.RemoveSSOCookie(request, ref response);

                        /*
                         * If the user is not authenticated, and the last status is authenticated.
                         * It's means the user just logged out from ACA and come back again with the anonymous status.
                         * We need to clear the session status from last user and create the user context as anonymous.
                         */
                        if (IsUserChanged(session, null, isAuthed))
                        {
                            session.Clear();

                            //Initialize the user context as anonymous.
                            AccountUtil.CreateUserContext(AccountUtil.MakeAnonymousUser());

                            //User just signed out from ACA and then return to ACA again, navigate user to initial page.
                            string initialPage = GetInitialPage(request, response);

                            if (IsAmcaRequest(request))
                            {
                                //For AMCA, redirect to login page and set return Url as initial page.
                                RedirectToLoginPage(initialPage, null);
                            }
                            else if (!request.Url.AbsolutePath.Equals(initialPage, StringComparison.OrdinalIgnoreCase))
                            {
                                response.Redirect(initialPage);
                            }
                        }
                    }
                }

                return isAuthed;
            }
        }

        /// <summary>
        ///  Gets a value indicating whether the current authentication adapter is the embedded internal authentication adapter.
        ///  The OAMAuthAdapter is not an internal authentication adapter.
        /// </summary>
        public bool IsInternalAuthAdapter
        {
            get { return false; }
        }

        /// <summary>
        /// Gets the login Url for the current authentication adapter.
        /// </summary>
        public string LoginUrl
        {
            get
            {
                return BuildLoginUrl(HttpContext.Current.Request, HttpContext.Current.Response);
            }
        }

        /// <summary>
        /// Gets the logout Url for the current authentication adapter.
        /// </summary>
        public string LogoutUrl
        {
            get
            {
                return oamConfig.LogoutUrl;
            }
        }

        /// <summary>
        /// Gets the register Url for the current authentication adapter.
        /// </summary>
        public string RegisterUrl
        {
            get
            {
                return oamConfig.RegisterUrl;
            }
        }

        /// <summary>
        /// Implement the IDisposable interface to release the ASDK.
        /// </summary>
        public void Dispose()
        {
            ObConfigMgd.shutdown();
        }

        /// <summary>
        /// Redirect user to login page.
        /// </summary>
        public void RedirectToLoginPage()
        {
            RedirectToLoginPage(null);
        }

        /// <summary>
        /// Redirect user to login page with the extra query strings.
        /// </summary>
        /// <param name="extraQueryString">Pass through the extra query strings to login page.</param>
        public void RedirectToLoginPage(string extraQueryString)
        {
            RedirectToLoginPage(HttpContext.Current.Request.Url.AbsoluteUri, extraQueryString);
        }

        /// <summary>
        /// Redirect user to login page with the specified return Url and the extra query strings.
        /// </summary>
        /// <param name="returnUrl">Spedify the return Url instead of the request Url.</param>
        /// <param name="extraQueryString">Pass through the extra query strings to login page.</param>
        public void RedirectToLoginPage(string returnUrl, string extraQueryString)
        {
            HttpRequest request = HttpContext.Current.Request;
            HttpResponse response = HttpContext.Current.Response;

            //Before redirect to challenge URL, the "ObSSOCookie" cookie should set as "loggedoutcontinue" without domain.
            Common.SetSSOCookie(request, ref response, Constant.SSOCookieValue_BeforeLogin);

            if (returnUrl.IndexOf("://") < 0)
            {
                if (returnUrl.StartsWith("~/", StringComparison.OrdinalIgnoreCase))
                {
                    returnUrl = VirtualPathUtility.ToAbsolute(returnUrl);
                }
                else if (!returnUrl.StartsWith("/", StringComparison.OrdinalIgnoreCase))
                {
                    returnUrl = FileUtil.CombineWebPath(request.ApplicationPath, returnUrl);
                }

                returnUrl = request.Url.Scheme + "://" + FileUtil.CombineWebPath(request.Url.Authority, returnUrl);
            }

            response.Redirect(BuildLoginUrl(new Uri(returnUrl), request.HttpMethod, extraQueryString));
        }

        /// <summary>
        /// Validate user's credentials and create mapping user in Accela system.
        /// </summary>
        /// <param name="username">User identity.</param>
        /// <param name="password">User password.</param>
        /// <returns>Public user model and the user information already synchronized to Accela system.</returns>
        public PublicUserModel4WS ValidateUser(string username, string password)
        {
            HttpRequest request = HttpContext.Current.Request;
            HttpResponse response = HttpContext.Current.Response;

            try
            {
                ObResourceRequestMgd requestResource = BuildRequestResource(request);

                ObDictionary credentials = new ObDictionary();
                credentials.Add("userid", username);
                credentials.Add("password", password);

                ObUserSessionMgd userSession = new ObUserSessionMgd(requestResource, credentials);

                if (userSession.Status.IsLoggedIn && userSession.IsAuthorized(requestResource))
                {
                    Common.SetSSOCookie(request, ref response, userSession.SessionToken);
                    PublicUserModel4WS userInfo = BuildUserInfo(userSession, oamConfig.AttributesMapping);
                    return SyncUserInfoToACA(userInfo);
                }

                return null;
            }
            catch (ObAccessExceptionMgd accessExp)
            {
                _log.Error(accessExp.Message, accessExp);
                throw new AuthenticationException(accessExp.Message, accessExp);
            }
            catch (Exception exp)
            {
                _log.Error(exp.Message, exp);
                throw new ACAException(exp);
            }
        }

        /// <summary>
        /// Build public user model based on the user information retrieved from the OAM server.
        /// </summary>
        /// <param name="session">ObUserSession object contains the user information.</param>
        /// <param name="userAttributes">A mapping attribute collection to indicates which attributes need to copy.</param>
        /// <returns>A public user model instance.</returns>
        private static PublicUserModel4WS BuildUserInfo(ObUserSessionMgd session, UserMappingAttributeCollection userAttributes)
        {
            if (session.getNumberOfActions(Constant.PolicyResponse_HeaderType) <= 0)
            {
                return new PublicUserModel4WS();
            }

            ObDictionary actions = session.getActions(Constant.PolicyResponse_HeaderType);
            string userID = GetActionItem(actions, Constant.HeaderName_OAMUser) as string;
            PublicUserModel4WS publicUser = new PublicUserModel4WS { userID = userID };
            Type publicUserType = publicUser.GetType();

            foreach (UserMappingAttribute attribute in userAttributes)
            {
                if (String.IsNullOrWhiteSpace(attribute.ExternalName))
                {
                    continue;
                }

                object value = GetActionItem(actions, attribute.ExternalName);

                if (value == null)
                {
                    continue;
                }

                PropertyInfo property = publicUserType.GetProperty(attribute.UserModelPropertyName);

                try
                {
                    //PublicUserModel's property is writable.
                    if (property != null && property.CanWrite)
                    {
                        //Special logic for BirthDate field, field name defined in OAMConfiguration section.
                        if (attribute.Name.Equals("BirthDate"))
                        {
                            if (value is string && ldapDatePattern.IsMatch(value as string))
                            {
                                string strValue = (string)value;
                                DateTime dt;

                                if (DateTime.TryParseExact(strValue, ldapDataFormat, null, DateTimeStyles.None, out dt))
                                {
                                    property.SetValue(publicUser, I18nDateTimeUtil.FormatToDateTimeStringForWebService(dt), null);
                                }
                            }
                        }
                        else if (property.PropertyType.Equals(value.GetType()))
                        {
                            //The data type are the same.
                            if (value is string)
                            {
                                string strValue = (string)value;

                                if (strValue.Length > attribute.MaxLength)
                                {
                                    value = strValue.Substring(0, attribute.MaxLength);
                                }
                            }

                            property.SetValue(publicUser, value, null);
                        }
                    }
                }
                catch (Exception exp)
                {
                    _log.Error("Sync user information failed: " + exp.Message, exp);
                }
            }

            return publicUser;
        }

        /// <summary>
        /// Get the item value from the ObDictionary object and filter out "NOT FOUND" value.
        /// </summary>
        /// <param name="obDict">The ObDictionary object contains the UserSession action items.</param>
        /// <param name="key">The item key.</param>
        /// <returns>The item value.</returns>
        private static object GetActionItem(ObDictionary obDict, object key)
        {
            object value = null;

            try
            {
                value = obDict.get_Item(key);

                if (Constant.PolicyResponse_NotFound.Equals(value))
                {
                    value = null;
                }
            }
            catch (Exception exp)
            {
                _log.Error(exp.Message, exp);
            }

            return value;
        }

        /// <summary>
        /// Get a reasonable contact method for public user based on the user's information.
        /// The priority from highest to lowest is: e-mail -> mobile phone -> work phone -> home phone -> fax
        /// </summary>
        /// <param name="publicUser">The public user model contains the user information.</param>
        /// <returns>The contact method defined in ACA.</returns>
        private static string GetContactMethod(PublicUserModel4WS publicUser)
        {
            string contactMethod = null;

            IBizDomainBll bizDomainBll = ObjectFactory.GetObject<IBizDomainBll>();

            //ShowType is 1 means to get the standard-choice description as the value.
            IList<ItemValue> stdItems = bizDomainBll.GetBizDomainList(ConfigManager.AgencyCode, BizDomainConstant.STD_CAT_CONTACT_METHODS, false, 1);

            if (stdItems != null && stdItems.Count > 0)
            {
                if (!String.IsNullOrWhiteSpace(publicUser.email) && stdItems.Any(p => ContactMethod.Email.Equals(p.Key, StringComparison.OrdinalIgnoreCase)))
                {
                    contactMethod = ContactMethod.Email;
                }
                else if (!String.IsNullOrWhiteSpace(publicUser.cellPhone) && stdItems.Any(p => ContactMethod.MobilePhone.Equals(p.Key, StringComparison.OrdinalIgnoreCase)))
                {
                    contactMethod = ContactMethod.MobilePhone;
                }
                else if (!String.IsNullOrWhiteSpace(publicUser.workPhone) && stdItems.Any(p => ContactMethod.WorkPhone.Equals(p.Key, StringComparison.OrdinalIgnoreCase)))
                {
                    contactMethod = ContactMethod.WorkPhone;
                }
                else if (!String.IsNullOrWhiteSpace(publicUser.homePhone) && stdItems.Any(p => ContactMethod.HomePhone.Equals(p.Key, StringComparison.OrdinalIgnoreCase)))
                {
                    contactMethod = ContactMethod.HomePhone;
                }
                else if (!String.IsNullOrWhiteSpace(publicUser.fax) && stdItems.Any(p => ContactMethod.Fax.Equals(p.Key, StringComparison.OrdinalIgnoreCase)))
                {
                    contactMethod = ContactMethod.Fax;
                }
            }

            return contactMethod;
        }

        /// <summary>
        /// Gets a value indicates whether the specify HTTP request is came from AMCA.
        /// </summary>
        /// <param name="request">HTTP request.</param>
        /// <returns>ture for AMCA request, otherwise false.</returns>
        private static bool IsAmcaRequest(HttpRequest request)
        {
            string amcaPath = FileUtil.CombineWebPath(request.ApplicationPath, "amca");
            return request.Url.AbsolutePath.StartsWith(amcaPath, StringComparison.OrdinalIgnoreCase);
        }

        /// <summary>
        /// To determine whether the user is changed in current http session based on the last user ID may be stored in current http session.
        /// Only handle two scenarios:
        /// 1. Registered user changed to anonymous user.
        /// 2. A registered user changed to another registered user.
        /// </summary>
        /// <param name="session">Current http session.</param>
        /// <param name="currentUserID">Current user ID.</param>
        /// <param name="isAuthenticated">To indicates current user whether is authenticated.</param>
        /// <returns>ture or false.</returns>
        private static bool IsUserChanged(HttpSessionState session, string currentUserID, bool isAuthenticated)
        {
            bool isUserChanged = false;

            if (session[Constant.SessionName_LastUserID] != null)
            {
                string lastUserID = (string)session[Constant.SessionName_LastUserID];

                if (!isAuthenticated || (isAuthenticated && !lastUserID.Equals(currentUserID, StringComparison.OrdinalIgnoreCase)))
                {
                    isUserChanged = true;
                }
            }

            return isUserChanged;
        }

        /// <summary>
        /// Synchronize the user information from the specified user model to ACA user store.
        /// The user will be auto-created if the specified user not exists in ACA.
        /// Only these properties which value are not null will be synchronize to ACA.
        /// </summary>
        /// <param name="publicUser">The specified user model contains user information.</param>
        /// <returns>Return the synchronized user model.</returns>
        private static PublicUserModel4WS SyncUserInfoToACA(PublicUserModel4WS publicUser)
        {
            IAccountBll accountBll = ObjectFactory.GetObject<IAccountBll>();
            string agencyCode = ConfigManager.AgencyCode;
            string defaultContactType = "Individual";

            PublicUserModel4WS existingUser = accountBll.Signon4External(ConfigManager.AgencyCode, publicUser.userID);

            if (existingUser == null)
            {
                /*
                 * Use does not exists in Accela - Create the mapping account in Accela db.
                 */

                if (!String.IsNullOrWhiteSpace(publicUser.cellPhone))
                {
                    publicUser.receiveSMS = ACAConstant.COMMON_Y;
                }

                publicUser.prefContactChannel = GetContactMethod(publicUser);
                publicUser.password = AccountUtil.GetRandomPassword();
                publicUser.servProvCode = ConfigManager.AgencyCode;
                publicUser.auditID = publicUser.userID;
                publicUser.auditStatus = ACAConstant.VALID_STATUS;
                publicUser.roleType = ACAConstant.ROLE_TYPE_CITIZEN;

                if (publicUser.peopleModel == null)
                {
                    // Initialize an empty associated contact for the public user.
                    PeopleModel4WS peopleModel = new PeopleModel4WS();
                    peopleModel.serviceProviderCode = agencyCode;
                    peopleModel.contactType = defaultContactType;
                    peopleModel.contactTypeFlag = defaultContactType;
                    peopleModel.auditStatus = ACAConstant.VALID_STATUS;
                    peopleModel.email = publicUser.email;

                    publicUser.peopleModel = new[] { peopleModel };
                }
                else
                {
                    // Check required data if is correct. Such as: Contact Type, Contact Type Flag, Agency Code, Audit Status.
                    foreach (var peopleModel in publicUser.peopleModel)
                    {
                        if (peopleModel == null)
                        {
                            continue;
                        }

                        if (string.IsNullOrEmpty(peopleModel.email))
                        {
                            peopleModel.email = publicUser.email;
                        }

                        if (string.IsNullOrEmpty(peopleModel.contactType))
                        {
                            peopleModel.contactType = defaultContactType;
                        }

                        // Only support "Individual" and "Organization" for contactTypeFlag.
                        if (string.IsNullOrEmpty(peopleModel.contactTypeFlag))
                        {
                            peopleModel.contactTypeFlag = defaultContactType;
                        }

                        peopleModel.serviceProviderCode = agencyCode;
                        peopleModel.auditStatus = ACAConstant.VALID_STATUS;
                    }
                }

                /*
                 * The account created by createPublicUser interface may be is inactive (Depends on Agency's e-mail verification setting).
                 * Call the signon4External again to validate the status.
                 */
                accountBll.CreatePublicUser(publicUser, null);
                publicUser = accountBll.Signon4External(ConfigManager.AgencyCode, publicUser.userID);
            }
            else
            {
                //Merge user info and sync to Accela db.
                accountBll.MergePublicUserInfo(publicUser, ref existingUser);
                existingUser.password = null;
                existingUser.servProvCode = ConfigManager.AgencyCode;
                accountBll.EditPublicUser(existingUser);
                publicUser = existingUser;
            }

            return publicUser;
        }

        /// <summary>
        /// Inject an iframe into current response and let navigate user to challenge URL to determine whether have user already signed in.
        /// The logic has a limitation - the login URL configured in Authentication Scheme must can works in iframe.
        /// </summary>
        /// <param name="request">HTTP request.</param>
        /// <param name="response">HTTP response.</param>
        private void AskChallengeUrl(HttpRequest request, HttpResponse response)
        {
            string challengeUrl = BuildLoginUrl(request, response);

            if (!response.Cookies.AllKeys.Contains("AskOAMChallenge"))
            {
                HttpCookie cookie = new HttpCookie("AskOAMChallenge");
                cookie.Expires = DateTime.Now.AddYears(-1);
                response.Cookies.Add(cookie);

                string scripts = "<!DOCTYPE html PUBLIC \"-//W3C//DTD XHTML 1.0 Strict//EN\" \"http://www.w3.org/TR/xhtml1/DTD/xhtml1-strict.dtd\">\r\n";
                scripts += "<script type='text/javascript'>";

                scripts += "if(document.attachEvent){document.attachEvent('onreadystatechange',function(){if(typeof(ShowLoading)!='undefined' && document.readyState=='interactive')ShowLoading();});}";
                scripts += "else if(document.addEventListener){document.addEventListener('readystatechange',function(){if(typeof(ShowLoading)!='undefined' && document.readyState=='interactive')ShowLoading();},false);}\r\n";

                scripts += "function AskOAMChallenge(frame){";
                scripts += "try{if(frame.contentWindow.location.pathname.toLowerCase()==\"" + request.Url.AbsolutePath.ToLowerInvariant() + "\"){window.location.reload();}}";
                scripts += "catch(e){if(typeof(HideLoading)!='undefined')HideLoading();}}";

                scripts += "</script>\r\n";
                scripts += "<iframe style='display:none;' onload='AskOAMChallenge(this);' src=\"" + challengeUrl + "\"></iframe>";
                response.Write(scripts);
            }
        }

        /// <summary>
        /// Build the login url based on the user's current request url.
        /// </summary>
        /// <param name="request">HTTP request.</param>
        /// <param name="response">HTTP response.</param>
        /// <returns>The login url.</returns>
        private string BuildLoginUrl(HttpRequest request, HttpResponse response)
        {
            string errorPageUrl = response.ApplyAppPathModifier("~/error.aspx");
            string initialPageUrl = GetInitialPage(request, response);
            Uri requestUrl = request.Url;

            //To avoid current request stay in error page.
            if (requestUrl.AbsolutePath.Equals(errorPageUrl, StringComparison.OrdinalIgnoreCase))
            {
                requestUrl = new Uri(FileUtil.CombineWebPath(requestUrl.Scheme + "://" + request.Url.Authority, initialPageUrl));
            }

            return BuildLoginUrl(requestUrl, request.HttpMethod, null);
        }

        /// <summary>
        /// Build the login url based on the specified request url.
        /// </summary>
        /// <param name="requestUrl">Specified request url.</param>
        /// <param name="httpMethod">The http data transfer method (GET or POST) used by the client for OAM policy evaluation.</param>
        /// <param name="extraQueryString">Pass through the extra query strings to login page.</param>
        /// <returns>The login url.</returns>
        private string BuildLoginUrl(Uri requestUrl, string httpMethod, string extraQueryString)
        {
            /*
             * The parameters of the challenge request are:
             * wh - the value of host identifier used in policy
             * wu - the value of url which OAM uses to evaluate policies  (url without host name)
             * wq - query parameters used by OAM to evaluate policies    (=rq)
             * wo - HTTP operation(GET / POST / HEAD, can be a number , 1 means the GET operation)
             * rh - hostname in the request (HTTP schema + HTTP hostname)
             * ru - uri in the request (encoded url without host name)
             * rq - query parameters in the request (encoded query string)
             * 
             * Example 1:
             * http://alan-oam.alanoim.com:14100/oam/server/obrareq.cgi?wh%3DWebGate10g%20wu%3D%2Foamtest%2Findex.aspx%3Fname%3Dalan%26sex%3Dmale%20wo%3D1%20rh%3Dhttp%3A%2F%2Faca-server.achievo.com%20ru%3D%252Foamtest%252Findex.aspx%20rq%3Dname%253Dalan%2526sex%253Dmale
             * http://alan-oam.alanoim.com:14100/oam/server/obrareq.cgi?wh=WebGate10g wu=/oamtest/index.aspx?name=alan&sex=male wo=1 rh=http://aca-server.achievo.com ru=/oamtest/index.aspx rq=name=alan&sex=male
             * 
             * Example 2:
             * http://alan-oam.alanoim.com:14100/oam/server/obrareq.cgi?wh%3DIAMSuiteAgent+wu%3D%2Foamconsole%2Ffaces%2Fpages%2FPolicyManager.jspx+wo%3DGET+rh%3Dhttp%3A%2F%2Falan-oam.alanoim.com%3A7001%2Foamconsole%2Ffaces%2Fpages+ru%3D%2Foamconsole%2Ffaces%2Fpages%2FPolicyManager.jspx
             * http://alan-oam.alanoim.com:14100/oam/server/obrareq.cgi?wh=IAMSuiteAgent wu=/oamconsole/faces/pages/PolicyManager.jspx wo=GET rh=http://alan-oam.alanoim.com:7001/oamconsole/faces/pages ru=/oamconsole/faces/pages/PolicyManager.jspx
             */

            string challengeUrl = oamConfig.ChallengeUrl;
            string queryString = requestUrl.Query.Length > 0 ? requestUrl.Query.Substring(1) : String.Empty;

            StringBuilder destUrl = new StringBuilder();
            destUrl.Append(HttpUtility.UrlEncode("wh=" + oamConfig.HostIdentifier));
            destUrl.Append(" " + HttpUtility.UrlEncode("wu=" + requestUrl.PathAndQuery.ToLower()));

            /*
             * The wq parameter is used by OAM according to the Query String to checking if the resource is protected or not.
             * Configuration: Application Domain -> Resources -> Resource -> Query String
             * 
             * But in ACA/AMCA, all resources should be configure as protected, so this setting is useless in ACA.
             * And if passed wq parameter to OAM, it's maybe occurs the below errors:
             * ---
             * <Warning> <oracle.oam.controller> <OAM-02073> <Error while checking if the resource is protected or not.>
             * oracle.security.am.engines.authz.AuthorizationException: OAMSSA-14003: Policy runtime failed.
             * oracle.security.am.common.policy.runtime.PolicyEvaluationException: OAMSSA-06191: The runtime request contains no resource.
             * ---
             * If the request URL changed to shorter, the error will disappeared, seems the request URL is exceed the maxium length.
             * So we do not pass the wq parameter to OAM.

            if (!String.IsNullOrEmpty(queryString))
            {
                destUrl.Append(" " + HttpUtility.UrlEncode("wq=" + HttpUtility.UrlEncode(queryString)));
            }
             */

            destUrl.Append(" " + HttpUtility.UrlEncode("wo=" + httpMethod));
            destUrl.Append(" " + HttpUtility.UrlEncode("rh=" + requestUrl.Scheme + "://" + requestUrl.Authority));
            destUrl.Append(" " + HttpUtility.UrlEncode("ru=" + HttpUtility.UrlEncode(requestUrl.AbsolutePath.ToLower())));

            if (!String.IsNullOrEmpty(queryString))
            {
                destUrl.Append(" " + HttpUtility.UrlEncode("rq=" + HttpUtility.UrlEncode(queryString)));
            }

            string loginUrl = challengeUrl + "?" + HttpUtility.UrlPathEncode(destUrl.ToString());

            if (!String.IsNullOrEmpty(extraQueryString))
            {
                loginUrl += "&" + extraQueryString;
            }

            return loginUrl;
        }

        /// <summary>
        /// Build request resource for the user permission evaluation from OAM server.
        /// </summary>
        /// <param name="request">User requested http request instance.</param>
        /// <returns>ObResourceRequest object.</returns>
        private ObResourceRequestMgd BuildRequestResource(HttpRequest request)
        {
            return new ObResourceRequestMgd("HTTP", "//" + oamConfig.HostIdentifier + request.Url.PathAndQuery, request.HttpMethod);
        }

        /// <summary>
        /// Gets the initial page for ACA and AMCA.
        /// </summary>
        /// <param name="request">HTTP request.</param>
        /// <param name="response">HTTP response.</param>
        /// <returns>The initial page. For ACA, it's Welcome page; For AMCA, it's Default page.</returns>
        private string GetInitialPage(HttpRequest request, HttpResponse response)
        {
            string initialPage;

            if (IsAmcaRequest(request))
            {
                initialPage = response.ApplyAppPathModifier("~/amca/default.aspx");
            }
            else
            {
                initialPage = response.ApplyAppPathModifier(ACAConstant.URL_DEFAULT);
            }

            return initialPage;
        }
    }
}