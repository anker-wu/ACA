/**
* <pre>
* 
*  Accela Citizen Access
*  File: AdvancedSearchResults.aspx.cs
* 
*  Accela, Inc.
*  Copyright (C): 2007-2013
* 
*  Description:
*  VO for Additional Information object. 
* 
*  Notes:
*      $Id: AdvancedSearchResults.aspx.cs 77905 2007-10-15 12:49:28Z ACHIEVO\Jackie.Yu $.
*  Revision History
*  &lt;Date&gt;,		&lt;Who&gt;,			&lt;What&gt;
*  10-01-2008           Dave Brewster           Modified list format for Mobile ACA 6.7.0 redesign.  Added alternating 
*                                               background colors in list items, changed font sizes, and element spacing.
*  11-16-2008           Dave Brewster           Added relates permits search mode.
*  04/01/2009           Dave Brewster           Modified to display AltID instead of the three segment CAP id.
*  12/03/2009           Dave Brewster           Corrected the filter by wild card logic that was disabling cross
*                                               module search loginc
*                                               And, corrected sort order for My Permits list to match ACA.
*                                               And, corrected code to exclude "Add to Collection" link from related records list.
*                                               
* </pre>
*/
using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Collections.Generic;
//using System.Web;
//using System.Web.Security;
//using System.Web.UI;
//using System.Web.UI.WebControls;
//using System.Web.UI.WebControls.WebParts;
//using System.Web.UI.HtmlControls;
using System.Text;
using Accela.ACA.BLL.MyCollection;
using Accela.ACA.Common;
using Accela.ACA.Web.Common;
using Accela.ACA.WSProxy;


/// <summary>
/// 
/// </summary>
public partial class AdvancedSearchResults : AccelaPage
{
    private const string HTML_EMPTY = "&nbsp;"; 
    public StringBuilder ResultHeader = new StringBuilder();
    public StringBuilder PagingFooter = new StringBuilder();
    public string SearchMode = string.Empty;
    public StringBuilder ResultOutput = new StringBuilder();
    public StringBuilder PagingFields = new StringBuilder();
    public StringBuilder SortOptions = new StringBuilder();
    public StringBuilder CollectionLinks = new StringBuilder();
    public string Filter = string.Empty;
    public string DisplayMode = string.Empty;
    public StringBuilder ErrorMessage = new StringBuilder();
    public StringBuilder Breadcrumbs = new StringBuilder();
    public StringBuilder FilterLink = new StringBuilder();
    public StringBuilder HiddenFields = new StringBuilder();
    // public string CollectionModuleName = string.Empty;

    public string BackForwardLinks = string.Empty;
    public string ListMode = string.Empty;
    public string PageTitle = string.Empty;
    public string capsInList = string.Empty;
    private string SearchDescripton = string.Empty;
    private int CurrentResultPageNumber = 1;
    public string PageResultPageNo = string.Empty;

    public string ListSortColumn = string.Empty;
    public string ListSortDesc = string.Empty;
    private string sortOrder = string.Empty;
    private bool sortByDate = false;
    private bool sortByNumber = false;
    private bool sortByAgency = false;
    private bool sortByStatus = false;
    private bool sortByModule = false;
    private bool sortDescending = false;
    private bool sortDefault = false;
    private bool isDefaultSearchOrder = false;
    private bool isCrossModuleEnabled = false;
    // private bool isCurrentModuleOnly = true;
    public const string HTML_NBSP = "&nbsp;";

    /// <summary>
    /// Advanced Search Options
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    //private string ErrorFormat = "<div style=\"margin-top:-10;\"><table cellpadding=\"1\" cellspacing=\"0\" width=\"95%\"><tr><td valign=\"top\"><img src=\"img\\error.png\"/></td><td style=\"color:#FF6600; font-weight:bold;\">";
    //private string ErrorFormatEnd = "</div></td></tr></table>";
    public string PageBreadcrumbIndex = string.Empty;
    private bool isBreadcrumbPagingMode = false;
    private bool isBreadcrumbRefeshMode = false;
    private string FilterType = string.Empty;
    private string FilterMask = string.Empty;
    private string viewModuleName = string.Empty;
    private string viewBaseModuleName = string.Empty;
    private bool isAllModuleView = false;

