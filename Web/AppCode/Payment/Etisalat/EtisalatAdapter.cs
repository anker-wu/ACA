#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: EtisalatAdapter.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2009-2014
 *
 *  Description:
 *
 *  Notes:
 *      $Id: EtisalatAdapter.cs 278210 2014-08-29 05:45:24Z ACHIEVO\james.shi $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * 12-31-2008           Xinter Peng               Initial
 * </pre>
 */

#endregion Header

using System;
using System.Collections;
using System.Text;
using System.Web;
using System.Web.SessionState;
using Accela.ACA.BLL.Finance;
using Accela.ACA.Common;
using Accela.ACA.Common.Log;
using Accela.ACA.Common.Util;
using Accela.ACA.Web.Common;
using Accela.ACA.Web.Util;
using Accela.ACA.WSProxy;
using log4net;

namespace Accela.ACA.Web.Payment
{
    /// <summary>
    /// Used to handle ETISALAT Payment.
    /// </summary>
    public class EtisalatAdapter
    {
        #region Fields

        /// <summary>
        /// key of flag indicating where the page is from.
        /// </summary>
        private const string ETISALAT_PAGEFROM_NAME = "PageFrom";

        /// <summary>
        /// value of flag indicating the page is from CAP list.
        /// </summary>
        private const string ETISALAT_PAGEFROM_VALUE_CAPLIST = "CapList";

        /// <summary>
        /// The CONID
        /// </summary>
        private const string ETISALAT_PARAM_CONID = "CONID";

        /// <summary>
        /// special key for notification URL query string
        /// </summary>
        private const string ETISALAT_POSTBACK_QUERYSTRING_KEY = "p";

        /// <summary>
        /// session key for storing model returned from EMSE
        /// </summary>
        private const string SESSION_ETISALAT_PAYMENT_RETURN_MODEL = "SESSION_ETISALAT_PAYMENT_RETURN_MODEL";
        
        /// <summary>
        /// The logger
        /// </summary>
        private static readonly ILog Logger = LogFactory.Instance.GetLogger(typeof(EtisalatAdapter));

        #endregion Fields

        #region Properties

        /// <summary>
        /// Gets System.Web.HttpContext.Current.Request.
        /// </summary>
        private static HttpRequest Request
        {
            get
            {
                return System.Web.HttpContext.Current.Request;
            }
        }

        /// <summary>
        /// Gets System.Web.HttpContext.Current.Server.
        /// </summary>
        private static HttpServerUtility Server
        {
            get
            {
                return System.Web.HttpContext.Current.Server;
            }
        }

