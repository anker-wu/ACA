#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: FacebookPortal.aspx.cs
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
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using Accela.ACA.BLL.Account;
using Accela.ACA.BLL.People;
using Accela.ACA.BLL.SocialMedia;
using Accela.ACA.Common;
using Accela.ACA.Common.Log;
using Accela.ACA.Common.Util;
using Accela.ACA.Web.Common;
using Accela.ACA.Web.SocialMedia;
using Accela.ACA.Web.Util;
using Accela.ACA.WSProxy;
using log4net;

namespace Accela.ACA.Web
{
    /// <summary>
    /// Facebook connect page
    /// </summary>
    public partial class FacebookPortal : BasePageWithoutMaster
    {
        #region Fields

        /// <summary>
        /// log object
        /// </summary>
        private static readonly ILog Logger = LogFactory.Instance.GetLogger(typeof(FacebookPortal));

        #endregion

        #region Methods

        /// <summary>
        /// Handles the <c>PreInit</c> event of the Page control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected void Page_PreInit(object sender, EventArgs e)
        {
            Page.Theme = I18nCultureUtil.UserPreferredCulture;
        }

        /// <summary>
        /// On Initial event
        /// </summary>
        /// <param name="e">event arguments</param>
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            //add the special custom css.
            HtmlHead header;

            if (this.Master != null)
            {
                header = this.Master.FindControl("Head1") as HtmlHead;
            }
            else
            {
                header = this.Header;
            }

            if (header != null)
            {
                HtmlGenericControl cssConnectFile = new HtmlGenericControl("link");
                cssConnectFile.ID = "FacebookConnectCssStyle";
                cssConnectFile.Attributes["type"] = "text/css";
                cssConnectFile.Attributes["rel"] = "stylesheet";
                cssConnectFile.Attributes["href"] = Page.ResolveUrl("~/App_Themes/SocialMedia/connect.css");

                if (header.FindControl(cssConnectFile.ID) == null)
                {
                    header.Controls.Add(cssConnectFile);
                }
            }

            DialogUtil.RegisterScriptForDialog(this.Page);
        }

        /// <summary>
        /// Handles the Load event of the Page control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack || AppSession.IsAdmin)
            {
                return;
            }

