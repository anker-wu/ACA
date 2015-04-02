#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: LicenseList.aspx.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2008-2014
 *
 *  Description:
 *
 *  Notes:
 *      $Id: LicenseList.aspx.cs 278861 2014-09-16 14:15:07Z ACHIEVO\james.shi $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using System;
using System.Data;
using System.Web;
using System.Web.UI.WebControls;

using Accela.ACA.BLL.Account;
using Accela.ACA.BLL.People;
using Accela.ACA.Common;
using Accela.ACA.Common.Log;
using Accela.ACA.Common.Util;
using Accela.ACA.Web.Common;
using Accela.ACA.Web.Common.Control;
using Accela.ACA.Web.NewUI;
using Accela.ACA.WSProxy;
using Accela.Web.Controls;
using log4net;

namespace Accela.ACA.Web.Account
{
    /// <summary>
    /// Page to display license list
    /// </summary>
    public partial class LicenseList : BasePage
    {
        #region Fields

        /// <summary>
        /// license number
        /// </summary>
        private const string COMMAND_LICENSENBR = "ConnectLicense";

        /// <summary>
        /// license list
        /// </summary>
        private const string LICENSE_LIST = "LicenseList";

        /// <summary>
        /// Creates an instance of ILog.
        /// </summary>
        private static readonly ILog Logger = LogFactory.Instance.GetLogger(typeof(LicenseList));

        /// <summary>
        /// is register LP account or not
        /// </summary>
        private bool _isRegisterLPAccount;

        #endregion Fields

        #region Properties

        /// <summary>
        /// Gets or sets a value indicating whether current user comes from another agency.
        /// </summary>
        protected bool IsLoginUseExistingAccount
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
        protected string ExistingAccountRegisterationUserID
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

        /// <summary>
        /// Gets License lists data source
        /// </summary>
        private DataTable GridViewDataSource
        {
            get
            {
                if (ViewState[LICENSE_LIST] == null)
                {
                    ViewState[LICENSE_LIST] = GetLicenseProfessionals();
                }

                return (DataTable)ViewState[LICENSE_LIST];
            }
        }

        /// <summary>
        /// Gets a value indicating whether it is from account manager page.
        /// </summary>
        private bool IsFromAccountManager
        {
            get
            {
                return ValidationUtil.IsYes(Request.QueryString[UrlConstant.IS_FROM_ACCOUNT_MANAGER]);
            }
        }

        #endregion Properties

        #region Methods

        /// <summary>
        /// rewrite  On Initial method to initialize component.
        /// </summary>
        /// <param name="e">A System.EventArgs Object containing the event data.</param>
        protected override void OnInit(EventArgs e)
        {
            IsLoginUseExistingAccount = ValidationUtil.IsYes(Request.QueryString[UrlConstant.IS_LOGIN_USE_EXISTING_ACCOUNT]);
            ExistingAccountRegisterationUserID = Request.QueryString[UrlConstant.USER_ID_OR_EMAIL];
            GridViewBuildHelper.SetSimpleViewElements(gdvLicenseList, ModuleName, AppSession.IsAdmin);
            base.OnInit(e);
        }

        /// <summary>
        /// Raises the page load event
        /// </summary>
        /// <param name="sender">An object that contains the event sender.</param>
        /// <param name="e">A System.EventArgs object containing the event data.</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            _isRegisterLPAccount = ACAConstant.COMMON_Y.Equals(Request.QueryString["isRegisterLPAccount"], StringComparison.InvariantCultureIgnoreCase);
            
            if (!IsPostBack)
            {
                if (!AppSession.IsAdmin)
                {
                    if (IsLoginUseExistingAccount)
                    {
                        lblPageInstruction.Visible = false;
                        lblResultClewInfo.Visible = false;
                    }
                    else if (IsFromAccountManager)
                    {
                        lblResultClewInfo.Visible = false;
                        lblResultClewInfo4ActivateAccount.Visible = false;
                    }
                    else
                    {
                        lblResultClewInfo4ActivateAccount.Visible = false;
                    }
                }

                // If it is from another agency and after searching a license, license type and license number will not be empty.
                bool isFromRegisterLicense = !string.IsNullOrEmpty(Request.QueryString[UrlConstant.LICENSE_TYPE])
                                             && !string.IsNullOrEmpty(Request.QueryString[UrlConstant.LICENSE_NBR]);

                if (IsLoginUseExistingAccount && !isFromRegisterLicense)
                {
                    BindExistingAccountLpList();
                }
                else
                {
                    BindLicenseList(true);
                }
            }
        }

