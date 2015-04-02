#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: CapCompletion.aspx.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2007-2014
 *
 *  Description:
 *
 *  Notes:
 *      $Id: CapCompletion.aspx.cs 277932 2014-08-22 10:29:52Z ACHIEVO\james.shi $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using System;
using System.Collections;
using System.Text;
using System.Web;
using System.Web.Services;
using Accela.ACA.BLL.Cap;
using Accela.ACA.BLL.LicenseCertification;
using Accela.ACA.Common;
using Accela.ACA.Common.Util;
using Accela.ACA.Web.Common;
using Accela.ACA.Web.Common.CAP;
using Accela.ACA.Web.Payment;
using Accela.ACA.WSProxy;
using Accela.Web.Controls;

namespace Accela.ACA.Web.Cap
{
    /// <summary>
    /// This class provide the ability to operation cap completion.
    /// </summary>
    public partial class CapCompletion : CapCompletionBasePage
    {
        #region Fields

        /// <summary>
        /// Is auto issue.
        /// </summary>
        private string _isAutoIssue = string.Empty;

        #endregion Fields

        #region Properties

        /// <summary>
        /// Gets a value indicating whether this instance has ready2 schedule examination.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance has ready2 schedule examination; otherwise, <c>false</c>.
        /// </value>
        protected bool HasReady2ScheduleExamination
        {
            get
            {
                bool result = false;
                bool paidSuccess;
                CapIDModel capIdModel = GetCapIdModel(this.ModuleName, out paidSuccess);

                if (paidSuccess && capIdModel != null)
                {
                    var examinationBll = ObjectFactory.GetObject<IExaminationBll>();
                    result = examinationBll.HasReady2ScheduleExamination(new[] { capIdModel });
                }

                return result;
            }
        }

        /// <summary>
        /// Gets a value indicating whether auto issue license flag.
        /// </summary>
        private bool IsAutoIssueLicense
        {
            get
            {
                if (_isAutoIssue == string.Empty)
                {
                    CapModel4WS capModel = AppSession.GetCapModelFromSession(ModuleName);
                    ICapBll capBll = (ICapBll)ObjectFactory.GetObject(typeof(ICapBll));

                    _isAutoIssue = capBll.IsAutoIssue4RenewalByChildCapID(capModel.capID).ToString();
                }

                return Convert.ToBoolean(_isAutoIssue);
            }
        }

        /// <summary>
        /// Gets a value indicating whether is renewal flag.
        /// </summary>
        private bool IsRenewal
        {
            get
            {
                return ACAConstant.COMMON_Y.Equals(Request["isRenewal"], StringComparison.InvariantCultureIgnoreCase);
            }
        }

        /// <summary>
        /// Gets a value indicating whether is Pay Fee Due process.
        /// </summary>
        private bool IsPayFeeDue
        {
            get
            {
                return ACAConstant.COMMON_Y.Equals(Request.QueryString["isPay4ExistingCap"], StringComparison.InvariantCultureIgnoreCase);
            }
        }

        /// <summary>
        /// Gets a value indicating whether is Deferred Payment process.
        /// </summary>
        private bool IsDeferredPayment
        {
            get
            {
                return ACAConstant.COMMON_Y.Equals(Request.QueryString["isDeferredPayment"], StringComparison.InvariantCultureIgnoreCase);
            }
        }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Schedules the examinations.
        /// </summary>
        /// <param name="moduleName">The module name.</param>
        /// <returns>Schedule result</returns>
        [WebMethod(EnableSession = true, Description = "Schedule Examinations")]
        public static string ScheduleExaminations(string moduleName)
        {
            string result = string.Empty;
            bool paidSuccess;
            CapIDModel capIdModel = GetCapIdModel(moduleName, out paidSuccess);

            if (paidSuccess && capIdModel != null)
            {
                IExaminationBll examinationBll = ObjectFactory.GetObject<IExaminationBll>();
                result = examinationBll.ScheduleAllPaidExam(new[] { capIdModel }, AppSession.User.PublicUserId);
            }

            return result;
        }

        /// <summary>
        /// Page load method.
        /// </summary>
        /// <param name="sender">Object sender.</param>
        /// <param name="e">EventArgs e.</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            Response.Buffer = true;
            Response.ExpiresAbsolute = DateTime.Now.AddDays(-1);
            Response.Expires = 0;
            Response.CacheControl = "no-cache";

            bool isFailed = false;

