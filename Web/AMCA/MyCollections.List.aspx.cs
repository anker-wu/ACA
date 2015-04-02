/**
* <pre>
* 
*  Accela Citizen Access
*  File: MyCollections.List.aspx.cs
* 
*  Accela, Inc.
*  Copyright (C): 2007-2013
* 
*  Description:
*  View the list of collection memebers.
* 
*  Notes:
*      $Id: MyCollections.List.aspx.cs 245878 2013-03-13 06:25:21Z ACHIEVO\alan.hu $.
*  Revision History
*  &lt;Date&gt;,		&lt;Who&gt;,			&lt;What&gt;
*  10-15-2009           Dave Brewster           Created page for version 7.0
* </pre>
*/
using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Collections.Generic;
using System.Web;
using System.Web.Security;
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
public partial class MyCollectionsList : AccelaPage
{
    private const string HTML_EMPTY = "&nbsp;";
    public StringBuilder ResultHeader = new StringBuilder();
    public StringBuilder PagingFooter = new StringBuilder();
    public string SearchMode = string.Empty;
    public StringBuilder ResultOutput = new StringBuilder();
    public StringBuilder ListMode = new StringBuilder(); 
    public StringBuilder PagingFields = new StringBuilder();
    public StringBuilder SortOptions = new StringBuilder();
    public StringBuilder CollectionLinks = new StringBuilder();
    public string Filter = string.Empty;
    public string DisplayMode = string.Empty;
    public StringBuilder ErrorMessage = new StringBuilder();
    public StringBuilder Breadcrumbs = new StringBuilder();
    public StringBuilder FilterLink = new StringBuilder();
    public StringBuilder HiddenFields = new StringBuilder();
    public string CollectionModuleName = string.Empty;
    public string CollectionModule = string.Empty;
    public string CollectionId = string.Empty;
    public string ListSortColumn = string.Empty;
    public string ListSortDesc = string.Empty;

    public string BackForwardLinks = string.Empty;
    public string capsInList = string.Empty;
    public int CurrentResultPageNumber = 1;
    public string PageResultPageNo = string.Empty;
    private string SearchDescripton = string.Empty;

    private string sortOrder = string.Empty;
    private bool sortByDate = false;
    private bool sortByNumber = false;
    private bool sortByAgency = false;
    private bool sortByStatus = false;
    private bool sortByModule = false;
    private bool sortDescending = false;
    private bool isCrossModuleEnabled = false;
    private bool isCurrentModuleOnly = true;
    public const string HTML_NBSP = "&nbsp;";

    /// <summary>
    /// Advanced Search Options
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    //private string ErrorFormat = "<div><table id=\"errorMessage\" cellpadding=\"1\" cellspacing=\"0\" width=\"95%\"><tr><td valign=\"top\"><img src=\"img\\error.png\"/></td><td id=\"errorMessage\">";
    //private string ErrorFormatEnd = "</td></tr></table></div>";
    public string PageBreadcrumbIndex = string.Empty;
    private bool isBreadcrumbPagingMode = false;
    private bool isBreadcrumbRefeshMode = false;
    private string FilterType = string.Empty;
    private string FilterMask = string.Empty;

