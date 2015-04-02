#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: FirstDataPayment.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2009-2014
 *
 *  Description:
 *  FirstDataPayment.
 *
 *  Notes:
 * $Id: FirstDataPayment.cs 140990 2009-07-31 12:15:03Z ACHIEVO\jackie.yu $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text;
using System.Web;
using System.Web.UI;
using Accela.ACA.BLL.Finance;
using Accela.ACA.Common;
using Accela.ACA.Common.Log;
using Accela.ACA.Common.Util;
using Accela.ACA.Web.Common;
using Accela.ACA.WSProxy;

namespace Accela.ACA.Web.Payment
{
    /// <summary>
    /// FirstPayment process
    /// </summary>
    public class FirstDataPayment : IPayment
    {
        #region Private Fields

        /// <summary>
        /// The url using which redirect to  FirstData web site
        /// </summary>
        private const string HOST_URL = "HostURL";

        /// <summary>
        /// AA transaction id
        /// </summary>
        private const string LOCAL_TRANSACTION_ID = "id";

        /// <summary>
        /// Payment amount
        /// </summary>
        private const string PAYMENT_AMOUNT = "amount";

        /// <summary>
        /// The return url which sent to FristData, and when finish payment, FirstData will use this url to redirect back to ACA
        /// </summary>
        private const string PARAMS_RETURN_URL = "returnurl";

        /// <summary>
        /// The AA transaction id that will sent to FristData, and FirstData will return back to ACA when finish payment
        /// </summary>
        private const string PARAMS_TRANSACTION_ID = "id";

        /// <summary>
        /// The AA transaction id which FirstData will save to it's database
        /// </summary>
        private const string PARAMS_REFERENCE_ID = "ref";

        /// <summary>
        /// The payment amount as part of the FirstData parameters
        /// </summary>
        private const string PARAMS_PAYMENT_AMOUNT = "amount";

        /// <summary>
        /// The AA transaction id return from FirstData
        /// </summary>
        private const string TRANSACTION_ID = "i";

        /// <summary>
        /// The flag which if no parameters return from FirstData used
        /// </summary>
        private const string N0_RETURN_PARAMETERS = "-1";

        /// <summary>
        /// log4net Logger
        /// </summary>
        private static readonly log4net.ILog Logger = LogFactory.Instance.GetLogger(typeof(FirstDataPayment));

        #endregion

        #region IPayment Members

        /// <summary>
        /// Initiate payment before 3rd do payment process
        /// </summary>
        /// <param name="currentPage">Source page instance</param>
        /// <returns>online payment result model</returns>
        public OnlinePaymentResultModel InitiatePayment(Page currentPage)
        {
            // 1. Log
            Logger.Debug("1. Begin initial payment.");

            // 2. Cache payment status, this status will save to cache which will be reused to build url
            // when 3rd web site finish payment and redirect to paymenthandle.aspx
            PaymentEntity entity = CachePaymentStatus(currentPage);
            OnlinePaymentResultModel paymentResult = new OnlinePaymentResultModel();

            try
            {
                // 3. Call InitiatePayment web service
                paymentResult = CallIntiatePaymentWebService(entity);

                if (paymentResult != null
                    && !string.IsNullOrEmpty(paymentResult.paramString) 
                    && paymentResult.exceptionMsg == null
                    && !string.IsNullOrEmpty(paymentResult.batchNbr))
                {
                    // 4. Combine url for 3rd
                    string url = CombineFirstDataURL(paymentResult.batchNbr, paymentResult.paramString);

                    if (string.IsNullOrEmpty(url))
                    {
                        paymentResult.exceptionMsg = new string[] { "EPaymentConfig is null or empty." };
                    }
                    else
                    {
                        // 5. Save payment status to cache before redirect to 3rd web site
                        PaymentHelper.SaveDataToCache<PaymentEntity>(paymentResult.batchNbr, entity);

                        /* 6. Redirect to 3rd web site
                         * HttpContext.Current.Response.Redirect(url);
                         * "Unable to evaluate expression because the code is optimized or a native frame is on top of the call stack."
                         * In order to avoid the exception, we can't redirect the url by original method above
                         * So we need to open third-party web site in parent page without iframe.
                         */
                        string pbMsg = LabelUtil.GetTextByKey("aca_payment_redirection_text", string.Empty);
                        pbMsg = Accela.ACA.Common.Common.ScriptFilter.EncodeJson(pbMsg);

                        string strScript = string.Format("<script>document.body.innerHTML = '<span class=\"font13px\">{0}</span>';NeedAsk = false; window.open('{1}','_parent');</script>", pbMsg, url);

                        Logger.InfoFormat("Redirect to FirstData Payment URL : {0}", url);
                        Page curPage = HttpContext.Current.Handler as Page;
                        
                        if (curPage != null)
                        {
                            curPage.RegisterClientScriptBlock("FirstData", strScript);
                        }
                    }
                }
                else
                {
                    string msg = "Error in paymentBll.InitiatePayment, no result returned.";
                    Logger.ErrorFormat("2. Error in paymentBll.InitiatePayment,Error message:{0}. ", msg);
                    paymentResult.exceptionMsg = new string[] { msg };
                }
            }
            catch (Exception ex)
            {
                Logger.Error("2. Error in paymentBll.InitiatePayment.", ex);
                paymentResult.exceptionMsg = new string[] { ex.Message };
            }

            return paymentResult;
        }

