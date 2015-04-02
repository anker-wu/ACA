#region

/**
 * <pre>
 * 
 *  Accela Citizen Access
 *  File: CommUtil.cs
 * 
 *  Accela, Inc.
 *  Copyright (C): 2014
 * 
 *  Description:
 *  
 *  Notes:
 *      $Id:PublicuserController.cs 77905 2014-06-11 12:49:28Z ACHIEVO\dennis.fu $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion

using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Caching;
using Accela.ACA.BLL.Account;
using Accela.ACA.BLL.Attachment;
using Accela.ACA.BLL.Cap;
using Accela.ACA.BLL.Common;
using Accela.ACA.BLL.Inspection;
using Accela.ACA.Common;
using Accela.ACA.Common.Common;
using Accela.ACA.Common.Util;
using Accela.ACA.Inspection;
using Accela.ACA.Web.Common;
using Accela.ACA.Web.Inspection;
using Accela.ACA.Web.Util;
using Accela.ACA.WSProxy;

namespace Accela.ACA.Web.WebApi
{
    /// <summary>
    /// API UTIL
    /// </summary>
    public class ApiUtil
    {
        /// <summary>
        ///  INT Timeout
        /// </summary>
        public static readonly int Timeout = 5000;

        /// <summary>
        /// Default User Agent
        /// </summary>
        private static readonly string DefaultUserAgent = "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; SV1; .NET CLR 1.1.4322; .NET CLR 2.0.50727)";

        /// <summary>
        /// Default Encoding
        /// </summary>
        private static Encoding defaultEncoding = Encoding.UTF8;

        /// <summary>
        /// POST method.
        /// </summary>
        /// <param name="url">post URL</param>
        /// <param name="postData">post data</param>
        /// <param name="timeout">Timeout. The default is 5000</param>
        /// <param name="encoding">The encoding format, the default format UTF-8</param>
        /// <returns>string post data</returns>
        public static string Post(string url, string postData, int? timeout = null, Encoding encoding = null)
        {
            encoding = encoding ?? Encoding.UTF8;
            timeout = timeout ?? ApiUtil.Timeout;
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            request.Timeout = timeout.Value;

            byte[] data = encoding.GetBytes(postData);
            request.ContentLength = data.Length;
            using (Stream stream = request.GetRequestStream())
            {
                stream.Write(data, 0, data.Length);
            }

            HttpWebResponse response = null;
            try
            {
                using (response = (HttpWebResponse)request.GetResponse())
                {
                    using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                    {
                        return reader.ReadToEnd();
                    }
                }
            }
            catch (WebException e)
            {
                using (WebResponse errorResponse = e.Response)
                {
                    HttpWebResponse httpResponse = (HttpWebResponse)errorResponse;
                    using (Stream dataStream = httpResponse.GetResponseStream())
                    using (var reader = new StreamReader(dataStream))
                    {
                        string text = reader.ReadToEnd();
                        throw new Exception(text);
                    }
                }
            }
        }

        /// <summary>
        /// HTTP Get.
        /// </summary>
        /// <param name="url">get url</param>
        /// <param name="timeout">Timeout. The default is 5000</param>
        /// <param name="encoding">The encoding format, the default format UTF-8</param>
        /// <returns>Get Data</returns>
        public static string Get(string url, int? timeout = null, Encoding encoding = null)
        {
            try
            {
                timeout = timeout ?? ApiUtil.Timeout;
                encoding = encoding ?? ApiUtil.defaultEncoding;
                Uri uri = new Uri(url);
                System.Net.HttpWebRequest request = (System.Net.HttpWebRequest)System.Net.WebRequest.Create(uri);
                request.Method = "GET";
                request.ContentType = "application/x-www-form-urlencoded";
                request.AllowAutoRedirect = false;
                request.Timeout = timeout.Value;
                System.Net.HttpWebResponse response = (System.Net.HttpWebResponse)request.GetResponse();
                Stream responseStream = response.GetResponseStream();
                StreamReader readStream = new StreamReader(responseStream, encoding);
                string retext = readStream.ReadToEnd().ToString();
                readStream.Close();
                return retext;
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }
        }

        #region Cache

        /// <summary>
        /// Cache Data
        /// </summary>
        /// <param name="data">Cache Date</param>
        /// <param name="key">Cache key</param>
        public static void AddCache(object data, string key)
        {
            if (string.IsNullOrEmpty(key))
            {
                return;
            }

            Cache cahe = HttpContext.Current.Cache ?? new Cache();

            //default 2 Hours
            cahe.Insert(key, data, null, DateTime.Now.AddHours(2), TimeSpan.Zero);
        }

        /// <summary>
        /// get Cache
        /// </summary>
        /// <param name="key">Cache key</param>
        /// <returns>object cache</returns>
        public static object GetCache(string key)
        {
            string publicUserIdString = key;
            object obj = HttpContext.Current.Cache[publicUserIdString];
            return obj;
        }
         
        /// <summary>
        /// add Data t
        /// </summary>
        /// <param name="data">object data</param>
        /// <param name="key">string key</param>
        public static void AddSession(object data, string key)
        {
            if (string.IsNullOrEmpty(key))
            {
                return;
            }

            HttpContext.Current.Session.Add(key, data);
        }

        /// <summary>
        /// Get Session
        /// </summary>
        /// <param name="key"> session key</param>
        /// <returns>value Session</returns>
        public static object GetSession(string key)
        {
            string publicUserIdString = key;
            object obj = HttpContext.Current.Session[publicUserIdString];
            return obj;
        }

        #endregion

        #region Global Srarch

        /// <summary>
        /// Display Amount Plus
        /// </summary>
        /// <param name="amountPlus">amount Plus</param>
        /// <returns>Amount Plus</returns>
        public static string DisplayAmountPlus(int amountPlus)
        {
            if (amountPlus < 100)
            {
                return amountPlus.ToString();
            }

            int result = amountPlus % (ACAConstant.DEFAULT_PAGESIZE * ACAConstant.DEFAULT_PAGECOUNT);
            if (result == 1)
            {
                return (amountPlus - 1) + "+";
            }

            return amountPlus.ToString();
        }
        #endregion

        #region cap
        /// <summary>
        /// Construct Record Detail URL
        /// </summary>
        /// <param name="capModel">cap Model</param>
        /// <returns>Record Detail URL</returns>
        public static string ConstructRecordDetailUrl(SimpleCapModel capModel)
        {
            var moduleName = capModel.moduleName;

            var capIDModel = new CapIDModel4WS
            {
                id1 = capModel.capID.ID1,
                id2 = capModel.capID.ID2,
                id3 = capModel.capID.ID3,
                serviceProviderCode = capModel.capType.serviceProviderCode
            };

            var url = string.Format(
                "~/Cap/CapDetail.aspx?Module={0}&TabName={0}&capID1={1}&capID2={2}&capID3={3}&{4}={5}&{6}={7}",
                ScriptFilter.AntiXssUrlEncode(moduleName),
                capIDModel.id1,
                capIDModel.id2,
                capIDModel.id3,
                UrlConstant.AgencyCode,
                HttpUtility.UrlEncode(capIDModel.serviceProviderCode),
                ACAConstant.IS_TO_SHOW_INSPECTION,
                true);

            return url;
        }

        /// <summary>
        /// Construct Record Detail URL
        /// </summary>
        /// <param name="moduleName">module Name</param>
        /// <param name="capID1">capID 1</param>
        /// <param name="capID2">capID 2</param>
        /// <param name="capID3">capID 3</param>
        /// <param name="serviceProviderCode">service Provider Code</param>
        /// <returns>Record Detail URL</returns>
        public static string ConstructRecordDetailUrl(string moduleName, string capID1, string capID2, string capID3, string serviceProviderCode)
        {
            var url = string.Format(
                "~/Cap/CapDetail.aspx?Module={0}&TabName={0}&capID1={1}&capID2={2}&capID3={3}&{4}={5}&{6}={7}",
                ScriptFilter.AntiXssUrlEncode(moduleName),
                capID1,
                capID2,
                capID3,
                UrlConstant.AgencyCode,
                HttpUtility.UrlEncode(serviceProviderCode),
                ACAConstant.IS_TO_SHOW_INSPECTION,
                true);
            return url;
        }

        /// <summary>
        /// Gets the row button CommandArgument.
        /// </summary>
        /// <param name="capIDModel">The CapIDModel.</param>
        /// <param name="actionFlag">The action flag.</param>
        /// <param name="externalFlag">The external flag, default value is null.</param>
        /// <returns>Return the row button CommandArgument</returns>
        public static string GetRowButtonCommandArgument(CapIDModel4WS capIDModel, string actionFlag, string externalFlag = null)
        {
            string result = string.Format(
                "{1}{0}{2}{0}{3}{0}{4}{0}{5}",
                ACAConstant.COMMA,
                capIDModel.id1,
                capIDModel.id2,
                capIDModel.id3,
                actionFlag,
                capIDModel.serviceProviderCode);

            if (!string.IsNullOrEmpty(externalFlag))
            {
                result += ACAConstant.COMMA + externalFlag;
            }

            return result;
        }

        /// <summary>
        /// Construct Pay Fees URL
        /// </summary>
        /// <param name="moduleName">module Name</param>
        /// <param name="capID1">capID 1</param>
        /// <param name="capID2">capID 2</param>
        /// <param name="capID3">capID 3</param>
        /// <param name="serviceProviderCode">service Provider Code</param>
        /// <returns>string Pay Fees URL</returns>
        public static string ConstructPayFeesDeepLink(string moduleName, string capID1, string capID2, string capID3, string serviceProviderCode)
        {
            var url = string.Format(
                "urlrouting.ashx?type=1009&permitType=PayFees&agencyCode={0}&Module={1}&capID1={2}&capID2={3}&capID3={4}&stepNumber=0&isPay4ExistingCap=Y",
                HttpUtility.UrlEncode(serviceProviderCode),
                ScriptFilter.AntiXssUrlEncode(moduleName),
                capID1,
                capID2,
                capID3);

            return url;
        }

        /// <summary>
        /// Construct PayFeeDue Url
        /// </summary>
        /// <param name="moduleName">module Name</param>
        /// <param name="capID1">capID 1</param>
        /// <param name="capID2">capID 2</param>
        /// <param name="capID3">capID 3</param>
        /// <param name="serviceProviderCode">service Provider Code</param>
        /// <returns>PayFeeDue Url</returns>
        public static string ConstructPayFeeDue4RenewDeepLink(string moduleName, string capID1, string capID2, string capID3, string serviceProviderCode)
        {
            var url = string.Format(
                "urlrouting.ashx?type=1010&permitType=PayFees&agencyCodeParam={0}&Module={1}&capID1={2}&capID2={3}&capID3={4}&stepNumber=0&isPay4ExistingCap=Y&isRenewal=Y",
                HttpUtility.UrlEncode(serviceProviderCode),
                ScriptFilter.AntiXssUrlEncode(moduleName),
                capID1,
                capID2,
                capID3);

            return url;
        }

        /// <summary>
        /// Construct Resume DeepLink
        /// </summary>
        /// <param name="moduleName">module Name</param>
        /// <param name="capID1">capID 1</param>
        /// <param name="capID2">capID 2</param>
        /// <param name="capID3">capID 3</param>
        /// <param name="serviceProviderCode">service Provider Code</param>
        /// <param name="capClass">cap Class</param>
        /// <param name="filterName">filter Name</param>
        /// <returns>Resume DeepLink</returns>
        public static string ConstructResumeDeepLink(string moduleName, string capID1, string capID2, string capID3, string serviceProviderCode, string capClass, string filterName)
        {
            var url = string.Format(
                "urlrouting.ashx?type=1005&permitType=resume&agencyCode={0}&Module={1}&capID1={2}&capID2={3}&capID3={4}&isFeeEstimator={5}&FilterName={6}&stepNumber=2&pageNumber=1",
                HttpUtility.UrlEncode(serviceProviderCode),
                ScriptFilter.AntiXssUrlEncode(moduleName),
                capID1,
                capID2,
                capID3,
                capClass == ACAConstant.INCOMPLETE ? ACAConstant.COMMON_Y : ACAConstant.COMMON_N,
                filterName);

            return url;
        }

        /// <summary>
        /// get filter name for resume application
        /// </summary>
        /// <param name="capTypeModel">Cap type model.</param>
        /// <param name="moduleName">module Name</param>
        /// <returns>filter name.</returns>
        public static string GetFilterNameForResume(CapTypeModel capTypeModel, string moduleName)
        {
            string filterName = string.Empty;
            ICapTypeBll capTypeBll = ObjectFactory.GetObject<ICapTypeBll>();
            bool isTradeLicenseCap = capTypeBll.IsMatchTheFilter(capTypeModel, moduleName, ACAConstant.REQUEST_PARMETER_TRADE_LICENSE);

            if (isTradeLicenseCap)
            {
                filterName = ACAConstant.REQUEST_PARMETER_TRADE_LICENSE;
            }
            else
            {
                bool isTradeNameCap = capTypeBll.IsMatchTheFilter(capTypeModel, moduleName, ACAConstant.REQUEST_PARMETER_TRADE_NAME);

                if (isTradeNameCap)
                {
                    filterName = ACAConstant.REQUEST_PARMETER_TRADE_NAME;
                }
            }

            return filterName;
        }

        #endregion

        #region LP

        /// <summary>
        /// Construct license professional detail url
        /// </summary>
        /// <param name="licenseNumber">license number</param>
        /// <param name="licenseType">license type</param>
        /// <returns>license detail url</returns>
        public static string ConstructLpDetailUrl(string licenseNumber, string licenseType)
        {
            return string.Format(
                CultureInfo.InvariantCulture,
                "~/GeneralProperty/LicenseeDetail.aspx?LicenseeNumber={0}&LicenseeType={1}",
                UrlEncode(licenseNumber),
                UrlEncode(licenseType));
        }

        #endregion

        #region APO

        /// <summary>
        /// Construct Parcel Detail URL
        /// </summary>
        /// <param name="parcelNumber">parcel Number</param>
        /// <param name="parcelSeqNbr">parcel Sequence</param>
        /// <param name="addressSeqNumber">address Sequence Number</param>
        /// <param name="addressSourceNumber">address Source Number</param>
        /// <returns>Parcel Detail URL</returns>
        public static string ConstructParceDetailUrl(string parcelNumber, string parcelSeqNbr, string addressSeqNumber, string addressSourceNumber)
        {
            return string.Format(
                CultureInfo.InvariantCulture,
                "~/APO/ParcelDetail.aspx?{0}={1}&{2}={3}&{4}={5}&{6}={7}&{8}={9}",
                ACAConstant.REQUEST_PARMETER_PARCEL_NUMBER,
                UrlEncode(parcelNumber),
                ACAConstant.REQUEST_PARMETER_PARCEL_SEQUENCE,
                UrlEncode(parcelSeqNbr),
                ACAConstant.REQUEST_PARMETER_REFADDRESS_ID,
                UrlEncode(addressSeqNumber),
                ACAConstant.REQUEST_PARMETER_ADDRESS_SEQUENCE,
                UrlEncode(addressSourceNumber),
                ACAConstant.REQUEST_PARMETER_PARCEL_UID,
                string.Empty);
        }

        #endregion

        #region Shopping Cart

        /// <summary>
        /// Get Process Type
        /// </summary>
        /// <param name="capClass">cap Class</param>
        /// <param name="renewalStatus">renewal Status</param>
        /// <param name="hasNopaidFee">has No paid Fee</param>
        /// <param name="capId">cap Id</param>
        /// <returns>Process Type</returns>
        public static string GetProcessType(string capClass, string renewalStatus, bool hasNopaidFee, CapIDModel4WS capId)
        {
            if (capId == null)
            {
                return string.Empty;
            }

            string agencyCode = capId.serviceProviderCode;
            if (!CapUtil.IsPartialCap(capClass))
            {
                if (ACAConstant.PAYFEEDUE_RENEWAL.Equals(renewalStatus, StringComparison.InvariantCulture))
                {
                    //1.Get PayFeeDue4Renewal Cap.
                    ICapBll capBll = (ICapBll)ObjectFactory.GetObject(typeof(ICapBll));
                    ShoppingCartItemModel4WS cartItem = new ShoppingCartItemModel4WS();

                    //1. Get child cap id by params cap id.
                    CapIDModel4WS childCapID = capBll.GetChildCapIDByMasterID(capId, ACAConstant.CAP_RENEWAL, ACAConstant.RENEWAL_INCOMPLETE, false);
                    if (childCapID != null)
                    {
                        return childCapID.serviceProviderCode;
                    }
                }
                else if (ACAConstant.RENEWAL_INCOMPLETE.Equals(renewalStatus, StringComparison.InvariantCulture))
                {
                    ShoppingCartItemModel4WS cartItem = new ShoppingCartItemModel4WS();
                    ICapBll capBll = (ICapBll)ObjectFactory.GetObject(typeof(ICapBll));

                    //Check the status of the renewal cap to see if it can be added to shopping cart .
                    CapModel4WS childCap = capBll.GetCapByRelationshipPartialCap(capId, ACAConstant.CAP_RENEWAL, ACAConstant.RENEWAL_INCOMPLETE);
                    if (childCap == null ||
                        !string.Equals(ACAConstant.INCOMPLETE_EST, childCap.capClass, StringComparison.InvariantCulture))
                    {
                        //If a renewal cap is not a full partial cap, we still need to check whether this cap is a pay fee due cap. 
                        if (hasNopaidFee && !StandardChoiceUtil.IsRemovePayFee(agencyCode))
                        {
                            return ACAConstant.CAP_PAYFEEDUE;
                        }
                    }
                    else
                    {
                        return ACAConstant.CAP_RENEWAL;
                    }
                }
                else if (hasNopaidFee && !StandardChoiceUtil.IsRemovePayFee(agencyCode))
                {
                    return ACAConstant.CAP_PAYFEEDUE;
                }
            }

            return string.Empty;
        }

        #endregion

        #region CapModel4WS

        /// <summary>
        /// Get  CapModel4 WS
        /// </summary>
        /// <param name="capIdModel">CAPID collection   [cap1,cap2,cap3,ProviderCode]</param>
        /// <returns>Cap Model4WS</returns>
        public static CapModel4WS GetCapModel4Ws(CapIDModel4WS capIdModel)
        {
            CapModel4WS capModel = null;

            CapWithConditionModel4WS capWithConditionModel = CapUtil.GetCapWithConditionModel4WS(capIdModel, AppSession.User.UserSeqNum, true);

            if (capWithConditionModel != null)
            {
                capModel = capWithConditionModel.capModel;
            }

            return capModel;
        }

        /// <summary>
        /// Get CapModel4WS by CapID
        /// </summary>
        /// <param name="capID1">capID 1</param>
        /// <param name="capID2">capID 2</param>
        /// <param name="capID3">capID 3</param>
        /// <param name="agencyCode">Agency Code</param>
        /// <returns>CapModel4WS by CapID</returns>
        public static SimpleCapModel GetCapModel4WsByCapID(string capID1, string capID2, string capID3, string agencyCode)
        {
            CapModel4WS capModel;
            CapIDModel4WS capIdModel = new CapIDModel4WS
            {
                id1 = capID1,
                id2 = capID2,
                id3 = capID3,
                serviceProviderCode = agencyCode
            };

            try
            {
                SimpleCapModel simpleCapModel = GetRecordById(capIdModel, out capModel);
                return simpleCapModel;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        /// <summary>
        /// Get Simple Cap Model by cap Id
        /// </summary>
        /// <param name="capID">the cap Id model.</param>
        /// <param name="capModel">out CapModel4WS</param>
        /// <returns>Simple Cap Model by cap Id</returns>
        public static SimpleCapModel GetRecordById(CapIDModel4WS capID, out CapModel4WS capModel)
        {
            if (capID == null)
            {
                throw new ACAException(LabelUtil.GetGUITextByKey("aca_urlrouting_label_invalidcapid"));
            }

            ICapBll capBll = ObjectFactory.GetObject<ICapBll>();
            bool isSuperCAP = CapUtil.IsSuperCAP(capID);

            string userSeqNum = (AppSession.User == null || string.IsNullOrEmpty(AppSession.User.UserSeqNum)) ? ACAConstant.ANONYMOUS_FLAG : AppSession.User.UserSeqNum;
            CapWithConditionModel4WS capWithConditionModel = capBll.GetCapViewBySingle(capID, userSeqNum, ACAConstant.COMMON_N, isSuperCAP);
            capModel = capWithConditionModel == null ? null : capWithConditionModel.capModel;

            if (capModel == null)
            {
                throw new ACAException(LabelUtil.GetGUITextByKey("aca_urlrouting_label_invalidcapid"));
            }

            CapModel4WS capCondition = new CapModel4WS
            {
                capID = capID,
                altID = capModel.altID,
                moduleName = capModel.moduleName
            };

            SearchResultModel searchResult = capBll.QueryPermitsGC(capCondition, null, userSeqNum, false, null, false, null);

            if (searchResult == null || searchResult.resultList == null || searchResult.resultList.Length == 0 || (searchResult.resultList[0] as SimpleCapModel) == null)
            {
                throw new ACAException(LabelUtil.GetGUITextByKey("aca_urlrouting_label_nopermission_record"));
            }

            SimpleCapModel simpleCapModel = searchResult.resultList[0] as SimpleCapModel;

            return simpleCapModel;
        }
        #endregion

        #region Collection

        /// <summary>
        /// Remove Empty List
        /// </summary>
        /// <typeparam name="T">class T</typeparam>
        /// <param name="collection">T collection</param>
        /// <returns>list T</returns>
        public static List<T> RemoveEmptyList<T>(List<T> collection)
        {
            List<T> result = new List<T>();
            if (collection == null)
            {
                return result;
            }

            foreach (var item in collection)
            {
                if (item != null)
                {
                    result.Add(item);
                }
            }

            collection.Clear();
            return result;
        }

        #endregion

        #region Attachment

        /// <summary>
        /// Get LP or CAP DocumentModel list 
        /// </summary>
        /// <param name="capModel">cap Model</param>
        /// <param name="agencyCode">agency Code</param>
        /// <param name="isAccountManagerPage">is AccountManagerPage</param>
        /// <param name="isPeopleDocument">is PeopleDocument</param>
        /// <param name="isDetailPage">is detail Page</param>
        /// <param name="entityModel">entity Model</param>
        /// <returns>DocumentModel list </returns>
        public static DocumentModel[] GetDocumentModels(CapModel4WS capModel, string agencyCode, bool isAccountManagerPage = false, bool isPeopleDocument = false, bool isDetailPage = true, EntityModel entityModel = null)
        {
            DocumentModel[] tempDocList = new DocumentModel[0];

            if (isAccountManagerPage)
            {
                isPeopleDocument = true;
            }

            CapIDModel capIDModel = null;
            bool isPartialCap = false;
            string moduleName = capModel == null ? string.Empty : capModel.moduleName;

            if (capModel != null)
            {
                capIDModel = TempModelConvert.Trim4WSOfCapIDModel(capModel.capID);

                if (!string.IsNullOrEmpty(capModel.capClass))
                {
                    isPartialCap = !capModel.capClass.Equals(ACAConstant.COMPLETED);
                }
            }

            try
            {
                // invoke the EMDS interface to get AttachmentList
                if (!AppSession.IsAdmin)
                {
                    IEDMSDocumentBll edmsDocBll = ObjectFactory.GetObject<IEDMSDocumentBll>();

                    if (isAccountManagerPage
                        && StandardChoiceUtil.IsEnableAccountAttachment())
                    {
                        //get people document list by reference LP
                        EntityModel[] entityList = entityModel == null ? AttachmentUtil.ConstructEntityModel(agencyCode) : new[] { entityModel };
                        DocumentResultModel resultModel = entityList.Length > 0 ? edmsDocBll.GetEntityDocumentList(agencyCode, AppSession.User.PublicUserId, entityList) : null;
                        resultModel = resultModel ?? new DocumentResultModel();
                        tempDocList = resultModel.documentList;

                        if (!string.IsNullOrEmpty(resultModel.errorMessage))
                        {
                            throw new Exception(resultModel.errorMessage);
                        }
                    }
                    else if (isPeopleDocument)
                    {
                        //get people document list by cap id after clicking view people attachments link
                        DocumentResultModel resultModel = edmsDocBll.GetPeopleDocumentByCapID(agencyCode, capIDModel, string.Empty, AppSession.User.PublicUserId);

                        resultModel = resultModel ?? new DocumentResultModel();
                        tempDocList = resultModel.documentList;

                        if (!string.IsNullOrEmpty(resultModel.errorMessage))
                        {
                            throw new Exception(resultModel.errorMessage);
                        }
                    }
                    else
                    {
                        //get record document list by cap id after clicking view record attachments link
                        tempDocList = edmsDocBll.GetRecordDocumentList(agencyCode, moduleName, AppSession.User.PublicUserId, capIDModel, isPartialCap);
                    }
                }
            }
            catch (ACAException ex)
            {
                throw ex;
            }

            tempDocList = tempDocList ?? new DocumentModel[0];

            // Check View Permission
            List<DocumentModel> listdDocumentModels = new List<DocumentModel>();
            bool isViewableForAllUser = StandardChoiceUtil.IsEnabledAllUserDocumentPermission(agencyCode, moduleName, "View");

            foreach (var documentModel in tempDocList)
            {
                // Show DocumentModel
                bool isDisplayDocTitleLink = false;

                if (isPartialCap)
                {
                    if (documentModel.viewTitleRoleModel != null)
                    {
                        var proxyUserRoleBll = ObjectFactory.GetObject<IProxyUserRoleBll>();
                        UserRolePrivilegeModel userRole = documentModel.viewTitleRoleModel;
                        isDisplayDocTitleLink = proxyUserRoleBll.HasPermission(capModel, userRole, ProxyPermissionType.VIEW_RECORD);

                        if (!isDisplayDocTitleLink)
                        {
                            isDisplayDocTitleLink = proxyUserRoleBll.HasPermission(capModel, userRole, ProxyPermissionType.MANAGE_DOCUMENTS);
                        }
                    }
                }
                else
                {
                    isDisplayDocTitleLink = isViewableForAllUser || documentModel.viewTitleable;
                    if (isPeopleDocument)
                    {
                        bool isFileOwner;

                        if (isDetailPage)
                        {
                            isFileOwner = AttachmentUtil.IsFileOwnerInCapDetail(documentModel, capModel);
                        }
                        else
                        {
                            // the request is from account management
                            isFileOwner = AttachmentUtil.IsFileOwner(documentModel);
                        }

                        if (!string.IsNullOrEmpty(documentModel.fileOwnerPermission) && isFileOwner)
                        {
                            isDisplayDocTitleLink = AttachmentUtil.CheckFileOwnerPermission(documentModel.fileOwnerPermission, FileOwnerPermission.TitleViewable);
                        }
                    }
                }

                if (!isDisplayDocTitleLink)
                {
                    continue;
                }

                listdDocumentModels.Add(documentModel);
            }

            return listdDocumentModels.ToArray();
        }

        /// <summary>
        /// download a file
        /// </summary>
        /// <param name="entityModel">cap id model.</param>
        /// <param name="documentNo">document number</param>
        /// <param name="fileKey">the field key</param>
        /// <param name="capClass">Cap Class</param>
        /// <param name="isAccountManagerPage">Is AccountManagerPage</param>
        /// <param name="isPeopleDocument">is PeopleDocument</param>
        /// <returns>Document Content Model</returns>
        public static DocumentModel DownloadAttachment(EntityModel entityModel, string documentNo, string fileKey, string capClass, bool isAccountManagerPage = false, bool isPeopleDocument = false)
        {
            if (isAccountManagerPage)
            {
                isPeopleDocument = true;
            }

            bool isPartialCap = false;

            if (!string.IsNullOrEmpty(capClass))
            {
                isPartialCap = !capClass.Equals(ACAConstant.COMPLETED);
            }

            string callerID = AppSession.User.PublicUserId;
            string agencyCode = ConfigManager.AgencyCode;

            //invoke the EDMS's to get a file.
            IEDMSDocumentBll edmsBll = (IEDMSDocumentBll)ObjectFactory.GetObject(typeof(IEDMSDocumentBll));
            string moduleName = isPeopleDocument ? string.Empty : entityModel.moduleName;

            try
            {
                //invoke the EDMS's to remove a file.
                DocumentModel documentModel = edmsBll.DoDownload(agencyCode, moduleName, callerID, entityModel, null, long.Parse(documentNo), fileKey, isPartialCap);

                if (documentModel == null)
                {
                    return null;
                }

                return documentModel;
            }
            catch (ACAException ex)
            {
                throw ex;
            }
        }

        /// <summary>
        ///  GET Download Permissions
        /// </summary>
        /// <param name="document">document model</param>
        /// <param name="capModel">cap Model</param>
        /// <param name="isDetailPage">is DetailPage default true</param>
        /// <param name="isPeopleDocument">Is PeopleDocument</param>
        /// <returns>get download right</returns>
        public static bool DownloadPermissions(DocumentModel document, CapModel4WS capModel, bool isDetailPage = true, bool isPeopleDocument = false)
        {
            if (document == null)
            {
                return false;
            }

            bool isPartialCap = false;

            if (capModel != null)
            {
                if (!string.IsNullOrEmpty(capModel.capClass))
                {
                    isPartialCap = !capModel.capClass.Equals(ACAConstant.COMPLETED);
                }
            }

            string enStyleDate = I18nDateTimeUtil.FormatToDateStringForWebService(document.recDate);
            string entityType = document.entityType;

            if (ACAConstant.FILE_PENDING_DATE.Equals(enStyleDate, StringComparison.InvariantCultureIgnoreCase)
                || (!(entityType is DBNull) && DocumentEntityType.TMP_CAP.Equals(entityType, StringComparison.InvariantCultureIgnoreCase) && !isPartialCap))
            {
                return false;
            }
            else if (enStyleDate.Equals(ACAConstant.FILE_FAILED_DATE, StringComparison.InvariantCultureIgnoreCase))
            {
                return false;
            }

            bool hasDownloadRight = false;

            try
            {
                bool isDisplayDownLink = false;
                string callerID = AppSession.User.PublicUserId;
                string moduleName = !isPeopleDocument ? capModel.moduleName : string.Empty;
                CapIDModel4WS capID = capModel == null ? null : capModel.capID;
                EdmsPolicyModel4WS edmsPolicyModel = CapUtil.GetEdmsPolicyModel4WS(ConfigManager.AgencyCode, moduleName, callerID, capID);
                string edmsDownloadRight = edmsPolicyModel.downloadRight;

                //Generate the actions based on the permissions.
                UserRolePrivilegeModel viewRole = document.viewRoleModel;

                string fileOwnerPermission = document.fileOwnerPermission;

                bool isDownloadableForAllUser = StandardChoiceUtil.IsEnabledAllUserDocumentPermission(ConfigManager.AgencyCode, moduleName, "Download");

                var proxyUserRoleBll = ObjectFactory.GetObject<IProxyUserRoleBll>();

                if (isPartialCap)
                {
                    if (viewRole != null)
                    {
                        hasDownloadRight = proxyUserRoleBll.HasPermission(capModel, viewRole, ProxyPermissionType.VIEW_RECORD);

                        if (!hasDownloadRight)
                        {
                            hasDownloadRight = proxyUserRoleBll.HasPermission(capModel, viewRole, ProxyPermissionType.MANAGE_DOCUMENTS);
                        }
                    }
                }
                else
                {
                    isDisplayDownLink = isDownloadableForAllUser || document.viewable;
                    if (isPeopleDocument)
                    {
                        // ACA Permission In V360
                        bool isFileOwner;

                        if (isDetailPage)
                        {
                            isFileOwner = AttachmentUtil.IsFileOwnerInCapDetail(document, capModel);
                        }
                        else
                        {
                            // the request is from account management
                            isFileOwner = AttachmentUtil.IsFileOwner(document);
                        }

                        if (!string.IsNullOrEmpty(document.fileOwnerPermission) && isFileOwner)
                        {
                            isDisplayDownLink = AttachmentUtil.CheckFileOwnerPermission(document.fileOwnerPermission, FileOwnerPermission.Downloadable);
                        }
                    }

                    hasDownloadRight = isDisplayDownLink;
                }

                hasDownloadRight = (AttachmentUtil.IsFileOwner(document) || hasDownloadRight) && (!ACAConstant.COMMON_FALSE.Equals(edmsDownloadRight, StringComparison.InvariantCultureIgnoreCase));

                if (isPeopleDocument && hasDownloadRight && !string.IsNullOrEmpty(fileOwnerPermission))
                {
                    hasDownloadRight = AttachmentUtil.CheckFileOwnerPermission(fileOwnerPermission, FileOwnerPermission.Downloadable);
                }
            }
            catch (Exception ex)
            {
                //not throw
                hasDownloadRight = false;
            }

            return hasDownloadRight;
        }

        /// <summary>
        /// Gets a value to indicating whether the current user has the Upload permission and the the upload button.
        /// </summary>
        /// <param name="moduleName">module Name</param>
        /// <param name="capID"> </param>
        /// <returns>return a value to indicating whether the current user has the Upload permission and the the upload button.</returns>
        public static bool IsShowUploadButtonInDetailPage(string moduleName, CapIDModel4WS capID)
        {
            if (AppSession.IsAdmin)
            {
                return true;
            }

            // Get section permissions
            Dictionary<string, UserRolePrivilegeModel> sectionPermissions = CapUtil.GetSectionPermissions(ConfigManager.AgencyCode, moduleName);

            if (!CapUtil.GetSectionVisibility(CapDetailSectionType.ATTACHMENTS.ToString(), sectionPermissions, moduleName))
            {
                return false;
            }

            // agency customize defined disabled then hide the link.
            if (!FunctionTable.IsEnableUploadDocument())
            {
                return false;
            }

            CapModel4WS capModel = GetCapModel4Ws(capID);
            capID = capModel != null ? capModel.capID : null;
            EdmsPolicyModel4WS edmsPolicyModel = CapUtil.GetEdmsPolicyModel4WS(ConfigManager.AgencyCode, moduleName, AppSession.User.PublicUserId, capID);

            if (edmsPolicyModel != null)
            {
                // get the EDMS upload right
                bool hasEDMSUploadRight = Convert.ToBoolean(edmsPolicyModel.uploadRight);

                bool hasUploadPermission = AttachmentUtil.HasDocumentTypeUploadPermission(capModel, moduleName, null, false);
                return hasEDMSUploadRight && hasUploadPermission;
            }

            return false;
        }
        #endregion

        #region Inspection

        #endregion

        #region Culture Language
        /// <summary>
        /// Gets the language switch panel.
        /// </summary>
        /// <returns>return language</returns>
        public static string GetLanguages()
        {
            StringBuilder cultureLanguages = new StringBuilder();
            cultureLanguages.Append("[");
            Dictionary<string, string> supportedCultureList = I18nCultureUtil.GetSupportedLanguageList();
            bool isMultiLanguageEnabled = I18nCultureUtil.IsMultiLanguageEnabled;

            string defaultCulture = supportedCultureList[I18nCultureUtil.UserPreferredCulture];
            cultureLanguages.Append("{");
            cultureLanguages.Append("\"languageKey\": \"" + I18nCultureUtil.UserPreferredCulture + "\",");
            cultureLanguages.Append("\"languageName\": \"" + defaultCulture + "\"");
            cultureLanguages.Append("},");

            if (isMultiLanguageEnabled)
            {
                foreach (string key in supportedCultureList.Keys)
                {
                    if (I18nCultureUtil.UserPreferredCulture != key)
                    {
                        string cultureLanguageText = supportedCultureList[key];
                        cultureLanguages.Append("{");
                        cultureLanguages.Append("\"languageKey\": \"" + key + "\",");
                        cultureLanguages.Append("\"languageName\": \"" + cultureLanguageText + "\"");
                        cultureLanguages.Append("},");
                    }
                }
            }

            cultureLanguages = ApiUtil.GetRealString(cultureLanguages);
            cultureLanguages.Append("]");

            return cultureLanguages.ToString();
        }
        #endregion

        #region method
        /// <summary>
        /// delete the "," when the StringBuilder is end with ","
        /// </summary>
        /// <param name="result">string result</param>
        /// <returns>format string</returns>
        public static StringBuilder GetRealString(StringBuilder result)
        {
            if (!string.IsNullOrEmpty(result.ToString()) && result.Length > 1 && result.ToString().EndsWith(","))
            {
                result.Length -= 1;
            }

            return result;
        }

        /// <summary>
        /// Get RECAPTCHA setting
        /// </summary>
        /// <returns>config data</returns>
        public static string GetRecaptchaSetting()
        {
            StringBuilder sbRecaptcha = new StringBuilder();
            string recaptchaPublicKey = ConfigurationManager.AppSettings["RecaptchaPublicKey"];
            string rcaptchaPrivateKey = ConfigurationManager.AppSettings["RecaptchaPrivateKey"];

            sbRecaptcha.Append("{");
            sbRecaptcha.Append("\"recaptchaPublicKey\":\"" + recaptchaPublicKey + "\",");
            sbRecaptcha.Append("\"rcaptchaPrivateKey\":\"" + rcaptchaPrivateKey + "\"");
            sbRecaptcha.Append("}");

            return sbRecaptcha.ToString();
        }

        /// <summary>
        ///  Get InspectionList Data
        /// </summary>
        /// <param name="capModel">cap Model</param>
        /// <returns>inspection count</returns>
        public static int GetInspectionCount(CapModel4WS capModel)
        {
            int count = 0;

            if (capModel != null && !AppSession.IsAdmin)
            {
                var inspectionBll = ObjectFactory.GetObject<IInspectionBll>();
                InspectionModel[] inspectionModels = inspectionBll.GetInspectionListByCapID(capModel.moduleName, TempModelConvert.Trim4WSOfCapIDModel(capModel.capID), null, AppSession.User.PublicUserId);

                if (inspectionModels != null)
                {
                    foreach (var inspectionModel in inspectionModels)
                    {
                        if (string.IsNullOrEmpty(inspectionModel.activity.displayInACA) || ValidationUtil.IsYes(inspectionModel.activity.displayInACA))
                        {
                            count++;
                        }
                    }
                }
            }

            return count;
        }

        /// <summary>
        /// Encodes a URL string.
        /// </summary>
        /// <param name="context">Context for encode</param>
        /// <returns>a encode url</returns>
        private static string UrlEncode(string context)
        {
            if (string.IsNullOrEmpty(context))
            {
                return string.Empty;
            }

            return HttpUtility.UrlEncode(context);
        }
        #endregion
    }
}