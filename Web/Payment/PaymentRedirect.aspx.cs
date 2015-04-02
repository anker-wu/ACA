#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: PaymentRedirect.aspx.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2009-2014
 *
 *  Description:
 *
 *  Notes:
 * $Id: GoToPayment.aspx.cs 131973 2009-07-27 01:07:37Z ACHIEVO\jackie.yu $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using System;
using System.Web;
using Accela.ACA.BLL.Finance;
using Accela.ACA.Common;
using Accela.ACA.Common.Log;
using Accela.ACA.Web.Common;
using Accela.ACA.Web.Payment;
using Accela.ACA.Web.Security;
using Accela.ACA.WSProxy;

/// <summary>
/// when adapter finish payment, it will redirect to this page with return code.
/// the page will redirect to completion page or error page depends on the return code
/// </summary>
[SuppressCsrfCheck]
public partial class Payment_PaymentRedirect : System.Web.UI.Page
{
    /// <summary>
    /// success code
    /// </summary>
    private const string SUCCESS_CODE = "1";

    /// <summary>
    /// failure code
    /// </summary>
    private const string FAILURE_CODE = "-1";

    /// <summary>
    /// the customer cancel payment and exit the web site
    /// </summary>
    private const string EXIT_CODE = "0";

    /// <summary>
    /// logger object.
    /// </summary>
    private static readonly log4net.ILog Logger = LogFactory.Instance.GetLogger(typeof(Payment_PaymentRedirect));

    /// <summary>
    /// page load entrance
    /// </summary>
    /// <param name="sender">object instance</param>
    /// <param name="e">event arguments</param>
    protected void Page_Load(object sender, EventArgs e)
    {
        Response.Buffer = true;

        LogPostbackData();

        string redirectedURL = string.Empty;
        string returnCode = PaymentHelper.ConvertToString(Request[PaymentConstant.ADAPTER2ACA_RETURN_CODE]);
        string transactionId = PaymentHelper.ConvertToString(Request[PaymentConstant.ADAPTER2ACA_TRANSACTION_ID]);
        string message = PaymentHelper.ConvertToString(Request[PaymentConstant.ADAPTER2ACA_USER_MESSAGE]);
        string agencyCode = ConfigManager.AgencyCode;

        PaymentEntity entity = PaymentHelper.GetDataFromCache<PaymentEntity>(transactionId);

        try
        {
            if (entity == null)
            {
                entity = PaymentHelper.GetPaymentEntityFromOnlinePaymentAuditLog(transactionId);
            }

            if (entity != null)
            {
                agencyCode = entity.ServProvCode;

                // For 2 cases, we need to get paymentresult from database again
                // 1, The payment entity is cache is missed
                // 2, The payment entiry is stored in another machine, like Load Balancer or BigIP
                if (entity.PaymentResult == null)
                {
                    ICommonPaymentBll commonPaymentBll = ObjectFactory.GetObject<ICommonPaymentBll>();
                    entity.PaymentResult = commonPaymentBll.GetPaymentResult(agencyCode, transactionId, entity.PublicUserID);
                }

                // For 2 cases, we needs to store transaction data into database dump
                // 1, The payment entity is cache is missed
                // 2, The payment entiry is stored in another machine, like Load Balancer or BigIP
                if (entity.PaymentResult != null)
                {
                    PaymentHelper.SaveOrUpdateDataToCache<PaymentEntity>(transactionId, entity);
                    AppSession.SetOnlinePaymentResultModelToSession(entity.PaymentResult);
                }
            }

            LogPaymentResult(entity);

            Logger.Debug("Begin HandlePaymentResult \n");

            //check return code
            if (returnCode.CompareTo(SUCCESS_CODE) == 0)
            {
                Logger.Info("The payment is successfully.");
                redirectedURL = GetRedirectURL(entity);
            }
            else if (returnCode.CompareTo(EXIT_CODE) == 0)
            {
                Logger.Info("Exit from 3rd web site.");
                redirectedURL = ACAConstant.URL_DEFAULT;
            }
            else if (returnCode.CompareTo(FAILURE_CODE) == 0 && string.IsNullOrEmpty(message))
            {
                Logger.Error("Error from 3rd web site.");
                redirectedURL = string.Format("~/payment/PaymentErrorPage.aspx?{0}={1}&{2}={3}", UrlConstant.AgencyCode, agencyCode, PaymentConstant.ADAPTER2ACA_TRANSACTION_ID, transactionId);
            }
            else
            {
                Logger.Error("Error occurred from 3rd web site.");

                string errorMessageID = DateTime.Now.Ticks.ToString();
                HttpContext.Current.Session[errorMessageID] = message;
                redirectedURL = string.Format("~/payment/PaymentErrorPage.aspx?ErrorMessageID={0}&{1}={2}&{3}={4}", errorMessageID, UrlConstant.AgencyCode, agencyCode, PaymentConstant.ADAPTER2ACA_TRANSACTION_ID, transactionId);
            }
        }
        catch (Exception ex)
        {
            Logger.Error("Error occurred when handling payment result.", ex);
            string errorMessageID = DateTime.Now.Ticks.ToString();
            Session[errorMessageID] = ex.Message;
            redirectedURL = string.Format("~/payment/PaymentErrorPage.aspx?ErrorMessageID={0}&{1}={2}&{3}={4}", errorMessageID, UrlConstant.AgencyCode, agencyCode, PaymentConstant.ADAPTER2ACA_TRANSACTION_ID, transactionId);
        }

        Logger.InfoFormat("payment redirected URL: {0}\n", redirectedURL);

        Response.Clear();

        HttpContext.Current.Session[ACAConstant.CURRENT_URL] = FileUtil.AppendApplicationRoot(redirectedURL.TrimStart('~'));
        Response.Redirect(ACAConstant.URL_DEFAULT_PAGE, false);
    }