            if (!IsPostBack)
            {
                if (AppSession.IsAdmin)
                {
                    divDependentCapTypeSetting.Visible = true;
                }

                try
                {
                    lblReceipt.Visible = AppSession.IsAdmin;
                    lblReceipt.LabelKey = "per_permitissuance_label_receipt";
                    h1Receipt.Visible = lblReceipt.Visible;

                    ShoppingCartUtil.SetCartItemNumber();

                    if (PaymentHelper.GetPaymentAdapterType() == PaymentAdapterType.Etisalat && AppSession.IsAdmin && !IsRenewal && !IsPayFeeDue)
                    {
                        registrationSuccessInfo.Show(MessageType.Success, "per_permitpaymentregistration_text_successinfo", MessageSeperationType.Bottom);
                    }

                    BreadCrumbParmsInfo breadcrumbParmsInfo = (BreadCrumbParmsInfo)HttpContext.Current.Session[SessionConstant.SESSION_BREADCRUMB];

                    if (breadcrumbParmsInfo != null && breadcrumbParmsInfo.PageFrom == PageFrom.PayFeeDue)
                    {
                        per_permitIssuance_label_permitIssuance.Visible = true;
                    }

                    CapModel4WS capModel = new CapModel4WS();

                    ReportBtn.IsForceLoginToApplyPermit = IsForceLoginToApplyPermit(ModuleName);

                    if (EtisalatAdapter.IsEtisalatOnlinePaymentUsed())
                    {
                        capModel = AppSession.GetCapModelFromSession(ModuleName);

                        //restore CapModel
                        if (capModel == null || EtisalatAdapter.IsPaid())
                        {
                            Hashtable htReturnValues = EtisalatAdapter.GetEMSEReturnValues();
                            CapIDModel4WS capIdModel = new CapIDModel4WS();

                            capIdModel.serviceProviderCode = ConfigManager.AgencyCode;
                            capIdModel.id1 = htReturnValues["capID1"].ToString();
                            capIdModel.id2 = htReturnValues["capID2"].ToString();
                            capIdModel.id3 = htReturnValues["capID3"].ToString();

                            try
                            {
                                ICapBll capBll = (ICapBll)ObjectFactory.GetObject(typeof(ICapBll));
                                capModel = capBll.GetCapViewDetailByPK(capIdModel, AppSession.User.UserSeqNum);
                            }
                            catch
                            {
                                //when partial CAP rec_status was set to "I" which means INVISIBLE/DELETED status.
                                //capBll.GetCapViewDetailByPK can't search this type of CAP, and throws error.
                            }

                            if (capModel != null)
                            {
                                AppSession.SetCapModelToSession(ModuleName, capModel);
                            }

                            ReportBtn.HasFee = true;
                            ReportBtn.ReceiptNbr = Request.QueryString["receiptNbr"];
                        }

                        if (Request.QueryString["failed"] != null)
                        {
                            isFailed = true;
                        }
                    }
                    else
                    {
                        OnlinePaymentResultModel onlinePaymentResultModel = AppSession.GetOnlinePaymentResultModelFromSession();
                        CapIDModel4WS capIdModel = new CapIDModel4WS();
                        if (onlinePaymentResultModel != null && onlinePaymentResultModel.capPaymentResultModels != null && onlinePaymentResultModel.capPaymentResultModels.Length > 0
                            && onlinePaymentResultModel.capPaymentResultModels[0].capID != null)
                        {
                            CapPaymentResultModel capPaymentResult = onlinePaymentResultModel.capPaymentResultModels[0];
                            if (!capPaymentResult.paymentStatus)
                            {
                                isFailed = true;
                            }

                            ReportBtn.HasFee = capPaymentResult.hasFee;
                            ReportBtn.ReceiptNbr = capPaymentResult.receiptNbr;

                            capIdModel = TempModelConvert.Add4WSForCapIDModel(capPaymentResult.capID);
                            ICapBll capBll = (ICapBll)ObjectFactory.GetObject(typeof(ICapBll));
                            capModel = capBll.GetCapViewDetailByPK(capIdModel, AppSession.User.UserSeqNum);

                            AppSession.SetCapModelToSession(ModuleName, capModel);
                        }
                    }

                    if (!AppSession.IsAdmin)
                    {
                        BreadCrumpToolBar.PageFlow = BreadCrumpUtil.GetPageFlowConfig(this.ModuleName, false);
                    }

                    capModel = AppSession.GetCapModelFromSession(ModuleName);

                    CapIDModel4WS capID = null;

                    if (capModel != null)
                    {
                        capID = capModel.capID;

                        if (!AppSession.IsAdmin)
                        {
                            lblCapID.Text = ConvertToString(capID.customID);
                            dependentCapTypeList.DisplayDependentCapTypeLink(capID, ModuleName);

                            // show conditions
                            CapWithConditionModel4WS capWithConditionModel4WS = CapUtil.GetCapWithConditionModel4WS(capID, AppSession.User.UserSeqNum, true, null);
                            capConditions.Display(capWithConditionModel4WS);
                        }
                    }
                    else
                    {
                        divContent.Visible = false;
                        ReportBtn.Failed = true;
                    }

                    if (isFailed)
                    {
                        ShowErrorMessage();
                    }
                    else
                    {
                        DisplayUIMsg();
                        CapUtil.ShowResultMessage(authMessage, this.ModuleName);
                        if (capID != null)
                        {
                            ReportBtn.CapID = capID;
                        }
                    }
                }
                catch (ACAException ex)
                {
                    string msg = ex.Message;
                    MessageUtil.ShowMessage(Page, MessageType.Error, msg);
                    return;
                }
            }
        }

