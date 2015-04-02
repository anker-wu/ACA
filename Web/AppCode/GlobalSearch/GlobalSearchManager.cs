#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: GlobalSearchManager.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2009-2014
 *
 *  Description:
 *  
 *
 *  Notes:
 *  $Id: GlobalSearchManager.cs 130988 2009-8-20  10:13:01Z ACHIEVO\jackie.yu $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Text.RegularExpressions;

using Accela.ACA.BLL.GlobalSearch;
using Accela.ACA.Common;
using Accela.ACA.Common.Util;
using Accela.ACA.WSProxy;
using Accela.Web.Controls;

namespace Accela.ACA.Web.Common.GlobalSearch
{
    /// <summary>
    /// Global search manager
    /// </summary>
    public sealed class GlobalSearchManager
    {
        #region Private Fields

        /// <summary>
        /// The page count for export
        /// </summary>
        private const int PAGE_COUNT_FOR_EXPORT = 1000;

        /// <summary>
        /// count label pattern
        /// </summary>
        private const string COUNT_LABEL_PATTERN = "{0}+";

        /// <summary>
        /// separator for APO by address
        /// </summary>
        private const string SEPARATOR_FOR_APO_BY_ADDRESS = @"*||*";

        #endregion

        #region Major Methods

        /// <summary>
        /// Get search result when user click history button
        /// </summary>
        /// <typeparam name="T">CAPView4UI, LPView4UI, APOView4UI</typeparam>
        /// <param name="searchType">CAP, License Professional, APO</param>
        /// <param name="hiddenViewElementNames">an array of hidden element name.</param>
        /// <returns>The collection depends on the generic</returns>
        public static List<T> ExecuteQuery<T>(GlobalSearchType searchType, string[] hiddenViewElementNames) where T : class, new()
        {
            List<T> list = null;
            GlobalSearchParameter parameter = GetGlobalSearchParameter(searchType);

            if (parameter != null)
            {
                list = ExecuteQuery<T>(parameter, hiddenViewElementNames);
            }

            return list;
        }

        /// <summary>
        /// Get search result when user select cap module from dropdownlist
        /// </summary>
        /// <typeparam name="T">CAPView4UI, LPView4UI, APOView4UI</typeparam>
        /// <param name="searchType">CAP, License Professional, APO</param>
        /// <param name="moduleArray">The module array.</param>
        /// <param name="hiddenViewElementNames">an array of hidden element name.</param>
        /// <returns>The collection depends on the generic</returns>
        public static List<T> ExecuteQuery<T>(GlobalSearchType searchType, string[] moduleArray, string[] hiddenViewElementNames) where T : class, new()
        {
            List<T> list = null;
            GlobalSearchParameter parameter = GetGlobalSearchParameter(searchType);

            if (parameter != null)
            {
                parameter.GlobalSearchType = searchType;
                parameter.PageIndex = 0;
                parameter.PaginationInfoCollection = new List<PaginationInfo>();
                parameter.ModuleArray = moduleArray;
                parameter.CapResults = null;
                parameter.LpResults = null;
                parameter.ApoResults = null;

                list = ExecuteQuery<T>(parameter, hiddenViewElementNames);
            }

            return list;
        }

        /// <summary>
        /// Get search result when user click search button
        /// </summary>
        /// <typeparam name="T">CAPView4UI, LPView4UI, APOView4UI</typeparam>
        /// <param name="searchType">CAP, License Professional, APO</param>
        /// <param name="queryText">query input value</param>
        /// <param name="moduleArray">The module array.</param>
        /// <param name="sortColumn">The sort column.</param>
        /// <param name="sortDirection">The sort direction.</param>
        /// <param name="pageSize">grid view pageSize.</param>
        /// <param name="hiddenViewElementNames">an array of hidden element name.</param>
        /// <returns>The search result</returns>
        public static List<T> ExecuteQuery<T>(GlobalSearchType searchType, string queryText, string[] moduleArray, string sortColumn, string sortDirection, int pageSize, string[] hiddenViewElementNames) where T : class, new()
        {
            List<T> list = null;

            // Initialize history action
            GlobalSearchManager.InitializeHistoryAction(searchType, queryText, moduleArray, sortColumn, sortDirection, pageSize);
            GlobalSearchParameter parameter = GetGlobalSearchParameter(searchType);

            if (parameter != null)
            {
                list = ExecuteQuery<T>(parameter, hiddenViewElementNames);
            }

            return list;
        }

        /// <summary>
        /// Get search result when user clicks column for sorting
        /// </summary>
        /// <typeparam name="T">CAPView4UI, LPView4UI, APOView4UI</typeparam>
        /// <param name="searchType">search type</param>
        /// <param name="sortColumn">sort column name</param>
        /// <param name="sortDirection">sort direction</param>
        /// <param name="hiddenViewElementNames">an array of hidden element name</param>
        /// <returns>The search result</returns>
        public static List<T> ExecuteQuery<T>(GlobalSearchType searchType, string sortColumn, string sortDirection, string[] hiddenViewElementNames) where T : class, new()
        {
            List<T> list = null;
            GlobalSearchParameter parameter = GetGlobalSearchParameter(searchType);

            if (parameter != null)
            {
                parameter.PageIndex = 0;
                parameter.PaginationInfoCollection = new List<PaginationInfo>();
                parameter.SortColulmn = GlobalSearchUtil.GetJavaPropertyName(searchType, sortColumn);
                parameter.SortDirection = sortDirection;
                parameter.CapResults = null;
                parameter.LpResults = null;
                parameter.ApoResults = null;

                list = ExecuteQuery<T>(parameter, hiddenViewElementNames);
            }

            return list;
        }

