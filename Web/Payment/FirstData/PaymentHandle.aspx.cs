#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: PaymentHandle.aspx.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2009-2014
 *
 *  Description:
 *
 *  Notes:
 * $Id: PaymentHandle.aspx.cs 132442 2009-07-31 05:27:19Z ACHIEVO\jackie.yu $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using System;
using System.Collections.Generic;
using System.Text;
using System.Web;

using Accela.ACA.Common;
using Accela.ACA.Common.Log;
using Accela.ACA.Web;
using Accela.ACA.Web.Common;
using Accela.ACA.Web.Payment;
using Accela.ACA.WSProxy;

/// <summary>
/// FirstData payment handler
/// </summary>
public partial class Payment_FirstData_PaymentHandle : BasePage
{
    #region Private Fields

    /// <summary>
    /// Transaction id
    /// </summary>
    private const string TRANSACTION_ID = "i";

    /// <summary>
    /// Return code from 3rd
    /// </summary>
    private const string RETURN_CODE = "returnCode";

    /// <summary>
    /// success code
    /// </summary>
    private const string RETURN_SUCCESS = "1";

    /// <summary>
    /// no parameters in 3rd url
    /// </summary>
    private const string N0_RETURN_PARAMETERS = "-1";

    /// <summary>
    /// log4net Logger
    /// </summary>
    private static readonly log4net.ILog Logger = LogFactory.Instance.GetLogger(typeof(Payment_FirstData_PaymentHandle));

    #endregion

    /// <summary>
    /// <c>OnPreInit</c> event.
    /// </summary>
    /// <param name="e">EventArgs e</param>
    protected override void OnPreInit(EventArgs e)
    {
        Uri url = Session[ACAConstant.CURRENT_URL] as Uri;

        bool isFromCapCompletion = url != null && (url.AbsoluteUri.IndexOf("CapCompletion.aspx") != -1 || url.AbsoluteUri.IndexOf("CapCompletions.aspx") != -1);

        if (isFromCapCompletion)
        {
            Response.Redirect(url.AbsoluteUri);
            return;
        }

        base.OnPreInit(e);
    }

    /// <summary>
    /// Page load event
    /// </summary>
    /// <param name="sender">handler instance</param>
    /// <param name="e">event arguments</param>
    protected void Page_Load(object sender, EventArgs e)
    {
        // The Buffer property tells whether to buffer the output page being sent to the browser.
        Response.Buffer = true;
        string redirectedURL = string.Empty;

        // Log
        string queryString = PaymentHelper.GetPostbackDataString();
        Logger.InfoFormat("Entering FirstDataHandle.aspx Page, the request data from FirstData are: \n\r" + queryString);

        // Handler Payment result
        try
        {
            IPayment paymentProcessor = PaymentProcessorFactory.CreateProcessor();
            OnlinePaymentResultModel completeResult = paymentProcessor.CompletePayment(this);

            if (completeResult == null || completeResult.paramString == N0_RETURN_PARAMETERS || completeResult.exceptionMsg != null)
            {
                // If error, redirect to error page
                redirectedURL = RedirectToErrorPage(completeResult);
            }
            else
            {
                try
                {
                    // Rebuild completion url
                    redirectedURL = HandlePaymentResult(completeResult);
                }
                catch (Exception ex)
                {
                    Logger.Error("Error occurred when handling payment result after payment completed.", ex);
                    redirectedURL = RedirectToErrorPage(ex.ToString());
                }
            }
        }
        catch (Exception ex)
        {
            Logger.Error("Error occurred during handling payment completion.", ex);
            redirectedURL = RedirectToErrorPage(ex.ToString());
        }

        //Response.Redirect(redirectedURL);
        //We need to redirect to default page to handle payment result.
        Response.Clear();

        HttpContext.Current.Session[ACAConstant.CURRENT_URL] = FileUtil.AppendApplicationRoot(redirectedURL.TrimStart('~'));
        Response.Redirect(ACAConstant.URL_DEFAULT_PAGE, false);
    }

    /// <summary>
    /// Check the result status,Complete payment if success else redirect to error page.
    /// </summary>
    /// <param name="result">result from AA,only return code and return message needed.</param>
    /// <returns>the redirect url.</returns>
    private string HandlePaymentResult(OnlinePaymentResultModel result)
    {
        string url = string.Empty;
        Dictionary<string, string> resultParameters = PaymentHelper.ConvertToDictionary(result.paramString);
        string returnCode = resultParameters[RETURN_CODE];

        // Handle error except the return code equals "1"
        if (returnCode == RETURN_SUCCESS)
        {
            url = HandleSuccessProcess();
        }
        else
        {
            url = HandleErrorProcess();
        }

        return url;
    }

    /// <summary>
    /// Handle success process
    /// </summary>
    /// <returns>the redirect url.</returns>
    private string HandleSuccessProcess()
    {
        string url = string.Empty;
        string transactionID = Request[TRANSACTION_ID];
        PaymentEntity entity = PaymentHelper.GetDataFromCache<PaymentEntity>(transactionID);

        AppSession.SetOnlinePaymentResultModelToSession(entity.PaymentResult);

        if (entity != null)
        {
            url = GetRedirectURL(entity);
        }
        else
        {
            url = RedirectToErrorPage(string.Empty);
        }

        return url;
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

    /// <summary>
    /// If not success, consider all other conditions as error
    /// </summary>
    /// <returns>the url to error page</returns>
    private string HandleErrorProcess()
    {
        Logger.Error("Error occurred from FirstData web site");

        string message = string.Empty;
        string transactionID = Request[TRANSACTION_ID];

        Logger.Error("getting the previous information from cache:\n");
        PaymentEntity entity = PaymentHelper.GetDataFromCache<PaymentEntity>(transactionID);

        if (entity != null && !string.IsNullOrEmpty(entity.ErrorMessage))
        {
            message = entity.ErrorMessage;
        }

        return RedirectToErrorPage(message);
    }

    /// <summary>
    /// Get the redirect url to error page
    /// </summary>
    /// <param name="result">online payment result</param>
    /// <returns>the url to error page</returns>
    private string RedirectToErrorPage(OnlinePaymentResultModel result)
    {
        string message = string.Empty;

        if (result == null)
        {
            string transactionID = Request[TRANSACTION_ID];

            PaymentEntity entity = PaymentHelper.GetDataFromCache<PaymentEntity>(transactionID);

            if (entity != null && !string.IsNullOrEmpty(entity.ErrorMessage))
            {
                message = entity.ErrorMessage;
            }
        }
        else if (result.paramString == N0_RETURN_PARAMETERS)
        {
            message = GetTextByKey("aca_payment_cancel_notice");
        }
        else if (result.exceptionMsg != null)
        {
            StringBuilder msgBuffer = new StringBuilder();

            foreach (string msg in result.exceptionMsg)
            {
                msgBuffer.Append(msg + Environment.NewLine);
            }

            message = msgBuffer.ToString();
        }

        Logger.ErrorFormat("Error occurred when completing payment, message:{0}", message);

        return RedirectToErrorPage(message);
    }

    /// <summary>
    /// Get the redirect url to error page
    /// </summary>
    /// <param name="errorMsg">error message</param>
    /// <returns>the url to error page</returns>
    private string RedirectToErrorPage(string errorMsg)
    {
        string errorMessageID = DateTime.Now.Ticks.ToString();
        HttpContext.Current.Session[errorMessageID] = errorMsg;

        return string.Format("~/Payment/PaymentErrorPage.aspx?ErrorMessageID={0}", errorMessageID);
    }
}
