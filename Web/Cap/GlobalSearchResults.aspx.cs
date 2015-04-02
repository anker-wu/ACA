#region Header

/**
 * <pre>
 *
 *  Accela Citizen Access
 *  File: GlobalSearchResults.aspx.cs
 *
 *  Accela, Inc.
 *  Copyright (C): 2009-2014
 *
 *  Description:
 *  
 *
 *  Notes:
 *  $Id: GlobalSearchResults.aspx.cs 130988 2009-8-20  14:53:01Z ACHIEVO\jackie.yu $.
 *  Revision History
 *  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
 * </pre>
 */

#endregion Header

using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Globalization;
using System.Text;

using Accela.ACA.Common;
using Accela.ACA.Common.Util;
using Accela.ACA.Web.Common;
using Accela.ACA.Web.Common.Control;
using Accela.ACA.Web.Common.GlobalSearch;
using Accela.Web.Controls;

namespace Accela.ACA.Web.Cap
{
    /// <summary>
    /// Global search result page
    /// </summary>
    public partial class GlobalSearchResults : BasePage
    {
        #region Fields

        /// <summary>
        /// copy cap info event name.
        /// </summary>
        private const string IS_NEW_QUERY = "isNewQuery";

        /// <summary>
        /// First column name of CAP grid view
        /// </summary>
        private const string CAP_FIRST_COLUMN_NAME = "createdDate";

        /// <summary>
        /// First column name of CAP grid view for UI.
        /// </summary>
        private const string CAP_FIRST_COLUMN_NAME_FOR_UI = "CreatedDate";

        /// <summary>
        /// First column name of LP grid view
        /// </summary>
        private const string LP_FIRST_COLUMN_NAME = "licenseNumber";

        /// <summary>
        /// First column name of APO Address grid view
        /// </summary>
        private const string ADDRESS_FIRST_COLUMN_NAME = "fullAddress";

        /// <summary>
        /// First column name of APO Parcel grid view
        /// </summary>
        private const string PARCEL_FIRST_COLUMN_NAME = "parcelNumber";

        #endregion Fields

        /// <summary>
        /// Page onLoad event
        /// </summary>
        /// <param name="sender">the event object</param>
        /// <param name="e">the event argument</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            Server.ScriptTimeout = int.MaxValue;

            SetupCache();
            InitEventBinding();

            if (!IsPostBack)
            {
                DialogUtil.RegisterScriptForDialog(this);
                if (AppSession.IsAdmin)
                {
                    SetupAdminUI();
                }
                else
                {
                    PreHandleQueryParam();
                    HandleGlobalSearch();
                }
            }
        }

        /// <summary>
        /// Setups the cache.
        /// </summary>
        private void SetupCache()
        {
            Response.Cache.SetCacheability(System.Web.HttpCacheability.NoCache);
            Response.Cache.SetNoStore();
        }

        /// <summary>
        /// Initial the event binding.
        /// </summary>
        private void InitEventBinding()
        {
            CapView.ListLoading += new EventHandler(CapView_ListLoading);
            CapView.ListLoaded += new EventHandler(CapView_ListLoaded);
            LPView.ListLoading += new EventHandler(LPView_ListLoading);
            LPView.ListLoaded += new EventHandler(LPView_ListLoaded);
            APOView.ListLoading += new EventHandler(APOView_ListLoading);
            APOView.ListLoaded += new EventHandler(APOView_ListLoaded);
        }

        /// <summary>
        /// Handles the ListLoading event of the CapView control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void CapView_ListLoading(object sender, EventArgs e)
        {
        }

        /// <summary>
        /// Handles the ListLoaded event of the CapView control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void CapView_ListLoaded(object sender, EventArgs e)
        {
            SetupRecordCount(CapView.CountSummary, CapView.TotalCount, lblCAPNavigation, "per_globalsearch_label_caplink");
        }

        /// <summary>
        /// Handles the ListLoading event of the LPView control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void LPView_ListLoading(object sender, EventArgs e)
        {
        }

        /// <summary>
        /// Handles the ListLoaded event of the LPView control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void LPView_ListLoaded(object sender, EventArgs e)
        {
            SetupRecordCount(LPView.CountSummary, LPView.TotalCount, lblLPNavigation, "per_globalsearch_label_lplink");
        }

        /// <summary>
        /// Handles the ListLoading event of the APOView control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void APOView_ListLoading(object sender, EventArgs e)
        {
        }

