#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: SocialMediaUtil.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2012-2014
 *
 *  Description:
 *
 * </pre>
 */
#endregion

using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.UI.HtmlControls;

using Accela.ACA.BLL.SocialMedia;
using Accela.ACA.Common;
using Accela.ACA.Common.Log;
using Accela.ACA.Common.Util;
using Accela.ACA.Web.Common;
using Accela.ACA.Web.Util;
using Accela.ACA.WSProxy;
using log4net;

namespace Accela.ACA.Web.SocialMedia
{
    /// <summary>
    /// Utility class for social media.
    /// </summary>
    public static class SocialMediaUtil
    {
        #region Properties

        /// <summary>
        /// social media type
        /// </summary>
        public const string SOCIAL_MEDIA_TYPE_FACEBOOK = "Facebook";

        /// <summary>
        /// facebook associated account type auto create
        /// </summary>
        public const string SOCIAL_MEDIA_ACCOUNT_TYPE_AUTOCREATE = "AutoCreate";

        /// <summary>
        /// facebook associated account type connect
        /// </summary>
        public const string SOCIAL_MEDIA_ACCOUNT_TYPE_CONNECT = "Connect";

        /// <summary>
        /// Gets the facebook profile  URL.
        /// </summary>
        /// <value>The facebook profile URL.</value>
        public const string FB_PROFILE_URL = "https://graph.facebook.com/me?fields=name,email,first_name,middle_name,last_name,birthday,gender,updated_time&";

        /// <summary>
        /// social media auto create session
        /// </summary>
        private const string SOCIAL_MEDIA_AUTOCREATE_SESSION = "AutoCreate";

        /// <summary>
        /// social media login facebook app
        /// </summary>
        private const string SOCIAL_MEDIA_LOGIN_FACEBOOK_APP = "LoginFacebookApp";

        /// <summary>
        /// Log object.
        /// </summary>
        private static readonly ILog Logger = LogFactory.Instance.GetLogger(typeof(SocialMediaUtil));

        /// <summary>
        /// Gets the Facebook OAUTH url
        /// </summary>
        public static string FB_OAUTH_URL
        {
            get
            {
                return "https://graph.facebook.com/oauth/access_token?client_id="
                                          + ConfigManager.FacebookAppId
                                          + "&client_secret=" + ConfigManager.FaceBookAppSecret
                                          + "&scope=email";
            }
        }

        /// <summary>
        /// Gets the portal page URL with the url encode.
        /// </summary>
        /// <value>The this portal URL.</value>
        public static string ThisPortalUrl
        {
            get
            {
                return HttpUtility.UrlEncode(PortalPageUrl);
            }
        }

        /// <summary>
        /// Gets the portal page URL.
        /// </summary>
        /// <value>The portal page URL.</value>
        public static string PortalPageUrl
        {
            get
            {
                return ConfigManager.Protocol + "://" + HttpContext.Current.Request.Url.Authority + FileUtil.AppendApplicationRoot("socialMedia/facebookportal.aspx");
            }
        }

