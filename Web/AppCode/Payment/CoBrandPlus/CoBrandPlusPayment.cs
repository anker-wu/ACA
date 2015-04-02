#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: CoBrandPlusPayment.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2009-2014
 *
 *  Description:
 *
 *  Notes:
 * $Id: CoBrandPlusPayment.cs 277174 2014-08-12 07:38:31Z ACHIEVO\james.shi $.
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
using Accela.ACA.Common;
using Accela.ACA.Common.Log;
using Accela.ACA.Common.Util;
using Accela.ACA.Web.Common;
using Accela.ACA.WSProxy;
using log4net;

namespace Accela.ACA.Web.Payment
{
    /// <summary>
    /// This class provide the CoBrandPlus Payment
    /// </summary>
    public class CoBrandPlusPayment : IPayment
    {
        #region Fields

        /// <summary>
        /// batch transaction number.
        /// </summary>
        private const string BATCH_TRANSACTION_NBR = "batchTransNbr";

        /// <summary>
        /// DECLINED – the user’s card was declined (could be insufficient funds, etc.)
        /// </summary>
        private const string ERROR_DECLINED = "DECLINED";

        /// <summary>
        /// Error Codes – there was an error during the process
        /// </summary>
        private const string ERROR_ERROR = "ERROR"; 

        /// <summary>
        /// HOLD – the user’s card was declined (could be insufficient funds, etc.)
        /// </summary>
        private const string ERROR_HOLD = "HOLD";

        /// <summary>
        /// OTHER – there was an unrecognizable error
        /// </summary>
        private const string ERROR_OTHER = "OTHER";

        /// <summary>
        /// REFERRAL – the user’s card was declined (user needs to call issuer)
        /// </summary>
        private const string ERROR_REFERRAL = "REFERRAL";

        /// <summary>
        /// return element: account type
        /// </summary>
        private const string RETURN_ELEMENT_ACCOUNTTYPE = "$$ACCOUNTTYPE$$";

        /// <summary>
        /// return element: address1
        /// </summary>
        private const string RETURN_ELEMENT_ADDRESS1 = "$$ADDRESS1$$";

        /// <summary>
        /// return element: address2
        /// </summary>
        private const string RETURN_ELEMENT_ADDRESS2 = "$$ADDRESS2$$";

        /// <summary>
        /// return element: address3
        /// </summary>
        private const string RETURN_ELEMENT_ADDRESS3 = "$$ADDRESS3$$";

        /// <summary>
        /// return element: confirmation
        /// </summary>
        private const string RETURN_ELEMENT_CONFIRMATION = "$$CONFIRMATION$$";

        /// <summary>
        /// return element: email
        /// </summary>
        private const string RETURN_ELEMENT_EMAIL = "$$EMAIL$$";

        /// <summary>
        /// return element: first name
        /// </summary>
        private const string RETURN_ELEMENT_FIRST_NAME = "$$FIRSTNAME$$";

        /// <summary>
        /// return element: last name
        /// </summary>
        private const string RETURN_ELEMENT_LAST_NAME = "$$LASTNAME$$";

        /// <summary>
        /// return element: middle name
        /// </summary>
        private const string RETURN_ELEMENT_MIDDLE_NAME = "$$MIDDLENAME$$";

        /// <summary>
        /// return element: phone number
        /// </summary>
        private const string RETURN_ELEMENT_PHONENUM = "$$PHONENUM$$";

        /// <summary>
        /// return element: unique id.
        /// composed with CAPID + "|" + DateTime.Now.Ticks
        /// </summary>
        private const string RETURN_ELEMENT_UNIQUEID = "$$UNIQUEID$$";

        /// <summary>
        /// url parameter: address1
        /// </summary>
        private const string URL_PARMETER_ADDRESS1 = "$$ADDRESS1$$";

        /// <summary>
        /// url parameter: cap id
        /// </summary>
        private const string URL_PARMETER_CAPID = "$$CAPID$$";

        /// <summary>
        /// url parameter: city
        /// </summary>
        private const string URL_PARMETER_CITY = "$$CITY$$";

        /// <summary>
        /// url parameter: email
        /// </summary>
        private const string URL_PARMETER_EMAIL = "$$EMAIL$$";

        /// <summary>
        /// url parameter: first name
        /// </summary>
        private const string URL_PARMETER_FIRSTNAME = "$$FIRSTNAME$$";

        /// <summary>
        /// url parameter: last name
        /// </summary>
        private const string URL_PARMETER_LASTNAME = "$$LASTNAME$$";

