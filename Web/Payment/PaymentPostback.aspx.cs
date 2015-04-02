#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: PaymentPostback.aspx.cs
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
using System.Globalization;
using System.Text;
using System.Web;
using Accela.ACA.BLL.Finance;
using Accela.ACA.Common;
using Accela.ACA.Common.Log;
using Accela.ACA.Common.Util;
using Accela.ACA.Web.Common;
using Accela.ACA.Web.Payment;
using Accela.ACA.Web.Security;
using Accela.ACA.WSProxy;
using log4net;

/// <summary>
/// payment post back page for adapter
/// </summary>
[SuppressCsrfCheck]
public partial class Payment_PaymentPostback : System.Web.UI.Page
{
    /// <summary>
    /// create cap failed
    /// </summary>
    private const string CREATE_CAP_FAILED = "false";

    /// <summary>
    /// create cap successful
    /// </summary>
    private const string CREATE_CAP_SUCCESS = "true";

    /// <summary>
    /// logger object.
    /// </summary>
    private static readonly ILog Logger = LogFactory.Instance.GetLogger(typeof(Payment_PaymentPostback));

    /// <summary>
    /// create cap when payment completed
    /// </summary>
    /// <param name="sender">object handler</param>
    /// <param name="e">event arguments</param>
    protected void Page_Load(object sender, EventArgs e)
    {
        string success = CREATE_CAP_FAILED;
        string message = string.Empty;

        HttpRequest request = HttpContext.Current.Request;
        string transactionId = PaymentHelper.ConvertToString(request[PaymentConstant.ADAPTER2ACA_TRANSACTION_ID]);
        string ccNumber = PaymentHelper.ConvertToString(request[PaymentConstant.ADAPTER2ACA_CC_NUMBER]);
        string paymentAmount = PaymentHelper.ConvertToString(request[PaymentConstant.ADAPTER2ACA_PAYMENT_AMOUNT]);
        string convFee = PaymentHelper.ConvertToString(request[PaymentConstant.ADAPTER2ACA_CONVENIENCE_FEE]);
        string customBatchTransNbr = PaymentHelper.ConvertToString(request[PaymentConstant.ADAPTER2ACA_CUSTOM_BATCH_TRANS_NBR]);
        string dataFromPaymentAdapter = PaymentHelper.GetPostbackDataString();
        string procTransactionId = PaymentHelper.ConvertToString(request[PaymentConstant.ADAPTER2ACA_PROC_TRANSACTION_ID]);

        if (string.IsNullOrWhiteSpace(transactionId) || string.IsNullOrWhiteSpace(paymentAmount) || string.IsNullOrWhiteSpace(convFee) || string.IsNullOrWhiteSpace(procTransactionId))
        {
            message = LabelUtil.GetTextByKey("aca_payment_msg_datavalidation_error", string.Empty);

            if (string.IsNullOrWhiteSpace(transactionId))
            {
                message += PaymentConstant.ADAPTER2ACA_TRANSACTION_ID + ACAConstant.COMMA;
            }

            if (string.IsNullOrWhiteSpace(paymentAmount))
            {
                message += PaymentConstant.ADAPTER2ACA_PAYMENT_AMOUNT + ACAConstant.COMMA;
            }

            if (string.IsNullOrWhiteSpace(convFee))
            {
                message += PaymentConstant.ADAPTER2ACA_CONVENIENCE_FEE + ACAConstant.COMMA;
            }

            if (string.IsNullOrWhiteSpace(procTransactionId))
            {
                message += PaymentConstant.ADAPTER2ACA_PROC_TRANSACTION_ID;
            }

            if (message.EndsWith(ACAConstant.COMMA))
            {
                message = message.Remove(message.Length - 1, 1);
            }

            Logger.Error(message);
        }
        else
        {
            Logger.InfoFormat("Data from payment adapter after user paid successfully:{0}\n", dataFromPaymentAdapter);

            Logger.InfoFormat("start to create cap, the transaction id from adapter is {0}, the ccNumber is {1}", transactionId, ccNumber);

            PaymentEntity entity = null;

            if (!string.IsNullOrEmpty(transactionId))
            {
                Logger.InfoFormat("Transaction ID is " + transactionId);
                entity = PaymentHelper.GetDataFromCache<PaymentEntity>(transactionId);

                // For 2 cases, we needs to store transaction data into database dump
                // 1, The payment entity is cache is missed
                // 2, The payment entiry is stored in another machine, like Load Balancer or BigIP
                if (entity == null)
                {
                    Logger.InfoFormat("Payment Entity is not in cache, so get it from database");
                    entity = PaymentHelper.GetPaymentEntityFromOnlinePaymentAuditLog(transactionId);
                }
            }

            if (entity != null)
            {
                PaymentModel paymentModel = new PaymentModel();
                paymentModel.batchTransCode = long.Parse(transactionId);

                double amount = 0.0;
                double fee = 0.0;
                bool isNumber = I18nNumberUtil.TryParseMoneyFromWebService(paymentAmount, out amount);
                I18nNumberUtil.TryParseMoneyFromWebService(convFee, out fee);

                paymentModel.tranCode = procTransactionId;
                paymentModel.ccType = PaymentHelper.ConvertToString(request[PaymentConstant.ADAPTER2ACA_CC_TYPE]);
                paymentModel.ccAuthCode = PaymentHelper.ConvertToString(request[PaymentConstant.ADAPTER2ACA_CC_AUTH_CODE]);
                paymentModel.paymentAmount = amount;
                paymentModel.processingFee = fee;
                paymentModel.paymentMethod = PaymentHelper.ConvertToString(request[PaymentConstant.ADAPTER2ACA_PROC_TRANSACTION_TYPE]);
                paymentModel.payee = PaymentHelper.ConvertToString(request[PaymentConstant.ADAPTER2ACA_PAYEE]);
                paymentModel.payeeAddress = PaymentHelper.ConvertToString(request[PaymentConstant.ADAPTER2ACA_PAYEE_ADDRESS]);
                paymentModel.payeePhone = PaymentHelper.ConvertToString(request[PaymentConstant.ADAPTER2ACA_PAYEE_PHONE]);
                paymentModel.paymentComment = PaymentHelper.ConvertToString(request[PaymentConstant.ADAPTER2ACA_PAYMENT_COMMENT]);

                string parameters = string.Format("{0}={1}", ACAConstant.ServProvCode_Key, ConfigManager.AgencyCode);
                parameters += string.Format(
                                            CultureInfo.InvariantCulture,
                                            "&EntityType={0}&EntityID={1}&totalFee={2}&{3}={4}&{5}={6}&PaymentMethod={7}&MerchantAccountID={8}",
                                            entity.EntityType.ToString(),
                                            HttpUtility.UrlEncode(entity.EntityID),
                                            entity.TotalFee,
                                            ACAConstant.CREDIT_CARD_NBR_KEY,
                                            ccNumber,
                                            PaymentConstant.ADAPTER2ACA_CUSTOM_BATCH_TRANS_NBR,
                                            customBatchTransNbr,
                                            entity.PaymentMethod == PaymentMethod.Check ? ACAConstant.PAY_METHOD_CHECK : ACAConstant.PAY_METHOD_CREDIT_CARD,
                                            entity.MerchantAccountID == 0 ? string.Empty : entity.MerchantAccountID.ToString());

                OnlinePaymentResultModel paymentResult = null;

                if (isNumber)
                {
                    try
                    {
                        ICommonPaymentBll commonPaymentBll = (ICommonPaymentBll)ObjectFactory.GetObject(typeof(ICommonPaymentBll));
                        paymentResult = commonPaymentBll.CompleteOnlinePayment(entity.ServProvCode, paymentModel, parameters, entity.PublicUserID);

                        //save payment result to session, shopping cart will reuse it in capcompletions.aspx
                        entity.PaymentResult = paymentResult;
                        PaymentHelper.SaveOrUpdateDataToCache<PaymentEntity>(transactionId, entity);
                        AppSession.SetOnlinePaymentResultModelToSession(paymentResult);
                    }
                    catch (Exception ex)
                    {
                        Logger.Error("Error occurred during completing online payment.", ex);
                        message = HttpUtility.HtmlEncode(ex.Message);
                        entity.ErrorMessage = PaymentHelper.FilterAAException(message);

                        if (!string.IsNullOrEmpty(entity.ErrorMessage))
                        {
                            message = entity.ErrorMessage;
                        }

                        PaymentHelper.SaveOrUpdateDataToCache<PaymentEntity>(transactionId, entity);
                    }
                }
                else
                {
                    message = "payment amount is not a valid number";
                }

                if (paymentResult != null)
                {
                    if (ACAConstant.PaymentEntityType.CAP.Equals(entity.EntityType))
                    {
                        //Cap Payment Result
                        if (paymentResult.capPaymentResultModels != null && paymentResult.capPaymentResultModels.Length > 0)
                        {
                            foreach (CapPaymentResultModel result in paymentResult.capPaymentResultModels)
                            {
                                if (result != null && result.paymentStatus)
                                {
                                    success = CREATE_CAP_SUCCESS;
                                }
                                else
                                {
                                    success = CREATE_CAP_FAILED;
                                    break;
                                }
                            }
                        }
                    }
                    else
                    {
                        //Others Entity Payment Result such as Trust Account Deposit Payment.
                        if (paymentResult.entityPaymentResultModels != null && paymentResult.entityPaymentResultModels.Length > 0)
                        {
                            foreach (EntityPaymentResultModel result in paymentResult.entityPaymentResultModels)
                            {
                                if (result != null && ACAConstant.COMMON_ZERO.Equals(result.errorCode))
                                {
                                    success = CREATE_CAP_SUCCESS;
                                }
                                else
                                {
                                    success = CREATE_CAP_FAILED;
                                    break;
                                }
                            }
                        }
                    }

                    StringBuilder msgbuffer = new StringBuilder();

                    if (paymentResult.exceptionMsg != null)
                    {
                        foreach (string excceptionMsg in paymentResult.exceptionMsg)
                        {
                            msgbuffer.Append(excceptionMsg);
                        }
                    }

                    Logger.Debug(msgbuffer.ToString());
                    message = HttpUtility.HtmlEncode(msgbuffer.ToString());
                }
                else
                {
                    if (string.IsNullOrEmpty(message))
                    {
                        message = "create cap failed, payment result is null";
                    }

                    Logger.Error(message);
                }
            }
            else
            {
                message = "can not get parameters from cache";
                Logger.Error(message);
            }
        }

        string responseText = string.Format("success={0}&user_message={1}&TransactionID={2}", success, message, transactionId);
        Logger.InfoFormat("response text for payment postback:{0}", responseText);
        Response.Write(responseText);
    }

