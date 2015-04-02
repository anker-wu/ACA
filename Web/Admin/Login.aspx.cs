/**
 * <pre>
 * 
 *  Accela Citizen Access
 *  File: Login.aspx.cs
 * 
 *  Accela, Inc.
 *  Copyright (C): 2008-2014
 * 
 *  Description: 
 * 
 *  Notes:
 *      $Id: Login.aspx.cs 277316 2014-08-14 02:02:02Z ACHIEVO\james.shi $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

using System;
using System.Web;

using Accela.ACA.BLL.Account;
using Accela.ACA.BLL.Admin;
using Accela.ACA.BLL.Common;
using Accela.ACA.Common;
using Accela.ACA.Common.Log;
using Accela.ACA.Common.Util;
using Accela.ACA.Web.Common;
using Accela.ACA.Web.Security;

namespace ACA.Admin
{
    /// <summary>
    /// Login page
    /// </summary>
    public partial class Login : System.Web.UI.Page
    {
        /// <summary>
        /// admin user name
        /// </summary>
        private const string LOGIN_USER_NAME = "ACA_ADMIN_USER_NAME";

        /// <summary>
        /// V360 url string
        /// </summary>
        private const string V360_URL = "V360_URL";

        /// <summary>
        /// Constant string to indicates the agency user has been locked.
        /// </summary>
        private const string USER_LOCKED_EXCEPTION = "com.accela.security.LockedUserException";

        /// <summary>
        /// log4net Logger
        /// </summary>
        private static readonly log4net.ILog Logger = LogFactory.Instance.GetLogger(typeof(Login));

        /// <summary>
        /// time ticks.
        /// </summary>
        private long _timeFlag;

        /// <summary>
        /// Stopwatch instance.
        /// </summary>
        private System.Diagnostics.Stopwatch _watch = null;

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="Login"/> class.
        /// </summary>
        public Login()
        {
            if (Logger.IsDebugEnabled)
            {
                _timeFlag = DateTime.Now.Ticks;
                _watch = new System.Diagnostics.Stopwatch();
                _watch.Start();
                Logger.DebugFormat("==={0}.aspx Load begin [{1}]===", this.GetType().BaseType.Name, _timeFlag.ToString());
            }
        }

        #endregion Constructors

        #region Methods

        /// <summary>
        /// do dispose when leave this page
        /// </summary>
        public override void Dispose()
        {
            base.Dispose();

            if (Logger.IsDebugEnabled)
            {
                _watch.Stop();
                Logger.DebugFormat("==={0}.aspx Load End   [{1}],costs(ms): {2} ms ===", this.GetType().BaseType.Name, _timeFlag.ToString(), _watch.ElapsedMilliseconds.ToString());
                _watch = null;
            }
        }

        /// <summary>
        /// Raise <c>OnPreInit</c> event
        /// </summary>
        /// <param name="e">An System.EventArgs object that contains the event data.</param>
        protected override void OnPreInit(EventArgs e)
        {
            ProductLicenseValidation.ValidateProductLicense(true);

            Page.Theme = "Default";

            if (Logger.IsDebugEnabled)
            {
                Logger.DebugFormat("OnPreInit of {0}.aspx", this.GetType().BaseType.Name);
            }
        }

        /// <summary>
        /// Raises the page load event
        /// </summary>
        /// <param name="sender">An object that contains the event sender.</param>
        /// <param name="e">A System.EventArgs object containing the event data.</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            // clear daily current url
            Session[ACAConstant.CURRENT_URL] = null;
            txtUserId.SetFocusOnError = false;
            txtPassword.SetFocusOnError = false;

            //Clear TabNav cookie
            HttpCookie cookie = HttpContext.Current.Request.Cookies.Get("TabNav");
            if (cookie != null)
            {
                cookie.HttpOnly = true;
                cookie.Expires = DateTime.Now.AddDays(-1);
                cookie.Value = null;
                Response.Cookies.Add(cookie);
            }

            this.DataBind();
            if (!IsPostBack && !SSOLogin())
            {
                this.Title = LabelUtil.GetAdminUITextByKey("acc_admin_page_title");

                AppSession.IsAdmin = false;

                if (Request["timeout"] == "true")
                {
                    tblMsg.Visible = true;
                    MessageUtil.ShowAlertMessage(lblMsg, LabelUtil.GetAdminUITextByKey("aca_admin_login_timeout_msg"));
                }

                string javasxript = "javascript:var keynum;";
                javasxript += " keynum = event.keyCode || event.which;";
                javasxript += " if(keynum==13) {";
                javasxript += " document.getElementById('" + this.btnHidden.ClientID + "').click();} ";
                txtPassword.Attributes.Add("onkeydown", javasxript);

                GetRememberedUser();
            }

            chkRemember.Attributes.Add("title", LabelUtil.RemoveHtmlFormat(LabelUtil.GetAdminUITextByKey("aca_admin_sign_label_rememberMe")));
        }

        /// <summary>
        /// Raises "Login" button click event
        /// </summary>
        /// <param name="sender">An object that contains the event sender.</param>
        /// <param name="e">A System.EventArgs object containing the event data.</param>
        protected void LoginButton_Click(object sender, EventArgs e)
        {
            try
            {
                ISSOBll ssoBll = (ISSOBll)ObjectFactory.GetObject(typeof(ISSOBll));

                AppSession.IsAdmin = true;
                Session[SessionConstant.SESSION_USER_PREFERRED_AGENCYCODE] = null;

                ssoBll.Signon(ConfigManager.AgencyCode, txtUserId.Text, txtPassword.Text);
                Session[SessionConstant.SESSION_ADMIN_USERNAME] = txtUserId.Text;
                RunACAInitialScripts();

                // Sets a Cookie based on RememberMe Checkbox
                CheckRememberMe();

                RedirectToDefaultPage();
            }
            catch (Exception ex)
            {
                Logger.ErrorFormat("Error occurred, error message:{0}", ex);
                string msgKey = "aca_admin_login_fail";

                if (!string.IsNullOrEmpty(ex.Message) &&
                    ex.Message.IndexOf(USER_LOCKED_EXCEPTION, StringComparison.InvariantCultureIgnoreCase) >= 0)
                {
                    msgKey = "acaadmin_account_msg_account_locked";
                }

                AppSession.IsAdmin = false;
                lblMsg.LabelKey = null;
                lblMsg.Text = LabelUtil.GetAdminUITextByKey(msgKey);
                tblMsg.Visible = true;
                MessageUtil.ShowAlertMessage(this, LabelUtil.GetAdminUITextByKey(msgKey));
            }
        }

        /// <summary>
        /// Get the text(label message) by label key.
        /// </summary>
        /// <param name="key">label key which is unique.</param>
        /// <returns>The text(label message) according the key.if can't find the key, return String.Empty.</returns>
        protected string GetTextByKey(string key)
        {
            return LabelUtil.GetAdminUITextByKey(key);
        }

        /// <summary>
        /// check if user has login in V360
        /// </summary>
        /// <returns>true if user has login in V360.</returns>
        private bool SSOLogin()
        {
            string sessionId = Request["sessionid"];

            if (string.IsNullOrEmpty(sessionId))
            {
                return false;
            }

            try
            {
                string v360url = Request["v360url"];
                string userID = Request.QueryString["userid"];

                if (string.IsNullOrEmpty(v360url) || string.IsNullOrEmpty(userID))
                {
                    return false;
                }
                else if (StandardChoiceUtil.IsEnableUrlRefererCheck()
                        && (Request.UrlReferrer == null
                            || !(AntiCsrfAttackUtil.IsTrustedExternalUrl(Request.UrlReferrer.ToString())
                                || AntiCsrfAttackUtil.IsTrustedLocalUrl(Request.UrlReferrer.ToString(), HttpContext.Current))))
                {
                    // Check if SSO Request is from the trusted site(e.g., V360, or local site redirect).
                    return false;
                }
                else
                {
                    SaveCookie(V360_URL, v360url);
                }

                ISSOBll sso = (ISSOBll)ObjectFactory.GetObject(typeof(ISSOBll));
                sso.Authenticate(null, sessionId);

                Session[SessionConstant.SESSION_ADMIN_USERNAME] = userID;

                // Check and init ACA
                RunACAInitialScripts();

                RedirectToDefaultPage();
                return true;
            }
            catch (Exception ex)
            {
                Logger.ErrorFormat("Error occurred, error message:{0}", ex);
                return false;
            }
        }

        /// <summary>
        /// Runs the ACA initial scripts.
        /// </summary>
        private void RunACAInitialScripts()
        {
            IACAInitBll acaInitBll = (IACAInitBll)ObjectFactory.GetObject(typeof(IACAInitBll));
            acaInitBll.InitACA();
        }

        /// <summary>
        /// Redirect to default page
        /// </summary>
        private void RedirectToDefaultPage()
        {
            AppSession.IsAdmin = true;
            IAdminConfigurationPreview preview = (IAdminConfigurationPreview)ObjectFactory.GetObject(typeof(IAdminConfigurationPreview));
            preview.SetPreviewCapModelDummyData();
            Response.Redirect("default.aspx", false);
        }

        /// <summary>
        /// If the remember me is checked then a cookie is added
        /// </summary>
        private void CheckRememberMe()
        {
            //Add by daly. remember user name when this option is checked .
            if (chkRemember.Checked)
            {
                string encryptedUserName = txtUserId.Text;

                if (!string.IsNullOrEmpty(encryptedUserName))
                {
                    encryptedUserName = SecurityUtil.MachineKeyEncode(encryptedUserName);
                }

                SaveCookie(LOGIN_USER_NAME, HttpUtility.UrlEncode(encryptedUserName));
            }
            else
            {
                // let the cookie expires right now.
                HttpCookie cookie = Context.Response.Cookies[LOGIN_USER_NAME];
                cookie.HttpOnly = true;
                cookie.Expires = DateTime.Now.AddDays(-1);
            }
        }

        /// <summary>
        /// Save cookie
        /// </summary>
        /// <param name="name">cookie name</param>
        /// <param name="value">cookie value</param>
        private void SaveCookie(string name, string value)
        {
            HttpCookie cookie = new HttpCookie(name, value);
            cookie.HttpOnly = true;
            TimeSpan cookiesExistTime = new TimeSpan(10, 0, 0, 0);
            cookie.Expires = DateTime.Now + cookiesExistTime;
            Context.Response.Cookies.Add(cookie);
        }

        /// <summary>
        /// Get remembered user info
        /// </summary>
        private void GetRememberedUser()
        {
            HttpCookie cookie = Request.Cookies.Get(LOGIN_USER_NAME);

            if (cookie != null && !string.IsNullOrEmpty(cookie.Value))
            {
                try
                {
                    string decryptedUserName = SecurityUtil.MachineKeyDecode(cookie.Value);

                    txtUserId.Text = decryptedUserName;
                    txtPassword.Focus();
                    chkRemember.Checked = true;
                }
                catch
                {
                    txtUserId.Text = string.Empty;

                    //let the current cookie expire if its user name is failed to decoded
                    cookie.HttpOnly = true;
                    cookie.Expires = DateTime.Now.AddDays(-1);
                    Context.Response.Cookies.Add(cookie);

                    txtUserId.Focus();
                }
            }
            else
            {
                txtUserId.Focus();
            }
        }

        #endregion Methods
    }
}