        /// <summary>
        /// view permit detail link
        /// </summary>
        /// <param name="sender">Object sender.</param>
        /// <param name="e">EventArgs e.</param>
        protected void ViewPermitDetailButton_Click(object sender, EventArgs e)
        {
            CapModel4WS capModel = AppSession.GetCapModelFromSession(ModuleName);
            CapIDModel4WS capIdModel = capModel != null ? capModel.capID : null;

            string url = "CapDetail.aspx?Module=" + ModuleName;

            if (capIdModel != null)
            {
                url += string.Format(
                    "&capID1={0}&capID2={1}&capID3={2}&{3}={4}&{5}={6}",
                    capIdModel.id1,
                    capIdModel.id2,
                    capIdModel.id3,
                    UrlConstant.AgencyCode,
                    Server.UrlEncode(capIdModel.serviceProviderCode),
                    ACAConstant.IS_TO_SHOW_INSPECTION,
                    Request.QueryString[ACAConstant.IS_TO_SHOW_INSPECTION]);
            }

            Response.Redirect(url);
        }

        /// <summary>
        /// Gets the cap id model.
        /// </summary>
        /// <param name="moduleName">Name of the module.</param>
        /// <param name="paidSuccess">if set to <c>true</c> [paid success].</param>
        /// <returns>The cap id model.</returns>
        private static CapIDModel GetCapIdModel(string moduleName, out bool paidSuccess)
        {
            CapIDModel capIdModel = null;
            paidSuccess = false;

            if (EtisalatAdapter.IsEtisalatOnlinePaymentUsed())
            {
                CapModel4WS capModel = AppSession.GetCapModelFromSession(moduleName);

                if (capModel != null)
                {
                    paidSuccess = true;
                    capIdModel = TempModelConvert.Trim4WSOfCapIDModel(capModel.capID);
                }
            }
            else
            {
                OnlinePaymentResultModel onlinePaymentResultModel = AppSession.GetOnlinePaymentResultModelFromSession();

                // Defer payment function will made the payment result is null.
                CapPaymentResultModel capPaymentResult = onlinePaymentResultModel == null
                                                             ? null
                                                             : onlinePaymentResultModel.capPaymentResultModels[0];

                if (capPaymentResult != null && capPaymentResult.paymentStatus)
                {
                    paidSuccess = true;
                    capIdModel = capPaymentResult.capID;
                }
            }

            return capIdModel;
        }

        /// <summary>
        /// Convert to string.
        /// </summary>
        /// <param name="o">Object data.</param>
        /// <returns> data of string</returns>
        private static string ConvertToString(object o)
        {
            return o == null ? string.Empty : o.ToString();
        }

