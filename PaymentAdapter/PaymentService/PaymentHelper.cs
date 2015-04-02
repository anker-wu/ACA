/**
 *  Accela Citizen Access
 *  File: PaymentHelper.cs
 * 
 *  Accela, Inc.
 *  Copyright (C): 2009-2013
 * 
 *  Description:
 *  
 * 
 *  Notes:
 * $Id: PaymentHelper.cs 130107 2009-07-21 12:23:56Z ACHIEVO\cary.cao $.
 *  Revision History
 *  Date,                  Who,                 What
 */

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Caching;
using Accela.ACA.Payment.Xml;
using Accela.ACA.PaymentAdapter.Service;
using System.Security.Cryptography.X509Certificates;
using System.Net.Security;

namespace Accela.ACA.PaymentAdapter
{
    /// <summary>
    /// the payment helper class
    /// </summary>
    public class PaymentHelper
    {
        /// <summary>
        /// logger object.
        /// </summary>
        private static readonly log4net.ILog _logger = LogFactory.Instance.GetLogger(typeof(PaymentHelper));

        /// <summary>
        /// initial transaction
        /// </summary>
        /// <param name="parameters">parameters to be saved</param>
        public static void InitialTransaction(Hashtable parameters)
        {
            OnlinePaymentTransactionModel transactionModel = new OnlinePaymentTransactionModel();

            transactionModel.aaTransId = ParameterHelper.GetParameterByKey(parameters, PaymentConstant.TRANSACTION_ID);
            transactionModel.applicationId = ParameterHelper.GetParameterByKey(parameters, PaymentConstant.APPLICATION_ID);

            string fee = ParameterHelper.GetParameterByKey(parameters, PaymentConstant.CONVENIENCE_FEE);

            if (String.IsNullOrEmpty(fee))
            {
                fee = "0.0";
            }

            transactionModel.convenienceFee = fee;

            string amount = ParameterHelper.GetParameterByKey(parameters, PaymentConstant.PAYMENT_AMOUNT);
            
            if (String.IsNullOrEmpty(amount))
            {
                amount = "0.0";
            }

            transactionModel.paymentAmount = amount;

            transactionModel.notificationURL = ParameterHelper.GetParameterByKey(parameters, PaymentConstant.POSTBACK_URL);
            transactionModel.redirectToACAURL = ParameterHelper.GetParameterByKey(parameters, PaymentConstant.REDIRECT_URL);
            transactionModel.recFulNam = ParameterHelper.GetParameterByKey(parameters, PaymentConstant.USER_ID);
            transactionModel.payorName = HttpUtility.UrlDecode(ParameterHelper.GetParameterByKey(parameters, PaymentConstant.FULL_NAME));
            transactionModel.recDate = DateTime.Now;

            PaymentService payment = new PaymentService();
            payment.CreatePaymentGatewayTrans(transactionModel);
        }

        /// <summary>
        /// Post data to ACA and get the result, use this method to complete cap process
        /// </summary>
        /// <param name="responseUrl">the url to ACA</param>
        /// <param name="postData">the data from third part web site</param>
        /// <returns>the result from ACA, success or failed and the message</returns>
        public static string DoPostBack(string responseUrl, string postData)
        {
            ASCIIEncoding encoding = new ASCIIEncoding();

            byte[] data = encoding.GetBytes(postData);

            // Prepare web request...
            HttpWebRequest myRequest = (HttpWebRequest)WebRequest.Create(responseUrl);

            myRequest.Method = "POST";
            myRequest.ContentType = "application/x-www-form-urlencoded";
            myRequest.ContentLength = data.Length;
            myRequest.Timeout = 300000;

            // Ignore Certificate Validation
            ServicePointManager.ServerCertificateValidationCallback = delegate(Object obj, X509Certificate certificate, X509Chain chain, SslPolicyErrors errors) { return (true); };
            
            using (Stream newStream = myRequest.GetRequestStream())
            {
                // Send the data.
                newStream.Write(data, 0, data.Length);
            }

            HttpWebResponse myResponse = null;
            string content = String.Empty;

            try
            {
                myResponse = (HttpWebResponse)myRequest.GetResponse();
                content = new StreamReader(myResponse.GetResponseStream(), Encoding.UTF8).ReadToEnd();
            }
            catch (Exception ex)
            {
                _logger.Error("Error occurred when doing postback.", ex);
            }
            finally
            {
                if (myResponse != null)
                {
                    myResponse.Close();
                }
            }

            return content;
        }

        /// <summary>
        /// Do request by HTTP GET method.
        /// </summary>
        /// <param name="requestUrl">The request url</param>
        /// <returns>The result that respone from the request url.</returns>
        public static string RequestGet(string requestUrl)
        {
            // Prepare web request...
            HttpWebRequest myRequest = (HttpWebRequest)WebRequest.Create(requestUrl);
            HttpWebResponse myResponse = null;
            string content = String.Empty;

            try
            {
                myRequest.Method = "GET";
                myRequest.ContentType = "application/x-www-form-urlencoded";
               
                myResponse = (HttpWebResponse)myRequest.GetResponse();
                content = new StreamReader(myResponse.GetResponseStream(), Encoding.UTF8).ReadToEnd();
            }
            catch (Exception ex)
            {
                _logger.Error("Error occurred when request GET method.", ex);
            }
            finally
            {
                if (myResponse != null)
                {
                    myResponse.Close();
                }
            }

            return content;
        }