    protected void Page_Load(object sender, EventArgs e)
    {
        ValidationChecks("MyCollections.List.aspx");

        // default page number
        int ResultCount = 0;

        //number of records shown per page - from web.config
        AppSettingsReader Settings = new AppSettingsReader();
        int ResultRecordsPerPage = int.Parse(Settings.GetValue("SearchRecordsPerPage", typeof(string)).ToString());
        string mycollection_managepage_label_name = StripHTMLFromLabelText(LocalGetTextByKey("mycollection_collectionmanagement_collectionname"), "Collections");
        StringBuilder sbWork = new StringBuilder();
        PageBreadcrumbIndex = GetFieldValue("PageBreadcrumbIndex", false);
        bool isElipseLink = GetFieldValue("IsElipseLink", false) != string.Empty;
        string breadCrumbIndex = GetFieldValue("BreadCrumbIndex", false);
        if (breadCrumbIndex == string.Empty)
        {
            breadCrumbIndex = PageBreadcrumbIndex;
        }
        isBreadcrumbPagingMode = GetFieldValue("PagingMode", false) == "true";

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
        SearchMode = GetFieldValue("Mode", false);
        CollectionId = GetFieldValue("CollectionId", false);
        CollectionModule = GetFieldValue("CollectionModule", false);
        //Display collectionModule on page.
        CollectionModuleName = LabelUtil.GetI18NModuleName(CollectionModule);

        bool resetList = GetFieldValue("ReloadList", false) == "true";
        bool clearFilter = GetFieldValue("ClearFilter", false) == "true";
        string listFilter = string.Empty;
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
        if (resetList == true)
        {
            CurrentResultPageNumber = 1;
        }
        if (clearFilter)
        {
            isBreadcrumbRefeshMode = true;
            listFilter = string.Empty;
            FilterType = string.Empty;
            FilterMask = string.Empty;
        }
        HiddenFields.Append(HTML.PresentHiddenField("FilterType", FilterType));
        HiddenFields.Append(HTML.PresentHiddenField("FilterMask", FilterMask));

        SearchDescripton = SearchMode;
        bool isSuperAgency = StandardChoiceUtil.IsSuperAgency();
        bool isMyCollectionsMode = false;
        DataTable dtCapList = null;

        MyCollectionModel[] myCollection = null;
        myCollection = AppSession.GetMyCollectionsFromSession();
        if (CollectionId != string.Empty && myCollection[0].collectionId != Convert.ToInt64(CollectionId))
        {
            // breadcrumbs have returned control to a list from a different collection
            // than the one that was being access previously so the application must restore the
            // previous collection.
            IMyCollectionBll myCollectionBll = (IMyCollectionBll)ObjectFactory.GetObject(typeof(IMyCollectionBll));
            MyCollectionModel myCollectionModel = myCollectionBll.GetCollectionDetailInfo(ConfigManager.AgencyCode, AppSession.User.PublicUserId, CollectionId, null);
            if (myCollectionModel != null)
            {
                myCollection[0] = myCollectionModel;
                AppSession.SetMyCollectionsToSession(myCollection);
            }
        }

        CheckForCollectionActionRequest(CollectionModule);

        GetSortOrder(resetList);

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
            string parenLeft = " (";
            string parenRight = ")";

            if (isiPhone == true)
            {
                parenLeft = string.Empty;
                parenRight = string.Empty;
            }
            linkStyle = "<a id=\"pageTitleButton\" href=\"";
            if (Session["AMCA_MyCollection_DataTable"] != null && resetList == false)
            {
                dtCapList = (DataTable)Session["AMCA_MyCollection_DataTable"];
            }
            if (dtCapList == null)
            {
                MyCollectionProxy myCollectionProxy = new MyCollectionProxy();
                dtCapList = myCollectionProxy.getMyCollectionList(myCollection[0].collectionId, CollectionModule);
                if (dtCapList != null && sortOrder.Length == 0)
                {
                    dtCapList.DefaultView.Sort = sortOrder;
                }
            }
            if (dtCapList != null)
            {
                FilterLink.Append(parenLeft);
                if (listFilter == string.Empty)
                {
                    FilterLink.Append(linkStyle + "AdvancedSearch.Filter.aspx?State=" + State);
                }
                else
                {
                    FilterLink.Append(linkStyle + "MyCollections.List.aspx?State=" + State);
                }
                FilterLink.Append("&Mode=" + SearchMode);
                FilterLink.Append("&Module=" + ModuleName);
                FilterLink.Append("&CollectionId=" + myCollection[0].collectionId);
                FilterLink.Append("&CollectionModule=" + CollectionModule);
                FilterLink.Append("&ResultPage=" + (CurrentResultPageNumber - 1).ToString());
                if (listFilter == string.Empty)
                {
                    FilterLink.Append("\">Filter Results</a>");
                }
                else
                {
                    FilterLink.Append("&ClearFilter=true");
                    FilterLink.Append("\">Clear Filter</a>");
                }
                FilterLink.Append(parenRight);
                linkStyle = "style=\"text-decoration:underline;\"";
            }
            if (!isiPhone == true)
            {
                ListMode.Append("<div id=\"pageTitle\">");
                ListMode.Append(CollectionModuleName);
                ListMode.Append(FilterLink.ToString());
                ListMode.Append("</div>");
            }
            else
            {
                iPhonePageTitle = CollectionModuleName;
            }

            sbWork.Append("&Module=" + ModuleName);
            sbWork.Append("&Mode=" + SearchMode);
            sbWork.Append("&CollectionModule=" + CollectionModule);
            sbWork.Append("&CollectionId=" + myCollection[0].collectionId);
            sbWork.Append("&ResultPage=" + (CurrentResultPageNumber - 1).ToString());
            sbWork.Append("&FilterType=" + FilterType);
            sbWork.Append("&FilterMask=" + FilterMask);
            Breadcrumbs = BreadCrumbHelper("MyCollections.List.aspx", sbWork, CollectionModuleName, breadCrumbIndex, isElipseLink, isBreadcrumbRefeshMode, isBreadcrumbPagingMode, false);
            PageBreadcrumbIndex = CurrentBreadCrumbIndex.ToString();

            iPhoneHideFooterBar = (dtCapList == null && dtCapList.Rows.Count == 0);
            BackForwardLinks = BackLinkHelper(CurrentBreadCrumbIndex.ToString());
            iPhoneHideFooterBar = false;

            if (dtCapList != null && dtCapList.Rows.Count != 0)
            {
                if (sortOrder.Length != 0 && sortOrder.Equals(dtCapList.DefaultView.Sort.ToString()) == false)
                {
                    dtCapList.DefaultView.Sort = sortOrder;
                }
                if (listFilter.Length != 0 && listFilter.Equals(dtCapList.DefaultView.RowFilter.ToString()) == false)
                {
                    try
                    {
                        dtCapList.DefaultView.RowFilter = "Alias LIKE '" + listFilter + "'";
                    }
                    catch (Exception ex)
                    {
                        Session["ACMA_FilterError"] = ex.Message;
                        Response.Redirect("AdvancedSearch.Filter.aspx?State=" + State
                            + "&Mode=" + SearchMode
                            + "&Module=" + ModuleName
                            + "&CollectionId=" + myCollection[0].collectionId
                            + "&CollectionModule=" + CollectionModule
                            + "&CollectionModuleName=" + CollectionModuleName
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
            Session["AMCA_MyCollection_DataTable"] = dtCapList;
            isMyCollectionsMode = true;
            SearchDescripton = CollectionModule;

            if (Results == null || (dtCapList != null && dtCapList.DefaultView.Count == 0))
            {
                ResultOutput.Append("<br><i>No records were found.");
                if (dtCapList != null && dtCapList.Rows.Count != 0)
                {
                    if (listFilter != string.Empty)
                    {
                        ResultOutput.Append("<br> Select the 'Clear Filter' link to veiw all of the the collecion members.");
                    }
                }
                ResultOutput.Append("</i><br>");
            }
            else if (MyProxy.OnErrorReturn)
            {  // Proxy Exception 
                ErrorMessage.Append(MyProxy.ExceptionMessage);
            }

            //#region  Permit Search

            if (Results != null)
            {
                ResultCount = Results.Length;
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
                    ResultOutput.Append("<table width=\"95%\" cellspacing=\"0px\" cellpadding=\"0px\" style=\"margin-bottom:0px; border-right-width:0px; border-left-width:0px; border-top-width:0px; border-bottom-width:0px; border-color:#e6e6e6;\" >");
                }
                string checkBox = string.Empty;
                int rowNum = 0;

                for (int x = ResultPageStart; x < (((ResultPageStart + ResultRecordsPerPage) < ResultCount) ? (ResultPageStart + ResultRecordsPerPage) : ResultCount); x++)
                {
                    string tableCellHTML = "<td valign=\"top\" style=\"border-right-width:0px; border-left-width:0px; border-top-width:0px; border-bottom-width:1px; border-color:#e6e6e6;";
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
                    if (isiPhone == true)
                    {
                        tableCellHTML = string.Empty;
                        endHTML = string.Empty;
                    }
                    checkBox = tableCellHTML + "<input id=\"Row" + rowNum.ToString() + "\" type=\"checkbox\""
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
                    rowNum++;
                    string permitDescription = (Results[x].Desc != null) ? Results[x].Desc : string.Empty;
                    if (permitDescription.Length > 30)
                    {
                        permitDescription = permitDescription.Substring(0, 25) + HTML.PresentLink("View.Details.aspx?State=" + State + "&Type=Permit&Id=" + Results[x].Number, "...");
   
                    }
                    StringBuilder cellWork = new StringBuilder();
                    string aDash = "<br>";
                    if (isiPhone != true)
                    {
                        cellWork.Append("<tr style=\"height:40px\">" + checkBox + tableCellHTML);
                        aDash = " - ";
                    }
                    cellWork.Append("<a  class=\"pageListLink\" href=\"");
                    cellWork.Append("Permits.View.aspx?State=" + State.ToString());
                    cellWork.Append("&PermitType=" + ((Results[x].Type == null) ? string.Empty : Results[x].Type));
                    cellWork.Append("&PermitNumber=" + ((Results[x].Number == null) ? string.Empty : Results[x].Number));
                    cellWork.Append("&AltID=" + ((Results[x].Alias == null || Results[x].Alias == string.Empty) ? Results[x].Number.ToString() : Results[x].Alias.ToString()));
                    cellWork.Append("&Module=" + ((Results[x].Module == null) ? string.Empty : Results[x].Module));
                    cellWork.Append("&CollectionId=" + CollectionId);
                    cellWork.Append("&CollectionModule=" + CollectionModule);
                    cellWork.Append("&Mode=" + ((SearchMode == "Related Permits") ? "View Permits" : SearchMode));
                    cellWork.Append("&ResultPage=" + (CurrentResultPageNumber - 1).ToString());
                    if (isiPhone == true)
                    {
                        cellWork.Append("\"><span class=\"pageListCellBoldText\">");
                        cellWork.Append(((Results[x].Alias == null || Results[x].Alias == string.Empty) ? "[" + Results[x].Number.ToString() + "]" : Results[x].Alias.ToString()));
                        cellWork.Append("</span>");
                    }
                    else
                    {
                        cellWork.Append("\">" + ((Results[x].Alias == null || Results[x].Alias == string.Empty) ? "[" + Results[x].Number.ToString() + "]" : Results[x].Alias.ToString()) + "</a>");
                    }
                    // cellWork.Append("\">" + Results[x].Date + " - " + Results[x].Number.ToString() + " - " + ((Results[x].Alias == null || Results[x].Alias == string.Empty) ? Results[x].Number.ToString() : Results[x].Alias.ToString()) + "</a>");
                    if (Results[x].Address != null && Results[x].Address.ToString() != string.Empty)
                    {
                        cellWork.Append(aDash + Results[x].Address.ToString());
                    }
                    if (Results[x].Status != null && Results[x].Status.ToString() != string.Empty)
                    {
                       cellWork.Append(aDash + Results[x].Status.ToString());
                       aDash = " - ";
                    }
                    if (isCrossModuleEnabled == true && isCurrentModuleOnly == false && Results[x].Module != null && Results[x].Agency.ToString() != string.Empty)
                    {
                        cellWork.Append(aDash +  "<i>" + Results[x].Module.ToString() + "</i>");
                        aDash = " - ";
                    }
                    if (isSuperAgency == true && Results[x].Agency != null && Results[x].Agency != string.Empty)
                    {
                        cellWork.Append(aDash + "<i>" + Results[x].Agency.ToString() + "</i>");
                    }
                    if (isiPhone == true)
                    {
                        ResultOutput.Append(MyProxy.CreateListCell(checkBox, cellWork.ToString(), rowNum - 1, x, ResultCount, ResultPageStart, ResultRecordsPerPage, isiPhone, false));
                    }
                    else
                    {
                        cellWork.Append("</td></tr>");
                        ResultOutput.Append(cellWork.ToString());
                    }
                }

                if (isiPhone != true)
                {
                    ResultOutput.Append("</table>");
                }
                capsInList = rowNum.ToString();
                CollectionLinks.Append("<div id=\"collectionActions\">");
                
                HTMLFactory htmlFactory = new HTMLFactory();

                if (isMyCollectionsMode)
                {
                    if (isiPhone == true)
                    {
                        CollectionLinks.Append("<center>");
                    }

                    CollectionLinks.Append(htmlFactory.PresentSubmitButton(isOpera, "submitMove", "Move To..."));
                    CollectionLinks.Append("|");
                    CollectionLinks.Append(htmlFactory.PresentSubmitButton(isOpera, "submitCopy", "Copy To..."));
                    CollectionLinks.Append("|");
                    CollectionLinks.Append(htmlFactory.PresentSubmitButton(isOpera, "submitRemove", "Remove"));

                    if (isiPhone == true)
                    {
                        CollectionLinks.Append("</center>");
                    }
                }
                else
                {
                    CollectionLinks.Append("<b>" + htmlFactory.PresentSubmitButton(isOpera, "submitAdd", "Add to " + mycollection_managepage_label_name) + "</b>");
                }

                CollectionLinks.Append("</div>");
                string sortDirectionIcon = "img/Caret_up_sml.gif";
                if (sortDescending == true)
                {
                    sortDirectionIcon = "img/Caret_down_sml.gif";
                }

                string inputEndHTML = " | ";
                //if (isiPhone == true)
                //{
                //    inputEndHTML = string.Empty;
                //}
                string sortDirectionHTML = "<img src=\"" + sortDirectionIcon + "\"/>";

                SortOptions.Append("<div id=\"sortSection\">");
                SortOptions.Append("Sort By: ");
                SortOptions.Append(htmlFactory.PresentSubmitButton(isOpera, "sortDate", "Date"));
                if (sortByDate == true)
                {
                    SortOptions.Append(sortDirectionHTML);
                }
                SortOptions.Append(inputEndHTML);

                SortOptions.Append(htmlFactory.PresentSubmitButton(isOpera, "sortNumber", "ID"));
                if (sortByNumber == true)
                {
                    SortOptions.Append(sortDirectionHTML);
                }
                SortOptions.Append(inputEndHTML);

                SortOptions.Append(htmlFactory.PresentSubmitButton(isOpera, "sortStatus", "Status"));
                if (sortByStatus == true)
                {
                    SortOptions.Append(sortDirectionHTML);
                }
                if (isCrossModuleEnabled == true && isCurrentModuleOnly == false)
                {
                    SortOptions.Append(htmlFactory.PresentSubmitButton(isOpera, "sortModule", "Module"));
                    if (sortByModule == true)
                    {
                        SortOptions.Append(sortDirectionHTML);
                    }
                    SortOptions.Append(inputEndHTML);
                }
                if (isSuperAgency == true)
                {
                    SortOptions.Append(htmlFactory.PresentSubmitButton(isOpera, "sortAgency", "Agency"));
                    if (sortByAgency == true)
                    {
                        SortOptions.Append(sortDirectionHTML);
                    }
                    SortOptions.Append(inputEndHTML);
                }
                SortOptions.Append("</div>");
            }
        }

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
                if (CurrentResultPageNumber == ResultPagesCount)
                {
                    PageStart = CurrentResultPageNumber - 9;
                }
                else
                {
                    PageStart = 1;
                }
            }

            string sLinkFormat = "<a id=\"pageNavigationButton\" href=\"";
            StringBuilder PagingLink = new StringBuilder();

            PagingLink.Append(sLinkFormat + "MyCollections.List.aspx?State=" + State);
            PagingLink.Append("&Mode=" + SearchMode);
            PagingLink.Append("&Module=" + ModuleName);
            PagingLink.Append("&InspectionStatus=" + Filter);
            PagingLink.Append("&CollectionId=" + CollectionId);
            PagingLink.Append("&CollectionModule=" + CollectionModule);
            PagingLink.Append("&PagingMode=true");
            PagingLink.Append("&SortColumn=" + SortColumn.ToString());
            PagingLink.Append("&SortDesc=" + SortDesc.ToString());
            PagingLink.Append("&cboDate=" + this.cboDate);
            PagingLink.Append("&cboMonth=" + this.cboMonth);
            PagingLink.Append("&cboYear=" + this.cboYear);
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
                PagingFooter.Append("\">&lt;</a>&nbsp;");

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
                    delimiter = " | ";
                    PagingFooter.Append("<span id=\"pageNavigationSelected\">" + page + "&nbsp;</span>");
                    NextPageNumber = page + 1;
                }
                else
                {
                    PagingFooter.Append(delimiter);
                    delimiter = " | ";
                    PagingFooter.Append(PagingLink);
                    if (CurrentResultPageNumber > page)
                    {
                        PagingFooter.Append("&SlidePage=LeftToRight");
                    }
                    PagingFooter.Append("&ResultPage=" + (page - 1).ToString());
                    //PagingFooter.Append(RelatedPermitsPaging);
                    PagingFooter.Append("\">");
                    PagingFooter.Append(page.ToString());
                    PagingFooter.Append("</a>&nbsp;");
                }
            }
            if (NextPageNumber <= ResultPagesCount)
            {
                PagingFooter.Append(delimiter);
                PagingFooter.Append(PagingLink);
                PagingFooter.Append("&ResultPage=" + (NextPageNumber - 1).ToString());
                PagingFooter.Append("\">&gt;</a>&nbsp;");
            }
            PagingFooter.Append("</div>");
            if (isiPhone == true)
            {
                PagingFooter.Append("</center>");
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
    private void CheckForCollectionActionRequest(string collectionModule)
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
                    + "&CollectionId=" + CollectionId
                    + "&CollectionModule=" + collectionModule
                    + "&CollectionOperation=" + MyCollectionOperation
                    + "&PageBreadcrumbIndex=" + PageBreadcrumbIndex.ToString()
                    + "&SortColumn=" + GetFieldValue("SortColumn", false)
                    + "&SortDesc=" + GetFieldValue("SortDesc", false)
                    + "&ResultPage=" + (CurrentResultPageNumber - 1).ToString()
                    + "&FilterType=" + FilterType
                    + "&FilterMask=" + FilterMask);
            }
        }
    }
    /// <summary>
    /// Set page sort order.
    /// </summary>
    private void GetSortOrder(bool resetList)
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
                sortOrder = sortColumn + sortDESC + ", Date DESC, Number";
                CurrentResultPageNumber = 1;
            }
            if (sortColumn == string.Empty)
            {
                if (prevSortColumn != string.Empty)
                {
                    sortColumn = prevSortColumn;
                    if (sortColumn == "sortDate")
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
                    sortColumn = "sortDate";
                    sortDESC = " DESC";
                    sortByDate = true;
                }
                sortOrder = sortColumn + sortDESC + ", Number";
            }
        }
        else
        {
            sortColumn = "sortDate";
            sortDESC = " DESC";
            sortByDate = true;
            sortOrder = sortColumn + sortDESC + ", Number";
        }
        sortDescending = sortDESC == " DESC";
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

    #endregion
}