        /// <summary>
        /// Get search result when paging
        /// </summary>
        /// <typeparam name="T">CAPView4UI, LPView4UI, APOView4UI</typeparam>
        /// <param name="searchType">search type</param>
        /// <param name="pageIndex">the current page index</param>
        /// <param name="hiddenViewElementNames">hidden element name array</param>
        /// <returns>The search result</returns>
        public static List<T> ExecuteQuery<T>(GlobalSearchType searchType, int pageIndex, string[] hiddenViewElementNames) where T : class, new()
        {
            List<T> list = null;
            GlobalSearchParameter parameter = GetGlobalSearchParameter(searchType);

            if (parameter != null)
            {
                parameter.PageIndex = pageIndex;

                list = ExecuteQuery<T>(parameter, hiddenViewElementNames);
            }

            return list;
        }

        /// <summary>
        /// Execute query for export
        /// </summary>
        /// <typeparam name="T">CAPView4UI, LPView4UI, APOView4UI</typeparam>
        /// <param name="searchType">search type</param>
        /// <param name="hiddenViewElementNames">hidden element name array</param>
        /// <returns>The search result</returns>
        public static List<T> ExecuteQuery4Export<T>(GlobalSearchType searchType, string[] hiddenViewElementNames) where T : class, new()
        {
            List<T> list = null;
            GlobalSearchParameter historyParam = GetGlobalSearchParameter(searchType);

            if (historyParam != null)
            {
                GlobalSearchParameter parameter = new GlobalSearchParameter(searchType);
                parameter.ModuleArray = historyParam.ModuleArray;
                parameter.QueryText = historyParam.QueryText;
                parameter.RecordCount = PAGE_COUNT_FOR_EXPORT;
                parameter.SortColulmn = historyParam.SortColulmn;
                parameter.SortDirection = historyParam.SortDirection;

                // used to keep converted UI data
                list = new List<T>();
                GlobalSearchResult4WS result = null;

                if (parameter != null && !string.IsNullOrEmpty(parameter.QueryText))
                {
                    result = GetResultFromWS(parameter, hiddenViewElementNames);
                }

                if (result != null)
                {
                    switch (parameter.GlobalSearchType)
                    {
                        case GlobalSearchType.CAP:
                            list = ConvertToCapViewList<T>(result.capViews);
                            break;
                        case GlobalSearchType.LP:
                            list = ConvertToLPViewList<T>(result.refLPViews);
                            break;
                        case GlobalSearchType.PARCEL:
                            List<APOView4UI> apoListByParcel = ConvertToAPOViewList(result.refParcelViews, parameter, parameter.PageIndex);
                            list = GetAPOViewList4Render<T>(apoListByParcel, false, parameter);
                            break;
                        case GlobalSearchType.ADDRESS:
                            List<APOView4UI> apoListByAddress = ConvertToAPOViewList(result.refAddressViews, parameter, parameter.PageIndex);
                            list = GetAPOViewList4Render<T>(apoListByAddress, false, parameter);
                            break;
                    }
                }
            }

            return list;
        }

        /// <summary>
        /// Export search result
        /// </summary>
        /// <param name="grid">AccelaGridView control</param>
        /// <param name="searchType">search type</param>
        /// <param name="hiddenViewElementNames">hidden viewElement Name array</param>
        public static void Export(AccelaGridView grid, string searchType, string[] hiddenViewElementNames)
        {
            if (grid != null)
            {
                GlobalSearchType globalSearchType = (GlobalSearchType)Enum.Parse(typeof(GlobalSearchType), searchType, true);

                switch (globalSearchType)
                {
                    case GlobalSearchType.CAP:
                        List<CAPView4UI> capList = ExecuteQuery4Export<CAPView4UI>(globalSearchType, hiddenViewElementNames);
                        grid.Export<CAPView4UI>(capList);
                        break;
                    case GlobalSearchType.LP:
                        List<LPView4UI> lpList = ExecuteQuery4Export<LPView4UI>(globalSearchType, hiddenViewElementNames);
                        grid.Export<LPView4UI>(lpList);
                        break;
                    case GlobalSearchType.PARCEL:
                    case GlobalSearchType.ADDRESS:
                        List<APOView4UI> apoList = ExecuteQuery4Export<APOView4UI>(globalSearchType, hiddenViewElementNames);
                        grid.Export<APOView4UI>(apoList);
                        break;
                }

                grid.DataBind();
            }
        }