        /// <summary>
        /// Build the url to the 3rd gateway
        /// </summary>
        /// <param name="dynamicKeyValue">the parameter key-value from ACA</param>
        /// <returns>the url string to 3rd gateway</returns>
        public static string BuildOnlinePaymentURL(Hashtable dynamicKeyValue)
        {
            // Get HostURL from config file
            string hostURL = PaymentUtil.GetConfig("HostURL");
            string parameters = PaymentUtil.GetConfig("RedirectURLParameters");

            if (String.IsNullOrEmpty(parameters) || String.IsNullOrEmpty(hostURL))
            {
                throw new Exception("Can not find HostURL or RedirectURLParameters in Adapter.config file, be sure you have configured them.");
            }

            StringBuilder parameterBuilder = new StringBuilder(parameters);

            // The key must format to {KeyName} pattern
            foreach (DictionaryEntry pair in dynamicKeyValue)
            {
                parameterBuilder.Replace(pair.Key.ToString(), pair.Value != null ? HttpUtility.UrlEncode(pair.Value.ToString()): string.Empty);
            }

            string param = parameterBuilder.ToString().Replace('|', '&');

            return String.Format("{0}?{1}", hostURL, param);
        }       

        /// <summary>
        /// Set data to cache
        /// </summary>
        /// <typeparam name="T">the data type</typeparam>
        /// <param name="key">the cache key</param>
        /// <param name="value">data to be save</param>
        /// <param name="minutes">the cache time</param>
        public static void SetDataToCache<T>(string key, T value, int minutes)
        {
            if (GetDataFromCache<T>(key) == null)
            {
                HttpRuntime.Cache[key] = value;
            }
            else
            {
                // set max expired time to 30 minutes
                int expirationTime = minutes;
                HttpRuntime.Cache.Insert(key, value, null, DateTime.Now.AddMinutes(expirationTime), TimeSpan.Zero, CacheItemPriority.Normal, null);
            }
        }

        /// <summary>
        /// Get data from cache
        /// </summary>
        /// <typeparam name="T">the data type</typeparam>
        /// <param name="key">the cache key</param>
        /// <returns>the value from cache</returns>
        public static T GetDataFromCache<T>(string key)
        {
            return (T)HttpContext.Current.Cache.Get(key);
        }

        /// <summary>
        /// When error occur, redirect to ACA.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="returnCode">The return code.</param>
        public static void HandleErrorRedirect(string message, string returnCode)
        {
            HandleErrorRedirect(message, returnCode, string.Empty);
        }

        /// <summary>
        /// When error occur, redirect to ACA.
        /// </summary>
        /// <param name="message">error message</param>
        /// <param name="urlParameters">the url paramters, for example: ACA transaction id.</param>
        /// <param name="returnCode">return code</param>
        public static void HandleErrorRedirect(string message, string returnCode, string urlParameters)
        {
            if (String.IsNullOrEmpty(returnCode))
            {
                returnCode = PaymentConstant.FAILURE_CODE;
            }

            string redirectURL = PaymentHelper.GetDataFromCache<string>(PaymentConstant.REDIRECT_URL);
            
            if (!String.IsNullOrEmpty(redirectURL))
            {
                string url = String.Format(
                                                  "{0}?{1}={2}&{3}={4}{5}",
                                                  redirectURL,
                                                  PaymentConstant.RETURN_CODE,
                                                  returnCode,
                                                  PaymentConstant.USER_MESSAGE,
                                                  HttpUtility.UrlEncode(message),
                                                  urlParameters);

                HttpContext.Current.Response.Redirect(url);
            }
            else
            {
                HttpContext.Current.Response.Write("Payment failed.");
            }
        }

        /// <summary>
        /// get the third party static value by the key
        /// </summary>
        /// <param name="key">the key configed in the ThirdPartyStaticParameters node in Adapter.config</param>
        /// <returns>return the value by the key</returns>
        public static string GetThirdPartyStaticValue(string agencyCode, string key)
        {
            string configNode = "ThirdPartyStaticParameters";
            if (!String.IsNullOrEmpty(agencyCode))
            {
                configNode += "_" + agencyCode;
            }

            string parameters = PaymentUtil.GetConfig(configNode);

            if (String.IsNullOrEmpty(parameters))
            {
                throw new Exception(String.Format("Can not find {0} in Adapter.config file, be sure you have configured them.", configNode));
            }

            return GetValueFromParameters(parameters, key);
        }

        #region Private Methods

        /// <summary>
        /// Update the transaction field if new value is not empty
        /// </summary>
        /// <param name="newValue">the value used to update the transaction</param>
        /// <param name="originValue">the origin value</param>
        /// <returns>the value to be updated</returns>
        private static string UpdateTransactionField(object newValue, string originValue)
        {
            string value = PaymentUtil.ParseObjectToString(newValue);
            return String.IsNullOrEmpty(value) ? originValue : value;
        }

        /// <summary>
        /// Get value from parameters, the parameters format as: 'key1=value1|key2=value2'
        /// </summary>
        /// <param name="parameters">the parameter string, format: 'key1=value1|key2=value2'</param>
        /// <param name="key">the key which want to find value by</param>
        /// <returns>return to the value by the key</returns>
        private static string GetValueFromParameters(string parameters, string key)
        {
            string[] keyValues = parameters.Split('|');
            string value = String.Empty;

            foreach (string keyValue in keyValues)
            {
                string[] apps = keyValue.Split('=');

                if (apps.Length == 2 && key.ToLower() == apps[0].ToLower())
                {
                    value = apps[1];
                    break;
                }
            }

            return value;
        }        

        #endregion
    }
}
