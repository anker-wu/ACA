#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: plan_PlanPay.ascx.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2008-2014
 *
 *  Description:
 *
 *  Notes:
 * $Id: PlanPay.aspx.cs 277863 2014-08-22 05:23:34Z ACHIEVO\james.shi $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using System;
using System.Globalization;
using System.Web;
using System.Web.UI.WebControls;

using Accela.ACA.BLL.Common;
using Accela.ACA.BLL.Finance;
using Accela.ACA.Common;
using Accela.ACA.Common.Util;
using Accela.ACA.Web.Common;
using Accela.ACA.WSProxy;

namespace Accela.ACA.Web.Plan
{
    /// <summary>
    /// the class for PlanPay.
    /// </summary>
    public partial class PlanPay : BasePage
    {
        #region Fields

        /// <summary>
        /// the tran id.
        /// </summary>
        private long tranID;

        #endregion Fields

        #region Properties

        /// <summary>
        /// Gets or sets total fee amount.
        /// </summary>
        private string TotalFeeAmount
        {
            get
            {
                return ViewState["PLAN_FEE_TOTAL"] as string;
            }

            set
            {
                ViewState["PLAN_FEE_TOTAL"] = value;
            }
        }

        #endregion Properties

        #region Methods

        /// <summary>
        /// initial UI method.
        /// </summary>
        public void InitUI()
        {
            string tranID = Request.Params[ACAConstant.REQUEST_PARMETER_TRAN_ID];

            IFeeBll feeBll = ObjectFactory.GetObject<IFeeBll>();
            double feeAmount = 0;
            feeAmount = feeBll.GetTotalFeeByTransactionID(long.Parse(tranID));
            TotalFeeAmount = feeAmount.ToString(CultureInfo.InvariantCulture);

            string amount = I18nNumberUtil.FormatMoneyForUI(feeAmount);
            lblChargedAmount.Text = amount;
            hdnChargedAmount.Value = feeAmount.ToString(CultureInfo.InvariantCulture);
            hdnPlanSeqNbr.Value = tranID;

            DropDownListBindUtil.BindState(ddlState);
            DropDownListBindUtil.BindStandardChoise(ddlCardType, BizDomainConstant.STD_CAT_PAYMENT_CREDITCARD_TYPE);
            DropDownListBindUtil.BindMonth(ddlExpMonth);
            DropDownListBindUtil.BindYear(ddlExpYear);
        }

        /// <summary>
        /// Page Load Method.
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            Response.Buffer = true;
            Response.ExpiresAbsolute = DateTime.Now.AddDays(-1);
            Response.Expires = 0;
            Response.CacheControl = "no-cache";

            if (!AppSession.IsAdmin && string.IsNullOrEmpty(Request.Params[ACAConstant.REQUEST_PARMETER_TRAN_ID]))
            {
                MessageUtil.ShowMessage(Page, MessageType.Error, GetTextByKey("aca_plan_error_sequence_number"));
                return;
            }

            SetTransactionID();

            if (tranID == -1)
            {
                return;
            }

            if (!Page.IsPostBack)
            {
                InitUI();
            }
        }

        /// <summary>
        /// Submit Payment OnClick
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void SubmitPaymentButton_OnClick(object sender, EventArgs e)
        {
            if (IsValid)
            {
                IOnlinePaymenBll paymentBll = ObjectFactory.GetObject<IOnlinePaymenBll>();
                string agencyCode = ConfigManager.AgencyCode;
                string callerID = AppSession.User.PublicUserId;

                string chargedAmountString = TotalFeeAmount;
                double chargedAmount = I18nNumberUtil.ParseMoneyFromWebService(chargedAmountString);
                string expMonth = ddlExpMonth.Text;
                string expYear = ddlExpYear.Text;
                DateTime expirattionDate = new DateTime(int.Parse(expYear), int.Parse(expMonth), 1);

                //construct an credit card model
                CreditCardModel4WS creditCardModel = new CreditCardModel4WS();
                creditCardModel.servProvCode = agencyCode;
                creditCardModel.callerID = callerID;
                creditCardModel.cardType = ddlCardType.Text;
                creditCardModel.holderName = txtCardName.Text;
                creditCardModel.expirationDate = I18nDateTimeUtil.FormatToDateTimeStringForWebService(expirattionDate);
                creditCardModel.expirationMonth = ddlExpMonth.Text;
                creditCardModel.expirationYear = ddlExpYear.Text;
                creditCardModel.streetAddress = txtStreetAdd1.Text;
                creditCardModel.city = txtCity.Text;
                creditCardModel.state = ddlState.Text;
                creditCardModel.postalCode = txtZip.GetZip(string.Empty);
                creditCardModel.pos = true;
                creditCardModel.accountNumber = txtCardNumber.Text;
                creditCardModel.securityCode = txtCCV.Text;
                creditCardModel.balance = chargedAmountString;
                creditCardModel.posTransSeq = tranID.ToString();

                // construct a payment model
                PaymentModel paymentModel = new PaymentModel();
                double processingFee = 0;
                double accelaFee = processingFee;

                paymentModel.paymentMethod = "Credit Card";
                paymentModel.capID = null;
                paymentModel.auditID = callerID;
                paymentModel.paymentRefNbr = string.Empty;
                paymentModel.ccExpDate = expirattionDate;
                paymentModel.ccType = creditCardModel.cardType;

                paymentModel.payee = string.Empty;
                paymentModel.paymentDate = DateTime.Now;
                paymentModel.paymentAmount = chargedAmount;
                paymentModel.paymentChange = 0;
                paymentModel.amountNotAllocated = chargedAmount;
                paymentModel.paymentStatus = "Paid";

                paymentModel.paymentComment = "Automated Plan Check";
                paymentModel.processingFee = accelaFee;

                //double totalFee = feeService.getTotalFee(capID4ws, callID);
                //paymentModel.totalInvoiceAmount = totalFee;
                paymentModel.totalPaidAmount = 0;

                // do the online payment
                OnLinePaymentReturnInfo4WS returnInfo = paymentBll.CreditCardPayment4PlanReview(creditCardModel, paymentModel);

                Session.Remove("PLAN_PAYMENTMODEL");

                if (returnInfo.errCode.Equals("0", StringComparison.InvariantCulture))
                {
                    PaymentModel returnPaymentModel = returnInfo.payment;

                    if (returnPaymentModel != null)
                    {
                        string receiptNbr = returnPaymentModel.receiptNbr.ToString();
                        Session["PLAN_PAYMENTMODEL"] = returnPaymentModel;
                        HttpContext.Current.Response.Redirect(string.Format("~/plan/PlanPaySuccess.aspx?module={0}&receiptNbr={1}", ModuleName, receiptNbr), true);
                    }
                    else
                    {
                        MessageUtil.ShowMessage(Page, MessageType.Error, returnInfo.rtnMsg);
                    }
                }
                else
                {
                    MessageUtil.ShowMessage(Page, MessageType.Error, returnInfo.rtnMsg);
                }
            }
        }

        /// <summary>
        /// Set Transaction ID.
        /// </summary>
        private void SetTransactionID()
        {
            tranID = -1;

            string tmpTran = Request.QueryString[ACAConstant.REQUEST_PARMETER_TRAN_ID];

            if (!string.IsNullOrEmpty(tmpTran))
            {
                try
                {
                    tranID = long.Parse(tmpTran);
                }
                catch
                {
                }
            }
        }

        #endregion Methods
    }
}
