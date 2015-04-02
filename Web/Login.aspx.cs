/**
 * <pre>
 * 
 *  Accela Citizen Access
 *  File: Login.aspx.cs
 * 
 *  Accela, Inc.
 *  Copyright (C): 2007-2014
 * 
 *  Description:
 *  login page.
 * 
 *  Notes:
 *      $Id: Login.aspx.cs 278148 2014-08-28 07:30:09Z ACHIEVO\james.shi $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

using System;
using Accela.ACA.Common;
using Accela.ACA.Web.Common;
using Accela.ACA.Web.SocialMedia;
using Accela.ACA.Web.Util;

namespace Accela.ACA.Web
{
    /// <summary>
    /// Login page for ACA
    /// </summary>
    public partial class Login : BasePage
    {
        /// <summary>
        /// record url, in error page clear the last page.
        /// </summary>
        protected override void RecordUrl()
        {
            Session[ACAConstant.CURRENT_URL] = null;
        }

        /// <summary>
        /// Raises the page load event
        /// </summary>
        /// <param name="sender">An object that contains the event sender.</param>
        /// <param name="e">A System.EventArgs object containing the event data.</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            SocialMediaUtil.TryRedirectToFacebookHome(true);

            Response.Cache.SetCacheability(System.Web.HttpCacheability.NoCache);

            // if registration links is disabled, Don't allow to access login page
            if (!AppSession.IsAdmin && (!AuthenticationUtil.IsInternalAuthAdapter || !StandardChoiceUtil.IsLoginEnabled()))
            {
                // avoid user enter url directly to login page.
                Response.Redirect(ACAConstant.URL_DEFAULT);
            }

            if (!IsPostBack)
            {
                //Request Feature need login , Show Notice to user.
                if (Request.QueryString[UrlConstant.RETURN_URL] != null)
                {
                    MessageUtil.ShowMessage(Page, MessageType.Notice, GetTextByKey("acc_login_label_forceLoginNote"));
                }

                // handle with onLogin EMSE returnToLogin function.
                string returnMessage = Request.QueryString["ReturnMessage"];
                string returnMessageKey = Request.QueryString["ReturnMessageKey"];

                if (string.IsNullOrEmpty(returnMessage) && !string.IsNullOrEmpty(returnMessageKey))
                {
                    returnMessage = GetTextByKey(returnMessageKey);
                }

                if (!string.IsNullOrEmpty(returnMessage))
                {
                    MessageUtil.ShowMessage(Page, MessageType.Notice, returnMessage);
                }

                //if registration links is disabled, hidden the registration relevant information.
                if (!StandardChoiceUtil.IsRegistrationEnabled())
                {
                    divRegistration.Visible = false;
                }

                if (!AppSession.IsAdmin)
                {
                    // Re-set I18n initial values(DefaultCulture,EnableMultiLanguage,UserPreferredCulture) 
                    // since we need user preferred culture info when calling web methods.
                    StandardChoiceUtil.SetupI18nInitialSettings();
                }
            }
        }

        /// <summary>
        /// Raises the register button event
        /// </summary>
        /// <param name="sender">An object that contains the event sender.</param>
        /// <param name="e">A System.EventArgs object containing the event data.</param>
        protected void RegisterNowButton_Click(object sender, EventArgs e)
        {
            Response.Redirect("./Account/RegisterDisclaimer.aspx");
        }
    }
}