        /// <summary>
        /// Handles the ListLoaded event of the APOView control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void APOView_ListLoaded(object sender, EventArgs e)
        {
            SetupRecordCount(APOView.CountSummary, APOView.TotalCount, lblAPONavigation, "per_globalsearch_label_apolink");
        }

        /// <summary>
        /// Setups the admin UI.
        /// </summary>
        private void SetupAdminUI()
        {
            pnlNoResults.Visible = false;
            pnlResultsHeader.Visible = false;
            pnlLoading.Visible = false;
            CapView.Display(null, false);
            LPView.Display(null, false);
            APOView.Display(null, false, GlobalSearchType.ADDRESS);
        }

        /// <summary>
        /// Prepare the handle query parameter.
        /// </summary>
        private void PreHandleQueryParam()
        {
            if (this.IsNewQuery())
            {
                GlobalSearchManager.ClearHistoryAction();

                NameValueCollection nameValues = new NameValueCollection(Request.QueryString);

                if (nameValues != null)
                {
                    nameValues.Remove(IS_NEW_QUERY);
                }

                string newQueryString = BuildQueryString(nameValues);
                string newPath = string.Concat(Request.Path, ACAConstant.QUESTION_MARK, newQueryString);
                Response.Redirect(newPath);
            }
        }

        /// <summary>
        /// Builds the query string.
        /// </summary>
        /// <param name="nameValues">The name values.</param>
        /// <returns>the new query string</returns>
        private string BuildQueryString(NameValueCollection nameValues)
        {
            string result = string.Empty;
            StringBuilder queryStringBuilder = new StringBuilder();

            if (nameValues != null)
            {
                foreach (string key in nameValues.Keys)
                {
                    if (queryStringBuilder.Length > 0)
                    {
                        queryStringBuilder.Append(ACAConstant.AMPERSAND);
                    }

                    string keyValue = string.Concat(key, ACAConstant.EQUAL_MARK, Server.UrlEncode(nameValues[key]));
                    queryStringBuilder.Append(keyValue);
                }

                result = queryStringBuilder.ToString();
            }

            return result;
        }

