#region Header

/**
 * <pre>
 *
 *  Accela
 *  File: ACAAuthAdapter.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2014
 *
 *  Description:
 *
 *  Notes:
 * $Id: ACAAuthAdapter.cs 263026 2014-1-7 00:00:00Z ACHIEVO\alan.hu $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using System;
using System.Web;
using System.Web.SessionState;
using Accela.ACA.BLL.Account;
using Accela.ACA.BLL.Common;
using Accela.ACA.Common;
using Accela.ACA.Common.Log;
using Accela.ACA.Web;
using Accela.ACA.Web.Common;
using Accela.ACA.WSProxy;
using log4net;

namespace SSOAuthAdapter
{
    public class ACAAuthAdapter : IAuthAdapter
    {
        private static readonly ILog _log = LogFactory.Instance.GetLogger(typeof(ACAAuthAdapter));

        public bool IsAuthenticated
        {
            get
            {
                bool isAuthed = false;

                HttpRequest request = HttpContext.Current.Request;
                HttpSessionState session = HttpContext.Current.Session;

                //TODO: Check whether the current HTTP request is authenticated, and assign the status to the isAuthed variable.

                if (isAuthed)
                {
                    _log.Debug("Current HTTP request is authenticated.");
                    
                    bool isJustSignedIn = false;

                    //TODO: Check whether the current user just signs in, and assign the status to the isJustSignedIn variable.

                    if (isJustSignedIn)
                    {
                        /*
                         *  If the user just signs in to Accela Citizen Access,
                         *  the user information in the third-party system is required to
                         *  synchronized with that in Accela Citizen Access.
                         */

                        _log.Debug("User just signed in.");

                        PublicUserModel4WS userInfo = null;
                        //TODO: Get the user account information from the third party system and build the PublicUserModel4WS model.

                        _log.Debug("Sync user information to ACA.");

                        //Synchronize the user account information from the SSO system to Accela Citizen Access.
                        PublicUserModel4WS currentUser = SyncUserInfoToACA(userInfo);

                        //Create the user context.
                        AccountUtil.CreateUserContext(currentUser);
                    }
                }
                else
                {
                    //Create user context for anonymous users.
                    AccountUtil.CreateUserContext(AccountUtil.MakeAnonymousUser());

                    //Check whether the current request asks for login.
                    IBizDomainBll bizDomain = ObjectFactory.GetObject<IBizDomainBll>();
                    bool isForceLogin = bizDomain.IsForceLogin(BasePage.GetModuleName(request), request.RawUrl, null);

                    //If the current request is protected and the user is not authenticated, clear the current session and redirect the user to the login page.
                    if (isForceLogin || IsAmcaRequest(request))
                    {
                        _log.Debug("Redirect user to login page.");
                        session.Clear();
                        RedirectToLoginPage();
                    }
                }

                return isAuthed;
            }
        }

        public bool IsInternalAuthAdapter
        {
            get { return false; }
        }

        /// <summary>
        /// Build the SSO URL when Accela Citizen Access need login
        /// </summary>
        /// <remarks>
        /// If the login page is not in the frame of the wrapper page and you want to keep the frame after login, then you need to set the returnUrl parameter of the RedirectToLoginPage method as the following format:
        /// ConfigurationManager.AppSettings["DefaultPageFile"] + "?CurrentURL=" + HttpContext.Current.Request.Url.AbsoluteUri
        /// ConfigurationManager.AppSettings["DefaultPageFile"] is the wrapper page of Accela Citizen Access and configured in the Web.Config file.
        /// The login page is not in the frame of the wrapper page means one of the following conditions:
        /// •Open a new window using the window.open method.
        /// •Redirect the current page to the login page by setting the value of the window.top.location.href property to the login page.
        /// </remarks>
        /// <value>
        /// The login URL.
        /// </value>
        public string LoginUrl
        {
            get 
            {
                return BuildLoginUrl(HttpContext.Current.Request.Url, null);
            }
        }

        public string LogoutUrl
        {
            get { return "http://www.ny.gov/logout.aspx"; }
        }

        public string RegisterUrl
        {
            get { return "http://www.ny.gov/register.aspx"; }
        }

        public void RedirectToLoginPage()
        {
            HttpContext.Current.Response.Redirect(BuildLoginUrl(HttpContext.Current.Request.Url, null));
        }

        public void RedirectToLoginPage(string extraQueryString)
        {
            HttpContext.Current.Response.Redirect(BuildLoginUrl(HttpContext.Current.Request.Url, extraQueryString));
        }

        public void RedirectToLoginPage(string returnUrl, string extraQueryString)
        {
            HttpContext.Current.Response.Redirect(BuildLoginUrl(new Uri(returnUrl), extraQueryString));
        }

        public PublicUserModel4WS ValidateUser(string username, string password)
        {
            throw new NotImplementedException();
        }

        private static string BuildLoginUrl(Uri requestUrl, string extraQueryString)
        {
            string loginUrl = "http://www.ny.gov/login.aspx";
            string returnUrl = HttpUtility.UrlEncode(requestUrl.AbsoluteUri);

            loginUrl += "?returnUrl=" + returnUrl;

            if (!string.IsNullOrWhiteSpace(extraQueryString))
            {
                loginUrl += "&" + extraQueryString;
            }

            return loginUrl;
        }

        /// <summary>
        /// Get a value which indicates whether the specified HTTP request comes from Accela Mobile Citizen Access
        /// (AMCA).AMCA is a web application which is embedded in Accela Citizen Access. AMCA does not support anonymous user while Accela Citizen Access does.
        /// </summary>
        /// <param name="request">HTTP request.</param>
        /// <returns>True for AMCA request, otherwise false.</returns>
        private static bool IsAmcaRequest(HttpRequest request)
        {
            string amcaPath = FileUtil.CombineWebPath(request.ApplicationPath, "amca");
            return request.Url.AbsolutePath.StartsWith(amcaPath, StringComparison.OrdinalIgnoreCase);
        }

        /// <summary>
        /// Synchronize the user information from the specified user model to the Accela Citizen Access user store.
        /// A user will be auto-created if the specified user does not exist in Accela Citizen Access.
        /// Only the properties whose values are not null will be synchronized to Accela Citizen Access.
        /// </summary>
        /// <param name="publicUser">The specified user model contains user information.</param>
        /// <returns>Return the synchronized user model.</returns>
        private static PublicUserModel4WS SyncUserInfoToACA(PublicUserModel4WS publicUser)
        {
            IAccountBll accountBll = ObjectFactory.GetObject<IAccountBll>();
            string agencyCode = ConfigManager.AgencyCode;
            string defaultContactType = "Individual";

            PublicUserModel4WS existingUser = accountBll.Signon4External(agencyCode, publicUser.userID);

            if (existingUser == null)
            {
                /*
                 * If the user does not exist in Accela Citizen Access, create the mapping account in the Accela Citizen Access database.
                 */

                if (!String.IsNullOrWhiteSpace(publicUser.cellPhone))
                {
                    publicUser.receiveSMS = ACAConstant.COMMON_Y;
                }

                /*
                 * Mandatory properties for public user data.
                 * "auditStatus" for public user model and contact data model should be "A", use the const ACAConstant.VALID_STATUS.
                 * "roleType" for public user model should be "0", use the const ACAConstant.ROLE_TYPE_CITIZEN.
                 */

                // Generate a random password for SSO mapping user.
                publicUser.password = AccountUtil.GetRandomPassword();

                // Get Agency code from ACA.
                publicUser.servProvCode = agencyCode;

                // Pass audioID, auditStatus and roleType to public user model.
                publicUser.auditID = publicUser.userID;
                publicUser.auditStatus = ACAConstant.VALID_STATUS;
                publicUser.roleType = ACAConstant.ROLE_TYPE_CITIZEN;

                /*
                 * An active public user must be associated with at least one contact(peopleModel property).
                 * 
                 * Also need provide mandatory properties for contact data model:
                 * PeopleModel4WS(contact data model):
                 * 1. PeopleModel4WS.serviceProviderCode: Agency code for current ACA site, pleae use ConfigManager.AgencyCode to get it.
                 * 2. PeopleModel4WS.email: The e-mail address of the user's contact.
                 * 3. PeopleModel4WS.contactType:
                 *      The role type of the user's contact. The available contact types are defined in the standard choice “CONTACT TYPE” in AA system.
                 *      Typically you can use "Individual" as the default contact type.
                 * 4. PeopleModel4WS.contactTypeFlag:
                 *      The type of the user’s contact. The possible values are either "Individual" or "Organization".
                 */
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

                        if (string.IsNullOrEmpty(peopleModel.contactTypeFlag))
                        {
                            peopleModel.contactTypeFlag = defaultContactType;
                        }

                        peopleModel.serviceProviderCode = agencyCode;
                        peopleModel.auditStatus = ACAConstant.VALID_STATUS;
                    }
                }

                /*
                 * The account created by the createPublicUser interface can be inactive. It depends on the agency's e-mail verification setting.
                 * Call the signon4External method again to validate the status.
                 */
                accountBll.CreatePublicUser(publicUser, null);
                publicUser = accountBll.Signon4External(agencyCode, publicUser.userID);
            }
            else
            {
                //Merge the user information and synchronize it to Accela Citizen Access database.
                accountBll.MergePublicUserInfo(publicUser, ref existingUser);
                existingUser.password = null;
                existingUser.servProvCode = agencyCode;
                accountBll.EditPublicUser(existingUser);
                publicUser = existingUser;
            }

            return publicUser;
        }
    }
}