        /// <summary>
        /// url parameter: middle name
        /// </summary>
        private const string URL_PARMETER_MIDDLENAME = "$$MIDDLENAME$$";

        /// <summary>
        /// url parameter: payment amount
        /// </summary>
        private const string URL_PARMETER_PAYMENTAMOUNT = "$$PAYMENTAMOUNT$$";

        /// <summary>
        /// url parameter: phone number
        /// </summary>
        private const string URL_PARMETER_PHONENUM = "$$PHONENUM$$";

        /// <summary>
        /// url parameter: zip
        /// </summary>
        private const string URL_PARMETER_POSTALCD = "$$ZIP$$";

        /// <summary>
        /// url parameter: product id
        /// </summary>
        private const string URL_PARMETER_PRODUCTID = "$$PRODUCTID$$";

        /// <summary>
        /// url parameter: state
        /// </summary>
        private const string URL_PARMETER_PROVINCECD = "$$STATE$$";

        /// <summary>
        /// url parameter: unique id
        /// composed with CAPID + "|" + DateTime.Now.Ticks
        /// </summary>
        private const string URL_PARMETER_UNIQUE_ID = "$$UNIQUEID$$";

        /// <summary>
        /// url parameter: number items
        /// </summary>
        private const string URL_PARAMETER_NUMBERITEMS = "$$NUMBERITEMS$$";

        /// <summary>
        /// url parameter: when select multiple caps to pay, it show "Multiple items" value on 3rd party payment site.
        /// </summary>
        private const string URL_PARAMETER_VALUE_MULTIPLE = "Multiple items";

        /// <summary>
        /// url parameter: when pay for partial cap, it show to "New" value on 3rd party payment site.
        /// </summary>
        private const string URL_PARAMETER_VALUE_NEW = "New";

        /// <summary>
        /// The logger.
        /// </summary>
        private static readonly ILog Logger = LogFactory.Instance.GetLogger(typeof(CoBrandPlusPayment));

        #endregion Fields

        #region Methods