        /// <summary>
        /// Gets System.Web.HttpContext.Current.Session.
        /// </summary>
        private static HttpSessionState Session
        {
            get
            {
                return System.Web.HttpContext.Current.Session;
            }
        }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Check online payment.
        /// </summary>
        /// <param name="moduleName">The module name</param>
        /// <param name="id1">cap id1</param>
        /// <param name="id2">cap id2</param>
        /// <param name="id3">cap id3</param>
        /// <param name="customID">custom ID</param>
        /// <param name="actionFlag">action flag</param>
        /// <param name="agencyCode">agency code</param>
        /// <param name="isPay4ExistingCapFlag">is pay for existing cap flag</param>
        /// <param name="isRenewalFlag">is renewal flag</param>
        /// <returns>online payment redirection URL</returns>
        public static string CheckPayment(string moduleName, string id1, string id2, string id3, string customID, string actionFlag, string agencyCode, string isPay4ExistingCapFlag, string isRenewalFlag)
        {
            string url = string.Empty;

            CapIDModel4WS capIdModel = new CapIDModel4WS();
            capIdModel.id1 = id1;
            capIdModel.id2 = id2;
            capIdModel.id3 = id3;
            capIdModel.customID = customID;
            capIdModel.serviceProviderCode = agencyCode;

            //check if current cap is doing amendment
            bool isAmendment = EtisalatHelper.IsDoingAmendment(capIdModel, AppSession.User.UserSeqNum);
            string isAmendmentFlag = isAmendment ? ACAConstant.COMMON_Y : ACAConstant.COMMON_N;

            //prepare BLL instance.
            IOnlinePaymenBll onlinePaymentBll = ObjectFactory.GetObject<IOnlinePaymenBll>();

            //prepare parameters
            ACAModel4WS acaModel4WS = GetACAModel4CAPList(moduleName, isAmendmentFlag);
            string notificationURL = string.Format("{0}://{1}:{2}{3}Cap/PaymentResult.aspx?{4}={5}", Request.Url.Scheme, Request.Url.Host, Request.Url.Port, FileUtil.ApplicationRoot, ETISALAT_POSTBACK_QUERYSTRING_KEY, GetQueryString4Registration(capIdModel, moduleName, false, string.Empty, isAmendment));
            Logger.InfoFormat("\nnotificationURL:{0}\n", notificationURL);

            //begin check payment status
            EMSEResultBaseModel4WS emseResult = onlinePaymentBll.OnlinePaymentProcess(capIdModel, acaModel4WS, notificationURL, AppSession.User.PublicUserId);

            //set output status
            EtisalatStatus status = EtisalatHelper.GetStatus(emseResult);

            if (emseResult != null && emseResult.result != null)
            {
                if (status == EtisalatStatus.Registered)
                {
                    Logger.InfoFormat("\nchecking result status:{0}\n", "Registered");
                    url = (string)emseResult.result;
                }
                else if (status == EtisalatStatus.RegistrationSucceeded)
                {
                    Logger.InfoFormat("\nchecking result status:{0}\n", "RegistrationSucceeded");
                    Session[SESSION_ETISALAT_PAYMENT_RETURN_MODEL] = emseResult;
                    string urlPattern = "~/Cap/CapCompletion.aspx?Module={0}&receiptNbr={1}&stepNumber={2}&isPay4ExistingCap={3}&isRenewal={4}";
                    url = string.Format(urlPattern, moduleName, string.Empty, "0", isPay4ExistingCapFlag, isRenewalFlag);
                }
                else if (status == EtisalatStatus.Paid)
                {
                    Logger.InfoFormat("\nchecking result status:{0}\n", "Paid");
                    url = GetNotificationURL(moduleName, (string)emseResult.result, isPay4ExistingCapFlag, isRenewalFlag, isAmendmentFlag, customID, id1, id2, id3);
                }
            }

            return url;
        }

        /// <summary>
        /// Complete online payment.
        /// </summary>
        /// <param name="htPostBackData">The post back data.</param>
        /// <returns>Redirection URL</returns>
        public static string CompletePayment(Hashtable htPostBackData)
        {
            //prepare parameters
            string parametersReturned = BuildQueryString(htPostBackData);
            Logger.InfoFormat("returned parameters from 3th party provider:{0}", parametersReturned);
            if (AppSession.User == null ||
                AppSession.User.IsAnonymous)
            {
                string redirectionURL = string.Format("{0}Cap/PaymentResult.aspx?{1}", FileUtil.ApplicationRoot, parametersReturned);
                
                //If user is anonymous, redirect user to login page, and return back to current url after is signed in.
                AuthenticationUtil.RedirectToLoginPage(redirectionURL, null);
                return redirectionURL;
            }
            else
            {
                string callerID = GetValueByKey(htPostBackData, "callerID");

                //check if current user id is the callerID sent back from Etisalat, if not ,redirect to home page directly.
                if (!AppSession.User.PublicUserId.Equals(callerID, StringComparison.InvariantCultureIgnoreCase))
                {
                    return ACAConstant.URL_DEFAULT;
                }
                else
                {
                    //begin prepare Pay parameters
                    CapIDModel4WS capIDModel4WS = GetCapIDModel4WS(htPostBackData);
                    ACAModel4WS acaModel4WS = GetACAModel(htPostBackData);

                    //begin invoke Pay method
                    EMSEResultBaseModel4WS emseResult = Pay(capIDModel4WS, acaModel4WS, parametersReturned, callerID);

                    //adjust return code
                    EtisalatHelper.AdjustReturnCode4Completion(emseResult);

                    //save result to session.
                    Session[SESSION_ETISALAT_PAYMENT_RETURN_MODEL] = emseResult;

                    //get query string
                    string queryString = string.Empty;
                    if (htPostBackData.Count > 0)
                    {
                        Hashtable returnValues = GetEMSEReturnValues();
                        if (returnValues != null &&
                            returnValues.ContainsKey("receiptNumber"))
                        {
                            htPostBackData["receiptNbr"] = returnValues["receiptNumber"];
                            parametersReturned = BuildQueryString(htPostBackData);
                        }

                        queryString = parametersReturned;
                    }
                    else
                    {
                        queryString = string.Format("failed=yes");
                    }

                    //return rediection URL
                    string redirectionURL = string.Format("{0}Cap/CapCompletion.aspx?{1}", FileUtil.ApplicationRoot, queryString);
                    return redirectionURL;
                }
            }
        }