    /// <summary>
    /// Log post back data
    /// </summary>
    private void LogPostbackData()
    {
        string allRequestString = PaymentHelper.GetPostbackDataString();
        Logger.InfoFormat("Entering PaymentRedirect.aspx Page, the request data to this page are: \n\r" + allRequestString);
    }

    /// <summary>
    /// log payment result from cache
    /// </summary>
    /// <param name="entity">the payment instance</param>
    private void LogPaymentResult(PaymentEntity entity)
    {
        if (entity != null && entity.PaymentResult != null)
        {
            Logger.Debug("payment result is not null \n");

            var capPaymentResults = entity.PaymentResult.capPaymentResultModels;
            var entityPaymentResults = entity.PaymentResult.entityPaymentResultModels;

            if (ACAConstant.PaymentEntityType.CAP.Equals(entity.EntityType) && capPaymentResults != null && capPaymentResults.Length > 0)
            {
                // For Cap Payment Process
                foreach (CapPaymentResultModel result in capPaymentResults)
                {
                    if (result != null && result.capID != null)
                    {
                        Logger.InfoFormat("cap id = \n" + result.capID.customID);
                    }
                    else
                    {
                        Logger.Error("CapPaymentResultModel is null.\n");
                    }
                }
            }
            else if (entityPaymentResults != null && entityPaymentResults.Length > 0)
            {
                // For Other Generic Payment Process for related Entity. such as Trust Account Deposit
                foreach (EntityPaymentResultModel result in entityPaymentResults)
                {
                    if (result != null && result.entityID != null)
                    {
                        Logger.InfoFormat("entity type = \n" + result.entityType);
                        Logger.InfoFormat("entity id = \n" + result.entityID);
                    }
                    else
                    {
                        Logger.Error("EntityPaymentResultModel is null.\n");
                    }
                }
            }
        }
        else
        {
            Logger.Error("payment result is null \n");
        }
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

        if (ACAConstant.PaymentEntityType.TrustAccount.Equals(entity.EntityType))
        {
            //For Trust Account Deposit Process
            url = string.Format("~/Account/DepositCompletion.aspx");
        }
        else if (enableShoppingCart || isSuperAgency)
        {
            //For Shopping Cart Payment Process
            url = string.Format("~/Cap/CapCompletions.aspx?stepNumber={0}&Module={1}", stepNumber, entity.ModuleName);
        }
        else
        {
            //For Normal Cap Payment Process
            url = string.Format(
                                    "~/Cap/CapCompletion.aspx?stepNumber={0}&Module={1}&isRenewal={2}&isPay4ExistingCap={3}",
                                    stepNumber,
                                    entity.ModuleName,
                                    entity.RenewalFlag,
                                    entity.Pay4ExistingCapFlag);
        }

        return url;
    }
}