        /// <summary>
        /// Handles the global search.
        /// </summary>
        private void HandleGlobalSearch()
        {
            List<CAPView4UI> capList = null;
            List<LPView4UI> lpList = null;
            List<APOView4UI> apoList = null;
            int capRecordCount = 0;
            int lpRecordCount = 0;
            int apoRecordCount = 0;
            bool hasResults = false;
            string queryText = Request.QueryString.Get(ACAConstant.GLOBAL_SEARCH_QUERY_TEXT);
            bool isLoadingHistory = this.CanLoadHistory(queryText);
            var toBeShownAPOType = GlobalSearchType.ADDRESS;

            string[] hiddenElementNames = ControlBuildHelper.GetHiddenViewElementNames(GviewID.GlobalSearchCapList, ModuleName);
            if (!isLoadingHistory)
            {
                // Get query text from search input box
                string[] modules = GlobalSearchUtil.GetAllModuleKeys();

                if (GlobalSearchUtil.IsRecordEnabled())
                {
                    capList = GlobalSearchManager.ExecuteQuery<CAPView4UI>(
                                                                        GlobalSearchType.CAP,
                                                                        queryText,
                                                                        modules,
                                                                        CAP_FIRST_COLUMN_NAME,
                                                                        ACAConstant.ORDER_BY_DESC,
                                                                        CapView.PageSize,
                                                                        hiddenElementNames);
                    CapView.SetGridViewSortInfo(CAP_FIRST_COLUMN_NAME_FOR_UI, ACAConstant.ORDER_BY_DESC);
                }

                if (GlobalSearchUtil.IsLPEnabled())
                {
                    lpList = GlobalSearchManager.ExecuteQuery<LPView4UI>(
                                                                        GlobalSearchType.LP,
                                                                        queryText,
                                                                        modules,
                                                                        LP_FIRST_COLUMN_NAME,
                                                                        ACAConstant.ORDER_BY_ASC,
                                                                        LPView.PageSize,
                                                                        null);
                    LPView.SetGridViewSortInfo(LP_FIRST_COLUMN_NAME, ACAConstant.ORDER_BY_ASC);
                }

                if (GlobalSearchUtil.IsAPOEnabled())
                {
                    apoList = GlobalSearchManager.ExecuteQuery<APOView4UI>(
                                                                        GlobalSearchType.ADDRESS,
                                                                        queryText,
                                                                        modules,
                                                                        ADDRESS_FIRST_COLUMN_NAME,
                                                                        ACAConstant.ORDER_BY_ASC,
                                                                        APOView.PageSize,
                                                                        null);
                    APOView.SetGridViewSortInfo(ADDRESS_FIRST_COLUMN_NAME, ACAConstant.ORDER_BY_ASC);

                    if (apoList == null || apoList.Count == 0)
                    {
                        apoList = GlobalSearchManager.ExecuteQuery<APOView4UI>(
                                                                            GlobalSearchType.PARCEL,
                                                                            queryText,
                                                                            modules,
                                                                            PARCEL_FIRST_COLUMN_NAME,
                                                                            ACAConstant.ORDER_BY_ASC,
                                                                            APOView.PageSize,
                                                                            null);
                        APOView.SetGridViewSortInfo(PARCEL_FIRST_COLUMN_NAME, ACAConstant.ORDER_BY_ASC);
                        toBeShownAPOType = (apoList != null && apoList.Count > 0)
                                               ? GlobalSearchType.PARCEL
                                               : toBeShownAPOType;
                    }
                }
            }
            else
            {
                // Get query conditions from last history action
                capList = GlobalSearchUtil.IsRecordEnabled()
                              ? GlobalSearchManager.ExecuteQuery<CAPView4UI>(GlobalSearchType.CAP, hiddenElementNames)
                              : null;

                lpList = GlobalSearchUtil.IsLPEnabled()
                             ? GlobalSearchManager.ExecuteQuery<LPView4UI>(GlobalSearchType.LP, null)
                             : null;

                apoList = GlobalSearchUtil.IsAPOEnabled()
                              ? GlobalSearchManager.ExecuteQuery<APOView4UI>(GlobalSearchType.ADDRESS, null)
                              : null;

                queryText = GlobalSearchManager.GetQueryText(GlobalSearchType.CAP);
            }

            // add or update query text
            GlobalSearchManager.AddOrUpdateQueryText(queryText);

            capRecordCount = (capList != null) ? capList.Count : 0;
            lpRecordCount = (lpList != null) ? lpList.Count : 0;
            apoRecordCount = (apoList != null) ? apoList.Count : 0;
            hasResults = capRecordCount > 0 || lpRecordCount > 0 || apoRecordCount > 0;
            bool isNeedRedirect = capRecordCount + lpRecordCount + apoRecordCount == 1;

            //setup condition notice
            SetupConditionNotice(queryText, hasResults);

            //if the search result only has one,then redictet to the detail info
            if (isNeedRedirect)
            {
                bool isNavigate = true;

                if (apoRecordCount == 1)
                {
                    isNavigate = APOView.RedirectToApoDetail(apoList);
                }
                else if (capRecordCount == 1)
                {
                    isNavigate = CapView.RedirectToCapDetail(capList);
                }
                else if (lpRecordCount == 1)
                {
                    isNavigate = LPView.RedirectToLpDetail(lpList);
                }

                if (isNavigate)
                {
                    return;
                }
            }

            //setup CAP list
            SetupCAPList(capList, isLoadingHistory, isNeedRedirect);

            //setup LP list
            SetupLPList(lpList, isLoadingHistory);

            //setup APO list
            SetupAPOList(apoList, isLoadingHistory, toBeShownAPOType);

            //setup navigation
            SetupNavigation(hasResults, capRecordCount, lpRecordCount, apoRecordCount);

            //setup result panel
            pnlLoading.Visible = false;
            pnlNoResults.Visible = !hasResults;
            pnlResults.Visible = hasResults;
        }

        /// <summary>
        /// Determines whether [is new query].
        /// </summary>
        /// <returns>
        /// <c>true</c> if [is new query]; otherwise, <c>false</c>.
        /// </returns>
        private bool IsNewQuery()
        {
            bool result = ValidationUtil.IsYes(Request.QueryString[IS_NEW_QUERY]);

            return result;
        }

        /// <summary>
        /// Determines whether this instance [can load history] the specified query text.
        /// </summary>
        /// <param name="queryText">The query text.</param>
        /// <returns>
        /// <c>true</c> if this instance [can load history] the specified query text; otherwise, <c>false</c>.
        /// </returns>
        private bool CanLoadHistory(string queryText)
        {
            bool result = false;

            GlobalSearchParameter parameter = GlobalSearchManager.GetGlobalSearchParameter(GlobalSearchType.CAP);

            if (parameter != null && (string.IsNullOrEmpty(queryText) || (!string.IsNullOrEmpty(queryText) && queryText.Equals(parameter.QueryText, StringComparison.OrdinalIgnoreCase))))
            {
                result = true;
            }

            return result;
        }