        /// <summary>
        /// get EMSE return value hashtable
        /// </summary>
        /// <returns>The EMSE return value hashtable.</returns>
        public static Hashtable GetEMSEReturnValues()
        {
            Hashtable htReturnValues = new Hashtable(StringComparer.InvariantCultureIgnoreCase);
            htReturnValues.Add("capID1", string.Empty);
            htReturnValues.Add("capID2", string.Empty);
            htReturnValues.Add("capID3", string.Empty);
            htReturnValues.Add("customID", string.Empty);
            htReturnValues.Add("receiptNumber", string.Empty);
            string returnVaule = GetEMSEReturnValue();
            if (!string.IsNullOrEmpty(returnVaule))
            {
                try
                {
                    string realCapID = EtisalatHelper.ExtractValueFromQueryString(returnVaule, "capID");
                    if (!string.IsNullOrEmpty(realCapID))
                    {
                        realCapID = realCapID.Trim();
                        string[] idArray = realCapID.Split(new char[] { '-' });
                        htReturnValues["capID1"] = idArray[0];
                        htReturnValues["capID2"] = idArray[1];
                        htReturnValues["capID3"] = idArray[2];
                    }

                    htReturnValues["customID"] = EtisalatHelper.ExtractValueFromQueryString(returnVaule, "customID");
                    htReturnValues["receiptNumber"] = EtisalatHelper.ExtractValueFromQueryString(returnVaule, "receiptNumber");
                }
                catch (Exception ex)
                {
                    Logger.Error(ex.Message, ex);
                }
            }

            return htReturnValues;
        }

        /// <summary>
        /// get EMSE return message.
        /// </summary>
        /// <returns>The EMSE return message.</returns>
        public static string GetEmseReturnMessage()
        {
            EMSEResultBaseModel4WS emseResult = GetEMSEResultBaseModel4WS();

            if (emseResult == null ||
                string.IsNullOrEmpty(emseResult.returnMessage))
            {
                return string.Empty;
            }
            else
            {
                return emseResult.returnMessage;
            }
        }

        /// <summary>
        /// get paymentURL
        /// </summary>
        /// <returns>The payment url.</returns>
        public static string GetPaymentURL()
        {
            return GetEMSEReturnValue();
        }

        /// <summary>
        /// get post back data(including query string and form data) from ETISALAT web online payment
        /// </summary>
        /// <returns>the data for post data</returns>
        public static Hashtable GetPostBackData()
        {
            Hashtable htPostBackData = new Hashtable();

            foreach (string key in Request.Form.AllKeys)
            {
                AddParameters(htPostBackData, key, Request.Form[key]);
            }

            foreach (string key in Request.QueryString.AllKeys)
            {
                string value = Request.QueryString[key];
                if (ETISALAT_POSTBACK_QUERYSTRING_KEY.Equals(key, StringComparison.InvariantCultureIgnoreCase) &&
                    !string.IsNullOrEmpty(value))
                {
                    PopulatePostBackParameters(htPostBackData, value);
                }
                else
                {
                    if (!htPostBackData.ContainsKey(key))
                    {
                        AddParameters(htPostBackData, key, value);
                    }
                }
            }

            return htPostBackData;
        }

        /// <summary>
        /// initial ETISALAT online payment.
        /// </summary>
        public static void InitEtisalatOnlinePayment()
        {
            Session[SESSION_ETISALAT_PAYMENT_RETURN_MODEL] = null;
        }

