#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: AccountUtil.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2010-2014
 *
 *  Description:
 *
 *  Notes:
 *      $Id: AccountUtil.cs 170745 2010-04-19 06:22:11Z ACHIEVO\daly.zeng $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;
using System.Web.UI;
using Accela.ACA.BLL.Account;
using Accela.ACA.BLL.Announcement;
using Accela.ACA.BLL.Common;
using Accela.ACA.Common;
using Accela.ACA.Common.Common;
using Accela.ACA.Common.Util;
using Accela.ACA.CustomizeAPI;
using Accela.ACA.UI.Model;
using Accela.ACA.Web.Util;
using Accela.ACA.WSProxy;

namespace Accela.ACA.Web.Common
{
    /// <summary>
    /// Utility class for Account
    /// </summary>
    public class AccountUtil
    {
        #region Private Property

        /// <summary>
        /// Password item of the enable aca standard choice.
        /// </summary>
        private const string PASSWORD_VERIFY = "verify";

        #endregion

        #region public Methods

        /// <summary>
        /// Check password security
        /// </summary>
        /// <param name="password">The user's password</param>
        /// <param name="userName">user name for login</param>
        /// <param name="isForNewClerk">if set to <c>true</c> [is for new clerk].</param>
        /// <returns>
        /// Check result
        /// </returns>
        public static string CheckPasswordSecurity(string password, string userName, bool isForNewClerk)
        {
            if (string.IsNullOrEmpty(password))
            {
                return string.Empty;
            }

            PasswordConditionModel passwordConditionModel = new PasswordConditionModel();
            passwordConditionModel.servProvCode = ConfigManager.AgencyCode;
            passwordConditionModel.password = password;

            if (AppSession.User.IsAuthorizedAgent && isForNewClerk)
            {
                PublicUserModel4WS user = AccountUtil.MakeAnonymousUser();
                passwordConditionModel.userID = user.userSeqNum;
                passwordConditionModel.userName = user.userID;
            }
            else
            {
                passwordConditionModel.userID = AppSession.User.UserSeqNum;
                passwordConditionModel.userName = AppSession.User.UserID;
            }

            // the userID is empty when user regist
            if (!string.IsNullOrEmpty(userName))
            {
                passwordConditionModel.userName = userName;
            }

            IAccountBll accountBll = (IAccountBll)ObjectFactory.GetObject(typeof(IAccountBll));
            PasswordResultModel passwordResult = accountBll.ValidatePasswordSecurity(passwordConditionModel);

            StringBuilder sb = new StringBuilder("{");

            if (passwordResult != null)
            {
                CapUtil.AddKeyValue(sb, "ErrorCode", passwordResult.errorCode.ToString());
                CapUtil.AddKeyValue(sb, "ErrorMessage", ScriptFilter.AntiXssHtmlEncode(passwordResult.errorMessage));
                CapUtil.AddKeyValue(sb, "PwdScore", passwordResult.pwdScore.ToString());
            }

            sb.Length -= 1;
            sb.Append("}");

            return sb.ToString();
        }

