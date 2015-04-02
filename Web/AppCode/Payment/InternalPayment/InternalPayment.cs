#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: InternalPayment.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2009-2014
 *
 *  Description:
 *
 *  Notes:
 * $Id: InternalPayment.cs 277174 2014-08-12 07:38:31Z ACHIEVO\james.shi $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using System;
using System.Collections;
using System.Globalization;
using System.Web;
using System.Web.UI;

using Accela.ACA.BLL.Finance;
using Accela.ACA.Common;
using Accela.ACA.Common.Log;
using Accela.ACA.Common.Util;
using Accela.ACA.Web.Common;
using Accela.ACA.WSProxy;

using log4net;

namespace Accela.ACA.Web.Payment
{
    /// <summary>
    /// Process Internal Payment Logic.
    /// </summary>
    public class InternalPayment : IPayment
    {
        #region Fields

        /// <summary>
        /// For write log. 
        /// </summary>
        private static readonly ILog Logger = LogFactory.Instance.GetLogger(typeof(InternalPayment));

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the InternalPayment class. 
        /// </summary>
        public InternalPayment()
        {
            // TODO: Add constructor logic here
        }

        #endregion Constructors

        #region Methods

        /// <summary>
        /// Complete payment
        /// </summary>
        /// <param name="currentPage">current Page</param>
        /// <returns>OnlinePaymentResultModel model for payment.</returns>
        public OnlinePaymentResultModel CompletePayment(Page currentPage)
        {
            Logger.DebugFormat("begin to invoke CompletePayment method for internal payment.\n");

            string _errorMessage = string.Empty;
            CreditCardModel4WS _creditCardModel = null;
            CheckModel4WS _checkModel = null;
            TrustAccountModel _trustAccountModel = null;
            string _paymentMethodValue = string.Empty;
            string adapterName = PaymentHelper.GetAdapterName();
            string parameters = string.Format("{0}={1}", ACAConstant.ServProvCode_Key, ConfigManager.AgencyCode);

            IPaymentPage paymentPage = currentPage as IPaymentPage;
            PaymentMethod paymentMethod = paymentPage.GetPaymentMethod();

            switch (paymentMethod)
            {
                case PaymentMethod.CreditCard:
                    _creditCardModel = paymentPage.GetCreditCardModel();
                    _paymentMethodValue = ACAConstant.PAY_METHOD_CREDIT_CARD;
                    parameters += string.Format("&PaymentMethod={0}", ACAConstant.PAY_METHOD_CREDIT_CARD);
                    break;
                case PaymentMethod.TrustAccount:
                    _trustAccountModel = paymentPage.GetTrustAccountModel();
                    _paymentMethodValue = ACAConstant.PAYMENT_METHOD_TRUST_ACCOUNT;
                    adapterName = ACAConstant.PAYMENT_METHOD_TRUST_ACCOUNT;
                    parameters += string.Format("&PaymentMethod={0}", ACAConstant.PAYMENT_METHOD_TRUST_ACCOUNT);
                    break;
                case PaymentMethod.Check:
                    _checkModel = paymentPage.GetCheckModel();
                    _paymentMethodValue = ACAConstant.PAY_METHOD_CHECK;
                    parameters += string.Format("&PaymentMethod={0}", ACAConstant.PAY_METHOD_CHECK);
                    break;
            }

            OnlinePaymentResultModel result = null;
            string callerId = AppSession.User.PublicUserId;

            if (_trustAccountModel != null)
            {
                parameters += string.Format(CultureInfo.InvariantCulture, "&TRUST_ACCOUNT_NAME={0}&TOTAL_FEE={1}", HttpUtility.UrlEncode(_trustAccountModel.acctID), _trustAccountModel.acctBalance);
            }

            Logger.InfoFormat("parameters for completing payment:{0}", parameters);
            Logger.DebugFormat("end to prepare parameters.\n");

            CapIDModel4WS[] capIds = AppSession.GetCapIDModelsFromSession();

            Logger.DebugFormat("begin to invoke CompletePayment method.\n");
            IPaymentBll paymentBll = ObjectFactory.GetObject<IPaymentBll>();

            switch (paymentPage.PaymentEntityType)
            {
                case ACAConstant.PaymentEntityType.CAP:
                    result = paymentBll.CompletePayment(_creditCardModel, _checkModel, TempModelConvert.Trim4WSOfCapIDModels(capIds), parameters, adapterName, callerId);
                    break;

                case ACAConstant.PaymentEntityType.TrustAccount:
                    _trustAccountModel = paymentPage.GetTrustAccountModel();
                    string paymentEntityType = paymentPage.PaymentEntityType.ToString();
                    result = paymentBll.CompleteGenericPayment(
                        _creditCardModel, _checkModel, _trustAccountModel.acctID, paymentEntityType, parameters, adapterName, callerId);
                    break;
            }
            
            Logger.DebugFormat("end to invoke CompletePayment method.\n");

            if (result != null)
            {
                Logger.InfoFormat("return data from CompletePayment method:\n {0}", result.paramString);
            }
            else
            {
                Logger.Error("No result returned from CompletePayment WS.\n ");
            }

            AppSession.SetOnlinePaymentResultModelToSession(result);

            if (paymentPage.PaymentEntityType == ACAConstant.PaymentEntityType.TrustAccount)
            {
                RedirectAfterDeposit(result);
            }
            else
            {
                RedirectAfterPayment(result);
            }

            return result;
        }