        /// <summary>
        /// Complete payment
        /// </summary>
        /// <param name="currentPage">current Page</param>
        /// <returns>The OnlinePaymentResultModel</returns>
        public OnlinePaymentResultModel CompletePayment(Page currentPage)
        {
            //Handle posted back data;
            Logger.Info("************log postback data begin*****************");

            foreach (string key in HttpContext.Current.Request.Form.AllKeys)
            {
                Logger.Info(key + " : " + HttpContext.Current.Request.Form[key]);
            }

            Logger.Info("************log postback data end*****************");

            Hashtable htPostBackData = PopulatePostBackData();

            if (htPostBackData == null)
            {
                Logger.Error("*****************the post back data is empty *****************");

                return null;
            }

            Hashtable configs = EPaymentConfig.GetConfig();
            if (configs == null ||
                !configs.ContainsKey("PostbackParameterMap"))
            {
                throw new ACAException("Configuration of payment is not correct.");
            }

            Hashtable postBackNameList = (Hashtable)configs["PostbackParameterMap"];

            string confirmation = string.Empty;
            string transactionID = string.Empty;

            Hashtable htNameValue = new Hashtable();
            foreach (DictionaryEntry item in postBackNameList)
            {
                if (item.Value == null)
                {
                    continue;
                }

                string value = item.Value.ToString().ToUpperInvariant();
                AddParameters(htNameValue, value, item.Key.ToString());

                if (value.Equals(RETURN_ELEMENT_CONFIRMATION, StringComparison.InvariantCulture) &&
                    htPostBackData[item.Key] != null)
                {
                    confirmation = htPostBackData[item.Key].ToString();
                }
                else if (value.Equals(RETURN_ELEMENT_UNIQUEID, StringComparison.InvariantCulture) &&
                         htPostBackData[item.Key] != null)
                {
                    transactionID = htPostBackData[item.Key].ToString();

                    Logger.ErrorFormat("The transaction id is {0}.", transactionID);
                }
            }

            if (string.IsNullOrEmpty(transactionID))
            {
                Logger.Error("Return parameter lost CAP id.");
                return null;
            }

            ICacheManager cacheManager = ObjectFactory.GetObject<ICacheManager>();
            CoBrandPlusEntity entity = cacheManager.GetSingleCachedItem(transactionID) as CoBrandPlusEntity;

            if (entity == null)
            {
                Logger.ErrorFormat("Payment Entity [Transaction ID:{0}] is lost in cache, so need to retrieve it again.", transactionID);

                ICommonPaymentBll commonPaymentBll = (ICommonPaymentBll)ObjectFactory.GetObject(typeof(ICommonPaymentBll));

                // For TPE payment method, some sub-agency records are paid on super agency.
                // So, the first parameter agencyCode should be empty.
                TransactionModel[] trans = commonPaymentBll.GetTransactionModelById(string.Empty, transactionID);

                if (trans != null && trans.Length > 0)
                {
                    entity = new CoBrandPlusEntity();
                    entity.ServProvCode = trans[0].serviceProviderCode;
                    entity.TotalFee = trans[0].totalFee.Value;
                    entity.PublicUserID = trans[0].auditID;
                    entity.PaymentStatus = ACAConstant.PAYMENT_STATUS_START_PAID;
                    entity.PaymentMethod = PaymentMethod.CreditCard.Equals(trans[0].procTransType) ? PaymentMethod.CreditCard : PaymentMethod.Check;
                    long batchTransCode = long.Parse(trans[0].batchTransCode.ToString());
                    if (StandardChoiceUtil.IsEnableShoppingCart())
                    {
                         entity.CapIDs = commonPaymentBll.GetCapListByTransCode(entity.ServProvCode, batchTransCode, entity.PublicUserID);
                    }
                    else
                    {
                        string entityID = trans[0].entityID;
                        entity.CapIDs = new CapIDModel[1];
                        entity.CapIDs[0] = commonPaymentBll.GetCapIDModelByEntityID(entity.ServProvCode, entityID);
                    }

                    Logger.ErrorFormat(
                                "Payment Entity Data: {0}||{1}||{2}",
                                entity.ServProvCode,
                                entity.TotalFee,
                                entity.PublicUserID);

                    cacheManager.AddSingleItemToCache(transactionID, entity, 30);
                }
            }

            if (entity == null
                || string.IsNullOrEmpty(confirmation)
                || ERROR_ERROR.Equals(confirmation, StringComparison.InvariantCulture)
                || ERROR_DECLINED.Equals(confirmation, StringComparison.InvariantCulture)
                || ERROR_REFERRAL.Equals(confirmation, StringComparison.InvariantCulture)
                || ERROR_HOLD.Equals(confirmation, StringComparison.InvariantCulture)
                || ERROR_OTHER.Equals(confirmation, StringComparison.InvariantCulture))
            {
                if (entity != null)
                {
                    entity.PaymentStatus = ACAConstant.PAYMENT_STATUS_PAID_FAILED;
                }
                else
                {
                    Logger.Error("CoBrandPlusEntity is lost.");
                }

                Logger.Error("*****************Payment failed *****************");
                foreach (string key in HttpContext.Current.Request.Form.AllKeys)
                {
                    Logger.Error(key + " : " + HttpContext.Current.Request.Form[key]);
                }

                return null;
            }

            Logger.ErrorFormat("The confirmation is {0},The UNIQUE ID is {1},the agency code is {2}.", confirmation, transactionID, entity.ServProvCode);

            OnlinePaymentResultModel paymentResult = null;

            bool canStartPayment = false;

            /* Void Posting Data From 3rd Party Twice
             * 1. Void posting data from 3rd Party twice by quick clicking submit 
             * 2. If the first time is failed and the next time is succeed.It should be creat the cap. 
             */
            if (ACAConstant.PAYMENT_STATUS_START_PAID.Equals(entity.PaymentStatus) 
                || ACAConstant.PAYMENT_STATUS_PAID_FAILED.Equals(entity.PaymentStatus))
            {               
                lock (entity)
                {
                    Logger.ErrorFormat("Transaction ID {0} is locked. The current payment status: {1}.", transactionID, entity.PaymentStatus);

                    if (ACAConstant.PAYMENT_STATUS_START_PAID.Equals(entity.PaymentStatus) 
                        || ACAConstant.PAYMENT_STATUS_PAID_FAILED.Equals(entity.PaymentStatus))
                    {
                        entity.PaymentStatus = ACAConstant.PAYMENT_STATUS_CREATING_CAP;
                        canStartPayment = true;
                    }
                    else
                    {
                        Logger.ErrorFormat("(In Lock) The confirmation {0} is posted twice. entity.PaymentStatus is {1}.", confirmation, entity.PaymentStatus);
                    }

                    Logger.ErrorFormat("Transaction ID {0} is unlocked.", transactionID);
                }
            }
            else
            {
                Logger.ErrorFormat("The confirmation {0} is posted twice. entity.PaymentStatus is {1}.", confirmation, entity.PaymentStatus);
            }

            if (canStartPayment)
            {
                paymentResult = CreditCardPayment(htNameValue, htPostBackData, entity);
            }
            else
            {
                paymentResult = null;
            }

            return paymentResult;            
        }

