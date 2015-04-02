#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: PaymentHelper.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2009-2014
 *
 *  Description:
 *
 *  Notes:
 * $Id: PaymentHelper.cs 277174 2014-08-12 07:38:31Z ACHIEVO\james.shi $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Web;
using System.Web.UI;

using Accela.ACA.BLL.Cap;
using Accela.ACA.BLL.Common;
using Accela.ACA.BLL.Finance;
using Accela.ACA.BLL.People;
using Accela.ACA.Common;
using Accela.ACA.Common.Common;
using Accela.ACA.Common.Log;
using Accela.ACA.Common.Util;
using Accela.ACA.Web.Common;
using Accela.ACA.WSProxy;
using log4net;
using Newtonsoft.Json;

namespace Accela.ACA.Web.Payment
{
    /// <summary>
    /// This class provide the payment helper.
    /// </summary>
    public class PaymentHelper
    {
        #region Fields

        /// <summary>
        /// The AA application id
        /// </summary>
        private const string APPLICATION_ID = "ApplicationID";

        /// <summary>
        /// ETISALAT payment adapter
        /// </summary>
        private const string ETISALAT = "Etisalat";

        /// <summary>
        /// GOVOLUTION payment adapter
        /// </summary>
        private const string GOVOLUTION = "Govolution";

        /// <summary>
        /// OPCoBrandPlus payment adapter
        /// </summary>
        private const string OPCOBRANDPLUS = "OPCoBrandPlus";

        /// <summary>
        /// OPSTP payment adapter
        /// </summary>
        private const string OPSTP = "OPSTP";

        /// <summary>
        /// First Data payment adapter
        /// </summary>
        private const string FIRSTDATA = "FirstData";

        #endregion Fields

        #region Fields for Adapter

        /// <summary>
        /// the adapter web site entrance url
        /// </summary>
        private const string XPOLICY_HOST_URL = "HostURL";

        /// <summary>
        /// the application id for adapter web site
        /// </summary>
        private const string XPOLICY_APPLICATION_ID = "ApplicationID";

        /// <summary>
        /// Addition info agency id.
        /// </summary>
        private const string ADDITIONINFO_KEY_AGENCYID = "AgencyID";

        /// <summary>
        /// Addition info Module Name.
        /// </summary>
        private const string ADDITIONINFO_KEY_MODULENAME = "ModuleName";

        /// <summary>
        /// Addition info User id.
        /// </summary>
        private const string ADDITIONINFO_KEY_USERID = "UserID";

        /// <summary>
        /// Addition info User group id.
        /// </summary>
        private const string ADDITIONINFO_KEY_USERGROUPID = "UserGroupID";

        /// <summary>
        /// Addition info Payment Comment.
        /// </summary>
        private const string ADDITIONINFO_KEY_PAYMENTCOMMENT = "PaymentComment";

        #endregion

        /// <summary>
        /// Create an instance of ILog
        /// </summary>
        private static readonly ILog Logger = LogFactory.Instance.GetLogger(typeof(PaymentHelper));

        #region Public Static Methods