    protected void Page_Load(object sender, EventArgs e)
    {
        ValidationChecks("AdvancedSearch.Results.aspx");
      
        // default page number
        int ResultCount = 0;

        //number of records shown per page - from web.config
        AppSettingsReader Settings = new AppSettingsReader(); 
        int ResultRecordsPerPage = int.Parse(Settings.GetValue("SearchRecordsPerPage", typeof(string)).ToString());
        string RelatedPermitsPaging = string.Empty;
        string mycollection_managepage_label_name = StripHTMLFromLabelText(LocalGetTextByKey("mycollection_collectionmanagement_collectionname"), "Collections");

        //Current list sort settings.
        string sortColumn = GetFieldValue("SortColumn", false);
        string sortDesc = GetFieldValue("SortDesc", false);
        isDefaultSearchOrder = GetFieldValue("NewSearch", false).ToString() == "true";

        string moduleLabelText = GetFieldValue("ModuleLabel", false);
        string tempTest = ModuleName;
        //Current breadcrumb and pageing status.
        StringBuilder sbWork = new StringBuilder();
        PageBreadcrumbIndex = GetFieldValue("PageBreadcrumbIndex", false);
        bool isElipseLink = GetFieldValue("IsElipseLink", false) != string.Empty;
        string breadCrumbIndex = GetFieldValue("BreadCrumbIndex", false);
        if (breadCrumbIndex == string.Empty)
        {
            breadCrumbIndex = PageBreadcrumbIndex;
        }
        isBreadcrumbPagingMode = GetFieldValue("PagingMode", false) == "true";

        // Get the current result list page.
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
        SearchMode = GetFieldValue("Mode", false);
        Filter = (GetFieldValue("Status", false) != string.Empty) ? GetFieldValue("Status", false) : GetFieldValue("InspectionStatus", false);

        //Check global search mode or my permits filtering mode and search pattern
        bool resetList = GetFieldValue("ReloadList", false) == "true";
        bool clearFilter = GetFieldValue("ClearFilter", false) == "true";
        bool isModuleNameChange = false;
        if (SearchMode == "View Permits")
        {
            viewModuleName = AdvancedSearchListTitle;
            viewBaseModuleName = AdvancedSearchListTitleBaseModule;
            isModuleNameChange = (moduleLabelText != string.Empty) || (GetFieldValue("ViewBaseModuleName", false) != viewBaseModuleName);
            isAllModuleView = GetFieldValue("isAllModuleView", false) == "true";
            // This code is used to make sure that the current CAP list stored in a session variable
            // is valid for a previous search request that the user has selected.  This allows the user
            // to use the browser back button to page through the browser page history, choose a 
            // previous cap list page and use the links on that page to resume browsing the list
            // of caps.
            // It also allows the user to pick prevous search result list from the breadcrumb tree
            // and resume browsing the list of CAPs.
            if (Session["AMCA_CapList_DataTable"] != null && resetList == false)
            {
                if (ModuleName == null || Session["AMCA_CapList_ModuleName"].ToString() != ModuleName)
                {
                    resetList = true;
                    isModuleNameChange = true;
                    isBreadcrumbRefeshMode = false;
                }
            }
        }
        string listFilter = string.Empty;
        if (resetList == true)
        {
            CurrentResultPageNumber = 1;
        }
        if (clearFilter)
        {
            isBreadcrumbRefeshMode = true;
            isBreadcrumbPagingMode = true;
        }
        else
        {
            FilterType = GetFieldValue("FilterType", false);
            FilterMask = GetFieldValue("FilterMask", false);
            if (FilterMask.ToString() != string.Empty)
            {
                if (FilterType.ToString() == "Contains")
                {
                    listFilter = "%" + FilterMask + "%";
                }
                else if (FilterType.ToString() == "EndsWith")
                {
                    listFilter = "%" + FilterMask;
                }
                else if (FilterType.ToString() == "StartsWith")
                {
                    listFilter = FilterMask + "%";
                }
                else
                {
                    listFilter = FilterMask;
                }
            }
        }
        HiddenFields.Append(HTML.PresentHiddenField("FilterType", FilterType));
        HiddenFields.Append(HTML.PresentHiddenField("FilterMask", FilterMask));

        SearchDescripton = SearchMode;
        bool isSuperAgency = StandardChoiceUtil.IsSuperAgency();
        DataTable dtCapList = null;

        List<string> selectedModules = null;
        if (SearchMode == "View Permits")
        {
            CheckForCollectionActionRequest();
            // moduleLableText is the QUI_TEXT key to the user defined description
            // for the current module. It will only have a value when the user has
            // selected a module from the HOME page.
            if (isModuleNameChange == true || moduleLabelText != string.Empty)
            {
                viewBaseModuleName = ModuleName;
                string text = StripHTMLFromLabelText(LabelUtil.GetTextByKey(moduleLabelText, ModuleName), ModuleName);
                if (string.IsNullOrEmpty(text) || text == ACAConstant.DEFAULT_MODULE_NAME)
                {
                    text = ModuleName;
                }
                viewModuleName = text;
                Session["AMCA_AdvancedSearch_Result_List_Name"] = viewModuleName;
                Session["AMCA_AdvancedSearch_Result_List_BaseModule"] = viewBaseModuleName;
            }
            AccelaProxy accelaProxy = new AccelaProxy();
            if (accelaProxy.isCrossModuleSearch() == true)
            {
                selectedModules = new List<string>();
                IList<string> selectedModuleList = TabUtil.GetCrossModules(ModuleName);
                if (selectedModuleList != null && selectedModuleList.Count != 0)
                {
                    isCrossModuleEnabled = true;
                    foreach (string aModule in selectedModuleList)
                    {
                        selectedModules.Add(aModule);
                    }
                }
                // Switch between current module list and Cross Module list
                if (isModuleNameChange == true) 
                {
                    resetList = true;
                    isAllModuleView = false;
                }
                else
                {
                    // set the current list mode as by module or by cross module
                    if (isAllModuleView == true && GetFieldValue("SwitchToCurrentModuleList", false) == "true")
                    {
                        // set current module only mode.
                        resetList = true;
                        isAllModuleView = false;
                        isBreadcrumbPagingMode = true;
                        isBreadcrumbRefeshMode = true;
                    }
                    // check if the user selected cross module list mode
                    if (isAllModuleView == false && GetFieldValue("SwitchToCrossModuleList", false) == "true")
                    {
                        // set list to cross module mode
                        resetList = true;
                        isAllModuleView = true;
                        isBreadcrumbPagingMode = true;
                        isBreadcrumbRefeshMode = true;
                    }
                }
            }
        }
        GetSortOrder(resetList, isDefaultSearchOrder);
        // Store values in hidden fields for paging mode 
        HiddenFields.Append(HTML.PresentHiddenField("SortColumn", sortColumn));
        HiddenFields.Append(HTML.PresentHiddenField("SortDesc", sortDesc));
        HiddenFields.Append(HTML.PresentHiddenField("ViewBaseModuleName", viewBaseModuleName));
        HiddenFields.Append(HTML.PresentHiddenField("isAllModuleView", isAllModuleView == true ? "true" : "false"));

        #region Search
        //check for permit / inspection
        string linkHTML = "<a style=\"color:#040478;  text-decoration:underline;\" href=\"";
        string linkStyle = string.Empty;

        if (SearchMode != "My Inspections") 
        {
            // DWB - 02-24-2008 - Added this to show the new "View Permits" option added for 2008 redesign.
            if (SearchMode == "View Permits")
                DisplayMode = "View Permits";
            else
                DisplayMode = linkHTML + "Search.Home.aspx?State=" + State + "\" title=\"Search\" > Search</a> > Permit";

             //Get Result List
            Permit[] Results;
            #region Different SEARCH MODE
            switch (SearchMode)
            { 
                case "Permit":
                    // DWB - this search is no longer used in AMCA
                    Permit permits = new Permit();
                    permits.Number = (Request.QueryString["PermitNumber"] != null) ? Request.QueryString["PermitNumber"] : ((Request.Form["PermitNumber"] != null) ? Request.Form["PermitNumber"] : string.Empty);
                    permits.Type = (Request.QueryString["PermitType"] != null) ? Request.QueryString["PermitType"] : ((Request.Form["PermitType"] != null) ? Request.Form["PermitType"] : string.Empty);
                    permits.Status = (Request.QueryString["PermitStatus"] != null) ? Request.QueryString["PermitStatus"] : ((Request.Form["PermitStatus"] != null) ? Request.Form["PermitStatus"] : string.Empty);
                    DateTime fromDate;
                    DateTime toDate;
                    //if (permits.Number != "")
                    //{
                        string[] dates = this.cboDate.Split(',');
                        string[] months = this.cboMonth.Split(',');
                        string[] years = this.cboYear.Split(',');
                        if (!String.IsNullOrEmpty(cboDate + cboMonth + cboYear))
                        {
                            string dateFrom = months[0] + "/" + dates[0] + "/" + years[0];
                            string dateTo = months[1] + "/" + dates[1] + "/" + years[1];
                            DateTime.TryParse(dateFrom, out fromDate);
                            DateTime.TryParse(dateTo, out toDate);
                        }                
                        else
                        {
                            fromDate = Convert.ToDateTime(System.DateTime.Now.ToShortDateString());
                            toDate = Convert.ToDateTime(System.DateTime.Now.ToShortDateString());
                        }
                    //}
                    Results = MyProxy.PermitSearch(permits.Number, permits.Type, permits.Status, fromDate, toDate);
                    break;
                case "Related Permits":
                    string PermitNumber = string.Empty;
                    string AltID = string.Empty;
                    string PermitType = string.Empty;
                    string SearchBy = string.Empty;
                    string SearchValue = string.Empty;
                    string Mode = string.Empty;
                    string ViewPermitPageNo = string.Empty;

                    PermitType = (Request.QueryString["PermitType"] != null) ? Request.QueryString["PermitType"] : (Request.Form["PermitType"] != null ? Request.Form["PermitType"].ToString() : string.Empty);
                    PermitNumber = (Request.QueryString["PermitNumber"] != null) ? Request.QueryString["PermitNumber"] : (Request.Form["PermitNumber"] != null ? Request.Form["PermitNumber"].ToString() : string.Empty);
                    AltID = (Request.QueryString["AltID"] != null) ? Request.QueryString["AltID"] : PermitNumber;
                    SearchBy = (Request.QueryString["SearchBy"] != null) ? Request.QueryString["SearchBy"] : (Request.Form["SearchBy"] != null ? Request.Form["SearchBy"].ToString() : string.Empty);
                    Mode = (Request.QueryString["Mode"] != null) ? Request.QueryString["Mode"] : (Request.Form["Mode"] != null ? Request.Form["Mode"].ToString() : string.Empty);
                    ViewPermitPageNo = (Request.QueryString["ViewPermitPageNo"] != null) ? Request.QueryString["ViewPermitPageNo"] : (Request.Form["ViewPermitPageNo"] != null ? Request.Form["ViewPermitPageNo"].ToString() : string.Empty);

                    ListMode = "Related Permits";
                    
                    sbWork.Append("&Mode=" + SearchMode);
                    sbWork.Append("&Module=" + ModuleName);
                    sbWork.Append("&PermitNumber=" + PermitNumber);
                    sbWork.Append("&Permittype=" + PermitType);
                    sbWork.Append("&AltID=" + AltID);
                    sbWork.Append("&SearchBy=" + SearchBy);
                    sbWork.Append("&ResultPage=" + (CurrentResultPageNumber - 1).ToString());
                    sbWork.Append("&ViewPermitPageNo=" + ViewPermitPageNo.ToString());
                    if (isGlobalSearchMode == true)
                    {
                        sbWork.Append("&GlobalSearchMode=true");
                    }
                    iPhonePageTitle = ListMode;
                    if (isiPhone == false)
                    {
                        PageTitle = "<div id=\"pageTitle\">" + ListMode + FilterLink + "</div>";
                    }

                    Results = MyProxy.RelatedPermitSearch(PermitNumber, PermitType);

                    Breadcrumbs = BreadCrumbHelper("AdvancedSearch.Results.aspx", sbWork, ListMode, breadCrumbIndex, isElipseLink, isBreadcrumbRefeshMode, isBreadcrumbPagingMode, false);
                    PageBreadcrumbIndex = CurrentBreadCrumbIndex.ToString();

                    iPhoneHideFooterBar = (Results == null || Results.Length == 0);
                    BackForwardLinks = BackLinkHelper(CurrentBreadCrumbIndex.ToString());
                    iPhoneHideFooterBar = false;

                    RelatedPermitsPaging = "&PermitType=" + PermitType
                        + "&PermitNumber=" + PermitNumber
                        + "&Module=" + ModuleName
                        + "&AltID=" + AltID
                        + "&SearchBy=" + SearchBy
                        + "&ViewPermitPageNo=" + ViewPermitPageNo;
                    break;
                default:

                    ListMode = viewModuleName;
                    Permit aPermit = new Permit();
                    if (listFilter != string.Empty || ModuleName != null)
                        {
                        if (Session["AMCA_CapList_DataTable"] != null && resetList == false)
                        {
                            dtCapList = (DataTable)Session["AMCA_CapList_DataTable"];
                        }
                        if (dtCapList == null || dtCapList.Rows.Count == 0)
                        {
                            aPermit.Number = string.Empty; //listFilter;
                            aPermit.Module = ModuleName;
                            if (!isAllModuleView) // (isCurrentModuleOnly)
                            {
                                dtCapList = MyProxy.MyPermitSearch(aPermit, null);
                            }
                            else
                            {
                                dtCapList = MyProxy.MyPermitSearch(aPermit, selectedModules);
                            }
                            if (sortDefault == false && dtCapList != null && sortOrder.Length != 0)
                            {
                                dtCapList.DefaultView.Sort = sortOrder ;
                            }
                        }
                    }
                    else
                    {
                        dtCapList = MyProxy.MyPermitSearch(aPermit, selectedModules);
                    }
                    string parenLeft = " (";
                    string parenRight = ")";
                    string separator = "|";

                    if (isiPhone == true)
                    {
                        separator = "";
                        parenLeft = string.Empty;
                        parenRight = string.Empty;
                    }
                    linkStyle = "<a id=\"pageTitleButton\" href=\"";
                    if (dtCapList != null && dtCapList.Rows.Count != 0)
                    {
                        if (listFilter == string.Empty)
                        {
                            FilterLink.Append(parenLeft + linkStyle + "AdvancedSearch.Filter.aspx?State=" + State);
                        }
                        else
                        {
                            FilterLink.Append(parenLeft + linkStyle + "AdvancedSearch.Results.aspx?State=" + State);
                        }
                        FilterLink.Append("&PageBreadcrumbIndex=" + (CurrentBreadCrumbIndex).ToString());
                        FilterLink.Append("&Mode=" + SearchMode);
                        FilterLink.Append("&Module=" + ModuleName);
                        FilterLink.Append("&ResultPage=" + (CurrentResultPageNumber - 1).ToString());
                        FilterLink.Append("&isAllModuleView=" + (isAllModuleView == true ? "true" : "false"));
                        FilterLink.Append("&ViewBaseModuleName=" + viewBaseModuleName);
                        if (listFilter == string.Empty)
                        {
                            FilterLink.Append("\">Filter Results</a>");
                        }
                        else
                        {
                            FilterLink.Append("&ClearFilter=true");
                            FilterLink.Append("\">Clear Filter</a>");
                        }
                        if (isCrossModuleEnabled)
                        {
                            FilterLink.Append(HTML_NBSP);
                            FilterLink.Append(separator);
                            FilterLink.Append(HTML_NBSP + linkStyle + "AdvancedSearch.Results.aspx?State=" + State);
                            FilterLink.Append("&Mode=" + SearchMode);
                            FilterLink.Append("&Module=" + ModuleName);
                            FilterLink.Append("&ResultPage=" + (CurrentResultPageNumber - 1).ToString());
                            FilterLink.Append("&ReloadList=true");
                            FilterLink.Append("&ViewBaseModuleName=" + viewBaseModuleName);
                            FilterLink.Append("&PageBreadcrumbIndex=" + (CurrentBreadCrumbIndex + 1).ToString());
                            FilterLink.Append("&isAllModuleView=" + (isAllModuleView == true ? "true" : "false"));
                            if (isAllModuleView == true)
                            {
                                FilterLink.Append("&SwitchToCurrentModuleList=true");
                                FilterLink.Append("\">" + viewModuleName + " Only</a>");
                            }
                            else
                            {
                                FilterLink.Append("&SwitchToCrossModuleList=true");
                                FilterLink.Append("\">All Modules</a>");
                            }
                        }
                        FilterLink.Append(parenRight);

                        linkStyle = "<a style=\"color:#040478; font-size:10pt; text-decoration:underline;\" href=\"";
                        if (sortDefault == false && sortOrder.Length != 0 && sortOrder.Equals(dtCapList.DefaultView.Sort.ToString()) == false)
                        {
                            dtCapList.DefaultView.Sort = sortOrder;
                        }
                        string rowFilter = string.Empty;
                        //if (isCurrentModuleOnly == true && selectedModules != null)
                        if (isAllModuleView != true && selectedModules != null)
                        {
                            rowFilter = "Module = '" + ModuleName + "'";
                        }
                        if (rowFilter != string.Empty && listFilter.Length != 0)
                        {
                            rowFilter = rowFilter + " AND ";
                        }
                        if (listFilter.Length != 0)
                        {
                            rowFilter = rowFilter + "Alias LIKE '" + listFilter + "'";
                        }
                        if (rowFilter.Length != 0 && !rowFilter.Equals(dtCapList.DefaultView.RowFilter.ToString()))
                        {
                            try
                            {
                                dtCapList.DefaultView.RowFilter = rowFilter;
                            }
                            catch (Exception ex)
                            {
                                Session["ACMA_FilterError"] = ex.Message;
                                Response.Redirect("AdvancedSearch.Filter.aspx?State=" + State
                                    + "&Mode=" + SearchMode
                                    + "&Module=" + ModuleName
                                    + "&ResultPage=" + (CurrentResultPageNumber - 1).ToString()
                                    + "&SortColumn=" + SortColumn.ToString()
                                    + "&SortDesc=" + SortDesc.ToString()
                                    + "&FilterError=true");
                            }
                        }
                        Results = ConvertUtil.convertCapListDatatableToPermit(dtCapList);
                    }
                    else
                    {
                        Results = null;
                    }
                    Session["AMCA_CapList_DataTable"] = dtCapList;
                    Session["AMCA_CapList_ModuleName"] = ModuleName;
                    sbWork.Append("&Module=" + ModuleName);
                    sbWork.Append("&Mode=" + SearchMode);
                    sbWork.Append("&ResultPage=" + (CurrentResultPageNumber - 1).ToString());
                    sbWork.Append("&FilterType=" + FilterType);
                    sbWork.Append("&FilterMask=" + FilterMask);
                    sbWork.Append("&ViewBaseModuleName=" + viewBaseModuleName);
                    sbWork.Append("&isAllModuleView=" + (isAllModuleView == true ? "true" : "false"));
                    iPhonePageTitle = ListMode;
                    Breadcrumbs = BreadCrumbHelper("AdvancedSearch.Results.aspx", sbWork, viewBaseModuleName + "|" + viewModuleName, breadCrumbIndex, isElipseLink, isBreadcrumbRefeshMode, isBreadcrumbPagingMode, false);
                    PageBreadcrumbIndex = CurrentBreadCrumbIndex.ToString();

                    iPhoneHideFooterBar = (Results == null || Results.Length == 0);
                    BackForwardLinks = BackLinkHelper(CurrentBreadCrumbIndex.ToString());
                    iPhoneHideFooterBar = false;

                    if (isiPhone == false)
                    {
                        PageTitle = "<div id=\"pageTitle\">" + ListMode + FilterLink + "</div>";
                    }
                    else
                    {
                        PageTitle = "<div id=\"pageTitle\">Records</div>";
                    }
                    DisplayMode = "View Permits";
                    linkStyle = "<a style=\"color:#040478;  text-decoration:underline; font-size:10pt;\" href=\"";
                    SearchDescripton = ModuleName;
                    break;
             }
             if (Results == null || (dtCapList != null && dtCapList.DefaultView.Count == 0))
             {
                 ResultOutput.Append("<br><i>No records were found.");
                 if (dtCapList != null && dtCapList.Rows.Count != 0)
                 {
                     ResultOutput.Append("<br> Select 'Clear Filter' to view entries from other modules.");
                 }
                 ResultOutput.Append("</i><br>");
             }
             else if (MyProxy.OnErrorReturn)
             {  // Proxy Exception 
                 ErrorMessage.Append(MyProxy.ExceptionMessage);
             }
            #endregion

            #region  Permit Search
            
            if (Results != null)
            {
                ResultCount = Results.Length;
            }
            int ResultPageStart = (CurrentResultPageNumber -1) * ResultRecordsPerPage;
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
                ResultHeader.Append("<div id=\"pageText\">" + "Showing ");
                ResultHeader.Append(FirstRecord.ToString() + "-");
                ResultHeader.Append(LastRecord.ToString() + " of " + ResultCount.ToString());
                ResultHeader.Append(ResultCount == 100 ? "+" : string.Empty);
                if (isiPhone == true)
                {
                    ResultHeader.Append(FilterLink.ToString());
                }
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
                    ResultOutput.Append("<table width=\"95%\" cellspacing=\"0px\" cellpadding=\"5px\"  border=\"1px\" style=\"border-right-width:0px; border-left-width:0px; border-top-width:0px; border-bottom-width:0px; border-color:#e6e6e6;\" >");
                }
                string checkBox = string.Empty;
                int rowNum = 0;

                for (int x = ResultPageStart; x < (((ResultPageStart + ResultRecordsPerPage) < ResultCount) ? (ResultPageStart + ResultRecordsPerPage) : ResultCount); x++)
                {
                    string tableCellHTML = "<td width=\"40px\" valign=\"top\" style=\"border-right-width:0px; border-left-width:0px; border-top-width:0px; border-bottom-width:1px; border-color:#e6e6e6;";
                    string rowColor = string.Empty;
                    string endHTML = "</td>";
                    if (isiPhone == true)
                    {
                        tableCellHTML = string.Empty;
                        endHTML = string.Empty;
                    }
                    else
                    {
                        if (oddRow == true && oodRowBackcolor != string.Empty)
                        {
                            oddRow = false;
                            rowColor = " background-color:" + oodRowBackcolor + ";\">";
                        }
                        else
                        {
                            oddRow = true;
                            rowColor = tableCellHTML + "\">";
                        }
                    }
                    if (SearchMode == "View Permits")
                    {
                        checkBox = tableCellHTML + rowColor + "<input id=\"Row" + rowNum.ToString() + "\" type=\"checkbox\""
                            + " name=\"Row" + rowNum.ToString() + "\""
                            + " value=\""
                            + ((Results[x].capId1 == null) ? string.Empty : Results[x].capId1)
                            + "|"
                            + ((Results[x].capId2 == null) ? string.Empty : Results[x].capId2)
                            + "|"
                            + ((Results[x].capId3 == null) ? string.Empty : Results[x].capId3)
                            + "|"
                            + ((Results[x].Alias != null) ? Results[x].Alias.ToString() : Results[x].Number.ToString())
                            + "|"
                            + ((Results[x].Agency == null || Results[x].Agency != null) ? Results[x].Agency.ToString() : string.Empty)
                            + "|"
                            + ((Results[x].Module != null) ? Results[x].Module.ToString() : string.Empty)
                            + "|"
                            + ((Results[x].capClass != null) ? Results[x].capClass.ToString() : string.Empty)
                            + "\"/>" + endHTML;
                    }
                    rowNum++;
                    tableCellHTML = "<td width=\"100%\" valign=\"top\" style=\"text-align:left; border-right-width:0px; border-left-width:0px; border-top-width:0px; border-bottom-width:1px; border-color:#e6e6e6;" + rowColor;
                    string permitDescription = (Results[x].Desc != null) ? Results[x].Desc : string.Empty;
                    //if (permitDescription.Length > 30)
                    //{
                    //    permitDescription = permitDescription.Substring(0, 25) + HTML.PresentLink("View.Details.aspx?State=" + State + "&Type=Permit&Id=" + Results[x].Number, "...");
                    //}
                    StringBuilder cellWork = new StringBuilder();
                    string theDash = "<br>";
                    if (isiPhone == false)
                    {
                        // theDash = " - ";
                        cellWork.Append("<tr style=\"height:40px;\">" + checkBox + tableCellHTML);
                    }
                    if (String.IsNullOrEmpty(Results[x].capClass) || Results[x].capClass == ACAConstant.COMPLETED)
                    {
                        cellWork.Append("<a class=\"pageListLink\" href=\"");
                        cellWork.Append("Permits.View.aspx?State=" + State.ToString());
                        cellWork.Append("&PermitType=" + ((Results[x].Type == null) ? string.Empty : Results[x].Type));
                        cellWork.Append("&PermitNumber=" + ((Results[x].Number == null) ? string.Empty : Results[x].Number));
                        cellWork.Append("&AltID=" + ((Results[x].Alias == null || Results[x].Alias == string.Empty) ? Results[x].Number.ToString() : Results[x].Alias.ToString()));
                        cellWork.Append("&Module=" + ((Results[x].Module == null) ? string.Empty : Results[x].Module));
                        cellWork.Append("&Mode=" + ((SearchMode == "Related Permits") ? "View Permits" : SearchMode));
                        cellWork.Append("&ResultPage=" + (CurrentResultPageNumber - 1).ToString());
                        cellWork.Append("&Agency=" + Results[x].Agency);
                        if (isiPhone == true)
                        {
                            cellWork.Append("\"><span class=\"pageListCellBoldText\">" + ((Results[x].Alias == null || Results[x].Alias == string.Empty) ? "[" + Results[x].Number.ToString() + "]" : Results[x].Alias.ToString()) + "</span>");
                        }
                        else
                        {
                            cellWork.Append("\">" + ((Results[x].Alias == null || Results[x].Alias == string.Empty) ? "[" + Results[x].Number.ToString() + "]" : Results[x].Alias.ToString()) + "</a>");
                        }
                    }
                    else
                    {
                        cellWork.Append("<label class=\"pageListNoLink\">");
                        if (isiPhone == true)
                        {
                            cellWork.Append(((Results[x].Alias == null || Results[x].Alias == string.Empty) ? "[" + Results[x].Number.ToString() + "]" : Results[x].Alias.ToString()));
                        }
                        else
                        {
                            cellWork.Append((Results[x].Alias == null || Results[x].Alias == string.Empty) ? "[" + Results[x].Number.ToString() + "]" : Results[x].Alias.ToString());
                        }
                        cellWork.Append("</label>");
                    }
                    if (Results[x].Address != null && Results[x].Address.ToString() != string.Empty)
                    {
                        cellWork.Append(theDash + Results[x].Address.ToString());
                    }
                    if (SearchMode == "View Permits")
                    {
                        if (Results[x].Status != null && Results[x].Status != string.Empty)
                        {
                            cellWork.Append(theDash + Results[x].Status.ToString());
                            theDash = " - ";
                        }
                        if (isCrossModuleEnabled == true && isAllModuleView == true && Results[x].Module != null && Results[x].Agency != string.Empty)
                        {
                            cellWork.Append(theDash + "<i>" + Results[x].Module.ToString() + "</i>");
                            theDash = " - ";
                        }
                        if (isSuperAgency == true && Results[x].Agency != null && Results[x].Agency != string.Empty)
                        {
                            cellWork.Append(theDash + "<i>" + Results[x].Agency.ToString() + "</i>");
                        }
                    }
                    if (isiPhone == true)
                    {
                        ResultOutput.Append(MyProxy.CreateListCell(checkBox, cellWork.ToString(), rowNum - 1, x, ResultCount, ResultPageStart, ResultRecordsPerPage, isiPhone, false, (String.IsNullOrEmpty(Results[x].capClass) || Results[x].capClass == ACAConstant.COMPLETED) ? true : false));
                    }
                    else
                    {
                        cellWork.Append("</td></tr>");
                        ResultOutput.Append(cellWork.ToString());
                    }
                }
                ResultOutput.Append("</table>");
                HTMLFactory htmlFactory = new HTMLFactory();
                if (SearchMode == "View Permits")
                {
                   capsInList = rowNum.ToString();
                   CollectionLinks.Append("<div id=\"collectionActions\">");
                   if (isiPhone == true)
                   {
                       CollectionLinks.Append("<center>");
                       CollectionLinks.Append("<b><input type=\"submit\"  name=\"submitAdd\" value=\"Add to " + mycollection_managepage_label_name + "\"/></b>");
                       CollectionLinks.Append("</center>");
                   }
                   else
                   {
                       CollectionLinks.Append("<b>" + htmlFactory.PresentSubmitButton(isOpera, "submitAdd", "Add to " + mycollection_managepage_label_name, "color:#040478; font-size:10pt; text-align:left; text-decoration:underline; background-color:transparent; border:none; cursor:pointer; cursor:hand;") + "</b>");
                   }
                   CollectionLinks.Append("</div>");
                   string sortDirectionIcon = "img/Caret_up_sml.gif";
                   if (sortDescending == true || sortDefault == true)
                   {
                       sortDirectionIcon = "img/Caret_down_sml.gif";
                   }
                   string sortDirectionHTML = "<img src=\"" + sortDirectionIcon + "\" style=\"margin-right:0px; margin-left:0px;\"/>";
                   //string sortInputStyle = "style=\"color:#040478; font-size:10pt; padding-left:0px; padding-right:0px; margin-left:0px; margin-right:0px; margin-top:00px; margin-bottom:0px; text-decoration:underline; background-color:transparent; border:none; cursor:pointer; cursor:hand;\"";

                   //SortOptions.Append("<table cellspacing=\"0px\" cellpadding=\"0px\" border=\"0px\" style=\"margin-top:0px; margin-bottom:10px; margin-left:0px;\">");
                   //SortOptions.Append("<td>Sort By:</td><td>");
                   string inputEndHTML = " | ";
                   if (isiPhone == true)
                   {
                       // inputEndHTML = string.Empty;
                   }                    
                   SortOptions.Append("<div id=\"sortSection\">");
                   SortOptions.Append("Sort By: ");
                   SortOptions.Append(htmlFactory.PresentSubmitButton(isOpera, "sortDate", "Date"));
                   // SortOptions.Append((sortByDate == true ? " font-weight:bold;" : string.Empty));
                   if (sortByDate == true)
                   {
                       SortOptions.Append(sortDirectionHTML);
                   }
                   SortOptions.Append(inputEndHTML);

                   SortOptions.Append(htmlFactory.PresentSubmitButton(isOpera, "sortNumber", "ID"));
                   // SortOptions.Append((sortByNumber == true ? " font-weight:bold;" : string.Empty));
                   if (sortByNumber == true)
                   {
                       SortOptions.Append(sortDirectionHTML);
                   }
                   SortOptions.Append(inputEndHTML);
                   SortOptions.Append(htmlFactory.PresentSubmitButton(isOpera, "sortStatus", "Status"));
                   // SortOptions.Append((sortByStatus == true ? " font-weight:bold;" : string.Empty));
                   if (sortByStatus == true)
                   {
                       SortOptions.Append(sortDirectionHTML);
                   }
                   if (isCrossModuleEnabled == true && isAllModuleView == true)
                   {
                       SortOptions.Append(inputEndHTML);
                       SortOptions.Append(htmlFactory.PresentSubmitButton(isOpera, "sortModule", "Module"));
                       // SortOptions.Append((sortByModule == true ? " font-weight:bold;" : string.Empty));
                       if (sortByModule == true)
                       {
                           SortOptions.Append(sortDirectionHTML);
                       }
                   }
                   if (isSuperAgency == true)
                   {
                       SortOptions.Append(inputEndHTML);
                       SortOptions.Append(htmlFactory.PresentSubmitButton(isOpera, "sortAgency", "Agency"));
                       // SortOptions.Append((sortByAgency == true ? " font-weight:bold;" : string.Empty));
                       if (sortByAgency == true)
                       {
                           SortOptions.Append(sortDirectionHTML);
                       }
                   }
                   // SortOptions.Append("</td></table>");
                   SortOptions.Append("</div>");
                }
            }
            #endregion
        }
        else
        {
            //DWB - This Inspeciton search is no longer used in AMCA
            #region Inspection Search
            //Title Row for Record table.
            //if filter type selected, keep it selected in next page refresh
            DisplayMode = "My Inspection";
            if (Filter != string.Empty)
            {
                ResultHeader.Append("<tr><td colspan=\"2\" >" + HTML.PresentSelectFromCachedArray(Application["InspectionStatus"].ToString(), "Status", Filter) + "<input type=\"submit\" value=\"Filter\"></input> &nbsp;");
                ResultHeader.Append(HTML.PresentLinkBr("AdvancedSearch.Results.aspx?State=" + State + "&Mode=" + SearchMode + "&SortByDate=true&InspectionStatus=" + Filter, "Sort by Date") + "</td></tr>");
            }
            else
            {
                ResultHeader.Append("<tr><td colspan=\"2\" >" + HTML.PresentSelectFromCachedArray(Application["InspectionStatus"].ToString(), "Status") + "<input type=\"submit\" value=\"Filter\"></input> &nbsp;");
                ResultHeader.Append(HTML.PresentLinkBr("AdvancedSearch.Results.aspx?State=" + State + "&Mode=" + SearchMode + "&SortByDate=true", "Sort by Date") + "</td></tr>");
            }
            ResultHeader.Append(HTML.Present2ColumnTableRow("Search Results : Inspections", "rowtitle"));
            //get sortby value
            bool sortBy = Request.QueryString["SortByDate"] != null ? true : false;
            //Get Result List
            // DWB - tempoarary remove
            // Inspection[] Results = MyProxy.InspectionSearch();
            Inspection[] Results = null;
            if (MyProxy.OnErrorReturn)
            {  // Proxy Exception 
                ErrorMessage.Append(MyProxy.ExceptionMessage);
            }

            string CSSClass = "row1";
            // get result count
            if (Results == null)
            {
                ResultCount = 0;
            }
            else
            {
                ResultCount = Results.Length;
            }
            // manipulate page start
            int ResultPageStart = (CurrentResultPageNumber - 1) * ResultRecordsPerPage;
            // Populate Records 
            for (int x = ResultPageStart; x < (((ResultPageStart + ResultRecordsPerPage) < ResultCount) ? (ResultPageStart + ResultRecordsPerPage) : ResultCount); x++)
            {
                string inspectionComments = Results[x].Comments;
                if (inspectionComments.Length > 30)
                    inspectionComments = inspectionComments.Substring(0, 25) + HTML.PresentLink("View.Details.aspx?State=" + State + "&Type=Inspection&Id=" + Results[x].Id,"...");
                ResultOutput.Append("<tr>" + HTML.PresentTableColumn("<b>Type: </b>" + HTML.PresentLink("Inspections.View.aspx?State=" + State + "&Id=" + Results[x].Id + "&Mode=" + SearchMode + "&ResultPage=" + CurrentResultPageNumber, Results[x].Type), CSSClass, "1") +
                                   HTML.PresentTableColumn("<b>Status: </b>" + HTML.PresentLink("Inspections.View.aspx?State=" + State + "&Id=" + Results[x].Id + "&Mode=" + SearchMode + "&ResultPage=" + CurrentResultPageNumber, Results[x].Status), CSSClass, "1") + "</tr>" +
                                   HTML.Present2ColumnTableRow("<b>Date: </b>" + HTML.PresentLink("Inspections.View.aspx?State=" + State + "&Id=" + Results[x].Id + "&Mode=" + SearchMode + "&ResultPage=" + CurrentResultPageNumber, Results[x].Date), CSSClass) +
                                   HTML.Present2ColumnTableRow("<b>Address: </b>" + HTML.PresentLink("Inspections.View.aspx?State=" + State + "&Id=" + Results[x].Id + "&Mode=" + SearchMode + "&ResultPage=" + CurrentResultPageNumber, Results[x].Address), CSSClass) +
                                   HTML.Present2ColumnTableRow("<b>Comments: </b>" + inspectionComments, CSSClass));

                CSSClass = (CSSClass == "row1") ? "row2" : "row1";
            }
            #endregion
        }

        #endregion

        #region Paging Part
        // Paging Part, appends to table footer
        // DWB - 07-25-2008 - Only include paging links if more than on page of records
        //                    are returned from the search.
        if ((double)ResultCount > (double)ResultRecordsPerPage)
        {
            int ResultPagesCount = (int)Math.Ceiling((double)ResultCount / (double)ResultRecordsPerPage);
            if ((int)Math.Ceiling((double)ResultPagesCount * (double)ResultRecordsPerPage) < ResultCount)
            {
                ResultPagesCount++;
            }
            // DWB - 02-26-2008 - Change paging according to specification is 2008 redesign document.
            int NextPageNumber = 0;
            int PageCount = 0;
            int PreviousPageNumber = CurrentResultPageNumber - 1;
            int PageStart = 0;
            if ((CurrentResultPageNumber - ((CurrentResultPageNumber / 10) * 10)) > 0)
            {
                PageStart = ((CurrentResultPageNumber / 10) * 10) + 1;
            }
            else
            {
                PageStart = CurrentResultPageNumber - 9;
            }
            string sLinkFormat = "<a id=\"pageNavigationButton\" href=\"";
            StringBuilder PagingLink = new StringBuilder();

            PagingLink.Append(sLinkFormat + "AdvancedSearch.Results.aspx?State=" + State);
            PagingLink.Append("&Mode=" + SearchMode);
            PagingLink.Append("&Module=" + ModuleName);
            PagingLink.Append("&InspectionStatus=" + Filter);
            PagingLink.Append("&PagingMode=true");
            PagingLink.Append("&cboDate=" + this.cboDate);
            PagingLink.Append("&cboMonth=" + this.cboMonth);
            PagingLink.Append("&cboYear=" + this.cboYear);
            PagingLink.Append("&SortColumn=" + SortColumn.ToString());
            PagingLink.Append("&SortDesc=" + SortDesc.ToString());
            PagingLink.Append("&ViewBaseModuleName=" + viewBaseModuleName);
            PagingLink.Append("&isAllModuleView=" + (isAllModuleView == true ? "true" : "false"));
            PagingLink.Append("&FilterType=" + FilterType);
            PagingLink.Append("&FilterMask=" + FilterMask);
            PagingFooter.Append("<div id=\"pageNavigation\">");
            if (isiPhone == true)
            {
                PagingFooter.Append("<center>");
            }

            string delimiter = string.Empty;
            if (PreviousPageNumber > 0)
            {
                PagingFooter.Append(PagingLink.ToString());
                PagingFooter.Append("&SlidePage=LeftToRight");
                PagingFooter.Append("&ResultPage=" + (PreviousPageNumber - 1).ToString());
                PagingFooter.Append(RelatedPermitsPaging);
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
                if (CurrentResultPageNumber == page)
                {
                    PagingFooter.Append(delimiter);
                    PagingFooter.Append("<span id=\"pageNavigationSelected\">" + page + "&nbsp;</span>");
                    NextPageNumber = page + 1;
                    delimiter = " | ";
                }
                else
                {
                    PagingFooter.Append(delimiter);
                    PagingFooter.Append(PagingLink);
                    if (CurrentResultPageNumber > page)
                    {
                        PagingFooter.Append("&SlidePage=LeftToRight");
                    }
                    PagingFooter.Append("&ResultPage=" + (page - 1).ToString());
                    PagingFooter.Append(RelatedPermitsPaging);
                    PagingFooter.Append("\">");
                    PagingFooter.Append(page.ToString());
                    PagingFooter.Append("</a>");
                    delimiter = " | ";
                }
            }
            if (NextPageNumber <= ResultPagesCount)
            {
                PagingFooter.Append(delimiter);
                PagingFooter.Append(PagingLink);
                PagingFooter.Append("&ResultPage=" + (NextPageNumber - 1).ToString());
                PagingFooter.Append(RelatedPermitsPaging);
                PagingFooter.Append("\">&gt;</a>");
                PagingFooter.Append("</div>");
                if (isiPhone == true)
                {
                    PagingFooter.Append("</center>");
                }
            }
        }
        #endregion
    }
    
    #region properties
    private string cboDate
    {
        get
        {
            if (Request["cboDate"] != null)
            {
                return Request["cboDate"].ToString();
            }
            else
                return String.Empty;
        }
    }
    private string cboMonth
    {
        get
        {
            if (Request["cboMonth"] != null)
                return Request["cboMonth"].ToString();
            else
                return String.Empty;
        }

    }
    private string cboYear
    {
        get
        {
            if (!String.IsNullOrEmpty(Request["cboYear"]))
            {
                return Request["cboYear"].ToString();
            }
            else
                return String.Empty;
        }
    }
    #endregion

    #region  Misc Subroutines
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
                        capModel.capClass = (capDetail[6] != null) ? capDetail[6] : string.Empty;
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
                    + "&Mode=" + SearchMode
                    + "&Module=" + ModuleName
                    + "&PageBreadcrumbIndex=" + PageBreadcrumbIndex.ToString()
                    + "&CollectionOperation=" + MyCollectionOperation
                    + "&ResultPage=" + (CurrentResultPageNumber - 1).ToString()
                    + "&SortColumn=" + GetFieldValue("SortColumn", false)
                    + "&SortDesc=" + GetFieldValue("SortDesc", false)
                    + "&FilterType=" + FilterType
                    + "&FilterMask=" + FilterMask
                    + "&ViewBaseModuleName=" + viewBaseModuleName
                    + "&isAllModuleView=" + (isAllModuleView == true ? "true" : "false")
                    );
            }
        }
    }
    /// <summary>
    /// Set the sort order for the current list.
    /// </summary>
    private void GetSortOrder(bool resetList, bool isDefaultSearchOrder)
    {
        string sortColumn = string.Empty;
        string sortDESC = string.Empty;
        string prevSortColumn = GetFieldValue("SortColumn", false);
        string prevSortDESC = GetFieldValue("SortDesc", false);

        if (resetList == false)
        {
            if (GetFieldValue("sortNumber", false) != string.Empty)
            {
                sortColumn = "Alias";
                isBreadcrumbRefeshMode = true;
                isBreadcrumbPagingMode = true;
                if (prevSortColumn.Equals(sortColumn) && prevSortDESC == string.Empty)
                {
                    sortColumn = "Alias";
                    sortDESC = " DESC";
                }
                sortByNumber = true;
                sortOrder = sortColumn + sortDESC;
                CurrentResultPageNumber = 1;
            }
            else if (GetFieldValue("sortStatus", false) != string.Empty)
            {
                sortColumn = "Status";
                isBreadcrumbRefeshMode = true;
                isBreadcrumbPagingMode = true;
                if (prevSortColumn.Equals(sortColumn) && prevSortDESC == string.Empty)
                {
                    sortDESC = " DESC";
                }
                sortByStatus = true;
                sortOrder = sortColumn + sortDESC + ", sortDate DESC, Number";
                CurrentResultPageNumber = 1;
            }
            else if (GetFieldValue("sortModule", false) != string.Empty)
            {
                sortColumn = "Module";
                isBreadcrumbRefeshMode = true;
                isBreadcrumbPagingMode = true;
                if (prevSortColumn.Equals(sortColumn) && prevSortDESC == string.Empty)
                {
                    sortDESC = " DESC";
                }
                sortByModule = true;
                sortOrder = sortColumn + sortDESC + ", Date DESC, Number DESC";
                CurrentResultPageNumber = 1;
            }
            else if (GetFieldValue("sortAgency", false) != string.Empty)
            {
                sortColumn = "Agency";
                isBreadcrumbRefeshMode = true;
                isBreadcrumbPagingMode = true;
                if (prevSortColumn.Equals(sortColumn) && prevSortDESC == string.Empty)
                {
                    sortDESC = " DESC";
                }
                sortByAgency = true;
                sortOrder = sortColumn + sortDESC + ", Date DESC, Number DESC";
                CurrentResultPageNumber = 1;
            }
            else if (GetFieldValue("sortDate", false) != string.Empty)
            {
                sortColumn = "sortDate";
                isBreadcrumbRefeshMode = true;
                isBreadcrumbPagingMode = true;
                if (prevSortColumn.Equals(sortColumn) && prevSortDESC == string.Empty)
                {
                    sortDESC = " DESC";
                }
                sortByDate = true;
                sortOrder = sortColumn + sortDESC + ",  Number";
                CurrentResultPageNumber = 1;
            }
            if (sortColumn == string.Empty)
            {
                if (prevSortColumn != string.Empty)
                {
                    sortColumn = prevSortColumn;
                    if (sortColumn == "sortDefault")
                    {
                        sortColumn = "sortDefault";
                        isDefaultSearchOrder = true;
                        sortDefault = true;
                    }
                    else if (sortColumn == "sortDate")
                        sortByDate = true;
                    else if (sortColumn == "Alias")
                        sortByNumber = true;
                    else if (sortColumn == "Status")
                        sortByStatus = true;
                    else if (sortColumn == "Module")
                        sortByModule = true;
                    else if (sortColumn == "Agency")
                        sortByAgency = true;
                    else
                        sortColumn = string.Empty;
                    sortDESC = prevSortDESC;
                }
                else
                {
                    if (isDefaultSearchOrder == true)
                    {
                        sortColumn = "sortDefault";
                        sortDefault = true;
                    }
                    else
                    {
                        sortColumn = "sortDate";
                        sortDESC = " DESC";
                    }
                    sortByDate = true;
                }
                sortOrder = sortColumn + sortDESC; // +", Number";
            }
        }
        else
        {
            if (isDefaultSearchOrder == true)
            {
                sortColumn = "sortDefault";
                sortDESC = " DESC";
                sortDefault = true;
            }
            else
            {
                sortColumn = "sortDate";
                sortDESC = " DESC";
                sortByDate = true;
                sortOrder = sortColumn + sortDESC; // +", Number";
            }
        }

        sortDescending = sortDESC == " DESC";
        SortColumn = sortColumn;
        SortDesc = sortDESC;
        ListSortColumn = sortColumn;
        ListSortDesc = sortDESC;
    }

    /// <summary>
    /// Get Field value from Form, Request, or QueryString.
    /// </summary>
    /// <param name="FieldName"> Field name </param>
    /// <param name="IsRequired"> Indcates if the user must enterer data into the field or not</param>
    /// <returns>String</returns>
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
    #endregion
}