        /// <summary>
        /// Get the 3rd party page height.
        /// </summary>
        /// <returns>The 3rd party page height.</returns>
        public int Get3rdPageHeight()
        {
            return 0;
        }

        /// <summary>
        /// get the e payment configuration
        /// </summary>
        /// <param name="policy"> a XPolicy model</param>
        /// <returns>
        /// the returned hashtable contains 5 key-values:
        /// Adapter                 :  "Redirect"
        /// HostURL                 :   a string value
        /// RequestParameterMap     :   a hashtable
        /// PostBackParameterMap    :   a hashtable
        /// ProductID               :   product id value
        /// </returns>
        public Hashtable GetEPaymentConfig(XPolicyModel policy)
        {
            if (policy == null)
            {
                return null;
            }

            Hashtable items = new Hashtable(StringComparer.InvariantCultureIgnoreCase);
            DataUtil.ParseString(items, policy.data1, ';', '=');
            DataUtil.ParseString(items, policy.data2, ';', '=');
            if (!string.IsNullOrEmpty(policy.data3))
            {
                string[] d = policy.data3.Split(';');
                DataUtil.ParseString(items, d[0], '=', ',', ':');
                if (d.Length > 1)
                {
                    DataUtil.ParseString(items, d[1], '=', ',', ':');
                }
            }

            DataUtil.ParseString(items, policy.data4, '=');

            return items;
        }

        /// <summary>
        /// Initiate payment
        /// </summary>
        /// <param name="currentPage">current Page</param>
        /// <returns>The OnlinePaymentResultModel</returns>
        public OnlinePaymentResultModel InitiatePayment(Page currentPage)
        {
            Logger.DebugFormat("begin to prepare parameters.\n");
            IPaymentPage paymentPage = currentPage as IPaymentPage;
            string parameters = string.Format("{0}={1}", ACAConstant.ServProvCode_Key, ConfigManager.AgencyCode);

            CoBrandPlusEntity entity = new CoBrandPlusEntity();
            entity.ServProvCode = ConfigManager.AgencyCode;
            entity.TotalFee = paymentPage.GetTotalFee();
            entity.CapIDs = TempModelConvert.Trim4WSOfCapIDModels(AppSession.GetCapIDModelsFromSession());
            entity.PublicUserID = AppSession.User.PublicUserId;
            entity.StepNumber = 0;
            if (HttpContext.Current.Request["stepNumber"] != null)
            {
                entity.StepNumber = int.Parse(HttpContext.Current.Request["stepNumber"]);
            }

            string moduleName = HttpContext.Current.Request.QueryString[ACAConstant.MODULE_NAME];
            if (string.IsNullOrEmpty(moduleName))
            {
                moduleName = string.Empty;
            }

            entity.ModuleName = moduleName;
            entity.PaymentMethod = paymentPage.GetPaymentMethod();

            string adapterName = PaymentHelper.GetAdapterName();

            Logger.DebugFormat("begin to invoke paymentBll.InitiatePayment() method.\n");
            Logger.InfoFormat("Parameters passed to the method are: parameters={0},callerID={1}.\n", parameters, entity.PublicUserID);
            IPaymentBll paymentBll = ObjectFactory.GetObject<IPaymentBll>();
            OnlinePaymentResultModel result = paymentBll.InitiatePayment(entity.CapIDs, parameters, adapterName, entity.PublicUserID);

            Logger.DebugFormat("begin to handle results.\n"); 
            if (result != null &&
                !string.IsNullOrEmpty(result.batchNbr))
            {
                Logger.DebugFormat("results:\n {0}", result.batchNbr);

                string transactionID = result.batchNbr; //[INITIATION_REMITTANCE_ID];
                HttpContext.Current.Session[SessionConstant.TRANSACTION_ID] = transactionID;

                PaymentMethod paymentMethod = paymentPage.GetPaymentMethod();
                string url = BuildInitiationMessage(transactionID, entity, paymentMethod);

                string pbMsg = LabelUtil.GetTextByKey("aca_payment_redirection_text", string.Empty);
                pbMsg = Accela.ACA.Common.Common.ScriptFilter.EncodeJson(pbMsg);

                string strScript = string.Format("<script>document.body.innerHTML = '<span class=\"font13px\">{0}</span>';NeedAsk = false; window.open('{1}','_parent');</script>", pbMsg, url);

                Logger.InfoFormat("Redirect to Official Payment URL : {0}", url);
                Page curPage = HttpContext.Current.Handler as Page;

                if (curPage != null)
                {
                    curPage.RegisterClientScriptBlock("CoBrandPlus", strScript);
                }
            }
            else
            {
                Logger.Error("No result returned from InitiatePayment method.\n ");
                string msg = result == null ? string.Empty : "No result returned.";
                MessageUtil.ShowMessage(currentPage, MessageType.Error, msg);
            }

            return result;
        }