        /// <summary>
        /// Create public user information to AppSession and initializes the user context.
        /// </summary>
        /// <param name="publicUser">Public user model.</param>
        public static void CreateUserContext(PublicUserModel4WS publicUser)
        {
            if (publicUser != null)
            {
                HttpRequest request = HttpContext.Current.Request;

                //A valid registered user must associated at least one approved reference contact.
                if (!ACAConstant.ANONYMOUS_FLAG.Equals(publicUser.userSeqNum, StringComparison.OrdinalIgnoreCase))
                {
                    if (publicUser.peopleModel == null
                        || publicUser.peopleModel.Count(p => ContractorPeopleStatus.Approved.Equals(p.contractorPeopleStatus, StringComparison.OrdinalIgnoreCase) || string.IsNullOrEmpty(p.contractorPeopleStatus)) == 0)
                    {
                        string errorMessage = LabelUtil.GetGUITextByKey("aca_message_account_disabled");

                        throw new ACAException(new Exception(errorMessage));
                    }
                }

                //Set user info in session.
                User user = new User();
                user.UserModel4WS = publicUser;
                user.ClientIP = request.UserHostAddress;

                // set the properities that is authorized agent or agent clerk.
                user.IsAuthorizedAgent = false;
                user.IsAgentClerk = false;

                if (string.Equals(publicUser.authServiceProviderCode, ConfigManager.AgencyCode, StringComparison.OrdinalIgnoreCase))
                {
                    user.IsAuthorizedAgent = ACAConstant.PUBLICUSER_TYPE_AUTH_AGENT.Equals(publicUser.accountType, StringComparison.OrdinalIgnoreCase);

                    user.IsAgentClerk =
                        ACAConstant.PUBLICUSER_TYPE_AUTH_AGENT_CLERK.Equals(publicUser.accountType, StringComparison.OrdinalIgnoreCase) &&
                        !string.IsNullOrEmpty(publicUser.authAgentID);

                    user.IsAuthAgentNeedPrinter = !ValidationUtil.IsNo(publicUser.enablePrint);
                }

                user.IsInspector =
                    ACAConstant.PUBLICUSER_TYPE_SELF_CERTIFIED_INSPECTOR.Equals(publicUser.accountType, StringComparison.OrdinalIgnoreCase) ||
                    ACAConstant.PUBLICUSER_TYPE_CONTRACT_INSPECTOR.Equals(publicUser.accountType, StringComparison.OrdinalIgnoreCase);

                if (!IsNewLdapUser(user.UserModel4WS))
                {
                    IAccountBll accountBll = ObjectFactory.GetObject<IAccountBll>();
                    user.UserToken = accountBll.GetUserToken(ConfigManager.AgencyCode, user.UserID);
                }

                AppSession.User = user;

                ApplyUserInfoToContext(user, HttpContext.Current);
            }
        }

        /// <summary>
        /// Apply specific User information to specific HTTP context.
        /// </summary>
        /// <param name="user">User info model.</param>
        /// <param name="context">Specific HTTP context.</param>
        public static void ApplyUserInfoToContext(User user, HttpContext context)
        {
            HttpRequest request = context.Request;
            HttpResponse response = context.Response;

            //Set user ID to SOAP header.
            I18nSoapHeaderExtension.CurrentUser = user != null ? user.PublicUserId : string.Empty;

            //Sets the user context for customized component.
            BaseCustomizeComponent.UserContext.CallerID = user.PublicUserId;
            BaseCustomizeComponent.UserContext.LoginName = user.UserID;
            BaseCustomizeComponent.UserContext.FirstName = user.FirstName;
            BaseCustomizeComponent.UserContext.MiddleName = user.MiddleName;
            BaseCustomizeComponent.UserContext.LastName = user.LastName;
            BaseCustomizeComponent.UserContext.UserToken = user.UserToken;

            string configPath = request.MapPath(FileUtil.GetCustomizeMappingConfigPath());
            FunctionTable.InitCustomizePermission(configPath);

            //Clear TabNav cookie
            HttpCookie tabCookie = request.Cookies["TabNav"];

            if (tabCookie != null)
            {
                tabCookie.HttpOnly = true;
                tabCookie.Expires = DateTime.Now.AddYears(-1);
                response.Cookies.Add(tabCookie);
            }

            //Update announcements' status.
            if (StandardChoiceUtil.IsEnableAnnouncement())
            {
                UpdateAnnouncementsInSession();
            }
        }

        /// <summary>
        /// Check user whether can enter the registration process.
        /// The user will be navigate to Welcome page if system does not allow user enter the registration process.
        /// </summary>
        public static void CheckRegistrationPermission()
        {
            PublicUserModel4WS registerUser = PeopleUtil.GetPublicUserFromSession();

            /* In below scenarios, use can enter the registration process:
             * 1. Is in Admin.
             * 2. Is internal authentication adapter and Registration is enabled.
             * 3. Is internal authentication adapter, Registration is disabled but this request is to fill in the required information for new LDAP user, 
             *    or current logined accout type is Authorized Agent.
             * 4. Is external authentication adapter.
             */
            bool allowAccessRegistration = AppSession.IsAdmin
                || (AuthenticationUtil.IsInternalAuthAdapter && StandardChoiceUtil.IsRegistrationEnabled())
                || (AuthenticationUtil.IsInternalAuthAdapter && !StandardChoiceUtil.IsRegistrationEnabled() 
                    && (IsNewLdapUser(registerUser) || AppSession.User.IsAuthorizedAgent))
                || AuthenticationUtil.IsNeedRegistration;

            if (!allowAccessRegistration)
            {
                HttpContext.Current.Response.Redirect(ACAConstant.URL_DEFAULT);
            }
        }

