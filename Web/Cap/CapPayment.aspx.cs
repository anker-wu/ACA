#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: CapDetail.aspx.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2007-2014
 *
 *  Description:
 *
 *  Notes:
 *      $Id: CapPayment.aspx.cs 277932 2014-08-22 10:29:52Z ACHIEVO\james.shi $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using System;
using System.Web;
using System.Web.Services;

using Accela.ACA.Common;
using Accela.ACA.Common.Util;
using Accela.ACA.Web.Common;
using Accela.ACA.Web.Payment;
using Accela.ACA.WSProxy;
using Accela.Web.Controls;

namespace Accela.ACA.Web.Cap
{
    /// <summary>
    /// This class provide the ability to operation CAP payment. 
    /// </summary>
    public partial class CapPayment : BasePage, IPaymentPage
    {
        /// <summary>
        /// Gets the payment entity type
        /// </summary>
        public ACAConstant.PaymentEntityType PaymentEntityType
        {
            get
            {
                return ACAConstant.PaymentEntityType.CAP;
            }
        }

        #region Methods

        /// <summary>
        /// Get Default Public User Model
        /// </summary>
        /// <param name="seqNumber">The sequence number.</param>
        /// <returns>string for public user info.</returns>
        [WebMethod(Description = "GetContactModel", EnableSession = true)]
        public static string GetContactModel(string seqNumber)
        {
            return AccountUtil.GetContact(seqNumber);
        }

        /// <summary>
        /// Get Trust Account Information by account id
        /// </summary>
        /// <param name="accountId">account id</param>
        /// <returns>Trust Account Information</returns>
        [WebMethod(Description = "GetTrustAccountInfo", EnableSession = true)]
        public static string GetTrustAccountInfo(string accountId)
        {
            double totalBalance = 0;
            string description = string.Empty;
            string errorMessage = string.Empty;

            try
            {
                TrustAccountModel trustAccountModel = TrustAccountUtil.GetTrustAccountModel(accountId);

                if (trustAccountModel != null)
                {
                    description = I18nStringUtil.GetString(trustAccountModel.resDescription, trustAccountModel.description);
                    totalBalance = TrustAccountUtil.GetTotalAmount(trustAccountModel);
                }
            }
            catch (Exception e)
            {
                errorMessage = e.Message;
            }

            return string.Format(
                @"{{""Balance"":""{0}"",""RealBalance"":""{1}"",""Name"":""{2}"",""Error"":""{3}""}}",
                I18nNumberUtil.FormatMoneyForUI(totalBalance),
                I18nNumberUtil.ConvertNumberToInvariantString(totalBalance), 
                MessageUtil.FilterQuotation(description),
                MessageUtil.FilterQuotation(errorMessage));
        }

        /// <summary>
        /// Construct CheckModel.
        /// </summary>
        /// <returns>a CheckModel4WS</returns>
        public CheckModel4WS GetCheckModel()
        {
            return this.Payment.GetCheckModel();
        }

        /// <summary>
        /// Construct CreditCardModel.
        /// </summary>
        /// <returns>a CreditCardModel4WS</returns>
        public CreditCardModel4WS GetCreditCardModel()
        {
            return this.Payment.GetCreditCardModel();
        }

        /// <summary>
        /// Construct a TrustAccountModel4WS from user's input
        /// </summary>
        /// <returns>a TrustAccountModel4WS</returns>
        public TrustAccountModel GetTrustAccountModel()
        {
            return this.Payment.GetTrustAccountModel();
        }

        /// <summary>
        /// Get payment method.
        /// </summary>
        /// <returns>a PaymentMethod</returns>
        public PaymentMethod GetPaymentMethod()
        {
            return this.Payment.GetPaymentMethod();
        }

        /// <summary>
        /// Get Total Fee
        /// </summary>
        /// <returns>double for total fee.</returns>
        public double GetTotalFee()
        {
            return Payment.GetTotalFee();
        }

        /// <summary>
        /// Get the IFrame page height
        /// </summary>
        /// <returns>Page Height</returns>
        protected int GetIFramePageHeight()
        {
            IPayment payment = PaymentProcessorFactory.CreateProcessor(); // PaymentHelper.GetPaymentInstance();
            if (payment != null)
            {
                return payment.Get3rdPageHeight();
            }
            else
            {
                // -1 means that this page will not adjust the iframe page heigh for inline 3rd page.
                return -1;
            }
        }

        /// <summary>
        /// Raises the page load event
        /// </summary>
        /// <param name="sender">An object that contains the event sender.</param>
        /// <param name="e">A System.EventArgs object containing the event data.</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            //the code is to forbiden user to come back from issuance page
            Response.Buffer = true;
            Response.ExpiresAbsolute = DateTime.Now.AddDays(-1);
            Response.Expires = 0;
            Response.CacheControl = "no-cache";
            this.Payment.Pay4ExistingCap = ACAConstant.COMMON_Y.Equals(Request.QueryString["isPay4ExistingCap"], StringComparison.InvariantCulture);

            if (!IsPostBack)
            {
                InitBreadCrumb();
            }
        }

        /// <summary>
        /// initial bread crumb.
        /// </summary>
        private void InitBreadCrumb()
        {
            if (!AppSession.IsAdmin)
            {
                if (!AppSession.User.IsAnonymous && StandardChoiceUtil.IsEnableShoppingCart())
                {
                    ucBreadCrumpToolBar.Visible = false;
                    BreadCrumpShoppingCart.PageFlow = BreadCrumpUtil.GetShoppingCartFlowConfig();
                }
                else
                {
                    BreadCrumpShoppingCart.Visible = false;
                    ucBreadCrumpToolBar.PageFlow = BreadCrumpUtil.GetPageFlowConfig(this.ModuleName, false);

                    CapModel4WS capModel = AppSession.GetCapModelFromSession(ModuleName);
                    PageFlowGroupModel pageflowGroup = PageFlowUtil.GetPageflowGroup(capModel);

                    if (!StandardChoiceUtil.IsSuperAgency() && PageFlowUtil.IsPageflowChanged(capModel, ModuleName, pageflowGroup))
                    {
                        ucBreadCrumpToolBar.Enabled = false;

                        // The PageFlowUtil.IsPageFlowTraceUpdated is used in the function CapUtil.BuildRedirectUrl(), so the value should be set before the related function used.
                        PageFlowUtil.IsPageFlowTraceUpdated = true;

                        string url = CapUtil.BuildRedirectUrl(null, string.Empty, pageflowGroup, string.Empty);
                        string message = string.Format(GetTextByKey("aca_capconfirm_msg_pageflowchange_notice"), url);
                        MessageUtil.ShowMessage(Page, MessageType.Notice, message);
                    }
                }
            }

            BreadCrumbParmsInfo breadcrumbParmsInfo = (BreadCrumbParmsInfo)HttpContext.Current.Session[SessionConstant.SESSION_BREADCRUMB];
            if (breadcrumbParmsInfo != null)
            {
                if (breadcrumbParmsInfo.PageFrom == PageFrom.PayFeeDue)
                {
                    per_permitPayFee_label_payFee.Visible = true;
                    h1PayFee.Visible = per_permitPayFee_label_payFee.Visible;
                }
            }
        }
        #endregion Methods
    }
}