            try
            {
                //Disallow to auto create account if the authentication adapter is not internal adapter.
                if (!AuthenticationUtil.IsInternalAuthAdapter)
                {
                    divAutoCreateAccount.Visible = false;
                }

                MessageUtil.HideMessage(Page);

                string fbCode = Request.QueryString["code"];

                string fbAccessToken = Request.QueryString["access_token"];
                string fbExpires = Request.QueryString["expires"];
                bool fromLoginButton = false;

                if (string.IsNullOrEmpty(fbCode))
                {
                    if (!string.IsNullOrEmpty(fbAccessToken) && !string.IsNullOrEmpty(fbExpires))
                    {
                        SocialMediaUtil.SetAccessToken(fbAccessToken, fbExpires);
                        fromLoginButton = true;
                    }
                    else
                    {
                        //Page.ClientScript.RegisterStartupScript(Page.GetType(), "test", "window.location.href = '" + FacebookLogiUrl + "'; ", true);
                        RedirectToPage(SocialMediaUtil.FacebookLoginUrl);
                        return;
                    }
                }

                if (!fromLoginButton)
                {
                    SocialMediaUtil.GetAccessToken(fbCode, true);
                }

                FacebookUser fbUser = SocialMediaUtil.GetObjectSocialMedia<FacebookUser>(SocialMediaUtil.FB_PROFILE_URL, fbCode);

                Session[SessionConstant.FACEBOOK_USER] = fbUser;

                if (SocialMediaUtil.Signon4SocialMedia(fbUser))
                {
                    if (fromLoginButton)
                    {
                        SocialMediaUtil.IsFacebookAppLogin = false;
                        RedirectToPage(ACAConstant.URL_DEFAULT);
                    }
                    else
                    {
                        SocialMediaUtil.IsFacebookAppLogin = true;

                        if (Session[ACAConstant.CURRENT_URL] == null || string.IsNullOrEmpty(Session[ACAConstant.CURRENT_URL].ToString()))
                        {
                            RedirectToPage("FaceBookWrapperPage.aspx?code=" + fbCode);
                        }
                        else
                        {
                            RedirectToPage(AppSession.CurrentURL);
                        }
                    }
                }
                else
                {
                    if (fromLoginButton)
                    {
                        this.AutoCreateACAAccount(fbUser);
                        SocialMediaUtil.IsFacebookAppLogin = false;
                        RedirectToPage(ACAConstant.URL_DEFAULT);
                    }
                }
            }
            catch (ACAException exception)
            {
                if (Logger.IsErrorEnabled)
                {
                    Logger.Error(exception);
                }

                MessageUtil.ShowMessageInPopup(Page, MessageType.Error, exception.Message);
            }
            catch (Exception exception)
            {
                if (Logger.IsErrorEnabled)
                {
                    Logger.Error(exception);
                }
            }
        }

        /// <summary>
        /// Do not record current URL in current page.
        /// </summary>
        protected override void RecordUrl()
        {
            return;
        }

        /// <summary>
        /// Handles the Click event of the <c>btnConnectToFacebook</c> control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected void ConnectToFacebookButton_Click(object sender, EventArgs e)
        {
            try
            {
                if (Session[SessionConstant.FACEBOOK_USER] == null)
                {
                    Response.Redirect(SocialMediaUtil.FacebookLoginUrl, true);
                }

                MessageUtil.HideMessage(Page);

                if (Membership.ValidateUser(txtUserId.Text, txtPassword.Text))
                {
                    User user = AppSession.User;
                    FacebookUser fbUser = Session[SessionConstant.FACEBOOK_USER] as FacebookUser;

                    string fbCode = Request.QueryString["code"];
                    string fbState = Request.QueryString["state"];

                    XSocialMediaUserModel socialMediaUserModel = new XSocialMediaUserModel();
                    socialMediaUserModel.publicUserSeq = string.IsNullOrEmpty(user.UserSeqNum)
                                                             ? 0
                                                             : Convert.ToInt64(user.UserSeqNum);
                    socialMediaUserModel.serviceProviderCode = ConfigManager.AgencyCode;
                    socialMediaUserModel.socialID = fbUser.ID;
                    socialMediaUserModel.socialType = SocialMediaUtil.SOCIAL_MEDIA_TYPE_FACEBOOK + SocialMediaUtil.SOCIAL_MEDIA_ACCOUNT_TYPE_CONNECT;

                    ISocialMediaBll socialMediaBll = ObjectFactory.GetObject<ISocialMediaBll>();

                    socialMediaBll.SaveSocialMedia4PublicUser(socialMediaUserModel);

                    //Create authentication cookie for internal authentication.
                    if (AuthenticationUtil.IsInternalAuthAdapter)
                    {
                        AccountUtil.CreateAuthTicketAndRedirect(AppSession.User.UserID, false);
                    }

                    SocialMediaUtil.IsFacebookAppLogin = true;
                    RedirectToPage("FaceBookWrapperPage.aspx?code=" + fbCode + "&state=" + fbState);
                }
            }
            catch (ACAException exception)
            {
                if (Logger.IsErrorEnabled)
                {
                    Logger.Error(exception);
                }

                lblLoginError.Text = exception.Message;
                ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "showError", "showPopupDiv(true);", true);
            }
        }

        /// <summary>
        /// Handles the Click event of the <c>btnAutoCreatAccout</c> control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected void AutoCreatAccoutButton_Click(object sender, EventArgs e)
        {
            try
            {
                if (Session[SessionConstant.FACEBOOK_USER] == null)
                {
                    Response.Redirect(SocialMediaUtil.FacebookLoginUrl, true);
                }

                string fbCode = Request.QueryString["code"];
                string fbState = Request.QueryString["state"];

                FacebookUser fbUser = Session[SessionConstant.FACEBOOK_USER] as FacebookUser;
                
                AutoCreateACAAccount(fbUser);

                if (SocialMediaUtil.Signon4SocialMedia(fbUser))
                {
                    SocialMediaUtil.IsFacebookAppLogin = true;
                    RedirectToPage("FaceBookWrapperPage.aspx?code=" + fbCode + "&state=" + fbState);
                }
            }
            catch (ACAException exception)
            {
                if (Logger.IsErrorEnabled)
                {
                    Logger.Error(exception);
                }

                MessageUtil.ShowMessageInPopup(Page, MessageType.Error, exception.Message);
            }
        }

        /// <summary>
        /// Autoes the create ACA account.
        /// </summary>
        /// <param name="fbUser">The facebook user.</param>
        private void AutoCreateACAAccount(FacebookUser fbUser)
        {
            IPeopleBll peopleBll = (IPeopleBll)ObjectFactory.GetObject(typeof(IPeopleBll));
            PublicUserModel4WS publicUserModel4Ws = SocialMediaUtil.ConvertFBUser2PublicUserModel(fbUser);
            PeopleModel4WS peopleModel = new PeopleModel4WS()
            {
                firstName = fbUser.FirstName,
                middleName = fbUser.MiddleName,
                lastName = fbUser.LastName,
                birthDate = I18nDateTimeUtil.ConvertDateTimeStringFromUIToWebService(fbUser.Birthday),
                gender = SocialMediaUtil.ConvertGender(fbUser.Gender),
                email = fbUser.Email,
                contactType = ContactType4License.Individual.ToString(),
                serviceProviderCode = ConfigManager.AgencyCode,
                auditStatus = ACAConstant.VALID_STATUS
            };
            
            PeopleModel[] peopleArray = peopleBll.GetRefContactByPeopleModel(ConfigManager.AgencyCode, TempModelConvert.ConvertToPeopleModel(peopleModel), true, false, null);

            if (peopleArray != null && peopleArray.Length > 0)
            {
                peopleModel = TempModelConvert.ConvertToPeopleModel4WS(peopleArray[0]);
            }

            publicUserModel4Ws.peopleModel = new PeopleModel4WS[] { peopleModel };
            publicUserModel4Ws.xSocialMedia = new XSocialMediaUserModel[1];
            publicUserModel4Ws.xSocialMedia[0] = new XSocialMediaUserModel()
            {
                serviceProviderCode = ConfigManager.AgencyCode,
                socialID = fbUser.ID,
                socialType =
                    SocialMediaUtil.SOCIAL_MEDIA_TYPE_FACEBOOK + SocialMediaUtil.SOCIAL_MEDIA_ACCOUNT_TYPE_AUTOCREATE
            };

            IAccountBll accountBll = ObjectFactory.GetObject<IAccountBll>();
            accountBll.CreatePublicUser(publicUserModel4Ws, null);
        }

        /// <summary>
        /// Redirects to page.
        /// </summary>
        /// <param name="pageLocation">Location of the page.</param>
        private void RedirectToPage(string pageLocation)
        {
            Response.Redirect(pageLocation);
        }

        #endregion
    }
}