        /// <summary>
        /// 1. Apply permit process UI
        /// 2. Pay fee for existing Cap process UI
        /// 3. Renewal license and permit process UI
        /// </summary>
        private void DisplayUIMsg()
        {
            ReportBtn.Failed = false;
            bool isRegistered4Etisalat = EtisalatAdapter.IsRegistered();

            if (IsRenewal && !isRegistered4Etisalat)
            {
                DisplayUIMsg4Renewal();
            }
            else if (IsPayFeeDue)
            {
                permitSuccessInfo.Show(MessageType.Success, "per_permitIssuance_text_payFeeSuccessInfo", MessageSeperationType.Bottom);
                lblReceipt.LabelKey = "per_permitissuance_label_payfeeduereceipt";
                ShowPayFeeDueMsg();
            }
            else
            {
                permitSuccessInfo.Show(MessageType.Success, "per_permitIssuance_text_applicationSuccessInfo", MessageSeperationType.Bottom);
            }

            if (EtisalatAdapter.IsEtisalatOnlinePaymentUsed())
            {
                DisplayUIMsg4Etisalat();
            }
        }

        /// <summary>
        /// Show pay fee due message.
        /// </summary>
        private void ShowPayFeeDueMsg()
        {
            lblPermitNum4PayFeeDue.Visible = true;
            lblLicense4PayFeeDue.Visible = true;
            lblWelcomeInfo4PayFeeDue.Visible = true;
            lblPermitNumText4PayFeeDue.Visible = true;

            per_permitIssuance_text_welcomeInfo.Visible = false;
            per_permitIssuance_label_permitNum.Visible = false;
            per_permitIssuance_text_permitNum.Visible = false;
            per_permitIssuance_text_license.Visible = false;
        }

        /// <summary>
        /// Display UI message for Renewal process.
        /// </summary>
        private void DisplayUIMsg4Renewal()
        {
            divRowLine.Visible = false;
            divRenewalLicenseTopMessage.Visible = true;
            divRenewalLicenseInstructions.Visible = true;
            per_renewallicenseissuance_label_licensenumber.Visible = true;

            permitSuccessInfo.Hide();

            divNormalPermitInstruction.Visible = false;
            per_permitIssuance_label_permitNum.Visible = false;
            per_permitIssuance_text_license.Visible = false;
            lnkViewPermitDetail.Visible = false;
            per_permitIssuance_sublabel_viewPermit.Visible = false;

            if (AppSession.IsAdmin)
            {
                renewalLicenseAutoIssueSuccessMessage.Show(MessageType.Success, "per_renewalLicensePermit_text_renewalSuccessInfo", MessageSeperationType.Bottom);
                renewalLicenseNoAutoIssueSuccessMessage.Show(MessageType.Success, "per_renewallicenseissuance_label_submittednoautoissuancetopmsg", MessageSeperationType.Bottom);
                renewalLicenseDeferredPaymentSuccessMessage.Show(MessageType.Success, "per_renewallicenseissuance_label_deferredpaymenttopmsg", MessageSeperationType.Bottom);
                permitSuccessInfo.Hide();

                divRenewalAutoIssuanceInstruction.Visible = true;
                divRenewalNoAutoIssuanceInstruction.Visible = true;
                divRenewalDeferredPaymentInstruction.Visible = true;
                lblReceipt.LabelKey = "per_permitissuance_label_renewalreceipt";
            }
            else if (IsDeferredPayment)
            {
                //Renewal: Deferred Payment Process
                renewalLicenseDeferredPaymentSuccessMessage.Show(MessageType.Success, "per_renewallicenseissuance_label_deferredpaymenttopmsg", MessageSeperationType.Bottom);
                divRenewalDeferredPaymentInstruction.Visible = true;
            }
            else if (IsAutoIssueLicense)
            {
                //Renewal: Approved. Auto Issue
                renewalLicenseAutoIssueSuccessMessage.Show(MessageType.Success, "per_renewalLicensePermit_text_renewalSuccessInfo", MessageSeperationType.Bottom);
                divRenewalAutoIssuanceInstruction.Visible = true;
            }
            else if (!IsAutoIssueLicense)
            {
                //Renewal: Pending. Not Auto Issue
                renewalLicenseNoAutoIssueSuccessMessage.Show(MessageType.Success, "per_renewallicenseissuance_label_submittednoautoissuancetopmsg", MessageSeperationType.Bottom);
                divRenewalNoAutoIssuanceInstruction.Visible = true;
            }
        }