        /// <summary>
        /// Verify payment
        /// </summary>
        /// <param name="currentPage">current Page</param>
        /// <returns>The OnlinePaymentResultModel</returns>
        public OnlinePaymentResultModel VerifyPayment(Page currentPage)
        {
            return null;
        }

        /// <summary>
        /// Add parameters.
        /// </summary>
        /// <param name="parameters">The parameters.</param>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        private void AddParameters(Hashtable parameters, string key, string value)
        {
            if (parameters == null ||
                string.IsNullOrEmpty(key))
            {
                return;
            }

            if (value != null)
            {
                parameters.Add(key.Trim(), value.Trim());
            }
            else
            {
                parameters.Add(key.Trim(), string.Empty);
            }
        }

        /// <summary>
        /// combine url
        /// </summary>
        /// <param name="transactionID">transaction id</param>
        /// <param name="entity">CoBrandPlus entity</param>
        /// <param name="paymentMethod">payment method</param>
        /// <returns>url message</returns>
        private string BuildInitiationMessage(string transactionID, CoBrandPlusEntity entity, PaymentMethod paymentMethod)
        {
            Hashtable configs = EPaymentConfig.GetConfig();

            //Get payment group level product ID
            IList<string> groupIDs = CapUtil.GetGroupIDListByCapIDs(entity.CapIDs);

            if (groupIDs != null && groupIDs.Count == 1)
            {
                if (configs.Contains("ProductID"))
                {
                    configs.Remove("ProductID");
                }

                //If it exists in standard choice, replace agency level product ID with it.
                configs.Add("ProductID", groupIDs[0]);
            }

            if (configs == null || configs.Count < 5)
            {
                throw new ACAException("Configuration of payment is not correct.");
            }

            string url = string.Empty;

            if (PaymentMethod.Check == paymentMethod)
            {
                url = (string)configs["ECheckHostURL"];
            }
            else
            {
                url = (string)configs["HostURL"];
            }

            Hashtable paramerters = (Hashtable)configs["RequestParameterMap"];

            // productId and cde-UniqID-1 parameter is required for online payment and which are case sensitive.
            if (!CheckRequiredParameter(paramerters))
            {
                throw new ACAException("cde-UniqID-1 and productId is not configured correctly.");
            }

            StringBuilder urlParameters = new StringBuilder();

            Hashtable htParameters = GetUrlParameters(configs, transactionID, entity);

            foreach (DictionaryEntry item in paramerters)
            {
                if (item.Value == null)
                {
                    continue;
                }

                //string valueKeyName = item.Value.ToString().ToUpper();
                string valueKeyName = item.Value.ToString().ToUpperInvariant();

                if (htParameters[valueKeyName] != null)
                {
                    string parameter = string.Format("{0}={1}&", item.Key, HttpUtility.UrlEncode(htParameters[valueKeyName].ToString()));
                    urlParameters.Append(parameter);
                }
            }

            if (urlParameters.Length > 0)
            {
                url = url + "?" + urlParameters.ToString().Substring(0, urlParameters.Length - 1);
            }

            HttpContext.Current.Session[SessionConstant.SESSION_LOAD_PAYMENT_COMPLETION] = null;
            HttpContext.Current.Session[ACAConstant.MODULE_NAME] = entity.ModuleName;

            return url;
        }

        /// <summary>
        /// checked whether productId and <c>cde-UniqID-1</c> parameter have been configured the parameter is correct which are case sensitive.
        /// </summary>
        /// <param name="parameters">parameter list in SC BizDomainConstant.STD_CAT_CREDIT_CARD_URL_PARAMETER.</param>
        /// <returns>true - parameter is valid.</returns>
        private bool CheckRequiredParameter(Hashtable parameters)
        {
            if (parameters == null ||
                parameters.Count == 0)
            {
                return false;
            }

            bool isUniqueIdConfigured = false;
            bool isProductIdConfigured = false;

            if (!parameters.ContainsKey("cde-UniqID-1") ||
                parameters["cde-UniqID-1"].ToString() != "$$UNIQUEID$$")
            {
                Logger.ErrorFormat("cde-UniqID-1 and $$UNIQUEID$$ must be configured correctly in {0}.", BizDomainConstant.STD_CAT_CREDIT_CARD_URL_PARAMETER);
            }
            else
            {
                isUniqueIdConfigured = true;
            }

            if (!parameters.ContainsKey("productId") ||
                parameters["productId"].ToString() != "$$ProductId$$")
            {
                Logger.ErrorFormat("productId and $$ProductId$$ must be configured correctly in {0}.", BizDomainConstant.STD_CAT_CREDIT_CARD_URL_PARAMETER);
            }
            else
            {
                isProductIdConfigured = true;
            }

            return isUniqueIdConfigured && isProductIdConfigured;
        }