        /// <summary>
        /// Method for invoking CompletePayment service.
        /// </summary>
        /// <param name="currentPage">Source page instance</param>
        /// <returns>Payment result</returns>
        public OnlinePaymentResultModel CompletePayment(Page currentPage)
        {
            // 1. Log
            Logger.Debug("Begin CompletePayment process.");

            // 2. Get query string from 3rd request
            string queryString = PaymentHelper.GetPostbackDataString();
            
            // 3. Log query string
            Logger.InfoFormat("The query string from FirstData: {0}", queryString);

            // 4. Get transaction id from 3rd url
            string transactionID = HttpContext.Current.Request[TRANSACTION_ID];
            OnlinePaymentResultModel result = new OnlinePaymentResultModel();

            // 5. If user exist page in 3rd website, it may be return no parameters
            if (string.IsNullOrEmpty(transactionID))
            {
                Logger.ErrorFormat("Error occurred in CompletePayment method, error message:{0}", "Payment Cancel.");
                result.paramString = N0_RETURN_PARAMETERS;
            }
            else
            {
                // 6. Get original payment status from cache
                PaymentEntity entity = PaymentHelper.GetDataFromCache<PaymentEntity>(transactionID);

                if (entity != null)
                {
                    // 7. Call CompletePayment web service
                    result = CallCompletePyamentWebService(entity, transactionID);
                }
                else
                {
                    Logger.ErrorFormat("Error occurred in CompletePayment method: Can not find PaymentEntity. The Transaction id is :{0}", transactionID);
                    result.paramString = N0_RETURN_PARAMETERS;
                }
            }

            return result;
        }

        /// <summary>
        /// Get 3rd page height to suit the ACA iFrame
        /// </summary>
        /// <returns>the height of 3rd page</returns>
        public int Get3rdPageHeight()
        {
            return 1000;
        }

        /// <summary>
        /// Get the EPayment configuration
        /// </summary>
        /// <param name="policy">the XPolicy data from database</param>
        /// <returns>the EPayment configuration key-value pairs</returns>
        public Hashtable GetEPaymentConfig(XPolicyModel policy)
        {
            if (policy == null)
            {
                return null;
            }

            Hashtable items = new Hashtable(StringComparer.InvariantCultureIgnoreCase);
            DataUtil.ParseString(items, policy.data1, ';', '=');
            DataUtil.ParseString(items, policy.data2, ';', '=');
            DataUtil.ParseString(items, policy.data3, ';', '=');
            DataUtil.ParseString(items, policy.data4, ';', '=');

            // Get params from xpolicy 'data3' column
            if (items.ContainsKey("RequestParameterMap"))
            {
                DataUtil.ParseString(items, items["RequestParameterMap"].ToString(), ',', ':');
            }

            // Replace $$ placeholder
            Hashtable configs = new Hashtable();
            string filter = "$$";

            foreach (DictionaryEntry dic in items)
            {
                string value = dic.Value.ToString();

                if (value.IndexOf(filter) > -1)
                {
                    configs[dic.Key] = value.Replace(filter, string.Empty);
                }
                else
                {
                    configs[dic.Key] = dic.Value;
                }
            }

            return configs;
        }