        /// <summary>
        /// get history script string
        /// </summary>
        /// <returns>the history script string</returns>
        public static string GetHistoryScript()
        {
            string result = string.Empty;

            if (AppSession.GlobalSearchHistoryAction == null)
            {
                result = "if(globalHistory != null){globalHistory.setNoResult();}";
            }
            else
            {
                string queryKeys = AppSession.GlobalSearchHistoryAction.GetQueryTexts(true);
                string queryTexts = AppSession.GlobalSearchHistoryAction.GetQueryTexts(false);
                string queryTextProp = string.Format("var queryKeys = {0};globalHistory.queryTexts = {1};", queryKeys, queryTexts);
                result = string.Concat("if(globalHistory != null){", queryTextProp, "globalHistory.rebuild();}");
            }

            return result;
        }

        /// <summary>
        /// Add or update query text
        /// </summary>
        /// <param name="queryText">the query text</param>
        public static void AddOrUpdateQueryText(string queryText)
        {
            if (AppSession.GlobalSearchHistoryAction != null)
            {
                AppSession.GlobalSearchHistoryAction.UpdateQueryText(queryText);
            }
        }
        
        /// <summary>
        /// Get query text from history
        /// </summary>
        /// <param name="searchType">search type</param>
        /// <returns>query text</returns>
        public static string GetQueryText(GlobalSearchType searchType)
        {
            GlobalSearchParameter historyParam = GetGlobalSearchParameter(searchType);
            string queryText = string.Empty;

            if (historyParam != null)
            {
                queryText = historyParam.QueryText;
            }

            return queryText;
        }

        /// <summary>
        /// Get history parameter from session
        /// </summary>
        /// <param name="searchType">The search type [CAP | LP | APO]</param>
        /// <returns>the search parameter</returns>
        public static GlobalSearchParameter GetGlobalSearchParameter(GlobalSearchType searchType)
        {
            GlobalSearchParameter parameter = null;

            if (AppSession.GlobalSearchHistoryAction != null)
            {
                switch (searchType)
                {
                    case GlobalSearchType.CAP:
                        if (AppSession.GlobalSearchHistoryAction.CapSearchParameter == null)
                        {
                            AppSession.GlobalSearchHistoryAction.CapSearchParameter = new GlobalSearchParameter(GlobalSearchType.CAP);
                        }

                        parameter = AppSession.GlobalSearchHistoryAction.CapSearchParameter;
                        break;
                    case GlobalSearchType.LP:
                        if (AppSession.GlobalSearchHistoryAction.LPSearchParameter == null)
                        {
                            AppSession.GlobalSearchHistoryAction.LPSearchParameter = new GlobalSearchParameter(GlobalSearchType.LP);
                        }

                        parameter = AppSession.GlobalSearchHistoryAction.LPSearchParameter;
                        break;
                    case GlobalSearchType.PARCEL:
                        if (AppSession.GlobalSearchHistoryAction.APOSearchParameter == null)
                        {
                            AppSession.GlobalSearchHistoryAction.APOSearchParameter = new GlobalSearchParameter(GlobalSearchType.PARCEL);
                        }

                        parameter = AppSession.GlobalSearchHistoryAction.APOSearchParameter;
                        break;
                    case GlobalSearchType.ADDRESS:
                        if (AppSession.GlobalSearchHistoryAction.APOSearchParameter == null)
                        {
                            AppSession.GlobalSearchHistoryAction.APOSearchParameter = new GlobalSearchParameter(GlobalSearchType.ADDRESS);
                        }

                        parameter = AppSession.GlobalSearchHistoryAction.APOSearchParameter;
                        break;
                }
            }

            return parameter;
        }

        /// <summary>
        /// Clears the history action.
        /// </summary>
        public static void ClearHistoryAction()
        {
            if (AppSession.GlobalSearchHistoryAction != null)
            {
                AppSession.GlobalSearchHistoryAction.APOSearchParameter = null;
                AppSession.GlobalSearchHistoryAction.CapSearchParameter = null;
                AppSession.GlobalSearchHistoryAction.LPSearchParameter = null;
            }
        }

        #endregion

        #region Utility Methods

        /// <summary>
        /// Get json string
        /// </summary>
        /// <param name="queryText">the query text</param>
        /// <returns>json string</returns>
        internal static string GetJsonString(string queryText)
        {
            DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(string));

            using (MemoryStream ms = new MemoryStream())
            {
                serializer.WriteObject(ms, queryText);
                return Encoding.UTF8.GetString(ms.ToArray());
            }
        }

