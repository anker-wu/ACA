#region

/**
* <pre>
* 
*  Accela Citizen Access
*  File: GlobalSearchController.cs
* 
*  Accela, Inc.
*  Copyright (C): 2014
* 
*  Description:
*  
*  Notes:
*      $Id:GlobalSearchController.cs 77905 2014-06-11 12:49:28Z ACHIEVO\dennis.fu $.
*  Revision History
*  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
* </pre>
*/

#endregion

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web.Http;
using Accela.ACA.Common;
using Accela.ACA.Common.Common;
using Accela.ACA.Common.Util;
using Accela.ACA.Web.Common;
using Accela.ACA.Web.Common.GlobalSearch;
using Accela.ACA.Web.WebApi.Entity.Adapter;
using Accela.ACA.Web.WebApi.Util;
using Accela.ACA.WSProxy;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Accela.ACA.Web.WebApi
{
    /// <summary>
    /// Web API for Global search function
    /// </summary>
    public class GlobalSearchController : ApiController
    {
        #region Field
        /// <summary>
        /// license number
        /// </summary>
        private const string LP_FIRST_COLUMN_NAME = "licenseNumber";

        /// <summary>
        /// full address
        /// </summary>
        private const string ADDRESS_FIRST_COLUMN_NAME = "fullAddress";

        /// <summary>
        /// created date
        /// </summary>
        private const string CAP_FIRST_COLUMN_NAME = "createdDate";

        /// <summary>
        /// parcel number
        /// </summary>
        private const string PARCEL_FIRST_COLUMN_NAME = "parcelNumber";

        /// <summary>
        /// ID, Type, Address
        /// </summary>
        private const string SoftList = "ID,Type,Address";

        /// <summary>
        /// Error message
        /// </summary>
        private const string Error = "We found more than 100 items matching your search. Please use filters to narrow your search.";

        /// <summary>
        /// Gets User data story in cache.
        /// </summary>
        private string CacheKey
        {
            get
            {
                if (AppSession.User != null)
                {
                    return AppSession.User.PublicUserId + "GlobalSearch";
                }
                else
                {
                    return null;
                }
            }
        }

        #endregion

        /// <summary>
        /// Search for global
        /// </summary>
        /// <param name="queryText">query Text</param>
        /// <param name="types">Type the query text(Values for","segmentation)[CAP|LP|APO]</param>
        /// <param name="sort">SORT The default value[ID,Type,Address]</param>
        /// <param name="isAsc">ASC boolean</param>
        /// <param name="isFilter">Is Filter boolean</param>
        /// <returns>Global Search Page Json Data</returns>
        [ActionName("GlobalSearch")]
        public HttpResponseMessage GetGlobalSearch(string queryText, string types, string sort, bool isAsc, bool isFilter)
        {
            if (string.IsNullOrEmpty(types) || string.IsNullOrEmpty(queryText))
            {
                return null;
            }

            string[] typeStrings = types.Split(',');

            foreach (string typeString in typeStrings)
            {
                if (!CheckType(typeString))
                {
                    return null;
                }
            }

            List<CustomLpView4Ui> customLpView4UiList = new List<CustomLpView4Ui>();
            List<CustomApoView4Ui> customapoView4UiList = new List<CustomApoView4Ui>();
            List<CustomCapView4Ui> customCapView4UiList = new List<CustomCapView4Ui>();
            int pageIndex = 0;
            string message = string.Empty;

            if (!string.IsNullOrEmpty(sort) || isFilter)
            {
                this.GetCacheData(out customLpView4UiList, out customapoView4UiList, out customCapView4UiList, out pageIndex, typeStrings);
            }
            else
            {
                // Get query text from search input box
                string[] modules = GlobalSearchUtil.GetAllModuleKeys();

                if (GlobalSearchUtil.IsLPEnabled() && typeStrings.Contains("LP"))
                {
                    List<LPView4UI> lpList = null;
                    lpList = GlobalSearchManager.ExecuteQuery<LPView4UI>(
                        GlobalSearchType.LP, 
                        queryText, 
                        modules,
                        LP_FIRST_COLUMN_NAME, 
                        ACAConstant.ORDER_BY_ASC,
                        ACAConstant.DEFAULT_PAGESIZE, 
                        null);
                    lpList = lpList ?? new List<LPView4UI>();

                    //Init LP SoftID
                    int initLPIndex = 0; 
                    foreach (var item in lpList)
                    {
                        customLpView4UiList.Add(new CustomLpView4Ui(item, initLPIndex++));
                    }
                }

                if (GlobalSearchUtil.IsAPOEnabled() && typeStrings.Contains("APO"))
                {
                    List<APOView4UI> apoList = null;
                    apoList = GlobalSearchManager.ExecuteQuery<APOView4UI>(
                        GlobalSearchType.ADDRESS, 
                        queryText, 
                        modules, 
                        ADDRESS_FIRST_COLUMN_NAME,
                        ACAConstant.ORDER_BY_ASC,
                        ACAConstant.DEFAULT_PAGESIZE, 
                        null);

                    if (apoList == null || apoList.Count == 0)
                    {
                        apoList = GlobalSearchManager.ExecuteQuery<APOView4UI>(
                            GlobalSearchType.PARCEL,
                            queryText,
                            modules, 
                            PARCEL_FIRST_COLUMN_NAME,
                            ACAConstant.ORDER_BY_ASC,
                            ACAConstant.DEFAULT_PAGESIZE, 
                            null);
                    }

                    //convert to custom APO
                    apoList = apoList ?? new List<APOView4UI>();
                    int initApoIndex = 0; //Init APO SoftID
                    foreach (var item in apoList)
                    {
                        customapoView4UiList.Add(new CustomApoView4Ui(item, initApoIndex++));
                    }
                }

                if (GlobalSearchUtil.IsRecordEnabled() && typeStrings.Contains("CAP"))
                {
                    List<CAPView4UI> capList = null;
                    capList = GlobalSearchManager.ExecuteQuery<CAPView4UI>(
                        GlobalSearchType.CAP, 
                        queryText, 
                        modules,
                        CAP_FIRST_COLUMN_NAME,
                        ACAConstant.ORDER_BY_DESC,
                        ACAConstant.DEFAULT_PAGESIZE,
                        null);

                    //convert to custom CAP 
                    capList = capList ?? new List<CAPView4UI>();
                    foreach (var item in capList)
                    {
                        CustomCapView4Ui customCapView4 = new CustomCapView4Ui(item);
                        customCapView4UiList.Add(customCapView4);
                    }
                }

                if (customLpView4UiList.Count + customapoView4UiList.Count +
                    customCapView4UiList.Count > ACAConstant.DEFAULT_PAGECOUNT * ACAConstant.DEFAULT_PAGESIZE)
                {
                    message = GlobalSearchController.Error;
                }
            }

            string[] splitSort = SoftList.Split(',');

            if (splitSort.Contains(sort))
            {
                customLpView4UiList.Sort(delegate(CustomLpView4Ui x, CustomLpView4Ui y)
                {
                    if (sort == splitSort[0])
                    {
                        int result = string.CompareOrdinal(x.LicenseNumber, y.LicenseNumber);

                        return (isAsc || result == 0) ? result : (result > 0 ? -1 : 1);
                    }
                    else if (sort == splitSort[1])
                    {
                        int result = string.CompareOrdinal(x.LicenseType, y.LicenseType);
                        return (isAsc || result == 0) ? result : (result > 0 ? -1 : 1);
                    }
                    else
                    {
                        int result = string.CompareOrdinal(x.LicenseNumber, y.LicenseNumber);
                        return (isAsc || result == 0) ? result : (result > 0 ? -1 : 1);
                    }
                });
            }

            if (splitSort.Contains(sort))
            {
                customapoView4UiList.Sort(delegate(CustomApoView4Ui x, CustomApoView4Ui y)
                {
                    if (sort == splitSort[0])
                    {
                        int result = string.CompareOrdinal(x.ParcelNumber, y.ParcelNumber);
                        return (isAsc || result == 0) ? result : (result > 0 ? -1 : 1);
                    }
                    else if (sort == splitSort[2])
                    {
                        int result = string.CompareOrdinal(x.AddressDescription, y.AddressDescription);
                        return (isAsc || result == 0) ? result : (result > 0 ? -1 : 1);
                    }
                    else
                    {
                        int result = string.CompareOrdinal(x.ParcelNumber, y.ParcelNumber);
                        return (isAsc || result == 0) ? result : (result > 0 ? -1 : 1);
                    }
                });
            }

            if (splitSort.Contains(sort))
            {
                customCapView4UiList.Sort(delegate(CustomCapView4Ui x, CustomCapView4Ui y)
                {
                    if (sort == splitSort[0])
                    {
                        int result = string.CompareOrdinal(x.PermitNumber, y.PermitNumber);
                        return (isAsc || result == 0) ? result : (result > 0 ? -1 : 1);
                    }
                    else if (sort == splitSort[1])
                    {
                        int result = string.CompareOrdinal(x.PermitType, y.PermitType);
                        return (isAsc || result == 0) ? result : (result > 0 ? -1 : 1);
                    }
                    else
                    {
                        int result = string.CompareOrdinal(x.Address, y.Address);
                        return (isAsc || result == 0) ? result : (result > 0 ? -1 : 1);
                    }
                });
            } 

            if (!string.IsNullOrEmpty(sort) || isFilter)
            {
                AddToCache(customLpView4UiList, customapoView4UiList, customCapView4UiList, -1, typeStrings);
            }
            else
            {
                AddToCache(customLpView4UiList, customapoView4UiList, customCapView4UiList, 0);
            }

            bool isEnableExport2CSV = StandardChoiceUtil.IsEnableExport2CSV();
            return new HttpResponseMessage
            {
                Content = new StringContent("{\"record\":[" + GetGlobalSearchPageJsonData(0) + "],\"message\":\"" + message + "\",\"enableExport\":" + isEnableExport2CSV.ToString().ToLower() + "}")
            };
        }

        /// <summary>
        /// Get current page data.
        /// </summary>
        /// <param name="currentPageIndex">current page index</param>
        /// <returns>Json format page data</returns>
        [ActionName("globalSearch-page-data")]
        public HttpResponseMessage GetGlobalSearchPageData(int currentPageIndex)
        {
            int pageEndIndex = (currentPageIndex + 1) * ACAConstant.DEFAULT_PAGESIZE;

            //total
            int total = GetGlobalSecarchTotal();
            
            //include '+' and  curren Max page big cache page Total
            if (total > pageEndIndex)
            {
                return new HttpResponseMessage
                {
                    Content = new StringContent(GetGlobalSearchPageJsonData(currentPageIndex))
                };
            }
            else
            {
                List<CustomLpView4Ui> customLpView4UiList = new List<CustomLpView4Ui>();
                List<CustomApoView4Ui> customApoView4Ui = new List<CustomApoView4Ui>();
                List<CustomCapView4Ui> customCapView4UiList = new List<CustomCapView4Ui>();

                int cacheTotal = 0;
                string[] filterTypes = GetFilterTypes();

                if (filterTypes.Length == 0)
                {
                    this.GetCacheData(out customLpView4UiList, out customApoView4Ui, out customCapView4UiList, out cacheTotal);
                }
                else
                {
                    this.GetCacheData(out customLpView4UiList, out customApoView4Ui, out customCapView4UiList, out cacheTotal, null, true);
                }

                cacheTotal++;

                List<LPView4UI> temLpView4Uis = null;
                List<APOView4UI> temApoView4UIs = null;
                List<CAPView4UI> temCapView4Uis = null;

                if (customLpView4UiList.Count > 0 && (filterTypes.Contains("LP") || filterTypes.Length == 0))
                {
                    if ((cacheTotal * ACAConstant.DEFAULT_PAGECOUNT * ACAConstant.DEFAULT_PAGESIZE) < customLpView4UiList.Count)
                    {
                        temLpView4Uis = GlobalSearchManager.ExecuteQuery<LPView4UI>(
                            GlobalSearchType.LP,
                            cacheTotal * ACAConstant.DEFAULT_PAGECOUNT, 
                            null);
                    }
                }

                if (customApoView4Ui.Count > 0 && (filterTypes.Contains("APO") || filterTypes.Length == 0))
                {
                    temApoView4UIs = GlobalSearchManager.ExecuteQuery<APOView4UI>(
                        GlobalSearchType.ADDRESS,
                        cacheTotal * ACAConstant.DEFAULT_PAGECOUNT,
                        null);

                    if (temApoView4UIs == null || temApoView4UIs.Count == 0)
                    {
                        temApoView4UIs = GlobalSearchManager.ExecuteQuery<APOView4UI>(
                            GlobalSearchType.PARCEL,
                            cacheTotal * ACAConstant.DEFAULT_PAGECOUNT, 
                            null);
                    }
                }

                if (customCapView4UiList.Count > 0 && (filterTypes.Contains("CAP") || filterTypes.Length == 0))
                {
                    if ((cacheTotal * ACAConstant.DEFAULT_PAGECOUNT * ACAConstant.DEFAULT_PAGESIZE) < customCapView4UiList.Count)
                    { 
                        temCapView4Uis = GlobalSearchManager.ExecuteQuery<CAPView4UI>(
                            GlobalSearchType.CAP,
                            cacheTotal * ACAConstant.DEFAULT_PAGECOUNT, 
                            null);
                    }
                }

                temLpView4Uis = ApiUtil.RemoveEmptyList(temLpView4Uis);
                temApoView4UIs = ApiUtil.RemoveEmptyList(temApoView4UIs);
                temCapView4Uis = ApiUtil.RemoveEmptyList(temCapView4Uis);

                if (temLpView4Uis.Count > 0)
                {
                    temLpView4Uis.RemoveAt(0);
                }

                if (temApoView4UIs.Count > 0)
                {
                    temApoView4UIs.RemoveAt(0);
                }

                if (temCapView4Uis.Count > 0)
                {
                    temCapView4Uis.RemoveAt(0);
                }

                int lpMaxIndx = customLpView4UiList.Count;

                //convert LP to custom LP
                foreach (var lpItem in temLpView4Uis)
                {
                    CustomLpView4Ui lpView4Ui = new CustomLpView4Ui(lpItem, lpMaxIndx++);
                    customLpView4UiList.Add(lpView4Ui);
                }

                int apoMaxIndx = customApoView4Ui.Count;
                foreach (var item in temApoView4UIs)
                {
                    customApoView4Ui.Add(new CustomApoView4Ui(item, apoMaxIndx++));
                }

                //convert CAP to custom Cap
                foreach (var capItem in temCapView4Uis)
                {
                    CustomCapView4Ui capView4Ui = new CustomCapView4Ui(capItem);
                    customCapView4UiList.Add(capView4Ui);
                }

                // Add to Cache
                AddToCache(customLpView4UiList, customApoView4Ui, customCapView4UiList, cacheTotal, filterTypes.Length == 0 ? null : filterTypes);

                return new HttpResponseMessage
                {
                    Content = new StringContent(GetGlobalSearchPageJsonData(currentPageIndex))
                };
            }
        }

        /// <summary>
        /// Get total page count.
        /// </summary>
        /// <returns>total page count</returns>
        [ActionName("globalSearch-page-count")]
        public HttpResponseMessage GetGlobalSearchPageCount()
        {
            int resultCount = this.GetGlobalSecarchTotal();
            resultCount = (resultCount / ACAConstant.DEFAULT_PAGESIZE) +
                          ((resultCount % ACAConstant.DEFAULT_PAGESIZE) > 0 ? 1 : 0);

            return new HttpResponseMessage
            {
                Content = new StringContent(resultCount.ToString())
            };
        }

        /// <summary>
        /// Get global search description. 
        /// </summary>
        /// <returns>string description</returns>
        [ActionName("globalSearch-description")]
        public HttpResponseMessage GetGlobalSearchDescription()
        {
            string result = string.Empty;
            Dictionary<string, object> caechDictionary = (Dictionary<string, object>)ApiUtil.GetSession(CacheKey);
            List<CustomLpView4Ui> lpList = null;
            List<CustomApoView4Ui> apoList = null;
            List<CustomCapView4Ui> capList = null;

            if (caechDictionary.ContainsKey("LP"))
            {
                lpList = caechDictionary["LP"] as List<CustomLpView4Ui>;
            }

            if (caechDictionary.ContainsKey("APO"))
            {
                apoList = caechDictionary["APO"] as List<CustomApoView4Ui>;
            }

            if (caechDictionary.ContainsKey("CAP"))
            {
                capList = caechDictionary["CAP"] as List<CustomCapView4Ui>;
            }

            lpList = lpList ?? new List<CustomLpView4Ui>();
            apoList = apoList ?? new List<CustomApoView4Ui>();
            capList = capList ?? new List<CustomCapView4Ui>();

            int resultCount = lpList.Count + apoList.Count + capList.Count;

            var recordDictionary = new Dictionary<string, string>();

            recordDictionary.Add("Professionals", ApiUtil.DisplayAmountPlus(lpList.Count));
            recordDictionary.Add("Properties", ApiUtil.DisplayAmountPlus(apoList.Count));
            recordDictionary.Add("Records", ApiUtil.DisplayAmountPlus(capList.Count));

            foreach (var item in recordDictionary.Keys)
            {
                result += recordDictionary[item] + " " + item + ", ";
            }

            result = result.Trim();
            result = result.Length > 1 ? result.Remove(result.Length - 1) : result;
            result = resultCount > 0 ? "{\"resultSize\":\"" + resultCount + "\",\"descriptions\":\"" + result + "\"}" : "No records found";
            return new HttpResponseMessage
            {
                Content = new StringContent(result)
            };
        }

        /// <summary>
        /// globalSearch  download 
        /// </summary>
        /// <param name="type"> download  Type</param>
        /// <returns>globalSearch Type Data</returns>
        [ActionName("globalSearch-download")]
        public HttpResponseMessage GetGlobalSearchDownload(string type)
        {
            string message = "[]";
            bool isOk = false;
          
            if (CheckType(type))
            {
                Dictionary<string, object> caechDictionary = (Dictionary<string, object>)ApiUtil.GetSession(CacheKey) ??
                                                       new Dictionary<string, object>();

                if (caechDictionary.ContainsKey("LP") && type == "LP")
                {
                    List<CustomLpView4Ui> lpList = caechDictionary["LP"] as List<CustomLpView4Ui>;
                    isOk = true;
                    message = Newtonsoft.Json.JsonConvert.SerializeObject(lpList);
                }

                if (caechDictionary.ContainsKey("APO") && type == "APO")
                {
                    List<CustomApoView4Ui> apoList = caechDictionary["APO"] as List<CustomApoView4Ui>;
                    isOk = true;
                    message = Newtonsoft.Json.JsonConvert.SerializeObject(apoList);
                }

                if (caechDictionary.ContainsKey("CAP") && type == "CAP")
                {
                    List<CustomCapView4Ui> capList = caechDictionary["CAP"] as List<CustomCapView4Ui>;
                    isOk = true;
                    message = Newtonsoft.Json.JsonConvert.SerializeObject(capList);
                }
            } 
            return new HttpResponseMessage
            {
                Content = new StringContent("{\"isOK\":" + isOk.ToString().ToLower() + ",\"message\":" + message + "}")
            };
        }

        /// <summary>
        /// Get Global Search total
        /// </summary>
        /// <returns>Global Search Total</returns>
        [NonAction]
        private int GetGlobalSecarchTotal()
        {
            List<CustomLpView4Ui> customLpView4UiList = new List<CustomLpView4Ui>();
            List<CustomApoView4Ui> customapoView4UiList = new List<CustomApoView4Ui>();
            List<CustomCapView4Ui> customCapView4UiList = new List<CustomCapView4Ui>();
            int pageIndex;
            this.GetCacheData(out customLpView4UiList, out customapoView4UiList, out customCapView4UiList, out pageIndex);
            int resultCount = customLpView4UiList.Count + customapoView4UiList.Count + customCapView4UiList.Count;
            return resultCount;
        }

        /// <summary>
        /// Get global search Json data. 
        /// </summary>
        /// <param name="currentPageIndex">current page index</param>
        /// <returns>Json format</returns>
        [NonAction]
        private string GetGlobalSearchPageJsonData(int currentPageIndex)
        {
            int startIndex = currentPageIndex * ACAConstant.DEFAULT_PAGESIZE;

            List<CustomLpView4Ui> lpList = null;
            List<CustomApoView4Ui> apoList = null;
            List<CustomCapView4Ui> capList = null;
            int pageIndex;
            this.GetCacheData(out lpList, out apoList, out capList, out pageIndex);
            List<object> result = new List<object>();

            int count = 0;
            foreach (LPView4UI lpItem in lpList)
            {
                if (count >= startIndex && count < startIndex + ACAConstant.DEFAULT_PAGESIZE)
                {
                    result.Add(lpItem);
                }

                count++;
            }

            foreach (CustomApoView4Ui lpItem in apoList)
            {
                if (count >= startIndex && count < startIndex + ACAConstant.DEFAULT_PAGESIZE)
                {
                    result.Add(lpItem);
                }

                count++;
            }

            foreach (CAPView4UI lpItem in capList)
            {
                if (count >= startIndex && count < startIndex + ACAConstant.DEFAULT_PAGESIZE)
                {
                    result.Add(lpItem);
                }

                count++;
            }

            //whether or not shown Add to cart button
            bool isShowAddToCart = !AppSession.User.IsAnonymous && StandardChoiceUtil.IsEnableShoppingCart();

            List<CustomLpView4Ui> lpListjosn = new List<CustomLpView4Ui>();
            List<CustomApoView4Ui> apoListjosn = new List<CustomApoView4Ui>();
            List<CustomCapView4Ui> capListjosn = new List<CustomCapView4Ui>();
            foreach (object item in result)
            {
                if (typeof(CustomLpView4Ui) == item.GetType())
                {
                    CustomLpView4Ui customLpView4Ui = item as CustomLpView4Ui;
                    if (customLpView4Ui != null)
                    {
                        customLpView4Ui.DetailViewUrl = ApiUtil.ConstructLpDetailUrl(customLpView4Ui.LicenseNumber, customLpView4Ui.LicenseType);
                        lpListjosn.Add(customLpView4Ui);
                    }
                }

                if (typeof(CustomApoView4Ui) == item.GetType())
                {
                    CustomApoView4Ui aPOView4UIa = item as CustomApoView4Ui;
                    if (aPOView4UIa != null)
                    {
                        aPOView4UIa.DetailViewUrl = ApiUtil.ConstructParceDetailUrl(
                            aPOView4UIa.ParcelNumber,
                            aPOView4UIa.ParcelSeqNbr, 
                            aPOView4UIa.AddressSeqNumber, 
                            aPOView4UIa.AddressSourceNumber);
                        apoListjosn.Add(aPOView4UIa);
                    }
                }

                if (typeof(CustomCapView4Ui) == item.GetType())
                {
                    CustomCapView4Ui capView4Ui = item as CustomCapView4Ui;

                    if (capView4Ui != null)
                    {
                        string[] capIDPartArray = capView4Ui.CapID.Split('-');

                        CapIDModel4WS capIDModel = new CapIDModel4WS
                        {
                            id1 = capIDPartArray[0],
                            id2 = capIDPartArray[1],
                            id3 = capIDPartArray[2],
                            serviceProviderCode = capView4Ui.AgencyCode
                        };
                     
                        // to show ReportButton or not.                   
                       CustomCapView4Ui customCapView4Ui = NewUiReportUtil.DisplayReportButton(capIDModel, capView4Ui.ModuleName);

                        //resume link
                        string resumeUrl = string.Empty;
                        bool isPartialCap = false;

                        string renewalStatus = string.Empty;
                        bool displayInspcetion = ActionButtonController.DisplayInspcetion(capIDModel, capView4Ui.ModuleName);

                        SimpleCapModel simpleCapModel = ApiUtil.GetCapModel4WsByCapID(capIDPartArray[0], capIDPartArray[1], capIDPartArray[2], capView4Ui.AgencyCode);
                        bool hasNoPaidFees = simpleCapModel.noPaidFeeFlag;

                        if (CapUtil.IsPartialCap(capView4Ui.CapClass))
                        {
                            isPartialCap = true;
                           
                            if (simpleCapModel != null && simpleCapModel.hasPrivilegeToHandleCap)
                            {
                                resumeUrl = ApiUtil.ConstructResumeDeepLink(
                                    capView4Ui.ModuleName, 
                                    capIDPartArray[0],
                                    capIDPartArray[1], 
                                    capIDPartArray[2],
                                    capView4Ui.AgencyCode, 
                                    capView4Ui.CapClass,
                                    ApiUtil.GetFilterNameForResume(simpleCapModel.capType, capView4Ui.ModuleName));
 
                                renewalStatus = simpleCapModel.renewalStatus;
                            }
                        }

                        //pay
                        bool canPayFeeDue = false;
                        bool canPayFeeDue4Renew = false;

                        if (!isPartialCap && !StandardChoiceUtil.IsRemovePayFee(ConfigManager.AgencyCode))
                        {
                            if (hasNoPaidFees && FunctionTable.IsEnableMakePayment())
                            {
                                canPayFeeDue = true;
                            }
                            else if (FunctionTable.IsEnableRenewRecord() && ACAConstant.PAYFEEDUE_RENEWAL.Equals(renewalStatus, StringComparison.InvariantCultureIgnoreCase))
                            {
                                canPayFeeDue4Renew = true;
                            }
                        }

                        string[] capid = capView4Ui.CapID.Split(new string[] { ACAConstant.SPLIT_CHAR4 }, StringSplitOptions.None);
                        string payfeesUrl = string.Empty;

                        if (canPayFeeDue)
                        {
                            payfeesUrl = ApiUtil.ConstructPayFeesDeepLink(capView4Ui.ModuleName, capid[0], capid[1], capid[2], capView4Ui.AgencyCode);
                        }
                        else if (canPayFeeDue4Renew)
                        {
                            payfeesUrl = ApiUtil.ConstructPayFeeDue4RenewDeepLink(capView4Ui.ModuleName, capid[0], capid[1], capid[2], capView4Ui.AgencyCode);
                        }

                        // Display\hide clone record button
                        bool isShowCopyRecord = false;

                        if (CloneRecordUtil.IsDisplayCloneButton((CapTypeModel)simpleCapModel.capType,
                            TempModelConvert.Trim4WSOfCapIDModel(capIDModel), capView4Ui.ModuleName, true)
                            && FunctionTable.IsEnableCloneRecord())
                        {
                            isShowCopyRecord = true;
                        }

                        capView4Ui.IsShowCopyRecord = isShowCopyRecord;
                        capView4Ui.PayfeesUrl = payfeesUrl;
                        capView4Ui.DisplayInspcetion = displayInspcetion;

                        CapModel4WS capModel4WS = new CapModel4WS();
                        capModel4WS.capID = capIDModel;
                        capModel4WS.moduleName = capView4Ui.ModuleName;

                        capView4Ui.Inspections = ApiUtil.GetInspectionCount(capModel4WS);
                        capView4Ui.HasNoPaidFees = hasNoPaidFees;
                        capView4Ui.RenewalStatus = renewalStatus;
                        capView4Ui.ResumeUrl = resumeUrl;
                        capView4Ui.IsPartialCap = isPartialCap;
                        capView4Ui.IsShowAddToCart = isShowAddToCart;
                        capView4Ui.IsShowUpload = !isPartialCap && ApiUtil.IsShowUploadButtonInDetailPage(capView4Ui.ModuleName, capIDModel);

                        //PrintPermitReport button
                        capView4Ui.IsShowPrintPermit = customCapView4Ui.IsShowPrintPermit;
                        capView4Ui.PrintPermitReportId = customCapView4Ui.PrintPermitReportId;
                        capView4Ui.PrintPermitReportName = customCapView4Ui.PrintPermitReportName;
                        capView4Ui.PrintPermitReportType = customCapView4Ui.PrintPermitReportType;

                        //PrintSummaryReport button
                        capView4Ui.IsShowPrintSummary = customCapView4Ui.IsShowPrintSummary;
                        capView4Ui.PrintSummaryReportId = customCapView4Ui.PrintSummaryReportId;
                        capView4Ui.PrintSummaryReportName = customCapView4Ui.PrintSummaryReportName;
                        capView4Ui.PrintSummaryReportType = customCapView4Ui.PrintSummaryReportType;

                        //PrintReceiptReport button
                        capView4Ui.IsShowPrintReceipt = customCapView4Ui.IsShowPrintReceipt;
                        capView4Ui.PrintReceiptReportId = customCapView4Ui.PrintReceiptReportId;
                        capView4Ui.PrintReceiptReportName = customCapView4Ui.PrintReceiptReportName;
                        capView4Ui.PrintReceiptReportType = customCapView4Ui.PrintReceiptReportType;

                        capView4Ui.DetailViewUrl = ApiUtil.ConstructRecordDetailUrl(
                            capView4Ui.ModuleName, 
                            capIDPartArray[0],
                            capIDPartArray[1], 
                            capIDPartArray[2], 
                            capView4Ui.AgencyCode);
                        capListjosn.Add(capView4Ui);
                    }
                }
            }

            Dictionary<string, object> dictionaryJson = new Dictionary<string, object>();

            if (lpListjosn.Count > 0)
            {
                dictionaryJson.Add("LP", lpListjosn);
            }

            if (apoListjosn.Count > 0)
            {
                dictionaryJson.Add("APO", apoListjosn);
            }

            if (capListjosn.Count > 0)
            {
                dictionaryJson.Add("CAP", capListjosn);
            }

            IsoDateTimeConverter timeConverter = new IsoDateTimeConverter { DateTimeFormat = I18nDateTimeUtil.ShortDatePattern };
            return JsonConvert.SerializeObject(dictionaryJson, Formatting.Indented, timeConverter);
        }

        /// <summary>
        /// Check current query type.
        /// </summary>
        /// <param name="type">query type</param>
        /// <returns>boolean value</returns>
        [NonAction]
        private bool CheckType(string type)
        {
            string[] typeStrings = { "CAP", "LP", "APO" };

            return typeStrings.Contains(type);
        }

        /// <summary>
        /// Get Filter Types
        /// </summary>
        /// <returns>Filter Types</returns>
        private string[] GetFilterTypes()
        {
            Dictionary<string, object> caechDictionary = (Dictionary<string, object>)ApiUtil.GetSession(CacheKey) ??
                                                         new Dictionary<string, object>();
            if (caechDictionary.ContainsKey("Filter"))
            {
                return caechDictionary["Filter"] as string[];
            }

            return new string[] { };
        }

        /// <summary>
        ///  Add Global Search to Cache
        /// </summary>
        /// <param name="lpList">LP List</param>
        /// <param name="apoList">APO list</param>
        /// <param name="capList">cap LIST</param>
        /// <param name="pageIndex">page index(The 10 page based)</param>
        /// <param name="filterFieldStrings">filter Field </param>
        [NonAction]
        private void AddToCache(List<CustomLpView4Ui> lpList, List<CustomApoView4Ui> apoList, List<CustomCapView4Ui> capList, int pageIndex = 0, string[] filterFieldStrings = null)
        {
            Dictionary<string, object> tempDictionary = (Dictionary<string, object>)ApiUtil.GetSession(CacheKey) ??
                                                    new Dictionary<string, object>();

            var caechDictionary = new Dictionary<string, object>();
            if (pageIndex >= 0)
            {
                caechDictionary.Add("pageIndex", pageIndex);
            }
            else
            {
                if (tempDictionary.ContainsKey("pageIndex"))
                {
                    caechDictionary.Add("pageIndex", tempDictionary["pageIndex"]);
                }
            }

            if (filterFieldStrings != null)
            {
                //There is not the case, then go to the cache
                if (!filterFieldStrings.Contains("LP"))
                {
                    var templpList = tempDictionary["LP"] as List<CustomLpView4Ui>;
                    templpList = templpList ?? new List<CustomLpView4Ui>();
                    lpList.Clear();
                    lpList.AddRange(templpList.ToArray());
                }

                if (!filterFieldStrings.Contains("APO"))
                {
                    var tempapoList = tempDictionary["APO"] as List<CustomApoView4Ui>;
                    tempapoList = tempapoList ?? new List<CustomApoView4Ui>();
                    apoList.Clear();
                    apoList.AddRange(tempapoList.ToArray());
                }

                if (!filterFieldStrings.Contains("CAP"))
                {
                    var tempcapList = tempDictionary["CAP"] as List<CustomCapView4Ui>;
                    tempcapList = tempcapList ?? new List<CustomCapView4Ui>();
                    capList.Clear();
                    capList.AddRange(tempcapList.ToArray());
                }

                caechDictionary.Add("Filter", filterFieldStrings);
            }

            if (filterFieldStrings == null && pageIndex > 0)
            {
                if (tempDictionary.ContainsKey("Filter"))
                {
                    caechDictionary.Add("Filter", tempDictionary["Filter"]);
                }
            }

            caechDictionary.Add("LP", lpList);
            caechDictionary.Add("APO", apoList);
            caechDictionary.Add("CAP", capList);
            ApiUtil.AddSession(caechDictionary, CacheKey);
        }

        /// <summary>
        /// Get  Cache Data
        /// </summary>
        /// <param name="lpList">LP list</param>
        /// <param name="apoList">APO List </param>
        /// <param name="capList">CAP List</param>
        /// <param name="pageIndex">page Index</param>
        /// <param name="filterFieldStrings"> Filter Field Array The default null</param>
        /// <param name="isAppend"> Append The default  False</param>
        [NonAction]
        private void GetCacheData(
            out List<CustomLpView4Ui> lpList,
            out List<CustomApoView4Ui> apoList,
            out List<CustomCapView4Ui> capList,
            out int pageIndex, 
            string[] filterFieldStrings = null, 
            bool isAppend = false)
        {
            Dictionary<string, object> caechDictionary = (Dictionary<string, object>)ApiUtil.GetSession(CacheKey) ??
                                                         new Dictionary<string, object>();
            pageIndex = 0;
            lpList = null;
            apoList = null;
            capList = null;
            string[] filterStrings = null;
            if (filterFieldStrings != null)
            {
                caechDictionary["Filter"] = filterFieldStrings;
            }

            if (caechDictionary.ContainsKey("Filter"))
            {
                filterStrings = caechDictionary["Filter"] as string[];
            }

            if (filterStrings != null || isAppend)
            {
                if (caechDictionary.ContainsKey("LP") && filterStrings.Contains("LP"))
                {
                    lpList = caechDictionary["LP"] as List<CustomLpView4Ui>;
                }

                if (caechDictionary.ContainsKey("APO") && filterStrings.Contains("APO"))
                {
                    apoList = caechDictionary["APO"] as List<CustomApoView4Ui>;
                }

                if (caechDictionary.ContainsKey("CAP") && filterStrings.Contains("CAP"))
                {
                    capList = caechDictionary["CAP"] as List<CustomCapView4Ui>;
                }

                if (caechDictionary.ContainsKey("pageIndex"))
                {
                    pageIndex = int.Parse(caechDictionary["pageIndex"] + string.Empty);
                }
            }
            else
            {
                if (caechDictionary.ContainsKey("LP"))
                {
                    lpList = caechDictionary["LP"] as List<CustomLpView4Ui>;
                }

                if (caechDictionary.ContainsKey("APO"))
                {
                    apoList = caechDictionary["APO"] as List<CustomApoView4Ui>;
                }

                if (caechDictionary.ContainsKey("CAP"))
                {
                    capList = caechDictionary["CAP"] as List<CustomCapView4Ui>;
                }

                if (caechDictionary.ContainsKey("pageIndex"))
                {
                    pageIndex = int.Parse(caechDictionary["pageIndex"] + string.Empty);
                }
            }

            lpList = lpList ?? new List<CustomLpView4Ui>();
            apoList = apoList ?? new List<CustomApoView4Ui>();
            capList = capList ?? new List<CustomCapView4Ui>();
        }

        /// <summary>
        /// UI model for bind json data.
        /// </summary>
        [Serializable]
        private class CustomApoView4Ui : APOView4UI
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="CustomApoView4Ui" /> class.
            /// </summary>
            /// <param name="apoView4Ui">apoView4 UI entity</param>
            /// <param name="sortID">sort id</param>
            public CustomApoView4Ui(APOView4UI apoView4Ui, int sortID)
            {
                this.ParcelSeqNbr = apoView4Ui.ParcelSeqNbr;
                this.ParcelNumber = apoView4Ui.ParcelNumber;
                this.OwnerSourceNumber = apoView4Ui.OwnerSourceNumber;
                this.OwnerSeqNumber = apoView4Ui.OwnerSeqNumber;
                this.OwnerName = apoView4Ui.OwnerName;
                this.AddressSourceNumber = apoView4Ui.AddressSourceNumber;
                this.AddressSeqNumber = apoView4Ui.AddressSeqNumber;
                this.AddressDescription = apoView4Ui.AddressDescription;
                SortID = sortID;
            }

            /// <summary>
            /// Prevents a default instance of the <see cref="CustomApoView4Ui" /> class from being created.
            /// </summary>
            private CustomApoView4Ui()
            {
            }

            /// <summary>
            /// Gets or sets Detail View Url 
            /// </summary>
            public string DetailViewUrl { get; set; }

            /// <summary>
            /// Gets or sets Sort ID
            /// </summary>
            public int SortID { get; set; }
        }

        /// <summary>
        /// UI model for bind json data.
        /// </summary>
        [Serializable]
        private class CustomLpView4Ui : LPView4UI
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="CustomLpView4Ui" /> class.
            /// </summary>
            /// <param name="lpView4">lP View4</param>
            /// <param name="sortID">sort ID</param>
            public CustomLpView4Ui(LPView4UI lpView4, int sortID)
            {
                this.AgencyCode = lpView4.AgencyCode;
                this.BusinessName = lpView4.BusinessName;
                this.LicenseNumber = lpView4.LicenseNumber;
                this.LicenseType = lpView4.LicenseType;
                this.LicensedProfessionalName = lpView4.LicensedProfessionalName;
                this.ResLicenseType = lpView4.ResLicenseType;
                SortID = sortID;
            }

            /// <summary>
            /// Gets or sets Detail View Url
            /// </summary>
            public string DetailViewUrl { get; set; }

            /// <summary>
            /// Gets or sets sort ID.
            /// </summary>
            public int SortID { get; set; }
        }
    }
}