        /// <summary>
        /// convert to dictionary.
        /// </summary>
        /// <param name="queryString">query string</param>
        /// <returns>The dictionary data type.</returns>
        public static Dictionary<string, string> ConvertToDictionary(string queryString)
        {
            Dictionary<string, string> result = new Dictionary<string, string>(StringComparer.InvariantCultureIgnoreCase);
            if (!string.IsNullOrEmpty(queryString))
            {
                if (queryString.StartsWith("?"))
                {
                    queryString = queryString.Substring(1);
                }

                HttpRequest httpRequest = GetHttpRequest(queryString);
                if (httpRequest != null)
                {
                    foreach (string key in httpRequest.QueryString.Keys)
                    {
                        result.Add(key, HttpContext.Current.Server.UrlDecode(httpRequest.QueryString[key]));
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// Gets payment adapter type
        /// </summary>
        /// <returns>The payment adapter type.</returns>
        public static PaymentAdapterType GetPaymentAdapterType()
        {
            string adapterType = StandardChoiceUtil.GetEPaymentAdapterType();

            if (string.IsNullOrEmpty(adapterType))
            {
                return PaymentAdapterType.Unknown;
            }

            PaymentAdapterType paymentAdapter;

            if (adapterType.IndexOf(GOVOLUTION, StringComparison.InvariantCultureIgnoreCase) > -1)
            {
                paymentAdapter = PaymentAdapterType.Govolution;
            }
            else if (adapterType.IndexOf(FIRSTDATA, StringComparison.InvariantCultureIgnoreCase) > -1)
            {
                paymentAdapter = PaymentAdapterType.FirstData;
            }
            else if (adapterType.IndexOf(ETISALAT, StringComparison.InvariantCultureIgnoreCase) > -1)
            {
                paymentAdapter = PaymentAdapterType.Etisalat;
            }
            else if (adapterType.IndexOf(OPCOBRANDPLUS, StringComparison.InvariantCultureIgnoreCase) > -1)
            {
                paymentAdapter = PaymentAdapterType.CoBrandPlus;
            }
            else if (adapterType.IndexOf(OPSTP, StringComparison.InvariantCultureIgnoreCase) > -1)
            {
                paymentAdapter = PaymentAdapterType.APIPayment;
            }
            else if (EPaymentConfig.IsPaymentMethodUseRedirect())
            {
                paymentAdapter = PaymentAdapterType.ExternalAdapter;
            }
            else
            {
                paymentAdapter = PaymentAdapterType.Unknown;
            }

            return paymentAdapter;
        }

        /// <summary>
        /// Is the payment adapter can be redirected to third party payment provider.
        /// </summary>
        /// <returns>Indicating whether redirect to payment provider or not.</returns>
        public static bool IsRedirectToPaymentProvider()
        {
            PaymentAdapterType paymentAdapterType = GetPaymentAdapterType();

            return paymentAdapterType == PaymentAdapterType.CoBrandPlus
                    || paymentAdapterType == PaymentAdapterType.Etisalat
                    || paymentAdapterType == PaymentAdapterType.Govolution
                    || paymentAdapterType == PaymentAdapterType.FirstData
                    || paymentAdapterType == PaymentAdapterType.ExternalAdapter;
        }

        /// <summary>
        /// convert to query string
        /// </summary>
        /// <param name="dicSource">The data source.</param>
        /// <returns>query string</returns>
        public static string ConvertToQueryString(Dictionary<string, string> dicSource)
        {
            StringBuilder sb = new StringBuilder();
            if (dicSource != null)
            {
                foreach (string key in dicSource.Keys)
                {
                    if (sb.Length > 0)
                    {
                        sb.Append("&");
                    }

                    sb.Append(string.Format("{0}={1}", key, HttpContext.Current.Server.UrlEncode(dicSource[key])));
                }
            }

            return sb.ToString();
        }

        /// <summary>
        /// extract value from savedHttpRequest.
        /// </summary>
        /// <param name="savedHttpRequest">The saved http request.</param>
        /// <param name="key">The key.</param>
        /// <returns>The value.</returns>
        public static string ExtractValue(HttpRequest savedHttpRequest, string key)
        {
            if (savedHttpRequest != null && savedHttpRequest.QueryString != null &&
                savedHttpRequest.QueryString[key] != null)
            {
                return savedHttpRequest[key].ToString();
            }
            else
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// Filter AAException, get rid of some redundant message.
        /// </summary>
        /// <param name="originalMessage">The original message.</param>
        /// <returns>The message that filtered AA exception.</returns>
        public static string FilterAAException(string originalMessage)
        {
            if (string.IsNullOrEmpty(originalMessage))
            {
                return string.Empty;
            }
            else
            {
                return originalMessage.Replace("com.accela.aa.exception.AAException: ", string.Empty);
            }
        }

        /// <summary>
        /// Get the adapter name from standard choice
        /// </summary>
        /// <returns>The adapter name.</returns>
        public static string GetAdapterName()
        {
            string adapterName = StandardChoiceUtil.GetEPaymentAdapterType();

            if (string.IsNullOrEmpty(adapterName))
            {
                return string.Empty;
            }

            int index = adapterName.IndexOf("_");

            if (index >= 0)
            {
                adapterName = adapterName.Substring(0, index);
            }

            return adapterName;
        }

        /// <summary>
        /// Get data from cache.
        /// </summary>
        /// <typeparam name="T">Generic Type.</typeparam>
        /// <param name="id">The cache key.</param>
        /// <returns>The data that from cache.</returns>
        public static T GetDataFromCache<T>(string id)
        {
            Logger.InfoFormat("get data from Cache,id={0}", id);
            ICacheManager cacheManager = (ICacheManager)ObjectFactory.GetObject(typeof(ICacheManager));
            return (T)cacheManager.GetSingleCachedItem(id);
        }

        /// <summary>
        /// get httpRequest based on queryString.
        /// </summary>
        /// <param name="queryString">The query string.</param>
        /// <returns>The http request.</returns>
        public static HttpRequest GetHttpRequest(string queryString)
        {
            HttpRequest myHttpRequest = null;
            try
            {
                myHttpRequest = new HttpRequest("a", "http://local/", queryString);
            }
            catch (Exception ex)
            {
                Logger.Error(ex.Message, ex);
            }

            return myHttpRequest;
        }

        /// <summary>
        /// get httpRequest based on queryString.
        /// </summary>
        /// <param name="isQualifiedURL">is qualified url or not.</param>
        /// <param name="url">The url.</param>
        /// <returns>The http request.</returns>
        public static HttpRequest GetHttpRequest(bool isQualifiedURL, string url)
        {
            string queryString = string.Empty;
            if (isQualifiedURL && !string.IsNullOrEmpty(url))
            {
                try
                {
                    Uri uri = new Uri(url);
                    queryString = uri.Query.Substring(1);
                }
                catch (Exception ex)
                {
                    Logger.Error(ex.Message, ex);
                }
            }

            return GetHttpRequest(queryString);
        }

        /// <summary>
        /// get post back data.
        /// </summary>
        /// <returns>The post back data.</returns>
        public static string GetPostbackDataString()
        {
            StringBuilder sb = new StringBuilder();

            sb.Append("Request.Form:\r\n");
            foreach (string key in HttpContext.Current.Request.Form.AllKeys)
            {
                sb.Append(string.Format("{0}={1}\n", key, HttpContext.Current.Request.Form[key]));
            }

            sb.Append("\r\nRequest.QueryString:\r\n");
            foreach (string key in HttpContext.Current.Request.QueryString.AllKeys)
            {
                sb.Append(string.Format("{0}={1}\n", key, HttpContext.Current.Request.QueryString[key]));
            }

            return sb.ToString();
        }

        /// <summary>
        /// get post back form data with query string style.
        /// </summary>
        /// <returns>The post back data.</returns>
        public static string GetPostbackFormData()
        {
            StringBuilder sb = new StringBuilder();
            foreach (string key in HttpContext.Current.Request.Form.AllKeys)
            {
                if (sb.Length > 0)
                {
                    sb.Append("&");
                }

                string value = HttpContext.Current.Server.UrlEncode(HttpContext.Current.Request.Form[key]);
                sb.Append(string.Format("{0}={1}", key, value));
            }

            return sb.ToString();
        }

        /// <summary>
        /// get query string 
        /// </summary>
        /// <param name="url">the url</param>
        /// <returns>query string</returns>
        public static string GetQueryString(string url)
        {
            string queryString = string.Empty;
            try
            {
                Uri uri = new Uri(url);
                queryString = uri.Query.Substring(1);
            }
            catch (Exception ex)
            {
                Logger.Error(ex.Message, ex);
            }

            return queryString;
        }

        /// <summary>
        /// Log details for dictionary.
        /// </summary>
        /// <param name="dicSource">The dictionary source.</param>
        public static void LogDetailsOfDictionary(Dictionary<string, string> dicSource)
        {
            if (dicSource != null)
            {
                StringBuilder sb = new StringBuilder();
                foreach (string tempKey in dicSource.Keys)
                {
                    sb.AppendFormat("key={0}, value={1}\t", tempKey, dicSource[tempKey]);
                }

                sb.Append("\n");
                Logger.Info(sb.ToString());
            }
            else
            {
                Logger.Info("No results in current Dictionary.");
            }
        }

        /// <summary>
        /// save data to cache.
        /// </summary>
        /// <typeparam name="T">Generic Type.</typeparam>
        /// <param name="id">The cache key.</param>
        /// <param name="data">The data</param>
        public static void SaveDataToCache<T>(string id, T data)
        {
            Logger.InfoFormat("Save data to Cache,id=", id);
            ICacheManager cacheManager = (ICacheManager)ObjectFactory.GetObject(typeof(ICacheManager));
            cacheManager.AddSingleItemToCache(id, data, ACAConstant.PAYMENT_STAUTS_CACHE_EXPIRE_TIME);
        }

        /// <summary>
        /// save or update data to cache
        /// </summary>
        /// <typeparam name="T">Generic Type</typeparam>
        /// <param name="id">the cache key</param>
        /// <param name="data">The data</param>
        public static void SaveOrUpdateDataToCache<T>(string id, T data)
        {
            Logger.InfoFormat("Update data to Cache,id={0}", id);
            ICacheManager cacheManager = (ICacheManager)ObjectFactory.GetObject(typeof(ICacheManager));
            object item = cacheManager.GetSingleCachedItem(id);

            if (item == null)
            {
                Logger.ErrorFormat("Can't get the correct information from cache['{0}'], add current data to cache.", id);
                cacheManager.AddSingleItemToCache(id, data, ACAConstant.PAYMENT_STAUTS_CACHE_EXPIRE_TIME);
            }
            else
            {
                cacheManager.UpdateSingleCacheItem(id, data);
            }
        }

        /// <summary>
        /// build AdditionInfo for AA biz validation before online payment.
        /// </summary>
        /// <param name="agencyCode">agency code.</param>
        /// <param name="moduleName">module name,if there is no module name,pass it with string.Empty.</param>
        /// <param name="userID">public user id</param>
        /// <param name="userGroupID">user group id</param>
        /// <param name="paymentComment">payment comment</param>
        /// <returns>AdditionInfo array</returns>
        public static AdditionInfo[] BuildAdditionInfoForPayment(string agencyCode, string moduleName, string userID, string userGroupID, string paymentComment)
        {
            AdditionInfo[] additionInfos = new AdditionInfo[5];
            additionInfos[0] = new AdditionInfo();
            additionInfos[0].name = ADDITIONINFO_KEY_AGENCYID;
            additionInfos[0].value = agencyCode;

            additionInfos[1] = new AdditionInfo();
            additionInfos[1].name = ADDITIONINFO_KEY_MODULENAME;
            additionInfos[1].value = moduleName;

            additionInfos[2] = new AdditionInfo();
            additionInfos[2].name = ADDITIONINFO_KEY_USERID;
            additionInfos[2].value = userID;

            additionInfos[3] = new AdditionInfo();
            additionInfos[3].name = ADDITIONINFO_KEY_USERGROUPID;
            additionInfos[3].value = userGroupID;

            additionInfos[4] = new AdditionInfo();
            additionInfos[4].name = ADDITIONINFO_KEY_PAYMENTCOMMENT;
            additionInfos[4].value = paymentComment;

            return additionInfos;
        }

        /// <summary>
        /// Determines whether is valid per agency payment.
        /// </summary>
        /// <param name="capIDModels">The cap ID models.</param>
        /// <param name="totalFee">The total fee.</param>
        /// <param name="alertMessageLabelKey">The alert message label key.</param>
        /// <returns>
        /// <c>true</c> if is valid per agency payment; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsValidPerAgencyPayment(CapIDModel4WS[] capIDModels, double totalFee, out string alertMessageLabelKey)
        {
            bool result = true;
            alertMessageLabelKey = string.Empty;
            PaymentAdapterType paymentAdapterType = GetPaymentAdapterType();

            if (paymentAdapterType == PaymentAdapterType.ExternalAdapter && totalFee > 0)
            {
                bool isPaid4MultiRecords = IsPaid4MultiRecords(capIDModels);
                bool isShoppingCartEnabled = StandardChoiceUtil.IsEnableShoppingCart();

                if (isShoppingCartEnabled)
                {
                    ShoppingCartTransactionType transactionType = ShoppingCartUtil.GetShoppingCartTransactionType();

                    if (transactionType == ShoppingCartTransactionType.TransactionPerRecord && isPaid4MultiRecords)
                    {
                        alertMessageLabelKey = "aca_payment_message_onlyonerecord";
                        result = false;
                    }
                }
                else if (isPaid4MultiRecords)
                {
                    alertMessageLabelKey = "aca_payment_message_onlyonerecord_forcartdisabled";
                    result = false;
                }
            }

            return result;
        }

        /// <summary>
        /// Get payment logs
        /// </summary>
        /// <param name="aaTransId">AA transaction id</param>
        /// <param name="applicationId">application id</param>
        /// <param name="processName">The process name.</param>
        /// <returns>the transaction logs</returns>
        public static OnlinePaymentAudiTrailModel[] GetPaymentGatewayLogs(string aaTransId, string applicationId, string processName)
        {
            OnlinePaymentAudiTrailModel logModel = new OnlinePaymentAudiTrailModel();
            logModel.aaTransId = aaTransId;
            logModel.applicationId = applicationId;
            logModel.processName = processName;

            IOnlinePaymentAuditTrailBll onlinePaymenAuditTrailBll = ObjectFactory.GetObject<IOnlinePaymentAuditTrailBll>();
            OnlinePaymentAudiTrailModel[] result = onlinePaymenAuditTrailBll.GetAudiTrailModels(logModel);

            return result;
        }

        /// <summary>
        /// get Application id
        /// </summary>
        /// <returns>The application id.</returns>
        public static string GetApplicationID()
        {
            string applicationID = string.Empty;
            Hashtable xpolicyItems = EPaymentConfig.GetConfig();

            if (xpolicyItems != null && xpolicyItems.ContainsKey(APPLICATION_ID))
            {
                applicationID = Convert.ToString(xpolicyItems[APPLICATION_ID]);
            }

            return applicationID;
        }

        #endregion Methods

        #region Methods for Adapter

        /// <summary>
        /// convert input data to string, if the object is null, return string.Empty
        /// </summary>
        /// <param name="data">the source data</param>
        /// <returns>the string value</returns>
        public static string ConvertToString(object data)
        {
            if (data == null)
            {
                return string.Empty;
            }

            return data.ToString();
        }

        /// <summary>
        /// Combine  ACA parameters and redirect to adapter web site
        /// </summary>
        /// <param name="currentPage">The current page instance</param>
        public static void InitiatePayment(Page currentPage)
        {
            Logger.Debug("begin adapter payment, the parameters:");

            IPeopleBll peopleBll = ObjectFactory.GetObject<IPeopleBll>();
            string fullName = peopleBll.GetContactUserName(AppSession.User.UserModel4WS);
            fullName = ScriptFilter.AntiXssUrlEncode(fullName);

            if (string.IsNullOrEmpty(fullName))
            {
                MessageUtil.ShowMessage(currentPage, MessageType.Error, LabelUtil.GetTextByKey("aca_payment_msg_usernamerequired_error", string.Empty));
                return;
            }

            // get parameters and convert to PaymentEntity
            PaymentEntity param = BuildPaymentParameters(currentPage);

            // call InitialPayment method by web service
            TransactionModel[] transactionModels = CallInitialPaymentWS(param);

            Logger.Debug("begin to handle results.");

            if (transactionModels != null && transactionModels.Length > 0)
            {
                /* The transactionModels[] contains multiple agencies' payment information.
               * One agency contain one record if not exists convenience fee,
               * else contain two records, one for payment amount, another one for convenience fee.
               */
                string transactionID = GetTransactionId(transactionModels);

                Logger.InfoFormat("the batch transaction number is {0}", transactionID);

                // save parameters to cache which will be reused when payment completed
                SaveDataToCache(transactionID, param);

                double permitFee = 0.0;
                double convFee = 0.0;
                CalculateFee(transactionModels, param.EntityType, out permitFee, out convFee);

                // build the adapter entrance url
                string url = BuildAdapterBeginURL(
                    transactionID,
                    permitFee.ToString(CultureInfo.InvariantCulture.NumberFormat),
                    convFee.ToString(CultureInfo.InvariantCulture.NumberFormat),
                    fullName,
                    param);

                CreateOnlinePaymenAudiTrail(transactionID, param);

                Logger.Info("begin to redirect to the adapter web site.");

                string pbMsg = LabelUtil.GetTextByKey("aca_payment_redirection_text", string.Empty);
                pbMsg = Accela.ACA.Common.Common.ScriptFilter.EncodeJson(pbMsg);

                string strScript = string.Format("<script>document.body.innerHTML = '<span class=\"font13px\">{0}</span>';NeedAsk = false; window.open('{1}','_parent');</script>", pbMsg, url);
                Page curPage = (Page)HttpContext.Current.Handler;

                if (curPage != null)
                {
                    curPage.RegisterClientScriptBlock("promptInfo", strScript);
                }
            }
            else
            {
                Logger.Error("no result returned from InitiatePayment method.");
                MessageUtil.ShowMessage(currentPage, MessageType.Error, "Online payment failed");
            }
        }

        /// <summary>
        /// Store payment result into online payment audit log table
        /// </summary>
        /// <param name="transId">Transaction ID</param>
        /// <param name="param">Payment Entity Model</param>
        public static void CreateOnlinePaymenAudiTrail(string transId, PaymentEntity param)
        {
            OnlinePaymentAudiTrailModel logModel = new OnlinePaymentAudiTrailModel();
            logModel.applicationId = GetApplicationID();
            logModel.aaTransId = transId;
            logModel.processName = "00.ACA Online Payment Result";
            logModel.processContent = JsonConvert.SerializeObject(param);

            logModel.recDate = DateTime.Now;
            logModel.recFulNam = param.PublicUserID;

            IOnlinePaymentAuditTrailBll onlinePaymenAuditTrailBll = ObjectFactory.GetObject<IOnlinePaymentAuditTrailBll>();
            onlinePaymenAuditTrailBll.CreateAudiTrail(logModel);
        }

        /// <summary>
        /// Get PaymentEntity Model from OnlinePaymentAuditLog
        /// </summary>
        /// <param name="transId">The transaction id.</param>
        /// <returns>The payment entity</returns>
        public static PaymentEntity GetPaymentEntityFromOnlinePaymentAuditLog(string transId)
        {
            OnlinePaymentAudiTrailModel onlinePaymentAuditTrail = null;
            PaymentEntity paymentEntity = null;

            string applicationId = GetApplicationID();

            OnlinePaymentAudiTrailModel[] onlinePaymentAuditTrails = GetPaymentGatewayLogs(transId, applicationId, "00.ACA Online Payment Result");

            if (onlinePaymentAuditTrails != null && onlinePaymentAuditTrails.Length > 0)
            {
                onlinePaymentAuditTrail = onlinePaymentAuditTrails[0];
            }

            if (onlinePaymentAuditTrail != null && !string.IsNullOrEmpty(onlinePaymentAuditTrail.processContent))
            {
                paymentEntity = (PaymentEntity)JsonConvert.DeserializeObject(onlinePaymentAuditTrail.processContent, typeof(PaymentEntity));
            }

            return paymentEntity;
        }

        /// <summary>
        /// Get the merchant account model.
        /// </summary>
        /// <returns>Return the merchant account model.</returns>
        public static List<RefMerchantAccountModel> GetMerchantAccount()
        {
            PaymentAdapterType paymentAdapterType = GetPaymentAdapterType();

            // the non [external redirect payment mode] will not use the Merchant Account Config.
            if (paymentAdapterType != PaymentAdapterType.ExternalAdapter)
            {
                return null;
            }

            IOnlinePaymenBll onlinePaymentBll = ObjectFactory.GetObject<IOnlinePaymenBll>();
            List<RefMerchantAccountModel> accountModelList = new List<RefMerchantAccountModel>();
            CapIDModel4WS[] capIDModels = AppSession.GetCapIDModelsFromSession();
            CapTypeModel capType4GetAccount = null;

            if (capIDModels != null && capIDModels.Length > 0)
            {
                List<string> agencyList = new List<string>();

                foreach (CapIDModel4WS capIDModel in capIDModels)
                {
                    string agencyCode = capIDModel.serviceProviderCode;

                    // cross agency payment may config more fee formula.
                    if (!string.IsNullOrEmpty(agencyCode) && !agencyList.Contains(agencyCode))
                    {
                        ICapTypeBll capTypeBll = ObjectFactory.GetObject<ICapTypeBll>();
                        capType4GetAccount = capTypeBll.GetCapTypeByCapID(capIDModel);

                        RefMerchantAccountModel accountModel = onlinePaymentBll.SearchMerchantAccount(capType4GetAccount, false);
                        accountModelList.Add(accountModel);

                        agencyList.Add(agencyCode);
                    }
                }
            }
            else
            {
                capType4GetAccount = new CapTypeModel();
                capType4GetAccount.serviceProviderCode = ConfigManager.AgencyCode;

                RefMerchantAccountModel accountModel = onlinePaymentBll.SearchMerchantAccount(capType4GetAccount, false);
                accountModelList.Add(accountModel);
            }

            return accountModelList;
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// build the adapter entrance url
        /// </summary>
        /// <param name="transactionID">the AA transaction id</param>
        /// <param name="paymentAmount">payment amount</param>
        /// <param name="convFee">convenience fee</param>
        /// <param name="fullName">full name</param>
        /// <param name="param">the parameter</param>
        /// <returns>the url to adapter web site</returns>
        private static string BuildAdapterBeginURL(string transactionID, string paymentAmount, string convFee, string fullName, PaymentEntity param)
        {
            string userEmail = AppSession.User.Email;
            string lang = I18nCultureUtil.UserPreferredCulture;
            string userID = param.PublicUserID;
            string agencyCode = param.ServProvCode;
            string paymentMethod = param.PaymentMethod == PaymentMethod.Check ? PaymentConstant.PAYMENT_TYPE_CHECK : PaymentConstant.PAYMENT_TYPE_CREDITCARD;

            // Get part of the parameters from xpolicy
            Hashtable xpolicyItems = EPaymentConfig.GetConfig();
            string hostURL = GetPolicyItemByKey(xpolicyItems, XPOLICY_HOST_URL);
            string applicationID = GetPolicyItemByKey(xpolicyItems, XPOLICY_APPLICATION_ID);

            // The URL that adapter sends the payment result to ACA so that ACA continues to complete CAP handle process
            string postbackURL = GetDomainUrl("/payment/paymentpostback.aspx");

            // The URL to which the user is redirected for showing payment result page when payment is completed
            string redirectURL = GetDomainUrl("/payment/paymentredirect.aspx");

            string url = string.Format(
                "{0}?{1}={2}&{3}={4}&{5}={6}&{7}={8}&{9}={10}&{11}={12}&{13}={14}&{15}={16}&{17}={18}&{19}={20}&{21}={22}&{23}={24}&{25}={26}",
                hostURL,
                PaymentConstant.ACA2ADAPTER_APPLICATION_ID,
                applicationID,
                PaymentConstant.ACA2ADAPTER_TRANSACTION_ID,
                transactionID,
                PaymentConstant.ACA2ADAPTER_PAYMENT_AMOUNT,
                paymentAmount,
                PaymentConstant.ACA2ADAPTER_CONVENIENCE_FEE,
                convFee,
                PaymentConstant.ACA2ADAPTER_POSTBACK_URL,
                HttpUtility.UrlEncode(postbackURL),
                PaymentConstant.ACA2ADAPTER_REDIRECT_URL,
                HttpUtility.UrlEncode(redirectURL),
                PaymentConstant.ACA2ADAPTER_USER_ID,
                HttpUtility.UrlEncode(userID),
                PaymentConstant.ACA2ADAPTER_FULL_NAME,
                HttpUtility.UrlEncode(fullName),
                PaymentConstant.ACA2ADAPTER_USER_EMAIL,
                HttpUtility.UrlEncode(userEmail),
                PaymentConstant.ACA2ADAPTER_AGENCY_CODE,
                agencyCode,
                PaymentConstant.ACA2ADAPTER_PAYMENT_METHOD,
                paymentMethod,
                PaymentConstant.ACA2ADAPTER_MERCHANT_ACCOUNTID,
                param.MerchantAccountID,
                PaymentConstant.ACA2ADAPTER_LANG_FLAG,
                lang);

            // append the customized url param if have set
            string customizedUrlParam = GetPolicyItemByKey(xpolicyItems, PaymentConstant.CUSTOMIZED_URL_PARAMETER);

            if (!string.IsNullOrEmpty(customizedUrlParam))
            {
                url = string.Format("{0}&{1}", url, customizedUrlParam);
            }

            Logger.InfoFormat("The url to Adapter is: {0}", url);
            return url;
        }

        /// <summary>
        /// initialize online payment
        /// </summary>
        /// <param name="paymentEntity">the payment entity</param>
        /// <returns>list of the TransactionModel</returns>
        private static TransactionModel[] CallInitialPaymentWS(PaymentEntity paymentEntity)
        {
            Logger.Info("Begin to invoke commonPaymentBll.InitialOnlinePaymnet() method.");
            string adapterName = GetAdapterName();
            string paymentMethod = paymentEntity.PaymentMethod == PaymentMethod.Check ? ACAConstant.PAY_METHOD_CHECK : ACAConstant.PAY_METHOD_CREDIT_CARD;
            string merchantAccountID = paymentEntity.MerchantAccountID != 0 ? paymentEntity.MerchantAccountID.ToString() : string.Empty;

            string parameters = string.Format("{0}={1}", ACAConstant.ServProvCode_Key, ConfigManager.AgencyCode);
            parameters += string.Format(
                                    CultureInfo.InvariantCulture,
                                    "&EntityType={0}&EntityID={1}&totalFee={2}&PaymentMethod={3}&MerchantAccountID={4}",
                                    paymentEntity.EntityType.ToString(),
                                    HttpUtility.UrlEncode(paymentEntity.EntityID),
                                    paymentEntity.TotalFee,
                                    paymentMethod,
                                    merchantAccountID);

            string logPattern = "The parameters: ServiceProviderCode={0}, AdapterName={1}, EntityType={2}, EntityID={3}, TotalFee={4}, PaymentMethod={5}, MerchantAccountID={6}, CallerID={7}";
            string logMessage = string.Format(
                                            logPattern,
                                            ConfigManager.AgencyCode,
                                            adapterName,
                                            paymentEntity.EntityType.ToString(),
                                            paymentEntity.EntityID,
                                            paymentEntity.TotalFee,
                                            paymentMethod,
                                            merchantAccountID,
                                            paymentEntity.PublicUserID);

            Logger.Info(logMessage);

            ICommonPaymentBll commonPaymentBll = (ICommonPaymentBll)ObjectFactory.GetObject(typeof(ICommonPaymentBll));
            TransactionModel[] transactionModels = commonPaymentBll.InitialOnlinePayment(TempModelConvert.Trim4WSOfCapIDModels(paymentEntity.CapIDs), parameters, adapterName);
            return transactionModels;
        }

        /// <summary>
        /// The TransactionModel list which have the same transaction id, so it only need get the first one.
        /// </summary>
        /// <param name="transactionModels">the TransactionModel list, it may have multiply agencies' records. One agency may have 2 records, one for payment amount, another for convenience fee.</param>
        /// <returns>the transaction ID</returns>
        private static string GetTransactionId(TransactionModel[] transactionModels)
        {
            if (transactionModels != null && transactionModels.Length > 0)
            {
                foreach (TransactionModel transModel in transactionModels)
                {
                    if (transModel != null)
                    {
                        return transModel.batchTransCode == null ? string.Empty : transModel.batchTransCode.ToString();
                    }
                }
            }

            return string.Empty;
        }

        /// <summary>
        /// Calculate the permit fee and convenience fee.
        /// </summary>
        /// <param name="transactionModels">The transaction model.</param>
        /// <param name="entityType">The payment entity type: CAP/Trust Account</param>
        /// <param name="permitFee">The permit fee</param>
        /// <param name="convFee">The convenience fee</param>
        private static void CalculateFee(TransactionModel[] transactionModels, ACAConstant.PaymentEntityType entityType, out double permitFee, out double convFee)
        {
            permitFee = 0.0;
            convFee = 0.0;

            if (transactionModels == null || transactionModels.Length == 0)
            {
                return;
            }

            // 1. loop to calculate the permit fee and convenience fee.
            foreach (TransactionModel transModel in transactionModels)
            {
                if (transModel == null || transModel.totalFee == null)
                {
                    continue;
                }

                // 1.1 calculate the permit fee/trust account fee.
                switch (entityType)
                {
                    case ACAConstant.PaymentEntityType.CAP:
                        if (ACAConstant.FeeType.Permit.ToString().Equals(transModel.feeType, StringComparison.InvariantCultureIgnoreCase))
                        {
                            permitFee += transModel.totalFee.Value;
                        }

                        break;
                    case ACAConstant.PaymentEntityType.TrustAccount:
                        if (ACAConstant.FeeType.TrustAccount.ToString().Equals(transModel.feeType, StringComparison.InvariantCultureIgnoreCase))
                        {
                            permitFee += transModel.totalFee.Value;
                        }

                        break;
                }

                // 1.2 calculate the convenience fee.
                if (ACAConstant.FeeType.PROCESSING_FEE.ToString().Equals(transModel.feeType, StringComparison.InvariantCultureIgnoreCase))
                {
                    convFee += transModel.totalFee.Value;
                }
            }
        }

        /// <summary>
        /// get policy item for adapter payment
        /// </summary>
        /// <param name="policyItems">all items in column data1 - data4</param>
        /// <param name="key">the item key</param>
        /// <returns>the item value from data1 - data4</returns>
        private static string GetPolicyItemByKey(Hashtable policyItems, string key)
        {
            string item = string.Empty;

            if (policyItems != null && policyItems.ContainsKey(key))
            {
                item = Convert.ToString(policyItems[key]);
            }

            return item;
        }

        /// <summary>
        /// get domain url
        /// </summary>
        /// <param name="pagePathName">the target page</param>
        /// <returns>the whole url</returns>
        private static string GetDomainUrl(string pagePathName)
        {
            string host = HttpContext.Current.Request.Url.Host;
            string port = HttpContext.Current.Request.Url.Port.ToString();
            string http = HttpContext.Current.Request.Url.Scheme;
            string appPath = HttpContext.Current.Request.ApplicationPath.TrimEnd('/');
            pagePathName = pagePathName.TrimStart('/');

            string url = string.Empty;

            // if port is set to default value, needn't show this port
            if (port == "80")
            {
                url = string.Format("{0}://{1}{2}/{3}", http, host, appPath, pagePathName);
            }
            else
            {
                url = string.Format("{0}://{1}:{2}{3}/{4}", http, host, port, appPath, pagePathName);
            }

            return url;
        }

        /// <summary>
        /// Determines whether is paid for multiple records.
        /// </summary>
        /// <param name="capIDModels">The CapIDModel4WS array</param>
        /// <returns>
        /// <c>true</c> if is paid for multi records; otherwise, <c>false</c>.
        /// </returns>
        private static bool IsPaid4MultiRecords(CapIDModel4WS[] capIDModels)
        {
            bool result = false;

            if (capIDModels != null && capIDModels.Length > 1)
            {
                result = true;
            }

            return result;
        }

        /// <summary>
        /// this parameters will save to cache and when payment completed, ACA will rebuild url depending on these parameters
        /// </summary>
        /// <param name="currentPage">the current page instance</param>
        /// <returns>the model holds on the parameters</returns>
        private static PaymentEntity BuildPaymentParameters(Page currentPage)
        {
            IPage page = currentPage as IPage;
            IPaymentPage paymentPage = currentPage as IPaymentPage;
            PaymentEntity param = new PaymentEntity();

            if (ACAConstant.PaymentEntityType.TrustAccount.Equals(paymentPage.PaymentEntityType))
            {
                if (paymentPage.GetTrustAccountModel() != null)
                {
                    param.EntityID = paymentPage.GetTrustAccountModel().acctID;
                }
            }

            param.TotalFee = paymentPage.GetTotalFee();
            param.EntityType = paymentPage.PaymentEntityType;
            param.CapIDs = AppSession.GetCapIDModelsFromSession();
            param.PublicUserID = AppSession.User.PublicUserId;

            param.ServProvCode = ConfigManager.AgencyCode;

            // set the merchant account id
            List<RefMerchantAccountModel> accountModels = GetMerchantAccount();
            param.MerchantAccountID = 0;

            if (accountModels != null && accountModels.Count > 0)
            {
                RefMerchantAccountModel accountModel = accountModels[0];

                if (accountModel != null && accountModel.merchantAccountPKModel != null)
                {
                    param.MerchantAccountID = accountModel.merchantAccountPKModel.resId;
                }
            }

            param.PaymentMethod = paymentPage.GetPaymentMethod();
            param.RenewalFlag = HttpContext.Current.Request.QueryString["isRenewal"];
            param.Pay4ExistingCapFlag = HttpContext.Current.Request.QueryString["isPay4ExistingCap"];
            param.ModuleName = page.GetModuleName();

            //if the request contains "Module", replace base module name with it
            if (HttpContext.Current.Request.QueryString["Module"] != null)
            {
                param.ModuleName = HttpContext.Current.Request.QueryString["Module"];
            }

            // the step number will plus 1 when payment completed
            int stepNumber = 0;
            int.TryParse(HttpContext.Current.Request["stepNumber"], out stepNumber);
            param.StepNumber = stepNumber;

            return param;
        }

        #endregion Private Methods
    }
}