        /// <summary>
        /// payment use credit card
        /// </summary>
        /// <param name="htNameValue">the mapping of return element name and ACA data object name</param>
        /// <param name="htPostBackData">the data set of post back from online payment web site</param>
        /// <param name="entity">the CoBrandPlus entity</param>
        /// <returns>The credit card payment.</returns>
        private OnlinePaymentResultModel CreditCardPayment(Hashtable htNameValue, Hashtable htPostBackData, CoBrandPlusEntity entity)
        {
            Logger.DebugFormat("begin to invoke CompletePayment method.\n");

            try
            {
                string adapterName = PaymentHelper.GetAdapterName();
                string transactionID = GetPostBackData(htNameValue, htPostBackData, RETURN_ELEMENT_UNIQUEID);
                Logger.InfoFormat("The transaction ID is {0}.", transactionID);

                CreditCardModel4WS creditCardModel = GetCreditCardModel(htNameValue, htPostBackData);

                string parameters = string.Format("{0}={1}&{2}={3}", ACAConstant.ServProvCode_Key, entity.ServProvCode, BATCH_TRANSACTION_NBR, transactionID);

                CheckModel4WS checkModel = GetCheckModel(htNameValue, htPostBackData);

                if (entity.PaymentMethod == PaymentMethod.Check)
                {
                    parameters += string.Format("&PaymentMethod={0}", ACAConstant.PAY_METHOD_CHECK);
                }
                else
                {
                    parameters += string.Format("&PaymentMethod={0}", ACAConstant.PAY_METHOD_CREDIT_CARD);
                }
  
                Logger.InfoFormat("The parameter string for credit card payment is:\n {0}", parameters);

                IPaymentBll paymentBll = ObjectFactory.GetObject<IPaymentBll>();
                entity.PaymentResult = paymentBll.CompletePayment(creditCardModel, checkModel, entity.CapIDs, parameters, adapterName, entity.PublicUserID);

                if (entity.PaymentResult != null)
                {
                    Logger.InfoFormat("return data from CompletePayment method:\n {0}", entity.PaymentResult.paramString);
                    entity.PaymentStatus = ACAConstant.PAYMENT_STATUS_CONVERT_CAP_SUCCESS;

                    ICacheManager cacheManager = ObjectFactory.GetObject<ICacheManager>();
                    cacheManager.AddSingleItemToCache(transactionID, entity, ACAConstant.PAYMENT_STAUTS_CACHE_EXPIRE_TIME);
                }
                else
                {
                    Logger.Error("No result returned from CompletePayment WS.\n ");
                }
            }
            catch (Exception ex)
            {
                entity.ErrorMessage = PaymentHelper.FilterAAException(ex.Message);
                entity.PaymentStatus = ACAConstant.PAYMENT_STATUS_CONVERT_CAP_FAILED;

                string transactionID = GetPostBackData(htNameValue, htPostBackData, RETURN_ELEMENT_UNIQUEID);
                ICacheManager cacheManager = ObjectFactory.GetObject<ICacheManager>();
                cacheManager.AddSingleItemToCache(transactionID, entity, ACAConstant.PAYMENT_STAUTS_CACHE_EXPIRE_TIME);

                Logger.Error("****************************Create Cap failed *****************");
                foreach (string key in HttpContext.Current.Request.Form.AllKeys)
                {
                    Logger.Error(key + " : " + HttpContext.Current.Request.Form[key]);
                }

                Logger.Error(ex.Message, ex);
            }

            Logger.DebugFormat("end to invoke CompletePayment method.\n");

            return entity.PaymentResult;
        }