        /// <summary>
        /// check if current flow used ETISALAT online payment.
        /// </summary>
        /// <returns>whether the ETISALAT online payment is used.</returns>
        public static bool IsEtisalatOnlinePaymentUsed()
        {
            EMSEResultBaseModel4WS emseResult = GetEMSEResultBaseModel4WS();
            return emseResult != null && PaymentHelper.GetPaymentAdapterType() == PaymentAdapterType.Etisalat;
        }

        /// <summary>
        /// check if payment has failed
        /// </summary>
        /// <returns>Is failed or not.</returns>
        public static bool IsFailed()
        {
            EtisalatStatus status = GetEMSEReturnStatus();
            return status == EtisalatStatus.RegistrationFailed || status == EtisalatStatus.CompletionFailed;
        }

        /// <summary>
        /// check if ETISALAT current page is from cap list.
        /// </summary>
        /// <returns>Is page from cap list or not.</returns>
        public static bool IsPageFromCapList()
        {
            return ETISALAT_PAGEFROM_VALUE_CAPLIST.Equals(Request.QueryString[ETISALAT_PAGEFROM_NAME], StringComparison.InvariantCultureIgnoreCase);
        }

        /// <summary>
        /// check if user has paid
        /// </summary>
        /// <returns>Is paid or not.</returns>
        public static bool IsPaid()
        {
            EtisalatStatus status = GetEMSEReturnStatus();
            return status == EtisalatStatus.Paid || status == EtisalatStatus.CompletionSucceeded;
        }

        /// <summary>
        /// check if user has registered.
        /// </summary>
        /// <returns>Is registered or not.</returns>
        public static bool IsRegistered()
        {
            EtisalatStatus status = GetEMSEReturnStatus();
            return status == EtisalatStatus.Registered || status == EtisalatStatus.RegistrationSucceeded;
        }

        /// <summary>
        /// check if payment has failed
        /// </summary>
        /// <returns>Is unknown or not.</returns>
        public static bool IsUnknown()
        {
            EtisalatStatus status = GetEMSEReturnStatus();
            return status == EtisalatStatus.Unknown;
        }

        /// <summary>
        /// Register online payment.
        /// </summary>
        /// <param name="input">ETISALAT Registration Input</param>
        /// <returns>ETISALAT Registration Output</returns>
        public static EtisalatRegistrationOutput RegisterPayment(EtisalatRegistrationInput input)
        {
            EtisalatRegistrationOutput output = new EtisalatRegistrationOutput();

            //prepare BLL instance.
            IOnlinePaymenBll onlinePaymentBll = ObjectFactory.GetObject<IOnlinePaymenBll>();

            //prepare parameters
            CapIDModel4WS capIDModel4WS = AppSession.GetCapModelFromSession(input.ModuleName).capID;

            //check if current cap is doing amendment
            bool isAmendment = EtisalatHelper.IsDoingAmendment(capIDModel4WS, AppSession.User.UserSeqNum);
            string isAmendmentFlag = isAmendment ? ACAConstant.COMMON_Y : ACAConstant.COMMON_N;
            string isPay4ExistingCapFlag = input.IsPay4ExistingCap ? ACAConstant.COMMON_Y : ACAConstant.COMMON_N;
            ACAModel4WS acaModel4WS = GetACAModel(input.ModuleName);
            acaModel4WS.strAction = EtisalatHelper.GetActionSource(Request["isRenewal"], isPay4ExistingCapFlag, isAmendmentFlag);

            //if is super agency,the receiptNbr need be got with child agence in other page.So give a string.Empty value.
            if (!input.IsMultiCAP)
            {
                IOnlinePaymenBll _paymentBll = ObjectFactory.GetObject<IOnlinePaymenBll>();
                output.ReceiptNbr = _paymentBll.GetReceiptNoByCapID(capIDModel4WS, null, AppSession.User.PublicUserId);
            }

            string notificationURL = string.Format("{0}://{1}:{2}{3}Cap/PaymentResult.aspx?{4}={5}", Request.Url.Scheme, Request.Url.Host, Request.Url.Port, FileUtil.ApplicationRoot, ETISALAT_POSTBACK_QUERYSTRING_KEY, GetQueryString4Registration(capIDModel4WS, input.ModuleName, input.IsPay4ExistingCap, output.ReceiptNbr, isAmendment));
            
            //begin register
            EMSEResultBaseModel4WS emseResult = onlinePaymentBll.OnlinePaymentProcess(capIDModel4WS, acaModel4WS, notificationURL, AppSession.User.PublicUserId);
            
            //set output status
            output.Status = EtisalatHelper.GetStatus(emseResult);

            //handle returned message
            switch (output.Status)
            {
                case EtisalatStatus.RegistrationSucceeded:
                    //if succeeded, redirect to another page.
                    Session[SESSION_ETISALAT_PAYMENT_RETURN_MODEL] = emseResult;
                    break;
                case EtisalatStatus.Registered:
                    //if registered, redirect to another page.
                    output.RedirectionURL = (string)emseResult.result;
                    break;
                case EtisalatStatus.Paid:
                    string conID = EtisalatHelper.ExtractValueFromURL((string)emseResult.result, ETISALAT_PARAM_CONID);

                    //if authorized, redirect to another page.
                    output.RedirectionURL = string.Format("{0}&{1}={2}", notificationURL, ETISALAT_PARAM_CONID, conID);
                    break;
                default:
                    output.ErrorMessage = emseResult.returnMessage;
                    break;
            }

            return output;
        }

