/**
* <pre>
* 
*  Accela Citizen Access
*  File: GlobalSearch.List.aspx.cs
* 
*  Accela, Inc.
*  Copyright (C): 2007-2013
* 
*  Description:
*  View Global Search query results details.
* 
*  Notes:
*      $Id: GlobalSearch.List.aspx.cs 271347 2014-05-12 08:57:40Z ACHIEVO\james.shi $.
*  Revision History
*  &lt;Date&gt;,		&lt;Who&gt;,			&lt;What&gt;
*  10-05-2009           Dave Brewster           New page added for version 7.0
* </pre>
*/
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Text;
using System.Web;
using Accela.ACA.BLL.Common;
using Accela.ACA.Common;
using Accela.ACA.Common.Util;
using Accela.ACA.Web.Common;
using Accela.ACA.Web.Common.GlobalSearch;
using Accela.ACA.WSProxy;

/// <summary>
/// 
/// </summary>
public partial class GlobalSearchList : AccelaPage
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
    public StringBuilder PagingFooter = new StringBuilder();
    public StringBuilder PagingFields = new StringBuilder();
    public StringBuilder ResultHeader = new StringBuilder();
    public StringBuilder ResultOutput = new StringBuilder();
    public StringBuilder SortOptions = new StringBuilder();
    public StringBuilder ParcelDetail = new StringBuilder();
    public StringBuilder ErrorMessage = new StringBuilder();
    public StringBuilder Breadcrumbs = new StringBuilder();
    public StringBuilder CollectionLinks = new StringBuilder();
    private StringBuilder sbWork = new StringBuilder();
    public StringBuilder HiddenFields = new StringBuilder();
    public string ListSortColumn = string.Empty;
    public string ListSortDesc = string.Empty;

    public string capsInList = string.Empty;
    public string SearchMode = string.Empty;
    public string Filter = string.Empty;
    public string DisplayMode = string.Empty;
    public string CollectionModuleName = string.Empty;
    public string BackForwardLinks = string.Empty;
    public string ListMode = string.Empty;
    private string SearchDescripton = string.Empty;
    private int CurrentResultPageNumber = 1;
    public string PageResultPageNo = string.Empty;
    private string sortOrder = string.Empty;
    private bool sortByDate = false;
    private bool sortByNumber = false;
    private bool sortByAddress = false;
    private bool sortByAgency = false;
    private bool sortByModule = false;
    private bool sortByStatus = false;
    private bool sortByType = false;
    private bool sortByName = false;
    private bool sortByBizName = false;
    private bool isSortOrderChange = false;
    private string gsSortColumn = string.Empty;
    private string gsSortDirection = string.Empty;

    
    private bool sortDescending = false;
    public string PageBreadcrumbIndex = string.Empty;
    private bool isBreadcrumbPagingMode = false;
    private bool isBreadcrumbRefeshMode = false;
    private bool licenseVisible = true;
    private bool licenseNameVisible = true;
    private bool licenseTypeVisible = true;
    private bool businessNameVisible = true;
    private bool addressVisible = true;
    private bool parcelVisible = true;
    private bool ownerVisisble = true;


    /// <summary>
    /// Advanced Search Options
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Load(object sender, EventArgs e)
    {
        ValidationChecks("GlobalSearch.List.aspx");
        //string ErrorFormat = "<div style=\"margin-top:-10;\"><table cellpadding=\"1\" cellspacing=\"0\" width=\"95%\"><tr><td valign=\"top\"><img src=\"img\\error.png\"/></td><td style=\"color:#FF6600; font-weight:bold;\">";
        //string ErrorFormatEnd = "</div></td></tr></table>";

        // default page number
        int ResultCount = 0;
        string moduleSearchPattern = string.Empty;

        //number of records shown per page - from web.config
        AppSettingsReader Settings = new AppSettingsReader();
        int ResultRecordsPerPage = int.Parse(Settings.GetValue("SearchRecordsPerPage", typeof(string)).ToString());

        //get current page number , by default '1'- used while paging
        PageResultPageNo = GetFieldValue("ResultPage", false);
        if (PageResultPageNo == string.Empty)
        {
            PageResultPageNo = GetFieldValue("PageResultPageNo", false);
        }
        if (PageResultPageNo != string.Empty)
        {

            CurrentResultPageNumber = int.Parse(PageResultPageNo) + 1;
        }

        // get search values - used while paging
        SearchMode = GetFieldValue("SearchMode", false);
        PageBreadcrumbIndex = GetFieldValue("PageBreadcrumbIndex", false);
        bool isElipseLink = GetFieldValue("IsElipseLink", false) != string.Empty;
        string breadCrumbIndex = GetFieldValue("BreadCrumbIndex", false);
        if (breadCrumbIndex == string.Empty)
        {
            breadCrumbIndex = PageBreadcrumbIndex;
        }
        isBreadcrumbPagingMode = GetFieldValue("PagingMode", false) == "true";

        bool resetList = GetFieldValue("ReloadList", false) == "true";
        bool clearFilter = GetFieldValue("ClearFilter", false) == "true";
        bool hasMultipleModules = false;
        bool hasMultipleAgencies = false;
        int previousRecsRead = 0;
        int arrayElementsToSkip = 0;
        int resultsTotalRecords = 0;

        CheckForCollectionActionRequest();
        GetSortOrder(resetList);

        SearchDescripton = SearchMode;

        List<CAPView4UI> capList = null;
        List<LPView4UI> lpList = null;
        List<APOView4UI> apoList = null;

        string per_globalsearch_label_searchresults = LocalGetTextByKey("per_globalsearch_label_searchresults");
        string per_globalsearch_label_cap = LocalGetTextByKeyWithoutHtml("per_globalsearch_label_cap");
        string per_globalsearch_label_lp = LocalGetTextByKeyWithoutHtml("per_globalsearch_label_lp");
        string mycollection_managepage_label_name = StripHTMLFromLabelText(LocalGetTextByKey("mycollection_collectionmanagement_collectionname"), "Collections");
        //string globalsearchparcelnavigatelabel = LocalGetTextByKey("globalsearch.parcel.navigate.label");
        //string globalsearchaddressnavigatelabel = LocalGetTextByKey("globalsearch.address.navigate.label");
        // hard code these since I cannot read the values from the GUI_TEXT table.
        string globalsearchparcelnavigatelabel = "Parcels";
        string globalsearchaddressnavigatelabel = "Addresses";

        //Get Result List
        #region Different SEARCH MODE
        //DataTable parcelList = null;
        DataTable Results = null;

        // If sort options have changed reset the list
        if (isSortOrderChange == true)
        {
            CurrentResultPageNumber = 1;
        }
        switch (SearchMode)
        {
            case "lpList":
		        try
		        {
                    //Get grid column visibility settings from ACA Global Search configutation settings.
                    IGviewBll gviewBll = (IGviewBll)ObjectFactory.GetObject(typeof(IGviewBll));
                    SimpleViewElementModel4WS[] models = gviewBll.GetSimpleViewElementModel(ModuleName, "60089");
                    licenseVisible = IsFieldVisible(models, "lnkLicenseProHeader");
                    licenseNameVisible = IsFieldVisible(models, "lnkLicenseTypeHeader");
                    licenseTypeVisible = IsFieldVisible(models, "lnkLicenseNameHeader");
                    businessNameVisible = IsFieldVisible(models, "lnkBusinessNameHeader");

                    // test exception processing
                    // throw new Exception("test and exception handeling for lpList");

                    // Read the search parameters
                    GlobalSearchParameter historyParameter = GlobalSearchManager.GetGlobalSearchParameter(GlobalSearchType.LP);

                    // If we are processing a list from a previouis global search call, then swich to 
                    // reading by page number. This allow records beyond 100 to be retrieved and displayed.
                    if (CurrentResultPageNumber > 1)
                    {
                        previousRecsRead = (CurrentResultPageNumber - 1) * ResultRecordsPerPage;
                        int ACApageIndex = previousRecsRead / historyParameter.PageSize;
                        lpList = GlobalSearchManager.ExecuteQuery<LPView4UI>(GlobalSearchType.LP, ACApageIndex,null);
                    }
                    else
                    {
                        // First time this result set is read by this page.
                        lpList = GlobalSearchManager.ExecuteQuery<LPView4UI>(GlobalSearchType.LP, SortColumn, gsSortDirection,null);
                    }

                    // Refresh the search parameter for current values.
                    historyParameter = GlobalSearchManager.GetGlobalSearchParameter(GlobalSearchType.LP);

                    // Capture the total records so far and check to see if results count is more than 100
                    resultsTotalRecords = historyParameter.TotalRecordsFromWS;

                    // Calculate the number of null search result array elements that will need to be skiped.
                    arrayElementsToSkip = previousRecsRead != 0 ? (previousRecsRead / 100) : 0;
                    arrayElementsToSkip = arrayElementsToSkip * 100;

                    // Convert the search results to the datatable that will be displayed.
                    Results = ConvertUtil.ConvertGlobalSearchResultLpListToDataTable<LPView4UI>(lpList, out hasMultipleAgencies, arrayElementsToSkip, ModuleName);

                    if (Results == null && Results.Rows.Count == 0)
                    {
                        Results = null;
                    }

                }
                catch (Exception ex)
                {
                    ErrorMessage.Append(ErrorFormat);
                    ErrorMessage.Append(ex.Message);
                    ErrorMessage.Append(ErrorFormatEnd);
                    ErrorMessage.Append("<br>");
                    lpList = new List<LPView4UI>();
                }
                iPhonePageTitle = per_globalsearch_label_lp;
                if (isiPhone == false)
                {
                    DisplayMode = "<div id=\"pageTitle\">" + iPhonePageTitle + "</div>";
                }
                sbWork.Append("&SearchMode=" + SearchMode);
                sbWork.Append("&ResultPage=" + (CurrentResultPageNumber - 1).ToString());
                sbWork.Append("&Module=" + ModuleName);
                Breadcrumbs = BreadCrumbHelper("GlobalSearch.List.aspx", sbWork, per_globalsearch_label_lp, breadCrumbIndex, isElipseLink, isBreadcrumbRefeshMode, isBreadcrumbPagingMode, false);
                BackForwardLinks = BackLinkHelper(CurrentBreadCrumbIndex.ToString());
                SearchDescripton = SearchMode;
                break;
            case "apoList":
            case "adrList":
                try
                {
                    //Get grid column visibility settings from ACA Global Search configutation settings.
                    IGviewBll gviewBll = (IGviewBll)ObjectFactory.GetObject(typeof(IGviewBll));
                    SimpleViewElementModel4WS[] models = gviewBll.GetSimpleViewElementModel(ModuleName, "60093");
                    addressVisible = IsFieldVisible(models, "lnkAddressHeader");
                    parcelVisible = IsFieldVisible(models, "lnkParcelNumberHeader");
                    ownerVisisble = IsFieldVisible(models, "lnkOwnerHeader");
  
                    if (SearchMode == "adrList")
                    {
                        addressVisible = true;
                        iPhonePageTitle = globalsearchaddressnavigatelabel;
                        //test exception processing
                        //throw new Exception("test and exception handeling for adrList");
                    }
                    else
                    {
                        parcelVisible = true;
                        iPhonePageTitle = globalsearchparcelnavigatelabel;
                        //test exception processing
                        //throw new Exception("test and exception handeling for apoList");
                    }
                    if (isiPhone == false)
                    {
                        DisplayMode = "<div id=\"pageTitle\">" + iPhonePageTitle + "</div>";
                    }
                    GlobalSearchParameter historyParameter = null;
                    if (SearchMode == "adrList")
                    {
                        // Read the search parameters
                        historyParameter = GlobalSearchManager.GetGlobalSearchParameter(GlobalSearchType.ADDRESS);
                        // if previous search results was by PARCEL then switch to ADDRESS
                        if (historyParameter.GlobalSearchType == GlobalSearchType.PARCEL)
                        {
                            string[] modules = GlobalSearchUtil.GetAllModuleKeys();
                            // apoList = GlobalSearchManager.ExecuteQuery<APOView4UI>(GlobalSearchType.ADDRESS, historyParameter.QueryText, modules, gsSortColumn, gsSortDirection);
                            apoList = GlobalSearchManager.ExecuteQuery<APOView4UI>(GlobalSearchType.ADDRESS, historyParameter.QueryText, modules, ADDRESS_FIRST_COLUMN_NAME, gsSortDirection, historyParameter.PageSize,null);
                        }
                        else
                        {
                            // If we are processing a list from a previouis global search call, then swich to 
                            // reading by page number. This allow records beyond 100 to be retrieved and displayed.
                            if (CurrentResultPageNumber > 1)
                            {
                                previousRecsRead = (CurrentResultPageNumber - 1) * ResultRecordsPerPage;
                                int ACApageIndex = previousRecsRead / historyParameter.PageSize;
                                apoList = GlobalSearchManager.ExecuteQuery<APOView4UI>(GlobalSearchType.ADDRESS, ACApageIndex,null);
                            }
                            else
                            {
                                apoList = GlobalSearchManager.ExecuteQuery<APOView4UI>(GlobalSearchType.ADDRESS, gsSortColumn, gsSortDirection,null);
                            }
                        }
                        // Refresh the search parameter for current values.
                        historyParameter = GlobalSearchManager.GetGlobalSearchParameter(GlobalSearchType.ADDRESS);
                    }
                    else
                    {
                        // Read the search parameters
                        historyParameter = GlobalSearchManager.GetGlobalSearchParameter(GlobalSearchType.PARCEL);

                        // if previous search results was by ADDRESS then switch to PARCEL
                        if (historyParameter.GlobalSearchType == GlobalSearchType.ADDRESS)
                        {
                            string[] modules = GlobalSearchUtil.GetAllModuleKeys();
                            apoList = GlobalSearchManager.ExecuteQuery<APOView4UI>(GlobalSearchType.PARCEL, historyParameter.QueryText, modules, PARCEL_FIRST_COLUMN_NAME, gsSortDirection, historyParameter.PageSize,null);
                        }
                        else
                        {
                            // If we are processing a list from a previouis global search call, then swich to 
                            // reading by page number. This allow records beyond 100 to be retrieved and displayed.
                            if (CurrentResultPageNumber > 1)
                            {
                                previousRecsRead = (CurrentResultPageNumber - 1) * ResultRecordsPerPage;
                                int ACApageIndex = previousRecsRead / historyParameter.PageSize;
                                apoList = GlobalSearchManager.ExecuteQuery<APOView4UI>(GlobalSearchType.PARCEL, ACApageIndex,null);
                            }
                            else
                            {
                                apoList = GlobalSearchManager.ExecuteQuery<APOView4UI>(GlobalSearchType.PARCEL, gsSortColumn, gsSortDirection,null);
                            }
                        }
                        // Refresh the search parameter for current values.
                        historyParameter = GlobalSearchManager.GetGlobalSearchParameter(GlobalSearchType.PARCEL);
                    }
                    // Capture the total records so far and check to see if results count is more than 100
                    resultsTotalRecords = historyParameter.TotalRecordsFromWS;

                    // Calculate the number of null search result array elements that will need to be skiped.
                    arrayElementsToSkip = previousRecsRead != 0 ? (previousRecsRead / 100) : 0;
                    arrayElementsToSkip = arrayElementsToSkip * 100;

                    // Convert the search results to the datatable that will be displayed.
                    Results = ConvertUtil.ConvertGlobalSearchResultApoListToDataTable<APOView4UI>(apoList, arrayElementsToSkip);
                    if (Results == null || Results.Rows.Count == 0)
                    {
                        Results = null;
                    }
                }
                catch (Exception ex)
                {
                    ErrorMessage.Append(ErrorFormat);
                    ErrorMessage.Append(ex.Message);
                    ErrorMessage.Append(ErrorFormatEnd);
                    ErrorMessage.Append("<br>");
                    apoList = new List<APOView4UI>();
                }
                sbWork.Append("&SearchMode=" + SearchMode);
                sbWork.Append("&ResultPage=" + (CurrentResultPageNumber - 1).ToString());
                sbWork.Append("&Module=" + ModuleName);
                Breadcrumbs = BreadCrumbHelper("GlobalSearch.List.aspx", sbWork, iPhonePageTitle, breadCrumbIndex, isElipseLink, isBreadcrumbRefeshMode, isBreadcrumbPagingMode, false);
                BackForwardLinks = BackLinkHelper(CurrentBreadCrumbIndex.ToString());

                SearchDescripton = SearchMode;
                break;
            default:
                ListMode = per_globalsearch_label_cap;
                iPhonePageTitle = ListMode;
                if (isiPhone == false)
                {
                    DisplayMode = "<div id=\"pageTitle\">" + iPhonePageTitle + "</div>";
                }
                try
                {
                    //test exception processing
                    //throw new Exception("test and exception handeling for capList");

                    // Read the search parameters
                    GlobalSearchParameter historyParameter = GlobalSearchManager.GetGlobalSearchParameter(GlobalSearchType.CAP);

                    // If we are processing a list from a previouis global search call, then swich to 
                    // reading by page number. This allow records beyond 100 to be retrieved and displayed.
                    if (CurrentResultPageNumber > 1)
                    {
                        previousRecsRead = (CurrentResultPageNumber - 1) * ResultRecordsPerPage;
                        int ACApageIndex = previousRecsRead / historyParameter.PageSize;
                        capList = GlobalSearchManager.ExecuteQuery<CAPView4UI>(GlobalSearchType.CAP, ACApageIndex,null);
                    }
                    else
                    {
                        // First time this result set is read by this page.
                        capList = GlobalSearchManager.ExecuteQuery<CAPView4UI>(GlobalSearchType.CAP, gsSortColumn, gsSortDirection,null);
                    }

                    // Refresh the search parameter for current values.
                    historyParameter = GlobalSearchManager.GetGlobalSearchParameter(GlobalSearchType.CAP);
                    
                    // Capture the total records so far and check to see if results count is more than 100
                    resultsTotalRecords = historyParameter.TotalRecordsFromWS;

                    // Calculate the number of null search result array elements that will need to be skiped.
                    arrayElementsToSkip = previousRecsRead != 0 ? (previousRecsRead / 100) : 0;
                    arrayElementsToSkip = arrayElementsToSkip * 100;

                    // Convert the search results to the datatable that will be displayed.
                    Results = ConvertUtil.ConvertGlobalSearchResultCapListToDataTable<CAPView4UI>(capList, ModuleName, out hasMultipleModules, out hasMultipleAgencies, arrayElementsToSkip);
                    if (resultsTotalRecords > 100)
                    {
                        hasMultipleModules = true;
                    }
                    CollectionLinks.Append("<div id=\"collectionActions\">");
                    if (isiPhone == true)
                    {
                        CollectionLinks.Append("<center>");
                    }
                                        
                    HTMLFactory htmlFactory = new HTMLFactory();
                    CollectionLinks.Append("<b>" + htmlFactory.PresentSubmitButton(isOpera, "submitAdd", "Add to " + mycollection_managepage_label_name) + "</b>");

                    if (isiPhone == true)
                    {
                        CollectionLinks.Append("</center>");
                    }
                    CollectionLinks.Append("</div>");

                    if (Results == null || Results.Rows.Count == 0)
                    {
                        Results = null;
                    }
                }
                catch (Exception ex)
                {
                    ErrorMessage.Append(ErrorFormat);
                    ErrorMessage.Append(ex.Message);
                    ErrorMessage.Append(ErrorFormatEnd);
                    ErrorMessage.Append("<br>");
                    capList = new List<CAPView4UI>();
                }
                sbWork.Append("&SearchMode=" + SearchMode);
                sbWork.Append("&ResultPage=" + (CurrentResultPageNumber - 1).ToString());
                sbWork.Append("&Module=" + ModuleName);
                if (ModuleName != null && ModuleName != string.Empty)
                {
                    Breadcrumbs = BreadCrumbHelper("GlobalSearch.List.aspx", sbWork, ModuleName, breadCrumbIndex, isElipseLink, isBreadcrumbRefeshMode, isBreadcrumbPagingMode, false);
                }
                else
                {
                    Breadcrumbs = BreadCrumbHelper("GlobalSearch.List.aspx", sbWork, ListMode, breadCrumbIndex, isElipseLink, isBreadcrumbRefeshMode, isBreadcrumbPagingMode, false);
                }
                PageBreadcrumbIndex = CurrentBreadCrumbIndex.ToString();
                BackForwardLinks = BackLinkHelper(CurrentBreadCrumbIndex.ToString());
                if (ModuleName != null && ModuleName != string.Empty)
                SearchDescripton = ListMode;
                break;
        }


        #endregion

        #region  Permit Search

        if (Results != null)
        {
            ResultCount = resultsTotalRecords; // Results.DefaultView.Count + arrayElementsToSkip;
        }
        int ResultPageStart = (CurrentResultPageNumber - 1) * ResultRecordsPerPage;
        // Populate Records 
        if (ResultCount > 0)
        {
            int FirstRecord = ResultPageStart + 1;
            int LastRecord = ResultCount;
            if (ResultCount >= ResultRecordsPerPage)
            {
                LastRecord = (FirstRecord - 1) + ResultRecordsPerPage;
                if (LastRecord > ResultCount)
                {
                    LastRecord = ResultCount;
                }
            }
            ResultHeader.Append("<div style=\"margin-top:5px; margin-bottom:5px; \">" + "Showing ");
            ResultHeader.Append(FirstRecord.ToString() + "-");
            ResultHeader.Append(LastRecord.ToString() + " of "
                + SearchResultUtil.GenerateRecordsSummary(ResultCount, GlobalSearch_Result_Page_Size, CurrentResultPageNumber - ACAConstant.ADDITIONAL_RECORDS_COUNT));
            ResultHeader.Append("</div>");
            string oodRowBackcolor = string.Empty;
            try
            {
                oodRowBackcolor = "#e5f3fd";
            }
            catch
            {
                oodRowBackcolor = string.Empty;
            }
            string tableRowEnd = string.Empty;

            Boolean oddRow = true;
            if (isiPhone != true)
            {
                ResultOutput.Append("<table width=\"95%\" cellspacing=\"0px\" cellpadding=\"0px\" border=\"1\" style=\"margin: 0px 0px 10px 0px; border-right-width:0px; border-left-width:0px; border-top-width:0px; border-bottom-width:0px; border-color:#e6e6e6;\" >");
            }
            string checkBox = string.Empty;
            int rowNum = 0;
            int recCount = 0;
            // new scrolling past the 100th record
            recCount = arrayElementsToSkip;
            foreach (DataRowView aRow in Results.DefaultView)
            {
                if (recCount >= ResultPageStart)
                {
                    string tableCellHTML = "<td  valign=\"top\" style=\"border-right-width:0px; border-left-width:0px; border-top-width:0px; border-bottom-width:1px; border-color:#e6e6e6;";

                    if (oddRow == true && oodRowBackcolor != string.Empty)
                    {
                        oddRow = false;
                        tableCellHTML = tableCellHTML + " background-color:" + oodRowBackcolor + ";\">";
                    }
                    else
                    {
                        oddRow = true;
                        tableCellHTML = tableCellHTML + "\">";
                    }
                    string endHTML = "</td>";
                    string detailDelimiter = " - ";
                    if (isiPhone == true)
                    {
                        tableCellHTML = string.Empty;
                        endHTML = string.Empty;
                        detailDelimiter = "<br>";
                    }
                   StringBuilder cellWork = new StringBuilder();
                   bool isLinkText = true;
                   if (SearchMode == "capList")
                    {
                        string[] capIdSegments = aRow["Number"].ToString().Split('-');
                        checkBox = tableCellHTML + "<input id=\"Row" + rowNum.ToString() + "\" type=\"checkbox\""
                            + " name=\"Row" + rowNum.ToString() + "\""
                            + " value=\""
                            + ((capIdSegments[0] == null) ? string.Empty : capIdSegments[0])
                            + "|"
                            + ((capIdSegments[1] == null) ? string.Empty : capIdSegments[1])
                            + "|"
                            + ((capIdSegments[2] == null) ? string.Empty : capIdSegments[2])
                            + "|"
                            + ((aRow["AltId"] != null) ? string.Empty : aRow["AltId"].ToString())
                            + "|"
                            + ((aRow["Agency"] == null) ? string.Empty : aRow["Agency"].ToString())
                            + "|"
                            + ((aRow["Module"] != null) ? string.Empty : aRow["Module"].ToString())
                            + "|none"
                            + "\"/>" + endHTML;
                        rowNum++;
                        //string delimiter = " - ";
                        if (isiPhone == true)
                        {
                            if (aRow["capClass"] == null || aRow["capClass"].ToString() == string.Empty || aRow["capClass"].ToString() == ACAConstant.COMPLETED)
                            {
                                cellWork.Append("<a class=\"pageListLink\" href=\"");
                            }
                            else
                            {
                                isLinkText = false;
                             }
                        }
                        else
                        {
                            cellWork.Append("<tr height=\"40px\">" + checkBox + tableCellHTML);
                            if (aRow["capClass"] == null || aRow["capClass"].ToString() == string.Empty || aRow["capClass"].ToString() == ACAConstant.COMPLETED)
                            {
                                cellWork.Append(ListLinkStyle);
                            }
                        }
                        if (aRow["capClass"] == null || aRow["capClass"].ToString() == string.Empty || aRow["capClass"].ToString() == ACAConstant.COMPLETED)
                        {
                            cellWork.Append("Permits.View.aspx?State=" + State.ToString());
                            cellWork.Append("&PermitType=" + ((aRow["Type"] == null ? string.Empty : aRow["Type"].ToString())));
                            cellWork.Append("&PermitNumber=" + ((aRow["Number"] == null) ? string.Empty : aRow["Number"].ToString()));
                            cellWork.Append("&AltID=" + ((aRow["AltId"].ToString() == null || aRow["AltId"].ToString() == string.Empty) ? aRow["Number"].ToString() : aRow["AltId"].ToString()));
                            cellWork.Append("&Module=" + ((aRow["Module"].ToString() == null) ? string.Empty : aRow["Module"].ToString()));
                            cellWork.Append("&Mode=" + ListMode);
                            cellWork.Append("&ResultPage=" + (CurrentResultPageNumber - 1).ToString());
                            cellWork.Append("&GlobalSearch=true");
                            // ResultOutput.Append("\">" + aRow["Date"].ToString() + " - " + ((aRow["AltId"].ToString() == null || aRow["AltId"].ToString() == string.Empty) ? aRow["Number"].ToString() : aRow["AltId"].ToString()) + "</a>");
                            if (isiPhone == true)
                            {
                                cellWork.Append("\"><span class=\"pageListCellBoldText\">" + ((aRow["AltId"].ToString() == null || aRow["AltId"].ToString() == string.Empty) ? aRow["Number"].ToString() : aRow["AltId"].ToString()) + "</span>");
                            }
                            else
                            {
                                cellWork.Append("\">" + ((aRow["AltId"].ToString() == null || aRow["AltId"].ToString() == string.Empty) ? aRow["Number"].ToString() : aRow["AltId"].ToString()) + "</a>");
                            }
                        }
                        else
                        {
                            if (isiPhone == true)
                            {
                                cellWork.Append("<span class=\"pageListNoLink\">" + ((aRow["AltId"].ToString() == null || aRow["AltId"].ToString() == string.Empty) ? aRow["Number"].ToString() : aRow["AltId"].ToString()) + "</span>");
                            }
                            else
                            {
                                cellWork.Append(((aRow["AltId"].ToString() == null || aRow["AltId"].ToString() == string.Empty) ? aRow["Number"].ToString() : aRow["AltId"].ToString()));
                            }
                        }
                        if (aRow["Address"] != null && aRow["Address"].ToString() != string.Empty)
                        {
                            cellWork.Append(detailDelimiter + aRow["Address"].ToString());
                        }
                        if (aRow["Status"] != null && aRow["Status"].ToString() != string.Empty)
                        {
                            cellWork.Append(detailDelimiter + aRow["Status"].ToString());
                            detailDelimiter = " - ";
                        }
                        if (hasMultipleModules == true && aRow["Module"] != null && aRow["Module"].ToString() != string.Empty)
                        {
                            cellWork.Append(detailDelimiter + "<i>" + aRow["Module"].ToString() + "</i>");
                            detailDelimiter = " - ";
                        }
                        if (hasMultipleAgencies == true && aRow["Agency"] != null && aRow["Agency"].ToString() != string.Empty)
                        {
                            cellWork.Append(detailDelimiter + "<i>" + aRow["Agency"].ToString() + "</i>");
                        }
                    }
                    else if (SearchMode == "apoList")
                    {
                        if (isiPhone == true)
                        {
                            cellWork.Append("<a class=\"pageListLink\" href=\"");
                        }
                        else
                        {
                            cellWork.Append("<tr height=\"40px\">" + tableCellHTML + ListLinkStyle);
                        }

                        cellWork.Append("Parcel.Info.aspx?State=" + State.ToString());
                        cellWork.Append("&SearchMode=" + SearchMode.ToString());
                        cellWork.Append("&ParcelNumber=" + aRow["ParcelNumber"].ToString());
                        cellWork.Append("&ParcelSequence=" + aRow["ParcelSeqNbr"].ToString());
                        cellWork.Append("&ResultPage=" + (CurrentResultPageNumber - 1).ToString());
                        cellWork.Append("&GlobalSearch=true");
                        cellWork.Append("\">");

                        string listLinkCloseHTML = "</a>";
                        string iPhoneLinkHTML = string.Empty;
                        string iPhoneLinkCloseHTML = string.Empty;

                        if (isiPhone == true)
                        {
                            iPhoneLinkHTML = "<span style=\"width:90%\"><span class=\"pageListCellBoldText\">";
                            iPhoneLinkCloseHTML = "</span></span>";
                        }

                        if (parcelVisible)
                        {
                            cellWork.Append(iPhoneLinkHTML);
                            cellWork.Append(aRow["ParcelNumber"].ToString());
                            cellWork.Append(iPhoneLinkCloseHTML);
                            cellWork.Append(listLinkCloseHTML);
                            listLinkCloseHTML = string.Empty;
                            iPhoneLinkHTML = string.Empty;
                            iPhoneLinkCloseHTML = string.Empty;
                        }

                        if (ownerVisisble)
                        {
                            cellWork.Append(iPhoneLinkHTML);
                            cellWork.Append(parcelVisible ? detailDelimiter : string.Empty);
                            cellWork.Append(aRow["OwnerName"].ToString());
                            cellWork.Append(iPhoneLinkCloseHTML);
                            cellWork.Append(listLinkCloseHTML);
                            listLinkCloseHTML = string.Empty;
                            iPhoneLinkHTML = string.Empty;
                            iPhoneLinkCloseHTML = string.Empty;
                        }

                        if (addressVisible)
                        {
                            cellWork.Append(iPhoneLinkHTML);
                            cellWork.Append(parcelVisible ? detailDelimiter : string.Empty);
                            cellWork.Append(aRow["AddressDescription"].ToString());
                            cellWork.Append(iPhoneLinkCloseHTML);
                            cellWork.Append(listLinkCloseHTML);
                            listLinkCloseHTML = string.Empty;
                            iPhoneLinkHTML = string.Empty;
                            iPhoneLinkCloseHTML = string.Empty;
                        }

                        rowNum++;
                    }
                    else if (SearchMode == "adrList")
                    {
                        if (isiPhone == true)
                        {
                            cellWork.Append("<a class=\"pageListLink\" href=\"");
                        }
                        else
                        {
                            cellWork.Append("<tr height=\"40px\">" + tableCellHTML + ListLinkStyle);
                        }
                        cellWork.Append("Address.Info.aspx?State=" + State.ToString());
                        cellWork.Append("&SearchMode=" + SearchMode.ToString());
                        cellWork.Append("&AddressSeq=" + aRow["AddressSeqNumber"].ToString());
                        cellWork.Append("&AddressRefId=" + aRow["AddressSourceNumber"].ToString());
                        cellWork.Append("&ResultPage=" + (CurrentResultPageNumber - 1).ToString());
                        cellWork.Append("&GlobalSearch=true");
                        cellWork.Append("\">");
                        /*
                        if (isiPhone == true)
                        {
                            cellWork.Append("<span class=\"pageListCellBoldText\">" + aRow["AddressDescription"].ToString() + "</span>");
                        }
                        else
                        {
                            cellWork.Append(aRow["AddressDescription"].ToString());
                            cellWork.Append("</a>");
                        }
                        cellWork.Append(detailDelimiter + aRow["ParcelNumber"].ToString());
                        cellWork.Append(detailDelimiter + aRow["OwnerName"].ToString());
                         */
                        string listLinkCloseHTML = "</a>";
                        string iPhoneLinkHTML = string.Empty;
                        string iPhoneLinkCloseHTML = string.Empty;

                        if (isiPhone == true)
                        {
                            iPhoneLinkHTML = "<span style=\"width:90%\"><span class=\"pageListCellBoldText\">";
                            iPhoneLinkCloseHTML = "</span></span>";
                        }

                        if (addressVisible)
                        {
                            cellWork.Append(iPhoneLinkHTML);
                            cellWork.Append(aRow["AddressDescription"].ToString());
                            cellWork.Append(iPhoneLinkCloseHTML);
                            cellWork.Append(listLinkCloseHTML);
                            listLinkCloseHTML = string.Empty;
                            iPhoneLinkHTML = string.Empty;
                            iPhoneLinkCloseHTML = string.Empty;
                        }

                        if (parcelVisible)
                        {
                            cellWork.Append(iPhoneLinkHTML);
                            cellWork.Append(addressVisible ? detailDelimiter : string.Empty);
                            cellWork.Append(aRow["ParcelNumber"].ToString());
                            cellWork.Append(iPhoneLinkCloseHTML);
                            cellWork.Append(listLinkCloseHTML);
                            listLinkCloseHTML = string.Empty;
                            iPhoneLinkHTML = string.Empty;
                            iPhoneLinkCloseHTML = string.Empty;
                        }


                        if (ownerVisisble)
                        {
                            cellWork.Append(iPhoneLinkHTML);
                            cellWork.Append(addressVisible || parcelVisible ? detailDelimiter : string.Empty);
                            cellWork.Append(aRow["OwnerName"].ToString());
                            cellWork.Append(iPhoneLinkCloseHTML);
                            cellWork.Append(listLinkCloseHTML);
                            listLinkCloseHTML = string.Empty;
                            iPhoneLinkHTML = string.Empty;
                            iPhoneLinkCloseHTML = string.Empty;
                        }

                        rowNum++;
                    }
                    else if (SearchMode == "lpList")
                    {
                        if (isiPhone == true)
                        {
                            cellWork.Append("<a class=\"pageListLink\" href=\"");
                        }
                        else
                        {
                            cellWork.Append("<tr height=\"40px\">" + tableCellHTML + ListLinkStyle);
                        }

                        cellWork.Append("License.Info.aspx?State=" + HttpUtility.UrlEncode(State));
                        cellWork.Append("&SearchMode=" + HttpUtility.UrlEncode(SearchMode));
                        cellWork.Append("&LicenseNumber=" + HttpUtility.UrlEncode(aRow["LicenseNumber"].ToString()));
                        cellWork.Append("&LicenseType=" + HttpUtility.UrlEncode(aRow["LicenseType"].ToString()));
                        cellWork.Append("&ResultPage=" + (CurrentResultPageNumber - 1).ToString());
                        cellWork.Append("&GlobalSearch=true");
                        cellWork.Append("\">");
                        string listLinkCloseHTML = "</a>";
                        string iPhoneLinkHTML = string.Empty;
                        string iPhoneLinkCloseHTML = string.Empty;
                       
                        if (isiPhone == true)
                        {
                            iPhoneLinkHTML = "<span style=\"width:90%\"><span class=\"pageListCellBoldText\">";
                            iPhoneLinkCloseHTML = "</span></span>";
                        }
                        if (licenseVisible)
                        {
                            cellWork.Append(iPhoneLinkHTML);
                            cellWork.Append(aRow["LicenseNumber"].ToString());
                            cellWork.Append(iPhoneLinkCloseHTML);
                            cellWork.Append(listLinkCloseHTML);
                            listLinkCloseHTML = string.Empty;
                            iPhoneLinkHTML = string.Empty;
                            iPhoneLinkCloseHTML = string.Empty;
                        }
                        if (licenseTypeVisible)
                        {
                            cellWork.Append(iPhoneLinkHTML);
                            cellWork.Append(licenseVisible == true ? " (" : string.Empty);
                            cellWork.Append(aRow["LicenseType"].ToString());
                            cellWork.Append(licenseVisible == true ? ")" : string.Empty);
                            cellWork.Append(iPhoneLinkCloseHTML);
                            cellWork.Append(listLinkCloseHTML);
                            listLinkCloseHTML = string.Empty;
                            iPhoneLinkHTML = string.Empty;
                            iPhoneLinkCloseHTML = string.Empty;
                        }

                        if (licenseNameVisible)
                        {
                            cellWork.Append(iPhoneLinkHTML);
                            cellWork.Append(licenseVisible || licenseTypeVisible ? detailDelimiter :string.Empty);
                            cellWork.Append(aRow["LicenseProfessionalName"].ToString());
                            cellWork.Append(iPhoneLinkCloseHTML);
                            cellWork.Append(listLinkCloseHTML);
                            listLinkCloseHTML = string.Empty;
                            iPhoneLinkHTML = string.Empty;
                            iPhoneLinkCloseHTML = string.Empty;
                        }
                        if (businessNameVisible)
                        {
                            cellWork.Append(iPhoneLinkHTML);
                            cellWork.Append(licenseVisible || licenseTypeVisible ? detailDelimiter : string.Empty);
                            cellWork.Append(aRow["businessName"].ToString());
                            cellWork.Append(iPhoneLinkCloseHTML);
                            cellWork.Append(listLinkCloseHTML);
                            listLinkCloseHTML = string.Empty;
                            iPhoneLinkHTML = string.Empty;
                            iPhoneLinkCloseHTML = string.Empty;
                        }
                        rowNum++;
                    }
                    if (isiPhone == true)
                    {
                        ResultOutput.Append(MyProxy.CreateListCell(checkBox, cellWork.ToString(), rowNum - 1, recCount, ResultCount, ResultPageStart, ResultRecordsPerPage, isiPhone, true, isLinkText));
                    }
                    else
                    {
                        cellWork.Append("</td></tr>");
                        ResultOutput.Append(cellWork.ToString());
                    }
                    if (rowNum >= ResultRecordsPerPage)
                    {
                        break;
                    }
                }
                recCount++;
            }
            if (isiPhone != true)
            {
                ResultOutput.Append("</table>");
            }
            capsInList = rowNum.ToString();

            string sortDirectionIcon = "img/Caret_up_sml.gif";
            if (sortDescending == true)
            {
                sortDirectionIcon = "img/Caret_down_sml.gif";
            }
            string inputEndHTML = " | ";

            SortOptions.Append("<div id=\"sortSection\">");
            SortOptions.Append("Sort By: ");
            string verticalDivider = string.Empty;

            HTMLFactory htmlFactory = new HTMLFactory();

            if (SearchMode == "lpList")
            {
                string sortDirectionHTML = "<img src=\"" + sortDirectionIcon + "\">";
                if (licenseVisible)
                {
                    SortOptions.Append(htmlFactory.PresentSubmitButton(isOpera, "sortNumber", "License", sortByNumber == true ? " font-weight:bold;" : string.Empty));
                    if (sortByNumber == true)
                    {
                        SortOptions.Append(sortDirectionHTML);
                    }
                    verticalDivider = inputEndHTML;
                }
                if (licenseTypeVisible)
                {
                    SortOptions.Append(verticalDivider);
                    verticalDivider = inputEndHTML;

                    SortOptions.Append(htmlFactory.PresentSubmitButton(isOpera, "sortType", "Type", sortByType == true ? " font-weight:bold;" : string.Empty));
                    if (sortByType == true)
                    {
                        SortOptions.Append(sortDirectionHTML);
                    }
                }
                if (licenseNameVisible)
                {
                    SortOptions.Append(verticalDivider);
                    verticalDivider = inputEndHTML;

                    SortOptions.Append(htmlFactory.PresentSubmitButton(isOpera, "sortName", "Name", sortByName == true ? " font-weight:bold;" : string.Empty));
                    if (sortByName == true)
                    {
                        SortOptions.Append(sortDirectionHTML);
                    }
                }
                if (businessNameVisible)
                {
                    SortOptions.Append(verticalDivider);
                    verticalDivider = inputEndHTML;

                    SortOptions.Append(htmlFactory.PresentSubmitButton(isOpera, "sortBizName", "Business", sortByBizName == true ? " font-weight:bold;" : string.Empty));
                    if (sortByBizName == true)
                    {
                        SortOptions.Append(sortDirectionHTML);
                    }
                }
            }
            if (SearchMode == "apoList")
            {
                string sortDirectionHTML = "<img src=\"" + sortDirectionIcon + "\"/>";
                if (parcelVisible)
                {
                    verticalDivider = inputEndHTML;

                    SortOptions.Append(htmlFactory.PresentSubmitButton(isOpera, "sortNumber", "Parcel", sortByNumber == true ? " font-weight:bold;" : string.Empty));
                    if (sortByNumber == true)
                    {
                        SortOptions.Append(sortDirectionHTML);
                    }
                }
                /* owner sort is not supported by GlobalSearchBLL at this time
                if (ownerVisisble)
                {
                    SortOptions.Append(verticalDivider);
                    verticalDivider = inputEndHTML;
                    
                    SortOptions.Append(htmlFactory.PresentSubmitButton(isOpera, "sortName", "Owner", sortByName == true ? " font-weight:bold;" : string.Empty));
                    if (sortByName == true)
                    {
                        SortOptions.Append(sortDirectionHTML);
                    }
                }
                if (addressVisible)
                {
                    SortOptions.Append(verticalDivider);

                    SortOptions.Append(htmlFactory.PresentSubmitButton(isOpera, "sortAddress","Address",sortByAddress == true ? " font-weight:bold;" : string.Empty));
                    if (sortByAddress == true)
                    {
                        SortOptions.Append(sortDirectionHTML);
                    }
                }
                */ 
            }
            if (SearchMode == "adrList")
            {
                string sortDirectionHTML = "<img src=\"" + sortDirectionIcon + "\"/>";
                if (addressVisible)
                {
                    verticalDivider = inputEndHTML;
                    SortOptions.Append(htmlFactory.PresentSubmitButton(isOpera, "sortAddress", "Address", sortByAddress == true ? " font-weight:bold;" : string.Empty));
                    if (sortByAddress == true)
                    {
                        SortOptions.Append(sortDirectionHTML);
                    }
                }
                /* parcel and owner sort is not supported by GlobalSearchBLL at this time
                if (parcelVisible)
                {
                    SortOptions.Append(verticalDivider);
                    verticalDivider = inputEndHTML;

                    SortOptions.Append("<input type=\"submit\"  name=\"sortNumber\" value=\"Parcel\"");
                    SortOptions.Append((sortByNumber == true ? " font-weight:bold;" : string.Empty));
                    SortOptions.Append("\"/>");
                    if (sortByNumber == true)
                    {
                        SortOptions.Append(sortDirectionHTML);
                    }
                }
                if (ownerVisisble)
                {
                    SortOptions.Append(verticalDivider);

                    SortOptions.Append("<input type=\"submit\"  name=\"sortName\" value=\"Owner\"");
                    SortOptions.Append((sortByName == true ? " font-weight:bold;" : string.Empty));
                    SortOptions.Append("\"/>");
                    if (sortByName == true)
                    {
                        SortOptions.Append(sortDirectionHTML);
                    }
                }
                 */
            }
            if (SearchMode == "capList")
            {
                string sortDirectionHTML = "<img src=\"" + sortDirectionIcon + "\"/>";
                SortOptions.Append(htmlFactory.PresentSubmitButton(isOpera, "sortDate", "Date", sortByDate == true ? " font-weight:bold;" : string.Empty));
                if (sortByDate == true)
                {
                    SortOptions.Append(sortDirectionHTML);
                }
                SortOptions.Append(inputEndHTML);

                SortOptions.Append(htmlFactory.PresentSubmitButton(isOpera, "sortNumber", "ID", sortByNumber == true ? " font-weight:bold;" : string.Empty));
                if (sortByNumber == true)
                {
                    SortOptions.Append(sortDirectionHTML);
                }

                SortOptions.Append(inputEndHTML);
                SortOptions.Append(htmlFactory.PresentSubmitButton(isOpera, "sortStatus", "Status", sortByStatus == true ? " font-weight:bold;" : string.Empty));
                if (sortByStatus == true)
                {
                    SortOptions.Append(sortDirectionHTML);
                }

                if (hasMultipleModules == true)
                {
                    SortOptions.Append(inputEndHTML);
                    SortOptions.Append(htmlFactory.PresentSubmitButton(isOpera, "sortModule", "Module", sortByModule == true ? " font-weight:bold;" : string.Empty));
                    if (sortByModule == true)
                    {
                        SortOptions.Append(sortDirectionHTML);
                    }
                }
                if (hasMultipleAgencies == true)
                {
                    SortOptions.Append(inputEndHTML);
                    SortOptions.Append(htmlFactory.PresentSubmitButton(isOpera, "sortAgency", "Agency", sortByAgency == true ? " font-weight:bold;" : string.Empty));
                    if (sortByAgency == true)
                    {
                        SortOptions.Append(sortDirectionHTML);
                    }
                }
            }
            // SortOptions.Append("</td></table>");
            SortOptions.Append("</div>");
        }
        #endregion

        #region Paging Part
        // Paging Part, appends to table footer
        if ((double)ResultCount > (double)ResultRecordsPerPage)
        {
            int ResultPagesCount = (int)Math.Ceiling((double)ResultCount / (double)ResultRecordsPerPage);
            if ((int)Math.Ceiling((double)ResultPagesCount * (double)ResultRecordsPerPage) < ResultCount)
            {
                ResultPagesCount++;
            }
            int NextPageNumber = 0;
            int PageCount = 0;
            int PageStart = 0;
            int PreviousPageNumber = CurrentResultPageNumber - 1;
            if (CurrentResultPageNumber < 6)
            {
                PageStart = 1;
            }
            else
            {
                PageStart = CurrentResultPageNumber - 5;
            }

            string sLinkFormat = "<a id=\"pageNavigationButton\" href=\"";
            StringBuilder PagingLink = new StringBuilder();

            PagingLink.Append(sLinkFormat + "GlobalSearch.List.aspx?State=" + State);
            PagingLink.Append("&PagingMode=true");
            PagingLink.Append("&SearchMode=" + SearchMode);
            PagingLink.Append("&Module=" + ModuleName);
            PagingLink.Append("&SortColumn=" + SortColumn.ToString());
            PagingLink.Append("&SortDesc=" + SortDesc.ToString());

            PagingFooter.Append("<div id=\"pageNavigation\">");
            string delimiter = string.Empty;
            if (isiPhone == true)
            {
                PagingFooter.Append("<center>");
            }
            if (PreviousPageNumber > 0)
            {
                PagingFooter.Append(PagingLink.ToString());
                PagingFooter.Append("&SlidePage=LeftToRight");
                PagingFooter.Append("&ResultPage=" + (PreviousPageNumber - 1).ToString());
                PagingFooter.Append("\">&lt;</a>");
                delimiter = " | ";
            }
            for (int page = PageStart; page <= ResultPagesCount; page++)
            {
                PageCount++;
                if (PageCount > 10)
                {
                    break;
                }
                PagingFooter.Append(delimiter);
                if (CurrentResultPageNumber == page)
                {
                    PagingFooter.Append("<span id=\"pageNavigationSelected\">" + page + "</span>");
                    NextPageNumber = page + 1;
                    delimiter = " | ";
                }
                else
                {
                    PagingFooter.Append(PagingLink.ToString() );
                    if (CurrentResultPageNumber > page)
                    {
                        PagingFooter.Append("&SlidePage=LeftToRight");
                    }
                    PagingFooter.Append("&ResultPage=" + (page - 1).ToString());
                    PagingFooter.Append("\">");
                    PagingFooter.Append(page.ToString());
                    PagingFooter.Append("</a>");
                    delimiter = " | ";
                }
            }
            if (NextPageNumber <= ResultPagesCount)
            {
                PagingFooter.Append(delimiter);
                PagingFooter.Append(PagingLink.ToString());
                PagingFooter.Append("&ResultPage=" + (NextPageNumber - 1).ToString());
                PagingFooter.Append("\">&gt;</a>");
            }
            PagingFooter.Append("</div>");
            if (isiPhone == true)
            {
                PagingFooter.Append("</center>");
            }
        }
        #endregion
    }
    #region  Misc Subroutines

    /// <summary>
    /// Set page sort order.
    /// </summary>
    private void GetSortOrder(bool resetList)
    {
        string sortColumn = string.Empty;
        string sortDESC = string.Empty;
        string prevSortColumn = GetFieldValue("SortColumn", false);
        string prevSortDESC = GetFieldValue("SortDesc", false);
        if (SearchMode == "apoList" || SearchMode == "adrList")
        {
            if (GetFieldValue("sortNumber", false) != string.Empty)
            {
                sortColumn = "ParcelNumber";
                sortByNumber = true;

                isSortOrderChange = true;
                isBreadcrumbRefeshMode = true;
                isBreadcrumbPagingMode = true;
                if (prevSortColumn.Equals(sortColumn) && prevSortDESC == string.Empty)
                {
                    sortDESC = " DESC";
                }
            }
            else if (GetFieldValue("sortAddress", false) != string.Empty)
            {
                sortColumn = "AddressDescription";
                sortByAddress = true;

                isSortOrderChange = true;
                isBreadcrumbRefeshMode = true;
                isBreadcrumbPagingMode = true;
                if (prevSortColumn.Equals(sortColumn) && prevSortDESC == string.Empty)
                {
                    sortDESC = " DESC";
                }
            }
            else if (GetFieldValue("sortName", false) != string.Empty)
            {
                sortColumn = "OwnerName";
                sortByName = true;

                isSortOrderChange = true;
                isBreadcrumbPagingMode = true;
                isBreadcrumbRefeshMode = true;
                if (prevSortColumn.Equals(sortColumn) && prevSortDESC == string.Empty)
                {
                    sortDESC = " DESC";
                }
            }
            else if (prevSortColumn != string.Empty)
            {
                sortColumn = prevSortColumn;
                sortDESC = prevSortDESC;
                if (prevSortColumn == "ParcelNumber")
                {
                    sortByNumber = true;

                }
                else if (prevSortColumn == "OwnerName")
                {
                    sortByName = true;

                }
                else if (prevSortColumn == "AddressDescription")
                {
                    sortByAddress = true;
                }
            }
            if (sortColumn == string.Empty)
            {
                sortDESC = "";
                if (SearchMode == "apoList")
                {
                    if (parcelVisible)
                    {
                        sortColumn = "ParcelNumber";
                        sortOrder = sortColumn + sortDESC + ", OwnerName";
                        sortByNumber = true;
                    }
                    /* Sort by owner name is not supported by GlobalSearchBLL 7.1
                    else if (ownerVisisble)
                    {
                        sortColumn = "OwnerName";
                        sortOrder = sortColumn + sortDESC + ", ParcelNumber";
                        sortByName = true;
                    }
                     */
                    else if (addressVisible)
                    {
                        sortColumn = "AddressDescription";
                        sortOrder = sortColumn + sortDESC + ", OwnerName";
                        sortByAddress = true;
                    }
                    else
                    {
                        sortColumn = "ParcelNumber";
                        sortOrder = sortColumn + sortDESC + ", OwnerName";
                        sortByNumber = true;
                    }
                }
                if (SearchMode == "adrList")
                {
                    if (addressVisible)
                    {
                        sortColumn = "AddressDescription";
                        sortOrder = sortColumn + sortDESC + ", OwnerName";
                        sortByAddress = true;
                    }
                    else if (parcelVisible)
                    {
                        sortColumn = "ParcelNumber";
                        sortOrder = sortColumn + sortDESC + ", OwnerName";
                        sortByNumber = true;
                    }
                    /* Sort by owner name is not supported by GlobalSearchBLL 7.1
                    else if (ownerVisisble)
                    {
                        sortColumn = "OwnerName";
                        sortOrder = sortColumn + sortDESC + ", ParcelNumber";
                        sortByName = true;
                    }
                     */
                    else
                    {
                        sortColumn = "AddressDescription";
                        sortOrder = sortColumn + sortDESC + ", OwnerName";
                        sortByAddress = true;
                    }
                }
            }
            gsSortColumn = sortColumn;
        }
        if (SearchMode == "lpList")
        {
            if (GetFieldValue("sortNumber", false) != string.Empty)
            {
                isSortOrderChange = true;
                sortColumn = "LicenseNumber";
                isBreadcrumbRefeshMode = true;
                isBreadcrumbPagingMode = true;
                if (prevSortColumn.Equals(sortColumn) && prevSortDESC == string.Empty)
                {
                    sortDESC = " DESC";
                }
                sortByNumber = true;
            }
            else if (GetFieldValue("sortType", false) != string.Empty)
            {
                isSortOrderChange = true;
                isBreadcrumbRefeshMode = true;
                isBreadcrumbPagingMode = true;
                sortColumn = "LicenseType";
                if (prevSortColumn.Equals(sortColumn) && prevSortDESC == string.Empty)
                {
                    sortDESC = " DESC";
                }
                sortByType = true;
            }
            else if (GetFieldValue("sortName", false) != string.Empty)
            {
                isSortOrderChange = true;
                isBreadcrumbPagingMode = true;
                isBreadcrumbRefeshMode = true;
                sortColumn = "LicensedProfessionalName";
                if (prevSortColumn.Equals(sortColumn) && prevSortDESC == string.Empty)
                {
                    sortDESC = " DESC";
                }
                sortByName = true;
            }
            else if (GetFieldValue("sortBizName", false) != string.Empty)
            {
                isSortOrderChange = true;
                isBreadcrumbRefeshMode = true;
                isBreadcrumbPagingMode = true;
                sortColumn = "BusinessName";
                if (prevSortColumn.Equals(sortColumn) && prevSortDESC == string.Empty)
                {
                    sortDESC = " DESC";
                }
                sortByBizName = true;
            }
            else
            {
                if (sortColumn == string.Empty)
                {
                    if (prevSortColumn != string.Empty)
                    {
                        sortColumn = prevSortColumn;
                        if (sortColumn == "LicenseNumber")
                            sortByNumber = true;
                        if (sortColumn == "LicenseType")
                            sortByType = true;
                        else if (sortColumn == "LicensedProfessionalName")
                            sortByName = true;
                        else if (sortColumn == "BusinessName")
                            sortByBizName = true;
                        else
                            sortColumn = string.Empty;
                        sortDESC = prevSortDESC;
                    }
                }
                if (sortColumn == string.Empty)
                {
                    sortDESC = "";
                    if (licenseVisible)
                    {
                        sortColumn = "LicenseNumber";
                        sortByNumber = true;
                    }
                    else if (licenseTypeVisible)
                    {
                        sortColumn = "LicenseType";
                        sortByType = true;
                    }
                    else if (licenseNameVisible)
                    {
                        sortColumn = "LicensedProfessionalName";
                        sortByName = true;
                    }
                    else if (businessNameVisible)
                    {
                        sortColumn = "BusinessName";
                        sortByBizName = true;                    }
                    else
                    {
                        sortColumn = "LicenseNumber";
                        sortByNumber = true;
                    }
                }
            }
            gsSortColumn = sortColumn;
            sortOrder = sortColumn + sortDESC;
        }
        if (SearchMode == "capList")
        {
            gsSortDirection = "";
            if (resetList == false)
            {
                if (GetFieldValue("sortNumber", false) != string.Empty)
                {
                    isSortOrderChange = true;
                    isBreadcrumbRefeshMode = true;
                    isBreadcrumbPagingMode = true;
                    sortColumn = "AltID";
                    gsSortColumn = "PermitNumber";
                    //gsSortColumn = "altId";
                    if (prevSortColumn.Equals(sortColumn) && prevSortDESC == string.Empty)
                    {
                        sortDESC = " DESC";
                    }
                    sortByNumber = true;
                    sortOrder = sortColumn + sortDESC;
                }
                else if (GetFieldValue("sortStatus", false) != string.Empty)
                {
                    isSortOrderChange = true;
                    isBreadcrumbRefeshMode = true;
                    isBreadcrumbPagingMode = true;
                    sortColumn = "Status";
                    gsSortColumn = "Status";
                    if (prevSortColumn.Equals(sortColumn) && prevSortDESC == string.Empty)
                    {
                        sortDESC = " DESC";
                    }
                    sortByStatus = true;
                    sortOrder = sortColumn + sortDESC + ", sortDate DESC, Number";
                }
                else if (GetFieldValue("sortModule", false) != string.Empty)
                {
                    isSortOrderChange = true;
                    isBreadcrumbRefeshMode = true;
                    isBreadcrumbPagingMode = true;
                    sortColumn = "Module";
                    gsSortColumn = "ModuleName";
                    if (prevSortColumn.Equals(sortColumn) && prevSortDESC == string.Empty)
                    {
                        sortDESC = " DESC";
                    }
                    sortByModule = true;
                    sortOrder = sortColumn + sortDESC + ", Date DESC, Number DESC";
                }
                else if (GetFieldValue("sortAgency", false) != string.Empty)
                {
                    isSortOrderChange = true;
                    isBreadcrumbRefeshMode = true;
                    isBreadcrumbPagingMode = true;
                    sortColumn = "Agency";
                    gsSortColumn = "AgencyCode";
                    if (prevSortColumn.Equals(sortColumn) && prevSortDESC == string.Empty)
                    {
                        sortDESC = " DESC";
                    }
                    sortByAgency = true;
                    sortOrder = sortColumn + sortDESC + ", Date DESC, Number DESC";
                }
                else if (GetFieldValue("sortDate", false) != string.Empty)
                {
                    isSortOrderChange = true;
                    isBreadcrumbRefeshMode = true;
                    isBreadcrumbPagingMode = true;
                    sortColumn = "sortDate";
                    gsSortColumn = "CreatedDate";
                    if (prevSortColumn.Equals(sortColumn) && prevSortDESC == string.Empty)
                    {
                        sortDESC = " DESC";
                    }
                    sortByDate = true;
                    sortOrder = sortColumn + sortDESC + ", Number";
                }
                if (sortColumn == string.Empty)
                {
                    if (prevSortColumn != string.Empty)
                    {
                        sortColumn = prevSortColumn;
                        if (sortColumn == "sortDate")
                        {
                            sortByDate = true;
                            gsSortColumn = "CreatedDate";
                        }
                        else if (sortColumn == "AltID")
                        {
                            sortByNumber = true;
                            gsSortColumn = "PermitNumber";
                        }
                        else if (sortColumn == "Status")
                        {
                            sortByStatus = true;
                            gsSortColumn = "Status";
                        }
                        else if (sortColumn == "Module")
                        {
                            sortByModule = true;
                            gsSortColumn = "ModuleName";
                        }
                        else if (sortColumn == "Agency")
                        {
                            sortByAgency = true;
                            gsSortColumn = "AgencyCode";
                        }
                        else
                        {
                            sortColumn = string.Empty;
                        }
                        sortDESC = prevSortDESC;
                    }
                    else
                    {
                        sortColumn = "sortDate";
                        gsSortColumn = "CreatedDate";
                        sortDESC = " DESC";
                        sortByDate = true;
                    }
                    sortOrder = sortColumn + sortDESC; // +", Number";
                }
            }
            else
            {
                sortColumn = "sortDate";
                gsSortColumn = "CreatedDate";
                sortDESC = " DESC";
                sortByDate = true;
                sortOrder = sortColumn + sortDESC; // +", Number";
            }
        }
        // Update Sort Order Session variables
        sortDescending = sortDESC == " DESC";
        if (sortDescending)
        {
            gsSortDirection = ACAConstant.ORDER_BY_DESC;
        }
        else
        {
            gsSortDirection = ACAConstant.ORDER_BY_ASC;
        }
        SortColumn = sortColumn;
        SortDesc = sortDESC;
        ListSortColumn = sortColumn;
        ListSortDesc = sortDESC;
        HiddenFields.Append(HTML.PresentHiddenField("SortColumn", sortColumn));
        HiddenFields.Append(HTML.PresentHiddenField("SortDesc", sortDESC));
    }
    /// <summary>
    /// Get Field value from Form, Request, or QueryString.
    /// </summary>
    /// <param name="FieldName"> Field name </param>
    /// <param name="IsRequired"> Indcates if the user must enterer data into the field or not</param>
    /// <returns>String</returns>
    // DWB - 07-18-2008 - Added new funciton.
    private string GetFieldValue(string FieldName, bool IsRequired)
    {
        string TheValue = string.Empty;
        try
        {
            TheValue = (Request.QueryString[FieldName] != null)
                   ? Request.QueryString[FieldName] : ((Request[FieldName] != null)
                   ? Request.Form[FieldName].ToString() : string.Empty);
        }
        catch
        {
            TheValue = string.Empty;
        }
        if (IsRequired == true && TheValue.ToString() == "")
        {
            // IsRequiredDataValid = false;
        }
        return TheValue;
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
    /// Check to see if user has selected CAPS for a collection action.
    /// </summary>
    private void CheckForCollectionActionRequest()
    {
        capsInList = GetFieldValue("capsInList", false);
        string MyCollectionOperation = string.Empty;
        SimpleCapModel[] caps = null;
        int capListSize = 0;

        if (int.TryParse(capsInList, out capListSize))
        {
            int rowCnt = 0;
            for (int aRow = 0; aRow < capListSize; aRow++)
            {
                string aCap = Request.Form["Row" + aRow.ToString()] != null ? Request.Form["Row" + aRow.ToString()].ToString() : string.Empty;
                if (aCap != string.Empty)
                {
                    rowCnt++;
                }
            }
            if (rowCnt != 0)
            {
                caps = new SimpleCapModel[rowCnt];
                rowCnt = 0;
                for (int aRow = 0; aRow < capListSize; aRow++)
                {
                    string aCap = Request.Form["Row" + aRow.ToString()] != null ? Request.Form["Row" + aRow.ToString()].ToString() : string.Empty;
                    if (aCap != string.Empty)
                    {
                        string[] capDetail = aCap.Split('|');
                        SimpleCapModel capModel = new SimpleCapModel();
                        capModel.capID = new CapIDModel();
                        capModel.capID.ID1 = (capDetail[0] != null) ? capDetail[0] : string.Empty;
                        capModel.capID.ID2 = (capDetail[1] != null) ? capDetail[1] : string.Empty;
                        capModel.capID.ID3 = (capDetail[2] != null) ? capDetail[2] : string.Empty;
                        capModel.altID = (capDetail[3] != null) ? capDetail[3] : string.Empty;
                        capModel.capID.serviceProviderCode = (capDetail[4] != null) ? capDetail[4] : string.Empty;
                        capModel.moduleName = (capDetail[5] != null) ? capDetail[5] : string.Empty;
                        capModel.capClass = (capDetail[6] != null && capDetail[6] != "none") ? capDetail[6] : string.Empty;
                        caps[rowCnt] = capModel;
                        rowCnt++;
                    }
                }
            }
        }
        if (GetFieldValue("submitAdd", false) != string.Empty)
        {
            MyCollectionOperation = "Add";
        }
        else if (GetFieldValue("submitCopy", false) != string.Empty)
        {
            MyCollectionOperation = "Copy";
        }
        else if (GetFieldValue("submitMove", false) != string.Empty)
        {
            MyCollectionOperation = "Move";
        }
        else if (GetFieldValue("submitRemove", false) != string.Empty)
        {
            MyCollectionOperation = "Remove";
        }
        if (MyCollectionOperation != string.Empty)
        {
            if (caps == null)
            {
                isBreadcrumbRefeshMode = true;
                ErrorMessage.Append(ErrorFormat);
                ErrorMessage.Append(LocalGetTextByKey("mycollection_caphomepage_message_nocapselected"));
                ErrorMessage.Append(ErrorFormatEnd);
            }
            else
            {
                Session["MyCollection_SelectedCaps"] = caps;
                Response.Redirect("MyCollections.Update.aspx?State=" + State
                    + "&Module=" + ModuleName
                    + "&CollectionOperation=" + MyCollectionOperation
                    + "&GlobalSearchMode=" + SearchMode
                    + "&PageBreadcrumbIndex=" + PageBreadcrumbIndex.ToString()
                    + "&SortColumn=" + GetFieldValue("SortColumn", false)
                    + "&SortDesc=" + GetFieldValue("SortDesc", false)
                    + "&ResultPage=" + (CurrentResultPageNumber - 1).ToString());
            }
        }
    }
    /// <summary>
    /// get field visible
    /// </summary>
    /// <param name="models">simple view element modles</param>
    /// <param name="fieldName">field name</param>
    /// <returns>true if field is visible,otherwise,it's false.</returns>
    public static bool IsFieldVisible(SimpleViewElementModel4WS[] models, string fieldName)
    {
        if (models == null ||
            models.Length == 0)
        {
            return true;
        }

        foreach (SimpleViewElementModel4WS model in models)
        {
            if (model.viewElementName == fieldName)
            {
                return model.recStatus == ACAConstant.VALID_STATUS;
            }
        }

        return true;
    }

    #endregion
}