        /// <summary>
        /// Get search result when change page index or resort column
        /// </summary>
        /// <typeparam name="T">CAPView4UI, LPView4UI, APOView4UI</typeparam>
        /// <param name="parameter">CAP, License Professional, APO</param>
        /// <param name="hiddenViewElementNames">an array of hidden element name.</param>
        /// <returns>The search result</returns>
        private static List<T> ExecuteQuery<T>(GlobalSearchParameter parameter, string[] hiddenViewElementNames) where T : class, new()
        {
            // used to keep converted UI data
            List<T> list = new List<T>();
            List<T> resultList = new List<T>();
            GlobalSearchResult4WS result = null;
            bool isRemained = true;

            if (parameter != null && !string.IsNullOrEmpty(parameter.QueryText))
            {
                result = GetResultFromWS(parameter, hiddenViewElementNames);
            }

            if (result != null)
            {
                List<APOView4UI> remainedAPOList = null;
                int count = 0;

                switch (parameter.GlobalSearchType)
                {
                    case GlobalSearchType.CAP:
                        list = ConvertToCapViewList<T>(result.capViews);
                        count = list.Count;
                        break;
                    case GlobalSearchType.LP:
                        list = ConvertToLPViewList<T>(result.refLPViews);
                        count = list.Count;
                        break;
                    case GlobalSearchType.PARCEL:
                        List<APOView4UI> apoListByParcel = ConvertToAPOViewList(result.refParcelViews, parameter, parameter.PageIndex);
                        count = apoListByParcel.Count;

                        if (result.refParcelViews != null && result.refParcelViews.Length < parameter.PageSize * ACAConstant.DEFAULT_PAGECOUNT)
                        {
                            isRemained = false;
                        }
                        else
                        {
                            remainedAPOList = GetRemainedAPOViewList(apoListByParcel, parameter);
                        }

                        list = GetAPOViewList4Render<T>(apoListByParcel, isRemained, parameter);

                        break;
                    case GlobalSearchType.ADDRESS:
                        List<APOView4UI> apoListByAddress = ConvertToAPOViewList(result.refAddressViews, parameter, parameter.PageIndex);
                        count = apoListByAddress.Count;

                        if (result.refAddressViews != null && result.refAddressViews.Length < parameter.PageSize * ACAConstant.DEFAULT_PAGECOUNT)
                        {
                            isRemained = false;
                        }
                        else
                        {
                            remainedAPOList = GetRemainedAPOViewList(apoListByAddress, parameter);
                        }

                        list = GetAPOViewList4Render<T>(apoListByAddress, isRemained, parameter);

                        break;
                }

                parameter.UpdatePaginationInfo(count, result.startNumber, parameter, remainedAPOList);
            }

            resultList = FillEmptyList<T>(list, parameter, isRemained);

            // update total record number
            parameter.TotalRecordsFromWS = resultList.Count;

            return resultList;
        }

        /// <summary>
        /// Fill empty list
        /// </summary>
        /// <typeparam name="T">CAPView4UI, LPView4UI, APOView4UI</typeparam>
        /// <param name="currentList">current list.</param>
        /// <param name="parameter">global search parameter</param>
        /// <param name="isRemained">false if the result from web service less than PageSize*PagerButtonCount</param>
        /// <returns>the result with null data</returns>
        private static List<T> FillEmptyList<T>(List<T> currentList, GlobalSearchParameter parameter, bool isRemained) where T : class, new()
        {
            if (parameter == null || currentList == null || currentList.Count == 0)
            {
                return currentList;
            }

            int totals = 0;

            if (parameter.GlobalSearchType == GlobalSearchType.PARCEL || parameter.GlobalSearchType == GlobalSearchType.ADDRESS)
            {
                totals = parameter.GetPreviousTotalCounts();
            }
            else
            {
                int pageGroupCount = parameter.PageIndex / ACAConstant.DEFAULT_PAGECOUNT;
                totals = pageGroupCount * ACAConstant.DEFAULT_PAGECOUNT * parameter.PageSize;
            }

            //totalItem=left data (value is null) + current data(search from DB) + right data (value is null)
            List<T> totalItems = new List<T>();
            List<T> leftItems = new List<T>();
            List<T> rightItems = new List<T>();

            for (int i = 0; i < totals; i++)
            {
                leftItems.Add(null);
            }

            totalItems.AddRange(leftItems);

            totalItems.AddRange(currentList);

            //add right data of current pages data.
            if (totalItems.Count < parameter.TotalRecordsFromWS)
            {
                for (int j = 0; j < parameter.TotalRecordsFromWS - totalItems.Count; j++)
                {
                    rightItems.Add(null);
                }
            }

            totalItems.AddRange(rightItems);

            return totalItems;
        }

        /// <summary>
        /// Get search result
        /// </summary>
        /// <param name="parameter">the input parameter</param>
        /// <param name="hiddenViewElementNames">an array of hidden element name.</param>
        /// <returns>the search result from web service</returns>
        private static GlobalSearchResult4WS GetResultFromWS(GlobalSearchParameter parameter, string[] hiddenViewElementNames)
        {
            if (parameter == null)
            {
                return null;
            }

            GlobalSearchParam4WS param = new GlobalSearchParam4WS();
            param.moduleArray = parameter.ModuleArray;
            param.queryString = parameter.QueryText;
            param.searchType = parameter.GlobalSearchType.ToString().ToUpperInvariant();
            param.servProvCode = ACAConstant.AgencyCode;
            param.sortColumn = parameter.SortColulmn;
            param.sortDirection = parameter.SortDirection;

            if (parameter.StartNumber == 0)
            {
                if (parameter.RecordCount > 0)
                {
                    param.recordCount = parameter.RecordCount;
                    param.recordStartNumber = 0;
                }
                else
                {
                    param.recordCount = (parameter.PageSize * ACAConstant.DEFAULT_PAGECOUNT) + ACAConstant.ADDITIONAL_RECORDS_COUNT;
                    param.recordStartNumber = parameter.GetCurrentStartNumber();
                }
            }
            else
            {
                param.recordCount = (parameter.PageSize * ACAConstant.DEFAULT_PAGECOUNT) + ACAConstant.ADDITIONAL_RECORDS_COUNT;
                param.recordStartNumber = parameter.StartNumber;
            }

            IGlobalSearchBll globalSearchBll = ObjectFactory.GetObject(typeof(IGlobalSearchBll)) as IGlobalSearchBll;
            return globalSearchBll.ExecuteQuery(param, hiddenViewElementNames);
        }