        /// <summary>
        /// Add key/value to parameters hash table.
        /// </summary>
        /// <param name="parameters">hash table to store key/value</param>
        /// <param name="key">The key</param>
        /// <param name="value">The value</param>
        private static void AddParameters(Hashtable parameters, string key, string value)
        {
            if (parameters == null || string.IsNullOrEmpty(key))
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
        /// build query string based on post back data.
        /// </summary>
        /// <param name="htPostBackData">The post back data</param>
        /// <returns>The query string</returns>
        private static string BuildQueryString(Hashtable htPostBackData)
        {
            if (htPostBackData == null ||
                htPostBackData.Count == 0)
            {
                return string.Empty;
            }

            StringBuilder sb = new StringBuilder();
            foreach (string key in htPostBackData.Keys)
            {
                if (sb.Length > 0)
                {
                    sb.Append("&");
                }

                string tempValue = htPostBackData[key] == null ? string.Empty : htPostBackData[key].ToString();
                sb.Append(string.Format("{0}={1}", key, Server.UrlEncode(tempValue)));
            }

            return sb.ToString();
        }

        /// <summary>
        /// Construct ACAModel.
        /// </summary>
        /// <param name="moduleName">module name</param>
        /// <returns>The ACAModel4WS</returns>
        private static ACAModel4WS GetACAModel(string moduleName)
        {
            // construct an ACA model.
            ACAModel4WS acaModel = new ACAModel4WS();
            acaModel.servProvCode = ConfigManager.AgencyCode;
            acaModel.callerID = AppSession.User.PublicUserId;
            acaModel.module = moduleName;
            BizDomainModel4WS[] workflowTasks = StandardChoiceUtil.GetWorkflowTasks(moduleName);
            acaModel.workflowTasks = workflowTasks;
            acaModel.taskDispositionComment = string.Empty;
            acaModel.strAction = string.Empty;

            //get the CAP 's status after apply a new permit.
            string capStatus = StandardChoiceUtil.GetCapStatusAfterApply(moduleName);
            acaModel.capStateAfterApply = capStatus;

            //set the serverName
            acaModel.serverName = Request.UserHostName;

            // set task disposition comment
            string comment = StandardChoiceUtil.GetTaskDispositionComment(moduleName);
            acaModel.taskDispositionComment = comment;

            SysUserModel4WS sysUserModel = new SysUserModel4WS();
            sysUserModel.firstName = AppSession.User.FirstName;
            sysUserModel.lastName = AppSession.User.LastName;
            sysUserModel.agencyCode = ConfigManager.AgencyCode;
            sysUserModel.email = AppSession.User.Email;
            sysUserModel.userID = AppSession.User.UserID;
            sysUserModel.fullName = AppSession.User.FullName;
            acaModel.suModel = sysUserModel;

            return acaModel;
        }

        /// <summary>
        /// get ACAModel for WS.
        /// </summary>
        /// <param name="htPostBackData">The post back data</param>
        /// <returns>The ACAModel4WS</returns>
        private static ACAModel4WS GetACAModel(Hashtable htPostBackData)
        {
            // construct an ACA model.
            ACAModel4WS acaModel = new ACAModel4WS();
            acaModel.servProvCode = ConfigManager.AgencyCode;
            acaModel.callerID = GetValueByKey(htPostBackData, "callerID");
            acaModel.module = GetModuleName(htPostBackData);
            acaModel.strAction = EtisalatHelper.GetActionSource(GetValueByKey(htPostBackData, "isRenewal"), GetValueByKey(htPostBackData, "isPay4ExistingCap"), GetValueByKey(htPostBackData, "isAmendment"));
            
            //set the serverName
            acaModel.serverName = Request.UserHostName;

            SysUserModel4WS sysUserModel = new SysUserModel4WS();
            sysUserModel.firstName = AppSession.User.FirstName;
            sysUserModel.lastName = AppSession.User.LastName;
            sysUserModel.agencyCode = ConfigManager.AgencyCode;
            sysUserModel.email = AppSession.User.Email;
            sysUserModel.userID = AppSession.User.UserID;
            sysUserModel.fullName = AppSession.User.FullName;
            acaModel.suModel = sysUserModel;

            return acaModel;
        }

        /// <summary>
        /// Construct ACAModel.
        /// </summary>
        /// <param name="moduleName">The module name.</param>
        /// <param name="isAmendmentFlag">is amendment.</param>
        /// <returns>The ACAModel4WS</returns>
        private static ACAModel4WS GetACAModel4CAPList(string moduleName, string isAmendmentFlag)
        {
            // construct an ACA model.
            ACAModel4WS acaModel = new ACAModel4WS();
            acaModel.servProvCode = ConfigManager.AgencyCode;
            acaModel.callerID = AppSession.User.PublicUserId;
            acaModel.module = moduleName;
            acaModel.taskDispositionComment = string.Empty;
            acaModel.strAction = EtisalatHelper.GetActionSource(ACAConstant.COMMON_N, ACAConstant.COMMON_N, isAmendmentFlag);

            //set the serverName
            acaModel.serverName = Request.UserHostName;

            SysUserModel4WS sysUserModel = new SysUserModel4WS();
            sysUserModel.firstName = AppSession.User.FirstName;
            sysUserModel.lastName = AppSession.User.LastName;
            sysUserModel.agencyCode = ConfigManager.AgencyCode;
            sysUserModel.email = AppSession.User.Email;
            sysUserModel.userID = AppSession.User.UserID;
            sysUserModel.fullName = AppSession.User.FullName;
            acaModel.suModel = sysUserModel;

            return acaModel;
        }

        /// <summary>
        /// get cap id model for WS
        /// </summary>
        /// <param name="htPostBackData">The post back data</param>
        /// <returns>The CapIDModel4WS</returns>
        private static CapIDModel4WS GetCapIDModel4WS(Hashtable htPostBackData)
        {
            CapIDModel4WS capIDModel4WS = new CapIDModel4WS();
            capIDModel4WS.customID = GetValueByKey(htPostBackData, "customID");
            capIDModel4WS.id1 = GetValueByKey(htPostBackData, "id1");
            capIDModel4WS.id2 = GetValueByKey(htPostBackData, "id2");
            capIDModel4WS.id3 = GetValueByKey(htPostBackData, "id3");
            capIDModel4WS.serviceProviderCode = ConfigManager.AgencyCode;
            return capIDModel4WS;
        }

        /// <summary>
        /// get EMSEResultBaseModel4WS from session which was stored in <c>CapPayment.aspx</c> or <c>PaymentResult.aspx</c> page.
        /// </summary>
        /// <returns>The EMSE result base model.</returns>
        private static EMSEResultBaseModel4WS GetEMSEResultBaseModel4WS()
        {
            return Session[SESSION_ETISALAT_PAYMENT_RETURN_MODEL] as EMSEResultBaseModel4WS;
        }

        /// <summary>
        /// get ETISALAT status based on EMSE return code.
        /// </summary>
        /// <returns>The EMSE return status.</returns>
        private static EtisalatStatus GetEMSEReturnStatus()
        {
            EMSEResultBaseModel4WS emseResult = GetEMSEResultBaseModel4WS();
            return EtisalatHelper.GetStatus(emseResult);
        }

        /// <summary>
        /// get EMSE return value.
        /// </summary>
        /// <returns>The EMSE return value.</returns>
        private static string GetEMSEReturnValue()
        {
            EMSEResultBaseModel4WS emseResult = GetEMSEResultBaseModel4WS();

            if (emseResult == null || emseResult.result == null)
            {
                return string.Empty;
            }
            
            return (string)emseResult.result;
        }

        /// <summary>
        /// Gets the module name from request.
        /// </summary>
        /// <param name="htPostBackData">The post back data</param>
        /// <returns>module name</returns>
        private static string GetModuleName(Hashtable htPostBackData)
        {
            string moduleName = GetValueByKey(htPostBackData, ACAConstant.MODULE_NAME);

            if (moduleName != null &&
                moduleName.IndexOf(",") > 0)
            {
                moduleName = moduleName.Split(new char[] { ',' })[0];
            }

            return moduleName;
        }

        /// <summary>
        /// Get Notification URL
        /// </summary>
        /// <param name="moduleName">The module name.</param>
        /// <param name="returnedURL">The returned URL.</param>
        /// <param name="isPay4ExistingCapFlag">Is pay for existing cap.</param>
        /// <param name="isRenewalString">is renewal</param>
        /// <param name="isAmendmentFlag">is amendment</param>
        /// <param name="customID">The custom id.</param>
        /// <param name="id1">The cap id1</param>
        /// <param name="id2">The cap id2</param>
        /// <param name="id3">The cap id3</param>
        /// <returns>The notification URL</returns>
        private static string GetNotificationURL(string moduleName, string returnedURL, string isPay4ExistingCapFlag, string isRenewalString, string isAmendmentFlag, string customID, string id1, string id2, string id3)
        {
            if (!string.IsNullOrEmpty(returnedURL))
            {
                try
                {
                    string conID = EtisalatHelper.ExtractValueFromURL(returnedURL, ETISALAT_PARAM_CONID);
                    string receiptNbr = string.Empty;
                    string pattern = "UserSeqNum|{0}|isMultiCAP|{1}|callerID|{2}|customID|{3}|id1|{4}|id2|{5}|id3|{6}|Module|{7}|receiptNbr|{8}|stepNumber|{9}|isPay4ExistingCap|{10}|isRenewal|{11}|isAmendment|{12}";
                    string newQueryString = string.Format(pattern, AppSession.User.UserSeqNum, ACAConstant.COMMON_N, AppSession.User.PublicUserId, EtisalatHelper.Encode(customID), EtisalatHelper.Encode(id1), EtisalatHelper.Encode(id2), EtisalatHelper.Encode(id3), EtisalatHelper.Encode(moduleName), EtisalatHelper.Encode(receiptNbr), 0, isPay4ExistingCapFlag, isRenewalString, isAmendmentFlag);
                    newQueryString = Server.UrlEncode(newQueryString);

                    //begin assemble notification URL
                    string notificationURL = string.Format("{0}://{1}:{2}{3}Cap/PaymentResult.aspx?{4}={5}&{6}={7}&{8}={9}", Request.Url.Scheme, Request.Url.Host, Request.Url.Port, FileUtil.ApplicationRoot, ETISALAT_PAGEFROM_NAME, ETISALAT_PAGEFROM_VALUE_CAPLIST, ETISALAT_PARAM_CONID, conID, ETISALAT_POSTBACK_QUERYSTRING_KEY, newQueryString);

                    return notificationURL;
                }
                catch (Exception ex)
                {
                    Logger.Error(ex.Message, ex);
                }
            }

            return string.Empty;
        }

        /// <summary>
        /// Get query string for registration.
        /// </summary>
        /// <param name="capIDModel4WS">The capID Model</param>
        /// <param name="moduleName">The module name</param>
        /// <param name="isPay4ExistingCap">is pay for existing Cap</param>
        /// <param name="receiptNbr">receipt number</param>
        /// <param name="isAmendment">is amendment</param>
        /// <returns>QueryString for registration</returns>
        private static string GetQueryString4Registration(CapIDModel4WS capIDModel4WS, string moduleName, bool isPay4ExistingCap, string receiptNbr, bool isAmendment)
        {
            // get the step number for parameter.
            string preStepNumber = Request.QueryString["stepNumber"];
            bool isNumber = ValidationUtil.IsNumber(preStepNumber);
            int stepNumber = 0;

            if (isNumber)
            {
                stepNumber = int.Parse(preStepNumber) + 1;
            }

            //if existing cap to pay fee
            string isPay4ExistingCapFlag = isPay4ExistingCap ? ACAConstant.COMMON_Y : ACAConstant.COMMON_N;
            string isRenewalFlag = ACAConstant.COMMON_Y == Request["isRenewal"] ? ACAConstant.COMMON_Y : ACAConstant.COMMON_N;
            string isAmendmentFlag = isAmendment ? ACAConstant.COMMON_Y : ACAConstant.COMMON_N;

            string isMultiCAPString = ACAConstant.COMMON_N;
            string pattern = "UserSeqNum|{0}|isMultiCAP|{1}|callerID|{2}|customID|{3}|id1|{4}|id2|{5}|id3|{6}|Module|{7}|receiptNbr|{8}|stepNumber|{9}|isPay4ExistingCap|{10}|isRenewal|{11}|isAmendment|{12}";
            string queryString = string.Format(pattern, EtisalatHelper.Encode(AppSession.User.UserSeqNum), EtisalatHelper.Encode(isMultiCAPString), EtisalatHelper.Encode(AppSession.User.PublicUserId), EtisalatHelper.Encode(capIDModel4WS.customID), EtisalatHelper.Encode(capIDModel4WS.id1), EtisalatHelper.Encode(capIDModel4WS.id2), EtisalatHelper.Encode(capIDModel4WS.id3), EtisalatHelper.Encode(moduleName), EtisalatHelper.Encode(receiptNbr), stepNumber, EtisalatHelper.Encode(isPay4ExistingCapFlag), EtisalatHelper.Encode(isRenewalFlag), EtisalatHelper.Encode(isAmendmentFlag));
            queryString = Server.UrlEncode(queryString);
            return queryString;
        }

        /// <summary>
        /// get value from hashtable
        /// </summary>
        /// <param name="htPostBackData">the post back data</param>
        /// <param name="key">The key</param>
        /// <returns>The value</returns>
        private static string GetValueByKey(Hashtable htPostBackData, string key)
        {
            if (htPostBackData == null ||
                htPostBackData.Count == 0)
            {
                return string.Empty;
            }

            if (htPostBackData.ContainsKey(key))
            {
                return htPostBackData[key] == null ? string.Empty : htPostBackData[key].ToString();
            }

            return string.Empty;
        }

        /// <summary>
        /// invoke PayOnline.
        /// </summary>
        /// <param name="capIDModel4WS">capID model</param>
        /// <param name="acaModel4WS">aca model</param>
        /// <param name="parametersReturned">parameters returned</param>
        /// <param name="callerID">caller ID</param>
        /// <returns>EMSE result base model</returns>
        private static EMSEResultBaseModel4WS Pay(CapIDModel4WS capIDModel4WS, ACAModel4WS acaModel4WS, string parametersReturned, string callerID)
        {
            IOnlinePaymenBll onlinePaymentBll = ObjectFactory.GetObject<IOnlinePaymenBll>();
            return onlinePaymentBll.CallEMSEtoPayOnline(capIDModel4WS, acaModel4WS, parametersReturned, callerID);
        }

        /// <summary>
        /// populate post back parameters.
        /// </summary>
        /// <param name="htPostBackData">hash table to store post back data</param>
        /// <param name="parameters">post back parameters</param>
        private static void PopulatePostBackParameters(Hashtable htPostBackData, string parameters)
        {
            string subQueryString = Server.UrlDecode(parameters);
            string[] subQueryStringArray = subQueryString.Split(new char[] { '|' });
            if (subQueryStringArray != null)
            {
                for (int i = 0; i < subQueryStringArray.Length; i++)
                {
                    string subKey = subQueryStringArray[i];
                    string subValue = EtisalatHelper.Decode(subQueryStringArray[i + 1]);
                    i++;
                    if (!htPostBackData.ContainsKey(subKey))
                    {
                        AddParameters(htPostBackData, subKey, subValue);
                    }
                }
            }
        }

        #endregion Methods
    }
}