        /// <summary>
        /// Construct CreditCardModel.
        /// </summary>
        /// <param name="htNameValue">the mapping of return element name and ACA data object name</param>
        /// <param name="htPostBackData">the data set of post back from online payment web site</param>
        /// <returns>The credit card model.</returns>
        private CreditCardModel4WS GetCreditCardModel(Hashtable htNameValue, Hashtable htPostBackData)
        {
            //construct an credit card model
            CreditCardModel4WS creditCardModel = new CreditCardModel4WS();
            creditCardModel.servProvCode = ConfigManager.AgencyCode;
            creditCardModel.cardType = GetPostBackData(htNameValue, htPostBackData, RETURN_ELEMENT_ACCOUNTTYPE);

            creditCardModel.streetAddress = GetPostBackData(htNameValue, htPostBackData, RETURN_ELEMENT_ADDRESS1);
            creditCardModel.streetAddress2 = GetPostBackData(htNameValue, htPostBackData, RETURN_ELEMENT_ADDRESS2);
            creditCardModel.streetAddress3 = GetPostBackData(htNameValue, htPostBackData, RETURN_ELEMENT_ADDRESS3);
            creditCardModel.city = string.Empty;
            creditCardModel.state = string.Empty;
            creditCardModel.email = GetPostBackData(htNameValue, htPostBackData, RETURN_ELEMENT_EMAIL);
            creditCardModel.pos = false;
            creditCardModel.accountNumber = string.Empty;
            creditCardModel.securityCode = string.Empty;
            creditCardModel.telephone = GetPostBackData(htNameValue, htPostBackData, RETURN_ELEMENT_PHONENUM);

            creditCardModel.firstName = GetPostBackData(htNameValue, htPostBackData, RETURN_ELEMENT_FIRST_NAME);
            creditCardModel.middleName = GetPostBackData(htNameValue, htPostBackData, RETURN_ELEMENT_MIDDLE_NAME);
            creditCardModel.lastName = GetPostBackData(htNameValue, htPostBackData, RETURN_ELEMENT_LAST_NAME);
            creditCardModel.billingPostalCD = creditCardModel.postalCode;
            creditCardModel.countryCD = string.Empty;

            return creditCardModel;
        }

