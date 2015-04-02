#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: CapFees.aspx.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2007-2014
 *
 *  Description:
 *
 *  Notes:
 *      $Id: CapFees.aspx.cs 278785 2014-09-13 09:51:14Z ACHIEVO\james.shi $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using System;
using System.Web;

using Accela.ACA.BLL.Cap;
using Accela.ACA.BLL.Common;
using Accela.ACA.BLL.EMSE;
using Accela.ACA.Common;
using Accela.ACA.Common.Common;
using Accela.ACA.Common.Log;
using Accela.ACA.Common.Util;
using Accela.ACA.Web.Common;
using Accela.ACA.Web.Payment;
using Accela.ACA.Web.Util;
using Accela.ACA.WSProxy;
using Accela.Web.Controls;
using log4net;

namespace Accela.ACA.Web.Cap
{
    /// <summary>
    /// Cap fee display and recalculation.
    /// </summary>
    /// <remark>
    /// this page comes from below:
    /// 1.Apply a permit
    /// 2.Obtain a Fee Estimate
    /// 3.Search permits.
    /// input parameter(required): the paramters are used to create CapIdModel.
    /// 1.capId1
    /// 2.capId2
    /// 3.capId3
    /// input parameter(optional): 
    /// 1.IsResume. this parameter indicats the current cap whether is a resume cap or not.
    /// </remark>
    public partial class CapFees : BasePage
    {
        #region Fields

        /// <summary>
        /// Loggers object.
        /// </summary>
        private static readonly ILog Logger = LogFactory.Instance.GetLogger(typeof(CapFees));

        /// <summary>
        /// Is for fee estimator.
        /// </summary>
        private bool _is4FeeEstimator;

        #endregion Fields

        #region Properties

        /// <summary>
        /// Gets or sets the error message if limit decimal by admin and detect that the fee item isn't integer. 
        /// </summary>
        protected string ErrorMessage
        {
            get;
            set;
        }

        /// <summary>
        /// Gets a value indicating whether is Pay Fee Due process.
        /// </summary>
        protected bool IsPayFeeDue
        {
            get
            {
                return ACAConstant.COMMON_Y.Equals(Request.QueryString[ACAConstant.REQUEST_PARMETER_ISPAYFEEDUE], StringComparison.InvariantCultureIgnoreCase);
            }
        }

        /// <summary>
        /// Gets a value indicating whether request come from shopping cart or not.
        /// </summary>
        private bool IsFromShoppingCart
        {
            get
            {
                return ACAConstant.COMMON_Y.Equals(Request.QueryString[ACAConstant.FROMSHOPPINGCART]);
            }
        }

        /// <summary>
        /// Gets a value indicating whether the request is from exam schedule or not.
        /// </summary>
        private bool IsFromExamSchedule
        {
            get
            {
                return ValidationUtil.IsYes(Request.QueryString[UrlConstant.IS_FROM_EXAM_SCHEDULE]);
            }
        }

        /// <summary>
        /// Gets a value indicating whether is Renewal process.
        /// </summary>
        private bool IsRenewal
        {
            get
            {
                return ACAConstant.COMMON_Y.Equals(Request.QueryString[ACAConstant.REQUEST_PARMETER_ISRENEWAL], StringComparison.InvariantCultureIgnoreCase);
            }
        }