        /// <summary>
        /// paid fees command.
        /// </summary>
        /// <param name="sender">An object that contains the event sender.</param>
        /// <param name="e">An System.EventArgs object that contains the event data.</param>
        protected void LicenseList_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == COMMAND_LICENSENBR)
            {
                ConnectLicense(sender, e);
            }

            BindLicenseList(false);
        }

        /// <summary>
        /// Connect license.
        /// </summary>
        /// <param name="sender">the sender object.</param>
        /// <param name="e">the event handler.</param>
        protected void ConnectLicense(object sender, GridViewCommandEventArgs e)
        {
            string licenseNbr = (string)e.CommandArgument;
            DataRow drLicense = FindLicenseFormDataSource(licenseNbr);

            if (drLicense == null)
            {
                return;
            }

            string licenseType = drLicense["LicenseType"].ToString();
            string licenseSeqNbr = drLicense["LicenseSeqNumber"].ToString();
            string isLicExpired = drLicense["IsLicExpired"].ToString();
            string isInsExpired = drLicense["IsInsExpired"].ToString();
            string isBusLicExpired = drLicense["IsBusLicExpired"].ToString();

            bool islicenseExpired = false;

            if (LicenseUtil.EnableExpiredLicense())
            {
                islicenseExpired = ValidationUtil.IsTrue(isLicExpired)
                                    || ValidationUtil.IsTrue(isInsExpired)
                                    || ValidationUtil.IsTrue(isBusLicExpired);
            }

            //2.selected license has added to user account.
            LicenseModel4WS licenseModel = new LicenseModel4WS();
            licenseModel.stateLicense = licenseNbr;
            licenseModel.licenseType = licenseType;
            licenseModel.serviceProviderCode = ConfigManager.AgencyCode;
            licenseModel.licSeqNbr = licenseSeqNbr;

            //3. judge whether the license is valid. If invalid it will display a error message to current page.
            try
            {
                // validate External license professional
                EMSEResultBaseModel4WS resultModel = EmseUtil.RunEMSEValidationLicense(ACAConstant.EMSE_ADD_LICENSE_VALIDATION, AppSession.User.UserID, licenseModel);

                // whether display error message to page
                if (resultModel != null && resultModel.returnCode == EmseUtil.RETURN_CODE_FOR_DISPLAY_MESSAGE)
                {
                    string returnMessage = resultModel.returnMessage;
                    MessageUtil.ShowMessageByControl(Page, MessageType.Error, returnMessage);
                    return;
                }
                else
                {
                    //4. swith page
                    if (_isRegisterLPAccount)
                    {
                        string defaultURL = string.Format(
                            "RegisterLicense.aspx?licenseType={0}&licenseNbr={1}&isLicenseExpired={2}",
                            licenseType,
                            HttpUtility.UrlEncode(licenseNbr),
                            islicenseExpired);

                        bool isFromNewUi = ValidationUtil.IsYes(Request[UrlConstant.IS_FROM_NEWUI]) && StandardChoiceUtil.IsEnableNewTemplate();

                        if (isFromNewUi)
                        {
                            defaultURL = NewUiUtil.GetUrlByResource(defaultURL);
                        }

                        if (IsLoginUseExistingAccount)
                        {
                            defaultURL += string.Format(
                                                    "&{0}={1}&{2}={3}",
                                                    UrlConstant.USER_ID_OR_EMAIL,
                                                    HttpUtility.UrlEncode(ExistingAccountRegisterationUserID),
                                                    UrlConstant.IS_LOGIN_USE_EXISTING_ACCOUNT,
                                                    ACAConstant.COMMON_Y);
                        }

                        UrlHelper.KeepReturnUrlAndRedirect(defaultURL);
                        return;
                    }

                    //5. if add licenses to account manageer, go to account manager page.
                    ILicenseBLL licenseBll = (ILicenseBLL)ObjectFactory.GetObject(typeof(ILicenseBLL));
                    int returnCode = licenseBll.IssueContractorLicense(licenseModel);

                    if (IssueLicenseReturnCode.ISSUE_SUCCESS_AUTO_APPROVED.Equals((IssueLicenseReturnCode)returnCode))
                    {
                        // Update license of user session
                        AppSession.User.UserModel4WS.licenseModel = licenseBll.GetContractorLicenseValidList(ConfigManager.AgencyCode, AppSession.User.UserSeqNum, true);
                    }

                    /*
                     * Success means the License associated to user but not approval.
                     * Success Auto Approved means the License associated to user and already approved.
                     * So if have new License associated to user, the Contractor License List session needs to be clear and initialize again in Account Manager page.
                     */
                    if (IssueLicenseReturnCode.ISSUE_SUCCESS.Equals((IssueLicenseReturnCode)returnCode)
                        || IssueLicenseReturnCode.ISSUE_SUCCESS_AUTO_APPROVED.Equals((IssueLicenseReturnCode)returnCode))
                    {
                        AppSession.User.AllContractorLicenses = null;
                    }

                    Response.Redirect(string.Format(
                                                    "AccountManager.aspx?returnCode={0}&LicenseNbr={1}&isLicenseExpired={2}",
                                                    returnCode,
                                                    HttpUtility.UrlEncode(licenseNbr),
                                                    islicenseExpired));
                }
            }
            catch (ACAException ex)
            {
                Logger.Error(ex);
                MessageUtil.ShowMessageByControl(Page, MessageType.Error, ex.Message);
            }
        }

        /// <summary>
        /// <c>gdvLicenseList</c> Row data bound event.
        /// </summary>
        /// <param name="sender">An object that contains the event sender.</param>
        /// <param name="e">An System.EventArgs object that contains the event data.</param>
        protected void LicenseList_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                AccelaLinkButton lnkLicenseNbr = (AccelaLinkButton)e.Row.FindControl("lnkLicenseNbr");
                string licenseNumber = lnkLicenseNbr.Text;

                DataRow drLicense = FindLicenseFormDataSource(licenseNumber);

                if (drLicense == null)
                {
                    return;
                }

                string licenseType = drLicense["LicenseType"].ToString();
                string licenseSeqNum = drLicense["LicenseSeqNumber"].ToString();

                lnkLicenseNbr.PostBackUrl = FileUtil.AppendApplicationRoot("/GeneralProperty/LicenseeDetail.aspx?LicenseeNumber="
                    + licenseNumber + "&LicenseeType=" + licenseType + "&LicenseSeqNum=" + licenseSeqNum);  
            }
        }

        /// <summary>
        /// click "Search Again" button
        /// </summary>
        /// <param name="sender">An object that contains the event sender.</param>
        /// <param name="e">An System.EventArgs object that contains the event data.</param>
        protected void BackToSearchButton_OnClick(object sender, EventArgs e)
        {
            //RegisterLicense.aspx: Go to register account license search seaction.
            //SearchLicense.aspx: Go to account manager license search seaction.
            string url = _isRegisterLPAccount ? "RegisterLicense.aspx" : "SearchLicense.aspx";

            if (IsLoginUseExistingAccount)
            {
                url += string.Format(
                                "?{0}={1}&{2}={3}",
                                UrlConstant.USER_ID_OR_EMAIL,
                                HttpUtility.UrlEncode(ExistingAccountRegisterationUserID),
                                UrlConstant.IS_LOGIN_USE_EXISTING_ACCOUNT,
                                ACAConstant.COMMON_Y);
            }

            if (IsFromAccountManager)
            {
                string connectorSymbol = url.IndexOf("?") > -1 ? "&" : "?";
                url += string.Format("{0}{1}={2}", connectorSymbol, UrlConstant.IS_FROM_ACCOUNT_MANAGER, ACAConstant.COMMON_Y);
            }


            UrlHelper.KeepReturnUrlAndRedirect(url);
        }

        /// <summary>
        /// bind data source,
        /// </summary>
        /// <param name="reset">whether direct to firstly page</param>
        private void BindLicenseList(bool reset)
        {
            gdvLicenseList.DataSource = GridViewDataSource;

            if (reset)
            {
                gdvLicenseList.PageIndex = 0;
            }

            gdvLicenseList.EmptyDataText = AppSession.IsAdmin ? string.Empty : GetTextByKey("acc_reg_message_noRecord");
            gdvLicenseList.DataBind();
        }

        /// <summary>
        /// Build license model by request parameter
        /// </summary>
        /// <returns>a LicenseModel4WS</returns>
        private LicenseModel4WS BuildQueryParameter()
        {
            string licenseType = Request.QueryString["licenseType"];
            string licenseNbr = Request.QueryString["licenseNbr"];

            LicenseModel4WS licenseModel = new LicenseModel4WS();
            licenseModel.licenseType = licenseType;
            licenseModel.stateLicense = licenseNbr;
            licenseModel.serviceProviderCode = ConfigManager.AgencyCode;

            return licenseModel;
        }

        /// <summary>
        /// get license professions by license number and Type
        /// </summary>
        /// <returns>data table contains license professions</returns>
        private DataTable GetLicenseProfessionals()
        {
            if (AppSession.IsAdmin)
            {
                // build a empty dataTable for license list when admin.
                DataTable dtEmpty = new DataTable();
                return dtEmpty;
            }
            else
            {
                //1. build query parameter
                LicenseModel4WS paramModel = BuildQueryParameter();

                //2. Invoke bll to get license by query parameter
                ILicenseBLL licenseBll = ObjectFactory.GetObject<ILicenseBLL>();

                //3. return licenses table.
                DataTable dt = licenseBll.GetRegistralLicense(paramModel);

                return dt;
            }
        }

        /// <summary>
        /// Find license form data source.
        /// </summary>
        /// <param name="licenseNbr">the license number.</param>
        /// <returns>License data row.</returns>
        private DataRow FindLicenseFormDataSource(string licenseNbr)
        {
            if (GridViewDataSource != null && GridViewDataSource.Rows.Count > 0)
            {
                DataRow[] drLicense = GridViewDataSource.Select("LicenseNumber='" + licenseNbr.Replace("'", "''") + "'");

                if (drLicense.Length > 0)
                {
                    return drLicense[0];
                }
            }

            return null;
        }

        /// <summary>
        /// Display existing account LP list.
        /// </summary>
        private void BindExistingAccountLpList()
        {           
            IAccountBll accountBll = ObjectFactory.GetObject<IAccountBll>();
            LicenseModel4WS[] licenses = accountBll.GetPublicUserAssociatedLicenses(ConfigManager.AgencyCode, ExistingAccountRegisterationUserID);

            if (licenses != null && licenses.Length > 0)
            {
                ILicenseBLL licenseBll = ObjectFactory.GetObject<ILicenseBLL>();
                DataTable dt = licenseBll.BuildLicenseDataTable(licenses);
                ViewState[LICENSE_LIST] = dt;
                BindLicenseList(true);
            }
            else
            {
                string url = string.Format(
                                        "RegisterLicense.aspx?{0}={1}&{2}={3}",
                                        UrlConstant.IS_LOGIN_USE_EXISTING_ACCOUNT,
                                        ACAConstant.COMMON_Y,
                                        UrlConstant.USER_ID_OR_EMAIL,
                                        HttpUtility.UrlEncode(ExistingAccountRegisterationUserID));
                
                if (_isRegisterLPAccount)
                {
                    url += string.Format("&{0}={1}", UrlConstant.IS_REGISTER_LP_ACCOUNT, ACAConstant.COMMON_Y);
                }

                if (ValidationUtil.IsYes(Request[UrlConstant.IS_FROM_NEWUI]) && StandardChoiceUtil.IsEnableNewTemplate())
                {
                    url += string.Format("&{0}={1}", UrlConstant.IS_FROM_NEWUI, ACAConstant.COMMON_Y);
                }

                Response.Redirect(url);
            }
        }

        #endregion Methods
    }
}