        /// <summary>
        /// No need to implement this method
        /// </summary>
        /// <param name="currentPage">Source page instance</param>
        /// <returns>online payment result model</returns>
        public OnlinePaymentResultModel VerifyPayment(Page currentPage)
        {
            return null;
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Get payment status, this status will save to cache which will be reused to build url
        /// when FirstData finish payment and redirect to <c>paymenthandle.aspx</c>
        /// </summary>
        /// <param name="currentPage">current page</param>
        /// <returns>the paymentEntity which holds the payment status</returns>
        private PaymentEntity CachePaymentStatus(Page currentPage)
        {
            IPage page = currentPage as IPage;
            NameValueCollection query = HttpContext.Current.Request.QueryString;
            PaymentEntity entity = new PaymentEntity();

            entity.ModuleName = page.GetModuleName();
            entity.CapIDs = AppSession.GetCapIDModelsFromSession();
            entity.PublicUserID = AppSession.User.PublicUserId;
            entity.ServProvCode = ConfigManager.AgencyCode;
            entity.RenewalFlag = query.Get("isRenewal");
            entity.Pay4ExistingCapFlag = query.Get("isPay4ExistingCap");
            
            // record the step number
            int stepNumber = 0;
            int.TryParse(query.Get("stepNumber"), out stepNumber);
            entity.StepNumber = stepNumber;

            return entity;
        }

        /// <summary>
        /// Call InitiatePayment web service
        /// </summary>
        /// <param name="entity">the payment status</param>
        /// <returns>online payment result</returns>
        private OnlinePaymentResultModel CallIntiatePaymentWebService(PaymentEntity entity)
        {
            Logger.Debug("Begin to call InitiatePayment web service");

            IPaymentBll paymentBll = (IPaymentBll)ObjectFactory.GetObject(typeof(IPaymentBll));

            string parameters = string.Format("{0}={1}", ACAConstant.ServProvCode_Key, entity.ServProvCode);

            OnlinePaymentResultModel result = paymentBll.InitiatePayment(TempModelConvert.Trim4WSOfCapIDModels(entity.CapIDs), parameters, "FirstData", entity.PublicUserID);

            // Log callback result
            if (result == null)
            {
                Logger.Info("No result returned from InitiatePayment method.");
            }
            else
            {
                Logger.InfoFormat("The result paramString is:{0}", result.paramString);
            }
            
            return result;
        }

        /// <summary>
        /// Call CompletePayment service
        /// </summary>
        /// <param name="entity">The payment status from cache</param>
        /// <param name="transactionID">AA Transaction ID</param>
        /// <returns>online payment result</returns>
        private OnlinePaymentResultModel CallCompletePyamentWebService(PaymentEntity entity, string transactionID)
        {
            Logger.Debug("Calling CompletePayment web service");
            string parameters = GetPostbackData();

            try
            {
                parameters += "&ServProvCode=" + entity.ServProvCode;
                IPaymentBll paymentBll = (IPaymentBll)ObjectFactory.GetObject(typeof(IPaymentBll));
                entity.PaymentResult = paymentBll.CompletePayment(
                                                                                    null,
                                                                                    null,
                                                                                    TempModelConvert.Trim4WSOfCapIDModels(entity.CapIDs),
                                                                                    parameters,
                                                                                    "FirstData",
                                                                                    entity.PublicUserID);

                if (entity.PaymentResult != null)
                {
                    Logger.InfoFormat("Return data from CompletePayment method: {0}", entity.PaymentResult.paramString);
                }
                else
                {
                    string messag = "No result returned from CompletePayment web service.";
                    Logger.Error(messag);
                    entity.ErrorMessage = messag;
                }
            }
            catch (Exception ex)
            {
                Logger.Error("Error occurred in CompletePayment method.", ex);
            }

            // Save payment result back to cache
            PaymentHelper.SaveDataToCache<PaymentEntity>(transactionID, entity);

            return entity.PaymentResult;
        }

        /// <summary>
        /// Combine url for FirstData
        /// </summary>
        /// <param name="transactionID">AA transaction id</param>
        /// <param name="responseText">the response string AA</param>
        /// <returns>the url to the 3rd web site</returns>
        private string CombineFirstDataURL(string transactionID, string responseText)
        {
            // Use dictionary to hold the response key-value
            //The response string from web service contains three params: transactionID, PaymentAmount and ConvenienceFee
            Dictionary<string, string> paramPair = ConvertToDictionary(responseText, '&');

            // Get the parameters for 3rd
            string paymentAmount = GetValueFromDictionary(paramPair, PAYMENT_AMOUNT, "0");

            // Get configuration from xpolicy
            Hashtable configs = EPaymentConfig.GetConfig();

            if (configs == null || configs.Count == 0)
            {
                Logger.Error("EPaymentConfig is null or empty.");
                return string.Empty;
            }

            string hostURL = GetValueFromHashtable(configs, HOST_URL);
            string returnURL = GetReturnUrl();
            string urlPattern = @"{0}?{1}={2}&{3}={4}&{5}={6}&{7}={8}";
            string url = string.Format(
                                                urlPattern, 
                                                hostURL, 
                                                PARAMS_RETURN_URL, 
                                                HttpUtility.UrlEncode(returnURL), 
                                                PARAMS_TRANSACTION_ID, 
                                                transactionID, 
                                                PARAMS_REFERENCE_ID, 
                                                transactionID, 
                                                PARAMS_PAYMENT_AMOUNT, 
                                                paymentAmount);

            Logger.InfoFormat("The url to FirstData:{0}.", url);

            return url;
        }

        /// <summary>
        /// Convert response string to key values pairs
        /// </summary>
        /// <param name="responseText">the request string or response string</param>
        /// <param name="splitChar">the char to split response string</param>
        /// <returns>the key values pairs</returns>
        private Dictionary<string, string> ConvertToDictionary(string responseText, char splitChar)
        {
            Dictionary<string, string> dictionary = new Dictionary<string, string>(StringComparer.InvariantCultureIgnoreCase);

            if (string.IsNullOrEmpty(responseText))
            {
                return dictionary;
            }

            string[] pairs = responseText.Split(splitChar);

            foreach (string param in pairs)
            {
                string[] keyValues = param.Split('=');

                if (keyValues.Length == 2)
                {
                    string key = keyValues[0];
                    string value = keyValues[1];

                    if (!dictionary.ContainsKey(key) && !string.IsNullOrEmpty(key))
                    {
                        dictionary.Add(key, value);
                    }
                }
            }

            return dictionary;
        }

        /// <summary>
        /// Get value from dictionary
        /// </summary>
        /// <param name="dictionary">the dictionary holds parameters</param>
        /// <param name="key">the dictionary key</param>
        /// <returns>value of parameter</returns>
        private string GetValueFromDictionary(Dictionary<string, string> dictionary, string key)
        {
            return GetValueFromDictionary(dictionary, key, string.Empty);
        }

        /// <summary>
        /// Get value from dictionary, if not found, return default value
        /// </summary>
        /// <param name="dictionary">the dictionary holds parameters</param>
        /// <param name="key">the dictionary key</param>
        /// <param name="defaultValue">if can not find the key in dictionary , return default value</param>
        /// <returns>value of parameter</returns>
        private string GetValueFromDictionary(Dictionary<string, string> dictionary, string key, string defaultValue)
        {
            return dictionary.ContainsKey(key) ? dictionary[key] : defaultValue;
        }

        /// <summary>
        /// Get config value from hashtable
        /// </summary>
        /// <param name="configs">EPayment config</param>
        /// <param name="key">hashtable key</param>
        /// <returns>config value</returns>
        private string GetValueFromHashtable(Hashtable configs, string key)
        {
            if (configs.ContainsKey(key) && configs[key] != null)
            {
                return configs[key].ToString().Trim();
            }

            return string.Empty;
        }

        /// <summary>
        /// Gets the payment return url that first data will callback.
        /// </summary>
        /// <returns>payment return url.</returns>
        private string GetReturnUrl()
        {
            string host = HttpContext.Current.Request.Url.Host;
            string port = HttpContext.Current.Request.Url.Port.ToString();
            string http = HttpContext.Current.Request.Url.Scheme;
            string appPath = HttpContext.Current.Request.ApplicationPath;
            string url = string.Empty;

            if (!string.IsNullOrEmpty(appPath))
            {
                if (!appPath.EndsWith("/", StringComparison.InvariantCulture))
                {
                    appPath += "/";
                }
            }
            else
            {
                appPath = "/";
            }

            string paymentFile = "payment/firstdata/paymenthandle.aspx";

            // If port is set to default value, needn't show this port
            if (port == "80")
            {
                url = string.Format("{0}://{1}{2}{3}", http, host, appPath, paymentFile);
            }
            else
            {
                url = string.Format("{0}://{1}:{2}{3}{4}", http, host, port, appPath, paymentFile);
            }

            return url;
        }

        /// <summary>
        /// get post back form data with query string style.
        /// </summary>
        /// <returns>post back query string</returns>
        private string GetPostbackData()
        {
            HttpRequest request = HttpContext.Current.Request;
            string[] parameters = request.QueryString.AllKeys;
            StringBuilder sb = new StringBuilder();

            foreach (string key in parameters)
            {
                if (sb.Length > 0)
                {
                    sb.Append("&");
                }

                string value = string.Empty;

                value = HttpContext.Current.Server.UrlEncode(request.QueryString[key]);

                string paramKey = key;

                //replace i to batchTransNbr
                if (key.ToLower() == "i")
                {
                    paramKey = "batchTransNbr";
                }

                sb.Append(string.Format("{0}={1}", paramKey, value));
            }

            return sb.ToString();
        }

        #endregion
    }
}
