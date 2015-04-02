#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: GovolutionHandler.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2009-2014
 *
 *  Description:
 *
 *  Notes:
 * $Id: GovolutionHandler.cs 277174 2014-08-12 07:38:31Z ACHIEVO\james.shi $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using System;
using System.Web;
using System.Web.UI;

using Accela.ACA.Common;
using Accela.ACA.Common.Log;
using Accela.ACA.Web.Common;
using Accela.ACA.WSProxy;

using log4net;

namespace Accela.ACA.Web.Payment
{
    /// <summary>
    /// <c>Govolution</c> payment logic.
    /// </summary>
    public class GovolutionHandler : IHandler
    {
        #region Fields

        /// <summary>
        /// Redirection remittance id.
        /// </summary>
        private const string INITIATION_REMITTANCE_ID = "remittance_id";

        /// <summary>
        /// Redirection flag key.
        /// </summary>
        private const string REDIRECTION_FLAGKEY = "p";

        /// <summary>
        /// Redirection flag exit.
        /// </summary>
        private const string REDIRECTION_FLAG_EXIT = "4";

        /// <summary>
        /// Redirection flag notification.
        /// </summary>
        private const string REDIRECTION_FLAG_NOTIFICATION = "2";

        /// <summary>
        /// Redirection flag success.
        /// </summary>
        private const string REDIRECTION_FLAG_SUCCEEDED = "1";

        /// <summary>
        /// Redirection flag verification.
        /// </summary>
        private const string REDIRECTION_FLAG_VERIFICATION = "1";

        /// <summary>
        /// Create an instance of ILog
        /// </summary>
        private static readonly ILog Logger = LogFactory.Instance.GetLogger(typeof(GovolutionHandler));

        #endregion Fields

        #region Methods

        /// <summary>
        /// Handle payment result and return the redirect url
        /// </summary>
        /// <param name="currentPage">The current page.</param>
        /// <returns>The redirect url after handle the payment result.</returns>
        public string HandlePaymentResult(Page currentPage)
        {
            string transactionID = HttpContext.Current.Request[INITIATION_REMITTANCE_ID];
            string keyOfsavedData = this.GetKeyOfSavedData(transactionID);
            PaymentEntity entity = PaymentHelper.GetDataFromCache<PaymentEntity>(keyOfsavedData);
            AppSession.SetOnlinePaymentResultModelToSession(entity.PaymentResult);

            if (entity.PaymentResult != null)
            {
                Logger.DebugFormat("payment result is not null \n");
                foreach (CapPaymentResultModel result in entity.PaymentResult.capPaymentResultModels)
                {
                    Logger.DebugFormat("cap id = \n" + result.capID.customID);
                }
            }
            else
            {
                Logger.Error("payment result is null \n");
            }

            Logger.DebugFormat("begin HandlePaymentResult \n");

            Logger.InfoFormat("the response data from 3rd party web site for handling payment result: {0} \n", HttpContext.Current.Request.QueryString);
            string p = HttpContext.Current.Request.QueryString[REDIRECTION_FLAGKEY];
            string redirectedURL = string.Empty;

            if (REDIRECTION_FLAG_SUCCEEDED.Equals(p))
            {
                Logger.DebugFormat("the payment is successfully \n");
                redirectedURL = GetRedirectURL(entity);
            }
            else if (REDIRECTION_FLAG_EXIT.Equals(p))
            {
                Logger.Info("Exit from govolution web site.");
                redirectedURL = ACAConstant.URL_DEFAULT;
            }
            else
            {
                Logger.Error("Error occurred from govolution web site");

                if (entity != null && string.IsNullOrEmpty(entity.ErrorMessage) && entity.PaymentResult != null && entity.PaymentResult.exceptionMsg != null)
                {
                    string tempErrorMessage = entity.PaymentResult.exceptionMsg[0];

                    if (!string.IsNullOrEmpty(tempErrorMessage))
                    {
                        Logger.ErrorFormat("Error message for handling payment results:{0}", tempErrorMessage);
                        entity.ErrorMessage = tempErrorMessage;
                    }
                }

                if (!string.IsNullOrEmpty(entity.ErrorMessage))
                {
                    string errorMessageID = DateTime.Now.Ticks.ToString();
                    HttpContext.Current.Session[errorMessageID] = entity.ErrorMessage;
                    redirectedURL = string.Format("../PaymentErrorPage.aspx?ErrorMessageID={0}", errorMessageID);
                }
                else
                {
                    redirectedURL = string.Format("../PaymentErrorPage.aspx");
                }
            }

            return redirectedURL;
        }

        /// <summary>
        /// Handle the data posted back from 3rd party web site
        /// </summary>
        /// <param name="currentPage">The current page.</param>
        public void HandlePostbackData(Page currentPage)
        {
            string p = HttpContext.Current.Request.QueryString[REDIRECTION_FLAGKEY];

            IPayment paymentProcessor = PaymentProcessorFactory.CreateProcessor();

            Logger.InfoFormat("the response data from 3rd party web site for verifypayment or notification: {0} \n", HttpContext.Current.Request.QueryString);

            if (REDIRECTION_FLAG_VERIFICATION.Equals(p))
            {
                Logger.DebugFormat("Begin ResponseVerification() \n");

                paymentProcessor.VerifyPayment(currentPage);

                Logger.DebugFormat("End ResponseVerification() \n");
            }
            else if (REDIRECTION_FLAG_NOTIFICATION.Equals(p))
            {
                Logger.DebugFormat("Begin ResponseNotification() \n");

                paymentProcessor.CompletePayment(currentPage);

                Logger.DebugFormat("End ResponseNotification() \n");
            }
        }

        /// <summary>
        /// Get key of saved data
        /// </summary>
        /// <param name="transactionID">transaction id</param>
        /// <returns>saved data key</returns>
        private string GetKeyOfSavedData(string transactionID)
        {
            string key = string.Format("Govolution_{0}", transactionID);
            return key;
        }

        /// <summary>
        /// Get the Redirect url after payment
        /// </summary>
        /// <param name="entity">payment No</param>
        /// <returns>the redirect url.</returns>
        private string GetRedirectURL(PaymentEntity entity)
        {
            //If enable shopping cart go to capcompletions.aspx else, go to capcompletion.aspx
            bool enableShoppingCart = StandardChoiceUtil.IsEnableShoppingCart();
            bool isSuperAgency = StandardChoiceUtil.IsSuperAgency();
            int stepNumber = entity.StepNumber + 1;
            string url = string.Empty;

            if (enableShoppingCart || isSuperAgency)
            {
                url = string.Format("~/Cap/CapCompletions.aspx?stepNumber={0}&Module={1}", entity.StepNumber + 1, entity.ModuleName);
            }
            else
            {
                url = string.Format("~/Cap/CapCompletion.aspx?stepNumber={0}&Module={1}&isRenewal={2}&isPay4ExistingCap={3}", entity.StepNumber + 1, entity.ModuleName, entity.RenewalFlag, entity.Pay4ExistingCapFlag);
            }

            return url;
        }

        #endregion Methods
    }
}