        /// <summary>
        /// Gets current CAP ID
        /// </summary>
        private CapIDModel4WS CurrentCapID
        {
            get
            {
                if (ViewState["CurrentCapID"] == null)
                {
                    CapModel4WS capModel = AppSession.GetCapModelFromSession(ModuleName);

                    ViewState["CurrentCapID"] = capModel.capID;
                }

                return ViewState["CurrentCapID"] as CapIDModel4WS;
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
            DialogUtil.RegisterScriptForDialog(Page);

            // Redirect error page when CAP Model in session is invalid
            if (CapUtil.IsCAPUpdatedInSession(CurrentCapID, false, ModuleName))
            {
                Logger.ErrorFormat(
                    "CAP Model is updated. The original ID is {0}-{1}-{2}({3})",
                    CurrentCapID.id1, 
                    CurrentCapID.id2, 
                    CurrentCapID.id3, 
                    CurrentCapID.serviceProviderCode);
                throw new InvalidOperationException(GetTextByKey("aca_cap_updated_error"));
            }

            ErrorMessage = GetTextByKey("per_feeItemList_error_quantityInvalidate");

            EtisalatAdapter.InitEtisalatOnlinePayment();

            _is4FeeEstimator = ACAConstant.COMMON_Y.Equals(Request.QueryString["isFeeEstimator"]);

            if (!IsPostBack)
            {
                lnkContinueApplication.AccessKey = AccessibilityUtil.GetAccessKey(AccessKeyType.SubmitForm);
                ValidateAndHandleSession();

                PresentateButton();

                // Run FeeEstimateAfter4ACA EMSE Event
                RunFeeEstimateAfter4ACAEvent();
            }

            if (!IsPostBack)
            {
                capFeeList.ShowFeeInformation();

                capFeeList.DisplayConditions();

                if (!AppSession.IsAdmin)
                {
                    CapModel4WS capModel = AppSession.GetCapModelFromSession(ModuleName);
                    PageFlowGroupModel pageflowGroup = PageFlowUtil.GetPageflowGroup(capModel);

                    if (!StandardChoiceUtil.IsSuperAgency() && PageFlowUtil.IsPageflowChanged(capModel, ModuleName, pageflowGroup))
                    {
                        BreadCrumpToolBar.Enabled = false;

                        // The PageFlowUtil.IsPageFlowTraceUpdated is used in the function CapUtil.BuildRedirectUrl(), so the value should be set before the related function used.
                        PageFlowUtil.IsPageFlowTraceUpdated = true;

                        string url = CapUtil.BuildRedirectUrl(null, string.Empty, pageflowGroup, string.Empty);
                        string message = string.Format(GetTextByKey("aca_capconfirm_msg_pageflowchange_notice"), url);
                        MessageUtil.ShowMessage(Page, MessageType.Notice, message);
                    }

                    bool isHideFee = CapUtil.IsFeeEstimateCapType(capModel.capType);
                    BreadCrumpToolBar.PageFlow = BreadCrumpUtil.GetPageFlowConfig(this.ModuleName, false, isHideFee);
                    BreadCrumbParmsInfo breadcrumbParmsInfo = (BreadCrumbParmsInfo)HttpContext.Current.Session[SessionConstant.SESSION_BREADCRUMB];

                    if (breadcrumbParmsInfo != null)
                    {
                        if (breadcrumbParmsInfo.PageFrom == PageFrom.PayFeeDue)
                        {
                            lblPayFeeDue.Visible = true;
                            h1PayFeeDue.Visible = lblPayFeeDue.Visible;
                        }
                    }
                }
            }

            capFeeList.DisplayReceiptButton();
            ChangeCheckOutLabel();
        }

        /// <summary>
        /// CreateAnotherApplication button Click
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void CreateAnotherApplicationButton_Click(object sender, EventArgs e)
        {
            string tabName = string.Empty;

            if (AppSession.User == null || string.IsNullOrEmpty(AppSession.User.PublicUserId))
            {
                return;
            }

            if (!capFeeList.DoRecalculate())
            {
                return;
            }

            //validate all input required.
            if (!capFeeList.ValidateInputValue())
            {
                MessageUtil.ShowMessage(Page, MessageType.Error, GetTextByKey("aca_capfee_validate_input_message"));

                // After pop up error info, need to show fee items to UI again.
                capFeeList.ShowFeeInformation();

                return;
            }

            bool isAllCapTotalFeePositive = capFeeList.IsAllCapTotalFeePositive();

            if (!isAllCapTotalFeePositive)
            {
                capFeeList.ShowNegativeFeeMessage();

                return;
            }

            capFeeList.AddToShoppingCart();

            IBizDomainBll bizBll = ObjectFactory.GetObject(typeof(IBizDomainBll)) as IBizDomainBll;
            string url = bizBll.GetConfiguredUrlFromXPolicy();

            if (string.IsNullOrEmpty(url))
            {
                url = ACAConstant.URL_WELCOME_PAGE;
            }
            else
            {
                tabName = GetTabName(url);
            }

            CapModel4WS capModel = AppSession.GetCapModelFromSession(ModuleName);
            string connectString = "{0}isCreateAnotherApplication={1}&altID={2}";
            string splitChar = url.Contains(ACAConstant.QUESTION_MARK) ? ACAConstant.AMPERSAND : ACAConstant.QUESTION_MARK;
            string isCreateAnotherApplication = string.IsNullOrEmpty(capModel.altID) ? ACAConstant.COMMON_N : ACAConstant.COMMON_Y;
            connectString = string.Format(connectString, splitChar, isCreateAnotherApplication, capModel.altID);

            bool isFromNewUi = StandardChoiceUtil.IsEnableNewTemplate();

            if (isFromNewUi)
            {
                ClientScript.RegisterStartupScript(ClientScript.GetType(), "redirectToNewUIHomeShowMessage",
                    "<script>window.parent.redirectToNewUIHomeShowMessage('" +
                    ((isCreateAnotherApplication == ACAConstant.COMMON_Y) ? capModel.altID : "") +
                    "');</script>");
                return;
            }

            url = FileUtil.AppendApplicationRoot(url + tabName + connectString);
            Response.Redirect(url);
        }

        /// <summary>
        /// Continue to edit application.
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void ToConvertApplicationButton_Click(object sender, EventArgs e)
        {
            try
            {
                if (!capFeeList.DoRecalculate())
                {
                    return;
                }

                ICapBll capBll = (ICapBll)ObjectFactory.GetObject(typeof(ICapBll));

                CapModel4WS capModel = AppSession.GetCapModelFromSession(ModuleName);
                string altID = capBll.UpdateCapAltID(capModel, false, true);
                capModel.altID = altID; //set new alt ID to CapModel within session.
                capModel.capID.customID = altID; //set new alt ID to CapIDModel within session.

                if (capModel.capClass != ACAConstant.INCOMPLETE_TEMP_CAP)
                {
                    capModel.capClass = ACAConstant.INCOMPLETE_CAP;
                }

                if (CapUtil.IsSuperCAP(ModuleName))
                {
                    CapUtil.FilterSameLicenseType(capModel);
                    capModel = capBll.UpdatePartialCaps(capModel);
                }
                else
                {
                    capModel = capBll.UpdatePartialCapModelWrapper(ConfigManager.AgencyCode, capModel, AppSession.User.PublicUserId);
                }

                /* The LP record in capModel.licenseProfessionalModel has no Agency Code if the capModel returned by the function capBll.UpdatePartialCapModelWrapper(),
                 * So it will skip the duplicate LP validation (The Duplicate validation uses three parameters: LP Type, LP Number and Agency Code) and can add duplicated
                 * LP record to capModel.capModel.licenseProfessionalList if merge the LP record from licenseProfessionalModel to licenseProfessionalList.
                 */
                capModel.licenseProfessionalModel = null;
                
                // update the cap model to session with fee information
                AppSession.SetCapModelToSession(ModuleName, capModel);

                //bool showFeeForm = string.IsNullOrEmpty(Request.QueryString["showFeeForm"]) ? false : Request.QueryString["showFeeForm"] == ACAConstant.COMMON_Y;
                ////If fee forms appears the steps before dynamic pages should be 3 else 2
                //int currentStep = showFeeForm ? 3 : 2;

                //currentStep++;
                int currentStep = int.Parse(Request.QueryString["stepNumber"]);
                currentStep++;

                string url = string.Format("CapEdit.aspx?Module={0}&stepNumber={1}&pageNumber=1&isFeeEstimator=N&IsConvertToApp=Y", ModuleName, currentStep);

                Response.Redirect(url);
            }
            catch (ACAException ex)
            {
                string msg = ex.Message;
                MessageUtil.ShowMessage(Page, MessageType.Error, msg);
            }
        }

        /// <summary>
        /// AddToShoppingCart LinkButton Click
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void AddToShoppingCartButton_Click(object sender, EventArgs e)
        {
            if (AppSession.User == null || string.IsNullOrEmpty(AppSession.User.PublicUserId))
            {
                return;
            }

            if (!capFeeList.DoRecalculate())
            {
                return;
            }

            //validate all input required.
            if (!capFeeList.ValidateInputValue())
            {
                MessageUtil.ShowMessage(Page, MessageType.Error, GetTextByKey("aca_capfee_validate_input_message"));

                // After pop up error info, need to show fee items to UI again.
                capFeeList.ShowFeeInformation();

                return;
            }

            bool isAllCapTotalFeePositive = capFeeList.IsAllCapTotalFeePositive();

            if (!isAllCapTotalFeePositive)
            {
                capFeeList.ShowNegativeFeeMessage();
                return;
            }

            capFeeList.AddToShoppingCart();

            // If it is for schedule exam payment, then skip the shopping cart even it is openning.
            if (IsFromExamSchedule)
            {
                CapModel4WS capModel = AppSession.GetCapModelFromSession(ModuleName);
                AppSession.SetCapIDModelsToSession(CapUtil.GetChildCapIDs(capModel, ModuleName, false));
                Response.Redirect(string.Format("CapPayment.aspx?Module={0}&pageNumber=2&isPay4ExistingCap={1}&isRenewal={2}", ModuleName, ACAConstant.COMMON_Y, ACAConstant.COMMON_N));
            }
            else
            {
                string url = "../ShoppingCart/ShoppingCart.aspx?TabName=Home&stepNumber=2";
                Response.Redirect(url);
            }
        }

        /// <summary>
        /// Continue Application to payment page. 
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void ContinueApplicationButton_Click(object sender, EventArgs e)
        {
            capFeeList.ContinueToPayment();
        }

        /// <summary>
        /// Continue to edit application.
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void PayAtCounterButton_Click(object sender, EventArgs e)
        {
            capFeeList.PayAtCounter();
        }

        /// <summary>
        /// if it is from shopping cart. show "save to cart". else show "check out".
        /// </summary>
        private void ChangeCheckOutLabel()
        {
            if (IsFromShoppingCart)
            {
                lnkAddToShoppingCart.LabelKey = "per_fee_label_savetocart";
            }
        }

        /// <summary>
        /// Get Tab Name.
        /// </summary>
        /// <param name="url">string url path.</param>
        /// <returns>the tab name.</returns>
        private string GetTabName(string url)
        {
            string moduleName = string.Empty;
            int start = url.IndexOf("module=");

            if (start > 0)
            {
                moduleName = url.Substring(start + 7);
            }
            else
            {
                return string.Empty;
            }

            return "&tabName=" + moduleName;
        }

        /// <summary>
        /// Indicates the current cap whether is a resume cap or not. 
        /// </summary>
        /// <returns>returns true it current cap whether is a resume, returns false if not.</returns>
        private bool IsEstimate()
        {
            string isFeeEstimator = Request.QueryString["isFeeEstimator"];

            if (isFeeEstimator != null)
            {
                return ACAConstant.COMMON_Y.Equals(isFeeEstimator, StringComparison.InvariantCultureIgnoreCase);
            }

            return false;
        }

        /// <summary>
        /// Controls buttons to show or hidden according to the difference page coming from.
        /// </summary>
        private void PresentateButton()
        {
            if (IsEstimate())
            {
                lnkContinueApplication.Visible = false;

                CapModel4WS capModel = AppSession.GetCapModelFromSession(ModuleName);

                //can't convert to application if the cap type is only fee estimate.
                if (CapUtil.IsFeeEstimateCapType(capModel.capType) ||
                    (!AppSession.IsAdmin && !FunctionTable.IsEnableCreateApplication()))
                {
                    lnkConvertApplication.Visible = false;
                }
                else
                {
                    lnkConvertApplication.Visible = AuthenticationUtil.IsAuthenticated || AppSession.IsAdmin;
                }

                if (AppSession.IsAdmin)
                {
                    divAddToShoppingCart.Visible = true;
                    divPayAtCounter.Visible = true;
                    btnCreateAnotherApplication.Visible = true;
                }
            }
            else if (!AppSession.User.IsAnonymous)
            {
                var bizBll = (IBizDomainBll)ObjectFactory.GetObject(typeof(IBizDomainBll));

                //Pay At Counter button control by "CONTINUE_SHOPPING_ENABLED" standchoice of "ACA_CONFIGS"
                string stdContinueShoppingFlag = bizBll.GetValueForACAConfig(ConfigManager.AgencyCode, BizDomainConstant.STD_ITEM_CONTINUE_SHOPPING_ENABLE);
                bool isContinueShoppingEnabled = ValidationUtil.IsYes(stdContinueShoppingFlag);

                //obtain fee estimate or anonymous user should not display defer payment button.
                //DeferPaymentEnabled controls buttons
                divPayAtCounter.Visible = StandardChoiceUtil.IsEnableDeferPayment();

                //continue shopping enable switch controls buttons
                if (isContinueShoppingEnabled)
                {
                    btnCreateAnotherApplication.Visible = true;
                }

                //shopping cart switch controls buttons
                if (StandardChoiceUtil.IsEnableShoppingCart())
                {
                    divAddToShoppingCart.Visible = true;
                    divContinueAndConvertApplication.Visible = false;

                    if (IsFromShoppingCart)
                    {
                        btnCreateAnotherApplication.Visible = false;
                    }
                }

                // set the agency-defined
                if (!AppSession.IsAdmin && !FunctionTable.IsEnableMakePayment())
                {
                    lnkContinueApplication.Visible = false;
                }
            }
        }

        /// <summary>
        /// Run FeeEstimateAfter4ACA Event
        /// </summary>
        private void RunFeeEstimateAfter4ACAEvent()
        {
            try
            {
                CapModel4WS capModel4WS = AppSession.GetCapModelFromSession(ModuleName);

                if (_is4FeeEstimator)
                {
                    //Trigger Fee estimate EMSE script.
                    IEMSEBll emseBll = (IEMSEBll)ObjectFactory.GetObject(typeof(IEMSEBll));
                    emseBll.TriggerFeeEstimateAfter4ACAEvent(capModel4WS);
                }
            }
            catch (ACAException ex)
            {
                MessageUtil.ShowMessage(Page, MessageType.Error, ex.Message);
            }
        }

        /// <summary>
        /// Validate and handle CapModel session. 
        /// If CapModel isn't existing in session, query CapModel and add it to session for next process to use.
        /// </summary>
        private void ValidateAndHandleSession()
        {
            // if Cap Model has existed in session, return directly.
            if (AppSession.GetCapModelFromSession(ModuleName) != null)
            {
                return;
            }

            // If CapModel isn't existing in session, query CapModel and add it to session for next process to use.
            // this scene will appear when it comes from CapHome (or search list) page.
            ICapBll capBll = (ICapBll)ObjectFactory.GetObject(typeof(ICapBll));
            CapWithConditionModel4WS capWithConditionModel = capBll.GetCapViewBySingle(capFeeList.GetCapIDModel(), AppSession.User.UserSeqNum, ACAConstant.COMMON_N, StandardChoiceUtil.IsSuperAgency());

            // if CapModel is found by capIdModel, add the CapModel to session
            if (capWithConditionModel != null && capWithConditionModel.capModel != null)
            {
                CapModel4WS capModel = capWithConditionModel.capModel;
                AppSession.SetCapModelToSession(ModuleName, capModel);
            }
            else
            {
                // if can't find cap model, direct it to CapHome, maybe input wrong cap id by user in url directly
                Response.Redirect("CapHome.aspx?Module=" + ModuleName);
            }
        }

        #endregion Methods
    }
}
