#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: GovolutionPayment.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2009-2014
 *
 *  Description:
 *  GovolutionPayment.
 *
 *  Notes:
 * $Id: GovolutionPayment.cs 277174 2014-08-12 07:38:31Z ACHIEVO\james.shi $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using System;
using System.Collections;
using System.Collections.Generic;
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
    /// The GOVOLUTION payment
    /// </summary>
    public class GovolutionPayment : IPayment
    {
        #region Fields

        /// <summary>
        /// Initiation application id.
        /// </summary>
        private const string INITIATION_APPLICATION_ID = "application_id";

        /// <summary>
        /// Redirection message version.
        /// </summary>
        private const string INITIATION_MESSAGE_VERSION = "message_version";

        /// <summary>
        /// Redirection remittance id.
        /// </summary>
        private const string INITIATION_REMITTANCE_ID = "remittance_id";

        /// <summary>
        /// XPolicy application id.
        /// </summary>
        private const string XPOLICY_APPLICATION_ID = "ApplicationID";

        /// <summary>
        /// XPolicy message version.
        /// </summary>
        private const string XPOLICY_MESSAGE_VERSION = "MessageVersion";

        /// <summary>
        /// XPolicy online url.
        /// </summary>
        private const string XPOLICY_ONLINE_URL = "HostURL";

        /// <summary>
        /// Create an instance of ILog
        /// </summary>
        private static readonly ILog Logger = LogFactory.Instance.GetLogger(typeof(GovolutionPayment));

        /// <summary>
        /// special characters that the 3rd doesn't support
        /// </summary>
        private static readonly string[] SPECIAL_CHARS = { "%", "&", "<", ">", "\"" };

        #endregion Fields

        #region Properties

        /// <summary>
        /// Gets payment method
        /// </summary>
        private string PaymentMethod
        {
            get
            {
                return ACAConstant.PAYMENT_METHOD_GOVOLUTION_ONLINE;
            }
        }

        #endregion Properties

        #region Methods

        /// <summary>
        /// get the e payment configuration
        /// </summary>
        /// <param name="policy"> a XPolicy model</param>
        /// <returns>
        /// the returned hashtable contains the following key-values:
        /// Adapter=specific adapter name
        /// Online_URL=specific online URL
        /// ApplicationID=specific application id
        /// ApplicationVersion=specific application version
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
            DataUtil.ParseString(items, policy.data4, ';', '=');

            return items;
        }

        /// <summary>
        /// Complete payment
        /// </summary>
        /// <param name="currentPage">current Page</param>
        /// <returns>the OnlinePaymentResultModel model.</returns>
        OnlinePaymentResultModel IPayment.CompletePayment(Page currentPage)
        {
            string transactionID = HttpContext.Current.Request[INITIATION_REMITTANCE_ID];
            string keyOfsavedData = GetKeyOfSavedData(transactionID);
            PaymentEntity entity = PaymentHelper.GetDataFromCache<PaymentEntity>(keyOfsavedData);
            string parameters = PaymentHelper.GetPostbackFormData();
            parameters += string.Format("&{0}={1}", ACAConstant.ServProvCode_Key, entity.ServProvCode);
            string adapterName = PaymentHelper.GetAdapterName();
            Logger.InfoFormat("PostbackFormData={0}\n", parameters);

            Logger.DebugFormat("begin to invoke CompletePayment method.\n");
            try
            {
                Logger.InfoFormat("Parameters passed to the method are: parameters={0},callerID={1},PaymentMethod={2}.\n", parameters, entity.PublicUserID, this.PaymentMethod);
                IPaymentBll paymentBll = ObjectFactory.GetObject<IPaymentBll>();
                entity.PaymentResult = paymentBll.CompletePayment(null, null, TempModelConvert.Trim4WSOfCapIDModels(entity.CapIDs), parameters, adapterName, entity.PublicUserID);
                PaymentHelper.SaveOrUpdateDataToCache<PaymentEntity>(keyOfsavedData, entity);

                if (entity.PaymentResult != null)
                {
                    Logger.InfoFormat("return data from CompletePayment method:\n {0}", entity.PaymentResult.paramString);
                    HttpContext.Current.Response.Write(entity.PaymentResult.paramString);
                }
                else
                {
                    Logger.Error("No result returned from CompletePayment WS.\n ");
                }
            }
            catch (Exception ex)
            {
                entity.ErrorMessage = PaymentHelper.FilterAAException(ex.Message);
                PaymentHelper.SaveOrUpdateDataToCache<PaymentEntity>(keyOfsavedData, entity);
                Logger.Error("Error occurred in CompletePayment method.", ex);
                HttpContext.Current.Response.Write("success=false&user_message=");
            }

            return entity.PaymentResult;
        }

        /// <summary>
        /// Get the 3rd page height to make ACA iFrame page meet the 3rd page height automatically.
        /// </summary>
        /// <returns>the third page height.</returns>
        int IPayment.Get3rdPageHeight()
        {
            // need to get the height from xpolicy accordingly.
            return 720;
        }

        /// <summary>
        /// Initiate payment
        /// </summary>
        /// <param name="currentPage">current Page</param>
        /// <returns>the OnlinePaymentResultModel model.</returns>
        OnlinePaymentResultModel IPayment.InitiatePayment(Page currentPage)
        {
            Logger.DebugFormat("begin to prepare parameters.\n");
            IPage paymentPage = currentPage as IPage;

            PaymentEntity entity = new PaymentEntity();
            entity.ModuleName = paymentPage.GetModuleName();
            entity.CapIDs = AppSession.GetCapIDModelsFromSession();
            entity.PublicUserID = AppSession.User.PublicUserId;
            entity.ServProvCode = ConfigManager.AgencyCode;
            entity.StepNumber = 0;
            if (HttpContext.Current.Request["stepNumber"] != null)
            {
                entity.StepNumber = int.Parse(HttpContext.Current.Request["stepNumber"]);
            }

            string moduleName = HttpContext.Current.Request.QueryString["Module"];
            if (string.IsNullOrEmpty(moduleName))
            {
                moduleName = string.Empty;
            }

            entity.ModuleName = moduleName;
 
            entity.RenewalFlag = Convert.ToString(HttpContext.Current.Request.QueryString["isRenewal"]);
            entity.Pay4ExistingCapFlag = Convert.ToString(HttpContext.Current.Request.QueryString["isPay4ExistingCap"]);

            string parameters = string.Format("{0}={1}", ACAConstant.ServProvCode_Key, entity.ServProvCode);
            string adapterName = PaymentHelper.GetAdapterName();

            Logger.DebugFormat("begin to invoke paymentBll.InitiatePayment() method.\n");
            Logger.InfoFormat("Parameters passed to the method are: parameters={0},callerID={1}.\n", parameters, entity.PublicUserID);
            IPaymentBll paymentBll = ObjectFactory.GetObject<IPaymentBll>();
            OnlinePaymentResultModel result = paymentBll.InitiatePayment(TempModelConvert.Trim4WSOfCapIDModels(entity.CapIDs), parameters, adapterName, entity.PublicUserID);

            Logger.DebugFormat("begin to handle results.\n");
            if (result != null &&
                !string.IsNullOrEmpty(result.batchNbr))
            {
                Logger.InfoFormat("the batch transaction number is {0}", result.batchNbr);

                string transactionID = result.batchNbr; //tempDic[INITIATION_REMITTANCE_ID];
                string url = BuildInitiationMessage(transactionID);

                Logger.DebugFormat("begin to save querystring of current page.\n");
                SavePaymentInitiationResults(entity, transactionID, HttpContext.Current.Request.Url.Query.Substring(1));

                Logger.DebugFormat("begin to redirect to the govolution web site.\n");
                HttpContext.Current.Response.Redirect(url);
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
        /// <returns>the OnlinePaymentResultModel model.</returns>
        OnlinePaymentResultModel IPayment.VerifyPayment(Page currentPage)
        {
            string transactionID = HttpContext.Current.Request[INITIATION_REMITTANCE_ID];
            string keyOfsavedData = GetKeyOfSavedData(transactionID);
            PaymentEntity entity = PaymentHelper.GetDataFromCache<PaymentEntity>(keyOfsavedData);
            string parameters = PaymentHelper.GetPostbackFormData();
            string adapterName = PaymentHelper.GetAdapterName();
            Logger.InfoFormat("PostbackFormData={0}\n", parameters);

            parameters += string.Format("&{0}={1}", ACAConstant.ServProvCode_Key, HttpContext.Current.Server.UrlEncode(entity.ServProvCode));

            Logger.DebugFormat("begin to invoke verifyPayment method.\n");
            OnlinePaymentResultModel result = null;
            try
            {
                Logger.InfoFormat("Parameters passed to the method are: parameters={0},callerID={1}.\n", parameters, entity.PublicUserID);
                IPaymentBll paymentBll = ObjectFactory.GetObject<IPaymentBll>();
                result = paymentBll.VerifyPayment(parameters, adapterName, entity.PublicUserID);

                if (result != null)
                {
                    string verificationMessage = result.paramString;

                    Logger.DebugFormat("begin to verify parameters passed to govolution:\n");
                    verificationMessage = this.FilterParameters(verificationMessage);

                    Logger.DebugFormat("return data from VerifyPayment method:\n {0}", verificationMessage);
                    HttpContext.Current.Response.Write(verificationMessage);
                }
                else
                {
                    Logger.Error("No result returned from Verification WS.\n ");
                }
            }
            catch (Exception ex)
            {
                entity.ErrorMessage = PaymentHelper.FilterAAException(ex.Message);
                PaymentHelper.SaveOrUpdateDataToCache<PaymentEntity>(keyOfsavedData, entity);
                Logger.Error("Error occurred in VerifyPayment method.", ex);
                HttpContext.Current.Response.Write("success=false&user_message=");
            }

            return result;
        }

        /// <summary>
        /// Build initiation message.
        /// </summary>
        /// <param name="transactionID">transaction ID</param>
        /// <returns>initiation message</returns>
        private string BuildInitiationMessage(string transactionID)
        {
            Hashtable configs = EPaymentConfig.GetConfig();
            Logger.DebugFormat("configs==null? {0} .\n", configs == null || configs.Count == 0);

            if (configs == null ||
                configs.Count == 0)
            {
                return null;
            }

            string onlineURL = configs[XPOLICY_ONLINE_URL].ToString().Trim();
            string applicationID = configs[XPOLICY_APPLICATION_ID].ToString().Trim();
            string messageVersion = configs[XPOLICY_MESSAGE_VERSION].ToString().Trim();
            string urlPattern = @"{0}?{1}={2}&{3}={4}&{5}={6}";
            string url = string.Format(urlPattern, onlineURL, INITIATION_APPLICATION_ID, applicationID, INITIATION_MESSAGE_VERSION, messageVersion, INITIATION_REMITTANCE_ID, transactionID);

            Logger.DebugFormat("url to govolution:{0}.\n", url);
            return url;
        }

        /// <summary>
        /// filter parameters passed to GOVOLUTION.
        /// </summary>
        /// <param name="queryString">the query string</param>
        /// <returns>filtered parameters</returns>
        private string FilterParameters(string queryString)
        {
            if (string.IsNullOrEmpty(queryString))
            {
                return string.Empty;
            }

            Dictionary<string, string> dicSource = PaymentHelper.ConvertToDictionary(queryString);
            Dictionary<string, string> dicTarget = new Dictionary<string, string>();
            if (dicSource == null ||
                dicSource.Count == 0)
            {
                return string.Empty;
            }

            foreach (string key in dicSource.Keys)
            {
                string tempValue = dicSource[key];
                if (!string.IsNullOrEmpty(tempValue))
                {
                    foreach (string specialChar in SPECIAL_CHARS)
                    {
                        tempValue = tempValue.Replace(specialChar, string.Empty);
                    }
                }

                dicTarget[key] = tempValue;
            }

            return PaymentHelper.ConvertToQueryString(dicTarget);
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
        /// Save payment initiation results.
        /// </summary>
        /// <param name="entity">the entity.</param>
        /// <param name="transactionID">the transaction ID</param> 
        /// <param name="dataFromRequest">the data From Request</param>
        private void SavePaymentInitiationResults(PaymentEntity entity, string transactionID, string dataFromRequest)
        {
            Logger.InfoFormat("results are being saved:\nresultsFromWS={0}\nresultsFromRequest={1}\n", transactionID, dataFromRequest);
            string keyOfsavedData = GetKeyOfSavedData(transactionID);

            Logger.DebugFormat("Save Processor To Cache. key={0}\n", keyOfsavedData);
            PaymentHelper.SaveDataToCache<PaymentEntity>(keyOfsavedData, entity);
        }

        #endregion Methods
    }
}
