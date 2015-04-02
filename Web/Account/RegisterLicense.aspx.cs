#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: RegisterLicense.aspx.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2008-2014
 *
 *  Description:
 *
 *  Notes:
 *      $Id: RegisterLicense.aspx.cs 278445 2014-09-04 05:44:51Z ACHIEVO\james.shi $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 */

#endregion Header

using System;
using System.Collections;
using System.Web;
using System.Web.UI.WebControls;

using Accela.ACA.BLL.Admin;
using Accela.ACA.BLL.People;
using Accela.ACA.Common;
using Accela.ACA.Common.Util;
using Accela.ACA.Web.Common;
using Accela.ACA.Web.NewUI;
using Accela.ACA.WSProxy;

namespace Accela.ACA.Web.Account
{
    /// <summary>
    /// register license page
    /// </summary>
    public partial class RegisterLicense : BasePage
    {
        #region Fields

        /// <summary>
        /// the url that match the aca admin tree
        /// </summary>
        private string _adminUrl = string.Empty;

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

                if (!string.IsNullOrEmpty(_adminUrl))
                {
                    IAdminBll adminBll = ObjectFactory.GetObject(typeof(IAdminBll)) as IAdminBll;
                    pageID = adminBll.GetPageIDbyUrl(_adminUrl);
                }