    /// <summary>
    /// get payment parameters from cache
    /// </summary>
    /// <param name="transactionId">transaction id</param>
    /// <returns>EPaymentParameter Model</returns>
    private PaymentEntity GetParameterFromCache(string transactionId)
    {
        if (string.IsNullOrEmpty(transactionId))
        {
            return null;
        }

        PaymentEntity paymentEntity = PaymentHelper.GetDataFromCache<PaymentEntity>(transactionId);

        if (paymentEntity == null)
        {
            Logger.ErrorFormat("Payment Entity [Transaction ID:{0}] is lost in cache, so need to retrieve it again.", transactionId);

            ICommonPaymentBll commonPaymentBll = (ICommonPaymentBll)ObjectFactory.GetObject(typeof(ICommonPaymentBll));

            // For TPE payment method, some sub-agency records are paid on super agency.
            // So, the first parameter agencyCode should be empty.
            TransactionModel[] trans = commonPaymentBll.GetTransactionModelById(string.Empty, transactionId);

            if (trans != null && trans.Length > 0)
            {
                paymentEntity = new PaymentEntity();
                paymentEntity.ServProvCode = trans[0].serviceProviderCode;
                paymentEntity.EntityType = ACAConstant.PaymentEntityType.CAP;
                paymentEntity.EntityID = trans[0].entityID;
                paymentEntity.TotalFee = trans[0].totalFee.Value;
                paymentEntity.PublicUserID = trans[0].auditID;

                Logger.ErrorFormat(
                            "Payment Entity Data: {0}||{1}||{2}||{3}",
                            paymentEntity.ServProvCode,
                            paymentEntity.EntityID,
                            paymentEntity.TotalFee,
                            paymentEntity.PublicUserID);

                PaymentHelper.SaveOrUpdateDataToCache<PaymentEntity>(transactionId, paymentEntity);
            }
        }

        return paymentEntity;
    }
}
