/**
* <pre>
* 
*  Accela Citizen Access
*  File: Login.Home.aspx.cs
* 
*  Accela, Inc.
*  Copyright (C): 2007-2014
* 
*  Description:
*  ACA login home page. 
* 
*  Notes:
*      $Id: Login.Home.aspx.cs 77905 2009-07-17 12:49:28Z dave.brewster $.
*  Revision History
*  &lt;Date&gt;,		&lt;Who&gt;,			&lt;What&gt;
*  07-17-2009           Dave Brewster           New page added for version 7.0
* 
* </pre>
*/
using System;
using System.Collections.Generic;
using System.Text;
using Accela.ACA.BLL.Common;
using Accela.ACA.BLL.People;
using Accela.ACA.Common;
using Accela.ACA.Web.Common;
using Accela.ACA.Web.Common.GlobalSearch;
using Accela.ACA.Web.Util;

public partial class LoginHome : AccelaPage
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
    public StringBuilder UserLink = new StringBuilder();
    public StringBuilder ErrorMessage = new StringBuilder();
    public StringBuilder SearchSection = new StringBuilder();
    public StringBuilder SearchMessage = new StringBuilder();
    public StringBuilder CollectionsSection = new StringBuilder();
    public StringBuilder ModulesSection = new StringBuilder();

    public string SearchMode = string.Empty;
    public string PageTitle = string.Empty;
    public string CurrentModule = string.Empty;
    string globalSearchPattern = string.Empty;
    public string aChar = string.Empty;

    protected void Page_Load(object sender, EventArgs e)
    {
        ValidationChecks("Login.Home.aspx");
        
        string mycollection_managepage_label_name = StripHTMLFromLabelText(LocalGetTextByKey("mycollection_global_label_collection"), "Collections");
        string aca_sys_default_home = StripHTMLFromLabelText(LocalGetTextByKey("aca_sys_default_home"), "Home");

        State = Request.QueryString["State"].ToString();
        bool invalidSearchPattern = (GetFieldValue("InvalidSearchPattern", false) == "true");
        bool notReadyYet = (GetFieldValue("NotReadyYet", false) == "true");
        string aChar = char.ConvertFromUtf32(171);
        SearchMode = "View Permits";
        int resultSets = 0;
        if (GetFieldValue("UpdateSession", false) != string.Empty)
        {
            string queryText = string.Empty;
            globalSearchPattern = GetFieldValue("GlobalSearchPattern", false) == string.Empty ? string.Empty : GetFieldValue("GlobalSearchPattern", false);
            if (globalSearchPattern != string.Empty && globalSearchPattern.Length > 2)
            {
                List<CAPView4UI> capList = new List<CAPView4UI>();
                List<LPView4UI> lpList = new List<LPView4UI>();
                List<APOView4UI> apoList = new List<APOView4UI>();
                List<APOView4UI> adrList = new List<APOView4UI>();

                if (Session["AMCA_GlobalSearchPattern"] != null)
                {
                    if (Session["AMCA_GlobalSearchPattern"].ToString() != globalSearchPattern.ToString())
                    {
                        GlobalSearchManager.ClearHistoryAction();
                    }
                }

                Session["AMCA_GlobalSearchPattern"] = globalSearchPattern.ToString();
                queryText = globalSearchPattern.ToString();
                bool isLoadingHistory = CanLoadHistory(queryText);
                
                try
                {
                    if (!isLoadingHistory)
                    {
                        string[] modules = GlobalSearchUtil.GetAllModuleKeys();
                        // Get query text from search input box
                        capList = GlobalSearchManager.ExecuteQuery<CAPView4UI>(GlobalSearchType.CAP, queryText, modules, CAP_FIRST_COLUMN_NAME, ACAConstant.ORDER_BY_DESC, ACAConstant.DEFAULT_PAGESIZE,null);
                        lpList = GlobalSearchManager.ExecuteQuery<LPView4UI>(GlobalSearchType.LP, queryText, modules, LP_FIRST_COLUMN_NAME, ACAConstant.ORDER_BY_ASC, ACAConstant.DEFAULT_PAGESIZE,null);
                        apoList = GlobalSearchManager.ExecuteQuery<APOView4UI>(GlobalSearchType.PARCEL, queryText, modules, PARCEL_FIRST_COLUMN_NAME, ACAConstant.ORDER_BY_ASC, ACAConstant.DEFAULT_PAGESIZE,null);
                        adrList = GlobalSearchManager.ExecuteQuery<APOView4UI>(GlobalSearchType.ADDRESS, queryText, modules, ADDRESS_FIRST_COLUMN_NAME, ACAConstant.ORDER_BY_ASC, ACAConstant.DEFAULT_PAGESIZE,null);
                    }
                    else
                    {
                        // Get query conditions from last history action
                        if (GlobalSearchUtil.IsRecordEnabled())
                        {
                            capList = GlobalSearchManager.ExecuteQuery<CAPView4UI>(GlobalSearchType.CAP,null);
                        }
                        if (GlobalSearchUtil.IsLPEnabled())
                        {
                            lpList = GlobalSearchManager.ExecuteQuery<LPView4UI>(GlobalSearchType.LP,null);
                        }
                        if (GlobalSearchUtil.IsAPOEnabled())
                        {
                            GlobalSearchParameter apoHistoryParameter = GlobalSearchManager.GetGlobalSearchParameter(GlobalSearchType.PARCEL);
                            apoList = GlobalSearchManager.ExecuteQuery<APOView4UI>(GlobalSearchType.PARCEL, apoHistoryParameter.QueryText, apoHistoryParameter.ModuleArray, string.Empty, string.Empty, apoHistoryParameter.PageSize,null);
                            GlobalSearchParameter adrHistoryParameter = GlobalSearchManager.GetGlobalSearchParameter(GlobalSearchType.ADDRESS);
                            adrList = GlobalSearchManager.ExecuteQuery<APOView4UI>(GlobalSearchType.ADDRESS, adrHistoryParameter.QueryText, adrHistoryParameter.ModuleArray, string.Empty, string.Empty, adrHistoryParameter.PageSize,null);
                        }
                    }
                    resultSets = capList != null ?  capList.Count : 0;
                    resultSets += lpList != null ? lpList.Count : 0;
                    resultSets += apoList != null ? apoList.Count : 0;
                    resultSets += adrList != null ? adrList.Count : 0;
                    string per_globalsearch_label_noresultsnotice = LocalGetTextByKey("per_globalsearch_label_noresultsnotice");
                    if (resultSets == 0)
                    {
                        // SearchMessage.Append("<table  cellpadding=\"1\" cellspacing=\"0\" width=\"95%\"><tr><td>");
                        if (per_globalsearch_label_noresultsnotice != string.Empty)
                        {
                            SearchMessage.Append(string.Format(per_globalsearch_label_noresultsnotice, globalSearchPattern.ToString()));
                        }
                        else
                        {
                            SearchMessage.Append("No records were found for the character pattern that you entered.");
                        }
                        // SearchMessage.Append("</td></tr></table>");
                    }
                }
                catch (Exception ex)
                {
                    ErrorMessage.Append(ErrorFormat);
                    ErrorMessage.Append(ex.Message);
                    ErrorMessage.Append(ErrorFormatEnd);
                    ErrorMessage.Append("<br>");
                    capList = null;
                    lpList = null;
                    apoList = null;
                    adrList = null;
                }
            }
            else
            {
                string per_globalsearch_message_searchhint = LocalGetTextByKey("per_globalsearch_message_searchhint");
                SearchMessage.Append("<table cellpadding=\"1\" cellspacing=\"0\" width=\"95%\"><tr><td>");
                if (per_globalsearch_message_searchhint != string.Empty)
                {
                    SearchMessage.Append(string.Format(per_globalsearch_message_searchhint, globalSearchPattern.ToString()));
                }
                else
                {
                    SearchMessage.Append("No records were found for the character pattern that you entered.");
                }
                SearchMessage.Append("</td></tr></table><br>");
            }
            if (resultSets != 0)
            {
                string globalSearchQueries = queryText;
                if (Session["AMCA_GlobalSearchQueries"] != null)
                {
                    string previouSearchQueries = Session["AMCA_GlobalSearchQueries"].ToString();
                    Session.Remove("AMCA_GlobalSearchQueries");
                    string[] history = previouSearchQueries.Split('|');
                    foreach (string qValue in history)
                    {
                        if (qValue == queryText)
                            continue;
                        if (qValue == "<end>")
                        {
                            continue;
                        }
                        globalSearchQueries += ("|" + qValue);
                    }
                }
                globalSearchQueries += ("|<end>");
                Session["AMCA_GlobalSearchQueries"] = globalSearchQueries;
                Session["AMCA_GlobalSearch_Mode"] = "true";
                Response.Redirect("GlobalSearch.Results.aspx?State=" + State
                   + "&Mode=" + SearchMode
                   + "&Module=" + ModuleName
                   + "&ResetList=true"
                   + "&BreadCrumbIndex=2"   //Added for 7.1 for browser  back button  issues. 
                   + "&GlobalSearch=true");
            }
        }

        AccelaProxy accelaProxy = new AccelaProxy();
        TabItemCollection moduleList = accelaProxy.GetModuleList();
        // string linkHTML = "<a style=\"color:#040478;  text-decoration:underline;\" href=\"";

        string linkHTML = "<a class=\"pageListLink\" href=\"";
        string iPhoneLink = (isiPhone == true) ? "<img style=\"float:right\" src=\"img/chevron.png\" /> " : string.Empty;
        StringBuilder listCell = new StringBuilder();
        int aColumn = 0;
        int rowCnt = 0;
        if (moduleList.Count > 0)
        {
            if (isiPhone)
            {
                ModulesSection.Append("<div id=\"pageSectionTitle\"><label>Records </label></div>"); 
            }
            else
            {
                ModulesSection.Append("<div id=\"pageSectionTitle\"><label>View Records In: </label></div>"); // + CurrentModule);
            }

            ModulesSection.Append("<table border=\"0\" cell-padding=\"0\" cell-spacing=\"0\" width=\"100%\">");
            foreach (TabItem aTab in moduleList)
            {
                rowCnt++;
                listCell = new StringBuilder();
                if (isiPhone == true)
                {
                    listCell.Append(linkHTML);
                    listCell.Append("AdvancedSearch.Results.aspx?State=" + State);
                    listCell.Append("&Mode=" + SearchMode);
                    listCell.Append("&Module=" + aTab.Module);
                    string text = StripHTMLFromLabelText(LabelUtil.GetTextByKey("aca_sys_default_module_name", aTab.Module), aTab.Module);
                    if (string.IsNullOrEmpty(text) || text == ACAConstant.DEFAULT_MODULE_NAME)
                    {
                        text = aTab.Module;
                    }
                    listCell.Append("&ModuleLabel=" + aTab.Label);
                    listCell.Append("&ReloadList=true");
                    listCell.Append("&ShowCurrentModuleList=true");
                    listCell.Append("&NewSearch=true\"><span class=\"pageListCellBoldText\">");
                    listCell.Append(text);
                    listCell.Append("<img style=\"float:right\" src=\"img/chevron.png\" /> ");
                    listCell.Append("</a></span>");
                    ModulesSection.Append(MyProxy.CreateSelectListCell(listCell.ToString(), rowCnt - 1, rowCnt - 1, moduleList.Count, 0, 9999, isiPhone, true));
                }
                else
                {
                    if (aColumn == 0)
                    {
                        ModulesSection.Append("<tr>");
                    }
                    ModulesSection.Append("<td class=\"pageListLink\" width=\"50%\">" + linkHTML + "AdvancedSearch.Results.aspx?State=" + State);
                    ModulesSection.Append("&Mode=" + SearchMode);
                    ModulesSection.Append("&Module=" + aTab.Module);
                    string text = StripHTMLFromLabelText(LabelUtil.GetTextByKey("aca_sys_default_module_name", aTab.Module), aTab.Module);
                    if (string.IsNullOrEmpty(text) || text == ACAConstant.DEFAULT_MODULE_NAME)
                    {
                        text = aTab.Module;
                    }
                    ModulesSection.Append("&ModuleLabel=" + aTab.Label);
                    ModulesSection.Append("&ReloadList=true");
                    ModulesSection.Append("&ShowCurrentModuleList=true");
                    ModulesSection.Append("&NewSearch=true\">");
                    ModulesSection.Append(text);
                    ModulesSection.Append("</a></td>");
                    if (aColumn == 1)
                    {
                        ModulesSection.Append("</tr>");
                        aColumn = -1;
                    }
                    aColumn++;
                }

            }
            if (isiPhone == true)
            {
                ModulesSection.Append("</div>");
            }
            if (aColumn == 1)
                ModulesSection.Append("</tr>");

            ModulesSection.Append("</table>");
        }

        if (!isiPhone)
        {
            IPeopleBll peopleBll = (IPeopleBll)ObjectFactory.GetObject(typeof(IPeopleBll));
            string userName = peopleBll.GetContactUserName(AppSession.User.UserModel4WS);
            userName = DataUtil.TruncateString(userName, 25);

            UserLink.Append("<div id=\"breadcrumbs\">");
            UserLink.Append("<table cell-padding=\"0\" cell-spacing=\"0\" width=\"100%\" style=\"margin-top:0px; margin-bottom:0px;\">");
            UserLink.Append("<tr>");
            UserLink.Append("<td width=\"90%\" style=\"text-align:right;\">"+ userName);

            if (AuthenticationUtil.IsInternalAuthAdapter)
            {
                UserLink.Append(" <a href=\"Default.aspx\">Logout</a>");
            }
            else
            {
                UserLink.Append(" <a href=\"" + AuthenticationUtil.LogoutUrl + "\">Logout</a>");
            }

            UserLink.Append("</td></tr></table></div>");

            PageTitle = "<div id=\"pageTitle\">" + aca_sys_default_home + "</div>";
        }
        if (Session["AMCA_GlobalSearchIsEnabled"] != null)
        {
            Session.Remove("AMCA_GlobalSearchIsEnabled");
        }
        if (GlobalSearchUtil.IsGlobalSearchEnabled() == true)
        {
            if (isiPhone)
            {
                Session["AMCA_GlobalSearchIsEnabled"] = "true";
            }
            else
            {
                string per_globalsearch_label_search = "Search";
                globalSearchPattern = Session["AMCA_globalSearchPattern"] == null ? string.Empty : Session["AMCA_globalSearchPattern"].ToString();

                SearchSection.Append("<div id=\"globalSearchSection\">");
                SearchSection.Append("<table border=\"0\" cellpadding=\"1\" cellspacing=\"0\" width=\"95%\">");
                SearchSection.Append("<tr>");
                SearchSection.Append("<td width=\"5%\"><label id=\"pageSectionTitle\">" + per_globalsearch_label_search + " </label>");
                SearchSection.Append("</td>");
                SearchSection.Append("<td width=\"85%\"><input type=\"text\" id=\"globalSearchTextInput\"; id=\"GlobalSearchPattern\" name=\"GlobalSearchPattern\" value=\"" + globalSearchPattern + "\"/>");
                SearchSection.Append("</td>");
                SearchSection.Append("<td width=\"5%\"><input type=\"image\" src=\"img/system-search.png\"/>");
                SearchSection.Append("</td>");
                SearchSection.Append("</tr>");

                if (SearchMessage.Length != 0)
                {
                    SearchSection.Append("<tr><td id=\"pageText\" colspan=\"3\">");
                    SearchSection.Append(SearchMessage.ToString());
                    SearchSection.Append("</td></tr>");
                    SearchMessage = new StringBuilder();
                }

                if (Session["AMCA_GlobalSearchQueries"] != null)
                {
                    SearchSection.Append("<tr>");
                    SearchSection.Append("<td colspan=\"3\" style=\"text-align:right; \">");
                    SearchSection.Append("<a class=\"pageTextLink\" href=\"GlobalSearch.History.aspx?State=" + State + "\">");
                    SearchSection.Append("Recent Searches");
                    SearchSection.Append("</a>");
                    SearchSection.Append("</td>");
                    SearchSection.Append("<td></td></tr>");
                }

                SearchSection.Append("</table>");
                SearchSection.Append("</div>");
            }
        }
        //Clear session variables.
        //This session variable is used by the AdvancedSearch.Results.aspx.cs
        //to store the current datatable that stores the search results. This
        //helps improve performance by re-using this table rather than rebuilding
        //the table each time the user pages through or returns to the list.
        if (Session["AMCA_CapList_DataTable"] != null)
        {
            Session.Remove("AMCA_CapList_DataTable");
        }
        //same as above only this is used by the GlobalSearch.List.aspx.cs.
        Session["AMCA_GlobalSearch_Mode"] = "false";
        if (Session["AMCA_MyCollection_DataTable"] != null)
        {
            Session.Remove("AMCA_MyCollection_DataTable");
        }
        //same as above only this is used by the MyCollections.List.aspx.cs.
        if (Session["AMCA_MyCollection_DataTable"] != null)
        {
            Session.Remove("AMCA_MyCollection_DataTable");
        }
        //This setting is used to store the AdvancedSearch.Results.aspx.cs
        //results list name.  This is the user defined name defined in AA
        //for the module name: ie: License = "Trade Name Management".
        //This may include HTML fomatting.
        if (Session["AMCA_AdvancedSearch_Result_List_Name"] != null)
        {
            Session.Remove("AMCA_AdvancedSearch_Result_List_Name");
        }
        //This setting is used to store the AdvancedSearch.Results.aspx.cs
        //results base module name that was used to read the list name 
        //that is descibed above.  This setting is used to determine
        //when the user uses breadcrumbs or browser back/forward buttons 
        //to select a previous list of result that were for a differnt 
        //module name.
        if (Session["AMCA_AdvancedSearch_Result_List_BaseModule"] != null)
        {
            Session.Remove("AMCA_AdvancedSearch_Result_List_BaseModule");
        }
        MyCollectionProxy myCollectionProxy = new MyCollectionProxy();
        int collectionCnt = myCollectionProxy.getMyCollectionsCount();
        if (collectionCnt != 0)
        {
            if (isiPhone == true)
            {
                // CollectionsSection.Append("<div id=\"pageListSingleRow\">");
                // CollectionsSection.Append("</div>");
            }
            else
            {
                CollectionsSection.Append("<table border=\"0\" cell-padding=\"0\" cell-spacing=\"0\" width=\"100%\">");
                CollectionsSection.Append("<tr><td class=\"pageListLink\">");
                CollectionsSection.Append("<a class=\"pageListLink\" href=\"MyCollections.aspx?State=" + State);
                CollectionsSection.Append("&BreadCrumbIndex=2"); //Added for 7.1 for browser back button  issues.               
                CollectionsSection.Append("\">" + "View " + mycollection_managepage_label_name + " (" + collectionCnt.ToString() + ")");
                CollectionsSection.Append("</a>");
                CollectionsSection.Append("</td></tr></table>");
            }
        }

        // Clear the breadcrumb list and initialize it back to "Home".
        if (Session["AMCA_BreadCrumbList"] != null)
        {
            Session.Remove("AMCA_BreadCrumbList");
        }
        if (isiPhone == true)
        {
            iPhonePageTitle = aca_sys_default_home;
            string initHeaders = BackLinkHelper("0");
        }
        StringBuilder sbWork = new StringBuilder();
        sbWork.Append("&Module=" + ModuleName);
        sbWork = BreadCrumbHelper("Login.Home.aspx", sbWork, "Home", string.Empty, false, false, false, false);

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
}