                return pageID;
            }
        }

        /// <summary>
        /// Gets selected licenses list.
        /// </summary>
        private IList SelectedLicenses
        {
            get
            {
                if (Session[SessionConstant.SESSION_REGISTER_LICENSES] == null)
                {
                    Session[SessionConstant.SESSION_REGISTER_LICENSES] = new ArrayList();
                }

                return Session[SessionConstant.SESSION_REGISTER_LICENSES] as ArrayList;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether current user comes from another agency.
        /// </summary>
        private bool IsLoginUseExistingAccount
        {
            get
            {
                return Convert.ToBoolean(ViewState["IsLoginUseExistingAccount"]);
            }

            set
            {
                ViewState["IsLoginUseExistingAccount"] = value;
            }
        }

        /// <summary>
        /// Gets or sets the user id which exists in another agency.
        /// </summary>
        private string ExistingAccountRegisterationUserID
        {
            get
            {
                return ViewState["ExistingAccountRegisterationUserID"] as string;
            }

            set
            {
                ViewState["ExistingAccountRegisterationUserID"] = value;
            }
        }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Raises the page load event
        /// </summary>
        /// <param name="sender">An object that contains the event sender.</param>
        /// <param name="e">A System.EventArgs object containing the event data.</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                DropDownListBindUtil.BindLicenseType(ddlLicenseType);

                bool isRegisterLPAccount = ValidationUtil.IsYes(Request.QueryString["isRegisterLPAccount"]);
                IsLoginUseExistingAccount = ValidationUtil.IsYes(Request.QueryString[UrlConstant.IS_LOGIN_USE_EXISTING_ACCOUNT]);
                ExistingAccountRegisterationUserID = Request.QueryString[UrlConstant.USER_ID_OR_EMAIL];

                if (isRegisterLPAccount)
                {
                    Session.Remove(SessionConstant.SESSION_REGISTER_LICENSES);
                    return;
                }

                bool isDisplayLicenseView = ACAConstant.COMMON_Y.Equals(Request.QueryString["isLicenseView"], StringComparison.InvariantCultureIgnoreCase);

                if (AppSession.IsAdmin && isDisplayLicenseView)
                {
                    //construct a virtual license model to display field in aca admin.
                    LicenseModel4WS license = new LicenseModel4WS();

                    //remove old record in session.
                    if (SelectedLicenses == null || SelectedLicenses.Count == 0)
                    {
                        SelectedLicenses.Add(license);
                    }

                    DisplayLicenseView();
                    return;
                }

                // set the url match aca_admin_tree for license view
                _adminUrl = ACAConstant.PAGE_REGISTER_LICENSE_VIEW;

                //add license to session.
                AddLicensetoSession();
            }

            InitButtons();
        }

        /// <summary>
        /// Raises ItemDataBound event
        /// </summary>
        /// <param name="sender">An object that contains the event sender.</param>
        /// <param name="e">A System.EventArgs object containing the event data.</param>
        protected void Repeater1_ItemDataBound(object sender, DataListItemEventArgs e)
        {
            if (e.Item.ItemType != ListItemType.Header)
            {
                int index = e.Item.ItemIndex;
                string cssClass = index % 2 == 0 ? "RegisterLicense_RowEven ACA_TabRow_SmallEven_FontSize_Restore" : "RegisterLicense_RowOdd ACA_TabRow_SmallOdd_FontSize_Restore";
                e.Item.CssClass = cssClass;

                LinkButton lbnDel = (LinkButton)e.Item.FindControl("removeLicenseItem");
                lbnDel.Attributes.Add("index", e.Item.ItemIndex.ToString());

                if (!AppSession.IsAdmin && StandardChoiceUtil.DisabledRemoveLicense())
                {
                    lbnDel.Visible = false;
                }
            }
        }

        /// <summary>
        /// Raises "find another license" button click event
        /// </summary>
        /// <param name="sender">An object that contains the event sender.</param>
        /// <param name="e">A System.EventArgs object containing the event data.</param>
        protected void FindAnotherLicenseButton_Click(object sender, EventArgs e)
        {
            DisplayLicenseDisclaimer();
        }

        /// <summary>
        /// Click 'Find License' button. 
        /// </summary>
        /// <param name="sender">An object that contains the event sender.</param>
        /// <param name="e">A System.EventArgs object containing the event data.</param>
        protected void FindLicenseButton_Click(object sender, EventArgs e)
        {
            string licenseType = ddlLicenseType.Text.Trim();
            string licenseNbr = HttpUtility.UrlEncode(txtLicenseNum.Text.Trim());
            string url = string.Format("LicenseList.aspx?licenseType={0}&licenseNbr={1}&isRegisterLPAccount=Y", licenseType, licenseNbr);

            if (IsLoginUseExistingAccount)
            {
                url += string.Format(
                                    "&{0}={1}&{2}={3}",
                                    UrlConstant.USER_ID_OR_EMAIL,
                                    HttpUtility.UrlEncode(ExistingAccountRegisterationUserID),
                                    UrlConstant.IS_LOGIN_USE_EXISTING_ACCOUNT,
                                    ACAConstant.COMMON_Y);
            }

            bool isFromNewUi = ValidationUtil.IsYes(Request[UrlConstant.IS_FROM_NEWUI]) && StandardChoiceUtil.IsEnableNewTemplate();

            if (isFromNewUi)
            {
                url = NewUiUtil.GetUrlByResource(url);
            }

            UrlHelper.KeepReturnUrlAndRedirect(url);
        }

        /// <summary>
        /// Raises "next account info" button click event
        /// </summary>
        /// <param name="sender">An object that contains the event sender.</param>
        /// <param name="e">A System.EventArgs object containing the event data.</param>
        protected void NextAccountInfoButton_OnClick(object sender, EventArgs e)
        {
            string nextURL = "RegisterEdit.aspx?isRegisterLPAccount=Y";
            bool isFromNewUi = ValidationUtil.IsYes(Request[UrlConstant.IS_FROM_NEWUI]) && StandardChoiceUtil.IsEnableNewTemplate();

            if (isFromNewUi)
            {
                nextURL = NewUiUtil.GetUrlByResource(nextURL);
            }

            AppSession.SetContactSessionParameter(null);
            AppSession.SetRegisterContactSessionParameter(null);

            if (IsLoginUseExistingAccount)
            {
                nextURL += string.Format(
                                        "&{0}={1}&{2}={3}",
                                        UrlConstant.USER_ID_OR_EMAIL,
                                        HttpUtility.UrlEncode(ExistingAccountRegisterationUserID),
                                        UrlConstant.IS_LOGIN_USE_EXISTING_ACCOUNT,
                                        ACAConstant.COMMON_Y);
            }

            UrlHelper.KeepReturnUrlAndRedirect(nextURL);
        }

        /// <summary>
        /// Click on button "Remove" and trigger event
        /// </summary>
        /// <param name="sender">An object that contains the event sender.</param>
        /// <param name="e">A System.EventArgs object containing the event data.</param>
        protected void RemoveLicenseItemButton_OnClick(object sender, EventArgs e)
        {
            //1. get the selected license to be deleted
            LinkButton lbnDelect = (LinkButton)sender;
            int index = Convert.ToInt32(lbnDelect.Attributes["index"]);

            //2. removed the selected license from temp array (viewstate)
            if (index < SelectedLicenses.Count)
            {
                SelectedLicenses.RemoveAt(index);
            }

            if (SelectedLicenses.Count <= 0)
            {
                DisplayLicenseDisclaimer();
                return;
            }

            //3. re-bind the license list to diplay
            BindLicenses();
        }

        /// <summary>
        /// add license to session
        /// </summary>
        private void AddLicensetoSession()
        {
            string licenseType = Request.QueryString["licenseType"];
            string licenseNbr = Request.QueryString["licenseNbr"];

            if (string.IsNullOrEmpty(licenseType) && string.IsNullOrEmpty(licenseNbr))
            {
                return;
            }

            LicenseModel4WS licenseModel = BuildQueryParameter(licenseType, licenseNbr);
            ILicenseBLL licenseBll = ObjectFactory.GetObject<ILicenseBLL>();
            licenseModel = licenseBll.GetLicenseByStateLicNbr(licenseModel);

            //format phone and zip show.
            if (licenseModel == null)
            {
                return;
            }
            
            //save selected license to session.
            bool isExist = false;

            foreach (LicenseModel4WS item in SelectedLicenses)
            {
                if (licenseModel.licSeqNbr == item.licSeqNbr)
                {
                    isExist = true;
                    break;
                }
            }

            if (!isExist)
            {
                SelectedLicenses.Add(licenseModel);
            }

            bool isLicenseExpired = Convert.ToBoolean(Request["isLicenseExpired"]);

            if (isLicenseExpired)
            {
                MessageUtil.ShowMessage(Page, MessageType.Notice, GetTextByKey("acc_message_expiredlicense"));
            }

            //display license view
            DisplayLicenseView();
        }

        /// <summary>
        /// Initialize Buttons.
        /// </summary>
        private void InitButtons()
        {
            if (!AppSession.IsAdmin && StandardChoiceUtil.DisabledAddLicense())
            {
                btnFindAntherLicense.Visible = false;
                lblFindAntherLicense.Visible = false;
            }
        }

        /// <summary>
        /// Bind repeater date 
        /// </summary>
        private void BindLicenses()
        {
            RepLicenseList.DataSource = SelectedLicenses;
            RepLicenseList.DataBind();
        }

        /// <summary>
        /// Build license model by request parameter
        /// </summary>
        /// <param name="licenseType">license type</param>
        /// <param name="licenseNbr">license number</param>
        /// <returns>a LicenseModel4WS</returns>
        private LicenseModel4WS BuildQueryParameter(string licenseType, string licenseNbr)
        {
            LicenseModel4WS licenseModel = new LicenseModel4WS();
            licenseModel.licenseType = licenseType;
            licenseModel.stateLicense = licenseNbr;
            licenseModel.serviceProviderCode = ConfigManager.AgencyCode;

            return licenseModel;
        }

        /// <summary>
        /// display licenses disclaimer in register license account.  
        /// </summary>
        private void DisplayLicenseDisclaimer()
        {
            divConfirmLicense.Visible = false;
            divLicenseDisclaimer.Visible = true;
        }

        /// <summary>
        /// display confirm license in register license account.  
        /// </summary>
        private void DisplayLicenseView()
        {
            divConfirmLicense.Visible = true;
            divLicenseDisclaimer.Visible = false;
            BindLicenses();
        }

        #endregion Methods
    }
}