        /// <summary>
        /// Get result from session
        /// </summary>
        /// <typeparam name="T">CAPView4UI, LPView4UI, APOView4UI</typeparam>
        /// <param name="parameter">global search parameter</param>
        /// <returns>the result from session</returns>
        private static List<T> GetResultFromSession<T>(GlobalSearchParameter parameter) where T : class, new()
        {
            List<T> list = new List<T>();

            switch (parameter.GlobalSearchType)
            {
                case GlobalSearchType.CAP:
                    list = parameter.CapResults as List<T>;
                    break;
                case GlobalSearchType.LP:
                    list = parameter.LpResults as List<T>;
                    break;
                case GlobalSearchType.PARCEL:
                case GlobalSearchType.ADDRESS:
                    list = parameter.ApoResults as List<T>;
                    break;
            }

            return list;
        }

        /// <summary>
        /// Set result to session
        /// </summary>
        /// <typeparam name="T">CAPView4UI, LPView4UI, APOView4UI</typeparam>
        /// <param name="parameter">global search parameter</param>
        /// <param name="list">the result list</param>
        private static void SetResultToSession<T>(GlobalSearchParameter parameter, List<T> list) where T : class, new()
        {
            switch (parameter.GlobalSearchType)
            {
                case GlobalSearchType.CAP:
                    parameter.CapResults = list as List<CAPView4UI>;
                    break;
                case GlobalSearchType.LP:
                    parameter.LpResults = list as List<LPView4UI>;
                    break;
                case GlobalSearchType.PARCEL:
                case GlobalSearchType.ADDRESS:
                    parameter.ApoResults = list as List<APOView4UI>;
                    break;
            }
        }

        /// <summary>
        /// Clone parameter
        /// </summary>
        /// <param name="parameter">global search parameter</param>
        /// <returns>the cloned parameter</returns>
        private static GlobalSearchParameter CloneParameter(GlobalSearchParameter parameter)
        {
            if (parameter == null)
            {
                return null;
            }

            GlobalSearchParameter cloneParam = new GlobalSearchParameter(parameter.GlobalSearchType);
            cloneParam.ModuleArray = parameter.ModuleArray;
            cloneParam.PageIndex = parameter.PageIndex;
            cloneParam.PageSize = parameter.PageSize;
            cloneParam.QueryText = parameter.QueryText;
            cloneParam.RecordCount = parameter.RecordCount;
            cloneParam.SortColulmn = parameter.SortColulmn;
            cloneParam.SortDirection = parameter.SortDirection;
            cloneParam.PaginationInfoCollection = parameter.PaginationInfoCollection;

            return cloneParam;
        }

        #endregion

        #region History Manage

        /// <summary>
        /// Initialize history action.
        /// </summary>
        /// <param name="searchType">CAP, License Professional, APO</param>
        /// <param name="queryText">the search text</param>
        /// <param name="paramArray">parameter array</param>
        /// <param name="sortColumn">The sort column.</param>
        /// <param name="sortDirection">The sort direction.</param>
        /// <param name="pageSize">grid view pageSize.</param>
        private static void InitializeHistoryAction(GlobalSearchType searchType, string queryText, string[] paramArray, string sortColumn, string sortDirection, int pageSize)
        {
            // Clear session first
            if (AppSession.GlobalSearchHistoryAction == null)
            {
                AppSession.GlobalSearchHistoryAction = new HistoryAction();
            }

            GlobalSearchParameter parameter = GetGlobalSearchParameter(searchType);

            if (parameter != null)
            {
                if (paramArray != null)
                {
                    parameter.ModuleArray = paramArray;
                }
                else
                {
                    parameter.ModuleArray = new string[] { string.Empty };
                }

                parameter.PageIndex = 0;
                parameter.RecordCount = 0;
                parameter.PageSize = pageSize;
                parameter.QueryText = queryText;
                parameter.SortColulmn = sortColumn;
                parameter.TotalRecordsFromWS = 0;
                parameter.SortDirection = sortDirection;
                parameter.GlobalSearchType = searchType;
                parameter.PaginationInfoCollection = new List<PaginationInfo>();
            }
        }

        #endregion

        #region Result Convertor

