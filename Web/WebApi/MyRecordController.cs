/**
* <pre>
* 
*  Accela Citizen Access
*  File: MyRecordController.cs
* 
*  Accela, Inc.
*  Copyright (C): 2014
* 
*  Description:
* 
*  Notes:
* $Id: MyRecordController.cs 72643 2014-06-19 09:52:06Z $.
*  Revision History
*  &lt;Date&gt;,    &lt;Who&gt;,    &lt;What&gt;
*  06/24/2014 Awen Initial.  
* </pre>
*/

using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Web;
using System.Web.Caching;
using System.Web.Http;
using Accela.ACA.BLL;
using Accela.ACA.BLL.AddressBuilder;
using Accela.ACA.BLL.Cap;
using Accela.ACA.BLL.Common;
using Accela.ACA.Common;
using Accela.ACA.Common.Common;
using Accela.ACA.Common.Util;
using Accela.ACA.Web.Common;
using Accela.ACA.Web.Common.Control;
using Accela.ACA.Web.WebApi.Entity.Adapter;
using Accela.ACA.Web.WebApi.Util;
using Accela.ACA.WSProxy;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Accela.ACA.Web.WebApi
{
    /// <summary>
    /// Web API for my record in dashboard.
    /// </summary>
    public class MyRecordController : ApiController
    {
        /// <summary>
        /// Sort list.
        /// </summary>
        private const string SortByList = "PermitNumber,PermitType,Status,EnglishTradeName,auditDate";

        #region public methods

        /// <summary>
        /// Get Paged My Records
        /// </summary>
        /// <param name="pageIndex">page Index</param>
        /// <param name="pageSize">page Size</param>
        /// <param name="module">module name</param>
        /// <param name="sortBy">sort By</param>
        /// <param name="isAsc">is ASC</param>
        /// <param name="isInitial">is Initial</param>
        /// <returns>record list</returns>
        [ActionName("getPagedRecord")]
        public HttpResponseMessage GetPagedMyRecords(int pageIndex, int pageSize, string module, string sortBy, bool isAsc, bool isInitial)
        {
            string records = string.Empty;
            int pageCount;
            string desc;
            string modules;
            DataRow[] dataRows = GetPagedMyRecordsByModule(module, out pageCount, out desc, out modules, pageIndex, pageSize, sortBy, isAsc, isInitial);

            if (dataRows == null)
            {
                return new HttpResponseMessage { Content = new StringContent("0") };
            }

            DataTable dt = CreateTable();
            foreach (var dataRow in dataRows)
            {
                dt.Rows.Add(dataRow.ItemArray);
            }

            records = GetMyRecordsListToJson(dt);
            bool isEnableExport2CSV = StandardChoiceUtil.IsEnableExport2CSV();
            return new HttpResponseMessage
            {
                Content = new StringContent("{\"enableExport\":" + isEnableExport2CSV.ToString().ToLower() + ",\"Records\":" + records + ", \"pageCount\":\"" + pageCount + "\",\"Modules\":[" + modules + "], " + desc + "}")
            };
        }

        /// <summary>
        ///  MyRecord  download 
        /// </summary>
        /// <param name="type"> download  Type</param>
        /// <returns>globalSearch Type Data</returns>
        [ActionName("myRecord-download")]
        public HttpResponseMessage GetGlobalSearchDownload(string type)
        {
            DataRow[] dataRows = GetMyRecordsFromCache() ?? new DataRow[0];
            DataTable dt = CreateTable();
            foreach (var dataRow in dataRows)
            {
                if ((dataRow["ModuleName"] + string.Empty) == type)
                {
                    dt.Rows.Add(dataRow.ItemArray);
                }
            }

            string records = GetMyRecordsListToJson(dt, true);

            return new HttpResponseMessage
            {
                Content = new StringContent("{\"Records\":" + records + "}")
            };
        }

        #endregion

        /// <summary>
        /// get Action Recent Json
        /// </summary>
        /// <param name="dt">data table</param>
        /// <param name="isdownload">download or not</param>
        /// <returns>record list</returns>
        private static string GetMyRecordsListToJson(DataTable dt, bool isdownload = false)
        {
            if (dt == null || dt.Rows.Count < 1)
            {
                return "{\"RecordsList\": []}";
            }

            string result = string.Empty;
            StringBuilder records = new StringBuilder();
            records.Append("\"RecordsList\": [");

            //whether or not shown Add to cart button
            bool isShowAddToCart = !AppSession.User.IsAnonymous && StandardChoiceUtil.IsEnableShoppingCart();

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                string permitNumber = dt.Rows[i]["permitNumber"] != null
                    ? dt.Rows[i]["permitNumber"].ToString()
                    : string.Empty;
                string expirationDate = dt.Rows[i]["expirationDate"] != null &&
                                        dt.Rows[i]["expirationDate"].ToString() != string.Empty
                    ? I18nDateTimeUtil.FormatToDateStringForUI(dt.Rows[i]["expirationDate"].ToString())
                    : string.Empty;
                string createdBy = dt.Rows[i]["CreatedBy"] != null ? dt.Rows[i]["CreatedBy"].ToString() : string.Empty;
                string capIndex = dt.Rows[i]["CapIndex"] != null ? dt.Rows[i]["CapIndex"].ToString() : string.Empty;
                string auditDate = dt.Rows[i]["auditDate"] != null && dt.Rows[i]["auditDate"].ToString() != string.Empty
                    ? I18nDateTimeUtil.FormatToDateStringForUI(dt.Rows[i]["auditDate"].ToString())
                    : string.Empty;
                int relatedRecords = int.Parse(dt.Rows[i]["RelatedRecords"] + string.Empty);
                string filterName = dt.Rows[i]["filterName"].ToString();
                string moduleName = dt.Rows[i]["ModuleName"].ToString();
                string renewalStatus = dt.Rows[i]["RenewalStatus"].ToString();
                string status = dt.Rows[i]["Status"] != null ? dt.Rows[i]["Status"].ToString() : string.Empty;
                bool hasPrivilegeToHandleCap = (bool)dt.Rows[i]["hasPrivilegeToHandleCap"];

                string createDate = dt.Rows[i]["auditDate"] != null &&
                                    dt.Rows[i]["auditDate"].ToString() != string.Empty
                    ? I18nDateTimeUtil.FormatToDateStringForUI(dt.Rows[i]["auditDate"].ToString())
                    : string.Empty;

                CapIDModel4WS capIDModel = new CapIDModel4WS
                {
                    id1 = dt.Rows[i]["capID1"].ToString(),
                    id2 = dt.Rows[i]["capID2"].ToString(),
                    id3 = dt.Rows[i]["capID3"].ToString(),
                    serviceProviderCode = dt.Rows[i]["AgencyCode"].ToString()
                };

                CapModel4WS capModel4WS = new CapModel4WS();
                capModel4WS.capID = capIDModel;
                capModel4WS.moduleName = moduleName;

                bool hasNopaidFee = (bool)dt.Rows[i]["HasNoPaidFees"];
                string permitType = dt.Rows[i]["PermitType"].ToString();
                string address = dt.Rows[i]["Address"].ToString();

                string agencyStateZip = DataUtil.ConcatStringWithSplitChar(
                    new[]
                    {
                        dt.Rows[i]["agencyCode"].ToString(), AppSession.User.State, AppSession.User.Zip
                    },
                    ACAConstant.COMMA_BLANK);
                string englishTradeName = dt.Rows[i]["englishTradeName"].ToString();

                string detailViewUrl = ApiUtil.ConstructRecordDetailUrl(
                    moduleName,
                    capIDModel.id1,
                    capIDModel.id2,
                    capIDModel.id3,
                    capIDModel.serviceProviderCode);

                string capID = capIDModel.id1 + ACAConstant.SPLIT_CHAR4 + capIDModel.id2 + ACAConstant.SPLIT_CHAR4 +
                               capIDModel.id3;
                string agencyCode = dt.Rows[i]["agencyCode"].ToString();
                bool displayInspcetion = ActionButtonController.DisplayInspcetion(capIDModel, moduleName);
                string capClass = dt.Rows[i]["CapClass"].ToString();

                // check whether the cap can be used for pay fee
                bool canPayFeeDue = false;
                bool canPayFeeDue4Renew = false;

                MyRecordsCapView4Ui myRecordsCapView4Ui = new MyRecordsCapView4Ui();
                myRecordsCapView4Ui.ModuleName = moduleName;
                myRecordsCapView4Ui.RelatedRecords = relatedRecords;
                myRecordsCapView4Ui.Status = status;
                myRecordsCapView4Ui.PermitNumber = permitNumber;
                myRecordsCapView4Ui.PermitType = permitType;
                myRecordsCapView4Ui.Address = address;
                myRecordsCapView4Ui.AgencyStateZip = agencyStateZip;
                myRecordsCapView4Ui.EnglishTradeName = englishTradeName;
                myRecordsCapView4Ui.CapIndex = capIndex;
                myRecordsCapView4Ui.DisplayInspcetion = displayInspcetion;
                myRecordsCapView4Ui.CapID = capID;
                myRecordsCapView4Ui.HasNoPaidFees = hasNopaidFee;
                myRecordsCapView4Ui.AgencyCode = agencyCode;
                myRecordsCapView4Ui.CapClass = capClass;
                myRecordsCapView4Ui.RenewalStatus = renewalStatus;
                myRecordsCapView4Ui.DetailViewUrl = detailViewUrl;
                myRecordsCapView4Ui.ExpirationDate = expirationDate;
                myRecordsCapView4Ui.CreatedBy = createdBy;
                myRecordsCapView4Ui.AuditDate = auditDate;
                myRecordsCapView4Ui.CreatedDate = I18nDateTimeUtil.ParseFromWebService(createDate);
                myRecordsCapView4Ui.Inspections = ApiUtil.GetInspectionCount(capModel4WS);
                myRecordsCapView4Ui.IsShowAddToCart = isShowAddToCart;
                
                if (!isdownload)
                {
                    bool isPartialCap = CapUtil.IsPartialCap(capClass);

                    if (!isPartialCap && !StandardChoiceUtil.IsRemovePayFee(ConfigManager.AgencyCode))
                    {
                        if (hasNopaidFee && FunctionTable.IsEnableMakePayment())
                        {
                            canPayFeeDue = true;
                        }
                        else if (FunctionTable.IsEnableRenewRecord() &&
                                 ACAConstant.PAYFEEDUE_RENEWAL.Equals(renewalStatus, StringComparison.InvariantCulture))
                        {
                            canPayFeeDue4Renew = true;
                        }
                    }

                    string payfeesUrl = string.Empty;
                    if (canPayFeeDue)
                    {
                        payfeesUrl = ApiUtil.ConstructPayFeesDeepLink(
                            moduleName,
                            capIDModel.id1,
                            capIDModel.id2,
                            capIDModel.id3,
                            capIDModel.serviceProviderCode);
                    }
                    else if (canPayFeeDue4Renew)
                    {
                        payfeesUrl = ApiUtil.ConstructPayFeeDue4RenewDeepLink(
                            moduleName,
                            capIDModel.id1,
                            capIDModel.id2,
                            capIDModel.id3,
                            capIDModel.serviceProviderCode);
                    }

                    //resume link
                    string resumeUrl = string.Empty;

                    if (hasPrivilegeToHandleCap && isPartialCap)
                    {
                        resumeUrl = ApiUtil.ConstructResumeDeepLink(
                            moduleName,
                            capIDModel.id1,
                            capIDModel.id2,
                            capIDModel.id3,
                            capIDModel.serviceProviderCode,
                            capClass,
                            ApiUtil.GetFilterNameForResume((CapTypeModel)dt.Rows[i]["CapTypeModel"], moduleName));
                    }

                    // Display\hide clone record button
                    bool isShowCopyRecord = false;

                    if (CloneRecordUtil.IsDisplayCloneButton((CapTypeModel)dt.Rows[i]["CapTypeModel"], TempModelConvert.Trim4WSOfCapIDModel(capIDModel), moduleName, true) && FunctionTable.IsEnableCloneRecord())
                    {
                        isShowCopyRecord = true;
                    }

                    // to show ReportButton or not.            
                    CustomCapView4Ui customCapView4Ui = NewUiReportUtil.DisplayReportButton(capIDModel, moduleName);
                    myRecordsCapView4Ui.PayfeesUrl = payfeesUrl;
                    myRecordsCapView4Ui.ResumeUrl = resumeUrl;
                    myRecordsCapView4Ui.IsShowCopyRecord = isShowCopyRecord;
                    myRecordsCapView4Ui.IsShowUpload = !isPartialCap && ApiUtil.IsShowUploadButtonInDetailPage(moduleName, capIDModel);

                    //PrintPermitReport button
                    myRecordsCapView4Ui.IsShowPrintPermit = customCapView4Ui.IsShowPrintPermit;
                    myRecordsCapView4Ui.PrintPermitReportId = customCapView4Ui.PrintPermitReportId;
                    myRecordsCapView4Ui.PrintPermitReportName = customCapView4Ui.PrintPermitReportName;
                    myRecordsCapView4Ui.PrintPermitReportType = customCapView4Ui.PrintPermitReportType;

                    //PrintSummaryReport button
                    myRecordsCapView4Ui.IsShowPrintSummary = customCapView4Ui.IsShowPrintSummary;
                    myRecordsCapView4Ui.PrintSummaryReportId = customCapView4Ui.PrintSummaryReportId;
                    myRecordsCapView4Ui.PrintSummaryReportName = customCapView4Ui.PrintSummaryReportName;
                    myRecordsCapView4Ui.PrintSummaryReportType = customCapView4Ui.PrintSummaryReportType;

                    //PrintReceiptReport button
                    myRecordsCapView4Ui.IsShowPrintReceipt = customCapView4Ui.IsShowPrintReceipt;
                    myRecordsCapView4Ui.PrintReceiptReportId = customCapView4Ui.PrintReceiptReportId;
                    myRecordsCapView4Ui.PrintReceiptReportName = customCapView4Ui.PrintReceiptReportName;
                    myRecordsCapView4Ui.PrintReceiptReportType = customCapView4Ui.PrintReceiptReportType;
                }

                IsoDateTimeConverter timeConverter = new IsoDateTimeConverter
                {
                    DateTimeFormat = I18nDateTimeUtil.ShortDatePattern
                };
                string myRecordsJson = JsonConvert.SerializeObject(myRecordsCapView4Ui, Formatting.Indented, timeConverter);

                //CapIndex is a test data  ModuleName
                records.Append(myRecordsJson + ",");
            }

            if (records.Length > 1)
            {
                records.Length -= 1;
            }

            records.Append("]");

            result = "{" + records + "}";

            return result;
        }

        /// <summary>
        /// create blank structure for cap list
        /// </summary>
        /// <returns>blank table for cap list</returns>
        private static DataTable CreateTable()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add(new DataColumn("CapIndex", typeof(string)));
            dt.Columns.Add(new DataColumn("Date", typeof(DateTime)));
            dt.Columns.Add(new DataColumn("PermitNumber", typeof(string)));
            dt.Columns.Add(new DataColumn("PermitType", typeof(string)));
            dt.Columns.Add(new DataColumn("Description", typeof(string)));
            dt.Columns.Add(new DataColumn("Status", typeof(string)));
            dt.Columns.Add(new DataColumn("CapClass", typeof(string)));
            dt.Columns.Add(new DataColumn("capID1", typeof(string)));
            dt.Columns.Add(new DataColumn("capID2", typeof(string)));
            dt.Columns.Add(new DataColumn("capID3", typeof(string)));
            dt.Columns.Add(new DataColumn("ProjectName", typeof(string)));
            dt.Columns.Add(new DataColumn("HasNoPaidFees", typeof(bool)));
            dt.Columns.Add(new DataColumn("RenewalStatus", typeof(string)));
            dt.Columns.Add(new DataColumn("expirationDate", typeof(DateTime)));
            dt.Columns.Add(new DataColumn("PermitAddress", typeof(string)));
            dt.Columns.Add(new DataColumn("RenewalSort", typeof(int)));
            dt.Columns.Add(new DataColumn("hasPrivilegeToHandleCap", typeof(bool)));
            dt.Columns.Add(new DataColumn("agencyCode", typeof(string)));
            dt.Columns.Add(new DataColumn("EnglishTradeName", typeof(string)));
            dt.Columns.Add(new DataColumn("ArabicTradeName", typeof(string)));
            dt.Columns.Add(new DataColumn("relatedTradeLicense", typeof(string)));
            dt.Columns.Add(new DataColumn("filterName", typeof(string)));
            dt.Columns.Add(new DataColumn("isAmendable", typeof(string)));
            dt.Columns.Add(new DataColumn("licenseType", typeof(string)));
            dt.Columns.Add(new DataColumn("isTNExpired", typeof(bool)));
            dt.Columns.Add(new DataColumn("ModuleName", typeof(string)));
            dt.Columns.Add(new DataColumn("Address", typeof(string)));
            dt.Columns.Add(new DataColumn("CreatedBy", typeof(string)));
            dt.Columns.Add(new DataColumn("RelatedRecords", typeof(int)));
            dt.Columns.Add(new DataColumn("AppStatusGroup", typeof(string)));
            dt.Columns.Add(new DataColumn("AppStatus", typeof(string)));
            dt.Columns.Add(new DataColumn("PaymentStatus", typeof(string)));
            dt.Columns.Add(new DataColumn("auditDate", typeof(DateTime)));
            dt.Columns.Add(new DataColumn("CapTypeModel", typeof(CapTypeModel)));

            return dt;
        }

        /// <summary>
        /// get address for display in permit list
        /// </summary>
        /// <param name="capMode">cap model for aca.</param>
        /// <returns>string for permit address.</returns>
        private static string GetPermitAddress(SimpleCapModel capMode)
        {
            string result = string.Empty;

            if (capMode != null && capMode.addressModel != null)
            {
                IAddressBuilderBll addressBuilderBll = ObjectFactory.GetObject<IAddressBuilderBll>();
                result = addressBuilderBll.BuildAddressByFormatType(capMode.addressModel, null, AddressFormatType.SHORT_ADDRESS_NO_FORMAT);
            }

            return result;
        }

        /// <summary>
        /// Build Tab Item
        /// </summary>
        /// <param name="tab">the instance of TabItem</param>
        /// <param name="setLabel">is set label</param>
        private static void BuildTabItem(TabItem tab, bool setLabel)
        {
            string label = LabelUtil.GetSuperAgencyTextByKey(tab.Label, tab.Module);

            if (label == LabelUtil.GetGlobalTextByKey("aca_sys_default_module_name"))
            {
                label = DataUtil.AddBlankToString(tab.Module);
            }

            tab.Title = LabelUtil.RemoveHtmlFormat(label);

            if (setLabel)
            {
                tab.Label = label;
            }
        }

        /// <summary>
        /// Sets the label status text.
        /// </summary>
        /// <param name="capStatus">The cap status.</param>
        /// <param name="renewalStatus">The renewal status.</param>
        /// <returns>string status</returns>
        private static string SetStatusText(string capStatus, string renewalStatus)
        {
            // get the label key accoding the renewal status
            string status = capStatus;

            if (ACAConstant.RENEWAL_REVIEW.Equals(renewalStatus, StringComparison.InvariantCultureIgnoreCase))
            {
                status = LabelUtil.GetGlobalTextByKey("per_permitList_label_renewalReviewing");
            }
            else if (ACAConstant.DEFERPAY_RENEWAL.Equals(renewalStatus, StringComparison.InvariantCultureIgnoreCase))
            {
                status = LabelUtil.GetGlobalTextByKey("per_permitList_label_renewal_deferpay");
            }

            return status;
        }

        /// <summary>
        /// Get My Records By Module
        /// </summary>
        /// <param name="module">module name</param>
        /// <param name="pageCount">page Count</param>
        /// <param name="desc">description parameter</param>
        /// <param name="modules">modules list</param>
        /// <param name="pageIndex">page Index</param>
        /// <param name="pageSize">page Size</param>
        /// <param name="sortBy">sort By</param>
        /// <param name="isAsc">is ASC</param>
        /// <param name="isInitial">is Initial</param>
        /// <returns>Get MyRecords</returns>
        [NonAction]
        private DataRow[] GetPagedMyRecordsByModule(string module, out int pageCount, out string desc, out string modules, int pageIndex, int pageSize = ACAConstant.DEFAULT_PAGESIZE, string sortBy = "", bool isAsc = true, bool isInitial = false)
        {
            DataRow[] cacheRows = GetMyRecordsFromCache() ?? new DataRow[] { };
            int maxIndex = (pageIndex + 1) * pageSize;

            PaginationModel pageInfo = PaginationUtil.GetPageInfoByID("MyRecordListForNewUI");

            if (isInitial || pageIndex > pageInfo.EndPage)
            {
                DataTable dataTable = null;
                pageInfo.SortExpression = null;
                pageInfo.CurrentPageIndex = pageIndex;
                pageInfo.CustomPageSize = pageSize;
                pageInfo.StartPage = pageInfo.CurrentPageIndex;
                pageInfo.EndPage = pageInfo.StartPage + ACAConstant.DEFAULT_PAGECOUNT - 1;

                SearchResultModel searchResult = SearchMyRecordsFromDb(module, pageInfo);
                dataTable = CreateDataSource(searchResult.resultList);

                if (dataTable != null)
                {
                    cacheRows = dataTable.Select();
                    AddMyRecordsToCache(cacheRows, isInitial);
                }
            }

            // get Cache
            cacheRows = GetMyRecordsFromCache();

            modules = string.Empty;
            foreach (DataRow cacheRow in cacheRows)
            {
                if (modules.IndexOf(cacheRow["ModuleName"].ToString(), StringComparison.InvariantCultureIgnoreCase) == -1)
                {
                    modules += "{" + "\"name\":\"" + cacheRow["ModuleName"].ToString() + "\"},";
                }
            }

            modules = modules.Length > 0 ? modules.Remove(modules.Length - 1) : modules;

            if (!string.IsNullOrEmpty(module))
            {
                cacheRows = cacheRows.Where(row => row["ModuleName"].ToString() == module).ToArray();
            }

            if (!string.IsNullOrEmpty(sortBy) && SortByList.IndexOf(sortBy, StringComparison.InvariantCultureIgnoreCase) > -1)
            {
                cacheRows = isAsc ? cacheRows.OrderBy(row => row["ModuleName"]).ThenBy(row => row[sortBy]).ToArray() : cacheRows.OrderByDescending(row => row["ModuleName"]).ThenByDescending(row => row[sortBy]).ToArray();
            }

            List<DataRow> resultData = new List<DataRow>();
            Dictionary<string, int> recordDictionary = new Dictionary<string, int>();
            for (int i = 0; i < cacheRows.Length; i++)
            {
                string moduleName = cacheRows[i]["ModuleName"] + string.Empty;

                if (recordDictionary.ContainsKey(moduleName))
                {
                    recordDictionary[moduleName] = recordDictionary[moduleName] + 1;
                }
                else
                {
                    recordDictionary.Add(moduleName, 1);
                }

                if (i >= maxIndex - pageSize && i < maxIndex)
                {
                    resultData.Add(cacheRows[i]);
                }
            }

            desc = string.Empty;

            foreach (string key in recordDictionary.Keys)
            {
                desc = desc + (recordDictionary[key] + " " + key + ", ");
            }

            desc = desc.Length > 1 ? desc.Remove(desc.Length - 2) : desc;
            desc = "\"totalCount\":\"" + cacheRows.Length + "\",\"descriptions\":\"" + desc + "\"";

            pageCount = (cacheRows.Length / pageSize) + ((cacheRows.Length % pageSize) > 0 ? 1 : 0);

            return resultData.ToArray();
        }

        /// <summary>
        /// Search My Records From data base
        /// </summary>
        /// <param name="module">module name</param>
        /// <param name="customPageInfo">Custom Query  Default Value is Null</param>
        /// <returns>Search Result Model</returns>
        [NonAction]
        private SearchResultModel SearchMyRecordsFromDb(string module, PaginationModel customPageInfo)
        {
            CapModel4WS capModel = new CapModel4WS();
            List<string> modulesList = new List<string>();

            if (string.IsNullOrEmpty(module))
            {
                IList<TabItem> tabsList = GetTabsList();

                if (tabsList != null && tabsList.Count > 0)
                {
                    foreach (var tabItem in tabsList)
                    {
                        if (!"APO".Equals(tabItem.Key, StringComparison.InvariantCultureIgnoreCase))
                        {
                            modulesList.Add(tabItem.Module);
                        }
                    }

                    if (modulesList.Count > 0)
                    {
                        capModel.moduleName = modulesList[0];
                        modulesList.RemoveAt(0);
                    }
                }
            }
            else
            {
                capModel.moduleName = module;
            }

            QueryFormat queryFormat = CreateQueryFormat(customPageInfo, int.Parse(GviewID.PermitList), 0);
            ICapBll capBll = ObjectFactory.GetObject<ICapBll>();

            SearchResultModel capResult = capBll.GetCapList4ACA(
                capModel, 
                queryFormat, 
                AppSession.User.UserSeqNum, 
                null, 
                true,
                modulesList,
                false, 
                null);

            return capResult;
        }
        
        /// <summary>
        /// Create Query Format
        /// </summary>
        /// <param name="pageInfo">page information</param>
        /// <param name="viewId">view ID</param>
        /// <param name="dataFilterId">data filter ID</param>
        /// <returns>Query format model</returns>
        [NonAction]
        private QueryFormat CreateQueryFormat(PaginationModel pageInfo, int viewId, long dataFilterId)
        {
            QueryFormat queryFormat = new QueryFormat();

            if (pageInfo.CurrentPageIndex.Equals(0))
            {
                pageInfo.StartDBRow = 0;
                pageInfo.IsSearchAllStartRow = false;
            }

            int startRow = pageInfo.StartDBRow > 0 ? pageInfo.StartDBRow : (pageInfo.CurrentPageIndex * pageInfo.CustomPageSize) + 1;
            int endRow = startRow + (pageInfo.CustomPageSize * ACAConstant.DEFAULT_PAGECOUNT) - 1;
            endRow += ACAConstant.ADDITIONAL_RECORDS_COUNT;

            queryFormat.startRow = startRow;
            queryFormat.endRow = endRow;
            IDataFilterBll dataFilterBll = ObjectFactory.GetObject<IDataFilterBll>();
            XDataFilterElementModel[] dataFilterElementModels = dataFilterBll.GetXDataFilterElementByDataFilterId(ConfigManager.AgencyCode, viewId, dataFilterId, AppSession.User.UserSeqNum);
            queryFormat.quickQuery = dataFilterElementModels;
            return queryFormat;
        }

        /// <summary>
        /// create data table by given cap list
        /// </summary>
        /// <param name="capList">Cap model list</param>
        /// <returns>data source for UI</returns>
        private DataTable CreateDataSource(object[] capList)
        {
            DataTable dt = CreateTable();

            if (capList == null || capList.Length == 0)
            {
                return dt;
            }

            int index = 0;

            foreach (object obj in capList)
            {
                SimpleCapModel cap = obj as SimpleCapModel;

                if (cap == null)
                {
                    continue;
                }

                string permitNumber = cap.altID != null ? cap.altID : string.Empty;
                string permitType = CAPHelper.GetAliasOrCapTypeLabel(cap);
                string status = I18nStringUtil.GetString(cap.resCapStatus, cap.capStatus);
                string capClass = cap.capClass != null ? cap.capClass : string.Empty;
                bool hasNoPaidFees = cap.noPaidFeeFlag;
                string renewalStatus = cap.renewalStatus != null ? cap.renewalStatus : string.Empty;
                string description = string.Empty;

                if (cap.capDetailModel != null)
                {
                    description = cap.capDetailModel.shortNotes != null ? cap.capDetailModel.shortNotes : string.Empty;
                }

                DataRow dr = dt.NewRow();
                dr["CapIndex"] = index;
                dr["PermitNumber"] = permitNumber;
                dr["PermitType"] = permitType;
                dr["Description"] = description;
                dr["Status"] = SetStatusText(status, renewalStatus);
                dr["CapClass"] = capClass;
                dr["capID1"] = cap.capID.ID1;
                dr["capID2"] = cap.capID.ID2;
                dr["capID3"] = cap.capID.ID3;
                dr["ProjectName"] = cap.specialText;
                dr["HasNoPaidFees"] = hasNoPaidFees;
                dr["RenewalStatus"] = renewalStatus;
                dr["hasPrivilegeToHandleCap"] = cap.hasPrivilegeToHandleCap;
                dr["EnglishTradeName"] = cap.englishTradeName;
                dr["ArabicTradeName"] = cap.arabicTradeName;
                dr["relatedTradeLicense"] = cap.relatedTradeLic;
                dr["filterName"] = cap.capType.filterName;
                dr["isAmendable"] = cap.capType.isAmendable;
                dr["licenseType"] = cap.licenseType;
                dr["isTNExpired"] = cap.isTNExpired;
                dr["ModuleName"] = cap.moduleName;
                dr["Address"] = GetPermitAddress(cap);
                dr["PaymentStatus"] = cap.paymentStatus;
                dr["auditDate"] = cap.auditDate;

                if (cap.capID != null && !string.IsNullOrEmpty(cap.capID.serviceProviderCode))
                {
                    dr["agencyCode"] = cap.capID.serviceProviderCode;
                }
                else
                {
                    dr["agencyCode"] = ConfigManager.AgencyCode;
                }

                dr["Date"] = cap.fileDate == null ? DBNull.Value : (object)cap.fileDate;
                dr["expirationDate"] = cap.expDate == null ? DBNull.Value : (object)cap.expDate;
                dr["RenewalSort"] = renewalStatus == ACAConstant.RENEWAL_INCOMPLETE ? 1 : 0;
                dr["CreatedBy"] = cap.createdByDisplay;
                dr["RelatedRecords"] = cap.relatedRecordsCount;
                dr["AppStatusGroup"] = cap.statusGroupCode;
                dr["AppStatus"] = cap.capStatus;
                dr["CapTypeModel"] = cap.capType;

                dt.Rows.Add(dr);
                index++;
            }

            return dt;
        }
        
        /// <summary>
        /// Add object into cache
        /// </summary>
        /// <param name="dateRows">data row</param>
        /// <param name="isInitAdd">is initial add</param>
        [NonAction]
        private void AddMyRecordsToCache(DataRow[] dateRows, bool isInitAdd = false)
        {
            if (dateRows == null)
            {
                return;
            }

            string publicUserIdString = AppSession.User.PublicUserId + "_" + "SearchMyRecords";
            List<DataRow> dataRowsList = new List<DataRow>();
            DataTable dt = CreateTable();

            if (!isInitAdd)
            {
                DataRow[] temRows = GetMyRecordsFromCache();

                if (temRows != null)
                {
                    dataRowsList.AddRange(temRows);
                }
            }

            dataRowsList.AddRange(dateRows);

            //Sort
            foreach (var dataRow in dataRowsList)
            {
                dt.Rows.Add(dataRow.ItemArray);
            }

            DataView dv = dt.DefaultView;
            dv.Sort = "ModuleName,auditDate desc";
            dt = dv.ToTable();

            dataRowsList = new List<DataRow>();
            foreach (DataRow itemRow in dt.Rows)
            {
                dataRowsList.Add(itemRow);
            }

            Cache cahe = HttpContext.Current.Cache ?? new Cache();
            cahe.Insert(publicUserIdString, dataRowsList.ToArray(), null, DateTime.Now.AddHours(2), TimeSpan.Zero);
        }

        /// <summary>
        /// Get object from cache
        /// </summary>
        /// <returns>data row</returns>
        [NonAction]
        private DataRow[] GetMyRecordsFromCache()
        {
            string publicUserIdString = AppSession.User.PublicUserId + "_" + "SearchMyRecords";
            DataRow[] rows = HttpContext.Current.Cache[publicUserIdString] as DataRow[];

            return rows;
        }

        /// <summary>
        /// Gets all of link blocks need to displayed to the current page.
        /// </summary>
        /// <returns>all link blocks.</returns>
        [NonAction]
        private IList<TabItem> GetTabsList()
        {
            bool isAdminModeRegistered = false;

            // get all defined tabs and links.
            IList<TabItem> tabsList = TabUtil.GetTabList(isAdminModeRegistered);
            IList<TabItem> linkTabs = new List<TabItem>();

            foreach (TabItem tab in tabsList)
            {
                // if the tab needn't to be showed in home page as link block
                if (!tab.BlockVisible || string.IsNullOrEmpty(tab.Label))
                {
                    continue;
                }

                BuildTabItem(tab, false);

                // found the block links in home page.
                if (tab.Children.Count > 0)
                {
                    ICapTypeFilterBll capTypFiltereBll = (ICapTypeFilterBll)ObjectFactory.GetObject(typeof(ICapTypeFilterBll));
                    List<LinkItem> listItemList = new List<LinkItem>();

                    foreach (LinkItem subLink in tab.Children)
                    {
                        string filterName = capTypFiltereBll.GetCapTypeFilterByLabelKey(ConfigManager.AgencyCode, subLink.Module, subLink.Label);

                        // append tab name to url to ensure tab can be selected correctly.
                        subLink.Url = TabUtil.RebuildUrl(subLink.Url, tab.Key, filterName);
                        listItemList.Add(subLink);
                    }

                    tab.Children = listItemList;
                    linkTabs.Add(tab);
                }
            }

            // Report block is configurated in standard choice,which is diffrent with other block.
            TabItem reportBlock = GetReportLink();

            if (reportBlock != null)
            {
                linkTabs.Add(reportBlock);
            }

            return linkTabs;
        }

        /// <summary>
        /// Gets report link block.
        /// </summary>
        /// <returns>TabLinkItem object.</returns>
        [NonAction]
        private TabItem GetReportLink()
        {
            TabItem reportTab = null;
            IBizDomainBll bizBll = (IBizDomainBll)ObjectFactory.GetObject(typeof(IBizDomainBll));

            // get the report name from standard choice.
            string reportName = bizBll.GetValueForACAConfig(ConfigManager.AgencyCode, BizDomainConstant.STD_ITEM_PRINT_REPORT_NAME);

            // if the report name is configruated, add the link.otherwise no report link block.
            if (!string.IsNullOrEmpty(reportName))
            {
                //Home Page Report URL
                string homePageReportUrl = string.Format("/Report/ShowReport.aspx?reportType={0}&reportName={1}&reportID={2}", ACAConstant.PRINT_HOMEPAGE_REPORT, reportName, ACAConstant.NONASSIGN_NUMBER);
                LinkItem linkItem = new LinkItem();
                linkItem.Label = "com_welcome_label_print";
                linkItem.Url = homePageReportUrl;

                reportTab = new TabItem();
                reportTab.Label = "com_welcome_label_print_title";
                reportTab.Url = homePageReportUrl;

                // set enough big order to ensure it is the last tab.
                reportTab.Order = 999; 
                reportTab.Children.Add(linkItem);
            }

            return reportTab;
        }
    }
}