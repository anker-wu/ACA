#region

/**
 * <pre>
 * 
 *  Accela Citizen Access
 *  File: PublicuserController.cs
 * 
 *  Accela, Inc.
 *  Copyright (C): 2014
 * 
 *  Description:
 *  
 *  Notes:
 *      $Id:PublicuserController.cs 77905 2014-06-11 12:49:28Z ACHIEVO\dennis.fu $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion

using System;
using System.Configuration;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Security.Authentication;
using System.Text;
using System.Web;
using System.Web.Http;
using System.Web.Security;
using Accela.ACA.BLL.MyCollection;
using Accela.ACA.BLL.People;
using Accela.ACA.Common;
using Accela.ACA.Common.Util;
using Accela.ACA.Web.Common;
using Accela.ACA.Web.UserInfo;
using Accela.ACA.Web.Util;
using Accela.ACA.WSProxy;

namespace Accela.ACA.Web.WebApi
{
    /// <summary>
    /// Public user controller
    /// </summary>
    public class PublicuserController : ApiController
    {
        #region controllers

        /// <summary>
        /// Get user information
        /// </summary>
        /// <returns>user login information</returns>
        [HttpGet]
        [ActionName("User")]
        public HttpResponseMessage GetUser()
        {
            string user = string.Empty;
            string name = AppSession.User.UserID;
            string pwd = AppSession.User.Password;

            if (!string.IsNullOrEmpty(name) && !string.IsNullOrEmpty(pwd))
            {
                user = "{\"type\": \"success\",\"Name\": \"" + name + "\",\"Pwd\": \"" + pwd + "\", \"IsRemember\": 0}";
            }
            else
            {
                user = "{\"type\":\"error\"}";
            }

            return new HttpResponseMessage
            {
                Content = new StringContent(user)
            };
        }

        /// <summary>
        /// Sign out information.
        /// </summary>
        /// <returns>sign out function</returns>
        [HttpGet]
        [ActionName("sign-out")]
        public HttpResponseMessage SignOut()
        {
            // Clear the authentication ticket
            FormsAuthentication.SignOut();
           
            // Clear the contents of the current session
            HttpContext.Current.Session.Clear();

            // Tell the system to drop the session reference so that it does 
            // not need to be carried around with the user
             HttpContext.Current.Session.Abandon();

            //Change session ID after user logout.
            string sessionCookieName = ConfigManager.SessionStateCookieName;

            if (HttpContext.Current.Request.Cookies[sessionCookieName] != null)
            {
                // Create the new session ID.
                AccountUtil.CreateNewSessionID(HttpContext.Current);
            }

            //If current authentication adpater implements IExternalAuthAdapter, it will invoke loginout of IExternalAuthAdapter
            AuthenticationUtil.Signout(HttpContext.Current);
            
            return new HttpResponseMessage
            {
                Content = new StringContent("{\"SignOut\":\"Success\"}")
            };
        }

        /// <summary>
        /// User sign in.
        /// </summary>
        /// <param name="userModel">user model</param>
        /// <returns>user login information</returns>
        public HttpResponseMessage SignIn([FromBody]UserModel userModel)
        {
            string[] result = null;

            try
            {
                AuthBySecurityQuestionModel authBySecurityQuestion = SecurityQuestionUtil.GetAuthBySecurityQuestionSetting();
                string urlParam4RemenberUser = string.Empty;

                if (userModel.IsRemember == 1)
                {
                    string encryptedUserName = SecurityUtil.MachineKeyEncode(userModel.Name);
                    urlParam4RemenberUser = string.Concat(UrlConstant.Remembered_User_Name, "=", encryptedUserName);
                }

                PublicUserModel4WS publicUser = AuthenticationUtil.ValidateUser(userModel.Name, userModel.Pwd);

                if (authBySecurityQuestion.Enable
                    && AuthenticationUtil.IsInternalAuthAdapter
                    && !StandardChoiceUtil.IsEnableLdapAuthentication()
                    && SecurityQuestionUtil.IsExistActiveQuestion(publicUser.questions))
                {
                    HttpContext.Current.Session[SessionConstant.SESSION_USER_FOR_LOGIN_SECURITY] = publicUser;
                     string url = "Account/SecurityQuestionVerification.aspx?" + urlParam4RemenberUser;

                    if (!string.IsNullOrEmpty(urlParam4RemenberUser))
                    {
                        url += "&isFromNewUi=Y";
                    }
                    else
                    {
                        url += "isFromNewUi=Y";
                    }

                    result = new[]
                        {
                            "\"type\":\"redirect\"",
                            "\"url\":\"" + url + "\""
                        };
                }
                else
                {
                    string url = string.Empty;
                    AccountUtil.CreateUserContext(publicUser);
                    bool needChangePassword = (ValidationUtil.IsYes(publicUser.needChangePassword) || AccountUtil.IsPasswordExpiration(publicUser));

                    /*
                     * Use the internal authentication and needs to change password.
                     * Will navigate user to change password process.
                     */
                    if (!StandardChoiceUtil.IsEnableLdapAuthentication() && !AuthenticationUtil.IsNeedRegistration && needChangePassword)
                    {
                        // Clear the authentication ticket
                        FormsAuthentication.SignOut();

                        // Clear the contents of their session
                        HttpContext.Current.Session.Clear();

                        string isPasswordExpires = ACAConstant.COMMON_Y.Equals(publicUser.needChangePassword) ? ACAConstant.COMMON_N : ACAConstant.COMMON_Y;
                        url = string.Format("Account/ChangePassword.aspx?IsPasswordExpires={0}&userID={1}&isFromNewUi={2}&{3}", isPasswordExpires, HttpUtility.UrlEncode(publicUser.userID), ACAConstant.COMMON_Y, urlParam4RemenberUser);

                        result = new[]
                        {
                            "\"type\":\"redirect\"",
                            "\"url\":\"" + url + "\""
                        };
                    }
                    else
                    {
                        if (AppSession.User != null)
                        {
                            PublicUserModel4WS publicUserModel = AppSession.User.UserModel4WS;

                            if (!StandardChoiceUtil.IsEnableLdapAuthentication()
                                && !AuthenticationUtil.IsNeedRegistration
                                && SecurityQuestionUtil.IsNeedUpdateUserQuestions(publicUserModel.questions))
                            {
                                //Clear the authentication ticket and clear the current session.
                                FormsAuthentication.SignOut();
                                HttpContext.Current.Session.Clear();

                                HttpContext.Current.Session[SessionConstant.SESSION_USER_FOR_LOGIN_SECURITY] = publicUserModel;
                                url = string.Format("Account/SecurityQuestionUpdate.aspx?isFromNewUi={0}&{1}", ACAConstant.COMMON_Y, urlParam4RemenberUser);
                                result = new[]
                                {
                                    "\"type\":\"redirect\"",
                                    "\"url\":\"" + url + "\""
                                };
                            }
                            else
                            {
                                IPeopleBll peopleBll = ObjectFactory.GetObject<IPeopleBll>();
                                string currentUserName = peopleBll.GetContactUserName(AppSession.User.UserModel4WS); //Login Name
                                string[] nameArr = currentUserName.Split(' ');
                                string firstName = nameArr[0].Substring(0, 1);
                                string lastName = string.Empty;

                                if (nameArr.Length > 1)
                                {
                                    lastName = nameArr[nameArr.Length - 1].Substring(0, 1);
                                }

                                //Create authentication ticket and redirect user to requested location.
                                AccountUtil.CreateAuthTicketAndRedirect(AppSession.User.UserID, false);
                                url = "Dashboard.html?" + urlParam4RemenberUser;

                                // Redirect the original request Url.
                                result = new[]
                                {
                                    "\"type\":\"success\"",
                                    "\"redirectUrl\":\"" + url + "\"",
                                    "\"sessionID\":\"" + HttpContext.Current.Session.SessionID + "\"",
                                    "\"firstName\":\"" + firstName + "\"",
                                    "\"lastName\":\"" + lastName + "\""
                                };
                            }
                        }
                    }
                }
            }
            catch (ConfigurationErrorsException configErr)
            {
                //Show configuration error message.
                result = new[]
                {
                    "\"type\":\"error\"",
                    "\"message:\"" + LabelUtil.GetGlobalTextByKey("aca.error.publicuser.login.fail") + "\""
                };
            }
            catch (AuthenticationException authErr)
            {
                //Show authentication error message.
                result = new[]
                {
                    "\"type:\"error\"",
                    "\"message:\"" + LabelUtil.GetGlobalTextByKey("aca.error.publicuser.login.fail") + "\""
                };
            }
            catch (ACAException ex)
            {
                LoginStatusCode statusCode = EnumUtil<LoginStatusCode>.Parse(ex.Message);

                if (statusCode == LoginStatusCode.NOTREGISTERED)
                {
                    string url = string.Format(
                        "Account/ExistingAccountRegisteration.aspx?{0}={1}&{2}={3}&{4}={5}",
                        UrlConstant.IS_LOGIN_USE_EXISTING_ACCOUNT,
                        ACAConstant.COMMON_Y,
                        UrlConstant.USER_ID_OR_EMAIL,
                        HttpUtility.UrlEncode(userModel.Name),
                        "isFromNewUi",
                        "Y");

                    result = new[]
                        {
                            "\"type\":\"redirect\"",
                            "\"url\":\"" + url + "\""
                        };
                }
                else
                {
                    string errorMessage = AccountUtil.GetErrorMessageByErrorCode(ex.Message);
                    result = new[]
                        {
                            "\"type\":\"error\"",
                            "\"message\":\"" + errorMessage + "\""
                        };
                }
            }

            return new HttpResponseMessage
            {
                Content = new StringContent("{" + string.Join(",", result) + "}")
            };
        }

        /// <summary>
        /// enable RECAPTCHA for login.
        /// </summary>
        /// <returns>is enable RECAPTCHA</returns>
        [HttpGet]
        public bool EnableRecaptchaForLogin()
        {
            return StandardChoiceUtil.IsEnableCaptchaForLogin();
        }

        /// <summary>
        /// login information
        /// </summary>
        /// <returns>get login information</returns>
        [ActionName("login-info")]
        public HttpResponseMessage GetLoginInfo()
        {
            string result = string.Empty;
            IPeopleBll peopleBll = (IPeopleBll)ObjectFactory.GetObject(typeof(IPeopleBll));

            string currentUserName = peopleBll.GetContactUserName(AppSession.User.UserModel4WS); //Login Name

            string[] nameArr = currentUserName.Split(' ');
            string firstName = nameArr[0].Substring(0, 1);
            string lastName = string.Empty;

            if (nameArr.Length > 1)
            {
                lastName = nameArr[nameArr.Length - 1].Substring(0, 1);
            }

            //shorthand Name
            string shorthandName = (firstName + lastName).ToUpper();
            int myCollectionAmount = 0; //Collection Count
            BuildMyCollectionHtml(out myCollectionAmount);
            IMyCollectionBll myCollectionBll = (IMyCollectionBll)ObjectFactory.GetObject(typeof(IMyCollectionBll));
            MyCollectionModel[] myCollections = myCollectionBll.GetCollections4Management(ConfigManager.SuperAgencyCode, AppSession.User.PublicUserId);

            if (myCollections == null)
            {
                myCollections = new MyCollectionModel[0];
            }

            StringBuilder collectionsInfoJson = new StringBuilder();

            string loginInfoJson = "\"LoginInfo\": [" +
                                      "{" +
                                          "\"name\": \"" + currentUserName +
                                          "\",\"shorthandName\": \"" + shorthandName +
                                          "\",\"CollectionsCount\": \"" + myCollections.Length +
                                          "\"" +
                                      "}" +
                                  "]";

            if (myCollections.Length > 0)
            {
                collectionsInfoJson.Append("\"CollectionsInfo\": [");

                foreach (MyCollectionModel myCollectionModel in myCollections)
                {
                    string collectionName = myCollectionModel.collectionName;
                    int capAmount = myCollectionModel.capAmount != null
                        ? Convert.ToInt32(myCollectionModel.capAmount)
                        : 0;
                    collectionsInfoJson.Append("{" + "\"collectionName\": \"" + collectionName + "\",\"capAmount\": \"" + capAmount + "\",\"collectionId\": \"" + myCollectionModel.collectionId + "\"" + "},");
                }

                if (collectionsInfoJson.Length > 1)
                {
                    collectionsInfoJson.Length -= 1;
                }

                collectionsInfoJson.Append("]");

                result = "{" + loginInfoJson + "," + collectionsInfoJson + "}";
            }
            else
            {
                result = "{" + loginInfoJson + "}";
            }

            if (string.IsNullOrEmpty(collectionsInfoJson.ToString()))
            {
                result = "{" + loginInfoJson + "}";
            }
            else
            {
                result = "{" + loginInfoJson + "," + collectionsInfoJson + "}";
            }

            return new HttpResponseMessage
            {
                Content = new StringContent(result)
            };
        }

        /// <summary>
        /// Verify RECAPTCHA.
        /// </summary>
        /// <param name="postUrl">post url</param>
        /// <param name="postData">post data</param>
        /// <returns>verify RECAPTCHA</returns>
        [HttpGet]
        [ActionName("verify-recaptcha")]
        public HttpResponseMessage VerifyRecaptcha(string postUrl, string postData)
        {
            postData += "&remoteip=" + HttpContext.Current.Request.UserHostAddress;
             
            string[] result = Post(postUrl, postData).Split(new[] { '\n' });

            if (result.Length == 2)
            {
                return new HttpResponseMessage
                {
                    Content =
                        new StringContent(
                            string.Format(
                            "{{\"result\":\"{0}\",\"message\":\"{1}\"}}", 
                            result[0],
                            result[1]))
                };
            }

            return new HttpResponseMessage
            {
                Content = new StringContent("{{\"result\":\"false\",\"message\":\"formatError\"}}")
            };
        }

        /// <summary>
        /// Get token
        /// </summary>
        /// <returns>token string</returns>
        [ActionName("access_token")]
        public HttpResponseMessage GetAccessToken()
        {
            string token = AppSession.User.UserToken;

            return new HttpResponseMessage
            {
                Content = new StringContent("{\"token\":\"" + token + "\"}")
            };
        }

        /// <summary>
        /// Get RECAPTCHA 
        /// </summary>
        /// <returns>Get RECAPTCHA setting</returns>
        [ActionName("Recaptcha-Setting")]
        public HttpResponseMessage GetRecaptcha()
        {
            string recaptchaSetting = ApiUtil.GetRecaptchaSetting();

            return new HttpResponseMessage
            {
                Content = new StringContent(recaptchaSetting.ToString())
            };
        }

        /// <summary>
        /// save customize style
        /// </summary>
        /// <param name="furl">from Body URL</param>
        /// <returns>FromBody URL</returns>
        [HttpPost]
        public HttpResponseMessage SaveStyle([FromBody]string furl)
        {
            furl = string.IsNullOrEmpty(furl) ? string.Empty : furl;
            ApiUtil.AddCache(furl, "CurrentStyle");

            return new HttpResponseMessage
            {
                Content = new StringContent(furl)
            };
        }

        /// <summary>
        /// get CSS style
        /// </summary>
        /// <returns>customize style</returns>
        public HttpResponseMessage GetStyle()
        {
            string result = string.Empty;
            object obj = ApiUtil.GetCache("CurrentStyle");

            if (obj != null)
            {
                result = obj.ToString();
            }

            return new HttpResponseMessage
            {
                Content = new StringContent(result)
            };
        }

        #endregion

        #region private methods.

        /// <summary>
        /// HTTP Post.
        /// </summary>
        /// <param name="url">post url</param>
        /// <param name="postData">post data</param>
        /// <returns>Response content</returns>
        private static string Post(string url, string postData)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";

            byte[] data = Encoding.UTF8.GetBytes(postData);
            request.ContentLength = data.Length;
            using (Stream stream = request.GetRequestStream())
            {
                stream.Write(data, 0, data.Length);
            }

            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            using (StreamReader reader = new StreamReader(response.GetResponseStream()))
            {
                return reader.ReadToEnd();
            }
        }

        /// <summary>
        /// Build my collection menu.
        /// </summary>
        /// <param name="myCollectionAmount">my collection amount.</param>
        /// <returns>my collection html string</returns>
        private int BuildMyCollectionHtml(out int myCollectionAmount)
        {
            MyCollectionModel[] myCollections = GetMyCollectionsFromSession();

            if (myCollections == null || myCollections.Length == 0)
            {
                myCollectionAmount = 0;

                return 0;
            }

            myCollectionAmount = myCollections.Length;

            return myCollectionAmount;
        }

        /// <summary>
        /// Get my collections from session
        /// </summary>
        /// <returns>My CollectionModel</returns>
        private MyCollectionModel[] GetMyCollectionsFromSession()
        {
            IMyCollectionBll myCollectionBll = (IMyCollectionBll)ObjectFactory.GetObject(typeof(IMyCollectionBll));

            //Get my collection models from session.
            MyCollectionModel[] myCollections = AppSession.GetMyCollectionsFromSession();

            if (myCollections == null)
            {
                myCollections = myCollectionBll.GetMyCollection(ConfigManager.SuperAgencyCode, AppSession.User.PublicUserId);
                AppSession.SetMyCollectionsToSession(myCollections);
            }

            return myCollections;
        }

        #endregion

        /// <summary>
        /// User Model
        /// </summary>
        public class UserModel
        {
            /// <summary>
            /// Gets or sets user name
            /// </summary>
            public string Name { get; set; }

            /// <summary>
            /// Gets or sets user password
            /// </summary>
            public string Pwd { get; set; }

            /// <summary>
            /// Gets or sets IsRemember
            /// </summary>
            public int IsRemember { get; set; }
        }
    }
}