        /// <summary>
        /// Convert result list for UI
        /// </summary>
        /// <typeparam name="T">CAPView4UI, LPView4UI, APOView4UI</typeparam>
        /// <param name="capArray">The collection of CapView from web service</param>
        /// <returns>The CapView4UI collection</returns>
        private static List<T> ConvertToCapViewList<T>(CapView[] capArray) where T : class, new()
        {
            List<CAPView4UI> capViewList = new List<CAPView4UI>();

            if (capArray != null && capArray.Length > 0)
            {
                foreach (CapView capView4WS in capArray)
                {
                    CAPView4UI capView4UI = new CAPView4UI();

                    capView4UI.AgencyCode = capView4WS.servProvCode.ToUpperInvariant();
                    capView4UI.Description = capView4WS.shortNotes;
                    capView4UI.ModuleName = capView4WS.moduleName;
                    capView4UI.PermitNumber = capView4WS.altId;
                    capView4UI.PermitType = string.IsNullOrEmpty(capView4WS.capTypeAlias) ? capView4WS.capType : capView4WS.capTypeAlias;
                    capView4UI.ProjectName = capView4WS.projectName;
                    capView4UI.Status = capView4WS.capStatus;
                    capView4UI.CapClass = capView4WS.capClass;
                    capView4UI.CapID = GetCAPID(capView4WS.id);
                    capView4UI.Address = capView4WS.location;
                    capView4UI.RelatedRecords = capView4WS.relatedRecordsCount;

                    if (capView4WS.createdDate != null)
                    {
                        capView4UI.CreatedDate = I18nDateTimeUtil.ParseFromWebService(capView4WS.createdDate);
                    }

                    capViewList.Add(capView4UI);
                }
            }

            return capViewList as List<T>;
        }

        /// <summary>
        /// Gets the real CAP ID.
        /// </summary>
        /// <param name="combinedID">The combined ID.</param>
        /// <returns>the real CAP ID.</returns>
        private static string GetCAPID(string combinedID)
        {
            string result = combinedID;

            if (!string.IsNullOrEmpty(combinedID))
            {
                int index = combinedID.IndexOf(ACAConstant.SPLIT_CHAR4);
                result = combinedID.Substring(index + 1);
            }

            return result;
        }

        /// <summary>
        /// Convert result list for UI
        /// </summary>
        /// <typeparam name="T">CAPView4UI, LPView4UI, APOView4UI</typeparam>
        /// <param name="refLPArray">The collection of RefLPView from web service</param>
        /// <returns>The LPView4UI collection</returns>
        private static List<T> ConvertToLPViewList<T>(RefLPView[] refLPArray) where T : class, new()
        {
            List<LPView4UI> lpViewList = new List<LPView4UI>();

            if (refLPArray != null && refLPArray.Length > 0)
            {
                foreach (RefLPView lpView4WS in refLPArray)
                {
                    LPView4UI lpview4UI = new LPView4UI();

                    lpview4UI.AgencyCode = lpView4WS.servProvCode.ToUpperInvariant();
                    lpview4UI.BusinessName = lpView4WS.businessName;
                    lpview4UI.LicensedProfessionalName = lpView4WS.contact;
                    lpview4UI.LicenseNumber = lpView4WS.licenseNumber;
                    lpview4UI.LicenseType = lpView4WS.licenseType;
                    lpview4UI.ResLicenseType = I18nStringUtil.GetString(lpView4WS.resLicenseType, lpView4WS.licenseType);

                    lpViewList.Add(lpview4UI);
                }
            }

            return lpViewList as List<T>;
        }

        /// <summary>
        /// Convert result list for UI
        /// </summary>
        /// <param name="parcelArray">The collection of RefParcelView from web service</param>
        /// <param name="historyParam">history parameter</param>
        /// <param name="pageIndex">the page index</param>
        /// <returns>The APOView4UI collection</returns>
        private static List<APOView4UI> ConvertToAPOViewList(RefParcelView[] parcelArray, GlobalSearchParameter historyParam, int pageIndex)
        {
            List<APOView4UI> apoViewList = new List<APOView4UI>();

            // Add previous remain records to the result list
            List<APOView4UI> remainList = historyParam.GetRemainedRecords(pageIndex);

            if (remainList != null && remainList.Count > 0)
            {
                apoViewList.AddRange(remainList);
            }

            if (parcelArray != null && parcelArray.Length > 0)
            {
                foreach (RefParcelView parcelView4WS in parcelArray)
                {
                    List<APOView4UI> apoView4UICollection = GetPropertyInformation(parcelView4WS);
                    apoViewList.AddRange(apoView4UICollection);
                }
            }

            return apoViewList;
        }

        /// <summary>
        /// Convert result list for UI
        /// </summary>
        /// <param name="addressViewArray">The address view array.</param>
        /// <param name="historyParam">history parameter</param>
        /// <param name="pageIndex">the page index</param>
        /// <returns>The APOView4UI collection</returns>
        private static List<APOView4UI> ConvertToAPOViewList(RefAddressView[] addressViewArray, GlobalSearchParameter historyParam, int pageIndex)
        {
            List<APOView4UI> apoViewList = new List<APOView4UI>();

            // Add previous remain records to the result list
            List<APOView4UI> remainList = historyParam.GetRemainedRecords(pageIndex);

            if (remainList != null && remainList.Count > 0)
            {
                apoViewList.AddRange(remainList);
            }

            if (addressViewArray != null && addressViewArray.Length > 0)
            {
                foreach (RefAddressView addressView4WS in addressViewArray)
                {
                    List<APOView4UI> apoView4UICollection = GetPropertyInformation(addressView4WS);
                    apoViewList.AddRange(apoView4UICollection);
                }
            }

            return apoViewList;
        }

