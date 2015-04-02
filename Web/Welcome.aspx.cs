/**
 * <pre>
 * 
 *  Accela Citizen Access
 *  File: Welcome.aspx.cs
 * 
 *  Accela, Inc.
 *  Copyright (C): 2008-2014
 * 
 *  Description:
 * 
 *  Notes:
 *      $Id: Welcome.aspx.cs 278219 2014-08-29 07:09:06Z ACHIEVO\james.shi $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

using System;
using System.Collections.Generic;
using System.IO;
using System.Web.Security;
using Accela.ACA.BLL.Admin;
using Accela.ACA.BLL.CustomComponent;
using Accela.ACA.BLL.People;
using Accela.ACA.Common;
using Accela.ACA.Common.Log;
using Accela.ACA.Common.Util;
using Accela.ACA.CustomizeAPI;
using Accela.ACA.SSOInterface;
using Accela.ACA.Web.Common;
using Accela.ACA.Web.SocialMedia;
using Accela.ACA.Web.Util;
using Accela.ACA.WSProxy;
using Accela.ACA.WSProxy.WSModel;
using log4net;

namespace Accela.ACA.Web
{
    /// <summary>
    /// the class for welcome.
    /// </summary>
    public partial class Welcome : BasePage
    {
        #region Fields

        /// <summary>
        /// Determines if exists trade name list.
        /// </summary>
        private const string EXISTS_TRADE_NAME_LIST = "EXISTS_TRADE_NAME_LIST";

        /// <summary>
        /// The standard component [Welcome Text] in welcome page
        /// </summary>
        private const string COMPONENT_WELCOME_TEXT = "Welcome Text";

        /// <summary>
        /// The standard component [Content Link] in welcome page
        /// </summary>
        private const string COMPONENT_CONTENT_LINK = "Content Link";

        /// <summary>
        /// The customize component in welcome page
        /// </summary>
        private const string COMPONENT_DEFAULT_CUSTOMIZED = "Customize Component";

        /// <summary>
        /// logger object.
        /// </summary>
        private static readonly ILog Logger = LogFactory.Instance.GetLogger(typeof(Welcome));

        /// <summary>
        /// The custom component BLL
        /// </summary>
        private ICustomComponentBll _customComponentBll = (ICustomComponentBll)ObjectFactory.GetObject(typeof(ICustomComponentBll));
        
        /// <summary>
        /// Indicates whether have a non-anonymous user entered this page.
        /// </summary>
        private bool? _isUserLoggedIn = null;

        #endregion Fields

        #region Properties

        /// <summary>
        /// Gets current page's page id
        /// </summary>
        public override string PageID
        {
            get
            {
                string pageID = base.PageID;

                // if registered status, get the page id use the url [PAGE_WELCOME_REGISTERED] that match aca_admin_tree.
                if (IsLoggedInStatus())
                {
                    IAdminBll adminBll = ObjectFactory.GetObject(typeof(IAdminBll)) as IAdminBll;
                    pageID = adminBll.GetPageIDbyUrl(ACAConstant.PAGE_WELCOME_REGISTERED);
                }

                return pageID;
            }
        }

        #endregion Properties

        /// <summary>
        /// Raise <c>OnInit</c> event
        /// </summary>
        /// <param name="e">The EventArgs.</param>
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            if (IsLoggedInStatus())
            {
                com_welcome_text_startInfo.LabelKey = "com_welcome_text_startInfo_registered";
                lblInstruction.LabelKey = "aca_welcome_label_registeredins";
            }
            else
            {
                com_welcome_text_startInfo.LabelKey = "com_welcome_text_startInfo";
                lblInstruction.LabelKey = "aca_welcome_label_anonymousins";
            }
        }

        /// <summary>
        /// Raises the page load event
        /// </summary>
        /// <param name="sender">An object that contains the event sender.</param>
        /// <param name="e">A System.EventArgs object containing the event data.</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            SocialMediaUtil.TryRedirectToFacebookHome(false);

            //If login successfully, invoke after login EMSE event.
            string emseMessage = string.Empty;

            //If current authentication adapter is internal adapter, execute the After Login EMSE.
            if (AuthenticationUtil.IsInternalAuthAdapter && IsLoginSuccess())
            {
                try
                {
                    emseMessage = RunAfterLoginEvent();
                }
                catch (ACAException ex)
                {
                    Logger.Error(ex);
                    MessageUtil.ShowMessage(Page, MessageType.Error, ex.Message);
                }
            }

            // show components and standard blocks
            ShowComponents();
            ShowLoginBox();
            ShowTopShoppingCartItems();

            string expiredMessage = CheckLicense();

            // handle with EMSE notice and license expired message.
            if (!string.IsNullOrEmpty(emseMessage) || !string.IsNullOrEmpty(expiredMessage))
            {
                string noticeMessage = emseMessage;

                if (!string.IsNullOrEmpty(noticeMessage))
                {
                    noticeMessage += "<br/>";
                }

                noticeMessage += expiredMessage;

                MessageUtil.ShowMessage(Page, MessageType.Notice, noticeMessage);
            }

            if (!IsPostBack)
            {
                // handle with onLogin EMSE returnToLogin function.
                string returnMessage = Request.QueryString[UrlConstant.RETURN_MESSAGE];
                string returnMessageKey = Request.QueryString[UrlConstant.RETURN_MESSAGE_KEY];

                if (string.IsNullOrEmpty(returnMessage) && !string.IsNullOrEmpty(returnMessageKey))
                {
                    returnMessage = GetTextByKey(returnMessageKey);
                }

                if (!string.IsNullOrEmpty(returnMessage))
                {
                    MessageType messageType = EnumUtil<MessageType>.Parse(Request.QueryString[UrlConstant.MESSAGE_TYPE], MessageType.Notice);
                    MessageUtil.ShowMessage(Page, messageType, returnMessage);
                }
            }

            //show a message after creating another application from continue shopping in cap fee page.
            ShowMessage4ContinueShopping();
        }

        /// <summary>
        /// TabDataList Link click event
        /// </summary>
        /// <param name="sender">The sender object.</param>
        /// <param name="arg">The CommonEventArgs.</param>
        protected void TabDataList_LinkClickEvent(object sender, CommonEventArgs arg)
        {
            string url = arg.ArgObject.ToString();

            // create a trade license
            if (url.IndexOf("filterName=" + ACAConstant.REQUEST_PARMETER_TRADE_LICENSE, StringComparison.InvariantCultureIgnoreCase) > 0)
            {
                string moduleName = GetModuleName(url);

                if (!IsExistsTradeNameList(moduleName))
                {
                    //show error message
                    MessageUtil.ShowMessage(Page, MessageType.Error, GetTextByKey("per_tradeName_msg_noTradeName"));
                    divContent.Visible = false;
                }
            }
        }

        /// <summary>
        /// record url, in error page clear the last page.
        /// </summary>
        protected override void RecordUrl()
        {
            Session[ACAConstant.CURRENT_URL] = null;
        }

        /// <summary>
        /// Shows the components in the page.
        /// </summary>
        private void ShowComponents()
        {
            CustomComponentModel searchModel = new CustomComponentModel();
            searchModel.serviceProviderCode = ConfigManager.AgencyCode;
            searchModel.elementID = long.Parse(PageID);

            CustomComponentModel[] list = _customComponentBll.GetComponentConfig(searchModel);

            if (AppSession.IsAdmin)
            {
                ShowComponentsInAdmin(list);
            }
            else
            {
                ShowComponentsInDaily(list);
            }

            // set the standard component
            if (divWelcomeText.Visible)
            {
                ShowLoggedInArea();
            }
        }

        /// <summary>
        /// Shows the components in admin side.
        /// </summary>
        /// <param name="list">The CustomComponentModel list.</param>
        private void ShowComponentsInAdmin(CustomComponentModel[] list)
        {
            // set default component setting
            divWelcomeText.SectionID = string.Format("{1}{0}{2}{0}{3}{0}{4}", ACAConstant.SPLIT_CHAR, 0, COMPONENT_WELCOME_TEXT, ACAConstant.COMMON_Y, string.Empty);
            divContentLink.SectionID = string.Format("{1}{0}{2}{0}{3}{0}{4}", ACAConstant.SPLIT_CHAR, 0, COMPONENT_CONTENT_LINK, ACAConstant.COMMON_Y, string.Empty);
            divCustomComponent.SectionID = string.Format("{1}{0}{2}{0}{3}{0}{4}", ACAConstant.SPLIT_CHAR, 0, COMPONENT_DEFAULT_CUSTOMIZED, ACAConstant.COMMON_N, string.Empty);
            
            if (list != null && list.Length > 0)
            {
                foreach (CustomComponentModel model in list)
                {
                    string sectionID = string.Format("{1}{0}{2}{0}{3}{0}{4}", ACAConstant.SPLIT_CHAR, model.resID, model.componentName, model.visible, model.path);
                    
                    switch (model.componentName)
                    {
                        case COMPONENT_WELCOME_TEXT:
                            divWelcomeText.SectionID = sectionID;
                            break;
                        case COMPONENT_CONTENT_LINK:
                            divContentLink.SectionID = sectionID;
                            break;
                        default:
                            divCustomComponent.SectionID = sectionID;
                            break;
                    }
                }
            }

            // set customize component visible
            phCustomComponent.Visible = false;
            divCustomComponent.Visible = true;
            divAdminCustomComponent.Visible = true;
        }

        /// <summary>
        /// Shows the components in daily side.
        /// </summary>
        /// <param name="list">The CustomComponentModel list.</param>
        private void ShowComponentsInDaily(CustomComponentModel[] list)
        {
            if (list == null || list.Length == 0)
            {
                return;
            }

            foreach (CustomComponentModel model in list)
            {
                switch (model.componentName)
                {
                    case COMPONENT_WELCOME_TEXT:
                        divWelcomeText.Visible = ValidationUtil.IsYes(model.visible);
                        break;
                    case COMPONENT_CONTENT_LINK:
                        divContentLink.Visible = ValidationUtil.IsYes(model.visible);
                        break;
                    default:
                        divCustomComponent.Visible = ValidationUtil.IsYes(model.visible);

                        string virtualPath = CombineWebPath(FileUtil.CustomizeUserControlFolder, model.path);
                        string physicalPath = Server.MapPath(virtualPath);

                        if (divCustomComponent.Visible && File.Exists(physicalPath))
                        {
                            BaseCustomizeComponent customizeComponent = null;

                            try
                            {
                               customizeComponent = LoadControl(virtualPath) as BaseCustomizeComponent;
                            }
                            catch (Exception ex)
                            {
                                // NOT throw exception
                                Logger.Error(ex.Message);
                            }
                            
                            if (customizeComponent != null)
                            {
                                phCustomComponent.Controls.Add(customizeComponent);

                                // execute the Show action
                                ResultMessage result = customizeComponent.Show();
                                if (!result.IsSuccess)
                                {
                                    MessageUtil.ShowMessage(Page, MessageType.Error, result.Message);
                                }
                            }
                        }

                        break;
                }
            }
        }

        /// <summary>
        /// Show top Shopping Cart Items.
        /// </summary>
        private void ShowTopShoppingCartItems()
        {
            if (!IsLoggedInStatus() || !StandardChoiceUtil.IsEnableShoppingCart())
            {
                divTopShoppingCartItems.Visible = false;
            }
        }

        /// <summary>
        /// Return the boolean result of login success info.
        /// Only support internal authentication.
        /// </summary>
        /// <returns>boolean. True means success</returns>
        private bool IsLoginSuccess()
        {
            string lasturl = string.Empty;

            if (Request.UrlReferrer != null)
            {
                lasturl = Request.UrlReferrer.ToString();
            }

            int flag = lasturl != string.Empty ? lasturl.IndexOf(FormsAuthentication.LoginUrl) : -1;

            return flag != -1;
        }

        /// <summary>
        /// Invoke after login EMSE Script event, and send the message to relevant page.
        /// </summary>
        /// <returns>Return the EMSE notice message.</returns>
        private string RunAfterLoginEvent()
        {
            // invoke EMSE.
            EMSEOnLoginResultModel4WS resultModel4ws = RunAfterLoginEMSEEvent();

            if (resultModel4ws == null)
            {
                return string.Empty;
            }

            string resultCode = resultModel4ws.returnCode;
            string resultMessage = resultModel4ws.returnMessage;
            string resultToLoginFlag = resultModel4ws.returnToLogin;
            string noticeMessage = string.Empty;

            // handle with the message is empty but it should display in the page.
            if (string.IsNullOrEmpty(resultMessage))
            {
                resultMessage = " ";
            }

            // handle with the EMSE result.
            if (!string.IsNullOrEmpty(resultCode) && EmseUtil.RETURN_CODE_FOR_DISPLAY_MESSAGE.Equals(resultCode))
            {
                if (!string.IsNullOrEmpty(resultToLoginFlag) && EmseUtil.RETURN_TO_LOGINPAGE_YES.Equals(resultToLoginFlag.ToUpper()))
                {
                    RedirectToLoginPage(resultCode, resultMessage);
                }
                else
                {
                    noticeMessage = resultMessage;
                }
            }

            return noticeMessage;
        }

        /// <summary>
        /// Redirect to Login Page
        /// </summary>
        /// <param name="resultCode">result code</param>
        /// <param name="resultMessage">result message</param>
        private void RedirectToLoginPage(string resultCode, string resultMessage)
        {
            // Redirect to login page and pass ReturnMessage to display
            string extraQueryString = null;

            if (!string.IsNullOrEmpty(resultCode) && EmseUtil.RETURN_CODE_FOR_DISPLAY_MESSAGE.Equals(resultCode))
            {
                extraQueryString = "ReturnMessage=" + Server.UrlEncode(resultMessage);
            }

            AuthenticationUtil.RedirectToLoginPage(extraQueryString);
        }

        #region Private methods

        /// <summary>
        /// Determines whether it exists trade name list by the specified module name.
        /// </summary>
        /// <param name="moduleName">The module name.</param>
        /// <returns>Return true if it exists trade name list, else false.</returns>
        private bool IsExistsTradeNameList(string moduleName)
        {
            if (ViewState[EXISTS_TRADE_NAME_LIST] == null)
            {
                ILicenseProfessionalBll lpBll = (ILicenseProfessionalBll)ObjectFactory.GetObject(typeof(ILicenseProfessionalBll));
                LicenseModel4WS[] list = lpBll.GetTradeNameList(moduleName);

                ViewState[EXISTS_TRADE_NAME_LIST] = list != null && list.Length > 0;
            }

            return (bool)ViewState[EXISTS_TRADE_NAME_LIST];
        }

        /// <summary>
        /// Get module name from url,this module name will be used for getting trade name list
        /// </summary>
        /// <param name="url">string for URL.</param>
        /// <returns>The module name.</returns>
        private string GetModuleName(string url)
        {
            string moduleName = string.Empty;

            int index = url.IndexOf(ACAConstant.MODULE_NAME, StringComparison.InvariantCultureIgnoreCase);
            string partUrl = url.Substring(index);
            partUrl = partUrl.Substring(partUrl.IndexOf('=') + 1);
            int m = partUrl.IndexOf('&');

            // module name is the last parameter
            if (m == -1)
            {
                moduleName = partUrl;
            }
            else
            {
                moduleName = partUrl.Substring(0, m);
            }

            return moduleName;
        }

        /// <summary>
        /// Display different items if person is logged in or not
        /// </summary>
        private void ShowLoggedInArea()
        {
            if (IsLoggedInStatus())
            {
                areaLoggedIn.Visible = true;
                areaNotLoggedIn.Visible = false;
                
                if (AppSession.IsAdmin)
                {
                    labelUserName.Text = GetTextByKey("Administrator");
                }
                else
                {
                    IPeopleBll peopleBll = (IPeopleBll)ObjectFactory.GetObject(typeof(IPeopleBll));
                    string currentUserName = peopleBll.GetContactUserName(AppSession.User.UserModel4WS);
                    labelUserName.Text = currentUserName;
                }
            }
            else
            {
                areaLoggedIn.Visible = false;
                areaNotLoggedIn.Visible = true;
            }
        }

        /// <summary>
        /// Shows the login box.
        /// </summary>
        private void ShowLoginBox()
        {
            if (!IsLoggedInStatus() && (AppSession.IsAdmin || StandardChoiceUtil.IsLoginEnabled()) && AuthenticationUtil.IsInternalAuthAdapter)
            {
                divLogin.Visible = true;
            }
        }

        /// <summary>
        /// Gets a value indicating whether the current status is logged in.
        /// </summary>
        /// <returns>
        /// <c>true</c> means a non-anonymous user entered this page; otherwise, <c>false</c>.
        /// </returns>
        private bool IsLoggedInStatus()
        {
            if (_isUserLoggedIn == null)
            {
                if (AppSession.IsAdmin)
                {
                    _isUserLoggedIn = !string.IsNullOrEmpty(Request.QueryString["registered"]);
                }
                else
                {
                    _isUserLoggedIn = AuthenticationUtil.IsAuthenticated && AppSession.User != null && !AppSession.User.IsAnonymous;
                }
            }

            return (bool)_isUserLoggedIn;
        }

        /// <summary>
        /// handled with the onLogin EMSE event
        /// </summary>
        /// <returns>an EMSEOnLoginResultModel4WS</returns>
        private EMSEOnLoginResultModel4WS RunAfterLoginEMSEEvent()
        {
            OnLoginParamsModel4WS infoModel = new OnLoginParamsModel4WS();
            infoModel.username = AppSession.User.UserID;
            return EmseUtil.RunEMSEScriptOnLogin(ACAConstant.EMSE_AFTER_LOGON, AppSession.User.PublicUserId, infoModel);
        }

        /// <summary>
        /// Check user's licenses.
        /// </summary>
        /// <returns>Get expired message.</returns>
        private string CheckLicense()
        {
            string expiredMessage = string.Empty;
            IList<string> listExpiredLicNums = LicenseUtil.GetExpiredLicNum4User();

            if (listExpiredLicNums != null && listExpiredLicNums.Count > 0)
            {
                string expiredLicNums = DataUtil.ConcatStringListWithComma(listExpiredLicNums);
                string labelKey = GetTextByKey("com_welcome_message_expiredlicense");
                expiredMessage = DataUtil.StringFormat(labelKey, expiredLicNums);
            }

            return expiredMessage;
        }

        /// <summary>
        /// show a message after creating another application from continue shopping in cap fee page.
        /// </summary>
        private void ShowMessage4ContinueShopping()
        {
            string altID = Request.QueryString["altID"];
            string message = string.Format(GetTextByKey("aca_createanotherapplication_success"), altID);
            bool isCreateAnotherApplication = ValidationUtil.IsYes(Request.QueryString["isCreateAnotherApplication"]);

            if (isCreateAnotherApplication)
            {
                MessageUtil.ShowMessage(Page, MessageType.Success, message);
            }
        }

        #endregion Private methods
    }
}