        /// <summary>
        /// Get the 3rd page height to make ACA iFrame page meet the 3rd page height automatically.
        /// </summary>
        /// <returns>The 3rd page height.</returns>
        public int Get3rdPageHeight()
        {
            return 0;
        }

        /// <summary>
        /// Get payment config
        /// </summary>
        /// <param name="policy">the XPolicy</param>
        /// <returns>XPolicy hashtable.</returns>
        public Hashtable GetEPaymentConfig(XPolicyModel policy)
        {
            return null;
        }

        /// <summary>
        /// Handle payment result and return the redirect url
        /// </summary>
        /// <param name="currentPage">the current page</param>
        /// <returns>return null.</returns>
        public string HandlePaymentResult(Page currentPage)
        {
            return null;
        }

        /// <summary>
        /// get OnlinePaymentResultModel model.
        /// </summary>
        /// <param name="currentPage">the current page</param>
        /// <returns>the OnlinePaymentResultModel model.</returns>
        public OnlinePaymentResultModel InitiatePayment(Page currentPage)
        {
            return CompletePayment(currentPage);
        }

        /// <summary>
        /// Verify payment and get  OnlinePaymentResultModel model.
        /// </summary>
        /// <param name="currentPage">the current page</param>
        /// <returns>the OnlinePaymentResultModel model.</returns>
        public OnlinePaymentResultModel VerifyPayment(Page currentPage)
        {
            return null;
        }

        /// <summary>
        /// Redirect the page after payment
        /// </summary>
        /// <param name="result">the OnlinePaymentResultModel model.</param>
        private void RedirectAfterPayment(OnlinePaymentResultModel result)
        {
            //Check if all caps failed to pay.
            bool succeedToPay = false;
            foreach (CapPaymentResultModel paymentResult in result.capPaymentResultModels)
            {
                if (paymentResult.paymentStatus)
                {
                    succeedToPay = true;
                }
            }

            //If all caps are failed to pay we need to stay on the current page and show error to user
            if (!succeedToPay && result.exceptionMsg != null &&
                result.exceptionMsg.Length > 0)
            {
                return;
            }

            // get the step number for parameter.
            string preStepNumber = HttpContext.Current.Request.QueryString["stepNumber"];
            bool isNumber = ValidationUtil.IsNumber(preStepNumber);
            int stepNumber = 0;

            if (isNumber)
            {
                stepNumber = int.Parse(preStepNumber) + 1;
            }

            //Get module from request
            string moduleName = HttpContext.Current.Request.QueryString["Module"];
            if (string.IsNullOrEmpty(moduleName))
            {
                moduleName = string.Empty;
            }

            //If enable shopping cart go to capcompletions.aspx else, go to capcompletion.aspx
            string url = string.Empty;
            bool enableShoppingCart = StandardChoiceUtil.IsEnableShoppingCart();
            bool isSuperAgency = StandardChoiceUtil.IsSuperAgency();

            if (enableShoppingCart || isSuperAgency)
            {
                url = string.Format("../Cap/CapCompletions.aspx?Module={0}&stepNumber={1}", moduleName, stepNumber);
            }
            else
            {
                string renewalFlag = Convert.ToString(HttpContext.Current.Request.QueryString["isRenewal"]);
                string pay4ExistingCapFlag = Convert.ToString(HttpContext.Current.Request.QueryString["isPay4ExistingCap"]);

                url = string.Format("../Cap/CapCompletion.aspx?Module={0}&stepNumber={1}&isRenewal={2}&isPay4ExistingCap={3}", moduleName, stepNumber, renewalFlag, pay4ExistingCapFlag);
            }

            HttpContext.Current.Response.Redirect(url);
        }

        /// <summary>
        /// Redirect the page after deposit
        /// </summary>
        /// <param name="depositResult">the OnlinePaymentResultModel model.</param>
        private void RedirectAfterDeposit(OnlinePaymentResultModel depositResult)
        {
            //Check if all caps failed to pay.
            bool succeedToPay = false;
            if (depositResult != null && depositResult.entityPaymentResultModels != null)
            {
                foreach (EntityPaymentResultModel paymentResult in depositResult.entityPaymentResultModels)
                {
                    if (ACAConstant.COMMON_ZERO.CompareTo(paymentResult.errorCode) == 0)
                    {
                        succeedToPay = true;
                    }
                }
            }

            //If all trust account are failed to pay we need to stay on the current page and show error to user
            if (!succeedToPay)
            {
                return;
            }

            string url = string.Empty;
            url = string.Format("../Account/DepositCompletion.aspx");

            HttpContext.Current.Response.Redirect(url);
        }
        #endregion Methods
    }
}