        /// <summary>
        /// Convert result list for UI
        /// </summary>
        /// <typeparam name="T">CAPView4UI, LPView4UI, APOView4UI</typeparam>
        /// <param name="apoViewList">APO result list</param>
        /// <param name="isRemained">false if the result from web service less than 100</param>
        /// <param name="parameter">The global search parameter</param>
        /// <returns>The APOView4UI collection</returns>
        private static List<T> GetAPOViewList4Render<T>(List<APOView4UI> apoViewList, bool isRemained, GlobalSearchParameter parameter) where T : class, new()
        {
            List<APOView4UI> newAPOViewList = apoViewList;

            // Check if the APOview collection has residual records
            if (apoViewList != null && apoViewList.Count > parameter.PageSize * ACAConstant.DEFAULT_PAGECOUNT)
            {
                int remains = isRemained ? (apoViewList.Count % (parameter.PageSize * ACAConstant.DEFAULT_PAGECOUNT)) - ACAConstant.ADDITIONAL_RECORDS_COUNT : 0;
                int count = apoViewList.Count - remains;
                newAPOViewList = apoViewList.GetRange(0, count);
            }

            return newAPOViewList as List<T>;
        }

        /// <summary>
        /// Get remained APO list, for example if apoViewList contains 306 records, the remain list is 6
        /// </summary>
        /// <param name="apoViewList">APO result list</param>
        /// <param name="parameter">The global search parameter</param>
        /// <returns>the remain APO records</returns>
        private static List<APOView4UI> GetRemainedAPOViewList(List<APOView4UI> apoViewList, GlobalSearchParameter parameter)
        {
            List<APOView4UI> remainedAPOViewList = new List<APOView4UI>();

            // Check if the APOview collection has residual records
            if (apoViewList != null && apoViewList.Count > parameter.PageSize * ACAConstant.DEFAULT_PAGECOUNT)
            {
                int remains = (apoViewList.Count % (parameter.PageSize * ACAConstant.DEFAULT_PAGECOUNT)) - ACAConstant.ADDITIONAL_RECORDS_COUNT;
                int count = apoViewList.Count - remains;
                remainedAPOViewList = apoViewList.GetRange(count, remains);
            }

            return remainedAPOViewList;
        }

        /// <summary>
        /// Get APO View for UI
        /// </summary>
        /// <param name="parcelView4WS">the RefParcelView4WS</param>
        /// <returns>APOView4UI collection</returns>
        private static List<APOView4UI> GetPropertyInformation(RefParcelView parcelView4WS)
        {
            string separators4null = string.Concat(ACAConstant.COMMA, ACAConstant.SPLIT_CHAR5);

            // the format of owner from web service is "sourceSeqNumber,ownerNumber_ownerFullName", e.g. 45,1002_test@achievo.com
            // the format of address from web service is "sourceSeqNumber,addressNumber_addressDes", e.g. 45,2000_address desc
            if (parcelView4WS != null)
            {
                if (parcelView4WS.refOwnerName == null)
                {
                    parcelView4WS.refOwnerName = new string[] { separators4null };
                }

                if (parcelView4WS.fullAddress == null)
                {
                    parcelView4WS.fullAddress = new string[] { separators4null };
                }
            }

            List<APOView4UI> apoView4UICollection = GetAPOView4UICollection(parcelView4WS);

            return apoView4UICollection;
        }

        /// <summary>
        /// Get APO View for UI
        /// </summary>
        /// <param name="addressView4WS">The address view4 WS.</param>
        /// <returns>APOView4UI collection</returns>
        private static List<APOView4UI> GetPropertyInformation(RefAddressView addressView4WS)
        {
            string separators4null = SEPARATOR_FOR_APO_BY_ADDRESS;

            // the format of owner from web service is sourceSeqNumber,ownerNumber[char(16)]ownerFullName
            if (addressView4WS != null)
            {
                if (addressView4WS.primaryOwner == null)
                {
                    addressView4WS.primaryOwner = new string[] { separators4null };
                }

                if (addressView4WS.parcelNumber == null)
                {
                    addressView4WS.parcelNumber = new string[] { string.Empty };
                }
            }

            List<APOView4UI> apoView4UICollection = GetAPOView4UICollection(addressView4WS);

            return apoView4UICollection;
        }