        /// <summary>
        /// Get post back data.
        /// </summary>
        /// <param name="htNameValue">The name value pair.</param>
        /// <param name="htPostBackData">The post back data.</param>
        /// <param name="key">The key.</param>
        /// <returns>Return the post back data.</returns>
        private string GetPostBackData(Hashtable htNameValue, Hashtable htPostBackData, string key)
        {
            if (htNameValue == null || htPostBackData == null || string.IsNullOrEmpty(key))
            {
                return string.Empty;
            }

            object name = htNameValue[key];

            if (name == null)
            {
                return string.Empty;
            }

            object value = htPostBackData[name];
            if (value != null)
            {
                return value.ToString();
            }
            else
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// Populate parameters of url for web online payment
        /// "$$PaymentAmount$$", "$$ProductId$$" and "$$CAPID$$" must pass to official payment site.
        /// </summary>
        /// <param name="configs">The config</param>
        /// <param name="transactionID">The transaction ID</param>
        /// <param name="entity">The CoBrandPlus entity</param>
        /// <returns>The url parameters.</returns>
        private Hashtable GetUrlParameters(Hashtable configs, string transactionID, CoBrandPlusEntity entity)
        {
            string amount = entity.TotalFee.ToString(CultureInfo.InvariantCulture);

            Hashtable htParameters = new Hashtable();

            if (!AppSession.User.IsAnonymous)
            {
                AddParameters(htParameters, URL_PARMETER_FIRSTNAME, AppSession.User.FirstName);
                AddParameters(htParameters, URL_PARMETER_LASTNAME, AppSession.User.LastName);
                AddParameters(htParameters, URL_PARMETER_MIDDLENAME, AppSession.User.MiddleName);
                AddParameters(htParameters, URL_PARMETER_EMAIL, AppSession.User.Email);
                AddParameters(htParameters, URL_PARMETER_PHONENUM, AppSession.User.HomePhone);
                AddParameters(htParameters, URL_PARMETER_ADDRESS1, AppSession.User.Address);
                AddParameters(htParameters, URL_PARMETER_CITY, AppSession.User.City);
                AddParameters(htParameters, URL_PARMETER_POSTALCD, AppSession.User.Zip);
                AddParameters(htParameters, URL_PARMETER_PROVINCECD, AppSession.User.State);
            }

            AddParameters(htParameters, URL_PARMETER_PAYMENTAMOUNT, amount);

            string productId = (string)configs["ProductID"];

            AddParameters(htParameters, URL_PARMETER_PRODUCTID, productId);

            // productId and cde-UniqID-1 parameter is required for online payment
            if (string.IsNullOrEmpty(productId))
            {
                Logger.ErrorFormat("product id is not configured in {0}.", BizDomainConstant.STD_CAT_ONLINE_PAYMENT_WEBSERVICE);
            }

            Logger.DebugFormat("The transaction Id is {0} before redirect to cobrand site.", transactionID);

            entity.PaymentStatus = ACAConstant.PAYMENT_STATUS_START_PAID;
            Logger.DebugFormat("the payment status is {0} before redirect to cobrand site.", entity.PaymentStatus);
            entity.PaymentQueryString = HttpContext.Current.Request.QueryString.ToString();
            Logger.DebugFormat("the query string is {0} before redirect to cobrand site.", entity.PaymentQueryString);

            CapIDModel[] capIds = entity.CapIDs;
            string capIDParam = string.Empty;

            if (capIds != null && capIds.Length > 0)
            {
                AddParameters(htParameters, URL_PARAMETER_NUMBERITEMS, capIds.Length.ToString());

                if (capIds.Length > 1)
                {
                    capIDParam = URL_PARAMETER_VALUE_MULTIPLE;
                }
                else
                {
                    var capBll = ObjectFactory.GetObject<ICapBll>();
                    CapModel4WS capModel = capBll.GetCapByPK(TempModelConvert.Add4WSForCapIDModel(capIds[0]));

                    if (CapUtil.IsPartialCap(capModel.capClass))
                    {
                        capIDParam = URL_PARAMETER_VALUE_NEW;
                    }
                    else
                    {
                        capIDParam = capModel.capID.customID;
                    }
                }
            }

            AddParameters(htParameters, URL_PARMETER_CAPID, capIDParam);
            
            // transaction id for OPC
            AddParameters(htParameters, URL_PARMETER_UNIQUE_ID, transactionID);

            entity.ConfigData = htParameters;

            ICacheManager cacheManager = ObjectFactory.GetObject<ICacheManager>();

            // uniqueId is regarded as cache key, which is used to retrieve the payment status when the post back data is returned by OP interface.
            // Here the session can't be used, because when the paymentResult page is redirect by third site, the session will be lost.
            cacheManager.AddSingleItemToCache(transactionID, entity, ACAConstant.PAYMENT_STAUTS_CACHE_EXPIRE_TIME);
            Logger.DebugFormat("the agency code is {0} when the data is added to cache", entity.ServProvCode);

            return htParameters;
        }

        /// <summary>
        /// Populate post back data from web online payment
        /// </summary>
        /// <returns>the data for post data</returns>
        private Hashtable PopulatePostBackData()
        {
            Hashtable htPostBackData = new Hashtable();

            foreach (string key in HttpContext.Current.Request.Form.AllKeys)
            {
                AddParameters(htPostBackData, key, HttpContext.Current.Request.Form[key]);
            }

            foreach (string key in HttpContext.Current.Request.QueryString.AllKeys)
            {
                AddParameters(htPostBackData, key, HttpContext.Current.Request.QueryString[key]);
            }

            return htPostBackData;
        }

        /// <summary>
        /// Construct CheckModel.
        /// </summary>
        /// <param name="htNameValue">the mapping of return element name and ACA data object name</param>
        /// <param name="htPostBackData">the data set of post back from online payment web site</param>
        /// <returns>Return the check model.</returns>
        private CheckModel4WS GetCheckModel(Hashtable htNameValue, Hashtable htPostBackData)
        {
            //construct an check model
            CheckModel4WS checkModel = new CheckModel4WS();
            
            checkModel.accountNbr = string.Empty;
            checkModel.checkType = GetPostBackData(htNameValue, htPostBackData, RETURN_ELEMENT_ACCOUNTTYPE);
            checkModel.city = string.Empty;
            checkModel.countryCD = string.Empty;
            checkModel.email = GetPostBackData(htNameValue, htPostBackData, RETURN_ELEMENT_EMAIL);
            checkModel.firstName = GetPostBackData(htNameValue, htPostBackData, RETURN_ELEMENT_FIRST_NAME);
            checkModel.middleName = GetPostBackData(htNameValue, htPostBackData, RETURN_ELEMENT_MIDDLE_NAME);
            checkModel.lastName = GetPostBackData(htNameValue, htPostBackData, RETURN_ELEMENT_LAST_NAME);
            checkModel.servProvCode = ConfigManager.AgencyCode;
            checkModel.state = string.Empty;
            checkModel.streetAddress = GetPostBackData(htNameValue, htPostBackData, RETURN_ELEMENT_ADDRESS1);
            checkModel.streetAddress2 = GetPostBackData(htNameValue, htPostBackData, RETURN_ELEMENT_ADDRESS2);
            checkModel.streetAddress3 = GetPostBackData(htNameValue, htPostBackData, RETURN_ELEMENT_ADDRESS3);
  
            return checkModel;
        }

        #endregion Methods
    }
}