        /// <summary>
        /// Gets the facebook login URL.
        /// </summary>
        /// <value>The facebook login URL.</value>
        public static string FacebookLoginUrl
        {
            get
            {
                return string.Format(
                            "{0}://www.facebook.com/dialog/oauth/?client_id={1}&redirect_uri={2}&state={3}&scope={4}",
                            ConfigManager.Protocol,
                            ConfigManager.FacebookAppId,
                            ThisPortalUrl,
                            HttpContext.Current.Session.SessionID,
                            "email");
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether current user logged in.
        /// </summary>
        /// <value>The facebook app login cookie.</value>
        public static bool IsFacebookAppLogin
        {
            get
            {
                HttpCookie cookie = HttpContext.Current.Request.Cookies[SOCIAL_MEDIA_LOGIN_FACEBOOK_APP];
                return cookie == null || string.IsNullOrEmpty(cookie.Value) ? false : bool.Parse(cookie.Value);
            }

            set
            {
                HttpCookie cookie = new HttpCookie(SOCIAL_MEDIA_LOGIN_FACEBOOK_APP);
                cookie.Value = value.ToString();
                HttpContext.Current.Response.Cookies.Add(cookie);
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether current account is auto created.
        /// </summary>
        /// <value>The is auto create account.</value>
        public static bool IsAutoCreateAccount
        {
            get
            {
                var sessionVar = HttpContext.Current.Session[SOCIAL_MEDIA_AUTOCREATE_SESSION];
                return sessionVar == null || string.IsNullOrEmpty(sessionVar.ToString()) ? false : bool.Parse(sessionVar.ToString());
            }

            set
            {
                HttpContext.Current.Session[SOCIAL_MEDIA_AUTOCREATE_SESSION] = value;
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Sets the open graph info.
        /// </summary>
        /// <param name="capId">The cap id.</param>
        /// <param name="sharedComments">The shared comments.</param>
        /// <param name="dataUrl">The Facebook share data URL.</param>
        /// <param name="header">The html header.</param>
        /// <param name="httpRequest">The HTTP request.</param>
        public static void SetOpenGraphInfo(string capId, string sharedComments, string dataUrl, HtmlHead header, HttpRequest httpRequest)
        {
            if (!AppSession.IsAdmin)
            {
                /*
                 *  <head prefix="og: http://ogp.me/ns# fb: http://ogp.me/ns/fb# website: http://ogp.me/ns/website#">
                 *   <meta property="fb:app_id"      content="214408415335523" /> 
                 *   <meta property="og:type"        content="website" /> 
                 *   <meta property="og:url"         content="Put your own URL to the object here" /> 
                 *   <meta property="og:title"       content="Your Open Graph object title" /> 
                 *   <meta property="og:image"       content="http://ogp.me/logo.png" /> 
                 *   <meta property="og:description" content="Your Open Graph object description" /> 
                 * https://fbcdn-photos-a.akamaihd.net/photos-ak-snc7/v85006/59/214408415335523/app_2_214408415335523_1772055451.gif
                 * 
                 */
                header.Attributes.Add(
                    "prefix", "og: http://ogp.me/ns# fb: http://ogp.me/ns/fb# website: http://ogp.me/ns/website#");

                //<meta property="fb:admins" content="100003864270019" />
                HtmlMeta meta1 = new HtmlMeta();
                meta1.Name = "og:title";
                meta1.Content = capId;
                header.Controls.Add(meta1);

                HtmlMeta meta2 = new HtmlMeta();
                meta2.Name = "og:type";
                meta2.Content = "article";
                header.Controls.Add(meta2);

                HtmlMeta meta3 = new HtmlMeta();
                meta3.Name = "og:image";

                if (!string.IsNullOrWhiteSpace(ConfigManager.FacebookAppId))
                {
                    meta3.Content = ConfigManager.Protocol + "://graph.facebook.com/" + ConfigManager.FacebookAppId + "/picture";
                }
                else
                {
                    meta3.Content = ConfigManager.Protocol + "://" + httpRequest.Url.Host + ImageUtil.GetImageURL("FacebookShareLogo.png");
                }

                header.Controls.Add(meta3);

                HtmlMeta meta4 = new HtmlMeta();
                meta4.Name = "og:site_name";
                meta4.Content = LabelUtil.GetGlobalTextByKey("ACA_TopPage_Title");
                header.Controls.Add(meta4);

                HtmlMeta meta5 = new HtmlMeta();
                meta5.Name = "og:url";
                meta5.Content = dataUrl;
                header.Controls.Add(meta5);

                HtmlMeta meta6 = new HtmlMeta();
                meta6.Name = "og:description";
                meta6.Content = sharedComments;
                header.Controls.Add(meta6);

                HtmlMeta meta7 = new HtmlMeta();
                meta7.Name = "fb:app_id";
                meta7.Content = ConfigManager.FacebookAppId;
                header.Controls.Add(meta7);
            }
        }

        /// <summary>
        /// Redirects to facebook home.
        /// </summary>
        /// <param name="isNeedValidate">if set to <c>true</c> [is need facebook user login validate].</param>
        public static void TryRedirectToFacebookHome(bool isNeedValidate)
        {
            if (IsFacebookAppLogin)
            {
                HttpContext.Current.Response.Write("<script>");

                if (isNeedValidate)
                {
                    HttpContext.Current.Response.Write(" window.location.href='SocialMedia/FacebookPortal.aspx'");
                }
                else
                {
                    HttpContext.Current.Response.Write(" window.location.href='SocialMedia/FaceBookWrapperPage.aspx'");
                }

                HttpContext.Current.Response.Write("</script>");
            }
        }

        /// <summary>
        /// Gets the result from social media.
        /// </summary>
        /// <param name="url">The request Facebook URL.</param>
        /// <param name="code">The Facebook session code.</param>
        /// <returns>The result from social media.</returns>
        public static string GetResultFromSocialMedia(string url, string code)
        {
            string accessToken = GetAccessToken(code, false);
            string result = string.Empty;
            try
            {
                result = GetResult(url, accessToken);
            }
            catch (Exception)
            {
                accessToken = GetAccessToken(code, true);
                GetResult(url, accessToken);
            }

            return result;
        }

        /// <summary>
        /// Gets the object social media.
        /// </summary>
        /// <typeparam name="T">Generic Type.</typeparam>
        /// <param name="url">The request Facebook URL.</param>
        /// <param name="code">The Facebook session code.</param>
        /// <returns>The object social media.</returns>
        public static T GetObjectSocialMedia<T>(string url, string code)
        {
            string jasonString = GetResultFromSocialMedia(url, code);

            return new JavaScriptSerializer().Deserialize<T>(jasonString);
        }

        /// <summary>
        /// Gets the access token.
        /// </summary>
        /// <param name="code">The Facebook session code.</param>
        /// <param name="reset">if set to <c>true</c> [reset access token in session].</param>
        /// <returns>The access token.</returns>
        public static string GetAccessToken(string code, bool reset)
        {
            string accessToken = HttpContext.Current.Session[SessionConstant.SESSION_SOCIAL_MEDIA_ACCESS_TOKEN] == null
                                     ? string.Empty
                                     : HttpContext.Current.Session[SessionConstant.SESSION_SOCIAL_MEDIA_ACCESS_TOKEN].ToString();

            if (reset || string.IsNullOrEmpty(accessToken))
            {
                try
                {
                    accessToken = GetResult(FB_OAUTH_URL + "&redirect_uri=" + ThisPortalUrl + "&code=" + code, null);
                    HttpContext.Current.Session[SessionConstant.SESSION_SOCIAL_MEDIA_ACCESS_TOKEN] = accessToken;
                }
                catch (Exception)
                {
                    accessToken = string.Empty;
                }
            }

            return accessToken;
        }

        /// <summary>
        /// Sets the access token.
        /// </summary>
        /// <param name="accessToken">The access token.</param>
        /// <param name="expires">The expires time.</param>
        public static void SetAccessToken(string accessToken, string expires)
        {
            if (!string.IsNullOrEmpty(accessToken) && !string.IsNullOrEmpty(expires))
            {
                HttpContext.Current.Session[SessionConstant.SESSION_SOCIAL_MEDIA_ACCESS_TOKEN] = string.Format("access_token={0}&expires={1}", accessToken, expires);
            }
        }

        /// <summary>
        /// Signon for the social media.
        /// </summary>
        /// <param name="facebookUser">The facebook user.</param>
        /// <returns>Indicating whether login success.</returns>
        public static bool Signon4SocialMedia(FacebookUser facebookUser)
        {
            if (facebookUser == null || string.IsNullOrEmpty(facebookUser.ID))
            {
                return false;
            }

            bool loginSuccess = false;

            ISocialMediaBll socialMediaBll = ObjectFactory.GetObject<ISocialMediaBll>();
            PublicUserModel4WS publicuser = socialMediaBll.GetPublicUserBySocialMedia(
                ConfigManager.AgencyCode, facebookUser.ID, SOCIAL_MEDIA_TYPE_FACEBOOK + "%");

            if (publicuser != null && !string.IsNullOrEmpty(publicuser.userSeqNum))
            {
                if (publicuser.xSocialMedia != null)
                {
                    var autoCreate = publicuser.xSocialMedia.Where(o => (SOCIAL_MEDIA_TYPE_FACEBOOK + SOCIAL_MEDIA_ACCOUNT_TYPE_AUTOCREATE).Equals(o.socialType)).FirstOrDefault();

                    if (autoCreate != null)
                    {
                        IsAutoCreateAccount = true;
                    }
                    else
                    {
                        IsAutoCreateAccount = false;

                        if (AuthenticationUtil.IsInternalAuthAdapter && !StandardChoiceUtil.IsEnableLdapAuthentication())
                        {
                            /*
                             * If is internal authentication and is not use the LDAP user store.
                             * To check the user's password whether is expired.
                             */
                            PasswordExpireRecirect(publicuser);
                        }
                    }
                }

                //Create authentication cookie for internal authentication.
                if (AuthenticationUtil.IsInternalAuthAdapter)
                {
                    AccountUtil.CreateAuthTicketAndRedirect(publicuser.userID, false);
                }

                AccountUtil.CreateUserContext(publicuser);
                loginSuccess = true;
            }

            return loginSuccess;
        }

        /// <summary>
        /// Converts the FB user to public user model.
        /// </summary>
        /// <param name="fbUser">The facebook user.</param>
        /// <returns>The public user model.</returns>
        public static PublicUserModel4WS ConvertFBUser2PublicUserModel(FacebookUser fbUser)
        {
            return new PublicUserModel4WS()
                {
                    servProvCode = ConfigManager.AgencyCode,
                    userID = "FB" + fbUser.ID,
                    password = AccountUtil.GetRandomPassword(),
                    firstName = fbUser.FirstName,
                    middleName = fbUser.MiddleName,
                    lastName = fbUser.LastName,
                    birthDate = I18nDateTimeUtil.ConvertDateTimeStringFromUIToWebService(fbUser.Birthday),
                    gender = ConvertGender(fbUser.Gender),
                    email = fbUser.Email,
                    auditID = "FB" + fbUser.ID,
                    roleType = ACAConstant.ROLE_TYPE_CITIZEN,
                    auditStatus = ACAConstant.VALID_STATUS
                };
        }

        /// <summary>
        /// Converts the gender.
        /// </summary>
        /// <param name="gender">The gender.</param>
        /// <returns>The gender converted.</returns>
        public static string ConvertGender(string gender)
        {
            string result = string.Empty;

            if ("male".Equals(gender, StringComparison.InvariantCultureIgnoreCase))
            {
                result = "M";
            }
            else
            {
                result = "F";
            }

            return result;
        }

        /// <summary>
        /// Use tiny url to shorten the specified Url.
        /// </summary>
        /// <param name="url">Specified Url</param>
        /// <returns>Tiny Url.</returns>
        public static string TinyUrl(string url)
        {
            string shortUrl = url;

            try
            {
                string tinyUrlAPI = ConfigManager.Protocol + "://tinyurl.com/api-create.php?url=";
                HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(tinyUrlAPI + url);
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    using (Stream stream = response.GetResponseStream())
                    {
                        if (stream != null)
                        {
                            StreamReader streamReader = new StreamReader(stream);
                            shortUrl = streamReader.ReadToEnd();
                            streamReader.Close();
                            streamReader.Dispose();
                            stream.Close();
                        }
                    }
                }
                else
                {
                    Logger.ErrorFormat("Failed to convert to tiny url:{0}-{1}", response.StatusCode, response.StatusDescription);
                }
            }
            catch (Exception exp)
            {
                Logger.Error(exp.Message, exp);
            }

            return shortUrl;
        }

        /// <summary>
        /// Gets the result.
        /// </summary>
        /// <param name="url">The URL.</param>
        /// <param name="accessToken">The access token.</param>
        /// <returns>The result.</returns>
        private static string GetResult(string url, string accessToken)
        {
            string result = string.Empty;
            WebRequest webRequest = WebRequest.Create(url + accessToken);
            HttpWebResponse webResponse;
            webResponse = (HttpWebResponse)webRequest.GetResponse();
            
            if (webResponse != null)
            {
                Stream responseStream = webResponse.GetResponseStream();

                if (responseStream != null)
                {
                    StreamReader streamReader = new StreamReader(responseStream);
                    result = streamReader.ReadToEnd();
                    webResponse.Close();
                    responseStream.Close();
                    streamReader.Close();
                }
            }

            return result;
        }
        
        /// <summary>
        /// Passwords the expire redirect.
        /// </summary>
        /// <param name="user">The user.</param>
        private static void PasswordExpireRecirect(PublicUserModel4WS user)
        {
            bool needChangePassword = user != null &&
                (ValidationUtil.IsYes(user.needChangePassword) ||
                AccountUtil.IsPasswordExpiration(user));

            // need change password.
            if (needChangePassword)
            {
                string isPasswordExpires = ACAConstant.COMMON_Y.Equals(user.needChangePassword)
                                               ? ACAConstant.COMMON_N
                                               : ACAConstant.COMMON_Y;
                string changePasswordUrl =
                    string.Format("~/Account/ChangePassword.aspx?IsPasswordExpires={0}&userID={1}", isPasswordExpires, HttpUtility.UrlEncode(user.userID));

                HttpContext.Current.Response.Redirect(changePasswordUrl);
            }
        }

        #endregion
    }
}