        /// <summary>
        /// Get APOView4UI Collection 
        /// </summary>
        /// <param name="parcelView4WS">The RefParcelView4WS</param>
        /// <returns>APOView4UI collection</returns>
        private static List<APOView4UI> GetAPOView4UICollection(RefParcelView parcelView4WS)
        {
            string pattern = "^(.*?),(.*?)_(.*?)$";
            List<APOView4UI> apoViews = new List<APOView4UI>();

            // the format of address from web service is sourceNumber, addressNumber, addressDesc
            // e.g. 57,10011234_shenzhen
            if (parcelView4WS != null)
            {
                int addressArrayLength = parcelView4WS.fullAddress.Length;
                int ownerArrayLength = parcelView4WS.refOwnerName.Length;
                int maxLength = Math.Max(addressArrayLength, ownerArrayLength);

                for (int i = 0; i < maxLength; i++)
                {
                    APOView4UI view4UI = new APOView4UI();
                    view4UI.ParcelNumber = parcelView4WS.parcelNumber;
                    view4UI.ParcelSeqNbr = parcelView4WS.sourceSeqNbr;

                    //begin handling address
                    string refAddress = (i >= addressArrayLength) ? string.Empty : parcelView4WS.fullAddress[i];
                    refAddress = string.IsNullOrEmpty(refAddress) ? string.Empty : refAddress;
                    Match addressMatch = Regex.Match(refAddress, pattern);

                    if (addressMatch.Success)
                    {
                        view4UI.AddressSourceNumber = addressMatch.Groups[1].Value;
                        view4UI.AddressSeqNumber = addressMatch.Groups[2].Value;
                        view4UI.AddressDescription = addressMatch.Groups[3].Value;
                    }

                    //begin handling parcel
                    string refOwner = (i >= ownerArrayLength) ? string.Empty : parcelView4WS.refOwnerName[i];
                    refOwner = string.IsNullOrEmpty(refOwner) ? string.Empty : refOwner;
                    Match ownerMatch = Regex.Match(refOwner, pattern);

                    if (ownerMatch.Success)
                    {
                        view4UI.OwnerSourceNumber = ownerMatch.Groups[1].Value;
                        view4UI.OwnerSeqNumber = ownerMatch.Groups[2].Value;
                        view4UI.OwnerName = ownerMatch.Groups[3].Value;
                    }

                    //add to the collection
                    apoViews.Add(view4UI);
                }
            }

            return apoViews;
        }

        /// <summary>
        /// Get APOView4UI Collection
        /// </summary>
        /// <param name="addressView4WS">The address view4 WS.</param>
        /// <returns>APOView4UI collection</returns>
        private static List<APOView4UI> GetAPOView4UICollection(RefAddressView addressView4WS)
        {
            string primaryOwnerRowPattern = "^(.*?)_(.*?)$";
            List<APOView4UI> apoViews = new List<APOView4UI>();

            // the format of address from web service is sourceNumber, addressNumber, addressDesc
            // e.g. 57,10011234*||*shenzhen
            if (addressView4WS != null && addressView4WS.parcelNumber != null)
            {
                Regex ownerRegex = new Regex(primaryOwnerRowPattern, RegexOptions.Compiled | RegexOptions.CultureInvariant | RegexOptions.IgnoreCase);
                Dictionary<string, string> parcelAndOwnerMapping = GetParcelAndOwnerMapping(addressView4WS);

                foreach (string parcelNumber in addressView4WS.parcelNumber)
                {
                    APOView4UI view4UI = new APOView4UI();
                    view4UI.ParcelNumber = parcelNumber;
                    view4UI.ParcelSeqNbr = addressView4WS.sourceSeqNbr;
                    view4UI.AddressDescription = addressView4WS.fullAddress;
                    view4UI.AddressSeqNumber = addressView4WS.addressNumber;
                    view4UI.AddressSourceNumber = addressView4WS.sourceSeqNbr;

                    string primaryOwnerRow = parcelAndOwnerMapping.ContainsKey(parcelNumber) ? parcelAndOwnerMapping[parcelNumber] : string.Empty;
                    primaryOwnerRow = string.IsNullOrEmpty(primaryOwnerRow) ? string.Empty : primaryOwnerRow;
                    Match ownerMatch = ownerRegex.Match(primaryOwnerRow);

                    if (ownerMatch.Success)
                    {
                        view4UI.OwnerSourceNumber = addressView4WS.sourceSeqNbr;
                        view4UI.OwnerSeqNumber = ownerMatch.Groups[1].Value;
                        view4UI.OwnerName = ownerMatch.Groups[2].Value;
                    }

                    apoViews.Add(view4UI);
                }
            }

            return apoViews;
        }

        /// <summary>
        /// Gets the parcel and owner mapping.
        /// </summary>
        /// <param name="addressView4WS">The address view4 WS.</param>
        /// <returns>the parcel and owner mapping.</returns>
        private static Dictionary<string, string> GetParcelAndOwnerMapping(RefAddressView addressView4WS)
        {
            Dictionary<string, string> result = new Dictionary<string, string>();

            if (addressView4WS != null && addressView4WS.primaryOwner != null)
            {
                foreach (string ownerItem in addressView4WS.primaryOwner)
                {
                    if (string.IsNullOrEmpty(ownerItem))
                    {
                        continue;
                    }

                    string parcelNumber = string.Empty;
                    string primaryOwner = string.Empty;
                    int separatorIndex = ownerItem.IndexOf(SEPARATOR_FOR_APO_BY_ADDRESS, StringComparison.OrdinalIgnoreCase);

                    if (separatorIndex > 0)
                    {
                        parcelNumber = ownerItem.Substring(0, separatorIndex);
                        primaryOwner = ownerItem.Substring(separatorIndex + SEPARATOR_FOR_APO_BY_ADDRESS.Length);

                        if (!result.ContainsKey(parcelNumber))
                        {
                            result.Add(parcelNumber, primaryOwner);
                        }
                    }
                }
            }

            return result;
        }

        #endregion
    }
}