        /// <summary>
        /// display UI message for ETISALAT
        /// </summary>
        private void DisplayUIMsg4Etisalat()
        {
            bool isFailed4Etisalat = EtisalatAdapter.IsFailed();
            bool isPaid4Etisalat = EtisalatAdapter.IsPaid();
            bool isRegistered4Etisalat = EtisalatAdapter.IsRegistered();
            if (isRegistered4Etisalat)
            {
                ReportBtn.Failed = true;
            }

            spanShowToolBar.Visible = isRegistered4Etisalat;
            divEtisalatCAPType.Visible = isPaid4Etisalat;

            if (isPaid4Etisalat)
            {
                divEtisalatCAPType.InnerText = this.GetEtisalatCAPName();
            }

            spanEtisalatSuccessMessage.Visible = !isFailed4Etisalat;

            if (isFailed4Etisalat)
            {
                etisalatErrorMessage.ShowWithText(MessageType.Error, EtisalatAdapter.GetEmseReturnMessage(), MessageSeperationType.Bottom);

                divContent.Visible = false;
                ReportBtn.Failed = true;
            }
            else if (isPaid4Etisalat || isRegistered4Etisalat)
            {
                if (isPaid4Etisalat)
                {
                    lblEtisalatReturnMessage.Text = EtisalatAdapter.GetEmseReturnMessage();
                }
                else if (isRegistered4Etisalat)
                {
                    permitSuccessInfo.Show(MessageType.Success, "per_permitpaymentregistration_text_successinfo", MessageSeperationType.Bottom);

                    lblEtisalatReturnMessage.Text = EtisalatAdapter.GetEmseReturnMessage();
                }

                per_permitIssuance_text_permitNum.Visible = isPaid4Etisalat && !IsPayFeeDue; // display lblPermitNum4PayFeeDue for pay fee due.
                ReportBtn.Visible = isPaid4Etisalat;
                BreadCrumpToolBar.Visible = !EtisalatAdapter.IsPageFromCapList();

                if (!IsRenewal)
                {
                    divRowLine.Visible = isPaid4Etisalat;
                    per_permitIssuance_text_license.Visible = isPaid4Etisalat && !IsPayFeeDue; //pay fee due: lblLicense4PayFeeDue 
                    lnkViewPermitDetail.Visible = isPaid4Etisalat;
                    per_permitIssuance_sublabel_viewPermit.Visible = isPaid4Etisalat;
                }
                else
                {
                    divRowLine.Visible = false;
                    per_permitIssuance_text_license.Visible = false;
                    lnkViewPermitDetail.Visible = false;
                    per_permitIssuance_sublabel_viewPermit.Visible = false;
                    divRowLine.Visible = false;
                }
            }
        }

        /// <summary>
        /// get ETISALAT CAP name
        /// </summary>
        /// <returns>the cap name.</returns>
        private string GetEtisalatCAPName()
        {
            string capName = string.Empty;

            if (!string.IsNullOrEmpty(ModuleName))
            {
                CapModel4WS capModel = AppSession.GetCapModelFromSession(ModuleName);
                capName = CAPHelper.GetAliasOrCapTypeLabel(capModel);
            }

            return capName;
        }

        /// <summary>
        /// Show error message.
        /// </summary>
        private void ShowErrorMessage()
        {
            OnlinePaymentResultModel onlinePaymentResult = AppSession.GetOnlinePaymentResultModelFromSession();

            string errorMessage = BuildPaymentErrorMessage(onlinePaymentResult);

            if (string.IsNullOrEmpty(errorMessage))
            {
                errorMessage = GetTextByKey("per_permitIssuance_label_createCapFailed");
            }
            else
            {
                errorMessage = GetTextByKey("per_permitIssuance_label_createCapFailed")
                            + ACAConstant.HTML_BR
                            + ACAConstant.HTML_BR
                            + errorMessage;
            }

            MessageUtil.ShowMessage(Page, MessageType.Error, errorMessage);

            divContent.Visible = false;
            ReportBtn.Failed = true;
        }

        /// <summary>
        /// Build Payment Error Message
        /// </summary>
        /// <param name="result">a OnlinePaymentResultModel4WS</param>
        /// <returns>Error Message Information.</returns>
        private string BuildPaymentErrorMessage(OnlinePaymentResultModel result)
        {
            if (result == null)
            {
                return string.Empty;
            }

            StringBuilder errorMessage = new StringBuilder();

            if (result.exceptionMsg != null && result.exceptionMsg.Length > 0)
            {
                foreach (string message in result.exceptionMsg)
                {
                    string tempMsg = GetTextByKey(message);

                    if (string.IsNullOrEmpty(tempMsg))
                    {
                        tempMsg = message;
                    }

                    errorMessage.Append(tempMsg);
                    errorMessage.Append("\n");
                }
            }

            return errorMessage.ToString();
        }

        #endregion Methods
    }
}