        /// <summary>
        /// Setups the CAP list.
        /// </summary>
        /// <param name="capList">The cap list.</param>
        /// <param name="isLoadingHistory">if set to <c>true</c> [is loading history].</param>
        /// <param name="isNeedRedirect">Boolean value is need direct to detail</param>
        private void SetupCAPList(List<CAPView4UI> capList, bool isLoadingHistory, bool isNeedRedirect)
        {
            if (capList != null && capList.Count > 0)
            {
                pnlCAPList.Visible = true;
                CapView.Display(capList, isLoadingHistory);
            }
            else
            {
                pnlCAPList.Visible = false;
            }
        }

        /// <summary>
        /// Setups the LP list.
        /// </summary>
        /// <param name="lpList">The LP list.</param>
        /// <param name="isLoadingHistory">if set to <c>true</c> [is loading history].</param>
        private void SetupLPList(List<LPView4UI> lpList, bool isLoadingHistory)
        {
            if (lpList != null && lpList.Count > 0)
            {
                pnlLPList.Visible = true;
                LPView.Display(lpList, isLoadingHistory);
            }
            else
            {
                pnlLPList.Visible = false;
            }
        }

        /// <summary>
        /// Setups the APO list.
        /// </summary>
        /// <param name="apoList">The apo list.</param>
        /// <param name="isLoadingHistory">if set to <c>true</c> [is loading history].</param>
        /// <param name="globalSearchType">Type of the global search.</param>
        private void SetupAPOList(List<APOView4UI> apoList, bool isLoadingHistory, GlobalSearchType globalSearchType)
        {
            if (apoList != null && apoList.Count > 0)
            {
                pnlAPOList.Visible = true;
                APOView.Display(apoList, isLoadingHistory, globalSearchType);
            }
            else
            {
                pnlAPOList.Visible = false;
            }
        }

        /// <summary>
        /// Setups the condition notice.
        /// </summary>
        /// <param name="queryText">The query text.</param>
        /// <param name="hasResults">if set to <c>true</c> [has results].</param>
        private void SetupConditionNotice(string queryText, bool hasResults)
        {
            if (hasResults)
            {
                string noticePattern = GetTextByKey("per_globalsearch_label_conditionnotice");
                lblSearchConditionNotice.Text = string.Format(CultureInfo.InvariantCulture, noticePattern, queryText);
            }
            else
            {
                string noticePattern = GetTextByKey("per_globalsearch_label_noresultsnotice");
                lblNoResults.Text = string.Format(CultureInfo.InvariantCulture, noticePattern, queryText);
            }
        }

        /// <summary>
        /// Setups the navigation.
        /// </summary>
        /// <param name="hasResults">if set to <c>true</c> [has results].</param>
        /// <param name="capRecordCount">The cap record count.</param>
        /// <param name="lpRecordCount">The LP record count.</param>
        /// <param name="apoRecordCount">The apo record count.</param>
        private void SetupNavigation(bool hasResults, int capRecordCount, int lpRecordCount, int apoRecordCount)
        {
            if (hasResults)
            {
                tdLinkCAPList.Visible = capRecordCount > 0;
                SetupRecordCount(CapView.CountSummary, capRecordCount, lblCAPNavigation, "per_globalsearch_label_caplink");

                tdLinkLPList.Visible = lpRecordCount > 0;
                SetupRecordCount(LPView.CountSummary, lpRecordCount, lblLPNavigation, "per_globalsearch_label_lplink");

                tdLinkAPOList.Visible = apoRecordCount > 0;
                SetupRecordCount(APOView.CountSummary, apoRecordCount, lblAPONavigation, "per_globalsearch_label_apolink");
            }
        }

        /// <summary>
        /// Setups the record count.
        /// </summary>
        /// <param name="displayCount">The display count.</param>
        /// <param name="recordCount">The record count.</param>
        /// <param name="theNavigation">The navigation control.</param>
        /// <param name="labelKey">The label key.</param>
        private void SetupRecordCount(string displayCount, int recordCount, AccelaLabel theNavigation, string labelKey)
        {
            string countLabel = string.Empty;

            if (recordCount < 0)
            {
                countLabel = GetTextByKey("capdetail_message_loading");
            }
            else
            {
                countLabel = string.Format(GetTextByKey(labelKey), displayCount);
            }

            theNavigation.Text = countLabel;
            upResultsHeader.Update();
        }
    }
}