        /// <summary>
        /// Generate a length is 8-21 random password.
        /// </summary>
        /// <returns>A password string.</returns>
        public static string GetRandomPassword()
        {
            char[] seeds = new char[]
                               {
                                   'a', 'b', 'd', 'c', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'p', 'r', 'q',
                                   's', 't', 'u', 'v', 'w', 'z', 'y', 'x',
                                   '0', '1', '2', '3', '4', '5', '6', '7', '8', '9',
                                   '`', '~', '!', '@', '#', '$', '%', '^', '&', '*', '(', ')', '_', '-', '+', '=', '{',
                                   '[', '}', ']', '|', '\\', ':', ';', '"', '\'', '<', ',', '>', '.', '?', '/',
                                   'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'Q', 'P', 'R',
                                   'T', 'S', 'V', 'U', 'W', 'X', 'Y', 'Z'
                               };

            StringBuilder password = new StringBuilder();
            Random random = new Random();
            int passLength = random.Next(8, 21);

            for (int i = 0; i < passLength; i++)
            {
                password.Append(seeds[random.Next(0, seeds.Length)].ToString());
            }

            return password.ToString();
        }

        /// <summary>
        /// the Check password security is enable for ACA
        /// </summary>
        /// <returns>If enable check, return true</returns>
        public static bool IsEnablePasswordSecurity()
        {
            string desc = StandardChoiceUtil.GetPasswordSecurityConfig(BizDomainConstant.STD_ITEM_ENABLE_ACA_PASSWORD_CHECK);
            
            if (string.IsNullOrEmpty(desc))
            {
                return false;
            }

            string[] securityItems = desc.Split(new string[] { ACAConstant.SPLIT_DOUBLE_VERTICAL }, StringSplitOptions.None);

            if (securityItems == null)
            {
                return false;
            }

            foreach (string item in securityItems)
            {
                if (string.IsNullOrEmpty(item))
                {
                    continue;
                }

                string[] subItems = item.Split(ACAConstant.TIME_SEPARATOR.ToCharArray());

                if (subItems != null && subItems.Length == 2)
                {
                    if (!string.IsNullOrEmpty(subItems[0]) && !string.IsNullOrEmpty(subItems[1])
                        && PASSWORD_VERIFY.Equals(subItems[0].Trim(), StringComparison.InvariantCultureIgnoreCase)
                        && ValidationUtil.IsYes(subItems[1].Trim()))
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        /// <summary>
        /// Indicates whether the LDAP authentication is enabled and the specified <see cref="publicUser"/> is a LDAP user and does not existing in ACA system.
        /// </summary>
        /// <param name="publicUser">Public user model.</param>
        /// <returns>true means the LDAP is enabled and the specified user is a LDAP user and does not existing in ACA.</returns>
        public static bool IsNewLdapUser(PublicUserModel4WS publicUser)
        {
            return StandardChoiceUtil.IsEnableLdapAuthentication() && publicUser != null && publicUser.userSeqNum == "-1";
        }

        /// <summary>
        /// Check the password is expires
        /// </summary>
        /// <param name="publicUser">The public user.</param>
        /// <returns>If password is expires, return true</returns>
        public static bool IsPasswordExpiration(PublicUserModel4WS publicUser)
        {
            if (publicUser == null)
            {
                return false;
            }

            // Get password expriation setting
            IXPolicyBll xPolicyBll = (IXPolicyBll)ObjectFactory.GetObject(typeof(IXPolicyBll));
            XPolicyModel[] xPolicyList = xPolicyBll.GetPolicyListByCategory(BizDomainConstant.STD_ITEM_PASSWORD_EXPRIATION_CHECK, ACAConstant.LEVEL_TYPE_AGENCY, ConfigManager.AgencyCode);

            if (xPolicyList == null || xPolicyList.Length == 0)
            {
                return false;
            }

            // Disable the password expriation checkbox in ACA admin
            if (!ValidationUtil.IsYes(xPolicyList[0].data2) || string.IsNullOrEmpty(xPolicyList[0].data3))
            {
                return false;
            }

            double expirationDay = I18nNumberUtil.ParseNumberFromWebService(xPolicyList[0].data3);
            string expiration = publicUser.passwordChangeDate;

            // If the passwordChangeDate is empty, use the auditDate
            if (string.IsNullOrEmpty(expiration))
            {
                expiration = publicUser.auditDate;
            }

            // If the expiration date is null, system don't check password expiration.
            if (string.IsNullOrEmpty(expiration))
            {
                return false;
            }

            DateTime expirationDate = I18nDateTimeUtil.ParseFromWebService(expiration);
            expirationDate = expirationDate.AddDays(expirationDay);
            ITimeZoneBll timeBll = (ITimeZoneBll)ObjectFactory.GetObject(typeof(ITimeZoneBll));
            DateTime currentAgencyDate = timeBll.GetAgencyCurrentDate(ConfigManager.AgencyCode);

            if (currentAgencyDate != null && currentAgencyDate.Date.CompareTo(expirationDate.Date) >= 0)
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Get Contact model by sequence number.
        /// </summary>
        /// <param name="seqNumber">sequence number</param>
        /// <returns>return json for contact.</returns>
        public static string GetContact(string seqNumber)
        {
            if (AppSession.User.ApprovedContacts == null || AppSession.User.ApprovedContacts.Length == 0)
            {
                return string.Empty;
            }

            PeopleModel4WS model = AppSession.User.ApprovedContacts.ToList().Find(c => c.contactSeqNumber == seqNumber);
            StringBuilder sb = new StringBuilder("{");
            ModelUIFormat.AddKeyValue(sb, "FirstName", ScriptFilter.EncodeJson(model.firstName));
            ModelUIFormat.AddKeyValue(sb, "LastName", ScriptFilter.EncodeJson(model.lastName));
            ModelUIFormat.AddKeyValue(sb, "MiddleName", ScriptFilter.EncodeJson(model.middleName));
            ModelUIFormat.AddKeyValue(sb, "BusinessName", ScriptFilter.EncodeJson(model.businessName));

            string address = string.Empty;
            string zip = string.Empty;
            string countryCode = string.Empty;
            string city = string.Empty;
            string state = string.Empty;

            if (model.compactAddress != null)
            {
                countryCode = model.compactAddress.countryCode;
                address = model.compactAddress.addressLine1;
                city = model.compactAddress.city;
                state = model.compactAddress.state;
                zip = ModelUIFormat.FormatZipShow(model.compactAddress.zip, countryCode);
            }

            ModelUIFormat.AddKeyValue(sb, "Address", ScriptFilter.EncodeJson(address));
            ModelUIFormat.AddKeyValue(sb, "City", city);
            ModelUIFormat.AddKeyValue(sb, "State", state);
            ModelUIFormat.AddKeyValue(sb, "Zip", zip);
            ModelUIFormat.AddKeyValue(sb, "CellPhoneIDD", model.phone2CountryCode);
            ModelUIFormat.AddKeyValue(sb, "CellPhone", ModelUIFormat.FormatPhone4EditPage(model.phone2, countryCode));
            ModelUIFormat.AddKeyValue(sb, "HomePhoneIDD", model.phone1CountryCode);
            ModelUIFormat.AddKeyValue(sb, "HomePhone", ModelUIFormat.FormatPhone4EditPage(model.phone1, countryCode));
            ModelUIFormat.AddKeyValue(sb, "WorkPhoneIDD", model.phone3CountryCode);
            ModelUIFormat.AddKeyValue(sb, "WorkPhone", ModelUIFormat.FormatPhone4EditPage(model.phone3, countryCode));
            ModelUIFormat.AddKeyValue(sb, "FaxIDD", model.faxCountryCode);
            ModelUIFormat.AddKeyValue(sb, "Fax", ModelUIFormat.FormatPhone4EditPage(model.fax, countryCode));
            ModelUIFormat.AddKeyValue(sb, "Email", model.email);
            ModelUIFormat.AddKeyValue(sb, "Country", countryCode);
            sb.Length -= 1;

            sb.Append("}");
            return sb.ToString();
        }

        /// <summary>
        /// Create authentication ticket and redirect user to requested location after login.
        /// </summary>
        /// <param name="userId">Public user ID.</param>
        /// <param name="autoRedirect">
        /// Indicates whether auto redirect user to requested location after create authentication ticket.
        /// If true, will navigation user to requested page, otherwise stay on current page.
        /// </param>
        public static void CreateAuthTicketAndRedirect(string userId, bool autoRedirect)
        {
            //Create authentication cookie for specified user.
            FormsAuthentication.SetAuthCookie(userId, false);

            if (autoRedirect)
            {
                HttpContext current = HttpContext.Current;
                string returnUrl = current.Server.UrlDecode(current.Request.QueryString[UrlConstant.RETURN_URL]);

                /*
                 * Redirect user to requested URL (ReturnUrl in query string).
                 * Or redirect to default url if ReturnUrl are null.
                 * The default url is configured in authentication.forms section in web.config.
                 */
                if (string.IsNullOrEmpty(returnUrl))
                {
                    returnUrl = FormsAuthentication.GetRedirectUrl(userId, false);
                }

                //If the returnUrl does not come from ACA, it redirects to welcome page.
                if (FileUtil.IsExternalUrl(returnUrl))
                {
                    returnUrl = ACAConstant.URL_DEFAULT;
                }

                current.Response.Redirect(returnUrl);
            }
        }

        /// <summary>
        /// Make the anonymous user.
        /// </summary>
        /// <returns>public user model for the anonymous user.</returns>
        public static PublicUserModel4WS MakeAnonymousUser()
        {
            PublicUserModel4WS anonymous = new PublicUserModel4WS();
            anonymous.servProvCode = ConfigManager.AgencyCode;
            anonymous.userSeqNum = "0";
            anonymous.userID = "anonymous";
            anonymous.firstName = "anonymous";

            return anonymous;
        }

        /// <summary>
        /// Create new session ID and apply to specific Http context.
        /// </summary>
        /// <param name="httpContext">The http context.</param>
        /// <returns>new session id</returns>
        public static string CreateNewSessionID(HttpContext httpContext)
        {
            bool redirected, cookieAdded;
            var sessionIdManager = new SessionIDManager();
            string newSessionId = sessionIdManager.CreateSessionID(httpContext);
            sessionIdManager.SaveSessionID(HttpContext.Current, newSessionId, out redirected, out cookieAdded);

            return newSessionId;
        }

        /// <summary>
        /// Get error message by error code, if not login status error code then return original error.
        /// </summary>
        /// <param name="errorCodeOrMsg">Error code or message.</param>
        /// <returns>Error message.</returns>
        public static string GetErrorMessageByErrorCode(string errorCodeOrMsg)
        {
            if (string.IsNullOrEmpty(errorCodeOrMsg))
            {
                return string.Empty;
            }

            string errorMessage = errorCodeOrMsg;
            LoginStatusCode statusCode = EnumUtil<LoginStatusCode>.Parse(errorCodeOrMsg);

            switch (statusCode)
            {
                case LoginStatusCode.DISABLE:
                    errorMessage = LabelUtil.GetTextByKey("aca.error.publicuser.disableAccount", string.Empty);
                    break;

                case LoginStatusCode.INACTIVE:
                    errorMessage = LabelUtil.GetTextByKey("aca.error.publicuser.inactiveAccount", string.Empty);
                    break;

                case LoginStatusCode.LOCKED:
                    errorMessage = LabelUtil.GetTextByKey("aca_login_msg_toomany_failattempts", string.Empty) + " " + LabelUtil.GetTextByKey("aca.error.publicuser.lockedaccount", string.Empty);
                    break;

                case LoginStatusCode.FAIL:
                    errorMessage = LabelUtil.GetTextByKey("aca.error.publicuser.login.fail", string.Empty);
                    break;
            }

            return errorMessage;
        }

        /// <summary>
        /// Check if the user enter correct password while he/she tries to register an existing account
        /// </summary>
        /// <param name="userIdentifier">User id or email.</param>
        /// <param name="password">The Password.</param>
        /// <returns>True if the password is correct for the not-registered user, else false.</returns>
        public static ResultModel IsPasswordCorrect(string userIdentifier, string password)
        {
            bool passValidation = false;
            string errorMessage = string.Empty;

            if (!string.IsNullOrEmpty(userIdentifier) && !string.IsNullOrEmpty(password))
            {
                IAccountBll accountBll = ObjectFactory.GetObject<IAccountBll>();

                try
                {
                    accountBll.Signon(ConfigManager.AgencyCode, userIdentifier, password);
                }
                catch (Exception exp)
                {
                    LoginStatusCode statusCode = EnumUtil<LoginStatusCode>.Parse(exp.Message);

                    if (statusCode == LoginStatusCode.NOTREGISTERED)
                    {
                        passValidation = true;
                    }
                    else if (statusCode == LoginStatusCode.LOCKED)
                    {
                        errorMessage = LabelUtil.GetTextByKey("aca.error.publicuser.lockedaccount", string.Empty);
                    }
                    else if (statusCode == LoginStatusCode.None)
                    {
                        errorMessage = exp.Message;
                    }
                }
            }

            return new ResultModel { entityValue = passValidation, errorMsg = errorMessage };
        }

        /// <summary>
        /// Get announcement list from server
        /// </summary>
        /// <returns>Announcement model list</returns>
        private static List<AnnouncementModel> GetAnnoucementListFromServer()
        {
            try
            {
                IAnnouncementBll announcementBll = (IAnnouncementBll)ObjectFactory.GetObject(typeof(IAnnouncementBll));
                MessageModel[] announcementArray = announcementBll.GetAnnouncementsFromServer();

                List<AnnouncementModel> announcementList = AnnouncementUtil.ConstructDailyModelFromWSModel(announcementArray);

                AppSession.SetAnnouncementsToSession(announcementList);

                return announcementList;
            }
            catch (Exception e)
            {
                throw new ACAException(e);
            }
        }

        /// <summary>
        /// Update announcements in session to server when user login
        /// </summary>
        private static void UpdateAnnouncementsInSession()
        {
            IAnnouncementBll announcementBll = (IAnnouncementBll)ObjectFactory.GetObject(typeof(IAnnouncementBll));

            if (AppSession.User != null && !string.IsNullOrEmpty(AppSession.User.UserID) && !AppSession.User.IsAnonymous)
            {
                List<AnnouncementModel> announcementsInSession = AppSession.GetAnnouncementsFromSession();
                List<AnnouncementModel> announcementsInServer = GetAnnoucementListFromServer();
                List<MessageModel> reads = new List<MessageModel>();

                if (announcementsInSession != null && announcementsInSession.Count > 0 && announcementsInServer != null && announcementsInServer.Count > 0)
                {
                    foreach (AnnouncementModel am in announcementsInSession)
                    {
                        if (am.IsRead && announcementsInServer.Find(a => a.AuditID == am.AuditID && !a.IsRead) != null)
                        {
                            MessageModel messageModel = new MessageModel();
                            messageModel.messageID = am.AuditID;
                            messageModel.servProvCode = AppSession.GetAnnouncementsFromSession().Find(o => o.AuditID == am.AuditID).AnnouncementAgencyCode;
                            reads.Add(messageModel);
                        }
                    }
                }

                if (reads != null && reads.Count > 0)
                {
                    announcementBll.UpdateAnnouncementFromServer(reads.ToArray());
                }
            }
        }

        #endregion
    }
}